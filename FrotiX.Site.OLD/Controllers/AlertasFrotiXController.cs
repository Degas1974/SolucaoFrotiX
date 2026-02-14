/* ****************************************************************************************
 * ⚡ ARQUIVO: AlertasFrotiXController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar alertas do sistema e notificações em tempo real via SignalR.
 *
 * 📥 ENTRADAS     : Identificadores de usuário, filtros de status e ações de leitura.
 *
 * 📤 SAÍDAS       : JSON com alertas, contador de não lidos e status das operações.
 *
 * 🔗 CHAMADA POR  : Frontend de alertas e notificações.
 *
 * 🔄 CHAMA        : IAlertasFrotiXRepository, AlertasHub (SignalR).
 *
 * 📦 DEPENDÊNCIAS : IUnitOfWork, SignalR, repositório de alertas.
 *
 * 📝 OBSERVAÇÕES  : Controller ignora antiforgery por operar como API.
 **************************************************************************************** */

using FrotiX.Hubs;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        /****************************************************************************************
         * ⚡ FUNÇÃO: AlertasFrotiXController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependencias do sistema de alertas
         * 📥 ENTRADAS     : [IUnitOfWork] unitOfWork - Repositorio unificado
         *                   [IAlertasFrotiXRepository] alertasRepo - Repositorio especializado
         *                   [IHubContext<AlertasHub>] hubContext - SignalR para tempo real
         * 📤 SAÍDAS       : Instancia do controller configurada
         * 🔗 CHAMADA POR  : ASP.NET Core Dependency Injection
         ****************************************************************************************/
        public AlertasFrotiXController(
            IUnitOfWork unitOfWork ,
            IAlertasFrotiXRepository alertasRepo ,
            IHubContext<AlertasHub> hubContext)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _alertasRepo = alertasRepo;
                _hubContext = hubContext;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AlertasFrotiXController.cs" ,
                    "AlertasFrotiXController" ,
                    error
                );
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetDetalhesAlerta
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter detalhes completos de um alerta incluindo estatisticas de
         *                   leitura, usuarios notificados, tempo no ar e informacoes do criador
         * 📥 ENTRADAS     : [Guid] id - ID do alerta
         * 📤 SAÍDAS       : [IActionResult] JSON com detalhes completos do alerta
         * 🔗 CHAMADA POR  : Modal de detalhes do alerta no frontend
         * 🔄 CHAMA        : AlertasFrotiX, AspNetUsers, AlertasUsuarios
         *
         * 📊 METRICAS CALCULADAS:
         *    - totalDestinatarios, totalNotificados, aguardandoNotificacao
         *    - usuariosLeram, usuariosNaoLeram, usuariosApagaram
         *    - percentualLeitura, tempoNoAr
         ****************************************************************************************/
        [HttpGet("GetDetalhesAlerta/{id}")]
        public async Task<IActionResult> GetDetalhesAlerta(Guid id)
        {
            try
            {
                // [DOC] Busca alerta com todos os relacionamentos necessarios
                var alerta = await _unitOfWork.AlertasFrotiX.GetFirstOrDefaultAsync(
                    a => a.AlertasFrotiXId == id ,
                    includeProperties: "AlertasUsuarios,Viagem,Manutencao,Veiculo,Motorista"
                );

                if (alerta == null)
                {
                    return NotFound(new
                    {
                        success = false ,
                        message = "Alerta não encontrado"
                    });
                }

                // [DOC] Debug info para desenvolvimento - contagem de usuarios
                var debugInfo = new
                {
                    alertasUsuariosCount = alerta.AlertasUsuarios?.Count ?? 0 ,
                    alertasUsuariosIsNull = alerta.AlertasUsuarios == null ,
                    totalLidosNoBanco = alerta.AlertasUsuarios?.Count(au => au.Lido) ?? 0
                };

                // [DOC] Monta lista de usuarios com status de cada um
                var usuariosDetalhes = new List<object>();

                foreach (var au in alerta.AlertasUsuarios)
                {
                    var usuario = await _unitOfWork.AspNetUsers.GetFirstOrDefaultAsync(
                        u => u.Id == au.UsuarioId
                    );

                    usuariosDetalhes.Add(new
                    {
                        usuarioId = au.UsuarioId ,
                        nomeUsuario = usuario?.UserName ?? "Usuário removido" ,
                        email = usuario?.Email ,
                        lido = au.Lido ,
                        dataLeitura = au.DataLeitura ,
                        dataNotificacao = au.DataNotificacao ,
                        notificado = au.Notificado ,
                        apagado = au.Apagado ,
                        dataApagado = au.DataApagado
                    });
                }

                // [DOC] Calcula metricas de engajamento do alerta
                var totalDestinatarios = alerta.AlertasUsuarios.Count;
                var totalNotificados = alerta.AlertasUsuarios.Count(au => au.Notificado);
                var aguardandoNotificacao = alerta.AlertasUsuarios.Count(au => !au.Notificado);
                var usuariosLeram = alerta.AlertasUsuarios.Count(au => au.Lido);
                var usuariosNaoLeram = alerta.AlertasUsuarios.Count(au => au.Notificado && !au.Lido && !au.Apagado);
                var usuariosApagaram = alerta.AlertasUsuarios.Count(au => au.Apagado);
                var percentualLeitura = totalNotificados > 0
                    ? Math.Round((double)usuariosLeram / totalNotificados * 100 , 1)
                    : 0;

                // [DOC] Calcula tempo que o alerta esta/esteve no ar
                var dataInicio = alerta.DataExibicao ?? alerta.DataInsercao;
                var dataFim = alerta.DataExpiracao ?? DateTime.Now;
                var tempoNoAr = dataFim - dataInicio;

                // [DOC] Formata tempo no ar de forma legivel (min, h min, d h min)
                string tempoNoArFormatado = "N/A";

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

                // [DOC] Busca nome do criador do alerta (pode ser usuario ou Sistema)
                string nomeCriador = "Sistema";

                if (!string.IsNullOrEmpty(alerta.UsuarioCriadorId) &&
                    alerta.UsuarioCriadorId.ToLower() != "system" &&
                    alerta.UsuarioCriadorId.ToLower() != "sistema")
                {
                    var criador = await _unitOfWork.AspNetUsers.GetFirstOrDefaultAsync(
                        u => u.Id == alerta.UsuarioCriadorId
                    );

                    if (criador != null)
                    {
                        // [DOC] Prioridade: NomeCompleto > Email (parte antes do @) > UserName
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

                // [DOC] Obtem informacoes formatadas de tipo e prioridade
                var tipoInfo = ObterInfoTipo(alerta.TipoAlerta);
                var prioridadeInfo = ObterInfoPrioridade(alerta.Prioridade);

                bool expirado = alerta.DataExpiracao.HasValue && alerta.DataExpiracao.Value < DateTime.Now;

                return Ok(new
                {
                    success = true ,
                    debug = debugInfo ,
                    data = new
                    {
                        alertaId = alerta.AlertasFrotiXId ,
                        titulo = alerta.Titulo ,
                        descricao = alerta.Descricao ,
                        tipoAlerta = tipoInfo.Nome ,
                        tipo = tipoInfo.Nome ,
                        prioridade = prioridadeInfo.Nome ,
                        iconeCss = tipoInfo.Icone ,
                        corBadge = tipoInfo.Cor ,
                        dataCriacao = alerta.DataInsercao ,
                        dataInsercao = alerta.DataInsercao ,
                        dataExibicao = alerta.DataExibicao ,
                        dataExpiracao = alerta.DataExpiracao ,
                        ativo = alerta.Ativo ,
                        expirado = expirado ,
                        tempoNoAr = tempoNoArFormatado ,
                        nomeCriador = nomeCriador ,
                        usuarioCriadorId = alerta.UsuarioCriadorId ,
                        totalDestinatarios = totalDestinatarios ,
                        totalNotificados = totalNotificados ,
                        aguardandoNotificacao = aguardandoNotificacao ,
                        leram = usuariosLeram ,
                        naoLeram = usuariosNaoLeram ,
                        apagaram = usuariosApagaram ,
                        percentualLeitura = percentualLeitura ,
                        usuarios = usuariosDetalhes ,
                        viagemId = alerta.ViagemId ,
                        manutencaoId = alerta.ManutencaoId ,
                        motoristaId = alerta.MotoristaId ,
                        veiculoId = alerta.VeiculoId
                    }
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AlertasFrotiXController.cs" ,
                    "GetDetalhesAlerta" ,
                    error
                );
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao buscar detalhes do alerta" ,
                    erro = error.Message
                });
            }
        }

        private (string Nome, string Icone, string Cor) ObterInfoTipo(TipoAlerta tipo)
        {
            return tipo switch
            {
                TipoAlerta.Agendamento => ("Agendamento", "fa-duotone fa-calendar-check", "#0ea5e9"),
                TipoAlerta.Manutencao => ("Manutenção", "fa-duotone fa-wrench", "#f59e0b"),
                TipoAlerta.Motorista => ("Motorista", "fa-duotone fa-user-tie", "#14b8a6"),
                TipoAlerta.Veiculo => ("Veículo", "fa-duotone fa-car", "#7c3aed"),
                TipoAlerta.Anuncio => ("Anúncio", "fa-duotone fa-bullhorn", "#dc2626"),
                TipoAlerta.Diversos => ("Diversos", "fa-duotone fa-circle-info", "#6b7280"),
                _ => ("Geral", "fa-duotone fa-bell", "#6b7280")
            };
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ObterInfoPrioridade (enum)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar nome e cor para exibicao de prioridade (enum PrioridadeAlerta)
         * 📥 ENTRADAS     : [PrioridadeAlerta] prioridade - Enum de prioridade
         * 📤 SAÍDAS       : (string Nome, string Cor) - Tupla com nome e cor hex
         ****************************************************************************************/
        private (string Nome, string Cor) ObterInfoPrioridade(PrioridadeAlerta prioridade)
        {
            return prioridade switch
            {
                PrioridadeAlerta.Baixa => ("Baixa", "#0ea5e9"),
                PrioridadeAlerta.Media => ("Média", "#f59e0b"),
                PrioridadeAlerta.Alta => ("Alta", "#dc2626"),
                _ => ("Normal", "#6b7280")
            };
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ObterInfoTipo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar nome, icone FontAwesome e cor para tipo de alerta
         * 📥 ENTRADAS     : [int] tipo - Codigo do tipo (1=Agendamento, 2=Manutencao, etc)
         * 📤 SAÍDAS       : (string Nome, string Icone, string Cor) - Tupla com info formatada
         ****************************************************************************************/
        private (string Nome, string Icone, string Cor) ObterInfoTipo(int tipo)
        {
            // [DOC] Tipos: 1=Agendamento, 2=Manutencao, 3=Motorista, 4=Veiculo, 5=Anuncio, 6=Diversos
            return tipo switch
            {
                1 => ("Agendamento", "fa-duotone fa-calendar-check", "#0ea5e9"),
                2 => ("Manutenção", "fa-duotone fa-wrench", "#f59e0b"),
                3 => ("Motorista", "fa-duotone fa-user-tie", "#14b8a6"),
                4 => ("Veículo", "fa-duotone fa-car", "#7c3aed"),
                5 => ("Anúncio", "fa-duotone fa-bullhorn", "#dc2626"),
                6 => ("Diversos", "fa-duotone fa-circle-info", "#6b7280"),
                _ => ("Geral", "fa-duotone fa-bell", "#6b7280")
            };
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ObterInfoPrioridade (int)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar nome e cor para exibicao de prioridade (int)
         * 📥 ENTRADAS     : [int] prioridade - Codigo da prioridade (1=Baixa a 4=Critica)
         * 📤 SAÍDAS       : (string Nome, string Cor) - Tupla com nome e cor hex
         ****************************************************************************************/
        private (string Nome, string Cor) ObterInfoPrioridade(int prioridade)
        {
            // [DOC] Prioridades: 1=Baixa(azul), 2=Media(amarelo), 3=Alta(vermelho), 4=Critica(vermelho escuro)
            return prioridade switch
            {
                1 => ("Baixa", "#0ea5e9"),
                2 => ("Média", "#f59e0b"),
                3 => ("Alta", "#dc2626"),
                4 => ("Crítica", "#991b1b"),
                _ => ("Normal", "#6b7280")
            };
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetAlertasAtivos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar alertas ativos nao lidos do usuario logado e marca-los
         *                   como notificados (primeira visualizacao)
         * 📥 ENTRADAS     : Usuario identificado via Claims (JWT/Cookie)
         * 📤 SAÍDAS       : [IActionResult] JSON com lista de alertas ativos nao lidos
         * 🔗 CHAMADA POR  : Componente de notificacoes no header (polling ou SignalR)
         * 🔄 CHAMA        : AlertasRepo.GetTodosAlertasAtivosAsync(), AlertasUsuario.Update()
         *
         * ⚡ COMPORTAMENTO:
         *    - Filtra alertas do usuario que ainda nao foram lidos nem apagados
         *    - Marca como Notificado=true na primeira visualizacao
         *    - Retorna dados formatados para exibicao no frontend
         ****************************************************************************************/
        [HttpGet("GetAlertasAtivos")]
        public async Task<IActionResult> GetAlertasAtivos()
        {
            try
            {
                // [DOC] Identifica usuario logado - tenta varios claims possiveis
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst("sub")?.Value
                                ?? User.Identity?.Name;

                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Ok(new List<object>());
                }

                // [DOC] Busca todos alertas ativos (dentro do periodo de vigencia)
                var alertas = await _alertasRepo.GetTodosAlertasAtivosAsync();

                if (alertas == null || !alertas.Any())
                {
                    return Ok(new List<object>());
                }

                // [DOC] Filtra apenas alertas do usuario que ainda nao foram lidos nem apagados
                var alertasDoUsuario = alertas
                    .Where(a => a.AlertasUsuarios != null &&
                                a.AlertasUsuarios.Any(au =>
                                    au.UsuarioId == usuarioId &&
                                    !au.Lido &&
                                    !au.Apagado))
                    .ToList();

                // [DOC] Identifica alertas que ainda nao foram notificados (primeira vez)
                var alertasParaNotificar = alertasDoUsuario
                    .Where(a => a.AlertasUsuarios.Any(au =>
                        au.UsuarioId == usuarioId &&
                        !au.Notificado))
                    .ToList();

                // [DOC] Marca como notificado - usuario esta vendo pela primeira vez
                if (alertasParaNotificar.Any())
                {
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

                // [DOC] Formata resultado para o frontend com icones, cores e badges
                var resultado = alertasDoUsuario.Select(a => new
                {
                    alertaId = a.AlertasFrotiXId ,
                    titulo = a.Titulo ,
                    descricao = a.Descricao ,
                    mensagem = a.Descricao ,
                    tipo = (int)a.TipoAlerta ,
                    prioridade = (int)a.Prioridade ,
                    dataInsercao = a.DataInsercao ,
                    usuarioCriadorId = a.UsuarioCriadorId ,
                    iconeCss = Alerta.GetIconePrioridade((PrioridadeAlerta)a.Prioridade) ,
                    corBadge = Alerta.GetCorHexPrioridade((PrioridadeAlerta)a.Prioridade) ,
                    textoBadge = a.Prioridade.ToString() ,
                    severidade = a.Prioridade.ToString()
                }).ToList();

                return Ok(resultado);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetAlertasAtivos" , error);
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao buscar alertas ativos" ,
                    erro = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetQuantidadeNaoLidos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar quantidade de alertas nao lidos do usuario para badge
         * 📥 ENTRADAS     : Usuario identificado via Claims
         * 📤 SAÍDAS       : [IActionResult] JSON { quantidade: int }
         * 🔗 CHAMADA POR  : Badge de notificacoes no header (polling)
         ****************************************************************************************/
        [HttpGet("GetQuantidadeNaoLidos")]
        public async Task<IActionResult> GetQuantidadeNaoLidos()
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst("sub")?.Value
                                ?? User.Identity?.Name;

                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Ok(new
                    {
                        quantidade = 0
                    });
                }

                var quantidade = await _alertasRepo.GetQuantidadeAlertasNaoLidosAsync(usuarioId);
                return Ok(new { quantidade });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetQuantidadeNaoLidos" , error);
                return Ok(new
                {
                    quantidade = 0
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: MarcarComoLido
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Marcar um alerta como lido pelo usuario, atualizando data de leitura
         * 📥 ENTRADAS     : [Guid] alertaId - ID do alerta
         * 📤 SAÍDAS       : [IActionResult] JSON { success, message }
         * 🔗 CHAMADA POR  : JavaScript quando usuario clica em um alerta
         * 🔄 CHAMA        : AlertasUsuario.GetFirstOrDefaultAsync(), Update(), SaveAsync()
         *
         * ⚠️  VALIDAÇÕES:
         *    - Usuario deve estar autenticado
         *    - Alerta deve existir para este usuario especifico
         ****************************************************************************************/
        [HttpPost("MarcarComoLido/{alertaId}")]
        public async Task<IActionResult> MarcarComoLido(Guid alertaId)
        {
            try
            {
                // [DOC] Identifica usuario logado - tenta varios claims possiveis
                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? User.FindFirstValue("sub")
                                ?? User.FindFirstValue(ClaimTypes.Name)
                                ?? User.Identity?.Name;

                Console.WriteLine($"🔍 AlertaId: {alertaId}");
                Console.WriteLine($"🔍 UsuarioId: {usuarioId ?? "NULL"}");

                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Unauthorized(new
                    {
                        success = false ,
                        message = "Usuário não autenticado"
                    });
                }

                // [DOC] Busca registro AlertaUsuario (vinculo entre alerta e usuario)
                var alertaUsuario = await _unitOfWork.AlertasUsuario.GetFirstOrDefaultAsync(
                    au => au.AlertasFrotiXId == alertaId && au.UsuarioId == usuarioId
                );

                if (alertaUsuario == null)
                {
                    // [DOC] Debug para identificar problema - log detalhado
                    Console.WriteLine($"❌ AlertaUsuario NÃO ENCONTRADO!");
                    Console.WriteLine($"   Buscando por AlertasFrotiXId={alertaId} e UsuarioId={usuarioId}");

                    var existeAlerta = await _unitOfWork.AlertasUsuario.GetFirstOrDefaultAsync(
                        au => au.AlertasFrotiXId == alertaId
                    );

                    if (existeAlerta != null)
                    {
                        Console.WriteLine($"⚠️ Alerta existe, mas não para este usuário!");
                        Console.WriteLine($"⚠️ UsuarioId no banco: {existeAlerta.UsuarioId}");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Alerta não existe no sistema!");
                    }

                    return NotFound(new
                    {
                        success = false ,
                        message = "Alerta não encontrado para este usuário" ,
                        alertaId = alertaId ,
                        usuarioId = usuarioId
                    });
                }

                Console.WriteLine($"✅ AlertaUsuario ENCONTRADO!");
                Console.WriteLine($"✅ Lido antes: {alertaUsuario.Lido}");

                // [DOC] Atualiza status para lido e registra data/hora da leitura
                alertaUsuario.Lido = true;
                alertaUsuario.DataLeitura = DateTime.Now;

                _unitOfWork.AlertasUsuario.Update(alertaUsuario);

                Console.WriteLine($"✅ Chamando SaveAsync...");
                await _unitOfWork.SaveAsync();
                Console.WriteLine($"✅ SaveAsync concluído!");

                return Ok(new
                {
                    success = true ,
                    message = "Alerta marcado como lido"
                });
            }
            catch (Exception error)
            {
                Console.WriteLine($"❌ ERRO: {error.Message}");
                Console.WriteLine($"❌ Stack: {error.StackTrace}");

                Alerta.TratamentoErroComLinha(
                    "AlertasFrotiXController.cs" ,
                    "MarcarComoLido" ,
                    error
                );
                return StatusCode(500 , new
                {
                    success = false ,
                    message = error.Message ,
                    innerException = error.InnerException?.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Salvar
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Criar novo alerta ou atualizar existente (Upsert)
         * 📥 ENTRADAS     : [AlertaDto] dto - DTO com dados do alerta via JSON Body
         * 📤 SAÍDAS       : [IActionResult] JSON { success, alertaId, message }
         * 🔗 CHAMADA POR  : Modal de criacao/edicao de alerta no frontend
         * 🔄 CHAMA        : CriarAlertaBase(), GerarDatasRecorrencia(), NotificarUsuariosNovoAlerta()
         *
         * 📝 TIPOS SUPORTADOS:
         *    - [1-3] Simples: Cria alerta unico
         *    - [4-8] Recorrentes: Gera multiplos alertas conforme padrão
         *    - 4=Diario, 5=Semanal, 6=Quinzenal, 7=Mensal, 8=Datas especificas
         *
         * 🔔 NOTIFICAÇÃO: Envia push via SignalR para usuarios destinatarios
         ****************************************************************************************/
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
                        success = false ,
                        message = "Usuário não identificado"
                    });
                }

                if (string.IsNullOrWhiteSpace(dto.Titulo))
                {
                    return BadRequest(new
                    {
                        success = false ,
                        message = "O título é obrigatório"
                    });
                }

                if (string.IsNullOrWhiteSpace(dto.Descricao))
                {
                    return BadRequest(new
                    {
                        success = false ,
                        message = "A descrição é obrigatória"
                    });
                }

                // ============================================================
                // TIPOS RECORRENTES (4-8): Criar um alerta para cada data
                // ============================================================
                if (dto.TipoExibicao >= 4 && dto.TipoExibicao <= 8)
                {
                    var datasRecorrencia = GerarDatasRecorrencia(dto);

                    if (datasRecorrencia == null || datasRecorrencia.Count == 0)
                    {
                        return BadRequest(new
                        {
                            success = false ,
                            message = "Não foi possível gerar datas para a recorrência informada"
                        });
                    }

                    var alertasCriados = new List<Guid>();
                    var recorrenciaId = Guid.NewGuid();
                    var usuariosParaNotificar = dto.UsuariosIds ?? new List<string>();

                    foreach (var dataExibicao in datasRecorrencia)
                    {
                        var alerta = CriarAlertaBase(dto , usuarioId);
                        alerta.AlertasFrotiXId = alertasCriados.Count == 0 ? recorrenciaId : Guid.NewGuid();
                        alerta.RecorrenciaAlertaId = recorrenciaId;
                        alerta.DataExibicao = dataExibicao;

                        await _alertasRepo.CriarAlertaAsync(alerta , usuariosParaNotificar);
                        alertasCriados.Add(alerta.AlertasFrotiXId);

                        // Notificar usuários para cada alerta criado
                        await NotificarUsuariosNovoAlerta(alerta , dto.UsuariosIds);
                    }

                    return Ok(new
                    {
                        success = true ,
                        message = $"{alertasCriados.Count} alertas criados com sucesso (recorrência)" ,
                        alertasIds = alertasCriados ,
                        quantidadeAlertas = alertasCriados.Count
                    });
                }

                // ============================================================
                // OUTROS TIPOS: Criar um único alerta (comportamento normal)
                // ============================================================
                AlertasFrotiX alertaUnico;
                bool isEdicao = dto.AlertasFrotiXId != Guid.Empty;

                if (isEdicao)
                {
                    alertaUnico = await _unitOfWork.AlertasFrotiX.GetFirstOrDefaultAsync(
                        a => a.AlertasFrotiXId == dto.AlertasFrotiXId ,
                        includeProperties: "AlertasUsuarios"
                    );

                    if (alertaUnico == null)
                    {
                        return NotFound(new
                        {
                            success = false ,
                            message = "Alerta não encontrado"
                        });
                    }

                    alertaUnico.Titulo = dto.Titulo;
                    alertaUnico.Descricao = dto.Descricao;
                    alertaUnico.TipoAlerta = (int)dto.TipoAlerta;
                    alertaUnico.Prioridade = (int)dto.Prioridade;
                    alertaUnico.TipoExibicao = (int)dto.TipoExibicao;
                    alertaUnico.DataExibicao = dto.DataExibicao;
                    alertaUnico.HorarioExibicao = dto.HorarioExibicao;
                    alertaUnico.DataExpiracao = dto.DataExpiracao;
                    alertaUnico.DiasSemana = ConverterDiasSemanaTexto(dto.DiasSemana);
                    alertaUnico.DiaMesRecorrencia = dto.DiaMesRecorrencia;
                    alertaUnico.ViagemId = dto.ViagemId;
                    alertaUnico.ManutencaoId = dto.ManutencaoId;
                    alertaUnico.MotoristaId = dto.MotoristaId;
                    alertaUnico.VeiculoId = dto.VeiculoId;

                    AplicarDiasSemana(alertaUnico , dto);

                    _unitOfWork.AlertasFrotiX.Update(alertaUnico);

                    // Remover associações antigas
                    var associacoesAntigas = await _unitOfWork.AlertasUsuario.GetAllAsync(
                        filter: au => au.AlertasFrotiXId == alertaUnico.AlertasFrotiXId
                    );

                    foreach (var assoc in associacoesAntigas)
                    {
                        _unitOfWork.AlertasUsuario.Remove(assoc);
                    }

                    // Criar novas associações
                    var usuariosParaNotificar = dto.UsuariosIds ?? new List<string>();
                    if (usuariosParaNotificar.Count > 0)
                    {
                        foreach (var uid in usuariosParaNotificar)
                        {
                            var alertaUsuario = new AlertasUsuario
                            {
                                AlertasFrotiXId = alertaUnico.AlertasFrotiXId ,
                                UsuarioId = uid ,
                                Lido = false ,
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
                        AlertasFrotiXId = Guid.NewGuid() ,
                        Titulo = dto.Titulo ,
                        Descricao = dto.Descricao ,
                        TipoAlerta = (int)dto.TipoAlerta ,
                        Prioridade = (int)dto.Prioridade ,
                        TipoExibicao = (int)dto.TipoExibicao ,
                        DataExibicao = dto.DataExibicao ,
                        HorarioExibicao = dto.HorarioExibicao ,
                        DataExpiracao = dto.DataExpiracao ,
                        DiasSemana = ConverterDiasSemanaTexto(dto.DiasSemana) ,
                        DiaMesRecorrencia = dto.DiaMesRecorrencia ,
                        DataInsercao = DateTime.Now ,
                        UsuarioCriadorId = usuarioId ,
                        Ativo = true ,
                        ViagemId = dto.ViagemId ,
                        ManutencaoId = dto.ManutencaoId ,
                        MotoristaId = dto.MotoristaId ,
                        VeiculoId = dto.VeiculoId
                    };

                    AplicarDiasSemana(alertaUnico , dto);

                    var usuariosParaNotificar = dto.UsuariosIds ?? new List<string>();
                    await _alertasRepo.CriarAlertaAsync(alertaUnico , usuariosParaNotificar);
                }

                await NotificarUsuariosNovoAlerta(alertaUnico , dto.UsuariosIds);

                return Ok(new
                {
                    success = true ,
                    message = isEdicao ? "Alerta atualizado com sucesso" : "Alerta criado com sucesso" ,
                    alertaId = alertaUnico.AlertasFrotiXId
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "Salvar" , error);
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao salvar alerta: " + error.Message
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

            // Campos de Data/Hora
            public DateTime? DataExibicao { get; set; }

            public TimeSpan? HorarioExibicao { get; set; }
            public DateTime? DataExpiracao { get; set; }

            // Campos de Recorrência
            public List<int> DiasSemana { get; set; }           // Ex: [1,2,3,4,5] (seg-sex)

            public int? DiaMesRecorrencia { get; set; }      // Ex: 15 (dia 15 do mês)
            public string DatasSelecionadas { get; set; }    // Ex: "2025-11-20,2025-11-25,2025-12-01"

            // Vínculos
            public Guid? ViagemId { get; set; }

            public Guid? ManutencaoId { get; set; }
            public Guid? MotoristaId { get; set; }
            public Guid? VeiculoId { get; set; }

            // Usuários
            public List<string> UsuariosIds { get; set; }
        }

        private AlertasFrotiX CriarAlertaBase(AlertaDto dto , string usuarioId)
        {
            var alerta = new AlertasFrotiX
            {
                AlertasFrotiXId = Guid.NewGuid() ,
                Titulo = dto.Titulo ,
                Descricao = dto.Descricao ,
                TipoAlerta = (int)dto.TipoAlerta ,
                Prioridade = (int)dto.Prioridade ,
                TipoExibicao = (int)dto.TipoExibicao ,
                DataExibicao = dto.DataExibicao ,
                HorarioExibicao = dto.HorarioExibicao ,
                DataExpiracao = dto.DataExpiracao ,
                DiasSemana = ConverterDiasSemanaTexto(dto.DiasSemana) ,
                DiaMesRecorrencia = dto.DiaMesRecorrencia ,
                DatasSelecionadas = dto.DatasSelecionadas ,
                DataInsercao = DateTime.Now ,
                UsuarioCriadorId = usuarioId ,
                Ativo = true ,
                ViagemId = dto.ViagemId ,
                ManutencaoId = dto.ManutencaoId ,
                MotoristaId = dto.MotoristaId ,
                VeiculoId = dto.VeiculoId
            };

            AplicarDiasSemana(alerta , dto);

            return alerta;
        }

        private string ConverterDiasSemanaTexto(List<int> diasSemana)
        {
            if (diasSemana == null || diasSemana.Count == 0)
            {
                return string.Empty;
            }

            return string.Join("," , diasSemana.OrderBy(d => d));
        }

        private void AplicarDiasSemana(AlertasFrotiX alerta , AlertaDto dto)
        {
            alerta.Monday = false;
            alerta.Tuesday = false;
            alerta.Wednesday = false;
            alerta.Thursday = false;
            alerta.Friday = false;
            alerta.Saturday = false;
            alerta.Sunday = false;

            if (dto.TipoExibicao == 4)
            {
                alerta.Monday = true;
                alerta.Tuesday = true;
                alerta.Wednesday = true;
                alerta.Thursday = true;
                alerta.Friday = true;
                return;
            }

            if (dto.TipoExibicao == 5 || dto.TipoExibicao == 6)
            {
                if (dto.DiasSemana == null || dto.DiasSemana.Count == 0)
                {
                    return;
                }

                foreach (var dia in dto.DiasSemana)
                {
                    switch (dia)
                    {
                        case 0:
                            alerta.Sunday = true;
                            break;
                        case 1:
                            alerta.Monday = true;
                            break;
                        case 2:
                            alerta.Tuesday = true;
                            break;
                        case 3:
                            alerta.Wednesday = true;
                            break;
                        case 4:
                            alerta.Thursday = true;
                            break;
                        case 5:
                            alerta.Friday = true;
                            break;
                        case 6:
                            alerta.Saturday = true;
                            break;
                    }
                }
            }
        }

        private List<DateTime> GerarDatasRecorrencia(AlertaDto dto)
        {
            if (!dto.DataExibicao.HasValue && dto.TipoExibicao != 8)
            {
                return new List<DateTime>();
            }

            var dataInicial = (dto.DataExibicao ?? DateTime.Today).Date;
            var dataFinal = (dto.DataExpiracao ?? dto.DataExibicao ?? DateTime.Today).Date;

            if (dataFinal < dataInicial)
            {
                return new List<DateTime>();
            }

            switch (dto.TipoExibicao)
            {
                case 4:
                    return GerarDatasDiarias(dataInicial , dataFinal);
                case 5:
                    return GerarDatasSemanais(dataInicial , dataFinal , dto.DiasSemana , 1);
                case 6:
                    return GerarDatasSemanais(dataInicial , dataFinal , dto.DiasSemana , 2);
                case 7:
                    return GerarDatasMensais(dataInicial , dataFinal , dto.DiaMesRecorrencia ?? dataInicial.Day);
                case 8:
                    return GerarDatasVariadas(dto.DatasSelecionadas);
                default:
                    return new List<DateTime>();
            }
        }

        private List<DateTime> GerarDatasDiarias(DateTime dataInicial , DateTime dataFinal)
        {
            var datas = new List<DateTime>();

            for (var data = dataInicial; data <= dataFinal; data = data.AddDays(1))
            {
                if (data.DayOfWeek != DayOfWeek.Saturday && data.DayOfWeek != DayOfWeek.Sunday)
                {
                    datas.Add(data);
                }
            }

            return datas;
        }

        private List<DateTime> GerarDatasSemanais(DateTime dataInicial , DateTime dataFinal , List<int> diasSemana , int intervaloSemanas)
        {
            if (diasSemana == null || diasSemana.Count == 0)
            {
                return new List<DateTime>();
            }

            var datas = new HashSet<DateTime>();
            var dataAtual = dataInicial;

            while (dataAtual <= dataFinal)
            {
                foreach (var dia in diasSemana)
                {
                    var dataDia = ProximoDiaSemana(dataAtual , (DayOfWeek)dia);
                    if (dataDia >= dataInicial && dataDia <= dataFinal)
                    {
                        datas.Add(dataDia);
                    }
                }

                dataAtual = dataAtual.AddDays(7 * intervaloSemanas);
            }

            return datas.OrderBy(d => d).ToList();
        }

        private List<DateTime> GerarDatasMensais(DateTime dataInicial , DateTime dataFinal , int diaMes)
        {
            var datas = new List<DateTime>();
            var ano = dataInicial.Year;
            var mes = dataInicial.Month;

            while (new DateTime(ano , mes , 1) <= dataFinal)
            {
                var ultimoDiaMes = DateTime.DaysInMonth(ano , mes);
                var dia = Math.Min(diaMes , ultimoDiaMes);
                var data = new DateTime(ano , mes , dia);

                if (data >= dataInicial && data <= dataFinal)
                {
                    datas.Add(data);
                }

                if (mes == 12)
                {
                    mes = 1;
                    ano++;
                }
                else
                {
                    mes++;
                }
            }

            return datas;
        }

        private List<DateTime> GerarDatasVariadas(string datasSelecionadas)
        {
            if (string.IsNullOrWhiteSpace(datasSelecionadas))
            {
                return new List<DateTime>();
            }

            var datas = new List<DateTime>();
            var datasStr = datasSelecionadas.Split(',' , StringSplitOptions.RemoveEmptyEntries);

            foreach (var dataStr in datasStr)
            {
                if (DateTime.TryParse(dataStr.Trim() , out DateTime data))
                {
                    datas.Add(data.Date);
                }
            }

            return datas.OrderBy(d => d).ToList();
        }

        private DateTime ProximoDiaSemana(DateTime dataBase , DayOfWeek diaSemana)
        {
            var diff = ((int)diaSemana - (int)dataBase.DayOfWeek + 7) % 7;
            return dataBase.AddDays(diff);
        }

        private async Task NotificarUsuariosNovoAlerta(AlertasFrotiX alerta , List<string> usuariosIds)
        {
            try
            {
                var alertaPayload = new
                {
                    alertaId = alerta.AlertasFrotiXId ,
                    titulo = alerta.Titulo ,
                    descricao = alerta.Descricao ,
                    tipo = alerta.TipoAlerta ,
                    prioridade = alerta.Prioridade ,
                    iconeCss = ObterIconePorTipo((TipoAlerta)alerta.TipoAlerta) ,
                    corBadge = ObterCorPorTipo((TipoAlerta)alerta.TipoAlerta) ,
                    textoBadge = ObterTextoPorTipo((TipoAlerta)alerta.TipoAlerta) ,
                    dataInsercao = alerta.DataInsercao
                };

                if (usuariosIds == null || usuariosIds.Count == 0)
                {
                    await _hubContext.Clients.All.SendAsync("NovoAlerta" , alertaPayload);
                }
                else
                {
                    foreach (var usuarioId in usuariosIds)
                    {
                        await _hubContext.Clients.User(usuarioId).SendAsync("NovoAlerta" , alertaPayload);
                    }
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "NotificarUsuariosNovoAlerta" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetHistoricoAlertas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar historico completo de alertas com estatisticas de leitura
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : [IActionResult] JSON { data: [ alertas com leituras ] }
         * 🔗 CHAMADA POR  : DataTable de historico de alertas no painel admin
         * 🔄 CHAMA        : _alertasRepo.GetTodosAlertasComLeituraAsync()
         ****************************************************************************************/
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
                        alertaId = a.AlertasFrotiXId ,
                        titulo = a.Titulo ,
                        descricao = a.Descricao ,
                        tipo = ObterTextoPorTipo((TipoAlerta)a.TipoAlerta) ,
                        prioridade = a.Prioridade.ToString() ,
                        dataInsercao = a.DataInsercao.HasValue ? a.DataInsercao.Value.ToString("dd/MM/yyyy HH:mm") : "-" ,
                        dataLeitura = ultimaLeitura?.DataLeitura?.ToString("dd/MM/yyyy HH:mm") ?? "" ,
                        icone = ObterIconePorTipo((TipoAlerta)a.TipoAlerta) ,
                        totalLeituras = a.AlertasUsuarios.Count(au => au.Lido) ,
                        totalUsuarios = a.AlertasUsuarios.Count
                    };
                }).ToList();

                return Ok(new
                {
                    data = resultado
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetHistoricoAlertas" , error);
                return Ok(new
                {
                    data = new List<object>()
                });
            }
        }

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

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetAlertasFinalizados
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar alertas que foram finalizados/baixados com paginacao
         * 📥 ENTRADAS     : [int?] dias - Periodo em dias (default 30)
         *                   [int] pagina - Numero da pagina (default 1)
         *                   [int] tamanhoPagina - Registros por pagina (default 20)
         * 📤 SAÍDAS       : [IActionResult] JSON paginado com alertas finalizados
         * 🔗 CHAMADA POR  : DataTable de alertas finalizados
         ****************************************************************************************/
        [HttpGet("GetAlertasFinalizados")]
        public async Task<IActionResult> GetAlertasFinalizados(
            [FromQuery] int? dias = 30 ,
            [FromQuery] int pagina = 1 ,
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
                        alertaId = a.AlertasFrotiXId ,
                        titulo = a.Titulo ,
                        descricao = a.Descricao ,
                        tipo = ObterTextoPorTipo((TipoAlerta)a.TipoAlerta) ,
                        prioridade = a.Prioridade.ToString() ,
                        dataInsercao = a.DataInsercao ,
                        dataFinalizacao = a.DataDesativacao ,
                        finalizadoPor = a.DesativadoPor ,
                        motivo = a.MotivoDesativacao
                    })
                    .ToList();

                return Ok(new
                {
                    success = true ,
                    total = total ,
                    pagina = pagina ,
                    tamanhoPagina = tamanhoPagina ,
                    totalPaginas = (int)Math.Ceiling((double)total / tamanhoPagina) ,
                    dados = alertasPaginados
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetAlertasFinalizados" , error);
                return StatusCode(500 , new
                {
                    success = false ,
                    mensagem = "Erro ao buscar histórico" ,
                    erro = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: DarBaixaAlerta
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Finalizar/Desativar um alerta ativo (dar baixa)
         * 📥 ENTRADAS     : [Guid] alertaId - ID do alerta a finalizar
         * 📤 SAÍDAS       : [IActionResult] JSON { success, mensagem }
         * 🔗 CHAMADA POR  : Botao "Dar Baixa" no painel de gerenciamento de alertas
         * 🔄 CHAMA        : AlertasFrotiX.Update(), SaveAsync()
         *
         * ⚠️  VALIDAÇÕES:
         *    - Alerta deve existir
         *    - Alerta deve estar ativo (Ativo=true)
         *
         * 📝 COMPORTAMENTO:
         *    - Seta Ativo=false
         *    - Registra DataDesativacao, DesativadoPor, MotivoDesativacao
         ****************************************************************************************/
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
                        success = false ,
                        mensagem = "Alerta não encontrado"
                    });
                }

                if (!alerta.Ativo)
                {
                    return BadRequest(new
                    {
                        success = false ,
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
                    success = true ,
                    mensagem = "Baixa do alerta realizada com sucesso" ,
                    alertaId = alertaId ,
                    dataFinalizacao = DateTime.Now ,
                    finalizadoPor = usuarioAtual
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "DarBaixaAlerta" , error);
                return StatusCode(500 , new
                {
                    success = false ,
                    mensagem = "Erro interno ao processar a baixa do alerta" ,
                    erro = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetMeusAlertas
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar todos alertas destinados ao usuario logado
         * 📥 ENTRADAS     : Usuario identificado via Claims
         * 📤 SAÍDAS       : [IActionResult] JSON { data: [ alertas do usuario ] }
         * 🔗 CHAMADA POR  : Pagina "Meus Alertas" do usuario
         * 🔄 CHAMA        : AlertasUsuario.GetAllAsync() com include AlertasFrotiX
         *
         * 📊 DADOS RETORNADOS POR ALERTA:
         *    - alertaId, titulo, descricao, tipo, icone
         *    - notificado, dataNotificacao
         *    - lido, dataLeitura
         *    - prioridade, dataCriacao
         ****************************************************************************************/
        [HttpGet("GetMeusAlertas")]
        public async Task<IActionResult> GetMeusAlertas()
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? User.FindFirst("sub")?.Value
                                ?? User.FindFirst(ClaimTypes.Name)?.Value
                                ?? User.Identity?.Name;

                Console.WriteLine($"GetMeusAlertas - UsuarioId: {usuarioId ?? "NULL"}");

                if (string.IsNullOrEmpty(usuarioId))
                {
                    Console.WriteLine("UsuarioId está NULL - retornando lista vazia");
                    return Ok(new
                    {
                        data = new List<object>()
                    });
                }

                var alertasUsuario = await _unitOfWork.AlertasUsuario.GetAllAsync(
                    filter: au => au.UsuarioId == usuarioId ,
                    includeProperties: "AlertasFrotiX"
                );

                Console.WriteLine($"Total de alertas encontrados: {alertasUsuario.Count()}");

                var resultado = alertasUsuario
                    .Where(au => au.AlertasFrotiX != null)
                    .OrderByDescending(au => au.AlertasFrotiX.DataInsercao)
                    .Select(au => new
                    {
                        alertaId = au.AlertasFrotiXId ,
                        titulo = au.AlertasFrotiX.Titulo ,
                        descricao = au.AlertasFrotiX.Descricao ,
                        tipo = ObterTextoPorTipo((TipoAlerta)au.AlertasFrotiX.TipoAlerta) ,
                        icone = ObterIconePorTipo((TipoAlerta)au.AlertasFrotiX.TipoAlerta) ,
                        notificado = au.Notificado ,
                        notificadoTexto = au.Notificado ? "Sim" : "Não" ,
                        dataNotificacao = au.DataNotificacao?.ToString("dd/MM/yyyy HH:mm") ?? "-" ,
                        lido = au.Lido ,
                        lidoTexto = au.Lido ? "Sim" : "Não" ,
                        dataLeitura = au.DataLeitura?.ToString("dd/MM/yyyy HH:mm") ?? "-" ,
                        prioridade = au.AlertasFrotiX.Prioridade.ToString() ,
                        dataCriacao = au.AlertasFrotiX.DataInsercao
                    })
                    .ToList();

                Console.WriteLine($"Total de resultados processados: {resultado.Count}");

                return Ok(new
                {
                    data = resultado
                });
            }
            catch (Exception error)
            {
                Console.WriteLine($"ERRO em GetMeusAlertas: {error.Message}");
                Console.WriteLine($"Stack: {error.StackTrace}");

                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetMeusAlertas" , error);
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao buscar meus alertas: " + error.Message ,
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetAlertasInativos
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar todos alertas que foram desativados (baixados)
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : [IActionResult] JSON { data: [ alertas inativos com metricas ] }
         * 🔗 CHAMADA POR  : DataTable de alertas inativos no painel admin
         *
         * 📊 METRICAS CALCULADAS POR ALERTA:
         *    - totalUsuarios, totalNotificados, totalLeram
         *    - percentualLeitura = totalLeram/totalNotificados * 100
         ****************************************************************************************/
        [HttpGet("GetAlertasInativos")]
        public async Task<IActionResult> GetAlertasInativos()
        {
            try
            {
                var alertasInativos = await _unitOfWork.AlertasFrotiX.GetAllAsync(
                    filter: a => !a.Ativo ,
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
                            alertaId = a.AlertasFrotiXId ,
                            titulo = a.Titulo ,
                            descricao = a.Descricao ,
                            tipo = ObterTextoPorTipo((TipoAlerta)a.TipoAlerta) ,
                            prioridade = a.Prioridade.ToString() ,
                            dataInsercao = a.DataInsercao?.ToString("dd/MM/yyyy HH:mm") ,
                            dataDesativacao = a.DataDesativacao?.ToString("dd/MM/yyyy HH:mm") ?? "-" ,
                            icone = ObterIconePorTipo((TipoAlerta)a.TipoAlerta) ,
                            percentualLeitura = percentualLeitura ,
                            totalUsuarios = totalUsuarios ,
                            totalNotificados = totalNotificados ,
                            totalLeram = totalLeram
                        };
                    })
                    .ToList();

                return Ok(new
                {
                    data = resultado
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetAlertasInativos" , error);
                return Ok(new
                {
                    data = new List<object>()
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetTodosAlertasAtivosGestao
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar todos alertas ativos para o painel de gestao (admin)
         * 📥 ENTRADAS     : Nenhuma
         * 📤 SAÍDAS       : [IActionResult] JSON [ alertas ativos com estatisticas ]
         * 🔗 CHAMADA POR  : DataTable de gerenciamento de alertas ativos
         *
         * 📊 DADOS RETORNADOS POR ALERTA:
         *    - alertaId, titulo, descricao, tipo, prioridade, icone
         *    - dataInsercao, usuarioCriador
         *    - totalUsuarios, usuariosLeram (para barra de progresso)
         ****************************************************************************************/
        [HttpGet("GetTodosAlertasAtivosGestao")]
        public async Task<IActionResult> GetTodosAlertasAtivosGestao()
        {
            try
            {
                var alertasAtivos = await _unitOfWork.AlertasFrotiX.GetAllAsync(
                    filter: a => a.Ativo ,
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
                        alertaId = a.AlertasFrotiXId ,
                        titulo = a.Titulo ,
                        descricao = a.Descricao ,
                        mensagem = a.Descricao ,
                        tipo = (int)a.TipoAlerta ,
                        prioridade = (int)a.Prioridade ,
                        dataInsercao = a.DataInsercao ,
                        usuarioCriadorId = a.UsuarioCriadorId ,
                        totalUsuarios = totalUsuarios ,
                        usuariosLeram = usuariosLeram ,
                        iconeCss = Alerta.GetIconePrioridade((PrioridadeAlerta)a.Prioridade) ,
                        corBadge = Alerta.GetCorHexPrioridade((PrioridadeAlerta)a.Prioridade) ,
                        textoBadge = a.Prioridade.ToString() ,
                        severidade = a.Prioridade.ToString()
                    };
                }).ToList();

                return Ok(resultado);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXController.cs" , "GetTodosAlertasAtivosGestao" , error);
                return StatusCode(500 , new
                {
                    success = false ,
                    message = "Erro ao buscar alertas ativos para gestío" ,
                    erro = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: VerificarPermissaoBaixa
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Verificar se usuario logado pode dar baixa em um alerta
         * 📥 ENTRADAS     : [Guid] alertaId - ID do alerta a verificar
         * 📤 SAÍDAS       : [IActionResult] JSON { podeDarBaixa: bool }
         * 🔗 CHAMADA POR  : Frontend antes de habilitar botao "Dar Baixa"
         *
         * 🔐 REGRAS DE PERMISSAO:
         *    - Criador do alerta PODE dar baixa
         *    - Administradores (Admin/Administrador) PODEM dar baixa
         *    - Demais usuarios NAO podem dar baixa
         ****************************************************************************************/
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
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha(
                    "AlertasFrotiXController.cs" ,
                    "VerificarPermissaoBaixa" ,
                    error
                );
                return StatusCode(500);
            }
        }

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

        private string ObterTextoPorTipo(TipoAlerta tipo)
        {
            return tipo switch
            {
                TipoAlerta.Agendamento => "Agendamento",
                TipoAlerta.Manutencao => "Manutenção",
                TipoAlerta.Motorista => "Motorista",
                TipoAlerta.Veiculo => "Veículo",
                TipoAlerta.Anuncio => "Anúncio",
                _ => "Diversos"
            };
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
}
