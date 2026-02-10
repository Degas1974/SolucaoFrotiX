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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: EventoRepository                                                                  │
    // │ 📦 HERDA DE: Repository                                                              │
    // │ 🔌 IMPLEMENTA: IEventoRepository                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável por eventos/solenidades.
    // Fornece paginação otimizada e cálculo agregado de custos.
    
    public class EventoRepository :Repository<Evento>, IEventoRepository
    {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: EventoRepository                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public EventoRepository(FrotiXDbContext db) : base(db)
        {
        _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetEventoListForDropDown                                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.Evento, OrderBy, Select                                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de eventos para composição de dropdowns.
        // Ordena pelo nome do evento.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para eventos.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : _db.Update, _db.SaveChanges                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de um evento no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // evento - Entidade contendo os dados atualizados.
        
        
        // Param evento: Entidade <see cref="Evento"/> com dados atualizados.
        public new void Update(Evento evento)
        {
        _db.Update(evento);
        _db.SaveChanges();
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetEventosPaginadoAsync                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : DbContext.Evento, Viagem, GroupBy, AsNoTracking, Stopwatch           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Buscar eventos com paginação e cálculo de custo total por evento.
        // Usa JOINs com requisitante/setor e calcula custos em batch.
        
        
        
        // 📥 PARÂMETROS:
        // page - Página atual (1-based)
        // pageSize - Quantidade de registros por página
        // filtroStatus - Filtro opcional por status
        
        
        
        // 📤 RETORNO:
        // Task&lt;(List&lt;EventoListDto&gt; eventos, int totalItems)&gt; - Eventos paginados e total.
        
        
        // Param page: Página atual (1-based).
        // Param pageSize: Tamanho da página.
        // Param filtroStatus: Filtro opcional por status.
        // Returns: Eventos paginados e total de itens.
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

        // [LOGICA] Query complexa com JOINs LEFT OUTER (DefaultIfEmpty):
        // 1. Join Evento + Requisitante (left outer: alguns eventos podem não ter requisitante)
        // 2. Join Evento + SetorSolicitante (left outer: alguns eventos podem não ter setor)
        // 3. Select: Projeção com coalescing de nomes (usa string vazia se null)
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
                        // [LOGICA] Coalescing: usa Nome se requisitante existir, senão string vazia
                        NomeRequisitante = req != null ? req.Nome : "" ,
                        // [LOGICA] Coalescing: usa Nome se setor existir, senão string vazia
                        NomeSetor = setor != null ? setor.Nome : ""
                    };

        // [LOGICA] Aplicar filtro de status se fornecido (filtra por coluna string)
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

        // [PERFORMANCE] Extrair IDs dos eventos paginados para filtro IN
        var eventoIds = eventos.Select(x => x.EventoId).ToList();

        // [LOGICA] Agregação complexa: GroupBy evento + Sum 5 tipos de custo
        // 1. Where: Filtrar viagens dos eventos paginados (evitar N+1 queries)
        // 2. GroupBy: Agrupar por EventoId
        // 3. Select: Calcular soma de 5 componentes de custo por evento (com casts double/decimal)
        // [PERFORMANCE] Conversão para double+decimal necessária para agregação SQL
        var custosPorEvento = await _db.Viagem
            .Where(v => eventoIds.Contains(v.EventoId.Value))
            .GroupBy(v => v.EventoId.Value)
            .Select(g => new
            {
                EventoId = g.Key ,
                // [LOGICA] Soma de 5 componentes: combustível, motorista, veículo, operador, lavador
                // Cada componente pode ser null, então usa ?? 0 (coalescing com zero)
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
