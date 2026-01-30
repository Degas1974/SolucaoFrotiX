/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: VeiculoAtaRepository.cs                                                                ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para associação N:N entre veículos e atas de registro de preços.                    ║
   ║    Centraliza listagens para UI e atualização por chave composta.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • VeiculoAtaRepository(FrotiXDbContext db)                                                       ║
   ║    • GetVeiculoAtaListForDropDown()                                                                ║
   ║    • Update(VeiculoAta veiculoAta)                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Entidade com chave composta (VeiculoId + AtaId).                                                 ║
   ║    ⚠️ ATENÇÃO: Listagem de dropdown está com campos comentados (placeholder).                       ║
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
    /// │ 🎯 CLASSE: VeiculoAtaRepository                                                               │
    /// │ 📦 HERDA DE: Repository<VeiculoAta>                                                           │
    /// │ 🔌 IMPLEMENTA: IVeiculoAtaRepository                                                          │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela associação entre veículos e atas de registro de preços.
    /// Disponibiliza listagem e atualização por chave composta.
    /// </summary>
    public class VeiculoAtaRepository : Repository<VeiculoAta>, IVeiculoAtaRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: VeiculoAtaRepository                                                         │
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
        public VeiculoAtaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetVeiculoAtaListForDropDown                                                  │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.VeiculoAta, Select                                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retornar lista de associações veículo/ata para dropdowns.
        ///    Atualmente está como placeholder, com campos de texto e valor comentados.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção (placeholder).</returns>
        public IEnumerable<SelectListItem> GetVeiculoAtaListForDropDown()
            {
            return _db.VeiculoAta.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.VeiculoAta.FirstOrDefault, _db.Update, _db.SaveChanges       │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualizar a associação veículo/ata usando chave composta.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    veiculoAta - Entidade contendo os dados atualizados.
        /// </para>
        /// </summary>
        /// <param name="veiculoAta">Entidade <see cref="VeiculoAta"/> com dados atualizados.</param>
        public new void Update(VeiculoAta veiculoAta)
            {
            var objFromDb = _db.VeiculoAta.FirstOrDefault(s => (s.VeiculoId == veiculoAta.VeiculoId) && (s.AtaId == veiculoAta.AtaId));

            _db.Update(veiculoAta);
            _db.SaveChanges();

            }


        }
    }
