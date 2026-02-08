# Pages/Multa/UpsertTipoMulta.cshtml

**Mudanca:** GRANDE | **+8** linhas | **-29** linhas

---

```diff
--- JANEIRO: Pages/Multa/UpsertTipoMulta.cshtml
+++ ATUAL: Pages/Multa/UpsertTipoMulta.cshtml
@@ -48,8 +48,7 @@
                                 <div class="form-group">
                                     <label class="form-label fw-bold" asp-for="TipoMultaObj.Artigo"></label>
                                     <span class="text-danger" asp-validation-for="TipoMultaObj.Artigo"></span>
-                                    <input class="form-control" asp-for="TipoMultaObj.Artigo"
-                                        data-ejtip="Artigo do Código de Trânsito" />
+                                    <input class="form-control" asp-for="TipoMultaObj.Artigo" data-ejtip="Artigo do Código de Trânsito" />
                                 </div>
                             </div>
 
@@ -57,8 +56,7 @@
                                 <div class="form-group">
                                     <label class="form-label fw-bold" asp-for="TipoMultaObj.Infracao"></label>
                                     <span class="text-danger" asp-validation-for="TipoMultaObj.Infracao"></span>
-                                    <select class="form-control" asp-for="TipoMultaObj.Infracao"
-                                        data-ejtip="Gravidade da Infração">
+                                    <select class="form-control" asp-for="TipoMultaObj.Infracao" data-ejtip="Gravidade da Infração">
                                         <option value="">-- Infração --</option>
                                         <option value="Gravíssima">Gravíssima</option>
                                         <option value="Grave">Grave</option>
@@ -72,8 +70,7 @@
                                 <div class="form-group">
                                     <label class="form-label fw-bold" asp-for="TipoMultaObj.CodigoDenatran"></label>
                                     <span class="text-danger" asp-validation-for="TipoMultaObj.CodigoDenatran"></span>
-                                    <input id="txtDenatran" class="form-control" asp-for="TipoMultaObj.CodigoDenatran"
-                                        data-ejtip="Código Denatran" />
+                                    <input id="txtDenatran" class="form-control" asp-for="TipoMultaObj.CodigoDenatran" data-ejtip="Código Denatran" />
                                 </div>
                             </div>
 
@@ -81,30 +78,25 @@
                                 <div class="form-group">
                                     <label class="form-label fw-bold" asp-for="TipoMultaObj.Desdobramento"></label>
                                     <span class="text-danger" asp-validation-for="TipoMultaObj.Desdobramento"></span>
-                                    <input class="form-control" asp-for="TipoMultaObj.Desdobramento"
-                                        data-ejtip="Desdobramento da Infração" />
+                                    <input class="form-control" asp-for="TipoMultaObj.Desdobramento" data-ejtip="Desdobramento da Infração" />
                                 </div>
                             </div>
 
                             <div class="col-12">
                                 <div class="form-group">
                                     <label class="form-label fw-bold">Descrição da Multa</label>
-                                    <ejs-richtexteditor ejs-for="@Model.TipoMultaObj.Descricao" id="rte"
-                                        toolbarClick="toolbarClick" locale="pt-BR" height="200px">
-                                        <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage"
-                                            path="./DadosEditaveis/ImagensViagens/"></e-richtexteditor-insertimagesettings>
+                                    <ejs-richtexteditor ejs-for="@Model.TipoMultaObj.Descricao" id="rte" toolbarClick="toolbarClick" locale="pt-BR" height="200px">
+                                        <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage" path="./DadosEditaveis/ImagensViagens/"></e-richtexteditor-insertimagesettings>
                                     </ejs-richtexteditor>
                                     <div id="errorMessage">
-                                        <span class="text-danger"
-                                            asp-validation-for="@Model.TipoMultaObj.Descricao"></span>
+                                        <span class="text-danger" asp-validation-for="@Model.TipoMultaObj.Descricao"></span>
                                     </div>
                                 </div>
                             </div>
 
                             <div class="col-12 mt-4">
                                 <div class="d-flex gap-2">
-                                    <button type="submit" value="Submit" asp-page-handler="Submit"
-                                        class="btn btn-azul btn-submit-spin px-4">
+                                    <button type="submit" value="Submit" asp-page-handler="Submit" class="btn btn-azul btn-submit-spin px-4">
                                         <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
                                         @(isEdit ? "Atualizar Infração" : "Criar Infração")
                                     </button>
@@ -127,15 +119,6 @@
     <script src="~/js/jquery.inputmask.js" asp-append-version="true"></script>
 
     <script asp-append-version="true">
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * UPSERT TIPO MULTA - FORMULÁRIO DE CADASTRO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Gerencia formulário de criação / edição de tipos de multa.
-             * @@requires jQuery, InputMask, Syncfusion RichTextEditor
-            * @@file Multa / UpsertTipoMulta.cshtml
-            */
-
         $(document).ready(function () {
             try {
                 $("#txtDenatran").inputmask("999-9");
@@ -144,11 +127,6 @@
             }
         });
 
-            /**
-             * Handler de clique na toolbar do RichTextEditor
-             * @@param { Object } e - Evento de clique com item.id
-            * @@description Configura token XSRF para upload de imagens
-                */
         function toolbarClick(e) {
             try {
                 if (e.item.id == "rte_toolbar_Image") {
```

