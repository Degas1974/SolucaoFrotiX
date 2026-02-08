# Pages/Contrato/ItensContrato.cshtml

**Mudanca:** GRANDE | **+22** linhas | **-48** linhas

---

```diff
--- JANEIRO: Pages/Contrato/ItensContrato.cshtml
+++ ATUAL: Pages/Contrato/ItensContrato.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Contrato.ItensContratoModel
 
 @{
@@ -25,8 +24,8 @@
         --text-light: #f8f9fa;
         --text-muted: #6c757d;
         --border-radius: 8px;
-        --shadow-sm: 0 2px 4px rgba(0, 0, 0, 0.1);
-        --shadow-md: 0 4px 12px rgba(0, 0, 0, 0.15);
+        --shadow-sm: 0 2px 4px rgba(0,0,0,0.1);
+        --shadow-md: 0 4px 12px rgba(0,0,0,0.15);
         --transition: all 0.3s ease;
         --verde-militar: #4a5d23;
         --cinza-escuro: #495057;
@@ -39,7 +38,7 @@
         border-radius: 30px;
         padding: 4px;
         margin-bottom: 1.25rem;
-        box-shadow: inset 0 2px 4px rgba(0, 0, 0, 0.1);
+        box-shadow: inset 0 2px 4px rgba(0,0,0,0.1);
     }
 
     .segmented-control .seg-btn {
@@ -277,11 +276,11 @@
         font-size: 0.7rem;
         padding: 0.2rem 0.5rem;
         border-radius: 10px;
-        background: rgba(255, 255, 255, 0.2);
+        background: rgba(255,255,255,0.2);
     }
 
     .nav-tabs-custom .nav-link.active .badge {
-        background: rgba(255, 255, 255, 0.3);
+        background: rgba(255,255,255,0.3);
         color: white;
     }
 
@@ -577,35 +576,18 @@
     }
 
     @@keyframes shimmerAnim {
-        0% {
-            background-position: -200% 0;
-        }
-
-        100% {
-            background-position: 200% 0;
-        }
+        0% { background-position: -200% 0; }
+        100% { background-position: 200% 0; }
     }
 
     @@keyframes fadeIn {
-        from {
-            opacity: 0;
-            transform: translateY(-10px);
-        }
-
-        to {
-            opacity: 1;
-            transform: translateY(0);
-        }
+        from { opacity: 0; transform: translateY(-10px); }
+        to { opacity: 1; transform: translateY(0); }
     }
 
     @@keyframes spin {
-        from {
-            transform: rotate(0deg);
-        }
-
-        to {
-            transform: rotate(360deg);
-        }
+        from { transform: rotate(0deg); }
+        to { transform: rotate(360deg); }
     }
 
     .fa-spin {
@@ -628,8 +610,7 @@
                 <div class="panel-content p-3">
 
                     <div class="segmented-control">
-                        <button type="button" class="seg-btn active" id="btnSwitchContrato"
-                            onclick="switchTipo('contrato')">
+                        <button type="button" class="seg-btn active" id="btnSwitchContrato" onclick="switchTipo('contrato')">
                             <i class="fa-duotone fa-file-contract"></i> CONTRATOS
                         </button>
                         <button type="button" class="seg-btn" id="btnSwitchAta" onclick="switchTipo('ata')">
@@ -731,36 +712,31 @@
                     <div class="tabstrip-container" id="tabstripContainer">
                         <ul class="nav nav-tabs nav-tabs-custom" role="tablist">
                             <li class="nav-item" id="navItemVeiculos" style="display:none;">
-                                <a class="nav-link" id="tabVeiculos" data-bs-toggle="tab" href="#paneVeiculos"
-                                    role="tab">
+                                <a class="nav-link" id="tabVeiculos" data-bs-toggle="tab" href="#paneVeiculos" role="tab">
                                     <i class="fa-duotone fa-car"></i> Veículos
                                     <span class="badge" id="badgeVeiculos">0</span>
                                 </a>
                             </li>
                             <li class="nav-item" id="navItemEncarregados" style="display:none;">
-                                <a class="nav-link" id="tabEncarregados" data-bs-toggle="tab" href="#paneEncarregados"
-                                    role="tab">
+                                <a class="nav-link" id="tabEncarregados" data-bs-toggle="tab" href="#paneEncarregados" role="tab">
                                     <i class="fa-duotone fa-user-tie"></i> Encarregados
                                     <span class="badge" id="badgeEncarregados">0</span>
                                 </a>
                             </li>
                             <li class="nav-item" id="navItemOperadores" style="display:none;">
-                                <a class="nav-link" id="tabOperadores" data-bs-toggle="tab" href="#paneOperadores"
-                                    role="tab">
+                                <a class="nav-link" id="tabOperadores" data-bs-toggle="tab" href="#paneOperadores" role="tab">
                                     <i class="fa-duotone fa-user-cog"></i> Operadores
                                     <span class="badge" id="badgeOperadores">0</span>
                                 </a>
                             </li>
                             <li class="nav-item" id="navItemMotoristas" style="display:none;">
-                                <a class="nav-link" id="tabMotoristas" data-bs-toggle="tab" href="#paneMotoristas"
-                                    role="tab">
+                                <a class="nav-link" id="tabMotoristas" data-bs-toggle="tab" href="#paneMotoristas" role="tab">
                                     <i class="fa-duotone fa-steering-wheel"></i> Motoristas
                                     <span class="badge" id="badgeMotoristas">0</span>
                                 </a>
                             </li>
                             <li class="nav-item" id="navItemLavadores" style="display:none;">
-                                <a class="nav-link" id="tabLavadores" data-bs-toggle="tab" href="#paneLavadores"
-                                    role="tab">
+                                <a class="nav-link" id="tabLavadores" data-bs-toggle="tab" href="#paneLavadores" role="tab">
                                     <i class="fa-duotone fa-soap"></i> Lavadores
                                     <span class="badge" id="badgeLavadores">0</span>
                                 </a>
@@ -773,8 +749,7 @@
                                 <div class="tab-toolbar">
                                     <div class="info-contratada">
                                         Quantidade Contratada: <strong id="qtdContratadaVeiculos">0</strong>
-                                        <span id="spanCustoVeiculos"> | Custo Mensal: <strong
-                                                id="custoMensalVeiculos">R$ 0,00</strong></span>
+                                        <span id="spanCustoVeiculos"> | Custo Mensal: <strong id="custoMensalVeiculos">R$ 0,00</strong></span>
                                     </div>
                                     <button type="button" class="btn btn-azul" onclick="abrirModalVeiculo()">
                                         <i class="fa-duotone fa-circle-plus"></i> Incluir Veículo
@@ -783,16 +758,14 @@
 
                                 <div class="filtro-item-container">
                                     <div class="filtro-item-group">
-                                        <label for="ddlFiltroItem"><i class="fa-duotone fa-filter"></i> Filtrar por
-                                            Item:</label>
+                                        <label for="ddlFiltroItem"><i class="fa-duotone fa-filter"></i> Filtrar por Item:</label>
                                         <select id="ddlFiltroItem" class="form-control">
                                             <option value="">-- Todos os Itens --</option>
                                         </select>
                                         <button type="button" class="btn-filtrar" onclick="filtrarPorItem()">
                                             <i class="fa-duotone fa-search"></i> Filtrar
                                         </button>
-                                        <button type="button" class="btn-limpar-filtro" onclick="limparFiltroItem()"
-                                            title="Limpar Filtro">
+                                        <button type="button" class="btn-limpar-filtro" onclick="limparFiltroItem()" title="Limpar Filtro">
                                             <i class="fa-duotone fa-times"></i>
                                         </button>
                                     </div>
@@ -977,9 +950,7 @@
     <div class="modal-dialog">
         <div class="modal-content">
             <div class="modal-header ftx-modal-header">
-                <h5 class="modal-title"><i class="fa-duotone fa-user-tie"
-                        style="--fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i> Incluir Encarregado
-                </h5>
+                <h5 class="modal-title"><i class="fa-duotone fa-user-tie" style="--fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i> Incluir Encarregado</h5>
                 <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
             </div>
             <div class="modal-body">
@@ -1093,8 +1064,7 @@
 
 <div id="loadingOverlayContrato" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text">Carregando Itens do Contrato...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
```

