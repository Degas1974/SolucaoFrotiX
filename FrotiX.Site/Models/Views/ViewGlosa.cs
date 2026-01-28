// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewGlosa.cs                                                       ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model Keyless para cálculos de glosa em contratos de manutenção.       ║
// ║ Mapeia view SQL com dados denormalizados de manutenção, veículo e valores.  ║
// ║                                                                              ║
// ║ PROPRIEDADES PRINCIPAIS:                                                     ║
// ║                                                                              ║
// ║ Identificadores:                                                             ║
// ║ • ContratoId, ManutencaoId, VeiculoId - Chaves de relacionamento            ║
// ║                                                                              ║
// ║ Dados da OS:                                                                 ║
// ║ • NumOS, ResumoOS, StatusOS - Informações da ordem de serviço               ║
// ║ • PlacaDescricao, DescricaoVeiculo, Placa, Sigla - Dados do veículo         ║
// ║ • CombustivelDescricao - Tipo de combustível                                ║
// ║ • Reserva - Veículo reserva utilizado                                       ║
// ║                                                                              ║
// ║ Datas (formatadas string e raw DateTime):                                   ║
// ║ • DataSolicitacao/Raw - Data de abertura                                    ║
// ║ • DataDisponibilidade/Raw - Disponibilidade do veículo                      ║
// ║ • DataRecolhimento, DataEntrega, DataDevolucao/Raw - Ciclo de manutenção    ║
// ║ • DataRecebimentoReserva, DataDevolucaoReserva - Período do reserva         ║
// ║                                                                              ║
// ║ Cálculos de Glosa:                                                           ║
// ║ • Descricao, Quantidade, NumItem - Item do contrato                         ║
// ║ • ValorUnitario (double/float) - Valor unitário do serviço                  ║
// ║ • DiasGlosa - Dias calculados para glosa via CROSS APPLY                    ║
// ║ • ValorGlosa (decimal 18,2) - Valor total da glosa                          ║
// ║ • Dias - Diferença entre solicitação e devolução                            ║
// ║                                                                              ║
// ║ Atributos de UI (controle visual e permissões):                             ║
// ║ • Habilitado, Icon - Estado geral                                           ║
// ║ • HabilitadoEditar, OpacityEditar, OpacityTooltipEditarEditar               ║
// ║ • HabilitadoBaixar, ModalBaixarAttrs, OpacityBaixar, Tooltip                ║
// ║ • HabilitadoCancelar, OpacityCancelar, TooltipCancelar                      ║
// ║                                                                              ║
// ║ CONFIGURAÇÃO EF CORE:                                                        ║
// ║ • [Keyless] - Entidade sem chave primária (projeção de view)                ║
// ║ • [Column(TypeName = "decimal(18,2)")] - Mapeamento preciso de ValorGlosa   ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Models
    {
    /// <summary>
    /// Modelo para a ViewGlosa (resultado do SELECT fornecido).
    /// Observações:
    /// - Campos *Data* com formatação dd/MM/yyyy vêm como string.
    /// - Campos "Raw" preservam os tipos de data originais do banco.
    /// - Entidade sem chave (view/projeção).
    /// </summary>
    [Keyless]
    public class ViewGlosa
        {
        // Campo inicial solicitado
        public string PlacaDescricao { get; set; }

        // ===== Chaves e referência ao contrato =====
        public Guid ContratoId { get; set; }

        // ===== Manutenção =====
        public Guid ManutencaoId { get; set; }
        public string NumOS { get; set; }
        public string ResumoOS { get; set; }

        // strings formatadas (dd/MM/yyyy)
        public string DataSolicitacao { get; set; }
        public string DataDisponibilidade { get; set; }
        public string DataRecolhimento { get; set; }
        public string DataRecebimentoReserva { get; set; }
        public string DataDevolucaoReserva { get; set; }
        public string DataEntrega { get; set; }

        // datas cruas
        public DateTime DataSolicitacaoRaw { get; set; }
        public DateTime? DataDisponibilidadeRaw { get; set; } // datas cruas
        public DateTime? DataDevolucaoRaw { get; set; }

        public string StatusOS { get; set; }
        public Guid VeiculoId { get; set; }

        // Derivado (Marca/Modelo) e informações auxiliares
        public string DescricaoVeiculo { get; set; }
        public string Sigla { get; set; }
        public string CombustivelDescricao { get; set; }
        public string Placa { get; set; }

        // Reserva como texto
        public string Reserva { get; set; }

        // ===== Planilha de Glosa (Item do contrato) =====
        public string Descricao { get; set; }
        public int? Quantidade { get; set; }
        public double? ValorUnitario { get; set; } // FLOAT no SQL

        // DataDevolucao formatada com sentinela (dd/MM/yyyy)
        public string DataDevolucao { get; set; }

        // DiasGlosa (via APPLY)
        public int DiasGlosa { get; set; }

        // ValorGlosa (DECIMAL(18,2))
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorGlosa { get; set; }

        // Dias (0 quando sem devolução, senão diff Solicitação->Devolução)
        public int Dias { get; set; }

        // Atributos de UI
        public string Habilitado { get; set; }
        public string Icon { get; set; }

        // Item do contrato
        public int? NumItem { get; set; }

        // Campos "montados"
        public string HabilitadoEditar { get; set; }
        public string OpacityEditar { get; set; }
        public string OpacityTooltipEditarEditar { get; set; }

        public string HabilitadoBaixar { get; set; }
        public string ModalBaixarAttrs { get; set; }
        public string OpacityBaixar { get; set; }
        public string Tooltip { get; set; }

        public string HabilitadoCancelar { get; set; }
        public string OpacityCancelar { get; set; }
        public string TooltipCancelar { get; set; }
        }
    }


