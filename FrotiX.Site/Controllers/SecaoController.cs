/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: SecaoController.cs                                                                      ║
   ║ 📂 CAMINHO: /Controllers                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: API para Seções Patrimoniais (subdivisões de Setores para organização de patrimônio).  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: ListaSecoes(), ListaSecoesCombo(), UpdateStatusSecao() - JOIN SetorPatrimonial           ║
   ║ 🔗 DEPS: IUnitOfWork (SecaoPatrimonial, SetorPatrimonial) | 📅 28/01/2026 | 👤 Copilot | 📝 v2.0    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
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
