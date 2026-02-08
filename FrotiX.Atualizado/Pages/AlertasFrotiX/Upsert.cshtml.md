# Pages/AlertasFrotiX/Upsert.cshtml

**Mudanca:** GRANDE | **+140** linhas | **-205** linhas

---

```diff
--- JANEIRO: Pages/AlertasFrotiX/Upsert.cshtml
+++ ATUAL: Pages/AlertasFrotiX/Upsert.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
 @addTagHelper *, Syncfusion.EJ2
 
@@ -22,7 +21,6 @@
             border-radius: 8px;
             margin-top: 1rem;
         }
-
         #divRecorrenciaAlerta .section-card-header {
             background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
             color: white;
@@ -30,59 +28,15 @@
             border-radius: 8px 8px 0 0;
             font-weight: 600;
         }
-
         #divRecorrenciaAlerta .section-card-body {
             padding: 1rem;
         }
-
-        /* ======== CALENDÁRIO DATEPICKER - LARGURA AUMENTADA ======== */
-        /* Popup do calendário com largura adequada para não trepar header */
-        #DataExibicao_popup.e-datepicker.e-popup,
-        #DataExpiracao_popup.e-datepicker.e-popup {
-            min-width: 280px !important;
-        }
-
-        /* Container do calendário */
-        #DataExibicao_popup .e-calendar,
-        #DataExpiracao_popup .e-calendar {
-            min-width: 280px !important;
-        }
-
-        /* Header do calendário (dias da semana) */
-        #DataExibicao_popup .e-calendar .e-header,
-        #DataExpiracao_popup .e-calendar .e-header {
-            padding: 8px 12px !important;
-        }
-
-        /* Células do calendário com espaçamento adequado */
-        #DataExibicao_popup .e-calendar .e-content table td,
-        #DataExpiracao_popup .e-calendar .e-content table td {
-            padding: 6px !important;
-            min-width: 36px !important;
-        }
     </style>
 }
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * TRATAMENTO DE ERROS GLOBAIS - UPSERT ALERTAS
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Captura e registra erros não tratados em toda a página.
-     * Evita que erros JavaScript quebrem a experiência do usuário.
-     */
-
-    /**
-     * Handler global para erros JavaScript
-     * @@description Captura erros não tratados em qualquer parte do script
-        * @@param { string } msg - Mensagem de erro
-            * @@param { string } url - URL do arquivo onde ocorreu o erro
-                * @@param { number } line - Número da linha do erro
-                    * @@param { number } col - Número da coluna do erro
-                        * @@param { Error } error - Objeto Error com detalhes do erro
-                            * @@returns { boolean } true para suprimir o erro padrão do navegador
-                                */
-    window.onerror = function (msg, url, line, col, error) {
+
+    window.onerror = function(msg, url, line, col, error) {
         try {
             console.error('Erro Global:', {
                 mensagem: msg,
@@ -98,12 +52,7 @@
         }
     };
 
-    /**
-     * Handler para Promises rejeitadas não tratadas
-     * @@description Monitora rejeições de Promise sem.catch()
-        * @@param { PromiseRejectionEvent } e - Evento de rejeição
-            */
-    window.addEventListener('unhandledrejection', function (e) {
+    window.addEventListener('unhandledrejection', function(e) {
         try {
             console.error('Promise rejeitada:', e.reason);
             e.preventDefault();
@@ -122,19 +71,12 @@
                     <i class="fa-duotone fa-bell-plus"></i>
                     @(Model.AlertasFrotiXId == Guid.Empty ? "Cadastrar Novo Alerta" : "Editar Alerta")
                 </h2>
-                <div class="ftx-card-actions">
-                    <a href="/Alertasfrotix/Alertasfrotix" class="btn btn-header-orange" data-ftx-loading>
-                        <i class="fa-duotone fa-rotate-left icon-space"></i>
-                        Voltar à Lista
-                    </a>
-                </div>
             </div>
 
             <div class="panel-container show">
                 <div class="panel-content">
                     <form id="formAlerta" method="post">
-                        <input type="hidden" id="AlertasFrotiXId" name="AlertasFrotiXId"
-                            value="@Model.AlertasFrotiXId" />
+                        <input type="hidden" id="AlertasFrotiXId" name="AlertasFrotiXId" value="@Model.AlertasFrotiXId" />
 
                         <div class="section-legend">
                             <i class="fa-duotone fa-info-circle"></i>
@@ -145,16 +87,20 @@
                             <div class="col-md-8">
                                 <label class="label required-field">Título do Alerta</label>
                                 <ejs-textbox id="Titulo" name="Titulo" placeholder="Digite o título do alerta"
-                                    value="@Model.Titulo" floatLabelType="Never" cssClass="e-outline">
+                                             value="@Model.Titulo"
+                                             floatLabelType="Never"
+                                             cssClass="e-outline">
                                 </ejs-textbox>
                                 <span class="text-danger field-validation-valid" data-valmsg-for="Titulo"></span>
                             </div>
 
                             <div class="col-md-4">
                                 <label class="label required-field">Prioridade</label>
-                                <ejs-dropdownlist id="Prioridade" name="Prioridade" placeholder="Selecione a prioridade"
-                                    dataSource="@Model.PrioridadesList" value="@((int)Model.Prioridade)"
-                                    floatLabelType="Never">
+                                <ejs-dropdownlist id="Prioridade" name="Prioridade"
+                                                  placeholder="Selecione a prioridade"
+                                                  dataSource="@Model.PrioridadesList"
+                                                  value="@((int)Model.Prioridade)"
+                                                  floatLabelType="Never">
                                     <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                                 </ejs-dropdownlist>
                                 <span class="text-danger field-validation-valid" data-valmsg-for="Prioridade"></span>
@@ -165,8 +111,12 @@
                             <div class="col-md-12">
                                 <label class="label required-field">Descrição</label>
                                 <ejs-textbox id="Descricao" name="Descricao"
-                                    placeholder="Digite a descrição detalhada do alerta" value="@Model.Descricao"
-                                    multiline="true" rows="3" floatLabelType="Never" cssClass="e-outline">
+                                             placeholder="Digite a descrição detalhada do alerta"
+                                             value="@Model.Descricao"
+                                             multiline="true"
+                                             rows="3"
+                                             floatLabelType="Never"
+                                             cssClass="e-outline">
                                 </ejs-textbox>
                                 <span class="text-danger field-validation-valid" data-valmsg-for="Descricao"></span>
                             </div>
@@ -215,10 +165,10 @@
                                 </div>
                             </div>
                             <div class="col-lg-2 col-md-4 col-sm-6 mb-3">
-                                <div class="tipo-alerta-card" data-tipo="6" data-ejtip="Selecionar tipo: Aniversário">
-                                    <i class="fa-duotone fa-cake-candles" style="color: #ec4899;"></i>
-                                    <div>Aniversário</div>
-                                    <span class="preview-badge" style="background-color: #ec4899;">Aniversário</span>
+                                <div class="tipo-alerta-card" data-tipo="6" data-ejtip="Selecionar tipo: Diversos">
+                                    <i class="fa-duotone fa-circle-info" style="color: #6c757d;"></i>
+                                    <div>Diversos</div>
+                                    <span class="preview-badge" style="background-color: #6c757d;">Diversos</span>
                                 </div>
                             </div>
                         </div>
@@ -234,39 +184,52 @@
                             <div class="row">
                                 <div class="col-md-6" id="divViagem" style="display: none;">
                                     <label class="label">Agendamento Relacionado</label>
-                                    <ejs-dropdownlist id="ViagemId" name="ViagemId"
-                                        placeholder="Selecione o agendamento" dataSource="@Model.ViagensListCompleta"
-                                        value="@Model.ViagemId" allowFiltering="true" filterType="Contains"
-                                        floatLabelType="Never" popupHeight="400px" popupWidth="600px">
-                                        <e-dropdownlist-fields text="DataInicial"
-                                            value="ViagemId"></e-dropdownlist-fields>
+                                    <ejs-dropdownlist id="ViagemId"
+                                                      name="ViagemId"
+                                                      placeholder="Selecione o agendamento"
+                                                      dataSource="@Model.ViagensListCompleta"
+                                                      value="@Model.ViagemId"
+                                                      allowFiltering="true"
+                                                      filterType="Contains"
+                                                      floatLabelType="Never"
+                                                      popupHeight="400px"
+                                                      popupWidth="600px">
+                                        <e-dropdownlist-fields text="DataInicial" value="ViagemId"></e-dropdownlist-fields>
                                     </ejs-dropdownlist>
                                 </div>
 
                                 <div class="col-md-6" id="divManutencao" style="display: none;">
                                     <label class="label">Manutenção Relacionada</label>
                                     <ejs-dropdownlist id="ManutencaoId" name="ManutencaoId"
-                                        placeholder="Selecione a manutenção" dataSource="@Model.ManutencoesListCompleta"
-                                        value="@Model.ManutencaoId" allowFiltering="true" floatLabelType="Never">
-                                        <e-dropdownlist-fields text="NumOS"
-                                            value="ManutencaoId"></e-dropdownlist-fields>
+                                                      placeholder="Selecione a manutenção"
+                                                      dataSource="@Model.ManutencoesListCompleta"
+                                                      value="@Model.ManutencaoId"
+                                                      allowFiltering="true"
+                                                      floatLabelType="Never">
+                                        <e-dropdownlist-fields text="NumOS" value="ManutencaoId"></e-dropdownlist-fields>
                                     </ejs-dropdownlist>
                                 </div>
 
                                 <div class="col-md-6" id="divMotorista" style="display: none;">
                                     <label class="label">Motorista Relacionado</label>
                                     <ejs-dropdownlist id="MotoristaId" name="MotoristaId"
-                                        placeholder="Selecione o motorista" dataSource="@Model.MotoristasList"
-                                        value="@Model.MotoristaId" allowFiltering="true" floatLabelType="Never">
+                                                      placeholder="Selecione o motorista"
+                                                      dataSource="@Model.MotoristasList"
+                                                      value="@Model.MotoristaId"
+                                                      allowFiltering="true"
+                                                      floatLabelType="Never">
                                         <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                                     </ejs-dropdownlist>
                                 </div>
 
                                 <div class="col-md-6" id="divVeiculo" style="display: none;">
                                     <label class="label">Veículo Relacionado</label>
-                                    <ejs-dropdownlist id="VeiculoId" name="VeiculoId" placeholder="Selecione o veículo"
-                                        dataSource="@Model.VeiculosList" value="@Model.VeiculoId" allowFiltering="true"
-                                        floatLabelType="Never">
+                                    <ejs-dropdownlist id="VeiculoId" name="VeiculoId"
+                                                      placeholder="Selecione o veículo"
+                                                      dataSource="@Model.VeiculosList"
+                                                      value="@Model.VeiculoId"
+                                                      allowFiltering="true"
+                                                      floatLabelType="Never">
                                         <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                                     </ejs-dropdownlist>
                                 </div>
@@ -281,9 +244,14 @@
                         <div class="row">
                             <div class="col-md-4">
                                 <label class="label required-field">Tipo de Exibição</label>
-                                <ejs-dropdownlist id="TipoExibicao" placeholder="Quando exibir o alerta"
-                                    dataSource="@Model.TipoExibicaoList" value="@((int)Model.TipoExibicao)"
-                                    allowFiltering="true" floatLabelType="Never" popupHeight="300px" zIndex="1000">
+                                <ejs-dropdownlist id="TipoExibicao"
+                                                  placeholder="Quando exibir o alerta"
+                                                  dataSource="@Model.TipoExibicaoList"
+                                                  value="@((int)Model.TipoExibicao)"
+                                                  allowFiltering="true"
+                                                  floatLabelType="Never"
+                                                  popupHeight="300px"
+                                                  zIndex="1000">
                                     <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                                 </ejs-dropdownlist>
                             </div>
@@ -291,29 +259,42 @@
 
                         <div class="row mt-3" id="rowCamposExibicao">
 
-                            <div class="col-md-3" id="divDataExibicao" style="display: none;">
+                            <div class="col-md-2" id="divDataExibicao" style="display: none;">
                                 <label class="label" id="lblDataExibicao">Data de Exibição</label>
-                                <ejs-datepicker id="DataExibicao" name="DataExibicao" placeholder="Selecione"
-                                    value="@Model.DataExibicao" format="dd/MM/yyyy" floatLabelType="Never"
-                                    locale="pt-BR">
+                                <ejs-datepicker id="DataExibicao"
+                                                name="DataExibicao"
+                                                placeholder="Selecione"
+                                                value="@Model.DataExibicao"
+                                                format="dd/MM/yyyy"
+                                                floatLabelType="Never"
+                                                locale="pt-BR">
                                 </ejs-datepicker>
                             </div>
 
-                            <div class="col-md-3" id="divHorarioExibicao" style="display: none;">
+                            <div class="col-md-2" id="divHorarioExibicao" style="display: none;">
                                 <label class="label" id="lblHorarioExibicao">Horário de Exibição</label>
-                                <ejs-timepicker id="HorarioExibicao" name="HorarioExibicao"
-                                    value="@Model.HorarioExibicao?.ToString(@"hh\:mm")" format="HH:mm" step="15"
-                                    floatLabelType="Never" locale="pt-BR" allowEdit="false" placeholder="Selecione">
+                                <ejs-timepicker id="HorarioExibicao"
+                                                name="HorarioExibicao"
+                                                value="@Model.HorarioExibicao?.ToString(@"hh\:mm")"
+                                                format="HH:mm"
+                                                step="15"
+                                                floatLabelType="Never"
+                                                locale="pt-BR"
+                                                allowEdit="false"
+                                                placeholder="Selecione">
                                 </ejs-timepicker>
-                                <span class="text-danger field-validation-valid"
-                                    data-valmsg-for="HorarioExibicao"></span>
-                            </div>
-
-                            <div class="col-md-3" id="divDataExpiracao" style="display: none;">
+                                <span class="text-danger field-validation-valid" data-valmsg-for="HorarioExibicao"></span>
+                            </div>
+
+                            <div class="col-md-2" id="divDataExpiracao" style="display: none;">
                                 <label class="label">Data de Expiração</label>
-                                <ejs-datepicker id="DataExpiracao" name="DataExpiracao" placeholder="Selecione"
-                                    value="@Model.DataExpiracao" format="dd/MM/yyyy" floatLabelType="Never"
-                                    locale="pt-BR">
+                                <ejs-datepicker id="DataExpiracao"
+                                                name="DataExpiracao"
+                                                placeholder="Selecione"
+                                                value="@Model.DataExpiracao"
+                                                format="dd/MM/yyyy"
+                                                floatLabelType="Never"
+                                                locale="pt-BR">
                                 </ejs-datepicker>
                             </div>
                         </div>
@@ -323,12 +304,17 @@
                             <div class="col-md-6" id="divDiasAlerta" style="display: none;">
                                 <label class="label font-weight-bold">
                                     Dias da Semana
-                                    <i class="fa-duotone fa-calendar-week field-icon"
-                                        style="font-size: 1.25em; cursor: pointer;"></i>
+                                    <i class="fa-duotone fa-calendar-week field-icon" style="font-size: 1.25em; cursor: pointer;"></i>
                                 </label>
-                                <ejs-multiselect id="lstDiasAlerta" name="DiasSemana" placeholder="Selecione os dias..."
-                                    mode="CheckBox" showSelectAll="true" selectAllText="Selecionar todos"
-                                    unSelectAllText="Desmarcar todos" popupHeight="250px" floatLabelType="Never">
+                                <ejs-multiselect id="lstDiasAlerta"
+                                                name="DiasSemana"
+                                                placeholder="Selecione os dias..."
+                                                mode="CheckBox"
+                                                showSelectAll="true"
+                                                selectAllText="Selecionar todos"
+                                                unSelectAllText="Desmarcar todos"
+                                                popupHeight="250px"
+                                                floatLabelType="Never">
                                     <e-multiselect-fields value="Value" text="Text"></e-multiselect-fields>
                                 </ejs-multiselect>
                             </div>
@@ -336,12 +322,15 @@
                             <div class="col-md-2" id="divDiaMesAlerta" style="display: none;">
                                 <label class="label font-weight-bold">
                                     Dia do Mês
-                                    <i class="fa-duotone fa-calendar-clock field-icon"
-                                        style="font-size: 1.25em; cursor: pointer;"></i>
+                                    <i class="fa-duotone fa-calendar-clock field-icon" style="font-size: 1.25em; cursor: pointer;"></i>
                                 </label>
-                                <ejs-dropdownlist id="lstDiasMesAlerta" name="DiaMesRecorrencia"
-                                    placeholder="Selecione..." value="@Model.DiaMesRecorrencia" popupHeight="250px"
-                                    floatLabelType="Never" cssClass="e-outline">
+                                <ejs-dropdownlist id="lstDiasMesAlerta"
+                                                name="DiaMesRecorrencia"
+                                                placeholder="Selecione..."
+                                                value="@Model.DiaMesRecorrencia"
+                                                popupHeight="250px"
+                                                floatLabelType="Never"
+                                                cssClass="e-outline">
                                     <e-dropdownlist-fields value="Value" text="Text"></e-dropdownlist-fields>
                                 </ejs-dropdownlist>
                             </div>
@@ -351,8 +340,7 @@
                             <div class="col-md-4 col-lg-3">
                                 <label class="label font-weight-bold d-block mb-2">
                                     Selecione as Datas
-                                    <i class="fa-duotone fa-calendar-days field-icon"
-                                        style="font-size: 1.25em; cursor: pointer;"></i>
+                                    <i class="fa-duotone fa-calendar-days field-icon" style="font-size: 1.25em; cursor: pointer;"></i>
                                 </label>
                                 <div style="position: relative; display: inline-block;">
 
@@ -376,8 +364,7 @@
 
                                     <div id="calDatasSelecionadasAlerta"></div>
 
-                                    <input type="hidden" id="DatasSelecionadas" name="DatasSelecionadas"
-                                        value="@Model.DatasSelecionadas" />
+                                    <input type="hidden" id="DatasSelecionadas" name="DatasSelecionadas" value="@Model.DatasSelecionadas" />
                                 </div>
                             </div>
                         </div>
@@ -391,13 +378,19 @@
                         <div class="row">
                             <div class="col-md-12">
                                 <label class="label required-field">Usuários</label>
-                                <ejs-multiselect id="UsuariosIds" name="UsuariosIds" placeholder="Selecione os usuários"
-                                    dataSource="@Model.UsuariosList" value="@Model.UsuariosIds" mode="Box"
-                                    showSelectAll="true" selectAllText="Selecionar Todos"
-                                    unSelectAllText="Desmarcar Todos" floatLabelType="Never"
-                                    cssClass="multiselect-usuarios" itemTemplate="usuariosItemTemplate"
-                                    valueTemplate="usuariosValueTemplate" closePopupOnSelect="false" appendTo="body">
-                                    <e-multiselect-fields text="NomeCompleto" value="Id"></e-multiselect-fields>
+                                <ejs-multiselect id="UsuariosIds" name="UsuariosIds"
+                                                 placeholder="Selecione os usuários"
+                                                 dataSource="@Model.UsuariosList"
+                                                 value="@Model.UsuariosIds"
+                                                 mode="Box"
+                                                 showSelectAll="true"
+                                                 selectAllText="Selecionar Todos"
+                                                 unSelectAllText="Desmarcar Todos"
+                                                 floatLabelType="Never"
+                                                 cssClass="multiselect-usuarios"
+                                                 closePopupOnSelect="false"
+                                                 appendTo="body">
+                                    <e-multiselect-fields text="Text" value="Value"></e-multiselect-fields>
                                 </ejs-multiselect>
                                 <span class="text-danger field-validation-valid" data-valmsg-for="UsuariosIds"></span>
                             </div>
@@ -405,8 +398,7 @@
 
                         <div class="row mt-4">
                             <div class="col-md-12 d-flex justify-content-end gap-2">
-                                <button type="button" class="btn btn-ftx-fechar form-control"
-                                    onclick="window.location.href='/AlertasFrotiX'" data-ftx-loading>
+                                <button type="button" class="btn btn-ftx-fechar form-control" onclick="window.location.href='/AlertasFrotiX'" data-ftx-loading>
                                     <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Cancelar Operação
                                 </button>
                                 <button type="submit" class="btn btn-azul btn-submit-spin form-control">
@@ -422,45 +414,14 @@
     </div>
 </div>
 
-<script id="usuariosItemTemplate" type="text/x-template">
-    <div class="user-item-multiselect">
-        <div class="user-icon-circle pointer-none">
-            ${NomeCompleto.substring(0,1).toUpperCase()}
-        </div>
-        <div class="user-info-text">
-            <span class="user-name-full">${NomeCompleto}</span>
-            <span class="user-ponto-text">${Ponto ? 'Ponto: ' + Ponto : 'Sem registro'}</span>
-        </div>
-    </div>
-</script>
-
-<script id="usuariosValueTemplate" type="text/x-template">
-    <div class="user-value-multiselect">
-        <span class="user-name-selected">${NomeCompleto}</span>
-    </div>
-</script>
-
 @section ScriptsBlock {
     <script src="~/js/alertasfrotix/alertas_upsert.js" asp-append-version="true"></script>
     <script src="~/js/alertasfrotix/alertas_recorrencia.js" asp-append-version="true"></script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * INICIALIZAÇÃO DOS CONTROLES DE RECORRÊNCIA V2
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Configura os dropdowns de dias da semana e dias do mês
-             * para alertas recorrentes. O controle é feito via TipoExibicao
-             * (valores 4-8 são recorrentes).
-             * @@requires alertas_recorrencia.js - Funções initCalendarioAlerta e verificarEstadoRecorrenciaAlerta
-            */
-        document.addEventListener('DOMContentLoaded', function () {
+        document.addEventListener('DOMContentLoaded', function() {
             try {
 
-                /**
-                 * DataSource para seleção de dias da semana
-                 * @@type {Array<{Text: string, Value: number}>}
-                 */
                 var dataDiasSemana = [
                     { Text: 'Domingo', Value: 0 },
                     { Text: 'Segunda-Feira', Value: 1 },
@@ -471,10 +432,6 @@
                     { Text: 'Sábado', Value: 6 }
                 ];
 
-                /**
-                 * DataSource para seleção de dia do mês (1-31)
-                 * @@type {Array<{Text: string, Value: number}>}
-                 */
                 var dataDiasMes = [];
                 for (var i = 1; i <= 31; i++) {
                     dataDiasMes.push({ Text: i.toString(), Value: i });
@@ -513,17 +470,9 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CONFIGURAÇÃO DO TIMEPICKER - HORÁRIO DE EXIBIÇÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Configura o componente Syncfusion TimePicker para seleção
-            * de horário de exibição do alerta com intervalos de 15 minutos.
-             */
-        document.addEventListener('DOMContentLoaded', function () {
+        document.addEventListener('DOMContentLoaded', function() {
             try {
-                    /** @@type { ej.calendars.TimePicker } Instância do TimePicker Syncfusion */
-                    var timePicker = document.getElementById('HorarioExibicao').ej2_instances[0];
+                var timePicker = document.getElementById('HorarioExibicao').ej2_instances[0];
 
                 if (timePicker) {
                     timePicker.locale = 'pt-BR';
@@ -532,23 +481,15 @@
                     timePicker.dataBind();
                 }
 
-                    /**
-                     * Callback quando o popup é criado
-                     * @@param { Object } args - Argumentos do evento
-                    */
-                    timePicker.popupCreated = function (args) {
-                        try {
-                            console.log('Popup criado com sucesso');
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("Upsert.cshtml", "timePicker.popupCreated", error);
-                        }
-                    };
-
-                    /**
-                     * Callback quando o popup é aberto - posiciona abaixo do input
-                     * @@param { Object } args - Argumentos do evento com referência ao popup
-                    */
-                timePicker.open = function (args) {
+                timePicker.popupCreated = function(args) {
+                    try {
+                        console.log('Popup criado com sucesso');
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Upsert.cshtml", "timePicker.popupCreated", error);
+                    }
+                };
+
+                timePicker.open = function(args) {
                     try {
                         if (args && args.popup) {
                             args.popup.position = { X: 'left', Y: 'bottom' };
@@ -558,25 +499,18 @@
                     }
                 };
 
-                    /**
-                     * Callback antes de abrir o popup - valida se pode ser criado
-                     * @@param { Object } args - Argumentos do evento(args.cancel pode ser setado)
-                    */
-                    timePicker.beforeOpen = function (args) {
-                        try {
-                            if (!args || !args.popup) {
-                                args.cancel = true;
-                                console.error('Popup não pode ser criado');
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("Upsert.cshtml", "timePicker.beforeOpen", error);
+                timePicker.beforeOpen = function(args) {
+                    try {
+                        if (!args || !args.popup) {
+                            args.cancel = true;
+                            console.error('Popup não pode ser criado');
                         }
-                    };
-
-                /**
-                 * Handler de clique no input - abre o popup manualmente se fechado
-                 */
-                timePicker.inputElement.addEventListener('click', function (e) {
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Upsert.cshtml", "timePicker.beforeOpen", error);
+                    }
+                };
+
+                timePicker.inputElement.addEventListener('click', function(e) {
                     try {
                         if (!timePicker.isPopupOpen()) {
                             timePicker.show();
@@ -596,14 +530,6 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CONFIGURAÇÃO DO FLATPICKR - SELETOR DE HORÁRIO ALTERNATIVO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Inicializa o Flatpickr como seletor de horário alternativo
-            * com formato 24h e incrementos de 15 minutos.
-             * @@requires flatpickr - Biblioteca de seleção de data / hora
-            */
         document.addEventListener('DOMContentLoaded', function () {
             try {
                 flatpickr("#dtpHorarioExibicao", {
```
