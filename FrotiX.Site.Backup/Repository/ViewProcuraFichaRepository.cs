/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewProcuraFichaRepository.cs                                                          ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewProcuraFicha.                                                   ║
   ║    Disponibiliza dados consolidados de procura de fichas por veículo.                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewProcuraFichaRepository(FrotiXDbContext db)                                                 ║
   ║    • GetViewProcuraFichaListForDropDown()                                                          ║
   ║    • Update(ViewProcuraFicha viewProcuraFicha)                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    Views são somente leitura; Update é mantido por compatibilidade.                                ║
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

    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ViewProcuraFichaRepository                                                         │
    // │ 📦 HERDA DE: Repository                                                     │
    // │ 🔌 IMPLEMENTA: IViewProcuraFichaRepository                                                    │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯

    // Repositório responsável pela view de procura de fichas.
    // Fornece listagens para UI com dados consolidados.

    public class ViewProcuraFichaRepository : Repository<ViewProcuraFicha>, IViewProcuraFichaRepository
        {
        private new readonly FrotiXDbContext _db;


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewProcuraFichaRepository                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.



        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.


        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewProcuraFichaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewProcuraFichaListForDropDown                                           │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewProcuraFicha, OrderBy, Select                          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯


        // 🎯 OBJETIVO:
        // Obter lista da view de procura de fichas para dropdowns.
        // Ordena pela data inicial.



        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.


        // Returns: Lista de itens de seleção para procura de fichas.
        public IEnumerable<SelectListItem> GetViewProcuraFichaListForDropDown()
            {
            return _db.ViewProcuraFicha
            .OrderBy(o => o.DataInicial)
            .Select(i => new SelectListItem()
                {
                Text = i.DataInicial.ToString(),
                Value = i.VeiculoId.ToString()
                }); ; ;
            }

        public new void Update(ViewProcuraFicha viewProcuraFicha)
            {
            var objFromDb = _db.ViewProcuraFicha.FirstOrDefault(s => s.VeiculoId == viewProcuraFicha.VeiculoId);

            _db.Update(viewProcuraFicha);
            _db.SaveChanges();

            }


        }
    }


