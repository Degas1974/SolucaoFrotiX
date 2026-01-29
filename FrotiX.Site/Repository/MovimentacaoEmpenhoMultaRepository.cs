// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : MovimentacaoEmpenhoMultaRepository.cs                           ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para movimentações financeiras de empenhos de multas.            ║
// ║ Registra débitos/créditos nas notas de empenho de multas de trânsito.        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetMovimentacaoEmpenhoMultaListForDropDown() → Lista movimentações         ║
// ║ • Update() → Atualiza registro de movimentação de empenho de multa           ║
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
    public class MovimentacaoEmpenhoMultaRepository : Repository<MovimentacaoEmpenhoMulta>, IMovimentacaoEmpenhoMultaRepository
        {
        private new readonly FrotiXDbContext _db;

        public MovimentacaoEmpenhoMultaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetMovimentacaoEmpenhoMultaListForDropDown()
            {
            return _db.MovimentacaoEmpenhoMulta
            .OrderBy(o => o.DataMovimentacao)
            .Select(i => new SelectListItem()
                {
                Text = i.DataMovimentacao + "(" + i.Valor + ")",
                Value = i.MovimentacaoId.ToString()
                });
            }

        public new void Update(MovimentacaoEmpenhoMulta movimentacaoempenhomulta)
            {
            var objFromDb = _db.MovimentacaoEmpenhoMulta.FirstOrDefault(s => s.MovimentacaoId == movimentacaoempenhomulta.MovimentacaoId);

            _db.Update(movimentacaoempenhomulta);
            _db.SaveChanges();

            }
        }
    }


