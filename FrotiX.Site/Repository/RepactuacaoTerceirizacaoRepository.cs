// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : RepactuacaoTerceirizacaoRepository.cs                           ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para repactuações de valores de terceirização.                   ║
// ║ Gerencia reajustes de valores de motoristas, operadores e encarregados.      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetRepactuacaoTerceirizacaoListForDropDown() → Lista por valor encarregado ║
// ║ • Update() → Atualiza registro de repactuação de terceirização               ║
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
    public class RepactuacaoTerceirizacaoRepository : Repository<RepactuacaoTerceirizacao>, IRepactuacaoTerceirizacaoRepository
        {
        private new readonly FrotiXDbContext _db;

        public RepactuacaoTerceirizacaoRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetRepactuacaoTerceirizacaoListForDropDown()
            {
            return _db.RepactuacaoTerceirizacao
                .Select(i => new SelectListItem()
                    {
                    Text = i.ValorEncarregado.ToString(),
                    Value = i.RepactuacaoContratoId.ToString()
                    });
            }

        public new void Update(RepactuacaoTerceirizacao RepactuacaoTerceirizacao)
            {
            var objFromDb = _db.RepactuacaoTerceirizacao.FirstOrDefault(s => s.RepactuacaoTerceirizacaoId == RepactuacaoTerceirizacao.RepactuacaoTerceirizacaoId);

            _db.Update(RepactuacaoTerceirizacao);
            _db.SaveChanges();

            }


        }
    }


