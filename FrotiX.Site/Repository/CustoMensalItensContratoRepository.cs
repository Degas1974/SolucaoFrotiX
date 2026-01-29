// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : CustoMensalItensContratoRepository.cs                           ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para custos mensais de itens de contrato.                        ║
// ║ Consolida valores por nota fiscal, mês e ano para controle orçamentário.     ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetCustoMensalItensContratoListForDropDown() → Lista para dropdowns        ║
// ║ • Update() → Atualiza custo mensal de item de contrato                       ║
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
    public class CustoMensalItensContratoRepository : Repository<CustoMensalItensContrato>, ICustoMensalItensContratoRepository
        {
        private new readonly FrotiXDbContext _db;

        public CustoMensalItensContratoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetCustoMensalItensContratoListForDropDown()
            {
            return _db.CustoMensalItensContrato
                .OrderBy(o => o.Ano)
                .Select(i => new SelectListItem()
                    {
                    Text = i.Ano.ToString(),
                    Value = i.NotaFiscalId.ToString()
                    });
            }

        public new void Update(CustoMensalItensContrato customensalitenscontrato)
            {
            var objFromDb = _db.CustoMensalItensContrato.FirstOrDefault(s => (s.NotaFiscalId == customensalitenscontrato.NotaFiscalId) && (s.Ano == customensalitenscontrato.Ano) && (s.Mes == customensalitenscontrato.Mes));

            _db.Update(customensalitenscontrato);
            _db.SaveChanges();

            }


        }
    }


