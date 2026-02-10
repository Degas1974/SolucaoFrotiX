/* ****************************************************************************************
 * ⚡ ARQUIVO: SecaoController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar seções patrimoniais vinculadas a setores.
 *
 * 📥 ENTRADAS     : IDs e filtros de seção/setor.
 *
 * 📤 SAÍDAS       : JSON com listas e status de operações.
 *
 * 🔗 CHAMADA POR  : Telas de cadastro e filtros patrimoniais.
 *
 * 🔄 CHAMA        : IUnitOfWork.SecaoPatrimonial, IUnitOfWork.SetorPatrimonial.
 **************************************************************************************** */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: SecaoController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints para listar seções e alternar status.
     *
     * 📥 ENTRADAS     : IDs e filtros.
     *
     * 📤 SAÍDAS       : JSON com dados e mensagens.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class SecaoController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: SecaoController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependência do UnitOfWork.
         *
         * 📥 ENTRADAS     : unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public SecaoController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SecaoController.cs" , "SecaoController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaSecoes
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar seções com nome do setor associado.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com data (lista de seções).
         *
         * 🔗 CHAMADA POR  : Grid de seções patrimoniais.
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaSecoes")]
        public IActionResult ListaSecoes()
        {
            try
            {

                var secoes = _unitOfWork
                    .SecaoPatrimonial.GetAll()
                    .Join(
                        _unitOfWork.SetorPatrimonial.GetAll() ,
                        secao => secao.SetorId ,
                        setor => setor.SetorId ,
                        (secao , setor) => new
                        {
                            SecaoId = secao.SecaoId ,
                            NomeSecao = secao.NomeSecao ,
                            SetorId = secao.SetorId ,
                            Status = secao.Status ,
                            NomeSetor = setor.NomeSetor
                        }
                    )
                    .OrderBy(x => x.NomeSecao).ToList();

                return Json(new
                {
                    success = true ,
                    data = secoes
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SecaoController.cs" , "ListaSecoes" , error);
                return Json(
                    new
                    {
                        success = false ,
                        data = new List<object>() ,
                        message = $"Erro ao carregar seções: {error.Message}" ,
                    }
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaSecoesCombo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar seções ativas por setor selecionado.
         *
         * 📥 ENTRADAS     : setorSelecionado (Guid?).
         *
         * 📤 SAÍDAS       : JSON com lista de seções (text/value).
         *
         * 🔗 CHAMADA POR  : Combos dependentes de setor.
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaSecoesCombo")]
        public IActionResult ListaSecoesCombo(Guid? setorSelecionado)
        {
            try
            {
                if (!setorSelecionado.HasValue || setorSelecionado == Guid.Empty)
                {
                    return Json(new
                    {
                        success = true ,
                        data = new List<object>()
                    });
                }

                var secoes = _unitOfWork
                    .SecaoPatrimonial.GetAll()
                    .Where(s => s.SetorId == setorSelecionado && s.Status == true)
                    .OrderBy(s => s.NomeSecao)
                    .Select(s => new { text = s.NomeSecao , value = s.SecaoId })
                    .ToList();

                return Json(new
                {
                    success = true ,
                    data = secoes
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SecaoController.cs" , "ListaSecoes" , error);
                return Json(
                    new
                    {
                        success = false ,
                        data = new List<object>() ,
                        message = $"Erro ao carregar seções: {error.Message}" ,
                    }
                );
            }
        }
        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusSecao
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Alternar o status ativo/inativo de uma seção.
         *
         * 📥 ENTRADAS     : Id (Guid da seção).
         *
         * 📤 SAÍDAS       : JSON com success, message e type.
         *
         * 🔗 CHAMADA POR  : Ação de ativar/desativar seção.
         ****************************************************************************************/
        [Route("UpdateStatusSecao")]
        public JsonResult UpdateStatusSecao(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.SecaoPatrimonial.GetFirstOrDefault(u =>
                        u.SecaoId == Id
                    );
                    string Description = "";
                    int type = 0;
                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status da Seção [Nome: {0}] (Inativo)" ,
                                objFromDb.NomeSecao
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status da Seção [Nome: {0}] (Ativo)" ,
                                objFromDb.NomeSecao
                            );
                            type = 0;
                        }
                        _unitOfWork.SecaoPatrimonial.Update(objFromDb);
                        _unitOfWork.Save();
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
                    "SecaoController.cs" ,
                    "UpdateStatusSecao" ,
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
