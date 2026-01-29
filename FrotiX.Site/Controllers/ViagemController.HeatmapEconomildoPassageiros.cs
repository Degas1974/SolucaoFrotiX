/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViagemController.HeatmapEconomildoPassageiros.cs                                        ║
   ║ 📂 CAMINHO: /Controllers                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Partial para Heatmap de PASSAGEIROS do Economildo. Retorna matriz 7x24 com SOMA       ║
   ║    de passageiros por dia/hora. Diferente do HeatmapEconomildo que conta VIAGENS.                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ENDPOINTS: [GET] /api/Viagem/HeatmapEconomildoPassageiros → Matriz de passageiros               ║
   ║    PARAMS: mob, mes, ano | DADOS: data[7], horas[24], maxValor, totalPassageiros                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: IUnitOfWork (ViewFluxoEconomildo), ViagemController                                        ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
    public partial class ViagemController
    {
        #region Heatmap Economildo Passageiros

        /****************************************************************************************
         * ⚡ FUNÇÃO: HeatmapEconomildoPassageiros
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Gerar matriz 7x24 com SOMA de passageiros por dia da semana e hora
         *                   Diferente do HeatmapEconomildo que conta VIAGENS, este soma PASSAGEIROS
         * 📥 ENTRADAS     : [string?] mob - Filtra por MOB específica
         *                   [string?] mes - Filtra por mês (1-12)
         *                   [string?] ano - Filtra por ano
         * 📤 SAÍDAS       : [IActionResult] JSON com matriz heatmap, maxValor e totalPassageiros
         * 🔗 CHAMADA POR  : Dashboard Economildo (JavaScript)
         * 🔄 CHAMA        : ViewFluxoEconomildo.GetAll()
         *
         * 📊 ESTRUTURA DO RETORNO:
         *    - data[7]: Array com 7 dias (Segunda=0 a Domingo=6)
         *    - horas[24]: Array com soma de passageiros por hora (0-23)
         *    - maxValor: Valor máximo para escala de cores do heatmap
         *    - totalPassageiros: Total geral de passageiros no período filtrado
         ****************************************************************************************/
        [HttpGet]
        [Route("HeatmapEconomildoPassageiros")]
        public IActionResult HeatmapEconomildoPassageiros(string? mob, string? mes, string? ano)
        {
            try
            {
                // [DOC] STEP 1: Buscar dados da View otimizada de fluxo Economildo
                var viagens = _unitOfWork.ViewFluxoEconomildo.GetAll();

                // [DOC] STEP 2: Aplicar filtros opcionais
                // Filtro por MOB (Mobilização específica)
                if (!string.IsNullOrEmpty(mob))
                {
                    viagens = viagens.Where(v => v.MOB == mob);
                }

                // Filtro por mês (1-12)
                if (!string.IsNullOrEmpty(mes) && int.TryParse(mes, out int mesInt))
                {
                    viagens = viagens.Where(v => v.Data.HasValue && v.Data.Value.Month == mesInt);
                }

                // Filtro por ano
                if (!string.IsNullOrEmpty(ano) && int.TryParse(ano, out int anoInt))
                {
                    viagens = viagens.Where(v => v.Data.HasValue && v.Data.Value.Year == anoInt);
                }

                // [DOC] STEP 3: Materializar query (executa SQL)
                var listaViagens = viagens.ToList();

                // [DOC] STEP 4: Criar matriz 7x24 (dias x horas) - SOMA DE PASSAGEIROS
                // Índices: [0]=Segunda, [1]=Terça, ..., [6]=Domingo
                // Horas: [0]=00:00, [1]=01:00, ..., [23]=23:00
                var heatmap = new int[7, 24];
                int maxValor = 0;

                foreach (var v in listaViagens)
                {
                    // [DOC] Ignora registros sem data ou hora de início
                    if (!v.Data.HasValue || string.IsNullOrEmpty(v.HoraInicio))
                        continue;

                    // [DOC] Converter DayOfWeek para índice (0=Segunda, 6=Domingo)
                    // .NET: Sunday=0, Monday=1, ..., Saturday=6
                    // Nosso: Segunda=0, Terça=1, ..., Domingo=6
                    int diaSemana = (int)v.Data.Value.DayOfWeek;
                    int diaIndex = diaSemana == 0 ? 6 : diaSemana - 1; // Sunday(0) vai para índice 6

                    // [DOC] Extrair hora do campo HoraInicio (formato esperado: "HH:mm" ou "HH:mm:ss")
                    int horaIndex = 0;
                    if (TimeSpan.TryParse(v.HoraInicio, out TimeSpan horaTimeSpan))
                    {
                        horaIndex = Math.Clamp(horaTimeSpan.Hours, 0, 23);
                    }
                    else if (v.HoraInicio.Length >= 2 && int.TryParse(v.HoraInicio.Substring(0, 2), out int horaInt))
                    {
                        horaIndex = Math.Clamp(horaInt, 0, 23);
                    }

                    // [DOC] DIFERENÇA PRINCIPAL: Somar passageiros ao invés de contar viagens
                    heatmap[diaIndex, horaIndex] += v.QtdPassageiros ?? 0;

                    // [DOC] Atualizar valor máximo para escala de cores
                    if (heatmap[diaIndex, horaIndex] > maxValor)
                        maxValor = heatmap[diaIndex, horaIndex];
                }

                // [DOC] STEP 5: Converter matriz para lista de objetos JSON
                var dados = new List<object>();
                var diasNomes = new[] { "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo" };

                // [DOC] STEP 6: Montar estrutura final para cada dia da semana
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

                // [DOC] STEP 7: Retornar JSON com dados, valor máximo e total
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
                // [DOC] Tratamento de erro padronizado com log
                Alerta.TratamentoErroComLinha("ViagemController.cs", "HeatmapEconomildoPassageiros", error);
                return Json(new { success = false, message = error.Message });
            }
        }

        #endregion Heatmap Economildo Passageiros
    }
}
