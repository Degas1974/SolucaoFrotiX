/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewSetoresRepository.cs                                                               ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewSetores.                                                        ║
   ║    Fornece visão consolidada de setores solicitantes com estatísticas de viagens.                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewSetoresRepository(FrotiXDbContext db)                                                     ║
   ║    • GetViewSetoresListForDropDown()                                                              ║
   ║    • Update(ViewSetores viewSetores)                                                              ║
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
    // │ 🎯 CLASSE: ViewSetoresRepository                                                              │
    // │ 📦 HERDA DE: Repository                                                          │
    // │ 🔌 IMPLEMENTA: IViewSetoresRepository                                                         │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de setores solicitantes.
    // Fornece listagens para UI com dados consolidados.
    
    public class ViewSetoresRepository : Repository<ViewSetores>, IViewSetoresRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewSetoresRepository                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewSetoresRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewSetoresListForDropDown                                                │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewSetores, OrderBy, Select                               │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista da view de setores para dropdowns.
        // Ordena pelo nome do setor.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para setores.
        public IEnumerable<SelectListItem> GetViewSetoresListForDropDown()
            {
            return _db.ViewSetores
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome.ToString(),
                Value = i.SetorSolicitanteId.ToString()
                }); ; ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewSetores.FirstOrDefault, _db.Update, _db.SaveChanges     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com o padrão de repositórios.
        // Views são somente leitura; operação não é recomendada.
        
        
        
        // 📥 PARÂMETROS:
        // viewSetores - Entidade com dados da view.
        
        
        // Param viewSetores: Entidade <see cref="ViewSetores"/>.
        public new void Update(ViewSetores viewSetores)
            {
            var objFromDb = _db.ViewSetores.FirstOrDefault(s => s.SetorSolicitanteId == viewSetores.SetorSolicitanteId);

            _db.Update(viewSetores);
            _db.SaveChanges();

            }


        }
    }
