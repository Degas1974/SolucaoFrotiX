// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : LavadorContratoRepository.cs                                    ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para vínculo entre lavadores e contratos terceirizados.          ║
// ║ Gerencia a associação de lavadores aos contratos de lavagem de veículos.     ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetLavadorContratoListForDropDown() → Lista vínculos para dropdown         ║
// ║ • Update() → Atualiza vínculo lavador-contrato (chave composta)              ║
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
    public class LavadorContratoRepository : Repository<LavadorContrato>, ILavadorContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        public LavadorContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetLavadorContratoListForDropDown()
            {
            return _db.LavadorContrato.Select(i => new SelectListItem()
                {
                //Text = i.Placa,
                //Value = i.VeiculoId.ToString()
                }); ;
            }

        public new void Update(LavadorContrato lavadorContrato)
            {
            var objFromDb = _db.LavadorContrato.FirstOrDefault(s => (s.LavadorId == lavadorContrato.LavadorId) && (s.ContratoId == lavadorContrato.ContratoId));

            _db.Update(lavadorContrato);
            _db.SaveChanges();

            }


        }
    }


