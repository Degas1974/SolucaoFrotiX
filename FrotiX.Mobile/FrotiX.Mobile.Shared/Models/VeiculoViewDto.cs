using System;

namespace FrotiX.Mobile.Shared.Models
{
    /// <summary>
    /// DTO que representa a estrutura retornada pela ViewVeiculos da API
    /// Esta view já retorna o campo VeiculoCompleto montado (Placa - Marca Modelo)
    /// </summary>
    public class VeiculoViewDto
    {
        /// <summary>
        /// ID do veículo
        /// </summary>
        public Guid VeiculoId { get; set; }

        /// <summary>
        /// Placa do veículo (ex: "ABC-1234")
        /// </summary>
        public string Placa { get; set; } = string.Empty;

        /// <summary>
        /// Veículo completo já montado pela View
        /// Formato: "ABC-1234 - Volkswagen Gol"
        /// </summary>
        public string VeiculoCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o veículo pertence à frota do Economildo
        /// Apenas veículos com Economildo = true devem ser sincronizados
        /// </summary>
        public bool Economildo { get; set; }

        /// <summary>
        /// Status do veículo (ativo/inativo)
        /// </summary>
        public bool Status { get; set; }
    }
}
