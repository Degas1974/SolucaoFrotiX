# Pages/Shared/Components/Navigation/TreeView.cshtml

**Mudanca:** GRANDE | **+209** linhas | **-302** linhas

---

```diff
--- JANEIRO: Pages/Shared/Components/Navigation/TreeView.cshtml
+++ ATUAL: Pages/Shared/Components/Navigation/TreeView.cshtml
@@ -11,220 +11,200 @@
 }
 
 <style>
-    /* ====== NAVEGAÇÃO SYNCFUSION TREEVIEW ====== */
-
-    /* Container principal */
-    .nav-syncfusion {
-        padding: 0;
-        margin: 0;
-    }
-
-    /* Remove estilos padrão do TreeView */
-    .nav-syncfusion .e-treeview {
-        background: transparent !important;
-        border: none !important;
-    }
-
-    .nav-syncfusion .e-treeview .e-ul {
-        padding: 0;
-        margin: 0;
-        background: transparent !important;
-        list-style: none;
-    }
-
-    /* Indentação para listas aninhadas (filhos) */
-    .nav-syncfusion .e-treeview .e-list-item>.e-ul {
-        padding-left: 15px !important;
-    }
-
-    .nav-syncfusion .e-treeview .e-list-item {
-        padding: 0;
-        margin: 0;
-        background: transparent !important;
-    }
-
-    /* Texto/conteúdo do item */
-    .nav-syncfusion .e-treeview .e-text-content {
-        padding: 0 !important;
-        background: transparent !important;
-    }
-
-    /* Link do menu */
-    .nav-syncfusion .nav-link {
-        display: flex;
-        align-items: center;
-        padding: 10px 15px;
-        color: #fff;
-        text-decoration: none;
-        position: relative;
-        transition: background-color 0.2s ease;
-    }
-
-    .nav-syncfusion .nav-link:hover {
-        background: rgba(255, 255, 255, 0.1);
-    }
-
-    /* ====== CORES DOS ÍCONES DUOTONE (Padrão FrotiX) ====== */
-    .nav-syncfusion .nav-icon.fa-duotone {
-        --fa-primary-color: #ff6b35;
-        /* Laranja forte (padrão FrotiX) */
-        --fa-secondary-color: #6c757d;
-        /* Cinza médio (padrão FrotiX) */
-        --fa-primary-opacity: 1;
-        --fa-secondary-opacity: 1;
-        font-size: 1.1rem;
-        width: 20px;
-        text-align: center;
-        margin-right: 12px;
-        /* Espaçamento adequado entre ícone e texto (2 caracteres) */
-        flex-shrink: 0;
-        /* Evita que o ícone encolha */
-    }
-
-    /* Ícones dos subitens - mantém mesmo padrão de cores */
-    .nav-syncfusion .nivel-1 .nav-icon.fa-duotone,
-    .nav-syncfusion .nivel-2 .nav-icon.fa-duotone {
-        --fa-primary-color: #ff6b35;
-        /* Laranja forte (padrão FrotiX) */
-        --fa-secondary-color: #6c757d;
-        /* Cinza médio (padrão FrotiX) */
-        font-size: 0.95rem;
-        margin-right: 12px;
-        /* Espaçamento consistente */
-    }
-
-    /* ====== INDENTAÇÃO POR NÍVEL ====== */
-    /* A indentação principal é feita pelo TreeView via .e-ul aninhado */
-    /* Estas classes adicionam padding interno ao link */
-    .nav-syncfusion .nivel-0 .nav-link {
-        padding-left: 15px;
-    }
-
-    .nav-syncfusion .nivel-1 .nav-link {
-        padding-left: 10px;
-    }
-
-    .nav-syncfusion .nivel-2 .nav-link {
-        padding-left: 10px;
-    }
-
-    /* ====== SETA DE EXPANSÃO - POSICIONADA À DIREITA ====== */
-    .nav-syncfusion .e-treeview .e-text-content {
-        display: flex !important;
-        align-items: center;
-        width: 100%;
-        position: relative;
-    }
-
-    .nav-syncfusion .e-treeview .e-icons.e-icon-collapsible,
-    .nav-syncfusion .e-treeview .e-icons.e-icon-expandable {
-        color: #c67750 !important;
-        font-size: 0.75rem;
-        position: absolute;
-        right: 10px;
-        top: 50%;
-        transform: translateY(-50%);
-        z-index: 10;
-        transition: transform 0.2s ease;
-    }
-
-    /* Seta para BAIXO quando fechado (expandable) - Font Awesome Pro 7 */
-    .nav-syncfusion .e-treeview .e-icons.e-icon-expandable::before {
-        content: "\f078" !important;
-        /* fa-chevron-down */
-        font-family: "Font Awesome 7 Pro", "Font Awesome 6 Pro", "FontAwesome" !important;
-        font-weight: 900;
-    }
-
-    /* Seta para CIMA quando aberto (collapsible) - Font Awesome Pro 7 */
-    .nav-syncfusion .e-treeview .e-icons.e-icon-collapsible::before {
-        content: "\f077" !important;
-        /* fa-chevron-up */
-        font-family: "Font Awesome 7 Pro", "Font Awesome 6 Pro", "FontAwesome" !important;
-        font-weight: 900;
-    }
-
-    /* ====== INDICADOR DE SELEÇÃO (BOLINHA LARANJA) ====== */
-    .nav-syncfusion .selection-dot {
-        display: none;
-        position: absolute;
-        right: 28px;
-        /* À direita, antes da seta */
-        top: 50%;
-        transform: translateY(-50%);
-        font-size: 0.5rem;
-        color: #c67750 !important;
-        /* Laranja */
-    }
-
-    /* Bolinha visível APENAS no link com classe ftx-page-active */
-    .nav-syncfusion .nav-link.ftx-page-active .selection-dot {
-        display: inline-block !important;
-    }
-
-    /* IMPORTANTE: Garante que bolinha NÃO apareça em itens .e-active do Syncfusion */
-    .nav-syncfusion .e-list-item.e-active .selection-dot,
-    .nav-syncfusion .e-list-item.e-node-focus .selection-dot {
-        display: none !important;
-    }
-
-    /* Só mostra bolinha se o LINK tiver ftx-page-active (não o list-item) */
-    .nav-syncfusion .e-list-item.e-active .nav-link:not(.ftx-page-active) .selection-dot {
-        display: none !important;
-    }
-
-    /* ====== ITEM ATIVO (apenas página atual) ====== */
-    /* Aplica APENAS ao link com ftx-page-active (definido via JS baseado na URL) */
-    .nav-syncfusion .nav-link.ftx-page-active {
-        background: rgba(255, 193, 7, 0.15) !important;
-        border-left: 3px solid #ffc107;
-    }
-
-    /* Remove destaque do Syncfusion ao clicar (não queremos isso) */
-    .nav-syncfusion .e-list-item.e-active>.e-text-content>.nav-link {
-        background: transparent !important;
-        border-left: none !important;
-    }
-
-    /* Texto do item - permite quebra de linha */
-    .nav-syncfusion .nav-text {
-        flex: 1;
-        font-size: 0.875rem;
-        white-space: normal;
-        word-wrap: break-word;
-        line-height: 1.3;
-        padding-right: 30px;
-        /* Espaço para a seta e bolinha */
-    }
-
-    /* Remove fullRow highlight padrão do Syncfusion */
-    .nav-syncfusion .e-treeview .e-fullrow {
-        display: none !important;
-    }
-
-    /* Cursor pointer em todos os itens */
-    .nav-syncfusion .e-treeview .e-list-item {
-        cursor: pointer;
-    }
-
-    /* OCULTA ícones padrão do Syncfusion (usamos apenas os do template) */
-    .nav-syncfusion .e-treeview .e-list-icon {
-        display: none !important;
-    }
-
-    /* Garante que filhos expandidos sejam visíveis */
-    .nav-syncfusion .e-treeview .e-list-item.e-node-collapsed>.e-ul {
-        display: none;
-    }
-
-    .nav-syncfusion .e-treeview .e-list-item:not(.e-node-collapsed)>.e-ul {
-        display: block !important;
-    }
-
-    /* Estilo para itens pai (com filhos) */
-    .nav-syncfusion .e-treeview .e-list-item.e-has-child>.e-text-content {
-        font-weight: 500;
-    }
+/* ====== NAVEGAÇÃO SYNCFUSION TREEVIEW ====== */
+
+/* Container principal */
+.nav-syncfusion {
+    padding: 0;
+    margin: 0;
+}
+
+/* Remove estilos padrão do TreeView */
+.nav-syncfusion .e-treeview {
+    background: transparent !important;
+    border: none !important;
+}
+
+.nav-syncfusion .e-treeview .e-ul {
+    padding: 0;
+    margin: 0;
+    background: transparent !important;
+    list-style: none;
+}
+
+/* Indentação para listas aninhadas (filhos) */
+.nav-syncfusion .e-treeview .e-list-item > .e-ul {
+    padding-left: 15px !important;
+}
+
+.nav-syncfusion .e-treeview .e-list-item {
+    padding: 0;
+    margin: 0;
+    background: transparent !important;
+}
+
+/* Texto/conteúdo do item */
+.nav-syncfusion .e-treeview .e-text-content {
+    padding: 0 !important;
+    background: transparent !important;
+}
+
+/* Link do menu */
+.nav-syncfusion .nav-link {
+    display: flex;
+    align-items: center;
+    padding: 10px 15px;
+    color: #fff;
+    text-decoration: none;
+    position: relative;
+    transition: background-color 0.2s ease;
+}
+
+.nav-syncfusion .nav-link:hover {
+    background: rgba(255, 255, 255, 0.1);
+}
+
+/* ====== CORES DOS ÍCONES DUOTONE (Padrão FrotiX) ====== */
+.nav-syncfusion .nav-icon.fa-duotone {
+    --fa-primary-color: #ff6b35; /* Laranja forte (padrão FrotiX) */
+    --fa-secondary-color: #6c757d; /* Cinza médio (padrão FrotiX) */
+    --fa-primary-opacity: 1;
+    --fa-secondary-opacity: 1;
+    font-size: 1.1rem;
+    width: 20px;
+    text-align: center;
+    margin-right: 12px; /* Espaçamento adequado entre ícone e texto (2 caracteres) */
+    flex-shrink: 0; /* Evita que o ícone encolha */
+}
+
+/* Ícones dos subitens - mantém mesmo padrão de cores */
+.nav-syncfusion .nivel-1 .nav-icon.fa-duotone,
+.nav-syncfusion .nivel-2 .nav-icon.fa-duotone {
+    --fa-primary-color: #ff6b35; /* Laranja forte (padrão FrotiX) */
+    --fa-secondary-color: #6c757d; /* Cinza médio (padrão FrotiX) */
+    font-size: 0.95rem;
+    margin-right: 12px; /* Espaçamento consistente */
+}
+
+/* ====== INDENTAÇÃO POR NÍVEL ====== */
+/* A indentação principal é feita pelo TreeView via .e-ul aninhado */
+/* Estas classes adicionam padding interno ao link */
+.nav-syncfusion .nivel-0 .nav-link { padding-left: 15px; }
+.nav-syncfusion .nivel-1 .nav-link { padding-left: 10px; }
+.nav-syncfusion .nivel-2 .nav-link { padding-left: 10px; }
+
+/* ====== SETA DE EXPANSÃO - POSICIONADA À DIREITA ====== */
+.nav-syncfusion .e-treeview .e-text-content {
+    display: flex !important;
+    align-items: center;
+    width: 100%;
+    position: relative;
+}
+
+.nav-syncfusion .e-treeview .e-icons.e-icon-collapsible,
+.nav-syncfusion .e-treeview .e-icons.e-icon-expandable {
+    color: #c67750 !important;
+    font-size: 0.75rem;
+    position: absolute;
+    right: 10px;
+    top: 50%;
+    transform: translateY(-50%);
+    z-index: 10;
+    transition: transform 0.2s ease;
+}
+
+/* Seta para BAIXO quando fechado (expandable) - Font Awesome Pro 7 */
+.nav-syncfusion .e-treeview .e-icons.e-icon-expandable::before {
+    content: "\f078" !important; /* fa-chevron-down */
+    font-family: "Font Awesome 7 Pro", "Font Awesome 6 Pro", "FontAwesome" !important;
+    font-weight: 900;
+}
+
+/* Seta para CIMA quando aberto (collapsible) - Font Awesome Pro 7 */
+.nav-syncfusion .e-treeview .e-icons.e-icon-collapsible::before {
+    content: "\f077" !important; /* fa-chevron-up */
+    font-family: "Font Awesome 7 Pro", "Font Awesome 6 Pro", "FontAwesome" !important;
+    font-weight: 900;
+}
+
+/* ====== INDICADOR DE SELEÇÃO (BOLINHA LARANJA) ====== */
+.nav-syncfusion .selection-dot {
+    display: none;
+    position: absolute;
+    right: 28px; /* À direita, antes da seta */
+    top: 50%;
+    transform: translateY(-50%);
+    font-size: 0.5rem;
+    color: #c67750 !important; /* Laranja */
+}
+
+/* Bolinha visível APENAS no link com classe ftx-page-active */
+.nav-syncfusion .nav-link.ftx-page-active .selection-dot {
+    display: inline-block !important;
+}
+
+/* IMPORTANTE: Garante que bolinha NÃO apareça em itens .e-active do Syncfusion */
+.nav-syncfusion .e-list-item.e-active .selection-dot,
+.nav-syncfusion .e-list-item.e-node-focus .selection-dot {
+    display: none !important;
+}
+
+/* Só mostra bolinha se o LINK tiver ftx-page-active (não o list-item) */
+.nav-syncfusion .e-list-item.e-active .nav-link:not(.ftx-page-active) .selection-dot {
+    display: none !important;
+}
+
+/* ====== ITEM ATIVO (apenas página atual) ====== */
+/* Aplica APENAS ao link com ftx-page-active (definido via JS baseado na URL) */
+.nav-syncfusion .nav-link.ftx-page-active {
+    background: rgba(255, 193, 7, 0.15) !important;
+    border-left: 3px solid #ffc107;
+}
+
+/* Remove destaque do Syncfusion ao clicar (não queremos isso) */
+.nav-syncfusion .e-list-item.e-active > .e-text-content > .nav-link {
+    background: transparent !important;
+    border-left: none !important;
+}
+
+/* Texto do item - permite quebra de linha */
+.nav-syncfusion .nav-text {
+    flex: 1;
+    font-size: 0.875rem;
+    white-space: normal;
+    word-wrap: break-word;
+    line-height: 1.3;
+    padding-right: 30px; /* Espaço para a seta e bolinha */
+}
+
+/* Remove fullRow highlight padrão do Syncfusion */
+.nav-syncfusion .e-treeview .e-fullrow {
+    display: none !important;
+}
+
+/* Cursor pointer em todos os itens */
+.nav-syncfusion .e-treeview .e-list-item {
+    cursor: pointer;
+}
+
+/* OCULTA ícones padrão do Syncfusion (usamos apenas os do template) */
+.nav-syncfusion .e-treeview .e-list-icon {
+    display: none !important;
+}
+
+/* Garante que filhos expandidos sejam visíveis */
+.nav-syncfusion .e-treeview .e-list-item.e-node-collapsed > .e-ul {
+    display: none;
+}
+
+.nav-syncfusion .e-treeview .e-list-item:not(.e-node-collapsed) > .e-ul {
+    display: block !important;
+}
+
+/* Estilo para itens pai (com filhos) */
+.nav-syncfusion .e-treeview .e-list-item.e-has-child > .e-text-content {
+    font-weight: 500;
+}
 </style>
 
 <nav class="nav-syncfusion">
@@ -232,87 +212,52 @@
 </nav>
 
 <script>
-/**
- * ═══════════════════════════════════════════════════════════════════════════
- * COMPONENTE DE NAVEGAÇÃO TREEVIEW - SYNCFUSION EJ2
- * ═══════════════════════════════════════════════════════════════════════════
- * @@description Sistema de navegação em árvore do FrotiX usando Syncfusion TreeView.
- * Gerencia menu lateral com suporte a multinível, ícones Duotone,
- * expansão inteligente e marcação de página ativa.
- * @@requires Syncfusion EJ2 Navigations(TreeView)
-        * @@requires FontAwesome Duotone
-            * @@file Shared / Components / Navigation / TreeView.cshtml
-            */
-
-/**
- * Inicializa o TreeView de navegação principal
- * @@description IIFE que aguarda Syncfusion carregar e licença ser validada,
- * então renderiza o menu de navegação com URLs convertidas e
-        * expande automaticamente até a página ativa.
- */
-                (function initNavTreeView() {
-                    var currentPage = '@pageName';
-                    var navData = @Html.Raw(jsonData);
-
-                    if (typeof ej === 'undefined' || typeof ej.navigations === 'undefined') {
-
-                        setTimeout(initNavTreeView, 100);
-                        return;
-                    }
-
-                    if (!window.syncfusionLicenseReady) {
-
-                        document.addEventListener('syncfusion-license-ready', initNavTreeView, { once: true });
-                        return;
-                    }
-
-    /**
-     * Converte URL do formato legado para o novo padrão ASP.NET
-     * @@param { string } href - URL no formato antigo(ex: "administracao_gerenciadornavegacao.html")
-                        * @@returns { string } URL convertida(ex: "/Administracao/GerenciadorNavegacao")
-                            * @@example convertHref("viagens_index.html") → "/Viagens/Index"
-                                */
-                    function convertHref(href) {
-                        if (!href || href === 'javascript:void(0);' || href.startsWith('/')) {
-                            return href;
-                        }
-
-                        var converted = href.replace('.html', '');
-
-                        var parts = converted.split('_');
-                        var capitalizedParts = parts.map(function (part) {
-                            return part.charAt(0).toUpperCase() + part.slice(1);
-                        });
-
-                        return '/' + capitalizedParts.join('/');
-                    }
-
-    /**
-     * Converte URLs recursivamente em toda a árvore de navegação
-     * @@param { Array < Object >} items - Array de itens de navegação
-                        * @@returns { Array<Object>} Nova árvore com URLs convertidas
-                            */
-                    function convertUrlsRecursive(items) {
-                        if (!items) return items;
-                        return items.map(function (item) {
-                            var newItem = Object.assign({}, item);
-                            newItem.href = convertHref(item.href);
-                            if (item.items && item.items.length > 0) {
-                                newItem.items = convertUrlsRecursive(item.items);
-                            }
-                            return newItem;
-                        });
-                    }
-
-                    navData = convertUrlsRecursive(navData);
-
-    /**
-     * Busca item ativo na árvore baseado na URL atual
-     * @@param { Array < Object >} items - Array de itens de navegação
-                        * @@param { string } href - URL para localizar(parcial ou completa)
-                            * @@returns { string| null
-                } ID do item encontrado ou null
-                    */
+
+(function initNavTreeView() {
+    var currentPage = '@pageName';
+    var navData = @Html.Raw(jsonData);
+
+    if (typeof ej === 'undefined' || typeof ej.navigations === 'undefined') {
+
+        setTimeout(initNavTreeView, 100);
+        return;
+    }
+
+    if (!window.syncfusionLicenseReady) {
+
+        document.addEventListener('syncfusion-license-ready', initNavTreeView, { once: true });
+        return;
+    }
+
+    function convertHref(href) {
+        if (!href || href === 'javascript:void(0);' || href.startsWith('/')) {
+            return href;
+        }
+
+        var converted = href.replace('.html', '');
+
+        var parts = converted.split('_');
+        var capitalizedParts = parts.map(function(part) {
+            return part.charAt(0).toUpperCase() + part.slice(1);
+        });
+
+        return '/' + capitalizedParts.join('/');
+    }
+
+    function convertUrlsRecursive(items) {
+        if (!items) return items;
+        return items.map(function(item) {
+            var newItem = Object.assign({}, item);
+            newItem.href = convertHref(item.href);
+            if (item.items && item.items.length > 0) {
+                newItem.items = convertUrlsRecursive(item.items);
+            }
+            return newItem;
+        });
+    }
+
+    navData = convertUrlsRecursive(navData);
+
     function findActiveItem(items, href) {
         for (var i = 0; i < items.length; i++) {
             var item = items[i];
@@ -327,13 +272,6 @@
         return null;
     }
 
-    /**
-     * Template personalizado para renderização de itens do menu
-     * @@param { Object } data - Dados do item(text, icon, href, nivel)
-        * @@returns { string } HTML do item de navegação
-            * @@description Garante ícones Duotone(padrão FrotiX), aplica classe de nível
-                * e adiciona indicador visual de seleção(selection - dot).
-     */
     function nodeTemplate(data) {
         var nivelClass = 'nivel-' + (data.nivel || 0);
 
@@ -361,15 +299,6 @@
         return html;
     }
 
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * INSTÂNCIA DO TREEVIEW
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Configuração e inicialização do componente Syncfusion TreeView.
-     * - Inicia fechado(expandOn: 'None')
-        * - Navegação controlada manualmente via nodeClicked
-            * - Itens pai apenas expandem / colapsam, nunca navegam
-                */
     var treeObj = new ej.navigations.TreeView({
         fields: {
             dataSource: navData,
@@ -382,7 +311,7 @@
         nodeTemplate: nodeTemplate,
         cssClass: 'nav-menu-tree',
         expandOn: 'None',
-        nodeClicked: function (args) {
+        nodeClicked: function(args) {
 
             if (args.event) {
                 args.event.preventDefault();
@@ -396,8 +325,8 @@
             var clickedElement = args.event ? args.event.target : null;
             if (clickedElement &&
                 (clickedElement.classList.contains('e-icons') ||
-                    clickedElement.classList.contains('e-icon-expandable') ||
-                    clickedElement.classList.contains('e-icon-collapsible'))) {
+                 clickedElement.classList.contains('e-icon-expandable') ||
+                 clickedElement.classList.contains('e-icon-collapsible'))) {
 
                 if (args.node && args.node.classList.contains('e-node-collapsed')) {
                     treeObj.expandAll([args.nodeData.id]);
@@ -430,18 +359,6 @@
 
     treeObj.appendTo('#navTreeView');
 
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * FUNÇÕES AUXILIARES DE EXPANSÃO E NAVEGAÇÃO
-     * ═══════════════════════════════════════════════════════════════════════════
-     */
-
-    /**
-     * Expande manualmente um nó pai via manipulação direta do DOM
-     * @@param { HTMLElement } nodeElement - Elemento li.e - list - item do nó a expandir
-        * @@description Remove classe e - node - collapsed, exibe ul filho e troca ícone.
-     * Usado como fallback quando API do TreeView não responde.
-     */
     function expandirPaiManualmente(nodeElement) {
         if (!nodeElement) return;
         nodeElement.classList.remove('e-node-collapsed');
@@ -456,12 +373,6 @@
         }
     }
 
-    /**
-     * Encontra o melhor match de item ativo baseado na URL atual
-     * @@returns { HTMLElement | null } Elemento.nav - link correspondente ou null
-        * @@description Compara URL atual com todos os links do menu, retornando
-            * o que tem maior correspondência(prefere match mais longo).
-     */
     function encontrarItemAtivo() {
         var currentUrl = window.location.pathname.toLowerCase();
         if (currentUrl.endsWith('/')) {
@@ -472,7 +383,7 @@
         var bestMatch = null;
         var bestMatchLength = 0;
 
-        allLinks.forEach(function (link) {
+        allLinks.forEach(function(link) {
             var href = link.getAttribute('href');
             if (!href || href === 'javascript:void(0);') {
                 return;
@@ -494,13 +405,6 @@
         return bestMatch;
     }
 
-    /**
-     * Coleta IDs de todos os nós pais de um elemento
-     * @@param { HTMLElement } listItem - Elemento li.e - list - item filho
-        * @@returns { Array<string>} Array de IDs dos pais(do mais próximo ao mais distante)
-     * @@description Percorre a árvore DOM subindo pelos e - list - item pais,
-     * coletando data - uid ou obtendo via API getNode.
-     */
     function coletarIdsPais(listItem) {
         var ids = [];
         if (!listItem) return ids;
@@ -527,7 +431,7 @@
         return ids;
     }
 
-    setTimeout(function () {
+    setTimeout(function() {
 
         var itemAtivo = encontrarItemAtivo();
         var idsPaisParaExpandir = [];
@@ -541,15 +445,15 @@
 
         treeObj.collapseAll();
 
-        setTimeout(function () {
+        setTimeout(function() {
             if (idsPaisParaExpandir.length > 0) {
-                idsPaisParaExpandir.reverse().forEach(function (parentId) {
+                idsPaisParaExpandir.reverse().forEach(function(parentId) {
                     treeObj.expandAll([parentId]);
                 });
             }
         }, 100);
 
-        setTimeout(function () {
+        setTimeout(function() {
             if (itemAtivo) {
                 var listItem = itemAtivo.closest('.e-list-item');
                 if (listItem) {
@@ -577,5 +481,5 @@
             }
         }, 500);
     }, 250);
-}) ();
+})();
 </script>
```
