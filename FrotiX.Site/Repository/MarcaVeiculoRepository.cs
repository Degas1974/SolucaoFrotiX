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
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: MarcaVeiculoRepository                                                             │
    /// │ 📦 HERDA DE: Repository&lt;MarcaVeiculo&gt;                                                           │
    /// │ 🔌 IMPLEMENTA: IMarcaVeiculoRepository                                                        │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório especializado para gerenciamento de marcas de veículos.
    /// Suporta operações CRUD para entidade MarcaVeiculo com filtragem por status.
    /// </summary>
    public class MarcaVeiculoRepository : Repository<MarcaVeiculo>, IMarcaVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        public MarcaVeiculoRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetMarcaVeiculoListForDropDown                                              │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers que utilizam dropdowns de marcas de veículos           │
        /// │    ➡️ CHAMA       : DbContext.MarcaVeiculo, Linq Where/OrderBy/Select                  │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retorna lista de marcas de veículos ATIVAS formatada para uso em DropDown.
        ///    Filtra apenas marcas com Status=true, ordenadas alfabeticamente por descrição.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Lista de marcas ativas com Text=DescricaoMarca e Value=MarcaId
        /// </para>
        /// </summary>
        /// <returns>Lista de SelectListItem com marcas de veículos ativas ordenadas alfabeticamente</returns>
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers de MarcaVeiculo, UnitOfWork                             │
        /// │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualiza dados de uma marca de veículo existente no banco de dados.
        ///    Localiza registro por MarcaId antes de persistir alterações.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    marcaVeiculo - Entidade MarcaVeiculo com dados atualizados
        /// </para>
        /// </summary>
        /// <param name="marcaVeiculo">Entidade MarcaVeiculo com dados a serem persistidos</param>
        public new void Update(MarcaVeiculo marcaVeiculo)
        {
            var objFromDb = _db.MarcaVeiculo.FirstOrDefault(s => s.MarcaId == marcaVeiculo.MarcaId);

            _db.Update(marcaVeiculo);
            _db.SaveChanges();
        }
    }
}
