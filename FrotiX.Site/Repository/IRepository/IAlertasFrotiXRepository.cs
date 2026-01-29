// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IAlertasFrotiXRepository.cs                                     ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de AlertasFrotiX, gerenciando sistema de alertas    ║
// ║ do sistema (vencimentos, manutenções, documentos, etc.).                    ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetTodosAlertasAtivosAsync() → Lista alertas ativos                        ║
// ║ • GetQuantidadeAlertasNaoLidosAsync() → Contador para badge                  ║
// ║ • MarcarComoLidoAsync() → Atualiza status de leitura                         ║
// ║ • CriarAlertaAsync() → Cria alerta com notificação para usuários             ║
// ║ • GetAlertasParaNotificarAsync() → Alertas pendentes de notificação          ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// Interface do repositório de AlertasFrotiX. Estende IRepository&lt;AlertasFrotiX&gt;.
    /// </summary>
    public interface IAlertasFrotiXRepository :IRepository<AlertasFrotiX>
    {
        // Métodos existentes
        Task<IEnumerable<AlertasFrotiX>> GetTodosAlertasAtivosAsync();
        Task<IEnumerable<AlertasFrotiX>> GetTodosAlertasComLeituraAsync();
        Task<int> GetQuantidadeAlertasNaoLidosAsync(string usuarioId);
        Task<bool> MarcarComoLidoAsync(Guid alertaId , string usuarioId);
        Task<AlertasFrotiX> CriarAlertaAsync(AlertasFrotiX alerta , List<string> usuariosIds);

        // NOVOS MÉTODOS
        Task<AlertasFrotiX> GetAlertaComDetalhesAsync(Guid alertaId);
        Task<bool> MarcarComoApagadoAsync(Guid alertaId , string usuarioId);
        Task<bool> DesativarAlertaAsync(Guid alertaId);
        Task<IEnumerable<AlertasUsuario>> GetUsuariosNotificadosAsync(Guid alertaId);
        Task<AspNetUsers> GetUsuarioAsync(string usuarioId);
        Task<IEnumerable<AlertasFrotiX>> GetAlertasParaNotificarAsync();

    }
}
