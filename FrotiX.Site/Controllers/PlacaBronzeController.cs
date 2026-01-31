/* ****************************************************************************************
 * ⚡ ARQUIVO: PlacaBronzeController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar placas de bronze e seus vínculos com veículos oficiais.
 *
 * 📥 ENTRADAS     : IDs e modelos de placa de bronze.
 *
 * 📤 SAÍDAS       : JSON com listas e status de operações.
 *
 * 🔗 CHAMADA POR  : Telas administrativas de patrimônio/veículos.
 *
 * 🔄 CHAMA        : IUnitOfWork.PlacaBronze, IUnitOfWork.Veiculo.
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: PlacaBronzeController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints para listar, excluir, atualizar status e desvincular
     *                   placas de bronze de veículos.
     *
     * 📥 ENTRADAS     : IDs e ViewModels de placa.
     *
     * 📤 SAÍDAS       : JSON com dados e mensagens de retorno.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class PlacaBronzeController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: PlacaBronzeController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependência do UnitOfWork.
         *
         * 📥 ENTRADAS     : unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public PlacaBronzeController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "PlacaBronzeController.cs" ,
                    "PlacaBronzeController" ,
                    error
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar placas de bronze com placa do veículo vinculado (se houver).
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com data (lista de placas e vínculos).
         *
         * 🔗 CHAMADA POR  : Grid de placas de bronze.
         *
         * 🔄 CHAMA        : PlacaBronze.GetAll(), Veiculo.GetAll().
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = (
                    from p in _unitOfWork.PlacaBronze.GetAll()
                    join v in _unitOfWork.Veiculo.GetAll()
                        on p.PlacaBronzeId equals v.PlacaBronzeId
                        into pb
                    from pbResult in pb.DefaultIfEmpty()
                    select new
                    {
                        p.PlacaBronzeId ,
                        p.DescricaoPlaca ,
                        p.Status ,
                        PlacaVeiculo = pbResult != null ? pbResult.Placa : "" ,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs" , "Get" , error);
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
         * 🎯 OBJETIVO     : Remover placa de bronze quando não houver veículo associado.
         *
         * 📥 ENTRADAS     : model (PlacaBronzeViewModel).
         *
         * 📤 SAÍDAS       : JSON com success e message.
         *
         * 🔗 CHAMADA POR  : Ação de exclusão no grid.
         *
         * 🔄 CHAMA        : PlacaBronze.GetFirstOrDefault(), Veiculo.GetFirstOrDefault(),
         *                   PlacaBronze.Remove(), UnitOfWork.Save().
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(PlacaBronzeViewModel model)
        {
            try
            {
                if (model != null && model.PlacaBronzeId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.PlacaBronze.GetFirstOrDefault(u =>
                        u.PlacaBronzeId == model.PlacaBronzeId
                    );
                    if (objFromDb != null)
                    {
                        var modelo = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                            u.PlacaBronzeId == model.PlacaBronzeId
                        );
                        if (modelo != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem veículos associados a essa placa" ,
                                }
                            );
                        }
                        _unitOfWork.PlacaBronze.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Placa de Bronze removida com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar placa de bronze"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs" , "Delete" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao deletar placa de bronze"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusPlacaBronze
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar o status ativo/inativo da placa de bronze.
         *
         * 📥 ENTRADAS     : Id (Guid da placa).
         *
         * 📤 SAÍDAS       : JSON com success, message e type.
         *
         * 🔗 CHAMADA POR  : Ação de ativar/desativar placa.
         *
         * 🔄 CHAMA        : PlacaBronze.GetFirstOrDefault(), PlacaBronze.Update().
         ****************************************************************************************/
        [Route("UpdateStatusPlacaBronze")]
        public JsonResult UpdateStatusPlacaBronze(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.PlacaBronze.GetFirstOrDefault(u =>
                        u.PlacaBronzeId == Id
                    );
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status da Placa [Nome: {0}] (Inativo)" ,
                                objFromDb.DescricaoPlaca
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status da Marca  [Nome: {0}] (Ativo)" ,
                                objFromDb.DescricaoPlaca
                            );
                            type = 0;
                        }
                        _unitOfWork.PlacaBronze.Update(objFromDb);
                    }
                    return Json(
                        new
                        {
                            success = true ,
                            message = Description ,
                            type = type ,
                        }
                    );
                }
                return Json(new
                {
                    success = false
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "PlacaBronzeController.cs" ,
                    "UpdateStatusPlacaBronze" ,
                    error
                );
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Desvincula
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo da placa de bronze do veículo associado.
         *
         * 📥 ENTRADAS     : model (PlacaBronzeViewModel).
         *
         * 📤 SAÍDAS       : JSON com success, message e type.
         *
         * 🔗 CHAMADA POR  : Ação de desvincular na tela de placas.
         *
         * 🔄 CHAMA        : Veiculo.GetFirstOrDefault(), Veiculo.Update().
         ****************************************************************************************/
        [Route("Desvincula")]
        [HttpPost]
        public IActionResult Desvincula(PlacaBronzeViewModel model)
        {
            try
            {
                if (model.PlacaBronzeId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                        u.PlacaBronzeId == model.PlacaBronzeId
                    );
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        objFromDb.PlacaBronzeId = Guid.Empty;
                        Description = string.Format(
                            "Placa de Bronze desassociada com sucesso!" ,
                            objFromDb.Placa
                        );
                        type = 1;
                        _unitOfWork.Veiculo.Update(objFromDb);
                    }
                    return Json(
                        new
                        {
                            success = true ,
                            message = Description ,
                            type = type ,
                        }
                    );
                }
                return Json(new
                {
                    success = false
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs" , "Desvincula" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao desvincular placa"
                });
            }
        }
    }
}
