// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : OperadorRepository.cs                                           ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Operador. Gerencia operadores de     ║
// ║ frota (auxiliares que acompanham motoristas em viagens).                     ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetOperadorListForDropDown() → SelectList ordenada por nome                ║
// ║ • Update() → Atualização da entidade Operador                                ║
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
    public class OperadorRepository : Repository<Operador>, IOperadorRepository
        {
        private new readonly FrotiXDbContext _db;

        public OperadorRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetOperadorListForDropDown()
            {
            return _db.Operador
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.OperadorId.ToString()
                }); ;
            }

        public new void Update(Operador operador)
            {
            var objFromDb = _db.Operador.FirstOrDefault(s => s.OperadorId == operador.OperadorId);

            _db.Update(operador);
            _db.SaveChanges();

            }


        }
    }


