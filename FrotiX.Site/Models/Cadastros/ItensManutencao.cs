// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ItensManutencao.cs                                                 ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para itens de manutenção vinculados a ordens de serviço.           ║
// ║ Pode originar de ocorrências de viagens ou ser adicionado diretamente.      ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • ItemManutencaoId [Key] - Identificador único                              ║
// ║ • TipoItem - Tipo (Peça, Serviço, Mão de Obra)                              ║
// ║ • NumFicha - Número da ficha de vistoria de origem                          ║
// ║ • DataItem - Data de registro do item                                       ║
// ║ • Resumo - Resumo curto do item/problema                                    ║
// ║ • Descricao - Descrição detalhada                                           ║
// ║ • Status - Status do item (Pendente, Em Andamento, Resolvido)               ║
// ║ • ImagemOcorrencia - URL da foto da ocorrência                              ║
// ║ • ManutencaoId → Manutencao (FK)                                            ║
// ║ • MotoristaId → Motorista (FK) - Quem reportou                              ║
// ║ • ViagemId → Viagem (FK) - Viagem de origem                                 ║
// ║                                                                              ║
// ║ CAMPOS NotMapped:                                                            ║
// ║ • NumOS, DataOS - Dados auxiliares para exibição                            ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
