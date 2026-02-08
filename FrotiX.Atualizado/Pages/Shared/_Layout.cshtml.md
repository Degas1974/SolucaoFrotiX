# Pages/Shared/_Layout.cshtml

**Mudanca:** GRANDE | **+287** linhas | **-298** linhas

---

```diff
--- JANEIRO: Pages/Shared/_Layout.cshtml
+++ ATUAL: Pages/Shared/_Layout.cshtml
@@ -1,4 +1,5 @@
 @using System.Text.Json
+
 <!DOCTYPE html>
 <html lang="pt-BR">
 
@@ -10,14 +11,10 @@
 
     <link href="~/css/ftx-card-styled.css" rel="stylesheet" asp-append-version="true" />
 
-    @RenderSection("HeadBlock", required: false)
+    @RenderSection("HeadBlock" , required: false)
 
     <style>
-        thead,
-        thead th,
-        table thead th,
-        .dataTable thead th,
-        table.dataTable thead th {
+        thead, thead th, table thead th, .dataTable thead th, table.dataTable thead th {
             background: #4a6fa5 !important;
             background-color: #4a6fa5 !important;
             color: #ffffff !important;
@@ -30,7 +27,6 @@
     <script src="~/js/ftx-datatable-style.js" asp-append-version="true"></script>
 
 </head>
-
 <body>
 
     <partial name="_ScriptsLoadingSaving" />
@@ -42,8 +38,10 @@
             <div class="page-content-wrapper">
                 <partial name="_PageHeader" />
 
-                <main id="js-page-content" role="main" class="page-content"
-                    style="background: url('/images/dust_scratches.png')">
+                <main id="js-page-content"
+                      role="main"
+                      class="page-content"
+                      style="background: url('/images/dust_scratches.png')">
                     @if (ViewBag.PreemptiveClass?.Length > 0)
                     {
 
@@ -52,298 +50,290 @@
                     {
                         <partial name="_PageBreadcrumb" />
                         <div class="subheader">
-                            @RenderSection("Subheaderblock", required: false)
+                            @RenderSection("Subheaderblock" , required: false)
                         </div>
                     }
 
                     @RenderBody()
                 </main>
 
-                <div class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false"
-                    id="form-modal" aria-hidden="true">
+                <div class="modal fade"
+                     tabindex="-1"
+                     role="dialog"
+                     data-backdrop="static"
+                     data-keyboard="false"
+                     id="form-modal"
+                     aria-hidden="true">
                     <div class="modal-dialog modal-lg" role="document">
                         <div class="modal-content">
                             <div class="modal-header modal-header-terracota">
                                 <h5 class="modal-title"></h5>
-                                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                                    aria-label="Fechar"></button>
+                                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
                             </div>
 
                             <div class="modal-body">
-                                @RenderSection("ModalBody", required: false)
+                                @RenderSection("ModalBody" , required: false)
                             </div>
 
                             <div class="modal-footer">
-                                @RenderSection("ModalFooter", required: false)
+                                @RenderSection("ModalFooter" , required: false)
                             </div>
                         </div>
                     </div>
                 </div>
 
-                <div class="modal fade" id="modalDetalhesAlerta" tabindex="-1" role="dialog"
-                    aria-labelledby="modalDetalhesAlertaLabel" aria-hidden="true">
-                    <div class="modal-dialog modal-xl" role="document">
-                        <div class="modal-content">
-
-                            <div class="modal-header modal-header-terracota" id="alertaCabecalho">
-                                <div class="d-flex align-items-center w-100">
-                                    <i class="fa-duotone fa-bell fa-3x text-primary mr-3" id="iconeAlerta"></i>
-                                    <div class="flex-grow-1">
-                                        <h5 class="modal-title mb-1" id="tituloAlerta">Carregando...</h5>
-                                        <div>
-                                            <span class="badge badge-primary mr-2" id="badgeTipo">Tipo</span>
-                                            <span class="badge badge-danger mr-2" id="badgePrioridade">Prioridade</span>
-                                            <span id="badgeStatus"></span>
+<div class="modal fade" id="modalDetalhesAlerta" tabindex="-1" role="dialog" aria-labelledby="modalDetalhesAlertaLabel" aria-hidden="true">
+    <div class="modal-dialog modal-xl" role="document">
+        <div class="modal-content">
+
+            <div class="modal-header modal-header-terracota" id="alertaCabecalho">
+                <div class="d-flex align-items-center w-100">
+                    <i class="fa-duotone fa-bell fa-3x text-primary mr-3" id="iconeAlerta"></i>
+                    <div class="flex-grow-1">
+                        <h5 class="modal-title mb-1" id="tituloAlerta">Carregando...</h5>
+                        <div>
+                            <span class="badge badge-primary mr-2" id="badgeTipo">Tipo</span>
+                            <span class="badge badge-danger mr-2" id="badgePrioridade">Prioridade</span>
+                            <span id="badgeStatus"></span>
+                        </div>
+                    </div>
+                </div>
+                <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
+                    <span aria-hidden="true">&times;</span>
+                </button>
+            </div>
+
+            <div class="modal-body">
+
+                <div id="loaderDetalhes" class="text-center py-5">
+                    <i class="fa-duotone fa-spinner fa-spin fa-3x text-primary mb-3"></i>
+                    <p>Carregando detalhes do alerta...</p>
+                </div>
+
+                <div id="conteudoDetalhes" style="display: none;">
+
+                    <div class="card mb-3">
+                        <div class="card-header bg-light">
+                            <i class="fa-duotone fa-info-circle"></i> Descrição
+                        </div>
+                        <div class="card-body">
+                            <p id="descricaoAlerta" class="mb-0"></p>
+                        </div>
+                    </div>
+
+                    <div class="row mb-3">
+                        <div class="col-md-6">
+                            <div class="card h-100">
+                                <div class="card-header bg-light">
+                                    <i class="fa-duotone fa-calendar-alt"></i> Informações Gerais
+                                </div>
+                                <div class="card-body">
+                                    <p><strong>Data de Criação:</strong> <span id="dataCriacao"></span></p>
+                                    <p><strong>Data de Exibição:</strong> <span id="dataExibicao"></span></p>
+                                    <p><strong>Criado Por:</strong> <span id="criadoPor"></span></p>
+                                    <p class="mb-0"><strong>Tempo no Ar:</strong> <span id="tempoNoAr"></span></p>
+                                </div>
+                            </div>
+                        </div>
+
+                        <div class="col-md-6">
+                            <div class="card h-100">
+                                <div class="card-header bg-light">
+                                    <i class="fa-duotone fa-chart-bar"></i> Resumo das Estatísticas
+                                </div>
+                                <div class="card-body">
+                                    <div class="row">
+                                        <div class="col-6">
+                                            <p class="mb-2">
+                                                <i class="fa-duotone fa-users text-primary"></i>
+                                                <strong>Destinatários:</strong> <span id="totalDestinatariosResumo">0</span>
+                                            </p>
+                                            <p class="mb-2">
+                                                <i class="fa-duotone fa-bell text-info"></i>
+                                                <strong>Notificados:</strong> <span id="totalNotificadosResumo">0</span>
+                                            </p>
+                                            <p class="mb-0">
+                                                <i class="fa-duotone fa-clock text-warning"></i>
+                                                <strong>Aguardando:</strong> <span id="aguardandoNotificacaoResumo">0</span>
+                                            </p>
+                                        </div>
+                                        <div class="col-6">
+                                            <p class="mb-2">
+                                                <i class="fa-duotone fa-check-circle text-success"></i>
+                                                <strong>Leram:</strong> <span id="usuariosLeramResumo">0</span>
+                                            </p>
+                                            <p class="mb-2">
+                                                <i class="fa-duotone fa-times-circle text-danger"></i>
+                                                <strong>Não Leram:</strong> <span id="usuariosNaoLeramResumo">0</span>
+                                            </p>
+                                            <p class="mb-0">
+                                                <i class="fa-duotone fa-trash text-secondary"></i>
+                                                <strong>Apagaram:</strong> <span id="usuariosApagaramResumo">0</span>
+                                            </p>
                                         </div>
                                     </div>
                                 </div>
-                                <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
-                                    <span aria-hidden="true">&times;</span>
-                                </button>
-                            </div>
-
-                            <div class="modal-body">
-
-                                <div id="loaderDetalhes" class="text-center py-5">
-                                    <i class="fa-duotone fa-spinner fa-spin fa-3x text-primary mb-3"></i>
-                                    <p>Carregando detalhes do alerta...</p>
-                                </div>
-
-                                <div id="conteudoDetalhes" style="display: none;">
-
-                                    <div class="card mb-3">
-                                        <div class="card-header bg-light">
-                                            <i class="fa-duotone fa-info-circle"></i> Descrição
-                                        </div>
-                                        <div class="card-body">
-                                            <p id="descricaoAlerta" class="mb-0"></p>
-                                        </div>
+                            </div>
+                        </div>
+                    </div>
+
+                    <div class="mb-4">
+                        <h6 class="fw-bold mb-3">
+                            <i class="fa-duotone fa-chart-line"></i> Estatísticas Detalhadas
+                        </h6>
+                        <div class="row g-3">
+
+                            <div class="col-md-4 col-lg-2">
+                                <div class="card border-primary shadow-sm h-100">
+                                    <div class="card-body text-center py-3">
+                                        <i class="fa-duotone fa-users fa-2x text-primary mb-2"></i>
+                                        <h6 class="card-subtitle mb-2 text-muted small">Total Destinatários</h6>
+                                        <h3 class="card-title mb-0" id="totalDestinatarios">0</h3>
                                     </div>
-
-                                    <div class="row mb-3">
-                                        <div class="col-md-6">
-                                            <div class="card h-100">
-                                                <div class="card-header bg-light">
-                                                    <i class="fa-duotone fa-calendar-alt"></i> Informações Gerais
+                                </div>
+                            </div>
+
+                            <div class="col-md-4 col-lg-2">
+                                <div class="card border-info shadow-sm h-100">
+                                    <div class="card-body text-center py-3">
+                                        <i class="fa-duotone fa-bell fa-2x text-info mb-2"></i>
+                                        <h6 class="card-subtitle mb-2 text-muted small">Já Notificados</h6>
+                                        <h3 class="card-title mb-0" id="totalNotificados">0</h3>
+                                    </div>
+                                </div>
+                            </div>
+
+                            <div class="col-md-4 col-lg-2">
+                                <div class="card border-warning shadow-sm h-100">
+                                    <div class="card-body text-center py-3">
+                                        <i class="fa-duotone fa-clock fa-2x text-warning mb-2"></i>
+                                        <h6 class="card-subtitle mb-2 text-muted small">Aguardando</h6>
+                                        <h3 class="card-title mb-0" id="aguardandoNotificacao">0</h3>
+                                    </div>
+                                </div>
+                            </div>
+
+                            <div class="col-md-4 col-lg-2">
+                                <div class="card border-success shadow-sm h-100">
+                                    <div class="card-body text-center py-3">
+                                        <i class="fa-duotone fa-check-circle fa-2x text-success mb-2"></i>
+                                        <h6 class="card-subtitle mb-2 text-muted small">Leram</h6>
+                                        <h3 class="card-title mb-0" id="usuariosLeram">0</h3>
+                                    </div>
+                                </div>
+                            </div>
+
+                            <div class="col-md-4 col-lg-2">
+                                <div class="card border-danger shadow-sm h-100">
+                                    <div class="card-body text-center py-3">
+                                        <i class="fa-duotone fa-times-circle fa-2x text-danger mb-2"></i>
+                                        <h6 class="card-subtitle mb-2 text-muted small">Não Leram</h6>
+                                        <h3 class="card-title mb-0" id="usuariosNaoLeram">0</h3>
+                                    </div>
+                                </div>
+                            </div>
+
+                            <div class="col-md-4 col-lg-2">
+                                <div class="card border-secondary shadow-sm h-100">
+                                    <div class="card-body text-center py-3">
+                                        <i class="fa-duotone fa-trash fa-2x text-secondary mb-2"></i>
+                                        <h6 class="card-subtitle mb-2 text-muted small">Apagaram</h6>
+                                        <h3 class="card-title mb-0" id="usuariosApagaram">0</h3>
+                                    </div>
+                                </div>
+                            </div>
+                        </div>
+                    </div>
+
+                    <div class="card mb-3">
+                        <div class="card-header bg-light">
+                            <i class="fa-duotone fa-chart-line"></i> Progresso de Leitura
+                        </div>
+                        <div class="card-body">
+                            <div class="mb-2">
+                                <small class="text-muted" id="infoLeitores">Carregando...</small>
+                            </div>
+                            <div class="progress" style="height: 30px;">
+                                <div class="progress-bar progress-bar-striped progress-bar-animated"
+                                     id="progressoLeitura"
+                                     role="progressbar"
+                                     aria-valuenow="0"
+                                     aria-valuemin="0"
+                                     aria-valuemax="100"
+                                     style="width: 0%;">
+                                    <span id="textoProgressoBarra">0%</span>
+                                </div>
+                            </div>
+                        </div>
+                    </div>
+
+                    <div class="card mb-3" id="linksRelacionados" style="display: none;">
+                        <div class="card-header bg-light">
+                            <i class="fa-duotone fa-link"></i> Links Relacionados
+                        </div>
+                        <div class="card-body" id="conteudoLinks">
+
+                        </div>
+                    </div>
+
+                    <div class="card">
+                        <div class="card-header bg-light d-flex justify-content-between align-items-center">
+                            <span>
+                                <i class="fa-duotone fa-users"></i>
+                                Usuários que Receberam o Alerta
+                            </span>
+                            <button class="btn btn-sm btn-verde" id="btnExportarDetalhes" style="display: none;">
+                                <i class="fa-duotone fa-file-excel"></i> Exportar Excel
+                            </button>
+                        </div>
+                        <div class="card-body p-0">
+                            <div class="table-responsive">
+                                <table class="table table-striped table-sm table-hover mb-0">
+                                    <thead class="table-dark">
+                                        <tr>
+                                            <th>Usuário</th>
+                                            <th class="text-center">Notificado</th>
+                                            <th class="text-center">Status Leitura</th>
+                                            <th class="text-center">Data Notificação</th>
+                                            <th class="text-center">Data Leitura</th>
+                                            <th class="text-center">Tempo até Leitura</th>
+                                        </tr>
+                                    </thead>
+                                    <tbody id="tabelaUsuarios">
+                                        <tr>
+                                            <td colspan="6" class="text-center py-4">
+                                                <div class="spinner-border spinner-border-sm text-primary" role="status">
+                                                    <span class="visually-hidden">Carregando...</span>
                                                 </div>
-                                                <div class="card-body">
-                                                    <p><strong>Data de Criação:</strong> <span id="dataCriacao"></span>
-                                                    </p>
-                                                    <p><strong>Data de Exibição:</strong> <span
-                                                            id="dataExibicao"></span></p>
-                                                    <p><strong>Criado Por:</strong> <span id="criadoPor"></span></p>
-                                                    <p class="mb-0"><strong>Tempo no Ar:</strong> <span
-                                                            id="tempoNoAr"></span></p>
-                                                </div>
-                                            </div>
-                                        </div>
-
-                                        <div class="col-md-6">
-                                            <div class="card h-100">
-                                                <div class="card-header bg-light">
-                                                    <i class="fa-duotone fa-chart-bar"></i> Resumo das Estatísticas
-                                                </div>
-                                                <div class="card-body">
-                                                    <div class="row">
-                                                        <div class="col-6">
-                                                            <p class="mb-2">
-                                                                <i class="fa-duotone fa-users text-primary"></i>
-                                                                <strong>Destinatários:</strong> <span
-                                                                    id="totalDestinatariosResumo">0</span>
-                                                            </p>
-                                                            <p class="mb-2">
-                                                                <i class="fa-duotone fa-bell text-info"></i>
-                                                                <strong>Notificados:</strong> <span
-                                                                    id="totalNotificadosResumo">0</span>
-                                                            </p>
-                                                            <p class="mb-0">
-                                                                <i class="fa-duotone fa-clock text-warning"></i>
-                                                                <strong>Aguardando:</strong> <span
-                                                                    id="aguardandoNotificacaoResumo">0</span>
-                                                            </p>
-                                                        </div>
-                                                        <div class="col-6">
-                                                            <p class="mb-2">
-                                                                <i class="fa-duotone fa-check-circle text-success"></i>
-                                                                <strong>Leram:</strong> <span
-                                                                    id="usuariosLeramResumo">0</span>
-                                                            </p>
-                                                            <p class="mb-2">
-                                                                <i class="fa-duotone fa-times-circle text-danger"></i>
-                                                                <strong>Não Leram:</strong> <span
-                                                                    id="usuariosNaoLeramResumo">0</span>
-                                                            </p>
-                                                            <p class="mb-0">
-                                                                <i class="fa-duotone fa-trash text-secondary"></i>
-                                                                <strong>Apagaram:</strong> <span
-                                                                    id="usuariosApagaramResumo">0</span>
-                                                            </p>
-                                                        </div>
-                                                    </div>
-                                                </div>
-                                            </div>
-                                        </div>
-                                    </div>
-
-                                    <div class="mb-4">
-                                        <h6 class="fw-bold mb-3">
-                                            <i class="fa-duotone fa-chart-line"></i> Estatísticas Detalhadas
-                                        </h6>
-                                        <div class="row g-3">
-
-                                            <div class="col-md-4 col-lg-2">
-                                                <div class="card border-primary shadow-sm h-100">
-                                                    <div class="card-body text-center py-3">
-                                                        <i class="fa-duotone fa-users fa-2x text-primary mb-2"></i>
-                                                        <h6 class="card-subtitle mb-2 text-muted small">Total
-                                                            Destinatários</h6>
-                                                        <h3 class="card-title mb-0" id="totalDestinatarios">0</h3>
-                                                    </div>
-                                                </div>
-                                            </div>
-
-                                            <div class="col-md-4 col-lg-2">
-                                                <div class="card border-info shadow-sm h-100">
-                                                    <div class="card-body text-center py-3">
-                                                        <i class="fa-duotone fa-bell fa-2x text-info mb-2"></i>
-                                                        <h6 class="card-subtitle mb-2 text-muted small">Já Notificados
-                                                        </h6>
-                                                        <h3 class="card-title mb-0" id="totalNotificados">0</h3>
-                                                    </div>
-                                                </div>
-                                            </div>
-
-                                            <div class="col-md-4 col-lg-2">
-                                                <div class="card border-warning shadow-sm h-100">
-                                                    <div class="card-body text-center py-3">
-                                                        <i class="fa-duotone fa-clock fa-2x text-warning mb-2"></i>
-                                                        <h6 class="card-subtitle mb-2 text-muted small">Aguardando</h6>
-                                                        <h3 class="card-title mb-0" id="aguardandoNotificacao">0</h3>
-                                                    </div>
-                                                </div>
-                                            </div>
-
-                                            <div class="col-md-4 col-lg-2">
-                                                <div class="card border-success shadow-sm h-100">
-                                                    <div class="card-body text-center py-3">
-                                                        <i
-                                                            class="fa-duotone fa-check-circle fa-2x text-success mb-2"></i>
-                                                        <h6 class="card-subtitle mb-2 text-muted small">Leram</h6>
-                                                        <h3 class="card-title mb-0" id="usuariosLeram">0</h3>
-                                                    </div>
-                                                </div>
-                                            </div>
-
-                                            <div class="col-md-4 col-lg-2">
-                                                <div class="card border-danger shadow-sm h-100">
-                                                    <div class="card-body text-center py-3">
-                                                        <i
-                                                            class="fa-duotone fa-times-circle fa-2x text-danger mb-2"></i>
-                                                        <h6 class="card-subtitle mb-2 text-muted small">Não Leram</h6>
-                                                        <h3 class="card-title mb-0" id="usuariosNaoLeram">0</h3>
-                                                    </div>
-                                                </div>
-                                            </div>
-
-                                            <div class="col-md-4 col-lg-2">
-                                                <div class="card border-secondary shadow-sm h-100">
-                                                    <div class="card-body text-center py-3">
-                                                        <i class="fa-duotone fa-trash fa-2x text-secondary mb-2"></i>
-                                                        <h6 class="card-subtitle mb-2 text-muted small">Apagaram</h6>
-                                                        <h3 class="card-title mb-0" id="usuariosApagaram">0</h3>
-                                                    </div>
-                                                </div>
-                                            </div>
-                                        </div>
-                                    </div>
-
-                                    <div class="card mb-3">
-                                        <div class="card-header bg-light">
-                                            <i class="fa-duotone fa-chart-line"></i> Progresso de Leitura
-                                        </div>
-                                        <div class="card-body">
-                                            <div class="mb-2">
-                                                <small class="text-muted" id="infoLeitores">Carregando...</small>
-                                            </div>
-                                            <div class="progress" style="height: 30px;">
-                                                <div class="progress-bar progress-bar-striped progress-bar-animated"
-                                                    id="progressoLeitura" role="progressbar" aria-valuenow="0"
-                                                    aria-valuemin="0" aria-valuemax="100" style="width: 0%;">
-                                                    <span id="textoProgressoBarra">0%</span>
-                                                </div>
-                                            </div>
-                                        </div>
-                                    </div>
-
-                                    <div class="card mb-3" id="linksRelacionados" style="display: none;">
-                                        <div class="card-header bg-light">
-                                            <i class="fa-duotone fa-link"></i> Links Relacionados
-                                        </div>
-                                        <div class="card-body" id="conteudoLinks">
-
-                                        </div>
-                                    </div>
-
-                                    <div class="card">
-                                        <div
-                                            class="card-header bg-light d-flex justify-content-between align-items-center">
-                                            <span>
-                                                <i class="fa-duotone fa-users"></i>
-                                                Usuários que Receberam o Alerta
-                                            </span>
-                                            <button class="btn btn-sm btn-verde" id="btnExportarDetalhes"
-                                                style="display: none;">
-                                                <i class="fa-duotone fa-file-excel"></i> Exportar Excel
-                                            </button>
-                                        </div>
-                                        <div class="card-body p-0">
-                                            <div class="table-responsive">
-                                                <table class="table table-striped table-sm table-hover mb-0">
-                                                    <thead class="table-dark">
-                                                        <tr>
-                                                            <th>Usuário</th>
-                                                            <th class="text-center">Notificado</th>
-                                                            <th class="text-center">Status Leitura</th>
-                                                            <th class="text-center">Data Notificação</th>
-                                                            <th class="text-center">Data Leitura</th>
-                                                            <th class="text-center">Tempo até Leitura</th>
-                                                        </tr>
-                                                    </thead>
-                                                    <tbody id="tabelaUsuarios">
-                                                        <tr>
-                                                            <td colspan="6" class="text-center py-4">
-                                                                <div class="spinner-border spinner-border-sm text-primary"
-                                                                    role="status">
-                                                                    <span class="visually-hidden">Carregando...</span>
-                                                                </div>
-                                                                <span class="ms-2 text-muted">Carregando
-                                                                    usuários...</span>
-                                                            </td>
-                                                        </tr>
-                                                    </tbody>
-                                                </table>
-                                            </div>
-                                        </div>
-                                    </div>
-                                </div>
-                            </div>
-
-                            <div class="modal-footer d-flex justify-content-between">
-
-                                <button type="button" class="btn btn-verde" id="btnBaixaAlerta"
-                                    onclick="darBaixaNoAlerta()">
-                                    <i class="fa-duotone fa-check-circle"></i> Baixa no Alerta
-                                </button>
-
-                                <div>
-                                    <button type="button" class="btn btn-vinho" data-dismiss="modal">
-                                        <i class="fa-duotone fa-times"></i> Fechar
-                                    </button>
-                                </div>
+                                                <span class="ms-2 text-muted">Carregando usuários...</span>
+                                            </td>
+                                        </tr>
+                                    </tbody>
+                                </table>
                             </div>
                         </div>
                     </div>
                 </div>
+            </div>
+
+            <div class="modal-footer d-flex justify-content-between">
+
+                <button type="button"
+                        class="btn btn-verde"
+                        id="btnBaixaAlerta"
+                        onclick="darBaixaNoAlerta()">
+                    <i class="fa-duotone fa-check-circle"></i> Baixa no Alerta
+                </button>
+
+                <div>
+                    <button type="button" class="btn btn-vinho" data-dismiss="modal">
+                        <i class="fa-duotone fa-times"></i> Fechar
+                    </button>
+                </div>
+            </div>
+        </div>
+    </div>
+</div>
                 <partial name="_ColorProfileReference" />
             </div>
         </div>
@@ -357,12 +347,12 @@
     <partial name="_ScriptsBasePlugins" />
 
     @await Component.InvokeAsync("Notyf")
-    @RenderSection("ScriptsBlock", required: false)
+    @RenderSection("ScriptsBlock" , required: false)
 
     @if (TempData["ShowSweetAlert"] != null)
     {
         <script>
-            document.addEventListener('DOMContentLoaded', function () {
+            document.addEventListener('DOMContentLoaded', function() {
                 try {
                     const alertData = @Html.Raw(TempData["ShowSweetAlert"]);
 
@@ -373,7 +363,7 @@
                         return;
                     }
 
-                    switch (alertData.type) {
+                    switch(alertData.type) {
                         case 'error':
                             SweetAlertInterop.ShowError(alertData.title, alertData.message, alertData.confirmButton);
                             break;
@@ -408,19 +398,19 @@
 
     <script>
         window.GlobalAlert = {
-            showError: function (title, message, details = null) {
+            showError: function(title, message, details = null) {
                 this.showDialog('error', title, message, details);
             },
 
-            showSuccess: function (title, message) {
+            showSuccess: function(title, message) {
                 this.showDialog('success', title, message);
             },
 
-            showWarning: function (title, message) {
+            showWarning: function(title, message) {
                 this.showDialog('warning', title, message);
             },
 
-            showDialog: function (type, title, message, details = null) {
+            showDialog: function(type, title, message, details = null) {
                 const dialog = document.getElementById('globalDialog').ej2_instances[0];
                 const content = document.getElementById('globalDialogContent');
 
@@ -439,7 +429,7 @@
                 dialog.show();
             },
 
-            getDialogConfig: function (type) {
+            getDialogConfig: function(type) {
                 const configs = {
                     error: {
                         icon: '🚨',
@@ -461,7 +451,7 @@
             }
         };
 
-        document.addEventListener('DOMContentLoaded', function () {
+        document.addEventListener('DOMContentLoaded', function() {
             if (window.pendingAlert) {
                 GlobalAlert.showError(
                     window.pendingAlert.title,
@@ -472,22 +462,17 @@
         });
     </script>
 
-    <ejs-dialog id="globalDialog" width="500px" isModal="true" visible="false" cssClass="custom-alert-dialog"
-        showCloseIcon="true" allowDragging="false">
+    <ejs-dialog id="globalDialog" width="500px" isModal="true" visible="false"
+                cssClass="custom-alert-dialog" showCloseIcon="true" allowDragging="false">
         <e-content-template>
             <div id="globalDialogContent"></div>
         </e-content-template>
         <e-dialog-buttons>
-            <e-dialog-button buttonModel="{ content: 'OK', isPrimary: true }"
-                click="closeGlobalDialog"></e-dialog-button>
+            <e-dialog-button buttonModel="{ content: 'OK', isPrimary: true }" click="closeGlobalDialog"></e-dialog-button>
         </e-dialog-buttons>
     </ejs-dialog>
 
     <script>
-        /**
-         * Fecha o dialog global Syncfusion
-         * @@description Oculta o modal de diálogo global customizado
-            */
         function closeGlobalDialog() {
             document.getElementById('globalDialog').ej2_instances[0].hide();
         }
@@ -497,7 +482,7 @@
         .custom-alert-dialog .e-dialog {
             border-radius: 15px;
             font-family: 'Segoe UI', sans-serif;
-            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.15);
+            box-shadow: 0 20px 60px rgba(0,0,0,0.15);
         }
 
         .custom-alert-dialog .e-dlg-header {
@@ -527,9 +512,7 @@
             padding: 20px;
         }
 
-        .error-message,
-        .success-message,
-        .warning-message {
+        .error-message, .success-message, .warning-message {
             font-size: 16px;
             line-height: 1.5;
             margin-bottom: 10px;
@@ -541,14 +524,13 @@
             color: #666;
         }
 
-        .error-details pre {
-            background: #f7fafc;
-            padding: 10px;
-            border-radius: 5px;
-            overflow-x: auto;
-            max-height: 200px;
-        }
-
+            .error-details pre {
+                background: #f7fafc;
+                padding: 10px;
+                border-radius: 5px;
+                overflow-x: auto;
+                max-height: 200px;
+            }
         /* Estilos para o modal */
         .modal-xl {
             max-width: 1200px;
@@ -605,6 +587,21 @@
         });
     </script>
 
+    <script>
+        (function () {
+            try {
+                var ajax = new ej.base.Ajax(location.origin + '/../../locale/pt-BR.json', 'GET', false);
+                ajax.send().then(function (e) {
+                    var loader = JSON.parse(e);
+                    ej.base.L10n.load(loader);
+                    ej.base.setCulture('pt-BR');
+                });
+            } catch (err) {
+                console.error('Falha ao carregar cultura Syncfusion', err);
+            }
+        })();
+    </script>
+
     <ejs-scripts></ejs-scripts>
 
     <div id="loading-overlay" hidden>
@@ -617,7 +614,7 @@
     @if (!string.IsNullOrWhiteSpace(uiError))
     {
         <script>
-            (function () {
+            (function(){
                 try {
                     var e = JSON.parse(@Html.Raw(JsonSerializer.Serialize(uiError)));
                     if (typeof e === 'string') { e = JSON.parse(e); }
@@ -636,7 +633,7 @@
                     } else {
                         console.error('Não encontrei Alerta.TratamentoErroComLinha nem SweetAlertInterop.ShowErrorUnexpected', e);
                     }
-                } catch (err) {
+                } catch(err) {
                     console.error('Falha ao exibir alerta de erro', err);
                 }
             })();
@@ -644,21 +641,9 @@
     }
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * LAYOUT - FUNÇÕES GLOBAIS DE ALERTAS
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Funções utilitárias para ícones e cores de prioridade de alertas.
-         * @@file Shared / _Layout.cshtml
-            */
-
-        /**
-         * Retorna a classe de ícone FontAwesome para uma prioridade
-         * @@param { number } prioridade - Nível de prioridade(1 = Info, 2 = Warning, 3 = Danger)
-            * @@returns { string } Classe CSS do ícone FontAwesome Duotone
-                */
+
         function getIconePrioridade(prioridade) {
-            switch (prioridade) {
+            switch(prioridade) {
                 case 1: return 'fa-duotone fa-circle-info';
                 case 2: return 'fa-duotone fa-circle-exclamation';
                 case 3: return 'fa-duotone fa-triangle-exclamation';
@@ -666,13 +651,8 @@
             }
         }
 
-        /**
-         * Retorna a classe de cor para uma prioridade
-         * @@param { number } prioridade - Nível de prioridade(1 = Info, 2 = Warning, 3 = Danger)
-            * @@returns { string } Classe CSS de cor Bootstrap
-                */
         function getCorPrioridade(prioridade) {
-            switch (prioridade) {
+            switch(prioridade) {
                 case 1: return 'text-info';
                 case 2: return 'text-warning';
                 case 3: return 'text-danger';
@@ -680,6 +660,13 @@
             }
         }
 
+    </script>
+
+    <script src="/js/localization-init.js"></script>
+    <script>
+        document.addEventListener("DOMContentLoaded", function () {
+            loadSyncfusionLocalization();
+        });
     </script>
 
     @if (TempData["ToastScripts"] != null)
@@ -689,8 +676,11 @@
         </script>
     }
 
+    <script src="~/js/global-error-handler.js" asp-append-version="true"></script>
+
+    <script src="~/js/frotix-api-client.js" asp-append-version="true"></script>
     <script src="~/js/frotix-error-logger.js" asp-append-version="true"></script>
+    <script src="~/js/console-interceptor.js" asp-append-version="true"></script>
 
 </body>
-
 </html>
```
