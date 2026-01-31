/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: VeiculoContrato.cs                                                                      ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Mapear vínculo N:N entre Veículo e Contrato via chave composta.                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: VeiculoContratoViewModel, VeiculoContrato                                               ║
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
    // Finalidade: transportar vínculo veículo-contrato nas telas de edição.
    // ==================================================================================================
    public class VeiculoContratoViewModel
    {
        // Identificador do veículo.
        public Guid VeiculoId { get; set; }

        // Identificador do contrato.
        public Guid ContratoId { get; set; }

        // Entidade do vínculo.
        public VeiculoContrato? VeiculoContrato { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa o relacionamento N:N entre Veículo e Contrato.
    // ⚠️ ATENÇÃO: chave composta (VeiculoId + ContratoId).
    // ==================================================================================================
    public class VeiculoContrato
    {
        // Chave composta - FK para Veículo.
        [Key, Column(Order = 0)]
        public Guid VeiculoId { get; set; }

        // Chave composta - FK para Contrato.
        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
