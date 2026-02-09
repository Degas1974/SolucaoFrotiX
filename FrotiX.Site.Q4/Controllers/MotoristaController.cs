/* ****************************************************************************************
 * ⚡ ARQUIVO: MotoristaController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar motoristas (condutores), incluindo CRUD, fotos e vínculos
 *                   com contratos, além de uso em escalas e viagens.
 *
 * 📥 ENTRADAS     : MotoristaViewModel, IDs, filtros e parâmetros de status.
 *
 * 📤 SAÍDAS       : JSON com motoristas, contratos e dados formatados.
 *
 * 🔗 CHAMADA POR  : Pages/Motoristas/Index, Escalas e Viagens (AJAX).
 *
 * 🔄 CHAMA        : IUnitOfWork (Motorista, Contrato, Fornecedor, CNH, VAssociado).
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, Entity Framework, File System.
 *
 * 📄 DOCUMENTAÇÃO : Documentacao/Pages/Motorista - Index.md
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: MotoristaController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor operações de listagem, exclusão, status e vínculos de motoristas.
 *
 * 📥 ENTRADAS     : IDs e view models de motorista.
 *
 * 📤 SAÍDAS       : JSON com registros e mensagens de validação.
 *
 * 🔗 CHAMADA POR  : Telas de Motoristas e grids do sistema.
 *
 * 🔄 CHAMA        : IUnitOfWork (Motorista, MotoristaContrato, ViewMotoristas).
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, Entity Framework.
 ****************************************************************************************/
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotoristaController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: MotoristaController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do UnitOfWork.
         *
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public MotoristaController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "MotoristaController.cs" ,
                    "MotoristaController" ,
                    error
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar motoristas com dados de contrato, fornecedor e usuário.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com lista de motoristas formatada para grid.
         *
         * 🔗 CHAMADA POR  : Grid principal de Motoristas.
         *
         * 🔄 CHAMA        : _unitOfWork.ViewMotoristas.GetAll().
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = (
                    from vm in _unitOfWork.ViewMotoristas.GetAll()

                    select new
                    {
                        vm.MotoristaId ,
                        vm.Nome ,
                        vm.Ponto ,
                        vm.CNH ,
                        vm.Celular01 ,
                        vm.CategoriaCNH ,

                        Sigla = vm.Sigla != null ? vm.Sigla : "" ,

                        ContratoMotorista = vm.AnoContrato != null
                            ? (
                                vm.AnoContrato
                                + "/"
                                + vm.NumeroContrato
                                + " - "
                                + vm.DescricaoFornecedor
                            )
                        : vm.TipoCondutor != null ? vm.TipoCondutor
                        : "(sem contrato)" ,

                        vm.Status ,

                        DatadeAlteracao = vm.DataAlteracao?.ToString("dd/MM/yy")
                            ?? string.Empty ,

                        vm.NomeCompleto ,

                        vm.EfetivoFerista ,

                        vm.Foto ,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "Get" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover motorista quando não há vínculos ativos com contratos.
         *
         * 📥 ENTRADAS     : [MotoristaViewModel] model (MotoristaId).
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de exclusão no grid.
         *
         * 🔄 CHAMA        : Motorista.GetFirstOrDefault(), MotoristaContrato.GetFirstOrDefault(),
         *                   Motorista.Remove(), Save().
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(MotoristaViewModel model)
        {
            try
            {
                if (model != null && model.MotoristaId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
                        u.MotoristaId == model.MotoristaId
                    );
                    if (objFromDb != null)
                    {
                        //Verifica se pode apagar o motorista
                        var motoristaContrato = _unitOfWork.MotoristaContrato.GetFirstOrDefault(u =>
                            u.MotoristaId == model.MotoristaId
                        );
                        if (motoristaContrato != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Não foi possível remover o motorista. Ele está associado a um ou mais contratos!" ,
                                }
                            );
                        }

                        _unitOfWork.Motorista.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Motorista removido com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar motorista"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "Delete" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusMotorista
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar status (ativo/inativo) do motorista.
         *
         * 📥 ENTRADAS     : Id (Guid) - identificador do motorista.
         *
         * 📤 SAÍDAS       : JSON com sucesso, mensagem e tipo.
         *
         * 🔗 CHAMADA POR  : Ações de ativação/inativação no grid.
         *
         * 🔄 CHAMA        : Motorista.GetFirstOrDefault(), Motorista.Update().
         ****************************************************************************************/
        [Route("UpdateStatusMotorista")]
        public JsonResult UpdateStatusMotorista(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
                        u.MotoristaId == Id
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
                                "Atualizado Status do Motorista [Nome: {0}] (Inativo)" ,
                                objFromDb.Nome
                            );
                            type = 1;
                        }
                        else
                        {
                            //res["success"] = 1;
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Motorista  [Nome: {0}] (Ativo)" ,
                                objFromDb.Nome
                            );
                            type = 0;
                        }
                        //_unitOfWork.Save();
                        _unitOfWork.Motorista.Update(objFromDb);
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
                    "MotoristaController.cs" ,
                    "UpdateStatusMotorista" ,
                    error
                );
                return new JsonResult(new
                {
                    sucesso = false
                }); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaFoto
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar objeto de motorista com foto em byte[].
         *
         * 📥 ENTRADAS     : id (Guid) - identificador do motorista.
         *
         * 📤 SAÍDAS       : JSON com objeto e foto convertida, ou false.
         *
         * 🔗 CHAMADA POR  : Tela de edição/detalhes.
         *
         * 🔄 CHAMA        : Motorista.GetFirstOrDefault(), GetImage().
         ****************************************************************************************/
        [HttpGet]
        [Route("PegaFoto")]
        public JsonResult PegaFoto(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
                        u.MotoristaId == id
                    );
                    if (objFromDb.Foto != null)
                    {
                        objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                        return Json(objFromDb);
                    }
                    return Json(false);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "PegaFoto" , error);
                return new JsonResult(new
                {
                    sucesso = false
                }); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaFotoModal
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar apenas a foto do motorista para exibição em modal.
         *
         * 📥 ENTRADAS     : id (Guid) - identificador do motorista.
         *
         * 📤 SAÍDAS       : JSON com byte[] da foto ou false.
         *
         * 🔗 CHAMADA POR  : Modal de visualização de foto.
         *
         * 🔄 CHAMA        : Motorista.GetFirstOrDefault(), GetImage().
         ****************************************************************************************/
        [HttpGet]
        [Route("PegaFotoModal")]
        public JsonResult PegaFotoModal(Guid id)
        {
            try
            {
                var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == id);
                if (objFromDb.Foto != null)
                {
                    objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                    return Json(objFromDb.Foto);
                }
                return Json(false);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "PegaFotoModal" , error);
                return new JsonResult(new
                {
                    sucesso = false
                }); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetImage
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Converter string base64 em array de bytes.
         *
         * 📥 ENTRADAS     : sBase64String (string).
         *
         * 📤 SAÍDAS       : [byte[]] imagem decodificada ou null.
         *
         * 🔗 CHAMADA POR  : PegaFoto(), PegaFotoModal().
         ****************************************************************************************/
        public byte[] GetImage(string sBase64String)
        {
            try
            {
                byte[] bytes = null;
                if (!string.IsNullOrEmpty(sBase64String))
                {
                    bytes = Convert.FromBase64String(sBase64String);
                }
                return bytes;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "GetImage" , error);
                return default(byte[]); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: MotoristaContratos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar motoristas associados a um contrato específico.
         *
         * 📥 ENTRADAS     : Id (Guid) - identificador do contrato.
         *
         * 📤 SAÍDAS       : JSON com lista de motoristas vinculados.
         *
         * 🔗 CHAMADA POR  : Grid de motoristas do contrato.
         *
         * 🔄 CHAMA        : ViewMotoristas.GetAll(), MotoristaContrato.GetAll() (join).
         ****************************************************************************************/
        [HttpGet]
        [Route("MotoristaContratos")]
        public IActionResult MotoristaContratos(Guid Id)
        {
            try
            {
                var result = (
                    from vm in _unitOfWork.ViewMotoristas.GetAll()

                    join mc in _unitOfWork.MotoristaContrato.GetAll()
                        on vm.MotoristaId equals mc.MotoristaId

                    where mc.ContratoId == Id

                    select new
                    {
                        vm.MotoristaId ,
                        vm.Nome ,
                        vm.Ponto ,
                        vm.CNH ,
                        vm.Celular01 ,
                        vm.CategoriaCNH ,

                        Sigla = vm.Sigla != null ? vm.Sigla : "" ,

                        ContratoMotorista = vm.AnoContrato != null
                            ? (
                                vm.AnoContrato
                                + "/"
                                + vm.NumeroContrato
                                + " - "
                                + vm.DescricaoFornecedor
                            )
                            : "<b>(Veículo Próprio)</b>" ,

                        vm.Status ,

                        DatadeAlteracao = vm.DataAlteracao?.ToString("dd/MM/yy")
                            ?? string.Empty ,

                        vm.NomeCompleto ,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "MotoristaController.cs" ,
                    "MotoristaContratos" ,
                    error
                );
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DeleteContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo do motorista com um contrato específico.
         *
         * 📥 ENTRADAS     : [MotoristaViewModel] model (MotoristaId, ContratoId).
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de remoção em grids de contrato.
         *
         * 🔄 CHAMA        : MotoristaContrato.GetFirstOrDefault(), MotoristaContrato.Remove(),
         *                   Motorista.Update(), Save().
         ****************************************************************************************/
        [Route("DeleteContrato")]
        [HttpPost]
        public IActionResult DeleteContrato(MotoristaViewModel model)
        {
            try
            {
                if (model != null && model.MotoristaId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
                        u.MotoristaId == model.MotoristaId
                    );
                    if (objFromDb != null)
                    {
                        //Verifica se pode apagar o motorista
                        var motoristaContrato = _unitOfWork.MotoristaContrato.GetFirstOrDefault(u =>
                            u.MotoristaId == model.MotoristaId && u.ContratoId == model.ContratoId
                        );
                        if (motoristaContrato != null)
                        {
                            if (objFromDb.ContratoId == model.ContratoId)
                            {
                                objFromDb.ContratoId = Guid.Empty;
                                _unitOfWork.Motorista.Update(objFromDb);
                            }
                            _unitOfWork.MotoristaContrato.Remove(motoristaContrato);
                            _unitOfWork.Save();
                            return Json(
                                new
                                {
                                    success = true ,
                                    message = "Motorista removido com sucesso"
                                }
                            );
                        }
                        return Json(new
                        {
                            success = false ,
                            message = "Erro ao remover motorista"
                        });
                    }
                    return Json(new
                    {
                        success = false ,
                        message = "Erro ao remover motorista"
                    });
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao remover motorista"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "DeleteContrato" , error);
                return View(); // padronizado
            }
        }
    }
}
