/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MovimentacaoEmpenhoRepository.cs                                                       ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade MovimentacaoEmpenho.                                    ║
   ║    Gerencia movimentações financeiras (créditos e débitos) em notas de empenho gerais.            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • MovimentacaoEmpenhoRepository(FrotiXDbContext db)                                             ║
   ║    • IEnumerable<SelectListItem> GetMovimentacaoEmpenhoListForDropDown()                          ║
   ║    • void Update(MovimentacaoEmpenho movimentacaoempenho)                                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    GetMovimentacaoEmpenhoListForDropDown usa JOIN com Empenho para enriquecer dados.              ║
   ║    Essencial para controle orçamentário e financeiro da frota.                                    ║
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
    // │ 🎯 CLASSE: MovimentacaoEmpenhoRepository                                                      │
    // │ 📦 HERDA DE: Repository&lt;MovimentacaoEmpenho&gt;                                                    │
    // │ 🔌 IMPLEMENTA: IMovimentacaoEmpenhoRepository                                                 │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório especializado para gerenciamento de movimentações de empenho.
    // Controla lançamentos contábeis em notas de empenho para compras e serviços da frota.
    
    public class MovimentacaoEmpenhoRepository : Repository<MovimentacaoEmpenho>, IMovimentacaoEmpenhoRepository
        {
        private new readonly FrotiXDbContext _db;

        public MovimentacaoEmpenhoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetMovimentacaoEmpenhoListForDropDown                                       │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de gerenciamento financeiro e orçamentário             │
        // │    ➡️ CHAMA       : DbContext.MovimentacaoEmpenho, Join com Empenho, Linq              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de movimentações de empenho para uso em DropDown.
        // Utiliza JOIN com tabela Empenho para enriquecimento de dados.
        // Ordenação por data, exibindo data e valor concatenados.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista com Text="DataMovimentacao(Valor)" e Value=MovimentacaoId
        
        
        // Returns: Lista de SelectListItem com movimentações de empenho ordenadas por data
        public IEnumerable<SelectListItem> GetMovimentacaoEmpenhoListForDropDown()
            {
            return _db.MovimentacaoEmpenho
            .Join(_db.Empenho, movimentacaoempenho => movimentacaoempenho.EmpenhoId, empenho => empenho.EmpenhoId, (movimentacaoempenho, empenho) => new { movimentacaoempenho, empenho })
            .OrderBy(o => o.movimentacaoempenho.DataMovimentacao)
            .Select(i => new SelectListItem()
                {
                Text = i.movimentacaoempenho.DataMovimentacao + "(" + i.movimentacaoempenho.Valor + ")",
                Value = i.movimentacaoempenho.MovimentacaoId.ToString()
                });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de MovimentacaoEmpenho, UnitOfWork                      │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza dados de uma movimentação de empenho existente.
        // Permite ajustes em lançamentos contábeis após registro inicial.
        
        
        
        // 📥 PARÂMETROS:
        // movimentacaoempenho - Entidade com dados atualizados da movimentação
        
        
        // Param movimentacaoempenho: Entidade MovimentacaoEmpenho com dados a serem persistidos
        public new void Update(MovimentacaoEmpenho movimentacaoempenho)
            {
            var objFromDb = _db.MovimentacaoEmpenho.FirstOrDefault(s => s.MovimentacaoId == movimentacaoempenho.MovimentacaoId);

            _db.Update(movimentacaoempenho);
            _db.SaveChanges();

            }
        }
    }


