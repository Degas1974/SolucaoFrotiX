using FrotiX.Mobile.Shared.Models;
using FrotiX.Mobile.Shared.Services.IServices;
using System.Diagnostics;

namespace FrotiX.Mobile.Shared.Services
{
    public class ViagemService : IViagemService
    {
        private readonly RelayApiService _api;
        private readonly IAlertaService _alerta;
        private readonly ILogService? _logger;

        public ViagemService(RelayApiService api, IAlertaService alerta, ILogService? logger = null)
        {
            _api = api;
            _alerta = alerta;
            _logger = logger;
        }

        private void LogInfo(string message)
        {
            Debug.WriteLine($"[ViagemService] {message}");
            _logger?.Info($"[ViagemService] {message}");
        }

        private void LogError(string message, Exception? ex = null)
        {
            Debug.WriteLine($"[ViagemService] ERRO: {message}");
            if (ex != null)
            {
                Debug.WriteLine($"[ViagemService] Exception: {ex.Message}");
                Debug.WriteLine($"[ViagemService] StackTrace: {ex.StackTrace}");
            }
            _logger?.Error($"[ViagemService] {message}", ex);
        }

        public async Task<List<VeiculoViewModel>> ObterVeiculoCompletoDropdownAsync()
        {
            try
            {
                var response = await _api.GetAsync<List<VeiculoViewModel>>("/api/veiculos/dropdown");
                return response ?? new List<VeiculoViewModel>();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("Erro ao carregar veículos" , ex.Message);
                return new List<VeiculoViewModel>();
            }
        }

        public async Task<List<MotoristaViewModel>> ObterMotoristasAsync()
        {
            try
            {
                var response = await _api.GetAsync<List<MotoristaViewModel>>("/api/motoristas/dropdown");
                return response ?? new List<MotoristaViewModel>();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("Erro ao carregar motoristas" , ex.Message);
                return new List<MotoristaViewModel>();
            }
        }

        public async Task<List<ViewViagemVistoria>> ObterTodasVistoriasAsync()
        {
            try
            {
                var response = await _api.GetAsync<List<ViewViagemVistoria>>("/api/vistorias");
                return response ?? new List<ViewViagemVistoria>();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("Erro ao carregar vistorias" , ex.Message);
                return new List<ViewViagemVistoria>();
            }
        }

        public async Task<List<ViewViagemVistoria>> ObterVistoriasAbertasAsync()
        {
            try
            {
                var response = await _api.GetAsync<List<ViewViagemVistoria>>("/api/vistorias/abertas");
                return response ?? new List<ViewViagemVistoria>();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("Erro ao carregar vistorias abertas" , ex.Message);
                return new List<ViewViagemVistoria>();
            }
        }

        public async Task<bool> SalvarVistoria(Viagem viagem)
        {
            try
            {
                // Garante que tem ID
                if (viagem.ViagemId == Guid.Empty)
                {
                    viagem.ViagemId = Guid.NewGuid();
                    Debug.WriteLine($"[ViagemService] Gerado novo ViagemId: {viagem.ViagemId}");
                }

                viagem.Status = viagem.DataFinal == null ? "Aberta" : "Realizada";

                // Log dos dados principais para debug
                Debug.WriteLine($"[ViagemService] Salvando vistoria:");
                Debug.WriteLine($"  - ViagemId: {viagem.ViagemId}");
                Debug.WriteLine($"  - VeiculoId: {viagem.VeiculoId}");
                Debug.WriteLine($"  - MotoristaId: {viagem.MotoristaId}");
                Debug.WriteLine($"  - DataInicial: {viagem.DataInicial}");
                Debug.WriteLine($"  - Status: {viagem.Status}");

                // Limpa referências para evitar problemas de serialização
                viagem.Requisitante = null;
                viagem.SetorSolicitante = null;
                viagem.Veiculo = null;
                viagem.Motorista = null;

                var resultado = await _api.PostAsync("/api/vistorias" , viagem);

                Debug.WriteLine($"[ViagemService] Resultado API: {resultado}");

                if (!resultado)
                {
                    _ = _alerta.Erro("Erro" , "Servidor retornou erro ao salvar vistoria");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ViagemService] ERRO: {ex.Message}");
                Debug.WriteLine($"[ViagemService] StackTrace: {ex.StackTrace}");
                _ = _alerta.Erro("Erro ao salvar vistoria" , ex.Message);
                return false;
            }
        }

