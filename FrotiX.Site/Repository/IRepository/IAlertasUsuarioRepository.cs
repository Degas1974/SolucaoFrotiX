/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: IAlertasUsuarioRepository.cs                                                                        ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de AlertasUsuario, gerenciando associação MxN entre alertas e usuários   ║
║              para controle de leitura/notificação.                                                               ║
║  📋 MÉTODOS ADICIONAIS:                                                                                          ║
║     • ObterAlertasPorUsuarioAsync() → Alertas de um usuário                                                     ║
║     • ObterUsuariosPorAlertaAsync() → Usuários vinculados a um alerta                                           ║
║     • UsuarioTemAlertaAsync() → Verifica vínculo existente                                                      ║
║     • RemoverAlertasDoUsuarioAsync() → Limpa alertas do usuário                                                 ║
║     • Update() → Atualização do vínculo alerta-usuário                                                          ║
║  🔗 DEPENDÊNCIAS: IRepository<AlertasUsuario>, Task, async/await                                                ║
║  📅 Atualizado: 29/01/2026    👤 Team: FrotiX    📝 Versão: 2.0                                                 ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
using FrotiX.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// Interface do repositório de AlertasUsuario. Estende IRepository&lt;AlertasUsuario&gt;.
    /// </summary>
    public interface IAlertasUsuarioRepository :IRepository<AlertasUsuario>
    {
        /// <summary>
        /// Obtém todos os alertas de um usuário específico
        /// </summary>
        Task<IEnumerable<AlertasUsuario>> ObterAlertasPorUsuarioAsync(string usuarioId);

        /// <summary>
        /// Obtém todos os usuários vinculados a um alerta
        /// </summary>
        Task<IEnumerable<AlertasUsuario>> ObterUsuariosPorAlertaAsync(Guid alertaId);

        /// <summary>
        /// Verifica se um usuário já tem um alerta específico vinculado
        /// </summary>
        Task<bool> UsuarioTemAlertaAsync(Guid alertaId , string usuarioId);

        /// <summary>
        /// Remove todos os alertas de um usuário
        /// </summary>
        Task RemoverAlertasDoUsuarioAsync(string usuarioId);

        /// <summary>
        /// Remove todos os usuários de um alerta
        /// </summary>
        Task RemoverUsuariosDoAlertaAsync(Guid alertaId);

        /// <summary>
        /// Atualiza a entidade AlertasUsuario
        /// </summary>
        void Update(AlertasUsuario alertaUsuario);
    }
}
