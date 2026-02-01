/* ****************************************************************************************
 * ⚡ ARQUIVO: LavadoresLavagem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear vínculo N:N entre lavadores e lavagens.
 *
 * 📥 ENTRADAS     : Identificadores de lavador e lavagem.
 *
 * 📤 SAÍDAS       : Entidade de relacionamento persistida.
 *
 * 🔗 CHAMADA POR  : Fluxos de registro de lavagem.
 *
 * 🔄 CHAMA        : DataAnnotations, Column(Order), ForeignKey.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.EntityFrameworkCore.
 *
 * ⚠️ ATENÇÃO      : Chave composta (LavagemId + LavadorId).
 **************************************************************************************** */

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
    /****************************************************************************************
     * ⚡ MODEL: LavadoresLavagem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar o relacionamento N:N entre Lavador e Lavagem.
     *
     * 📥 ENTRADAS     : LavagemId e LavadorId.
     *
     * 📤 SAÍDAS       : Registro de vínculo persistido.
     *
     * 🔗 CHAMADA POR  : Processos de lavagem.
     *
     * 🔄 CHAMA        : ForeignKey.
     *
     * ⚠️ ATENÇÃO      : Chave composta (LavagemId + LavadorId).
     ****************************************************************************************/
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
