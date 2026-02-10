/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: PlacaBronzeRepository.cs                                                               ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para placas de bronze vinculadas ao patrimônio veicular.                            ║
   ║    Fornece listagens para UI e atualização de cadastros de placas metálicas.                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • PlacaBronzeRepository(FrotiXDbContext db)                                                      ║
   ║    • GetPlacaBronzeListForDropDown()                                                               ║
   ║    • Update(PlacaBronze placaBronze)                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem filtra Status == true e ordena por DescricaoPlaca.                                    ║
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
    // │ 🎯 CLASSE: PlacaBronzeRepository                                                               │
    // │ 📦 HERDA DE: Repository                                                           │
    // │ 🔌 IMPLEMENTA: IPlacaBronzeRepository                                                          │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelo cadastro de placas de bronze (identificação patrimonial).
    // Centraliza consultas para listagem e atualização de registros.
    
    public class PlacaBronzeRepository : Repository<PlacaBronze>, IPlacaBronzeRepository
    {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: PlacaBronzeRepository                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto de dados do EF Core.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public PlacaBronzeRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetPlacaBronzeListForDropDown                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.PlacaBronze, Where, OrderBy, Select                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de placas de bronze ativas para composição de dropdowns.
        // Ordena os registros pela descrição da placa.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção com placas de bronze ativas.
        public IEnumerable<SelectListItem> GetPlacaBronzeListForDropDown()
        {
            return _db
                .PlacaBronze.Where(e => e.Status == true) // Mudança aqui
                .OrderBy(o => o.DescricaoPlaca)
                .Select(i => new SelectListItem()
                {
                    Text = i.DescricaoPlaca,
                    Value = i.PlacaBronzeId.ToString(),
                });
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.PlacaBronze.FirstOrDefault, _db.Update, _db.SaveChanges     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de uma placa de bronze no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // placaBronze - Entidade contendo os dados atualizados da placa.
        
        
        // Param placaBronze: Entidade <see cref="PlacaBronze"/> com dados atualizados.
        public new void Update(PlacaBronze placaBronze)
        {
            var objFromDb = _db.PlacaBronze.FirstOrDefault(s =>
                s.PlacaBronzeId == placaBronze.PlacaBronzeId
            );

            _db.Update(placaBronze);
            _db.SaveChanges();
        }
    }
}
