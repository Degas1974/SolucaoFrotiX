using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Services
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║                                                                              ║
    /// ║  📊 CLASSE: ViagemEstatisticaService                                         ║
    /// ║                                                                              ║
    /// ║  DESCRIÇÃO:                                                                  ║
    /// ║  Serviço de cálculo e cache de estatísticas agregadas de viagens.           ║
    /// ║  Otimiza consultas pesadas através de pré-processamento e armazenamento.    ║
    /// ║                                                                              ║
    /// ║  ESTRATÉGIA DE CACHE:                                                        ║
    /// ║  - Estatísticas são calculadas uma vez e armazenadas na tabela              ║
    /// ║    ViagemEstatistica (cache persistente).                                   ║
    /// ║  - Atualizações podem ser forçadas (recálculo) ou incrementais (update).    ║
    /// ║  - Consultas de período leem APENAS do cache (sem recálculo).               ║
    /// ║                                                                              ║
    /// ║  DADOS CALCULADOS:                                                           ║
    /// ║  1. Contadores: Total, Finalizadas, Em Andamento, Agendadas, Canceladas     ║
    /// ║  2. Custos: Total, Médio, Por Tipo (Veículo/Motorista/Operador/etc)        ║
    /// ║  3. Quilometragem: Total, Média por Viagem                                   ║
    /// ║  4. Rankings JSON: Top 10 Motoristas, Veículos, Setores, Requisitantes      ║
    /// ║  5. Distribuições JSON: Por Status, Por Finalidade, Por Custo               ║
    /// ║                                                                              ║
    /// ║  USO TÍPICO:                                                                 ║
    /// ║  - Dashboards e gráficos executivos.                                         ║
    /// ║  - Relatórios gerenciais com grandes volumes de dados.                       ║
    /// ║  - Análises comparativas de períodos (Mensal, Anual).                        ║
    /// ║                                                                              ║
    /// ║  ÚLTIMA ATUALIZAÇÃO: 19/01/2026                                              ║
    /// ║                                                                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    /// </summary>
    public class ViagemEstatisticaService
    {
        private readonly FrotiXDbContext _context;
        private readonly IViagemEstatisticaRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Construtor do Serviço de Estatísticas de Viagem.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ PARÂMETROS:
        /// │    -> context: DbContext para consultas diretas.
        /// │    -> repository: Repositório especializado em ViagemEstatistica.
        /// │    -> unitOfWork: Padrão Unit of Work para transações.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        public ViagemEstatisticaService(
            FrotiXDbContext context,
            IViagemEstatisticaRepository repository,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Obtém ou calcula estatísticas para uma data específica.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    SEMPRE recalcula e atualiza se o registro já existe no cache.
        /// │    Garante que os dados estejam sempre atualizados.
        /// │
        /// │ FLUXO DE EXECUÇÃO:
        /// │    1. Busca estatística existente no cache (tabela ViagemEstatistica).
        /// │    2. Calcula novas estatísticas (sempre, independente de cache).
        /// │    3. Se existe cache → Atualiza (UPDATE).
        /// │    4. Se não existe → Insere novo registro (INSERT).
        /// │
        /// │ CONCEITO DE CACHE SEMPRE ATUALIZADO:
        /// │    Este método NÃO confia no cache antigo. Sempre recalcula para garantir
        /// │    dados frescos. Use ObterEstatisticasPeriodoAsync() para leitura pura.
        /// │
        /// │ PARÂMETROS:
        /// │    -> data: Data de referência (será convertida para Date, ignorando hora).
        /// │
        /// │ RETORNO:
        /// │    -> Task<ViagemEstatistica>: Estatísticas recém-calculadas/atualizadas.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<ViagemEstatistica> ObterEstatisticasAsync(DateTime data)
        {
            try
            {
                // ─────────────────────────────────────────────────────────────────────────────
                // [ETAPA 1] NORMALIZAÇÃO: Remove hora, mantém apenas data
                // ─────────────────────────────────────────────────────────────────────────────
                var dataReferencia = data.Date;

                // ─────────────────────────────────────────────────────────────────────────────
                // [ETAPA 2] BUSCA NO CACHE: Verifica se já existe estatística calculada
                // ─────────────────────────────────────────────────────────────────────────────
                var estatisticaExistente = await _repository.ObterPorDataAsync(dataReferencia);

                // ─────────────────────────────────────────────────────────────────────────────
                // [ETAPA 3] RECÁLCULO: SEMPRE calcula novos dados (não confia em cache antigo)
                // ─────────────────────────────────────────────────────────────────────────────
                // IMPORTANTE: Mesmo que exista cache, recalcula para garantir dados atualizados.
                // Use caso: Após criar/editar/deletar viagem, precisa atualizar estatísticas.
                var novaEstatistica = await CalcularEstatisticasAsync(dataReferencia);

                // ─────────────────────────────────────────────────────────────────────────────
                // [ETAPA 4] PERSISTÊNCIA: UPDATE ou INSERT conforme existência
                // ─────────────────────────────────────────────────────────────────────────────
                if (estatisticaExistente != null)
                {
                    // CENÁRIO: Cache existe → Atualiza todos os campos
                    AtualizarEstatistica(estatisticaExistente, novaEstatistica);
                    await _context.SaveChangesAsync();
                    return estatisticaExistente;
                }
                else
                {
                    // CENÁRIO: Cache não existe → Insere novo registro
                    novaEstatistica.DataCriacao = DateTime.Now;
                    await _repository.AddAsync(novaEstatistica);
                    await _context.SaveChangesAsync();
                    return novaEstatistica;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter estatísticas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Obtém estatísticas de um período APENAS lendo do cache.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    ✅ CORREÇÃO CRÍTICA: NÃO recalcula, apenas lê dados já processados.
        /// │    Método otimizado para consultas rápidas de períodos grandes.
        /// │
        /// │ DIFERENÇA DE ObterEstatisticasAsync():
        /// │    - ObterEstatisticasAsync(): SEMPRE recalcula (1 dia).
        /// │    - ObterEstatisticasPeriodoAsync(): APENAS lê cache (N dias).
        /// │
        /// │ USO TÍPICO:
        /// │    Dashboards, relatórios mensais/anuais que precisam de performance.
        /// │    Se os dados não existirem no cache, retorna lista vazia.
        /// │
        /// │ IMPORTANTE:
        /// │    Antes de usar este método, certifique-se que o cache foi populado via:
        /// │    - ObterEstatisticasAsync() ou RecalcularEstatisticasAsync().
        /// │
        /// │ PARÂMETROS:
        /// │    -> dataInicio: Data inicial do período.
        /// │    -> dataFim: Data final do período (inclusive).
        /// │
        /// │ RETORNO:
        /// │    -> Task<List<ViagemEstatistica>>: Lista ordenada por data (sem recálculo).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<List<ViagemEstatistica>> ObterEstatisticasPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                // ─────────────────────────────────────────────────────────────────────────────
                // [LÓGICA] LEITURA DIRETA DO CACHE - Performance otimizada
                // ─────────────────────────────────────────────────────────────────────────────
                // IMPORTANTE: NÃO usa .Include() ou JOINs desnecessários.
                // AsNoTracking() garante leitura sem rastreamento de mudanças (mais rápido).
                var estatisticas = await _context.ViagemEstatistica
                    .Where(e => e.DataReferencia >= dataInicio.Date && e.DataReferencia <= dataFim.Date)
                    .OrderBy(e => e.DataReferencia)
                    .AsNoTracking()
                    .ToListAsync();

                return estatisticas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter estatísticas do período: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Calcula estatísticas em tempo real (método privado core).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Motor de cálculo que processa todas as viagens de uma data e gera
        /// │    estatísticas agregadas, rankings JSON e custos consolidados.
        /// │
        /// │ PROCESSAMENTO:
        /// │    1. Busca todas as viagens da data com dados relacionados (Includes).
        /// │    2. Calcula contadores por Status (Realizada, Aberta, Agendada, etc).
        /// │    3. Soma custos por tipo (Veículo, Motorista, Operador, Lavador, Combustível).
        /// │    4. Calcula quilometragem total e média (apenas viagens Realizadas).
        /// │    5. Gera rankings JSON (Top 10): Motoristas, Veículos, Setores, etc.
        /// │    6. Gera distribuições JSON: Por Status, Por Finalidade, Por Custo.
        /// │
        /// │ PERFORMANCE:
        /// │    - Usa .Include() para evitar N+1 queries.
        /// │    - Traz todos os dados em UMA consulta, processa em memória.
        /// │    - Adequado para volumes diários (centenas/milhares de viagens por dia).
        /// │
        /// │ PARÂMETROS:
        /// │    -> dataReferencia: Data a ser processada (Date, sem hora).
        /// │
        /// │ RETORNO:
        /// │    -> Task<ViagemEstatistica>: Objeto completo com todos os campos calculados.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        private async Task<ViagemEstatistica> CalcularEstatisticasAsync(DateTime dataReferencia)
        {
            var estatistica = new ViagemEstatistica
            {
                DataReferencia = dataReferencia
            };

            // ─────────────────────────────────────────────────────────────────────────────
            // [ETAPA 1] COLETA DE DADOS: Busca viagens do dia com dados relacionados
            // ─────────────────────────────────────────────────────────────────────────────
            // ESTRATÉGIA: Include para evitar N+1 queries.
            // CONCEITO: Traz TUDO de uma vez (Motorista, Veículo, Requisitante, Setor).
            var viagens = await _context.Viagem
                .Include(v => v.Motorista)
                .Include(v => v.Veiculo)
                .Include(v => v.Requisitante)
                .Include(v => v.SetorSolicitante)
                .Where(v => v.DataInicial.HasValue && v.DataInicial.Value.Date == dataReferencia)
                .ToListAsync();

            // ═════════════════════════════════════════════════════════════════════════════
            // [BLOCO 1] ESTATÍSTICAS GERAIS - Contadores por Status
            // ═════════════════════════════════════════════════════════════════════════════
            // CONCEITO: Contagem simples de viagens agrupadas por Status.
            // STATUS POSSÍVEIS: "Realizada", "Aberta", "Agendada", "Cancelada".
            estatistica.TotalViagens = viagens.Count;
            estatistica.ViagensFinalizadas = viagens.Count(v => v.Status == "Realizada");
            estatistica.ViagensEmAndamento = viagens.Count(v => v.Status == "Aberta");
            estatistica.ViagensAgendadas = viagens.Count(v => v.Status == "Agendada");
            estatistica.ViagensCanceladas = viagens.Count(v => v.Status == "Cancelada");

            // ═════════════════════════════════════════════════════════════════════════════
            // [BLOCO 2] CUSTOS GERAIS - Somatório e Médias
            // ═════════════════════════════════════════════════════════════════════════════
            // CONCEITO: Cada viagem pode ter 5 tipos de custo. CustoTotal = Σ(todos os custos).
            // TIPOS DE CUSTO:
            // - CustoVeiculo: Diária/depreciação do veículo.
            // - CustoMotorista: Salário/hora do motorista.
            // - CustoOperador: Salário/hora do operador (se houver).
            // - CustoLavador: Custo de limpeza/lavagem.
            // - CustoCombustivel: Abastecimento/combustível.
            estatistica.CustoTotal = (decimal)viagens.Sum(v =>
                (v.CustoVeiculo ?? 0) +
                (v.CustoMotorista ?? 0) +
                (v.CustoOperador ?? 0) +
                (v.CustoLavador ?? 0) +
                (v.CustoCombustivel ?? 0));

            // CÁLCULO: Custo médio por viagem (evita divisão por zero)
            estatistica.CustoMedioPorViagem = estatistica.TotalViagens > 0
                ? estatistica.CustoTotal / estatistica.TotalViagens
                : 0;

            // DESAGREGAÇÃO: Custos individuais por tipo (para gráficos de pizza/barra)
            estatistica.CustoVeiculo = (decimal)viagens.Sum(v => v.CustoVeiculo ?? 0);
            estatistica.CustoMotorista = (decimal)viagens.Sum(v => v.CustoMotorista ?? 0);
            estatistica.CustoOperador = (decimal)viagens.Sum(v => v.CustoOperador ?? 0);
            estatistica.CustoLavador = (decimal)viagens.Sum(v => v.CustoLavador ?? 0);
            estatistica.CustoCombustivel = (decimal)viagens.Sum(v => v.CustoCombustivel ?? 0);

            // ═════════════════════════════════════════════════════════════════════════════
            // [BLOCO 3] QUILOMETRAGEM - Total e Média
            // ═════════════════════════════════════════════════════════════════════════════
            // REGRA: Apenas viagens "Realizadas" com KmFinal > KmInicial são consideradas.
            // VALIDAÇÕES:
            // - KmFinal e KmInicial devem existir (HasValue).
            // - Status deve ser "Realizada" (viagens não finalizadas não têm KM válido).
            // - KmFinal > 0 (proteção contra dados inconsistentes).
            var viagensComKm = viagens
                .Where(v => v.KmFinal.HasValue &&
                           v.KmInicial.HasValue &&
                           v.Status == "Realizada" &&
                           v.KmFinal > 0)
                .ToList();

            if (viagensComKm.Any())
            {
                // CÁLCULO: KM rodado = KmFinal - KmInicial (para cada viagem)
                estatistica.QuilometragemTotal = viagensComKm.Sum(v =>
                    (v.KmFinal ?? 0) - (v.KmInicial ?? 0));
                // CÁLCULO: Média simples (KM total dividido pelo número de viagens)
                estatistica.QuilometragemMedia = estatistica.QuilometragemTotal / viagensComKm.Count;
            }

            // ═════════════════════════════════════════════════════════════════════════════
            // [BLOCO 4] RANKINGS E DISTRIBUIÇÕES JSON - Para Gráficos Dinâmicos
            // ═════════════════════════════════════════════════════════════════════════════
            // CONCEITO: Dados pré-agregados em JSON para evitar joins complexos no frontend.
            // FORMATO: Serialização JSON direta (charts consomem JSON, não objetos C#).

            // ─────────────────────────────────────────────────────────────────────────────
            // [JSON 1] Distribuição por Status
            // ─────────────────────────────────────────────────────────────────────────────
            // USO: Gráfico de pizza/rosca mostrando % de Realizadas, Agendadas, Canceladas.
            var viagensPorStatus = viagens
                .GroupBy(v => v.Status)
                .Select(g => new { status = g.Key, quantidade = g.Count() })
                .ToList();
            estatistica.ViagensPorStatusJson = JsonSerializer.Serialize(viagensPorStatus);

            // ─────────────────────────────────────────────────────────────────────────────
            // [JSON 2] Ranking TOP 10 Motoristas (por quantidade de viagens)
            // ─────────────────────────────────────────────────────────────────────────────
            // USO: Tabela/gráfico de barras dos motoristas mais ativos.
            var viagensPorMotorista = viagens
                .Where(v => v.Motorista != null)
                .GroupBy(v => v.Motorista.Nome)
                .Select(g => new { motorista = g.Key, quantidade = g.Count() })
                .OrderByDescending(x => x.quantidade)
                .Take(10)
                .ToList();
            estatistica.ViagensPorMotoristaJson = JsonSerializer.Serialize(viagensPorMotorista);

            // ─────────────────────────────────────────────────────────────────────────────
            // [JSON 3] Ranking TOP 10 Veículos (por quantidade de viagens)
            // ─────────────────────────────────────────────────────────────────────────────
            // USO: Tabela/gráfico identificando veículos mais utilizados.
            var viagensPorVeiculo = viagens
                .Where(v => v.Veiculo != null)
                .GroupBy(v => v.Veiculo.Placa)
                .Select(g => new { veiculo = g.Key, quantidade = g.Count() })
                .OrderByDescending(x => x.quantidade)
                .Take(10)
                .ToList();
            estatistica.ViagensPorVeiculoJson = JsonSerializer.Serialize(viagensPorVeiculo);

            // ─────────────────────────────────────────────────────────────────────────────
            // [JSON 4] Distribuição por Finalidade (Todas as finalidades)
            // ─────────────────────────────────────────────────────────────────────────────
            // USO: Gráfico de barras horizontais mostrando propósito das viagens.
            // EXEMPLOS: "Transporte de Material", "Visita Técnica", "Reunião Externa".
            var viagensPorFinalidade = viagens
                .Where(v => !string.IsNullOrEmpty(v.Finalidade))
                .GroupBy(v => v.Finalidade)
                .Select(g => new { finalidade = g.Key, quantidade = g.Count() })
                .OrderByDescending(x => x.quantidade)
                .ToList();
            estatistica.ViagensPorFinalidadeJson = JsonSerializer.Serialize(viagensPorFinalidade);

            // ─────────────────────────────────────────────────────────────────────────────
            // [JSON 5] Ranking TOP 10 Requisitantes (quem mais solicitou viagens)
            // ─────────────────────────────────────────────────────────────────────────────
            // USO: Identificar funcionários que mais demandam transporte.
            var viagensPorRequisitante = viagens
                .Where(v => v.Requisitante != null)
                .GroupBy(v => v.Requisitante.Nome)
                .Select(g => new { requisitante = g.Key, quantidade = g.Count() })
                .OrderByDescending(x => x.quantidade)
                .Take(10)
                .ToList();
            estatistica.ViagensPorRequisitanteJson = JsonSerializer.Serialize(viagensPorRequisitante);

            // ─────────────────────────────────────────────────────────────────────────────
            // [JSON 6] Ranking TOP 10 Setores Solicitantes
            // ─────────────────────────────────────────────────────────────────────────────
            // USO: Análise de demanda por departamento/setor.
            // CONCEITO: Identificar quais setores mais consomem recursos da frota.
            var viagensPorSetor = viagens
                .Where(v => v.SetorSolicitante != null)
                .GroupBy(v => v.SetorSolicitante.Nome)
                .Select(g => new { setor = g.Key, quantidade = g.Count() })
                .OrderByDescending(x => x.quantidade)
                .Take(10)
                .ToList();
            estatistica.ViagensPorSetorJson = JsonSerializer.Serialize(viagensPorSetor);

            // ─────────────────────────────────────────────────────────────────────────────
            // [JSON 7] Ranking TOP 10 Custos por Motorista
            // ─────────────────────────────────────────────────────────────────────────────
            // USO: Identificar motoristas com maior custo agregado (não apenas quantidade).
            var custosPorMotorista = viagens
                .Where(v => v.Motorista != null)
                .GroupBy(v => v.Motorista.Nome)
                .Select(g => new
                {
                    motorista = g.Key,
                    custoTotal = g.Sum(v => (v.CustoMotorista ?? 0))
                })
                .OrderByDescending(x => x.custoTotal)
                .Take(10)
                .ToList();
            estatistica.CustosPorMotoristaJson = JsonSerializer.Serialize(custosPorMotorista);

            // ─────────────────────────────────────────────────────────────────────────────
            // [JSON 8] Ranking TOP 10 Custos por Veículo
            // ─────────────────────────────────────────────────────────────────────────────
            // USO: Identificar veículos mais caros de operar (manutenção, depreciação).
            var custosPorVeiculo = viagens
                .Where(v => v.Veiculo != null)
                .GroupBy(v => v.Veiculo.Placa)
                .Select(g => new
                {
                    veiculo = g.Key,
                    custoTotal = g.Sum(v => (v.CustoVeiculo ?? 0))
                })
                .OrderByDescending(x => x.custoTotal)
                .Take(10)
                .ToList();
            estatistica.CustosPorVeiculoJson = JsonSerializer.Serialize(custosPorVeiculo);

            // ─────────────────────────────────────────────────────────────────────────────
            // [JSON 9] Ranking TOP 10 KM por Veículo
            // ─────────────────────────────────────────────────────────────────────────────
            // USO: Identificar veículos com maior rodagem (pode indicar desgaste).
            // REGRA: Apenas viagens Realizadas com KM válido.
            var kmPorVeiculo = viagens
                .Where(v => v.Veiculo != null &&
                           v.KmFinal.HasValue &&
                           v.KmInicial.HasValue &&
                           v.Status == "Realizada" &&
                           v.KmFinal > 0)
                .GroupBy(v => v.Veiculo.Placa)
                .Select(g => new
                {
                    veiculo = g.Key,
                    kmTotal = g.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0))
                })
                .OrderByDescending(x => x.kmTotal)
                .Take(10)
                .ToList();
            estatistica.KmPorVeiculoJson = JsonSerializer.Serialize(kmPorVeiculo);

            // ─────────────────────────────────────────────────────────────────────────────
            // [JSON 10] Distribuição de Custos por Tipo (Para gráfico de pizza)
            // ─────────────────────────────────────────────────────────────────────────────
            // USO: Visualizar % de cada tipo de custo no total.
            // EXEMPLO: Veículo=40%, Motorista=30%, Combustível=20%, Lavador=5%, Operador=5%.
            var custosPorTipo = new List<object>
            {
                new { tipo = "Veículo", custo = estatistica.CustoVeiculo },
                new { tipo = "Motorista", custo = estatistica.CustoMotorista },
                new { tipo = "Operador", custo = estatistica.CustoOperador },
                new { tipo = "Lavador", custo = estatistica.CustoLavador },
                new { tipo = "Combustível", custo = estatistica.CustoCombustivel }
            };
            estatistica.CustosPorTipoJson = JsonSerializer.Serialize(custosPorTipo);

            return estatistica;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Atualiza estatística existente com novos dados.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Método auxiliar que copia TODOS os campos calculados de uma estatística
        /// │    nova para uma existente, preservando apenas DataCriacao e atualizando
        /// │    DataAtualizacao.
        /// │
        /// │ CONCEITO:
        /// │    Evita deletar e recriar registros no banco (preserva chave primária).
        /// │    Atualiza in-place, mantendo histórico de DataCriacao.
        /// │
        /// │ PARÂMETROS:
        /// │    -> existente: Registro atual no banco (será modificado).
        /// │    -> nova: Dados recém-calculados (fonte dos valores).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        private void AtualizarEstatistica(ViagemEstatistica existente, ViagemEstatistica nova)
        {
            // ─────────────────────────────────────────────────────────────────────────────
            // [ATUALIZAÇÃO] Copia campo a campo (sem reflexão, por performance)
            // ─────────────────────────────────────────────────────────────────────────────
            existente.TotalViagens = nova.TotalViagens;
            existente.ViagensFinalizadas = nova.ViagensFinalizadas;
            existente.ViagensEmAndamento = nova.ViagensEmAndamento;
            existente.ViagensAgendadas = nova.ViagensAgendadas;
            existente.ViagensCanceladas = nova.ViagensCanceladas;
            existente.CustoTotal = nova.CustoTotal;
            existente.CustoMedioPorViagem = nova.CustoMedioPorViagem;
            existente.CustoVeiculo = nova.CustoVeiculo;
            existente.CustoMotorista = nova.CustoMotorista;
            existente.CustoOperador = nova.CustoOperador;
            existente.CustoLavador = nova.CustoLavador;
            existente.CustoCombustivel = nova.CustoCombustivel;
            existente.QuilometragemTotal = nova.QuilometragemTotal;
            existente.QuilometragemMedia = nova.QuilometragemMedia;
            existente.ViagensPorStatusJson = nova.ViagensPorStatusJson;
            existente.ViagensPorMotoristaJson = nova.ViagensPorMotoristaJson;
            existente.ViagensPorVeiculoJson = nova.ViagensPorVeiculoJson;
            existente.ViagensPorFinalidadeJson = nova.ViagensPorFinalidadeJson;
            existente.ViagensPorRequisitanteJson = nova.ViagensPorRequisitanteJson;
            existente.ViagensPorSetorJson = nova.ViagensPorSetorJson;
            existente.CustosPorMotoristaJson = nova.CustosPorMotoristaJson;
            existente.CustosPorVeiculoJson = nova.CustosPorVeiculoJson;
            existente.KmPorVeiculoJson = nova.KmPorVeiculoJson;
            existente.CustosPorTipoJson = nova.CustosPorTipoJson;
            // ⚠️ PRESERVA: DataCriacao (campo de auditoria, não pode ser alterado)
            // ✅ ATUALIZA: DataAtualizacao (timestamp do recálculo)
            existente.DataAtualizacao = DateTime.Now;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Força recálculo das estatísticas (alias de ObterEstatisticasAsync).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Nome mais explícito para indicar que o método SEMPRE recalcula.
        /// │    Internamente faz exatamente o mesmo que ObterEstatisticasAsync().
        /// │
        /// │ USO:
        /// │    Quando a semântica "recalcular" for mais clara que "obter".
        /// │    Exemplo: Após corrigir dados inconsistentes, forçar recálculo.
        /// │
        /// │ PARÂMETROS:
        /// │    -> data: Data de referência.
        /// │
        /// │ RETORNO:
        /// │    -> Task<ViagemEstatistica>: Estatísticas recém-calculadas.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<ViagemEstatistica> RecalcularEstatisticasAsync(DateTime data)
        {
            try
            {
                var dataReferencia = data.Date;

                // ─────────────────────────────────────────────────────────────────────────────
                // [LÓGICA] Idêntica a ObterEstatisticasAsync (sempre recalcula)
                // ─────────────────────────────────────────────────────────────────────────────
                var novaEstatistica = await CalcularEstatisticasAsync(dataReferencia);
                var estatisticaExistente = await _repository.ObterPorDataAsync(dataReferencia);

                if (estatisticaExistente != null)
                {
                    AtualizarEstatistica(estatisticaExistente, novaEstatistica);
                    await _context.SaveChangesAsync();
                    return estatisticaExistente;
                }
                else
                {
                    novaEstatistica.DataCriacao = DateTime.Now;
                    await _repository.AddAsync(novaEstatistica);
                    await _context.SaveChangesAsync();
                    return novaEstatistica;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao recalcular estatísticas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Atualiza estatísticas de um dia específico (trigger hook).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Método chamado automaticamente após criar/editar/deletar uma viagem.
        /// │    Garante que o cache esteja sempre sincronizado com os dados reais.
        /// │
        /// │ FLUXO DE CHAMADA:
        /// │    ViagemController.Create() → AtualizarEstatisticasDiaAsync()
        /// │    ViagemController.Edit()   → AtualizarEstatisticasDiaAsync()
        /// │    ViagemController.Delete() → AtualizarEstatisticasDiaAsync()
        /// │
        /// │ CONCEITO:
        /// │    Hook de sincronização. Mantém cache quente (warm cache).
        /// │
        /// │ PARÂMETROS:
        /// │    -> data: Data da viagem afetada.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task AtualizarEstatisticasDiaAsync(DateTime data)
        {
            try
            {
                // ─────────────────────────────────────────────────────────────────────────────
                // [DELEGAÇÃO] Redireciona para RecalcularEstatisticasAsync (mesmo comportamento)
                // ─────────────────────────────────────────────────────────────────────────────
                await RecalcularEstatisticasAsync(data);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar estatísticas do dia: {ex.Message}", ex);
            }
        }
    }
}
