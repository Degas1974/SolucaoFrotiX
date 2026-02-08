# Areas/Identity/Pages/Account/Lockout.cshtml

**Mudanca:** MEDIA | **+18** linhas | **-0** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/Lockout.cshtml
+++ ATUAL: Areas/Identity/Pages/Account/Lockout.cshtml
@@ -6,9 +6,11 @@
 
 <div class="container py-4 py-lg-5 my-lg-5 px-4 px-sm-0 text-white d-flex align-items-center justify-content-center">
     <form id="js-login" role="form" class="text-center text-white mb-5 pb-5" method="post">
+
         <div class="py-3">
             <img src="~/img/demo/avatars/avatar-admin-lg.png" class="img-responsive rounded-circle img-thumbnail" alt="thumbnail">
         </div>
+
         <div class="form-group">
             <h3>
                 @ViewBag.User
@@ -17,7 +19,9 @@
                 </small>
             </h3>
             <p class="text-white opacity-50">Enter password to unlock screen</p>
+
             <div class="input-group input-group-lg">
+
                 <input type="password" asp-for="Input.Password" class="form-control" value="Password123!" required="required" />
                 <span class="invalid-feedback" asp-validation-for="Input.Password">Sorry, you missed this one.</span>
                 <div class="input-group-append">
@@ -25,6 +29,7 @@
                 </div>
             </div>
         </div>
+
         <div class="text-center">
             <a asp-page="Login" class="text-white opacity-90">Not <span class="text-secondary">@ViewBag.User</span>?</a>
         </div>
@@ -34,6 +39,24 @@
 @section Scripts {
     <partial name="_ValidationScriptsPartial" />
     <script>
+        /****************************************************************************************
+         * ⚡ FUNÇÃO: click #js-login-btn
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Validar formulário de desbloqueio antes do submit usando API de
+         * validação HTML5 + classes Bootstrap.
+         *
+         * 📥 ENTRADAS : [Event] event - Evento de clique do botão
+         *
+         * 📤 SAÍDAS : Form validado visualmente (classe 'was-validated')
+         *
+         * 🔗 CHAMADA POR : Evento click do botão #js-login-btn
+         *
+         * 🔄 CHAMA : form.checkValidity(), form.addClass()
+         *
+         * 📦 DEPENDÊNCIAS : jQuery, Bootstrap Validation
+         *
+         * 📝 OBSERVAÇÕES : Validação ocorre no cliente antes do POST ao servidor.
+         ****************************************************************************************/
         $("#js-login-btn").click(function (event) {
 
             var form = $("#js-login");
```

### ADICIONAR ao Janeiro

```html
        /****************************************************************************************
         * ⚡ FUNÇÃO: click #js-login-btn
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO : Validar formulário de desbloqueio antes do submit usando API de
         * validação HTML5 + classes Bootstrap.
         *
         * 📥 ENTRADAS : [Event] event - Evento de clique do botão
         *
         * 📤 SAÍDAS : Form validado visualmente (classe 'was-validated')
         *
         * 🔗 CHAMADA POR : Evento click do botão #js-login-btn
         *
         * 🔄 CHAMA : form.checkValidity(), form.addClass()
         *
         * 📦 DEPENDÊNCIAS : jQuery, Bootstrap Validation
         *
         * 📝 OBSERVAÇÕES : Validação ocorre no cliente antes do POST ao servidor.
         ****************************************************************************************/
```
