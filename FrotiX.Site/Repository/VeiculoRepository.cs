/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: VeiculoRepository.cs                                                                   ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para veículos, com suporte a listagens simples e completas.                         ║
   ║    Utiliza ViewVeiculos para dados enriquecidos e atualização direta sem re-busca.                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • VeiculoRepository(FrotiXDbContext db)                                                          ║
   ║    • GetVeiculoListForDropDown()                                                                   ║
   ║    • GetVeiculoCompletoListForDropDown()                                                           ║
   ║    • Update(Veiculo veiculo)                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem completa usa ViewVeiculos e filtra Status == true.                                   ║
   ║    A atualização evita re-busca para reduzir conflitos de tracking no EF Core.                     ║
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
    /// │ 🎯 CLASSE: VeiculoRepository                                                                  │
    /// │ 📦 HERDA DE: Repository<Veiculo>                                                              │
    /// │ 🔌 IMPLEMENTA: IVeiculoRepository                                                             │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pelas operações de veículos.
    /// Disponibiliza listagens simples e completas para UI.
    /// </summary>
    public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: VeiculoRepository                                                            │
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
        public VeiculoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetVeiculoListForDropDown                                                    │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.Veiculo, OrderBy, Select                                   │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista simples de veículos para composição de dropdowns.
        ///    Ordena os registros pela placa.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção com placas.</returns>
        public IEnumerable<SelectListItem> GetVeiculoListForDropDown()
            {
            return _db.Veiculo
            .OrderBy(o => o.Placa)
            .Select(i => new SelectListItem()
                {
                Text = i.Placa,
                Value = i.VeiculoId.ToString()
                }); ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetVeiculoCompletoListForDropDown                                             │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViewVeiculos, Where, OrderBy, Select                       │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista completa de veículos para composição de dropdowns.
        ///    Usa ViewVeiculos, filtra ativos e retorna descrição completa do veículo.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção com dados completos.</returns>
        public IEnumerable<SelectListItem> GetVeiculoCompletoListForDropDown()
        {
            return _db.ViewVeiculos
                .Where(v => v.Status == true)
                .OrderBy(v => v.Placa)
                .Select(v => new SelectListItem()
                {
                    Text = v.VeiculoCompleto,
                    Value = v.VeiculoId.ToString()
                });
        }


        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : _db.Update, _db.SaveChanges                                           │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Atualizar os dados de um veículo sem re-busca no banco.
        ///    Evita conflitos de tracking no EF Core.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    veiculo - Entidade contendo os dados atualizados.
        /// </para>
        /// </summary>
        /// <param name="veiculo">Entidade <see cref="Veiculo"/> com dados atualizados.</param>
        public new void Update(Veiculo veiculo)
            {
            // Atualiza diretamente sem buscar novamente do banco
            // (evita conflito de tracking no EF Core)
            _db.Update(veiculo);
            _db.SaveChanges();
            }


        }
    }
