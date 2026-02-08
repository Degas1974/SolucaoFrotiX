/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViagensEconomildoRepository.cs                                                         ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para viagens do módulo Economildo.                                                  ║
   ║    Fornece listagens para UI e atualização de registros.                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViagensEconomildoRepository(FrotiXDbContext db)                                                ║
   ║    • GetViagensEconomildoListForDropDown()                                                         ║
   ║    • Update(ViagensEconomildo viagensEconomildo)                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem utiliza a data do registro como texto no dropdown.                                   ║
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
    // │ 🎯 CLASSE: ViagensEconomildoRepository                                                        │
    // │ 📦 HERDA DE: Repository                                                    │
    // │ 🔌 IMPLEMENTA: IViagensEconomildoRepository                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas viagens do módulo Economildo.
    // Centraliza listagens para UI e atualização de registros.
    
    public class ViagensEconomildoRepository : Repository<ViagensEconomildo>, IViagensEconomildoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViagensEconomildoRepository                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViagensEconomildoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViagensEconomildoListForDropDown                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViagensEconomildo, Select                                 │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de viagens Economildo para composição de dropdowns.
        // Exibe a data do registro.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para viagens Economildo.
        public IEnumerable<SelectListItem> GetViagensEconomildoListForDropDown()
            {
            return _db.ViagensEconomildo
            .Select(i => new SelectListItem()
                {
                Text = i.Data.ToString(),
                Value = i.ViagemEconomildoId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViagensEconomildo.FirstOrDefault, _db.Update,               │
        // │                     _db.SaveChanges                                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de uma viagem Economildo no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // viagensEconomildo - Entidade contendo os dados atualizados.
        
        
        // Param viagensEconomildo: Entidade <see cref="ViagensEconomildo"/> com dados atualizados.
        public new void Update(ViagensEconomildo viagensEconomildo)
            {
            var objFromDb = _db.ViagensEconomildo.FirstOrDefault(s => s.ViagemEconomildoId == viagensEconomildo.ViagemEconomildoId);

            _db.Update(viagensEconomildo);
            _db.SaveChanges();

            }


        }
    }
