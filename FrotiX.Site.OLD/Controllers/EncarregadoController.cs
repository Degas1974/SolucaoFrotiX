using FrotiX.Helpers;

/*
 *  _________________________________________________________________________________________________________
 * |                                                                                                         |
 * |                                   FROTIX - SOLUÇÃO GESTÃO DE FROTAS                                     |
 * |_________________________________________________________________________________________________________|
 * |                                                                                                         |
 * | (IA) CAMADA: CONTROLLERS                                                                                |
 * | (IA) IDENTIDADE: EncarregadoController.cs                                                               |
 * | (IA) DESCRIÇÃO: CRUD de encarregados (supervisores) com upload de foto.                                 |
 * | (IA) PADRÃO: FrotiX 2026 Core (ASCII Hero Banner + XML Documentation)                                   |
 * |_________________________________________________________________________________________________________|
 */

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
    public class EncarregadoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: EncarregadoController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do Unit of Work
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork
         * 📤 SAÍDAS       : Instância configurada
         * 🔗 CHAMADA POR  : ASP.NET Core DI
         ****************************************************************************************/
        public EncarregadoController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("EncarregadoController.cs", "EncarregadoController", error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar todos os encarregados com dados de contrato e fornecedor
         *                   Utiliza LEFT JOIN para incluir encarregados sem contrato
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : [IActionResult] JSON com lista de encarregados formatados
         * 🔗 CHAMADA POR  : JavaScript (DataTables) da página Encarregados/Index
         * 🔄 CHAMA        : Encarregado.GetAll(), Contrato, Fornecedor, AspNetUsers
         *
         * 🔍 QUERY SQL:
         *    SELECT e.*, ct.*, f.*, us.*
         *    FROM Encarregado e
         *    LEFT JOIN Contrato ct ON e.ContratoId = ct.ContratoId
         *    LEFT JOIN Fornecedor f ON ct.FornecedorId = f.FornecedorId
         *    INNER JOIN AspNetUsers us ON e.UsuarioIdAlteracao = us.Id
         ****************************************************************************************/
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DOC] LINQ com LEFT JOIN para incluir encarregados sem contrato
                var result = (
                    from e in _unitOfWork.Encarregado.GetAll()

                    join ct in _unitOfWork.Contrato.GetAll()
                        on e.ContratoId equals ct.ContratoId
                        into ctr
                    from ctrResult in ctr.DefaultIfEmpty()

                    join f in _unitOfWork.Fornecedor.GetAll()
                        on ctrResult == null
                            ? Guid.Empty
                            : ctrResult.FornecedorId equals f.FornecedorId
                        into frd
                    from frdResult in frd.DefaultIfEmpty()

                    join us in _unitOfWork.AspNetUsers.GetAll()
                        on e.UsuarioIdAlteracao equals us.Id

                    select new
                    {
                        e.EncarregadoId,
                        e.Nome,
                        e.Ponto,
                        e.Celular01,

                        ContratoEncarregado = ctrResult != null
                            ? (
                                ctrResult.AnoContrato
                                + "/"
                                + ctrResult.NumeroContrato
                                + " - "
                                + frdResult.DescricaoFornecedor
                            )
                            : "<b>(Sem Contrato)</b>",

                        e.Status,
                        e.Foto,

                        DatadeAlteracao = e.DataAlteracao?.ToString("dd/MM/yy"),

                        us.NomeCompleto,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("EncarregadoController.cs", "Get", error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Excluir um encarregado do banco de dados
         *                   Valida se não possui vínculos com contratos antes de deletar
         * 📥 ENTRADAS     : [EncarregadoViewModel] model - contém EncarregadoId
         * 📤 SAÍDAS       : [IActionResult] JSON success/message
         * ⬅️ CHAMADO POR  : JavaScript (AJAX) da página Encarregados via DELETE
         * ➡️ CHAMA        : Encarregado.GetFirstOrDefault(), EncarregadoContrato.GetFirstOrDefault(),
         *                   Remove(), Save()
         * ⚠️  VALIDAÇÃO   : Bloqueia exclusão se houver EncarregadoContrato associado
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(EncarregadoViewModel model)
        {
            try
            {
                if (model != null && model.EncarregadoId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Encarregado.GetFirstOrDefault(u =>
                        u.EncarregadoId == model.EncarregadoId
                    );
                    if (objFromDb != null)
                    {
                        var encarregadoContrato = _unitOfWork.EncarregadoContrato.GetFirstOrDefault(u =>
                            u.EncarregadoId == model.EncarregadoId
                        );
                        if (encarregadoContrato != null)
                        {
                            return Json(
                                new
                                {
                                    success = false,
                                    message = "Não foi possível remover o encarregado. Ele está associado a um ou mais contratos!",
                                }
                            );
                        }

                        _unitOfWork.Encarregado.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true,
                                message = "Encarregado removido com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false,
                    message = "Erro ao apagar encarregado"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("EncarregadoController.cs", "Delete", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao apagar encarregado"
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusEncarregado
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar status do encarregado entre ativo e inativo
         *                   Retorna mensagem descritiva com o novo status
         * 📥 ENTRADAS     : [Guid] Id - EncarregadoId
         * 📤 SAÍDAS       : [JsonResult] { success: bool, message: string, type: int }
         * ⬅️ CHAMADO POR  : JavaScript (AJAX) ao clicar botão de ativar/desativar
         * ➡️ CHAMA        : Encarregado.GetFirstOrDefault(), Update(), Save()
         * 📝 OBSERVAÇÕES  : type=0 (ativo), type=1 (inativo) para feedback visual no frontend
         ****************************************************************************************/
        [Route("UpdateStatusEncarregado")]
        public JsonResult UpdateStatusEncarregado(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Encarregado.GetFirstOrDefault(u => u.EncarregadoId == Id);
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Encarregado [Nome: {0}] (Inativo)",
                                objFromDb.Nome
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Encarregado [Nome: {0}] (Ativo)",
                                objFromDb.Nome
                            );
                            type = 0;
                        }
                        _unitOfWork.Encarregado.Update(objFromDb);
                    }
                    return Json(
                        new
                        {
                            success = true,
                            message = Description,
                            type = type,
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
                Alerta.TratamentoErroComLinha("EncarregadoController.cs", "UpdateStatusEncarregado", error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaFoto
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar foto do encarregado e converter para Base64 para exibição
         * 📥 ENTRADAS     : [Guid] id - EncarregadoId
         * 📤 SAÍDAS       : [JsonResult] Objeto encarregado com foto em Base64 ou false
         * 🔗 CHAMADA POR  : JavaScript (AJAX) ao exibir foto no formulário
         * 🔄 CHAMA        : Encarregado.GetFirstOrDefault(), GetImage()
         * ⚠️  CONVERSÃO   : byte[] → Base64 String → byte[] (para compatibilidade)
         ****************************************************************************************/
        [HttpGet]
        [Route("PegaFoto")]
        public JsonResult PegaFoto(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Encarregado.GetFirstOrDefault(u =>
                        u.EncarregadoId == id
                    );
                    if (objFromDb.Foto != null)
                    {
                        // [DOC] Converte byte[] → Base64 → byte[] para exibição no frontend
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
                Alerta.TratamentoErroComLinha("EncarregadoController.cs", "PegaFoto", error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PegaFotoModal
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar foto do encarregado para exibição em modal
         *                   Converte byte[] para Base64 para envio ao frontend
         * 📥 ENTRADAS     : [Guid] id - EncarregadoId
         * 📤 SAÍDAS       : [JsonResult] Base64String da imagem ou false
         * ⬅️ CHAMADO POR  : JavaScript (AJAX) ao abrir modal de visualização de foto
         * ➡️ CHAMA        : Encarregado.GetFirstOrDefault(), GetImage()
         * 📝 OBSERVAÇÕES  : Similar a PegaFoto mas retorna apenas a imagem sem objeto completo
         ****************************************************************************************/
        [HttpGet]
        [Route("PegaFotoModal")]
        public JsonResult PegaFotoModal(Guid id)
        {
            try
            {
                var objFromDb = _unitOfWork.Encarregado.GetFirstOrDefault(u => u.EncarregadoId == id);
                if (objFromDb.Foto != null)
                {
                    objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                    return Json(objFromDb.Foto);
                }
                return Json(false);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("EncarregadoController.cs", "PegaFotoModal", error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetImage
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Converter string Base64 em array de bytes
         *                   Função utilitária para desserialização de imagens
         * 📥 ENTRADAS     : [string] sBase64String - String Base64 codificada
         * 📤 SAÍDAS       : [byte[]] Array de bytes da imagem ou null se vazio
         * ⬅️ CHAMADO POR  : PegaFoto(), PegaFotoModal()
         * ➡️ CHAMA        : Convert.FromBase64String()
         * 📝 OBSERVAÇÕES  : Retorna null se string for nula ou vazia
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
                Alerta.TratamentoErroComLinha("EncarregadoController.cs", "GetImage", error);
                return default(byte[]);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: EncarregadoContratos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar todos os encarregados vinculados a um contrato específico
         *                   Utiliza INNER JOIN com EncarregadoContrato
         * 📥 ENTRADAS     : [Guid] Id - ContratoId
         * 📤 SAÍDAS       : [IActionResult] JSON com lista de encarregados formatados
         * ⬅️ CHAMADO POR  : JavaScript (DataTables) da página de Contratos/Detalhes
         * ➡️ CHAMA        : Encarregado.GetAll(), EncarregadoContrato.GetAll()
         * 📝 OBSERVAÇÕES  : Retorna lista vazia se contrato não tiver encarregados
         ****************************************************************************************/
        [HttpGet]
        [Route("EncarregadoContratos")]
        public IActionResult EncarregadoContratos(Guid Id)
        {
            try
            {
                var result = (
                    from e in _unitOfWork.Encarregado.GetAll()

                    join ec in _unitOfWork.EncarregadoContrato.GetAll()
                        on e.EncarregadoId equals ec.EncarregadoId

                    where ec.ContratoId == Id

                    select new
                    {
                        e.EncarregadoId,
                        e.Nome,
                        e.Ponto,
                        e.Celular01,
                        e.Status,
                    }
                ).ToList();

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("EncarregadoController.cs", "EncarregadoContratos", error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DeleteContrato
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Desvincular um encarregado de um contrato específico
         *                   Remove registro de EncarregadoContrato (não deleta encarregado)
         * 📥 ENTRADAS     : [EncarregadoViewModel] model - contém EncarregadoId e ContratoId
         * 📤 SAÍDAS       : [IActionResult] JSON success/message
         * ⬅️ CHAMADO POR  : JavaScript (AJAX) da página de Contratos/Detalhes
         * ➡️ CHAMA        : EncarregadoContrato.GetFirstOrDefault(), Remove(), Save()
         * 📝 OBSERVAÇÕES  : Operação em cascata - não deleta encarregado, apenas desvincula
         ****************************************************************************************/
        [Route("DeleteContrato")]
        [HttpPost]
        public IActionResult DeleteContrato(EncarregadoViewModel model)
        {
            try
            {
                if (model != null && model.EncarregadoId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Encarregado.GetFirstOrDefault(u =>
                        u.EncarregadoId == model.EncarregadoId
                    );
                    if (objFromDb != null)
                    {
                        var encarregadoContrato = _unitOfWork.EncarregadoContrato.GetFirstOrDefault(u =>
                            u.EncarregadoId == model.EncarregadoId && u.ContratoId == model.ContratoId
                        );
                        if (encarregadoContrato != null)
                        {
                            if (objFromDb.ContratoId == model.ContratoId)
                            {
                                objFromDb.ContratoId = Guid.Empty;
                                _unitOfWork.Encarregado.Update(objFromDb);
                            }
                            _unitOfWork.EncarregadoContrato.Remove(encarregadoContrato);
                            _unitOfWork.Save();
                            return Json(
                                new
                                {
                                    success = true,
                                    message = "Encarregado removido com sucesso"
                                }
                            );
                        }
                        return Json(new
                        {
                            success = false,
                            message = "Erro ao remover encarregado"
                        });
                    }
                    return Json(new
                    {
                        success = false,
                        message = "Erro ao remover encarregado"
                    });
                }
                return Json(new
                {
                    success = false,
                    message = "Erro ao remover encarregado"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("EncarregadoController.cs", "DeleteContrato", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao remover encarregado"
                });
            }
        }
    }
}
