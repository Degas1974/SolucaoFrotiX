using FrotiX.Mobile.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Mobile.Shared.Data
{
    public class FrotiXDbContext : DbContext
    {
        public FrotiXDbContext(DbContextOptions<FrotiXDbContext> options)
            : base(options) { }

        // Entidades essenciais apenas
        public DbSet<Motorista> Motoristas { get; set; } = null!;
        public DbSet<ViagensEconomildo> ViagensEconomildo { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Mapeamento de tabela
            modelBuilder.Entity<Motorista>().ToTable("Motorista");
        }
    }
}
