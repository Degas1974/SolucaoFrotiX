// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: INavigationModel.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Interface para modelo de navegação do sistema SmartAdmin.                   ║
// ║ Define estrutura de menu lateral com dois níveis de configuração.           ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - Seed: Menu inicial/básico (configuração estática)                         ║
// ║ - Full: Menu completo (com todos os itens disponíveis)                      ║
// ║                                                                              ║
// ║ USO: Implementado por NavigationModel para construção do menu               ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

namespace FrotiX.Models
    {
    public interface INavigationModel
        {
        SmartNavigation Seed { get; }
        SmartNavigation Full { get; }
        }
    }


