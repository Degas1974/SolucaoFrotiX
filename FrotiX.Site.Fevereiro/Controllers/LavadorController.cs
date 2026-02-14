/* ****************************************************************************************
 * ⚡ ARQUIVO: LavadorController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar lavadores (equipe de limpeza de veículos), incluindo CRUD,
 *                   consulta de vínculos com contratos e operações de status/foto.
 *
 * 📥 ENTRADAS     : ViewModels, IDs, filtros e parâmetros de atualização.
 *
 * 📤 SAÍDAS       : JSON com dados de lavadores, mensagens de sucesso/erro e imagens.
 *
 * 🔗 CHAMADA POR  : Pages/Lavadores/Index e chamadas AJAX do frontend.
 *
 * 🔄 CHAMA        : Repositórios via IUnitOfWork (Lavador, Contrato, Fornecedor, Users).
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, Entity Framework, LINQ.
 *
 * 📝 OBSERVAÇÕES  : Inclui endpoints para foto e manutenção de vínculos com contratos.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: LavadorController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor operações de listagem, exclusão, status e vínculos de lavadores.
 *
 * 📥 ENTRADAS     : LavadorViewModel, IDs e filtros de contrato.
 *
 * 📤 SAÍDAS       : JSON com registros e mensagens de validação.
 *
 * 🔗 CHAMADA POR  : Páginas de Lavadores e grids AJAX.
 *
 * 🔄 CHAMA        : IUnitOfWork (Lavador, Contrato, Fornecedor, AspNetUsers, LavadorContrato).
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
    public class LavadorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: LavadorController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do UnitOfWork.
         *
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public LavadorController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "LavadorController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar lavadores com dados de contrato, fornecedor e usuário.
         *
         * 📥 ENTRADAS     : Nenhuma (requisição GET).
         *
         * 📤 SAÍDAS       : JSON com lista de lavadores formatada para grid.
         *
         * 🔗 CHAMADA POR  : Grid principal de Lavadores.
         *
         * 🔄 CHAMA        : IUnitOfWork.Lavador/Contrato/Fornecedor/AspNetUsers (joins).
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = (
                    from l in _unitOfWork.Lavador.GetAll()

                    join ct in _unitOfWork.Contrato.GetAll()
                        on l.ContratoId equals ct.ContratoId
                        into ctr
                    from ctrResult in ctr.DefaultIfEmpty() // <= Left Join

                    join f in _unitOfWork.Fornecedor.GetAll()
                        on ctrResult == null
                            ? Guid.Empty
                            : ctrResult.FornecedorId equals f.FornecedorId
                        into frd
                    from frdResult in frd.DefaultIfEmpty() // <= Left Join

                    join us in _unitOfWork.AspNetUsers.GetAll()
                        on l.UsuarioIdAlteracao equals us.Id

                    select new
                    {
                        l.LavadorId ,
                        l.Nome ,
                        l.Ponto ,
                        l.Celular01 ,

                        ContratoLavador = ctrResult != null
                            ? (
                                ctrResult.AnoContrato
                                + "/"
                                + ctrResult.NumeroContrato
                                + " - "
                                + frdResult.DescricaoFornecedor
                            )
                            : "<b>(Sem Contrato)</b>" ,

                        l.Status ,
                        l.Foto ,

                        DatadeAlteracao = l.DataAlteracao.HasValue ? l.DataAlteracao.Value.ToString("dd/MM/yy") : string.Empty ,

                        us.NomeCompleto ,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "Get" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover lavador quando não há vínculo ativo com contratos.
         *
         * 📥 ENTRADAS     : [LavadorViewModel] model (LavadorId).
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de exclusão no grid.
         *
         * 🔄 CHAMA        : Lavador.GetFirstOrDefault(), LavadorContrato.GetFirstOrDefault(),
         *                   Lavador.Remove(), Save().
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(LavadorViewModel model)
        {
            try
            {
                if (model != null && model.LavadorId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u =>
                        u.LavadorId == model.LavadorId
                    );
                    if (objFromDb != null)
                    {
                        //Verifica se pode apagar o operador
                        var lavadorContrato = _unitOfWork.LavadorContrato.GetFirstOrDefault(u =>
                            u.LavadorId == model.LavadorId
                        );
                        if (lavadorContrato != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Não foi possível remover o lavador. Ele está associado a um ou mais contratos!" ,
                                }
                            );
                        }

                        _unitOfWork.Lavador.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Lavador removido com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar lavador"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "Delete" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusLavador
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar status (ativo/inativo) do lavador.
         *
         * 📥 ENTRADAS     : Id (Guid) - identificador do lavador.
         *
         * 📤 SAÍDAS       : JSON com sucesso, mensagem e tipo.
         *
         * 🔗 CHAMADA POR  : Ações de ativação/inativação no grid.
         *
         * 🔄 CHAMA        : Lavador.GetFirstOrDefault(), Lavador.Update().
         ****************************************************************************************/
        [Route("UpdateStatusLavador")]
        public JsonResult UpdateStatusLavador(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u => u.LavadorId == Id);
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            //res["success"] = 0;
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Lavador [Nome: {0}] (Inativo)" ,
                                objFromDb.Nome
                            );
                            type = 1;
                        }
                        else
                        {
                            //res["success"] = 1;
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Lavador  [Nome: {0}] (Ativo)" ,
                                objFromDb.Nome
                            );
                            type = 0;
                        }
                        //_unitOfWork.Save();
                        _unitOfWork.Lavador.Update(objFromDb);
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
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "UpdateStatusLavador" , error);
                return new JsonResult(new
                {
                    sucesso = false
                }); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaFoto
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar foto do lavador em formato byte[] (base64 convertido).
         *
         * 📥 ENTRADAS     : id (Guid) - identificador do lavador.
         *
         * 📤 SAÍDAS       : JSON com objeto de lavador e foto ou false.
         *
         * 🔗 CHAMADA POR  : Tela de detalhes/edição.
         *
         * 🔄 CHAMA        : Lavador.GetFirstOrDefault(), GetImage().
         ****************************************************************************************/
        [HttpGet]
        [Route("PegaFoto")]
        public JsonResult PegaFoto(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u =>
                        u.LavadorId == id
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
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "PegaFoto" , error);
                return new JsonResult(new
                {
                    sucesso = false
                }); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaFotoModal
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar apenas a foto do lavador para uso em modal.
         *
         * 📥 ENTRADAS     : id (Guid) - identificador do lavador.
         *
         * 📤 SAÍDAS       : JSON com byte[] da foto ou false.
         *
         * 🔗 CHAMADA POR  : Modal de visualização de foto.
         *
         * 🔄 CHAMA        : Lavador.GetFirstOrDefault(), GetImage().
         ****************************************************************************************/
        [HttpGet]
        [Route("PegaFotoModal")]
        public JsonResult PegaFotoModal(Guid id)
        {
            try
            {
                var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u => u.LavadorId == id);
                if (objFromDb.Foto != null)
                {
                    objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                    return Json(objFromDb.Foto);
                }
                return Json(false);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "PegaFotoModal" , error);
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
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "GetImage" , error);
                return default(byte[]); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: LavadorContratos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar lavadores associados a um contrato específico.
         *
         * 📥 ENTRADAS     : Id (Guid) - identificador do contrato.
         *
         * 📤 SAÍDAS       : JSON com lista de lavadores vinculados.
         *
         * 🔗 CHAMADA POR  : Grid de lavadores do contrato.
         *
         * 🔄 CHAMA        : Lavador.GetAll(), LavadorContrato.GetAll() (join).
         ****************************************************************************************/
        [HttpGet]
        [Route("LavadorContratos")]
        public IActionResult LavadorContratos(Guid Id)
        {
            try
            {
                var result = (
                    from m in _unitOfWork.Lavador.GetAll()

                    join lc in _unitOfWork.LavadorContrato.GetAll()
                        on m.LavadorId equals lc.LavadorId

                    where lc.ContratoId == Id

                    select new
                    {
                        m.LavadorId ,
                        m.Nome ,
                        m.Ponto ,
                        m.Celular01 ,
                        m.Status ,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "LavadorContratos" , error);
                return View(); // padronizado
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DeleteContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover vínculo do lavador com um contrato específico.
         *
         * 📥 ENTRADAS     : [LavadorViewModel] model (LavadorId, ContratoId).
         *
         * 📤 SAÍDAS       : JSON com mensagem de sucesso/erro.
         *
         * 🔗 CHAMADA POR  : Ações de remoção em grids de contrato.
         *
         * 🔄 CHAMA        : LavadorContrato.GetFirstOrDefault(), LavadorContrato.Remove(),
         *                   Lavador.Update(), Save().
         ****************************************************************************************/
        [Route("DeleteContrato")]
        [HttpPost]
        public IActionResult DeleteContrato(LavadorViewModel model)
        {
            try
            {
                if (model != null && model.LavadorId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Lavador.GetFirstOrDefault(u =>
                        u.LavadorId == model.LavadorId
                    );
                    if (objFromDb != null)
                    {
                        //Verifica se pode apagar o Lavador
                        var lavadorContrato = _unitOfWork.LavadorContrato.GetFirstOrDefault(u =>
                            u.LavadorId == model.LavadorId && u.ContratoId == model.ContratoId
                        );
                        if (lavadorContrato != null)
                        {
                            if (objFromDb.ContratoId == model.ContratoId)
                            {
                                objFromDb.ContratoId = Guid.Empty;
                                _unitOfWork.Lavador.Update(objFromDb);
                            }
                            _unitOfWork.LavadorContrato.Remove(lavadorContrato);
                            _unitOfWork.Save();
                            return Json(
                                new
                                {
                                    success = true ,
                                    message = "Lavador removido com sucesso"
                                }
                            );
                        }
                        return Json(new
                        {
                            success = false ,
                            message = "Erro ao remover lavador"
                        });
                    }
                    return Json(new
                    {
                        success = false ,
                        message = "Erro ao remover lavador"
                    });
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao remover lavador"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LavadorController.cs" , "DeleteContrato" , error);
                return View(); // padronizado
            }
        }
    }
}
