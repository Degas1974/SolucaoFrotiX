/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MultaRepository.cs                                                                     ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade Multa.                                                  ║
   ║    Gerencia registro e controle de multas de trânsito aplicadas aos veículos da frota.            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • MultaRepository(FrotiXDbContext db)                                                           ║
   ║    • IEnumerable<SelectListItem> GetMultaListForDropDown()                                        ║
   ║    • void Update(Multa multa)                                                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    GetMultaListForDropDown retorna lista ordenada por número da infração.                         ║
   ║    Essencial para controle financeiro e identificação de motoristas infratores.                   ║
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
    // │ 🎯 CLASSE: MultaRepository                                                                    │
    // │ 📦 HERDA DE: Repository&lt;Multa&gt;                                                                  │
    // │ 🔌 IMPLEMENTA: IMultaRepository                                                               │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório especializado para gerenciamento de multas de trânsito.
    // Fornece acesso a dados e operações específicas para controle de infrações da frota.
    
    public class MultaRepository : Repository<Multa>, IMultaRepository
        {
        private new readonly FrotiXDbContext _db;

        public MultaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetMultaListForDropDown                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers que utilizam dropdowns de multas                        │
        // │    ➡️ CHAMA       : DbContext.Multa, Linq OrderBy/Select                                │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de multas formatada para uso em DropDown/SelectList.
        // Ordenação por número da infração para facilitar localização.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista com Text=NumInfracao e Value=MultaId
        
        
        // Returns: Lista de SelectListItem ordenada por número da infração
        public IEnumerable<SelectListItem> GetMultaListForDropDown()
            {
            return _db.Multa
                .OrderBy(o => o.NumInfracao)
                .Select(i => new SelectListItem()
                    {
                    Text = i.NumInfracao,
                    Value = i.MultaId.ToString()
                    });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de Multa, UnitOfWork                                    │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza dados de uma multa existente no banco de dados.
        // Permite alterações em status de pagamento, recursos e demais informações.
        
        
        
        // 📥 PARÂMETROS:
        // multa - Entidade Multa com dados atualizados
        
        
        // Param multa: Entidade Multa com dados a serem persistidos
        public new void Update(Multa multa)
            {
            var objFromDb = _db.Multa.FirstOrDefault(s => s.MultaId == multa.MultaId);

            _db.Update(multa);
            _db.SaveChanges();

            }


        }
    }


