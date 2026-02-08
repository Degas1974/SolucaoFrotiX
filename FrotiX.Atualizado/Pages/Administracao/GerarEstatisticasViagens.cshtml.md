# Pages/Administracao/GerarEstatisticasViagens.cshtml

**Mudanca:** GRANDE | **+27** linhas | **-97** linhas

---

```diff
--- JANEIRO: Pages/Administracao/GerarEstatisticasViagens.cshtml
+++ ATUAL: Pages/Administracao/GerarEstatisticasViagens.cshtml
@@ -1,5 +1,4 @@
 @page "/Administracao/gerarestatisticasviagens"
-
 @model FrotiX.Pages.Administracao.GerarEstatisticasViagensModel
 @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
 
@@ -31,7 +30,7 @@
         padding: 25px;
         background: white;
         border-radius: 8px;
-        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
+        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
     }
 
     .progress-info {
@@ -61,7 +60,7 @@
         border-radius: 8px;
         overflow: hidden;
         position: relative;
-        box-shadow: inset 0 2px 4px rgba(0, 0, 0, 0.1);
+        box-shadow: inset 0 2px 4px rgba(0,0,0,0.1);
     }
 
     .custom-progress-fill {
@@ -84,7 +83,7 @@
         font-weight: bold;
         font-size: 16px;
         line-height: 45px;
-        text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
+        text-shadow: 0 1px 2px rgba(0,0,0,0.3);
         z-index: 10;
     }
 
@@ -106,7 +105,7 @@
 
     .stat-card:hover {
         transform: translateY(-2px);
-        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
+        box-shadow: 0 4px 8px rgba(0,0,0,0.1);
     }
 
     .stat-label {
@@ -148,11 +147,12 @@
         top: 50%;
         left: 20px;
         margin-top: -8px;
-        border: 2px solid rgba(255, 255, 255, 0.3);
+        border: 2px solid rgba(255,255,255,0.3);
         border-radius: 50%;
         border-top-color: white;
         animation: spinner 0.8s linear infinite;
     }
+
 </style>
 
 <div class="row">
@@ -172,8 +172,7 @@
                         <div class="col-12">
                             <p class="text-muted mb-3">
                                 <i class="fa-duotone fa-info-circle"></i>
-                                Esta operação irá processar e armazenar as estatísticas consolidadas de todas as viagens
-                                no sistema.
+                                Esta operação irá processar e armazenar as estatísticas consolidadas de todas as viagens no sistema.
                             </p>
                         </div>
                     </div>
@@ -226,34 +225,11 @@
 
 @section ScriptsBlock {
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * GERAÇÃO DE ESTATÍSTICAS DE VIAGENS
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Sistema de geração de estatísticas agregadas de viagens
-            * Processa dados em batch com barra de progresso e monitoramento em tempo real
-                */
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * VARIÁVEIS GLOBAIS
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /** @@type { number | null } ID do intervalo de polling */
         let intervalId = null;
-            /** @@type { Date | null } Timestamp de início do processamento */
-            let inicioProcessamento = null;
-            /** @@type { number } Último número de registros processados(para cálculo de velocidade) */
+        let inicioProcessamento = null;
         let ultimoProcessado = 0;
-            /** @@type { Date | null } Último timestamp de consulta(para cálculo de velocidade) */
         let ultimoTempo = null;
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * INICIALIZAÇÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
         $(document).ready(function () {
             try {
                 $("#btnGerarEstatisticas").click(function () {
@@ -264,17 +240,6 @@
             }
         });
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÃO PRINCIPAL - GERAÇÃO DE ESTATÍSTICAS
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Inicia a geração de estatísticas de viagens
-             * @@description Faz POST para API e inicia monitoramento de progresso
-            * @@returns { void}
-            */
         function gerarEstatisticas() {
             try {
                 marcarBotaoProcessando();
@@ -311,17 +276,6 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CONTROLE DO BOTÃO E BARRA DE PROGRESSO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Marca o botão como processando
-             * @@description Desabilita e mostra spinner
-            * @@returns { void}
-            */
         function marcarBotaoProcessando() {
             try {
                 const btn = $("#btnGerarEstatisticas");
@@ -333,10 +287,6 @@
             }
         }
 
-            /**
-             * Restaura o botão ao estado inicial
-             * @@returns { void}
-             */
         function desmarcarBotaoProcessando() {
             try {
                 const btn = $("#btnGerarEstatisticas");
@@ -348,10 +298,6 @@
             }
         }
 
-            /**
-             * Exibe a barra de progresso com animação
-             * @@returns { void}
-             */
         function mostrarBarraProgresso() {
             try {
                 $("#progressContainer").slideDown(300);
@@ -360,17 +306,6 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * MONITORAMENTO DE PROGRESSO - POLLING
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Inicia o polling de progresso
-             * @@description Consulta o servidor a cada 500ms
-            * @@returns { void}
-            */
         function iniciarMonitoramento() {
             try {
 
@@ -386,11 +321,6 @@
             }
         }
 
-            /**
-             * Consulta o progresso atual via API
-             * @@description Atualiza interface e verifica conclusão
-            * @@returns { void}
-            */
         function consultarProgresso() {
             try {
                 $.ajax({
@@ -422,37 +352,32 @@
             }
         }
 
-            /**
-             * Atualiza a interface com o progresso atual
-             * @@param { Object } progresso - Dados de progresso do servidor
-            * @@returns { void}
-            */
-            function atualizarProgresso(progresso) {
-                try {
-
-                    if (inicioProcessamento === null && progresso.processado > 0) {
-                        inicioProcessamento = new Date();
-                        ultimoProcessado = 0;
-                    }
-
-                    $("#progressFill").css("width", progresso.percentual + "%");
-                    $("#progressText").text(progresso.percentual + "%");
-
-                    $("#progressLabel").text(progresso.mensagem);
-                    $("#progressPercent").text(progresso.percentual + "%");
-                    $("#progressMessage").text(progresso.mensagem);
-
-                    animarNumero("#statTotal", progresso.total);
-                    animarNumero("#statProcessado", progresso.processado);
-                    animarNumero("#statRestante", Math.max(0, progresso.total - progresso.processado));
-
-                    calcularTempoRestante(progresso);
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("GerarEstatisticasViagens.cshtml", "atualizarProgresso", error);
-                }
-            }
-
-            function calcularTempoRestante(progresso) {
+        function atualizarProgresso(progresso) {
+            try {
+
+                if (inicioProcessamento === null && progresso.processado > 0) {
+                    inicioProcessamento = new Date();
+                    ultimoProcessado = 0;
+                }
+
+                $("#progressFill").css("width", progresso.percentual + "%");
+                $("#progressText").text(progresso.percentual + "%");
+
+                $("#progressLabel").text(progresso.mensagem);
+                $("#progressPercent").text(progresso.percentual + "%");
+                $("#progressMessage").text(progresso.mensagem);
+
+                animarNumero("#statTotal", progresso.total);
+                animarNumero("#statProcessado", progresso.processado);
+                animarNumero("#statRestante", Math.max(0, progresso.total - progresso.processado));
+
+                calcularTempoRestante(progresso);
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("GerarEstatisticasViagens.cshtml", "atualizarProgresso", error);
+            }
+        }
+
+        function calcularTempoRestante(progresso) {
             try {
                 if (progresso.processado === 0 || progresso.total === 0 || inicioProcessamento === null) {
                     $("#statTempo").text("--");
```
