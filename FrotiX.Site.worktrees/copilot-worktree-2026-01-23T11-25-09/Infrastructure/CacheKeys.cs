namespace FrotiX.Infrastructure
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║                                                                              ║
    /// ║  🔑 ARQUIVO: CacheKeys.cs (Chaves de Cache Centralizadas)                   ║
    /// ║                                                                              ║
    /// ║  DESCRIÇÃO:                                                                  ║
    /// ║  Classe estática com constantes para chaves de cache do sistema.            ║
    /// ║  Centraliza nomes de chaves para evitar erros de digitação (typo).          ║
    /// ║                                                                              ║
    /// ║  PADRÃO DE NOMENCLATURA:                                                     ║
    /// ║  - Formato: "operacao:entidade" (ex: "upsert:motoristas").                  ║
    /// ║  - Operação: upsert (insert/update combinado).                              ║
    /// ║  - Entidade: nome da entidade no plural e minúsculo.                        ║
    /// ║                                                                              ║
    /// ║  TECNOLOGIA DE CACHE:                                                        ║
    /// ║  - IMemoryCache (ASP.NET Core in-memory cache).                             ║
    /// ║  - Usado em MotoristaCache.cs e outros serviços de cache.                   ║
    /// ║                                                                              ║
    /// ║  CHAVES DISPONÍVEIS:                                                         ║
    /// ║  - Motoristas: Cache de motoristas ativos (MotoristaCache).                 ║
    /// ║  - Veiculos: Cache de veículos ativos (VeiculoCache - se existir).          ║
    /// ║  - VeiculosReserva: Cache de veículos de reserva (backup).                  ║
    /// ║                                                                              ║
    /// ║  BENEFÍCIOS:                                                                 ║
    /// ║  - Evita typos: CacheKeys.Motoristas vs "upsert:motoristas" hardcoded.      ║
    /// ║  - Refatoração segura: Mudar chave em um único lugar.                       ║
    /// ║  - IntelliSense: IDE sugere chaves disponíveis.                             ║
    /// ║                                                                              ║
    /// ║  USO NO SISTEMA:                                                             ║
    /// ║  _cache.Set(CacheKeys.Motoristas, listaMotoristasAtivos);                   ║
    /// ║  var motoristas = _cache.Get<List<Motorista>>(CacheKeys.Motoristas);        ║
    /// ║                                                                              ║
    /// ║  ÚLTIMA ATUALIZAÇÃO: 19/01/2026                                              ║
    /// ║                                                                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    /// </summary>
    public static class CacheKeys
    {
        /// <summary>
        /// 🔑 Chave de cache para lista de motoristas ativos (usado em MotoristaCache).
        /// Padrão: "upsert:motoristas"
        /// </summary>
        public const string Motoristas = "upsert:motoristas";

        /// <summary>
        /// 🔑 Chave de cache para lista de veículos ativos.
        /// Padrão: "upsert:veiculos"
        /// </summary>
        public const string Veiculos = "upsert:veiculos";

        /// <summary>
        /// 🔑 Chave de cache para lista de veículos de reserva (backup).
        /// Padrão: "upsert:veiculosreserva"
        /// </summary>
        public const string VeiculosReserva = "upsert:veiculosreserva";
    }
}


