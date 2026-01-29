// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : PlacaBronzeRepository.cs                                        ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para placas de bronze (identificação patrimonial de veículos).   ║
// ║ Gerencia cadastro de placas metálicas com numeração patrimonial.             ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetPlacaBronzeListForDropDown() → Lista placas ativas ordenadas            ║
// ║ • Update() → Atualiza registro de placa de bronze                            ║
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
    public class PlacaBronzeRepository : Repository<PlacaBronze>, IPlacaBronzeRepository
    {
        private new readonly FrotiXDbContext _db;

        public PlacaBronzeRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetPlacaBronzeListForDropDown()
        {
            return _db
                .PlacaBronze.Where(e => e.Status == true) // Mudança aqui
                .OrderBy(o => o.DescricaoPlaca)
                .Select(i => new SelectListItem()
                {
                    Text = i.DescricaoPlaca,
                    Value = i.PlacaBronzeId.ToString(),
                });
        }

        public new void Update(PlacaBronze placaBronze)
        {
            var objFromDb = _db.PlacaBronze.FirstOrDefault(s =>
                s.PlacaBronzeId == placaBronze.PlacaBronzeId
            );

            _db.Update(placaBronze);
            _db.SaveChanges();
        }
    }
}
