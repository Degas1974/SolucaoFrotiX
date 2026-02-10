/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IGlosaService.cs                                                                        ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Interface do serviço de glosa contratual. Implementado por GlosaService.               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: ListarResumo(contratoId,mes,ano), ListarDetalhes(contratoId,mes,ano)                     ║
   ║ 🔗 DEPS: GlosaResumoItemDto, GlosaDetalheItemDto | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
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


