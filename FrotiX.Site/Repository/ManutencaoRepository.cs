/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ManutencaoRepository.cs                                                                ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade Manutencao.                                             ║
   ║    Gerencia ordens de serviço e registros de manutenções preventivas/corretivas da frota.         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ManutencaoRepository(FrotiXDbContext db)                                                      ║
   ║    • IEnumerable<SelectListItem> GetManutencaoListForDropDown()                                   ║
   ║    • void Update(Manutencao manutencao)                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    GetManutencaoListForDropDown retorna lista ordenada por ResumoOS (resumo da ordem de serviço). ║
   ║    Essencial para controle de manutenções preventivas e corretivas dos veículos.                  ║
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
    /// │ 🎯 CLASSE: ManutencaoRepository                                                               │
    /// │ 📦 HERDA DE: Repository&lt;Manutencao&gt;                                                             │
    /// │ 🔌 IMPLEMENTA: IManutencaoRepository                                                          │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório especializado para gerenciamento de manutenções de veículos.
    /// Controla ordens de serviço, manutenções preventivas e corretivas da frota.
    /// </summary>
    public class ManutencaoRepository : Repository<Manutencao>, IManutencaoRepository
        {
        private new readonly FrotiXDbContext _db;

        public ManutencaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetManutencaoListForDropDown                                                │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers que utilizam dropdowns de manutenções                  │
        /// │    ➡️ CHAMA       : DbContext.Manutencao, Linq OrderBy/Select                          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retorna lista de manutenções formatada para uso em DropDown/SelectList.
        ///    Ordenação por ResumoOS (resumo da ordem de serviço) para facilitar seleção.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Lista com Text=ResumoOS e Value=ManutencaoId
        /// </para>
        /// </summary>
        /// <returns>Lista de SelectListItem ordenada por resumo da OS</returns>
        public IEnumerable<SelectListItem> GetManutencaoListForDropDown()
            {
            return _db.Manutencao
            .OrderBy(o => o.ResumoOS)
            .Select(i => new SelectListItem()
                {
                Text = i.ResumoOS,
                Value = i.ManutencaoId.ToString()
                }); ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers de Manutencao, UnitOfWork                              │
        /// │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualiza dados de uma manutenção existente no banco de dados.
        ///    Permite modificar informações da OS após sua criação inicial.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    manutencao - Entidade Manutencao com dados atualizados da ordem de serviço
        /// </para>
        /// </summary>
        /// <param name="manutencao">Entidade Manutencao com dados a serem persistidos</param>
        public new void Update(Manutencao manutencao)
            {
            var objFromDb = _db.Manutencao.FirstOrDefault(s => s.ManutencaoId == manutencao.ManutencaoId);

            _db.Update(manutencao);
            _db.SaveChanges();

            }


        }
    }


