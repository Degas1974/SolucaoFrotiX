/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewProcuraFichaRepository.cs                                                          ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewProcuraFicha.                                                   ║
   ║    Disponibiliza dados consolidados de procura de fichas por veículo.                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewProcuraFichaRepository(FrotiXDbContext db)                                                 ║
   ║    • GetViewProcuraFichaListForDropDown()                                                          ║
   ║    • Update(ViewProcuraFicha viewProcuraFicha)                                                     ║
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
    /// │ 🎯 CLASSE: ViewProcuraFichaRepository                                                         │
    /// │ 📦 HERDA DE: Repository<ViewProcuraFicha>                                                     │
    /// │ 🔌 IMPLEMENTA: IViewProcuraFichaRepository                                                    │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de procura de fichas.
    /// Fornece listagens para UI com dados consolidados.
    /// </summary>
    public class ViewProcuraFichaRepository : Repository<ViewProcuraFicha>, IViewProcuraFichaRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewProcuraFichaRepository                                                   │
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
        public ViewProcuraFichaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewProcuraFichaListForDropDown                                           │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViewProcuraFicha, OrderBy, Select                          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista da view de procura de fichas para dropdowns.
        ///    Ordena pela data inicial.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para procura de fichas.</returns>
        public IEnumerable<SelectListItem> GetViewProcuraFichaListForDropDown()
            {
            return _db.ViewProcuraFicha
            .OrderBy(o => o.DataInicial)
            .Select(i => new SelectListItem()
                {
                Text = i.DataInicial.ToString(),
                Value = i.VeiculoId.ToString()
                }); ; ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewProcuraFicha.FirstOrDefault, _db.Update, _db.SaveChanges│
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
        ///    viewProcuraFicha - Entidade com dados da view.
        /// </para>
        /// </summary>
        /// <param name="viewProcuraFicha">Entidade <see cref="ViewProcuraFicha"/>.</param>
        public new void Update(ViewProcuraFicha viewProcuraFicha)
            {
            var objFromDb = _db.ViewProcuraFicha.FirstOrDefault(s => s.VeiculoId == viewProcuraFicha.VeiculoId);

            _db.Update(viewProcuraFicha);
            _db.SaveChanges();

            }


        }
    }
