# Pages/Administracao/DashboardAdministracao.cshtml

**Mudanca:** GRANDE | **+17** linhas | **-33** linhas

---

```diff
--- JANEIRO: Pages/Administracao/DashboardAdministracao.cshtml
+++ ATUAL: Pages/Administracao/DashboardAdministracao.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Administracao.DashboardAdministracaoModel
 @{
     ViewData["Title"] = "Dashboard Administração";
@@ -27,18 +26,13 @@
         right: -50%;
         width: 100%;
         height: 200%;
-        background: linear-gradient(45deg, transparent 30%, rgba(255, 255, 255, 0.03) 50%, transparent 70%);
+        background: linear-gradient(45deg, transparent 30%, rgba(255,255,255,0.03) 50%, transparent 70%);
         animation: shimmer 3s infinite;
     }
 
     @@keyframes shimmer {
-        0% {
-            transform: translateX(-100%) rotate(45deg);
-        }
-
-        100% {
-            transform: translateX(100%) rotate(45deg);
-        }
+        0% { transform: translateX(-100%) rotate(45deg); }
+        100% { transform: translateX(100%) rotate(45deg); }
     }
 
     .dashboard-header .header-icon {
@@ -46,7 +40,7 @@
         color: #fff;
         margin-right: 1rem;
         --fa-primary-color: #fff;
-        --fa-secondary-color: rgba(255, 255, 255, 0.7);
+        --fa-secondary-color: rgba(255,255,255,0.7);
         --fa-secondary-opacity: 0.7;
         align-self: center;
     }
@@ -67,7 +61,7 @@
 
     .dashboard-header .subtitle {
         font-family: 'Outfit', sans-serif;
-        color: rgba(255, 255, 255, 0.75);
+        color: rgba(255,255,255,0.75);
         font-size: 0.85rem;
         margin-top: 0.15rem;
         font-weight: 400;
@@ -163,16 +157,11 @@
                         </div>
                         <div class="col-auto ms-auto">
                             <div class="btn-group btn-group-sm" role="group">
-                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(7)">7
-                                    dias</button>
-                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(30)">30
-                                    dias</button>
-                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(90)">90
-                                    dias</button>
-                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(180)">6
-                                    meses</button>
-                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(365)">1
-                                    ano</button>
+                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(7)">7 dias</button>
+                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(30)">30 dias</button>
+                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(90)">90 dias</button>
+                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(180)">6 meses</button>
+                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(365)">1 ano</button>
                             </div>
                         </div>
                     </div>
@@ -249,8 +238,7 @@
         <div class="col-xl-4 col-lg-6 mb-3">
             <div class="card shadow-sm h-100">
                 <div class="card-header bg-white py-2">
-                    <h6 class="mb-0"><i class="fa-duotone fa-chart-pie me-2 text-primary"></i>Viagens Originais vs
-                        Normalizadas</h6>
+                    <h6 class="mb-0"><i class="fa-duotone fa-chart-pie me-2 text-primary"></i>Viagens Originais vs Normalizadas</h6>
                 </div>
                 <div class="card-body">
                     <div id="containerPizzaNormalizacao" style="height: 280px;">
@@ -279,8 +267,7 @@
         <div class="col-xl-4 col-lg-6 mb-3">
             <div class="card shadow-sm h-100">
                 <div class="card-header bg-white py-2">
-                    <h6 class="mb-0"><i class="fa-duotone fa-car-side me-2 text-success"></i>Distribuição por Tipo de
-                        Uso</h6>
+                    <h6 class="mb-0"><i class="fa-duotone fa-car-side me-2 text-success"></i>Distribuição por Tipo de Uso</h6>
                 </div>
                 <div class="card-body">
                     <div id="containerTipoUso" style="height: 280px;">
@@ -296,8 +283,7 @@
         <div class="col-12">
             <div class="card shadow-sm">
                 <div class="card-header bg-white py-2">
-                    <h6 class="mb-0"><i class="fa-duotone fa-th me-2 text-danger"></i>Mapa de Calor - Viagens por
-                        Dia/Hora</h6>
+                    <h6 class="mb-0"><i class="fa-duotone fa-th me-2 text-danger"></i>Mapa de Calor - Viagens por Dia/Hora</h6>
                 </div>
                 <div class="card-body">
                     <div class="table-responsive">
@@ -362,8 +348,7 @@
         <div class="col-xl-6 mb-3">
             <div class="card shadow-sm h-100">
                 <div class="card-header bg-white py-2">
-                    <h6 class="mb-0"><i class="fa-duotone fa-money-bill-wave me-2 text-warning"></i>Custo Médio por
-                        Finalidade</h6>
+                    <h6 class="mb-0"><i class="fa-duotone fa-money-bill-wave me-2 text-warning"></i>Custo Médio por Finalidade</h6>
                 </div>
                 <div class="card-body">
                     <div id="containerCustoPorFinalidade" style="height: 350px;">
@@ -375,8 +360,7 @@
         <div class="col-xl-6 mb-3">
             <div class="card shadow-sm h-100">
                 <div class="card-header bg-white py-2">
-                    <h6 class="mb-0"><i class="fa-duotone fa-balance-scale me-2 text-info"></i>Próprios vs Terceirizados
-                    </h6>
+                    <h6 class="mb-0"><i class="fa-duotone fa-balance-scale me-2 text-info"></i>Próprios vs Terceirizados</h6>
                 </div>
                 <div class="card-body">
                     <div id="containerPropriosTerceirizados" style="height: 350px;">
@@ -391,8 +375,7 @@
         <div class="col-xl-6 mb-3">
             <div class="card shadow-sm h-100">
                 <div class="card-header bg-white py-2">
-                    <h6 class="mb-0"><i class="fa-duotone fa-trophy me-2 text-success"></i>Eficiência da Frota (Menor
-                        Custo/KM)</h6>
+                    <h6 class="mb-0"><i class="fa-duotone fa-trophy me-2 text-success"></i>Eficiência da Frota (Menor Custo/KM)</h6>
                 </div>
                 <div class="card-body">
                     <div id="containerEficiencia" style="height: 350px;">
@@ -404,8 +387,7 @@
         <div class="col-xl-6 mb-3">
             <div class="card shadow-sm h-100">
                 <div class="card-header bg-white py-2">
-                    <h6 class="mb-0"><i class="fa-duotone fa-chart-area me-2 text-primary"></i>Evolução Mensal de Custos
-                    </h6>
+                    <h6 class="mb-0"><i class="fa-duotone fa-chart-area me-2 text-primary"></i>Evolução Mensal de Custos</h6>
                 </div>
                 <div class="card-body">
                     <div id="containerEvolucaoMensal" style="height: 350px;">
```

