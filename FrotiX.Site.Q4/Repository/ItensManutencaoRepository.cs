/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ItensManutencaoRepository.cs                                                           ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para itens de manutenção de veículos.                                               ║
   ║    Controla peças, serviços e insumos utilizados em cada ordem de serviço.                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ItensManutencaoRepository(FrotiXDbContext db)                                                  ║
   ║    • GetItensManutencaoListForDropDown()                                                           ║
   ║    • Update(ItensManutencao itensManutencao)                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem exibe o resumo do item e ordena por DataItem.                                        ║
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
    // │ 🎯 CLASSE: ItensManutencaoRepository                                                          │
    // │ 📦 HERDA DE: Repository                                                      │
    // │ 🔌 IMPLEMENTA: IItensManutencaoRepository                                                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelos itens de manutenção.
    // Disponibiliza listagens para UI e atualização de registros.
    
    public class ItensManutencaoRepository : Repository<ItensManutencao>, IItensManutencaoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ItensManutencaoRepository                                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ItensManutencaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetItensManutencaoListForDropDown                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ItensManutencao, OrderBy, Select                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de itens de manutenção para composição de dropdowns.
        // Ordena pela data do item e exibe o resumo.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para itens de manutenção.
        public IEnumerable<SelectListItem> GetItensManutencaoListForDropDown()
            {
            return _db.ItensManutencao
                .OrderBy(o => o.DataItem)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Resumo,
                    Value = i.ItemManutencaoId.ToString()
                    });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ItensManutencao.FirstOrDefault, _db.Update, _db.SaveChanges │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de um item de manutenção no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // itensManutencao - Entidade contendo os dados atualizados.
        
        
        // Param itensManutencao: Entidade <see cref="ItensManutencao"/> com dados atualizados.
        public new void Update(ItensManutencao itensManutencao)
            {
            var objFromDb = _db.ItensManutencao.FirstOrDefault(s => s.ItemManutencaoId == itensManutencao.ItemManutencaoId);

            _db.Update(itensManutencao);
            _db.SaveChanges();

            }


        }
    }
