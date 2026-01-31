/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: VeiculoAta.cs                                                                           ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Mapear vínculo N:N entre Veículo e Ata de Preços via chave composta.                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: VeiculoAtaViewModel, VeiculoAta                                                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: transportar vínculo veículo-ata nas telas de edição.
    // ==================================================================================================
    public class VeiculoAtaViewModel
    {
        // Identificador do veículo.
        public Guid VeiculoId { get; set; }

        // Identificador da ata.
        public Guid AtaId { get; set; }

        // Entidade do vínculo.
        public VeiculoAta? VeiculoAta { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa o relacionamento N:N entre Veículo e Ata de Preços.
    // ⚠️ ATENÇÃO: chave composta (VeiculoId + AtaId).
    // ==================================================================================================
    public class VeiculoAta
    {
        // Chave composta - FK para Veículo.
        [Key, Column(Order = 0)]
        public Guid VeiculoId { get; set; }

        // Chave composta - FK para Ata.
        [Key, Column(Order = 1)]
        public Guid AtaId { get; set; }
    }
}
