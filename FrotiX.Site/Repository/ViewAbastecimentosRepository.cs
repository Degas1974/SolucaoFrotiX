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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewAbastecimentosRepository                                                       │
    // │ 📦 HERDA DE: Repository                                                   │
    // │ 🔌 IMPLEMENTA: IViewAbastecimentosRepository                                                  │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de abastecimentos.
    // Disponibiliza listagens para UI baseadas em dados consolidados.
    
    public class ViewAbastecimentosRepository : Repository<ViewAbastecimentos>, IViewAbastecimentosRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewAbastecimentosRepository                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewAbastecimentosRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewAbastecimentosListForDropDown                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewAbastecimentos, OrderBy, Select                        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista da view de abastecimentos para composição de dropdowns.
        // Ordena por nome.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para abastecimentos.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewAbastecimentos.FirstOrDefault, _db.Update,             │
        // │                     _db.SaveChanges                                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com o padrão de repositórios.
        // Views são somente leitura; operação não é recomendada.
        
        
        
        // 📥 PARÂMETROS:
        // viewAbastecimentos - Entidade com dados da view.
        
        
        // Param viewAbastecimentos: Entidade <see cref="ViewAbastecimentos"/>.
        public new void Update(ViewAbastecimentos viewAbastecimentos)
            {
            var objFromDb = _db.ViewAbastecimentos.FirstOrDefault(s => s.AbastecimentoId == viewAbastecimentos.AbastecimentoId);

            _db.Update(viewAbastecimentos);
            _db.SaveChanges();

            }


        }
    }
