# Pages/Shared/_CookieConsentPartial.cshtml

**Mudanca:** MEDIA | **+7** linhas | **-18** linhas

---

```diff
--- JANEIRO: Pages/Shared/_CookieConsentPartial.cshtml
+++ ATUAL: Pages/Shared/_CookieConsentPartial.cshtml
@@ -9,36 +9,23 @@
 
 @if (showBanner)
 {
-    <div id="cookieConsent"
-        class="alert bg-fusion-500 alert-dismissible fade show position-fixed pos-top pos-left pos-right rounded-0 border-0 m-0 shadow-lg"
-        role="alert" style="z-index: 999999;margin: 0 !important;">
+    <div id="cookieConsent" class="alert bg-fusion-500 alert-dismissible fade show position-fixed pos-top pos-left pos-right rounded-0 border-0 m-0 shadow-lg" role="alert" style="z-index: 999999;margin: 0 !important;">
         <button type="button" class="close" data-dismiss="alert" aria-label="Close">
             <span aria-hidden="true"><i class="fa-duotone fa-times"></i></span>
         </button>
         <h4 class="m-0">This website is using cookies.</h4>
-        We use them to give you the best experience. If you continue using our website, we'll assume that you are happy to
-        receive all cookies on this website.
+        We use them to give you the best experience. If you continue using our website, we'll assume that you are happy to receive all cookies on this website.
         <div class="d-flex mt-3">
-            <button class="btn btn-sm btn-terracota mr-2" data-dismiss="alert" aria-label="Close"
-                data-cookie-string="@cookieString">Accept</button>
+            <button class="btn btn-sm btn-terracota mr-2" data-dismiss="alert" aria-label="Close" data-cookie-string="@cookieString">Accept</button>
             <button class="btn btn-sm btn-light">Learn more</button>
         </div>
     </div>
     <script>
         (function () {
-            try {
-
-                var button = document.querySelector("#cookieConsent button[data-cookie-string]");
-                if (!button) {
-                    return;
-                }
-
-                button.addEventListener("click", function () {
-                    document.cookie = button.dataset.cookieString;
-                }, false);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("_CookieConsentPartial.cshtml", "cookieConsent.init", error);
-            }
+            var button = document.querySelector("#cookieConsent button[data-cookie-string]");
+            button.addEventListener("click", function () {
+                document.cookie = button.dataset.cookieString;
+            }, false);
         })();
     </script>
 }
```

### REMOVER do Janeiro

```html
    <div id="cookieConsent"
        class="alert bg-fusion-500 alert-dismissible fade show position-fixed pos-top pos-left pos-right rounded-0 border-0 m-0 shadow-lg"
        role="alert" style="z-index: 999999;margin: 0 !important;">
        We use them to give you the best experience. If you continue using our website, we'll assume that you are happy to
        receive all cookies on this website.
            <button class="btn btn-sm btn-terracota mr-2" data-dismiss="alert" aria-label="Close"
                data-cookie-string="@cookieString">Accept</button>
            try {
                var button = document.querySelector("#cookieConsent button[data-cookie-string]");
                if (!button) {
                    return;
                }
                button.addEventListener("click", function () {
                    document.cookie = button.dataset.cookieString;
                }, false);
            } catch (error) {
                Alerta.TratamentoErroComLinha("_CookieConsentPartial.cshtml", "cookieConsent.init", error);
            }
```


### ADICIONAR ao Janeiro

```html
    <div id="cookieConsent" class="alert bg-fusion-500 alert-dismissible fade show position-fixed pos-top pos-left pos-right rounded-0 border-0 m-0 shadow-lg" role="alert" style="z-index: 999999;margin: 0 !important;">
        We use them to give you the best experience. If you continue using our website, we'll assume that you are happy to receive all cookies on this website.
            <button class="btn btn-sm btn-terracota mr-2" data-dismiss="alert" aria-label="Close" data-cookie-string="@cookieString">Accept</button>
            var button = document.querySelector("#cookieConsent button[data-cookie-string]");
            button.addEventListener("click", function () {
                document.cookie = button.dataset.cookieString;
            }, false);
```
