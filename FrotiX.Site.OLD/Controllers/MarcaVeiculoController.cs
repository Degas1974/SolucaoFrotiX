/* ****************************************************************************************
 * ⚡ ARQUIVO: MarcaVeiculoController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar marcas de veículos (FIAT, VW, GM etc.) com CRUD básico
 *                   para listas e dropdowns.
 *
 * 📥 ENTRADAS     : MarcaVeiculoViewModel, IDs de marca.
 *
 * 📤 SAÍDAS       : JSON com marcas e mensagens de validação.
 *
 * 🔗 CHAMADA POR  : Pages/MarcasVeiculos/Index e cadastros de veículos.
 *
 * 🔄 CHAMA        : IUnitOfWork.MarcaVeiculo, IUnitOfWork.ModeloVeiculo.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, IUnitOfWork.
 *
 * 📄 DOCUMENTAÇÃO : Documentacao/Pages/MarcaVeiculo - Index.md
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: MarcaVeiculoController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor endpoints para listar, excluir e alterar status de marcas.
 *
 * 📥 ENTRADAS     : IDs e dados de marca.
 *
 * 📤 SAÍDAS       : JSON com listas e mensagens.
 *
 * 🔗 CHAMADA POR  : Telas administrativas e dropdowns de veículos.
 *
 * 🔄 CHAMA        : IUnitOfWork (MarcaVeiculo, ModeloVeiculo).
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
    public class MarcaVeiculoController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: MarcaVeiculoController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependência de acesso aos repositórios.
         *
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public MarcaVeiculoController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "MarcaVeiculoController.cs" ,
                    "MarcaVeiculoController" ,
                    error
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar todas as marcas de veículos.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com lista de marcas.
         *
         * 🔗 CHAMADA POR  : Grids e dropdowns de marca.
         *
         * 🔄 CHAMA        : _unitOfWork.MarcaVeiculo.GetAll().
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Json(new
                {
                    data = _unitOfWork.MarcaVeiculo.GetAll()
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MarcaVeiculoController.cs" , "Get" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover marca quando não há modelos vinculados.
         *
         * 📥 ENTRADAS     : [MarcaVeiculoViewModel] model (MarcaId).
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de exclusão em grids.
         *
         * 🔄 CHAMA        : MarcaVeiculo.GetFirstOrDefault(), ModeloVeiculo.GetFirstOrDefault(),
         *                   MarcaVeiculo.Remove(), Save().
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(MarcaVeiculoViewModel model)
        {
            try
            {
                if (model != null && model.MarcaId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefault(u =>
                        u.MarcaId == model.MarcaId
                    );
                    if (objFromDb != null)
                    {
                        var modelo = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u =>
                            u.MarcaId == model.MarcaId
                        );
                        if (modelo != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem modelos associados a essa marca" ,
                                }
                            );
                        }
                        _unitOfWork.MarcaVeiculo.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Marca de veículo removida com sucesso" ,
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar marca de veículo"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MarcaVeiculoController.cs" , "Delete" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusMarcaVeiculo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar status (ativo/inativo) da marca de veículo.
         *
         * 📥 ENTRADAS     : Id (Guid) - identificador da marca.
         *
         * 📤 SAÍDAS       : JSON com sucesso, mensagem e tipo.
         *
         * 🔗 CHAMADA POR  : Ações de ativação/inativação no grid.
         *
         * 🔄 CHAMA        : MarcaVeiculo.GetFirstOrDefault(), MarcaVeiculo.Update().
         ****************************************************************************************/
        [Route("UpdateStatusMarcaVeiculo")]
        public JsonResult UpdateStatusMarcaVeiculo(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefault(u =>
                        u.MarcaId == Id
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
                                "Atualizado Status da Marca [Nome: {0}] (Inativo)" ,
                                objFromDb.DescricaoMarca
                            );
                            type = 1;
                        }
                        else
                        {
                            //res["success"] = 1;
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status da Marca  [Nome: {0}] (Ativo)" ,
                                objFromDb.DescricaoMarca
                            );
                            type = 0;
                        }
                        //_unitOfWork.Save();
                        _unitOfWork.MarcaVeiculo.Update(objFromDb);
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
                    "MarcaVeiculoController.cs" ,
                    "UpdateStatusMarcaVeiculo" ,
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
