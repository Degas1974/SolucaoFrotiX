// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: LookupsDto.cs                                                      ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Records imutáveis para lookups/dropdowns de motoristas e veículos.          ║
// ║ Usados para carregar dados em combos de seleção.                            ║
// ║                                                                              ║
// ║ RECORDS:                                                                     ║
// ║ - MotoristaData(MotoristaId, Nome)                                          ║
// ║ - VeiculoData(VeiculoId, Descricao)                                         ║
// ║ - VeiculoReservaData(VeiculoId, Descricao) - Para veículos de reserva       ║
// ║                                                                              ║
// ║ NOTA: Usando C# 9 records para imutabilidade e value equality               ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;

namespace FrotiX.Models.DTO // <-- ajuste para o namespace do seu projeto
    {
    public sealed record MotoristaData(Guid MotoristaId, string Nome);

    public sealed record VeiculoData(Guid VeiculoId, string Descricao);

    public sealed record VeiculoReservaData(Guid VeiculoId, string Descricao);
    }


