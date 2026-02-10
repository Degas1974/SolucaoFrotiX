/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewContratoFornecedorRepository.cs                                                    ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório para a SQL View ViewContratoFornecedor.                                             ║
   ║    Disponibiliza listagens consolidadas de contratos e fornecedores.                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewContratoFornecedorRepository(FrotiXDbContext db)                                          ║
   ║    • GetViewContratoFornecedorListForDropDown()                                                    ║
   ║    • Update(ViewContratoFornecedor viewContratoFornecedor)                                         ║
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
    // │ 🎯 CLASSE: ViewContratoFornecedorRepository                                                   │
    // │ 📦 HERDA DE: Repository                                               │
    // │ 🔌 IMPLEMENTA: IViewContratoFornecedorRepository                                              │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório responsável pela view de contratos e fornecedores.
    // Fornece listagens para UI com dados consolidados.
    
    public class ViewContratoFornecedorRepository : Repository<ViewContratoFornecedor>, IViewContratoFornecedorRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewContratoFornecedorRepository                                              │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, Services, Controllers                                     │
        // │    ➡️ CHAMA       : base(db)                                                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório com o contexto do banco de dados.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto do banco de dados da aplicação.
        
        
        // Param db: Instância de <see cref="FrotiXDbContext"/>.
        public ViewContratoFornecedorRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewContratoFornecedorListForDropDown                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services, UI (DropDowns)                                │
        // │    ➡️ CHAMA       : DbContext.ViewContratoFornecedor, OrderBy, Select                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter lista da view de contratos e fornecedores para composição de dropdowns.
        // Ordena pela descrição do contrato.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens prontos para seleção em UI.
        
        
        // Returns: Lista de itens de seleção para contratos/fornecedores.
        public IEnumerable<SelectListItem> GetViewContratoFornecedorListForDropDown()
            {
            return _db.ViewContratoFornecedor
            .OrderBy(o => o.Descricao)
            .Select(i => new SelectListItem()
                {
                Text = i.Descricao.ToString(),
                Value = i.ContratoId.ToString()
                }); ; ;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Services                                                 │
        // │    ➡️ CHAMA       : DbContext.ViewContratoFornecedor.FirstOrDefault, _db.Update,         │
        // │                     _db.SaveChanges                                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com o padrão de repositórios.
        // Views são somente leitura; operação não é recomendada.
        
        
        
        // 📥 PARÂMETROS:
        // viewContratoFornecedor - Entidade com dados da view.
        
        
        // Param viewContratoFornecedor: Entidade <see cref="ViewContratoFornecedor"/>.
        public new void Update(ViewContratoFornecedor viewContratoFornecedor)
            {
            var objFromDb = _db.ViewContratoFornecedor.FirstOrDefault(s => s.ContratoId == viewContratoFornecedor.ContratoId);

            _db.Update(viewContratoFornecedor);
            _db.SaveChanges();

            }


        }
    }
