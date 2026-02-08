# Pages/Administracao/GestaoRecursosNavegacao.cshtml

**Mudanca:** GRANDE | **+2897** linhas | **-3454** linhas

---

```diff
--- JANEIRO: Pages/Administracao/GestaoRecursosNavegacao.cshtml
+++ ATUAL: Pages/Administracao/GestaoRecursosNavegacao.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Administracao.GestaoRecursosNavegacaoModel
 
 @{
@@ -36,18 +35,13 @@
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
@@ -90,7 +84,7 @@
             flex: 0 0 400px;
             background: #fff;
             border-radius: 8px;
-            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
+            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
             display: flex;
             flex-direction: column;
         }
@@ -135,7 +129,7 @@
         .props-card {
             background: #fff;
             border-radius: 8px;
-            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
+            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
         }
 
         .props-header {
@@ -158,16 +152,14 @@
 
         /* ====== FORMULARIO DE PROPRIEDADES ====== */
         .form-group {
-            margin-bottom: 2rem;
-            /* Espaçamento vertical aumentado significativamente */
+            margin-bottom: 2rem; /* Espaçamento vertical aumentado significativamente */
         }
 
         .form-group label {
             display: block;
             font-weight: 500;
             color: #495057;
-            margin-bottom: 0.375rem;
-            /* Mantém label próxima ao controle */
+            margin-bottom: 0.375rem; /* Mantém label próxima ao controle */
             font-size: 0.875rem;
         }
 
@@ -352,10 +344,8 @@
         #gestaoTreeView .tree-node.selected .node-text,
         #gestaoTreeView .e-list-item.e-active .tree-node.selected .node-text,
         #gestaoTreeView .e-list-item.e-node-focus .tree-node.selected .node-text {
-            color: #cc5500 !important;
-            /* Laranja bem escuro */
-            font-weight: 700 !important;
-            /* Negrito */
+            color: #cc5500 !important; /* Laranja bem escuro */
+            font-weight: 700 !important; /* Negrito */
             opacity: 1 !important;
             visibility: visible !important;
         }
@@ -376,14 +366,11 @@
 
         /* Classe para ícones duotone padrão FrotiX (reutilizável) */
         .icon-duotone-ftx {
-            --fa-primary-color: #ff6b35;
-            /* Laranja forte */
-            --fa-secondary-color: #6c757d;
-            /* Cinza médio */
+            --fa-primary-color: #ff6b35; /* Laranja forte */
+            --fa-secondary-color: #6c757d; /* Cinza médio */
             --fa-primary-opacity: 1;
             --fa-secondary-opacity: 1;
-            margin-right: 0.5rem;
-            /* Espaçamento de 2 caracteres */
+            margin-right: 0.5rem; /* Espaçamento de 2 caracteres */
         }
 
         /* Ícones duotone com cores FrotiX padrão */
@@ -470,7 +457,7 @@
 
         #gestaoTreeView .tree-node .node-badge:hover {
             transform: scale(1.05);
-            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
+            box-shadow: 0 2px 6px rgba(0,0,0,0.2);
         }
 
         /* Badge Página - Azul */
@@ -496,7 +483,7 @@
         /* Drag and drop */
         #gestaoTreeView .e-drag-item {
             background: #fff;
-            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
+            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
             border-radius: 6px;
         }
 
@@ -585,36 +572,28 @@
 
         /* Setas de Ordenação (Esquerda) - Vinho/Vermelho claro */
         .tree-nav-btn.btn-order i {
-            --fa-primary-color: #722F37;
-            /* Vinho */
-            --fa-secondary-color: #FFCCCB;
-            /* Vermelho claro */
+            --fa-primary-color: #722F37; /* Vinho */
+            --fa-secondary-color: #FFCCCB; /* Vermelho claro */
             --fa-primary-opacity: 1;
             --fa-secondary-opacity: 0.7;
         }
 
         .tree-nav-btn.btn-order:hover i {
-            --fa-primary-color: #5a252c;
-            /* Vinho mais escuro */
-            --fa-secondary-color: #ff9999;
-            /* Vermelho mais visível */
+            --fa-primary-color: #5a252c; /* Vinho mais escuro */
+            --fa-secondary-color: #ff9999; /* Vermelho mais visível */
         }
 
         /* Setas de Hierarquia (Direita) - Roxo/Azul claro */
         .tree-nav-btn.btn-level i {
-            --fa-primary-color: #6B3FA0;
-            /* Roxo */
-            --fa-secondary-color: #ADD8E6;
-            /* Azul claro */
+            --fa-primary-color: #6B3FA0; /* Roxo */
+            --fa-secondary-color: #ADD8E6; /* Azul claro */
             --fa-primary-opacity: 1;
             --fa-secondary-opacity: 0.7;
         }
 
         .tree-nav-btn.btn-level:hover i {
-            --fa-primary-color: #522f7a;
-            /* Roxo mais escuro */
-            --fa-secondary-color: #87CEEB;
-            /* Azul mais visível */
+            --fa-primary-color: #522f7a; /* Roxo mais escuro */
+            --fa-secondary-color: #87CEEB; /* Azul mais visível */
         }
 
         /* Separador entre botões esquerda/direita e o conteúdo */
@@ -627,14 +606,12 @@
 
         /* ====== CARD DE CONFIRMAÇÃO DE ALTERAÇÕES ====== */
         .tree-changes-card {
-            display: none !important;
-            /* Oculto por padrão */
+            display: none !important; /* Oculto por padrão */
             background: linear-gradient(135deg, #fff3cd 0%, #ffeeba 100%);
             border: 2px solid #ffc107;
             border-radius: 8px;
             padding: 0.75rem 1.5rem;
-            margin: 0 1.5rem 1rem 1.5rem;
-            /* Margem para alinhar com o container */
+            margin: 0 1.5rem 1rem 1.5rem; /* Margem para alinhar com o container */
             align-items: center;
             justify-content: space-between;
             gap: 1rem;
@@ -647,15 +624,8 @@
         }
 
         @@keyframes slideIn {
-            from {
-                opacity: 0;
-                transform: translateY(-10px);
-            }
-
-            to {
-                opacity: 1;
-                transform: translateY(0);
-            }
+            from { opacity: 0; transform: translateY(-10px); }
+            to { opacity: 1; transform: translateY(0); }
         }
 
         .tree-changes-info {
@@ -696,10 +666,8 @@
 
         .btn-confirmar-alteracoes i {
             font-size: 0.95rem;
-            --fa-primary-color: #fff;
-            /* Branco */
-            --fa-secondary-color: #a0c4e8;
-            /* Azul claro */
+            --fa-primary-color: #fff; /* Branco */
+            --fa-secondary-color: #a0c4e8; /* Azul claro */
             --fa-primary-opacity: 1;
             --fa-secondary-opacity: 0.8;
         }
@@ -725,10 +693,8 @@
 
         .btn-cancelar-alteracoes i {
             font-size: 0.95rem;
-            --fa-primary-color: #fff;
-            /* Branco */
-            --fa-secondary-color: #FFCCCB;
-            /* Vermelho claro */
+            --fa-primary-color: #fff; /* Branco */
+            --fa-secondary-color: #FFCCCB; /* Vermelho claro */
             --fa-primary-opacity: 1;
             --fa-secondary-opacity: 0.8;
         }
@@ -814,15 +780,8 @@
         }
 
         @@keyframes slideIn {
-            from {
-                transform: translateX(100%);
-                opacity: 0;
-            }
-
-            to {
-                transform: translateX(0);
-                opacity: 1;
-            }
+            from { transform: translateX(100%); opacity: 0; }
+            to { transform: translateX(0); opacity: 1; }
         }
 
         /* ====== DROPDOWNTREE DE ÍCONES - BORDAS SEMPRE VISÍVEIS ====== */
@@ -898,12 +857,10 @@
                 <span id="changesMessage">Alterações pendentes</span>
             </div>
             <div class="tree-changes-actions">
-                <button type="button" class="btn-confirmar-alteracoes" id="btnConfirmarAlteracoes"
-                    title="Confirmar e salvar">
+                <button type="button" class="btn-confirmar-alteracoes" id="btnConfirmarAlteracoes" title="Confirmar e salvar">
                     <i class="fa-duotone fa-circle-check"></i>
                 </button>
-                <button type="button" class="btn-cancelar-alteracoes" id="btnCancelarAlteracoes"
-                    title="Cancelar alterações">
+                <button type="button" class="btn-cancelar-alteracoes" id="btnCancelarAlteracoes" title="Cancelar alterações">
                     <i class="fa-duotone fa-circle-xmark"></i>
                 </button>
             </div>
@@ -927,8 +884,7 @@
 
         <div class="props-card" id="propsCard" style="display: none;">
             <div class="props-header">
-                <h5><i class="fa-duotone fa-sliders"></i> Propriedades do Item <span id="modoEdicao"
-                        class="badge bg-secondary ms-2">Selecione</span></h5>
+                <h5><i class="fa-duotone fa-sliders"></i> Propriedades do Item <span id="modoEdicao" class="badge bg-secondary ms-2">Selecione</span></h5>
                 <span id="selectedItemName" class="text-muted">Nenhum item selecionado</span>
             </div>
             <div class="props-body">
@@ -939,15 +895,13 @@
                     <div class="row">
                         <div class="col-md-6">
                             <div class="form-group">
-                                <label for="txtNome">Nome (exibido no menu)<span
-                                        class="required-asterisk">*</span></label>
+                                <label for="txtNome">Nome (exibido no menu)<span class="required-asterisk">*</span></label>
                                 <input type="text" class="form-control" id="txtNome" placeholder="Ex: Veiculos" />
                             </div>
                         </div>
                         <div class="col-md-6">
                             <div class="form-group">
-                                <label for="txtNomeMenu">NomeMenu (identificador único)<span
-                                        class="required-asterisk">*</span></label>
+                                <label for="txtNomeMenu">NomeMenu (identificador único)<span class="required-asterisk">*</span></label>
                                 <input type="text" class="form-control" id="txtNomeMenu" placeholder="Ex: veiculo" />
                             </div>
                         </div>
@@ -957,10 +911,16 @@
                         <div class="col-12">
                             <div class="form-group">
                                 <label for="ddlPosicao">Posicionar abaixo de...</label>
-                                <ejs-dropdowntree id="ddlPosicao" placeholder="Selecione a posição na estrutura..."
-                                    popupHeight="350px" showCheckBox="false" showClearButton="true"
-                                    allowFiltering="true" filterType="Contains" ignoreCase="true" cssClass="e-outline"
-                                    change="onPosicaoChange">
+                                <ejs-dropdowntree id="ddlPosicao"
+                                                 placeholder="Selecione a posição na estrutura..."
+                                                 popupHeight="350px"
+                                                 showCheckBox="false"
+                                                 showClearButton="true"
+                                                 allowFiltering="true"
+                                                 filterType="Contains"
+                                                 ignoreCase="true"
+                                                 cssClass="e-outline"
+                                                 change="onPosicaoChange">
                                 </ejs-dropdowntree>
                                 <small class="form-text text-muted mt-1">
                                     O item será posicionado logo após o item selecionado
@@ -974,13 +934,11 @@
                             <div class="form-group">
                                 <label>Tipo de Item</label>
                                 <div class="tipo-item-toggle" id="tipoItemToggle">
-                                    <button type="button" class="tipo-item-btn active" id="btnTipoPagina"
-                                        onclick="selecionarTipoItem('pagina')">
+                                    <button type="button" class="tipo-item-btn active" id="btnTipoPagina" onclick="selecionarTipoItem('pagina')">
                                         <i class="fa-duotone fa-file-lines"></i>
                                         <span>Página</span>
                                     </button>
-                                    <button type="button" class="tipo-item-btn" id="btnTipoGrupo"
-                                        onclick="selecionarTipoItem('grupo')">
+                                    <button type="button" class="tipo-item-btn" id="btnTipoGrupo" onclick="selecionarTipoItem('grupo')">
                                         <i class="fa-duotone fa-folder-tree"></i>
                                         <span>Grupo</span>
                                     </button>
@@ -993,13 +951,18 @@
                     <div class="row" id="rowPaginaSistema">
                         <div class="col-12">
                             <div class="form-group">
-                                <label for="ddlPagina">Selecione a Página do Sistema<span
-                                        class="required-asterisk">*</span></label>
-
-                                <ejs-dropdowntree id="ddlPagina" placeholder="Busque ou selecione uma página..."
-                                    popupHeight="400px" showCheckBox="false" showClearButton="true"
-                                    allowFiltering="true" filterType="Contains" ignoreCase="true"
-                                    cssClass="e-outline ddl-pagina-border" created="onPaginaDropdownCreated">
+                                <label for="ddlPagina">Selecione a Página do Sistema<span class="required-asterisk">*</span></label>
+
+                                <ejs-dropdowntree id="ddlPagina"
+                                                 placeholder="Busque ou selecione uma página..."
+                                                 popupHeight="400px"
+                                                 showCheckBox="false"
+                                                 showClearButton="true"
+                                                 allowFiltering="true"
+                                                 filterType="Contains"
+                                                 ignoreCase="true"
+                                                 cssClass="e-outline ddl-pagina-border"
+                                                 created="onPaginaDropdownCreated">
                                 </ejs-dropdowntree>
 
                                 <small class="form-text text-muted mt-1">
@@ -1016,20 +979,25 @@
                     <div class="row">
                         <div class="col-md-8">
                             <div class="form-group">
-                                <label for="ddlIcone">Selecione o Ícone (FontAwesome 7 Pro)<span
-                                        class="required-asterisk">*</span></label>
-
-                                <ejs-dropdowntree id="ddlIcone" placeholder="Busque ou selecione um ícone..."
-                                    popupHeight="400px" showCheckBox="false" showClearButton="true"
-                                    allowFiltering="true" filterType="Contains" ignoreCase="true"
-                                    cssClass="e-outline ddl-icone-border" created="onIconeDropdownCreated"
-                                    change="onIconeChange">
+                                <label for="ddlIcone">Selecione o Ícone (FontAwesome 7 Pro)<span class="required-asterisk">*</span></label>
+
+                                <ejs-dropdowntree id="ddlIcone"
+                                                 placeholder="Busque ou selecione um ícone..."
+                                                 popupHeight="400px"
+                                                 showCheckBox="false"
+                                                 showClearButton="true"
+                                                 allowFiltering="true"
+                                                 filterType="Contains"
+                                                 ignoreCase="true"
+                                                 cssClass="e-outline ddl-icone-border"
+                                                 created="onIconeDropdownCreated"
+                                                 change="onIconeChange">
                                 </ejs-dropdowntree>
 
                                 <small class="form-text text-muted mt-1">Classe CSS:</small>
                                 <input type="text" class="form-control form-control-sm mt-1" id="txtIconClass" readonly
-                                    placeholder="A classe do ícone aparecerá aqui..."
-                                    style="background-color: #f8f9fa; font-family: monospace; font-size: 0.85rem;" />
+                                       placeholder="A classe do ícone aparecerá aqui..."
+                                       style="background-color: #f8f9fa; font-family: monospace; font-size: 0.85rem;" />
                             </div>
                         </div>
                         <div class="col-md-4">
@@ -1045,8 +1013,7 @@
 
                     <div class="form-group">
                         <label for="txtDescricao">Descrição</label>
-                        <textarea class="form-control" id="txtDescricao" rows="2"
-                            placeholder="Descrição do recurso..."></textarea>
+                        <textarea class="form-control" id="txtDescricao" rows="2" placeholder="Descrição do recurso..."></textarea>
                     </div>
 
                     <div class="d-flex gap-2 mt-3">
@@ -1088,732 +1055,543 @@
 </div>
 
 @section ScriptsBlock {
-    <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * GESTÃO DE RECURSOS DE NAVEGAÇÃO - EDITOR DO MENU LATERAL
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Sistema completo de gerenciamento do menu de navegação FrotiX
-            * Permite criar, editar, reorganizar e excluir itens do menu lateral
-                * Utiliza TreeView do Syncfusion com suporte a drag - and - drop
-                    */
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * VARIÁVEIS GLOBAIS
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /** @@type { Array } Dados da árvore de navegação */
-        var treeData = [];
-            /** @@type { Object | null } Instância do TreeView Syncfusion */
-            var treeObj = null;
-            /** @@type { Object | null } Item atualmente selecionado na árvore */
-        var selectedItem = null;
-            /** @@type { Object | null } Informações do último drag - and - drop para toast */
-        var lastDragInfo = null;
-
-            /**
-             * Controle de alterações pendentes na estrutura
-             * @@description Rastreia se há mudanças não salvas no JSON de navegação
-            */
-            /** @@type { Object | null } Estado original antes das alterações */
-        var treeDataOriginal = null;
-            /** @@type { boolean } Flag para indicar se há alterações não salvas */
-        var alteracoesPendentes = false;
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES AUXILIARES - BUSCA DE ÍCONES
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Busca ícone pelo ID no dataSource do DropDownTree
-             * @@param { string } iconId - ID do ícone(classe FontAwesome)
-            * @@returns { Object| null} Objeto do ícone ou null se não encontrado
-                */
-        function findIconById(iconId) {
-            try {
-                var ddlIconeObj = document.getElementById('ddlIcone')?.ej2_instances?.[0];
-                if (!ddlIconeObj || !ddlIconeObj.fields || !ddlIconeObj.fields.dataSource) return null;
-                var ds = ddlIconeObj.fields.dataSource;
-
-                for (var i = 0; i < ds.length; i++) {
-                    var cat = ds[i];
-                    if (cat.child && cat.child.length > 0) {
-                        for (var j = 0; j < cat.child.length; j++) {
-                            if (cat.child[j].id === iconId) return cat.child[j];
+<script>
+    var treeData = [];
+    var treeObj = null;
+    var selectedItem = null;
+    var lastDragInfo = null;
+
+    var treeDataOriginal = null;
+    var alteracoesPendentes = false;
+
+    function findIconById(iconId) {
+        try {
+            var ddlIconeObj = document.getElementById('ddlIcone')?.ej2_instances?.[0];
+            if (!ddlIconeObj || !ddlIconeObj.fields || !ddlIconeObj.fields.dataSource) return null;
+            var ds = ddlIconeObj.fields.dataSource;
+            for (var i = 0; i < ds.length; i++) {
+                var cat = ds[i];
+                if (cat.child && cat.child.length > 0) {
+                    for (var j = 0; j < cat.child.length; j++) {
+                        if (cat.child[j].id === iconId) return cat.child[j];
+                    }
+                }
+            }
+            return null;
+        } catch (e) {
+            console.warn('[findIconById] Erro:', e);
+            return null;
+        }
+    }
+
+    var ultimoIconeSelecionado = null;
+    var ultimaPaginaSelecionada = null;
+
+    function onIconeDropdownCreated() {
+        try {
+            var ddlIconeObj = document.getElementById('ddlIcone').ej2_instances[0];
+            if (ddlIconeObj) {
+                ddlIconeObj.itemTemplate = function(data) {
+                    if (data.isCategory) {
+                        return '<div style="font-weight: 600; padding: 4px 0; color: #3D5771;">' + data.text + '</div>';
+                    }
+
+                    var iconClass = data.id || '';
+                    if (iconClass && !iconClass.includes('fa-duotone')) {
+                        iconClass = iconClass.replace(/fa-(regular|solid|light)/g, 'fa-duotone');
+                    }
+                    return '<div style="display: flex; align-items: center; gap: 8px;">' +
+                           '<i class="' + iconClass + '" style="font-size: 16px; width: 20px; --fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i>' +
+                           '<span style="color: #5f6b7a;">' + data.text + '</span>' +
+                           '</div>';
+                };
+
+                ddlIconeObj.valueTemplate = function(data) {
+                    if (!data || data.isCategory) return '';
+
+                    var iconClass = data.id || '';
+                    if (iconClass && !iconClass.includes('fa-duotone')) {
+                        iconClass = iconClass.replace(/fa-(regular|solid|light)/g, 'fa-duotone');
+                    }
+                    return '<div style="display: flex; align-items: center; gap: 8px;">' +
+                           '<i class="' + iconClass + '" style="font-size: 14px; width: 18px; --fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i>' +
+                           '<span style="color: #5f6b7a;">' + data.text + '</span>' +
+                           '</div>';
+                };
+
+                ddlIconeObj.selecting = function(args) {
+                    if (args.nodeData && !args.nodeData.isCategory) {
+                        ultimoIconeSelecionado = args.nodeData.id;
+                        var txtIconClass = document.getElementById('txtIconClass');
+                        if (txtIconClass) {
+                            txtIconClass.value = args.nodeData.id;
                         }
                     }
-                }
-                return null;
-            } catch (e) {
-                console.warn('[findIconById] Erro:', e);
-                return null;
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * VARIÁVEIS DE RASTREAMENTO DE SELEÇÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /** @@type { string | null } Último ícone selecionado no DropDownTree */
-        var ultimoIconeSelecionado = null;
-            /** @@type { string | null } Última página selecionada no DropDownTree */
-        var ultimaPaginaSelecionada = null;
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CONFIGURAÇÃO DO DROPDOWN DE ÍCONES
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Callback executado após criação do DropDownTree de ícones
-             * @@description Configura templates de item / valor e eventos de seleção
-            * Aplica padrão FrotiX(Duotone com cores laranja / cinza)
-                * @@returns { void}
-                */
-        function onIconeDropdownCreated() {
-            try {
-                var ddlIconeObj = document.getElementById('ddlIcone').ej2_instances[0];
-                if (ddlIconeObj) {
-
-                    ddlIconeObj.itemTemplate = function (data) {
-                        if (data.isCategory) {
-                            return '<div style="font-weight: 600; padding: 4px 0; color: #3D5771;">' + data.text + '</div>';
+                };
+
+                ddlIconeObj.select = function(args) {
+                    console.log('=== SELECT ícone ===', args);
+                    if (args.nodeData && !args.nodeData.isCategory) {
+                        ultimoIconeSelecionado = args.nodeData.id;
+                        var txtIconClass = document.getElementById('txtIconClass');
+                        if (txtIconClass) {
+                            txtIconClass.value = args.nodeData.id;
                         }
 
-                        var iconClass = data.id || '';
+                        ddlIconeObj.value = [args.nodeData.id];
+                        ddlIconeObj.dataBind();
+                        setTimeout(function() {
+                            ddlIconeObj.hidePopup();
+
+                            onIconeChange({ itemData: args.nodeData, value: [args.nodeData.id] });
+
+                            atualizarEstadoBotoesOrdenacao();
+                        }, 50);
+                    }
+                };
+
+                ddlIconeObj.change = function(args) {
+                    onIconeChange(args);
+                };
+
+                ddlIconeObj.changeOnBlur = false;
+
+                console.log('Templates e eventos de ícone configurados');
+            }
+        } catch (error) {
+            console.error('Erro ao configurar template do DropDownTree:', error);
+        }
+    }
+
+    function decodeHtml(html) {
+        try {
+            if (!html) return html;
+            var txt = document.createElement("textarea");
+            txt.innerHTML = html;
+            return txt.value;
+        } catch (erro) {
+            console.error('[decodeHtml] Erro:', erro);
+            return html;
+        }
+    }
+
+    function encodeHtml(text) {
+        try {
+            if (!text) return text;
+
+            var result = '';
+            for (var i = 0; i < text.length; i++) {
+                var char = text[i];
+                var code = char.charCodeAt(0);
+
+                if (code >= 32 && code <= 126 && char !== '<' && char !== '>' && char !== '&' && char !== '"' && char !== "'") {
+                    result += char;
+                } else {
+
+                    result += '&#' + code + ';';
+                }
+            }
+            return result;
+        } catch (erro) {
+            console.error('[encodeHtml] Erro:', erro);
+            return text;
+        }
+    }
+
+    document.addEventListener('DOMContentLoaded', function() {
+        carregarArvore();
+        carregarIconesFontAwesome();
+        carregarPaginasSistema();
+
+        configurarBotoesAlteracoes();
+    });
+
+    function popularDropdownItemPai() {
+        try {
+            var grupos = [];
+
+            grupos.push({
+                id: '_SEM_GRUPO_',
+                text: '<sem grupo>',
+                textFormatado: '<sem grupo>',
+                textoSemIndentacao: '<sem grupo>',
+                nivel: -1
+            });
+
+            function extrairGrupos(items) {
+                if (!items) return;
+
+                items.forEach(function(item) {
+
+                    var isGrupo = !item.href || item.href === 'javascript:void(0);';
+
+                    if (isGrupo) {
+                        grupos.push({
+                            id: item.id,
+                            text: item.text,
+                            nivel: calcularNivel(item.id)
+                        });
+                    }
+
+                    if (item.items && item.items.length > 0) {
+                        extrairGrupos(item.items);
+                    }
+                });
+            }
+
+            function calcularNivel(itemId) {
+                var nivel = 0;
+                var item = buscarItemPorId(treeData, itemId);
+
+                while (item && item.parentId) {
+                    nivel++;
+                    item = buscarItemPorId(treeData, item.parentId);
+                }
+
+                return nivel;
+            }
+
+            extrairGrupos(treeData);
+
+            grupos.forEach(function(grupo) {
+                if (grupo.id === '_SEM_GRUPO_') {
+
+                    return;
+                } else {
+                    var textoDecodificado = decodeHtml(grupo.text);
+                    var indentacao = ' '.repeat(grupo.nivel);
+                    grupo.textFormatado = indentacao + textoDecodificado;
+                    grupo.textoSemIndentacao = textoDecodificado;
+                }
+            });
+
+            grupos.sort(function(a, b) {
+                if (a.id === '_SEM_GRUPO_') return -1;
+                if (b.id === '_SEM_GRUPO_') return 1;
+
+                return a.textoSemIndentacao.localeCompare(b.textoSemIndentacao, 'pt-BR', { sensitivity: 'base' });
+            });
+
+            console.log('Grupos encontrados:', grupos.length - 1, '+ <sem grupo>');
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "popularDropdownItemPai", erro);
+        }
+    }
+
+    function popularDropdownPosicao() {
+        try {
+            var itens = [];
+
+            itens.push({
+                id: '_INICIO_',
+                text: '📍 No início (primeiro item)',
+                hasChildren: false
+            });
+
+            function extrairItensHierarquicos(lista, nivel) {
+                lista.forEach(function(item) {
+                    var indentacao = ' '.repeat(nivel);
+                    var icone = (item.items && item.items.length > 0) ? '📁' : '📄';
+
+                    itens.push({
+                        id: item.id,
+                        text: indentacao + icone + ' ' + decodeHtml(item.text),
+                        hasChildren: false
+                    });
+
+                    if (item.items && item.items.length > 0) {
+                        extrairItensHierarquicos(item.items, nivel + 1);
+                    }
+                });
+            }
+
+            extrairItensHierarquicos(treeData, 0);
+
+            var ddlPosicao = document.getElementById('ddlPosicao');
+            if (ddlPosicao && ddlPosicao.ej2_instances && ddlPosicao.ej2_instances[0]) {
+                var ddl = ddlPosicao.ej2_instances[0];
+                ddl.fields = {
+                    dataSource: itens,
+                    value: 'id',
+                    text: 'text',
+                    hasChildren: 'hasChildren'
+                };
+                ddl.dataBind();
+                console.log('✅ Dropdown de posição populado com', itens.length, 'itens');
+            }
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "popularDropdownPosicao", erro);
+        }
+    }
+
+    function selecionarTipoItem(tipo) {
+        try {
+            var btnPagina = document.getElementById('btnTipoPagina');
+            var btnGrupo = document.getElementById('btnTipoGrupo');
+            var tipoItemInput = document.getElementById('tipoItem');
+            var rowPagina = document.getElementById('rowPaginaSistema');
+
+            if (tipo === 'pagina') {
+                btnPagina.classList.add('active');
+                btnGrupo.classList.remove('active');
+                tipoItemInput.value = 'pagina';
+
+                if (rowPagina) rowPagina.style.display = 'block';
+
+            } else {
+                btnPagina.classList.remove('active');
+                btnGrupo.classList.add('active');
+                tipoItemInput.value = 'grupo';
+
+                if (rowPagina) rowPagina.style.display = 'none';
+
+                document.getElementById('txtHref').value = '';
+            }
+
+            console.log('[TIPO] Tipo de item alterado para:', tipo);
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "selecionarTipoItem", erro);
+        }
+    }
+
+    function onPosicaoChange(args) {
+        try {
+            console.log('[POSIÇÃO] Nova posição selecionada:', args.value);
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "onPosicaoChange", erro);
+        }
+    }
+
+    function atualizarToggleTipo(item) {
+        if (!item) return;
+
+        var ehGrupo = !item.href || item.href === 'javascript:void(0);' || (item.items && item.items.length > 0);
+
+        if (ehGrupo) {
+            selecionarTipoItem('grupo');
+        } else {
+            selecionarTipoItem('pagina');
+        }
+    }
+
+    function alternarTipoItem(itemId, event) {
+        try {
+            console.log('[TOGGLE-TIPO] Alternando tipo do item:', itemId);
+
+            var item = buscarItemPorId(treeData, itemId);
+            if (!item) {
+                console.error('[TOGGLE-TIPO] Item não encontrado:', itemId);
+                return;
+            }
+
+            var nomeItem = decodeHtml(item.text);
+            var ehGrupoAtual = !item.href || item.href === 'javascript:void(0);';
+
+            if (ehGrupoAtual) {
+
+                var htmlPaginas = montarSelectPaginasParaSwal(itemId);
+
+                var mensagemBase = 'O Grupo <strong>"' + nomeItem + '"</strong> será transformado em uma <strong>Página</strong>.<br><br>';
+
+                if (item.items && item.items.length > 0) {
+                    mensagemBase += '<div class="alert alert-warning" style="text-align:left; font-size:0.9rem; margin-bottom:15px;">' +
+                        '<i class="fa-duotone fa-triangle-exclamation"></i> ' +
+                        'Este grupo possui <strong>' + item.items.length + '</strong> filho(s).<br>' +
+                        'Eles serão movidos para o nível acima.' +
+                        '</div>';
+                }
+
+                mostrarModalTransformacaoGrupoEmPagina('Transformar Grupo em Página', mensagemBase, htmlPaginas)
+                    .then(function(result) {
+                        if (result.confirmado && result.paginaUrl) {
+                            executarTransformacaoGrupoEmPagina(item, result.paginaUrl);
+                        }
+                    });
+
+            } else {
+
+                var irmaosAbaixo = encontrarIrmaosAbaixo(item);
+
+                var mensagem = 'A Página <strong>"' + nomeItem + '"</strong> será transformada em <strong>Grupo</strong>.<br><br>' +
+                    '<div class="alert alert-info" style="text-align:left; font-size:0.9rem;">' +
+                    '<i class="fa-duotone fa-info-circle"></i> A URL da página será removida.' +
+                    '</div>';
+
+                if (irmaosAbaixo.length > 0) {
+
+                    mostrarModalTransformacaoPaginaEmGrupo(item, nomeItem, irmaosAbaixo);
+
+                } else {
+
+                    Alerta.Confirmar(
+                        'Transformar Página em Grupo',
+                        mensagem,
+                        'Sim, Transformar',
+                        'Cancelar'
+                    ).then(function(confirmado) {
+                        if (confirmado) {
+                            executarTransformacaoPaginaEmGrupo(item, false, [], null);
+                        }
+                    });
+                }
+            }
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "alternarTipoItem", erro);
+        }
+    }
+
+    function buscarIconePorUrl(items, url) {
+        try {
+            if (!items || !url) return null;
+
+            var normalizarUrl = function(u) {
+                if (!u) return '';
+                u = u.toLowerCase().trim();
+                u = u.replace(/^\
+                u = u.replace(/\.html$/, '');
+                u = u.replace(/\.cshtml$/, '');
+                u = u.replace(/_/g, '');
+                u = u.replace(/\
+                return u;
+            };
+
+            var extrairPartesUrl = function(u) {
+                if (!u) return { modulo: '', pagina: '', completa: '' };
+                u = u.toLowerCase().trim();
+                var partes = u.replace(/\.(html|cshtml)$/, '').split(/[\/_]/);
+                return {
+                    modulo: partes[0] || '',
+                    pagina: partes[1] || partes[0] || '',
+                    completa: u.replace(/\.(html|cshtml)$/, '').replace(/[\/_]/g, '')
+                };
+            };
+
+            var urlNormalizada = normalizarUrl(url);
+            var partesUrl = extrairPartesUrl(url);
+
+            for (var i = 0; i < items.length; i++) {
+                var item = items[i];
+
+                if (item.href) {
+                    var hrefNormalizado = normalizarUrl(item.href);
+                    var partesHref = extrairPartesUrl(item.href);
+
+                    var matchExato = hrefNormalizado === urlNormalizada;
+
+                    var matchPartes = partesHref.completa === partesUrl.completa ||
+                                     (partesHref.modulo === partesUrl.modulo && partesHref.pagina === partesUrl.pagina);
+
+                    var matchFlexivel = hrefNormalizado.includes(urlNormalizada) || urlNormalizada.includes(hrefNormalizado);
+
+                    if ((matchExato || matchPartes || matchFlexivel) && item.icon) {
+
+                        var iconClass = item.icon || '';
                         if (iconClass && !iconClass.includes('fa-duotone')) {
                             iconClass = iconClass.replace(/fa-(regular|solid|light)/g, 'fa-duotone');
                         }
-                        return '<div style="display: flex; align-items: center; gap: 8px;">' +
-                            '<i class="' + iconClass + '" style="font-size: 16px; width: 20px; --fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i>' +
-                            '<span style="color: #5f6b7a;">' + data.text + '</span>' +
-                            '</div>';
-                    };
-
-                    ddlIconeObj.valueTemplate = function (data) {
-                        if (!data || data.isCategory) return '';
-
-                        var iconClass = data.id || '';
-                        if (iconClass && !iconClass.includes('fa-duotone')) {
-                            iconClass = iconClass.replace(/fa-(regular|solid|light)/g, 'fa-duotone');
-                        }
-                        return '<div style="display: flex; align-items: center; gap: 8px;">' +
-                            '<i class="' + iconClass + '" style="font-size: 14px; width: 18px; --fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i>' +
-                            '<span style="color: #5f6b7a;">' + data.text + '</span>' +
-                            '</div>';
-                    };
-
-                    ddlIconeObj.selecting = function (args) {
-                        if (args.nodeData && !args.nodeData.isCategory) {
-                            ultimoIconeSelecionado = args.nodeData.id;
-                            var txtIconClass = document.getElementById('txtIconClass');
-                            if (txtIconClass) {
-                                txtIconClass.value = args.nodeData.id;
-                            }
-                        }
-                    };
-
-                    ddlIconeObj.select = function (args) {
-                        console.log('=== SELECT ícone ===', args);
-                        if (args.nodeData && !args.nodeData.isCategory) {
-                            ultimoIconeSelecionado = args.nodeData.id;
-                            var txtIconClass = document.getElementById('txtIconClass');
-                            if (txtIconClass) {
-                                txtIconClass.value = args.nodeData.id;
-                            }
-
-                            ddlIconeObj.value = [args.nodeData.id];
-                            ddlIconeObj.dataBind();
-                            setTimeout(function () {
-                                ddlIconeObj.hidePopup();
-
-                                onIconeChange({ itemData: args.nodeData, value: [args.nodeData.id] });
-
-                                atualizarEstadoBotoesOrdenacao();
-                            }, 50);
-                        }
-                    };
-
-                    ddlIconeObj.change = function (args) {
-                        onIconeChange(args);
-                    };
-
-                    ddlIconeObj.changeOnBlur = false;
-
-                    console.log('Templates e eventos de ícone configurados');
-                }
-            } catch (error) {
-                console.error('Erro ao configurar template do DropDownTree:', error);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES DE CODIFICAÇÃO/DECODIFICAÇÃO HTML
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Funções para tratar caracteres especiais e acentos
-            * Necessário para gravação correta no arquivo JSON de navegação
-                */
-
-            /**
-             * Decodifica entidades HTML para caracteres normais
-             * @@description Usado para exibição na interface
-            * @@param { string } html - Texto com entidades HTML
-                * @@returns { string } Texto decodificado
-                    */
-        function decodeHtml(html) {
-            try {
-                if (!html) return html;
-                var txt = document.createElement("textarea");
-                txt.innerHTML = html;
-                return txt.value;
-            } catch (erro) {
-                console.error('[decodeHtml] Erro:', erro);
-                return html;
-            }
-        }
-
-            /**
-             * Codifica TODOS caracteres especiais e acentuados para entidades HTML
-             * @@description Usado para gravação no arquivo JSON
-            * @@param { string } text - Texto a ser codificado
-                * @@returns { string } Texto com entidades HTML
-                    */
-        function encodeHtml(text) {
-            try {
-                if (!text) return text;
-
-                var result = '';
-                for (var i = 0; i < text.length; i++) {
-                    var char = text[i];
-                    var code = char.charCodeAt(0);
-
-                    if (code >= 32 && code <= 126 && char !== '<' && char !== '>' && char !== '&' && char !== '"' && char !== "'") {
-                        result += char;
-                    } else {
-
-                        result += '&#' + code + ';';
-                    }
-                }
-                return result;
-            } catch (erro) {
-                console.error('[encodeHtml] Erro:', erro);
-                return text;
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * INICIALIZAÇÃO DA PÁGINA
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Carrega todos os dados necessários ao iniciar a página
-            * - Árvore de navegação(do banco de dados)
-             * - Ícones FontAwesome disponíveis
-            * - Páginas do sistema para mapeamento
-                */
-        document.addEventListener('DOMContentLoaded', function () {
-            carregarArvore();
-            carregarIconesFontAwesome();
-            carregarPaginasSistema();
-
-            configurarBotoesAlteracoes();
-        });
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * DROPDOWN DE ITEM PAI - HIERARQUIA DO MENU
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Popula dropdown com todos os grupos disponíveis
-             * @@description Extrai grupos do treeData recursivamente e formata
-            * para exibição hierárquica com indentação visual
-                * @@returns { void}
-                */
-        function popularDropdownItemPai() {
-            try {
-                var grupos = [];
-
-                grupos.push({
-                    id: '_SEM_GRUPO_',
-                    text: '<sem grupo>',
-                    textFormatado: '<sem grupo>',
-                    textoSemIndentacao: '<sem grupo>',
-                    nivel: -1
-                });
-
-                    /**
-                     * Função recursiva para extrair grupos
-                     * @@param { Array } items - Lista de itens a processar
-                    */
-                function extrairGrupos(items) {
-                    if (!items) return;
-
-                    items.forEach(function (item) {
-
-                        var isGrupo = !item.href || item.href === 'javascript:void(0);';
-
-                        if (isGrupo) {
-                            grupos.push({
-                                id: item.id,
-                                text: item.text,
-                                nivel: calcularNivel(item.id)
-                            });
-                        }
-
-                        if (item.items && item.items.length > 0) {
-                            extrairGrupos(item.items);
-                        }
-                    });
-                }
-
-                    /**
-                     * Calcula nível hierárquico do item
-                     * @@param { string } itemId - ID do item
-                    * @@returns { number } Nível de profundidade(0 = raiz)
-                        */
-                function calcularNivel(itemId) {
-                    var nivel = 0;
-                    var item = buscarItemPorId(treeData, itemId);
-
-                    while (item && item.parentId) {
-                        nivel++;
-                        item = buscarItemPorId(treeData, item.parentId);
-                    }
-
-                    return nivel;
-                }
-
-                extrairGrupos(treeData);
-
-                grupos.forEach(function (grupo) {
-                    if (grupo.id === '_SEM_GRUPO_') {
-
-                        return;
-                    } else {
-                        var textoDecodificado = decodeHtml(grupo.text);
-                        var indentacao = ' '.repeat(grupo.nivel);
-                        grupo.textFormatado = indentacao + textoDecodificado;
-                        grupo.textoSemIndentacao = textoDecodificado;
-                    }
-                });
-
-                grupos.sort(function (a, b) {
-                    if (a.id === '_SEM_GRUPO_') return -1;
-                    if (b.id === '_SEM_GRUPO_') return 1;
-
-                    return a.textoSemIndentacao.localeCompare(b.textoSemIndentacao, 'pt-BR', { sensitivity: 'base' });
-                });
-
-                console.log('Grupos encontrados:', grupos.length - 1, '+ <sem grupo>');
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "popularDropdownItemPai", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CONTROLES DE POSIÇÃO E TIPO DE ITEM
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Popula dropdown "Posicionar abaixo de..."
-             * @@description Exibe lista hierárquica de todos os itens do menu
-            * para definir a posição de novos itens
-                * @@returns { void}
-                */
-        function popularDropdownPosicao() {
-            try {
-                var itens = [];
-
-                itens.push({
-                    id: '_INICIO_',
-                    text: '📍 No início (primeiro item)',
-                    hasChildren: false
-                });
-
-                    /**
-                     * Função recursiva para extrair itens com hierarquia visual
-                     * @@param { Array } lista - Lista de itens
-                    * @@param { number } nivel - Nível de indentação atual
-                        */
-                function extrairItensHierarquicos(lista, nivel) {
-                    lista.forEach(function (item) {
-                        var indentacao = ' '.repeat(nivel);
-                        var icone = (item.items && item.items.length > 0) ? '📁' : '📄';
-
-                        itens.push({
-                            id: item.id,
-                            text: indentacao + icone + ' ' + decodeHtml(item.text),
-                            hasChildren: false
-                        });
-
-                        if (item.items && item.items.length > 0) {
-                            extrairItensHierarquicos(item.items, nivel + 1);
-                        }
-                    });
-                }
-
-                extrairItensHierarquicos(treeData, 0);
-
-                var ddlPosicao = document.getElementById('ddlPosicao');
-                if (ddlPosicao && ddlPosicao.ej2_instances && ddlPosicao.ej2_instances[0]) {
-                    var ddl = ddlPosicao.ej2_instances[0];
-                    ddl.fields = {
-                        dataSource: itens,
-                        value: 'id',
-                        text: 'text',
-                        hasChildren: 'hasChildren'
-                    };
-                    ddl.dataBind();
-                    console.log('Dropdown de posição populado com', itens.length, 'itens');
-                }
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "popularDropdownPosicao", erro);
-            }
-        }
-
-            /**
-             * Alterna tipo do item entre Página e Grupo
-             * @@description Atualiza UI de toggle de tipo e campos relacionados
-            * @@param { string } tipo - 'pagina' ou 'grupo'
-                * @@returns { void}
-                */
-        function selecionarTipoItem(tipo) {
-            try {
-                var btnPagina = document.getElementById('btnTipoPagina');
-                var btnGrupo = document.getElementById('btnTipoGrupo');
-                var tipoItemInput = document.getElementById('tipoItem');
-                var rowPagina = document.getElementById('rowPaginaSistema');
-
-                if (tipo === 'pagina') {
-                    btnPagina.classList.add('active');
-                    btnGrupo.classList.remove('active');
-                    tipoItemInput.value = 'pagina';
-
-                    if (rowPagina) rowPagina.style.display = 'block';
-
-                } else {
-                    btnPagina.classList.remove('active');
-                    btnGrupo.classList.add('active');
-                    tipoItemInput.value = 'grupo';
-
-                    if (rowPagina) rowPagina.style.display = 'none';
-
-                    document.getElementById('txtHref').value = '';
-                }
-
-                console.log('[TIPO] Tipo de item alterado para:', tipo);
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "selecionarTipoItem", erro);
-            }
-        }
-
-            /**
-             * Evento de mudança de posição no dropdown
-             * @@param { Object } args - Argumentos do evento Syncfusion
-            * @@returns { void}
-            */
-            function onPosicaoChange(args) {
-            try {
-                console.log('[POSIÇÃO] Nova posição selecionada:', args.value);
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "onPosicaoChange", erro);
-            }
-        }
-
-            /**
-             * Atualiza toggle de tipo baseado no item selecionado
-             * @@param { Object } item - Item da árvore selecionado
-            * @@returns { void}
-            */
-        function atualizarToggleTipo(item) {
-            if (!item) return;
-
-            var ehGrupo = !item.href || item.href === 'javascript:void(0);' || (item.items && item.items.length > 0);
-
-            if (ehGrupo) {
-                selecionarTipoItem('grupo');
-            } else {
-                selecionarTipoItem('pagina');
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * TRANSFORMAÇÃO DE TIPO (GRUPO ↔ PÁGINA)
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Alterna tipo do item entre Grupo e Página
-             * @@description Abre modal apropriado para confirmar transformação
-            * Se Grupo→Página: pede para selecionar URL
-                * Se Página→Grupo: pergunta o que fazer com itens abaixo
-                    * @@param { string } itemId - ID do item a transformar
-                        * @@param { Event } event - Evento de clique
-                            * @@returns { void}
-                            */
-        function alternarTipoItem(itemId, event) {
-            try {
-                console.log('[TOGGLE-TIPO] Alternando tipo do item:', itemId);
-
-                var item = buscarItemPorId(treeData, itemId);
-                if (!item) {
-                    console.error('[TOGGLE-TIPO] Item não encontrado:', itemId);
-                    return;
-                }
-
-                var nomeItem = decodeHtml(item.text);
-                var ehGrupoAtual = !item.href || item.href === 'javascript:void(0);';
-
-                if (ehGrupoAtual) {
-
-                    var htmlPaginas = montarSelectPaginasParaSwal(itemId);
-
-                    var mensagemBase = 'O Grupo <strong>"' + nomeItem + '"</strong> será transformado em uma <strong>Página</strong>.<br><br>';
-
-                    if (item.items && item.items.length > 0) {
-                        mensagemBase += '<div class="alert alert-warning" style="text-align:left; font-size:0.9rem; margin-bottom:15px;">' +
-                            '<i class="fa-duotone fa-triangle-exclamation"></i> ' +
-                            'Este grupo possui <strong>' + item.items.length + '</strong> filho(s).<br>' +
-                            'Eles serão movidos para o nível acima.' +
-                            '</div>';
-                    }
-
-                    mostrarModalTransformacaoGrupoEmPagina('Transformar Grupo em Página', mensagemBase, htmlPaginas)
-                        .then(function (result) {
-                            if (result.confirmado && result.paginaUrl) {
-                                executarTransformacaoGrupoEmPagina(item, result.paginaUrl);
-                            }
-                        });
-
-                } else {
-
-                    var irmaosAbaixo = encontrarIrmaosAbaixo(item);
-
-                    var mensagem = 'A Página <strong>"' + nomeItem + '"</strong> será transformada em <strong>Grupo</strong>.<br><br>' +
-                        '<div class="alert alert-info" style="text-align:left; font-size:0.9rem;">' +
-                        '<i class="fa-duotone fa-info-circle"></i> A URL da página será removida.' +
-                        '</div>';
-
-                    if (irmaosAbaixo.length > 0) {
-
-                        mostrarModalTransformacaoPaginaEmGrupo(item, nomeItem, irmaosAbaixo);
-
-                    } else {
-
-                        Alerta.Confirmar(
-                            'Transformar Página em Grupo',
-                            mensagem,
-                            'Sim, Transformar',
-                            'Cancelar'
-                        ).then(function (confirmado) {
-                            if (confirmado) {
-                                executarTransformacaoPaginaEmGrupo(item, false, [], null);
-                            }
-                        });
-                    }
-                }
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "alternarTipoItem", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * BUSCA DE ÍCONES POR URL/TEXTO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Busca ícone personalizado na treeData por URL/href
-             * @@description Procura item que corresponda à URL e retorna seu ícone
-            * Usa múltiplas estratégias de comparação para maior flexibilidade
-                * @@param { Array } items - Lista de itens para buscar
-                    * @@param { string } url - URL a ser buscada
-                        * @@returns { string| null} Classe do ícone ou null
-                            */
-        function buscarIconePorUrl(items, url) {
-            try {
-                if (!items || !url) return null;
-
-                var normalizarUrl = function (u) {
-                    if (!u) return '';
-                    u = u.toLowerCase().trim();
-                    u = u.replace(/^\
-                    u = u.replace(/\.html$/, '');
-                    u = u.replace(/\.cshtml$/, '');
-                    u = u.replace(/_/g, '');
-                    u = u.replace(/\
-                    return u;
-                };
-
-                var extrairPartesUrl = function (u) {
-                    if (!u) return { modulo: '', pagina: '', completa: '' };
-                    u = u.toLowerCase().trim();
-                    var partes = u.replace(/\.(html|cshtml)$/, '').split(/[\/_]/);
-                    return {
-                        modulo: partes[0] || '',
-                        pagina: partes[1] || partes[0] || '',
-                        completa: u.replace(/\.(html|cshtml)$/, '').replace(/[\/_]/g, '')
-                    };
-                };
-
-                var urlNormalizada = normalizarUrl(url);
-                var partesUrl = extrairPartesUrl(url);
-
-                for (var i = 0; i < items.length; i++) {
-                    var item = items[i];
-
-                    if (item.href) {
-                        var hrefNormalizado = normalizarUrl(item.href);
-                        var partesHref = extrairPartesUrl(item.href);
-
-                        var matchExato = hrefNormalizado === urlNormalizada;
-
-                        var matchPartes = partesHref.completa === partesUrl.completa ||
-                            (partesHref.modulo === partesUrl.modulo && partesHref.pagina === partesUrl.pagina);
-
-                        var matchFlexivel = hrefNormalizado.includes(urlNormalizada) || urlNormalizada.includes(hrefNormalizado);
-
-                        if ((matchExato || matchPartes || matchFlexivel) && item.icon) {
-
-                            var iconClass = item.icon || '';
-                            if (iconClass && !iconClass.includes('fa-duotone')) {
+                        return iconClass || null;
+                    }
+                }
+
+                if (item.items && item.items.length > 0) {
+                    var found = buscarIconePorUrl(item.items, url);
+                    if (found) return found;
+                }
+            }
+
+            return null;
+        } catch (erro) {
+            console.error('[buscarIconePorUrl]', erro);
+            return null;
+        }
+    }
+
+    function buscarItemPorTexto(items, texto) {
+        try {
+            if (!items || !texto) return null;
+
+            var textoBusca = (texto || '').toLowerCase().trim();
+
+            for (var i = 0; i < items.length; i++) {
+                var item = items[i];
+                var textoItem = (item.text || '').toLowerCase().trim();
+
+                if (textoItem === textoBusca) {
+                    return item;
+                }
+
+                if (item.items && item.items.length > 0) {
+                    var found = buscarItemPorTexto(item.items, texto);
+                    if (found) return found;
+                }
+            }
+
+            return null;
+        } catch (erro) {
+            console.error('[buscarItemPorTexto]', erro);
+            return null;
+        }
+    }
+
+    function montarSelectPaginasParaSwal(itemIdExcluir) {
+        try {
+            var ddlPaginaObj = document.getElementById('ddlPagina');
+            if (!ddlPaginaObj || !ddlPaginaObj.ej2_instances || !ddlPaginaObj.ej2_instances[0]) {
+                return '<p style="color:#ff6b35;">Erro ao carregar páginas</p>';
+            }
+
+            var ddl = ddlPaginaObj.ej2_instances[0];
+            var dataSource = ddl.fields && ddl.fields.dataSource ? ddl.fields.dataSource : [];
+
+            if (!dataSource || dataSource.length === 0) {
+                return '<p style="color:#ff6b35; padding:20px; text-align:center;">⚠️ Nenhuma página disponível. Aguarde o carregamento das páginas do sistema.</p>';
+            }
+
+            var html = '<div id="swalListaPaginas" style="max-height:300px; overflow-y:auto; border:2px solid #3D5771; border-radius:6px; background:#1e1e2f;">';
+
+            var totalOpcoes = 0;
+
+            function adicionarOpcoes(items, nivel) {
+                items.forEach(function(item) {
+                    var indentPx = nivel * 20;
+                    var temFilhos = item.child && item.child.length > 0;
+
+                    var isCategory = item.isCategory || (temFilhos && !item.paginaRef);
+
+                    var paginaUrl = item.paginaRef || item.url || '';
+
+                    var iconClass = '';
+                    if (isCategory) {
+
+                        var grupoEncontrado = buscarItemPorTexto(treeData || [], item.text);
+                        if (grupoEncontrado && grupoEncontrado.icon) {
+                            iconClass = grupoEncontrado.icon;
+
+                            if (!iconClass.includes('fa-duotone')) {
                                 iconClass = iconClass.replace(/fa-(regular|solid|light)/g, 'fa-duotone');
                             }
-                            return iconClass || null;
+                        } else {
+
+                            iconClass = 'fa-duotone fa-folder-tree';
                         }
-                    }
-
-                    if (item.items && item.items.length > 0) {
-                        var found = buscarIconePorUrl(item.items, url);
-                        if (found) return found;
-                    }
-                }
-
-                return null;
-            } catch (erro) {
-                console.error('[buscarIconePorUrl]', erro);
-                return null;
-            }
-        }
-
-            /**
-             * Busca item na treeData por texto/nome
-             * @@description Usado para encontrar grupos / categorias pelo nome
-            * @@param { Array } items - Lista de itens para buscar
-                * @@param { string } texto - Texto a ser buscado
-                    * @@returns { Object| null} Item encontrado ou null
-                        */
-        function buscarItemPorTexto(items, texto) {
-            try {
-                if (!items || !texto) return null;
-
-                var textoBusca = (texto || '').toLowerCase().trim();
-
-                for (var i = 0; i < items.length; i++) {
-                    var item = items[i];
-                    var textoItem = (item.text || '').toLowerCase().trim();
-
-                    if (textoItem === textoBusca) {
-                        return item;
-                    }
-
-                    if (item.items && item.items.length > 0) {
-                        var found = buscarItemPorTexto(item.items, texto);
-                        if (found) return found;
-                    }
-                }
-
-                return null;
-            } catch (erro) {
-                console.error('[buscarItemPorTexto]', erro);
-                return null;
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * MODAL PARA SELEÇÃO DE PÁGINAS DO SISTEMA
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Monta HTML do select de páginas com ícones
-             * @@description Usado no modal de transformação Grupo→Página
-            * @@param { string } itemIdExcluir - ID do item a excluir da lista
-                * @@returns { string } HTML do select ou mensagem de erro
-                    */
-        function montarSelectPaginasParaSwal(itemIdExcluir) {
-            try {
-                var ddlPaginaObj = document.getElementById('ddlPagina');
-                if (!ddlPaginaObj || !ddlPaginaObj.ej2_instances || !ddlPaginaObj.ej2_instances[0]) {
-                    return '<p style="color:#ff6b35;">Erro ao carregar páginas</p>';
-                }
-
-                var ddl = ddlPaginaObj.ej2_instances[0];
-                var dataSource = ddl.fields && ddl.fields.dataSource ? ddl.fields.dataSource : [];
-
-                if (!dataSource || dataSource.length === 0) {
-                    return '<p style="color:#ff6b35; padding:20px; text-align:center;">⚠️ Nenhuma página disponível. Aguarde o carregamento das páginas do sistema.</p>';
-                }
-
-                var html = '<div id="swalListaPaginas" style="max-height:300px; overflow-y:auto; border:2px solid #3D5771; border-radius:6px; background:#1e1e2f;">';
-
-                var totalOpcoes = 0;
-
-                function adicionarOpcoes(items, nivel) {
-                    items.forEach(function (item) {
-                        var indentPx = nivel * 20;
-                        var temFilhos = item.child && item.child.length > 0;
-
-                        var isCategory = item.isCategory || (temFilhos && !item.paginaRef);
-
-                        var paginaUrl = item.paginaRef || item.url || '';
-
-                        var iconClass = '';
-                        if (isCategory) {
-
-                            var grupoEncontrado = buscarItemPorTexto(treeData || [], item.text);
-                            if (grupoEncontrado && grupoEncontrado.icon) {
-                                iconClass = grupoEncontrado.icon;
-
-                                if (!iconClass.includes('fa-duotone')) {
-                                    iconClass = iconClass.replace(/fa-(regular|solid|light)/g, 'fa-duotone');
-                                }
-                            } else {
-
-                                iconClass = 'fa-duotone fa-folder-tree';
-                            }
+                    } else {
+
+                        var iconPersonalizado = buscarIconePorUrl(treeData || [], paginaUrl);
+                        if (iconPersonalizado) {
+                            iconClass = iconPersonalizado;
                         } else {
 
-                            var iconPersonalizado = buscarIconePorUrl(treeData || [], paginaUrl);
-                            if (iconPersonalizado) {
-                                iconClass = iconPersonalizado;
-                            } else {
-
-                                iconClass = 'fa-duotone fa-file-lines';
-                            }
+                            iconClass = 'fa-duotone fa-file-lines';
                         }
-
-                        var podeSelecionar = !isCategory && paginaUrl;
-
-                        html += '<div class="swal-item-pagina' + (isCategory ? ' swal-item-categoria' : ' swal-item-pagina-sel') + '" ' +
+                    }
+
+                    var podeSelecionar = !isCategory && paginaUrl;
+
+                    html += '<div class="swal-item-pagina' + (isCategory ? ' swal-item-categoria' : ' swal-item-pagina-sel') + '" ' +
                             'data-itemid="' + item.id + '" ' +
                             'data-url="' + paginaUrl + '" ' +
                             'data-iscategory="' + (isCategory ? 'true' : 'false') + '" ' +
@@ -1825,3339 +1603,2969 @@
                             '<span style="flex:1;">' + decodeHtml(item.text) + '</span>' +
                             (podeSelecionar ? '' : '<i class="fa-duotone fa-ban" style="font-size:14px; flex-shrink:0; --fa-primary-color:#ff4444; --fa-secondary-color:#ff8888;"></i>') +
                             '</div>';
-                        totalOpcoes++;
-
-                        if (temFilhos) {
-                            adicionarOpcoes(item.child, nivel + 1);
+                    totalOpcoes++;
+
+                    if (temFilhos) {
+                        adicionarOpcoes(item.child, nivel + 1);
+                    }
+                });
+            }
+
+            adicionarOpcoes(dataSource, 0);
+            html += '</div>';
+
+            html += '<input type="hidden" id="swalPaginaSelecionada" value="" />';
+            html += '<input type="hidden" id="swalUrlSelecionada" value="" />';
+
+            return html;
+
+        } catch (erro) {
+            console.error('[SWAL-SELECT]', erro);
+            return '<p style="color:#ff6b35;">Erro ao carregar páginas</p>';
+        }
+    }
+
+    function mostrarModalTransformacaoGrupoEmPagina(titulo, mensagem, htmlSelect) {
+        return new Promise(function(resolve) {
+            var iconHtml = '<img src="/images/confirmar_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
+
+            var msg = `
+            <div style="background:#1e1e2f; border-radius: 8px; overflow: hidden; font-family: 'Segoe UI', sans-serif; color: #e0e0e0;">
+              <div style="background:#2d2d4d; padding: 20px; text-align: center;">
+                <div style="margin-bottom: 10px;">
+                  <div style="display: inline-block; max-width: 200px; width: 100%;">
+                    ${iconHtml}
+                  </div>
+                </div>
+                <div style="font-size: 20px; color: #c9a8ff; font-weight: bold;">${titulo}</div>
+              </div>
+
+              <div style="padding: 20px; font-size: 15px; line-height: 1.6; text-align: center; background:#1e1e2f">
+                <div style="margin-bottom: 20px;">${mensagem}</div>
+                <div style="margin-top: 15px; text-align: left;">
+                  <label style="font-weight:600; margin-bottom:8px; display:block; color:#e0e0e0;">Selecione a Página do Sistema:</label>
+                  ${htmlSelect}
+                </div>
+                <div id="swalMensagemErro" style="display:none; margin-top:15px; background:linear-gradient(135deg, #ff6b35, #e55a2b); color:white; padding:10px 15px; border-radius:8px; border:2px solid rgba(255,255,255,0.5); text-align:center;">
+                  <i class="fa-duotone fa-triangle-exclamation" style="--fa-primary-color:#ffffff; --fa-secondary-color:#ffcc00; margin-right:8px;"></i>
+                  <strong>Selecione uma página</strong> clicando em um item da lista acima!
+                </div>
+              </div>
+
+              <div style="background:#3b3b5c; padding: 15px; text-align: center;">
+                <button id="btnCancelTransform" style="
+                  background: #722F37;
+                  border: none;
+                  color: #fff;
+                  padding: 10px 20px;
+                  margin-right: 10px;
+                  font-size: 14px;
+                  border-radius: 5px;
+                  cursor: pointer;
+                  transition: background 0.3s;
+                " onmouseover="this.style.background='#5a252b'" onmouseout="this.style.background='#722F37'">
+                  <i class="fa-duotone fa-circle-xmark" style="--fa-primary-color:#fff; --fa-secondary-color:#FFCCCB; margin-right: 5px;"></i>Cancelar
+                </button>
+
+                <button id="btnConfirmTransform" style="
+                  background: #3D5771;
+                  border: none;
+                  color: #fff;
+                  padding: 10px 20px;
+                  font-size: 14px;
+                  border-radius: 5px;
+                  cursor: pointer;
+                  transition: background 0.3s;
+                " onmouseover="this.style.background='#2d4154'" onmouseout="this.style.background='#3D5771'">
+                  <i class="fa-duotone fa-arrow-right-arrow-left" style="--fa-primary-color:#fff; --fa-secondary-color:#a0c4e8; margin-right: 5px;"></i>Transformar
+                </button>
+              </div>
+            </div>`;
+
+            Swal.fire({
+                showConfirmButton: false,
+                html: msg,
+                backdrop: true,
+                heightAuto: false,
+                allowOutsideClick: false,
+                allowEscapeKey: true,
+                focusConfirm: false,
+                customClass: {
+                    popup: 'swal2-popup swal2-no-border swal2-no-shadow'
+                },
+                didOpen: function() {
+                    var popup = document.querySelector('.swal2-popup');
+                    if (popup) {
+                        popup.style.border = 'none';
+                        popup.style.boxShadow = 'none';
+                        popup.style.background = 'transparent';
+                    }
+
+                    setTimeout(function() {
+                        var listaPaginas = document.getElementById('swalListaPaginas');
+                        if (listaPaginas) {
+                            listaPaginas.addEventListener('click', function(e) {
+                                e.preventDefault();
+                                e.stopPropagation();
+
+                                var target = e.target;
+                                var item = null;
+
+                                while (target && target !== listaPaginas) {
+                                    if (target.classList && target.classList.contains('swal-item-pagina')) {
+                                        item = target;
+                                        break;
+                                    }
+                                    target = target.parentElement;
+                                }
+
+                                if (!item) {
+                                    return;
+                                }
+
+                                var podeSelecionar = item.dataset.podeselecionar === 'true';
+                                var isCategory = item.dataset.iscategory === 'true';
+
+                                if (!podeSelecionar || isCategory || !item.dataset.itemid) {
+                                    return;
+                                }
+
+                                document.querySelectorAll('.swal-item-pagina').forEach(function(el) {
+                                    el.style.background = 'transparent';
+                                    el.style.borderLeft = 'none';
+                                    el.classList.remove('swal-item-selected');
+                                    delete el.dataset.selected;
+                                });
+
+                                item.style.background = '#3D5771';
+                                item.style.borderLeft = '3px solid #ff6b35';
+                                item.classList.add('swal-item-selected');
+                                item.dataset.selected = 'true';
+
+                                var mensagemErro = document.getElementById('swalMensagemErro');
+                                if (mensagemErro) {
+                                    mensagemErro.style.display = 'none';
+                                }
+
+                                var paginaIdInput = document.getElementById('swalPaginaSelecionada');
+                                var urlInput = document.getElementById('swalUrlSelecionada');
+
+                                if (!paginaIdInput || !urlInput) {
+                                    return;
+                                }
+
+                                paginaIdInput.value = item.dataset.itemid;
+                                urlInput.value = item.dataset.url || '';
+                            }, true);
+
+                            listaPaginas.addEventListener('mouseover', function(e) {
+                                var target = e.target;
+                                var item = null;
+                                while (target && target !== listaPaginas) {
+                                    if (target.classList && target.classList.contains('swal-item-pagina')) {
+                                        item = target;
+                                        break;
+                                    }
+                                    target = target.parentElement;
+                                }
+                                if (item && item.dataset.podeselecionar === 'true' && item.dataset.selected !== 'true') {
+                                    item.style.background = '#2d2d4d';
+                                }
+                            }, true);
+
+                            listaPaginas.addEventListener('mouseout', function(e) {
+                                var target = e.target;
+                                var item = null;
+                                while (target && target !== listaPaginas) {
+                                    if (target.classList && target.classList.contains('swal-item-pagina')) {
+                                        item = target;
+                                        break;
+                                    }
+                                    target = target.parentElement;
+                                }
+                                if (item && item.dataset.selected !== 'true') {
+                                    item.style.background = 'transparent';
+                                }
+                            }, true);
+                        }
+                    }, 100);
+
+                    var confirmBtn = document.getElementById('btnConfirmTransform');
+                    if (confirmBtn) {
+                        confirmBtn.onclick = function() {
+                            var paginaIdInput = document.getElementById('swalPaginaSelecionada');
+                            var urlInput = document.getElementById('swalUrlSelecionada');
+                            var mensagemErro = document.getElementById('swalMensagemErro');
+
+                            if (!paginaIdInput || !paginaIdInput.value || !urlInput || !urlInput.value) {
+
+                                if (mensagemErro) {
+                                    mensagemErro.style.display = 'block';
+
+                                    mensagemErro.scrollIntoView({ behavior: 'smooth', block: 'center' });
+                                }
+                                return;
+                            }
+
+                            var paginaId = paginaIdInput.value;
+                            var paginaUrl = urlInput.value;
+
+                            Swal.close();
+                            resolve({ confirmado: true, paginaId: paginaId, paginaUrl: paginaUrl });
+                        };
+                    }
+
+                    var cancelBtn = document.getElementById('btnCancelTransform');
+                    if (cancelBtn) {
+                        cancelBtn.onclick = function() {
+                            Swal.close();
+                            resolve({ confirmado: false });
+                        };
+                    }
+                }
+            });
+        });
+    }
+
+    function encontrarIrmaosAbaixo(item) {
+        try {
+            var lista = item.parentId ? null : treeData;
+
+            if (item.parentId) {
+                var pai = buscarItemPorId(treeData, item.parentId);
+                lista = pai ? pai.items : null;
+            }
+
+            if (!lista) return [];
+
+            var indexAtual = lista.findIndex(function(i) { return i.id === item.id; });
+            if (indexAtual === -1 || indexAtual >= lista.length - 1) return [];
+
+            return lista.slice(indexAtual + 1);
+
+        } catch (erro) {
+            console.error('[IRMAOS-ABAIXO]', erro);
+            return [];
+        }
+    }
+
+    function executarTransformacaoGrupoEmPagina(item, novaUrl) {
+        try {
+
+            var itemId = item.id;
+
+            if (item.items && item.items.length > 0) {
+                var paiId = item.parentId;
+                var listaDestino = paiId ? null : treeData;
+
+                if (paiId) {
+                    var pai = buscarItemPorId(treeData, paiId);
+                    listaDestino = pai ? pai.items : null;
+                }
+
+                if (listaDestino) {
+                    var indexItem = listaDestino.findIndex(function(i) { return i.id === item.id; });
+
+                    item.items.forEach(function(filho, idx) {
+                        filho.parentId = paiId || null;
+                        listaDestino.splice(indexItem + 1 + idx, 0, filho);
+                    });
+                }
+
+                item.items = [];
+            }
+
+            item.href = novaUrl;
+
+            treeData = JSON.parse(JSON.stringify(treeData));
+
+            atualizarTreeViewAposMovimento(null, false);
+
+            salvarOrdenacaoAutomaticaSemReload().then(function() {
+
+                selectedItem = null;
+                if (treeObj) {
+                    treeObj.selectedNodes = [];
+                }
+                limparFormulario();
+
+                Alerta.Sucesso('Transformação Realizada', 'O grupo foi transformado em página e salvo com sucesso!');
+
+                setTimeout(function() {
+                    window.location.reload();
+                }, 1500);
+            }).catch(function(erro) {
+                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "executarTransformacaoGrupoEmPagina", erro);
+            });
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "executarTransformacaoGrupoEmPagina", erro);
+        }
+    }
+
+    function mostrarModalTransformacaoPaginaEmGrupo(item, nomeItem, irmaosAbaixo) {
+        try {
+
+            var listaItensHtml = irmaosAbaixo.slice(0, 5).map(function(irmao) {
+                return '<div style="padding:4px 0; border-bottom:1px solid rgba(255,255,255,0.1);">• ' + decodeHtml(irmao.text) + '</div>';
+            }).join('');
+
+            if (irmaosAbaixo.length > 5) {
+                listaItensHtml += '<div style="padding:4px 0; font-style:italic; color:#aaa;">... e mais ' + (irmaosAbaixo.length - 5) + ' item(ns)</div>';
+            }
+
+            var htmlModal = `
+            <div id="modalTransformPaginaGrupo" style="
+                position: fixed;
+                top: 0;
+                left: 0;
+                width: 100%;
+                height: 100%;
+                background: rgba(0,0,0,0.7);
+                display: flex;
+                align-items: center;
+                justify-content: center;
+                z-index: 99999;
+            ">
+                <div style="
+                    background: linear-gradient(135deg, #1e1e2f 0%, #2d2d4d 100%);
+                    border-radius: 12px;
+                    max-width: 500px;
+                    width: 90%;
+                    box-shadow: 0 10px 40px rgba(0,0,0,0.5);
+                    overflow: hidden;
+                    font-family: 'Segoe UI', sans-serif;
+                ">
+
+                    <div style="background:#2d2d4d; padding:25px; text-align:center; border-bottom:1px solid rgba(255,255,255,0.1);">
+                        <div style="font-size:50px; margin-bottom:10px;">🤔</div>
+                        <div style="font-size:22px; color:#c9a8ff; font-weight:bold;">Transformar Página em Grupo</div>
+                    </div>
+
+                    <div style="padding:20px; color:#e0e0e0;">
+                        <p style="text-align:center; margin-bottom:15px;">
+                            A Página <strong style="color:#ff6b35;">"${decodeHtml(nomeItem)}"</strong> será transformada em <strong>Grupo</strong>.
+                        </p>
+
+                        <div style="
+                            background: rgba(255,107,53,0.2);
+                            border: 1px solid rgba(255,255,255,0.3);
+                            border-radius: 8px;
+                            padding: 12px 15px;
+                            margin-bottom: 20px;
+                            color: #fff;
+                            font-size: 0.9rem;
+                        ">
+                            <i class="fa-duotone fa-info-circle" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#fff; margin-right:8px;"></i>
+                            A URL da página será removida.
+                        </div>
+
+                        <div style="text-align:left;">
+                            <strong style="color:#c9a8ff;">Existem ${irmaosAbaixo.length} item(ns) abaixo desta página:</strong>
+                            <div style="
+                                margin-top:10px;
+                                max-height:150px;
+                                overflow-y:auto;
+                                background:rgba(0,0,0,0.2);
+                                border-radius:6px;
+                                padding:10px;
+                            ">
+                                ${listaItensHtml}
+                            </div>
+                        </div>
+
+                        <p style="text-align:center; margin-top:20px; font-weight:600; color:#c9a8ff;">
+                            O que deseja fazer com estes itens?
+                        </p>
+                    </div>
+
+                    <div style="background:#3b3b5c; padding:20px; display:flex; flex-wrap:wrap; gap:10px; justify-content:center;">
+
+                        <button id="btnSubordinarNovoGrupo" style="
+                            background: #4a5d23;
+                            border: none;
+                            color: #fff;
+                            padding: 12px 16px;
+                            font-size: 13px;
+                            border-radius: 6px;
+                            cursor: pointer;
+                            transition: all 0.3s;
+                            flex: 1 1 45%;
+                            min-width: 180px;
+                        " onmouseover="this.style.background='#3d4d1c'" onmouseout="this.style.background='#4a5d23'">
+                            <i class="fa-duotone fa-folder-tree" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
+                            Subordinar ao Novo Grupo
+                        </button>
+
+                        <button id="btnSubordinarOutroGrupo" style="
+                            background: #68432C;
+                            border: none;
+                            color: #fff;
+                            padding: 12px 16px;
+                            font-size: 13px;
+                            border-radius: 6px;
+                            cursor: pointer;
+                            transition: all 0.3s;
+                            flex: 1 1 45%;
+                            min-width: 180px;
+                        " onmouseover="this.style.background='#523522'" onmouseout="this.style.background='#68432C'">
+                            <i class="fa-duotone fa-folder-open" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
+                            Subordinar a Outro Grupo
+                        </button>
+
+                        <button id="btnManterOndeEstao" style="
+                            background: #154c62;
+                            border: none;
+                            color: #fff;
+                            padding: 12px 16px;
+                            font-size: 13px;
+                            border-radius: 6px;
+                            cursor: pointer;
+                            transition: all 0.3s;
+                            flex: 1 1 45%;
+                            min-width: 180px;
+                        " onmouseover="this.style.background='#0f3a4a'" onmouseout="this.style.background='#154c62'">
+                            <i class="fa-duotone fa-location-dot" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
+                            Manter Onde Estão
+                        </button>
+
+                        <button id="btnCancelarTransformacao" style="
+                            background: #722F37;
+                            border: none;
+                            color: #fff;
+                            padding: 12px 16px;
+                            font-size: 13px;
+                            border-radius: 6px;
+                            cursor: pointer;
+                            transition: all 0.3s;
+                            flex: 1 1 45%;
+                            min-width: 180px;
+                        " onmouseover="this.style.background='#5a252b'" onmouseout="this.style.background='#722F37'">
+                            <i class="fa-duotone fa-circle-xmark" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
+                            Cancelar Operação
+                        </button>
+                    </div>
+                </div>
+            </div>`;
+
+            document.body.insertAdjacentHTML('beforeend', htmlModal);
+
+            var modal = document.getElementById('modalTransformPaginaGrupo');
+
+            document.getElementById('btnSubordinarNovoGrupo').onclick = function() {
+                modal.remove();
+                executarTransformacaoPaginaEmGrupo(item, true, irmaosAbaixo, null);
+            };
+
+            document.getElementById('btnSubordinarOutroGrupo').onclick = function() {
+
+                mostrarModalSelecionarGrupoDestino(item, irmaosAbaixo, modal);
+            };
+
+            document.getElementById('btnManterOndeEstao').onclick = function() {
+                modal.remove();
+
+                executarTransformacaoPaginaEmGrupo(item, false, [], null);
+            };
+
+            document.getElementById('btnCancelarTransformacao').onclick = function() {
+                modal.remove();
+            };
+
+            var escHandler = function(e) {
+                if (e.key === 'Escape') {
+                    modal.remove();
+                    document.removeEventListener('keydown', escHandler);
+                }
+            };
+            document.addEventListener('keydown', escHandler);
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "mostrarModalTransformacaoPaginaEmGrupo", erro);
+        }
+    }
+
+    function mostrarModalSelecionarGrupoDestino(itemOrigem, itensParaMover, modalPrincipal) {
+        try {
+            var htmlGrupos = montarSelectGruposParaSwal(itemOrigem.id);
+
+            var htmlModal = `
+            <div id="modalSelecionarGrupo" style="
+                position: fixed;
+                top: 0;
+                left: 0;
+                width: 100%;
+                height: 100%;
+                background: rgba(0,0,0,0.5);
+                display: flex;
+                align-items: center;
+                justify-content: center;
+                z-index: 100000;
+            ">
+                <div style="
+                    background: linear-gradient(135deg, #1e1e2f 0%, #2d2d4d 100%);
+                    border-radius: 12px;
+                    max-width: 450px;
+                    width: 90%;
+                    box-shadow: 0 10px 40px rgba(0,0,0,0.5);
+                    overflow: hidden;
+                    font-family: 'Segoe UI', sans-serif;
+                ">
+
+                    <div style="background:#2d2d4d; padding:20px; text-align:center; border-bottom:1px solid rgba(255,255,255,0.1);">
+                        <div style="font-size:40px; margin-bottom:8px;">📁</div>
+                        <div style="font-size:18px; color:#c9a8ff; font-weight:bold;">Selecionar Grupo Destino</div>
+                    </div>
+
+                    <div style="padding:20px; color:#e0e0e0;">
+                        <p style="margin-bottom:15px;">
+                            Os <strong style="color:#ff6b35;">${itensParaMover.length}</strong> item(ns) serão movidos para o grupo selecionado:
+                        </p>
+
+                        <div style="text-align:left;">
+                            <label style="font-weight:600; margin-bottom:8px; display:block; color:#c9a8ff;">
+                                Selecione o Grupo:
+                            </label>
+                            ${htmlGrupos}
+                        </div>
+                    </div>
+
+                    <div style="background:#3b3b5c; padding:15px; display:flex; gap:10px; justify-content:center;">
+
+                        <button id="btnConfirmarGrupo" style="
+                            background: #4a5d23;
+                            border: none;
+                            color: #fff;
+                            padding: 12px 25px;
+                            font-size: 14px;
+                            border-radius: 6px;
+                            cursor: pointer;
+                            transition: all 0.3s;
+                        " onmouseover="this.style.background='#3d4d1c'" onmouseout="this.style.background='#4a5d23'">
+                            <i class="fa-duotone fa-check" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
+                            Confirmar
+                        </button>
+
+                        <button id="btnVoltarSelecao" style="
+                            background: #154c62;
+                            border: none;
+                            color: #fff;
+                            padding: 12px 25px;
+                            font-size: 14px;
+                            border-radius: 6px;
+                            cursor: pointer;
+                            transition: all 0.3s;
+                        " onmouseover="this.style.background='#0f3a4a'" onmouseout="this.style.background='#154c62'">
+                            <i class="fa-duotone fa-arrow-left" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
+                            Voltar
+                        </button>
+                    </div>
+                </div>
+            </div>`;
+
+            document.body.insertAdjacentHTML('beforeend', htmlModal);
+
+            var modalSelecao = document.getElementById('modalSelecionarGrupo');
+
+            document.getElementById('btnConfirmarGrupo').onclick = function() {
+                var selectGrupo = document.getElementById('swalSelectGrupo');
+                if (!selectGrupo || !selectGrupo.value) {
+                    Alerta.Warning('Atenção', 'Selecione um grupo destino!');
+                    return;
+                }
+
+                var grupoDestinoId = selectGrupo.value;
+
+                modalSelecao.remove();
+                modalPrincipal.remove();
+
+                executarTransformacaoPaginaEmGrupo(itemOrigem, false, itensParaMover, grupoDestinoId);
+            };
+
+            document.getElementById('btnVoltarSelecao').onclick = function() {
+
+                modalSelecao.remove();
+            };
+
+            var escHandler = function(e) {
+                if (e.key === 'Escape') {
+                    modalSelecao.remove();
+                    document.removeEventListener('keydown', escHandler);
+                }
+            };
+            document.addEventListener('keydown', escHandler);
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "mostrarModalSelecionarGrupoDestino", erro);
+        }
+    }
+
+    function montarSelectGruposParaSwal(itemIdExcluir) {
+        try {
+            var grupos = [];
+
+            grupos.push({
+                id: '_RAIZ_',
+                text: '📍 Raiz (nível principal)',
+                nivel: 0
+            });
+
+            function coletarGrupos(items, nivel) {
+                items.forEach(function(item) {
+
+                    var ehGrupo = !item.href || item.href === 'javascript:void(0);' || (item.items && item.items.length > 0);
+
+                    if (ehGrupo && item.id !== itemIdExcluir) {
+                        grupos.push({
+                            id: item.id,
+                            text: decodeHtml(item.text),
+                            nivel: nivel
+                        });
+                    }
+
+                    if (item.items && item.items.length > 0) {
+                        coletarGrupos(item.items, nivel + 1);
+                    }
+                });
+            }
+
+            coletarGrupos(treeData, 1);
+
+            var html = '<select id="swalSelectGrupo" class="form-control" style="width:100%; padding:10px; font-size:1rem; margin-top:8px;">';
+            html += '<option value="">-- Selecione um grupo --</option>';
+
+            grupos.forEach(function(grupo) {
+                var indent = '&nbsp;&nbsp;'.repeat(grupo.nivel * 2);
+                var icone = grupo.id === '_RAIZ_' ? '📍' : '📁';
+                html += '<option value="' + grupo.id + '">' + indent + icone + ' ' + grupo.text + '</option>';
+            });
+
+            html += '</select>';
+
+            return html;
+
+        } catch (erro) {
+            console.error('[SWAL-SELECT-GRUPOS]', erro);
+            return '<p class="text-danger">Erro ao carregar grupos</p>';
+        }
+    }
+
+    function executarTransformacaoPaginaEmGrupo(item, subordinarAoNovoGrupo, itensParaMover, grupoDestinoId) {
+        try {
+
+            item.href = 'javascript:void(0);';
+            item.items = item.items || [];
+
+            var listaOrigem = item.parentId ? null : treeData;
+            if (item.parentId) {
+                var paiOrigem = buscarItemPorId(treeData, item.parentId);
+                listaOrigem = paiOrigem ? paiOrigem.items : null;
+            }
+
+            if (itensParaMover && itensParaMover.length > 0 && listaOrigem) {
+                if (subordinarAoNovoGrupo) {
+
+                    itensParaMover.forEach(function(irmao) {
+                        var idx = listaOrigem.findIndex(function(i) { return i.id === irmao.id; });
+                        if (idx !== -1) {
+                            listaOrigem.splice(idx, 1);
+                            irmao.parentId = item.id;
+                            item.items.push(irmao);
                         }
                     });
-                }
-
-                adicionarOpcoes(dataSource, 0);
-                html += '</div>';
-
-                html += '<input type="hidden" id="swalPaginaSelecionada" value="" />';
-                html += '<input type="hidden" id="swalUrlSelecionada" value="" />';
-
-                return html;
-
-            } catch (erro) {
-                console.error('[SWAL-SELECT]', erro);
-                return '<p style="color:#ff6b35;">Erro ao carregar páginas</p>';
-            }
-        }
-
-        function mostrarModalTransformacaoGrupoEmPagina(titulo, mensagem, htmlSelect) {
-            return new Promise(function (resolve) {
-                var iconHtml = '<img src="/images/confirmar_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
-
-                var msg = `
-                    <div style="background:#1e1e2f; border-radius: 8px; overflow: hidden; font-family: 'Segoe UI', sans-serif; color: #e0e0e0;">
-                      <div style="background:#2d2d4d; padding: 20px; text-align: center;">
-                        <div style="margin-bottom: 10px;">
-                          <div style="display: inline-block; max-width: 200px; width: 100%;">
-                            ${iconHtml}
-                          </div>
-                        </div>
-                        <div style="font-size: 20px; color: #c9a8ff; font-weight: bold;">${titulo}</div>
-                      </div>
-
-                      <div style="padding: 20px; font-size: 15px; line-height: 1.6; text-align: center; background:#1e1e2f">
-                        <div style="margin-bottom: 20px;">${mensagem}</div>
-                        <div style="margin-top: 15px; text-align: left;">
-                          <label style="font-weight:600; margin-bottom:8px; display:block; color:#e0e0e0;">Selecione a Página do Sistema:</label>
-                          ${htmlSelect}
-                        </div>
-                        <div id="swalMensagemErro" style="display:none; margin-top:15px; background:linear-gradient(135deg, #ff6b35, #e55a2b); color:white; padding:10px 15px; border-radius:8px; border:2px solid rgba(255,255,255,0.5); text-align:center;">
-                          <i class="fa-duotone fa-triangle-exclamation" style="--fa-primary-color:#ffffff; --fa-secondary-color:#ffcc00; margin-right:8px;"></i>
-                          <strong>Selecione uma página</strong> clicando em um item da lista acima!
-                        </div>
-                      </div>
-
-                      <div style="background:#3b3b5c; padding: 15px; text-align: center;">
-                        <button id="btnCancelTransform" style="
-                          background: #722F37;
-                          border: none;
-                          color: #fff;
-                          padding: 10px 20px;
-                          margin-right: 10px;
-                          font-size: 14px;
-                          border-radius: 5px;
-                          cursor: pointer;
-                          transition: background 0.3s;
-                        " onmouseover="this.style.background='#5a252b'" onmouseout="this.style.background='#722F37'">
-                          <i class="fa-duotone fa-circle-xmark" style="--fa-primary-color:#fff; --fa-secondary-color:#FFCCCB; margin-right: 5px;"></i>Cancelar
-                        </button>
-
-                        <button id="btnConfirmTransform" style="
-                          background: #3D5771;
-                          border: none;
-                          color: #fff;
-                          padding: 10px 20px;
-                          font-size: 14px;
-                          border-radius: 5px;
-                          cursor: pointer;
-                          transition: background 0.3s;
-                        " onmouseover="this.style.background='#2d4154'" onmouseout="this.style.background='#3D5771'">
-                          <i class="fa-duotone fa-arrow-right-arrow-left" style="--fa-primary-color:#fff; --fa-secondary-color:#a0c4e8; margin-right: 5px;"></i>Transformar
-                        </button>
-                      </div>
-                    </div>`;
-
-                Swal.fire({
-                    showConfirmButton: false,
-                    html: msg,
-                    backdrop: true,
-                    heightAuto: false,
-                    allowOutsideClick: false,
-                    allowEscapeKey: true,
-                    focusConfirm: false,
-                    customClass: {
-                        popup: 'swal2-popup swal2-no-border swal2-no-shadow'
-                    },
-                    didOpen: function () {
-                        var popup = document.querySelector('.swal2-popup');
-                        if (popup) {
-                            popup.style.border = 'none';
-                            popup.style.boxShadow = 'none';
-                            popup.style.background = 'transparent';
+                } else if (grupoDestinoId) {
+
+                    var listaDestino;
+                    var novoParentId;
+
+                    if (grupoDestinoId === '_RAIZ_') {
+                        listaDestino = treeData;
+                        novoParentId = null;
+                    } else {
+                        var grupoDestino = buscarItemPorId(treeData, grupoDestinoId);
+                        if (grupoDestino) {
+                            grupoDestino.items = grupoDestino.items || [];
+                            listaDestino = grupoDestino.items;
+                            novoParentId = grupoDestinoId;
                         }
-
-                        setTimeout(function () {
-                            var listaPaginas = document.getElementById('swalListaPaginas');
-                            if (listaPaginas) {
-                                listaPaginas.addEventListener('click', function (e) {
-                                    e.preventDefault();
-                                    e.stopPropagation();
-
-                                    var target = e.target;
-                                    var item = null;
-
-                                    while (target && target !== listaPaginas) {
-                                        if (target.classList && target.classList.contains('swal-item-pagina')) {
-                                            item = target;
+                    }
+
+                    if (listaDestino) {
+                        itensParaMover.forEach(function(irmao) {
+                            var idx = listaOrigem.findIndex(function(i) { return i.id === irmao.id; });
+                            if (idx !== -1) {
+                                listaOrigem.splice(idx, 1);
+                                irmao.parentId = novoParentId;
+                                listaDestino.push(irmao);
+                            }
+                        });
+                    }
+                }
+            }
+
+            treeData = JSON.parse(JSON.stringify(treeData));
+
+            atualizarTreeViewAposMovimento(null, false);
+
+            salvarOrdenacaoAutomaticaSemReload().then(function() {
+
+                selectedItem = null;
+                if (treeObj) {
+                    treeObj.selectedNodes = [];
+                }
+                limparFormulario();
+
+                var msg = 'Página transformada em Grupo e salva com sucesso!';
+                if (itensParaMover && itensParaMover.length > 0) {
+                    if (subordinarAoNovoGrupo) {
+                        msg = 'Grupo criado com ' + itensParaMover.length + ' filho(s) e salvo com sucesso!';
+                    } else if (grupoDestinoId) {
+                        msg = 'Grupo criado! ' + itensParaMover.length + ' item(ns) movido(s) para outro grupo. Alterações salvas.';
+                    }
+                }
+
+                Alerta.Sucesso('Transformação Realizada', msg);
+
+                setTimeout(function() {
+                    window.location.reload();
+                }, 1500);
+            }).catch(function(erro) {
+                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "executarTransformacaoPaginaEmGrupo", erro);
+            });
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "executarTransformacaoPaginaEmGrupo", erro);
+        }
+    }
+
+    function guardarEstadoOriginal() {
+        treeDataOriginal = JSON.parse(JSON.stringify(treeData));
+        alteracoesPendentes = false;
+        esconderCardAlteracoes();
+        console.log('[ALTERAÇÕES] Estado original guardado');
+    }
+
+    function marcarAlteracoesPendentes() {
+        alteracoesPendentes = true;
+        mostrarCardAlteracoes();
+        console.log('[ALTERAÇÕES] ⚠️ Alterações pendentes marcadas');
+    }
+
+    function mostrarCardAlteracoes() {
+        var card = document.getElementById('treeChangesCard');
+        console.log('[ALTERAÇÕES] Tentando mostrar card:', card);
+        if (card) {
+            card.classList.add('show');
+            console.log('[ALTERAÇÕES] Card show adicionado. Classes:', card.className);
+        } else {
+            console.error('[ALTERAÇÕES] Card treeChangesCard não encontrado!');
+        }
+    }
+
+    function esconderCardAlteracoes() {
+        var card = document.getElementById('treeChangesCard');
+        if (card) {
+            card.classList.remove('show');
+        }
+    }
+
+    function confirmarAlteracoes() {
+        try {
+            console.log('[ALTERAÇÕES] ✅ Confirmando alterações...');
+
+            if (selectedItem && selectedItem._transformado) {
+                console.log('[ALTERAÇÕES] Item foi transformado, salvando primeiro...');
+
+                var tipoItem = document.getElementById('tipoItem');
+                var txtHref = document.getElementById('txtHref');
+
+                if (tipoItem) tipoItem.value = 'pagina';
+                if (txtHref && selectedItem._novaUrl) txtHref.value = selectedItem._novaUrl;
+
+                salvarPropriedades().then(function() {
+
+                    delete selectedItem._transformado;
+                    delete selectedItem._novaUrl;
+
+                    salvarArvoreCompleta();
+                }).catch(function(erro) {
+                    console.error('[ALTERAÇÕES] Erro ao salvar item transformado:', erro);
+                    Alerta.Erro('Erro ao Salvar', 'Não foi possível salvar a transformação do item.');
+                });
+                return;
+            }
+
+            salvarArvoreCompleta();
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "confirmarAlteracoes", erro);
+        }
+    }
+
+    function salvarArvoreCompleta() {
+        try {
+
+            salvarOrdenacaoAutomatica();
+
+            treeObj.fields.dataSource = treeData;
+            treeObj.dataBind();
+            treeObj.refresh();
+            treeObj.expandAll();
+            configurarBotoesNavegacao();
+
+            guardarEstadoOriginal();
+
+            alteracoesPendentes = false;
+            esconderCardAlteracoes();
+
+            Alerta.Sucesso('Alterações Salvas', 'As alterações na estrutura do menu foram salvas com sucesso!');
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarArvoreCompleta", erro);
+        }
+    }
+
+    function cancelarAlteracoes() {
+        try {
+            console.log('[ALTERAÇÕES] ❌ Cancelando alterações...');
+
+            if (!treeDataOriginal) {
+                console.warn('[ALTERAÇÕES] Não há estado original para restaurar');
+                return;
+            }
+
+            treeData = JSON.parse(JSON.stringify(treeDataOriginal));
+
+            treeObj.fields.dataSource = treeData;
+            treeObj.dataBind();
+            treeObj.refresh();
+            treeObj.expandAll();
+
+            configurarBotoesNavegacao();
+
+            selectedItem = null;
+            limparFormulario(false);
+
+            alteracoesPendentes = false;
+            esconderCardAlteracoes();
+
+            mostrarAlerta('Alterações canceladas. Estrutura restaurada.', 'info');
+
+            console.log('[ALTERAÇÕES] ✅ Estado original restaurado');
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "cancelarAlteracoes", erro);
+        }
+    }
+
+    function configurarBotoesAlteracoes() {
+        var btnConfirmar = document.getElementById('btnConfirmarAlteracoes');
+        var btnCancelar = document.getElementById('btnCancelarAlteracoes');
+
+        if (btnConfirmar) {
+            btnConfirmar.onclick = confirmarAlteracoes;
+        }
+        if (btnCancelar) {
+            btnCancelar.onclick = cancelarAlteracoes;
+        }
+
+        console.log('✅ Botões de confirmação/cancelamento configurados');
+    }
+
+    function calcularOrdemAutomatica(paiId) {
+        try {
+            console.log('Calculando ordem para pai:', paiId);
+
+            var pai = buscarItemPorId(treeData, paiId);
+
+            if (!pai) {
+                console.warn('Pai não encontrado no treeData');
+                document.getElementById('txtOrdem').value = 0;
+                return;
+            }
+
+            var filhos = pai.items || [];
+            console.log('Filhos encontrados:', filhos.length);
+
+            if (filhos.length === 0) {
+
+                document.getElementById('txtOrdem').value = 0;
+                console.log('✅ Ordem definida: 0 (primeiro filho)');
+                return;
+            }
+
+            var maiorOrdem = -1;
+            filhos.forEach(function(filho) {
+                var ordem = parseFloat(filho.ordem) || 0;
+                if (ordem > maiorOrdem) {
+                    maiorOrdem = ordem;
+                }
+            });
+
+            var novaOrdem = maiorOrdem + 1;
+            document.getElementById('txtOrdem').value = novaOrdem;
+            console.log('✅ Ordem calculada:', novaOrdem, '(maior atual:', maiorOrdem, ')');
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "calcularOrdemAutomatica", erro);
+        }
+    }
+
+    function carregarArvore() {
+        try {
+            console.log('[DEBUG] Iniciando carregamento da árvore...');
+            fetch('/api/Navigation/GetTreeAdmin')
+                .then(r => {
+                    console.log('[DEBUG] Response recebido:', r.status, r.statusText);
+                    return r.json();
+                })
+                .then(result => {
+                    console.log('[DEBUG] Dados recebidos da API:', result);
+                    console.log('[DEBUG] result.success:', result.success);
+                    console.log('[DEBUG] result.data:', result.data);
+                    console.log('[DEBUG] result.data.length:', result.data ? result.data.length : 'undefined');
+
+                    if (result.success && result.data && result.data.length > 0) {
+                        console.log('[DEBUG] ✅ Condição OK - Renderizando árvore com', result.data.length, 'itens');
+                        treeData = result.data;
+
+                        var emptyState = document.getElementById('emptyTreeState');
+                        if (emptyState) {
+                            emptyState.style.display = 'none';
+                        }
+
+                        console.log('[DEBUG] Chamando renderizarTreeView()...');
+                        renderizarTreeView();
+                        console.log('[DEBUG] Chamando atualizarContagem()...');
+                        atualizarContagem();
+                        console.log('[DEBUG] Chamando popularDropdownItemPai()...');
+
+                        popularDropdownItemPai();
+
+                        popularDropdownPosicao();
+
+                        guardarEstadoOriginal();
+
+                        console.log('[DEBUG] ✅ Árvore carregada com sucesso!');
+                    } else {
+                        console.log('[DEBUG] ❌ Condição falhou - Mostrando estado vazio');
+                        console.log('[DEBUG] Motivo: success=' + result.success + ', data=' + (result.data ? 'existe' : 'null/undefined') + ', length=' + (result.data ? result.data.length : 'N/A'));
+
+                        var emptyState = document.getElementById('emptyTreeState');
+                        if (emptyState) {
+                            emptyState.style.display = 'block';
+                        }
+
+                        var itemCount = document.getElementById('itemCount');
+                        if (itemCount) {
+                            itemCount.textContent = '0 itens';
+                        }
+                    }
+                })
+                .catch(erro => {
+                    console.error('[DEBUG] ❌ ERRO no fetch:', erro);
+                    Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarArvore", erro);
+                });
+        } catch (erro) {
+            console.error('[DEBUG] ❌ ERRO no try-catch:', erro);
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarArvore", erro);
+        }
+    }
+
+    function renderizarTreeView() {
+        try {
+            console.log('[DEBUG renderizarTreeView] Iniciando renderização...');
+            console.log('[DEBUG renderizarTreeView] treeData:', treeData);
+            console.log('[DEBUG renderizarTreeView] treeData.length:', treeData ? treeData.length : 'undefined');
+
+            if (treeObj) {
+                console.log('[DEBUG renderizarTreeView] Destruindo instância anterior do TreeView...');
+                try {
+                    treeObj.destroy();
+                    treeObj = null;
+                } catch (error) {
+                    console.warn('[DEBUG renderizarTreeView] Erro ao destruir TreeView anterior:', error);
+                }
+            }
+
+            var container = document.getElementById('gestaoTreeView');
+            console.log('[DEBUG renderizarTreeView] Container encontrado:', container);
+            container.innerHTML = '<div id="treeViewControl"></div>';
+
+            console.log('[DEBUG renderizarTreeView] Criando nova instância do TreeView com', treeData.length, 'itens');
+
+            treeObj = new ej.navigations.TreeView({
+                fields: {
+                    dataSource: treeData,
+                    id: 'id',
+                    text: 'text',
+                    child: 'items'
+                },
+                nodeTemplate: function(data) {
+
+                    var isGroup = (data.items && data.items.length > 0) || !data.href || data.href === 'javascript:void(0);' || data.href === '';
+
+                    if (data.href && data.href !== 'javascript:void(0);' && data.href !== '') {
+                        isGroup = false;
+                    }
+
+                    var badge = isGroup ? 'Grupo' : 'Página';
+                    var badgeClass = isGroup ? 'badge-grupo' : 'badge-pagina';
+                    var badgeTitle = isGroup ? 'Clique para transformar em Página' : 'Clique para transformar em Grupo';
+                    var displayText = decodeHtml(data.text);
+
+                    var iconClass = data.icon || 'fa-duotone fa-folder';
+                    if (iconClass && !iconClass.includes('fa-duotone')) {
+                        iconClass = iconClass.replace(/fa-(regular|solid|light)/g, 'fa-duotone');
+                    }
+
+                    var btnsEsquerda =
+                        '<div class="tree-nav-buttons tree-nav-left">' +
+                            '<button type="button" class="tree-nav-btn btn-order btn-mover-cima" data-itemid="' + data.id + '" title="Mover para cima">' +
+                                '<i class="fa-duotone fa-arrow-up-from-bracket"></i>' +
+                            '</button>' +
+                            '<button type="button" class="tree-nav-btn btn-order btn-mover-baixo" data-itemid="' + data.id + '" title="Mover para baixo">' +
+                                '<i class="fa-duotone fa-arrow-down-from-bracket"></i>' +
+                            '</button>' +
+                        '</div>';
+
+                    var btnsDireita =
+                        '<div class="tree-nav-buttons tree-nav-right">' +
+                            '<button type="button" class="tree-nav-btn btn-level btn-mover-esq" data-itemid="' + data.id + '" title="Subir nível (vira irmão do pai)">' +
+                                '<i class="fa-duotone fa-arrow-left-from-bracket"></i>' +
+                            '</button>' +
+                            '<button type="button" class="tree-nav-btn btn-level btn-mover-dir" data-itemid="' + data.id + '" title="Descer nível (vira filho do item acima)">' +
+                                '<i class="fa-duotone fa-arrow-right-from-bracket"></i>' +
+                            '</button>' +
+                        '</div>';
+
+                    var badgeBtn = '<button type="button" class="node-badge ' + badgeClass + ' btn-toggle-tipo" ' +
+                                   'data-itemid="' + data.id + '" title="' + badgeTitle + '">' + badge + '</button>';
+
+                    return '<div class="tree-node" data-id="' + data.id + '">' +
+                           btnsEsquerda +
+                           '<div class="tree-nav-separator"></div>' +
+                           '<i class="' + iconClass + ' icon-duotone-ftx"></i>' +
+                           '<span class="node-text ' + (isGroup ? 'node-group' : '') + '">' + displayText + '</span>' +
+                           badgeBtn +
+                           '<div class="tree-nav-separator"></div>' +
+                           btnsDireita +
+                           '</div>';
+                },
+                allowDragAndDrop: false,
+                allowDropSibling: false,
+                allowDropChild: false,
+                allowMultiSelection: false,
+                nodeClicked: function(args) {
+                    try {
+                        console.log('=== nodeClicked event ===', args);
+
+                        if (!args || !args.node) {
+                            console.warn('Node clicado sem elemento válido:', args);
+                            return;
+                        }
+
+                        var nodeElement = args.node;
+                        var treeNodeDiv = nodeElement.querySelector('.tree-node');
+
+                        if (!treeNodeDiv) {
+                            console.error('Elemento .tree-node não encontrado dentro do node');
+                            return;
+                        }
+
+                        var nodeId = treeNodeDiv.getAttribute('data-id');
+                        console.log('Node clicado - ID extraído do DOM:', nodeId);
+
+                        if (!nodeId) {
+                            console.error('data-id não encontrado no elemento');
+                            return;
+                        }
+
+                        var itemCompleto = buscarItemPorId(treeData, nodeId);
+                        console.log('Item encontrado no treeData:', itemCompleto);
+
+                        if (itemCompleto) {
+                            selecionarItem(itemCompleto);
+                        } else {
+                            console.error('Item não encontrado no treeData para ID:', nodeId);
+                        }
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "renderizarTreeView.nodeClicked", error);
+                    }
+                },
+                expandOn: 'Click'
+            });
+
+            treeObj.appendTo('#treeViewControl');
+            console.log('✅ TreeView renderizado com sucesso! Total de itens:', treeData.length);
+
+            configurarBotoesNavegacao();
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "renderizarTreeView", erro);
+        }
+    }
+
+    function configurarBotoesNavegacao() {
+        var treeContainer = document.getElementById('gestaoTreeView');
+        if (!treeContainer) {
+            console.error('[NAV-BTNS] Container da árvore não encontrado');
+            return;
+        }
+
+        treeContainer.removeEventListener('click', handleNavButtonClick);
+
+        treeContainer.addEventListener('click', handleNavButtonClick, true);
+
+        console.log('✅ Event listeners de navegação configurados');
+    }
+
+    function handleNavButtonClick(event) {
+        var target = event.target;
+
+        if (target.tagName === 'I') {
+            target = target.parentElement;
+        }
+
+        var isNavBtn = target.classList.contains('tree-nav-btn');
+        var isToggleTipo = target.classList.contains('btn-toggle-tipo');
+
+        if (!isNavBtn && !isToggleTipo) {
+            return;
+        }
+
+        event.stopPropagation();
+        event.preventDefault();
+
+        var itemId = target.getAttribute('data-itemid');
+        console.log('[NAV-BTN] Botão clicado, itemId:', itemId);
+
+        if (!itemId) {
+            console.error('[NAV-BTN] data-itemid não encontrado no botão');
+            return;
+        }
+
+        if (isToggleTipo) {
+            alternarTipoItem(itemId, event);
+            return;
+        }
+
+        if (target.classList.contains('btn-mover-cima')) {
+            moverItemCima(itemId, event);
+        } else if (target.classList.contains('btn-mover-baixo')) {
+            moverItemBaixo(itemId, event);
+        } else if (target.classList.contains('btn-mover-esq')) {
+            moverItemEsquerda(itemId, event);
+        } else if (target.classList.contains('btn-mover-dir')) {
+            moverItemDireita(itemId, event);
+        }
+    }
+
+    function selecionarItem(itemData) {
+        try {
+            console.log('=== INICIO selecionarItem ===');
+            console.log('itemData recebido:', itemData);
+
+            if (!itemData) {
+                console.error('itemData é null ou undefined');
+                return;
+            }
+
+            selectedItem = itemData;
+
+            var propsCard = document.getElementById('propsCard');
+            if (!propsCard) {
+                console.error('Elemento propsCard não encontrado!');
+                return;
+            }
+            propsCard.style.display = 'block';
+            console.log('Card de propriedades mostrado');
+
+            document.querySelectorAll('.tree-node').forEach(n => n.classList.remove('selected'));
+            var node = document.querySelector('.tree-node[data-id="' + selectedItem.id + '"]');
+            if (node) {
+                node.classList.add('selected');
+                console.log('Node marcado visualmente');
+            }
+
+            var recursoId = document.getElementById('recursoId');
+            var parentId = document.getElementById('parentId');
+            var txtNome = document.getElementById('txtNome');
+            var txtNomeMenu = document.getElementById('txtNomeMenu');
+            var txtHref = document.getElementById('txtHref');
+            var txtIconClass = document.getElementById('txtIconClass');
+            var txtOrdem = document.getElementById('txtOrdem');
+            var txtDescricao = document.getElementById('txtDescricao');
+            var chkAtivo = document.getElementById('chkAtivo');
+            var selectedItemName = document.getElementById('selectedItemName');
+            var modoEdicao = document.getElementById('modoEdicao');
+
+            if (recursoId) recursoId.value = selectedItem.id || '';
+            if (parentId) parentId.value = selectedItem.parentId || '';
+            if (txtNome) txtNome.value = decodeHtml(selectedItem.text) || '';
+            if (txtNomeMenu) txtNomeMenu.value = decodeHtml(selectedItem.nomeMenu) || '';
+            if (txtHref) txtHref.value = selectedItem.href || '';
+            if (txtOrdem) txtOrdem.value = selectedItem.ordem || 0;
+            if (txtDescricao) txtDescricao.value = decodeHtml(selectedItem.descricao) || '';
+            if (chkAtivo) chkAtivo.checked = selectedItem.ativo !== false;
+            if (selectedItemName) selectedItemName.textContent = decodeHtml(selectedItem.text);
+
+            console.log('Campos básicos preenchidos (decodificados)');
+
+            atualizarEstadoBotoesOrdenacao();
+
+            var iconClass = selectedItem.icon || 'fa-duotone fa-file';
+            if (txtIconClass) txtIconClass.value = iconClass;
+
+            var ddlIconeObj = document.getElementById('ddlIcone');
+            if (ddlIconeObj && ddlIconeObj.ej2_instances && ddlIconeObj.ej2_instances[0]) {
+                var ddlIcone = ddlIconeObj.ej2_instances[0];
+
+                var iconClassNormalizado = iconClass;
+                if (iconClass) {
+
+                    iconClassNormalizado = iconClass.replace(/^fa-(regular|solid|light|duotone)\s+/, 'fa-duotone ');
+
+                    if (!iconClassNormalizado.startsWith('fa-')) {
+                        iconClassNormalizado = 'fa-duotone ' + iconClassNormalizado;
+                    }
+                }
+
+                ddlIcone.value = null;
+                ddlIcone.dataBind();
+
+                setTimeout(function() {
+                    if (ddlIcone.fields && ddlIcone.fields.dataSource) {
+                        var dataSource = ddlIcone.fields.dataSource;
+                        var iconEncontrado = null;
+
+                        for (var i = 0; i < dataSource.length; i++) {
+                            var cat = dataSource[i];
+                            if (cat.child && cat.child.length > 0) {
+                                for (var j = 0; j < cat.child.length; j++) {
+                                    var icon = cat.child[j];
+
+                                    var iconIdStr = String(icon.id || '');
+                                    var iconClassStr = String(iconClassNormalizado || '');
+                                    var iconClassOriginalStr = String(iconClass || '');
+
+                                    var iconNameNormalizado = iconClassNormalizado.replace(/^fa-duotone\s+/, '');
+                                    var iconNameOriginal = iconClassOriginalStr.replace(/^fa-(regular|solid|light|duotone)\s+/, '');
+
+                                    if (iconIdStr === iconClassNormalizado ||
+                                        iconIdStr === iconClassOriginalStr ||
+                                        iconIdStr.endsWith(' ' + iconNameNormalizado) ||
+                                        iconIdStr.endsWith(' ' + iconNameOriginal)) {
+                                        iconEncontrado = icon;
+                                        break;
+                                    }
+                                }
+                            }
+                            if (iconEncontrado) break;
+                        }
+
+                        if (iconEncontrado) {
+                            ddlIcone.value = [iconEncontrado.id];
+                            ddlIcone.dataBind();
+                            console.log('✅ DropDownTree de ícone atualizado para:', iconEncontrado.id);
+                        } else {
+                            console.warn('Ícone não encontrado no dataSource. Tentando:', iconClassNormalizado, 'ou', iconClass);
+                        }
+                    }
+                }, 500);
+            }
+
+            var href = selectedItem.href || '';
+            console.log('URL do item (href):', href);
+
+            if (href && href.endsWith('.html')) {
+                var ddlPaginaObj = document.getElementById('ddlPagina');
+                if (ddlPaginaObj && ddlPaginaObj.ej2_instances && ddlPaginaObj.ej2_instances[0]) {
+                    var ddlPagina = ddlPaginaObj.ej2_instances[0];
+
+                    var pageId = null;
+                    if (ddlPagina.fields && ddlPagina.fields.dataSource) {
+                        var dataSource = ddlPagina.fields.dataSource;
+                        console.log('Buscando página com paginaRef:', href);
+
+                        for (var i = 0; i < dataSource.length; i++) {
+                            var category = dataSource[i];
+                            if (category.child && category.child.length > 0) {
+
+                                for (var j = 0; j < category.child.length; j++) {
+                                    var page = category.child[j];
+                                    if (page.paginaRef === href) {
+                                        pageId = page.id;
+                                        console.log('✅ Página encontrada! ID:', pageId, '| Módulo:', category.text, '| Página:', page.text);
+                                        break;
+                                    }
+                                }
+                            }
+                            if (pageId) break;
+                        }
+                    }
+
+                    if (pageId) {
+
+                        ddlPagina.value = null;
+
+                        setTimeout(function() {
+                            ddlPagina.value = [pageId];
+                            console.log('✅ DropDownTree de página atualizado para:', pageId);
+                        }, 100);
+                    } else {
+                        console.warn('⚠️ Página não encontrada no DropDownTree para href:', href);
+                    }
+                }
+            } else {
+                console.log('URL não está no formato esperado ou está vazia');
+            }
+
+            var parentId = selectedItem.parentId;
+            console.log('Parent ID do item:', parentId);
+            document.getElementById('parentId').value = parentId || '';
+
+            if (modoEdicao) {
+                modoEdicao.textContent = 'Editar';
+                modoEdicao.className = 'badge bg-warning text-dark ms-2';
+            }
+
+            habilitarCardPropriedades(true);
+
+            carregarControleAcesso(selectedItem.id);
+            document.getElementById('acessoCard').style.display = 'block';
+
+            atualizarToggleTipo(selectedItem);
+
+            console.log('=== FIM selecionarItem (sucesso) ===');
+        } catch (error) {
+            console.error('ERRO em selecionarItem:', error);
+            console.error('Stack:', error.stack);
+            mostrarAlerta('Erro ao selecionar item: ' + error.message, 'danger');
+        }
+    }
+
+    function buscarItemPorId(items, id) {
+        try {
+            if (!items || !id) return null;
+
+            for (var i = 0; i < items.length; i++) {
+
+                if (String(items[i].id) === String(id)) {
+                    return items[i];
+                }
+                if (items[i].items && items[i].items.length > 0) {
+                    var found = buscarItemPorId(items[i].items, id);
+                    if (found) return found;
+                }
+            }
+            return null;
+        } catch (erro) {
+            console.error('[buscarItemPorId] Erro:', erro);
+            return null;
+        }
+    }
+
+    function removerItemDaArvore(items, id) {
+        try {
+            if (!items || !id) return false;
+
+            for (var i = 0; i < items.length; i++) {
+                if (String(items[i].id) === String(id)) {
+
+                    items.splice(i, 1);
+                    return true;
+                }
+
+                if (items[i].items && items[i].items.length > 0) {
+                    if (removerItemDaArvore(items[i].items, id)) {
+                        return true;
+                    }
+                }
+            }
+            return false;
+        } catch (erro) {
+            console.error('[removerItemDaArvore] Erro:', erro);
+            return false;
+        }
+    }
+
+    function buscarItemPorTexto(items, texto) {
+        try {
+            if (!items || !texto) return null;
+
+            for (var i = 0; i < items.length; i++) {
+                var itemText = items[i].text || items[i].nomeMenu || '';
+                if (itemText.trim() === texto.trim()) {
+                    return items[i];
+                }
+                if (items[i].items && items[i].items.length > 0) {
+                    var found = buscarItemPorTexto(items[i].items, texto);
+                    if (found) return found;
+                }
+            }
+            return null;
+        } catch (erro) {
+            console.error('[buscarItemPorTexto] Erro:', erro);
+            return null;
+        }
+    }
+
+    function carregarIconesFontAwesome() {
+        try {
+            fetch('/api/Navigation/GetIconesFontAwesomeHierarquico')
+                .then(r => r.json())
+                .then(result => {
+                    console.log('Ícones FontAwesome carregados:', result);
+                    if (result.success && result.data) {
+                        var ddlIconeObj = document.getElementById('ddlIcone').ej2_instances[0];
+                        if (ddlIconeObj) {
+
+                            ddlIconeObj.fields = {
+                                dataSource: result.data,
+                                value: 'id',
+                                text: 'text',
+                                parentValue: 'parentId',
+                                hasChildren: 'hasChild',
+                                child: 'child'
+                            };
+                            ddlIconeObj.dataBind();
+                            console.log('DropDownTree de ícones populado com', result.data.length, 'categorias');
+                        }
+                    }
+                })
+                .catch(erro => {
+                    Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarIconesFontAwesome", erro);
+                });
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarIconesFontAwesome", erro);
+        }
+    }
+
+    function onIconeSelect(args) {
+        try {
+            console.log('=== onIconeSelect disparado (AO CLICAR) ===');
+            console.log('args.nodeData:', args.nodeData);
+
+            if (!args.nodeData || args.nodeData.isCategory) {
+                console.log('Categoria clicada - não atualiza campo');
+                return;
+            }
+
+            var iconClass = args.nodeData.id;
+            var iconLabel = args.nodeData.text;
+
+            var txtIconClass = document.getElementById('txtIconClass');
+            if (txtIconClass) {
+                txtIconClass.value = iconClass;
+                console.log('✅ Classe CSS atualizada IMEDIATAMENTE:', iconClass, '| Label:', iconLabel);
+            } else {
+                console.error('❌ Elemento txtIconClass não encontrado!');
+            }
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "onIconeSelect", erro);
+        }
+    }
+
+    function onIconeChange(args) {
+        try {
+            console.log('=== onIconeChange disparado ===');
+            console.log('args completo:', args);
+            console.log('args.itemData:', args.itemData);
+            console.log('args.value:', args.value);
+
+            var itemData = args.itemData;
+
+            if (!itemData && args.value && args.value.length > 0) {
+                console.log('itemData undefined - buscando no dataSource...');
+                var ddlIconeObj = document.getElementById('ddlIcone').ej2_instances[0];
+                if (ddlIconeObj && ddlIconeObj.fields && ddlIconeObj.fields.dataSource) {
+                    var iconId = args.value[0];
+                    console.log('Buscando iconId:', iconId);
+
+                    itemData = findIconById(iconId);
+                    if (itemData) {
+                        console.log('✅ Ícone encontrado no dataSource:', itemData);
+                    }
+
+                    if (!itemData) {
+                        console.log('❌ Ícone não encontrado no dataSource - usando iconId diretamente');
+                        itemData = { id: iconId, text: iconId, isCategory: false };
+                    }
+                }
+            }
+
+            if (!itemData || itemData.isCategory) {
+                document.getElementById('txtIconClass').value = '';
+                console.log('Categoria selecionada ou campo limpo');
+                return;
+            }
+
+            var iconClass = itemData.id;
+            var iconLabel = itemData.text;
+
+            var txtIconClass = document.getElementById('txtIconClass');
+            if (txtIconClass) {
+                txtIconClass.value = iconClass;
+                console.log('✅ Classe CSS atualizada:', iconClass, '| Label:', iconLabel);
+            } else {
+                console.error('❌ Elemento txtIconClass não encontrado!');
+            }
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "onIconeChange", erro);
+        }
+    }
+
+    function carregarPaginasSistema() {
+        try {
+            fetch('/api/Navigation/GetPaginasHierarquico')
+                .then(r => r.json())
+                .then(result => {
+                    console.log('Páginas carregadas:', result);
+                    if (result.success && result.data) {
+                        var ddlPaginaObj = document.getElementById('ddlPagina').ej2_instances[0];
+                        if (ddlPaginaObj) {
+                            ddlPaginaObj.fields = {
+                                dataSource: result.data,
+                                value: 'id',
+                                text: 'text',
+                                parentValue: 'parentId',
+                                hasChildren: 'hasChild',
+                                child: 'child'
+                            };
+                            ddlPaginaObj.dataBind();
+                            console.log('DropDownTree de páginas populado com', result.data.length, 'módulos');
+                        }
+                    }
+                })
+                .catch(erro => {
+                    Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarPaginasSistema", erro);
+                });
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarPaginasSistema", erro);
+        }
+    }
+
+    function onPaginaDropdownCreated() {
+        try {
+            var ddlPaginaObj = document.getElementById('ddlPagina').ej2_instances[0];
+            if (ddlPaginaObj) {
+                ddlPaginaObj.itemTemplate = function(data) {
+                    if (data.isCategory) {
+                        return '<div style="font-weight: 600; padding: 4px 0; color: #3D5771;">' + data.text + '</div>';
+                    }
+                    return '<div style="display: flex; align-items: center; gap: 8px;">' +
+                           '<i class="fa-duotone fa-file-lines" style="font-size: 16px; width: 20px; --fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i>' +
+                           '<span>' + data.text + '</span>' +
+                           '</div>';
+                };
+
+                ddlPaginaObj.valueTemplate = function(data) {
+                    if (!data || data.isCategory) return '';
+                    var displayText = data.displayText || data.text;
+                    return '<div style="display: flex; align-items: center; gap: 8px;">' +
+                           '<i class="fa-duotone fa-file-lines" style="font-size: 14px; width: 18px; --fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i>' +
+                           '<span>' + displayText + '</span>' +
+                           '</div>';
+                };
+
+                ddlPaginaObj.selecting = function(args) {
+                    console.log('=== SELECTING página (ANTES) ===', args);
+                    if (args.nodeData && !args.nodeData.isCategory) {
+
+                        ultimaPaginaSelecionada = {
+                            id: args.nodeData.id,
+                            paginaRef: args.nodeData.paginaRef
+                        };
+
+                        var txtHref = document.getElementById('txtHref');
+                        if (txtHref && args.nodeData.paginaRef) {
+                            txtHref.value = args.nodeData.paginaRef;
+                            console.log('✅ URL atualizada via SELECTING:', args.nodeData.paginaRef);
+                        }
+                    }
+                };
+
+                ddlPaginaObj.select = function(args) {
+                    console.log('=== SELECT página ===', args);
+                    if (args.nodeData && !args.nodeData.isCategory) {
+
+                        ultimaPaginaSelecionada = {
+                            id: args.nodeData.id,
+                            paginaRef: args.nodeData.paginaRef
+                        };
+
+                        var txtHref = document.getElementById('txtHref');
+                        if (txtHref && args.nodeData.paginaRef) {
+                            txtHref.value = args.nodeData.paginaRef;
+                            console.log('✅ URL atualizada via SELECT:', args.nodeData.paginaRef);
+                        }
+
+                        ddlPaginaObj.value = [args.nodeData.id];
+                        onPaginaChange({ itemData: args.nodeData, value: [args.nodeData.id] });
+                        ddlPaginaObj.hidePopup();
+                    }
+                };
+
+                ddlPaginaObj.change = function(args) {
+                    onPaginaChange(args);
+                };
+
+                ddlPaginaObj.changeOnBlur = false;
+
+                ddlPaginaObj.close = function(args) {
+                    console.log('=== CLOSE página ===');
+
+                    var paginaRef = null;
+                    var pageId = null;
+
+                    var valor = ddlPaginaObj.value;
+                    if (valor && valor.length > 0) {
+                        pageId = valor[0];
+                        console.log('Valor do componente:', pageId);
+
+                        if (ddlPaginaObj.fields && ddlPaginaObj.fields.dataSource) {
+                            var dataSource = ddlPaginaObj.fields.dataSource;
+                            for (var i = 0; i < dataSource.length; i++) {
+                                var modulo = dataSource[i];
+                                if (modulo.child && modulo.child.length > 0) {
+                                    for (var j = 0; j < modulo.child.length; j++) {
+                                        var page = modulo.child[j];
+                                        if (page.id === pageId && page.paginaRef) {
+                                            paginaRef = page.paginaRef;
                                             break;
                                         }
-                                        target = target.parentElement;
                                     }
-
-                                    if (!item) {
-                                        return;
-                                    }
-
-                                    var podeSelecionar = item.dataset.podeselecionar === 'true';
-                                    var isCategory = item.dataset.iscategory === 'true';
-
-                                    if (!podeSelecionar || isCategory || !item.dataset.itemid) {
-                                        return;
-                                    }
-
-                                    document.querySelectorAll('.swal-item-pagina').forEach(function (el) {
-                                        el.style.background = 'transparent';
-                                        el.style.borderLeft = 'none';
-                                        el.classList.remove('swal-item-selected');
-                                        delete el.dataset.selected;
-                                    });
-
-                                    item.style.background = '#3D5771';
-                                    item.style.borderLeft = '3px solid #ff6b35';
-                                    item.classList.add('swal-item-selected');
-                                    item.dataset.selected = 'true';
-
-                                    var mensagemErro = document.getElementById('swalMensagemErro');
-                                    if (mensagemErro) {
-                                        mensagemErro.style.display = 'none';
-                                    }
-
-                                    var paginaIdInput = document.getElementById('swalPaginaSelecionada');
-                                    var urlInput = document.getElementById('swalUrlSelecionada');
-
-                                    if (!paginaIdInput || !urlInput) {
-                                        return;
-                                    }
-
-                                    paginaIdInput.value = item.dataset.itemid;
-                                    urlInput.value = item.dataset.url || '';
-                                }, true);
-
-                                listaPaginas.addEventListener('mouseover', function (e) {
-                                    var target = e.target;
-                                    var item = null;
-                                    while (target && target !== listaPaginas) {
-                                        if (target.classList && target.classList.contains('swal-item-pagina')) {
-                                            item = target;
-                                            break;
-                                        }
-                                        target = target.parentElement;
-                                    }
-                                    if (item && item.dataset.podeselecionar === 'true' && item.dataset.selected !== 'true') {
-                                        item.style.background = '#2d2d4d';
-                                    }
-                                }, true);
-
-                                listaPaginas.addEventListener('mouseout', function (e) {
-                                    var target = e.target;
-                                    var item = null;
-                                    while (target && target !== listaPaginas) {
-                                        if (target.classList && target.classList.contains('swal-item-pagina')) {
-                                            item = target;
-                                            break;
-                                        }
-                                        target = target.parentElement;
-                                    }
-                                    if (item && item.dataset.selected !== 'true') {
-                                        item.style.background = 'transparent';
-                                    }
-                                }, true);
+                                }
+                                if (paginaRef) break;
                             }
-                        }, 100);
-
-                        var confirmBtn = document.getElementById('btnConfirmTransform');
-                        if (confirmBtn) {
-                            confirmBtn.onclick = function () {
-                                var paginaIdInput = document.getElementById('swalPaginaSelecionada');
-                                var urlInput = document.getElementById('swalUrlSelecionada');
-                                var mensagemErro = document.getElementById('swalMensagemErro');
-
-                                if (!paginaIdInput || !paginaIdInput.value || !urlInput || !urlInput.value) {
-
-                                    if (mensagemErro) {
-                                        mensagemErro.style.display = 'block';
-
-                                        mensagemErro.scrollIntoView({ behavior: 'smooth', block: 'center' });
-                                    }
-                                    return;
+                        }
+                    }
+
+                    if (!paginaRef && ultimaPaginaSelecionada) {
+                        paginaRef = ultimaPaginaSelecionada.paginaRef;
+                        console.log('Usando última página selecionada:', paginaRef);
+                    }
+
+                    if (paginaRef) {
+                        var txtHref = document.getElementById('txtHref');
+                        if (txtHref) {
+                            txtHref.value = paginaRef;
+                            console.log('✅ URL atualizada via CLOSE:', paginaRef);
+                        }
+                    }
+                };
+
+                console.log('DropDownTree de páginas configurado');
+            }
+        } catch (error) {
+            console.error('Erro ao configurar DropDownTree de páginas:', error);
+        }
+    }
+
+    function onPaginaSelect(args) {
+        try {
+            console.log('=== onPaginaSelect disparado (AO CLICAR) ===');
+            console.log('args.nodeData:', args.nodeData);
+
+            if (!args.nodeData || args.nodeData.isCategory) {
+                console.log('Categoria clicada - não gera URL');
+                return;
+            }
+
+            var paginaRef = args.nodeData.paginaRef;
+            console.log('paginaRef extraída:', paginaRef);
+
+            var txtHref = document.getElementById('txtHref');
+            if (txtHref) {
+                txtHref.value = paginaRef;
+                console.log('✅ URL atualizada IMEDIATAMENTE:', paginaRef);
+            } else {
+                console.error('❌ Elemento txtHref não encontrado!');
+            }
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "onPaginaSelect", erro);
+        }
+    }
+
+    function onPaginaChange(args) {
+        try {
+            console.log('=== onPaginaChange disparado ===');
+            console.log('args completo:', args);
+            console.log('args.itemData:', args.itemData);
+            console.log('args.value:', args.value);
+
+            var itemData = args.itemData;
+
+            if (!itemData && args.value && args.value.length > 0) {
+                console.log('itemData undefined - buscando no dataSource...');
+                var ddlPaginaObj = document.getElementById('ddlPagina').ej2_instances[0];
+                if (ddlPaginaObj && ddlPaginaObj.fields && ddlPaginaObj.fields.dataSource) {
+                    var pageId = args.value[0];
+                    console.log('Buscando pageId:', pageId);
+
+                    var dataSource = ddlPaginaObj.fields.dataSource;
+                    for (var i = 0; i < dataSource.length; i++) {
+                        var category = dataSource[i];
+                        if (category.child && category.child.length > 0) {
+                            for (var j = 0; j < category.child.length; j++) {
+                                if (category.child[j].id === pageId) {
+                                    itemData = category.child[j];
+                                    console.log('✅ Item encontrado no dataSource:', itemData);
+                                    break;
                                 }
-
-                                var paginaId = paginaIdInput.value;
-                                var paginaUrl = urlInput.value;
-
-                                Swal.close();
-                                resolve({ confirmado: true, paginaId: paginaId, paginaUrl: paginaUrl });
-                            };
+                            }
                         }
-
-                        var cancelBtn = document.getElementById('btnCancelTransform');
-                        if (cancelBtn) {
-                            cancelBtn.onclick = function () {
-                                Swal.close();
-                                resolve({ confirmado: false });
-                            };
+                        if (itemData) break;
+                    }
+                }
+            }
+
+            if (!itemData || itemData.isCategory) {
+                console.log('Categoria selecionada ou campo limpo - não gera URL');
+                return;
+            }
+
+            var paginaRef = itemData.paginaRef;
+            console.log('paginaRef extraída:', paginaRef);
+
+            var txtHref = document.getElementById('txtHref');
+            if (txtHref) {
+                txtHref.value = paginaRef;
+                console.log('✅ URL atualizada no campo txtHref:', paginaRef);
+            } else {
+                console.error('❌ Elemento txtHref não encontrado!');
+            }
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "onPaginaChange", erro);
+        }
+    }
+
+    function carregarControleAcesso(recursoId) {
+        try {
+            if (!recursoId) {
+                document.getElementById('listaAcessos').innerHTML = '<p class="text-muted text-center">Selecione um item para gerenciar acessos</p>';
+                return;
+            }
+
+            fetch('/api/Navigation/GetUsuariosAcesso?recursoId=' + recursoId)
+                .then(r => r.json())
+                .then(result => {
+                    console.log('Acessos carregados:', result);
+                    var container = document.getElementById('listaAcessos');
+                    if (result.success && result.data && result.data.length > 0) {
+                        container.innerHTML = result.data.map(u =>
+                            '<div class="acesso-item">' +
+                            '<input type="checkbox" class="form-check-input me-2" ' +
+                            'id="acesso_' + u.usuarioId + '" ' +
+                            (u.acesso ? 'checked' : '') +
+                            ' onchange="atualizarAcesso(\'' + u.usuarioId + '\', this.checked)" />' +
+                            '<label for="acesso_' + u.usuarioId + '">' + u.nome + '</label>' +
+                            '</div>'
+                        ).join('');
+                    } else {
+                        container.innerHTML = '<p class="text-muted text-center">Nenhum usuário encontrado</p>';
+                    }
+                })
+                .catch(erro => {
+                    Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarControleAcesso", erro);
+                });
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarControleAcesso", erro);
+        }
+    }
+
+    function atualizarAcesso(usuarioId, acesso) {
+        try {
+            if (!selectedItem) return;
+
+            fetch('/api/Navigation/UpdateAcesso', {
+                method: 'POST',
+                headers: { 'Content-Type': 'application/json' },
+                body: JSON.stringify({
+                    usuarioId: usuarioId,
+                    recursoId: selectedItem.id,
+                    acesso: acesso
+                })
+            })
+            .then(r => r.json())
+            .then(result => {
+                if (result.success) {
+                    mostrarAlerta('Acesso atualizado!', 'success');
+                } else {
+                    mostrarAlerta('Erro ao atualizar acesso: ' + result.message, 'danger');
+                }
+            })
+            .catch(erro => {
+                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "atualizarAcesso", erro);
+            });
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "atualizarAcesso", erro);
+        }
+    }
+
+    function salvarPropriedades() {
+        return new Promise(function(resolve, reject) {
+            try {
+
+                var tipoItem = document.getElementById('tipoItem').value;
+                var ehGrupo = (tipoItem === 'grupo');
+
+                var txtHref = document.getElementById('txtHref').value;
+                var parentIdValue = document.getElementById('parentId').value;
+
+                var hrefFinal;
+                if (ehGrupo) {
+
+                    hrefFinal = 'javascript:void(0);';
+                } else {
+
+                    hrefFinal = txtHref || null;
+
+                    if (!hrefFinal) {
+                        Alerta.Warning('Atenção', 'Para itens do tipo Página, selecione uma página do sistema!');
+                        reject(new Error('URL não informada'));
+                        return;
+                    }
+                }
+
+            var posicaoSelecionada = null;
+            var ddlPosicaoObj = document.getElementById('ddlPosicao');
+            if (ddlPosicaoObj && ddlPosicaoObj.ej2_instances && ddlPosicaoObj.ej2_instances[0]) {
+                var ddlPosicao = ddlPosicaoObj.ej2_instances[0];
+                posicaoSelecionada = ddlPosicao.value && ddlPosicao.value.length > 0 ? ddlPosicao.value[0] : null;
+            }
+            console.log('[SALVAR] Posição selecionada:', posicaoSelecionada);
+
+            var parentIdFinal = parentIdValue || null;
+            var ehNovoItem = !document.getElementById('recursoId').value;
+
+            if (ehNovoItem && posicaoSelecionada && posicaoSelecionada !== '_INICIO_') {
+
+                var itemReferencia = buscarItemPorId(treeData, posicaoSelecionada);
+                if (itemReferencia) {
+                    parentIdFinal = itemReferencia.parentId || null;
+                }
+            }
+
+            var dto = {
+                id: document.getElementById('recursoId').value,
+                text: encodeHtml(document.getElementById('txtNome').value),
+                nomeMenu: encodeHtml(document.getElementById('txtNomeMenu').value),
+                href: hrefFinal,
+                icon: document.getElementById('txtIconClass').value,
+                ordem: 0,
+                descricao: encodeHtml(document.getElementById('txtDescricao').value),
+                ativo: document.getElementById('chkAtivo').checked,
+                parentId: parentIdFinal,
+                posicaoAbaixoDe: posicaoSelecionada
+            };
+
+                console.log('DTO codificado para envio:', dto);
+
+                if (!dto.text || !dto.nomeMenu) {
+                    Alerta.Warning('Atenção', 'Preencha Nome e NomeMenu!');
+                    reject(new Error('Campos obrigatórios não preenchidos'));
+                    return;
+                }
+
+                var ehNovoItem = !dto.id;
+
+                fetch('/api/Navigation/SaveRecurso', {
+                    method: 'POST',
+                    headers: { 'Content-Type': 'application/json' },
+                    body: JSON.stringify(dto)
+                })
+                .then(r => r.json())
+                .then(result => {
+                    if (result.success) {
+                        mostrarAlerta(result.message, 'success');
+
+                        setTimeout(function() {
+                            carregarArvore();
+                        }, 300);
+
+                        atualizarNavegacaoLateral();
+
+                        var recursoId = result.id || dto.id;
+
+                        if (ehNovoItem && recursoId) {
+
+                            habilitarAcessoTodosUsuarios(recursoId);
+
+                            Alerta.Confirmar(
+                                'Gerenciar Controle de Acesso',
+                                'Deseja gerenciar o controle de acesso deste item agora?',
+                                'Sim',
+                                'Não'
+                            ).then(function(result) {
+                                if (result) {
+
+                                    selectedItem = {
+                                        id: recursoId,
+                                        text: dto.text,
+                                        nomeMenu: dto.nomeMenu,
+                                        href: dto.href,
+                                        icon: dto.icon,
+                                        ordem: dto.ordem,
+                                        descricao: dto.descricao,
+                                        ativo: dto.ativo,
+                                        parentId: dto.parentId
+                                    };
+                                    document.getElementById('recursoId').value = recursoId;
+
+                                    habilitarCardPropriedades(false);
+
+                                    carregarControleAcesso(recursoId);
+
+                                    document.getElementById('acessoCard').style.display = 'block';
+                                } else {
+
+                                    document.getElementById('propsCard').style.display = 'none';
+                                    document.getElementById('acessoCard').style.display = 'none';
+                                    habilitarCardPropriedades(true);
+                                }
+                            });
+                        } else {
+
+                            document.getElementById('propsCard').style.display = 'none';
+                            habilitarCardPropriedades(true);
                         }
-                    }
+
+                        resolve(result);
+                    } else {
+                        mostrarAlerta('Erro: ' + result.message, 'danger');
+                        reject(new Error(result.message));
+                    }
+                })
+                .catch(erro => {
+                    Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarPropriedades", erro);
+                    reject(erro);
+                });
+            } catch (erro) {
+                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarPropriedades", erro);
+                reject(erro);
+            }
+        });
+    }
+
+    function adicionarNovoItem() {
+        try {
+
+            limparFormulario(false);
+
+            habilitarCardPropriedades(true);
+
+            document.getElementById('propsCard').style.display = 'block';
+
+            document.getElementById('acessoCard').style.display = 'none';
+
+            document.getElementById('txtNome').focus();
+            document.getElementById('selectedItemName').textContent = 'Novo Item';
+
+            document.getElementById('modoEdicao').textContent = 'Criar';
+            document.getElementById('modoEdicao').className = 'badge bg-success ms-2';
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "adicionarNovoItem", erro);
+        }
+    }
+
+    function excluirItem() {
+        try {
+            if (!selectedItem || !selectedItem.id) {
+                mostrarAlerta('Selecione um item para excluir!', 'warning');
+                return;
+            }
+
+            Alerta.Confirmar(
+                'Confirmar Exclusão',
+                'Tem certeza que deseja excluir "' + decodeHtml(selectedItem.text) + '"?',
+                'Excluir',
+                'Cancelar'
+            ).then(function(result) {
+                if (!result) return;
+
+                fetch('/api/Navigation/DeleteRecurso', {
+                    method: 'POST',
+                    headers: { 'Content-Type': 'application/json' },
+                    body: JSON.stringify({ recursoId: selectedItem.id })
+                })
+                .then(r => r.json())
+                .then(result => {
+                    if (result.success) {
+                        mostrarAlerta(result.message, 'success');
+                        limparFormulario();
+                        carregarArvore();
+                    } else {
+                        mostrarAlerta('Erro: ' + result.message, 'danger');
+                    }
+                })
+                .catch(erro => {
+                    Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "excluirItem", erro);
                 });
             });
-        }
-
-        function encontrarIrmaosAbaixo(item) {
-            try {
-                var lista = item.parentId ? null : treeData;
-
-                if (item.parentId) {
-                    var pai = buscarItemPorId(treeData, item.parentId);
-                    lista = pai ? pai.items : null;
-                }
-
-                if (!lista) return [];
-
-                var indexAtual = lista.findIndex(function (i) { return i.id === item.id; });
-                if (indexAtual === -1 || indexAtual >= lista.length - 1) return [];
-
-                return lista.slice(indexAtual + 1);
-
-            } catch (erro) {
-                console.error('[IRMAOS-ABAIXO]', erro);
-                return [];
-            }
-        }
-
-        function executarTransformacaoGrupoEmPagina(item, novaUrl) {
-            try {
-
-                var itemId = item.id;
-
-                if (item.items && item.items.length > 0) {
-                    var paiId = item.parentId;
-                    var listaDestino = paiId ? null : treeData;
-
-                    if (paiId) {
-                        var pai = buscarItemPorId(treeData, paiId);
-                        listaDestino = pai ? pai.items : null;
-                    }
-
-                    if (listaDestino) {
-                        var indexItem = listaDestino.findIndex(function (i) { return i.id === item.id; });
-
-                        item.items.forEach(function (filho, idx) {
-                            filho.parentId = paiId || null;
-                            listaDestino.splice(indexItem + 1 + idx, 0, filho);
-                        });
-                    }
-
-                    item.items = [];
-                }
-
-                item.href = novaUrl;
-
-                treeData = JSON.parse(JSON.stringify(treeData));
-
-                atualizarTreeViewAposMovimento(null, false);
-
-                salvarOrdenacaoAutomaticaSemReload().then(function () {
-
-                    selectedItem = null;
-                    if (treeObj) {
-                        treeObj.selectedNodes = [];
-                    }
-                    limparFormulario();
-
-                    Alerta.Sucesso('Transformação Realizada', 'O grupo foi transformado em página e salvo com sucesso!');
-
-                    setTimeout(function () {
-                        window.location.reload();
-                    }, 1500);
-                }).catch(function (erro) {
-                    Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "executarTransformacaoGrupoEmPagina", erro);
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "excluirItem", erro);
+        }
+    }
+
+    function validarCamposObrigatorios() {
+        var nome = document.getElementById('txtNome').value.trim();
+        var nomeMenu = document.getElementById('txtNomeMenu').value.trim();
+        var pagina = document.getElementById('ddlPagina')?.ej2_instances?.[0]?.value;
+        var icone = document.getElementById('ddlIcone')?.ej2_instances?.[0]?.value;
+
+        return nome && nomeMenu && pagina && pagina.length > 0 && icone && icone.length > 0;
+    }
+
+    function atualizarEstadoBotoesOrdenacao() {
+
+    }
+
+    function podeMoverParaCima() {
+        if (!selectedItem || !selectedItem.id || !treeData) return false;
+
+        var item = buscarItemPorId(treeData, selectedItem.id);
+        if (!item) return false;
+
+        var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
+        if (!resultado) return false;
+
+        return resultado.indice > 0;
+    }
+
+    function podeMoverParaBaixo() {
+        if (!selectedItem || !selectedItem.id || !treeData) return false;
+
+        var item = buscarItemPorId(treeData, selectedItem.id);
+        if (!item) return false;
+
+        var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
+        if (!resultado) return false;
+
+        return resultado.indice < resultado.lista.length - 1;
+    }
+
+    function encontrarListaPaiEIndice(lista, targetId, targetParentId) {
+        for (var i = 0; i < lista.length; i++) {
+            var currentItem = lista[i];
+            var currentParentId = currentItem.parentId || null;
+
+            if (String(currentItem.id) === String(targetId)) {
+                return { lista: lista, indice: i };
+            }
+
+            if (currentItem.items && currentItem.items.length > 0) {
+                var resultado = encontrarListaPaiEIndice(currentItem.items, targetId, targetParentId);
+                if (resultado) return resultado;
+            }
+        }
+        return null;
+    }
+
+    function moverItemCima(itemId, event) {
+        console.log('[MOVER-CIMA] === FUNÇÃO CHAMADA ===');
+        console.log('[MOVER-CIMA] itemId recebido:', itemId, 'tipo:', typeof itemId);
+
+        if (event) {
+            event.stopPropagation();
+            event.preventDefault();
+        }
+
+        try {
+            console.log('[MOVER-CIMA] treeData:', treeData);
+            var item = buscarItemPorId(treeData, itemId);
+            console.log('[MOVER-CIMA] Item encontrado:', item);
+            if (!item) {
+                console.error('[MOVER-CIMA] Item não encontrado:', itemId);
+                return;
+            }
+
+            var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
+            if (!resultado || resultado.indice <= 0) {
+                console.log('[MOVER-CIMA] Item já está na primeira posição');
+                return;
+            }
+
+            var listaIrmaos = resultado.lista;
+            var indexAtual = resultado.indice;
+
+            var temp = listaIrmaos[indexAtual];
+            listaIrmaos[indexAtual] = listaIrmaos[indexAtual - 1];
+            listaIrmaos[indexAtual - 1] = temp;
+
+            console.log('[MOVER-CIMA] ✅ Item movido para cima:', item.text);
+
+            atualizarTreeViewAposMovimento(itemId);
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemCima", erro);
+        }
+    }
+
+    function moverItemBaixo(itemId, event) {
+        console.log('[MOVER-BAIXO] === FUNÇÃO CHAMADA ===');
+        console.log('[MOVER-BAIXO] itemId recebido:', itemId, 'tipo:', typeof itemId);
+
+        if (event) {
+            event.stopPropagation();
+            event.preventDefault();
+        }
+
+        try {
+            var item = buscarItemPorId(treeData, itemId);
+            console.log('[MOVER-BAIXO] Item encontrado:', item);
+            if (!item) {
+                console.error('[MOVER-BAIXO] Item não encontrado:', itemId);
+                return;
+            }
+
+            var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
+            if (!resultado || resultado.indice >= resultado.lista.length - 1) {
+                console.log('[MOVER-BAIXO] Item já está na última posição');
+                return;
+            }
+
+            var listaIrmaos = resultado.lista;
+            var indexAtual = resultado.indice;
+
+            var temp = listaIrmaos[indexAtual];
+            listaIrmaos[indexAtual] = listaIrmaos[indexAtual + 1];
+            listaIrmaos[indexAtual + 1] = temp;
+
+            console.log('[MOVER-BAIXO] ✅ Item movido para baixo:', item.text);
+
+            atualizarTreeViewAposMovimento(itemId);
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemBaixo", erro);
+        }
+    }
+
+    function moverItemEsquerda(itemId, event) {
+        console.log('[MOVER-ESQ] === FUNÇÃO CHAMADA ===');
+        console.log('[MOVER-ESQ] itemId recebido:', itemId, 'tipo:', typeof itemId);
+
+        if (event) {
+            event.stopPropagation();
+            event.preventDefault();
+        }
+
+        try {
+            var item = buscarItemPorId(treeData, itemId);
+            console.log('[MOVER-ESQ] Item encontrado:', item);
+            if (!item) {
+                console.error('[MOVER-ESQ] Item não encontrado:', itemId);
+                return;
+            }
+
+            if (!item.parentId) {
+                console.log('[MOVER-ESQ] Item já está na raiz');
+                return;
+            }
+
+            var paiAtual = buscarItemPorId(treeData, item.parentId);
+            if (!paiAtual) {
+                console.error('[MOVER-ESQ] Pai não encontrado:', item.parentId);
+                return;
+            }
+
+            console.log('[MOVER-ESQ] Movendo item para nível do pai:', paiAtual.text);
+
+            if (paiAtual.items) {
+                var indexNoFilho = paiAtual.items.findIndex(function(i) { return i.id === item.id; });
+                if (indexNoFilho !== -1) {
+                    paiAtual.items.splice(indexNoFilho, 1);
+                }
+            }
+
+            var novoParentId = paiAtual.parentId || null;
+            item.parentId = novoParentId;
+
+            if (!novoParentId) {
+
+                var indexPai = treeData.findIndex(function(i) { return i.id === paiAtual.id; });
+                if (indexPai !== -1) {
+                    treeData.splice(indexPai + 1, 0, item);
+                } else {
+                    treeData.push(item);
+                }
+            } else {
+
+                var avo = buscarItemPorId(treeData, novoParentId);
+                if (avo) {
+                    if (!avo.items) avo.items = [];
+
+                    var indexPaiNoAvo = avo.items.findIndex(function(i) { return i.id === paiAtual.id; });
+                    if (indexPaiNoAvo !== -1) {
+                        avo.items.splice(indexPaiNoAvo + 1, 0, item);
+                    } else {
+                        avo.items.push(item);
+                    }
+                }
+            }
+
+            if (paiAtual.items && paiAtual.items.length === 0) {
+                console.log('[MOVER-ESQ] Pai ficou sem filhos, transformando em página');
+                paiAtual.items = [];
+
+            }
+
+            console.log('[MOVER-ESQ] ✅ Item movido para esquerda (subiu nível)');
+
+            atualizarTreeViewAposMovimento(itemId);
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemEsquerda", erro);
+        }
+    }
+
+    function moverItemDireita(itemId, event) {
+        console.log('[MOVER-DIR] === FUNÇÃO CHAMADA ===');
+        console.log('[MOVER-DIR] itemId recebido:', itemId, 'tipo:', typeof itemId);
+
+        if (event) {
+            event.stopPropagation();
+            event.preventDefault();
+        }
+
+        try {
+            var item = buscarItemPorId(treeData, itemId);
+            console.log('[MOVER-DIR] Item encontrado:', item);
+            if (!item) {
+                console.error('[MOVER-DIR] Item não encontrado:', itemId);
+                return;
+            }
+
+            var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
+            if (!resultado || resultado.indice <= 0) {
+                console.log('[MOVER-DIR] Não há item acima para virar pai');
+                return;
+            }
+
+            var listaIrmaos = resultado.lista;
+            var indexAtual = resultado.indice;
+            var itemAcima = listaIrmaos[indexAtual - 1];
+
+            console.log('[MOVER-DIR] Item acima que virará pai:', itemAcima.text);
+
+            var itemAcimaEhPagina = itemAcima.href && itemAcima.href !== 'javascript:void(0);' && itemAcima.href.trim() !== '';
+
+            if (itemAcimaEhPagina) {
+
+                console.log('[MOVER-DIR] Item acima é uma Página - exibindo aviso de transformação');
+
+                Alerta.Confirmar(
+                    'Transformação em Grupo',
+                    '<div style="text-align: left; padding: 10px;">' +
+                        '<p style="margin-bottom: 15px;">Ao subordinar <strong>"' + item.text + '"</strong> como filho de <strong>"' + itemAcima.text + '"</strong>, ' +
+                        'a página <strong>"' + itemAcima.text + '"</strong> será <span style="color: #d9534f; font-weight: bold;">transformada em GRUPO</span>.</p>' +
+                        '<div style="background: linear-gradient(135deg, #ff6b35, #e55a2b); color: white; padding: 12px 15px; border-radius: 8px; border: 2px solid rgba(255,255,255,0.5); margin: 10px 0;">' +
+                            '<i class="fa-duotone fa-triangle-exclamation" style="--fa-primary-color:#ffffff; --fa-secondary-color:#ffcc00; margin-right: 8px;"></i>' +
+                            '<strong>ATENÇÃO:</strong> O link HTML da página será removido permanentemente e ela não acessará mais nenhuma funcionalidade diretamente!' +
+                        '</div>' +
+                        '<p style="margin-top: 15px;">Deseja prosseguir com esta operação?</p>' +
+                    '</div>',
+                    '<i class="fa-duotone fa-check" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right: 6px;"></i>Sim, Transformar em Grupo',
+                    '<i class="fa-duotone fa-xmark" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right: 6px;"></i>Cancelar'
+                ).then(function(confirmado) {
+                    if (confirmado) {
+                        console.log('[MOVER-DIR] Usuário confirmou transformação - executando movimentação');
+                        executarMovimentacaoDireita(item, listaIrmaos, indexAtual, itemAcima, itemId);
+                    } else {
+                        console.log('[MOVER-DIR] Usuário cancelou - operação revertida');
+                        Alerta.Info('Operação Cancelada', 'A movimentação foi cancelada. Nenhuma alteração foi feita.', 'OK');
+                    }
                 });
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "executarTransformacaoGrupoEmPagina", erro);
-            }
-        }
-
-        function mostrarModalTransformacaoPaginaEmGrupo(item, nomeItem, irmaosAbaixo) {
-            try {
-
-                var listaItensHtml = irmaosAbaixo.slice(0, 5).map(function (irmao) {
-                    return '<div style="padding:4px 0; border-bottom:1px solid rgba(255,255,255,0.1);">• ' + decodeHtml(irmao.text) + '</div>';
-                }).join('');
-
-                if (irmaosAbaixo.length > 5) {
-                    listaItensHtml += '<div style="padding:4px 0; font-style:italic; color:#aaa;">... e mais ' + (irmaosAbaixo.length - 5) + ' item(ns)</div>';
-                }
-
-                var htmlModal = `
-                    <div id="modalTransformPaginaGrupo" style="
-                        position: fixed;
-                        top: 0;
-                        left: 0;
-                        width: 100%;
-                        height: 100%;
-                        background: rgba(0,0,0,0.7);
-                        display: flex;
-                        align-items: center;
-                        justify-content: center;
-                        z-index: 99999;
-                    ">
-                        <div style="
-                            background: linear-gradient(135deg, #1e1e2f 0%, #2d2d4d 100%);
-                            border-radius: 12px;
-                            max-width: 500px;
-                            width: 90%;
-                            box-shadow: 0 10px 40px rgba(0,0,0,0.5);
-                            overflow: hidden;
-                            font-family: 'Segoe UI', sans-serif;
-                        ">
-
-                            <div style="background:#2d2d4d; padding:25px; text-align:center; border-bottom:1px solid rgba(255,255,255,0.1);">
-                                <div style="font-size:50px; margin-bottom:10px;">🤔</div>
-                                <div style="font-size:22px; color:#c9a8ff; font-weight:bold;">Transformar Página em Grupo</div>
-                            </div>
-
-                            <div style="padding:20px; color:#e0e0e0;">
-                                <p style="text-align:center; margin-bottom:15px;">
-                                    A Página <strong style="color:#ff6b35;">"${decodeHtml(nomeItem)}"</strong> será transformada em <strong>Grupo</strong>.
-                                </p>
-
-                                <div style="
-                                    background: rgba(255,107,53,0.2);
-                                    border: 1px solid rgba(255,255,255,0.3);
-                                    border-radius: 8px;
-                                    padding: 12px 15px;
-                                    margin-bottom: 20px;
-                                    color: #fff;
-                                    font-size: 0.9rem;
-                                ">
-                                    <i class="fa-duotone fa-info-circle" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#fff; margin-right:8px;"></i>
-                                    A URL da página será removida.
-                                </div>
-
-                                <div style="text-align:left;">
-                                    <strong style="color:#c9a8ff;">Existem ${irmaosAbaixo.length} item(ns) abaixo desta página:</strong>
-                                    <div style="
-                                        margin-top:10px;
-                                        max-height:150px;
-                                        overflow-y:auto;
-                                        background:rgba(0,0,0,0.2);
-                                        border-radius:6px;
-                                        padding:10px;
-                                    ">
-                                        ${listaItensHtml}
-                                    </div>
-                                </div>
-
-                                <p style="text-align:center; margin-top:20px; font-weight:600; color:#c9a8ff;">
-                                    O que deseja fazer com estes itens?
-                                </p>
-                            </div>
-
-                            <div style="background:#3b3b5c; padding:20px; display:flex; flex-wrap:wrap; gap:10px; justify-content:center;">
-
-                                <button id="btnSubordinarNovoGrupo" style="
-                                    background: #4a5d23;
-                                    border: none;
-                                    color: #fff;
-                                    padding: 12px 16px;
-                                    font-size: 13px;
-                                    border-radius: 6px;
-                                    cursor: pointer;
-                                    transition: all 0.3s;
-                                    flex: 1 1 45%;
-                                    min-width: 180px;
-                                " onmouseover="this.style.background='#3d4d1c'" onmouseout="this.style.background='#4a5d23'">
-                                    <i class="fa-duotone fa-folder-tree" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
-                                    Subordinar ao Novo Grupo
-                                </button>
-
-                                <button id="btnSubordinarOutroGrupo" style="
-                                    background: #68432C;
-                                    border: none;
-                                    color: #fff;
-                                    padding: 12px 16px;
-                                    font-size: 13px;
-                                    border-radius: 6px;
-                                    cursor: pointer;
-                                    transition: all 0.3s;
-                                    flex: 1 1 45%;
-                                    min-width: 180px;
-                                " onmouseover="this.style.background='#523522'" onmouseout="this.style.background='#68432C'">
-                                    <i class="fa-duotone fa-folder-open" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
-                                    Subordinar a Outro Grupo
-                                </button>
-
-                                <button id="btnManterOndeEstao" style="
-                                    background: #154c62;
-                                    border: none;
-                                    color: #fff;
-                                    padding: 12px 16px;
-                                    font-size: 13px;
-                                    border-radius: 6px;
-                                    cursor: pointer;
-                                    transition: all 0.3s;
-                                    flex: 1 1 45%;
-                                    min-width: 180px;
-                                " onmouseover="this.style.background='#0f3a4a'" onmouseout="this.style.background='#154c62'">
-                                    <i class="fa-duotone fa-location-dot" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
-                                    Manter Onde Estão
-                                </button>
-
-                                <button id="btnCancelarTransformacao" style="
-                                    background: #722F37;
-                                    border: none;
-                                    color: #fff;
-                                    padding: 12px 16px;
-                                    font-size: 13px;
-                                    border-radius: 6px;
-                                    cursor: pointer;
-                                    transition: all 0.3s;
-                                    flex: 1 1 45%;
-                                    min-width: 180px;
-                                " onmouseover="this.style.background='#5a252b'" onmouseout="this.style.background='#722F37'">
-                                    <i class="fa-duotone fa-circle-xmark" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
-                                    Cancelar Operação
-                                </button>
-                            </div>
-                        </div>
-                    </div>`;
-
-                document.body.insertAdjacentHTML('beforeend', htmlModal);
-
-                var modal = document.getElementById('modalTransformPaginaGrupo');
-
-                document.getElementById('btnSubordinarNovoGrupo').onclick = function () {
-                    modal.remove();
-                    executarTransformacaoPaginaEmGrupo(item, true, irmaosAbaixo, null);
-                };
-
-                document.getElementById('btnSubordinarOutroGrupo').onclick = function () {
-
-                    mostrarModalSelecionarGrupoDestino(item, irmaosAbaixo, modal);
-                };
-
-                document.getElementById('btnManterOndeEstao').onclick = function () {
-                    modal.remove();
-
-                    executarTransformacaoPaginaEmGrupo(item, false, [], null);
-                };
-
-                document.getElementById('btnCancelarTransformacao').onclick = function () {
-                    modal.remove();
-                };
-
-                var escHandler = function (e) {
-                    if (e.key === 'Escape') {
-                        modal.remove();
-                        document.removeEventListener('keydown', escHandler);
-                    }
-                };
-                document.addEventListener('keydown', escHandler);
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "mostrarModalTransformacaoPaginaEmGrupo", erro);
-            }
-        }
-
-        function mostrarModalSelecionarGrupoDestino(itemOrigem, itensParaMover, modalPrincipal) {
-            try {
-                var htmlGrupos = montarSelectGruposParaSwal(itemOrigem.id);
-
-                var htmlModal = `
-                    <div id="modalSelecionarGrupo" style="
-                        position: fixed;
-                        top: 0;
-                        left: 0;
-                        width: 100%;
-                        height: 100%;
-                        background: rgba(0,0,0,0.5);
-                        display: flex;
-                        align-items: center;
-                        justify-content: center;
-                        z-index: 100000;
-                    ">
-                        <div style="
-                            background: linear-gradient(135deg, #1e1e2f 0%, #2d2d4d 100%);
-                            border-radius: 12px;
-                            max-width: 450px;
-                            width: 90%;
-                            box-shadow: 0 10px 40px rgba(0,0,0,0.5);
-                            overflow: hidden;
-                            font-family: 'Segoe UI', sans-serif;
-                        ">
-
-                            <div style="background:#2d2d4d; padding:20px; text-align:center; border-bottom:1px solid rgba(255,255,255,0.1);">
-                                <div style="font-size:40px; margin-bottom:8px;">📁</div>
-                                <div style="font-size:18px; color:#c9a8ff; font-weight:bold;">Selecionar Grupo Destino</div>
-                            </div>
-
-                            <div style="padding:20px; color:#e0e0e0;">
-                                <p style="margin-bottom:15px;">
-                                    Os <strong style="color:#ff6b35;">${itensParaMover.length}</strong> item(ns) serão movidos para o grupo selecionado:
-                                </p>
-
-                                <div style="text-align:left;">
-                                    <label style="font-weight:600; margin-bottom:8px; display:block; color:#c9a8ff;">
-                                        Selecione o Grupo:
-                                    </label>
-                                    ${htmlGrupos}
-                                </div>
-                            </div>
-
-                            <div style="background:#3b3b5c; padding:15px; display:flex; gap:10px; justify-content:center;">
-
-                                <button id="btnConfirmarGrupo" style="
-                                    background: #4a5d23;
-                                    border: none;
-                                    color: #fff;
-                                    padding: 12px 25px;
-                                    font-size: 14px;
-                                    border-radius: 6px;
-                                    cursor: pointer;
-                                    transition: all 0.3s;
-                                " onmouseover="this.style.background='#3d4d1c'" onmouseout="this.style.background='#4a5d23'">
-                                    <i class="fa-duotone fa-check" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
-                                    Confirmar
-                                </button>
-
-                                <button id="btnVoltarSelecao" style="
-                                    background: #154c62;
-                                    border: none;
-                                    color: #fff;
-                                    padding: 12px 25px;
-                                    font-size: 14px;
-                                    border-radius: 6px;
-                                    cursor: pointer;
-                                    transition: all 0.3s;
-                                " onmouseover="this.style.background='#0f3a4a'" onmouseout="this.style.background='#154c62'">
-                                    <i class="fa-duotone fa-arrow-left" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right:6px;"></i>
-                                    Voltar
-                                </button>
-                            </div>
-                        </div>
-                    </div>`;
-
-                document.body.insertAdjacentHTML('beforeend', htmlModal);
-
-                var modalSelecao = document.getElementById('modalSelecionarGrupo');
-
-                document.getElementById('btnConfirmarGrupo').onclick = function () {
-                    var selectGrupo = document.getElementById('swalSelectGrupo');
-                    if (!selectGrupo || !selectGrupo.value) {
-                        Alerta.Warning('Atenção', 'Selecione um grupo destino!');
-                        return;
-                    }
-
-                    var grupoDestinoId = selectGrupo.value;
-
-                    modalSelecao.remove();
-                    modalPrincipal.remove();
-
-                    executarTransformacaoPaginaEmGrupo(itemOrigem, false, itensParaMover, grupoDestinoId);
-                };
-
-                document.getElementById('btnVoltarSelecao').onclick = function () {
-
-                    modalSelecao.remove();
-                };
-
-                var escHandler = function (e) {
-                    if (e.key === 'Escape') {
-                        modalSelecao.remove();
-                        document.removeEventListener('keydown', escHandler);
-                    }
-                };
-                document.addEventListener('keydown', escHandler);
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "mostrarModalSelecionarGrupoDestino", erro);
-            }
-        }
-
-        function montarSelectGruposParaSwal(itemIdExcluir) {
-            try {
-                var grupos = [];
-
-                grupos.push({
-                    id: '_RAIZ_',
-                    text: '📍 Raiz (nível principal)',
-                    nivel: 0
-                });
-
-                function coletarGrupos(items, nivel) {
-                    items.forEach(function (item) {
-
-                        var ehGrupo = !item.href || item.href === 'javascript:void(0);' || (item.items && item.items.length > 0);
-
-                        if (ehGrupo && item.id !== itemIdExcluir) {
-                            grupos.push({
-                                id: item.id,
-                                text: decodeHtml(item.text),
-                                nivel: nivel
-                            });
-                        }
-
-                        if (item.items && item.items.length > 0) {
-                            coletarGrupos(item.items, nivel + 1);
-                        }
-                    });
-                }
-
-                coletarGrupos(treeData, 1);
-
-                var html = '<select id="swalSelectGrupo" class="form-control" style="width:100%; padding:10px; font-size:1rem; margin-top:8px;">';
-                html += '<option value="">-- Selecione um grupo --</option>';
-
-                grupos.forEach(function (grupo) {
-                    var indent = '&nbsp;&nbsp;'.repeat(grupo.nivel * 2);
-                    var icone = grupo.id === '_RAIZ_' ? '📍' : '📁';
-                    html += '<option value="' + grupo.id + '">' + indent + icone + ' ' + grupo.text + '</option>';
-                });
-
-                html += '</select>';
-
-                return html;
-
-            } catch (erro) {
-                console.error('[SWAL-SELECT-GRUPOS]', erro);
-                return '<p class="text-danger">Erro ao carregar grupos</p>';
-            }
-        }
-
-        function executarTransformacaoPaginaEmGrupo(item, subordinarAoNovoGrupo, itensParaMover, grupoDestinoId) {
-            try {
-
-                item.href = 'javascript:void(0);';
-                item.items = item.items || [];
-
-                var listaOrigem = item.parentId ? null : treeData;
-                if (item.parentId) {
-                    var paiOrigem = buscarItemPorId(treeData, item.parentId);
-                    listaOrigem = paiOrigem ? paiOrigem.items : null;
-                }
-
-                if (itensParaMover && itensParaMover.length > 0 && listaOrigem) {
-                    if (subordinarAoNovoGrupo) {
-
-                        itensParaMover.forEach(function (irmao) {
-                            var idx = listaOrigem.findIndex(function (i) { return i.id === irmao.id; });
-                            if (idx !== -1) {
-                                listaOrigem.splice(idx, 1);
-                                irmao.parentId = item.id;
-                                item.items.push(irmao);
-                            }
-                        });
-                    } else if (grupoDestinoId) {
-
-                        var listaDestino;
-                        var novoParentId;
-
-                        if (grupoDestinoId === '_RAIZ_') {
-                            listaDestino = treeData;
-                            novoParentId = null;
-                        } else {
-                            var grupoDestino = buscarItemPorId(treeData, grupoDestinoId);
-                            if (grupoDestino) {
-                                grupoDestino.items = grupoDestino.items || [];
-                                listaDestino = grupoDestino.items;
-                                novoParentId = grupoDestinoId;
-                            }
-                        }
-
-                        if (listaDestino) {
-                            itensParaMover.forEach(function (irmao) {
-                                var idx = listaOrigem.findIndex(function (i) { return i.id === irmao.id; });
-                                if (idx !== -1) {
-                                    listaOrigem.splice(idx, 1);
-                                    irmao.parentId = novoParentId;
-                                    listaDestino.push(irmao);
-                                }
-                            });
-                        }
-                    }
-                }
-
-                treeData = JSON.parse(JSON.stringify(treeData));
-
-                atualizarTreeViewAposMovimento(null, false);
-
-                salvarOrdenacaoAutomaticaSemReload().then(function () {
-
-                    selectedItem = null;
-                    if (treeObj) {
-                        treeObj.selectedNodes = [];
-                    }
-                    limparFormulario();
-
-                    var msg = 'Página transformada em Grupo e salva com sucesso!';
-                    if (itensParaMover && itensParaMover.length > 0) {
-                        if (subordinarAoNovoGrupo) {
-                            msg = 'Grupo criado com ' + itensParaMover.length + ' filho(s) e salvo com sucesso!';
-                        } else if (grupoDestinoId) {
-                            msg = 'Grupo criado! ' + itensParaMover.length + ' item(ns) movido(s) para outro grupo. Alterações salvas.';
-                        }
-                    }
-
-                    Alerta.Sucesso('Transformação Realizada', msg);
-
-                    setTimeout(function () {
-                        window.location.reload();
-                    }, 1500);
-                }).catch(function (erro) {
-                    Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "executarTransformacaoPaginaEmGrupo", erro);
-                });
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "executarTransformacaoPaginaEmGrupo", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CONTROLE DE ALTERAÇÕES PENDENTES
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Sistema para rastrear mudanças não salvas na estrutura
-            * Mostra card de confirmação quando há alterações pendentes
-                */
-
-            /**
-             * Guarda estado original do treeData
-             * @@description Permite cancelar alterações voltando ao estado original
-            * @@returns { void}
-            */
-        function guardarEstadoOriginal() {
-            treeDataOriginal = JSON.parse(JSON.stringify(treeData));
-            alteracoesPendentes = false;
-            esconderCardAlteracoes();
-            console.log('[ALTERAÇÕES] Estado original guardado');
-        }
-
-            /**
-             * Marca que há alterações pendentes
-             * @@description Exibe o card de confirmação ao usuário
-            * @@returns { void}
-            */
-        function marcarAlteracoesPendentes() {
-            alteracoesPendentes = true;
-            mostrarCardAlteracoes();
-            console.log('[ALTERAÇÕES] ⚠️ Alterações pendentes marcadas');
-        }
-
-            /**
-             * Mostra o card flutuante de confirmação de alterações
-             * @@returns { void}
-             */
-        function mostrarCardAlteracoes() {
-            var card = document.getElementById('treeChangesCard');
-            console.log('[ALTERAÇÕES] Tentando mostrar card:', card);
-            if (card) {
-                card.classList.add('show');
-                console.log('[ALTERAÇÕES] Card show adicionado. Classes:', card.className);
             } else {
-                console.error('[ALTERAÇÕES] Card treeChangesCard não encontrado!');
-            }
-        }
-
-            /**
-             * Esconde o card de confirmação de alterações
-             * @@returns { void}
-             */
-        function esconderCardAlteracoes() {
-            var card = document.getElementById('treeChangesCard');
-            if (card) {
-                card.classList.remove('show');
-            }
-        }
-
-            /**
-             * Confirma as alterações e salva no banco
-             * @@description Trata casos especiais como transformação de tipo
-            * @@returns { void}
-            */
-        function confirmarAlteracoes() {
-            try {
-                console.log('[ALTERAÇÕES] ✅ Confirmando alterações...');
-
-                if (selectedItem && selectedItem._transformado) {
-                    console.log('[ALTERAÇÕES] Item foi transformado, salvando primeiro...');
-
-                    var tipoItem = document.getElementById('tipoItem');
-                    var txtHref = document.getElementById('txtHref');
-
-                    if (tipoItem) tipoItem.value = 'pagina';
-                    if (txtHref && selectedItem._novaUrl) txtHref.value = selectedItem._novaUrl;
-
-                    salvarPropriedades().then(function () {
-
-                        delete selectedItem._transformado;
-                        delete selectedItem._novaUrl;
-
-                        salvarArvoreCompleta();
-                    }).catch(function (erro) {
-                        console.error('[ALTERAÇÕES] Erro ao salvar item transformado:', erro);
-                        Alerta.Erro('Erro ao Salvar', 'Não foi possível salvar a transformação do item.');
-                    });
-                    return;
-                }
-
-                salvarArvoreCompleta();
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "confirmarAlteracoes", erro);
-            }
-        }
-
-            /**
-             * Salva a árvore completa após confirmação
-             * @@description Chama API e atualiza TreeView
-            * @@returns { void}
-            */
-        function salvarArvoreCompleta() {
-            try {
-
-                salvarOrdenacaoAutomatica();
-
+
+                console.log('[MOVER-DIR] Item acima já é Grupo - executando movimentação direta');
+                executarMovimentacaoDireita(item, listaIrmaos, indexAtual, itemAcima, itemId);
+            }
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemDireita", erro);
+        }
+    }
+
+    function executarMovimentacaoDireita(item, listaIrmaos, indexAtual, itemAcima, itemId) {
+        try {
+
+            listaIrmaos.splice(indexAtual, 1);
+
+            item.parentId = itemAcima.id;
+
+            if (!itemAcima.items) {
+                itemAcima.items = [];
+            }
+            itemAcima.items.push(item);
+
+            if (!itemAcima.href || itemAcima.href === 'javascript:void(0);') {
+
+            } else {
+
+                console.log('[MOVER-DIR] Item acima transformado em grupo');
+                itemAcima.href = 'javascript:void(0);';
+            }
+
+            console.log('[MOVER-DIR] ✅ Item movido para direita (desceu nível, virou filho de ' + itemAcima.text + ')');
+
+            atualizarTreeViewAposMovimento(itemId);
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "executarMovimentacaoDireita", erro);
+        }
+    }
+
+    function atualizarTreeViewAposMovimento(itemIdParaManter, marcarPendentes) {
+        try {
+
+            if (marcarPendentes === undefined) marcarPendentes = true;
+
+            console.log('[ATUALIZAR] Atualizando TreeView após movimento... marcarPendentes=' + marcarPendentes);
+
+            treeData = JSON.parse(JSON.stringify(treeData));
+
+            if (treeObj) {
                 treeObj.fields.dataSource = treeData;
                 treeObj.dataBind();
                 treeObj.refresh();
                 treeObj.expandAll();
-                configurarBotoesNavegacao();
-
-                guardarEstadoOriginal();
-
-                alteracoesPendentes = false;
-                esconderCardAlteracoes();
-
-                Alerta.Sucesso('Alterações Salvas', 'As alterações na estrutura do menu foram salvas com sucesso!');
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarArvoreCompleta", erro);
-            }
-        }
-
-            /**
-             * Cancela alterações e restaura estado original
-             * @@description Desfaz todas as mudanças não salvas
-            * @@returns { void}
-            */
-        function cancelarAlteracoes() {
-            try {
-                console.log('[ALTERAÇÕES] ❌ Cancelando alterações...');
-
-                if (!treeDataOriginal) {
-                    console.warn('[ALTERAÇÕES] Não há estado original para restaurar');
+            }
+
+            configurarBotoesNavegacao();
+
+            popularDropdownItemPai();
+            popularDropdownPosicao();
+
+            if (itemIdParaManter) {
+                var itemAtualizado = buscarItemPorId(treeData, itemIdParaManter);
+                if (itemAtualizado) {
+                    selectedItem = itemAtualizado;
+
+                }
+            } else {
+
+                selectedItem = null;
+                if (treeObj) {
+                    treeObj.selectedNodes = [];
+                }
+            }
+
+            if (marcarPendentes) {
+                marcarAlteracoesPendentes();
+                console.log('[ATUALIZAR] ✅ TreeView atualizado (alterações pendentes)');
+            } else {
+                console.log('[ATUALIZAR] ✅ TreeView atualizado (sem marcar pendentes - já salvo no banco)');
+            }
+
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "atualizarTreeViewAposMovimento", erro);
+        }
+    }
+
+    function moverItemParaCima() {
+        try {
+            if (!selectedItem || !selectedItem.id) {
+                mostrarAlerta('Selecione um item primeiro', 'warning');
+                return;
+            }
+
+            var item = buscarItemPorId(treeData, selectedItem.id);
+            if (!item) {
+                mostrarAlerta('Item não encontrado na árvore', 'danger');
+                return;
+            }
+
+            var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
+            if (!resultado || resultado.indice <= 0) {
+                mostrarAlerta('Item já está na primeira posição', 'info');
+                return;
+            }
+
+            var listaIrmaos = resultado.lista;
+            var indexAtual = resultado.indice;
+
+            var temp = listaIrmaos[indexAtual];
+            listaIrmaos[indexAtual] = listaIrmaos[indexAtual - 1];
+            listaIrmaos[indexAtual - 1] = temp;
+
+            console.log('[MOVER-CIMA] ✅ Posições trocadas na lista original');
+            console.log('[MOVER-CIMA] Lista após troca:', listaIrmaos.map(function(i) { return i.text; }));
+
+            var treeDataNovo = JSON.parse(JSON.stringify(treeData));
+            treeData = treeDataNovo;
+
+            treeObj.fields.dataSource = treeData;
+            treeObj.dataBind();
+
+            console.log('[MOVER-CIMA] ✅ TreeView atualizado');
+
+            setTimeout(function() {
+                var itemAtualizado = buscarItemPorId(treeData, item.id);
+                if (itemAtualizado) {
+                    selecionarItem(itemAtualizado);
+                }
+                salvarOrdenacaoAutomatica();
+            }, 300);
+
+        } catch (erro) {
+            console.error('[moverItemParaCima] Erro:', erro);
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemParaCima", erro);
+        }
+    }
+
+    function moverItemParaBaixo() {
+        try {
+            if (!selectedItem || !selectedItem.id) {
+                mostrarAlerta('Selecione um item primeiro', 'warning');
+                return;
+            }
+
+            var item = buscarItemPorId(treeData, selectedItem.id);
+            if (!item) {
+                mostrarAlerta('Item não encontrado na árvore', 'danger');
+                return;
+            }
+
+            var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
+            if (!resultado || resultado.indice >= resultado.lista.length - 1) {
+                mostrarAlerta('Item já está na última posição', 'info');
+                return;
+            }
+
+            var listaIrmaos = resultado.lista;
+            var indexAtual = resultado.indice;
+
+            console.log('[MOVER-BAIXO] Lista encontrada:', listaIrmaos.length, 'itens');
+            console.log('[MOVER-BAIXO] Índice atual:', indexAtual);
+            console.log('[MOVER-BAIXO] Item atual:', item.text);
+            console.log('[MOVER-BAIXO] Item seguinte:', listaIrmaos[indexAtual + 1].text);
+
+            var temp = listaIrmaos[indexAtual];
+            listaIrmaos[indexAtual] = listaIrmaos[indexAtual + 1];
+            listaIrmaos[indexAtual + 1] = temp;
+
+            console.log('[MOVER-BAIXO] ✅ Posições trocadas na lista');
+
+            var treeDataNovo = JSON.parse(JSON.stringify(treeData));
+            treeData = treeDataNovo;
+
+            treeObj.fields.dataSource = treeData;
+            treeObj.dataBind();
+
+            console.log('[MOVER-BAIXO] ✅ TreeView atualizado');
+
+            setTimeout(function() {
+                var itemAtualizado = buscarItemPorId(treeData, item.id);
+                if (itemAtualizado) {
+                    selecionarItem(itemAtualizado);
+                }
+                salvarOrdenacaoAutomatica();
+            }, 300);
+
+        } catch (erro) {
+            console.error('[moverItemParaBaixo] Erro:', erro);
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemParaBaixo", erro);
+        }
+    }
+
+    function limparFormulario(esconderCard = true) {
+        try {
+            selectedItem = null;
+
+            ultimoIconeSelecionado = null;
+            ultimaPaginaSelecionada = null;
+
+            document.getElementById('formPropriedades').reset();
+            document.getElementById('recursoId').value = '';
+            document.getElementById('parentId').value = '';
+            document.getElementById('txtIconClass').value = '';
+            document.getElementById('selectedItemName').textContent = 'Nenhum item selecionado';
+
+            var ddlIconeObj = document.getElementById('ddlIcone').ej2_instances[0];
+            if (ddlIconeObj) {
+                ddlIconeObj.value = [];
+                ddlIconeObj.text = '';
+                ddlIconeObj.refresh();
+            }
+
+            var ddlPaginaObj = document.getElementById('ddlPagina').ej2_instances[0];
+            if (ddlPaginaObj) {
+                ddlPaginaObj.value = null;
+            }
+
+            document.getElementById('parentId').value = '';
+
+            document.getElementById('listaAcessos').innerHTML = '<p class="text-muted text-center">Selecione um item para gerenciar acessos</p>';
+            document.querySelectorAll('.tree-node').forEach(n => n.classList.remove('selected'));
+
+        document.getElementById('modoEdicao').textContent = 'Selecione';
+        document.getElementById('modoEdicao').className = 'badge bg-secondary ms-2';
+
+        atualizarEstadoBotoesOrdenacao();
+
+        if (esconderCard) {
+            document.getElementById('propsCard').style.display = 'none';
+        }
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "limparFormulario", erro);
+        }
+    }
+
+    /*
+
+    function salvarOrdenacao() {
+        try {
+            if (!treeObj) return;
+            var dataSource = treeObj.getTreeData();
+            var items = extrairItensDoTreeView(dataSource, null, 0);
+            fetch('/api/Navigation/SaveTreeToDb', {
+                method: 'POST',
+                headers: { 'Content-Type': 'application/json' },
+                body: JSON.stringify(items)
+            })
+            .then(r => r.json())
+            .then(result => {
+                if (result.success) {
+                    mostrarAlerta('Ordenação salva com sucesso!', 'success');
+                    carregarArvore();
+                    atualizarNavegacaoLateral();
+                } else {
+                    mostrarAlerta('Erro: ' + result.message, 'danger');
+                }
+            })
+            .catch(erro => {
+                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarOrdenacao", erro);
+            });
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarOrdenacao", erro);
+        }
+    }
+    */
+
+    function salvarOrdenacaoAutomatica() {
+        try {
+            if (!treeObj && !treeData) {
+                console.error('[AUTO-SAVE] Nem treeObj nem treeData existem!');
+                return;
+            }
+
+            var dataSource = treeData || treeObj.getTreeData();
+            console.log('[AUTO-SAVE] DataSource obtido (treeData):', dataSource);
+
+            var items = extrairItensDoTreeView(dataSource, null, 0);
+            console.log('[AUTO-SAVE] Items extraídos para salvar:', items);
+            console.log('[AUTO-SAVE] Total de items:', items.length);
+
+            if (items.length === 0) {
+                console.warn('[AUTO-SAVE] Nenhum item para salvar!');
+                return;
+            }
+
+            console.log('[AUTO-SAVE] Enviando para backend...');
+
+            var jsonPayload = JSON.stringify(items);
+            console.log('[AUTO-SAVE] JSON Payload (primeiros 500 chars):', jsonPayload.substring(0, 500));
+
+            fetch('/api/Navigation/SaveTreeToDb', {
+                method: 'POST',
+                headers: { 'Content-Type': 'application/json' },
+                body: jsonPayload
+            })
+            .then(r => {
+                console.log('[AUTO-SAVE] Response status:', r.status);
+
+                return r.json().then(data => ({ ok: r.ok, status: r.status, data: data }));
+            })
+            .then(response => {
+                console.log('[AUTO-SAVE] Response do backend:', response.data);
+
+                if (!response.ok) {
+
+                    console.error('[AUTO-SAVE] ❌ HTTP Error:', response.status);
+
+                    if (response.data.errors) {
+                        console.error('[AUTO-SAVE] ❌ Erros de Validação:', JSON.stringify(response.data.errors, null, 2));
+                        var erroMsg = 'Erro de validação: ';
+                        for (var campo in response.data.errors) {
+                            erroMsg += campo + ': ' + response.data.errors[campo].join(', ') + '; ';
+                        }
+                        mostrarAlerta(erroMsg, 'danger');
+                    } else {
+                        mostrarAlerta('Erro HTTP ' + response.status + ': ' + (response.data.message || response.data.title || 'Erro desconhecido'), 'danger');
+                    }
                     return;
                 }
 
-                treeData = JSON.parse(JSON.stringify(treeDataOriginal));
-
-                treeObj.fields.dataSource = treeData;
-                treeObj.dataBind();
-                treeObj.refresh();
-                treeObj.expandAll();
-
-                configurarBotoesNavegacao();
-
-                selectedItem = null;
-                limparFormulario(false);
-
-                alteracoesPendentes = false;
-                esconderCardAlteracoes();
-
-                mostrarAlerta('Alterações canceladas. Estrutura restaurada.', 'info');
-
-                console.log('[ALTERAÇÕES] ✅ Estado original restaurado');
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "cancelarAlteracoes", erro);
-            }
-        }
-
-            /**
-             * Configura event listeners dos botões de alterações
-             * @@description Atribui confirmarAlteracoes e cancelarAlteracoes aos botões
-            * @@returns { void}
-            */
-        function configurarBotoesAlteracoes() {
-            var btnConfirmar = document.getElementById('btnConfirmarAlteracoes');
-            var btnCancelar = document.getElementById('btnCancelarAlteracoes');
-
-            if (btnConfirmar) {
-                btnConfirmar.onclick = confirmarAlteracoes;
-            }
-            if (btnCancelar) {
-                btnCancelar.onclick = cancelarAlteracoes;
-            }
-
-            console.log('Botões de confirmação/cancelamento configurados');
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CÁLCULO DE ORDEM AUTOMÁTICA
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Calcula ordem automaticamente baseada nos filhos do pai
-             * @@description Define campo txtOrdem com próximo valor disponível
-            * @@param { string } paiId - ID do item pai
-                * @@returns { void}
-                */
-            function calcularOrdemAutomatica(paiId) {
-            try {
-                console.log('Calculando ordem para pai:', paiId);
-
-                var pai = buscarItemPorId(treeData, paiId);
-
-                if (!pai) {
-                    console.warn('Pai não encontrado no treeData');
-                    document.getElementById('txtOrdem').value = 0;
-                    return;
-                }
-
-                var filhos = pai.items || [];
-                console.log('Filhos encontrados:', filhos.length);
-
-                if (filhos.length === 0) {
-
-                    document.getElementById('txtOrdem').value = 0;
-                    console.log('Ordem definida: 0 (primeiro filho)');
-                    return;
-                }
-
-                var maiorOrdem = -1;
-                filhos.forEach(function (filho) {
-                    var ordem = parseFloat(filho.ordem) || 0;
-                    if (ordem > maiorOrdem) {
-                        maiorOrdem = ordem;
-                    }
-                });
-
-                var novaOrdem = maiorOrdem + 1;
-                document.getElementById('txtOrdem').value = novaOrdem;
-                console.log('Ordem calculada:', novaOrdem, '(maior atual:', maiorOrdem, ')');
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "calcularOrdemAutomatica", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CARREGAMENTO E RENDERIZAÇÃO DA ÁRVORE
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Carrega árvore de navegação do banco de dados
-             * @@description Chama API GetTreeAdmin e renderiza o TreeView
-            * @@returns { void}
-            */
-        function carregarArvore() {
-            try {
-                console.log('[DEBUG] Iniciando carregamento da árvore...');
-                fetch('/api/Navigation/GetTreeAdmin')
-                    .then(r => {
-                        console.log('[DEBUG] Response recebido:', r.status, r.statusText);
-                        return r.json();
-                    })
-                    .then(result => {
-                        console.log('[DEBUG] Dados recebidos da API:', result);
-                        console.log('[DEBUG] result.success:', result.success);
-                        console.log('[DEBUG] result.data:', result.data);
-                        console.log('[DEBUG] result.data.length:', result.data ? result.data.length : 'undefined');
-
-                        if (result.success && result.data && result.data.length > 0) {
-                            console.log('[DEBUG] ✅ Condição OK - Renderizando árvore com', result.data.length, 'itens');
-                            treeData = result.data;
-
-                            var emptyState = document.getElementById('emptyTreeState');
-                            if (emptyState) {
-                                emptyState.style.display = 'none';
-                            }
-
-                            console.log('[DEBUG] Chamando renderizarTreeView()...');
-                            renderizarTreeView();
-                            console.log('[DEBUG] Chamando atualizarContagem()...');
-                            atualizarContagem();
-                            console.log('[DEBUG] Chamando popularDropdownItemPai()...');
-
-                            popularDropdownItemPai();
-
-                            popularDropdownPosicao();
-
-                            guardarEstadoOriginal();
-
-                            console.log('[DEBUG] ✅ Árvore carregada com sucesso!');
-                        } else {
-                            console.log('[DEBUG] ❌ Condição falhou - Mostrando estado vazio');
-                            console.log('[DEBUG] Motivo: success=' + result.success + ', data=' + (result.data ? 'existe' : 'null/undefined') + ', length=' + (result.data ? result.data.length : 'N/A'));
-
-                            var emptyState = document.getElementById('emptyTreeState');
-                            if (emptyState) {
-                                emptyState.style.display = 'block';
-                            }
-
-                            var itemCount = document.getElementById('itemCount');
-                            if (itemCount) {
-                                itemCount.textContent = '0 itens';
-                            }
-                        }
-                    })
-                    .catch(erro => {
-                        console.error('[DEBUG] ❌ ERRO no fetch:', erro);
-                        Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarArvore", erro);
-                    });
-            } catch (erro) {
-                console.error('[DEBUG] ❌ ERRO no try-catch:', erro);
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarArvore", erro);
-            }
-        }
-
-            /**
-             * Renderiza o TreeView usando Syncfusion EJ2
-             * @@description Cria instância do TreeView com templates customizados
-            * Configura nodeTemplate, drag - and - drop e eventos de clique
-                * @@returns { void}
-                */
-        function renderizarTreeView() {
-            try {
-                console.log('[DEBUG renderizarTreeView] Iniciando renderização...');
-                console.log('[DEBUG renderizarTreeView] treeData:', treeData);
-                console.log('[DEBUG renderizarTreeView] treeData.length:', treeData ? treeData.length : 'undefined');
-
-                if (treeObj) {
-                    console.log('[DEBUG renderizarTreeView] Destruindo instância anterior do TreeView...');
-                    try {
-                        treeObj.destroy();
-                        treeObj = null;
-                    } catch (error) {
-                        console.warn('[DEBUG renderizarTreeView] Erro ao destruir TreeView anterior:', error);
-                    }
-                }
-
-                var container = document.getElementById('gestaoTreeView');
-                console.log('[DEBUG renderizarTreeView] Container encontrado:', container);
-                container.innerHTML = '<div id="treeViewControl"></div>';
-
-                console.log('[DEBUG renderizarTreeView] Criando nova instância do TreeView com', treeData.length, 'itens');
-
-                treeObj = new ej.navigations.TreeView({
-                    fields: {
-                        dataSource: treeData,
-                        id: 'id',
-                        text: 'text',
-                        child: 'items'
-                    },
-                    nodeTemplate: function (data) {
-
-                        var isGroup = (data.items && data.items.length > 0) || !data.href || data.href === 'javascript:void(0);' || data.href === '';
-
-                        if (data.href && data.href !== 'javascript:void(0);' && data.href !== '') {
-                            isGroup = false;
-                        }
-
-                        var badge = isGroup ? 'Grupo' : 'Página';
-                        var badgeClass = isGroup ? 'badge-grupo' : 'badge-pagina';
-                        var badgeTitle = isGroup ? 'Clique para transformar em Página' : 'Clique para transformar em Grupo';
-                        var displayText = decodeHtml(data.text);
-
-                        var iconClass = data.icon || 'fa-duotone fa-folder';
-                        if (iconClass && !iconClass.includes('fa-duotone')) {
-                            iconClass = iconClass.replace(/fa-(regular|solid|light)/g, 'fa-duotone');
-                        }
-
-                        var btnsEsquerda =
-                            '<div class="tree-nav-buttons tree-nav-left">' +
-                            '<button type="button" class="tree-nav-btn btn-order btn-mover-cima" data-itemid="' + data.id + '" title="Mover para cima">' +
-                            '<i class="fa-duotone fa-arrow-up-from-bracket"></i>' +
-                            '</button>' +
-                            '<button type="button" class="tree-nav-btn btn-order btn-mover-baixo" data-itemid="' + data.id + '" title="Mover para baixo">' +
-                            '<i class="fa-duotone fa-arrow-down-from-bracket"></i>' +
-                            '</button>' +
-                            '</div>';
-
-                        var btnsDireita =
-                            '<div class="tree-nav-buttons tree-nav-right">' +
-                            '<button type="button" class="tree-nav-btn btn-level btn-mover-esq" data-itemid="' + data.id + '" title="Subir nível (vira irmão do pai)">' +
-                            '<i class="fa-duotone fa-arrow-left-from-bracket"></i>' +
-                            '</button>' +
-                            '<button type="button" class="tree-nav-btn btn-level btn-mover-dir" data-itemid="' + data.id + '" title="Descer nível (vira filho do item acima)">' +
-                            '<i class="fa-duotone fa-arrow-right-from-bracket"></i>' +
-                            '</button>' +
-                            '</div>';
-
-                        var badgeBtn = '<button type="button" class="node-badge ' + badgeClass + ' btn-toggle-tipo" ' +
-                            'data-itemid="' + data.id + '" title="' + badgeTitle + '">' + badge + '</button>';
-
-                        return '<div class="tree-node" data-id="' + data.id + '">' +
-                            btnsEsquerda +
-                            '<div class="tree-nav-separator"></div>' +
-                            '<i class="' + iconClass + ' icon-duotone-ftx"></i>' +
-                            '<span class="node-text ' + (isGroup ? 'node-group' : '') + '">' + displayText + '</span>' +
-                            badgeBtn +
-                            '<div class="tree-nav-separator"></div>' +
-                            btnsDireita +
-                            '</div>';
-                    },
-                    allowDragAndDrop: false,
-                    allowDropSibling: false,
-                    allowDropChild: false,
-                    allowMultiSelection: false,
-                    nodeClicked: function (args) {
-                        try {
-                            console.log('=== nodeClicked event ===', args);
-
-                            if (!args || !args.node) {
-                                console.warn('Node clicado sem elemento válido:', args);
-                                return;
-                            }
-
-                            var nodeElement = args.node;
-                            var treeNodeDiv = nodeElement.querySelector('.tree-node');
-
-                            if (!treeNodeDiv) {
-                                console.error('Elemento .tree-node não encontrado dentro do node');
-                                return;
-                            }
-
-                            var nodeId = treeNodeDiv.getAttribute('data-id');
-                            console.log('Node clicado - ID extraído do DOM:', nodeId);
-
-                            if (!nodeId) {
-                                console.error('data-id não encontrado no elemento');
-                                return;
-                            }
-
-                            var itemCompleto = buscarItemPorId(treeData, nodeId);
-                            console.log('Item encontrado no treeData:', itemCompleto);
-
-                            if (itemCompleto) {
-                                selecionarItem(itemCompleto);
-                            } else {
-                                console.error('Item não encontrado no treeData para ID:', nodeId);
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "renderizarTreeView.nodeClicked", error);
-                        }
-                    },
-                    expandOn: 'Click'
-                });
-
-                treeObj.appendTo('#treeViewControl');
-                console.log('✅ TreeView renderizado com sucesso! Total de itens:', treeData.length);
-
-                configurarBotoesNavegacao();
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "renderizarTreeView", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * BOTÕES DE NAVEGAÇÃO DA ÁRVORE
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Configura event listeners para botões de navegação
-             * @@description Usa delegação de eventos para botões no TreeView
-            * @@returns { void}
-            */
-        function configurarBotoesNavegacao() {
-            var treeContainer = document.getElementById('gestaoTreeView');
-            if (!treeContainer) {
-                console.error('[NAV-BTNS] Container da árvore não encontrado');
-                return;
-            }
-
-            treeContainer.removeEventListener('click', handleNavButtonClick);
-
-            treeContainer.addEventListener('click', handleNavButtonClick, true);
-
-            console.log('Event listeners de navegação configurados');
-        }
-
-            /**
-             * Handler para cliques nos botões de navegação
-             * @@description Identifica qual botão foi clicado e executa ação correspondente
-            * @@param { Event } event - Evento de clique
-                * @@returns { void}
-                */
-        function handleNavButtonClick(event) {
-            var target = event.target;
-
-            if (target.tagName === 'I') {
-                target = target.parentElement;
-            }
-
-            var isNavBtn = target.classList.contains('tree-nav-btn');
-            var isToggleTipo = target.classList.contains('btn-toggle-tipo');
-
-            if (!isNavBtn && !isToggleTipo) {
-                return;
-            }
-
-            event.stopPropagation();
-            event.preventDefault();
-
-            var itemId = target.getAttribute('data-itemid');
-            console.log('[NAV-BTN] Botão clicado, itemId:', itemId);
-
-            if (!itemId) {
-                console.error('[NAV-BTN] data-itemid não encontrado no botão');
-                return;
-            }
-
-            if (isToggleTipo) {
-                alternarTipoItem(itemId, event);
-                return;
-            }
-
-            if (target.classList.contains('btn-mover-cima')) {
-                moverItemCima(itemId, event);
-            } else if (target.classList.contains('btn-mover-baixo')) {
-                moverItemBaixo(itemId, event);
-            } else if (target.classList.contains('btn-mover-esq')) {
-                moverItemEsquerda(itemId, event);
-            } else if (target.classList.contains('btn-mover-dir')) {
-                moverItemDireita(itemId, event);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * SELEÇÃO E EDIÇÃO DE ITENS
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Seleciona item e carrega suas propriedades no formulário
-             * @@description Preenche todos os campos de edição com dados do item
-            * @@param { Object } itemData - Dados do item selecionado
-                * @@returns { void}
-                */
-            function selecionarItem(itemData) {
-            try {
-                console.log('=== INICIO selecionarItem ===');
-                console.log('itemData recebido:', itemData);
-
-                if (!itemData) {
-                    console.error('itemData é null ou undefined');
-                    return;
-                }
-
-                selectedItem = itemData;
-
-                var propsCard = document.getElementById('propsCard');
-                if (!propsCard) {
-                    console.error('Elemento propsCard não encontrado!');
-                    return;
-                }
-                propsCard.style.display = 'block';
-                console.log('Card de propriedades mostrado');
-
-                document.querySelectorAll('.tree-node').forEach(n => n.classList.remove('selected'));
-                var node = document.querySelector('.tree-node[data-id="' + selectedItem.id + '"]');
-                if (node) {
-                    node.classList.add('selected');
-                    console.log('Node marcado visualmente');
-                }
-
-                var recursoId = document.getElementById('recursoId');
-                var parentId = document.getElementById('parentId');
-                var txtNome = document.getElementById('txtNome');
-                var txtNomeMenu = document.getElementById('txtNomeMenu');
-                var txtHref = document.getElementById('txtHref');
-                var txtIconClass = document.getElementById('txtIconClass');
-                var txtOrdem = document.getElementById('txtOrdem');
-                var txtDescricao = document.getElementById('txtDescricao');
-                var chkAtivo = document.getElementById('chkAtivo');
-                var selectedItemName = document.getElementById('selectedItemName');
-                var modoEdicao = document.getElementById('modoEdicao');
-
-                if (recursoId) recursoId.value = selectedItem.id || '';
-                if (parentId) parentId.value = selectedItem.parentId || '';
-                if (txtNome) txtNome.value = decodeHtml(selectedItem.text) || '';
-                if (txtNomeMenu) txtNomeMenu.value = decodeHtml(selectedItem.nomeMenu) || '';
-                if (txtHref) txtHref.value = selectedItem.href || '';
-                if (txtOrdem) txtOrdem.value = selectedItem.ordem || 0;
-                if (txtDescricao) txtDescricao.value = decodeHtml(selectedItem.descricao) || '';
-                if (chkAtivo) chkAtivo.checked = selectedItem.ativo !== false;
-                if (selectedItemName) selectedItemName.textContent = decodeHtml(selectedItem.text);
-
-                console.log('Campos básicos preenchidos (decodificados)');
-
-                atualizarEstadoBotoesOrdenacao();
-
-                var iconClass = selectedItem.icon || 'fa-duotone fa-file';
-                if (txtIconClass) txtIconClass.value = iconClass;
-
-                var ddlIconeObj = document.getElementById('ddlIcone');
-                if (ddlIconeObj && ddlIconeObj.ej2_instances && ddlIconeObj.ej2_instances[0]) {
-                    var ddlIcone = ddlIconeObj.ej2_instances[0];
-
-                    var iconClassNormalizado = iconClass;
-                    if (iconClass) {
-
-                        iconClassNormalizado = iconClass.replace(/^fa-(regular|solid|light|duotone)\s+/, 'fa-duotone ');
-
-                        if (!iconClassNormalizado.startsWith('fa-')) {
-                            iconClassNormalizado = 'fa-duotone ' + iconClassNormalizado;
-                        }
-                    }
-
-                    ddlIcone.value = null;
-                    ddlIcone.dataBind();
-
-                    setTimeout(function () {
-                        if (ddlIcone.fields && ddlIcone.fields.dataSource) {
-                            var dataSource = ddlIcone.fields.dataSource;
-                            var iconEncontrado = null;
-
-                            for (var i = 0; i < dataSource.length; i++) {
-                                var cat = dataSource[i];
-                                if (cat.child && cat.child.length > 0) {
-                                    for (var j = 0; j < cat.child.length; j++) {
-                                        var icon = cat.child[j];
-
-                                        var iconIdStr = String(icon.id || '');
-                                        var iconClassStr = String(iconClassNormalizado || '');
-                                        var iconClassOriginalStr = String(iconClass || '');
-
-                                        var iconNameNormalizado = iconClassNormalizado.replace(/^fa-duotone\s+/, '');
-                                        var iconNameOriginal = iconClassOriginalStr.replace(/^fa-(regular|solid|light|duotone)\s+/, '');
-
-                                        if (iconIdStr === iconClassNormalizado ||
-                                            iconIdStr === iconClassOriginalStr ||
-                                            iconIdStr.endsWith(' ' + iconNameNormalizado) ||
-                                            iconIdStr.endsWith(' ' + iconNameOriginal)) {
-                                            iconEncontrado = icon;
-                                            break;
-                                        }
-                                    }
-                                }
-                                if (iconEncontrado) break;
-                            }
-
-                            if (iconEncontrado) {
-                                ddlIcone.value = [iconEncontrado.id];
-                                ddlIcone.dataBind();
-                                console.log('✅ DropDownTree de ícone atualizado para:', iconEncontrado.id);
-                            } else {
-                                console.warn('Ícone não encontrado no dataSource. Tentando:', iconClassNormalizado, 'ou', iconClass);
-                            }
-                        }
-                    }, 500);
-                }
-
-                var href = selectedItem.href || '';
-                console.log('URL do item (href):', href);
-
-                if (href && href.endsWith('.html')) {
-                    var ddlPaginaObj = document.getElementById('ddlPagina');
-                    if (ddlPaginaObj && ddlPaginaObj.ej2_instances && ddlPaginaObj.ej2_instances[0]) {
-                        var ddlPagina = ddlPaginaObj.ej2_instances[0];
-
-                        var pageId = null;
-                        if (ddlPagina.fields && ddlPagina.fields.dataSource) {
-                            var dataSource = ddlPagina.fields.dataSource;
-                            console.log('Buscando página com paginaRef:', href);
-
-                            for (var i = 0; i < dataSource.length; i++) {
-                                var category = dataSource[i];
-                                if (category.child && category.child.length > 0) {
-
-                                    for (var j = 0; j < category.child.length; j++) {
-                                        var page = category.child[j];
-                                        if (page.paginaRef === href) {
-                                            pageId = page.id;
-                                            console.log('✅ Página encontrada! ID:', pageId, '| Módulo:', category.text, '| Página:', page.text);
-                                            break;
-                                        }
-                                    }
-                                }
-                                if (pageId) break;
-                            }
-                        }
-
-                        if (pageId) {
-
-                            ddlPagina.value = null;
-
-                            setTimeout(function () {
-                                ddlPagina.value = [pageId];
-                                console.log('✅ DropDownTree de página atualizado para:', pageId);
-                            }, 100);
-                        } else {
-                            console.warn('⚠️ Página não encontrada no DropDownTree para href:', href);
-                        }
-                    }
+                if (response.data.success) {
+                    console.log('[AUTO-SAVE] ✅ Ordenação salva automaticamente!');
+
+                    var msg = 'Ordenação salva automaticamente';
+                    if (lastDragInfo && lastDragInfo.itemName) {
+                        msg = "Item '" + lastDragInfo.itemName + "' movido de '" +
+                            (lastDragInfo.fromParentName || 'Sem grupo') + "' para '" +
+                            (lastDragInfo.toParentName || 'Sem grupo') + "'";
+                    }
+                    mostrarToastSucesso(msg);
+
+                    carregarArvore();
+
+                    atualizarNavegacaoLateral();
                 } else {
-                    console.log('URL não está no formato esperado ou está vazia');
-                }
-
-                var parentId = selectedItem.parentId;
-                console.log('Parent ID do item:', parentId);
-                document.getElementById('parentId').value = parentId || '';
-
-                if (modoEdicao) {
-                    modoEdicao.textContent = 'Editar';
-                    modoEdicao.className = 'badge bg-warning text-dark ms-2';
-                }
-
-                habilitarCardPropriedades(true);
-
-                carregarControleAcesso(selectedItem.id);
-                document.getElementById('acessoCard').style.display = 'block';
-
-                atualizarToggleTipo(selectedItem);
-
-                console.log('=== FIM selecionarItem (sucesso) ===');
-            } catch (error) {
-                console.error('ERRO em selecionarItem:', error);
-                console.error('Stack:', error.stack);
-                mostrarAlerta('Erro ao selecionar item: ' + error.message, 'danger');
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * BUSCA RECURSIVA NA ÁRVORE
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Busca item por ID recursivamente no treeData
-             * @@param { Array } items - Lista de itens para buscar
-            * @@param { string } id - ID do item a buscar
-                * @@returns { Object| null} Item encontrado ou null
-                    */
-        function buscarItemPorId(items, id) {
-            try {
-                if (!items || !id) return null;
-
-                for (var i = 0; i < items.length; i++) {
-
-                    if (String(items[i].id) === String(id)) {
-                        return items[i];
-                    }
-                    if (items[i].items && items[i].items.length > 0) {
-                        var found = buscarItemPorId(items[i].items, id);
-                        if (found) return found;
-                    }
-                }
-                return null;
-            } catch (erro) {
-                console.error('[buscarItemPorId] Erro:', erro);
-                return null;
-            }
-        }
-
-            /**
-             * Remove um item da árvore recursivamente
-             * @@param { Array } items - Lista de itens
-            * @@param { string } id - ID do item a remover
-                * @@returns { boolean } true se removeu, false se não encontrou
-                    */
-        function removerItemDaArvore(items, id) {
-            try {
-                if (!items || !id) return false;
-
-                for (var i = 0; i < items.length; i++) {
-                    if (String(items[i].id) === String(id)) {
-
-                        items.splice(i, 1);
-                        return true;
-                    }
-
-                    if (items[i].items && items[i].items.length > 0) {
-                        if (removerItemDaArvore(items[i].items, id)) {
-                            return true;
-                        }
-                    }
-                }
-                return false;
-            } catch (erro) {
-                console.error('[removerItemDaArvore] Erro:', erro);
-                return false;
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CARREGAMENTO DE ÍCONES E PÁGINAS
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Carrega ícones FontAwesome do endpoint da API
-             * @@description Popula o DropDownTree com ícones hierárquicos
-            * @@returns { void}
-            */
-        function carregarIconesFontAwesome() {
-            try {
-                fetch('/api/Navigation/GetIconesFontAwesomeHierarquico')
-                    .then(r => r.json())
-                    .then(result => {
-                        console.log('Ícones FontAwesome carregados:', result);
-                        if (result.success && result.data) {
-                            var ddlIconeObj = document.getElementById('ddlIcone').ej2_instances[0];
-                            if (ddlIconeObj) {
-
-                                ddlIconeObj.fields = {
-                                    dataSource: result.data,
-                                    value: 'id',
-                                    text: 'text',
-                                    parentValue: 'parentId',
-                                    hasChildren: 'hasChild',
-                                    child: 'child'
-                                };
-                                ddlIconeObj.dataBind();
-                                console.log('DropDownTree de ícones populado com', result.data.length, 'categorias');
-                            }
-                        }
-                    })
-                    .catch(erro => {
-                        Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarIconesFontAwesome", erro);
-                    });
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarIconesFontAwesome", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * EVENTOS DE SELEÇÃO DE ÍCONE
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Evento SELECT: Dispara ao clicar no ícone (não precisa clicar fora)
-             * @@param { Object } args - Argumentos do evento Syncfusion
-            * @@returns { void}
-            */
-            function onIconeSelect(args) {
-            try {
-                console.log('=== onIconeSelect disparado (AO CLICAR) ===');
-                console.log('args.nodeData:', args.nodeData);
-
-                if (!args.nodeData || args.nodeData.isCategory) {
-                    console.log('Categoria clicada - não atualiza campo');
-                    return;
-                }
-
-                var iconClass = args.nodeData.id;
-                var iconLabel = args.nodeData.text;
-
-                var txtIconClass = document.getElementById('txtIconClass');
-                if (txtIconClass) {
-                    txtIconClass.value = iconClass;
-                    console.log('Classe CSS atualizada IMEDIATAMENTE:', iconClass, '| Label:', iconLabel);
-                } else {
-                    console.error('Elemento txtIconClass não encontrado!');
-                }
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "onIconeSelect", erro);
-            }
-        }
-
-            /**
-             * Evento CHANGE: Backup para quando valor é setado programaticamente
-             * @@param { Object } args - Argumentos do evento Syncfusion
-            * @@returns { void}
-            */
-            function onIconeChange(args) {
-            try {
-                console.log('=== onIconeChange disparado ===');
-                console.log('args completo:', args);
-                console.log('args.itemData:', args.itemData);
-                console.log('args.value:', args.value);
-
-                var itemData = args.itemData;
-
-                if (!itemData && args.value && args.value.length > 0) {
-                    console.log('itemData undefined - buscando no dataSource...');
-                    var ddlIconeObj = document.getElementById('ddlIcone').ej2_instances[0];
-                    if (ddlIconeObj && ddlIconeObj.fields && ddlIconeObj.fields.dataSource) {
-                        var iconId = args.value[0];
-                        console.log('Buscando iconId:', iconId);
-
-                        itemData = findIconById(iconId);
-                        if (itemData) {
-                            console.log('Ícone encontrado no dataSource:', itemData);
-                        }
-
-                        if (!itemData) {
-                            console.log('Ícone não encontrado no dataSource - usando iconId diretamente');
-                            itemData = { id: iconId, text: iconId, isCategory: false };
-                        }
-                    }
-                }
-
-                if (!itemData || itemData.isCategory) {
-                    document.getElementById('txtIconClass').value = '';
-                    console.log('Categoria selecionada ou campo limpo');
-                    return;
-                }
-
-                var iconClass = itemData.id;
-                var iconLabel = itemData.text;
-
-                var txtIconClass = document.getElementById('txtIconClass');
-                if (txtIconClass) {
-                    txtIconClass.value = iconClass;
-                    console.log('Classe CSS atualizada:', iconClass, '| Label:', iconLabel);
-                } else {
-                    console.error('Elemento txtIconClass não encontrado!');
-                }
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "onIconeChange", erro);
-            }
-        }
-
-            /**
-             * Carrega páginas do sistema da API
-             * @@description Popula o DropDownTree de páginas
-            * @@returns { void}
-            */
-        function carregarPaginasSistema() {
-            try {
-                fetch('/api/Navigation/GetPaginasHierarquico')
-                    .then(r => r.json())
-                    .then(result => {
-                        console.log('Páginas carregadas:', result);
-                        if (result.success && result.data) {
-                            var ddlPaginaObj = document.getElementById('ddlPagina').ej2_instances[0];
-                            if (ddlPaginaObj) {
-                                ddlPaginaObj.fields = {
-                                    dataSource: result.data,
-                                    value: 'id',
-                                    text: 'text',
-                                    parentValue: 'parentId',
-                                    hasChildren: 'hasChild',
-                                    child: 'child'
-                                };
-                                ddlPaginaObj.dataBind();
-                                console.log('DropDownTree de páginas populado com', result.data.length, 'módulos');
-                            }
-                        }
-                    })
-                    .catch(erro => {
-                        Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarPaginasSistema", erro);
-                    });
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarPaginasSistema", erro);
-            }
-        }
-
-            /**
-             * Callback após criação do DropDownTree de páginas
-             * @@description Configura templates de item e valor para exibição
-            * @@returns { void}
-            */
-        function onPaginaDropdownCreated() {
-            try {
-                var ddlPaginaObj = document.getElementById('ddlPagina').ej2_instances[0];
-                if (ddlPaginaObj) {
-                    ddlPaginaObj.itemTemplate = function (data) {
-                        if (data.isCategory) {
-                            return '<div style="font-weight: 600; padding: 4px 0; color: #3D5771;">' + data.text + '</div>';
-                        }
-                        return '<div style="display: flex; align-items: center; gap: 8px;">' +
-                            '<i class="fa-duotone fa-file-lines" style="font-size: 16px; width: 20px; --fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i>' +
-                            '<span>' + data.text + '</span>' +
-                            '</div>';
-                    };
-
-                    ddlPaginaObj.valueTemplate = function (data) {
-                        if (!data || data.isCategory) return '';
-                        var displayText = data.displayText || data.text;
-                        return '<div style="display: flex; align-items: center; gap: 8px;">' +
-                            '<i class="fa-duotone fa-file-lines" style="font-size: 14px; width: 18px; --fa-primary-color: #ff6b35; --fa-secondary-color: #6c757d;"></i>' +
-                            '<span>' + displayText + '</span>' +
-                            '</div>';
-                    };
-
-                    ddlPaginaObj.selecting = function (args) {
-                        console.log('=== SELECTING página (ANTES) ===', args);
-                        if (args.nodeData && !args.nodeData.isCategory) {
-
-                            ultimaPaginaSelecionada = {
-                                id: args.nodeData.id,
-                                paginaRef: args.nodeData.paginaRef
-                            };
-
-                            var txtHref = document.getElementById('txtHref');
-                            if (txtHref && args.nodeData.paginaRef) {
-                                txtHref.value = args.nodeData.paginaRef;
-                                console.log('✅ URL atualizada via SELECTING:', args.nodeData.paginaRef);
-                            }
-                        }
-                    };
-
-                    ddlPaginaObj.select = function (args) {
-                        console.log('=== SELECT página ===', args);
-                        if (args.nodeData && !args.nodeData.isCategory) {
-
-                            ultimaPaginaSelecionada = {
-                                id: args.nodeData.id,
-                                paginaRef: args.nodeData.paginaRef
-                            };
-
-                            var txtHref = document.getElementById('txtHref');
-                            if (txtHref && args.nodeData.paginaRef) {
-                                txtHref.value = args.nodeData.paginaRef;
-                                console.log('✅ URL atualizada via SELECT:', args.nodeData.paginaRef);
-                            }
-
-                            ddlPaginaObj.value = [args.nodeData.id];
-                            onPaginaChange({ itemData: args.nodeData, value: [args.nodeData.id] });
-                            ddlPaginaObj.hidePopup();
-                        }
-                    };
-
-                    ddlPaginaObj.change = function (args) {
-                        onPaginaChange(args);
-                    };
-
-                    ddlPaginaObj.changeOnBlur = false;
-
-                    ddlPaginaObj.close = function (args) {
-                        console.log('=== CLOSE página ===');
-
-                        var paginaRef = null;
-                        var pageId = null;
-
-                        var valor = ddlPaginaObj.value;
-                        if (valor && valor.length > 0) {
-                            pageId = valor[0];
-                            console.log('Valor do componente:', pageId);
-
-                            if (ddlPaginaObj.fields && ddlPaginaObj.fields.dataSource) {
-                                var dataSource = ddlPaginaObj.fields.dataSource;
-                                for (var i = 0; i < dataSource.length; i++) {
-                                    var modulo = dataSource[i];
-                                    if (modulo.child && modulo.child.length > 0) {
-                                        for (var j = 0; j < modulo.child.length; j++) {
-                                            var page = modulo.child[j];
-                                            if (page.id === pageId && page.paginaRef) {
-                                                paginaRef = page.paginaRef;
-                                                break;
-                                            }
-                                        }
-                                    }
-                                    if (paginaRef) break;
-                                }
-                            }
-                        }
-
-                        if (!paginaRef && ultimaPaginaSelecionada) {
-                            paginaRef = ultimaPaginaSelecionada.paginaRef;
-                            console.log('Usando última página selecionada:', paginaRef);
-                        }
-
-                        if (paginaRef) {
-                            var txtHref = document.getElementById('txtHref');
-                            if (txtHref) {
-                                txtHref.value = paginaRef;
-                                console.log('✅ URL atualizada via CLOSE:', paginaRef);
-                            }
-                        }
-                    };
-
-                    console.log('DropDownTree de páginas configurado');
-                }
-            } catch (error) {
-                console.error('Erro ao configurar DropDownTree de páginas:', error);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * EVENTOS DE SELEÇÃO DE PÁGINA
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Evento SELECT: Dispara ao clicar na página (não precisa clicar fora)
-             * @@param { Object } args - Argumentos do evento Syncfusion
-            * @@returns { void}
-            */
-            function onPaginaSelect(args) {
-            try {
-                console.log('=== onPaginaSelect disparado (AO CLICAR) ===');
-                console.log('args.nodeData:', args.nodeData);
-
-                if (!args.nodeData || args.nodeData.isCategory) {
-                    console.log('Categoria clicada - não gera URL');
-                    return;
-                }
-
-                var paginaRef = args.nodeData.paginaRef;
-                console.log('paginaRef extraída:', paginaRef);
-
-                var txtHref = document.getElementById('txtHref');
-                if (txtHref) {
-                    txtHref.value = paginaRef;
-                    console.log('URL atualizada IMEDIATAMENTE:', paginaRef);
-                } else {
-                    console.error('Elemento txtHref não encontrado!');
-                }
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "onPaginaSelect", erro);
-            }
-        }
-
-            /**
-             * Evento CHANGE: Backup para quando valor é setado programaticamente
-             * @@param { Object } args - Argumentos do evento Syncfusion
-            * @@returns { void}
-            */
-            function onPaginaChange(args) {
-            try {
-                console.log('=== onPaginaChange disparado ===');
-                console.log('args completo:', args);
-                console.log('args.itemData:', args.itemData);
-                console.log('args.value:', args.value);
-
-                var itemData = args.itemData;
-
-                if (!itemData && args.value && args.value.length > 0) {
-                    console.log('itemData undefined - buscando no dataSource...');
-                    var ddlPaginaObj = document.getElementById('ddlPagina').ej2_instances[0];
-                    if (ddlPaginaObj && ddlPaginaObj.fields && ddlPaginaObj.fields.dataSource) {
-                        var pageId = args.value[0];
-                        console.log('Buscando pageId:', pageId);
-
-                        var dataSource = ddlPaginaObj.fields.dataSource;
-                        for (var i = 0; i < dataSource.length; i++) {
-                            var category = dataSource[i];
-                            if (category.child && category.child.length > 0) {
-                                for (var j = 0; j < category.child.length; j++) {
-                                    if (category.child[j].id === pageId) {
-                                        itemData = category.child[j];
-                                        console.log('Item encontrado no dataSource:', itemData);
-                                        break;
-                                    }
-                                }
-                            }
-                            if (itemData) break;
-                        }
-                    }
-                }
-
-                if (!itemData || itemData.isCategory) {
-                    console.log('Categoria selecionada ou campo limpo - não gera URL');
-                    return;
-                }
-
-                var paginaRef = itemData.paginaRef;
-                console.log('paginaRef extraída:', paginaRef);
-
-                var txtHref = document.getElementById('txtHref');
-                if (txtHref) {
-                    txtHref.value = paginaRef;
-                    console.log('URL atualizada no campo txtHref:', paginaRef);
-                } else {
-                    console.error('Elemento txtHref não encontrado!');
-                }
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "onPaginaChange", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CONTROLE DE ACESSO (PERMISSÕES DE USUÁRIO)
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Carrega lista de usuários com acesso ao recurso
-             * @@param { string } recursoId - ID do recurso de navegação
-            * @@returns { void}
-            */
-        function carregarControleAcesso(recursoId) {
-            try {
-                if (!recursoId) {
-                    document.getElementById('listaAcessos').innerHTML = '<p class="text-muted text-center">Selecione um item para gerenciar acessos</p>';
-                    return;
-                }
-
-                fetch('/api/Navigation/GetUsuariosAcesso?recursoId=' + recursoId)
-                    .then(r => r.json())
-                    .then(result => {
-                        console.log('Acessos carregados:', result);
-                        var container = document.getElementById('listaAcessos');
-                        if (result.success && result.data && result.data.length > 0) {
-                            container.innerHTML = result.data.map(u =>
-                                '<div class="acesso-item">' +
-                                '<input type="checkbox" class="form-check-input me-2" ' +
-                                'id="acesso_' + u.usuarioId + '" ' +
-                                (u.acesso ? 'checked' : '') +
-                                ' onchange="atualizarAcesso(\'' + u.usuarioId + '\', this.checked)" />' +
-                                '<label for="acesso_' + u.usuarioId + '">' + u.nome + '</label>' +
-                                '</div>'
-                            ).join('');
-                        } else {
-                            container.innerHTML = '<p class="text-muted text-center">Nenhum usuário encontrado</p>';
-                        }
-                    })
-                    .catch(erro => {
-                        Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarControleAcesso", erro);
-                    });
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "carregarControleAcesso", erro);
-            }
-        }
-
-            /**
-             * Atualiza acesso de usuário ao recurso
-             * @@param { string } usuarioId - ID do usuário
-            * @@param { boolean } acesso - true para habilitar, false para remover
-                * @@returns { void}
-                */
-        function atualizarAcesso(usuarioId, acesso) {
-            try {
-                if (!selectedItem) return;
-
-                fetch('/api/Navigation/UpdateAcesso', {
-                    method: 'POST',
-                    headers: { 'Content-Type': 'application/json' },
-                    body: JSON.stringify({
-                        usuarioId: usuarioId,
-                        recursoId: selectedItem.id,
-                        acesso: acesso
-                    })
-                })
-                    .then(r => r.json())
-                    .then(result => {
-                        if (result.success) {
-                            mostrarAlerta('Acesso atualizado!', 'success');
-                        } else {
-                            mostrarAlerta('Erro ao atualizar acesso: ' + result.message, 'danger');
-                        }
-                    })
-                    .catch(erro => {
-                        Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "atualizarAcesso", erro);
-                    });
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "atualizarAcesso", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * SALVAMENTO DE PROPRIEDADES DO ITEM
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Salva propriedades do item no banco de dados
-             * @@description Valida campos, codifica HTML e envia para API
-            * @@returns { Promise } Promise que resolve com sucesso ou rejeita com erro
-                */
-        function salvarPropriedades() {
-            return new Promise(function (resolve, reject) {
-                try {
-
-                    var tipoItem = document.getElementById('tipoItem').value;
-                    var ehGrupo = (tipoItem === 'grupo');
-
-                    var txtHref = document.getElementById('txtHref').value;
-                    var parentIdValue = document.getElementById('parentId').value;
-
-                    var hrefFinal;
-                    if (ehGrupo) {
-
-                        hrefFinal = 'javascript:void(0);';
-                    } else {
-
-                        hrefFinal = txtHref || null;
-
-                        if (!hrefFinal) {
-                            Alerta.Warning('Atenção', 'Para itens do tipo Página, selecione uma página do sistema!');
-                            reject(new Error('URL não informada'));
-                            return;
-                        }
-                    }
-
-                    var posicaoSelecionada = null;
-                    var ddlPosicaoObj = document.getElementById('ddlPosicao');
-                    if (ddlPosicaoObj && ddlPosicaoObj.ej2_instances && ddlPosicaoObj.ej2_instances[0]) {
-                        var ddlPosicao = ddlPosicaoObj.ej2_instances[0];
-                        posicaoSelecionada = ddlPosicao.value && ddlPosicao.value.length > 0 ? ddlPosicao.value[0] : null;
-                    }
-                    console.log('[SALVAR] Posição selecionada:', posicaoSelecionada);
-
-                    var parentIdFinal = parentIdValue || null;
-                    var ehNovoItem = !document.getElementById('recursoId').value;
-
-                    if (ehNovoItem && posicaoSelecionada && posicaoSelecionada !== '_INICIO_') {
-
-                        var itemReferencia = buscarItemPorId(treeData, posicaoSelecionada);
-                        if (itemReferencia) {
-                            parentIdFinal = itemReferencia.parentId || null;
-                        }
-                    }
-
-                    var dto = {
-                        id: document.getElementById('recursoId').value,
-                        text: encodeHtml(document.getElementById('txtNome').value),
-                        nomeMenu: encodeHtml(document.getElementById('txtNomeMenu').value),
-                        href: hrefFinal,
-                        icon: document.getElementById('txtIconClass').value,
-                        ordem: 0,
-                        descricao: encodeHtml(document.getElementById('txtDescricao').value),
-                        ativo: document.getElementById('chkAtivo').checked,
-                        parentId: parentIdFinal,
-                        posicaoAbaixoDe: posicaoSelecionada
-                    };
-
-                    console.log('DTO codificado para envio:', dto);
-
-                    if (!dto.text || !dto.nomeMenu) {
-                        Alerta.Warning('Atenção', 'Preencha Nome e NomeMenu!');
-                        reject(new Error('Campos obrigatórios não preenchidos'));
-                        return;
-                    }
-
-                    var ehNovoItem = !dto.id;
-
-                    fetch('/api/Navigation/SaveRecurso', {
-                        method: 'POST',
-                        headers: { 'Content-Type': 'application/json' },
-                        body: JSON.stringify(dto)
-                    })
-                        .then(r => r.json())
-                        .then(result => {
-                            if (result.success) {
-                                mostrarAlerta(result.message, 'success');
-
-                                setTimeout(function () {
-                                    carregarArvore();
-                                }, 300);
-
-                                atualizarNavegacaoLateral();
-
-                                var recursoId = result.id || dto.id;
-
-                                if (ehNovoItem && recursoId) {
-
-                                    habilitarAcessoTodosUsuarios(recursoId);
-
-                                    Alerta.Confirmar(
-                                        'Gerenciar Controle de Acesso',
-                                        'Deseja gerenciar o controle de acesso deste item agora?',
-                                        'Sim',
-                                        'Não'
-                                    ).then(function (result) {
-                                        if (result) {
-
-                                            selectedItem = {
-                                                id: recursoId,
-                                                text: dto.text,
-                                                nomeMenu: dto.nomeMenu,
-                                                href: dto.href,
-                                                icon: dto.icon,
-                                                ordem: dto.ordem,
-                                                descricao: dto.descricao,
-                                                ativo: dto.ativo,
-                                                parentId: dto.parentId
-                                            };
-                                            document.getElementById('recursoId').value = recursoId;
-
-                                            habilitarCardPropriedades(false);
-
-                                            carregarControleAcesso(recursoId);
-
-                                            document.getElementById('acessoCard').style.display = 'block';
-                                        } else {
-
-                                            document.getElementById('propsCard').style.display = 'none';
-                                            document.getElementById('acessoCard').style.display = 'none';
-                                            habilitarCardPropriedades(true);
-                                        }
-                                    });
-                                } else {
-
-                                    document.getElementById('propsCard').style.display = 'none';
-                                    habilitarCardPropriedades(true);
-                                }
-
-                                resolve(result);
-                            } else {
-                                mostrarAlerta('Erro: ' + result.message, 'danger');
-                                reject(new Error(result.message));
-                            }
-                        })
-                        .catch(erro => {
-                            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarPropriedades", erro);
-                            reject(erro);
-                        });
-                } catch (erro) {
-                    Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarPropriedades", erro);
-                    reject(erro);
-                }
+                    console.error('[AUTO-SAVE] ❌ Erro no backend:', response.data.message);
+                    mostrarAlerta('Erro ao salvar ordenação: ' + response.data.message, 'danger');
+                }
+            })
+            .catch(erro => {
+                console.error('[AUTO-SAVE] ❌ Erro no fetch:', erro);
+                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarOrdenacaoAutomatica", erro);
             });
-        }
-
-        function adicionarNovoItem() {
-            try {
-
-                limparFormulario(false);
-
-                habilitarCardPropriedades(true);
-
-                document.getElementById('propsCard').style.display = 'block';
-
-                document.getElementById('acessoCard').style.display = 'none';
-
-                document.getElementById('txtNome').focus();
-                document.getElementById('selectedItemName').textContent = 'Novo Item';
-
-                document.getElementById('modoEdicao').textContent = 'Criar';
-                document.getElementById('modoEdicao').className = 'badge bg-success ms-2';
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "adicionarNovoItem", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CRUD DE ITENS DA ÁRVORE
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Exclui item selecionado da árvore
-             * @@description Pede confirmação e chama API de exclusão
-            * @@returns { void}
-            */
-        function excluirItem() {
-            try {
-                if (!selectedItem || !selectedItem.id) {
-                    mostrarAlerta('Selecione um item para excluir!', 'warning');
-                    return;
-                }
-
-                Alerta.Confirmar(
-                    'Confirmar Exclusão',
-                    'Tem certeza que deseja excluir "' + decodeHtml(selectedItem.text) + '"?',
-                    'Excluir',
-                    'Cancelar'
-                ).then(function (result) {
-                    if (!result) return;
-
-                    fetch('/api/Navigation/DeleteRecurso', {
-                        method: 'POST',
-                        headers: { 'Content-Type': 'application/json' },
-                        body: JSON.stringify({ recursoId: selectedItem.id })
-                    })
-                        .then(r => r.json())
-                        .then(result => {
-                            if (result.success) {
-                                mostrarAlerta(result.message, 'success');
-                                limparFormulario();
-                                carregarArvore();
-                            } else {
-                                mostrarAlerta('Erro: ' + result.message, 'danger');
-                            }
-                        })
-                        .catch(erro => {
-                            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "excluirItem", erro);
-                        });
-                });
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "excluirItem", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * VALIDAÇÃO DE FORMULÁRIO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Valida se campos obrigatórios estão preenchidos
-             * @@returns { boolean } true se todos os campos obrigatórios estão preenchidos
-            */
-        function validarCamposObrigatorios() {
-            var nome = document.getElementById('txtNome').value.trim();
-            var nomeMenu = document.getElementById('txtNomeMenu').value.trim();
-            var pagina = document.getElementById('ddlPagina')?.ej2_instances?.[0]?.value;
-            var icone = document.getElementById('ddlIcone')?.ej2_instances?.[0]?.value;
-
-            return nome && nomeMenu && pagina && pagina.length > 0 && icone && icone.length > 0;
-        }
-
-            /**
-             * Atualiza estado dos botões de ordenação
-             * @@description Mantida para compatibilidade - botões agora estão inline
-            * @@returns { void}
-            */
-        function atualizarEstadoBotoesOrdenacao() {
-
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * VERIFICAÇÕES DE MOVIMENTAÇÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Verifica se item selecionado pode mover para cima
-             * @@returns { boolean } true se pode mover
-            */
-        function podeMoverParaCima() {
-            if (!selectedItem || !selectedItem.id || !treeData) return false;
-
-            var item = buscarItemPorId(treeData, selectedItem.id);
-            if (!item) return false;
-
-            var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
-            if (!resultado) return false;
-
-            return resultado.indice > 0;
-        }
-
-            /**
-             * Verifica se item selecionado pode mover para baixo
-             * @@returns { boolean } true se pode mover
-            */
-        function podeMoverParaBaixo() {
-            if (!selectedItem || !selectedItem.id || !treeData) return false;
-
-            var item = buscarItemPorId(treeData, selectedItem.id);
-            if (!item) return false;
-
-            var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
-            if (!resultado) return false;
-
-            return resultado.indice < resultado.lista.length - 1;
-        }
-
-            /**
-             * Busca a lista pai que contém um item
-             * @@param { Array } lista - Lista para buscar
-            * @@param { string } targetId - ID do item alvo
-                * @@param { string } targetParentId - ID do pai do item alvo
-                    * @@returns { Object| null} Objeto com lista e índice, ou null
-                        */
-        function encontrarListaPaiEIndice(lista, targetId, targetParentId) {
-            for (var i = 0; i < lista.length; i++) {
-                var currentItem = lista[i];
-                var currentParentId = currentItem.parentId || null;
-
-                if (String(currentItem.id) === String(targetId)) {
-                    return { lista: lista, indice: i };
-                }
-
-                if (currentItem.items && currentItem.items.length > 0) {
-                    var resultado = encontrarListaPaiEIndice(currentItem.items, targetId, targetParentId);
-                    if (resultado) return resultado;
-                }
-            }
-            return null;
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES DE MOVIMENTAÇÃO NA ÁRVORE
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Funções chamadas pelos botões inline no TreeView
-            * Permitem reordenar itens e mudar hierarquia sem drag - and - drop
-                */
-
-            /**
-             * Move item para CIMA (ordenação vertical)
-             * @@param { string } itemId - ID do item a mover
-            * @@param { Event } event - Evento de clique
-                * @@returns { void}
-                */
-        function moverItemCima(itemId, event) {
-            console.log('[MOVER-CIMA] === FUNÇÃO CHAMADA ===');
-            console.log('[MOVER-CIMA] itemId recebido:', itemId, 'tipo:', typeof itemId);
-
-            if (event) {
-                event.stopPropagation();
-                event.preventDefault();
-            }
-
-            try {
-                console.log('[MOVER-CIMA] treeData:', treeData);
-                var item = buscarItemPorId(treeData, itemId);
-                console.log('[MOVER-CIMA] Item encontrado:', item);
-                if (!item) {
-                    console.error('[MOVER-CIMA] Item não encontrado:', itemId);
-                    return;
-                }
-
-                var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
-                if (!resultado || resultado.indice <= 0) {
-                    console.log('[MOVER-CIMA] Item já está na primeira posição');
-                    return;
-                }
-
-                var listaIrmaos = resultado.lista;
-                var indexAtual = resultado.indice;
-
-                var temp = listaIrmaos[indexAtual];
-                listaIrmaos[indexAtual] = listaIrmaos[indexAtual - 1];
-                listaIrmaos[indexAtual - 1] = temp;
-
-                console.log('[MOVER-CIMA] Item movido para cima:', item.text);
-
-                atualizarTreeViewAposMovimento(itemId);
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemCima", erro);
-            }
-        }
-
-            /**
-             * Move item para BAIXO (ordenação vertical)
-             * @@param { string } itemId - ID do item a mover
-            * @@param { Event } event - Evento de clique
-                * @@returns { void}
-                */
-        function moverItemBaixo(itemId, event) {
-            console.log('[MOVER-BAIXO] === FUNÇÃO CHAMADA ===');
-            console.log('[MOVER-BAIXO] itemId recebido:', itemId, 'tipo:', typeof itemId);
-
-            if (event) {
-                event.stopPropagation();
-                event.preventDefault();
-            }
-
-            try {
-                var item = buscarItemPorId(treeData, itemId);
-                console.log('[MOVER-BAIXO] Item encontrado:', item);
-                if (!item) {
-                    console.error('[MOVER-BAIXO] Item não encontrado:', itemId);
-                    return;
-                }
-
-                var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
-                if (!resultado || resultado.indice >= resultado.lista.length - 1) {
-                    console.log('[MOVER-BAIXO] Item já está na última posição');
-                    return;
-                }
-
-                var listaIrmaos = resultado.lista;
-                var indexAtual = resultado.indice;
-
-                var temp = listaIrmaos[indexAtual];
-                listaIrmaos[indexAtual] = listaIrmaos[indexAtual + 1];
-                listaIrmaos[indexAtual + 1] = temp;
-
-                console.log('[MOVER-BAIXO] Item movido para baixo:', item.text);
-
-                atualizarTreeViewAposMovimento(itemId);
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemBaixo", erro);
-            }
-        }
-
-            /**
-             * Move item para ESQUERDA (sobe nível - vira irmão do pai)
-             * @@param { string } itemId - ID do item a mover
-            * @@param { Event } event - Evento de clique
-                * @@returns { void}
-                */
-        function moverItemEsquerda(itemId, event) {
-            console.log('[MOVER-ESQ] === FUNÇÃO CHAMADA ===');
-            console.log('[MOVER-ESQ] itemId recebido:', itemId, 'tipo:', typeof itemId);
-
-            if (event) {
-                event.stopPropagation();
-                event.preventDefault();
-            }
-
-            try {
-                var item = buscarItemPorId(treeData, itemId);
-                console.log('[MOVER-ESQ] Item encontrado:', item);
-                if (!item) {
-                    console.error('[MOVER-ESQ] Item não encontrado:', itemId);
-                    return;
-                }
-
-                if (!item.parentId) {
-                    console.log('[MOVER-ESQ] Item já está na raiz');
-                    return;
-                }
-
-                var paiAtual = buscarItemPorId(treeData, item.parentId);
-                if (!paiAtual) {
-                    console.error('[MOVER-ESQ] Pai não encontrado:', item.parentId);
-                    return;
-                }
-
-                console.log('[MOVER-ESQ] Movendo item para nível do pai:', paiAtual.text);
-
-                if (paiAtual.items) {
-                    var indexNoFilho = paiAtual.items.findIndex(function (i) { return i.id === item.id; });
-                    if (indexNoFilho !== -1) {
-                        paiAtual.items.splice(indexNoFilho, 1);
-                    }
-                }
-
-                var novoParentId = paiAtual.parentId || null;
-                item.parentId = novoParentId;
-
-                if (!novoParentId) {
-
-                    var indexPai = treeData.findIndex(function (i) { return i.id === paiAtual.id; });
-                    if (indexPai !== -1) {
-                        treeData.splice(indexPai + 1, 0, item);
-                    } else {
-                        treeData.push(item);
-                    }
-                } else {
-
-                    var avo = buscarItemPorId(treeData, novoParentId);
-                    if (avo) {
-                        if (!avo.items) avo.items = [];
-
-                        var indexPaiNoAvo = avo.items.findIndex(function (i) { return i.id === paiAtual.id; });
-                        if (indexPaiNoAvo !== -1) {
-                            avo.items.splice(indexPaiNoAvo + 1, 0, item);
-                        } else {
-                            avo.items.push(item);
-                        }
-                    }
-                }
-
-                if (paiAtual.items && paiAtual.items.length === 0) {
-                    console.log('[MOVER-ESQ] Pai ficou sem filhos, transformando em página');
-                    paiAtual.items = [];
-
-                }
-
-                console.log('[MOVER-ESQ] Item movido para esquerda (subiu nível)');
-
-                atualizarTreeViewAposMovimento(itemId);
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemEsquerda", erro);
-            }
-        }
-
-            /**
-             * Move item para DIREITA (desce nível - vira filho do item acima)
-             * @@description Se o item acima for Página, avisa que será transformado em Grupo
-            * @@param { string } itemId - ID do item a mover
-                * @@param { Event } event - Evento de clique
-                    * @@returns { void}
-                    */
-        function moverItemDireita(itemId, event) {
-            console.log('[MOVER-DIR] === FUNÇÃO CHAMADA ===');
-            console.log('[MOVER-DIR] itemId recebido:', itemId, 'tipo:', typeof itemId);
-
-            if (event) {
-                event.stopPropagation();
-                event.preventDefault();
-            }
-
-            try {
-                var item = buscarItemPorId(treeData, itemId);
-                console.log('[MOVER-DIR] Item encontrado:', item);
-                if (!item) {
-                    console.error('[MOVER-DIR] Item não encontrado:', itemId);
-                    return;
-                }
-
-                var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
-                if (!resultado || resultado.indice <= 0) {
-                    console.log('[MOVER-DIR] Não há item acima para virar pai');
-                    return;
-                }
-
-                var listaIrmaos = resultado.lista;
-                var indexAtual = resultado.indice;
-                var itemAcima = listaIrmaos[indexAtual - 1];
-
-                console.log('[MOVER-DIR] Item acima que virará pai:', itemAcima.text);
-
-                var itemAcimaEhPagina = itemAcima.href && itemAcima.href !== 'javascript:void(0);' && itemAcima.href.trim() !== '';
-
-                if (itemAcimaEhPagina) {
-
-                    console.log('[MOVER-DIR] Item acima é uma Página - exibindo aviso de transformação');
-
-                    Alerta.Confirmar(
-                        'Transformação em Grupo',
-                        '<div style="text-align: left; padding: 10px;">' +
-                        '<p style="margin-bottom: 15px;">Ao subordinar <strong>"' + item.text + '"</strong> como filho de <strong>"' + itemAcima.text + '"</strong>, ' +
-                        'a página <strong>"' + itemAcima.text + '"</strong> será <span style="color: #d9534f; font-weight: bold;">transformada em GRUPO</span>.</p>' +
-                        '<div style="background: linear-gradient(135deg, #ff6b35, #e55a2b); color: white; padding: 12px 15px; border-radius: 8px; border: 2px solid rgba(255,255,255,0.5); margin: 10px 0;">' +
-                        '<i class="fa-duotone fa-triangle-exclamation" style="--fa-primary-color:#ffffff; --fa-secondary-color:#ffcc00; margin-right: 8px;"></i>' +
-                        '<strong>ATENÇÃO:</strong> O link HTML da página será removido permanentemente e ela não acessará mais nenhuma funcionalidade diretamente!' +
-                        '</div>' +
-                        '<p style="margin-top: 15px;">Deseja prosseguir com esta operação?</p>' +
-                        '</div>',
-                        '<i class="fa-duotone fa-check" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right: 6px;"></i>Sim, Transformar em Grupo',
-                        '<i class="fa-duotone fa-xmark" style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d; margin-right: 6px;"></i>Cancelar'
-                    ).then(function (confirmado) {
-                        if (confirmado) {
-                            console.log('[MOVER-DIR] Usuário confirmou transformação - executando movimentação');
-                            executarMovimentacaoDireita(item, listaIrmaos, indexAtual, itemAcima, itemId);
-                        } else {
-                            console.log('[MOVER-DIR] Usuário cancelou - operação revertida');
-                            Alerta.Info('Operação Cancelada', 'A movimentação foi cancelada. Nenhuma alteração foi feita.', 'OK');
-                        }
-                    });
-                } else {
-
-                    console.log('[MOVER-DIR] Item acima já é Grupo - executando movimentação direta');
-                    executarMovimentacaoDireita(item, listaIrmaos, indexAtual, itemAcima, itemId);
-                }
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemDireita", erro);
-            }
-        }
-
-            /**
-             * Executa movimentação para direita após confirmação
-             * @@description Função auxiliar para moverItemDireita
-            * @@param { Object } item - O item sendo movido
-                * @@param { Array } listaIrmaos - Lista de irmãos onde o item está
-                    * @@param { number } indexAtual - Índice atual do item na lista
-                        * @@param { Object } itemAcima - O item que será o novo pai
-                            * @@param { string } itemId - ID do item para manter selecionado
-                                * @@returns { void}
-                                */
-        function executarMovimentacaoDireita(item, listaIrmaos, indexAtual, itemAcima, itemId) {
-            try {
-
-                listaIrmaos.splice(indexAtual, 1);
-
-                item.parentId = itemAcima.id;
-
-                if (!itemAcima.items) {
-                    itemAcima.items = [];
-                }
-                itemAcima.items.push(item);
-
-                if (!itemAcima.href || itemAcima.href === 'javascript:void(0);') {
-
-                } else {
-
-                    console.log('[MOVER-DIR] Item acima transformado em grupo');
-                    itemAcima.href = 'javascript:void(0);';
-                }
-
-                console.log('[MOVER-DIR] Item movido para direita (desceu nível, virou filho de ' + itemAcima.text + ')');
-
-                atualizarTreeViewAposMovimento(itemId);
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "executarMovimentacaoDireita", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * ATUALIZAÇÃO DO TREEVIEW
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Atualiza TreeView após movimentos de itens
-             * @@description Reconstrói a árvore e reconfigura event listeners
-            * @@param { string| null} itemIdParaManter - ID do item a manter selecionado
-                * @@param { boolean }[marcarPendentes = true] - Se deve marcar alterações pendentes
-                    * @@returns { void}
-                    */
-        function atualizarTreeViewAposMovimento(itemIdParaManter, marcarPendentes) {
-            try {
-
-                if (marcarPendentes === undefined) marcarPendentes = true;
-
-                console.log('[ATUALIZAR] Atualizando TreeView após movimento... marcarPendentes=' + marcarPendentes);
-
-                treeData = JSON.parse(JSON.stringify(treeData));
-
-                if (treeObj) {
-                    treeObj.fields.dataSource = treeData;
-                    treeObj.dataBind();
-                    treeObj.refresh();
-                    treeObj.expandAll();
-                }
-
-                configurarBotoesNavegacao();
-
-                popularDropdownItemPai();
-                popularDropdownPosicao();
-
-                if (itemIdParaManter) {
-                    var itemAtualizado = buscarItemPorId(treeData, itemIdParaManter);
-                    if (itemAtualizado) {
-                        selectedItem = itemAtualizado;
-
-                    }
-                } else {
-
-                    selectedItem = null;
-                    if (treeObj) {
-                        treeObj.selectedNodes = [];
-                    }
-                }
-
-                if (marcarPendentes) {
-                    marcarAlteracoesPendentes();
-                    console.log('[ATUALIZAR] TreeView atualizado (alterações pendentes)');
-                } else {
-                    console.log('[ATUALIZAR] TreeView atualizado (sem marcar pendentes - já salvo no banco)');
-                }
-
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "atualizarTreeViewAposMovimento", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES DE COMPATIBILIDADE (deprecated)
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Funções antigas mantidas para compatibilidade
-            * Prefira usar as funções inline: moverItemCima, moverItemBaixo, etc.
-             */
-
-            /**
-             * Move item selecionado para cima (deprecated - use moverItemCima)
-             * @@returns { void}
-             */
-        function moverItemParaCima() {
-            try {
-                if (!selectedItem || !selectedItem.id) {
-                    mostrarAlerta('Selecione um item primeiro', 'warning');
-                    return;
-                }
-
-                var item = buscarItemPorId(treeData, selectedItem.id);
-                if (!item) {
-                    mostrarAlerta('Item não encontrado na árvore', 'danger');
-                    return;
-                }
-
-                var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
-                if (!resultado || resultado.indice <= 0) {
-                    mostrarAlerta('Item já está na primeira posição', 'info');
-                    return;
-                }
-
-                var listaIrmaos = resultado.lista;
-                var indexAtual = resultado.indice;
-
-                var temp = listaIrmaos[indexAtual];
-                listaIrmaos[indexAtual] = listaIrmaos[indexAtual - 1];
-                listaIrmaos[indexAtual - 1] = temp;
-
-                console.log('[MOVER-CIMA] ✅ Posições trocadas na lista original');
-                console.log('[MOVER-CIMA] Lista após troca:', listaIrmaos.map(function (i) { return i.text; }));
-
-                var treeDataNovo = JSON.parse(JSON.stringify(treeData));
-                treeData = treeDataNovo;
-
-                treeObj.fields.dataSource = treeData;
-                treeObj.dataBind();
-
-                console.log('[MOVER-CIMA] ✅ TreeView atualizado');
-
-                setTimeout(function () {
-                    var itemAtualizado = buscarItemPorId(treeData, item.id);
-                    if (itemAtualizado) {
-                        selecionarItem(itemAtualizado);
-                    }
-                    salvarOrdenacaoAutomatica();
-                }, 300);
-
-            } catch (erro) {
-                console.error('[moverItemParaCima] Erro:', erro);
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemParaCima", erro);
-            }
-        }
-
-            /**
-             * Move item selecionado para baixo (deprecated - use moverItemBaixo)
-             * @@returns { void}
-             */
-        function moverItemParaBaixo() {
-            try {
-                if (!selectedItem || !selectedItem.id) {
-                    mostrarAlerta('Selecione um item primeiro', 'warning');
-                    return;
-                }
-
-                var item = buscarItemPorId(treeData, selectedItem.id);
-                if (!item) {
-                    mostrarAlerta('Item não encontrado na árvore', 'danger');
-                    return;
-                }
-
-                var resultado = encontrarListaPaiEIndice(treeData, item.id, item.parentId);
-                if (!resultado || resultado.indice >= resultado.lista.length - 1) {
-                    mostrarAlerta('Item já está na última posição', 'info');
-                    return;
-                }
-
-                var listaIrmaos = resultado.lista;
-                var indexAtual = resultado.indice;
-
-                console.log('[MOVER-BAIXO] Lista encontrada:', listaIrmaos.length, 'itens');
-                console.log('[MOVER-BAIXO] Índice atual:', indexAtual);
-                console.log('[MOVER-BAIXO] Item atual:', item.text);
-                console.log('[MOVER-BAIXO] Item seguinte:', listaIrmaos[indexAtual + 1].text);
-
-                var temp = listaIrmaos[indexAtual];
-                listaIrmaos[indexAtual] = listaIrmaos[indexAtual + 1];
-                listaIrmaos[indexAtual + 1] = temp;
-
-                console.log('[MOVER-BAIXO] ✅ Posições trocadas na lista');
-
-                var treeDataNovo = JSON.parse(JSON.stringify(treeData));
-                treeData = treeDataNovo;
-
-                treeObj.fields.dataSource = treeData;
-                treeObj.dataBind();
-
-                console.log('[MOVER-BAIXO] ✅ TreeView atualizado');
-
-                setTimeout(function () {
-                    var itemAtualizado = buscarItemPorId(treeData, item.id);
-                    if (itemAtualizado) {
-                        selecionarItem(itemAtualizado);
-                    }
-                    salvarOrdenacaoAutomatica();
-                }, 300);
-
-            } catch (erro) {
-                console.error('[moverItemParaBaixo] Erro:', erro);
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "moverItemParaBaixo", erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * LIMPEZA DE FORMULÁRIO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Limpa todos os campos do formulário de propriedades
-             * @@param { boolean } [esconderCard = true] - Se deve esconder o card de propriedades
-            * @@returns { void}
-            */
-        function limparFormulario(esconderCard = true) {
-            try {
-                selectedItem = null;
-
-                ultimoIconeSelecionado = null;
-                ultimaPaginaSelecionada = null;
-
-                document.getElementById('formPropriedades').reset();
-                document.getElementById('recursoId').value = '';
-                document.getElementById('parentId').value = '';
-                document.getElementById('txtIconClass').value = '';
-                document.getElementById('selectedItemName').textContent = 'Nenhum item selecionado';
-
-                var ddlIconeObj = document.getElementById('ddlIcone').ej2_instances[0];
-                if (ddlIconeObj) {
-                    ddlIconeObj.value = [];
-                    ddlIconeObj.text = '';
-                    ddlIconeObj.refresh();
-                }
-
-                var ddlPaginaObj = document.getElementById('ddlPagina').ej2_instances[0];
-                if (ddlPaginaObj) {
-                    ddlPaginaObj.value = null;
-                }
-
-                document.getElementById('parentId').value = '';
-
-                document.getElementById('listaAcessos').innerHTML = '<p class="text-muted text-center">Selecione um item para gerenciar acessos</p>';
-                document.querySelectorAll('.tree-node').forEach(n => n.classList.remove('selected'));
-
-                document.getElementById('modoEdicao').textContent = 'Selecione';
-                document.getElementById('modoEdicao').className = 'badge bg-secondary ms-2';
-
-                atualizarEstadoBotoesOrdenacao();
-
-                if (esconderCard) {
-                    document.getElementById('propsCard').style.display = 'none';
-                }
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "limparFormulario", erro);
-            }
-        }
-
-            /*
-
-            function salvarOrdenacao() {
-                try {
-                    if (!treeObj) return;
-                    var dataSource = treeObj.getTreeData();
-                    var items = extrairItensDoTreeView(dataSource, null, 0);
-                    fetch('/api/Navigation/SaveTreeToDb', {
-                        method: 'POST',
-                        headers: { 'Content-Type': 'application/json' },
-                        body: JSON.stringify(items)
-                    })
-                    .then(r => r.json())
-                    .then(result => {
-                        if (result.success) {
-                            mostrarAlerta('Ordenação salva com sucesso!', 'success');
-                            carregarArvore();
-                            atualizarNavegacaoLateral();
-                        } else {
-                            mostrarAlerta('Erro: ' + result.message, 'danger');
-                        }
-                    })
-                    .catch(erro => {
-                        Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarOrdenacao", erro);
-                    });
-                } catch (erro) {
-                    Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarOrdenacao", erro);
-                }
-            }
-            */
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * SALVAMENTO AUTOMÁTICO (AUTO-SAVE)
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Salva ordenação automaticamente após drag-drop ou transformação
-             * @@description Envia estrutura completa para API e atualiza navegação
-            * @@returns { void}
-            */
-        function salvarOrdenacaoAutomatica() {
+        } catch (erro) {
+            console.error('[AUTO-SAVE] ❌ Erro no try-catch:', erro);
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarOrdenacaoAutomatica", erro);
+        }
+    }
+
+    function salvarOrdenacaoAutomaticaSemReload() {
+        return new Promise(function(resolve, reject) {
             try {
                 if (!treeObj && !treeData) {
-                    console.error('[AUTO-SAVE] Nem treeObj nem treeData existem!');
+                    console.error('[AUTO-SAVE-SR] Nem treeObj nem treeData existem!');
+                    reject(new Error('Dados da árvore não disponíveis'));
                     return;
                 }
 
                 var dataSource = treeData || treeObj.getTreeData();
-                console.log('[AUTO-SAVE] DataSource obtido (treeData):', dataSource);
+                console.log('[AUTO-SAVE-SR] DataSource obtido (treeData):', dataSource);
 
                 var items = extrairItensDoTreeView(dataSource, null, 0);
-                console.log('[AUTO-SAVE] Items extraídos para salvar:', items);
-                console.log('[AUTO-SAVE] Total de items:', items.length);
+                console.log('[AUTO-SAVE-SR] Items extraídos para salvar:', items.length);
 
                 if (items.length === 0) {
-                    console.warn('[AUTO-SAVE] Nenhum item para salvar!');
+                    console.warn('[AUTO-SAVE-SR] Nenhum item para salvar!');
+                    reject(new Error('Nenhum item para salvar'));
                     return;
                 }
 
-                console.log('[AUTO-SAVE] Enviando para backend...');
-
-                var jsonPayload = JSON.stringify(items);
-                console.log('[AUTO-SAVE] JSON Payload (primeiros 500 chars):', jsonPayload.substring(0, 500));
+                console.log('[AUTO-SAVE-SR] Enviando para backend...');
 
                 fetch('/api/Navigation/SaveTreeToDb', {
                     method: 'POST',
                     headers: { 'Content-Type': 'application/json' },
-                    body: jsonPayload
+                    body: JSON.stringify(items)
                 })
-                    .then(r => {
-                        console.log('[AUTO-SAVE] Response status:', r.status);
-                        return r.json().then(data => ({ ok: r.ok, status: r.status, data: data }));
-                    })
-                    .then(response => {
-                        console.log('[AUTO-SAVE] Response do backend:', response.data);
-
-                        if (!response.ok) {
-                            console.error('[AUTO-SAVE] HTTP Error:', response.status);
-
-                            if (response.data.errors) {
-                                console.error('[AUTO-SAVE] Erros de Validação:', JSON.stringify(response.data.errors, null, 2));
-                                var erroMsg = 'Erro de validação: ';
-                                for (var campo in response.data.errors) {
-                                    erroMsg += campo + ': ' + response.data.errors[campo].join(', ') + '; ';
-                                }
-                                mostrarAlerta(erroMsg, 'danger');
-                            } else {
-                                mostrarAlerta('Erro HTTP ' + response.status + ': ' + (response.data.message || response.data.title || 'Erro desconhecido'), 'danger');
-                            }
-                            return;
-                        }
-
-                        if (response.data.success) {
-                            console.log('[AUTO-SAVE] Ordenação salva automaticamente!');
-
-                            var msg = 'Ordenação salva automaticamente';
-                            if (lastDragInfo && lastDragInfo.itemName) {
-                                msg = "Item '" + lastDragInfo.itemName + "' movido de '" +
-                                    (lastDragInfo.fromParentName || 'Sem grupo') + "' para '" +
-                                    (lastDragInfo.toParentName || 'Sem grupo') + "'";
-                            }
-                            mostrarToastSucesso(msg);
-
-                            carregarArvore();
-
-                            atualizarNavegacaoLateral();
-                        } else {
-                            console.error('[AUTO-SAVE] Erro no backend:', response.data.message);
-                            mostrarAlerta('Erro ao salvar ordenação: ' + response.data.message, 'danger');
-                        }
-                    })
-                    .catch(erro => {
-                        console.error('[AUTO-SAVE] Erro no fetch:', erro);
-                        Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarOrdenacaoAutomatica", erro);
+                .then(function(r) {
+                    console.log('[AUTO-SAVE-SR] Response status:', r.status);
+                    return r.json().then(function(data) {
+                        return { ok: r.ok, status: r.status, data: data };
                     });
-            } catch (erro) {
-                console.error('[AUTO-SAVE] Erro no try-catch:', erro);
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "salvarOrdenacaoAutomatica", erro);
-            }
-        }
-
-            /**
-             * Versão do auto-save que retorna Promise e não faz reload
-             * @@description Usada para transformações onde reload é controlado externamente
-            * @@returns { Promise } Promise que resolve com dados do backend
-                */
-            function salvarOrdenacaoAutomaticaSemReload() {
-                return new Promise(function (resolve, reject) {
-                    try {
-                        if (!treeObj && !treeData) {
-                            console.error('[AUTO-SAVE-SR] Nem treeObj nem treeData existem!');
-                            reject(new Error('Dados da árvore não disponíveis'));
-                            return;
-                        }
-
-                        var dataSource = treeData || treeObj.getTreeData();
-                        console.log('[AUTO-SAVE-SR] DataSource obtido (treeData):', dataSource);
-
-                        var items = extrairItensDoTreeView(dataSource, null, 0);
-                        console.log('[AUTO-SAVE-SR] Items extraídos para salvar:', items.length);
-
-                        if (items.length === 0) {
-                            console.warn('[AUTO-SAVE-SR] Nenhum item para salvar!');
-                            reject(new Error('Nenhum item para salvar'));
-                            return;
-                        }
-
-                        console.log('[AUTO-SAVE-SR] Enviando para backend...');
-
-                        fetch('/api/Navigation/SaveTreeToDb', {
-                            method: 'POST',
-                            headers: { 'Content-Type': 'application/json' },
-                            body: JSON.stringify(items)
-                        })
-                            .then(function (r) {
-                                console.log('[AUTO-SAVE-SR] Response status:', r.status);
-                                return r.json().then(function (data) {
-                                    return { ok: r.ok, status: r.status, data: data };
-                                });
-                            })
-                            .then(function (response) {
-                                console.log('[AUTO-SAVE-SR] Response do backend:', response.data);
-
-                                if (!response.ok) {
-                                    console.error('[AUTO-SAVE-SR] HTTP Error:', response.status);
-                                    reject(new Error(response.data.message || 'Erro HTTP ' + response.status));
-                                    return;
-                                }
-
-                                if (response.data.success) {
-                                    console.log('[AUTO-SAVE-SR] Ordenação salva com sucesso (sem reload)!');
-                                    resolve(response.data);
-                                } else {
-                                    console.error('[AUTO-SAVE-SR] Erro no backend:', response.data.message);
-                                    reject(new Error(response.data.message));
-                                }
-                            })
-                            .catch(function (erro) {
-                                console.error('[AUTO-SAVE-SR] Erro no fetch:', erro);
-                                reject(erro);
-                            });
-                    } catch (erro) {
-                        console.error('[AUTO-SAVE-SR] Erro no try-catch:', erro);
-                        reject(erro);
-                    }
-                });
-            }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * ATUALIZAÇÃO DA NAVEGAÇÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Atualiza a navegação lateral (menu do sistema)
-             * @@description Recarrega a página para refletir mudanças no menu
-            * @@returns { void}
-            */
-        function atualizarNavegacaoLateral() {
-            try {
-                console.log('[NAV-UPDATE] Atualizando navegação lateral...');
-
-                setTimeout(function () {
-                    console.log('[NAV-UPDATE] Recarregando página para atualizar navegação...');
-                    window.location.reload();
-                }, 1000);
-            } catch (erro) {
-                console.error('[NAV-UPDATE] Erro:', erro);
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES AUXILIARES
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Mostra toast discreto de sucesso para auto-save
-             * @@param { string } mensagem - Mensagem a exibir
-            * @@returns { void}
-            */
-        function mostrarToastSucesso(mensagem) {
-            try {
-                if (window.AppToast && typeof window.AppToast.show === 'function') {
-
-                    var msgDecodificada = decodeHtml(mensagem);
-                    window.AppToast.show('Verde', msgDecodificada, 4000);
-                } else if (typeof showSyncfusionToast === 'function') {
-                    showSyncfusionToast(decodeHtml(mensagem), 'success', '✅');
-                } else if (typeof Alerta !== 'undefined' && Alerta.Toast) {
-                    Alerta.Toast('success', decodeHtml(mensagem));
-                } else {
-                    console.log('[TOAST]', mensagem);
-                }
-            } catch (erro) {
-                console.log('[TOAST]', mensagem);
-            }
-        }
-
-            /**
-             * Extrai itens do TreeView para formato de salvamento
-             * @@param { Array } items - Lista de itens da árvore
-            * @@param { string| null} parentId - ID do pai(null para raiz)
-                * @@param { number } nivel - Nível hierárquico atual
-                    * @@returns { Array } Lista de itens formatados para API
-                        */
-        function extrairItensDoTreeView(items, parentId, nivel) {
-            try {
-                if (!items) return [];
-
-                var result = [];
-
-                items.forEach(function (item, index) {
-                    var newItem = {
-                        id: item.id,
-                        text: item.text,
-                        nomeMenu: item.nomeMenu,
-                        icon: item.icon,
-                        href: item.href,
-                        parentId: parentId,
-                        ordem: (nivel * 100) + index + 1,
-                        nivel: nivel,
-                        ativo: item.ativo !== false,
-                        descricao: item.descricao,
-                        items: []
-                    };
-
-                    if (item.items && item.items.length > 0) {
-                        newItem.items = extrairItensDoTreeView(item.items, item.id, nivel + 1);
-                    }
-
-                    result.push(newItem);
-                });
-
-                return result;
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "extrairItensDoTreeView", erro);
-                return [];
-            }
-        }
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CONTAGEM DE ITENS
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Atualiza contador de itens na interface
-             * @@returns { void}
-             */
-        function atualizarContagem() {
-            try {
-                var total = contarItens(treeData);
-                document.getElementById('itemCount').textContent = total + ' itens';
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "atualizarContagem", erro);
-            }
-        }
-
-            /**
-             * Conta itens recursivamente na árvore
-             * @@param { Array } items - Lista de itens
-            * @@returns { number } Total de itens
-                */
-        function contarItens(items) {
-            try {
-                if (!items) return 0;
-                var count = items.length;
-                items.forEach(function (item) {
-                    if (item.items && item.items.length > 0) {
-                        count += contarItens(item.items);
-                    }
-                });
-                return count;
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "contarItens", erro);
-                return 0;
-            }
-        }
-
-        function migrarDadosJson() {
-            try {
-                Alerta.Confirmar(
-                    'Confirmar Migração',
-                    'Isso irá migrar os dados do nav.json para o banco de dados. Continuar?',
-                    'Migrar',
-                    'Cancelar'
-                ).then(function (result) {
-                    if (!result) return;
-
-                    mostrarAlerta('Migrando dados...', 'info');
-
-                    fetch('/api/Navigation/MigrateFromJson', {
-                        method: 'POST'
-                    })
-                        .then(r => r.json())
-                        .then(result => {
-                            if (result.success) {
-                                mostrarAlerta(result.message, 'success');
-                                carregarArvore();
-                            } else {
-                                mostrarAlerta('Erro: ' + result.message, 'danger');
-                            }
-                        })
-                        .catch(erro => {
-                            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "migrarDadosJson", erro);
-                        });
+                })
+                .then(function(response) {
+                    console.log('[AUTO-SAVE-SR] Response do backend:', response.data);
+
+                    if (!response.ok) {
+                        console.error('[AUTO-SAVE-SR] ❌ HTTP Error:', response.status);
+                        reject(new Error(response.data.message || 'Erro HTTP ' + response.status));
+                        return;
+                    }
+
+                    if (response.data.success) {
+                        console.log('[AUTO-SAVE-SR] ✅ Ordenação salva com sucesso (sem reload)!');
+                        resolve(response.data);
+                    } else {
+                        console.error('[AUTO-SAVE-SR] ❌ Erro no backend:', response.data.message);
+                        reject(new Error(response.data.message));
+                    }
+                })
+                .catch(function(erro) {
+                    console.error('[AUTO-SAVE-SR] ❌ Erro no fetch:', erro);
+                    reject(erro);
                 });
             } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "migrarDadosJson", erro);
-            }
-        }
-
-        function selecionarTodosAcessos() {
-            try {
-                document.querySelectorAll('#listaAcessos input[type="checkbox"]').forEach(function (cb) {
-                    if (!cb.checked) {
-                        cb.checked = true;
-                        cb.dispatchEvent(new Event('change'));
-                    }
+                console.error('[AUTO-SAVE-SR] ❌ Erro no try-catch:', erro);
+                reject(erro);
+            }
+        });
+    }
+
+    function atualizarNavegacaoLateral() {
+        try {
+            console.log('[NAV-UPDATE] Atualizando navegação lateral...');
+
+            setTimeout(function() {
+                console.log('[NAV-UPDATE] Recarregando página para atualizar navegação...');
+                window.location.reload();
+            }, 1000);
+        } catch (erro) {
+            console.error('[NAV-UPDATE] Erro:', erro);
+        }
+    }
+
+    function mostrarToastSucesso(mensagem) {
+        try {
+            if (window.AppToast && typeof window.AppToast.show === 'function') {
+
+                var msgDecodificada = decodeHtml(mensagem);
+                window.AppToast.show('Verde', msgDecodificada, 4000);
+            } else if (typeof showSyncfusionToast === 'function') {
+                showSyncfusionToast(decodeHtml(mensagem), 'success', '✅');
+            } else if (typeof Alerta !== 'undefined' && Alerta.Toast) {
+                Alerta.Toast('success', decodeHtml(mensagem));
+            } else {
+                console.log('[TOAST]', mensagem);
+            }
+        } catch (erro) {
+            console.log('[TOAST]', mensagem);
+        }
+    }
+
+    function extrairItensDoTreeView(items, parentId, nivel) {
+        try {
+            if (!items) return [];
+
+            var result = [];
+
+            items.forEach(function(item, index) {
+                var newItem = {
+                    id: item.id,
+                    text: item.text,
+                    nomeMenu: item.nomeMenu,
+                    icon: item.icon,
+                    href: item.href,
+                    parentId: parentId,
+                    ordem: (nivel * 100) + index + 1,
+                    nivel: nivel,
+                    ativo: item.ativo !== false,
+                    descricao: item.descricao,
+                    items: []
+                };
+
+                if (item.items && item.items.length > 0) {
+                    newItem.items = extrairItensDoTreeView(item.items, item.id, nivel + 1);
+                }
+
+                result.push(newItem);
+            });
+
+            return result;
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "extrairItensDoTreeView", erro);
+            return [];
+        }
+    }
+
+    function atualizarContagem() {
+        try {
+            var total = contarItens(treeData);
+            document.getElementById('itemCount').textContent = total + ' itens';
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "atualizarContagem", erro);
+        }
+    }
+
+    function contarItens(items) {
+        try {
+            if (!items) return 0;
+            var count = items.length;
+            items.forEach(function(item) {
+                if (item.items && item.items.length > 0) {
+                    count += contarItens(item.items);
+                }
+            });
+            return count;
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "contarItens", erro);
+            return 0;
+        }
+    }
+
+    function migrarDadosJson() {
+        try {
+            Alerta.Confirmar(
+                'Confirmar Migração',
+                'Isso irá migrar os dados do nav.json para o banco de dados. Continuar?',
+                'Migrar',
+                'Cancelar'
+            ).then(function(result) {
+                if (!result) return;
+
+                mostrarAlerta('Migrando dados...', 'info');
+
+                fetch('/api/Navigation/MigrateFromJson', {
+                    method: 'POST'
+                })
+                .then(r => r.json())
+                .then(result => {
+                    if (result.success) {
+                        mostrarAlerta(result.message, 'success');
+                        carregarArvore();
+                    } else {
+                        mostrarAlerta('Erro: ' + result.message, 'danger');
+                    }
+                })
+                .catch(erro => {
+                    Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "migrarDadosJson", erro);
                 });
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "selecionarTodosAcessos", erro);
-            }
-        }
-
-        function verificarMudancasNaoSalvas() {
-            try {
-                if (!selectedItem) return false;
-
-                var txtNome = document.getElementById('txtNome').value;
-                var txtNomeMenu = document.getElementById('txtNomeMenu').value;
-                var txtHref = document.getElementById('txtHref').value;
-                var txtIconClass = document.getElementById('txtIconClass').value;
-                var txtOrdem = parseFloat(document.getElementById('txtOrdem').value) || 0;
-                var txtDescricao = document.getElementById('txtDescricao').value;
-                var chkAtivo = document.getElementById('chkAtivo').checked;
-                var parentId = document.getElementById('parentId').value;
-
-                var nomeOriginal = decodeHtml(selectedItem.text) || '';
-                var nomeMenuOriginal = decodeHtml(selectedItem.nomeMenu) || '';
-                var hrefOriginal = selectedItem.href || '';
-                var iconOriginal = selectedItem.icon || '';
-                var ordemOriginal = selectedItem.ordem || 0;
-                var descricaoOriginal = decodeHtml(selectedItem.descricao) || '';
-                var ativoOriginal = selectedItem.ativo !== false;
-                var parentIdOriginal = selectedItem.parentId || '';
-
-                return txtNome !== nomeOriginal ||
-                    txtNomeMenu !== nomeMenuOriginal ||
-                    txtHref !== hrefOriginal ||
-                    txtIconClass !== iconOriginal ||
-                    txtOrdem !== ordemOriginal ||
-                    txtDescricao !== descricaoOriginal ||
-                    chkAtivo !== ativoOriginal ||
-                    parentId !== parentIdOriginal;
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "verificarMudancasNaoSalvas", erro);
-                return false;
-            }
-        }
-
-        function fecharCards() {
-            try {
-
-                var emModoEdicao = selectedItem && selectedItem.id;
-
-                if (emModoEdicao && verificarMudancasNaoSalvas()) {
-                    Alerta.Confirmar(
-                        'Mudanças Não Salvas',
-                        'Você tem mudanças não salvas. Deseja salvar antes de fechar?',
-                        'Salvar',
-                        'Descartar'
-                    ).then(function (result) {
-                        if (result) {
-
-                            salvarPropriedades();
-
-                        } else {
-
-                            document.getElementById('propsCard').style.display = 'none';
-                            document.getElementById('acessoCard').style.display = 'none';
-                            habilitarCardPropriedades(true);
-                            limparFormulario(true);
-                        }
-                    });
-                    return;
-                }
-
-                document.getElementById('propsCard').style.display = 'none';
-                document.getElementById('acessoCard').style.display = 'none';
-                habilitarCardPropriedades(true);
-                limparFormulario(true);
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "fecharCards", erro);
-            }
-        }
-
-        function habilitarCardPropriedades(habilitar) {
-            try {
-                var campos = document.querySelectorAll('#formPropriedades input, #formPropriedades textarea, #formPropriedades select, #formPropriedades button');
-                campos.forEach(function (campo) {
-                    if (habilitar) {
-                        campo.removeAttribute('disabled');
-                        campo.style.opacity = '1';
-                        campo.style.cursor = 'auto';
+            });
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "migrarDadosJson", erro);
+        }
+    }
+
+    function selecionarTodosAcessos() {
+        try {
+            document.querySelectorAll('#listaAcessos input[type="checkbox"]').forEach(function(cb) {
+                if (!cb.checked) {
+                    cb.checked = true;
+                    cb.dispatchEvent(new Event('change'));
+                }
+            });
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "selecionarTodosAcessos", erro);
+        }
+    }
+
+    function verificarMudancasNaoSalvas() {
+        try {
+            if (!selectedItem) return false;
+
+            var txtNome = document.getElementById('txtNome').value;
+            var txtNomeMenu = document.getElementById('txtNomeMenu').value;
+            var txtHref = document.getElementById('txtHref').value;
+            var txtIconClass = document.getElementById('txtIconClass').value;
+            var txtOrdem = parseFloat(document.getElementById('txtOrdem').value) || 0;
+            var txtDescricao = document.getElementById('txtDescricao').value;
+            var chkAtivo = document.getElementById('chkAtivo').checked;
+            var parentId = document.getElementById('parentId').value;
+
+            var nomeOriginal = decodeHtml(selectedItem.text) || '';
+            var nomeMenuOriginal = decodeHtml(selectedItem.nomeMenu) || '';
+            var hrefOriginal = selectedItem.href || '';
+            var iconOriginal = selectedItem.icon || '';
+            var ordemOriginal = selectedItem.ordem || 0;
+            var descricaoOriginal = decodeHtml(selectedItem.descricao) || '';
+            var ativoOriginal = selectedItem.ativo !== false;
+            var parentIdOriginal = selectedItem.parentId || '';
+
+            return txtNome !== nomeOriginal ||
+                   txtNomeMenu !== nomeMenuOriginal ||
+                   txtHref !== hrefOriginal ||
+                   txtIconClass !== iconOriginal ||
+                   txtOrdem !== ordemOriginal ||
+                   txtDescricao !== descricaoOriginal ||
+                   chkAtivo !== ativoOriginal ||
+                   parentId !== parentIdOriginal;
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "verificarMudancasNaoSalvas", erro);
+            return false;
+        }
+    }
+
+    function fecharCards() {
+        try {
+
+            var emModoEdicao = selectedItem && selectedItem.id;
+
+            if (emModoEdicao && verificarMudancasNaoSalvas()) {
+                Alerta.Confirmar(
+                    'Mudanças Não Salvas',
+                    'Você tem mudanças não salvas. Deseja salvar antes de fechar?',
+                    'Salvar',
+                    'Descartar'
+                ).then(function(result) {
+                    if (result) {
+
+                        salvarPropriedades();
+
                     } else {
-                        campo.setAttribute('disabled', 'disabled');
-                        campo.style.opacity = '0.6';
-                        campo.style.cursor = 'not-allowed';
+
+                        document.getElementById('propsCard').style.display = 'none';
+                        document.getElementById('acessoCard').style.display = 'none';
+                        habilitarCardPropriedades(true);
+                        limparFormulario(true);
                     }
                 });
-
-                var btnSalvar = document.querySelector('#formPropriedades button[onclick*="salvarPropriedades"]');
-                if (btnSalvar && !habilitar) {
-                    btnSalvar.setAttribute('disabled', 'disabled');
-                }
-            } catch (erro) {
-                Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "habilitarCardPropriedades", erro);
-            }
-        }
-
-        function habilitarAcessoTodosUsuarios(recursoId) {
-            try {
-                fetch('/api/Navigation/HabilitarAcessoTodosUsuarios', {
-                    method: 'POST',
-                    headers: { 'Content-Type': 'application/json' },
-                    body: JSON.stringify({ recursoId: recursoId })
-                })
-                    .then(r => r.json())
-                    .then(result => {
-                        if (result.success) {
-                            console.log('Acessos habilitados para todos os usuários');
-                            carregarControleAcesso(recursoId);
-                        } else {
-                            console.error('Erro ao habilitar acessos:', result.message);
-                        }
-                    })
-                    .catch(erro => {
-                        Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "habilitarAcessoTodosUsuarios", erro);
-                    });
-            } catch (erro) {
+                return;
+            }
+
+            document.getElementById('propsCard').style.display = 'none';
+            document.getElementById('acessoCard').style.display = 'none';
+            habilitarCardPropriedades(true);
+            limparFormulario(true);
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "fecharCards", erro);
+        }
+    }
+
+    function habilitarCardPropriedades(habilitar) {
+        try {
+            var campos = document.querySelectorAll('#formPropriedades input, #formPropriedades textarea, #formPropriedades select, #formPropriedades button');
+            campos.forEach(function(campo) {
+                if (habilitar) {
+                    campo.removeAttribute('disabled');
+                    campo.style.opacity = '1';
+                    campo.style.cursor = 'auto';
+                } else {
+                    campo.setAttribute('disabled', 'disabled');
+                    campo.style.opacity = '0.6';
+                    campo.style.cursor = 'not-allowed';
+                }
+            });
+
+            var btnSalvar = document.querySelector('#formPropriedades button[onclick*="salvarPropriedades"]');
+            if (btnSalvar && !habilitar) {
+                btnSalvar.setAttribute('disabled', 'disabled');
+            }
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "habilitarCardPropriedades", erro);
+        }
+    }
+
+    function habilitarAcessoTodosUsuarios(recursoId) {
+        try {
+            fetch('/api/Navigation/HabilitarAcessoTodosUsuarios', {
+                method: 'POST',
+                headers: { 'Content-Type': 'application/json' },
+                body: JSON.stringify({ recursoId: recursoId })
+            })
+            .then(r => r.json())
+            .then(result => {
+                if (result.success) {
+                    console.log('Acessos habilitados para todos os usuários');
+                    carregarControleAcesso(recursoId);
+                } else {
+                    console.error('Erro ao habilitar acessos:', result.message);
+                }
+            })
+            .catch(erro => {
                 Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "habilitarAcessoTodosUsuarios", erro);
-            }
-        }
-
-        function mostrarAlerta(mensagem, tipo) {
-            try {
-
-                switch (tipo) {
-                    case 'success':
-                        return Alerta.Sucesso('Sucesso', mensagem);
-                    case 'danger':
-                    case 'error':
-                        return Alerta.Erro('Erro', mensagem);
-                    case 'warning':
-                        return Alerta.Warning('Atenção', mensagem);
-                    case 'info':
-                        return Alerta.Info('Informação', mensagem);
-                    default:
-                        return Alerta.Info('Aviso', mensagem);
-                }
-            } catch (erro) {
-
-                console.error('[mostrarAlerta] Erro ao exibir alerta:', erro);
-                console.log(`[${tipo}] ${mensagem}`);
-            }
-        }
-    </script>
+            });
+        } catch (erro) {
+            Alerta.TratamentoErroComLinha("GestaoRecursosNavegacao.cshtml", "habilitarAcessoTodosUsuarios", erro);
+        }
+    }
+
+    function mostrarAlerta(mensagem, tipo) {
+        try {
+
+            switch(tipo) {
+                case 'success':
+                    return Alerta.Sucesso('Sucesso', mensagem);
+                case 'danger':
+                case 'error':
+                    return Alerta.Erro('Erro', mensagem);
+                case 'warning':
+                    return Alerta.Warning('Atenção', mensagem);
+                case 'info':
+                    return Alerta.Info('Informação', mensagem);
+                default:
+                    return Alerta.Info('Aviso', mensagem);
+            }
+        } catch (erro) {
+
+            console.error('[mostrarAlerta] Erro ao exibir alerta:', erro);
+            console.log(`[${tipo}] ${mensagem}`);
+        }
+    }
+</script>
 }
```
