using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrotiX.Controllers
{
    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: EscalaController (Partial API)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO: Endpoints API para suporte a DataTables e operações AJAX.
    /// │            Complementa o EscalaController principal (MVC).
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ OBSERVACAO:
    /// │    - Não possui atributos [Route] de classe para evitar conflito MVC.
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
    public partial class EscalaController : Controller
    {
        // ===================================================================
        // API ENDPOINTS PARA DATATABLES SERVER-SIDE
        // ===================================================================

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ListaEscalasServerSide                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Endpoint server-side para DataTables com filtros e paginação.             ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite listagem escalável com filtros avançados.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Form Data (draw, start, length, search, filters...).                    ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON compatível com DataTables.                          ║
        /// ║    • Consumidor: UI DataTables (Escala).                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewEscalasCompletas.GetAll()                                ║
        /// ║    • _logger.LogError() / Alerta.TratamentoErroComLinha() → erros.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/Escala/ListaEscalasServerSide                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Escalas                                                 ║
        /// ║    • Arquivos relacionados: Pages/Escala/*.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpPost]
        [Route("api/Escala/ListaEscalasServerSide")]
        public IActionResult ListaEscalasServerSide()
        {
            try
            {
            // [DADOS] Parâmetros do DataTables
                var draw = Request.Form["draw"].FirstOrDefault();
                var startStr = Request.Form["start"].FirstOrDefault();
                var lengthStr = Request.Form["length"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = lengthStr != null ? Convert.ToInt32(lengthStr) : 10;
                int skip = startStr != null ? Convert.ToInt32(startStr) : 0;

                // [DADOS] Filtros customizados
                var dataFiltroStr = Request.Form["dataFiltro"].FirstOrDefault();
                var tipoServicoIdStr = Request.Form["tipoServicoId"].FirstOrDefault();
                var turnoIdStr = Request.Form["turnoId"].FirstOrDefault();
                var statusMotorista = Request.Form["statusMotorista"].FirstOrDefault();
                var motoristaIdStr = Request.Form["motoristaId"].FirstOrDefault();
                var veiculoIdStr = Request.Form["veiculoId"].FirstOrDefault();
                var lotacao = Request.Form["lotacao"].FirstOrDefault();
                var textoPesquisa = Request.Form["textoPesquisa"].FirstOrDefault();

                // [DADOS] Query base
                var query = _unitOfWork.ViewEscalasCompletas.GetAll().AsQueryable();

                // [LOGICA] Filtro de data
                if (!string.IsNullOrEmpty(dataFiltroStr))
                {
                    if (DateTime.TryParse(dataFiltroStr, out var dataFiltro))
                    {
                        query = query.Where(e => e.DataEscala.Date == dataFiltro.Date);
                    }
                }

                // [LOGICA] Filtro de tipo de serviço
                if (!string.IsNullOrEmpty(tipoServicoIdStr) && Guid.TryParse(tipoServicoIdStr, out var tipoServicoId))
                {
                    query = query.Where(e => e.TipoServicoId == tipoServicoId);
                }

                // [LOGICA] Filtro de turno
                if (!string.IsNullOrEmpty(turnoIdStr) && Guid.TryParse(turnoIdStr, out var turnoId))
                {
                    query = query.Where(e => e.TurnoId == turnoId);
                }

                // [LOGICA] Filtro de status
                if (!string.IsNullOrEmpty(statusMotorista))
                {
                    query = query.Where(e => e.StatusMotorista == statusMotorista);
                }

                // [LOGICA] Filtro de motorista
                if (!string.IsNullOrEmpty(motoristaIdStr) && Guid.TryParse(motoristaIdStr, out var motoristaId))
                {
                    query = query.Where(e => e.MotoristaId == motoristaId);
                }

                // [LOGICA] Filtro de veículo
                if (!string.IsNullOrEmpty(veiculoIdStr) && Guid.TryParse(veiculoIdStr, out var veiculoId))
                {
                    query = query.Where(e => e.VeiculoId == veiculoId);
                }

                // [LOGICA] Filtro de lotação
                if (!string.IsNullOrEmpty(lotacao))
                {
                    query = query.Where(e => e.Lotacao == lotacao);
                }

                // [LOGICA] Pesquisa livre (DataTables + campo customizado)
                var searchTerm = !string.IsNullOrEmpty(textoPesquisa) ? textoPesquisa : searchValue;
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var lowerSearch = searchTerm.ToLower();
                    query = query.Where(e =>
                        (e.NomeMotorista != null && e.NomeMotorista.ToLower().Contains(lowerSearch)) ||
                        (e.Placa != null && e.Placa.ToLower().Contains(lowerSearch)) ||
                        (e.NomeServico != null && e.NomeServico.ToLower().Contains(lowerSearch)) ||
                        (e.NomeRequisitante != null && e.NomeRequisitante.ToLower().Contains(lowerSearch)) ||
                        (e.Lotacao != null && e.Lotacao.ToLower().Contains(lowerSearch)) ||
                        (e.StatusMotorista != null && e.StatusMotorista.ToLower().Contains(lowerSearch))
                    );
                }

                // [DADOS] Total de registros (sem filtro)
                int recordsTotal = _unitOfWork.ViewEscalasCompletas.GetAll().Count();

                // [DADOS] Total de registros (com filtro)
                int recordsFiltered = query.Count();

                // [LOGICA] Ordenação
                query = query.OrderByDescending(e => e.DataEscala)
                            .ThenBy(e => e.HoraInicio)
                            .ThenBy(e => e.NomeMotorista);

                // [LOGICA] Paginação
                var pageData = query.Skip(skip).Take(pageSize).ToList();

                // [DADOS] Formatação de dados para DataTables
                var data = pageData.Select(x => new
                {
                    escalaDiaId = x.EscalaDiaId,
                    nomeTurno = x.NomeTurno,
                    dataEscala = x.DataEscala.ToString("dd/MM/yyyy"),
                    horaInicio = x.HoraInicio,
                    horaFim = x.HoraFim,
                    nomeServico = x.NomeServico,
                    lotacao = x.Lotacao,
                    numeroSaidas = x.NumeroSaidas,
                    nomeMotorista = x.NomeMotorista,
                    placa = x.Placa,
                    statusMotorista = x.StatusMotorista,
                    motoristaId = x.MotoristaId,
                    veiculoId = x.VeiculoId
                }).ToList();

                // [DADOS] Retorno no formato DataTables
                var result = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsFiltered,
                    data = data
                };

                return Json(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("EscalaController_Api.cs", "ListaEscalasServerSide", error);
                _logger?.LogError(error, "Erro ao processar ListaEscalasServerSide");

                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<object>(),
                    error = "Erro ao carregar escalas: " + error.Message
                });
            }
        }

        // ===================================================================
        // API ENDPOINTS ADICIONAIS
        // ===================================================================

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetEscalasFiltradas                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna escalas filtradas (alternativa ao server-side).                   ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite filtros rápidos sem DataTables server-side.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dataFiltro (DateTime?): data específica.                                ║
        /// ║    • tipoServicoId (string?): tipo de serviço.                               ║
        /// ║    • turnoId (string?): turno.                                               ║
        /// ║    • motoristaId (Guid?): motorista.                                         ║
        /// ║    • veiculoId (Guid?): veículo.                                             ║
        /// ║    • statusMotorista (string?): status do motorista.                         ║
        /// ║    • lotacao (string?): lotação.                                             ║
        /// ║    • textoPesquisa (string?): pesquisa livre.                                ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com escalas filtradas.                              ║
        /// ║    • Consumidor: UI de Escalas.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewEscalasCompletas.GetAll()                                ║
        /// ║    • _logger.LogError() / Alerta.TratamentoErroComLinha() → erros.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/Escala/GetEscalasFiltradas                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Escalas                                                 ║
        /// ║    • Arquivos relacionados: Pages/Escala/*.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpPost]
        [Route("api/Escala/GetEscalasFiltradas")]
        public IActionResult GetEscalasFiltradas([FromForm] DateTime? dataFiltro,
            [FromForm] string? tipoServicoId,
            [FromForm] string? turnoId,
            [FromForm] Guid? motoristaId,
            [FromForm] Guid? veiculoId,
            [FromForm] string? statusMotorista,
            [FromForm] string? lotacao,
            [FromForm] string? textoPesquisa)
        {
            try
            {
                // [DADOS] Query base
                var query = _unitOfWork.ViewEscalasCompletas.GetAll();

                // [LOGICA] Aplicar filtros
                if (dataFiltro.HasValue)
                {
                    query = query.Where(e => e.DataEscala.Date == dataFiltro.Value.Date);
                }

                if (!string.IsNullOrWhiteSpace(tipoServicoId))
                {
                    query = query.Where(e => e.NomeServico == tipoServicoId);
                }

                if (!string.IsNullOrWhiteSpace(turnoId))
                {
                    query = query.Where(e => e.NomeTurno == turnoId);
                }

                if (motoristaId.HasValue && motoristaId.Value != Guid.Empty)
                {
                    query = query.Where(e => e.MotoristaId == motoristaId.Value);
                }

                if (veiculoId.HasValue && veiculoId.Value != Guid.Empty)
                {
                    query = query.Where(e => e.VeiculoId == veiculoId.Value);
                }

                if (!string.IsNullOrWhiteSpace(statusMotorista))
                {
                    query = query.Where(e => e.StatusMotorista == statusMotorista);
                }

                if (!string.IsNullOrWhiteSpace(lotacao))
                {
                    query = query.Where(e => e.Lotacao == lotacao);
                }

                if (!string.IsNullOrWhiteSpace(textoPesquisa))
                {
                    // [LOGICA] Pesquisa livre
                    var texto = textoPesquisa.ToLower();
                    query = query.Where(e =>
                        (e.NomeMotorista != null && e.NomeMotorista.ToLower().Contains(texto)) ||
                        (e.Placa != null && e.Placa.ToLower().Contains(texto)) ||
                        (e.NomeServico != null && e.NomeServico.ToLower().Contains(texto)) ||
                        (e.NomeRequisitante != null && e.NomeRequisitante.ToLower().Contains(texto))
                    );
                }

                var escalas = query
                    .OrderBy(e => e.DataEscala)
                    .ThenBy(e => e.HoraInicio)
                    .ToList();

                return Json(new { success = true, data = escalas });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Erro ao buscar escalas filtradas");
                Alerta.TratamentoErroComLinha("EscalaController_Api.cs", "GetEscalasFiltradas", ex);
                return Json(new { success = false, message = "Erro ao buscar escalas filtradas: " + ex.Message });
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: VerificarDisponibilidade                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Verifica disponibilidade do motorista em uma data específica.             ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Evita conflitos na criação de escalas.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • motoristaId (Guid): motorista alvo.                                     ║
        /// ║    • data (DateTime): data da escala.                                        ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com flag de disponibilidade.                        ║
        /// ║    • Consumidor: UI de Escalas.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewEscalasCompletas.GetAll()                                ║
        /// ║    • _logger.LogError() / Alerta.TratamentoErroComLinha() → erros.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Escala/VerificarDisponibilidade                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Escalas                                                 ║
        /// ║    • Arquivos relacionados: Pages/Escala/*.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("api/Escala/VerificarDisponibilidade")]
        public async Task<IActionResult> VerificarDisponibilidade(Guid motoristaId, DateTime data)
        {
            try
            {
                // [DADOS] Verifica se existe escala na data
                var escalaExistente = _unitOfWork.ViewEscalasCompletas
                    .GetAll()
                    .Any(e => e.MotoristaId == motoristaId && e.DataEscala.Date == data.Date);

                return Json(new
                {
                    success = true,
                    disponivel = !escalaExistente,
                    message = escalaExistente ? "Motorista já possui escala nesta data" : "Motorista disponível"
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Erro ao verificar disponibilidade");
                Alerta.TratamentoErroComLinha("EscalaController_Api.cs", "VerificarDisponibilidade", ex);
                return Json(new { success = false, message = "Erro ao verificar disponibilidade" });
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AtualizarStatusMotorista                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Atualiza o status do motorista em uma escala específica.                  ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Mantém status operacional atualizado na escala diária.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • motoristaId (Guid): motorista alvo.                                     ║
        /// ║    • novoStatus (string): novo status.                                       ║
        /// ║    • data (DateTime?): data da escala.                                       ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com resultado da atualização.                       ║
        /// ║    • Consumidor: UI de Escalas.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewEscalasCompletas.GetAll()                                ║
        /// ║    • _unitOfWork.EscalaDiaria.GetFirstOrDefault() / Update / Save             ║
        /// ║    • _logger.LogError() / Alerta.TratamentoErroComLinha() → erros.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/Escala/AtualizarStatusMotorista                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Escalas                                                 ║
        /// ║    • Arquivos relacionados: Pages/Escala/*.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpPost]
        [Route("api/Escala/AtualizarStatusMotorista")]
        public async Task<IActionResult> AtualizarStatusMotorista(Guid motoristaId, string novoStatus, DateTime? data)
        {
            try
            {
                // [LOGICA] Determina data alvo
                var dataFiltro = data ?? DateTime.Today;

                // [DADOS] Buscar a escala do motorista na data especificada
                var escalaView = _unitOfWork.ViewEscalasCompletas
                    .GetAll()
                    .FirstOrDefault(e => e.MotoristaId == motoristaId && e.DataEscala.Date == dataFiltro.Date);

                if (escalaView == null)
                {
                    return Json(new { success = false, message = "Escala não encontrada" });
                }

                // [DADOS] Atualizar entidade EscalaDiaria
                var escala = _unitOfWork.EscalaDiaria.GetFirstOrDefault(e => e.EscalaDiaId == escalaView.EscalaDiaId);

                if (escala == null)
                {
                    return Json(new { success = false, message = "Escala não encontrada" });
                }

                escala.StatusMotorista = novoStatus;
                escala.DataAlteracao = DateTime.Now;

                _unitOfWork.EscalaDiaria.Update(escala);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Status atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Erro ao atualizar status");
                Alerta.TratamentoErroComLinha("EscalaController_Api.cs", "AtualizarStatusMotorista", ex);
                return Json(new { success = false, message = "Erro ao atualizar status" });
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DeleteEscala                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exclui uma escala pelo identificador.                                     ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite remoção de registros inválidos ou duplicados.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): identificador da escala.                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com resultado da exclusão.                          ║
        /// ║    • Consumidor: UI de Escalas.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.EscalaDiaria.GetFirstOrDefault() / Remove / Save             ║
        /// ║    • _logger.LogError() / Alerta.TratamentoErroComLinha() → erros.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/Escala/DeleteEscala/{id}                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Escalas                                                 ║
        /// ║    • Arquivos relacionados: Pages/Escala/*.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpPost]
        [Route("api/Escala/DeleteEscala/{id}")]
        public async Task<IActionResult> DeleteEscala(Guid id)
        {
            try
            {
            // [DADOS] Busca escala
                var escala = _unitOfWork.EscalaDiaria.GetFirstOrDefault(e => e.EscalaDiaId == id);

                if (escala == null)
                {
                    return Json(new { success = false, message = "Escala não encontrada" });
                }

                _unitOfWork.EscalaDiaria.Remove(escala);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Escala excluída com sucesso" });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Erro ao excluir escala");
                Alerta.TratamentoErroComLinha("EscalaController_Api.cs", "DeleteEscala", ex);
                return Json(new { success = false, message = "Erro ao excluir escala" });
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetEscala                                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna dados de uma escala específica.                                   ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite edição e visualização detalhada de escala.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): identificador da escala.                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados da escala.                                ║
        /// ║    • Consumidor: UI de Escalas.                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.ViewEscalasCompletas.GetFirstOrDefault()                     ║
        /// ║    • _logger.LogError() / Alerta.TratamentoErroComLinha() → erros.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Escala/GetEscala/{id}                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Escalas                                                 ║
        /// ║    • Arquivos relacionados: Pages/Escala/*.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpGet]
        [Route("api/Escala/GetEscala/{id}")]
        public async Task<IActionResult> GetEscala(Guid id)
        {
            try
            {
            // [DADOS] Busca escala na view
                var escala = _unitOfWork.ViewEscalasCompletas.GetFirstOrDefault(e => e.EscalaDiaId == id);

                if (escala == null)
                {
                    return Json(new { success = false, message = "Escala não encontrada" });
                }

                return Json(new { success = true, data = escala });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Erro ao buscar escala");
                Alerta.TratamentoErroComLinha("EscalaController_Api.cs", "GetEscala", ex);
                return Json(new { success = false, message = "Erro ao buscar escala" });
            }
        }

        // =====================================================
        // MÉTODO EditEscala - VERSÃO 16
        // CORREÇÃO: Quando escala atual está FORA do período de
        // indisponibilidade, mantém o status que ela JÁ TINHA no banco
        // 
        // Substituir no EscalaController_Api.cs
        // =====================================================

        /// <summary>
        /// Atualiza uma escala existente via API
        /// Rota: POST /api/Escala/EditEscala
        /// </summary>
        [HttpPost]
        [Route("api/Escala/EditEscala")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> EditEscala([FromForm] EditEscalaRequest model)
        {
            try
            {
                if (model == null || model.EscalaDiaId == Guid.Empty)
                {
                    return Json(new { success = false, message = "ID da escala inválido" });
                }

                var horaInicio = TimeSpan.Parse(model.HoraInicio);
                var horaFim = TimeSpan.Parse(model.HoraFim);
                TimeSpan? horaIntervaloInicio = !string.IsNullOrEmpty(model.HoraIntervaloInicio) ?
                    TimeSpan.Parse(model.HoraIntervaloInicio) : null;
                TimeSpan? horaIntervaloFim = !string.IsNullOrEmpty(model.HoraIntervaloFim) ?
                    TimeSpan.Parse(model.HoraIntervaloFim) : null;

                var db = HttpContext.RequestServices.GetService(typeof(FrotiXDbContext)) as FrotiXDbContext;

                if (db == null)
                {
                    return Json(new { success = false, message = "Erro interno: DbContext não disponível" });
                }

                var escala = await db.EscalaDiaria.FirstOrDefaultAsync(e => e.EscalaDiaId == model.EscalaDiaId);

                if (escala == null)
                {
                    return Json(new { success = false, message = "Escala não encontrada" });
                }

                var usuarioId = User?.Identity?.Name ?? "Sistema";
                int escalasAtualizadas = 0;
                int escalasCriadas = 0;

                // =====================================================
                // GUARDAR STATUS ATUAL DA ESCALA (antes de qualquer alteração)
                // =====================================================
                string statusAnteriorEscalaAtual = escala.StatusMotorista;
                string lotacaoAnteriorEscalaAtual = escala.Lotacao;
                Guid? requisitanteAnteriorEscalaAtual = escala.RequisitanteId;

                // =====================================================
                // DETERMINAR STATUS BASE (do formulário)
                // =====================================================
                string statusBase = "Disponível";
                if (model.MotoristaEconomildo) statusBase = "Economildo";
                else if (model.MotoristaEmServico) statusBase = "Em Serviço";
                else if (model.MotoristaReservado) statusBase = "Reservado para Serviço";

                // =====================================================
                // VERIFICAR SE ESTÁ MARCANDO COMO INDISPONÍVEL
                // =====================================================
                bool condicaoIndisponibilidade = model.MotoristaIndisponivel &&
                    model.MotoristaId.HasValue &&
                    model.DataInicioIndisponibilidade.HasValue &&
                    model.DataFimIndisponibilidade.HasValue &&
                    !string.IsNullOrEmpty(model.CategoriaIndisponibilidade);

                // =====================================================
                // DETERMINAR O STATUS FINAL DA ESCALA ATUAL
                // =====================================================
                string statusFinal;

                if (condicaoIndisponibilidade)
                {
                    var dataInicio = model.DataInicioIndisponibilidade.Value.Date;
                    var dataFim = model.DataFimIndisponibilidade.Value.Date;

                    // Verificar se a data da escala atual está no período
                    if (model.DataEscala.Date >= dataInicio && model.DataEscala.Date <= dataFim)
                    {
                        // Está no período - fica Indisponível
                        statusFinal = "Indisponível";
                    }
                    else
                    {
                        // FORA do período - MANTÉM o status que já tinha no banco
                        // (não usa statusBase pois os checkboxes foram desmarcados pelo JS)
                        if (statusAnteriorEscalaAtual != "Indisponível")
                        {
                            statusFinal = statusAnteriorEscalaAtual;
                        }
                        else
                        {
                            // Se já estava Indisponível, usa Disponível como fallback
                            statusFinal = "Disponível";
                        }
                    }
                }
                else if (model.MotoristaIndisponivel)
                {
                    // Marcou indisponível mas sem período definido - aplica direto
                    statusFinal = "Indisponível";
                }
                else
                {
                    // Não está marcando como indisponível - usa o status do formulário
                    statusFinal = statusBase;
                }

                // =====================================================
                // ATUALIZAR ESCALA ATUAL
                // =====================================================
                escala.TipoServicoId = model.TipoServicoId;
                escala.TurnoId = model.TurnoId;
                escala.DataEscala = model.DataEscala;
                escala.HoraInicio = horaInicio;
                escala.HoraFim = horaFim;
                escala.HoraIntervaloInicio = horaIntervaloInicio;
                escala.HoraIntervaloFim = horaIntervaloFim;

                bool foraDoPeríodo = condicaoIndisponibilidade &&
                   (model.DataEscala.Date < model.DataInicioIndisponibilidade.Value.Date ||
                    model.DataEscala.Date > model.DataFimIndisponibilidade.Value.Date);

                // LOTAÇÃO
                if (foraDoPeríodo)
                {
                    // Fora do período - mantém a lotação que já estava no banco
                    // (não altera escala.Lotacao)
                }
                else if (statusFinal == "Economildo")
                {
                    // É Economildo - usa a lotação do formulário
                    escala.Lotacao = model.Lotacao;
                }
                else if (statusFinal == "Indisponível")
                {
                    // É Indisponível - mantém a lotação que já estava no banco
                    // (não altera escala.Lotacao)
                }
                else
                {
                    // Qualquer outro status - limpa a lotação
                    escala.Lotacao = null;
                }

                // REQUISITANTE
                if (foraDoPeríodo)
                {
                    // Fora do período - mantém o requisitante que já estava no banco
                    // (não altera escala.RequisitanteId)
                }
                else if (statusFinal == "Em Serviço")
                {
                    // É Em Serviço - usa o requisitante do formulário
                    escala.RequisitanteId = model.RequisitanteId;
                }
                else if (statusFinal == "Indisponível")
                {
                    // É Indisponível - mantém o requisitante que já estava no banco
                    // (não altera escala.RequisitanteId)
                }
                else
                {
                    // Qualquer outro status - limpa o requisitante
                    escala.RequisitanteId = null;
                }

                escala.StatusMotorista = statusFinal;
                escala.Observacoes = model.Observacoes;
                escala.DataAlteracao = DateTime.Now;
                escala.UsuarioIdAlteracao = usuarioId;

                if (model.MotoristaId.HasValue)
                {
                    // ✅ CORREÇÃO: Sempre criar/buscar VAssociado, mesmo sem veículo
                    // VeiculoId pode ser NULL na associação
                    var associacao = await db.VAssociado.FirstOrDefaultAsync(
                        a => a.MotoristaId == model.MotoristaId.Value &&
                             a.VeiculoId == model.VeiculoId &&  // Pode ser NULL
                             a.Ativo);

                    if (associacao == null)
                    {
                        associacao = new VAssociado
                        {
                            AssociacaoId = Guid.NewGuid(),
                            MotoristaId = model.MotoristaId.Value,
                            VeiculoId = model.VeiculoId,  // Pode ser NULL (veículo não definido)
                            DataInicio = model.DataEscala,
                            Ativo = true,
                            DataCriacao = DateTime.Now,
                            UsuarioIdAlteracao = usuarioId
                        };
                        db.VAssociado.Add(associacao);
                        await db.SaveChangesAsync();
                    }
                    escala.AssociacaoId = associacao.AssociacaoId;
                }

                db.EscalaDiaria.Update(escala);
                await db.SaveChangesAsync();

                // Variável para controle de troca de cobertor (declarada fora do bloco para uso no retorno)
                bool houveTrocaDeCobertor = false;

                // =====================================================
                // LÓGICA DE INDISPONIBILIDADE (com período definido)
                // =====================================================
                if (condicaoIndisponibilidade)
                {
                    var motoristaId = model.MotoristaId.Value;
                    var dataInicio = model.DataInicioIndisponibilidade.Value.Date;
                    var dataFim = model.DataFimIndisponibilidade.Value.Date;

                    // =====================================================
                    // BUSCAR TODAS AS ESCALAS DO MOTORISTA NO PERÍODO
                    // =====================================================
                    var todasAssociacoes = await db.VAssociado
                        .Where(a => a.MotoristaId == motoristaId)
                        .Select(a => a.AssociacaoId)
                        .ToListAsync();

                    // Dicionário para guardar status original de cada dia
                    var statusPorDia = new Dictionary<DateTime, (string Status, Guid? RequisitanteId, string Lotacao)>();

                    if (todasAssociacoes.Any())
                    {
                        var escalasNoPeriodo = await db.EscalaDiaria
                            .Where(e => e.AssociacaoId.HasValue &&
                                       todasAssociacoes.Contains(e.AssociacaoId.Value) &&
                                       e.DataEscala >= dataInicio &&
                                       e.DataEscala <= dataFim &&
                                       e.Ativo)
                            .ToListAsync();

                        // =====================================================
                        // CAPTURAR STATUS ORIGINAL DE CADA DIA
                        // =====================================================
                        foreach (var esc in escalasNoPeriodo)
                        {
                            string statusOriginalDia;
                            Guid? requisitanteOriginal;
                            string lotacaoOriginal;

                            // Se for a escala atual, usar os valores que guardamos ANTES da alteração
                            if (esc.EscalaDiaId == model.EscalaDiaId)
                            {
                                statusOriginalDia = statusAnteriorEscalaAtual != "Indisponível" 
                                    ? statusAnteriorEscalaAtual 
                                    : "Disponível";
                                requisitanteOriginal = requisitanteAnteriorEscalaAtual;
                                lotacaoOriginal = lotacaoAnteriorEscalaAtual;
                            }
                            else if (esc.StatusMotorista == "Indisponível")
                            {
                                // Já está indisponível - buscar status original da cobertura existente
                                var coberturaAnterior = await db.CoberturaFolga
                                    .FirstOrDefaultAsync(c => c.MotoristaFolgaId == motoristaId &&
                                                              c.DataInicio <= esc.DataEscala &&
                                                              c.DataFim >= esc.DataEscala &&
                                                              c.Ativo);

                                if (coberturaAnterior != null && !string.IsNullOrEmpty(coberturaAnterior.StatusOriginal))
                                {
                                    statusOriginalDia = coberturaAnterior.StatusOriginal;
                                }
                                else
                                {
                                    statusOriginalDia = "Disponível";
                                }
                                requisitanteOriginal = esc.RequisitanteId;
                                lotacaoOriginal = esc.Lotacao;
                            }
                            else
                            {
                                statusOriginalDia = esc.StatusMotorista;
                                requisitanteOriginal = esc.RequisitanteId;
                                lotacaoOriginal = esc.Lotacao;
                            }

                            statusPorDia[esc.DataEscala.Date] = (statusOriginalDia, requisitanteOriginal, lotacaoOriginal);
                        }

                        // =====================================================
                        // PROPAGAR STATUS INDISPONÍVEL PARA ESCALAS NO PERÍODO
                        // =====================================================
                        var escalasParaAtualizar = escalasNoPeriodo
                            .Where(e => e.EscalaDiaId != model.EscalaDiaId)
                            .ToList();

                        foreach (var escalaPeriodo in escalasParaAtualizar)
                        {
                            escalaPeriodo.StatusMotorista = "Indisponível";
                            escalaPeriodo.DataAlteracao = DateTime.Now;
                            escalaPeriodo.UsuarioIdAlteracao = usuarioId;
                            db.EscalaDiaria.Update(escalaPeriodo);
                            escalasAtualizadas++;
                        }

                        if (escalasAtualizadas > 0)
                        {
                            await db.SaveChangesAsync();
                        }
                    }

                    // =====================================================
                    // DETERMINAR STATUS ORIGINAL PREDOMINANTE
                    // =====================================================
                    string statusOriginalPredominante = "Disponível";

                    if (statusPorDia.Any())
                    {
                        var statusMaisComum = statusPorDia.Values
                            .Where(v => v.Status != "Indisponível")
                            .GroupBy(v => v.Status)
                            .OrderByDescending(g => g.Count())
                            .FirstOrDefault()?.Key;

                        if (!string.IsNullOrEmpty(statusMaisComum))
                        {
                            statusOriginalPredominante = statusMaisComum;
                        }
                    }

                    // =====================================================
                    // COBERTURA
                    // =====================================================
                    var coberturaExistente = await db.CoberturaFolga
                        .FirstOrDefaultAsync(c => c.MotoristaFolgaId == motoristaId && c.Ativo);

                    // Variáveis para controle de troca de cobertor
                    Guid? cobertorAntigoId = null;
                    DateTime dataInicioNovoCobertor = dataInicio;

                    if (coberturaExistente != null)
                    {
                        // Verificar se houve troca de motorista cobertor
                        if (model.MotoristaCobertorId.HasValue && 
                            model.MotoristaCobertorId.Value != Guid.Empty &&
                            coberturaExistente.MotoristaCoberturaId != model.MotoristaCobertorId.Value)
                        {
                            houveTrocaDeCobertor = true;
                            cobertorAntigoId = coberturaExistente.MotoristaCoberturaId;
                            dataInicioNovoCobertor = DateTime.Today; // Novo cobertor assume a partir de hoje

                            // Se a troca é no primeiro dia do período, desativa a cobertura antiga
                            if (DateTime.Today <= coberturaExistente.DataInicio)
                            {
                                coberturaExistente.Ativo = false;
                                coberturaExistente.DataAlteracao = DateTime.Now;
                                coberturaExistente.UsuarioIdAlteracao = usuarioId;
                                coberturaExistente.Observacoes = $"Substituído por outro cobertor em {DateTime.Now:dd/MM/yyyy}";
                                db.CoberturaFolga.Update(coberturaExistente);
                            }
                            else
                            {
                                // Ajusta a cobertura antiga para terminar ontem
                                coberturaExistente.DataFim = DateTime.Today.AddDays(-1);
                                coberturaExistente.DataAlteracao = DateTime.Now;
                                coberturaExistente.UsuarioIdAlteracao = usuarioId;
                                coberturaExistente.Observacoes = $"Período ajustado - substituído por outro cobertor em {DateTime.Now:dd/MM/yyyy}";
                                db.CoberturaFolga.Update(coberturaExistente);
                            }
                            await db.SaveChangesAsync();

                            // Criar nova cobertura para o novo motorista
                            var novaCobertura = new CoberturaFolga
                            {
                                CoberturaId = Guid.NewGuid(),
                                MotoristaFolgaId = motoristaId,
                                MotoristaCoberturaId = model.MotoristaCobertorId.Value,
                                DataInicio = dataInicioNovoCobertor,
                                DataFim = dataFim,
                                Motivo = model.CategoriaIndisponibilidade,
                                StatusOriginal = statusOriginalPredominante,
                                Observacoes = $"Substituiu cobertor anterior a partir de {dataInicioNovoCobertor:dd/MM/yyyy}",
                                Ativo = true,
                                DataCriacao = DateTime.Now,
                                UsuarioIdAlteracao = usuarioId
                            };
                            db.CoberturaFolga.Add(novaCobertura);
                            await db.SaveChangesAsync();

                            // =====================================================
                            // EXCLUIR ESCALAS DO COBERTOR ANTIGO (a partir de hoje)
                            // =====================================================
                            if (cobertorAntigoId.HasValue)
                            {
                                var associacoesCobertorAntigo = await db.VAssociado
                                    .Where(a => a.MotoristaId == cobertorAntigoId.Value)
                                    .Select(a => a.AssociacaoId)
                                    .ToListAsync();

                                if (associacoesCobertorAntigo.Any())
                                {
                                    var escalasParaExcluir = await db.EscalaDiaria
                                        .Where(e => e.AssociacaoId.HasValue &&
                                                    associacoesCobertorAntigo.Contains(e.AssociacaoId.Value) &&
                                                    e.DataEscala >= dataInicioNovoCobertor &&
                                                    e.DataEscala <= dataFim &&
                                                    e.Ativo &&
                                                    e.Observacoes != null && 
                                                    e.Observacoes.Contains("Cobertura"))
                                        .ToListAsync();

                                    foreach (var escalaExcluir in escalasParaExcluir)
                                    {
                                        db.EscalaDiaria.Remove(escalaExcluir);
                                    }

                                    if (escalasParaExcluir.Any())
                                    {
                                        await db.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Não houve troca, apenas atualiza a cobertura existente
                            coberturaExistente.DataInicio = dataInicio;
                            coberturaExistente.DataFim = dataFim;
                            coberturaExistente.Motivo = model.CategoriaIndisponibilidade;
                            coberturaExistente.DataAlteracao = DateTime.Now;
                            coberturaExistente.UsuarioIdAlteracao = usuarioId;
                            coberturaExistente.StatusOriginal = statusOriginalPredominante;

                            if (model.MotoristaCobertorId.HasValue && model.MotoristaCobertorId.Value != Guid.Empty)
                            {
                                coberturaExistente.MotoristaCoberturaId = model.MotoristaCobertorId.Value;
                            }

                            db.CoberturaFolga.Update(coberturaExistente);
                            await db.SaveChangesAsync();
                        }
                    }
                    else if (model.MotoristaCobertorId.HasValue && model.MotoristaCobertorId.Value != Guid.Empty)
                    {
                        var novaCobertura = new CoberturaFolga
                        {
                            CoberturaId = Guid.NewGuid(),
                            MotoristaFolgaId = motoristaId,
                            MotoristaCoberturaId = model.MotoristaCobertorId.Value,
                            DataInicio = dataInicio,
                            DataFim = dataFim,
                            Motivo = model.CategoriaIndisponibilidade,
                            StatusOriginal = statusOriginalPredominante,
                            Observacoes = "Criado via edição de escala",
                            Ativo = true,
                            DataCriacao = DateTime.Now,
                            UsuarioIdAlteracao = usuarioId
                        };
                        db.CoberturaFolga.Add(novaCobertura);
                        await db.SaveChangesAsync();
                    }

                    // =====================================================
                    // CLONAGEM DE ESCALAS PARA O COBERTOR
                    // =====================================================
                    if (model.MotoristaCobertorId.HasValue && model.MotoristaCobertorId.Value != Guid.Empty)
                    {
                        var motoristaCobertorId = model.MotoristaCobertorId.Value;

                        var associacoesCobertor = await db.VAssociado
                            .Where(a => a.MotoristaId == motoristaCobertorId)
                            .Select(a => a.AssociacaoId)
                            .ToListAsync();

                        Guid? associacaoCobertorId = null;

                        if (model.VeiculoId.HasValue && model.VeiculoId.Value != Guid.Empty)
                        {
                            var assocExistente = await db.VAssociado.FirstOrDefaultAsync(
                                a => a.MotoristaId == motoristaCobertorId &&
                                     a.VeiculoId == model.VeiculoId.Value &&
                                     a.Ativo);

                            if (assocExistente != null)
                            {
                                associacaoCobertorId = assocExistente.AssociacaoId;
                            }
                            else
                            {
                                var novaAssoc = new VAssociado
                                {
                                    AssociacaoId = Guid.NewGuid(),
                                    MotoristaId = motoristaCobertorId,
                                    VeiculoId = model.VeiculoId.Value,
                                    DataInicio = houveTrocaDeCobertor ? dataInicioNovoCobertor : dataInicio,
                                    Ativo = true,
                                    DataCriacao = DateTime.Now,
                                    UsuarioIdAlteracao = usuarioId
                                };
                                db.VAssociado.Add(novaAssoc);
                                await db.SaveChangesAsync();
                                associacaoCobertorId = novaAssoc.AssociacaoId;
                                associacoesCobertor.Add(novaAssoc.AssociacaoId);
                            }
                        }
                        else
                        {
                            var assocQualquer = await db.VAssociado
                                .FirstOrDefaultAsync(a => a.MotoristaId == motoristaCobertorId && a.Ativo);
                            associacaoCobertorId = assocQualquer?.AssociacaoId;
                        }

                        // Se houve troca de cobertor, criar escalas a partir de hoje
                        // Senão, criar a partir do início do período
                        DateTime dataInicioEscalas = houveTrocaDeCobertor ? dataInicioNovoCobertor : dataInicio;

                        // =====================================================
                        // DETECTAR DIAS DA SEMANA DO MOTORISTA ORIGINAL
                        // =====================================================
                        // Buscar os dias da semana que o motorista original tinha escalas
                        var diasSemanaMotoristaOriginal = new HashSet<DayOfWeek>();
                        
                        // Usar statusPorDia que já foi preenchido com as escalas originais
                        foreach (var kvp in statusPorDia)
                        {
                            diasSemanaMotoristaOriginal.Add(kvp.Key.DayOfWeek);
                        }

                        // Se não encontrou nenhum dia (caso raro), usar os dias do request
                        if (!diasSemanaMotoristaOriginal.Any())
                        {
                            if (model.Segunda) diasSemanaMotoristaOriginal.Add(DayOfWeek.Monday);
                            if (model.Terca) diasSemanaMotoristaOriginal.Add(DayOfWeek.Tuesday);
                            if (model.Quarta) diasSemanaMotoristaOriginal.Add(DayOfWeek.Wednesday);
                            if (model.Quinta) diasSemanaMotoristaOriginal.Add(DayOfWeek.Thursday);
                            if (model.Sexta) diasSemanaMotoristaOriginal.Add(DayOfWeek.Friday);
                            if (model.Sabado) diasSemanaMotoristaOriginal.Add(DayOfWeek.Saturday);
                            if (model.Domingo) diasSemanaMotoristaOriginal.Add(DayOfWeek.Sunday);
                        }

                        for (var data = dataInicioEscalas; data <= dataFim; data = data.AddDays(1))
                        {
                            // =====================================================
                            // VERIFICAR SE O DIA DA SEMANA ESTÁ NOS DIAS DO MOTORISTA ORIGINAL
                            // =====================================================
                            if (!diasSemanaMotoristaOriginal.Contains(data.DayOfWeek))
                            {
                                // Motorista original não trabalhava neste dia, cobertor também não trabalha
                                continue;
                            }

                            bool cobertorJaTemEscala = await db.EscalaDiaria.AnyAsync(
                                e => e.AssociacaoId.HasValue &&
                                     associacoesCobertor.Contains(e.AssociacaoId.Value) &&
                                     e.DataEscala == data &&
                                     e.Ativo);

                            if (!cobertorJaTemEscala && associacaoCobertorId.HasValue)
                            {
                                // =====================================================
                                // HERDAR STATUS ORIGINAL DO DIA
                                // =====================================================
                                string statusCobertor = "Disponível";
                                Guid? requisitanteCobertor = null;
                                string lotacaoCobertor = "";

                                if (statusPorDia.TryGetValue(data, out var infoOriginal))
                                {
                                    statusCobertor = infoOriginal.Status;

                                    if (infoOriginal.Status == "Em Serviço" && infoOriginal.RequisitanteId.HasValue)
                                    {
                                        requisitanteCobertor = infoOriginal.RequisitanteId;
                                    }

                                    if (infoOriginal.Status == "Economildo" && !string.IsNullOrEmpty(infoOriginal.Lotacao))
                                    {
                                        lotacaoCobertor = infoOriginal.Lotacao;
                                    }
                                }

                                var novaEscala = new EscalaDiaria
                                {
                                    EscalaDiaId = Guid.NewGuid(),
                                    AssociacaoId = associacaoCobertorId,
                                    TipoServicoId = model.TipoServicoId,
                                    TurnoId = model.TurnoId,
                                    DataEscala = data,
                                    HoraInicio = horaInicio,
                                    HoraFim = horaFim,
                                    HoraIntervaloInicio = horaIntervaloInicio,
                                    HoraIntervaloFim = horaIntervaloFim,
                                    Lotacao = lotacaoCobertor,
                                    NumeroSaidas = 0,
                                    StatusMotorista = statusCobertor,
                                    RequisitanteId = requisitanteCobertor,
                                    Observacoes = $"Cobertura ({model.CategoriaIndisponibilidade})",
                                    Ativo = true,
                                    DataCriacao = DateTime.Now,
                                    UsuarioIdAlteracao = usuarioId
                                };

                                db.EscalaDiaria.Add(novaEscala);
                                escalasCriadas++;
                            }
                        }

                        if (escalasCriadas > 0)
                        {
                            await db.SaveChangesAsync();
                        }
                    }
                }

                // =====================================================
                // PROCESSAMENTO DE DIAS DA SEMANA (escalas futuras)
                // =====================================================
                int escalasExcluidas = 0;
                int escalasCriadasDias = 0;

                if (model.MotoristaId.HasValue)
                {
                    var motoristaId = model.MotoristaId.Value;
                    
                    // Obter associações do motorista
                    var associacoesMotorista = await db.VAssociado
                        .Where(a => a.MotoristaId == motoristaId)
                        .Select(a => a.AssociacaoId)
                        .ToListAsync();

                    if (associacoesMotorista.Any())
                    {
                        // Mapear dias da semana do request
                        var diasMarcados = new HashSet<DayOfWeek>();
                        if (model.Segunda) diasMarcados.Add(DayOfWeek.Monday);
                        if (model.Terca) diasMarcados.Add(DayOfWeek.Tuesday);
                        if (model.Quarta) diasMarcados.Add(DayOfWeek.Wednesday);
                        if (model.Quinta) diasMarcados.Add(DayOfWeek.Thursday);
                        if (model.Sexta) diasMarcados.Add(DayOfWeek.Friday);
                        if (model.Sabado) diasMarcados.Add(DayOfWeek.Saturday);
                        if (model.Domingo) diasMarcados.Add(DayOfWeek.Sunday);

                        // Buscar escalas futuras (a partir de amanhã para não afetar a escala atual)
                        var amanha = DateTime.Today.AddDays(1);
                        
                        // Buscar IDs das escalas futuras - filtrar no banco primeiro, depois no cliente
                        var escalasFuturas = await db.EscalaDiaria
                            .AsNoTracking()
                            .Where(e => e.AssociacaoId.HasValue &&
                                       associacoesMotorista.Contains(e.AssociacaoId.Value) &&
                                       e.DataEscala >= amanha &&
                                       e.Ativo &&
                                       e.StatusMotorista != "Indisponível" &&
                                       e.EscalaDiaId != model.EscalaDiaId)
                            .Select(e => new { e.EscalaDiaId, e.DataEscala })
                            .ToListAsync();

                        // Filtrar dias desmarcados no cliente (DayOfWeek não traduz para SQL)
                        var idsParaExcluir = escalasFuturas
                            .Where(e => !diasMarcados.Contains(e.DataEscala.DayOfWeek))
                            .Select(e => e.EscalaDiaId)
                            .ToList();

                        // Excluir escalas de dias desmarcados
                        if (idsParaExcluir.Any())
                        {
                            foreach (var idExcluir in idsParaExcluir)
                            {
                                var escalaParaExcluir = new EscalaDiaria { EscalaDiaId = idExcluir };
                                db.Entry(escalaParaExcluir).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                                escalasExcluidas++;
                            }
                            await db.SaveChangesAsync();
                        }

                        // Criar escalas para dias marcados que não existem (próximos 30 dias)
                        var dataLimite = DateTime.Today.AddDays(30);
                        var associacaoId = escala.AssociacaoId;

                        for (var data = amanha; data <= dataLimite; data = data.AddDays(1))
                        {
                            if (diasMarcados.Contains(data.DayOfWeek))
                            {
                                // Verificar se já existe escala neste dia
                                bool jaExiste = await db.EscalaDiaria.AnyAsync(
                                    e => e.AssociacaoId.HasValue &&
                                         associacoesMotorista.Contains(e.AssociacaoId.Value) &&
                                         e.DataEscala == data &&
                                         e.Ativo);

                                if (!jaExiste && associacaoId.HasValue)
                                {
                                    var novaEscala = new EscalaDiaria
                                    {
                                        EscalaDiaId = Guid.NewGuid(),
                                        AssociacaoId = associacaoId,
                                        TipoServicoId = model.TipoServicoId,
                                        TurnoId = model.TurnoId,
                                        DataEscala = data,
                                        HoraInicio = horaInicio,
                                        HoraFim = horaFim,
                                        HoraIntervaloInicio = horaIntervaloInicio,
                                        HoraIntervaloFim = horaIntervaloFim,
                                        Lotacao = statusFinal == "Economildo" ? model.Lotacao : null,
                                        NumeroSaidas = 0,
                                        StatusMotorista = statusFinal,
                                        RequisitanteId = statusFinal == "Em Serviço" ? model.RequisitanteId : null,
                                        Observacoes = "Criada via alteração de dias da semana",
                                        Ativo = true,
                                        DataCriacao = DateTime.Now,
                                        UsuarioIdAlteracao = usuarioId
                                    };

                                    db.EscalaDiaria.Add(novaEscala);
                                    escalasCriadasDias++;
                                }
                            }
                        }

                        if (escalasCriadasDias > 0)
                        {
                            await db.SaveChangesAsync();
                        }
                    }
                }

                // =====================================================
                // RETORNO
                // =====================================================
                string mensagem = "Escala atualizada com sucesso!";
                if (escalasAtualizadas > 0) mensagem += $" {escalasAtualizadas} outra(s) escala(s) marcada(s) como indisponível.";
                if (escalasCriadas > 0) mensagem += $" {escalasCriadas} escala(s) criada(s) para o cobertor.";
                if (houveTrocaDeCobertor) mensagem += " Motorista cobertor substituído a partir de hoje.";
                if (escalasExcluidas > 0) mensagem += $" {escalasExcluidas} escala(s) futura(s) removida(s).";
                if (escalasCriadasDias > 0) mensagem += $" {escalasCriadasDias} escala(s) futura(s) criada(s).";

                return Json(new { success = true, message = mensagem });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Erro ao editar escala");
                Alerta.TratamentoErroComLinha("EscalaController_Api.cs", "EditEscala", ex);
                return Json(new { success = false, message = "Erro ao atualizar escala: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("api/Escala/TesteRota")]
        public IActionResult TesteRota()
        {
            return Json(new { success = true, message = "Rota funcionando!", timestamp = DateTime.Now });
        }


        [HttpGet]
        [Route("api/Escala/GetEscalaDetalhes")]
        public IActionResult GetEscalaDetalhes(Guid id)
        {
            try
            {
                var escala = _unitOfWork.ViewEscalasCompletas
                    .GetFirstOrDefault(e => e.EscalaDiaId == id);

                if (escala == null)
                    return Json(new { success = false, message = "Escala não encontrada" });

                // Converter foto para Base64
                string fotoBase64 = "";
                if (escala.Foto != null && escala.Foto.Length > 0)
                    fotoBase64 = Convert.ToBase64String(escala.Foto);

                // Verificar se este motorista está COBRINDO alguém
                string nomeMotoristaCobrindo = "";
                string motivoCoberturaCobrindo = "";
                
                if (escala.MotoristaId.HasValue)
                {
                    var db = HttpContext.RequestServices.GetService(typeof(FrotiXDbContext)) as FrotiXDbContext;
                    
                    if (db != null)
                    {
                        // Buscar cobertura onde este motorista é o COBERTOR
                        var coberturaComoCobertor = db.CoberturaFolga
                            .FirstOrDefault(c => c.MotoristaCoberturaId == escala.MotoristaId.Value &&
                                                 c.DataInicio <= escala.DataEscala &&
                                                 c.DataFim >= escala.DataEscala &&
                                                 c.Ativo);
                        
                        if (coberturaComoCobertor != null)
                        {
                            // Buscar nome do motorista que está sendo coberto
                            var motoristaCoberto = db.Motorista
                                .FirstOrDefault(m => m.MotoristaId == coberturaComoCobertor.MotoristaFolgaId);
                            
                            if (motoristaCoberto != null)
                            {
                                nomeMotoristaCobrindo = motoristaCoberto.Nome ?? "";
                                motivoCoberturaCobrindo = coberturaComoCobertor.Motivo ?? "";
                            }
                        }
                    }
                }

                var resultado = new
                {
                    escalaDiaId = escala.EscalaDiaId,
                    dataEscala = escala.DataEscala,
                    horaInicio = escala.HoraInicio,
                    horaFim = escala.HoraFim,
                    nomeTurno = escala.NomeTurno,
                    nomeServico = escala.NomeServico,
                    nomeMotorista = escala.NomeMotorista,
                    placa = escala.VeiculoDescricao,
                    lotacao = escala.Lotacao,
                    numeroSaidas = escala.NumeroSaidas,
                    statusMotorista = escala.StatusMotorista,
                    nomeRequisitante = escala.NomeRequisitante,
                    observacoes = escala.Observacoes,
                    fotoBase64 = fotoBase64,
                    nomeMotoristaCobertor = escala.NomeMotoristaCobertor ?? "",
                    motivoCobertura = escala.MotivoCobertura ?? "",
                    // Novos campos para quando este motorista é o COBERTOR
                    nomeMotoristaCobrindo = nomeMotoristaCobrindo,
                    motivoCoberturaCobrindo = motivoCoberturaCobrindo
                };

                return Json(new { success = true, data = resultado });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("EscalaController.cs", "GetEscalaDetalhes", error);
                return Json(new { success = false, message = error.Message });
            }
        }

        // ===================================================================
        // CLASSES DE REQUEST
        // ===================================================================

        /// <summary>
        /// Classe para receber dados de edição de escala via API
        /// </summary>
        public class EditEscalaRequest
        {
            public Guid EscalaDiaId { get; set; }
            public Guid? MotoristaId { get; set; }
            public Guid? VeiculoId { get; set; }
            public Guid TipoServicoId { get; set; }
            public Guid TurnoId { get; set; }
            public DateTime DataEscala { get; set; }
            public string HoraInicio { get; set; } = "06:00";
            public string HoraFim { get; set; } = "18:00";
            public string? HoraIntervaloInicio { get; set; }
            public string? HoraIntervaloFim { get; set; }
            public string? Lotacao { get; set; }
            public int NumeroSaidas { get; set; }
            public Guid? RequisitanteId { get; set; }
            public string? Observacoes { get; set; }

            // Status checkboxes
            public bool MotoristaIndisponivel { get; set; }
            public bool MotoristaEconomildo { get; set; }
            public bool MotoristaEmServico { get; set; }
            public bool MotoristaReservado { get; set; }

            // =====================================================
            // CAMPOS DE INDISPONIBILIDADE 
            // =====================================================
            public string? CategoriaIndisponibilidade { get; set; }
            public DateTime? DataInicioIndisponibilidade { get; set; }
            public DateTime? DataFimIndisponibilidade { get; set; }
            public Guid? MotoristaCobertorId { get; set; }

            // =====================================================
            // DIAS DA SEMANA
            // =====================================================
            public bool Segunda { get; set; }
            public bool Terca { get; set; }
            public bool Quarta { get; set; }
            public bool Quinta { get; set; }
            public bool Sexta { get; set; }
            public bool Sabado { get; set; }
            public bool Domingo { get; set; }
        }

        /// <summary>
        /// Classe para receber filtros via JSON
        /// </summary>
        public class FiltroEscalaRequest
        {
            [System.Text.Json.Serialization.JsonPropertyName("dataFiltro")]
            public DateTime? DataFiltro { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("tipoServicoId")]
            public Guid? TipoServicoId { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("turnoId")]
            public Guid? TurnoId { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("motoristaId")]
            public Guid? MotoristaId { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("veiculoId")]
            public Guid? VeiculoId { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("statusMotorista")]
            public string? StatusMotorista { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("lotacao")]
            public string? Lotacao { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("textoPesquisa")]
            public string? TextoPesquisa { get; set; }
        }

        // =====================================================
        // OBSERVAÇÕES DO DIA
        // =====================================================

        /// <summary>
        /// Salva uma nova observação do dia
        /// </summary>
        [HttpPost]
        [Route("api/Escala/SalvarObservacaoDia")]
        public async Task<IActionResult> SalvarObservacaoDia([FromBody] ObservacaoDiaRequest model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Titulo))
                {
                    return Json(new { success = false, message = "Título é obrigatório" });
                }

                if (!model.ExibirDe.HasValue || !model.ExibirAte.HasValue)
                {
                    return Json(new { success = false, message = "Datas de exibição são obrigatórias" });
                }

                var db = HttpContext.RequestServices.GetService(typeof(FrotiXDbContext)) as FrotiXDbContext;

                if (db == null)
                {
                    return Json(new { success = false, message = "Erro interno: DbContext não disponível" });
                }

                var observacao = new FrotiX.Models.ObservacoesEscala
                {
                    ObservacaoId = Guid.NewGuid(),
                    Titulo = model.Titulo,
                    Descricao = model.Descricao,
                    Prioridade = model.Prioridade ?? "Normal",
                    ExibirDe = model.ExibirDe.Value,
                    ExibirAte = model.ExibirAte.Value,
                    Ativo = true,
                    DataCriacao = DateTime.Now,
                    UsuarioIdAlteracao = User?.Identity?.Name ?? "Sistema"
                };

                db.Set<FrotiX.Models.ObservacoesEscala>().Add(observacao);
                await db.SaveChangesAsync();

                return Json(new { success = true, message = "Observação salva com sucesso!" });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EscalaController_Api.cs", "SalvarObservacaoDia", ex);
                return Json(new { success = false, message = "Erro ao salvar observação: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtém observações ativas para uma data específica
        /// </summary>
        [HttpGet]
        [Route("api/Escala/GetObservacoesDia")]
        public IActionResult GetObservacoesDia(DateTime? data)
        {
            try
            {
                var db = HttpContext.RequestServices.GetService(typeof(FrotiXDbContext)) as FrotiXDbContext;

                if (db == null)
                {
                    return Json(new { success = false, message = "Erro interno: DbContext não disponível" });
                }

                var dataFiltro = data?.Date ?? DateTime.Today;

                // Buscar observações onde a data está dentro do período ExibirDe - ExibirAte
                var observacoes = db.Set<FrotiX.Models.ObservacoesEscala>()
                    .Where(o => o.Ativo &&
                                o.ExibirDe.Date <= dataFiltro &&
                                o.ExibirAte.Date >= dataFiltro)
                    .OrderByDescending(o => o.Prioridade == "Alta")
                    .ThenByDescending(o => o.Prioridade == "Normal")
                    .ThenByDescending(o => o.DataCriacao)
                    .ToList()
                    .Select(o => new
                    {
                        observacaoId = o.ObservacaoId.ToString(),
                        titulo = o.Titulo,
                        descricao = o.Descricao,
                        prioridade = o.Prioridade,
                        exibirDe = o.ExibirDe.ToString("dd/MM/yyyy"),
                        exibirAte = o.ExibirAte.ToString("dd/MM/yyyy")
                    })
                    .ToList();

                return Json(new { success = true, data = observacoes });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EscalaController_Api.cs", "GetObservacoesDia", ex);
                return Json(new { success = false, message = "Erro ao buscar observações: " + ex.Message });
            }
        }

        /// <summary>
        /// Exclui uma observação (remoção física)
        /// </summary>
        [HttpPost]
        [Route("api/Escala/ExcluirObservacaoDia")]
        public async Task<IActionResult> ExcluirObservacaoDia([FromBody] ExcluirObservacaoRequest model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.ObservacaoId))
                {
                    return Json(new { success = false, message = "ID da observação é obrigatório" });
                }

                if (!Guid.TryParse(model.ObservacaoId, out var observacaoGuid))
                {
                    return Json(new { success = false, message = "ID da observação inválido" });
                }

                var db = HttpContext.RequestServices.GetService(typeof(FrotiXDbContext)) as FrotiXDbContext;

                if (db == null)
                {
                    return Json(new { success = false, message = "Erro interno: DbContext não disponível" });
                }

                var observacao = await db.Set<FrotiX.Models.ObservacoesEscala>()
                    .FirstOrDefaultAsync(o => o.ObservacaoId == observacaoGuid);

                if (observacao == null)
                {
                    return Json(new { success = false, message = "Observação não encontrada" });
                }

                // Remoção física
                db.Set<FrotiX.Models.ObservacoesEscala>().Remove(observacao);
                await db.SaveChangesAsync();

                return Json(new { success = true, message = "Observação excluída com sucesso!" });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EscalaController_Api.cs", "ExcluirObservacaoDia", ex);
                return Json(new { success = false, message = "Erro ao excluir observação: " + ex.Message });
            }
        }

        /// <summary>
        /// Request para salvar observação
        /// </summary>
        public class ObservacaoDiaRequest
        {
            public string? Titulo { get; set; }
            public string? Descricao { get; set; }
            public string? Prioridade { get; set; }
            public DateTime? ExibirDe { get; set; }
            public DateTime? ExibirAte { get; set; }
        }

        /// <summary>
        /// Request para excluir observação
        /// </summary>
        public class ExcluirObservacaoRequest
        {
            public string? ObservacaoId { get; set; }
        }
    }
}
