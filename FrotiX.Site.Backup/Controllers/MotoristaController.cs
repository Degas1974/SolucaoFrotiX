/*
 *  _______________________________________________________
 * |                                                       |
 * |   FrotiX Core - Gestão de Motoristas (Core Stack)      |
 * |_______________________________________________________|
 *
 * (IA) Controlador responsável pela gestão de condutores,
 * incluindo dados cadastrais, CNH e vínculos contratuais.
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
    /// ║ 📌 NOME: MotoristaController                                                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    API para gerenciamento de Motoristas.                                     ║
    /// ║    Controla dados, fotos, contratos e status dos condutores.                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/Motorista                                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class MotoristaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MotoristaController (Construtor)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com UnitOfWork e Log centralizado.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public MotoristaController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MotoristaController.cs", "MotoristaController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista geral de motoristas (dados para grid).                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista projetada.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Consulta view de motoristas.
                var result = (
                    from vm in _unitOfWork.ViewMotoristas.GetAll()
                    select new
                    {
                        vm.MotoristaId ,
                        vm.Nome ,
                        vm.Ponto ,
                        vm.CNH ,
                        vm.Celular01 ,
                        vm.CategoriaCNH ,
                        Sigla = vm.Sigla != null ? vm.Sigla : "" ,
                        ContratoMotorista = vm.AnoContrato != null
                            ? (vm.AnoContrato + "/" + vm.NumeroContrato + " - " + vm.DescricaoFornecedor)
                            : vm.TipoCondutor != null ? vm.TipoCondutor
                            : "(sem contrato)" ,
                        vm.Status ,
                        DatadeAlteracao = vm.DataAlteracao?.ToString("dd/MM/yy") ?? string.Empty ,
                        vm.NomeCompleto ,
                        vm.EfetivoFerista ,
                        vm.Foto ,
                    }
                ).ToList();

                // [RETORNO] Lista projetada para grid.
                return Json(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "MotoristaController", "Get");
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "Get" , error);
                return Json(new { success = false, message = "Erro ao listar motoristas" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove motorista se não houver vínculos ativos.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (MotoristaViewModel): Dados com ID.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(MotoristaViewModel model)
        {
            try
            {
                // [VALIDACAO] Verifica payload e ID.
                if (model != null && model.MotoristaId != Guid.Empty)
                {
                    // [DADOS] Carrega motorista.
                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == model.MotoristaId);
                    if (objFromDb != null)
                    {
                        // [REGRA] Verifica vínculo com contratos.
                        var motoristaContrato = _unitOfWork.MotoristaContrato.GetFirstOrDefault(u => u.MotoristaId == model.MotoristaId);
                        if (motoristaContrato != null)
                        {
                            // [RETORNO] Bloqueia exclusão por vínculo.
                            return Json(new { success = false , message = "Não foi possível remover o motorista. Ele está associado a um ou mais contratos!" });
                        }

                        // [ACAO] Remove e persiste.
                        _unitOfWork.Motorista.Remove(objFromDb);
                        _unitOfWork.Save();

                        // [LOG] Registro de remoção.
                        _log.Warning($"Motorista removido. ID: {model.MotoristaId}, Nome: {objFromDb.Nome}", "MotoristaController", "Delete");

                        return Json(new { success = true , message = "Motorista removido com sucesso" });
                    }
                }
                // [RETORNO] Falha de validação.
                return Json(new { success = false , message = "Erro ao apagar motorista" });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "MotoristaController", "Delete");
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "Delete" , error);
                return Json(new { success = false, message = "Erro interno ao apagar motorista" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusMotorista                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alterna status Ativo/Inativo do motorista.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do motorista.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusMotorista")]
        public JsonResult UpdateStatusMotorista(Guid Id)
        {
            try
            {
                // [VALIDACAO] Confirma ID válido.
                if (Id != Guid.Empty)
                {
                    // [DADOS] Carrega motorista.
                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == Id);
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        // [STATUS] Alterna status e define mensagem.
                        objFromDb.Status = !objFromDb.Status;
                        Description = string.Format("Atualizado Status do Motorista [Nome: {0}] ({1})", objFromDb.Nome, objFromDb.Status ? "Ativo" : "Inativo");
                        type = objFromDb.Status ? 0 : 1;

                        // [ACAO] Atualiza e persiste.
                        _unitOfWork.Motorista.Update(objFromDb);
                        _unitOfWork.Save();

                        // [LOG] Registro de alteração de status.
                        _log.Info($"Status do motorista alterado. ID: {Id}, Novo Status: {objFromDb.Status}", "MotoristaController", "UpdateStatusMotorista");
                    }
                    // [RETORNO] Status atualizado.
                    return Json(new { success = true , message = Description , type = type });
                }
                // [RETORNO] Falha de validação.
                return Json(new { success = false });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "MotoristaController", "UpdateStatusMotorista");
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "UpdateStatusMotorista" , error);
                return Json(new { success = false });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaFoto (GET)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna objeto do motorista com foto em Base64.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID do motorista.                                             ║
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
                // [VALIDACAO] Confirma ID válido.
                if (id != Guid.Empty)
                {
                    // [DADOS] Busca motorista por ID.
                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == id);
                    if (objFromDb?.Foto != null)
                    {
                        // [CONVERSAO] Converte foto para Base64.
                        objFromDb.Foto = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                        return Json(objFromDb);
                    }
                }
                // [RETORNO] Foto não encontrada.
                return Json(false);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "MotoristaController", "PegaFoto");
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "PegaFoto" , error);
                return Json(false);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaFotoModal (GET)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna apenas a foto em Base64 para exibição em modal.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID do motorista.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Base64 da foto.                                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("PegaFotoModal")]
        public JsonResult PegaFotoModal(Guid id)
        {
            try
            {
                // [DADOS] Busca motorista por ID.
                var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == id);
                if (objFromDb?.Foto != null)
                {
                    // [CONVERSAO] Converte foto para Base64.
                    var fotoBase64 = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
                    return Json(fotoBase64);
                }
                // [RETORNO] Foto inexistente.
                return Json(false);
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "MotoristaController", "PegaFotoModal");
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "PegaFotoModal" , error);
                return Json(false);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetImage                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Helper: Converte String Base64 para byte[].                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public byte[] GetImage(string sBase64String)
        {
            try
            {
                if (!string.IsNullOrEmpty(sBase64String))
                {
                    // [CONVERSAO] Decodifica Base64 para bytes.
                    return Convert.FromBase64String(sBase64String);
                }
                // [RETORNO] Base64 vazio.
                return null;
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "MotoristaController", "GetImage");
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "GetImage" , error);
                return null;
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MotoristaContratos (GET)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista motoristas vinculados a um contrato específico.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do contrato.                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista projetada.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("MotoristaContratos")]
        public IActionResult MotoristaContratos(Guid Id)
        {
            try
            {
                // [DADOS] Consulta vínculos de motoristas com contrato.
                var result = (
                    from vm in _unitOfWork.ViewMotoristas.GetAll()
                    join mc in _unitOfWork.MotoristaContrato.GetAll()
                        on vm.MotoristaId equals mc.MotoristaId
                    where mc.ContratoId == Id
                    select new
                    {
                        vm.MotoristaId ,
                        vm.Nome ,
                        vm.Ponto ,
                        vm.CNH ,
                        vm.Celular01 ,
                        vm.CategoriaCNH ,
                        Sigla = vm.Sigla != null ? vm.Sigla : "" ,
                        ContratoMotorista = vm.AnoContrato != null
                            ? (vm.AnoContrato + "/" + vm.NumeroContrato + " - " + vm.DescricaoFornecedor)
                            : "<b>(Veículo Próprio)</b>" ,
                        vm.Status ,
                        DatadeAlteracao = vm.DataAlteracao?.ToString("dd/MM/yy") ?? string.Empty ,
                        vm.NomeCompleto ,
                    }
                ).ToList();

                // [RETORNO] Lista projetada para grid.
                return Json(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "MotoristaController", "MotoristaContratos");
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "MotoristaContratos" , error);
                return Json(new { success = false, message = "Erro ao listar contratos do motorista" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DeleteContrato (POST)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove vínculo entre motorista e contrato.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (MotoristaViewModel): Dados com IDs.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da operação.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("DeleteContrato")]
        [HttpPost]
        public IActionResult DeleteContrato(MotoristaViewModel model)
        {
            try
            {
                // [VALIDACAO] Verifica payload e ID.
                if (model != null && model.MotoristaId != Guid.Empty)
                {
                    // [DADOS] Carrega motorista.
                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == model.MotoristaId);
                    if (objFromDb != null)
                    {
                        // [REGRA] Verifica vínculo específico.
                        var motoristaContrato = _unitOfWork.MotoristaContrato.GetFirstOrDefault(u =>
                            u.MotoristaId == model.MotoristaId && u.ContratoId == model.ContratoId
                        );
                        if (motoristaContrato != null)
                        {
                            if (objFromDb.ContratoId == model.ContratoId)
                            {
                                // [ACAO] Remove referência de contrato no motorista.
                                objFromDb.ContratoId = Guid.Empty;
                                _unitOfWork.Motorista.Update(objFromDb);
                            }
                            // [ACAO] Remove vínculo e persiste.
                            _unitOfWork.MotoristaContrato.Remove(motoristaContrato);
                            _unitOfWork.Save();

                            // [LOG] Registro de remoção de vínculo.
                            _log.Warning($"Vínculo de contrato removido para o motorista. ID Motorista: {model.MotoristaId}, ID Contrato: {model.ContratoId}", "MotoristaController", "DeleteContrato");

                            return Json(new { success = true , message = "Motorista removido com sucesso" });
                        }
                    }
                }
                // [RETORNO] Falha de validação.
                return Json(new { success = false , message = "Erro ao remover motorista" });
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "MotoristaController", "DeleteContrato");
                Alerta.TratamentoErroComLinha("MotoristaController.cs" , "DeleteContrato" , error);
                return Json(new { success = false, message = "Erro interno ao remover contrato" });
            }
        }
    }
}
