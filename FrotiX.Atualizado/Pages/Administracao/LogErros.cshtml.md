# Pages/Administracao/LogErros.cshtml

**Mudanca:** GRANDE | **+670** linhas | **-265** linhas

---

```diff
--- JANEIRO: Pages/Administracao/LogErros.cshtml
+++ ATUAL: Pages/Administracao/LogErros.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Administracao.LogErrosModel
 
 @{
@@ -14,6 +13,17 @@
     <link rel="preconnect" href="https://fonts.googleapis.com">
     <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
     <link href="https://fonts.googleapis.com/css2?family=Outfit:wght@400;600;700;900&display=swap" rel="stylesheet">
+
+    <script src="https://cdn.jsdelivr.net/npm/marked/marked.min.js"></script>
+
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/styles/github-dark.min.css">
+    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/core.min.js"></script>
+    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/csharp.min.js"></script>
+    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/javascript.min.js"></script>
+    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/json.min.js"></script>
+    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/sql.min.js"></script>
+    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/xml.min.js"></script>
+    <script src="https://cdn.jsdelivr.net/npm/highlight.js@11.9.0/lib/languages/bash.min.js"></script>
     <style>
         /* ====== HEADER AZUL PADRÃO FROTIX ====== */
         .ftx-card-header {
@@ -36,18 +46,13 @@
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
@@ -89,7 +94,7 @@
             border-radius: 8px;
             padding: 1rem;
             text-align: center;
-            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06);
+            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
             border-left: 4px solid #3D5771;
             transition: all 0.2s ease;
             cursor: pointer;
@@ -98,7 +103,7 @@
 
         .stat-card:hover {
             transform: translateY(-3px);
-            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
+            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
         }
 
         .stat-card:active {
@@ -106,49 +111,24 @@
         }
 
         .stat-card.active {
-            box-shadow: 0 0 0 3px rgba(61, 87, 113, 0.3), 0 4px 12px rgba(0, 0, 0, 0.15);
+            box-shadow: 0 0 0 3px rgba(61, 87, 113, 0.3), 0 4px 12px rgba(0,0,0,0.15);
             transform: translateY(-2px);
         }
 
-        .stat-card.errors {
-            border-left-color: #dc3545;
-        }
-
-        .stat-card.errors.active {
-            box-shadow: 0 0 0 3px rgba(220, 53, 69, 0.3), 0 4px 12px rgba(0, 0, 0, 0.15);
-        }
-
-        .stat-card.warnings {
-            border-left-color: #ffc107;
-        }
-
-        .stat-card.warnings.active {
-            box-shadow: 0 0 0 3px rgba(255, 193, 7, 0.3), 0 4px 12px rgba(0, 0, 0, 0.15);
-        }
-
-        .stat-card.info {
-            border-left-color: #17a2b8;
-        }
-
-        .stat-card.info.active {
-            box-shadow: 0 0 0 3px rgba(23, 162, 184, 0.3), 0 4px 12px rgba(0, 0, 0, 0.15);
-        }
-
-        .stat-card.js-errors {
-            border-left-color: #6f42c1;
-        }
-
-        .stat-card.js-errors.active {
-            box-shadow: 0 0 0 3px rgba(111, 66, 193, 0.3), 0 4px 12px rgba(0, 0, 0, 0.15);
-        }
-
-        .stat-card.http-errors {
-            border-left-color: #fd7e14;
-        }
-
-        .stat-card.http-errors.active {
-            box-shadow: 0 0 0 3px rgba(253, 126, 20, 0.3), 0 4px 12px rgba(0, 0, 0, 0.15);
-        }
+        .stat-card.errors { border-left-color: #dc3545; }
+        .stat-card.errors.active { box-shadow: 0 0 0 3px rgba(220, 53, 69, 0.3), 0 4px 12px rgba(0,0,0,0.15); }
+
+        .stat-card.warnings { border-left-color: #ffc107; }
+        .stat-card.warnings.active { box-shadow: 0 0 0 3px rgba(255, 193, 7, 0.3), 0 4px 12px rgba(0,0,0,0.15); }
+
+        .stat-card.info { border-left-color: #17a2b8; }
+        .stat-card.info.active { box-shadow: 0 0 0 3px rgba(23, 162, 184, 0.3), 0 4px 12px rgba(0,0,0,0.15); }
+
+        .stat-card.js-errors { border-left-color: #6f42c1; }
+        .stat-card.js-errors.active { box-shadow: 0 0 0 3px rgba(111, 66, 193, 0.3), 0 4px 12px rgba(0,0,0,0.15); }
+
+        .stat-card.http-errors { border-left-color: #fd7e14; }
+        .stat-card.http-errors.active { box-shadow: 0 0 0 3px rgba(253, 126, 20, 0.3), 0 4px 12px rgba(0,0,0,0.15); }
 
         .stat-value {
             font-size: 2rem;
@@ -157,25 +137,11 @@
             line-height: 1;
         }
 
-        .stat-card.errors .stat-value {
-            color: #dc3545;
-        }
-
-        .stat-card.warnings .stat-value {
-            color: #ffc107;
-        }
-
-        .stat-card.info .stat-value {
-            color: #17a2b8;
-        }
-
-        .stat-card.js-errors .stat-value {
-            color: #6f42c1;
-        }
-
-        .stat-card.http-errors .stat-value {
-            color: #fd7e14;
-        }
+        .stat-card.errors .stat-value { color: #dc3545; }
+        .stat-card.warnings .stat-value { color: #ffc107; }
+        .stat-card.info .stat-value { color: #17a2b8; }
+        .stat-card.js-errors .stat-value { color: #6f42c1; }
+        .stat-card.http-errors .stat-value { color: #fd7e14; }
 
         .stat-label {
             font-size: 0.75rem;
@@ -197,7 +163,7 @@
             border-radius: 8px;
             padding: 1rem;
             margin-bottom: 1rem;
-            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06);
+            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
         }
 
         .filtro-ativo {
@@ -242,30 +208,13 @@
             background: #2d2d30;
         }
 
-        .log-block.error {
-            border-left-color: #f14c4c;
-        }
-
-        .log-block.warning {
-            border-left-color: #cca700;
-        }
-
-        .log-block.info {
-            border-left-color: #3794ff;
-        }
-
-        .log-block.http-error {
-            border-left-color: #e67e22;
-            background: #2d2a26;
-        }
-
-        .log-block.js-error {
-            border-left-color: #b388ff;
-        }
-
-        .log-block.user {
-            border-left-color: #c586c0;
-        }
+        .log-block.error { border-left-color: #f14c4c; }
+        .log-block.warning { border-left-color: #cca700; }
+        .log-block.info { border-left-color: #3794ff; }
+        .log-block.http-error { border-left-color: #e67e22; background: #2d2a26; }
+        .log-block.js-error { border-left-color: #b388ff; }
+        .log-block.user { border-left-color: #c586c0; }
+        .log-block.console { border-left-color: #9333ea; background: #2a2640; }
 
         /* ====== CABEÇALHO DO LOG (VERDE/COLORIDO) ====== */
         .log-header {
@@ -279,47 +228,51 @@
             flex-wrap: wrap;
         }
 
-        .log-header.error {
-            background: linear-gradient(90deg, #c62828 0%, #b71c1c 100%);
-        }
-
-        .log-header.warning {
-            background: linear-gradient(90deg, #f9a825 0%, #f57f17 100%);
-            color: #000;
-        }
-
-        .log-header.info {
-            background: linear-gradient(90deg, #1565c0 0%, #0d47a1 100%);
-        }
-
-        .log-header.http-error {
-            background: linear-gradient(90deg, #d35400 0%, #a04000 100%);
-        }
-
-        .log-header.js-error {
-            background: linear-gradient(90deg, #7b1fa2 0%, #4a148c 100%);
-        }
-
-        .log-header.user {
-            background: linear-gradient(90deg, #7b1fa2 0%, #6a1b9a 100%);
-        }
-
-        .log-header.operation {
-            background: linear-gradient(90deg, #00695c 0%, #004d40 100%);
+        .log-header.error { background: linear-gradient(90deg, #c62828 0%, #b71c1c 100%); }
+        .log-header.warning { background: linear-gradient(90deg, #f9a825 0%, #f57f17 100%); color: #000; }
+        .log-header.info { background: linear-gradient(90deg, #1565c0 0%, #0d47a1 100%); }
+        .log-header.http-error { background: linear-gradient(90deg, #d35400 0%, #a04000 100%); }
+        .log-header.js-error { background: linear-gradient(90deg, #7b1fa2 0%, #4a148c 100%); }
+        .log-header.user { background: linear-gradient(90deg, #7b1fa2 0%, #6a1b9a 100%); }
+        .log-header.operation { background: linear-gradient(90deg, #00695c 0%, #004d40 100%); }
+        .log-header.console { background: linear-gradient(90deg, #7c3aed 0%, #5b21b6 100%); }
+
+        /* ====== BADGES DE ORIGEM ====== */
+        .log-origin-badge {
+            display: inline-block;
+            padding: 0.15rem 0.5rem;
+            border-radius: 4px;
+            font-size: 0.65rem;
+            font-weight: 700;
+            letter-spacing: 0.5px;
+            text-transform: uppercase;
+            margin-left: 0.5rem;
+        }
+
+        .log-origin-badge.server {
+            background: rgba(37, 99, 235, 0.2);
+            color: #60a5fa;
+            border: 1px solid rgba(96, 165, 250, 0.3);
+        }
+
+        .log-origin-badge.client {
+            background: rgba(124, 58, 237, 0.2);
+            color: #c084fc;
+            border: 1px solid rgba(192, 132, 252, 0.3);
         }
 
         .log-header .log-datetime {
             display: flex;
             align-items: center;
             gap: 0.5rem;
-            background: rgba(255, 255, 255, 0.15);
+            background: rgba(255,255,255,0.15);
             padding: 0.2rem 0.5rem;
             border-radius: 4px;
             font-size: 0.75rem;
         }
 
         .log-header .log-type {
-            background: rgba(0, 0, 0, 0.25);
+            background: rgba(0,0,0,0.25);
             padding: 0.15rem 0.5rem;
             border-radius: 4px;
             font-size: 0.7rem;
@@ -347,45 +300,306 @@
         }
 
         /* Cores para cada tipo de detalhe */
-        .detail-line.usuario {
-            color: #9cdcfe;
-        }
-
-        .detail-line.message {
-            color: #ce9178;
-        }
-
-        .detail-line.method {
-            color: #dcdcaa;
-        }
-
-        .detail-line.path {
-            color: #4ec9b0;
-        }
-
-        .detail-line.arquivo {
-            color: #9cdcfe;
-        }
-
-        .detail-line.metodo {
-            color: #dcdcaa;
-        }
-
-        .detail-line.linha {
-            color: #b5cea8;
-        }
-
-        .detail-line.url {
-            color: #4ec9b0;
-        }
-
-        .detail-line.stack {
-            color: #808080;
-            font-size: 0.72rem;
-        }
-
-        .detail-line.exception {
-            color: #f14c4c;
+        .detail-line.usuario { color: #9cdcfe; }
+        .detail-line.message { color: #ce9178; }
+        .detail-line.method { color: #dcdcaa; }
+        .detail-line.path { color: #4ec9b0; }
+        .detail-line.arquivo { color: #9cdcfe; }
+        .detail-line.metodo { color: #dcdcaa; }
+        .detail-line.linha { color: #b5cea8; }
+        .detail-line.url { color: #4ec9b0; }
+        .detail-line.stack { color: #808080; font-size: 0.72rem; }
+        .detail-line.exception { color: #f14c4c; }
+
+        /* ====== BOTÃO ANALISAR IA ====== */
+        .btn-analyze-ai {
+            background: linear-gradient(135deg, #7c3aed 0%, #5b21b6 100%);
+            color: #fff;
+            border: none;
+            padding: 0.35rem 0.75rem;
+            border-radius: 6px;
+            font-size: 0.75rem;
+            font-weight: 600;
+            cursor: pointer;
+            transition: all 0.2s ease;
+            display: inline-flex;
+            align-items: center;
+            gap: 0.35rem;
+            margin-left: auto;
+        }
+
+        .btn-analyze-ai:hover {
+            background: linear-gradient(135deg, #8b5cf6 0%, #6d28d9 100%);
+            transform: translateY(-1px);
+            box-shadow: 0 4px 12px rgba(124, 58, 237, 0.4);
+        }
+
+        .btn-analyze-ai:disabled {
+            opacity: 0.6;
+            cursor: not-allowed;
+            transform: none;
+        }
+
+        .btn-analyze-ai .spinner-border {
+            width: 12px;
+            height: 12px;
+            border-width: 2px;
+        }
+
+        /* ====== MODAL DE ANÁLISE IA ====== */
+        .ai-modal-overlay {
+            position: fixed;
+            top: 0;
+            left: 0;
+            right: 0;
+            bottom: 0;
+            background: rgba(0, 0, 0, 0.7);
+            z-index: 10000;
+            display: none;
+            align-items: center;
+            justify-content: center;
+            padding: 1rem;
+        }
+
+        .ai-modal-overlay.show {
+            display: flex;
+        }
+
+        .ai-modal {
+            background: #fff;
+            border-radius: 12px;
+            max-width: 900px;
+            width: 100%;
+            max-height: 90vh;
+            display: flex;
+            flex-direction: column;
+            box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.5);
+            animation: modalSlideIn 0.3s ease;
+        }
+
+        @@keyframes modalSlideIn {
+            from { opacity: 0; transform: translateY(-20px); }
+            to { opacity: 1; transform: translateY(0); }
+        }
+
+        .ai-modal-header {
+            background: linear-gradient(135deg, #7c3aed 0%, #5b21b6 100%);
+            color: #fff;
+            padding: 1rem 1.5rem;
+            border-radius: 12px 12px 0 0;
+            display: flex;
+            align-items: center;
+            justify-content: space-between;
+        }
+
+        .ai-modal-header h5 {
+            margin: 0;
+            font-weight: 700;
+            display: flex;
+            align-items: center;
+            gap: 0.5rem;
+        }
+
+        .ai-modal-close {
+            background: rgba(255, 255, 255, 0.2);
+            border: none;
+            color: #fff;
+            width: 32px;
+            height: 32px;
+            border-radius: 50%;
+            cursor: pointer;
+            display: flex;
+            align-items: center;
+            justify-content: center;
+            transition: background 0.2s;
+        }
+
+        .ai-modal-close:hover {
+            background: rgba(255, 255, 255, 0.3);
+        }
+
+        .ai-modal-body {
+            padding: 1.5rem;
+            overflow-y: auto;
+            flex: 1;
+        }
+
+        /* ====== CONTEÚDO DA ANÁLISE ====== */
+        .ai-analysis-result {
+            background: #f8fafc;
+            border-radius: 8px;
+            padding: 1rem;
+        }
+
+        .ai-analysis-header {
+            display: flex;
+            align-items: center;
+            gap: 0.75rem;
+            padding-bottom: 0.75rem;
+            border-bottom: 1px solid #e2e8f0;
+            margin-bottom: 1rem;
+        }
+
+        .ai-analysis-header i {
+            font-size: 1.5rem;
+            color: #7c3aed;
+        }
+
+        .ai-analysis-header .ai-title {
+            font-weight: 700;
+            color: #1e293b;
+        }
+
+        .ai-analysis-header .ai-meta {
+            font-size: 0.75rem;
+            color: #64748b;
+        }
+
+        .ai-analysis-content {
+            line-height: 1.6;
+            color: #334155;
+        }
+
+        .ai-analysis-content h1,
+        .ai-analysis-content h2,
+        .ai-analysis-content h3 {
+            color: #1e293b;
+            margin-top: 1rem;
+            margin-bottom: 0.5rem;
+            font-weight: 700;
+        }
+
+        .ai-analysis-content h1 { font-size: 1.25rem; }
+        .ai-analysis-content h2 { font-size: 1.1rem; }
+        .ai-analysis-content h3 { font-size: 1rem; }
+
+        .ai-analysis-content code {
+            background: #1e293b;
+            color: #e2e8f0;
+            padding: 0.15rem 0.4rem;
+            border-radius: 4px;
+            font-size: 0.85em;
+            font-family: 'Consolas', 'Monaco', 'Courier New', monospace;
+        }
+
+        .ai-analysis-content pre {
+            background: #0d1117 !important;
+            color: #c9d1d9 !important;
+            padding: 1rem;
+            border-radius: 8px;
+            overflow-x: auto;
+            margin: 0.75rem 0;
+            border: 1px solid #30363d;
+            font-family: 'Consolas', 'Monaco', 'Courier New', monospace;
+            font-size: 0.875rem;
+            line-height: 1.5;
+            max-height: 400px;
+            overflow-y: auto;
+        }
+
+        .ai-analysis-content pre code {
+            background: transparent !important;
+            padding: 0 !important;
+            color: inherit !important;
+        }
+
+        /* Container para pre com botão de copiar */
+        .ai-analysis-content .code-block-wrapper {
+            position: relative;
+            margin: 0.75rem 0;
+        }
+
+        .ai-analysis-content .code-block-wrapper pre {
+            margin: 0;
+        }
+
+        .ai-analysis-content .code-copy-btn {
+            position: absolute;
+            top: 8px;
+            right: 8px;
+            background: rgba(255, 255, 255, 0.1);
+            border: 1px solid rgba(255, 255, 255, 0.2);
+            color: #8b949e;
+            padding: 4px 8px;
+            border-radius: 4px;
+            font-size: 0.75rem;
+            cursor: pointer;
+            transition: all 0.2s ease;
+            display: flex;
+            align-items: center;
+            gap: 4px;
+            z-index: 10;
+        }
+
+        .ai-analysis-content .code-copy-btn:hover {
+            background: rgba(255, 255, 255, 0.2);
+            color: #fff;
+        }
+
+        .ai-analysis-content .code-copy-btn.copied {
+            background: #238636;
+            border-color: #238636;
+            color: #fff;
+        }
+
+        .ai-analysis-content .code-lang-badge {
+            position: absolute;
+            top: 8px;
+            left: 12px;
+            background: rgba(255, 255, 255, 0.1);
+            color: #8b949e;
+            padding: 2px 8px;
+            border-radius: 4px;
+            font-size: 0.7rem;
+            text-transform: uppercase;
+            font-weight: 600;
+        }
+
+        .ai-analysis-content ul, .ai-analysis-content ol {
+            padding-left: 1.5rem;
+            margin: 0.5rem 0;
+        }
+
+        .ai-analysis-content li {
+            margin-bottom: 0.25rem;
+        }
+
+        /* Highlight.js colors */
+        .ai-analysis-content .hljs-keyword { color: #ff7b72; }
+        .ai-analysis-content .hljs-string { color: #a5d6ff; }
+        .ai-analysis-content .hljs-comment { color: #8b949e; font-style: italic; }
+        .ai-analysis-content .hljs-function { color: #d2a8ff; }
+        .ai-analysis-content .hljs-number { color: #79c0ff; }
+        .ai-analysis-content .hljs-title { color: #d2a8ff; }
+        .ai-analysis-content .hljs-built_in { color: #ffa657; }
+
+        /* Loading state */
+        .ai-loading {
+            text-align: center;
+            padding: 3rem;
+        }
+
+        .ai-loading i {
+            font-size: 3rem;
+            color: #7c3aed;
+            margin-bottom: 1rem;
+        }
+
+        .ai-loading p {
+            color: #64748b;
+            margin: 0;
+        }
+
+        /* Error state */
+        .ai-error {
+            text-align: center;
+            padding: 2rem;
+            color: #dc3545;
+        }
+
+        .ai-error i {
+            font-size: 2.5rem;
+            margin-bottom: 0.75rem;
         }
 
         /* ====== RESPONSIVO ====== */
@@ -437,14 +651,12 @@
                             <div class="stat-value" id="statErrors">0</div>
                             <div class="stat-label">Erros</div>
                         </div>
-                        <div class="stat-card js-errors" data-filter="ERROR-JS"
-                            title="Clique para filtrar erros JavaScript">
+                        <div class="stat-card js-errors" data-filter="ERROR-JS" title="Clique para filtrar erros JavaScript">
                             <div class="stat-icon"><i class="fa-duotone fa-js"></i></div>
                             <div class="stat-value" id="statJsErrors">0</div>
                             <div class="stat-label">JS Errors</div>
                         </div>
-                        <div class="stat-card http-errors" data-filter="HTTP-ERROR"
-                            title="Clique para filtrar erros HTTP">
+                        <div class="stat-card http-errors" data-filter="HTTP-ERROR" title="Clique para filtrar erros HTTP">
                             <div class="stat-icon"><i class="fa-duotone fa-globe"></i></div>
                             <div class="stat-value" id="statHttpErrors">0</div>
                             <div class="stat-label">HTTP Errors</div>
@@ -459,13 +671,17 @@
                             <div class="stat-value" id="statInfo">0</div>
                             <div class="stat-label">Info</div>
                         </div>
+                        <div class="stat-card console-logs" data-filter="CONSOLE" title="Clique para filtrar logs do console do navegador" style="border-left-color: #9333ea;">
+                            <div class="stat-icon"><i class="fa-duotone fa-browser"></i></div>
+                            <div class="stat-value" id="statConsole" style="color: #9333ea;">0</div>
+                            <div class="stat-label">Console</div>
+                        </div>
                     </div>
 
                     <div class="filtros-container" id="filtrosContainer">
                         <div class="filtro-ativo-label">
                             <i class="fa-duotone fa-filter"></i> Filtro ativo: <span id="filtroAtivoNome">Todos</span>
-                            <button type="button" class="btn btn-sm btn-vinho ms-2" onclick="limparTodosFiltros()"
-                                title="Limpar filtro">
+                            <button type="button" class="btn btn-sm btn-vinho ms-2" onclick="limparTodosFiltros()" title="Limpar filtro">
                                 <i class="fa-duotone fa-xmark"></i>
                             </button>
                         </div>
@@ -481,16 +697,24 @@
                                     <option value="ERROR">Erros</option>
                                     <option value="ERROR-JS">JS Errors</option>
                                     <option value="HTTP-ERROR">HTTP Errors</option>
+                                    <option value="CONSOLE">Console</option>
                                     <option value="WARN">Avisos</option>
                                     <option value="INFO">Info</option>
                                 </select>
                             </div>
-                            <div class="col-md-4">
+                            <div class="col-md-2">
+                                <label class="form-label fw-bold">Origem</label>
+                                <select id="origemFiltro" class="form-select">
+                                    <option value="">Todas</option>
+                                    <option value="SERVER">🖥️ Servidor</option>
+                                    <option value="CLIENT">🌐 Cliente</option>
+                                </select>
+                            </div>
+                            <div class="col-md-3">
                                 <label class="form-label fw-bold">Buscar</label>
-                                <input type="text" id="buscaFiltro" class="form-control"
-                                    placeholder="Filtrar por texto..." />
+                                <input type="text" id="buscaFiltro" class="form-control" placeholder="Filtrar por texto..." />
                             </div>
-                            <div class="col-md-2">
+                            <div class="col-md-1">
                                 <button type="button" class="btn btn-azul w-100" onclick="aplicarFiltros()">
                                     <i class="fa-duotone fa-filter icon-space"></i> Filtrar
                                 </button>
@@ -518,62 +742,44 @@
 
 <div id="loadingOverlayLogErros" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text">Carregando Logs...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
     </div>
 </div>
 
+<div id="aiAnalysisModal" class="ai-modal-overlay">
+    <div class="ai-modal">
+        <div class="ai-modal-header">
+            <h5>
+                <i class="fa-duotone fa-sparkles"></i>
+                Análise com Claude AI
+            </h5>
+            <button type="button" class="ai-modal-close" onclick="fecharModalIA()">
+                <i class="fa-solid fa-xmark"></i>
+            </button>
+        </div>
+        <div class="ai-modal-body" id="aiAnalysisContent">
+
+        </div>
+    </div>
+</div>
+
 @section ScriptsBlock {
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * LOG DE ERROS - VISUALIZADOR DE LOGS DO SISTEMA
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Sistema de visualização e análise de logs de erros do FrotiX
-            * Permite filtrar por tipo, data e busca textual
-                */
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * VARIÁVEIS GLOBAIS
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /** @@type { Array } Logs parseados da API */
+
         var blocosParsed = [];
-            /** @@type { string } Data atual do filtro no formato BR */
         var dataLogAtual = '';
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES DE LOADING OVERLAY
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Exibe overlay de loading
-             * @@returns { void}
-             */
         function mostrarLoading() {
             $('#loadingOverlayLogErros').css('display', 'flex');
         }
 
-            /**
-             * Oculta overlay de loading
-             * @@returns { void}
-             */
         function esconderLoading() {
             $('#loadingOverlayLogErros').css('display', 'none');
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * INICIALIZAÇÃO DA PÁGINA
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
         $(document).ready(function () {
             try {
 
@@ -581,7 +787,7 @@
                 $('#dataFiltro').val(hoje);
                 dataLogAtual = formatarDataBR(hoje);
 
-                $('.stat-card').on('click', function () {
+                $('.stat-card').on('click', function() {
                     try {
                         var filtro = $(this).data('filter');
 
@@ -599,10 +805,11 @@
                 carregarLogs();
 
                 $('#tipoFiltro').on('change', aplicarFiltros);
-                $('#buscaFiltro').on('keypress', function (e) {
+                $('#origemFiltro').on('change', aplicarFiltros);
+                $('#buscaFiltro').on('keypress', function(e) {
                     if (e.which === 13) aplicarFiltros();
                 });
-                $('#dataFiltro').on('change', function () {
+                $('#dataFiltro').on('change', function() {
                     dataLogAtual = formatarDataBR($(this).val());
                     carregarLogs();
                 });
@@ -612,111 +819,90 @@
             }
         });
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES UTILITÁRIAS
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Formata data ISO para formato brasileiro DD/MM/YYYY
-             * @@param { string } dataISO - Data no formato YYYY - MM - DD
-            * @@returns { string } Data no formato DD / MM / YYYY
-                */
         function formatarDataBR(dataISO) {
             if (!dataISO) return '';
             var partes = dataISO.split('-');
             return partes[2] + '/' + partes[1] + '/' + partes[0];
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CARREGAMENTO E EXIBIÇÃO DE LOGS
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Carrega logs do servidor via API
-             * @@description Usa fetch nativo para evitar interceptadores do jQuery
-            * @@returns { void}
-            */
-            function carregarLogs() {
-                try {
-                    mostrarLoading();
-                    $('#logsContainer').html('<div class="text-center text-muted py-5"><i class="fa-duotone fa-spinner fa-spin fa-2x mb-3"></i><p>Carregando logs...</p></div>');
-
-                    fetch('/api/LogErros/ObterLogs', {
-                        method: 'GET',
-                        headers: {
-                            'Accept': 'application/json',
-                            'X-Requested-With': 'XMLHttpRequest'
+        function carregarLogs() {
+            try {
+                mostrarLoading();
+                $('#logsContainer').html('<div class="text-center text-muted py-5"><i class="fa-duotone fa-spinner fa-spin fa-2x mb-3"></i><p>Carregando logs...</p></div>');
+
+                fetch('/api/LogErros/ObterLogs', {
+                    method: 'GET',
+                    headers: {
+                        'Accept': 'application/json',
+                        'X-Requested-With': 'XMLHttpRequest'
+                    }
+                })
+                .then(function(response) {
+                    console.log('[LogErros] Response status:', response.status);
+                    console.log('[LogErros] Response headers:', response.headers.get('content-type'));
+
+                    return response.text();
+                })
+                .then(function(textoResposta) {
+                    console.log('[LogErros] Resposta bruta (primeiros 200 chars):', textoResposta.substring(0, 200));
+
+                    var res;
+                    try {
+                        res = JSON.parse(textoResposta);
+                    } catch (parseErr) {
+                        console.error('[LogErros] Erro ao parsear JSON:', parseErr);
+                        console.error('[LogErros] Texto completo:', textoResposta);
+                        $('#logsContainer').html('<div class="text-danger p-3">Erro ao processar resposta JSON</div>');
+                        esconderLoading();
+                        return;
+                    }
+
+                    console.log('[LogErros] JSON parseado com sucesso');
+
+                    if (res && res.success) {
+
+                        if (res.stats) {
+                            $('#statTotal').text(res.stats.TotalLogs || res.stats.totalLogs || 0);
+                            $('#statErrors').text(res.stats.ErrorCount || res.stats.errorCount || 0);
+                            $('#statJsErrors').text(res.stats.JSErrorCount || res.stats.jsErrorCount || 0);
+                            $('#statHttpErrors').text(res.stats.HttpErrorCount || res.stats.httpErrorCount || 0);
+                            $('#statWarnings').text(res.stats.WarningCount || res.stats.warningCount || 0);
+                            $('#statInfo').text(res.stats.InfoCount || res.stats.infoCount || 0);
+                            $('#statConsole').text(res.stats.ConsoleCount || res.stats.consoleCount || 0);
                         }
-                    })
-                        .then(function (response) {
-                            console.log('[LogErros] Response status:', response.status);
-                            console.log('[LogErros] Response headers:', response.headers.get('content-type'));
-
-                            return response.text();
-                        })
-                        .then(function (textoResposta) {
-                            console.log('[LogErros] Resposta bruta (primeiros 200 chars):', textoResposta.substring(0, 200));
-
-                            var res;
-                            try {
-                                res = JSON.parse(textoResposta);
-                            } catch (parseErr) {
-                                console.error('[LogErros] Erro ao parsear JSON:', parseErr);
-                                console.error('[LogErros] Texto completo:', textoResposta);
-                                $('#logsContainer').html('<div class="text-danger p-3">Erro ao processar resposta JSON</div>');
-                                esconderLoading();
-                                return;
-                            }
-
-                            console.log('[LogErros] JSON parseado com sucesso');
-
-                            if (res && res.success) {
-
-                                if (res.stats) {
-                                    $('#statTotal').text(res.stats.TotalLogs || res.stats.totalLogs || 0);
-                                    $('#statErrors').text(res.stats.ErrorCount || res.stats.errorCount || 0);
-                                    $('#statJsErrors').text(res.stats.JSErrorCount || res.stats.jsErrorCount || 0);
-                                    $('#statHttpErrors').text(res.stats.HttpErrorCount || res.stats.httpErrorCount || 0);
-                                    $('#statWarnings').text(res.stats.WarningCount || res.stats.warningCount || 0);
-                                    $('#statInfo').text(res.stats.InfoCount || res.stats.infoCount || 0);
-                                }
-
-                                var logsTexto = res.logs || '';
-                                console.log('[LogErros] Logs recebidos, tamanho:', logsTexto.length);
-
-                                blocosParsed = parsearLogsEmBlocos(logsTexto);
-
-                                console.log('[LogErros] Blocos parseados:', blocosParsed.length);
-
-                                aplicarFiltros();
-                                esconderLoading();
-                            } else {
-                                console.error('[LogErros] Resposta não sucesso:', res);
-                                $('#logsContainer').html('<div class="text-warning p-3">Erro ao carregar logs: ' + (res && res.error ? res.error : 'desconhecido') + '</div>');
-                                esconderLoading();
-                            }
-                        })
-                        .catch(function (error) {
-                            console.error('[LogErros] Erro fetch:', error);
-                            $('#logsContainer').html('<div class="text-danger p-3">Erro ao conectar: ' + error.message + '</div>');
-                            esconderLoading();
-                        });
-
-                } catch (error) {
-                    console.error('[LogErros] Erro geral:', error);
-                    Alerta.TratamentoErroComLinha("LogErros.cshtml", "carregarLogs", error);
+
+                        var logsTexto = res.logs || '';
+                        console.log('[LogErros] Logs recebidos, tamanho:', logsTexto.length);
+
+                        blocosParsed = parsearLogsEmBlocos(logsTexto);
+
+                        console.log('[LogErros] Blocos parseados:', blocosParsed.length);
+
+                        aplicarFiltros();
+                        esconderLoading();
+                    } else {
+                        console.error('[LogErros] Resposta não sucesso:', res);
+                        $('#logsContainer').html('<div class="text-warning p-3">Erro ao carregar logs: ' + (res && res.error ? res.error : 'desconhecido') + '</div>');
+                        esconderLoading();
+                    }
+                })
+                .catch(function(error) {
+                    console.error('[LogErros] Erro fetch:', error);
+                    $('#logsContainer').html('<div class="text-danger p-3">Erro ao conectar: ' + error.message + '</div>');
                     esconderLoading();
-                }
-            }
-
-            /**
-             * Parseia o texto de logs em blocos estruturados
-             */
-            function parsearLogsEmBlocos(logsTexto) {
+                });
+
+            } catch (error) {
+                console.error('[LogErros] Erro geral:', error);
+                Alerta.TratamentoErroComLinha("LogErros.cshtml", "carregarLogs", error);
+                esconderLoading();
+            }
+        }
+
+        /**
+         * Parseia o texto de logs em blocos estruturados
+         */
+        function parsearLogsEmBlocos(logsTexto) {
             try {
                 console.log('[LogErros] Iniciando parsing, tamanho:', logsTexto ? logsTexto.length : 0);
 
@@ -783,19 +969,20 @@
             try {
                 var tipoFiltro = $('#tipoFiltro').val() || '';
                 var buscaFiltro = ($('#buscaFiltro').val() || '').toLowerCase().trim();
-
-                console.log('[LogErros] Aplicando filtros - tipo:', tipoFiltro, 'busca:', buscaFiltro);
+                var origemFiltro = $('#origemFiltro').val() || '';
+
+                console.log('[LogErros] Aplicando filtros - tipo:', tipoFiltro, 'busca:', buscaFiltro, 'origem:', origemFiltro);
                 console.log('[LogErros] Total blocos para filtrar:', blocosParsed.length);
 
                 atualizarVisualFiltro(tipoFiltro, buscaFiltro);
 
-                if (!tipoFiltro && !buscaFiltro) {
+                if (!tipoFiltro && !buscaFiltro && !origemFiltro) {
                     console.log('[LogErros] Sem filtros, renderizando todos');
                     renderizarBlocos(blocosParsed);
                     return;
                 }
 
-                var blocosFiltrados = blocosParsed.filter(function (bloco) {
+                var blocosFiltrados = blocosParsed.filter(function(bloco) {
 
                     var passaTipo = true;
                     if (tipoFiltro) {
@@ -803,9 +990,17 @@
                             passaTipo = (bloco.tipo === 'ERROR' || bloco.tipo === 'OPERATION-FAIL');
                         } else if (tipoFiltro === 'INFO') {
                             passaTipo = (bloco.tipo === 'INFO' || bloco.tipo === 'USER' || bloco.tipo === 'OPERATION' || bloco.tipo === 'DEBUG');
+                        } else if (tipoFiltro === 'CONSOLE') {
+                            passaTipo = (bloco.tipo && bloco.tipo.startsWith('CONSOLE-'));
                         } else {
                             passaTipo = (bloco.tipo === tipoFiltro);
                         }
+                    }
+
+                    var passaOrigem = true;
+                    if (origemFiltro) {
+                        var origem = detectarOrigem(bloco.tipo, bloco.resumo);
+                        passaOrigem = (origem === origemFiltro);
                     }
 
                     var passaBusca = true;
@@ -814,7 +1009,7 @@
                         passaBusca = textoCompleto.toLowerCase().indexOf(buscaFiltro) >= 0;
                     }
 
-                    return passaTipo && passaBusca;
+                    return passaTipo && passaOrigem && passaBusca;
                 });
 
                 console.log('[LogErros] Blocos após filtro:', blocosFiltrados.length);
@@ -869,11 +1064,15 @@
 
                 var html = '';
 
-                blocos.forEach(function (bloco, index) {
+                blocos.forEach(function(bloco, index) {
                     var classeBloco = getClasseBloco(bloco.tipo);
                     var classeHeader = getClasseHeader(bloco.tipo);
-
-                    html += '<div class="log-block ' + classeBloco + '">';
+                    var origem = detectarOrigem(bloco.tipo, bloco.resumo);
+                    var badgeOrigem = getBadgeOrigem(origem);
+
+                    var podeAnalisar = ['ERROR', 'ERROR-JS', 'HTTP-ERROR', 'OPERATION-FAIL', 'WARN'].includes(bloco.tipo);
+
+                    html += '<div class="log-block ' + classeBloco + '" data-index="' + index + '">';
 
                     html += '<div class="log-header ' + classeHeader + '">';
                     html += '<div class="log-datetime">';
@@ -882,14 +1081,21 @@
                     html += '<i class="fa-duotone fa-clock"></i> ' + (bloco.hora || '--:--:--');
                     html += '</div>';
                     html += '<span class="log-type">' + (bloco.tipo || 'LOG') + '</span>';
+                    html += badgeOrigem;
                     if (bloco.resumo) {
                         html += '<span class="log-summary">' + escapeHtml(bloco.resumo) + '</span>';
                     }
+
+                    if (podeAnalisar) {
+                        html += '<button type="button" class="btn-analyze-ai" onclick="analisarComIA(' + index + ')" title="Analisar erro com Claude AI">';
+                        html += '<i class="fa-duotone fa-sparkles"></i> Analisar IA';
+                        html += '</button>';
+                    }
                     html += '</div>';
 
                     if (bloco.detalhes && bloco.detalhes.length > 0) {
                         html += '<div class="log-details">';
-                        bloco.detalhes.forEach(function (detalhe) {
+                        bloco.detalhes.forEach(function(detalhe) {
                             var classeDetalhe = getClasseDetalhe(detalhe);
                             html += '<div class="detail-line ' + classeDetalhe + '">' + escapeHtml(detalhe) + '</div>';
                         });
@@ -910,6 +1116,9 @@
         }
 
         function getClasseBloco(tipo) {
+
+            if (tipo && tipo.startsWith('CONSOLE-')) return 'console';
+
             var map = {
                 'ERROR': 'error',
                 'ERROR-JS': 'js-error',
@@ -925,6 +1134,9 @@
         }
 
         function getClasseHeader(tipo) {
+
+            if (tipo && tipo.startsWith('CONSOLE-')) return 'console';
+
             var map = {
                 'ERROR': 'error',
                 'ERROR-JS': 'js-error',
@@ -961,10 +1173,32 @@
                 .replace(/'/g, '&#039;');
         }
 
+        function detectarOrigem(tipo, resumo) {
+
+            if (!tipo) return 'SERVER';
+
+            if (tipo.startsWith('CONSOLE-')) return 'CLIENT';
+            if (tipo === 'ERROR-JS') return 'CLIENT';
+
+            if (resumo && resumo.includes('[🌐 CLIENT]')) return 'CLIENT';
+
+            return 'SERVER';
+        }
+
+        function getBadgeOrigem(origem) {
+
+            if (origem === 'CLIENT') {
+                return '<span class="log-origin-badge client">🌐 CLIENTE</span>';
+            } else {
+                return '<span class="log-origin-badge server">🖥️ SERVIDOR</span>';
+            }
+        }
+
         function limparTodosFiltros() {
             try {
                 $('#tipoFiltro').val('');
                 $('#buscaFiltro').val('');
+                $('#origemFiltro').val('');
                 $('.stat-card').removeClass('active');
                 $('.stat-card[data-filter=""]').addClass('active');
                 $('#filtrosContainer').removeClass('filtro-ativo');
@@ -1013,5 +1247,232 @@
                 Alerta.TratamentoErroComLinha("LogErros.cshtml", "limparLogs", error);
             }
         }
+
+        /**
+         * Abre o modal e inicia análise do log com Claude AI
+         */
+        function analisarComIA(index) {
+            try {
+                var bloco = blocosParsed[index];
+                if (!bloco) {
+                    AppToast.show('vermelho', 'Erro: bloco não encontrado', 3000);
+                    return;
+                }
+
+                $('#aiAnalysisModal').addClass('show');
+                $('#aiAnalysisContent').html(`
+                    <div class="ai-loading">
+                        <i class="fa-duotone fa-sparkles fa-spin"></i>
+                        <p>Analisando erro com Claude AI...</p>
+                        <p class="text-muted small mt-2">Isso pode levar alguns segundos</p>
+                    </div>
+                `);
+
+                var dadosErro = extrairDadosDoBloco(bloco);
+
+                fetch('/api/LogErros/AnalisarErroTexto', {
+                    method: 'POST',
+                    headers: {
+                        'Content-Type': 'application/json',
+                        'Accept': 'application/json'
+                    },
+                    body: JSON.stringify(dadosErro)
+                })
+                .then(response => response.json())
+                .then(data => {
+                    if (data.success) {
+                        renderizarAnaliseIA(data);
+                    } else {
+                        $('#aiAnalysisContent').html(`
+                            <div class="ai-error">
+                                <i class="fa-duotone fa-triangle-exclamation"></i>
+                                <p><strong>Erro na análise:</strong></p>
+                                <p>${escapeHtml(data.error || 'Erro desconhecido')}</p>
+                            </div>
+                        `);
+                    }
+                })
+                .catch(error => {
+                    console.error('[LogErros] Erro na análise IA:', error);
+                    $('#aiAnalysisContent').html(`
+                        <div class="ai-error">
+                            <i class="fa-duotone fa-triangle-exclamation"></i>
+                            <p><strong>Erro de conexão:</strong></p>
+                            <p>${escapeHtml(error.message)}</p>
+                        </div>
+                    `);
+                });
+
+            } catch (error) {
+                console.error('[LogErros] Erro ao analisar com IA:', error);
+                Alerta.TratamentoErroComLinha("LogErros.cshtml", "analisarComIA", error);
+            }
+        }
+
+        /**
+         * Extrai dados estruturados de um bloco de log
+         */
+        function extrairDadosDoBloco(bloco) {
+            var dados = {
+                tipo: bloco.tipo,
+                origem: detectarOrigem(bloco.tipo, bloco.resumo),
+                mensagem: bloco.resumo || '',
+                dataHora: dataLogAtual + ' ' + (bloco.hora || '00:00:00')
+            };
+
+            if (bloco.detalhes && bloco.detalhes.length > 0) {
+                bloco.detalhes.forEach(function(detalhe) {
+
+                    var matchArquivo = detalhe.match(/Arquivo:\s*(.+)/i) || detalhe.match(/📄\s*(.+)/);
+                    if (matchArquivo) dados.arquivo = matchArquivo[1].trim();
+
+                    var matchMetodo = detalhe.match(/Método:\s*(.+)/i) || detalhe.match(/Função:\s*(.+)/i) || detalhe.match(/🔧\s*(.+)/);
+                    if (matchMetodo) dados.metodo = matchMetodo[1].trim();
+
+                    var matchLinha = detalhe.match(/Linha:\s*(\d+)/i);
+                    if (matchLinha) dados.linha = parseInt(matchLinha[1]);
+
+                    var matchColuna = detalhe.match(/Coluna:\s*(\d+)/i);
+                    if (matchColuna) dados.coluna = parseInt(matchColuna[1]);
+
+                    var matchUrl = detalhe.match(/URL:\s*(.+)/i) || detalhe.match(/🌐\s*(.+)/);
+                    if (matchUrl) dados.url = matchUrl[1].trim();
+
+                    if (detalhe.includes('Stack') || detalhe.includes(' at ')) {
+                        dados.stack = (dados.stack || '') + detalhe + '\n';
+                    }
+
+                    var matchMsg = detalhe.match(/Message:\s*(.+)/i) || detalhe.match(/💬\s*(.+)/);
+                    if (matchMsg) {
+                        dados.mensagem = matchMsg[1].trim();
+                    }
+                });
+            }
+
+            return dados;
+        }
+
+        /**
+         * Renderiza o resultado da análise IA no modal
+         */
+        function renderizarAnaliseIA(data) {
+
+            if (typeof marked !== 'undefined') {
+                marked.setOptions({
+                    breaks: true,
+                    gfm: true,
+                    highlight: function(code, lang) {
+                        if (typeof hljs !== 'undefined') {
+                            const language = hljs.getLanguage(lang) ? lang : 'plaintext';
+                            try {
+                                return hljs.highlight(code, { language: language }).value;
+                            } catch (e) {
+                                return code;
+                            }
+                        }
+                        return code;
+                    }
+                });
+            }
+
+            var analysisHtml = '';
+            try {
+                if (typeof marked !== 'undefined') {
+                    if (typeof marked.parse === 'function') {
+                        analysisHtml = marked.parse(data.analysis || '');
+                    } else if (typeof marked === 'function') {
+                        analysisHtml = marked(data.analysis || '');
+                    } else {
+                        analysisHtml = (data.analysis || '').replace(/\n/g, '<br>');
+                    }
+                } else {
+                    analysisHtml = (data.analysis || '').replace(/\n/g, '<br>');
+                }
+            } catch (e) {
+                console.error('Erro ao processar markdown:', e);
+                analysisHtml = (data.analysis || '').replace(/\n/g, '<br>');
+            }
+
+            var dataFormatada = new Date(data.analyzedAt).toLocaleString('pt-BR');
+
+            var html = `
+                <div class="ai-analysis-result">
+                    <div class="ai-analysis-header">
+                        <i class="fa-duotone fa-sparkles"></i>
+                        <div>
+                            <div class="ai-title">Análise do Claude AI</div>
+                            <div class="ai-meta">Modelo: ${escapeHtml(data.model || 'N/A')} | Tokens: ${data.tokens?.input || 0} → ${data.tokens?.output || 0}</div>
+                        </div>
+                        <div class="ai-meta text-end">
+                            ${dataFormatada}
+                        </div>
+                    </div>
+                    <div class="ai-analysis-content">
+                        ${analysisHtml}
+                    </div>
+                </div>
+            `;
+
+            $('#aiAnalysisContent').html(html);
+
+            $('#aiAnalysisContent pre').each(function(i, pre) {
+                var $pre = $(pre);
+                var $code = $pre.find('code');
+
+                var lang = '';
+                if ($code.length) {
+                    var classMatch = ($code.attr('class') || '').match(/language-(\w+)/);
+                    lang = classMatch ? classMatch[1] : '';
+
+                    if (typeof hljs !== 'undefined') {
+                        hljs.highlightElement($code[0]);
+                    }
+                }
+
+                $pre.wrap('<div class="code-block-wrapper"></div>');
+                var $wrapper = $pre.parent();
+
+                if (lang) {
+                    $wrapper.prepend('<span class="code-lang-badge">' + lang + '</span>');
+                }
+
+                var $copyBtn = $('<button class="code-copy-btn" title="Copiar código"><i class="fa-regular fa-copy"></i> Copiar</button>');
+
+                $copyBtn.on('click', function() {
+                    var codeText = $code.length ? $code.text() : $pre.text();
+                    navigator.clipboard.writeText(codeText).then(function() {
+                        var $btn = $copyBtn;
+                        $btn.addClass('copied').html('<i class="fa-regular fa-check"></i> Copiado!');
+                        setTimeout(function() {
+                            $btn.removeClass('copied').html('<i class="fa-regular fa-copy"></i> Copiar');
+                        }, 2000);
+                    }).catch(function(err) {
+                        console.error('Erro ao copiar:', err);
+                    });
+                });
+
+                $wrapper.append($copyBtn);
+            });
+        }
+
+        /**
+         * Fecha o modal de análise IA
+         */
+        function fecharModalIA() {
+            $('#aiAnalysisModal').removeClass('show');
+            $('#aiAnalysisContent').html('');
+        }
+
+        $(document).on('click', '#aiAnalysisModal', function(e) {
+            if (e.target === this) {
+                fecharModalIA();
+            }
+        });
+
+        $(document).on('keydown', function(e) {
+            if (e.key === 'Escape' && $('#aiAnalysisModal').hasClass('show')) {
+                fecharModalIA();
+            }
+        });
     </script>
 }
```
