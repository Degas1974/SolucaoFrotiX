/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewMotoristaFluxo.cs                                                                  ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório da view de fluxo de motoristas para consultas e listas de seleção.                 ║
   ║    Disponibiliza métodos para popular dropdowns por nome de motorista.                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewMotoristaFluxoRepository(FrotiXDbContext db)                                               ║
   ║    • GetViewMotoristaFluxoListForDropDown()                                                         ║
   ║    • Update(ViewMotoristaFluxo viewMotoristaFluxo)                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A entidade é uma view; o método Update existe por compatibilidade e usa DbContext.Update().     ║
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
    // │ 🎯 CLASSE: ViewMotoristaFluxoRepository                                                     │
    // │ 📦 HERDA DE: Repository                                                 │
    // │ 🔌 IMPLEMENTA: IViewMotoristaFluxoRepository                                                │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório de leitura da view de fluxo de motoristas no FrotiX.
    
    public class ViewMotoristaFluxoRepository : Repository<ViewMotoristaFluxo>, IViewMotoristaFluxoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewMotoristaFluxoRepository                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, DI                                                       │
        // │    ➡️ CHAMA       : Repository                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório da view de fluxo de motoristas com o contexto atual.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto de dados do FrotiX
        
        
        // Param db: Instância do contexto utilizada pelo repositório.
        public ViewMotoristaFluxoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewMotoristaFluxoListForDropDown                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers, UI                                             │
        // │    ➡️ CHAMA       : _db.ViewMotoristaFluxo, OrderBy, Select                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Gerar lista de itens para dropdown com os motoristas da view.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens ordenados por nome do motorista.
        
        
        // Returns: Lista de itens de seleção de motoristas.
        public IEnumerable<SelectListItem> GetViewMotoristaFluxoListForDropDown()
            {
            return _db.ViewMotoristaFluxo
            .OrderBy(o => o.NomeMotorista)
            .Select(i => new SelectListItem()
                {
                Text = i.NomeMotorista.ToString(),
                Value = i.NomeMotorista.ToString()
                }); ; ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers                                                 │
        // │    ➡️ CHAMA       : _db.Update, _db.SaveChanges                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Encaminhar atualização de entidade da view quando necessário.
        
        
        
        // 📥 PARÂMETROS:
        // viewMotoristaFluxo - Entidade da view a ser atualizada
        
        
        // Param viewMotoristaFluxo: Entidade de fluxo de motorista a atualizar.
        public new void Update(ViewMotoristaFluxo viewMotoristaFluxo)
            {
            var objFromDb = _db.ViewMotoristaFluxo.FirstOrDefault(s => s.NomeMotorista == viewMotoristaFluxo.NomeMotorista);

            _db.Update(viewMotoristaFluxo);
            _db.SaveChanges();

            }


        }
    }


