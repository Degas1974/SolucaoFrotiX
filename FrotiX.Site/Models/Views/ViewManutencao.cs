// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewManutencao.cs                                                  ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model complexo para gestão de ordens de serviço de manutenção.         ║
// ║ Inclui dados do veículo, contrato, datas, valores e atributos de UI.        ║
// ║                                                                              ║
// ║ PROPRIEDADES PRINCIPAIS:                                                     ║
// ║ Identificadores:                                                             ║
// ║ • ManutencaoId, ContratoId, VeiculoId - Chaves de relacionamento            ║
// ║                                                                              ║
// ║ Dados da OS:                                                                 ║
// ║ • NumOS - Número da ordem de serviço                                        ║
// ║ • ResumoOS - Descrição resumida do serviço                                  ║
// ║ • StatusOS - Status atual (Aberta, Em Andamento, Concluída, etc)            ║
// ║                                                                              ║
// ║ Datas (formatadas char(10) e raw DateTime):                                 ║
// ║ • DataSolicitacao/Raw - Data de abertura da OS                              ║
// ║ • DataDisponibilidade - Data de disponibilidade do veículo                  ║
// ║ • DataRecolhimento - Data de recolhimento para manutenção                   ║
// ║ • DataRecebimentoReserva/DevolucaoReserva - Período do carro reserva        ║
// ║ • DataEntrega/Devolucao/DevolucaoRaw - Datas de conclusão                   ║
// ║                                                                              ║
// ║ Dados do Veículo:                                                            ║
// ║ • DescricaoVeiculo, PlacaDescricao, Placa, Sigla                            ║
// ║ • CombustivelDescricao - Tipo de combustível                                ║
// ║ • Reserva, CarroReserva - Dados do veículo reserva                          ║
// ║                                                                              ║
// ║ Valores:                                                                      ║
// ║ • Quantidade, NumItem - Itens da OS                                         ║
// ║ • ValorUnitario (double) - Valor unitário do serviço                        ║
// ║ • ValorGlosa (decimal), DiasGlosa, Dias - Cálculos de glosa                 ║
// ║                                                                              ║
// ║ Atributos de UI (controle de permissões e estados visuais):                 ║
// ║ • Habilitado, Icon - Estado geral da linha                                  ║
// ║ • HabilitadoEditar, OpacityEditar - Controle do botão editar                ║
// ║ • HabilitadoBaixar, ModalBaixarAttrs, OpacityBaixar - Controle de download  ║
// ║ • HabilitadoCancelar, OpacityCancelar, TooltipCancelar - Controle cancelar  ║
// ║ • Tooltip, OpacityTooltipEditarEditar - Tooltips dinâmicos                  ║
// ║ • ReservaEnviado - Flag de notificação enviada                              ║
// ║                                                                              ║
// ║ MAPEAMENTO:                                                                   ║
// ║ • Mapeia para view SQL vwManutencao com dados denormalizados                ║
// ║ • Campos de UI calculados via CASE no SQL para performance                  ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;

namespace FrotiX.Models
{
    public class ViewManutencao
    {
        // Chaves
        public Guid ManutencaoId
        {
            get; set;
        }

        public Guid? ContratoId
        {
            get; set;
        }

        public Guid VeiculoId
        {
            get; set;
        }

        // Strings básicas
        public string? NumOS
        {
            get; set;
        }

        public string? ResumoOS
        {
            get; set;
        }

        public string? StatusOS
        {
            get; set;
        }

        // Datas formatadas (char(10) no banco)
        public string? DataSolicitacao
        {
            get; set;
        }

        public string? DataDisponibilidade
        {
            get; set;
        }

        public string? DataRecolhimento
        {
            get; set;
        }

        public string? DataRecebimentoReserva
        {
            get; set;
        }

        public string? DataDevolucaoReserva
        {
            get; set;
        }

        public string? DataEntrega
        {
            get; set;
        }

        public string? DataDevolucao
        {
            get; set;
        }

        // Datas cruas (datetime no banco)
        public DateTime? DataSolicitacaoRaw
        {
            get; set;
        }

        public DateTime? DataDevolucaoRaw
        {
            get; set;
        }

        // Descrições e textos
        public string? DescricaoVeiculo
        {
            get; set;
        } // varchar(151)

        public string? PlacaDescricao
        {
            get; set;
        } // varchar(164)

        public string? Sigla
        {
            get; set;
        } // varchar(50)

        public string? CombustivelDescricao
        {
            get; set;
        } // varchar(50)

        public string? Placa
        {
            get; set;
        } // varchar(10)

        public string? Reserva
        {
            get; set;
        } // varchar(7)

        public string? Descricao
        {
            get; set;
        } // varchar(100)

        // Campos numéricos inteiros
        public int? Quantidade
        {
            get; set;
        } // int

        public int? DiasGlosa
        {
            get; set;
        } // int

        public int? Dias
        {
            get; set;
        } // int

        public int? NumItem
        {
            get; set;
        } // int

        // Campos numéricos decimais - ATENÇÃO AOS TIPOS ESPECÍFICOS!
        public double? ValorUnitario
        {
            get; set;
        } // float no banco = double no C#

        public decimal? ValorGlosa
        {
            get; set;
        } // decimal no banco = decimal no C#

        // Campos de UI (todos varchar)
        public string? Habilitado
        {
            get; set;
        } // varchar(50)

        public string? Icon
        {
            get; set;
        } // varchar(21)

        public string? HabilitadoEditar
        {
            get; set;
        } // varchar(8)

        public string? OpacityEditar
        {
            get; set;
        } // varchar(33)

        public string? OpacityTooltipEditarEditar
        {
            get; set;
        } // varchar(25)

        public string? HabilitadoBaixar
        {
            get; set;
        } // varchar(8)

        public string? ModalBaixarAttrs
        {
            get; set;
        } // varchar(50)

        public string? OpacityBaixar
        {
            get; set;
        } // varchar(33)

        public string? Tooltip
        {
            get; set;
        } // varchar(25)

        public string? HabilitadoCancelar
        {
            get; set;
        } // varchar(8)

        public string? OpacityCancelar
        {
            get; set;
        } // varchar(33)

        public string? TooltipCancelar
        {
            get; set;
        } // varchar(20)

        public string? Veiculo
        {
            get; set;
        } // varchar(20)

        public string? CarroReserva
        {
            get; set;
        } // varchar(20)

        public bool? ReservaEnviado
        {
            get; set;
        } // varchar(20)
    }
}
