# Pages/Agenda/Index.cshtml

**Mudanca:** GRANDE | **+103** linhas | **-429** linhas

---

```diff
--- JANEIRO: Pages/Agenda/Index.cshtml
+++ ATUAL: Pages/Agenda/Index.cshtml
@@ -86,12 +86,6 @@
 
     <link rel="stylesheet" type="text/css" href="~/css/spinkit.css">
 
-    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/themes/8.2.1/default/default-main.css" />
-    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
-    <script src="https://kendo.cdn.telerik.com/2024.3.806/js/jszip.min.js"></script>
-    <script src="https://kendo.cdn.telerik.com/2024.3.806/js/kendo.all.min.js"></script>
-    <script src="https://kendo.cdn.telerik.com/2024.3.806/js/kendo.aspnetmvc.min.js"></script>
-
     <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/3.11.174/pdf.min.js" defer></script>
     <script>
 
@@ -123,16 +117,9 @@
     <script>
 
         /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * COMBOBOX MOTORISTA - TEMPLATES COM FOTO
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Inicializa templates do ComboBox de Motorista.
-         * @@description Configura itemTemplate e valueTemplate com fotos.
-         * Chamada pelo evento created do ComboBox Syncfusion.
-         */
+    * Inicializa os templates do ComboBox de Motorista
+    * Configura itemTemplate e valueTemplate com fotos
+    */
         function onLstMotoristaCreated() {
             try {
                 const combo = document.getElementById('lstMotorista');
@@ -153,13 +140,13 @@
                     }
 
                     return `
