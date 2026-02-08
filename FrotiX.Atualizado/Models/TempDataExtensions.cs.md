# Models/TempDataExtensions.cs

**Mudanca:** PEQUENA | **+0** linhas | **-0** linhas

---

```diff
--- JANEIRO: Models/TempDataExtensions.cs
+++ ATUAL: Models/TempDataExtensions.cs
@@ -6,6 +6,7 @@
 
     public static class TempDataExtensions
         {
+
         public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
             {
             tempData[key] = JsonConvert.SerializeObject(value);
```
