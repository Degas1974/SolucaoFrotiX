// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : AtaRegistroPrecosRepository.cs                                  ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade AtaRegistroPrecos. Gerencia atas de  ║
// ║ registro de preços para locação de veículos e serviços.                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetAtaListForDropDown(status) → SelectList "Ano/Numero - Fornecedor"       ║
// ║ • Update() → Atualização da entidade AtaRegistroPrecos                       ║
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
    public class AtaRegistroPrecosRepository : Repository<AtaRegistroPrecos>, IAtaRegistroPrecosRepository
        {
        private new readonly FrotiXDbContext _db;

        public AtaRegistroPrecosRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetAtaListForDropDown(int status)
            {
            return _db.AtaRegistroPrecos
            .Where(s => s.Status == Convert.ToBoolean(status))
            .Join(_db.Fornecedor, ataregistroprecos => ataregistroprecos.FornecedorId, fornecedor => fornecedor.FornecedorId, (ataregistroprecos, fornecedor) => new { ataregistroprecos, fornecedor })
            .OrderByDescending(o => o.ataregistroprecos.AnoAta + "/" + o.ataregistroprecos.NumeroAta + " - " + o.fornecedor.DescricaoFornecedor)
            .Select(i => new SelectListItem()
                {
                Text = i.ataregistroprecos.AnoAta + "/" + i.ataregistroprecos.NumeroAta + " - " + i.fornecedor.DescricaoFornecedor,
                Value = i.ataregistroprecos.AtaId.ToString()
                });
            }

        public new void Update(AtaRegistroPrecos ataRegistroPrecos)
            {
            var objFromDb = _db.AtaRegistroPrecos.FirstOrDefault(s => s.AtaId == ataRegistroPrecos.AtaId);

            _db.Update(ataRegistroPrecos);
            _db.SaveChanges();

            }


        }
    }


