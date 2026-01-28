// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewItensManutencao.cs                                             ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para exibição de itens de manutenção vinculados a uma OS.        ║
// ║ Pode incluir ocorrências de viagem associadas ao item de manutenção.        ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • ItemManutencaoId - Identificador único do item                            ║
// ║ • ManutencaoId - OS pai do item                                             ║
// ║ • MotoristaId - Motorista que reportou (se aplicável)                       ║
// ║ • ViagemId - Viagem de origem da ocorrência (se aplicável)                  ║
// ║                                                                              ║
// ║ Dados do Item:                                                               ║
// ║ • TipoItem - Tipo (peça, serviço, mão de obra)                              ║
// ║ • NumFicha - Número da ficha de manutenção                                  ║
// ║ • DataItem - Data do registro do item                                       ║
// ║ • Resumo - Resumo do item/serviço                                           ║
// ║ • Descricao - Descrição detalhada                                           ║
// ║ • Status - Status atual do item                                             ║
// ║                                                                              ║
// ║ Dados Associados:                                                            ║
// ║ • ImagemOcorrencia - URL da imagem de ocorrência (se houver)                ║
// ║ • NomeMotorista - Nome do motorista que reportou                            ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    public class ViewItensManutencao
    {
        public Guid ItemManutencaoId { get; set; }

        public Guid ManutencaoId { get; set; }

        public string? TipoItem { get; set; }

        public string? NumFicha { get; set; }

        public string? DataItem { get; set; }

        public string? Resumo { get; set; }

        public string? Descricao { get; set; }

        public string? Status { get; set; }

        public string? ImagemOcorrencia { get; set; }

        public string? NomeMotorista { get; set; }

        public Guid? MotoristaId { get; set; }

        public Guid? ViagemId { get; set; }
    }
}
