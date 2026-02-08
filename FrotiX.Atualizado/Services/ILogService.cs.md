# Services/ILogService.cs

**Mudanca:** PEQUENA | **+2** linhas | **-0** linhas

---

```diff
--- JANEIRO: Services/ILogService.cs
+++ ATUAL: Services/ILogService.cs
@@ -13,6 +13,8 @@
     void Error(string message, Exception? exception = null, string? arquivo = null, string? metodo = null, int? linha = null);
 
     void ErrorJS(string message, string? arquivo = null, string? metodo = null, int? linha = null, int? coluna = null, string? stack = null, string? userAgent = null, string? url = null);
+
+    void LogConsole(string tipo, string message, string? arquivo = null, string? metodo = null, int? linha = null, int? coluna = null, string? stack = null, string? userAgent = null, string? url = null);
 
     void Debug(string message, string? arquivo = null);
 
@@ -64,6 +66,7 @@
     public int InfoCount { get; set; }
     public int JSErrorCount { get; set; }
     public int HttpErrorCount { get; set; }
+    public int ConsoleCount { get; set; }
     public DateTime? FirstLogDate { get; set; }
     public DateTime? LastLogDate { get; set; }
 }
```

### ADICIONAR ao Janeiro

```csharp
    void LogConsole(string tipo, string message, string? arquivo = null, string? metodo = null, int? linha = null, int? coluna = null, string? stack = null, string? userAgent = null, string? url = null);
    public int ConsoleCount { get; set; }
```
