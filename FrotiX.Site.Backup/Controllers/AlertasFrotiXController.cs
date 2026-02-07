using FrotiX.Hubs;
/*
 *  ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 *  ║                                     F R O T I X   -   2 0 2 6                                        ║
 *  ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 *  ║  PROJETO: FrotiX.Site                                                                                ║
 *  ║  MÓDULO:  Controllers                                                                                ║
 *  ║  ARQUIVO: AlertasFrotiXController.cs                                                                 ║
 *  ║  DESCRIÇÃO: Controlador para gestão de Alertas e Notificações via SignalR.                           ║
 *  ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrotiX.Helpers;
using FrotiX.Services;

namespace FrotiX.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[IgnoreAntiforgeryToken]
	public class AlertasFrotiXController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAlertasFrotiXRepository _alertasRepo;
		private readonly IHubContext<AlertasHub> _hubContext;
		private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: AlertasFrotiXController (Constructor)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa repositórios, SignalR e serviço de log do módulo de alertas.   ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Garante comunicação em tempo real e rastreabilidade dos alertas.          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • unitOfWork (IUnitOfWork): acesso a dados.                               ║
        /// ║    • alertasRepo (IAlertasFrotiXRepository): repositório de alertas.          ║
        /// ║    • hubContext (IHubContext<AlertasHub>): SignalR hub.                      ║
        /// ║    • logService (ILogService): log centralizado.                             ║
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
        public AlertasFrotiXController(
            IUnitOfWork unitOfWork,
            IAlertasFrotiXRepository alertasRepo,
            IHubContext<AlertasHub> hubContext,
            ILogService logService)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _alertasRepo = alertasRepo;
                _hubContext = hubContext;
                _log = logService;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "AlertasFrotiXController", ex);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetDetalhesAlerta                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna detalhes completos do alerta, incluindo destinatários e status.   ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Fornece visibilidade total do engajamento e histórico do alerta.          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • id (Guid): identificador do alerta.                                     ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados do alerta e estatísticas.                  ║
        /// ║    • Consumidor: UI de Alertas/Notificações.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _unitOfWork.AlertasFrotiX.GetFirstOrDefaultAsync()                       ║
        /// ║    • _unitOfWork.AspNetUsers.GetFirstOrDefaultAsync()                         ║
        /// ║    • ObterInfoTipo() / ObterInfoPrioridade()                                  ║
        /// ║    • _log.Error() / Alerta.TratamentoErroComLinha() → erros.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/AlertasFrotiX/GetDetalhesAlerta/{id}                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Alertas e Notificações                                  ║
        /// ║    • Arquivos relacionados: Views/Alertas/*.cshtml                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("GetDetalhesAlerta/{id}")]
        public async Task<IActionResult> GetDetalhesAlerta(Guid id)
        {
            try
            {
                // [DADOS] Busca alerta com relacionamentos necessários
                var alerta = await _unitOfWork.AlertasFrotiX.GetFirstOrDefaultAsync(
                    a => a.AlertasFrotiXId == id,
                    includeProperties: "AlertasUsuarios,Viagem,Manutencao,Veiculo,Motorista"
                );

                if (alerta == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Alerta não encontrado"
                    });
                }

				// [DEBUG] Informações auxiliares para diagnóstico
                var debugInfo = new
                {
                    alertasUsuariosCount = alerta.AlertasUsuarios?.Count ?? 0,
                    alertasUsuariosIsNull = alerta.AlertasUsuarios == null,
                    totalLidosNoBanco = alerta.AlertasUsuarios?.Count(au => au.Lido) ?? 0
                };

                var usuariosDetalhes = new List<object>();

				// [LOGICA] Mapeamento dos destinatários do alerta
                foreach (var au in alerta.AlertasUsuarios)
                {
                    var usuario = await _unitOfWork.AspNetUsers.GetFirstOrDefaultAsync(
                        u => u.Id == au.UsuarioId
                    );

                    usuariosDetalhes.Add(new
                    {
                        usuarioId = au.UsuarioId,
                        nomeUsuario = usuario?.UserName ?? "Usuário removido",
                        email = usuario?.Email,
                        lido = au.Lido,
                        dataLeitura = au.DataLeitura,
                        dataNotificacao = au.DataNotificacao,
                        notificado = au.Notificado,
                        apagado = au.Apagado,
                        dataApagado = au.DataApagado
                    });
                }

				// [LOGICA] Estatísticas de engajamento
                var totalDestinatarios = alerta.AlertasUsuarios.Count;
                var totalNotificados = alerta.AlertasUsuarios.Count(au => au.Notificado);
                var aguardandoNotificacao = alerta.AlertasUsuarios.Count(au => !au.Notificado);
                var usuariosLeram = alerta.AlertasUsuarios.Count(au => au.Lido);
                var usuariosNaoLeram = alerta.AlertasUsuarios.Count(au => au.Notificado && !au.Lido && !au.Apagado);
                var usuariosApagaram = alerta.AlertasUsuarios.Count(au => au.Apagado);
                var percentualLeitura = totalNotificados > 0
                    ? Math.Round((double)usuariosLeram / totalNotificados * 100, 1)
                    : 0;

                var dataInicio = alerta.DataExibicao ?? alerta.DataInsercao;
                var dataFim = alerta.DataExpiracao ?? DateTime.Now;
                var tempoNoAr = dataFim - dataInicio;

                string tempoNoArFormatado = "N/A";

				// [LOGICA] Formatação amigável do tempo do alerta
                if (tempoNoAr.HasValue && tempoNoAr.Value.TotalSeconds > 0)
                {
                    var tempo = tempoNoAr.Value;

                    if (tempo.TotalMinutes < 1)
                    {
                        tempoNoArFormatado = "Menos de 1 min";
                    }
                    else if (tempo.TotalMinutes < 60)
                    {
                        tempoNoArFormatado = $"{(int)tempo.TotalMinutes} min";
                    }
                    else if (tempo.TotalHours < 24)
                    {
                        int horas = (int)tempo.TotalHours;
                        int minutos = tempo.Minutes;
                        tempoNoArFormatado = $"{horas}h {minutos}min";
                    }
                    else
                    {
                        int dias = (int)tempo.TotalDays;
                        int horas = tempo.Hours;
                        int minutos = tempo.Minutes;
                        tempoNoArFormatado = $"{dias}d {horas}h {minutos}min";
                    }
                }

                string nomeCriador = "Sistema";

				// [LOGICA] Identificação do criador do alerta
                if (!string.IsNullOrEmpty(alerta.UsuarioCriadorId) &&
                    alerta.UsuarioCriadorId.ToLower() != "system" &&
                    alerta.UsuarioCriadorId.ToLower() != "sistema")
                {
                    var criador = await _unitOfWork.AspNetUsers.GetFirstOrDefaultAsync(
                        u => u.Id == alerta.UsuarioCriadorId
                    );

                    if (criador != null)
                    {
                        if (!string.IsNullOrWhiteSpace(criador.NomeCompleto))
                        {
                            nomeCriador = criador.NomeCompleto;
                        }
                        else if (!string.IsNullOrWhiteSpace(criador.Email))
                        {
                            nomeCriador = criador.Email.Split('@')[0];
                        }
                        else
                        {
                            nomeCriador = criador.UserName;
                        }
                    }
                    else
                    {
                        nomeCriador = alerta.UsuarioCriadorId;
                    }
                }

                var tipoInfo = ObterInfoTipo(alerta.TipoAlerta);
                var prioridadeInfo = ObterInfoPrioridade(alerta.Prioridade);

                bool expirado = alerta.DataExpiracao.HasValue && alerta.DataExpiracao.Value < DateTime.Now;

                return Ok(new
                {
                    success = true,
                    debug = debugInfo,
                    data = new
                    {
                        alertaId = alerta.AlertasFrotiXId,
                        titulo = alerta.Titulo,
                        descricao = alerta.Descricao,
                        tipoAlerta = tipoInfo.Nome,
                        tipo = tipoInfo.Nome,
                        prioridade = prioridadeInfo.Nome,
                        iconeCss = tipoInfo.Icone,
                        corBadge = tipoInfo.Cor,
                        dataCriacao = alerta.DataInsercao,
                        dataInsercao = alerta.DataInsercao,
                        dataExibicao = alerta.DataExibicao,
                        dataExpiracao = alerta.DataExpiracao,
                        ativo = alerta.Ativo,
                        expirado = expirado,
                        tempoNoAr = tempoNoArFormatado,
                        nomeCriador = nomeCriador,
                        usuarioCriadorId = alerta.UsuarioCriadorId,
                        totalDestinatarios = totalDestinatarios,
                        totalNotificados = totalNotificados,
                        aguardandoNotificacao = aguardandoNotificacao,
                        leram = usuariosLeram,
                        naoLeram = usuariosNaoLeram,
                        apagaram = usuariosApagaram,
                        percentualLeitura = percentualLeitura,
                        usuarios = usuariosDetalhes,
                        viagemId = alerta.ViagemId,
                        manutencaoId = alerta.ManutencaoId,
                        motoristaId = alerta.MotoristaId,
                        veiculoId = alerta.VeiculoId
                    }
                });
            }
            catch (Exception ex)
            {
                // (IA) Registro centralizado de erro FrotiX.
                _log.Error($"[AlertasFrotiXController] Erro em GetDetalhesAlerta: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetDetalhesAlerta", ex);
                return StatusCode(500, new
                {
                    success = false,
                    mensagem = "Erro ao obter detalhes do alerta",
                    erro = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterInfoTipo (Auxiliar)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna metadados visuais (nome, ícone, cor) por tipo de alerta.          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • tipo (TipoAlerta)                                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • (Nome, Icone, Cor)                                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private (string Nome, string Icone, string Cor) ObterInfoTipo(TipoAlerta tipo)
        {
            // (IA) Mapeamento de tipos para iconografia Duotone e paleta FrotiX.
            return tipo switch
            {
                TipoAlerta.Agendamento => ("Agendamento", "fa-duotone fa-calendar-check", "#0ea5e9"),
                TipoAlerta.Manutencao => ("Manutencao", "fa-duotone fa-wrench", "#f59e0b"),
                TipoAlerta.Motorista => ("Motorista", "fa-duotone fa-user-tie", "#14b8a6"),
                TipoAlerta.Veiculo => ("Veiculo", "fa-duotone fa-car", "#7c3aed"),
                TipoAlerta.Anuncio => ("Anuncio", "fa-duotone fa-bullhorn", "#dc2626"),
                // TipoAlerta.Aniversario => ("Aniversario", "fa-duotone fa-cake-candles", "#ec4899"), // Não existe na enum
                _ => ("Geral", "fa-duotone fa-bell", "#6b7280")
            };
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterInfoPrioridade (Auxiliar)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna metadados visuais por prioridade do alerta.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • prioridade (PrioridadeAlerta)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • (Nome, Cor)                                                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private (string Nome, string Cor) ObterInfoPrioridade(PrioridadeAlerta prioridade)
        {
            // (IA) Mapeamento de prioridade para escala de cores semântica FrotiX.
            return prioridade switch
            {
                PrioridadeAlerta.Baixa => ("Baixa", "#0ea5e9"),
                PrioridadeAlerta.Media => ("Media", "#f59e0b"),
                PrioridadeAlerta.Alta => ("Alta", "#dc2626"),
                _ => ("Normal", "#6b7280")
            };
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterInfoTipo (Auxiliar Overload int)                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Versão overload que aceita inteiro para metadados do tipo.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • tipo (int)                                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • (Nome, Icone, Cor)                                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
		private (string Nome, string Icone, string Cor) ObterInfoTipo(int tipo)
		{
			return tipo switch
			{
				1 => ("Agendamento", "fa-duotone fa-calendar-check", "#0ea5e9"),
				2 => ("Manutencao", "fa-duotone fa-wrench", "#f59e0b"),
				3 => ("Motorista", "fa-duotone fa-user-tie", "#14b8a6"),
				4 => ("Veiculo", "fa-duotone fa-car", "#7c3aed"),
				5 => ("Anuncio", "fa-duotone fa-bullhorn", "#dc2626"),
				6 => ("Aniversario", "fa-duotone fa-cake-candles", "#ec4899"),
				_ => ("Geral", "fa-duotone fa-bell", "#6b7280")
			};
		}

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterInfoPrioridade (Auxiliar Overload int)                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Versão overload que aceita inteiro para metadados de prioridade.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • prioridade (int)                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • (Nome, Cor)                                                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
		private (string Nome, string Cor) ObterInfoPrioridade(int prioridade)
		{
			return prioridade switch
			{
				1 => ("Baixa", "#0ea5e9"),
				2 => ("Media", "#f59e0b"),
				3 => ("Alta", "#dc2626"),
				4 => ("Critica", "#991b1b"),
				_ => ("Normal", "#6b7280")
			};
		}

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetAlertasAtivos (GET)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém estatísticas de alertas ativos (não lidos/pendentes).                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com lista de alertas ativos.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("GetAlertasAtivos")]
        public async Task<IActionResult> GetAlertasAtivos()
        {
            try
            {
                // (IA) Identificação do usuário para busca de notificações pendentes.
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst("sub")?.Value
                                ?? User.Identity?.Name;

                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Ok(new List<object>());
                }

                var alertas = await _alertasRepo.GetTodosAlertasAtivosAsync();

                if (alertas == null || !alertas.Any())
                {
                    return Ok(new List<object>());
                }

                // (IA) Cruzamento de dados para identificar alertas destinados ao usuário logado.
                var alertasDoUsuario = alertas
                    .Where(a => a.AlertasUsuarios != null &&
                                a.AlertasUsuarios.Any(au =>
                                    au.UsuarioId == usuarioId &&
                                    !au.Lido &&
                                    !au.Apagado))
                    .ToList();

                var alertasParaNotificar = alertasDoUsuario
                    .Where(a => a.AlertasUsuarios.Any(au =>
                        au.UsuarioId == usuarioId &&
                        !au.Notificado))
                    .ToList();

                if (alertasParaNotificar.Any())
                {
                    // (IA) Atualização em lote para marcar alertas como notificados.
                    foreach (var alerta in alertasParaNotificar)
                    {
                        var alertaUsuario = alerta.AlertasUsuarios
                            .First(au => au.UsuarioId == usuarioId);

                        alertaUsuario.Notificado = true;
                        alertaUsuario.DataNotificacao = DateTime.Now;

                        _unitOfWork.AlertasUsuario.Update(alertaUsuario);
                    }

                    await _unitOfWork.SaveAsync();
                }

                var resultado = alertasDoUsuario.Select(a => new
                {
                    alertaId = a.AlertasFrotiXId,
                    titulo = a.Titulo,
                    descricao = a.Descricao,
                    mensagem = a.Descricao,
                    tipo = (int)a.TipoAlerta,
                    prioridade = (int)a.Prioridade,
                    dataInsercao = a.DataInsercao,
                    usuarioCriadorId = a.UsuarioCriadorId,
                    iconeCss = Alerta.GetIconePrioridade(a.Prioridade),
                    corBadge = Alerta.GetCorHexPrioridade(a.Prioridade),
                    textoBadge = a.Prioridade.ToString(),
                    severidade = a.Prioridade.ToString()
                }).ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                // (IA) Log de erro FrotiX.
                _log.Error($"[AlertasFrotiXController] Erro em GetAlertasAtivos: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetAlertasAtivos", ex);
                return StatusCode(500, new
                {
                    success = false,
                    mensagem = "Erro ao obter alertas ativos",
                    erro = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetQuantidadeNaoLidos (GET)                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna contagem de alertas não lidos para o badge do menu.               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com quantidade.                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("GetQuantidadeNaoLidos")]
        public async Task<IActionResult> GetQuantidadeNaoLidos()
        {
            try
            {
                // (IA) Identificação do usuário para contagem de novas mensagens.
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst("sub")?.Value
                                ?? User.Identity?.Name;

                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Ok(new { quantidade = 0 });
                }

                var quantidade = await _alertasRepo.GetQuantidadeAlertasNaoLidosAsync(usuarioId);
                return Ok(new { quantidade });
            }
            catch (Exception ex)
            {
                // (IA) Retornamos 0 em caso de erro para não quebrar a UI, mas registramos no log.
                _log.Error($"[AlertasFrotiXController] Erro em GetQuantidadeNaoLidos: {ex.Message}", ex);
                return Ok(new { quantidade = 0 });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MarcarComoLido (POST)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Marca um alerta como lido para o usuário atual.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • alertaId (Guid)                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com sucesso/erro.                                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("MarcarComoLido/{alertaId}")]
        public async Task<IActionResult> MarcarComoLido(Guid alertaId)
        {
            try
            {
                // (IA) Identificação granular do usuário logado.
                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? User.FindFirstValue("sub")
                                ?? User.FindFirstValue(ClaimTypes.Name)
                                ?? User.Identity?.Name;

                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Unauthorized(new { success = false, message = "Usuário não autenticado" });
                }

                // (IA) Localização do registro de vínculo entre alerta e usuário.
                var alertaUsuario = await _unitOfWork.AlertasUsuario.GetFirstOrDefaultAsync(
                    au => au.AlertasFrotiXId == alertaId && au.UsuarioId == usuarioId
                );

                if (alertaUsuario == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Alerta não encontrado para este usuário",
                        alertaId = alertaId,
                        usuarioId = usuarioId
                    });
                }

                // (IA) Atualização do status e auditoria de leitura.
                alertaUsuario.Lido = true;
                alertaUsuario.DataLeitura = DateTime.Now;

                _unitOfWork.AlertasUsuario.Update(alertaUsuario);
                await _unitOfWork.SaveAsync();

                return Ok(new { success = true, message = "Alerta marcado como lido" });
            }
            catch (Exception ex)
            {
                // (IA) Registro de falha na operação de leitura.
                _log.Error($"[AlertasFrotiXController] Erro em MarcarComoLido: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "MarcarComoLido", ex);
                return StatusCode(500, new
                {
                    success = false,
                    mensagem = "Erro ao marcar alerta como lido",
                    erro = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Salvar (POST)                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Salva ou atualiza alerta no sistema, tratando recorrências se necessário. ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dto (AlertaDto)                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com resultado da operação.                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("Salvar")]
        [Route("Salvar")]
        public async Task<IActionResult> Salvar([FromBody] AlertaDto dto)
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst("sub")?.Value
                                ?? User.Identity?.Name;

                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Usuário não identificado"
                    });
                }

                if (string.IsNullOrWhiteSpace(dto.Titulo))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "O título é obrigatório"
                    });
                }

                if (string.IsNullOrWhiteSpace(dto.Descricao))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "A descrição é obrigatória"
                    });
                }

                // (IA) Verifica se é um alerta recorrente (tipos 4-8)
                if (dto.TipoExibicao >= 4 && dto.TipoExibicao <= 8)
                {
                    // (IA) Calcula datas recorrentes conforme regras de negócio
                    var datasRecorrentes = CalcularDatasRecorrentes(dto);
                    
                    if (datasRecorrentes.Count == 0)
                    {
                        return BadRequest(new
                        {
                            success = false,
                            message = "Nenhuma data válida encontrada para o alerta recorrente"
                        });
                    }

                    var alertasCriados = new List<Guid>();
                    var recorrenciaAlertaId = Guid.NewGuid();

                    foreach (var dataExibicao in datasRecorrentes)
                    {
                        var alerta = new AlertasFrotiX
                        {
                            AlertasFrotiXId = Guid.NewGuid(),
                            Titulo = dto.Titulo,
                            Descricao = dto.Descricao,
                            TipoAlerta = (TipoAlerta)dto.TipoAlerta,
                            Prioridade = (PrioridadeAlerta)dto.Prioridade,
                            TipoExibicao = (TipoExibicaoAlerta)dto.TipoExibicao,
                            DataExibicao = dataExibicao,
                            HorarioExibicao = dto.HorarioExibicao,
                            DataExpiracao = dto.DataExpiracao,
                            DiasSemana = dto.DiasSemana,
                            DiaMesRecorrencia = dto.DiaMesRecorrencia,
                            DataInsercao = DateTime.Now,
                            UsuarioCriadorId = usuarioId,
                            Ativo = true,
                            ViagemId = dto.ViagemId,
                            ManutencaoId = dto.ManutencaoId,
                            MotoristaId = dto.MotoristaId,
                            VeiculoId = dto.VeiculoId,
                            RecorrenciaAlertaId = recorrenciaAlertaId,
                            Monday = dto.DiasSemana?.Contains("1") ?? false,
                            Tuesday = dto.DiasSemana?.Contains("2") ?? false,
                            Wednesday = dto.DiasSemana?.Contains("3") ?? false,
                            Thursday = dto.DiasSemana?.Contains("4") ?? false,
                            Friday = dto.DiasSemana?.Contains("5") ?? false,
                            Saturday = dto.DiasSemana?.Contains("6") ?? false,
                            Sunday = dto.DiasSemana?.Contains("0") ?? false,
                            DatasSelecionadas = dto.DatasSelecionadas
                        };

                        var usuariosParaNotificar = dto.UsuariosIds ?? new List<string>();
                        await _alertasRepo.CriarAlertaAsync(alerta, usuariosParaNotificar);

                        alertasCriados.Add(alerta.AlertasFrotiXId);

                        await NotificarUsuariosNovoAlerta(alerta, dto.UsuariosIds);
                    }

                    return Ok(new
                    {
                        success = true,
                        message = $"{alertasCriados.Count} alertas recorrentes criados com sucesso",
                        alertasIds = alertasCriados,
                        quantidadeAlertas = alertasCriados.Count,
                        recorrenciaAlertaId = recorrenciaAlertaId
                    });
                }

                AlertasFrotiX alertaUnico;
                bool isEdicao = dto.AlertasFrotiXId != Guid.Empty;

                if (isEdicao)
                {
                    alertaUnico = await _unitOfWork.AlertasFrotiX.GetFirstOrDefaultAsync(
                        a => a.AlertasFrotiXId == dto.AlertasFrotiXId,
                        includeProperties: "AlertasUsuarios"
                    );

                    if (alertaUnico == null)
                    {
                        return NotFound(new
                        {
                            success = false,
                            message = "Alerta não encontrado"
                        });
                    }

                    alertaUnico.Titulo = dto.Titulo;
                    alertaUnico.Descricao = dto.Descricao;
                    alertaUnico.TipoAlerta = (TipoAlerta)dto.TipoAlerta;
                    alertaUnico.Prioridade = (PrioridadeAlerta)dto.Prioridade;
                    alertaUnico.TipoExibicao = (TipoExibicaoAlerta)dto.TipoExibicao;
                    alertaUnico.DataExibicao = dto.DataExibicao;
                    alertaUnico.HorarioExibicao = dto.HorarioExibicao;
                    alertaUnico.DataExpiracao = dto.DataExpiracao;
                    alertaUnico.DiasSemana = dto.DiasSemana;
                    alertaUnico.DiaMesRecorrencia = dto.DiaMesRecorrencia;
                    alertaUnico.ViagemId = dto.ViagemId;
                    alertaUnico.ManutencaoId = dto.ManutencaoId;
                    alertaUnico.MotoristaId = dto.MotoristaId;
                    alertaUnico.VeiculoId = dto.VeiculoId;

                    _unitOfWork.AlertasFrotiX.Update(alertaUnico);

                    var associacoesAntigas = await _unitOfWork.AlertasUsuario.GetAllAsync(
                        filter: au => au.AlertasFrotiXId == alertaUnico.AlertasFrotiXId
                    );

                    foreach (var assoc in associacoesAntigas)
                    {
                        _unitOfWork.AlertasUsuario.Remove(assoc);
                    }

                    var usuariosParaNotificar = dto.UsuariosIds ?? new List<string>();
                    if (usuariosParaNotificar.Count > 0)
                    {
                        foreach (var uid in usuariosParaNotificar)
                        {
                            var alertaUsuario = new AlertasUsuario
                            {
                                AlertasFrotiXId = alertaUnico.AlertasFrotiXId,
                                UsuarioId = uid,
                                Lido = false,
                                Notificado = false
                            };
                            _unitOfWork.AlertasUsuario.Add(alertaUsuario);
                        }
                    }

                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    alertaUnico = new AlertasFrotiX
                    {
                        AlertasFrotiXId = Guid.NewGuid(),
                        Titulo = dto.Titulo,
                        Descricao = dto.Descricao,
                        TipoAlerta = (TipoAlerta)dto.TipoAlerta,
                        Prioridade = (PrioridadeAlerta)dto.Prioridade,
                        TipoExibicao = (TipoExibicaoAlerta)dto.TipoExibicao,
                        DataExibicao = dto.DataExibicao,
                        HorarioExibicao = dto.HorarioExibicao,
                        DataExpiracao = dto.DataExpiracao,
                        DiasSemana = dto.DiasSemana,
                        DiaMesRecorrencia = dto.DiaMesRecorrencia,
                        DataInsercao = DateTime.Now,
                        UsuarioCriadorId = usuarioId,
                        Ativo = true,
                        ViagemId = dto.ViagemId,
                        ManutencaoId = dto.ManutencaoId,
                        MotoristaId = dto.MotoristaId,
                        VeiculoId = dto.VeiculoId
                    };

                    var usuariosParaNotificar = dto.UsuariosIds ?? new List<string>();
                    await _alertasRepo.CriarAlertaAsync(alertaUnico, usuariosParaNotificar);
                }

                await NotificarUsuariosNovoAlerta(alertaUnico, dto.UsuariosIds);

                return Ok(new
                {
                    success = true,
                    message = isEdicao ? "Alerta atualizado com sucesso" : "Alerta criado com sucesso",
                    alertaId = alertaUnico.AlertasFrotiXId
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AlertasFrotiXController] Erro em Salvar: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "Salvar", ex);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro ao salvar alerta: " + ex.Message
                });
            }
        }

        public class AlertaDto
        {
            public Guid AlertasFrotiXId { get; set; }
            public string Titulo { get; set; }
            public string Descricao { get; set; }
            public int TipoAlerta { get; set; }
            public int Prioridade { get; set; }
            public int TipoExibicao { get; set; }
            public DateTime? DataExibicao { get; set; }
            public TimeSpan? HorarioExibicao { get; set; }
            public DateTime? DataExpiracao { get; set; }
            public string DiasSemana { get; set; }
            public int? DiaMesRecorrencia { get; set; }
            public string DatasSelecionadas { get; set; }
            public Guid? ViagemId { get; set; }
            public Guid? ManutencaoId { get; set; }
            public Guid? MotoristaId { get; set; }
            public Guid? VeiculoId { get; set; }
            public List<string> UsuariosIds { get; set; }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: NotificarUsuariosNovoAlerta (Private Async)              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Envia notificação em tempo real (SignalR) sobre novo alerta.             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • alerta (AlertasFrotiX)                                                  ║
        /// ║    • usuariosIds (List<string>)                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Task                                                                    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private async Task NotificarUsuariosNovoAlerta(AlertasFrotiX alerta, List<string> usuariosIds)
        {
            try
            {
                var alertaPayload = new
                {
                    alertaId = alerta.AlertasFrotiXId,
                    titulo = alerta.Titulo,
                    descricao = alerta.Descricao,
                    tipo = alerta.TipoAlerta,
                    prioridade = alerta.Prioridade,
                    iconeCss = ObterIconePorTipo(alerta.TipoAlerta),
                    corBadge = ObterCorPorTipo(alerta.TipoAlerta),
                    textoBadge = ObterTextoPorTipo(alerta.TipoAlerta),
                    dataInsercao = alerta.DataInsercao
                };

                if (usuariosIds == null || usuariosIds.Count == 0)
                {
                    await _hubContext.Clients.All.SendAsync("NovoAlerta", alertaPayload);
                }
                else
                {
                    foreach (var usuarioId in usuariosIds)
                    {
                        await _hubContext.Clients.User(usuarioId).SendAsync("NovoAlerta", alertaPayload);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("[AlertasFrotiXController] Erro em NotificarUsuariosNovoAlerta: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "NotificarUsuariosNovoAlerta", ex);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetHistoricoAlertas (GET)                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém histórico geral de alertas, incluindo status de leitura.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON sumarizado para grid.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("GetHistoricoAlertas")]
        public async Task<IActionResult> GetHistoricoAlertas()
        {
            try
            {
                var alertas = await _alertasRepo.GetTodosAlertasComLeituraAsync();

                var resultado = alertas.Select(a =>
                {
                    var ultimaLeitura = a.AlertasUsuarios
                        .Where(au => au.Lido && au.DataLeitura.HasValue)
                        .OrderByDescending(au => au.DataLeitura)
                        .FirstOrDefault();

                    return new
                    {
                        alertaId = a.AlertasFrotiXId,
                        titulo = a.Titulo,
                        descricao = a.Descricao,
                        tipo = ObterTextoPorTipo(a.TipoAlerta),
                        prioridade = a.Prioridade.ToString(),
                        dataInsercao = a.DataInsercao.HasValue ? a.DataInsercao.Value.ToString("dd/MM/yyyy HH:mm") : "-",
                        dataLeitura = ultimaLeitura?.DataLeitura?.ToString("dd/MM/yyyy HH:mm") ?? "",
                        icone = ObterIconePorTipo(a.TipoAlerta),
                        totalLeituras = a.AlertasUsuarios.Count(au => au.Lido),
                        totalUsuarios = a.AlertasUsuarios.Count
                    };
                }).ToList();

                return Ok(new
                {
                    data = resultado
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AlertasFrotiXController] Erro em GetHistoricoAlertas: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetHistoricoAlertas", ex);
                return Ok(new
                {
                    data = new List<object>()
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterIconePorTipo (Auxiliar)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna a classe CSS do ícone conforme o tipo de alerta.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • tipo (TipoAlerta)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • string (classe CSS FontAwesome Duotone)                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private string ObterIconePorTipo(TipoAlerta tipo)
        {
            return tipo switch
            {
                TipoAlerta.Agendamento => "fa-duotone fa-calendar-check",
                TipoAlerta.Manutencao => "fa-duotone fa-screwdriver-wrench",
                TipoAlerta.Motorista => "fa-duotone fa-id-card-clip",
                TipoAlerta.Veiculo => "fa-duotone fa-car-bus",
                TipoAlerta.Anuncio => "fa-duotone fa-bullhorn",
                _ => "fa-duotone fa-circle-info"
            };
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetAlertasFinalizados (GET)                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna lista paginada de alertas finalizados (inativos).                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dias (int?), pagina (int), tamanhoPagina (int)                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados paginados.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("GetAlertasFinalizados")]
        public async Task<IActionResult> GetAlertasFinalizados(
            [FromQuery] int? dias = 30,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 20)
        {
            try
            {
                var dataLimite = DateTime.Now.AddDays(-(dias ?? 30));

                var todosAlertas = await _unitOfWork.AlertasFrotiX.GetAllAsync(
                    filter: a => !a.Ativo &&
                                 a.DataDesativacao.HasValue &&
                                 a.DataDesativacao.Value >= dataLimite
                );

                var alertasOrdenados = todosAlertas
                    .OrderByDescending(a => a.DataDesativacao)
                    .ToList();

                var total = alertasOrdenados.Count;

                var alertasPaginados = alertasOrdenados
                    .Skip((pagina - 1) * tamanhoPagina)
                    .Take(tamanhoPagina)
                    .Select(a => new
                    {
                        alertaId = a.AlertasFrotiXId,
                        titulo = a.Titulo,
                        descricao = a.Descricao,
                        tipo = ObterTextoPorTipo(a.TipoAlerta),
                        prioridade = a.Prioridade.ToString(),
                        dataInsercao = a.DataInsercao,
                        dataFinalizacao = a.DataDesativacao,
                        finalizadoPor = a.DesativadoPor,
                        motivo = a.MotivoDesativacao
                    })
                    .ToList();

                return Ok(new
                {
                    success = true,
                    total = total,
                    pagina = pagina,
                    tamanhoPagina = tamanhoPagina,
                    totalPaginas = (int)Math.Ceiling((double)total / tamanhoPagina),
                    dados = alertasPaginados
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AlertasFrotiXController] Erro em GetAlertasFinalizados: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetAlertasFinalizados", ex);
                return StatusCode(500, new
                {
                    success = false,
                    mensagem = "Erro ao buscar histórico",
                    erro = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DarBaixaAlerta (POST)                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Finaliza (dá baixa) em um alerta manualmente, inativando-o.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • alertaId (Guid)                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com resultado da operação.                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("DarBaixaAlerta/{alertaId}")]
        public async Task<IActionResult> DarBaixaAlerta(Guid alertaId)
        {
            try
            {
                var alerta = await _unitOfWork.AlertasFrotiX.GetFirstOrDefaultAsync(
                    a => a.AlertasFrotiXId == alertaId
                );

                if (alerta == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        mensagem = "Alerta não encontrado"
                    });
                }

                if (!alerta.Ativo)
                {
                    return BadRequest(new
                    {
                        success = false,
                        mensagem = "Este alerta já foi finalizado anteriormente"
                    });
                }

                var usuarioAtual = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                  ?? User.FindFirst("sub")?.Value
                                  ?? User.Identity?.Name
                                  ?? "Sistema";

                alerta.Ativo = false;
                alerta.DataDesativacao = DateTime.Now;
                alerta.DesativadoPor = usuarioAtual;
                alerta.MotivoDesativacao = "Baixa realizada pelo usuário";

                _unitOfWork.AlertasFrotiX.Update(alerta);
                await _unitOfWork.SaveAsync();

                return Ok(new
                {
                    success = true,
                    mensagem = "Baixa do alerta realizada com sucesso",
                    alertaId = alertaId,
                    dataFinalizacao = DateTime.Now,
                    finalizadoPor = usuarioAtual
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AlertasFrotiXController] Erro em DarBaixaAlerta: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "DarBaixaAlerta", ex);
                return StatusCode(500, new
                {
                    success = false,
                    mensagem = "Erro interno ao processar a baixa do alerta",
                    erro = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetMeusAlertas (GET)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém lista de alertas destinados ao usuário autenticado.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com alertas personalizados.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("GetMeusAlertas")]
        public async Task<IActionResult> GetMeusAlertas()
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst("sub")?.Value
                                ?? User.FindFirst(ClaimTypes.Name)?.Value
                                ?? User.Identity?.Name;

                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Ok(new
                    {
                        data = new List<object>()
                    });
                }

                var alertasUsuario = await _unitOfWork.AlertasUsuario.GetAllAsync(
                    filter: au => au.UsuarioId == usuarioId,
                    includeProperties: "AlertasFrotiX"
                );

                var resultado = alertasUsuario
                    .Where(au => au.AlertasFrotiX != null)
                    .OrderByDescending(au => au.AlertasFrotiX.DataInsercao)
                    .Select(au => new
                    {
                        alertaId = au.AlertasFrotiXId,
                        titulo = au.AlertasFrotiX.Titulo,
                        descricao = au.AlertasFrotiX.Descricao,
                        tipo = ObterTextoPorTipo(au.AlertasFrotiX.TipoAlerta),
                        icone = ObterIconePorTipo(au.AlertasFrotiX.TipoAlerta),
                        notificado = au.Notificado,
                        notificadoTexto = au.Notificado ? "Sim" : "Não",
                        dataNotificacao = au.DataNotificacao?.ToString("dd/MM/yyyy HH:mm") ?? "-",
                        lido = au.Lido,
                        lidoTexto = au.Lido ? "Sim" : "Não",
                        dataLeitura = au.DataLeitura?.ToString("dd/MM/yyyy HH:mm") ?? "-",
                        prioridade = au.AlertasFrotiX.Prioridade.ToString(),
                        dataCriacao = au.AlertasFrotiX.DataInsercao?.ToString("dd/MM/yyyy HH:mm") ?? "-"
                    })
                    .ToList();

                return Ok(new
                {
                    data = resultado
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AlertasFrotiXController] Erro em GetMeusAlertas: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetMeusAlertas", ex);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro ao buscar meus alertas: " + ex.Message,
                    data = new List<object>()
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetAlertasInativos (GET)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna alertas inativos com estatísticas de leitura.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON para gestão de inativos.                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("GetAlertasInativos")]
        public async Task<IActionResult> GetAlertasInativos()
        {
            try
            {
                var alertasInativos = await _unitOfWork.AlertasFrotiX.GetAllAsync(
                    filter: a => !a.Ativo,
                    includeProperties: "AlertasUsuarios"
                );

                var resultado = alertasInativos
                    .OrderByDescending(a => a.DataDesativacao ?? a.DataInsercao)
                    .Select(a =>
                    {
                        var totalUsuarios = a.AlertasUsuarios.Count();
                        var totalNotificados = a.AlertasUsuarios.Count(au => au.Notificado);
                        var totalLeram = a.AlertasUsuarios.Count(au => au.Lido);

                        var percentualLeitura = totalNotificados > 0
                            ? (double)totalLeram / totalNotificados * 100
                            : 0;

                        return new
                        {
                            alertaId = a.AlertasFrotiXId,
                            titulo = a.Titulo,
                            descricao = a.Descricao,
                            tipo = ObterTextoPorTipo(a.TipoAlerta),
                            prioridade = a.Prioridade.ToString(),
                            dataInsercao = a.DataInsercao?.ToString("dd/MM/yyyy HH:mm"),
                            dataDesativacao = a.DataDesativacao?.ToString("dd/MM/yyyy HH:mm") ?? "-",
                            icone = ObterIconePorTipo(a.TipoAlerta),
                            percentualLeitura = percentualLeitura,
                            totalUsuarios = totalUsuarios,
                            totalNotificados = totalNotificados,
                            totalLeram = totalLeram
                        };
                    })
                    .ToList();

                return Ok(new
                {
                    data = resultado
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AlertasFrotiXController] Erro em GetAlertasInativos: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetAlertasInativos", ex);
                return Ok(new
                {
                    data = new List<object>()
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetTodosAlertasAtivosGestao (GET)                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna alertas ativos para o painel de gestão administrativa.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • N/A                                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com alertas ativos.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("GetTodosAlertasAtivosGestao")]
        public async Task<IActionResult> GetTodosAlertasAtivosGestao()
        {
            try
            {
                var alertasAtivos = await _unitOfWork.AlertasFrotiX.GetAllAsync(
                    filter: a => a.Ativo,
                    includeProperties: "AlertasUsuarios"
                );

                if (alertasAtivos == null || !alertasAtivos.Any())
                {
                    return Ok(new List<object>());
                }

                var resultado = alertasAtivos.Select(a =>
                {
                    var totalUsuarios = a.AlertasUsuarios?.Count ?? 0;
                    var usuariosLeram = a.AlertasUsuarios?.Count(au => au.Lido) ?? 0;

                    return new
                    {
                        alertaId = a.AlertasFrotiXId,
                        titulo = a.Titulo,
                        descricao = a.Descricao,
                        mensagem = a.Descricao,
                        tipo = (int)a.TipoAlerta,
                        prioridade = (int)a.Prioridade,
                        dataInsercao = a.DataInsercao,
                        usuarioCriadorId = a.UsuarioCriadorId,
                        totalUsuarios = totalUsuarios,
                        usuariosLeram = usuariosLeram,
                        iconeCss = Alerta.GetIconePrioridade(a.Prioridade),
                        corBadge = Alerta.GetCorHexPrioridade(a.Prioridade),
                        textoBadge = a.Prioridade.ToString(),
                        severidade = a.Prioridade.ToString()
                    };
                }).ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _log.Error("[AlertasFrotiXController] Erro em GetTodosAlertasAtivosGestao: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs", "GetTodosAlertasAtivosGestao", ex);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro ao buscar alertas ativos para gestão",
                    erro = ex.Message
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: VerificarPermissaoBaixa (GET)                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Verifica se o usuário autenticado pode dar baixa em alerta específico.   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • alertaId (Guid)                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com permissão.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("VerificarPermissaoBaixa/{alertaId}")]
        public async Task<IActionResult> VerificarPermissaoBaixa(Guid alertaId)
        {
            try
            {
                var alerta = await _unitOfWork.AlertasFrotiX.GetFirstOrDefaultAsync(
                    a => a.AlertasFrotiXId == alertaId
                );

                var usuarioAtual = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                  ?? User.FindFirst("sub")?.Value
                                  ?? User.Identity?.Name;

                var ehCriador = alerta.UsuarioCriadorId == usuarioAtual;
                var ehAdmin = User.IsInRole("Admin") || User.IsInRole("Administrador");

                var podeDarBaixa = ehCriador || ehAdmin;

                return Ok(new
                {
                    podeDarBaixa = podeDarBaixa
                });
            }
            catch (Exception ex)
            {
                _log.Error("[AlertasFrotiXController] Erro em VerificarPermissaoBaixa: {ex.Message}", ex);
                Alerta.TratamentoErroComLinha(
                    "AlertasFrotiXController.cs",
                    "VerificarPermissaoBaixa",
                    ex
                );
                return StatusCode(500);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterCorPorTipo (Auxiliar)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna cor hexadecimal associada ao tipo de alerta.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • tipo (TipoAlerta)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • string (hexadecimal)                                                    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private string ObterCorPorTipo(TipoAlerta tipo)
        {
            return tipo switch
            {
                TipoAlerta.Agendamento => "#0ea5e9",
                TipoAlerta.Manutencao => "#f59e0b",
                TipoAlerta.Motorista => "#14b8a6",
                TipoAlerta.Veiculo => "#7c3aed",
                TipoAlerta.Anuncio => "#dc2626",
                _ => "#6c757d"
            };
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: CalcularDatasRecorrentes (Auxiliar)                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Calcula datas recorrentes conforme tipo de exibição e parâmetros do DTO. ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • dto (AlertaDto)                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • List<DateTime>                                                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private List<DateTime> CalcularDatasRecorrentes(AlertaDto dto)
        {
            var datas = new List<DateTime>();
            
            if (!dto.DataExibicao.HasValue)
                return datas;

            var dataBase = dto.DataExibicao.Value;
            var dataFinal = dto.DataExpiracao ?? dataBase.AddYears(1); // (IA) Default: 1 ano

            // (IA) TipoExibicao: 4=Diário, 5=Semanal, 6=Quinzenal, 7=Mensal, 8=Dias Variados
            switch (dto.TipoExibicao)
            {
                case 4: // Diário
                    for (var data = dataBase; data <= dataFinal; data = data.AddDays(1))
                    {
                        datas.Add(data);
                    }
                    break;

                case 5: // Semanal
                    var diasSemana = ParseDiasSemana(dto.DiasSemana);
                    for (var data = dataBase; data <= dataFinal; data = data.AddDays(1))
                    {
                        if (diasSemana.Contains(data.DayOfWeek))
                        {
                            datas.Add(data);
                        }
                    }
                    break;

                case 6: // Quinzenal
                    for (var data = dataBase; data <= dataFinal; data = data.AddDays(14))
                    {
                        datas.Add(data);
                    }
                    break;

                case 7: // Mensal
                    if (dto.DiaMesRecorrencia.HasValue)
                    {
                        var diaMes = dto.DiaMesRecorrencia.Value;
                        for (var data = dataBase; data <= dataFinal; data = data.AddMonths(1))
                        {
                            var ultimoDiaMes = DateTime.DaysInMonth(data.Year, data.Month);
                            var diaValido = Math.Min(diaMes, ultimoDiaMes);
                            var dataRecorrente = new DateTime(data.Year, data.Month, diaValido);
                            if (dataRecorrente >= dataBase && dataRecorrente <= dataFinal)
                            {
                                datas.Add(dataRecorrente);
                            }
                        }
                    }
                    else
                    {
                        // (IA) Usa o mesmo dia do mês da data base se não especificado
                        for (var data = dataBase; data <= dataFinal; data = data.AddMonths(1))
                        {
                            datas.Add(data);
                        }
                    }
                    break;

                case 8: // Dias Variados
                    if (!string.IsNullOrWhiteSpace(dto.DatasSelecionadas))
                    {
                        var datasStr = dto.DatasSelecionadas.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var dataStr in datasStr)
                        {
                            if (DateTime.TryParse(dataStr.Trim(), out DateTime dataExibicao))
                            {
                                if (dataExibicao >= dataBase && dataExibicao <= dataFinal)
                                {
                                    datas.Add(dataExibicao);
                                }
                            }
                        }
                    }
                    break;
            }

            return datas.Distinct().OrderBy(d => d).ToList();
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ParseDiasSemana (Auxiliar)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Converte string de dias da semana para lista de DayOfWeek.               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • diasSemanaStr (string)                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • List<DayOfWeek>                                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private List<DayOfWeek> ParseDiasSemana(string diasSemanaStr)
        {
            var dias = new List<DayOfWeek>();
            
            if (string.IsNullOrWhiteSpace(diasSemanaStr))
                return dias;

            var diasArray = diasSemanaStr.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var dia in diasArray)
            {
                if (int.TryParse(dia.Trim(), out int diaNum) && diaNum >= 0 && diaNum <= 6)
                {
                    dias.Add((DayOfWeek)diaNum);
                }
            }

            return dias;
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterTextoPorTipo (Auxiliar)                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna representação textual do tipo de alerta.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • tipo (TipoAlerta)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • string amigável                                                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private string ObterTextoPorTipo(TipoAlerta tipo)
        {
            return tipo switch
            {
                TipoAlerta.Agendamento => "Agendamento",
                TipoAlerta.Manutencao => "Manutenção",
                TipoAlerta.Motorista => "Motorista",
                TipoAlerta.Veiculo => "Veículo",
                TipoAlerta.Anuncio => "Anúncio",
                _ => "Aniversario"
            };
        }
    }
}

public class ExportarDetalhesDto
{
    public Guid AlertaId { get; set; }
    public string Titulo { get; set; }
    public List<UsuarioExportDto> Usuarios { get; set; }
}

public class UsuarioExportDto
{
    public string NomeUsuario { get; set; }
    public string Email { get; set; }
    public bool Lido { get; set; }
    public bool Apagado { get; set; }
    public DateTime? DataNotificacao { get; set; }
    public DateTime? DataLeitura { get; set; }
}


