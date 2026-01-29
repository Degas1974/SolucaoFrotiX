/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: FrotiXDbContext.OcorrenciaViagem.cs                                                     ║
   ║ 📂 CAMINHO: /Data                                                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Partial class - DbSets de OcorrenciaViagem e Views relacionadas.                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: OcorrenciaViagem, ViewOcorrenciasViagem, ViewOcorrenciasAbertasVeiculo                   ║
   ║ 🔗 DEPS: FrotiX.Models | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                                       ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using FrotiX.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Data
{
    public partial class FrotiXDbContext
    {
        public DbSet<OcorrenciaViagem> OcorrenciaViagem { get; set; }
        public DbSet<ViewOcorrenciasViagem> ViewOcorrenciasViagem { get; set; }
        public DbSet<ViewOcorrenciasAbertasVeiculo> ViewOcorrenciasAbertasVeiculo { get; set; }
    }
}
