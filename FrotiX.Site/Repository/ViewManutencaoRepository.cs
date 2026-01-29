// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ViewManutencaoRepository.cs                                     ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para SQL View ViewManutencao. Fornece visão       ║
// ║ consolidada de manutenções com dados de veículo, fornecedor, contrato.        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetViewManutencaoListForDropDown() → SelectList ordenada por data          ║
// ║ • Update() → Atualização (não aplicável a Views, apenas compat.)              ║
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
    public class ViewManutencaoRepository : Repository<ViewManutencao>, IViewManutencaoRepository
        {
        private new readonly FrotiXDbContext _db;

        public ViewManutencaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetViewManutencaoListForDropDown()
            {
            return _db.ViewManutencao
            .OrderBy(o => o.DataSolicitacao)
            .Select(i => new SelectListItem()
                {
                Text = i.DataSolicitacao.ToString(),
                Value = i.ManutencaoId.ToString()
                }); ; ;
            }

        public new void Update(ViewManutencao viewManutencao)
            {
            var objFromDb = _db.ViewManutencao.FirstOrDefault(s => s.ManutencaoId == viewManutencao.ManutencaoId);

            _db.Update(viewManutencao);
            _db.SaveChanges();

            }


        }
    }


