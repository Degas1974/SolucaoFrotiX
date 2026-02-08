# Pages/Ocorrencia/Ocorrencias.cshtml

**Mudanca:** GRANDE | **+65** linhas | **-22** linhas

---

```diff
--- JANEIRO: Pages/Ocorrencia/Ocorrencias.cshtml
+++ ATUAL: Pages/Ocorrencia/Ocorrencias.cshtml
@@ -1,4 +1,5 @@
 @page
+
 @model FrotiX.Pages.Ocorrencia.OcorrenciasModel
 
 @using FrotiX.Repository.IRepository
@@ -433,7 +434,12 @@
 
                 <div class="col-12 col-md-3">
                     <label class="form-label">Data Específica</label>
-                    <input id="txtData" class="form-control form-control-sm" type="date" />
+                    <div class="input-group input-group-sm">
+                        <input id="txtData" class="form-control" type="date" />
+                        <button class="btn btn-outline-secondary" type="button" onclick="$('#txtData').val('').trigger('change');" title="Limpar Data">
+                            <i class="fa-duotone fa-xmark"></i>
+                        </button>
+                    </div>
                 </div>
 
                 <div class="col-12 col-md-5">
@@ -447,11 +453,21 @@
                         <div class="row g-2">
                             <div class="col-6">
                                 <label class="form-label small">Data Inicial</label>
-                                <input id="txtDataInicial" class="form-control form-control-sm" type="date" />
+                                <div class="input-group input-group-sm">
+                                    <input id="txtDataInicial" class="form-control" type="date" />
+                                    <button class="btn btn-outline-secondary" type="button" onclick="$('#txtDataInicial').val('').trigger('change');" title="Limpar Data Inicial">
+                                        <i class="fa-duotone fa-xmark"></i>
+                                    </button>
+                                </div>
                             </div>
                             <div class="col-6">
                                 <label class="form-label small">Data Final</label>
-                                <input id="txtDataFinal" class="form-control form-control-sm" type="date" />
+                                <div class="input-group input-group-sm">
+                                    <input id="txtDataFinal" class="form-control" type="date" />
+                                    <button class="btn btn-outline-secondary" type="button" onclick="$('#txtDataFinal').val('').trigger('change');" title="Limpar Data Final">
+                                        <i class="fa-duotone fa-xmark"></i>
+                                    </button>
+                                </div>
                             </div>
                         </div>
                     </div>
@@ -476,7 +492,8 @@
                                   placeholder="Selecione um Motorista..."
                                   allowFiltering="true" filterType="Contains"
                                   dataSource="@lstMotoristas"
-                                  popupHeight="250px" width="100%" showClearButton="true">
+                                  popupHeight="250px" width="100%" showClearButton="true"
+                                  created="onCmbMotoristaCreated">
                         <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                     </ejs-combobox>
                 </div>
@@ -568,21 +585,18 @@
             </div>
             <div class="modal-footer justify-content-between">
                 <div>
-                    <button id="btnBaixarOcorrenciaModal" type="button"
-                        style="background:linear-gradient(135deg,#27ae60,#2ecc71); border:none; color:#fff; padding:0.5rem 1.25rem; border-radius:6px; font-weight:600;">
-                        <i class="fa-duotone fa-flag-checkered me-1" style="color:#fff;"></i>
+                    <button id="btnBaixarOcorrenciaModal" type="button" class="btn btn-verde">
+                        <i class="fa-duotone fa-flag-checkered me-1"></i>
                         Dar Baixa
                     </button>
                 </div>
                 <div class="d-flex gap-2">
-                    <button id="btnEditarOcorrencia" type="button"
-                        style="background:linear-gradient(135deg,#2980b9,#3498db); border:none; color:#fff; padding:0.5rem 1.25rem; border-radius:6px; font-weight:600;">
-                        <i class="fa-duotone fa-floppy-disk me-1" style="color:#fff;"></i>
+                    <button id="btnEditarOcorrencia" type="button" class="btn btn-azul" data-ejtip="Salva a Ocorrência com os novos dados sem dar Baixa">
+                        <i class="fa-duotone fa-floppy-disk fa-lg me-1"></i>
                         Salvar Alterações
                     </button>
-                    <button id="btnFechar" type="button" data-bs-dismiss="modal"
-                        style="background:linear-gradient(135deg,#6c757d,#5a6268); border:none; color:#fff; padding:0.5rem 1.25rem; border-radius:6px; font-weight:600;">
-                        <i class="fa-duotone fa-xmark me-1" style="color:#fff;"></i>
+                    <button id="btnFechar" type="button" data-bs-dismiss="modal" class="btn btn-vinho">
+                        <i class="fa-duotone fa-circle-xmark me-1"></i>
                         Fechar
                     </button>
                 </div>
@@ -604,9 +618,9 @@
                 <div id="divImagemVisualizacao"></div>
             </div>
             <div class="modal-footer">
-                <button type="button" data-bs-dismiss="modal"
-                    style="background:linear-gradient(135deg,#6c757d,#5a6268); border:none; color:#fff; padding:0.5rem 1.25rem; border-radius:6px; font-weight:600;">
-                    <i class="fa-duotone fa-xmark me-1" style="color:#fff;"></i>Fechar
+                <button type="button" data-bs-dismiss="modal" class="btn btn-vinho"
+                    style="padding:0.5rem 1.25rem; border-radius:6px; font-weight:600;">
+                    <i class="fa-duotone fa-circle-xmark me-1"></i>Fechar
                 </button>
             </div>
         </div>
@@ -638,13 +652,13 @@
                 </div>
             </div>
             <div class="modal-footer justify-content-between">
-                <button type="button" data-bs-dismiss="modal"
-                    style="background:#6c757d; border:none; color:#fff; padding:0.5rem 1.25rem; border-radius:6px; font-weight:600;">
-                    <i class="fa-duotone fa-xmark me-1" style="color:#fff;"></i>Cancelar
+                <button type="button" data-bs-dismiss="modal" class="btn btn-vinho"
+                    style="padding:0.5rem 1.25rem; border-radius:6px; font-weight:600;">
+                    <i class="fa-duotone fa-circle-xmark me-1"></i>Cancelar
                 </button>
-                <button type="button" id="btnConfirmarBaixaRapida"
-                    style="background:linear-gradient(135deg,#27ae60,#2ecc71); border:none; color:#fff; padding:0.5rem 1.25rem; border-radius:6px; font-weight:600;">
-                    <i class="fa-duotone fa-flag-checkered me-1" style="color:#fff;"></i>Confirmar Baixa
+                <button type="button" id="btnConfirmarBaixaRapida" class="btn btn-verde"
+                    style="padding:0.5rem 1.25rem; border-radius:6px; font-weight:600;">
+                    <i class="fa-duotone fa-flag-checkered me-1"></i>Confirmar Baixa
                 </button>
             </div>
         </div>
@@ -662,4 +676,36 @@
 
 @section ScriptsBlock {
     <script src="~/js/cadastros/ocorrencias.js" asp-append-version="true"></script>
+    <script>
+        function onCmbMotoristaCreated() {
+            try {
+                var combo = document.getElementById('lstMotorista').ej2_instances[0];
+
+                combo.itemTemplate = function (data) {
+                    var imgSrc = (data.Foto && data.Foto.startsWith('data:image'))
+                        ? data.Foto
+                        : '/images/barbudo.jpg';
+                    return `
+                    <div class="d-flex align-items-center">
+                        <img src="${imgSrc}" alt="Foto" style="height:40px; width:40px; border-radius:50%; margin-right:10px;" />
+                        <span>${data.Descricao}</span>
+                    </div>`;
+                };
+
+                combo.valueTemplate = function (data) {
+                    if (!data) return '';
+                    var imgSrc = (data.Foto && data.Foto.startsWith('data:image'))
+                        ? data.Foto
+                        : '/images/barbudo.jpg';
+                    return `
+                    <div class="d-flex align-items-center">
+                        <img src="${imgSrc}" alt="Foto" style="height:30px; width:30px; border-radius:50%; margin-right:10px;" />
+                        <span>${data.Descricao}</span>
+                    </div>`;
+                };
+            } catch (error) {
+                console.error("Erro ao configurar templates do motorista:", error);
+            }
+        }
+    </script>
 }
```
