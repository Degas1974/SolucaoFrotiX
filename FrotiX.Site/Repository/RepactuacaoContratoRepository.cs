// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : RepactuacaoContratoRepository.cs                                ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade RepactuacaoContrato. Gerencia        ║
// ║ repactuações/aditivos em contratos administrativos.                          ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetRepactuacaoContratoListForDropDown() → SelectList ordenada             ║
// ║ • Update() → Atualização da entidade RepactuacaoContrato                     ║
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
    public class RepactuacaoContratoRepository : Repository<RepactuacaoContrato>, IRepactuacaoContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        public RepactuacaoContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetRepactuacaoContratoListForDropDown()
            {
            return _db.RepactuacaoContrato
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Descricao,
                    Value = i.RepactuacaoContratoId.ToString()
                    });
            }

        public new void Update(RepactuacaoContrato RepactuacaoContrato)
            {
            var objFromDb = _db.RepactuacaoContrato.FirstOrDefault(s => s.RepactuacaoContratoId == RepactuacaoContrato.RepactuacaoContratoId);

            _db.Update(RepactuacaoContrato);
            _db.SaveChanges();

            }


        }
    }


