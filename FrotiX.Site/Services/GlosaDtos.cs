// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: GlosaDtos.cs                                                        ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ DTOs para cálculos de glosa de contratos de veículos.                        ║
// ║ Glosa = desconto por dias de indisponibilidade do veículo.                   ║
// ║                                                                              ║
// ║ DTOs INCLUÍDOS:                                                              ║
// ║ - GlosaResumoItemDto: Consolidado por item do contrato (para grid)           ║
// ║   → NumItem, Descricao, Quantidade, ValorUnitario, PrecoTotalMensal          ║
// ║   → PrecoDiario, Glosa, ValorParaAteste                                      ║
// ║                                                                              ║
// ║ - GlosaDetalheItemDto: Linhas individuais por O.S.                           ║
// ║   → NumItem, Descricao, Placa, DataSolicitacao, DataDisponibilidade          ║
// ║   → DataRecolhimento, DataDevolucao, DiasGlosa                               ║
// ║                                                                              ║
// ║ USO:                                                                         ║
// ║ - IGlosaService.ListarResumo() → GlosaResumoItemDto                          ║
// ║ - IGlosaService.ListarDetalhes() → GlosaDetalheItemDto                       ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 14                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;

namespace FrotiX.Services
    {
    /// <summary>
    /// DTO de Resumo de glosa (consolidado por item do contrato).
    /// </summary>
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


