/* ****************************************************************************************
 * ⚡ ARQUIVO: ViagemController.AtualizarDados.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Atualizar dados de viagens e fornecer detalhes para ajuste.
 *
 * 📥 ENTRADAS     : IDs de viagem e DTOs de atualização.
 *
 * 📤 SAÍDAS       : JSON com dados e mensagens de status.
 *
 * 🔗 CHAMADA POR  : Página AjustaCustosViagem.
 *
 * 🔄 CHAMA        : IUnitOfWork.Viagem.
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER PARTIAL: ViagemController.AtualizarDados
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Implementar endpoints de consulta e atualização de dados de viagem.
     *
     * 📥 ENTRADAS     : IDs e DTOs de ajuste.
     *
     * 📤 SAÍDAS       : JSON com dados detalhados.
     ****************************************************************************************/
    public partial class ViagemController
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: GetViagem
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar dados completos de uma viagem pelo ID.
         *
         * 📥 ENTRADAS     : id (Guid da viagem).
         *
         * 📤 SAÍDAS       : JSON com success e dados da viagem.
         *
         * 🔗 CHAMADA POR  : GET /api/Viagem/GetViagem/{id}.
         ****************************************************************************************/
        [Route("GetViagem/{id}")]
        [HttpGet]
        public IActionResult GetViagem(Guid id)
        {
            try
            {
                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == id);

                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Viagem não encontrada"
                    });
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        viagemId = viagem.ViagemId,
                        noFichaVistoria = viagem.NoFichaVistoria,
                        finalidade = viagem.Finalidade,
                        eventoId = viagem.EventoId,
                        dataInicial = viagem.DataInicial?.ToString("yyyy-MM-dd"),
                        horaInicio = viagem.HoraInicio?.ToString("HH:mm"),
                        dataFinal = viagem.DataFinal?.ToString("yyyy-MM-dd"),
                        horaFim = viagem.HoraFim?.ToString("HH:mm"),
                        kmInicial = viagem.KmInicial,
                        kmFinal = viagem.KmFinal,
                        motoristaId = viagem.MotoristaId,
                        veiculoId = viagem.VeiculoId,
                        setorSolicitanteId = viagem.SetorSolicitanteId,
                        requisitanteId = viagem.RequisitanteId,
                        ramalRequisitante = viagem.RamalRequisitante
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "GetViagem", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao buscar viagem: " + error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: AtualizarDadosViagem
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Atualizar campos da viagem a partir de ajustes manuais.
         *
         * 📥 ENTRADAS     : request (AtualizarDadosViagemRequest).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : POST /api/Viagem/AtualizarDadosViagem.
         *
         * 🔄 CHAMA        : Viagem.GetFirstOrDefault(), Viagem.Update(), UnitOfWork.Save().
         ****************************************************************************************/
        [Route("AtualizarDadosViagem")]
        [HttpPost]
        public IActionResult AtualizarDadosViagem([FromBody] AtualizarDadosViagemRequest request)
        {
            try
            {
                if (request == null || request.ViagemId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Dados inválidos"
                    });
                }

                var viagem = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == request.ViagemId);

                if (viagem == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Viagem não encontrada"
                    });
                }

                // Atualizar campos
                if (request.NoFichaVistoria.HasValue)
                {
                    viagem.NoFichaVistoria = request.NoFichaVistoria.Value;
                }

                if (!string.IsNullOrEmpty(request.Finalidade))
                {
                    viagem.Finalidade = request.Finalidade;
                }

                if (request.EventoId.HasValue)
                {
                    viagem.EventoId = request.EventoId.Value;
                }
                else if (request.Finalidade != "Evento")
                {
                    viagem.EventoId = null;
                }

                // Datas e Horas
                if (request.DataInicial.HasValue)
                {
                    viagem.DataInicial = request.DataInicial.Value;
                }

                if (request.HoraInicio.HasValue)
                {
                    viagem.HoraInicio = request.HoraInicio.Value;
                }

                if (request.DataFinal.HasValue)
                {
                    viagem.DataFinal = request.DataFinal.Value;
                }

                if (request.HoraFim.HasValue)
                {
                    viagem.HoraFim = request.HoraFim.Value;
                }

                // Quilometragem
                if (request.KmInicial.HasValue)
                {
                    viagem.KmInicial = request.KmInicial.Value;
                }

                if (request.KmFinal.HasValue)
                {
                    viagem.KmFinal = request.KmFinal.Value;
                }

                // Motorista, Veículo e Setor
                if (request.MotoristaId.HasValue)
                {
                    viagem.MotoristaId = request.MotoristaId.Value;
                }

                if (request.VeiculoId.HasValue)
                {
                    viagem.VeiculoId = request.VeiculoId.Value;
                }

                if (request.SetorSolicitanteId.HasValue)
                {
                    viagem.SetorSolicitanteId = request.SetorSolicitanteId.Value;
                }

                // Requisitante e Ramal
                if (request.RequisitanteId.HasValue)
                {
                    viagem.RequisitanteId = request.RequisitanteId.Value;
                }

                if (!string.IsNullOrEmpty(request.RamalRequisitante))
                {
                    viagem.RamalRequisitante = request.RamalRequisitante;
                }

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
                Alerta.TratamentoErroComLinha("ViagemController.cs", "AtualizarDadosViagem", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao atualizar viagem: " + error.Message
                });
            }
        }
    }

    /****************************************************************************************
     * ⚡ DTO: AtualizarDadosViagemRequest
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar dados de ajuste da viagem.
     *
     * 📥 ENTRADAS     : Campos de ficha, datas, horários, km e vínculos.
     *
     * 📤 SAÍDAS       : Nenhuma (apenas transporte de dados).
     *
     * 🔗 CHAMADA POR  : AtualizarDadosViagem.
     ****************************************************************************************/
    public class AtualizarDadosViagemRequest
    {
        public Guid ViagemId { get; set; }
        public int? NoFichaVistoria { get; set; }
        public string Finalidade { get; set; }
        public Guid? EventoId { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public DateTime? HoraFim { get; set; }
        public int? KmInicial { get; set; }
        public int? KmFinal { get; set; }
        public Guid? MotoristaId { get; set; }
        public Guid? VeiculoId { get; set; }
        public Guid? SetorSolicitanteId { get; set; }
        public Guid? RequisitanteId { get; set; }
        public string RamalRequisitante { get; set; }
    }
}
