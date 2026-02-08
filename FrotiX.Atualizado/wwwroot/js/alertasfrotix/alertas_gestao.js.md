# wwwroot/js/alertasfrotix/alertas_gestao.js

**Mudanca:** GRANDE | **+1178** linhas | **-1104** linhas

---

```diff
--- JANEIRO: wwwroot/js/alertasfrotix/alertas_gestao.js
+++ ATUAL: wwwroot/js/alertasfrotix/alertas_gestao.js
@@ -7,12 +7,15 @@
 
 var alertaDetalhesAtual = null;
 
-$(document).ready(function () {
-    try {
+$(document).ready(function ()
+{
+    try
+    {
         inicializarDataTableLidos();
         inicializarDataTableMeusAlertas();
 
-        if ($('#alertasAtivosContainer').length > 0) {
+        if ($('#alertasAtivosContainer').length > 0)
+        {
             carregarAlertasGestao();
         }
 
@@ -21,19 +24,17 @@
         inicializarSignalR();
         iniciarRecarregamentoAutomatico();
 
-        $(document).on('click', '#btnBaixaAlerta', function (e) {
+        $(document).on('click', '#btnBaixaAlerta', function (e)
+        {
             e.preventDefault();
             e.stopPropagation();
 
             const alertaId = $(this).data('alerta-id');
 
-            if (!alertaId) {
+            if (!alertaId)
+            {
                 console.error('ID do alerta não encontrado no botão');
-                AppToast.show(
-                    'Vermelho',
-                    'Erro: ID do alerta não encontrado',
-                    2000,
-                );
+                AppToast.show("Vermelho", "Erro: ID do alerta não encontrado", 2000);
                 return;
             }
 
@@ -41,23 +42,27 @@
 
             darBaixaAlertaComConfirmacao(alertaId);
         });
-    } catch (error) {
-        TratamentoErroComLinha('alertas_gestao.js', 'document.ready', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "document.ready", error);
     }
 });
 
-async function carregarAlertasSeguro() {
-    try {
+async function carregarAlertasSeguro()
+{
+    try
+    {
         const url = '/api/alertas';
         console.log('🔄 Carregando alertas de:', url);
 
         const response = await fetch(url, {
             method: 'GET',
             headers: {
-                Accept: 'application/json',
-                'Content-Type': 'application/json',
+                'Accept': 'application/json',
+                'Content-Type': 'application/json'
             },
-            credentials: 'same-origin',
+            credentials: 'same-origin'
         });
 
         console.log('📊 Status:', response.status, response.statusText);
@@ -65,7 +70,8 @@
         const contentType = response.headers.get('content-type');
         console.log('📄 Content-Type:', contentType);
 
-        if (!contentType || !contentType.includes('application/json')) {
+        if (!contentType || !contentType.includes('application/json'))
+        {
             const textoErro = await response.text();
 
             console.error('❌ Resposta não é JSON!');
@@ -73,8 +79,8 @@
 
             const erro = new Error(
                 `A API retornou ${contentType || 'conteúdo HTML'} ao invés de JSON. ` +
-                    `Status: ${response.status}. ` +
-                    `Verifique se a URL está correta e se o servidor está funcionando.`,
+                `Status: ${response.status}. ` +
+                `Verifique se a URL está correta e se o servidor está funcionando.`
             );
             erro.statusCode = response.status;
             erro.contentType = contentType;
@@ -87,13 +93,15 @@
         console.log('✅ Dados recebidos:', data);
 
         return data;
-    } catch (erro) {
+
+    } catch (erro)
+    {
         console.error('❌ Erro detalhado:', {
             nome: erro.name,
             mensagem: erro.message,
             status: erro.statusCode,
             contentType: erro.contentType,
-            preview: erro.responsePreview,
+            preview: erro.responsePreview
         });
 
         await Alerta.TratamentoErroComLinha(
@@ -105,40 +113,35 @@
                 stack: erro.stack,
                 statusCode: erro.statusCode || 'N/A',
                 contentType: erro.contentType || 'desconhecido',
-                preview: erro.responsePreview,
-            },
+                preview: erro.responsePreview
+            }
         );
 
         return null;
     }
 }
 
-function inicializarProgressBar() {
-    try {
-        console.log(
-            '✅ ProgressBar Bootstrap pronto (não requer inicialização)',
-        );
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'inicializarProgressBar',
-            error,
-        );
-    }
-}
-
-{
-    try {
+function inicializarProgressBar()
+{
+    try
+    {
+        console.log('✅ ProgressBar Bootstrap pronto (não requer inicialização)');
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "inicializarProgressBar", error);
+    }
+}
+
+{
+    try
+    {
         var progressBar = document.getElementById('progressoLeitura');
         var progressWarned = false;
 
         function verificarProgressBar(tentativa) {
             try {
-                if (
-                    progressBar &&
-                    progressBar.ej2_instances &&
-                    progressBar.ej2_instances[0]
-                ) {
+                if (progressBar && progressBar.ej2_instances && progressBar.ej2_instances[0]) {
                     console.log('✅ ProgressBar Syncfusion inicializado');
                     return;
                 }
@@ -149,127 +152,123 @@
                     return;
                 }
 
-                setTimeout(function () {
-                    verificarProgressBar(tentativa + 1);
-                }, 400);
+                setTimeout(function () { verificarProgressBar(tentativa + 1); }, 400);
             } catch (e) {
                 console.error('❌ Erro ao verificar ProgressBar:', e);
             }
         }
 
-        if (progressBar) {
-
-            setTimeout(function () {
-                verificarProgressBar(1);
-            }, 300);
-        } else {
+        if (progressBar)
+        {
+
+            setTimeout(function () { verificarProgressBar(1); }, 300);
+        } else
+        {
             console.warn('⚠️ Elemento progressoLeitura não encontrado no DOM');
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'inicializarProgressBar',
-            error,
-        );
-    }
-}
-
-$('#modalDetalhesAlerta').on('shown.bs.modal', function () {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "inicializarProgressBar", error);
+    }
+}
+
+$('#modalDetalhesAlerta').on('shown.bs.modal', function ()
+{
+    try
+    {
         inicializarProgressBar();
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'modalDetalhesAlerta.shown',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "modalDetalhesAlerta.shown", error);
     }
 });
 
-function inicializarDataTableLidos() {
-    try {
+function inicializarDataTableLidos()
+{
+    try
+    {
         if ($('#tblAlertasInativos').length === 0) return;
 
         tabelaAlertasLidos = $('#tblAlertasInativos').DataTable({
-            processing: true,
-            serverSide: false,
-            ajax: {
-                url: '/api/AlertasFrotiX/GetAlertasInativos',
-                type: 'GET',
-                datatype: 'json',
-                dataSrc: function (json) {
-                    if (json && json.data) {
+            "processing": true,
+            "serverSide": false,
+            "ajax": {
+                "url": "/api/AlertasFrotiX/GetAlertasInativos",
+                "type": "GET",
+                "datatype": "json",
+                "dataSrc": function (json)
+                {
+                    if (json && json.data)
+                    {
                         atualizarBadgeInativos(json.data.length);
                         return json.data;
                     }
                     atualizarBadgeInativos(0);
                     return [];
                 },
-                error: function (xhr, error, code) {
-                    TratamentoErroComLinha(
-                        'alertas_gestao.js',
-                        'ajax.error',
-                        error,
-                    );
+                "error": function (xhr, error, code)
+                {
+                    TratamentoErroComLinha("alertas_gestao.js", "ajax.error", error);
                     atualizarBadgeInativos(0);
-                },
+                }
             },
-            columns: [
-                {
-                    data: 'dataInsercao',
-                    width: '10%',
-                    className: 'text-center',
-                },
-                {
-                    data: 'icone',
-                    render: function (data, type, row) {
+            "columns": [
+                {
+                    "data": "icone",
+                    "render": function (data, type, row)
+                    {
                         var corIcone = obterCorPorTipo(row.tipo);
                         return `<i class="${data} fa-lg" style="color: ${corIcone}"></i>`;
                     },
-                    className: 'text-center',
-                    width: '3%',
+                    "className": "text-center",
+                    "width": "3%"
                 },
                 {
-                    data: 'titulo',
-                    width: '15%',
+                    "data": "titulo",
+                    "width": "17%"
                 },
                 {
-                    data: 'descricao',
-                    width: '35%',
+                    "data": "descricao",
+                    "width": "37%"
                 },
                 {
-                    data: 'tipo',
-                    render: function (data) {
+                    "data": "tipo",
+                    "render": function (data)
+                    {
                         var badgeClass = obterClasseBadge(data);
                         return `<span class="badge badge-tipo ${badgeClass}">${data}</span>`;
                     },
-                    className: 'text-center',
-                    width: '7%',
+                    "className": "text-center",
+                    "width": "7%"
                 },
                 {
-                    data: 'prioridade',
-                    render: function (data) {
+                    "data": "prioridade",
+                    "render": function (data)
+                    {
                         var classPrioridade = obterClassePrioridade(data);
-                        return `<span class="badge-prioridade-ftx ${classPrioridade}">${data}</span>`;
+                        return `<span class="${classPrioridade}"><i class="fa-duotone fa-flag"></i> ${data}</span>`;
                     },
-                    className: 'text-center',
-                    width: '7%',
+                    "className": "text-center",
+                    "width": "7%"
                 },
                 {
-                    data: 'dataDesativacao',
-                    width: '10%',
-                    className: 'text-center',
+                    "data": "dataInsercao",
+                    "width": "8%",
+                    "className": "text-center"
                 },
                 {
-                    data: 'percentualLeitura',
-                    render: function (data, type, row) {
+                    "data": "dataDesativacao",
+                    "width": "8%",
+                    "className": "text-center"
+                },
+                {
+                    "data": "percentualLeitura",
+                    "render": function (data, type, row)
+                    {
                         var percentual = parseFloat(data) || 0;
-                        var cor =
-                            percentual >= 75
-                                ? 'success'
-                                : percentual >= 50
-                                  ? 'warning'
-                                  : 'danger';
+                        var cor = percentual >= 75 ? 'success' : percentual >= 50 ? 'warning' : 'danger';
 
                         return `
                             <div class="d-flex align-items-center justify-content-center">
@@ -284,64 +283,68 @@
                                 <span class="badge bg-${cor}">${percentual.toFixed(1)}%</span>
                             </div>`;
                     },
-                    className: 'text-center',
-                    width: '8%',
+                    "className": "text-center",
+                    "width": "8%"
                 },
                 {
-                    data: 'alertaId',
-                    render: function (data, type, row) {
+                    "data": "alertaId",
+                    "render": function (data, type, row)
+                    {
                         return `
