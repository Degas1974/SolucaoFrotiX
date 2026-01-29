// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : VeiculoAtaRepository.cs                                         ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade VeiculoAta. Gerencia associação      ║
// ║ N:N entre veículos e atas de registro de preços.                              ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetVeiculoAtaListForDropDown() → SelectList (placeholder)                  ║
// ║ • Update() → Atualização por chave composta (VeiculoId + AtaId)              ║
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
    public class VeiculoAtaRepository : Repository<VeiculoAta>, IVeiculoAtaRepository
        {
        private new readonly FrotiXDbContext _db;

        public VeiculoAtaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetVeiculoAtaListForDropDown()
            {
            return _db.VeiculoAta.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        public new void Update(VeiculoAta veiculoAta)
            {
            var objFromDb = _db.VeiculoAta.FirstOrDefault(s => (s.VeiculoId == veiculoAta.VeiculoId) && (s.AtaId == veiculoAta.AtaId));

            _db.Update(veiculoAta);
            _db.SaveChanges();

            }


        }
    }


