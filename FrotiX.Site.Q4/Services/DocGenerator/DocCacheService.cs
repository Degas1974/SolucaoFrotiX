/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: DocCacheService.cs                                                                      ║
   ║ 📂 CAMINHO: /Services/DocGenerator                                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Cache para evitar regeneração desnecessária de documentação. Usa manifest JSON.        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: GetAsync(), SetAsync(), IsValidAsync(), InvalidateAsync(), LoadManifestAsync()           ║
   ║ 🔗 DEPS: IDocCacheService, DocCacheManifest | 📅 13/01/2026 | 👤 Copilot | 📝 v2.0                  ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Services.DocGenerator.Interfaces;
using FrotiX.Services.DocGenerator.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FrotiX.Services.DocGenerator
{
    /// <summary>
    /// Serviço de cache para evitar regeneração desnecessária de documentação
    /// </summary>
    public class DocCacheService : IDocCacheService
    {
        private readonly IWebHostEnvironment _env;
        private readonly DocGeneratorSettings _settings;
        private readonly ILogger<DocCacheService> _logger;
        private DocCacheManifest _manifest;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private bool _isLoaded;

        public DocCacheService(
            IWebHostEnvironment env,
            IOptions<DocGeneratorSettings> settings,
            ILogger<DocCacheService> logger)
        {
            try
            {
                _env = env;
                _settings = settings.Value;
                _logger = logger;
                _manifest = new DocCacheManifest();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocCacheService.cs", ".ctor", error);
                _manifest = new DocCacheManifest();
            }
        }

        public async Task<DocCacheEntry?> GetAsync(string filePath, CancellationToken ct = default)
        {
            try
            {
                await LoadAsync(ct);

                var normalizedPath = NormalizePath(filePath);
                return _manifest.Entries.TryGetValue(normalizedPath, out var entry) ? entry : null;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocCacheService.cs", "GetAsync", error);
                return null;
            }
        }

        public async Task SetAsync(string filePath, DocCacheEntry entry, CancellationToken ct = default)
        {
            try
            {
                await _lock.WaitAsync(ct);

                try
                {
                    var normalizedPath = NormalizePath(filePath);
                    _manifest.Entries[normalizedPath] = entry;
                    _manifest.LastUpdated = DateTime.Now;
                    await SaveAsync(ct);
                }
                finally
                {
                    _lock.Release();
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocCacheService.cs", "SetAsync", error);
            }
        }

        public async Task RemoveAsync(string filePath, CancellationToken ct = default)
        {
            try
            {
                await _lock.WaitAsync(ct);

                try
                {
                    var normalizedPath = NormalizePath(filePath);
                    _manifest.Entries.Remove(normalizedPath);
                    _manifest.LastUpdated = DateTime.Now;
                    await SaveAsync(ct);
                }
                finally
                {
                    _lock.Release();
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocCacheService.cs", "RemoveAsync", error);
            }
        }

        public async Task ClearAsync(CancellationToken ct = default)
        {
            try
            {
                await _lock.WaitAsync(ct);

                try
                {
                    _manifest = new DocCacheManifest();
                    await SaveAsync(ct);
                    _logger.LogInformation("Cache de documentação limpo");
                }
                finally
                {
                    _lock.Release();
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocCacheService.cs", "ClearAsync", error);
            }
        }

        public async Task LoadAsync(CancellationToken ct = default)
        {
            if (_isLoaded) return;

            try
            {
                await _lock.WaitAsync(ct);

                try
                {
                    if (_isLoaded) return;

                    var cachePath = GetCachePath();

                    if (File.Exists(cachePath))
                    {
                        var json = await File.ReadAllTextAsync(cachePath, ct);
                        _manifest = JsonSerializer.Deserialize<DocCacheManifest>(json)
                            ?? new DocCacheManifest();

                        _logger.LogInformation(
                            "Cache carregado: {Count} entradas",
                            _manifest.Entries.Count);
                    }
                    else
                    {
                        _manifest = new DocCacheManifest();
                        _logger.LogInformation("Cache não encontrado, criando novo");
                    }

                    _isLoaded = true;
                }
                finally
                {
                    _lock.Release();
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocCacheService.cs", "LoadAsync", error);
                _manifest = new DocCacheManifest();
                _isLoaded = true;
            }
        }

        public async Task SaveAsync(CancellationToken ct = default)
        {
            try
            {
                var cachePath = GetCachePath();
                var cacheDir = Path.GetDirectoryName(cachePath);

                if (!string.IsNullOrEmpty(cacheDir))
                    Directory.CreateDirectory(cacheDir);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(_manifest, options);
                await File.WriteAllTextAsync(cachePath, json, ct);

                _logger.LogDebug("Cache salvo: {Path}", cachePath);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocCacheService.cs", "SaveAsync", error);
            }
        }

        public async Task<bool> NeedsRegenerationAsync(string filePath, string currentHash, CancellationToken ct = default)
        {
            try
            {
                var entry = await GetAsync(filePath, ct);

                if (entry == null)
                    return true;

                // Verificar hash do conteúdo
                if (entry.ContentHash != currentHash)
                    return true;

                // Verificar versão do template
                if (entry.TemplateVersion != _settings.TemplateVersion)
                    return true;

                // Verificar versão do prompt
                if (entry.PromptVersion != _settings.PromptVersion)
                    return true;

                // Verificar se os arquivos de saída existem
                if (!File.Exists(entry.MarkdownPath))
                    return true;

                foreach (var htmlPath in entry.HtmlPaths)
                {
                    if (!File.Exists(htmlPath))
                        return true;
                }

                return false;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("DocCacheService.cs", "NeedsRegenerationAsync", error);
                return true;
            }
        }

        private string GetCachePath()
        {
            return Path.Combine(
                _env.ContentRootPath,
                _settings.OutputPath,
                _settings.CacheFile);
        }

        private string NormalizePath(string filePath)
        {
            return filePath
                .Replace('\\', '/')
                .ToLowerInvariant()
                .Trim('/');
        }
    }
}
