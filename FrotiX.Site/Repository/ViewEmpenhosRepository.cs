// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ViewEmpenhosRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para SQL View ViewEmpenhos. Fornece visão         ║
// ║ consolidada de empenhos orçamentários com dados de contrato e fornecedor.    ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetViewEmpenhosListForDropDown() → SelectList ordenada por NotaEmpenho     ║
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
    public class ViewEmpenhosRepository : Repository<ViewEmpenhos>, IViewEmpenhosRepository
        {
        private new readonly FrotiXDbContext _db;

        public ViewEmpenhosRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewEmpenhosListForDropDown()
            {
            return _db.ViewEmpenhos
            .OrderBy(o => o.NotaEmpenho)
            .Select(i => new SelectListItem()
                {
                Text = i.NotaEmpenho,
                Value = i.EmpenhoId.ToString()
                });
            }

        public new void Update(ViewEmpenhos viewEmpenhos)
            {
            var objFromDb = _db.ViewEmpenhos.FirstOrDefault(s => s.EmpenhoId == viewEmpenhos.EmpenhoId);

            _db.Update(viewEmpenhos);
            _db.SaveChanges();

            }
        }
    }


