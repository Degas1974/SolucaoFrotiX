/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: AbastecimentoPendente.cs                                                                ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade para abastecimentos pendentes de importação/validação.                       ║
   ║    Armazena dados da planilha com pendências de veículo, motorista ou KM para correção.            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: AbastecimentoPendenteId, AutorizacaoQCard, Placa, CodMotorista, NomeMotorista            ║
   ║    Produto, DataHora, KmAnterior, Km, KmRodado, Litros, ValorUnitario                               ║
   ║    VeiculoId?, MotoristaId?, CombustivelId?, DescricaoPendencia, TipoPendencia, TemSugestao        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System.ComponentModel.DataAnnotations                                                      ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FrotiX.Models
{
    public class AbastecimentoPendente
    {
        [Key]
        public Guid AbastecimentoPendenteId { get; set; }

        // Dados originais da planilha (todos nullable para evitar erro de leitura NULL)
        public int? AutorizacaoQCard { get; set; }
        public string? Placa { get; set; }
        public int? CodMotorista { get; set; }
        public string? NomeMotorista { get; set; }
        public string? Produto { get; set; }
        public DateTime? DataHora { get; set; }
        public int? KmAnterior { get; set; }
        public int? Km { get; set; }
        public int? KmRodado { get; set; }
        public double? Litros { get; set; }
        public double? ValorUnitario { get; set; }

        // IDs identificados (podem ser nulos se não encontrados)
        public Guid? VeiculoId { get; set; }
        public Guid? MotoristaId { get; set; }
        public Guid? CombustivelId { get; set; }

        // Descrição das pendências/erros
        [MaxLength(2000)]
        public string? DescricaoPendencia { get; set; }

        // Tipo principal do erro (para facilitar filtros)
        [MaxLength(50)]
        public string? TipoPendencia { get; set; }

        // Sugestão de correção (para erros de KM)
        public bool TemSugestao { get; set; }
        [MaxLength(20)]
        public string? CampoCorrecao { get; set; }
        public int? ValorAtualErrado { get; set; }
        public int? ValorSugerido { get; set; }
        [MaxLength(500)]
        public string? JustificativaSugestao { get; set; }
        public double? MediaConsumoVeiculo { get; set; }

        // Controle
        public DateTime DataImportacao { get; set; }
        public int NumeroLinhaOriginal { get; set; }
        [MaxLength(255)]
        public string? ArquivoOrigem { get; set; }

        // Status da pendência
        public int Status { get; set; } // 0 = Pendente, 1 = Resolvida, 2 = Ignorada

        // Relacionamentos virtuais (opcionais) - NÃO VALIDAR
        [ForeignKey("VeiculoId")]
        [ValidateNever]
        public virtual Veiculo? Veiculo { get; set; }

        [ForeignKey("MotoristaId")]
        [ValidateNever]
        public virtual Motorista? Motorista { get; set; }

        [ForeignKey("CombustivelId")]
        [ValidateNever]
        public virtual Combustivel? Combustivel { get; set; }
    }
}
