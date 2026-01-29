// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : MediaCombustivelRepository.cs                                   ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para médias de preço de combustível por período.                 ║
// ║ Consolida valores mensais por nota fiscal, combustível e ano/mês.            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetMediaCombustivelListForDropDown() → Lista médias ordenadas por ano      ║
// ║ • Update() → Atualiza registro de média de combustível                       ║
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
    public class MediaCombustivelRepository : Repository<MediaCombustivel>, IMediaCombustivelRepository
        {
        private new readonly FrotiXDbContext _db;

        public MediaCombustivelRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetMediaCombustivelListForDropDown()
            {
            return _db.MediaCombustivel
                .OrderBy(o => o.Ano)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Ano.ToString(),
                    Value = i.CombustivelId.ToString()
                    });
            }

        public new void Update(MediaCombustivel mediacombustivel)
            {
            var objFromDb = _db.MediaCombustivel.FirstOrDefault(s => (s.CombustivelId == mediacombustivel.CombustivelId) && (s.NotaFiscalId == mediacombustivel.NotaFiscalId) && (s.Ano == mediacombustivel.Ano) && (s.Mes == mediacombustivel.Mes));

            _db.Update(mediacombustivel);
            _db.SaveChanges();

            }


        }
    }


