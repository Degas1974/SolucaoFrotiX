/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViagensEconomildoRepository.cs                                                         ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para viagens do módulo Economildo.                                                  ║
   ║    Fornece listagens para UI e atualização de registros.                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViagensEconomildoRepository(FrotiXDbContext db)                                                ║
   ║    • GetViagensEconomildoListForDropDown()                                                         ║
   ║    • Update(ViagensEconomildo viagensEconomildo)                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem utiliza a data do registro como texto no dropdown.                                   ║
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
    /// │ 🎯 CLASSE: ViagensEconomildoRepository                                                        │
    /// │ 📦 HERDA DE: Repository<ViagensEconomildo>                                                    │
    /// │ 🔌 IMPLEMENTA: IViagensEconomildoRepository                                                   │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pelas viagens do módulo Economildo.
    /// Centraliza listagens para UI e atualização de registros.
    /// </summary>
    public class ViagensEconomildoRepository : Repository<ViagensEconomildo>, IViagensEconomildoRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViagensEconomildoRepository                                                   │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        /// │    ➡️ CHAMA       : base(db)                                                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Inicializar o repositório com o contexto do banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    db - Contexto do banco de dados da aplicação.
        /// </para>
        /// </summary>
        /// <param name="db">Instância de <see cref="FrotiXDbContext"/>.</param>
        public ViagensEconomildoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViagensEconomildoListForDropDown                                           │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViagensEconomildo, Select                                 │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista de viagens Economildo para composição de dropdowns.
        ///    Exibe a data do registro.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para viagens Economildo.</returns>
        public IEnumerable<SelectListItem> GetViagensEconomildoListForDropDown()
            {
            return _db.ViagensEconomildo
            .Select(i => new SelectListItem()
                {
                Text = i.Data.ToString(),
                Value = i.ViagemEconomildoId.ToString()
                }); ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViagensEconomildo.FirstOrDefault, _db.Update,               │
        /// │                     _db.SaveChanges                                                     │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualizar os dados de uma viagem Economildo no banco de dados.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    viagensEconomildo - Entidade contendo os dados atualizados.
        /// </para>
        /// </summary>
        /// <param name="viagensEconomildo">Entidade <see cref="ViagensEconomildo"/> com dados atualizados.</param>
        public new void Update(ViagensEconomildo viagensEconomildo)
            {
            var objFromDb = _db.ViagensEconomildo.FirstOrDefault(s => s.ViagemEconomildoId == viagensEconomildo.ViagemEconomildoId);

            _db.Update(viagensEconomildo);
            _db.SaveChanges();

            }


        }
    }
