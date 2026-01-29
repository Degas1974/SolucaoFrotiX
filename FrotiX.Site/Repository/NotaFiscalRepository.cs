// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : NotaFiscalRepository.cs                                         ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade NotaFiscal. Gerencia notas fiscais   ║
// ║ de compras de peças, serviços e combustível.                                   ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetNotaFiscalListForDropDown() → SelectList ordenada por número NF         ║
// ║ • Update() → Atualização da entidade NotaFiscal                              ║
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
    public class NotaFiscalRepository : Repository<NotaFiscal>, INotaFiscalRepository
        {
        private new readonly FrotiXDbContext _db;

        public NotaFiscalRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetNotaFiscalListForDropDown()
            {
            return _db.NotaFiscal
            .OrderBy(o => o.NumeroNF)
            .Select(i => new SelectListItem()
                {
                Text = i.NumeroNF.ToString(),
                Value = i.NotaFiscalId.ToString()
                }); ;
            }

        public new void Update(NotaFiscal notaFiscal)
            {
            var objFromDb = _db.NotaFiscal.FirstOrDefault(s => s.NotaFiscalId == notaFiscal.NotaFiscalId);

            _db.Update(notaFiscal);
            _db.SaveChanges();

            }
        }
    }


