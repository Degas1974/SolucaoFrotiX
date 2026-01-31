/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: EncarregadoContrato.cs                                                                  ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Mapear vínculo N:N entre Encarregado e Contrato via chave composta.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: EncarregadoContratoViewModel, EncarregadoContrato                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: transportar vínculo encarregado-contrato nas telas de edição.
    // ==================================================================================================
    public class EncarregadoContratoViewModel
    {
        // Identificador do encarregado.
        public Guid EncarregadoId { get; set; }
        // Identificador do contrato.
        public Guid ContratoId { get; set; }
        // Entidade do vínculo.
        public EncarregadoContrato? EncarregadoContrato { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa o relacionamento N:N entre Encarregado e Contrato.
    // ⚠️ ATENÇÃO: chave composta (EncarregadoId + ContratoId).
    // ==================================================================================================
    public class EncarregadoContrato
    {
        // Chave composta - FK para Encarregado.
        [Key, Column(Order = 0)]
        public Guid EncarregadoId { get; set; }

        // Chave composta - FK para Contrato.
        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
