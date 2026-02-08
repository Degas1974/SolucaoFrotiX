# Pages/Viagens/TaxiLeg.cshtml

**Mudanca:** GRANDE | **+469** linhas | **-516** linhas

---

```diff
--- JANEIRO: Pages/Viagens/TaxiLeg.cshtml
+++ ATUAL: Pages/Viagens/TaxiLeg.cshtml
@@ -44,21 +44,6 @@
 }
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * TAXI LEG - FORMULÁRIO DE VIAGEM EXTERNA
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Funções para formulário de cadastro de viagens TaxiLeg,
-     * incluindo validação e prevenção de submit via Enter.
-     * @@requires jQuery, Syncfusion EJ2 ComboBox / DropDownTree
-        * @@file Viagens / TaxiLeg.cshtml
-        */
-
-    /**
-     * Previne submit do formulário ao pressionar Enter
-     * @@param { KeyboardEvent } e - Evento de teclado
-        * @@returns { boolean| undefined} False para prevenir submit
-            */
     function stopEnterSubmitting(e) {
         try {
             if (e.key === 'Enter') {
@@ -176,12 +161,19 @@
                         <div id="ControlRegion">
                             <div class="form-control-xs" style="margin: 0 auto; width: 400px;">
                                 <label class="label font-weight-bold">Setor do Requisitante</label>
-                                <ejs-dropdowntree id="ddtSetorRequisitante" placeholder="Selecione um Setor"
-                                    sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
-                                    allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
+                                <ejs-dropdowntree id="ddtSetorRequisitante"
+                                                  placeholder="Selecione um Setor"
+                                                  sortOrder="Ascending"
+                                                  showCheckBox="false"
+                                                  allowMultiSelection="false"
+                                                  allowFiltering="true"
+                                                  filterType="Contains"
+                                                  filterBarPlaceholder="Procurar...">
                                     <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]"
-                                        value="SetorSolicitanteId" text="Nome" parentValue="SetorPaiId"
-                                        hasChildren="HasChild">
+                                                           value="SetorSolicitanteId"
+                                                           text="Nome"
+                                                           parentValue="SetorPaiId"
+                                                           hasChildren="HasChild">
                                     </e-dropdowntree-fields>
                                 </ejs-dropdowntree>
                             </div>
@@ -215,22 +207,19 @@
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
@@ -239,12 +228,19 @@
                         <div id="ControlRegion">
                             <div class="form-control-xs" style="margin: 0 auto; width: 400px;">
                                 <label class="label font-weight-bold">Setor Pai (se houver)</label>
-                                <ejs-dropdowntree id="ddtSetorPai" placeholder="Selecione um Setor"
-                                    sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
-                                    allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
+                                <ejs-dropdowntree id="ddtSetorPai"
+                                                  placeholder="Selecione um Setor"
+                                                  sortOrder="Ascending"
+                                                  showCheckBox="false"
+                                                  allowMultiSelection="false"
+                                                  allowFiltering="true"
+                                                  filterType="Contains"
+                                                  filterBarPlaceholder="Procurar...">
                                     <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]"
-                                        value="SetorSolicitanteId" text="Nome" parentValue="SetorPaiId"
-                                        hasChildren="HasChild">
+                                                           value="SetorSolicitanteId"
+                                                           text="Nome"
+                                                           parentValue="SetorPaiId"
+                                                           hasChildren="HasChild">
                                     </e-dropdowntree-fields>
                                 </ejs-dropdowntree>
                             </div>
@@ -621,11 +617,6 @@
             }
         });
 
-                /**
-                 * Handler de clique na toolbar do RichTextEditor
-                 * @@param { Object } e - Evento de clique contendo info do item
-            * @@description Configura upload de imagem com token XSRF
-                */
         function toolbarClick(e) {
             try {
                 if (e.item.id === "rte_toolbar_Image") {
@@ -676,17 +667,7 @@
     </script>
 
     <script type="text/javascript">
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * PREENCHIMENTO DINÂMICO DE LISTAS
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Funções para atualização de listas após inserção de novos registros.
-             */
-
-            /**
-             * Atualiza a lista de requisitantes após inserção de novo registro
-             * @@description Recarrega dados via AJAX e atualiza o DropDownTree
-            */
+
         function PreencheListaRequisitantes() {
             try {
                 $.ajax({
@@ -713,523 +694,495 @@
                 Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "PreencheListaRequisitantes", error);
             }
         }
-            /**
-             * Atualiza a lista de setores após inserção de novo registro
-             * @@description Recarrega dados hierárquicos via AJAX e atualiza o DropDownTree
-            */
-        function PreencheListaSetores() {
-            try {
-                $.ajax({
-                    url: "/Viagens/Upsert?handler=AJAXPreencheListaSetores",
-                    method: "GET",
-                    datatype: "json",
-                    success: function (res) {
-                        var setorSolicitanteId = res.data[0].setorSolicitanteId;
-                        var setorPaiId = res.data[0].setorPaiId;
-                        var nome = res.data[0].nome;
-                        var hasChild = res.data[0].hasChild;
-
-                        let SetorList = [{
+
+    function PreencheListaSetores() {
+        try {
+            $.ajax({
+                url: "/Viagens/Upsert?handler=AJAXPreencheListaSetores",
+                method: "GET",
+                datatype: "json",
+                success: function (res) {
+                    var setorSolicitanteId = res.data[0].setorSolicitanteId;
+                    var setorPaiId = res.data[0].setorPaiId;
+                    var nome = res.data[0].nome;
+                    var hasChild = res.data[0].hasChild;
+
+                    let SetorList = [{
+                        "SetorSolicitanteId": setorSolicitanteId,
+                        "SetorPaiId": setorPaiId,
+                        "Nome": nome,
+                        "HasChild": hasChild
+                    }];
+
+                    for (var i = 1; i < res.data.length; ++i) {
+                        setorSolicitanteId = res.data[i].setorSolicitanteId;
+                        setorPaiId = res.data[i].setorPaiId;
+                        nome = res.data[i].nome;
+                        hasChild = res.data[i].hasChild;
+                        let setor = {
                             "SetorSolicitanteId": setorSolicitanteId,
                             "SetorPaiId": setorPaiId,
                             "Nome": nome,
                             "HasChild": hasChild
-                        }];
-
-                        for (var i = 1; i < res.data.length; ++i) {
-                            setorSolicitanteId = res.data[i].setorSolicitanteId;
-                            setorPaiId = res.data[i].setorPaiId;
-                            nome = res.data[i].nome;
-                            hasChild = res.data[i].hasChild;
-                            let setor = {
-                                "SetorSolicitanteId": setorSolicitanteId,
-                                "SetorPaiId": setorPaiId,
-                                "Nome": nome,
-                                "HasChild": hasChild
-                            };
-                            SetorList.push(setor);
+                        };
+                        SetorList.push(setor);
+                    }
+
+                    document.getElementById("ddtSetor").ej2_instances[0].fields.dataSource = SetorList;
+                    document.getElementById("ddtSetor").ej2_instances[0].refresh();
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "PreencheListaSetores", error);
+        }
+    }
+
+    function RequisitanteValueChange() {
+        try {
+            var ddTreeObj = document.getElementById("ddtRequisitante").ej2_instances[0];
+            if (ddTreeObj.value === null) return;
+
+            var requisitanteid = String(ddTreeObj.value);
+
+            $.ajax({
+                url: "/Viagens/Upsert?handler=PegaSetor",
+                method: "GET",
+                datatype: "json",
+                data: { id: requisitanteid },
+                success: function (res) {
+                    document.getElementById("ddtSetor").ej2_instances[0].value = [res.data];
+                }
+            });
+
+            $.ajax({
+                url: "/Viagens/Upsert?handler=PegaRamal",
+                method: "GET",
+                datatype: "json",
+                data: { id: requisitanteid },
+                success: function (res) {
+                    var s = document.getElementById("txtRamalRequisitante");
+                    s.value = res.data;
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "RequisitanteValueChange", error);
+        }
+    }
+
+    function MotoristaValueChange() {
+        try {
+            var ddTreeObj = document.getElementById("lstMotorista").ej2_instances[0];
+            if (ddTreeObj.value === null) return;
+
+            var motoristaid = String(ddTreeObj.value);
+
+            $.ajax({
+                url: "/Viagens/Upsert?handler=VerificaMotoristaViagem",
+                method: "GET",
+                datatype: "json",
+                data: { id: motoristaid },
+                success: function (res) {
+                    if (res.data) {
+                        swal({
+                            title: "Motorista em Viagem",
+                            text: "Este motorista encontra-se em uma viagem não terminada!",
+                            icon: "warning",
+                            buttons: { ok: "Ok" }
+                        });
+                    }
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "MotoristaValueChange", error);
+        }
+    }
+
+    function VeiculoValueChange() {
+        try {
+            var ddTreeObj = document.getElementById("lstVeiculo").ej2_instances[0];
+            if (ddTreeObj.value === null) return;
+
+            var veiculoid = String(ddTreeObj.value);
+
+            $.ajax({
+                url: "/Viagens/Upsert?handler=VerificaVeiculoViagem",
+                method: "GET",
+                datatype: "json",
+                data: { id: veiculoid },
+                success: function (res) {
+                    if (res.data) {
+                        swal({
+                            title: "Veículo em Viagem",
+                            text: "Este veículo encontra-se em uma viagem não terminada!",
+                            icon: "warning",
+                            buttons: { ok: "Ok" }
+                        });
+                    }
+                }
+            });
+
+            $.ajax({
+                url: "/Viagens/Upsert?handler=PegaKmAtualVeiculo",
+                method: "GET",
+                datatype: "json",
+                data: { id: veiculoid },
+                success: function (res) {
+                    var kmAtual = document.getElementById("txtKmAtual");
+                    kmAtual.value = res.data;
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "VeiculoValueChange", error);
+        }
+    }
+
+    function valueChange() {
+        try {
+
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "valueChange", error);
+        }
+    }
+
+    function select(args) {
+        try {
+
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "select", error);
+        }
+    }
+
+    function ddtCombustivelChange() {
+        try {
+
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "ddtCombustivelChange", error);
+        }
+    }
+</script>
+
+<script>
+
+    $("#btnInserirRequisitante").click(function (e) {
+        try {
+            e.preventDefault();
+
+            if ($("#txtPonto").val() === "") {
+                swal({
+                    title: "Atenção",
+                    text: "O Ponto do Requisitante é obrigatório!",
+                    icon: "error",
+                    buttons: { close: "Fechar" }
+                });
+                return;
+            }
+
+            if ($("#txtNome").val() === "") {
+                swal({
+                    title: "Atenção",
+                    text: "O Nome do Requisitante é obrigatório!",
+                    icon: "error",
+                    buttons: { close: "Fechar" }
+                });
+                return;
+            }
+
+            if ($("#txtRamal").val() === "") {
+                swal({
+                    title: "Atenção",
+                    text: "O Ramal do Requisitante é obrigatório!",
+                    icon: "error",
+                    buttons: { close: "Fechar" }
+                });
+                return;
+            }
+
+            var setores = document.getElementById('ddtSetorRequisitante').ej2_instances[0];
+            if (setores.value === null) {
+                swal({
+                    title: "Atenção",
+                    text: "O Setor do Requisitante é obrigatório!",
+                    icon: "error",
+                    buttons: { close: "Fechar" }
+                });
+                return;
+            }
+
+            var setorSolicitanteId = setores.value.toString();
+            var objRequisitante = JSON.stringify({
+                "Nome": $('#txtNome').val(),
+                "Ponto": $('#txtPonto').val(),
+                "Ramal": $('#txtRamal').val(),
+                "Email": $('#txtEmail').val(),
+                "SetorSolicitanteId": setorSolicitanteId
+            });
+
+            $.ajax({
+                type: "post",
+                url: "/api/Viagem/AdicionarRequisitante",
+                contentType: "application/json; charset=utf-8",
+                dataType: "json",
+                data: objRequisitante,
+                beforeSend: function (xhr) {
+                    xhr.setRequestHeader('RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());
+                },
+                success: function (data) {
+                    AppToast.show('Verde', data.message);
+                    PreencheListaRequisitantes();
+                    $("#modalRequisitante").modal('hide');
+                },
+                error: function (data) {
+                    console.log(data);
+                    alert('error');
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "#btnInserirRequisitante.click", error);
+        }
+    });
+
+    $("#btnInserirSetor").click(function (e) {
+        try {
+            e.preventDefault();
+
+            if ($("#txtNomeSetor").val() === "") {
+                swal({
+                    title: "Atenção",
+                    text: "O Nome do Setor é obrigatório!",
+                    icon: "error",
+                    buttons: { close: "Fechar" }
+                });
+                return;
+            }
+
+            if ($("#txtRamalSetor").val() === "") {
+                swal({
+                    title: "Atenção",
+                    text: "O Ramal do Setor é obrigatório!",
+                    icon: "error",
+                    buttons: { close: "Fechar" }
+                });
+                return;
+            }
+
+            var setorPaiId = null;
+            var ddtPai = document.getElementById('ddtSetorPai').ej2_instances[0];
+
+            if (ddtPai.value !== '' && ddtPai.value !== null) {
+                setorPaiId = ddtPai.value.toString();
+            }
+
+            var objSetor = JSON.stringify({
+                "Nome": $('#txtNomeSetor').val(),
+                "Ramal": $('#txtRamalSetor').val(),
+                "Sigla": $('#txtSigla').val(),
+                "SetorPaiId": setorPaiId
+            });
+
+            $.ajax({
+                type: "post",
+                url: "/api/Viagem/AdicionarSetor",
+                contentType: "application/json; charset=utf-8",
+                dataType: "json",
+                data: objSetor,
+                beforeSend: function (xhr) {
+                    xhr.setRequestHeader('RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());
+                },
+                success: function (data) {
+                    AppToast.show('Verde', data.message);
+                    PreencheListaSetores();
+                    $("#modalSetor").modal('hide');
+                },
+                error: function (data) {
+                    console.log(data);
+                    alert('error');
+                }
+            });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "#btnInserirSetor.click", error);
+        }
+    });
+
+    ej.base.L10n.load({
+        "pt-BR": {
+            "richtexteditor": {
+                "alignments": "Alinhamentos",
+                "justifyLeft": "Alinhar à Esquerda",
+                "justifyCenter": "Centralizar",
+                "justifyRight": "Alinhar à Direita",
+                "justifyFull": "Justificar",
+                "fontName": "Nome da Fonte",
+                "fontSize": "Tamanho da Fonte",
+                "fontColor": "Cor da Fonte",
+                "backgroundColor": "Cor de Fundo",
+                "bold": "Negrito",
+                "italic": "Itálico",
+                "underline": "Sublinhado",
+                "strikethrough": "Tachado",
+                "clearFormat": "Limpa Formatação",
+                "clearAll": "Limpa Tudo",
+                "cut": "Cortar",
+                "copy": "Copiar",
+                "paste": "Colar",
+                "unorderedList": "Lista com Marcadores",
+                "orderedList": "Lista Numerada",
+                "indent": "Aumentar Identação",
+                "outdent": "Diminuir Identação",
+                "undo": "Desfazer",
+                "redo": "Refazer",
+                "superscript": "Sobrescrito",
+                "subscript": "Subscrito",
+                "createLink": "Inserir Link",
+                "openLink": "Abrir Link",
+                "editLink": "Editar Link",
+                "removeLink": "Remover Link",
+                "image": "Inserir Imagem",
+                "replace": "Substituir",
+                "align": "Alinhar",
+                "caption": "Título da Imagem",
+                "remove": "Remover",
+                "insertLink": "Inserir Link",
+                "display": "Exibir",
+                "altText": "Texto Alternativo",
+                "dimension": "Mudar Tamanho",
+                "fullscreen": "Maximizar",
+                "maximize": "Maximizar",
+                "minimize": "Minimizar",
+                "lowerCase": "Caixa Baixa",
+                "upperCase": "Caixa Alta",
+                "print": "Imprimir",
+                "formats": "Formatos",
+                "sourcecode": "Visualizar Código",
+                "preview": "Exibir",
+                "viewside": "ViewSide",
+                "insertCode": "Inserir Código",
+                "linkText": "Exibir Texto",
+                "linkTooltipLabel": "Título",
+                "linkWebUrl": "Endereço Web",
+                "linkTitle": "Entre com um título",
+                "linkurl": "http://exemplo.com",
+                "linkOpenInNewWindow": "Abrir Link em Nova Janela",
+                "linkHeader": "Inserir Link",
+                "dialogInsert": "Inserir",
+                "dialogCancel": "Cancelar",
+                "dialogUpdate": "Atualizar",
+                "imageHeader": "Inserir Imagem",
+                "imageLinkHeader": "Você pode proporcionar um link da web",
+                "mdimageLink": "Favor proporcionar uma URL para sua imagem",
+                "imageUploadMessage": "Solte a imagem aqui ou busque para o upload",
+                "imageDeviceUploadMessage": "Clique aqui para o upload",
+                "imageAlternateText": "Texto Alternativo",
+                "alternateHeader": "Texto Alternativo",
+                "browse": "Procurar",
+                "imageUrl": "http://exemplo.com/imagem.png",
+                "imageCaption": "Título",
+                "imageSizeHeader": "Tamanho da Imagem",
+                "imageHeight": "Altura",
+                "imageWidth": "Largura",
+                "textPlaceholder": "Entre com um Texto",
+                "inserttablebtn": "Inserir Tabela",
+                "tabledialogHeader": "Inserir Tabela",
+                "tableWidth": "Largura",
+                "cellpadding": "Espaçamento de célula",
+                "cellspacing": "Espaçamento de célula",
+                "columns": "Número de colunas",
+                "rows": "Número de linhas",
+                "tableRows": "Linhas da Tabela",
+                "tableColumns": "Colunas da Tabela",
+                "tableCellHorizontalAlign": "Alinhamento Horizontal da Célular",
+                "tableCellVerticalAlign": "Alinhamento Vertical da Célular",
+                "createTable": "Criar Tabela",
+                "removeTable": "Remover Tabela",
+                "tableHeader": "Cabeçalho da Tabela",
+                "tableRemove": "Remover Tabela",
+                "tableCellBackground": "Fundo da Célula",
+                "tableEditProperties": "Editar Propriedades da Tabela",
+                "styles": "Estilos",
+                "insertColumnLeft": "Inserir Coluna à Esquerda",
+                "insertColumnRight": "Inserir Coluna à Direita",
+                "deleteColumn": "Apagar Coluna",
+                "insertRowBefore": "Inserir Linha Antes",
+                "insertRowAfter": "Inserir Linha Depois",
+                "deleteRow": "Apagar Linha",
+                "tableEditHeader": "Edita Tabela",
+                "TableHeadingText": "Cabeçãlho",
+                "TableColText": "Col",
+                "imageInsertLinkHeader": "Inserir Link",
+                "editImageHeader": "Edita Imagem",
+                "alignmentsDropDownLeft": "Alinhar à Esquerda",
+                "alignmentsDropDownCenter": "Centralizar",
+                "alignmentsDropDownRight": "Alinhar à Direita",
+                "alignmentsDropDownJustify": "Justificar",
+                "imageDisplayDropDownInline": "Inline",
+                "imageDisplayDropDownBreak": "Break",
+                "tableInsertRowDropDownBefore": "Inserir linha antes",
+                "tableInsertRowDropDownAfter": "Inserir linha depois",
+                "tableInsertRowDropDownDelete": "Apagar linha",
+                "tableInsertColumnDropDownLeft": "Inserir coluna à esquerda",
+                "tableInsertColumnDropDownRight": "Inserir coluna à direita",
+                "tableInsertColumnDropDownDelete": "Apagar Coluna",
+                "tableVerticalAlignDropDownTop": "Alinhar no Topo",
+                "tableVerticalAlignDropDownMiddle": "Alinhar no Meio",
+                "tableVerticalAlignDropDownBottom": "Alinhar no Fundo",
+                "tableStylesDropDownDashedBorder": "Bordas Pontilhadas",
+                "tableStylesDropDownAlternateRows": "Linhas Alternadas",
+                "pasteFormat": "Colar Formato",
+                "pasteFormatContent": "Escolha a ação de formatação",
+                "plainText": "Texto Simples",
+                "cleanFormat": "Limpar",
+                "keepFormat": "Manter",
+                "formatsDropDownParagraph": "Parágrafo",
+                "formatsDropDownCode": "Código",
+                "formatsDropDownQuotation": "Citação",
+                "formatsDropDownHeading1": "Cabeçalho 1",
+                "formatsDropDownHeading2": "Cabeçalho 2",
+                "formatsDropDownHeading3": "Cabeçalho 3",
+                "formatsDropDownHeading4": "Cabeçalho 4",
+                "fontNameSegoeUI": "SegoeUI",
+                "fontNameArial": "Arial",
+                "fontNameGeorgia": "Georgia",
+                "fontNameImpact": "Impact",
+                "fontNameTahoma": "Tahoma",
+                "fontNameTimesNewRoman": "Times New Roman",
+                "fontNameVerdana": "Verdana"
+            }
+        }
+    });
+
+    $("#txtFile").change(function (event) {
+        try {
+            var files = event.target.files;
+            $("#imgViewer").attr("src", window.URL.createObjectURL(files[0]));
+            $("#painelfundo").css({ "padding-bottom:": "200px" });
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "#txtFile.change", error);
+        }
+    });
+
+    $(document).ready(function () {
+        try {
+            if ('@Model.ViagemObj.Viagem.ViagemId' != '00000000-0000-0000-0000-000000000000') {
+                $.ajax({
+                    type: "GET",
+                    url: "/api/Viagem/PegaFicha",
+                    success: function (data) {
+                        if (data.fichaVistoria != null) {
+                            $('#imgViewer').attr('src', "data:image/jpg;base64," + data.fichaVistoria);
+                        } else {
+                            $('#imgViewer').attr('src', "/Images/FichaAmarelaNova.jpg");
                         }
-
-                        document.getElementById("ddtSetor").ej2_instances[0].fields.dataSource = SetorList;
-                        document.getElementById("ddtSetor").ej2_instances[0].refresh();
-                    }
-                });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "PreencheListaSetores", error);
-            }
-        }
-
-            /**
-             * Callback de mudança no DropDownTree de requisitantes
-             * @@description Busca setor e ramal padrão do requisitante selecionado
-            */
-            function RequisitanteValueChange() {
-            try {
-                var ddTreeObj = document.getElementById("ddtRequisitante").ej2_instances[0];
-                if (ddTreeObj.value === null) return;
-
-                var requisitanteid = String(ddTreeObj.value);
-
-                $.ajax({
-                    url: "/Viagens/Upsert?handler=PegaSetor",
-                    method: "GET",
-                    datatype: "json",
-                    data: { id: requisitanteid },
-                    success: function (res) {
-                        document.getElementById("ddtSetor").ej2_instances[0].value = [res.data];
-                    }
-                });
-
-                $.ajax({
-                    url: "/Viagens/Upsert?handler=PegaRamal",
-                    method: "GET",
-                    datatype: "json",
-                    data: { id: requisitanteid },
-                    success: function (res) {
-                        var s = document.getElementById("txtRamalRequisitante");
-                        s.value = res.data;
-                    }
-                });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "RequisitanteValueChange", error);
-            }
-        }
-
-            /**
-             * Callback de mudança no ComboBox de motoristas
-             * @@description Verifica se motorista está em viagem ativa
-            */
-        function MotoristaValueChange() {
-            try {
-                var ddTreeObj = document.getElementById("lstMotorista").ej2_instances[0];
-                if (ddTreeObj.value === null) return;
-
-                var motoristaid = String(ddTreeObj.value);
-
-                $.ajax({
-                    url: "/Viagens/Upsert?handler=VerificaMotoristaViagem",
-                    method: "GET",
-                    datatype: "json",
-                    data: { id: motoristaid },
-                    success: function (res) {
-                        if (res.data) {
-                            swal({
-                                title: "Motorista em Viagem",
-                                text: "Este motorista encontra-se em uma viagem não terminada!",
-                                icon: "warning",
-                                buttons: { ok: "Ok" }
-                            });
-                        }
-                    }
-                });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "MotoristaValueChange", error);
-            }
-        }
-
-            /**
-             * Callback de mudança no ComboBox de veículos
-             * @@description Verifica se veículo está em viagem ativa e busca km atual
-            */
-        function VeiculoValueChange() {
-            try {
-                var ddTreeObj = document.getElementById("lstVeiculo").ej2_instances[0];
-                if (ddTreeObj.value === null) return;
-
-                var veiculoid = String(ddTreeObj.value);
-
-                $.ajax({
-                    url: "/Viagens/Upsert?handler=VerificaVeiculoViagem",
-                    method: "GET",
-                    datatype: "json",
-                    data: { id: veiculoid },
-                    success: function (res) {
-                        if (res.data) {
-                            swal({
-                                title: "Veículo em Viagem",
-                                text: "Este veículo encontra-se em uma viagem não terminada!",
-                                icon: "warning",
-                                buttons: { ok: "Ok" }
-                            });
-                        }
-                    }
-                });
-
-                $.ajax({
-                    url: "/Viagens/Upsert?handler=PegaKmAtualVeiculo",
-                    method: "GET",
-                    datatype: "json",
-                    data: { id: veiculoid },
-                    success: function (res) {
-                        var kmAtual = document.getElementById("txtKmAtual");
-                        kmAtual.value = res.data;
-                    }
-                });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "VeiculoValueChange", error);
-            }
-        }
-
-            /**
-             * Callback genérico de mudança de valor (placeholder)
-             * @@description Reservado para regras futuras
-            */
-        function valueChange() {
-            try {
-
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "valueChange", error);
-            }
-        }
-
-            /**
-             * Callback de seleção de item (placeholder)
-             * @@param { Object } args - Argumentos do evento de seleção
-            * @@description Reservado para regras futuras
-                */
-        function select(args) {
-            try {
-
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "select", error);
-            }
-        }
-
-            /**
-             * Callback de mudança no ComboBox de combustível (placeholder)
-             * @@description Reservado para regras futuras
-            */
-        function ddtCombustivelChange() {
-            try {
-
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "ddtCombustivelChange", error);
-            }
-        }
-    </script>
-
-    <script>
-
-        $("#btnInserirRequisitante").click(function (e) {
-            try {
-                e.preventDefault();
-
-                if ($("#txtPonto").val() === "") {
-                    swal({
-                        title: "Atenção",
-                        text: "O Ponto do Requisitante é obrigatório!",
-                        icon: "error",
-                        buttons: { close: "Fechar" }
-                    });
-                    return;
-                }
-
-                if ($("#txtNome").val() === "") {
-                    swal({
-                        title: "Atenção",
-                        text: "O Nome do Requisitante é obrigatório!",
-                        icon: "error",
-                        buttons: { close: "Fechar" }
-                    });
-                    return;
-                }
-
-                if ($("#txtRamal").val() === "") {
-                    swal({
-                        title: "Atenção",
-                        text: "O Ramal do Requisitante é obrigatório!",
-                        icon: "error",
-                        buttons: { close: "Fechar" }
-                    });
-                    return;
-                }
-
-                var setores = document.getElementById('ddtSetorRequisitante').ej2_instances[0];
-                if (setores.value === null) {
-                    swal({
-                        title: "Atenção",
-                        text: "O Setor do Requisitante é obrigatório!",
-                        icon: "error",
-                        buttons: { close: "Fechar" }
-                    });
-                    return;
-                }
-
-                var setorSolicitanteId = setores.value.toString();
-                var objRequisitante = JSON.stringify({
-                    "Nome": $('#txtNome').val(),
-                    "Ponto": $('#txtPonto').val(),
-                    "Ramal": $('#txtRamal').val(),
-                    "Email": $('#txtEmail').val(),
-                    "SetorSolicitanteId": setorSolicitanteId
-                });
-
-                $.ajax({
-                    type: "post",
-                    url: "/api/Viagem/AdicionarRequisitante",
-                    contentType: "application/json; charset=utf-8",
-                    dataType: "json",
-                    data: objRequisitante,
-                    beforeSend: function (xhr) {
-                        xhr.setRequestHeader('RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());
-                    },
-                    success: function (data) {
-                        AppToast.show('Verde', data.message);
-                        PreencheListaRequisitantes();
-                        $("#modalRequisitante").modal('hide');
                     },
                     error: function (data) {
-                        console.log(data);
-                        alert('error');
+                        console.log('Error:', data);
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "#btnInserirRequisitante.click", error);
-            }
-        });
-
-        $("#btnInserirSetor").click(function (e) {
-            try {
-                e.preventDefault();
-
-                if ($("#txtNomeSetor").val() === "") {
-                    swal({
-                        title: "Atenção",
-                        text: "O Nome do Setor é obrigatório!",
-                        icon: "error",
-                        buttons: { close: "Fechar" }
-                    });
-                    return;
-                }
-
-                if ($("#txtRamalSetor").val() === "") {
-                    swal({
-                        title: "Atenção",
-                        text: "O Ramal do Setor é obrigatório!",
-                        icon: "error",
-                        buttons: { close: "Fechar" }
-                    });
-                    return;
-                }
-
-                var setorPaiId = null;
-                var ddtPai = document.getElementById('ddtSetorPai').ej2_instances[0];
-
-                if (ddtPai.value !== '' && ddtPai.value !== null) {
-                    setorPaiId = ddtPai.value.toString();
-                }
-
-                var objSetor = JSON.stringify({
-                    "Nome": $('#txtNomeSetor').val(),
-                    "Ramal": $('#txtRamalSetor').val(),
-                    "Sigla": $('#txtSigla').val(),
-                    "SetorPaiId": setorPaiId
-                });
-
-                $.ajax({
-                    type: "post",
-                    url: "/api/Viagem/AdicionarSetor",
-                    contentType: "application/json; charset=utf-8",
-                    dataType: "json",
-                    data: objSetor,
-                    beforeSend: function (xhr) {
-                        xhr.setRequestHeader('RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());
-                    },
-                    success: function (data) {
-                        AppToast.show('Verde', data.message);
-                        PreencheListaSetores();
-                        $("#modalSetor").modal('hide');
-                    },
-                    error: function (data) {
-                        console.log(data);
-                        alert('error');
-                    }
-                });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "#btnInserirSetor.click", error);
-            }
-        });
-
-        ej.base.L10n.load({
-            "pt-BR": {
-                "richtexteditor": {
-                    "alignments": "Alinhamentos",
-                    "justifyLeft": "Alinhar à Esquerda",
-                    "justifyCenter": "Centralizar",
-                    "justifyRight": "Alinhar à Direita",
-                    "justifyFull": "Justificar",
-                    "fontName": "Nome da Fonte",
-                    "fontSize": "Tamanho da Fonte",
-                    "fontColor": "Cor da Fonte",
-                    "backgroundColor": "Cor de Fundo",
-                    "bold": "Negrito",
-                    "italic": "Itálico",
-                    "underline": "Sublinhado",
-                    "strikethrough": "Tachado",
-                    "clearFormat": "Limpa Formatação",
-                    "clearAll": "Limpa Tudo",
-                    "cut": "Cortar",
-                    "copy": "Copiar",
-                    "paste": "Colar",
-                    "unorderedList": "Lista com Marcadores",
-                    "orderedList": "Lista Numerada",
-                    "indent": "Aumentar Identação",
-                    "outdent": "Diminuir Identação",
-                    "undo": "Desfazer",
-                    "redo": "Refazer",
-                    "superscript": "Sobrescrito",
-                    "subscript": "Subscrito",
-                    "createLink": "Inserir Link",
-                    "openLink": "Abrir Link",
-                    "editLink": "Editar Link",
-                    "removeLink": "Remover Link",
-                    "image": "Inserir Imagem",
-                    "replace": "Substituir",
-                    "align": "Alinhar",
-                    "caption": "Título da Imagem",
-                    "remove": "Remover",
-                    "insertLink": "Inserir Link",
-                    "display": "Exibir",
-                    "altText": "Texto Alternativo",
-                    "dimension": "Mudar Tamanho",
-                    "fullscreen": "Maximizar",
-                    "maximize": "Maximizar",
-                    "minimize": "Minimizar",
-                    "lowerCase": "Caixa Baixa",
-                    "upperCase": "Caixa Alta",
-                    "print": "Imprimir",
-                    "formats": "Formatos",
-                    "sourcecode": "Visualizar Código",
-                    "preview": "Exibir",
-                    "viewside": "ViewSide",
-                    "insertCode": "Inserir Código",
-                    "linkText": "Exibir Texto",
-                    "linkTooltipLabel": "Título",
-                    "linkWebUrl": "Endereço Web",
-                    "linkTitle": "Entre com um título",
-                    "linkurl": "http://exemplo.com",
-                    "linkOpenInNewWindow": "Abrir Link em Nova Janela",
-                    "linkHeader": "Inserir Link",
-                    "dialogInsert": "Inserir",
-                    "dialogCancel": "Cancelar",
-                    "dialogUpdate": "Atualizar",
-                    "imageHeader": "Inserir Imagem",
-                    "imageLinkHeader": "Você pode proporcionar um link da web",
-                    "mdimageLink": "Favor proporcionar uma URL para sua imagem",
-                    "imageUploadMessage": "Solte a imagem aqui ou busque para o upload",
-                    "imageDeviceUploadMessage": "Clique aqui para o upload",
-                    "imageAlternateText": "Texto Alternativo",
-                    "alternateHeader": "Texto Alternativo",
-                    "browse": "Procurar",
-                    "imageUrl": "http://exemplo.com/imagem.png",
-                    "imageCaption": "Título",
-                    "imageSizeHeader": "Tamanho da Imagem",
-                    "imageHeight": "Altura",
-                    "imageWidth": "Largura",
-                    "textPlaceholder": "Entre com um Texto",
-                    "inserttablebtn": "Inserir Tabela",
-                    "tabledialogHeader": "Inserir Tabela",
-                    "tableWidth": "Largura",
-                    "cellpadding": "Espaçamento de célula",
-                    "cellspacing": "Espaçamento de célula",
-                    "columns": "Número de colunas",
-                    "rows": "Número de linhas",
-                    "tableRows": "Linhas da Tabela",
-                    "tableColumns": "Colunas da Tabela",
-                    "tableCellHorizontalAlign": "Alinhamento Horizontal da Célular",
-                    "tableCellVerticalAlign": "Alinhamento Vertical da Célular",
-                    "createTable": "Criar Tabela",
-                    "removeTable": "Remover Tabela",
-                    "tableHeader": "Cabeçalho da Tabela",
-                    "tableRemove": "Remover Tabela",
-                    "tableCellBackground": "Fundo da Célula",
-                    "tableEditProperties": "Editar Propriedades da Tabela",
-                    "styles": "Estilos",
-                    "insertColumnLeft": "Inserir Coluna à Esquerda",
-                    "insertColumnRight": "Inserir Coluna à Direita",
-                    "deleteColumn": "Apagar Coluna",
-                    "insertRowBefore": "Inserir Linha Antes",
-                    "insertRowAfter": "Inserir Linha Depois",
-                    "deleteRow": "Apagar Linha",
-                    "tableEditHeader": "Edita Tabela",
-                    "TableHeadingText": "Cabeçãlho",
-                    "TableColText": "Col",
-                    "imageInsertLinkHeader": "Inserir Link",
-                    "editImageHeader": "Edita Imagem",
-                    "alignmentsDropDownLeft": "Alinhar à Esquerda",
-                    "alignmentsDropDownCenter": "Centralizar",
-                    "alignmentsDropDownRight": "Alinhar à Direita",
-                    "alignmentsDropDownJustify": "Justificar",
-                    "imageDisplayDropDownInline": "Inline",
-                    "imageDisplayDropDownBreak": "Break",
-                    "tableInsertRowDropDownBefore": "Inserir linha antes",
-                    "tableInsertRowDropDownAfter": "Inserir linha depois",
-                    "tableInsertRowDropDownDelete": "Apagar linha",
-                    "tableInsertColumnDropDownLeft": "Inserir coluna à esquerda",
-                    "tableInsertColumnDropDownRight": "Inserir coluna à direita",
-                    "tableInsertColumnDropDownDelete": "Apagar Coluna",
-                    "tableVerticalAlignDropDownTop": "Alinhar no Topo",
-                    "tableVerticalAlignDropDownMiddle": "Alinhar no Meio",
-                    "tableVerticalAlignDropDownBottom": "Alinhar no Fundo",
-                    "tableStylesDropDownDashedBorder": "Bordas Pontilhadas",
-                    "tableStylesDropDownAlternateRows": "Linhas Alternadas",
-                    "pasteFormat": "Colar Formato",
-                    "pasteFormatContent": "Escolha a ação de formatação",
-                    "plainText": "Texto Simples",
-                    "cleanFormat": "Limpar",
-                    "keepFormat": "Manter",
-                    "formatsDropDownParagraph": "Parágrafo",
-                    "formatsDropDownCode": "Código",
-                    "formatsDropDownQuotation": "Citação",
-                    "formatsDropDownHeading1": "Cabeçalho 1",
-                    "formatsDropDownHeading2": "Cabeçalho 2",
-                    "formatsDropDownHeading3": "Cabeçalho 3",
-                    "formatsDropDownHeading4": "Cabeçalho 4",
-                    "fontNameSegoeUI": "SegoeUI",
-                    "fontNameArial": "Arial",
-                    "fontNameGeorgia": "Georgia",
-                    "fontNameImpact": "Impact",
-                    "fontNameTahoma": "Tahoma",
-                    "fontNameTimesNewRoman": "Times New Roman",
-                    "fontNameVerdana": "Verdana"
-                }
-            }
-        });
-
-        $("#txtFile").change(function (event) {
-            try {
-                var files = event.target.files;
-                $("#imgViewer").attr("src", window.URL.createObjectURL(files[0]));
-                $("#painelfundo").css({ "padding-bottom:": "200px" });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "#txtFile.change", error);
-            }
-        });
-
-        $(document).ready(function () {
-            try {
-                if ('@Model.ViagemObj.Viagem.ViagemId' != '00000000-0000-0000-0000-000000000000') {
-                    $.ajax({
-                        type: "GET",
-                        url: "/api/Viagem/PegaFicha",
-                        success: function (data) {
-                            if (data.fichaVistoria != null) {
-                                $('#imgViewer').attr('src', "data:image/jpg;base64," + data.fichaVistoria);
-                            } else {
-                                $('#imgViewer').attr('src', "/Images/FichaAmarelaNova.jpg");
-                            }
-                        },
-                        error: function (data) {
-                            console.log('Error:', data);
-                        }
-                    });
-                } else {
-                    $('#imgViewer').attr('src', "/Images/FichaAmarelaNova.jpg");
-
-                }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "document.ready", error);
-            }
-        });
-    </script>
+            } else {
+                $('#imgViewer').attr('src', "/Images/FichaAmarelaNova.jpg");
+
+            }
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("TaxiLeg.cshtml", "document.ready", error);
+        }
+    });
+</script>
 }
```
