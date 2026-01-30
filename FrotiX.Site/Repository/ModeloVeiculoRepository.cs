/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ModeloVeiculoRepository.cs                                                             ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade ModeloVeiculo.                                          ║
   ║    Gerencia cadastro de modelos de veículos (Uno, Gol, Onix, HB20, Voyage, etc).                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ModeloVeiculoRepository(FrotiXDbContext db)                                                   ║
   ║    • IEnumerable<SelectListItem> GetModeloVeiculoListForDropDown()                                ║
   ║    • void Update(ModeloVeiculo modeloVeiculo)                                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Trabalha em conjunto com MarcaVeiculoRepository (relacionamento Marca → Modelo).               ║
   ║    Retorna lista ordenada alfabeticamente por descrição do modelo.                                ║
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
    /// │ 🎯 CLASSE: ModeloVeiculoRepository                                                            │
    /// │ 📦 HERDA DE: Repository&lt;ModeloVeiculo&gt;                                                          │
    /// │ 🔌 IMPLEMENTA: IModeloVeiculoRepository                                                       │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório especializado para gerenciamento de modelos de veículos.
    /// Suporta operações CRUD para entidade ModeloVeiculo com ordenação por descrição.
    /// </summary>
    public class ModeloVeiculoRepository : Repository<ModeloVeiculo>, IModeloVeiculoRepository
        {
        private new readonly FrotiXDbContext _db;

        public ModeloVeiculoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetModeloVeiculoListForDropDown                                             │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers que utilizam dropdowns de modelos de veículos          │
        /// │    ➡️ CHAMA       : DbContext.ModeloVeiculo, Linq OrderBy/Select                        │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retorna lista de modelos de veículos formatada para uso em DropDown.
        ///    Ordenação alfabética por descrição do modelo para facilitar localização.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Lista com Text=DescricaoModelo e Value=ModeloId
        /// </para>
        /// </summary>
        /// <returns>Lista de SelectListItem com modelos ordenados alfabeticamente</returns>
        public IEnumerable<SelectListItem> GetModeloVeiculoListForDropDown()
            {
            return _db.ModeloVeiculo
            .OrderBy(o => o.DescricaoModelo)
            .Select(i => new SelectListItem()
                {
                Text = i.DescricaoModelo,
                Value = i.ModeloId.ToString()
                }); ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers de ModeloVeiculo, UnitOfWork                            │
        /// │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualiza dados de um modelo de veículo existente no banco de dados.
        ///    Localiza registro por ModeloId antes de persistir alterações.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    modeloVeiculo - Entidade ModeloVeiculo com dados atualizados
        /// </para>
        /// </summary>
        /// <param name="modeloVeiculo">Entidade ModeloVeiculo com dados a serem persistidos</param>
        public new void Update(ModeloVeiculo modeloVeiculo)
            {
            var objFromDb = _db.ModeloVeiculo.FirstOrDefault(s => s.ModeloId == modeloVeiculo.ModeloId);

            _db.Update(modeloVeiculo);
            _db.SaveChanges();

            }


        }
    }


