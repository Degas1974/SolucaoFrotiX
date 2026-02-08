# wwwroot/js/viagens/ocorrencia-viagem-popup.js

**Mudanca:** GRANDE | **+28** linhas | **-55** linhas

---

```diff
--- JANEIRO: wwwroot/js/viagens/ocorrencia-viagem-popup.js
+++ ATUAL: wwwroot/js/viagens/ocorrencia-viagem-popup.js
@@ -1,29 +1,18 @@
 var OcorrenciaViagemPopup = (function () {
+
     function verificar(veiculoId, veiculoDescricao, callback) {
-        if (
-            !veiculoId ||
-            veiculoId === '00000000-0000-0000-0000-000000000000'
-        ) {
+        if (!veiculoId || veiculoId === '00000000-0000-0000-0000-000000000000') {
             if (callback) callback();
             return;
         }
 
-        $.get(
-            '/api/OcorrenciaViagem/ContarAbertasPorVeiculo',
-            { veiculoId: veiculoId },
-            function (res) {
-                if (res.success && res.count > 0) {
-                    mostrarPopup(
-                        veiculoId,
-                        veiculoDescricao,
-                        res.count,
-                        callback,
-                    );
-                } else {
-                    if (callback) callback();
-                }
-            },
-        );
+        $.get('/api/OcorrenciaViagem/ContarAbertasPorVeiculo', { veiculoId: veiculoId }, function (res) {
+            if (res.success && res.count > 0) {
+                mostrarPopup(veiculoId, veiculoDescricao, res.count, callback);
+            } else {
+                if (callback) callback();
+            }
+        });
     }
 
     function mostrarPopup(veiculoId, veiculoDescricao, count, callback) {
@@ -57,9 +46,7 @@
             </div>`;
 
         $('body').append(modalHtml);
-        var modal = new bootstrap.Modal(
-            document.getElementById('modalOcorrenciasAbertas'),
-        );
+        var modal = new bootstrap.Modal(document.getElementById('modalOcorrenciasAbertas'));
         modal.show();
 
         carregarOcorrencias(veiculoId);
@@ -75,22 +62,15 @@
     }
 
     function carregarOcorrencias(veiculoId) {
-        $.get(
-            '/api/OcorrenciaViagem/ListarAbertasPorVeiculo',
-            { veiculoId: veiculoId },
-            function (res) {
-                if (res.success) {
-                    var html = '';
-                    res.data.forEach(function (oc) {
-                        html += criarItemOcorrencia(oc);
-                    });
-                    $('#listaOcorrenciasAbertas').html(
-                        html ||
-                            '<p class="text-muted">Nenhuma ocorrência encontrada.</p>',
-                    );
-                }
-            },
-        );
+        $.get('/api/OcorrenciaViagem/ListarAbertasPorVeiculo', { veiculoId: veiculoId }, function (res) {
+            if (res.success) {
+                var html = '';
+                res.data.forEach(function (oc) {
+                    html += criarItemOcorrencia(oc);
+                });
+                $('#listaOcorrenciasAbertas').html(html || '<p class="text-muted">Nenhuma ocorrência encontrada.</p>');
+            }
+        });
     }
 
     function criarItemOcorrencia(oc) {
@@ -121,25 +101,19 @@
     function darBaixa(ocorrenciaId) {
         if (!confirm('Confirma dar baixa nesta ocorrência?')) return;
 
-        $.post(
-            '/api/OcorrenciaViagem/DarBaixa',
-            { ocorrenciaId: ocorrenciaId },
-            function (res) {
-                if (res.success) {
-                    AppToast.show('Verde', 'Ocorrência baixada!', 2000);
-                    var veiculoId = $('#modalOcorrenciasAbertas').data(
-                        'veiculo-id',
-                    );
-                    carregarOcorrencias(veiculoId);
-                } else {
-                    AppToast.show('Vermelho', res.message, 3000);
-                }
-            },
-        );
+        $.post('/api/OcorrenciaViagem/DarBaixa', { ocorrenciaId: ocorrenciaId }, function (res) {
+            if (res.success) {
+                AppToast.show('Verde', 'Ocorrência baixada!', 2000);
+                var veiculoId = $('#modalOcorrenciasAbertas').data('veiculo-id');
+                carregarOcorrencias(veiculoId);
+            } else {
+                AppToast.show('Vermelho', res.message, 3000);
+            }
+        });
     }
 
     return {
         verificar: verificar,
-        darBaixa: darBaixa,
+        darBaixa: darBaixa
     };
 })();
```
