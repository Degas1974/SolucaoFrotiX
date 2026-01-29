// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : UnidadeRepository.cs                                            ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Unidade. Gerencia unidades           ║
// ║ organizacionais (órgãos, departamentos, setores).                             ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetUnidadeListForDropDown() → SelectList "Sigla - Descricao" de ativos     ║
// ║ • Update() → Atualização da entidade Unidade                                 ║
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
    public class UnidadeRepository : Repository<Unidade>, IUnidadeRepository
    {
        private new readonly FrotiXDbContext _db;

        public UnidadeRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetUnidadeListForDropDown()
        {
            return _db
                .Unidade.Where(e => e.Status == true)
                .OrderBy(o => o.Sigla + " - " + o.Descricao)
                .Select(i => new SelectListItem()
                {
                    Text = i.Sigla + " - " + i.Descricao,
                    Value = i.UnidadeId.ToString(),
                });
        }

        public new void Update(Unidade unidade)
        {
            var objFromDb = _db.Unidade.FirstOrDefault(s => s.UnidadeId == unidade.UnidadeId);

            _db.Update(unidade);
            _db.SaveChanges();
        }
    }
}
