# Extensions/ToastExtensions.cs

**Mudanca:** GRANDE | **+15** linhas | **-25** linhas

---

```diff
--- JANEIRO: Extensions/ToastExtensions.cs
+++ ATUAL: Extensions/ToastExtensions.cs
@@ -2,47 +2,40 @@
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.AspNetCore.Mvc.RazorPages;
 using Microsoft.Extensions.DependencyInjection;
-using FrotiX.Helpers;
-using System;
 
 namespace FrotiX.Extensions
 {
+
     public static class ToastExtensions
     {
 
-        public static void ShowToast(this PageModel page , string texto , string cor = "Verde" , int duracao = 2000)
+        public static void ShowToast(this PageModel page, string texto, string cor = "Verde", int duracao = 2000)
         {
-            try
-            {
-                var toastService = page.HttpContext.RequestServices.GetService<IToastService>();
-                toastService?.Show(texto , cor , duracao);
-            }
-            catch (Exception ex) { Alerta.TratamentoErroComLinha("ToastExtensions.cs", "ShowToast_PageModel", ex); }
+
+            var toastService = page.HttpContext.RequestServices.GetService<IToastService>();
+            toastService?.Show(texto, cor, duracao);
         }
 
-        public static void ShowSuccess(this PageModel page , string texto , int duracao = 2000)
+        public static void ShowSuccess(this PageModel page, string texto, int duracao = 2000)
         {
-            page.ShowToast(texto , "Verde" , duracao);
+            page.ShowToast(texto, "Verde", duracao);
         }
 
-        public static void ShowError(this PageModel page , string texto , int duracao = 2000)
+        public static void ShowError(this PageModel page, string texto, int duracao = 2000)
         {
-            page.ShowToast(texto , "Vermelho" , duracao);
+            page.ShowToast(texto, "Vermelho", duracao);
         }
 
-        public static void ShowWarning(this PageModel page , string texto , int duracao = 2000)
+        public static void ShowWarning(this PageModel page, string texto, int duracao = 2000)
         {
-            page.ShowToast(texto , "Amarelo" , duracao);
+            page.ShowToast(texto, "Amarelo", duracao);
         }
 
-        public static void ShowToast(this Controller controller , string texto , string cor = "Verde" , int duracao = 2000)
+        public static void ShowToast(this Controller controller, string texto, string cor = "Verde", int duracao = 2000)
         {
-            try
-            {
-                var toastService = controller.HttpContext.RequestServices.GetService<IToastService>();
-                toastService?.Show(texto , cor , duracao);
-            }
-             catch (Exception ex) { Alerta.TratamentoErroComLinha("ToastExtensions.cs", "ShowToast_Controller", ex); }
-         }
-     }
- }
+
+            var toastService = controller.HttpContext.RequestServices.GetService<IToastService>();
+            toastService?.Show(texto, cor, duracao);
+        }
+    }
+}
```

### REMOVER do Janeiro

```csharp
using FrotiX.Helpers;
using System;
        public static void ShowToast(this PageModel page , string texto , string cor = "Verde" , int duracao = 2000)
            try
            {
                var toastService = page.HttpContext.RequestServices.GetService<IToastService>();
                toastService?.Show(texto , cor , duracao);
            }
            catch (Exception ex) { Alerta.TratamentoErroComLinha("ToastExtensions.cs", "ShowToast_PageModel", ex); }
        public static void ShowSuccess(this PageModel page , string texto , int duracao = 2000)
            page.ShowToast(texto , "Verde" , duracao);
        public static void ShowError(this PageModel page , string texto , int duracao = 2000)
            page.ShowToast(texto , "Vermelho" , duracao);
        public static void ShowWarning(this PageModel page , string texto , int duracao = 2000)
            page.ShowToast(texto , "Amarelo" , duracao);
        public static void ShowToast(this Controller controller , string texto , string cor = "Verde" , int duracao = 2000)
            try
            {
                var toastService = controller.HttpContext.RequestServices.GetService<IToastService>();
                toastService?.Show(texto , cor , duracao);
            }
             catch (Exception ex) { Alerta.TratamentoErroComLinha("ToastExtensions.cs", "ShowToast_Controller", ex); }
         }
     }
 }
```


### ADICIONAR ao Janeiro

```csharp
        public static void ShowToast(this PageModel page, string texto, string cor = "Verde", int duracao = 2000)
            var toastService = page.HttpContext.RequestServices.GetService<IToastService>();
            toastService?.Show(texto, cor, duracao);
        public static void ShowSuccess(this PageModel page, string texto, int duracao = 2000)
            page.ShowToast(texto, "Verde", duracao);
        public static void ShowError(this PageModel page, string texto, int duracao = 2000)
            page.ShowToast(texto, "Vermelho", duracao);
        public static void ShowWarning(this PageModel page, string texto, int duracao = 2000)
            page.ShowToast(texto, "Amarelo", duracao);
        public static void ShowToast(this Controller controller, string texto, string cor = "Verde", int duracao = 2000)
            var toastService = controller.HttpContext.RequestServices.GetService<IToastService>();
            toastService?.Show(texto, cor, duracao);
        }
    }
}
```
