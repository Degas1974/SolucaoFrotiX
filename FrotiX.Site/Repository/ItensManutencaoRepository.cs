// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ItensManutencaoRepository.cs                                    ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para itens de manutenção de veículos.                            ║
// ║ Controla peças, serviços e insumos utilizados em cada ordem de serviço.      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetItensManutencaoListForDropDown() → Lista itens ordenados por data       ║
// ║ • Update() → Atualiza registro de item de manutenção                         ║
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
    public class ItensManutencaoRepository : Repository<ItensManutencao>, IItensManutencaoRepository
        {
        private new readonly FrotiXDbContext _db;

        public ItensManutencaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetItensManutencaoListForDropDown()
            {
            return _db.ItensManutencao
                .OrderBy(o => o.DataItem)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Resumo,
                    Value = i.ItemManutencaoId.ToString()
                    });
            }

        public new void Update(ItensManutencao itensManutencao)
            {
            var objFromDb = _db.ItensManutencao.FirstOrDefault(s => s.ItemManutencaoId == itensManutencao.ItemManutencaoId);

            _db.Update(itensManutencao);
            _db.SaveChanges();

            }


        }
    }


