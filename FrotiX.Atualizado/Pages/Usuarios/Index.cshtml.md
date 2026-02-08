# Pages/Usuarios/Index.cshtml

**Mudanca:** MEDIA | **+7** linhas | **-12** linhas

---

```diff
--- JANEIRO: Pages/Usuarios/Index.cshtml
+++ ATUAL: Pages/Usuarios/Index.cshtml
@@ -72,7 +72,7 @@
             max-width: 300px;
             border: 3px solid #dee2e6;
             border-radius: 12px;
-            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
+            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
         }
 
         .no-foto-placeholder {
@@ -102,7 +102,6 @@
 
         /* ===== RESPONSIVO ===== */
         @@media (max-width: 768px) {
-
             #modalControleAcesso .modal-dialog,
             #modalFoto .modal-dialog {
                 max-width: 95%;
@@ -129,15 +128,14 @@
 
                 <div class="panel-content">
                     <div class="box-body">
-                        <table id="tblUsuario" class="table table-bordered table-striped table-hover ftx-table"
-                            width="100%">
+                        <table id="tblUsuario" class="table table-bordered table-striped table-hover ftx-table" width="100%">
                             <thead>
                                 <tr>
                                     <th>Usuário</th>
                                     <th>Ponto</th>
                                     <th>Detentor Carga</th>
                                     <th>Status</th>
-                                    <th class="text-center">Ações</th>
+                                    <th>Ação</th>
                                 </tr>
                             </thead>
                             <tbody>
@@ -150,8 +148,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalControleAcesso" tabindex="-1" aria-labelledby="tituloControleAcesso"
-    aria-hidden="true">
+<div class="modal fade" id="modalControleAcesso" tabindex="-1" aria-labelledby="tituloControleAcesso" aria-hidden="true">
     <div class="modal-dialog modal-xl">
         <div class="modal-content">
             <div class="modal-header">
@@ -159,16 +156,14 @@
                     <i class="fa-duotone fa-shield-keyhole"></i>
                     Gestão de Recursos do Usuário
                 </h3>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <input type="hidden" id="txtUsuarioIdRecurso" />
 
                 <div class="alert alert-info mb-3">
                     <i class="fa-duotone fa-circle-info me-2"></i>
-                    <strong id="txtNomeUsuarioRecurso"></strong> - Clique nos botões para conceder ou remover acesso aos
-                    recursos.
+                    <strong id="txtNomeUsuarioRecurso"></strong> - Clique nos botões para conceder ou remover acesso aos recursos.
                 </div>
 
                 <table id="tblRecursos" class="table table-bordered table-striped table-hover ftx-table" width="100%">
@@ -199,8 +194,7 @@
                     <i class="fa-duotone fa-id-badge"></i>
                     Foto do Usuário
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body text-center">
                 <h6 id="txtNomeUsuarioFoto" class="text-muted mb-3"></h6>
```

### REMOVER do Janeiro

```html
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
                        <table id="tblUsuario" class="table table-bordered table-striped table-hover ftx-table"
                            width="100%">
                                    <th class="text-center">Ações</th>
<div class="modal fade" id="modalControleAcesso" tabindex="-1" aria-labelledby="tituloControleAcesso"
    aria-hidden="true">
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
                    aria-label="Fechar"></button>
                    <strong id="txtNomeUsuarioRecurso"></strong> - Clique nos botões para conceder ou remover acesso aos
                    recursos.
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
                    aria-label="Fechar"></button>
```


### ADICIONAR ao Janeiro

```html
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
                        <table id="tblUsuario" class="table table-bordered table-striped table-hover ftx-table" width="100%">
                                    <th>Ação</th>
<div class="modal fade" id="modalControleAcesso" tabindex="-1" aria-labelledby="tituloControleAcesso" aria-hidden="true">
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
                    <strong id="txtNomeUsuarioRecurso"></strong> - Clique nos botões para conceder ou remover acesso aos recursos.
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
```
