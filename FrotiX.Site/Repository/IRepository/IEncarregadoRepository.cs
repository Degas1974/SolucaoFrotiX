// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IEncarregadoRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Encarregado, gerenciando funcionários           ║
// ║ responsáveis pela supervisão de contratos de frota.                          ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • Update() → Atualização de encarregado (new para esconder base)             ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Models;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// Interface do repositório de Encarregado. Estende IRepository&lt;Encarregado&gt;.
    /// </summary>
    public interface IEncarregadoRepository : IRepository<Encarregado>
    {
        new void Update(Encarregado encarregado);
    }
}
