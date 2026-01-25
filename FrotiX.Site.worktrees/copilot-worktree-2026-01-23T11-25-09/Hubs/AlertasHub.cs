using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using FrotiX.Helpers;

namespace FrotiX.Hubs
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║                                                                              ║
    /// ║  🔔 CLASSE: AlertasHub (SignalR Hub)                                         ║
    /// ║                                                                              ║
    /// ║  DESCRIÇÃO:                                                                  ║
    /// ║  Hub SignalR para comunicação em tempo real de alertas/notificações.        ║
    /// ║  Permite que o servidor envie notificações push para usuários conectados.   ║
    /// ║                                                                              ║
    /// ║  ARQUITETURA SignalR:                                                        ║
    /// ║  - Cliente (JavaScript) conecta ao Hub via /alertasHub.                     ║
    /// ║  - Servidor (C#) envia mensagens via IHubContext<AlertasHub>.               ║
    /// ║  - Conexões persistentes (WebSocket, SSE, Long Polling).                    ║
    /// ║                                                                              ║
    /// ║  FUNCIONALIDADES:                                                            ║
    /// ║  1. Notificar usuários específicos sobre novos alertas.                     ║
    /// ║  2. Marcar alertas como lidos em tempo real.                                ║
    /// ║  3. Gerenciar grupos por usuário (user_<usuarioId>).                        ║
    /// ║                                                                              ║
    /// ║  IDENTIFICAÇÃO DE USUÁRIOS:                                                  ║
    /// ║  - Context.UserIdentifier: GUID do usuário (AspNetUsers.Id).                ║
    /// ║  - Fornecido via EmailBasedUserIdProvider (configurado no Startup).         ║
    /// ║                                                                              ║
    /// ║  EVENTOS DO SERVIDOR PARA CLIENTE:                                          ║
    /// ║  - "ReceberAlerta": Novo alerta disponível.                                 ║
    /// ║  - "AlertaMarcadoComoLido": Confirmação de leitura.                         ║
    /// ║                                                                              ║
    /// ║  ÚLTIMA ATUALIZAÇÃO: 19/01/2026                                              ║
    /// ║                                                                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    /// </summary>
    public class AlertasHub : Hub
    {
        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Marca um alerta como lido (chamado pelo cliente).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Método público que o cliente JavaScript pode invocar via SignalR.
        /// │    Envia confirmação de volta ao usuário específico.
        /// │
        /// │ FLUXO:
        /// │    1. Cliente chama: connection.invoke("MarcarComoLido", alertaId, userId)
        /// │    2. Servidor processa (atualiza banco via Controller/Service)
        /// │    3. Servidor notifica cliente via "AlertaMarcadoComoLido"
        /// │
        /// │ PARÂMETROS:
        /// │    -> alertaId: GUID do alerta a ser marcado como lido.
        /// │    -> usuarioId: GUID do usuário que leu o alerta.
        /// │
        /// │ COMUNICAÇÃO:
        /// │    Clients.User(): Envia apenas para o usuário específico (unicast).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task MarcarComoLido(string alertaId, string usuarioId)
        {
            try
            {
                // ─────────────────────────────────────────────────────────────────────────────
                // [NOTIFICAÇÃO] Envia confirmação para o usuário que leu o alerta
                // ─────────────────────────────────────────────────────────────────────────────
                // IMPORTANTE: Clients.User() usa Context.UserIdentifier (configurado via IUserIdProvider)
                await Clients.User(usuarioId).SendAsync("AlertaMarcadoComoLido", alertaId);
            }
            catch (Exception ex)
            {
                // ⚠️ AVISO: Erro aqui não bloqueia UI, apenas loga no console
                Alerta.TratamentoErroComLinha("AlertasHub.cs", "MarcarComoLido", ex);
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Evento de conexão de cliente (lifecycle hook).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Método automático chamado pelo SignalR quando um cliente estabelece conexão.
        /// │    Adiciona o usuário autenticado ao seu grupo exclusivo para comunicação.
        /// │
        /// │ FLUXO DE AUTENTICAÇÃO:
        /// │    1. Cliente conecta (navegador carrega página com SignalR).
        /// │    2. SignalR valida cookie de autenticação (.AspNetCore.Identity).
        /// │    3. IUserIdProvider extrai GUID do usuário (EmailBasedUserIdProvider).
        /// │    4. Hub adiciona conexão ao grupo "user_<GUID>".
        /// │
        /// │ CONCEITO DE GRUPOS:
        /// │    - Cada usuário tem um grupo único: "user_<UsuarioId>".
        /// │    - Servidor pode enviar mensagens via: Clients.Group("user_123").SendAsync(...)
        /// │    - Permite broadcast seletivo (apenas para usuário específico).
        /// │
        /// │ LOGS DE DEBUG:
        /// │    Console.WriteLine() com detalhes de UserIdentifier, Name, IsAuthenticated.
        /// │    Útil para diagnosticar problemas de autenticação/identificação.
        /// │
        /// │ TRATAMENTO DE ERROS:
        /// │    Re-lança exceção para que SignalR rejeite a conexão (evita conexões zumbis).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            try
            {
                // ─────────────────────────────────────────────────────────────────────────────
                // [DEBUG] Log de auditoria de conexão
                // ─────────────────────────────────────────────────────────────────────────────
                Console.WriteLine("=== CLIENTE CONECTANDO ===");

                var usuarioId = Context.UserIdentifier;  // GUID do usuário (fornecido por IUserIdProvider)
                Console.WriteLine($"UserIdentifier: {usuarioId ?? "NULL"}");

                var userName = Context.User?.Identity?.Name;  // Email ou nome de login
                Console.WriteLine($"User Name: {userName ?? "NULL"}");

                var isAuthenticated = Context.User?.Identity?.IsAuthenticated ?? false;
                Console.WriteLine($"IsAuthenticated: {isAuthenticated}");

                // ─────────────────────────────────────────────────────────────────────────────
                // [GRUPOS] Adiciona usuário ao seu grupo exclusivo (se identificado)
                // ─────────────────────────────────────────────────────────────────────────────
                // IMPORTANTE: Só adiciona se UserIdentifier não for nulo (usuário autenticado).
                // EXEMPLO: Usuário "abc-123-def" entra no grupo "user_abc-123-def".
                if (!string.IsNullOrEmpty(usuarioId))
                {
                    var groupName = $"user_{usuarioId}";
                    Console.WriteLine($"Adicionando ao grupo: {groupName}");
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                }
                else
                {
                    // ⚠️ ALERTA: Usuário conectado sem identificação (provável erro de config)
                    Console.WriteLine("AVISO: UserIdentifier está nulo!");
                }

                await base.OnConnectedAsync();
                Console.WriteLine("=== CONEXÃO BEM-SUCEDIDA ===");
            }
            catch (Exception ex)
            {
                // ❌ ERRO CRÍTICO: Falha na conexão (loga e re-lança)
                Alerta.TratamentoErroComLinha("AlertasHub.cs", "OnConnectedAsync", ex);
                throw; // Re-lançar para que SignalR rejeite a conexão
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Evento de desconexão de cliente (lifecycle hook).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Método automático chamado quando um cliente fecha a conexão SignalR.
        /// │    Remove o usuário do seu grupo exclusivo para liberar recursos.
        /// │
        /// │ CENÁRIOS DE DESCONEXÃO:
        /// │    1. Cliente fecha navegador/aba.
        /// │    2. Cliente navega para outra página (sem SignalR).
        /// │    3. Timeout de conexão (inatividade prolongada).
        /// │    4. Erro de rede.
        /// │
        /// │ LIMPEZA DE RECURSOS:
        /// │    Remove conexão do grupo "user_<UsuarioId>" para evitar memory leaks.
        /// │
        /// │ PARÂMETROS:
        /// │    -> exception: Se não nulo, indica desconexão por erro (logar razão).
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                // ─────────────────────────────────────────────────────────────────────────────
                // [DEBUG] Log de auditoria de desconexão
                // ─────────────────────────────────────────────────────────────────────────────
                Console.WriteLine("=== CLIENTE DESCONECTANDO ===");

                if (exception != null)
                {
                    // ⚠️ Desconexão com erro (timeout, rede, etc)
                    Console.WriteLine($"Razão: {exception.Message}");
                }

                var usuarioId = Context.UserIdentifier;

                // ─────────────────────────────────────────────────────────────────────────────
                // [LIMPEZA] Remove usuário do grupo (se identificado)
                // ─────────────────────────────────────────────────────────────────────────────
                if (!string.IsNullOrEmpty(usuarioId))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{usuarioId}");
                }

                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                // ❌ ERRO: Falha na desconexão (raro, mas possível)
                Alerta.TratamentoErroComLinha("AlertasHub.cs", "OnDisconnectedAsync", ex);
            }
        }
    }
}
