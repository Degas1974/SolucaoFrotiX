# wwwroot/js/signalr_manager.js

**Mudanca:** GRANDE | **+579** linhas | **-629** linhas

---

```diff
--- JANEIRO: wwwroot/js/signalr_manager.js
+++ ATUAL: wwwroot/js/signalr_manager.js
@@ -1,4 +1,5 @@
-var SignalRManager = (function () {
+var SignalRManager = (function ()
+{
     'use strict';
 
     var connection = null;
@@ -9,99 +10,105 @@
     var connectionPromise = null;
 
     var config = {
-        hubUrl: '/alertasHub',
+        hubUrl: "/alertasHub",
         reconnectDelays: [0, 2000, 5000, 10000, 30000],
         logLevel: signalR.LogLevel.Warning,
         maxReconnectAttempts: 5,
         reconnectAttempt: 0,
         initialDelay: 1000,
 
-        fallbackToLongPolling: true,
+        fallbackToLongPolling: true
     };
 
-    function delay(ms) {
-        return new Promise(function (resolve) {
-            setTimeout(resolve, ms);
-        });
-    }
-
-    function getTransportName(conn) {
-        try {
-
-            if (conn && conn.connection) {
-
-                if (
-                    conn.connection.transport &&
-                    conn.connection.transport.name
-                ) {
+    function delay(ms)
+    {
+        try
+        {
+            return new Promise(function (resolve)
+            {
+                setTimeout(resolve, ms);
+            });
+        }
+        catch (erro)
+        {
+            console.error('Erro em delay:', erro);
+            return Promise.resolve();
+        }
+    }
+
+    function getTransportName(conn)
+    {
+        try
+        {
+
+            if (conn && conn.connection)
+            {
+
+                if (conn.connection.transport && conn.connection.transport.name)
+                {
                     return conn.connection.transport.name;
                 }
 
-                if (
-                    conn.connection._transport &&
-                    conn.connection._transport.name
-                ) {
+                if (conn.connection._transport && conn.connection._transport.name)
+                {
                     return conn.connection._transport.name;
                 }
 
-                if (
-                    conn.connection.connectionStarted &&
-                    conn.connection.transport
-                ) {
-                    return (
-                        conn.connection.transport.name ||
-                        conn.connection.transport.constructor.name
-                    );
+                if (conn.connection.connectionStarted && conn.connection.transport)
+                {
+                    return conn.connection.transport.name || conn.connection.transport.constructor.name;
                 }
 
                 var keys = Object.keys(conn.connection);
-                for (var i = 0; i < keys.length; i++) {
+                for (var i = 0; i < keys.length; i++)
+                {
                     var key = keys[i];
-                    if (
-                        key.includes('transport') &&
-                        conn.connection[key] &&
-                        conn.connection[key].name
-                    ) {
+                    if (key.includes('transport') && conn.connection[key] && conn.connection[key].name)
+                    {
                         return conn.connection[key].name;
                     }
                 }
             }
 
-            if (config.fallbackToLongPolling) {
-                return 'LongPolling (fallback)';
-            }
-
-            return 'Não detectado';
-        } catch (error) {
-            return 'Erro na detecção';
-        }
-    }
-
-    function createConnection(useFallback) {
-        try {
-            if (connection && !useFallback) {
-                console.log(
-                    '⚠️ Conexão SignalR já existe, retornando existente',
-                );
+            if (config.fallbackToLongPolling)
+            {
+                return "LongPolling (fallback)";
+            }
+
+            return "Não detectado";
+        }
+        catch (error)
+        {
+            return "Erro na detecção";
+        }
+    }
+
+    function createConnection(useFallback)
+    {
+        try
+        {
+            if (connection && !useFallback)
+            {
+                console.log("⚠️ Conexão SignalR já existe, retornando existente");
                 return connection;
             }
 
-            console.log('🔧 Criando nova conexão SignalR...');
-
-            if (useFallback || config.fallbackToLongPolling) {
-                console.log('🔄 Usando LongPolling como transporte...');
-            }
-
-            try {
+            console.log("🔧 Criando nova conexão SignalR...");
+
+            if (useFallback || config.fallbackToLongPolling)
+            {
+                console.log("🔄 Usando LongPolling como transporte...");
+            }
+
+            try
+            {
                 var builder = new signalR.HubConnectionBuilder()
                     .withUrl(config.hubUrl, {
-                        transport:
-                            useFallback || config.fallbackToLongPolling
-                                ? signalR.HttpTransportType.LongPolling
-                                : signalR.HttpTransportType.WebSockets |
-                                  signalR.HttpTransportType.LongPolling,
+                        transport: (useFallback || config.fallbackToLongPolling)
+                            ? signalR.HttpTransportType.LongPolling
+                            : signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
                         skipNegotiation: false,
-                        withCredentials: true,
+                        withCredentials: true
                     })
                     .withAutomaticReconnect(config.reconnectDelays)
                     .configureLogging(config.logLevel);
@@ -110,104 +117,99 @@
                 setupConnectionHandlers();
                 isInitialized = true;
 
-                console.log('✅ Conexão SignalR criada com sucesso');
+                console.log("✅ Conexão SignalR criada com sucesso");
                 return connection;
-            } catch (innerError) {
-                TratamentoErroComLinha(
-                    'signalr_manager.js',
-                    'createConnection.build',
-                    innerError,
-                );
+
+            } catch (innerError)
+            {
+                TratamentoErroComLinha("signalr_manager.js", "createConnection.build", innerError);
                 connection = null;
                 isInitialized = false;
                 throw innerError;
             }
-        } catch (error) {
-            TratamentoErroComLinha(
-                'signalr_manager.js',
-                'createConnection',
-                error,
-            );
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "createConnection", error);
             connection = null;
             isInitialized = false;
             throw error;
         }
     }
 
-    function setupConnectionHandlers() {
-        try {
+    function setupConnectionHandlers()
+    {
+        try
+        {
             if (!connection) return;
 
-            connection.onreconnecting(function (error) {
-                try {
-                    console.log('🔄 SignalR reconectando...');
-                    if (error) {
-                        console.log('Motivo:', error.toString());
+            connection.onreconnecting(function (error)
+            {
+                try
+                {
+                    console.log("🔄 SignalR reconectando...");
+                    if (error)
+                    {
+                        console.log("Motivo:", error.toString());
                     }
                     config.reconnectAttempt++;
 
-                    if (typeof AppToast !== 'undefined') {
-                        AppToast.show(
-                            'Amarelo',
-                            'Reconectando ao servidor...',
-                            2000,
-                        );
+                    if (typeof AppToast !== 'undefined')
+                    {
+                        AppToast.show("Amarelo", "Reconectando ao servidor...", 2000);
                     }
 
                     notifyAllCallbacks('onReconnecting', error);
-                } catch (callbackError) {
-                    TratamentoErroComLinha(
-                        'signalr_manager.js',
-                        'onreconnecting.callback',
-                        callbackError,
-                    );
+                }
+                catch (callbackError)
+                {
+                    TratamentoErroComLinha("signalr_manager.js", "onreconnecting.callback", callbackError);
                 }
             });
 
-            connection.onreconnected(function (connectionId) {
-                try {
-                    console.log('✅ SignalR reconectado com sucesso!');
-                    console.log('Connection ID:', connectionId);
+            connection.onreconnected(function (connectionId)
+            {
+                try
+                {
+                    console.log("✅ SignalR reconectado com sucesso!");
+                    console.log("Connection ID:", connectionId);
                     config.reconnectAttempt = 0;
 
-                    if (typeof AppToast !== 'undefined') {
-                        AppToast.show('Verde', 'Conexão restabelecida', 2000);
+                    if (typeof AppToast !== 'undefined')
+                    {
+                        AppToast.show("Verde", "Conexão restabelecida", 2000);
                     }
 
                     reregisterEventHandlers();
 
                     notifyAllCallbacks('onReconnected', connectionId);
-                } catch (callbackError) {
-                    TratamentoErroComLinha(
-                        'signalr_manager.js',
-                        'onreconnected.callback',
-                        callbackError,
-                    );
+                }
+                catch (callbackError)
+                {
+                    TratamentoErroComLinha("signalr_manager.js", "onreconnected.callback", callbackError);
                 }
             });
 
-            connection.onclose(function (error) {
-                try {
-                    console.log('❌ Conexão SignalR fechada');
-                    if (error) {
+            connection.onclose(function (error)
+            {
+                try
+                {
+                    console.log("❌ Conexão SignalR fechada");
+                    if (error)
+                    {
                         var errorMessage = error.toString().toLowerCase();
-                        var isWebSocketIssue =
-                            errorMessage.includes('websocket');
-
-                        if (isWebSocketIssue && !config.fallbackToLongPolling) {
-                            console.log(
-                                '🔄 Erro de WebSocket detectado, tentando fallback para LongPolling...',
-                            );
+                        var isWebSocketIssue = errorMessage.includes('websocket');
+
+                        if (isWebSocketIssue && !config.fallbackToLongPolling)
+                        {
+                            console.log("🔄 Erro de WebSocket detectado, tentando fallback para LongPolling...");
                             config.fallbackToLongPolling = true;
                         }
 
                         if (isWebSocketIssue) {
-                            console.warn(
-                                'WebSocket indisponível, usando fallback:',
-                                error.toString(),
-                            );
+                            console.warn("WebSocket indisponível, usando fallback:", error.toString());
                         } else {
-                            console.error('Erro:', error.toString());
+                            console.error("Erro:", error.toString());
                         }
                     }
 
@@ -218,191 +220,171 @@
 
                     notifyAllCallbacks('onClose', error);
 
-                    if (config.reconnectAttempt < config.maxReconnectAttempts) {
-                        var retryDelay =
-                            config.reconnectDelays[
-                                Math.min(
-                                    config.reconnectAttempt,
-                                    config.reconnectDelays.length - 1,
-                                )
-                            ];
-                        console.log(
-                            '🔄 Tentando reconectar em ' + retryDelay + 'ms...',
-                        );
-
-                        setTimeout(function () {
-                            try {
-                                console.log(
-                                    '🔄 Tentando reconectar automaticamente...',
-                                );
-                                getConnection().catch(function (err) {
-                                    try {
-                                        console.error(
-                                            '❌ Falha na reconexão automática:',
-                                            err,
-                                        );
-                                    } catch (logError) {
-                                        TratamentoErroComLinha(
-                                            'signalr_manager.js',
-                                            'onclose.reconnect.catch',
-                                            logError,
-                                        );
+                    if (config.reconnectAttempt < config.maxReconnectAttempts)
+                    {
+                        var retryDelay = config.reconnectDelays[Math.min(config.reconnectAttempt, config.reconnectDelays.length - 1)];
+                        console.log("🔄 Tentando reconectar em " + retryDelay + "ms...");
+
+                        setTimeout(function ()
+                        {
+                            try
+                            {
+                                console.log("🔄 Tentando reconectar automaticamente...");
+                                getConnection().catch(function (err)
+                                {
+                                    try
+                                    {
+                                        console.error("❌ Falha na reconexão automática:", err);
+                                    }
+                                    catch (logError)
+                                    {
+                                        TratamentoErroComLinha("signalr_manager.js", "onclose.reconnect.catch", logError);
                                     }
                                 });
-                            } catch (reconnectError) {
-                                TratamentoErroComLinha(
-                                    'signalr_manager.js',
-                                    'onclose.setTimeout',
-                                    reconnectError,
-                                );
+                            }
+                            catch (reconnectError)
+                            {
+                                TratamentoErroComLinha("signalr_manager.js", "onclose.setTimeout", reconnectError);
                             }
                         }, retryDelay);
-                    } else {
-                        console.error(
-                            '❌ Máximo de tentativas de reconexão excedido',
-                        );
+                    } else
+                    {
+                        console.error("❌ Máximo de tentativas de reconexão excedido");
                         config.reconnectAttempt = 0;
 
-                        if (typeof ShowErrorUnexpected !== 'undefined') {
-                            ShowErrorUnexpected(
-                                'Sistema de notificações indisponível. Por favor, recarregue a página.',
-                            );
+                        if (typeof ShowErrorUnexpected !== 'undefined')
+                        {
+                            ShowErrorUnexpected("Sistema de notificações indisponível. Por favor, recarregue a página.");
                         }
                     }
-                } catch (callbackError) {
-                    TratamentoErroComLinha(
-                        'signalr_manager.js',
-                        'onclose.callback',
-                        callbackError,
-                    );
+                }
+                catch (callbackError)
+                {
+                    TratamentoErroComLinha("signalr_manager.js", "onclose.callback", callbackError);
                 }
             });
-        } catch (error) {
-            TratamentoErroComLinha(
-                'signalr_manager.js',
-                'setupConnectionHandlers',
-                error,
-            );
-        }
-    }
-
-    function reregisterEventHandlers() {
-        try {
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "setupConnectionHandlers", error);
+        }
+    }
+
+    function reregisterEventHandlers()
+    {
+        try
+        {
             if (!connection) return;
 
-            console.log('📡 Reregistrando event handlers...');
-
-            Object.keys(eventHandlers).forEach(function (eventName) {
-                try {
+            console.log("📡 Reregistrando event handlers...");
+
+            Object.keys(eventHandlers).forEach(function (eventName)
+            {
+                try
+                {
                     var handlers = eventHandlers[eventName];
-                    handlers.forEach(function (handler) {
-                        try {
+                    handlers.forEach(function (handler)
+                    {
+                        try
+                        {
                             connection.on(eventName, handler);
-                        } catch (handlerError) {
-                            TratamentoErroComLinha(
-                                'signalr_manager.js',
-                                'reregisterEventHandlers.forEach.handler',
-                                handlerError,
-                            );
+                        }
+                        catch (handlerError)
+                        {
+                            TratamentoErroComLinha("signalr_manager.js", "reregisterEventHandlers.forEach.handler", handlerError);
                         }
                     });
-                } catch (eventError) {
-                    TratamentoErroComLinha(
-                        'signalr_manager.js',
-                        'reregisterEventHandlers.forEach',
-                        eventError,
-                    );
+                }
+                catch (eventError)
+                {
+                    TratamentoErroComLinha("signalr_manager.js", "reregisterEventHandlers.forEach", eventError);
                 }
             });
 
-            console.log(
-                '✅ Event handlers reregistrados:',
-                Object.keys(eventHandlers).length,
-            );
-        } catch (error) {
-            TratamentoErroComLinha(
-                'signalr_manager.js',
-                'reregisterEventHandlers',
-                error,
-            );
-        }
-    }
-
-    function notifyAllCallbacks(callbackName, data) {
-        try {
-            reconnectCallbacks.forEach(function (callback) {
-                try {
-                    if (typeof callback[callbackName] === 'function') {
-                        try {
+            console.log("✅ Event handlers reregistrados:", Object.keys(eventHandlers).length);
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "reregisterEventHandlers", error);
+        }
+    }
+
+    function notifyAllCallbacks(callbackName, data)
+    {
+        try
+        {
+            reconnectCallbacks.forEach(function (callback)
+            {
+                try
+                {
+                    if (typeof callback[callbackName] === 'function')
+                    {
+                        try
+                        {
                             callback[callbackName](data);
-                        } catch (execError) {
-                            TratamentoErroComLinha(
-                                'signalr_manager.js',
-                                'notifyAllCallbacks.execute',
-                                execError,
-                            );
+                        } catch (execError)
+                        {
+                            TratamentoErroComLinha("signalr_manager.js", "notifyAllCallbacks.execute", execError);
                         }
                     }
-                } catch (callbackError) {
-                    TratamentoErroComLinha(
-                        'signalr_manager.js',
-                        'notifyAllCallbacks.forEach',
-                        callbackError,
-                    );
+                }
+                catch (callbackError)
+                {
+                    TratamentoErroComLinha("signalr_manager.js", "notifyAllCallbacks.forEach", callbackError);
                 }
             });
-        } catch (error) {
-            TratamentoErroComLinha(
-                'signalr_manager.js',
-                'notifyAllCallbacks',
-                error,
-            );
-        }
-    }
-
-    function getConnection() {
-        try {
-
-            if (
-                connection &&
-                connection.state === signalR.HubConnectionState.Connected
-            ) {
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "notifyAllCallbacks", error);
+        }
+    }
+
+    function getConnection()
+    {
+        try
+        {
+
+            if (connection && connection.state === signalR.HubConnectionState.Connected)
+            {
                 return Promise.resolve(connection);
             }
 
-            if (connectionPromise) {
-                console.log('⏳ Retornando promise de conexão existente...');
+            if (connectionPromise)
+            {
+                console.log("⏳ Retornando promise de conexão existente...");
                 return connectionPromise;
             }
 
-            connectionPromise = new Promise(function (resolve, reject) {
-                try {
-
-                    if (isConnecting) {
-                        console.log('⏳ Aguardando conexão em andamento...');
-                        var checkInterval = setInterval(function () {
-                            try {
-                                if (
-                                    connection &&
-                                    connection.state ===
-                                        signalR.HubConnectionState.Connected
-                                ) {
+            connectionPromise = new Promise(function (resolve, reject)
+            {
+                try
+                {
+
+                    if (isConnecting)
+                    {
+                        console.log("⏳ Aguardando conexão em andamento...");
+                        var checkInterval = setInterval(function ()
+                        {
+                            try
+                            {
+                                if (connection && connection.state === signalR.HubConnectionState.Connected)
+                                {
                                     clearInterval(checkInterval);
                                     connectionPromise = null;
                                     resolve(connection);
-                                } else if (!isConnecting) {
+                                } else if (!isConnecting)
+                                {
                                     clearInterval(checkInterval);
                                     connectionPromise = null;
-                                    reject(new Error('Conexão falhou'));
+                                    reject(new Error("Conexão falhou"));
                                 }
-                            } catch (checkError) {
+                            }
+                            catch (checkError)
+                            {
                                 clearInterval(checkInterval);
                                 connectionPromise = null;
-                                TratamentoErroComLinha(
-                                    'signalr_manager.js',
-                                    'getConnection.checkInterval',
-                                    checkError,
-                                );
+                                TratamentoErroComLinha("signalr_manager.js", "getConnection.checkInterval", checkError);
                                 reject(checkError);
                             }
                         }, 100);
@@ -411,212 +393,157 @@
 
                     isConnecting = true;
 
-                    console.log(
-                        '⏰ Aguardando ' +
-                            config.initialDelay +
-                            'ms antes de conectar...',
-                    );
-
-                    delay(config.initialDelay)
-                        .then(function () {
-                            try {
-                                var conn = createConnection(
-                                    config.fallbackToLongPolling,
-                                );
-
-                                console.log('🚀 Iniciando conexão SignalR...');
-
-                                var timeoutId = setTimeout(function () {
-                                    try {
-                                        console.error(
-                                            '⏱️ Timeout na conexão SignalR',
-                                        );
+                    console.log("⏰ Aguardando " + config.initialDelay + "ms antes de conectar...");
+
+                    delay(config.initialDelay).then(function ()
+                    {
+                        try
+                        {
+                            var conn = createConnection(config.fallbackToLongPolling);
+
+                            console.log("🚀 Iniciando conexão SignalR...");
+
+                            var timeoutId = setTimeout(function ()
+                            {
+                                try
+                                {
+                                    console.error("⏱️ Timeout na conexão SignalR");
+                                    isConnecting = false;
+                                    connectionPromise = null;
+
+                                    if (!config.fallbackToLongPolling)
+                                    {
+                                        console.log("🔄 Tentando fallback para LongPolling após timeout...");
+                                        config.fallbackToLongPolling = true;
+                                        connection = null;
+                                        isInitialized = false;
+
+                                        setTimeout(function ()
+                                        {
+                                            getConnection().then(resolve).catch(reject);
+                                        }, 1000);
+                                    }
+                                    else
+                                    {
+                                        reject(new Error("Timeout na conexão"));
+                                    }
+                                }
+                                catch (timeoutError)
+                                {
+                                    TratamentoErroComLinha("signalr_manager.js", "getConnection.timeout", timeoutError);
+                                    reject(timeoutError);
+                                }
+                            }, 30000);
+
+                            conn.start()
+                                .then(function ()
+                                {
+                                    try
+                                    {
+                                        clearTimeout(timeoutId);
+                                        console.log("✅✅✅ SIGNALR CONECTADO COM SUCESSO ✅✅✅");
+                                        console.log("Estado:", conn.state);
+                                        console.log("Connection ID:", conn.connectionId);
+
+                                        var transportName = getTransportName(conn);
+                                        console.log("Transporte:", transportName);
+
                                         isConnecting = false;
                                         connectionPromise = null;
-
-                                        if (!config.fallbackToLongPolling) {
-                                            console.log(
-                                                '🔄 Tentando fallback para LongPolling após timeout...',
-                                            );
+                                        config.reconnectAttempt = 0;
+                                        resolve(conn);
+                                    }
+                                    catch (thenError)
+                                    {
+                                        TratamentoErroComLinha("signalr_manager.js", "getConnection.start.then", thenError);
+                                        isConnecting = false;
+                                        connectionPromise = null;
+                                        reject(thenError);
+                                    }
+                                })
+                                .catch(function (err)
+                                {
+                                    try
+                                    {
+                                        clearTimeout(timeoutId);
+
+                                        var errorMessage = err.toString().toLowerCase();
+
+                                        if (!errorMessage.includes('websocket'))
+                                        {
+                                            console.error("❌❌❌ ERRO AO CONECTAR SIGNALR ❌❌❌");
+                                            console.error("Erro:", err.toString());
+                                        } else
+                                        {
+
+                                            console.log("⚠️ WebSocket indisponível, tentando fallback para LongPolling...");
+                                        }
+
+                                        if (errorMessage.includes('websocket') && !config.fallbackToLongPolling)
+                                        {
+                                            console.log("🔄 Erro de WebSocket, tentando LongPolling...");
                                             config.fallbackToLongPolling = true;
                                             connection = null;
                                             isInitialized = false;
-
-                                            setTimeout(function () {
-                                                getConnection()
-                                                    .then(resolve)
-                                                    .catch(reject);
-                                            }, 1000);
-                                        } else {
-                                            reject(
-                                                new Error('Timeout na conexão'),
-                                            );
-                                        }
-                                    } catch (timeoutError) {
-                                        TratamentoErroComLinha(
-                                            'signalr_manager.js',
-                                            'getConnection.timeout',
-                                            timeoutError,
-                                        );
-                                        reject(timeoutError);
-                                    }
-                                }, 30000);
-
-                                conn.start()
-                                    .then(function () {
-                                        try {
-                                            clearTimeout(timeoutId);
-                                            console.log(
-                                                '✅✅✅ SIGNALR CONECTADO COM SUCESSO ✅✅✅',
-                                            );
-                                            console.log('Estado:', conn.state);
-                                            console.log(
-                                                'Connection ID:',
-                                                conn.connectionId,
-                                            );
-
-                                            var transportName =
-                                                getTransportName(conn);
-                                            console.log(
-                                                'Transporte:',
-                                                transportName,
-                                            );
-
                                             isConnecting = false;
                                             connectionPromise = null;
-                                            config.reconnectAttempt = 0;
-                                            resolve(conn);
-                                        } catch (thenError) {
-                                            TratamentoErroComLinha(
-                                                'signalr_manager.js',
-                                                'getConnection.start.then',
-                                                thenError,
-                                            );
-                                            isConnecting = false;
-                                            connectionPromise = null;
-                                            reject(thenError);
+
+                                            setTimeout(function ()
+                                            {
+                                                getConnection().then(resolve).catch(reject);
+                                            }, 1000);
+                                            return;
                                         }
-                                    })
-                                    .catch(function (err) {
-                                        try {
-                                            clearTimeout(timeoutId);
-
-                                            var errorMessage = err
-                                                .toString()
-                                                .toLowerCase();
-
-                                            if (
-                                                !errorMessage.includes(
-                                                    'websocket',
-                                                )
-                                            ) {
-                                                console.error(
-                                                    '❌❌❌ ERRO AO CONECTAR SIGNALR ❌❌❌',
-                                                );
-                                                console.error(
-                                                    'Erro:',
-                                                    err.toString(),
-                                                );
-                                            } else {
-
-                                                console.log(
-                                                    '⚠️ WebSocket indisponível, tentando fallback para LongPolling...',
-                                                );
+
+                                        if (errorMessage.includes('401') || errorMessage.includes('unauthorized'))
+                                        {
+                                            console.error("🔴 Erro de autenticação no SignalR");
+                                            if (typeof ShowErrorUnexpected !== 'undefined')
+                                            {
+                                                ShowErrorUnexpected("Erro de autenticação. Por favor, faça login novamente.");
                                             }
-
-                                            if (
-                                                errorMessage.includes(
-                                                    'websocket',
-                                                ) &&
-                                                !config.fallbackToLongPolling
-                                            ) {
-                                                console.log(
-                                                    '🔄 Erro de WebSocket, tentando LongPolling...',
-                                                );
-                                                config.fallbackToLongPolling = true;
-                                                connection = null;
-                                                isInitialized = false;
-                                                isConnecting = false;
-                                                connectionPromise = null;
-
-                                                setTimeout(function () {
-                                                    getConnection()
-                                                        .then(resolve)
-                                                        .catch(reject);
-                                                }, 1000);
-                                                return;
-                                            }
-
-                                            if (
-                                                errorMessage.includes('401') ||
-                                                errorMessage.includes(
-                                                    'unauthorized',
-                                                )
-                                            ) {
-                                                console.error(
-                                                    '🔴 Erro de autenticação no SignalR',
-                                                );
-                                                if (
-                                                    typeof ShowErrorUnexpected !==
-                                                    'undefined'
-                                                ) {
-                                                    ShowErrorUnexpected(
-                                                        'Erro de autenticação. Por favor, faça login novamente.',
-                                                    );
-                                                }
-                                            } else {
-                                                TratamentoErroComLinha(
-                                                    'signalr_manager.js',
-                                                    'getConnection.start.catch',
-                                                    err,
-                                                );
-                                            }
-
-                                            isConnecting = false;
-                                            connectionPromise = null;
-                                            connection = null;
-                                            isInitialized = false;
-                                            reject(err);
-                                        } catch (catchError) {
-                                            TratamentoErroComLinha(
-                                                'signalr_manager.js',
-                                                'getConnection.start.catch.inner',
-                                                catchError,
-                                            );
-                                            isConnecting = false;
-                                            connectionPromise = null;
-                                            reject(catchError);
                                         }
-                                    });
-                            } catch (startError) {
-                                TratamentoErroComLinha(
-                                    'signalr_manager.js',
-                                    'getConnection.delay.then',
-                                    startError,
-                                );
-                                isConnecting = false;
-                                connectionPromise = null;
-                                connection = null;
-                                isInitialized = false;
-                                reject(startError);
-                            }
-                        })
-                        .catch(function (delayError) {
-                            TratamentoErroComLinha(
-                                'signalr_manager.js',
-                                'getConnection.delay.catch',
-                                delayError,
-                            );
+                                        else
+                                        {
+                                            TratamentoErroComLinha("signalr_manager.js", "getConnection.start.catch", err);
+                                        }
+
+                                        isConnecting = false;
+                                        connectionPromise = null;
+                                        connection = null;
+                                        isInitialized = false;
+                                        reject(err);
+                                    }
+                                    catch (catchError)
+                                    {
+                                        TratamentoErroComLinha("signalr_manager.js", "getConnection.start.catch.inner", catchError);
+                                        isConnecting = false;
+                                        connectionPromise = null;
+                                        reject(catchError);
+                                    }
+                                });
+                        }
+                        catch (startError)
+                        {
+                            TratamentoErroComLinha("signalr_manager.js", "getConnection.delay.then", startError);
+                            isConnecting = false;
+                            connectionPromise = null;
+                            connection = null;
+                            isInitialized = false;
+                            reject(startError);
+                        }
+                    })
+                        .catch(function (delayError)
+                        {
+                            TratamentoErroComLinha("signalr_manager.js", "getConnection.delay.catch", delayError);
                             isConnecting = false;
                             connectionPromise = null;
                             reject(delayError);
                         });
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        'signalr_manager.js',
-                        'getConnection.promise',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha("signalr_manager.js", "getConnection.promise", error);
                     isConnecting = false;
                     connectionPromise = null;
                     connection = null;
@@ -626,210 +553,225 @@
             });
 
             return connectionPromise;
-        } catch (error) {
-            TratamentoErroComLinha(
-                'signalr_manager.js',
-                'getConnection',
-                error,
-            );
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "getConnection", error);
             connectionPromise = null;
             return Promise.reject(error);
         }
     }
 
-    function on(eventName, handler) {
-        try {
-            if (typeof handler !== 'function') {
-                console.error('Handler deve ser uma função');
+    function on(eventName, handler)
+    {
+        try
+        {
+            if (typeof handler !== 'function')
+            {
+                console.error("Handler deve ser uma função");
                 return;
             }
 
-            if (!eventHandlers[eventName]) {
+            if (!eventHandlers[eventName])
+            {
                 eventHandlers[eventName] = [];
             }
             eventHandlers[eventName].push(handler);
 
-            if (connection) {
-                try {
+            if (connection)
+            {
+                try
+                {
                     connection.on(eventName, handler);
-                    console.log('📡 Event handler registrado:', eventName);
-                } catch (registerError) {
-                    TratamentoErroComLinha(
-                        'signalr_manager.js',
-                        'on.connection.on',
-                        registerError,
-                    );
-                }
-            }
-        } catch (error) {
-            TratamentoErroComLinha('signalr_manager.js', 'on', error);
-        }
-    }
-
-    function off(eventName, handler) {
-        try {
-            if (eventHandlers[eventName]) {
-                eventHandlers[eventName] = eventHandlers[eventName].filter(
-                    function (h) {
-                        return h !== handler;
-                    },
-                );
-            }
-
-            if (connection) {
-                try {
+                    console.log("📡 Event handler registrado:", eventName);
+                }
+                catch (registerError)
+                {
+                    TratamentoErroComLinha("signalr_manager.js", "on.connection.on", registerError);
+                }
+            }
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "on", error);
+        }
+    }
+
+    function off(eventName, handler)
+    {
+        try
+        {
+            if (eventHandlers[eventName])
+            {
+                eventHandlers[eventName] = eventHandlers[eventName].filter(function (h)
+                {
+                    return h !== handler;
+                });
+            }
+
+            if (connection)
+            {
+                try
+                {
                     connection.off(eventName, handler);
-                    console.log('📡 Event handler removido:', eventName);
-                } catch (offError) {
-                    TratamentoErroComLinha(
-                        'signalr_manager.js',
-                        'off.connection.off',
-                        offError,
-                    );
-                }
-            }
-        } catch (error) {
-            TratamentoErroComLinha('signalr_manager.js', 'off', error);
-        }
-    }
-
-    function invoke(methodName) {
-        try {
+                    console.log("📡 Event handler removido:", eventName);
+                }
+                catch (offError)
+                {
+                    TratamentoErroComLinha("signalr_manager.js", "off.connection.off", offError);
+                }
+            }
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "off", error);
+        }
+    }
+
+    function invoke(methodName)
+    {
+        try
+        {
             var args = Array.prototype.slice.call(arguments, 1);
 
-            return getConnection()
-                .then(function (conn) {
-                    try {
-                        console.log('📤 Invocando método:', methodName);
-                        return conn.invoke.apply(
-                            conn,
-                            [methodName].concat(args),
-                        );
-                    } catch (invokeError) {
-                        TratamentoErroComLinha(
-                            'signalr_manager.js',
-                            'invoke.then',
-                            invokeError,
-                        );
-                        throw invokeError;
-                    }
-                })
-                .catch(function (err) {
-                    TratamentoErroComLinha(
-                        'signalr_manager.js',
-                        'invoke.catch',
-                        err,
-                    );
-                    throw err;
+            return getConnection().then(function (conn)
+            {
+                try
+                {
+                    console.log("📤 Invocando método:", methodName);
+                    return conn.invoke.apply(conn, [methodName].concat(args));
+                }
+                catch (invokeError)
+                {
+                    TratamentoErroComLinha("signalr_manager.js", "invoke.then", invokeError);
+                    throw invokeError;
+                }
+            }).catch(function (err)
+            {
+                TratamentoErroComLinha("signalr_manager.js", "invoke.catch", err);
+                throw err;
+            });
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "invoke", error);
+            return Promise.reject(error);
+        }
+    }
+
+    function registerCallback(callback)
+    {
+        try
+        {
+            if (typeof callback !== 'object')
+            {
+                console.error("Callback deve ser um objeto com métodos");
+                return;
+            }
+
+            reconnectCallbacks.push(callback);
+            console.log("✅ Callback registrado");
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "registerCallback", error);
+        }
+    }
+
+    function unregisterCallback(callback)
+    {
+        try
+        {
+            var index = reconnectCallbacks.indexOf(callback);
+            if (index > -1)
+            {
+                reconnectCallbacks.splice(index, 1);
+                console.log("✅ Callback removido");
+            }
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "unregisterCallback", error);
+        }
+    }
+
+    function getState()
+    {
+        try
+        {
+            if (!connection)
+            {
+                return "Disconnected";
+            }
+
+            switch (connection.state)
+            {
+                case signalR.HubConnectionState.Connected:
+                    return "Connected";
+                case signalR.HubConnectionState.Connecting:
+                    return "Connecting";
+                case signalR.HubConnectionState.Reconnecting:
+                    return "Reconnecting";
+                case signalR.HubConnectionState.Disconnected:
+                    return "Disconnected";
+                default:
+                    return "Unknown";
+            }
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "getState", error);
+            return "Error";
+        }
+    }
+
+    function disconnect()
+    {
+        try
+        {
+            if (connection)
+            {
+                return connection.stop().then(function ()
+                {
+                    try
+                    {
+                        console.log("✅ Desconectado manualmente");
+                        connection = null;
+                        isConnecting = false;
+                        isInitialized = false;
+                        connectionPromise = null;
+                        config.fallbackToLongPolling = false;
+                        config.reconnectAttempt = 0;
+                    }
+                    catch (stopError)
+                    {
+                        TratamentoErroComLinha("signalr_manager.js", "disconnect.stop.then", stopError);
+                    }
+                }).catch(function (err)
+                {
+                    TratamentoErroComLinha("signalr_manager.js", "disconnect.stop.catch", err);
                 });
-        } catch (error) {
-            TratamentoErroComLinha('signalr_manager.js', 'invoke', error);
+            }
+            return Promise.resolve();
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "disconnect", error);
             return Promise.reject(error);
         }
     }
 
-    function registerCallback(callback) {
-        try {
-            if (typeof callback !== 'object') {
-                console.error('Callback deve ser um objeto com métodos');
-                return;
-            }
-
-            reconnectCallbacks.push(callback);
-            console.log('✅ Callback registrado');
-        } catch (error) {
-            TratamentoErroComLinha(
-                'signalr_manager.js',
-                'registerCallback',
-                error,
-            );
-        }
-    }
-
-    function unregisterCallback(callback) {
-        try {
-            var index = reconnectCallbacks.indexOf(callback);
-            if (index > -1) {
-                reconnectCallbacks.splice(index, 1);
-                console.log('✅ Callback removido');
-            }
-        } catch (error) {
-            TratamentoErroComLinha(
-                'signalr_manager.js',
-                'unregisterCallback',
-                error,
-            );
-        }
-    }
-
-    function getState() {
-        try {
-            if (!connection) {
-                return 'Disconnected';
-            }
-
-            switch (connection.state) {
-                case signalR.HubConnectionState.Connected:
-                    return 'Connected';
-                case signalR.HubConnectionState.Connecting:
-                    return 'Connecting';
-                case signalR.HubConnectionState.Reconnecting:
-                    return 'Reconnecting';
-                case signalR.HubConnectionState.Disconnected:
-                    return 'Disconnected';
-                default:
-                    return 'Unknown';
-            }
-        } catch (error) {
-            TratamentoErroComLinha('signalr_manager.js', 'getState', error);
-            return 'Error';
-        }
-    }
-
-    function disconnect() {
-        try {
-            if (connection) {
-                return connection
-                    .stop()
-                    .then(function () {
-                        try {
-                            console.log('✅ Desconectado manualmente');
-                            connection = null;
-                            isConnecting = false;
-                            isInitialized = false;
-                            connectionPromise = null;
-                            config.fallbackToLongPolling = false;
-                            config.reconnectAttempt = 0;
-                        } catch (stopError) {
-                            TratamentoErroComLinha(
-                                'signalr_manager.js',
-                                'disconnect.stop.then',
-                                stopError,
-                            );
-                        }
-                    })
-                    .catch(function (err) {
-                        TratamentoErroComLinha(
-                            'signalr_manager.js',
-                            'disconnect.stop.catch',
-                            err,
-                        );
-                    });
-            }
-            return Promise.resolve();
-        } catch (error) {
-            TratamentoErroComLinha('signalr_manager.js', 'disconnect', error);
-            return Promise.reject(error);
-        }
-    }
-
-    function forceReconnect() {
-        try {
-            console.log('🔄 Forçando reconexão...');
+    function forceReconnect()
+    {
+        try
+        {
+            console.log("🔄 Forçando reconexão...");
             config.reconnectAttempt = 0;
 
-            if (connection) {
-                return connection.stop().then(function () {
+            if (connection)
+            {
+                return connection.stop().then(function ()
+                {
                     connection = null;
                     isConnecting = false;
                     isInitialized = false;
@@ -837,24 +779,27 @@
                     config.fallbackToLongPolling = false;
                     return getConnection();
                 });
-            } else {
+            }
+            else
+            {
                 return getConnection();
             }
-        } catch (error) {
-            TratamentoErroComLinha(
-                'signalr_manager.js',
-                'forceReconnect',
-                error,
-            );
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "forceReconnect", error);
             return Promise.reject(error);
         }
     }
 
-    function getDebugInfo() {
-        try {
-            var transportName = 'N/A';
-
-            if (connection) {
+    function getDebugInfo()
+    {
+        try
+        {
+            var transportName = "N/A";
+
+            if (connection)
+            {
                 transportName = getTransportName(connection);
             }
 
@@ -867,13 +812,15 @@
                 registeredEvents: Object.keys(eventHandlers),
                 registeredCallbacks: reconnectCallbacks.length,
                 usingFallback: config.fallbackToLongPolling,
-                transport: transportName,
+                transport: transportName
             };
-        } catch (error) {
-            TratamentoErroComLinha('signalr_manager.js', 'getDebugInfo', error);
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("signalr_manager.js", "getDebugInfo", error);
             return {
-                error: 'Erro ao obter informações de debug',
-                message: error.message,
+                error: "Erro ao obter informações de debug",
+                message: error.message
             };
         }
     }
@@ -888,16 +835,20 @@
         getState: getState,
         disconnect: disconnect,
         forceReconnect: forceReconnect,
-        getDebugInfo: getDebugInfo,
+        getDebugInfo: getDebugInfo
     };
 })();
 
-try {
+try
+{
     window.SignalRManager = SignalRManager;
-    console.log('✅ SignalRManager carregado e pronto para uso');
-} catch (error) {
-
-    if (typeof console !== 'undefined' && console.error) {
-        console.error('Erro ao expor SignalRManager globalmente:', error);
-    }
+    console.log("✅ SignalRManager carregado e pronto para uso");
 }
+catch (error)
+{
+
+    if (typeof console !== 'undefined' && console.error)
+    {
+        console.error("Erro ao expor SignalRManager globalmente:", error);
+    }
+}
```
