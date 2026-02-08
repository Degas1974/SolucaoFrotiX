# Cache/MotoristaCache.cs

**Mudanca:** GRANDE | **+33** linhas | **-36** linhas

---

```diff
--- JANEIRO: Cache/MotoristaCache.cs
+++ ATUAL: Cache/MotoristaCache.cs
@@ -2,43 +2,42 @@
 using global::FrotiX.Repository.IRepository;
 using Microsoft.EntityFrameworkCore;
 using Microsoft.Extensions.DependencyInjection;
-using FrotiX.Services;
 using System;
 using System.Collections.Generic;
 using System.Linq;
 
 namespace FrotiX.Cache
-{
+    {
 
     public class MotoristaCache
-    {
+        {
         private readonly IUnitOfWork _unitOfWork;
         private List<object> _cachedMotoristas;
         private readonly object _lock = new();
+
         private readonly IServiceScopeFactory _scopeFactory;
-        private readonly ILogService _log;
 
-        public MotoristaCache(IUnitOfWork unitOfWork, IServiceScopeFactory scopeFactory, ILogService log)
-        {
+        public MotoristaCache(IUnitOfWork unitOfWork, IServiceScopeFactory scopeFactory)
+            {
             _unitOfWork = unitOfWork;
             _scopeFactory = scopeFactory;
-            _log = log;
             LoadMotoristas();
-        }
+            }
 
         public void LoadMotoristas()
-        {
+            {
             try
             {
                 lock (_lock)
                 {
+
                     var motoristas = _unitOfWork.ViewMotoristasViagem.GetAllReduced(
                         selector: m => new
-                        {
+                            {
                             m.MotoristaId,
                             Nome = m.MotoristaCondutor,
                             m.Foto
-                        },
+                            },
                         orderBy: q => q.OrderBy(m => m.MotoristaCondutor)
                     ).ToList();
 
@@ -47,64 +46,73 @@
                         string fotoBase64;
 
                         if (m.Foto != null && m.Foto.Length > 0)
-                        {
+                            {
                             try
+                                {
+
+                                fotoBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(m.Foto)}";
+                                }
+                            catch
+                                {
+
+                                fotoBase64 = "/images/barbudo.jpg";
+                                }
+                            }
+                        else
                             {
-                                fotoBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(m.Foto)}";
+
+                            fotoBase64 = "/images/barbudo.jpg";
                             }
-                            catch
-                            {
-                                fotoBase64 = "/images/barbudo.jpg";
-                            }
-                        }
-                        else
-                        {
-                            fotoBase64 = "/images/barbudo.jpg";
-                        }
 
                         return new
-                        {
+                            {
                             m.MotoristaId,
                             Nome = m.Nome,
                             Foto = fotoBase64
-                        };
+                            };
                     }).Cast<object>().ToList();
                 }
-
-                _log.Info($"Cache de Motoristas carregado com sucesso: {_cachedMotoristas?.Count} registros.");
             }
             catch (Exception ex)
             {
-                _log.Error("Falha ao carregar cache de motoristas", ex);
+
+                Alerta.TratamentoErroComLinha("MotoristaCache.cs", "LoadMotoristas", ex);
+                _cachedMotoristas = new List<object>();
             }
-        }
+            }
 
         public List<object> GetMotoristas()
-        {
+            {
             try
             {
+
                 return _cachedMotoristas?.Select(m =>
                 {
                     dynamic motorista = m;
+
                     if (string.IsNullOrWhiteSpace(motorista.Foto))
-                    {
+                        {
                         motorista.Foto = "/images/barbudo.jpg";
-                    }
+                        }
                     return motorista;
                 }).ToList<object>();
             }
             catch (Exception ex)
             {
-                _log.Error("Erro ao recuperar motoristas do cache", ex);
+
+                Alerta.TratamentoErroComLinha("MotoristaCache.cs", "GetMotoristas", ex);
                 return new List<object>();
+            }
             }
         }
     }
 
-    public class MotoristaDto
+public class MotoristaDto
     {
-        public Guid MotoristaId { get; set; }
-        public string Nome { get; set; }
-        public string Foto { get; set; }
+
+    public Guid MotoristaId { get; set; }
+
+    public string Nome { get; set; }
+
+    public string Foto { get; set; }
     }
-}
```

### REMOVER do Janeiro

```csharp
using FrotiX.Services;
{
    {
        private readonly ILogService _log;
        public MotoristaCache(IUnitOfWork unitOfWork, IServiceScopeFactory scopeFactory, ILogService log)
        {
            _log = log;
        }
        {
                        {
                        },
                        {
                                fotoBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(m.Foto)}";
                            catch
                            {
                                fotoBase64 = "/images/barbudo.jpg";
                            }
                        }
                        else
                        {
                            fotoBase64 = "/images/barbudo.jpg";
                        }
                        {
                        };
                _log.Info($"Cache de Motoristas carregado com sucesso: {_cachedMotoristas?.Count} registros.");
                _log.Error("Falha ao carregar cache de motoristas", ex);
        }
        {
                    {
                    }
                _log.Error("Erro ao recuperar motoristas do cache", ex);
    public class MotoristaDto
        public Guid MotoristaId { get; set; }
        public string Nome { get; set; }
        public string Foto { get; set; }
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
        public MotoristaCache(IUnitOfWork unitOfWork, IServiceScopeFactory scopeFactory)
            {
            }
            {
                            {
                            },
                            {
                                {
                                fotoBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(m.Foto)}";
                                }
                            catch
                                {
                                fotoBase64 = "/images/barbudo.jpg";
                                }
                            }
                        else
                            fotoBase64 = "/images/barbudo.jpg";
                            {
                            };
                Alerta.TratamentoErroComLinha("MotoristaCache.cs", "LoadMotoristas", ex);
                _cachedMotoristas = new List<object>();
            }
            {
                        {
                        }
                Alerta.TratamentoErroComLinha("MotoristaCache.cs", "GetMotoristas", ex);
            }
public class MotoristaDto
    public Guid MotoristaId { get; set; }
    public string Nome { get; set; }
    public string Foto { get; set; }
```
