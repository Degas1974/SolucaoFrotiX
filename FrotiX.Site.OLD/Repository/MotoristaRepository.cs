/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MotoristaRepository.cs                                                                 ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade Motorista.                                              ║
   ║    Gerencia cadastro de motoristas da frota, incluindo motoristas próprios e terceirizados.       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • MotoristaRepository(FrotiXDbContext db)                                                       ║
   ║    • IEnumerable<SelectListItem> GetMotoristaListForDropDown()                                    ║
   ║    • void Update(Motorista motorista)                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Implementa IMotoristaRepository. Herda de Repository<Motorista> para operações CRUD básicas.   ║
   ║    GetMotoristaListForDropDown retorna lista ordenada alfabeticamente por nome.                   ║
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
    // │ 🎯 CLASSE: MotoristaRepository                                                                │
    // │ 📦 HERDA DE: Repository&lt;Motorista&gt;                                                              │
    // │ 🔌 IMPLEMENTA: IMotoristaRepository                                                           │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório especializado para gerenciamento de motoristas da frota.
    // Fornece acesso a dados e operações específicas para entidade Motorista.
    
    public class MotoristaRepository : Repository<Motorista>, IMotoristaRepository
        {
        private new readonly FrotiXDbContext _db;

        public MotoristaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetMotoristaListForDropDown                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers que utilizam dropdowns de motoristas                    │
        // │    ➡️ CHAMA       : DbContext.Motorista, Linq OrderBy/Select                            │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de motoristas formatada para uso em DropDown/SelectList.
        // Ordenação alfabética por nome para facilitar seleção pelo usuário.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista de motoristas com Text=Nome e Value=MotoristaId
        
        
        // Returns: Lista de SelectListItem ordenada por nome do motorista
        public IEnumerable<SelectListItem> GetMotoristaListForDropDown()
            {
            return _db.Motorista
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.MotoristaId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de Motorista, UnitOfWork                                │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza dados de um motorista existente no banco de dados.
        // Sobrescreve método herdado para permitir validações específicas.
        
        
        
        // 📥 PARÂMETROS:
        // motorista - Entidade Motorista com dados atualizados
        
        
        // Param motorista: Entidade Motorista com dados a serem persistidos
        public new void Update(Motorista motorista)
            {
            var objFromDb = _db.Motorista.FirstOrDefault(s => s.MotoristaId == motorista.MotoristaId);

            _db.Update(motorista);
            _db.SaveChanges();

            }


        }
    }


