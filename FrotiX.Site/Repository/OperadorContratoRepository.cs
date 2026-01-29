// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : OperadorContratoRepository.cs                                   ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para vínculo entre operadores e contratos de abastecimento.      ║
// ║ Gerencia a associação de operadores aos contratos de postos de combustível.  ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetOperadorContratoListForDropDown() → Lista vínculos para dropdown        ║
// ║ • Update() → Atualiza vínculo operador-contrato (chave composta)             ║
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
    public class OperadorContratoRepository : Repository<OperadorContrato>, IOperadorContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        public OperadorContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetOperadorContratoListForDropDown()
            {
            return _db.OperadorContrato.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        public new void Update(OperadorContrato operadorContrato)
            {
            var objFromDb = _db.OperadorContrato.FirstOrDefault(s => (s.OperadorId == operadorContrato.OperadorId) && (s.ContratoId == operadorContrato.ContratoId));

            _db.Update(operadorContrato);
            _db.SaveChanges();

            }


        }
    }


