/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: INavigationModel.cs                                                                     ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Interface para modelo de navegação do sistema (menu lateral).                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS: Seed (navegação inicial), Full (navegação completa)                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: SmartNavigation | 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                   ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

namespace FrotiX.Models
    {
    public interface INavigationModel
        {
        SmartNavigation Seed { get; }
        SmartNavigation Full { get; }
        }
    }


