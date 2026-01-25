using FrotiX.Mobile.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrotiX.Mobile.Shared.Services.IServices
{
    public interface IViagensEconomildoService
    {
        // Operações de armazenamento local
        Task<bool> SalvarViagemLocalAsync(ViagemEconomildoLocal viagem);
        Task<bool> ExcluirViagemLocalAsync(Guid viagemId);
        Task<List<ViagemEconomildoLocal>> ObterViagensLocaisAsync();
        Task<List<ViagemEconomildoLocal>> ObterViagensNaoTransmitidasAsync();
        Task<bool> MarcarViagemComoTransmitidaAsync(Guid viagemId);
        Task<bool> LimparViagensTransmitidasAsync();
        Task<int> ContarViagensNaoTransmitidasAsync();

        // Operações de transmissão para o servidor
        Task<(bool sucesso, string mensagem)> TransmitirViagensAsync(List<ViagemEconomildoLocal> viagens);
        Task<(bool sucesso, string mensagem)> TransmitirTodasViagensAsync();
    }
}
