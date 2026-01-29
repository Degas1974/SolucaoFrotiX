// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ItemVeiculoContratoRepository.cs                                ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para itens de veículos em contratos de locação.                  ║
// ║ Gerencia a descrição e valores dos itens de veículos contratados.            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetItemVeiculoContratoListForDropDown() → Lista itens para seleção         ║
// ║ • Update() → Atualiza item de veículo em contrato                            ║
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
    public class ItemVeiculoContratoRepository : Repository<ItemVeiculoContrato>, IItemVeiculoContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        public ItemVeiculoContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetItemVeiculoContratoListForDropDown()
            {
            return _db.ItemVeiculoContrato
                .OrderBy(o => o.Descricao)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Descricao,
                    Value = i.ItemVeiculoId.ToString()
                    });
            }

        public new void Update(ItemVeiculoContrato itemveiculocontrato)
            {
            var objFromDb = _db.ItemVeiculoContrato.FirstOrDefault(s => s.ItemVeiculoId == itemveiculocontrato.ItemVeiculoId);

            _db.Update(itemveiculocontrato);
            _db.SaveChanges();

            }


        }
    }


