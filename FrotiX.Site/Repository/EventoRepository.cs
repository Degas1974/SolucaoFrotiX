// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : EventoRepository.cs                                             ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Evento. Gerencia eventos/solenidades ║
// ║ com queries otimizadas para listagem paginada e cálculo de custos.           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetEventoListForDropDown() → SelectList ordenada por nome                  ║
// ║ • GetEventosPaginadoAsync() → Query otimizada com JOIN e cálculo de custos   ║
// ║ • Update() → Atualização direta da entidade Evento                           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ OTIMIZAÇÕES                                                                  ║
// ║ • Custos calculados em batch por GroupBy ao invés de N+1 queries              ║
// ║ • AsNoTracking() para read-only operations                                   ║
// ║ • Logs de performance com Stopwatch para diagnóstico                         ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
    public class EventoRepository :Repository<Evento>, IEventoRepository
    {
        private new readonly FrotiXDbContext _db;

        public EventoRepository(FrotiXDbContext db) : base(db)
        {
        _db = db;
        }

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

        public new void Update(Evento evento)
        {
        _db.Update(evento);
        _db.SaveChanges();
        }

        /// <summary>
        /// ⚡ Query otimizada para listar eventos com paginação
        /// </summary>
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
