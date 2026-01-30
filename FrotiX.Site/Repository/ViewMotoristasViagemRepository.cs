/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewMotoristasViagemRepository.cs                                                      ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewMotoristasViagem.                                               ║
   ║    Fornece visão consolidada de motoristas associados a viagens.                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewMotoristasViagemRepository(FrotiXDbContext db)                                             ║
   ║    • GetViewMotoristasViagemListForDropDown()                                                      ║
   ║    • Update(ViewMotoristasViagem viewMotoristasviagem)                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Views são somente leitura; Update é mantido por compatibilidade.                                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Models.Views;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Repository
    {
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: ViewMotoristasViagemRepository                                                     │
    /// │ 📦 HERDA DE: Repository<ViewMotoristasViagem>                                                 │
    /// │ 🔌 IMPLEMENTA: IViewMotoristasViagemRepository                                                │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de motoristas por viagem.
    /// Fornece listagens para UI com dados consolidados.
    /// </summary>
    public class ViewMotoristasViagemRepository : Repository<ViewMotoristasViagem>, IViewMotoristasViagemRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewMotoristasViagemRepository                                               │
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
        public ViewMotoristasViagemRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewMotoristasViagemListForDropDown                                       │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViewMotoristasViagem, OrderBy, Select                      │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista da view de motoristas por viagem para dropdowns.
        ///    Ordena pelo nome.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para motoristas por viagem.</returns>
        public IEnumerable<SelectListItem> GetViewMotoristasViagemListForDropDown()
            {
            return _db.ViewMotoristasViagem
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.MotoristaId.ToString()
                }); ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewMotoristasViagem.FirstOrDefault, _db.Update,           │
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
        ///    viewMotoristasviagem - Entidade com dados da view.
        /// </para>
        /// </summary>
        /// <param name="viewMotoristasviagem">Entidade <see cref="ViewMotoristasViagem"/>.</param>
        public new void Update(ViewMotoristasViagem viewMotoristasviagem)
            {
            var objFromDb = _db.ViewMotoristasViagem.FirstOrDefault(s => s.MotoristaId == viewMotoristasviagem.MotoristaId);

            _db.Update(viewMotoristasviagem);
            _db.SaveChanges();

            }


        }
    }
