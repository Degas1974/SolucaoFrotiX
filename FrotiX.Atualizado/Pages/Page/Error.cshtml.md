# Pages/Page/Error.cshtml

**Mudanca:** GRANDE | **+48** linhas | **-61** linhas

---

```diff
--- JANEIRO: Pages/Page/Error.cshtml
+++ ATUAL: Pages/Page/Error.cshtml
@@ -1,13 +1,13 @@
 @page "/Error"
+
 @model ErrorModel
 @{
-    Layout = null;
-    ViewData["Title"] = "Erro - FrotiX";
+Layout = null;
+ViewData["Title"] = "Erro - FrotiX";
 }
 
 <!DOCTYPE html>
 <html lang="pt-BR">
-
 <head>
     <meta http-equiv="X-UA-Compatible" content="IE=edge">
     <meta charset="utf-8">
@@ -26,7 +26,8 @@
     <link rel="stylesheet" href="~/Neon/css/neon-theme.css">
     <link rel="stylesheet" href="~/Neon/css/neon-forms.css">
     <link rel="stylesheet" href="~/Neon/css/custom.css">
-    <link href="~/lib/fontawesome-pro/css/all.css" rel="stylesheet" />
+
+    <script src="https://kit.fontawesome.com/afeb78ad1f.js" crossorigin="anonymous"></script>
 
     <script src="~/Neon/js/jquery-1.11.3.min.js"></script>
 
@@ -96,33 +97,32 @@
             backdrop-filter: blur(10px);
         }
 
-        .btn-error:hover {
-            background: rgba(255, 255, 255, 0.3);
-            border-color: rgba(255, 255, 255, 0.5);
-            color: #ffffff;
-            text-decoration: none;
-            transform: translateY(-2px);
-            box-shadow: 0 10px 20px rgba(0, 0, 0, 0.2);
-        }
-
-        .btn-error.primary {
-            background: #3498db;
-            color: #ffffff;
-            border-color: #3498db;
-        }
-
-        .btn-error.primary:hover {
-            background: #2980b9;
-            color: #ffffff;
-            border-color: #2980b9;
-        }
+            .btn-error:hover {
+                background: rgba(255, 255, 255, 0.3);
+                border-color: rgba(255, 255, 255, 0.5);
+                color: #ffffff;
+                text-decoration: none;
+                transform: translateY(-2px);
+                box-shadow: 0 10px 20px rgba(0, 0, 0, 0.2);
+            }
+
+            .btn-error.primary {
+                background: #3498db;
+                color: #ffffff;
+                border-color: #3498db;
+            }
+
+                .btn-error.primary:hover {
+                    background: #2980b9;
+                    color: #ffffff;
+                    border-color: #2980b9;
+                }
 
         .logo-container {
             margin-bottom: 30px;
         }
 
-        .logo img,
-        .logo canvas {
+        .logo img, .logo canvas {
             max-width: 400px;
             height: auto;
             filter: brightness(1.1) contrast(1.1);
@@ -144,29 +144,29 @@
             animation: float 6s infinite ease-in-out;
         }
 
-        .floating-element:nth-child(1) {
-            top: 20%;
-            left: 10%;
-            animation-delay: 0s;
-        }
-
-        .floating-element:nth-child(2) {
-            top: 60%;
-            right: 15%;
-            animation-delay: 2s;
-        }
-
-        .floating-element:nth-child(3) {
-            bottom: 20%;
-            left: 20%;
-            animation-delay: 4s;
-        }
-
-        .floating-element:nth-child(4) {
-            top: 40%;
-            right: 30%;
-            animation-delay: 1s;
-        }
+            .floating-element:nth-child(1) {
+                top: 20%;
+                left: 10%;
+                animation-delay: 0s;
+            }
+
+            .floating-element:nth-child(2) {
+                top: 60%;
+                right: 15%;
+                animation-delay: 2s;
+            }
+
+            .floating-element:nth-child(3) {
+                bottom: 20%;
+                left: 20%;
+                animation-delay: 4s;
+            }
+
+            .floating-element:nth-child(4) {
+                top: 40%;
+                right: 30%;
+                animation-delay: 1s;
+            }
 
         @@keyframes glow {
             0% {
@@ -179,9 +179,7 @@
         }
 
         @@keyframes float {
-
-            0%,
-            100% {
+            0%, 100% {
                 transform: translateY(0px) rotate(0deg);
                 opacity: 0.1;
             }
@@ -205,9 +203,7 @@
         }
 
         @@keyframes pulse {
-
-            0%,
-            100% {
+            0%, 100% {
                 transform: scale(1);
                 opacity: 0.7;
             }
@@ -244,7 +240,6 @@
         }
     </style>
 </head>
-
 <body class="page-body error-page">
     <script type="text/javascript">var baseurl = '';</script>
 
@@ -261,9 +256,7 @@
                 <div class="col">
                     <div class="logo-container">
                         <a href="/" class="logo">
-                            <img id="logoOriginal"
-                                src="~/Images/LogotipoCompletoFrotiXRoxoFundoTransparenteEstreita.png"
-                                alt="Logo FrotiX" />
+                            <img id="logoOriginal" src="~/Images/LogotipoCompletoFrotiXRoxoFundoTransparenteEstreita.png" alt="Logo FrotiX" />
                         </a>
                     </div>
                 </div>
@@ -294,9 +287,9 @@
                         </a>
                         @if (Model.ShowLoginButton)
                         {
-                            <a href="/Account/Login" class="btn-error">
-                                <i class="fa-duotone fa-sign-in-alt"></i> Fazer Login
-                            </a>
+                        <a href="/Account/Login" class="btn-error">
+                            <i class="fa-duotone fa-sign-in-alt"></i> Fazer Login
+                        </a>
                         }
                     </div>
                 </div>
@@ -305,14 +298,6 @@
     </div>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * PÁGINA DE ERRO - TRATAMENTO DE IMAGEM
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Remove transparência ao redor de uma imagem PNG e retorna canvas recortado
-         * @@param {HTMLImageElement} img - Elemento de imagem a ser processado
-         * @@returns {HTMLCanvasElement} Canvas com a imagem recortada sem bordas transparentes
-         */
         function trimTransparentPNG(img) {
             const canvas = document.createElement('canvas');
             const ctx = canvas.getContext('2d');
@@ -381,5 +366,4 @@
     <script src="~/Neon/js/neon-api.js"></script>
     <script src="~/Neon/js/neon-custom.js"></script>
 </body>
-
 </html>
```
