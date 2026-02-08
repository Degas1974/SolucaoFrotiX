# Models/LogErro.cs

**ARQUIVO NOVO** | 109 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models;

[Table("LogErros")]
public class LogErro
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long LogErroId { get; set; }

    [Required]
    [Column(TypeName = "datetime2(3)")]
    public DateTime DataHora { get; set; } = DateTime.Now;

    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; } = "";

    [Required]
    [MaxLength(20)]
    public string Origem { get; set; } = "SERVER";

    [MaxLength(20)]
    public string? Nivel { get; set; }

    [MaxLength(100)]
    public string? Categoria { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Mensagem { get; set; } = "";

    [MaxLength(203)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string? MensagemCurta { get; set; }

    [MaxLength(500)]
    public string? Arquivo { get; set; }

    [MaxLength(200)]
    public string? Metodo { get; set; }

    public int? Linha { get; set; }

    public int? Coluna { get; set; }

    [MaxLength(200)]
    public string? ExceptionType { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? ExceptionMessage { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? StackTrace { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? InnerException { get; set; }

    [MaxLength(1000)]
    public string? Url { get; set; }

    [MaxLength(10)]
    public string? HttpMethod { get; set; }

    public int? StatusCode { get; set; }

    [MaxLength(500)]
    public string? UserAgent { get; set; }

    [MaxLength(45)]
    public string? IpAddress { get; set; }

    [MaxLength(100)]
    public string? Usuario { get; set; }

    [MaxLength(100)]
    public string? SessionId { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? DadosAdicionais { get; set; }

    [Required]
    public bool Resolvido { get; set; } = false;

    [Column(TypeName = "datetime2(3)")]
    public DateTime? DataResolucao { get; set; }

    [MaxLength(100)]
    public string? ResolvidoPor { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? Observacoes { get; set; }

    [MaxLength(64)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string? HashErro { get; set; }

    [Required]
    [Column(TypeName = "datetime2(3)")]
    public DateTime CriadoEm { get; set; } = DateTime.Now;

    [NotMapped]
    public bool IsCliente => Origem?.Equals("CLIENT", StringComparison.OrdinalIgnoreCase) ?? false;

    [NotMapped]
    public bool IsServidor => Origem?.Equals("SERVER", StringComparison.OrdinalIgnoreCase) ?? false;

    [NotMapped]
    public bool IsCritico =>
        (Tipo?.Contains("ERROR", StringComparison.OrdinalIgnoreCase) ?? false) ||
        (Nivel?.Equals("Critical", StringComparison.OrdinalIgnoreCase) ?? false);

    [NotMapped]
    public bool IsConsole => Tipo?.StartsWith("CONSOLE-", StringComparison.OrdinalIgnoreCase) ?? false;

    [NotMapped]
    public string MensagemExibicao =>
        Mensagem?.Length > 200
            ? Mensagem.Substring(0, 200) + "..."
            : Mensagem ?? "";

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

    [NotMapped]
    public string Cor => Tipo switch
    {
        var t when t.Contains("ERROR") && !t.Contains("JS") => "#dc3545",
        "ERROR-JS" => "#6f42c1",
        var t when t.Contains("HTTP-ERROR") => "#fd7e14",
        var t when t.Contains("WARN") => "#ffc107",
        var t when t.Contains("INFO") => "#17a2b8",
        var t when t.Contains("CONSOLE") => "#9333ea",
        _ => "#6c757d"
    };
}
```
