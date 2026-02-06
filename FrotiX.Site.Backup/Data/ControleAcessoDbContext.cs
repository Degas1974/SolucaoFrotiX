/* ****************************************************************************************
 * ⚡ ARQUIVO: ControleAcessoDbContext.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar o contexto de controle de acesso (recursos e permissões).
 *
 * 📥 ENTRADAS     : Opções de configuração do EF Core (provider/connection string).
 *
 * 📤 SAÍDAS       : DbContext configurado para consultas e gravações.
 *
 * 🔗 CHAMADA POR  : Configuração de serviços e módulos de segurança.
 *
 * 🔄 CHAMA        : DbContext (base) e mapeamentos de Recurso/ControleAcesso.
 *
 * 📦 DEPENDÊNCIAS : Microsoft.EntityFrameworkCore, FrotiX.Models.
 *
 * ⚠️ ATENÇÃO      : ControleAcesso usa chave composta (UsuarioId + RecursoId).
 *
 * 📝 OBSERVAÇÕES  : Mapeia hierarquia de Recurso com auto-relacionamento e delete restrito.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.Text;
using FrotiX.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Data
{
    /****************************************************************************************
     * ⚡ CLASSE: ControleAcessoDbContext
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Definir DbSets e regras de mapeamento do módulo de acesso.
     *
     * 📥 ENTRADAS     : DbContextOptions<ControleAcessoDbContext>.
     *
     * 📤 SAÍDAS       : Contexto pronto para operações de segurança.
     *
     * 🔗 CHAMADA POR  : ASP.NET Core DI.
     *
     * 🔄 CHAMA        : base(options) e Database.SetCommandTimeout.
     *
     * 📦 DEPENDÊNCIAS : DbContext.
     *
     * ⚠️ ATENÇÃO      : Chave composta em ControleAcesso (UsuarioId + RecursoId).
     ****************************************************************************************/
    public class ControleAcessoDbContext  : DbContext
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: ControleAcessoDbContext (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Configurar o DbContext com opções do EF Core.
         *
         * 📥 ENTRADAS     : options (DbContextOptions<ControleAcessoDbContext>).
         *
         * 📤 SAÍDAS       : Instância configurada com timeout estendido.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         *
         * 🔄 CHAMA        : base(options), Database.SetCommandTimeout.
         *
         * 📝 OBSERVAÇÕES  : Timeout elevado para operações administrativas.
         ****************************************************************************************/
        public ControleAcessoDbContext(DbContextOptions<ControleAcessoDbContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(9000);
        }

        // Recurso para tabelas com múltiplas chaves primárias
        //====================================================
        /****************************************************************************************
         * ⚡ FUNÇÃO: OnModelCreating
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Configurar chaves compostas e relacionamento hierárquico de recursos.
         *
         * 📥 ENTRADAS     : modelBuilder.
         *
         * 📤 SAÍDAS       : Mapeamentos adicionados ao modelo EF Core.
         *
         * 🔗 CHAMADA POR  : EF Core durante a construção do modelo.
         *
         * 🔄 CHAMA        : modelBuilder.Entity<ControleAcesso>(), modelBuilder.Entity<Recurso>().
         *
         * ⚠️ ATENÇÃO      : ControleAcesso usa chave composta (UsuarioId + RecursoId).
         ****************************************************************************************/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControleAcesso>().HasKey(ca => new { ca.UsuarioId, ca.RecursoId});

            // Configuração da hierarquia de Recursos (auto-relacionamento)
            modelBuilder.Entity<Recurso>()
                .HasOne(r => r.Parent)
                .WithMany(r => r.Children)
                .HasForeignKey(r => r.ParentId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata
        }


        public DbSet<Recurso> Recurso { get; set; }
        public DbSet<ControleAcesso> ControleAcesso { get; set; }

    }
}
