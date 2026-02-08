# Pages/Abastecimento/Importacao.cshtml

**Mudanca:** GRANDE | **+111** linhas | **-279** linhas

---

```diff
--- JANEIRO: Pages/Abastecimento/Importacao.cshtml
+++ ATUAL: Pages/Abastecimento/Importacao.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Abastecimentos.ImportarModel
 
 @{
@@ -87,8 +86,8 @@
             border-radius: 12px;
             padding: 1rem;
             text-align: center;
-            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
-            border: 1px solid rgba(0, 0, 0, 0.04);
+            box-shadow: 0 2px 8px rgba(0,0,0,0.06);
+            border: 1px solid rgba(0,0,0,0.04);
             transition: transform 0.2s ease;
         }
 
@@ -113,37 +112,17 @@
             font-weight: 600;
         }
 
-        .stat-card-ftx.stat-total .stat-icon {
-            color: #3D5771;
-        }
-
-        .stat-card-ftx.stat-total .stat-valor {
-            color: #3D5771;
-        }
-
-        .stat-card-ftx.stat-sucesso .stat-icon {
-            color: #22c55e;
-        }
-
-        .stat-card-ftx.stat-sucesso .stat-valor {
-            color: #22c55e;
-        }
-
-        .stat-card-ftx.stat-erro .stat-icon {
-            color: #ef4444;
-        }
-
-        .stat-card-ftx.stat-erro .stat-valor {
-            color: #ef4444;
-        }
-
-        .stat-card-ftx.stat-ignorado .stat-icon {
-            color: #6c757d;
-        }
-
-        .stat-card-ftx.stat-ignorado .stat-valor {
-            color: #6c757d;
-        }
+        .stat-card-ftx.stat-total .stat-icon { color: #3D5771; }
+        .stat-card-ftx.stat-total .stat-valor { color: #3D5771; }
+
+        .stat-card-ftx.stat-sucesso .stat-icon { color: #22c55e; }
+        .stat-card-ftx.stat-sucesso .stat-valor { color: #22c55e; }
+
+        .stat-card-ftx.stat-erro .stat-icon { color: #ef4444; }
+        .stat-card-ftx.stat-erro .stat-valor { color: #ef4444; }
+
+        .stat-card-ftx.stat-ignorado .stat-icon { color: #6c757d; }
+        .stat-card-ftx.stat-ignorado .stat-valor { color: #6c757d; }
 
         /* ===== ESTADOS ===== */
         .estado-aguardando {
@@ -403,13 +382,8 @@
         }
 
         /* Stat card para linhas corrigíveis */
-        .stat-card-ftx.stat-corrigivel .stat-icon {
-            color: #059669;
-        }
-
-        .stat-card-ftx.stat-corrigivel .stat-valor {
-            color: #059669;
-        }
+        .stat-card-ftx.stat-corrigivel .stat-icon { color: #059669; }
+        .stat-card-ftx.stat-corrigivel .stat-valor { color: #059669; }
 
         /* ===== TABELA IMPORTADOS ===== */
         .tabela-importados-container {
@@ -438,7 +412,7 @@
             background: #e2e8f0;
             border-radius: 6px;
             overflow: hidden;
-            box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.1);
+            box-shadow: inset 0 1px 3px rgba(0,0,0,0.1);
         }
 
         .ftx-progress-fill {
@@ -451,13 +425,8 @@
         }
 
         @@keyframes progressShine {
-            0% {
-                background-position: 200% 0;
-            }
-
-            100% {
-                background-position: -200% 0;
-            }
+            0% { background-position: 200% 0; }
+            100% { background-position: -200% 0; }
         }
 
         .ftx-progress-text {
@@ -490,7 +459,6 @@
                 opacity: 0;
                 transform: translateY(20px);
             }
-
             to {
                 opacity: 1;
                 transform: translateY(0);
@@ -584,7 +552,6 @@
         .resumo-stat-total {
             background: rgba(59, 130, 246, 0.1);
         }
-
         .resumo-stat-total .resumo-stat-valor {
             color: #3b82f6;
         }
@@ -592,7 +559,6 @@
         .resumo-stat-gasolina {
             background: rgba(34, 197, 94, 0.1);
         }
-
         .resumo-stat-gasolina .resumo-stat-valor {
             color: #22c55e;
         }
@@ -600,7 +566,6 @@
         .resumo-stat-diesel {
             background: rgba(245, 158, 11, 0.1);
         }
-
         .resumo-stat-diesel .resumo-stat-valor {
             color: #f59e0b;
         }
@@ -608,7 +573,6 @@
         .resumo-stat-outros {
             background: rgba(107, 114, 128, 0.1);
         }
-
         .resumo-stat-outros .resumo-stat-valor {
             color: #6b7280;
         }
@@ -648,28 +612,22 @@
                                                 Arquivo Excel
                                             </label>
                                             <div id="dropZoneXlsx" class="ftx-dropzone ftx-dropzone-small"
-                                                data-bs-toggle="tooltip" data-bs-custom-class="tooltip-ftx-azul"
-                                                data-bs-placement="top" title="Nenhum arquivo escolhido">
-                                                <input type="file" id="arquivoXlsx" accept=".xlsx,.xls"
-                                                    class="ftx-dropzone-input" />
+                                                 data-bs-toggle="tooltip"
+                                                 data-bs-custom-class="tooltip-ftx-azul"
+                                                 data-bs-placement="top"
+                                                 title="Nenhum arquivo escolhido">
+                                                <input type="file" id="arquivoXlsx" accept=".xlsx,.xls" class="ftx-dropzone-input" />
                                                 <div id="dropZoneXlsxContent">
                                                     <i class="fa-duotone fa-cloud-arrow-up ftx-dropzone-icon-small"></i>
                                                     <p class="mb-1 fw-semibold small">Arraste aqui</p>
-                                                    <p class="text-muted small mb-2" style="font-size: 0.7rem;">ou
-                                                        clique</p>
-                                                    <span class="badge bg-light text-dark border"
-                                                        style="font-size: 0.65rem;">.xlsx ou .xls</span>
-                                                    <p class="text-muted mt-2 mb-0" style="font-size: 0.65rem;">
-                                                        <strong>Contém:</strong> Data/Hora
-                                                    </p>
+                                                    <p class="text-muted small mb-2" style="font-size: 0.7rem;">ou clique</p>
+                                                    <span class="badge bg-light text-dark border" style="font-size: 0.65rem;">.xlsx ou .xls</span>
+                                                    <p class="text-muted mt-2 mb-0" style="font-size: 0.65rem;"><strong>Contém:</strong> Data/Hora</p>
                                                 </div>
                                                 <div id="arquivoXlsxSelecionado" class="d-none">
-                                                    <i
-                                                        class="fa-duotone fa-file-excel ftx-dropzone-icon-small text-success"></i>
-                                                    <p class="mb-1 fw-semibold text-success small" id="nomeArquivoXlsx">
-                                                    </p>
-                                                    <button type="button" class="btn btn-sm btn-vinho mt-2"
-                                                        id="btnRemoverXlsx">
+                                                    <i class="fa-duotone fa-file-excel ftx-dropzone-icon-small text-success"></i>
+                                                    <p class="mb-1 fw-semibold text-success small" id="nomeArquivoXlsx"></p>
+                                                    <button type="button" class="btn btn-sm btn-vinho mt-2" id="btnRemoverXlsx">
                                                         <i class="fa-duotone fa-trash-can me-1"></i>Remover
                                                     </button>
                                                 </div>
@@ -682,28 +640,22 @@
                                                 Arquivo CSV
                                             </label>
                                             <div id="dropZoneCsv" class="ftx-dropzone ftx-dropzone-small"
-                                                data-bs-toggle="tooltip" data-bs-custom-class="tooltip-ftx-azul"
-                                                data-bs-placement="top" title="Nenhum arquivo escolhido">
-                                                <input type="file" id="arquivoCsv" accept=".csv"
-                                                    class="ftx-dropzone-input" />
+                                                 data-bs-toggle="tooltip"
+                                                 data-bs-custom-class="tooltip-ftx-azul"
+                                                 data-bs-placement="top"
+                                                 title="Nenhum arquivo escolhido">
+                                                <input type="file" id="arquivoCsv" accept=".csv" class="ftx-dropzone-input" />
                                                 <div id="dropZoneCsvContent">
                                                     <i class="fa-duotone fa-cloud-arrow-up ftx-dropzone-icon-small"></i>
                                                     <p class="mb-1 fw-semibold small">Arraste aqui</p>
-                                                    <p class="text-muted small mb-2" style="font-size: 0.7rem;">ou
-                                                        clique</p>
-                                                    <span class="badge bg-light text-dark border"
-                                                        style="font-size: 0.65rem;">.csv</span>
-                                                    <p class="text-muted mt-2 mb-0" style="font-size: 0.65rem;">
-                                                        <strong>Contém:</strong> Placa, KM, Litros...
-                                                    </p>
+                                                    <p class="text-muted small mb-2" style="font-size: 0.7rem;">ou clique</p>
+                                                    <span class="badge bg-light text-dark border" style="font-size: 0.65rem;">.csv</span>
+                                                    <p class="text-muted mt-2 mb-0" style="font-size: 0.65rem;"><strong>Contém:</strong> Placa, KM, Litros...</p>
                                                 </div>
                                                 <div id="arquivoCsvSelecionado" class="d-none">
-                                                    <i
-                                                        class="fa-duotone fa-file-csv ftx-dropzone-icon-small text-warning"></i>
-                                                    <p class="mb-1 fw-semibold text-warning small" id="nomeArquivoCsv">
-                                                    </p>
-                                                    <button type="button" class="btn btn-sm btn-vinho mt-2"
-                                                        id="btnRemoverCsv">
+                                                    <i class="fa-duotone fa-file-csv ftx-dropzone-icon-small text-warning"></i>
+                                                    <p class="mb-1 fw-semibold text-warning small" id="nomeArquivoCsv"></p>
+                                                    <button type="button" class="btn btn-sm btn-vinho mt-2" id="btnRemoverCsv">
                                                         <i class="fa-duotone fa-trash-can me-1"></i>Remover
                                                     </button>
                                                 </div>
@@ -711,8 +663,7 @@
                                         </div>
                                     </div>
 
-                                    <button type="button" id="btnImportar" class="btn btn-fundo-laranja btn-lg w-100"
-                                        disabled>
+                                    <button type="button" id="btnImportar" class="btn btn-fundo-laranja btn-lg w-100" disabled>
                                         <i class="fa-duotone fa-upload me-2"></i>
                                         Importar Abastecimentos
                                     </button>
@@ -724,21 +675,17 @@
                                         <hr class="my-2">
                                         <p class="small mb-2"><strong>📄 Arquivo XLSX (Excel):</strong></p>
                                         <ul class="small mb-3">
-                                            <li>Colunas: <strong>Data</strong> (com hora), <strong>Autorizacao</strong>
-                                            </li>
+                                            <li>Colunas: <strong>Data</strong> (com hora), <strong>Autorizacao</strong></li>
                                             <li>A data/hora deve estar no formato correto do Excel</li>
                                         </ul>
                                         <p class="small mb-2"><strong>📄 Arquivo CSV:</strong></p>
                                         <ul class="small mb-3">
-                                            <li>Colunas: Autorizacao, Placa, KM, Produto, Qtde, VrUnitario, Rodado,
-                                                CodMotorista, KMAnterior</li>
-                                            <li>Produtos aceitos: <strong>Gasolina Comum</strong> e <strong>Diesel
-                                                    S-10</strong></li>
+                                            <li>Colunas: Autorizacao, Placa, KM, Produto, Qtde, VrUnitario, Rodado, CodMotorista, KMAnterior</li>
+                                            <li>Produtos aceitos: <strong>Gasolina Comum</strong> e <strong>Diesel S-10</strong></li>
                                         </ul>
                                         <p class="small mb-2"><strong>✅ Regras:</strong></p>
                                         <ul class="small">
-                                            <li>Os arquivos são combinados pelo número de <strong>Autorização</strong>
-                                            </li>
+                                            <li>Os arquivos são combinados pelo número de <strong>Autorização</strong></li>
                                             <li>Outros produtos (ARLA, etc) serão ignorados</li>
                                             <li>Limite: 500 litros por abastecimento</li>
                                             <li>Limite: 1.000 km rodados</li>
@@ -774,8 +721,7 @@
 
                                         <div class="ftx-progress-container">
                                             <div class="ftx-progress-bar">
-                                                <div class="ftx-progress-fill" id="progressFill" style="width: 0%">
-                                                </div>
+                                                <div class="ftx-progress-fill" id="progressFill" style="width: 0%"></div>
                                             </div>
                                             <div class="ftx-progress-text" id="progressText">0%</div>
                                         </div>
@@ -789,8 +735,7 @@
                                                 <div class="resumo-body">
                                                     <div class="resumo-periodo">
                                                         <div class="resumo-periodo-item">
-                                                            <i
-                                                                class="fa-duotone fa-calendar-arrow-down text-success"></i>
+                                                            <i class="fa-duotone fa-calendar-arrow-down text-success"></i>
                                                             <div>
                                                                 <small>Data Inicial</small>
                                                                 <strong id="resumoDataInicial">--/--/----</strong>
@@ -831,8 +776,7 @@
                                     </div>
 
                                     <div id="estadoSucesso" class="d-none">
-                                        <div
-                                            class="alert alert-resultado alert-success d-flex align-items-center gap-3 mb-4">
+                                        <div class="alert alert-resultado alert-success d-flex align-items-center gap-3 mb-4">
                                             <div class="alert-icon">
                                                 <i class="fa-duotone fa-circle-check"></i>
                                             </div>
@@ -885,8 +829,7 @@
                                     </div>
 
                                     <div id="estadoParcial" class="d-none">
-                                        <div
-                                            class="alert alert-resultado alert-warning d-flex align-items-center gap-3 mb-4">
+                                        <div class="alert alert-resultado alert-warning d-flex align-items-center gap-3 mb-4">
                                             <div class="alert-icon">
                                                 <i class="fa-duotone fa-triangle-exclamation"></i>
                                             </div>
@@ -929,15 +872,13 @@
 
                                         <div class="row g-3 mb-4">
                                             <div class="col-md-7">
-                                                <button onclick="window.location.href='/Abastecimento/Pendencias'"
-                                                    class="btn btn-azul btn-lg w-100">
+                                                <button onclick="window.location.href='/Abastecimento/Pendencias'" class="btn btn-azul btn-lg w-100">
                                                     <i class="fa-duotone fa-clipboard-list-check me-2"></i>
                                                     Acessar Pendências para Correção
                                                 </button>
                                             </div>
                                             <div class="col-md-5">
-                                                <button onclick="exportarPendencias()"
-                                                    class="btn btn-verde-dinheiro btn-lg w-100">
+                                                <button onclick="exportarPendencias()" class="btn btn-verde-dinheiro btn-lg w-100">
                                                     <i class="fa-duotone fa-file-excel me-2"></i>
                                                     Exportar Excel
                                                 </button>
@@ -952,8 +893,7 @@
                                     </div>
 
                                     <div id="estadoErro" class="d-none">
-                                        <div
-                                            class="alert alert-resultado alert-danger d-flex align-items-center gap-3 mb-4">
+                                        <div class="alert alert-resultado alert-danger d-flex align-items-center gap-3 mb-4">
                                             <div class="alert-icon">
                                                 <i class="fa-duotone fa-circle-xmark"></i>
                                             </div>
@@ -989,15 +929,13 @@
 
                                         <div class="row g-3 mb-4">
                                             <div class="col-md-7">
-                                                <button onclick="window.location.href='/Abastecimento/Pendencias'"
-                                                    class="btn btn-azul btn-lg w-100">
+                                                <button onclick="window.location.href='/Abastecimento/Pendencias'" class="btn btn-azul btn-lg w-100">
                                                     <i class="fa-duotone fa-clipboard-list-check me-2"></i>
                                                     Acessar Pendências para Correção
                                                 </button>
                                             </div>
                                             <div class="col-md-5">
-                                                <button onclick="exportarPendencias()"
-                                                    class="btn btn-verde-dinheiro btn-lg w-100">
+                                                <button onclick="exportarPendencias()" class="btn btn-verde-dinheiro btn-lg w-100">
                                                     <i class="fa-duotone fa-file-excel me-2"></i>
                                                     Exportar Excel
                                                 </button>
@@ -1028,17 +966,6 @@
         (function () {
             'use strict';
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * IMPORTAÇÃO DE ABASTECIMENTOS - SISTEMA DE UPLOAD EM LOTE
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Sistema de importação de planilhas Excel/CSV com validação
-             * em tempo real via SignalR. Suporta correção de erros in-line
-             * e exportação de pendências.
-             * @@requires SignalR, Bootstrap 5, AppToast, Alerta
-             * @@file Abastecimento/Importacao.cshtml
-             */
-
             const arquivoXlsx = document.getElementById('arquivoXlsx');
             const dropZoneXlsx = document.getElementById('dropZoneXlsx');
             const dropZoneXlsxContent = document.getElementById('dropZoneXlsxContent');
@@ -1077,17 +1004,9 @@
             let arquivoXlsxAtual = null;
             let arquivoCsvAtual = null;
 
-            /** @@type {Object|null} Conexão do hub SignalR */
             let hubConnection = null;
-            /** @@type {string|null} ID da conexão para envio de progresso */
             let connectionId = null;
 
-            /**
-             * Inicializa conexão SignalR para receber progresso em tempo real
-             * @@async
-             * @@returns {Promise<void>}
-             * @@description Estabelece conexão com hub de importação e registra handlers de eventos
-             */
             async function inicializarSignalR() {
                 try {
                     console.log("🔧 Inicializando SignalR...");
@@ -1152,13 +1071,6 @@
                 }
             }
 
-            /**
-             * Exibe card com resumo estatístico da planilha importada
-             * @@param {Object} progresso - Dados do progresso com resumo
-             * @@param {string} progresso.dataInicial - Data inicial dos registros
-             * @@param {string} progresso.dataFinal - Data final dos registros
-             * @@param {number} progresso.totalRegistros - Total de linhas
-             */
             function mostrarResumoPlanilha(progresso) {
                 try {
                     resumoDataInicial.textContent = progresso.dataInicial || '--/--/----';
@@ -1173,10 +1085,6 @@
                 }
             }
 
-            /**
-             * Oculta card de resumo da planilha
-             * @@returns {void}
-             */
             function ocultarResumoPlanilha() {
                 try {
                     resumoPlanilha.classList.add('d-none');
@@ -1185,11 +1093,6 @@
                 }
             }
 
-            /**
-             * Inicialização principal da página
-             * @@async
-             * @@description Conecta SignalR e configura todos os event listeners
-             */
             async function init() {
                 try {
 
@@ -1200,10 +1103,6 @@
                 }
             }
 
-            /**
-             * Configura todos os event listeners da página
-             * @@description Registra eventos de drag-drop, click e change nos elementos
-             */
             function configurarEventos() {
                 try {
 
@@ -1270,12 +1169,6 @@
                 }
             }
 
-            /**
-             * Cria elemento HTML para exibição de erro de importação
-             * @@param {Object} erro - Dados do erro
-             * @@param {number} index - Índice do erro na lista
-             * @@returns {HTMLElement} Elemento DOM com card de erro
-             */
             function criarItemErro(erro, index) {
                 try {
                     const item = document.createElement('div');
@@ -1286,64 +1179,64 @@
                     const badgeText = erro.corrigivel ? 'sugestão' : erro.tipo;
 
                     let html = `
