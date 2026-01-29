// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : AbastecimentoRepository.cs                                      ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Abastecimento. Gerencia operações    ║
// ║ CRUD de registros de abastecimento de combustível da frota.                  ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetAbastecimentoListForDropDown() → SelectList com litros                  ║
// ║ • Update() → Atualização da entidade Abastecimento                           ║
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
    public class AbastecimentoRepository : Repository<Abastecimento>, IAbastecimentoRepository
        {
        private new readonly FrotiXDbContext _db;

        public AbastecimentoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetAbastecimentoListForDropDown()
            {
            return _db.Abastecimento
            .Select(i => new SelectListItem()
                {
                Text = i.Litros.ToString(),
                Value = i.AbastecimentoId.ToString()
                }); ;
            }

        public new void Update(Abastecimento abastecimento)
            {
            var objFromDb = _db.Abastecimento.FirstOrDefault(s => s.AbastecimentoId == abastecimento.AbastecimentoId);

            _db.Update(abastecimento);
            _db.SaveChanges();

            }


        }
    }


