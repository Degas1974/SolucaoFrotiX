/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EncarregadoContratoRepository.cs                                                                   ║
   ║ 📂 CAMINHO: FrotiX.Site/Repository/                                                                            ║
   ║ 🎯 OBJETIVO: Repositório para gerenciar vínculo entre encarregados e contratos de prestação de serviço        ║
   ║ 📋 MÉTODOS:                                                                                                    ║
   ║    • Update() → Atualiza associação encarregado-contrato terceirizado                                         ║
   ║ 🔗 DEPS: Repository<EncarregadoContrato>, IEncarregadoContratoRepository, FrotiXDbContext                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📅 Atualizado: 29/01/2026  |  👤 Team: FrotiX Development  |  📝 Versão: 2.0                                  ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository
{
    public class EncarregadoContratoRepository : Repository<EncarregadoContrato>, IEncarregadoContratoRepository
    {
        private new readonly FrotiXDbContext _db;

        public EncarregadoContratoRepository(FrotiXDbContext db) : base(db)
        {
            _db = db;
        }

        public new void Update(EncarregadoContrato encarregadoContrato)
        {
            _db.EncarregadoContrato.Update(encarregadoContrato);
            _db.SaveChanges();
        }
    }
}
