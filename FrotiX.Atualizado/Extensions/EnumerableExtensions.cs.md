# Extensions/EnumerableExtensions.cs

**Mudanca:** MEDIA | **+5** linhas | **-19** linhas

---

```diff
--- JANEIRO: Extensions/EnumerableExtensions.cs
+++ ATUAL: Extensions/EnumerableExtensions.cs
@@ -1,15 +1,15 @@
-using System;
 using System.Collections.Generic;
 using System.Diagnostics;
 using System.Linq;
 using System.Text.Json;
 using System.Text.Json.Serialization;
-using FrotiX.Helpers;
 
 namespace FrotiX.Extensions
 {
+
     public static class EnumerableExtensions
     {
+
         [DebuggerStepThrough]
         public static bool HasItems<T>(this IEnumerable<T> source) => source != null && source.Any();
 
@@ -22,18 +22,18 @@
         private static readonly JsonSerializerOptions DefaultSettings = SerializerSettings();
 
         private static JsonSerializerOptions SerializerSettings(bool indented = true)
+        {
+            var options = new JsonSerializerOptions
             {
-            var options = new JsonSerializerOptions
-                {
                 DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                 WriteIndented = indented,
                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
-                };
+            };
 
             options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
 
             return options;
-            }
+        }
 
         [DebuggerStepThrough]
         private static string Serialize<TTarget>(this TTarget source) => JsonSerializer.Serialize(source, DefaultSettings);
@@ -48,19 +48,6 @@
         public static TTarget MapTo<TSource, TTarget>(this TSource source) => source.Serialize().Deserialize<TTarget>();
 
         [DebuggerStepThrough]
-        public static IEnumerable<TTarget> MapTo<TSource, TTarget>(this IEnumerable<TSource> source)
-        {
-            try
-            {
-                if (source == null) return new List<TTarget>();
-                return source.Select(element => element.MapTo<TTarget>());
-            }
-            catch (Exception ex)
-            {
-
-                 Alerta.TratamentoErroComLinha("EnumerableExtensions.cs", "MapTo", ex);
-                 throw;
-            }
-        }
+        public static IEnumerable<TTarget> MapTo<TSource, TTarget>(this IEnumerable<TSource> source) => source.Select(element => element.MapTo<TTarget>());
     }
 }
```

### REMOVER do Janeiro

```csharp
using System;
using FrotiX.Helpers;
            var options = new JsonSerializerOptions
                {
                };
            }
        public static IEnumerable<TTarget> MapTo<TSource, TTarget>(this IEnumerable<TSource> source)
        {
            try
            {
                if (source == null) return new List<TTarget>();
                return source.Select(element => element.MapTo<TTarget>());
            }
            catch (Exception ex)
            {
                 Alerta.TratamentoErroComLinha("EnumerableExtensions.cs", "MapTo", ex);
                 throw;
            }
        }
```


### ADICIONAR ao Janeiro

```csharp
        {
            var options = new JsonSerializerOptions
            };
        }
        public static IEnumerable<TTarget> MapTo<TSource, TTarget>(this IEnumerable<TSource> source) => source.Select(element => element.MapTo<TTarget>());
```
