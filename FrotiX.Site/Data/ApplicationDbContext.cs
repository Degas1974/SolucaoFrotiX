/* ****************************************************************************************
 * ⚡ ARQUIVO: ApplicationDbContext.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Centralizar o DbContext do ASP.NET Identity (usuários, roles e claims).
 *
 * 📥 ENTRADAS     : Opções de configuração do EF Core (provider, connection string).
 *
 * 📤 SAÍDAS       : Contexto configurado para operações de Identity.
 *
 * 🔗 CHAMADA POR  : Configuração de serviços (Program/Startup) e Identity.
 *
 * 🔄 CHAMA        : IdentityDbContext (base).
 *
 * 📦 DEPENDÊNCIAS : Microsoft.EntityFrameworkCore, ASP.NET Identity.
 *
 * 📝 OBSERVAÇÕES  : Não define DbSets adicionais; usa o modelo padrão do Identity.
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
     * ⚡ CLASSE: ApplicationDbContext
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar o contexto EF Core específico do Identity do FrotiX.
     *
     * 📥 ENTRADAS     : DbContextOptions<ApplicationDbContext>.
     *
     * 📤 SAÍDAS       : Instância configurada do DbContext.
     *
     * 🔗 CHAMADA POR  : ASP.NET Core Identity (UserManager/RoleManager).
     *
     * 🔄 CHAMA        : base(options).
     *
     * 📦 DEPENDÊNCIAS : IdentityDbContext.
     *
     * 📝 OBSERVAÇÕES  : Extensível para adicionar DbSets se necessário no futuro.
     ****************************************************************************************/
    public class ApplicationDbContext : IdentityDbContext
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: ApplicationDbContext (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Configurar o DbContext com as opções fornecidas pelo DI.
         *
         * 📥 ENTRADAS     : options (DbContextOptions<ApplicationDbContext>).
         *
         * 📤 SAÍDAS       : Contexto pronto para operações de Identity.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         *
         * 🔄 CHAMA        : base(options).
         *
         * 📦 DEPENDÊNCIAS : DbContextOptions<ApplicationDbContext>.
         *
         * 📝 OBSERVAÇÕES  : Provider e conexão são definidos na configuração da aplicação.
         ****************************************************************************************/
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
