/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: IEncarregadoContratoRepository.cs                                                                   ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de EncarregadoContrato, gerenciando associação MxN entre encarregados e   ║
║              contratos de frota.                                                                               ║
║  📋 MÉTODOS ADICIONAIS:                                                                                          ║
║     • Update() → Atualização de encarregado-contrato (new)                                                      ║
║  🔗 DEPENDÊNCIAS: IRepository<EncarregadoContrato>                                                              ║
║  📅 Atualizado: 29/01/2026    👤 Team: FrotiX    📝 Versão: 2.0                                                 ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
using FrotiX.Models;

namespace FrotiX.Repository.IRepository
{
    
    // Interface do repositório de EncarregadoContrato. Estende IRepository&lt;EncarregadoContrato&gt;.
    
    public interface IEncarregadoContratoRepository : IRepository<EncarregadoContrato>
    {
        new void Update(EncarregadoContrato encarregadoContrato);
    }
}
