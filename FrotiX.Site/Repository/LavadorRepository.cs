/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LavadorRepository.cs                                                                   ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade Lavador.                                                ║
   ║    Gerencia cadastro de lavadores responsáveis pela limpeza dos veículos da frota.                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • LavadorRepository(FrotiXDbContext db)                                                         ║
   ║    • IEnumerable<SelectListItem> GetLavadorListForDropDown()                                      ║
   ║    • void Update(Lavador lavador)                                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Implementa ILavadorRepository. Herda de Repository<Lavador> para operações CRUD básicas.       ║
   ║    GetLavadorListForDropDown retorna lista ordenada alfabeticamente por nome.                     ║
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
    /// │ 🎯 CLASSE: LavadorRepository                                                                  │
    /// │ 📦 HERDA DE: Repository&lt;Lavador&gt;                                                                │
    /// │ 🔌 IMPLEMENTA: ILavadorRepository                                                             │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório especializado para gerenciamento de lavadores da frota.
    /// Fornece acesso a dados e operações específicas para entidade Lavador.
    /// </summary>
    public class LavadorRepository : Repository<Lavador>, ILavadorRepository
        {
        private new readonly FrotiXDbContext _db;

        public LavadorRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetLavadorListForDropDown                                                   │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers que utilizam dropdowns de lavadores                     │
        /// │    ➡️ CHAMA       : DbContext.Lavador, Linq OrderBy/Select                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retorna lista de lavadores formatada para uso em DropDown/SelectList.
        ///    Ordenação alfabética por nome para facilitar seleção pelo usuário.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Lista de lavadores com Text=Nome e Value=LavadorId
        /// </para>
        /// </summary>
        /// <returns>Lista de SelectListItem ordenada por nome do lavador</returns>
        public IEnumerable<SelectListItem> GetLavadorListForDropDown()
            {
            return _db.Lavador
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.LavadorId.ToString()
                }); ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers de Lavador, UnitOfWork                                  │
        /// │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualiza dados de um lavador existente no banco de dados.
        ///    Sobrescreve método herdado para permitir validações específicas.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    lavador - Entidade Lavador com dados atualizados
        /// </para>
        /// </summary>
        /// <param name="lavador">Entidade Lavador com dados a serem persistidos</param>
        public new void Update(Lavador lavador)
            {
            var objFromDb = _db.Lavador.FirstOrDefault(s => s.LavadorId == lavador.LavadorId);

            _db.Update(lavador);
            _db.SaveChanges();

            }


        }
    }


