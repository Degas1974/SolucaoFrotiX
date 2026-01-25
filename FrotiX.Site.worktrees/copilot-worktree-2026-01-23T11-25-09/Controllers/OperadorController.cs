/*
 *  _______________________________________________________
 * |                                                       |
 * |   FrotiX Core - Gestão de Operadores (Core Stack)      |
 * |_______________________________________________________|
 *
 * (IA) Controlador responsável pela gestão de operadores de máquinas
 * e equipamentos pesados, incluindo vínculos contratuais.
 */

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: OperadorController                                                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    API para gerenciamento de Operadores.                                     ║
    /// ║    Controla cadastro e vínculos com Contratos.                               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Operador                                             ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class OperadorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OperadorController (Construtor)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com UnitOfWork e Log centralizado.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public OperadorController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("OperadorController.cs", "OperadorController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna lista de operadores com dados contratuais.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista projetada.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Consulta operadores com joins de contrato/fornecedor/usuário.
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

                // [RETORNO] Lista projetada para grid.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception error)
            {
                _log.Error("OperadorController.Get", error);
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "Get" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove um operador se não tiver contratos associados.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (OperadorViewModel): Dados com ID.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(OperadorViewModel model)
        {
            try
            {
                // [VALIDACAO] Verifica payload e ID.
                if (model != null && model.OperadorId != Guid.Empty)
                {
                    // [DADOS] Carrega operador.
                    var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u =>
                        u.OperadorId == model.OperadorId
                    );
                    if (objFromDb != null)
                    {
                        // [REGRA] Verifica vínculos com contratos.
                        var operadorContrato = _unitOfWork.OperadorContrato.GetFirstOrDefault(u =>
                            u.OperadorId == model.OperadorId
                        );
                        if (operadorContrato != null)
                        {
                            // [RETORNO] Bloqueia exclusão por vínculo.
                            return Json(
                                new
                                {
                                    success = false ,
                                    message = "Não foi possível remover o operador. Ele está associado a um ou mais contratos!" ,
                                }
                            );
                        }

                        // [ACAO] Remove e persiste.
                        _unitOfWork.Operador.Remove(objFromDb);
                        _unitOfWork.Save();
                        // [LOG] Registro de exclusão.
                        _log.Info($"OperadorController.Delete: Operador {objFromDb.Nome} ({objFromDb.OperadorId}) removido.");
                        return Json(
                            new
                            {
                                success = true ,
                                message = "Operador removido com sucesso"
                            }
                        );
                    }
                }
                // [RETORNO] Falha de validação.
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar operador"
                });
            }
            catch (Exception error)
            {
                _log.Error("OperadorController.Delete", error);
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "Delete" , error);
                return Json(new
                {
                    success = false ,
                    message = "Erro ao apagar operador"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusOperador                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Ativa ou inativa o operador.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do operador.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusOperador")]
        public JsonResult UpdateStatusOperador(Guid Id)
        {
            try
            {
                // [VALIDACAO] Confirma ID válido.
                if (Id != Guid.Empty)
                {
                    // [DADOS] Carrega operador.
                    var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u => u.OperadorId == Id);
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            // [STATUS] Marca como inativo.
                            objFromDb.Status = false;
                            Description = string.Format(
                                "Atualizado Status do Operador [Nome: {0}] (Inativo)" ,
                                objFromDb.Nome
                            );
                            type = 1;
                        }
                        else
                        {
                            // [STATUS] Marca como ativo.
                            objFromDb.Status = true;
                            Description = string.Format(
                                "Atualizado Status do Operador  [Nome: {0}] (Ativo)" ,
                                objFromDb.Nome
                            );
                            type = 0;
                        }
                        // [ACAO] Atualiza e persiste.
                        _unitOfWork.Operador.Update(objFromDb);
                        _unitOfWork.Save();
                        // [LOG] Registro de alteração.
                        _log.Info($"OperadorController.UpdateStatusOperador: {Description}");
                    }
                    // [RETORNO] Status atualizado.
                    return Json(
                        new
                        {
                            success = true ,
                            message = Description ,
                            type = type ,
                        }
                    );
                }
                // [RETORNO] Falha de validação.
                return Json(new
                {
                    success = false
                });
            }
            catch (Exception error)
            {
                _log.Error("OperadorController.UpdateStatusOperador", error);
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "UpdateStatusOperador" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaFoto (GET)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna a foto do operador convertida de Base64.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID do operador.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Objeto com foto convertida.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("PegaFoto")]
        public JsonResult PegaFoto(Guid id)
        {
            try
            {
            // [VALIDACAO] Verifica ID do operador.
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
                _log.Error("OperadorController.PegaFoto", error);
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "PegaFoto" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Pegar Foto Modal
        /// │ DESCRIÇÃO: Retorna apenas os bytes da foto do operador para exibição em modais.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
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
                _log.Error("OperadorController.PegaFotoModal", error);
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "PegaFotoModal" , error);
                return new JsonResult(new
                {
                    success = false
                });
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Obter Bytes da Imagem
        /// │ DESCRIÇÃO: Converte string Base64 para array de bytes.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
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
                _log.Error("OperadorController.GetImage", error);
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "GetImage" , error);
                return default(byte[]);
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Listar Contratos do Operador
        /// │ DESCRIÇÃO: Retorna os vínculos do operador com um contrato específico.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
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
                _log.Error("OperadorController.OperadorContratos", error);
                Alerta.TratamentoErroComLinha("OperadorController.cs" , "OperadorContratos" , error);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Remover vínculo com Contrato
        /// │ DESCRIÇÃO: Exclui a associação de um operador com um contrato.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
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
                            _log.Info($"OperadorController.DeleteContrato: Vínculo do Operador {objFromDb.Nome} com contrato {model.ContratoId} removido.");
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
                _log.Error("OperadorController.DeleteContrato", error);
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
