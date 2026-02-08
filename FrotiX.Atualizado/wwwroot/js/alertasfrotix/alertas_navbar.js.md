# wwwroot/js/alertasfrotix/alertas_navbar.js

**Mudanca:** GRANDE | **+448** linhas | **-458** linhas

---

```diff
--- JANEIRO: wwwroot/js/alertasfrotix/alertas_navbar.js
+++ ATUAL: wwwroot/js/alertasfrotix/alertas_navbar.js
@@ -1,230 +1,227 @@
 var connectionAlertasNavbar;
 var alertasNaoLidos = [];
 
-$(document).ready(function () {
-    try {
-        console.log('✅ Inicializando alertas_navbar.js...');
+$(document).ready(function ()
+{
+    try
+    {
+        console.log("✅ Inicializando alertas_navbar.js...");
         inicializarAlertasNavbar();
         inicializarSignalRNavbar();
-    } catch (error) {
-        TratamentoErroComLinha('alertas_navbar.js', 'document.ready', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "document.ready", error);
     }
 });
 
-function inicializarAlertasNavbar() {
-    try {
+function inicializarAlertasNavbar()
+{
+    try
+    {
 
         carregarAlertasNaoLidos();
 
-        $('#btnNotificacoes, #iconeSino').on('click', function (e) {
-            try {
+        $('#btnNotificacoes, #iconeSino').on('click', function (e)
+        {
+            try
+            {
                 e.preventDefault();
                 e.stopPropagation();
                 toggleDropdownAlertas();
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'alertas_navbar.js',
-                    'btnNotificacoes.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', function (e) {
-            try {
-                if (
-                    !$(e.target).closest(
-                        '#dropdownAlertas, #btnNotificacoes, #iconeSino',
-                    ).length
-                ) {
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("alertas_navbar.js", "btnNotificacoes.click", error);
+            }
+        });
+
+        $(document).on('click', function (e)
+        {
+            try
+            {
+                if (!$(e.target).closest('#dropdownAlertas, #btnNotificacoes, #iconeSino').length)
+                {
                     fecharDropdownAlertas();
                 }
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'alertas_navbar.js',
-                    'document.click',
-                    error,
-                );
-            }
-        });
-
-        $('#dropdownAlertas').on('click', function (e) {
-            try {
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("alertas_navbar.js", "document.click", error);
+            }
+        });
+
+        $('#dropdownAlertas').on('click', function (e)
+        {
+            try
+            {
                 e.stopPropagation();
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'alertas_navbar.js',
-                    'dropdownAlertas.click',
-                    error,
-                );
-            }
-        });
-
-        console.log('✅ Alertas navbar inicializado');
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'inicializarAlertasNavbar',
-            error,
-        );
-    }
-}
-
-function inicializarSignalRNavbar() {
-    try {
-        console.log('🔧 Configurando SignalR para Navbar...');
-
-        if (typeof SignalRManager === 'undefined') {
-            console.error('❌ SignalRManager não está carregado!');
-            console.error(
-                'Certifique-se de que signalr_manager.js está carregado ANTES de alertas_navbar.js',
-            );
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("alertas_navbar.js", "dropdownAlertas.click", error);
+            }
+        });
+
+        console.log("✅ Alertas navbar inicializado");
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "inicializarAlertasNavbar", error);
+    }
+}
+
+function inicializarSignalRNavbar()
+{
+    try
+    {
+        console.log("🔧 Configurando SignalR para Navbar...");
+
+        if (typeof SignalRManager === 'undefined')
+        {
+            console.error("❌ SignalRManager não está carregado!");
+            console.error("Certifique-se de que signalr_manager.js está carregado ANTES de alertas_navbar.js");
             return;
         }
 
         SignalRManager.getConnection()
-            .then(function (conn) {
-                try {
+            .then(function (conn)
+            {
+                try
+                {
                     connectionAlertasNavbar = conn;
-                    console.log('✅ Conexão SignalR obtida para Navbar');
+                    console.log("✅ Conexão SignalR obtida para Navbar");
 
                     configurarEventHandlersSignalR();
 
                     SignalRManager.registerCallback({
-                        onReconnected: function (connectionId) {
-                            try {
-                                console.log(
-                                    '🔄 Navbar: SignalR reconectado, recarregando alertas...',
-                                );
+                        onReconnected: function (connectionId)
+                        {
+                            try
+                            {
+                                console.log("🔄 Navbar: SignalR reconectado, recarregando alertas...");
                                 carregarAlertasNaoLidos();
-                            } catch (error) {
-                                TratamentoErroComLinha(
-                                    'alertas_navbar.js',
-                                    'callback.onReconnected',
-                                    error,
-                                );
+                            }
+                            catch (error)
+                            {
+                                TratamentoErroComLinha("alertas_navbar.js", "callback.onReconnected", error);
                             }
                         },
-                        onReconnecting: function (error) {
-                            console.log('🔄 Navbar: SignalR reconectando...');
+                        onReconnecting: function (error)
+                        {
+                            console.log("🔄 Navbar: SignalR reconectando...");
                         },
-                        onClose: function (error) {
-                            console.log('❌ Navbar: Conexão SignalR fechada');
-                        },
+                        onClose: function (error)
+                        {
+                            console.log("❌ Navbar: Conexão SignalR fechada");
+                        }
                     });
 
-                    console.log(
-                        '✅ SignalR configurado com sucesso para Navbar',
-                    );
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        'alertas_navbar.js',
-                        'getConnection.then',
-                        error,
-                    );
+                    console.log("✅ SignalR configurado com sucesso para Navbar");
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha("alertas_navbar.js", "getConnection.then", error);
                 }
             })
-            .catch(function (err) {
-                try {
-                    console.error(
-                        '❌ Erro ao obter conexão SignalR para Navbar:',
-                        err,
-                    );
+            .catch(function (err)
+            {
+                try
+                {
+                    console.error("❌ Erro ao obter conexão SignalR para Navbar:", err);
 
                     setTimeout(inicializarSignalRNavbar, 5000);
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        'alertas_navbar.js',
-                        'getConnection.catch',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha("alertas_navbar.js", "getConnection.catch", error);
                 }
             });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'inicializarSignalRNavbar',
-            error,
-        );
-    }
-}
-
-function configurarEventHandlersSignalR() {
-    try {
-
-        SignalRManager.on('NovoAlerta', function (alerta) {
-            try {
-                console.log('📬 Novo alerta recebido no navbar:', alerta);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "inicializarSignalRNavbar", error);
+    }
+}
+
+function configurarEventHandlersSignalR()
+{
+    try
+    {
+
+        SignalRManager.on("NovoAlerta", function (alerta)
+        {
+            try
+            {
+                console.log("📬 Novo alerta recebido no navbar:", alerta);
 
                 alertasNaoLidos.unshift(alerta);
 
                 atualizarBadgeNavbar(alertasNaoLidos.length);
 
-                if ($('#dropdownAlertas').is(':visible')) {
+                if ($('#dropdownAlertas').is(':visible'))
+                {
                     renderizarDropdownAlertas();
                 }
 
-                if (typeof AppToast !== 'undefined') {
-                    AppToast.show(
-                        'Amarelo',
-                        'Novo alerta: ' + alerta.titulo,
-                        3000,
-                    );
+                if (typeof AppToast !== 'undefined')
+                {
+                    AppToast.show("Amarelo", "Novo alerta: " + alerta.titulo, 3000);
                 }
 
                 mostrarNotificacaoNavegador(alerta);
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'alertas_navbar.js',
-                    'SignalR.NovoAlerta',
-                    error,
-                );
-            }
-        });
-
-        SignalRManager.on('AtualizarBadgeAlertas', function (quantidade) {
-            try {
-                console.log('🔢 Atualizar badge navbar:', quantidade);
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("alertas_navbar.js", "SignalR.NovoAlerta", error);
+            }
+        });
+
+        SignalRManager.on("AtualizarBadgeAlertas", function (quantidade)
+        {
+            try
+            {
+                console.log("🔢 Atualizar badge navbar:", quantidade);
                 atualizarBadgeNavbar(quantidade);
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'alertas_navbar.js',
-                    'SignalR.AtualizarBadgeAlertas',
-                    error,
-                );
-            }
-        });
-
-        console.log('✅ Event handlers configurados para Navbar');
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'configurarEventHandlersSignalR',
-            error,
-        );
-    }
-}
-
-function toggleDropdownAlertas() {
-    try {
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("alertas_navbar.js", "SignalR.AtualizarBadgeAlertas", error);
+            }
+        });
+
+        console.log("✅ Event handlers configurados para Navbar");
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "configurarEventHandlersSignalR", error);
+    }
+}
+
+function toggleDropdownAlertas()
+{
+    try
+    {
         var dropdown = $('#dropdownAlertas');
 
-        if (dropdown.is(':visible')) {
+        if (dropdown.is(':visible'))
+        {
             fecharDropdownAlertas();
-        } else {
+        } else
+        {
             abrirDropdownAlertas();
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'toggleDropdownAlertas',
-            error,
-        );
-    }
-}
-
-function abrirDropdownAlertas() {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "toggleDropdownAlertas", error);
+    }
+}
+
+function abrirDropdownAlertas()
+{
+    try
+    {
         var dropdown = $('#dropdownAlertas');
 
         $('.dropdown-menu').not(dropdown).hide();
@@ -232,80 +229,77 @@
         dropdown.fadeIn(200);
 
         carregarAlertasNaoLidos();
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'abrirDropdownAlertas',
-            error,
-        );
-    }
-}
-
-function fecharDropdownAlertas() {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "abrirDropdownAlertas", error);
+    }
+}
+
+function fecharDropdownAlertas()
+{
+    try
+    {
         $('#dropdownAlertas').fadeOut(200);
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'fecharDropdownAlertas',
-            error,
-        );
-    }
-}
-
-function carregarAlertasNaoLidos() {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "fecharDropdownAlertas", error);
+    }
+}
+
+function carregarAlertasNaoLidos()
+{
+    try
+    {
         $.ajax({
             url: '/api/AlertasFrotiX/GetAlertasAtivos',
             type: 'GET',
             dataType: 'json',
-            success: function (response) {
-                try {
-                    if (response && response.sucesso) {
+            success: function (response)
+            {
+                try
+                {
+                    if (response && response.sucesso)
+                    {
                         alertasNaoLidos = response.dados || [];
                         atualizarBadgeNavbar(alertasNaoLidos.length);
                         renderizarDropdownAlertas();
                     }
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        'alertas_navbar.js',
-                        'carregarAlertasNaoLidos.success',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha("alertas_navbar.js", "carregarAlertasNaoLidos.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                TratamentoErroComLinha(
-                    'alertas_navbar.js',
-                    'carregarAlertasNaoLidos.error',
-                    error,
-                );
-            },
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'carregarAlertasNaoLidos',
-            error,
-        );
-    }
-}
-
-function renderizarDropdownAlertas() {
-    try {
+            error: function (xhr, status, error)
+            {
+                TratamentoErroComLinha("alertas_navbar.js", "carregarAlertasNaoLidos.error", error);
+            }
+        });
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "carregarAlertasNaoLidos", error);
+    }
+}
+
+function renderizarDropdownAlertas()
+{
+    try
+    {
         var container = $('#listaAlertasNavbar');
 
-        if (container.length === 0) {
-            try {
-                console.warn(
-                    '⚠️ Container #listaAlertasNavbar não encontrado, criando...',
-                );
+        if (container.length === 0)
+        {
+            try
+            {
+                console.warn('⚠️ Container #listaAlertasNavbar não encontrado, criando...');
 
                 var dropdown = $('#dropdownAlertas');
-                if (dropdown.length === 0) {
-                    console.error(
-                        '❌ Dropdown #dropdownAlertas também não existe!',
-                    );
+                if (dropdown.length === 0)
+                {
+                    console.error('❌ Dropdown #dropdownAlertas também não existe!');
                     return;
                 }
 
@@ -323,30 +317,29 @@
 
                 container = $('#listaAlertasNavbar');
 
-                $('#btnMarcarTodosLidosNavbar').on('click', function () {
-                    try {
+                $('#btnMarcarTodosLidosNavbar').on('click', function ()
+                {
+                    try
+                    {
                         marcarTodosComoLidosNavbar();
-                    } catch (error) {
-                        TratamentoErroComLinha(
-                            'alertas_navbar.js',
-                            'btnMarcarTodosLidosNavbar.click',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        TratamentoErroComLinha("alertas_navbar.js", "btnMarcarTodosLidosNavbar.click", error);
                     }
                 });
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'alertas_navbar.js',
-                    'renderizarDropdownAlertas.criarContainer',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("alertas_navbar.js", "renderizarDropdownAlertas.criarContainer", error);
                 return;
             }
         }
 
         container.empty();
 
-        if (alertasNaoLidos.length === 0) {
+        if (alertasNaoLidos.length === 0)
+        {
             container.html(`
                 <div class="p-4 text-center text-muted">
                     <i class="fal fa-check-circle fa-2x mb-2"></i>
