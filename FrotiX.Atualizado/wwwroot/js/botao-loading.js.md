# wwwroot/js/botao-loading.js

**Mudanca:** GRANDE | **+42** linhas | **-25** linhas

---

```diff
--- JANEIRO: wwwroot/js/botao-loading.js
+++ ATUAL: wwwroot/js/botao-loading.js
@@ -1,39 +1,56 @@
 (function () {
     function ativarBotaoLoading($btn) {
-        const $spinner = $btn.find('.spinner-border');
-        const $btnText = $btn.find('.btn-text');
-        const novoTexto = $btn.data('loading-text') || 'Aguarde...';
+        try {
+            const $spinner = $btn.find(".spinner-border");
+            const $btnText = $btn.find(".btn-text");
+            const novoTexto = $btn.data("loading-text") || "Aguarde...";
 
-        $btn.prop('disabled', true);
-        $spinner.removeClass('d-none');
-        $btnText.text(novoTexto);
-        document.body.style.cursor = 'wait';
+            $btn.prop("disabled", true);
+            $spinner.removeClass("d-none");
+            $btnText.text(novoTexto);
+            document.body.style.cursor = "wait";
+        } catch (erro) {
+            console.error('Erro em ativarBotaoLoading:', erro);
+            Alerta.TratamentoErroComLinha('botao-loading.js', 'ativarBotaoLoading', erro);
+        }
     }
 
     function restaurarBotaoLoading($btn, textoOriginal) {
-        const $spinner = $btn.find('.spinner-border');
-        const $btnText = $btn.find('.btn-text');
+        try {
+            const $spinner = $btn.find(".spinner-border");
+            const $btnText = $btn.find(".btn-text");
 
-        $btn.prop('disabled', false);
-        $spinner.addClass('d-none');
-        $btnText.text(textoOriginal);
-        document.body.style.cursor = 'default';
+            $btn.prop("disabled", false);
+            $spinner.addClass("d-none");
+            $btnText.text(textoOriginal);
+            document.body.style.cursor = "default";
+        } catch (erro) {
+            console.error('Erro em restaurarBotaoLoading:', erro);
+            Alerta.TratamentoErroComLinha('botao-loading.js', 'restaurarBotaoLoading', erro);
+        }
     }
 
-    $(document).on('click', '.btn-loading', function (e) {
-        const $btn = $(this);
-        const textoOriginal = $btn.find('.btn-text').text();
+    $(document).on("click", ".btn-loading", function (e) {
+        try {
+            const $btn = $(this);
+            const textoOriginal = $btn.find(".btn-text").text();
 
-        ativarBotaoLoading($btn);
+            ativarBotaoLoading($btn);
 
-        setTimeout(() => {
-            $btn.trigger('btn:loading:start', [
-                $btn,
-                textoOriginal,
-                function done() {
+            setTimeout(() => {
+                try {
+                    $btn.trigger("btn:loading:start", [$btn, textoOriginal, function done() {
+                        restaurarBotaoLoading($btn, textoOriginal);
+                    }]);
+                } catch (erro) {
+                    console.error('Erro em setTimeout callback:', erro);
+                    Alerta.TratamentoErroComLinha('botao-loading.js', 'click.setTimeout', erro);
                     restaurarBotaoLoading($btn, textoOriginal);
-                },
-            ]);
-        }, 100);
+                }
+            }, 100);
+        } catch (erro) {
+            console.error('Erro em click handler .btn-loading:', erro);
+            Alerta.TratamentoErroComLinha('botao-loading.js', 'click.btn-loading', erro);
+        }
     });
 })();
```

### REMOVER do Janeiro

```javascript
        const $spinner = $btn.find('.spinner-border');
        const $btnText = $btn.find('.btn-text');
        const novoTexto = $btn.data('loading-text') || 'Aguarde...';
        $btn.prop('disabled', true);
        $spinner.removeClass('d-none');
        $btnText.text(novoTexto);
        document.body.style.cursor = 'wait';
        const $spinner = $btn.find('.spinner-border');
        const $btnText = $btn.find('.btn-text');
        $btn.prop('disabled', false);
        $spinner.addClass('d-none');
        $btnText.text(textoOriginal);
        document.body.style.cursor = 'default';
    $(document).on('click', '.btn-loading', function (e) {
        const $btn = $(this);
        const textoOriginal = $btn.find('.btn-text').text();
        ativarBotaoLoading($btn);
        setTimeout(() => {
            $btn.trigger('btn:loading:start', [
                $btn,
                textoOriginal,
                function done() {
                },
            ]);
        }, 100);
```


### ADICIONAR ao Janeiro

```javascript
        try {
            const $spinner = $btn.find(".spinner-border");
            const $btnText = $btn.find(".btn-text");
            const novoTexto = $btn.data("loading-text") || "Aguarde...";
            $btn.prop("disabled", true);
            $spinner.removeClass("d-none");
            $btnText.text(novoTexto);
            document.body.style.cursor = "wait";
        } catch (erro) {
            console.error('Erro em ativarBotaoLoading:', erro);
            Alerta.TratamentoErroComLinha('botao-loading.js', 'ativarBotaoLoading', erro);
        }
        try {
            const $spinner = $btn.find(".spinner-border");
            const $btnText = $btn.find(".btn-text");
            $btn.prop("disabled", false);
            $spinner.addClass("d-none");
            $btnText.text(textoOriginal);
            document.body.style.cursor = "default";
        } catch (erro) {
            console.error('Erro em restaurarBotaoLoading:', erro);
            Alerta.TratamentoErroComLinha('botao-loading.js', 'restaurarBotaoLoading', erro);
        }
    $(document).on("click", ".btn-loading", function (e) {
        try {
            const $btn = $(this);
            const textoOriginal = $btn.find(".btn-text").text();
            ativarBotaoLoading($btn);
            setTimeout(() => {
                try {
                    $btn.trigger("btn:loading:start", [$btn, textoOriginal, function done() {
                        restaurarBotaoLoading($btn, textoOriginal);
                    }]);
                } catch (erro) {
                    console.error('Erro em setTimeout callback:', erro);
                    Alerta.TratamentoErroComLinha('botao-loading.js', 'click.setTimeout', erro);
                }
            }, 100);
        } catch (erro) {
            console.error('Erro em click handler .btn-loading:', erro);
            Alerta.TratamentoErroComLinha('botao-loading.js', 'click.btn-loading', erro);
        }
```
