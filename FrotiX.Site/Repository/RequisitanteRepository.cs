// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : RequisitanteRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Requisitante. Gerencia os            ║
// ║ solicitantes de viagens com seu ponto de contato.                            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetRequisitanteListForDropDown() → SelectList "Nome(Ponto)" de ativos      ║
// ║ • Update() → Atualização da entidade Requisitante                            ║
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
    public class RequisitanteRepository : Repository<Requisitante>, IRequisitanteRepository
    {
        private new readonly FrotiXDbContext _db;

        public RequisitanteRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetRequisitanteListForDropDown()
        {
            return _db
                .Requisitante.Where(s => s.Status == true)
                .OrderBy(o => o.Nome)
                .Select(i => new SelectListItem()
                {
                    Text = i.Nome + "(" + i.Ponto + ")",
                    Value = i.RequisitanteId.ToString(),
                });
        }

        public new void Update(Requisitante requisitante)
        {
            var objFromDb = _db.Requisitante.FirstOrDefault(s =>
                s.RequisitanteId == requisitante.RequisitanteId
            );

            _db.Update(requisitante);
            _db.SaveChanges();
        }
    }
}
