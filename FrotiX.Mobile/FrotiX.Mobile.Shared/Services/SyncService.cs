using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FrotiX.Mobile.Shared.Models;
using FrotiX.Mobile.Shared.Services.IServices;

namespace FrotiX.Mobile.Shared.Services
{
    public class SyncService : ISyncService
    {
        // CHAVES PADRONIZADAS PARA ECONOMILDO
        private const string KEY_VEICULOS = "veiculos_economildo";
        private const string KEY_MOTORISTAS = "motoristas_economildo";
        private const string KEY_SYNC_VEICULOS = "sync_veiculos_data";
        private const string KEY_SYNC_MOTORISTAS = "sync_motoristas_data";
        private const string KEY_DATA_SYNC = "data_ultima_sync";

        private readonly IVeiculoService _veiculoService;
        private readonly IMotoristaService _motoristaService;
        private readonly ILogService _logger;

        public SyncService(IVeiculoService veiculoService, IMotoristaService motoristaService, ILogService logger)
        {
            _veiculoService = veiculoService;
            _motoristaService = motoristaService;
            _logger = logger;
        }

        // ============================================
        // MÉTODOS DE CONTROLE DE EXPIRAÇÃO
        // ============================================

        private async Task<bool> DadosExpiradosAsync()
        {
            try
            {
#if ANDROID || IOS || MACCATALYST
                var dataUltimaSyncStr = await Microsoft.Maui.Storage.SecureStorage.GetAsync(KEY_DATA_SYNC);
#else
                var dataUltimaSyncStr = await Microsoft.Maui.Storage.SecureStorage.Default.GetAsync(KEY_DATA_SYNC);
#endif
                if (string.IsNullOrEmpty(dataUltimaSyncStr))
                    return true;

                if (DateTime.TryParse(dataUltimaSyncStr, out var dataUltimaSync))
                {
                    var dataAtual = DateTime.Now.Date;
                    var dataSyncArmazenada = dataUltimaSync.Date;
                    return dataAtual != dataSyncArmazenada;
                }

                return true;
            }
            catch (Exception ex)
            {
            _logger.Error("Erro ao verificar expiração dos dados", ex);
                return true;
            }
        }

        private async Task LimparDadosExpiradosSeNecessarioAsync()
        {
            try
            {
                if (await DadosExpiradosAsync())
                {
                    await LimparTodosDadosLocaisAsync();
                }
            }
            catch (Exception ex)
            {
            _logger.Error("Erro ao limpar dados expirados", ex);
            }
        }

        private async Task AtualizarDataSyncAsync()
        {
            try
            {
#if ANDROID || IOS || MACCATALYST
                await Microsoft.Maui.Storage.SecureStorage.SetAsync(KEY_DATA_SYNC, DateTime.Now.ToString("o"));
#else
                await Microsoft.Maui.Storage.SecureStorage.Default.SetAsync(KEY_DATA_SYNC, DateTime.Now.ToString("o"));
#endif
            }
            catch (Exception ex)
            {
            _logger.Error("Erro ao atualizar data de sincronização", ex);
            }
        }

        // ============================================
        // MÉTODOS DE VEÍCULOS
        // ============================================

        public async Task<(bool sucesso, string mensagem)> SincronizarVeiculosAsync()
        {
            try
            {
            _logger.Info("Iniciando sincronização de veículos...");
                
                // Chamar API
                var veiculosApi = await _veiculoService.ObterVeiculoCompletoDropdownAsync();
                
                if (veiculosApi == null || !veiculosApi.Any())
                {
                _logger.Warning("Nenhum veículo retornado da API");
                    return (false, "Nenhum veículo encontrado na API.");
                }

            _logger.Info($"Recebidos {veiculosApi.Count} veículos da API");

                // CORREÇÃO: Processar dados em background thread
                await Task.Run(async () =>
                {
                    try
                    {
                        // Converter para DTO local
                        var veiculosLocal = veiculosApi.Select(v => new VeiculoLocalDto
                        {
                            VeiculoId = v.VeiculoId,
                            Placa = v.Placa ?? "",
                            VeiculoCompleto = v.VeiculoCompleto ?? $"Veículo {v.Placa}",
                            Economildo = true,
                            Status = true
                        }).ToList();

                        // Serializar
                        var json = JsonSerializer.Serialize(veiculosLocal);
                    _logger.Info($"JSON serializado com {json.Length} caracteres");

                        // Salvar no SecureStorage
#if ANDROID || IOS || MACCATALYST
                        await Microsoft.Maui.Storage.SecureStorage.SetAsync(KEY_VEICULOS, json);
                        await Microsoft.Maui.Storage.SecureStorage.SetAsync(KEY_SYNC_VEICULOS, DateTime.Now.ToString("o"));
#else
                        await Microsoft.Maui.Storage.SecureStorage.Default.SetAsync(KEY_VEICULOS, json);
                        await Microsoft.Maui.Storage.SecureStorage.Default.SetAsync(KEY_SYNC_VEICULOS, DateTime.Now.ToString("o"));
#endif
                        await AtualizarDataSyncAsync();
                        
                    _logger.Info("Veículos salvos com sucesso no armazenamento local");
                    }
                    catch (Exception innerEx)
                    {
                    _logger.Error("Erro ao processar veículos em background", innerEx);
                        throw;
                    }
                });

                return (true, $"{veiculosApi.Count} veículos sincronizados!");
            }
            catch (Exception ex)
            {
            _logger.Error("Erro geral na sincronização de veículos", ex);
                return (false, $"Erro: {ex.Message}");
            }
        }

