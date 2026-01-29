// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : VeiculoContratoRepository.cs                                    ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade VeiculoContrato. Gerencia associação ║
// ║ N:N entre veículos e contratos administrativos.                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetVeiculoContratoListForDropDown() → SelectList (placeholder)             ║
// ║ • Update() → Atualização por chave composta (VeiculoId + ContratoId)         ║
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
    public class VeiculoContratoRepository : Repository<VeiculoContrato>, IVeiculoContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        public VeiculoContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetVeiculoContratoListForDropDown()
            {
            return _db.VeiculoContrato.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        public new void Update(VeiculoContrato veiculoContrato)
            {
            var objFromDb = _db.VeiculoContrato.FirstOrDefault(s => (s.VeiculoId == veiculoContrato.VeiculoId) && (s.ContratoId == veiculoContrato.ContratoId));

            _db.Update(veiculoContrato);
            _db.SaveChanges();

            }


        }
    }


