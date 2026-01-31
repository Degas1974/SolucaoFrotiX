/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RepactuacaoContratoRepository.cs                                                       ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para repactuações/aditivos de contratos administrativos.                            ║
   ║    Centraliza listagens para UI e atualizações da entidade RepactuacaoContrato.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RepactuacaoContratoRepository(FrotiXDbContext db)                                              ║
   ║    • GetRepactuacaoContratoListForDropDown()                                                        ║
   ║    • Update(RepactuacaoContrato RepactuacaoContrato)                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem é ordenada por Descricao para apresentação em dropdowns.                              ║
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
    // │ 🎯 CLASSE: RepactuacaoContratoRepository                                                       │
    // │ 📦 HERDA DE: Repository                                                   │
    // │ 🔌 IMPLEMENTA: IRepactuacaoContratoRepository                                                  │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas repactuações de contratos administrativos.
    // Provê consultas para dropdowns e atualização de registros.
    
    public class RepactuacaoContratoRepository : Repository<RepactuacaoContrato>, IRepactuacaoContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RepactuacaoContratoRepository                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RepactuacaoContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetRepactuacaoContratoListForDropDown                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.RepactuacaoContrato, OrderBy, Select                        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de repactuações de contratos para composição de dropdowns.
        // Ordena os registros pela descrição.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para repactuações de contratos.
        public IEnumerable<SelectListItem> GetRepactuacaoContratoListForDropDown()
            {
            return _db.RepactuacaoContrato
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Descricao,
                    Value = i.RepactuacaoContratoId.ToString()
                    });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.RepactuacaoContrato.FirstOrDefault, _db.Update, _db.SaveChanges │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de uma repactuação de contrato no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // RepactuacaoContrato - Entidade contendo os dados atualizados.
        
        
        // Param RepactuacaoContrato: Entidade <see cref="RepactuacaoContrato"/> com dados atualizados.
        public new void Update(RepactuacaoContrato RepactuacaoContrato)
            {
            var objFromDb = _db.RepactuacaoContrato.FirstOrDefault(s => s.RepactuacaoContratoId == RepactuacaoContrato.RepactuacaoContratoId);

            _db.Update(RepactuacaoContrato);
            _db.SaveChanges();

            }


        }
    }
