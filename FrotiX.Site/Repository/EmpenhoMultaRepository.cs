// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : EmpenhoMultaRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para empenhos específicos de multas de trânsito.                 ║
// ║ Vincula notas de empenho aos órgãos autuantes para controle financeiro.      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetEmpenhoMultaListForDropDown() → Lista empenhos com sigla do órgão       ║
// ║ • Update() → Atualiza registro de empenho de multa                           ║
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
    public class EmpenhoMultaRepository : Repository<EmpenhoMulta>, IEmpenhoMultaRepository
        {
        private new readonly FrotiXDbContext _db;

        public EmpenhoMultaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetEmpenhoMultaListForDropDown()
            {
            return _db.EmpenhoMulta
            .Join(_db.OrgaoAutuante, empenhomulta => empenhomulta.OrgaoAutuanteId, orgaoautuante => orgaoautuante.OrgaoAutuanteId, (empenhomulta, orgaoautuante) => new { empenhomulta, orgaoautuante })
            .OrderBy(o => o.empenhomulta.NotaEmpenho)
            .Select(i => new SelectListItem()
                {
                Text = i.empenhomulta.NotaEmpenho + "(" + i.orgaoautuante.Sigla + "/" + i.orgaoautuante.Nome + ")",
                Value = i.empenhomulta.EmpenhoMultaId.ToString()
                });
            }

        public new void Update(EmpenhoMulta empenhomulta)
            {
            var objFromDb = _db.EmpenhoMulta.FirstOrDefault(s => s.EmpenhoMultaId == empenhomulta.EmpenhoMultaId);

            _db.Update(empenhomulta);
            _db.SaveChanges();

            }
        }
    }


