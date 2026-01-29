// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : EncarregadoContratoRepository.cs                                ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório para vínculo entre encarregados e contratos terceirizados.       ║
// ║ Gerencia a associação de encarregados aos contratos de prestação de serviço. ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ PRINCIPAIS MÉTODOS                                                           ║
// ║ • Update() → Atualiza vínculo encarregado-contrato                           ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
