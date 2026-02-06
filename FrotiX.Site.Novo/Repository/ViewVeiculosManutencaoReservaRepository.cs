/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewVeiculosManutencaoReservaRepository.cs                                             ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewVeiculosManutencaoReserva.                                      ║
   ║    Fornece visão consolidada de veículos em manutenção/reserva.                                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewVeiculosManutencaoReservaRepository(FrotiXDbContext db)                                   ║
   ║    • GetViewVeiculosManutencaoReservaListForDropDown()                                             ║
   ║    • Update(ViewVeiculosManutencaoReserva viewVeiculosManutencaoReserva)                           ║
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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewVeiculosManutencaoReservaRepository                                            │
    // │ 📦 HERDA DE: Repository                                        │
    // │ 🔌 IMPLEMENTA: IViewVeiculosManutencaoReservaRepository                                       │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de veículos em manutenção/reserva.
    // Fornece listagens para UI com dados consolidados.
    
    public class ViewVeiculosManutencaoReservaRepository : Repository<ViewVeiculosManutencaoReserva>, IViewVeiculosManutencaoReservaRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewVeiculosManutencaoReservaRepository                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewVeiculosManutencaoReservaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewVeiculosManutencaoReservaListForDropDown                               │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewVeiculosManutencaoReserva, OrderBy, Select            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista da view de veículos em manutenção/reserva para dropdowns.
        // Ordena pela descrição do veículo.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para veículos em manutenção/reserva.
        public IEnumerable<SelectListItem> GetViewVeiculosManutencaoReservaListForDropDown()
            {
            return _db.ViewVeiculosManutencaoReserva
            .OrderBy(o => o.Descricao)
            .Select(i => new SelectListItem()
                {
                Text = i.Descricao,
                Value = i.VeiculoId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewVeiculosManutencaoReserva.FirstOrDefault, _db.Update,  │
        // │                     _db.SaveChanges                                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com o padrão de repositórios.
        // Views são somente leitura; operação não é recomendada.
        
        
        
        // 📥 PARÂMETROS:
        // viewVeiculosManutencaoReserva - Entidade com dados da view.
        
        
        // Param viewVeiculosManutencaoReserva: Entidade <see cref="ViewVeiculosManutencaoReserva"/>.
        public new void Update(ViewVeiculosManutencaoReserva viewVeiculosManutencaoReserva)
            {
            var objFromDb = _db.ViewVeiculosManutencaoReserva.FirstOrDefault(s => s.VeiculoId == viewVeiculosManutencaoReserva.VeiculoId);

            _db.Update(viewVeiculosManutencaoReserva);
            _db.SaveChanges();
            }
        }
    }
