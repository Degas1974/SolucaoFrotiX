/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewAbastecimentosRepository.cs                                                        ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewAbastecimentos.                                                  ║
   ║    Fornece visão consolidada de abastecimentos com veículo e motorista.                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewAbastecimentosRepository(FrotiXDbContext db)                                               ║
   ║    • GetViewAbastecimentosListForDropDown()                                                        ║
   ║    • Update(ViewAbastecimentos viewAbastecimentos)                                                 ║
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
    /// │ 🎯 CLASSE: ViewAbastecimentosRepository                                                       │
    /// │ 📦 HERDA DE: Repository<ViewAbastecimentos>                                                   │
    /// │ 🔌 IMPLEMENTA: IViewAbastecimentosRepository                                                  │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de abastecimentos.
    /// Disponibiliza listagens para UI baseadas em dados consolidados.
    /// </summary>
    public class ViewAbastecimentosRepository : Repository<ViewAbastecimentos>, IViewAbastecimentosRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewAbastecimentosRepository                                                 │
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
        public ViewAbastecimentosRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewAbastecimentosListForDropDown                                          │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViewAbastecimentos, OrderBy, Select                        │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista da view de abastecimentos para composição de dropdowns.
        ///    Ordena por nome.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para abastecimentos.</returns>
        public IEnumerable<SelectListItem> GetViewAbastecimentosListForDropDown()
            {
            return _db.ViewAbastecimentos
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
        /// │    ➡️ CHAMA       : DbContext.ViewAbastecimentos.FirstOrDefault, _db.Update,             │
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
        ///    viewAbastecimentos - Entidade com dados da view.
        /// </para>
        /// </summary>
        /// <param name="viewAbastecimentos">Entidade <see cref="ViewAbastecimentos"/>.</param>
        public new void Update(ViewAbastecimentos viewAbastecimentos)
            {
            var objFromDb = _db.ViewAbastecimentos.FirstOrDefault(s => s.AbastecimentoId == viewAbastecimentos.AbastecimentoId);

            _db.Update(viewAbastecimentos);
            _db.SaveChanges();

            }


        }
    }
