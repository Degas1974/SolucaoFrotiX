# Pages/Administracao/CalculaCustoViagensTotal.cshtml

**Mudanca:** GRANDE | **+23** linhas | **-140** linhas

---

```diff
--- JANEIRO: Pages/Administracao/CalculaCustoViagensTotal.cshtml
+++ ATUAL: Pages/Administracao/CalculaCustoViagensTotal.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Administracao.CalculaCustoViagensTotalModel
 @addTagHelper*, Microsoft.AspNetCore.Mvc.TagHelpers
 
@@ -30,7 +29,7 @@
         padding: 25px;
         background: white;
         border-radius: 8px;
-        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
+        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
     }
 
     .progress-info {
@@ -60,7 +59,7 @@
         border-radius: 8px;
         overflow: hidden;
         position: relative;
-        box-shadow: inset 0 2px 4px rgba(0, 0, 0, 0.1);
+        box-shadow: inset 0 2px 4px rgba(0,0,0,0.1);
     }
 
     .custom-progress-fill {
@@ -83,7 +82,7 @@
         font-weight: bold;
         font-size: 16px;
         line-height: 45px;
-        text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
+        text-shadow: 0 1px 2px rgba(0,0,0,0.3);
         z-index: 10;
     }
 
@@ -105,7 +104,7 @@
 
     .stat-card:hover {
         transform: translateY(-2px);
-        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
+        box-shadow: 0 4px 8px rgba(0,0,0,0.1);
     }
 
     .stat-label {
@@ -147,16 +146,14 @@
         top: 50%;
         left: 20px;
         margin-top: -8px;
-        border: 2px solid rgba(255, 255, 255, 0.3);
+        border: 2px solid rgba(255,255,255,0.3);
         border-radius: 50%;
         border-top-color: white;
         animation: spinner 0.8s linear infinite;
     }
 
     @@keyframes spinner {
-        to {
-            transform: rotate(360deg);
-        }
+        to { transform: rotate(360deg); }
     }
 
     .success-icon {
@@ -168,19 +165,9 @@
     }
 
     @@keyframes successPulse {
-        0% {
-            transform: scale(0.3);
-            opacity: 0;
-        }
-
-        50% {
-            transform: scale(1.1);
-        }
-
-        100% {
-            transform: scale(1);
-            opacity: 1;
-        }
+        0% { transform: scale(0.3); opacity: 0; }
+        50% { transform: scale(1.1); }
+        100% { transform: scale(1); opacity: 1; }
     }
 
     .info-box {
@@ -214,8 +201,7 @@
                         <div class="col-12">
                             <p class="text-muted mb-3">
                                 <i class="fa-duotone fa-info-circle"></i>
-                                Esta operação irá recalcular os custos de todas as viagens realizadas no sistema usando
-                                processamento em batch otimizado.
+                                Esta operação irá recalcular os custos de todas as viagens realizadas no sistema usando processamento em batch otimizado.
                             </p>
                         </div>
                     </div>
@@ -225,10 +211,8 @@
                             <div class="info-box">
                                 <i class="fa-duotone fa-lightbulb"></i>
                                 <strong>Otimização Implementada:</strong>
-                                Este processo carrega todos os dados necessários uma única vez em memória e processa as
-                                viagens em lotes de 500 registros.
-                                Tempo estimado: <strong>5-15 minutos</strong> para 53.000 viagens (vs. 111 horas do
-                                método anterior).
+                                Este processo carrega todos os dados necessários uma única vez em memória e processa as viagens em lotes de 500 registros.
+                                Tempo estimado: <strong>5-15 minutos</strong> para 53.000 viagens (vs. 111 horas do método anterior).
                             </div>
                         </div>
                     </div>
@@ -287,25 +271,10 @@
     <script src="~/js/Toasts/AppToast.js" asp-append-version="true"></script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CÁLCULO DE CUSTOS EM BATCH - ADMINISTRAÇÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Sistema de processamento batch para recálculo de custos de viagens
-            * Inclui barra de progresso em tempo real, polling de status e contador de tempo
-                */
-
-            /** @@type { Date | null } Timestamp de início do processamento */
-            let inicioProcessamento = null;
-            /** @@type { number | null } ID do intervalo do contador de tempo */
+        let inicioProcessamento = null;
         let intervalTimerId = null;
-            /** @@type { number | null } ID do intervalo de polling do progresso */
-            let intervalProgressoId = null;
-
-            /**
-             * Inicialização da página
-             * @@description Configura handler do botão de cálculo de custos
-            */
+        let intervalProgressoId = null;
+
         $(document).ready(function () {
             try {
                 $("#btnCalculaCustoViagens").on("click", function () {
@@ -316,21 +285,8 @@
             }
         });
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES PRINCIPAIS - PROCESSAMENTO BATCH
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Inicia o cálculo de custos em batch
-             * @@description Prepara interface, inicia contadores e dispara requisição AJAX
-            * Timeout configurado para 30 minutos devido ao volume de dados
-                * @@returns { void}
-                */
         function iniciarCalculoCustos() {
             try {
-
                 marcarBotaoProcessando();
                 mostrarBarraProgresso();
 
@@ -352,7 +308,6 @@
                     timeout: 1800000,
                     success: function (data) {
                         try {
-
                             pararMonitoramentoProgresso();
                             pararContagemTempo();
 
@@ -375,7 +330,6 @@
                     },
                     error: function (jqXHR, textStatus, errorThrown) {
                         try {
-
                             pararMonitoramentoProgresso();
                             pararContagemTempo();
                             desmarcarBotaoProcessando();
@@ -397,17 +351,6 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * POLLING DE PROGRESSO - MONITORAMENTO EM TEMPO REAL
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Inicia o polling de progresso
-             * @@description Consulta o servidor a cada 500ms para atualizar a barra de progresso
-            * @@returns { void}
-            */
         function iniciarMonitoramentoProgresso() {
             try {
 
@@ -423,11 +366,6 @@
             }
         }
 
-            /**
-             * Para o polling de progresso
-             * @@description Limpa o intervalo de consulta
-            * @@returns { void}
-            */
         function pararMonitoramentoProgresso() {
             try {
                 if (intervalProgressoId !== null) {
@@ -439,11 +377,6 @@
             }
         }
 
-            /**
-             * Consulta o progresso atual do processamento
-             * @@description Faz requisição GET ao endpoint de status e atualiza interface
-            * @@returns { void}
-            */
         function consultarProgresso() {
             try {
                 $.ajax({
@@ -480,21 +413,6 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES DE INTERFACE - ATUALIZAÇÃO VISUAL
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Atualiza a interface com o progresso atual
-             * @@description Atualiza barra de progresso, labels e estatísticas
-            * @@param { number } processado - Quantidade de registros processados
-                * @@param { number } total - Total de registros a processar
-                    * @@param { string } mensagem - Mensagem de status
-                        * @@param { number } percentual - Percentual de conclusão(0 - 100)
-                            * @@returns { void}
-                            */
         function atualizarInterface(processado, total, mensagem, percentual) {
             try {
 
@@ -519,12 +437,6 @@
             }
         }
 
-            /**
-             * Atualiza a interface para estado de erro
-             * @@description Exibe mensagem de erro e reseta barra de progresso
-            * @@param { string } mensagem - Mensagem de erro a exibir
-                * @@returns { void}
-                */
         function atualizarErro(mensagem) {
             try {
                 $("#progressFill").css("width", "0%");
@@ -537,17 +449,6 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES DE CONTROLE DO BOTÃO E BARRA DE PROGRESSO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Marca o botão como processando
-             * @@description Desabilita botão e mostra spinner de loading
-            * @@returns { void}
-            */
         function marcarBotaoProcessando() {
             try {
                 $("#btnCalculaCustoViagens")
@@ -559,11 +460,6 @@
             }
         }
 
-            /**
-             * Restaura o botão ao estado inicial
-             * @@description Habilita botão e restaura texto original
-            * @@returns { void}
-            */
         function desmarcarBotaoProcessando() {
             try {
                 $("#btnCalculaCustoViagens")
@@ -575,11 +471,6 @@
             }
         }
 
-            /**
-             * Exibe a barra de progresso com animação
-             * @@description Faz slideDown do container e oculta ícone de sucesso
-            * @@returns { void}
-            */
         function mostrarBarraProgresso() {
             try {
                 $("#progressContainer").slideDown(300);
@@ -589,17 +480,6 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CONTADOR DE TEMPO DECORRIDO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Inicia a contagem de tempo decorrido
-             * @@description Atualiza o tempo a cada segundo no formato "Xm Xs"
-            * @@returns { void}
-            */
         function iniciarContagemTempo() {
             try {
 
@@ -625,26 +505,17 @@
             }
         }
 
-            /**
-             * Para a contagem de tempo decorrido
-             * @@description Limpa o intervalo do timer
-            * @@returns { void}
-            */
-            function pararContagemTempo() {
-                try {
-                    if (intervalTimerId) {
-                        clearInterval(intervalTimerId);
-                        intervalTimerId = null;
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("CalculaCustoViagensTotal.cshtml", "pararContagemTempo", error);
+        function pararContagemTempo() {
+            try {
+                if (intervalTimerId) {
+                    clearInterval(intervalTimerId);
+                    intervalTimerId = null;
                 }
-            }
-
-            /**
-             * Cleanup ao fechar a página
-             * @@description Limpa intervalos ativos para evitar memory leaks
-            */
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("CalculaCustoViagensTotal.cshtml", "pararContagemTempo", error);
+            }
+        }
+
         window.addEventListener('beforeunload', function () {
             try {
                 pararContagemTempo();
```
