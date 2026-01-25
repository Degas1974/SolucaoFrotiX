using Microsoft.Extensions.Logging;
using FrotiX.Mobile.Shared.Services;
using FrotiX.Mobile.Shared.Services.IServices;
using Syncfusion.Blazor;
using Radzen;
using System.Collections.Concurrent;

namespace FrotiX.Mobile.Vistorias
{
    public static class MauiProgram
    {
        private static string LogPath => Path.Combine(FileSystem.AppDataDirectory , "app_logs.txt");

        // ⚡ BUFFER ASSÍNCRONO para logs - NUNCA bloqueia a UI
        private static readonly ConcurrentQueue<string> _logQueue = new();

        private static readonly SemaphoreSlim _logSemaphore = new(1 , 1);
        private static Timer? _logFlushTimer;

        public static MauiApp CreateMauiApp()
        {
            InitializeErrorLogging();
            InitializeAsyncLogging();

            try
            {
                LogInfo("========================================");
                LogInfo("    INICIANDO FROTIX MOBILE VISTORIAS   ");
                LogInfo("========================================");
                LogInfo($"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                LogInfo($"Plataforma: {DeviceInfo.Platform}");
                LogInfo($"Versão SO: {DeviceInfo.VersionString}");
                LogInfo($"Dispositivo: {DeviceInfo.Model}");
                LogInfo("----------------------------------------");

                LogInfo("Registrando licença Syncfusion...");
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                    "Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH5fcHRcRWZfV0ZwV0BWYEg="
                );
                LogInfo("✓ Licença Syncfusion registrada");

                LogInfo("Criando MauiApp Builder...");
                var builder = MauiApp.CreateBuilder();

                builder
                    .UseMauiApp<App>()
                    .ConfigureFonts(fonts =>
                    {
                        fonts.AddFont("OpenSans-Regular.ttf" , "OpenSansRegular");
                        fonts.AddFont("OpenSans-Semibold.ttf" , "OpenSansSemibold");
                    });

                LogInfo("✓ Fontes configuradas");

                LogInfo("Adicionando Blazor WebView...");
                builder.Services.AddMauiBlazorWebView();

#if DEBUG
                builder.Services.AddBlazorWebViewDeveloperTools();
                builder.Logging.AddDebug();
#endif
                LogInfo("✓ Blazor WebView adicionado");

                // ⭐ RADZEN - Componentes de UI (leve e rápido)
                LogInfo("Configurando Radzen...");
                builder.Services.AddRadzenComponents();
                LogInfo("✓ Radzen configurado");

                // ⚡ SYNCFUSION: Apenas para SfSignature (assinatura)
                LogInfo("Configurando Syncfusion (apenas SfSignature)...");
                builder.Services.AddSyncfusionBlazor(options =>
                {
                    // ⚡ Desabilita animações para melhor performance
                    options.EnableRippleEffect = false;
                });
                LogInfo("✓ Syncfusion configurado");

                // ⭐ Azure Relay API Service (substitui o HttpClient)
                LogInfo("Configurando Azure Relay...");
                builder.Services.AddSingleton<RelayApiService>();
                
                // ⚡ Configura logger estático do RelayApiService para capturar erros da API
                RelayApiService.ConfigurarLogger(
                    msg => LogInfo(msg),
                    (msg, ex) => LogError(msg, ex)
                );
                LogInfo("✓ Azure Relay configurado");

                // ⭐ Core Services
                LogInfo("Registrando Services...");
                builder.Services.AddScoped<ILogService , LogService>();
                builder.Services.AddScoped<IAlertaService , AlertaService>();
                builder.Services.AddScoped<IToastService , ToastService>();
                builder.Services.AddScoped<AlertaJs>();

                // ⭐ API Services (usando RelayApiService)
                builder.Services.AddScoped<IVeiculoService , VeiculoService>();
                builder.Services.AddScoped<IMotoristaService , MotoristaService>();
                builder.Services.AddScoped<IViagemService>(sp => new ViagemService(
                    sp.GetRequiredService<RelayApiService>(),
                    sp.GetRequiredService<IAlertaService>(),
                    sp.GetRequiredService<ILogService>()
                ));
                builder.Services.AddSingleton<IVistoriadorService , VistoriadorService>();
                builder.Services.AddScoped<IOcorrenciaService , OcorrenciaService>();

                LogInfo("✓ Services registrados");

                builder.Services.AddSingleton<MainPage>();

                LogInfo("Buildando aplicação...");
                var app = builder.Build();
                LogInfo("✓✓✓ APLICAÇÃO INICIADA COM SUCESSO ✓✓✓");
                LogInfo("========================================\n");

                return app;
            }
            catch (Exception ex)
            {
                LogError("❌❌❌ ERRO FATAL NA INICIALIZAÇÃO ❌❌❌" , ex);
                throw;
            }
        }

