/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewExisteItemContratoRepository.cs                                                    ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewExisteItemContrato.                                             ║
   ║    Disponibiliza verificação/seleção de itens de contrato existentes.                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewExisteItemContratoRepository(FrotiXDbContext db)                                          ║
   ║    • GetViewExisteItemContratoListForDropDown()                                                    ║
   ║    • Update(ViewExisteItemContrato viewExisteItemContrato)                                         ║
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
    // │ 🎯 CLASSE: ViewExisteItemContratoRepository                                                   │
    // │ 📦 HERDA DE: Repository                                               │
    // │ 🔌 IMPLEMENTA: IViewExisteItemContratoRepository                                              │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de itens de contrato.
    // Fornece listagens para dropdowns e validações de existência.
    
    public class ViewExisteItemContratoRepository : Repository<ViewExisteItemContrato>, IViewExisteItemContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewExisteItemContratoRepository                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewExisteItemContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewExisteItemContratoListForDropDown                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewExisteItemContrato, OrderBy, Select                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de itens de contrato para composição de dropdowns.
        // Ordena pelo número do item.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para itens de contrato.
        public IEnumerable<SelectListItem> GetViewExisteItemContratoListForDropDown()
            {
            return _db.ViewExisteItemContrato
            .OrderBy(o => o.NumItem)
            .Select(i => new SelectListItem()
                {
                Text = i.Descricao.ToString(),
                Value = i.NumItem.ToString()
                }); ; ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewExisteItemContrato.FirstOrDefault, _db.Update,         │
        // │                     _db.SaveChanges                                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com o padrão de repositórios.
        // Views são somente leitura; operação não é recomendada.
        
        
        
        // 📥 PARÂMETROS:
        // viewExisteItemContrato - Entidade com dados da view.
        
        
        // Param viewExisteItemContrato: Entidade <see cref="ViewExisteItemContrato"/>.
        public new void Update(ViewExisteItemContrato viewExisteItemContrato)
            {
            var objFromDb = _db.ViewExisteItemContrato.FirstOrDefault(v => v.RepactuacaoContratoId == viewExisteItemContrato.RepactuacaoContratoId);

            _db.Update(viewExisteItemContrato);
            _db.SaveChanges();

            }


        }
    }
