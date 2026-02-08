# Pages/Usuarios/Recursos.cshtml

**Mudanca:** GRANDE | **+26** linhas | **-51** linhas

---

```diff
--- JANEIRO: Pages/Usuarios/Recursos.cshtml
+++ ATUAL: Pages/Usuarios/Recursos.cshtml
@@ -1,4 +1,5 @@
 @page
+
 @model FrotiX.Models.Unidade
 
 @{
@@ -18,25 +19,11 @@
 
         /* ====== ANIMAÇÃO GLOW FROTIX ====== */
         @@keyframes buttonWiggle {
-            0% {
-                transform: translateY(0) rotate(0deg);
-            }
-
-            25% {
-                transform: translateY(-2px) rotate(-1deg);
-            }
-
-            50% {
-                transform: translateY(-3px) rotate(0deg);
-            }
-
-            75% {
-                transform: translateY(-2px) rotate(1deg);
-            }
-
-            100% {
-                transform: translateY(0) rotate(0deg);
-            }
+            0% { transform: translateY(0) rotate(0deg); }
+            25% { transform: translateY(-2px) rotate(-1deg); }
+            50% { transform: translateY(-3px) rotate(0deg); }
+            75% { transform: translateY(-2px) rotate(1deg); }
+            100% { transform: translateY(0) rotate(0deg); }
         }
 
         /* ====== VARIÁVEIS LOCAIS ====== */
@@ -67,7 +54,7 @@
         .ftx-card-header .ftx-card-title i {
             color: #fff !important;
             --fa-primary-color: #fff !important;
-            --fa-secondary-color: rgba(255, 255, 255, 0.7) !important;
+            --fa-secondary-color: rgba(255,255,255,0.7) !important;
         }
 
         /* ====== TABELA ====== */
@@ -102,7 +89,7 @@
         #tblRecurso tbody td {
             padding: 0.625rem;
             vertical-align: middle !important;
-            border-color: rgba(0, 0, 0, 0.05) !important;
+            border-color: rgba(0,0,0,0.05) !important;
         }
 
         /* ====== BOTÕES DE AÇÃO NA TABELA - COM GLOW FROTIX ====== */
@@ -145,14 +132,11 @@
             background-color: #3D5771 !important;
             box-shadow: 0 0 8px rgba(61, 87, 113, 0.5), 0 2px 4px rgba(61, 87, 113, 0.3) !important;
         }
-
         .ftx-btn-editar:hover:not([aria-disabled="true"]) {
             background-color: #2d4559 !important;
             box-shadow: 0 0 20px rgba(61, 87, 113, 0.8), 0 6px 12px rgba(61, 87, 113, 0.5) !important;
         }
-
-        .ftx-btn-editar:active,
-        .ftx-btn-editar:focus {
+        .ftx-btn-editar:active, .ftx-btn-editar:focus {
             box-shadow: 0 0 0 0.2rem rgba(61, 87, 113, 0.35) !important;
         }
 
@@ -161,14 +145,11 @@
             background-color: #722f37 !important;
             box-shadow: 0 0 8px rgba(114, 47, 55, 0.5), 0 2px 4px rgba(114, 47, 55, 0.3) !important;
         }
-
         .ftx-btn-apagar:hover:not([aria-disabled="true"]) {
             background-color: #5a252c !important;
             box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important;
         }
-
-        .ftx-btn-apagar:active,
-        .ftx-btn-apagar:focus {
+        .ftx-btn-apagar:active, .ftx-btn-apagar:focus {
             box-shadow: 0 0 0 0.2rem rgba(114, 47, 55, 0.25) !important;
         }
 
@@ -177,14 +158,11 @@
             background-color: #d35400 !important;
             box-shadow: 0 0 8px rgba(211, 84, 0, 0.5), 0 2px 4px rgba(211, 84, 0, 0.3) !important;
         }
-
         .ftx-btn-acesso:hover:not([aria-disabled="true"]) {
             background-color: #b84800 !important;
             box-shadow: 0 0 20px rgba(211, 84, 0, 0.8), 0 6px 12px rgba(211, 84, 0, 0.5) !important;
         }
-
-        .ftx-btn-acesso:active,
-        .ftx-btn-acesso:focus {
+        .ftx-btn-acesso:active, .ftx-btn-acesso:focus {
             box-shadow: 0 0 0 0.2rem rgba(211, 84, 0, 0.35) !important;
         }
 
@@ -247,7 +225,7 @@
                                     <th>Nome</th>
                                     <th>Nome Menu</th>
                                     <th>Ordem</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                 </tr>
                             </thead>
                             <tbody>
@@ -266,8 +244,7 @@
         <div class="modal-content">
             <div class="modal-header modal-header-azul">
                 <h3 class="modal-title" id="h3Titulo">Exibe os Usuários e seu acesso ao recurso</h3>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmStatus">
@@ -301,13 +278,11 @@
 
 @section ScriptsBlock {
 
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script>
-        (function () {
+        (function() {
             "use strict";
 
             var dataTable;
@@ -464,16 +439,6 @@
                 }
             });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * DATATABLE DE RECURSOS
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 */
-
-                /**
-                 * Carrega/recarrega o DataTable de recursos do sistema.
-                 * @@description Lista recursos com botões de editar, excluir e controle de acesso.
-                 */
             function loadList() {
                 try {
                     if ($.fn.DataTable.isDataTable('#tblRecurso')) {
@@ -502,17 +467,17 @@
                                 data: "recursoId",
                                 render: function (data) {
                                     return `
