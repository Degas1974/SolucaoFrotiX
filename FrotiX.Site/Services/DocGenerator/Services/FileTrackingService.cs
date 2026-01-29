/*
╔══════════════════════════════════════════════════════════════════════════╗
║                                                                          ║
║  📚 SERVIÇO DE RASTREAMENTO DE ARQUIVOS - DOCGENERATOR                  ║
║                                                                          ║
║  Serviço para detectar mudanças em arquivos e gerenciar cache          ║
║                                                                          ║
╚══════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FrotiX.Services.DocGenerator.Services
{
    public interface IFileTrackingService
    {
        Task<bool> NeedsUpdateAsync(string filePath, CancellationToken ct = default);
        Task<FileTrackingInfo> GetFileInfoAsync(string filePath, CancellationToken ct = default);
        Task<FileChangeDetectionResult> DetectChangesAsync(string filePath, CancellationToken ct = default);
        Task MarkAsDocumentedAsync(string filePath, int? version = null, CancellationToken ct = default);
        Task<List<FileTrackingInfo>> GetFilesNeedingUpdateAsync(CancellationToken ct = default);
        Task<List<DocumentationAlert>> GetPendingAlertsAsync(CancellationToken ct = default);
        Task RunAutoScanAsync(CancellationToken ct = default);
    }

    public class FileTrackingService : IFileTrackingService, IHostedService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileTrackingService> _logger;
        private readonly string _connectionString;
        private readonly string _projectRoot;
        private Timer? _autoScanTimer;
        private bool _isRunningAutoScan = false;

        // FileSystemWatcher para monitoramento em tempo real
        private readonly List<FileSystemWatcher> _watchers = new();
        private readonly HashSet<string> _pendingChanges = new();
        private readonly object _pendingChangesLock = new();
        private Timer? _debounceTimer;
        private const int DebounceDelayMs = 2000; // Aguarda 2 segundos após última mudança

        // Extensões monitoradas
        private static readonly string[] MonitoredExtensions = { ".cs", ".cshtml", ".js", ".css" };

        // Pastas monitoradas (relativas à raiz do projeto)
        private static readonly string[] MonitoredFolders =
        {
            "Controllers",
            "Pages",
            "Services",
            "Repository",
            "Helpers",
            "Middlewares",
            "Hubs",
            "Models",
            "ViewModels",
            "Data",
            "wwwroot\\js",
            "wwwroot\\css"
        };

        // Pastas ignoradas
        private static readonly string[] IgnoredFolders = { "bin", "obj", ".git", "node_modules", "Documentacao" };

        public FileTrackingService(
            IConfiguration configuration,
            ILogger<FileTrackingService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            // Determinar raiz do projeto
            _projectRoot = configuration.GetValue<string>("DocGenerator:ProjectRoot")
                ?? Directory.GetCurrentDirectory();
        }

        public async Task<bool> NeedsUpdateAsync(string filePath, CancellationToken ct = default)
        {
            try
            {
                var info = await GetFileInfoAsync(filePath, ct);
                
                if (info == null)
                {
                    // Arquivo não existe no tracking = precisa atualizar
                    return true;
                }

                return info.NeedsUpdate;
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao verificar se arquivo precisa atualizar: {FilePath}", filePath);
                return true; // Em caso de erro, assume que precisa atualizar
            }
        }

        public async Task<FileTrackingInfo> GetFileInfoAsync(string filePath, CancellationToken ct = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(ct);

                const string sql = @"
                    SELECT 
                        FilePath,
                        FileHash,
                        FileSize,
                        LineCount,
                        CharacterCount,
                        LastModified,
                        LastDocumented,
                        DocumentationVersion,
                        NeedsUpdate,
                        UpdateReason,
                        CreatedAt,
                        UpdatedAt
                    FROM DocGenerator.FileTracking
                    WHERE FilePath = @FilePath";

                return await connection.QueryFirstOrDefaultAsync<FileTrackingInfo>(sql, new { FilePath = filePath });
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao obter informações do arquivo: {FilePath}", filePath);
                return null;
            }
        }

        public async Task<FileChangeDetectionResult> DetectChangesAsync(string filePath, CancellationToken ct = default)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new FileChangeDetectionResult
                    {
                        FilePath = filePath,
                        ChangeType = FileChangeType.Deleted,
                        NeedsUpdate = false,
                        ErrorMessage = "Arquivo não encontrado"
                    };
                }

                // Calcular hash do arquivo
                var fileInfo = new FileInfo(filePath);
                var hash = await CalculateFileHashAsync(filePath, ct);
                var lineCount = await CountLinesAsync(filePath, ct);
                var charCount = await CountCharactersAsync(filePath, ct);

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(ct);

                // Chamar stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@FilePath", filePath);
                parameters.Add("@NewHash", hash);
                parameters.Add("@NewSize", (int)fileInfo.Length);
                parameters.Add("@NewLineCount", lineCount);
                parameters.Add("@NewCharCount", charCount);
                parameters.Add("@NeedsUpdate", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@UpdateReason", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "DocGenerator.sp_DetectFileChanges",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var needsUpdate = parameters.Get<bool>("@NeedsUpdate");
                var updateReason = parameters.Get<string>("@UpdateReason");

                return new FileChangeDetectionResult
                {
                    FilePath = filePath,
                    FileHash = hash,
                    FileSize = fileInfo.Length,
                    LineCount = lineCount,
                    CharacterCount = charCount,
                    ChangeType = needsUpdate ? FileChangeType.Modified : FileChangeType.Unchanged,
                    NeedsUpdate = needsUpdate,
                    UpdateReason = updateReason,
                    DetectedAt = DateTime.Now
                };
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao detectar mudanças no arquivo: {FilePath}", filePath);
                return new FileChangeDetectionResult
                {
                    FilePath = filePath,
                    ChangeType = FileChangeType.Error,
                    NeedsUpdate = true,
                    ErrorMessage = error.Message
                };
            }
        }

        public async Task MarkAsDocumentedAsync(string filePath, int? version = null, CancellationToken ct = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(ct);

                await connection.ExecuteAsync(
                    "DocGenerator.sp_MarkAsDocumented",
                    new { FilePath = filePath, DocumentationVersion = version },
                    commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Arquivo marcado como documentado: {FilePath}", filePath);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao marcar arquivo como documentado: {FilePath}", filePath);
                throw;
            }
        }

        public async Task<List<FileTrackingInfo>> GetFilesNeedingUpdateAsync(CancellationToken ct = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(ct);

                const string sql = @"
                    SELECT * FROM DocGenerator.vw_FilesNeedingUpdate
                    ORDER BY Priority DESC, DaysSinceDocumented DESC";

                var result = await connection.QueryAsync<FileTrackingInfo>(sql);
                return result.AsList();
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao obter arquivos que precisam atualizar");
                return new List<FileTrackingInfo>();
            }
        }

        public async Task<List<DocumentationAlert>> GetPendingAlertsAsync(CancellationToken ct = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(ct);

                const string sql = @"
                    SELECT * FROM DocGenerator.DocumentationAlerts
                    WHERE Status = 'PENDING'
                    ORDER BY Priority DESC, CreatedAt DESC";

                var result = await connection.QueryAsync<DocumentationAlert>(sql);
                return result.AsList();
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao obter alertas pendentes");
                return new List<DocumentationAlert>();
            }
        }

        public async Task RunAutoScanAsync(CancellationToken ct = default)
        {
            if (_isRunningAutoScan)
            {
                _logger.LogWarning("Varredura automática já está em execução");
                return;
            }

            _isRunningAutoScan = true;
            
            try
            {
                _logger.LogInformation("Iniciando varredura automática de arquivos...");

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(ct);

                await connection.ExecuteAsync(
                    "DocGenerator.sp_RunAutoScan",
                    commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Varredura automática concluída");
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro durante varredura automática");
            }
            finally
            {
                _isRunningAutoScan = false;
            }
        }

        #region Helper Methods

        private async Task<string> CalculateFileHashAsync(string filePath, CancellationToken ct)
        {
            using var sha256 = SHA256.Create();
            using var stream = File.OpenRead(filePath);
            var hashBytes = await sha256.ComputeHashAsync(stream, ct);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

        private async Task<int> CountLinesAsync(string filePath, CancellationToken ct)
        {
            var lineCount = 0;
            using var reader = new StreamReader(filePath);
            while (await reader.ReadLineAsync() != null)
            {
                lineCount++;
                ct.ThrowIfCancellationRequested();
            }
            return lineCount;
        }

        private async Task<int> CountCharactersAsync(string filePath, CancellationToken ct)
        {
            var content = await File.ReadAllTextAsync(filePath, ct);
            return content.Length;
        }

        #endregion

        #region IHostedService Implementation

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de rastreamento de arquivos iniciado");

            // Configurar timer para varredura automática (a cada 24 horas)
            var autoScanInterval = _configuration.GetValue<int>("DocGenerator:AutoScanIntervalHours", 24);
            _autoScanTimer = new Timer(
                async _ => await RunAutoScanAsync(),
                null,
                TimeSpan.FromHours(autoScanInterval),
                TimeSpan.FromHours(autoScanInterval));

            // Iniciar FileSystemWatchers para monitoramento em tempo real
            var enableRealTimeMonitoring = _configuration.GetValue<bool>("DocGenerator:EnableRealTimeMonitoring", true);
            if (enableRealTimeMonitoring)
            {
                InitializeFileWatchers();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de rastreamento de arquivos parando");
            _autoScanTimer?.Change(Timeout.Infinite, 0);
            _debounceTimer?.Change(Timeout.Infinite, 0);

            // Parar todos os watchers
            foreach (var watcher in _watchers)
            {
                watcher.EnableRaisingEvents = false;
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _autoScanTimer?.Dispose();
            _debounceTimer?.Dispose();

            foreach (var watcher in _watchers)
            {
                watcher.Dispose();
            }
            _watchers.Clear();
        }

        #endregion

        #region FileSystemWatcher - Monitoramento em Tempo Real

        /// <summary>
        /// Inicializa os FileSystemWatchers para cada pasta monitorada
        /// </summary>
        private void InitializeFileWatchers()
        {
            try
            {
                _logger.LogInformation("Inicializando monitoramento em tempo real de arquivos em: {Root}", _projectRoot);

                foreach (var folder in MonitoredFolders)
                {
                    var fullPath = Path.Combine(_projectRoot, folder);

                    if (!Directory.Exists(fullPath))
                    {
                        _logger.LogDebug("Pasta não encontrada, ignorando: {Folder}", fullPath);
                        continue;
                    }

                    try
                    {
                        var watcher = new FileSystemWatcher(fullPath)
                        {
                            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime,
                            IncludeSubdirectories = true,
                            EnableRaisingEvents = true
                        };

                        // Registrar eventos
                        watcher.Changed += OnFileChanged;
                        watcher.Created += OnFileChanged;
                        watcher.Renamed += OnFileRenamed;
                        watcher.Deleted += OnFileDeleted;
                        watcher.Error += OnWatcherError;

                        _watchers.Add(watcher);
                        _logger.LogDebug("FileSystemWatcher criado para: {Folder}", fullPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Erro ao criar watcher para pasta: {Folder}", fullPath);
                    }
                }

                _logger.LogInformation("Monitoramento em tempo real ativado para {Count} pastas", _watchers.Count);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FileTrackingService.cs", "InitializeFileWatchers", error);
            }
        }

        /// <summary>
        /// Evento disparado quando um arquivo é modificado ou criado
        /// </summary>
        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (!ShouldMonitorFile(e.FullPath))
                    return;

                _logger.LogDebug("Arquivo modificado detectado: {File}", e.FullPath);
                AddPendingChange(e.FullPath);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao processar evento de mudança: {File}", e.FullPath);
            }
        }

        /// <summary>
        /// Evento disparado quando um arquivo é renomeado
        /// </summary>
        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            try
            {
                // Se o novo nome deve ser monitorado, adicionar
                if (ShouldMonitorFile(e.FullPath))
                {
                    _logger.LogDebug("Arquivo renomeado detectado: {OldName} -> {NewName}", e.OldFullPath, e.FullPath);
                    AddPendingChange(e.FullPath);
                }

                // Marcar arquivo antigo como deletado (se estava sendo rastreado)
                if (ShouldMonitorFile(e.OldFullPath))
                {
                    // Poderia registrar a deleção do arquivo antigo no banco
                    _logger.LogDebug("Arquivo antigo removido do monitoramento: {OldName}", e.OldFullPath);
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao processar evento de renomeação: {File}", e.FullPath);
            }
        }

        /// <summary>
        /// Evento disparado quando um arquivo é deletado
        /// </summary>
        private void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (!ShouldMonitorFile(e.FullPath))
                    return;

                _logger.LogDebug("Arquivo deletado detectado: {File}", e.FullPath);
                // Não adicionar a pendingChanges, pois o arquivo não existe mais
                // Poderia registrar a deleção no banco se necessário
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Erro ao processar evento de deleção: {File}", e.FullPath);
            }
        }

        /// <summary>
        /// Evento disparado quando ocorre erro no watcher
        /// </summary>
        private void OnWatcherError(object sender, ErrorEventArgs e)
        {
            var error = e.GetException();
            _logger.LogError(error, "Erro no FileSystemWatcher");

            // Tentar reiniciar o watcher após erro
            if (sender is FileSystemWatcher watcher)
            {
                try
                {
                    watcher.EnableRaisingEvents = false;
                    watcher.EnableRaisingEvents = true;
                    _logger.LogInformation("FileSystemWatcher reiniciado após erro");
                }
                catch (Exception restartError)
                {
                    _logger.LogError(restartError, "Falha ao reiniciar FileSystemWatcher");
                }
            }
        }

        /// <summary>
        /// Verifica se o arquivo deve ser monitorado baseado na extensão e pasta
        /// </summary>
        private bool ShouldMonitorFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            if (!MonitoredExtensions.Contains(extension))
                return false;

            // Verificar se está em pasta ignorada
            foreach (var ignored in IgnoredFolders)
            {
                if (filePath.Contains(Path.DirectorySeparatorChar + ignored + Path.DirectorySeparatorChar) ||
                    filePath.Contains(Path.AltDirectorySeparatorChar + ignored + Path.AltDirectorySeparatorChar))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adiciona um arquivo à lista de mudanças pendentes com debounce
        /// </summary>
        private void AddPendingChange(string filePath)
        {
            lock (_pendingChangesLock)
            {
                _pendingChanges.Add(filePath);

                // Reiniciar o timer de debounce
                _debounceTimer?.Dispose();
                _debounceTimer = new Timer(
                    async _ => await ProcessPendingChangesAsync(),
                    null,
                    DebounceDelayMs,
                    Timeout.Infinite);
            }
        }

        /// <summary>
        /// Processa todas as mudanças pendentes (após debounce)
        /// </summary>
        private async Task ProcessPendingChangesAsync()
        {
            List<string> filesToProcess;

            lock (_pendingChangesLock)
            {
                if (_pendingChanges.Count == 0)
                    return;

                filesToProcess = new List<string>(_pendingChanges);
                _pendingChanges.Clear();
            }

            _logger.LogInformation("Processando {Count} arquivos modificados em tempo real", filesToProcess.Count);

            foreach (var filePath in filesToProcess)
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        _logger.LogDebug("Arquivo não existe mais, ignorando: {File}", filePath);
                        continue;
                    }

                    var result = await DetectChangesAsync(filePath);

                    if (result.NeedsUpdate)
                    {
                        _logger.LogInformation(
                            "Documentação desatualizada detectada: {File} - Motivo: {Reason}",
                            Path.GetFileName(filePath),
                            result.UpdateReason);
                    }
                }
                catch (Exception error)
                {
                    _logger.LogError(error, "Erro ao processar mudança em: {File}", filePath);
                }
            }
        }

        #endregion
    }

    #region Model Classes

    public class FileTrackingInfo
    {
        public string FilePath { get; set; } = null!;
        public string FileHash { get; set; } = null!;
        public int FileSize { get; set; }
        public int LineCount { get; set; }
        public int CharacterCount { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime? LastDocumented { get; set; }
        public int DocumentationVersion { get; set; } = 1;
        public bool NeedsUpdate { get; set; }
        public string? UpdateReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? DaysSinceDocumented { get; set; }
        public string? AlertType { get; set; }
        public int? Priority { get; set; }
    }

    public class FileChangeDetectionResult
    {
        public string FilePath { get; set; } = null!;
        public string? FileHash { get; set; }
        public long FileSize { get; set; }
        public int LineCount { get; set; }
        public int CharacterCount { get; set; }
        public FileChangeType ChangeType { get; set; }
        public bool NeedsUpdate { get; set; }
        public string? UpdateReason { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime DetectedAt { get; set; }
        public decimal? ChangePercentage { get; set; }
    }

    public enum FileChangeType
    {
        Unchanged,
        Modified,
        Added,
        Deleted,
        Error
    }

    public class DocumentationAlert
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = null!;
        public string AlertType { get; set; } = null!;
        public string AlertMessage { get; set; } = null!;
        public int Priority { get; set; } = 1;
        public int? AssignedToUserId { get; set; }
        public string Status { get; set; } = "PENDING";
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    #endregion
}
