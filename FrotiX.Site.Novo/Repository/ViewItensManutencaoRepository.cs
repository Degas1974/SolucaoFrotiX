/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewItensManutencaoRepository.cs                                                       ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewItensManutencao.                                                ║
   ║    Disponibiliza dados consolidados de itens de manutenção.                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewItensManutencaoRepository(FrotiXDbContext db)                                              ║
   ║    • GetViewItensManutencaoListForDropDown()                                                       ║
   ║    • Update(ViewItensManutencao viewItensManutencao)                                               ║
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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewItensManutencaoRepository                                                      │
    // │ 📦 HERDA DE: Repository                                                  │
    // │ 🔌 IMPLEMENTA: IViewItensManutencaoRepository                                                 │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de itens de manutenção.
    // Fornece listagens para UI com dados consolidados.
    
    public class ViewItensManutencaoRepository : Repository<ViewItensManutencao>, IViewItensManutencaoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewItensManutencaoRepository                                                │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewItensManutencaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewItensManutencaoListForDropDown                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewItensManutencao, OrderBy, Select                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista da view de itens de manutenção para dropdowns.
        // Ordena pela data do item.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para itens de manutenção.
        public IEnumerable<SelectListItem> GetViewItensManutencaoListForDropDown()
            {
            return _db.ViewItensManutencao
            .OrderBy(o => o.DataItem)
            .Select(i => new SelectListItem()
                {
                Text = i.DataItem.ToString(),
                Value = i.ItemManutencaoId.ToString()
                }); ; ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewItensManutencao.FirstOrDefault, _db.Update,            │
        // │                     _db.SaveChanges                                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com o padrão de repositórios.
        // Views são somente leitura; operação não é recomendada.
        
        
        
        // 📥 PARÂMETROS:
        // viewItensManutencao - Entidade com dados da view.
        
        
        // Param viewItensManutencao: Entidade <see cref="ViewItensManutencao"/>.
        public new void Update(ViewItensManutencao viewItensManutencao)
            {
            var objFromDb = _db.ViewItensManutencao.FirstOrDefault(s => s.ItemManutencaoId == viewItensManutencao.ItemManutencaoId);

            _db.Update(viewItensManutencao);
            _db.SaveChanges();

            }


        }
    }
