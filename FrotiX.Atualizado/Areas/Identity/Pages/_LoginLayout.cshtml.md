# Areas/Identity/Pages/_LoginLayout.cshtml

**Mudanca:** GRANDE | **+36** linhas | **-1** linhas

---

```diff
--- JANEIRO: Areas/Identity/Pages/_LoginLayout.cshtml
+++ ATUAL: Areas/Identity/Pages/_LoginLayout.cshtml
@@ -22,7 +22,7 @@
     <link rel="stylesheet" href="~/Neon/css/custom.css">
     <link rel="stylesheet" href="~/css/botaologin.css">
 
-    <link href="~/lib/fontawesome-pro/css/all.css" rel="stylesheet" />
+    <script src="https://kit.fontawesome.com/afeb78ad1f.js" crossorigin="anonymous"></script>
 
     <script src="~/Neon/js/jquery-1.11.3.min.js"></script>
 
@@ -66,6 +66,24 @@
     </div>
 
     <script>
+        /****************************************************************************************
+         * ⚡ FUNÇÃO: trimTransparentPNG
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Remove pixels transparentes das bordas de uma imagem PNG, retornando
+         * um canvas contendo apenas a área visível da imagem (crop automático
+         * de transparência).
+         *
+         * 📥 ENTRADAS : [HTMLImageElement] img - Elemento de imagem carregado.
+         *
+         * 📤 SAÍDAS : [HTMLCanvasElement] - Canvas contendo a imagem recortada sem bordas
+         * transparentes.
+         *
+         * 🔗 CHAMADA POR : Função anônima no evento window.load (linha ~133).
+         *
+         * 🔄 CHAMA : Canvas API nativa (getImageData, putImageData, drawImage).
+         *
+         * 📦 DEPENDÊNCIAS : Canvas API (HTML5).
+         ****************************************************************************************/
         function trimTransparentPNG(img) {
           const canvas = document.createElement('canvas');
           const ctx = canvas.getContext('2d');
@@ -106,6 +124,23 @@
     </script>
 
     <script>
+        /****************************************************************************************
+         * ⚡ FUNÇÃO: window.load Event Listener (Processamento de Logo)
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Processar o logo FrotiX removendo transparências e redimensionando
+         * para 800px de largura (mantendo proporção), garantindo visual
+         * limpo e consistente na tela de login.
+         *
+         * 📥 ENTRADAS : Nenhuma (captura elemento #logoOriginal do DOM).
+         *
+         * 📤 SAÍDAS : Substitui elemento <img> original por <canvas> processado.
+         *
+         * 🔗 CHAMADA POR : Evento 'load' do navegador (window.addEventListener).
+         *
+         * 🔄 CHAMA : trimTransparentPNG(), Canvas API (drawImage, replaceWith).
+         *
+         * 📦 DEPENDÊNCIAS : Canvas API (HTML5), trimTransparentPNG().
+         ****************************************************************************************/
         window.addEventListener('load', () => {
             const img = document.getElementById('logoOriginal');
             const proxyImg = new Image();
```

### REMOVER do Janeiro

```html
    <link href="~/lib/fontawesome-pro/css/all.css" rel="stylesheet" />
```


### ADICIONAR ao Janeiro

```html
    <script src="https://kit.fontawesome.com/afeb78ad1f.js" crossorigin="anonymous"></script>
        /****************************************************************************************
         * ⚡ FUNÇÃO: trimTransparentPNG
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO : Remove pixels transparentes das bordas de uma imagem PNG, retornando
         * um canvas contendo apenas a área visível da imagem (crop automático
         * de transparência).
         *
         * 📥 ENTRADAS : [HTMLImageElement] img - Elemento de imagem carregado.
         *
         * 📤 SAÍDAS : [HTMLCanvasElement] - Canvas contendo a imagem recortada sem bordas
         * transparentes.
         *
         * 🔗 CHAMADA POR : Função anônima no evento window.load (linha ~133).
         *
         * 🔄 CHAMA : Canvas API nativa (getImageData, putImageData, drawImage).
         *
         * 📦 DEPENDÊNCIAS : Canvas API (HTML5).
         ****************************************************************************************/
        /****************************************************************************************
         * ⚡ FUNÇÃO: window.load Event Listener (Processamento de Logo)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO : Processar o logo FrotiX removendo transparências e redimensionando
         * para 800px de largura (mantendo proporção), garantindo visual
         * limpo e consistente na tela de login.
         *
         * 📥 ENTRADAS : Nenhuma (captura elemento #logoOriginal do DOM).
         *
         * 📤 SAÍDAS : Substitui elemento <img> original por <canvas> processado.
         *
         * 🔗 CHAMADA POR : Evento 'load' do navegador (window.addEventListener).
         *
         * 🔄 CHAMA : trimTransparentPNG(), Canvas API (drawImage, replaceWith).
         *
         * 📦 DEPENDÊNCIAS : Canvas API (HTML5), trimTransparentPNG().
         ****************************************************************************************/
```
