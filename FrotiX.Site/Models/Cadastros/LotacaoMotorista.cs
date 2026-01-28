// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: LotacaoMotorista.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para controle de lotação de motoristas em unidades.                ║
// ║ Mantém histórico de transferências e coberturas entre unidades.             ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • LotacaoMotoristaId [Key] - Identificador único                            ║
// ║ • MotoristaId - Motorista sendo lotado                                      ║
// ║ • MotoristaCoberturaId - Motorista que cobre durante ausência               ║
// ║ • UnidadeId - Unidade de lotação                                            ║
// ║ • DataInicio - Data de início da lotação                                    ║
// ║ • DataFim - Data de término (null se ainda lotado)                          ║
// ║ • Lotado - Flag indicando se está atualmente lotado                         ║
// ║ • Motivo - Motivo da mudança de lotação                                     ║
// ║                                                                              ║
// ║ REGRAS DE NEGÓCIO:                                                           ║
// ║ • Motorista pode ter múltiplas lotações históricas                          ║
// ║ • Apenas uma lotação ativa (Lotado = true) por vez                          ║
// ║ • MotoristaCoberturaId usado quando motorista titular se ausenta            ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;

namespace FrotiX.Models
    {
    public class LotacaoMotorista
        {
        [Key]
        public Guid LotacaoMotoristaId { get; set; }

        public Guid MotoristaId { get; set; }

        public Guid MotoristaCoberturaId { get; set; }

        public Guid UnidadeId { get; set; }

        [Required(ErrorMessage = "(A data de início da lotação é obrigatória)")]
        [Display(Name = "Data de Início")]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "Data de Fim")]
        public DateTime? DataFim { get; set; }

        [Display(Name = "Lotado (S/N)")]
        public bool Lotado { get; set; }

        [Required(ErrorMessage = "(O motivo de mudança da lotação é obrigatório)")]
        [Display(Name = "Motivo de Mudança")]
#pragma warning disable CS8632 // A anotação para tipos de referência anuláveis deve ser usada apenas em código em um contexto de anotações '#nullable'.
        public string? Motivo { get; set; }
#pragma warning restore CS8632 // A anotação para tipos de referência anuláveis deve ser usada apenas em código em um contexto de anotações '#nullable'.

        }
    }


