/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemController.HeatmapEconomildo.cs                            ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
    /****************************************************************************************
     * ⚡ CONTROLLER: Viagem API (Partial - HeatmapEconomildo)
     * 🎯 OBJETIVO: Gerar matriz 7×24 (dia da semana × hora) com quantidade de viagens Economildo
     * 📋 ROTAS: /api/viagem/HeatmapEconomildo [GET]
     * 🔗 ENTIDADES: ViewFluxoEconomildo (view materializada)
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     * 📊 FORMATO: Heatmap para visualização de densidade de viagens por período
     * 📝 NOTA: Classe parcial - ver ViagemController.cs principal
     ****************************************************************************************/
    public partial class ViagemController : Controller
    {
        #region Heatmap Economildo

        /****************************************************************************************
         * ⚡ FUNÇÃO: HeatmapEconomildo
         * 🎯 OBJETIVO: Gerar matriz 7×24 com contagem de viagens Economildo por dia da semana e hora
         * 📥 ENTRADAS: mob (opcional), mes (opcional), ano (opcional)
         * 📤 SAÍDAS: JSON { success, data: Array<{ diaSemana, diaIndex, horas: int[24] }>, maxValor, totalViagens }
         * 🔗 CHAMADA POR: Dashboard Economildo (gráfico heatmap)
         * 🔄 CHAMA: ViewFluxoEconomildo.GetAll()
         * 📊 LÓGICA:
         *    1. Filtra viagens por MOB/mês/ano (opcional)
         *    2. Cria matriz 7×24 (dias × horas)
         *    3. Conta viagens para cada slot dia/hora
         *    4. Retorna array com 7 objetos (um por dia) + horas[24]
         ****************************************************************************************/
        [HttpGet]
        [Route("HeatmapEconomildo")]
        public IActionResult HeatmapEconomildo(string? mob , string? mes , string? ano)
        {
            try
            {
                // [DOC] Usa view materializada ViewFluxoEconomildo para performance otimizada
                var viagens = _unitOfWork.ViewFluxoEconomildo.GetAll();

                // [DOC] Filtro por MOB (opcional)
                if (!string.IsNullOrEmpty(mob))
                {
                    viagens = viagens.Where(v => v.MOB == mob);
                }

                // [DOC] Filtro por mês (opcional)
                if (!string.IsNullOrEmpty(mes) && int.TryParse(mes , out int mesInt))
                {
                    viagens = viagens.Where(v => v.Data.HasValue && v.Data.Value.Month == mesInt);
                }

                // [DOC] Filtro por ano (opcional)
                if (!string.IsNullOrEmpty(ano) && int.TryParse(ano , out int anoInt))
                {
                    viagens = viagens.Where(v => v.Data.HasValue && v.Data.Value.Year == anoInt);
                }

                // [DOC] Materializar a query antes de processar
                var listaViagens = viagens.ToList();

                // [DOC] Cria matriz 7×24 (7 dias × 24 horas) e rastreia valor máximo para escala
                var heatmap = new int[7 , 24];
                int maxValor = 0;

                // [DOC] Popula matriz contando viagens para cada slot dia/hora
                foreach (var v in listaViagens)
                {
                    if (!v.Data.HasValue || string.IsNullOrEmpty(v.HoraInicio))
                        continue;

                    // [DOC] Converte DayOfWeek para índice (0=Segunda, 6=Domingo)
                    // DayOfWeek: Sunday=0, Monday=1, ..., Saturday=6
                    // Matriz: Segunda=0, Terça=1, ..., Domingo=6
                    int diaSemana = (int)v.Data.Value.DayOfWeek;
                    int diaIndex = diaSemana == 0 ? 6 : diaSemana - 1;

                    // [DOC] Extrai hora do campo HoraInicio (formato "HH:mm" ou "HH:mm:ss")
                    int horaIndex = 0;
                    if (TimeSpan.TryParse(v.HoraInicio , out TimeSpan horaTimeSpan))
                    {
                        horaIndex = Math.Clamp(horaTimeSpan.Hours , 0 , 23);
                    }
                    else if (v.HoraInicio.Length >= 2 && int.TryParse(v.HoraInicio.Substring(0 , 2) , out int horaInt))
                    {
                        horaIndex = Math.Clamp(horaInt , 0 , 23);
                    }

                    // [DOC] Incrementa contagem no slot [dia, hora] correspondente
                    heatmap[diaIndex , horaIndex]++;

                    // [DOC] Atualiza valor máximo para normalização de escala de cores
                    if (heatmap[diaIndex , horaIndex] > maxValor)
                        maxValor = heatmap[diaIndex , horaIndex];
                }

                // [DOC] Converte matriz para lista de objetos (um por dia) para serialização JSON
                var dados = new List<object>();
                var diasNomes = new[] { "Segunda" , "Terça" , "Quarta" , "Quinta" , "Sexta" , "Sábado" , "Domingo" };

                for (int dia = 0 ; dia < 7 ; dia++)
                {
                    var horasArray = new int[24];
                    for (int hora = 0 ; hora < 24 ; hora++)
                    {
                        horasArray[hora] = heatmap[dia , hora];
                    }

                    dados.Add(new
                    {
                        diaSemana = diasNomes[dia] ,
                        diaIndex = dia ,
                        horas = horasArray // Array de 24 posições com contagem de viagens por hora
                    });
                }

                return Json(new
                {
                    success = true ,
                    data = dados ,
                    maxValor = maxValor ,
                    totalViagens = listaViagens.Count
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs" , "HeatmapEconomildo" , error);
                return Json(new { success = false , message = error.Message });
            }
        }

        #endregion Heatmap Economildo
    }
}
