using FrotiX.Mobile.Shared.Helpers;
using FrotiX.Mobile.Shared.Models;
using FrotiX.Mobile.Shared.Services.IServices;
using System.Text.Json;

namespace FrotiX.Mobile.Shared.Services
{
    public class ViagensEconomildoService : IViagensEconomildoService
    {
        private const string VIAGENS_KEY = "viagens_economildo";
        private readonly RelayApiService _api;
        private readonly string _apiUrl;

        public ViagensEconomildoService(RelayApiService api)
        {
            _api = api;
            _apiUrl = "/api/viagenseconomildo";
        }

        #region Armazenamento Local

        public async Task<bool> SalvarViagemLocalAsync(ViagemEconomildoLocal viagem)
        {
            try
            {
                var viagens = await ObterViagensLocaisAsync();
                
                // Verifica se já existe
                var existente = viagens.FirstOrDefault(v => v.ViagemEconomildoId == viagem.ViagemEconomildoId);
                if (existente != null)
                {
                    viagens.Remove(existente);
                }
                
                viagens.Add(viagem);
                
                var json = JsonSerializer.Serialize(viagens);
                await SecureStorage.SetAsync(VIAGENS_KEY, json);
                
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar viagem local: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ExcluirViagemLocalAsync(Guid viagemId)
        {
            try
            {
                var viagens = await ObterViagensLocaisAsync();
                var viagem = viagens.FirstOrDefault(v => v.ViagemEconomildoId == viagemId);
                
                if (viagem == null)
                    return false;
                
                viagens.Remove(viagem);
                
                var json = JsonSerializer.Serialize(viagens);
                await SecureStorage.SetAsync(VIAGENS_KEY, json);
                
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao excluir viagem local: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ViagemEconomildoLocal>> ObterViagensLocaisAsync()
        {
            try
            {
                var json = await SecureStorage.GetAsync(VIAGENS_KEY);
                if (string.IsNullOrEmpty(json))
                {
                    return new List<ViagemEconomildoLocal>();
                }
                
                var viagens = JsonSerializer.Deserialize<List<ViagemEconomildoLocal>>(json);
                return viagens ?? new List<ViagemEconomildoLocal>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao obter viagens locais: {ex.Message}");
                return new List<ViagemEconomildoLocal>();
            }
        }

        public async Task<List<ViagemEconomildoLocal>> ObterViagensNaoTransmitidasAsync()
        {
            try
            {
                var todasViagens = await ObterViagensLocaisAsync();
                return todasViagens.Where(v => !v.Transmitido).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao obter viagens não transmitidas: {ex.Message}");
                return new List<ViagemEconomildoLocal>();
            }
        }

        public async Task<bool> MarcarViagemComoTransmitidaAsync(Guid viagemId)
        {
            try
            {
                var viagens = await ObterViagensLocaisAsync();
                var viagem = viagens.FirstOrDefault(v => v.ViagemEconomildoId == viagemId);
                
                if (viagem == null)
                    return false;
                
                viagem.Transmitido = true;
                
                var json = JsonSerializer.Serialize(viagens);
                await SecureStorage.SetAsync(VIAGENS_KEY, json);
                
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao marcar viagem como transmitida: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LimparViagensTransmitidasAsync()
        {
            try
            {
                var viagens = await ObterViagensLocaisAsync();
                var viagensNaoTransmitidas = viagens.Where(v => !v.Transmitido).ToList();
                
                var json = JsonSerializer.Serialize(viagensNaoTransmitidas);
                await SecureStorage.SetAsync(VIAGENS_KEY, json);
                
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao limpar viagens transmitidas: {ex.Message}");
                return false;
            }
        }

        public async Task<int> ContarViagensNaoTransmitidasAsync()
        {
            try
            {
                var viagensNaoTransmitidas = await ObterViagensNaoTransmitidasAsync();
                return viagensNaoTransmitidas.Count;
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region Transmissão para Servidor

        public async Task<(bool sucesso, string mensagem)> TransmitirViagensAsync(List<ViagemEconomildoLocal> viagens)
        {
            try
            {
                if (viagens == null || !viagens.Any())
                {
                    return (false, "Nenhuma viagem para transmitir.");
                }

                // Converte para o modelo de API
                var viagensApi = viagens.Select(v =>
                {
                    var (mobCorreto, idaVoltaCorreta) = ViagensEconomildoHelper.DeterminarMobEIdaVolta(
                        v.MOB ?? "", 
                        v.IdaVolta ?? ""
                    );

                    System.Diagnostics.Debug.WriteLine(
                        $"📍 Viagem {v.ViagemEconomildoId.ToString().Substring(0, 8)}... | " +
                        $"Linha: {v.MOB} | Trajeto: {v.IdaVolta} → " +
                        $"MOB: {mobCorreto} | IdaVolta: {idaVoltaCorreta}"
                    );

                    var dataOriginal = v.Data;
                    var dataUtc = DateTime.SpecifyKind(dataOriginal.Date, DateTimeKind.Utc);
                    System.Diagnostics.Debug.WriteLine(
                        $"📅 Data Original: {dataOriginal:dd/MM/yyyy HH:mm:ss} (Kind: {dataOriginal.Kind}) → " +
                        $"Data UTC: {dataUtc:dd/MM/yyyy HH:mm:ss} (Kind: {dataUtc.Kind})"
                    );

                    return new ViagensEconomildo
                    {
                        ViagemEconomildoId = v.ViagemEconomildoId,
                        Data = DateTime.SpecifyKind(v.Data.Date, DateTimeKind.Utc),
                        VeiculoId = v.VeiculoId,
                        MotoristaId = v.MotoristaId,
                        MOB = mobCorreto,
                        Responsavel = v.Responsavel,
                        IdaVolta = idaVoltaCorreta,
                        HoraInicio = v.HoraInicio,
                        HoraFim = v.HoraFim,
                        QtdPassageiros = v.QtdPassageiros
                    };
                }).ToList();

                var sucesso = await _api.PostAsync($"{_apiUrl}/batch", viagensApi);
                
                if (sucesso)
                {
                    // Marca todas como transmitidas
                    foreach (var viagem in viagens)
                    {
                        await MarcarViagemComoTransmitidaAsync(viagem.ViagemEconomildoId);
                    }
                    
                    return (true, $"{viagens.Count} trajeto(s) transmitido(s) com sucesso!");
                }
                else
                {
                    return (false, "Erro ao transmitir viagens");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro na transmissão: {ex.Message}");
                return (false, $"Erro ao transmitir viagens: {ex.Message}");
            }
        }

        public async Task<(bool sucesso, string mensagem)> TransmitirTodasViagensAsync()
        {
            try
            {
                var viagensNaoTransmitidas = await ObterViagensNaoTransmitidasAsync();
                
                if (!viagensNaoTransmitidas.Any())
                {
                    return (false, "Não há viagens pendentes para transmitir.");
                }

                return await TransmitirViagensAsync(viagensNaoTransmitidas);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao transmitir todas as viagens: {ex.Message}");
                return (false, $"Erro ao transmitir: {ex.Message}");
            }
        }

        #endregion
    }
}
