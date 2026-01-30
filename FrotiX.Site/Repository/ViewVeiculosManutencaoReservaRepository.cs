/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewVeiculosManutencaoReservaRepository.cs                                             ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewVeiculosManutencaoReserva.                                      ║
   ║    Fornece visão consolidada de veículos em manutenção/reserva.                                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewVeiculosManutencaoReservaRepository(FrotiXDbContext db)                                   ║
   ║    • GetViewVeiculosManutencaoReservaListForDropDown()                                             ║
   ║    • Update(ViewVeiculosManutencaoReserva viewVeiculosManutencaoReserva)                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Views são somente leitura; Update é mantido por compatibilidade.                                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace FrotiX.Repository
    {
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: ViewVeiculosManutencaoReservaRepository                                            │
    /// │ 📦 HERDA DE: Repository<ViewVeiculosManutencaoReserva>                                        │
    /// │ 🔌 IMPLEMENTA: IViewVeiculosManutencaoReservaRepository                                       │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// Repositório responsável pela view de veículos em manutenção/reserva.
    /// Fornece listagens para UI com dados consolidados.
    /// </summary>
    public class ViewVeiculosManutencaoReservaRepository : Repository<ViewVeiculosManutencaoReserva>, IViewVeiculosManutencaoReservaRepository
        {
        private new readonly FrotiXDbContext _db;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ViewVeiculosManutencaoReservaRepository                                      │
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
        public ViewVeiculosManutencaoReservaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewVeiculosManutencaoReservaListForDropDown                               │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        /// │    ➡️ CHAMA       : DbContext.ViewVeiculosManutencaoReserva, OrderBy, Select            │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter lista da view de veículos em manutenção/reserva para dropdowns.
        ///    Ordena pela descrição do veículo.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        /// </para>
        /// </summary>
        /// <returns>Lista de itens de seleção para veículos em manutenção/reserva.</returns>
        public IEnumerable<SelectListItem> GetViewVeiculosManutencaoReservaListForDropDown()
            {
            return _db.ViewVeiculosManutencaoReserva
            .OrderBy(o => o.Descricao)
            .Select(i => new SelectListItem()
                {
                Text = i.Descricao,
                Value = i.VeiculoId.ToString()
                }); ;
            }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        /// │    ➡️ CHAMA       : DbContext.ViewVeiculosManutencaoReserva.FirstOrDefault, _db.Update,  │
        /// │                     _db.SaveChanges                                                     │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Manter compatibilidade com o padrão de repositórios.
        ///    Views são somente leitura; operação não é recomendada.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    viewVeiculosManutencaoReserva - Entidade com dados da view.
        /// </para>
        /// </summary>
        /// <param name="viewVeiculosManutencaoReserva">Entidade <see cref="ViewVeiculosManutencaoReserva"/>.</param>
        public new void Update(ViewVeiculosManutencaoReserva viewVeiculosManutencaoReserva)
            {
            var objFromDb = _db.ViewVeiculosManutencaoReserva.FirstOrDefault(s => s.VeiculoId == viewVeiculosManutencaoReserva.VeiculoId);

            _db.Update(viewVeiculosManutencaoReserva);
            _db.SaveChanges();
            }
        }
    }
