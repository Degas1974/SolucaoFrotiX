// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : VeiculoRepository.cs                                            ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Veiculo. Gerencia operações CRUD     ║
// ║ e fornece listas para dropdowns. Usa ViewVeiculos para dados completos.      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetVeiculoListForDropDown() → SelectList simples com placas                ║
// ║ • GetVeiculoCompletoListForDropDown() → SelectList com dados completos       ║
// ║ • Update() → Atualização direta sem re-buscar do banco                       ║
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
    public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
        {
        private new readonly FrotiXDbContext _db;

        public VeiculoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetVeiculoListForDropDown()
            {
            return _db.Veiculo
            .OrderBy(o => o.Placa)
            .Select(i => new SelectListItem()
                {
                Text = i.Placa,
                Value = i.VeiculoId.ToString()
                }); ;
            }
        public IEnumerable<SelectListItem> GetVeiculoCompletoListForDropDown()
        {
            return _db.ViewVeiculos
                .Where(v => v.Status == true)
                .OrderBy(v => v.Placa)
                .Select(v => new SelectListItem()
                {
                    Text = v.VeiculoCompleto,
                    Value = v.VeiculoId.ToString()
                });
        }


        public new void Update(Veiculo veiculo)
            {
            // Atualiza diretamente sem buscar novamente do banco
            // (evita conflito de tracking no EF Core)
            _db.Update(veiculo);
            _db.SaveChanges();
            }


        }
    }


