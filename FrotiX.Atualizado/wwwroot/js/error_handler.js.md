# wwwroot/js/error_handler.js

**Mudanca:** GRANDE | **+77** linhas | **-54** linhas

---

```diff
--- JANEIRO: wwwroot/js/error_handler.js
+++ ATUAL: wwwroot/js/error_handler.js
@@ -59,79 +59,94 @@
 
         criarErroEnriquecido: function (error, errorInfo, contexto)
         {
-
-            let errorEnriquecido;
-
-            if (error instanceof Error)
-            {
-                errorEnriquecido = error;
-            }
-            else if (typeof error === 'object' && error !== null)
-            {
-                errorEnriquecido = new Error(errorInfo.mensagem);
-
-                Object.assign(errorEnriquecido, error);
-            }
-            else
-            {
-                errorEnriquecido = new Error(String(error));
-            }
-
-            errorEnriquecido.contexto = errorInfo.contexto;
-            errorEnriquecido.origem = errorInfo.origem;
-            errorEnriquecido.timestamp = errorInfo.timestamp;
-
-            if (contexto.url)
-            {
-                errorEnriquecido.detalhes = errorEnriquecido.detalhes || {};
-                errorEnriquecido.detalhes.url = contexto.url;
-                errorEnriquecido.detalhes.method = contexto.method;
-                errorEnriquecido.detalhes.status = contexto.status;
-                errorEnriquecido.detalhes.statusText = contexto.statusText;
-                errorEnriquecido.detalhes.responseText = contexto.responseText;
-                errorEnriquecido.detalhes.serverMessage = contexto.serverMessage;
-            }
-
-            if (!errorEnriquecido.stack && error.stack)
-            {
-                errorEnriquecido.stack = error.stack;
-            }
-
-            return errorEnriquecido;
+            try {
+
+                let errorEnriquecido;
+
+                if (error instanceof Error)
+                {
+                    errorEnriquecido = error;
+                }
+                else if (typeof error === 'object' && error !== null)
+                {
+                    errorEnriquecido = new Error(errorInfo.mensagem);
+
+                    Object.assign(errorEnriquecido, error);
+                }
+                else
+                {
+                    errorEnriquecido = new Error(String(error));
+                }
+
+                errorEnriquecido.contexto = errorInfo.contexto;
+                errorEnriquecido.origem = errorInfo.origem;
+                errorEnriquecido.timestamp = errorInfo.timestamp;
+
+                if (contexto.url)
+                {
+                    errorEnriquecido.detalhes = errorEnriquecido.detalhes || {};
+                    errorEnriquecido.detalhes.url = contexto.url;
+                    errorEnriquecido.detalhes.method = contexto.method;
+                    errorEnriquecido.detalhes.status = contexto.status;
+                    errorEnriquecido.detalhes.statusText = contexto.statusText;
+                    errorEnriquecido.detalhes.responseText = contexto.responseText;
+                    errorEnriquecido.detalhes.serverMessage = contexto.serverMessage;
+                }
+
+                if (!errorEnriquecido.stack && error.stack)
+                {
+                    errorEnriquecido.stack = error.stack;
+                }
+
+                return errorEnriquecido;
+            } catch (erro) {
+                console.error('Erro em criarErroEnriquecido:', erro);
+                return new Error(errorInfo?.mensagem || 'Erro desconhecido');
+            }
         },
 
         extrairArquivo: function (error, contexto)
         {
-
-            if (contexto.filename) return contexto.filename;
-
-            if (error.fileName) return error.fileName;
-            if (error.arquivo) return error.arquivo;
-            if (error.detalhes?.arquivo) return error.detalhes.arquivo;
-
-            if (error.stack)
-            {
-                const match = error.stack.match(/(?:https?:)?\/\/[^\/]+\/(?:.*\/)?([\w\-_.]+\.(?:js|ts|jsx|tsx))/);
-                if (match) return match[1];
-            }
-
-            return 'agendamento_viagem.js';
+            try {
+
+                if (contexto.filename) return contexto.filename;
+
+                if (error.fileName) return error.fileName;
+                if (error.arquivo) return error.arquivo;
+                if (error.detalhes?.arquivo) return error.detalhes.arquivo;
+
+                if (error.stack)
+                {
+                    const match = error.stack.match(/(?:https?:)?\/\/[^\/]+\/(?:.*\/)?([\w\-_.]+\.(?:js|ts|jsx|tsx))/);
+                    if (match) return match[1];
+                }
+
+                return 'agendamento_viagem.js';
+            } catch (erro) {
+                console.error('Erro em extrairArquivo:', erro);
+                return 'agendamento_viagem.js';
+            }
         },
 
         extrairFuncao: function (error, origem)
         {
-
-            if (error.stack)
-            {
-                const lines = error.stack.split('\n');
-                if (lines.length > 1)
-                {
-                    const match = lines[1].match(/at\s+(\w+)/);
-                    if (match) return match[1];
-                }
-            }
-
-            return origem;
+            try {
+
+                if (error.stack)
+                {
+                    const lines = error.stack.split('\n');
+                    if (lines.length > 1)
+                    {
+                        const match = lines[1].match(/at\s+(\w+)/);
+                        if (match) return match[1];
+                    }
+                }
+
+                return origem;
+            } catch (erro) {
+                console.error('Erro em extrairFuncao:', erro);
+                return origem;
+            }
         },
 
         registrarMetrica: function (origem, errorInfo)
@@ -161,12 +176,20 @@
 
         setContexto: function (contexto)
         {
-            this.contextoAtual = { ...this.contextoAtual, ...contexto };
+            try {
+                this.contextoAtual = { ...this.contextoAtual, ...contexto };
+            } catch (erro) {
+                console.error('Erro em setContexto:', erro);
+            }
         },
 
         limparContexto: function ()
         {
-            this.contextoAtual = {};
+            try {
+                this.contextoAtual = {};
+            } catch (erro) {
+                console.error('Erro em limparContexto:', erro);
+            }
         },
 
         obterLog: function ()
```
