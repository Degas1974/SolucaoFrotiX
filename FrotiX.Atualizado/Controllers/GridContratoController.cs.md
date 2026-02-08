# Controllers/GridContratoController.cs

**Mudanca:** GRANDE | **+58** linhas | **-30** linhas

---

```diff
--- JANEIRO: Controllers/GridContratoController.cs
+++ ATUAL: Controllers/GridContratoController.cs
@@ -1,6 +1,4 @@
 using FrotiX.Repository.IRepository;
-using FrotiX.Services;
-using FrotiX.Helpers;
 using Microsoft.AspNetCore.Mvc;
 using System;
 using System.Collections.Generic;
@@ -8,32 +6,36 @@
 
 namespace FrotiX.Controllers
 {
-
     [Route("api/[controller]")]
     [ApiController]
     [IgnoreAntiforgeryToken]
-    public class GridContratoController : Controller
+    public class GridContratoController :Controller
     {
         private readonly IUnitOfWork _unitOfWork;
-        private readonly ILogService _log;
+
         public static List<ItensVeiculo> veiculo = new List<ItensVeiculo>();
 
         public class objItem
         {
-            public Guid RepactuacaoContratoId { get; set; }
+            Guid RepactuacaoContratoId
+            {
+                get; set;
+            }
         }
 
-        public GridContratoController(IUnitOfWork unitOfWork, ILogService log)
+        public GridContratoController(IUnitOfWork unitOfWork)
         {
             try
             {
                 _unitOfWork = unitOfWork;
-                _log = log;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("GridContratoController.cs", "GridContratoController", error);
-                throw;
+                Alerta.TratamentoErroComLinha(
+                    "GridContratoController.cs" ,
+                    "GridContratoController" ,
+                    error
+                );
             }
         }
 
@@ -43,29 +45,34 @@
         {
             try
             {
-
                 var veiculo = ItensVeiculo.GetAllRecords(_unitOfWork);
 
                 return Json(veiculo);
             }
             catch (Exception error)
             {
-                _log.Error(error.Message, error, "GridContratoController.cs", "DataSource");
-                Alerta.TratamentoErroComLinha("GridContratoController.cs", "DataSource", error);
-                return StatusCode(500, new { success = false, message = "Erro ao carregar dados" });
+                Alerta.TratamentoErroComLinha("GridContratoController.cs" , "DataSource" , error);
+                return View();
             }
         }
     }
 
     public class ItensVeiculo
     {
+
         public static List<ItensVeiculo> veiculo = new List<ItensVeiculo>();
 
-        public ItensVeiculo(int numitem, string descricao, int quantidade, double valorunitario, double valortotal, Guid repactuacaoId)
+        public ItensVeiculo(
+            int numitem ,
+            string descricao ,
+            int quantidade ,
+            double valorunitario ,
+            double valortotal ,
+            Guid repactuacaoId
+        )
         {
             try
             {
-
                 this.numitem = numitem;
                 this.descricao = descricao;
                 this.quantidade = quantidade;
@@ -75,7 +82,7 @@
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("GridContratoController.cs", "ItensVeiculo", error);
+                Alerta.TratamentoErroComLinha("GridContratoController.cs" , "ItensVeiculo" , error);
             }
         }
 
@@ -83,7 +90,6 @@
         {
             try
             {
-
                 var objItemVeiculos = _unitOfWork
                     .ItemVeiculoContrato.GetAll()
                     .OrderBy(o => o.NumItem);
@@ -92,31 +98,50 @@
 
                 foreach (var item in objItemVeiculos)
                 {
-
-                    veiculo.Add(new ItensVeiculo(
-                        (int)(item.NumItem ?? 0),
-                        item.Descricao,
-                        (int)(item.Quantidade ?? 0),
-                        (double)(item.ValorUnitario ?? 0),
-                        (double)((item.Quantidade ?? 0) * (item.ValorUnitario ?? 0)),
-                        item.RepactuacaoContratoId
-                    ));
+                    veiculo.Add(
+                        new ItensVeiculo(
+                            (int)item.NumItem ,
+                            item.Descricao ,
+                            (int)item.Quantidade ,
+                            (double)item.ValorUnitario ,
+                            (double)(item.Quantidade * item.ValorUnitario) ,
+                            item.RepactuacaoContratoId
+                        )
+                    );
                 }
 
                 return veiculo;
             }
             catch (Exception error)
             {
-                Alerta.TratamentoErroComLinha("GridContratoController.cs", "GetAllRecords", error);
-                return new List<ItensVeiculo>();
+                Alerta.TratamentoErroComLinha("GridContratoController.cs" , "GetAllRecords" , error);
+                return default(List<ItensVeiculo>);
             }
         }
 
-        public int? numitem { get; set; }
-        public string descricao { get; set; }
-        public int? quantidade { get; set; }
-        public double? valorunitario { get; set; }
-        public double? valortotal { get; set; }
-        public Guid repactuacaoId { get; set; }
+        public int? numitem
+        {
+            get; set;
+        }
+        public string descricao
+        {
+            get; set;
+        }
+        public int? quantidade
+        {
+            get; set;
+        }
+        public double? valorunitario
+        {
+            get; set;
+        }
+        public double? valortotal
+        {
+            get; set;
+        }
+        public Guid repactuacaoId
+        {
+            get; set;
+        }
     }
 }
```
