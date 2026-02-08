# Pages/Shared/_Contact.cshtml

**Mudanca:** GRANDE | **+22** linhas | **-57** linhas

---

```diff
--- JANEIRO: Pages/Shared/_Contact.cshtml
+++ ATUAL: Pages/Shared/_Contact.cshtml
@@ -1,5 +1,4 @@
-<div id="js-chat-contact"
-    class="flex-wrap position-relative slide-on-mobile slide-on-mobile-left border-faded border-left-0 border-top-0 border-bottom-0">
+<div id="js-chat-contact" class="flex-wrap position-relative slide-on-mobile slide-on-mobile-left border-faded border-left-0 border-top-0 border-bottom-0">
     <div class="d-flex flex-column bg-faded position-absolute pos-top pos-bottom w-100">
         <div class="px-3 py-4">
             <input type="text" class="form-control bg-white" placeholder="Search key words">
@@ -10,9 +9,7 @@
                 <ul class="list-unstyled m-0">
                     <li>
                         <div class="d-flex w-100 px-3 py-2 text-dark hover-white cursor-pointer show-child-on-hover">
-                            <div class="profile-image-md rounded-circle"
-                                style="background-image:url('/img/demo/avatars/avatar-d.png'); background-size: cover;">
-                            </div>
+                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-d.png'); background-size: cover;"></div>
                             <div class="px-2 flex-1">
                                 <div class="text-truncate text-truncate-md">
                                     Tracey Chang
@@ -22,20 +19,15 @@
                                 </div>
                             </div>
                             <div class="position-absolute pos-right mt-2 mr-3 show-on-hover-parent">
-                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0"
-                                    data-bs-toggle="tooltip"
-                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>"
-                                    data-original-title="Delete">
-                                    <i class="fa-duotone fa-trash-alt"></i>
+                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>" data-original-title="Delete">
+                                    <i class="@(Settings.Theme.IconPrefix) fa-trash-alt"></i>
                                 </button>
                             </div>
                         </div>
                     </li>
                     <li>
                         <div class="d-flex w-100 px-3 py-2 text-dark hover-white cursor-pointer show-child-on-hover">
-                            <div class="profile-image-md rounded-circle"
-                                style="background-image:url('/img/demo/avatars/avatar-b.png'); background-size: cover;">
-                            </div>
+                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-b.png'); background-size: cover;"></div>
                             <div class="px-2 flex-1">
                                 <div class="text-truncate text-truncate-md">
                                     Oliver Kopyuv
@@ -45,20 +37,15 @@
                                 </div>
                             </div>
                             <div class="position-absolute pos-right mt-2 mr-3 show-on-hover-parent">
-                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0"
-                                    data-bs-toggle="tooltip"
-                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>"
-                                    data-original-title="Delete">
-                                    <i class="fa-duotone fa-trash-alt"></i>
+                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>" data-original-title="Delete">
+                                    <i class="@(Settings.Theme.IconPrefix) fa-trash-alt"></i>
                                 </button>
                             </div>
                         </div>
                     </li>
                     <li>
                         <div class="d-flex w-100 px-3 py-2 text-dark hover-white cursor-pointer show-child-on-hover">
-                            <div class="profile-image-md rounded-circle"
-                                style="background-image:url('/img/demo/avatars/avatar-e.png'); background-size: cover;">
-                            </div>
+                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-e.png'); background-size: cover;"></div>
                             <div class="px-2 flex-1">
                                 <div class="text-truncate text-truncate-md">
                                     Dr. John Cook PhD
@@ -68,20 +55,15 @@
                                 </div>
                             </div>
                             <div class="position-absolute pos-right mt-2 mr-3 show-on-hover-parent">
-                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0"
-                                    data-bs-toggle="tooltip"
-                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>"
-                                    data-original-title="Delete">
-                                    <i class="fa-duotone fa-trash-alt"></i>
+                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>" data-original-title="Delete">
+                                    <i class="@(Settings.Theme.IconPrefix) fa-trash-alt"></i>
                                 </button>
                             </div>
                         </div>
                     </li>
                     <li>
                         <div class="d-flex w-100 px-3 py-2 text-dark hover-white cursor-pointer show-child-on-hover">
-                            <div class="profile-image-md rounded-circle"
-                                style="background-image:url('/img/demo/avatars/avatar-g.png'); background-size: cover;">
-                            </div>
+                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-g.png'); background-size: cover;"></div>
                             <div class="px-2 flex-1">
                                 <div class="text-truncate text-truncate-md">
                                     Ali Amdaney
@@ -91,11 +73,8 @@
                                 </div>
                             </div>
                             <div class="position-absolute pos-right mt-2 mr-3 show-on-hover-parent">
-                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0"
-                                    data-bs-toggle="tooltip"
-                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>"
-                                    data-original-title="Delete">
-                                    <i class="fa-duotone fa-trash-alt"></i>
+                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>" data-original-title="Delete">
+                                    <i class="@(Settings.Theme.IconPrefix) fa-trash-alt"></i>
                                 </button>
                             </div>
                         </div>
@@ -105,9 +84,7 @@
                 <ul class="list-unstyled m-0">
                     <li>
                         <div class="d-flex w-100 px-3 py-2 text-dark hover-white cursor-pointer show-child-on-hover">
-                            <div class="profile-image-md rounded-circle"
-                                style="background-image:url('/img/demo/avatars/avatar-m.png'); background-size: cover;">
-                            </div>
+                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-m.png'); background-size: cover;"></div>
                             <div class="px-2 flex-1">
                                 <div class="text-truncate text-truncate-md">
                                     +714651347790
@@ -117,26 +94,18 @@
                                 </div>
                             </div>
                             <div class="position-absolute pos-right mt-2 mr-3 show-on-hover-parent">
-                                <button class="btn btn-verde btn-xs btn-icon rounded-circle shadow-0"
-                                    data-bs-toggle="tooltip"
-                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-success-500&quot;></div></div>"
-                                    data-original-title="Play voicemail">
-                                    <i class="fa-duotone fa-play"></i>
+                                <button class="btn btn-verde btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-success-500&quot;></div></div>" data-original-title="Play voicemail">
+                                    <i class="@(Settings.Theme.IconPrefix) fa-play"></i>
                                 </button>
-                                <button class="btn btn-info btn-xs btn-icon rounded-circle shadow-0"
-                                    data-bs-toggle="tooltip"
-                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-info-500&quot;></div></div>"
-                                    data-original-title="Call back">
-                                    <i class="fa-duotone fa-phone"></i>
+                                <button class="btn btn-info btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-info-500&quot;></div></div>" data-original-title="Call back">
+                                    <i class="@(Settings.Theme.IconPrefix) fa-phone"></i>
                                 </button>
                             </div>
                         </div>
                     </li>
                     <li>
                         <div class="d-flex w-100 px-3 py-2 text-dark hover-white cursor-pointer show-child-on-hover">
-                            <div class="profile-image-md rounded-circle"
-                                style="background-image:url('/img/demo/avatars/avatar-m.png'); background-size: cover;">
-                            </div>
+                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-m.png'); background-size: cover;"></div>
                             <div class="px-2 flex-1">
                                 <div class="text-truncate text-truncate-md">
                                     +13471349199
@@ -146,11 +115,8 @@
                                 </div>
                             </div>
                             <div class="position-absolute pos-right mt-2 mr-3 show-on-hover-parent">
-                                <button class="btn btn-info btn-xs btn-icon rounded-circle shadow-0"
-                                    data-bs-toggle="tooltip"
-                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-info-500&quot;></div></div>"
-                                    data-original-title="Call back">
-                                    <i class="fa-duotone fa-phone"></i>
+                                <button class="btn btn-info btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-info-500&quot;></div></div>" data-original-title="Call back">
+                                    <i class="@(Settings.Theme.IconPrefix) fa-phone"></i>
                                 </button>
                             </div>
                         </div>
@@ -160,5 +126,4 @@
         </div>
     </div>
 </div>
-<div class="slide-backdrop" data-action="toggle" data-class="slide-on-mobile-left-show"
-    data-bs-target="#js-chat-contact"></div>
+<div class="slide-backdrop" data-action="toggle" data-class="slide-on-mobile-left-show" data-bs-target="#js-chat-contact"></div>
```

