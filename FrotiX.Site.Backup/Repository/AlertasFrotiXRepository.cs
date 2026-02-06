// ============================================================================
// AlertasFrotiXRepository.cs - VERSÃO CORRIGIDA
// Tratamento defensivo de NULLs nos Includes
// ============================================================================

using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Repository
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║                                                                              ║
    /// ║  🔔 REPOSITORY: AlertasFrotiXRepository                                      ║
    /// ║                                                                              ║
    /// ║  DESCRIÇÃO:                                                                  ║
    /// ║  Repository especializado para sistema de alertas do FrotiX.                ║
    /// ║  Gerencia AlertasFrotiX (alertas globais) e AlertasUsuario (leitura/notif). ║
    /// ║                                                                              ║
    /// ║  ENTIDADES:                                                                  ║
    /// ║  - AlertasFrotiX: Alerta global (título, mensagem, prioridade, tipo).       ║
    /// ║  - AlertasUsuario: Relação N:N (usuário específico + status lido/apagado).  ║
    /// ║                                                                              ║
    /// ║  FUNCIONALIDADES:                                                            ║
    /// ║  - Criação de alertas globais ou direcionados a usuários específicos.       ║
    /// ║  - Marcar como lido/apagado por usuário.                                    ║
    /// ║  - Contagem de alertas não lidos.                                           ║
    /// ║  - Notificações com base em tipo de exibição:                               ║
    /// ║    * AoAbrir: Exibido ao abrir sistema.                                     ║
    /// ║    * Horario: Exibido em horário específico (janela de 5 min).              ║
    /// ║    * DataHora: Exibido em data/hora específica (com expiração opcional).    ║
    /// ║                                                                              ║
    /// ║  CORREÇÃO CRÍTICA:                                                           ║
    /// ║  - REMOVIDO Include(Viagem) e Include(Manutencao) em GetTodosAlertasAtivosAsync(). ║
    /// ║  - Causa "Data is Null" quando campos nullable são NULL no banco.           ║
    /// ║  - Mantido apenas em GetAlertaComDetalhesAsync() (query individual).        ║
    /// ║                                                                              ║
    /// ║  INTEGRAÇÃO SIGNALR:                                                         ║
    /// ║  - AlertasHub.cs usa este repository para notificações real-time.           ║
    /// ║  - GetAlertasParaNotificarAsync() retorna alertas prontos para envio.       ║
    /// ║                                                                              ║
    /// ║  ÚLTIMA ATUALIZAÇÃO: 19/01/2026                                              ║
    /// ║                                                                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    /// </summary>
    public class AlertasFrotiXRepository : Repository<AlertasFrotiX>, IAlertasFrotiXRepository
    {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// Construtor que recebe DbContext e passa para classe base.
        /// </summary>
        public AlertasFrotiXRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetTodosAlertasAtivosAsync (Lista de Alertas Ativos)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna TODOS os alertas ativos do sistema (Ativo == true).
        /// │    Include: AlertasUsuarios (relação N:N com usuários).
        /// │
        /// │ CORREÇÃO CRÍTICA:
        /// │    REMOVIDO Include(Viagem) e Include(Manutencao) para evitar erro
        /// │    "Data is Null" quando entidades relacionadas têm campos nullable NULL.
        /// │
        /// │ ORDENAÇÃO:
        /// │    DataInsercao DESC (alertas mais recentes primeiro).
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<AlertasFrotiX> com AlertasUsuarios carregados.
        /// │
        /// │ TRATAMENTO DE ERRO:
        /// │    Retorna lista vazia em caso de exceção (nunca null).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<IEnumerable<AlertasFrotiX>> GetTodosAlertasAtivosAsync()
        {
            try
            {
                // [QUERY] - Busca alertas ativos com AlertasUsuarios (relação N:N)
                return await _db.AlertasFrotiX
                    .Include(a => a.AlertasUsuarios)
                    // REMOVIDO: .Include(a => a.Viagem)
                    // REMOVIDO: .Include(a => a.Manutencao)
                    // Esses includes causam erro quando Viagem/Manutencao têm campos NULL
                    .Where(a => a.Ativo)
                    .OrderByDescending(a => a.DataInsercao)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXRepository.cs", "GetTodosAlertasAtivosAsync", ex);
                return new List<AlertasFrotiX>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetTodosAlertasComLeituraAsync (Alertas com Leitura)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna alertas que possuem PELO MENOS UM usuário que já leu.
        /// │    Usado para análise de engajamento de alertas.
        /// │
        /// │ FILTRO:
        /// │    Any(au => au.Lido) - Pelo menos um AlertasUsuario.Lido == true.
        /// │
        /// │ USO:
        /// │    Relatórios de alertas lidos, análise de efetividade de notificações.
        /// │
        /// │ ORDENAÇÃO:
        /// │    DataInsercao DESC (alertas mais recentes primeiro).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<IEnumerable<AlertasFrotiX>> GetTodosAlertasComLeituraAsync()
        {
            try
            {
                // [QUERY] - Alertas com pelo menos um usuário que leu
                return await _db.AlertasFrotiX
                    .Include(a => a.AlertasUsuarios)
                    .Where(a => a.AlertasUsuarios.Any(au => au.Lido))
                    .OrderByDescending(a => a.DataInsercao)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXRepository.cs", "GetTodosAlertasComLeituraAsync", ex);
                return new List<AlertasFrotiX>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetQuantidadeAlertasNaoLidosAsync (Contador de Não Lidos)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Conta quantos alertas não lidos um usuário específico possui.
        /// │    Usado para badge de notificações (ícone de sino com número).
        /// │
        /// │ FILTROS:
        /// │    1. AlertasUsuario: UsuarioId == usuarioId AND !Lido AND !Apagado.
        /// │    2. JOIN AlertasFrotiX: Ativo == true (alerta ainda ativo).
        /// │
        /// │ QUERY JOIN:
        /// │    Usa Join() explícito para performance (evita Include desnecessário).
        /// │
        /// │ USO:
        /// │    - Header do sistema: Exibe badge "🔔 5" (5 alertas não lidos).
        /// │    - SignalR: Atualiza contador em tempo real quando novo alerta chega.
        /// │
        /// │ RETORNO:
        /// │    int: Quantidade de alertas não lidos (0 se nenhum).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<int> GetQuantidadeAlertasNaoLidosAsync(string usuarioId)
        {
            try
            {
                // [QUERY JOIN] - Conta AlertasUsuario não lidos do usuário com Alerta ativo
                return await _db.AlertasUsuario
                    .Where(au => au.UsuarioId == usuarioId && !au.Lido && !au.Apagado)
                    .Join(_db.AlertasFrotiX,
                        au => au.AlertasFrotiXId,
                        a => a.AlertasFrotiXId,
                        (au, a) => a)
                    .Where(a => a.Ativo)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXRepository.cs", "GetQuantidadeAlertasNaoLidosAsync", ex);
                return 0;
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: MarcarComoLidoAsync (Marcar Alerta como Lido)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Marca alerta como lido para um usuário específico.
        /// │    Atualiza AlertasUsuario.Lido = true e DataLeitura = Now.
        /// │
        /// │ PARÂMETROS:
        /// │    - alertaId: Guid do AlertasFrotiX.
        /// │    - usuarioId: string (AspNetUsers.Id).
        /// │
        /// │ TRACKING:
        /// │    AsTracking() necessário para EF Core rastrear e salvar alteração.
        /// │
        /// │ FLUXO:
        /// │    1. Busca AlertasUsuario (alertaId + usuarioId).
        /// │    2. Se encontrado: Marca Lido = true, DataLeitura = DateTime.Now.
        /// │    3. SaveChangesAsync() persiste alteração.
        /// │    4. Retorna true (sucesso) ou false (não encontrado).
        /// │
        /// │ INTEGRAÇÃO SIGNALR:
        /// │    AlertasHub.MarcarComoLido() chama este método ao usuário clicar no alerta.
        /// │
        /// │ RETORNO:
        /// │    bool: true (marcado com sucesso) ou false (AlertasUsuario não existe).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<bool> MarcarComoLidoAsync(Guid alertaId, string usuarioId)
        {
            try
            {
                // [QUERY TRACKING] - Busca AlertasUsuario específico (alertaId + usuarioId)
                var alertaUsuario = await _db.AlertasUsuario
                    .AsTracking()
                    .FirstOrDefaultAsync(au => au.AlertasFrotiXId == alertaId && au.UsuarioId == usuarioId);

                if (alertaUsuario != null)
                {
                    // [UPDATE] - Marca como lido e registra data/hora da leitura
                    alertaUsuario.Lido = true;
                    alertaUsuario.DataLeitura = DateTime.Now;
                    await _db.SaveChangesAsync();
                    return true; // Sucesso
                }

                return false; // AlertasUsuario não encontrado
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXRepository.cs", "MarcarComoLidoAsync", ex);
                return false;
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: CriarAlertaAsync (Criação de Novo Alerta)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Cria novo alerta global e vincula a usuários específicos ou TODOS.
        /// │    Método transacional (2 SaveChangesAsync).
        /// │
        /// │ PARÂMETROS:
        /// │    - alerta: AlertasFrotiX (título, mensagem, prioridade, tipo exibição).
        /// │    - usuariosIds: List<string> com IDs dos usuários (null = TODOS).
        /// │
        /// │ FLUXO:
        /// │    1. Insere AlertasFrotiX no banco (SaveChangesAsync #1).
        /// │    2. Se usuariosIds == null: Busca TODOS os usuários do sistema.
        /// │    3. Para cada usuário: Cria AlertasUsuario (vinculação N:N).
        /// │    4. SaveChangesAsync #2 persiste AlertasUsuario.
        /// │
        /// │ ALERTASUSUARIO CRIADO:
        /// │    - Lido: false (usuário ainda não leu).
        /// │    - Notificado: false (SignalR ainda não notificou).
        /// │    - Apagado: false (usuário não apagou).
        /// │    - DataNotificacao: null (será preenchida quando SignalR notificar).
        /// │
        /// │ USO:
        /// │    - Controllers: Criar alertas de manutenção, viagens canceladas, etc.
        /// │    - Background Services: Alertas automáticos (manutenção vencida, etc.).
        /// │
        /// │ RETORNO:
        /// │    AlertasFrotiX criado (com AlertasFrotiXId preenchido após SaveChanges).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<AlertasFrotiX> CriarAlertaAsync(AlertasFrotiX alerta, List<string> usuariosIds)
        {
            try
            {
                // [ETAPA 1] - Insere alerta principal no banco
                _db.AlertasFrotiX.Add(alerta);
                await _db.SaveChangesAsync(); // Gera AlertasFrotiXId

                // [ETAPA 2] - Determina lista de usuários
                if (usuariosIds == null || !usuariosIds.Any())
                {
                    // Se null/vazio: Busca TODOS os usuários (broadcast global)
                    var todosUsuarios = await _db.AspNetUsers.Select(u => u.Id).ToListAsync();
                    usuariosIds = todosUsuarios;
                }

                // [ETAPA 3] - Cria vinculação N:N (AlertasUsuario) para cada usuário
                foreach (var usuarioId in usuariosIds)
                {
                    var alertaUsuario = new AlertasUsuario
                    {
                        AlertasFrotiXId = alerta.AlertasFrotiXId,
                        UsuarioId = usuarioId,
                        Lido = false,            // Ainda não leu
                        Notificado = false,      // SignalR ainda não notificou
                        DataNotificacao = null,
                        Apagado = false          // Não foi apagado
                    };

                    _db.AlertasUsuario.Add(alertaUsuario);
                }

                // [ETAPA 4] - Persiste AlertasUsuario
                await _db.SaveChangesAsync();
                return alerta;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXRepository.cs", "CriarAlertaAsync", ex);
                throw; // Propaga exceção (transação falhou)
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: GetAlertaComDetalhesAsync (Detalhes Completos do Alerta)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna alerta específico com TODAS as navegações carregadas.
        /// │    Usado para exibição detalhada (modal, página de visualização).
        /// │
        /// │ INCLUDES:
        /// │    - AlertasUsuarios → Usuario (lista de usuários notificados).
        /// │    - Viagem (se alerta vinculado a viagem).
        /// │    - Manutencao → Veiculo (se alerta vinculado a manutenção).
        /// │    - Veiculo (vínculo direto).
        /// │    - Motorista (vínculo direto).
        /// │
        /// │ OBSERVAÇÃO CRÍTICA:
        /// │    AQUI é SEGURO usar Include(Viagem) e Include(Manutencao) porque:
        /// │    - É consulta de REGISTRO ÚNICO (FirstOrDefault).
        /// │    - Se Viagem/Manutencao forem NULL, EF Core retorna null (sem erro).
        /// │    - Erro "Data is Null" só ocorre em listas com múltiplos registros.
        /// │
        /// │ RETORNO:
        /// │    AlertasFrotiX completo ou null (se não encontrado).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<AlertasFrotiX> GetAlertaComDetalhesAsync(Guid alertaId)
        {
            try
            {
                return await _db.AlertasFrotiX
                    .Include(a => a.AlertasUsuarios)
                        .ThenInclude(au => au.Usuario)
                    .Include(a => a.Viagem)
                    .Include(a => a.Manutencao)
                        .ThenInclude(m => m.Veiculo)
                    .Include(a => a.Veiculo)
                    .Include(a => a.Motorista)
                    .FirstOrDefaultAsync(a => a.AlertasFrotiXId == alertaId);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXRepository.cs", "GetAlertaComDetalhesAsync", ex);
                return null;
            }
        }

        public async Task<bool> MarcarComoApagadoAsync(Guid alertaId, string usuarioId)
        {
            try
            {
                var alertaUsuario = await _db.AlertasUsuario
                    .AsTracking()
                    .FirstOrDefaultAsync(au => au.AlertasFrotiXId == alertaId && au.UsuarioId == usuarioId);

                if (alertaUsuario != null && !alertaUsuario.Lido)
                {
                    alertaUsuario.Apagado = true;
                    alertaUsuario.DataApagado = DateTime.Now;
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXRepository.cs", "MarcarComoApagadoAsync", ex);
                return false;
            }
        }

        public async Task<bool> DesativarAlertaAsync(Guid alertaId)
        {
            try
            {
                var alerta = await _db.AlertasFrotiX
                    .AsTracking()
                    .FirstOrDefaultAsync(a => a.AlertasFrotiXId == alertaId);

                if (alerta != null)
                {
                    alerta.Ativo = false;
                    alerta.DataDesativacao = DateTime.Now;
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXRepository.cs", "DesativarAlertaAsync", ex);
                return false;
            }
        }

        public async Task<IEnumerable<AlertasUsuario>> GetUsuariosNotificadosAsync(Guid alertaId)
        {
            try
            {
                return await _db.AlertasUsuario
                    .Include(au => au.Usuario)
                    .Where(au => au.AlertasFrotiXId == alertaId)
                    .OrderBy(au => au.Usuario.UserName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXRepository.cs", "GetUsuariosNotificadosAsync", ex);
                return new List<AlertasUsuario>();
            }
        }

        public async Task<AspNetUsers> GetUsuarioAsync(string usuarioId)
        {
            try
            {
                return await _db.AspNetUsers
                    .FirstOrDefaultAsync(u => u.Id == usuarioId);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXRepository.cs", "GetUsuarioAsync", ex);
                return null;
            }
        }

        public async Task<IEnumerable<AlertasFrotiX>> GetAlertasParaNotificarAsync()
        {
            try
            {
                var agora = DateTime.Now;

                return await _db.AlertasFrotiX
                    .Include(a => a.AlertasUsuarios)
                        .ThenInclude(au => au.Usuario)
                    .Where(a => a.Ativo &&
                           (
                               // Alertas que devem ser exibidos ao abrir o sistema
                               (a.TipoExibicao == TipoExibicaoAlerta.AoAbrir) ||

                               // Alertas com horário específico (verifica se chegou a hora hoje)
                               (a.TipoExibicao == TipoExibicaoAlerta.Horario &&
                                a.HorarioExibicao.HasValue &&
                                agora.TimeOfDay >= a.HorarioExibicao.Value &&
                                agora.TimeOfDay <= a.HorarioExibicao.Value.Add(TimeSpan.FromMinutes(5))) ||

                               // Alertas com data/hora específica
                               (a.TipoExibicao == TipoExibicaoAlerta.DataHora &&
                                a.DataExibicao.HasValue &&
                                a.DataExibicao.Value <= agora &&
                                (!a.DataExpiracao.HasValue || a.DataExpiracao.Value >= agora))
                           ))
                    .Where(a => !a.DataExpiracao.HasValue || a.DataExpiracao.Value >= agora) // Não expirados
                    .OrderByDescending(a => a.Prioridade)
                    .ThenByDescending(a => a.DataInsercao)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiXRepository.cs", "GetAlertasParaNotificarAsync", ex);
                return new List<AlertasFrotiX>();
            }
        }
    }
}
