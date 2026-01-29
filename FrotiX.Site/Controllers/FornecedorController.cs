/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : FornecedorController.cs                                         ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ Controller para gestao de fornecedores (empresas contratadas). CRUD          ║
║ basico com validacao de integridade (contratos vinculados).                  ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - GET  /api/Fornecedor/GetAll   : Lista todos fornecedores                   ║
║ - POST /api/Fornecedor/Upsert   : Criar/atualizar fornecedor                 ║
║ - POST /api/Fornecedor/Delete   : Excluir fornecedor (valida vinculos)       ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Data Documentacao: 28/01/2026                              LOTE: 21          ║
╚══════════════════════════════════════════════════════════════════════════════╝
*/

/****************************************************************************************
 * ⚡ CONTROLLER: FornecedorController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar fornecedores (empresas contratadas)
 *                   CRUD básico com validação de integridade (contratos vinculados)
 * 📥 ENTRADAS     : FornecedorViewModel, IDs (via API REST)
 * 📤 SAÍDAS       : JSON com fornecedores, status de operações
 * 🔗 CHAMADA POR  : Pages/Fornecedores/Index, Pages/Contratos (seleção de fornecedor)
 * 🔄 CHAMA        : IUnitOfWork (Fornecedor, Contrato), Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, Entity Framework
 *
 * ⚠️  INTEGRIDADE:
 *    - Bloqueia exclusão se houver contratos vinculados ao fornecedor
 *    - Permite ativar/desativar fornecedor sem excluir (soft delete)
 ****************************************************************************************/
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FrotiX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: FornecedorController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do Unit of Work
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork
         * 📤 SAÍDAS       : Instância configurada
         * 🔗 CHAMADA POR  : ASP.NET Core DI
         ****************************************************************************************/
        public FornecedorController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "FornecedorController.cs" ,
                    "FornecedorController" ,
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
                    data = _unitOfWork.Fornecedor.GetAll()
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FornecedorController.cs" , "Get" , error);
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Excluir fornecedor do banco validando integridade referencial
         * 📥 ENTRADAS     : [FornecedorViewModel] model - contém FornecedorId
         * 📤 SAÍDAS       : [IActionResult] JSON success/message
         * 🔄 CHAMA        : Fornecedor.GetFirstOrDefault(), Contrato, Remove(), Save()
         * ⚠️  VALIDAÇÃO   : Bloqueia exclusão se houver contratos associados ao fornecedor
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(FornecedorViewModel model)
        {
            try
            {
                if (model != null && model.FornecedorId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Fornecedor.GetFirstOrDefault(u =>
                        u.FornecedorId == model.FornecedorId
                    );
                    if (objFromDb != null)
                    {
                        // [DOC] Verifica integridade referencial - bloqueia se houver contratos vinculados
                        var contrato = _unitOfWork.Contrato.GetFirstOrDefault(u =>
                            u.FornecedorId == model.FornecedorId
                        );
                        if (contrato != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem contratos associados a esse fornecedor" ,
                                }
                            );
                        }
                        _unitOfWork.Fornecedor.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Fornecedor removido com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Fornecedor"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FornecedorController.cs" , "Delete" , error);
                return StatusCode(500);
            }
        }

        [Route("UpdateStatusFornecedor")]
        public JsonResult UpdateStatusFornecedor(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Fornecedor.GetFirstOrDefault(u =>
                        u.FornecedorId == Id
                    );
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Fornecedor [Nome: {0}] (Inativo)" ,
                                objFromDb.DescricaoFornecedor
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Fornecedor  [Nome: {0}] (Ativo)" ,
                                objFromDb.DescricaoFornecedor
                            );
                            type = 0;
                        }
                        _unitOfWork.Fornecedor.Update(objFromDb);
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
                    "FornecedorController.cs" ,
                    "UpdateStatusFornecedor" ,
                    error
                );
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }
    }
}
