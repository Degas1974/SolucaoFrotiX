// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : TipoMultaRepository.cs                                          ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade TipoMulta. Gerencia tipos de multas  ║
// ║ de trânsito (artigos, descrições, valores) conforme CTB.                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetTipoMultaListForDropDown() → SelectList "Artigo - Descricao"            ║
// ║ • Update() → Atualização da entidade TipoMulta                               ║
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
    public class TipoMultaRepository : Repository<TipoMulta>, ITipoMultaRepository
        {
        private new readonly FrotiXDbContext _db;

        public TipoMultaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetTipoMultaListForDropDown()
            {
            return _db.TipoMulta
                .OrderBy(o => o.Artigo)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Artigo + " - " + i.Descricao,
                    Value = i.TipoMultaId.ToString()
                    });
            }

        public new void Update(TipoMulta tipomulta)
            {
            var objFromDb = _db.TipoMulta.FirstOrDefault(s => s.TipoMultaId == tipomulta.TipoMultaId);

            _db.Update(tipomulta);
            _db.SaveChanges();

            }


        }
    }


