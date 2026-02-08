/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: OperadorRepository.cs                                                                  ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade Operador.                                               ║
   ║    Gerencia operadores de frota (auxiliares que acompanham motoristas em viagens).                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • OperadorRepository(FrotiXDbContext db)                                                        ║
   ║    • IEnumerable<SelectListItem> GetOperadorListForDropDown()                                     ║
   ║    • void Update(Operador operador)                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    GetOperadorListForDropDown retorna lista ordenada alfabeticamente por Nome.                    ║
   ║    Operadores são auxiliares que acompanham motoristas, diferente dos motoristas principais.     ║
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
    // │ 🎯 CLASSE: OperadorRepository                                                                 │
    // │ 📦 HERDA DE: Repository&lt;Operador&gt;                                                               │
    // │ 🔌 IMPLEMENTA: IOperadorRepository                                                            │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório especializado para gerenciamento de operadores de frota.
    // Operadores são auxiliares que acompanham motoristas em viagens e deslocamentos.
    
    public class OperadorRepository : Repository<Operador>, IOperadorRepository
        {
        private new readonly FrotiXDbContext _db;

        public OperadorRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetOperadorListForDropDown                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers que utilizam dropdowns de operadores                   │
        // │    ➡️ CHAMA       : DbContext.Operador, Linq OrderBy/Select                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de operadores formatada para uso em DropDown/SelectList.
        // Ordenação alfabética por nome para facilitar localização.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista com Text=Nome e Value=OperadorId
        
        
        // Returns: Lista de SelectListItem ordenada alfabeticamente por nome do operador
        public IEnumerable<SelectListItem> GetOperadorListForDropDown()
            {
            return _db.Operador
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.OperadorId.ToString()
                }); ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de Operador, UnitOfWork                                 │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza dados de um operador existente no banco de dados.
        // Permite alterações em informações cadastrais e vínculos do operador.
        
        
        
        // 📥 PARÂMETROS:
        // operador - Entidade Operador com dados atualizados
        
        
        // Param operador: Entidade Operador com dados a serem persistidos
        public new void Update(Operador operador)
            {
            var objFromDb = _db.Operador.FirstOrDefault(s => s.OperadorId == operador.OperadorId);

            _db.Update(operador);
            _db.SaveChanges();

            }


        }
    }


