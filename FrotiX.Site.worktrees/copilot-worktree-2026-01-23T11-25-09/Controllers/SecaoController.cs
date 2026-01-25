/*
 *  _____________________________________________________________
 * |                                                             |
 * |   FrotiX Core - Gestão de Seções (Core Stack)               |
 * |_____________________________________________________________|
 *
 * (IA) Controlador responsável pela gestão de seções patrimoniais,
 * estruturando a hierarquia organizacional para inventário e bens.
 */

using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: SecaoController                                                   ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de seções patrimoniais e hierarquia organizacional.                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Secao                                                  ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class SecaoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SecaoController (Construtor)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa UnitOfWork e serviço de log.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public SecaoController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SecaoController.cs", "SecaoController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListaSecoes (GET)                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista seções patrimoniais com o nome do setor.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de seções.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListaSecoes")]
        public IActionResult ListaSecoes()
        {
            try
            {
                // [DADOS] Consulta seções e setores.
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

                // [RETORNO] Lista de seções.
                return Json(new
                {
                    success = true ,
                    data = secoes
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SecaoController.cs" , "ListaSecoes" , error);
                _log.Error("Erro ao listar seções patrimoniais", error);
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

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListaSecoesCombo (GET)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna seções ativas filtradas por setor (combo).                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • setorSelecionado (Guid?): ID do setor selecionado.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com opções do combo.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListaSecoesCombo")]
        public IActionResult ListaSecoesCombo(Guid? setorSelecionado)
        {
            try
            {
                // [VALIDACAO] Setor selecionado.
                if (!setorSelecionado.HasValue || setorSelecionado == Guid.Empty)
                {
                    return Json(new
                    {
                        success = true ,
                        data = new List<object>()
                    });
                }

                // [DADOS] Consulta seções ativas do setor.
                var secoes = _unitOfWork
                    .SecaoPatrimonial.GetAll()
                    .Where(s => s.SetorId == setorSelecionado && s.Status == true)
                    .OrderBy(s => s.NomeSecao)
                    .Select(s => new { text = s.NomeSecao , value = s.SecaoId })
                    .ToList();

                // [RETORNO] Lista para combo.
                return Json(new
                {
                    success = true ,
                    data = secoes
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SecaoController.cs" , "ListaSecoes" , error);
                _log.Error($", errorErro ao carregar combo de seções para o setor [ID: {setorSelecionado}]");
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
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusSecao (POST)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna status (ativo/inativo) da seção patrimonial.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID da seção.                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusSecao")]
        public JsonResult UpdateStatusSecao(Guid Id)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (Id != Guid.Empty)
                {
                    // [DADOS] Busca seção.
                    var objFromDb = _unitOfWork.SecaoPatrimonial.GetFirstOrDefault(u =>
                        u.SecaoId == Id
                    );
                    string Description = "";
                    int type = 0;
                    if (objFromDb != null)
                    {
                        // [REGRA] Alterna status.
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
                        _log.Info(Description);
                    }
                    // [RETORNO] Resultado da operação.
                    return Json(
                        new
                        {
                            success = true ,
                            message = Description ,
                            type = type ,
                        }
                    );
                }
                // [RETORNO] ID inválido.
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
                _log.Error($", errorErro ao alternar status da seção [ID: {Id}]");
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

    }
}
