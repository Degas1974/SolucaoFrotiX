/*
 *  _____________________________________________________________
 * |                                                             |
 * |   FrotiX Core - Gestão de Setores (Core Stack)              |
 * |_____________________________________________________________|
 *
 * (IA) Controlador responsável pela gestão de setores patrimoniais
 * e definição de responsabilidade (detentores) de bens.
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
    /// ║ 📌 NOME: SetorController                                                   ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de setores patrimoniais e responsáveis (detentores).               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Setor                                                  ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class SetorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SetorController (Construtor)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa UnitOfWork e serviço de log.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public SetorController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorController.cs", "SetorController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListaSetores (GET)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista setores patrimoniais com info do detentor.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de setores.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListaSetores")]
        public IActionResult ListaSetores()
        {
            try
            {
                // [DADOS] Consulta setores e detentores.
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

                // [RETORNO] Lista de setores.
                return Json(new
                {
                    success = true ,
                    data = setores
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorController.cs" , "ListaSetores" , error);
                _log.Error("Erro ao listar setores patrimoniais", error);
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

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusSetor (POST)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna status (ativo/inativo) do setor patrimonial.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do setor.                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusSetor")]
        public JsonResult UpdateStatusSetor(Guid Id)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (Id != Guid.Empty)
                {
                    // [DADOS] Busca setor.
                    var objFromDb = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(u =>
                        u.SetorId == Id
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
                    "SetorController.cs" , // Corrigido nome do arquivo que estava SetorPatrimonialController
                    "UpdateStatusSetor" ,
                    error
                );
                _log.Error($", errorErro ao alternar status do setor [ID: {Id}]");
                return new JsonResult(new
                {
                    sucesso = false
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove setor se não houver seções vinculadas.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID do setor.                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete([FromBody] Guid id)
        {
            try
            {
                // [VALIDACAO] ID informado.
                if (id != Guid.Empty)
                {
                    // [DADOS] Busca setor.
                    var objFromDb = _unitOfWork.SetorPatrimonial.GetFirstOrDefault(u =>
                        u.SetorId == id
                    );
                    if (objFromDb != null)
                    {
                        // [REGRA] Verifica dependência de seções.
                        var secao = _unitOfWork.SecaoPatrimonial.GetFirstOrDefault(u =>
                            u.SetorId == id
                        );
                        if (secao != null)
                        {
                            // [RETORNO] Bloqueia exclusão por dependência.
                            _log.Warning($"Tentativa de exclusão de setor com dependências: [Setor: {objFromDb.NomeSetor}] [ID: {id}]");
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Existem seções associadas a esse setor" ,
                                }
                            );
                        }
                        // [ACAO] Remove setor.
                        var nomeSetor = objFromDb.NomeSetor;
                        _unitOfWork.SetorPatrimonial.Remove(objFromDb);
                        _unitOfWork.Save();
                        _log.Info($"Setor patrimonial removido: [ID: {id}] [Nome: {nomeSetor}]");
                        // [RETORNO] Sucesso.
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Setor removido com sucesso"
                            }
                        );
                    }
                }
                // [RETORNO] Falha padrão.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar Setor"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorController.cs" , "Delete" , error);
                _log.Error($", errorErro ao deletar setor patrimonial [ID: {id}]");
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListaSetoresCombo (GET)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna setores ativos para combo/select.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com opções do combo.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("ListaSetoresCombo")]
        public IActionResult ListaSetoresCombo()
        {
            try
            {
                // [DADOS] Consulta setores ativos.
                var setores = _unitOfWork
                    .SetorPatrimonial.GetAll()
                    .Where(s => s.Status == true)
                    .OrderBy(s => s.NomeSetor)
                    .Select(s => new { text = s.NomeSetor , value = s.SetorId })
                    .ToList();

                // [RETORNO] Lista para combo.
                return Json(new
                {
                    success = true ,
                    data = setores
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("SetorController.cs" , "ListaSetoresCombo" , error);
                _log.Error("Erro ao carregar combo de setores", error);
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