        public async Task<List<VeiculoLocalDto>> ObterVeiculosLocaisAsync()
        {
            try
            {
#if ANDROID || IOS || MACCATALYST
                var json = await Microsoft.Maui.Storage.SecureStorage.GetAsync(KEY_VEICULOS);
#else
                var json = await Microsoft.Maui.Storage.SecureStorage.Default.GetAsync(KEY_VEICULOS);
#endif
                if (string.IsNullOrEmpty(json))
                    return new List<VeiculoLocalDto>();

                // CORREÇÃO: Deserializar em background thread
                return await Task.Run(() => 
                    JsonSerializer.Deserialize<List<VeiculoLocalDto>>(json) ?? new List<VeiculoLocalDto>()
                );
            }
            catch (Exception ex)
            {
            _logger.Error("Erro ao obter veículos locais", ex);
                return new List<VeiculoLocalDto>();
            }
        }

        public async Task<bool> LimparVeiculosLocaisAsync()
        {
            try
            {
#if ANDROID || IOS || MACCATALYST
                Microsoft.Maui.Storage.SecureStorage.Remove(KEY_VEICULOS);
                Microsoft.Maui.Storage.SecureStorage.Remove(KEY_SYNC_VEICULOS);
#else
                Microsoft.Maui.Storage.SecureStorage.Default.Remove(KEY_VEICULOS);
                Microsoft.Maui.Storage.SecureStorage.Default.Remove(KEY_SYNC_VEICULOS);
#endif
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
            _logger.Error("Erro ao limpar veículos locais", ex);
                return false;
            }
        }

        public async Task<int> ContarVeiculosLocaisAsync()
        {
            var veiculos = await ObterVeiculosLocaisAsync();
            return veiculos.Count;
        }

        public async Task<DateTime?> ObterDataUltimaSyncVeiculosAsync()
        {
            try
            {
#if ANDROID || IOS || MACCATALYST
                var syncStr = await Microsoft.Maui.Storage.SecureStorage.GetAsync(KEY_SYNC_VEICULOS);
#else
                var syncStr = await Microsoft.Maui.Storage.SecureStorage.Default.GetAsync(KEY_SYNC_VEICULOS);
#endif
                if (!string.IsNullOrEmpty(syncStr) && DateTime.TryParse(syncStr, out var dt))
                    return dt;
            }
            catch (Exception ex)
            {
            _logger.Error("Erro ao obter data de sync de veículos", ex);
            }
            
            return null;
        }

        // ============================================
        // MÉTODOS DE MOTORISTAS
        // ============================================

