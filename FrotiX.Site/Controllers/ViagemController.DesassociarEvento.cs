/* ****************************************************************************************
 * ⚡ ARQUIVO: ViagemController.DesassociarEvento.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Desassociar viagens de eventos e ajustar a finalidade.
 *
 * 📥 ENTRADAS     : DTO com ID da viagem e nova finalidade.
 *
 * 📤 SAÍDAS       : JSON com status da operação.
 *
 * 🔗 CHAMADA POR  : Ajustes administrativos de viagens.
 *
 * 🔄 CHAMA        : IUnitOfWork.Viagem, IMemoryCache (limpeza de cache).
 **************************************************************************************** */

using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER PARTIAL: ViagemController.DesassociarEvento
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Implementar desassociação de viagens de eventos.
     *
     * 📥 ENTRADAS     : DTOs de desassociação.
     *
     * 📤 SAÍDAS       : JSON com sucesso/erro.
     ****************************************************************************************/
    public partial class ViagemController
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: DesassociarViagemEvento
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo da viagem com evento e atualizar finalidade.
         *
         * 📥 ENTRADAS     : request (DesassociarViagemRequest).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : POST /api/Viagem/DesassociarViagemEvento.
         ****************************************************************************************/
        [Route("DesassociarViagemEvento")]
        [HttpPost]
        public IActionResult DesassociarViagemEvento([FromBody] DesassociarViagemRequest request)
        {
            try
            {
                if (request == null || request.ViagemId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "ID da viagem não informado"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.NovaFinalidade))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Nova finalidade não informada"
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

                // Guarda o evento antigo para invalidar cache
                var eventoAntigoId = viagem.EventoId;

                // Remove a associação com o evento
                viagem.EventoId = null;
                viagem.NomeEvento = null;

                // Altera a finalidade
                viagem.Finalidade = request.NovaFinalidade;

                // Atualiza a viagem
                _unitOfWork.Viagem.Update(viagem);
                _unitOfWork.Save();

                // Invalida cache do evento (se existir)
                if (eventoAntigoId.HasValue && _cache != null)
                {
                    _cache.Remove($"viagens_evento_{eventoAntigoId.Value}_1_50");
                    _cache.Remove($"viagens_evento_{eventoAntigoId.Value}_1_100");
                }

                return Json(new
                {
                    success = true,
                    message = "Viagem desassociada com sucesso!"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemController.cs", "DesassociarViagemEvento", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao desassociar viagem do evento"
                });
            }
        }
    }

    /****************************************************************************************
     * ⚡ DTO: DesassociarViagemRequest
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar dados para desassociação de evento.
     *
     * 📥 ENTRADAS     : ViagemId, NovaFinalidade.
     *
     * 📤 SAÍDAS       : Nenhuma (apenas transporte de dados).
     *
     * 🔗 CHAMADA POR  : DesassociarViagemEvento.
     ****************************************************************************************/
    public class DesassociarViagemRequest
    {
        public Guid ViagemId { get; set; }
        public string NovaFinalidade { get; set; }
    }
}
