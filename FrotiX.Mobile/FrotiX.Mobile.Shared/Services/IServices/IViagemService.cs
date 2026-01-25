using FrotiX.Mobile.Shared.Models;

namespace FrotiX.Mobile.Shared.Services.IServices
{
    public interface IViagemService
    {
        Task<List<VeiculoViewModel>> ObterVeiculoCompletoDropdownAsync();

        Task<List<MotoristaViewModel>> ObterMotoristasAsync();

        Task<List<ViewViagemVistoria>> ObterTodasVistoriasAsync();

        Task<List<ViewViagemVistoria>> ObterVistoriasAbertasAsync();

        Task<bool> SalvarVistoria(Viagem viagem);

        Task<bool> AtualizarVistoriaAsync(Viagem viagem);

        Task<bool> ExcluirVistoriaAsync(Guid id);

        Task<Viagem?> ObterVistoriaPorIdAsync(Guid id);

        Task<List<SetorSolicitante>> ObterSetoresSolicitantesAsync();

        Task<List<Requisitante>> ObterRequisitantesAsync();

        Task<List<Origem>> ObterOrigensAsync();

        Task<List<Destino>> ObterDestinosAsync();

        Task<string?> ObterNomeUsuarioAsync(string usuarioId);
    }
}