// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : RepactuacaoServicosRepository.cs                                ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para repactuações de serviços em contratos.                      ║
// ║ Gerencia reajustes de valores de serviços prestados (manutenção, lavagem).   ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetRepactuacaoServicosListForDropDown() → Lista repactuações por valor     ║
// ║ • Update() → Atualiza registro de repactuação de serviço                     ║
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
    public class RepactuacaoServicosRepository : Repository<RepactuacaoServicos>, IRepactuacaoServicosRepository
        {
        private new readonly FrotiXDbContext _db;

        public RepactuacaoServicosRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetRepactuacaoServicosListForDropDown()
            {
            return _db.RepactuacaoServicos
                .Select(i => new SelectListItem()
                    {
                    Text = i.Valor.ToString(),
                    Value = i.RepactuacaoContratoId.ToString()
                    });
            }

        public new void Update(RepactuacaoServicos RepactuacaoServicos)
            {
            var objFromDb = _db.RepactuacaoServicos.FirstOrDefault(s => s.RepactuacaoServicoId == RepactuacaoServicos.RepactuacaoServicoId);

            _db.Update(RepactuacaoServicos);
            _db.SaveChanges();

            }


        }
    }


