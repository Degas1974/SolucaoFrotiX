/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : RecursoController.cs                                            ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ Controller API para gerenciamento de Recursos do sistema (permissões/menus). ║
║ Recursos representam funcionalidades do sistema que podem ser atribuídas     ║
║ a usuários via ControleAcesso.                                               ║
║ Endpoint: /api/Recurso                                                       ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - GET /api/Recurso : Lista todos os recursos ordenados por Ordem             ║
║   * Retorna: { data: [{ RecursoId, Nome, NomeMenu, Descricao, Ordem }] }     ║
║ - POST Delete      : Remove um recurso do sistema                            ║
║   * Validação: Não permite excluir se houver ControleAcesso vinculado        ║
║   * Retorna mensagem específica se recurso está associado a usuários         ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ CAMPOS DO RECURSO                                                            ║
║ - RecursoId : Identificador único (GUID)                                     ║
║ - Nome      : Nome interno do recurso                                        ║
║ - NomeMenu  : Nome exibido no menu do sistema                                ║
║ - Descricao : Descrição detalhada do recurso                                 ║
║ - Ordem     : Posição de ordenação no menu                                   ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ REGRAS DE NEGOCIO                                                            ║
║ - Recurso só pode ser excluído se não estiver vinculado a nenhum usuário     ║
║ - Verificação via tabela ControleAcesso antes da exclusão                    ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DEPENDENCIAS                                                                 ║
║ - IUnitOfWork      : Acesso a repositórios Recurso e ControleAcesso          ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Data Documentacao: 28/01/2026                              LOTE: 19          ║
╚══════════════════════════════════════════════════════════════════════════════╝
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
