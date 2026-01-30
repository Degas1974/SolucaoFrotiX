/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewFluxoEconomildoData.cs                                                             ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório da view de fluxo Economildo por data para consultas e listas de seleção.           ║
   ║    Oferece métodos auxiliares para popular dropdowns a partir da view.                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewFluxoEconomildoDataRepository(FrotiXDbContext db)                                          ║
   ║    • GetViewFluxoEconomildoDataListForDropDown()                                                    ║
   ║    • Update(ViewFluxoEconomildoData viewFluxoEconomildoData)                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A entidade é uma view; o método Update existe por compatibilidade e usa DbContext.Update().     ║
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
    /// │ 🎯 CLASSE: ViewFluxoEconomildoDataRepository                                                 │
    /// │ 📦 HERDA DE: Repository<ViewFluxoEconomildoData>                                             │
    /// │ 🔌 IMPLEMENTA: IViewFluxoEconomildoDataRepository                                            │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório de leitura da view de fluxo Economildo por data no FrotiX.
    /// </summary>
    public class ViewFluxoEconomildoDataRepository : Repository<ViewFluxoEconomildoData>, IViewFluxoEconomildoDataRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewFluxoEconomildoDataRepository                                             │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UnitOfWork, DI                                                       │
        /// │    ➡️ CHAMA       : Repository<ViewFluxoEconomildoData>                                  │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Inicializar o repositório da view de fluxo Economildo por data com o contexto atual.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    db - Contexto de dados do FrotiX
        /// </para>
        /// </summary>
        /// <param name="db">Instância do contexto utilizada pelo repositório.</param>
        public ViewFluxoEconomildoDataRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewFluxoEconomildoDataListForDropDown                                     │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers, UI                                             │
        /// │    ➡️ CHAMA       : _db.ViewFluxoEconomildoData, OrderBy, Select                          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Gerar lista de itens para dropdown a partir da view de fluxo Economildo por data.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens ordenados por data.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção da view de fluxo Economildo por data.</returns>
        public IEnumerable<SelectListItem> GetViewFluxoEconomildoDataListForDropDown()
            {
            return _db.ViewFluxoEconomildoData
            .OrderBy(o => o.Data)
            .Select(i => new SelectListItem()
                {
                Text = i.Data.ToString(),
                Value = i.ViagemEconomildoId.ToString()
                }); ; ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                       │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        /// │    ➡️ CHAMA       : _db.Update, _db.SaveChanges                                           │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Encaminhar atualização de entidade da view quando necessário.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    viewFluxoEconomildoData - Entidade da view a ser atualizada
        /// </para>
        /// </summary>
        /// <param name="viewFluxoEconomildoData">Entidade de fluxo Economildo por data a atualizar.</param>
        public new void Update(ViewFluxoEconomildoData viewFluxoEconomildoData)
            {
            var objFromDb = _db.ViewFluxoEconomildoData.FirstOrDefault(s => s.ViagemEconomildoId == viewFluxoEconomildoData.ViagemEconomildoId);

            _db.Update(viewFluxoEconomildoData);
            _db.SaveChanges();

            }


        }
    }


