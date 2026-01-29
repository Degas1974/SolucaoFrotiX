// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : LavadoresLavagemRepository.cs                                   ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para vínculo entre lavadores e lavagens realizadas (N:N).        ║
// ║ Registra quais lavadores participaram de cada serviço de lavagem.            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • GetLavadoresLavagemListForDropDown() → Lista associações lavador-lavagem   ║
// ║ • Update() → Atualiza registro de participação de lavador                    ║
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
    public class LavadoresLavagemRepository : Repository<LavadoresLavagem>, ILavadoresLavagemRepository
        {
        private new readonly FrotiXDbContext _db;

        public LavadoresLavagemRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        public IEnumerable<SelectListItem> GetLavadoresLavagemListForDropDown()
            {
            return _db.LavadoresLavagem
            .OrderBy(o => o.LavagemId)
            .Select(i => new SelectListItem()
                {
                Text = i.LavadorId.ToString(),
                Value = i.LavagemId.ToString()
                }); ; ;
            }

        public new void Update(LavadoresLavagem lavadoresLavagem)
            {
            var objFromDb = _db.LavadoresLavagem.FirstOrDefault(s => s.LavagemId == lavadoresLavagem.LavagemId);

            _db.Update(lavadoresLavagem);
            _db.SaveChanges();

            }


        }
    }


