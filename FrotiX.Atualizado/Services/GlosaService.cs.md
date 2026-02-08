# Services/GlosaService.cs

**Mudanca:** GRANDE | **+17** linhas | **-17** linhas

---

```diff
--- JANEIRO: Services/GlosaService.cs
+++ ATUAL: Services/GlosaService.cs
@@ -4,38 +4,38 @@
 using FrotiX.Repository.IRepository;
 
 namespace FrotiX.Services
-{
+    {
 
     public class GlosaService : IGlosaService
-    {
+        {
         private readonly IUnitOfWork _uow;
 
         private class ResumoWork
-        {
+            {
             public int? NumItem { get; set; }
             public string Descricao { get; set; }
             public int Quantidade { get; set; }
             public decimal ValorUnitario { get; set; }
             public decimal ValorGlosa { get; set; }
-        }
+            }
 
         public GlosaService(IUnitOfWork uow)
-        {
+            {
             _uow = uow;
-        }
+            }
 
         public IEnumerable<GlosaResumoItemDto> ListarResumo(Guid contratoId, int mes, int ano)
-        {
+            {
 
             var baseQuery = _uow.ViewGlosa.GetAllReducedIQueryable(
                 selector: x => new ResumoWork
-                {
+                    {
                     NumItem = x.NumItem,
                     Descricao = x.Descricao,
                     Quantidade = x.Quantidade ?? 0,
                     ValorUnitario = (decimal)(x.ValorUnitario ?? 0d),
                     ValorGlosa = x.ValorGlosa,
-                },
+                    },
                 filter: x =>
                     x.ContratoId == contratoId
                     && x.DataSolicitacaoRaw.Month == mes
@@ -46,12 +46,10 @@
             var query = baseQuery
                 .GroupBy(g => new { g.NumItem, g.Descricao })
                 .Select(s => new GlosaResumoItemDto
-                {
+                    {
                     NumItem = s.Key.NumItem,
                     Descricao = s.Key.Descricao,
-
                     Quantidade = s.Max(i => (int?)i.Quantidade),
-
                     ValorUnitario = s.Max(i => i.ValorUnitario),
 
                     PrecoTotalMensal = (s.Max(i => i.Quantidade) * s.Max(i => i.ValorUnitario)),
@@ -63,27 +61,29 @@
                     ValorParaAteste =
                         (s.Max(i => i.Quantidade) * s.Max(i => i.ValorUnitario))
                         - s.Sum(i => i.ValorGlosa),
-                })
+                    })
                 .OrderBy(x => x.NumItem);
 
             return query.ToList();
-        }
+            }
 
         public IEnumerable<GlosaDetalheItemDto> ListarDetalhes(Guid contratoId, int mes, int ano)
-        {
+            {
 
             var query = _uow.ViewGlosa.GetAllReducedIQueryable(
                 selector: x => new GlosaDetalheItemDto
-                {
+                    {
                     NumItem = x.NumItem,
                     Descricao = x.Descricao,
                     Placa = x.Placa,
+
                     DataSolicitacao = x.DataSolicitacao,
                     DataDisponibilidade = x.DataDisponibilidade,
                     DataRecolhimento = x.DataRecolhimento,
                     DataDevolucao = x.DataDevolucao,
+
                     DiasGlosa = x.DiasGlosa,
-                },
+                    },
                 filter: x =>
                     x.ContratoId == contratoId
                     && x.DataSolicitacaoRaw.Month == mes
@@ -92,12 +92,12 @@
             );
 
             return query.ToList();
-        }
+            }
 
         IEnumerable<GlosaDetalheItemDto> IGlosaService.ListarDetalhes(
             Guid contratoId,
             int mes,
             int ano
         ) => ListarDetalhes(contratoId, mes, ano);
+        }
     }
-}
```

### REMOVER do Janeiro

```csharp
{
    {
        {
        }
        {
        }
        {
                {
                },
                {
                })
        }
        {
                {
                },
        }
}
```


### ADICIONAR ao Janeiro

```csharp
    {
        {
            {
            }
            {
            }
            {
                    {
                    },
                    {
                    })
            }
            {
                    {
                    },
            }
        }
```
