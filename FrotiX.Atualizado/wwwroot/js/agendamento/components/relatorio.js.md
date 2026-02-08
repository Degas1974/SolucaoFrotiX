# wwwroot/js/agendamento/components/relatorio.js

**Mudanca:** GRANDE | **+519** linhas | **-529** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/components/relatorio.js
+++ ATUAL: wwwroot/js/agendamento/components/relatorio.js
@@ -1,7 +1,9 @@
-(function () {
+(function ()
+{
     'use strict';
 
-    window.mostrarLoadingRelatorio = function () {
+    window.mostrarLoadingRelatorio = function ()
+    {
         console.log('[Relatório] ⏳ Mostrando overlay...');
 
         $('#modal-relatorio-loading-overlay').remove();
@@ -19,7 +21,8 @@
 
         $('body').append(html);
 
-        $('#modal-relatorio-loading-overlay').on('click keydown', function (e) {
+        $('#modal-relatorio-loading-overlay').on('click keydown', function (e)
+        {
             e.preventDefault();
             e.stopImmediatePropagation();
             return false;
@@ -28,18 +31,19 @@
         console.log('[Relatório] ✅ Overlay visível');
     };
 
-    window.esconderLoadingRelatorio = function () {
-        console.log(
-            '[Relatório] ✅ Aguardando 2 segundos antes de remover overlay...',
-        );
-
-        setTimeout(function () {
-            $('#modal-relatorio-loading-overlay').fadeOut(300, function () {
+    window.esconderLoadingRelatorio = function ()
+    {
+        console.log('[Relatório] ✅ Aguardando 1 segundo antes de remover overlay...');
+
+        setTimeout(function ()
+        {
+            $('#modal-relatorio-loading-overlay').fadeOut(300, function ()
+            {
                 $(this).remove();
             });
 
             console.log('[Relatório] ✅ Overlay removido');
-        }, 2000);
+        }, 1000);
     };
 
     const CONFIG = {
@@ -53,7 +57,7 @@
         SHOW_DELAY: 500,
 
         VIEWER_HEIGHT: '800px',
-        CONTAINER_MIN_HEIGHT: '850px',
+        CONTAINER_MIN_HEIGHT: '850px'
     };
 
     let reportViewerInstance = null;
@@ -64,121 +68,136 @@
     window.reportViewerInitPromise = null;
     window.reportViewerDestroyPromise = null;
 
-    async function waitUntil(condition, timeout = 15000, interval = 100) {
+    async function waitUntil(condition, timeout = 15000, interval = 100)
+    {
         const startTime = Date.now();
 
-        while (!condition()) {
-            if (Date.now() - startTime > timeout) {
+        while (!condition())
+        {
+            if (Date.now() - startTime > timeout)
+            {
                 console.warn('⚠️ [Relatório] Timeout ao aguardar condição');
                 return false;
             }
 
-            await new Promise((resolve) => setTimeout(resolve, interval));
+            await new Promise(resolve => setTimeout(resolve, interval));
         }
 
         return true;
     }
 
-    function validarDependencias() {
+    function validarDependencias()
+    {
         const deps = {
             jQuery: typeof $ !== 'undefined',
             jQueryFn: typeof $.fn !== 'undefined',
             Telerik: typeof $.fn.telerik_ReportViewer === 'function',
             TelerikViewer: typeof telerikReportViewer !== 'undefined',
-            Kendo: typeof kendo !== 'undefined',
+            Kendo: typeof kendo !== 'undefined'
         };
 
-        const todasCarregadas = Object.values(deps).every((v) => v === true);
-
-        if (!todasCarregadas) {
-            console.error(
-                '❌ Dependências faltando:',
+        const todasCarregadas = Object.values(deps).every(v => v === true);
+
+        if (!todasCarregadas)
+        {
+            console.error("❌ Dependências faltando:",
                 Object.entries(deps)
                     .filter(([_, loaded]) => !loaded)
-                    .map(([name]) => name),
+                    .map(([name]) => name)
             );
         }
 
         return {
             valido: todasCarregadas,
-            dependencias: deps,
+            dependencias: deps
         };
     }
 
-    function validarViagemId(viagemId) {
-        if (
-            !viagemId ||
-            viagemId === '' ||
-            viagemId === '00000000-0000-0000-0000-000000000000'
-        ) {
-            console.warn('⚠️ ViagemId inválido:', viagemId);
+    function validarViagemId(viagemId)
+    {
+        if (!viagemId ||
+            viagemId === "" ||
+            viagemId === "00000000-0000-0000-0000-000000000000")
+        {
+            console.warn("⚠️ ViagemId inválido:", viagemId);
             return false;
         }
         return true;
     }
 
-    function obterCard() {
+    function obterCard()
+    {
         const card = document.getElementById(CONFIG.CARD_ID);
 
-        if (!card) {
+        if (!card)
+        {
             console.error(`❌ #${CONFIG.CARD_ID} não encontrado no DOM`);
         }
 
         return card;
     }
 
-    function obterContainer() {
+    function obterContainer()
+    {
         const container = document.getElementById(CONFIG.CONTAINER_ID);
 
-        if (!container) {
+        if (!container)
+        {
             console.error(`❌ #${CONFIG.CONTAINER_ID} não encontrado no DOM`);
         }
 
         return container;
     }
 
-    function obterViewer() {
+    function obterViewer()
+    {
         const viewer = document.getElementById(CONFIG.VIEWER_ID);
 
-        if (!viewer) {
+        if (!viewer)
+        {
             console.error(`❌ #${CONFIG.VIEWER_ID} não encontrado no DOM`);
         }
 
         return viewer;
     }
 
-    function limparInstanciaAnterior() {
-        try {
+    function limparInstanciaAnterior()
+    {
+        try
+        {
             const $viewer = $(`#${CONFIG.VIEWER_ID}`);
 
-            const viewer = $viewer.data('telerik_ReportViewer');
-
-            if (viewer) {
-                console.log('🗑️ Destruindo viewer anterior...');
-
-                if (typeof viewer.dispose === 'function') {
+            const viewer = $viewer.data("telerik_ReportViewer");
+
+            if (viewer)
+            {
+                console.log("🗑️ Destruindo viewer anterior...");
+
+                if (typeof viewer.dispose === 'function')
+                {
                     viewer.dispose();
-                } else if (typeof viewer.destroy === 'function') {
+                } else if (typeof viewer.destroy === 'function')
+                {
                     viewer.destroy();
                 }
 
                 reportViewerInstance = null;
             }
 
-            $viewer.removeData('telerik_ReportViewer');
+            $viewer.removeData("telerik_ReportViewer");
 
             $viewer.empty();
 
-            console.log('✅ Instância anterior limpa');
-        } catch (error) {
-            console.warn(
-                '⚠️ Erro ao limpar instância anterior:',
-                error.message,
-            );
-        }
-    }
-
-    function mostrarLoading(mensagem = 'Carregando relatório...') {
+            console.log("✅ Instância anterior limpa");
+
+        } catch (error)
+        {
+            console.warn("⚠️ Erro ao limpar instância anterior:", error.message);
+        }
+    }
+
+    function mostrarLoading(mensagem = 'Carregando relatório...')
+    {
         const viewer = obterViewer();
 
         if (!viewer) return;
@@ -193,7 +212,8 @@
         `;
     }
 
-    function mostrarErro(mensagem) {
+    function mostrarErro(mensagem)
+    {
         const viewer = obterViewer();
 
         if (!viewer) return;
@@ -206,144 +226,153 @@
         `;
     }
 
-    function aplicarAlturasFixas() {
-        console.log('📏 Aplicando alturas fixas aos containers...');
+    function aplicarAlturasFixas()
+    {
+        console.log("📏 Aplicando alturas fixas aos containers...");
 
         const $viewer = $(`#${CONFIG.VIEWER_ID}`);
         const $container = $(`#${CONFIG.CONTAINER_ID}`);
 
         $viewer.css({
-            height: CONFIG.VIEWER_HEIGHT,
+            'height': CONFIG.VIEWER_HEIGHT,
             'min-height': CONFIG.VIEWER_HEIGHT,
             'max-height': 'none',
-            width: '100%',
-            display: 'block',
-            visibility: 'visible',
-            opacity: '1',
-            position: 'relative',
+            'width': '100%',
+            'display': 'block',
+            'visibility': 'visible',
+            'opacity': '1',
+            'position': 'relative'
         });
 
         $container.css({
-            height: 'auto',
+            'height': 'auto',
             'min-height': CONFIG.CONTAINER_MIN_HEIGHT,
-            display: 'block',
-            visibility: 'visible',
-            opacity: '1',
+            'display': 'block',
+            'visibility': 'visible',
+            'opacity': '1'
         });
 
-        console.log('✅ Alturas aplicadas:', {
+        console.log("✅ Alturas aplicadas:", {
             viewer: CONFIG.VIEWER_HEIGHT,
-            containerMin: CONFIG.CONTAINER_MIN_HEIGHT,
+            containerMin: CONFIG.CONTAINER_MIN_HEIGHT
         });
     }
 
-    function mostrarRelatorio() {
-        try {
-            console.log('👁️ Mostrando relatório...');
+    function mostrarRelatorio()
+    {
+        try
+        {
+            console.log("👁️ Mostrando relatório...");
 
             const $card = $(`#${CONFIG.CARD_ID}`);
             const $container = $(`#${CONFIG.CONTAINER_ID}`);
             const $viewer = $(`#${CONFIG.VIEWER_ID}`);
 
-            if ($card.length === 0) {
-                console.error('❌ Card não encontrado');
+            if ($card.length === 0)
+            {
+                console.error("❌ Card não encontrado");
                 return;
             }
 
             aplicarAlturasFixas();
 
-            console.log('📺 Mostrando #cardRelatorio');
+            console.log("📺 Mostrando #cardRelatorio");
             $card.show().css({
-                display: 'block',
-                visibility: 'visible',
-                opacity: '1',
+                'display': 'block',
+                'visibility': 'visible',
+                'opacity': '1'
             });
 
-            if ($container.length > 0) {
-                console.log('📺 Mostrando #ReportContainerAgenda');
+            if ($container.length > 0)
+            {
+                console.log("📺 Mostrando #ReportContainerAgenda");
                 $container.show().css({
-                    display: 'block',
-                    visibility: 'visible',
-                    opacity: '1',
+                    'display': 'block',
+                    'visibility': 'visible',
+                    'opacity': '1'
                 });
             }
 
-            console.log('📺 Mostrando #reportViewerAgenda');
+            console.log("📺 Mostrando #reportViewerAgenda");
             $viewer.show().css({
-                display: 'block',
-                visibility: 'visible',
-                opacity: '1',
+                'display': 'block',
+                'visibility': 'visible',
+                'opacity': '1'
             });
 
             const viewerInstance = $viewer.data('telerik_ReportViewer');
-            if (viewerInstance) {
-                console.log('🔄 Forçando refresh do viewer');
-                try {
-                    if (typeof viewerInstance.refreshReport === 'function') {
+            if (viewerInstance)
+            {
+                console.log("🔄 Forçando refresh do viewer");
+                try
+                {
+                    if (typeof viewerInstance.refreshReport === 'function')
+                    {
                         viewerInstance.refreshReport();
                     }
-                } catch (e) {
-                    console.warn('⚠️ Erro ao fazer refresh:', e);
-                }
-            }
-
-            setTimeout(() => {
+                } catch (e)
+                {
+                    console.warn("⚠️ Erro ao fazer refresh:", e);
+                }
+            }
+
+            setTimeout(() =>
+            {
                 const cardElement = $card[0];
-                if (cardElement) {
-                    console.log('📜 Fazendo scroll até o relatório');
+                if (cardElement)
+                {
+                    console.log("📜 Fazendo scroll até o relatório");
                     cardElement.scrollIntoView({
                         behavior: 'smooth',
-                        block: 'start',
+                        block: 'start'
                     });
                 }
             }, 300);
 
-            console.log('✅ Relatório exibido');
-
-            setTimeout(() => {
-                if (
-                    typeof window.diagnosticarVisibilidadeRelatorio ===
-                    'function'
-                ) {
+            console.log("✅ Relatório exibido");
+
+            setTimeout(() =>
+            {
+                if (typeof window.diagnosticarVisibilidadeRelatorio === 'function')
+                {
                     window.diagnosticarVisibilidadeRelatorio();
                 }
             }, 500);
-        } catch (error) {
-            console.error('❌ Erro ao mostrar relatório:', error);
-
-            if (
-                typeof Alerta !== 'undefined' &&
-                Alerta.TratamentoErroComLinha
-            ) {
-                Alerta.TratamentoErroComLinha(
-                    'relatorio.js',
-                    'mostrarRelatorio',
-                    error,
-                );
-            }
-        }
-    }
-
-    function esconderRelatorio() {
-        console.log('🙈 Escondendo relatório...');
+
+        } catch (error)
+        {
+            console.error("❌ Erro ao mostrar relatório:", error);
+
+            if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
+            {
+                Alerta.TratamentoErroComLinha("relatorio.js", "mostrarRelatorio", error);
+            }
+        }
+    }
+
+    function esconderRelatorio()
+    {
+        console.log("🙈 Escondendo relatório...");
 
         const card = obterCard();
         const container = obterContainer();
 
         if (!card || !container) return;
 
-        $(card).slideUp(300, function () {
-            card.style.display = 'none';
+        $(card).slideUp(300, function ()
+        {
+            card.style.display = "none";
         });
 
-        container.style.display = 'none';
-        container.classList.remove('visible');
+        container.style.display = "none";
+        container.classList.remove("visible");
 
         limparInstanciaAnterior();
 
         const viewer = obterViewer();
 
-        if (viewer) {
+        if (viewer)
+        {
             viewer.innerHTML = `
                 <div class="text-center p-5">
                     <div class="spinner-border text-primary" role="status">
@@ -354,69 +383,76 @@
             `;
         }
 
-        console.log('✅ Relatório escondido e resetado');
-    }
-
-    function determinarRelatorio(data) {
-        if (!data) {
-            console.warn('⚠️ Dados vazios, usando relatório padrão');
-            return 'FichaAberta.trdp';
+        console.log("✅ Relatório escondido e resetado");
+    }
+
+    function determinarRelatorio(data)
+    {
+        if (!data)
+        {
+            console.warn("⚠️ Dados vazios, usando relatório padrão");
+            return "FichaAberta.trdp";
         }
 
         const status = data.status || data.Status;
         const finalidade = data.finalidade || data.Finalidade;
-        const statusAgendamento =
-            data.statusAgendamento ?? data.StatusAgendamento;
-
-        let relatorioAsString = 'FichaAberta.trdp';
-
-        if (status === 'Cancelada' || status === 'Cancelado') {
-            relatorioAsString =
-                finalidade !== 'Evento'
-                    ? 'FichaCancelada.trdp'
-                    : 'FichaEventoCancelado.trdp';
-        } else if (finalidade === 'Evento' && status !== 'Cancelada') {
-            relatorioAsString = 'FichaEvento.trdp';
-        } else if (status === 'Aberta' && finalidade !== 'Evento') {
-            relatorioAsString = 'FichaAberta.trdp';
-        } else if (status === 'Realizada') {
-            relatorioAsString =
-                finalidade !== 'Evento'
-                    ? 'FichaRealizada.trdp'
-                    : 'FichaEventoRealizado.trdp';
-        } else if (statusAgendamento === true) {
-            relatorioAsString =
-                finalidade !== 'Evento'
-                    ? 'FichaAgendamento.trdp'
-                    : 'FichaEventoAgendado.trdp';
-        }
-
-        console.log('📄 Relatório selecionado:', relatorioAsString);
-        console.log(' - Status:', status);
-        console.log(' - Finalidade:', finalidade);
-        console.log(' - StatusAgendamento:', statusAgendamento);
-        console.log(
-            ' - Dados originais:',
-            JSON.stringify(data).substring(0, 500),
-        );
+        const statusAgendamento = data.statusAgendamento ?? data.StatusAgendamento;
+
+        let relatorioAsString = "FichaAberta.trdp";
+
+        if (status === "Cancelada" || status === "Cancelado")
+        {
+            relatorioAsString = finalidade !== "Evento"
+                ? "FichaCancelada.trdp"
+                : "FichaEventoCancelado.trdp";
+        }
+        else if (finalidade === "Evento" && status !== "Cancelada")
+        {
+            relatorioAsString = "FichaEvento.trdp";
+        }
+        else if (status === "Aberta" && finalidade !== "Evento")
+        {
+            relatorioAsString = "FichaAberta.trdp";
+        }
+        else if (status === "Realizada")
+        {
+            relatorioAsString = finalidade !== "Evento"
+                ? "FichaRealizada.trdp"
+                : "FichaEventoRealizado.trdp";
+        }
+        else if (statusAgendamento === true)
+        {
+            relatorioAsString = finalidade !== "Evento"
+                ? "FichaAgendamento.trdp"
+                : "FichaEventoAgendado.trdp";
+        }
+
+        console.log("📄 Relatório selecionado:", relatorioAsString);
+        console.log(" - Status:", status);
+        console.log(" - Finalidade:", finalidade);
+        console.log(" - StatusAgendamento:", statusAgendamento);
+        console.log(" - Dados originais:", JSON.stringify(data).substring(0, 500));
 
         return relatorioAsString;
     }
 
-    function inicializarViewer(viagemId, relatorioNome) {
+    function inicializarViewer(viagemId, relatorioNome)
+    {
         const $viewer = $(`#${CONFIG.VIEWER_ID}`);
 
-        console.log('🎨 Inicializando Telerik ReportViewer...');
-        console.log(' - ViagemId:', viagemId);
-        console.log(' - Relatório:', relatorioNome);
-
-        try {
+        console.log("🎨 Inicializando Telerik ReportViewer...");
+        console.log(" - ViagemId:", viagemId);
+        console.log(" - Relatório:", relatorioNome);
+
+        try
+        {
 
             $viewer.empty();
 
             aplicarAlturasFixas();
 
-            if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress) {
+            if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress)
+            {
                 kendo.ui.progress($viewer, true);
             }
 
@@ -425,70 +461,67 @@
                 reportSource: {
                     report: relatorioNome,
                     parameters: {
-                        ViagemId: viagemId.toString().toUpperCase(),
-                    },
+                        ViagemId: viagemId.toString().toUpperCase()
+                    }
                 },
                 viewMode: telerikReportViewer.ViewModes.PRINT_PREVIEW,
                 scaleMode: telerikReportViewer.ScaleModes.SPECIFIC,
                 scale: 1.0,
                 enableAccessibility: false,
                 sendEmail: {
-                    enabled: true,
+                    enabled: true
                 },
 
-                ready: function () {
-                    console.log('✅ Telerik ReportViewer PRONTO!');
-                    console.log('📄 Relatório renderizado com sucesso');
-
-                    if (
-                        typeof kendo !== 'undefined' &&
-                        kendo.ui &&
-                        kendo.ui.progress
-                    ) {
+                ready: function ()
+                {
+                    console.log("✅ Telerik ReportViewer PRONTO!");
+                    console.log("📄 Relatório renderizado com sucesso");
+
+                    if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress)
+                    {
                         kendo.ui.progress($viewer, false);
                     }
                 },
 
-                renderingBegin: function () {
-                    console.log('🎨 Iniciando renderização do relatório...');
+                renderingBegin: function ()
+                {
+                    console.log("🎨 Iniciando renderização do relatório...");
                 },
 
-                renderingEnd: function () {
-                    console.log('🎨 Renderização concluída!');
+                renderingEnd: function ()
+                {
+                    console.log("🎨 Renderização concluída!");
                 },
 
-                error: function (e, args) {
-                    console.error('❌ Erro no Telerik ReportViewer:', args);
-
-                    if (
-                        typeof kendo !== 'undefined' &&
-                        kendo.ui &&
-                        kendo.ui.progress
-                    ) {
+                error: function (e, args)
+                {
+                    console.error("❌ Erro no Telerik ReportViewer:", args);
+
+                    if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress)
+                    {
                         kendo.ui.progress($viewer, false);
                     }
 
-                    const mensagem =
-                        args.message || 'Falha ao renderizar o relatório';
+                    const mensagem = args.message || "Falha ao renderizar o relatório";
                     mostrarErro(mensagem);
 
-                    if (typeof AppToast !== 'undefined') {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao renderizar relatório',
-                            mensagem,
-                        );
-                    }
-                },
+                    if (typeof AppToast !== 'undefined')
+                    {
+                        AppToast.show("Vermelho", "Erro ao renderizar relatório", mensagem);
+                    }
+                }
             });
 
-            reportViewerInstance = $viewer.data('telerik_ReportViewer');
-
-            console.log('✅ Viewer inicializado');
-        } catch (error) {
-            console.error('❌ Erro ao inicializar viewer:', error);
-
-            if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress) {
+            reportViewerInstance = $viewer.data("telerik_ReportViewer");
+
+            console.log("✅ Viewer inicializado");
+
+        } catch (error)
+        {
+            console.error("❌ Erro ao inicializar viewer:", error);
+
+            if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress)
+            {
                 kendo.ui.progress($viewer, false);
             }
 
@@ -497,81 +530,79 @@
         }
     }
 
-    function buscarDadosViagem(viagemId) {
-        console.log('🌐 Fazendo requisição para RecuperaViagem...');
-
-        return new Promise((resolve, reject) => {
+    function buscarDadosViagem(viagemId)
+    {
+        console.log("🌐 Fazendo requisição para RecuperaViagem...");
+
+        return new Promise((resolve, reject) =>
+        {
             $.ajax({
-                type: 'GET',
+                type: "GET",
                 url: CONFIG.RECOVERY_URL,
                 data: { id: viagemId },
-                contentType: 'application/json',
-                dataType: 'json',
+                contentType: "application/json",
+                dataType: "json",
                 timeout: CONFIG.TIMEOUT,
 
-                success: function (response) {
-                    console.log('📥 Resposta recebida da API:', response);
-
-                    if (!response || !response.data) {
-                        reject(
-                            new Error('Resposta vazia ou inválida do servidor'),
-                        );
+                success: function (response)
+                {
+                    console.log("📥 Resposta recebida da API:", response);
+
+                    if (!response || !response.data)
+                    {
+                        reject(new Error("Resposta vazia ou inválida do servidor"));
                         return;
                     }
 
                     resolve(response.data);
                 },
 
-                error: function (jqXHR, textStatus, errorThrown) {
-                    console.error('❌ Erro na requisição AJAX:', {
+                error: function (jqXHR, textStatus, errorThrown)
+                {
+                    console.error("❌ Erro na requisição AJAX:", {
                         status: jqXHR.status,
                         statusText: jqXHR.statusText,
                         textStatus: textStatus,
-                        error: errorThrown,
+                        error: errorThrown
                     });
 
-                    let mensagem = 'Falha na comunicação com o servidor';
-
-                    if (typeof window.criarErroAjax === 'function') {
-                        const erro = window.criarErroAjax(
-                            jqXHR,
-                            textStatus,
-                            errorThrown,
-                            this,
-                        );
+                    let mensagem = "Falha na comunicação com o servidor";
+
+                    if (typeof window.criarErroAjax === 'function')
+                    {
+                        const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
                         mensagem = erro.mensagemUsuario || mensagem;
-                    } else if (
-                        jqXHR.responseJSON &&
-                        jqXHR.responseJSON.message
-                    ) {
+                    } else if (jqXHR.responseJSON && jqXHR.responseJSON.message)
+                    {
                         mensagem = jqXHR.responseJSON.message;
                     }
 
                     reject(new Error(mensagem));
-                },
+                }
             });
         });
     }
 
-    window.carregarRelatorioViagem = async function (viagemId) {
+    window.carregarRelatorioViagem = async function (viagemId)
+    {
         console.log('[Relatório] ===== INICIANDO CARREGAMENTO =====');
         console.log('[Relatório] ViagemId:', viagemId);
 
         window.mostrarLoadingRelatorio();
 
-        try {
-
-            if (
-                !viagemId ||
-                viagemId === '00000000-0000-0000-0000-000000000000'
-            ) {
+        try
+        {
+
+            if (!viagemId || viagemId === '00000000-0000-0000-0000-000000000000')
+            {
                 console.error('[Relatório] ViagemId inválido:', viagemId);
                 window.esconderLoadingRelatorio();
                 window.limparRelatorio();
                 return;
             }
 
-            if (typeof $ === 'undefined' || !$.fn.telerik_ReportViewer) {
+            if (typeof $ === 'undefined' || !$.fn.telerik_ReportViewer)
+            {
                 console.error('[Relatório] ❌ Telerik não disponível');
 
                 $('#reportViewerAgenda').html(`
@@ -585,23 +616,22 @@
             }
 
             const modalAberto = $('#modalViagens').hasClass('show');
-            if (!modalAberto) {
-                console.warn(
-                    '[Relatório] ⚠️ Modal foi fechado, cancelando carregamento',
-                );
+            if (!modalAberto)
+            {
+                console.warn('[Relatório] ⚠️ Modal foi fechado, cancelando carregamento');
                 window.esconderLoadingRelatorio();
                 return;
             }
 
-            if (window.isReportViewerDestroying) {
+            if (window.isReportViewerDestroying)
+            {
                 console.log('[Relatório] ⏳ Aguardando limpeza anterior...');
                 await waitUntil(() => !window.isReportViewerDestroying, 3000);
             }
 
-            if (window.isReportViewerLoading) {
-                console.log(
-                    '[Relatório] ⚠️ Já existe carregamento em andamento',
-                );
+            if (window.isReportViewerLoading)
+            {
+                console.log('[Relatório] ⚠️ Já existe carregamento em andamento');
                 window.esconderLoadingRelatorio();
                 return;
             }
@@ -611,10 +641,11 @@
             console.log('[Relatório] 🧹 Limpando viewer anterior...');
             await window.limparRelatorio();
 
-            await new Promise((resolve) => setTimeout(resolve, 500));
+            await new Promise(resolve => setTimeout(resolve, 500));
 
             const modalAindaAberto = $('#modalViagens').hasClass('show');
-            if (!modalAindaAberto) {
+            if (!modalAindaAberto)
+            {
                 console.warn('[Relatório] ⚠️ Modal fechado durante debounce');
                 window.isReportViewerLoading = false;
                 window.esconderLoadingRelatorio();
@@ -622,10 +653,9 @@
             }
 
             const viagemIdAtual = $('#txtViagemIdRelatorio').val();
-            if (viagemIdAtual && viagemIdAtual !== viagemId) {
-                console.warn(
-                    '[Relatório] ⚠️ ViagemId mudou durante carregamento',
-                );
+            if (viagemIdAtual && viagemIdAtual !== viagemId)
+            {
+                console.warn('[Relatório] ⚠️ ViagemId mudou durante carregamento');
                 window.isReportViewerLoading = false;
                 window.esconderLoadingRelatorio();
                 return;
@@ -636,7 +666,8 @@
             await destruirViewerAnterior();
 
             const $container = $('#ReportContainerAgenda');
-            if ($container.length === 0) {
+            if ($container.length === 0)
+            {
                 console.error('[Relatório] Container principal não encontrado');
                 window.isReportViewerLoading = false;
                 window.esconderLoadingRelatorio();
@@ -657,32 +688,31 @@
 
             let tipoRelatorio = 'FichaAgendamento.trdp';
 
-            try {
+            try
+            {
                 const response = await $.ajax({
-                    type: 'GET',
+                    type: "GET",
                     url: '/api/Agenda/RecuperaViagem',
                     data: { id: viagemId },
-                    timeout: 10000,
+                    timeout: 10000
                 });
 
-                if (response && response.data) {
+                if (response && response.data)
+                {
                     tipoRelatorio = determinarRelatorio(response.data);
                     console.log('[Relatório] Tipo determinado:', tipoRelatorio);
                 }
-            } catch (error) {
-                console.warn(
-                    '[Relatório] Usando relatório padrío, erro ao buscar dados:',
-                    error,
-                );
-            }
-
-            await new Promise((resolve) => setTimeout(resolve, 500));
+            } catch (error)
+            {
+                console.warn('[Relatório] Usando relatório padrío, erro ao buscar dados:', error);
+            }
+
+            await new Promise(resolve => setTimeout(resolve, 500));
 
             const $viewer = $('#reportViewerAgenda');
-            if ($viewer.length === 0) {
-                console.error(
-                    '[Relatório] Viewer não foi recriado corretamente',
-                );
+            if ($viewer.length === 0)
+            {
+                console.error('[Relatório] Viewer não foi recriado corretamente');
                 window.isReportViewerLoading = false;
                 window.esconderLoadingRelatorio();
                 return;
@@ -697,118 +727,98 @@
                 reportSource: {
                     report: tipoRelatorio,
                     parameters: {
-                        ViagemId: viagemId.toString().toUpperCase(),
-                    },
+                        ViagemId: viagemId.toString().toUpperCase()
+                    }
                 },
                 scale: 1.0,
                 viewMode: 'PRINT_PREVIEW',
                 scaleMode: 'SPECIFIC',
 
-                ready: function () {
-                    try {
+                ready: function ()
+                {
+                    try
+                    {
                         const modalAberto = $('#modalViagens').hasClass('show');
-                        if (!modalAberto) {
-                            console.warn(
-                                '[Relatório] ⚠️ Modal fechado durante ready',
-                            );
+                        if (!modalAberto)
+                        {
+                            console.warn('[Relatório] ⚠️ Modal fechado durante ready');
                             window.isReportViewerLoading = false;
                             return;
                         }
                         window.esconderLoadingRelatorio();
                         console.log('[Relatório] ✅ ready - Viewer pronto');
                         window.isReportViewerLoading = false;
-                        window.telerikReportViewer = $viewer.data(
-                            'telerik_ReportViewer',
-                        );
-                        setTimeout(() => {
+                        window.telerikReportViewer = $viewer.data('telerik_ReportViewer');
+                        setTimeout(() =>
+                        {
                             if (!$('#modalViagens').hasClass('show')) return;
-                            if (
-                                window.telerikReportViewer &&
-                                typeof window.telerikReportViewer.scale ===
-                                    'function'
-                            ) {
-                                try {
-                                    window.telerikReportViewer.scale({
-                                        scale: 1.4,
-                                        scaleMode: 'SPECIFIC',
-                                    });
-                                    console.log(
-                                        '[Relatório] Zoom automático aplicado: 140%',
-                                    );
-                                } catch (e) {
-                                    console.warn(
-                                        '[Relatório] Erro ao aplicar zoom:',
-                                        e,
-                                    );
+                            if (window.telerikReportViewer && typeof window.telerikReportViewer.scale === 'function')
+                            {
+                                try
+                                {
+                                    window.telerikReportViewer.scale({ scale: 1.4, scaleMode: 'SPECIFIC' });
+                                    console.log('[Relatório] Zoom automático aplicado: 140%');
+                                } catch (e)
+                                {
+                                    console.warn('[Relatório] Erro ao aplicar zoom:', e);
                                 }
                             }
                         }, 500);
-                        if (
-                            typeof kendo !== 'undefined' &&
-                            kendo.ui &&
-                            kendo.ui.progress
-                        ) {
+                        if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress)
+                        {
                             kendo.ui.progress($viewer, false);
                         }
-                    } catch (error) {
-                        console.error(
-                            '[Relatório] Erro no callback ready:',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        console.error('[Relatório] Erro no callback ready:', error);
                         window.isReportViewerLoading = false;
                     }
                 },
 
-                renderingBegin: function () {
-                    try {
+                renderingBegin: function ()
+                {
+                    try
+                    {
                         console.log('[Relatório] 🎬 renderingBegin');
 
                         const modalAberto = $('#modalViagens').hasClass('show');
-                        if (!modalAberto) {
-                            console.warn(
-                                '[Relatório] ⚠️ Modal fechado durante renderingBegin',
-                            );
+                        if (!modalAberto)
+                        {
+                            console.warn('[Relatório] ⚠️ Modal fechado durante renderingBegin');
                             window.esconderLoadingRelatorio();
                             return;
                         }
-                    } catch (error) {
-                        console.error(
-                            '[Relatório] Erro no callback renderingBegin:',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        console.error('[Relatório] Erro no callback renderingBegin:', error);
                         window.esconderLoadingRelatorio();
                     }
                 },
 
-                renderingEnd: function () {
-                    try {
+                renderingEnd: function ()
+                {
+                    try
+                    {
                         window.esconderLoadingRelatorio();
-                        console.log(
-                            '[Relatório] ✅ renderingEnd - Overlay removido',
-                        );
+                        console.log('[Relatório] ✅ renderingEnd - Overlay removido');
 
                         const modalAberto = $('#modalViagens').hasClass('show');
-                        if (!modalAberto) {
-                            console.warn(
-                                '[Relatório] ⚠️ Modal fechado durante renderingEnd',
-                            );
+                        if (!modalAberto)
+                        {
+                            console.warn('[Relatório] ⚠️ Modal fechado durante renderingEnd');
                             return;
                         }
-                    } catch (error) {
-                        console.error(
-                            '[Relatório] Erro no callback renderingEnd:',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        console.error('[Relatório] Erro no callback renderingEnd:', error);
                         window.esconderLoadingRelatorio();
                     }
                 },
 
-                error: function (e, args) {
+                error: function (e, args)
+                {
                     window.esconderLoadingRelatorio();
-                    console.error(
-                        '[Relatório] ❌ Erro - Overlay removido:',
-                        args,
-                    );
+                    console.error('[Relatório] ❌ Erro - Overlay removido:', args);
                     window.isReportViewerLoading = false;
 
                     $viewer.html(`
@@ -819,45 +829,38 @@
                     </div>
                 `);
 
-                    if (typeof AppToast !== 'undefined') {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao carregar relatório',
-                            3000,
-                        );
-                    }
-                },
+                    if (typeof AppToast !== 'undefined')
+                    {
+                        AppToast.show('Vermelho', 'Erro ao carregar relatório', 3000);
+                    }
+                }
             });
 
             $('#cardRelatorio').slideDown(300);
             $('#ReportContainerAgenda').show();
 
-            setTimeout(() => {
+            setTimeout(() =>
+            {
                 const cardElement = document.getElementById('cardRelatorio');
-                if (cardElement) {
+                if (cardElement)
+                {
                     cardElement.scrollIntoView({
                         behavior: 'smooth',
-                        block: 'start',
+                        block: 'start'
                     });
                 }
             }, 500);
 
             console.log('[Relatório] ✅ Processo concluído com sucesso');
-        } catch (error) {
+
+        } catch (error)
+        {
             console.error('[Relatório] ❌ Erro crítico:', error);
             window.isReportViewerLoading = false;
 
-            window.esconderLoadingRelatorio();
-
-            if (
-                typeof Alerta !== 'undefined' &&
-                Alerta.TratamentoErroComLinha
-            ) {
-                Alerta.TratamentoErroComLinha(
-                    'relatorio.js',
-                    'carregarRelatorioViagem',
-                    error,
-                );
+            if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
+            {
+                Alerta.TratamentoErroComLinha("relatorio.js", "carregarRelatorioViagem", error);
             }
 
             $('#reportViewerAgenda').html(`
@@ -870,16 +873,18 @@
         }
     };
 
-    window.limparRelatorio = async function () {
-        try {
+    window.limparRelatorio = async function ()
+    {
+        try
+        {
             console.log('[Relatório] 🧹 Iniciando limpeza segura...');
 
-            if (window.isReportViewerDestroying) {
-                console.log(
-                    '[Relatório] ⚠️ Limpeza já em andamento, aguardando...',
-                );
-
-                if (window.reportViewerDestroyPromise) {
+            if (window.isReportViewerDestroying)
+            {
+                console.log('[Relatório] ⚠️ Limpeza já em andamento, aguardando...');
+
+                if (window.reportViewerDestroyPromise)
+                {
                     await window.reportViewerDestroyPromise;
                 }
 
@@ -889,68 +894,48 @@
 
             window.isReportViewerDestroying = true;
 
-            if (window.isReportViewerLoading) {
-                console.log(
-                    '[Relatório] ⚠️ Cancelando carregamento pendente...',
-                );
+            if (window.isReportViewerLoading)
+            {
+                console.log('[Relatório] ⚠️ Cancelando carregamento pendente...');
                 window.isReportViewerLoading = false;
 
-                if (loadTimeout) {
+                if (loadTimeout)
+                {
                     clearTimeout(loadTimeout);
                     loadTimeout = null;
                 }
             }
 
-            window.reportViewerDestroyPromise = new Promise(async (resolve) => {
-                try {
+            window.reportViewerDestroyPromise = new Promise(async (resolve) =>
+            {
+                try
+                {
                     const $viewer = $('#reportViewerAgenda');
 
-                    if ($viewer.length > 0) {
+                    if ($viewer.length > 0)
+                    {
                         const instance = $viewer.data('telerik_ReportViewer');
 
-                        if (instance) {
-                            console.log(
-                                '[Relatório] 🗑️ Destruindo instância do viewer...',
-                            );
-
-                            try {
-
-                                const isInitialized =
-                                    instance.reportSource &&
-                                    typeof instance.reportSource === 'function';
-
-                                if (isInitialized) {
-                                    if (
-                                        typeof instance.dispose === 'function'
-                                    ) {
-                                        instance.dispose();
-                                    } else if (
-                                        typeof instance.destroy === 'function'
-                                    ) {
-                                        instance.destroy();
-                                    }
-                                } else {
-                                    console.log(
-                                        '[Relatório] ⚠️ Viewer não inicializado completamente, pulando dispose',
-                                    );
+                        if (instance)
+                        {
+                            console.log('[Relatório] 🗑️ Destruindo instância do viewer...');
+
+                            try
+                            {
+                                if (typeof instance.dispose === 'function')
+                                {
+                                    instance.dispose();
                                 }
-
-                                await new Promise((r) => setTimeout(r, 200));
-                            } catch (e) {
-
-                                if (
-                                    e.message &&
-                                    e.message.includes('collapsible')
-                                ) {
-                                    console.log(
-                                        '[Relatório] ⚠️ Kendo não inicializado, ignorando erro',
-                                    );
-                                } else {
-                                    console.warn(
-                                        '[Relatório] ⚠️ Erro ao destruir viewer:',
-                                        e,
-                                    );
+                                else if (typeof instance.destroy === 'function')
+                                {
+                                    instance.destroy();
                                 }
+
+                                await new Promise(r => setTimeout(r, 200));
+
+                            } catch (e)
+                            {
+                                console.warn('[Relatório] ⚠️ Erro ao destruir viewer:', e);
                             }
                         }
 
@@ -966,12 +951,13 @@
                     $('#txtViagemIdRelatorio').val('');
 
                     console.log('[Relatório] ✅ Limpeza concluída');
-                } catch (error) {
-                    console.error(
-                        '[Relatório] ❌ Erro durante limpeza:',
-                        error,
-                    );
-                } finally {
+
+                } catch (error)
+                {
+                    console.error('[Relatório] ❌ Erro durante limpeza:', error);
+                }
+                finally
+                {
                     window.isReportViewerDestroying = false;
                     window.reportViewerDestroyPromise = null;
                     resolve();
@@ -979,7 +965,9 @@
             });
 
             await window.reportViewerDestroyPromise;
-        } catch (error) {
+
+        } catch (error)
+        {
             console.error('[Relatório] ❌ Erro na limpeza:', error);
 
             window.isReportViewerDestroying = false;
@@ -987,37 +975,40 @@
         }
     };
 
-    function obterEstado() {
+    function obterEstado()
+    {
         return {
             temInstancia: !!reportViewerInstance,
             cardVisivel: obterCard()?.style.display !== 'none',
             containerVisivel: obterContainer()?.style.display !== 'none',
             viewerDisponivel: !!obterViewer(),
-            viagemId: $(`#${CONFIG.HIDDEN_ID}`).val() || window.currentViagemId,
+            viagemId: $(`#${CONFIG.HIDDEN_ID}`).val() || window.currentViagemId
         };
     }
 
-    function diagnosticarVisibilidadeRelatorio() {
-        console.log('🔍 ===== DIAGNÓSTICO DE VISIBILIDADE =====');
+    function diagnosticarVisibilidadeRelatorio()
+    {
+        console.log("🔍 ===== DIAGNÓSTICO DE VISIBILIDADE =====");
 
         const reportContainer = document.getElementById(CONFIG.VIEWER_ID);
-        if (!reportContainer) {
+        if (!reportContainer)
+        {
             console.error(`❌ #${CONFIG.VIEWER_ID} NÃO EXISTE no DOM`);
             return;
         }
 
         console.log(`✅ #${CONFIG.VIEWER_ID} existe`);
-        console.log('📏 Dimensões:', {
+        console.log("📏 Dimensões:", {
             offsetWidth: reportContainer.offsetWidth,
             offsetHeight: reportContainer.offsetHeight,
             clientWidth: reportContainer.clientWidth,
             clientHeight: reportContainer.clientHeight,
             scrollWidth: reportContainer.scrollWidth,
-            scrollHeight: reportContainer.scrollHeight,
+            scrollHeight: reportContainer.scrollHeight
         });
 
         const styles = window.getComputedStyle(reportContainer);
-        console.log('🎨 Estilos computados:', {
+        console.log("🎨 Estilos computados:", {
             display: styles.display,
             visibility: styles.visibility,
             opacity: styles.opacity,
@@ -1026,80 +1017,74 @@
             maxHeight: styles.maxHeight,
             position: styles.position,
             zIndex: styles.zIndex,
-            overflow: styles.overflow,
+            overflow: styles.overflow
         });
 
-        const reportContainerAgenda = document.getElementById(
-            CONFIG.CONTAINER_ID,
-        );
-        if (reportContainerAgenda) {
+        const reportContainerAgenda = document.getElementById(CONFIG.CONTAINER_ID);
+        if (reportContainerAgenda)
+        {
             console.log(`✅ #${CONFIG.CONTAINER_ID} existe`);
             const styles2 = window.getComputedStyle(reportContainerAgenda);
-            console.log('📏 Dimensões:', {
+            console.log("📏 Dimensões:", {
                 offsetWidth: reportContainerAgenda.offsetWidth,
-                offsetHeight: reportContainerAgenda.offsetHeight,
+                offsetHeight: reportContainerAgenda.offsetHeight
             });
-            console.log('🎨 Estilos:', {
+            console.log("🎨 Estilos:", {
                 display: styles2.display,
                 visibility: styles2.visibility,
                 opacity: styles2.opacity,
                 height: styles2.height,
-                minHeight: styles2.minHeight,
+                minHeight: styles2.minHeight
             });
-        } else {
+        } else
+        {
             console.warn(`⚠️ #${CONFIG.CONTAINER_ID} NÃO EXISTE`);
         }
 
         const cardRelatorio = document.getElementById(CONFIG.CARD_ID);
-        if (cardRelatorio) {
+        if (cardRelatorio)
+        {
             console.log(`✅ #${CONFIG.CARD_ID} existe`);
             const styles3 = window.getComputedStyle(cardRelatorio);
-            console.log('📏 Dimensões:', {
+            console.log("📏 Dimensões:", {
                 offsetWidth: cardRelatorio.offsetWidth,
-                offsetHeight: cardRelatorio.offsetHeight,
+                offsetHeight: cardRelatorio.offsetHeight
             });
-            console.log('🎨 Estilos:', {
+            console.log("🎨 Estilos:", {
                 display: styles3.display,
                 visibility: styles3.visibility,
-                opacity: styles3.opacity,
+                opacity: styles3.opacity
             });
-        } else {
+        } else
+        {
             console.warn(`⚠️ #${CONFIG.CARD_ID} NÃO EXISTE`);
         }
 
         const htmlLength = reportContainer.innerHTML.length;
-        console.log('📄 Tamanho do HTML:', htmlLength);
-        if (htmlLength > 0) {
-            console.log(
-                '📄 Primeiros 500 caracteres:',
-                reportContainer.innerHTML.substring(0, 500),
-            );
-        }
-
-        const viewerInstance = $(`#${CONFIG.VIEWER_ID}`).data(
-            'telerik_ReportViewer',
-        );
-        console.log(
-            '🔧 Instância do viewer:',
-            viewerInstance ? 'EXISTE' : 'NÃO EXISTE',
-        );
-
-        if (viewerInstance) {
-            try {
-                console.log('📊 Estado do viewer:', {
-                    reportSource: viewerInstance.reportSource
-                        ? viewerInstance.reportSource()
-                        : null,
-                    serviceUrl: viewerInstance.serviceUrl
-                        ? viewerInstance.serviceUrl()
-                        : null,
+        console.log("📄 Tamanho do HTML:", htmlLength);
+        if (htmlLength > 0)
+        {
+            console.log("📄 Primeiros 500 caracteres:", reportContainer.innerHTML.substring(0, 500));
+        }
+
+        const viewerInstance = $(`#${CONFIG.VIEWER_ID}`).data('telerik_ReportViewer');
+        console.log("🔧 Instância do viewer:", viewerInstance ? "EXISTE" : "NÃO EXISTE");
+
+        if (viewerInstance)
+        {
+            try
+            {
+                console.log("📊 Estado do viewer:", {
+                    reportSource: viewerInstance.reportSource ? viewerInstance.reportSource() : null,
+                    serviceUrl: viewerInstance.serviceUrl ? viewerInstance.serviceUrl() : null
                 });
-            } catch (e) {
-                console.warn('⚠️ Erro ao obter estado do viewer:', e);
-            }
-        }
-
-        console.log('🔍 ===== FIM DO DIAGNÓSTICO =====');
+            } catch (e)
+            {
+                console.warn("⚠️ Erro ao obter estado do viewer:", e);
+            }
+        }
+
+        console.log("🔍 ===== FIM DO DIAGNÓSTICO =====");
     }
 
     window.carregarRelatorioViagem = carregarRelatorioViagem;
@@ -1107,87 +1092,83 @@
     window.esconderRelatorio = esconderRelatorio;
     window.limparRelatorio = limparRelatorio;
     window.obterEstadoRelatorio = obterEstado;
-    window.diagnosticarVisibilidadeRelatorio =
-        diagnosticarVisibilidadeRelatorio;
-
-    console.log('✅ Módulo de relatório carregado!');
-    console.log('✅ Funções registradas globalmente:', {
+    window.diagnosticarVisibilidadeRelatorio = diagnosticarVisibilidadeRelatorio;
+
+    console.log("✅ Módulo de relatório carregado!");
+    console.log("✅ Funções registradas globalmente:", {
         carregarRelatorioViagem: typeof carregarRelatorioViagem,
         mostrarRelatorio: typeof mostrarRelatorio,
         esconderRelatorio: typeof esconderRelatorio,
         limparRelatorio: typeof limparRelatorio,
         obterEstadoRelatorio: typeof obterEstado,
-        diagnosticarVisibilidadeRelatorio:
-            typeof diagnosticarVisibilidadeRelatorio,
+        diagnosticarVisibilidadeRelatorio: typeof diagnosticarVisibilidadeRelatorio
     });
+
 })();
 
-async function aguardarTelerikReportViewer() {
+async function aguardarTelerikReportViewer()
+{
     console.log('[Relatório] Aguardando Telerik ReportViewer...');
 
     const maxTentativas = 50;
     const intervalo = 100;
 
-    for (let i = 0; i < maxTentativas; i++) {
-
-        if (
-            typeof $ !== 'undefined' &&
+    for (let i = 0; i < maxTentativas; i++)
+    {
+
+        if (typeof $ !== 'undefined' &&
             typeof $.fn !== 'undefined' &&
-            typeof $.fn.telerik_ReportViewer === 'function'
-        ) {
-            console.log(
-                '[Relatório] ✅ Telerik ReportViewer disponível após',
-                i * intervalo,
-                'ms',
-            );
-
-            if (
-                typeof telerikReportViewer === 'undefined' &&
-                typeof window.telerikReportViewer === 'undefined'
-            ) {
-                console.warn(
-                    '[Relatório] ⚠️ Objeto telerikReportViewer global não encontrado',
-                );
-
-                if (typeof Telerik !== 'undefined' && Telerik.ReportViewer) {
+            typeof $.fn.telerik_ReportViewer === 'function')
+        {
+
+            console.log('[Relatório] ✅ Telerik ReportViewer disponível após', i * intervalo, 'ms');
+
+            if (typeof telerikReportViewer === 'undefined' && typeof window.telerikReportViewer === 'undefined')
+            {
+                console.warn('[Relatório] ⚠️ Objeto telerikReportViewer global não encontrado');
+
+                if (typeof Telerik !== 'undefined' && Telerik.ReportViewer)
+                {
                     window.telerikReportViewer = Telerik.ReportViewer;
-                    console.log(
-                        '[Relatório] Objeto telerikReportViewer encontrado em Telerik.ReportViewer',
-                    );
+                    console.log('[Relatório] Objeto telerikReportViewer encontrado em Telerik.ReportViewer');
                 }
             }
 
             return true;
         }
 
-        await new Promise((resolve) => setTimeout(resolve, intervalo));
+        await new Promise(resolve => setTimeout(resolve, intervalo));
     }
 
     throw new Error('Telerik ReportViewer não foi carregado após 5 segundos');
 }
 
-if (typeof window.carregarRelatorioViagem !== 'function') {
-    window.carregarRelatorioViagem = function (viagemId) {
+if (typeof window.carregarRelatorioViagem !== 'function')
+{
+    window.carregarRelatorioViagem = function (viagemId)
+    {
         console.log('[Relatório] Função simplificada - ViagemId:', viagemId);
 
-        try {
-
-            if (!viagemId) {
+        try
+        {
+
+            if (!viagemId)
+            {
                 console.error('[Relatório] ViagemId não fornecido');
                 return;
             }
 
             const $viewer = $('#reportViewerAgenda');
-            if ($viewer.length === 0 || !$.fn.telerik_ReportViewer) {
+            if ($viewer.length === 0 || !$.fn.telerik_ReportViewer)
+            {
                 console.error('[Relatório] Viewer não disponível');
                 return;
             }
 
             const oldViewer = $viewer.data('telerik_ReportViewer');
-            if (oldViewer && oldViewer.dispose) {
-                try {
-                    oldViewer.dispose();
-                } catch (e) {}
+            if (oldViewer && oldViewer.dispose)
+            {
+                try { oldViewer.dispose(); } catch (e) { }
             }
 
             $viewer.empty().telerik_ReportViewer({
@@ -1195,42 +1176,52 @@
                 reportSource: {
                     report: 'Agendamento.trdp',
                     parameters: {
-                        ViagemId: viagemId.toString().toUpperCase(),
-                    },
+                        ViagemId: viagemId.toString().toUpperCase()
+                    }
                 },
-                scale: 1.0,
+                scale: 1.0
             });
 
             $('#cardRelatorio').show();
             $('#ReportContainerAgenda').show();
-        } catch (error) {
+
+        } catch (error)
+        {
             console.error('[Relatório] Erro:', error);
         }
     };
 }
 
-async function destruirViewerAnterior() {
+async function destruirViewerAnterior()
+{
     console.log('[Relatório] Destruindo viewer anterior...');
 
-    try {
+    try
+    {
 
         const $viewer = $('#reportViewerAgenda');
 
-        if ($viewer.length > 0) {
+        if ($viewer.length > 0)
+        {
 
             const instance = $viewer.data('telerik_ReportViewer');
-            if (instance) {
+            if (instance)
+            {
                 console.log('[Relatório] Destruindo instância Telerik...');
 
-                try {
-
-                    if (typeof instance.dispose === 'function') {
+                try
+                {
+
+                    if (typeof instance.dispose === 'function')
+                    {
                         instance.dispose();
                     }
-                    if (typeof instance.destroy === 'function') {
+                    if (typeof instance.destroy === 'function')
+                    {
                         instance.destroy();
                     }
-                } catch (e) {
+                } catch (e)
+                {
                     console.warn('[Relatório] Erro ao destruir instância:', e);
                 }
 
@@ -1244,12 +1235,16 @@
             $viewer.empty();
         }
 
-        if (window.telerikReportViewer) {
-            try {
-                if (typeof window.telerikReportViewer.dispose === 'function') {
+        if (window.telerikReportViewer)
+        {
+            try
+            {
+                if (typeof window.telerikReportViewer.dispose === 'function')
+                {
                     window.telerikReportViewer.dispose();
                 }
-            } catch (e) {
+            } catch (e)
+            {
 
             }
             window.telerikReportViewer = null;
@@ -1257,10 +1252,12 @@
 
         $('.k-window, .k-overlay').remove();
 
-        await new Promise((resolve) => setTimeout(resolve, 100));
+        await new Promise(resolve => setTimeout(resolve, 100));
 
         console.log('[Relatório] ✅ Viewer anterior destruído');
-    } catch (error) {
+
+    } catch (error)
+    {
         console.error('[Relatório] Erro ao destruir viewer:', error);
 
     }
```
