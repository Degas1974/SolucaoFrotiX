/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RepactuacaoAtaRepository.cs                                                            ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para repactuações de atas de registro de preços.                                     ║
   ║    Centraliza listagens para UI e atualizações da entidade RepactuacaoAta.                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RepactuacaoAtaRepository(FrotiXDbContext db)                                                    ║
   ║    • GetRepactuacaoAtaListForDropDown()                                                             ║
   ║    • Update(RepactuacaoAta repactuacaoitemveiculoata)                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem é ordenada por Descricao para apresentação em dropdowns.                              ║
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
    // │ 🎯 CLASSE: RepactuacaoAtaRepository                                                            │
    // │ 📦 HERDA DE: Repository                                                        │
    // │ 🔌 IMPLEMENTA: IRepactuacaoAtaRepository                                                       │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas repactuações de atas de registro de preços.
    // Provê consultas para dropdowns e atualização de registros.
    
    public class RepactuacaoAtaRepository : Repository<RepactuacaoAta>, IRepactuacaoAtaRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RepactuacaoAtaRepository                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RepactuacaoAtaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetRepactuacaoAtaListForDropDown                                               │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.RepactuacaoAta, OrderBy, Select                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de repactuações de atas para composição de dropdowns.
        // Ordena os registros pela descrição.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para repactuações de atas.
        public IEnumerable<SelectListItem> GetRepactuacaoAtaListForDropDown()
            {
            return _db.RepactuacaoAta
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Descricao,
                    Value = i.RepactuacaoAtaId.ToString()
                    });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.RepactuacaoAta.FirstOrDefault, _db.Update, _db.SaveChanges   │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de uma repactuação de ata no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // repactuacaoitemveiculoata - Entidade contendo os dados atualizados.
        
        
        // Param repactuacaoitemveiculoata: Entidade <see cref="RepactuacaoAta"/> com dados atualizados.
        public new void Update(RepactuacaoAta repactuacaoitemveiculoata)
            {
            var objFromDb = _db.RepactuacaoAta.FirstOrDefault(s => s.RepactuacaoAtaId == repactuacaoitemveiculoata.RepactuacaoAtaId);

            _db.Update(repactuacaoitemveiculoata);
            _db.SaveChanges();

            }


        }
    }
