# Pages/Manutencao/ControleLavagem.cshtml

**Mudanca:** GRANDE | **+91** linhas | **-167** linhas

---

```diff
--- JANEIRO: Pages/Manutencao/ControleLavagem.cshtml
+++ ATUAL: Pages/Manutencao/ControleLavagem.cshtml
@@ -36,7 +36,7 @@
         .ftx-card-styled {
             border: none;
             border-radius: 12px;
-            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
+            box-shadow: 0 4px 20px rgba(0,0,0,0.08);
             overflow: hidden;
         }
 
@@ -51,7 +51,7 @@
 
         .ftx-card-header .titulo-paginas {
             color: #fff !important;
-            text-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
+            text-shadow: 0 2px 4px rgba(0,0,0,0.2);
             margin: 0;
             font-size: 1.5rem;
         }
@@ -59,7 +59,7 @@
         /* Ícone do header: cores definidas no frotix.css global */
         .ftx-card-header .titulo-paginas i.fa-duotone {
             font-size: 1.8rem;
-            filter: drop-shadow(0 2px 3px rgba(0, 0, 0, 0.2));
+            filter: drop-shadow(0 2px 3px rgba(0,0,0,0.2));
         }
 
         /* ===== BOTÃO HEADER LARANJA ===== */
@@ -97,7 +97,7 @@
         .ftx-section-card {
             background: #fff;
             border-radius: 10px;
-            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
+            box-shadow: 0 2px 8px rgba(0,0,0,0.06);
             margin-bottom: 1.5rem;
             overflow: hidden;
             border-left: 4px solid #325d88;
@@ -114,7 +114,7 @@
         .ftx-section-header {
             background: linear-gradient(135deg, #f8f9fa 0%, #ffffff 100%);
             padding: 0.875rem 1.25rem;
-            border-bottom: 1px solid rgba(0, 0, 0, 0.06);
+            border-bottom: 1px solid rgba(0,0,0,0.06);
             display: flex;
             align-items: center;
             gap: 0.75rem;
@@ -290,13 +290,7 @@
 }
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * CONTROLE DE ESTADO: VARIÁVEIS GLOBAIS E FLAGS DE FILTRO
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Variáveis que controlam qual filtro está ativo (veículo,
-     * motorista, data ou lavador). Evita conflitos entre filtros.
-     */
+
     var URLapi = "/api/manutencao/ListaLavagens";
     var IDapi = "";
 
@@ -305,16 +299,6 @@
     var escolhendoData = false;
     var escolhendoLavador = false;
 
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * FUNÇÕES DE DEFINIÇÃO DE ESCOLHA (FOCO NOS FILTROS)
-     * ═══════════════════════════════════════════════════════════════════════════
-     */
-
-    /**
-     * Ativa flag indicando que usuário está selecionando veículo.
-     * @@description Reseta outras flags para evitar conflitos de filtro.
-     */
     function DefineEscolhaVeiculo() {
         try {
             escolhendoVeiculo = true;
@@ -331,10 +315,6 @@
         }
     }
 
-    /**
-     * Ativa flag indicando que usuário está selecionando motorista.
-     * @@description Reseta outras flags para evitar conflitos de filtro.
-     */
     function DefineEscolhaMotorista() {
         try {
             escolhendoVeiculo = false;
@@ -351,10 +331,6 @@
         }
     }
 
-    /**
-     * Ativa flag indicando que usuário está selecionando data.
-     * @@description Reseta outras flags para evitar conflitos de filtro.
-     */
     function DefineEscolhaData() {
         try {
             escolhendoVeiculo = false;
@@ -366,10 +342,6 @@
         }
     }
 
-    /**
-     * Ativa flag indicando que usuário está selecionando lavador.
-     * @@description Reseta outras flags para evitar conflitos de filtro.
-     */
     function DefineEscolhaLavador() {
         try {
             escolhendoVeiculo = false;
@@ -386,16 +358,6 @@
         }
     }
 
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * FUNÇÕES DE LISTAGEM E CARREGAMENTO DE DADOS
-     * ═══════════════════════════════════════════════════════════════════════════
-     */
-
-    /**
-     * Lista todas as lavagens sem filtro.
-     * @@description Define URL base da API e chama ListaTblLavagens.
-     */
     function ListaTodasLavagens() {
         try {
             URLapi = "/api/manutencao/ListaLavagens";
@@ -406,16 +368,6 @@
         }
     }
 
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * CALLBACKS DE MUDANÇA DE VALOR (VALUE CHANGE)
-     * ═══════════════════════════════════════════════════════════════════════════
-     */
-
-    /**
-     * Callback disparado quando o ComboBox de veículos muda.
-     * @@description Limpa outros filtros e recarrega tabela filtrada por veículo.
-     */
     function VeiculosValueChange() {
         try {
             if (!escolhendoVeiculo) return;
@@ -436,10 +388,6 @@
         }
     }
 
-    /**
-     * Callback disparado quando o ComboBox de motoristas muda.
-     * @@description Limpa outros filtros e recarrega tabela filtrada por motorista.
-     */
     function MotoristaValueChange() {
         try {
             if (!escolhendoMotorista) return;
@@ -460,10 +408,6 @@
         }
     }
 
-    /**
-     * Callback disparado quando o ComboBox de lavadores muda.
-     * @@description Limpa outros filtros e recarrega tabela filtrada por lavador.
-     */
     function LavadorValueChange() {
         try {
             if (!escolhendoLavador) return;
@@ -513,44 +457,46 @@
                                             <input id="txtDataLavagem" class="form-control" type="date" />
                                         </div>
                                         <div class="col-md-2">
-                                            <label class="ftx-label">Hora Início</label>
+                                            <label class="ftx-label">Hora da Lavagem</label>
                                             <input id="txtHoraLavagem" class="form-control" type="time" />
                                         </div>
-                                        <div class="col-md-2">
-                                            <label class="ftx-label">Hora Fim</label>
-                                            <input id="txtHoraFimLavagem" class="form-control" type="time" />
-                                        </div>
-                                        <div class="col-md-3">
+                                        <div class="col-md-4">
                                             <label class="ftx-label">Veículo Lavado</label>
-                                            <ejs-combobox id="lstVeiculoLavagem" placeholder="Selecione um Veículo"
-                                                allowFiltering="true" filterType="Contains"
-                                                dataSource="@ViewData["lstVeiculos"]" popupHeight="250px" width="100%"
-                                                showClearButton="true">
+                                            <ejs-combobox id="lstVeiculoLavagem"
+                                                          placeholder="Selecione um Veículo"
+                                                          allowFiltering="true"
+                                                          filterType="Contains"
+                                                          dataSource="@ViewData["lstVeiculos"]"
+                                                          popupHeight="250px"
+                                                          width="100%"
+                                                          showClearButton="true">
                                                 <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                             </ejs-combobox>
                                         </div>
-                                        <div class="col-md-3">
+                                        <div class="col-md-4">
                                             <label class="ftx-label">Condutor do Veículo</label>
-                                            <ejs-combobox id="lstMotoristaLavagem" placeholder="Selecione um Motorista"
-                                                allowFiltering="true" filterType="Contains"
-                                                dataSource="@ViewData["lstMotorista"]" popupHeight="250px" width="100%"
-                                                showClearButton="true">
+                                            <ejs-combobox id="lstMotoristaLavagem"
+                                                          placeholder="Selecione um Motorista"
+                                                          allowFiltering="true"
+                                                          filterType="Contains"
+                                                          dataSource="@ViewData["lstMotorista"]"
+                                                          popupHeight="250px"
+                                                          width="100%"
+                                                          showClearButton="true">
                                                 <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                             </ejs-combobox>
                                         </div>
                                         <div class="col-12">
                                             <label class="ftx-label">Lavadores Participantes</label>
                                             <select id="lstLavadores"
-                                                data-placeholder="Selecione os lavadores participantes da lavagem..."
-                                                style="width:100%; max-width:600px;"></select>
+                                                    data-placeholder="Selecione os lavadores participantes da lavagem..."
+                                                    style="width:100%; max-width:600px;"></select>
                                         </div>
                                         <div class="col-12 d-flex gap-2 mt-2">
-                                            <button id="btnInserir" type="button" class="btn btn-inserir-ftx"
-                                                data-ejtip="Inserir Lavagem">
+                                            <button id="btnInserir" type="button" class="btn btn-inserir-ftx" data-ejtip="Inserir Lavagem">
                                                 <i class="fa-duotone fa-file-plus icon-pulse"></i> Inserir
                                             </button>
-                                            <button id="btnLimpar" type="button" class="btn btn-limpar-ftx"
-                                                data-ejtip="Limpar Campos">
+                                            <button id="btnLimpar" type="button" class="btn btn-limpar-ftx" data-ejtip="Limpar Campos">
                                                 <i class="fa-duotone fa-rotate-left icon-rotate-left"></i> Limpar
                                             </button>
                                         </div>
@@ -573,31 +519,46 @@
                                         </div>
                                         <div class="col-md-3">
                                             <label class="ftx-label">Veículo</label>
-                                            <ejs-combobox id="lstVeiculos" placeholder="Selecione um Veículo"
-                                                allowFiltering="true" filterType="Contains"
-                                                dataSource="@ViewData["lstVeiculos"]" popupHeight="250px"
-                                                change="DefineEscolhaVeiculo" width="100%" showClearButton="true"
-                                                close="VeiculosValueChange">
+                                            <ejs-combobox id="lstVeiculos"
+                                                          placeholder="Selecione um Veículo"
+                                                          allowFiltering="true"
+                                                          filterType="Contains"
+                                                          dataSource="@ViewData["lstVeiculos"]"
+                                                          popupHeight="250px"
+                                                          change="DefineEscolhaVeiculo"
+                                                          width="100%"
+                                                          showClearButton="true"
+                                                          close="VeiculosValueChange">
                                                 <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                             </ejs-combobox>
                                         </div>
                                         <div class="col-md-3">
                                             <label class="ftx-label">Motorista</label>
-                                            <ejs-combobox id="lstMotorista" placeholder="Selecione um Motorista"
-                                                allowFiltering="true" filterType="Contains"
-                                                dataSource="@ViewData["lstMotorista"]" popupHeight="250px"
-                                                change="DefineEscolhaMotorista" width="100%" showClearButton="true"
-                                                close="MotoristaValueChange">
+                                            <ejs-combobox id="lstMotorista"
+                                                          placeholder="Selecione um Motorista"
+                                                          allowFiltering="true"
+                                                          filterType="Contains"
+                                                          dataSource="@ViewData["lstMotorista"]"
+                                                          popupHeight="250px"
+                                                          change="DefineEscolhaMotorista"
+                                                          width="100%"
+                                                          showClearButton="true"
+                                                          close="MotoristaValueChange">
                                                 <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                             </ejs-combobox>
                                         </div>
                                         <div class="col-md-3">
                                             <label class="ftx-label">Lavador</label>
-                                            <ejs-combobox id="lstLavador" placeholder="Selecione um Lavador"
-                                                allowFiltering="true" filterType="Contains"
-                                                dataSource="@ViewData["lstLavadores"]" popupHeight="250px" width="100%"
-                                                showClearButton="true" change="DefineEscolhaLavador"
-                                                close="LavadorValueChange">
+                                            <ejs-combobox id="lstLavador"
+                                                          placeholder="Selecione um Lavador"
+                                                          allowFiltering="true"
+                                                          filterType="Contains"
+                                                          dataSource="@ViewData["lstLavadores"]"
+                                                          popupHeight="250px"
+                                                          width="100%"
+                                                          showClearButton="true"
+                                                          change="DefineEscolhaLavador"
+                                                          close="LavadorValueChange">
                                                 <e-combobox-fields text="Nome" value="LavadorId"></e-combobox-fields>
                                             </ejs-combobox>
                                         </div>
@@ -606,8 +567,7 @@
                             </div>
 
                             <div id="divViagens">
-                                <table id="tblLavagem" class="table table-bordered table-striped ftx-table"
-                                    width="100%">
+                                <table id="tblLavagem" class="table table-bordered table-striped ftx-table" width="100%">
                                     <thead>
                                         <tr>
                                             <th>Data</th>
@@ -615,7 +575,7 @@
                                             <th>Veículo</th>
                                             <th>Motorista</th>
                                             <th>Lavadores</th>
-                                            <th>Ações</th>
+                                            <th>Ação</th>
                                             <th>#</th>
                                         </tr>
                                     </thead>
@@ -633,28 +593,26 @@
 
 @section ScriptsBlock {
 
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script src="https://kendo.cdn.telerik.com/2022.1.412/js/jszip.min.js"></script>
     <script src="https://kendo.cdn.telerik.com/2022.1.412/js/kendo.all.min.js"></script>
     <script src="https://kendo.cdn.telerik.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
-
+    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/cultures/kendo.culture.pt-BR.min.js"></script>
+    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/messages/kendo.messages.pt-BR.min.js"></script>
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * BLOCO PRINCIPAL DE SCRIPTS - CONTROLE DE LAVAGEM
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description DataTable de lavagens, CRUD completo, filtros por data,
-         * veículo, motorista e lavador. Usa Kendo MultiSelect.
-         */
-
-        /**
-         * Lista todas as lavagens sem filtro.
-         * @@description Wrapper que exibe loading e chama ListaTblLavagens.
-         */
+        try {
+            if (window.kendo && kendo.culture) {
+                kendo.culture("pt-BR");
+            }
+        } catch (error) {
+            console.warn("Kendo pt-BR: falha ao aplicar cultura.", error);
+        }
+    </script>
+
+    <script>
+
         function ListaTodasLavagens() {
             try {
                 FtxSpin.show("Carregando Lavagens");
@@ -665,11 +623,6 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * DOCUMENT READY - INICIALIZAÇÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
         $(document).ready(function () {
             try {
 
@@ -735,19 +688,14 @@
             }
         });
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * DATATABLE DE LAVAGENS
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Carrega/recarrega o DataTable de lavagens.
-         * @@param {string} URLapi - Endpoint da API para buscar dados.
-         * @@param {string} IDapi - ID para filtro (veículo, motorista, etc.).
-         * @@description Destrói tabela existente, recria com dados da API.
-         * Inclui botões de exportação Excel/PDF e paginação.
-         */
+        $("#btnLimpar").click(function () {
+            try {
+                LimpaControles();
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("ControleLavagem.cshtml", "#btnLimpar.click", error);
+            }
+        });
+
         function ListaTblLavagens(URLapi, IDapi) {
             try {
                 var dt = $('#tblLavagem').DataTable();
@@ -800,15 +748,15 @@
                             data: "lavagemId",
                             render: function (data) {
                                 return `
