/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RepactuacaoServicosRepository.cs                                                       ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para repactuações de serviços em contratos administrativos.                         ║
   ║    Gerencia reajustes de serviços como manutenção, lavagem e similares.                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • RepactuacaoServicosRepository(FrotiXDbContext db)                                              ║
   ║    • GetRepactuacaoServicosListForDropDown()                                                       ║
   ║    • Update(RepactuacaoServicos repactuacaoServicos)                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    A listagem usa Valor como texto e RepactuacaoContratoId como identificador.                     ║
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
    // │ 🎯 CLASSE: RepactuacaoServicosRepository                                                       │
    // │ 📦 HERDA DE: Repository                                                   │
    // │ 🔌 IMPLEMENTA: IRepactuacaoServicosRepository                                                  │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pelas repactuações de serviços em contratos.
    // Centraliza listagens para UI e atualização de registros.
    
    public class RepactuacaoServicosRepository : Repository<RepactuacaoServicos>, IRepactuacaoServicosRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: RepactuacaoServicosRepository                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public RepactuacaoServicosRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetRepactuacaoServicosListForDropDown                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.RepactuacaoServicos, Select                                │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista de repactuações de serviços para composição de dropdowns.
        // Exibe o valor da repactuação e usa o vínculo do contrato como chave.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para repactuações de serviços.
        public IEnumerable<SelectListItem> GetRepactuacaoServicosListForDropDown()
            {
            return _db.RepactuacaoServicos
                .Select(i => new SelectListItem()
                    {
                    Text = i.Valor.ToString(),
                    Value = i.RepactuacaoContratoId.ToString()
                    });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.RepactuacaoServicos.FirstOrDefault, _db.Update,             │
        // │                     _db.SaveChanges                                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualizar os dados de uma repactuação de serviços no banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // repactuacaoServicos - Entidade contendo os dados atualizados.
        
        
        // Param repactuacaoServicos: Entidade <see cref="RepactuacaoServicos"/> com dados atualizados.
        public new void Update(RepactuacaoServicos repactuacaoServicos)
            {
            var objFromDb = _db.RepactuacaoServicos.FirstOrDefault(s => s.RepactuacaoServicoId == repactuacaoServicos.RepactuacaoServicoId);

            _db.Update(repactuacaoServicos);
            _db.SaveChanges();

            }


        }
    }