-                            <button class="btn btn-eye-frotix btn-sm"
+                            <button class="btn btn-info btn-sm"
                                     onclick="verDetalhesAlertaInativo('${data}')"
                                     title="Ver Detalhes"
                                     data-ejtip="Ver detalhes completos do alerta">
                                 <i class="fa-duotone fa-eye"></i>
                             </button>`;
                     },
-                    className: 'text-center',
-                    width: '5%',
-                },
+                    "className": "text-center",
+                    "orderable": false,
+                    "width": "5%"
+                }
             ],
-            columnDefs: [{ orderable: false, targets: '_all' }],
-            order: [[0, 'desc']],
-            language: {
-                url: '
+            "order": [[6, "desc"]],
+            "language": {
+                "url": "
             },
-            pageLength: 25,
-            lengthMenu: [
-                [10, 25, 50, 100],
-                [10, 25, 50, 100],
-            ],
-            responsive: true,
-            dom: '<"top"lf>rt<"bottom"ip><"clear">',
-            initComplete: function () {
+            "pageLength": 25,
+            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
+            "responsive": true,
+            "dom": '<"top"lf>rt<"bottom"ip><"clear">',
+            "initComplete": function ()
+            {
                 configurarTooltips();
-            },
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'inicializarDataTableLidos',
-            error,
-        );
-    }
-}
-
-function atualizarBadgeInativos(quantidade) {
-    try {
+            }
+        });
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "inicializarDataTableLidos", error);
+    }
+}
+
+function atualizarBadgeInativos(quantidade)
+{
+    try
+    {
         var badge = $('#badgeInativos');
-        if (badge.length) {
+        if (badge.length)
+        {
             badge.text(quantidade);
         }
-    } catch (error) {
+    }
+    catch (error)
+    {
         console.error('Erro ao atualizar badge de inativos:', error);
     }
 }
 
-function obterCorPorTipo(tipo) {
+function obterCorPorTipo(tipo)
+{
     var tipoLower = tipo.toLowerCase();
 
-    switch (tipoLower) {
+    switch (tipoLower)
+    {
         case 'agendamento':
             return '#0ea5e9';
         case 'manutenção':
@@ -362,28 +365,39 @@
     }
 }
 
-function atualizarBadgeHistorico(quantidade) {
-    try {
+function atualizarBadgeHistorico(quantidade)
+{
+    try
+    {
         var badge = $('#badgeHistorico');
-        if (badge.length) {
+        if (badge.length)
+        {
             badge.text(quantidade);
-            if (quantidade > 0) {
+            if (quantidade > 0)
+            {
                 badge.removeClass('bg-secondary').addClass('bg-success');
-            } else {
+            } else
+            {
                 badge.removeClass('bg-success').addClass('bg-secondary');
             }
         }
-    } catch (error) {
+    }
+    catch (error)
+    {
         console.error('Erro ao atualizar badge do histórico:', error);
     }
 }
 
-function configurarEventHandlers() {
-    try {
+function configurarEventHandlers()
+{
+    try
+    {
 
         var sinoBtn = document.getElementById('alertasSinoBtn');
-        if (sinoBtn) {
-            sinoBtn.addEventListener('click', function (e) {
+        if (sinoBtn)
+        {
+            sinoBtn.addEventListener('click', function (e)
+            {
                 e.preventDefault();
                 e.stopPropagation();
                 toggleMenu();
@@ -391,16 +405,20 @@
         }
 
         var btnMarcarTodos = document.getElementById('btnMarcarTodosLidos');
-        if (btnMarcarTodos) {
-            btnMarcarTodos.addEventListener('click', function (e) {
+        if (btnMarcarTodos)
+        {
+            btnMarcarTodos.addEventListener('click', function (e)
+            {
                 e.stopPropagation();
                 marcarTodosComoLidos();
             });
         }
 
         var btnVerTodos = document.getElementById('btnVerTodosAlertas');
-        if (btnVerTodos) {
-            btnVerTodos.addEventListener('click', function (e) {
+        if (btnVerTodos)
+        {
+            btnVerTodos.addEventListener('click', function (e)
+            {
                 e.preventDefault();
                 e.stopPropagation();
 
@@ -410,130 +428,161 @@
             });
         }
 
-        document.addEventListener('click', function (e) {
+        document.addEventListener('click', function (e)
+        {
             var container = document.getElementById('alertasSinoContainer');
-            if (container && !container.contains(e.target) && menuEstaAberto) {
+            if (container && !container.contains(e.target) && menuEstaAberto)
+            {
                 fecharMenu();
             }
         });
 
         var menu = document.getElementById('alertasMenu');
-        if (menu) {
-            menu.addEventListener('click', function (e) {
+        if (menu)
+        {
+            menu.addEventListener('click', function (e)
+            {
                 e.stopPropagation();
             });
         }
 
-        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
-            try {
-                var target = $(e.target).attr('href');
-                if (target === '#tabAtivos') {
+        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e)
+        {
+            try
+            {
+                var target = $(e.target).attr("href");
+                if (target === "#tabAtivos")
+                {
                     carregarAlertasGestao();
-                } else if (target === '#tabInativos') {
-                    if (tabelaAlertasLidos && tabelaAlertasLidos.ajax) {
+                }
+                else if (target === "#tabInativos")
+                {
+                    if (tabelaAlertasLidos && tabelaAlertasLidos.ajax)
+                    {
                         tabelaAlertasLidos.ajax.reload(null, false);
                     }
-                } else if (
-                    target === '#tabMeusAlertas'
-                )
-                {
-                    if (tabelaMeusAlertas && tabelaMeusAlertas.ajax) {
+                }
+                else if (target === "#tabMeusAlertas")
+                {
+                    if (tabelaMeusAlertas && tabelaMeusAlertas.ajax)
+                    {
                         tabelaMeusAlertas.ajax.reload(null, false);
                     }
                 }
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'alertas_gestao.js',
-                    'shown.bs.tab',
-                    error,
-                );
-            }
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'configurarEventHandlers',
-            error,
-        );
-    }
-}
-
-function inicializarSignalR() {
-    try {
-        if (typeof SignalRManager === 'undefined') {
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("alertas_gestao.js", "shown.bs.tab", error);
+            }
+        });
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "configurarEventHandlers", error);
+    }
+}
+
+function inicializarSignalR()
+{
+    try
+    {
+        if (typeof SignalRManager === 'undefined')
+        {
             setTimeout(inicializarSignalR, 2000);
             return;
         }
 
         SignalRManager.getConnection()
-            .then(function (conn) {
+            .then(function (conn)
+            {
                 connectionSino = conn;
                 configurarEventosSignalR();
             })
-            .catch(function (err) {
+            .catch(function (err)
+            {
                 console.error('Sino: Erro ao conectar SignalR:', err);
                 setTimeout(inicializarSignalR, 10000);
             });
-    } catch (error) {
+    }
+    catch (error)
+    {
         console.error('Sino: Erro ao inicializar SignalR:', error);
     }
 }
 
-function configurarEventosSignalR() {
+function configurarEventosSignalR()
+{
     if (!connectionSino) return;
 
-    try {
-        SignalRManager.on('NovoAlerta', function (alerta) {
+    try
+    {
+        SignalRManager.on("NovoAlerta", function (alerta)
+        {
             exibirNovoAlerta(alerta);
         });
 
-        SignalRManager.on('AtualizarBadgeAlertas', function (quantidade) {
+        SignalRManager.on("AtualizarBadgeAlertas", function (quantidade)
+        {
             atualizarBadge(quantidade);
         });
 
-        SignalRManager.on('ExibirAlertasIniciais', function (alertas) {
-            if (alertas && alertas.length > 0) {
+        SignalRManager.on("ExibirAlertasIniciais", function (alertas)
+        {
+            if (alertas && alertas.length > 0)
+            {
                 renderizarAlertas(alertas);
             }
         });
 
         SignalRManager.registerCallback({
-            onReconnected: function () {
+            onReconnected: function ()
+            {
                 carregarAlertasAtivos();
-            },
-        });
-    } catch (error) {
+            }
+        });
+    }
+    catch (error)
+    {
         console.error('Sino: Erro ao configurar eventos:', error);
     }
 }
 
-function toggleMenu() {
-    if (menuEstaAberto) {
+function toggleMenu()
+{
+    if (menuEstaAberto)
+    {
         fecharMenu();
-    } else {
+    } else
+    {
         abrirMenu();
     }
 }
 
-function abrirMenu() {
+function abrirMenu()
+{
     var menu = document.getElementById('alertasMenu');
-    if (menu) {
+    if (menu)
+    {
         menu.classList.add('show');
         menuEstaAberto = true;
     }
 }
 
-function fecharMenu() {
+function fecharMenu()
+{
     var menu = document.getElementById('alertasMenu');
-    if (menu) {
+    if (menu)
+    {
         menu.classList.remove('show');
         menuEstaAberto = false;
     }
 }
 
-function carregarAlertasAtivos() {
+function carregarAlertasAtivos()
+{
     var container = document.getElementById('listaAlertasSino');
-    if (container) {
+    if (container)
+    {
         container.innerHTML = `
             <div class="alertas-vazio">
                 <i class="fa-duotone fa-spinner fa-spin"></i>
@@ -544,32 +593,40 @@
     fetch('/api/AlertasFrotiX/GetAlertasAtivos', {
         method: 'GET',
         headers: {
-            'Content-Type': 'application/json',
-        },
+            'Content-Type': 'application/json'
+        }
     })
