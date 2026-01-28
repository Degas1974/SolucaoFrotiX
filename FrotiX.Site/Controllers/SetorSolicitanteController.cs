/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: SetorSolicitanteController.cs                                    ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: SetorSolicitante API (Partial Class)
     * 🎯 OBJETIVO: Gerenciar setores solicitantes (estrutura hierárquica de departamentos)
     * 📋 ROTAS: /api/SetorSolicitante/*
     * 🔗 ENTIDADES: SetorSolicitante (hierarquia pai-filho)
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     * 📝 NOTA: Classe parcial - métodos adicionais em arquivos separados
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public partial class SetorSolicitanteController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

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
         * 🎯 OBJETIVO: Excluir setor solicitante (valida hierarquia antes de remover)
         * 📥 ENTRADAS: model (SetorSolicitanteViewModel com SetorSolicitanteId)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de exclusão de setor solicitante
         * 🔄 CHAMA: SetorSolicitante.GetFirstOrDefault(), SetorSolicitante.Remove()
         * ⚠️ VALIDAÇÃO: Impede exclusão se existirem setores filho associados
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
                        // [DOC] Valida integridade referencial: não permite excluir pai com filhos
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
