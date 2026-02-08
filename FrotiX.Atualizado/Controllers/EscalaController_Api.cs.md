# Controllers/EscalaController_Api.cs

**Mudanca:** PEQUENA | **+1** linhas | **-0** linhas

---

```diff
--- JANEIRO: Controllers/EscalaController_Api.cs
+++ ATUAL: Controllers/EscalaController_Api.cs
@@ -1,3 +1,5 @@
+using FrotiX.Helpers;
+
 using System;
 using System.Collections.Generic;
 using System.Linq;
@@ -11,7 +13,6 @@
 
 namespace FrotiX.Controllers
 {
-
     public partial class EscalaController : Controller
     {
 
@@ -159,7 +160,6 @@
         {
             try
             {
-
                 var query = _unitOfWork.ViewEscalasCompletas.GetAll();
 
                 if (dataFiltro.HasValue)
@@ -199,7 +199,6 @@
 
                 if (!string.IsNullOrWhiteSpace(textoPesquisa))
                 {
-
                     var texto = textoPesquisa.ToLower();
                     query = query.Where(e =>
                         (e.NomeMotorista != null && e.NomeMotorista.ToLower().Contains(texto)) ||
@@ -230,7 +229,6 @@
         {
             try
             {
-
                 var escalaExistente = _unitOfWork.ViewEscalasCompletas
                     .GetAll()
                     .Any(e => e.MotoristaId == motoristaId && e.DataEscala.Date == data.Date);
@@ -256,7 +254,6 @@
         {
             try
             {
-
                 var dataFiltro = data ?? DateTime.Today;
 
                 var escalaView = _unitOfWork.ViewEscalasCompletas
@@ -297,7 +294,6 @@
         {
             try
             {
-
                 var escala = _unitOfWork.EscalaDiaria.GetFirstOrDefault(e => e.EscalaDiaId == id);
 
                 if (escala == null)
@@ -324,7 +320,6 @@
         {
             try
             {
-
                 var escala = _unitOfWork.ViewEscalasCompletas.GetFirstOrDefault(e => e.EscalaDiaId == id);
 
                 if (escala == null)
```

### ADICIONAR ao Janeiro

```csharp
using FrotiX.Helpers;
```
