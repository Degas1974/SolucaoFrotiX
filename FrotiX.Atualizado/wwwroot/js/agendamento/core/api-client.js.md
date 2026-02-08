# wwwroot/js/agendamento/core/api-client.js

**Mudanca:** GRANDE | **+77** linhas | **-69** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/core/api-client.js
+++ ATUAL: wwwroot/js/agendamento/core/api-client.js
@@ -1,144 +1,152 @@
-class ApiClient {
-    constructor(baseUrl = '') {
+class ApiClient
+{
+    constructor(baseUrl = '')
+    {
         this.baseUrl = baseUrl;
         this.defaultHeaders = {
-            'Content-Type': 'application/json; charset=utf-8',
+            'Content-Type': 'application/json; charset=utf-8'
         };
     }
 
-    async get(url, params = {}) {
-        try {
-            return new Promise((resolve, reject) => {
+    async get(url, params = {})
+    {
+        try
+        {
+            return new Promise((resolve, reject) =>
+            {
                 $.ajax({
                     url: this.baseUrl + url,
                     type: 'GET',
                     data: params,
                     dataType: 'json',
-                    success: function (data) {
+                    success: function (data)
+                    {
                         resolve(data);
                     },
-                    error: function (jqXHR, textStatus, errorThrown) {
-                        const erro = criarErroAjax(
-                            jqXHR,
-                            textStatus,
-                            errorThrown,
-                            this,
-                        );
+                    error: function (jqXHR, textStatus, errorThrown)
+                    {
+                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                         reject(erro);
-                    },
+                    }
                 });
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('api-client.js', 'get', error);
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("api-client.js", "get", error);
             throw error;
         }
     }
 
-    async post(url, data = {}) {
-        try {
-            return new Promise((resolve, reject) => {
+    async post(url, data = {})
+    {
+        try
+        {
+            return new Promise((resolve, reject) =>
+            {
                 $.ajax({
                     url: this.baseUrl + url,
                     type: 'POST',
                     contentType: this.defaultHeaders['Content-Type'],
                     dataType: 'json',
                     data: JSON.stringify(data),
-                    success: function (response) {
+                    success: function (response)
+                    {
                         resolve(response);
                     },
-                    error: function (jqXHR, textStatus, errorThrown) {
-                        const erro = criarErroAjax(
-                            jqXHR,
-                            textStatus,
-                            errorThrown,
-                            this,
-                        );
+                    error: function (jqXHR, textStatus, errorThrown)
+                    {
+                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                         reject(erro);
-                    },
+                    }
                 });
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('api-client.js', 'post', error);
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("api-client.js", "post", error);
             throw error;
         }
     }
 
-    async put(url, data = {}) {
-        try {
-            return new Promise((resolve, reject) => {
+    async put(url, data = {})
+    {
+        try
+        {
+            return new Promise((resolve, reject) =>
+            {
                 $.ajax({
                     url: this.baseUrl + url,
                     type: 'PUT',
                     contentType: this.defaultHeaders['Content-Type'],
                     dataType: 'json',
                     data: JSON.stringify(data),
-                    success: function (response) {
+                    success: function (response)
+                    {
                         resolve(response);
                     },
-                    error: function (jqXHR, textStatus, errorThrown) {
-                        const erro = criarErroAjax(
-                            jqXHR,
-                            textStatus,
-                            errorThrown,
-                            this,
-                        );
+                    error: function (jqXHR, textStatus, errorThrown)
+                    {
+                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                         reject(erro);
-                    },
+                    }
                 });
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('api-client.js', 'put', error);
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("api-client.js", "put", error);
             throw error;
         }
     }
 
-    async delete(url, params = {}) {
-        try {
-            return new Promise((resolve, reject) => {
+    async delete(url, params = {})
+    {
+        try
+        {
+            return new Promise((resolve, reject) =>
+            {
                 $.ajax({
                     url: this.baseUrl + url,
                     type: 'DELETE',
                     data: params,
                     dataType: 'json',
-                    success: function (response) {
+                    success: function (response)
+                    {
                         resolve(response);
                     },
-                    error: function (jqXHR, textStatus, errorThrown) {
-                        const erro = criarErroAjax(
-                            jqXHR,
-                            textStatus,
-                            errorThrown,
-                            this,
-                        );
+                    error: function (jqXHR, textStatus, errorThrown)
+                    {
+                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                         reject(erro);
-                    },
+                    }
                 });
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('api-client.js', 'delete', error);
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("api-client.js", "delete", error);
             throw error;
         }
     }
 
-    async fetch(url, options = {}) {
-        try {
+    async fetch(url, options = {})
+    {
+        try
+        {
             const response = await fetch(this.baseUrl + url, {
                 ...options,
                 headers: {
                     ...this.defaultHeaders,
-                    ...(options.headers || {}),
-                },
+                    ...(options.headers || {})
+                }
             });
 
-            if (!response.ok) {
-                throw new Error(
-                    `HTTP ${response.status}: ${response.statusText}`,
-                );
+            if (!response.ok)
+            {
+                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
             }
 
             return await response.json();
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('api-client.js', 'fetch', error);
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("api-client.js", "fetch", error);
             throw error;
         }
     }
```
