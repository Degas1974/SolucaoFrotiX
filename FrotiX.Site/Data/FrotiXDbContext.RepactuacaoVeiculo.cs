/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : FrotiXDbContext.RepactuacaoVeiculo.cs                           ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ Partial class do FrotiXDbContext para adicionar DbSet de RepactuacaoVeiculo. ║
║ Repactuacao = ajuste de valores contratuais de veiculos ao longo do tempo.   ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DBSETS                                                                       ║
║ - RepactuacaoVeiculo : Historico de repactuacoes contratuais                 ║
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
        public DbSet<RepactuacaoVeiculo> RepactuacaoVeiculo { get; set; }
    }
}