-                                            <div class="text-center">
-                                                <a href="/Usuarios/UpsertRecurso?id=${data}" class="btn ftx-btn-icon ftx-btn-editar" data-ejtip="Editar Recurso" aria-label="Editar Recurso" role="button">
-                                                    <i class="fa-duotone fa-edit"></i>
-                                                </a>
-                                                <a class="btn-delete btn ftx-btn-icon ftx-btn-apagar" data-ejtip="Excluir Recurso" aria-label="Excluir Recurso" role="button" data-id='${data}'>
-                                                    <i class="fa-duotone fa-trash-alt"></i>
-                                                </a>
-                                                <a class="btn ftx-btn-icon ftx-btn-acesso" data-ejtip="Controle de Acesso" aria-label="Controle de Acesso" role="button" data-id='${data}'>
-                                                    <i class="fa-duotone fa-users-gear"></i>
-                                                </a>
-                                            </div>`;
+                                        <div class="text-center">
+                                            <a href="/Usuarios/UpsertRecurso?id=${data}" class="btn ftx-btn-icon ftx-btn-editar" data-ejtip="Editar Recurso" aria-label="Editar Recurso" role="button">
+                                                <i class="fa-duotone fa-edit"></i>
+                                            </a>
+                                            <a class="btn-delete btn ftx-btn-icon ftx-btn-apagar" data-ejtip="Excluir Recurso" aria-label="Excluir Recurso" role="button" data-id='${data}'>
+                                                <i class="fa-duotone fa-trash-alt"></i>
+                                            </a>
+                                            <a class="btn ftx-btn-icon ftx-btn-acesso" data-ejtip="Controle de Acesso" aria-label="Controle de Acesso" role="button" data-id='${data}'>
+                                                <i class="fa-duotone fa-users-gear"></i>
+                                            </a>
+                                        </div>`;
                                 }
                             }
                         ],
```

### REMOVER do Janeiro

```html
            0% {
                transform: translateY(0) rotate(0deg);
            }
            25% {
                transform: translateY(-2px) rotate(-1deg);
            }
            50% {
                transform: translateY(-3px) rotate(0deg);
            }
            75% {
                transform: translateY(-2px) rotate(1deg);
            }
            100% {
                transform: translateY(0) rotate(0deg);
            }
            --fa-secondary-color: rgba(255, 255, 255, 0.7) !important;
            border-color: rgba(0, 0, 0, 0.05) !important;
        .ftx-btn-editar:active,
        .ftx-btn-editar:focus {
        .ftx-btn-apagar:active,
        .ftx-btn-apagar:focus {
        .ftx-btn-acesso:active,
        .ftx-btn-acesso:focus {
                                    <th>Ações</th>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
                    aria-label="Fechar"></button>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
        crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
        crossorigin="anonymous"></script>
        (function () {
                /**
                 * ═══════════════════════════════════════════════════════════════════════════
                 * DATATABLE DE RECURSOS
                 * ═══════════════════════════════════════════════════════════════════════════
                 */
                /**
                 * Carrega/recarrega o DataTable de recursos do sistema.
                 * @@description Lista recursos com botões de editar, excluir e controle de acesso.
                 */
                                            <div class="text-center">
                                                <a href="/Usuarios/UpsertRecurso?id=${data}" class="btn ftx-btn-icon ftx-btn-editar" data-ejtip="Editar Recurso" aria-label="Editar Recurso" role="button">
                                                    <i class="fa-duotone fa-edit"></i>
                                                </a>
                                                <a class="btn-delete btn ftx-btn-icon ftx-btn-apagar" data-ejtip="Excluir Recurso" aria-label="Excluir Recurso" role="button" data-id='${data}'>
                                                    <i class="fa-duotone fa-trash-alt"></i>
                                                </a>
                                                <a class="btn ftx-btn-icon ftx-btn-acesso" data-ejtip="Controle de Acesso" aria-label="Controle de Acesso" role="button" data-id='${data}'>
                                                    <i class="fa-duotone fa-users-gear"></i>
                                                </a>
                                            </div>`;
```


### ADICIONAR ao Janeiro

```html
            0% { transform: translateY(0) rotate(0deg); }
            25% { transform: translateY(-2px) rotate(-1deg); }
            50% { transform: translateY(-3px) rotate(0deg); }
            75% { transform: translateY(-2px) rotate(1deg); }
            100% { transform: translateY(0) rotate(0deg); }
            --fa-secondary-color: rgba(255,255,255,0.7) !important;
            border-color: rgba(0,0,0,0.05) !important;
        .ftx-btn-editar:active, .ftx-btn-editar:focus {
        .ftx-btn-apagar:active, .ftx-btn-apagar:focus {
        .ftx-btn-acesso:active, .ftx-btn-acesso:focus {
                                    <th>Ação</th>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
        (function() {
                                        <div class="text-center">
                                            <a href="/Usuarios/UpsertRecurso?id=${data}" class="btn ftx-btn-icon ftx-btn-editar" data-ejtip="Editar Recurso" aria-label="Editar Recurso" role="button">
                                                <i class="fa-duotone fa-edit"></i>
                                            </a>
                                            <a class="btn-delete btn ftx-btn-icon ftx-btn-apagar" data-ejtip="Excluir Recurso" aria-label="Excluir Recurso" role="button" data-id='${data}'>
                                                <i class="fa-duotone fa-trash-alt"></i>
                                            </a>
                                            <a class="btn ftx-btn-icon ftx-btn-acesso" data-ejtip="Controle de Acesso" aria-label="Controle de Acesso" role="button" data-id='${data}'>
                                                <i class="fa-duotone fa-users-gear"></i>
                                            </a>
                                        </div>`;
```
