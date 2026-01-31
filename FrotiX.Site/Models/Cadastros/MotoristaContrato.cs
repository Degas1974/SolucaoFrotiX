/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: MotoristaContrato.cs                                                                    ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Mapear vínculo N:N entre Motorista e Contrato via chave composta.                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: MotoristaoContratoViewModel, MotoristaContrato                                          ║
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
    // Finalidade: transportar o vínculo Motorista-Contrato nas telas de edição.
    // ==================================================================================================
    public class MotoristaoContratoViewModel
    {
        // Identificador do motorista.
        public Guid MotoristaId { get; set; }

        // Identificador do contrato.
        public Guid ContratoId { get; set; }

        // Entidade do vínculo.
        public MotoristaContrato? MotoristaContrato { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa o relacionamento N:N entre Motorista e Contrato.
    // ⚠️ ATENÇÃO: chave composta (MotoristaId + ContratoId).
    // ==================================================================================================
    public class MotoristaContrato
    {
        // Chave composta - FK para Motorista.
        [Key, Column(Order = 0)]
        public Guid MotoristaId { get; set; }

        // Chave composta - FK para Contrato.
        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
