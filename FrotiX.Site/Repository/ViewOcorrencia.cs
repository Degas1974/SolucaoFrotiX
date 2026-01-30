/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewOcorrencia.cs                                                                      ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório da view de ocorrências para consultas e listas de seleção.                         ║
   ║    Disponibiliza métodos para popular dropdowns a partir da view de ocorrências.                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewOcorrenciaRepository(FrotiXDbContext db)                                                   ║
   ║    • GetViewOcorrenciaListForDropDown()                                                             ║
   ║    • Update(ViewOcorrencia viewOcorrencia)                                                          ║
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
    /// │ 🎯 CLASSE: ViewOcorrenciaRepository                                                         │
    /// │ 📦 HERDA DE: Repository<ViewOcorrencia>                                                     │
    /// │ 🔌 IMPLEMENTA: IViewOcorrenciaRepository                                                    │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório de leitura da view de ocorrências no FrotiX.
    /// </summary>
    public class ViewOcorrenciaRepository : Repository<ViewOcorrencia>, IViewOcorrenciaRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewOcorrenciaRepository                                                     │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UnitOfWork, DI                                                       │
        /// │    ➡️ CHAMA       : Repository<ViewOcorrencia>                                          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Inicializar o repositório da view de ocorrências com o contexto atual.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    db - Contexto de dados do FrotiX
        /// </para>
        /// </summary>
        /// <param name="db">Instância do contexto utilizada pelo repositório.</param>
        public ViewOcorrenciaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewOcorrenciaListForDropDown                                             │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Services, Controllers, UI                                             │
        /// │    ➡️ CHAMA       : _db.ViewOcorrencia, OrderBy, Select                                  │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Gerar lista de itens para dropdown a partir das ocorrências registradas na view.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens ordenados pela data inicial.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para ocorrências.</returns>
        public IEnumerable<SelectListItem> GetViewOcorrenciaListForDropDown()
            {
            return _db.ViewOcorrencia
            .OrderBy(o => o.DataInicial)
            .Select(i => new SelectListItem()
                {
                Text = i.DataInicial.ToString(),
                Value = i.ViagemId.ToString()
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
        ///    viewOcorrencia - Entidade da view a ser atualizada
        /// </para>
        /// </summary>
        /// <param name="viewOcorrencia">Entidade de ocorrência a atualizar.</param>
        public new void Update(ViewOcorrencia viewOcorrencia)
            {
            var objFromDb = _db.ViewOcorrencia.FirstOrDefault(s => s.ViagemId == viewOcorrencia.ViagemId);

            _db.Update(viewOcorrencia);
            _db.SaveChanges();

            }


        }
    }


