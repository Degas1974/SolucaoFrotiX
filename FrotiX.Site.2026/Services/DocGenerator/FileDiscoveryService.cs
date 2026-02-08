/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: FileDiscoveryService.cs                                                                 ║
   ║ 📂 CAMINHO: /Services/DocGenerator                                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Descoberta de arquivos no projeto. Aplica globs, filtra exclusões, calcula hashes.     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: DiscoverFilesAsync(), ShouldInclude(), ComputeHash(), GetFileCategory()                  ║
   ║ 🔗 DEPS: IFileDiscoveryService, DocGeneratorSettings | 📅 13/01/2026 | 👤 Copilot | 📝 v2.0         ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Services.DocGenerator.Interfaces;
using FrotiX.Services.DocGenerator.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FrotiX.Services.DocGenerator
{
    /// <summary>
    /// Serviço de descoberta de arquivos no projeto
    /// </summary>
    public class FileDiscoveryService : IFileDiscoveryService
    {
        private readonly IWebHostEnvironment _env;
        private readonly DocGeneratorSettings _settings;
        private readonly IDocCacheService _cacheService;
        private readonly ILogger<FileDiscoveryService> _logger;

        private DiscoveryResult? _lastResult;
        private FolderNode? _rootNode;

        public FileDiscoveryService(
            IWebHostEnvironment env,
            IOptions<DocGeneratorSettings> settings,
            IDocCacheService cacheService,
            ILogger<FileDiscoveryService> logger)
        {
            try
            {
                _env = env;
                _settings = settings.Value;
                _cacheService = cacheService;
                _logger = logger;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FileDiscoveryService.cs", ".ctor", error);
            }
        }

        public async Task<DiscoveryResult> DiscoverAsync(CancellationToken ct = default)
        {
            try
            {
                _logger.LogInformation("Iniciando descoberta de arquivos em {Path}", _env.ContentRootPath);

                var result = new DiscoveryResult();
                var allFiles = new List<DiscoveredFile>();

                // Descobrir arquivos usando glob patterns
                foreach (var pattern in _settings.IncludePatterns)
                {
                    var files = DiscoverByPattern(pattern, ct);
                    allFiles.AddRange(files);
                }

                // Remover duplicados
                allFiles = allFiles
                    .GroupBy(f => f.FullPath)
                    .Select(g => g.First())
                    .ToList();

                // Verificar quais precisam regeneração
                await _cacheService.LoadAsync(ct);

                foreach (var file in allFiles)
                {
                    file.ContentHash = ComputeHash(file.FullPath);
                    file.NeedsRegeneration = await _cacheService.NeedsRegenerationAsync(
                        file.RelativePath, file.ContentHash, ct);

                    // Verificar se já existe documentação
                    var docPath = GetExpectedDocPath(file);
                    file.HasExistingDoc = File.Exists(docPath);
                    file.ExistingDocPath = file.HasExistingDoc ? docPath : string.Empty;
                }

                // Construir árvore de pastas
                _rootNode = BuildFolderTree(allFiles);
                result.RootNode = _rootNode;

                // Estatísticas
                result.TotalFiles = allFiles.Count;
                result.FilesWithDocs = allFiles.Count(f => f.HasExistingDoc);
                result.FilesNeedingUpdate = allFiles.Count(f => f.NeedsRegeneration);
                result.FilesByCategory = allFiles
                    .GroupBy(f => f.Category)
                    .ToDictionary(g => g.Key, g => g.Count());

                _lastResult = result;

                _logger.LogInformation(
                    "Descoberta concluída: {Total} arquivos, {WithDocs} com docs, {NeedUpdate} precisam atualização",
                    result.TotalFiles, result.FilesWithDocs, result.FilesNeedingUpdate);

                return result;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FileDiscoveryService.cs", "DiscoverAsync", error);
                return new DiscoveryResult();
            }
        }

        public FolderNode GetFolderTree()
        {
            return _rootNode ?? new FolderNode { Name = "Root", Path = "/" };
        }

        public async Task<bool> NeedsRegenerationAsync(string filePath, CancellationToken ct = default)
        {
            try
            {
                var hash = ComputeHash(filePath);
                return await _cacheService.NeedsRegenerationAsync(filePath, hash, ct);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FileDiscoveryService.cs", "NeedsRegenerationAsync", error);
                return true;
            }
        }

        public string ComputeHash(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return string.Empty;

                using var sha256 = SHA256.Create();
                using var stream = File.OpenRead(filePath);
                var hash = sha256.ComputeHash(stream);
                return Convert.ToBase64String(hash);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FileDiscoveryService.cs", "ComputeHash", error);
                return string.Empty;
            }
        }

        public FileCategory DetectCategory(string filePath)
        {
            try
            {
                var relativePath = GetRelativePath(filePath);
                var extension = Path.GetExtension(filePath).ToLowerInvariant();
                var fileName = Path.GetFileName(filePath);

                // Por diretório
                if (relativePath.StartsWith("Controllers", StringComparison.OrdinalIgnoreCase))
                    return FileCategory.Controller;

                if (relativePath.StartsWith("Pages", StringComparison.OrdinalIgnoreCase))
                {
                    if (extension == ".cshtml")
                        return FileCategory.RazorPage;
                    if (fileName.EndsWith(".cshtml.cs", StringComparison.OrdinalIgnoreCase))
                        return FileCategory.RazorPageModel;
                }

                if (relativePath.StartsWith("Services", StringComparison.OrdinalIgnoreCase))
                    return FileCategory.Service;

                if (relativePath.StartsWith("Repository", StringComparison.OrdinalIgnoreCase))
                    return FileCategory.Repository;

                if (relativePath.StartsWith("Models", StringComparison.OrdinalIgnoreCase))
                    return FileCategory.Model;

                if (relativePath.StartsWith("ViewModels", StringComparison.OrdinalIgnoreCase))
                    return FileCategory.ViewModel;

                if (relativePath.StartsWith("Helpers", StringComparison.OrdinalIgnoreCase))
                    return FileCategory.Helper;

                if (relativePath.StartsWith("Middlewares", StringComparison.OrdinalIgnoreCase))
                    return FileCategory.Middleware;

                if (relativePath.StartsWith("Hubs", StringComparison.OrdinalIgnoreCase))
                    return FileCategory.Hub;

                if (relativePath.StartsWith("Data", StringComparison.OrdinalIgnoreCase))
                    return FileCategory.Data;

                // Por extensão
                if (extension == ".js")
                    return FileCategory.JavaScript;

                if (extension == ".css")
                    return FileCategory.Css;

                return FileCategory.Other;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FileDiscoveryService.cs", "DetectCategory", error);
                return FileCategory.Other;
            }
        }

        private IEnumerable<DiscoveredFile> DiscoverByPattern(string pattern, CancellationToken ct)
        {
            var files = new List<DiscoveredFile>();

            try
            {
                var matcher = new Matcher();
                matcher.AddInclude(pattern);

                foreach (var excludePattern in _settings.ExcludePatterns)
                {
                    matcher.AddExclude(excludePattern);
                }

                var result = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(_env.ContentRootPath)));

                foreach (var match in result.Files)
                {
                    ct.ThrowIfCancellationRequested();

                    var fullPath = Path.Combine(_env.ContentRootPath, match.Path);
                    var fileInfo = new FileInfo(fullPath);

                    if (!fileInfo.Exists)
                        continue;

                    var discovered = new DiscoveredFile
                    {
                        FullPath = fullPath,
                        RelativePath = match.Path.Replace('/', Path.DirectorySeparatorChar),
                        FileName = fileInfo.Name,
                        Extension = fileInfo.Extension,
                        FileSize = fileInfo.Length,
                        LastModified = fileInfo.LastWriteTimeUtc,
                        Category = DetectCategory(fullPath)
                    };

                    files.Add(discovered);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("FileDiscoveryService.cs", "DiscoverByPattern", error);
            }

            return files;
        }

        private FolderNode BuildFolderTree(List<DiscoveredFile> files)
        {
            var root = new FolderNode
            {
                Name = "FrotiX.Site",
                Path = "/",
                IsExpanded = true
            };

            foreach (var file in files)
            {
                var parts = file.RelativePath.Split(Path.DirectorySeparatorChar, '/');
                var currentNode = root;

                // Navegar/criar estrutura de pastas
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    var folderName = parts[i];
                    var existingFolder = currentNode.Children.FirstOrDefault(
                        c => c.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase));

                    if (existingFolder == null)
                    {
                        existingFolder = new FolderNode
                        {
                            Name = folderName,
                            Path = string.Join("/", parts.Take(i + 1)),
                            Category = DetectFolderCategory(folderName)
                        };
                        currentNode.Children.Add(existingFolder);
                    }

                    currentNode = existingFolder;
                }

                // Adicionar arquivo ao nó atual
                currentNode.Files.Add(file);
            }

            // Calcular totais
            CalculateTotals(root);

            return root;
        }

        private void CalculateTotals(FolderNode node)
        {
            node.TotalFiles = node.Files.Count;

            foreach (var child in node.Children)
            {
                CalculateTotals(child);
                node.TotalFiles += child.TotalFiles;
            }
        }

        private FileCategory? DetectFolderCategory(string folderName)
        {
            return folderName.ToLowerInvariant() switch
            {
                "controllers" => FileCategory.Controller,
                "pages" => FileCategory.RazorPage,
                "services" => FileCategory.Service,
                "repository" => FileCategory.Repository,
                "irepository" => FileCategory.Repository,
                "models" => FileCategory.Model,
                "viewmodels" => FileCategory.ViewModel,
                "helpers" => FileCategory.Helper,
                "middlewares" => FileCategory.Middleware,
                "hubs" => FileCategory.Hub,
                "data" => FileCategory.Data,
                "js" => FileCategory.JavaScript,
                "css" => FileCategory.Css,
                _ => null
            };
        }

        private string GetRelativePath(string fullPath)
        {
            return Path.GetRelativePath(_env.ContentRootPath, fullPath);
        }

        private string GetExpectedDocPath(DiscoveredFile file)
        {
            // Caminho de documentação espelha a estrutura do projeto
            var docRoot = Path.Combine(_env.ContentRootPath, _settings.OutputPath);
            var relativePath = file.RelativePath;

            // Trocar extensão para .md
            var mdPath = Path.ChangeExtension(relativePath, ".md");

            return Path.Combine(docRoot, mdPath);
        }
    }

    /// <summary>
    /// Wrapper para DirectoryInfo compatível com Microsoft.Extensions.FileSystemGlobbing
    /// </summary>
    internal class DirectoryInfoWrapper : Microsoft.Extensions.FileSystemGlobbing.Abstractions.DirectoryInfoBase
    {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryInfoWrapper(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo;
        }

        public override string Name => _directoryInfo.Name;
        public override string FullName => _directoryInfo.FullName;
        public override Microsoft.Extensions.FileSystemGlobbing.Abstractions.DirectoryInfoBase? ParentDirectory =>
            _directoryInfo.Parent != null ? new DirectoryInfoWrapper(_directoryInfo.Parent) : null;

        public override IEnumerable<Microsoft.Extensions.FileSystemGlobbing.Abstractions.FileSystemInfoBase> EnumerateFileSystemInfos()
        {
            if (!_directoryInfo.Exists)
                yield break;

            foreach (var fileInfo in _directoryInfo.EnumerateFiles())
            {
                yield return new FileInfoWrapper(fileInfo);
            }

            foreach (var dirInfo in _directoryInfo.EnumerateDirectories())
            {
                yield return new DirectoryInfoWrapper(dirInfo);
            }
        }

        public override Microsoft.Extensions.FileSystemGlobbing.Abstractions.DirectoryInfoBase GetDirectory(string path)
        {
            return new DirectoryInfoWrapper(new DirectoryInfo(Path.Combine(_directoryInfo.FullName, path)));
        }

        public override Microsoft.Extensions.FileSystemGlobbing.Abstractions.FileInfoBase GetFile(string path)
        {
            return new FileInfoWrapper(new FileInfo(Path.Combine(_directoryInfo.FullName, path)));
        }
    }

    /// <summary>
    /// Wrapper para FileInfo compatível com Microsoft.Extensions.FileSystemGlobbing
    /// </summary>
    internal class FileInfoWrapper : Microsoft.Extensions.FileSystemGlobbing.Abstractions.FileInfoBase
    {
        private readonly FileInfo _fileInfo;

        public FileInfoWrapper(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public override string Name => _fileInfo.Name;
        public override string FullName => _fileInfo.FullName;
        public override Microsoft.Extensions.FileSystemGlobbing.Abstractions.DirectoryInfoBase? ParentDirectory =>
            _fileInfo.Directory != null ? new DirectoryInfoWrapper(_fileInfo.Directory) : null;
    }
}
