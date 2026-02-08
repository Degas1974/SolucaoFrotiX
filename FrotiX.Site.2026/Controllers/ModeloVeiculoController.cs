/* ****************************************************************************************
 * ⚡ ARQUIVO: ModeloVeiculoController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar modelos de veículos vinculados a marcas, com CRUD básico
 *                   e retorno de dados para dropdowns.
 *
 * 📥 ENTRADAS     : ModeloVeiculoViewModel e IDs de modelo.
 *
 * 📤 SAÍDAS       : JSON com modelos e informações de marca (JOIN).
 *
 * 🔗 CHAMADA POR  : Pages/ModelosVeiculos/Index e cadastros de veículos.
 *
 * 🔄 CHAMA        : IUnitOfWork.ModeloVeiculo, IUnitOfWork.MarcaVeiculo.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, IUnitOfWork.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: ModeloVeiculoController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor endpoints para listar, excluir e alterar status de modelos.
 *
 * 📥 ENTRADAS     : IDs e dados do modelo.
 *
 * 📤 SAÍDAS       : JSON com listas e mensagens.
 *
 * 🔗 CHAMADA POR  : Telas administrativas e dropdowns de veículos.
 *
 * 🔄 CHAMA        : IUnitOfWork (ModeloVeiculo, MarcaVeiculo).
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, IUnitOfWork.
 ****************************************************************************************/
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeloVeiculoController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: ModeloVeiculoController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependência de acesso aos repositórios.
         *
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public ModeloVeiculoController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "ModeloVeiculoController.cs" ,
                    "ModeloVeiculoController" ,
                    error
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar todos os modelos com a marca associada.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com lista de modelos e marcas.
         *
         * 🔗 CHAMADA POR  : Grids e dropdowns de modelos.
         *
         * 🔄 CHAMA        : _unitOfWork.ModeloVeiculo.GetAll(..., include MarcaVeiculo).
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Json(
                    new
                    {
                        data = _unitOfWork.ModeloVeiculo.GetAll(null , null , "MarcaVeiculo")
                    }
                );
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ModeloVeiculoController.cs" , "Get" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover modelo quando não há veículos associados.
         *
         * 📥 ENTRADAS     : [ModeloVeiculoViewModel] model (ModeloId).
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de exclusão em grids.
         *
         * 🔄 CHAMA        : ModeloVeiculo.GetFirstOrDefault(), Veiculo.GetFirstOrDefault(),
         *                   ModeloVeiculo.Remove(), Save().
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(ModeloVeiculoViewModel model)
        {
            try
            {
                if (model != null && model.ModeloId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u =>
                        u.ModeloId == model.ModeloId
                    );
                    if (objFromDb != null)
                    {
                        // Verifica se existem veículos associados ao modelo
                        //==================================================
                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                            u.ModeloId == model.ModeloId
                        );
                        if (veiculo != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem veículos associados a esse modelo" ,
                                }
                            );
                        }
                        _unitOfWork.ModeloVeiculo.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Modelo de veículo removido com sucesso" ,
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar modelo de veículo"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ModeloVeiculoController.cs" , "Delete" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusModeloVeiculo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar status (ativo/inativo) do modelo de veículo.
         *
         * 📥 ENTRADAS     : Id (Guid) - identificador do modelo.
         *
         * 📤 SAÍDAS       : JSON com sucesso, mensagem e tipo.
         *
         * 🔗 CHAMADA POR  : Ações de ativação/inativação no grid.
         *
         * 🔄 CHAMA        : ModeloVeiculo.GetFirstOrDefault(), ModeloVeiculo.Update().
         ****************************************************************************************/
        [Route("UpdateStatusModeloVeiculo")]
        public JsonResult UpdateStatusModeloVeiculo(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u =>
                        u.ModeloId == Id
                    );
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            //res["success"] = 0;
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Modelo [Nome: {0}] (Inativo)" ,
                                objFromDb.DescricaoModelo
                            );
                            type = 1;
                        }
                        else
                        {
                            //res["success"] = 1;
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Modelo  [Nome: {0}] (Ativo)" ,
                                objFromDb.DescricaoModelo
                            );
                            type = 0;
                        }
                        //_unitOfWork.Save();
                        _unitOfWork.ModeloVeiculo.Update(objFromDb);
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
                    "ModeloVeiculoController.cs" ,
                    "UpdateStatusModeloVeiculo" ,
                    error
                );
                return new JsonResult(new
                {
                    sucesso = false
                }); // padronizado
            }
        }
    }
}
