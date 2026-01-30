/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MediaCombustivelRepository.cs                                                          ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para controle de médias de preços de combustível por período.                      ║
   ║    Consolida valores médios mensais organizados por nota fiscal, tipo de combustível e ano/mês.   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • MediaCombustivelRepository(FrotiXDbContext db)                                                ║
   ║    • IEnumerable<SelectListItem> GetMediaCombustivelListForDropDown()                             ║
   ║    • void Update(MediaCombustivel mediacombustivel)                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Tabela com chave composta (CombustivelId, NotaFiscalId, Ano, Mes).                             ║
   ║    Essencial para análises históricas de variação de preços de combustível.                       ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
    {
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: MediaCombustivelRepository                                                         │
    /// │ 📦 HERDA DE: Repository&lt;MediaCombustivel&gt;                                                       │
    /// │ 🔌 IMPLEMENTA: IMediaCombustivelRepository                                                    │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório para gerenciamento de médias históricas de preços de combustível.
    /// Permite análise de variação de custos por tipo de combustível ao longo do tempo.
    /// </summary>
    public class MediaCombustivelRepository : Repository<MediaCombustivel>, IMediaCombustivelRepository
        {
        private new readonly FrotiXDbContext _db;

        public MediaCombustivelRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetMediaCombustivelListForDropDown                                          │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers de relatórios e análises financeiras                   │
        /// │    ➡️ CHAMA       : DbContext.MediaCombustivel, Linq OrderBy/Select                     │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retorna lista de médias de combustível para uso em DropDown.
        ///    Ordenação por Ano, exibindo ano como texto e CombustivelId como valor.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Lista ordenada com Text=Ano e Value=CombustivelId
        /// </para>
        /// </summary>
        /// <returns>Lista de SelectListItem com médias de combustível ordenadas por ano</returns>
        public IEnumerable<SelectListItem> GetMediaCombustivelListForDropDown()
            {
            return _db.MediaCombustivel
                .OrderBy(o => o.Ano)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Ano.ToString(),
                    Value = i.CombustivelId.ToString()
                    });
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers de MediaCombustivel, UnitOfWork                         │
        /// │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualiza registro de média de combustível no banco de dados.
        ///    Utiliza chave composta quádrupla (CombustivelId + NotaFiscalId + Ano + Mes).
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    mediacombustivel - Entidade com chave composta e valores médios atualizados
        /// </para>
        /// </summary>
        /// <param name="mediacombustivel">Entidade MediaCombustivel com dados a serem persistidos</param>
        public new void Update(MediaCombustivel mediacombustivel)
            {
            var objFromDb = _db.MediaCombustivel.FirstOrDefault(s => (s.CombustivelId == mediacombustivel.CombustivelId) && (s.NotaFiscalId == mediacombustivel.NotaFiscalId) && (s.Ano == mediacombustivel.Ano) && (s.Mes == mediacombustivel.Mes));

            _db.Update(mediacombustivel);
            _db.SaveChanges();

            }


        }
    }


