/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewOcorrenciasAbertasVeiculo.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear a view SQL de ocorrências abertas por veículo.
 *
 * 📥 ENTRADAS     : Dados retornados pela view ViewOcorrenciasAbertasVeiculo.
 *
 * 📤 SAÍDAS       : DTO de leitura para consultas e dashboards.
 *
 * 🔗 CHAMADA POR  : Consultas de ocorrências e relatórios.
 *
 * 🔄 CHAMA        : DataAnnotations, Table.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations,
 *                   System.ComponentModel.DataAnnotations.Schema.
 **************************************************************************************** */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewOcorrenciasAbertasVeiculo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar a view SQL de ocorrências abertas por veículo.
     *
     * 📥 ENTRADAS     : Campos retornados pela view (ocorrências, veículo e motorista).
     *
     * 📤 SAÍDAS       : Registro somente leitura para consumo na aplicação.
     *
     * 🔗 CHAMADA POR  : Camadas de consulta e exibição.
     *
     * 🔄 CHAMA        : Table, Key.
     ****************************************************************************************/
    [Table("ViewOcorrenciasAbertasVeiculo")]
    public class ViewOcorrenciasAbertasVeiculo
    {
        // Identificador da ocorrência.
        [Key]
        public Guid OcorrenciaViagemId { get; set; }
        // Viagem associada.
        public Guid ViagemId { get; set; }
        // Veículo associado.
        public Guid VeiculoId { get; set; }
        // Motorista associado.
        public Guid? MotoristaId { get; set; }
        // Resumo da ocorrência.
        public string? Resumo { get; set; }
        // Descrição detalhada.
        public string? Descricao { get; set; }
        // Imagem da ocorrência.
        public string? ImagemOcorrencia { get; set; }
        // Data de criação.
        public DateTime DataCriacao { get; set; }
        // Usuário de criação.
        public string? UsuarioCriacao { get; set; }
        // Placa do veículo.
        public string? Placa { get; set; }
        // Marca do veículo.
        public string? DescricaoMarca { get; set; }
        // Modelo do veículo.
        public string? DescricaoModelo { get; set; }
        // Descrição completa do veículo.
        public string? VeiculoCompleto { get; set; }
        // Data da viagem.
        public DateTime? DataViagem { get; set; }
        // Número da ficha de vistoria.
        public int? NoFichaVistoria { get; set; }
        // Nome do motorista.
        public string? NomeMotorista { get; set; }
        // Dias em aberto.
        public int? DiasEmAberto { get; set; }
        // Nível de urgência.
        public string? Urgencia { get; set; }
        // Cor relacionada à urgência.
        public string? CorUrgencia { get; set; }
    }
}
