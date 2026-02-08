# Areas/Identity/Pages/Account/LoginFrotiX.cshtml

**Mudanca:** GRANDE | **+15** linhas | **-26** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/Account/LoginFrotiX.cshtml
+++ ATUAL: Areas/Identity/Pages/Account/LoginFrotiX.cshtml
@@ -6,14 +6,8 @@
 }
 
 <style>
-    :root {
-        --fa-primary-color: #ff6b35;
-        --fa-secondary-color: #6c757d;
-    }
-
     .forgot-password-link {
-        margin-top: 200px;
-        /* Ajuste o valor conforme necessário */
+        margin-top: 200px; /* Ajuste o valor conforme necessário */
     }
 </style>
 
@@ -26,52 +20,47 @@
 
     <form method="post" id="form_login" autocomplete="off">
         @Html.AntiForgeryToken()
+
         <div class="form-group">
+            <div class="input-group">
+                <div class="input-group-addon">
+                    <i class="entypo-user"></i>
+                </div>
+                <input name="Ponto" asp-for="Input.Ponto" class="form-control" placeholder="Insira seu ponto" autocomplete="off" data-validate="required"/>
 
-            <div class="input-group">
-                <div class="input-group-addon" style="padding: 10px; background: #eee; border: 1px solid #ccc;">
-                    <i class="fa-duotone fa-user fa-lg"></i>
-                </div>
-                <input name="Ponto" asp-for="Input.Ponto" class="form-control" placeholder="Insira seu ponto"
-                    autocomplete="off" data-validate="required" />
             </div>
-
         </div>
 
         <div class="form-group">
+            <div class="input-group">
+                <div class="input-group-addon">
+                    <i class="entypo-key"></i>
+                </div>
+                <input name="Password" asp-for="Input.Password" type="password" class="form-control" placeholder="Senha" autocomplete="off" data-validate="required"/>
 
-            <div class="input-group">
-                <div class="input-group-addon" style="padding: 10px; background: #eee; border: 1px solid #ccc;">
-                    <i class="fa-duotone fa-key fa-lg"></i>
-                </div>
-                <input name="Password" asp-for="Input.Password" type="password" class="form-control" placeholder="Senha"
-                    autocomplete="off" data-validate="required" />
             </div>
-
         </div>
 
         <button class="login-button">
             <div class="brushed-effect"></div>
             <div class="button-text">
+
                 <svg class="login-icon" viewBox="0 0 24 24">
-                    <circle cx="12" cy="6" r="3" fill="none" stroke="currentColor" stroke-width="2"
-                        stroke-linecap="round" />
-                    <path d="M8 14c0-2.21 1.79-4 4-4s4 1.79 4 4v6H8v-6z" fill="none" stroke="currentColor"
-                        stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
+                    <circle cx="12" cy="6" r="3" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" />
+                    <path d="M8 14c0-2.21 1.79-4 4-4s4 1.79 4 4v6H8v-6z" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                     <path d="M8 16v2M16 16v2" stroke="currentColor" stroke-width="2" stroke-linecap="round" />
-                    <ellipse cx="12" cy="21" rx="8" ry="1.5" fill="none" stroke="currentColor" stroke-width="1.5"
-                        opacity="0.6" />
+                    <ellipse cx="12" cy="21" rx="8" ry="1.5" fill="none" stroke="currentColor" stroke-width="1.5" opacity="0.6" />
                 </svg>
                 Entrar no Sistema
             </div>
         </button>
-
     </form>
 
     <div class="login-bottom-links">
 
         <a href="extra-forgot-password.html" class="forgot-password-link">
             <div class="link-text">
+
                 <svg class="pilot-icon" viewBox="0 0 24 24">
 
                     <path d="M6 8c0-1 2-3 6-3s6 2 6 3l-1 1H7l-1-1z" fill="currentColor" opacity="0.3" />
@@ -82,8 +71,7 @@
                     <circle cx="12" cy="11" r="2.5" fill="none" stroke="currentColor" stroke-width="1.5" />
 
                     <path d="M8.5 15c0-1.5 1.5-3 3.5-3s3.5 1.5 3.5 3v5H8.5v-5z" fill="currentColor" opacity="0.2" />
-                    <path d="M8.5 15c0-1.5 1.5-3 3.5-3s3.5 1.5 3.5 3v5H8.5v-5z" fill="none" stroke="currentColor"
-                        stroke-width="1.5" />
+                    <path d="M8.5 15c0-1.5 1.5-3 3.5-3s3.5 1.5 3.5 3v5H8.5v-5z" fill="none" stroke="currentColor" stroke-width="1.5" />
 
                     <path d="M12 14v4" stroke="currentColor" stroke-width="1.2" />
                     <path d="M11.2 14.5l1.6 0M11.4 16l1.2 0" stroke="currentColor" stroke-width="0.8" />
```

### REMOVER do Janeiro

```html
    :root {
        --fa-primary-color: #ff6b35;
        --fa-secondary-color: #6c757d;
    }
        margin-top: 200px;
        /* Ajuste o valor conforme necessário */
            <div class="input-group">
                <div class="input-group-addon" style="padding: 10px; background: #eee; border: 1px solid #ccc;">
                    <i class="fa-duotone fa-user fa-lg"></i>
                </div>
                <input name="Ponto" asp-for="Input.Ponto" class="form-control" placeholder="Insira seu ponto"
                    autocomplete="off" data-validate="required" />
            <div class="input-group">
                <div class="input-group-addon" style="padding: 10px; background: #eee; border: 1px solid #ccc;">
                    <i class="fa-duotone fa-key fa-lg"></i>
                </div>
                <input name="Password" asp-for="Input.Password" type="password" class="form-control" placeholder="Senha"
                    autocomplete="off" data-validate="required" />
                    <circle cx="12" cy="6" r="3" fill="none" stroke="currentColor" stroke-width="2"
                        stroke-linecap="round" />
                    <path d="M8 14c0-2.21 1.79-4 4-4s4 1.79 4 4v6H8v-6z" fill="none" stroke="currentColor"
                        stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                    <ellipse cx="12" cy="21" rx="8" ry="1.5" fill="none" stroke="currentColor" stroke-width="1.5"
                        opacity="0.6" />
                    <path d="M8.5 15c0-1.5 1.5-3 3.5-3s3.5 1.5 3.5 3v5H8.5v-5z" fill="none" stroke="currentColor"
                        stroke-width="1.5" />
```


### ADICIONAR ao Janeiro

```html
        margin-top: 200px; /* Ajuste o valor conforme necessário */
            <div class="input-group">
                <div class="input-group-addon">
                    <i class="entypo-user"></i>
                </div>
                <input name="Ponto" asp-for="Input.Ponto" class="form-control" placeholder="Insira seu ponto" autocomplete="off" data-validate="required"/>
            <div class="input-group">
                <div class="input-group-addon">
                    <i class="entypo-key"></i>
                </div>
                <input name="Password" asp-for="Input.Password" type="password" class="form-control" placeholder="Senha" autocomplete="off" data-validate="required"/>
                    <circle cx="12" cy="6" r="3" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" />
                    <path d="M8 14c0-2.21 1.79-4 4-4s4 1.79 4 4v6H8v-6z" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                    <ellipse cx="12" cy="21" rx="8" ry="1.5" fill="none" stroke="currentColor" stroke-width="1.5" opacity="0.6" />
                    <path d="M8.5 15c0-1.5 1.5-3 3.5-3s3.5 1.5 3.5 3v5H8.5v-5z" fill="none" stroke="currentColor" stroke-width="1.5" />
```
