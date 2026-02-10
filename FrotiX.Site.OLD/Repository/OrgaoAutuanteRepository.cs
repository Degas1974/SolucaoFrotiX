/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: OrgaoAutuanteRepository.cs                                                             ║
   ║ 📂 CAMINHO: Repository/                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Repositório especializado para entidade OrgaoAutuante.                                          ║
   ║    Gerencia órgãos emissores de multas de trânsito (DETRAN, PRF, DER, Polícia Rodoviária, etc).  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • OrgaoAutuanteRepository(FrotiXDbContext db)                                                   ║
   ║    • IEnumerable<SelectListItem> GetOrgaoAutuanteListForDropDown()                                ║
   ║    • void Update(OrgaoAutuante orgaoautuante)                                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ OBSERVAÇÕES:                                                                                     ║
   ║    GetOrgaoAutuanteListForDropDown exibe "Nome (Sigla)" para fácil identificação.                 ║
   ║    Essencial para controle e registro correto de multas de trânsito da frota.                    ║
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
    // │ 🎯 CLASSE: OrgaoAutuanteRepository                                                            │
    // │ 📦 HERDA DE: Repository&lt;OrgaoAutuante&gt;                                                          │
    // │ 🔌 IMPLEMENTA: IOrgaoAutuanteRepository                                                       │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    // Repositório especializado para gerenciamento de órgãos autuantes.
    // Gerencia cadastro de entidades emissoras de multas de trânsito (DETRAN, PRF, DER, etc).
    
    public class OrgaoAutuanteRepository : Repository<OrgaoAutuante>, IOrgaoAutuanteRepository
        {
        private new readonly FrotiXDbContext _db;

        public OrgaoAutuanteRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetOrgaoAutuanteListForDropDown                                             │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers que registram multas de trânsito                       │
        // │    ➡️ CHAMA       : DbContext.OrgaoAutuante, Linq OrderBy/Select                        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Retorna lista de órgãos autuantes para uso em DropDown/SelectList.
        // Formato de exibição: "Nome (Sigla)" - Ex: "DETRAN-PA (DETRAN)", "PRF (PRF)".
        // Ordenação alfabética por nome do órgão.
        
        
        
        // 📤 RETORNO:
        // IEnumerable&lt;SelectListItem&gt; - Lista com Text="Nome (Sigla)" e Value=OrgaoAutuanteId
        
        
        // Returns: Lista de SelectListItem ordenada alfabeticamente por nome do órgão
        public IEnumerable<SelectListItem> GetOrgaoAutuanteListForDropDown()
            {
            return _db.OrgaoAutuante
                .OrderBy(o => o.Nome)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Nome + " (" + i.Sigla + ")",
                    Value = i.OrgaoAutuanteId.ToString()
                    });
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Update                                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                    │
        // │    ⬅️ CHAMADO POR : Controllers de OrgaoAutuante, UnitOfWork                            │
        // │    ➡️ CHAMA       : DbContext.Update(), DbContext.SaveChanges()                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Atualiza dados de um órgão autuante existente no banco de dados.
        // Permite correções em nome, sigla e demais informações cadastrais.
        
        
        
        // 📥 PARÂMETROS:
        // orgaoautuante - Entidade OrgaoAutuante com dados atualizados
        
        
        // Param orgaoautuante: Entidade OrgaoAutuante com dados a serem persistidos
        public new void Update(OrgaoAutuante orgaoautuante)
            {
            var objFromDb = _db.OrgaoAutuante.FirstOrDefault(s => s.OrgaoAutuanteId == orgaoautuante.OrgaoAutuanteId);

            _db.Update(orgaoautuante);
            _db.SaveChanges();

            }


        }
    }


