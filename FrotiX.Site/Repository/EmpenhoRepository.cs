// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : EmpenhoRepository.cs                                            ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Empenho. Gerencia notas de empenho   ║
// ║ orçamentário vinculadas a contratos.                                          ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetEmpenhoListForDropDown() → SelectList com JOIN Contrato                 ║
// ║   - Formato: "NotaEmpenho (Ano/NumeroContrato)"                              ║
// ║ • Update() → Atualização da entidade Empenho                                 ║
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
    public class EmpenhoRepository : Repository<Empenho>, IEmpenhoRepository
        {
        private new readonly FrotiXDbContext _db;

        public EmpenhoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetEmpenhoListForDropDown()
            {
            return _db.Empenho
            .Join(_db.Contrato, empenho => empenho.ContratoId, contrato => contrato.ContratoId, (empenho, contrato) => new { empenho, contrato })
            .OrderBy(o => o.empenho.NotaEmpenho)
            .Select(i => new SelectListItem()
                {
                Text = i.empenho.NotaEmpenho + "(" + i.contrato.AnoContrato + "/" + i.contrato.NumeroContrato + ")",
                Value = i.contrato.ContratoId.ToString()
                });
            }

        public new void Update(Empenho empenho)
            {
            var objFromDb = _db.Empenho.FirstOrDefault(s => s.EmpenhoId == empenho.EmpenhoId);

            _db.Update(empenho);
            _db.SaveChanges();

            }
        }
    }


