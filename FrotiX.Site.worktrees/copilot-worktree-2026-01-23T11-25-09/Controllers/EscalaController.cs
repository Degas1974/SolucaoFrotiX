using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrotiX.Helpers;
using FrotiX.Hubs;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using EscalaDiaria = FrotiX.Models.EscalaDiaria;
using FolgaRecesso = FrotiX.Models.FolgaRecesso;
using CoberturaFolga = FrotiX.Models.CoberturaFolga;

namespace FrotiX.Controllers
{
    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: EscalaController (Partial)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO: Controlador MVC e API para gestão de Escala de Motoristas.
    /// │            Gerencia agendamentos, conflitos e visualização de escalas.
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ ARQUIVOS PARCIAIS:
    /// │    - EscalaController.cs (Actions MVC, CRUD Views)
    /// │    - EscalaController_Api.cs (Endpoints API e DataTables)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
    public partial class EscalaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EscalaController> _logger;
        private readonly IHubContext<EscalaHub> _hubContext;

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Construtor
        /// │ DESCRIÇÃO: Inicializa o controlador de Escalas.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ PARÂMETROS:
        /// │    -> unitOfWork: Serviço de Unit of Work.
        /// │    -> logger: Serviço de Logging.
        /// │    -> hubContext: Contexto SignalR para atualizações em tempo real.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        public EscalaController(IUnitOfWork unitOfWork, ILogger<EscalaController> logger, IHubContext<EscalaHub> hubContext)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _hubContext = hubContext;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EscalaController.cs", "EscalaController", ex);
            }
        }

        #region Views

        // GET: Escala
      /*  public async Task<IActionResult> Index()
        {
            var model = new FiltroEscalaViewModel
            {
                DataFiltro = DateTime.Today,
                TipoServicoList = _unitOfWork.TipoServico.GetTipoServicoListForDropDown(),
                TurnoList = _unitOfWork.Turno.GetTurnoListForDropDown(),
                MotoristaList = _unitOfWork.Motorista.GetMotoristaListForDropDown(),
                VeiculoList = _unitOfWork.Veiculo.GetVeiculoListForDropDown(),
                LotacaoList = GetLotacaoList(),
                StatusList = GetStatusList()
            };

            return View(model);
        }*/

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Create (GET)                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exibe o formulário para criar uma nova escala de motorista.               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: View com modelo EscalaDiariaViewModel.                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public IActionResult Create()
        {
            try
            {
                var model = new EscalaDiariaViewModel
                {
                    DataEscala = DateTime.Today,
                    MotoristaList = _unitOfWork.Motorista.GetMotoristaListForDropDown(),
                    VeiculoList = _unitOfWork.Veiculo.GetVeiculoListForDropDown(),
                    TipoServicoList = _unitOfWork.TipoServico.GetTipoServicoListForDropDown(),
                    TurnoList = _unitOfWork.Turno.GetTurnoListForDropDown(),
                    RequisitanteList = _unitOfWork.Requisitante.GetRequisitanteListForDropDown(),
                    LotacaoList = GetLotacaoList(),
                    StatusList = GetStatusList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EscalaController.cs", "Create (GET)", ex);
                return RedirectToAction("Index");
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Create (POST)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa a criação de nova escala com validação de conflitos.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • model (EscalaDiariaViewModel): Dados da escala.                         ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Redirect para Index ou View com erros.                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EscalaDiariaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar conflitos
                    var horaInicio = TimeSpan.Parse(model.HoraInicio);
                    var horaFim = TimeSpan.Parse(model.HoraFim);

                    if (model.MotoristaId.HasValue)
                    {
                        var conflito = await _unitOfWork.EscalaDiaria.ExisteEscalaConflitanteAsync(
                            model.MotoristaId.Value, model.DataEscala, horaInicio, horaFim, null);

                        if (conflito)
                        {
                            TempData["error"] = "Já existe uma escala para este motorista neste horário";
                            return RedirectToAction(nameof(Create));
                        }

                        // Obter ou criar associação veículo-motorista
                        var associacao = await _unitOfWork.VAssociado.GetAssociacaoAtivaAsync(model.MotoristaId.Value);

                        if (associacao == null && model.VeiculoId.HasValue)
                        {
                            associacao = new VAssociado
                            {
                                MotoristaId = model.MotoristaId.Value,
                                VeiculoId = model.VeiculoId.Value,
                                DataInicio = DateTime.Now,
                                Ativo = true,
                                UsuarioIdAlteracao = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            };

                            _unitOfWork.VAssociado.Add(associacao);
                            await _unitOfWork.SaveAsync();
                        }

                        // Criar escala
                        var escala = new EscalaDiaria
                        {
                            AssociacaoId = associacao?.AssociacaoId,
                            TipoServicoId = model.TipoServicoId,
                            TurnoId = model.TurnoId,
                            DataEscala = model.DataEscala,
                            HoraInicio = horaInicio,
                            HoraFim = horaFim,
                            HoraIntervaloInicio = !string.IsNullOrEmpty(model.HoraIntervaloInicio) ?
                                TimeSpan.Parse(model.HoraIntervaloInicio) : null,
                            HoraIntervaloFim = !string.IsNullOrEmpty(model.HoraIntervaloFim) ?
                                TimeSpan.Parse(model.HoraIntervaloFim) : null,
                            Lotacao = model.Lotacao,
                            StatusMotorista = DeterminarStatus(model),
                            RequisitanteId = model.RequisitanteId,
                            Observacoes = model.Observacoes,
                            Ativo = true,
                            UsuarioIdAlteracao = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        };

                        _unitOfWork.EscalaDiaria.Add(escala);

                        // Tratar indisponibilidade se necessário
                        if (model.MotoristaIndisponivel && model.DataInicioIndisponibilidade.HasValue &&
                            model.DataFimIndisponibilidade.HasValue)
                        {
                            await CriarIndisponibilidade(model);
                        }

                        await _unitOfWork.SaveAsync();

                        // Notificar via SignalR
                        await NotificarAtualizacaoEscalas();

                        TempData["success"] = "Escala criada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["error"] = "Motorista é obrigatório";
                }
                catch (Exception ex)
                {
                    Alerta.TratamentoErroComLinha("EscalaController.cs", "Create (POST)", ex);
                    TempData["error"] = "Erro ao criar escala: " + ex.Message;
                }
            }

            // Recarregar listas em caso de erro
            model.MotoristaList = _unitOfWork.Motorista.GetMotoristaListForDropDown();
            model.VeiculoList = _unitOfWork.Veiculo.GetVeiculoListForDropDown();
            model.TipoServicoList = _unitOfWork.TipoServico.GetTipoServicoListForDropDown();
            model.TurnoList = _unitOfWork.Turno.GetTurnoListForDropDown();
            model.RequisitanteList = _unitOfWork.Requisitante.GetRequisitanteListForDropDown();
            model.LotacaoList = GetLotacaoList();
            model.StatusList = GetStatusList();

            return View(model);
        }

        // GET: Escala/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var escala = await _unitOfWork.EscalaDiaria.GetFirstOrDefaultAsync(e => e.EscalaDiaId == id, includeProperties: "Associacao,Associacao.Motorista,Associacao.Veiculo,TipoServico,Turno,Requisitante");

            if (escala == null)
            {
                TempData["error"] = "Escala não encontrada";
                return RedirectToAction(nameof(Index));
            }

            var model = new EscalaDiariaViewModel
            {
                EscalaDiaId = escala.EscalaDiaId,
                MotoristaId = escala.Associacao?.MotoristaId,
                VeiculoId = escala.Associacao?.VeiculoId,
                TipoServicoId = escala.TipoServicoId,
                TurnoId = escala.TurnoId,
                DataEscala = escala.DataEscala,
                HoraInicio = escala.HoraInicio.ToString(@"hh\:mm"),
                HoraFim = escala.HoraFim.ToString(@"hh\:mm"),
                HoraIntervaloInicio = escala.HoraIntervaloInicio?.ToString(@"hh\:mm"),
                HoraIntervaloFim = escala.HoraIntervaloFim?.ToString(@"hh\:mm"),
                Lotacao = escala.Lotacao,
                NumeroSaidas = await GetNumeroViagensRealizadas(escala.Associacao?.MotoristaId, escala.DataEscala),
                StatusMotorista = escala.StatusMotorista,
                RequisitanteId = escala.RequisitanteId,
                Observacoes = escala.Observacoes,
                MotoristaList = _unitOfWork.Motorista.GetMotoristaListForDropDown(),
                VeiculoList = _unitOfWork.Veiculo.GetVeiculoListForDropDown(),
                TipoServicoList = _unitOfWork.TipoServico.GetTipoServicoListForDropDown(),
                TurnoList = _unitOfWork.Turno.GetTurnoListForDropDown(),
                RequisitanteList = _unitOfWork.Requisitante.GetRequisitanteListForDropDown(),
                LotacaoList = GetLotacaoList(),
                StatusList = GetStatusList()
            };

            return View(model);
        }

        // POST: Escala/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EscalaDiariaViewModel model)
        {
            if (id != model.EscalaDiaId)
            {
                TempData["error"] = "ID inválido";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var escala = await _unitOfWork.EscalaDiaria.GetFirstOrDefaultAsync(e => e.EscalaDiaId == id);

                    if (escala == null)
                    {
                        TempData["error"] = "Escala não encontrada";
                        return RedirectToAction(nameof(Index));
                    }

                    // Verificar conflitos se o horário mudou
                    var horaInicio = TimeSpan.Parse(model.HoraInicio);
                    var horaFim = TimeSpan.Parse(model.HoraFim);

                    if (model.MotoristaId.HasValue &&
                        (escala.HoraInicio != horaInicio || escala.HoraFim != horaFim || escala.DataEscala != model.DataEscala))
                    {
                        var conflito = await _unitOfWork.EscalaDiaria.ExisteEscalaConflitanteAsync(
                            model.MotoristaId.Value, model.DataEscala, horaInicio, horaFim, id);

                        if (conflito)
                        {
                            TempData["error"] = "Já existe uma escala para este motorista neste horário";
                            return RedirectToAction(nameof(Edit), new { id });
                        }
                    }

                    // Atualizar escala
                    escala.TipoServicoId = model.TipoServicoId;
                    escala.TurnoId = model.TurnoId;
                    escala.DataEscala = model.DataEscala;
                    escala.HoraInicio = horaInicio;
                    escala.HoraFim = horaFim;
                    escala.HoraIntervaloInicio = !string.IsNullOrEmpty(model.HoraIntervaloInicio) ?
                        TimeSpan.Parse(model.HoraIntervaloInicio) : null;
                    escala.HoraIntervaloFim = !string.IsNullOrEmpty(model.HoraIntervaloFim) ?
                        TimeSpan.Parse(model.HoraIntervaloFim) : null;
                    escala.Lotacao = model.Lotacao;
                    escala.StatusMotorista = DeterminarStatus(model);
                    escala.RequisitanteId = model.RequisitanteId;
                    escala.Observacoes = model.Observacoes;
                    escala.DataAlteracao = DateTime.Now;
                    escala.UsuarioIdAlteracao = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _unitOfWork.EscalaDiaria.Update(escala);
                    await _unitOfWork.SaveAsync();

                    // Notificar via SignalR
                    await NotificarAtualizacaoEscalas();

                    TempData["success"] = "Escala atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Alerta.TratamentoErroComLinha("EscalaController.cs", "Edit (POST)", ex);
                    TempData["error"] = "Erro ao atualizar escala: " + ex.Message;
                }
            }

            // Recarregar listas em caso de erro
            model.MotoristaList = _unitOfWork.Motorista.GetMotoristaListForDropDown();
            model.VeiculoList = _unitOfWork.Veiculo.GetVeiculoListForDropDown();
            model.TipoServicoList = _unitOfWork.TipoServico.GetTipoServicoListForDropDown();
            model.TurnoList = _unitOfWork.Turno.GetTurnoListForDropDown();
            model.RequisitanteList = _unitOfWork.Requisitante.GetRequisitanteListForDropDown();
            model.LotacaoList = GetLotacaoList();
            model.StatusList = GetStatusList();

            return View(model);
        }

        // GET: Escala/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var escala = await _unitOfWork.EscalaDiaria.GetEscalaCompletaByIdAsync(id);

            if (escala == null)
            {
                TempData["error"] = "Escala não encontrada";
                return RedirectToAction(nameof(Index));
            }

            return View(escala);
        }

        // POST: Escala/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var escala = await _unitOfWork.EscalaDiaria.GetFirstOrDefaultAsync(e => e.EscalaDiaId == id);

                if (escala == null)
                {
                    TempData["error"] = "Escala não encontrada";
                    return RedirectToAction(nameof(Index));
                }

                escala.Ativo = false;
                escala.DataAlteracao = DateTime.Now;
                escala.UsuarioIdAlteracao = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _unitOfWork.EscalaDiaria.Update(escala);
                await _unitOfWork.SaveAsync();

                // Notificar via SignalR
                await NotificarAtualizacaoEscalas();

                TempData["success"] = "Escala excluída com sucesso!";
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EscalaController.cs", "DeleteConfirmed", ex);
                TempData["error"] = "Erro ao excluir escala: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Escala/FichaEscala
        public async Task<IActionResult> FichaEscala(DateTime? data)
        {
            var dataEscala = data ?? DateTime.Today;
            var escalas = await _unitOfWork.EscalaDiaria.GetEscalasCompletasAsync(dataEscala);

            ViewBag.DataEscala = dataEscala;
            return View(escalas);
        }

        #endregion

        #region API Methods

        // GET: Escala/GetEscalas
        /* ⚠️ COMENTADO: Método duplicado - causava conflito com EscalaController_Api.cs
        // GET: Escala/GetEscalas
        [HttpGet]
        public async Task<IActionResult> GetEscalas(DateTime? data)
        {
            try
            {
                var dataEscala = data ?? DateTime.Today;
                var escalas = await _unitOfWork.EscalaDiaria.GetEscalasCompletasAsync(dataEscala);

                // Adicionar número de viagens realizadas
                foreach (var escala in escalas)
                {
                    if (escala.MotoristaId.HasValue)
                    {
                        escala.NumeroSaidas = await GetNumeroViagensRealizadas(escala.MotoristaId.Value, dataEscala);
                    }
                }

                return Json(new { success = true, data = escalas });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar escalas");
                return Json(new { success = false, message = "Erro ao buscar escalas" });
            }
        }
        */

        // GET: Escala/GetEscalasFiltradas
      /*  [HttpPost]
        public async Task<IActionResult> GetEscalasFiltradas(FiltroEscalaViewModel filtro)
        {
            try
            {
                var escalas = await _unitOfWork.EscalaDiaria.GetEscalasPorFiltroAsync(filtro);

                // Adicionar número de viagens realizadas
                foreach (var escala in escalas)
                {
                    if (escala.MotoristaId.HasValue)
                    {
                        escala.NumeroSaidas = await GetNumeroViagensRealizadas(escala.MotoristaId.Value, escala.DataEscala);
                    }
                }

                return Json(new { success = true, data = escalas });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar escalas filtradas");
                return Json(new { success = false, message = "Erro ao buscar escalas" });
            }
        }*/

        /* ⚠️ COMENTADO: Método duplicado - causava conflito com EscalaController_Api.cs
        // GET: Escala/GetMotoristasVez
        [HttpGet]
        public async Task<IActionResult> GetMotoristasVez(int quantidade = 5)
        {
            try
            {
                var motoristas = await _unitOfWork.EscalaDiaria.GetMotoristasVezAsync(quantidade);
                return Json(new { success = true, data = motoristas });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar motoristas da vez");
                return Json(new { success = false, message = "Erro ao buscar motoristas da vez" });
            }
        }
        */

        // POST: Escala/AtualizarStatus
        [HttpPost]
        public async Task<IActionResult> AtualizarStatus(Guid motoristaId, string novoStatus, DateTime? data)
        {
            try
            {
                var sucesso = await _unitOfWork.EscalaDiaria.AtualizarStatusMotoristaAsync(
                    motoristaId, novoStatus, data);

                if (sucesso)
                {
                    await _unitOfWork.SaveAsync();
                    await NotificarAtualizacaoEscalas();
                    return Json(new { success = true, message = "Status atualizado com sucesso" });
                }

                return Json(new { success = false, message = "Não foi possível atualizar o status" });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EscalaController.cs", "AtualizarStatus", ex);
                return Json(new { success = false, message = "Erro ao atualizar status" });
            }
        }

        #endregion

        #region Helper Methods

        private string DeterminarStatus(EscalaDiariaViewModel model)
        {
            if (model.MotoristaIndisponivel) return "Indisponível";
            if (model.MotoristaEconomildo) return "Economildo";
            if (model.MotoristaEmServico) return "Em Serviço";
            if (model.MotoristaReservado) return "Reservado para Serviço";

            return "Disponível";
        }

        private async Task CriarIndisponibilidade(EscalaDiariaViewModel model)
        {
            if (!model.MotoristaId.HasValue || !model.DataInicioIndisponibilidade.HasValue ||
                !model.DataFimIndisponibilidade.HasValue)
                return;

            if (model.CategoriaIndisponibilidade == "Férias")
            {
                var ferias = new Ferias
                {
                    MotoristaId = model.MotoristaId.Value,
                    MotoristaSubId = model.MotoristaCobertorId,
                    DataInicio = model.DataInicioIndisponibilidade.Value,
                    DataFim = model.DataFimIndisponibilidade.Value,
                    Ativo = true,
                    UsuarioIdAlteracao = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                _unitOfWork.Ferias.Add(ferias);
            }
            else
            {
                var folga = new FolgaRecesso
                {
                    MotoristaId = model.MotoristaId.Value,
                    DataInicio = model.DataInicioIndisponibilidade.Value,
                    DataFim = model.DataFimIndisponibilidade.Value,
                    Tipo = model.CategoriaIndisponibilidade ?? "Folga",
                    Ativo = true,
                    UsuarioIdAlteracao = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                _unitOfWork.FolgaRecesso.Add(folga);
            }

            // Criar cobertura se houver motorista cobertor
            if (model.MotoristaCobertorId.HasValue)
            {
                var cobertura = new CoberturaFolga
                {
                    MotoristaFolgaId = model.MotoristaId.Value,
                    MotoristaCoberturaId = model.MotoristaCobertorId.Value,
                    DataInicio = model.DataInicioIndisponibilidade.Value,
                    DataFim = model.DataFimIndisponibilidade.Value,
                    Motivo = model.CategoriaIndisponibilidade,
                    Ativo = true,
                    UsuarioIdAlteracao = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                _unitOfWork.CoberturaFolga.Add(cobertura);
            }
        }

        private async Task<int> GetNumeroViagensRealizadas(Guid? motoristaId, DateTime data)
        {
            if (!motoristaId.HasValue)
                return 0;

            return await _unitOfWork.Viagem
                .GetAllAsync(v => v.MotoristaId == motoristaId.Value &&
                                 v.DataFinalizacao == data.Date &&
                                 v.Status == "Realizada")
                .ContinueWith(t => t.Result.Count());
        }

        private async Task NotificarAtualizacaoEscalas()
        {
            try
            {
                // Notificar todos os clientes sobre a atualização
                await _hubContext.Clients.All.SendAsync("AtualizarEscalas");

                // Atualizar motoristas da vez
                var motoristasVez = await _unitOfWork.EscalaDiaria.GetMotoristasVezAsync(5);
                await _hubContext.Clients.All.SendAsync("AtualizarMotoristasVez", motoristasVez);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EscalaController.cs", "NotificarAtualizacaoEscalas", ex);
            }
        }

        private IEnumerable<SelectListItem> GetLotacaoList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Selecione...", Value = "" },
                new SelectListItem { Text = "Aeroporto", Value = "Aeroporto" },
                new SelectListItem { Text = "Rodoviária", Value = "Rodoviária" },
                new SelectListItem { Text = "PGR", Value = "PGR" },
                new SelectListItem { Text = "Cefor", Value = "Cefor" },
                new SelectListItem { Text = "Setor de Obras", Value = "Setor de Obras" },
                new SelectListItem { Text = "Outros", Value = "Outros" }
            };
        }

        private IEnumerable<SelectListItem> GetStatusList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Disponível", Value = "Disponível" },
                new SelectListItem { Text = "Em Viagem", Value = "Em Viagem" },
                new SelectListItem { Text = "Indisponível", Value = "Indisponível" },
                new SelectListItem { Text = "Em Serviço", Value = "Em Serviço" },
                new SelectListItem { Text = "Economildo", Value = "Economildo" },
                new SelectListItem { Text = "Reservado para Serviço", Value = "Reservado para Serviço" }
            };
        }

        #endregion
    }
}
