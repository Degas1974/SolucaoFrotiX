# Areas/Identity/Pages/_Layout.cshtml

**Mudanca:** PEQUENA | **+0** linhas | **-0** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/_Layout.cshtml
+++ ATUAL: Areas/Identity/Pages/_Layout.cshtml
@@ -1,24 +1,33 @@
 <!DOCTYPE html>
 <html lang="en">
 <head>
+
     <partial name="_Head" />
+
     @RenderSection("Header", required: false)
 </head>
 <body>
+
     <partial name="_CookieConsentPartial" />
 
     <div class="page-wrapper">
         <div class="page-inner bg-brand-gradient">
             <div class="page-content-wrapper bg-transparent m-0">
+
                 <partial name="_PageHeader" />
+
                 <div class="flex-1" style="background: url('/img/svg/pattern-1.svg') no-repeat center bottom fixed; background-size: cover;">
                     <div class="container py-4 my-lg-5 px-4 px-sm-0">
                         <div class="row">
+
                             @RenderSection("PageHeading", required: false)
+
                             @RenderBody()
                         </div>
                     </div>
+
                     <partial name="_PageFooter" />
+
                     <partial name="_ColorProfileReference" />
                 </div>
             </div>
@@ -26,7 +35,9 @@
     </div>
 
     <partial name="_GoogleAnalytics" />
+
     <partial name="_ScriptsBasePlugins" />
+
     @RenderSection("Scripts", required: false)
 </body>
 </html>
```
