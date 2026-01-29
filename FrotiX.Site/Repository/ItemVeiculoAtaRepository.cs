// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ItemVeiculoAtaRepository.cs                                     ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade ItemVeiculoAta. Gerencia itens de    ║
// ║ veículos nas atas de registro de preços (categorias, especificações).         ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetItemVeiculoAtaListForDropDown() → SelectList ordenada por descrição     ║
// ║ • Update() → Atualização da entidade ItemVeiculoAta                          ║
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
    public class ItemVeiculoAtaRepository : Repository<ItemVeiculoAta>, IItemVeiculoAtaRepository
        {
        private new readonly FrotiXDbContext _db;

        public ItemVeiculoAtaRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetItemVeiculoAtaListForDropDown()
            {
            return _db.ItemVeiculoAta
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Descricao,
                    Value = i.ItemVeiculoAtaId.ToString()
                    });
            }

        public new void Update(ItemVeiculoAta itemveiculoata)
            {
            var objFromDb = _db.ItemVeiculoAta.FirstOrDefault(s => s.ItemVeiculoAtaId == itemveiculoata.ItemVeiculoAtaId);

            _db.Update(itemveiculoata);
            _db.SaveChanges();

            }


        }
    }


