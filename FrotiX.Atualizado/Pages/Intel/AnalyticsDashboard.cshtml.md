# Pages/Intel/AnalyticsDashboard.cshtml

**Mudanca:** GRANDE | **+32** linhas | **-56** linhas

---

```diff
--- JANEIRO: Pages/Intel/AnalyticsDashboard.cshtml
+++ ATUAL: Pages/Intel/AnalyticsDashboard.cshtml
@@ -35,20 +35,20 @@
         color: #b29b2b;
     }
 
-    .botaoFormat:hover {
-        font-weight: bold;
-        box-shadow: 0 0 0.8em #808080;
-    }
+        .botaoFormat:hover {
+            font-weight: bold;
+            box-shadow: 0 0 0.8em #808080;
+        }
 
     .row.display-flex {
         display: flex;
         flex-wrap: wrap;
     }
 
-    .row.display-flex>[class*='col-'] {
-        display: flex;
-        flex-direction: column;
-    }
+        .row.display-flex > [class*='col-'] {
+            display: flex;
+            flex-direction: column;
+        }
 
     #notify {
         position: fixed;
@@ -92,9 +92,12 @@
             <div class="col-lg-12 text-center">
 
                 <div>
-                    <img id="logoOriginal" src="~/Images/LogotipoCompletoFrotiXRoxoFundoTransparenteEstreita.png"
-                        alt="FrotiX" data-trim data-trim-maxwidth="500"
-                        style="display:block;margin:0 auto;max-width:100%;height:auto;" />
+                    <img id="logoOriginal"
+                         src="~/Images/LogotipoCompletoFrotiXRoxoFundoTransparenteEstreita.png"
+                         alt="FrotiX"
+                         data-trim
+                         data-trim-maxwidth="500"
+                         style="display:block;margin:0 auto;max-width:100%;height:auto;" />
                 </div>
 
                 <div class="mt-4">
@@ -108,8 +111,7 @@
                                             <div class="profile-content">
                                                 <div class="profile-img">
                                                     <a href="/agenda/index">
-                                                        <img src="~/Images/Schedule.jpg" class="img img-responsive"
-                                                            alt="Agenda">
+                                                        <img src="~/Images/Schedule.jpg" class="img img-responsive" alt="Agenda">
                                                     </a>
                                                 </div>
                                                 <div class="profile-name">
@@ -130,8 +132,7 @@
                                             <div class="profile-content">
                                                 <div class="profile-img">
                                                     <a href="/viagens/upsert">
-                                                        <img src="~/Images/Trip.jpg" class="img img-responsive"
-                                                            alt="Nova Viagem">
+                                                        <img src="~/Images/Trip.jpg" class="img img-responsive" alt="Nova Viagem">
                                                     </a>
                                                 </div>
                                                 <div class="profile-name">
@@ -152,8 +153,7 @@
                                             <div class="profile-content">
                                                 <div class="profile-img">
                                                     <a href="/viagens/index">
-                                                        <img src="~/Images/GestaoViagens.jpg" class="img img-responsive"
-                                                            alt="Gestão de Viagens">
+                                                        <img src="~/Images/GestaoViagens.jpg" class="img img-responsive" alt="Gestão de Viagens">
                                                     </a>
                                                 </div>
                                                 <div class="profile-name">
@@ -174,8 +174,7 @@
                                             <div class="profile-content">
                                                 <div class="profile-img">
                                                     <a href="/veiculo/index">
-                                                        <img src="~/Images/Veiculos.jpg" class="img img-responsive"
-                                                            alt="Gestão de Veículos">
+                                                        <img src="~/Images/Veiculos.jpg" class="img img-responsive" alt="Gestão de Veículos">
                                                     </a>
                                                 </div>
                                                 <div class="profile-name">
@@ -196,8 +195,7 @@
                                             <div class="profile-content">
                                                 <div class="profile-img">
                                                     <a href="/abastecimento/index">
-                                                        <img src="~/Images/Abastecimento.jpg" class="img img-responsive"
-                                                            alt="Gestão de Abastecimento">
+                                                        <img src="~/Images/Abastecimento.jpg" class="img img-responsive" alt="Gestão de Abastecimento">
                                                     </a>
                                                 </div>
                                                 <div class="profile-name">
@@ -218,8 +216,7 @@
                                             <div class="profile-content">
                                                 <div class="profile-img">
                                                     <a href="/manutencao/listamanutencao">
-                                                        <img src="~/Images/Manutencao.jpg" class="img img-responsive"
-                                                            alt="Gestão de Manutenções">
+                                                        <img src="~/Images/Manutencao.jpg" class="img img-responsive" alt="Gestão de Manutenções">
                                                     </a>
                                                 </div>
                                                 <div class="profile-name">
@@ -240,8 +237,7 @@
                                             <div class="profile-content">
                                                 <div class="profile-img">
                                                     <a href="/multa/listapenalidade">
-                                                        <img src="~/Images/Multa.jpg" class="img img-responsive"
-                                                            alt="Gestão de Multas">
+                                                        <img src="~/Images/Multa.jpg" class="img img-responsive" alt="Gestão de Multas">
                                                     </a>
                                                 </div>
                                                 <div class="profile-name">
@@ -262,8 +258,7 @@
                                             <div class="profile-content">
                                                 <div class="profile-img">
                                                     <a href="/viagens/fluxopassageiros">
-                                                        <img src="~/Images/MOB.jpg" class="img img-responsive"
-                                                            alt="Inserir Fichas MOB">
+                                                        <img src="~/Images/MOB.jpg" class="img img-responsive" alt="Inserir Fichas MOB">
                                                     </a>
                                                 </div>
                                                 <div class="profile-name">
@@ -291,10 +286,8 @@
 }
 
 @section ScriptsBlock {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script>
 
@@ -359,14 +352,7 @@
     </script>
 
     <script>
-        /**
-         * Recorta bordas transparentes de imagem PNG
-         * @@description Analisa pixels via canvas e retorna canvas recortado sem áreas transparentes
-         * @@param {HTMLImageElement} img - Elemento de imagem a processar
-         * @@param {Object} options - Configurações opcionais
-         * @@param {number} [options.alphaThreshold=1] - Valor mínimo de alpha para considerar pixel visível (0-255)
-         * @@returns {HTMLCanvasElement|null} Canvas recortado ou null se CORS ou imagem transparente
-         */
+        /* ========= NÚCLEO: recorta bordas transparentes de PNG ========= */
         function trimTransparentPNG(img, { alphaThreshold = 1 } = {}) {
             try {
                 const src = document.createElement('canvas');
@@ -417,17 +403,7 @@
             }
         }
 
-        /**
-         * Aplica trim de transparência em elemento img e substitui por canvas
-         * @@description Carrega imagem, aplica trimTransparentPNG, redimensiona e substitui elemento
-         * @@param {HTMLImageElement} imgEl - Elemento img a processar
-         * @@param {Object} options - Configurações opcionais
-         * @@param {number} [options.targetWidth] - Largura final desejada
-         * @@param {number} [options.maxWidth=500] - Largura máxima responsiva
-         * @@param {number} [options.alphaThreshold=1] - Threshold de alpha (0-255)
-         * @@param {boolean} [options.replace=true] - Se true substitui img, senão insere após
-         * @@returns {void}
-         */
+        /* ========= HELPER: aplica o trim num <img> ========= */
         function applyTrimToImg(imgEl, {
             targetWidth,
             maxWidth = 500,
@@ -520,10 +496,10 @@
         /* =========================================================================
            FEEDBACK VISUAL: CURSOR AMPULHETA AO CLICAR NOS CARDS
            ========================================================================= */
-        (function () {
+        (function() {
             'use strict';
 
-            document.addEventListener('click', function (e) {
+            document.addEventListener('click', function(e) {
                 try {
 
                     var link = e.target.closest('.profile-card-1 a');
@@ -537,7 +513,7 @@
                         var card = link.closest('.profile-card-1');
                         if (card) {
                             card.style.cursor = 'wait';
-                            card.querySelectorAll('*').forEach(function (el) {
+                            card.querySelectorAll('*').forEach(function(el) {
                                 el.style.cursor = 'wait';
                             });
                         }
@@ -548,15 +524,15 @@
                 }
             });
 
-            window.addEventListener('pageshow', function (e) {
+            window.addEventListener('pageshow', function(e) {
                 try {
                     if (e.persisted) {
                         document.body.style.cursor = '';
                         document.documentElement.style.cursor = '';
                         document.body.classList.remove('ftx-loading');
-                        document.querySelectorAll('.profile-card-1').forEach(function (card) {
+                        document.querySelectorAll('.profile-card-1').forEach(function(card) {
                             card.style.cursor = '';
-                            card.querySelectorAll('*').forEach(function (el) {
+                            card.querySelectorAll('*').forEach(function(el) {
                                 el.style.cursor = '';
                             });
                         });
```
