/* ****************************************************************************************
 * ⚡ ARQUIVO: RecursoController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar recursos do sistema (menus/permissões) vinculados ao acesso.
 *
 * 📥 ENTRADAS     : Modelos de recurso e IDs.
 *
 * 📤 SAÍDAS       : JSON com listas e status de operações.
 *
 * 🔗 CHAMADA POR  : Telas administrativas de controle de acesso.
 *
 * 🔄 CHAMA        : IUnitOfWork.Recurso, IUnitOfWork.ControleAcesso.
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: RecursoController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints para listar e excluir recursos do sistema.
     *
     * 📥 ENTRADAS     : Recurso (model) e IDs.
     *
     * 📤 SAÍDAS       : JSON com dados ou mensagens de erro.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class RecursoController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: RecursoController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependência do UnitOfWork.
         *
         * 📥 ENTRADAS     : unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public RecursoController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RecursoController.cs" , "RecursoController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar recursos ordenados por ordem de exibição.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com data (lista de recursos).
         *
         * 🔗 CHAMADA POR  : Grid de recursos.
         *
         * 🔄 CHAMA        : Recurso.GetAll().
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = (
                    from r in _unitOfWork.Recurso.GetAll()
                    select new
                    {
                        r.RecursoId ,
                        r.Nome ,
                        r.NomeMenu ,
                        r.Descricao ,
                        r.Ordem ,
                    }
                ).ToList().OrderBy(r => r.Ordem);

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RecursoController.cs" , "Get" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao carregar dados"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover um recurso se não houver vínculo em ControleAcesso.
         *
         * 📥 ENTRADAS     : model (Recurso).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : Ação de exclusão no grid.
         *
         * 🔄 CHAMA        : Recurso.GetFirstOrDefault(), ControleAcesso.GetFirstOrDefault(),
         *                   Recurso.Remove(), UnitOfWork.Save().
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(Recurso model)
        {
            try
            {
                if (model != null && model.RecursoId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Recurso.GetFirstOrDefault(r =>
                        r.RecursoId == model.RecursoId
                    );
                    if (objFromDb != null)
                    {
                        var objControleAcesso = _unitOfWork.ControleAcesso.GetFirstOrDefault(ca =>
                            ca.RecursoId == model.RecursoId
                        );
                        if (objControleAcesso != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Não foi possível remover o Recurso. Ele está associado a um ou mais usuários!" ,
                                }
                            );
                        }

                        _unitOfWork.Recurso.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Recurso removido com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Recurso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("RecursoController.cs" , "Delete" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar recurso"
                });
            }
        }
    }
}
