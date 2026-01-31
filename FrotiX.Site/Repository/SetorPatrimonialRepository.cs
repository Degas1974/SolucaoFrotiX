/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: SetorPatrimonialRepository.cs                                                          ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para setores patrimoniais (controle de bens móveis e imóveis).                      ║
   ║    Centraliza listagens para UI e atualização da entidade SetorPatrimonial.                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • SetorPatrimonialRepository(FrotiXDbContext db)                                                 ║
   ║    • GetSetorListForDropDown()                                                                     ║
   ║    • Update(SetorPatrimonial setor)                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem é ordenada por NomeSetor para apresentação em dropdowns.                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Models.Cadastros;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
    {
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: SetorPatrimonialRepository                                                         │
    // │ 📦 HERDA DE: Repository                                                     │
    // │ 🔌 IMPLEMENTA: ISetorPatrimonialRepository                                                    │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelos setores patrimoniais.
    // Fornece consultas para dropdowns e atualização de registros.
    
    public class SetorPatrimonialRepository : Repository<SetorPatrimonial>, ISetorPatrimonialRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: SetorPatrimonialRepository                                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public SetorPatrimonialRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetSetorListForDropDown                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.SetorPatrimonial, OrderBy, Select                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de setores patrimoniais para composição de dropdowns.
        // Ordena os registros pelo nome do setor.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para setores patrimoniais.
        public IEnumerable<SelectListItem> GetSetorListForDropDown()
            {
            return _db.SetorPatrimonial
            .OrderBy(o => o.NomeSetor)
            .Select(i => new SelectListItem()
                {
                Text = i.NomeSetor,
                Value = i.SetorId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.SetorPatrimonial.FirstOrDefault, _db.Update,                │
        // │                     _db.SaveChanges                                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de um setor patrimonial no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // setor - Entidade contendo os dados atualizados.
        
        
        // Param setor: Entidade <see cref="SetorPatrimonial"/> com dados atualizados.
        public new void Update(SetorPatrimonial setor)
            {
            var objFromDb = _db.SetorPatrimonial.FirstOrDefault(s => s.SetorId == setor.SetorId);

            _db.Update(setor);
            _db.SaveChanges();

            }


        }
    }
