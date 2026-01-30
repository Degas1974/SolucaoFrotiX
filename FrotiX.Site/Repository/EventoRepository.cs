/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EventoRepository.cs                                                                    ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para eventos/solenidades com consultas paginadas e cálculo de custos.              ║
   ║    Inclui listagens para UI e atualização direta da entidade Evento.                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • EventoRepository(FrotiXDbContext db)                                                          ║
   ║    • GetEventoListForDropDown()                                                                    ║
   ║    • Update(Evento evento)                                                                         ║
   ║    • GetEventosPaginadoAsync(int page, int pageSize, string filtroStatus = null)                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Custos são calculados em batch (GroupBy) para evitar N+1 queries.                               ║
   ║    AsNoTracking é aplicado em consultas de leitura e há logs com Stopwatch.                       ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Repository
{
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: EventoRepository                                                                  │
    /// │ 📦 HERDA DE: Repository<Evento>                                                              │
    /// │ 🔌 IMPLEMENTA: IEventoRepository                                                             │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável por eventos/solenidades.
    /// Fornece paginação otimizada e cálculo agregado de custos.
    /// </summary>
    public class EventoRepository :Repository<Evento>, IEventoRepository
    {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: EventoRepository                                                             │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        /// │    ➡️ CHAMA       : base(db)                                                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Inicializar o repositório com o contexto do banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    db - Contexto do banco de dados da aplicação.
        /// </para>
        /// </summary>
        /// <param name="db">Instância de <see cref="FrotiXDbContext"/>.</param>
        public EventoRepository(FrotiXDbContext db) : base(db)
        {
        _db = db;
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetEventoListForDropDown                                                    │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.Evento, OrderBy, Select                                    │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista de eventos para composição de dropdowns.
        ///    Ordena pelo nome do evento.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para eventos.</returns>
        public IEnumerable<SelectListItem> GetEventoListForDropDown()
        {
        return _db.Evento
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
            {
                Text = i.Nome ,
                Value = i.EventoId.ToString()
            });
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : _db.Update, _db.SaveChanges                                           │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualizar os dados de um evento no banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    evento - Entidade contendo os dados atualizados.
        /// </para>
        /// </summary>
        /// <param name="evento">Entidade <see cref="Evento"/> com dados atualizados.</param>
        public new void Update(Evento evento)
        {
        _db.Update(evento);
        _db.SaveChanges();
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetEventosPaginadoAsync                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : DbContext.Evento, Viagem, GroupBy, AsNoTracking, Stopwatch           │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Buscar eventos com paginação e cálculo de custo total por evento.
        ///    Usa JOINs com requisitante/setor e calcula custos em batch.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    page - Página atual (1-based)<br/>
        ///    pageSize - Quantidade de registros por página<br/>
        ///    filtroStatus - Filtro opcional por status
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    Task&lt;(List&lt;EventoListDto&gt; eventos, int totalItems)&gt; - Eventos paginados e total.
        /// </para>
        /// </summary>
        /// <param name="page">Página atual (1-based).</param>
        /// <param name="pageSize">Tamanho da página.</param>
        /// <param name="filtroStatus">Filtro opcional por status.</param>
        /// <returns>Eventos paginados e total de itens.</returns>
        public async Task<(List<EventoListDto> eventos, int totalItems)> GetEventosPaginadoAsync(
            int page ,
            int pageSize ,
            string filtroStatus = null
        )
        {
        try
        {
        var swTotal = System.Diagnostics.Stopwatch.StartNew();

        Console.WriteLine("=== INÍCIO GetEventosPaginadoAsync ===");

        // ====================================
        // ETAPA 1: Buscar dados dos eventos
        // ====================================
        var swEventos = System.Diagnostics.Stopwatch.StartNew();

        var query = from e in _db.Evento
                    join r in _db.Requisitante on e.RequisitanteId equals r.RequisitanteId into reqJoin
                    from req in reqJoin.DefaultIfEmpty()
                    join s in _db.SetorSolicitante on e.SetorSolicitanteId equals s.SetorSolicitanteId into setorJoin
                    from setor in setorJoin.DefaultIfEmpty()
                    select new
                    {
                        e.EventoId ,
                        e.Nome ,
                        e.Descricao ,
                        e.DataInicial ,
                        e.DataFinal ,
                        e.QtdParticipantes ,
                        e.Status ,
                        NomeRequisitante = req != null ? req.Nome : "" ,
                        NomeSetor = setor != null ? setor.Nome : ""
                    };

        // Aplicar filtro de status se fornecido
        if (!string.IsNullOrEmpty(filtroStatus))
        {
        query = query.Where(x => x.Status == filtroStatus);
        }

        // Count total
        var totalItems = await query.CountAsync();

        if (totalItems == 0)
        {
        Console.WriteLine("=== FIM (sem dados) ===\n");
        return (new List<EventoListDto>(), 0);
        }

        // Paginação
        var eventos = await query
            .OrderByDescending(x => x.DataInicial)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        swEventos.Stop();
        Console.WriteLine($"[QUERY EVENTOS] {eventos.Count}/{totalItems} registros - {swEventos.ElapsedMilliseconds}ms");

        // ====================================
        // ETAPA 2: Calcular custos (batch)
        // ====================================
        var swCustos = System.Diagnostics.Stopwatch.StartNew();

        var eventoIds = eventos.Select(x => x.EventoId).ToList();

        var custosPorEvento = await _db.Viagem
            .Where(v => eventoIds.Contains(v.EventoId.Value))
            .GroupBy(v => v.EventoId.Value)
            .Select(g => new
            {
                EventoId = g.Key ,
                CustoTotal = (decimal)(
                    g.Sum(v => (double)(v.CustoCombustivel ?? 0)) +
                    g.Sum(v => (double)(v.CustoMotorista ?? 0)) +
                    g.Sum(v => (double)(v.CustoVeiculo ?? 0)) +
                    g.Sum(v => (double)(v.CustoOperador ?? 0)) +
                    g.Sum(v => (double)(v.CustoLavador ?? 0))
                )
            })
            .AsNoTracking()
            .ToListAsync();

        var custosDict = custosPorEvento.ToDictionary(x => x.EventoId , x => x.CustoTotal);

        swCustos.Stop();
        Console.WriteLine($"[CUSTOS] {custosPorEvento.Count} eventos com custos - {swCustos.ElapsedMilliseconds}ms");

        // ====================================
        // ETAPA 3: Processar formatações
        // ====================================
        var swFormato = System.Diagnostics.Stopwatch.StartNew();

        var result = eventos.Select(x =>
        {
        var custo = custosDict.ContainsKey(x.EventoId) ? custosDict[x.EventoId] : 0;

        return new EventoListDto
        {
            EventoId = x.EventoId ,
            Nome = x.Nome ,
            Descricao = x.Descricao ,
            DataInicial = x.DataInicial ,
            DataFinal = x.DataFinal ,
            QtdParticipantes = (x.QtdParticipantes ?? 0).ToString().PadLeft(3 , '0') ,
            Status = x.Status ,
            NomeRequisitante = x.NomeRequisitante ,
            NomeRequisitanteHTML = Servicos.ConvertHtml(x.NomeRequisitante ?? "") ,
            NomeSetor = x.NomeSetor ,
            CustoViagem = string.Format("R$ {0:N2}" , custo) ,
            CustoViagemNaoFormatado = custo
        };
        }).ToList();

        swFormato.Stop();
        Console.WriteLine($"[FORMATO] {result.Count} registros - {swFormato.ElapsedMilliseconds}ms");

        swTotal.Stop();
        Console.WriteLine($"[TOTAL REPOSITORY] {swTotal.ElapsedMilliseconds}ms");
        Console.WriteLine("=== FIM GetEventosPaginadoAsync ===\n");

        return (result, totalItems);
        }
        catch (Exception error)
        {
        Console.WriteLine($"[ERRO REPOSITORY] {error.Message}");
        Console.WriteLine($"[STACK] {error.StackTrace}");
        Alerta.TratamentoErroComLinha("EventoRepository.cs" , "GetEventosPaginadoAsync" , error);
        throw;
        }
        }
    }
}
