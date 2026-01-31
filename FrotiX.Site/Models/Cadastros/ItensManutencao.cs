/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ItensManutencao.cs                                                                    ║
   ║ 📂 CAMINHO: Models/Cadastros/                                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Entidade para itens de manutenção de veículos (peças e serviços).                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES DISPONÍVEIS:                                                                           ║
   ║    • ItensManutencao                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: FrotiX.Services, FrotiX.Validations, SelectListItem                                ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: FrotiX Team | 📝 VERSÃO: 2.0                                 ║
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
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ItensManutencao                                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Registrar itens vinculados a manutenção, motorista e viagem.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Fluxos de manutenção
    // ➡️ CHAMA       : ForeignKey, NotMapped
    //
    // ⚠️ ATENÇÃO:
    // Campos NumOS e DataOS são NotMapped (uso em UI).
    //
    public class ItensManutencao
    {
        [Key]
        public Guid ItemManutencaoId { get; set; }

        public string? TipoItem { get; set; }

        public string? NumFicha { get; set; }

        public DateTime? DataItem { get; set; }

        public string? Resumo { get; set; }

        public string? Descricao { get; set; }

        public string? Status { get; set; }

        public string? ImagemOcorrencia { get; set; }

        public Guid? ManutencaoId { get; set; }

        [ForeignKey("ManutencaoId")]
        public virtual Manutencao Manutencao { get; set; }

        public Guid? MotoristaId { get; set; }

        [ForeignKey("MotoristaId")]
        public virtual Motorista Motorista { get; set; }

        public Guid? ViagemId { get; set; }

        [ForeignKey("ViagemId")]
        public virtual Viagem Viagem { get; set; }

        [NotMapped]
        public string NumOS { get; set; }

        [NotMapped]
        public string DataOS { get; set; }
    }
}
