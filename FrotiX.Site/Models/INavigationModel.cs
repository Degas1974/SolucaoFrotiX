/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/INavigationModel.cs                                     ║
 * ║  Descrição: Interface para modelo de navegação do sistema                ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

namespace FrotiX.Models
    {
    public interface INavigationModel
        {
        SmartNavigation Seed { get; }
        SmartNavigation Full { get; }
        }
    }


