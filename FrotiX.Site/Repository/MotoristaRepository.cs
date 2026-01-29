// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : MotoristaRepository.cs                                          ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Motorista. Gerencia operações CRUD   ║
// ║ e fornece lista para dropdowns ordenada por nome.                            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetMotoristaListForDropDown() → SelectList ordenada por nome               ║
// ║ • Update() → Atualização da entidade Motorista                               ║
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
    public class MotoristaRepository : Repository<Motorista>, IMotoristaRepository
        {
        private new readonly FrotiXDbContext _db;

        public MotoristaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetMotoristaListForDropDown()
            {
            return _db.Motorista
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.MotoristaId.ToString()
                }); ;
            }

        public new void Update(Motorista motorista)
            {
            var objFromDb = _db.Motorista.FirstOrDefault(s => s.MotoristaId == motorista.MotoristaId);

            _db.Update(motorista);
            _db.SaveChanges();

            }


        }
    }


