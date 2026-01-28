/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: SecaoController.cs                                               ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: Secao API
     * 🎯 OBJETIVO: Gerenciar seções patrimoniais (subdivisões de setores)
     * 📋 ROTAS: /api/Secao/*
     * 🔗 ENTIDADES: SecaoPatrimonial, SetorPatrimonial
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class SecaoController :Controller
    {
        private readonly IUnitOfWork _unitOfWork;

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
         * 🎯 OBJETIVO: Listar todas as seções patrimoniais com seus setores
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { success, data: List<{ SecaoId, NomeSecao, SetorId, Status, NomeSetor }> }
         * 🔗 CHAMADA POR: Grid de seções patrimoniais
         * 🔄 CHAMA: SecaoPatrimonial.GetAll(), SetorPatrimonial.GetAll()
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaSecoes")]
        public IActionResult ListaSecoes()
        {
            try
            {
                // [DOC] Inner join: combina seções com setores patrimoniais
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
         * 🎯 OBJETIVO: Listar seções ativas de um setor específico para combobox (dropdown)
         * 📥 ENTRADAS: setorSelecionado (Guid? - pode ser null)
         * 📤 SAÍDAS: JSON { success, data: List<{ text, value }> }
         * 🔗 CHAMADA POR: Combobox de seções em formulários
         * 🔄 CHAMA: SecaoPatrimonial.GetAll()
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaSecoesCombo")]
        public IActionResult ListaSecoesCombo(Guid? setorSelecionado)
        {
            try
            {
                // [DOC] Se setor não informado, retorna lista vazia (válido para limpar combo)
                if (!setorSelecionado.HasValue || setorSelecionado == Guid.Empty)
                {
                    return Json(new
                    {
                        success = true ,
                        data = new List<object>()
                    });
                }

                // [DOC] Filtra apenas seções ativas do setor selecionado
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
         * 🎯 OBJETIVO: Alternar status da seção patrimonial (Ativo ↔ Inativo)
         * 📥 ENTRADAS: Id (Guid da seção)
         * 📤 SAÍDAS: JSON { success, message, type (0=ativo, 1=inativo) }
         * 🔗 CHAMADA POR: Toggle de status no grid
         * 🔄 CHAMA: SecaoPatrimonial.GetFirstOrDefault(), SecaoPatrimonial.Update()
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
                        // [DOC] Toggle status: true → false (type=1) ou false → true (type=0)
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
