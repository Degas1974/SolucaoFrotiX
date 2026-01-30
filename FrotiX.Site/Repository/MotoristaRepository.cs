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
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: MotoristaRepository                                                                │
    /// │ 📦 HERDA DE: Repository&lt;Motorista&gt;                                                              │
    /// │ 🔌 IMPLEMENTA: IMotoristaRepository                                                           │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório especializado para gerenciamento de motoristas da frota.
    /// Fornece acesso a dados e operações específicas para entidade Motorista.
    /// </summary>
    public class MotoristaRepository : Repository<Motorista>, IMotoristaRepository
        {
        private new readonly FrotiXDbContext _db;

        public MotoristaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetMotoristaListForDropDown                                                 │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers que utilizam dropdowns de motoristas                    │
        /// │    ➡️ CHAMA       : DbContext.Motorista, Linq OrderBy/Select                            │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retorna lista de motoristas formatada para uso em DropDown/SelectList.
        ///    Ordenação alfabética por nome para facilitar seleção pelo usuário.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Lista de motoristas com Text=Nome e Value=MotoristaId
        /// </para>
        /// </summary>
        /// <returns>Lista de SelectListItem ordenada por nome do motorista</returns>
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

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers de Motorista, UnitOfWork                                │
        /// │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualiza dados de um motorista existente no banco de dados.
        ///    Sobrescreve método herdado para permitir validações específicas.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    motorista - Entidade Motorista com dados atualizados
        /// </para>
        /// </summary>
        /// <param name="motorista">Entidade Motorista com dados a serem persistidos</param>
        public new void Update(Motorista motorista)
            {
            var objFromDb = _db.Motorista.FirstOrDefault(s => s.MotoristaId == motorista.MotoristaId);

            _db.Update(motorista);
            _db.SaveChanges();

            }


        }
    }


