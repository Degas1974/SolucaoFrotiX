/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewViagensAgendaTodosMesesRepository.cs                                               ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewViagensAgendaTodosMeses.                                        ║
   ║    Fornece visão consolidada de viagens para todos os meses.                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewViagensAgendaTodosMesesRepository(FrotiXDbContext db)                                      ║
   ║    • GetViewViagensAgendaTodosMesesListForDropDown()                                               ║
   ║    • Update(ViewViagensAgendaTodosMeses viewViagensAgendaTodosMeses)                               ║
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
    /// │ 🎯 CLASSE: ViewViagensAgendaTodosMesesRepository                                              │
    /// │ 📦 HERDA DE: Repository<ViewViagensAgendaTodosMeses>                                          │
    /// │ 🔌 IMPLEMENTA: IViewViagensAgendaTodosMesesRepository                                         │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de agenda de viagens (todos os meses).
    /// Fornece listagens para UI com dados consolidados.
    /// </summary>
    public class ViewViagensAgendaTodosMesesRepository : Repository<ViewViagensAgendaTodosMeses>, IViewViagensAgendaTodosMesesRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewViagensAgendaTodosMesesRepository                                        │
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
        public ViewViagensAgendaTodosMesesRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewViagensAgendaTodosMesesListForDropDown                                │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViewViagensAgendaTodosMeses, OrderBy, Select              │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista da view de agenda de viagens para dropdowns.
        ///    Ordena pela data inicial.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para agenda de viagens (todos os meses).</returns>
        public IEnumerable<SelectListItem> GetViewViagensAgendaTodosMesesListForDropDown()
            {
            return _db.ViewViagensAgendaTodosMeses
            .OrderBy(o => o.DataInicial)
            .Select(i => new SelectListItem()
                {
                Text = i.DataInicial.ToString(),
                Value = i.ViagemId.ToString()
                });
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewViagensAgendaTodosMeses.FirstOrDefault, _db.Update,     │
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
        ///    viewViagensAgendaTodosMeses - Entidade com dados da view.
        /// </para>
        /// </summary>
        /// <param name="viewViagensAgendaTodosMeses">Entidade <see cref="ViewViagensAgendaTodosMeses"/>.</param>
        public new void Update(ViewViagensAgendaTodosMeses viewViagensAgendaTodosMeses)
            {
            var objFromDb = _db.ViewViagensAgendaTodosMeses.FirstOrDefault(s => s.ViagemId == viewViagensAgendaTodosMeses.ViagemId);

            _db.Update(viewViagensAgendaTodosMeses);
            _db.SaveChanges();

            }


        }
    }
