/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: CacheKeys.cs                                                                           ║
   ║ 📂 CAMINHO: /Infrastructure                                                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Constantes de chaves para cache de memória (IMemoryCache).                                      ║
   ║    Usadas em ListaCacheService e CacheWarmupService para cache de Motoristas e Veículos.           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE CONSTANTES:                                                                           ║
   ║ 1. [Motoristas]        : Cache de motoristas COM FOTO.......... "lista:motoristas"                 ║
   ║ 2. [Veiculos]          : Cache de veículos (VeiculoCompleto)... "lista:veiculos"                   ║
   ║ 3. [VeiculosManutencao]: Cache de veículos manutenção.......... "lista:veiculos:manutencao"        ║
   ║ 4. [VeiculosReserva]   : Cache de veículos reserva............. "lista:veiculos:reserva"           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ║ 🔗 DEPS: ListaCacheService, CacheWarmupService | 📅 04/02/2026 | 👤 Copilot | 📝 v2.0              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

namespace FrotiX.Infrastructure
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ CLASSE: CacheKeys                                                                  │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO: Constantes estáticas para chaves de cache de memória (IMemoryCache).   │
    // │    Padroniza nomenclatura de cache evitando strings mágicas no código.               │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : ListaCacheService, CacheWarmupService, Pages/*, Controllers/*    │
    // │    ➡️ CHAMA       : (nenhum - classe de constantes)                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    public static class CacheKeys
    {
        // [DADOS] Chave para cache de motoristas COM FOTO (ViewMotoristasViagem)
        public const string Motoristas = "lista:motoristas";
        
        // [DADOS] Chave para cache de veículos (ViewVeiculos.VeiculoCompleto)
        public const string Veiculos = "lista:veiculos";
        
        // [DADOS] Chave para cache de veículos para manutenção (ViewVeiculosManutencao)
        public const string VeiculosManutencao = "lista:veiculos:manutencao";
        
        // [DADOS] Chave para cache de veículos reserva (ViewVeiculosManutencaoReserva)
        public const string VeiculosReserva = "lista:veiculos:reserva";
    }
}


