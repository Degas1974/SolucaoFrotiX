// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: CacheKeys.cs                                                        ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Constantes centralizadas para chaves de cache do sistema.                    ║
// ║ Usado com IMemoryCache para caching de dados frequentemente acessados.       ║
// ║                                                                              ║
// ║ CHAVES DISPONÍVEIS:                                                          ║
// ║ - Motoristas: "upsert:motoristas" → Cache da lista de motoristas             ║
// ║ - Veiculos: "upsert:veiculos" → Cache da lista de veículos                   ║
// ║ - VeiculosReserva: "upsert:veiculosreserva" → Cache de veículos reserva      ║
// ║                                                                              ║
// ║ PADRÃO DE NOMENCLATURA:                                                      ║
// ║ - Prefixo "upsert:" indica cache relacionado a páginas de criação/edição     ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 13                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

namespace FrotiX.Infrastructure
    {
    /// <summary>
    /// Constantes para chaves de cache do sistema.
    /// </summary>
    public static class CacheKeys
        {
        public const string Motoristas = "upsert:motoristas";
        public const string Veiculos = "upsert:veiculos";
        public const string VeiculosReserva = "upsert:veiculosreserva";
        }
    }


