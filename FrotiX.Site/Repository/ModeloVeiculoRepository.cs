// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ModeloVeiculoRepository.cs                                      ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade ModeloVeiculo. Gerencia modelos de   ║
// ║ veículos (Uno, Gol, Onix, etc).                                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetModeloVeiculoListForDropDown() → SelectList ordenada por descrição      ║
// ║ • Update() → Atualização da entidade ModeloVeiculo                           ║
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
    public class ModeloVeiculoRepository : Repository<ModeloVeiculo>, IModeloVeiculoRepository
        {
        private new readonly FrotiXDbContext _db;

        public ModeloVeiculoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetModeloVeiculoListForDropDown()
            {
            return _db.ModeloVeiculo
            .OrderBy(o => o.DescricaoModelo)
            .Select(i => new SelectListItem()
                {
                Text = i.DescricaoModelo,
                Value = i.ModeloId.ToString()
                }); ;
            }

        public new void Update(ModeloVeiculo modeloVeiculo)
            {
            var objFromDb = _db.ModeloVeiculo.FirstOrDefault(s => s.ModeloId == modeloVeiculo.ModeloId);

            _db.Update(modeloVeiculo);
            _db.SaveChanges();

            }


        }
    }