        public async Task<bool> AtualizarVistoriaAsync(Viagem viagem)
        {
            try
            {
                LogInfo($"Atualizando vistoria {viagem.ViagemId}");
                LogInfo($"  - Status: {viagem.Status}");
                LogInfo($"  - DataFinal: {viagem.DataFinal}");
                LogInfo($"  - HoraFim: {viagem.HoraFim}");
                LogInfo($"  - KmFinal: {viagem.KmFinal}");

                // Limpa referências para evitar problemas de serialização
                viagem.Requisitante = null;
                viagem.SetorSolicitante = null;
                viagem.Veiculo = null;
                viagem.Motorista = null;

                var resultado = await _api.PutAsync($"/api/vistorias/{viagem.ViagemId}", viagem);

                if (!resultado)
                {
                    var erroApi = RelayApiService.UltimoErro ?? "Erro desconhecido";
                    var respostaApi = RelayApiService.UltimaRespostaErro ?? "Sem resposta";
                    LogError($"Falha ao atualizar vistoria: {erroApi}");
                    LogError($"Resposta da API: {respostaApi}");
                    _ = _alerta.Erro("Erro ao atualizar vistoria", $"{erroApi}\n\nDetalhes: {respostaApi}");
                }
                else
                {
                    LogInfo($"Vistoria {viagem.ViagemId} atualizada com sucesso");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                LogError($"Exceção ao atualizar vistoria {viagem.ViagemId}: {ex.Message}", ex);
                _ = _alerta.Erro("Erro ao atualizar vistoria", ex.Message);
                return false;
            }
        }

        public async Task<bool> ExcluirVistoriaAsync(Guid id)
        {
            try
            {
                return await _api.DeleteAsync($"/api/vistorias/{id}");
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("Erro ao excluir vistoria" , ex.Message);
                return false;
            }
        }

        public async Task<List<Origem>> ObterOrigensAsync()
        {
            try
            {
                var response = await _api.GetAsync<List<Origem>>("/api/origens");
                return response ?? new List<Origem>();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("Erro ao carregar origens" , ex.Message);
                return new List<Origem>();
            }
        }

        public async Task<List<Destino>> ObterDestinosAsync()
        {
            try
            {
                var response = await _api.GetAsync<List<Destino>>("/api/destinos");
                return response ?? new List<Destino>();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("Erro ao carregar destinos" , ex.Message);
                return new List<Destino>();
            }
        }

        public async Task<Viagem?> ObterVistoriaPorIdAsync(Guid id)
        {
            try
            {
                return await _api.GetAsync<Viagem>($"/api/vistorias/{id}");
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("Erro ao carregar vistoria" , ex.Message);
                return null;
            }
        }

        public async Task<List<SetorSolicitante>> ObterSetoresSolicitantesAsync()
        {
            try
            {
                var response = await _api.GetAsync<List<SetorSolicitante>>("/api/setoressolicitantes");
                return response ?? new List<SetorSolicitante>();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("Erro ao carregar setores solicitantes" , ex.Message);
                return new List<SetorSolicitante>();
            }
        }

        public async Task<List<Requisitante>> ObterRequisitantesAsync()
        {
            try
            {
                var response = await _api.GetAsync<List<Requisitante>>("/api/requisitantes");
                return response ?? new List<Requisitante>();
            }
            catch (Exception ex)
            {
                _ = _alerta.Erro("Erro ao carregar requisitantes" , ex.Message);
                return new List<Requisitante>();
            }
        }

        public Task<string?> ObterNomeUsuarioAsync(string usuarioId)
        {
            // Não é mais necessário - os nomes são retornados diretamente pela API no endpoint /api/vistorias/{id}
            return Task.FromResult<string?>(null);
        }
    }
}