using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: SetorSolicitanteController (UpdateStatus)                         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Extensão parcial para alteração de status de setores solicitantes.        ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public partial class SetorSolicitanteController : Controller
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatus (GET)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna status do setor solicitante (Ativo/Inativo).                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (string): ID do setor solicitante.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status atualizado.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatus")]
        [HttpGet]
        public IActionResult UpdateStatus(string id)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid guidId))
                {
                    return Json(new { success = false, message = "ID inválido" });
                }

                // [DADOS] Busca setor.
                var setor = _unitOfWork.SetorSolicitante.GetFirstOrDefault(s => s.SetorSolicitanteId == guidId);
                if (setor == null)
                {
                    return Json(new { success = false, message = "Setor não encontrado" });
                }

                // [REGRA] Alterna o status.
                setor.Status = !setor.Status;
                setor.DataAlteracao = DateTime.Now;

                // [ACAO] Persiste alterações.
                _unitOfWork.SetorSolicitante.Update(setor);
                _unitOfWork.Save();

                // [LOG] Registro de status.
                var statusMsg = setor.Status ? "Ativo" : "Inativo";
                _log.Info($"Atualizado Status do Setor Solicitante [Nome: {setor.Nome}] ({statusMsg})");

                // [RETORNO] Resultado da operação.
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
                _log.Error($", errorErro ao alternar status do setor solicitante [ID: {id}]");
                return Json(new { success = false, message = "Erro ao alterar status do setor" });
            }
        }
    }
}
