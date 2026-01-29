/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: AtaRegistroPrecosRepository.cs                                                          ║
    ║ 📂 CAMINHO: /Repository                                                                             ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Repositório de AtaRegistroPrecos (atas de registro e seleção em dropdown).           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 MÉTODOS: GetAtaListForDropDown(status), Update()                                                 ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Data, Repository<T>, SelectListItem                                                 ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
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


