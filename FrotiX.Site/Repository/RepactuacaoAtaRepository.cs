// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : RepactuacaoAtaRepository.cs                                     ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade RepactuacaoAta. Gerencia reajustes   ║
// ║ de preços em atas de registro de preços.                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetRepactuacaoAtaListForDropDown() → SelectList ordenada por descrição     ║
// ║ • Update() → Atualização da entidade RepactuacaoAta                          ║
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
    public class RepactuacaoAtaRepository : Repository<RepactuacaoAta>, IRepactuacaoAtaRepository
        {
        private new readonly FrotiXDbContext _db;

        public RepactuacaoAtaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetRepactuacaoAtaListForDropDown()
            {
            return _db.RepactuacaoAta
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Descricao,
                    Value = i.RepactuacaoAtaId.ToString()
                    });
            }

        public new void Update(RepactuacaoAta repactuacaoitemveiculoata)
            {
            var objFromDb = _db.RepactuacaoAta.FirstOrDefault(s => s.RepactuacaoAtaId == repactuacaoitemveiculoata.RepactuacaoAtaId);

            _db.Update(repactuacaoitemveiculoata);
            _db.SaveChanges();

            }


        }
    }


