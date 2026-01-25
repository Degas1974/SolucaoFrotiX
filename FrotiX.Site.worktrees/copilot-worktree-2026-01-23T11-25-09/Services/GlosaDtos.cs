/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  SERVIÇOS - DTOs DE GLOSA                                                           #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using System;

namespace FrotiX.Services
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GlosaResumoItemDto                                                  ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    DTO de resumo consolidado por item do contrato para cálculo de glosas.    ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
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

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: GlosaDetalheItemDto                                                 ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    DTO com linhas individuais de glosa (detalhamento por veículo/data).      ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
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


