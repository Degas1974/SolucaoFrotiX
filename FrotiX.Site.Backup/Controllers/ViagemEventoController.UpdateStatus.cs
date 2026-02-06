/* ****************************************************************************************
 * ⚡ ARQUIVO: ViagemEventoController.UpdateStatus.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Alternar status de eventos associados a viagens.
 *
 * 📥 ENTRADAS     : Id do evento.
 *
 * 📤 SAÍDAS       : JSON com success, type e message.
 *
 * 🔗 CHAMADA POR  : Grid de eventos.
 *
 * 🔄 CHAMA        : IUnitOfWork.Evento.
 **************************************************************************************** */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER PARTIAL: ViagemEventoController.UpdateStatus
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Implementar toggle de status de evento.
     *
     * 📥 ENTRADAS     : Id do evento.
     *
     * 📤 SAÍDAS       : JSON com status atualizado.
     ****************************************************************************************/
    public partial class ViagemEventoController
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusEvento
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar status do evento entre Ativo ("1") e Inativo ("0")
         *                   Operação toggle: se ativo→inativo, se inativo→ativo
         * 📥 ENTRADAS     : [Guid] Id - ID do evento a alterar
         * 📤 SAÍDAS       : [IActionResult] JSON com success, type (novo status), message
         * 🔗 CHAMADA POR  : JavaScript (botão de ativar/inativar na grid de eventos)
         * 🔄 CHAMA        : IUnitOfWork.Evento.GetFirstOrDefault, Update, Save
         *
         * 📊 RETORNO:
         *    - success: true/false
         *    - type: Novo status como int (0 ou 1) para atualizar UI
         *    - message: Mensagem de confirmação para toast/alert
         ****************************************************************************************/
        [Route("UpdateStatusEvento")]
        [HttpGet]
        public IActionResult UpdateStatusEvento(Guid Id)
        {
            try
            {
                // [DOC] STEP 1: Buscar evento pelo ID
                var evento = _unitOfWork.Evento.GetFirstOrDefault(e => e.EventoId == Id);

                // [DOC] STEP 2: Validar se evento existe
                if (evento == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Evento não encontrado"
                    });
                }

                // [DOC] STEP 3: Toggle do status - Alterna entre "1" (Ativo) e "0" (Inativo)
                evento.Status = evento.Status == "1" ? "0" : "1";

                // [DOC] STEP 4: Persistir alteração no banco
                _unitOfWork.Evento.Update(evento);
                _unitOfWork.Save();

                // [DOC] STEP 5: Retornar sucesso com novo status para atualizar UI
                return Json(new
                {
                    success = true,
                    type = int.Parse(evento.Status),  // [DOC] Retorna como int para facilitar uso no JS
                    message = evento.Status == "1" ? "Evento ativado com sucesso" : "Evento inativado com sucesso"
                });
            }
            catch (Exception error)
            {
                // [DOC] Tratamento de erro padronizado
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "UpdateStatusEvento", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao alterar status do evento"
                });
            }
        }
    }
}
