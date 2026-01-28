// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: LavadoresLavagem.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade de relacionamento N:N entre Lavagem e Lavador.                     ║
// ║ Permite múltiplos lavadores em uma única lavagem de veículo.                ║
// ║                                                                              ║
// ║ PROPRIEDADES (Chave Composta):                                              ║
// ║ • LavagemId [Key, Order=0] - ID da lavagem → Lavagem (FK)                   ║
// ║ • LavadorId [Key, Order=1] - ID do lavador → Lavador (FK)                   ║
// ║                                                                              ║
// ║ RELACIONAMENTOS:                                                             ║
// ║ • Lavagem - Navegação para a lavagem                                        ║
// ║ • Lavador - Navegação para o lavador                                        ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Registro de quais lavadores participaram de cada lavagem                  ║
// ║ • Cálculo de produtividade por lavador                                      ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
    public class LavadoresLavagem
    {
        [Key, Column(Order = 0)]
        public Guid LavagemId { get; set; }

        [ForeignKey("LavagemId")]
        public virtual Lavagem? Lavagem { get; set; }

        [Key, Column(Order = 1)]
        public Guid LavadorId { get; set; }

        [ForeignKey("LavadorId")]
        public virtual Lavador? Lavador { get; set; }
    }
}
