# Pages/Shared/_DropdownApp.cshtml

**Mudanca:** MEDIA | **+6** linhas | **-6** linhas

---

```diff
--- JANEIRO: Pages/Shared/_DropdownApp.cshtml
+++ ATUAL: Pages/Shared/_DropdownApp.cshtml
@@ -83,7 +83,7 @@
                     <i class="base-4 icon-stack-3x color-fusion-400"></i>
                     <i class="base-5 icon-stack-2x color-fusion-200"></i>
                     <i class="base-5 icon-stack-1x color-fusion-100"></i>
-                    <i class="fa-duotone fa-keyboard icon-stack-1x color-info-50"></i>
+                    <i class="@(Settings.Theme.IconPrefix) fa-keyboard icon-stack-1x color-info-50"></i>
                 </span>
                 <span class="app-list-name">
                     Notes
@@ -96,7 +96,7 @@
                     <i class="base-16 icon-stack-3x color-fusion-500"></i>
                     <i class="base-10 icon-stack-1x color-primary-50 opacity-30"></i>
                     <i class="base-10 icon-stack-1x fs-xl color-primary-50 opacity-20"></i>
-                    <i class="fa-duotone fa-dot-circle icon-stack-1x text-white opacity-85"></i>
+                    <i class="@(Settings.Theme.IconPrefix) fa-dot-circle icon-stack-1x text-white opacity-85"></i>
                 </span>
                 <span class="app-list-name">
                     Photos
@@ -110,7 +110,7 @@
                     <i class="base-7 icon-stack-2x color-primary-300"></i>
                     <i class="base-7 icon-stack-1x fs-xxl color-primary-200"></i>
                     <i class="base-7 icon-stack-1x color-primary-500"></i>
-                    <i class="fa-duotone fa-globe icon-stack-1x text-white opacity-85"></i>
+                    <i class="@(Settings.Theme.IconPrefix) fa-globe icon-stack-1x text-white opacity-85"></i>
                 </span>
                 <span class="app-list-name">
                     Maps
@@ -122,7 +122,7 @@
                 <span class="icon-stack">
                     <i class="base-5 icon-stack-3x color-success-700 opacity-80"></i>
                     <i class="base-12 icon-stack-2x color-success-700 opacity-30"></i>
-                    <i class="fa-duotone fa-comment-alt icon-stack-1x text-white"></i>
+                    <i class="@(Settings.Theme.IconPrefix) fa-comment-alt icon-stack-1x text-white"></i>
                 </span>
                 <span class="app-list-name">
                     Chat
@@ -134,7 +134,7 @@
                 <span class="icon-stack">
                     <i class="base-5 icon-stack-3x color-warning-600"></i>
                     <i class="base-7 icon-stack-2x color-warning-800 opacity-50"></i>
-                    <i class="fa-duotone fa-phone icon-stack-1x text-white"></i>
+                    <i class="@(Settings.Theme.IconPrefix) fa-phone icon-stack-1x text-white"></i>
                 </span>
                 <span class="app-list-name">
                     Phone
@@ -145,7 +145,7 @@
             <a href="#" class="app-list-item hover-white">
                 <span class="icon-stack">
                     <i class="base-6 icon-stack-3x color-danger-600"></i>
-                    <i class="fa-duotone fa-chart-line icon-stack-1x text-white"></i>
+                    <i class="@(Settings.Theme.IconPrefix) fa-chart-line icon-stack-1x text-white"></i>
                 </span>
                 <span class="app-list-name">
                     Projects
```

### REMOVER do Janeiro

```html
                    <i class="fa-duotone fa-keyboard icon-stack-1x color-info-50"></i>
                    <i class="fa-duotone fa-dot-circle icon-stack-1x text-white opacity-85"></i>
                    <i class="fa-duotone fa-globe icon-stack-1x text-white opacity-85"></i>
                    <i class="fa-duotone fa-comment-alt icon-stack-1x text-white"></i>
                    <i class="fa-duotone fa-phone icon-stack-1x text-white"></i>
                    <i class="fa-duotone fa-chart-line icon-stack-1x text-white"></i>
```


### ADICIONAR ao Janeiro

```html
                    <i class="@(Settings.Theme.IconPrefix) fa-keyboard icon-stack-1x color-info-50"></i>
                    <i class="@(Settings.Theme.IconPrefix) fa-dot-circle icon-stack-1x text-white opacity-85"></i>
                    <i class="@(Settings.Theme.IconPrefix) fa-globe icon-stack-1x text-white opacity-85"></i>
                    <i class="@(Settings.Theme.IconPrefix) fa-comment-alt icon-stack-1x text-white"></i>
                    <i class="@(Settings.Theme.IconPrefix) fa-phone icon-stack-1x text-white"></i>
                    <i class="@(Settings.Theme.IconPrefix) fa-chart-line icon-stack-1x text-white"></i>
```
