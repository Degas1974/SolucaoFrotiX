using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using FrotiX.Helpers;

namespace FrotiX.Repository
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════
    /// ║ REPOSITORY: AlertasUsuarioRepository
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// ║ DESCRIÇÃO:
    /// ║    Gerencia a relação N:N entre AlertasFrotiX e AspNetUsers.
    /// ║    Tabela intermediária que armazena o estado de leitura de cada alerta
    /// ║    para cada usuário (Lido, Notificado, Apagado, DataLeitura, etc.).
    /// ║
    /// ║ ENTIDADE PRINCIPAL:
    /// ║    - AlertasUsuario (tabela de junção/pivot table).
    /// ║
    /// ║ RELACIONAMENTOS:
    /// ║    - AlertasFrotiX (N:1) - Alerta global.
    /// ║    - AspNetUsers (N:1) - Usuário do sistema.
    /// ║
    /// ║ MÉTODOS PRINCIPAIS:
    /// ║    1. ObterAlertasPorUsuarioAsync() - Lista alertas de um usuário.
    /// ║    2. ObterUsuariosPorAlertaAsync() - Lista usuários de um alerta.
    /// ║    3. UsuarioTemAlertaAsync() - Verifica se vinculação existe.
    /// ║    4. RemoverAlertasDoUsuarioAsync() - Remove todas vinculações do usuário.
    /// ║    5. RemoverUsuariosDoAlertaAsync() - Remove todas vinculações do alerta.
    /// ║    6. Update() - Atualiza estado de leitura (Lido, Notificado, Apagado).
    /// ║
    /// ║ OBSERVAÇÕES:
    /// ║    - NÃO remove registros do banco (soft delete via campo Apagado).
    /// ║    - Update() usado para marcar Lido/Notificado/Apagado.
    /// ║    - RemoveRange() usado apenas em cascata (exclusão de alerta/usuário).
    /// ║
    /// ║ INTEGRAÇÃO:
    /// ║    - AlertasHub (SignalR) - Notificações em tempo real.
    /// ║    - AlertasFrotiXRepository - Criação de alertas com usuários.
    /// ║══════════════════════════════════════════════════════════════════════════════
    /// </summary>
    public class AlertasUsuarioRepository : Repository<AlertasUsuario>, IAlertasUsuarioRepository
    {
        private new readonly FrotiXDbContext _db;

        public AlertasUsuarioRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: ObterAlertasPorUsuarioAsync (Alertas do Usuário)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna TODOS os alertas vinculados a um usuário específico.
        /// │    Include: AlertasFrotiX (dados completos do alerta).
        /// │
        /// │ USO:
        /// │    - Modal de notificações: Lista todos os alertas do usuário logado.
        /// │    - Relatórios: Histórico de alertas recebidos por usuário.
        /// │
        /// │ TRACKING:
        /// │    AsNoTracking() - Consulta somente leitura (performance).
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<AlertasUsuario> com AlertasFrotiX carregado.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<IEnumerable<AlertasUsuario>> ObterAlertasPorUsuarioAsync(string usuarioId)
        {
            try
            {
                // [QUERY] - Lista AlertasUsuario do usuário com dados do alerta
                return await _db.Set<AlertasUsuario>()
                    .Where(au => au.UsuarioId == usuarioId)
                    .Include(au => au.AlertasFrotiX) // Carrega dados do alerta
                    .AsNoTracking() // Somente leitura
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "ObterAlertasPorUsuarioAsync", ex);
                return new List<AlertasUsuario>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: ObterUsuariosPorAlertaAsync (Usuários do Alerta)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Retorna TODOS os usuários vinculados a um alerta específico.
        /// │    Não carrega Usuario (AspNetUsers) - use Include se necessário.
        /// │
        /// │ USO:
        /// │    - Admin: Ver quem recebeu um alerta específico.
        /// │    - Relatórios: Análise de distribuição de alertas.
        /// │
        /// │ TRACKING:
        /// │    AsNoTracking() - Consulta somente leitura.
        /// │
        /// │ RETORNO:
        /// │    IEnumerable<AlertasUsuario> (sem navegações carregadas).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<IEnumerable<AlertasUsuario>> ObterUsuariosPorAlertaAsync(Guid alertaId)
        {
            try
            {
                // [QUERY] - Lista AlertasUsuario do alerta (sem Include Usuario)
                return await _db.Set<AlertasUsuario>()
                    .Where(au => au.AlertasFrotiXId == alertaId)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "ObterUsuariosPorAlertaAsync", ex);
                return new List<AlertasUsuario>();
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: UsuarioTemAlertaAsync (Verificar Vinculação)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Verifica se existe vinculação entre alerta e usuário.
        /// │    Usa AnyAsync() para performance (não traz dados, só verifica existência).
        /// │
        /// │ USO:
        /// │    - Validação: Antes de criar duplicata de AlertasUsuario.
        /// │    - Controllers: Verificar se usuário já recebeu alerta.
        /// │
        /// │ RETORNO:
        /// │    bool: true (vinculação existe) ou false (não existe).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task<bool> UsuarioTemAlertaAsync(Guid alertaId, string usuarioId)
        {
            try
            {
                // [QUERY EXISTÊNCIA] - Verifica se vinculação existe (performance)
                return await _db.Set<AlertasUsuario>()
                    .AnyAsync(au => au.AlertasFrotiXId == alertaId && au.UsuarioId == usuarioId);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "UsuarioTemAlertaAsync", ex);
                return false;
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: RemoverAlertasDoUsuarioAsync (Cascata - Exclusão Usuário)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Remove TODAS as vinculações de alertas de um usuário.
        /// │    Usado em cascata quando usuário é excluído do sistema.
        /// │
        /// │ ATENÇÃO:
        /// │    - HARD DELETE (remove fisicamente do banco).
        /// │    - NÃO chama SaveChangesAsync() - deve ser chamado no UnitOfWork.
        /// │    - Usa RemoveRange() para performance (remoção em lote).
        /// │
        /// │ USO:
        /// │    - Exclusão de usuário: Limpar alertas vinculados antes de deletar.
        /// │    - Admin: Resetar alertas de um usuário específico.
        /// │
        /// │ FLUXO:
        /// │    1. Busca todos AlertasUsuario do usuário.
        /// │    2. Se houver registros: RemoveRange().
        /// │    3. UnitOfWork.SaveAsync() persiste exclusão.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task RemoverAlertasDoUsuarioAsync(string usuarioId)
        {
            try
            {
                // [QUERY] - Busca todos AlertasUsuario do usuário
                var alertasUsuario = await _db.Set<AlertasUsuario>()
                    .Where(au => au.UsuarioId == usuarioId)
                    .ToListAsync();

                if (alertasUsuario.Any())
                {
                    // [DELETE] - Remove todos em lote (performance)
                    _db.Set<AlertasUsuario>().RemoveRange(alertasUsuario);
                    // NÃO chama SaveChangesAsync() aqui - UnitOfWork faz isso
                }
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "RemoverAlertasDoUsuarioAsync", ex);
                throw; // Propaga exceção (transação deve falhar)
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: RemoverUsuariosDoAlertaAsync (Cascata - Exclusão Alerta)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Remove TODAS as vinculações de usuários de um alerta.
        /// │    Usado em cascata quando alerta é excluído do sistema.
        /// │
        /// │ ATENÇÃO:
        /// │    - HARD DELETE (remove fisicamente do banco).
        /// │    - NÃO chama SaveChangesAsync() - deve ser chamado no UnitOfWork.
        /// │    - Usa RemoveRange() para performance.
        /// │
        /// │ USO:
        /// │    - Exclusão de alerta: Limpar usuários vinculados antes de deletar.
        /// │    - Admin: Resetar distribuição de alerta específico.
        /// │
        /// │ FLUXO:
        /// │    1. Busca todos AlertasUsuario do alerta.
        /// │    2. Se houver registros: RemoveRange().
        /// │    3. UnitOfWork.SaveAsync() persiste exclusão.
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task RemoverUsuariosDoAlertaAsync(Guid alertaId)
        {
            try
            {
                // [QUERY] - Busca todos AlertasUsuario do alerta
                var alertasUsuario = await _db.Set<AlertasUsuario>()
                    .Where(au => au.AlertasFrotiXId == alertaId)
                    .ToListAsync();

                if (alertasUsuario.Any())
                {
                    // [DELETE] - Remove todos em lote (performance)
                    _db.Set<AlertasUsuario>().RemoveRange(alertasUsuario);
                    // NÃO chama SaveChangesAsync() aqui - UnitOfWork faz isso
                }
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "RemoverUsuariosDoAlertaAsync", ex);
                throw; // Propaga exceção (transação deve falhar)
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Update (Atualizar Estado de Leitura)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Atualiza registro AlertasUsuario (Lido, Notificado, Apagado, Datas).
        /// │    Marca entidade como Modified para EF Core rastrear alteração.
        /// │
        /// │ VALIDAÇÃO:
        /// │    Lança ArgumentNullException se alertaUsuario for null.
        /// │
        /// │ USO:
        /// │    - MarcarComoLidoAsync(): Atualiza Lido=true, DataLeitura=Now.
        /// │    - SignalR: Atualiza Notificado=true, DataNotificacao=Now.
        /// │    - MarcarComoApagadoAsync(): Atualiza Apagado=true, DataApagado=Now.
        /// │
        /// │ ATENÇÃO:
        /// │    - NÃO chama SaveChangesAsync() - deve ser chamado no UnitOfWork.
        /// │    - Entidade DEVE ser rastreada (AsTracking() na query).
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public new void Update(AlertasUsuario alertaUsuario)
        {
            try
            {
                // [VALIDAÇÃO] - Garante que entidade não é null
                if (alertaUsuario == null)
                    throw new ArgumentNullException(nameof(alertaUsuario));

                // [UPDATE] - Marca entidade como Modified (EF Core rastreia)
                _db.Set<AlertasUsuario>().Update(alertaUsuario);
                // NÃO chama SaveChangesAsync() aqui - UnitOfWork faz isso
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("AlertasUsuarioRepository.cs", "Update", ex);
                throw; // Propaga exceção (update deve falhar)
            }
        }
    }
}
