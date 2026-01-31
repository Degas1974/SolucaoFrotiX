/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: LavadorContrato.cs                                                                      ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Mapear vínculo N:N entre Lavador e Contrato via chave composta.                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: LavadorContratoViewModel, LavadorContrato                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: transportar dados do vínculo Lavador-Contrato nas telas de cadastro/edição.
    // ==================================================================================================
    public class LavadorContratoViewModel
    {
        // Identificador do lavador selecionado.
        public Guid LavadorId { get; set; }

        // Identificador do contrato selecionado.
        public Guid ContratoId { get; set; }

        // Entidade que representa o vínculo persistido.
        public LavadorContrato LavadorContrato { get; set; }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa o relacionamento N:N entre Lavador e Contrato.
    // ⚠️ ATENÇÃO: chave composta (LavadorId + ContratoId).
    // ==================================================================================================
    public class LavadorContrato
    {
        // Chave composta - FK para Lavador.
        [Key, Column(Order = 0)]
        public Guid LavadorId { get; set; }

        // Chave composta - FK para Contrato.
        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}

