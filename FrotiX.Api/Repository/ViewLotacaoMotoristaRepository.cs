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
    public class ViewLotacaoMotoristaRepository : Repository<ViewLotacaoMotorista>, IViewLotacaoMotoristaRepository
        {
        private readonly FrotiXDbContext _db;

        public ViewLotacaoMotoristaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        }
    }


