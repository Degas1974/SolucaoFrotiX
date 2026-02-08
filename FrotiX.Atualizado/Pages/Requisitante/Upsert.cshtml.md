# Pages/Requisitante/Upsert.cshtml

**Mudanca:** GRANDE | **+38** linhas | **-59** linhas

---

```diff
--- JANEIRO: Pages/Requisitante/Upsert.cshtml
+++ ATUAL: Pages/Requisitante/Upsert.cshtml
@@ -37,18 +37,13 @@
             left: -100%;
             width: 100%;
             height: 100%;
-            background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.1), transparent);
+            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.1), transparent);
             animation: shimmer 3s infinite;
         }
 
         @@keyframes shimmer {
-            0% {
-                left: -100%;
-            }
-
-            100% {
-                left: 100%;
-            }
+            0% { left: -100%; }
+            100% { left: 100%; }
         }
 
         .ftx-card-header .titulo-paginas {
@@ -98,7 +93,7 @@
             background: #fff;
             border-radius: 8px;
             margin-bottom: 1rem;
-            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06);
+            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
             overflow: hidden;
         }
 
@@ -278,51 +273,48 @@
                                 <div class="row g-3">
                                     <div class="col-12 col-md-2">
                                         <label class="ftx-label" asp-for="RequisitanteObj.Requisitante.Ponto"></label>
-                                        <span class="text-danger small"
-                                            asp-validation-for="RequisitanteObj.Requisitante.Ponto"></span>
-                                        <input class="form-control ftx-form-control"
-                                            asp-for="RequisitanteObj.Requisitante.Ponto" placeholder="Ex: p_123" />
+                                        <span class="text-danger small" asp-validation-for="RequisitanteObj.Requisitante.Ponto"></span>
+                                        <input class="form-control ftx-form-control" asp-for="RequisitanteObj.Requisitante.Ponto" placeholder="Ex: p_123" />
                                     </div>
 
                                     <div class="col-12 col-md-4">
                                         <label class="ftx-label" asp-for="RequisitanteObj.Requisitante.Nome"></label>
-                                        <span class="text-danger small"
-                                            asp-validation-for="RequisitanteObj.Requisitante.Nome"></span>
-                                        <input class="form-control ftx-form-control"
-                                            asp-for="RequisitanteObj.Requisitante.Nome"
-                                            placeholder="Nome do requisitante" />
+                                        <span class="text-danger small" asp-validation-for="RequisitanteObj.Requisitante.Nome"></span>
+                                        <input class="form-control ftx-form-control" asp-for="RequisitanteObj.Requisitante.Nome" placeholder="Nome do requisitante" />
                                     </div>
 
                                     <div class="col-6 col-md-2">
                                         <label class="ftx-label" asp-for="RequisitanteObj.Requisitante.Ramal"></label>
-                                        <span class="text-danger small"
-                                            asp-validation-for="RequisitanteObj.Requisitante.Ramal"></span>
-                                        <input class="form-control ftx-form-control"
-                                            asp-for="RequisitanteObj.Requisitante.Ramal" type="number"
-                                            placeholder="Ramal" />
+                                        <span class="text-danger small" asp-validation-for="RequisitanteObj.Requisitante.Ramal"></span>
+                                        <input class="form-control ftx-form-control" asp-for="RequisitanteObj.Requisitante.Ramal" type="number" placeholder="Ramal" />
                                     </div>
 
                                     <div class="col-12 col-md-4">
                                         <label class="ftx-label" asp-for="RequisitanteObj.Requisitante.Email"></label>
-                                        <span class="text-danger small"
-                                            asp-validation-for="RequisitanteObj.Requisitante.Email"></span>
-                                        <input class="form-control ftx-form-control"
-                                            asp-for="RequisitanteObj.Requisitante.Email" type="email"
-                                            placeholder="email@exemplo.com" />
+                                        <span class="text-danger small" asp-validation-for="RequisitanteObj.Requisitante.Email"></span>
+                                        <input class="form-control ftx-form-control" asp-for="RequisitanteObj.Requisitante.Email" type="email" placeholder="email@exemplo.com" />
                                     </div>
                                 </div>
 
                                 <div class="row g-3 mt-2">
                                     <div class="col-12 col-md-6">
                                         <label class="ftx-label">Setor Solicitante</label>
-                                        <ejs-dropdowntree id="ddtree" placeholder="Selecione um Setor"
-                                            sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
-                                            allowFiltering="true" filterType="Contains"
-                                            filterBarPlaceholder="Procurar..." select="select" change="valueChange"
-                                            ejs-for="@Model.RequisitanteObj.Requisitante.SetorSolicitanteId">
+                                        <ejs-dropdowntree id="ddtree"
+                                                          placeholder="Selecione um Setor"
+                                                          sortOrder="Ascending"
+                                                          showCheckBox="false"
+                                                          allowMultiSelection="false"
+                                                          allowFiltering="true"
+                                                          filterType="Contains"
+                                                          filterBarPlaceholder="Procurar..."
+                                                          select="select"
+                                                          change="valueChange"
+                                                          ejs-for="@Model.RequisitanteObj.Requisitante.SetorSolicitanteId">
                                             <e-dropdowntree-fields dataSource="@ViewData["dataSource"]"
-                                                value="SetorSolicitanteId" text="Nome" parentValue="SetorPaiId"
-                                                hasChildren="HasChild">
+                                                                   value="SetorSolicitanteId"
+                                                                   text="Nome"
+                                                                   parentValue="SetorPaiId"
+                                                                   hasChildren="HasChild">
                                             </e-dropdowntree-fields>
                                         </ejs-dropdowntree>
                                     </div>
@@ -337,8 +329,7 @@
                             </div>
                             <div class="ftx-section-body">
                                 <div class="ftx-config-card" style="max-width: 200px;">
-                                    <input type="checkbox" class="form-check-input"
-                                        asp-for="RequisitanteObj.Requisitante.Status" id="chkStatus" />
+                                    <input type="checkbox" class="form-check-input" asp-for="RequisitanteObj.Requisitante.Status" id="chkStatus" />
                                     <label for="chkStatus">Ativo</label>
                                 </div>
                             </div>
@@ -347,17 +338,22 @@
                         <div class="ftx-actions-row">
                             @if (isEdit)
                             {
-                                <button type="submit" asp-page-handler="Edit"
-                                    asp-route-id="@Model.RequisitanteObj.Requisitante.RequisitanteId"
-                                    class="btn btn-azul btn-submit-spin" data-ejtip="Atualizar Requisitante">
+                                <button type="submit"
+                                        asp-page-handler="Edit"
+                                        asp-route-id="@Model.RequisitanteObj.Requisitante.RequisitanteId"
+                                        class="btn btn-azul btn-submit-spin"
+                                        data-ejtip="Atualizar Requisitante">
                                     <i class="fa-duotone fa-floppy-disk icon-pulse"></i>
                                     Atualizar Requisitante
                                 </button>
                             }
                             else
                             {
-                                <button type="submit" value="Submit" asp-page-handler="Submit"
-                                    class="btn btn-azul btn-submit-spin" data-ejtip="Criar Requisitante">
+                                <button type="submit"
+                                        value="Submit"
+                                        asp-page-handler="Submit"
+                                        class="btn btn-azul btn-submit-spin"
+                                        data-ejtip="Criar Requisitante">
                                     <i class="fa-duotone fa-floppy-disk icon-pulse"></i>
                                     Criar Requisitante
                                 </button>
@@ -378,16 +374,6 @@
 
 @section ScriptsBlock {
     <script type="text/javascript">
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * UPSERT REQUISITANTE - FORMULÁRIO DE CADASTRO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Gerencia formulário de criação / edição de requisitantes.
-             * Integra com DropDownTree para seleção de setor.
-             * @@requires jQuery, Syncfusion DropDownTree
-            * @@file Requisitante / Upsert.cshtml
-            */
-
         $(document).ready(function () {
             try {
 
@@ -402,10 +388,6 @@
     </script>
 
     <script type="text/javascript">
-            /**
-             * Handler de mudança de valor no DropDownTree
-             * @@description Loga valor e texto selecionado no console
-            */
         function valueChange() {
             try {
                 var ddTreeObj = document.getElementById("ddtree").ej2_instances[0];
@@ -415,11 +397,6 @@
             }
         }
 
-            /**
-             * Handler de seleção de item no DropDownTree
-             * @@param { Object } args - Evento de seleção
-            * @@description Loga valor e texto selecionado no console
-                */
         function select(args) {
             try {
                 var ddTreeObj = document.getElementById("ddtree").ej2_instances[0];
```
