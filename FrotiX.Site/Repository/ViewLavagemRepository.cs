/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewLavagemRepository.cs                                                               ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewLavagem.                                                        ║
   ║    Fornece visão consolidada de lavagens de veículos com dados de lavadores.                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewLavagemRepository(FrotiXDbContext db)                                                      ║
   ║    • GetViewLavagemListForDropDown()                                                               ║
   ║    • Update(ViewLavagem viewLavagem)                                                               ║
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
    /// │ 🎯 CLASSE: ViewLavagemRepository                                                              │
    /// │ 📦 HERDA DE: Repository<ViewLavagem>                                                          │
    /// │ 🔌 IMPLEMENTA: IViewLavagemRepository                                                         │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de lavagens.
    /// Fornece listagens para UI com dados consolidados.
    /// </summary>
    public class ViewLavagemRepository : Repository<ViewLavagem>, IViewLavagemRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewLavagemRepository                                                        │
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
        public ViewLavagemRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewLavagemListForDropDown                                                 │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViewLavagem, OrderBy, Select                               │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista da view de lavagens para dropdowns.
        ///    Ordena pela data da lavagem.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para lavagens.</returns>
        public IEnumerable<SelectListItem> GetViewLavagemListForDropDown()
            {
            return _db.ViewLavagem
            .OrderBy(o => o.Data)
            .Select(i => new SelectListItem()
                {
                Text = i.Data.ToString(),
                Value = i.Lavadores.ToString()
                }); ; ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewLavagem.FirstOrDefault, _db.Update, _db.SaveChanges     │
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
        ///    viewLavagem - Entidade com dados da view.
        /// </para>
        /// </summary>
        /// <param name="viewLavagem">Entidade <see cref="ViewLavagem"/>.</param>
        public new void Update(ViewLavagem viewLavagem)
            {
            var objFromDb = _db.ViewLavagem.FirstOrDefault(s => s.LavagemId == viewLavagem.LavagemId);

            _db.Update(viewLavagem);
            _db.SaveChanges();

            }


        }
    }
