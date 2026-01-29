/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : SetorSolicitanteController.UpdateStatus.cs                      ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ Partial class do SetorSolicitanteController para atualizacao de status.      ║
║ Alterna entre Ativo e Inativo de forma toggle.                               ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - GET /api/SetorSolicitante/UpdateStatus?id={guid} : Alterna status          ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ RETORNO                                                                      ║
║ - success    : true/false                                                    ║
║ - message    : Mensagem de confirmacao                                       ║
║ - novoStatus : 0 (inativo) ou 1 (ativo)                                      ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Data Documentacao: 28/01/2026                              LOTE: 20          ║
╚══════════════════════════════════════════════════════════════════════════════╝
*/

using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    public partial class SetorSolicitanteController : Controller
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatus
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar status Ativo/Inativo de um setor solicitante (toggle)
         * 📥 ENTRADAS     : [string] id - GUID do setor como string
         * 📤 SAÍDAS       : [IActionResult] JSON { success, message, novoStatus }
         * 🔗 CHAMADA POR  : Botao toggle de status na TreeView de setores
         * 🔄 CHAMA        : SetorSolicitante.Update(), Save()
         *
         * 📝 COMPORTAMENTO:
         *    - Se Status=true, muda para false (desativa)
         *    - Se Status=false, muda para true (ativa)
         *    - Atualiza DataAlteracao com timestamp atual
         ****************************************************************************************/
        [Route("UpdateStatus")]
        [HttpGet]
        public IActionResult UpdateStatus(string id)
        {
            try
            {
                // [DOC] Valida e converte ID string para GUID
                if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid guidId))
                {
                    return Json(new { success = false, message = "ID inválido" });
                }

                // [DOC] Busca setor no banco
                var setor = _unitOfWork.SetorSolicitante.GetFirstOrDefault(s => s.SetorSolicitanteId == guidId);
                if (setor == null)
                {
                    return Json(new { success = false, message = "Setor não encontrado" });
                }

                // [DOC] TOGGLE: Inverte o status booleano (true<->false)
                setor.Status = !setor.Status;
                setor.DataAlteracao = DateTime.Now;

                // [DOC] Persiste alteracao no banco
                _unitOfWork.SetorSolicitante.Update(setor);
                _unitOfWork.Save();

                // [DOC] Retorna novo status para atualizar UI sem recarregar
                return Json(new
                {
                    success = true,
                    message = setor.Status ? "Setor ativado com sucesso" : "Setor desativado com sucesso",
                    novoStatus = setor.Status ? 1 : 0
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "UpdateStatus", error);
                return Json(new { success = false, message = "Erro ao alterar status do setor" });
            }
        }
    }
}
