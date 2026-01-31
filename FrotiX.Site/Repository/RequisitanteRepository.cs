/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RequisitanteRepository.cs                                                              ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para requisitantes de viagens e seus pontos de contato.                             ║
   ║    Fornece listagens para UI e atualização de cadastros.                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RequisitanteRepository(FrotiXDbContext db)                                                     ║
   ║    • GetRequisitanteListForDropDown()                                                              ║
   ║    • Update(Requisitante requisitante)                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem filtra Status == true e concatena Nome com Ponto.                                    ║
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
    // │ 🎯 CLASSE: RequisitanteRepository                                                             │
    // │ 📦 HERDA DE: Repository                                                         │
    // │ 🔌 IMPLEMENTA: IRequisitanteRepository                                                        │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelos requisitantes de viagens.
    // Centraliza consultas para dropdowns e atualização de registros.
    
    public class RequisitanteRepository : Repository<Requisitante>, IRequisitanteRepository
    {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RequisitanteRepository                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto de dados do EF Core.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RequisitanteRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetRequisitanteListForDropDown                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.Requisitante, Where, OrderBy, Select                       │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de requisitantes ativos para composição de dropdowns.
        // Exibe Nome e Ponto no formato "Nome(Ponto)".
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para requisitantes ativos.
        public IEnumerable<SelectListItem> GetRequisitanteListForDropDown()
        {
            return _db
                .Requisitante.Where(s => s.Status == true)
                .OrderBy(o => o.Nome)
                .Select(i => new SelectListItem()
                {
                    Text = i.Nome + "(" + i.Ponto + ")",
                    Value = i.RequisitanteId.ToString(),
                });
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.Requisitante.FirstOrDefault, _db.Update, _db.SaveChanges    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de um requisitante no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // requisitante - Entidade contendo os dados atualizados.
        
        
        // Param requisitante: Entidade <see cref="Requisitante"/> com dados atualizados.
        public new void Update(Requisitante requisitante)
        {
            var objFromDb = _db.Requisitante.FirstOrDefault(s =>
                s.RequisitanteId == requisitante.RequisitanteId
            );

            _db.Update(requisitante);
            _db.SaveChanges();
        }
    }
}
