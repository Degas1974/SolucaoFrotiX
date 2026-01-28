/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemController.ListaEventos.cs                                 ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Viagem API (Partial - ListaEventos)
     * 🎯 OBJETIVO: Listar eventos com paginação server-side DataTables (SUPER OTIMIZADO)
     * 📋 ROTAS: /api/viagem/ListaEventos [GET]
     * 🔗 ENTIDADES: Evento, SetorSolicitante, Requisitante, Viagem
     * 📦 DEPENDÊNCIAS: ApplicationDbContext (acesso direto ao EF Core)
     * ⚡ PERFORMANCE: <2s (antes: 30+ segundos com timeout)
     * 📊 OTIMIZAÇÕES:
     *    - Paginação server-side (25 registros por página)
     *    - AsNoTracking para queries read-only
     *    - Custos agregados apenas para eventos da página atual
     *    - Ordenação dinâmica por coluna (DataTables)
     * 📝 NOTA: Classe parcial - ver ViagemController.cs principal
     ****************************************************************************************/
    public partial class ViagemController : Controller
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaEventos
         * 🎯 OBJETIVO: Listar eventos com paginação server-side e custos agregados (DataTables)
         * 📥 ENTRADAS:
         *    - draw: Contador de requisição DataTables (controle de sincronização)
         *    - start: Offset de registros (início da página)
         *    - length: Quantidade de registros por página (padrão: 25)
         *    - orderColumn: Índice da coluna para ordenação (0-6)
         *    - orderDir: Direção da ordenação ("asc" ou "desc")
         * 📤 SAÍDAS: JSON DataTables { draw, recordsTotal, recordsFiltered, data: Array<EventoDTO> }
         * 🔗 CHAMADA POR: DataTable de eventos (frontend com paginação)
         * 🔄 CHAMA: Evento.Include(), Viagem.GroupBy() (custos), AsNoTracking()
         * ⚡ PERFORMANCE: <2 segundos (antes: 30+ segundos com timeout)
         * 📊 ALGORITMO (5 passos):
         *    1. Contar total de eventos
         *    2. Buscar APENAS eventos da página atual (Skip + Take)
         *    3. Calcular custos APENAS dos eventos da página
         *    4. Montar resultado em memória (apenas 25 registros)
         *    5. Retornar formato DataTables server-side
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaEventos")]
        public IActionResult ListaEventos(
            int draw = 1,
            int start = 0,
            int length = 25,
            int orderColumn = 1,
            string orderDir = "desc")
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                // [DOC] ============================================================
                // [DOC] PASSO 1: Contar total de registros (para paginação DataTables)
                // [DOC] ============================================================
                var totalRecords = _context.Evento.Count();

                Console.WriteLine($"[ListaEventos] Total de eventos: {totalRecords}");

                // [DOC] ============================================================
                // [DOC] PASSO 2: Buscar APENAS eventos da página atual (Skip + Take)
                // [DOC] ============================================================

                // [DOC] Mapeia índice da coluna DataTables para campo de ordenação no banco
                // Colunas do DataTable: 0=nome, 1=dataInicial, 2=dataFinal, 3=qtdParticipantes,
                //                       4=nomeSetor, 5=custoViagem (em memória), 6=status, 7=acao (não ordenável)
                // [DOC] Cria query base com AsNoTracking (read-only, sem tracking de mudanças)
                IQueryable<Evento> query = _context.Evento
                    .Include(e => e.SetorSolicitante)
                    .Include(e => e.Requisitante)
                    .AsNoTracking();

                // [DOC] Aplica ordenação dinâmica baseada nos parâmetros do DataTables
                query = orderColumn switch
                {
                    0 => orderDir == "asc" ? query.OrderBy(e => e.Nome) : query.OrderByDescending(e => e.Nome),
                    1 => orderDir == "asc" ? query.OrderBy(e => e.DataInicial) : query.OrderByDescending(e => e.DataInicial),
                    2 => orderDir == "asc" ? query.OrderBy(e => e.DataFinal) : query.OrderByDescending(e => e.DataFinal),
                    3 => orderDir == "asc" ? query.OrderBy(e => e.QtdParticipantes) : query.OrderByDescending(e => e.QtdParticipantes),
                    4 => orderDir == "asc" ? query.OrderBy(e => e.SetorSolicitante.Nome) : query.OrderByDescending(e => e.SetorSolicitante.Nome),
                    // Coluna 5 (custoViagem) será ordenada em memória após carregar os dados
                    6 => orderDir == "asc" ? query.OrderBy(e => e.Status) : query.OrderByDescending(e => e.Status),
                    // Coluna 7 não é ordenável (orderable: false no DataTable)
                    _ => orderDir == "asc" ? query.OrderBy(e => e.DataInicial) : query.OrderByDescending(e => e.DataInicial) // padrão: coluna 1
                };

                var eventos = query
                    .Skip(start)
                    .Take(length)
                    .ToList();

                Console.WriteLine($"[ListaEventos] Eventos da página: {sw.ElapsedMilliseconds}ms ({eventos.Count} eventos)");

                // [DOC] ============================================================
                // [DOC] PASSO 3: Calcular custos APENAS dos eventos da página atual
                // [DOC] ============================================================
                var eventoIds = eventos.Select(e => e.EventoId).ToList();

                // [DOC] Agrega custos apenas para os eventos da página atual (GroupBy + Sum)
                // Inclui soma de 5 componentes de custo + contagem de viagens
                var viagensDict = _context.Viagem
                    .Where(v => v.EventoId != null && eventoIds.Contains(v.EventoId.Value) && v.Status == "Realizada")
                    .AsNoTracking()
                    .GroupBy(v => v.EventoId)
                    .Select(g => new
                    {
                        EventoId = g.Key,
                        CustoTotal = g.Sum(v =>
                            (v.CustoCombustivel ?? 0) +
                            (v.CustoMotorista ?? 0) +
                            (v.CustoVeiculo ?? 0) +
                            (v.CustoOperador ?? 0) +
                            (v.CustoLavador ?? 0)),
                        ViagensCount = g.Count()
                    })
                    .ToDictionary(x => x.EventoId, x => new { Custo = Math.Round(x.CustoTotal, 2), Viagens = x.ViagensCount });

                Console.WriteLine($"[ListaEventos] Custos calculados: {sw.ElapsedMilliseconds}ms ({viagensDict.Count} eventos com viagens)");

                // [DOC] ============================================================
                // [DOC] PASSO 4: Montar resultado DTO (em memória - apenas 25 registros)
                // [DOC] ============================================================
                var resultado = eventos.Select(e =>
                {
                    string nomeSetor = "";
                    if (e.SetorSolicitante != null)
                    {
                        nomeSetor = !string.IsNullOrEmpty(e.SetorSolicitante.Sigla)
                            ? $"{e.SetorSolicitante.Nome} ({e.SetorSolicitante.Sigla})"
                            : e.SetorSolicitante.Nome ?? "";
                    }

                    double custoViagem = 0;
                    int viagensCount = 0;
                    if (viagensDict.TryGetValue(e.EventoId, out var viagemInfo))
                    {
                        custoViagem = viagemInfo.Custo;
                        viagensCount = viagemInfo.Viagens;
                    }

                    return new
                    {
                        eventoId = e.EventoId,
                        nome = e.Nome ?? "",
                        descricao = e.Descricao ?? "",
                        dataInicial = e.DataInicial,
                        dataFinal = e.DataFinal,
                        qtdParticipantes = e.QtdParticipantes,
                        status = e.Status == "1" ? 1 : 0,
                        nomeSetor = nomeSetor,
                        nomeRequisitante = e.Requisitante?.Nome ?? "",
                        nomeRequisitanteHTML = e.Requisitante?.Nome ?? "",
                        custoViagem = custoViagem,
                        viagensCount = viagensCount
                    };
                });

                // [DOC] Se ordenação é pela coluna 5 (custoViagem), ordena em memória
                // (não pode ser feito no SQL pois é campo calculado)
                if (orderColumn == 5)
                {
                    resultado = orderDir == "asc"
                        ? resultado.OrderBy(r => r.custoViagem)
                        : resultado.OrderByDescending(r => r.custoViagem);
                }

                var resultadoFinal = resultado.ToList();

                sw.Stop();
                Console.WriteLine($"[ListaEventos] ✅ TOTAL: {sw.ElapsedMilliseconds}ms - Página {(start / length) + 1} ({resultadoFinal.Count} de {totalRecords} eventos) - Ordenado por coluna {orderColumn} ({orderDir})");

                // [DOC] ============================================================
                // [DOC] PASSO 5: Retornar no formato DataTables server-side processing
                // [DOC] ============================================================
                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = resultadoFinal
                });
            }
            catch (Exception error)
            {
                sw.Stop();
                Console.WriteLine($"[ListaEventos] ❌ ERRO após {sw.ElapsedMilliseconds}ms: {error.Message}");
                Alerta.TratamentoErroComLinha("ViagemController.cs", "ListaEventos", error);

                return Json(new
                {
                    draw = draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<object>(),
                    error = "Erro ao carregar eventos: " + error.Message
                });
            }
        }
    }
}
