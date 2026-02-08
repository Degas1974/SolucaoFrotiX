# Controllers/ContratoController.VerificarDependencias.cs

**Mudanca:** GRANDE | **+50** linhas | **-10** linhas

---

```diff
--- JANEIRO: Controllers/ContratoController.VerificarDependencias.cs
+++ ATUAL: Controllers/ContratoController.VerificarDependencias.cs
@@ -4,6 +4,7 @@
 
 namespace FrotiX.Controllers
 {
+
     public partial class ContratoController
     {
 
@@ -22,13 +23,61 @@
             try
             {
 
-                try { veiculosContrato = _unitOfWork.VeiculoContrato.GetAll(x => x.ContratoId == id).Count(); } catch { }
-                try { encarregados = _unitOfWork.Encarregado.GetAll(x => x.ContratoId == id).Count(); } catch { }
-                try { operadores = _unitOfWork.Operador.GetAll(x => x.ContratoId == id).Count(); } catch { }
-                try { lavadores = _unitOfWork.Lavador.GetAll(x => x.ContratoId == id).Count(); } catch { }
-                try { motoristas = _unitOfWork.Motorista.GetAll(x => x.ContratoId == id).Count(); } catch { }
-                try { empenhos = _unitOfWork.Empenho.GetAll(x => x.ContratoId == id).Count(); } catch { }
-                try { notasFiscais = _unitOfWork.NotaFiscal.GetAll(x => x.ContratoId == id).Count(); } catch { }
+                try
+                {
+                    veiculosContrato = _unitOfWork.VeiculoContrato
+                        .GetAll(x => x.ContratoId == id)
+                        .Count();
+                }
+                catch { }
+
+                try
+                {
+                    encarregados = _unitOfWork.Encarregado
+                        .GetAll(x => x.ContratoId == id)
+                        .Count();
+                }
+                catch { }
+
+                try
+                {
+                    operadores = _unitOfWork.Operador
+                        .GetAll(x => x.ContratoId == id)
+                        .Count();
+                }
+                catch { }
+
+                try
+                {
+                    lavadores = _unitOfWork.Lavador
+                        .GetAll(x => x.ContratoId == id)
+                        .Count();
+                }
+                catch { }
+
+                try
+                {
+                    motoristas = _unitOfWork.Motorista
+                        .GetAll(x => x.ContratoId == id)
+                        .Count();
+                }
+                catch { }
+
+                try
+                {
+                    empenhos = _unitOfWork.Empenho
+                        .GetAll(x => x.ContratoId == id)
+                        .Count();
+                }
+                catch { }
+
+                try
+                {
+                    notasFiscais = _unitOfWork.NotaFiscal
+                        .GetAll(x => x.ContratoId == id)
+                        .Count();
+                }
+                catch { }
 
                 var possuiDependencias = veiculosContrato > 0 || encarregados > 0 ||
                                          operadores > 0 || lavadores > 0 || motoristas > 0 ||
@@ -47,10 +96,8 @@
                     notasFiscais = notasFiscais
                 });
             }
-            catch (Exception ex)
+            catch (System.Exception ex)
             {
-                _log.Error(ex.Message, ex, "ContratoController.VerificarDependencias.cs", "VerificarDependencias");
-                Alerta.TratamentoErroComLinha("ContratoController.VerificarDependencias.cs", "VerificarDependencias", ex);
                 return Json(new
                 {
                     success = false,
```

### REMOVER do Janeiro

```csharp
                try { veiculosContrato = _unitOfWork.VeiculoContrato.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { encarregados = _unitOfWork.Encarregado.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { operadores = _unitOfWork.Operador.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { lavadores = _unitOfWork.Lavador.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { motoristas = _unitOfWork.Motorista.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { empenhos = _unitOfWork.Empenho.GetAll(x => x.ContratoId == id).Count(); } catch { }
                try { notasFiscais = _unitOfWork.NotaFiscal.GetAll(x => x.ContratoId == id).Count(); } catch { }
            catch (Exception ex)
                _log.Error(ex.Message, ex, "ContratoController.VerificarDependencias.cs", "VerificarDependencias");
                Alerta.TratamentoErroComLinha("ContratoController.VerificarDependencias.cs", "VerificarDependencias", ex);
```


### ADICIONAR ao Janeiro

```csharp
                try
                {
                    veiculosContrato = _unitOfWork.VeiculoContrato
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }
                try
                {
                    encarregados = _unitOfWork.Encarregado
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }
                try
                {
                    operadores = _unitOfWork.Operador
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }
                try
                {
                    lavadores = _unitOfWork.Lavador
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }
                try
                {
                    motoristas = _unitOfWork.Motorista
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }
                try
                {
                    empenhos = _unitOfWork.Empenho
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }
                try
                {
                    notasFiscais = _unitOfWork.NotaFiscal
                        .GetAll(x => x.ContratoId == id)
                        .Count();
                }
                catch { }
            catch (System.Exception ex)
```
