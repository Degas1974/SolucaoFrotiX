// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : MultaRepository.cs                                              ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Multa. Gerencia multas de trânsito   ║
// ║ aplicadas aos veículos da frota.                                              ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetMultaListForDropDown() → SelectList ordenada por NumInfracao            ║
// ║ • Update() → Atualização da entidade Multa                                   ║
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
    public class MultaRepository : Repository<Multa>, IMultaRepository
        {
        private new readonly FrotiXDbContext _db;

        public MultaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetMultaListForDropDown()
            {
            return _db.Multa
                .OrderBy(o => o.NumInfracao)
                .Select(i => new SelectListItem()
                    {
                    Text = i.NumInfracao,
                    Value = i.MultaId.ToString()
                    });
            }

        public new void Update(Multa multa)
            {
            var objFromDb = _db.Multa.FirstOrDefault(s => s.MultaId == multa.MultaId);

            _db.Update(multa);
            _db.SaveChanges();

            }


        }
    }


