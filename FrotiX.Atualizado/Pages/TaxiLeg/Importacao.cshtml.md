# Pages/TaxiLeg/Importacao.cshtml

**Mudanca:** GRANDE | **+12** linhas | **-34** linhas

---

```diff
--- JANEIRO: Pages/TaxiLeg/Importacao.cshtml
+++ ATUAL: Pages/TaxiLeg/Importacao.cshtml
@@ -28,8 +28,7 @@
                                 <input type="file" id="fileupload" class="form-control" accept=".xls,.xlsx" />
                             </div>
                             <div class="col-12 col-md-6">
-                                <button id="btnupload" class="btn btn-azul text-white"
-                                    aria-label="Fazer upload da planilha do TaxiLeg">
+                                <button id="btnupload" class="btn btn-azul text-white" aria-label="Fazer upload da planilha do TaxiLeg">
                                     <i class="fa-duotone fa-file-plus icon-space icon-pulse"></i> &nbsp; Upload
                                 </button>
                             </div>
@@ -45,10 +44,8 @@
 </div>
 
 @section ScriptsBlock {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script src="https://cdn.syncfusion.com/ej2/dist/ej2.min.js"></script>
 
@@ -130,10 +127,10 @@
                                         const msg = (data && data.message) ? data.message : 'Nenhum dado para exibir.';
                                         AppToast.show('Amarela', msg, 2000);
                                         $('#divTaxiLeg').html(`
-                                                <div class="taxi-leg-container">
-                                                    <h4>${msg}</h4>
-                                                </div>
-                                            `);
+                                            <div class="taxi-leg-container">
+                                                <h4>${msg}</h4>
+                                            </div>
+                                        `);
                                     }
                                 } catch (error) {
                                     Alerta.TratamentoErroComLinha("Importacao.cshtml", "btnupload.ajax.success", error);
@@ -166,21 +163,6 @@
             }
         });
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * IMPORTAÇÃO TAXI LEG - FUNÇÕES DE GRID
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Funções para importação e visualização de dados do TaxiLeg
-            * em grid Syncfusion com scroll e formatação customizada.
-             * @@requires Syncfusion EJ2 Grid
-            * @@file TaxiLeg / Importacao.cshtml
-            */
-
-            /**
-             * Reseta a posição de scroll horizontal do grid
-             * @@param { Object } g - Instância do Syncfusion Grid
-            * @@description Sincroniza scroll do conteúdo e cabeçalho
-                */
         function resetScroll(g) {
             try {
 
@@ -194,11 +176,6 @@
             }
         }
 
-            /**
-             * Renderiza o grid Syncfusion com os dados importados
-             * @@param { Array } dados - Array de objetos com os dados do TaxiLeg
-            * @@description Cria grid com paginação, ordenação e colunas formatadas
-                */
         function renderSyncfusionGrid(dados) {
             try {
 
@@ -245,9 +222,9 @@
                                 try {
                                     return args.glosa === true
                                         ? `<span class="botao-sim-fake">
-                                                   <i class="fa-duotone fa-skull-crossbones"></i>
-                                                   Sim
-                                               </span>`
+                                               <i class="fa-duotone fa-skull-crossbones"></i>
+                                               Sim
+                                           </span>`
                                         : '';
                                 } catch (error) {
                                     Alerta.TratamentoErroComLinha("Importacao.cshtml", "grid.glosa.template", error);
@@ -280,8 +257,8 @@
                                 try {
                                     const total = Number(args.Sum) || 0;
                                     return `<span style="font-weight:bold;white-space:nowrap">
-                                                    Total: ${total.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
-                                                </span>`;
+                                                Total: ${total.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
+                                            </span>`;
                                 } catch (error) {
                                     Alerta.TratamentoErroComLinha("Importacao.cshtml", "grid.aggregates.footerTemplate", error);
                                     return '';
```

### REMOVER do Janeiro

```html
                                <button id="btnupload" class="btn btn-azul text-white"
                                    aria-label="Fazer upload da planilha do TaxiLeg">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
        crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
        crossorigin="anonymous"></script>
                                                <div class="taxi-leg-container">
                                                    <h4>${msg}</h4>
                                                </div>
                                            `);
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * IMPORTAÇÃO TAXI LEG - FUNÇÕES DE GRID
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Funções para importação e visualização de dados do TaxiLeg
            * em grid Syncfusion com scroll e formatação customizada.
             * @@requires Syncfusion EJ2 Grid
            * @@file TaxiLeg / Importacao.cshtml
            */
            /**
             * Reseta a posição de scroll horizontal do grid
             * @@param { Object } g - Instância do Syncfusion Grid
            * @@description Sincroniza scroll do conteúdo e cabeçalho
                */
            /**
             * Renderiza o grid Syncfusion com os dados importados
             * @@param { Array } dados - Array de objetos com os dados do TaxiLeg
            * @@description Cria grid com paginação, ordenação e colunas formatadas
                */
                                                   <i class="fa-duotone fa-skull-crossbones"></i>
                                                   Sim
                                               </span>`
                                                    Total: ${total.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                                </span>`;
```


### ADICIONAR ao Janeiro

```html
                                <button id="btnupload" class="btn btn-azul text-white" aria-label="Fazer upload da planilha do TaxiLeg">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
                                            <div class="taxi-leg-container">
                                                <h4>${msg}</h4>
                                            </div>
                                        `);
                                               <i class="fa-duotone fa-skull-crossbones"></i>
                                               Sim
                                           </span>`
                                                Total: ${total.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                            </span>`;
```
