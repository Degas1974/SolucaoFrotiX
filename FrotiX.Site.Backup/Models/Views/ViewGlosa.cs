/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewGlosa.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL de glosas de contratos (dados financeiros/manutenção)
 *
 * 📥 ENTRADAS     : Contratos, manutenção, itens, datas e valores de glosa
 *
 * 📤 SAÍDAS       : DTO somente leitura para relatórios financeiros
 *
 * 🔗 CHAMADA POR  : Dashboards de glosa e relatórios de manutenção
 *
 * 🔄 CHAMA        : Microsoft.EntityFrameworkCore.Keyless
 *
 * 📦 DEPENDÊNCIAS : Microsoft.EntityFrameworkCore, System.ComponentModel.DataAnnotations
 **************************************************************************************** */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Models
    {

    // Modelo para a ViewGlosa (resultado do SELECT fornecido).
    // Observações:
    // - Campos *Data* com formatação dd/MM/yyyy vêm como string.
    // - Campos "Raw" preservam os tipos de data originais do banco.
    // - Entidade sem chave (view/projeção).

    [Keyless]
    /****************************************************************************************
     * ⚡ MODEL: ViewGlosa
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de glosas com itens de contrato
     *
     * 📥 ENTRADAS     : Contrato, manutenção, veículo, datas, valores
     *
     * 📤 SAÍDAS       : Registro somente leitura (Keyless) para relatórios
     *
     * 🔗 CHAMADA POR  : Consultas de glosa e dashboards financeiros
     *
     * 🔄 CHAMA        : Não se aplica (view somente leitura)
     ****************************************************************************************/
    public class ViewGlosa
    {
        // [DADOS] Placa do veículo + descrição completa
        public string PlacaDescricao { get; set; }

        // [DADOS] Identificador do contrato (FK)
        public Guid ContratoId { get; set; }

        // [DADOS] Identificador da manutenção
        public Guid ManutencaoId { get; set; }
        // [DADOS] Número da ordem de serviço
        public string NumOS { get; set; }
        // [DADOS] Resumo/descrição da OS
        public string ResumoOS { get; set; }

        // [DADOS] Data de solicitação (formatada dd/MM/yyyy)
        public string DataSolicitacao { get; set; }
        // [DADOS] Data de disponibilidade (formatada)
        public string DataDisponibilidade { get; set; }
        // [DADOS] Data de recolhimento (formatada)
        public string DataRecolhimento { get; set; }
        // [DADOS] Data de recebimento da reserva (formatada)
        public string DataRecebimentoReserva { get; set; }
        // [DADOS] Data de devolução da reserva (formatada)
        public string DataDevolucaoReserva { get; set; }
        // [DADOS] Data de entrega (formatada)
        public string DataEntrega { get; set; }

        // [DADOS] Data de solicitação (valor bruto do banco)
        public DateTime DataSolicitacaoRaw { get; set; }
        // [DADOS] Data de disponibilidade (valor bruto)
        public DateTime? DataDisponibilidadeRaw { get; set; }
        // [DADOS] Data de devolução (valor bruto)
        public DateTime? DataDevolucaoRaw { get; set; }

        // [DADOS] Status da ordem de serviço
        public string StatusOS { get; set; }
        // [DADOS] Identificador do veículo
        public Guid VeiculoId { get; set; }

        // [DADOS] Descrição completa do veículo
        public string DescricaoVeiculo { get; set; }
        // [DADOS] Sigla da unidade/setor
        public string Sigla { get; set; }
        // [DADOS] Descrição do combustível
        public string CombustivelDescricao { get; set; }
        // [DADOS] Placa do veículo
        public string Placa { get; set; }

        // [DADOS] Indicador de reserva (0=Efetivo, 1=Reserva)
        public string Reserva { get; set; }

        // [DADOS] Descrição do item de contrato
        public string Descricao { get; set; }
        // [DADOS] Quantidade do item
        public int? Quantidade { get; set; }
        // [DADOS] Valor unitário (FLOAT no SQL)
        public double? ValorUnitario { get; set; }

        // [DADOS] Data de devolução (formatada dd/MM/yyyy)
        public string DataDevolucao { get; set; }

        // [DADOS] Número de dias em glosa
        public int DiasGlosa { get; set; }

        // [DADOS] Valor total de glosa (DECIMAL 18,2)
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorGlosa { get; set; }

        // [DADOS] Número de dias da manutenção
        public int Dias { get; set; }

        // [UI] Indicador se campo está habilitado
        public string Habilitado { get; set; }
        // [UI] Classe de ícone Font Awesome
        public string Icon { get; set; }

        // [DADOS] Número do item de contrato
        public int? NumItem { get; set; }

        // [UI] Flag para habilitar edição
        public string HabilitadoEditar { get; set; }
        // [UI] Classe de opacidade para editar
        public string OpacityEditar { get; set; }
        // [UI] Classe de tooltip para editar
        public string OpacityTooltipEditarEditar { get; set; }

        // [UI] Flag para habilitar baixa
        public string HabilitadoBaixar { get; set; }
        // [UI] Atributos para modal de baixa
        public string ModalBaixarAttrs { get; set; }
        // [UI] Classe de opacidade para baixar
        public string OpacityBaixar { get; set; }
        // [UI] Tooltip da ação
        public string Tooltip { get; set; }

        // [UI] Flag para habilitar cancelamento
        public string HabilitadoCancelar { get; set; }
        // [UI] Classe de opacidade para cancelar
        public string OpacityCancelar { get; set; }
        // [UI] Tooltip de cancelamento
        public string TooltipCancelar { get; set; }
        }
    }

