/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViagemEstatisticaService.cs                                                             ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Cálculo e cache de estatísticas diárias de viagens. Persiste em ViagemEstatistica.     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: ObterEstatisticasAsync(), ObterEstatisticasPeriodoAsync(), RecalcularEstatisticasAsync() ║
   ║ 🔗 DEPS: FrotiXDbContext, IViagemEstatisticaRepository | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0       ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

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
    public class ViagemEstatisticaService
    {
        private readonly FrotiXDbContext _context;
        private readonly IViagemEstatisticaRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ViagemEstatisticaService(
            FrotiXDbContext context ,
            IViagemEstatisticaRepository repository ,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ObterEstatisticasAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obtém ou calcula estatísticas diárias de viagens, com cache em DB
         *                   SEMPRE recalcula (não confia apenas no cache) e atualiza
         *
         * 📥 ENTRADAS     : data [DateTime] - Data das estatísticas (qualquer hora, usa .Date)
         *
         * 📤 SAÍDAS       : Task<ViagemEstatistica> - Objeto com todas estatísticas do dia
         *
         * ⬅️ CHAMADO POR  : DashboardViagensController.ObterEstatisticas() [Dashboard]
         *                   ViagemController.AoFinalizarViagem() [Trigger após alteração]
         *
         * ➡️ CHAMA        : _repository.ObterPorDataAsync() [Busca cache]
         *                   CalcularEstatisticasAsync() [Recalcula sempre]
         *                   AtualizarEstatistica() [UPDATE se existe]
         *                   _repository.AddAsync() [INSERT se novo]
         *                   _context.SaveChangesAsync() [DB commit]
         *
         * 📝 OBSERVAÇÕES  : [REGRA] SEMPRE recalcula (não trusted cache-only)
         *                   [LOGICA] INSERT or UPDATE pattern (upsert)
         *                   [PERFORMANCE] Cálculo é assíncrono (não bloqueia thread)
         *                   [DEBUG] Se erro, lança com mensagem original
         ****************************************************************************************/
        public async Task<ViagemEstatistica> ObterEstatisticasAsync(DateTime data)
        {
            try
            {
                var dataReferencia = data.Date;

                // [DB] Tenta buscar estatísticas já calculadas
                var estatisticaExistente = await _repository.ObterPorDataAsync(dataReferencia);

                // [LOGICA] SEMPRE recalcula (mesmo se existe cache)
                // Previne dados stale após criação/edição/exclusão de viagens
                var novaEstatistica = await CalcularEstatisticasAsync(dataReferencia);

                // [LOGICA] INSERT or UPDATE
                if (estatisticaExistente != null)
                {
                    // [DB] UPDATE: merge nova estatística na existente
                    AtualizarEstatistica(estatisticaExistente , novaEstatistica);
                    await _context.SaveChangesAsync();
                    return estatisticaExistente;
                }
                else
                {
                    // [DB] INSERT: novo registro
                    novaEstatistica.DataCriacao = DateTime.Now;
                    await _repository.AddAsync(novaEstatistica);
                    await _context.SaveChangesAsync();
                    return novaEstatistica;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter estatísticas: {ex.Message}" , ex);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ObterEstatisticasPeriodoAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obtém estatísticas de um período APENAS DO CACHE (read-only)
         *                   Não recalcula, apenas lê tabela ViagemEstatistica pré-calculada
         *
         * 📥 ENTRADAS     : dataInicio [DateTime] - Data inicial do período (inclusive)
         *                   dataFim [DateTime] - Data final do período (inclusive)
         *
         * 📤 SAÍDAS       : Task<List<ViagemEstatistica>> - Estatísticas do período
         *
         * ⬅️ CHAMADO POR  : DashboardViagensController.ObterGráficos() [Período selecionado]
         *                   ReportController.GerarRelatorioMensal() [Relatório]
         *
         * ➡️ CHAMA        : _context.ViagemEstatistica.ToListAsync() [EF Core query]
         *
         * 📝 OBSERVAÇÕES  : [PERFORMANCE] LEITURA PURA - sem cálculos (muito rápido)
         *                   [REGRA] AsNoTracking() = sem tracking, menor memória
         *                   [REGRA] Retorna vazio se dados não calculados ainda
         *                   [VALIDACAO] dataFim é inclusiva (<=)
         *                   ⚠️ ATENÇÃO: Se tabela vazia, retorna [], precisar chamar
         *                              ObterEstatisticasAsync para calcular primeira vez
         ****************************************************************************************/
        public async Task<List<ViagemEstatistica>> ObterEstatisticasPeriodoAsync(DateTime dataInicio , DateTime dataFim)
        {
            try
            {
                // [PERFORMANCE] LEITURA DIRETA DO CACHE - NÃO RECALCULA
                // AsNoTracking = sem tracking (mais rápido, sem warmup)
                var estatisticas = await _context.ViagemEstatistica
                    .Where(e => e.DataReferencia >= dataInicio.Date && e.DataReferencia <= dataFim.Date)
                    .OrderBy(e => e.DataReferencia)
                    .AsNoTracking()
                    .ToListAsync();

                return estatisticas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter estatísticas do período: {ex.Message}" , ex);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularEstatisticasAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Calcula TODAS estatísticas de um dia (11 dimensões diferentes)
         *                   Inclui: contagens, custos, rankings TOP 10, séries históricas JSON
         *
         * 📥 ENTRADAS     : dataReferencia [DateTime] - Data para cálculo (date only, sem hora)
         *
         * 📤 SAÍDAS       : Task<ViagemEstatistica> - Objeto com 30+ propriedades preenchidas
         *
         * ⬅️ CHAMADO POR  : ObterEstatisticasAsync() [linha 44]
         *                   RecalcularEstatisticasAsync() [linha 337]
         *
         * ➡️ CHAMA        : _context.Viagem.Include(...).ToListAsync() [EF materializa viagens]
         *                   JsonSerializer.Serialize() [Serializa rankings]
         *
         * 📝 OBSERVAÇÕES  : [PERFORMANCE] Materializa TODAS viagens do dia na memória
         *                   [LOGICA] 11 seções: gerais, custos, KM, status, motorista, etc
         *                   [REGRA] TOP 10 para motoristas, veículos, requisitantes, setores
         *                   [DADOS] Transforma listas em JSON para armazenar rankings
         *                   [VALIDACAO] Filtros (HasValue, > 0) previnem cálculos em nulos
         *                   ⚠️ PERFORMANCE: Se muitas viagens/dia, pode ser lento
         ****************************************************************************************/
        private async Task<ViagemEstatistica> CalcularEstatisticasAsync(DateTime dataReferencia)
        {
            var estatistica = new ViagemEstatistica
            {
                DataReferencia = dataReferencia
            };

            // Busca todas as viagens do dia com dados relacionados
            var viagens = await _context.Viagem
                .Include(v => v.Motorista)
                .Include(v => v.Veiculo)
                .Include(v => v.Requisitante)
                .Include(v => v.SetorSolicitante)
                .Where(v => v.DataInicial.HasValue && v.DataInicial.Value.Date == dataReferencia)
                .ToListAsync();

            // ========================================
            // ESTATÍSTICAS GERAIS
            // ========================================
            estatistica.TotalViagens = viagens.Count;
            estatistica.ViagensFinalizadas = viagens.Count(v => v.Status == "Realizada");
            estatistica.ViagensEmAndamento = viagens.Count(v => v.Status == "Aberta");
            estatistica.ViagensAgendadas = viagens.Count(v => v.Status == "Agendada");
            estatistica.ViagensCanceladas = viagens.Count(v => v.Status == "Cancelada");

            // ========================================
            // CUSTOS GERAIS
            // ========================================
            estatistica.CustoTotal = (decimal)viagens.Sum(v =>
                (v.CustoVeiculo ?? 0) +
                (v.CustoMotorista ?? 0) +
                (v.CustoOperador ?? 0) +
                (v.CustoLavador ?? 0) +
                (v.CustoCombustivel ?? 0));

            estatistica.CustoMedioPorViagem = estatistica.TotalViagens > 0
                ? estatistica.CustoTotal / estatistica.TotalViagens
                : 0;

            estatistica.CustoVeiculo = (decimal)viagens.Sum(v => v.CustoVeiculo ?? 0);
            estatistica.CustoMotorista = (decimal)viagens.Sum(v => v.CustoMotorista ?? 0);
            estatistica.CustoOperador = (decimal)viagens.Sum(v => v.CustoOperador ?? 0);
            estatistica.CustoLavador = (decimal)viagens.Sum(v => v.CustoLavador ?? 0);
            estatistica.CustoCombustivel = (decimal)viagens.Sum(v => v.CustoCombustivel ?? 0);

            // ========================================
            // QUILOMETRAGEM
            // ========================================
            var viagensComKm = viagens
                .Where(v => v.KmFinal.HasValue &&
                           v.KmInicial.HasValue &&
                           v.Status == "Realizada" &&
                           v.KmFinal > 0)
                .ToList();

            if (viagensComKm.Any())
            {
                estatistica.QuilometragemTotal = viagensComKm.Sum(v =>
                    (v.KmFinal ?? 0) - (v.KmInicial ?? 0));
                estatistica.QuilometragemMedia = estatistica.QuilometragemTotal / viagensComKm.Count;
            }

            // ========================================
            // VIAGENS POR STATUS (JSON)
            // ========================================
            var viagensPorStatus = viagens
                .GroupBy(v => v.Status)
                .Select(g => new { status = g.Key , quantidade = g.Count() })
                .ToList();
            estatistica.ViagensPorStatusJson = JsonSerializer.Serialize(viagensPorStatus);

            // ========================================
            // VIAGENS POR MOTORISTA - TOP 10 (JSON)
            // ========================================
            var viagensPorMotorista = viagens
                .Where(v => v.Motorista != null)
                .GroupBy(v => v.Motorista.Nome)
                .Select(g => new { motorista = g.Key , quantidade = g.Count() })
                .OrderByDescending(x => x.quantidade)
                .Take(10)
                .ToList();
            estatistica.ViagensPorMotoristaJson = JsonSerializer.Serialize(viagensPorMotorista);

            // ========================================
            // VIAGENS POR VEÍCULO - TOP 10 (JSON)
            // ========================================
            var viagensPorVeiculo = viagens
                .Where(v => v.Veiculo != null)
                .GroupBy(v => v.Veiculo.Placa)
                .Select(g => new { veiculo = g.Key , quantidade = g.Count() })
                .OrderByDescending(x => x.quantidade)
                .Take(10)
                .ToList();
            estatistica.ViagensPorVeiculoJson = JsonSerializer.Serialize(viagensPorVeiculo);

            // ========================================
            // VIAGENS POR FINALIDADE (JSON)
            // ========================================
            var viagensPorFinalidade = viagens
                .Where(v => !string.IsNullOrEmpty(v.Finalidade))
                .GroupBy(v => v.Finalidade)
                .Select(g => new { finalidade = g.Key , quantidade = g.Count() })
                .OrderByDescending(x => x.quantidade)
                .ToList();
            estatistica.ViagensPorFinalidadeJson = JsonSerializer.Serialize(viagensPorFinalidade);

            // ========================================
            // VIAGENS POR REQUISITANTE - TOP 10 (JSON)
            // ========================================
            var viagensPorRequisitante = viagens
                .Where(v => v.Requisitante != null)
                .GroupBy(v => v.Requisitante.Nome)
                .Select(g => new { requisitante = g.Key , quantidade = g.Count() })
                .OrderByDescending(x => x.quantidade)
                .Take(10)
                .ToList();
            estatistica.ViagensPorRequisitanteJson = JsonSerializer.Serialize(viagensPorRequisitante);

            // ========================================
            // VIAGENS POR SETOR - TOP 10 (JSON)
            // ========================================
            var viagensPorSetor = viagens
                .Where(v => v.SetorSolicitante != null)
                .GroupBy(v => v.SetorSolicitante.Nome)
                .Select(g => new { setor = g.Key , quantidade = g.Count() })
                .OrderByDescending(x => x.quantidade)
                .Take(10)
                .ToList();
            estatistica.ViagensPorSetorJson = JsonSerializer.Serialize(viagensPorSetor);

            // ========================================
            // CUSTOS POR MOTORISTA - TOP 10 (JSON)
            // ========================================
            var custosPorMotorista = viagens
                .Where(v => v.Motorista != null)
                .GroupBy(v => v.Motorista.Nome)
                .Select(g => new
                {
                    motorista = g.Key ,
                    custoTotal = g.Sum(v => (v.CustoMotorista ?? 0))
                })
                .OrderByDescending(x => x.custoTotal)
                .Take(10)
                .ToList();
            estatistica.CustosPorMotoristaJson = JsonSerializer.Serialize(custosPorMotorista);

            // ========================================
            // CUSTOS POR VEÍCULO - TOP 10 (JSON)
            // ========================================
            var custosPorVeiculo = viagens
                .Where(v => v.Veiculo != null)
                .GroupBy(v => v.Veiculo.Placa)
                .Select(g => new
                {
                    veiculo = g.Key ,
                    custoTotal = g.Sum(v => (v.CustoVeiculo ?? 0))
                })
                .OrderByDescending(x => x.custoTotal)
                .Take(10)
                .ToList();
            estatistica.CustosPorVeiculoJson = JsonSerializer.Serialize(custosPorVeiculo);

            // ========================================
            // KM POR VEÍCULO - TOP 10 (JSON)
            // ========================================
            var kmPorVeiculo = viagens
                .Where(v => v.Veiculo != null &&
                           v.KmFinal.HasValue &&
                           v.KmInicial.HasValue &&
                           v.Status == "Realizada" &&
                           v.KmFinal > 0)
                .GroupBy(v => v.Veiculo.Placa)
                .Select(g => new
                {
                    veiculo = g.Key ,
                    kmTotal = g.Sum(v => (v.KmFinal ?? 0) - (v.KmInicial ?? 0))
                })
                .OrderByDescending(x => x.kmTotal)
                .Take(10)
                .ToList();
            estatistica.KmPorVeiculoJson = JsonSerializer.Serialize(kmPorVeiculo);

            // ========================================
            // CUSTOS POR TIPO (JSON)
            // ========================================
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

        /****************************
         * ⚡ FUNÇÃO: AtualizarEstatistica
         * ✅ Faz MERGE de objeto novo em objeto existente (UPDATE pattern)
         * 📝 OBSERVAÇÕES: Copia TODAS as 30+ propriedades
         ****************************/
        private void AtualizarEstatistica(ViagemEstatistica existente , ViagemEstatistica nova)
        {
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
            existente.DataAtualizacao = DateTime.Now;
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RecalcularEstatisticasAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Força recalcular estatísticas (ignora cache)
         *                   Similar a ObterEstatisticasAsync, mas com semântica de "forçar"
         *
         * 📥 ENTRADAS     : data [DateTime] - Data das estatísticas
         *
         * 📤 SAÍDAS       : Task<ViagemEstatistica> - Objeto atualizado
         *
         * ⬅️ CHAMADO POR  : ViagemController.AoEditarViagem() [Trigger após edição]
         *                   ViagemController.AoDeletarViagem() [Trigger após deleção]
         *                   AtualizarEstatisticasDiaAsync() [Wrapper]
         *
         * ➡️ CHAMA        : CalcularEstatisticasAsync() [Recalcula SEMPRE]
         *                   _repository.ObterPorDataAsync() [Busca para UPDATE]
         *                   AtualizarEstatistica() [Merge dados]
         *
         * 📝 OBSERVAÇÕES  : [REGRA] SEMPRE recalcula (não usa cache)
         *                   [PATTERN] INSERT or UPDATE, mesmo que ObterEstatisticasAsync
         *                   [DEBUG] Se erro, lança com mensagem contextual
         ****************************************************************************************/
        public async Task<ViagemEstatistica> RecalcularEstatisticasAsync(DateTime data)
        {
            try
            {
                var dataReferencia = data.Date;

                // [LOGICA] Recalcula SEMPRE (ignora cache)
                var novaEstatistica = await CalcularEstatisticasAsync(dataReferencia);

                // [DB] Busca estatística existente para UPDATE
                var estatisticaExistente = await _repository.ObterPorDataAsync(dataReferencia);

                if (estatisticaExistente != null)
                {
                    // [DB] UPDATE: merge nova estatística
                    AtualizarEstatistica(estatisticaExistente , novaEstatistica);
                    await _context.SaveChangesAsync();
                    return estatisticaExistente;
                }
                else
                {
                    // [DB] INSERT: novo registro
                    novaEstatistica.DataCriacao = DateTime.Now;
                    await _repository.AddAsync(novaEstatistica);
                    await _context.SaveChangesAsync();
                    return novaEstatistica;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao recalcular estatísticas: {ex.Message}" , ex);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AtualizarEstatisticasDiaAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Wrapper para atualizar estatísticas do dia (trigger após alteração)
         *                   Mantém cache fresco após CRUD de viagens
         *
         * 📥 ENTRADAS     : data [DateTime] - Data do dia afetado
         *
         * 📤 SAÍDAS       : Task (void) - Sem retorno
         *
         * ⬅️ CHAMADO POR  : ViagemController.OnCreate() [Trigger após criar viagem]
         *                   ViagemController.OnEdit() [Trigger após editar viagem]
         *                   ViagemController.OnDelete() [Trigger após deletar viagem]
         *
         * ➡️ CHAMA        : RecalcularEstatisticasAsync() [Força recalcular]
         *
         * 📝 OBSERVAÇÕES  : [PATTERN] Simple wrapper, sem lógica extra
         *                   [REGRA] Sempre recalcula (garante consistência)
         *                   [DEBUG] Se erro, relança com contexto
         ****************************************************************************************/
        public async Task AtualizarEstatisticasDiaAsync(DateTime data)
        {
            try
            {
                // [LOGICA] Chama recalcular (garante dados sempre frescos)
                await RecalcularEstatisticasAsync(data);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar estatísticas do dia: {ex.Message}" , ex);
            }
        }
    }
}
