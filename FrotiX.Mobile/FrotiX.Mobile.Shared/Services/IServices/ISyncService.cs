using FrotiX.Mobile.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrotiX.Mobile.Shared.Services.IServices
{
    public interface ISyncService
    {
        // Sincronização de Veículos
        Task<(bool sucesso, string mensagem)> SincronizarVeiculosAsync();
        Task<List<VeiculoLocalDto>> ObterVeiculosLocaisAsync();
        Task<bool> LimparVeiculosLocaisAsync();
        Task<int> ContarVeiculosLocaisAsync();
        Task<DateTime?> ObterDataUltimaSyncVeiculosAsync();

        // Sincronização de Motoristas
        Task<(bool sucesso, string mensagem)> SincronizarMotoristasAsync();
        Task<(bool sucesso, string mensagem)> SincronizarMotoristasComFotosAsync();
        Task<List<MotoristaViewModel>> ObterMotoristasLocaisAsync(); // CORRIGIDO: MotoristaViewModel
        Task<bool> LimparMotoristasLocaisAsync();
        Task<int> ContarMotoristasLocaisAsync();
        Task<DateTime?> ObterDataUltimaSyncMotoristasAsync();

        // Sincronização Completa
        Task<(bool sucesso, string mensagem)> SincronizarTodosOsDadosAsync();
        Task<bool> LimparTodosDadosLocaisAsync();

        // Verificação de Status
        Task<SyncStatusViewModel> ObterStatusSincronizacaoAsync();
    }
}
