// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : RegistroCupomAbastecimentoRepository.cs                         ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para cupons de abastecimento digitalizados.                      ║
// ║ Armazena referências a arquivos PDF dos cupons para auditoria.               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetRegistroCupomAbastecimentoListForDropDown() → Lista cupons por data     ║
// ║ • Update() → Atualiza registro de cupom digitalizado                         ║
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
    public class RegistroCupomAbastecimentoRepository : Repository<RegistroCupomAbastecimento>, IRegistroCupomAbastecimentoRepository
        {
        private new readonly FrotiXDbContext _db;

        public RegistroCupomAbastecimentoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetRegistroCupomAbastecimentoListForDropDown()
            {
            return _db.RegistroCupomAbastecimento
                .OrderBy(o => o.DataRegistro)
                .Select(i => new SelectListItem()
                    {
                    Text = i.RegistroPDF,
                    Value = i.RegistroCupomId.ToString()
                    });
            }

        public new void Update(RegistroCupomAbastecimento registroCupomAbastecimento)
            {
            var objFromDb = _db.RegistroCupomAbastecimento.FirstOrDefault(s => s.RegistroCupomId == registroCupomAbastecimento.RegistroCupomId);

            _db.Update(registroCupomAbastecimento);
            _db.SaveChanges();

            }


        }
    }


