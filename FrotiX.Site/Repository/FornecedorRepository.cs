// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : FornecedorRepository.cs                                         ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Fornecedor. Gerencia fornecedores    ║
// ║ de contratos (combustível, veículos, manutenção, etc).                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetFornecedorListForDropDown() → SelectList apenas ativos (Status=true)    ║
// ║ • Update() → Atualização da entidade Fornecedor                              ║
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
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
        {
        private new readonly FrotiXDbContext _db;

        public FornecedorRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetFornecedorListForDropDown()
            {

            return _db.Fornecedor
            .Where(f => f.Status == true)
            .OrderBy(o => o.DescricaoFornecedor)
            .Select(i => new SelectListItem()
                {
                Text = i.DescricaoFornecedor,
                Value = i.FornecedorId.ToString()
                }); ;
            }

        public new void Update(Fornecedor fornecedor)
            {
            var objFromDb = _db.Fornecedor.FirstOrDefault(s => s.FornecedorId == fornecedor.FornecedorId);

            _db.Update(fornecedor);
            _db.SaveChanges();

            }


        }
    }


