# wwwroot/js/agendamento/services/evento.service.js

**Mudanca:** GRANDE | **+56** linhas | **-61** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/services/evento.service.js
+++ ATUAL: wwwroot/js/agendamento/services/evento.service.js
@@ -1,99 +1,96 @@
-class EventoService {
-    constructor() {
+class EventoService
+{
+    constructor()
+    {
         this.api = window.ApiClient;
     }
 
-    async adicionar(dados) {
-        try {
-            const response = await this.api.post(
-                '/api/Viagem/AdicionarEvento',
-                dados,
-            );
+    async adicionar(dados)
+    {
+        try
+        {
+            const response = await this.api.post('/api/Viagem/AdicionarEvento', dados);
 
-            if (response.success) {
+            if (response.success)
+            {
                 return {
                     success: true,
                     message: response.message,
                     eventoId: response.eventoId,
-                    eventoText: response.eventoText,
+                    eventoText: response.eventoText
                 };
-            } else {
+            } else
+            {
                 return {
                     success: false,
-                    message: response.message || 'Erro ao adicionar evento',
+                    message: response.message || "Erro ao adicionar evento"
                 };
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'evento.service.js',
-                'adicionar',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("evento.service.js", "adicionar", error);
             return {
                 success: false,
-                error: error.message,
+                error: error.message
             };
         }
     }
 
-    async listar() {
-        try {
-            return new Promise((resolve, reject) => {
+    async listar()
+    {
+        try
+        {
+            return new Promise((resolve, reject) =>
+            {
                 $.ajax({
-                    url: '/Viagens/Upsert?handler=AJAXPreencheListaEventos',
-                    method: 'GET',
-                    datatype: 'json',
-                    success: function (res) {
-                        const eventos = res.data.map((item) => ({
+                    url: "/Viagens/Upsert?handler=AJAXPreencheListaEventos",
+                    method: "GET",
+                    datatype: "json",
+                    success: function (res)
+                    {
+                        const eventos = res.data.map(item => ({
                             EventoId: item.eventoId,
-                            Evento: item.nome,
+                            Evento: item.nome
                         }));
 
                         resolve({
                             success: true,
-                            data: eventos,
+                            data: eventos
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
-                            'evento.service.js',
-                            'listar',
-                            erro,
-                        );
+                    error: function (jqXHR, textStatus, errorThrown)
+                    {
+                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
+                        Alerta.TratamentoErroComLinha("evento.service.js", "listar", erro);
                         reject(erro);
-                    },
+                    }
                 });
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('evento.service.js', 'listar', error);
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("evento.service.js", "listar", error);
             return {
                 success: false,
                 error: error.message,
-                data: [],
+                data: []
             };
         }
     }
 
-    async atualizarListaDropdown(eventoId = null) {
-        try {
+    async atualizarListaDropdown(eventoId = null)
+    {
+        try
+        {
             const result = await this.listar();
 
-            if (!result.success) {
+            if (!result.success)
+            {
                 throw new Error(result.error);
             }
 
-            const lstEventosElement = document.getElementById('lstEventos');
-            if (
-                !lstEventosElement ||
-                !lstEventosElement.ej2_instances ||
-                !lstEventosElement.ej2_instances[0]
-            ) {
+            const lstEventosElement = document.getElementById("lstEventos");
+            if (!lstEventosElement || !lstEventosElement.ej2_instances || !lstEventosElement.ej2_instances[0])
+            {
                 return;
             }
 
@@ -101,15 +98,13 @@
             lstEventos.dataSource = result.data;
             lstEventos.dataBind();
 
-            if (eventoId) {
+            if (eventoId)
+            {
                 lstEventos.value = eventoId;
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'evento.service.js',
-                'atualizarListaDropdown',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("evento.service.js", "atualizarListaDropdown", error);
         }
     }
 }
```
