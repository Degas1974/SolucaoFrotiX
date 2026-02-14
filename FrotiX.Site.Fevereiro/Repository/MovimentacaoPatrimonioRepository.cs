/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MovimentacaoPatrimonioRepository.cs                                                    ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para movimentações patrimoniais de veículos próprios.                              ║
   ║    Registra transferências entre setores, baixas e alterações de situação patrimonial.            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • MovimentacaoPatrimonioRepository(FrotiXDbContext db)                                          ║
   ║    • IEnumerable<SelectListItem> GetMovimentacaoPatrimonioListForDropDown()                       ║
   ║    • void Update(MovimentacaoPatrimonio movimentacaoPatrimonio)                                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Controla histórico de movimentações patrimoniais para veículos próprios (não terceirizados).   ║
   ║    Essencial para controle de transferências e baixas de patrimônio.                              ║
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
    // │ 🎯 CLASSE: MovimentacaoPatrimonioRepository                                                   │
    // │ 📦 HERDA DE: Repository&lt;MovimentacaoPatrimonio&gt;                                                 │
    // │ 🔌 IMPLEMENTA: IMovimentacaoPatrimonioRepository                                              │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório especializado para gerenciamento de movimentações patrimoniais.
    // Controla transferências, baixas e mudanças de situação de veículos próprios.
    
    public class MovimentacaoPatrimonioRepository : Repository<MovimentacaoPatrimonio>, IMovimentacaoPatrimonioRepository
        {
        private new readonly FrotiXDbContext _db;

        public MovimentacaoPatrimonioRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetMovimentacaoPatrimonioListForDropDown                                    │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de gerenciamento patrimonial                            │
        // │    ➡️ CHAMA       : DbContext.MovimentacaoPatrimonio, Linq OrderBy/Select               │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de movimentações patrimoniais para uso em DropDown.
        // Ordenação por PatrimonioId, exibindo data da movimentação.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista com Text=DataMovimentacao e Value=MovimentacaoPatrimonioId
        
        
        // Returns: Lista de SelectListItem com movimentações ordenadas por patrimônio
        public IEnumerable<SelectListItem> GetMovimentacaoPatrimonioListForDropDown()
            {
            return _db.MovimentacaoPatrimonio
            .OrderBy(o => o.PatrimonioId)
            .Select(i => new SelectListItem()
                {
                Text = i.DataMovimentacao.ToString(),
                Value = i.MovimentacaoPatrimonioId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de MovimentacaoPatrimonio, UnitOfWork                   │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza dados de uma movimentação patrimonial existente.
        // Permite correções em registros de transferência ou baixa de patrimônio.
        
        
        
        // 📥 PARÂMETROS:
        // movimentacaoPatrimonio - Entidade com dados atualizados da movimentação
        
        
        // Param movimentacaoPatrimonio: Entidade MovimentacaoPatrimonio com dados a serem persistidos
        public new void Update(MovimentacaoPatrimonio movimentacaoPatrimonio)
            {
            var objFromDb = _db.MovimentacaoPatrimonio.FirstOrDefault(s => s.MovimentacaoPatrimonioId == movimentacaoPatrimonio.MovimentacaoPatrimonioId);

            _db.Update(movimentacaoPatrimonio);
            _db.SaveChanges();

            }


        }
    }


