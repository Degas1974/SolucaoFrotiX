// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : LavadorRepository.cs                                            ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Lavador. Gerencia lavadores          ║
// ║ responsáveis pela limpeza dos veículos da frota.                              ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetLavadorListForDropDown() → SelectList ordenada por nome                 ║
// ║ • Update() → Atualização da entidade Lavador                                 ║
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
    public class LavadorRepository : Repository<Lavador>, ILavadorRepository
        {
        private new readonly FrotiXDbContext _db;

        public LavadorRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetLavadorListForDropDown()
            {
            return _db.Lavador
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.LavadorId.ToString()
                }); ;
            }

        public new void Update(Lavador lavador)
            {
            var objFromDb = _db.Lavador.FirstOrDefault(s => s.LavadorId == lavador.LavadorId);

            _db.Update(lavador);
            _db.SaveChanges();

            }


        }
    }


