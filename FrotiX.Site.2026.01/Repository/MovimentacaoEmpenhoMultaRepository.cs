/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MovimentacaoEmpenhoMultaRepository.cs                                                  ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para movimentações financeiras em empenhos de multas de trânsito.                  ║
   ║    Registra débitos e créditos nas notas de empenho específicas de multas.                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • MovimentacaoEmpenhoMultaRepository(FrotiXDbContext db)                                        ║
   ║    • IEnumerable<SelectListItem> GetMovimentacaoEmpenhoMultaListForDropDown()                     ║
   ║    • void Update(MovimentacaoEmpenhoMulta movimentacaoempenhomulta)                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    GetMovimentacaoEmpenhoMultaListForDropDown exibe Data + Valor para fácil identificação.        ║
   ║    Essencial para controle financeiro de multas de trânsito.                                      ║
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
    // │ 🎯 CLASSE: MovimentacaoEmpenhoMultaRepository                                                 │
    // │ 📦 HERDA DE: Repository&lt;MovimentacaoEmpenhoMulta&gt;                                               │
    // │ 🔌 IMPLEMENTA: IMovimentacaoEmpenhoMultaRepository                                            │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório especializado para controle de movimentações financeiras em empenhos de multas.
    // Gerencia lançamentos contábeis relacionados ao pagamento de multas de trânsito.
    
    public class MovimentacaoEmpenhoMultaRepository : Repository<MovimentacaoEmpenhoMulta>, IMovimentacaoEmpenhoMultaRepository
        {
        private new readonly FrotiXDbContext _db;

        public MovimentacaoEmpenhoMultaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetMovimentacaoEmpenhoMultaListForDropDown                                  │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de gerenciamento financeiro de multas                  │
        // │    ➡️ CHAMA       : DbContext.MovimentacaoEmpenhoMulta, Linq OrderBy/Select             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de movimentações de empenho de multa para uso em DropDown.
        // Ordenação por data, exibindo data e valor concatenados para identificação.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista com Text="DataMovimentacao(Valor)" e Value=MovimentacaoId
        
        
        // Returns: Lista de SelectListItem com movimentações ordenadas por data
        public IEnumerable<SelectListItem> GetMovimentacaoEmpenhoMultaListForDropDown()
            {
            return _db.MovimentacaoEmpenhoMulta
            .OrderBy(o => o.DataMovimentacao)
            .Select(i => new SelectListItem()
                {
                Text = i.DataMovimentacao + "(" + i.Valor + ")",
                Value = i.MovimentacaoId.ToString()
                });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de MovimentacaoEmpenhoMulta, UnitOfWork                 │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza dados de uma movimentação de empenho de multa existente.
        // Permite ajustes em lançamentos contábeis após registro inicial.
        
        
        
        // 📥 PARÂMETROS:
        // movimentacaoempenhomulta - Entidade com dados atualizados da movimentação
        
        
        // Param movimentacaoempenhomulta: Entidade MovimentacaoEmpenhoMulta com dados a serem persistidos
        public new void Update(MovimentacaoEmpenhoMulta movimentacaoempenhomulta)
            {
            var objFromDb = _db.MovimentacaoEmpenhoMulta.FirstOrDefault(s => s.MovimentacaoId == movimentacaoempenhomulta.MovimentacaoId);

            _db.Update(movimentacaoempenhomulta);
            _db.SaveChanges();

            }
        }
    }


