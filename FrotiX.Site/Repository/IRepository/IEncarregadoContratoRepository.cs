// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IEncarregadoContratoRepository.cs                               ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de EncarregadoContrato, gerenciando associação      ║
// ║ MxN entre encarregados e contratos de frota.                                 ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • Update() → Atualização de encarregado-contrato (new)                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Models;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// Interface do repositório de EncarregadoContrato. Estende IRepository&lt;EncarregadoContrato&gt;.
    /// </summary>
    public interface IEncarregadoContratoRepository : IRepository<EncarregadoContrato>
    {
        new void Update(EncarregadoContrato encarregadoContrato);
    }
}
