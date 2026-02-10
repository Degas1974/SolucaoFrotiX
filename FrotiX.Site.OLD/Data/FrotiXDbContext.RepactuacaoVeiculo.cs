/* ****************************************************************************************
 * ⚡ ARQUIVO: FrotiXDbContext.RepactuacaoVeiculo.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Complementar o DbContext com o histórico de repactuação de veículos.
 *
 * 📥 ENTRADAS     : Nenhuma (declaração de DbSet).
 *
 * 📤 SAÍDAS       : Exposição da entidade RepactuacaoVeiculo no contexto principal.
 *
 * 🔗 CHAMADA POR  : FrotiXDbContext (partial).
 *
 * 🔄 CHAMA        : DbSet<RepactuacaoVeiculo>.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Models, Microsoft.EntityFrameworkCore.
 **************************************************************************************** */

using FrotiX.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Data
{
    /****************************************************************************************
     * ⚡ CLASSE PARCIAL: FrotiXDbContext (RepactuacaoVeiculo)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Disponibilizar o DbSet de repactuações para consultas e relatórios.
     *
     * 📥 ENTRADAS     : Nenhuma (DbSets).
     *
     * 📤 SAÍDAS       : Propriedade DbSet para acesso aos dados.
     *
     * 🔗 CHAMADA POR  : FrotiXDbContext.
     ****************************************************************************************/
    public partial class FrotiXDbContext
    {
        public DbSet<RepactuacaoVeiculo> RepactuacaoVeiculo { get; set; }
    }
}
