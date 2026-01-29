// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : RepactuacaoVeiculoRepository.cs                                 ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para repactuações de valores de locação de veículos.             ║
// ║ Gerencia reajustes individuais por veículo dentro do contrato de locação.    ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetRepactuacaoVeiculoListForDropDown() → Lista repactuações por valor      ║
// ║ • Update() → Atualiza valor, observação e vínculos da repactuação            ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
{
    public class RepactuacaoVeiculoRepository : Repository<RepactuacaoVeiculo>, IRepactuacaoVeiculoRepository
    {
        private new readonly FrotiXDbContext _db;

        public RepactuacaoVeiculoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetRepactuacaoVeiculoListForDropDown()
        {
            return _db.RepactuacaoVeiculo
                .OrderBy(o => o.RepactuacaoVeiculoId)
                .Select(i => new SelectListItem()
                {
                    Text = i.Valor.ToString(),
                    Value = i.RepactuacaoVeiculoId.ToString()
                });
        }

        public new void Update(RepactuacaoVeiculo repactuacaoVeiculo)
        {
            var objFromDb = _db.RepactuacaoVeiculo.FirstOrDefault(s =>
                s.RepactuacaoVeiculoId == repactuacaoVeiculo.RepactuacaoVeiculoId
            );

            if (objFromDb != null)
            {
                objFromDb.Valor = repactuacaoVeiculo.Valor;
                objFromDb.Observacao = repactuacaoVeiculo.Observacao;
                objFromDb.VeiculoId = repactuacaoVeiculo.VeiculoId;
                objFromDb.RepactuacaoContratoId = repactuacaoVeiculo.RepactuacaoContratoId;
            }

            _db.SaveChanges();
        }
    }
}
