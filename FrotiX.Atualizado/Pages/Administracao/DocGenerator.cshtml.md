# Pages/Administracao/DocGenerator.cshtml

**Mudanca:** GRANDE | **+77** linhas | **-160** linhas

---

```diff
--- JANEIRO: Pages/Administracao/DocGenerator.cshtml
+++ ATUAL: Pages/Administracao/DocGenerator.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Administracao.DocGeneratorModel
 @{
     ViewData["Title"] = "Gerador de Documentação";
@@ -26,18 +25,13 @@
         right: -50%;
         width: 100%;
         height: 200%;
-        background: linear-gradient(45deg, transparent 30%, rgba(255, 255, 255, 0.03) 50%, transparent 70%);
+        background: linear-gradient(45deg, transparent 30%, rgba(255,255,255,0.03) 50%, transparent 70%);
         animation: shimmer 3s infinite;
     }
 
     @@keyframes shimmer {
-        0% {
-            transform: translateX(-100%) rotate(45deg);
-        }
-
-        100% {
-            transform: translateX(100%) rotate(45deg);
-        }
+        0% { transform: translateX(-100%) rotate(45deg); }
+        100% { transform: translateX(100%) rotate(45deg); }
     }
 
     .dashboard-header .header-icon {
@@ -45,7 +39,7 @@
         color: #fff;
         margin-right: 1rem;
         --fa-primary-color: #fff;
-        --fa-secondary-color: rgba(255, 255, 255, 0.7);
+        --fa-secondary-color: rgba(255,255,255,0.7);
         align-self: center;
     }
 
@@ -59,7 +53,7 @@
 
     .dashboard-header .subtitle {
         font-family: 'Outfit', sans-serif;
-        color: rgba(255, 255, 255, 0.75);
+        color: rgba(255,255,255,0.75);
         font-size: 0.85rem;
         margin-top: 0.15rem;
     }
@@ -155,7 +149,7 @@
         border-radius: 8px;
         padding: 1.5rem;
         margin-bottom: 1rem;
-        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
+        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
     }
 
     .progress-container.active {
@@ -237,17 +231,8 @@
     }
 
     @@keyframes pulse-logo {
-
-        0%,
-        100% {
-            transform: scale(1);
-            opacity: 1;
-        }
-
-        50% {
-            transform: scale(1.1);
-            opacity: 0.8;
-        }
+        0%, 100% { transform: scale(1); opacity: 1; }
+        50% { transform: scale(1.1); opacity: 0.8; }
     }
 
     .processing-title {
@@ -281,13 +266,8 @@
     }
 
     @@keyframes progress-shimmer {
-        0% {
-            background-position: 200% 0;
-        }
-
-        100% {
-            background-position: -200% 0;
-        }
+        0% { background-position: 200% 0; }
+        100% { background-position: -200% 0; }
     }
 
     .processing-file {
@@ -391,7 +371,7 @@
         border-radius: 8px;
         padding: 1rem;
         text-align: center;
-        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
+        box-shadow: 0 2px 8px rgba(0,0,0,0.08);
     }
 
     .stat-number {
@@ -441,29 +421,12 @@
     }
 
     /* Category Icons */
-    .category-controller {
-        --fa-primary-color: #3498db;
-    }
-
-    .category-page {
-        --fa-primary-color: #9b59b6;
-    }
-
-    .category-service {
-        --fa-primary-color: #e67e22;
-    }
-
-    .category-repository {
-        --fa-primary-color: #27ae60;
-    }
-
-    .category-helper {
-        --fa-primary-color: #f39c12;
-    }
-
-    .category-js {
-        --fa-primary-color: #f1c40f;
-    }
+    .category-controller { --fa-primary-color: #3498db; }
+    .category-page { --fa-primary-color: #9b59b6; }
+    .category-service { --fa-primary-color: #e67e22; }
+    .category-repository { --fa-primary-color: #27ae60; }
+    .category-helper { --fa-primary-color: #f39c12; }
+    .category-js { --fa-primary-color: #f1c40f; }
 </style>
 
 <div class="container-fluid">
@@ -707,37 +670,13 @@
 @section ScriptsBlock {
     <script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.0/signalr.min.js"></script>
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * DOC GENERATOR - GERADOR DE DOCUMENTAÇÃO AUTOMÁTICA
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Sistema de geração automática de documentação para arquivos C#
-            * Utiliza SignalR para comunicação em tempo real com fallback para polling
-                * Integração com IA(OpenAI / Google Gemini) para análise e documentação
-                    */
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * ESTADO GLOBAL DA APLICAÇÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /** @@type { string[] } Lista de caminhos de arquivos selecionados para documentação */
+
         let selectedPaths = [];
-            /** @@type { string | null } ID do job de geração em andamento */
         let currentJobId = null;
-            /** @@type { signalR.HubConnection | null } Conexão SignalR ativa */
         let connection = null;
-            /** @@type { number | null } Intervalo de polling(fallback quando SignalR falha) */
         let pollingInterval = null;
-            /** @@type { boolean } Flag para usar polling em vez de SignalR */
         let usePollingFallback = false;
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * INICIALIZAÇÃO DA PÁGINA
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
         document.addEventListener('DOMContentLoaded', function () {
             try {
                 initSignalR();
@@ -747,25 +686,14 @@
             }
         });
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * SIGNALR - COMUNICAÇÃO EM TEMPO REAL
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Inicializa conexão SignalR com fallback automático
-             * @@description Configura conexão com fallback: WebSockets → SSE → LongPolling
-            * @@returns { void}
-            */
         function initSignalR() {
             try {
                 connection = new signalR.HubConnectionBuilder()
                     .withUrl("/hubs/docgeneration", {
 
                         transport: signalR.HttpTransportType.WebSockets |
-                            signalR.HttpTransportType.ServerSentEvents |
-                            signalR.HttpTransportType.LongPolling,
+                                   signalR.HttpTransportType.ServerSentEvents |
+                                   signalR.HttpTransportType.LongPolling,
                         withCredentials: true
                     })
                     .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
@@ -780,15 +708,15 @@
                     addLog("Conectado ao servidor (ID: " + connectionId.substring(0, 8) + "...)", "info");
                 });
 
-                connection.onreconnecting(function (error) {
+                connection.onreconnecting(function(error) {
                     addLog("Reconectando ao servidor...", "warning");
                 });
 
-                connection.onreconnected(function (connectionId) {
+                connection.onreconnected(function(connectionId) {
                     addLog("Reconectado ao servidor", "success");
                 });
 
-                connection.onclose(function (error) {
+                connection.onclose(function(error) {
                     if (error) {
                         addLog("Conexão SignalR fechada: " + (error.message || error.toString()), "warning");
                     }
@@ -807,26 +735,14 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * ÁRVORE DE ARQUIVOS - CARREGAMENTO E RENDERIZAÇÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Carrega a árvore de arquivos do servidor
-             * @@description Faz requisição GET à API e renderiza estrutura de diretórios
-            * @@returns { void}
-            */
         function refreshTree() {
             try {
-
                 document.getElementById('treeContainer').innerHTML = `