### REMOVER do Janeiro

```html
                                    <input class="form-control" asp-for="TipoMultaObj.Artigo"
                                        data-ejtip="Artigo do Código de Trânsito" />
                                    <select class="form-control" asp-for="TipoMultaObj.Infracao"
                                        data-ejtip="Gravidade da Infração">
                                    <input id="txtDenatran" class="form-control" asp-for="TipoMultaObj.CodigoDenatran"
                                        data-ejtip="Código Denatran" />
                                    <input class="form-control" asp-for="TipoMultaObj.Desdobramento"
                                        data-ejtip="Desdobramento da Infração" />
                                    <ejs-richtexteditor ejs-for="@Model.TipoMultaObj.Descricao" id="rte"
                                        toolbarClick="toolbarClick" locale="pt-BR" height="200px">
                                        <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage"
                                            path="./DadosEditaveis/ImagensViagens/"></e-richtexteditor-insertimagesettings>
                                        <span class="text-danger"
                                            asp-validation-for="@Model.TipoMultaObj.Descricao"></span>
                                    <button type="submit" value="Submit" asp-page-handler="Submit"
                                        class="btn btn-azul btn-submit-spin px-4">
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * UPSERT TIPO MULTA - FORMULÁRIO DE CADASTRO
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Gerencia formulário de criação / edição de tipos de multa.
             * @@requires jQuery, InputMask, Syncfusion RichTextEditor
            * @@file Multa / UpsertTipoMulta.cshtml
            */
            /**
             * Handler de clique na toolbar do RichTextEditor
             * @@param { Object } e - Evento de clique com item.id
            * @@description Configura token XSRF para upload de imagens
                */
```


### ADICIONAR ao Janeiro

```html
                                    <input class="form-control" asp-for="TipoMultaObj.Artigo" data-ejtip="Artigo do Código de Trânsito" />
                                    <select class="form-control" asp-for="TipoMultaObj.Infracao" data-ejtip="Gravidade da Infração">
                                    <input id="txtDenatran" class="form-control" asp-for="TipoMultaObj.CodigoDenatran" data-ejtip="Código Denatran" />
                                    <input class="form-control" asp-for="TipoMultaObj.Desdobramento" data-ejtip="Desdobramento da Infração" />
                                    <ejs-richtexteditor ejs-for="@Model.TipoMultaObj.Descricao" id="rte" toolbarClick="toolbarClick" locale="pt-BR" height="200px">
                                        <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage" path="./DadosEditaveis/ImagensViagens/"></e-richtexteditor-insertimagesettings>
                                        <span class="text-danger" asp-validation-for="@Model.TipoMultaObj.Descricao"></span>
                                    <button type="submit" value="Submit" asp-page-handler="Submit" class="btn btn-azul btn-submit-spin px-4">
```