### REMOVER do Janeiro

```html
<div id="js-chat-contact"
    class="flex-wrap position-relative slide-on-mobile slide-on-mobile-left border-faded border-left-0 border-top-0 border-bottom-0">
                            <div class="profile-image-md rounded-circle"
                                style="background-image:url('/img/demo/avatars/avatar-d.png'); background-size: cover;">
                            </div>
                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0"
                                    data-bs-toggle="tooltip"
                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>"
                                    data-original-title="Delete">
                                    <i class="fa-duotone fa-trash-alt"></i>
                            <div class="profile-image-md rounded-circle"
                                style="background-image:url('/img/demo/avatars/avatar-b.png'); background-size: cover;">
                            </div>
                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0"
                                    data-bs-toggle="tooltip"
                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>"
                                    data-original-title="Delete">
                                    <i class="fa-duotone fa-trash-alt"></i>
                            <div class="profile-image-md rounded-circle"
                                style="background-image:url('/img/demo/avatars/avatar-e.png'); background-size: cover;">
                            </div>
                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0"
                                    data-bs-toggle="tooltip"
                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>"
                                    data-original-title="Delete">
                                    <i class="fa-duotone fa-trash-alt"></i>
                            <div class="profile-image-md rounded-circle"
                                style="background-image:url('/img/demo/avatars/avatar-g.png'); background-size: cover;">
                            </div>
                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0"
                                    data-bs-toggle="tooltip"
                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>"
                                    data-original-title="Delete">
                                    <i class="fa-duotone fa-trash-alt"></i>
                            <div class="profile-image-md rounded-circle"
                                style="background-image:url('/img/demo/avatars/avatar-m.png'); background-size: cover;">
                            </div>
                                <button class="btn btn-verde btn-xs btn-icon rounded-circle shadow-0"
                                    data-bs-toggle="tooltip"
                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-success-500&quot;></div></div>"
                                    data-original-title="Play voicemail">
                                    <i class="fa-duotone fa-play"></i>
                                <button class="btn btn-info btn-xs btn-icon rounded-circle shadow-0"
                                    data-bs-toggle="tooltip"
                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-info-500&quot;></div></div>"
                                    data-original-title="Call back">
                                    <i class="fa-duotone fa-phone"></i>
                            <div class="profile-image-md rounded-circle"
                                style="background-image:url('/img/demo/avatars/avatar-m.png'); background-size: cover;">
                            </div>
                                <button class="btn btn-info btn-xs btn-icon rounded-circle shadow-0"
                                    data-bs-toggle="tooltip"
                                    data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-info-500&quot;></div></div>"
                                    data-original-title="Call back">
                                    <i class="fa-duotone fa-phone"></i>
<div class="slide-backdrop" data-action="toggle" data-class="slide-on-mobile-left-show"
    data-bs-target="#js-chat-contact"></div>
```


