/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: CacheKeys.cs                                                                           ║
   ║ 📂 CAMINHO: /Infrastructure                                                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Constantes de chaves para cache de memória (IMemoryCache).                                      ║
   ║    Usadas em Upsert de Viagem para cache de Motoristas e Veículos.                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE CONSTANTES:                                                                           ║
   ║ 1. [Motoristas]      : Cache de lista de motoristas.......... "upsert:motoristas"                  ║
   ║ 2. [Veiculos]        : Cache de lista de veículos............ "upsert:veiculos"                    ║
   ║ 3. [VeiculosReserva] : Cache de veículos reserva............. "upsert:veiculosreserva"             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

namespace FrotiX.Infrastructure
{
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ CLASSE: CacheKeys                                                                  │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO: Constantes estáticas para chaves de cache de memória (IMemoryCache).   │
    /// │    Padroniza nomenclatura de cache evitando strings mágicas no código.               │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🔗 RASTREABILIDADE:                                                                   │
    /// │    ⬅️ CHAMADO POR : ViagemController.Upsert, ViagemController.GetMotoristas          │
    /// │    ➡️ CHAMA       : (nenhum - classe de constantes)                                   │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public static class CacheKeys
    {
        // [DADOS] Chave para cache de motoristas no upsert de viagem
        public const string Motoristas = "upsert:motoristas";
        
        // [DADOS] Chave para cache de veículos no upsert de viagem
        public const string Veiculos = "upsert:veiculos";
        
        // [DADOS] Chave para cache de veículos reserva no upsert de viagem
        public const string VeiculosReserva = "upsert:veiculosreserva";
    }
}


