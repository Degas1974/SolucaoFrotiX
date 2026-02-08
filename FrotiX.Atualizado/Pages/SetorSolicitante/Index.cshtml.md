# Pages/SetorSolicitante/Index.cshtml

**Mudanca:** GRANDE | **+634** linhas | **-666** linhas

---

```diff
--- JANEIRO: Pages/SetorSolicitante/Index.cshtml
+++ ATUAL: Pages/SetorSolicitante/Index.cshtml
@@ -18,7 +18,7 @@
 
 @section HeadBlock {
     <style>
-        /* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
+/* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
         .ftx-card-header .btn-fundo-laranja {
             outline: 2px solid rgba(255, 255, 255, 0.5) !important;
             outline-offset: 1px;
@@ -29,6 +29,7 @@
             outline: 2px solid rgba(255, 255, 255, 0.8) !important;
             outline-offset: 2px;
         }
+
     </style>
 }
 
@@ -51,12 +52,12 @@
         letter-spacing: 0.3px;
         gap: 0.35rem;
         transition: all 0.2s ease;
-        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.15) !important;
+        box-shadow: 0 2px 4px rgba(0,0,0,0.15) !important;
     }
 
     .ftx-badge-status:hover {
         transform: translateY(-1px);
-        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2) !important;
+        box-shadow: 0 4px 8px rgba(0,0,0,0.2) !important;
     }
 
     .ftx-badge-status i {
@@ -196,7 +197,7 @@
 
     .modal-header-azul .modal-title i {
         --fa-primary-color: #fff;
-        --fa-secondary-color: rgba(255, 255, 255, 0.6);
+        --fa-secondary-color: rgba(255,255,255,0.6);
     }
 
     /* Labels do formulário */
@@ -267,751 +268,718 @@
                             </div>
                         </div>
                     </div>
-                </div>
             </div>
         </div>
     </div>
-
-    <div class="modal fade" id="modalUpsert" tabindex="-1" aria-labelledby="modalUpsertLabel" aria-hidden="true">
-        <div class="modal-dialog modal-dialog-centered">
-            <div class="modal-content">
-                <div class="modal-header modal-header-azul">
-                    <h5 class="modal-title d-flex align-items-center gap-2" id="modalUpsertLabel">
-                        <i class="fa-duotone fa-building"></i>
-                        <span id="modalTitulo">Novo Setor Solicitante</span>
-                    </h5>
-                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
+</div>
+
+<div class="modal fade" id="modalUpsert" tabindex="-1" aria-labelledby="modalUpsertLabel" aria-hidden="true">
+    <div class="modal-dialog modal-dialog-centered">
+        <div class="modal-content">
+            <div class="modal-header modal-header-azul">
+                <h5 class="modal-title d-flex align-items-center gap-2" id="modalUpsertLabel">
+                    <i class="fa-duotone fa-building"></i>
+                    <span id="modalTitulo">Novo Setor Solicitante</span>
+                </h5>
+                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
+            </div>
+
+            <div class="modal-body">
+                <input type="hidden" id="hiddenSetorId" />
+
+                <div class="mb-3">
+                    <label class="ftx-label">
+                        <i class="fa-duotone fa-input-text"></i>
+                        Nome do Setor <span class="text-danger">*</span>
+                    </label>
+                    <input type="text" id="txtNome" class="form-control" placeholder="Digite o nome do setor" maxlength="200" />
                 </div>
 
-                <div class="modal-body">
-                    <input type="hidden" id="hiddenSetorId" />
-
-                    <div class="mb-3">
-                        <label class="ftx-label">
-                            <i class="fa-duotone fa-input-text"></i>
-                            Nome do Setor <span class="text-danger">*</span>
-                        </label>
-                        <input type="text" id="txtNome" class="form-control" placeholder="Digite o nome do setor"
-                            maxlength="200" />
-                    </div>
-
-                    <div class="mb-3">
-                        <label class="ftx-label">
-                            <i class="fa-duotone fa-tag"></i>
-                            Sigla
-                        </label>
-                        <input type="text" id="txtSigla" class="form-control" placeholder="Ex: DTI, DRH"
-                            maxlength="50" />
-                    </div>
-
-                    <div class="mb-3">
-                        <label class="ftx-label">
-                            <i class="fa-duotone fa-phone"></i>
-                            Ramal
-                        </label>
-                        <input type="number" id="txtRamal" class="form-control" placeholder="Ex: 1234" />
-                    </div>
-
-                    <div class="mb-3">
-                        <label class="ftx-label">
-                            <i class="fa-duotone fa-sitemap"></i>
-                            Setor Pai (Hierarquia)
-                        </label>
-                        <input type="text" id="ddlSetorPai" />
-                    </div>
-
-                    <div class="mb-3">
-                        <label class="ftx-label">
-                            <i class="fa-duotone fa-toggle-on"></i>
-                            Status
-                        </label>
-                        <div class="form-check form-switch">
-                            <input class="form-check-input" type="checkbox" id="chkStatus" checked
-                                style="width: 3rem; height: 1.5rem;">
-                            <label class="form-check-label ms-2" for="chkStatus" id="lblStatus">Ativo</label>
-                        </div>
+                <div class="mb-3">
+                    <label class="ftx-label">
+                        <i class="fa-duotone fa-tag"></i>
+                        Sigla
+                    </label>
+                    <input type="text" id="txtSigla" class="form-control" placeholder="Ex: DTI, DRH" maxlength="50" />
+                </div>
+
+                <div class="mb-3">
+                    <label class="ftx-label">
+                        <i class="fa-duotone fa-phone"></i>
+                        Ramal
+                    </label>
+                    <input type="number" id="txtRamal" class="form-control" placeholder="Ex: 1234" />
+                </div>
+
+                <div class="mb-3">
+                    <label class="ftx-label">
+                        <i class="fa-duotone fa-sitemap"></i>
+                        Setor Pai (Hierarquia)
+                    </label>
+                    <input type="text" id="ddlSetorPai" />
+                </div>
+
+                <div class="mb-3">
+                    <label class="ftx-label">
+                        <i class="fa-duotone fa-toggle-on"></i>
+                        Status
+                    </label>
+                    <div class="form-check form-switch">
+                        <input class="form-check-input" type="checkbox" id="chkStatus" checked style="width: 3rem; height: 1.5rem;">
+                        <label class="form-check-label ms-2" for="chkStatus" id="lblStatus">Ativo</label>
                     </div>
                 </div>
-
-                <div class="modal-footer">
-                    <button type="button" class="btn btn-vinho" data-bs-dismiss="modal">
-                        <i class="fa-duotone fa-circle-xmark me-1"
-                            style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i> Cancelar
-                    </button>
-                    <button type="button" class="btn btn-azul" id="btnSalvarSetor">
-                        <i class="fa-duotone fa-floppy-disk me-1 btn-icon"
-                            style="--fa-primary-color:#fff; --fa-secondary-color:#90caf9;"></i>
-                        <span class="btn-text">Salvar</span>
-                        <i class="fa-duotone fa-spinner-third fa-spin btn-spinner"
-                            style="display:none; --fa-primary-color:#fff; --fa-secondary-color:#90caf9;"></i>
-                    </button>
-                </div>
+            </div>
+
+            <div class="modal-footer">
+                <button type="button" class="btn btn-vinho" data-bs-dismiss="modal">
+                    <i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i> Cancelar
+                </button>
+                <button type="button" class="btn btn-azul" id="btnSalvarSetor">
+                    <i class="fa-duotone fa-floppy-disk me-1 btn-icon" style="--fa-primary-color:#fff; --fa-secondary-color:#90caf9;"></i>
+                    <span class="btn-text">Salvar</span>
+                    <i class="fa-duotone fa-spinner-third fa-spin btn-spinner" style="display:none; --fa-primary-color:#fff; --fa-secondary-color:#90caf9;"></i>
+                </button>
             </div>
         </div>
     </div>
-
-    <div id="loadingOverlaySetor" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
-        <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-            <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-                style="display: block;" />
-            <div class="ftx-loading-bar"></div>
-            <div class="ftx-loading-text">Carregando Setores...</div>
-            <div class="ftx-loading-subtext">Aguarde, por favor</div>
-        </div>
+</div>
+
+<div id="loadingOverlaySetor" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
+    <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
+        <div class="ftx-loading-bar"></div>
+        <div class="ftx-loading-text">Carregando Setores...</div>
+        <div class="ftx-loading-subtext">Aguarde, por favor</div>
     </div>
+</div>
 
 @section ScriptsBlock {
-        <script asp-append-version="true">
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * SETORES SOLICITANTES - GESTÃO HIERÁRQUICA
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Módulo de gestão de setores solicitantes com estrutura hierárquica,
-             * TreeGrid para visualização e modal para CRUD.
-             * @@requires jQuery, Syncfusion EJ2 TreeGrid / DropDownTree, Bootstrap Modal
-                * @@file SetorSolicitante / Index.cshtml
-                */
-
-            var modalUpsert = null;
-            var treeGridObj = null;
-            var dropDownTreeSetorPai = null;
-            var cacheSetoresHierarquicos = null;
-
-            /**
-             * Exibe o overlay de loading padrão FrotiX
-             */
-            function mostrarLoading() {
-                $('#loadingOverlaySetor').css('display', 'flex');
-            }
-
-            /**
-             * Oculta o overlay de loading padrão FrotiX
-             */
-            function esconderLoading() {
-                $('#loadingOverlaySetor').css('display', 'none');
-            }
-
-            /**
-             * Inicializa o DropDownTree de seleção de setor pai
-             * @@description Cria componente Syncfusion para seleção hierárquica de setor pai
-                */
-            function inicializarDropDownTreeSetorPai() {
-                try {
-                    if (dropDownTreeSetorPai) {
-                        return;
-                    }
-
-                    dropDownTreeSetorPai = new ej.dropdowns.DropDownTree({
-                        width: '100%',
-                        placeholder: '-- Nenhum (Setor Raiz) --',
-                        showClearButton: true,
-                        allowFiltering: true,
-                        filterType: 'Contains',
-                        filterBarPlaceholder: 'Buscar setor...',
-                        allowMultiSelection: false,
-                        popupHeight: '320px',
-                        fields: {
-                            dataSource: [],
+    <script asp-append-version="true">
+        var modalUpsert = null;
+        var treeGridObj = null;
+        var dropDownTreeSetorPai = null;
+        var cacheSetoresHierarquicos = null;
+
+        function mostrarLoading() {
+            $('#loadingOverlaySetor').css('display', 'flex');
+        }
+
+        function esconderLoading() {
+            $('#loadingOverlaySetor').css('display', 'none');
+        }
+
+        function inicializarDropDownTreeSetorPai() {
+            try {
+                if (dropDownTreeSetorPai) {
+                    return;
+                }
+
+                dropDownTreeSetorPai = new ej.dropdowns.DropDownTree({
+                    width: '100%',
+                    placeholder: '-- Nenhum (Setor Raiz) --',
+                    showClearButton: true,
+                    allowFiltering: true,
+                    filterType: 'Contains',
+                    filterBarPlaceholder: 'Buscar setor...',
+                    allowMultiSelection: false,
+                    popupHeight: '320px',
+                    fields: {
+                        dataSource: [],
+                        value: 'setorSolicitanteId',
+                        text: 'nome',
+                        child: 'children',
+                        disabled: 'disabled'
+                    },
+                    change: function (args) {
+                        try {
+                            if (Array.isArray(args.value) && args.value.length) {
+                                dropDownTreeSetorPai.value = args.value[0];
+                            }
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Index.cshtml", "dropDownTreeSetorPai.change", error);
+                        }
+                    }
+                });
+
+                dropDownTreeSetorPai.appendTo('#ddlSetorPai');
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "inicializarDropDownTreeSetorPai", error);
+            }
+        }
+
+        function prepararDadosHierarquicos(lista, excludeId) {
+            try {
+                if (!Array.isArray(lista)) return [];
+
+                return lista.map(function (item) {
+                    var novoItem = {
+                        setorSolicitanteId: item.setorSolicitanteId,
+                        setorPaiId: item.setorPaiId,
+                        nome: item.nome,
+                        sigla: item.sigla,
+                        ramal: item.ramal,
+                        status: item.status
+                    };
+
+                    if (item.children && Array.isArray(item.children) && item.children.length > 0) {
+                        novoItem.children = prepararDadosHierarquicos(item.children, excludeId);
+                    }
+
+                    if (excludeId && item.setorSolicitanteId === excludeId) {
+                        novoItem.disabled = true;
+                    }
+
+                    return novoItem;
+                });
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "prepararDadosHierarquicos", error);
+                return [];
+            }
+        }
+
+        $(document).ready(function () {
+            try {
+
+                modalUpsert = new bootstrap.Modal(document.getElementById('modalUpsert'));
+
+                $('#chkStatus').on('change', function () {
+                    try {
+                        $('#lblStatus').text($(this).is(':checked') ? 'Ativo' : 'Inativo');
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Index.cshtml", "chkStatus.change", error);
+                    }
+                });
+
+                $('#btnAdicionarSetor').on('click', function () {
+                    try {
+                        abrirModalNovo();
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Index.cshtml", "btnAdicionarSetor.click", error);
+                    }
+                });
+
+                $('#btnSalvarSetor').on('click', function () {
+                    try {
+                        salvarSetor();
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Index.cshtml", "btnSalvarSetor.click", error);
+                    }
+                });
+
+                $(document).on('click', '.btn-editar-setor', function () {
+                    try {
+                        var id = $(this).data('id');
+                        if (id) {
+                            abrirModalEditar(id);
+                        }
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Index.cshtml", "btn-editar-setor.click", error);
+                    }
+                });
+
+                $(document).on('click', '.btn-excluir-setor', function () {
+                    try {
+                        var id = $(this).data('id');
+                        if (id) {
+                            excluirSetor(id);
+                        }
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Index.cshtml", "btn-excluir-setor.click", error);
+                    }
+                });
+
+                $(document).on('click', '.btn-toggle-status', function () {
+                    try {
+                        var id = $(this).data('id');
+                        var currentStatus = $(this).data('status');
+                        if (id) {
+                            alternarStatus(id, currentStatus, $(this));
+                        }
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Index.cshtml", "btn-toggle-status.click", error);
+                    }
+                });
+
+                carregarSetores();
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "document.ready", error);
+            }
+        });
+
+        function carregarSetores() {
+            try {
+                mostrarLoading();
+                $.ajax({
+                    url: "/api/SetorSolicitante/GetAll",
+                    type: "GET",
+                    dataType: "json",
+                    success: function (data) {
+                        try {
+                            inicializarTreeGrid(data);
+                            esconderLoading();
+                        } catch (error) {
+                            esconderLoading();
+                            Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetores.success", error);
+                        }
+                    },
+                    error: function (err) {
+                        try {
+                            esconderLoading();
+                            console.log(err);
+                            AppToast.show('Vermelho', 'Erro ao carregar setores.', 2000);
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetores.error", error);
+                        }
+                    }
+                });
+            } catch (error) {
+                esconderLoading();
+                Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetores", error);
+            }
+        }
+
+        function inicializarTreeGrid(dados) {
+            try {
+                $('#TreeGrid').empty();
+
+                treeGridObj = new ej.treegrid.TreeGrid({
+                    dataSource: dados,
+                    childMapping: 'children',
+                    treeColumnIndex: 1,
+                    allowPaging: true,
+                    allowSorting: true,
+                    enableHover: true,
+                    locale: 'pt-BR',
+                    toolbar: ['Search', 'ExpandAll', 'CollapseAll'],
+                    pageSettings: { pageSize: 15 },
+                    sortSettings: {
+                        columns: [{ field: 'nome', direction: 'Ascending' }]
+                    },
+                    queryCellInfo: queryCellInfo,
+                    columns: [
+                        {
+                            field: 'setorSolicitanteId',
+                            headerText: 'ID',
+                            visible: false,
+                            width: 0
+                        },
+                        {
+                            field: 'nome',
+                            headerText: 'Nome',
+                            width: 250
+                        },
+                        {
+                            field: 'sigla',
+                            headerText: 'Sigla',
+                            textAlign: 'Center',
+                            width: 100
+                        },
+                        {
+                            field: 'ramal',
+                            headerText: 'Ramal',
+                            textAlign: 'Center',
+                            width: 80
+                        },
+                        {
+                            field: 'status',
+                            headerText: 'Status',
+                            textAlign: 'Center',
+                            width: 100
+                        },
+                        {
+                            headerText: 'Ações',
+                            textAlign: 'Center',
+                            width: 120,
+                            field: 'acoes'
+                        }
+                    ]
+                });
+                treeGridObj.appendTo('#TreeGrid');
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "inicializarTreeGrid", error);
+            }
+        }
+
+        function queryCellInfo(args) {
+            try {
+
+                if (args.column.field === 'status') {
+                    var statusVal = args.data.status;
+                    var id = args.data.setorSolicitanteId;
+                    if (statusVal === 1 || statusVal === true || statusVal === "1") {
+                        args.cell.innerHTML = '<span class="ftx-badge-status ftx-badge-ativo ftx-badge-clickable btn-toggle-status" data-id="' + id + '" data-status="1" title="Clique para desativar">' +
+                            '<i class="fa-duotone fa-circle-check" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i>' +
+                            'Ativo</span>';
+                    } else {
+                        args.cell.innerHTML = '<span class="ftx-badge-status ftx-badge-inativo ftx-badge-clickable btn-toggle-status" data-id="' + id + '" data-status="0" title="Clique para ativar">' +
+                            '<i class="fa-duotone fa-circle-xmark" style="--fa-primary-color:#fff; --fa-secondary-color:#adb5bd;"></i>' +
+                            'Inativo</span>';
+                    }
+                }
+
+                if (args.column.field === 'acoes') {
+                    var id = args.data.setorSolicitanteId;
+                    args.cell.innerHTML =
+                        '<div class="d-flex justify-content-center gap-1">' +
+                            '<button type="button" class="btn-acao-ftx btn-azul btn-editar-setor" data-id="' + id + '" title="Editar">' +
+                                '<i class="fa-duotone fa-pen-to-square" style="--fa-primary-color:#fff; --fa-secondary-color:#90caf9;"></i>' +
+                            '</button>' +
+                            '<button type="button" class="btn-acao-ftx btn-vinho btn-excluir-setor" data-id="' + id + '" title="Excluir">' +
+                                '<i class="fa-duotone fa-trash-can" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i>' +
+                            '</button>' +
+                        '</div>';
+                }
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "queryCellInfo", error);
+            }
+        }
+
+        function carregarSetoresPai(excludeId, selectedValue) {
+            try {
+                inicializarDropDownTreeSetorPai();
+
+                var aplicarDados = function (dadosBase) {
+                    try {
+                        if (!dropDownTreeSetorPai) return;
+                        var dataPreparada = prepararDadosHierarquicos(dadosBase || [], excludeId);
+
+                        dropDownTreeSetorPai.fields = {
+                            dataSource: dataPreparada,
                             value: 'setorSolicitanteId',
                             text: 'nome',
                             child: 'children',
                             disabled: 'disabled'
-                        },
-                        change: function (args) {
-                            try {
-                                if (Array.isArray(args.value) && args.value.length) {
-                                    dropDownTreeSetorPai.value = args.value[0];
+                        };
+
+                        if (selectedValue) {
+                            dropDownTreeSetorPai.value = selectedValue;
+                        } else {
+                            dropDownTreeSetorPai.value = null;
+                            dropDownTreeSetorPai.text = '';
+                        }
+
+                        dropDownTreeSetorPai.dataBind();
+
+                        if (!selectedValue && typeof dropDownTreeSetorPai.clear === 'function') {
+                            dropDownTreeSetorPai.clear();
+                        }
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetoresPai.applyData", error);
+                    }
+                };
+
+                if (cacheSetoresHierarquicos) {
+                    aplicarDados(cacheSetoresHierarquicos);
+                    return;
+                }
+
+                $.ajax({
+                    url: "/api/SetorSolicitante/GetAll",
+                    type: "GET",
+                    dataType: "json",
+                    success: function (data) {
+                        try {
+                            cacheSetoresHierarquicos = data || [];
+                            aplicarDados(cacheSetoresHierarquicos);
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetoresPai.success", error);
+                        }
+                    },
+                    error: function (err) {
+                        try {
+                            console.log(err);
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetoresPai.error", error);
+                        }
+                    }
+                });
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetoresPai", error);
+            }
+        }
+
+        function abrirModalNovo() {
+            try {
+                $('#modalTitulo').text('Novo Setor Solicitante');
+                $('#hiddenSetorId').val('');
+                $('#txtNome').val('');
+                $('#txtSigla').val('');
+                $('#txtRamal').val('');
+                $('#chkStatus').prop('checked', true);
+                $('#lblStatus').text('Ativo');
+
+                carregarSetoresPai(null, null);
+                modalUpsert.show();
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "abrirModalNovo", error);
+            }
+        }
+
+        function abrirModalEditar(id) {
+            try {
+                if (!id) return;
+
+                $.ajax({
+                    url: "/api/SetorSolicitante/GetById?id=" + id,
+                    type: "GET",
+                    dataType: "json",
+                    success: function (response) {
+                        try {
+                            if (response.success) {
+                                var setor = response.data;
+                                $('#modalTitulo').text('Editar Setor Solicitante');
+                                $('#hiddenSetorId').val(setor.setorSolicitanteId);
+                                $('#txtNome').val(setor.nome);
+                                $('#txtSigla').val(setor.sigla);
+                                $('#txtRamal').val(setor.ramal || '');
+                                $('#chkStatus').prop('checked', setor.status);
+                                $('#lblStatus').text(setor.status ? 'Ativo' : 'Inativo');
+
+                                carregarSetoresPai(setor.setorSolicitanteId, setor.setorPaiId || null);
+
+                                setTimeout(function () {
+                                    $('#ddlSetorPai').val(setor.setorPaiId || '');
+                                }, 300);
+
+                                modalUpsert.show();
+                            } else {
+                                AppToast.show('Vermelho', response.message, 2000);
+                            }
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Index.cshtml", "abrirModalEditar.success", error);
+                        }
+                    },
+                    error: function (err) {
+                        try {
+                            console.log(err);
+                            AppToast.show('Vermelho', 'Erro ao carregar dados do setor.', 2000);
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Index.cshtml", "abrirModalEditar.error", error);
+                        }
+                    }
+                });
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "abrirModalEditar", error);
+            }
+        }
+
+        function salvarSetor() {
+            try {
+                var nome = $('#txtNome').val().trim();
+                if (!nome) {
+                    AppToast.show('Amarelo', 'O nome do setor é obrigatório.', 2000);
+                    $('#txtNome').focus();
+                    return;
+                }
+
+                var model = {
+                    SetorSolicitanteId: $('#hiddenSetorId').val() || '',
+                    Nome: nome,
+                    Sigla: $('#txtSigla').val().trim(),
+                    Ramal: $('#txtRamal').val() ? parseInt($('#txtRamal').val()) : null,
+                    SetorPaiId: $('#ddlSetorPai').val() || '',
+                    Status: $('#chkStatus').is(':checked')
+                };
+
+                var btnSalvar = $('#btnSalvarSetor');
+                btnSalvar.prop('disabled', true);
+                btnSalvar.find('.btn-icon').hide();
+                btnSalvar.find('.btn-text').css('visibility', 'hidden');
+                btnSalvar.find('.btn-spinner').show();
+
+                $.ajax({
+                    url: "/api/SetorSolicitante/Upsert",
+                    type: "POST",
+                    data: JSON.stringify(model),
+                    contentType: "application/json; charset=utf-8",
+                    dataType: "json",
+                    success: function (response) {
+                        try {
+
+                            btnSalvar.prop('disabled', false);
+                            btnSalvar.find('.btn-icon').show();
+                            btnSalvar.find('.btn-text').css('visibility', 'visible');
+                            btnSalvar.find('.btn-spinner').hide();
+
+                            if (response.success) {
+                                AppToast.show('Verde', response.message, 2000);
+                                modalUpsert.hide();
+                                location.reload();
+                            } else {
+                                AppToast.show('Vermelho', response.message, 2000);
+                            }
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Index.cshtml", "salvarSetor.success", error);
+                        }
+                    },
+                    error: function (err) {
+                        try {
+
+                            btnSalvar.prop('disabled', false);
+                            btnSalvar.find('.btn-icon').show();
+                            btnSalvar.find('.btn-text').css('visibility', 'visible');
+                            btnSalvar.find('.btn-spinner').hide();
+
+                            console.log(err);
+                            AppToast.show('Vermelho', 'Erro ao salvar setor.', 2000);
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Index.cshtml", "salvarSetor.error", error);
+                        }
+                    }
+                });
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "salvarSetor", error);
+            }
+        }
+
+        function alternarStatus(id, currentStatus, element) {
+            try {
+                if (!id) return;
+
+                var novoStatus = currentStatus == 1 ? false : true;
+
+                $.ajax({
+                    url: "/api/SetorSolicitante/UpdateStatus?id=" + id,
+                    type: "GET",
+                    dataType: "json",
+                    success: function (response) {
+                        try {
+                            if (response.success) {
+                                AppToast.show('Verde', 'Status alterado com sucesso!', 2000);
+
+                                if (response.novoStatus === 1 || response.novoStatus === true) {
+                                    element
+                                        .removeClass('ftx-badge-inativo')
+                                        .addClass('ftx-badge-ativo')
+                                        .data('status', 1)
+                                        .attr('title', 'Clique para desativar')
+                                        .html('<i class="fa-duotone fa-circle-check" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i>Ativo');
+                                } else {
+                                    element
+                                        .removeClass('ftx-badge-ativo')
+                                        .addClass('ftx-badge-inativo')
+                                        .data('status', 0)
+                                        .attr('title', 'Clique para ativar')
+                                        .html('<i class="fa-duotone fa-circle-xmark" style="--fa-primary-color:#fff; --fa-secondary-color:#adb5bd;"></i>Inativo');
                                 }
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("Index.cshtml", "dropDownTreeSetorPai.change", error);
+                            } else {
+                                AppToast.show('Vermelho', response.message || 'Erro ao alterar status.', 2000);
                             }
-                        }
-                    });
-
-                    dropDownTreeSetorPai.appendTo('#ddlSetorPai');
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "inicializarDropDownTreeSetorPai", error);
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Index.cshtml", "alternarStatus.success", error);
+                        }
+                    },
+                    error: function (err) {
+                        try {
+                            console.log(err);
+                            AppToast.show('Vermelho', 'Erro ao alterar status do setor.', 2000);
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Index.cshtml", "alternarStatus.error", error);
+                        }
+                    }
+                });
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "alternarStatus", error);
+            }
+        }
+
+        function excluirSetor(id) {
+            try {
+                if (!id) return;
+
+                Alerta.Confirmar(
+                    "Confirmar Exclusão",
+                    "Você tem certeza que deseja apagar este setor? Não será possível recuperar os dados eliminados!",
+                    "Excluir",
+                    "Cancelar"
+                ).then(function (confirmed) {
+                    try {
+                        if (confirmed) {
+                            var dataToPost = JSON.stringify({ "SetorSolicitanteId": id });
+                            $.ajax({
+                                url: "/api/SetorSolicitante/Delete",
+                                type: "POST",
+                                data: dataToPost,
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
+                                success: function (data) {
+                                    try {
+                                        if (data.success) {
+                                            AppToast.show('Verde', data.message, 2000);
+                                            location.reload();
+                                        } else {
+                                            AppToast.show('Vermelho', data.message, 2000);
+                                        }
+                                    } catch (error) {
+                                        Alerta.TratamentoErroComLinha("Index.cshtml", "excluirSetor.ajax.success", error);
+                                    }
+                                },
+                                error: function (err) {
+                                    try {
+                                        console.log(err);
+                                        AppToast.show('Vermelho', 'Ocorreu um erro ao excluir o registro.', 2000);
+                                    } catch (error) {
+                                        Alerta.TratamentoErroComLinha("Index.cshtml", "excluirSetor.ajax.error", error);
+                                    }
+                                }
+                            });
+                        }
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Index.cshtml", "excluirSetor.confirmar.then", error);
+                    }
+                });
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "excluirSetor", error);
+            }
+        }
+    </script>
+
+    <script asp-append-version="true">
+        var L10n = ej.base.L10n;
+        L10n.load({
+            "pt-BR": {
+                "treegrid": {
+                    "EmptyRecord": "Nenhum registro encontrado",
+                    "ExpandAll": "Expandir Tudo",
+                    "CollapseAll": "Recolher Tudo",
+                    "Print": "Imprimir",
+                    "Pdfexport": "Exportar para PDF",
+                    "Excelexport": "Exportar para Excel",
+                    "Wordexport": "Exportar para Word",
+                    "FilterButton": "Filtrar",
+                    "ClearButton": "Limpar",
+                    "StartsWith": "Começa com",
+                    "EndsWith": "Termina com",
+                    "Contains": "Contém",
+                    "Equal": "Igual",
+                    "NotEqual": "Diferente de",
+                    "LessThan": "Menor que",
+                    "LessThanOrEqual": "Menor ou igual a",
+                    "GreaterThan": "Maior que",
+                    "GreaterThanOrEqual": "Maior ou igual a",
+                    "EnterValue": "Informe um valor",
+                    "FilterMenu": "Menu de filtro",
+                    "Search": "Buscar"
+                },
+                "pager": {
+                    "currentPageInfo": "Página {0} de {1}",
+                    "totalItemsInfo": "({0} registro(s))",
+                    "firstPageTooltip": "Primeira página",
+                    "lastPageTooltip": "Última página",
+                    "nextPageTooltip": "Próxima página",
+                    "previousPageTooltip": "Página anterior",
+                    "nextPagerTooltip": "Próximo paginador",
+                    "previousPagerTooltip": "Paginador anterior"
+                },
+                "dropdowns": {
+                    "noRecordsTemplate": "Nenhum registro encontrado"
+                },
+                "datepicker": {
+                    "placeholder": "Selecione uma data",
+                    "today": "Hoje"
                 }
             }
-
-            /**
-             * Prepara dados para estrutura hierárquica do DropDownTree
-             * @@param { Array } lista - Lista de setores a processar
-                * @@param { string }[excludeId] - ID do setor a desabilitar(para evitar auto - referência)
-                    * @@returns { Array } Lista processada com estrutura hierárquica
-                        */
-            function prepararDadosHierarquicos(lista, excludeId) {
-                try {
-                    if (!Array.isArray(lista)) return [];
-
-                    return lista.map(function (item) {
-                        var novoItem = {
-                            setorSolicitanteId: item.setorSolicitanteId,
-                            setorPaiId: item.setorPaiId,
-                            nome: item.nome,
-                            sigla: item.sigla,
-                            ramal: item.ramal,
-                            status: item.status
-                        };
-
-                        if (item.children && Array.isArray(item.children) && item.children.length > 0) {
-                            novoItem.children = prepararDadosHierarquicos(item.children, excludeId);
-                        }
-
-                        if (excludeId && item.setorSolicitanteId === excludeId) {
-                            novoItem.disabled = true;
-                        }
-
-                        return novoItem;
-                    });
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "prepararDadosHierarquicos", error);
-                    return [];
-                }
-            }
-
-            $(document).ready(function () {
-                try {
-
-                    modalUpsert = new bootstrap.Modal(document.getElementById('modalUpsert'));
-
-                    $('#chkStatus').on('change', function () {
-                        try {
-                            $('#lblStatus').text($(this).is(':checked') ? 'Ativo' : 'Inativo');
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("Index.cshtml", "chkStatus.change", error);
-                        }
-                    });
-
-                    $('#btnAdicionarSetor').on('click', function () {
-                        try {
-                            abrirModalNovo();
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("Index.cshtml", "btnAdicionarSetor.click", error);
-                        }
-                    });
-
-                    $('#btnSalvarSetor').on('click', function () {
-                        try {
-                            salvarSetor();
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("Index.cshtml", "btnSalvarSetor.click", error);
-                        }
-                    });
-
-                    $(document).on('click', '.btn-editar-setor', function () {
-                        try {
-                            var id = $(this).data('id');
-                            if (id) {
-                                abrirModalEditar(id);
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("Index.cshtml", "btn-editar-setor.click", error);
-                        }
-                    });
-
-                    $(document).on('click', '.btn-excluir-setor', function () {
-                        try {
-                            var id = $(this).data('id');
-                            if (id) {
-                                excluirSetor(id);
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("Index.cshtml", "btn-excluir-setor.click", error);
-                        }
-                    });
-
-                    $(document).on('click', '.btn-toggle-status', function () {
-                        try {
-                            var id = $(this).data('id');
-                            var currentStatus = $(this).data('status');
-                            if (id) {
-                                alternarStatus(id, currentStatus, $(this));
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("Index.cshtml", "btn-toggle-status.click", error);
-                        }
-                    });
-
-                    carregarSetores();
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "document.ready", error);
-                }
-            });
-
-            function carregarSetores() {
-                try {
-                    mostrarLoading();
-                    $.ajax({
-                        url: "/api/SetorSolicitante/GetAll",
-                        type: "GET",
-                        dataType: "json",
-                        success: function (data) {
-                            try {
-                                inicializarTreeGrid(data);
-                                esconderLoading();
-                            } catch (error) {
-                                esconderLoading();
-                                Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetores.success", error);
-                            }
-                        },
-                        error: function (err) {
-                            try {
-                                esconderLoading();
-                                console.log(err);
-                                AppToast.show('Vermelho', 'Erro ao carregar setores.', 2000);
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetores.error", error);
-                            }
-                        }
-                    });
-                } catch (error) {
-                    esconderLoading();
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetores", error);
-                }
-            }
-
-            function inicializarTreeGrid(dados) {
-                try {
-                    $('#TreeGrid').empty();
-
-                    treeGridObj = new ej.treegrid.TreeGrid({
-                        dataSource: dados,
-                        childMapping: 'children',
-                        treeColumnIndex: 1,
-                        allowPaging: true,
-                        allowSorting: true,
-                        enableHover: true,
-                        locale: 'pt-BR',
-                        toolbar: ['Search', 'ExpandAll', 'CollapseAll'],
-                        pageSettings: { pageSize: 15 },
-                        sortSettings: {
-                            columns: [{ field: 'nome', direction: 'Ascending' }]
-                        },
-                        queryCellInfo: queryCellInfo,
-                        columns: [
-                            {
-                                field: 'setorSolicitanteId',
-                                headerText: 'ID',
-                                visible: false,
-                                width: 0
-                            },
-                            {
-                                field: 'nome',
-                                headerText: 'Nome',
-                                width: 250
-                            },
-                            {
-                                field: 'sigla',
-                                headerText: 'Sigla',
-                                textAlign: 'Center',
-                                width: 100
-                            },
-                            {
-                                field: 'ramal',
-                                headerText: 'Ramal',
-                                textAlign: 'Center',
-                                width: 80
-                            },
-                            {
-                                field: 'status',
-                                headerText: 'Status',
-                                textAlign: 'Center',
-                                width: 100
-                            },
-                            {
-                                headerText: 'Ações',
-                                textAlign: 'Center',
-                                width: 120,
-                                field: 'acoes'
-                            }
-                        ]
-                    });
-                    treeGridObj.appendTo('#TreeGrid');
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "inicializarTreeGrid", error);
-                }
-            }
-
-            function queryCellInfo(args) {
-                try {
-
-                    if (args.column.field === 'status') {
-                        var statusVal = args.data.status;
-                        var id = args.data.setorSolicitanteId;
-                        if (statusVal === 1 || statusVal === true || statusVal === "1") {
-                            args.cell.innerHTML = '<span class="ftx-badge-status ftx-badge-ativo ftx-badge-clickable btn-toggle-status" data-id="' + id + '" data-status="1" title="Clique para desativar">' +
-                                '<i class="fa-duotone fa-circle-check" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i>' +
-                                'Ativo</span>';
-                        } else {
-                            args.cell.innerHTML = '<span class="ftx-badge-status ftx-badge-inativo ftx-badge-clickable btn-toggle-status" data-id="' + id + '" data-status="0" title="Clique para ativar">' +
-                                '<i class="fa-duotone fa-circle-xmark" style="--fa-primary-color:#fff; --fa-secondary-color:#adb5bd;"></i>' +
-                                'Inativo</span>';
-                        }
-                    }
-
-                    if (args.column.field === 'acoes') {
-                        var id = args.data.setorSolicitanteId;
-                        args.cell.innerHTML =
-                            '<div class="d-flex justify-content-center gap-1">' +
-                            '<button type="button" class="btn-acao-ftx btn-azul btn-editar-setor" data-id="' + id + '" title="Editar">' +
-                            '<i class="fa-duotone fa-pen-to-square" style="--fa-primary-color:#fff; --fa-secondary-color:#90caf9;"></i>' +
-                            '</button>' +
-                            '<button type="button" class="btn-acao-ftx btn-vinho btn-excluir-setor" data-id="' + id + '" title="Excluir">' +
-                            '<i class="fa-duotone fa-trash-can" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i>' +
-                            '</button>' +
-                            '</div>';
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "queryCellInfo", error);
-                }
-            }
-
-            function carregarSetoresPai(excludeId, selectedValue) {
-                try {
-                    inicializarDropDownTreeSetorPai();
-
-                    var aplicarDados = function (dadosBase) {
-                        try {
-                            if (!dropDownTreeSetorPai) return;
-                            var dataPreparada = prepararDadosHierarquicos(dadosBase || [], excludeId);
-
-                            dropDownTreeSetorPai.fields = {
-                                dataSource: dataPreparada,
-                                value: 'setorSolicitanteId',
-                                text: 'nome',
-                                child: 'children',
-                                disabled: 'disabled'
-                            };
-
-                            if (selectedValue) {
-                                dropDownTreeSetorPai.value = selectedValue;
-                            } else {
-                                dropDownTreeSetorPai.value = null;
-                                dropDownTreeSetorPai.text = '';
-                            }
-
-                            dropDownTreeSetorPai.dataBind();
-
-                            if (!selectedValue && typeof dropDownTreeSetorPai.clear === 'function') {
-                                dropDownTreeSetorPai.clear();
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetoresPai.applyData", error);
-                        }
-                    };
-
-                    if (cacheSetoresHierarquicos) {
-                        aplicarDados(cacheSetoresHierarquicos);
-                        return;
-                    }
-
-                    $.ajax({
-                        url: "/api/SetorSolicitante/GetAll",
-                        type: "GET",
-                        dataType: "json",
-                        success: function (data) {
-                            try {
-                                cacheSetoresHierarquicos = data || [];
-                                aplicarDados(cacheSetoresHierarquicos);
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetoresPai.success", error);
-                            }
-                        },
-                        error: function (err) {
-                            try {
-                                console.log(err);
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetoresPai.error", error);
-                            }
-                        }
-                    });
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "carregarSetoresPai", error);
-                }
-            }
-
-            function abrirModalNovo() {
-                try {
-                    $('#modalTitulo').text('Novo Setor Solicitante');
-                    $('#hiddenSetorId').val('');
-                    $('#txtNome').val('');
-                    $('#txtSigla').val('');
-                    $('#txtRamal').val('');
-                    $('#chkStatus').prop('checked', true);
-                    $('#lblStatus').text('Ativo');
-
-                    carregarSetoresPai(null, null);
-                    modalUpsert.show();
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "abrirModalNovo", error);
-                }
-            }
-
-            function abrirModalEditar(id) {
-                try {
-                    if (!id) return;
-
-                    $.ajax({
-                        url: "/api/SetorSolicitante/GetById?id=" + id,
-                        type: "GET",
-                        dataType: "json",
-                        success: function (response) {
-                            try {
-                                if (response.success) {
-                                    var setor = response.data;
-                                    $('#modalTitulo').text('Editar Setor Solicitante');
-                                    $('#hiddenSetorId').val(setor.setorSolicitanteId);
-                                    $('#txtNome').val(setor.nome);
-                                    $('#txtSigla').val(setor.sigla);
-                                    $('#txtRamal').val(setor.ramal || '');
-                                    $('#chkStatus').prop('checked', setor.status);
-                                    $('#lblStatus').text(setor.status ? 'Ativo' : 'Inativo');
-
-                                    carregarSetoresPai(setor.setorSolicitanteId, setor.setorPaiId || null);
-
-                                    setTimeout(function () {
-                                        $('#ddlSetorPai').val(setor.setorPaiId || '');
-                                    }, 300);
-
-                                    modalUpsert.show();
-                                } else {
-                                    AppToast.show('Vermelho', response.message, 2000);
-                                }
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("Index.cshtml", "abrirModalEditar.success", error);
-                            }
-                        },
-                        error: function (err) {
-                            try {
-                                console.log(err);
-                                AppToast.show('Vermelho', 'Erro ao carregar dados do setor.', 2000);
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("Index.cshtml", "abrirModalEditar.error", error);
-                            }
-                        }
-                    });
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "abrirModalEditar", error);
-                }
-            }
-
-            function salvarSetor() {
-                try {
-                    var nome = $('#txtNome').val().trim();
-                    if (!nome) {
-                        AppToast.show('Amarelo', 'O nome do setor é obrigatório.', 2000);
-                        $('#txtNome').focus();
-                        return;
-                    }
-
-                    var model = {
-                        SetorSolicitanteId: $('#hiddenSetorId').val() || '',
-                        Nome: nome,
-                        Sigla: $('#txtSigla').val().trim(),
-                        Ramal: $('#txtRamal').val() ? parseInt($('#txtRamal').val()) : null,
-                        SetorPaiId: $('#ddlSetorPai').val() || '',
-                        Status: $('#chkStatus').is(':checked')
-                    };
-
-                    var btnSalvar = $('#btnSalvarSetor');
-                    btnSalvar.prop('disabled', true);
-                    btnSalvar.find('.btn-icon').hide();
-                    btnSalvar.find('.btn-text').css('visibility', 'hidden');
-                    btnSalvar.find('.btn-spinner').show();
-
-                    $.ajax({
-                        url: "/api/SetorSolicitante/Upsert",
-                        type: "POST",
-                        data: JSON.stringify(model),
-                        contentType: "application/json; charset=utf-8",
-                        dataType: "json",
-                        success: function (response) {
-                            try {
-
-                                btnSalvar.prop('disabled', false);
-                                btnSalvar.find('.btn-icon').show();
-                                btnSalvar.find('.btn-text').css('visibility', 'visible');
-                                btnSalvar.find('.btn-spinner').hide();
-
-                                if (response.success) {
-                                    AppToast.show('Verde', response.message, 2000);
-                                    modalUpsert.hide();
-                                    location.reload();
-                                } else {
-                                    AppToast.show('Vermelho', response.message, 2000);
-                                }
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("Index.cshtml", "salvarSetor.success", error);
-                            }
-                        },
-                        error: function (err) {
-                            try {
-
-                                btnSalvar.prop('disabled', false);
-                                btnSalvar.find('.btn-icon').show();
-                                btnSalvar.find('.btn-text').css('visibility', 'visible');
-                                btnSalvar.find('.btn-spinner').hide();
-
-                                console.log(err);
-                                AppToast.show('Vermelho', 'Erro ao salvar setor.', 2000);
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("Index.cshtml", "salvarSetor.error", error);
-                            }
-                        }
-                    });
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "salvarSetor", error);
-                }
-            }
-
-            function alternarStatus(id, currentStatus, element) {
-                try {
-                    if (!id) return;
-
-                    var novoStatus = currentStatus == 1 ? false : true;
-
-                    $.ajax({
-                        url: "/api/SetorSolicitante/UpdateStatus?id=" + id,
-                        type: "GET",
-                        dataType: "json",
-                        success: function (response) {
-                            try {
-                                if (response.success) {
-                                    AppToast.show('Verde', 'Status alterado com sucesso!', 2000);
-
-                                    if (response.novoStatus === 1 || response.novoStatus === true) {
-                                        element
-                                            .removeClass('ftx-badge-inativo')
-                                            .addClass('ftx-badge-ativo')
-                                            .data('status', 1)
-                                            .attr('title', 'Clique para desativar')
-                                            .html('<i class="fa-duotone fa-circle-check" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i>Ativo');
-                                    } else {
-                                        element
-                                            .removeClass('ftx-badge-ativo')
-                                            .addClass('ftx-badge-inativo')
-                                            .data('status', 0)
-                                            .attr('title', 'Clique para ativar')
-                                            .html('<i class="fa-duotone fa-circle-xmark" style="--fa-primary-color:#fff; --fa-secondary-color:#adb5bd;"></i>Inativo');
-                                    }
-                                } else {
-                                    AppToast.show('Vermelho', response.message || 'Erro ao alterar status.', 2000);
-                                }
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("Index.cshtml", "alternarStatus.success", error);
-                            }
-                        },
-                        error: function (err) {
-                            try {
-                                console.log(err);
-                                AppToast.show('Vermelho', 'Erro ao alterar status do setor.', 2000);
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("Index.cshtml", "alternarStatus.error", error);
-                            }
-                        }
-                    });
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "alternarStatus", error);
-                }
-            }
-
-            function excluirSetor(id) {
-                try {
-                    if (!id) return;
-
-                    Alerta.Confirmar(
-                        "Confirmar Exclusão",
-                        "Você tem certeza que deseja apagar este setor? Não será possível recuperar os dados eliminados!",
-                        "Excluir",
-                        "Cancelar"
-                    ).then(function (confirmed) {
-                        try {
-                            if (confirmed) {
-                                var dataToPost = JSON.stringify({ "SetorSolicitanteId": id });
-                                $.ajax({
-                                    url: "/api/SetorSolicitante/Delete",
-                                    type: "POST",
-                                    data: dataToPost,
-                                    contentType: "application/json; charset=utf-8",
-                                    dataType: "json",
-                                    success: function (data) {
-                                        try {
-                                            if (data.success) {
-                                                AppToast.show('Verde', data.message, 2000);
-                                                location.reload();
-                                            } else {
-                                                AppToast.show('Vermelho', data.message, 2000);
-                                            }
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha("Index.cshtml", "excluirSetor.ajax.success", error);
-                                        }
-                                    },
-                                    error: function (err) {
-                                        try {
-                                            console.log(err);
-                                            AppToast.show('Vermelho', 'Ocorreu um erro ao excluir o registro.', 2000);
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha("Index.cshtml", "excluirSetor.ajax.error", error);
-                                        }
-                                    }
-                                });
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("Index.cshtml", "excluirSetor.confirmar.then", error);
-                        }
-                    });
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "excluirSetor", error);
-                }
-            }
-        </script>
-
-        <script asp-append-version="true">
-            var L10n = ej.base.L10n;
-            L10n.load({
-                "pt-BR": {
-                    "treegrid": {
-                        "EmptyRecord": "Nenhum registro encontrado",
-                        "ExpandAll": "Expandir Tudo",
-                        "CollapseAll": "Recolher Tudo",
-                        "Print": "Imprimir",
-                        "Pdfexport": "Exportar para PDF",
-                        "Excelexport": "Exportar para Excel",
-                        "Wordexport": "Exportar para Word",
-                        "FilterButton": "Filtrar",
-                        "ClearButton": "Limpar",
-                        "StartsWith": "Começa com",
-                        "EndsWith": "Termina com",
-                        "Contains": "Contém",
-                        "Equal": "Igual",
-                        "NotEqual": "Diferente de",
-                        "LessThan": "Menor que",
-                        "LessThanOrEqual": "Menor ou igual a",
-                        "GreaterThan": "Maior que",
-                        "GreaterThanOrEqual": "Maior ou igual a",
-                        "EnterValue": "Informe um valor",
-                        "FilterMenu": "Menu de filtro",
-                        "Search": "Buscar"
-                    },
-                    "pager": {
-                        "currentPageInfo": "Página {0} de {1}",
-                        "totalItemsInfo": "({0} registro(s))",
-                        "firstPageTooltip": "Primeira página",
-                        "lastPageTooltip": "Última página",
-                        "nextPageTooltip": "Próxima página",
-                        "previousPageTooltip": "Página anterior",
-                        "nextPagerTooltip": "Próximo paginador",
-                        "previousPagerTooltip": "Paginador anterior"
-                    },
-                    "dropdowns": {
-                        "noRecordsTemplate": "Nenhum registro encontrado"
-                    },
-                    "datepicker": {
-                        "placeholder": "Selecione uma data",
-                        "today": "Hoje"
-                    }
-                }
-            });
-        </script>
+        });
+    </script>
 }
```
