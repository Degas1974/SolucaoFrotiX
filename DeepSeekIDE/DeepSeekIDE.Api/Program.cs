using DeepSeekIDE.Api.Models;
using DeepSeekIDE.Core.Models;
using DeepSeekIDE.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DeepSeek IDE API", Version = "v1" });
});

// Configuração do DeepSeek
var apiKey = builder.Configuration["DeepSeek:ApiKey"] ?? "sk-abe79be96b3347d6b07888636e5253b3";
builder.Services.AddSingleton(sp => new DeepSeekService(apiKey));
builder.Services.AddSingleton<FileSystemService>();

// CORS para permitir chamadas do App WPF
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

// ==================== ENDPOINTS ====================

// Health Check
app.MapGet("/api/status", async (DeepSeekService deepSeekService) =>
{
    var deepSeekAvailable = await deepSeekService.TestConnectionAsync();
    return new ApiStatusResponse
    {
        IsHealthy = true,
        Version = "1.0.0",
        DeepSeekApiAvailable = deepSeekAvailable,
        ServerTime = DateTime.UtcNow
    };
})
.WithName("GetStatus")
.WithTags("Status")
.WithOpenApi();

// Chat - Enviar mensagem
app.MapPost("/api/chat", async (ChatRequest request, DeepSeekService deepSeekService, CancellationToken ct) =>
{
    try
    {
        var messages = request.Messages.Select(m => new ChatMessage
        {
            Role = m.Role,
            Content = m.Content
        }).ToList();

        var response = await deepSeekService.SendMessageAsync(messages, request.Model, ct);

        return Results.Ok(new ChatResponse
        {
            Success = true,
            Content = response
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new ChatResponse
        {
            Success = false,
            Error = ex.Message
        });
    }
})
.WithName("SendChatMessage")
.WithTags("Chat")
.WithOpenApi();

// Análise de Código
app.MapPost("/api/code/analyze", async (CodeAnalysisRequest request, DeepSeekService deepSeekService, CancellationToken ct) =>
{
    try
    {
        string result = request.AnalysisType switch
        {
            CodeAnalysisType.Analyze => await deepSeekService.AnalyzeCodeAsync(request.Code, request.Language, ct),
            CodeAnalysisType.Explain => await deepSeekService.ExplainCodeAsync(request.Code, request.Language, ct),
            CodeAnalysisType.GenerateTests => await deepSeekService.GenerateTestsAsync(request.Code, request.Language, request.TestFramework ?? "", ct),
            CodeAnalysisType.Complete => await deepSeekService.GetCodeCompletionAsync(request.Code, request.Language, request.Instruction ?? "", ct),
            _ => throw new ArgumentException("Tipo de análise inválido")
        };

        return Results.Ok(new CodeAnalysisResponse
        {
            Success = true,
            Result = result
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new CodeAnalysisResponse
        {
            Success = false,
            Error = ex.Message
        });
    }
})
.WithName("AnalyzeCode")
.WithTags("Code")
.WithOpenApi();

// Ler arquivo
app.MapPost("/api/files/read", async (FileOperationRequest request, FileSystemService fileService) =>
{
    try
    {
        var content = await fileService.ReadFileAsync(request.Path);
        return Results.Ok(new FileOperationResponse
        {
            Success = true,
            Content = content
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new FileOperationResponse
        {
            Success = false,
            Error = ex.Message
        });
    }
})
.WithName("ReadFile")
.WithTags("Files")
.WithOpenApi();

// Salvar arquivo
app.MapPost("/api/files/save", async (FileOperationRequest request, FileSystemService fileService) =>
{
    try
    {
        await fileService.SaveFileAsync(request.Path, request.Content ?? "");
        return Results.Ok(new FileOperationResponse
        {
            Success = true
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new FileOperationResponse
        {
            Success = false,
            Error = ex.Message
        });
    }
})
.WithName("SaveFile")
.WithTags("Files")
.WithOpenApi();

// Listar diretório
app.MapGet("/api/files/list", (string path, FileSystemService fileService) =>
{
    try
    {
        var items = fileService.GetDirectoryStructure(path);
        return Results.Ok(new { Success = true, Items = items });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { Success = false, Error = ex.Message, Items = Array.Empty<object>() });
    }
})
.WithName("ListDirectory")
.WithTags("Files")
.WithOpenApi();

// Buscar em arquivos
app.MapPost("/api/files/search", async (SearchRequest request, FileSystemService fileService) =>
{
    try
    {
        // SearchInFilesAsync(directory, searchText, caseSensitive, useRegex, filePattern)
        var filePattern = request.FileExtensions?.Count > 0
            ? $"*{request.FileExtensions[0]}"
            : "*.*";

        var results = await fileService.SearchInFilesAsync(
            request.RootPath,
            request.Pattern,
            caseSensitive: false,
            useRegex: request.UseRegex,
            filePattern: filePattern
        );
        return Results.Ok(new { Success = true, Results = results });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { Success = false, Error = ex.Message, Results = Array.Empty<object>() });
    }
})
.WithName("SearchInFiles")
.WithTags("Files")
.WithOpenApi();

// Criar arquivo
app.MapPost("/api/files/create", async (FileOperationRequest request, FileSystemService fileService) =>
{
    try
    {
        // CreateFileAsync(directory, fileName, content)
        var directory = Path.GetDirectoryName(request.Path) ?? "";
        var fileName = Path.GetFileName(request.Path);
        await fileService.CreateFileAsync(directory, fileName, request.Content ?? "");
        return Results.Ok(new FileOperationResponse { Success = true });
    }
    catch (Exception ex)
    {
        return Results.Ok(new FileOperationResponse { Success = false, Error = ex.Message });
    }
})
.WithName("CreateFile")
.WithTags("Files")
.WithOpenApi();

// Deletar arquivo/pasta
app.MapDelete("/api/files/delete", (string path, FileSystemService fileService) =>
{
    try
    {
        fileService.Delete(path);
        return Results.Ok(new FileOperationResponse { Success = true });
    }
    catch (Exception ex)
    {
        return Results.Ok(new FileOperationResponse { Success = false, Error = ex.Message });
    }
})
.WithName("DeleteFile")
.WithTags("Files")
.WithOpenApi();

// Renomear arquivo/pasta
app.MapPost("/api/files/rename", (FileOperationRequest request, FileSystemService fileService) =>
{
    try
    {
        if (string.IsNullOrEmpty(request.NewPath))
            throw new ArgumentException("NewPath é obrigatório para renomear");

        fileService.Rename(request.Path, request.NewPath);
        return Results.Ok(new FileOperationResponse { Success = true });
    }
    catch (Exception ex)
    {
        return Results.Ok(new FileOperationResponse { Success = false, Error = ex.Message });
    }
})
.WithName("RenameFile")
.WithTags("Files")
.WithOpenApi();

app.Run();
