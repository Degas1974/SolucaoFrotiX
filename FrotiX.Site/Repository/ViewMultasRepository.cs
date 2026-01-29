// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ViewMultasRepository.cs                                         ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para SQL View ViewMultas. Fornece visão           ║
// ║ consolidada das multas com dados de veículo, tipo, órgão autuante, etc.       ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetviewMultasListForDropDown() → SelectList ordenada por NumInfracao       ║
// ║ • Update() → Atualização (não aplicável a Views, apenas compat.)              ║
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
    public class viewMultasRepository : Repository<ViewMultas>, IviewMultasRepository
        {
        private new readonly FrotiXDbContext _db;

        public viewMultasRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetviewMultasListForDropDown()
            {
            return _db.viewMultas
            .OrderBy(o => o.NumInfracao)
            .Select(i => new SelectListItem()
                {
                Text = i.NumInfracao.ToString(),
                Value = i.MultaId.ToString()
                }); ; ;
            }

        public new void Update(ViewMultas viewMultas)
            {
            var objFromDb = _db.viewMultas.FirstOrDefault(s => s.MultaId == viewMultas.MultaId);

            _db.Update(viewMultas);
            _db.SaveChanges();

            }


        }
    }


