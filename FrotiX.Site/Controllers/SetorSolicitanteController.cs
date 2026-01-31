/* ****************************************************************************************
 * ⚡ ARQUIVO: SetorSolicitanteController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar setores solicitantes e validar hierarquia pai/filho.
 *
 * 📥 ENTRADAS     : IDs e view models de setor solicitante.
 *
 * 📤 SAÍDAS       : JSON com status das operações.
 *
 * 🔗 CHAMADA POR  : Telas de cadastro/gestão de setores solicitantes.
 *
 * 🔄 CHAMA        : IUnitOfWork.SetorSolicitante.
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: SetorSolicitanteController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints para manutenção de setores solicitantes.
     *
     * 📥 ENTRADAS     : Modelos e IDs de setor solicitante.
     *
     * 📤 SAÍDAS       : JSON com mensagens de retorno.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public partial class SetorSolicitanteController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: SetorSolicitanteController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependência do UnitOfWork.
         *
         * 📥 ENTRADAS     : unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public SetorSolicitanteController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "SetorSolicitanteController.cs" ,
                    "SetorSolicitanteController" ,
                    error
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover setor solicitante se não houver setores filhos vinculados.
         *
         * 📥 ENTRADAS     : model (SetorSolicitanteViewModel).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : Ação de exclusão no grid.
         *
         * 🔄 CHAMA        : SetorSolicitante.GetFirstOrDefault(), SetorSolicitante.Remove(),
         *                   UnitOfWork.Save().
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(SetorSolicitanteViewModel model)
        {
            try
            {
                if (model != null && model.SetorSolicitanteId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
                        u.SetorSolicitanteId == model.SetorSolicitanteId
                    );
                    if (objFromDb != null)
                    {
                        // Verifica se existem filhos associados ao setor
                        var filhos = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
                            u.SetorPaiId == model.SetorSolicitanteId
                        );
                        if (filhos != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem setores filho associados a esse Setor pai" ,
                                }
                            );
                        }

                        _unitOfWork.SetorSolicitante.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Setor Solicitante removido com sucesso" ,
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Setor Solicitante"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorSolicitanteController.cs" , "Delete" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar setor solicitante"
                });
            }
        }
    }
}
