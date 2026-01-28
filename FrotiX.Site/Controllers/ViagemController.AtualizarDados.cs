/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemController.AtualizarDados.cs                               ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Viagem API (Partial - AtualizarDados)
     * 🎯 OBJETIVO: Buscar e atualizar dados de viagem (usado na página de ajustes)
     * 📋 ROTAS: /api/Viagem/GetViagem/{id}, /api/Viagem/AtualizarDadosViagem [POST]
     * 🔗 ENTIDADES: Viagem
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     * 🛠️ FUNCIONALIDADE: Interface de ajuste manual de dados de viagens
     * 📝 NOTA: Classe parcial - ver ViagemController.cs principal
     ****************************************************************************************/
    public partial class ViagemController
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: GetViagem
         * 🎯 OBJETIVO: Buscar dados completos de uma viagem pelo ID
         * 📥 ENTRADAS: id (Guid da viagem)
         * 📤 SAÍDAS: JSON { success, data: ViagemDTO com todos os campos }
         * 🔗 CHAMADA POR: Página de ajuste de dados de viagens (carregamento do formulário)
         * 🔄 CHAMA: Viagem.GetFirstOrDefault()
         * 📋 CAMPOS: NoFichaVistoria, Finalidade, EventoId, Data/Hora, Km, Motorista, Veículo, Setor, Requisitante
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
         * 🎯 OBJETIVO: Atualizar dados de uma viagem (atualização parcial de campos)
         * 📥 ENTRADAS: AtualizarDadosViagemRequest (todos os campos opcionais)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Página de ajuste de dados de viagens (salvamento do formulário)
         * 🔄 CHAMA: Viagem.GetFirstOrDefault(), Viagem.Update()
         * 📝 LÓGICA: Atualiza apenas os campos que foram informados (HasValue / !IsNullOrEmpty)
         * ⚙️ REGRA: Se Finalidade != "Evento", limpa EventoId
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

                // [DOC] Atualiza campos básicos (NoFichaVistoria, Finalidade, EventoId)
                if (request.NoFichaVistoria.HasValue)
                {
                    viagem.NoFichaVistoria = request.NoFichaVistoria.Value;
                }

                if (!string.IsNullOrEmpty(request.Finalidade))
                {
                    viagem.Finalidade = request.Finalidade;
                }

                // [DOC] Regra de negócio: Se finalidade não é "Evento", remove EventoId
                if (request.EventoId.HasValue)
                {
                    viagem.EventoId = request.EventoId.Value;
                }
                else if (request.Finalidade != "Evento")
                {
                    viagem.EventoId = null;
                }

                // [DOC] Atualiza datas e horários da viagem
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

                // [DOC] Atualiza quilometragem inicial e final
                if (request.KmInicial.HasValue)
                {
                    viagem.KmInicial = request.KmInicial.Value;
                }

                if (request.KmFinal.HasValue)
                {
                    viagem.KmFinal = request.KmFinal.Value;
                }

                // [DOC] Atualiza motorista, veículo e setor solicitante
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

                // [DOC] Atualiza requisitante e ramal
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
     * 📦 DTO: AtualizarDadosViagemRequest
     * 🎯 OBJETIVO: Request para atualização parcial de dados de viagem
     * 📋 PROPRIEDADES (todos opcionais exceto ViagemId):
     *    - ViagemId: Identificador da viagem (obrigatório)
     *    - NoFichaVistoria: Número da ficha de vistoria
     *    - Finalidade: Finalidade da viagem
     *    - EventoId: ID do evento (null se finalidade != "Evento")
     *    - DataInicial/HoraInicio: Data e hora de início
     *    - DataFinal/HoraFim: Data e hora de término
     *    - KmInicial/KmFinal: Quilometragem inicial e final
     *    - MotoristaId/VeiculoId/SetorSolicitanteId: IDs relacionados
     *    - RequisitanteId/RamalRequisitante: Dados do requisitante
     * 📝 NOTA: Apenas campos informados (HasValue/!IsNullOrEmpty) serão atualizados
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