@@ -356,8 +349,10 @@
             return;
         }
 
-        alertasNaoLidos.forEach(function (alerta) {
-            try {
+        alertasNaoLidos.forEach(function (alerta)
+        {
+            try
+            {
                 var alertaHtml = `
                     <div class="alerta-item p-3 border-bottom hover-bg-light" data-alerta-id="${alerta.alertaId}">
                         <div class="d-flex">
@@ -384,308 +379,292 @@
                     </div>
                 `;
                 container.append(alertaHtml);
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'alertas_navbar.js',
-                    'renderizarDropdownAlertas.forEach',
-                    error,
-                );
-            }
-        });
-
-        console.log(
-            '✅ Dropdown renderizado com',
-            alertasNaoLidos.length,
-            'alertas',
-        );
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'renderizarDropdownAlertas',
-            error,
-        );
-    }
-}
-
-function marcarComoLidoNavbar(alertaId) {
-    try {
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("alertas_navbar.js", "renderizarDropdownAlertas.forEach", error);
+            }
+        });
+
+        console.log("✅ Dropdown renderizado com", alertasNaoLidos.length, "alertas");
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "renderizarDropdownAlertas", error);
+    }
+}
+
+function marcarComoLidoNavbar(alertaId)
+{
+    try
+    {
         $.ajax({
             url: '/api/AlertasFrotiX/MarcarComoLido',
             type: 'POST',
             contentType: 'application/json',
             data: JSON.stringify({ alertaId: alertaId }),
-            success: function (response) {
-                try {
-                    if (response && response.sucesso) {
-
-                        $(
-                            '.alerta-item[data-alerta-id="' + alertaId + '"]',
-                        ).fadeOut(300, function () {
-                            try {
+            success: function (response)
+            {
+                try
+                {
+                    if (response && response.sucesso)
+                    {
+
+                        $('.alerta-item[data-alerta-id="' + alertaId + '"]').fadeOut(300, function ()
+                        {
+                            try
+                            {
                                 $(this).remove();
 
-                                alertasNaoLidos = alertasNaoLidos.filter(
-                                    function (a) {
-                                        return a.alertaId !== alertaId;
-                                    },
-                                );
+                                alertasNaoLidos = alertasNaoLidos.filter(function (a)
+                                {
+                                    return a.alertaId !== alertaId;
+                                });
 
                                 atualizarBadgeNavbar(alertasNaoLidos.length);
 
-                                if (alertasNaoLidos.length === 0) {
+                                if (alertasNaoLidos.length === 0)
+                                {
                                     renderizarDropdownAlertas();
                                 }
-                            } catch (error) {
-                                TratamentoErroComLinha(
-                                    'alertas_navbar.js',
-                                    'marcarComoLidoNavbar.fadeOut',
-                                    error,
-                                );
+                            }
+                            catch (error)
+                            {
+                                TratamentoErroComLinha("alertas_navbar.js", "marcarComoLidoNavbar.fadeOut", error);
                             }
                         });
 
-                        if (typeof AppToast !== 'undefined') {
-                            AppToast.show(
-                                'Verde',
-                                'Alerta marcado como lido',
-                                2000,
-                            );
+                        if (typeof AppToast !== 'undefined')
+                        {
+                            AppToast.show("Verde", "Alerta marcado como lido", 2000);
                         }
                     }
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        'alertas_navbar.js',
-                        'marcarComoLidoNavbar.success',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha("alertas_navbar.js", "marcarComoLidoNavbar.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                TratamentoErroComLinha(
-                    'alertas_navbar.js',
-                    'marcarComoLidoNavbar.error',
-                    error,
-                );
-
-                if (typeof AppToast !== 'undefined') {
-                    AppToast.show(
-                        'Vermelho',
-                        'Erro ao marcar alerta como lido',
-                        2000,
-                    );
-                }
-            },
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'marcarComoLidoNavbar',
-            error,
-        );
-    }
-}
-
-function marcarTodosComoLidosNavbar() {
-    try {
-        if (alertasNaoLidos.length === 0) {
-            if (typeof AppToast !== 'undefined') {
-                AppToast.show(
-                    'Amarelo',
-                    'Não há alertas para marcar como lidos',
-                    2000,
-                );
+            error: function (xhr, status, error)
+            {
+                TratamentoErroComLinha("alertas_navbar.js", "marcarComoLidoNavbar.error", error);
+
+                if (typeof AppToast !== 'undefined')
+                {
+                    AppToast.show("Vermelho", "Erro ao marcar alerta como lido", 2000);
+                }
+            }
+        });
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "marcarComoLidoNavbar", error);
+    }
+}
+
+function marcarTodosComoLidosNavbar()
+{
+    try
+    {
+        if (alertasNaoLidos.length === 0)
+        {
+            if (typeof AppToast !== 'undefined')
+            {
+                AppToast.show("Amarelo", "Não há alertas para marcar como lidos", 2000);
             }
             return;
         }
 
-        if (
-            typeof Alerta !== 'undefined' &&
-            typeof Alerta.Confirmar === 'function'
-        ) {
+        if (typeof Alerta !== 'undefined' && typeof Alerta.Confirmar === 'function')
+        {
             Alerta.Confirmar(
-                'Confirmar Ação',
-                'Deseja marcar todos os ' +
-                    alertasNaoLidos.length +
-                    ' alertas como lidos?',
-                'Sim, marcar todos',
-                'Cancelar',
-            ).then(function (confirmed) {
-                try {
-                    if (confirmed) {
+                "Confirmar Ação",
+                "Deseja marcar todos os " + alertasNaoLidos.length + " alertas como lidos?",
+                "Sim, marcar todos",
+                "Cancelar"
+            ).then(function (confirmed)
+            {
+                try
+                {
+                    if (confirmed)
+                    {
                         executarMarcarTodosComoLidos();
                     }
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        'alertas_navbar.js',
-                        'marcarTodosComoLidosNavbar.confirmar',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha("alertas_navbar.js", "marcarTodosComoLidosNavbar.confirmar", error);
                 }
             });
-        } else {
+        } else
+        {
 
             executarMarcarTodosComoLidos();
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'marcarTodosComoLidosNavbar',
-            error,
-        );
-    }
-}
-
-function executarMarcarTodosComoLidos() {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "marcarTodosComoLidosNavbar", error);
+    }
+}
+
+function executarMarcarTodosComoLidos()
+{
+    try
+    {
         $.ajax({
             url: '/api/AlertasFrotiX/MarcarTodosComoLidos',
             type: 'POST',
             contentType: 'application/json',
-            success: function (response) {
-                try {
-                    if (response && response.sucesso) {
+            success: function (response)
+            {
+                try
+                {
+                    if (response && response.sucesso)
+                    {
                         alertasNaoLidos = [];
                         atualizarBadgeNavbar(0);
                         renderizarDropdownAlertas();
 
-                        if (typeof AppToast !== 'undefined') {
-                            AppToast.show(
-                                'Verde',
-                                'Todos os alertas foram marcados como lidos',
-                                2000,
-                            );
+                        if (typeof AppToast !== 'undefined')
+                        {
+                            AppToast.show("Verde", "Todos os alertas foram marcados como lidos", 2000);
                         }
                     }
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        'alertas_navbar.js',
-                        'executarMarcarTodosComoLidos.success',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha("alertas_navbar.js", "executarMarcarTodosComoLidos.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                TratamentoErroComLinha(
-                    'alertas_navbar.js',
-                    'executarMarcarTodosComoLidos.error',
-                    error,
-                );
-
-                if (typeof AppToast !== 'undefined') {
-                    AppToast.show(
-                        'Vermelho',
-                        'Erro ao marcar todos os alertas como lidos',
-                        2000,
-                    );
-                }
-            },
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'executarMarcarTodosComoLidos',
-            error,
-        );
-    }
-}
-
-function atualizarBadgeNavbar(total) {
+            error: function (xhr, status, error)
+            {
+                TratamentoErroComLinha("alertas_navbar.js", "executarMarcarTodosComoLidos.error", error);
+
+                if (typeof AppToast !== 'undefined')
+                {
+                    AppToast.show("Vermelho", "Erro ao marcar todos os alertas como lidos", 2000);
+                }
+            }
+        });
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "executarMarcarTodosComoLidos", error);
+    }
+}
+
+function atualizarBadgeNavbar(total)
+{
     const badge = document.getElementById('badgeAlertasSino');
 
-    if (!badge) {
+    if (!badge)
+    {
         console.warn('⚠️ Badge #badgeAlertasSino não encontrado');
         return;
     }
 
-    if (total > 0) {
+    if (total > 0)
+    {
         badge.textContent = total;
         badge.style.display = 'block';
-    } else {
+    } else
+    {
         badge.style.display = 'none';
     }
 }
-function mostrarNotificacaoNavegador(alerta) {
-    try {
-
-        if (!('Notification' in window)) {
+function mostrarNotificacaoNavegador(alerta)
+{
+    try
+    {
+
+        if (!("Notification" in window))
+        {
             return;
         }
 
-        if (Notification.permission === 'granted') {
+        if (Notification.permission === "granted")
+        {
             criarNotificacao(alerta);
-        } else if (Notification.permission !== 'denied') {
-            Notification.requestPermission().then(function (permission) {
-                try {
-                    if (permission === 'granted') {
+        } else if (Notification.permission !== "denied")
+        {
+            Notification.requestPermission().then(function (permission)
+            {
+                try
+                {
+                    if (permission === "granted")
+                    {
                         criarNotificacao(alerta);
                     }
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        'alertas_navbar.js',
-                        'mostrarNotificacaoNavegador.requestPermission',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha("alertas_navbar.js", "mostrarNotificacaoNavegador.requestPermission", error);
                 }
             });
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'mostrarNotificacaoNavegador',
-            error,
-        );
-    }
-}
-
-function criarNotificacao(alerta) {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "mostrarNotificacaoNavegador", error);
+    }
+}
+
+function criarNotificacao(alerta)
+{
+    try
+    {
         var notification = new Notification(alerta.titulo, {
             body: alerta.mensagem,
             icon: '/img/logo-small.png',
-            badge: '/img/badge.png',
-        });
-
-        notification.onclick = function () {
-            try {
+            badge: '/img/badge.png'
+        });
+
+        notification.onclick = function ()
+        {
+            try
+            {
                 window.focus();
                 abrirDropdownAlertas();
                 notification.close();
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'alertas_navbar.js',
-                    'notification.onclick',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("alertas_navbar.js", "notification.onclick", error);
             }
         };
-    } catch (error) {
-        TratamentoErroComLinha('alertas_navbar.js', 'criarNotificacao', error);
-    }
-}
-
-function obterClasseSeveridade(severidade) {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "criarNotificacao", error);
+    }
+}
+
+function obterClasseSeveridade(severidade)
+{
+    try
+    {
         var classes = {
-            Critico: 'danger',
-            Crítico: 'danger',
-            Alto: 'warning',
-            Medio: 'info',
-            Médio: 'info',
-            Baixo: 'secondary',
+            'Critico': 'danger',
+            'Crítico': 'danger',
+            'Alto': 'warning',
+            'Medio': 'info',
+            'Médio': 'info',
+            'Baixo': 'secondary'
         };
         return classes[severidade] || 'info';
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_navbar.js',
-            'obterClasseSeveridade',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "obterClasseSeveridade", error);
         return 'info';
     }
 }
 
-function formatarDataHora(dataStr) {
-    try {
+function formatarDataHora(dataStr)
+{
+    try
+    {
         var data = new Date(dataStr);
         var agora = new Date();
         var diff = agora - data;
@@ -701,25 +680,33 @@
         if (dias < 7) return dias + ' dia' + (dias > 1 ? 's' : '') + ' atrás';
 
         return data.toLocaleDateString('pt-BR');
-    } catch (error) {
-        TratamentoErroComLinha('alertas_navbar.js', 'formatarDataHora', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "formatarDataHora", error);
         return dataStr;
     }
 }
 
-function truncarTexto(texto, maxLength) {
-    try {
+function truncarTexto(texto, maxLength)
+{
+    try
+    {
         if (!texto) return '';
         if (texto.length <= maxLength) return texto;
         return texto.substring(0, maxLength) + '...';
-    } catch (error) {
-        TratamentoErroComLinha('alertas_navbar.js', 'truncarTexto', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "truncarTexto", error);
         return texto || '';
     }
 }
 
-(function () {
-    try {
+(function ()
+{
+    try
+    {
         var estiloAlertas = `
         <style id="estiloAlertasNavbar">
         #dropdownAlertas {
@@ -777,12 +764,15 @@
         </style>
         `;
 
-        if ($('#estiloAlertasNavbar').length === 0) {
+        if ($('#estiloAlertasNavbar').length === 0)
+        {
             $('head').append(estiloAlertas);
         }
-    } catch (error) {
-        TratamentoErroComLinha('alertas_navbar.js', 'injecaoEstilos', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_navbar.js", "injecaoEstilos", error);
     }
 })();
 
-console.log('✅ alertas_navbar.js carregado completamente');
+console.log("✅ alertas_navbar.js carregado completamente");
```
