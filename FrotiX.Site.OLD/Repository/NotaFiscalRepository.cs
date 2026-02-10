/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: NotaFiscalRepository.cs                                                                ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade NotaFiscal.                                             ║
   ║    Gerencia notas fiscais de compras de peças, serviços, combustível e outros insumos da frota.   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • NotaFiscalRepository(FrotiXDbContext db)                                                      ║
   ║    • IEnumerable<SelectListItem> GetNotaFiscalListForDropDown()                                   ║
   ║    • void Update(NotaFiscal notaFiscal)                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    GetNotaFiscalListForDropDown retorna lista ordenada por número da nota fiscal.                 ║
   ║    Essencial para controle financeiro e vinculação com empenhos/abastecimentos/manutenções.       ║
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
    // │ 🎯 CLASSE: NotaFiscalRepository                                                               │
    // │ 📦 HERDA DE: Repository&lt;NotaFiscal&gt;                                                             │
    // │ 🔌 IMPLEMENTA: INotaFiscalRepository                                                          │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório especializado para gerenciamento de notas fiscais.
    // Fornece acesso a dados e operações específicas para controle fiscal da frota.
    
    public class NotaFiscalRepository : Repository<NotaFiscal>, INotaFiscalRepository
        {
        private new readonly FrotiXDbContext _db;

        public NotaFiscalRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetNotaFiscalListForDropDown                                                │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers que utilizam dropdowns de notas fiscais                 │
        // │    ➡️ CHAMA       : DbContext.NotaFiscal, Linq OrderBy/Select                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de notas fiscais formatada para uso em DropDown/SelectList.
        // Ordenação por número da nota fiscal para facilitar localização.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista com Text=NumeroNF e Value=NotaFiscalId
        
        
        // Returns: Lista de SelectListItem ordenada por número da nota fiscal
        public IEnumerable<SelectListItem> GetNotaFiscalListForDropDown()
            {
            return _db.NotaFiscal
            .OrderBy(o => o.NumeroNF)
            .Select(i => new SelectListItem()
                {
                Text = i.NumeroNF.ToString(),
                Value = i.NotaFiscalId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de NotaFiscal, UnitOfWork                               │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza dados de uma nota fiscal existente no banco de dados.
        // Permite correções em informações fiscais após registro inicial.
        
        
        
        // 📥 PARÂMETROS:
        // notaFiscal - Entidade NotaFiscal com dados atualizados
        
        
        // Param notaFiscal: Entidade NotaFiscal com dados a serem persistidos
        public new void Update(NotaFiscal notaFiscal)
            {
            var objFromDb = _db.NotaFiscal.FirstOrDefault(s => s.NotaFiscalId == notaFiscal.NotaFiscalId);

            _db.Update(notaFiscal);
            _db.SaveChanges();

            }
        }
    }