-                                <div class="d-flex align-items-center">
-                                    <img src="${imgSrc}"
-                                         alt="Foto ${data.Nome || 'Motorista'}"
-                                         style="height:40px; width:40px; border-radius:50%; margin-right:10px; object-fit: cover;"
-                                         onerror="this.src='/images/barbudo.jpg';" />
-                                    <span>${data.Nome || ''}</span>
-                                </div>`;
+                        <div class="d-flex align-items-center">
+                            <img src="${imgSrc}"
+                                 alt="Foto ${data.Nome || 'Motorista'}"
+                                 style="height:40px; width:40px; border-radius:50%; margin-right:10px; object-fit: cover;"
+                                 onerror="this.src='/images/barbudo.jpg';" />
+                            <span>${data.Nome || ''}</span>
+                        </div>`;
                 };
 
                 motoristaCombo.valueTemplate = function (data) {
@@ -172,13 +159,13 @@
                     }
 
                     return `
-                                <div class="d-flex align-items-center">
-                                    <img src="${imgSrc}"
-                                         alt="Foto ${data.Nome || 'Motorista'}"
-                                         style="height:30px; width:30px; border-radius:50%; margin-right:10px; object-fit: cover;"
-                                         onerror="this.src='/images/barbudo.jpg';" />
-                                    <span>${data.Nome || ''}</span>
-                                </div>`;
+                        <div class="d-flex align-items-center">
+                            <img src="${imgSrc}"
+                                 alt="Foto ${data.Nome || 'Motorista'}"
+                                 style="height:30px; width:30px; border-radius:50%; margin-right:10px; object-fit: cover;"
+                                 onerror="this.src='/images/barbudo.jpg';" />
+                            <span>${data.Nome || ''}</span>
+                        </div>`;
                 };
 
                 console.log('Templates de foto do motorista configurados com sucesso');
@@ -192,9 +179,8 @@
         }
 
         /**
-         * Handler de mudança do ComboBox de Motorista.
-         * @@param {Object} args - Argumentos do evento change do Syncfusion ComboBox.
-         * @@description Atualiza foto do motorista quando selecionado.
+         * Handler de mudança do ComboBox de Motorista
+         * Atualiza foto quando motorista é selecionado
          */
         function onLstMotoristaChange(args) {
             try {
@@ -444,77 +430,6 @@
             font-weight: 400;
         }
 
-        /* ======== Botão Laranja de Visualizar Ficha de Vistoria ======== */
-        .btn-ficha-vistoria {
-            background-color: #ff9800;
-            color: white;
-            border: none;
-            border-radius: 6px;
-            padding: 6px 12px;
-            cursor: pointer;
-            transition: all 0.3s ease;
-            font-size: 1.1em;
-            display: inline-flex;
-            align-items: center;
-            justify-content: center;
-            min-width: 40px;
-            height: 38px;
-        }
-
-        .btn-ficha-vistoria:hover:not(:disabled) {
-            background-color: #f57c00;
-            transform: scale(1.05);
-            box-shadow: 0 2px 8px rgba(255, 152, 0, 0.4);
-        }
-
-        .btn-ficha-vistoria:active:not(:disabled) {
-            transform: scale(0.98);
-        }
-
-        .btn-ficha-vistoria:disabled {
-            background-color: #ccc;
-            cursor: not-allowed;
-            opacity: 0.6;
-            pointer-events: none;
-            /* Previne cliques no botão desabilitado */
-        }
-
-        .btn-ficha-vistoria i {
-            font-size: 1.2em;
-        }
-
-        /* ======== Modal Ficha de Vistoria ======== */
-        .modal-header-ficha {
-            background: linear-gradient(135deg, #ff9800 0%, #f57c00 100%);
-            color: white;
-            border-bottom: 3px solid #e65100;
-        }
-
-        .modal-header-ficha .modal-title {
-            font-weight: 600;
-            font-size: 1.3rem;
-        }
-
-        .modal-header-ficha .btn-close {
-            filter: brightness(0) invert(1);
-        }
-
-        .ficha-vistoria-container {
-            background-color: #f5f5f5;
-            display: flex;
-            align-items: center;
-            justify-content: center;
-            min-height: 500px;
-        }
-
-        .ficha-vistoria-img {
-            width: 100%;
-            height: auto;
-            max-width: 100%;
-            object-fit: contain;
-            background-color: white;
-        }
-
         /* ======== Distância > 100km - Vermelho Negrito ======== */
         .distancia-alerta {
             color: #dc3545 !important;
@@ -557,19 +472,19 @@
             z-index: 1070 !important;
         }
 
-        /* ======== Kendo NumericTextBox - Forçar bordas ======== */
-        #txtQtdParticipantesEventoCadastro,
-        #txtQtdParticipantesEvento,
-        #txtDuracao {
-            width: 100% !important;
-        }
-
-        .k-numerictextbox {
-            width: 100%;
-        }
-
-        .k-numerictextbox .k-input {
-            height: 38px !important;
+        /* ======== NumericTextBox - Forçar bordas no Modal Evento ======== */
+        #txtQtdParticipantesEventoCadastro.e-numerictextbox {
+            border: 1px solid #ced4da !important;
+            border-radius: 0.25rem !important;
+        }
+
+        #txtQtdParticipantesEventoCadastro .e-input-group {
+            border: 1px solid #ced4da !important;
+            border-radius: 0.25rem !important;
+        }
+
+        #txtQtdParticipantesEventoCadastro input.e-input {
+            border: none !important;
         }
 
         /* ======== Telerik DatePickers - Ajustar Altura no Modal Evento ======== */
@@ -812,7 +727,7 @@
                     <div class="section-card-body">
                         <div class="row align-baseline">
 
-                            <div id="divNoFichaVistoria" class="col-12 col-md-2" style="display: none !important;">
+                            <div id="divNoFichaVistoria" class="col-12 col-md-2">
                                 <div class="form-group">
                                     <label class="label font-weight-bold">
                                         Nº Ficha Vistoria
@@ -830,9 +745,9 @@
                                         <i class="fa-duotone fa-calendar-day field-icon"
                                             style="font-size: 1.25em; cursor: pointer;"></i>
                                     </label>
-                                    <kendo-datepicker id="txtDataInicial" name="txtDataInicial" format="dd/MM/yyyy"
-                                        placeholder="Selecione a data" min="@DateTime.Today" culture="pt-BR">
-                                    </kendo-datepicker>
+                                    <ejs-datepicker id="txtDataInicial" format="dd/MM/yyyy"
+                                        placeholder="Selecione a data" min="@DateTime.Today">
+                                    </ejs-datepicker>
                                 </div>
                             </div>
 
@@ -854,9 +769,9 @@
                                         <i class="fa-duotone fa-calendar-check field-icon"
                                             style="font-size: 1.25em; cursor: pointer;"></i>
                                     </label>
-                                    <kendo-datepicker id="txtDataFinal" name="txtDataFinal" format="dd/MM/yyyy"
-                                        placeholder="Selecione a data" min="@DateTime.Today" culture="pt-BR">
-                                    </kendo-datepicker>
+                                    <ejs-datepicker id="txtDataFinal" format="dd/MM/yyyy" placeholder="Selecione a data"
+                                        min="@DateTime.Today">
+                                    </ejs-datepicker>
                                 </div>
                             </div>
 
@@ -878,9 +793,10 @@
                                         <i class="fa-duotone fa-hourglass-half field-icon"
                                             style="font-size: 1.25em; cursor: pointer;"></i>
                                     </label>
-                                    <kendo-numerictextbox name="txtDuracao" enable="false" format="n0" spinners="false"
-                                        readonly="true" title="Se a Duração estiver alta, revise o horário e datas">
-                                    </kendo-numerictextbox>
+                                    <ejs-numerictextbox id="txtDuracao" Enabled="false" format="n0" Readonly="true"
+                                        showSpinButton="false"
+                                        data-ejtip="Se a <strong>Duração</strong> estiver alta, revise o horário e datas">
+                                    </ejs-numerictextbox>
                                 </div>
                             </div>
 
@@ -949,23 +865,11 @@
                                                 style="font-size: 1.25em; cursor: pointer;"></i>
                                         </div>
 
-                                        <div class="d-flex align-items-center">
-
-                                            <ejs-combobox id="cmbDestino" dataSource="@ViewData["ListaDestino"]"
-                                                placeholder="Selecione ou digite o destino" allowFiltering="true"
-                                                filterType="Contains" allowCustom="true" popupHeight="220px"
-                                                data-ejtip="Destino não está na lista? Consulte direito antes de cadastrar!"
-                                                cssClass="flex-grow-1">
-                                            </ejs-combobox>
-
-                                            <button type="button" id="btnVisualizarFichaVistoria"
-                                                class="btn-ficha-vistoria ms-2 disabled"
-                                                title="Visualizar Ficha de Vistoria"
-                                                data-ejtip="Clique para visualizar a Ficha de Vistoria desta viagem"
-                                                style="display: none;" disabled>
-                                                <i class="fa-duotone fa-clipboard-list"></i>
-                                            </button>
-                                        </div>
+                                        <ejs-combobox id="cmbDestino" dataSource="@ViewData["ListaDestino"]"
+                                            placeholder="Selecione ou digite o destino" allowFiltering="true"
+                                            filterType="Contains" allowCustom="true" popupHeight="220px"
+                                            data-ejtip="Destino não está na lista? Consulte direito antes de cadastrar!">
+                                        </ejs-combobox>
                                     </div>
                                 </div>
                             </div>
@@ -1014,9 +918,9 @@
                                     <i class="fa-duotone fa-calendar-day field-icon"
                                         style="font-size: 1.25em; cursor: pointer;"></i>
                                 </label>
-                                <kendo-datepicker id="txtDataInicioEvento" name="txtDataInicioEvento"
-                                    format="dd/MM/yyyy" placeholder="Data Início" culture="pt-BR" enable="false">
-                                </kendo-datepicker>
+                                <ejs-datepicker id="txtDataInicioEvento" format="dd/MM/yyyy" placeholder="Data Início"
+                                    locale="pt-BR" enabled="false">
+                                </ejs-datepicker>
                             </div>
 
                             <div class="col-md-4 mb-3">
@@ -1025,9 +929,9 @@
                                     <i class="fa-duotone fa-calendar-check field-icon"
                                         style="font-size: 1.25em; cursor: pointer;"></i>
                                 </label>
-                                <kendo-datepicker id="txtDataFimEvento" name="txtDataFimEvento" format="dd/MM/yyyy"
-                                    placeholder="Data Fim" culture="pt-BR" enable="false">
-                                </kendo-datepicker>
+                                <ejs-datepicker id="txtDataFimEvento" format="dd/MM/yyyy" placeholder="Data Fim"
+                                    locale="pt-BR" enabled="false">
+                                </ejs-datepicker>
                             </div>
 
                             <div class="col-md-4 mb-3">
@@ -1036,9 +940,9 @@
                                     <i class="fa-duotone fa-users field-icon"
                                         style="font-size: 1.25em; cursor: pointer;"></i>
                                 </label>
-                                <kendo-numerictextbox name="txtQtdParticipantesEvento" format="n0" min="0"
-                                    placeholder="Quantidade" enable="false">
-                                </kendo-numerictextbox>
+                                <ejs-numerictextbox id="txtQtdParticipantesEvento" format="N0" min="0"
+                                    placeholder="Quantidade" enabled="false">
+                                </ejs-numerictextbox>
                             </div>
 
                         </div>
@@ -1171,65 +1075,6 @@
                     </div>
                 </div>
 
-                <div class="section-card" id="cardEquipamentos" style="display: none;">
-                    <div class="section-card-header">
-                        <i class="fa-duotone fa-toolbox"></i> Equipamentos
-                    </div>
-                    <div class="section-card-body">
-                        <div class="row align-items-center">
-
-                            <div class="col-6 col-md-3">
-                                <div class="form-check form-switch">
-                                    <input type="hidden" id="hidCaboEntregue" value="false" />
-                                    <input class="form-check-input" type="checkbox" id="chkCaboEntregue"
-                                        onchange="document.getElementById('hidCaboEntregue').value = this.checked ? 'true' : 'false';">
-                                    <label class="form-check-label fw-semibold" for="chkCaboEntregue">
-                                        <i class="fa-duotone fa-plug me-1 text-warning"></i>
-                                        Cabo Entregue
-                                    </label>
-                                </div>
-                            </div>
-
-                            <div class="col-6 col-md-3">
-                                <div class="form-check form-switch">
-                                    <input type="hidden" id="hidCaboDevolvido" value="false" />
-                                    <input class="form-check-input" type="checkbox" id="chkCaboDevolvido"
-                                        onchange="document.getElementById('hidCaboDevolvido').value = this.checked ? 'true' : 'false';">
-                                    <label class="form-check-label fw-semibold" for="chkCaboDevolvido">
-                                        <i class="fa-duotone fa-plug me-1 text-warning"></i>
-                                        Cabo Devolvido
-                                    </label>
-                                </div>
-                            </div>
-
-                            <div class="col-6 col-md-3">
-                                <div class="form-check form-switch">
-                                    <input type="hidden" id="hidArlaEntregue" value="false" />
-                                    <input class="form-check-input" type="checkbox" id="chkArlaEntregue"
-                                        onchange="document.getElementById('hidArlaEntregue').value = this.checked ? 'true' : 'false';">
-                                    <label class="form-check-label fw-semibold" for="chkArlaEntregue">
-                                        <i class="fa-duotone fa-gas-pump me-1 text-success"></i>
-                                        Arla Entregue
-                                    </label>
-                                </div>
-                            </div>
-
-                            <div class="col-6 col-md-3">
-                                <div class="form-check form-switch">
-                                    <input type="hidden" id="hidArlaDevolvido" value="false" />
-                                    <input class="form-check-input" type="checkbox" id="chkArlaDevolvido"
-                                        onchange="document.getElementById('hidArlaDevolvido').value = this.checked ? 'true' : 'false';">
-                                    <label class="form-check-label fw-semibold" for="chkArlaDevolvido">
-                                        <i class="fa-duotone fa-gas-pump me-1 text-success"></i>
-                                        Arla Devolvido
-                                    </label>
-                                </div>
-                            </div>
-
-                        </div>
-                    </div>
-                </div>
-
                 <div class="section-card">
                     <div class="section-card-header">
                         <i class="fa-duotone fa-users"></i> Requisitante e Setor
@@ -1337,8 +1182,11 @@
                                             <i class="fa-duotone fa-arrows-spin field-icon"
                                                 style="font-size: 1.25em; cursor: pointer;"></i>
                                         </label>
-
-                                        <input id="lstRecorrente" style="width: 100%;" />
+                                        <ejs-dropdownlist id="lstRecorrente" popupHeight="200px"
+                                            cssClass="e-outline text-center" floatLabelType="Never">
+                                            <e-dropdownlist-fields text="Descricao"
+                                                value="RecorrenteId"></e-dropdownlist-fields>
+                                        </ejs-dropdownlist>
                                     </div>
                                 </div>
 
@@ -1349,8 +1197,8 @@
                                             <i class="fa-duotone fa-calendar-circle-exclamation field-icon"
                                                 style="font-size: 1.25em; cursor: pointer;"></i>
                                         </label>
-
-                                        <input id="lstPeriodos" style="width: 100%;" />
+                                        <input id="lstPeriodos" type="text" class="e-control e-dropdownlist"
+                                            style="width: 100%; height: 38px;" />
                                     </div>
                                 </div>
 
@@ -1358,15 +1206,20 @@
 
                             <div class="row">
 
-                                <div class="col-md-3" id="divDias" style="display: none;">
+                                <div class="col-md-6" id="divDias" style="display: none;">
                                     <div class="form-group mb-3">
                                         <label for="lstDias" class="label font-weight-bold">
                                             <span class="campo-obrigatorio">*</span>Dias da Semana
                                             <i class="fa-duotone fa-calendar-week field-icon"
                                                 style="font-size: 1.25em; cursor: pointer;"></i>
                                         </label>
-
-                                        <select id="lstDias" style="width: 100%;"></select>
+                                        <ejs-multiselect id="lstDias" placeholder="Selecione os dias..." mode="Box"
+                                            showSelectAll="true" selectAllText="Selecionar todos"
+                                            unSelectAllText="Desmarcar todos" showDropDownIcon="true"
+                                            allowFiltering="false" readonly="false" cssClass="multiselect-dias-semana"
+                                            popupHeight="250px">
+                                            <e-multiselect-fields value="Value" text="Text"></e-multiselect-fields>
+                                        </ejs-multiselect>
                                     </div>
                                 </div>
 
@@ -1377,8 +1230,10 @@
                                             <i class="fa-duotone fa-calendar-clock field-icon"
                                                 style="font-size: 1.25em; cursor: pointer;"></i>
                                         </label>
-
-                                        <input id="lstDiasMes" style="width: 100%;" />
+                                        <ejs-dropdownlist id="lstDiasMes" placeholder="Selecione o dia..."
+                                            popupHeight="250px" cssClass="e-outline">
+                                            <e-dropdownlist-fields value="Value" text="Text"></e-dropdownlist-fields>
+                                        </ejs-dropdownlist>
                                     </div>
                                 </div>
 
@@ -1389,14 +1244,14 @@
                                             <i class="fa-duotone fa-calendar-check field-icon"
                                                 style="font-size: 1.25em; cursor: pointer;"></i>
                                         </label>
-                                        <kendo-datepicker id="txtFinalRecorrencia" name="txtFinalRecorrencia"
-                                            format="dd/MM/yyyy" placeholder="Selecione a data final"
-                                            min="@DateTime.Today" culture="pt-BR">
-                                        </kendo-datepicker>
+                                        <ejs-datepicker id="txtFinalRecorrencia" format="dd/MM/yyyy"
+                                            placeholder="Selecione a data final" locale="pt-BR" min="@DateTime.Today"
+                                            cssClass="e-outline">
+                                        </ejs-datepicker>
 
                                         <input type="text" id="txtFinalRecorrenciaTexto"
-                                            class="form-control e-outline ftx-final-recorrencia-texto" readonly
-                                            style="display:none;" placeholder="dd/MM/yyyy">
+                                            class="form-control e-outline ftx-final-recorrencia-texto"
+                                            readonly style="display:none;" placeholder="dd/MM/yyyy">
                                     </div>
                                 </div>
 
@@ -1415,12 +1270,6 @@
                                 .ftx-ocultar-recorrencia-date {
                                     display: none !important;
                                 }
-
-                                .ftx-calendario-wrapper {
-                                    position: relative;
-                                    display: inline-block;
-                                    max-width: 100%;
-                                }
                             </style>
 
                             <div class="row" id="calendarContainer" style="display: none;">
@@ -1432,11 +1281,7 @@
                                                 style="font-size: 1.25em; cursor: pointer;"></i>
                                         </label>
 
-                                        <div class="ftx-calendario-wrapper position-relative">
-                                            <span id="badgeContadorDatas" class="badge-contador-datas"
-                                                style="display:none;">0</span>
-                                            <div id="calDatasSelecionadas"></div>
-                                        </div>
+                                        <div id="calDatasSelecionadas"></div>
                                     </div>
                                 </div>
                             </div>
@@ -1613,9 +1458,9 @@
                             <label for="txtQtdParticipantesEventoCadastro" class="font-weight-bold">
                                 Quantidade de Participantes <span class="text-danger">*</span>
                             </label>
-                            <kendo-numerictextbox name="txtQtdParticipantesEventoCadastro" format="n0" min="1"
+                            <ejs-numerictextbox id="txtQtdParticipantesEventoCadastro" format="N0" min="1"
                                 placeholder="Quantidade de participantes">
-                            </kendo-numerictextbox>
+                            </ejs-numerictextbox>
                         </div>
                     </div>
 
@@ -1757,56 +1602,10 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalFichaVistoria" data-bs-backdrop="static" data-bs-keyboard="false"
-    style="z-index: 1060;">
-    <div class="modal-dialog modal-xl modal-dialog-centered">
-        <div class="modal-content">
-
-            <div class="modal-header modal-header-ficha">
-                <h5 class="modal-title">
-                    <i class="fa-duotone fa-clipboard-list"></i>
-                    Ficha de Vistoria
-                    <span id="spanNoFichaModal" class="badge bg-light text-dark ms-2"></span>
-                </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
-            </div>
-
-            <div class="modal-body p-0" style="min-height: 500px; max-height: 75vh; overflow: auto;">
-
-                <div id="containerFichaVistoria" class="ficha-vistoria-container">
-
-                    <div id="fichaVistoriaLoading" class="text-center py-5">
-                        <div class="spinner-border text-warning" role="status" style="width: 3rem; height: 3rem;">
-                            <span class="visually-hidden">Carregando...</span>
-                        </div>
-                        <p class="mt-3 text-muted">Carregando Ficha de Vistoria...</p>
-                    </div>
-
-                    <img id="imgFichaVistoria" class="ficha-vistoria-img" style="display: none;"
-                        alt="Ficha de Vistoria" />
-
-                    <div id="fichaVistoriaSemImagem" class="text-center py-5" style="display: none;">
-                        <i class="fa-duotone fa-file-circle-question text-muted" style="font-size: 4rem;"></i>
-                        <p class="mt-3 text-muted">Nenhuma imagem de Ficha de Vistoria disponível.</p>
-                    </div>
-                </div>
-            </div>
-
-            <div class="modal-footer justify-content-center">
-                <button type="button" class="btn btn-fechar-ficha" data-bs-dismiss="modal">
-                    <i class="fa-duotone fa-circle-xmark"></i>
-                    Fechar
-                </button>
-            </div>
-        </div>
-    </div>
-</div>
-
 @section ScriptsBlock
 {
 
-            <script>
+    <script>
         window.dataFinalidade = @Html.Raw(Json.Serialize(ViewData["dataFinalidade"]));
         window.dataPeriodo = @Html.Raw(Json.Serialize(ViewData["dataPeriodo"]));
         window.dataPeriodos = @Html.Raw(Json.Serialize(ViewData["dataPeriodos"]));
@@ -1816,16 +1615,16 @@
             forcarTextoRecorrencia: @RecorrenciaToggleSettings.ForcarTextoRecorrencia.ToString().ToLower(),
             forcarDatePickerRecorrencia: @RecorrenciaToggleSettings.ForcarDatePickerRecorrencia.ToString().ToLower(),
             mostrarToggleDev: @RecorrenciaToggleSettings.MostrarToggleDev.ToString().ToLower()
-                };
+        };
     </script>
 
-            <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet"
+    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet"
         type="text/css" />
     <script src="https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js"></script>
     <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js"></script>
     <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
 
-            <script src="/api/reports/resources/js/telerikReportViewer"></script>
+    <script src="/api/reports/resources/js/telerikReportViewer"></script>
     <script src="https://cdn.datatables.net/1.10.25/js/jquery.dataTables.min.js"></script>
     <script src="https://cdn.datatables.net/responsive/2.2.9/js/dataTables.responsive.min.js"></script>
     <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
@@ -1835,89 +1634,44 @@
         integrity="sha512-zAadCzZHXo/f5A/3uhF50bZXahdYNqisNgKKniyoJJVpp27b2bR82N4hPLGj3/qBEh3tGZ9SYGmSA1jpdtNF5A=="
         crossorigin="anonymous" referrerpolicy="no-referrer"></script>
 
-            <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.2/moment.min.js"></script>
+    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.2/moment.min.js"></script>
     <script src="/api/reports/resources/js/telerikReportViewer-18.1.24.514.min.js"></script>
 
-            <script>
-
-        (function () {
-            const originalError = window.onerror;
-            window.onerror = function (message, source, lineno, colno, error) {
-
-                if (message && message.includes && message.includes('percentSign')) {
-                    console.warn('[Syncfusion] ⚠️ Erro de formatação silenciado (não afeta funcionalidade)');
-                    return true;
-                }
-
-                if (originalError) {
-                    return originalError.apply(this, arguments);
-                }
-                return false;
-            };
-
-            window.addEventListener('unhandledrejection', function (event) {
-                if (event.reason && event.reason.message && event.reason.message.includes('percentSign')) {
-                    console.warn('[Syncfusion] ⚠️ Promise rejection silenciada (percentSign)');
-                    event.preventDefault();
-                }
-            });
-        })();
-    </script>
-
-            <script src="~/js/localization-init.js" asp-append-version="true"></script>
-
-            <script>
-        window.syncfusionLocalizationLoading = true;
-        window.syncfusionLocalizationReady = false;
-
-        (async function () {
-            try {
-                if (typeof window.loadSyncfusionLocalization === 'function') {
-                    await window.loadSyncfusionLocalization();
-                    window.syncfusionLocalizationReady = true;
-                    window.syncfusionLocalizationLoading = false;
-                    console.log('[Agenda/Index] ✅ Localização Syncfusion pt-BR carregada');
-                }
-            } catch (error) {
-                console.warn('[Agenda/Index] ⚠️ Erro ao carregar localização (não crítico):', error.message);
-                window.syncfusionLocalizationLoading = false;
-            }
-        })();
-    </script>
-
-            <script src="~/js/agendamento/core/ajax-helper.js" asp-append-version="true"></script>
+    <script src="~/js/localization-init.js" asp-append-version="true"></script>
+
+    <script src="~/js/agendamento/core/ajax-helper.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/core/state.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/core/api-client.js" asp-append-version="true"></script>
 
-            <script src="~/js/agendamento/utils/date.utils.js" asp-append-version="true"></script>
+    <script src="~/js/agendamento/utils/date.utils.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/utils/syncfusion.utils.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/utils/kendo-editor-helper.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/utils/formatters.js" asp-append-version="true"></script>
 
-            <script src="~/js/agendamento/services/agendamento.service.js" asp-append-version="true"></script>
+    <script src="~/js/agendamento/services/agendamento.service.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/services/viagem.service.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/services/requisitante.service.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/services/evento.service.js" asp-append-version="true"></script>
 
-            <script src="~/js/agendamento/components/validacao.js" asp-append-version="true"></script>
+    <script src="~/js/agendamento/components/validacao.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/components/modal-viagem-novo.js" asp-append-version="true"></script>
-
-            <script src="~/js/agendamento/components/modal-config.js" asp-append-version="true"></script>
+    <script src="~/js/agendamento/components/recorrencia.js" asp-append-version="true"></script>
+    <script src="~/js/agendamento/components/modal-config.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/components/exibe-viagem.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/components/event-handlers.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/components/dialogs.js" asp-append-version="true"></script>
-
-            <script src="~/js/agendamento/components/reportviewer-close-guard.js" asp-append-version="true"></script>
+    <script src="~/js/agendamento/components/controls-init.js" asp-append-version="true"></script>
+    <script src="~/js/agendamento/components/recorrencia-init.js" asp-append-version="true"></script>
+
+    <script src="~/js/agendamento/components/reportviewer-close-guard.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/components/relatorio.js" asp-append-version="true"></script>
     <script src="~/js/agendamento/components/evento.js" asp-append-version="true"></script>
-
-            <script src="~/js/agendamento/components/calendario.js" asp-append-version="true"></script>
-
-            <script src="~/js/agendamento/agendamento-core.js" asp-append-version="true"></script>
-
-            <script src="~/js/validacao/ValidadorFinalizacaoIA.js" asp-append-version="true"></script>
-
-            <script>
+    <script src="~/js/agendamento/components/recorrencia-logic.js" asp-append-version="true"></script>
+    <script src="~/js/agendamento/components/calendario.js" asp-append-version="true"></script>
+
+    <script src="~/js/validacao/ValidadorFinalizacaoIA.js" asp-append-version="true"></script>
+
+    <script>
         /**
          * Callback quando um setor é selecionado no TreeView
          */
@@ -1960,157 +1714,6 @@
         });
     </script>
 
-            <script>
-        /**
-         * ╔══════════════════════════════════════════════════════════════════════════════╗
-         * ║ BOTÃO FICHA DE VISTORIA - VISUALIZAÇÃO ║
-         * ╚══════════════════════════════════════════════════════════════════════════════╝
-         *
-         * Este script gerencia o botão laranja que aparece ao lado do campo Destino
-         * para visualizar a Ficha de Vistoria da viagem.
-         *
-         * REGRAS:
-         * - Botão só aparece quando a viagem TEM número de ficha válido (> 0)
-         * - Botão fica ao lado do ComboBox Destino (layout horizontal)
-         * - Ao clicar, abre modal com a imagem da ficha de vistoria
-         * - Modal abre sobre o modal principal (z-index maior)
-         *
-         * DATA: 22/01/2026
-         */
-
-        let numeroFichaVistoriaAtual = null;
-
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * BOTÃO FICHA DE VISTORIA
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Gerenciamento do botão para visualizar Ficha de Vistoria.
-         * Botão só aparece quando a viagem tem número de ficha válido.
-         * @@since 22/01/2026
-         */
-
-        /** @@type {number|null} Número da ficha de vistoria atual */
-
-        /**
-         * Mostra ou oculta o botão de visualização da Ficha de Vistoria.
-         * @@param {number|string} numeroFicha - Número da ficha de vistoria.
-         * @@description Usa padrão de Contrato/Index para tooltips funcionarem
-         * em botões desabilitados com pointer-events: auto.
-         */
-        function gerenciarBotaoFichaVistoria(numeroFicha) {
-            const btnVisualizar = document.getElementById("btnVisualizarFichaVistoria");
-
-            if (!btnVisualizar) {
-                console.warn("⚠️ Botão btnVisualizarFichaVistoria não encontrado no DOM");
-                return;
-            }
-
-            const fichaNum = parseInt(numeroFicha);
-            const fichaValida = fichaNum && fichaNum > 0;
-
-            if (fichaValida) {
-
-                btnVisualizar.style.display = "inline-flex";
-                btnVisualizar.disabled = false;
-                btnVisualizar.classList.remove("disabled");
-                btnVisualizar.setAttribute("data-ejtip", "Clique para visualizar a Ficha de Vistoria desta viagem");
-                numeroFichaVistoriaAtual = fichaNum;
-                console.log(`✅ Botão Ficha Vistoria EXIBIDO e HABILITADO (Ficha: ${fichaNum})`);
-            } else {
-
-                btnVisualizar.style.display = "none";
-                btnVisualizar.disabled = true;
-                btnVisualizar.classList.add("disabled");
-                numeroFichaVistoriaAtual = null;
-                console.log("🔒 Botão Ficha Vistoria OCULTO (sem ficha válida)");
-            }
-
-            if (window.ejTooltip) {
-                window.ejTooltip.refresh();
-            }
-        }
-
-        /**
-         * Carrega e exibe a imagem da ficha de vistoria no modal.
-         * @@param {number} numeroFicha - Número da ficha de vistoria.
-         * @@description Abre modal sobre o modal principal (z-index maior).
-         */
-        function visualizarFichaVistoria(numeroFicha) {
-            try {
-                console.log(`🖼️ Abrindo Ficha de Vistoria: ${numeroFicha}`);
-
-                const modal = document.getElementById("modalFichaVistoria");
-                const imgFicha = document.getElementById("imgFichaVistoria");
-                const spanNo = document.getElementById("spanNoFichaModal");
-                const loading = document.getElementById("fichaVistoriaLoading");
-                const semImagem = document.getElementById("fichaVistoriaSemImagem");
-
-                if (!modal || !imgFicha || !loading || !semImagem) {
-                    console.error("❌ Elementos do modal de ficha não encontrados");
-                    Alerta.Erro("Erro", "Não foi possível abrir o modal da ficha de vistoria");
-                    return;
-                }
-
-                imgFicha.style.display = "none";
-                loading.style.display = "block";
-                semImagem.style.display = "none";
-
-                if (spanNo) {
-                    spanNo.textContent = `Nº ${numeroFicha}`;
-                }
-
-                const bsModal = new bootstrap.Modal(modal);
-                bsModal.show();
-
-                const urlImagem = `/upload/fichas-vistoria/${numeroFicha}.jpg`;
-                console.log(`📡 Carregando imagem: ${urlImagem}`);
-
-                imgFicha.onload = function () {
-                    loading.style.display = "none";
-                    imgFicha.style.display = "block";
-                    console.log("✅ Imagem da ficha carregada com sucesso");
-                };
-
-                imgFicha.onerror = function () {
-                    loading.style.display = "none";
-                    semImagem.style.display = "block";
-                    console.warn("⚠️ Imagem da ficha não encontrada");
-                };
-
-                imgFicha.src = urlImagem;
-
-            } catch (error) {
-                console.error("❌ Erro ao visualizar ficha de vistoria:", error);
-                Alerta.TratamentoErroComLinha("Agenda/Index.cshtml", "visualizarFichaVistoria", error);
-            }
-        }
-
-        /**
-         * Inicialização do botão de ficha de vistoria
-         */
-        $(document).ready(function () {
-
-            $("#btnVisualizarFichaVistoria").on("click", function (e) {
-                e.preventDefault();
-                e.stopPropagation();
-
-                if (numeroFichaVistoriaAtual) {
-                    visualizarFichaVistoria(numeroFichaVistoriaAtual);
-                } else {
-                    console.warn("⚠️ Nenhuma ficha de vistoria definida");
-                    Alerta.Aviso("Aviso", "Número da ficha de vistoria não encontrado");
-                }
-            });
-
-            console.log("✅ Botão Ficha Vistoria inicializado");
-
-            gerenciarBotaoFichaVistoria(null);
-        });
-
-        window.gerenciarBotaoFichaVistoria = gerenciarBotaoFichaVistoria;
-        window.visualizarFichaVistoria = visualizarFichaVistoria;
-    </script>
-
-            <script src="~/js/agendamento/main.js" asp-append-version="true"></script>
+    <script src="~/js/agendamento/main.js" asp-append-version="true"></script>
 
 }
```