-                            <div class="p-4 text-center text-muted">
-                                <i class="fa-duotone fa-spinner fa-spin fa-2x mb-2"></i>
-                                <p>Carregando estrutura...</p>
-                            </div>
-                        `;
+                    <div class="p-4 text-center text-muted">
+                        <i class="fa-duotone fa-spinner fa-spin fa-2x mb-2"></i>
+                        <p>Carregando estrutura...</p>
+                    </div>
+                `;
 
                 fetch('/api/docgenerator/discover')
                     .then(response => response.json())
@@ -847,37 +763,24 @@
             }
         }
 
-            /**
-             * Renderiza estatísticas de arquivos
-             * @@description Atualiza contadores na interface(total, com docs, precisam atualizar)
-            * @@param { Object } data - Dados de estatísticas do servidor
-                * @@returns { void}
-                */
-            function renderStats(data) {
-                try {
-                    document.getElementById('statTotal').textContent = data.totalFiles || 0;
-                    document.getElementById('statWithDocs').textContent = data.filesWithDocs || 0;
-                    document.getElementById('statNeedUpdate').textContent = data.filesNeedingUpdate || 0;
-
-                    const byCategory = data.filesByCategory || {};
-                    document.getElementById('statControllers').textContent = byCategory['Controller'] || 0;
-                    document.getElementById('statPages').textContent =
-                        (byCategory['RazorPage'] || 0) + (byCategory['RazorPageModel'] || 0);
-                    document.getElementById('statOthers').textContent =
-                        (byCategory['Service'] || 0) + (byCategory['Repository'] || 0) +
-                        (byCategory['JavaScript'] || 0) + (byCategory['Helper'] || 0);
-                } catch (erro) {
-                    Alerta.TratamentoErroComLinha("DocGenerator.cshtml", "renderStats", erro);
-                }
-            }
-
-            /**
-             * Renderiza a árvore de arquivos recursivamente
-             * @@description Cria HTML da estrutura de diretórios e arquivos com checkboxes
-            * @@param { Object } node - Nó da árvore(pasta ou arquivo)
-                * @@param { number } level - Nível de profundidade(para indentação)
-                    * @@returns { string } HTML da árvore
-                        */
+        function renderStats(data) {
+            try {
+                document.getElementById('statTotal').textContent = data.totalFiles || 0;
+                document.getElementById('statWithDocs').textContent = data.filesWithDocs || 0;
+                document.getElementById('statNeedUpdate').textContent = data.filesNeedingUpdate || 0;
+
+                const byCategory = data.filesByCategory || {};
+                document.getElementById('statControllers').textContent = byCategory['Controller'] || 0;
+                document.getElementById('statPages').textContent =
+                    (byCategory['RazorPage'] || 0) + (byCategory['RazorPageModel'] || 0);
+                document.getElementById('statOthers').textContent =
+                    (byCategory['Service'] || 0) + (byCategory['Repository'] || 0) +
+                    (byCategory['JavaScript'] || 0) + (byCategory['Helper'] || 0);
+            } catch (erro) {
+                Alerta.TratamentoErroComLinha("DocGenerator.cshtml", "renderStats", erro);
+            }
+        }
+
         function renderTree(node, level = 0) {
             try {
                 if (!node) return '';
@@ -890,15 +793,15 @@
 
                 if (node.children && node.children.length > 0) {
                     html += `<div class="tree-node tree-folder" style="padding-left: ${level * 16}px;">
-                                <span class="tree-toggle" onclick="toggleFolder(this)">
-                                    <i class="fa-duotone fa-caret-down"></i>
-                                </span>
-                                <input type="checkbox" class="tree-checkbox" data-path="${node.path}" onchange="toggleSelection(this)">
-                                <i class="fa-duotone fa-folder tree-icon"></i>
-                                <span>${node.name}</span>
-                                <span class="badge bg-secondary tree-badge">${node.totalFiles}</span>
-                            </div>
-                            <div class="tree-children">`;
+                        <span class="tree-toggle" onclick="toggleFolder(this)">
+                            <i class="fa-duotone fa-caret-down"></i>
+                        </span>
+                        <input type="checkbox" class="tree-checkbox" data-path="${node.path}" onchange="toggleSelection(this)">
+                        <i class="fa-duotone fa-folder tree-icon"></i>
+                        <span>${node.name}</span>
+                        <span class="badge bg-secondary tree-badge">${node.totalFiles}</span>
+                    </div>
+                    <div class="tree-children">`;
 
                     for (const child of node.children) {
                         html += renderTree(child, level + 1);
@@ -907,7 +810,6 @@
                     if (node.files && node.files.length > 0) {
                         for (const file of node.files) {
                             const icon = getCategoryIcon(file.category);
-
                             const badge = file.needsRegeneration
                                 ? '<span class="badge badge-needs-update tree-badge">Atualizar</span>'
                                 : (file.hasExistingDoc
@@ -915,12 +817,12 @@
                                     : '<span class="badge badge-new tree-badge">Novo</span>');
 
                             html += `<div class="tree-node tree-file" style="padding-left: ${(level + 1) * 16}px;">
-                                        <span class="tree-toggle"></span>
-                                        <input type="checkbox" class="tree-checkbox" data-path="${file.relativePath}" onchange="toggleSelection(this)">
-                                        <i class="${icon} tree-icon"></i>
-                                        <span>${file.fileName}</span>
-                                        ${badge}
-                                    </div>`;
+                                <span class="tree-toggle"></span>
+                                <input type="checkbox" class="tree-checkbox" data-path="${file.relativePath}" onchange="toggleSelection(this)">
+                                <i class="${icon} tree-icon"></i>
+                                <span>${file.fileName}</span>
+                                ${badge}
+                            </div>`;
                         }
                     }
 
@@ -935,12 +837,12 @@
                                 : '<span class="badge badge-new tree-badge">Novo</span>');
 
                         html += `<div class="tree-node tree-file" style="padding-left: ${level * 16}px;">
-                                    <span class="tree-toggle"></span>
-                                    <input type="checkbox" class="tree-checkbox" data-path="${file.relativePath}" onchange="toggleSelection(this)">
-                                    <i class="${icon} tree-icon"></i>
-                                    <span>${file.fileName}</span>
-                                    ${badge}
-                                </div>`;
+                            <span class="tree-toggle"></span>
+                            <input type="checkbox" class="tree-checkbox" data-path="${file.relativePath}" onchange="toggleSelection(this)">
+                            <i class="${icon} tree-icon"></i>
+                            <span>${file.fileName}</span>
+                            ${badge}
+                        </div>`;
                     }
                 }
 
@@ -1094,10 +996,10 @@
 
                             if (connection && connection.state === signalR.HubConnectionState.Connected) {
                                 connection.invoke("SubscribeToJob", currentJobId)
-                                    .then(function () {
+                                    .then(function() {
                                         addLog("Inscrito no SignalR para receber atualizações", "info");
                                     })
-                                    .catch(function (err) {
+                                    .catch(function(err) {
 
                                         addLog("SignalR indisponível, usando polling...", "warning");
                                         startPolling(currentJobId);
@@ -1172,7 +1074,7 @@
                     clearInterval(pollingInterval);
                 }
 
-                pollingInterval = setInterval(function () {
+                pollingInterval = setInterval(function() {
                     if (!currentJobId) {
                         stopPolling();
                         return;
@@ -1280,11 +1182,11 @@
                 document.getElementById('overlaySubtitle').textContent = 'Aguarde enquanto processamos seus arquivos';
 
                 document.getElementById('overlayLogContainer').innerHTML = `
-                            <div class="processing-log-entry">
-                                <span class="processing-log-time">[Sistema]</span>
-                                <span class="processing-log-info">Iniciando processamento...</span>
-                            </div>
-                        `;
+                    <div class="processing-log-entry">
+                        <span class="processing-log-time">[Sistema]</span>
+                        <span class="processing-log-info">Iniciando processamento...</span>
+                    </div>
+                `;
             } catch (erro) {
                 Alerta.TratamentoErroComLinha("DocGenerator.cshtml", "showProcessingOverlay", erro);
             }
@@ -1358,11 +1260,11 @@
         function clearLog() {
             try {
                 document.getElementById('logContainer').innerHTML = `
-                            <div class="log-entry">
-                                <span class="log-time">[Sistema]</span>
-                                <span class="log-info">Log limpo</span>
-                            </div>
-                        `;
+                    <div class="log-entry">
+                        <span class="log-time">[Sistema]</span>
+                        <span class="log-info">Log limpo</span>
+                    </div>
+                `;
             } catch (erro) {
                 Alerta.TratamentoErroComLinha("DocGenerator.cshtml", "clearLog", erro);
             }
```
