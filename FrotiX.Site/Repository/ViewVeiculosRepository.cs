/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewVeiculosRepository.cs                                                              ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewVeiculos.                                                       ║
   ║    Fornece visão consolidada dos veículos com marca, modelo e contrato.                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewVeiculosRepository(FrotiXDbContext db)                                                    ║
   ║    • GetViewVeiculosListForDropDown()                                                             ║
   ║    • Update(ViewVeiculos viewVeiculos)                                                            ║
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
    // │ 🎯 CLASSE: ViewVeiculosRepository                                                             │
    // │ 📦 HERDA DE: Repository                                                         │
    // │ 🔌 IMPLEMENTA: IViewVeiculosRepository                                                        │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de veículos.
    // Fornece listagens para UI com dados consolidados.
    
    public class ViewVeiculosRepository : Repository<ViewVeiculos>, IViewVeiculosRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewVeiculosRepository                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewVeiculosRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewVeiculosListForDropDown                                                │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewVeiculos, OrderBy, Select                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista da view de veículos para dropdowns.
        // Ordena pela placa.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para veículos.
        public IEnumerable<SelectListItem> GetViewVeiculosListForDropDown()
            {
            return _db.ViewVeiculos
            .OrderBy(o => o.Placa)
            .Select(i => new SelectListItem()
                {
                Text = i.Placa,
                Value = i.VeiculoId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewVeiculos.FirstOrDefault, _db.Update, _db.SaveChanges   │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com o padrão de repositórios.
        // Views são somente leitura; operação não é recomendada.
        
        
        
        // 📥 PARÂMETROS:
        // viewVeiculos - Entidade com dados da view.
        
        
        // Param viewVeiculos: Entidade <see cref="ViewVeiculos"/>.
        public new void Update(ViewVeiculos viewVeiculos)
            {
            var objFromDb = _db.ViewVeiculos.FirstOrDefault(s => s.VeiculoId == viewVeiculos.VeiculoId);

            _db.Update(viewVeiculos);
            _db.SaveChanges();

            }


        }
    }
