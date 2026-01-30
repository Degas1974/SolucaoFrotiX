/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: VeiculoContratoRepository.cs                                                           ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para associação N:N entre veículos e contratos administrativos.                    ║
   ║    Centraliza listagens para UI e atualização por chave composta.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • VeiculoContratoRepository(FrotiXDbContext db)                                                  ║
   ║    • GetVeiculoContratoListForDropDown()                                                           ║
   ║    • Update(VeiculoContrato veiculoContrato)                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Entidade com chave composta (VeiculoId + ContratoId).                                            ║
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
    /// │ 🎯 CLASSE: VeiculoContratoRepository                                                          │
    /// │ 📦 HERDA DE: Repository<VeiculoContrato>                                                      │
    /// │ 🔌 IMPLEMENTA: IVeiculoContratoRepository                                                     │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela associação entre veículos e contratos administrativos.
    /// Disponibiliza listagem e atualização por chave composta.
    /// </summary>
    public class VeiculoContratoRepository : Repository<VeiculoContrato>, IVeiculoContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: VeiculoContratoRepository                                                     │
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
        public VeiculoContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetVeiculoContratoListForDropDown                                              │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.VeiculoContrato, Select                                    │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Retornar lista de associações veículo/contrato para dropdowns.
        ///    Atualmente está como placeholder, com campos de texto e valor comentados.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção (placeholder).</returns>
        public IEnumerable<SelectListItem> GetVeiculoContratoListForDropDown()
            {
            return _db.VeiculoContrato.Select(i => new SelectListItem()
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
        /// │    ➡️ CHAMA       : DbContext.VeiculoContrato.FirstOrDefault, _db.Update,                 │
        /// │                     _db.SaveChanges                                                     │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualizar a associação veículo/contrato usando chave composta.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    veiculoContrato - Entidade contendo os dados atualizados.
        /// </para>
        /// </summary>
        /// <param name="veiculoContrato">Entidade <see cref="VeiculoContrato"/> com dados atualizados.</param>
        public new void Update(VeiculoContrato veiculoContrato)
            {
            var objFromDb = _db.VeiculoContrato.FirstOrDefault(s => (s.VeiculoId == veiculoContrato.VeiculoId) && (s.ContratoId == veiculoContrato.ContratoId));

            _db.Update(veiculoContrato);
            _db.SaveChanges();

            }


        }
    }
