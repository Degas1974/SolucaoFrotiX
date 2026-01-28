/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: RecursoController.cs                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Recurso API
     * 🎯 OBJETIVO: Gerenciar recursos do sistema (menus, permissões, funcionalidades)
     * 📋 ROTAS: /api/Recurso/*
     * 🔗 ENTIDADES: Recurso, ControleAcesso
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class RecursoController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

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
         * 🎯 OBJETIVO: Listar todos os recursos do sistema ordenados por Ordem
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { data: List<{ RecursoId, Nome, NomeMenu, Descricao, Ordem }> }
         * 🔗 CHAMADA POR: Telas de gerenciamento de recursos/permissões
         * 🔄 CHAMA: Recurso.GetAll()
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DOC] Ordena por campo Ordem para manter hierarquia do menu
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
         * 🎯 OBJETIVO: Excluir um recurso do sistema (valida dependências antes)
         * 📥 ENTRADAS: model (Recurso com RecursoId)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de exclusão de recursos
         * 🔄 CHAMA: Recurso.GetFirstOrDefault(), ControleAcesso.GetFirstOrDefault(), Recurso.Remove()
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
                        // [DOC] Valida se recurso está em uso antes de excluir (integridade referencial)
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
