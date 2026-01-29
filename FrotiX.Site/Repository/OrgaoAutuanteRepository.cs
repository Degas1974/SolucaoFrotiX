// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : OrgaoAutuanteRepository.cs                                      ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade OrgaoAutuante. Gerencia órgãos       ║
// ║ emissores de multas de trânsito (DETRAN, PRF, DER, etc).                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetOrgaoAutuanteListForDropDown() → SelectList "Nome (Sigla)"              ║
// ║ • Update() → Atualização da entidade OrgaoAutuante                           ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
    public class OrgaoAutuanteRepository : Repository<OrgaoAutuante>, IOrgaoAutuanteRepository
        {
        private new readonly FrotiXDbContext _db;

        public OrgaoAutuanteRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

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

        public new void Update(OrgaoAutuante orgaoautuante)
            {
            var objFromDb = _db.OrgaoAutuante.FirstOrDefault(s => s.OrgaoAutuanteId == orgaoautuante.OrgaoAutuanteId);

            _db.Update(orgaoautuante);
            _db.SaveChanges();

            }


        }
    }


