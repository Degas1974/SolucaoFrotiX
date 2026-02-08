# Pages/Abastecimento/PBI.cshtml

**Mudanca:** GRANDE | **+21** linhas | **-89** linhas

---

```diff
--- JANEIRO: Pages/Abastecimento/PBI.cshtml
+++ ATUAL: Pages/Abastecimento/PBI.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @addTagHelper*, Microsoft.AspNetCore.Mvc.TagHelpers
 @using Syncfusion.EJ2
 @model FrotiX.Pages.Viagens.TaxiLegModel
@@ -45,12 +44,6 @@
 }
 
 <script>
-    /**
-     * Impede que a tecla Enter submeta o formulário
-     * @@description Bloqueia submit via Enter exceto em elementos div para permitir comportamento normal em editores
-     * @@param {KeyboardEvent} e - Evento de teclado
-     * @@returns {boolean|void} False para bloquear submit
-     */
     function stopEnterSubmitting(e) {
         if (e.keyCode == 13) {
             var src = e.srcElement || e.target;
@@ -68,21 +61,18 @@
     }
 </script>
 
-<form method="post" asp-action="FichasAntigas" onkeypress='stopEnterSubmitting(window.event)'
-    enctype="multipart/form-data">
+<form method="post" asp-action="FichasAntigas" onkeypress='stopEnterSubmitting(window.event)' enctype="multipart/form-data">
     <div class="row">
         <div class="col-xl-12">
             <div id="panel-1" class="panel">
                 <div class="panel-container show">
                     <div id="divPainel" class="panel-content">
 
-                        <iframe title="Frotix Atualizado - Relatório por Veículo" width="100%" height="900"
-                            src="https://app.powerbi.com/view?r=eyJrIjoiNmVhZjNhYTktZGYwNy00NDdkLTk5NGEtNTc2NzU1OTUzMjEwIiwidCI6IjU2MjFkNjRmLTRjZjgtNDdmNS1iMzc5LTJiMmFiNzljMWM1ZiJ9"
-                            frameborder="0" allowFullScreen="true"></iframe>
-                    </div>
+                        <iframe title="Frotix Atualizado - Relatório por Veículo" width="100%" height="900" src="https://app.powerbi.com/view?r=eyJrIjoiNmVhZjNhYTktZGYwNy00NDdkLTk5NGEtNTc2NzU1OTUzMjEwIiwidCI6IjU2MjFkNjRmLTRjZjgtNDdmNS1iMzc5LTJiMmFiNzljMWM1ZiJ9" frameborder="0" allowFullScreen="true"></iframe>
                 </div>
             </div>
         </div>
+    </div>
 </form>
 
 <div class="modal fade" id="modalRequisitante">
@@ -130,21 +120,15 @@
                         <div id="ControlRegion">
                             <div class="form-control-xs" style="margin: 0 auto; width: 400px;">
                                 <label class="label font-weight-bold">Setor do Requisitante</label>
-                                <ejs-dropdowntree id="ddtSetorRequisitante" placeholder="Selecione um Setor"
-                                    sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
-                                    allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
-                                    <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]"
-                                        value="SetorSolicitanteId" text="Nome" parentValue="SetorPaiId"
-                                        hasChildren="HasChild"></e-dropdowntree-fields>
+                                <ejs-dropdowntree id="ddtSetorRequisitante" placeholder="Selecione um Setor" sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false" allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
+                                    <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]" value="SetorSolicitanteId" text="Nome" parentValue="SetorPaiId" hasChildren="HasChild"></e-dropdowntree-fields>
                                 </ejs-dropdowntree>
                             </div>
                         </div>
                     </div>
                     <div class="modal-footer">
-                        <button id="btnInserirRequisitante" class="btn btn-azul" type="submit" value="SUBMIT">Inserir
-                            Requisitante</button>
-                        <button type="button" class="btn btn-vinho" data-bs-dismiss="modal"><i
-                                class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Fechar</button>
+                        <button id="btnInserirRequisitante" class="btn btn-azul" type="submit" value="SUBMIT">Inserir Requisitante</button>
+                        <button type="button" class="btn btn-vinho" data-bs-dismiss="modal"><i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Fechar</button>
                     </div>
                 </form>
             </div>
@@ -169,22 +153,19 @@
                         <div class="col-3">
                             <div class="form-group">
                                 <label class="label font-weight-bold">Sigla</label>
-                                <input id="txtSigla" class="form-control form-control-xs"
-                                    placeholder="Insira a sigla" />
+                                <input id="txtSigla" class="form-control form-control-xs" placeholder="Insira a sigla" />
                             </div>
                         </div>
                         <div class="col-7">
                             <div class="form-group">
                                 <label class="label font-weight-bold">Nome do Setor</label>
-                                <input id="txtNomeSetor" class="form-control form-control-xs"
-                                    placeholder="Insira o nome do setor" />
+                                <input id="txtNomeSetor" class="form-control form-control-xs" placeholder="Insira o nome do setor" />
                             </div>
                         </div>
                         <div class="col-2">
                             <div class="form-group">
                                 <label class="label font-weight-bold">Ramal</label>
-                                <input id="txtRamalSetor" class="form-control form-control-xs"
-                                    placeholder="Insira o ramal" />
+                                <input id="txtRamalSetor" class="form-control form-control-xs" placeholder="Insira o ramal" />
                             </div>
                         </div>
                     </div>
@@ -193,21 +174,15 @@
                         <div id="ControlRegion">
                             <div class="form-control-xs" style="margin: 0 auto; width: 400px;">
                                 <label class="label font-weight-bold">Setor Pai (se houver)</label>
-                                <ejs-dropdowntree id="ddtSetorPai" placeholder="Selecione um Setor"
-                                    sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
-                                    allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
-                                    <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]"
-                                        value="SetorSolicitanteId" text="Nome" parentValue="SetorPaiId"
-                                        hasChildren="HasChild"></e-dropdowntree-fields>
+                                <ejs-dropdowntree id="ddtSetorPai" placeholder="Selecione um Setor" sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false" allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
+                                    <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]" value="SetorSolicitanteId" text="Nome" parentValue="SetorPaiId" hasChildren="HasChild"></e-dropdowntree-fields>
                                 </ejs-dropdowntree>
                             </div>
                         </div>
                     </div>
                     <div class="modal-footer">
-                        <button id="btnInserirSetor" class="btn btn-azul" type="submit" value="SUBMIT">Inserir
-                            Setor</button>
-                        <button type="button" class="btn btn-vinho" data-bs-dismiss="modal"><i
-                                class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Fechar</button>
+                        <button id="btnInserirSetor" class="btn btn-azul" type="submit" value="SUBMIT">Inserir Setor</button>
+                        <button type="button" class="btn btn-vinho" data-bs-dismiss="modal"><i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Fechar</button>
                     </div>
                 </form>
             </div>
@@ -227,7 +202,8 @@
                 ddtCombustivelInicial.value = '@Model.ViagemObj.Viagem.CombustivelInicial';
             }
 
-            if ('@Model.ViagemObj.Viagem.ViagemId' === '00000000-0000-0000-0000-000000000000') {
+            if ('@Model.ViagemObj.Viagem.ViagemId' === '00000000-0000-0000-0000-000000000000')
+            {
                 document.getElementById("txtDataInicial").value = moment(Date()).format("YYYY-MM-DD");
 
             }
@@ -425,7 +401,8 @@
 
         $(document).ready(function () {
 
-            if ('@Model.ViagemObj.Viagem.ViagemId' === '00000000-0000-0000-0000-000000000000') {
+            if ('@Model.ViagemObj.Viagem.ViagemId' === '00000000-0000-0000-0000-000000000000')
+            {
                 document.getElementById("txtDataInicial").value = moment(Date()).format("YYYY-MM-DD");
                 document.getElementById("txtHoraInicial").value = moment(Date()).format("HH:mm");
                 document.getElementById("divFicha").style.display = 'none';
@@ -697,12 +674,6 @@
             }
         });
 
-        /**
-         * Trata clique na toolbar do RichTextEditor
-         * @@description Configura header de XSRF-Token para upload de imagem
-         * @@param {Object} e - Evento de clique da toolbar
-         * @@returns {void}
-         */
         function toolbarClick(e) {
             if (e.item.id == "rte_toolbar_Image") {
                 var element = document.getElementById('rte_upload')
@@ -754,11 +725,6 @@
 
     <script type="text/javascript">
 
-        /**
-         * Recarrega lista de requisitantes no dropdown após inserção
-         * @@description Busca requisitantes via AJAX e atualiza datasource do componente Syncfusion
-         * @@returns {void}
-         */
         function PreencheListaRequisitantes() {
 
             $.ajax({
@@ -793,11 +759,6 @@
             document.getElementById("ddtRequisitante").ej2_instances[0].refresh();
         }
 
-        /**
-         * Recarrega lista de setores no dropdown após inserção
-         * @@description Busca setores via AJAX e atualiza datasource do componente Syncfusion TreeView
-         * @@returns {void}
-         */
         function PreencheListaSetores() {
 
             $.ajax({
@@ -835,11 +796,6 @@
             document.getElementById("ddtSetor").ej2_instances[0].refresh();
         }
 
-        /**
-         * Handler de mudança de requisitante selecionado
-         * @@description Busca setor padrão e ramal do requisitante via AJAX
-         * @@returns {void}
-         */
         function RequisitanteValueChange() {
 
             var ddTreeObj = document.getElementById("ddtRequisitante").ej2_instances[0];
@@ -873,11 +829,6 @@
             })
         }
 
-        /**
-         * Handler de mudança de motorista selecionado
-         * @@description Verifica via AJAX se motorista está em viagem ativa e exibe alerta
-         * @@returns {void}
-         */
         function MotoristaValueChange() {
 
             var ddTreeObj = document.getElementById("lstMotorista").ej2_instances[0];
@@ -917,11 +868,6 @@
             })
         }
 
-        /**
-         * Handler de mudança de veículo selecionado
-         * @@description Verifica via AJAX se veículo está em viagem e pega KM atual
-         * @@returns {void}
-         */
         function VeiculoValueChange() {
 
             var ddTreeObj = document.getElementById("lstVeiculo").ej2_instances[0];
@@ -971,30 +917,14 @@
 
         }
 
-        /**
-         * Handler genérico de mudança de valor (stub)
-         * @@description Placeholder para processamento de mudança de dropdown tree
-         * @@returns {void}
-         */
         function valueChange() {
 
         }
 
-        /**
-         * Handler de seleção de item (stub)
-         * @@description Placeholder para processamento de seleção em dropdown tree
-         * @@param {Object} args - Argumentos do evento de seleção
-         * @@returns {void}
-         */
         function select(args) {
 
         }
 
-        /**
-         * Handler de mudança de combustível (stub)
-         * @@description Placeholder para processamento de mudança de tipo de combustível
-         * @@returns {void}
-         */
         function ddtCombustivelChange() {
 
         }
@@ -1122,7 +1052,7 @@
 
             setorPaiId = null;
 
-            if (document.getElementById('ddtSetorPai').ej2_instances[0].value != '' && document.getElementById('ddtSetorPai').ej2_instances[0].value != null) {
+            if (document.getElementById('ddtSetorPai').ej2_instances[0].value != '' && document.getElementById('ddtSetorPai').ej2_instances[0].value != null ) {
                 setorPaiId = document.getElementById('ddtSetorPai').ej2_instances[0].value.toString();
             }
 
@@ -1322,7 +1252,8 @@
                         if (data.fichaVistoria != null) {
                             $('#imgViewer').attr('src', "data:image/jpg;base64," + data.fichaVistoria + "");
                         }
-                        else {
+                        else
+                        {
                             $('#imgViewer').attr('src', "/Images/FichaAmarelaNova.jpg");
                         }
 
```
