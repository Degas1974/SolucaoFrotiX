using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Models.DTO;
using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrotiX.Helpers;

namespace FrotiX.Controllers
{
/*
 * ╭─────────────────────────────────────────────────────────────────────────────────────────────╮
 * │                                     FROTIX SOLUÇÕES EM GESTÃO                               │
 * ├─────────────────────────────────────────────────────────────────────────────────────────────┤
 * │                                                                                             │
 * │ 📝 ARQUIVO: AgendaController.cs                                                             │
 * │ 📍 LOCAL: Controllers                                                                       │
 * │ ❓ DESCRIÇÃO: Gerencia o calendário de viagens, agendamentos recorrentes e                  │
 * │              verificação de conflitos. Centraliza a lógica de criação                      │
 * │              e edição de viagens agendadas.                                               │
 * │ 🔗 RELEVÂNCIA: Alta (Core do módulo de Agendamento/Calendário)                             │
 * │                                                                                             │
 * ├─────────────────────────────────────────────────────────────────────────────────────────────┤
 * │                                     © 2026 FrotiX Soluções                                  │
 * ╰─────────────────────────────────────────────────────────────────────────────────────────────╯
 */

[Route("api/[controller]")]
    [ApiController]
    public class AgendaController : ControllerBase
    {
        private readonly FrotiXDbContext _context;
        private readonly ILogger<AgendaController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ViagemEstatisticaService _viagemEstatisticaService;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AgendaController (Constructor)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador de agenda com dependências de dados, log e       ║
        /// ║    serviço de estatísticas de viagem.                                        ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Centraliza o acesso a dados e métricas do módulo de agendamento.          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • logger (ILogger<AgendaController>): logger de rastreio.                 ║
        /// ║    • hostingEnvironment (IWebHostEnvironment): ambiente/paths.               ║
        /// ║    • unitOfWork (IUnitOfWork): repositórios de dados.                         ║
        /// ║    • context (FrotiXDbContext): contexto EF Core.                             ║
        /// ║    • viagemEstatisticaRepository (IViagemEstatisticaRepository): estatísticas.║
        /// ║    • logService (ILogService): serviço de log centralizado.                   ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Tipo: N/A                                                               ║
        /// ║    • Significado: N/A                                                        ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • new ViagemEstatisticaService() → inicializa serviço de métricas.        ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Injeção de dependência ao instanciar o controller.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Program.cs                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public AgendaController(
            ILogger<AgendaController> logger,
            IWebHostEnvironment hostingEnvironment,
            IUnitOfWork unitOfWork,
            FrotiXDbContext context,
            IViagemEstatisticaRepository viagemEstatisticaRepository,
            ILogService logService
        )
        {
            try
            {
                _logger = logger;
                _hostingEnvironment = hostingEnvironment;
                _unitOfWork = unitOfWork;
                _context = context;
                _viagemEstatisticaService = new ViagemEstatisticaService(context, viagemEstatisticaRepository, unitOfWork);
                _log = logService;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AgendaController.cs", "AgendaController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: TesteView                                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Verifica acesso à ViewViagensAgenda e retorna amostra de registros.       ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Diagnóstico rápido para validar view e conectividade do módulo.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com total e primeiro registro.                       ║
        /// ║    • Consumidor: Suporte/diagnóstico de agenda.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _context.ViewViagensAgenda → consultas EF Core.                          ║
        /// ║    • _logger.LogInformation() → telemetria.                                   ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Agenda/TesteView                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Diagnóstico                                              ║
        /// ║    • Arquivos relacionados: Views/Agenda/*.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("TesteView")]
        public IActionResult TesteView()
        {
            try
            {
                // [DEBUG] Logs de diagnóstico
                _logger.LogInformation("[TesteView] Testando acesso à ViewViagensAgenda");

                // [DADOS] Contagem de registros da view
                var count = _context.ViewViagensAgenda.Count();

                _logger.LogInformation($"[TesteView] Total de registros na view: {count}");

                // [DADOS] Amostra do primeiro registro
                var primeiro = _context.ViewViagensAgenda
                    .AsNoTracking()
                    .FirstOrDefault();

                // [DADOS] Payload de retorno
                return Ok(new
                {
                    totalRegistros = count,
                    primeiroRegistro = primeiro != null ? new
                    {
                        viagemId = primeiro.ViagemId,
                        titulo = primeiro.Titulo,
                        dataInicial = primeiro.DataInicial,
                        start = primeiro.Start,
                        corEvento = primeiro.CorEvento,
                        corTexto = primeiro.CorTexto
                    } : null,
                    mensagem = "Teste concluído com sucesso"
                });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao testar view de agenda", ex, "AgendaController.cs", "TesteView");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "TesteView", ex);
                return StatusCode(500, new
                {
                    sucesso = false,
                    mensagem = "Erro no teste",
                    erro = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DiagnosticoAgenda                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Executa diagnóstico detalhado do carregamento da agenda por período.      ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Auxilia na análise de timezone e volume de registros.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • start (DateTime?): início do período.                                   ║
        /// ║    • end (DateTime?): fim do período.                                        ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com contadores e amostras.                           ║
        /// ║    • Consumidor: Suporte/diagnóstico de agenda.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _context.ViewViagensAgenda → consultas EF Core.                          ║
        /// ║    • _logger.LogInformation() → telemetria.                                   ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/Agenda/DiagnosticoAgenda                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Diagnóstico                                              ║
        /// ║    • Arquivos relacionados: Views/Agenda/*.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("DiagnosticoAgenda")]
        public IActionResult DiagnosticoAgenda(DateTime? start = null, DateTime? end = null)
        {
            try
            {
                // [REGRA] Período padrão quando não informado
                if (!start.HasValue)
                {
                    start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }
                if (!end.HasValue)
                {
                    end = start.Value.AddMonths(1);
                }

                // [DEBUG] Logs de período
                _logger.LogInformation($"[DiagnosticoAgenda] Período solicitado: {start} até {end}");

                // [LOGICA] Ajuste de timezone (-3)
                DateTime startMenos3 = start.Value.AddHours(-3);
                DateTime endMenos3 = end.Value.AddHours(-3);

                _logger.LogInformation($"[DiagnosticoAgenda] Período ajustado (timezone): {startMenos3} até {endMenos3}");

                // [DADOS] Contadores gerais
                var totalGeral = _context.ViewViagensAgenda.Count();

                var totalComData = _context.ViewViagensAgenda
                    .Count(v => v.DataInicial.HasValue);

                // [DADOS] Registros no período sem ajuste
                var noPeriodoSemAjuste = _context.ViewViagensAgenda
                    .Where(v => v.DataInicial.HasValue
                        && v.DataInicial >= start
                        && v.DataInicial < end)
                    .ToList();

                // [DADOS] Registros no período com ajuste
                var noPeriodoComAjuste = _context.ViewViagensAgenda
                    .AsNoTracking()
                    .Where(v => v.DataInicial.HasValue
                        && v.DataInicial >= startMenos3
                        && v.DataInicial < endMenos3)
                    .ToList();

                // [DADOS] Amostras recentes
                var primeiros5 = _context.ViewViagensAgenda
                    .AsNoTracking()
                    .Where(v => v.DataInicial.HasValue)
                    .OrderByDescending(v => v.DataInicial)
                    .Take(5)
                    .Select(v => new
                    {
                        v.ViagemId,
                        v.Titulo,
                        v.DataInicial,
                        v.Start,
                        v.CorEvento,
                        v.Status
                    })
                    .ToList();

                return Ok(new
                {
                    periodoSolicitado = new
                    {
                        inicio = start,
                        fim = end
                    },
                    periodoAjustado = new
                    {
                        inicio = startMenos3,
                        fim = endMenos3,
                        observacao = "Período com ajuste de -3 horas (timezone)"
                    },
                    contadores = new
                    {
                        totalRegistrosNaView = totalGeral,
                        totalComDataInicial = totalComData,
                        noPeriodoSemAjuste = noPeriodoSemAjuste.Count,
                        noPeriodoComAjuste = noPeriodoComAjuste.Count
                    },
                    amostras = new
                    {
                        primeiros5RegistrosDaView = primeiros5,
                        registrosNoPeriodoSemAjuste = noPeriodoSemAjuste.Take(3).Select(v => new
                        {
                            v.ViagemId,
                            v.Titulo,
                            v.DataInicial,
                            v.Start
                        }),
                        registrosNoPeriodoComAjuste = noPeriodoComAjuste.Take(3).Select(v => new
                        {
                            v.ViagemId,
                            v.Titulo,
                            v.DataInicial,
                            v.Start
                        })
                    },
                    mensagem = "Diagnóstico concluído"
                });
            }
            catch (Exception ex)
            {
                _log.Error("Erro no diagnóstico de agenda", ex, "AgendaController.cs", "DiagnosticoAgenda");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "DiagnosticoAgenda", ex);
                return StatusCode(500, new
                {
                    sucesso = false,
                    mensagem = "Erro no diagnóstico",
                    erro = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: TesteCarregaViagens (GET)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Testa carregamento de viagens com lógica idêntica ao FullCalendar.       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • start (DateTime), end (DateTime)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com eventos simulados.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("TesteCarregaViagens")]
        public IActionResult TesteCarregaViagens(DateTime start, DateTime end)
        {
            try
            {
                _logger.LogInformation($"[TesteCarregaViagens] INÍCIO - Período: {start} até {end}");

                DateTime startMenos3 = start.AddHours(-3);
                DateTime endMenos3 = end.AddHours(-3);

                _logger.LogInformation($"[TesteCarregaViagens] Período ajustado: {startMenos3} até {endMenos3}");

                var viagensRaw = _context.ViewViagensAgenda
                    .AsNoTracking()
                    .Where(v => v.DataInicial.HasValue
                        && v.DataInicial >= startMenos3
                        && v.DataInicial < endMenos3)
                    .ToList();

                _logger.LogInformation($"[TesteCarregaViagens] Registros encontrados: {viagensRaw.Count}");

                var viagens = viagensRaw
                    .Select(v =>
                    {
                        var startDate = v.Start ?? v.DataInicial ?? DateTime.Now;
                        var endDate = startDate.AddHours(1);

                        return new
                        {
                            id = v.ViagemId,
                            title = v.Titulo ?? "Viagem",
                            start = startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                            end = endDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                            backgroundColor = v.CorEvento ?? "#808080",
                            textColor = v.CorTexto ?? "#FFFFFF",
                            descricao = v.Descricao ?? "",
                            placa = v.Placa,
                            motorista = v.NomeMotorista,
                            evento = v.NomeEventoFull,
                            finalidade = v.Finalidade
                        };
                    })
                    .ToList();

                _logger.LogInformation($"[TesteCarregaViagens] Viagens processadas: {viagens.Count}");

                return Ok(new
                {
                    data = viagens,
                    debug = new
                    {
                        periodoOriginal = new { start, end },
                        periodoAjustado = new { startMenos3, endMenos3 },
                        totalEncontrado = viagensRaw.Count,
                        totalProcessado = viagens.Count,
                        primeiros3 = viagens.Take(3)
                    }
                });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao testar carregamento de viagens", ex, "AgendaController.cs", "TesteCarregaViagens");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "TesteCarregaViagens", ex);
                return StatusCode(500, new
                {
                    sucesso = false,
                    mensagem = "Erro no teste",
                    erro = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: BuscarViagensRecorrencia (GET)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Busca viagens que pertencem à mesma série de recorrência.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid?)                                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com viagens relacionadas.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("BuscarViagensRecorrencia")]
        public async Task<IActionResult> BuscarViagensRecorrencia(Guid? id)
        {
            try
            {
                if (!id.HasValue || id == Guid.Empty)
                {
                    return BadRequest("ID inválido");
                }

                var viagens = await _unitOfWork.Viagem.GetAllReducedIQueryable(
                    selector: v => new
                    {
                        v.ViagemId,
                        v.DataInicial,
                        v.RecorrenciaViagemId,
                        v.Status
                    },
                    filter: v =>
                        (v.RecorrenciaViagemId == id || v.ViagemId == id) &&
                        v.Status != "Cancelada",
                    orderBy: q => q.OrderBy(v => v.DataInicial),
                    includeProperties: null,
                    asNoTracking: true
                )
                .Take(100)
                .ToListAsync();

                return Ok(viagens);
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao buscar viagens da série de recorrência", ex, "AgendaController.cs", "BuscarViagensRecorrencia");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "BuscarViagensRecorrencia", ex);
                return StatusCode(500, "Erro interno");
            }
        }

        public class ViagemRecorrenciaDto
        {
            public Guid ViagemId { get; set; }
            public DateTime DataInicial { get; set; }
            public Guid? RecorrenciaViagemId { get; set; }
            public string Status { get; set; }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AgendamentoAsync (POST)                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Cria ou atualiza viagem/agendamento com suporte a recorrência.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagem (AgendamentoViagem)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da operação.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("Agendamento")]
        public async Task<IActionResult> AgendamentoAsync([FromBody] AgendamentoViagem viagem)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                if (currentUser == null || currentUser.FindFirst(ClaimTypes.NameIdentifier) == null)
                {
                    return Unauthorized();
                }
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (viagem.DataFinal.HasValue && viagem.DataFinal.Value.Date > DateTime.Today)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "A Data Final não pode ser superior à data atual."
                    });
                }

                viagem.Monday = viagem.Monday ?? false;
                viagem.Tuesday = viagem.Tuesday ?? false;
                viagem.Wednesday = viagem.Wednesday ?? false;
                viagem.Thursday = viagem.Thursday ?? false;
                viagem.Friday = viagem.Friday ?? false;
                viagem.Saturday = viagem.Saturday ?? false;
                viagem.Sunday = viagem.Sunday ?? false;

                if (viagem.Status == "Realizada")
                {
                    if (string.IsNullOrEmpty(viagem.UsuarioIdFinalizacao))
                    {
                        viagem.UsuarioIdFinalizacao = currentUserID;
                        viagem.DataFinalizacao = DateTime.Now;
                    }

                    if (viagem.CriarViagemFechada == true)
                    {
                        if (string.IsNullOrEmpty(viagem.UsuarioIdCriacao))
                        {
                            viagem.UsuarioIdCriacao = currentUserID;
                            viagem.DataCriacao = DateTime.Now;
                        }
                    }
                }
                else if (viagem.Status == "Aberta")
                {
                    if (string.IsNullOrEmpty(viagem.UsuarioIdCriacao))
                    {
                        viagem.UsuarioIdCriacao = currentUserID;
                        viagem.DataCriacao = DateTime.Now;
                    }
                }
                
                bool isNew = viagem.ViagemId == Guid.Empty;

                if (isNew == true && viagem.Recorrente == "S")
                {
                    if (viagem.DatasSelecionadas == null)
                    {
                        viagem.DatasSelecionadas = new List<DateTime>
                        {
                            viagem.DataInicial ?? DateTime.Now,
                        };
                    }

                    if (viagem.DatasSelecionadas.Count == 0)
                    {
                        viagem.DatasSelecionadas = new List<DateTime>
                        {
                            viagem.DataInicial ?? DateTime.Now,
                        };
                    }

                    Viagem novaViagem = new Viagem();
                    var DatasSelecionadasAdicao = viagem.DatasSelecionadas;
                    viagem.DatasSelecionadas = null;

                    viagem.StatusAgendamento = true;
                    viagem.FoiAgendamento = false;

                    var primeiraDataSelecionada = DatasSelecionadasAdicao.FirstOrDefault();
                    if (primeiraDataSelecionada != default(DateTime))
                    {
                        var DataInicial = primeiraDataSelecionada.ToString("dd/MM/yyyy");
                        var HoraInicio = viagem.HoraInicio?.ToString("HH:mm");

                        DateTime DataInicialCompleta;
                        DateTime.TryParseExact(
                            (DataInicial + " " + HoraInicio),
                            new string[] { "dd/MM/yyyy HH:mm" },
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None,
                            out DataInicialCompleta
                        );

                        AtualizarDadosAgendamento(novaViagem, viagem);
                        novaViagem.DataInicial = primeiraDataSelecionada;
                        novaViagem.HoraInicio = new DateTime(
                            DataInicialCompleta.Year,
                            DataInicialCompleta.Month,
                            DataInicialCompleta.Day,
                            DataInicialCompleta.Hour,
                            DataInicialCompleta.Minute,
                            DataInicialCompleta.Second
                        );

                        novaViagem.UsuarioIdAgendamento = currentUserID;
                        novaViagem.DataAgendamento = DateTime.Now;

                        _unitOfWork.Viagem.Add(novaViagem);

                        if (viagem.KmFinal != null && viagem.KmFinal != 0)
                        {
                            var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v =>
                                v.VeiculoId == viagem.VeiculoId
                            );
                            if (veiculo.Quilometragem < viagem.KmFinal)
                            {
                                veiculo.Quilometragem = viagem.KmFinal;
                                _unitOfWork.Veiculo.Update(veiculo);
                            }
                        }

                        _unitOfWork.Save();

                        var viagemIdRecorrente = novaViagem.ViagemId;

                        foreach (var dataSelecionada in DatasSelecionadasAdicao.Skip(1))
                        {
                            try
                            {
                                Viagem novaViagemRecorrente = new Viagem();

                                DataInicial = dataSelecionada.ToString("dd/MM/yyyy");
                                DateTime.TryParseExact(
                                    (DataInicial + " " + HoraInicio),
                                    new string[] { "dd/MM/yyyy HH:mm" },
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None,
                                    out DataInicialCompleta
                                );

                                AtualizarDadosAgendamento(novaViagemRecorrente, viagem);
                                novaViagemRecorrente.DataInicial = dataSelecionada;
                                novaViagemRecorrente.HoraInicio = new DateTime(
                                    DataInicialCompleta.Year,
                                    DataInicialCompleta.Month,
                                    DataInicialCompleta.Day,
                                    DataInicialCompleta.Hour,
                                    DataInicialCompleta.Minute,
                                    DataInicialCompleta.Second
                                );
                                novaViagemRecorrente.RecorrenciaViagemId = viagemIdRecorrente; 
                                novaViagemRecorrente.UsuarioIdAgendamento = currentUserID;
                                novaViagemRecorrente.DataAgendamento = DateTime.Now;

                                if (viagem.Status == "Realizada")
                                {
                                    if (string.IsNullOrEmpty(novaViagemRecorrente.UsuarioIdFinalizacao))
                                    {
                                        novaViagemRecorrente.UsuarioIdFinalizacao = currentUserID;
                                        novaViagemRecorrente.DataFinalizacao = DateTime.Now;
                                    }

                                    if (viagem.CriarViagemFechada == true)
                                    {
                                        if (string.IsNullOrEmpty(novaViagemRecorrente.UsuarioIdCriacao))
                                        {
                                            novaViagemRecorrente.UsuarioIdCriacao = currentUserID;
                                            novaViagemRecorrente.DataCriacao = DateTime.Now;
                                        }
                                    }
                                }
                                else if (viagem.Status == "Aberta")
                                {
                                    if (string.IsNullOrEmpty(novaViagemRecorrente.UsuarioIdCriacao))
                                    {
                                        novaViagemRecorrente.UsuarioIdCriacao = currentUserID;
                                        novaViagemRecorrente.DataCriacao = DateTime.Now;
                                    }
                                }

                                _unitOfWork.Viagem.Add(novaViagemRecorrente);
                                _unitOfWork.Save();
                            }
                            catch (Exception error)
                            {
                                _log.Error(error.Message, error, "AgendaController.cs", "AgendamentoAsync.RecorrenciaDatas");
                                Alerta.TratamentoErroComLinha(
                                    "AgendaController.cs",
                                    "AgendamentoAsync.RecorrenciaDatas",
                                    error
                                );
                            }
                        }

                        if (
                            viagem.Intervalo == "D"
                            || viagem.Intervalo == "S"
                            || viagem.Intervalo == "Q"
                            || viagem.Intervalo == "M"
                        )
                        {
                            try
                            {
                                DateTime proximaData = primeiraDataSelecionada;
                                int incremento = 0;

                                switch (viagem.Intervalo)
                                {
                                    case "D":
                                        incremento = 1;
                                        break;

                                    case "S":
                                        incremento = 7;
                                        break;

                                    case "Q":
                                        incremento = 15;
                                        break;

                                    case "M":
                                        incremento = 30;
                                        break;
                                }

                                for (int i = 1; i < DatasSelecionadasAdicao.Count(); i++)
                                {
                                    try
                                    {
                                        proximaData = proximaData.AddDays(incremento);

                                        Viagem novaViagemPeriodo = new Viagem();
                                        AtualizarDadosAgendamento(novaViagemPeriodo, viagem);
                                        novaViagemPeriodo.DataInicial = proximaData;
                                        novaViagemPeriodo.HoraInicio = new DateTime(
                                            proximaData.Year,
                                            proximaData.Month,
                                            proximaData.Day,
                                            DataInicialCompleta.Hour,
                                            DataInicialCompleta.Minute,
                                            DataInicialCompleta.Second
                                        );
                                        novaViagemPeriodo.RecorrenciaViagemId = viagemIdRecorrente;

                                        novaViagemPeriodo.UsuarioIdAgendamento = currentUserID;
                                        novaViagemPeriodo.DataAgendamento = DateTime.Now;

                                        if (viagem.Status == "Realizada")
                                        {
                                            if (string.IsNullOrEmpty(novaViagemPeriodo.UsuarioIdFinalizacao))
                                            {
                                                novaViagemPeriodo.UsuarioIdFinalizacao = currentUserID;
                                                novaViagemPeriodo.DataFinalizacao = DateTime.Now;
                                            }

                                            if (viagem.CriarViagemFechada == true)
                                            {
                                                if (string.IsNullOrEmpty(novaViagemPeriodo.UsuarioIdCriacao))
                                                {
                                                    novaViagemPeriodo.UsuarioIdCriacao = currentUserID;
                                                    novaViagemPeriodo.DataCriacao = DateTime.Now;
                                                }
                                            }
                                        }
                                        else if (viagem.Status == "Aberta")
                                        {
                                            if (string.IsNullOrEmpty(novaViagemPeriodo.UsuarioIdCriacao))
                                            {
                                                novaViagemPeriodo.UsuarioIdCriacao = currentUserID;
                                                novaViagemPeriodo.DataCriacao = DateTime.Now;
                                            }
                                        }

                                        _unitOfWork.Viagem.Add(novaViagemPeriodo);
                                        _unitOfWork.Save();
                                    }
                                    catch (Exception error)
                                    {
                                        _log.Error(error.Message, error, "AgendaController.cs", "Agendamento.intervalo.for");
                                        Alerta.TratamentoErroComLinha(
                                            "AgendaController.cs",
                                            "Agendamento.intervalo.for",
                                            error
                                        );
                                    }
                                }
                            }
                            catch (Exception error)
                            {
                                _log.Error(error.Message, error, "AgendaController.cs", "Agendamento.intervalo");
                                Alerta.TratamentoErroComLinha(
                                    "AgendaController.cs",
                                    "Agendamento.intervalo",
                                    error
                                );
                            }
                        }
                    }

                    novaViagem.OperacaoBemSucedida = true;
                    return Ok(new
                    {
                        novaViagem,
                        success = true
                    });
                }

                if (isNew == false)
                {
                    var agendamentoAtual = _unitOfWork.Viagem.GetFirstOrDefaultWithTracking(vg =>
                        vg.ViagemId == viagem.ViagemId
                    );
                    if (agendamentoAtual == null)
                    {
                        return NotFound(
                            new
                            {
                                success = false,
                                message = "Agendamento não encontrado"
                            }
                        );
                    }

                    var dataOriginal = agendamentoAtual.DataInicial;
                    var horaOriginal = agendamentoAtual.HoraInicio;
                    var dataFinalKeep = agendamentoAtual.DataFinal;
                    var horaFimKeep = agendamentoAtual.HoraFim;

                    agendamentoAtual.AtualizarDados(viagem);
                    agendamentoAtual.DataInicial = dataOriginal;

                    if (viagem.Status == "Agendada")
                    {
                        agendamentoAtual.DataInicial = viagem.DataInicial;
                    }

                    if (viagem.HoraInicio.HasValue && dataOriginal.HasValue)
                    {
                        agendamentoAtual.HoraInicio = CombineHourKeepingDate(
                            dataOriginal,
                            viagem.HoraInicio
                        );
                    }
                    else
                    {
                        agendamentoAtual.HoraInicio = horaOriginal;
                    }

                    agendamentoAtual.DataFinal = viagem.DataFinal ?? dataFinalKeep;
                    agendamentoAtual.HoraFim = viagem.HoraFim ?? horaFimKeep;

                    bool temCamposFinalizacao = viagem.DataFinal.HasValue &&
                                                viagem.HoraFim.HasValue;

                    if (temCamposFinalizacao && viagem.Status == "Realizada")
                    {
                        if (string.IsNullOrEmpty(agendamentoAtual.UsuarioIdFinalizacao))
                        {
                            agendamentoAtual.UsuarioIdFinalizacao = currentUserID;
                            agendamentoAtual.DataFinalizacao = DateTime.Now;
                        }
                    }

                    bool isTransformacaoParaViagem = viagem.FoiAgendamento == true &&
                                                      (viagem.Status == "Aberta" || viagem.Status == "Realizada");

                    if (isTransformacaoParaViagem)
                    {
                        if (!string.IsNullOrEmpty(viagem.UsuarioIdCriacao))
                        {
                            if (string.IsNullOrEmpty(agendamentoAtual.UsuarioIdCriacao))
                            {
                                agendamentoAtual.UsuarioIdCriacao = viagem.UsuarioIdCriacao;
                                agendamentoAtual.DataCriacao = viagem.DataCriacao;
                            }
                        }
                        else if (string.IsNullOrEmpty(agendamentoAtual.UsuarioIdCriacao))
                        {
                            agendamentoAtual.UsuarioIdCriacao = currentUserID;
                            agendamentoAtual.DataCriacao = DateTime.Now;
                        }
                    }

                    if (viagem.KmFinal != null && viagem.KmFinal != 0)
                    {
                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefaultWithTracking(vcl =>
                            vcl.VeiculoId == viagem.VeiculoId
                        );
                        if (veiculo != null && veiculo.Quilometragem < viagem.KmFinal)
                        {
                            veiculo.Quilometragem = viagem.KmFinal.Value;
                            _unitOfWork.Veiculo.Update(veiculo);
                        }
                    }

                    _unitOfWork.Viagem.Update(agendamentoAtual);
                    _unitOfWork.Save();

                    if (agendamentoAtual.Status == "Realizada")
                    {
                        await _viagemEstatisticaService.AtualizarEstatisticasDiaAsync(viagem.DataInicial.Value);
                    }

                    return Ok(
                        new
                        {
                            success = true,
                            message = "Agendamento Atualizado com Sucesso",
                            viagemId = agendamentoAtual.ViagemId,
                            objViagem = agendamentoAtual,
                        }
                    );
                }

                if ((viagem.ViagemId == Guid.Empty))
                {
                    Viagem objViagem = new Viagem();
                    AtualizarDadosAgendamento(objViagem, viagem);

                    objViagem.UsuarioIdAgendamento = currentUserID;
                    objViagem.DataAgendamento = DateTime.Now;

                    _unitOfWork.Viagem.Add(objViagem);
                    _unitOfWork.Save();

                    if (viagem.KmFinal != null && viagem.KmFinal != 0)
                    {
                        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefaultWithTracking(v =>
                            v.VeiculoId == viagem.VeiculoId
                        );
                        if (veiculo.Quilometragem < viagem.KmFinal)
                        {
                            veiculo.Quilometragem = viagem.KmFinal;
                            _unitOfWork.Veiculo.Update(veiculo);
                        }
                    }

                    objViagem.OperacaoBemSucedida = true;
                    return Ok(
                        new
                        {
                            success = true,
                            message = "Agendamento inserido com sucesso",
                            viagemId = objViagem.ViagemId,
                            objViagem,
                        }
                    );
                }

                return Ok(
                    new
                    {
                        success = true,
                        message = "Operação realizada com sucesso",
                        viagemId = viagem.ViagemId,
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("Erro no processamento do agendamento", ex, "AgendaController.cs", "AgendamentoAsync");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "AgendamentoAsync", ex);
                return BadRequest(new
                {
                    success = false,
                    mensagem = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ApagaAgendamento (POST)                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove um agendamento específico.                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagem (AgendamentoViagem)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da exclusão.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("ApagaAgendamento")]
        public IActionResult ApagaAgendamento(AgendamentoViagem viagem)
        {
            try
            {
                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefaultWithTracking(v =>
                    v.ViagemId == viagem.ViagemId
                );
                if (objFromDb == null)
                {
                    return NotFound(
                        new
                        {
                            success = false,
                            message = "Agendamento não encontrado"
                        }
                    );
                }

                var itensManutencao = _context.ItensManutencao
                    .AsTracking()
                    .Where(i => i.ViagemId.HasValue && i.ViagemId.Value == viagem.ViagemId)
                    .ToList();

                if (itensManutencao.Any())
                {
                    _context.ItensManutencao.RemoveRange(itensManutencao);
                    _context.SaveChanges();
                }

                _unitOfWork.Viagem.Remove(objFromDb);
                _unitOfWork.Save();
                return Ok(new
                {
                    success = true,
                    message = "Agendamento apagado com sucesso"
                });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao apagar agendamento", ex, "AgendaController.cs", "ApagaAgendamento");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "ApagaAgendamento", ex);
                return StatusCode(500, new
                {
                    success = false,
                    error = "Erro interno do servidor",
                    message = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ApagaAgendamentosRecorrentes (POST)                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Apaga agendamentos recorrentes em lote.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • request (ApagaRecorrentesRequest)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com resultado da exclusão.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("ApagaAgendamentosRecorrentes")]
        public IActionResult ApagaAgendamentosRecorrentes([FromBody] ApagaRecorrentesRequest request)
        {
            try
            {
                if (request == null || request.RecorrenciaViagemId == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "RecorrenciaViagemId é obrigatório"
                    });
                }

                var viagemIds = _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.RecorrenciaViagemId == request.RecorrenciaViagemId
                             || v.ViagemId == request.RecorrenciaViagemId)
                    .Select(v => v.ViagemId)
                    .ToList();

                if (!viagemIds.Any())
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Nenhum agendamento recorrente encontrado"
                    });
                }

                var paramPlaceholders = string.Join(",", viagemIds.Select((_, i) => $"@p{i}"));

                var sqlDeleteItens = $@"
                    DELETE FROM ItensManutencao
                    WHERE ViagemId IN ({paramPlaceholders})";

                _context.Database.ExecuteSqlRaw(
                    sqlDeleteItens,
                    viagemIds.Cast<object>().ToArray()
                );

                var sqlDeleteViagens = $@"
                    DELETE FROM Viagem
                    WHERE ViagemId IN ({paramPlaceholders})";

                _context.Database.ExecuteSqlRaw(
                    sqlDeleteViagens,
                    viagemIds.Cast<object>().ToArray()
                );

                return Ok(new
                {
                    success = true,
                    message = $"{viagemIds.Count} agendamento(s) recorrente(s) foram excluídos com sucesso"
                });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao apagar agendamentos recorrentes", ex, "AgendaController.cs", "ApagaAgendamentosRecorrentes");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "ApagaAgendamentosRecorrentes", ex);
                return StatusCode(500, new
                {
                    success = false,
                    error = "Erro interno do servidor",
                    message = ex.Message
                });
            }
        }

        public class ApagaRecorrentesRequest
        {
            public Guid RecorrenciaViagemId { get; set; }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CancelaAgendamento (POST)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Cancela um agendamento marcando o status como Cancelada.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagem (AgendamentoViagem)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com resultado do cancelamento.                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("CancelaAgendamento")]
        public IActionResult CancelaAgendamento(AgendamentoViagem viagem)
        {
            try
            {
                var objFromDb = _unitOfWork.Viagem.GetFirstOrDefaultWithTracking(v =>
                    v.ViagemId == viagem.ViagemId
                );
                if (objFromDb == null)
                {
                    return NotFound(
                        new
                        {
                            success = false,
                            message = "Agendamento não encontrado"
                        }
                    );
                }

                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                objFromDb.UsuarioIdCancelamento = currentUserID;
                objFromDb.DataCancelamento = DateTime.Now;
                objFromDb.Status = "Cancelada";
                objFromDb.Descricao = viagem.Descricao;

                objFromDb.Monday = viagem.Monday ?? false;
                objFromDb.Tuesday = viagem.Tuesday ?? false;
                objFromDb.Wednesday = viagem.Wednesday ?? false;
                objFromDb.Thursday = viagem.Thursday ?? false;
                objFromDb.Friday = viagem.Friday ?? false;
                objFromDb.Saturday = viagem.Saturday ?? false;
                objFromDb.Sunday = viagem.Sunday ?? false;

                _unitOfWork.Viagem.Update(objFromDb);
                _unitOfWork.Save();
                return Ok(new
                {
                    success = true,
                    message = "Agendamento cancelado com sucesso"
                });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao cancelar agendamento", ex, "AgendaController.cs", "CancelaAgendamento");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "CancelaAgendamento", ex);
                return StatusCode(500, new
                {
                    success = false,
                    error = "Erro interno do servidor"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CarregaViagens (GET)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Carrega viagens para o componente de calendário (FullCalendar).          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • start (DateTime), end (DateTime)                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ActionResult: JSON com eventos da agenda.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("CarregaViagens")]
        public ActionResult CarregaViagens(DateTime start, DateTime end)
        {
            try
            {
                DateTime startMenos3 = start.AddHours(-3);
                DateTime endMenos3 = end.AddHours(-3);

                var viagensRaw = _context.ViewViagensAgenda
                    .AsNoTracking()
                    .Where(v => v.DataInicial.HasValue
                        && v.DataInicial >= startMenos3
                        && v.DataInicial < endMenos3)
                    .ToList();

                var viagens = viagensRaw
                    .Select(v =>
                    {
                        var startDate = v.Start ?? v.DataInicial ?? DateTime.Now;
                        var endDate = startDate.AddHours(1);

                        return new
                        {
                            id = v.ViagemId,
                            title = v.Titulo ?? "Viagem",
                            start = startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                            end = endDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                            backgroundColor = v.CorEvento ?? "#808080",
                            textColor = v.CorTexto ?? "#FFFFFF",
                            descricao = v.Descricao ?? "",
                            placa = v.Placa,
                            motorista = v.NomeMotorista,
                            evento = v.NomeEventoFull,
                            finalidade = v.Finalidade
                        };
                    })
                    .ToList();

                return Ok(new { data = viagens });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao carregar viagens", ex, "AgendaController.cs", "CarregaViagens");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "CarregaViagens", ex);
                return StatusCode(500, new
                {
                    success = false,
                    error = "Erro interno do servidor",
                    mensagemDetalhada = ex.Message,
                    stackTrace = ex.StackTrace,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetDatasViagem (GET)                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna datas associadas a série de agendamentos recorrentes.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagemId (Guid)                                                         ║
        /// ║    • recorrenciaViagemId (Guid)                                              ║
        /// ║    • editarProximos (bool)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de datas.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("GetDatasViagem")]
        public IActionResult GetDatasViagem(
            Guid viagemId,
            Guid recorrenciaViagemId = default(Guid),
            bool editarProximos = false
        )
        {
            try
            {
                var objViagens = _unitOfWork.Viagem.GetAllReduced(selector: v => new
                {
                    v.DataInicial,
                    v.RecorrenciaViagemId,
                    v.ViagemId,
                });

                List<DateTime> datasOrdenadas;

                if (recorrenciaViagemId == Guid.Empty)
                {
                    datasOrdenadas = objViagens
                        .Where(v => v.ViagemId == viagemId || v.RecorrenciaViagemId == viagemId)
                        .Select(v => v.DataInicial)
                        .Where(d => d.HasValue)
                        .Select(d => d.Value)
                        .OrderBy(d => d)
                        .ToList();
                }
                else if (editarProximos)
                {
                    var dataAtual = objViagens
                        .FirstOrDefault(v => v.ViagemId == viagemId)
                        ?.DataInicial;

                    if (dataAtual.HasValue)
                    {
                        datasOrdenadas = objViagens
                            .Where(v =>
                                v.RecorrenciaViagemId == recorrenciaViagemId
                                && v.DataInicial >= dataAtual
                            )
                            .Select(v => v.DataInicial)
                            .Where(d => d.HasValue)
                            .Select(d => d.Value)
                            .OrderBy(d => d)
                            .ToList();
                    }
                    else
                    {
                        return BadRequest(
                            new
                            {
                                sucesso = false,
                                mensagem = "Registro de viagem não encontrado."
                            }
                        );
                    }
                }
                else
                {
                    datasOrdenadas = objViagens
                        .Where(v =>
                            v.RecorrenciaViagemId == recorrenciaViagemId
                            || v.ViagemId == viagemId
                            || v.ViagemId == recorrenciaViagemId
                        )
                        .Select(v => v.DataInicial)
                        .Where(d => d.HasValue)
                        .Select(d => d.Value)
                        .OrderBy(d => d)
                        .ToList();
                }

                return Ok(datasOrdenadas);
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter datas da viagem", ex, "AgendaController.cs", "GetDatasViagem");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "GetDatasViagem", ex);
                return BadRequest(new
                {
                    sucesso = false,
                    mensagem = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterAgendamento (GET)                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém detalhes completos de um agendamento específico.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagemId (Guid)                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados do agendamento.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("ObterAgendamento")]
        public async Task<IActionResult> ObterAgendamento(Guid viagemId)
        {
            try
            {
                var objViagem = _unitOfWork
                    .Viagem.GetAll()
                    .Where(v => v.ViagemId == viagemId)
                    .FirstOrDefault();

                if (objViagem == null)
                {
                    return NotFound(new
                    {
                        mensagem = "Agendamento não encontrado"
                    });
                }

                return Ok(objViagem);
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter agendamento", ex, "AgendaController.cs", "ObterAgendamento");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "ObterAgendamento", ex);
                return StatusCode(
                    500,
                    new
                    {
                        mensagem = "Erro interno ao obter o agendamento",
                        detalhes = ex.Message
                    }
                );
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterAgendamentoEdicao (GET)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém dados de um agendamento para edição.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagemId (Guid)                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ActionResult: JSON com dados do agendamento.                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("ObterAgendamentoEdicao")]
        public ActionResult ObterAgendamentoEdicao(Guid viagemId)
        {
            try
            {
                var agendamentoEdicao = _unitOfWork.Viagem.GetFirstOrDefault(v =>
                    v.ViagemId == viagemId
                );
                if (agendamentoEdicao != null)
                {
                    return Ok(agendamentoEdicao);
                }
                return NotFound(
                    new
                    {
                        sucesso = false,
                        mensagem = "Registro de viagem não encontrado."
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter agendamento para edição", ex, "AgendaController.cs", "ObterAgendamentoEdicao");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "ObterAgendamentoEdicao", ex);
                return StatusCode(500, new
                {
                    sucesso = false,
                    error = "Erro interno do servidor"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterAgendamentoEdicaoInicial (GET)                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém lista de agendamentos para a fase inicial de edição.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • viagemId (Guid)                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ActionResult: JSON com agendamentos.                                    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("ObterAgendamentoEdicaoInicial")]
        public ActionResult ObterAgendamentoEdicaoInicial(Guid viagemId)
        {
            try
            {
                var agendamentoEdicao = _unitOfWork.Viagem.GetAll(v => v.ViagemId == viagemId);
                if (agendamentoEdicao != null)
                {
                    return Ok(agendamentoEdicao);
                }
                return NotFound(
                    new
                    {
                        sucesso = false,
                        mensagem = "Registro de viagem não encontrado."
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter agendamento edição inicial", ex, "AgendaController.cs", "ObterAgendamentoEdicaoInicial");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "ObterAgendamentoEdicaoInicial", ex);
                return StatusCode(500, new
                {
                    sucesso = false,
                    error = "Erro interno do servidor"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterAgendamentoExclusao (GET)                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém agendamentos vinculados a uma recorrência para exclusão em massa.  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • recorrenciaViagemId (Guid)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ActionResult: JSON com agendamentos da recorrência.                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("ObterAgendamentoExclusao")]
        public ActionResult ObterAgendamentoExclusao(Guid recorrenciaViagemId)
        {
            try
            {
                var objExclusao = _unitOfWork.Viagem.GetAll(v =>
                    v.RecorrenciaViagemId == recorrenciaViagemId
                );
                if (objExclusao != null)
                {
                    return Ok(objExclusao);
                }
                return NotFound(
                    new
                    {
                        sucesso = false,
                        mensagem = "Registro de viagem não encontrado."
                    }
                );
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter agendamentos para exclusão", ex, "AgendaController.cs", "ObterAgendamentoExclusao");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "ObterAgendamentoExclusao", ex);
                return StatusCode(500, new
                {
                    sucesso = false,
                    error = "Erro interno do servidor"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterAgendamentosRecorrentes (GET)                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém agendamentos pertencentes a uma mesma série recorrente.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • RecorrenciaViagemId (string)                                            ║
        /// ║    • DataInicialRecorrencia (string)                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com agendamentos recorrentes.                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("ObterAgendamentosRecorrentes")]
        public async Task<IActionResult> ObterAgendamentosRecorrentes(
            string RecorrenciaViagemId,
            string DataInicialRecorrencia
        )
        {
            try
            {
                var objViagens = _unitOfWork
                    .Viagem.GetAll()
                    .Where(v => v.RecorrenciaViagemId == Guid.Parse(RecorrenciaViagemId));

                if (objViagens == null || !objViagens.Any())
                {
                    return NotFound(new
                    {
                        mensagem = "Agendamentos recorrentes não encontrados"
                    });
                }
                return Ok(objViagens);
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter agendamentos recorrentes", ex, "AgendaController.cs", "ObterAgendamentosRecorrentes");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "ObterAgendamentosRecorrentes", ex);
                return StatusCode(500, new
                {
                    mensagem = "Erro interno ao obter os agendamentos recorrentes",
                    erro = ex.Message,
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RecuperaUsuario (GET)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Busca o nome completo de um usuário pelo identificador.                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (string)                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados do usuário.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("RecuperaUsuario")]
        public IActionResult RecuperaUsuario(string Id)
        {
            try
            {
                var objUsuario = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == Id);

                if (objUsuario == null)
                {
                    return Ok(new { data = "" });
                }
                else
                {
                    return Ok(new { data = objUsuario.NomeCompleto });
                }
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao recuperar usuário", ex, "AgendaController.cs", "RecuperaUsuario");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "RecuperaUsuario", ex);
                return StatusCode(500, new
                {
                    success = false,
                    error = "Erro interno do servidor"
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RecuperaViagem (GET)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recupera os dados de uma viagem específica pelo ID.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Id (Guid)                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ActionResult: JSON com dados da viagem.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("RecuperaViagem")]
        public ActionResult RecuperaViagem(Guid Id)
        {
            try
            {
                var viagemObj = _unitOfWork.Viagem.GetFirstOrDefault(v => v.ViagemId == Id);
                return Ok(new
                {
                    data = viagemObj
                });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao recuperar viagem", ex, "AgendaController.cs", "RecuperaViagem");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "RecuperaViagem", ex);
                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    innerException = ex.InnerException?.Message,
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: VerificarAgendamento (GET)                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Verifica conflitos de agendamento em data e hora específicas.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • data (string)                                                           ║
        /// ║    • viagemIdRecorrente (Guid)                                                ║
        /// ║    • horaInicio (string)                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status de conflito.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("VerificarAgendamento")]
        public IActionResult VerificarAgendamento(
            string data,
            Guid viagemIdRecorrente = default,
            string horaInicio = null
        )
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    return BadRequest(
                        new
                        {
                            sucesso = false,
                            mensagem = "A data é obrigatória para verificar o agendamento.",
                        }
                    );
                }

                if (!DateTime.TryParse(data, out DateTime dataAgendamento))
                {
                    return BadRequest(new
                    {
                        sucesso = false,
                        mensagem = "Data inválida."
                    });
                }

                TimeSpan? horaAgendamento = null;
                if (
                    !string.IsNullOrEmpty(horaInicio)
                    && TimeSpan.TryParse(horaInicio, out TimeSpan parsedHora)
                )
                {
                    horaAgendamento = parsedHora;
                }

                var query = _context.Viagem
                    .AsNoTracking()
                    .Where(v => v.DataInicial.HasValue
                        && v.DataInicial.Value.Date == dataAgendamento.Date);

                if (horaAgendamento.HasValue)
                {
                    query = query.Where(v => v.HoraInicio.HasValue
                        && v.HoraInicio.Value.TimeOfDay == horaAgendamento);
                }

                if (viagemIdRecorrente != Guid.Empty)
                {
                    query = query.Where(v => v.RecorrenciaViagemId == viagemIdRecorrente);
                }

                var existeAgendamento = query.Any();

                return Ok(new
                {
                    existe = existeAgendamento
                });
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao verificar disponibilidade de agendamento", ex, "AgendaController.cs", "VerificarAgendamento");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "VerificarAgendamento", ex);
                return BadRequest(new
                {
                    sucesso = false,
                    mensagem = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AtualizarDadosAgendamento (Private)                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Copia dados do DTO para o objeto de persistência `Viagem`.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • objViagem (Viagem), viagem (AgendamentoViagem)                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void                                                                    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private void AtualizarDadosAgendamento(Viagem objViagem, AgendamentoViagem viagem)
        {
            try
            {
                // (IA) Mapeamento de propriedades entre View Model e Entidade de Banco.
                objViagem.DataInicial = viagem.DataInicial;
                objViagem.HoraInicio = viagem.HoraInicio;
                objViagem.Finalidade = viagem.Finalidade;
                objViagem.Origem = viagem.Origem;
                objViagem.Destino = viagem.Destino;
                objViagem.MotoristaId = viagem.MotoristaId;
                objViagem.VeiculoId = viagem.VeiculoId;
                objViagem.RequisitanteId = viagem.RequisitanteId;
                objViagem.RamalRequisitante = viagem.RamalRequisitante;
                objViagem.SetorSolicitanteId = viagem.SetorSolicitanteId ?? Guid.Empty;
                objViagem.Descricao = viagem.Descricao;
                objViagem.StatusAgendamento = viagem.StatusAgendamento;
                objViagem.FoiAgendamento = viagem.FoiAgendamento;
                objViagem.Status = viagem.Status;
                objViagem.DataFinal = viagem.DataFinal;
                objViagem.HoraFim = viagem.HoraFim;
                objViagem.NoFichaVistoria = viagem.NoFichaVistoria;
                objViagem.EventoId = viagem.EventoId;
                objViagem.KmAtual = viagem.KmAtual ?? 0;
                objViagem.KmInicial = viagem.KmInicial ?? 0;
                objViagem.KmFinal = viagem.KmFinal ?? 0;
                objViagem.CombustivelInicial = viagem.CombustivelInicial;
                objViagem.CombustivelFinal = viagem.CombustivelFinal;
                objViagem.UsuarioIdAgendamento = viagem.UsuarioIdAgendamento;
                objViagem.DataAgendamento = viagem.DataAgendamento;
                objViagem.UsuarioIdCriacao = viagem.UsuarioIdCriacao;
                objViagem.DataCriacao = viagem.DataCriacao;
                objViagem.UsuarioIdFinalizacao = viagem.UsuarioIdFinalizacao;
                objViagem.DataFinalizacao = viagem.DataFinalizacao;
                objViagem.Recorrente = viagem.Recorrente;
                objViagem.RecorrenciaViagemId = viagem.RecorrenciaViagemId;
                objViagem.Intervalo = viagem.Intervalo;
                objViagem.DataFinalRecorrencia = viagem.DataFinalRecorrencia;
                objViagem.Monday = viagem.Monday;
                objViagem.Tuesday = viagem.Tuesday;
                objViagem.Wednesday = viagem.Wednesday;
                objViagem.Thursday = viagem.Thursday;
                objViagem.Friday = viagem.Friday;
                objViagem.Saturday = viagem.Saturday;
                objViagem.Sunday = viagem.Sunday;
                objViagem.DiaMesRecorrencia = viagem.DiaMesRecorrencia;

                string descricao = objViagem.Descricao;
                if (objViagem.Descricao != null)
                    descricao = Servicos.ConvertHtml(descricao);
                objViagem.DescricaoSemFormato = descricao;
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao atualizar dados do agendamento", ex, "AgendaController.cs", "AtualizarDadosAgendamento");
                Alerta.TratamentoErroComLinha("AgendaController.cs", "AtualizarDadosAgendamento", ex);
                return;
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CombineHourKeepingDate (Private Static)                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Combina a data de um elemento com a hora de outro.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • baseDate (DateTime?), newTime (DateTime?)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • DateTime?                                                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private static DateTime? CombineHourKeepingDate(DateTime? baseDate, DateTime? newTime)
        {
            try
            {
                if (!baseDate.HasValue || !newTime.HasValue)
                    return null;

                var d = baseDate.Value.Date;
                var t = newTime.Value;

                return new DateTime(
                    d.Year,
                    d.Month,
                    d.Day,
                    t.Hour,
                    t.Minute,
                    0,
                    DateTimeKind.Unspecified
                );
            }
            catch (Exception ex)
            {
                // (IA) Métodos estáticos não possuem acesso ao campo _log instanciado.
                Alerta.TratamentoErroComLinha("AgendaController.cs", "CombineHourKeepingDate", ex);
                return default(DateTime?);
            }
        }
    }
}
