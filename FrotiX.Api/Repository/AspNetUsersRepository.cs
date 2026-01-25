using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Data;
using FrotiXApi.Models;
using FrotiXApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository
    {
    public class AspNetUsersRepository : Repository<AspNetUsers>, IAspNetUsersRepository
        {
        private readonly FrotiXDbContext _db;

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

        public void Update(AspNetUsers aspNetUsers)
            {
            var objFromDb = _db.AspNetUsers.FirstOrDefault(s => s.Id == aspNetUsers.Id);

            _db.Update(aspNetUsers);
            _db.SaveChanges();

            }


        }
    }


