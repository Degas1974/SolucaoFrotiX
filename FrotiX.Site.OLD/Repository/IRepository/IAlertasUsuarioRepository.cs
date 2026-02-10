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
    
    // Interface do repositório de AlertasUsuario. Estende IRepository&lt;AlertasUsuario&gt;.
    
    public interface IAlertasUsuarioRepository :IRepository<AlertasUsuario>
    {
        
        // Obtém todos os alertas de um usuário específico
        
        Task<IEnumerable<AlertasUsuario>> ObterAlertasPorUsuarioAsync(string usuarioId);

        
        // Obtém todos os usuários vinculados a um alerta
        
        Task<IEnumerable<AlertasUsuario>> ObterUsuariosPorAlertaAsync(Guid alertaId);

        
        // Verifica se um usuário já tem um alerta específico vinculado
        
        Task<bool> UsuarioTemAlertaAsync(Guid alertaId , string usuarioId);

        
        // Remove todos os alertas de um usuário
        
        Task RemoverAlertasDoUsuarioAsync(string usuarioId);

        
        // Remove todos os usuários de um alerta
        
        Task RemoverUsuariosDoAlertaAsync(Guid alertaId);

        
        // Atualiza a entidade AlertasUsuario
        
        void Update(AlertasUsuario alertaUsuario);
    }
}