-                                        <div class="text-center">
-                                            <a class="btn btn-apagar btn-apagar-ftx"
-                                               aria-label="Apagar a Lavagem!"
-                                               data-ejtip="Apagar Lavagem"
-                                               style="cursor:pointer;"
-                                               data-id='${data}'>
-                                               <i class="fa-duotone fa-trash-can"></i>
-                                            </a>
-                                        </div>`;
+                                    <div class="text-center">
+                                        <a class="btn btn-apagar btn-apagar-ftx"
+                                           aria-label="Apagar a Lavagem!"
+                                           data-ejtip="Apagar Lavagem"
+                                           style="cursor:pointer;"
+                                           data-id='${data}'>
+                                           <i class="fa-duotone fa-trash-can"></i>
+                                        </a>
+                                    </div>`;
                             }
                         },
                         {
@@ -852,17 +800,6 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * CRUD DE LAVAGENS
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Handler do botão Inserir Lavagem.
-         * @@description Valida campos obrigatórios (data, hora, veículo, motorista,
-         * lavadores), envia para API e associa lavadores selecionados.
-         */
         $("#btnInserir").click(function (e) {
             try {
                 e.preventDefault();
@@ -875,14 +812,7 @@
 
                 var HoraLavagem = $("#txtHoraLavagem").val();
                 if (!HoraLavagem) {
-                    Alerta.Erro("Erro na Hora", "A hora de início da lavagem é obrigatória!");
-                    return;
-                }
-
-                var HoraFimLavagem = $("#txtHoraFimLavagem").val();
-
-                if (HoraFimLavagem && HoraFimLavagem <= HoraLavagem) {
-                    Alerta.Erro("Erro na Hora", "A hora de fim deve ser posterior à hora de início!");
+                    Alerta.Erro("Erro na Hora", "A hora da lavagem é obrigatória!");
                     return;
                 }
 
@@ -906,8 +836,7 @@
 
                 var objLavagem = JSON.stringify({
                     "Data": DataLavagem,
-                    "HorarioInicio": HoraLavagem,
-                    "HorarioFim": HoraFimLavagem || null,
+                    "HorarioLavagem": HoraLavagem,
                     "VeiculoId": veiculo.value,
                     "MotoristaId": motorista.value
                 });
@@ -963,15 +892,10 @@
             }
         });
 
-        /**
-         * Limpa todos os campos do formulário de inserção.
-         * @@description Reseta data, horários, ComboBoxes e MultiSelect.
-         */
         function LimpaControles() {
             try {
                 $("#txtDataLavagem").val("");
                 $("#txtHoraLavagem").val("");
-                $("#txtHoraFimLavagem").val("");
 
                 var veiculo = document.getElementById('lstVeiculoLavagem').ej2_instances[0]; veiculo.value = null;
                 var motorista = document.getElementById('lstMotoristaLavagem').ej2_instances[0]; motorista.value = null;
@@ -983,11 +907,6 @@
             }
         }
 
-        /**
-         * Handler delegado para exclusão de lavagem.
-         * @@description Exibe confirmação SweetAlert, envia DELETE para API
-         * e recarrega tabela em caso de sucesso.
-         */
         $(document).on('click', '.btn-apagar', function () {
             try {
                 var id = $(this).data('id');
```
