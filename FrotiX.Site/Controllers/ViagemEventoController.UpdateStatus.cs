/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: ViagemEventoController.UpdateStatus.cs                           ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: ViagemEvento API (Partial - UpdateStatus)
     * 🎯 OBJETIVO: Alternar status de eventos de viagem (Ativo ↔ Inativo)
     * 📋 ROTAS: /api/ViagemEvento/UpdateStatusEvento
     * 🔗 ENTIDADES: Evento
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     * 📝 NOTA: Classe parcial - ver ViagemEventoController.cs principal
     ****************************************************************************************/
    public partial class ViagemEventoController
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusEvento
         * 🎯 OBJETIVO: Alternar status do evento entre Ativo ("1") e Inativo ("0")
         * 📥 ENTRADAS: Id (Guid do evento)
         * 📤 SAÍDAS: JSON { success, type (0=inativo, 1=ativo), message }
         * 🔗 CHAMADA POR: Toggle de status no grid de eventos
         * 🔄 CHAMA: Evento.GetFirstOrDefault(), Evento.Update()
         * 📝 LÓGICA: Status armazenado como string "0"/"1", retorna int para JavaScript
         ****************************************************************************************/
        [Route("UpdateStatusEvento")]
        [HttpGet]
        public IActionResult UpdateStatusEvento(Guid Id)
        {
            try
            {
                var evento = _unitOfWork.Evento.GetFirstOrDefault(e => e.EventoId == Id);

                if (evento == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Evento não encontrado"
                    });
                }

                // [DOC] Alterna o status: "1" (Ativo) → "0" (Inativo) ou "0" → "1"
                evento.Status = evento.Status == "1" ? "0" : "1";

                _unitOfWork.Evento.Update(evento);
                _unitOfWork.Save();

                // [DOC] Retorna type como int (0 ou 1) para facilitar manipulação no JavaScript frontend
                return Json(new
                {
                    success = true,
                    type = int.Parse(evento.Status),
                    message = evento.Status == "1" ? "Evento ativado com sucesso" : "Evento inativado com sucesso"
                });
            }
            catch (Exception error)
            {
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
