# wwwroot/js/agendamento/services/viagem.service.js

**Mudanca:** GRANDE | **+93** linhas | **-120** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/services/viagem.service.js
+++ ATUAL: wwwroot/js/agendamento/services/viagem.service.js
@@ -1,201 +1,174 @@
-class ViagemService {
-    constructor() {
+class ViagemService
+{
+    constructor()
+    {
         this.api = window.ApiClient;
     }
 
-    async verificarStatus(viagemId) {
-        try {
-            const isAberta = await this.api.get(
-                '/api/Viagem/PegarStatusViagem',
-                { viagemId },
-            );
+    async verificarStatus(viagemId)
+    {
+        try
+        {
+            const isAberta = await this.api.get('/api/Viagem/PegarStatusViagem', { viagemId });
 
             return {
                 success: true,
-                data: isAberta,
+                data: isAberta
             };
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'viagem.service.js',
-                'verificarStatus',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("viagem.service.js", "verificarStatus", error);
             return {
                 success: false,
                 error: error.message,
-                data: false,
+                data: false
             };
         }
     }
 
-    async recuperarUsuario(id) {
-        try {
-            const data = await this.api.get('/api/Agenda/RecuperaUsuario', {
-                id,
-            });
+    async recuperarUsuario(id)
+    {
+        try
+        {
+            const data = await this.api.get('/api/Agenda/RecuperaUsuario', { id });
 
-            let nomeUsuario = '';
-            $.each(data, function (key, val) {
+            let nomeUsuario = "";
+            $.each(data, function (key, val)
+            {
                 nomeUsuario = val;
             });
 
             return {
                 success: true,
-                data: nomeUsuario,
+                data: nomeUsuario
             };
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'viagem.service.js',
-                'recuperarUsuario',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("viagem.service.js", "recuperarUsuario", error);
             return {
                 success: false,
                 error: error.message,
-                data: '',
+                data: ""
             };
         }
     }
 
-    async verificarFicha(noFicha) {
-        try {
-            return new Promise((resolve, reject) => {
+    async verificarFicha(noFicha)
+    {
+        try
+        {
+            return new Promise((resolve, reject) =>
+            {
                 $.ajax({
-                    url: '/Viagens/Upsert?handler=VerificaFicha',
-                    method: 'GET',
-                    datatype: 'json',
+                    url: "/Viagens/Upsert?handler=VerificaFicha",
+                    method: "GET",
+                    datatype: "json",
                     data: { id: noFicha },
-                    success: function (res) {
+                    success: function (res)
+                    {
                         const maxFicha = parseInt(res.data);
-                        const diferencaGrande =
-                            noFicha > maxFicha + 100 ||
-                            noFicha < maxFicha - 100;
+                        const diferencaGrande = noFicha > maxFicha + 100 || noFicha < maxFicha - 100;
 
                         resolve({
                             success: true,
                             maxFicha: maxFicha,
-                            diferencaGrande: diferencaGrande,
+                            diferencaGrande: diferencaGrande
                         });
                     },
-                    error: function (jqXHR, textStatus, errorThrown) {
-                        const erro = criarErroAjax(
-                            jqXHR,
-                            textStatus,
-                            errorThrown,
-                            this,
-                        );
-                        Alerta.TratamentoErroComLinha(
-                            'viagem.service.js',
-                            'verificarFicha',
-                            erro,
-                        );
+                    error: function (jqXHR, textStatus, errorThrown)
+                    {
+                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
+                        Alerta.TratamentoErroComLinha("viagem.service.js", "verificarFicha", erro);
                         reject(erro);
-                    },
+                    }
                 });
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'viagem.service.js',
-                'verificarFicha',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("viagem.service.js", "verificarFicha", error);
             return {
                 success: false,
-                error: error.message,
+                error: error.message
             };
         }
     }
 
-    async fichaExiste(noFicha) {
-        try {
-            return new Promise((resolve, reject) => {
+    async fichaExiste(noFicha)
+    {
+        try
+        {
+            return new Promise((resolve, reject) =>
+            {
                 $.ajax({
-                    url: '/Viagens/Upsert?handler=FichaExistente',
-                    method: 'GET',
-                    datatype: 'json',
+                    url: "/Viagens/Upsert?handler=FichaExistente",
+                    method: "GET",
+                    datatype: "json",
                     data: { id: noFicha },
-                    success: function (res) {
+                    success: function (res)
+                    {
                         resolve({
                             success: true,
-                            existe: res.data === true,
+                            existe: res.data === true
                         });
                     },
-                    error: function (jqXHR, textStatus, errorThrown) {
-                        const erro = criarErroAjax(
-                            jqXHR,
-                            textStatus,
-                            errorThrown,
-                            this,
-                        );
-                        Alerta.TratamentoErroComLinha(
-                            'viagem.service.js',
-                            'fichaExiste',
-                            erro,
-                        );
+                    error: function (jqXHR, textStatus, errorThrown)
+                    {
+                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
+                        Alerta.TratamentoErroComLinha("viagem.service.js", "fichaExiste", erro);
                         reject(erro);
-                    },
+                    }
                 });
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'viagem.service.js',
-                'fichaExiste',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("viagem.service.js", "fichaExiste", error);
             return {
                 success: false,
                 error: error.message,
-                existe: false,
+                existe: false
             };
         }
     }
 
-    async listarSetores() {
-        try {
-            return new Promise((resolve, reject) => {
+    async listarSetores()
+    {
+        try
+        {
+            return new Promise((resolve, reject) =>
+            {
                 $.ajax({
-                    url: '/Viagens/Upsert?handler=AJAXPreencheListaSetores',
-                    method: 'GET',
-                    datatype: 'json',
-                    success: function (res) {
-                        const setores = res.data.map((item) => ({
+                    url: "/Viagens/Upsert?handler=AJAXPreencheListaSetores",
+                    method: "GET",
+                    datatype: "json",
+                    success: function (res)
+                    {
+                        const setores = res.data.map(item => ({
                             SetorSolicitanteId: item.setorSolicitanteId,
                             SetorPaiId: item.setorPaiId,
                             Nome: item.nome,
-                            HasChild: item.hasChild,
+                            HasChild: item.hasChild
                         }));
 
                         resolve({
                             success: true,
-                            data: setores,
+                            data: setores
                         });
                     },
-                    error: function (jqXHR, textStatus, errorThrown) {
-                        const erro = criarErroAjax(
-                            jqXHR,
-                            textStatus,
-                            errorThrown,
-                            this,
-                        );
-                        Alerta.TratamentoErroComLinha(
-                            'viagem.service.js',
-                            'listarSetores',
-                            erro,
-                        );
+                    error: function (jqXHR, textStatus, errorThrown)
+                    {
+                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
+                        Alerta.TratamentoErroComLinha("viagem.service.js", "listarSetores", erro);
                         reject(erro);
-                    },
+                    }
                 });
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'viagem.service.js',
-                'listarSetores',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("viagem.service.js", "listarSetores", error);
             return {
                 success: false,
                 error: error.message,
-                data: [],
+                data: []
             };
         }
     }
```