-        .then(function (response) {
-            if (!response.ok) {
+        .then(function (response)
+        {
+            if (!response.ok)
+            {
                 throw new Error('HTTP ' + response.status);
             }
             return response.json();
         })
-        .then(function (data) {
-            if (Array.isArray(data)) {
+        .then(function (data)
+        {
+            if (Array.isArray(data))
+            {
                 alertasSino = data;
                 renderizarAlertas(alertasSino);
                 atualizarBadge(alertasSino.length);
 
-                if (alertasSino.length > 0) {
+                if (alertasSino.length > 0)
+                {
                     mostrarBotaoMarcarTodos();
-                    if (!menuEstaAberto) {
+                    if (!menuEstaAberto)
+                    {
                         setTimeout(abrirMenu, 500);
                     }
-                } else {
+                } else
+                {
                     esconderBotaoMarcarTodos();
                 }
             }
         })
-        .catch(function (error) {
+        .catch(function (error)
+        {
             console.error('Erro ao carregar alertas:', error);
             mostrarErroCarregamento('Erro ao carregar alertas');
             atualizarBadge(0);
@@ -577,13 +634,15 @@
         });
 }
 
-function renderizarAlertas(alertas) {
+function renderizarAlertas(alertas)
+{
     var container = document.getElementById('listaAlertasSino');
     if (!container) return;
 
     container.innerHTML = '';
 
-    if (!alertas || alertas.length === 0) {
+    if (!alertas || alertas.length === 0)
+    {
         container.innerHTML = `
             <div class="alertas-vazio">
                 <i class="fa-duotone fa-bell-slash"></i>
@@ -592,19 +651,23 @@
         return;
     }
 
-    alertas.forEach(function (alerta) {
+    alertas.forEach(function (alerta)
+    {
         container.insertAdjacentHTML('beforeend', criarHtmlAlerta(alerta));
     });
 
-    container.querySelectorAll('.btn-marcar-lido').forEach(function (btn) {
-        btn.addEventListener('click', function (e) {
+    container.querySelectorAll('.btn-marcar-lido').forEach(function (btn)
+    {
+        btn.addEventListener('click', function (e)
+        {
             e.stopPropagation();
             marcarComoLido(this.dataset.alertaId);
         });
     });
 }
 
-function criarHtmlAlerta(alerta) {
+function criarHtmlAlerta(alerta)
+{
     var tempo = formatarData(alerta.dataInsercao);
     var prioridadeInfo = obterIconePrioridade(alerta.prioridade);
     var tipoInfo = obterInfoTipo(alerta.tipo);
@@ -640,42 +703,29 @@
         </div>`;
 }
 
-function obterIconePrioridade(prioridade) {
-    var prioridadeNum =
-        typeof prioridade === 'string' ? parseInt(prioridade) : prioridade;
-
-    switch (prioridadeNum) {
+function obterIconePrioridade(prioridade)
+{
+    var prioridadeNum = typeof prioridade === 'string' ? parseInt(prioridade) : prioridade;
+
+    switch (prioridadeNum)
+    {
         case 1:
-            return {
-                icone: 'fa-duotone fa-circle-info',
-                cor: '#0ea5e9',
-                nome: 'Prioridade Baixa',
-            };
+            return { icone: 'fa-duotone fa-circle-info', cor: '#0ea5e9', nome: 'Prioridade Baixa' };
         case 2:
-            return {
-                icone: 'fa-duotone fa-circle-exclamation',
-                cor: '#f59e0b',
-                nome: 'Prioridade Média',
-            };
+            return { icone: 'fa-duotone fa-circle-exclamation', cor: '#f59e0b', nome: 'Prioridade Média' };
         case 3:
-            return {
-                icone: 'fa-duotone fa-triangle-exclamation',
-                cor: '#dc2626',
-                nome: 'Prioridade Alta',
-            };
+            return { icone: 'fa-duotone fa-triangle-exclamation', cor: '#dc2626', nome: 'Prioridade Alta' };
         default:
-            return {
-                icone: 'fa-duotone fa-circle',
-                cor: '#6b7280',
-                nome: 'Normal',
-            };
-    }
-}
-
-function obterInfoTipo(tipo) {
+            return { icone: 'fa-duotone fa-circle', cor: '#6b7280', nome: 'Normal' };
+    }
+}
+
+function obterInfoTipo(tipo)
+{
     var tipoNum = typeof tipo === 'string' ? parseInt(tipo) : tipo;
 
-    switch (tipoNum) {
+    switch (tipoNum)
+    {
         case 1:
             return { texto: 'Agendamento', cor: '#0ea5e9' };
         case 2:
@@ -687,62 +737,73 @@
         case 5:
             return { texto: 'Anúncio', cor: '#dc2626' };
         case 6:
-            return { texto: 'Aniversário', cor: '#6b7280' };
+            return { texto: 'Diversos', cor: '#6b7280' };
         default:
             return { texto: 'Info', cor: '#6b7280' };
     }
 }
 
-function exibirNovoAlerta(alerta) {
+function exibirNovoAlerta(alerta)
+{
     alertasSino.unshift(alerta);
     renderizarAlertas(alertasSino);
     atualizarBadge(alertasSino.length);
     mostrarBotaoMarcarTodos();
 
-    if (!menuEstaAberto) {
+    if (!menuEstaAberto)
+    {
         abrirMenu();
     }
 }
 
-function atualizarBadge(quantidade) {
+function atualizarBadge(quantidade)
+{
     var badge = document.getElementById('badgeAlertasSino');
     var btn = document.getElementById('alertasSinoBtn');
 
     if (!badge || !btn) return;
 
-    if (quantidade > 0) {
+    if (quantidade > 0)
+    {
         badge.textContent = quantidade > 99 ? '99+' : quantidade;
         badge.style.display = 'inline-block';
         btn.classList.add('has-alerts');
-    } else {
+    } else
+    {
         badge.style.display = 'none';
         btn.classList.remove('has-alerts');
     }
 }
 
-function marcarTodosComoLidos() {
+function marcarTodosComoLidos()
+{
     if (alertasSino.length === 0) return;
 
     fetch('/api/AlertasFrotiX/MarcarTodosComoLidos', {
         method: 'POST',
-        headers: { 'Content-Type': 'application/json' },
+        headers: { 'Content-Type': 'application/json' }
     })
-        .then((response) => response.json())
-        .then(function (data) {
-            if (data && data.success) {
+        .then(response => response.json())
+        .then(function (data)
+        {
+            if (data && data.success)
+            {
                 alertasSino = [];
                 renderizarAlertas(alertasSino);
                 atualizarBadge(0);
                 esconderBotaoMarcarTodos();
             }
         })
-        .catch((error) => console.error('Erro:', error));
-}
-
-function verificarListaVazia() {
-    if (alertasSino.length === 0) {
+        .catch(error => console.error('Erro:', error));
+}
+
+function verificarListaVazia()
+{
+    if (alertasSino.length === 0)
+    {
         var container = document.getElementById('listaAlertasSino');
-        if (container) {
+        if (container)
+        {
             container.innerHTML = `
                 <div class="alertas-vazio">
                     <i class="fa-duotone fa-bell-slash"></i>
@@ -753,19 +814,23 @@
     }
 }
 
-function mostrarBotaoMarcarTodos() {
+function mostrarBotaoMarcarTodos()
+{
     var btn = document.getElementById('btnMarcarTodosLidos');
     if (btn) btn.style.display = 'inline-block';
 }
 
-function esconderBotaoMarcarTodos() {
+function esconderBotaoMarcarTodos()
+{
     var btn = document.getElementById('btnMarcarTodosLidos');
     if (btn) btn.style.display = 'none';
 }
 
-function mostrarErroCarregamento(mensagem) {
+function mostrarErroCarregamento(mensagem)
+{
     var container = document.getElementById('listaAlertasSino');
-    if (container) {
+    if (container)
+    {
         container.innerHTML = `
             <div class="alertas-vazio">
                 <i class="fa-duotone fa-exclamation-triangle"></i>
@@ -779,145 +844,141 @@
 
 var intervaloRecarregamento;
 
-function iniciarRecarregamentoAutomatico() {
+function iniciarRecarregamentoAutomatico()
+{
     if (intervaloRecarregamento) clearInterval(intervaloRecarregamento);
 
-    intervaloRecarregamento = setInterval(function () {
-        if (!document.hidden) {
+    intervaloRecarregamento = setInterval(function ()
+    {
+        if (!document.hidden)
+        {
             carregarAlertasAtivos();
         }
     }, 30000);
 }
 
-document.addEventListener('visibilitychange', function () {
-    if (document.hidden) {
+document.addEventListener('visibilitychange', function ()
+{
+    if (document.hidden)
+    {
         if (intervaloRecarregamento) clearInterval(intervaloRecarregamento);
-    } else {
+    } else
+    {
         iniciarRecarregamentoAutomatico();
         carregarAlertasAtivos();
     }
 });
 
-function obterUsuarioAtualId() {
-    try {
+function obterUsuarioAtualId()
+{
+    try
+    {
 
         return window.usuarioAtualId || $('#usuarioAtualId').val() || '';
-    } catch (error) {
+    }
+    catch (error)
+    {
         console.error('Erro ao obter ID do usuário:', error);
         return '';
     }
 }
 
-function darBaixaAlerta(alertaId) {
-    try {
+function darBaixaAlerta(alertaId)
+{
+    try
+    {
         Alerta.Confirmar(
-            'Tem certeza que deseja dar baixa neste alerta?',
-            'Esta ação não pode ser desfeita. O alerta será marcado como finalizado.',
-            'Sim, dar baixa',
-            'Cancelar',
-        ).then((willDelete) => {
-            try {
-                if (willDelete) {
+            "Tem certeza que deseja dar baixa neste alerta?",
+            "Esta ação não pode ser desfeita. O alerta será marcado como finalizado.",
+            "Sim, dar baixa",
+            "Cancelar"
+        ).then((willDelete) =>
+        {
+            try
+            {
+                if (willDelete)
+                {
                     $.ajax({
                         url: `/api/AlertasFrotiX/DarBaixaAlerta/${alertaId}`,
                         type: 'POST',
                         contentType: 'application/json',
-                        success: function (response) {
-                            try {
-                                if (response.success) {
-
-                                    AppToast.show(
-                                        'Verde',
-                                        'Baixa do alerta realizada com sucesso',
-                                        2000,
-                                    );
+                        success: function (response)
+                        {
+                            try
+                            {
+                                if (response.success)
+                                {
+
+                                    AppToast.show("Verde", "Baixa do alerta realizada com sucesso", 2000);
 
                                     $('#modalDetalhesAlerta').modal('hide');
 
                                     removerCardAlerta(alertaId);
 
-                                    setTimeout(function () {
+                                    setTimeout(function ()
+                                    {
 
                                         carregarAlertasGestao();
 
-                                        if (
-                                            tabelaAlertasLidos &&
-                                            tabelaAlertasLidos.ajax
-                                        ) {
-                                            tabelaAlertasLidos.ajax.reload(
-                                                null,
-                                                false,
-                                            );
+                                        if (tabelaAlertasLidos && tabelaAlertasLidos.ajax)
+                                        {
+                                            tabelaAlertasLidos.ajax.reload(null, false);
                                         }
 
                                         carregarAlertasAtivos();
+
                                     }, 500);
-                                } else {
-                                    AppToast.show(
-                                        'Amarelo',
-                                        response.mensagem ||
-                                            'Não foi possível dar baixa no alerta',
-                                        3000,
-                                    );
                                 }
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'alertas_gestao.js',
-                                    'darBaixaAlerta.success',
-                                    error,
-                                );
+                                else
+                                {
+                                    AppToast.show("Amarelo", response.mensagem || "Não foi possível dar baixa no alerta", 3000);
+                                }
+                            }
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha("alertas_gestao.js", "darBaixaAlerta.success", error);
                             }
                         },
-                        error: function (xhr, status, error) {
-                            try {
-                                console.error(
-                                    'Erro ao dar baixa no alerta:',
-                                    error,
-                                );
-
-                                var mensagemErro =
-                                    'Erro ao dar baixa no alerta';
-                                if (
-                                    xhr.responseJSON &&
-                                    xhr.responseJSON.mensagem
-                                ) {
+                        error: function (xhr, status, error)
+                        {
+                            try
+                            {
+                                console.error('Erro ao dar baixa no alerta:', error);
+
+                                var mensagemErro = "Erro ao dar baixa no alerta";
+                                if (xhr.responseJSON && xhr.responseJSON.mensagem)
+                                {
                                     mensagemErro = xhr.responseJSON.mensagem;
                                 }
 
-                                AppToast.show('Vermelho', mensagemErro, 3000);
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'alertas_gestao.js',
-                                    'darBaixaAlerta.error',
-                                    error,
-                                );
+                                AppToast.show("Vermelho", mensagemErro, 3000);
                             }
-                        },
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha("alertas_gestao.js", "darBaixaAlerta.error", error);
+                            }
+                        }
                     });
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'alertas_gestao.js',
-                    'darBaixaAlerta.callback@swal.then#0',
-                    error,
-                );
-            }
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'darBaixaAlerta',
-            error,
-        );
-    }
-}
-
-function adicionarCardAlerta(alerta) {
-    try {
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("alertas_gestao.js", "darBaixaAlerta.callback@swal.then#0", error);
+            }
+        });
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("alertas_gestao.js", "darBaixaAlerta", error);
+    }
+}
+
+function adicionarCardAlerta(alerta)
+{
+    try
+    {
         var prioridadeClass = obterClassePrioridade(alerta.prioridade);
-        var badgeClass = obterClasseBadge(
-            alerta.textoBadge || obterTextoPorTipo(alerta.tipo),
-        );
+        var badgeClass = obterClasseBadge(alerta.textoBadge || obterTextoPorTipo(alerta.tipo));
 
         var usuarioAtualId = obterUsuarioAtualId();
         var ehCriador = alerta.usuarioCriadorId === usuarioAtualId;
@@ -931,27 +992,23 @@
                                 <i class="${alerta.iconeCss} fa-2x me-3" style="color: ${alerta.corBadge}"></i>
                                 <div>
                                     <h6 class="card-title mb-1">${alerta.titulo}</h6>
-                                    <div class="d-flex flex-wrap gap-1">
-                                        <span class="badge badge-tipo ${badgeClass}">${alerta.textoBadge || obterTextoPorTipo(alerta.tipo)}</span>
-                                        <span class="badge-prioridade-ftx ${prioridadeClass}">${alerta.prioridade}</span>
-                                    </div>
+                                    <span class="badge badge-tipo ${badgeClass}">${alerta.textoBadge || obterTextoPorTipo(alerta.tipo)}</span>
                                 </div>
                             </div>
+                            <span class="${prioridadeClass}" title="Prioridade">
+                                <i class="fa-duotone fa-flag"></i>
+                            </span>
                         </div>
                         <p class="card-text text-muted small">${alerta.descricao}</p>
 
-                        ${
-                            alerta.totalUsuarios > 0
-                                ? `
+                        ${alerta.totalUsuarios > 0 ? `
                         <div class="mb-2">
                             <small class="text-muted">
                                 <i class="fa-duotone fa-users"></i>
                                 ${alerta.usuariosLeram}/${alerta.totalUsuarios} usuários leram
                             </small>
                         </div>
-                        `
-                                : ''
-                        }
+                        ` : ''}
 
                         <div class="d-flex justify-content-between align-items-center mt-3">
                             <small class="text-muted">
@@ -959,20 +1016,16 @@
                                 ${formatarData(alerta.dataInsercao)}
                             </small>
                             <div class="btn-group btn-group-sm">
-                                <button class="btn btn-eye-frotix btn-sm" onclick="verDetalhesAlerta('${alerta.alertaId}')"
-                                        title="Ver Detalhes" data-ejtip="Ver detalhes completos do alerta">
+                                <button class="btn btn-info btn-sm" onclick="verDetalhesAlerta('${alerta.alertaId}')"
+                                        title="Ver Detalhes">
                                     <i class="fa-duotone fa-eye"></i>
                                 </button>
-                                ${
-                                    ehCriador
-                                        ? `
+                                ${ehCriador ? `
                                 <button class="btn btn-vinho btn-sm" onclick="darBaixaAlerta('${alerta.alertaId}')"
                                         title="Dar Baixa no Alerta" data-ejtip="Finalizar este alerta">
                                     <i class="fa-duotone fa-circle-xmark"></i>
                                 </button>
-                                `
-                                        : ''
-                                }
+                                ` : ''}
                             </div>
                         </div>
                     </div>
@@ -981,32 +1034,37 @@
 
         $('#alertasAtivosContainer').prepend(cardHtml);
         configurarTooltips();
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'adicionarCardAlerta',
-            error,
-        );
-    }
-}
-function carregarAlertasGestao() {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "adicionarCardAlerta", error);
+    }
+}
+function carregarAlertasGestao()
+{
+    try
+    {
         $.ajax({
             url: '/api/AlertasFrotiX/GetTodosAlertasAtivosGestao',
             type: 'GET',
-            success: function (data) {
+            success: function (data)
+            {
                 $('#alertasAtivosContainer').empty();
 
-                if (Array.isArray(data) && data.length > 0) {
+                if (Array.isArray(data) && data.length > 0)
+                {
                     atualizarBadgeAtivos(data.length);
 
-                    data.forEach(function (alerta) {
-                        if (!alerta.usuarioCriadorId) {
+                    data.forEach(function (alerta)
+                    {
+                        if (!alerta.usuarioCriadorId)
+                        {
                             alerta.usuarioCriadorId = alerta.criadoPor || '';
                         }
                         adicionarCardAlerta(alerta);
                     });
-                } else {
+                } else
+                {
                     atualizarBadgeAtivos(0);
 
                     $('#alertasAtivosContainer').html(`
@@ -1017,7 +1075,8 @@
                     `);
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('Erro ao carregar alertas gestão:', error);
                 atualizarBadgeAtivos(0);
 
@@ -1030,23 +1089,24 @@
                         </button>
                     </div>
                 `);
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'carregarAlertasGestao',
-            error,
-        );
-    }
-}
-
-function removerCardAlerta(alertaId) {
-    try {
-        $(`#card-alerta-${alertaId}`).fadeOut(300, function () {
+            }
+        });
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("alertas_gestao.js", "carregarAlertasGestao", error);
+    }
+}
+
+function removerCardAlerta(alertaId)
+{
+    try
+    {
+        $(`#card-alerta-${alertaId}`).fadeOut(300, function ()
+        {
             $(this).remove();
 
-            if ($('#alertasAtivosContainer .card').length === 0) {
+            if ($('#alertasAtivosContainer .card').length === 0)
+            {
                 $('#alertasAtivosContainer').html(`
                     <div class="col-12 text-center py-5">
                         <i class="fa-duotone fa-bell-slash fa-3x text-muted mb-3"></i>
@@ -1055,112 +1115,113 @@
                 `);
             }
         });
-    } catch (error) {
-        TratamentoErroComLinha('alertas_gestao.js', 'removerCardAlerta', error);
-    }
-}
-
-function marcarComoLido(alertaId) {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "removerCardAlerta", error);
+    }
+}
+
+function marcarComoLido(alertaId)
+{
+    try
+    {
         $.ajax({
             url: `/api/AlertasFrotiX/MarcarComoLido/${alertaId}`,
             type: 'POST',
             contentType: 'application/json',
             dataType: 'json',
-            success: function (response) {
-                if (response.success) {
-                    AppToast.show('Verde', 'Alerta marcado como lido', 2000);
+            success: function (response)
+            {
+                if (response.success)
+                {
+                    AppToast.show("Verde", "Alerta marcado como lido", 2000);
 
                     removerCardAlerta(alertaId);
 
-                    var item = document.querySelector(
-                        '[data-alerta-id="' + alertaId + '"]',
-                    );
-                    if (item) {
+                    var item = document.querySelector('[data-alerta-id="' + alertaId + '"]');
+                    if (item)
+                    {
                         item.style.animation = 'slideUp 0.3s ease-out';
-                        setTimeout(function () {
+                        setTimeout(function ()
+                        {
                             item.remove();
-                            alertasSino = alertasSino.filter(
-                                (a) => a.alertaId !== alertaId,
-                            );
+                            alertasSino = alertasSino.filter(a => a.alertaId !== alertaId);
                             atualizarBadge(alertasSino.length);
                             verificarListaVazia();
                         }, 300);
                     }
 
-                    if (tabelaAlertasLidos && tabelaAlertasLidos.ajax) {
-                        setTimeout(function () {
+                    if (tabelaAlertasLidos && tabelaAlertasLidos.ajax)
+                    {
+                        setTimeout(function ()
+                        {
                             tabelaAlertasLidos.ajax.reload(null, false);
                         }, 500);
                     }
 
-                    if (tabelaMeusAlertas && tabelaMeusAlertas.ajax) {
-                        setTimeout(function () {
+                    if (tabelaMeusAlertas && tabelaMeusAlertas.ajax)
+                    {
+                        setTimeout(function ()
+                        {
                             tabelaMeusAlertas.ajax.reload(null, false);
                         }, 500);
                     }
-                } else {
+                }
+                else
+                {
                     console.error('Backend retornou success: false', response);
-                    AppToast.show(
-                        'Vermelho',
-                        response.message ||
-                            response.mensagem ||
-                            'Erro ao marcar como lido',
-                        2000,
-                    );
+                    AppToast.show("Vermelho", response.message || response.mensagem || "Erro ao marcar como lido", 2000);
                 }
             },
-            error: function (xhr, status, errorThrown) {
+            error: function (xhr, status, errorThrown)
+            {
                 console.error('Erro AJAX:', xhr.responseJSON);
                 console.error('Status:', status);
                 console.error('Error:', errorThrown);
 
-                var mensagemErro = 'Erro ao marcar alerta como lido';
-                if (xhr.responseJSON && xhr.responseJSON.message) {
+                var mensagemErro = "Erro ao marcar alerta como lido";
+                if (xhr.responseJSON && xhr.responseJSON.message)
+                {
                     mensagemErro = xhr.responseJSON.message;
                 }
 
-                AppToast.show('Vermelho', mensagemErro, 3000);
-
-                TratamentoErroComLinha(
-                    'alertas_gestao.js',
-                    'marcarComoLido.ajax.error',
-                    {
-                        message: errorThrown,
-                        status: status,
-                        response: xhr.responseJSON,
-                    },
-                );
-            },
-        });
-    } catch (err) {
+                AppToast.show("Vermelho", mensagemErro, 3000);
+
+                TratamentoErroComLinha("alertas_gestao.js", "marcarComoLido.ajax.error", {
+                    message: errorThrown,
+                    status: status,
+                    response: xhr.responseJSON
+                });
+            }
+        });
+    }
+    catch (err)
+    {
         console.error('Erro no try-catch:', err);
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'marcarComoLido.catch',
-            err,
-        );
-        AppToast.show('Vermelho', 'Erro ao processar requisição', 2000);
-    }
-}
-
-function visualizarDetalhes(alertaId) {
+        TratamentoErroComLinha("alertas_gestao.js", "marcarComoLido.catch", err);
+        AppToast.show("Vermelho", "Erro ao processar requisição", 2000);
+    }
+}
+
+function visualizarDetalhes(alertaId)
+{
     console.log('🔍 Visualizando detalhes do alerta:', alertaId);
 
     $.ajax({
         url: `/Alertas/GetDetalhesAlerta/${alertaId}`,
         method: 'GET',
-        success: function (response) {
+        success: function (response)
+        {
             console.log('✅ Resposta recebida:', response);
 
-            if (response.success && response.data) {
+            if (response.success && response.data)
+            {
                 const detalhes = response.data;
 
                 $('#totalDestinatarios').text(detalhes.totalDestinatarios || 0);
                 $('#totalNotificados').text(detalhes.totalNotificados || 0);
-                $('#aguardandoNotificacao').text(
-                    detalhes.aguardandoNotificacao || 0,
-                );
+                $('#aguardandoNotificacao').text(detalhes.aguardandoNotificacao || 0);
                 $('#usuariosLeram').text(detalhes.leram || 0);
                 $('#usuariosNaoLeram').text(detalhes.naoLeram || 0);
                 $('#usuariosApagaram').text(detalhes.apagaram || 0);
@@ -1168,23 +1229,24 @@
                 preencherTabelaUsuarios(detalhes.usuarios || []);
 
                 $('#modalDetalhesAlerta').modal('show');
-            } else {
+            } else
+            {
                 console.error('❌ Resposta inválida:', response);
-                AppToast.show(
-                    'Vermelho',
-                    'Erro ao carregar detalhes do alerta',
-                );
+                AppToast.show('Vermelho', 'Erro ao carregar detalhes do alerta');
             }
         },
-        error: function (xhr, status, error) {
+        error: function (xhr, status, error)
+        {
             console.error('❌ Erro na requisição:', error);
-            AppToast.show('Vermelho', 'Erro ao carregar detalhes: ' + error);
-        },
+            AppToast.show('Vermelho','Erro ao carregar detalhes: ' + error);
+        }
     });
 }
 
-function preencherEstatisticas(stats) {
-    if (!stats) {
+function preencherEstatisticas(stats)
+{
+    if (!stats)
+    {
         console.warn('⚠️ Estatísticas não disponíveis');
         return;
     }
@@ -1203,8 +1265,10 @@
         .attr('aria-valuenow', percentualLidos);
 }
 
-async function verDetalhesAlerta(alertaId) {
-    try {
+async function verDetalhesAlerta(alertaId)
+{
+    try
+    {
         console.log('Carregando detalhes do alerta:', alertaId);
 
         $('#modalDetalhesAlerta').modal('show');
@@ -1213,11 +1277,10 @@
 
         $('#btnBaixaAlerta').data('alerta-id', alertaId);
 
-        const response = await fetch(
-            `/api/AlertasFrotiX/GetDetalhesAlerta/${alertaId}`,
-        );
-
-        if (!response.ok) {
+        const response = await fetch(`/api/AlertasFrotiX/GetDetalhesAlerta/${alertaId}`);
+
+        if (!response.ok)
+        {
             throw new Error(`Erro HTTP ${response.status}`);
         }
 
@@ -1227,7 +1290,8 @@
         console.log('Leram:', resultado.data.leram);
         console.log('Usuários:', resultado.data.usuarios);
 
-        if (!resultado.success) {
+        if (!resultado.success)
+        {
             throw new Error(resultado.message || 'Erro ao carregar detalhes');
         }
 
@@ -1238,62 +1302,45 @@
 
         $('#loaderDetalhes').hide();
         $('#conteudoDetalhes').show();
-    } catch (erro) {
+
+    } catch (erro)
+    {
         console.error('Erro ao carregar detalhes:', erro);
 
         $('#loaderDetalhes').hide();
-        $('#conteudoDetalhes')
-            .html(
-                `
+        $('#conteudoDetalhes').html(`
             <div class="alert alert-danger">
                 <i class="fas fa-exclamation-triangle"></i>
                 Erro ao carregar detalhes do alerta: ${erro.message}
             </div>
-        `,
-            )
-            .show();
+        `).show();
 
         await Alerta.TratamentoErroComLinha(
             'alertas_gestao.js',
             'verDetalhesAlerta',
-            erro,
+            erro
         );
     }
 }
 
-function popularModalDetalhes(alerta) {
+function popularModalDetalhes(alerta)
+{
     console.log('Populando modal com dados:', alerta);
 
     $('#tituloAlerta').text(alerta.titulo || 'Sem título');
     $('#descricaoAlerta').text(alerta.descricao || 'Sem descrição');
 
-    const corStatus = alerta.ativo
-        ? '#28a745'
-        : alerta.expirado
-          ? '#ffc107'
-          : '#dc3545';
+    const corStatus = alerta.ativo ? '#28a745' : alerta.expirado ? '#ffc107' : '#dc3545';
     const textoStatus = alerta.status || 'Desconhecido';
 
-    $('#badgeTipo')
-        .attr('style', `background-color: ${alerta.corBadge}`)
-        .text(alerta.tipo);
-    $('#badgePrioridade')
-        .attr('style', `background-color: ${alerta.corBadge}`)
-        .text(alerta.prioridade);
-    $('#badgeStatus')
-        .attr('style', `background-color: ${corStatus}`)
-        .text(textoStatus);
-
-    $('#iconeAlerta')
-        .attr('class', alerta.iconeCss)
-        .attr('style', `color: ${alerta.corBadge}`);
+    $('#badgeTipo').attr('style', `background-color: ${alerta.corBadge}`).text(alerta.tipo);
+    $('#badgePrioridade').attr('style', `background-color: ${alerta.corBadge}`).text(alerta.prioridade);
+    $('#badgeStatus').attr('style', `background-color: ${corStatus}`).text(textoStatus);
+
+    $('#iconeAlerta').attr('class', alerta.iconeCss).attr('style', `color: ${alerta.corBadge}`);
 
     $('#dataCriacao').text(formatarDataHora(alerta.dataInsercao));
-    $('#dataExibicao').text(
-        alerta.dataExibicao
-            ? formatarDataHora(alerta.dataExibicao)
-            : 'Imediata',
-    );
+    $('#dataExibicao').text(alerta.dataExibicao ? formatarDataHora(alerta.dataExibicao) : 'Imediata');
     $('#criadoPor').text(obterNomeCriador(alerta.usuarioCriadorId));
     $('#tempoNoAr').text(alerta.tempoNoArFormatado || 'N/A');
 
@@ -1307,31 +1354,30 @@
     $('#totalApagaram').text(stats.totalApagados || 0);
 
     const percentual = stats.percentualLeitura || 0;
-    $('#progressoLeitura')
-        .css('width', percentual + '%')
-        .attr('aria-valuenow', percentual);
+    $('#progressoLeitura').css('width', percentual + '%').attr('aria-valuenow', percentual);
     $('#percentualLeitura').text(percentual.toFixed(1) + '%');
 
     popularTabelaUsuarios(alerta.usuarios || []);
 }
 
-function obterNomeCriador(criadorId) {
+function obterNomeCriador(criadorId)
+{
     if (!criadorId) return 'Desconhecido';
-    if (
-        criadorId.toLowerCase() === 'system' ||
-        criadorId.toLowerCase() === 'sistema'
-    ) {
+    if (criadorId.toLowerCase() === 'system' || criadorId.toLowerCase() === 'sistema')
+    {
         return 'Sistema';
     }
 
     return criadorId;
 }
 
-function popularTabelaUsuarios(usuarios) {
+function popularTabelaUsuarios(usuarios)
+{
     const tbody = $('#tabelaUsuarios tbody');
     tbody.empty();
 
-    if (!usuarios || usuarios.length === 0) {
+    if (!usuarios || usuarios.length === 0)
+    {
         tbody.html(`
             <tr>
                 <td colspan="5" class="text-center text-muted">
@@ -1342,14 +1388,11 @@
         return;
     }
 
-    usuarios.forEach((usuario) => {
+    usuarios.forEach(usuario =>
+    {
         const statusHtml = gerarStatusUsuario(usuario);
-        const dataNotificacao = usuario.dataNotificacao
-            ? formatarDataHora(usuario.dataNotificacao)
-            : '-';
-        const dataLeitura = usuario.dataLeitura
-            ? formatarDataHora(usuario.dataLeitura)
-            : '-';
+        const dataNotificacao = usuario.dataNotificacao ? formatarDataHora(usuario.dataNotificacao) : '-';
+        const dataLeitura = usuario.dataLeitura ? formatarDataHora(usuario.dataLeitura) : '-';
         const tempoLeitura = usuario.tempoAteLeitura
             ? formatarTempoMinutos(usuario.tempoAteLeitura)
             : '-';
@@ -1371,55 +1414,67 @@
     });
 }
 
-function formatarTempoMinutos(minutos) {
+function formatarTempoMinutos(minutos)
+{
     if (!minutos || minutos < 0) return '-';
 
-    if (minutos < 1) {
+    if (minutos < 1)
+    {
         return 'Menos de 1 min';
-    } else if (minutos < 60) {
+    } else if (minutos < 60)
+    {
         return Math.round(minutos) + ' min';
-    } else {
+    } else
+    {
         const horas = Math.floor(minutos / 60);
         const mins = Math.round(minutos % 60);
         return `${horas}h ${mins}min`;
     }
 }
 
-function formatarDataHora(dataString) {
+function formatarDataHora(dataString)
+{
     if (!dataString) return '-';
 
-    try {
+    try
+    {
         const data = new Date(dataString);
         return data.toLocaleString('pt-BR', {
             day: '2-digit',
             month: '2-digit',
             year: 'numeric',
             hour: '2-digit',
-            minute: '2-digit',
-        });
-    } catch (e) {
+            minute: '2-digit'
+        });
+    } catch (e)
+    {
         return dataString;
     }
 }
 
-function gerarStatusUsuario(usuario) {
+function gerarStatusUsuario(usuario)
+{
     let badge = '';
     let icone = '';
     let cor = '';
 
-    if (usuario.apagado) {
+    if (usuario.apagado)
+    {
         badge = 'Apagou sem ler';
         icone = 'fa-trash';
         cor = 'danger';
-    } else if (usuario.lido) {
+    } else if (usuario.lido)
+    {
         badge = 'Lido';
         icone = 'fa-check-circle';
         cor = 'success';
-    } else if (usuario.notificado) {
+    } else if (usuario.notificado)
+    {
         badge = 'Não lido';
         icone = 'fa-clock';
         cor = 'warning';
-    } else {
+    } else
+    {
         badge = 'Não notificado';
         icone = 'fa-circle';
         cor = 'secondary';
@@ -1428,8 +1483,10 @@
     return `<span class="badge badge-${cor}"><i class="fas ${icone}"></i> ${badge}</span>`;
 }
 
-function preencherModalDetalhes(alerta) {
-    try {
+function preencherModalDetalhes(alerta)
+{
+    try
+    {
         console.log('📋 Preenchendo modal com:', alerta);
 
         $('#tituloAlerta').text(alerta.titulo || 'Sem título');
@@ -1438,36 +1495,30 @@
         let statusHtml = '';
         let statusClass = '';
 
-        if (alerta.ativo) {
-            statusHtml =
-                '<span class="badge badge-success"><i class="fa-duotone fa-check-circle"></i> Ativo</span>';
+        if (alerta.ativo)
+        {
+            statusHtml = '<span class="badge badge-success"><i class="fa-duotone fa-check-circle"></i> Ativo</span>';
             statusClass = 'border-left-success';
-        } else if (alerta.expirado) {
-            statusHtml =
-                '<span class="badge badge-danger"><i class="fa-duotone fa-clock"></i> Expirado</span>';
+        } else if (alerta.expirado)
+        {
+            statusHtml = '<span class="badge badge-danger"><i class="fa-duotone fa-clock"></i> Expirado</span>';
             statusClass = 'border-left-danger';
-        } else {
-            statusHtml =
-                '<span class="badge badge-secondary"><i class="fa-duotone fa-ban"></i> Inativo</span>';
+        } else
+        {
+            statusHtml = '<span class="badge badge-secondary"><i class="fa-duotone fa-ban"></i> Inativo</span>';
             statusClass = 'border-left-secondary';
         }
 
         $('#badgeStatus').html(statusHtml);
         $('#alertaCabecalho')
-            .removeClass(
-                'border-left-success border-left-danger border-left-secondary',
-            )
+            .removeClass('border-left-success border-left-danger border-left-secondary')
             .addClass(statusClass);
 
-        $('#badgeTipo').html(
-            `<i class="fa-duotone fa-tag"></i> ${alerta.tipoAlerta || alerta.tipo || 'Geral'}`,
-        );
+        $('#badgeTipo').html(`<i class="fa-duotone fa-tag"></i> ${alerta.tipoAlerta || alerta.tipo || 'Geral'}`);
 
         const corPrioridade = alerta.corBadge || '#6c757d';
         $('#badgePrioridade')
-            .html(
-                `<i class="fa-duotone fa-flag"></i> ${alerta.prioridade || 'Normal'}`,
-            )
+            .html(`<i class="fa-duotone fa-flag"></i> ${alerta.prioridade || 'Normal'}`)
             .css('background-color', corPrioridade);
 
         const icone = alerta.iconeCss || 'fa-duotone fa-bell';
@@ -1475,55 +1526,37 @@
             .attr('class', `${icone} fa-3x`)
             .css('color', corPrioridade);
 
-        const dataCriacao =
-            alerta.dataCriacao || alerta.dataInsercao || alerta.dataCadastro;
-        $('#dataCriacao').text(
-            dataCriacao ? formatarDataCompleta(dataCriacao) : '-',
-        );
+        const dataCriacao = alerta.dataCriacao || alerta.dataInsercao || alerta.dataCadastro;
+        $('#dataCriacao').text(dataCriacao ? formatarDataCompleta(dataCriacao) : '-');
         console.log('📅 Data de Criação:', dataCriacao);
 
-        $('#dataExibicao').text(
-            alerta.dataExibicao
-                ? formatarDataCompleta(alerta.dataExibicao)
-                : 'Imediata',
-        );
-
-        const criador =
-            alerta.nomeCriador ||
-            alerta.criadoPor ||
-            alerta.usuarioCriador ||
-            'Sistema';
+        $('#dataExibicao').text(alerta.dataExibicao ? formatarDataCompleta(alerta.dataExibicao) : 'Imediata');
+
+        const criador = alerta.nomeCriador || alerta.criadoPor || alerta.usuarioCriador || 'Sistema';
         $('#criadoPor').text(criador);
 
         let tempoNoAr = alerta.tempoNoAr;
-        if (tempoNoAr && tempoNoAr.includes(':')) {
+        if (tempoNoAr && tempoNoAr.includes(':'))
+        {
 
             tempoNoAr = formatarTimeSpan(tempoNoAr);
         }
         $('#tempoNoAr').text(tempoNoAr || 'N/A');
         console.log('⏱️ Tempo no Ar:', tempoNoAr);
 
-        const totalDest =
-            alerta.totalDestinatarios || alerta.totalUsuarios || 0;
+        const totalDest = alerta.totalDestinatarios || alerta.totalUsuarios || 0;
 
         const totalNotif = alerta.totalNotificados || alerta.notificados || 0;
 
-        const aguardando =
-            alerta.aguardandoNotificacao ||
-            alerta.aguardando ||
-            totalDest - totalNotif;
-
-        const leram =
-            alerta.leram || alerta.totalLidos || alerta.usuariosLeram || 0;
-
-        const naoLeram =
-            alerta.naoLeram || alerta.totalNaoLidos || totalNotif - leram;
-
-        const apagaram =
-            alerta.apagaram ||
-            alerta.totalApagados ||
-            alerta.usuariosApagaram ||
-            0;
+        const aguardando = alerta.aguardandoNotificacao || alerta.aguardando ||
+            (totalDest - totalNotif);
+
+        const leram = alerta.leram || alerta.totalLidos || alerta.usuariosLeram || 0;
+
+        const naoLeram = alerta.naoLeram || alerta.totalNaoLidos ||
+            (totalNotif - leram);
+
+        const apagaram = alerta.apagaram || alerta.totalApagados || alerta.usuariosApagaram || 0;
 
         console.log('📊 Estatísticas processadas:', {
             totalDestinatarios: totalDest,
@@ -1531,7 +1564,7 @@
             aguardando: aguardando,
             leram: leram,
             naoLeram: naoLeram,
-            apagaram: apagaram,
+            apagaram: apagaram
         });
 
         $('#totalDestinatariosResumo').text(totalDest);
@@ -1556,95 +1589,101 @@
             .attr('aria-valuenow', percentual);
 
         $('#textoProgressoBarra').text(percentual + '%');
-        $('#infoLeitores').text(
-            `${leram} de ${totalNotif} usuários notificados leram o alerta`,
-        );
-
-        console.log(
-            '👥 Total de usuários recebidos:',
-            (alerta.usuarios || []).length,
-        );
+        $('#infoLeitores').text(`${leram} de ${totalNotif} usuários notificados leram o alerta`);
+
+        console.log('👥 Total de usuários recebidos:', (alerta.usuarios || []).length);
         preencherTabelaUsuarios(alerta.usuarios || []);
 
         preencherLinksRelacionados(alerta);
 
         console.log('✅ Modal preenchido com sucesso');
-    } catch (error) {
+
+    } catch (error)
+    {
         console.error('❌ Erro ao preencher modal:', error);
         console.error('Stack:', error.stack);
-        Alerta.TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'preencherModalDetalhes',
-            error,
-        );
-    }
-}
-
-function formatarTimeSpan(timeSpanString) {
-    try {
+        Alerta.TratamentoErroComLinha("alertas_gestao.js", "preencherModalDetalhes", error);
+    }
+}
+
+function formatarTimeSpan(timeSpanString)
+{
+    try
+    {
 
         const semMilissegundos = timeSpanString.split('.')[0];
 
         const partes = semMilissegundos.split(':');
 
-        if (partes.length === 3) {
+        if (partes.length === 3)
+        {
             const horas = parseInt(partes[0]);
             const minutos = parseInt(partes[1]);
 
-            if (horas === 0) {
+            if (horas === 0)
+            {
                 return `${minutos} min`;
-            } else {
+            } else
+            {
                 return `${horas}h ${minutos}min`;
             }
         }
 
         return timeSpanString;
-    } catch (e) {
+    } catch (e)
+    {
         console.error('Erro ao formatar TimeSpan:', e);
         return timeSpanString;
     }
 }
 
-function preencherLinksRelacionados(alerta) {
+function preencherLinksRelacionados(alerta)
+{
     const container = $('#linksRelacionados');
     container.empty();
 
     const links = [];
 
-    if (alerta.viagemId) {
+    if (alerta.viagemId)
+    {
         links.push({
             texto: 'Ver Viagem',
             url: `/Viagens/Detalhes/${alerta.viagemId}`,
-            icone: 'fa-duotone fa-route',
-        });
-    }
-
-    if (alerta.manutencaoId) {
+            icone: 'fa-duotone fa-route'
+        });
+    }
+
+    if (alerta.manutencaoId)
+    {
         links.push({
             texto: 'Ver Manutenção',
             url: `/Manutencoes/Detalhes/${alerta.manutencaoId}`,
-            icone: 'fa-duotone fa-wrench',
-        });
-    }
-
-    if (alerta.motoristaId) {
+            icone: 'fa-duotone fa-wrench'
+        });
+    }
+
+    if (alerta.motoristaId)
+    {
         links.push({
             texto: 'Ver Motorista',
             url: `/Motoristas/Detalhes/${alerta.motoristaId}`,
-            icone: 'fa-duotone fa-user',
-        });
-    }
-
-    if (alerta.veiculoId) {
+            icone: 'fa-duotone fa-user'
+        });
+    }
+
+    if (alerta.veiculoId)
+    {
         links.push({
             texto: 'Ver Veículo',
             url: `/Veiculos/Detalhes/${alerta.veiculoId}`,
-            icone: 'fa-duotone fa-car',
-        });
-    }
-
-    if (links.length > 0) {
-        links.forEach((link) => {
+            icone: 'fa-duotone fa-car'
+        });
+    }
+
+    if (links.length > 0)
+    {
+        links.forEach(link =>
+        {
             container.append(`
                 <a href="${link.url}" class="btn btn-sm btn-outline-primary me-2 mb-2" target="_blank">
                     <i class="${link.icone} me-1"></i>
@@ -1652,20 +1691,21 @@
                 </a>
             `);
         });
-    } else {
-        container.html(
-            '<small class="text-muted">Nenhum link relacionado</small>',
-        );
-    }
-}
-
-function preencherTabelaUsuarios(usuarios) {
+    } else
+    {
+        container.html('<small class="text-muted">Nenhum link relacionado</small>');
+    }
+}
+
+function preencherTabelaUsuarios(usuarios)
+{
     console.log('📋 Preenchendo tabela com', usuarios.length, 'usuários');
 
     const tbody = $('#tabelaUsuarios');
     tbody.empty();
 
-    if (!usuarios || usuarios.length === 0) {
+    if (!usuarios || usuarios.length === 0)
+    {
         tbody.html(`
             <tr>
                 <td colspan="6" class="text-center text-muted py-4">
@@ -1677,31 +1717,33 @@
         return;
     }
 
-    const usuariosOrdenados = [...usuarios].sort((a, b) => {
+    const usuariosOrdenados = [...usuarios].sort((a, b) =>
+    {
         const nomeA = (a.nomeUsuario || '').toLowerCase();
         const nomeB = (b.nomeUsuario || '').toLowerCase();
         return nomeA.localeCompare(nomeB, 'pt-BR');
     });
 
-    usuariosOrdenados.forEach((usuario) => {
+    usuariosOrdenados.forEach(usuario =>
+    {
 
         const notificadoBadge = usuario.notificado
             ? '<span class="badge bg-success"><i class="fa fa-check"></i> Sim</span>'
             : '<span class="badge bg-warning"><i class="fa fa-clock"></i> Aguardando</span>';
 
         let statusLeitura = '';
-        if (usuario.apagado) {
-            statusLeitura =
-                '<span class="badge bg-secondary"><i class="fa fa-trash"></i> Apagado</span>';
-        } else if (usuario.lido) {
-            statusLeitura =
-                '<span class="badge bg-success"><i class="fa fa-check"></i> Lido</span>';
-        } else if (usuario.notificado) {
-            statusLeitura =
-                '<span class="badge bg-danger"><i class="fa fa-times"></i> Não Lido</span>';
-        } else {
-            statusLeitura =
-                '<span class="badge bg-secondary"><i class="fa fa-minus"></i> N/A</span>';
+        if (usuario.apagado)
+        {
+            statusLeitura = '<span class="badge bg-secondary"><i class="fa fa-trash"></i> Apagado</span>';
+        } else if (usuario.lido)
+        {
+            statusLeitura = '<span class="badge bg-success"><i class="fa fa-check"></i> Lido</span>';
+        } else if (usuario.notificado)
+        {
+            statusLeitura = '<span class="badge bg-danger"><i class="fa fa-times"></i> Não Lido</span>';
+        } else
+        {
+            statusLeitura = '<span class="badge bg-secondary"><i class="fa fa-minus"></i> N/A</span>';
         }
 
         const dataNotificacao = usuario.dataNotificacao
@@ -1713,14 +1755,17 @@
             : '<span class="text-muted">-</span>';
 
         let tempoLeitura = '<span class="text-muted">-</span>';
-        if (usuario.dataNotificacao && usuario.dataLeitura) {
+        if (usuario.dataNotificacao && usuario.dataLeitura)
+        {
             const notif = new Date(usuario.dataNotificacao);
             const leit = new Date(usuario.dataLeitura);
             const diffMinutos = Math.floor((leit - notif) / 1000 / 60);
 
-            if (diffMinutos < 60) {
+            if (diffMinutos < 60)
+            {
                 tempoLeitura = `${diffMinutos} min`;
-            } else {
+            } else
+            {
                 const horas = Math.floor(diffMinutos / 60);
                 const mins = diffMinutos % 60;
                 tempoLeitura = `${horas}h ${mins}min`;
@@ -1745,23 +1790,23 @@
         tbody.append(row);
     });
 
-    console.log(
-        '✅ Tabela preenchida com',
-        usuariosOrdenados.length,
-        'usuários',
-    );
-}
-
-function preencherTimeline(eventos) {
-    try {
+    console.log('✅ Tabela preenchida com', usuariosOrdenados.length, 'usuários');
+}
+
+function preencherTimeline(eventos)
+{
+    try
+    {
         var html = '';
 
-        eventos.forEach(function (evento) {
+        eventos.forEach(function (evento)
+        {
             var classeItem = '';
             var icone = '';
             var cor = '';
 
-            switch (evento.tipo) {
+            switch (evento.tipo)
+            {
                 case 'criado':
                     icone = 'fa-plus-circle';
                     cor = 'text-primary';
@@ -1799,29 +1844,35 @@
         });
 
         $('#conteudoTimeline').html(html);
-    } catch (error) {
-        TratamentoErroComLinha('alertas_gestao.js', 'preencherTimeline', error);
-    }
-}
-
-function obterClasseBadge(tipo) {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "preencherTimeline", error);
+    }
+}
+
+function obterClasseBadge(tipo)
+{
     var mapa = {
-        Agendamento: 'badge-agendamento',
-        Manutenção: 'badge-manutencao',
-        Motorista: 'badge-motorista',
-        Veículo: 'badge-veiculo',
-        Anúncio: 'badge-anuncio',
-        Aniversário: 'badge-aniversario',
+        'Agendamento': 'badge-agendamento',
+        'Manutenção': 'badge-manutencao',
+        'Motorista': 'badge-motorista',
+        'Veículo': 'badge-veiculo',
+        'Anúncio': 'badge-anuncio',
+        'Diversos': 'badge-diversos'
     };
-    return mapa[tipo] || 'badge-aniversario';
-}
-
-function obterClassePrioridade(prioridade) {
-    if (typeof prioridade === 'string') {
+    return mapa[tipo] || 'badge-diversos';
+}
+
+function obterClassePrioridade(prioridade)
+{
+    if (typeof prioridade === 'string')
+    {
         prioridade = prioridade.toLowerCase();
     }
 
-    switch (prioridade) {
+    switch (prioridade)
+    {
         case 'alta':
         case 3:
             return 'prioridade-alta';
@@ -1834,81 +1885,79 @@
     }
 }
 
-function obterCorBorda(tipo) {
-    switch (tipo) {
-        case 1:
-            return 'info';
-        case 2:
-            return 'warning';
-        case 3:
-            return 'success';
-        case 4:
-            return 'primary';
-        case 5:
-            return 'danger';
-        default:
-            return 'secondary';
-    }
-}
-
-function obterTextoPorTipo(tipo) {
+function obterCorBorda(tipo)
+{
+    switch (tipo)
+    {
+        case 1: return 'info';
+        case 2: return 'warning';
+        case 3: return 'success';
+        case 4: return 'primary';
+        case 5: return 'danger';
+        default: return 'secondary';
+    }
+}
+
+function obterTextoPorTipo(tipo)
+{
     var mapa = {
         1: 'Agendamento',
         2: 'Manutenção',
         3: 'Motorista',
         4: 'Veículo',
         5: 'Anúncio',
-        6: 'Aniversário',
+        6: 'Diversos'
     };
-    return mapa[tipo] || 'Aniversário';
-}
-
-function formatarData(dataString) {
-    try {
+    return mapa[tipo] || 'Diversos';
+}
+
+function formatarData(dataString)
+{
+    try
+    {
         if (!dataString) return '';
         var data = new Date(dataString);
-        return (
-            data.toLocaleDateString('pt-BR') +
-            ' ' +
-            data.toLocaleTimeString('pt-BR', {
-                hour: '2-digit',
-                minute: '2-digit',
-            })
-        );
-    } catch (error) {
+        return data.toLocaleDateString('pt-BR') + ' ' + data.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
+    }
+    catch (error)
+    {
         return dataString;
     }
 }
 
-function calcularTempoNoAr(data) {
-    try {
+function calcularTempoNoAr(data)
+{
+    try
+    {
         var inicio = new Date(data.dataInsercao);
-        var fim =
-            !data.ativo && data.dataDesativacao
-                ? new Date(data.dataDesativacao)
-                : new Date();
+        var fim = !data.ativo && data.dataDesativacao ? new Date(data.dataDesativacao) : new Date();
         var diff = fim - inicio;
 
         var dias = Math.floor(diff / (1000 * 60 * 60 * 24));
-        var horas = Math.floor(
-            (diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60),
-        );
+        var horas = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
         var minutos = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
 
-        if (dias > 0) {
+        if (dias > 0)
+        {
             return `${dias}d ${horas}h ${minutos}min`;
-        } else if (horas > 0) {
+        } else if (horas > 0)
+        {
             return `${horas}h ${minutos}min`;
-        } else {
+        } else
+        {
             return `${minutos} minutos`;
         }
-    } catch (error) {
+    }
+    catch (error)
+    {
         return '-';
     }
 }
 
-function calcularDiferenca(dataInicial, dataFinal) {
-    try {
+function calcularDiferenca(dataInicial, dataFinal)
+{
+    try
+    {
         var inicio = new Date(dataInicial);
         var fim = new Date(dataFinal);
         var diff = fim - inicio;
@@ -1916,59 +1965,73 @@
         var horas = Math.floor(minutos / 60);
         var dias = Math.floor(horas / 24);
 
-        if (dias > 0) {
+        if (dias > 0)
+        {
             return `${dias}d ${horas % 24}h`;
-        } else if (horas > 0) {
+        } else if (horas > 0)
+        {
             return `${horas}h ${minutos % 60}min`;
-        } else {
+        } else
+        {
             return `${minutos} min`;
         }
-    } catch (error) {
+    }
+    catch (error)
+    {
         return '-';
     }
 }
 
-function atualizarBadgeAtivos(quantidade) {
+function atualizarBadgeAtivos(quantidade)
+{
     var badge = $('#badgeAtivos');
-    if (badge.length) {
+    if (badge.length)
+    {
         badge.text(quantidade);
-        if (quantidade > 0) {
+        if (quantidade > 0)
+        {
             badge.removeClass('bg-secondary').addClass('bg-danger');
-        } else {
+        } else
+        {
             badge.removeClass('bg-danger').addClass('bg-secondary');
         }
     }
 }
 
-function configurarTooltips() {
-    if (typeof ej !== 'undefined' && ej.popups && ej.popups.Tooltip) {
+function configurarTooltips()
+{
+    if (typeof ej !== 'undefined' && ej.popups && ej.popups.Tooltip)
+    {
         var tooltip = new ej.popups.Tooltip({
-            cssClass: 'tooltip-ftx-azul ftx-tooltip-noarrow',
+            cssClass: 'ftx-tooltip-noarrow',
             position: 'TopCenter',
             animation: {
                 open: { effect: 'FadeIn', duration: 150 },
-                close: { effect: 'FadeOut', duration: 150 },
-            },
+                close: { effect: 'FadeOut', duration: 150 }
+            }
         });
         tooltip.appendTo('body');
     }
 }
 
-function escapeHtml(text) {
+function escapeHtml(text)
+{
     if (!text) return '';
     var div = document.createElement('div');
     div.textContent = text;
     return div.innerHTML;
 }
 
-$(document).on('click', '#btnExportarDetalhes', function () {
-    try {
+$(document).on('click', '#btnExportarDetalhes', function ()
+{
+    try
+    {
         if (!alertaDetalhesAtual) return;
 
         var dadosExport = {
             alertaId: alertaDetalhesAtual.alertaId,
             titulo: alertaDetalhesAtual.titulo,
-            usuarios: alertaDetalhesAtual.usuarios,
+            usuarios: alertaDetalhesAtual.usuarios
         };
 
         $.ajax({
@@ -1977,7 +2040,8 @@
             contentType: 'application/json',
             data: JSON.stringify(dadosExport),
             xhrFields: { responseType: 'blob' },
-            success: function (data) {
+            success: function (data)
+            {
                 var a = document.createElement('a');
                 var url = window.URL.createObjectURL(data);
                 a.href = url;
@@ -1986,105 +2050,105 @@
                 a.click();
                 window.URL.revokeObjectURL(url);
                 document.body.removeChild(a);
-                AppToast.show(
-                    'Verde',
-                    'Relatório exportado com sucesso!',
-                    2000,
-                );
+                AppToast.show("Verde", "Relatório exportado com sucesso!", 2000);
             },
-            error: function (xhr, status, error) {
-                AppToast.show('Vermelho', 'Erro ao exportar relatório', 3000);
-                TratamentoErroComLinha(
-                    'alertas_gestao.js',
-                    'ExportarDetalhes.error',
-                    error,
-                );
-            },
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'btnExportarDetalhes.click',
-            error,
-        );
+            error: function (xhr, status, error)
+            {
+                AppToast.show("Vermelho", "Erro ao exportar relatório", 3000);
+                TratamentoErroComLinha("alertas_gestao.js", "ExportarDetalhes.error", error);
+            }
+        });
+    } catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "btnExportarDetalhes.click", error);
     }
 });
 
-function obterNomeTipo(tipo) {
-    try {
+function obterNomeTipo(tipo)
+{
+    try
+    {
         const tipos = {
             1: 'Viagem',
             2: 'Manutenção',
             3: 'Vencimento',
             4: 'Veículo',
-            5: 'Geral',
+            5: 'Geral'
         };
         return tipos[tipo] || 'Desconhecido';
-    } catch (error) {
-        TratamentoErroComLinha('alertas_gestao.js', 'obterNomeTipo', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "obterNomeTipo", error);
         return 'Desconhecido';
     }
 }
-function obterNomePrioridade(prioridade) {
-    try {
+function obterNomePrioridade(prioridade)
+{
+    try
+    {
         const prioridades = {
             1: 'Baixa',
             2: 'Média',
             3: 'Alta',
-            4: 'Crítica',
+            4: 'Crítica'
         };
         return prioridades[prioridade] || 'Desconhecida';
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'obterNomePrioridade',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "obterNomePrioridade", error);
         return 'Desconhecida';
     }
 }
 
-function obterCorPrioridade(prioridade) {
-    try {
+function obterCorPrioridade(prioridade)
+{
+    try
+    {
         const cores = {
             1: '#17a2b8',
             2: '#ffc107',
             3: '#fd7e14',
-            4: '#dc3545',
+            4: '#dc3545'
         };
         return cores[prioridade] || '#6c757d';
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'obterCorPrioridade',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "obterCorPrioridade", error);
         return '#6c757d';
     }
 }
 
-function obterIconePorTipo(tipo) {
-    try {
+function obterIconePorTipo(tipo)
+{
+    try
+    {
         const icones = {
             1: 'fa-duotone fa-route',
             2: 'fa-duotone fa-wrench',
             3: 'fa-duotone fa-calendar-exclamation',
             4: 'fa-duotone fa-car',
-            5: 'fa-duotone fa-bell',
+            5: 'fa-duotone fa-bell'
         };
         return icones[tipo] || 'fa-duotone fa-bell';
-    } catch (error) {
-        TratamentoErroComLinha('alertas_gestao.js', 'obterIconePorTipo', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "obterIconePorTipo", error);
         return 'fa-duotone fa-bell';
     }
 }
 
-function formatarDataCompleta(dataStr) {
+function formatarDataCompleta(dataStr)
+{
     if (!dataStr) return '-';
 
     const data = new Date(dataStr);
 
-    if (isNaN(data.getTime())) {
+    if (isNaN(data.getTime()))
+    {
         return dataStr;
     }
 
@@ -2093,17 +2157,20 @@
         month: '2-digit',
         year: 'numeric',
         hour: '2-digit',
-        minute: '2-digit',
+        minute: '2-digit'
     };
 
     return data.toLocaleString('pt-BR', opcoes);
 }
 
-function verificarPermissaoBaixaAlerta(alerta) {
-    try {
+function verificarPermissaoBaixaAlerta(alerta)
+{
+    try
+    {
         var btnBaixa = $('#btnBaixaAlerta');
 
-        if (!alerta || !alerta.alertaId) {
+        if (!alerta || !alerta.alertaId)
+        {
             btnBaixa.hide();
             return;
         }
@@ -2111,259 +2178,264 @@
         $.ajax({
             url: `/api/AlertasFrotiX/VerificarPermissaoBaixa/${alerta.alertaId}`,
             type: 'GET',
-            success: function (response) {
-                if (response.podeDarBaixa) {
+            success: function (response)
+            {
+                if (response.podeDarBaixa)
+                {
                     btnBaixa.show();
-                } else {
+                }
+                else
+                {
                     btnBaixa.hide();
                     console.log('Usuário sem permissão:', response.motivo);
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('Erro ao verificar permissão:', error);
                 btnBaixa.hide();
-            },
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'verificarPermissaoBaixaAlerta',
-            error,
-        );
+            }
+        });
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "verificarPermissaoBaixaAlerta", error);
         $('#btnBaixaAlerta').hide();
     }
 }
 
-$(document).on(
-    'click',
-    '[data-dismiss="modal"], [data-bs-dismiss="modal"]',
-    function () {
-        const modalId = $(this).closest('.modal').attr('id');
-        if (modalId) {
-            $(`#${modalId}`).modal('hide');
-
-            setTimeout(() => {
-                $('.modal-backdrop').remove();
-                $('body').removeClass('modal-open');
-                $('body').css('padding-right', '');
-            }, 300);
-        }
-    },
-);
-
-$(document).on('click', '.modal', function (e) {
-    if (e.target === this) {
+$(document).on('click', '[data-dismiss="modal"], [data-bs-dismiss="modal"]', function ()
+{
+    const modalId = $(this).closest('.modal').attr('id');
+    if (modalId)
+    {
+        $(`#${modalId}`).modal('hide');
+
+        setTimeout(() =>
+        {
+            $('.modal-backdrop').remove();
+            $('body').removeClass('modal-open');
+            $('body').css('padding-right', '');
+        }, 300);
+    }
+});
+
+$(document).on('click', '.modal', function (e)
+{
+    if (e.target === this)
+    {
         $(this).modal('hide');
     }
 });
 
-$(document).on('keydown', function (e) {
-    if (e.key === 'Escape' && $('.modal.show').length) {
+$(document).on('keydown', function (e)
+{
+    if (e.key === 'Escape' && $('.modal.show').length)
+    {
         $('.modal.show').modal('hide');
     }
 });
 
-function darBaixaAlertaComConfirmacao(alertaId) {
-    try {
+function darBaixaAlertaComConfirmacao(alertaId)
+{
+    try
+    {
         Alerta.Confirmar(
-            'Tem certeza que deseja dar baixa neste alerta?',
-            'Esta ação não pode ser desfeita. O alerta será marcado como finalizado.',
-            'Sim, dar baixa',
-            'Cancelar',
-        ).then((confirmado) => {
-            try {
-                if (confirmado) {
+            "Tem certeza que deseja dar baixa neste alerta?",
+            "Esta ação não pode ser desfeita. O alerta será marcado como finalizado.",
+            "Sim, dar baixa",
+            "Cancelar"
+        ).then((confirmado) =>
+        {
+            try
+            {
+                if (confirmado)
+                {
                     executarBaixaAlerta(alertaId);
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'alertas_gestao.js',
-                    'darBaixaAlertaComConfirmacao.callback',
-                    error,
-                );
-            }
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'darBaixaAlertaComConfirmacao',
-            error,
-        );
-    }
-}
-
-function executarBaixaAlerta(alertaId) {
-    try {
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("alertas_gestao.js", "darBaixaAlertaComConfirmacao.callback", error);
+            }
+        });
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("alertas_gestao.js", "darBaixaAlertaComConfirmacao", error);
+    }
+}
+
+function executarBaixaAlerta(alertaId)
+{
+    try
+    {
         $.ajax({
             url: `/api/AlertasFrotiX/DarBaixaAlerta/${alertaId}`,
             type: 'POST',
             contentType: 'application/json',
-            success: function (response) {
-                try {
-                    if (response.success) {
-
-                        AppToast.show(
-                            'Verde',
-                            'Baixa do alerta realizada com sucesso',
-                            2000,
-                        );
+            success: function (response)
+            {
+                try
+                {
+                    if (response.success)
+                    {
+
+                        AppToast.show("Verde", "Baixa do alerta realizada com sucesso", 2000);
 
                         $('#modalDetalhesAlerta').modal('hide');
 
-                        setTimeout(function () {
+                        setTimeout(function ()
+                        {
 
                             carregarAlertasGestao();
 
-                            if (tabelaAlertasLidos && tabelaAlertasLidos.ajax) {
+                            if (tabelaAlertasLidos && tabelaAlertasLidos.ajax)
+                            {
                                 tabelaAlertasLidos.ajax.reload(null, false);
                             }
 
                             carregarAlertasAtivos();
+
                         }, 500);
-                    } else {
-                        AppToast.show(
-                            'Amarelo',
-                            response.mensagem ||
-                                'Não foi possível dar baixa no alerta',
-                            3000,
-                        );
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'alertas_gestao.js',
-                        'executarBaixaAlerta.success',
-                        error,
-                    );
+                    else
+                    {
+                        AppToast.show("Amarelo", response.mensagem || "Não foi possível dar baixa no alerta", 3000);
+                    }
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("alertas_gestao.js", "executarBaixaAlerta.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                try {
+            error: function (xhr, status, error)
+            {
+                try
+                {
                     console.error('Erro ao dar baixa no alerta:', error);
 
-                    var mensagemErro = 'Erro ao dar baixa no alerta';
-                    if (xhr.responseJSON && xhr.responseJSON.mensagem) {
+                    var mensagemErro = "Erro ao dar baixa no alerta";
+                    if (xhr.responseJSON && xhr.responseJSON.mensagem)
+                    {
                         mensagemErro = xhr.responseJSON.mensagem;
                     }
 
-                    AppToast.show('Vermelho', mensagemErro, 3000);
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'alertas_gestao.js',
-                        'executarBaixaAlerta.error',
-                        error,
-                    );
+                    AppToast.show("Vermelho", mensagemErro, 3000);
                 }
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'executarBaixaAlerta',
-            error,
-        );
-    }
-}
-
-function darBaixaAlertaComConfirmacao(alertaId) {
-    try {
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("alertas_gestao.js", "executarBaixaAlerta.error", error);
+                }
+            }
+        });
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("alertas_gestao.js", "executarBaixaAlerta", error);
+    }
+}
+
+function darBaixaAlertaComConfirmacao(alertaId)
+{
+    try
+    {
         Alerta.Confirmar(
-            'Tem certeza que deseja dar baixa neste alerta?',
-            'Esta ação não pode ser desfeita. O alerta será marcado como finalizado.',
-            'Sim, dar baixa',
-            'Cancelar',
-        ).then((confirmado) => {
-            try {
-                if (confirmado) {
+            "Tem certeza que deseja dar baixa neste alerta?",
+            "Esta ação não pode ser desfeita. O alerta será marcado como finalizado.",
+            "Sim, dar baixa",
+            "Cancelar"
+        ).then((confirmado) =>
+        {
+            try
+            {
+                if (confirmado)
+                {
                     executarBaixaAlertaAjax(alertaId);
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'alertas_gestao.js',
-                    'darBaixaAlertaComConfirmacao.callback',
-                    error,
-                );
-            }
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'darBaixaAlertaComConfirmacao',
-            error,
-        );
-    }
-}
-
-function executarBaixaAlertaAjax(alertaId) {
-    try {
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("alertas_gestao.js", "darBaixaAlertaComConfirmacao.callback", error);
+            }
+        });
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("alertas_gestao.js", "darBaixaAlertaComConfirmacao", error);
+    }
+}
+
+function executarBaixaAlertaAjax(alertaId)
+{
+    try
+    {
         $.ajax({
             url: `/api/AlertasFrotiX/DarBaixaAlerta/${alertaId}`,
             type: 'POST',
             contentType: 'application/json',
-            success: function (response) {
-                try {
-                    if (response.success) {
-                        AppToast.show(
-                            'Verde',
-                            'Baixa do alerta realizada com sucesso',
-                            2000,
-                        );
+            success: function (response)
+            {
+                try
+                {
+                    if (response.success)
+                    {
+                        AppToast.show("Verde", "Baixa do alerta realizada com sucesso", 2000);
                         $('#modalDetalhesAlerta').modal('hide');
 
-                        setTimeout(function () {
+                        setTimeout(function ()
+                        {
                             carregarAlertasGestao();
 
-                            if (tabelaAlertasLidos && tabelaAlertasLidos.ajax) {
+                            if (tabelaAlertasLidos && tabelaAlertasLidos.ajax)
+                            {
                                 tabelaAlertasLidos.ajax.reload(null, false);
                             }
 
                             carregarAlertasAtivos();
                         }, 500);
-                    } else {
-                        AppToast.show(
-                            'Amarelo',
-                            response.mensagem ||
-                                'Não foi possível dar baixa no alerta',
-                            3000,
-                        );
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'alertas_gestao.js',
-                        'executarBaixaAlertaAjax.success',
-                        error,
-                    );
+                    else
+                    {
+                        AppToast.show("Amarelo", response.mensagem || "Não foi possível dar baixa no alerta", 3000);
+                    }
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("alertas_gestao.js", "executarBaixaAlertaAjax.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                try {
+            error: function (xhr, status, error)
+            {
+                try
+                {
                     console.error('Erro ao dar baixa no alerta:', error);
 
-                    var mensagemErro = 'Erro ao dar baixa no alerta';
-                    if (xhr.responseJSON && xhr.responseJSON.mensagem) {
+                    var mensagemErro = "Erro ao dar baixa no alerta";
+                    if (xhr.responseJSON && xhr.responseJSON.mensagem)
+                    {
                         mensagemErro = xhr.responseJSON.mensagem;
                     }
 
-                    AppToast.show('Vermelho', mensagemErro, 3000);
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'alertas_gestao.js',
-                        'executarBaixaAlertaAjax.error',
-                        error,
-                    );
+                    AppToast.show("Vermelho", mensagemErro, 3000);
                 }
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'executarBaixaAlertaAjax',
-            error,
-        );
-    }
-}
-
-async function verDetalhesAlertaInativo(alertaId) {
-    try {
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("alertas_gestao.js", "executarBaixaAlertaAjax.error", error);
+                }
+            }
+        });
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("alertas_gestao.js", "executarBaixaAlertaAjax", error);
+    }
+}
+
+async function verDetalhesAlertaInativo(alertaId)
+{
+    try
+    {
         console.log('Carregando detalhes do alerta inativo:', alertaId);
 
         $('#modalDetalhesAlerta').modal('show');
@@ -2372,17 +2444,17 @@
 
         $('#btnBaixaAlerta').hide();
 
-        const response = await fetch(
-            `/api/AlertasFrotiX/GetDetalhesAlerta/${alertaId}`,
-        );
-
-        if (!response.ok) {
+        const response = await fetch(`/api/AlertasFrotiX/GetDetalhesAlerta/${alertaId}`);
+
+        if (!response.ok)
+        {
             throw new Error(`Erro HTTP ${response.status}`);
         }
 
         const resultado = await response.json();
 
-        if (!resultado.success) {
+        if (!resultado.success)
+        {
             throw new Error(resultado.message || 'Erro ao carregar detalhes');
         }
 
@@ -2391,183 +2463,191 @@
 
         $('#loaderDetalhes').hide();
         $('#conteudoDetalhes').show();
-    } catch (erro) {
+
+    } catch (erro)
+    {
         console.error('Erro ao carregar detalhes:', erro);
 
         $('#loaderDetalhes').hide();
-        $('#conteudoDetalhes')
-            .html(
-                `
+        $('#conteudoDetalhes').html(`
             <div class="alert alert-danger">
                 <i class="fas fa-exclamation-triangle"></i>
                 Erro ao carregar detalhes do alerta: ${erro.message}
             </div>
-        `,
-            )
-            .show();
+        `).show();
 
         await Alerta.TratamentoErroComLinha(
             'alertas_gestao.js',
             'verDetalhesAlertaInativo',
-            erro,
+            erro
         );
     }
 }
 
 var tabelaMeusAlertas;
 
-function inicializarDataTableMeusAlertas() {
-    try {
+function inicializarDataTableMeusAlertas()
+{
+    try
+    {
         if ($('#tblMeusAlertas').length === 0) return;
 
         tabelaMeusAlertas = $('#tblMeusAlertas').DataTable({
-            processing: true,
-            serverSide: false,
-            ajax: {
-                url: '/api/AlertasFrotiX/GetMeusAlertas',
-                type: 'GET',
-                datatype: 'json',
-                dataSrc: function (json) {
-                    if (json && json.data) {
+            "processing": true,
+            "serverSide": false,
+            "ajax": {
+                "url": "/api/AlertasFrotiX/GetMeusAlertas",
+                "type": "GET",
+                "datatype": "json",
+                "dataSrc": function (json)
+                {
+                    if (json && json.data)
+                    {
                         atualizarBadgeMeusAlertas(json.data.length);
                         return json.data;
                     }
                     atualizarBadgeMeusAlertas(0);
                     return [];
                 },
-                error: function (xhr, error, code) {
-                    TratamentoErroComLinha(
-                        'alertas_gestao.js',
-                        'ajax.getMeusAlertas.error',
-                        error,
-                    );
+                "error": function (xhr, error, code)
+                {
+                    TratamentoErroComLinha("alertas_gestao.js", "ajax.getMeusAlertas.error", error);
                     atualizarBadgeMeusAlertas(0);
-                },
+                }
             },
-            columns: [
-                {
-                    data: 'dataCriacao',
-                    width: '10%',
-                    className: 'text-center',
-                },
-                {
-                    data: 'icone',
-                    render: function (data, type, row) {
+            "columns": [
+                {
+                    "data": "icone",
+                    "render": function (data, type, row)
+                    {
                         var corIcone = obterCorPorTipo(row.tipo);
                         return `<i class="${data} fa-lg" style="color: ${corIcone}"></i>`;
                     },
-                    className: 'text-center',
-                    width: '3%',
+                    "className": "text-center",
+                    "width": "3%"
                 },
                 {
-                    data: 'titulo',
-                    width: '15%',
+                    "data": "titulo",
+                    "width": "15%"
                 },
                 {
-                    data: 'descricao',
-                    render: function (data, type, row) {
-                        if (data.length > 100) {
+                    "data": "descricao",
+                    "render": function (data, type, row)
+                    {
+                        if (data.length > 100)
+                        {
                             return data.substring(0, 100) + '...';
                         }
                         return data;
                     },
-                    width: '25%',
+                    "width": "30%"
                 },
                 {
-                    data: 'tipo',
-                    render: function (data) {
+                    "data": "tipo",
+                    "render": function (data)
+                    {
                         var badgeClass = obterClasseBadge(data);
                         return `<span class="badge badge-tipo ${badgeClass}">${data}</span>`;
                     },
-                    className: 'text-center',
-                    width: '7%',
+                    "className": "text-center",
+                    "width": "8%"
                 },
                 {
-                    data: 'notificado',
-                    render: function (data, type, row) {
-                        if (data) {
+                    "data": "notificado",
+                    "render": function (data, type, row)
+                    {
+                        if (data)
+                        {
                             return '<span class="badge bg-success"><i class="fa fa-check"></i> Sim</span>';
-                        } else {
+                        } else
+                        {
                             return '<span class="badge bg-warning"><i class="fa fa-clock"></i> Não</span>';
                         }
                     },
-                    className: 'text-center',
-                    width: '7%',
+                    "className": "text-center",
+                    "width": "8%"
                 },
                 {
-                    data: 'dataNotificacao',
-                    className: 'text-center',
-                    width: '10%',
+                    "data": "dataNotificacao",
+                    "className": "text-center",
+                    "width": "10%"
                 },
                 {
-                    data: 'lido',
-                    render: function (data, type, row) {
-                        if (data) {
+                    "data": "lido",
+                    "render": function (data, type, row)
+                    {
+                        if (data)
+                        {
                             return '<span class="badge bg-success"><i class="fa fa-check-circle"></i> Sim</span>';
-                        } else {
+                        } else
+                        {
                             return '<span class="badge bg-danger"><i class="fa fa-times-circle"></i> Não</span>';
                         }
                     },
-                    className: 'text-center',
-                    width: '7%',
+                    "className": "text-center",
+                    "width": "8%"
                 },
                 {
-                    data: 'dataLeitura',
-                    className: 'text-center',
-                    width: '10%',
+                    "data": "dataLeitura",
+                    "className": "text-center",
+                    "width": "10%"
                 },
                 {
-                    data: 'alertaId',
-                    render: function (data, type, row) {
+                    "data": "alertaId",
+                    "render": function (data, type, row)
+                    {
                         return `
-                            <button class="btn btn-eye-frotix btn-sm"
+                            <button class="btn btn-info btn-sm"
                                     onclick="verDetalhesAlerta('${data}')"
                                     title="Ver Detalhes"
                                     data-ejtip="Ver detalhes completos do alerta">
                                 <i class="fa-duotone fa-eye"></i>
                             </button>`;
                     },
-                    className: 'text-center',
-                    width: '6%',
-                },
+                    "className": "text-center",
+                    "orderable": false,
+                    "width": "8%"
+                }
             ],
-            columnDefs: [{ orderable: false, targets: '_all' }],
-            order: [[0, 'desc']],
-            language: {
-                url: '
+            "order": [[1, "desc"]],
+            "language": {
+                "url": "
             },
-            pageLength: 25,
-            lengthMenu: [
-                [10, 25, 50, 100],
-                [10, 25, 50, 100],
-            ],
-            responsive: true,
-            dom: '<"top"lf>rt<"bottom"ip><"clear">',
-            initComplete: function () {
+            "pageLength": 25,
+            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
+            "responsive": true,
+            "dom": '<"top"lf>rt<"bottom"ip><"clear">',
+            "initComplete": function ()
+            {
                 configurarTooltips();
-            },
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_gestao.js',
-            'inicializarDataTableMeusAlertas',
-            error,
-        );
-    }
-}
-
-function atualizarBadgeMeusAlertas(quantidade) {
-    try {
+            }
+        });
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_gestao.js", "inicializarDataTableMeusAlertas", error);
+    }
+}
+
+function atualizarBadgeMeusAlertas(quantidade)
+{
+    try
+    {
         var badge = $('#badgeMeusAlertas');
-        if (badge.length) {
+        if (badge.length)
+        {
             badge.text(quantidade);
-            if (quantidade > 0) {
+            if (quantidade > 0)
+            {
                 badge.removeClass('bg-secondary').addClass('bg-primary');
-            } else {
+            } else
+            {
                 badge.removeClass('bg-primary').addClass('bg-secondary');
             }
         }
-    } catch (error) {
+    }
+    catch (error)
+    {
         console.error('Erro ao atualizar badge de Meus Alertas:', error);
     }
 }
```
