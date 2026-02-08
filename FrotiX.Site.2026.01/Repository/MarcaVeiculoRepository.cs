/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MarcaVeiculoRepository.cs                                                              ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade MarcaVeiculo.                                           ║
   ║    Gerencia cadastro de marcas de veículos (Fiat, Volkswagen, Chevrolet, Renault, etc).          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • MarcaVeiculoRepository(FrotiXDbContext db)                                                    ║
   ║    • IEnumerable<SelectListItem> GetMarcaVeiculoListForDropDown()                                 ║
   ║    • void Update(MarcaVeiculo marcaVeiculo)                                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    GetMarcaVeiculoListForDropDown filtra apenas marcas ativas (Status=true).                      ║
   ║    Retorna lista ordenada alfabeticamente por descrição da marca.                                 ║
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
    // │ 🎯 CLASSE: MarcaVeiculoRepository                                                             │
    // │ 📦 HERDA DE: Repository&lt;MarcaVeiculo&gt;                                                           │
    // │ 🔌 IMPLEMENTA: IMarcaVeiculoRepository                                                        │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório especializado para gerenciamento de marcas de veículos.
    // Suporta operações CRUD para entidade MarcaVeiculo com filtragem por status.
    
    public class MarcaVeiculoRepository : Repository<MarcaVeiculo>, IMarcaVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        public MarcaVeiculoRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetMarcaVeiculoListForDropDown                                              │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers que utilizam dropdowns de marcas de veículos           │
        // │    ➡️ CHAMA       : DbContext.MarcaVeiculo, Linq Where/OrderBy/Select                  │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de marcas de veículos ATIVAS formatada para uso em DropDown.
        // Filtra apenas marcas com Status=true, ordenadas alfabeticamente por descrição.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista de marcas ativas com Text=DescricaoMarca e Value=MarcaId
        
        
        // Returns: Lista de SelectListItem com marcas de veículos ativas ordenadas alfabeticamente
        public IEnumerable<SelectListItem> GetMarcaVeiculoListForDropDown()
        {
            return _db
                .MarcaVeiculo.Where(e => e.Status == true)
                .OrderBy(o => o.DescricaoMarca)
                .Select(i => new SelectListItem()
                {
                    Text = i.DescricaoMarca,
                    Value = i.MarcaId.ToString(),
                });
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de MarcaVeiculo, UnitOfWork                             │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza dados de uma marca de veículo existente no banco de dados.
        // Localiza registro por MarcaId antes de persistir alterações.
        
        
        
        // 📥 PARÂMETROS:
        // marcaVeiculo - Entidade MarcaVeiculo com dados atualizados
        
        
        // Param marcaVeiculo: Entidade MarcaVeiculo com dados a serem persistidos
        public new void Update(MarcaVeiculo marcaVeiculo)
        {
            var objFromDb = _db.MarcaVeiculo.FirstOrDefault(s => s.MarcaId == marcaVeiculo.MarcaId);

            _db.Update(marcaVeiculo);
            _db.SaveChanges();
        }
    }
}
