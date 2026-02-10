/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: AspNetUsersRepository.cs                                                                ║
    ║ 📂 CAMINHO: /Repository                                                                             ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Repositório de usuários (AspNetUsers) integrados ao Identity.                         ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 MÉTODOS: GetAspNetUsersListForDropDown(), Update()                                               ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Data, Repository<T>, SelectListItem                                                 ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
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
    public class AspNetUsersRepository : Repository<AspNetUsers>, IAspNetUsersRepository
        {
        private new readonly FrotiXDbContext _db;

        public AspNetUsersRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetAspNetUsersListForDropDown()
            {
            return _db.AspNetUsers
                .Where(e => (bool)e.Status)
                .OrderBy(o => o.NomeCompleto)
                .Select(i => new SelectListItem()
                    {
                    Text = i.NomeCompleto,
                    Value = i.Id.ToString()
                    });
            }

        public new void Update(AspNetUsers aspNetUsers)
            {
            var objFromDb = _db.AspNetUsers.FirstOrDefault(s => s.Id == aspNetUsers.Id);

            _db.Update(aspNetUsers);
            _db.SaveChanges();

            }


        }
    }


