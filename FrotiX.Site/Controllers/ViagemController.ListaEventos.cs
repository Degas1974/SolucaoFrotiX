/* ****************************************************************************************
 * ⚡ ARQUIVO: ViagemController.ListaEventos.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Listar eventos com paginação server-side para DataTables.
 *
 * 📥 ENTRADAS     : draw, start, length, orderColumn, orderDir.
 *
 * 📤 SAÍDAS       : JSON no formato DataTables.
 *
 * 🔗 CHAMADA POR  : Grid de eventos.
 *
 * 🔄 CHAMA        : FrotiXDbContext.Evento/Viagem (AsNoTracking).
 **************************************************************************************** */

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
     * ⚡ CONTROLLER PARTIAL: ViagemController.ListaEventos
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Implementar listagem otimizada de eventos.
     *
     * 📥 ENTRADAS     : Parâmetros do DataTables.
     *
     * 📤 SAÍDAS       : JSON paginado.
     ****************************************************************************************/
    public partial class ViagemController : Controller
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaEventos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Lista eventos com paginação server-side SUPER OTIMIZADO
         *                   Carrega apenas 25 registros por vez para melhor performance
         * 📥 ENTRADAS     : [int] draw - Contador de requisição (DataTables)
         *                   [int] start - Offset/início da página (0, 25, 50...)
         *                   [int] length - Quantidade por página (padrão: 25)
         *                   [int] orderColumn - Índice da coluna (0-6)
         *                   [string] orderDir - Direção (asc/desc)
         * 📤 SAÍDAS       : [IActionResult] JSON formato DataTables (draw, recordsTotal, data)
         * 🔗 CHAMADA POR  : JavaScript (DataTables) da página de Eventos via AJAX
         * 🔄 CHAMA        : DbContext.Evento, DbContext.Viagem
         *
         * ⚡ PERFORMANCE:
         *    - < 2 segundos (vs 30+ segundos timeout versão anterior)
         *    - Paginação server-side evita carregar todos os registros
         *    - Custos calculados apenas para eventos da página atual
         *    - AsNoTracking para queries de leitura
         *
         * 📊 COLUNAS ORDENÁVEIS:
         *    0=Nome, 1=DataInicial, 2=DataFinal, 3=QtdParticipantes
         *    4=NomeSetor, 5=CustoViagem(em memória), 6=Status
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaEventos")]
        public IActionResult ListaEventos(
            int draw = 1,           // [DOC] DataTables: contador de requisição para sincronização
            int start = 0,          // [DOC] DataTables: offset (início da página)
            int length = 25,        // [DOC] DataTables: quantidade de registros por página
            int orderColumn = 1,    // [DOC] DataTables: índice da coluna a ordenar (padrão: coluna 1 - Início)
            string orderDir = "desc") // [DOC] DataTables: direção da ordenação (asc/desc)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                // ============================================================
                // [DOC] PASSO 1: Contar total de registros (para paginação)
                // ============================================================
                var totalRecords = _context.Evento.Count();

                Console.WriteLine($"[ListaEventos] Total de eventos: {totalRecords}");

                // ============================================================
                // [DOC] PASSO 2: Buscar APENAS eventos da página atual (com Include)
                // Mapeia índice da coluna para campo de ordenação
                // Colunas do DataTable: 0=nome, 1=dataInicial, 2=dataFinal, 3=qtdParticipantes,
                //                       4=nomeSetor, 5=custoViagem, 6=status (ordenável), 7=acao (não ordenável)
                // ============================================================
                IQueryable<Evento> query = _context.Evento
                    .Include(e => e.SetorSolicitante)
                    .Include(e => e.Requisitante)
                    .AsNoTracking();

                // [DOC] Aplica ordenação baseada nos parâmetros do DataTables (switch expression)
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

                // ============================================================
                // [DOC] PASSO 3: Buscar custos APENAS dos eventos da página atual
                // Otimização: Evita calcular custos de todos os eventos do banco
                // ============================================================
                var eventoIds = eventos.Select(e => e.EventoId).ToList();

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

                // ============================================================
                // [DOC] PASSO 4: Montar resultado (em memória - apenas 25 registros)
                // Projeção dos dados para o formato esperado pelo DataTables
                // ============================================================
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
                // porque custoViagem é calculado, não está no banco
                if (orderColumn == 5)
                {
                    resultado = orderDir == "asc"
                        ? resultado.OrderBy(r => r.custoViagem)
                        : resultado.OrderByDescending(r => r.custoViagem);
                }

                var resultadoFinal = resultado.ToList();

                sw.Stop();
                Console.WriteLine($"[ListaEventos] ✅ TOTAL: {sw.ElapsedMilliseconds}ms - Página {(start / length) + 1} ({resultadoFinal.Count} de {totalRecords} eventos) - Ordenado por coluna {orderColumn} ({orderDir})");

                // ============================================================
                // [DOC] PASSO 5: Retornar no formato DataTables server-side
                // Formato: { draw, recordsTotal, recordsFiltered, data }
                // ============================================================
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
