/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: TipoMultaRepository.cs                                                                 ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para tipos de multas de trânsito (artigos, descrições e valores).                   ║
   ║    Centraliza listagens para UI e atualização da entidade TipoMulta.                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • TipoMultaRepository(FrotiXDbContext db)                                                        ║
   ║    • GetTipoMultaListForDropDown()                                                                 ║
   ║    • Update(TipoMulta tipomulta)                                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem exibe "Artigo - Descricao" e ordena pelo artigo.                                     ║
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
    // │ 🎯 CLASSE: TipoMultaRepository                                                                │
    // │ 📦 HERDA DE: Repository                                                            │
    // │ 🔌 IMPLEMENTA: ITipoMultaRepository                                                           │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelos tipos de multas de trânsito.
    // Fornece consultas para dropdowns e atualização de registros.
    
    public class TipoMultaRepository : Repository<TipoMulta>, ITipoMultaRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: TipoMultaRepository                                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public TipoMultaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetTipoMultaListForDropDown                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.TipoMulta, OrderBy, Select                                 │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de tipos de multa para composição de dropdowns.
        // Exibe "Artigo - Descricao" e ordena pelo artigo.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para tipos de multa.
        public IEnumerable<SelectListItem> GetTipoMultaListForDropDown()
            {
            return _db.TipoMulta
                .OrderBy(o => o.Artigo)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Artigo + " - " + i.Descricao,
                    Value = i.TipoMultaId.ToString()
                    });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.TipoMulta.FirstOrDefault, _db.Update, _db.SaveChanges       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de um tipo de multa no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // tipomulta - Entidade contendo os dados atualizados.
        
        
        // Param tipomulta: Entidade <see cref="TipoMulta"/> com dados atualizados.
        public new void Update(TipoMulta tipomulta)
            {
            var objFromDb = _db.TipoMulta.FirstOrDefault(s => s.TipoMultaId == tipomulta.TipoMultaId);

            _db.Update(tipomulta);
            _db.SaveChanges();

            }


        }
    }
