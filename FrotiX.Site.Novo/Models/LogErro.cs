/* ****************************************************************************************
 * ⚡ ARQUIVO: LogErro.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Armazenar logs de erros do servidor e do cliente no banco de dados.
 *
 * 📥 ENTRADAS     : Dados de erro, contexto HTTP e informações de usuário.
 *
 * 📤 SAÍDAS       : Registro persistido para auditoria, análise e dashboards.
 *
 * 🔗 CHAMADA POR  : LogService, LogRepository, LogErrosController.
 *
 * 🔄 CHAMA        : DataAnnotations, EF Core (Table/Column).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations,
 *                   System.ComponentModel.DataAnnotations.Schema.
 **************************************************************************************** */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models;

/****************************************************************************************
 * ⚡ MODEL: LogErro
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Representar registro de log de erro do sistema.
 *
 * 📥 ENTRADAS     : Mensagens de erro, contexto HTTP e dados de usuário.
 *
 * 📤 SAÍDAS       : Registro persistido para auditoria e análise.
 *
 * 🔗 CHAMADA POR  : LogService, LogRepository, LogErrosController.
 *
 * 🔄 CHAMA        : Não se aplica.
 ****************************************************************************************/
[Table("LogErros")]
public class LogErro
{
    // ====== IDENTIFICAÇÃO ======

    // ID único do log.
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long LogErroId { get; set; }

    // Data e hora do erro (precisão de milissegundos).
    [Required]
    [Column(TypeName = "datetime2(3)")]
    public DateTime DataHora { get; set; } = DateTime.Now;

    // ====== CLASSIFICAÇÃO ======

