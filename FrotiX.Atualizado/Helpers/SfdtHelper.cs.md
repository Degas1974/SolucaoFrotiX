# Helpers/SfdtHelper.cs

**Mudanca:** MEDIA | **+4** linhas | **-4** linhas

---

```diff
--- JANEIRO: Helpers/SfdtHelper.cs
+++ ATUAL: Helpers/SfdtHelper.cs
@@ -10,17 +10,15 @@
 using SkiaSharp;
 
 namespace FrotiX.Helpers
-{
+    {
 
     public static class SfdtHelper
-    {
+        {
 
         public static byte[] SalvarImagemDeDocx(byte[] docxBytes)
-        {
-
+            {
             using var docStream = new MemoryStream(docxBytes);
             using var document = new WordDocument(docStream, FormatType.Docx);
-
             using var renderer = new DocIORenderer();
             using var pdfDoc = renderer.ConvertToPDF(document);
 
@@ -34,11 +32,9 @@
             pdfRenderer.Load(input);
 
             using var bitmap = pdfRenderer.ExportAsImage(0);
-
             using var img = SKImage.FromBitmap(bitmap);
             using var encoded = img.Encode(SKEncodedImageFormat.Png, 100);
-
             return encoded.ToArray();
+            }
         }
     }
-}
```

### REMOVER do Janeiro

```csharp
{
    {
        {
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
            {
            }
```
