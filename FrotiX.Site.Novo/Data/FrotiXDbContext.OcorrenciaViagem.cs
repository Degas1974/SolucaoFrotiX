/* ****************************************************************************************
 * ⚡ ARQUIVO: FrotiXDbContext.OcorrenciaViagem.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Complementar o DbContext com ocorrências de viagem e views correlatas.
 *
 * 📥 ENTRADAS     : Nenhuma (declarações de DbSet).
 *
 * 📤 SAÍDAS       : Exposição de tabelas/views no contexto principal.
 *
 * 🔗 CHAMADA POR  : FrotiXDbContext (partial).
 *
 * 🔄 CHAMA        : DbSet<OcorrenciaViagem>, DbSet<ViewOcorrenciasViagem>,
 *                   DbSet<ViewOcorrenciasAbertasVeiculo>.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Models, Microsoft.EntityFrameworkCore.
 **************************************************************************************** */

using FrotiX.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Data
{
    /****************************************************************************************
     * ⚡ CLASSE PARCIAL: FrotiXDbContext (OcorrenciaViagem)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Disponibilizar ocorrências de viagem e views para consultas.
     *
     * 📥 ENTRADAS     : Nenhuma (DbSets).
     *
     * 📤 SAÍDAS       : Propriedades DbSet para uso em queries.
     *
     * 🔗 CHAMADA POR  : FrotiXDbContext.
     ****************************************************************************************/
    public partial class FrotiXDbContext
    {
        public DbSet<OcorrenciaViagem> OcorrenciaViagem { get; set; }
        public DbSet<ViewOcorrenciasViagem> ViewOcorrenciasViagem { get; set; }
        public DbSet<ViewOcorrenciasAbertasVeiculo> ViewOcorrenciasAbertasVeiculo { get; set; }
    }
}