-                                <div class="d-flex align-items-start gap-3">
-                                    <div class="erro-icon-circle">
-                                        <i class="fa-solid ${erro.icone}"></i>
+                        <div class="d-flex align-items-start gap-3">
+                            <div class="erro-icon-circle">
+                                <i class="fa-solid ${erro.icone}"></i>
+                            </div>
+                            <div class="flex-grow-1">
+                                <div class="d-flex justify-content-between align-items-center mb-1">
+                                    <strong class="text-danger">Linha ${erro.linhaArquivoErros} (original: ${erro.linhaOriginal})</strong>
+                                    <div class="d-flex gap-2">
+                                        <span class="badge-tipo-erro badge-erro">${erro.tipo}</span>
+                                        ${erro.corrigivel ? '<span class="badge-tipo-erro badge-corrigivel"><i class="fa-duotone fa-wand-magic-sparkles me-1"></i>sugestão</span>' : ''}
                                     </div>
-                                    <div class="flex-grow-1">
-                                        <div class="d-flex justify-content-between align-items-center mb-1">
-                                            <strong class="text-danger">Linha ${erro.linhaArquivoErros} (original: ${erro.linhaOriginal})</strong>
-                                            <div class="d-flex gap-2">
-                                                <span class="badge-tipo-erro badge-erro">${erro.tipo}</span>
-                                                ${erro.corrigivel ? '<span class="badge-tipo-erro badge-corrigivel"><i class="fa-duotone fa-wand-magic-sparkles me-1"></i>sugestão</span>' : ''}
-                                            </div>
-                                        </div>
-                                        <p class="mb-0 text-muted small">${erro.descricao}</p>
-                                        <div class="mt-1">
-                                            <small class="text-muted">
-                                                <strong>Placa:</strong> ${erro.placa || '-'} |
-                                                <strong>Aut:</strong> ${erro.autorizacao || '-'} |
-                                                <strong>KM Ant:</strong> ${erro.kmAnterior?.toLocaleString('pt-BR') || '-'} |
-                                                <strong>KM:</strong> ${erro.km?.toLocaleString('pt-BR') || '-'}
-                                            </small>
-                                        </div>
-                            `;
+                                </div>
+                                <p class="mb-0 text-muted small">${erro.descricao}</p>
+                                <div class="mt-1">
+                                    <small class="text-muted">
+                                        <strong>Placa:</strong> ${erro.placa || '-'} |
+                                        <strong>Aut:</strong> ${erro.autorizacao || '-'} |
+                                        <strong>KM Ant:</strong> ${erro.kmAnterior?.toLocaleString('pt-BR') || '-'} |
+                                        <strong>KM:</strong> ${erro.km?.toLocaleString('pt-BR') || '-'}
+                                    </small>
+                                </div>
+                    `;
 
                     if (erro.corrigivel) {
                         const campoLabel = erro.campoCorrecao === 'KmAnterior' ? 'KM Anterior' : 'KM Atual';
 
                         html += `
-                                        <div class="sugestao-correcao">
-                                            <div class="sugestao-header">
-                                                <i class="fa-duotone fa-lightbulb-on"></i>
-                                                <span>Sugestão de Correção: ${campoLabel}</span>
-                                            </div>
-                                            <div class="sugestao-valores">
-                                                <div class="sugestao-valor valor-errado">
-                                                    <span class="valor-label">Atual (errado)</span>
-                                                    <span class="valor-numero">${erro.valorAtual?.toLocaleString('pt-BR')}</span>
-                                                </div>
-                                                <i class="fa-duotone fa-arrow-right sugestao-seta"></i>
-                                                <div class="sugestao-valor valor-correto">
-                                                    <span class="valor-label">Sugerido</span>
-                                                    <span class="valor-numero">${erro.valorSugerido?.toLocaleString('pt-BR')}</span>
-                                                </div>
-                                            </div>
-                                            <div class="sugestao-justificativa">
-                                                ${erro.justificativaSugestao}
-                                            </div>
-                                            <button type="button" class="btn-aplicar-correcao" onclick="aplicarCorrecao(${index}, this)">
-                                                <i class="fa-duotone fa-check"></i>
-                                                Aplicar e Importar
-                                            </button>
+                                <div class="sugestao-correcao">
+                                    <div class="sugestao-header">
+                                        <i class="fa-duotone fa-lightbulb-on"></i>
+                                        <span>Sugestão de Correção: ${campoLabel}</span>
+                                    </div>
+                                    <div class="sugestao-valores">
+                                        <div class="sugestao-valor valor-errado">
+                                            <span class="valor-label">Atual (errado)</span>
+                                            <span class="valor-numero">${erro.valorAtual?.toLocaleString('pt-BR')}</span>
                                         </div>
-                                `;
+                                        <i class="fa-duotone fa-arrow-right sugestao-seta"></i>
+                                        <div class="sugestao-valor valor-correto">
+                                            <span class="valor-label">Sugerido</span>
+                                            <span class="valor-numero">${erro.valorSugerido?.toLocaleString('pt-BR')}</span>
+                                        </div>
+                                    </div>
+                                    <div class="sugestao-justificativa">
+                                        ${erro.justificativaSugestao}
+                                    </div>
+                                    <button type="button" class="btn-aplicar-correcao" onclick="aplicarCorrecao(${index}, this)">
+                                        <i class="fa-duotone fa-check"></i>
+                                        Aplicar e Importar
+                                    </button>
+                                </div>
+                        `;
                     }
 
                     html += `
-                                    </div>
-                                </div>
-                            `;
+                            </div>
+                        </div>
+                    `;
 
                     item.innerHTML = html;
                     return item;
@@ -1356,14 +1249,6 @@
 
             let errosAtuais = [];
 
-            /**
-             * Aplica correção de um registro com erro
-             * @@async
-             * @@param {number} index - Índice do erro na lista
-             * @@param {HTMLElement} btnElement - Botão que disparou a ação
-             * @@returns {Promise<void>}
-             * @@description Envia dados corrigidos para API e atualiza interface
-             */
             async function aplicarCorrecao(index, btnElement) {
                 try {
                     const erro = errosAtuais[index];
@@ -1421,10 +1306,6 @@
                 }
             }
 
-            /**
-             * Atualiza contadores de sucesso/erro após correção
-             * @@description Incrementa importados e decrementa erros nos estados parcial/erro
-             */
             function atualizarContadoresAposCorrecao() {
                 try {
 
@@ -1450,12 +1331,6 @@
 
             window.aplicarCorrecao = aplicarCorrecao;
 
-            /**
-             * Atualiza barra de progresso e textos de etapa
-             * @@param {number} pct - Percentual de conclusão (0-100)
-             * @@param {string} etapa - Título da etapa atual
-             * @@param {string} detalhe - Detalhe adicional
-             */
             function atualizarProgresso(pct, etapa, detalhe) {
                 try {
                     const porcentagem = Math.min(Math.round(pct), 100);
@@ -1474,11 +1349,6 @@
                 }
             }
 
-            /**
-             * Processa arquivo Excel selecionado
-             * @@param {File} file - Arquivo selecionado pelo usuário
-             * @@description Valida extensão e atualiza interface com nome do arquivo
-             */
             function selecionarArquivoXlsx(file) {
                 try {
                     const extensao = file.name.split('.').pop().toLowerCase();
@@ -1505,10 +1375,6 @@
                 }
             }
 
-            /**
-             * Remove arquivo Excel selecionado
-             * @@description Limpa seleção e restaura estado inicial do dropzone
-             */
             function removerArquivoXlsx() {
                 try {
                     arquivoXlsxAtual = null;
@@ -1529,11 +1395,6 @@
                 }
             }
 
-            /**
-             * Processa arquivo CSV selecionado
-             * @@param {File} file - Arquivo selecionado pelo usuário
-             * @@description Valida extensão .csv e atualiza interface
-             */
             function selecionarArquivoCsv(file) {
                 try {
                     const extensao = file.name.split('.').pop().toLowerCase();
@@ -1560,10 +1421,6 @@
                 }
             }
 
-            /**
-             * Remove arquivo CSV selecionado
-             * @@description Limpa seleção e restaura estado inicial do dropzone CSV
-             */
             function removerArquivoCsv() {
                 try {
                     arquivoCsvAtual = null;
@@ -1584,10 +1441,6 @@
                 }
             }
 
-            /**
-             * Remove todos os arquivos selecionados
-             * @@description Limpa tanto XLSX quanto CSV de uma vez
-             */
             function removerArquivo() {
                 try {
                     removerArquivoXlsx();
@@ -1597,11 +1450,6 @@
                 }
             }
 
-            /**
-             * Atualiza estado e texto do botão de importação
-             * @@description Habilita botão apenas quando ambos arquivos estão selecionados
-             * e conexão SignalR está ativa
-             */
             function atualizarBotaoImportar() {
                 try {
 
@@ -1622,12 +1470,6 @@
                 }
             }
 
-            /**
-             * Executa importação dos arquivos selecionados
-             * @@async
-             * @@returns {Promise<void>}
-             * @@description Envia FormData para API e processa resultado
-             */
             async function importar() {
                 try {
                     console.log('[IMPORTAR] Iniciando importação...');
@@ -1686,10 +1528,6 @@
                 }
             }
 
-            /**
-             * Alterna entre estados visuais da página
-             * @@param {string} estado - Estado a exibir: 'inicial'|'loading'|'sucesso'|'parcial'|'erro'
-             */
             function mostrarEstado(estado) {
                 try {
                     estadoInicial.classList.add('d-none');
@@ -1732,12 +1570,6 @@
                 }
             }
 
-            /**
-             * Exibe estado de sucesso total
-             * @@param {Object} result - Resultado da API com totais e lista
-             * @@param {number} result.totalLinhas - Total de linhas processadas
-             * @@param {number} result.linhasImportadas - Linhas importadas com sucesso
-             */
             function mostrarSucesso(result) {
                 try {
                     mostrarEstado('sucesso');
@@ -1754,18 +1586,18 @@
                         result.linhasImportadasLista.forEach(linha => {
                             const tr = document.createElement('tr');
                             tr.innerHTML = `
-                                        <td><small class="text-muted">${linha.dataHora}</small></td>
-                                        <td><span class="badge bg-secondary">${linha.placa}</span></td>
-                                        <td>${linha.motorista || '-'}</td>
-                                        <td>
-                                            <span class="badge ${linha.produto === 'Gasolina Comum' ? 'bg-warning text-dark' : 'bg-dark'}">
-                                                ${linha.produto}
-                                            </span>
-                                        </td>
-                                        <td class="text-end">${linha.quantidade}</td>
-                                        <td class="text-end">${linha.kmRodado}</td>
-                                        <td class="text-end"><strong>${linha.consumo}</strong></td>
-                                    `;
+                                <td><small class="text-muted">${linha.dataHora}</small></td>
+                                <td><span class="badge bg-secondary">${linha.placa}</span></td>
+                                <td>${linha.motorista || '-'}</td>
+                                <td>
+                                    <span class="badge ${linha.produto === 'Gasolina Comum' ? 'bg-warning text-dark' : 'bg-dark'}">
+                                        ${linha.produto}
+                                    </span>
+                                </td>
+                                <td class="text-end">${linha.quantidade}</td>
+                                <td class="text-end">${linha.kmRodado}</td>
+                                <td class="text-end"><strong>${linha.consumo}</strong></td>
+                            `;
                             tbody.appendChild(tr);
                         });
                     }
@@ -1778,11 +1610,6 @@
                 }
             }
 
-            /**
-             * Exibe estado de sucesso parcial (com erros)
-             * @@param {Object} result - Resultado com importados e erros
-             * @@param {Array} result.erros - Lista de erros para exibição
-             */
             function mostrarParcial(result) {
                 try {
                     mostrarEstado('parcial');
@@ -1818,10 +1645,6 @@
                 }
             }
 
-            /**
-             * Exibe estado de erro total (nenhum importado)
-             * @@param {Object} result - Resultado com lista de erros
-             */
             function mostrarErro(result) {
                 try {
                     mostrarEstado('erro');
@@ -1855,10 +1678,6 @@
                 }
             }
 
-            /**
-             * Exporta pendências para arquivo Excel
-             * @@description Redireciona para endpoint de download
-             */
             function exportarPendencias() {
                 try {
                     AppToast.show('Azul', 'Gerando arquivo Excel...', 3000);
```