        public async Task<(bool sucesso, string mensagem)> SincronizarMotoristasAsync()
        {
            try
            {
            _logger.Info("Iniciando sincronização de motoristas...");
                
                // Chamar API - Retorna MotoristaViewModel (apenas MotoristaId e Nome)
                var motoristasApi = await _motoristaService.ObterMotoristasAsync();
                
                if (motoristasApi == null || !motoristasApi.Any())
                {
                _logger.Warning("Nenhum motorista retornado da API");
                    return (false, "Nenhum motorista encontrado na API.");
                }

            _logger.Info($"Recebidos {motoristasApi.Count} motoristas da API");

                // CORREÇÃO: Processar dados em background thread
                await Task.Run(async () =>
                {
                    try
                    {
                        // MotoristaViewModel tem apenas: MotoristaId e Nome
                        var motoristasLocal = motoristasApi.Select(m => new MotoristaViewModel
                        {
                            MotoristaId = m.MotoristaId,
                            Nome = m.Nome ?? ""
                        }).ToList();

                        // Serializar
                        var json = JsonSerializer.Serialize(motoristasLocal);
                    _logger.Info($"JSON serializado com {json.Length} caracteres");

                        // Salvar no SecureStorage
#if ANDROID || IOS || MACCATALYST
                        await Microsoft.Maui.Storage.SecureStorage.SetAsync(KEY_MOTORISTAS, json);
                        await Microsoft.Maui.Storage.SecureStorage.SetAsync(KEY_SYNC_MOTORISTAS, DateTime.Now.ToString("o"));
#else
                        await Microsoft.Maui.Storage.SecureStorage.Default.SetAsync(KEY_MOTORISTAS, json);
                        await Microsoft.Maui.Storage.SecureStorage.Default.SetAsync(KEY_SYNC_MOTORISTAS, DateTime.Now.ToString("o"));
#endif
                        await AtualizarDataSyncAsync();
                        
                    _logger.Info("Motoristas salvos com sucesso no armazenamento local");
                    }
                    catch (Exception innerEx)
                    {
                    _logger.Error("Erro ao processar motoristas em background", innerEx);
                        throw;
                    }
                });

                return (true, $"{motoristasApi.Count} motoristas sincronizados!");
            }
            catch (Exception ex)
            {
            _logger.Error("Erro geral na sincronização de motoristas", ex);
                return (false, $"Erro: {ex.Message}");
            }
        }

        public async Task<(bool sucesso, string mensagem)> SincronizarMotoristasComFotosAsync()
        {
            return await Task.FromResult((false, "Sincronização com fotos desabilitada para economizar memória"));
        }

        public async Task<List<MotoristaViewModel>> ObterMotoristasLocaisAsync()
        {
            try
            {
#if ANDROID || IOS || MACCATALYST
                var json = await Microsoft.Maui.Storage.SecureStorage.GetAsync(KEY_MOTORISTAS);
#else
                var json = await Microsoft.Maui.Storage.SecureStorage.Default.GetAsync(KEY_MOTORISTAS);
#endif
                if (string.IsNullOrEmpty(json))
                    return new List<MotoristaViewModel>();

                // CORREÇÃO: Deserializar em background thread
                return await Task.Run(() =>
                    JsonSerializer.Deserialize<List<MotoristaViewModel>>(json) ?? new List<MotoristaViewModel>()
                );
            }
            catch (Exception ex)
            {
            _logger.Error("Erro ao obter motoristas locais", ex);
                return new List<MotoristaViewModel>();
            }
        }

        public async Task<bool> LimparMotoristasLocaisAsync()
        {
            try
            {
#if ANDROID || IOS || MACCATALYST
                Microsoft.Maui.Storage.SecureStorage.Remove(KEY_MOTORISTAS);
                Microsoft.Maui.Storage.SecureStorage.Remove(KEY_SYNC_MOTORISTAS);
#else
                Microsoft.Maui.Storage.SecureStorage.Default.Remove(KEY_MOTORISTAS);
                Microsoft.Maui.Storage.SecureStorage.Default.Remove(KEY_SYNC_MOTORISTAS);
#endif
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
            _logger.Error("Erro ao limpar motoristas locais", ex);
                return false;
            }
        }

        public async Task<int> ContarMotoristasLocaisAsync()
        {
            var motoristas = await ObterMotoristasLocaisAsync();
            return motoristas.Count;
        }

        public async Task<DateTime?> ObterDataUltimaSyncMotoristasAsync()
        {
            try
            {
#if ANDROID || IOS || MACCATALYST
                var syncStr = await Microsoft.Maui.Storage.SecureStorage.GetAsync(KEY_SYNC_MOTORISTAS);
#else
                var syncStr = await Microsoft.Maui.Storage.SecureStorage.Default.GetAsync(KEY_SYNC_MOTORISTAS);
#endif
                if (!string.IsNullOrEmpty(syncStr) && DateTime.TryParse(syncStr, out var dt))
                    return dt;
            }
            catch (Exception ex)
            {
            _logger.Error("Erro ao obter data de sync de motoristas", ex);
            }
            
            return null;
        }

