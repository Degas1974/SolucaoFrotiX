/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewViagensAgendaRepository.cs                                                         ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewViagensAgenda.                                                  ║
   ║    Fornece visão consolidada de viagens para agenda.                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewViagensAgendaRepository(FrotiXDbContext db)                                               ║
   ║    • GetViewViagensAgendaListForDropDown()                                                        ║
   ║    • Update(ViewViagensAgenda viewViagensAgenda)                                                  ║
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
using NPOI.SS.Formula.Functions;

namespace FrotiX.Repository
    {
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewViagensAgendaRepository                                                        │
    // │ 📦 HERDA DE: Repository                                                    │
    // │ 🔌 IMPLEMENTA: IViewViagensAgendaRepository                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de agenda de viagens.
    // Fornece listagens para UI com dados consolidados.
    
    public class ViewViagensAgendaRepository
        : Repository<ViewViagensAgenda>,
            IViewViagensAgendaRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewViagensAgendaRepository                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewViagensAgendaRepository(FrotiXDbContext db)
            : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewViagensAgendaListForDropDown                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewViagensAgenda, OrderBy, Select                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista da view de agenda de viagens para dropdowns.
        // Ordena pela data inicial.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para agenda de viagens.
        public IEnumerable<SelectListItem> GetViewViagensAgendaListForDropDown()
            {
            return _db
                .ViewViagensAgenda.OrderBy(o => o.DataInicial)
                .Select(i => new SelectListItem()
                    {
                    Text = i.DataInicial.ToString(),
                    Value = i.ViagemId.ToString(),
                    });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewViagensAgenda.FirstOrDefault, _db.Update, _db.SaveChanges│
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com o padrão de repositórios.
        // Views são somente leitura; operação não é recomendada.
        
        
        
        // 📥 PARÂMETROS:
        // viewViagensAgenda - Entidade com dados da view.
        
        
        // Param viewViagensAgenda: Entidade <see cref="ViewViagensAgenda"/>.
        public new void Update(ViewViagensAgenda viewViagensAgenda)
            {
            var objFromDb = _db.ViewViagensAgenda.FirstOrDefault(s =>
                s.ViagemId == viewViagensAgenda.ViagemId
            );

            _db.Update(viewViagensAgenda);
            _db.SaveChanges();
            }
        }
    }
