/* ****************************************************************************************
 * ⚡ ARQUIVO: Multa.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar multas de trânsito com valores, status, vínculos e anexos.
 *
 * 📥 ENTRADAS     : Dados da multa, motoristas, veículos e documentos anexos.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Módulos de multas, financeiro e relatórios.
 *
 * 🔄 CHAMA        : DataAnnotations, ForeignKey, SelectListItem.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Validations, Microsoft.AspNetCore.Mvc.Rendering.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.DDF;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: MultaViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Consolidar dados da multa e listas de seleção usadas na UI.
     *
     * 📥 ENTRADAS     : Multa, dados de pagamento e listas auxiliares.
     *
     * 📤 SAÍDAS       : ViewModel para telas de cadastro/edição.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de multas.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class MultaViewModel
    {
        // Identificador da multa.
        public Guid MultaId
        {
            get; set;
        }

        // Órgão autuante selecionado.
        public Guid? OrgaoAutuanteId
        {
            get; set;
        }

        // Entidade principal do formulário.
        public Multa? Multa
        {
            get; set;
        }

        // Valor pago (quando aplicável).
        public double? ValorPago
        {
            get; set;
        }

        // Data de pagamento.
        public DateTime? DataPagamento
        {
            get; set;
        }

        // Forma de pagamento.
        public string? FormaPagamento
        {
            get; set;
        }

        // Caminho/identificador do comprovante.
        public string? ComprovantePDF
        {
            get; set;
        }

        // Lista de motoristas para seleção.
        public IEnumerable<SelectListItem>? MotoristaList
        {
            get; set;
        }

        // Lista de veículos para seleção.
        public IEnumerable<SelectListItem>? VeiculoList
        {
            get; set;
        }

        // Lista de órgãos autuantes para seleção.
        public IEnumerable<SelectListItem>? OrgaoList
        {
            get; set;
        }

        // Lista de tipos de multa para seleção.
        public IEnumerable<SelectListItem>? TipoMultaList
        {
            get; set;
        }

        // Lista de empenhos para seleção.
        public IEnumerable<SelectListItem>? EmpenhoList
        {
            get; set;
        }
    }

    /****************************************************************************************
     * ⚡ MODEL: Multa
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar uma multa de trânsito e seus vínculos.
     *
     * 📥 ENTRADAS     : Dados da infração, valores e anexos.
     *
     * 📤 SAÍDAS       : Registro persistido para controle e cobrança.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers de multas.
     *
     * 🔄 CHAMA        : ForeignKey, ValidaZero.
     ****************************************************************************************/
    public class Multa
    {
        // Identificador único da multa.
        [Key]
        public Guid MultaId
        {
            get; set;
        }

        // Número da infração.
        [Required(ErrorMessage = "(O número da Infração é obrigatório)")]
        [Display(Name = "Nº da Infração")]
        public string? NumInfracao
        {
            get; set;
        }

        // Data da infração.
        [Required(ErrorMessage = "(A data é obrigatória)")]
        [Display(Name = "Data Infração")]
        public DateTime? Data
        {
            get; set;
        }

        // Hora da infração.
        [Required(ErrorMessage = "(A hora é obrigatória)")]
        [Display(Name = "Hora")]
        public DateTime? Hora
        {
            get; set;
        }

        // Local da infração.
        [Required(ErrorMessage = "(A localização é obrigatória)")]
        [Display(Name = "Localização da Infração")]
        public string? Localizacao
        {
            get; set;
        }

        // Data de vencimento.
        [Display(Name = "Data de Vencimento")]
        public DateTime? Vencimento
        {
            get; set;
        }

        // Valor até o vencimento.
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "(O valor até o vencimento é obrigatório)")]
        [ValidaZero(ErrorMessage = "(O valor até o vencimento é obrigatório)")]
        [Display(Name = "Valor Até Vencimento")]
        public double? ValorAteVencimento
        {
            get; set;
        }

        // Valor após vencimento.
        [ValidaZero(ErrorMessage = "(O valor após o vencimento é obrigatório)")]
        [Required(ErrorMessage = "(O valor após o vencimento é obrigatório)")]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor Após Vencimento")]
        public double? ValorPosVencimento
        {
            get; set;
        }

        // Observações gerais.
        public string? Observacao
        {
            get; set;
        }

        // Anexo da autuação.
        public string? AutuacaoPDF
        {
            get; set;
        }

        // Anexo da penalidade.
        public string? PenalidadePDF
        {
            get; set;
        }

        // Comprovante anexado.
        public string? ComprovantePDF
        {
            get; set;
        }

        // Processo eDoc anexado.
        public string? ProcessoEdocPDF
        {
            get; set;
        }

        // Outros documentos anexados.
        public string? OutrosDocumentosPDF
        {
            get; set;
        }

        // ✅ CORRIGIDO: Campos Boolean agora são nullable
        // Indica se a multa foi paga.
        public bool? Paga
        {
            get; set;
        }

        // Indica se foi enviada para Secle.
        public bool? EnviadaSecle
        {
            get; set;
        }

        // Fase/processo da multa.
        public string? Fase
        {
            get; set;
        }

        // Processo eDoc.
        [Display(Name = "Processo eDoc")]
        public string? ProcessoEDoc
        {
            get; set;
        }

        // Status da multa.
        [Required(ErrorMessage = "(O status é obrigatório)")]
        public string? Status
        {
            get; set;
        }

        // Número da ficha de vistoria da viagem.
        [Display(Name = "Nº Ficha Vistoria da Viagem")]
        public int? NoFichaVistoria
        {
            get; set;
        }

        // Data da notificação.
        [Required(ErrorMessage = "(A data da notificação é obrigatória)")]
        [Display(Name = "Data Notificação")]
        public DateTime? DataNotificacao
        {
            get; set;
        }

        // Data limite para recurso.
        [Required(ErrorMessage = "(A data limite do recurso é obrigatória)")]
        [Display(Name = "Data Limite Reconhecimento")]
        public DateTime? DataLimite
        {
            get; set;
        }

        // Valor pago.
        public double? ValorPago
        {
            get; set;
        }

        // Data de pagamento.
        public DateTime? DataPagamento
        {
            get; set;
        }

        // Forma de pagamento.
        public string? FormaPagamento
        {
            get; set;
        }

        // Contrato do veículo.
        public Guid? ContratoVeiculoId
        {
            get; set;
        }

        // Contrato do motorista.
        public Guid? ContratoMotoristaId
        {
            get; set;
        }

        // Motorista autuado.
        [Required(ErrorMessage = "(O motorista é obrigatório)")]
        [Display(Name = "Motorista")]
        public Guid? MotoristaId
        {
            get; set;
        }

        // Navegação para motorista.
        [ForeignKey("MotoristaId")]
        public virtual Motorista? Motorista
        {
            get; set;
        }

        // Veículo autuado.
        [Required(ErrorMessage = "(O veículo é obrigatório)")]
        [Display(Name = "Veí­culo")]
        public Guid? VeiculoId
        {
            get; set;
        }

        // Navegação para veículo.
        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo
        {
            get; set;
        }

        // Órgão autuante.
        [Required(ErrorMessage = "(O Órgão Autuante é obrigatório)")]
        [Display(Name = "Órgão Autuante")]
        public Guid? OrgaoAutuanteId
        {
            get; set;
        }

        // Navegação para órgão autuante.
        [ForeignKey("OrgaoAutuanteId")]
        public virtual OrgaoAutuante? OrgaoAutuante
        {
            get; set;
        }

        // Tipo de infração/multa.
        [Required(ErrorMessage = "(A infração é obrigatória)")]
        [Display(Name = "Infração")]
        public Guid? TipoMultaId
        {
            get; set;
        }

        // Navegação para tipo de multa.
        [ForeignKey("TipoMultaId")]
        public virtual TipoMulta? TipoMulta
        {
            get; set;
        }

        // Empenho relacionado.
        [Display(Name = "Empenho")]
        public Guid? EmpenhoMultaId
        {
            get; set;
        }

        // Navegação para empenho.
        [ForeignKey("EmpenhoMultaId")]
        public virtual EmpenhoMulta? EmpenhoMulta
        {
            get; set;
        }

        // Ata vinculada ao veículo.
        public Guid? AtaVeiculoId
        {
            get; set;
        }
    }
}
