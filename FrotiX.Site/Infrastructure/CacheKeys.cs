/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Infrastructure/CacheKeys.cs                                    ║
 * ║  Descrição: Constantes de chaves para cache de memória (IMemoryCache).   ║
 * ║             Usadas em Upsert de Viagem para Motoristas e Veículos.       ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

namespace FrotiX.Infrastructure
    {
    public static class CacheKeys
        {
        public const string Motoristas = "upsert:motoristas";
        public const string Veiculos = "upsert:veiculos";
        public const string VeiculosReserva = "upsert:veiculosreserva";
        }
    }


