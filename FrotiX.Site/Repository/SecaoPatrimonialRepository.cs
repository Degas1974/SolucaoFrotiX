// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : SecaoPatrimonialRepository.cs                                   ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade SecaoPatrimonial. Gerencia seções    ║
// ║ patrimoniais (subdivisões de setores).                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetSecaoListForDropDown() → SelectList "NomeSecao/SetorId"                 ║
// ║ • Update() → Atualização da entidade SecaoPatrimonial                        ║
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
    public class SecaoPatrimonialRepository : Repository<SecaoPatrimonial>, ISecaoPatrimonialRepository
        {
        private new readonly FrotiXDbContext _db;

        public SecaoPatrimonialRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetSecaoListForDropDown()
            {
            return _db.SecaoPatrimonial
            .OrderBy(o => o.NomeSecao)
            .Select(i => new SelectListItem()
                {
                Text = i.NomeSecao + "/" + i.SetorId.ToString(),
                Value = i.SecaoId.ToString()
                }); ;
            }

        public new void Update(SecaoPatrimonial secao)
            {
            var objFromDb = _db.SecaoPatrimonial.FirstOrDefault(s => s.SecaoId == secao.SecaoId);

            _db.Update(secao);
            _db.SaveChanges();

            }


        }
    }


