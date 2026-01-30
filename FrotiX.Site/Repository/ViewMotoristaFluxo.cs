/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewMotoristaFluxo.cs                                                                  ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório da view de fluxo de motoristas para consultas e listas de seleção.                 ║
   ║    Disponibiliza métodos para popular dropdowns por nome de motorista.                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewMotoristaFluxoRepository(FrotiXDbContext db)                                               ║
   ║    • GetViewMotoristaFluxoListForDropDown()                                                         ║
   ║    • Update(ViewMotoristaFluxo viewMotoristaFluxo)                                                  ║
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
    /// │ 🎯 CLASSE: ViewMotoristaFluxoRepository                                                     │
    /// │ 📦 HERDA DE: Repository<ViewMotoristaFluxo>                                                 │
    /// │ 🔌 IMPLEMENTA: IViewMotoristaFluxoRepository                                                │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório de leitura da view de fluxo de motoristas no FrotiX.
    /// </summary>
    public class ViewMotoristaFluxoRepository : Repository<ViewMotoristaFluxo>, IViewMotoristaFluxoRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewMotoristaFluxoRepository                                                 │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UnitOfWork, DI                                                       │
        /// │    ➡️ CHAMA       : Repository<ViewMotoristaFluxo>                                      │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Inicializar o repositório da view de fluxo de motoristas com o contexto atual.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    db - Contexto de dados do FrotiX
        /// </para>
        /// </summary>
        /// <param name="db">Instância do contexto utilizada pelo repositório.</param>
        public ViewMotoristaFluxoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewMotoristaFluxoListForDropDown                                         │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers, UI                                             │
        /// │    ➡️ CHAMA       : _db.ViewMotoristaFluxo, OrderBy, Select                              │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Gerar lista de itens para dropdown com os motoristas da view.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens ordenados por nome do motorista.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção de motoristas.</returns>
        public IEnumerable<SelectListItem> GetViewMotoristaFluxoListForDropDown()
            {
            return _db.ViewMotoristaFluxo
            .OrderBy(o => o.NomeMotorista)
            .Select(i => new SelectListItem()
                {
                Text = i.NomeMotorista.ToString(),
                Value = i.NomeMotorista.ToString()
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
        ///    viewMotoristaFluxo - Entidade da view a ser atualizada
        /// </para>
        /// </summary>
        /// <param name="viewMotoristaFluxo">Entidade de fluxo de motorista a atualizar.</param>
        public new void Update(ViewMotoristaFluxo viewMotoristaFluxo)
            {
            var objFromDb = _db.ViewMotoristaFluxo.FirstOrDefault(s => s.NomeMotorista == viewMotoristaFluxo.NomeMotorista);

            _db.Update(viewMotoristaFluxo);
            _db.SaveChanges();

            }


        }
    }


