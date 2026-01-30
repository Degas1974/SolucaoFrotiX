/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LavadoresLavagemRepository.cs                                                          ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para vínculo N:N entre lavadores e lavagens de veículos.                           ║
   ║    Registra participação de cada lavador nos serviços de lavagem realizados.                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • LavadoresLavagemRepository(FrotiXDbContext db)                                                ║
   ║    • IEnumerable<SelectListItem> GetLavadoresLavagemListForDropDown()                             ║
   ║    • void Update(LavadoresLavagem lavadoresLavagem)                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Tabela associativa para controle de equipe de lavagem.                                         ║
   ║    Permite rastreamento de quem executou cada serviço de limpeza de veículos.                     ║
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
    /// │ 🎯 CLASSE: LavadoresLavagemRepository                                                         │
    /// │ 📦 HERDA DE: Repository&lt;LavadoresLavagem&gt;                                                       │
    /// │ 🔌 IMPLEMENTA: ILavadoresLavagemRepository                                                    │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório para relacionamento muitos-para-muitos entre Lavadores e Lavagens.
    /// Registra histórico de participação de lavadores em cada serviço executado.
    /// </summary>
    public class LavadoresLavagemRepository : Repository<LavadoresLavagem>, ILavadoresLavagemRepository
        {
        private new readonly FrotiXDbContext _db;

        public LavadoresLavagemRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetLavadoresLavagemListForDropDown                                          │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers de gerenciamento de lavagens                            │
        /// │    ➡️ CHAMA       : DbContext.LavadoresLavagem, Linq OrderBy/Select                     │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retorna lista de associações lavador-lavagem para uso em DropDown.
        ///    Ordenação por LavagemId, exibindo LavadorId como texto e LavagemId como valor.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Lista ordenada por lavagem com lavadores associados
        /// </para>
        /// </summary>
        /// <returns>Lista de SelectListItem com vínculos lavador-lavagem</returns>
        public IEnumerable<SelectListItem> GetLavadoresLavagemListForDropDown()
            {
            return _db.LavadoresLavagem
            .OrderBy(o => o.LavagemId)
            .Select(i => new SelectListItem()
                {
                Text = i.LavadorId.ToString(),
                Value = i.LavagemId.ToString()
                }); ; ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers de LavadoresLavagem, UnitOfWork                         │
        /// │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualiza registro de participação de lavador em serviço de lavagem.
        ///    Localiza registro existente por LavagemId antes de persistir alterações.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    lavadoresLavagem - Entidade com dados atualizados da associação
        /// </para>
        /// </summary>
        /// <param name="lavadoresLavagem">Entidade LavadoresLavagem com dados a serem persistidos</param>
        public new void Update(LavadoresLavagem lavadoresLavagem)
            {
            var objFromDb = _db.LavadoresLavagem.FirstOrDefault(s => s.LavagemId == lavadoresLavagem.LavagemId);

            _db.Update(lavadoresLavagem);
            _db.SaveChanges();

            }


        }
    }


