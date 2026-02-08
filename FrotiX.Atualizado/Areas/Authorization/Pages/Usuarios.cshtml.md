# Areas/Authorization/Pages/Usuarios.cshtml

**Mudanca:** MEDIA | **+10** linhas | **-20** linhas

---

```diff
--- JANEIRO: Areas/Authorization/Pages/Usuarios.cshtml
+++ ATUAL: Areas/Authorization/Pages/Usuarios.cshtml
@@ -3,9 +3,9 @@
 @{
     ViewData["Title"] = "Usuários";
     ViewData["PageName"] = "authorization_usuarios";
-    ViewData["Heading"] = "<i class='fa-duotone fa-user-lock'></i> Cadastros: <span class='fw-300'>Usuários</span>";
+    ViewData["Heading"] = "<i class='fal fa-shield-alt'></i> Cadastros: <span class='fw-300'>Usuários</span>";
     ViewData["Category1"] = "Authorization";
-    ViewData["PageIcon"] = "fa-user-lock";
+    ViewData["PageIcon"] = "fa-shield-alt";
 }
 
 @section HeadBlock {
@@ -14,27 +14,15 @@
 }
 
 <style>
-    :root {
-        --fa-primary-color: #ff6b35;
-        --fa-secondary-color: #6c757d;
-    }
-
+    /* [DOC] Estilo customizado para destaque de linhas/cabeçalhos da tabela */
     .fundo-cinza {
         background-color: #2F4F4F;
         color: aliceblue;
     }
-
     .label {
         color: white;
     }
 </style>
-
-<div id="loading-overlay" class="ftx-spin-overlay" style="display:none;">
-    <div class="ftx-spin-box">
-        <img src="/images/logo_gota_frotix_transparente.png" class="ftx-loading-logo" />
-        <div class="ftx-loading-text">Processando...</div>
-    </div>
-</div>
 
 <div class="row">
     <div class="col-xl-12">
@@ -46,16 +34,15 @@
                         <div class="row float-right">
                             <div class="col-xs-12 float-right">
                                 <div class="box float-right">
-                                    <div class="box-header float-right"
-                                        style="padding-bottom: 10px; padding-right: 10px">
-                                        <a href="/admin/user/create" class="btn btn-header-orange">
-                                            <i class="fa-duotone fa-user-plus"></i>&nbsp; Adicionar Usuário
+                                    <div class="box-header float-right" style="padding-bottom: 10px; padding-right: 10px">
+                                        <a href="/admin/user/create" class="btn btn-info">
+                                            <i class="fa fa-user-plus">
+                                            </i> Adicionar Usuário
                                         </a>
                                         <div class="box-body">
                                             <span id="message"></span>
 
-                                            <table id="tblUser" class="table table-bordered table-striped style="
-                                                width:100%"">
+                                            <table id="tblUser" class="table table-bordered table-striped style="width:100%"">
                                                 <thead>
                                                     <tr>
                                                         <th>Usuário</th>
@@ -64,7 +51,7 @@
                                                         <th>Papel</th>
                                                         <th>Data de Registro</th>
                                                         <th>Status</th>
-                                                        <th>Ações</th>
+                                                        <th>Ação</th>
                                                     </tr>
                                                 </thead>
                                             </table>
@@ -82,6 +69,6 @@
 
 @section Scripts {
 
-    <script src="~/js/usuarios.js"></script>
+<script src="~/js/usuarios.js"></script>
 
 }
```

### REMOVER do Janeiro

```html
    ViewData["Heading"] = "<i class='fa-duotone fa-user-lock'></i> Cadastros: <span class='fw-300'>Usuários</span>";
    ViewData["PageIcon"] = "fa-user-lock";
    :root {
        --fa-primary-color: #ff6b35;
        --fa-secondary-color: #6c757d;
    }
<div id="loading-overlay" class="ftx-spin-overlay" style="display:none;">
    <div class="ftx-spin-box">
        <img src="/images/logo_gota_frotix_transparente.png" class="ftx-loading-logo" />
        <div class="ftx-loading-text">Processando...</div>
    </div>
</div>
                                    <div class="box-header float-right"
                                        style="padding-bottom: 10px; padding-right: 10px">
                                        <a href="/admin/user/create" class="btn btn-header-orange">
                                            <i class="fa-duotone fa-user-plus"></i>&nbsp; Adicionar Usuário
                                            <table id="tblUser" class="table table-bordered table-striped style="
                                                width:100%"">
                                                        <th>Ações</th>
    <script src="~/js/usuarios.js"></script>
```


### ADICIONAR ao Janeiro

```html
    ViewData["Heading"] = "<i class='fal fa-shield-alt'></i> Cadastros: <span class='fw-300'>Usuários</span>";
    ViewData["PageIcon"] = "fa-shield-alt";
    /* [DOC] Estilo customizado para destaque de linhas/cabeçalhos da tabela */
                                    <div class="box-header float-right" style="padding-bottom: 10px; padding-right: 10px">
                                        <a href="/admin/user/create" class="btn btn-info">
                                            <i class="fa fa-user-plus">
                                            </i> Adicionar Usuário
                                            <table id="tblUser" class="table table-bordered table-striped style="width:100%"">
                                                        <th>Ação</th>
<script src="~/js/usuarios.js"></script>
```