        // ============================================
        // MÉTODOS COMBINADOS
        // ============================================

        public async Task<(bool sucesso, string mensagem)> SincronizarTodosOsDadosAsync()
        {
            try
            {
            _logger.Info("Iniciando sincronização completa...");
                
                var resultadoVeiculos = await SincronizarVeiculosAsync();
                var resultadoMotoristas = await SincronizarMotoristasAsync();

                if (resultadoVeiculos.sucesso && resultadoMotoristas.sucesso)
                {
                    return (true, $"Sincronização completa! {resultadoVeiculos.mensagem} {resultadoMotoristas.mensagem}");
                }
                else if (!resultadoVeiculos.sucesso && !resultadoMotoristas.sucesso)
                {
                    return (false, $"Falha total: {resultadoVeiculos.mensagem} | {resultadoMotoristas.mensagem}");
                }
                else
                {
                    var mensagemSucesso = resultadoVeiculos.sucesso ? resultadoVeiculos.mensagem : resultadoMotoristas.mensagem;
                    var mensagemErro = !resultadoVeiculos.sucesso ? resultadoVeiculos.mensagem : resultadoMotoristas.mensagem;
                    return (true, $"Sincronização parcial: {mensagemSucesso} | ERRO: {mensagemErro}");
                }
            }
            catch (Exception ex)
            {
            _logger.Error("Erro geral na sincronização completa", ex);
                return (false, $"Erro geral: {ex.Message}");
            }
        }

        public async Task<bool> LimparTodosDadosLocaisAsync()
        {
            try
            {
                await LimparVeiculosLocaisAsync();
                await LimparMotoristasLocaisAsync();
#if ANDROID || IOS || MACCATALYST
                Microsoft.Maui.Storage.SecureStorage.Remove(KEY_DATA_SYNC);
#else
                Microsoft.Maui.Storage.SecureStorage.Default.Remove(KEY_DATA_SYNC);
#endif
                
            _logger.Info("Todos os dados locais foram limpos");
                return true;
            }
            catch (Exception ex)
            {
            _logger.Error("Erro ao limpar todos os dados locais", ex);
                return false;
            }
        }

        public async Task<SyncStatusViewModel> ObterStatusSincronizacaoAsync()
        {
            try
            {
                // Verificar e limpar dados expirados
                await LimparDadosExpiradosSeNecessarioAsync();

                var status = new SyncStatusViewModel
                {
                    TotalVeiculos = await ContarVeiculosLocaisAsync(),
                    TotalMotoristas = await ContarMotoristasLocaisAsync(),
                    UltimaSyncVeiculos = await ObterDataUltimaSyncVeiculosAsync(),
                    UltimaSyncMotoristas = await ObterDataUltimaSyncMotoristasAsync()
                };

                var dadosExpiraram = await DadosExpiradosAsync();

                if (dadosExpiraram || status.TotalVeiculos == 0 || status.TotalMotoristas == 0)
                {
                    status.MensagemStatus = dadosExpiraram 
                        ? "Dados expirados - Sincronização necessária" 
                        : "Sincronização necessária";
                    status.DadosDisponiveis = false;
                }
                else
                {
                    var syncMaisAntiga = new[] { status.UltimaSyncVeiculos, status.UltimaSyncMotoristas }
                        .Where(d => d.HasValue)
                        .OrderBy(d => d.Value)
                        .FirstOrDefault();

                    if (syncMaisAntiga.HasValue)
                    {
                        var diasDesdeSync = (DateTime.Now - syncMaisAntiga.Value).Days;
                        
                        status.MensagemStatus = diasDesdeSync == 0 
                            ? "Dados atualizados (hoje)"
                            : diasDesdeSync <= 7 
                                ? $"Última sync há {diasDesdeSync} dia(s)"
                                : $"Dados desatualizados ({diasDesdeSync} dias)";
                    }
                    else
                    {
                        status.MensagemStatus = "Status desconhecido";
                    }

                    status.DadosDisponiveis = true;
                }

                return status;
            }
            catch (Exception ex)
            {
            _logger.Error("Erro ao obter status de sincronização", ex);
                return new SyncStatusViewModel
                {
                    MensagemStatus = $"Erro: {ex.Message}",
                    DadosDisponiveis = false
                };
            }
        }
    }
}
