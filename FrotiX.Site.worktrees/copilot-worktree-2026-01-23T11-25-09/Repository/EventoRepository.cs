// ╔═══════════════════════════════════════════════════════════════════════════════════════════════════╗
// ║                                                                                                   ║
// ║   ███████╗██╗   ██╗███████╗███╗   ██╗████████╗ ██████╗     ██████╗ ███████╗██████╗  ██████╗     ║
// ║   ██╔════╝██║   ██║██╔════╝████╗  ██║╚══██╔══╝██╔═══██╗    ██╔══██╗██╔════╝██╔══██╗██╔═══██╗    ║
// ║   █████╗  ██║   ██║█████╗  ██╔██╗ ██║   ██║   ██║   ██║    ██████╔╝█████╗  ██████╔╝██║   ██║    ║
// ║   ██╔══╝  ╚██╗ ██╔╝██╔══╝  ██║╚██╗██║   ██║   ██║   ██║    ██╔══██╗██╔══╝  ██╔═══╝ ██║   ██║    ║
// ║   ███████╗ ╚████╔╝ ███████╗██║ ╚████║   ██║   ╚██████╔╝    ██║  ██║███████╗██║     ╚██████╔╝    ║
// ║   ╚══════╝  ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝    ╚═════╝     ╚═╝  ╚═╝╚══════╝╚═╝      ╚═════╝     ║
// ║                                                                                                   ║
// ║   📋 ARQUIVO: EventoRepository.cs                                                                 ║
// ║   📂 LOCALIZAÇÃO: Repository/                                                                     ║
// ║   📅 DOCUMENTADO EM: 2026-01-14                                                                   ║
// ║   👤 AUTOR: GitHub Copilot (Documentação INTRA-CODE)                                              ║
// ║   ⚙️ TECNOLOGIAS: C#, .NET 10, EF Core, Repository Pattern                                       ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 📖 DESCRIÇÃO GERAL                                                                                ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ Gerencia eventos corporativos/governamentais (seminários, reuniões, deslocamentos especiais).    ║
// ║ Eventos agregam múltiplas VIAGENS e permitem rastreamento de custos consolidados.                ║
// ║                                                                                                   ║
// ║ CAMPOS PRINCIPAIS:                                                                                ║
// ║ • EventoId (PK), Nome, Descricao                                                                  ║
// ║ • DataInicial, DataFinal (período do evento)                                                      ║
// ║ • QtdParticipantes (estimativa de pessoas)                                                        ║
// ║ • Status (Planejado/Em Andamento/Finalizado/Cancelado)                                           ║
// ║ • RequisitanteId, SetorSolicitanteId (FK)                                                        ║
// ║                                                                                                   ║
// ║ RELACIONAMENTOS:                                                                                  ║
// ║ Evento (1) ──────────── (N) Viagem   → Um evento pode ter múltiplas viagens associadas          ║
// ║ Evento (N) ──────────── (1) Requisitante → Quem solicitou o evento                               ║
// ║ Evento (N) ──────────── (1) SetorSolicitante → Setor responsável                                 ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 🎯 FUNCIONALIDADES PRINCIPAIS                                                                     ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ ✅ GetEventoListForDropDown()                                                                     ║
// ║    • Retorna SelectListItem para dropdowns MVC                                                    ║
// ║    • Ordenação: Nome ASC                                                                          ║
// ║                                                                                                   ║
// ║ ✅ Update(Evento)                                                                                 ║
// ║    • ⚠️ QUEBRA UNIT OF WORK: Chama _db.Update() + SaveChanges()                                  ║
// ║    • Atualiza registro completo do evento                                                         ║
// ║                                                                                                   ║
// ║ ✅ GetEventosPaginadoAsync() ⭐ QUERY OTIMIZADA                                                   ║
// ║    • Consulta paginada com JOIN (Requisitante + SetorSolicitante)                                ║
// ║    • CÁLCULO DE CUSTOS: SUM de todos custos das viagens do evento                                ║
// ║    • Filtro por Status (opcional)                                                                 ║
// ║    • Retorna: (List<EventoListDto>, int totalItems) para paginação                                ║
// ║    • PERFORMANCE: 3 etapas otimizadas com Stopwatch (logs de performance)                        ║
// ║                                                                                                   ║
// ║ ETAPAS DE GetEventosPaginadoAsync:                                                                ║
// ║ 1. QUERY EVENTOS: JOIN com Requisitante + SetorSolicitante, paginação                            ║
// ║ 2. CUSTOS BATCH: GroupBy em Viagem para somar custos por EventoId (1 query)                      ║
// ║ 3. FORMATAÇÕES: ConvertHtml, formatação monetária, padding QtdParticipantes                      ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ ⚠️ PROBLEMAS IDENTIFICADOS                                                                        ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ ❌ PROBLEMA: Update() quebra Unit of Work                                                         ║
// ║    → Chama _db.Update() + SaveChanges() diretamente                                               ║
// ║    → Impede transações coordenadas pelo Controller/Service                                        ║
// ║    → SOLUÇÃO: Remover SaveChanges(), deixar para UnitOfWork                                       ║
// ║                                                                                                   ║
// ║ ⚠️ OBSERVAÇÃO: GetEventoListForDropDown() simples                                                 ║
// ║    → Retorna apenas EventoId + Nome                                                               ║
// ║    → Não filtra por Status (retorna TODOS os eventos, inclusive cancelados)                      ║
// ║    → SUGESTÃO: Adicionar filtro ".Where(x => x.Status != 'Cancelado')"                            ║
// ║                                                                                                   ║
// ║ ✅ BOA PRÁTICA: GetEventosPaginadoAsync com logs de performance                                   ║
// ║    → Stopwatch em cada etapa (Query, Custos, Formatações)                                        ║
// ║    → Console.WriteLine para debug (útil em desenvolvimento)                                       ║
// ║    → Tratamento de erro com Alerta.TratamentoErroComLinha                                         ║
// ║                                                                                                   ║
// ║ ✅ BOA PRÁTICA: AsNoTracking() em queries de leitura                                              ║
// ║    → Performance otimizada (não rastreia mudanças no contexto)                                    ║
// ║                                                                                                   ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║ 📊 CÁLCULO DE CUSTOS                                                                              ║
// ╠═══════════════════════════════════════════════════════════════════════════════════════════════════╣
// ║                                                                                                   ║
// ║ CustoTotal = SUM(Viagem.CustoCombustivel) +                                                       ║
// ║              SUM(Viagem.CustoMotorista) +                                                         ║
// ║              SUM(Viagem.CustoVeiculo) +                                                           ║
// ║              SUM(Viagem.CustoOperador) +                                                          ║
// ║              SUM(Viagem.CustoLavador)                                                             ║
// ║                                                                                                   ║
// ║ Agrupado por: Viagem.EventoId                                                                     ║
// ║ Filtro: WHERE EventoId IN (ids dos eventos da página)                                             ║
// ║ Performance: 1 query batch para múltiplos eventos (evita N+1)                                     ║
// ║                                                                                                   ║
// ╚═══════════════════════════════════════════════════════════════════════════════════════════════════╝

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
{
    public class EventoRepository : Repository<Evento>, IEventoRepository
    {
        private new readonly FrotiXDbContext _db;

        public EventoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        // [ETAPA] Busca eventos para popular dropdown
        // Retorna SelectListItem com EventoId (GUID) e Nome
        // Ordenação: Nome ASC
        // ⚠️ PROBLEMA: Não filtra por Status (retorna TODOS, inclusive cancelados)
        // TODO: Adicionar filtro ".Where(x => x.Status != 'Cancelado')"
        public IEnumerable<SelectListItem> GetEventoListForDropDown()
        {
            return _db.Evento
                .OrderBy(o => o.Nome)
                .Select(i => new SelectListItem()
                {
                    Text = i.Nome,
                    Value = i.EventoId.ToString()
                });
        }

        // [ETAPA] Atualiza evento existente
        // ⚠️ QUEBRA UNIT OF WORK: Chama _db.Update() + SaveChanges() diretamente
        // Atualiza registro completo (todos os campos modificados são detectados)
        // TODO: Remover SaveChanges() para respeitar Unit of Work pattern
        public new void Update(Evento evento)
        {
            _db.Update(evento);
            _db.SaveChanges();
        }

        // [ETAPA] ⚡ QUERY OTIMIZADA: Busca eventos paginados com custos consolidados
        //
        // PERFORMANCE: 3 etapas otimizadas com Stopwatch (logs de debug)
        //
        // ETAPA 1 - QUERY EVENTOS:
        // • JOIN com Requisitante + SetorSolicitante (LEFT JOIN via DefaultIfEmpty)
        // • Filtro opcional por Status
        // • Paginação: Skip + Take
        // • AsNoTracking() para performance
        //
        // ETAPA 2 - CUSTOS BATCH:
        // • GroupBy em Viagem.EventoId para somar custos
        // • SUM de: CustoCombustivel + CustoMotorista + CustoVeiculo + CustoOperador + CustoLavador
        // • WHERE Viagem.EventoId IN (ids dos eventos da página) → Evita N+1
        // • Resultado armazenado em Dictionary para lookup rápido
        //
        // ETAPA 3 - FORMATAÇÕES:
        // • ConvertHtml() no nome do requisitante (escaping de caracteres especiais)
        // • Formatação monetária: "R$ 0,00"
        // • Padding em QtdParticipantes: "001", "015", "100"
        //
        // RETORNO: (List<EventoListDto>, int totalItems) para paginação no frontend
        //
        // LOGS: Console.WriteLine em cada etapa (útil para profiling)
        //
        // TRATAMENTO DE ERROS: Try-catch com Alerta.TratamentoErroComLinha
        /// <summary>
        /// ⚡ Query otimizada para listar eventos com paginação
        /// </summary>
        public async Task<(List<EventoListDto> eventos, int totalItems)> GetEventosPaginadoAsync(
            int page,
            int pageSize,
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
                                e.EventoId,
                                e.Nome,
                                e.Descricao,
                                e.DataInicial,
                                e.DataFinal,
                                e.QtdParticipantes,
                                e.Status,
                                NomeRequisitante = req != null ? req.Nome : "",
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
                        EventoId = g.Key,
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

                var custosDict = custosPorEvento.ToDictionary(x => x.EventoId, x => x.CustoTotal);

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
                        EventoId = x.EventoId,
                        Nome = x.Nome,
                        Descricao = x.Descricao,
                        DataInicial = x.DataInicial,
                        DataFinal = x.DataFinal,
                        QtdParticipantes = (x.QtdParticipantes ?? 0).ToString().PadLeft(3, '0'),
                        Status = x.Status,
                        NomeRequisitante = x.NomeRequisitante,
                        NomeRequisitanteHTML = Servicos.ConvertHtml(x.NomeRequisitante ?? ""),
                        NomeSetor = x.NomeSetor,
                        CustoViagem = string.Format("R$ {0:N2}", custo),
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
                Alerta.TratamentoErroComLinha("EventoRepository.cs", "GetEventosPaginadoAsync", error);
                throw;
            }
        }
    }
}
