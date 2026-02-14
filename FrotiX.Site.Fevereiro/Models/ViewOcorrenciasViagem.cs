/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewOcorrenciasViagem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear a view SQL de ocorrências de viagens.
 *
 * 📥 ENTRADAS     : Campos retornados pela view ViewOcorrenciasViagem.
 *
 * 📤 SAÍDAS       : DTO de leitura para consultas e relatórios.
 *
 * 🔗 CHAMADA POR  : Consultas de ocorrências e dashboards.
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
     * ⚡ MODEL: ViewOcorrenciasViagem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar a view SQL de ocorrências de viagem.
     *
     * 📥 ENTRADAS     : Ocorrência, viagem, veículo e motorista.
     *
     * 📤 SAÍDAS       : Registro somente leitura.
     *
     * 🔗 CHAMADA POR  : Camadas de consulta e exibição.
     *
     * 🔄 CHAMA        : Table, Key.
     ****************************************************************************************/
    [Table("ViewOcorrenciasViagem")]
    public class ViewOcorrenciasViagem
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
        // Status da ocorrência.
        public string? Status { get; set; }
        // Data de criação.
        public DateTime DataCriacao { get; set; }
        // Data de baixa.
        public DateTime? DataBaixa { get; set; }
        // Usuário de criação.
        public string? UsuarioCriacao { get; set; }
        // Usuário de baixa.
        public string? UsuarioBaixa { get; set; }
        // Item de manutenção relacionado.
        public Guid? ItemManutencaoId { get; set; }
        // Observações adicionais.
        public string? Observacoes { get; set; }
        // Data inicial da viagem.
        public DateTime? DataInicial { get; set; }
        // Data final da viagem.
        public DateTime? DataFinal { get; set; }
        // Hora de início.
        public DateTime? HoraInicio { get; set; }
        // Hora de fim.
        public DateTime? HoraFim { get; set; }
        // Número da ficha de vistoria.
        public int? NoFichaVistoria { get; set; }
        // Origem da viagem.
        public string? Origem { get; set; }
        // Destino da viagem.
        public string? Destino { get; set; }
        // Finalidade da viagem.
        public string? FinalidadeViagem { get; set; }
        // Status da viagem.
        public string? StatusViagem { get; set; }
        // Placa do veículo.
        public string? Placa { get; set; }
        // Marca do veículo.
        public string? DescricaoMarca { get; set; }
        // Modelo do veículo.
        public string? DescricaoModelo { get; set; }
        // Descrição completa do veículo.
        public string? VeiculoCompleto { get; set; }
        // Marca/modelo agrupados.
        public string? MarcaModelo { get; set; }
        // Nome do motorista.
        public string? NomeMotorista { get; set; }
        // Foto do motorista.
        public string? FotoMotorista { get; set; }
        // Dias em aberto.
        public int? DiasEmAberto { get; set; }
        // Urgência.
        public string? Urgencia { get; set; }
        // Cor da urgência.
        public string? CorUrgencia { get; set; }
    }
}
