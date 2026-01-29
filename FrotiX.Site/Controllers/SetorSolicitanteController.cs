/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: SetorSolicitanteController.cs (Controllers)                                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ API Controller para exclusão de Setores Solicitantes (setores que solicitam viagens).                   ║
 * ║ Estrutura hierárquica: Setor Pai → Setores Filho (SetorPaiId).                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ROTA BASE: api/SetorSolicitante                                                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ENDPOINTS                                                                                                 ║
 * ║ • [POST] /Delete : Remove setor solicitante (verifica setores filhos)                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ VALIDAÇÃO DE EXCLUSÃO                                                                                     ║
 * ║ • Não pode excluir se existem setores filhos (SetorPaiId = SetorSolicitanteId)                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork (SetorSolicitante)                                                                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
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
