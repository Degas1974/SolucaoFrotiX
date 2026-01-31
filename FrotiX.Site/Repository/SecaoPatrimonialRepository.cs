/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: SecaoPatrimonialRepository.cs                                                          ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para seções patrimoniais (subdivisões de setores).                                  ║
   ║    Centraliza listagens para UI e atualização da entidade SecaoPatrimonial.                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • SecaoPatrimonialRepository(FrotiXDbContext db)                                                 ║
   ║    • GetSecaoListForDropDown()                                                                     ║
   ║    • Update(SecaoPatrimonial secao)                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem exibe NomeSecao e SetorId no formato "Nome/Setor".                                    ║
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
    // │ 🎯 CLASSE: SecaoPatrimonialRepository                                                         │
    // │ 📦 HERDA DE: Repository                                                     │
    // │ 🔌 IMPLEMENTA: ISecaoPatrimonialRepository                                                    │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas seções patrimoniais.
    // Fornece consultas para dropdowns e atualização de registros.
    
    public class SecaoPatrimonialRepository : Repository<SecaoPatrimonial>, ISecaoPatrimonialRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: SecaoPatrimonialRepository                                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public SecaoPatrimonialRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetSecaoListForDropDown                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.SecaoPatrimonial, OrderBy, Select                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de seções patrimoniais para composição de dropdowns.
        // Ordena por nome da seção e exibe "NomeSecao/SetorId".
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para seções patrimoniais.
        public IEnumerable<SelectListItem> GetSecaoListForDropDown()
            {
            return _db.SecaoPatrimonial
            .OrderBy(o => o.NomeSecao)
            .Select(i => new SelectListItem()
                {
                Text = i.NomeSecao + "/" + i.SetorId.ToString(),
                Value = i.SecaoId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.SecaoPatrimonial.FirstOrDefault, _db.Update,                │
        // │                     _db.SaveChanges                                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de uma seção patrimonial no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // secao - Entidade contendo os dados atualizados.
        
        
        // Param secao: Entidade <see cref="SecaoPatrimonial"/> com dados atualizados.
        public new void Update(SecaoPatrimonial secao)
            {
            var objFromDb = _db.SecaoPatrimonial.FirstOrDefault(s => s.SecaoId == secao.SecaoId);

            _db.Update(secao);
            _db.SaveChanges();

            }


        }
    }
