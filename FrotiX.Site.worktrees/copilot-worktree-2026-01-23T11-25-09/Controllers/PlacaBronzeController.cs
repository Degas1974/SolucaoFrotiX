/*
*  #################################################################################################
*  #                                                                                               #
*  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
*  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
*  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
*  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
*  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
*  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
*  #                                                                                               #
*  #   PROJETO: FROTIX - GESTÃO DE FROTAS                                                          #
*  #   MODULO:  ADMINISTRAÇÃO (PLACAS BRONZE)                                                      #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #                                                                                               #
*  #################################################################################################
*/

using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using FrotiX.Helpers;
using FrotiX.Services;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: PlacaBronzeController                                              ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de placas metálicas oficiais (Bronze) e vínculo com veículos.      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/PlacaBronze                                           ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class PlacaBronzeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PlacaBronzeController (Construtor)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador de placas de bronze.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public PlacaBronzeController(IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs", "Constructor", ex);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna placas de bronze e veículos associados.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista projetada.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [DADOS] Consulta placas e vínculo com veículos.
                var result = (
                    from p in _unitOfWork.PlacaBronze.GetAll()
                    join v in _unitOfWork.Veiculo.GetAll()
                        on p.PlacaBronzeId equals v.PlacaBronzeId
                        into pb
                    from pbResult in pb.DefaultIfEmpty()
                    select new
                    {
                        p.PlacaBronzeId,
                        p.DescricaoPlaca,
                        p.Status,
                        PlacaVeiculo = pbResult != null ? pbResult.Placa : "",
                    }
                ).ToList();

                // [RETORNO] Lista projetada para grid.
                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                _log.Error("PlacaBronzeController.Get", ex);
                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs", "Get", ex);
                return Json(new { success = false, message = "Erro ao carregar dados" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Delete (POST)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove uma placa se não houver veículos vinculados.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (PlacaBronzeViewModel): Dados com ID.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(PlacaBronzeViewModel model)
        {
            try
            {
                // [VALIDACAO] Verifica payload e ID.
                if (model != null && model.PlacaBronzeId != Guid.Empty)
                {
                    // [DADOS] Carrega placa.
                    var objFromDb = _unitOfWork.PlacaBronze.GetFirstOrDefault(u =>
                        u.PlacaBronzeId == model.PlacaBronzeId
                    );
                    if (objFromDb != null)
                    {
                        // [REGRA] Verifica vínculo com veículos.
                        var modelo = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                            u.PlacaBronzeId == model.PlacaBronzeId
                        );
                        if (modelo != null)
                        {
                            // [RETORNO] Bloqueia exclusão por vínculo.
                            _log.Warning($"PlacaBronzeController.Delete: Tentativa de remoção de placa com veículo vinculado ({objFromDb.DescricaoPlaca})");
                            return Json(new { success = false, message = "Existem veículos associados a essa placa" });
                        }

                        // [ACAO] Remove placa e persiste.
                        string descricao = objFromDb.DescricaoPlaca;
                        _unitOfWork.PlacaBronze.Remove(objFromDb);
                        _unitOfWork.Save();

                        // [LOG] Registro de exclusão.
                        _log.Info($"PlacaBronzeController.Delete: Placa de Bronze ({descricao}) removida com sucesso.");
                        return Json(new { success = true, message = "Placa de Bronze removida com sucesso" });
                    }
                }
                // [RETORNO] Falha de validação.
                return Json(new { success = false, message = "Erro ao apagar placa de bronze" });
            }
            catch (Exception ex)
            {
                _log.Error("PlacaBronzeController.Delete", ex);
                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs", "Delete", ex);
                return Json(new { success = false, message = "Erro ao deletar placa de bronze" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UpdateStatusPlacaBronze                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Ativa ou inativa o cadastro da placa.                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID da placa.                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Status da operação.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("UpdateStatusPlacaBronze")]
        public JsonResult UpdateStatusPlacaBronze(Guid Id)
        {
            try
            {
                // [VALIDACAO] Confirma ID válido.
                if (Id != Guid.Empty)
                {
                    // [DADOS] Carrega placa.
                    var objFromDb = _unitOfWork.PlacaBronze.GetFirstOrDefault(u =>
                        u.PlacaBronzeId == Id
                    );
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        if (objFromDb.Status == true)
                        {
                            // [STATUS] Marca como inativa.
                            objFromDb.Status = false;
                            Description = string.Format("Atualizado Status da Placa [Nome: {0}] (Inativo)", objFromDb.DescricaoPlaca);
                            type = 1;
                        }
                        else
                        {
                            // [STATUS] Marca como ativa.
                            objFromDb.Status = true;
                            Description = string.Format("Atualizado Status da Placa [Nome: {0}] (Ativo)", objFromDb.DescricaoPlaca);
                            type = 0;
                        }
                        // [ACAO] Atualiza e persiste.
                        _unitOfWork.PlacaBronze.Update(objFromDb);
                        _unitOfWork.Save();

                        // [LOG] Registro de alteração.
                        _log.Info($"PlacaBronzeController.UpdateStatusPlacaBronze: {Description}");
                    }
                    // [RETORNO] Status atualizado.
                    return Json(new { success = true, message = Description, type = type });
                }
                // [RETORNO] Falha de validação.
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                _log.Error("PlacaBronzeController.UpdateStatusPlacaBronze", ex);
                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs", "UpdateStatusPlacaBronze", ex);
                return Json(new { success = false });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Desvincula (POST)                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove a associação da placa de bronze de um veículo.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (PlacaBronzeViewModel): Dados com ID.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da operação.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Desvincula")]
        [HttpPost]
        public IActionResult Desvincula(PlacaBronzeViewModel model)
        {
            try
            {
                // [VALIDACAO] Verifica ID.
                if (model.PlacaBronzeId != Guid.Empty)
                {
                    // [DADOS] Busca veículo vinculado à placa.
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                        u.PlacaBronzeId == model.PlacaBronzeId
                    );
                    string Description = "";
                    int type = 0;

                    if (objFromDb != null)
                    {
                        // [ACAO] Remove vínculo da placa.
                        objFromDb.PlacaBronzeId = Guid.Empty;
                        Description = string.Format("Placa de Bronze desassociada com sucesso do veículo {0}!", objFromDb.Placa);
                        type = 1;
                        _unitOfWork.Veiculo.Update(objFromDb);
                        _unitOfWork.Save();

                        // [LOG] Registro de desvínculo.
                        _log.Info($"PlacaBronzeController.Desvincula: {Description}");
                    }
                    // [RETORNO] Resultado da operação.
                    return Json(new { success = true, message = Description, type = type });
                }
                // [RETORNO] Falha de validação.
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                _log.Error("PlacaBronzeController.Desvincula", ex);
                Alerta.TratamentoErroComLinha("PlacaBronzeController.cs", "Desvincula", ex);
                return Json(new { success = false, message = "Erro ao desvincular placa" });
            }
        }
    }
}
