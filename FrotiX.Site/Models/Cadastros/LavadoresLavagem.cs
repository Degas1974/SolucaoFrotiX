/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: LavadoresLavagem.cs                                                                     ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Mapear vínculo N:N entre Lavadores e Lavagens via chave composta.                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: LavadoresLavagem                                                                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
{
    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa o relacionamento N:N entre Lavador e Lavagem.
    // ⚠️ ATENÇÃO: chave composta (LavagemId + LavadorId).
    // ==================================================================================================
    public class LavadoresLavagem
    {
        // Chave composta - FK para Lavagem.
        [Key, Column(Order = 0)]
        public Guid LavagemId { get; set; }

        // Navegação para Lavagem.
        [ForeignKey("LavagemId")]
        public virtual Lavagem? Lavagem { get; set; }

        // Chave composta - FK para Lavador.
        [Key, Column(Order = 1)]
        public Guid LavadorId { get; set; }

        // Navegação para Lavador.
        [ForeignKey("LavadorId")]
        public virtual Lavador? Lavador { get; set; }
    }
}
