/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemController.DashboardEconomildo.cs                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Viagem API (Partial - DashboardEconomildo)
     * 🎯 OBJETIVO: Dashboard analítico completo com 15+ métricas do sistema Economildo
     * 📋 ROTAS: /api/viagem/DashboardEconomildo [GET]
     * 🔗 ENTIDADES: ViagensEconomildo (view), ViewVeiculos
     * 📦 DEPENDÊNCIAS: IUnitOfWork, ApplicationDbContext
     * 📊 MÉTRICAS: Total usuários/viagens, médias (mensal/diária), análises temporais, comparativos por MOB
     * 📝 NOTA: Classe parcial - ver ViagemController.cs principal
     ****************************************************************************************/
    public partial class ViagemController
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: DashboardEconomildo
         * 🎯 OBJETIVO: Retornar 15+ métricas analíticas do sistema Economildo (passageiros, viagens, médias, comparativos)
         * 📥 ENTRADAS: mob (opcional: "PGR", "Rodoviaria", "Cefor"), mes (1-12), ano
         * 📤 SAÍDAS: JSON com 27 propriedades analíticas
         * 🔗 CHAMADA POR: Dashboard Economildo (frontend)
         * 🔄 CHAMA: ViagensEconomildo (view), ViewVeiculos, métodos helper
         * 📊 MÉTRICAS RETORNADAS:
         *    - Totais gerais: totalUsuarios, totalViagens, mediaMensal, mediaDiaria
         *    - Por MOB: totalPGR/Rodoviaria/Cefor, mediaMensalPGR/Rodoviaria/Cefor
         *    - Tempos médios: tempoMedioIda/Volta por MOB
         *    - Distribuições: usuariosPorMes, usuariosPorTurno, usuariosPorDiaSemana, usuariosPorHora
         *    - Comparativos: comparativoMob (mensal), topVeiculos (top 10)
         ****************************************************************************************/
        [HttpGet]
        [Route("DashboardEconomildo")]
        public IActionResult DashboardEconomildo(string? mob, int? mes, int? ano)
        {
            try
            {
                // [DOC] ========== ETAPA 1: Filtra viagens Economildo base (com filtros opcionais) ==========
                var query = _context.ViagensEconomildo.AsQueryable();

                // [DOC] Filtro por MOB (PGR, Rodoviaria, Cefor)
                if (!string.IsNullOrEmpty(mob))
                {
                    query = query.Where(v => v.MOB == mob);
                }

                // [DOC] Filtro por mês
                if (mes.HasValue && mes.Value > 0)
                {
                    query = query.Where(v => v.Data.HasValue && v.Data.Value.Month == mes.Value);
                }

                // [DOC] Filtro por ano
                if (ano.HasValue && ano.Value > 0)
                {
                    query = query.Where(v => v.Data.HasValue && v.Data.Value.Year == ano.Value);
                }

                var viagens = query.ToList();

                // [DOC] ========== ETAPA 2: Cálculos gerais (totais e médias) ==========
                // Total de passageiros (usuários) transportados
                var totalUsuarios = viagens.Sum(v => v.QtdPassageiros ?? 0);
                var totalViagens = viagens.Count;

                // [DOC] Média mensal: total passageiros / quantidade meses distintos
                var mesesDistintos = viagens
                    .Where(v => v.Data.HasValue)
                    .Select(v => new { v.Data.Value.Year, v.Data.Value.Month })
                    .Distinct()
                    .Count();

                var mediaMensal = mesesDistintos > 0 ? (double)totalUsuarios / mesesDistintos : 0;

                // [DOC] Média diária: total passageiros / quantidade dias distintos
                var diasDistintos = viagens
                    .Where(v => v.Data.HasValue)
                    .Select(v => v.Data.Value.Date)
                    .Distinct()
                    .Count();

                var mediaDiaria = diasDistintos > 0 ? (double)totalUsuarios / diasDistintos : 0;

                // [DOC] ========== ETAPA 3: Totais e médias por MOB (sem filtro MOB para comparar todos) ==========
                var queryTodos = _context.ViagensEconomildo.AsQueryable();

                if (mes.HasValue && mes.Value > 0)
                {
                    queryTodos = queryTodos.Where(v => v.Data.HasValue && v.Data.Value.Month == mes.Value);
                }

                if (ano.HasValue && ano.Value > 0)
                {
                    queryTodos = queryTodos.Where(v => v.Data.HasValue && v.Data.Value.Year == ano.Value);
                }

                var viagensTodos = queryTodos.ToList();

                var viagensPGR = viagensTodos.Where(v => v.MOB == "PGR").ToList();
                var viagensRodoviaria = viagensTodos.Where(v => v.MOB == "Rodoviaria").ToList();
                var viagensCefor = viagensTodos.Where(v => v.MOB == "Cefor").ToList();

                var totalPGR = viagensPGR.Sum(v => v.QtdPassageiros ?? 0);
                var totalRodoviaria = viagensRodoviaria.Sum(v => v.QtdPassageiros ?? 0);
                var totalCefor = viagensCefor.Sum(v => v.QtdPassageiros ?? 0);

                // Medias mensais por MOB
                var mesesPGR = viagensPGR.Where(v => v.Data.HasValue).Select(v => new { v.Data.Value.Year, v.Data.Value.Month }).Distinct().Count();
                var mesesRodoviaria = viagensRodoviaria.Where(v => v.Data.HasValue).Select(v => new { v.Data.Value.Year, v.Data.Value.Month }).Distinct().Count();
                var mesesCefor = viagensCefor.Where(v => v.Data.HasValue).Select(v => new { v.Data.Value.Year, v.Data.Value.Month }).Distinct().Count();

                var mediaMensalPGR = mesesPGR > 0 ? (double)totalPGR / mesesPGR : 0;
                var mediaMensalRodoviaria = mesesRodoviaria > 0 ? (double)totalRodoviaria / mesesRodoviaria : 0;
                var mediaMensalCefor = mesesCefor > 0 ? (double)totalCefor / mesesCefor : 0;

                // [DOC] ========== ETAPA 4: Tempos médios de IDA e VOLTA (geral + por MOB) ==========
                var tempoMedioIda = CalcularMediaDuracao(viagens, true);
                var tempoMedioVolta = CalcularMediaDuracao(viagens, false);

                // [DOC] Tempo médio por MOB (PGR, Rodoviaria, Cefor)
                var tempoMedioIdaPGR = CalcularMediaDuracao(viagensPGR, true);
                var tempoMedioVoltaPGR = CalcularMediaDuracao(viagensPGR, false);
                var tempoMedioIdaRodoviaria = CalcularMediaDuracao(viagensRodoviaria, true);
                var tempoMedioVoltaRodoviaria = CalcularMediaDuracao(viagensRodoviaria, false);
                var tempoMedioIdaCefor = CalcularMediaDuracao(viagensCefor, true);
                var tempoMedioVoltaCefor = CalcularMediaDuracao(viagensCefor, false);

                // [DOC] ========== ETAPA 5: Distribuições temporais (mês, turno, dia semana, hora) ==========
                // Passageiros por mês
                var usuariosPorMes = viagens
                    .Where(v => v.Data.HasValue)
                    .GroupBy(v => v.Data.Value.Month)
                    .Select(g => new
                    {
                        mesNum = g.Key,
                        mes = ObterNomeMes(g.Key),
                        total = g.Sum(v => v.QtdPassageiros ?? 0)
                    })
                    .OrderBy(x => x.mesNum)
                    .ToList();

                // [DOC] Passageiros por turno (Manhã 6-12h, Tarde 12-18h, Noite 18-6h)
                var usuariosPorTurno = new
                {
                    manha = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Manha").Sum(v => v.QtdPassageiros ?? 0),
                    tarde = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Tarde").Sum(v => v.QtdPassageiros ?? 0),
                    noite = viagens.Where(v => ClassificarTurno(v.HoraInicio) == "Noite").Sum(v => v.QtdPassageiros ?? 0)
                };

                // [DOC] ========== ETAPA 6: Comparativos e rankings ==========
                // Comparativo mensal por MOB (grafico multi-linhas)
                var comparativoMob = viagensTodos
                    .Where(v => v.Data.HasValue)
                    .GroupBy(v => v.Data.Value.Month)
                    .Select(g => new
                    {
                        mesNum = g.Key,
                        mes = ObterNomeMes(g.Key),
                        rodoviaria = g.Where(v => v.MOB == "Rodoviaria").Sum(v => v.QtdPassageiros ?? 0),
                        pgr = g.Where(v => v.MOB == "PGR").Sum(v => v.QtdPassageiros ?? 0),
                        cefor = g.Where(v => v.MOB == "Cefor").Sum(v => v.QtdPassageiros ?? 0)
                    })
                    .OrderBy(x => x.mesNum)
                    .ToList();

                // [DOC] Passageiros por dia da semana (exclui sábado e domingo)
                var usuariosPorDiaSemana = viagens
                    .Where(v => v.Data.HasValue)
                    .GroupBy(v => v.Data.Value.DayOfWeek)
                    .Where(g => g.Key != DayOfWeek.Saturday && g.Key != DayOfWeek.Sunday)
                    .Select(g => new
                    {
                        diaNum = (int)g.Key,
                        dia = ObterNomeDiaSemana(g.Key),
                        total = g.Sum(v => v.QtdPassageiros ?? 0)
                    })
                    .OrderBy(x => x.diaNum == 0 ? 7 : x.diaNum)
                    .ToList();

                // [DOC] Passageiros por hora (00:00 a 23:00)
                var usuariosPorHora = viagens
                    .Where(v => !string.IsNullOrEmpty(v.HoraInicio))
                    .GroupBy(v => ExtrairHora(v.HoraInicio))
                    .Where(g => g.Key >= 0)
                    .Select(g => new
                    {
                        horaNum = g.Key,
                        hora = g.Key.ToString("00") + ":00",
                        total = g.Sum(v => v.QtdPassageiros ?? 0)
                    })
                    .OrderBy(x => x.horaNum)
                    .ToList();

                // [DOC] Top 10 veículos mais usados (ranking por quantidade de viagens)
                var topVeiculos = viagens
                    .Where(v => v.VeiculoId != Guid.Empty)
                    .GroupBy(v => v.VeiculoId)
                    .Select(g => new
                    {
                        veiculoId = g.Key,
                        total = g.Count()
                    })
                    .OrderByDescending(x => x.total)
                    .Take(10)
                    .ToList();

                // Buscar placas dos veiculos
                var veiculoIds = topVeiculos.Select(v => v.veiculoId).ToList();
                var veiculos = _unitOfWork.ViewVeiculos
                    .GetAll(v => veiculoIds.Contains(v.VeiculoId))
                    .ToDictionary(v => v.VeiculoId, v => v.Placa ?? "S/N");

                var topVeiculosComPlaca = topVeiculos
                    .Select(v => new
                    {
                        placa = veiculos.ContainsKey(v.veiculoId) ? veiculos[v.veiculoId] : "S/N",
                        total = v.total
                    })
                    .ToList();

                return Json(new
                {
                    success = true,
                    totalUsuarios,
                    totalViagens,
                    mediaMensal,
                    mediaDiaria,
                    totalPGR,
                    totalRodoviaria,
                    totalCefor,
                    mediaMensalPGR,
                    mediaMensalRodoviaria,
                    mediaMensalCefor,
                    tempoMedioIda,
                    tempoMedioVolta,
                    tempoMedioIdaPGR,
                    tempoMedioVoltaPGR,
                    tempoMedioIdaRodoviaria,
                    tempoMedioVoltaRodoviaria,
                    tempoMedioIdaCefor,
                    tempoMedioVoltaCefor,
                    usuariosPorMes,
                    usuariosPorTurno,
                    comparativoMob,
                    usuariosPorDiaSemana,
                    usuariosPorHora,
                    topVeiculos = topVeiculosComPlaca
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "DashboardEconomildo", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar dashboard: " + error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: EhIda (HELPER)
         * 🎯 OBJETIVO: Verificar se viagem é do tipo IDA
         * 📥 ENTRADAS: idaVolta (string: "IDA", "I", ou variações)
         * 📤 SAÍDAS: bool (true se é IDA)
         ****************************************************************************************/
        private bool EhIda(string? idaVolta)
        {
            try
            {
                if (string.IsNullOrEmpty(idaVolta)) return false;
                var tipo = idaVolta.Trim().ToUpper();
                return tipo == "IDA" || tipo == "I";
            }
            catch
            {
                return false;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: EhVolta (HELPER)
         * 🎯 OBJETIVO: Verificar se viagem é do tipo VOLTA
         * 📥 ENTRADAS: idaVolta (string: "VOLTA", "V", ou variações)
         * 📤 SAÍDAS: bool (true se é VOLTA)
         ****************************************************************************************/
        private bool EhVolta(string? idaVolta)
        {
            try
            {
                if (string.IsNullOrEmpty(idaVolta)) return false;
                var tipo = idaVolta.Trim().ToUpper();
                return tipo == "VOLTA" || tipo == "V";
            }
            catch
            {
                return false;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularMediaDuracao (HELPER)
         * 🎯 OBJETIVO: Calcular média de duração de viagens (IDA ou VOLTA)
         * 📥 ENTRADAS: viagens (lista), ehIda (bool: true=IDA, false=VOLTA)
         * 📤 SAÍDAS: string formatada "X min"
         * 📊 LÓGICA: Filtra por tipo (IDA/VOLTA) e calcula média do campo Duracao
         ****************************************************************************************/
        private string CalcularMediaDuracao(List<ViagensEconomildo> viagens, bool ehIda)
        {
            try
            {
                var viagensFiltradas = viagens
                    .Where(v => v.Duracao.HasValue && v.Duracao > 0 && (ehIda ? EhIda(v.IdaVolta) : EhVolta(v.IdaVolta)))
                    .ToList();

                if (!viagensFiltradas.Any()) return "0 min";

                return Math.Round(viagensFiltradas.Average(v => v.Duracao.Value), 0).ToString() + " min";
            }
            catch
            {
                return "0 min";
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ClassificarTurno (HELPER)
         * 🎯 OBJETIVO: Classificar turno com base na hora de início
         * 📥 ENTRADAS: horaInicio (string TimeSpan)
         * 📤 SAÍDAS: string ("Manha", "Tarde", "Noite")
         * 📊 REGRAS: Manhã 6-12h, Tarde 12-18h, Noite 18-6h
         ****************************************************************************************/
        private string ClassificarTurno(string? horaInicio)
        {
            try
            {
                if (string.IsNullOrEmpty(horaInicio)) return "Manha";

                if (TimeSpan.TryParse(horaInicio, out var hora))
                {
                    if (hora.Hours >= 6 && hora.Hours < 12) return "Manha";
                    if (hora.Hours >= 12 && hora.Hours < 18) return "Tarde";
                    return "Noite";
                }

                return "Manha";
            }
            catch
            {
                return "Manha";
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ExtrairHora (HELPER)
         * 🎯 OBJETIVO: Extrair hora (0-23) de string TimeSpan
         * 📥 ENTRADAS: horaStr (string formato TimeSpan)
         * 📤 SAÍDAS: int (0-23, ou -1 se inválido)
         ****************************************************************************************/
        private int ExtrairHora(string? horaStr)
        {
            try
            {
                if (string.IsNullOrEmpty(horaStr)) return -1;

                if (TimeSpan.TryParse(horaStr, out var hora))
                {
                    return hora.Hours;
                }

                return -1;
            }
            catch
            {
                return -1;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ObterNomeMes (HELPER)
         * 🎯 OBJETIVO: Converter número do mês (1-12) para nome abreviado
         * 📥 ENTRADAS: mes (int de 1 a 12)
         * 📤 SAÍDAS: string abreviada ("Jan", "Fev", ..., "Dez")
         * 📊 ARRAY: ["", "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
         ****************************************************************************************/
        private string ObterNomeMes(int mes)
        {
            try
            {
                var nomes = new[] { "", "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };
                return mes >= 1 && mes <= 12 ? nomes[mes] : "";
            }
            catch
            {
                return "";
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ObterNomeDiaSemana (HELPER)
         * 🎯 OBJETIVO: Converter DayOfWeek enum para nome abreviado do dia da semana
         * 📥 ENTRADAS: dia (DayOfWeek enum)
         * 📤 SAÍDAS: string abreviada ("Seg", "Ter", "Qua", "Qui", "Sex", "Sab", "Dom")
         * 📊 SWITCH: Monday→Seg, Tuesday→Ter, Wednesday→Qua, Thursday→Qui, Friday→Sex, Saturday→Sab, Sunday→Dom
         ****************************************************************************************/
        private string ObterNomeDiaSemana(DayOfWeek dia)
        {
            try
            {
                return dia switch
                {
                    DayOfWeek.Monday => "Seg",
                    DayOfWeek.Tuesday => "Ter",
                    DayOfWeek.Wednesday => "Qua",
                    DayOfWeek.Thursday => "Qui",
                    DayOfWeek.Friday => "Sex",
                    DayOfWeek.Saturday => "Sab",
                    DayOfWeek.Sunday => "Dom",
                    _ => ""
                };
            }
            catch
            {
                return "";
            }
        }
    }
}
