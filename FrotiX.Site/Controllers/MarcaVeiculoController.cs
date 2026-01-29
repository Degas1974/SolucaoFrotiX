/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MarcaVeiculoController.cs                                                               ║
   ║ 📂 CAMINHO: /Controllers                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Gerenciar marcas de veículos (FIAT, VW, GM, etc). CRUD básico para dropdowns.          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Get(), Upsert(), Delete() - MarcaVeiculoViewModel para cadastro de veículos              ║
   ║ 🔗 DEPS: IUnitOfWork | 📅 28/01/2026 | 👤 Copilot | 📝 v2.0                                         ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

/****************************************************************************************
 * ⚡ CONTROLLER: MarcaVeiculoController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar marcas de veículos (FIAT, VW, GM, etc) - CRUD básico
 * 📥 ENTRADAS     : MarcaVeiculoViewModel, IDs
 * 📤 SAÍDAS       : JSON com marcas
 * 🔗 CHAMADA POR  : Pages/MarcasVeiculos/Index, Dropdowns de cadastro de veículos
 * 🔄 CHAMA        : IUnitOfWork (MarcaVeiculo)
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, IUnitOfWork
 * 📄 DOCUMENTAÇÃO : Documentacao/Pages/MarcaVeiculo - Index.md
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
