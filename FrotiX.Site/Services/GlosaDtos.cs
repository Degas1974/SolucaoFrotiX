/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: GlosaDtos.cs                                                                            ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: DTOs para cálculo de glosa contratual. Usados por GlosaService.                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: GlosaResumoItemDto (consolidado), GlosaDetalheItemDto (linhas individuais)               ║
   ║ 🔗 DEPS: Nenhuma (POCOs) | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;

namespace FrotiX.Services
    {
    // DTO de Resumo (consolidado por item do contrato)
    public class GlosaResumoItemDto
        {
        public int? NumItem { get; set; }
        public string Descricao { get; set; }
        public int? Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal PrecoTotalMensal { get; set; }
        public decimal PrecoDiario { get; set; }
        public decimal Glosa { get; set; } // numérico para agregações no Grid
        public decimal ValorParaAteste { get; set; }
        }

    // DTO de Detalhes (linhas individuais)
    public class GlosaDetalheItemDto
        {
        public int? NumItem { get; set; }
        public string Descricao { get; set; }
        public string Placa { get; set; }
        public string DataSolicitacao { get; set; }
        public string DataDisponibilidade { get; set; }
        public string DataRecolhimento { get; set; }
        public string DataDevolucao { get; set; } // "Retorno" na UI
        public int DiasGlosa { get; set; }
        }
    }


