// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║   📊 ViagemEstatisticaRepository.cs | Repository/ | 2026-01-20                                    ║
// ║   Estatísticas agregadas de viagens (analytics/cache). 5 métodos async especializados            ║
// ║   ✅ AsNoTracking, tratamento de erros, SaveChangesAsync. ⚠️ Quebra Unit of Work                 ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
{
    public class ViagemEstatisticaRepository : Repository<ViagemEstatistica>, IViagemEstatisticaRepository
    {
        private readonly FrotiXDbContext _context;

        public ViagemEstatisticaRepository(FrotiXDbContext context) : base(context)
        {
            _context = context;
        }

        // [ETAPA] ObterPorDataAsync - Busca estatística de data específica
        // ✅ AsNoTracking() evita problemas de tracking, try-catch com mensagem descritiva
        // [ETAPA] ObterPorDataAsync - Busca estatística de data específica
        // ✅ AsNoTracking() evita problemas de tracking, try-catch com mensagem descritiva
        /// <summary>
        /// Busca estatística por data de referência
        /// </summary>
        public async Task<ViagemEstatistica> ObterPorDataAsync(DateTime dataReferencia)
        {
            try
            {
                var data = dataReferencia.Date;

                // AsNoTracking() evita problemas de tracking do EF Core
                // Comparação usando variável local evita problemas de timezone
                return await _context.ViagemEstatistica
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.DataReferencia == data);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar estatística por data: {ex.Message}" , ex);
            }
        }

        // [ETAPA] ObterPorPeriodoAsync - Busca estatísticas de intervalo de datas
        /// <summary>
        /// Busca estatísticas de um período
        /// </summary>
        public async Task<List<ViagemEstatistica>> ObterPorPeriodoAsync(DateTime dataInicio , DateTime dataFim)
        {
            try
            {
                return await _context.ViagemEstatistica
                    .Where(e => e.DataReferencia >= dataInicio.Date && e.DataReferencia <= dataFim.Date)
                    .OrderBy(e => e.DataReferencia)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar estatísticas por período: {ex.Message}" , ex);
            }
        }

        // [ETAPA] ExisteParaDataAsync - Verifica existência (AnyAsync)
        /// <summary>
        /// Verifica se existe estatística para uma data
        /// </summary>
        public async Task<bool> ExisteParaDataAsync(DateTime dataReferencia)
        {
            try
            {
                var data = dataReferencia.Date;
                return await _context.ViagemEstatistica
                    .AnyAsync(e => e.DataReferencia == data);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar existência de estatística: {ex.Message}" , ex);
            }
        }

        // [ETAPA] RemoverEstatisticasAntigasAsync - Cleanup automático (remover > 365 dias)
        // ⚠️ Quebra Unit of Work (SaveChangesAsync direto)
        /// <summary>
        /// Remove estatísticas antigas (mais de X dias)
        /// </summary>
        public async Task<int> RemoverEstatisticasAntigasAsync(int diasParaManter = 365)
        {
            try
            {
                var dataLimite = DateTime.Now.Date.AddDays(-diasParaManter);

                var estatisticasAntigas = await _context.ViagemEstatistica
                    .Where(e => e.DataReferencia < dataLimite)
                    .ToListAsync();

                if (estatisticasAntigas.Any())
                {
                    _context.ViagemEstatistica.RemoveRange(estatisticasAntigas);
                    await _context.SaveChangesAsync();
                    return estatisticasAntigas.Count;
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover estatísticas antigas: {ex.Message}" , ex);
            }
        }

        // [ETAPA] ObterEstatisticasDesatualizadasAsync - Busca estatísticas que precisam recalcular (> 1 hora)
        /// <summary>
        /// Busca estatísticas desatualizadas (mais de 1 hora desde última atualização)
        /// </summary>
        public async Task<List<ViagemEstatistica>> ObterEstatisticasDesatualizadasAsync()
        {
            try
            {
                var umHoraAtras = DateTime.Now.AddHours(-1);

                return await _context.ViagemEstatistica
                    .Where(e => e.DataAtualizacao == null || e.DataAtualizacao < umHoraAtras)
                    .OrderBy(e => e.DataReferencia)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar estatísticas desatualizadas: {ex.Message}" , ex);
            }
        }
    }
}
