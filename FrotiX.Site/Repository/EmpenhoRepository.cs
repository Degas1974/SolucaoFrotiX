/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EmpenhoRepository.cs                                                                   ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para notas de empenho orçamentário vinculadas a contratos.                          ║
   ║    Fornece listagem com JOIN de contrato e atualização de registros.                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • EmpenhoRepository(FrotiXDbContext db)                                                         ║
   ║    • GetEmpenhoListForDropDown()                                                                   ║
   ║    • Update(Empenho empenho)                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem utiliza JOIN com Contrato e usa ContratoId como Value.                               ║
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
    /// │ 🎯 CLASSE: EmpenhoRepository                                                                  │
    /// │ 📦 HERDA DE: Repository<Empenho>                                                              │
    /// │ 🔌 IMPLEMENTA: IEmpenhoRepository                                                             │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável por notas de empenho orçamentário.
    /// Centraliza listagens para UI e atualização de registros.
    /// </summary>
    public class EmpenhoRepository : Repository<Empenho>, IEmpenhoRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: EmpenhoRepository                                                             │
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
        public EmpenhoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetEmpenhoListForDropDown                                                     │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.Empenho, Join(Contrato), OrderBy, Select                   │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista de empenhos com referência ao contrato para dropdowns.
        ///    Formato: "NotaEmpenho (Ano/NumeroContrato)".
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para empenhos.</returns>
        public IEnumerable<SelectListItem> GetEmpenhoListForDropDown()
            {
            return _db.Empenho
            .Join(_db.Contrato, empenho => empenho.ContratoId, contrato => contrato.ContratoId, (empenho, contrato) => new { empenho, contrato })
            .OrderBy(o => o.empenho.NotaEmpenho)
            .Select(i => new SelectListItem()
                {
                Text = i.empenho.NotaEmpenho + "(" + i.contrato.AnoContrato + "/" + i.contrato.NumeroContrato + ")",
                Value = i.contrato.ContratoId.ToString()
                });
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.Empenho.FirstOrDefault, _db.Update, _db.SaveChanges         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualizar os dados de um empenho no banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    empenho - Entidade contendo os dados atualizados.
        /// </para>
        /// </summary>
        /// <param name="empenho">Entidade <see cref="Empenho"/> com dados atualizados.</param>
        public new void Update(Empenho empenho)
            {
            var objFromDb = _db.Empenho.FirstOrDefault(s => s.EmpenhoId == empenho.EmpenhoId);

            _db.Update(empenho);
            _db.SaveChanges();

            }
        }
    }
