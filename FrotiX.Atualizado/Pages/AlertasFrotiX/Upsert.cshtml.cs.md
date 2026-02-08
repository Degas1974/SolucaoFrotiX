# Pages/AlertasFrotiX/Upsert.cshtml.cs

**Mudanca:** GRANDE | **+6** linhas | **-32** linhas

---

```diff
--- JANEIRO: Pages/AlertasFrotiX/Upsert.cshtml.cs
+++ ATUAL: Pages/AlertasFrotiX/Upsert.cshtml.cs
@@ -13,7 +13,6 @@
 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
-using System.Globalization;
 using System.Linq;
 using System.Threading.Tasks;
 
@@ -97,7 +96,7 @@
 
         public List<SelectListItem> VeiculosList { get; set; }
 
-        public List<UsuarioDropdownItem> UsuariosList { get; set; }
+        public List<SelectListItem> UsuariosList { get; set; }
 
         public List<SelectListItem> DiasSemanaList { get; set; }
 
@@ -116,14 +115,6 @@
         {
             public Guid ManutencaoId { get; set; }
             public string NumOS { get; set; }
-        }
-
-        public class UsuarioDropdownItem
-        {
-            public string Id { get; set; }
-            public string Ponto { get; set; }
-            public string NomeCompleto { get; set; }
-            public string NomeCamelCase { get; set; }
         }
 
         public UpsertModel(
@@ -679,23 +670,13 @@
                     .ToList();
 
                 var usuarios = await _context.AspNetUsers
-                    .OrderBy(u => u.NomeCompleto ?? u.UserName)
-                    .ThenBy(u => u.UserName)
+                    .OrderBy(u => u.UserName)
                     .ToListAsync();
 
-                UsuariosList = usuarios.Select(u =>
-                {
-                    var nome = !string.IsNullOrWhiteSpace(u.NomeCompleto)
-                        ? u.NomeCompleto
-                        : u.UserName ?? string.Empty;
-
-                    return new UsuarioDropdownItem
-                    {
-                        Id = u.Id,
-                        Ponto = u.Ponto,
-                        NomeCompleto = nome,
-                        NomeCamelCase = ToCamelCaseNome(nome)
-                    };
+                UsuariosList = usuarios.Select(u => new SelectListItem
+                {
+                    Text = u.UserName,
+                    Value = u.Id
                 }).ToList();
 
                 var motoristas = await _unitOfWork.ViewMotoristasViagem.GetAllAsync();
@@ -766,17 +747,6 @@
                 Alerta.TratamentoErroComLinha("Upsert.cshtml.cs", "CarregarListas", error);
             }
         }
-
-        private static string ToCamelCaseNome(string valor)
-        {
-            if (string.IsNullOrWhiteSpace(valor))
-            {
-                return string.Empty;
-            }
-
-            var textInfo = CultureInfo.CurrentCulture.TextInfo;
-            return textInfo.ToTitleCase(valor.ToLowerInvariant());
-        }
     }
 
     public static class EnumExtensions
```

### REMOVER do Janeiro

```csharp
using System.Globalization;
        public List<UsuarioDropdownItem> UsuariosList { get; set; }
        }
        public class UsuarioDropdownItem
        {
            public string Id { get; set; }
            public string Ponto { get; set; }
            public string NomeCompleto { get; set; }
            public string NomeCamelCase { get; set; }
                    .OrderBy(u => u.NomeCompleto ?? u.UserName)
                    .ThenBy(u => u.UserName)
                UsuariosList = usuarios.Select(u =>
                {
                    var nome = !string.IsNullOrWhiteSpace(u.NomeCompleto)
                        ? u.NomeCompleto
                        : u.UserName ?? string.Empty;
                    return new UsuarioDropdownItem
                    {
                        Id = u.Id,
                        Ponto = u.Ponto,
                        NomeCompleto = nome,
                        NomeCamelCase = ToCamelCaseNome(nome)
                    };
        private static string ToCamelCaseNome(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return string.Empty;
            }
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(valor.ToLowerInvariant());
        }
```


### ADICIONAR ao Janeiro

```csharp
        public List<SelectListItem> UsuariosList { get; set; }
                    .OrderBy(u => u.UserName)
                UsuariosList = usuarios.Select(u => new SelectListItem
                {
                    Text = u.UserName,
                    Value = u.Id
```
