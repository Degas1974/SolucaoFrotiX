# Pages/Administracao/HigienizarViagens.cshtml

**Mudanca:** GRANDE | **+69** linhas | **-66** linhas

---

```diff
--- JANEIRO: Pages/Administracao/HigienizarViagens.cshtml
+++ ATUAL: Pages/Administracao/HigienizarViagens.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Administracao.HigienizarViagensModel
 @{
     ViewData["Title"] = "Higienizar Origens/Destinos das Viagens";
@@ -25,15 +24,15 @@
         margin-top: 60px;
     }
 
-    .transfer-buttons button {
-        padding: 10px;
-        font-size: 1rem;
-        background-color: #007bff;
-        color: white;
-        border: none;
-        border-radius: 5px;
-        cursor: pointer;
-    }
+        .transfer-buttons button {
+            padding: 10px;
+            font-size: 1rem;
+            background-color: #007bff;
+            color: white;
+            border: none;
+            border-radius: 5px;
+            cursor: pointer;
+        }
 
     .right-panel {
         display: flex;
@@ -48,22 +47,22 @@
         gap: 10px;
     }
 
-    .input-group input {
-        padding: 8px;
-        border: 1px solid #ccc;
-        border-radius: 4px;
-        flex: 1;
-    }
-
-    .input-group button {
-        padding: 8px 16px;
-        background-color: #28a745;
-        color: white;
-        font-weight: bold;
-        border: none;
-        border-radius: 4px;
-        cursor: pointer;
-    }
+        .input-group input {
+            padding: 8px;
+            border: 1px solid #ccc;
+            border-radius: 4px;
+            flex: 1;
+        }
+
+        .input-group button {
+            padding: 8px 16px;
+            background-color: #28a745;
+            color: white;
+            font-weight: bold;
+            border: none;
+            border-radius: 4px;
+            cursor: pointer;
+        }
 
     .badge-contador {
         position: absolute;
@@ -124,8 +123,7 @@
     }
 
     .e-badge {
-        border-radius: 12px !important;
-        /* ou 999px para deixar tipo "pílula" */
+        border-radius: 12px !important; /* ou 999px para deixar tipo "pílula" */
         padding: 0.35em 0.75em;
         font-size: 0.85em;
     }
@@ -171,8 +169,7 @@
 
     /* (Opcional) Define largura mínima para todos */
     .e-listbox {
-        min-width: 250px;
-        /* ajuste conforme seu layout */
+        min-width: 250px; /* ajuste conforme seu layout */
         width: 100%;
     }
 
@@ -195,10 +192,11 @@
         letter-spacing: 0.03em !important;
     }
 
-    .e-badge.e-badge-modern.e-badge-modern-warning {
-        background: linear-gradient(90deg, #ffb347 50%, #ff8800 100%) !important;
-        color: #fffbe8 !important;
-    }
+        .e-badge.e-badge-modern.e-badge-modern-warning {
+            background: linear-gradient(90deg, #ffb347 50%, #ff8800 100%) !important;
+            color: #fffbe8 !important;
+        }
+
 </style>
 
 @section HeadBlock {
@@ -229,38 +227,41 @@
                         <div class="row">
                             <div class="col-md-5">
 
-                                <ejs-listbox id="listOrigens" dataSource="@Model.OrigensDistintas" allowFiltering="true"
-                                    scope="origens" height="350px" cssClass="e-custom-listbox w-100"
-                                    filterBarPlaceholder="Filtrar...">
+                                <ejs-listbox id="listOrigens"
+                                             dataSource="@Model.OrigensDistintas"
+                                             allowFiltering="true"
+                                             scope="origens"
+                                             height="350px"
+                                             cssClass="e-custom-listbox w-100"
+                                             filterBarPlaceholder="Filtrar...">
                                 </ejs-listbox>
 
                                                             </div>
 
                             <div class="col-md-2 d-flex flex-column justify-content-center align-items-center">
-                                <button class="e-btn mb-2 w-100" type="button"
-                                    onclick="moverSelecionados('listOrigens', 'listOrigensSelecionadas')">→</button>
-                                <button class="e-btn w-100" type="button"
-                                    onclick="moverSelecionados('listOrigensSelecionadas', 'listOrigens')">←</button>
+                                <button class="e-btn mb-2 w-100" type="button" onclick="moverSelecionados('listOrigens', 'listOrigensSelecionadas')">→</button>
+                                <button class="e-btn w-100" type="button" onclick="moverSelecionados('listOrigensSelecionadas', 'listOrigens')">←</button>
                             </div>
 
                             <div class="col-md-5">
                                 <h5>
                                     Origens Selecionadas
-                                    <span id="badgeOrigensSelecionadas"
-                                        class="e-badge e-badge-modern e-badge-modern-warning">
-                                        <span class="badge-icon">📥</span><span
-                                            class="badge-num">@Model.OrigensParaCorrigir?.Count</span>
+                                    <span id="badgeOrigensSelecionadas" class="e-badge e-badge-modern e-badge-modern-warning">
+                                        <span class="badge-icon">📥</span><span class="badge-num">@Model.OrigensParaCorrigir?.Count</span>
                                     </span>
                                 </h5>
 
-                                <ejs-listbox id="listOrigensSelecionadas" dataSource="@Model.OrigensParaCorrigir"
-                                    allowFiltering="true" scope="origens" height="250px"
-                                    cssClass="e-custom-listbox w-100" filterBarPlaceholder="Filtrar...">
+                                <ejs-listbox id="listOrigensSelecionadas"
+                                             dataSource="@Model.OrigensParaCorrigir"
+                                             allowFiltering="true"
+                                             scope="origens"
+                                             height="250px"
+                                             cssClass="e-custom-listbox w-100"
+                                             filterBarPlaceholder="Filtrar...">
                                 </ejs-listbox>
 
                                 <div class="input-group mt-3">
-                                    <input type="text" id="txtNovaOrigem" class="form-control"
-                                        placeholder="Novo valor de origem..." />
+                                    <input type="text" id="txtNovaOrigem" class="form-control" placeholder="Novo valor de origem..." />
                                     <button id="btnGravarOrigem" class="btn btn-azul" type="button">Gravar</button>
                                 </div>
                             </div>
@@ -277,38 +278,41 @@
                         <div class="row">
                             <div class="col-md-5">
 
-                                <ejs-listbox id="listDestinos" dataSource="@Model.DestinosDistintos"
-                                    allowFiltering="true" scope="origens" height="350px"
-                                    cssClass="e-custom-listbox w-100" filterBarPlaceholder="Filtrar...">
+                                <ejs-listbox id="listDestinos"
+                                             dataSource="@Model.DestinosDistintos"
+                                             allowFiltering="true"
+                                             scope="origens"
+                                             height="350px"
+                                             cssClass="e-custom-listbox w-100"
+                                             filterBarPlaceholder="Filtrar...">
                                 </ejs-listbox>
 
                             </div>
 
                             <div class="col-md-2 d-flex flex-column justify-content-center align-items-center">
-                                <button class="e-btn mb-2 w-100" type="button"
-                                    onclick="moverSelecionados('listDestinos', 'listDestinosSelecionados')">→</button>
-                                <button class="e-btn w-100" type="button"
-                                    onclick="moverSelecionados('listDestinosSelecionados', 'listDestinos')">←</button>
+                                <button class="e-btn mb-2 w-100" type="button" onclick="moverSelecionados('listDestinos', 'listDestinosSelecionados')">→</button>
+                                <button class="e-btn w-100" type="button" onclick="moverSelecionados('listDestinosSelecionados', 'listDestinos')">←</button>
                             </div>
 
                             <div class="col-md-5">
                                 <h5>
                                     Destinos Selecionados
-                                    <span id="badgeDestinosSelecionados"
-                                        class="e-badge e-badge-modern e-badge-modern-warning">
-                                        <span class="badge-icon">📥</span><span
-                                            class="badge-num">@Model.DestinosParaCorrigir?.Count</span>
+                                    <span id="badgeDestinosSelecionados" class="e-badge e-badge-modern e-badge-modern-warning">
+                                        <span class="badge-icon">📥</span><span class="badge-num">@Model.DestinosParaCorrigir?.Count</span>
                                     </span>
                                 </h5>
 
-                                <ejs-listbox id="listDestinosSelecionados" dataSource="@Model.DestinosParaCorrigir"
-                                    allowFiltering="true" scope="origens" height="250px"
-                                    cssClass="e-custom-listbox w-100" filterBarPlaceholder="Filtrar...">
+                                <ejs-listbox id="listDestinosSelecionados"
+                                             dataSource="@Model.DestinosParaCorrigir"
+                                             allowFiltering="true"
+                                             scope="origens"
+                                             height="250px"
+                                             cssClass="e-custom-listbox w-100"
+                                             filterBarPlaceholder="Filtrar...">
                                 </ejs-listbox>
 
                                 <div class="input-group mt-3">
-                                    <input type="text" id="txtNovoDestino" class="form-control"
-                                        placeholder="Novo valor de destino..." />
+                                    <input type="text" id="txtNovoDestino" class="form-control" placeholder="Novo valor de destino..." />
                                     <button id="btnGravarDestino" class="btn btn-azul" type="button">Gravar</button>
                                 </div>
                             </div>
@@ -322,8 +326,7 @@
 
 <div id="loadingOverlayHigienizar" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text">Carregando Viagens...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
```
