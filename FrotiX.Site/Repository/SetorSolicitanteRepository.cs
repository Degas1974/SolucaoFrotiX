// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : SetorSolicitanteRepository.cs                                   ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade SetorSolicitante. Gerencia os        ║
// ║ setores que podem solicitar viagens/serviços de frota.                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetSetorSolicitanteListForDropDown() → SelectList ordenada por nome        ║
// ║ • Update() → Atualização da entidade SetorSolicitante                        ║
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
    public class SetorSolicitanteRepository : Repository<SetorSolicitante>, ISetorSolicitanteRepository
        {
        private new readonly FrotiXDbContext _db;

        public SetorSolicitanteRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetSetorSolicitanteListForDropDown()
            {
            return _db.SetorSolicitante
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.SetorSolicitanteId.ToString()
                }); ;
            }

        public new void Update(SetorSolicitante setorSolicitante)
            {
            var objFromDb = _db.SetorSolicitante.FirstOrDefault(s => s.SetorSolicitanteId == setorSolicitante.SetorSolicitanteId);

            _db.Update(setorSolicitante);
            _db.SaveChanges();

            }


        }
    }


