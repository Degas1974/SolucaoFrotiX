// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : SetorPatrimonialRepository.cs                                   ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade SetorPatrimonial. Gerencia setores   ║
// ║ patrimoniais (controle de bens móveis e imóveis).                            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetSetorListForDropDown() → SelectList ordenada por NomeSetor              ║
// ║ • Update() → Atualização da entidade SetorPatrimonial                        ║
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
    public class SetorPatrimonialRepository : Repository<SetorPatrimonial>, ISetorPatrimonialRepository
        {
        private new readonly FrotiXDbContext _db;

        public SetorPatrimonialRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetSetorListForDropDown()
            {
            return _db.SetorPatrimonial
            .OrderBy(o => o.NomeSetor)
            .Select(i => new SelectListItem()
                {
                Text = i.NomeSetor,
                Value = i.SetorId.ToString()
                }); ;
            }

        public new void Update(SetorPatrimonial setor)
            {
            var objFromDb = _db.SetorPatrimonial.FirstOrDefault(s => s.SetorId == setor.SetorId);

            _db.Update(setor);
            _db.SaveChanges();

            }


        }
    }


