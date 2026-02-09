using System.Text;
using System.Text.RegularExpressions;
using DeepSeekIDE.Core.Models;

namespace DeepSeekIDE.Core.Services;

/// <summary>
/// Serviço para manipulação de arquivos e diretórios locais
/// </summary>
public class FileSystemService
{
    private static readonly string[] ExcludedFolders = { "node_modules", "bin", "obj", ".git", ".vs", "packages", ".idea", "__pycache__" };
    private static readonly string[] ExcludedExtensions = { ".exe", ".dll", ".pdb", ".cache", ".suo" };
    private readonly LogService _logger = LogService.Instance;

    /// <summary>
    /// Obtém a estrutura de diretório de uma pasta
    /// </summary>
    public FileSystemItem GetDirectoryStructure(string path, int maxDepth = 3, int currentDepth = 0)
    {
        var info = new DirectoryInfo(path);

        var item = new FileSystemItem
        {
            Name = info.Name,
            FullPath = info.FullName,
            IsDirectory = true,
            LastModified = info.LastWriteTime
        };

        if (currentDepth >= maxDepth)
            return item;

        try
        {
            // Adiciona subdiretórios
            foreach (var dir in info.GetDirectories().OrderBy(d => d.Name))
            {
                if (ExcludedFolders.Contains(dir.Name, StringComparer.OrdinalIgnoreCase))
                    continue;

                if (dir.Attributes.HasFlag(FileAttributes.Hidden))
                    continue;

                var childItem = GetDirectoryStructure(dir.FullName, maxDepth, currentDepth + 1);
                item.Children.Add(childItem);
            }

            // Adiciona arquivos
            foreach (var file in info.GetFiles().OrderBy(f => f.Name))
            {
                if (ExcludedExtensions.Contains(file.Extension, StringComparer.OrdinalIgnoreCase))
                    continue;

                if (file.Attributes.HasFlag(FileAttributes.Hidden))
                    continue;

                item.Children.Add(new FileSystemItem
                {
                    Name = file.Name,
                    FullPath = file.FullName,
                    IsDirectory = false,
                    Size = file.Length,
                    LastModified = file.LastWriteTime
                });
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.Warning($"Acesso negado à pasta: {path}", "FileSystemService", ex);
        }
        catch (Exception ex)
        {
            _logger.Error($"Erro ao ler estrutura do diretório: {path}", "FileSystemService", ex);
        }

        return item;
    }

    /// <summary>
    /// Lê o conteúdo de um arquivo
    /// </summary>
    public async Task<string> ReadFileAsync(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Arquivo não encontrado: {path}");

        return await File.ReadAllTextAsync(path, Encoding.UTF8);
    }

    /// <summary>
    /// Salva o conteúdo em um arquivo
    /// </summary>
    public async Task SaveFileAsync(string path, string content)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(path, content, Encoding.UTF8);
    }

    /// <summary>
    /// Cria um novo arquivo
    /// </summary>
    public async Task<string> CreateFileAsync(string directory, string fileName, string content = "")
    {
        var path = Path.Combine(directory, fileName);
        await SaveFileAsync(path, content);
        return path;
    }

    /// <summary>
    /// Cria uma nova pasta
    /// </summary>
    public string CreateDirectory(string parentPath, string folderName)
    {
        var path = Path.Combine(parentPath, folderName);
        Directory.CreateDirectory(path);
        return path;
    }

