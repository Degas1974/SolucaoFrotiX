/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViewOcorrencia.cs                                                                      ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório da view de ocorrências para consultas e listas de seleção.                         ║
   ║    Disponibiliza métodos para popular dropdowns a partir da view de ocorrências.                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ViewOcorrenciaRepository(FrotiXDbContext db)                                                   ║
   ║    • GetViewOcorrenciaListForDropDown()                                                             ║
   ║    • Update(ViewOcorrencia viewOcorrencia)                                                          ║
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
    // │ 🎯 CLASSE: ViewOcorrenciaRepository                                                         │
    // │ 📦 HERDA DE: Repository                                                     │
    // │ 🔌 IMPLEMENTA: IViewOcorrenciaRepository                                                    │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório de leitura da view de ocorrências no FrotiX.
    
    public class ViewOcorrenciaRepository : Repository<ViewOcorrencia>, IViewOcorrenciaRepository
        {
        private new readonly FrotiXDbContext _db;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ViewOcorrenciaRepository                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UnitOfWork, DI                                                       │
        // │    ➡️ CHAMA       : Repository                                          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Inicializar o repositório da view de ocorrências com o contexto atual.
        
        
        
        // 📥 PARÂMETROS:
        // db - Contexto de dados do FrotiX
        
        
        // Param db: Instância do contexto utilizada pelo repositório.
        public ViewOcorrenciaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetViewOcorrenciaListForDropDown                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Services, Controllers, UI                                             │
        // │    ➡️ CHAMA       : _db.ViewOcorrencia, OrderBy, Select                                  │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Gerar lista de itens para dropdown a partir das ocorrências registradas na view.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Itens ordenados pela data inicial.
        
        
        // Returns: Lista de itens de seleção para ocorrências.
        public IEnumerable<SelectListItem> GetViewOcorrenciaListForDropDown()
            {
            return _db.ViewOcorrencia
            .OrderBy(o => o.DataInicial)
            .Select(i => new SelectListItem()
                {
                Text = i.DataInicial.ToString(),
                Value = i.ViagemId.ToString()
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
        // viewOcorrencia - Entidade da view a ser atualizada
        
        
        // Param viewOcorrencia: Entidade de ocorrência a atualizar.
        public new void Update(ViewOcorrencia viewOcorrencia)
            {
            var objFromDb = _db.ViewOcorrencia.FirstOrDefault(s => s.ViagemId == viewOcorrencia.ViagemId);

            _db.Update(viewOcorrencia);
            _db.SaveChanges();

            }


        }
    }


