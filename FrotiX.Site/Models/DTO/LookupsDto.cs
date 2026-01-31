/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: LookupsDto.cs                                                                           ║
   ║ 📂 CAMINHO: /Models/DTO                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Records imutáveis para lookups de motoristas, veículos e reservas.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: MotoristaData, VeiculoData, VeiculoReservaData                                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System                                                                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;

namespace FrotiX.Models.DTO // <-- ajuste para o namespace do seu projeto
    {
    // Dados básicos de motorista.
    public sealed record MotoristaData(Guid MotoristaId, string Nome);

    // Dados básicos de veículo.
    public sealed record VeiculoData(Guid VeiculoId, string Descricao);

    // Dados básicos de veículo reserva.
    public sealed record VeiculoReservaData(Guid VeiculoId, string Descricao);
    }

