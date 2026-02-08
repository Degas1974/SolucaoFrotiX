/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViagemEstatisticaRepository.cs                                                         ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para estatísticas de viagens (agregações por data).                                 ║
   ║    Fornece consultas por data/período, verificação e limpeza de históricos.                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViagemEstatisticaRepository(FrotiXDbContext context)                                          ║
   ║    • ObterPorDataAsync(DateTime dataReferencia)                                                    ║
   ║    • ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)                                   ║
   ║    • ExisteParaDataAsync(DateTime dataReferencia)                                                  ║
   ║    • RemoverEstatisticasAntigasAsync(int diasParaManter = 365)                                     ║
   ║    • ObterEstatisticasDesatualizadasAsync()                                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Consultas usam DateTime.Date para evitar variações de horário e timezone.                       ║
   ║    Métodos utilizam try/catch e encapsulam exceções com mensagens específicas.                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Repository
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViagemEstatisticaRepository                                                        │
    // │ 📦 HERDA DE: Repository                                                    │
    // │ 🔌 IMPLEMENTA: IViagemEstatisticaRepository                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável por estatísticas de viagens agregadas por data.
    // Disponibiliza consultas e rotinas de manutenção de histórico.
    
    public class ViagemEstatisticaRepository : Repository<ViagemEstatistica>, IViagemEstatisticaRepository
    {
        private readonly FrotiXDbContext _context;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViagemEstatisticaRepository                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Jobs                                            │
        // │    ➡️ CHAMA       : base(context)                                                        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // context - Contexto do banco de dados da aplicação.
        
        
        // Param context: Instância de <see cref="FrotiXDbContext"/>.
        public ViagemEstatisticaRepository(FrotiXDbContext context) : base(context)
        {
            _context = context;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ObterPorDataAsync                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Jobs, Controllers                                          │
        // │    ➡️ CHAMA       : DbContext.ViagemEstatistica, AsNoTracking, FirstOrDefaultAsync        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Buscar estatística por data de referência (ignora horário).
        
        
        
        // 📥 PARÂMETROS:
        // dataReferencia - Data de referência para consulta.
        
        
        
        // 📤 RETORNO:
        // Task&lt;ViagemEstatistica&gt; - Estatística encontrada ou null.
        
        
        // Param dataReferencia: Data de referência.
        // Returns: Estatística encontrada ou null.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ObterPorPeriodoAsync                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Jobs, Controllers                                          │
        // │    ➡️ CHAMA       : DbContext.ViagemEstatistica, Where, OrderBy, ToListAsync              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Buscar estatísticas dentro de um período de datas (inclusivo).
        
        
        
        // 📥 PARÂMETROS:
        // dataInicio - Data inicial do período
        // dataFim - Data final do período
        
        
        
        // 📤 RETORNO:
        // Task&lt;List&lt;ViagemEstatistica&gt;&gt; - Lista de estatísticas do período.
        
        
        // Param dataInicio: Data inicial.
        // Param dataFim: Data final.
        // Returns: Lista de estatísticas do período.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ExisteParaDataAsync                                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Jobs, Controllers                                          │
        // │    ➡️ CHAMA       : DbContext.ViagemEstatistica, AnyAsync                                 │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Verificar se existe estatística registrada para uma data.
        
        
        
        // 📥 PARÂMETROS:
        // dataReferencia - Data de referência para consulta.
        
        
        
        // 📤 RETORNO:
        // Task&lt;bool&gt; - True se existir estatística, senão false.
        
        
        // Param dataReferencia: Data de referência.
        // Returns: Indicador de existência.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RemoverEstatisticasAntigasAsync                                               │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Jobs, Controllers                                          │
        // │    ➡️ CHAMA       : DbContext.ViagemEstatistica, RemoveRange, SaveChangesAsync            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Remover estatísticas antigas (mais de X dias).
        
        
        
        // 📥 PARÂMETROS:
        // diasParaManter - Quantidade de dias a preservar no histórico.
        
        
        
        // 📤 RETORNO:
        // Task&lt;int&gt; - Quantidade de registros removidos.
        
        
        // Param diasParaManter: Dias de histórico a manter.
        // Returns: Quantidade de registros removidos.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ObterEstatisticasDesatualizadasAsync                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Jobs, Controllers                                          │
        // │    ➡️ CHAMA       : DbContext.ViagemEstatistica, Where, OrderBy, ToListAsync              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Buscar estatísticas desatualizadas (mais de 1 hora sem atualização).
        
        
        
        // 📤 RETORNO:
        // Task&lt;List&lt;ViagemEstatistica&gt;&gt; - Lista de estatísticas desatualizadas.
        
        
        // Returns: Lista de estatísticas desatualizadas.
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
