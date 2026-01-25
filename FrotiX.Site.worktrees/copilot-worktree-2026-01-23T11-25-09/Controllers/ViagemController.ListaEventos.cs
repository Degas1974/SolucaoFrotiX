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
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ViagemController (Partial: ListaEventos)                           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Listagem otimizada de eventos logísticos.                                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rotas: /api/Viagem/*                                                   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class ViagemController : Controller
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListaEventos (GET)                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista eventos com paginação server-side otimizada.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • draw (int): DataTables draw.                                           ║
        /// ║    • start (int): Offset.                                                   ║
        /// ║    • length (int): Tamanho da página.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON DataTables.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListaEventos")]
        public IActionResult ListaEventos(
            int draw = 1,           // DataTables: contador de requisição
            int start = 0,          // DataTables: offset (início da página)
            int length = 25)        // DataTables: quantidade de registros por página
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                // ============================================================
                // [DADOS] Contagem total (paginação).
                // ============================================================
                var totalRecords = _context.Evento.Count();

                Console.WriteLine($"[ListaEventos] Total de eventos: {totalRecords}");

                // ============================================================
                // [DADOS] Eventos da página atual.
                // ============================================================
                var eventos = _context.Evento
                    .Include(e => e.SetorSolicitante)
                    .Include(e => e.Requisitante)
                    .AsNoTracking()
                    .OrderBy(e => e.Nome)
                    .Skip(start)
                    .Take(length)
                    .ToList();

                Console.WriteLine($"[ListaEventos] Eventos da página: {sw.ElapsedMilliseconds}ms ({eventos.Count} eventos)");

                // ============================================================
                // [DADOS] Custos dos eventos da página atual.
                // ============================================================
                var eventoIds = eventos.Select(e => e.EventoId).ToList();

                var viagensDict = _context.Viagem
                    .Where(v => v.EventoId != null && eventoIds.Contains(v.EventoId.Value))
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
                // [PROCESSAMENTO] Monta payload (página).
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
                })
                .ToList();

                sw.Stop();
                Console.WriteLine($"[ListaEventos] ✅ TOTAL: {sw.ElapsedMilliseconds}ms - Página {(start / length) + 1} ({resultado.Count} de {totalRecords} eventos)");

                // ============================================================
                // [RETORNO] Formato DataTables.
                // ============================================================
                return Json(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = resultado
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
