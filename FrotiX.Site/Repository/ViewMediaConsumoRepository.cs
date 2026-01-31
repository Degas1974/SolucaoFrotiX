/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewMediaConsumoRepository.cs                                                          ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewMediaConsumo.                                                   ║
   ║    Disponibiliza dados consolidados de média de consumo por veículo.                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewMediaConsumoRepository(FrotiXDbContext db)                                                 ║
   ║    • GetViewMediaConsumoListForDropDown()                                                          ║
   ║    • Update(ViewMediaConsumo viewMediaConsumo)                                                     ║
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
    // │ 🎯 CLASSE: ViewMediaConsumoRepository                                                         │
    // │ 📦 HERDA DE: Repository                                                     │
    // │ 🔌 IMPLEMENTA: IViewMediaConsumoRepository                                                    │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de média de consumo.
    // Fornece listagens para UI com dados consolidados.
    
    public class ViewMediaConsumoRepository : Repository<ViewMediaConsumo>, IViewMediaConsumoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewMediaConsumoRepository                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewMediaConsumoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewMediaConsumoListForDropDown                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewMediaConsumo, OrderBy, Select                          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista da view de média de consumo para dropdowns.
        // Ordena pelo consumo geral.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para média de consumo.
        public IEnumerable<SelectListItem> GetViewMediaConsumoListForDropDown()
            {
            return _db.ViewMediaConsumo
            .OrderBy(o => o.ConsumoGeral)
            .Select(i => new SelectListItem()
                {
                Text = i.ConsumoGeral.ToString(),
                Value = i.VeiculoId.ToString()
                }); ; ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewMediaConsumo.FirstOrDefault, _db.Update, _db.SaveChanges│
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com o padrão de repositórios.
        // Views são somente leitura; operação não é recomendada.
        
        
        
        // 📥 PARÂMETROS:
        // viewMediaConsumo - Entidade com dados da view.
        
        
        // Param viewMediaConsumo: Entidade <see cref="ViewMediaConsumo"/>.
        public new void Update(ViewMediaConsumo viewMediaConsumo)
            {
            var objFromDb = _db.ViewMediaConsumo.FirstOrDefault(s => s.VeiculoId == viewMediaConsumo.VeiculoId);

            _db.Update(viewMediaConsumo);
            _db.SaveChanges();

            }


        }
    }
