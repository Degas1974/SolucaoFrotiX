# wwwroot/js/agendamento/core/ajax-helper.js

**Mudanca:** GRANDE | **+30** linhas | **-25** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/core/ajax-helper.js
+++ ATUAL: wwwroot/js/agendamento/core/ajax-helper.js
@@ -1,5 +1,7 @@
-window.criarErroAjax = function (jqXHR, textStatus, errorThrown, ajaxSettings) {
-    try {
+window.criarErroAjax = function (jqXHR, textStatus, errorThrown, ajaxSettings)
+{
+    try
+    {
         const erro = {
 
             status: jqXHR.status || 0,
@@ -14,53 +16,55 @@
 
             timestamp: new Date().toISOString(),
 
-            mensagemUsuario: '',
+            mensagemUsuario: ''
         };
 
-        try {
-            if (jqXHR.responseText) {
+        try
+        {
+            if (jqXHR.responseText)
+            {
                 erro.responseJSON = JSON.parse(jqXHR.responseText);
             }
-        } catch (e) {
+        } catch (e)
+        {
 
         }
 
-        switch (erro.status) {
+        switch (erro.status)
+        {
             case 0:
-                erro.mensagemUsuario =
-                    'Sem conexão com o servidor. Verifique sua internet.';
+                erro.mensagemUsuario = 'Sem conexão com o servidor. Verifique sua internet.';
                 break;
             case 400:
                 erro.mensagemUsuario = 'Dados inválidos enviados ao servidor.';
                 break;
             case 401:
-                erro.mensagemUsuario =
-                    'Sessão expirada. Por favor, faça login novamente.';
+                erro.mensagemUsuario = 'Sessão expirada. Por favor, faça login novamente.';
                 break;
             case 403:
-                erro.mensagemUsuario =
-                    'Você não tem permissão para esta operação.';
+                erro.mensagemUsuario = 'Você não tem permissão para esta operação.';
                 break;
             case 404:
                 erro.mensagemUsuario = 'Recurso não encontrado no servidor.';
                 break;
             case 500:
-                erro.mensagemUsuario =
-                    'Erro interno do servidor. Tente novamente mais tarde.';
+                erro.mensagemUsuario = 'Erro interno do servidor. Tente novamente mais tarde.';
                 break;
             case 503:
                 erro.mensagemUsuario = 'Servidor temporariamente indisponível.';
                 break;
             default:
-                if (textStatus === 'timeout') {
-                    erro.mensagemUsuario =
-                        'A operação demorou muito. Tente novamente.';
-                } else if (textStatus === 'abort') {
+                if (textStatus === 'timeout')
+                {
+                    erro.mensagemUsuario = 'A operação demorou muito. Tente novamente.';
+                } else if (textStatus === 'abort')
+                {
                     erro.mensagemUsuario = 'Operação cancelada.';
-                } else if (textStatus === 'parsererror') {
-                    erro.mensagemUsuario =
-                        'Erro ao processar resposta do servidor.';
-                } else {
+                } else if (textStatus === 'parsererror')
+                {
+                    erro.mensagemUsuario = 'Erro ao processar resposta do servidor.';
+                } else
+                {
                     erro.mensagemUsuario = `Erro ao comunicar com o servidor (${erro.status}).`;
                 }
         }
@@ -69,14 +73,16 @@
             `Status: ${erro.status} - ${erro.statusText}`,
             `URL: ${erro.url}`,
             `Método: ${erro.method}`,
-            `Mensagem: ${erro.mensagemUsuario}`,
+            `Mensagem: ${erro.mensagemUsuario}`
         ].join('\n');
 
         const errorObj = new Error(mensagemCompleta);
         errorObj.ajax = erro;
 
         return errorObj;
-    } catch (e) {
+
+    } catch (e)
+    {
 
         return new Error(`Erro AJAX: ${textStatus || 'Desconhecido'}`);
     }
```

### REMOVER do Janeiro

```javascript
window.criarErroAjax = function (jqXHR, textStatus, errorThrown, ajaxSettings) {
    try {
            mensagemUsuario: '',
        try {
            if (jqXHR.responseText) {
        } catch (e) {
        switch (erro.status) {
                erro.mensagemUsuario =
                    'Sem conexão com o servidor. Verifique sua internet.';
                erro.mensagemUsuario =
                    'Sessão expirada. Por favor, faça login novamente.';
                erro.mensagemUsuario =
                    'Você não tem permissão para esta operação.';
                erro.mensagemUsuario =
                    'Erro interno do servidor. Tente novamente mais tarde.';
                if (textStatus === 'timeout') {
                    erro.mensagemUsuario =
                        'A operação demorou muito. Tente novamente.';
                } else if (textStatus === 'abort') {
                } else if (textStatus === 'parsererror') {
                    erro.mensagemUsuario =
                        'Erro ao processar resposta do servidor.';
                } else {
            `Mensagem: ${erro.mensagemUsuario}`,
    } catch (e) {
```


### ADICIONAR ao Janeiro

```javascript
window.criarErroAjax = function (jqXHR, textStatus, errorThrown, ajaxSettings)
{
    try
    {
            mensagemUsuario: ''
        try
        {
            if (jqXHR.responseText)
            {
        } catch (e)
        {
        switch (erro.status)
        {
                erro.mensagemUsuario = 'Sem conexão com o servidor. Verifique sua internet.';
                erro.mensagemUsuario = 'Sessão expirada. Por favor, faça login novamente.';
                erro.mensagemUsuario = 'Você não tem permissão para esta operação.';
                erro.mensagemUsuario = 'Erro interno do servidor. Tente novamente mais tarde.';
                if (textStatus === 'timeout')
                {
                    erro.mensagemUsuario = 'A operação demorou muito. Tente novamente.';
                } else if (textStatus === 'abort')
                {
                } else if (textStatus === 'parsererror')
                {
                    erro.mensagemUsuario = 'Erro ao processar resposta do servidor.';
                } else
                {
            `Mensagem: ${erro.mensagemUsuario}`
    } catch (e)
    {
```
