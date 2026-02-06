/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewManutencao.cs                                                                     ║
   ║ 📂 CAMINHO: /Models/Views                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: View SQL de manutenções de veículos (OS, datas, custos).                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: ManutencaoId, ContratoId, VeiculoId, NumOS, ResumoOS, datas e valores                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System                                                                                     ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewManutencao
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de manutenções com dados consolidados
     *
     * 📥 ENTRADAS     : Veículo, contrato, datas, valores, status
     *
     * 📤 SAÍDAS       : Registro somente leitura para relatórios de manutenção
     *
     * 🔗 CHAMADA POR  : Dashboards de manutenção e glosa
     *
     * 🔄 CHAMA        : Não se aplica (view somente leitura)
     ****************************************************************************************/
    public class ViewManutencao
    {
        // [DADOS] Identificador único da manutenção
        public Guid ManutencaoId
        {
            get; set;
        }

        // [DADOS] Identificador do contrato (FK nullable)
        public Guid? ContratoId
        {
            get; set;
        }

        // [DADOS] Identificador do veículo (FK)
        public Guid VeiculoId
        {
            get; set;
        }

        // [DADOS] Número da ordem de serviço
        public string? NumOS
        {
            get; set;
        }

        // [DADOS] Resumo/descrição da OS
        public string? ResumoOS
        {
            get; set;
        }

        // [DADOS] Status atual da OS
        public string? StatusOS
        {
            get; set;
        }

        // [DADOS] Data de solicitação (formatada dd/MM/yyyy)
        public string? DataSolicitacao
        {
            get; set;
        }

        // [DADOS] Data de disponibilidade (formatada)
        public string? DataDisponibilidade
        {
            get; set;
        }

        // [DADOS] Data de recolhimento (formatada)
        public string? DataRecolhimento
        {
            get; set;
        }

        // [DADOS] Data de recebimento de reserva (formatada)
        public string? DataRecebimentoReserva
        {
            get; set;
        }

        // [DADOS] Data de devolução de reserva (formatada)
        public string? DataDevolucaoReserva
        {
            get; set;
        }

        // [DADOS] Data de entrega (formatada)
        public string? DataEntrega
        {
            get; set;
        }

        // [DADOS] Data de devolução (formatada)
        public string? DataDevolucao
        {
            get; set;
        }

        // [DADOS] Data de solicitação (valor bruto do banco)
        public DateTime? DataSolicitacaoRaw
        {
            get; set;
        }

        // [DADOS] Data de devolução (valor bruto do banco)
        public DateTime? DataDevolucaoRaw
        {
            get; set;
        }

        // [DADOS] Descrição completa do veículo
        public string? DescricaoVeiculo
        {
            get; set;
        }

        // [DADOS] Placa + descrição consolidada
        public string? PlacaDescricao
        {
            get; set;
        }

        // [DADOS] Sigla da unidade/combustível
        public string? Sigla
        {
            get; set;
        }

        // [DADOS] Descrição do combustível
        public string? CombustivelDescricao
        {
            get; set;
        }

        // [DADOS] Placa do veículo
        public string? Placa
        {
            get; set;
        }

        // [DADOS] Indicador de reserva (0=Efetivo, 1=Reserva)
        public string? Reserva
        {
            get; set;
        }

        // [DADOS] Descrição do item
        public string? Descricao
        {
            get; set;
        }

        // [DADOS] Quantidade do item
        public int? Quantidade
        {
            get; set;
        }

        // [DADOS] Número de dias em glosa
        public int? DiasGlosa
        {
            get; set;
        }

        // [DADOS] Número de dias da manutenção
        public int? Dias
        {
            get; set;
        }

        // [DADOS] Número do item de contrato
        public int? NumItem
        {
            get; set;
        }

        // [DADOS] Valor unitário (FLOAT = double em C#)
        public double? ValorUnitario
        {
            get; set;
        }

        // [DADOS] Valor total de glosa (DECIMAL 18,2)
        public decimal? ValorGlosa
        {
            get; set;
        }

        // [UI] Flag de habilitação geral
        public string? Habilitado
        {
            get; set;
        }

        // [UI] Classe/nome de ícone Font Awesome
        public string? Icon
        {
            get; set;
        }

        // [UI] Flag para habilitar edição
        public string? HabilitadoEditar
        {
            get; set;
        }

        // [UI] Classe de opacidade para editar
        public string? OpacityEditar
        {
            get; set;
        }

        // [UI] Tooltip de editar
        public string? OpacityTooltipEditarEditar
        {
            get; set;
        }

        // [UI] Flag para habilitar baixa
        public string? HabilitadoBaixar
        {
            get; set;
        }

        // [UI] Atributos do modal de baixa
        public string? ModalBaixarAttrs
        {
            get; set;
        }

        // [UI] Classe de opacidade para baixar
        public string? OpacityBaixar
        {
            get; set;
        }

        // [UI] Tooltip da ação
        public string? Tooltip
        {
            get; set;
        }

        // [UI] Flag para habilitar cancelamento
        public string? HabilitadoCancelar
        {
            get; set;
        }

        // [UI] Classe de opacidade para cancelar
        public string? OpacityCancelar
        {
            get; set;
        }

        // [UI] Tooltip de cancelamento
        public string? TooltipCancelar
        {
            get; set;
        }

        // [UI] Indicador de tipo de veículo
        public string? Veiculo
        {
            get; set;
        }

        // [UI] Descrição de carro reserva
        public string? CarroReserva
        {
            get; set;
        }

        // [DADOS] Flag indicando se reserva foi enviada
        public bool? ReservaEnviado
        {
            get; set;
        }
    }
}