        /// <summary>
        /// ⚡ Inicializa o sistema de log assíncrono com flush periódico
        /// </summary>
        private static void InitializeAsyncLogging()
        {
            // Flush de logs a cada 2 segundos (não bloqueia UI!)
            _logFlushTimer = new Timer(async _ => await FlushLogsAsync() , null ,
                TimeSpan.FromSeconds(2) , TimeSpan.FromSeconds(2));
        }

        /// <summary>
        /// ⚡ Flush assíncrono dos logs em buffer
        /// </summary>
        private static async Task FlushLogsAsync()
        {
            if (_logQueue.IsEmpty)
                return;

            if (!await _logSemaphore.WaitAsync(0))
                return;

            try
            {
                var logs = new List<string>();
                while (_logQueue.TryDequeue(out var log))
                {
                    logs.Add(log);
                }

                if (logs.Count > 0)
                {
                    await File.AppendAllLinesAsync(LogPath , logs);
                }
            }
            catch { }
            finally
            {
                _logSemaphore.Release();
            }
        }

        private static void InitializeErrorLogging()
        {
            try
            {
                if (File.Exists(LogPath))
                {
                    var fileInfo = new System.IO.FileInfo(LogPath);
                    if (fileInfo.Length > 5 * 1024 * 1024)
                    {
                        var backupPath = Path.Combine(FileSystem.AppDataDirectory , "app_logs_backup.txt");
                        if (File.Exists(backupPath))
                            File.Delete(backupPath);
                        File.Move(LogPath , backupPath);
                    }
                }

                AppDomain.CurrentDomain.UnhandledException += (sender , e) =>
                {
                    var exception = e.ExceptionObject as Exception;
                    LogError("ERRO NÃO TRATADO (UnhandledException)" , exception);
                };

                TaskScheduler.UnobservedTaskException += (sender , e) =>
                {
                    LogError("ERRO TASK NÃO OBSERVADA" , e.Exception);
                    e.SetObserved();
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO AO INICIALIZAR LOG: {ex}");
            }
        }

        /// <summary>
        /// ⚡ OTIMIZADO: Log NÃO-BLOQUEANTE - adiciona ao buffer
        /// </summary>
        public static void LogInfo(string message)
        {
            try
            {
                var logMessage = $"[INFO  {DateTime.Now:HH:mm:ss.fff}] {message}";
                _logQueue.Enqueue(logMessage);
                System.Diagnostics.Debug.WriteLine(logMessage);
            }
            catch { }
        }

        /// <summary>
        /// ⚡ OTIMIZADO: Log de erro NÃO-BLOQUEANTE
        /// </summary>
        public static void LogError(string message , Exception? exception = null)
        {
            try
            {
                var logMessage = $"[ERROR {DateTime.Now:HH:mm:ss.fff}] {message}";
                if (exception != null)
                {
                    logMessage += $"\n  Exception: {exception.GetType().Name}";
                    logMessage += $"\n  Message: {exception.Message}";
                    logMessage += $"\n  StackTrace: {exception.StackTrace}";
                    if (exception.InnerException != null)
                    {
                        logMessage += $"\n  InnerException: {exception.InnerException.Message}";
                    }
                }
                _logQueue.Enqueue(logMessage);
                System.Diagnostics.Debug.WriteLine(logMessage);
            }
            catch { }
        }

        public static string GetLogPath() => LogPath;

        public static string ReadAllLogs()
        {
            try
            {
                return File.Exists(LogPath) ? File.ReadAllText(LogPath) : "Nenhum log disponível.";
            }
            catch (Exception ex)
            {
                return $"Erro ao ler logs: {ex.Message}";
            }
        }

        public static async Task ClearLogsAsync()
        {
            try
            {
                await FlushLogsAsync();

                if (File.Exists(LogPath))
                    File.Delete(LogPath);
                LogInfo("========== LOGS LIMPOS ==========");
            }
            catch { }
        }

        public static void ClearLogs() => _ = ClearLogsAsync();
    }
}