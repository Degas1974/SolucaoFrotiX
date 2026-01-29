/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LookupsDto.cs                                                                           ║
   ║ 📂 CAMINHO: /Models/DTO                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: DTOs records para lookups de motoristas, veículos e reservas (imutáveis).             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 RECORDS: MotoristaData, VeiculoData, VeiculoReservaData (sealed records C# 10)                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System                                                                                     ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;

namespace FrotiX.Models.DTO // <-- ajuste para o namespace do seu projeto
    {
    public sealed record MotoristaData(Guid MotoristaId, string Nome);

    public sealed record VeiculoData(Guid VeiculoId, string Descricao);

    public sealed record VeiculoReservaData(Guid VeiculoId, string Descricao);
    }


