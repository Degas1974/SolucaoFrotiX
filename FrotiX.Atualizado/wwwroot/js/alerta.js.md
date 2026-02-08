# wwwroot/js/alerta.js

**Mudanca:** GRANDE | **+142** linhas | **-95** linhas

---

```diff
--- JANEIRO: wwwroot/js/alerta.js
+++ ATUAL: wwwroot/js/alerta.js
@@ -227,35 +227,56 @@
         console.log(' - Objeto completo:', erroObj);
         console.log('=== TratamentoErroComLinha ENVIANDO ===');
 
+        _enviarLogParaServidor(classeOuArquivo, metodo, erroObj);
+
+        return SweetAlertInterop.ShowErrorUnexpected(classeOuArquivo, metodo, erroObj);
+    }
+
+    function _enviarLogParaServidor(arquivo, metodo, erroObj)
+    {
         try
         {
+
             const payload = {
-                Mensagem: erroObj.message || erroObj.erro || "Erro JS desconhecido",
-                Arquivo: classeOuArquivo,
-                Metodo: metodo,
-                Linha: parseInt(erroObj.linha || 0),
-                Coluna: parseInt(erroObj.coluna || 0),
-                Stack: erroObj.stack,
-                UserAgent: navigator.userAgent,
-                Url: window.location.href,
-                Timestamp: new Date().toISOString()
+                mensagem: erroObj.message || erroObj.erro || 'Erro JavaScript',
+                arquivo: arquivo || 'desconhecido.js',
+                metodo: metodo || 'desconhecido',
+                linha: erroObj.linha || erroObj.lineNumber || null,
+                coluna: erroObj.coluna || erroObj.columnNumber || null,
+                stack: erroObj.stack || null,
+                userAgent: navigator.userAgent,
+                url: window.location.href,
+                timestamp: new Date().toISOString()
             };
 
             fetch('/api/LogErros/LogJavaScript', {
                 method: 'POST',
                 headers: {
-                    'Content-Type': 'application/json'
+                    'Content-Type': 'application/json',
+                    'Accept': 'application/json'
                 },
-                body: JSON.stringify(payload),
-                keepalive: true
-            }).catch(e => console.warn("Falha ao enviar log para servidor:", e));
-        }
-        catch (eLog)
-        {
-            console.warn("Erro ao tentar logar erro no servidor:", eLog);
-        }
-
-        return SweetAlertInterop.ShowErrorUnexpected(classeOuArquivo, metodo, erroObj);
+                body: JSON.stringify(payload)
+            })
+            .then(response => {
+                if (response.ok)
+                {
+                    console.log('✅ [Alerta] Erro enviado para o servidor com sucesso');
+                }
+                else
+                {
+                    console.warn('⚠️ [Alerta] Falha ao enviar erro para servidor:', response.status);
+                }
+            })
+            .catch(err => {
+
+                console.warn('⚠️ [Alerta] Não foi possível enviar erro para servidor:', err.message);
+            });
+        }
+        catch (ex)
+        {
+
+            console.warn('⚠️ [Alerta] Exceção ao preparar envio de log:', ex.message);
+        }
     }
 
     window.Alerta.TratamentoErroComLinha = window.Alerta.TratamentoErroComLinha || _TratamentoErroComLinha;
@@ -360,99 +381,129 @@
 
 (function integrarErrorHandler()
 {
-    let tentativas = 0;
-    const maxTentativas = 50;
-
-    function tentarIntegrar()
-    {
-        tentativas++;
-
-        if (typeof ErrorHandler !== 'undefined')
-        {
-            console.log('✅ [Alerta] Integrado com ErrorHandler');
-
-            window.Alerta.criarErroAjax = window.criarErroAjax;
-
-            window.Alerta.TratamentoErroComLinhaEnriquecido = function (arquivo, funcao, erro, contextoAdicional = {})
-            {
-
-                if (contextoAdicional && Object.keys(contextoAdicional).length > 0)
-                {
-
-                    if (typeof erro === 'object' && erro !== null)
+    try {
+        let tentativas = 0;
+        const maxTentativas = 50;
+
+        function tentarIntegrar()
+        {
+            try {
+                tentativas++;
+
+                if (typeof ErrorHandler !== 'undefined')
+                {
+                    console.log('✅ [Alerta] Integrado com ErrorHandler');
+
+                    window.Alerta.criarErroAjax = window.criarErroAjax;
+
+                    window.Alerta.TratamentoErroComLinhaEnriquecido = function (arquivo, funcao, erro, contextoAdicional = {})
                     {
-                        erro.contextoManual = contextoAdicional;
-                    }
-                    else
+                        try {
+
+                            if (contextoAdicional && Object.keys(contextoAdicional).length > 0)
+                            {
+
+                                if (typeof erro === 'object' && erro !== null)
+                                {
+                                    erro.contextoManual = contextoAdicional;
+                                }
+                                else
+                                {
+
+                                    const mensagem = String(erro);
+                                    erro = {
+                                        message: mensagem,
+                                        erro: mensagem,
+                                        contextoManual: contextoAdicional,
+                                        stack: new Error(mensagem).stack
+                                    };
+                                }
+                            }
+
+                            return window.Alerta.TratamentoErroComLinha(arquivo, funcao, erro);
+                        } catch (erro) {
+                            console.error('Erro em TratamentoErroComLinhaEnriquecido:', erro);
+                            return Promise.resolve();
+                        }
+                    };
+
+                    window.Alerta.setContextoGlobal = function (contexto)
                     {
-
-                        const mensagem = String(erro);
-                        erro = {
-                            message: mensagem,
-                            erro: mensagem,
-                            contextoManual: contextoAdicional,
-                            stack: new Error(mensagem).stack
-                        };
-                    }
-                }
-
-                return window.Alerta.TratamentoErroComLinha(arquivo, funcao, erro);
-            };
-
-            window.Alerta.setContextoGlobal = function (contexto)
-            {
-                if (ErrorHandler && ErrorHandler.setContexto)
-                {
-                    ErrorHandler.setContexto(contexto);
-                }
-            };
-
-            window.Alerta.limparContextoGlobal = function ()
-            {
-                if (ErrorHandler && ErrorHandler.limparContexto)
-                {
-                    ErrorHandler.limparContexto();
-                }
-            };
-
-            window.Alerta.obterLogErros = function ()
-            {
-                if (ErrorHandler && ErrorHandler.obterLog)
-                {
-                    return ErrorHandler.obterLog();
-                }
-                return [];
-            };
-
-            window.Alerta.limparLogErros = function ()
-            {
-                if (ErrorHandler && ErrorHandler.limparLog)
-                {
-                    ErrorHandler.limparLog();
-                }
-            };
-
-            console.log('📋 [Alerta] Funções adicionais disponíveis:');
-            console.log(' - Alerta.criarErroAjax(jqXHR, textStatus, errorThrown, ajaxSettings)');
-            console.log(' - Alerta.TratamentoErroComLinhaEnriquecido(arquivo, funcao, erro, contexto)');
-            console.log(' - Alerta.setContextoGlobal(contexto)');
-            console.log(' - Alerta.limparContextoGlobal()');
-            console.log(' - Alerta.obterLogErros()');
-            console.log(' - Alerta.limparLogErros()');
-        }
-        else if (tentativas < maxTentativas)
-        {
-
-            setTimeout(tentarIntegrar, 100);
-        }
-        else
-        {
-            console.warn('⚠️ [Alerta] ErrorHandler não foi carregado após 5 segundos');
-            console.warn(' Certifique-se de que error_handler.js está sendo carregado');
-        }
-    }
-
-    tentarIntegrar();
+                        try {
+                            if (ErrorHandler && ErrorHandler.setContexto)
+                            {
+                                ErrorHandler.setContexto(contexto);
+                            }
+                        } catch (erro) {
+                            console.error('Erro em setContextoGlobal:', erro);
+                        }
+                    };
+
+                    window.Alerta.limparContextoGlobal = function ()
+                    {
+                        try {
+                            if (ErrorHandler && ErrorHandler.limparContexto)
+                            {
+                                ErrorHandler.limparContexto();
+                            }
+                        } catch (erro) {
+                            console.error('Erro em limparContextoGlobal:', erro);
+                        }
+                    };
+
+                    window.Alerta.obterLogErros = function ()
+                    {
+                        try {
+                            if (ErrorHandler && ErrorHandler.obterLog)
+                            {
+                                return ErrorHandler.obterLog();
+                            }
+                            return [];
+                        } catch (erro) {
+                            console.error('Erro em obterLogErros:', erro);
+                            return [];
+                        }
+                    };
+
+                    window.Alerta.limparLogErros = function ()
+                    {
+                        try {
+                            if (ErrorHandler && ErrorHandler.limparLog)
+                            {
+                                ErrorHandler.limparLog();
+                            }
+                        } catch (erro) {
+                            console.error('Erro em limparLogErros:', erro);
+                        }
+                    };
+
+                    console.log('📋 [Alerta] Funções adicionais disponíveis:');
+                    console.log(' - Alerta.criarErroAjax(jqXHR, textStatus, errorThrown, ajaxSettings)');
+                    console.log(' - Alerta.TratamentoErroComLinhaEnriquecido(arquivo, funcao, erro, contexto)');
+                    console.log(' - Alerta.setContextoGlobal(contexto)');
+                    console.log(' - Alerta.limparContextoGlobal()');
+                    console.log(' - Alerta.obterLogErros()');
+                    console.log(' - Alerta.limparLogErros()');
+                }
+                else if (tentativas < maxTentativas)
+                {
+
+                    setTimeout(tentarIntegrar, 100);
+                }
+                else
+                {
+                    console.warn('⚠️ [Alerta] ErrorHandler não foi carregado após 5 segundos');
+                    console.warn(' Certifique-se de que error_handler.js está sendo carregado');
+                }
+            } catch (erro) {
+                console.error('Erro em tentarIntegrar:', erro);
+            }
+        }
+
+        tentarIntegrar();
+    } catch (erro) {
+        console.error('Erro em integrarErrorHandler:', erro);
+    }
 })();
 
 console.log('%c[Alerta] Sistema completo carregado',
```
