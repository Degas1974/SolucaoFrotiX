# wwwroot/js/viagens/ocorrencia-viagem.js

**Mudanca:** GRANDE | **+36** linhas | **-75** linhas

---

```diff
--- JANEIRO: wwwroot/js/viagens/ocorrencia-viagem.js
+++ ATUAL: wwwroot/js/viagens/ocorrencia-viagem.js
@@ -3,9 +3,7 @@
     var contadorOcorrencias = 0;
 
     function init() {
-        $('#btnAdicionarOcorrencia')
-            .off('click')
-            .on('click', adicionarOcorrencia);
+        $('#btnAdicionarOcorrencia').off('click').on('click', adicionarOcorrencia);
         atualizarContador();
     }
 
@@ -15,9 +13,9 @@
         $('#listaOcorrencias').append(html);
 
         var card = $(`.card-ocorrencia[data-index="${contadorOcorrencias}"]`);
-        card.find('[data-bs-toggle="tooltip"]').each(function () {
+        card.find('[data-bs-toggle="tooltip"]').each(function() {
             new bootstrap.Tooltip(this, {
-                customClass: 'tooltip-ftx-azul',
+                customClass: 'tooltip-ftx-azul'
             });
         });
 
@@ -93,25 +91,18 @@
 
         var descricao = card.find('.input-descricao').val().trim();
 
-        card.find('.label-resumo')
-            .html('<strong>Resumo:</strong> ' + resumo)
-            .show();
+        card.find('.label-resumo').html('<strong>Resumo:</strong> ' + resumo).show();
         card.find('.input-resumo').hide();
 
         if (descricao) {
-            card.find('.label-descricao')
-                .html('<strong>Descrição:</strong> ' + descricao)
-                .show();
+            card.find('.label-descricao').html('<strong>Descrição:</strong> ' + descricao).show();
         }
         card.find('.input-descricao').hide();
 
         card.find('.input-imagem').prop('disabled', true).hide();
 
         card.find('.btn-confirmar-ocorrencia').hide();
-        card.find('.badge-status')
-            .removeClass('bg-warning text-dark')
-            .addClass('bg-success text-white')
-            .text('Confirmada #' + index);
+        card.find('.badge-status').removeClass('bg-warning text-dark').addClass('bg-success text-white').text('Confirmada #' + index);
     }
 
     function atualizarContador() {
@@ -134,17 +125,9 @@
             var isVideo = file.type.startsWith('video/');
 
             if (isVideo) {
-                container.html(
-                    '<video src="' +
-                        URL.createObjectURL(file) +
-                        '" controls class="img-thumbnail" style="max-height:100px;"></video>',
-                );
+                container.html('<video src="' + URL.createObjectURL(file) + '" controls class="img-thumbnail" style="max-height:100px;"></video>');
             } else {
-                container.html(
-                    '<img src="' +
-                        URL.createObjectURL(file) +
-                        '" class="img-thumbnail" style="max-height:100px;">',
-                );
+                container.html('<img src="' + URL.createObjectURL(file) + '" class="img-thumbnail" style="max-height:100px;">');
             }
 
             uploadImagem(file, index);
@@ -166,7 +149,7 @@
                     var card = $(`.card-ocorrencia[data-index="${index}"]`);
                     card.find('.input-imagem-url').val(res.url);
                 }
-            },
+            }
         });
     }
 
@@ -180,9 +163,7 @@
             var resumo = $(this).find('.input-resumo').val().trim();
             if (!resumo) {
                 $(this).addClass('border-danger shake');
-                setTimeout(function () {
-                    $('.shake').removeClass('shake');
-                }, 500);
+                setTimeout(function () { $('.shake').removeClass('shake'); }, 500);
                 valido = false;
             }
         });
@@ -191,17 +172,15 @@
 
     function coletarOcorrenciasSimples() {
         var lista = [];
-        console.log(
-            '📋 Coletando ocorrências simples para envio junto com viagem...',
-        );
+        console.log("📋 Coletando ocorrências simples para envio junto com viagem...");
 
         $('.card-ocorrencia').each(function (idx) {
             var item = {
                 Resumo: $(this).find('.input-resumo').val().trim(),
                 Descricao: $(this).find('.input-descricao').val().trim(),
-                ImagemOcorrencia: $(this).find('.input-imagem-url').val() || '',
+                ImagemOcorrencia: $(this).find('.input-imagem-url').val() || ''
             };
-            console.log(' Ocorrência ' + (idx + 1) + ':', item);
+            console.log(" Ocorrência " + (idx + 1) + ":", item);
             lista.push(item);
         });
         return lista;
@@ -209,22 +188,21 @@
 
     function coletarOcorrencias(viagemId, veiculoId, motoristaId) {
         var lista = [];
-        console.log('📋 Coletando ocorrências...');
-        console.log(' viagemId:', viagemId);
-        console.log(' veiculoId:', veiculoId);
-        console.log(' motoristaId:', motoristaId);
+        console.log("📋 Coletando ocorrências...");
+        console.log(" viagemId:", viagemId);
+        console.log(" veiculoId:", veiculoId);
+        console.log(" motoristaId:", motoristaId);
 
         $('.card-ocorrencia').each(function (idx) {
             var item = {
                 ViagemId: viagemId,
                 VeiculoId: veiculoId,
-                MotoristaId:
-                    motoristaId || '00000000-0000-0000-0000-000000000000',
+                MotoristaId: motoristaId || '00000000-0000-0000-0000-000000000000',
                 Resumo: $(this).find('.input-resumo').val().trim(),
                 Descricao: $(this).find('.input-descricao').val().trim(),
-                ImagemOcorrencia: $(this).find('.input-imagem-url').val() || '',
+                ImagemOcorrencia: $(this).find('.input-imagem-url').val() || ''
             };
-            console.log(' Ocorrência ' + (idx + 1) + ':', item);
+            console.log(" Ocorrência " + (idx + 1) + ":", item);
             lista.push(item);
         });
         return lista;
@@ -232,36 +210,28 @@
 
     function salvarOcorrencias(viagemId, veiculoId, motoristaId, callback) {
         try {
-            console.log('💾 Iniciando salvamento de ocorrências...');
+            console.log("💾 Iniciando salvamento de ocorrências...");
 
             if (!viagemId || viagemId === '' || viagemId === 'undefined') {
-                console.error('❌ ViagemId inválido:', viagemId);
-                if (callback)
-                    callback({ success: false, message: 'ViagemId inválido' });
+                console.error("❌ ViagemId inválido:", viagemId);
+                if (callback) callback({ success: false, message: 'ViagemId inválido' });
                 return;
             }
             if (!veiculoId || veiculoId === '' || veiculoId === 'undefined') {
-                console.error('❌ VeiculoId inválido:', veiculoId);
-                if (callback)
-                    callback({ success: false, message: 'VeiculoId inválido' });
+                console.error("❌ VeiculoId inválido:", veiculoId);
+                if (callback) callback({ success: false, message: 'VeiculoId inválido' });
                 return;
             }
 
             var lista = coletarOcorrencias(viagemId, veiculoId, motoristaId);
             if (lista.length === 0) {
-                console.log('ℹ️ Nenhuma ocorrência para salvar.');
-                if (callback)
-                    callback({
-                        success: true,
-                        message: 'Nenhuma ocorrência para salvar.',
-                    });
+                console.log("ℹ️ Nenhuma ocorrência para salvar.");
+                if (callback) callback({ success: true, message: 'Nenhuma ocorrência para salvar.' });
                 return;
             }
 
-            console.log(
-                '📤 Enviando ' + lista.length + ' ocorrência(s) para API...',
-            );
-            console.log(' Payload:', JSON.stringify(lista, null, 2));
+            console.log("📤 Enviando " + lista.length + " ocorrência(s) para API...");
+            console.log(" Payload:", JSON.stringify(lista, null, 2));
 
             $.ajax({
                 url: '/api/OcorrenciaViagem/CriarMultiplas',
@@ -269,26 +239,17 @@
                 contentType: 'application/json',
                 data: JSON.stringify(lista),
                 success: function (res) {
-                    console.log('✅ Resposta da API:', res);
+                    console.log("✅ Resposta da API:", res);
                     if (callback) callback(res);
                 },
                 error: function (xhr, status, error) {
-                    console.error('❌ Erro AJAX:', {
-                        status: status,
-                        error: error,
-                        response: xhr.responseText,
-                    });
-                    if (callback)
-                        callback({
-                            success: false,
-                            message: 'Erro de comunicação: ' + error,
-                        });
-                },
+                    console.error("❌ Erro AJAX:", { status: status, error: error, response: xhr.responseText });
+                    if (callback) callback({ success: false, message: 'Erro de comunicação: ' + error });
+                }
             });
         } catch (ex) {
-            console.error('❌ Exceção em salvarOcorrencias:', ex);
-            if (callback)
-                callback({ success: false, message: 'Exceção: ' + ex.message });
+            console.error("❌ Exceção em salvarOcorrencias:", ex);
+            if (callback) callback({ success: false, message: 'Exceção: ' + ex.message });
         }
     }
 
@@ -308,7 +269,7 @@
         validarOcorrencias: validarOcorrencias,
         coletarOcorrenciasSimples: coletarOcorrenciasSimples,
         salvarOcorrencias: salvarOcorrencias,
-        limparOcorrencias: limparOcorrencias,
+        limparOcorrencias: limparOcorrencias
     };
 })();
 
```
