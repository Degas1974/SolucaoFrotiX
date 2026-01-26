using System;

namespace FrotiXApi.Models
{
    public class ViewVeiculos
    {
        public Guid VeiculoId
        {
            get; set;
        }

        public string? Placa
        {
            get; set;
        }

        public string? MarcaModelo
        {
            get; set;
        }

        public string? Sigla
        {
            get; set;
        }

        public string? Descricao
        {
            get; set;
        }

        public decimal? Consumo
        {
            get; set;
        }

        public int? Quilometragem
        {
            get; set;
        }

        public string? OrigemVeiculo
        {
            get; set;
        }

        public string? DataAlteracao
        {
            get; set;
        }

        public string? NomeCompleto
        {
            get; set;
        }

        public string? VeiculoCompleto
        {
            get; set;
        }

        public string? VeiculoReserva
        {
            get; set;
        }

        public bool Status
        {
            get; set;
        }

        public bool Economildo
        {
            get; set;
        }

        public Guid CombustivelId
        {
            get; set;
        }
    }
}