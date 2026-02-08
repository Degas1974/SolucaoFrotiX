# Areas/Identity/Pages/Account/Login.cshtml

**Mudanca:** MEDIA | **+19** linhas | **-0** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/Login.cshtml
+++ ATUAL: Areas/Identity/Pages/Account/Login.cshtml
@@ -10,6 +10,7 @@
 
 @section PageHeading {
     <div class="col col-md-6 col-lg-7 hidden-sm-down">
+
         <h2 class="fs-xxl fw-500 text-white">
             The simplest UI toolkit for developers &amp; programmers
             <small class="h3 fw-300 mt-3 mb-5 text-white opacity-60">
@@ -17,6 +18,7 @@
             </small>
         </h2>
         <a href="#" class="fs-lg fw-500 text-white opacity-70">Learn more &gt;&gt;</a>
+
         <div class="d-sm-flex flex-column align-items-center justify-content-center d-md-block">
             <div class="px-0 py-1 mt-5 text-white fs-nano opacity-50">
                 Find us on social media
@@ -40,35 +42,42 @@
 }
 
 <div class="col-sm-12 col-md-6 col-lg-5 col-xl-4 ml-auto">
+
     <h1 class="text-white fw-300 mb-3 d-sm-block d-md-none">
         Secure login
     </h1>
     <div class="card p-4 rounded-plus bg-faded">
         <form id="js-login" method="post">
+
             <div asp-validation-summary="All" class="alert alert-primary text-dark"></div>
+
             <div class="form-group">
                 <label class="form-label" asp-for="Input.Ponto">Username</label>
                 <input type="text" asp-for="Input.Ponto" class="form-control" placeholder="Insira seu ponto" value="" required="required" />
                 <span class="invalid-feedback" asp-validation-for="Input.Ponto">Sorry, you missed this one.</span>
                 <div class="help-block">The email address you registered with</div>
             </div>
+
             <div class="form-group">
                 <label class="form-label" asp-for="Input.Password">Password</label>
                 <input type="password" asp-for="Input.Password" class="form-control" placeholder="Insira sua senha da Redecamara" value="" required="required" />
                 <span class="invalid-feedback" asp-validation-for="Input.Password">Sorry, you missed this one.</span>
                 <div class="help-block">Insira a senha da Redecamara para acesso aos seus emails</div>
             </div>
+
             <div class="form-group text-left">
                 <div class="custom-control custom-checkbox">
                     <input type="checkbox" class="custom-control-input" asp-for="Input.RememberMe" />
                     <label class="custom-control-label" asp-for="Input.RememberMe">Remember me for the next 30 days</label>
                 </div>
             </div>
+
             <div class="row no-gutters">
                 <div class="col-lg-6 pl-lg-1 my-2">
                     <button id="js-login-btn" type="submit" class="btn btn-primary btn-block btn-lg">Secure login</button>
                 </div>
             </div>
+
             <div class="text-center">
                 <a asp-page="ForgotPassword" class="opacity-90">Esqueceu sua senha?</a>
             </div>
@@ -79,6 +88,26 @@
 @section Scripts {
     <partial name="_ValidationScriptsPartial" />
     <script>
+        /****************************************************************************************
+         * ⚡ FUNÇÃO: click #js-logsin-btn (BUG: seletor incorreto)
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Validar formulário de login antes do submit usando API de
+         * validação HTML5 + classes Bootstrap.
+         *
+         * 📥 ENTRADAS : [Event] event - Evento de clique do botão
+         *
+         * 📤 SAÍDAS : Form validado visualmente (classe 'was-validated')
+         *
+         * 🔗 CHAMADA POR : Evento click do botão #js-logsin-btn
+         *
+         * 🔄 CHAMA : form.checkValidity(), form.addClass()
+         *
+         * 📦 DEPENDÊNCIAS : jQuery, Bootstrap Validation
+         *
+         * 📝 OBSERVAÇÕES : BUG: Seletor é "#js-logsin-btn" mas botão tem id="js-login-btn"
+         * (falta 'n'). Validação JavaScript NÃO está funcionando por este typo.
+         ****************************************************************************************/
+
         $("#js-logsin-btn").click(function (event) {
 
             var form = $("#js-login");
```

### ADICIONAR ao Janeiro

```html
        /****************************************************************************************
         * ⚡ FUNÇÃO: click #js-logsin-btn (BUG: seletor incorreto)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO : Validar formulário de login antes do submit usando API de
         * validação HTML5 + classes Bootstrap.
         *
         * 📥 ENTRADAS : [Event] event - Evento de clique do botão
         *
         * 📤 SAÍDAS : Form validado visualmente (classe 'was-validated')
         *
         * 🔗 CHAMADA POR : Evento click do botão #js-logsin-btn
         *
         * 🔄 CHAMA : form.checkValidity(), form.addClass()
         *
         * 📦 DEPENDÊNCIAS : jQuery, Bootstrap Validation
         *
         * 📝 OBSERVAÇÕES : BUG: Seletor é "#js-logsin-btn" mas botão tem id="js-login-btn"
         * (falta 'n'). Validação JavaScript NÃO está funcionando por este typo.
         ****************************************************************************************/
```
