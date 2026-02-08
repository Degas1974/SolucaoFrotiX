/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: PatrimonioRepository.cs                                                                ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade Patrimonio.                                             ║
   ║    Gerencia ativos patrimoniais da organização (equipamentos, móveis, bens permanentes).          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • PatrimonioRepository(FrotiXDbContext db)                                                      ║
   ║    • IEnumerable<SelectListItem> GetPatrimonioListForDropDown()                                   ║
   ║    • void Update(Patrimonio patrimonio)                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    GetPatrimonioListForDropDown ordenado por NumeroSerie, exibe NPR (Número de Patrimônio).       ║
   ║    Essencial para controle patrimonial e rastreamento de bens da organização.                    ║
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
    // │ 🎯 CLASSE: PatrimonioRepository                                                               │
    // │ 📦 HERDA DE: Repository&lt;Patrimonio&gt;                                                             │
    // │ 🔌 IMPLEMENTA: IPatrimonioRepository                                                          │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório especializado para gerenciamento de patrimônio.
    // Controla ativos permanentes da organização (equipamentos, móveis, bens patrimoniais).
    
    public class PatrimonioRepository : Repository<Patrimonio>, IPatrimonioRepository
        {
        private new readonly FrotiXDbContext _db;

        public PatrimonioRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetPatrimonioListForDropDown                                                │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de gerenciamento patrimonial                           │
        // │    ➡️ CHAMA       : DbContext.Patrimonio, Linq OrderBy/Select                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de patrimônios para uso em DropDown/SelectList.
        // Exibe NPR (Número de Patrimônio) para identificação.
        // Ordenação por número de série para facilitar localização.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista com Text=NPR e Value=PatrimonioId
        
        
        // Returns: Lista de SelectListItem ordenada por número de série do patrimônio
        public IEnumerable<SelectListItem> GetPatrimonioListForDropDown()
            {
            return _db.Patrimonio
            .OrderBy(o => o.NumeroSerie)
            .Select(i => new SelectListItem()
                {
                Text = i.NPR,
                Value = i.PatrimonioId.ToString()
                });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de Patrimonio, UnitOfWork                               │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza dados de um patrimônio existente no banco de dados.
        // Permite alterações em informações cadastrais, localização e status do bem.
        
        
        
        // 📥 PARÂMETROS:
        // patrimonio - Entidade Patrimonio com dados atualizados
        
        
        // Param patrimonio: Entidade Patrimonio com dados a serem persistidos
        public new void Update(Patrimonio patrimonio)
            {
            var objFromDb = _db.Patrimonio.FirstOrDefault(s => s.PatrimonioId == patrimonio.PatrimonioId);

            _db.Update(patrimonio);
            _db.SaveChanges();

            }


        }
    }


