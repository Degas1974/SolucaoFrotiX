/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : FrotiXDbContext.OcorrenciaViagem.cs                             ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ Partial class do FrotiXDbContext para adicionar DbSets de OcorrenciaViagem.  ║
║ Inclui tabela principal e views relacionadas.                                ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DBSETS                                                                       ║
║ - OcorrenciaViagem           : Tabela principal de ocorrencias               ║
║ - ViewOcorrenciasViagem      : View consolidada para listagem                ║
║ - ViewOcorrenciasAbertasVeiculo : View de ocorrencias abertas por veiculo    ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Data Documentacao: 28/01/2026                              LOTE: 21          ║
╚══════════════════════════════════════════════════════════════════════════════╝
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
