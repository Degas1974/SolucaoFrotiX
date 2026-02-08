# Pages/Shared/_DropdownMenu.cshtml

**Mudanca:** PEQUENA | **+1** linhas | **-2** linhas

---

```diff
--- JANEIRO: Pages/Shared/_DropdownMenu.cshtml
+++ ATUAL: Pages/Shared/_DropdownMenu.cshtml
@@ -5,8 +5,7 @@
             <img src="~/Images/barbudo.jpg" class="rounded-circle profile-image" alt="@(Settings.Theme.User)">
         </span>
         <div class="info-card-text">
-            <div id="divUser" class="fs-lg text-truncate text-truncate-lg" data-default="@(Settings.Theme.User)">
-                @(Settings.Theme.User)</div>
+            <div id="divUser" class="fs-lg text-truncate text-truncate-lg">@(Settings.Theme.User)</div>
 
         </div>
     </div>
```

### REMOVER do Janeiro

```html
            <div id="divUser" class="fs-lg text-truncate text-truncate-lg" data-default="@(Settings.Theme.User)">
                @(Settings.Theme.User)</div>
```


### ADICIONAR ao Janeiro

```html
            <div id="divUser" class="fs-lg text-truncate text-truncate-lg">@(Settings.Theme.User)</div>
```
