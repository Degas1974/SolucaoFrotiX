/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EncarregadoRepository.cs                                                                           ║
   ║ 📂 CAMINHO: FrotiX.Site/Repository/                                                                            ║
   ║ 🎯 OBJETIVO: Repositório especializado para gerenciar encarregados responsáveis pela supervisão de frota      ║
   ║ 📋 MÉTODOS:                                                                                                    ║
   ║    • Update() → Atualização da entidade Encarregado                                                           ║
   ║ 🔗 DEPS: Repository<Encarregado>, IEncarregadoRepository, FrotiXDbContext                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📅 Atualizado: 29/01/2026  |  👤 Team: FrotiX Development  |  📝 Versão: 2.0                                  ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
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
