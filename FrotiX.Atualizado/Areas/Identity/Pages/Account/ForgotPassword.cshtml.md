# Areas/Identity/Pages/Account/ForgotPassword.cshtml

**Mudanca:** MEDIA | **+18** linhas | **-0** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/ForgotPassword.cshtml
+++ ATUAL: Areas/Identity/Pages/Account/ForgotPassword.cshtml
@@ -18,12 +18,14 @@
 <div class="col-xl-6 ml-auto mr-auto">
     <div class="card p-4 rounded-plus bg-faded">
         <form id="js-login" method="post">
+
             <div class="form-group">
                 <label class="form-label" asp-for="Input.Email">Your username or email</label>
                 <input type="email" asp-for="Input.Email" class="form-control" placeholder="Recovery email" required>
                 <span class="invalid-feedback" asp-validation-for="Input.Email">No, you missed this one.</span>
                 <div class="help-block">We will email you the instructions on how to reset your password.</div>
             </div>
+
             <div class="row no-gutters">
                 <div class="col-md-4 ml-auto text-right">
                     <button id="js-login-btn" type="submit" class="btn btn-vinho">Recover</button>
@@ -36,6 +38,24 @@
 @section Scripts {
     <partial name="_ValidationScriptsPartial" />
     <script>
+        /****************************************************************************************
+         * ⚡ FUNÇÃO: click #js-login-btn
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Validar formulário de recuperação de senha antes do submit usando
+         * API de validação HTML5 + classes Bootstrap.
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
         * 🎯 OBJETIVO : Validar formulário de recuperação de senha antes do submit usando
         * API de validação HTML5 + classes Bootstrap.
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
