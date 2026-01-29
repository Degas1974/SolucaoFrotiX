// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ViewMotoristasRepository.cs                                     ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para SQL View ViewMotoristas. Fornece visão       ║
// ║ consolidada dos motoristas com dados de contrato e lotação.                   ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetViewMotoristasListForDropDown() → SelectList ordenada por nome          ║
// ║ • Update() → Atualização (não aplicável a Views, apenas compat.)              ║
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
    public class ViewMotoristasRepository : Repository<ViewMotoristas>, IViewMotoristasRepository
        {
        private new readonly FrotiXDbContext _db;

        public ViewMotoristasRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewMotoristasListForDropDown()
            {
            return _db.ViewMotoristas
            .OrderBy(o => o.Nome)
            .Select(i => new SelectListItem()
                {
                Text = i.Nome,
                Value = i.MotoristaId.ToString()
                }); ;
            }

        public new void Update(ViewMotoristas viewMotoristas)
            {
            var objFromDb = _db.ViewMotoristas.FirstOrDefault(s => s.MotoristaId == viewMotoristas.MotoristaId);

            _db.Update(viewMotoristas);
            _db.SaveChanges();

            }


        }
    }


