using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Helpers;
using FrotiX.Services;

/*
 *  _________________________________________________________________________________________________________
 * |                                                                                                         |
 * |                                   FROTIX - SOLUÇÃO GESTÃO DE FROTAS                                     |
 * |_________________________________________________________________________________________________________|
 * |                                                                                                         |
 * | (IA) CAMADA: CONTROLLERS (API)                                                                          |
 * | (IA) IDENTIDADE: CustosViagemController.cs                                                              |
 * | (IA) DESCRIÇÃO: API para consulta e recálculo de custos de viagens.                                     |
 * | (IA) PADRÃO: FrotiX 2026 Core (ASCII Hero Banner + XML Documentation)                                   |
 * |_________________________________________________________________________________________________________|
 */

namespace FrotiX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class CustosViagemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostingEnv;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CustosViagemController (Constructor)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador de custos de viagem com UoW, ambiente e log.     ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita consultas e recálculos de custos com rastreabilidade.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): acesso a repositórios.                         ║
        /// ║    • env (IWebHostEnvironment): ambiente/paths.                              ║
        /// ║    • logService (ILogService): log centralizado.                              ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Tipo: N/A                                                               ║
        /// ║    • Significado: N/A                                                        ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Injeção de dependência ao instanciar o controller.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Program.cs                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public CustosViagemController(IUnitOfWork unitOfWork, IWebHostEnvironment env, ILogService logService)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _hostingEnv = env;
                _log = logService;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "CustosViagemController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get                                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista custos de viagem usando view otimizada para grid.                   ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Reduz payload e melhora performance na listagem.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): filtro opcional (não utilizado).                            ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de custos.                                 ║
        /// ║    • Consumidor: UI de Custos de Viagem.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewCustosViagem.GetAllReduced()                             ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/CustosViagem                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Custos de Viagem                                        ║
        /// ║    • Arquivos relacionados: Pages/CustosViagem/*.cshtml                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get(string Id = null)
        {
            try
            {
                // [DADOS] Obtém dados reduzidos da view para performance no grid
                var objCustos = _unitOfWork.ViewCustosViagem.GetAllReduced(selector: v => new
                {
                    v.NoFichaVistoria,
                    v.ViagemId,
                    v.DataInicial,
                    v.DataFinal,
                    v.HoraInicio,
                    v.HoraFim,
                    v.Finalidade,
                    v.Status,
                    v.KmInicial,
                    v.KmFinal,
                    v.Quilometragem,
                    v.CustoMotorista,
                    v.CustoCombustivel,
                    v.CustoVeiculo,
                    v.NomeMotorista,
                    v.MotoristaId,
                    v.VeiculoId,
                    v.StatusAgendamento,
                    v.SetorSolicitanteId,
                    v.DescricaoVeiculo,
                    v.RowNum,
                });

                return Json(new { data = objCustos });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "Get");
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "Get", error);
                return StatusCode(500, new { success = false, message = "Erro ao carregar custos de viagem" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CalculaCustoViagens                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recalcula custos de todas as viagens realizadas no sistema.               ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Atualiza custos para auditoria e relatórios financeiros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON indicando sucesso ou erro.                           ║
        /// ║    • Consumidor: UI/Admin de Custos.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Servicos.CalculaCustoMotorista/Veiculo/Combustivel                       ║
        /// ║    • _unitOfWork.Viagem.Update() / _unitOfWork.Save()                         ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/CustosViagem/CalculaCustoViagens                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Custos de Viagem                                        ║
        /// ║    • Arquivos relacionados: Pages/CustosViagem/*.cshtml                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("CalculaCustoViagens")]
        [HttpPost]
        public IActionResult CalculaCustoViagens()
        {
            try
            {
                // [DADOS] Obtém viagens realizadas que não são apenas agendamentos
                var objViagens = _unitOfWork.Viagem.GetAll(v =>
                    v.StatusAgendamento == false && v.Status == "Realizada"
                );

                foreach (var viagem in objViagens)
                {
                    // [LOGICA] Recalcula custos conforme disponibilidade
                    if (viagem.MotoristaId != null)
                    {
                        var minutos = 0;
                        viagem.CustoMotorista = Servicos.CalculaCustoMotorista(viagem, _unitOfWork, ref minutos);
                    }

                    if (viagem.VeiculoId != null)
                    {
                        viagem.CustoVeiculo = Servicos.CalculaCustoVeiculo(viagem, _unitOfWork);
                        viagem.CustoCombustivel = Servicos.CalculaCustoCombustivel(viagem, _unitOfWork);
                    }

                    // [DADOS] Atualiza entidade
                    _unitOfWork.Viagem.Update(viagem);
                }

                // [DADOS] Persiste alterações
                _unitOfWork.Save();

                return Json(new { success = true });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "CalculaCustoViagens");
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "CalculaCustoViagens", error);
                return StatusCode(500, new { success = false, message = "Erro ao recalcular custos" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemVeiculos                                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Filtra custos de viagem por veículo específico.                           ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite análises de custo por veículo.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do veículo.                                               ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista filtrada.                                  ║
        /// ║    • Consumidor: UI de Custos de Viagem.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewCustosViagem.GetAll()                                    ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/CustosViagem/ViagemVeiculos                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Custos de Viagem                                        ║
        /// ║    • Arquivos relacionados: Pages/CustosViagem/*.cshtml                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemVeiculos")]
        [HttpGet]
        public IActionResult ViagemVeiculos(Guid Id)
        {
            try
            {
                // [DADOS] Filtra por veículo
                var result = _unitOfWork.ViewCustosViagem.GetAll(vv => vv.VeiculoId == Id && vv.StatusAgendamento == false);
                return Json(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "ViagemVeiculos");
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "ViagemVeiculos", error);
                return StatusCode(500, new { success = false, message = "Erro ao filtrar custos por veículo" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemMotoristas                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Filtra custos de viagem por motorista específico.                         ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite análises de custo por motorista.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do motorista.                                             ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista filtrada.                                  ║
        /// ║    • Consumidor: UI de Custos de Viagem.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewCustosViagem.GetAll()                                    ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/CustosViagem/ViagemMotoristas                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Custos de Viagem                                        ║
        /// ║    • Arquivos relacionados: Pages/CustosViagem/*.cshtml                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemMotoristas")]
        [HttpGet]
        public IActionResult ViagemMotoristas(Guid Id)
        {
            try
            {
                // [DADOS] Filtra por motorista
                var result = _unitOfWork.ViewCustosViagem.GetAll(vv => vv.MotoristaId == Id && vv.StatusAgendamento == false);
                return Json(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "ViagemMotoristas");
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "ViagemMotoristas", error);
                return StatusCode(500, new { success = false, message = "Erro ao filtrar custos por motorista" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemStatus                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Filtra custos de viagem por status (Realizada, Cancelada, etc.).          ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Facilita análises por situação das viagens.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): status da viagem.                                          ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista filtrada.                                  ║
        /// ║    • Consumidor: UI de Custos de Viagem.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewCustosViagem.GetAll()                                    ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/CustosViagem/ViagemStatus                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Custos de Viagem                                        ║
        /// ║    • Arquivos relacionados: Pages/CustosViagem/*.cshtml                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemStatus")]
        [HttpGet]
        public IActionResult ViagemStatus(string Id)
        {
            try
            {
                // [DADOS] Filtra por status
                var result = _unitOfWork.ViewCustosViagem.GetAll(vv => vv.Status == Id && vv.StatusAgendamento == false);
                return Json(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "ViagemStatus");
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "ViagemStatus", error);
                return StatusCode(500, new { success = false, message = "Erro ao filtrar custos por status" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemFinalidade                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Filtra custos de viagem por finalidade.                                   ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Apoia análises por motivo/objetivo da viagem.                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string): finalidade da viagem.                                      ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista filtrada.                                 ║
        /// ║    • Consumidor: UI de Custos de Viagem.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewCustosViagem.GetAll()                                    ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/CustosViagem/ViagemFinalidade                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Custos de Viagem                                        ║
        /// ║    • Arquivos relacionados: Pages/CustosViagem/*.cshtml                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemFinalidade")]
        [HttpGet]
        public IActionResult ViagemFinalidade(string Id)
        {
            try
            {
                // [DADOS] Filtra por finalidade
                return Json(
                    new
                    {
                        data = _unitOfWork
                            .ViewCustosViagem.GetAll()
                            .Where(vv => vv.Finalidade == Id && vv.StatusAgendamento == false),
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "ViagemFinalidade");
                Alerta.TratamentoErroComLinha(
                    "CustosViagemController.cs",
                    "ViagemFinalidade",
                    error
                );
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemSetores                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Filtra custos de viagem por setor solicitante.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid): ID do Setor Solicitante.                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista filtrada.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemSetores")]
        [HttpGet]
        public IActionResult ViagemSetores(Guid Id)
        {
            try
            {
                return Json(
                    new
                    {
                        data = _unitOfWork
                            .ViewCustosViagem.GetAll()
                            .Where(vv =>
                                vv.SetorSolicitanteId == Id && vv.StatusAgendamento == false
                            ),
                    }
                );
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "ViagemSetores");
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "ViagemSetores", error);
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ViagemData (GET)                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Filtra custos de viagem por data (yyyy-MM-dd).                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (string)                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista filtrada.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("ViagemData")]
        [HttpGet]
        public IActionResult ViagemData(string Id)
        {
            try
            {
                var result = _unitOfWork.ViewCustosViagem.GetAll(vv => vv.DataInicial == Id && vv.StatusAgendamento == false);
                return Json(new { data = result });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "ViagemData");
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "ViagemData", error);
                return StatusCode(500, new { success = false, message = "Erro ao filtrar por data" });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaFicha (GET)                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recupera imagem da ficha de vistoria de uma viagem.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid)                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: viagem com imagem (bytes) ou false.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("PegaFicha")]
        public JsonResult PegaFicha(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return Json(false);

                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id);
                if (objFromDb?.FichaVistoria != null)
                {
                    // (IA) Helper de conversão para garantir integridade do array de bytes
                    objFromDb.FichaVistoria = this.GetImage(Convert.ToBase64String(objFromDb.FichaVistoria));
                    return Json(objFromDb);
                }

                return Json(false);
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "PegaFicha");
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "PegaFicha", error);
                return new JsonResult(new { successo = false });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaFichaModal (GET)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna apenas os bytes da ficha de vistoria (modal).                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid)                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: bytes da imagem ou false.                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("PegaFichaModal")]
        public JsonResult PegaFichaModal(Guid id)
        {
            try
            {
                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id);
                if (objFromDb?.FichaVistoria != null)
                {
                    var bytes = this.GetImage(Convert.ToBase64String(objFromDb.FichaVistoria));
                    return Json(bytes);
                }
                return Json(false);
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "PegaFichaModal");
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "PegaFichaModal", error);
                return new JsonResult(new { sucesso = false });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetImage (Helper)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Converte string Base64 em array de bytes.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • sBase64String (string)                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • byte[]                                                                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public byte[] GetImage(string sBase64String)
        {
            try
            {
                if (string.IsNullOrEmpty(sBase64String)) return null;
                return Convert.FromBase64String(sBase64String);
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "GetImage");
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "GetImage", error);
                return null;
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PegaMotoristaVeiculo (GET)                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém metadados de relacionamento da viagem (motorista, veículo, etc.).  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid)                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • JsonResult: objeto com IDs relacionados.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        [Route("PegaMotoristaVeiculo")]
        public JsonResult PegaMotoristaVeiculo(Guid id)
        {
            try
            {
                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == id);
                if (objFromDb != null)
                {
                    return Json(new
                    {
                        success = true,
                        motoristaId = objFromDb.MotoristaId,
                        veiculoId = objFromDb.VeiculoId,
                        finalidadeId = objFromDb.Finalidade,
                        setorsolicitanteId = objFromDb.SetorSolicitanteId,
                        eventoId = objFromDb.EventoId,
                    });
                }

                return Json(new { success = false });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "CustosViagemController.cs", "PegaMotoristaVeiculo");
                Alerta.TratamentoErroComLinha("CustosViagemController.cs", "PegaMotoristaVeiculo", error);
                return new JsonResult(new { sucesso = false });
            }
        }
    }
}
