/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RecursoController.cs                                                                    ║
   ║ 📂 CAMINHO: /Controllers                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Gerenciar Recursos do sistema (permissões/menus). Atribuídos via ControleAcesso.       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Get(), Delete() - ordenados por Ordem, valida vínculos ControleAcesso antes excluir      ║
   ║ 🔗 DEPS: IUnitOfWork (Recurso, ControleAcesso) | 📅 28/01/2026 | 👤 Copilot | 📝 v2.0               ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
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
