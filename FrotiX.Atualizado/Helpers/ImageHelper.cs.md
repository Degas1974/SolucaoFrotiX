# Helpers/ImageHelper.cs

**Mudanca:** MEDIA | **+4** linhas | **-4** linhas

---

```diff
--- JANEIRO: Helpers/ImageHelper.cs
+++ ATUAL: Helpers/ImageHelper.cs
@@ -6,11 +6,11 @@
 using System.Runtime.Versioning;
 
 namespace FrotiX.Helpers
-{
+    {
+    [SupportedOSPlatform("windows")]
 
-    [SupportedOSPlatform("windows")]
     public static class ImageHelper
-    {
+        {
 
         public static bool IsImageValid(byte[] imageData)
             {
@@ -49,7 +49,7 @@
                 }
             catch (Exception ex)
                 {
-                Alerta.TratamentoErroComLinha("ImageHelper.cs", "ResizeImage", ex);
+                Console.WriteLine($"[ImageHelper] Erro ao redimensionar imagem: {ex.Message}");
                 return null;
                 }
             }
```

### REMOVER do Janeiro

```csharp
{
    [SupportedOSPlatform("windows")]
    {
                Alerta.TratamentoErroComLinha("ImageHelper.cs", "ResizeImage", ex);
```


### ADICIONAR ao Janeiro

```csharp
    {
    [SupportedOSPlatform("windows")]
        {
                Console.WriteLine($"[ImageHelper] Erro ao redimensionar imagem: {ex.Message}");
```
