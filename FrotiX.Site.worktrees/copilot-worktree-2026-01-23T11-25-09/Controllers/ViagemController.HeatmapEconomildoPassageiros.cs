using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ViagemController (Partial: HeatmapEconomildoPassageiros)           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Mapa de calor de volume de passageiros.                                   ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rotas: /api/Viagem/*                                                   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class ViagemController
    {
        #region Heatmap Economildo Passageiros

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: HeatmapEconomildoPassageiros (GET)                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Matriz 7x24 (dia x hora) com volume de passageiros.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • mob (string?): Filtro MOB.                                             ║
        /// ║    • mes (string?): Filtro mês.                                             ║
        /// ║    • ano (string?): Filtro ano.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com matriz de heatmap.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("HeatmapEconomildoPassageiros")]
        public IActionResult HeatmapEconomildoPassageiros(string? mob, string? mes, string? ano)
        {
            try
            {
                // [DADOS] Base em ViewFluxoEconomildo.
                var viagens = _unitOfWork.ViewFluxoEconomildo.GetAll();

                // [FILTRO] MOB.
                if (!string.IsNullOrEmpty(mob))
                {
                    viagens = viagens.Where(v => v.MOB == mob);
                }

                // [FILTRO] Mês.
                if (!string.IsNullOrEmpty(mes) && int.TryParse(mes, out int mesInt))
                {
                    viagens = viagens.Where(v => v.Data.HasValue && v.Data.Value.Month == mesInt);
                }

                // [FILTRO] Ano.
                if (!string.IsNullOrEmpty(ano) && int.TryParse(ano, out int anoInt))
                {
                    viagens = viagens.Where(v => v.Data.HasValue && v.Data.Value.Year == anoInt);
                }

                // [DADOS] Materializa query.
                var listaViagens = viagens.ToList();

                // [CALCULO] Matriz 7x24 (passageiros).
                var heatmap = new int[7, 24];
                int maxValor = 0;

                foreach (var v in listaViagens)
                {
                    if (!v.Data.HasValue || string.IsNullOrEmpty(v.HoraInicio))
                        continue;

                    // Converter DayOfWeek para índice (0=Segunda, 6=Domingo)
                    int diaSemana = (int)v.Data.Value.DayOfWeek;
                    int diaIndex = diaSemana == 0 ? 6 : diaSemana - 1; // Sunday(0) vai para 6

                    // Extrair hora do campo HoraInicio (formato esperado: "HH:mm" ou "HH:mm:ss")
                    int horaIndex = 0;
                    if (TimeSpan.TryParse(v.HoraInicio, out TimeSpan horaTimeSpan))
                    {
                        horaIndex = Math.Clamp(horaTimeSpan.Hours, 0, 23);
                    }
                    else if (v.HoraInicio.Length >= 2 && int.TryParse(v.HoraInicio.Substring(0, 2), out int horaInt))
                    {
                        horaIndex = Math.Clamp(horaInt, 0, 23);
                    }

                    // Somar passageiros ao invés de contar viagens
                    heatmap[diaIndex, horaIndex] += v.QtdPassageiros ?? 0;

                    if (heatmap[diaIndex, horaIndex] > maxValor)
                        maxValor = heatmap[diaIndex, horaIndex];
                }

                // [RETORNO] Monta payload.
                var dados = new List<object>();
                var diasNomes = new[] { "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo" };

                for (int dia = 0; dia < 7; dia++)
                {
                    var horasArray = new int[24];
                    for (int hora = 0; hora < 24; hora++)
                    {
                        horasArray[hora] = heatmap[dia, hora];
                    }

                    dados.Add(new
                    {
                        diaSemana = diasNomes[dia],
                        diaIndex = dia,
                        horas = horasArray
                    });
                }

                return Json(new
                {
                    success = true,
                    data = dados,
                    maxValor = maxValor,
                    totalPassageiros = listaViagens.Sum(v => v.QtdPassageiros ?? 0)
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "HeatmapEconomildoPassageiros", error);
                return Json(new { success = false, message = error.Message });
            }
        }

        #endregion Heatmap Economildo Passageiros
    }
}
