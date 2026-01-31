/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: AbastecimentoPendente.cs                                                                ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Registrar abastecimentos pendentes de importação/validação de planilhas.             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: AbastecimentoPendente                                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FrotiX.Models
{
    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa um abastecimento pendente de correção/validação após importação.
    // ==================================================================================================
    public class AbastecimentoPendente
    {
        // Identificador único do registro.
        [Key]
        public Guid AbastecimentoPendenteId { get; set; }

        // Dados originais da planilha (todos nullable para evitar erro de leitura NULL)
        // Número da autorização QCard.
        public int? AutorizacaoQCard { get; set; }

        // Placa informada na planilha.
        public string? Placa { get; set; }

        // Código do motorista.
        public int? CodMotorista { get; set; }

        // Nome do motorista.
        public string? NomeMotorista { get; set; }

        // Produto informado (ex.: combustível).
        public string? Produto { get; set; }

        // Data/hora do abastecimento.
        public DateTime? DataHora { get; set; }

        // Quilometragem anterior.
        public int? KmAnterior { get; set; }

        // Quilometragem informada.
        public int? Km { get; set; }

        // Quilometragem rodada.
        public int? KmRodado { get; set; }

        // Litros abastecidos.
        public double? Litros { get; set; }

        // Valor unitário.
        public double? ValorUnitario { get; set; }

        // IDs identificados (podem ser nulos se não encontrados)
        // Veículo identificado.
        public Guid? VeiculoId { get; set; }

        // Motorista identificado.
        public Guid? MotoristaId { get; set; }

        // Combustível identificado.
        public Guid? CombustivelId { get; set; }

        // Descrição das pendências/erros
        // Detalhe textual da pendência.
        [MaxLength(2000)]
        public string? DescricaoPendencia { get; set; }

        // Tipo principal do erro (para facilitar filtros)
        // Tipo da pendência.
        [MaxLength(50)]
        public string? TipoPendencia { get; set; }

        // Sugestão de correção (para erros de KM)
        // Indica se há sugestão automática.
        public bool TemSugestao { get; set; }

        // Campo que precisa de correção.
        [MaxLength(20)]
        public string? CampoCorrecao { get; set; }

        // Valor atual incorreto.
        public int? ValorAtualErrado { get; set; }

        // Valor sugerido.
        public int? ValorSugerido { get; set; }

        // Justificativa da sugestão.
        [MaxLength(500)]
        public string? JustificativaSugestao { get; set; }

        // Média de consumo do veículo.
        public double? MediaConsumoVeiculo { get; set; }

        // Controle
        // Data de importação do arquivo.
        public DateTime DataImportacao { get; set; }

        // Número da linha original.
        public int NumeroLinhaOriginal { get; set; }

        // Arquivo de origem.
        [MaxLength(255)]
        public string? ArquivoOrigem { get; set; }

        // Status da pendência
        // 0 = Pendente, 1 = Resolvida, 2 = Ignorada.
        public int Status { get; set; } // 0 = Pendente, 1 = Resolvida, 2 = Ignorada

        // Relacionamentos virtuais (opcionais) - NÃO VALIDAR
        // Navegação para veículo.
        [ForeignKey("VeiculoId")]
        [ValidateNever]
        public virtual Veiculo? Veiculo { get; set; }

        // Navegação para motorista.
        [ForeignKey("MotoristaId")]
        [ValidateNever]
        public virtual Motorista? Motorista { get; set; }

        // Navegação para combustível.
        [ForeignKey("CombustivelId")]
        [ValidateNever]
        public virtual Combustivel? Combustivel { get; set; }
    }
}
