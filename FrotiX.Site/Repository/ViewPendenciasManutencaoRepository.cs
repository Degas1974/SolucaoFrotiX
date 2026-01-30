/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewPendenciasManutencaoRepository.cs                                                  ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewPendenciasManutencao.                                           ║
   ║    Disponibiliza dados consolidados de pendências de manutenção.                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewPendenciasManutencaoRepository(FrotiXDbContext db)                                         ║
   ║    • GetViewPendenciasManutencaoListForDropDown()                                                  ║
   ║    • Update(ViewPendenciasManutencao viewPendenciasManutencao)                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Views são somente leitura; Update é mantido por compatibilidade.                                ║
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
    /// │ 🎯 CLASSE: ViewPendenciasManutencaoRepository                                                 │
    /// │ 📦 HERDA DE: Repository<ViewPendenciasManutencao>                                             │
    /// │ 🔌 IMPLEMENTA: IViewPendenciasManutencaoRepository                                            │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de pendências de manutenção.
    /// Fornece listagens para UI com dados consolidados.
    /// </summary>
    public class ViewPendenciasManutencaoRepository : Repository<ViewPendenciasManutencao>, IViewPendenciasManutencaoRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewPendenciasManutencaoRepository                                           │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        /// │    ➡️ CHAMA       : base(db)                                                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Inicializar o repositório com o contexto do banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    db - Contexto do banco de dados da aplicação.
        /// </para>
        /// </summary>
        /// <param name="db">Instância de <see cref="FrotiXDbContext"/>.</param>
        public ViewPendenciasManutencaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewPendenciasManutencaoListForDropDown                                   │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViewPendenciasManutencao, OrderBy, Select                 │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista da view de pendências de manutenção para dropdowns.
        ///    Ordena pela data do item.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para pendências de manutenção.</returns>
        public IEnumerable<SelectListItem> GetViewPendenciasManutencaoListForDropDown()
            {
            return _db.ViewPendenciasManutencao
            .OrderBy(o => o.DataItem)
            .Select(i => new SelectListItem()
                {
                Text = i.DataItem.ToString(),
                Value = i.Nome.ToString()
                }); ; ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewPendenciasManutencao.FirstOrDefault, _db.Update,       │
        /// │                     _db.SaveChanges                                                     │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Manter compatibilidade com o padrão de repositórios.
        ///    Views são somente leitura; operação não é recomendada.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    viewPendenciasManutencao - Entidade com dados da view.
        /// </para>
        /// </summary>
        /// <param name="viewPendenciasManutencao">Entidade <see cref="ViewPendenciasManutencao"/>.</param>
        public new void Update(ViewPendenciasManutencao viewPendenciasManutencao)
            {
            var objFromDb = _db.ViewPendenciasManutencao.FirstOrDefault(s => s.ItemManutencaoId == viewPendenciasManutencao.ItemManutencaoId);

            _db.Update(viewPendenciasManutencao);
            _db.SaveChanges();

            }


        }
    }
