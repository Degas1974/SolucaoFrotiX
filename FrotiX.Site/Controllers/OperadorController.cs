/* ****************************************************************************************
 * ⚡ ARQUIVO: OperadorController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar operadores (funcionários de fornecedores) com CRUD,
 *                   vínculos de contrato e upload de fotos.
 *
 * 📥 ENTRADAS     : ViewModels, IDs e parâmetros de status/foto.
 *
 * 📤 SAÍDAS       : JSON com dados e mensagens de operação.
 *
 * 🔗 CHAMADA POR  : Telas de operadores e grids AJAX.
 *
 * 🔄 CHAMA        : IUnitOfWork (Operador, Contrato, Fornecedor, OperadorContrato, AspNetUsers).
 **************************************************************************************** */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: OperadorController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints de listagem, exclusão, status e vínculos de operadores.
     *
     * 📥 ENTRADAS     : IDs e view models.
     *
     * 📤 SAÍDAS       : JSON com registros e mensagens.
     *
     * 🔗 CHAMADA POR  : Páginas de operadores e modais de contrato.
     ****************************************************************************************/

    [Route("api/[controller]")]
    [ApiController]
    public class OperadorController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: OperadorController
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializa controller com injeção de dependência do UnitOfWork
         * 📥 ENTRADAS     : IUnitOfWork
         * 📤 SAÍDAS       : Instância do controller
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         * 🔄 CHAMA        : Alerta.TratamentoErroComLinha (se erro)
         * 📦 DEPENDÊNCIAS : IUnitOfWork, Alerta.js
         ****************************************************************************************/
        public OperadorController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "OperadorController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Lista todos operadores com dados de contrato, fornecedor e usuário alteração
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : JSON com data (array de operadores enriquecidos)
         * 🔗 CHAMADA POR  : Grid de operadores via GET /api/Operador
         * 🔄 CHAMA        : _unitOfWork (Operador, Contrato, Fornecedor, AspNetUsers)
         * 📦 DEPENDÊNCIAS : LINQ joins, Alerta.TratamentoErroComLinha
         * 📝 OBSERVAÇÃO   : [DOC] Usa LEFT JOINs para incluir operadores sem contrato
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = (
                    from o in _unitOfWork.Operador.GetAll()

                    join ct in _unitOfWork.Contrato.GetAll()
                        on o.ContratoId equals ct.ContratoId
                        into ctr
                    from ctrResult in ctr.DefaultIfEmpty()

                    join f in _unitOfWork.Fornecedor.GetAll()
                        on ctrResult == null
                            ? Guid.Empty
                            : ctrResult.FornecedorId equals f.FornecedorId
                        into frd
                    from frdResult in frd.DefaultIfEmpty()

                    join us in _unitOfWork.AspNetUsers.GetAll()
                        on o.UsuarioIdAlteracao equals us.Id

                    select new
                    {
                        o.OperadorId ,
                        o.Nome ,
                        o.Ponto ,
                        o.Celular01 ,

                        ContratoOperador = ctrResult != null
                            ? (
                                ctrResult.AnoContrato
                                + "/"
                                + ctrResult.NumeroContrato
                                + " - "
                                + frdResult.DescricaoFornecedor
                            )
                            : "<b>(Sem Contrato)</b>" ,

                        o.Status ,
                        o.Foto ,

                        DatadeAlteracao = o.DataAlteracao?.ToString("dd/MM/yy") ,

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
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "Get" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remove operador se não estiver associado a contratos
         * 📥 ENTRADAS     : OperadorViewModel (OperadorId)
         * 📤 SAÍDAS       : JSON com success e message
         * 🔗 CHAMADA POR  : Botão "Excluir" na grid via POST /api/Operador/Delete
         * 🔄 CHAMA        : _unitOfWork (Operador, OperadorContrato)
         * 📦 DEPENDÊNCIAS : Alerta.TratamentoErroComLinha
         * 📝 OBSERVAÇÃO   : [DOC] Valida se operador está vinculado a contratos antes de excluir
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(OperadorViewModel model)
        {
            try
            {
                if (model != null && model.OperadorId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u =>
                        u.OperadorId == model.OperadorId
                    );
                    if (objFromDb != null)
                    {
                        var operadorContrato = _unitOfWork.OperadorContrato.GetFirstOrDefault(u =>
                            u.OperadorId == model.OperadorId
                        );
                        if (operadorContrato != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Não foi possível remover o operador. Ele está associado a um ou mais contratos!" ,
                                }
                            );
                        }

                        _unitOfWork.Operador.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Operador removido com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar operador"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "Delete" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar operador"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusOperador
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alterna status Ativo/Inativo do operador
         * 📥 ENTRADAS     : Id (Guid do operador)
         * 📤 SAÍDAS       : JSON com success, message, type (0=ativo, 1=inativo)
         * 🔗 CHAMADA POR  : Toggle de status na grid via GET /api/Operador/UpdateStatusOperador
         * 🔄 CHAMA        : _unitOfWork.Operador (GetFirstOrDefault, Update)
         * 📦 DEPENDÊNCIAS : Alerta.TratamentoErroComLinha
         * 📝 OBSERVAÇÃO   : [DOC] Não salva automaticamente (_unitOfWork.Save não é chamado)
         ****************************************************************************************/
        [Route("UpdateStatusOperador")]
        public JsonResult UpdateStatusOperador(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u => u.OperadorId == Id);
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Operador [Nome: {0}] (Inativo)" ,
                                objFromDb.Nome
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Operador  [Nome: {0}] (Ativo)" ,
                                objFromDb.Nome
                            );
                            type = 0;
                        }
                        _unitOfWork.Operador.Update(objFromDb);
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
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "UpdateStatusOperador" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaFoto
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retorna foto do operador convertida de Base64
         * 📥 ENTRADAS     : id (Guid do operador)
         * 📤 SAÍDAS       : JSON com objeto Operador (Foto convertida) ou false
         * 🔗 CHAMADA POR  : Exibição de perfil via GET /api/Operador/PegaFoto
         * 🔄 CHAMA        : _unitOfWork.Operador.GetFirstOrDefault, GetImage()
         * 📦 DEPENDÊNCIAS : GetImage (método interno), Alerta.TratamentoErroComLinha
         ****************************************************************************************/
        [HttpGet]
        [Route("PegaFoto")]
        public JsonResult PegaFoto(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u =>
                        u.OperadorId == id
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
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "PegaFoto" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaFotoModal
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retorna apenas foto convertida (sem objeto completo) para modals
         * 📥 ENTRADAS     : id (Guid do operador)
         * 📤 SAÍDAS       : JSON com byte[] da foto ou false
         * 🔗 CHAMADA POR  : Modais de visualização via GET /api/Operador/PegaFotoModal
         * 🔄 CHAMA        : _unitOfWork.Operador.GetFirstOrDefault, GetImage()
         * 📦 DEPENDÊNCIAS : GetImage (método interno), Alerta.TratamentoErroComLinha
         ****************************************************************************************/
        [HttpGet]
        [Route("PegaFotoModal")]
        public JsonResult PegaFotoModal(Guid id)
        {
            try
            {
                var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u => u.OperadorId == id);
                if (objFromDb.Foto != null)
                {
                    objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                    return Json(objFromDb.Foto);
                }
                return Json(false);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "PegaFotoModal" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetImage (Helper)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Converte string Base64 em byte array
         * 📥 ENTRADAS     : sBase64String (string)
         * 📤 SAÍDAS       : byte[] da imagem ou null
         * 🔗 CHAMADA POR  : PegaFoto, PegaFotoModal
         * 🔄 CHAMA        : Convert.FromBase64String
         * 📦 DEPENDÊNCIAS : System.Convert, Alerta.TratamentoErroComLinha
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
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "GetImage" , error);
                return default(byte[]);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: OperadorContratos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Lista operadores associados a um contrato específico
         * 📥 ENTRADAS     : Id (Guid do contrato)
         * 📤 SAÍDAS       : JSON com data (array de operadores vinculados)
         * 🔗 CHAMADA POR  : Grid de operadores por contrato via GET /api/Operador/OperadorContratos
         * 🔄 CHAMA        : _unitOfWork (Operador, OperadorContrato)
         * 📦 DEPENDÊNCIAS : LINQ join, Alerta.TratamentoErroComLinha
         ****************************************************************************************/
        [HttpGet]
        [Route("OperadorContratos")]
        public IActionResult OperadorContratos(Guid Id)
        {
            try
            {
                var result = (
                    from m in _unitOfWork.Operador.GetAll()

                    join oc in _unitOfWork.OperadorContrato.GetAll()
                        on m.OperadorId equals oc.OperadorId

                    where oc.ContratoId == Id

                    select new
                    {
                        m.OperadorId ,
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
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "OperadorContratos" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DeleteContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remove associação entre operador e contrato
         * 📥 ENTRADAS     : OperadorViewModel (OperadorId, ContratoId)
         * 📤 SAÍDAS       : JSON com success e message
         * 🔗 CHAMADA POR  : Botão "Remover Contrato" via POST /api/Operador/DeleteContrato
         * 🔄 CHAMA        : _unitOfWork (Operador, OperadorContrato)
         * 📦 DEPENDÊNCIAS : Alerta.TratamentoErroComLinha
         * 📝 OBSERVAÇÃO   : [DOC] Limpa ContratoId do operador se for o contrato principal
         ****************************************************************************************/
        [Route("DeleteContrato")]
        [HttpPost]
        public IActionResult DeleteContrato(OperadorViewModel model)
        {
            try
            {
                if (model != null && model.OperadorId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u =>
                        u.OperadorId == model.OperadorId
                    );
                    if (objFromDb != null)
                    {
                        var operadorContrato = _unitOfWork.OperadorContrato.GetFirstOrDefault(u =>
                            u.OperadorId == model.OperadorId && u.ContratoId == model.ContratoId
                        );
                        if (operadorContrato != null)
                        {
                            if (objFromDb.ContratoId == model.ContratoId)
                            {
                                objFromDb.ContratoId = Guid.Empty;
                                _unitOfWork.Operador.Update(objFromDb);
                            }
                            _unitOfWork.OperadorContrato.Remove(operadorContrato);
                            _unitOfWork.Save();
                            return Json(
                                new
                                {
                                    success = true ,
                                    message = "Operador removido com sucesso"
                                }
                            );
                        }
                        return Json(new
                        {
                            success = false ,
                            message = "Erro ao remover operador"
                        });
                    }
                    return Json(new
                    {
                        success = false ,
                        message = "Erro ao remover operador"
                    });
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao remover operador"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "DeleteContrato" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao remover operador"
                });
            }
        }
    }
}
