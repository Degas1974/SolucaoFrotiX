# Services/DocGenerator/Providers/BaseDocProvider.cs

**Mudanca:** PEQUENA | **+0** linhas | **-2** linhas

---

```diff
--- JANEIRO: Services/DocGenerator/Providers/BaseDocProvider.cs
+++ ATUAL: Services/DocGenerator/Providers/BaseDocProvider.cs
@@ -319,13 +319,8 @@
                 foreach (var func in js.Functions)
                 {
                     var asyncTag = func.IsAsync ? " `async`" : "";
-
-                    var paramsList = func.Parameters.Count > 0 ? string.Join(", ", func.Parameters) : "";
-                    sb.AppendLine($"- `{func.Name}({paramsList})`{asyncTag}");
-
                     var funcParams = func.Parameters.Count > 0 ? string.Join(", ", func.Parameters) : "";
                     sb.AppendLine($"- `{func.Name}({funcParams})`{asyncTag}");
-
                 }
                 sb.AppendLine();
             }
```

### REMOVER do Janeiro

```csharp
                    var paramsList = func.Parameters.Count > 0 ? string.Join(", ", func.Parameters) : "";
                    sb.AppendLine($"- `{func.Name}({paramsList})`{asyncTag}");
```

