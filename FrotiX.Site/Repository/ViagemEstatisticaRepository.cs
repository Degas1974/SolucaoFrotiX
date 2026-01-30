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
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: ViagemEstatisticaRepository                                                        │
    /// │ 📦 HERDA DE: Repository<ViagemEstatistica>                                                    │
    /// │ 🔌 IMPLEMENTA: IViagemEstatisticaRepository                                                   │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável por estatísticas de viagens agregadas por data.
    /// Disponibiliza consultas e rotinas de manutenção de histórico.
    /// </summary>
    public class ViagemEstatisticaRepository : Repository<ViagemEstatistica>, IViagemEstatisticaRepository
    {
        private readonly FrotiXDbContext _context;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViagemEstatisticaRepository                                                   │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UnitOfWork, Services, Jobs                                            │
        /// │    ➡️ CHAMA       : base(context)                                                        │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Inicializar o repositório com o contexto do banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    context - Contexto do banco de dados da aplicação.
        /// </para>
        /// </summary>
        /// <param name="context">Instância de <see cref="FrotiXDbContext"/>.</param>
        public ViagemEstatisticaRepository(FrotiXDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ObterPorDataAsync                                                            │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Jobs, Controllers                                          │
        /// │    ➡️ CHAMA       : DbContext.ViagemEstatistica, AsNoTracking, FirstOrDefaultAsync        │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Buscar estatística por data de referência (ignora horário).
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    dataReferencia - Data de referência para consulta.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;ViagemEstatistica&gt; - Estatística encontrada ou null.
        /// </para>
        /// </summary>
        /// <param name="dataReferencia">Data de referência.</param>
        /// <returns>Estatística encontrada ou null.</returns>
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ObterPorPeriodoAsync                                                         │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Jobs, Controllers                                          │
        /// │    ➡️ CHAMA       : DbContext.ViagemEstatistica, Where, OrderBy, ToListAsync              │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Buscar estatísticas dentro de um período de datas (inclusivo).
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    dataInicio - Data inicial do período<br/>
        ///    dataFim - Data final do período
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;List&lt;ViagemEstatistica&gt;&gt; - Lista de estatísticas do período.
        /// </para>
        /// </summary>
        /// <param name="dataInicio">Data inicial.</param>
        /// <param name="dataFim">Data final.</param>
        /// <returns>Lista de estatísticas do período.</returns>
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ExisteParaDataAsync                                                          │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Jobs, Controllers                                          │
        /// │    ➡️ CHAMA       : DbContext.ViagemEstatistica, AnyAsync                                 │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Verificar se existe estatística registrada para uma data.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    dataReferencia - Data de referência para consulta.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;bool&gt; - True se existir estatística, senão false.
        /// </para>
        /// </summary>
        /// <param name="dataReferencia">Data de referência.</param>
        /// <returns>Indicador de existência.</returns>
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: RemoverEstatisticasAntigasAsync                                               │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Jobs, Controllers                                          │
        /// │    ➡️ CHAMA       : DbContext.ViagemEstatistica, RemoveRange, SaveChangesAsync            │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Remover estatísticas antigas (mais de X dias).
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    diasParaManter - Quantidade de dias a preservar no histórico.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;int&gt; - Quantidade de registros removidos.
        /// </para>
        /// </summary>
        /// <param name="diasParaManter">Dias de histórico a manter.</param>
        /// <returns>Quantidade de registros removidos.</returns>
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ObterEstatisticasDesatualizadasAsync                                          │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Jobs, Controllers                                          │
        /// │    ➡️ CHAMA       : DbContext.ViagemEstatistica, Where, OrderBy, ToListAsync              │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Buscar estatísticas desatualizadas (mais de 1 hora sem atualização).
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;List&lt;ViagemEstatistica&gt;&gt; - Lista de estatísticas desatualizadas.
        /// </para>
        /// </summary>
        /// <returns>Lista de estatísticas desatualizadas.</returns>
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
