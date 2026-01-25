using Microsoft.Extensions.Logging;
using FrotiX.Mobile.Shared.Services;
using FrotiX.Mobile.Shared.Services.IServices;
using FrotiX.Mobile.Shared.Data.Repository;
using FrotiX.Mobile.Shared.Data.Repository.IRepository;
using FrotiX.Mobile.Shared.Helpers;
using Syncfusion.Blazor;
using MudBlazor.Services;

namespace FrotiX.Mobile.Economildo
{
    public static class MauiProgram
    {
        private static string LogPath => Path.Combine(FileSystem.AppDataDirectory, "app_logs.txt");

        public static MauiApp CreateMauiApp()
        {
            // ========== SISTEMA DE LOG DE ERROS ==========
            InitializeErrorLogging();

            try
            {
                LogInfo("========================================");
                LogInfo("    INICIANDO FROTIX MOBILE ECONOMILDO  ");
                LogInfo("========================================");
                LogInfo($"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                LogInfo($"Plataforma: {DeviceInfo.Platform}");
                LogInfo($"Versão SO: {DeviceInfo.VersionString}");
                LogInfo($"Dispositivo: {DeviceInfo.Model}");
                LogInfo("----------------------------------------");

                // Register the Syncfusion license key
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
                        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    });

                LogInfo("✓ Fontes configuradas");

                // Adicionar Blazor WebView
                LogInfo("Adicionando Blazor WebView...");
                builder.Services.AddMauiBlazorWebView();

#if DEBUG
                builder.Services.AddBlazorWebViewDeveloperTools();
                builder.Logging.AddDebug();
#endif
                LogInfo("✓ Blazor WebView adicionado");

                // ⭐ Syncfusion
                LogInfo("Configurando Syncfusion...");
                builder.Services.AddSyncfusionBlazor();
                LogInfo("✓ Syncfusion configurado");

                // ⭐ MudBlazor
                LogInfo("Configurando MudBlazor...");
                builder.Services.AddMudServices();
                LogInfo("✓ MudBlazor configurado");

                // ⭐ Azure Relay API Service (substitui o HttpClient)
                LogInfo("Configurando Azure Relay...");
                builder.Services.AddSingleton<RelayApiService>();
                LogInfo("✓ Azure Relay configurado");

                // ⭐ Core Services
                LogInfo("Registrando Services...");
                builder.Services.AddScoped<ILogService, LogService>();
                builder.Services.AddScoped<IAlertaService, AlertaService>();
                builder.Services.AddScoped<IToastService, ToastService>();
                builder.Services.AddScoped<AlertaJs>();

                // ⭐ API Services (usando RelayApiService)
                builder.Services.AddScoped<IVeiculoService, VeiculoService>();
                builder.Services.AddScoped<IMotoristaService, MotoristaService>();

                // ⭐ Economildo Services
                builder.Services.AddScoped<ISyncService, SyncService>();
                builder.Services.AddScoped<IViagensEconomildoService, ViagensEconomildoService>();

                // ⭐ Repositories
                builder.Services.AddScoped<IViagensEconomildoRepository, ViagensEconomildoRepository>();

                LogInfo("✓ Services e Repositories registrados");

                // Página raiz do app
                builder.Services.AddSingleton<MainPage>();

                LogInfo("Buildando aplicação...");
                var app = builder.Build();
                LogInfo("✓✓✓ APLICAÇÃO INICIADA COM SUCESSO ✓✓✓");
                LogInfo("========================================\n");

                return app;
            }
            catch (Exception ex)
            {
                LogError("❌❌❌ ERRO FATAL NA INICIALIZAÇÃO ❌❌❌", ex);
                throw;
            }
        }

        // ========== MÉTODOS DE LOG ==========

        private static void InitializeErrorLogging()
        {
            try
            {
                if (File.Exists(LogPath))
                {
                    var fileInfo = new FileInfo(LogPath);
                    if (fileInfo.Length > 5 * 1024 * 1024)
                    {
                        var backupPath = Path.Combine(FileSystem.AppDataDirectory, "app_logs_backup.txt");
                        if (File.Exists(backupPath)) File.Delete(backupPath);
                        File.Move(LogPath, backupPath);
                    }
                }

                AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                {
                    var exception = e.ExceptionObject as Exception;
                    LogError("ERRO NÃO TRATADO (UnhandledException)", exception);
                };

                TaskScheduler.UnobservedTaskException += (sender, e) =>
                {
                    LogError("ERRO TASK NÃO OBSERVADA", e.Exception);
                    e.SetObserved();
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO AO INICIALIZAR LOG: {ex}");
            }
        }

        public static void LogInfo(string message)
        {
            try
            {
                var logMessage = $"[INFO  {DateTime.Now:HH:mm:ss.fff}] {message}";
                File.AppendAllText(LogPath, logMessage + Environment.NewLine);
                System.Diagnostics.Debug.WriteLine(logMessage);
            }
            catch { }
        }

        public static void LogError(string message, Exception? exception = null)
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
                File.AppendAllText(LogPath, logMessage + Environment.NewLine);
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

        public static void ClearLogs()
        {
            try
            {
                if (File.Exists(LogPath)) File.Delete(LogPath);
                LogInfo("========== LOGS LIMPOS ==========");
            }
            catch { }
        }
    }
}
