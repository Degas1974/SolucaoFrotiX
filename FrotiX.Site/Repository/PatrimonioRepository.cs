// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : PatrimonioRepository.cs                                         ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Patrimonio. Gerencia ativos          ║
// ║ patrimoniais (equipamentos, móveis) da organização.                           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetPatrimonioListForDropDown() → SelectList ordenada por NúmeroSerie       ║
// ║ • Update() → Atualização da entidade Patrimonio                              ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Models.Cadastros;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
    {
    public class PatrimonioRepository : Repository<Patrimonio>, IPatrimonioRepository
        {
        private new readonly FrotiXDbContext _db;

        public PatrimonioRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetPatrimonioListForDropDown()
            {
            return _db.Patrimonio
            .OrderBy(o => o.NumeroSerie)
            .Select(i => new SelectListItem()
                {
                Text = i.NPR,
                Value = i.PatrimonioId.ToString()
                });
            }

        public new void Update(Patrimonio patrimonio)
            {
            var objFromDb = _db.Patrimonio.FirstOrDefault(s => s.PatrimonioId == patrimonio.PatrimonioId);

            _db.Update(patrimonio);
            _db.SaveChanges();

            }


        }
    }


