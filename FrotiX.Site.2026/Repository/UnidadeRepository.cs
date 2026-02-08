/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: UnidadeRepository.cs                                                                   ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para unidades organizacionais (órgãos, departamentos, setores).                     ║
   ║    Fornece listagens para UI e atualização da entidade Unidade.                                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • UnidadeRepository(FrotiXDbContext db)                                                         ║
   ║    • GetUnidadeListForDropDown()                                                                   ║
   ║    • Update(Unidade unidade)                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem filtra Status == true e ordena por "Sigla - Descricao".                               ║
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
    // │ 🎯 CLASSE: UnidadeRepository                                                                  │
    // │ 📦 HERDA DE: Repository                                                              │
    // │ 🔌 IMPLEMENTA: IUnidadeRepository                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas unidades organizacionais.
    // Centraliza listagens para dropdowns e atualização de registros.
    
    public class UnidadeRepository : Repository<Unidade>, IUnidadeRepository
    {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: UnidadeRepository                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto de dados do EF Core.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public UnidadeRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetUnidadeListForDropDown                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.Unidade, Where, OrderBy, Select                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de unidades ativas para composição de dropdowns.
        // Ordena por "Sigla - Descricao".
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para unidades ativas.
        public IEnumerable<SelectListItem> GetUnidadeListForDropDown()
        {
            return _db
                .Unidade.Where(e => e.Status == true)
                .OrderBy(o => o.Sigla + " - " + o.Descricao)
                .Select(i => new SelectListItem()
                {
                    Text = i.Sigla + " - " + i.Descricao,
                    Value = i.UnidadeId.ToString(),
                });
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.Unidade.FirstOrDefault, _db.Update, _db.SaveChanges         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de uma unidade no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // unidade - Entidade contendo os dados atualizados.
        
        
        // Param unidade: Entidade <see cref="Unidade"/> com dados atualizados.
        public new void Update(Unidade unidade)
        {
            var objFromDb = _db.Unidade.FirstOrDefault(s => s.UnidadeId == unidade.UnidadeId);

            _db.Update(unidade);
            _db.SaveChanges();
        }
    }
}
