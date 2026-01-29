// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViagemEstatisticaRepository.cs                                 ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViagemEstatistica, gerenciando estatísticas      ║
// ║ consolidadas de viagens para dashboards e relatórios.                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • ObterPorDataAsync() → Busca estatística por data específica                ║
// ║ • ObterPorPeriodoAsync() → Listagem por período                              ║
// ║ • ExisteParaDataAsync() → Verifica se existe registro para a data            ║
// ║ • RemoverEstatisticasAntigasAsync() → Limpeza de dados antigos               ║
// ║ • ObterEstatisticasDesatualizadasAsync() → Identifica registros desatualizados║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// Interface do repositório de ViagemEstatistica. Estende IRepository&lt;ViagemEstatistica&gt;.
    /// </summary>
    public interface IViagemEstatisticaRepository : IRepository<ViagemEstatistica>
    {
        Task<ViagemEstatistica> ObterPorDataAsync(DateTime dataReferencia);

        Task<List<ViagemEstatistica>> ObterPorPeriodoAsync(DateTime dataInicio , DateTime dataFim);

        Task<bool> ExisteParaDataAsync(DateTime dataReferencia);

        Task<int> RemoverEstatisticasAntigasAsync(int diasParaManter = 365);

        Task<List<ViagemEstatistica>> ObterEstatisticasDesatualizadasAsync();
    }
}
