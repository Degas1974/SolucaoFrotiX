// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ � DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : EncarregadoRepository.cs                                        ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para entidade Encarregado. Gerencia encarregados   ║
// ║ responsáveis pela supervisão das operações de frota.                           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • Update() → Atualização da entidade Encarregado                             ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
{
    public class EncarregadoRepository : Repository<Encarregado>, IEncarregadoRepository
    {
        private new readonly FrotiXDbContext _db;

        public EncarregadoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        public new void Update(Encarregado encarregado)
        {
            _db.Encarregado.Update(encarregado);
            _db.SaveChanges();
        }
    }
}
