/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: SetorController.cs                                               ║
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
     * ⚡ CONTROLLER: Setor API (SetorPatrimonial)
     * 🎯 OBJETIVO: Gerenciar setores patrimoniais do sistema
     * 📋 ROTAS: /api/Setor/* (ListaSetores, UpdateStatusSetor, Delete, ListaSetoresCombo)
     * 🔗 ENTIDADES: SetorPatrimonial, SecaoPatrimonial, AspNetUsers
     * 📦 DEPENDÊNCIAS: IUnitOfWork
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class SetorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetorController(IUnitOfWork unitOfWork)
        {
            try
            {
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorController.cs" , "SetorController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaSetores
         * 🎯 OBJETIVO: Listar todos os setores patrimoniais com seus detentores
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { success, data: List<{ SetorId, NomeSetor, NomeCompleto, Status, SetorBaixa }> }
         * 🔗 CHAMADA POR: Grid de setores patrimoniais
         * 🔄 CHAMA: SetorPatrimonial.GetAll(), AspNetUsers.GetAll()
         * 🔀 JOIN: SetorPatrimonial com AspNetUsers (detentor)
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaSetores")]
        public IActionResult ListaSetores()
        {
            try
            {
                // [DOC] Join com usuários para obter nome do detentor do setor
                var setores = _unitOfWork
                    .SetorPatrimonial.GetAll()
                    .Join(
                        _unitOfWork.AspNetUsers.GetAll() ,
                        setor => setor.DetentorId ,
                        usuario => usuario.Id ,
                        (setor , usuario) => new
                        {
                            SetorId = setor.SetorId ,
                            NomeSetor = setor.NomeSetor ,
                            NomeCompleto = usuario.NomeCompleto ,
                            Status = setor.Status ,
                            SetorBaixa = setor.SetorBaixa
                        }
                    )
                    .OrderBy(x => x.NomeSetor).ToList();

                return Json(new
                {
                    success = true ,
                    data = setores
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorController.cs" , "ListaSetores" , error);
                return Json(
                    new
                    {
                        success = false ,
                        data = new List<object>() ,
                        message = $"Erro ao carregar setores: {error.Message}" ,
                    }
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UpdateStatusSetor
         * 🎯 OBJETIVO: Alternar status do setor patrimonial (Ativo ↔ Inativo)
         * 📥 ENTRADAS: Id (Guid do setor)
         * 📤 SAÍDAS: JSON { success, message, type (0=ativo, 1=inativo) }
         * 🔗 CHAMADA POR: Toggle de status no grid
         * 🔄 CHAMA: SetorPatrimonial.GetFirstOrDefault(), SetorPatrimonial.Update()
         ****************************************************************************************/
        [Route("UpdateStatusSetor")]
        public JsonResult UpdateStatusSetor(Guid Id)
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(u =>
                        u.SetorId == Id
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
                                "Atualizado Status do Setor [Nome: {0}] (Inativo)" ,
                                objFromDb.NomeSetor
                            );
                            type = 1;
                        }
                        else
                        {
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Setor [Nome: {0}] (Ativo)" ,
                                objFromDb.NomeSetor
                            );
                            type = 0;
                        }
                        _unitOfWork.SetorPatrimonial.Update(objFromDb);
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
                    "SetorPatrimonialController.cs" ,
                    "UpdateStatusSetor" ,
                    error
                );
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Delete
         * 🎯 OBJETIVO: Excluir setor patrimonial (valida dependências antes de remover)
         * 📥 ENTRADAS: id (Guid do setor)
         * 📤 SAÍDAS: JSON { success, message }
         * 🔗 CHAMADA POR: Modal de exclusão de setor
         * 🔄 CHAMA: SetorPatrimonial.GetFirstOrDefault(), SecaoPatrimonial.GetFirstOrDefault(), SetorPatrimonial.Remove()
         * ⚠️ VALIDAÇÃO: Impede exclusão se existirem seções associadas
         ****************************************************************************************/
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete([FromBody] Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(u =>
                        u.SetorId == id
                    );
                    if (objFromDb != null)
                    {
                        // [DOC] Valida integridade referencial: não permite excluir setor com seções
                        var secao = _unitOfWork.SecaoPatrimonial.GetFirstOrDefault(u =>
                            u.SetorId == id
                        );
                        if (secao != null)
                        {
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem seções associadas a esse setor" ,
                                }
                            );
                        }
                        _unitOfWork.SetorPatrimonial.Remove(objFromDb);
                        _unitOfWork.Save();
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Setor removido com sucesso"
                            }
                        );
                    }
                }
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Setor"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorPatrimonialController.cs" , "Delete" , error);
                return StatusCode(500);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ListaSetoresCombo
         * 🎯 OBJETIVO: Listar setores ativos para combobox (dropdown)
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { success, data: List<{ text, value }> }
         * 🔗 CHAMADA POR: Combobox de setores em formulários
         * 🔄 CHAMA: SetorPatrimonial.GetAll()
         * 🔍 FILTRO: Apenas setores com Status = true
         ****************************************************************************************/
        [HttpGet]
        [Route("ListaSetoresCombo")]
        public IActionResult ListaSetoresCombo()
        {
            try
            {
                // [DOC] Filtra apenas setores ativos e formata para combobox (text/value)
                var setores = _unitOfWork
                    .SetorPatrimonial.GetAll()
                    .Where(s => s.Status == true)
                    .OrderBy(s => s.NomeSetor)
                    .Select(s => new { text = s.NomeSetor , value = s.SetorId })
                    .ToList();

                return Json(new
                {
                    success = true ,
                    data = setores
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorController.cs" , "ListaSetores" , error);
                return Json(
                    new
                    {
                        success = false ,
                        data = new List<object>() ,
                        message = $"Erro ao carregar setores: {error.Message}" ,
                    }
                );
            }
        }
    }
}
