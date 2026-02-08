# Pages/PlacaBronze/Upsert.cshtml

**Mudanca:** GRANDE | **+57** linhas | **-20** linhas

---

```diff
--- JANEIRO: Pages/PlacaBronze/Upsert.cshtml
+++ ATUAL: Pages/PlacaBronze/Upsert.cshtml
@@ -122,27 +122,68 @@
     <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
 
     <script>
-        $.ajax({
-            type: "GET",
-            url: "/PlacaBronze/Upsert?handler=VeiculoData",
-            data: {},
-            success: function (data) {
-                var s = '<option value="">-- Selecione um Veículo (opcional) --</option>';
-                for (var i = 0; i < data.length; i++) {
-                    s += '<option value="' + data[i]["value"] + '">' + data[i]["text"] + '</option>';
+        /***
+         * ⚡ FUNÇÃO: Carregamento de Lista de Veículos (AJAX)
+         * ============================================================================
+         * 🎯 OBJETIVO : Buscar lista de veículos do servidor e popular dropdown
+         * #VeiculoLista dinamicamente, com pré-seleção se em edição
+         *
+         * 📥 ENTRADAS : GET /PlacaBronze/Upsert?handler=VeiculoData (server response)
+         *
+         * 📤 SAÍDAS : #VeiculoLista populado com <option> tags (value + text),
+         * seleção pré-preenchida se veiculo_id != Guid.Empty
+         *
+         * 🎯 MOTIVO : Veículos são carregados dinamicamente do banco para permitir
+         * seleção da associação da Placa de Bronze
+         *
+         * 📝 OBSERVAÇÕES : [AJAX] Veículo é OPCIONAL na Placa de Bronze.
+         * Se falhar, exibe só placeholder.
+         ***/
+        try {
+            /***
+             * [AJAX] Endpoint: GET /PlacaBronze/Upsert?handler=VeiculoData
+             * ============================================================================
+             * 📥 ENVIA : Nenhum parâmetro
+             * 📤 RECEBE : Array<{ value: Guid, text: string }> - Lista de veículos
+             * 🎯 MOTIVO : Popular dropdown de seleção de veículo associado
+             ***/
+            $.ajax({
+                type: "GET",
+                url: "/PlacaBronze/Upsert?handler=VeiculoData",
+                data: {},
+                success: function (data) {
+                    try {
+
+                        var s = '<option value="">-- Selecione um Veículo (opcional) --</option>';
+
+                        for (var i = 0; i < data.length; i++) {
+                            s += '<option value="' + data[i]["value"] + '">' + data[i]["text"] + '</option>';
+                        }
+
+                        $("#VeiculoLista").html(s);
+
+                        if ("@veiculo_id" == "00000000-0000-0000-0000-000000000000") {
+                            $("#VeiculoLista").val("");
+                        } else {
+                            $("#VeiculoLista").val("@veiculo_id");
+                        }
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("PlacaBronze/Upsert.cshtml", "ajax.success", error);
+                    }
+                },
+                error: function (data) {
+                    try {
+
+                        var s = '<option value="">-- Selecione um Veículo (opcional) --</option>';
+                        $("#VeiculoLista").html(s);
+                        console.warn("Erro ao carregar veículos via AJAX");
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("PlacaBronze/Upsert.cshtml", "ajax.error", error);
+                    }
                 }
-                $("#VeiculoLista").html(s);
-
-                if ("@veiculo_id" == "00000000-0000-0000-0000-000000000000") {
-                    $("#VeiculoLista").val("");
-                } else {
-                    $("#VeiculoLista").val("@veiculo_id");
-                }
-            },
-            error: function (data) {
-                var s = '<option value="">-- Selecione um Veículo (opcional) --</option>';
-                $("#VeiculoLista").html(s);
-            }
-        });
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("PlacaBronze/Upsert.cshtml", "script.setup", error);
+        }
     </script>
 }
```

### REMOVER do Janeiro

```html
        $.ajax({
            type: "GET",
            url: "/PlacaBronze/Upsert?handler=VeiculoData",
            data: {},
            success: function (data) {
                var s = '<option value="">-- Selecione um Veículo (opcional) --</option>';
                for (var i = 0; i < data.length; i++) {
                    s += '<option value="' + data[i]["value"] + '">' + data[i]["text"] + '</option>';
                $("#VeiculoLista").html(s);
                if ("@veiculo_id" == "00000000-0000-0000-0000-000000000000") {
                    $("#VeiculoLista").val("");
                } else {
                    $("#VeiculoLista").val("@veiculo_id");
                }
            },
            error: function (data) {
                var s = '<option value="">-- Selecione um Veículo (opcional) --</option>';
                $("#VeiculoLista").html(s);
            }
        });
```


### ADICIONAR ao Janeiro

```html
        /***
         * ⚡ FUNÇÃO: Carregamento de Lista de Veículos (AJAX)
         * ============================================================================
         * 🎯 OBJETIVO : Buscar lista de veículos do servidor e popular dropdown
         * #VeiculoLista dinamicamente, com pré-seleção se em edição
         *
         * 📥 ENTRADAS : GET /PlacaBronze/Upsert?handler=VeiculoData (server response)
         *
         * 📤 SAÍDAS : #VeiculoLista populado com <option> tags (value + text),
         * seleção pré-preenchida se veiculo_id != Guid.Empty
         *
         * 🎯 MOTIVO : Veículos são carregados dinamicamente do banco para permitir
         * seleção da associação da Placa de Bronze
         *
         * 📝 OBSERVAÇÕES : [AJAX] Veículo é OPCIONAL na Placa de Bronze.
         * Se falhar, exibe só placeholder.
         ***/
        try {
            /***
             * [AJAX] Endpoint: GET /PlacaBronze/Upsert?handler=VeiculoData
             * ============================================================================
             * 📥 ENVIA : Nenhum parâmetro
             * 📤 RECEBE : Array<{ value: Guid, text: string }> - Lista de veículos
             * 🎯 MOTIVO : Popular dropdown de seleção de veículo associado
             ***/
            $.ajax({
                type: "GET",
                url: "/PlacaBronze/Upsert?handler=VeiculoData",
                data: {},
                success: function (data) {
                    try {
                        var s = '<option value="">-- Selecione um Veículo (opcional) --</option>';
                        for (var i = 0; i < data.length; i++) {
                            s += '<option value="' + data[i]["value"] + '">' + data[i]["text"] + '</option>';
                        }
                        $("#VeiculoLista").html(s);
                        if ("@veiculo_id" == "00000000-0000-0000-0000-000000000000") {
                            $("#VeiculoLista").val("");
                        } else {
                            $("#VeiculoLista").val("@veiculo_id");
                        }
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("PlacaBronze/Upsert.cshtml", "ajax.success", error);
                    }
                },
                error: function (data) {
                    try {
                        var s = '<option value="">-- Selecione um Veículo (opcional) --</option>';
                        $("#VeiculoLista").html(s);
                        console.warn("Erro ao carregar veículos via AJAX");
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("PlacaBronze/Upsert.cshtml", "ajax.error", error);
                    }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha("PlacaBronze/Upsert.cshtml", "script.setup", error);
        }
```