### ADICIONAR ao Janeiro

```html
<div id="js-chat-contact" class="flex-wrap position-relative slide-on-mobile slide-on-mobile-left border-faded border-left-0 border-top-0 border-bottom-0">
                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-d.png'); background-size: cover;"></div>
                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>" data-original-title="Delete">
                                    <i class="@(Settings.Theme.IconPrefix) fa-trash-alt"></i>
                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-b.png'); background-size: cover;"></div>
                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>" data-original-title="Delete">
                                    <i class="@(Settings.Theme.IconPrefix) fa-trash-alt"></i>
                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-e.png'); background-size: cover;"></div>
                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>" data-original-title="Delete">
                                    <i class="@(Settings.Theme.IconPrefix) fa-trash-alt"></i>
                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-g.png'); background-size: cover;"></div>
                                <button class="btn btn-vinho btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-danger-500&quot;></div></div>" data-original-title="Delete">
                                    <i class="@(Settings.Theme.IconPrefix) fa-trash-alt"></i>
                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-m.png'); background-size: cover;"></div>
                                <button class="btn btn-verde btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-success-500&quot;></div></div>" data-original-title="Play voicemail">
                                    <i class="@(Settings.Theme.IconPrefix) fa-play"></i>
                                <button class="btn btn-info btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-info-500&quot;></div></div>" data-original-title="Call back">
                                    <i class="@(Settings.Theme.IconPrefix) fa-phone"></i>
                            <div class="profile-image-md rounded-circle" style="background-image:url('/img/demo/avatars/avatar-m.png'); background-size: cover;"></div>
                                <button class="btn btn-info btn-xs btn-icon rounded-circle shadow-0" data-bs-toggle="tooltip" data-template="<div class=&quot;tooltip&quot; role=&quot;tooltip&quot;><div class=&quot;tooltip-inner bg-info-500&quot;></div></div>" data-original-title="Call back">
                                    <i class="@(Settings.Theme.IconPrefix) fa-phone"></i>
<div class="slide-backdrop" data-action="toggle" data-class="slide-on-mobile-left-show" data-bs-target="#js-chat-contact"></div>
```
