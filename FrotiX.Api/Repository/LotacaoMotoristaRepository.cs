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
    public class LotacaoMotoristaRepository : Repository<LotacaoMotorista>, ILotacaoMotoristaRepository
        {
        private readonly FrotiXDbContext _db;

        public LotacaoMotoristaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetLotacaoMotoristaListForDropDown()
            {
            return _db.LotacaoMotorista
                .Select(i => new SelectListItem()
                    {
                    });
            }

        public void Update(LotacaoMotorista lotacaoMotorista)
            {
            var objFromDb = _db.LotacaoMotorista.FirstOrDefault(lm => lm.LotacaoMotoristaId == lotacaoMotorista.LotacaoMotoristaId);

            _db.Update(lotacaoMotorista);
            _db.SaveChanges();

            }


        }
    }


