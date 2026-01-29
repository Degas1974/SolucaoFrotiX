// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ViewVeiculosRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para SQL View ViewVeiculos. Fornece visão         ║
// ║ consolidada dos veículos com dados de marca, modelo, contrato, etc.           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetViewVeiculosListForDropDown() → SelectList ordenada por placa           ║
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
    public class ViewVeiculosRepository : Repository<ViewVeiculos>, IViewVeiculosRepository
        {
        private new readonly FrotiXDbContext _db;

        public ViewVeiculosRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewVeiculosListForDropDown()
            {
            return _db.ViewVeiculos
            .OrderBy(o => o.Placa)
            .Select(i => new SelectListItem()
                {
                Text = i.Placa,
                Value = i.VeiculoId.ToString()
                }); ;
            }

        public new void Update(ViewVeiculos viewVeiculos)
            {
            var objFromDb = _db.ViewVeiculos.FirstOrDefault(s => s.VeiculoId == viewVeiculos.VeiculoId);

            _db.Update(viewVeiculos);
            _db.SaveChanges();

            }


        }
    }


