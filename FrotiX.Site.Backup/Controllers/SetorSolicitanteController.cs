/*
 *  _____________________________________________________________
 * |                                                             |
 * |   FrotiX Core - Gestão de Setores (Core Stack)              |
 * |_____________________________________________________________|
 *
 * (IA) Controlador parcial para gestão de setores solicitantes,
 * organizando a hierarquia de demanas de viagens.
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: SetorSolicitanteController                                        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de setores solicitantes e hierarquia de demandas.                  ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/SetorSolicitante                                       ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public partial class SetorSolicitanteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SetorSolicitanteController (Construtor)                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa UnitOfWork e serviço de log.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public SetorSolicitanteController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs", "SetorSolicitanteController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove setor solicitante se não houver setores filhos.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (SetorSolicitanteViewModel): Dados com ID do setor.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(SetorSolicitanteViewModel model)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (model != null && model.SetorSolicitanteId != Guid.Empty)
                {
                    // [DADOS] Busca setor solicitante.
                    var objFromDb = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
                        u.SetorSolicitanteId == model.SetorSolicitanteId
                    );
                    if (objFromDb != null)
                    {
                        // [REGRA] Verifica filhos associados.
                        var filhos = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
                            u.SetorPaiId == model.SetorSolicitanteId
                        );
                        if (filhos != null)
                        {
                            // [RETORNO] Bloqueia exclusão por filhos.
                            _log.Warning($"Tentativa de exclusão de setor solicitante com filhos: [Setor: {objFromDb.Nome}] [ID: {model.SetorSolicitanteId}]");
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem setores filho associados a esse Setor pai" ,
                                }
                            );
                        }

                        // [ACAO] Remove setor solicitante.
                        var nomeSetor = objFromDb.Nome;
                        _unitOfWork.SetorSolicitante.Remove(objFromDb);
                        _unitOfWork.Save();
                        _log.Info($"Setor solicitante removido: [ID: {model.SetorSolicitanteId}] [Nome: {nomeSetor}]");
                        // [RETORNO] Sucesso.
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Setor Solicitante removido com sucesso" ,
                            }
                        );
                    }
                }
                // [RETORNO] Falha padrão.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Setor Solicitante"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs" , "Delete" , error);
                _log.Error($", errorErro ao deletar setor solicitante [ID: {model?.SetorSolicitanteId}]");
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar setor solicitante"
                });
            }
        }
    }
}
