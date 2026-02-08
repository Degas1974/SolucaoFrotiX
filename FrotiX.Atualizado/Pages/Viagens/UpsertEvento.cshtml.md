# Pages/Viagens/UpsertEvento.cshtml

**Mudanca:** GRANDE | **+699** linhas | **-543** linhas

---

```diff
--- JANEIRO: Pages/Viagens/UpsertEvento.cshtml
+++ ATUAL: Pages/Viagens/UpsertEvento.cshtml
@@ -6,16 +6,16 @@
 @model FrotiX.Pages.Viagens.UpsertEventoModel
 
 @{
-    ViewData["Title"] = "Eventos";
-    ViewData["PageName"] = "viagens_upsertevento";
-    ViewData["Heading"] = "<i class='fa-duotone fa-calendar-users'></i> Cadastros: <span class='fw-300'>Eventos</span>";
-    ViewData["Category1"] = "Cadastros";
-    ViewData["PageIcon"] = "fa-duotone fa-calendar-users";
+ViewData["Title"] = "Eventos";
+ViewData["PageName"] = "viagens_upsertevento";
+ViewData["Heading"] = "<i class='fa-duotone fa-calendar-users'></i> Cadastros: <span class='fw-300'>Eventos</span>";
+ViewData["Category1"] = "Cadastros";
+ViewData["PageIcon"] = "fa-duotone fa-calendar-users";
 }
 
 @section HeadBlock {
     <style>
-        /* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
+/* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
         .ftx-card-header .btn-fundo-laranja {
             outline: 2px solid rgba(255, 255, 255, 0.5) !important;
             outline-offset: 1px;
@@ -26,6 +26,7 @@
             outline: 2px solid rgba(255, 255, 255, 0.8) !important;
             outline-offset: 2px;
         }
+
     </style>
     <link href="~/css/ftx-card-styled.css" rel="stylesheet" asp-append-version="true" />
 }
@@ -36,11 +37,9 @@
         0% {
             background-position: 0% 50%;
         }
-
         50% {
             background-position: 100% 50%;
         }
-
         100% {
             background-position: 0% 50%;
         }
@@ -117,90 +116,90 @@
         font-size: 0.875rem !important;
     }
 
-    /* Input group do Syncfusion */
-    .e-dropdowntree .e-input-group,
-    .e-dropdowntree.e-input-group {
-        height: 40px !important;
-        border: 1px solid #ced4da !important;
-        border-radius: 0.375rem !important;
-        background-color: #fff !important;
-    }
-
-    /* Input dentro do dropdown */
-    .e-dropdowntree input.e-dropdownbase,
-    .e-dropdowntree .e-input {
-        height: 38px !important;
-        padding: 0.5rem 0.875rem !important;
-        font-size: 0.875rem !important;
-        border: none !important;
-        background: transparent !important;
-    }
-
-    /* Texto do placeholder e valor selecionado */
-    .e-dropdowntree .e-input-value,
-    .e-dropdowntree input::placeholder {
-        color: #495057 !important;
-        font-size: 0.875rem !important;
-    }
-
-    .e-dropdowntree input:placeholder-shown {
-        color: #6c757d !important;
-    }
-
-    /* === ÍCONES === */
-
-    /* Container dos ícones */
-    .e-dropdowntree .e-input-group-icon,
-    .e-dropdowntree .e-ddt-icon {
-        border: none !important;
-        background: transparent !important;
-        min-height: 36px !important;
-        display: flex !important;
-        align-items: center !important;
-        padding: 0 8px !important;
-    }
-
-    /* Ajuste do ícone de dropdown */
-    .e-dropdowntree .e-input-group-icon.e-ddt-icon {
-        border-left: 1px solid #ced4da !important;
-    }
-
-    /* === ESTADOS INTERATIVOS === */
-
-    /* Hover */
-    .e-dropdowntree:hover .e-input-group,
-    .e-dropdowntree .e-input-group:hover {
-        border-color: #86b7fe !important;
-    }
-
-    /* Focus */
-    .e-dropdowntree.e-input-focus .e-input-group,
-    .e-dropdowntree .e-input-group.e-input-focus {
-        border-color: #86b7fe !important;
-        box-shadow: 0 0 0 0.25rem rgba(13, 110, 253, .25) !important;
-        outline: none !important;
-    }
-
-    /* === CHIPS (SELEÇÃO MÚLTIPLA) === */
-
-    .e-dropdowntree .e-chips-wrapper {
-        min-height: 36px !important;
-        padding: 0.25rem !important;
-    }
-
-    .e-dropdowntree .e-chip {
-        height: 24px !important;
-        margin: 2px !important;
-        font-size: 0.75rem !important;
-    }
+        /* Input group do Syncfusion */
+        .e-dropdowntree .e-input-group,
+        .e-dropdowntree.e-input-group {
+            height: 40px !important;
+            border: 1px solid #ced4da !important;
+            border-radius: 0.375rem !important;
+            background-color: #fff !important;
+        }
+
+        /* Input dentro do dropdown */
+        .e-dropdowntree input.e-dropdownbase,
+        .e-dropdowntree .e-input {
+            height: 38px !important;
+            padding: 0.5rem 0.875rem !important;
+            font-size: 0.875rem !important;
+            border: none !important;
+            background: transparent !important;
+        }
+
+        /* Texto do placeholder e valor selecionado */
+        .e-dropdowntree .e-input-value,
+        .e-dropdowntree input::placeholder {
+            color: #495057 !important;
+            font-size: 0.875rem !important;
+        }
+
+        .e-dropdowntree input:placeholder-shown {
+            color: #6c757d !important;
+        }
+
+        /* === ÍCONES === */
+
+        /* Container dos ícones */
+        .e-dropdowntree .e-input-group-icon,
+        .e-dropdowntree .e-ddt-icon {
+            border: none !important;
+            background: transparent !important;
+            min-height: 36px !important;
+            display: flex !important;
+            align-items: center !important;
+            padding: 0 8px !important;
+        }
+
+            /* Ajuste do ícone de dropdown */
+            .e-dropdowntree .e-input-group-icon.e-ddt-icon {
+                border-left: 1px solid #ced4da !important;
+            }
+
+        /* === ESTADOS INTERATIVOS === */
+
+        /* Hover */
+        .e-dropdowntree:hover .e-input-group,
+        .e-dropdowntree .e-input-group:hover {
+            border-color: #86b7fe !important;
+        }
+
+        /* Focus */
+        .e-dropdowntree.e-input-focus .e-input-group,
+        .e-dropdowntree .e-input-group.e-input-focus {
+            border-color: #86b7fe !important;
+            box-shadow: 0 0 0 0.25rem rgba(13,110,253,.25) !important;
+            outline: none !important;
+        }
+
+        /* === CHIPS (SELEÇÃO MÚLTIPLA) === */
+
+        .e-dropdowntree .e-chips-wrapper {
+            min-height: 36px !important;
+            padding: 0.25rem !important;
+        }
+
+        .e-dropdowntree .e-chip {
+            height: 24px !important;
+            margin: 2px !important;
+            font-size: 0.75rem !important;
+        }
 
     /* === POPUP DO DROPDOWN === */
 
     .e-popup.e-dropdownbase,
     .e-ddt-popup {
-        border: 1px solid rgba(0, 0, 0, .15) !important;
+        border: 1px solid rgba(0,0,0,.15) !important;
         border-radius: 0.375rem !important;
-        box-shadow: 0 0.25rem 0.5rem rgba(0, 0, 0, .2) !important;
+        box-shadow: 0 0.25rem 0.5rem rgba(0,0,0,.2) !important;
         margin-top: 2px !important;
         font-size: 0.875rem !important;
     }
@@ -208,7 +207,7 @@
     /* === FIX PARA ALINHAMENTO COM BOOTSTRAP === */
 
     /* Remove margins indesejadas */
-    .form-label+.e-dropdowntree {
+    .form-label + .e-dropdowntree {
         margin-top: 0 !important;
     }
 
@@ -221,9 +220,9 @@
     /* === COMPATIBILIDADE COM LABELS DO BOOTSTRAP === */
 
     /* Ajuste quando precedido por label */
-    label.form-label+.e-dropdowntree,
-    label.form-label+#lstRequisitanteEvento,
-    label.form-label+#ddtSetorRequisitanteEvento {
+    label.form-label + .e-dropdowntree,
+    label.form-label + #lstRequisitanteEvento,
+    label.form-label + #ddtSetorRequisitanteEvento {
         display: block !important;
         width: 100% !important;
     }
@@ -269,7 +268,7 @@
     #lstRequisitanteEvento.e-dropdowntree,
     #lstRequisitanteEvento .e-input-group,
     #lstRequisitanteEvento.e-input-group,
-    #lstRequisitanteEvento>.e-input-group,
+    #lstRequisitanteEvento > .e-input-group,
     #lstRequisitanteEvento .e-ddt,
     #lstRequisitanteEvento .e-ddt-wrapper {
         border: 1px solid #ced4da !important;
@@ -307,10 +306,18 @@
         gap: 0.35rem;
     }
 
-    /* Labels com classe small */
-    .form-label.small {
-        font-size: 0.8125rem !important;
-    }
+    /* Asterisco vermelho itálico para campos obrigatórios */
+    .form-label .required-asterisk {
+        color: #dc3545 !important;
+        font-style: italic;
+        margin-right: -2px;
+        margin-left: 2px;
+    }
+
+        /* Labels com classe small */
+        .form-label.small {
+            font-size: 0.8125rem !important;
+        }
 
     /* Ícones nos labels */
     .form-label i {
@@ -325,7 +332,7 @@
         padding: 1.25rem;
         margin-bottom: 1.25rem;
         border: 1px solid #dee2e6;
-        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
+        box-shadow: 0 1px 3px rgba(0,0,0,0.05);
     }
 
     .form-section-title {
@@ -352,15 +359,15 @@
         margin-bottom: 0.75rem;
     }
 
-    .row:last-child {
-        margin-bottom: 0;
-    }
-
-    /* Gutters personalizados */
-    .row.g-2>* {
-        padding-right: 0.5rem !important;
-        padding-left: 0.5rem !important;
-    }
+        .row:last-child {
+            margin-bottom: 0;
+        }
+
+        /* Gutters personalizados */
+        .row.g-2 > * {
+            padding-right: 0.5rem !important;
+            padding-left: 0.5rem !important;
+        }
 
     /* Margens bottom específicas */
     .mb-2 {
@@ -380,9 +387,9 @@
         display: block;
     }
 
-    .text-danger:empty {
-        display: none;
-    }
+        .text-danger:empty {
+            display: none;
+        }
 
     .alert-danger:empty {
         display: none;
@@ -461,7 +468,7 @@
 
     #estatisticasViagens .card:hover {
         transform: translateY(-2px);
-        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15) !important;
+        box-shadow: 0 4px 12px rgba(0,0,0,0.15) !important;
     }
 
     #estatisticasViagens h4 {
@@ -476,18 +483,18 @@
         font-size: 0.875rem !important;
     }
 
-    .table thead th {
-        padding: 0.5rem !important;
-        font-size: 0.875rem !important;
-        font-weight: 600 !important;
-        background-color: #f8f9fa !important;
-    }
-
-    .table tbody td {
-        padding: 0.375rem !important;
-        font-size: 0.875rem !important;
-        vertical-align: middle !important;
-    }
+        .table thead th {
+            padding: 0.5rem !important;
+            font-size: 0.875rem !important;
+            font-weight: 600 !important;
+            background-color: #f8f9fa !important;
+        }
+
+        .table tbody td {
+            padding: 0.375rem !important;
+            font-size: 0.875rem !important;
+            vertical-align: middle !important;
+        }
 
     /* Tabela responsiva */
     .table-responsive {
@@ -495,18 +502,78 @@
         overflow-x: auto;
     }
 
+    /* === PAGINAÇÃO DO DATATABLE === */
+
+    /* Botões de paginação - PADRÃO LARANJA FROTIX */
+    .dataTables_wrapper .dataTables_paginate .paginate_button,
+    .dataTables_wrapper .page-item .page-link {
+        color: #ffffff !important;
+        background: linear-gradient(135deg, #cc5200 0%, #b34700 100%) !important;
+        border: none !important;
+        border-radius: 0.25rem !important;
+        padding: 0.25rem 0.5rem !important;
+        margin: 0 2px !important;
+        box-shadow: 0 2px 4px rgba(204, 82, 0, 0.3) !important;
+    }
+
+    .dataTables_wrapper .dataTables_paginate .paginate_button:hover,
+    .dataTables_wrapper .page-item .page-link:hover {
+        color: #ffffff !important;
+        background: linear-gradient(135deg, #b34700 0%, #a03d00 100%) !important;
+        transform: translateY(-1px) !important;
+    }
+
+    .dataTables_wrapper .dataTables_paginate .paginate_button.current,
+    .dataTables_wrapper .dataTables_paginate .paginate_button.active,
+    .dataTables_wrapper .page-item.active .page-link {
+        color: #fff !important;
+        background: linear-gradient(135deg, #a03d00 0%, #8a3300 100%) !important;
+        font-weight: bold !important;
+        border: 1px solid rgba(255,255,255,0.2) !important;
+    }
+
+    .dataTables_wrapper .dataTables_paginate .paginate_button.disabled,
+    .dataTables_wrapper .page-item.disabled .page-link {
+        color: #6c757d !important;
+        background: #e9ecef !important;
+        border-color: #dee2e6 !important;
+        cursor: not-allowed !important;
+        opacity: 0.8 !important;
+        box-shadow: none !important;
+    }
+
+    /* Força cor branca nos links internos */
+    .dataTables_wrapper .dataTables_paginate .paginate_button *,
+    .dataTables_wrapper .page-item .page-link * {
+        color: #ffffff !important;
+    }
+
+    /* Info de paginação (ex: "Mostrando 1 a 10 de 25 registros") */
+    .dataTables_wrapper .dataTables_info,
+    .dataTables_wrapper .dataTables_length label,
+    .dataTables_wrapper .dataTables_filter label {
+        color: #495057 !important;
+        padding-top: 0.75rem !important;
+        font-size: 0.875rem !important;
+    }
+
+    /* Container da paginação */
+    .dataTables_wrapper .dataTables_paginate {
+        padding-top: 0.75rem !important;
+    }
+
     /* === ALINHAMENTO VERTICAL === */
 
     /* Flex para colunas - garante alinhamento */
-    .row>[class*="col-"] {
+    .row > [class*="col-"] {
         display: flex;
         flex-direction: column;
     }
 
-    /* Labels alinhados ao topo */
-    .row>[class*="col-"] .form-label {
-        align-self: flex-start;
-    }
+        /* Labels alinhados ao topo */
+        .row > [class*="col-"] .form-label {
+            align-self: flex-start;
+        }
 
     /* === BORDAS E DIVISORES === */
 
@@ -570,12 +637,12 @@
         color: #495057 !important;
     }
 
-    /* Placeholder mais visível para campo readonly */
-    #txtSetorRequisitante[readonly]::placeholder {
-        color: #6c757d !important;
-        opacity: 1 !important;
-        font-style: italic;
-    }
+        /* Placeholder mais visível para campo readonly */
+        #txtSetorRequisitante[readonly]::placeholder {
+            color: #6c757d !important;
+            opacity: 1 !important;
+            font-style: italic;
+        }
 
     /* Garante que o campo tenha a mesma altura dos outros controles */
     #txtSetorRequisitante {
@@ -601,6 +668,7 @@
     #ddlSetorRequisitanteEvento option[value^="nivel0"] {
         font-weight: bold;
     }
+
 </style>
 
 <form method="post" asp-action="Upsert" autocomplete="off">
@@ -631,238 +699,263 @@
 
                             @if (Model.EventoObj.Evento.EventoId != Guid.Empty)
                             {
-                                <input type="hidden" asp-for="EventoObj.Evento.EventoId" />
+                            <input type="hidden" asp-for="EventoObj.Evento.EventoId" />
                             }
 
-                            <div class="form-section">
-                                <div class="form-section-title">
-                                    <i class="fa-duotone fa-info-circle"></i> Informações Básicas
-                                </div>
-                                <div class="row g-2 mb-2">
-
-                                    <div class="col-12 col-md-4">
-                                        <label class="form-label mb-1 small fw-bold">
-                                            <i class="fa-duotone fa-input-text"></i> Nome do Evento
-                                        </label>
-                                        <input id="txtNomeEvento" class="form-control" asp-for="EventoObj.Evento.Nome"
-                                            placeholder="Digite o nome do evento" />
-                                        <span class="text-danger small"
-                                            asp-validation-for="EventoObj.Evento.Nome"></span>
+                <div class="form-section">
+                    <div class="form-section-title">
+                        <i class="fa-duotone fa-info-circle"></i> Informações Básicas
+                    </div>
+                    <div class="row g-2 mb-2">
+
+                        <div class="col-12 col-md-4">
+                            <label class="form-label mb-1 small fw-bold">
+                                <i class="fa-duotone fa-input-text"></i> Nome do Evento <span class="required-asterisk">*</span>
+                            </label>
+                            <input id="txtNomeEvento"
+                                   class="form-control"
+                                   asp-for="EventoObj.Evento.Nome"
+                                   maxlength="200"
+                                   placeholder="Digite o nome do evento" />
+                            <span class="text-danger small"
+                                  asp-validation-for="EventoObj.Evento.Nome"></span>
+                        </div>
+
+                        <div class="col-12 col-md-8">
+                            <label class="form-label mb-1 small fw-bold">
+                                <i class="fa-duotone fa-align-left"></i> Descrição <span class="required-asterisk">*</span>
+                            </label>
+                            <input id="txtDescricaoEvento"
+                                   class="form-control"
+                                   asp-for="EventoObj.Evento.Descricao"
+                                   maxlength="300"
+                                   placeholder="Descreva o evento" />
+                            <span class="text-danger small"
+                                  asp-validation-for="EventoObj.Evento.Descricao"></span>
+                        </div>
+                    </div>
+                </div>
+
+                <div class="form-section">
+                    <div class="form-section-title">
+                        <i class="fa-duotone fa-calendar-range"></i> Período e Detalhes
+                    </div>
+                    <div class="row g-2 mb-2">
+
+                        <div class="col-12 col-md-3">
+                            <label class="form-label mb-1 small fw-bold">
+                                <i class="fa-duotone fa-calendar-day"></i> Data Inicial <span class="required-asterisk">*</span>
+                            </label>
+                            <input id="txtDataInicialEvento"
+                                   class="form-control"
+                                   asp-for="EventoObj.Evento.DataInicial"
+                                   type="date" />
+                            <span class="text-danger small"
+                                  asp-validation-for="EventoObj.Evento.DataInicial"></span>
+                        </div>
+
+                        <div class="col-12 col-md-3">
+                            <label class="form-label mb-1 small fw-bold">
+                                <i class="fa-duotone fa-calendar-check"></i> Data Final <span class="required-asterisk">*</span>
+                            </label>
+                            <input id="txtDataFinalEvento"
+                                   class="form-control"
+                                   asp-for="EventoObj.Evento.DataFinal"
+                                   type="date" />
+                            <span class="text-danger small"
+                                  asp-validation-for="EventoObj.Evento.DataFinal"></span>
+                        </div>
+
+                        <div class="col-12 col-md-3">
+                            <label class="form-label mb-1 small fw-bold">
+                                <i class="fa-duotone fa-users"></i> Qtd. Participantes <span class="required-asterisk">*</span>
+                            </label>
+                            <input id="txtQtdParticipantes"
+                                   class="form-control"
+                                   asp-for="EventoObj.Evento.QtdParticipantes"
+                                   type="number"
+                                   min="0"
+                                   step="1"
+                                   placeholder="0" />
+                            <span class="text-danger small"
+                                  asp-validation-for="EventoObj.Evento.QtdParticipantes"></span>
+                        </div>
+
+                        <div class="col-12 col-md-3">
+                            <label class="form-label mb-1 small fw-bold">
+                                <i class="fa-duotone fa-toggle-on"></i> Status <span class="required-asterisk">*</span>
+                            </label>
+                            <select id="lstStatus"
+                                    class="form-select"
+                                    asp-for="EventoObj.Evento.Status">
+                                <option value="1">Ativo</option>
+                                <option value="0">Inativo</option>
+                            </select>
+                            <span class="text-danger small"
+                                  asp-validation-for="EventoObj.Evento.Status"></span>
+                        </div>
+                    </div>
+                </div>
+
+                <div class="form-section">
+                    <div class="form-section-title">
+                        <i class="fa-duotone fa-user-tie"></i> Responsáveis
+                    </div>
+                    <div class="row g-2 mb-2">
+
+                        <div class="col-12 col-md-6">
+                            <label class="form-label mb-1 small fw-bold">
+                                <i class="fa-duotone fa-user"></i> Requisitante do Evento <span class="required-asterisk">*</span>
+                            </label>
+                            <ejs-dropdowntree id="lstRequisitanteEvento"
+                                              class="form-select"
+                                              placeholder="Selecione um Requisitante"
+                                              showCheckBox="false"
+                                              allowMultiSelection="false"
+                                              allowFiltering="true"
+                                              filterType="Contains"
+                                              filterBarPlaceholder="Procurar..."
+                                              popupHeight="200px"
+                                              select="RequisitanteEventoValueChange"
+                                              change="RequisitanteEventoValueChange"
+                                              ejs-for="@Model.EventoObj.Evento.RequisitanteId">
+                                <e-dropdowntree-fields dataSource="@ViewData["dataRequisitante"]"
+                                                       value="RequisitanteId"
+                                                       text="Requisitante">
+                                </e-dropdowntree-fields>
+                            </ejs-dropdowntree>
+                            <span class="text-danger small"
+                                  asp-validation-for="EventoObj.Evento.RequisitanteId"></span>
+                        </div>
+
+                        <div class="col-12 col-md-6">
+                            <label class="form-label mb-1 small fw-bold">
+                                <i class="fa-duotone fa-building"></i> Setor do Requisitante <span class="required-asterisk">*</span>
+                            </label>
+
+                            <input type="text"
+                                   id="txtSetorRequisitante"
+                                   class="form-control"
+                                   readonly
+                                   placeholder="Setor será preenchido automaticamente"
+                                   value="" />
+
+                            <span class="text-danger small"
+                                  asp-validation-for="EventoObj.Evento.SetorSolicitanteId"></span>
+                        </div>
+                    </div>
+                </div>
+
+                <div class="row g-3 mt-3">
+                    <div class="col-12 col-md-3">
+                        <button type="submit"
+                                id="btnSalvarEvento"
+                                value="Submit"
+                                asp-page-handler="Submit"
+                                class="btn btn-azul btn-submit-spin w-100">
+                            <span class="btn-content">
+                                <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
+                                @(Model.EventoObj.Evento.EventoId != Guid.Empty ? "Atualizar Evento" : "Criar Evento")
+                            </span>
+                            <span class="btn-loading d-none">
+                                <span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
+                                Salvando...
+                            </span>
+                        </button>
+                    </div>
+                    <div class="col-12 col-md-3">
+                        <a asp-page="./ListaEventos"
+                           class="btn btn-vinho w-100" data-ftx-loading>
+                            <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i>
+                            Cancelar Operação
+                        </a>
+                    </div>
+                </div>
+
+                <div class="col-12 col-md-6" style="display: none">
+                    <label class="form-label mb-1 small fw-bold">Setor do Requisitante</label>
+                    <select id="ddlSetorRequisitanteEvento"
+                            class="form-select"
+                            asp-for="@Model.EventoObj.Evento.SetorSolicitanteId"
+                            asp-items="@(new SelectList(ViewData["dataSetor"] as List<UpsertEventoModel.SetorListItem>, "SetorSolicitanteId", "NomeFormatado"))">
+                        <option value="">Selecione um Setor</option>
+                    </select>
+                    <span class="text-danger small"
+                          asp-validation-for="EventoObj.Evento.SetorSolicitanteId"></span>
+                </div>
+
+                @if (Model.EventoObj.Evento.EventoId != Guid.Empty)
+                {
+                    <div class="row mt-4">
+                        <div class="col-12">
+                            <h5 class="fs-5 mb-2">
+                                <i class="fa-duotone fa-route text-primary"></i> Viagens associadas ao Evento
+                            </h5>
+                            <div class="table-responsive">
+                                <table id="tblViagens" class="table table-striped table-bordered table-hover table-sm w-100">
+                                    <thead class="table-primary">
+                                        <tr>
+                                            <th>Vistoria</th>
+                                            <th>Data</th>
+                                            <th>Início</th>
+                                            <th>Requisitante</th>
+                                            <th>Setor</th>
+                                            <th>Motorista</th>
+                                            <th>Veículo</th>
+                                            <th class="text-end">Custo</th>
+                                            <th class="text-center" style="width:120px">Ações</th>
+                                        </tr>
+                                    </thead>
+                                    <tbody></tbody>
+                                </table>
+                            </div>
+                        </div>
+                    </div>
+
+                    <div class="row mt-4" id="estatisticasViagens">
+                        <div class="col-12">
+                            <h5 class="fs-5 mb-3">
+                                <i class="fa-duotone fa-chart-bar text-success"></i> Estatísticas das Viagens Realizadas
+                            </h5>
+                        </div>
+
+                        <div class="col-md-4">
+                            <div class="card border-0 shadow-sm h-100">
+                                <div class="card-body text-center py-3" style="background: linear-gradient(135deg, #e3f2fd 0%, #bbdefb 100%); border-radius: 8px;">
+                                    <div class="text-info mb-1">
+                                        <i class="fa-duotone fa-car fa-lg"></i>
                                     </div>
-
-                                    <div class="col-12 col-md-8">
-                                        <label class="form-label mb-1 small fw-bold">
-                                            <i class="fa-duotone fa-align-left"></i> Descrição
-                                        </label>
-                                        <input id="txtDescricaoEvento" class="form-control"
-                                            asp-for="EventoObj.Evento.Descricao" placeholder="Descreva o evento" />
-                                        <span class="text-danger small"
-                                            asp-validation-for="EventoObj.Evento.Descricao"></span>
-                                    </div>
+                                    <small class="text-muted d-block mb-1">Total de Viagens</small>
+                                    <h4 class="mb-0 fw-bold text-dark" id="totalViagens">0</h4>
                                 </div>
                             </div>
-
-                            <div class="form-section">
-                                <div class="form-section-title">
-                                    <i class="fa-duotone fa-calendar-range"></i> Período e Detalhes
-                                </div>
-                                <div class="row g-2 mb-2">
-
-                                    <div class="col-12 col-md-3">
-                                        <label class="form-label mb-1 small fw-bold">
-                                            <i class="fa-duotone fa-calendar-day"></i> Data Inicial
-                                        </label>
-                                        <input id="txtDataInicialEvento" class="form-control"
-                                            asp-for="EventoObj.Evento.DataInicial" type="date" />
-                                        <span class="text-danger small"
-                                            asp-validation-for="EventoObj.Evento.DataInicial"></span>
+                        </div>
+
+                        <div class="col-md-4">
+                            <div class="card border-0 shadow-sm h-100">
+                                <div class="card-body text-center py-3" style="background: linear-gradient(135deg, #e8f5e9 0%, #c8e6c9 100%); border-radius: 8px;">
+                                    <div class="text-success mb-1">
+                                        <i class="fa-duotone fa-dollar-sign fa-lg"></i>
                                     </div>
-
-                                    <div class="col-12 col-md-3">
-                                        <label class="form-label mb-1 small fw-bold">
-                                            <i class="fa-duotone fa-calendar-check"></i> Data Final
-                                        </label>
-                                        <input id="txtDataFinalEvento" class="form-control"
-                                            asp-for="EventoObj.Evento.DataFinal" type="date" />
-                                        <span class="text-danger small"
-                                            asp-validation-for="EventoObj.Evento.DataFinal"></span>
-                                    </div>
-
-                                    <div class="col-12 col-md-3">
-                                        <label class="form-label mb-1 small fw-bold">
-                                            <i class="fa-duotone fa-users"></i> Qtd. Participantes
-                                        </label>
-                                        <input id="txtQtdParticipantes" class="form-control"
-                                            asp-for="EventoObj.Evento.QtdParticipantes" type="number" min="0" step="1"
-                                            placeholder="0" />
-                                        <span class="text-danger small"
-                                            asp-validation-for="EventoObj.Evento.QtdParticipantes"></span>
-                                    </div>
-
-                                    <div class="col-12 col-md-3">
-                                        <label class="form-label mb-1 small fw-bold">
-                                            <i class="fa-duotone fa-toggle-on"></i> Status
-                                        </label>
-                                        <select id="lstStatus" class="form-select" asp-for="EventoObj.Evento.Status">
-                                            <option value="1">Ativo</option>
-                                            <option value="0">Inativo</option>
-                                        </select>
-                                        <span class="text-danger small"
-                                            asp-validation-for="EventoObj.Evento.Status"></span>
-                                    </div>
+                                    <small class="text-muted d-block mb-1">Valor Total do Evento</small>
+                                    <h4 class="mb-0 fw-bold text-dark" id="custoTotalViagens">R$ 0,00</h4>
                                 </div>
                             </div>
-
-                            <div class="form-section">
-                                <div class="form-section-title">
-                                    <i class="fa-duotone fa-user-tie"></i> Responsáveis
-                                </div>
-                                <div class="row g-2 mb-2">
-
-                                    <div class="col-12 col-md-6">
-                                        <label class="form-label mb-1 small fw-bold">
-                                            <i class="fa-duotone fa-user"></i> Requisitante do Evento
-                                        </label>
-                                        <ejs-dropdowntree id="lstRequisitanteEvento" class="form-select"
-                                            placeholder="Selecione um Requisitante" showCheckBox="false"
-                                            allowMultiSelection="false" allowFiltering="true" filterType="Contains"
-                                            filterBarPlaceholder="Procurar..." popupHeight="200px"
-                                            change="RequisitanteEventoValueChange"
-                                            ejs-for="@Model.EventoObj.Evento.RequisitanteId">
-                                            <e-dropdowntree-fields dataSource="@ViewData["dataRequisitante"]"
-                                                value="RequisitanteId" text="Requisitante">
-                                            </e-dropdowntree-fields>
-                                        </ejs-dropdowntree>
-                                        <span class="text-danger small"
-                                            asp-validation-for="EventoObj.Evento.RequisitanteId"></span>
+                        </div>
+
+                        <div class="col-md-4">
+                            <div class="card border-0 shadow-sm h-100">
+                                <div class="card-body text-center py-3" style="background: linear-gradient(135deg, #fff3e0 0%, #ffe0b2 100%); border-radius: 8px;">
+                                    <div class="text-warning mb-1">
+                                        <i class="fa-duotone fa-chart-line fa-lg"></i>
                                     </div>
-
-                                    <div class="col-12 col-md-6">
-                                        <label class="form-label mb-1 small fw-bold">
-                                            <i class="fa-duotone fa-building"></i> Setor do Requisitante
-                                        </label>
-
-                                        <input type="text" id="txtSetorRequisitante" class="form-control" readonly
-                                            placeholder="Setor será preenchido automaticamente" value="" />
-
-                                        <span class="text-danger small"
-                                            asp-validation-for="EventoObj.Evento.SetorSolicitanteId"></span>
-                                    </div>
+                                    <small class="text-muted d-block mb-1">Custo Médio por Viagem</small>
+                                    <h4 class="mb-0 fw-bold text-dark" id="custoMedioViagem">R$ 0,00</h4>
                                 </div>
                             </div>
-
-                            <div class="row g-3 mt-3">
-                                <div class="col-12 col-md-3">
-                                    <button type="submit" id="btnSalvarEvento" value="Submit" asp-page-handler="Submit"
-                                        class="btn btn-azul btn-submit-spin w-100">
-                                        <span class="btn-content">
-                                            <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
-                                            @(Model.EventoObj.Evento.EventoId != Guid.Empty ? "Atualizar Evento" :
-                                                                                        "Criar Evento")
-                                        </span>
-                                        <span class="btn-loading d-none">
-                                            <span class="spinner-border spinner-border-sm me-2" role="status"
-                                                aria-hidden="true"></span>
-                                            Salvando...
-                                        </span>
-                                    </button>
-                                </div>
-                                <div class="col-12 col-md-3">
-                                    <a asp-page="./ListaEventos" class="btn btn-vinho w-100" data-ftx-loading>
-                                        <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i>
-                                        Cancelar Operação
-                                    </a>
-                                </div>
-                            </div>
-
-                            <div class="col-12 col-md-6" style="display: none">
-                                <label class="form-label mb-1 small fw-bold">Setor do Requisitante</label>
-                                <select id="ddlSetorRequisitanteEvento" class="form-select"
-                                    asp-for="@Model.EventoObj.Evento.SetorSolicitanteId"
-                                    asp-items="@(new SelectList(ViewData["dataSetor"] as List<UpsertEventoModel.SetorListItem>, "SetorSolicitanteId", "NomeFormatado"))">
-                                    <option value="">Selecione um Setor</option>
-                                </select>
-                                <span class="text-danger small"
-                                    asp-validation-for="EventoObj.Evento.SetorSolicitanteId"></span>
-                            </div>
-
-                            @if (Model.EventoObj.Evento.EventoId != Guid.Empty)
-                            {
-                                <div class="row mt-4">
-                                    <div class="col-12">
-                                        <h5 class="fs-5 mb-2">
-                                            <i class="fa-duotone fa-route text-primary"></i> Viagens associadas ao Evento
-                                        </h5>
-                                        <div class="table-responsive">
-                                            <table id="tblViagens"
-                                                class="table table-striped table-bordered table-hover table-sm w-100">
-                                                <thead class="table-primary">
-                                                    <tr>
-                                                        <th>Vistoria</th>
-                                                        <th>Data</th>
-                                                        <th>Início</th>
-                                                        <th>Requisitante</th>
-                                                        <th>Setor</th>
-                                                        <th>Motorista</th>
-                                                        <th>Veículo</th>
-                                                        <th class="text-end">Custo</th>
-                                                        <th class="text-center" style="width:120px">Ações</th>
-                                                    </tr>
-                                                </thead>
-                                                <tbody></tbody>
-                                            </table>
-                                        </div>
-                                    </div>
-                                </div>
-
-                                <div class="row mt-4" id="estatisticasViagens">
-                                    <div class="col-12">
-                                        <h5 class="fs-5 mb-3">
-                                            <i class="fa-duotone fa-chart-bar text-success"></i> Estatísticas das Viagens
-                                            Realizadas
-                                        </h5>
-                                    </div>
-
-                                    <div class="col-md-4">
-                                        <div class="card border-0 shadow-sm h-100">
-                                            <div class="card-body text-center py-3"
-                                                style="background: linear-gradient(135deg, #e3f2fd 0%, #bbdefb 100%); border-radius: 8px;">
-                                                <div class="text-info mb-1">
-                                                    <i class="fa-duotone fa-car fa-lg"></i>
-                                                </div>
-                                                <small class="text-muted d-block mb-1">Total de Viagens</small>
-                                                <h4 class="mb-0 fw-bold text-dark" id="totalViagens">0</h4>
-                                            </div>
-                                        </div>
-                                    </div>
-
-                                    <div class="col-md-4">
-                                        <div class="card border-0 shadow-sm h-100">
-                                            <div class="card-body text-center py-3"
-                                                style="background: linear-gradient(135deg, #e8f5e9 0%, #c8e6c9 100%); border-radius: 8px;">
-                                                <div class="text-success mb-1">
-                                                    <i class="fa-duotone fa-dollar-sign fa-lg"></i>
-                                                </div>
-                                                <small class="text-muted d-block mb-1">Valor Total do Evento</small>
-                                                <h4 class="mb-0 fw-bold text-dark" id="custoTotalViagens">R$ 0,00</h4>
-                                            </div>
-                                        </div>
-                                    </div>
-
-                                    <div class="col-md-4">
-                                        <div class="card border-0 shadow-sm h-100">
-                                            <div class="card-body text-center py-3"
-                                                style="background: linear-gradient(135deg, #fff3e0 0%, #ffe0b2 100%); border-radius: 8px;">
-                                                <div class="text-warning mb-1">
-                                                    <i class="fa-duotone fa-chart-line fa-lg"></i>
-                                                </div>
-                                                <small class="text-muted d-block mb-1">Custo Médio por Viagem</small>
-                                                <h4 class="mb-0 fw-bold text-dark" id="custoMedioViagem">R$ 0,00</h4>
-                                            </div>
-                                        </div>
-                                    </div>
-                                </div>
-                            }
+                        </div>
+                    </div>
+                }
 
                         </div>
                     </div>
@@ -873,24 +966,21 @@
 
     <div id="loadingOverlayEvento" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
         <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-            <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-                style="display: block;" />
+            <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
             <div class="ftx-loading-bar"></div>
             <div class="ftx-loading-text" id="loadingMessage">Carregando...</div>
             <div class="ftx-loading-subtext">Aguarde, por favor</div>
         </div>
     </div>
 
-    <div class="modal fade" id="modalCustosViagem" tabindex="-1" aria-labelledby="modalCustosViagemLabel"
-        aria-hidden="true">
+    <div class="modal fade" id="modalCustosViagem" tabindex="-1" aria-labelledby="modalCustosViagemLabel" aria-hidden="true">
         <div class="modal-dialog modal-lg modal-dialog-centered">
             <div class="modal-content">
                 <div class="modal-header modal-header-dinheiro">
                     <h5 class="modal-title text-white" id="modalCustosViagemLabel">
                         <i class="fa-duotone fa-file-invoice-dollar"></i> Detalhamento de Custos da Viagem
                     </h5>
-                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                        aria-label="Fechar"></button>
+                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
                 </div>
                 <div class="modal-body">
 
@@ -967,8 +1057,7 @@
 
                     <div class="row">
                         <div class="col-12">
-                            <div class="card text-white"
-                                style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);">
+                            <div class="card text-white" style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);">
                                 <div class="card-body text-center py-4">
                                     <i class="fa-duotone fa-sack-dollar fa-3x mb-2"></i>
                                     <h5 class="mb-2">Custo Total da Viagem</h5>
@@ -989,24 +1078,21 @@
         </div>
     </div>
 
-    <div class="modal fade" id="modalDesassociar" tabindex="-1" aria-labelledby="modalDesassociarLabel"
-        aria-hidden="true">
+    <div class="modal fade" id="modalDesassociar" tabindex="-1" aria-labelledby="modalDesassociarLabel" aria-hidden="true">
         <div class="modal-dialog modal-dialog-centered">
             <div class="modal-content">
                 <div class="modal-header modal-header-vinho">
                     <h5 class="modal-title text-white" id="modalDesassociarLabel">
                         <i class="fa-duotone fa-link-slash"></i> Desassociar Viagem do Evento
                     </h5>
-                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                        aria-label="Fechar"></button>
+                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
                 </div>
                 <div class="modal-body">
                     <input type="hidden" id="viagemIdDesassociar" value="" />
 
                     <div class="alert alert-warning mb-3">
                         <i class="fa-duotone fa-triangle-exclamation me-2"></i>
-                        <strong>Atenção!</strong> Ao desassociar a viagem deste evento, você deverá informar uma nova
-                        finalidade para ela.
+                        <strong>Atenção!</strong> Ao desassociar a viagem deste evento, você deverá informar uma nova finalidade para ela.
                     </div>
 
                     <div class="mb-3">
@@ -1070,269 +1156,358 @@
 
 @section ScriptsBlock {
 
-    @using System.Text.Json
-
-<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-    crossorigin="anonymous" />
-<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-    crossorigin="anonymous"></script>
-
-    <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * UPSERT EVENTO - FUNÇÕES DE LOADING
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Funções de controle de overlay de loading para formulário de evento.
-         * @@file Viagens / UpsertEvento.cshtml
-            */
-
-        /**
-         * Exibe o overlay de loading padrão FrotiX
-         * @@param { string } [mensagem = 'Carregando...'] - Texto exibido durante o loading
-            */
-        function mostrarLoading(mensagem = 'Carregando...') {
-            try {
-                $('#loadingMessage').text(mensagem);
-                $('#loadingOverlayEvento').css('display', 'flex');
-            } catch (error) {
-                console.error('Erro ao mostrar loading:', error);
+@using System.Text.Json
+
+<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
+
+<script>
+
+    /***
+     * ⚡ FUNÇÃO: mostrarLoading
+     * --------------------------------------------------------------------------------------
+     * 🎯 OBJETIVO : Exibir overlay de loading com mensagem customizável
+     *
+     * 📥 ENTRADAS : mensagem [string] - Texto a exibir (padrão: 'Carregando...')
+     *
+     * 📤 SAÍDAS : void - Modifica visibilidade do #loadingOverlayEvento
+     *
+     * ⬅️ CHAMADO POR : Formulário submit [linha 1319]
+     * Carregamento de viagens [linha 1331]
+     *
+     * ➡️ CHAMA : jQuery $('#loadingMessage', '#loadingOverlayEvento')
+     ***/
+    function mostrarLoading(mensagem = 'Carregando...') {
+        try {
+
+            $('#loadingMessage').text(mensagem);
+
+            $('#loadingOverlayEvento').css('display', 'flex');
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("UpsertEvento.cshtml", "mostrarLoading", error);
+        }
+    }
+
+    /***
+     * ⚡ FUNÇÃO: esconderLoading
+     * --------------------------------------------------------------------------------------
+     * 🎯 OBJETIVO : Ocultar overlay de loading após operação completa
+     *
+     * 📥 ENTRADAS : Nenhuma
+     *
+     * 📤 SAÍDAS : void - Modifica visibilidade do #loadingOverlayEvento
+     *
+     * ⬅️ CHAMADO POR : DataTable init check [linha 1342]
+     * Timeout de segurança [linha 1355]
+     *
+     * ➡️ CHAMA : jQuery('#loadingOverlayEvento')
+     ***/
+    function esconderLoading() {
+        try {
+
+            $('#loadingOverlayEvento').css('display', 'none');
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("UpsertEvento.cshtml", "esconderLoading", error);
+        }
+    }
+</script>
+
+<script type="text/javascript" src="~/js/cadastros/eventoupsert.js" asp-append-version="true"></script>
+
+<script>
+    $(document).ready(function() {
+        try {
+
+            /***
+             * ⚡ FUNÇÃO: validarDatas
+             * --------------------------------------------------------------------------------------
+             * 🎯 OBJETIVO : Validar que data final não é menor que data inicial
+             *
+             * 📥 ENTRADAS : Nenhuma (lê valores dos inputs #txtDataInicialEvento,
+             * #txtDataFinalEvento)
+             *
+             * 📤 SAÍDAS : boolean - true se datas válidas, false se data final < inicial
+             *
+             * ⬅️ CHAMADO POR : form.on('submit') [linha 1295]
+             *
+             * ➡️ CHAMA : jQuery('#txtDataInicialEvento', '#txtDataFinalEvento')
+             *
+             * 📝 OBSERVAÇÕES : [VALIDACAO] Regra de negócio: Data final não pode ser anterior
+             * à data inicial do evento. Retorna true para dados vazios (assume
+             * como válido - validação HTML5 fará o resto).
+             ***/
+            function validarDatas() {
+                try {
+                    const dataInicial = $('#txtDataInicialEvento').val();
+                    const dataFinal = $('#txtDataFinalEvento').val();
+
+                    if (dataInicial && dataFinal) {
+
+                        const dtInicial = new Date(dataInicial);
+                        const dtFinal = new Date(dataFinal);
+
+                        if (dtFinal < dtInicial) {
+                            return false;
+                        }
+                    }
+                    return true;
+                } catch (error) {
+                    Alerta.TratamentoErroComLinha('UpsertEvento.cshtml', 'validarDatas', error);
+                    return true;
+                }
             }
-        }
-
-        /**
-         * Oculta o overlay de loading padrão FrotiX
-         */
-        function esconderLoading() {
-            try {
-                $('#loadingOverlayEvento').css('display', 'none');
-            } catch (error) {
-                console.error('Erro ao esconder loading:', error);
-            }
-        }
-    </script>
-
-    <script type="text/javascript" src="~/js/cadastros/eventoupsert.js" asp-append-version="true"></script>
-
-    <script>
-        $(document).ready(function () {
-            try {
-                /**
-                 * Valida se a data final é maior ou igual à data inicial
-                 * @@returns { boolean } True se válido, false se data final é anterior
-                    */
-                function validarDatas() {
+
+            $('#txtDataFinalEvento').on('change', function() {
+                try {
+                    const dataInicial = $('#txtDataInicialEvento').val();
+                    const dataFinal = $(this).val();
+
+                    if (dataInicial && dataFinal) {
+                        const dtInicial = new Date(dataInicial);
+                        const dtFinal = new Date(dataFinal);
+
+                        if (dtFinal < dtInicial) {
+                            AppToast.show('Vermelho', 'Data Final não pode ser menor que a Data Inicial!', 3000);
+                            $(this).val('');
+                            $(this).focus();
+                        }
+                    }
+                } catch (error) {
+                    Alerta.TratamentoErroComLinha('UpsertEvento.cshtml', 'txtDataFinalEvento.change', error);
+                }
+            });
+
+            $('#txtDataInicialEvento').on('change', function() {
+                try {
+                    const dataInicial = $(this).val();
+                    const dataFinal = $('#txtDataFinalEvento').val();
+
+                    if (dataInicial && dataFinal) {
+                        const dtInicial = new Date(dataInicial);
+                        const dtFinal = new Date(dataFinal);
+
+                        if (dtFinal < dtInicial) {
+                            AppToast.show('Amarelo', 'Atenção: Data Final foi limpa pois era menor que a nova Data Inicial.', 3000);
+                            $('#txtDataFinalEvento').val('');
+                        }
+                    }
+                } catch (error) {
+                    Alerta.TratamentoErroComLinha('UpsertEvento.cshtml', 'txtDataInicialEvento.change', error);
+                }
+            });
+
+            $('form').on('submit', function(e) {
+                try {
+                    const btn = $('#btnSalvarEvento');
+                    const form = $(this);
+
+                    if (!validarDatas()) {
+                        e.preventDefault();
+                        AppToast.show('Vermelho', 'Data Final não pode ser menor que a Data Inicial!', 3000);
+                        $('#txtDataFinalEvento').focus();
+                        return false;
+                    }
+
+                    if (form[0].checkValidity && !form[0].checkValidity()) {
+                        return true;
+                    }
+
+                    btn.prop('disabled', true);
+                    btn.find('.btn-content').addClass('d-none');
+                    btn.find('.btn-loading').removeClass('d-none');
+
+                    mostrarLoading('Salvando Evento...');
+
+                } catch (error) {
+                    console.error('Erro no submit:', error);
+                }
+            });
+
+            @if (Model.EventoObj.Evento.EventoId != Guid.Empty)
+            {
+                <text>
+
+                mostrarLoading('Carregando Viagens...');
+
+                var checkDataTable = setInterval(function() {
                     try {
-                        const dataInicial = $('#txtDataInicialEvento').val();
-                        const dataFinal = $('#txtDataFinalEvento').val();
-
-                        if (dataInicial && dataFinal) {
-                            const dtInicial = new Date(dataInicial);
-                            const dtFinal = new Date(dataFinal);
-
-                            if (dtFinal < dtInicial) {
-                                return false;
+                        if ($.fn.DataTable.isDataTable('#tblViagens')) {
+                            var dt = $('#tblViagens').DataTable();
+
+                            if (dt.data().count() >= 0) {
+                                clearInterval(checkDataTable);
+                                setTimeout(function() {
+                                    esconderLoading();
+                                }, 300);
                             }
                         }
-                        return true;
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha('UpsertEvento.cshtml', 'validarDatas', error);
-                        return true;
+                    } catch (e) {
+                        clearInterval(checkDataTable);
+                        esconderLoading();
+                    }
+                }, 200);
+
+                setTimeout(function() {
+                    clearInterval(checkDataTable);
+                    esconderLoading();
+                }, 5000);
+
+                const setorId = '@Model.EventoObj.Evento.SetorSolicitanteId';
+                if (setorId && setorId !== '00000000-0000-0000-0000-000000000000') {
+                    const ddlSetor = document.getElementById('ddlSetorRequisitanteEvento');
+                    const txtSetor = document.getElementById('txtSetorRequisitante');
+                    if (ddlSetor && ddlSetor.value && txtSetor) {
+                        const opcaoSelecionada = ddlSetor.options[ddlSetor.selectedIndex];
+                        if (opcaoSelecionada) {
+                            let textoSetor = opcaoSelecionada.text
+                                .replace(/^[–\s├└│]+/g, '')
+                                .trim();
+                            txtSetor.value = textoSetor;
+                        }
                     }
                 }
-
-                $('#txtDataFinalEvento').on('change', function () {
-                    try {
-                        const dataInicial = $('#txtDataInicialEvento').val();
-                        const dataFinal = $(this).val();
-
-                        if (dataInicial && dataFinal) {
-                            const dtInicial = new Date(dataInicial);
-                            const dtFinal = new Date(dataFinal);
-
-                            if (dtFinal < dtInicial) {
-                                AppToast.show('Vermelho', 'Data Final não pode ser menor que a Data Inicial!', 3000);
-                                $(this).val('');
-                                $(this).focus();
+                </text>
+            }
+
+        } catch (error) {
+            console.error('Erro no document.ready:', error);
+        }
+    });
+
+    const eventoId = @Html.Raw(JsonSerializer.Serialize(@Model.EventoObj.Evento?.EventoId.ToString()));
+    const requisitanteId = @Html.Raw(JsonSerializer.Serialize(@Model.EventoObj.Evento?.RequisitanteId.ToString()));
+    const setorsolicitanteId = @Html.Raw(JsonSerializer.Serialize(@Model.EventoObj.Evento?.SetorSolicitanteId.ToString()));
+
+    window.eventoId = eventoId;
+
+    /***
+     * ⚡ FUNÇÃO: RequisitanteEventoValueChange
+     * --------------------------------------------------------------------------------------
+     * 🎯 OBJETIVO : Preencher automaticamente o setor quando requisitante é selecionado
+     * no DropDownTree (lstRequisitanteEvento)
+     *
+     * 📥 ENTRADAS : args [Syncfusion DropDownTree EventArgs] - Contém nodeData/itemData
+     * com ID do requisitante selecionado
+     *
+     * 📤 SAÍDAS : void - Atualiza #txtSetorRequisitante com setor e #ddlSetorRequisitanteEvento
+     * com ID do setor (oculto)
+     *
+     * ⬅️ CHAMADO POR : DropDownTree select/change event [linha 834-835]
+     * Event bind manual [linha 1490-1491]
+     *
+     * ➡️ CHAMA : GET /Viagens/UpsertEvento?handler=PegaSetor [AJAX]
+     * jQuery('#txtSetorRequisitante', '#ddlSetorRequisitanteEvento')
+     *
+     * 📝 OBSERVAÇÕES : [AJAX] Chamada ao servidor para buscar setor via requisitanteId.
+     * Múltiplas estratégias de extração de ID (nodeData, itemData, value,
+     * dataset.uid). Fallback para leitura direta do componente Syncfusion.
+     ***/
+    window.RequisitanteEventoValueChange = function (args) {
+        try {
+            console.log('Event Triggered: RequisitanteEventoValueChange', args);
+
+            let requisitanteId = null;
+
+            if (args) {
+                if (args.nodeData && args.nodeData.id) {
+                    requisitanteId = args.nodeData.id;
+                } else if (args.itemData && (args.itemData.value || args.itemData.RequisitanteId)) {
+                    requisitanteId = args.itemData.value || args.itemData.RequisitanteId;
+                } else if (args.value) {
+
+                    requisitanteId = Array.isArray(args.value) ? args.value[0] : args.value;
+                } else if (args.item && args.item.dataset && args.item.dataset.uid) {
+
+                    requisitanteId = args.item.dataset.uid;
+                }
+            }
+
+            if (!requisitanteId) {
+                const ddTreeObj = document.getElementById("lstRequisitanteEvento")?.ej2_instances?.[0];
+                if (ddTreeObj && ddTreeObj.value) {
+                    requisitanteId = Array.isArray(ddTreeObj.value) ? ddTreeObj.value[0] : ddTreeObj.value;
+                }
+            }
+
+            console.log('ID Identificado:', requisitanteId);
+
+            if (!requisitanteId) {
+                console.warn('Nenhum ID de requisitante válido encontrado. Aguardando interação...');
+                return;
+            }
+
+            const txtSetor = document.getElementById('txtSetorRequisitante');
+
+            if (txtSetor) {
+                txtSetor.value = 'Carregando Setor...';
+                txtSetor.classList.add('pulse-input');
+            }
+
+            /***
+             * [AJAX] Endpoint: GET /Viagens/UpsertEvento?handler=PegaSetor
+             * ============================================================================
+             * 📥 ENVIA : id [Guid] - ID do requisitante selecionado
+             * 📤 RECEBE : { success: bool, data: setorId, setorNome: string }
+             * 🎯 MOTIVO : Buscar setor vinculado ao requisitante para popular campo
+             * readonly txtSetorRequisitante e dropdown oculto
+             ***/
+            $.ajax({
+                url: "/Viagens/UpsertEvento?handler=PegaSetor",
+                method: "GET",
+                dataType: "json",
+                data: { id: requisitanteId },
+                success: function (res) {
+                    if (res.success && res.data) {
+
+                        const ddlSetor = document.getElementById('ddlSetorRequisitanteEvento');
+                        if (ddlSetor) {
+                            ddlSetor.value = res.data;
+
+                            const event = new Event('change', { bubbles: true });
+                            ddlSetor.dispatchEvent(event);
+
+                            const opcaoSelecionada = ddlSetor.options[ddlSetor.selectedIndex];
+                            let textoSetor = opcaoSelecionada ? opcaoSelecionada.text : res.setorNome;
+
+                            if (textoSetor) {
+                                textoSetor = textoSetor.replace(/^[–\s├└│]+/g, '').trim();
                             }
+
+                            if (txtSetor) {
+                                txtSetor.value = textoSetor;
+                                txtSetor.classList.remove('pulse-input');
+                                txtSetor.classList.add('is-valid');
+                                setTimeout(() => txtSetor.classList.remove('is-valid'), 1000);
+                            }
+
+                            console.log('✓ Setor Atualizado com Sucesso:', textoSetor);
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha('UpsertEvento.cshtml', 'txtDataFinalEvento.change', error);
+                    } else {
+                        if (txtSetor) txtSetor.value = '';
+                        console.warn('Setor não encontrado na resposta:', res);
                     }
-                });
-
-                $('#txtDataInicialEvento').on('change', function () {
-                    try {
-                        const dataInicial = $(this).val();
-                        const dataFinal = $('#txtDataFinalEvento').val();
-
-                        if (dataInicial && dataFinal) {
-                            const dtInicial = new Date(dataInicial);
-                            const dtFinal = new Date(dataFinal);
-
-                            if (dtFinal < dtInicial) {
-                                AppToast.show('Amarelo', 'Atenção: Data Final foi limpa pois era menor que a nova Data Inicial.', 3000);
-                                $('#txtDataFinalEvento').val('');
-                            }
-                        }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha('UpsertEvento.cshtml', 'txtDataInicialEvento.change', error);
-                    }
-                });
-
-                $('form').on('submit', function (e) {
-                    try {
-                        const btn = $('#btnSalvarEvento');
-                        const form = $(this);
-
-                        if (!validarDatas()) {
-                            e.preventDefault();
-                            AppToast.show('Vermelho', 'Data Final não pode ser menor que a Data Inicial!', 3000);
-                            $('#txtDataFinalEvento').focus();
-                            return false;
-                        }
-
-                        if (form[0].checkValidity && !form[0].checkValidity()) {
-                            return true;
-                        }
-
-                        btn.prop('disabled', true);
-                        btn.find('.btn-content').addClass('d-none');
-                        btn.find('.btn-loading').removeClass('d-none');
-
-                        mostrarLoading('Salvando Evento...');
-
-                    } catch (error) {
-                        console.error('Erro no submit:', error);
-                    }
-                });
-
-            @if (Model.EventoObj.Evento.EventoId != Guid.Empty)
-                    {
-                        <text>
-
-                            mostrarLoading('Carregando Viagens...');
-
-                            var checkDataTable = setInterval(function() {
-                                    try {
-                                        if ($.fn.DataTable.isDataTable('#tblViagens')) {
-                                            var dt = $('#tblViagens').DataTable();
-
-                                            if (dt.data().count() >= 0) {
-                                clearInterval(checkDataTable);
-                            setTimeout(function() {
-                                esconderLoading();
-                                                }, 300);
-                                            }
-                                        }
-                                    } catch (e) {
-                                clearInterval(checkDataTable);
-                            esconderLoading();
-                                    }
-                                }, 200);
-
-                            setTimeout(function() {
-                                clearInterval(checkDataTable);
-                            esconderLoading();
-                                }, 5000);
-
-                            const setorId = '@Model.EventoObj.Evento.SetorSolicitanteId';
-                            if (setorId && setorId !== '00000000-0000-0000-0000-000000000000') {
-                                    const ddlSetor = document.getElementById('ddlSetorRequisitanteEvento');
-                            const txtSetor = document.getElementById('txtSetorRequisitante');
-                            if (ddlSetor && ddlSetor.value && txtSetor) {
-                                        const opcaoSelecionada = ddlSetor.options[ddlSetor.selectedIndex];
-                            if (opcaoSelecionada) {
-                                let textoSetor = opcaoSelecionada.text
-                            .replace(/^[–\s├└│]+/g, '')
-                            .trim();
-                            txtSetor.value = textoSetor;
-                                        }
-                                    }
-                                }
-                        </text>
+                },
+                error: function (xhr, status, error) {
+                    console.error('Erro AJAX ao buscar setor:', error);
+                    if (txtSetor) txtSetor.value = 'Erro ao buscar setor';
                 }
-
-                } catch (error) {
-                console.error('Erro no document.ready:', error);
+            });
+
+        } catch (error) {
+            console.error('Erro Crítico em RequisitanteEventoValueChange:', error);
+        }
+    };
+
+    $(document).ready(function () {
+        try {
+            const ddtReq = document.getElementById("lstRequisitanteEvento")?.ej2_instances?.[0];
+            if (ddtReq) {
+                ddtReq.select = window.RequisitanteEventoValueChange;
+                ddtReq.change = window.RequisitanteEventoValueChange;
             }
-        });
-
-        const eventoId = @Html.Raw(JsonSerializer.Serialize(@Model.EventoObj.Evento?.EventoId.ToString()));
-        const requisitanteId = @Html.Raw(JsonSerializer.Serialize(@Model.EventoObj.Evento?.RequisitanteId.ToString()));
-        const setorsolicitanteId = @Html.Raw(JsonSerializer.Serialize(@Model.EventoObj.Evento?.SetorSolicitanteId.ToString()));
-
-        window.eventoId = eventoId;
-
-        /**
-         * Callback de mudança no ComboBox de requisitantes do evento
-         * @@description Busca e preenche automaticamente o setor do requisitante selecionado
-         */
-        function RequisitanteEventoValueChange() {
-            try {
-                const ddTreeObj = document.getElementById("lstRequisitanteEvento")?.ej2_instances?.[0];
-
-                if (!ddTreeObj || !ddTreeObj.value || ddTreeObj.value.length === 0) {
-
-                    const ddlSetor = document.getElementById('ddlSetorRequisitanteEvento');
-                    const txtSetor = document.getElementById('txtSetorRequisitante');
-
-                    if (ddlSetor) ddlSetor.value = '';
-                    if (txtSetor) txtSetor.value = '';
-                    return;
-                }
-
-                const requisitanteId = String(ddTreeObj.value[0] || ddTreeObj.value);
-                const txtSetor = document.getElementById('txtSetorRequisitante');
-
-                if (txtSetor) {
-                    txtSetor.value = 'Carregando...';
-                }
-
-                $.ajax({
-                    url: "/Viagens/UpsertEvento?handler=PegaSetor",
-                    method: "GET",
-                    dataType: "json",
-                    data: { id: requisitanteId },
-                    success: function (res) {
-                        if (res.success && res.data) {
-
-                            const ddlSetor = document.getElementById('ddlSetorRequisitanteEvento');
-                            if (ddlSetor) {
-                                ddlSetor.value = res.data;
-
-                                ddlSetor.dispatchEvent(new Event('change'));
-
-                                const opcaoSelecionada = ddlSetor.options[ddlSetor.selectedIndex];
-                                let textoSetor = opcaoSelecionada ? opcaoSelecionada.text : res.setorNome;
-
-                                textoSetor = textoSetor
-                                    .replace(/^[–\s├└│]+/g, '')
-                                    .trim();
-
-                                if (txtSetor) {
-                                    txtSetor.value = textoSetor;
-                                }
-
-                                console.log('✓ Setor selecionado:', textoSetor);
-                            }
-                        } else {
-                            if (txtSetor) {
-                                txtSetor.value = '';
-                            }
-                            console.warn('Setor não encontrado:', res.message);
-                        }
-                    },
-                    error: function (xhr, status, error) {
-                        console.error('Erro ao buscar setor:', error);
-                        if (txtSetor) {
-                            txtSetor.value = 'Erro ao carregar';
-                        }
-                    }
-                });
-
-            } catch (error) {
-                console.error('Erro em RequisitanteEventoValueChange:', error);
-            }
-        }
-    </script>
+        } catch (error) {
+            console.error('Erro ao bindar eventos do requisitante:', error);
+        }
+    });
+</script>
 }
```
