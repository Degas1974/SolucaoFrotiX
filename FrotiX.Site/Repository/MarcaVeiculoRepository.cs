// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : MarcaVeiculoRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade MarcaVeiculo. Gerencia marcas de     ║
// ║ veículos (Fiat, VW, Chevrolet, etc).                                          ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetMarcaVeiculoListForDropDown() → SelectList de marcas ativas             ║
// ║ • Update() → Atualização da entidade MarcaVeiculo                            ║
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
    public class MarcaVeiculoRepository : Repository<MarcaVeiculo>, IMarcaVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        public MarcaVeiculoRepository(FrotiXDbContext db)
            : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetMarcaVeiculoListForDropDown()
        {
            return _db
                .MarcaVeiculo.Where(e => e.Status == true)
                .OrderBy(o => o.DescricaoMarca)
                .Select(i => new SelectListItem()
                {
                    Text = i.DescricaoMarca,
                    Value = i.MarcaId.ToString(),
                });
        }

        public new void Update(MarcaVeiculo marcaVeiculo)
        {
            var objFromDb = _db.MarcaVeiculo.FirstOrDefault(s => s.MarcaId == marcaVeiculo.MarcaId);

            _db.Update(marcaVeiculo);
            _db.SaveChanges();
        }
    }
}
