# Areas/Identity/Pages/ConfirmarSenha.cshtml

**Mudanca:** MEDIA | **+8** linhas | **-17** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/ConfirmarSenha.cshtml
+++ ATUAL: Areas/Identity/Pages/ConfirmarSenha.cshtml
@@ -1,16 +1,9 @@
 @page
-@model FrotiX.Areas.Identity.Pages.ConfirmarSenha
+@model FrotiX.Areas.Identity.Pages.ConfirmarSenha.ConfirmarSenhaModel
 @{
     ViewData["Title"] = "FrotiX | Confirmar Email";
     Layout = Layout = "_ConfirmacaoLayout";
 }
-<style>
-    :root {
-        --fa-primary-color: #ff6b35;
-        --fa-secondary-color: #6c757d;
-    }
-</style>
-
 <div class="login-content">
 
     <div class="form-login-error">
@@ -20,32 +13,30 @@
 
     <form method="post" autocomplete="off">
         @Html.AntiForgeryToken()
+
         <div class="form-group">
 
             <div class="input-group">
-                <div class="input-group-addon" style="padding: 10px; background: #eee; border: 1px solid #ccc;">
-                    <i class="fa-duotone fa-key fa-lg"></i>
+                <div class="input-group-addon">
+                    <i class="entypo-key"></i>
                 </div>
-                <input name="Password" asp-for="@Model.Input.Password" type="password" class="form-control"
-                    placeholder="Senha" autocomplete="off" data-validate="required" />
+                <input name="Password" asp-for="@Model.Password" type="password" class="form-control" placeholder="Senha" autocomplete="off" data-validate="required" />
             </div>
         </div>
 
         <div class="form-group">
             <div class="input-group">
-                <div class="input-group-addon" style="padding: 10px; background: #eee; border: 1px solid #ccc;">
-                    <i class="fa-duotone fa-key fa-lg"></i>
+                <div class="input-group-addon">
+                    <i class="entypo-key"></i>
                 </div>
-                <input name="ConfirmacaoPassword" asp-for="@Model.Input.ConfirmacaoPassword" type="password"
-                    class="form-control" placeholder="Confirmação de Senha" autocomplete="off"
-                    data-validate="required" />
+                <input name="ConfirmacaoPassword" asp-for="@Model.ConfirmacaoPassword" type="password" class="form-control" placeholder="Confirmação de Senha" autocomplete="off" data-validate="required" />
             </div>
 
         </div>
 
         <div class="form-group">
             <button class="btn btn-azul btn-block btn-login">
-                <i class="fa-duotone fa-sign-in-alt"></i>
+                <i class="entypo-login"></i>
                 Confirmar Senha e Logar no Sistema
             </button>
         </div>
```

### REMOVER do Janeiro

```html
@model FrotiX.Areas.Identity.Pages.ConfirmarSenha
<style>
    :root {
        --fa-primary-color: #ff6b35;
        --fa-secondary-color: #6c757d;
    }
</style>
                <div class="input-group-addon" style="padding: 10px; background: #eee; border: 1px solid #ccc;">
                    <i class="fa-duotone fa-key fa-lg"></i>
                <input name="Password" asp-for="@Model.Input.Password" type="password" class="form-control"
                    placeholder="Senha" autocomplete="off" data-validate="required" />
                <div class="input-group-addon" style="padding: 10px; background: #eee; border: 1px solid #ccc;">
                    <i class="fa-duotone fa-key fa-lg"></i>
                <input name="ConfirmacaoPassword" asp-for="@Model.Input.ConfirmacaoPassword" type="password"
                    class="form-control" placeholder="Confirmação de Senha" autocomplete="off"
                    data-validate="required" />
                <i class="fa-duotone fa-sign-in-alt"></i>
```


### ADICIONAR ao Janeiro

```html
@model FrotiX.Areas.Identity.Pages.ConfirmarSenha.ConfirmarSenhaModel
                <div class="input-group-addon">
                    <i class="entypo-key"></i>
                <input name="Password" asp-for="@Model.Password" type="password" class="form-control" placeholder="Senha" autocomplete="off" data-validate="required" />
                <div class="input-group-addon">
                    <i class="entypo-key"></i>
                <input name="ConfirmacaoPassword" asp-for="@Model.ConfirmacaoPassword" type="password" class="form-control" placeholder="Confirmação de Senha" autocomplete="off" data-validate="required" />
                <i class="entypo-login"></i>
```
