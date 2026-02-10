/* ****************************************************************************************
 * ⚡ ARQUIVO: ViagemController.AtualizarDadosViagem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Atualizar dados de viagem via Dashboard e recalcular campos normalizados.
 *
 * 📥 ENTRADAS     : DTOs de ajuste do dashboard.
 *
 * 📤 SAÍDAS       : JSON com status da operação.
 *
 * 🔗 CHAMADA POR  : Modal do Dashboard de viagens.
 *
 * 🔄 CHAMA        : IUnitOfWork.Viagem, cálculo de minutos normalizados.
 **************************************************************************************** */

using FrotiX.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER PARTIAL: ViagemController.AtualizarDadosViagem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Implementar atualização de viagem via dashboard.
     *
     * 📥 ENTRADAS     : DTOs de ajuste.
     *
     * 📤 SAÍDAS       : JSON com sucesso/erro.
     ****************************************************************************************/
    public partial class ViagemController
    {
        // Constantes para cálculo de jornada
        private const int MINUTOS_JORNADA_DIA = 480; // 8 horas
        private static readonly TimeSpan INICIO_EXPEDIENTE = new TimeSpan(8, 0, 0);  // 08:00
        private static readonly TimeSpan FIM_EXPEDIENTE = new TimeSpan(18, 0, 0);    // 18:00

        /****************************************************************************************
         * ⚡ DTO: AtualizarViagemDashboardDTO
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Transportar dados do modal de ajuste do dashboard.
         *
         * 📥 ENTRADAS     : Campos de ficha, datas, horas, km e vínculos.
         *
         * 📤 SAÍDAS       : Nenhuma (apenas transporte de dados).
         *
         * 🔗 CHAMADA POR  : AtualizarDadosViagemDashboard.
         ****************************************************************************************/
        public class AtualizarViagemDashboardDTO
        {
            public Guid ViagemId { get; set; }
            public int? NoFichaVistoria { get; set; }
            public string Finalidade { get; set; }
            public Guid? EventoId { get; set; }
            public string DataInicial { get; set; }
            public string HoraInicio { get; set; }
            public string DataFinal { get; set; }
            public string HoraFim { get; set; }
            public int? KmInicial { get; set; }
            public int? KmFinal { get; set; }
            public Guid? MotoristaId { get; set; }
            public Guid? VeiculoId { get; set; }
            public Guid? SetorSolicitanteId { get; set; }
            public Guid? RequisitanteId { get; set; }
            public string RamalRequisitante { get; set; }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: CalcularMinutosNormalizadoComJornada (Helper)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Calcular minutos normalizados com jornada de 8h/dia.
         *
         * 📥 ENTRADAS     : dataInicial, dataFinal, horaInicio, horaFim.
         *
         * 📤 SAÍDAS       : Total de minutos normalizados.
         *
         * 📝 OBSERVAÇÕES  : Mesmo dia limita a 480 min; múltiplos dias soma início, meio e fim.
         ****************************************************************************************/
        private int CalcularMinutosNormalizadoComJornada(DateTime dataInicial, DateTime dataFinal, TimeSpan horaInicio, TimeSpan horaFim)
        {
            try
            {
                int totalDias = (dataFinal.Date - dataInicial.Date).Days + 1;

                // Mesmo dia
                if (totalDias == 1)
                {
                    int minutosDia = (int)(horaFim - horaInicio).TotalMinutes;
                    return Math.Min(Math.Max(minutosDia, 0), MINUTOS_JORNADA_DIA);
                }

                int totalMinutos = 0;

                // Primeiro dia: de HoraInicio até FIM_EXPEDIENTE (18:00), limitado a 480
                int minutosPrimeiroDia = (int)(FIM_EXPEDIENTE - horaInicio).TotalMinutes;
                minutosPrimeiroDia = Math.Min(Math.Max(minutosPrimeiroDia, 0), MINUTOS_JORNADA_DIA);
                totalMinutos += minutosPrimeiroDia;

                // Dias intermediários: 480 minutos cada
                int diasIntermediarios = totalDias - 2;
                if (diasIntermediarios > 0)
                {
                    totalMinutos += diasIntermediarios * MINUTOS_JORNADA_DIA;
                }

                // Último dia: de INICIO_EXPEDIENTE (08:00) até HoraFim, limitado a 480
                int minutosUltimoDia = (int)(horaFim - INICIO_EXPEDIENTE).TotalMinutes;
                minutosUltimoDia = Math.Min(Math.Max(minutosUltimoDia, 0), MINUTOS_JORNADA_DIA);
                totalMinutos += minutosUltimoDia;

                return totalMinutos;
            }
            catch
            {
                return 0;
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AtualizarDadosViagemDashboard
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Atualizar dados de viagem via modal do dashboard.
         *
         * 📥 ENTRADAS     : dados (AtualizarViagemDashboardDTO).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : POST /api/Viagem/AtualizarDadosViagemDashboard.
         *
         * 📝 OBSERVAÇÕES  : Trigger tr_Viagem_CalculaCustos recalcula custos no banco.
         ****************************************************************************************/
        [Route("AtualizarDadosViagemDashboard")]
        [HttpPost]
        public IActionResult AtualizarDadosViagemDashboard([FromBody] AtualizarViagemDashboardDTO dados)
        {
            try
            {
                if (dados == null || dados.ViagemId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Dados inválidos ou ID da viagem não informado"
                    });
                }

                // Busca a viagem
                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == dados.ViagemId);
                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Viagem não encontrada"
                    });
                }

                // Atualiza campos básicos
                viagem.NoFichaVistoria = dados.NoFichaVistoria;
                viagem.Finalidade = dados.Finalidade;
                viagem.EventoId = dados.EventoId;

                // Variáveis para cálculo dos campos normalizados
                DateTime? dataInicialDt = null;
                DateTime? dataFinalDt = null;
                TimeSpan? horaInicioTs = null;
                TimeSpan? horaFimTs = null;

                // Atualiza datas
                if (!string.IsNullOrEmpty(dados.DataInicial))
                {
                    if (DateTime.TryParse(dados.DataInicial, out DateTime dtInicial))
                    {
                        viagem.DataInicial = dtInicial;
                        viagem.DataInicialNormalizada = dtInicial;
                        dataInicialDt = dtInicial;
                    }
                }

                if (!string.IsNullOrEmpty(dados.DataFinal))
                {
                    if (DateTime.TryParse(dados.DataFinal, out DateTime dtFinal))
                    {
                        viagem.DataFinal = dtFinal;
                        viagem.DataFinalNormalizada = dtFinal;
                        dataFinalDt = dtFinal;
                    }
                }

                // Atualiza horas
                if (!string.IsNullOrEmpty(dados.HoraInicio))
                {
                    if (TimeSpan.TryParse(dados.HoraInicio, out TimeSpan horaInicio))
                    {
                        viagem.HoraInicio = DateTime.Today.Add(horaInicio);
                        viagem.HoraInicioNormalizada = horaInicio;
                        horaInicioTs = horaInicio;
                    }
                }

                if (!string.IsNullOrEmpty(dados.HoraFim))
                {
                    if (TimeSpan.TryParse(dados.HoraFim, out TimeSpan horaFim))
                    {
                        viagem.HoraFim = DateTime.Today.Add(horaFim);
                        viagem.HoraFimNormalizada = horaFim;
                        horaFimTs = horaFim;
                    }
                }

                // Atualiza quilometragem
                viagem.KmInicial = dados.KmInicial;
                viagem.KmFinal = dados.KmFinal;

                // Atualiza campos normalizados de KM
                viagem.KmInicialNormalizado = dados.KmInicial;
                viagem.KmFinalNormalizado = dados.KmFinal;

                // Calcula KmRodadoNormalizado
                if (dados.KmFinal.HasValue && dados.KmInicial.HasValue)
                {
                    int kmRodado = dados.KmFinal.Value - dados.KmInicial.Value;
                    viagem.KmRodadoNormalizado = kmRodado > 0 ? kmRodado : 0;
                }
                else
                {
                    viagem.KmRodadoNormalizado = 0;
                }

                // ================================================================
                // CALCULA MinutosNormalizado COM JORNADA DE 8H/DIA (Opção B)
                // ================================================================
                if (dataInicialDt.HasValue && dataFinalDt.HasValue && 
                    horaInicioTs.HasValue && horaFimTs.HasValue)
                {
                    viagem.MinutosNormalizado = CalcularMinutosNormalizadoComJornada(
                        dataInicialDt.Value,
                        dataFinalDt.Value,
                        horaInicioTs.Value,
                        horaFimTs.Value
                    );
                }
                else if (viagem.DataInicial.HasValue && viagem.DataFinal.HasValue &&
                         viagem.HoraInicioNormalizada.HasValue && viagem.HoraFimNormalizada.HasValue)
                {
                    // Fallback usando campos já existentes
                    viagem.MinutosNormalizado = CalcularMinutosNormalizadoComJornada(
                        viagem.DataInicial.Value,
                        viagem.DataFinal.Value,
                        viagem.HoraInicioNormalizada.Value,
                        viagem.HoraFimNormalizada.Value
                    );
                }
                else
                {
                    viagem.MinutosNormalizado = 0;
                }

                // Atualiza relacionamentos
                viagem.MotoristaId = dados.MotoristaId;
                viagem.VeiculoId = dados.VeiculoId;
                viagem.SetorSolicitanteId = dados.SetorSolicitanteId;
                viagem.RequisitanteId = dados.RequisitanteId;
                viagem.RamalRequisitante = dados.RamalRequisitante;

                // Salva - O TRIGGER tr_Viagem_CalculaCustos vai recalcular automaticamente
                _unitOfWork.Viagem.Update(viagem);
                _unitOfWork.Save();

                return Json(new
                {
                    success = true,
                    message = "Viagem atualizada com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "AtualizarDadosViagemDashboard", error);
                return Json(new
                {
                    success = false,
                    message = $"Erro ao atualizar viagem: {error.Message}"
                });
            }
        }
    }
}
