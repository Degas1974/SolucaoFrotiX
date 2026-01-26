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
    /// <summary>
    /// Partial class para endpoint ListaEventos otimizado com paginação server-side
    /// Arquivo: ViagemController_ListaEventos.cs
    /// Destino: /Controllers/ViagemController_ListaEventos.cs
    /// </summary>
    public partial class ViagemController : Controller
    {
        /// <summary>
        /// Lista eventos com paginação server-side - SUPER OTIMIZADO
        /// Rota: /api/viagem/listaeventos
        ///
        /// OTIMIZAÇÕES:
        /// 1. Paginação server-side - carrega apenas 25 registros por vez
        /// 2. Agrega custos apenas dos eventos da página atual
        /// 3. Queries otimizadas com AsNoTracking
        ///
        /// PERFORMANCE: < 2 segundos (ao invés de 30+ segundos timeout)
        /// </summary>
        [HttpGet]
        [Route("ListaEventos")]
        public IActionResult ListaEventos(
            int draw = 1,           // DataTables: contador de requisição
            int start = 0,          // DataTables: offset (início da página)
            int length = 25,        // DataTables: quantidade de registros por página
            int orderColumn = 1,    // DataTables: índice da coluna a ordenar (padrão: coluna 1 - Início)
            string orderDir = "desc") // DataTables: direção da ordenação (asc/desc)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                // ============================================================
                // PASSO 1: Contar total de registros (para paginação)
                // ============================================================
                var totalRecords = _context.Evento.Count();

                Console.WriteLine($"[ListaEventos] Total de eventos: {totalRecords}");

                // ============================================================
                // PASSO 2: Buscar APENAS eventos da página atual (com Include)
                // ============================================================

                // Mapeia índice da coluna para campo de ordenação
                // Colunas do DataTable: 0=nome, 1=dataInicial, 2=dataFinal, 3=qtdParticipantes,
                //                       4=nomeSetor, 5=custoViagem, 6=status (ordenável), 7=acao (não ordenável)
                IQueryable<Evento> query = _context.Evento
                    .Include(e => e.SetorSolicitante)
                    .Include(e => e.Requisitante)
                    .AsNoTracking();

                // Aplica ordenação baseada nos parâmetros do DataTables
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
                // PASSO 3: Buscar custos APENAS dos eventos da página atual
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
                // PASSO 4: Montar resultado (em memória - apenas 25 registros)
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

                // Se ordenação é pela coluna 5 (custoViagem), ordena em memória
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
                // PASSO 5: Retornar no formato DataTables server-side
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
