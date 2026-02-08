# Pages/Escalas/FichaEscalas.cshtml

**Mudanca:** GRANDE | **+44** linhas | **-115** linhas

---

```diff
--- JANEIRO: Pages/Escalas/FichaEscalas.cshtml
+++ ATUAL: Pages/Escalas/FichaEscalas.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Escalas.FichaEscalasModel
 @{
     ViewData["Title"] = "Ficha de Escala";
@@ -10,31 +9,30 @@
     diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);
 
     var servicosGeraisMatutino = Model.Escalas
-    .Where(e => e.NomeTurno == "Matutino" && e.NomeServico == "Serviços Gerais" && e.StatusMotorista != "Indisponível")
-    .OrderBy(e => e.HoraInicio).ToList();
+        .Where(e => e.NomeTurno == "Matutino" && e.NomeServico == "Serviços Gerais" && e.StatusMotorista != "Indisponível")
+        .OrderBy(e => e.HoraInicio).ToList();
 
     var economildoMatutino = Model.Escalas
-    .Where(e => e.NomeTurno == "Matutino" && e.NomeServico == "Economildo" && e.StatusMotorista != "Indisponível")
-    .OrderBy(e => e.HoraInicio).ToList();
+        .Where(e => e.NomeTurno == "Matutino" && e.NomeServico == "Economildo" && e.StatusMotorista != "Indisponível")
+        .OrderBy(e => e.HoraInicio).ToList();
 
     var servicosGeraisVespertino = Model.Escalas
-    .Where(e => e.NomeTurno == "Vespertino" && e.NomeServico == "Serviços Gerais" && e.StatusMotorista != "Indisponível")
-    .OrderBy(e => e.HoraInicio).ToList();
+        .Where(e => e.NomeTurno == "Vespertino" && e.NomeServico == "Serviços Gerais" && e.StatusMotorista != "Indisponível")
+        .OrderBy(e => e.HoraInicio).ToList();
 
     var economildoVespertino = Model.Escalas
-    .Where(e => e.NomeTurno == "Vespertino" && e.NomeServico == "Economildo" && e.StatusMotorista != "Indisponível")
-    .OrderBy(e => e.HoraInicio).ToList();
+        .Where(e => e.NomeTurno == "Vespertino" && e.NomeServico == "Economildo" && e.StatusMotorista != "Indisponível")
+        .OrderBy(e => e.HoraInicio).ToList();
 
     var servicosGeraisNoturno = Model.Escalas
-    .Where(e => e.NomeTurno == "Noturno" && e.NomeServico == "Serviços Gerais" && e.StatusMotorista != "Indisponível")
-    .OrderBy(e => e.HoraInicio).ToList();
+        .Where(e => e.NomeTurno == "Noturno" && e.NomeServico == "Serviços Gerais" && e.StatusMotorista != "Indisponível")
+        .OrderBy(e => e.HoraInicio).ToList();
 
     var ferias = Model.Escalas.Where(e => e.MotivoCobertura == "Férias" && e.StatusMotorista == "Indisponível").ToList();
     var folgas = Model.Escalas.Where(e => e.MotivoCobertura == "Folga" && e.StatusMotorista == "Indisponível").ToList();
     var faltas = Model.Escalas.Where(e => e.MotivoCobertura == "Falta" && e.StatusMotorista == "Indisponível").ToList();
     var recessos = Model.Escalas.Where(e => e.MotivoCobertura == "Recesso" && e.StatusMotorista == "Indisponível").ToList();
-    var atestados = Model.Escalas.Where(e => e.MotivoCobertura == "Atestado" && e.StatusMotorista ==
-    "Indisponível").ToList();
+    var atestados = Model.Escalas.Where(e => e.MotivoCobertura == "Atestado" && e.StatusMotorista == "Indisponível").ToList();
 
     var afastamentos = folgas.Concat(faltas).Concat(recessos).Concat(atestados).ToList();
 }
@@ -44,7 +42,6 @@
     /* ESTILOS DE IMPRESSÃO / EXPORTAÇÃO PDF */
     /* ============================================= */
     @@media print {
-
         /* Configuração da página com margens */
         @@page {
             margin: 10mm;
@@ -52,8 +49,7 @@
         }
 
         /* Configurações gerais */
-        html,
-        body {
+        html, body {
             margin: 0 !important;
             padding: 0 !important;
             font-size: 9pt;
@@ -184,11 +180,13 @@
         }
 
         .saida-box.hachurado {
-            background: repeating-linear-gradient(45deg,
-                    #666,
-                    #666 2px,
-                    #fff 2px,
-                    #fff 4px) !important;
+            background: repeating-linear-gradient(
+                45deg,
+                #666,
+                #666 2px,
+                #fff 2px,
+                #fff 4px
+            ) !important;
             -webkit-print-color-adjust: exact !important;
             print-color-adjust: exact !important;
         }
@@ -232,8 +230,7 @@
         font-size: 10pt;
     }
 
-    .ficha-table th,
-    .ficha-table td {
+    .ficha-table th, .ficha-table td {
         border: 1px solid #000;
         padding: 4px 6px;
         text-align: left;
@@ -254,61 +251,36 @@
         color: white;
     }
 
-    .col-horario {
-        width: 60px;
-        text-align: center;
-    }
-
-    .col-qtd {
-        width: 40px;
-        text-align: center;
-    }
-
-    .col-motorista {
-        width: 180px;
-    }
-
-    .col-saidas {
-        width: 100px;
-    }
-
-    .col-veiculo {
-        width: 80px;
-        text-align: center;
-    }
-
-    .col-lotacao {
-        width: 120px;
-    }
-
-    .col-fim {
-        width: 60px;
-        text-align: center;
-    }
+    .col-horario { width: 60px; text-align: center; }
+    .col-qtd { width: 40px; text-align: center; }
+    .col-motorista { width: 180px; }
+    .col-saidas { width: 100px; }
+    .col-veiculo { width: 80px; text-align: center; }
+    .col-lotacao { width: 120px; }
+    .col-fim { width: 60px; text-align: center; }
 
     /* Quadradinhos de saídas */
     .saidas-container {
         display: flex;
         gap: 2px;
     }
-
     .saida-box {
         width: 14px;
         height: 14px;
         border: 1px solid #000;
         display: inline-block;
     }
-
     .saida-box.preenchido {
         background-color: #000;
     }
-
     .saida-box.hachurado {
-        background: repeating-linear-gradient(45deg,
-                #666,
-                #666 2px,
-                #fff 2px,
-                #fff 4px);
+        background: repeating-linear-gradient(
+            45deg,
+            #666,
+            #666 2px,
+            #fff 2px,
+            #fff 4px
+        );
     }
 
     /* Selo de Cobertura */
@@ -324,7 +296,7 @@
         font-size: 10px;
         font-weight: bold;
         margin-left: 5px;
-        box-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
+        box-shadow: 0 1px 2px rgba(0,0,0,0.3);
     }
 
     /* Seções de férias/afastamentos */
@@ -333,12 +305,10 @@
         gap: 20px;
         margin-top: 15px;
     }
-
     .afastamento-box {
         flex: 1;
         border: 1px solid #000;
     }
-
     .afastamento-header {
         background-color: #2c3e50;
         color: white;
@@ -348,20 +318,16 @@
         border-bottom: 1px solid #000;
         font-size: 10pt;
     }
-
     .afastamento-table {
         width: 100%;
         border-collapse: collapse;
         font-size: 9pt;
     }
-
     .afastamento-table th {
         background-color: #4a6278;
         color: white;
     }
-
-    .afastamento-table th,
-    .afastamento-table td {
+    .afastamento-table th, .afastamento-table td {
         border: 1px solid #000;
         padding: 3px 5px;
     }
@@ -373,7 +339,6 @@
         margin-top: 15px;
         min-height: 80px;
     }
-
     .observacoes-header {
         font-weight: bold;
         margin-bottom: 5px;
@@ -394,7 +359,8 @@
         </button>
         <label for="dataFiltro" class="mb-0 ms-auto">Data:</label>
         <input type="date" id="dataFiltro" class="form-control" style="width: 160px;"
-            value="@dataEscala.ToString("yyyy-MM-dd")" onchange="filtrarPorData()" />
+               value="@dataEscala.ToString("yyyy-MM-dd")"
+               onchange="filtrarPorData()" />
     </div>
 
     <div class="ficha-container" id="fichaConteudo">
@@ -419,9 +385,7 @@
                     </tr>
                 </thead>
                 <tbody>
-                    @{
-                        int qtdSGM = 0;
-                    }
+                    @{ int qtdSGM = 0; }
                     @foreach (var escala in servicosGeraisMatutino)
                     {
                         qtdSGM++;
@@ -472,9 +436,7 @@
                     </tr>
                 </thead>
                 <tbody>
-                    @{
-                        int qtdEM = 0;
-                    }
+                    @{ int qtdEM = 0; }
                     @foreach (var escala in economildoMatutino)
                     {
                         qtdEM++;
@@ -526,9 +488,7 @@
                     </tr>
                 </thead>
                 <tbody>
-                    @{
-                        int qtdSGV = 0;
-                    }
+                    @{ int qtdSGV = 0; }
                     @foreach (var escala in servicosGeraisVespertino)
                     {
                         qtdSGV++;
@@ -579,9 +539,7 @@
                     </tr>
                 </thead>
                 <tbody>
-                    @{
-                        int qtdEV = 0;
-                    }
+                    @{ int qtdEV = 0; }
                     @foreach (var escala in economildoVespertino)
                     {
                         qtdEV++;
@@ -632,9 +590,7 @@
                     </tr>
                 </thead>
                 <tbody>
-                    @{
-                        int qtdSGN = 0;
-                    }
+                    @{ int qtdSGN = 0; }
                     @foreach (var escala in servicosGeraisNoturno)
                     {
                         qtdSGN++;
@@ -671,8 +627,7 @@
 
             <div class="afastamento-box">
                 <div class="afastamento-header">
-                    FÉRIAS - @DateTime.Now.ToString("MMMM yyyy", new
-                                        System.Globalization.CultureInfo("pt-BR")).ToUpper()
+                    FÉRIAS - @DateTime.Now.ToString("MMMM yyyy", new System.Globalization.CultureInfo("pt-BR")).ToUpper()
                 </div>
                 <table class="afastamento-table">
                     <thead>
@@ -754,25 +709,7 @@
 
 @section ScriptsBlock {
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * FICHA DE ESCALAS - SCRIPTS DE CONTROLE
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Funções para filtro por data, exportação PDF e carregamento
-         * de observações do dia via AJAX
-         */
-
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * FILTRO POR DATA
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Aplica filtro por data redirecionando a página
-         * @@description Redireciona para a mesma página com parâmetro DataEscala
-         * @@global
-         */
+
         function filtrarPorData() {
             try {
                 var dataInput = document.getElementById('dataFiltro');
@@ -787,17 +724,6 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * EXPORTAÇÃO PDF
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Exporta a ficha para PDF via impressão do navegador
-         * @@description Abre diálogo de impressão nativo do navegador
-         * @@global
-         */
         function exportarPDF() {
             try {
                 console.log('Exportando PDF...');
@@ -810,17 +736,6 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * CARREGAMENTO DE OBSERVAÇÕES DO DIA
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Carrega observações da escala para a data selecionada
-         * @@description Faz requisição AJAX para /api/Escala/GetObservacoesDia
-         * e popula o container #observacoesConteudo com os resultados
-         */
         function carregarObservacoesFicha() {
             try {
                 var dataInput = document.getElementById('dataFiltro');
@@ -879,12 +794,6 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * INICIALIZAÇÃO DO DOCUMENTO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Carrega observações do dia e configura handler do botão PDF
-         */
         $(document).ready(function () {
             try {
                 console.log('FichaEscalas inicializando...');
```