### REMOVER do Janeiro

```html
        --shadow-sm: 0 2px 4px rgba(0, 0, 0, 0.1);
        --shadow-md: 0 4px 12px rgba(0, 0, 0, 0.15);
        box-shadow: inset 0 2px 4px rgba(0, 0, 0, 0.1);
        background: rgba(255, 255, 255, 0.2);
        background: rgba(255, 255, 255, 0.3);
        0% {
            background-position: -200% 0;
        }
        100% {
            background-position: 200% 0;
        }
        from {
            opacity: 0;
            transform: translateY(-10px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
        from {
            transform: rotate(0deg);
        }
        to {
            transform: rotate(360deg);
        }
                        <button type="button" class="seg-btn active" id="btnSwitchContrato"
                            onclick="switchTipo('contrato')">
                                <a class="nav-link" id="tabVeiculos" data-bs-toggle="tab" href="#paneVeiculos"
                                    role="tab">
                                <a class="nav-link" id="tabEncarregados" data-bs-toggle="tab" href="#paneEncarregados"
                                    role="tab">
                                <a class="nav-link" id="tabOperadores" data-bs-toggle="tab" href="#paneOperadores"
                                    role="tab">
                                <a class="nav-link" id="tabMotoristas" data-bs-toggle="tab" href="#paneMotoristas"
                                    role="tab">
                                <a class="nav-link" id="tabLavadores" data-bs-toggle="tab" href="#paneLavadores"
                                    role="tab">
                                        <span id="spanCustoVeiculos"> | Custo Mensal: <strong
                                                id="custoMensalVeiculos">R$ 0,00</strong></span>
                                        <label for="ddlFiltroItem"><i class="fa-duotone fa-filter"></i> Filtrar por
                                            Item:</label>
                                        <button type="button" class="btn-limpar-filtro" onclick="limparFiltroItem()"
                                            title="Limpar Filtro">
                <h5 class="modal-title"><i class="fa-duotone fa-user-tie"
                        style="--fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i> Incluir Encarregado
                </h5>
        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
            style="display: block;" />
```