    // Tipo do log: ERROR, WARN, INFO, ERROR-JS, CONSOLE-INFO, CONSOLE-ERROR, HTTP-ERROR, etc.
    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; } = "";

    // Origem do log: SERVER (C#/ASP.NET) ou CLIENT (JavaScript/Console)
    [Required]
    [MaxLength(20)]
    public string Origem { get; set; } = "SERVER";

    // Nível do log: Critical, Error, Warning, Information, Debug
    [MaxLength(20)]
    public string? Nivel { get; set; }

    // Categoria: Controller, Service, Page, JavaScript, Console, etc.
    [MaxLength(100)]
    public string? Categoria { get; set; }

    // ====== MENSAGEM E DETALHES ======

    // Mensagem completa do erro.
    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Mensagem { get; set; } = "";

    // Mensagem curta (200 caracteres) - Campo computado no banco
    [MaxLength(203)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string? MensagemCurta { get; set; }

    // ====== LOCALIZAÇÃO DO ERRO (CÓDIGO) ======

    // Arquivo onde o erro ocorreu (ex: VeiculoController.cs, app.js).
    [MaxLength(500)]
    public string? Arquivo { get; set; }

    // Método/função onde o erro ocorreu.
    [MaxLength(200)]
    public string? Metodo { get; set; }

    // Número da linha do código.
    public int? Linha { get; set; }

    // Número da coluna (para JavaScript).
    public int? Coluna { get; set; }

    // ====== EXCEÇÃO (PARA ERROS DO SERVIDOR) ======

    // Tipo da exceção (ex: NullReferenceException, ArgumentException).
    [MaxLength(200)]
    public string? ExceptionType { get; set; }

    // Mensagem da exceção.
    [Column(TypeName = "nvarchar(max)")]
    public string? ExceptionMessage { get; set; }

    // Stack trace completo.
    [Column(TypeName = "nvarchar(max)")]
    public string? StackTrace { get; set; }

    // Inner exception (se houver).
    [Column(TypeName = "nvarchar(max)")]
    public string? InnerException { get; set; }

    // ====== CONTEXTO HTTP ======

    // URL onde o erro ocorreu.
    [MaxLength(1000)]
    public string? Url { get; set; }

    // Método HTTP (GET, POST, PUT, DELETE, etc.).
    [MaxLength(10)]
    public string? HttpMethod { get; set; }

    // Status HTTP (para erros HTTP: 404, 500, etc.).
    public int? StatusCode { get; set; }

    // User Agent (navegador/dispositivo).
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    // Endereço IP do usuário (suporta IPv6).
    [MaxLength(45)]
    public string? IpAddress { get; set; }

    // ====== USUÁRIO E SESSÃO ======

    // Nome/email do usuário logado quando o erro ocorreu.
    [MaxLength(100)]
    public string? Usuario { get; set; }

    // ID da sessão (para correlacionar múltiplos erros).
    [MaxLength(100)]
    public string? SessionId { get; set; }

    // ====== DADOS ADICIONAIS (JSON) ======

    // JSON com dados extras (formulários, estado, parâmetros, etc.)
    [Column(TypeName = "nvarchar(max)")]
    public string? DadosAdicionais { get; set; }

    // ====== RESOLUÇÃO ======

    // Indica se o erro foi resolvido/corrigido.
    [Required]
    public bool Resolvido { get; set; } = false;

    // Data em que o erro foi marcado como resolvido.
    [Column(TypeName = "datetime2(3)")]
    public DateTime? DataResolucao { get; set; }

    // Usuário que marcou o erro como resolvido.
    [MaxLength(100)]
    public string? ResolvidoPor { get; set; }

    // Observações sobre a resolução do erro.
    [Column(TypeName = "nvarchar(max)")]
    public string? Observacoes { get; set; }

    // ====== AGRUPAMENTO (PARA ANÁLISE) ======

    // Hash SHA256 para agrupar erros similares (mesmo tipo, arquivo, linha, mensagem curta)
    // Campo computado no banco
    [MaxLength(64)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string? HashErro { get; set; }

    // ====== AUDITORIA ======

    // Data de criação do registro no banco.
    [Required]
    [Column(TypeName = "datetime2(3)")]
    public DateTime CriadoEm { get; set; } = DateTime.Now;

    // ====== PROPRIEDADES CALCULADAS (NÃO MAPEADAS) ======

    // Indica se o log é do cliente (JavaScript/Console)
    [NotMapped]
    public bool IsCliente => Origem?.Equals("CLIENT", StringComparison.OrdinalIgnoreCase) ?? false;

    // Indica se o log é do servidor (C#/ASP.NET)
    [NotMapped]
    public bool IsServidor => Origem?.Equals("SERVER", StringComparison.OrdinalIgnoreCase) ?? false;

    // Indica se é um erro crítico (ERROR ou CRITICAL)
    [NotMapped]
    public bool IsCritico =>
        (Tipo?.Contains("ERROR", StringComparison.OrdinalIgnoreCase) ?? false) ||
        (Nivel?.Equals("Critical", StringComparison.OrdinalIgnoreCase) ?? false);

    // Indica se é um log de console do navegador
    [NotMapped]
    public bool IsConsole => Tipo?.StartsWith("CONSOLE-", StringComparison.OrdinalIgnoreCase) ?? false;

    // Retorna mensagem formatada para exibição (limita a 200 caracteres)
    [NotMapped]
    public string MensagemExibicao =>
        Mensagem?.Length > 200
            ? Mensagem.Substring(0, 200) + "..."
            : Mensagem ?? "";

    // Retorna ícone baseado no tipo do log
    [NotMapped]
    public string Icone => Tipo switch
    {
        var t when t.Contains("ERROR") => "fa-circle-exclamation",
        var t when t.Contains("WARN") => "fa-triangle-exclamation",
        var t when t.Contains("INFO") => "fa-circle-info",
        var t when t.Contains("CONSOLE") => "fa-browser",
        var t when t.Contains("HTTP") => "fa-globe",
        _ => "fa-file-lines"
    };

    // Retorna cor baseada no tipo do log
    [NotMapped]
    public string Cor => Tipo switch
    {
        var t when t.Contains("ERROR") && !t.Contains("JS") => "#dc3545", // Vermelho
        "ERROR-JS" => "#6f42c1",                                            // Roxo
        var t when t.Contains("HTTP-ERROR") => "#fd7e14",                  // Laranja
        var t when t.Contains("WARN") => "#ffc107",                        // Amarelo
        var t when t.Contains("INFO") => "#17a2b8",                        // Ciano
        var t when t.Contains("CONSOLE") => "#9333ea",                     // Roxo escuro
        _ => "#6c757d"                                                     // Cinza
    };
}