### REMOVER do Janeiro

```html
        background: linear-gradient(45deg, transparent 30%, rgba(255, 255, 255, 0.03) 50%, transparent 70%);
        0% {
            transform: translateX(-100%) rotate(45deg);
        }
        100% {
            transform: translateX(100%) rotate(45deg);
        }
        --fa-secondary-color: rgba(255, 255, 255, 0.7);
        color: rgba(255, 255, 255, 0.75);
                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(7)">7
                                    dias</button>
                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(30)">30
                                    dias</button>
                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(90)">90
                                    dias</button>
                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(180)">6
                                    meses</button>
                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(365)">1
                                    ano</button>
                    <h6 class="mb-0"><i class="fa-duotone fa-chart-pie me-2 text-primary"></i>Viagens Originais vs
                        Normalizadas</h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-car-side me-2 text-success"></i>Distribuição por Tipo de
                        Uso</h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-th me-2 text-danger"></i>Mapa de Calor - Viagens por
                        Dia/Hora</h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-money-bill-wave me-2 text-warning"></i>Custo Médio por
                        Finalidade</h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-balance-scale me-2 text-info"></i>Próprios vs Terceirizados
                    </h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-trophy me-2 text-success"></i>Eficiência da Frota (Menor
                        Custo/KM)</h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-chart-area me-2 text-primary"></i>Evolução Mensal de Custos
                    </h6>
```


### ADICIONAR ao Janeiro

```html
        background: linear-gradient(45deg, transparent 30%, rgba(255,255,255,0.03) 50%, transparent 70%);
        0% { transform: translateX(-100%) rotate(45deg); }
        100% { transform: translateX(100%) rotate(45deg); }
        --fa-secondary-color: rgba(255,255,255,0.7);
        color: rgba(255,255,255,0.75);
                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(7)">7 dias</button>
                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(30)">30 dias</button>
                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(90)">90 dias</button>
                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(180)">6 meses</button>
                                <button type="button" class="btn btn-outline-secondary" onclick="definirPeriodo(365)">1 ano</button>
                    <h6 class="mb-0"><i class="fa-duotone fa-chart-pie me-2 text-primary"></i>Viagens Originais vs Normalizadas</h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-car-side me-2 text-success"></i>Distribuição por Tipo de Uso</h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-th me-2 text-danger"></i>Mapa de Calor - Viagens por Dia/Hora</h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-money-bill-wave me-2 text-warning"></i>Custo Médio por Finalidade</h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-balance-scale me-2 text-info"></i>Próprios vs Terceirizados</h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-trophy me-2 text-success"></i>Eficiência da Frota (Menor Custo/KM)</h6>
                    <h6 class="mb-0"><i class="fa-duotone fa-chart-area me-2 text-primary"></i>Evolução Mensal de Custos</h6>
```
