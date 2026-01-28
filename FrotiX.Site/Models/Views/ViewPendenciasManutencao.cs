// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewPendenciasManutencao.cs                                        ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para listagem de pendências de manutenção não resolvidas.        ║
// ║ Usado em dashboards de gestão para acompanhamento de itens pendentes.       ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • ItemManutencaoId - Identificador do item pendente                         ║
// ║ • ManutencaoId - OS associada                                               ║
// ║ • MotoristaId - Motorista que reportou                                      ║
// ║ • ViagemId - Viagem de origem                                               ║
// ║ • VeiculoId - Veículo com pendência                                         ║
// ║                                                                              ║
// ║ Dados do Item:                                                               ║
// ║ • TipoItem - Tipo (peça, serviço, etc)                                      ║
// ║ • NumFicha - Número da ficha de vistoria                                    ║
// ║ • DataItem - Data do registro                                               ║
// ║ • Resumo, Descricao - Detalhes da pendência                                 ║
// ║ • Status - Status atual                                                     ║
// ║                                                                              ║
// ║ Dados Adicionais:                                                            ║
// ║ • Nome - Nome do motorista que reportou                                     ║
// ║ • ImagemOcorrencia - Foto da ocorrência                                     ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
    {
    public class ViewPendenciasManutencao
        {

        public Guid ItemManutencaoId { get; set; }

        public Guid ManutencaoId { get; set; }

        public Guid MotoristaId { get; set; }

        public Guid ViagemId { get; set; }

        public Guid VeiculoId { get; set; }

        public string? TipoItem { get; set; }

        public string? NumFicha { get; set; }

        public string? DataItem { get; set; }

        public string? Resumo { get; set; }

        public string? Descricao { get; set; }

        public string? Status { get; set; }

        public string? Nome { get; set; }

        public string? ImagemOcorrencia { get; set; }

        }
    }


