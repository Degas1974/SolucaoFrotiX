/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewFluxoEconomildo.cs                                                                 ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório da view de fluxo Economildo para consultas e listas de seleção.                    ║
   ║    Fornece métodos auxiliares para preencher dropdowns a partir da view.                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewFluxoEconomildoRepository(FrotiXDbContext db)                                              ║
   ║    • GetViewFluxoEconomildoListForDropDown()                                                        ║
   ║    • Update(ViewFluxoEconomildo viewFluxoEconomildo)                                                ║
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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewFluxoEconomildoRepository                                                     │
    // │ 📦 HERDA DE: Repository                                                 │
    // │ 🔌 IMPLEMENTA: IViewFluxoEconomildoRepository                                                │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório de leitura da view de fluxo Economildo no contexto do FrotiX.
    
    public class ViewFluxoEconomildoRepository : Repository<ViewFluxoEconomildo>, IViewFluxoEconomildoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewFluxoEconomildoRepository                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, DI                                                       │
        // │    ➡️ CHAMA       : Repository                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório da view de fluxo Economildo com o contexto atual.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto de dados do FrotiX
        
        
        // Param db: Instância do contexto utilizada pelo repositório.
        public ViewFluxoEconomildoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewFluxoEconomildoListForDropDown                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers, UI                                             │
        // │    ➡️ CHAMA       : _db.ViewFluxoEconomildo, OrderBy, Select                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Gerar lista de itens para dropdown a partir da view de fluxo Economildo.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens ordenados por data.
        
        
        // Returns: Lista de itens de seleção para a view de fluxo Economildo.
        public IEnumerable<SelectListItem> GetViewFluxoEconomildoListForDropDown()
            {
            return _db.ViewFluxoEconomildo
            .OrderBy(o => o.Data)
            .Select(i => new SelectListItem()
                {
                Text = i.Data.ToString(),
                Value = i.ViagemEconomildoId.ToString()
                }); ; ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : _db.Update, _db.SaveChanges                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Encaminhar atualização de entidade da view quando necessário.
        
        
        
        // 📥 PARÂMETROS:
        // viewFluxoEconomildo - Entidade da view a ser atualizada
        
        
        // Param viewFluxoEconomildo: Entidade de fluxo Economildo a atualizar.
        public new void Update(ViewFluxoEconomildo viewFluxoEconomildo)
            {
            var objFromDb = _db.ViewFluxoEconomildo.FirstOrDefault(s => s.ViagemEconomildoId == viewFluxoEconomildo.ViagemEconomildoId);

            _db.Update(viewFluxoEconomildo);
            _db.SaveChanges();

            }


        }
    }


