/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewVeiculosManutencaoRepository.cs                                                    ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewVeiculosManutencao.                                             ║
   ║    Fornece visão consolidada de veículos com dados de manutenção.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewVeiculosManutencaoRepository(FrotiXDbContext db)                                          ║
   ║    • GetViewVeiculosManutencaoListForDropDown()                                                    ║
   ║    • Update(ViewVeiculosManutencao viewVeiculosManutencao)                                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Views são somente leitura; Update é mantido por compatibilidade.                                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Repository
    {
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: ViewVeiculosManutencaoRepository                                                   │
    /// │ 📦 HERDA DE: Repository<ViewVeiculosManutencao>                                               │
    /// │ 🔌 IMPLEMENTA: IViewVeiculosManutencaoRepository                                              │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de veículos em manutenção.
    /// Fornece listagens para UI com dados consolidados.
    /// </summary>
    public class ViewVeiculosManutencaoRepository : Repository<ViewVeiculosManutencao>, IViewVeiculosManutencaoRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewVeiculosManutencaoRepository                                             │
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
        public ViewVeiculosManutencaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewVeiculosManutencaoListForDropDown                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViewVeiculosManutencao, OrderBy, Select                    │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista da view de veículos em manutenção para dropdowns.
        ///    Ordena pela descrição do veículo.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para veículos em manutenção.</returns>
        public IEnumerable<SelectListItem> GetViewVeiculosManutencaoListForDropDown()
            {
            return _db.ViewVeiculosManutencao
            .OrderBy(o => o.Descricao)
            .Select(i => new SelectListItem()
                {
                Text = i.Descricao,
                Value = i.VeiculoId.ToString()
                }); ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewVeiculosManutencao.FirstOrDefault, _db.Update,         │
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
        ///    viewVeiculosManutencao - Entidade com dados da view.
        /// </para>
        /// </summary>
        /// <param name="viewVeiculosManutencao">Entidade <see cref="ViewVeiculosManutencao"/>.</param>
        public new void Update(ViewVeiculosManutencao viewVeiculosManutencao)
            {
            var objFromDb = _db.ViewVeiculosManutencao.FirstOrDefault(s => s.VeiculoId == viewVeiculosManutencao.VeiculoId);

            _db.Update(viewVeiculosManutencao);
            _db.SaveChanges();
            }
        }
    }