    /// <summary>
    /// Deleta um arquivo ou pasta
    /// </summary>
    public void Delete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive: true);
        }
    }

    /// <summary>
    /// Renomeia um arquivo ou pasta
    /// </summary>
    public string Rename(string path, string newName)
    {
        var directory = Path.GetDirectoryName(path)!;
        var newPath = Path.Combine(directory, newName);

        if (File.Exists(path))
        {
            File.Move(path, newPath);
        }
        else if (Directory.Exists(path))
        {
            Directory.Move(path, newPath);
        }

        return newPath;
    }

    /// <summary>
    /// Copia um arquivo ou pasta
    /// </summary>
    public string Copy(string sourcePath, string destinationDirectory)
    {
        var name = Path.GetFileName(sourcePath);
        var destinationPath = Path.Combine(destinationDirectory, name);

        if (File.Exists(sourcePath))
        {
            File.Copy(sourcePath, destinationPath, overwrite: true);
        }
        else if (Directory.Exists(sourcePath))
        {
            CopyDirectory(sourcePath, destinationPath);
        }

        return destinationPath;
    }

    private void CopyDirectory(string sourceDir, string destinationDir)
    {
        Directory.CreateDirectory(destinationDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var fileName = Path.GetFileName(file);
            File.Copy(file, Path.Combine(destinationDir, fileName));
        }

        foreach (var dir in Directory.GetDirectories(sourceDir))
        {
            var dirName = Path.GetFileName(dir);
            CopyDirectory(dir, Path.Combine(destinationDir, dirName));
        }
    }

    /// <summary>
    /// Busca texto em arquivos
    /// </summary>
    public async Task<List<SearchResult>> SearchInFilesAsync(string directory, string searchText, bool caseSensitive = false, bool useRegex = false, string filePattern = "*.*")
    {
        var results = new List<SearchResult>();
        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        Regex? regex = useRegex ? new Regex(searchText, caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase) : null;

        var files = Directory.EnumerateFiles(directory, filePattern, SearchOption.AllDirectories)
            .Where(f => !ExcludedFolders.Any(ef => f.Contains(Path.DirectorySeparatorChar + ef + Path.DirectorySeparatorChar)))
            .Where(f => !ExcludedExtensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase));

        foreach (var file in files)
        {
            try
            {
                var lines = await File.ReadAllLinesAsync(file);

                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    int matchIndex = -1;
                    int matchLength = 0;

                    if (useRegex && regex != null)
                    {
                        var match = regex.Match(line);
                        if (match.Success)
                        {
                            matchIndex = match.Index;
                            matchLength = match.Length;
                        }
                    }
                    else
                    {
                        matchIndex = line.IndexOf(searchText, comparison);
                        matchLength = searchText.Length;
                    }

                    if (matchIndex >= 0)
                    {
                        results.Add(new SearchResult
                        {
                            FilePath = file,
                            FileName = Path.GetFileName(file),
                            LineNumber = i + 1,
                            LineContent = line.Trim(),
                            MatchStart = matchIndex,
                            MatchLength = matchLength
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Warning($"Não foi possível ler arquivo para busca: {file}", "FileSystemService", ex);
            }
        }

        return results;
    }

    /// <summary>
    /// Busca arquivos por nome
    /// </summary>
    public List<FileSystemItem> SearchFiles(string directory, string searchPattern)
    {
        var results = new List<FileSystemItem>();

        try
        {
            var files = Directory.EnumerateFiles(directory, $"*{searchPattern}*", SearchOption.AllDirectories)
                .Where(f => !ExcludedFolders.Any(ef => f.Contains(Path.DirectorySeparatorChar + ef + Path.DirectorySeparatorChar)))
                .Take(100); // Limita resultados

            foreach (var file in files)
            {
                var info = new FileInfo(file);
                results.Add(new FileSystemItem
                {
                    Name = info.Name,
                    FullPath = info.FullName,
                    IsDirectory = false,
                    Size = info.Length,
                    LastModified = info.LastWriteTime
                });
            }
        }
        catch (Exception ex)
        {
            _logger.Warning($"Erro ao buscar arquivos em: {directory}", "FileSystemService", ex);
        }

        return results;
    }

    /// <summary>
    /// Verifica se um caminho é válido
    /// </summary>
    public bool IsValidPath(string path) => Directory.Exists(path) || File.Exists(path);

    /// <summary>
    /// Obtém informações de um arquivo
    /// </summary>
    public FileSystemItem GetFileInfo(string path)
    {
        if (File.Exists(path))
        {
            var info = new FileInfo(path);
            return new FileSystemItem
            {
                Name = info.Name,
                FullPath = info.FullName,
                IsDirectory = false,
                Size = info.Length,
                LastModified = info.LastWriteTime
            };
        }
        else if (Directory.Exists(path))
        {
            var info = new DirectoryInfo(path);
            return new FileSystemItem
            {
                Name = info.Name,
                FullPath = info.FullName,
                IsDirectory = true,
                LastModified = info.LastWriteTime
            };
        }

        throw new FileNotFoundException($"Caminho não encontrado: {path}");
    }
}
