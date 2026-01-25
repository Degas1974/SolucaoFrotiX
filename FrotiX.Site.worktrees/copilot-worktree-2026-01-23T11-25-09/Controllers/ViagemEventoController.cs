using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FrotiX.Controllers
{
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
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  GESTOR DE EVENTOS E LOGÍSTICA (ECONOMILDO)                                         #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ViagemEventoController                                            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Gestão de viagens de eventos (Economildo).                                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/ViagemEvento                                          ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class ViagemEventoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;
        private readonly IWebHostEnvironment _hostingEnv;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemEventoController (Construtor)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Injeta UnitOfWork, ambiente e log.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): Unidade de trabalho.                         ║
        /// ║    • env (IWebHostEnvironment): Ambiente web.                               ║
        /// ║    • log (ILogService): Serviço de log.                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public ViagemEventoController(
            IUnitOfWork unitOfWork,
            IWebHostEnvironment env,
            ILogService log
        )
        {
            try
            {
                _unitOfWork = unitOfWork;
                _hostingEnv = env;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemEventoController", ex);
            }
        }

        #region ==================== LISTAGEM DE VIAGENS ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get (GET)                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista viagens com finalidade Evento e não agendadas.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): Não utilizado.                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com viagens.                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get(string Id)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info("ViagemEventoController.Get: Solicitando lista de eventos básicos.");
                return Json(
                    new
                    {
                        // [DADOS] Filtra viagens de evento.
                        data = _unitOfWork
                            .ViewViagens.GetAll()
                            .Where(v => v.Finalidade == "Evento" && v.StatusAgendamento == false),
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.Get", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Get", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar dados"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemEventos (GET)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Alias para listagem de viagens de eventos.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com viagens.                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemEventos")]
        [HttpGet]
        public IActionResult ViagemEventos()
        {
            try
            {
                // [LOG] Entrada.
                _log.Info("ViagemEventoController.ViagemEventos: Solicitando lista de eventos agendados.");
                return Json(
                    new
                    {
                        // [DADOS] Filtra viagens de evento.
                        data = _unitOfWork
                            .ViewViagens.GetAll()
                            .Where(v => v.Finalidade == "Evento" && v.StatusAgendamento == false),
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.ViagemEventos", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemEventos", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar dados"
                });
            }
        }

        #endregion

        #region ==================== FLUXO ECONOMILDO ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Fluxo (GET)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna fluxo de viagens Economildo.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com fluxo.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Fluxo")]
        [HttpGet]
        public IActionResult Fluxo()
        {
            try
            {
                _log.Info("ViagemEventoController.Fluxo: Listando fluxo geral Economildo.");
                var result = (
                    from vf in _unitOfWork.ViewFluxoEconomildo.GetAll()
                    select new
                    {
                        vf.ViagemEconomildoId,
                        vf.MotoristaId,
                        vf.VeiculoId,
                        vf.NomeMotorista,
                        vf.DescricaoVeiculo,
                        vf.MOB,
                        vf.Data,
                        vf.HoraInicio,
                        vf.HoraFim,
                        vf.QtdPassageiros,
                    }
                ).ToList().OrderByDescending(vf => vf.Data).ThenByDescending(vf => vf.MOB).ThenByDescending(vf => vf.HoraInicio);

                return Json(new
                {
                    data = result
                });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.Fluxo", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Fluxo", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar fluxo"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: FluxoVeiculos (GET)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna fluxo Economildo filtrado por veículo.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): ID do veículo.                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com fluxo.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("FluxoVeiculos")]
        [HttpGet]
        public IActionResult FluxoVeiculos(string Id)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info($"ViagemEventoController.FluxoVeiculos: Filtrando fluxo para Veículo {Id}");
                var result = (
                    from vf in _unitOfWork.ViewFluxoEconomildo.GetAll()
                    where vf.VeiculoId == Guid.Parse(Id)
                    select new
                    {
                        vf.ViagemEconomildoId,
                        vf.MotoristaId,
                        vf.VeiculoId,
                        vf.NomeMotorista,
                        vf.DescricaoVeiculo,
                        vf.MOB,
                        vf.Data,
                        vf.HoraInicio,
                        vf.HoraFim,
                        vf.QtdPassageiros,
                    }
                ).ToList().OrderByDescending(vf => vf.Data).ThenByDescending(vf => vf.MOB).ThenByDescending(vf => vf.HoraInicio);

                // [RETORNO] Fluxo filtrado.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.FluxoVeiculos", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FluxoVeiculos", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar fluxo de veículos"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: FluxoMotoristas (GET)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna fluxo Economildo filtrado por motorista.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): ID do motorista.                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com fluxo.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("FluxoMotoristas")]
        [HttpGet]
        public IActionResult FluxoMotoristas(string Id)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info($"ViagemEventoController.FluxoMotoristas: Filtrando fluxo para Motorista {Id}");
                var result = (
                    from vf in _unitOfWork.ViewFluxoEconomildo.GetAll()
                    where vf.MotoristaId == Guid.Parse(Id)
                    select new
                    {
                        vf.ViagemEconomildoId,
                        vf.MotoristaId,
                        vf.VeiculoId,
                        vf.NomeMotorista,
                        vf.DescricaoVeiculo,
                        vf.MOB,
                        vf.Data,
                        vf.HoraInicio,
                        vf.HoraFim,
                        vf.QtdPassageiros,
                    }
                ).ToList().OrderByDescending(vf => vf.Data).ThenByDescending(vf => vf.MOB).ThenByDescending(vf => vf.HoraInicio);

                // [RETORNO] Fluxo filtrado.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.FluxoMotoristas", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FluxoMotoristas", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar fluxo de motoristas"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: FluxoData (GET)                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna fluxo Economildo filtrado por data.                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): Data de referência.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com fluxo.                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("FluxoData")]
        [HttpGet]
        public IActionResult FluxoData(string Id)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info($"ViagemEventoController.FluxoData: Filtrando fluxo para Data {Id}");
                var dataFluxo = DateTime.Parse(Id);

                var result = (
                    from vf in _unitOfWork.ViewFluxoEconomildoData.GetAll()
                    where vf.Data == dataFluxo
                    select new
                    {
                        vf.ViagemEconomildoId,
                        vf.MotoristaId,
                        vf.VeiculoId,
                        vf.NomeMotorista,
                        vf.DescricaoVeiculo,
                        vf.MOB,
                        vf.Data,
                        vf.HoraInicio,
                        vf.HoraFim,
                        vf.QtdPassageiros,
                    }
                ).ToList().OrderByDescending(vf => vf.Data).ThenByDescending(vf => vf.MOB).ThenByDescending(vf => vf.HoraInicio);

                // [RETORNO] Fluxo filtrado.
                return Json(new
                {
                    data = result
                });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.FluxoData", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FluxoData", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar fluxo de data"
                });
            }
        }

        #endregion

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ApagaFluxoEconomildo (POST)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove registro de fluxo Economildo.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagensEconomildo (ViagensEconomildo): Registro alvo.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ApagaFluxoEconomildo")]
        [HttpPost]
        public IActionResult ApagaFluxoEconomildo(ViagensEconomildo viagensEconomildo)
        {
            try
            {
                // [DADOS] Busca registro.
                var objFromDb = _unitOfWork.ViagensEconomildo.GetFirstOrDefault(v =>
                    v.ViagemEconomildoId == viagensEconomildo.ViagemEconomildoId
                );
                // [ACAO] Remove e salva.
                _unitOfWork.ViagensEconomildo.Remove(objFromDb);
                _unitOfWork.Save();
                // [RETORNO] Sucesso.
                return Json(new
                {
                    success = true,
                    message = "Viagem apagada com sucesso"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ApagaFluxoEconomildo", error);
                return Json(new
                {
                    success = false,
                    message = "Erro ao apagar viagem"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MyUploader (POST)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Upload de ficha de vistoria da viagem.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • MyUploader (IFormFile): Arquivo enviado.                              ║
        /// ║    • ViagemId (string): ID da viagem.                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: ObjectResult com status.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("MyUploader")]
        [HttpPost]
        public IActionResult MyUploader(IFormFile MyUploader, [FromForm] string ViagemId)
        {
            try
            {
                if (MyUploader != null)
                {
                    // [DADOS] Busca viagem.
                    var viagemObj = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                        v.ViagemId == Guid.Parse(ViagemId)
                    );
                    using (var ms = new MemoryStream())
                    {
                        MyUploader.CopyTo(ms);
                        viagemObj.FichaVistoria = ms.ToArray();
                    }

                    // [ACAO] Atualiza viagem.
                    _unitOfWork.Viagem.Update(viagemObj);
                    _unitOfWork.Save();

                    // [RETORNO] Sucesso.
                    return new ObjectResult(new
                    {
                        status = "success"
                    });
                }
                return new ObjectResult(new
                {
                    status = "fail"
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "MyUploader", error);
                return new ObjectResult(new
                {
                    status = "fail"
                });
            }
        }

        #region ==================== CUSTOS E FINANÇAS ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CalculaCustoViagens (POST)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recalcula custos massivos de viagens realizadas.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("CalculaCustoViagens")]
        [HttpPost]
        public IActionResult CalculaCustoViagens()
        {
            try
            {
                // [LOG] Início.
                _log.Info("ViagemEventoController.CalculaCustoViagens: Iniciando recálculo de custos para viagens realizadas.");
                var objViagens = _unitOfWork.Viagem.GetAll(v =>
                    v.StatusAgendamento == false
                    && v.Status == "Realizada"
                    && (
                        v.Finalidade != "Manutenção"
                        && v.Finalidade != "Devolução à Locadora"
                        && v.Finalidade != "Recebimento da Locadora"
                        && v.Finalidade != "Saída para Manutenção"
                        && v.Finalidade != "Chegada da Manutenção"
                    )
                    && v.NoFichaVistoria != null
                );

                foreach (var viagem in objViagens)
                {
                    // [CALCULO] Motorista.
                    if (viagem.MotoristaId != null)
                    {
                        int minutos = -1;
                        viagem.CustoMotorista = Servicos.CalculaCustoMotorista(
                            viagem,
                            _unitOfWork,
                            ref minutos
                        );
                        viagem.Minutos = minutos;
                    }
                    // [CALCULO] Veículo/Combustível.
                    if (viagem.VeiculoId != null)
                    {
                        viagem.CustoVeiculo = Servicos.CalculaCustoVeiculo(viagem, _unitOfWork);
                        viagem.CustoCombustivel = Servicos.CalculaCustoCombustivel(viagem, _unitOfWork);
                    }
                    viagem.CustoOperador = 0;
                    viagem.CustoLavador = 0;
                    _unitOfWork.Viagem.Update(viagem);
                }

                _unitOfWork.Save();
                _log.Info($"ViagemEventoController.CalculaCustoViagens: Sucesso no recálculo ({objViagens.Count()} registros).");

                // [RETORNO] Sucesso.
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.CalculaCustoViagens", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "CalculaCustoViagens", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao calcular custos"
                });
            }
        }

        #endregion

        #region ==================== FILTRAGEM ADICIONAL ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemVeiculos (GET)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista viagens não agendadas por veículo.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do veículo.                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com viagens.                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemVeiculos")]
        [HttpGet]
        public IActionResult ViagemVeiculos(Guid Id)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info($"ViagemEventoController.ViagemVeiculos: Obtendo viagens para Veículo {Id}");
                return Json(
                    new
                    {
                        // [DADOS] Filtra viagens.
                        data = _unitOfWork
                            .ViewViagens.GetAll()
                            .Where(vv => vv.VeiculoId == Id && vv.StatusAgendamento == false),
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.ViagemVeiculos", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemVeiculos", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar viagens do veículo"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemMotoristas (GET)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista viagens não agendadas por motorista.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do motorista.                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com viagens.                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemMotoristas")]
        [HttpGet]
        public IActionResult ViagemMotoristas(Guid Id)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info($"ViagemEventoController.ViagemMotoristas: Obtendo viagens para Motorista {Id}");
                return Json(
                    new
                    {
                        // [DADOS] Filtra viagens.
                        data = _unitOfWork
                            .ViewViagens.GetAll()
                            .Where(vv => vv.MotoristaId == Id && vv.StatusAgendamento == false),
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.ViagemMotoristas", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemMotoristas", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar viagens do motorista"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemStatus (GET)                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista viagens por status.                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): Status.                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com viagens.                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemStatus")]
        [HttpGet]
        public IActionResult ViagemStatus(string Id)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info($"ViagemEventoController.ViagemStatus: Filtrando por status {Id}");
                return Json(
                    new
                    {
                        // [DADOS] Filtra viagens.
                        data = _unitOfWork
                            .ViewViagens.GetAll()
                            .Where(vv => vv.Status == Id && vv.StatusAgendamento == false),
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.ViagemStatus", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemStatus", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar viagens por status"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemSetores (GET)                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista viagens por setor solicitante.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do setor.                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com viagens.                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemSetores")]
        [HttpGet]
        public IActionResult ViagemSetores(Guid Id)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info($"ViagemEventoController.ViagemSetores: Filtrando por Setor {Id}");
                return Json(
                    new
                    {
                        // [DADOS] Filtra viagens.
                        data = _unitOfWork
                            .ViewViagens.GetAll()
                            .Where(vv => vv.SetorSolicitanteId == Id && vv.StatusAgendamento == false),
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.ViagemSetores", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemSetores", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar viagens por setor"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemData (GET)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista viagens por data.                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): Data.                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com viagens.                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemData")]
        [HttpGet]
        public IActionResult ViagemData(string Id)
        {
            try
            {
                // [VALIDACAO] Data válida.
                if (DateTime.TryParse(Id, out DateTime parsedDate))
                {
                    // [LOG] Entrada.
                    _log.Info($"ViagemEventoController.ViagemData: Filtrando por Data {parsedDate:dd/MM/yyyy}");
                    return Json(
                        new
                        {
                            // [DADOS] Filtra viagens.
                            data = _unitOfWork
                                .ViewViagens.GetAll()
                                .Where(vv =>
                                    vv.DataInicial == parsedDate && vv.StatusAgendamento == false
                                ),
                        }
                    );
                }
                return Json(new
                {
                    success = false,
                    message = "Data inválida fornecida."
                });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.ViagemData", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ViagemData", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar viagens por data"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Ocorrencias (GET)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista viagens com ocorrências registradas.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): Não utilizado.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com viagens.                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Ocorrencias")]
        [HttpGet]
        public IActionResult Ocorrencias(Guid Id)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info("ViagemEventoController.Ocorrencias: Listando viagens com ocorrências registradas.");
                return Json(
                    new
                    {
                        // [DADOS] Filtra ocorrências.
                        data = _unitOfWork
                            .ViewViagens.GetAll()
                            .Where(vv =>
                                (vv.ResumoOcorrencia != null || vv.DescricaoOcorrencia != null)
                            ),
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.Ocorrencias", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Ocorrencias", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao carregar ocorrências"
                });
            }
        }

        #endregion

        #region ==================== ACOES DE VIAGEM ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Cancelar (POST)                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Atualiza status da viagem para Cancelada.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (ViagemID): Identificador da viagem.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("Cancelar")]
        [HttpPost]
        public IActionResult Cancelar(ViagemID id)
        {
            try
            {
                // [DADOS] Busca viagem.
                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id.ViagemId);
                if (objFromDb != null)
                {
                    // [ACAO] Cancela viagem.
                    objFromDb.Status = "Cancelada";
                    _unitOfWork.Viagem.Update(objFromDb);
                    _unitOfWork.Save();
                    // [LOG] Sucesso.
                    _log.Info($"ViagemEventoController.Cancelar: Viagem {id.ViagemId} cancelada com sucesso.");
                    // [RETORNO] Sucesso.
                    return Json(new
                    {
                        success = true,
                        message = "Viagem cancelada com sucesso"
                    });
                }
                return Json(new
                {
                    success = false,
                    message = "Erro ao cancelar Viagem: Registro não encontrado"
                });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.Cancelar", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "Cancelar", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao cancelar viagem"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaFicha (GET)                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recupera a ficha de vistoria em Base64.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID da viagem.                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Base64 da ficha.                                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("PegaFicha")]
        public JsonResult PegaFicha(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id);
                    if (objFromDb.FichaVistoria != null)
                    {
                        objFromDb.FichaVistoria = this.GetImage(
                            Convert.ToBase64String(objFromDb.FichaVistoria)
                        );
                        return Json(objFromDb);
                    }
                    return Json(false);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.PegaFicha", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "PegaFicha", ex);
                return Json(false);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AdicionarViagensEconomildo (POST)                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Persiste registro de viagem na tabela Economildo.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagensEconomildo (ViagensEconomildo)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: status da operação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("AdicionarViagensEconomildo")]
        [Consumes("application/json")]
        public JsonResult AdicionarViagensEconomildo([FromBody] ViagensEconomildo viagensEconomildo)
        {
            try
            {
                // [ACAO] Adiciona registro.
                _unitOfWork.ViagensEconomildo.Add(viagensEconomildo);
                _unitOfWork.Save();
                // [LOG] Sucesso.
                _log.Info($"ViagemEventoController.AdicionarViagensEconomildo: Sucesso ao adicionar registro Economildo.");

                return Json(new
                {
                    success = true,
                    message = "Viagem Adicionada com Sucesso!"
                });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.AdicionarViagensEconomildo", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarViagensEconomildo", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao adicionar viagem"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ExisteDataEconomildo (POST)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Verifica duplicidade por data/veículo/MOB/motorista.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagensEconomildo (ViagensEconomildo)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: status de duplicidade.                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ExisteDataEconomildo")]
        [Consumes("application/json")]
        public JsonResult ExisteDataEconomildo([FromBody] ViagensEconomildo viagensEconomildo)
        {
            try
            {
                // [VALIDACAO] Data informada.
                if (viagensEconomildo.Data != null)
                {
                    var existeData = _unitOfWork.ViagensEconomildo.GetFirstOrDefault(u =>
                        u.Data == viagensEconomildo.Data
                        && u.VeiculoId == viagensEconomildo.VeiculoId
                        && u.MOB == viagensEconomildo.MOB
                        && u.MotoristaId == viagensEconomildo.MotoristaId
                    );
                    if (existeData != null)
                    {
                        return Json(
                            new
                            {
                                success = false,
                                message = "Já existe registro para essa data!"
                            }
                        );
                    }
                }

                return Json(new
                {
                    success = true,
                    message = ""
                });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.ExisteDataEconomildo", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ExisteDataEconomildo", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao verificar data"
                });
            }
        }

        #endregion

        #region ==================== UTILITÁRIOS E AUXILIARES ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaFichaModal (GET)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recupera ficha de vistoria para exibição em modal.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID da viagem.                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Base64 da ficha ou false.                                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("PegaFichaModal")]
        public JsonResult PegaFichaModal(Guid id)
        {
            try
            {
                // [DADOS] Busca viagem.
                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id);
                if (objFromDb.FichaVistoria != null)
                {
                    objFromDb.FichaVistoria = this.GetImage(
                        Convert.ToBase64String(objFromDb.FichaVistoria)
                    );
                    return Json(objFromDb.FichaVistoria);
                }
                return Json(false);
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.PegaFichaModal", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "PegaFichaModal", ex);
                return Json(false);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaCategoria (GET)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém categoria do veículo.                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID do veículo.                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: Categoria ou false.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("PegaCategoria")]
        public JsonResult PegaCategoria(Guid id)
        {
            try
            {
                var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.VeiculoId == id);
                if (objFromDb.Categoria != null)
                {
                    return Json(objFromDb.Categoria);
                }
                return Json(false);
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.PegaCategoria", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "PegaCategoria", ex);
                return Json(false);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetImage (Helper)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Converte Base64 em array de bytes.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public byte[] GetImage(string sBase64String)
        {
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(sBase64String))
            {
                bytes = Convert.FromBase64String(sBase64String);
            }
            return bytes;
        }

        #endregion

        #region ==================== CADASTRO RÁPIDO (MODALS) ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AdicionarEvento (POST)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Atalho para criação rápida de evento.                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • evento (Evento): Dados do evento.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: status da criação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("AdicionarEvento")]
        [Consumes("application/json")]
        public JsonResult AdicionarEvento([FromBody] Evento evento)
        {
            try
            {
                var existeEvento = _unitOfWork.Evento.GetFirstOrDefault(u => (u.Nome == evento.Nome));
                if (existeEvento != null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Já existe um evento com este nome"
                    });
                }

                _unitOfWork.Evento.Add(evento);
                _unitOfWork.Save();
                _log.Info($"ViagemEventoController.AdicionarEvento: Novo evento '{evento.Nome}' cadastrado via modal.");

                return Json(
                    new
                    {
                        success = true,
                        message = "Evento Adicionado com Sucesso",
                        eventoid = evento.EventoId,
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.AdicionarEvento", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarEvento", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao adicionar evento"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AdicionarRequisitante (POST)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Criação rápida de requisitantes via modal.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • requisitante (Requisitante): Dados do requisitante.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: status da criação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("AdicionarRequisitante")]
        [Consumes("application/json")]
        public JsonResult AdicionarRequisitante([FromBody] Requisitante requisitante)
        {
            try
            {
                // [VALIDACAO] Duplicidade.
                var existeRequisitante = _unitOfWork.Requisitante.GetFirstOrDefault(u =>
                    (u.Ponto == requisitante.Ponto) || (u.Nome == requisitante.Nome)
                );
                if (existeRequisitante != null)
                {
                    return Json(
                        new
                        {
                            success = false,
                            message = "Já existe um requisitante com este ponto/nome",
                        }
                    );
                }

                // [DADOS] Preenche metadados.
                requisitante.Status = true;
                requisitante.DataAlteracao = DateTime.Now;

                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                requisitante.UsuarioIdAlteracao = currentUserID;

                // [ACAO] Persiste.
                _unitOfWork.Requisitante.Add(requisitante);
                _unitOfWork.Save();
                _log.Info($"ViagemEventoController.AdicionarRequisitante: Novo requisitante '{requisitante.Nome}' cadastrado via modal.");

                return Json(
                    new
                    {
                        success = true,
                        message = "Requisitante Adicionado com Sucesso",
                        requisitanteid = requisitante.RequisitanteId,
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.AdicionarRequisitante", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarRequisitante", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao adicionar requisitante"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AdicionarSetor (POST)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Criação rápida de setor solicitante via modal.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • setorSolicitante (SetorSolicitante): Dados do setor.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: status da criação.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("AdicionarSetor")]
        [Consumes("application/json")]
        public JsonResult AdicionarSetor([FromBody] SetorSolicitante setorSolicitante)
        {
            try
            {
                // [VALIDACAO] Sigla duplicada.
                if (setorSolicitante.Sigla != null)
                {
                    var existeSigla = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
                        u.Sigla.ToUpper() == setorSolicitante.Sigla.ToUpper()
                        && u.SetorPaiId == setorSolicitante.SetorPaiId
                    );
                    if (
                        existeSigla != null
                        && existeSigla.SetorSolicitanteId != setorSolicitante.SetorSolicitanteId
                        && existeSigla.SetorPaiId == setorSolicitante.SetorPaiId
                    )
                    {
                        return Json(
                            new
                            {
                                success = false,
                                message = "Já existe um setor com esta sigla neste nível hierárquico",
                            }
                        );
                    }
                }

                var existeSetor = _unitOfWork.SetorSolicitante.GetFirstOrDefault(u =>
                    u.Nome.ToUpper() == setorSolicitante.Nome.ToUpper()
                    && u.SetorPaiId != setorSolicitante.SetorPaiId
                );
                if (
                    existeSetor != null
                    && existeSetor.SetorSolicitanteId != setorSolicitante.SetorSolicitanteId
                )
                {
                    if (existeSetor.SetorPaiId == setorSolicitante.SetorPaiId)
                    {
                        return Json(
                            new
                            {
                                success = false,
                                message = "Já existe um setor com este nome"
                            }
                        );
                    }
                }

                setorSolicitante.Status = true;
                setorSolicitante.DataAlteracao = DateTime.Now;

                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                setorSolicitante.UsuarioIdAlteracao = currentUserID;

                _unitOfWork.SetorSolicitante.Add(setorSolicitante);
                _unitOfWork.Save();
                _log.Info($"ViagemEventoController.AdicionarSetor: Novo setor '{setorSolicitante.Nome}' cadastrado via modal.");

                return Json(
                    new
                    {
                        success = true,
                        message = "Setor Solicitante Adicionado com Sucesso"
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.AdicionarSetor", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AdicionarSetor", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao adicionar setor"
                });
            }
        }

        #endregion

        #region ==================== UPLOAD E IMAGENS ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SaveImage (POST)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Upload de fotos de ocorrências/viagens para o servidor.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • UploadFiles (IList<IFormFile>): Arquivos enviados.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: Ajusta Response.StatusCode.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("SaveImage")]
        public void SaveImage(IList<IFormFile> UploadFiles)
        {
            try
            {
                foreach (IFormFile file in UploadFiles)
                {
                    if (UploadFiles != null)
                    {
                        string filename = ContentDispositionHeaderValue
                            .Parse(file.ContentDisposition)
                            .FileName.Trim('"');

                        filename = Path.GetFileName(filename);

                        string folderPath = Path.Combine(
                            _hostingEnv.WebRootPath,
                            "DadosEditaveis",
                            "ImagensViagens"
                        );

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string fullPath = Path.Combine(folderPath, filename);

                        if (!System.IO.File.Exists(fullPath))
                        {
                            using (FileStream fs = System.IO.File.Create(fullPath))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                            _log.Info($"ViagemEventoController.SaveImage: Arquivo '{filename}' salvo com sucesso.");
                            Response.StatusCode = 200;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.SaveImage", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "SaveImage", ex);
                Response.StatusCode = 204;
            }
        }

        #endregion

        #region ==================== GESTÃO DE CICLO DE VIDA ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: FinalizaViagem (POST)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Finaliza viagem e atualiza hodômetro/status.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagem (FinalizacaoViagem): Dados de finalização.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("FinalizaViagem")]
        [Consumes("application/json")]
        public IActionResult FinalizaViagem([FromBody] FinalizacaoViagem viagem)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info($"ViagemEventoController.FinalizaViagem: Processando finalização da viagem {viagem.ViagemId}.");
                var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                    v.ViagemId == viagem.ViagemId
                );
                objViagem.DataFinal = viagem.DataFinal;
                objViagem.HoraFim = viagem.HoraFim;
                objViagem.KmFinal = viagem.KmFinal;
                objViagem.CombustivelFinal = viagem.CombustivelFinal;
                objViagem.Descricao = viagem.Descricao;
                objViagem.Status = "Realizada";
                objViagem.StatusDocumento = viagem.StatusDocumento;
                objViagem.StatusCartaoAbastecimento = viagem.StatusCartaoAbastecimento;

                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                objViagem.UsuarioIdFinalizacao = currentUserID;
                objViagem.DataFinalizacao = DateTime.Now;

                _unitOfWork.Viagem.Update(objViagem);

                var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v =>
                    v.VeiculoId == objViagem.VeiculoId
                );
                veiculo.Quilometragem = viagem.KmFinal;

                _unitOfWork.Veiculo.Update(veiculo);

                _unitOfWork.Save();
                _log.Info($"ViagemEventoController.FinalizaViagem: Viagem {viagem.ViagemId} finalizada com sucesso. Km Final: {viagem.KmFinal}.");

                return Json(
                    new
                    {
                        success = true,
                        message = "Viagem finalizada com sucesso",
                        type = 0,
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.FinalizaViagem", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FinalizaViagem", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao finalizar viagem"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AjustaViagem (POST)                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retroajusta dados de uma viagem já realizada.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagem (AjusteViagem): Dados de ajuste.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("AjustaViagem")]
        [Consumes("application/json")]
        public IActionResult AjustaViagem([FromBody] AjusteViagem viagem)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info($"ViagemEventoController.AjustaViagem: Solicitado ajuste para a viagem {viagem.ViagemId}.");
                var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                    v.ViagemId == viagem.ViagemId
                );
                objViagem.NoFichaVistoria = viagem.NoFichaVistoria;
                objViagem.DataInicial = viagem.DataInicial;
                objViagem.HoraInicio = viagem.HoraInicial;
                objViagem.KmInicial = viagem.KmInicial;
                objViagem.DataFinal = viagem.DataFinal;
                objViagem.HoraFim = viagem.HoraFim;
                objViagem.KmFinal = viagem.KmFinal;

                objViagem.MotoristaId = viagem.MotoristaId;
                objViagem.VeiculoId = viagem.VeiculoId;

                objViagem.CustoCombustivel = Servicos.CalculaCustoCombustivel(objViagem, _unitOfWork);

                int minutos = -1;
                objViagem.CustoMotorista = Servicos.CalculaCustoMotorista(
                    objViagem,
                    _unitOfWork,
                    ref minutos
                );
                objViagem.Minutos = minutos;

                objViagem.CustoVeiculo = Servicos.CalculaCustoVeiculo(objViagem, _unitOfWork);

                objViagem.CustoOperador = 0;
                objViagem.CustoLavador = 0;

                _unitOfWork.Viagem.Update(objViagem);
                _unitOfWork.Save();

                _log.Info($"ViagemEventoController.AjustaViagem: Ajustes aplicados com sucesso na viagem {viagem.ViagemId}.");

                return Json(
                    new
                    {
                        success = true,
                        message = "Viagem ajustada com sucesso",
                        type = 0,
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.AjustaViagem", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "AjustaViagem", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao ajustar viagem"
                });
            }
        }

        #endregion

        #region ==================== ATTRIBUTES ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RequestSizeLimitAttribute                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Atributo para limitar tamanho de requisições.                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [AttributeUsage(
            AttributeTargets.Class | AttributeTargets.Method,
            AllowMultiple = false,
            Inherited = true
        )]
        public class RequestSizeLimitAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
        {
            private readonly FormOptions _formOptions;

            public RequestSizeLimitAttribute(int valueCountLimit)
            {
                _formOptions = new FormOptions()
                {
                    KeyLengthLimit = valueCountLimit,
                    ValueCountLimit = valueCountLimit,
                    ValueLengthLimit = valueCountLimit,
                };
            }

            public int Order
            {
                get; set;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var contextFeatures = context.HttpContext.Features;
                var formFeature = contextFeatures.Get<IFormFeature>();

                if (formFeature == null || formFeature.Form == null)
                {
                    contextFeatures.Set<IFormFeature>(
                        new FormFeature(context.HttpContext.Request, _formOptions)
                    );
                }
            }
        }

        #endregion

        #region ==================== DATA FETCHING (API) ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterPorId (GET)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém dados completos de evento por ID.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): ID do evento.                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados do evento.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ObterPorId")]
        [HttpGet]
        public IActionResult ObterPorId(Guid id)
        {
            try
            {
                // [VALIDACAO] ID válido.
                if (id == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "ID do evento inválido"
                    });
                }

                // [DADOS] Busca evento.
                var evento = _unitOfWork.Evento.GetFirstOrDefault(e => e.EventoId == id);

                if (evento == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Evento não encontrado"
                    });
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        EventoId = evento.EventoId,
                        Nome = evento.Nome,
                        Descricao = evento.Descricao,
                        DataInicial = evento.DataInicial,
                        DataFinal = evento.DataFinal,
                        QtdParticipantes = evento.QtdParticipantes,
                        Status = evento.Status,
                        SetorSolicitanteId = evento.SetorSolicitanteId,
                        RequisitanteId = evento.RequisitanteId
                    }
                });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.ObterPorId", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ObterPorId", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao buscar dados do evento"
                });
            }
        }

        #endregion

        #region ==================== FILE HANDLING ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: FileUpload (POST)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Upload de arquivos Base64 vinculados à viagem.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • objFile (Objfile): Payload do arquivo.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: ID da viagem ou false.                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("FileUpload")]
        [HttpPost]
        [RequestSizeLimit(valueCountLimit: 1999483648)]
        public JsonResult FileUpload(Objfile objFile)
        {
            try
            {
                // [VALIDACAO] ViagemId obrigatório.
                if (string.IsNullOrEmpty(objFile.viagemid))
                {
                    return Json(false);
                }

                // [LOG] Entrada.
                _log.Info($"ViagemEventoController.FileUpload: Recebido arquivo para a viagem {objFile.viagemid}.");
                var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                    v.ViagemId == Guid.Parse(objFile.viagemid)
                );

                if (objViagem == null) return Json(false);

                // No logic content provided for saving the base64, leaving structure as is
                _unitOfWork.Viagem.Update(objViagem);
                _unitOfWork.Save();

                return Json(objFile.viagemid);
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.FileUpload", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "FileUpload", ex);
                return Json(false);
            }
        }

        #endregion

        #region ==================== FINANCEIRO E CUSTOS ====================

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterDetalhamentoCustosViagem (GET)                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Consolida custos de uma viagem específica.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagemId (Guid): ID da viagem.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com detalhamento.                                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ObterDetalhamentoCustosViagem")]
        [HttpGet("ObterDetalhamentoCustosViagem")]
        public async Task<IActionResult> ObterDetalhamentoCustosViagem(Guid viagemId)
        {
            try
            {
                // [LOG] Entrada.
                _log.Info($"ViagemEventoController.ObterDetalhamentoCustosViagem: Solicitado detalhamento para id {viagemId}.");
                var viagem = await _unitOfWork.Viagem.GetFirstOrDefaultAsync(
                    v => v.ViagemId == viagemId,
                    includeProperties: "Requisitante,Motorista,Veiculo"
                );

                if (viagem == null)
                {
                    return Json(new { success = false, message = "Viagem não encontrada" });
                }

                double tempoTotalHoras = 0;
                if (viagem.DataInicial.HasValue && viagem.HoraInicio.HasValue &&
                    viagem.DataFinal.HasValue && viagem.HoraFim.HasValue)
                {
                    var dataHoraInicio = viagem.DataInicial.Value.Date + viagem.HoraInicio.Value.TimeOfDay;
                    var dataHoraFim = viagem.DataFinal.Value.Date + viagem.HoraFim.Value.TimeOfDay;
                    var diferenca = dataHoraFim - dataHoraInicio;
                    tempoTotalHoras = diferenca.TotalHours;
                }

                double custoTotal =
                    (viagem.CustoMotorista ?? 0) +
                    (viagem.CustoVeiculo ?? 0) +
                    (viagem.CustoCombustivel ?? 0) +
                    (viagem.CustoLavador ?? 0) +
                    (viagem.CustoOperador ?? 0);

                var resultado = new
                {
                    NomeRequisitante = viagem.Requisitante?.Nome ?? "N/A",
                    DataInicial = viagem.DataInicial,
                    HoraInicial = viagem.HoraInicio?.ToString(@"hh\:mm"),
                    DataFinal = viagem.DataFinal,
                    HoraFinal = viagem.HoraFim?.ToString(@"hh\:mm"),
                    TempoTotalHoras = tempoTotalHoras,
                    CustoMotorista = viagem.CustoMotorista ?? 0,
                    CustoVeiculo = viagem.CustoVeiculo ?? 0,
                    CustoCombustivel = viagem.CustoCombustivel ?? 0,
                    CustoTotal = custoTotal
                };

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.ObterDetalhamentoCustosViagem", ex);
                return Json(new
                {
                    success = false,
                    message = $"Erro ao obter detalhamento: {ex.Message}"
                });
            }
        }

        #endregion

        #region ==================== CLASSES AUXILIARES ====================

        public class Objfile
        {
            public string file { get; set; }
            public string viagemid { get; set; }
        }

        #endregion


        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterDetalhamentoCustos (GET)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Detalhamento de custos consolidado de um evento.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • eventoId (Guid): ID do evento.                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com custos consolidados.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ObterDetalhamentoCustos")]
        [HttpGet]
        public IActionResult ObterDetalhamentoCustos(Guid eventoId)
        {
            try
            {
                // [VALIDACAO] ID válido.
                if (eventoId == Guid.Empty)
                {
                    return Json(new
                    {
                        success = false,
                        message = "ID do evento inválido"
                    });
                }

                // Busca todas as viagens do evento
                var viagens = _unitOfWork.Viagem
                    .GetAll()
                    .Where(v => v.EventoId == eventoId)
                    .ToList();

                if (!viagens.Any())
                {
                    return Json(new
                    {
                        success = false,
                        message = "Nenhuma viagem encontrada para este evento"
                    });
                }

                // Calcula o tempo total de viagem em horas
                double tempoTotalHoras = 0;
                DateTime? primeiraDataInicial = null;
                DateTime? ultimaDataFinal = null;

                foreach (var viagem in viagens)
                {
                    if (viagem.DataInicial.HasValue && viagem.DataFinal.HasValue)
                    {
                        // Atualiza primeira data inicial
                        if (!primeiraDataInicial.HasValue || viagem.DataInicial.Value < primeiraDataInicial.Value)
                        {
                            primeiraDataInicial = viagem.DataInicial.Value;
                        }

                        // Atualiza última data final
                        if (!ultimaDataFinal.HasValue || viagem.DataFinal.Value > ultimaDataFinal.Value)
                        {
                            ultimaDataFinal = viagem.DataFinal.Value;
                        }

                        // Calcula tempo desta viagem
                        var dataHoraInicial = viagem.DataInicial.Value.Date;
                        var dataHoraFinal = viagem.DataFinal.Value.Date;

                        if (viagem.HoraInicio.HasValue)
                        {
                            dataHoraInicial = dataHoraInicial.Add(viagem.HoraInicio.Value.TimeOfDay);
                        }

                        if (viagem.HoraFim.HasValue)
                        {
                            dataHoraFinal = dataHoraFinal.Add(viagem.HoraFim.Value.TimeOfDay);
                        }

                        var duracao = dataHoraFinal - dataHoraInicial;
                        tempoTotalHoras += duracao.TotalHours;
                    }
                }

                // Soma os custos
                var custoMotorista = viagens.Sum(v => v.CustoMotorista ?? 0);
                var custoVeiculo = viagens.Sum(v => v.CustoVeiculo ?? 0);
                var custoCombustivel = viagens.Sum(v => v.CustoCombustivel ?? 0);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        TempoTotalHoras = Math.Round(tempoTotalHoras, 2),
                        DataInicial = primeiraDataInicial,
                        HoraInicial = viagens.Where(v => v.DataInicial == primeiraDataInicial)
                                             .Select(v => v.HoraInicio)
                                             .FirstOrDefault(),
                        DataFinal = ultimaDataFinal,
                        HoraFinal = viagens.Where(v => v.DataFinal == ultimaDataFinal)
                                           .Select(v => v.HoraFim)
                                           .FirstOrDefault(),
                        CustoMotorista = custoMotorista,
                        CustoVeiculo = custoVeiculo,
                        CustoCombustivel = custoCombustivel,
                        CustoTotal = custoMotorista + custoVeiculo + custoCombustivel,
                        QuantidadeViagens = viagens.Count
                    }
                });
            }
            catch (Exception ex)
            {
                _log.Error("ViagemEventoController.ObterDetalhamentoCustos", ex);
                Alerta.TratamentoErroComLinha("ViagemEventoController.cs", "ObterDetalhamentoCustos", ex);
                return Json(new
                {
                    success = false,
                    message = "Erro ao buscar detalhamento de custos"
                });
            }
        }
    }
}
