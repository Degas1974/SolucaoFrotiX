// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: IGlosaService.cs                                                    ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Interface para serviço de cálculo de glosa.                                  ║
// ║                                                                              ║
// ║ MÉTODOS:                                                                     ║
// ║ - ListarResumo(): Retorna resumo consolidado por item do contrato            ║
// ║ - ListarDetalhes(): Retorna detalhes individuais por O.S.                    ║
// ║                                                                              ║
// ║ IMPLEMENTAÇÃO: GlosaService                                                  ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 14                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.Collections.Generic;

namespace FrotiX.Services
    {
    /// <summary>
    /// Interface para serviço de cálculo de glosa.
    /// </summary>
    public interface IGlosaService
        {
        IEnumerable<GlosaResumoItemDto> ListarResumo(Guid contratoId, int mes, int ano);
        IEnumerable<GlosaDetalheItemDto> ListarDetalhes(Guid contratoId, int mes, int ano);
        }
    }


