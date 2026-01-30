/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewAtaFornecedorRepository.cs                                                         ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewAtaFornecedor.                                                  ║
   ║    Disponibiliza listagens consolidadas de atas e fornecedores.                                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewAtaFornecedorRepository(FrotiXDbContext db)                                               ║
   ║    • GetViewAtaFornecedorListForDropDown()                                                         ║
   ║    • Update(ViewAtaFornecedor viewAtaFornecedor)                                                   ║
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
    /// │ 🎯 CLASSE: ViewAtaFornecedorRepository                                                        │
    /// │ 📦 HERDA DE: Repository<ViewAtaFornecedor>                                                    │
    /// │ 🔌 IMPLEMENTA: IViewAtaFornecedorRepository                                                   │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de atas e fornecedores.
    /// Fornece listagens para UI com dados consolidados.
    /// </summary>
    public class ViewAtaFornecedorRepository : Repository<ViewAtaFornecedor>, IViewAtaFornecedorRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewAtaFornecedorRepository                                                   │
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
        public ViewAtaFornecedorRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewAtaFornecedorListForDropDown                                           │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViewAtaFornecedor, OrderBy, Select                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista da view de atas e fornecedores para composição de dropdowns.
        ///    Ordena pelo campo AtaVeiculo.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para atas/fornecedores.</returns>
        public IEnumerable<SelectListItem> GetViewAtaFornecedorListForDropDown()
            {
            return _db.ViewAtaFornecedor
            .OrderBy(o => o.AtaVeiculo)
            .Select(i => new SelectListItem()
                {
                Text = i.AtaVeiculo.ToString(),
                Value = i.AtaId.ToString()
                }); ; ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewAtaFornecedor.FirstOrDefault, _db.Update,               │
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
        ///    viewAtaFornecedor - Entidade com dados da view.
        /// </para>
        /// </summary>
        /// <param name="viewAtaFornecedor">Entidade <see cref="ViewAtaFornecedor"/>.</param>
        public new void Update(ViewAtaFornecedor viewAtaFornecedor)
            {
            var objFromDb = _db.ViewAtaFornecedor.FirstOrDefault(s => s.AtaId == viewAtaFornecedor.AtaId);

            _db.Update(viewAtaFornecedor);
            _db.SaveChanges();

            }


        }
    }
