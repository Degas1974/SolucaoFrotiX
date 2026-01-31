/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewEmpenhosRepository.cs                                                              ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewEmpenhos.                                                       ║
   ║    Fornece visão consolidada de empenhos orçamentários com contratos e fornecedores.               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewEmpenhosRepository(FrotiXDbContext db)                                                     ║
   ║    • GetViewEmpenhosListForDropDown()                                                              ║
   ║    • Update(ViewEmpenhos viewEmpenhos)                                                             ║
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
    // │ 🎯 CLASSE: ViewEmpenhosRepository                                                             │
    // │ 📦 HERDA DE: Repository                                                         │
    // │ 🔌 IMPLEMENTA: IViewEmpenhosRepository                                                        │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de empenhos orçamentários.
    // Fornece listagens para UI com dados consolidados.
    
    public class ViewEmpenhosRepository : Repository<ViewEmpenhos>, IViewEmpenhosRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewEmpenhosRepository                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewEmpenhosRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewEmpenhosListForDropDown                                                │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewEmpenhos, OrderBy, Select                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista da view de empenhos para dropdowns.
        // Ordena pela nota de empenho.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para empenhos.
        public IEnumerable<SelectListItem> GetViewEmpenhosListForDropDown()
            {
            return _db.ViewEmpenhos
            .OrderBy(o => o.NotaEmpenho)
            .Select(i => new SelectListItem()
                {
                Text = i.NotaEmpenho,
                Value = i.EmpenhoId.ToString()
                });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewEmpenhos.FirstOrDefault, _db.Update, _db.SaveChanges   │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com o padrão de repositórios.
        // Views são somente leitura; operação não é recomendada.
        
        
        
        // 📥 PARÂMETROS:
        // viewEmpenhos - Entidade com dados da view.
        
        
        // Param viewEmpenhos: Entidade <see cref="ViewEmpenhos"/>.
        public new void Update(ViewEmpenhos viewEmpenhos)
            {
            var objFromDb = _db.ViewEmpenhos.FirstOrDefault(s => s.EmpenhoId == viewEmpenhos.EmpenhoId);

            _db.Update(viewEmpenhos);
            _db.SaveChanges();

            }
        }
    }
