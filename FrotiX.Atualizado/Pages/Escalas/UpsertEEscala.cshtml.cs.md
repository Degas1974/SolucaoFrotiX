# Pages/Escalas/UpsertEEscala.cshtml.cs

**Mudanca:** GRANDE | **+1** linhas | **-35** linhas

---

```diff
--- JANEIRO: Pages/Escalas/UpsertEEscala.cshtml.cs
+++ ATUAL: Pages/Escalas/UpsertEEscala.cshtml.cs
@@ -1,7 +1,6 @@
 using System;
 using System.Collections.Generic;
 using System.Linq;
-using FrotiX.Helpers;
 using FrotiX.Models;
 using FrotiX.Repository.IRepository;
 using Microsoft.AspNetCore.Mvc;
@@ -19,100 +18,56 @@
             _unitOfWork = unitOfWork;
         }
 
-        [BindProperty]
         public Guid EscalaDiaId { get; set; }
 
-        [BindProperty]
         public Guid? MotoristaId { get; set; }
 
-        [BindProperty]
         public Guid? VeiculoId { get; set; }
 
-        [BindProperty]
         public Guid TipoServicoId { get; set; }
 
-        [BindProperty]
         public Guid TurnoId { get; set; }
 
-        [BindProperty]
         public DateTime DataEscala { get; set; }
 
-        [BindProperty]
         public string HoraInicio { get; set; } = "06:00";
 
-        [BindProperty]
         public string HoraFim { get; set; } = "18:00";
 
-        [BindProperty]
         public string? Lotacao { get; set; }
 
-        [BindProperty]
         public int NumeroSaidas { get; set; } = 0;
 
-        [BindProperty]
         public string StatusMotorista { get; set; } = "Disponível";
 
-        [BindProperty]
         public Guid? RequisitanteId { get; set; }
 
-        [BindProperty]
         public string? Observacoes { get; set; }
 
-        [BindProperty]
         public bool MotoristaIndisponivel { get; set; }
 
-        [BindProperty]
         public bool MotoristaEconomildo { get; set; }
 
-        [BindProperty]
         public bool MotoristaEmServico { get; set; }
 
-        [BindProperty]
         public bool MotoristaReservado { get; set; }
 
-        [BindProperty]
         public Guid? CoberturaId { get; set; }
-
-        [BindProperty]
         public Guid? MotoristaTitularId { get; set; }
-
-        [BindProperty]
         public Guid? MotoristaCobertorId { get; set; }
-
-        [BindProperty]
         public string? CategoriaIndisponibilidade { get; set; }
-
-        [BindProperty]
         public string? NomeMotoristaCobertor { get; set; }
-
-        [BindProperty]
         public string? NomeMotoristaTitular { get; set; }
-
-        [BindProperty]
         public DateTime? DataInicioIndisponibilidade { get; set; }
 
-        [BindProperty]
         public DateTime? DataFimIndisponibilidade { get; set; }
 
-        [BindProperty]
         public bool Segunda { get; set; }
-
-        [BindProperty]
         public bool Terca { get; set; }
-
-        [BindProperty]
         public bool Quarta { get; set; }
-
-        [BindProperty]
         public bool Quinta { get; set; }
-
-        [BindProperty]
         public bool Sexta { get; set; }
-
-        [BindProperty]
         public bool Sabado { get; set; }
-
-        [BindProperty]
         public bool Domingo { get; set; }
 
         public IEnumerable<SelectListItem> MotoristaList { get; set; } = new List<SelectListItem>();
@@ -178,6 +133,7 @@
                         CategoriaIndisponibilidade = escalaView.MotivoCobertura;
                         DataInicioIndisponibilidade = escalaView.DataInicio;
                         DataFimIndisponibilidade = escalaView.DataFim;
+
                     }
 
                     if (MotoristaId.HasValue)
@@ -209,6 +165,7 @@
                             Domingo = false;
                         }
                     }
+
                 }
 
                 CarregarDropdowns();
@@ -216,7 +173,6 @@
             }
             catch (Exception ex)
             {
-                Alerta.TratamentoErroComLinha("UpsertEEscalaModel.cs", "OnGet", ex);
                 TempData["erro"] = $"Erro ao carregar escala: {ex.Message}";
                 return RedirectToPage("./ListaEscala");
             }
@@ -253,6 +209,8 @@
 
                 var todosTipos = _unitOfWork.TipoServico.GetAll().ToList();
                 var tiposAtivos = todosTipos.Where(t => t.Ativo).ToList();
+
+                Console.WriteLine($"[DEBUG] Total de tipos ativos: {tiposAtivos.Count}");
 
                 var tiposFiltrados = tiposAtivos
                     .Where(t => t.NomeServico == "Economildo" || t.NomeServico == "Serviços Gerais")
@@ -301,10 +259,10 @@
                     new SelectListItem { Value = "Setor de Obras", Text = "Setor de Obras" },
                     new SelectListItem { Value = "Outros", Text = "Outros" }
                 };
+
             }
             catch (Exception ex)
             {
-                Alerta.TratamentoErroComLinha("UpsertEEscalaModel.cs", "CarregarDropdowns", ex);
                 TempData["erro"] = $"Erro ao carregar dropdowns: {ex.Message}";
             }
         }
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
        [BindProperty]
                Alerta.TratamentoErroComLinha("UpsertEEscalaModel.cs", "OnGet", ex);
                Alerta.TratamentoErroComLinha("UpsertEEscalaModel.cs", "CarregarDropdowns", ex);
```


### ADICIONAR ao Janeiro

```csharp
                Console.WriteLine($"[DEBUG] Total de tipos ativos: {tiposAtivos.Count}");
```