### ADICIONAR ao Janeiro

```html
        --shadow-sm: 0 2px 4px rgba(0,0,0,0.1);
        --shadow-md: 0 4px 12px rgba(0,0,0,0.15);
        box-shadow: inset 0 2px 4px rgba(0,0,0,0.1);
        background: rgba(255,255,255,0.2);
        background: rgba(255,255,255,0.3);
        0% { background-position: -200% 0; }
        100% { background-position: 200% 0; }
        from { opacity: 0; transform: translateY(-10px); }
        to { opacity: 1; transform: translateY(0); }
        from { transform: rotate(0deg); }
        to { transform: rotate(360deg); }
                        <button type="button" class="seg-btn active" id="btnSwitchContrato" onclick="switchTipo('contrato')">
                                <a class="nav-link" id="tabVeiculos" data-bs-toggle="tab" href="#paneVeiculos" role="tab">
                                <a class="nav-link" id="tabEncarregados" data-bs-toggle="tab" href="#paneEncarregados" role="tab">
                                <a class="nav-link" id="tabOperadores" data-bs-toggle="tab" href="#paneOperadores" role="tab">
                                <a class="nav-link" id="tabMotoristas" data-bs-toggle="tab" href="#paneMotoristas" role="tab">
                                <a class="nav-link" id="tabLavadores" data-bs-toggle="tab" href="#paneLavadores" role="tab">
                                        <span id="spanCustoVeiculos"> | Custo Mensal: <strong id="custoMensalVeiculos">R$ 0,00</strong></span>
                                        <label for="ddlFiltroItem"><i class="fa-duotone fa-filter"></i> Filtrar por Item:</label>
                                        <button type="button" class="btn-limpar-filtro" onclick="limparFiltroItem()" title="Limpar Filtro">
                <h5 class="modal-title"><i class="fa-duotone fa-user-tie" style="--fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i> Incluir Encarregado</h5>
        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
```
