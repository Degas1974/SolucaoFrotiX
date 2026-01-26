using FrotiXApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotiXApi.Data
{
    public partial class FrotiXDbContext
    {
        public DbSet<OcorrenciaViagem> OcorrenciaViagem { get; set; }
        public DbSet<ViewOcorrenciasViagem> ViewOcorrenciasViagem { get; set; }
        public DbSet<ViewOcorrenciasAbertasVeiculo> ViewOcorrenciasAbertasVeiculo { get; set; }
    }
}