/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Services/IGlosaService.cs                                      ║
 * ║  Descrição: Interface do serviço de glosa contratual.                    ║
 * ║             ListarResumo: Resumo consolidado por item do contrato.       ║
 * ║             ListarDetalhes: Linhas detalhadas com datas e placas.        ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;

namespace FrotiX.Services
    {
    public interface IGlosaService
        {
        IEnumerable<GlosaResumoItemDto> ListarResumo(Guid contratoId, int mes, int ano);
        IEnumerable<GlosaDetalheItemDto> ListarDetalhes(Guid contratoId, int mes, int ano);
        }
    }


