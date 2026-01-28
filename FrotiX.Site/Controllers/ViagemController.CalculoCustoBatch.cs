/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemController.CalculoCustoBatch.cs                            ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Viagem API (Partial - CalculoCustoBatch)
     * 🎯 OBJETIVO: Cálculo otimizado de custos em BATCH para milhares de viagens
     * 📋 ROTAS:
     *    - /api/Viagem/ExecutarCalculoCustoBatch [POST]
     *    - /api/Viagem/ObterProgressoCalculoCustoBatch [GET]
     *    - /api/Viagem/LimparProgressoCalculoCustoBatch [POST]
     * 🔗 ENTIDADES: Viagem, Veiculo, Motorista, Abastecimento, Setor, ViewViagens
     * 📦 DEPENDÊNCIAS: IUnitOfWork, ApplicationDbContext, IMemoryCache
     * ⚡ PERFORMANCE: Processa em lotes de 500, evita timeout (milhares de registros)
     * 🗑️ CACHE: Carrega TODOS os dados necessários UMA VEZ em memória (DadosCalculoCache)
     * 📊 ALGORITMO: 3 etapas - (1) Carregar dados, (2) Processar batches, (3) Salvar
     * 💰 CUSTOS: CustoCombustivel, CustoVeiculo, CustoMotorista, CustoOperador, CustoLavador
     * 📝 NOTA: Classe parcial - ver ViagemController.cs principal
     ****************************************************************************************/
    public partial class ViagemController : Controller
    {
        // [DOC] =============================================
        // [DOC] CLASSE DE CACHE PARA DADOS COMPARTILHADOS
        // [DOC] =============================================

        /****************************************************************************************
         * 📦 CLASSE: DadosCalculoCache
         * 🎯 OBJETIVO: Cache em memória de TODOS os dados necessários para cálculo de custos
         * 📋 PROPRIEDADES:
         *    - TodasDatasViagens: Lista de todas as datas com viagens (para cálculo de médias)
         *    - CacheMediasPorMes: Médias de viagens por mês (string yyyy-MM)
         *    - ValoresVeiculos: Valor depreciado de cada veículo (Guid → double)
         *    - ConsumosVeiculos: Consumo médio de cada veículo (Guid → double km/L)
         *    - CombustiveisVeiculos: Tipo de combustível por veículo (Guid → CombustivelId)
         *    - ValoresCombustivel: Preço por tipo de combustível (Guid → double R$/L)
         *    - MediasCombustivel: Média de preço por tipo de combustível (Guid → double)
         *    - InfoMotoristas: Informações de motoristas (Guid → MotoristaInfo)
         * 🗑️ CACHE: Carregado UMA VEZ no início do batch (evita N+1 queries)
         * ⚡ PERFORMANCE: Substitui milhares de queries individuais por lookups em memória
         ****************************************************************************************/
        private class DadosCalculoCache
        {
            public List<DateTime> TodasDatasViagens { get; set; } = new List<DateTime>();
            public Dictionary<string, double> CacheMediasPorMes { get; set; } = new Dictionary<string, double>();
            public Dictionary<Guid, double> ValoresVeiculos { get; set; } = new Dictionary<Guid, double>();
            public Dictionary<Guid, double> ConsumosVeiculos { get; set; } = new Dictionary<Guid, double>();
            public Dictionary<Guid, Guid?> CombustiveisVeiculos { get; set; } = new Dictionary<Guid, Guid?>();
            public Dictionary<Guid, double> ValoresCombustivel { get; set; } = new Dictionary<Guid, double>();
            public Dictionary<Guid?, double> MediasCombustivel { get; set; } = new Dictionary<Guid?, double>();
            public Dictionary<Guid, MotoristaInfo> InfoMotoristas { get; set; } = new Dictionary<Guid, MotoristaInfo>();
        }

        /****************************************************************************************
         * 📦 CLASSE: MotoristaInfo
         * 🎯 OBJETIVO: Armazenar informações do motorista para cálculo de custo
         * 📋 PROPRIEDADES:
         *    - EhTerceirizado: Se motorista é terceirizado (true) ou efetivo (false)
         *    - ValorMotorista: Custo horário do motorista (R$/h)
         ****************************************************************************************/
        private class MotoristaInfo
        {
            public bool EhTerceirizado { get; set; }
            public double ValorMotorista { get; set; }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ExecutarCalculoCustoBatch
         * 🎯 OBJETIVO: Calcular custos de TODAS as viagens realizadas em batch otimizado
         * 📥 ENTRADAS: Nenhuma (processa todas as viagens com status "Realizada")
         * 📤 SAÍDAS: JSON { success, message, totalViagens, tempoSegundos, tempoFormatado }
         * 🔗 CHAMADA POR: Interface de administração (botão "Recalcular Custos")
         * 🔄 CHAMA: CarregarDadosCalculoCache(), CalcularCustosViagem() para cada viagem
         * ⚡ PERFORMANCE: Processa em batches de 500 viagens (evita timeout em milhares de registros)
         * 📊 ALGORITMO (3 etapas):
         *    1. Carregar TODOS os dados necessários UMA VEZ em memória (DadosCalculoCache)
         *    2. Processar viagens em batches de 500 (Skip/Take)
         *    3. SaveChanges a cada batch (commit incremental)
         * 🗑️ CACHE: Armazena progresso em "CalculoCusto_Progresso" (30 min)
         * 💰 CUSTOS CALCULADOS: CustoCombustivel, CustoVeiculo, CustoMotorista, CustoOperador, CustoLavador
         ****************************************************************************************/
        [HttpPost]
        [Route("ExecutarCalculoCustoBatch")]
        public async Task<IActionResult> ExecutarCalculoCustoBatch()
        {
            var stopwatch = Stopwatch.StartNew();
            string chaveProgresso = "CalculoCusto_Progresso";

            Console.WriteLine("==========================================================");
            Console.WriteLine(">>> INICIANDO CÁLCULO DE CUSTOS EM BATCH <<<");
            Console.WriteLine("==========================================================");

            try
            {
                // [DOC] Limpa progresso anterior (evita confusão com execução anterior)
                _cache.Remove(chaveProgresso);

                // [DOC] ========== ETAPA 1: Carregar TODOS os dados necessários UMA VEZ em memória ==========
                // [DOC] Esta é a otimização-chave: ao invés de fazer milhares de queries individuais,
                // [DOC] carregamos TUDO de uma vez (veículos, motoristas, combustíveis, médias)
                AtualizarProgresso(chaveProgresso, 0, 0, "Carregando dados em cache...", false, null);
                var cache = await CarregarDadosCalculoCache();

                // [DOC] ========== ETAPA 2: Buscar viagens que precisam ser processadas ==========
                // [DOC] Filtra apenas viagens "Realizada" com todos os campos obrigatórios preenchidos
                AtualizarProgresso(chaveProgresso, 0, 0, "Buscando viagens para processar...", false, null);
                var viagensParaProcessar = _unitOfWork.ViewViagens.GetAll()
                    .Where(v => v.Status == "Realizada"
                        && v.DataInicial != null
                        && v.DataFinal != null
                        && v.HoraInicio != null
                        && v.HoraFim != null
                        && v.KmInicial != null
                        && v.KmFinal != null
                        && v.VeiculoId != null
                        && v.MotoristaId != null)
                    .Select(v => new
                    {
                        v.ViagemId,
                        v.VeiculoId,
                        v.MotoristaId,
                        v.DataInicial,
                        v.DataFinal,
                        v.HoraInicio,
                        v.HoraFim,
                        v.KmInicial,
                        v.KmFinal
                    })
                    .ToList();

                int totalViagens = viagensParaProcessar.Count;
                int processados = 0;

                // [DOC] ========== ETAPA 3: Processar em BATCHES de 500 registros ==========
                // [DOC] Batch processing evita timeout em grandes volumes (milhares de viagens)
                // [DOC] Processa 500 viagens → SaveChanges → próximas 500 → SaveChanges...
                const int BATCH_SIZE = 500;

                for (int i = 0; i < totalViagens; i += BATCH_SIZE)
                {
                    var batch = viagensParaProcessar.Skip(i).Take(BATCH_SIZE).ToList();
                    var viagemIds = batch.Select(v => v.ViagemId).ToList();

                    // [DOC] Carrega entidades completas do batch COM TRACKING (EF rastreia mudanças)
                    var viagensEntidades = await _context.Viagem
                        .AsTracking()
                        .Where(v => viagemIds.Contains(v.ViagemId))
                        .ToListAsync();

                    Console.WriteLine($"\n=== BATCH {i / BATCH_SIZE + 1}: Carregadas {viagensEntidades.Count} viagens ===");

                    // [DOC] Loop de processamento: calcula custos para cada viagem do batch
                    foreach (var viagem in viagensEntidades)
                    {
                        try
                        {
                            // LOG ANTES do cálculo
                            var valorAntesCombustivel = viagem.CustoCombustivel;
                            var valorAntesVeiculo = viagem.CustoVeiculo;
                            var valorAntesMotorista = viagem.CustoMotorista;
                            var valorAntesOperador = viagem.CustoOperador;
                            var valorAntesLavador = viagem.CustoLavador;

                            // [DOC] Calcula TODOS os 5 custos usando dados do cache (sem queries adicionais)
                            // [DOC] Atualiza viagem em memória (EF tracking registra mudança)
                            CalcularCustosViagem(viagem, cache);

                            // LOG DEPOIS do cálculo (primeiras 5 viagens)
                            if (processados < 5)
                            {
                                Console.WriteLine($"\n>>> Viagem {viagem.ViagemId}:");
                                Console.WriteLine($"    ANTES:  Combustivel={valorAntesCombustivel}, Veiculo={valorAntesVeiculo}, Motorista={valorAntesMotorista}, Operador={valorAntesOperador}, Lavador={valorAntesLavador}");
                                Console.WriteLine($"    DEPOIS: Combustivel={viagem.CustoCombustivel:F2}, Veiculo={viagem.CustoVeiculo:F2}, Motorista={viagem.CustoMotorista:F2}, Operador={viagem.CustoOperador:F2}, Lavador={viagem.CustoLavador:F2}");
                                Console.WriteLine($"    Minutos={viagem.Minutos}, Média mensal={CalcularMediaViagensParaData(viagem.DataInicial.Value, cache):F2}");
                                Console.WriteLine($"    State: {_context.Entry(viagem).State}");
                            }

                            processados++;

                            // LOG A CADA 1000 VIAGENS
                            if (processados % 1000 == 0)
                            {
                                Console.WriteLine($">>> PROGRESSO: {processados:N0} / {totalViagens:N0} viagens processadas ({(processados * 100.0 / totalViagens):F1}%)");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao processar viagem {viagem.ViagemId}: {ex.Message}");
                        }
                    }

                    // [DOC] ===== Salva o batch completo =====
                    // [DOC] SaveChanges persiste TODAS as viagens modificadas do batch de uma vez
                    Console.WriteLine($"\n>>> Salvando batch {i / BATCH_SIZE + 1}...");

                    var entriesTracked = _context.ChangeTracker.Entries<Viagem>()
                        .Where(e => e.State == EntityState.Modified)
                        .Count();
                    Console.WriteLine($"    Entidades Modified: {entriesTracked}");

                    int mudancas = await _context.SaveChangesAsync();
                    Console.WriteLine($"=== SaveChanges: {mudancas} registros atualizados ===");

                    // [DOC] Atualiza progresso no cache para o frontend exibir barra de progresso
                    double percentual = (processados * 100.0) / totalViagens;
                    string mensagem = $"Processando {processados:N0} de {totalViagens:N0} viagens...";
                    AtualizarProgresso(chaveProgresso, processados, totalViagens, mensagem, false, null);

                    // Log de progresso a cada 10 batches
                    if (i % (BATCH_SIZE * 10) == 0)
                    {
                        Console.WriteLine($"Progresso: {processados}/{totalViagens} viagens ({percentual:F1}%)");
                    }
                }

                stopwatch.Stop();

                var response = new
                {
                    success = true,
                    message = $"✓ Cálculo concluído! {processados:N0} viagens atualizadas em {FormatarTempo(stopwatch.Elapsed)}",
                    totalViagens = processados,
                    tempoSegundos = stopwatch.Elapsed.TotalSeconds,
                    tempoFormatado = FormatarTempo(stopwatch.Elapsed)
                };

                // Marca progresso como concluído
                AtualizarProgresso(chaveProgresso, processados, totalViagens, response.message, true, null);

                return Json(response);
            }
            catch (Exception error)
            {
                stopwatch.Stop();
                string mensagemErro = "Erro ao executar cálculo de custos: " + error.Message;

                // Marca progresso como erro
                AtualizarProgresso(chaveProgresso, 0, 0, mensagemErro, true, mensagemErro);

                Alerta.TratamentoErroComLinha("ViagemController.cs", "ExecutarCalculoCustoBatch", error);
                return Json(new
                {
                    success = false,
                    message = mensagemErro,
                    tempoSegundos = stopwatch.Elapsed.TotalSeconds
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AtualizarProgresso (HELPER)
         * 🎯 OBJETIVO: Atualizar progresso do batch no cache (exibir barra no frontend)
         * 📥 ENTRADAS: chave, processado, total, mensagem, concluido, erro
         * 📤 SAÍDAS: void (atualiza cache)
         * 🗑️ CACHE: Armazena em chave especificada (30 min de expiração)
         * 📊 FORMATO: { processado, total, percentual, mensagem, concluido, erro }
         ****************************************************************************************/
        private void AtualizarProgresso(string chave, int processado, int total, string mensagem, bool concluido, string erro)
        {
            try
            {
                var progresso = new
                {
                    processado = processado,
                    total = total,
                    percentual = total > 0 ? (int)((processado * 100.0) / total) : 0,
                    mensagem = mensagem,
                    concluido = concluido,
                    erro = erro
                };

                _cache.Set(chave, progresso, TimeSpan.FromMinutes(30));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar progresso: {ex.Message}");
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ObterProgressoCalculoCustoBatch
         * 🎯 OBJETIVO: Consultar progresso atual do cálculo batch (polling do frontend)
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { success, progresso: { processado, total, percentual, mensagem, concluido, erro } }
         * 🔗 CHAMADA POR: Frontend (a cada X segundos para atualizar barra de progresso)
         * 🔄 CHAMA: IMemoryCache.TryGetValue()
         * 🗑️ CACHE: Lê "CalculoCusto_Progresso"
         ****************************************************************************************/
        [HttpGet]
        [Route("ObterProgressoCalculoCustoBatch")]
        public IActionResult ObterProgressoCalculoCustoBatch()
        {
            try
            {
                string chaveProgresso = "CalculoCusto_Progresso";

                if (_cache.TryGetValue(chaveProgresso, out object progresso))
                {
                    return Json(new
                    {
                        success = true,
                        progresso = progresso
                    });
                }

                return Json(new
                {
                    success = true,
                    progresso = new
                    {
                        processado = 0,
                        total = 0,
                        percentual = 0,
                        mensagem = "Nenhum processamento em andamento",
                        concluido = false,
                        erro = (string)null
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "ObterProgressoCalculoCustoBatch", error);
                return Json(new
                {
                    success = false,
                    message = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: LimparProgressoCalculoCustoBatch
         * 🎯 OBJETIVO: Limpar progresso do cache (resetar estado)
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { success }
         * 🔗 CHAMADA POR: Frontend (botão de reset ou após conclusão)
         * 🔄 CHAMA: IMemoryCache.Remove()
         * 🗑️ CACHE: Remove "CalculoCusto_Progresso"
         ****************************************************************************************/
        [HttpPost]
        [Route("LimparProgressoCalculoCustoBatch")]
        public IActionResult LimparProgressoCalculoCustoBatch()
        {
            try
            {
                string chaveProgresso = "CalculoCusto_Progresso";
                _cache.Remove(chaveProgresso);

                return Json(new { success = true });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "LimparProgressoCalculoCustoBatch", error);
                return Json(new { success = false, message = error.Message });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CarregarDadosCalculoCache (PRIVATE)
         * 🎯 OBJETIVO: Carregar TODOS os dados necessários UMA VEZ em memória (otimização-chave)
         * 📥 ENTRADAS: Nenhuma (acessa banco via _context)
         * 📤 SAÍDAS: DadosCalculoCache preenchido com todos os dados
         * 🔄 CHAMA: CarregarDadosVeiculosCache(), CarregarDadosMotoristasCache()
         * 📊 DADOS CARREGADOS:
         *    - TodasDatasViagens: Todas as datas únicas de viagens realizadas
         *    - CacheMediasPorMes: Médias de viagens por mês (yyyy-MM → média)
         *    - ValoresVeiculos, ConsumosVeiculos, CombustiveisVeiculos (via helper)
         *    - ValoresCombustivel, MediasCombustivel (preços atuais e médias)
         *    - InfoMotoristas (via helper)
         * ⚡ PERFORMANCE: 1 query ao invés de N queries dentro do loop de viagens
         ****************************************************************************************/
        private async Task<DadosCalculoCache> CarregarDadosCalculoCache()
        {
            var cache = new DadosCalculoCache();

            // 1. CARREGAR TODAS AS DATAS DE VIAGENS REALIZADAS (para cálculo dinâmico de média)
            cache.TodasDatasViagens = _unitOfWork.ViewViagens.GetAll()
                .Where(v => v.DataInicial != null && v.Status == "Realizada")
                .Select(v => v.DataInicial.Value)
                .ToList();

            cache.TodasDatasViagens.Sort();

            // 2. DADOS DE VEÍCULOS (carregados UMA VEZ)
            await CarregarDadosVeiculosCache(cache);

            // 3. ÚLTIMOS VALORES DE COMBUSTÍVEL POR VEÍCULO (carregados UMA VEZ)
            var valoresCombustivel = await _context.Abastecimento
                .GroupBy(a => a.VeiculoId)
                .Select(g => new
                {
                    VeiculoId = g.Key,
                    Abastecimento = g.OrderByDescending(a => a.DataHora).FirstOrDefault()
                })
                .ToListAsync();

            foreach (var vc in valoresCombustivel)
            {
                if (vc.VeiculoId != Guid.Empty && vc.Abastecimento != null && vc.Abastecimento.ValorUnitario > 0)
                {
                    cache.ValoresCombustivel[vc.VeiculoId] = (double)vc.Abastecimento.ValorUnitario;
                }
            }

            // 4. MÉDIA DE COMBUSTÍVEL POR TIPO (carregada UMA VEZ)
            var mediasCombustivel = await _context.MediaCombustivel
                .GroupBy(mc => mc.CombustivelId)
                .Select(g => new
                {
                    CombustivelId = g.Key,
                    MediaCombustivel = g.OrderByDescending(mc => mc.Ano).ThenByDescending(mc => mc.Mes).FirstOrDefault()
                })
                .ToListAsync();

            foreach (var mc in mediasCombustivel)
            {
                if (mc.MediaCombustivel != null && mc.MediaCombustivel.PrecoMedio > 0)
                {
                    cache.MediasCombustivel[mc.CombustivelId] = (double)mc.MediaCombustivel.PrecoMedio;
                }
            }

            // 5. DADOS DE MOTORISTAS (carregados UMA VEZ)
            await CarregarDadosMotoristasCache(cache);

            return cache;
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CarregarDadosVeiculosCache (PRIVATE)
         * 🎯 OBJETIVO: Carregar dados de veículos (valor, consumo, combustível) para o cache
         * 📥 ENTRADAS: cache (DadosCalculoCache para preencher)
         * 📤 SAÍDAS: void (preenche cache.ValoresVeiculos, ConsumosVeiculos, CombustiveisVeiculos)
         * 🔗 CHAMADA POR: CarregarDadosCalculoCache()
         * 📊 DADOS CARREGADOS:
         *    - ValoresVeiculos: Valor unitário depreciado de cada veículo (ItemVeiculoContrato)
         *    - ConsumosVeiculos: Consumo médio (km/L) de cada veículo
         *    - CombustiveisVeiculos: Tipo de combustível de cada veículo
         * 🔄 JOINS: Veiculo → ItemVeiculoContrato → RepactuacaoContrato
         ****************************************************************************************/
        private async Task CarregarDadosVeiculosCache(DadosCalculoCache cache)
        {
            // Busca veículos com contratos
            var veiculosContrato = await (
                from v in _context.Veiculo
                where v.ContratoId != null && v.ItemVeiculoId != null
                select new
                {
                    v.VeiculoId,
                    v.ItemVeiculoId,
                    v.CombustivelId
                }
            ).ToListAsync();

            var itemIds = veiculosContrato.Where(v => v.ItemVeiculoId.HasValue).Select(v => v.ItemVeiculoId.Value).ToList();

            var valoresContrato = await (
                from ivc in _context.ItemVeiculoContrato
                join rc in _context.RepactuacaoContrato on ivc.RepactuacaoContratoId equals rc.RepactuacaoContratoId
                where itemIds.Contains(ivc.ItemVeiculoId)
                group new { ivc, rc } by ivc.ItemVeiculoId into g
                select new
                {
                    ItemVeiculoId = g.Key,
                    ItemContrato = g.OrderByDescending(x => x.rc.DataRepactuacao).FirstOrDefault()
                }
            ).ToListAsync();

            var dictValoresContrato = valoresContrato
                .Where(v => v.ItemContrato != null)
                .ToDictionary(v => v.ItemVeiculoId, v => (double)(v.ItemContrato.ivc.ValorUnitario ?? 0));

            foreach (var v in veiculosContrato)
            {
                if (v.ItemVeiculoId.HasValue && dictValoresContrato.ContainsKey(v.ItemVeiculoId.Value))
                {
                    cache.ValoresVeiculos[v.VeiculoId] = dictValoresContrato[v.ItemVeiculoId.Value];
                }
                cache.CombustiveisVeiculos[v.VeiculoId] = v.CombustivelId;
            }

            // Busca veículos com atas
            var veiculosAta = await (
                from v in _context.Veiculo
                where v.AtaId != null && v.ItemVeiculoAtaId != null
                select new
                {
                    v.VeiculoId,
                    v.ItemVeiculoAtaId,
                    v.CombustivelId
                }
            ).ToListAsync();

            var ataItemIds = veiculosAta.Where(v => v.ItemVeiculoAtaId.HasValue).Select(v => v.ItemVeiculoAtaId.Value).ToList();

            var valoresAta = await (
                from iva in _context.ItemVeiculoAta
                join ra in _context.RepactuacaoAta on iva.RepactuacaoAtaId equals ra.RepactuacaoAtaId
                where ataItemIds.Contains(iva.ItemVeiculoAtaId)
                group new { iva, ra } by iva.ItemVeiculoAtaId into g
                select new
                {
                    ItemVeiculoAtaId = g.Key,
                    ItemAta = g.OrderByDescending(x => x.ra.DataRepactuacao).FirstOrDefault()
                }
            ).ToListAsync();

            var dictValoresAta = valoresAta
                .Where(v => v.ItemAta != null)
                .ToDictionary(v => v.ItemVeiculoAtaId, v => (double)(v.ItemAta.iva.ValorUnitario ?? 0));

            foreach (var v in veiculosAta)
            {
                if (v.ItemVeiculoAtaId.HasValue && dictValoresAta.ContainsKey(v.ItemVeiculoAtaId.Value))
                {
                    cache.ValoresVeiculos[v.VeiculoId] = dictValoresAta[v.ItemVeiculoAtaId.Value];
                }
                cache.CombustiveisVeiculos[v.VeiculoId] = v.CombustivelId;
            }

            // Busca veículos próprios (sem contrato/ata)
            var veiculosProprios = await _context.Veiculo
                .Where(v => v.ContratoId == null && v.AtaId == null)
                .Select(v => new { v.VeiculoId, v.CombustivelId })
                .ToListAsync();

            foreach (var v in veiculosProprios)
            {
                if (!cache.ValoresVeiculos.ContainsKey(v.VeiculoId))
                {
                    cache.ValoresVeiculos[v.VeiculoId] = 100; // Veículo próprio
                }
                cache.CombustiveisVeiculos[v.VeiculoId] = v.CombustivelId;
            }

            // Busca consumos de veículos da ViewVeiculos
            var todosVeiculos = _unitOfWork.ViewVeiculos.GetAll().ToList();
            var consumos = todosVeiculos.Select(v => new { v.VeiculoId, v.Consumo }).ToList();

            foreach (var c in consumos)
            {
                double consumo = c.Consumo.HasValue ? Convert.ToDouble(c.Consumo.Value) : 10;
                if (consumo == 0)
                    consumo = 10;
                cache.ConsumosVeiculos[c.VeiculoId] = consumo;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CarregarDadosMotoristasCache (PRIVATE)
         * 🎯 OBJETIVO: Carregar dados de motoristas (terceirizado + custo) para o cache
         * 📥 ENTRADAS: cache (DadosCalculoCache para preencher)
         * 📤 SAÍDAS: void (preenche cache.InfoMotoristas)
         * 🔗 CHAMADA POR: CarregarDadosCalculoCache()
         * 📊 DADOS CARREGADOS:
         *    - InfoMotoristas: Dictionary<Guid, MotoristaInfo> com:
         *      - EhTerceirizado: true se ContratoId != null
         *      - ValorMotorista: Custo mensal do motorista (Contrato.CustoMensalMotorista)
         * 🔄 JOIN: Motorista LEFT JOIN Contrato
         * 📝 NOTA: Motoristas sem contrato = efetivos (ValorMotorista = 0)
         ****************************************************************************************/
        private async Task CarregarDadosMotoristasCache(DadosCalculoCache cache)
        {
            // Busca motoristas com seus contratos e o CustoMensalMotorista
            var motoristasComContrato = await (
                from m in _context.Motorista
                join c in _context.Contrato on m.ContratoId equals c.ContratoId into contratoJoin
                from contrato in contratoJoin.DefaultIfEmpty()
                select new
                {
                    m.MotoristaId,
                    m.ContratoId,
                    CustoMensalMotorista = contrato != null ? contrato.CustoMensalMotorista : null
                }
            ).ToListAsync();

            foreach (var m in motoristasComContrato)
            {
                if (m.ContratoId == null)
                {
                    // Motorista não é terceirizado
                    cache.InfoMotoristas[m.MotoristaId] = new MotoristaInfo
                    {
                        EhTerceirizado = false,
                        ValorMotorista = 0
                    };
                }
                else
                {
                    // Motorista terceirizado - usa CustoMensalMotorista do Contrato
                    cache.InfoMotoristas[m.MotoristaId] = new MotoristaInfo
                    {
                        EhTerceirizado = true,
                        ValorMotorista = (double)(m.CustoMensalMotorista ?? 0)
                    };
                }
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularCustosViagem (PRIVATE)
         * 🎯 OBJETIVO: Calcular TODOS os 5 custos de uma viagem usando dados em cache
         * 📥 ENTRADAS: viagem (entidade Viagem), cache (DadosCalculoCache com todos os dados)
         * 📤 SAÍDAS: void (atualiza viagem em memória - EF tracking registra mudança)
         * 🔗 CHAMADA POR: ExecutarCalculoCustoBatch() (loop de batch)
         * 🔄 CHAMA: CalcularMediaViagensParaData(), CalcularCusto*Cache() (5 helpers)
         * 💰 CUSTOS CALCULADOS:
         *    1. CustoCombustivel (Km rodado × preço/L ÷ consumo)
         *    2. CustoVeiculo (Minutos × valor depreciado ÷ 43800 min/mês)
         *    3. CustoMotorista (Minutos × custo mensal ÷ 9600 min/mês)
         *    4. CustoOperador (Dinâmico baseado na média de viagens do mês)
         *    5. CustoLavador (Dinâmico baseado na média de viagens do mês)
         * ⚡ PERFORMANCE: Nenhuma query ao banco (usa apenas dados do cache em memória)
         ****************************************************************************************/
        private void CalcularCustosViagem(Viagem viagem, DadosCalculoCache cache)
        {
            try
            {
                bool modificou = false;

                // CALCULA MÉDIA DOS ÚLTIMOS 12 MESES PARA ESTA VIAGEM ESPECÍFICA
                double mediaViagens = CalcularMediaViagensParaData(viagem.DataInicial.Value, cache);

                // 1. CUSTO COMBUSTÍVEL
                double custoCombustivel = CalcularCustoCombustivelCache(viagem, cache);
                if (custoCombustivel > 0)
                {
                    viagem.CustoCombustivel = custoCombustivel;
                    modificou = true;
                }

                // 2. CUSTO VEÍCULO
                double custoVeiculo = CalcularCustoVeiculoCache(viagem, cache);
                if (custoVeiculo > 0)
                {
                    viagem.CustoVeiculo = custoVeiculo;
                    modificou = true;
                }

                // 3. CUSTO MOTORISTA (Minutos já calculados pelo trigger no banco)
                double custoMotorista = CalcularCustoMotoristaCache(viagem, cache);
                if (custoMotorista >= 0)
                {
                    viagem.CustoMotorista = custoMotorista;
                    modificou = true;
                }

                // 4. CUSTO OPERADOR (calculado dinamicamente com a média desta viagem)
                double custoOperador = CalcularCustoOperadorDinamico(mediaViagens);
                if (custoOperador > 0)
                {
                    viagem.CustoOperador = custoOperador;
                    modificou = true;
                }

                // 5. CUSTO LAVADOR
                double custoLavador = CalcularCustoLavadorDinamico(mediaViagens);
                if (custoLavador > 0)
                {
                    viagem.CustoLavador = custoLavador;
                    modificou = true;
                }
                else
                {
                    Console.WriteLine($"  [AVISO] Viagem {viagem.ViagemId}: CustoLavador = 0 (média: {mediaViagens:F2})");
                }

                if (!modificou)
                {
                    Console.WriteLine($"  [ERRO] ⚠️ Viagem {viagem.ViagemId}: NENHUM valor foi modificado!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [ERRO] ❌ Viagem {viagem.ViagemId}: {ex.Message}");
                throw;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularMediaViagensParaData (PRIVATE)
         * 🎯 OBJETIVO: Calcular média MENSAL de viagens realizadas ANTES da data especificada
         * 📥 ENTRADAS: dataViagem (data da viagem), cache (com TodasDatasViagens)
         * 📤 SAÍDAS: double (média mensal de viagens - min 0.1)
         * 🔗 CHAMADA POR: CalcularCustosViagem()
         * 📊 ALGORITMO:
         *    1. Filtra viagens anteriores à dataViagem
         *    2. Calcula total de dias entre primeira viagem e dataViagem
         *    3. Calcula média diária: totalViagens / totalDias
         *    4. Converte para média mensal: mediaDiaria × 30
         * 🗑️ CACHE: Armazena resultado em cache.CacheMediasPorMes (evita recálculo)
         * 📝 NOTA: Mínimo de 0.1 (evita divisão por zero em custos dinâmicos)
         ****************************************************************************************/
        private double CalcularMediaViagensParaData(DateTime dataViagem, DadosCalculoCache cache)
        {
            try
            {
                string chaveData = dataViagem.ToString("yyyy-MM-dd");

                if (cache.CacheMediasPorMes.ContainsKey(chaveData))
                {
                    return cache.CacheMediasPorMes[chaveData];
                }

                var viagensAnteriores = cache.TodasDatasViagens
                    .Where(d => d < dataViagem)
                    .ToList();

                int totalViagens = viagensAnteriores.Count;

                if (totalViagens == 0)
                {
                    cache.CacheMediasPorMes[chaveData] = 1.0;
                    return 1.0;
                }

                DateTime primeiraViagem = viagensAnteriores.Min();
                int totalDias = (dataViagem.Date - primeiraViagem.Date).Days;

                if (totalDias <= 0)
                    totalDias = 1;

                double mediaDiaria = (double)totalViagens / (double)totalDias;
                double mediaMensal = mediaDiaria * 30.0;
                mediaMensal = Math.Max(mediaMensal, 0.1);

                cache.CacheMediasPorMes[chaveData] = mediaMensal;

                return mediaMensal;
            }
            catch
            {
                return 1.0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularCustoOperadorDinamico (PRIVATE)
         * 🎯 OBJETIVO: Calcular custo de operador dinamicamente baseado na média de viagens
         * 📥 ENTRADAS: mediaViagens (média mensal de viagens)
         * 📤 SAÍDAS: double (custo por viagem = custoMensalTotal / mediaViagens)
         * 🔗 CHAMADA POR: CalcularCustosViagem()
         * 📊 ALGORITMO:
         *    1. Busca contrato de "Terceirização" com ContratoOperadores = true
         *    2. Busca última repactuação com QtdOperadores e ValorOperador
         *    3. Calcula custo mensal total: QtdOperadores × ValorOperador
         *    4. Divide pela média mensal de viagens
         * 📝 NOTA: Custo dinâmico - quanto mais viagens, menor o custo por viagem
         ****************************************************************************************/
        private double CalcularCustoOperadorDinamico(double mediaViagens)
        {
            try
            {
                if (mediaViagens == 0)
                    return 0;

                var contratoOperadores = _context.Contrato
                    .Where(c => c.TipoContrato == "Terceirização" && c.ContratoOperadores == true)
                    .OrderByDescending(c => c.DataInicio)
                    .FirstOrDefault();

                if (contratoOperadores == null)
                    return 0;

                var dadosOperador = (
                    from rc in _context.RepactuacaoContrato
                    join rt in _context.RepactuacaoTerceirizacao on rc.RepactuacaoContratoId equals rt.RepactuacaoContratoId
                    where rc.ContratoId == contratoOperadores.ContratoId
                        && rt.QtdOperadores != null
                        && rt.ValorOperador != null
                    orderby rc.DataRepactuacao descending
                    select new { rt.QtdOperadores, rt.ValorOperador }
                ).FirstOrDefault();

                if (dadosOperador == null)
                    return 0;

                double custoMensalOperadores = (double)(dadosOperador.QtdOperadores.Value * dadosOperador.ValorOperador.Value);
                return custoMensalOperadores / mediaViagens;
            }
            catch
            {
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularCustoLavadorDinamico (PRIVATE)
         * 🎯 OBJETIVO: Calcular custo de lavador dinamicamente baseado na média de viagens
         * 📥 ENTRADAS: mediaViagens (média mensal de viagens)
         * 📤 SAÍDAS: double (custo por viagem = custoMensalTotal / mediaViagens)
         * 🔗 CHAMADA POR: CalcularCustosViagem()
         * 📊 ALGORITMO:
         *    1. Busca contrato de "Terceirização" com ContratoLavadores = true
         *    2. Busca última repactuação com QtdLavadores e ValorLavador
         *    3. Calcula custo mensal total: QtdLavadores × ValorLavador
         *    4. Divide pela média mensal de viagens
         * 📝 NOTA: Custo dinâmico - quanto mais viagens, menor o custo por viagem
         ****************************************************************************************/
        private double CalcularCustoLavadorDinamico(double mediaViagens)
        {
            try
            {
                if (mediaViagens == 0)
                {
                    Console.WriteLine($"    [LAVADOR] ⚠️ Média = 0, retornando 0");
                    return 0;
                }

                var contratoLavadores = _context.Contrato
                    .Where(c => c.TipoContrato == "Terceirização" && c.ContratoLavadores == true)
                    .OrderByDescending(c => c.DataInicio)
                    .FirstOrDefault();

                if (contratoLavadores == null)
                {
                    Console.WriteLine($"    [LAVADOR] ❌ Nenhum contrato de lavadores encontrado");
                    return 0;
                }

                var dadosLavador = (
                    from rc in _context.RepactuacaoContrato
                    join rt in _context.RepactuacaoTerceirizacao on rc.RepactuacaoContratoId equals rt.RepactuacaoContratoId
                    where rc.ContratoId == contratoLavadores.ContratoId
                        && rt.QtdLavadores != null
                        && rt.ValorLavador != null
                    orderby rc.DataRepactuacao descending
                    select new { rt.QtdLavadores, rt.ValorLavador }
                ).FirstOrDefault();

                if (dadosLavador == null)
                {
                    Console.WriteLine($"    [LAVADOR] ❌ Nenhuma repactuação encontrada para o contrato");
                    return 0;
                }

                double custoMensalLavadores = (double)(dadosLavador.QtdLavadores.Value * dadosLavador.ValorLavador.Value);
                double custoFinal = custoMensalLavadores / mediaViagens;

                return custoFinal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    [LAVADOR] ❌ ERRO: {ex.Message}");
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularCustoCombustivelCache (PRIVATE)
         * 🎯 OBJETIVO: Calcular custo de combustível usando dados do cache
         * 📥 ENTRADAS: viagem (entidade Viagem), cache (DadosCalculoCache)
         * 📤 SAÍDAS: double (custo de combustível em R$)
         * 🔗 CHAMADA POR: CalcularCustosViagem()
         * 📊 ALGORITMO:
         *    1. Busca consumo do veículo no cache (default: 10 km/L)
         *    2. Busca preço do combustível (ValoresCombustivel ou MediasCombustivel)
         *    3. Calcula litros consumidos: KmRodado / consumo
         *    4. Custo = litros × precoCombustível
         * 🗑️ CACHE: ConsumosVeiculos, ValoresCombustivel, MediasCombustivel
         ****************************************************************************************/
        private double CalcularCustoCombustivelCache(Viagem viagem, DadosCalculoCache cache)
        {
            try
            {
                if (!viagem.VeiculoId.HasValue)
                    return 0;

                double consumo = cache.ConsumosVeiculos.ContainsKey(viagem.VeiculoId.Value)
                    ? cache.ConsumosVeiculos[viagem.VeiculoId.Value]
                    : 10;

                double valorCombustivel = 0;

                if (cache.ValoresCombustivel.ContainsKey(viagem.VeiculoId.Value))
                {
                    valorCombustivel = cache.ValoresCombustivel[viagem.VeiculoId.Value];
                }
                else
                {
                    var combustivelId = cache.CombustiveisVeiculos.ContainsKey(viagem.VeiculoId.Value)
                        ? cache.CombustiveisVeiculos[viagem.VeiculoId.Value]
                        : null;

                    if (combustivelId.HasValue && cache.MediasCombustivel.ContainsKey(combustivelId))
                    {
                        valorCombustivel = cache.MediasCombustivel[combustivelId];
                    }
                }

                var quilometragem = viagem.KmFinal - viagem.KmInicial;
                var custoViagem = (quilometragem / consumo) * valorCombustivel;

                return (double)custoViagem;
            }
            catch
            {
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularCustoVeiculoCache (PRIVATE)
         * 🎯 OBJETIVO: Calcular custo de veículo (depreciação) usando dados do cache
         * 📥 ENTRADAS: viagem (entidade Viagem), cache (DadosCalculoCache)
         * 📤 SAÍDAS: double (custo de veículo em R$)
         * 🔗 CHAMADA POR: CalcularCustosViagem()
         * 📊 ALGORITMO:
         *    - Fórmula: (ValorUnitario / 43200 minutos/mês) × Minutos da viagem
         *    - Teto mensal: não pode ultrapassar ValorUnitario
         * 🗑️ CACHE: ValoresVeiculos
         * 📝 NOTA: 43200 min/mês = 30 dias × 24 horas × 60 minutos
         ****************************************************************************************/
        private double CalcularCustoVeiculoCache(Viagem viagem, DadosCalculoCache cache)
        {
            try
            {
                if (!viagem.VeiculoId.HasValue)
                    return 0;

                double valorUnitario = cache.ValoresVeiculos.ContainsKey(viagem.VeiculoId.Value)
                    ? cache.ValoresVeiculos[viagem.VeiculoId.Value]
                    : 100;

                // Usa Minutos da viagem diretamente (calculado pelo trigger)
                int minutos = viagem.Minutos ?? 0;
                if (minutos <= 0)
                    return 0;

                // Fórmula: (ValorUnitarioItem / 30 / 24 / 60) × Minutos
                // = ValorUnitario / 43200 × Minutos
                const double MINUTOS_MES = 43200.0; // 30 dias × 24 horas × 60 minutos

                double custoCalculado = (valorUnitario / MINUTOS_MES) * minutos;

                // Teto: não pode ultrapassar o valor mensal do veículo
                return Math.Min(custoCalculado, valorUnitario);
            }
            catch
            {
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularCustoMotoristaCache (PRIVATE)
         * 🎯 OBJETIVO: Calcular custo de motorista usando dados do cache
         * 📥 ENTRADAS: viagem (entidade Viagem), cache (DadosCalculoCache)
         * 📤 SAÍDAS: double (custo de motorista em R$)
         * 🔗 CHAMADA POR: CalcularCustosViagem()
         * 📊 ALGORITMO:
         *    - Motorista efetivo: retorna 0 (sem custo adicional)
         *    - Motorista terceirizado: CustoMensalMotorista × (Minutos / 9600 min/mês)
         *    - Teto mensal: não pode ultrapassar CustoMensalMotorista
         * 🗑️ CACHE: InfoMotoristas (EhTerceirizado, ValorMotorista)
         * 📝 NOTA: 9600 min/mês = 220 horas × 60 minutos (jornada mensal aproximada)
         ****************************************************************************************/
        private double CalcularCustoMotoristaCache(Viagem viagem, DadosCalculoCache cache)
        {
            try
            {
                if (!viagem.MotoristaId.HasValue)
                    return 0;

                // Busca info do motorista no cache
                if (!cache.InfoMotoristas.TryGetValue(viagem.MotoristaId.Value, out var infoMotorista))
                    return 0;

                // Se não é terceirizado, custo = 0
                if (!infoMotorista.EhTerceirizado || infoMotorista.ValorMotorista <= 0)
                    return 0;

                // Usa Minutos da viagem diretamente (calculado pelo trigger)
                int minutos = viagem.Minutos ?? 0;
                if (minutos <= 0)
                    return 0;

                double custoMensalMotorista = infoMotorista.ValorMotorista;

                // Fórmula igual à SP: CustoMensalMotorista × (Minutos / 13200)
                // 13200 = 220 horas × 60 minutos (jornada mensal padrão)
                const double MINUTOS_MES = 13200.0;

                double custoCalculado = custoMensalMotorista * (minutos / MINUTOS_MES);

                // Teto: não pode ultrapassar o custo mensal do motorista
                return Math.Min(custoCalculado, custoMensalMotorista);
            }
            catch
            {
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: FormatarTempo (PRIVATE)
         * 🎯 OBJETIVO: Formatar TimeSpan para exibição amigável (horas/minutos/segundos)
         * 📥 ENTRADAS: tempo (TimeSpan)
         * 📤 SAÍDAS: string formatada ("Xh Ymin Zs", "Ymin Zs", ou "Zs")
         * 🔗 CHAMADA POR: ExecutarCalculoCustoBatch() (mensagem de conclusão)
         * 📊 FORMATO:
         *    - >= 1 hora: "Xh Ymin Zs"
         *    - >= 1 minuto: "Ymin Zs"
         *    - < 1 minuto: "Zs"
         ****************************************************************************************/
        private string FormatarTempo(TimeSpan tempo)
        {
            if (tempo.TotalHours >= 1)
                return $"{(int)tempo.TotalHours}h {tempo.Minutes}min {tempo.Seconds}s";
            else if (tempo.TotalMinutes >= 1)
                return $"{(int)tempo.TotalMinutes}min {tempo.Seconds}s";
            else
                return $"{tempo.Seconds}s";
        }
    }
}
