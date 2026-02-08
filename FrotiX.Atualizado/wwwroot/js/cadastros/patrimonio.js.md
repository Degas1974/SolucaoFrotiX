# wwwroot/js/cadastros/patrimonio.js

**Mudanca:** GRANDE | **+1165** linhas | **-1385** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/patrimonio.js
+++ ATUAL: wwwroot/js/cadastros/patrimonio.js
@@ -1,12 +1,15 @@
 var IndexOuUpsert = 0;
 var path = window.location.pathname.toLowerCase();
 var dataTable;
-console.log('Caminho atual:', path);
-
-$(document).ready(function () {
-    try {
-
-        if (path.includes('/patrimonio/upsert')) {
+console.log("Caminho atual:", path);
+
+$(document).ready(function ()
+{
+    try
+    {
+
+        if (path.includes('/patrimonio/upsert'))
+        {
             initModaisBootstrap5();
         }
 
@@ -15,53 +18,57 @@
         setupAddModeloButton();
 
         setTimeout(cleanupEmptyGuids, 1000);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'patrimonio.js',
-            'document.ready.global',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("patrimonio.js", "document.ready.global", error);
     }
 });
 
-function initModaisBootstrap5() {
-    try {
+function initModaisBootstrap5()
+{
+    try
+    {
         const modalIds = ['modalNovaMarca', 'modalNovoModelo'];
 
-        modalIds.forEach((id) => {
+        modalIds.forEach(id =>
+        {
             const element = document.getElementById(id);
-            if (element) {
+            if (element)
+            {
 
                 const existingModal = bootstrap.Modal.getInstance(element);
-                if (existingModal) {
+                if (existingModal)
+                {
                     existingModal.dispose();
                 }
 
                 new bootstrap.Modal(element, {
                     backdrop: 'static',
-                    keyboard: true,
+                    keyboard: true
                 });
             }
         });
 
         setupModalEventListeners();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'patrimonio.js',
-            'initModaisBootstrap5',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("patrimonio.js", "initModaisBootstrap5", error);
     }
 }
 
-function setupModalEventListeners() {
-    try {
+function setupModalEventListeners()
+{
+    try
+    {
 
         const modalNovaMarca = document.getElementById('modalNovaMarca');
-        if (modalNovaMarca) {
-            modalNovaMarca.addEventListener('hidden.bs.modal', function () {
+        if (modalNovaMarca)
+        {
+            modalNovaMarca.addEventListener('hidden.bs.modal', function ()
+            {
                 const input = document.getElementById('inputNovaMarca');
-                if (input) {
+                if (input)
+                {
                     input.value = '';
                     input.classList.remove('is-invalid');
                 }
@@ -69,721 +76,616 @@
         }
 
         const modalNovoModelo = document.getElementById('modalNovoModelo');
-        if (modalNovoModelo) {
-            modalNovoModelo.addEventListener('hidden.bs.modal', function () {
+        if (modalNovoModelo)
+        {
+            modalNovoModelo.addEventListener('hidden.bs.modal', function ()
+            {
                 const inputModelo = document.getElementById('inputNovoModelo');
-                const campoMarca = document.getElementById(
-                    'marcaSelecionadaModelo',
-                );
-
-                if (inputModelo) {
+                const campoMarca = document.getElementById('marcaSelecionadaModelo');
+
+                if (inputModelo)
+                {
                     inputModelo.value = '';
                     inputModelo.classList.remove('is-invalid');
                 }
-                if (campoMarca) {
+                if (campoMarca)
+                {
                     campoMarca.value = '';
                 }
             });
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'patrimonio.js',
-            'setupModalEventListeners',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("patrimonio.js", "setupModalEventListeners", error);
     }
 }
 
-function setupModalCloseHandlers() {
-    try {
-        $('#btnFechaModelo').on('click', function () {
-            try {
-                closeModal('modalNovoModelo');
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'patrimonio.js',
-                    'btnFechaModelo.click',
-                    error,
-                );
+function setupModalCloseHandlers()
+{
+    try
+    {
+        $("#btnFechaModelo").on("click", function ()
+        {
+            try
+            {
+                closeModal("modalNovoModelo");
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("patrimonio.js", "btnFechaModelo.click", error);
             }
         });
 
-        $('#btnFechaMarca').on('click', function () {
-            try {
-                closeModal('modalNovaMarca');
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'patrimonio.js',
-                    'btnFechaMarca.click',
-                    error,
-                );
+        $("#btnFechaMarca").on("click", function ()
+        {
+            try
+            {
+                closeModal("modalNovaMarca");
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("patrimonio.js", "btnFechaMarca.click", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'patrimonio.js',
-            'setupModalCloseHandlers',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("patrimonio.js", "setupModalCloseHandlers", error);
     }
 }
 
-function closeModal(modalId) {
-    try {
+function closeModal(modalId)
+{
+    try
+    {
         const modalElement = document.getElementById(modalId);
-        if (modalElement) {
+        if (modalElement)
+        {
             const modalInstance = bootstrap.Modal.getInstance(modalElement);
-            if (modalInstance) {
+            if (modalInstance)
+            {
                 modalInstance.hide();
-            } else {
-
-                $('#' + modalId).modal('hide');
-                $(document.body).removeClass('modal-open');
-                $('.modal-backdrop').remove();
-                $(document.body).css('overflow', '');
-            }
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('patrimonio.js', 'closeModal', error);
+            } else
+            {
+
+                $("#" + modalId).modal("hide");
+                $(document.body).removeClass("modal-open");
+                $(".modal-backdrop").remove();
+                $(document.body).css("overflow", "");
+            }
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("patrimonio.js", "closeModal", error);
     }
 }
 
-function setupAddModeloButton() {
-    try {
+function setupAddModeloButton()
+{
+    try
+    {
 
         var btnAddMarca = document.getElementById('btnAddMarca');
-        if (btnAddMarca) {
-            btnAddMarca.addEventListener('click', function (e) {
-                try {
+        if (btnAddMarca)
+        {
+            btnAddMarca.addEventListener('click', function (e)
+            {
+                try
+                {
                     e.preventDefault();
                     console.log('Botão adicionar marca clicado');
 
-                    var modalElement =
-                        document.getElementById('modalNovaMarca');
-                    if (modalElement) {
-                        var modalInstance =
-                            bootstrap.Modal.getInstance(modalElement);
-                        if (!modalInstance) {
+                    var modalElement = document.getElementById('modalNovaMarca');
+                    if (modalElement)
+                    {
+                        var modalInstance = bootstrap.Modal.getInstance(modalElement);
+                        if (!modalInstance)
+                        {
                             modalInstance = new bootstrap.Modal(modalElement, {
                                 backdrop: 'static',
-                                keyboard: true,
+                                keyboard: true
                             });
                         }
                         modalInstance.show();
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'patrimonio.js',
-                        'btnAddMarca.click',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("patrimonio.js", "btnAddMarca.click", error);
                 }
             });
         }
 
         var btnAddModelo = document.getElementById('btnAddModelo');
-        if (btnAddModelo) {
-            btnAddModelo.addEventListener('click', function (e) {
-                try {
+        if (btnAddModelo)
+        {
+            btnAddModelo.addEventListener('click', function (e)
+            {
+                try
+                {
                     e.preventDefault();
                     console.log('Botão adicionar modelo clicado');
 
                     var cmbMarcas = getComboboxInstance('cmbMarcas');
-                    if (!cmbMarcas || !cmbMarcas.value) {
-                        AppToast.show(
-                            'Amarelo',
-                            'Por favor, selecione uma marca primeiro.',
-                            2000,
-                        );
+                    if (!cmbMarcas || !cmbMarcas.value)
+                    {
+                        AppToast.show('Amarelo', 'Por favor, selecione uma marca primeiro.', 2000);
                         return;
                     }
 
-                    setTimeout(function () {
-                        try {
-                            var campoMarca = document.getElementById(
-                                'marcaSelecionadaModelo',
-                            );
-                            if (campoMarca && cmbMarcas.value) {
+                    setTimeout(function ()
+                    {
+                        try
+                        {
+                            var campoMarca = document.getElementById('marcaSelecionadaModelo');
+                            if (campoMarca && cmbMarcas.value)
+                            {
                                 campoMarca.value = cmbMarcas.value;
-                                console.log(
-                                    'Marca preenchida no modal:',
-                                    cmbMarcas.value,
-                                );
+                                console.log('Marca preenchida no modal:', cmbMarcas.value);
                             }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'patrimonio.js',
-                                'btnAddModelo.setTimeout',
-                                error,
-                            );
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("patrimonio.js", "btnAddModelo.setTimeout", error);
                         }
                     }, 100);
 
-                    var modalElement =
-                        document.getElementById('modalNovoModelo');
-                    if (modalElement) {
-                        var modalInstance =
-                            bootstrap.Modal.getInstance(modalElement);
-                        if (!modalInstance) {
+                    var modalElement = document.getElementById('modalNovoModelo');
+                    if (modalElement)
+                    {
+                        var modalInstance = bootstrap.Modal.getInstance(modalElement);
+                        if (!modalInstance)
+                        {
                             modalInstance = new bootstrap.Modal(modalElement, {
                                 backdrop: 'static',
-                                keyboard: true,
+                                keyboard: true
                             });
                         }
                         modalInstance.show();
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'patrimonio.js',
-                        'btnAddModelo.click',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("patrimonio.js", "btnAddModelo.click", error);
                 }
             });
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'patrimonio.js',
-            'setupAddModeloButton',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("patrimonio.js", "setupAddModeloButton", error);
     }
 }
 
-function cleanupEmptyGuids() {
-    try {
+function cleanupEmptyGuids()
+{
+    try
+    {
         var cmbSetores = document.getElementById('cmbSetores');
         var cmbSecoes = document.getElementById('cmbSecoes');
 
-        if (
-            cmbSetores &&
-            cmbSetores.ej2_instances &&
-            cmbSetores.ej2_instances[0]
-        ) {
+        if (cmbSetores && cmbSetores.ej2_instances && cmbSetores.ej2_instances[0])
+        {
             var setorCombo = cmbSetores.ej2_instances[0];
-            if (setorCombo.text === '00000000-0000-0000-0000-000000000000') {
+            if (setorCombo.text === "00000000-0000-0000-0000-000000000000")
+            {
                 setorCombo.text = null;
                 setorCombo.value = null;
             }
         }
 
-        if (
-            cmbSecoes &&
-            cmbSecoes.ej2_instances &&
-            cmbSecoes.ej2_instances[0]
-        ) {
+        if (cmbSecoes && cmbSecoes.ej2_instances && cmbSecoes.ej2_instances[0])
+        {
             var secaoCombo = cmbSecoes.ej2_instances[0];
-            if (secaoCombo.text === '00000000-0000-0000-0000-000000000000') {
+            if (secaoCombo.text === "00000000-0000-0000-0000-000000000000")
+            {
                 secaoCombo.text = null;
                 secaoCombo.value = null;
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'patrimonio.js',
-            'cleanupEmptyGuids',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("patrimonio.js", "cleanupEmptyGuids", error);
     }
 }
 
-function stopEnterSubmitting(e) {
-    try {
-        if (e.keyCode == 13) {
+function stopEnterSubmitting(e)
+{
+    try
+    {
+        if (e.keyCode == 13)
+        {
             var src = e.srcElement || e.target;
             console.log(src.tagName.toLowerCase());
 
-            if (src.tagName.toLowerCase() !== 'div') {
-                if (e.preventDefault) {
+            if (src.tagName.toLowerCase() !== "div")
+            {
+                if (e.preventDefault)
+                {
                     e.preventDefault();
-                } else {
+                } else
+                {
                     e.returnValue = false;
                 }
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'patrimonio.js',
-            'stopEnterSubmitting',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("patrimonio.js", "stopEnterSubmitting", error);
     }
 }
 
-function getComboboxInstance(elementId) {
-    try {
+function getComboboxInstance(elementId)
+{
+    try
+    {
         const element = document.getElementById(elementId);
-        if (
-            element &&
-            element.ej2_instances &&
-            element.ej2_instances.length > 0
-        ) {
+        if (element && element.ej2_instances && element.ej2_instances.length > 0)
+        {
             return element.ej2_instances[0];
         }
         return null;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'patrimonio.js',
-            'getComboboxInstance',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("patrimonio.js", "getComboboxInstance", error);
         return null;
     }
 }
 
-if (path == '/patrimonio/index' || path == '/patrimonio') {
-    console.log('Página: Index/Listagem');
-
-    function mostrarLoading() {
-        try {
+if (path == "/patrimonio/index" || path == "/patrimonio")
+{
+    console.log("Página: Index/Listagem");
+
+    function mostrarLoading()
+    {
+        try
+        {
             var overlay = document.getElementById('loadingOverlayPatrimonio');
-            if (overlay) {
+            if (overlay)
+            {
                 overlay.style.display = 'flex';
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'mostrarLoading',
-                error,
-            );
-        }
-    }
-
-    function esconderLoading() {
-        try {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "mostrarLoading", error);
+        }
+    }
+
+    function esconderLoading()
+    {
+        try
+        {
             var overlay = document.getElementById('loadingOverlayPatrimonio');
-            if (overlay) {
+            if (overlay)
+            {
                 overlay.style.display = 'none';
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'esconderLoading',
-                error,
-            );
-        }
-    }
-
-    $(document).ready(function () {
-        try {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "esconderLoading", error);
+        }
+    }
+
+    $(document).ready(function ()
+    {
+        try
+        {
             carregarFiltros();
             loadGrid();
             setupDeleteHandlers();
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'index.ready',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "index.ready", error);
         }
     });
 
-    function carregarFiltros() {
-        try {
-            console.log('Carregando dados dos filtros...');
+    function carregarFiltros()
+    {
+        try
+        {
+            console.log("Carregando dados dos filtros...");
 
             $.ajax({
-                url: '/api/Patrimonio/ListaMarcasModelos',
-                type: 'GET',
-                dataType: 'json',
-                success: function (result) {
-                    try {
-                        if (result && result.success && result.data) {
-                            var ddtMarcaModelo =
-                                document.getElementById('ddtMarcaModelo');
-                            if (
-                                ddtMarcaModelo &&
-                                ddtMarcaModelo.ej2_instances &&
-                                ddtMarcaModelo.ej2_instances[0]
-                            ) {
+                url: "/api/Patrimonio/ListaMarcasModelos",
+                type: "GET",
+                dataType: "json",
+                success: function (result)
+                {
+                    try
+                    {
+                        if (result && result.success && result.data)
+                        {
+                            var ddtMarcaModelo = document.getElementById('ddtMarcaModelo');
+                            if (ddtMarcaModelo && ddtMarcaModelo.ej2_instances && ddtMarcaModelo.ej2_instances[0])
+                            {
                                 ddtMarcaModelo.ej2_instances[0].fields = {
                                     dataSource: result.data,
                                     text: 'text',
                                     value: 'value',
                                     parentValue: 'parentValue',
-                                    hasChildren: 'hasChildren',
+                                    hasChildren: 'hasChildren'
                                 };
                                 ddtMarcaModelo.ej2_instances[0].dataBind();
-                                console.log(
-                                    '✅ Marca/Modelo carregado:',
-                                    result.data.length,
-                                    'itens',
-                                );
+                                console.log("✅ Marca/Modelo carregado:", result.data.length, "itens");
                             }
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'carregarFiltros.marcaModelo.success',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "carregarFiltros.marcaModelo.success", error);
                     }
                 },
-                error: function (err) {
-                    try {
-                        console.error('❌ Erro ao carregar Marca/Modelo:', err);
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'carregarFiltros.marcaModelo.error',
-                            error,
-                        );
-                    }
-                },
+                error: function (err)
+                {
+                    try
+                    {
+                        console.error("❌ Erro ao carregar Marca/Modelo:", err);
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "carregarFiltros.marcaModelo.error", error);
+                    }
+                }
             });
 
             $.ajax({
-                url: '/api/Patrimonio/ListaSetoresSecoes',
-                type: 'GET',
-                dataType: 'json',
-                success: function (result) {
-                    try {
-                        if (result && result.success && result.data) {
-                            var ddtSetorSecao =
-                                document.getElementById('ddtSetorSecao');
-                            if (
-                                ddtSetorSecao &&
-                                ddtSetorSecao.ej2_instances &&
-                                ddtSetorSecao.ej2_instances[0]
-                            ) {
+                url: "/api/Patrimonio/ListaSetoresSecoes",
+                type: "GET",
+                dataType: "json",
+                success: function (result)
+                {
+                    try
+                    {
+                        if (result && result.success && result.data)
+                        {
+                            var ddtSetorSecao = document.getElementById('ddtSetorSecao');
+                            if (ddtSetorSecao && ddtSetorSecao.ej2_instances && ddtSetorSecao.ej2_instances[0])
+                            {
                                 ddtSetorSecao.ej2_instances[0].fields = {
                                     dataSource: result.data,
                                     text: 'text',
                                     value: 'value',
                                     parentValue: 'parentValue',
-                                    hasChildren: 'hasChildren',
+                                    hasChildren: 'hasChildren'
                                 };
                                 ddtSetorSecao.ej2_instances[0].dataBind();
-                                console.log(
-                                    '✅ Setor/Seção carregado:',
-                                    result.data.length,
-                                    'itens',
-                                );
+                                console.log("✅ Setor/Seção carregado:", result.data.length, "itens");
                             }
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'carregarFiltros.setorSecao.success',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "carregarFiltros.setorSecao.success", error);
                     }
                 },
-                error: function (err) {
-                    try {
-                        console.error('❌ Erro ao carregar Setor/Seção:', err);
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'carregarFiltros.setorSecao.error',
-                            error,
-                        );
+                error: function (err)
+                {
+                    try
+                    {
+                        console.error("❌ Erro ao carregar Setor/Seção:", err);
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "carregarFiltros.setorSecao.error", error);
+                    }
+                }
+            });
+
+            $.ajax({
+                url: "/api/Patrimonio/ListaSituacoes",
+                type: "GET",
+                dataType: "json",
+                success: function (result)
+                {
+                    try
+                    {
+                        if (result && result.success && result.data)
+                        {
+                            var cmbSituacao = document.getElementById('cmbSituacao');
+                            if (cmbSituacao && cmbSituacao.ej2_instances && cmbSituacao.ej2_instances[0])
+                            {
+                                cmbSituacao.ej2_instances[0].dataSource = result.data;
+                                cmbSituacao.ej2_instances[0].fields = { text: 'text', value: 'value' };
+                                cmbSituacao.ej2_instances[0].dataBind();
+                                console.log("✅ Situações carregadas:", result.data.length, "itens");
+                            }
+                        }
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "carregarFiltros.situacao.success", error);
                     }
                 },
+                error: function (err)
+                {
+                    try
+                    {
+                        console.error("❌ Erro ao carregar Situações:", err);
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "carregarFiltros.situacao.error", error);
+                    }
+                }
             });
 
-            $.ajax({
-                url: '/api/Patrimonio/ListaSituacoes',
-                type: 'GET',
-                dataType: 'json',
-                success: function (result) {
-                    try {
-                        if (result && result.success && result.data) {
-                            var cmbSituacao =
-                                document.getElementById('cmbSituacao');
-                            if (
-                                cmbSituacao &&
-                                cmbSituacao.ej2_instances &&
-                                cmbSituacao.ej2_instances[0]
-                            ) {
-                                cmbSituacao.ej2_instances[0].dataSource =
-                                    result.data;
-                                cmbSituacao.ej2_instances[0].fields = {
-                                    text: 'text',
-                                    value: 'value',
-                                };
-                                cmbSituacao.ej2_instances[0].dataBind();
-                                console.log(
-                                    '✅ Situações carregadas:',
-                                    result.data.length,
-                                    'itens',
-                                );
-                            }
-                        }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'carregarFiltros.situacao.success',
-                            error,
-                        );
-                    }
-                },
-                error: function (err) {
-                    try {
-                        console.error('❌ Erro ao carregar Situações:', err);
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'carregarFiltros.situacao.error',
-                            error,
-                        );
-                    }
-                },
-            });
-
             var btnFiltrar = document.getElementById('btnFiltrarPatrimonios');
-            if (btnFiltrar) {
-                btnFiltrar.addEventListener('click', function () {
-                    try {
-                        console.log('🔍 Aplicando filtros...');
+            if (btnFiltrar)
+            {
+                btnFiltrar.addEventListener('click', function ()
+                {
+                    try
+                    {
+                        console.log("🔍 Aplicando filtros...");
                         mostrarLoading();
                         aplicarFiltros();
-                    } catch (error) {
+                    } catch (error)
+                    {
                         esconderLoading();
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'btnFiltrarPatrimonios.click',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "btnFiltrarPatrimonios.click", error);
                     }
                 });
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'carregarFiltros',
-                error,
-            );
-        }
-    }
-
-    function aplicarFiltros() {
-        try {
-
-            var ddtMarcaModelo = document.getElementById('ddtMarcaModelo');
-            var ddtSetorSecao = document.getElementById('ddtSetorSecao');
-            var cmbSituacao = document.getElementById('cmbSituacao');
-
-            var marcasModelos = [];
-            var setoresSecoes = [];
-            var situacao = '';
-
-            if (
-                ddtMarcaModelo &&
-                ddtMarcaModelo.ej2_instances &&
-                ddtMarcaModelo.ej2_instances[0]
-            ) {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "carregarFiltros", error);
+        }
+    }
+
+    function aplicarFiltros()
+    {
+        try
+        {
+
+            if (ddtMarcaModelo && ddtMarcaModelo.ej2_instances && ddtMarcaModelo.ej2_instances[0])
+            {
                 var valoresSelecionados = ddtMarcaModelo.ej2_instances[0].value;
-                if (valoresSelecionados && valoresSelecionados.length > 0) {
+                if (valoresSelecionados && valoresSelecionados.length > 0)
+                {
                     marcasModelos = valoresSelecionados;
                 }
             }
 
-            if (
-                ddtSetorSecao &&
-                ddtSetorSecao.ej2_instances &&
-                ddtSetorSecao.ej2_instances[0]
-            ) {
+            if (ddtSetorSecao && ddtSetorSecao.ej2_instances && ddtSetorSecao.ej2_instances[0])
+            {
                 var valoresSelecionados = ddtSetorSecao.ej2_instances[0].value;
-                if (valoresSelecionados && valoresSelecionados.length > 0) {
+                if (valoresSelecionados && valoresSelecionados.length > 0)
+                {
                     setoresSecoes = valoresSelecionados;
                 }
             }
 
-            if (
-                cmbSituacao &&
-                cmbSituacao.ej2_instances &&
-                cmbSituacao.ej2_instances[0]
-            ) {
-                situacao = cmbSituacao.ej2_instances[0].value || '';
-            }
-
-            console.log('Filtros aplicados:', {
+            if (cmbSituacao && cmbSituacao.ej2_instances && cmbSituacao.ej2_instances[0])
+            {
+                situacao = cmbSituacao.ej2_instances[0].value || "";
+            }
+
+            console.log("Filtros aplicados:", {
                 marcasModelos: marcasModelos,
                 setoresSecoes: setoresSecoes,
-                situacao: situacao,
+                situacao: situacao
             });
 
-            if ($.fn.DataTable.isDataTable('#tblPatrimonio')) {
+            if ($.fn.DataTable.isDataTable('#tblPatrimonio'))
+            {
                 var table = $('#tblPatrimonio').DataTable();
 
-                var marcaParam =
-                    marcasModelos.length > 0 ? marcasModelos.join(',') : '';
-                var setorParam =
-                    setoresSecoes.length > 0 ? setoresSecoes.join(',') : '';
-
-                table.ajax
-                    .url(
-                        `/api/Patrimonio/?marca=${encodeURIComponent(marcaParam)}&modelo=${encodeURIComponent(marcaParam)}&setor=${encodeURIComponent(setorParam)}&secao=${encodeURIComponent(setorParam)}&situacao=${encodeURIComponent(situacao)}`,
-                    )
-                    .load(function (json) {
-                        try {
-                            esconderLoading();
-
-                            if (json && json.data && json.data.length === 0) {
-                                AppToast.show(
-                                    'Amarelo',
-                                    'Nenhum patrimônio encontrado com os filtros selecionados.',
-                                    3000,
-                                );
-                            } else if (
-                                json &&
-                                json.data &&
-                                json.data.length > 0
-                            ) {
-                                console.log(
-                                    `✅ ${json.data.length} patrimônio(s) encontrado(s)`,
-                                );
-                            }
-                        } catch (error) {
-                            esconderLoading();
-                            Alerta.TratamentoErroComLinha(
-                                'patrimonio.js',
-                                'aplicarFiltros.load.callback',
-                                error,
-                            );
-                        }
-                    });
-            }
-        } catch (error) {
+                var marcaParam = marcasModelos.length > 0 ? marcasModelos.join(',') : '';
+                var setorParam = setoresSecoes.length > 0 ? setoresSecoes.join(',') : '';
+
+                table.ajax.url(
+                    `/api/Patrimonio/?marca=${encodeURIComponent(marcaParam)}&modelo=${encodeURIComponent(marcaParam)}&setor=${encodeURIComponent(setorParam)}&secao=${encodeURIComponent(setorParam)}&situacao=${encodeURIComponent(situacao)}`
+                ).load(function (json)
+                {
+                    try
+                    {
+                        esconderLoading();
+
+                        if (json && json.data && json.data.length === 0)
+                        {
+                            AppToast.show('Amarelo', 'Nenhum patrimônio encontrado com os filtros selecionados.', 3000);
+                        }
+                        else if (json && json.data && json.data.length > 0)
+                        {
+                            console.log(`✅ ${json.data.length} patrimônio(s) encontrado(s)`);
+                        }
+                    } catch (error)
+                    {
+                        esconderLoading();
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "aplicarFiltros.load.callback", error);
+                    }
+                });
+            }
+        } catch (error)
+        {
             esconderLoading();
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'aplicarFiltros',
-                error,
-            );
-        }
-    }
-
-    function loadGrid() {
-        try {
-            console.log('Carregando grid de patrimônios');
+            Alerta.TratamentoErroComLinha("patrimonio.js", "aplicarFiltros", error);
+        }
+    }
+
+    function loadGrid()
+    {
+        try
+        {
+            console.log("Carregando grid de patrimônios");
             mostrarLoading();
-            dataTable = $('#tblPatrimonio').DataTable({
+            dataTable = $("#tblPatrimonio").DataTable({
                 columnDefs: [
-                    { targets: 0, className: 'text-center', width: '6%' },
-                    {
-                        targets: 1,
-                        className: 'text-left',
-                        width: '10%',
-                        defaultContent: 'Não informado',
-                    },
-                    {
-                        targets: 2,
-                        className: 'text-left',
-                        width: '10%',
-                        defaultContent: 'Não informado',
-                    },
-                    {
-                        targets: 3,
-                        className: 'text-left',
-                        width: '10%',
-                        defaultContent: 'Não informado',
-                    },
-                    { targets: 4, className: 'text-left', width: '10%' },
-                    { targets: 5, className: 'text-left', width: '10%' },
-                    {
-                        targets: 6,
-                        className: 'text-center',
-                        width: '8%',
-                        defaultContent: 'Não informado',
-                    },
-                    { targets: 7, className: 'text-center', width: '8%' },
+                    { targets: 0, className: "text-center", width: "6%" },
+                    { targets: 1, className: "text-left", width: "10%", defaultContent: "Não informado" },
+                    { targets: 2, className: "text-left", width: "10%", defaultContent: "Não informado" },
+                    { targets: 3, className: "text-left", width: "10%", defaultContent: "Não informado" },
+                    { targets: 4, className: "text-left", width: "10%" },
+                    { targets: 5, className: "text-left", width: "10%" },
+                    { targets: 6, className: "text-center", width: "8%", defaultContent: "Não informado" },
+                    { targets: 7, className: "text-center", width: "8%" }
                 ],
                 responsive: true,
                 ajax: {
-                    url: '/api/Patrimonio/',
-                    type: 'GET',
-                    datatype: 'json',
-                    error: function (xhr, status, error) {
-                        try {
+                    url: "/api/Patrimonio/",
+                    type: "GET",
+                    datatype: "json",
+                    error: function (xhr, status, error)
+                    {
+                        try
+                        {
                             esconderLoading();
-                            console.error('Erro ao carregar os dados:', error);
-                            AppToast.show(
-                                'Vermelho',
-                                'Erro ao carregar dados da tabela',
-                                3000,
-                            );
-                        } catch (err) {
+                            console.error("Erro ao carregar os dados:", error);
+                            AppToast.show('Vermelho', 'Erro ao carregar dados da tabela', 3000);
+                        } catch (err)
+                        {
                             esconderLoading();
-                            Alerta.TratamentoErroComLinha(
-                                'patrimonio.js',
-                                'loadGrid.ajax.error',
-                                err,
-                            );
-                        }
-                    },
+                            Alerta.TratamentoErroComLinha("patrimonio.js", "loadGrid.ajax.error", err);
+                        }
+                    }
                 },
-                initComplete: function () {
+                initComplete: function ()
+                {
                     esconderLoading();
                 },
-                drawCallback: function () {
+                drawCallback: function ()
+                {
                     esconderLoading();
                 },
                 columns: [
-                    { data: 'npr' },
-                    { data: 'marca' },
-                    { data: 'modelo' },
-                    { data: 'descricao' },
-                    { data: 'nomeSetor' },
-                    { data: 'nomeSecao' },
-                    {
-                        data: 'situacao',
-                        render: function (data, type, row) {
-                            try {
-                                if (!data)
-                                    return '<span class="btn fundo-cinza" style="display: inline-block; font-size: 0.75rem; padding: 0.35rem 0.65rem; border-radius: 0.375rem; text-align: center; min-width: 100px; cursor: default;">Não informado</span>';
-
-                                var situacaoNormalizada = data
-                                    .toString()
-                                    .trim()
-                                    .toLowerCase();
-
-                                if (situacaoNormalizada === 'em uso') {
+                    { data: "npr" },
+                    { data: "marca" },
+                    { data: "modelo" },
+                    { data: "descricao" },
+                    { data: "nomeSetor" },
+                    { data: "nomeSecao" },
+                    {
+                        data: "situacao",
+                        render: function (data, type, row)
+                        {
+                            try
+                            {
+                                if (!data) return '<span class="btn fundo-cinza" style="display: inline-block; font-size: 0.75rem; padding: 0.35rem 0.65rem; border-radius: 0.375rem; text-align: center; min-width: 100px; cursor: default;">Não informado</span>';
+
+                                var situacaoNormalizada = data.toString().trim().toLowerCase();
+
+                                if (situacaoNormalizada === "em uso")
+                                {
                                     return '<span class="btn btn-verde" style="display: inline-block; font-size: 0.75rem; padding: 0.35rem 0.65rem; border-radius: 0.375rem; text-align: center; min-width: 100px; cursor: default;">Em Uso</span>';
-                                } else if (
-                                    situacaoNormalizada === 'em manutenção' ||
-                                    situacaoNormalizada === 'em manutencao'
-                                ) {
+                                }
+                                else if (situacaoNormalizada === "em manutenção" || situacaoNormalizada === "em manutencao")
+                                {
                                     return '<span class="btn btn-fundo-laranja" style="display: inline-block; font-size: 0.75rem; padding: 0.35rem 0.65rem; border-radius: 0.375rem; text-align: center; min-width: 100px; cursor: default;">Em Manutenção</span>';
-                                } else if (
-                                    situacaoNormalizada === 'não localizado' ||
-                                    situacaoNormalizada === 'nao localizado'
-                                ) {
+                                }
+                                else if (situacaoNormalizada === "não localizado" || situacaoNormalizada === "nao localizado")
+                                {
                                     return '<span class="btn btn-vinho" style="display: inline-block; font-size: 0.75rem; padding: 0.35rem 0.65rem; border-radius: 0.375rem; text-align: center; min-width: 100px; cursor: default;">Não Localizado</span>';
-                                } else if (
-                                    situacaoNormalizada ===
-                                        'avariado/inservível' ||
-                                    situacaoNormalizada ===
-                                        'avariado/inservivel'
-                                ) {
+                                }
+                                else if (situacaoNormalizada === "avariado/inservível" || situacaoNormalizada === "avariado/inservivel")
+                                {
                                     return '<span class="btn btn-preto" style="display: inline-block; font-size: 0.75rem; padding: 0.35rem 0.65rem; border-radius: 0.375rem; text-align: center; min-width: 100px; cursor: default;">Avariado/Inservível</span>';
-                                } else if (
-                                    situacaoNormalizada ===
-                                        'transferido (baixado)' ||
-                                    situacaoNormalizada === 'transferido'
-                                ) {
+                                }
+                                else if (situacaoNormalizada === "transferido (baixado)" || situacaoNormalizada === "transferido")
+                                {
                                     return '<span class="btn fundo-cinza" style="display: inline-block; font-size: 0.75rem; padding: 0.35rem 0.65rem; border-radius: 0.375rem; text-align: center; min-width: 100px; cursor: default;">Transferido</span>';
-                                } else {
-                                    return (
-                                        '<span class="btn fundo-cinza" style="display: inline-block; font-size: 0.75rem; padding: 0.35rem 0.65rem; border-radius: 0.375rem; text-align: center; min-width: 100px; cursor: default;">' +
-                                        data +
-                                        '</span>'
-                                    );
                                 }
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'patrimonio.js',
-                                    'loadGrid.situacao.render',
-                                    error,
-                                );
+                                else
+                                {
+                                    return '<span class="btn fundo-cinza" style="display: inline-block; font-size: 0.75rem; padding: 0.35rem 0.65rem; border-radius: 0.375rem; text-align: center; min-width: 100px; cursor: default;">' + data + '</span>';
+                                }
+                            } catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha("patrimonio.js", "loadGrid.situacao.render", error);
                                 return data;
                             }
-                        },
+                        }
                     },
                     {
-                        data: 'patrimonioId',
-                        render: function (data) {
+                        data: "patrimonioId",
+                        render: function (data)
+                        {
                             return `<div class="d-flex justify-content-center gap-1">
                                 <a href="/Patrimonio/Upsert?id=${data}"
                                    class="btn btn-sm btn-azul text-white"
@@ -804,868 +706,769 @@
                                     <i class="fa-solid fa-arrow-up-arrow-down"></i>
                                 </a>
                             </div>`;
-                        },
-                    },
+                        }
+                    }
                 ],
                 language: {
-                    url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                    emptyTable: 'Sem Dados para Exibição',
+                    url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                    emptyTable: "Sem Dados para Exibição"
                 },
-                width: '100%',
+                width: "100%"
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('patrimonio.js', 'loadGrid', error);
-        }
-    }
-
-    function setupDeleteHandlers() {
-        try {
-            $(document).on('click', '.btn-delete', function () {
-                try {
-                    var id = $(this).data('id');
-                    console.log('Deletar patrimônio:', id);
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "loadGrid", error);
+        }
+    }
+
+    function setupDeleteHandlers()
+    {
+        try
+        {
+            $(document).on("click", ".btn-delete", function ()
+            {
+                try
+                {
+                    var id = $(this).data("id");
+                    console.log("Deletar patrimônio:", id);
 
                     Alerta.Confirmar(
-                        'Confirmar Exclusão',
-                        'Você tem certeza que deseja apagar este patrimônio? Não será possível recuperar os dados eliminados!',
-                        'Sim, excluir',
-                        'Cancelar',
-                    ).then(function (willDelete) {
-                        try {
-                            if (willDelete) {
-                                var dataToPost = JSON.stringify({
-                                    PatrimonioId: id,
-                                });
+                        "Confirmar Exclusão",
+                        "Você tem certeza que deseja apagar este patrimônio? Não será possível recuperar os dados eliminados!",
+                        "Sim, excluir",
+                        "Cancelar"
+                    ).then(function (willDelete)
+                    {
+                        try
+                        {
+                            if (willDelete)
+                            {
+                                var dataToPost = JSON.stringify({ PatrimonioId: id });
                                 $.ajax({
-                                    url: '/api/Patrimonio/Delete',
-                                    type: 'POST',
+                                    url: "/api/Patrimonio/Delete",
+                                    type: "POST",
                                     data: dataToPost,
-                                    contentType:
-                                        'application/json; charset=utf-8',
-                                    dataType: 'json',
-                                    success: function (data) {
-                                        try {
-                                            if (data.success) {
-                                                AppToast.show(
-                                                    'Verde',
-                                                    data.message,
-                                                    2000,
-                                                );
+                                    contentType: "application/json; charset=utf-8",
+                                    dataType: "json",
+                                    success: function (data)
+                                    {
+                                        try
+                                        {
+                                            if (data.success)
+                                            {
+                                                AppToast.show('Verde', data.message, 2000);
                                                 dataTable.ajax.reload();
-                                            } else {
-                                                AppToast.show(
-                                                    'Vermelho',
-                                                    data.message,
-                                                    2000,
-                                                );
+                                            } else
+                                            {
+                                                AppToast.show('Vermelho', data.message, 2000);
                                             }
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha(
-                                                'patrimonio.js',
-                                                'setupDeleteHandlers.success',
-                                                error,
-                                            );
+                                        } catch (error)
+                                        {
+                                            Alerta.TratamentoErroComLinha("patrimonio.js", "setupDeleteHandlers.success", error);
                                         }
                                     },
-                                    error: function (err) {
-                                        try {
+                                    error: function (err)
+                                    {
+                                        try
+                                        {
                                             console.error(err);
-                                            AppToast.show(
-                                                'Vermelho',
-                                                'Erro ao deletar patrimônio',
-                                                2000,
-                                            );
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha(
-                                                'patrimonio.js',
-                                                'setupDeleteHandlers.error',
-                                                error,
-                                            );
+                                            AppToast.show('Vermelho', 'Erro ao deletar patrimônio', 2000);
+                                        } catch (error)
+                                        {
+                                            Alerta.TratamentoErroComLinha("patrimonio.js", "setupDeleteHandlers.error", error);
                                         }
-                                    },
+                                    }
                                 });
                             }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'patrimonio.js',
-                                'setupDeleteHandlers.then',
-                                error,
-                            );
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("patrimonio.js", "setupDeleteHandlers.then", error);
                         }
                     });
-                } catch (innerError) {
-                    Alerta.TratamentoErroComLinha(
-                        'patrimonio.js',
-                        'setupDeleteHandlers.click',
-                        innerError,
-                    );
+                } catch (innerError)
+                {
+                    Alerta.TratamentoErroComLinha("patrimonio.js", "setupDeleteHandlers.click", innerError);
                 }
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'setupDeleteHandlers',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "setupDeleteHandlers", error);
         }
     }
 }
 
-else if (path == '/patrimonio/upsert') {
-    console.log('Página: Upsert (Cadastro/Edição)');
+else if (path == "/patrimonio/upsert")
+{
+    console.log("Página: Upsert (Cadastro/Edição)");
 
     var edicao = false;
     var filesName = [];
 
-    $(document).ready(function () {
-        try {
-            console.log('Inicializando página Upsert');
-
-            var patrimonioId = document.getElementById('PatrimonioId');
-            if (
-                patrimonioId &&
-                patrimonioId.value &&
-                patrimonioId.value !== '00000000-0000-0000-0000-000000000000'
-            ) {
+    $(document).ready(function ()
+    {
+        try
+        {
+            console.log("Inicializando página Upsert");
+
+            var patrimonioId = document.getElementById("PatrimonioId");
+            if (patrimonioId && patrimonioId.value && patrimonioId.value !== "00000000-0000-0000-0000-000000000000")
+            {
                 edicao = true;
             }
 
-            setTimeout(function () {
-                try {
+            setTimeout(function ()
+            {
+                try
+                {
                     initSituacao();
                     initMarcaModelo();
                     initSetorSecao();
                     setupValidation();
                     setupModalHandlers();
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'patrimonio.js',
-                        'upsert.setTimeout',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("patrimonio.js", "upsert.setTimeout", error);
                 }
             }, 500);
 
-            const checkbox = document.getElementById('chkStatus');
-            if (!edicao && checkbox) {
+            const checkbox = document.getElementById("chkStatus");
+            if (!edicao && checkbox)
+            {
                 checkbox.checked = true;
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'upsert.ready',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "upsert.ready", error);
         }
     });
 
-    function initSituacao() {
-        try {
-            console.log('Inicializando Situação');
-
-            var dropdownSituacao = getComboboxInstance('cmbSituacao');
-
-            if (!dropdownSituacao) {
-                console.warn(
-                    'Dropdown Situação ainda não inicializado, tentando novamente...',
-                );
+    function initSituacao()
+    {
+        try
+        {
+            console.log("Inicializando Situação");
+
+            var dropdownSituacao = getComboboxInstance("cmbSituacao");
+
+            if (!dropdownSituacao)
+            {
+                console.warn("Dropdown Situação ainda não inicializado, tentando novamente...");
                 setTimeout(initSituacao, 500);
                 return;
             }
 
             var listaSituacoes = [
-                { text: 'Em Uso', value: 'Em Uso' },
-                { text: 'Em Manutenção', value: 'Em Manutenção' },
-                { text: 'Não Localizado', value: 'Não Localizado' },
-                { text: 'Avariado/Inservível', value: 'Avariado/Inservível' },
-                {
-                    text: 'Transferido (baixado)',
-                    value: 'Transferido (baixado)',
-                },
+                { text: "Em Uso", value: "Em Uso" },
+                { text: "Em Manutenção", value: "Em Manutenção" },
+                { text: "Não Localizado", value: "Não Localizado" },
+                { text: "Avariado/Inservível", value: "Avariado/Inservível" },
+                { text: "Transferido (baixado)", value: "Transferido (baixado)" }
             ];
 
             dropdownSituacao.dataSource = listaSituacoes;
-            dropdownSituacao.fields = { text: 'text', value: 'value' };
+            dropdownSituacao.fields = { text: "text", value: "value" };
             dropdownSituacao.dataBind();
 
-            var situacaoAtual =
-                document.getElementById('SituacaoId')?.value || '';
-            if (situacaoAtual) {
-                setTimeout(function () {
-                    try {
+            var situacaoAtual = document.getElementById("SituacaoId")?.value || "";
+            if (situacaoAtual)
+            {
+                setTimeout(function ()
+                {
+                    try
+                    {
                         dropdownSituacao.value = situacaoAtual;
-                        console.log('Situação carregada:', situacaoAtual);
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'initSituacao.setTimeout',
-                            error,
-                        );
+                        console.log("Situação carregada:", situacaoAtual);
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "initSituacao.setTimeout", error);
                     }
                 }, 300);
             }
 
-            console.log('Situação inicializada com sucesso');
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'initSituacao',
-                error,
-            );
-        }
-    }
-
-    function initMarcaModelo() {
-        try {
-            console.log('Inicializando Marca e Modelo');
-
-            var marcaAtual = document.getElementById('MarcaId')?.value || '';
-            var modeloAtual = document.getElementById('ModeloId')?.value || '';
-
-            var comboBoxMarcas = getComboboxInstance('cmbMarcas');
-            var comboBoxModelos = getComboboxInstance('cmbModelos');
-
-            if (!comboBoxMarcas || !comboBoxModelos) {
-                console.warn(
-                    'ComboBoxes ainda não inicializadas, tentando novamente...',
-                );
+            console.log("Situação inicializada com sucesso");
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "initSituacao", error);
+        }
+    }
+
+    function initMarcaModelo()
+    {
+        try
+        {
+            console.log("Inicializando Marca e Modelo");
+
+            var marcaAtual = document.getElementById("MarcaId")?.value || "";
+            var modeloAtual = document.getElementById("ModeloId")?.value || "";
+
+            var comboBoxMarcas = getComboboxInstance("cmbMarcas");
+            var comboBoxModelos = getComboboxInstance("cmbModelos");
+
+            if (!comboBoxMarcas || !comboBoxModelos)
+            {
+                console.warn("ComboBoxes ainda não inicializadas, tentando novamente...");
                 setTimeout(initMarcaModelo, 500);
                 return;
             }
 
             loadListaMarcas();
 
-            if (marcaAtual) {
-                setTimeout(function () {
-                    try {
+            if (marcaAtual)
+            {
+                setTimeout(function ()
+                {
+                    try
+                    {
                         comboBoxMarcas.value = marcaAtual;
                         loadListaModelos(marcaAtual, modeloAtual);
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'initMarcaModelo.setTimeout',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "initMarcaModelo.setTimeout", error);
                     }
                 }, 1000);
             }
 
-            comboBoxMarcas.addEventListener('change', onMarcaChange);
-
-            var btnAddModelo = document.getElementById('btnAddModelo');
-            if (btnAddModelo) {
+            comboBoxMarcas.addEventListener("change", onMarcaChange);
+
+            var btnAddModelo = document.getElementById("btnAddModelo");
+            if (btnAddModelo)
+            {
                 btnAddModelo.disabled = true;
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'initMarcaModelo',
-                error,
-            );
-        }
-    }
-
-    function onMarcaChange(args) {
-        try {
-            var comboBoxModelos = getComboboxInstance('cmbModelos');
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "initMarcaModelo", error);
+        }
+    }
+
+    function onMarcaChange(args)
+    {
+        try
+        {
+            var comboBoxModelos = getComboboxInstance("cmbModelos");
             var marcaSelecionada = args.value;
-            console.log('Marca selecionada:', marcaSelecionada);
-
-            var btnAddModelo = document.getElementById('btnAddModelo');
-            if (btnAddModelo) {
+            console.log("Marca selecionada:", marcaSelecionada);
+
+            var btnAddModelo = document.getElementById("btnAddModelo");
+            if (btnAddModelo)
+            {
                 btnAddModelo.disabled = !marcaSelecionada;
             }
 
-            if (marcaSelecionada) {
+            if (marcaSelecionada)
+            {
                 loadListaModelos(marcaSelecionada);
-            } else {
-
-                if (comboBoxModelos) {
+            } else
+            {
+
+                if (comboBoxModelos)
+                {
                     comboBoxModelos.dataSource = [];
                     comboBoxModelos.value = null;
                     comboBoxModelos.dataBind();
                 }
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'onMarcaChange',
-                error,
-            );
-        }
-    }
-
-    function loadListaMarcas() {
-        try {
-            var comboBox = getComboboxInstance('cmbMarcas');
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "onMarcaChange", error);
+        }
+    }
+
+    function loadListaMarcas()
+    {
+        try
+        {
+            var comboBox = getComboboxInstance("cmbMarcas");
             if (!comboBox) return;
 
             $.ajax({
-                type: 'GET',
-                url: '/api/Patrimonio/ListaMarcas',
-                success: function (res) {
-                    try {
-                        if (res && res.data && res.data.length > 0) {
-                            comboBox.fields = { text: 'text', value: 'value' };
+                type: "GET",
+                url: "/api/Patrimonio/ListaMarcas",
+                success: function (res)
+                {
+                    try
+                    {
+                        if (res && res.data && res.data.length > 0)
+                        {
+                            comboBox.fields = { text: "text", value: "value" };
                             comboBox.dataSource = res.data;
                             comboBox.dataBind();
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'loadListaMarcas.success',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaMarcas.success", error);
                     }
                 },
-                error: function (error) {
-                    try {
-                        console.error('Erro ao carregar marcas:', error);
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao carregar lista de marcas',
-                            2000,
-                        );
-                    } catch (err) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'loadListaMarcas.error',
-                            err,
-                        );
-                    }
-                },
+                error: function (error)
+                {
+                    try
+                    {
+                        console.error("Erro ao carregar marcas:", error);
+                        AppToast.show('Vermelho', 'Erro ao carregar lista de marcas', 2000);
+                    } catch (err)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaMarcas.error", err);
+                    }
+                }
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'loadListaMarcas',
-                error,
-            );
-        }
-    }
-
-    function loadListaModelos(marca, modeloAtual) {
-        try {
-            var comboBoxModelos = getComboboxInstance('cmbModelos');
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaMarcas", error);
+        }
+    }
+
+    function loadListaModelos(marca, modeloAtual)
+    {
+        try
+        {
+            var comboBoxModelos = getComboboxInstance("cmbModelos");
             if (!comboBoxModelos) return;
 
             $.ajax({
-                type: 'GET',
-                url: '/api/Patrimonio/ListaModelos',
+                type: "GET",
+                url: "/api/Patrimonio/ListaModelos",
                 data: { marca: marca },
-                success: function (res) {
-                    try {
-                        if (res && res.data && res.data.length > 0) {
-                            comboBoxModelos.fields = {
-                                text: 'text',
-                                value: 'value',
-                            };
+                success: function (res)
+                {
+                    try
+                    {
+                        if (res && res.data && res.data.length > 0)
+                        {
+                            comboBoxModelos.fields = { text: "text", value: "value" };
                             comboBoxModelos.dataSource = res.data;
 
-                            if (modeloAtual) {
-                                comboBoxModelos.dataBound = function () {
+                            if (modeloAtual)
+                            {
+                                comboBoxModelos.dataBound = function ()
+                                {
                                     comboBoxModelos.value = modeloAtual;
                                 };
                             }
 
                             comboBoxModelos.dataBind();
-                        } else {
+                        } else
+                        {
 
                             comboBoxModelos.dataSource = [];
                             comboBoxModelos.value = null;
                             comboBoxModelos.dataBind();
 
-                            if (!modeloAtual) {
-                                AppToast.show(
-                                    'Amarelo',
-                                    'Nenhum modelo cadastrado para esta marca',
-                                    2000,
-                                );
+                            if (!modeloAtual)
+                            {
+                                AppToast.show('Amarelo', 'Nenhum modelo cadastrado para esta marca', 2000);
                             }
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'loadListaModelos.success',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaModelos.success", error);
                     }
                 },
-                error: function (error) {
-                    try {
-                        console.error('Erro ao carregar modelos:', error);
-
-                        if (comboBoxModelos) {
+                error: function (error)
+                {
+                    try
+                    {
+                        console.error("Erro ao carregar modelos:", error);
+
+                        if (comboBoxModelos)
+                        {
                             comboBoxModelos.dataSource = [];
                             comboBoxModelos.value = null;
                             comboBoxModelos.dataBind();
                         }
-                        AppToast.show(
-                            'Amarelo',
-                            'Nenhum modelo encontrado para esta marca',
-                            2000,
-                        );
-                    } catch (err) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'loadListaModelos.error',
-                            err,
-                        );
-                    }
-                },
+                        AppToast.show('Amarelo', 'Nenhum modelo encontrado para esta marca', 2000);
+                    } catch (err)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaModelos.error", err);
+                    }
+                }
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'loadListaModelos',
-                error,
-            );
-        }
-    }
-
-    function initSetorSecao() {
-        try {
-            console.log('Inicializando Setor e Seção');
-
-            var comboBoxSetores = getComboboxInstance('cmbSetores');
-            var comboBoxSecoes = getComboboxInstance('cmbSecoes');
-
-            if (!comboBoxSetores || !comboBoxSecoes) {
-                console.warn(
-                    'ComboBoxes de setor/seção ainda não inicializadas',
-                );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaModelos", error);
+        }
+    }
+
+    function initSetorSecao()
+    {
+        try
+        {
+            console.log("Inicializando Setor e Seção");
+
+            var comboBoxSetores = getComboboxInstance("cmbSetores");
+            var comboBoxSecoes = getComboboxInstance("cmbSecoes");
+
+            if (!comboBoxSetores || !comboBoxSecoes)
+            {
+                console.warn("ComboBoxes de setor/seção ainda não inicializadas");
                 setTimeout(initSetorSecao, 500);
                 return;
             }
 
-            if (
-                comboBoxSetores.value === '00000000-0000-0000-0000-000000000000'
-            ) {
+            if (comboBoxSetores.value === "00000000-0000-0000-0000-000000000000")
+            {
                 comboBoxSetores.value = null;
             }
-            if (
-                comboBoxSecoes.value === '00000000-0000-0000-0000-000000000000'
-            ) {
+            if (comboBoxSecoes.value === "00000000-0000-0000-0000-000000000000")
+            {
                 comboBoxSecoes.value = null;
             }
 
-            var setorAtual = document.getElementById('SetorId')?.value || '';
-            var secaoAtual = document.getElementById('SecaoId')?.value || '';
-
-            loadListaSetores(function () {
-                try {
-                    if (
-                        setorAtual &&
-                        setorAtual !== '00000000-0000-0000-0000-000000000000'
-                    ) {
+            var setorAtual = document.getElementById("SetorId")?.value || "";
+            var secaoAtual = document.getElementById("SecaoId")?.value || "";
+
+            loadListaSetores(function ()
+            {
+                try
+                {
+                    if (setorAtual && setorAtual !== "00000000-0000-0000-0000-000000000000")
+                    {
                         comboBoxSetores.value = setorAtual;
                         loadListaSecoes(setorAtual, secaoAtual);
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'patrimonio.js',
-                        'initSetorSecao.callback',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("patrimonio.js", "initSetorSecao.callback", error);
                 }
             });
 
             comboBoxSetores.change = onSetorChange;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'initSetorSecao',
-                error,
-            );
-        }
-    }
-
-    function onSetorChange(args) {
-        try {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "initSetorSecao", error);
+        }
+    }
+
+    function onSetorChange(args)
+    {
+        try
+        {
             var setorSelecionado = args.value;
-            console.log('Setor selecionado:', setorSelecionado);
-
-            document.getElementById('divSecao').style.display = 'block';
-
-            if (setorSelecionado) {
+            console.log("Setor selecionado:", setorSelecionado);
+
+            document.getElementById("divSecao").style.display = "block";
+
+            if (setorSelecionado)
+            {
                 loadListaSecoes(setorSelecionado);
-            } else {
-                var comboBoxSecoes = getComboboxInstance('cmbSecoes');
-                if (comboBoxSecoes) {
+            } else
+            {
+                var comboBoxSecoes = getComboboxInstance("cmbSecoes");
+                if (comboBoxSecoes)
+                {
                     comboBoxSecoes.dataSource = [];
                     comboBoxSecoes.value = null;
                     comboBoxSecoes.dataBind();
                 }
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'onSetorChange',
-                error,
-            );
-        }
-    }
-
-    function loadListaSetores(callback) {
-        try {
-            var comboBoxSetores = getComboboxInstance('cmbSetores');
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "onSetorChange", error);
+        }
+    }
+
+    function loadListaSetores(callback)
+    {
+        try
+        {
+            var comboBoxSetores = getComboboxInstance("cmbSetores");
             if (!comboBoxSetores) return;
 
             $.ajax({
-                type: 'GET',
-                url: '/api/Patrimonio/ListaSetores',
-                success: function (res) {
-                    try {
-                        if (res && res.data && res.data.length > 0) {
-
-                            if (
-                                comboBoxSetores.text ===
-                                '00000000-0000-0000-0000-000000000000'
-                            ) {
+                type: "GET",
+                url: "/api/Patrimonio/ListaSetores",
+                success: function (res)
+                {
+                    try
+                    {
+                        if (res && res.data && res.data.length > 0)
+                        {
+
+                            if (comboBoxSetores.text === "00000000-0000-0000-0000-000000000000")
+                            {
                                 comboBoxSetores.text = null;
                             }
 
                             comboBoxSetores.dataSource = res.data;
-                            comboBoxSetores.fields = {
-                                text: 'text',
-                                value: 'value',
-                            };
-                            comboBoxSetores.placeholder = 'Selecione um Setor';
+                            comboBoxSetores.fields = { text: "text", value: "value" };
+                            comboBoxSetores.placeholder = "Selecione um Setor";
                             comboBoxSetores.dataBind();
 
                             if (callback) callback();
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'loadListaSetores.success',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaSetores.success", error);
                     }
                 },
-                error: function (error) {
-                    try {
-                        console.error('Erro ao carregar setores:', error);
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao carregar lista de setores',
-                            2000,
-                        );
-                    } catch (err) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'loadListaSetores.error',
-                            err,
-                        );
-                    }
-                },
+                error: function (error)
+                {
+                    try
+                    {
+                        console.error("Erro ao carregar setores:", error);
+                        AppToast.show('Vermelho', 'Erro ao carregar lista de setores', 2000);
+                    } catch (err)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaSetores.error", err);
+                    }
+                }
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'loadListaSetores',
-                error,
-            );
-        }
-    }
-
-    function loadListaSecoes(setorId, secaoAtual) {
-        try {
-            var comboBoxSecoes = getComboboxInstance('cmbSecoes');
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaSetores", error);
+        }
+    }
+
+    function loadListaSecoes(setorId, secaoAtual)
+    {
+        try
+        {
+            var comboBoxSecoes = getComboboxInstance("cmbSecoes");
             if (!comboBoxSecoes) return;
 
             $.ajax({
-                type: 'GET',
-                url: '/api/Patrimonio/ListaSecoes',
+                type: "GET",
+                url: "/api/Patrimonio/ListaSecoes",
                 data: { setorSelecionado: setorId },
-                success: function (res) {
-                    try {
-                        if (res && res.data && res.data.length > 0) {
-
-                            if (
-                                comboBoxSecoes.text ===
-                                '00000000-0000-0000-0000-000000000000'
-                            ) {
+                success: function (res)
+                {
+                    try
+                    {
+                        if (res && res.data && res.data.length > 0)
+                        {
+
+                            if (comboBoxSecoes.text === "00000000-0000-0000-0000-000000000000")
+                            {
                                 comboBoxSecoes.text = null;
                             }
 
                             comboBoxSecoes.dataSource = res.data;
-                            comboBoxSecoes.fields = {
-                                text: 'text',
-                                value: 'value',
-                            };
-                            comboBoxSecoes.placeholder = 'Selecione uma Seção';
-
-                            if (
-                                secaoAtual &&
-                                secaoAtual !==
-                                    '00000000-0000-0000-0000-000000000000'
-                            ) {
+                            comboBoxSecoes.fields = { text: "text", value: "value" };
+                            comboBoxSecoes.placeholder = "Selecione uma Seção";
+
+                            if (secaoAtual && secaoAtual !== "00000000-0000-0000-0000-000000000000")
+                            {
                                 comboBoxSecoes.value = secaoAtual;
-                            } else {
+                            } else
+                            {
                                 comboBoxSecoes.value = null;
                             }
 
                             comboBoxSecoes.dataBind();
-                        } else {
+                        } else
+                        {
 
                             comboBoxSecoes.dataSource = [];
                             comboBoxSecoes.value = null;
                             comboBoxSecoes.text = null;
                             comboBoxSecoes.dataBind();
 
-                            if (!secaoAtual) {
-                                AppToast.show(
-                                    'Amarelo',
-                                    'Nenhuma seção cadastrada para este setor',
-                                    2000,
-                                );
+                            if (!secaoAtual)
+                            {
+                                AppToast.show('Amarelo', 'Nenhuma seção cadastrada para este setor', 2000);
                             }
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'loadListaSecoes.success',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaSecoes.success", error);
                     }
                 },
-                error: function (error) {
-                    try {
-                        console.error('Erro ao carregar seções:', error);
-
-                        if (comboBoxSecoes) {
+                error: function (error)
+                {
+                    try
+                    {
+                        console.error("Erro ao carregar seções:", error);
+
+                        if (comboBoxSecoes)
+                        {
                             comboBoxSecoes.dataSource = [];
                             comboBoxSecoes.value = null;
                             comboBoxSecoes.text = null;
                             comboBoxSecoes.dataBind();
                         }
-                        AppToast.show(
-                            'Amarelo',
-                            'Nenhuma seção encontrada para este setor',
-                            2000,
-                        );
-                    } catch (err) {
-                        Alerta.TratamentoErroComLinha(
-                            'patrimonio.js',
-                            'loadListaSecoes.error',
-                            err,
-                        );
-                    }
-                },
+                        AppToast.show('Amarelo', 'Nenhuma seção encontrada para este setor', 2000);
+                    } catch (err)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaSecoes.error", err);
+                    }
+                }
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'loadListaSecoes',
-                error,
-            );
-        }
-    }
-
-    function setupModalHandlers() {
-        try {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "loadListaSecoes", error);
+        }
+    }
+
+    function setupModalHandlers()
+    {
+        try
+        {
             var modalNovoModelo = document.getElementById('modalNovoModelo');
             var modalNovaMarca = document.getElementById('modalNovaMarca');
 
-            if (modalNovoModelo) {
-                modalNovoModelo.addEventListener(
-                    'show.bs.modal',
-                    function (event) {
-                        try {
-                            console.log('Modal de novo modelo abrindo...');
-
-                            var cmbMarcas = getComboboxInstance('cmbMarcas');
-                            if (cmbMarcas) {
-                                var marcaSelecionada =
-                                    cmbMarcas.text || cmbMarcas.value;
-                                console.log(
-                                    'Marca selecionada (text):',
-                                    cmbMarcas.text,
-                                );
-                                console.log(
-                                    'Marca selecionada (value):',
-                                    cmbMarcas.value,
-                                );
-                                console.log('Marca final:', marcaSelecionada);
-
-                                if (marcaSelecionada) {
-                                    var campoMarca = document.getElementById(
-                                        'marcaSelecionadaModelo',
-                                    );
-                                    if (campoMarca) {
-                                        campoMarca.value = marcaSelecionada;
-                                        console.log(
-                                            'Campo preenchido com:',
-                                            marcaSelecionada,
-                                        );
-                                    } else {
-                                        console.error(
-                                            'Campo marcaSelecionadaModelo não encontrado!',
-                                        );
+            if (modalNovoModelo)
+            {
+                modalNovoModelo.addEventListener('show.bs.modal', function (event)
+                {
+                    try
+                    {
+                        console.log('Modal de novo modelo abrindo...');
+
+                        var cmbMarcas = getComboboxInstance('cmbMarcas');
+                        if (cmbMarcas)
+                        {
+                            var marcaSelecionada = cmbMarcas.text || cmbMarcas.value;
+                            console.log('Marca selecionada (text):', cmbMarcas.text);
+                            console.log('Marca selecionada (value):', cmbMarcas.value);
+                            console.log('Marca final:', marcaSelecionada);
+
+                            if (marcaSelecionada)
+                            {
+                                var campoMarca = document.getElementById('marcaSelecionadaModelo');
+                                if (campoMarca)
+                                {
+                                    campoMarca.value = marcaSelecionada;
+                                    console.log('Campo preenchido com:', marcaSelecionada);
+                                } else
+                                {
+                                    console.error('Campo marcaSelecionadaModelo não encontrado!');
+                                }
+                            } else
+                            {
+                                event.preventDefault();
+                                event.stopPropagation();
+
+                                setTimeout(function ()
+                                {
+                                    try
+                                    {
+                                        var modalInstance = bootstrap.Modal.getInstance(modalNovoModelo);
+                                        if (modalInstance)
+                                        {
+                                            modalInstance.hide();
+                                        }
+                                    } catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha("patrimonio.js", "setupModalHandlers.setTimeout", error);
                                     }
-                                } else {
-                                    event.preventDefault();
-                                    event.stopPropagation();
-
-                                    setTimeout(function () {
-                                        try {
-                                            var modalInstance =
-                                                bootstrap.Modal.getInstance(
-                                                    modalNovoModelo,
-                                                );
-                                            if (modalInstance) {
-                                                modalInstance.hide();
-                                            }
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha(
-                                                'patrimonio.js',
-                                                'setupModalHandlers.setTimeout',
-                                                error,
-                                            );
-                                        }
-                                    }, 10);
-
-                                    AppToast.show(
-                                        'Amarelo',
-                                        'Por favor, selecione uma marca primeiro.',
-                                        2000,
-                                    );
-                                }
-                            } else {
-                                console.error(
-                                    'ComboBox de marcas não encontrada!',
-                                );
+                                }, 10);
+
+                                AppToast.show('Amarelo', 'Por favor, selecione uma marca primeiro.', 2000);
                             }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'patrimonio.js',
-                                'setupModalHandlers.show',
-                                error,
-                            );
-                        }
-                    },
-                );
-            }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'setupModalHandlers',
-                error,
-            );
-        }
-    }
-
-    function setupValidation() {
-        try {
-            $('#formsPatrimonio').on('submit', function (event) {
-                try {
-                    var NPR = document.getElementsByName(
-                        'PatrimonioObj.Patrimonio.NPR',
-                    )[0].value;
+                        } else
+                        {
+                            console.error('ComboBox de marcas não encontrada!');
+                        }
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("patrimonio.js", "setupModalHandlers.show", error);
+                    }
+                });
+            }
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "setupModalHandlers", error);
+        }
+    }
+
+    function setupValidation()
+    {
+        try
+        {
+            $("#formsPatrimonio").on("submit", function (event)
+            {
+                try
+                {
+                    var NPR = document.getElementsByName("PatrimonioObj.Patrimonio.NPR")[0].value;
                     var setorId, secaoId;
 
-                    if (!edicao) {
-                        setorId = getComboboxInstance('cmbSetores')?.value;
-                        secaoId = getComboboxInstance('cmbSecoes')?.value;
-                    } else {
-                        setorId = document.getElementById('SetorId')?.value;
-                        secaoId = document.getElementById('SecaoId')?.value;
-                    }
-
-                    if (!NPR) {
+                    if (!edicao)
+                    {
+                        setorId = getComboboxInstance("cmbSetores")?.value;
+                        secaoId = getComboboxInstance("cmbSecoes")?.value;
+                    } else
+                    {
+                        setorId = document.getElementById("SetorId")?.value;
+                        secaoId = document.getElementById("SecaoId")?.value;
+                    }
+
+                    if (!NPR)
+                    {
                         event.preventDefault();
-                        Alerta.Erro(
-                            'Erro no NPR',
-                            'O NPR não pode estar em branco!',
-                            'Ok',
-                        );
+                        Alerta.Erro("Erro no NPR", "O NPR não pode estar em branco!", "Ok");
                         return false;
                     }
 
-                    if (!setorId) {
+                    if (!setorId)
+                    {
                         event.preventDefault();
-                        Alerta.Erro(
-                            'Erro no setor',
-                            'O setor não pode estar em branco!',
-                            'Ok',
-                        );
+                        Alerta.Erro("Erro no setor", "O setor não pode estar em branco!", "Ok");
                         return false;
                     }
 
-                    if (!secaoId) {
+                    if (!secaoId)
+                    {
                         event.preventDefault();
-                        Alerta.Erro(
-                            'Erro na seção',
-                            'A seção não pode estar em branco!',
-                            'Ok',
-                        );
+                        Alerta.Erro("Erro na seção", "A seção não pode estar em branco!", "Ok");
                         return false;
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'patrimonio.js',
-                        'setupValidation.submit',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("patrimonio.js", "setupValidation.submit", error);
                 }
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'setupValidation',
-                error,
-            );
-        }
-    }
-
-    window.salvarNovaMarca = function () {
-        try {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "setupValidation", error);
+        }
+    }
+
+    window.salvarNovaMarca = function ()
+    {
+        try
+        {
             var inputMarca = document.getElementById('inputNovaMarca');
             var marcaErrorMsg = document.getElementById('marcaErrorMsg');
             var novaMarca = inputMarca.value.trim();
 
-            if (!novaMarca) {
+            if (!novaMarca)
+            {
                 inputMarca.classList.add('is-invalid');
-                marcaErrorMsg.textContent =
-                    'Por favor, informe o nome da marca.';
+                marcaErrorMsg.textContent = 'Por favor, informe o nome da marca.';
                 return;
             }
 
-            if (novaMarca.length > 30) {
+            if (novaMarca.length > 30)
+            {
                 inputMarca.classList.add('is-invalid');
-                marcaErrorMsg.textContent =
-                    'O nome da marca não pode exceder 30 caracteres.';
+                marcaErrorMsg.textContent = 'O nome da marca não pode exceder 30 caracteres.';
                 return;
             }
 
             var cmbMarcas = getComboboxInstance('cmbMarcas');
-            if (!cmbMarcas) {
-                AppToast.show(
-                    'Vermelho',
-                    'Erro ao adicionar marca. Recarregue a página.',
-                    2000,
-                );
+            if (!cmbMarcas)
+            {
+                AppToast.show('Vermelho', 'Erro ao adicionar marca. Recarregue a página.', 2000);
                 return;
             }
 
             var marcaExiste = false;
-            if (cmbMarcas.dataSource && Array.isArray(cmbMarcas.dataSource)) {
-                marcaExiste = cmbMarcas.dataSource.some(function (m) {
-                    return (
-                        m.text &&
-                        m.text.toLowerCase() === novaMarca.toLowerCase()
-                    );
+            if (cmbMarcas.dataSource && Array.isArray(cmbMarcas.dataSource))
+            {
+                marcaExiste = cmbMarcas.dataSource.some(function (m)
+                {
+                    return m.text && m.text.toLowerCase() === novaMarca.toLowerCase();
                 });
             }
 
-            if (marcaExiste) {
+            if (marcaExiste)
+            {
                 inputMarca.classList.add('is-invalid');
                 marcaErrorMsg.textContent = 'Esta marca já está cadastrada.';
                 return;
             }
 
-            if (!cmbMarcas.dataSource) {
+            if (!cmbMarcas.dataSource)
+            {
                 cmbMarcas.dataSource = [];
             }
 
@@ -1679,12 +1482,14 @@
             cmbMarcas.dataBind();
 
             var btnAddModelo = document.getElementById('btnAddModelo');
-            if (btnAddModelo) {
+            if (btnAddModelo)
+            {
                 btnAddModelo.disabled = false;
             }
 
-            var comboBoxModelos = getComboboxInstance('cmbModelos');
-            if (comboBoxModelos) {
+            var comboBoxModelos = getComboboxInstance("cmbModelos");
+            if (comboBoxModelos)
+            {
                 comboBoxModelos.dataSource = [];
                 comboBoxModelos.value = null;
                 comboBoxModelos.dataBind();
@@ -1696,23 +1501,23 @@
             closeModal('modalNovaMarca');
 
             AppToast.show('Verde', 'Marca adicionada com sucesso!', 2000);
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'salvarNovaMarca',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "salvarNovaMarca", error);
         }
     };
 
-    window.salvarNovoModelo = function () {
-        try {
+    window.salvarNovoModelo = function ()
+    {
+        try
+        {
             var inputModelo = document.getElementById('inputNovoModelo');
             var modeloErrorMsg = document.getElementById('modeloErrorMsg');
             var novoModelo = inputModelo.value.trim();
 
             var cmbMarcas = getComboboxInstance('cmbMarcas');
-            if (!cmbMarcas || !cmbMarcas.value) {
+            if (!cmbMarcas || !cmbMarcas.value)
+            {
                 AppToast.show('Amarelo', 'Selecione uma marca primeiro.', 2000);
                 closeModal('modalNovoModelo');
                 return;
@@ -1720,43 +1525,45 @@
 
             var marcaSelecionada = cmbMarcas.value;
 
-            if (!novoModelo) {
+            if (!novoModelo)
+            {
                 inputModelo.classList.add('is-invalid');
-                modeloErrorMsg.textContent =
-                    'Por favor, informe o nome do modelo.';
+                modeloErrorMsg.textContent = 'Por favor, informe o nome do modelo.';
                 return;
             }
 
-            if (novoModelo.length > 30) {
+            if (novoModelo.length > 30)
+            {
                 inputModelo.classList.add('is-invalid');
-                modeloErrorMsg.textContent =
-                    'O nome do modelo não pode exceder 30 caracteres.';
+                modeloErrorMsg.textContent = 'O nome do modelo não pode exceder 30 caracteres.';
                 return;
             }
 
             var cmbModelos = getComboboxInstance('cmbModelos');
-            if (!cmbModelos) {
+            if (!cmbModelos)
+            {
                 AppToast.show('Vermelho', 'Erro ao adicionar modelo.', 2000);
                 return;
             }
 
             var modeloExiste = false;
-            if (cmbModelos.dataSource && Array.isArray(cmbModelos.dataSource)) {
-                modeloExiste = cmbModelos.dataSource.some(function (m) {
-                    return (
-                        m.text &&
-                        m.text.toLowerCase() === novoModelo.toLowerCase()
-                    );
+            if (cmbModelos.dataSource && Array.isArray(cmbModelos.dataSource))
+            {
+                modeloExiste = cmbModelos.dataSource.some(function (m)
+                {
+                    return m.text && m.text.toLowerCase() === novoModelo.toLowerCase();
                 });
             }
 
-            if (modeloExiste) {
+            if (modeloExiste)
+            {
                 inputModelo.classList.add('is-invalid');
                 modeloErrorMsg.textContent = 'Este modelo já está cadastrado.';
                 return;
             }
 
-            if (!cmbModelos.dataSource) {
+            if (!cmbModelos.dataSource)
+            {
                 cmbModelos.dataSource = [];
             }
 
@@ -1773,202 +1580,192 @@
             closeModal('modalNovoModelo');
 
             AppToast.show('Verde', 'Modelo adicionado com sucesso!', 2000);
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'salvarNovoModelo',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "salvarNovoModelo", error);
         }
     };
 
-    window.onFileSelect = function (args) {
-        try {
-            console.log('Arquivos selecionados:', args.filesData);
+    window.onFileSelect = function (args)
+    {
+        try
+        {
+            console.log("Arquivos selecionados:", args.filesData);
             var validFiles = validateFiles(args);
-            if (validFiles.length > 0) {
-                for (var i = 0; i < validFiles.length; i++) {
+            if (validFiles.length > 0)
+            {
+                for (var i = 0; i < validFiles.length; i++)
+                {
                     readURL(validFiles[i]);
                 }
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'onFileSelect',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "onFileSelect", error);
         }
     };
 
-    window.onFileRemove = function (args) {
-        try {
-            console.log('Arquivo removido:', args);
+    window.onFileRemove = function (args)
+    {
+        try
+        {
+            console.log("Arquivo removido:", args);
             args.postRawFile = false;
-            var preview = document.getElementById('previewImage');
-            if (preview) {
-                preview.style.display = 'none';
-            }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'onFileRemove',
-                error,
-            );
+            var preview = document.getElementById("previewImage");
+            if (preview)
+            {
+                preview.style.display = "none";
+            }
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "onFileRemove", error);
         }
     };
 
-    function readURL(file) {
-        try {
-            var preview = document.getElementById('previewImage');
+    function readURL(file)
+    {
+        try
+        {
+            var preview = document.getElementById("previewImage");
             var reader = new FileReader();
 
-            reader.onload = function () {
+            reader.onload = function ()
+            {
                 preview.src = reader.result;
-                preview.style.display = 'block';
+                preview.style.display = "block";
             };
 
-            if (file.rawFile) {
+            if (file.rawFile)
+            {
                 reader.readAsDataURL(file.rawFile);
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('patrimonio.js', 'readURL', error);
-        }
-    }
-
-    function validateFiles(args) {
-        try {
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "readURL", error);
+        }
+    }
+
+    function validateFiles(args)
+    {
+        try
+        {
             var validFiles = [];
-            var allImages = ['jpg', 'jpeg', 'png'];
-
-            for (var i = 0; i < args.filesData.length; i++) {
+            var allImages = ["jpg", "jpeg", "png"];
+
+            for (var i = 0; i < args.filesData.length; i++)
+            {
                 var file = args.filesData[i];
-                if (
-                    allImages.indexOf(file.type) !== -1 &&
-                    filesName.indexOf(file.name) === -1
-                ) {
+                if (allImages.indexOf(file.type) !== -1 && filesName.indexOf(file.name) === -1)
+                {
                     filesName.push(file.name);
                     validFiles.push(file);
-                } else {
-                    console.warn(
-                        'Tipo de arquivo inválido ou duplicado:',
-                        file.name,
-                    );
+                } else
+                {
+                    console.warn("Tipo de arquivo inválido ou duplicado:", file.name);
                 }
             }
 
             return validFiles;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'validateFiles',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "validateFiles", error);
             return [];
         }
     }
 }
 
-else if (path.indexOf('/patrimonio/visualizarmovimentacoes') !== -1) {
-    console.log('Página: Visualizar Movimentações');
-
-    $(document).ready(function () {
-        try {
-            var patrimonioId = document.getElementById('patrimonioId')?.value;
-            console.log('ID do patrimônio:', patrimonioId);
-
-            if (patrimonioId) {
+else if (path.indexOf("/patrimonio/visualizarmovimentacoes") !== -1)
+{
+    console.log("Página: Visualizar Movimentações");
+
+    $(document).ready(function ()
+    {
+        try
+        {
+            var patrimonioId = document.getElementById("patrimonioId")?.value;
+            console.log("ID do patrimônio:", patrimonioId);
+
+            if (patrimonioId)
+            {
                 loadGridMovimentacoes(patrimonioId);
                 setupDeleteMovimentacaoHandlers();
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'visualizarmovimentacoes.ready',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "visualizarmovimentacoes.ready", error);
         }
     });
 
-    function loadGridMovimentacoes(patrimonioId) {
-        try {
-            console.log('Carregando movimentações');
-            dataTable = $('#tblMovimentacaoPatrimonio').DataTable({
+    function loadGridMovimentacoes(patrimonioId)
+    {
+        try
+        {
+            console.log("Carregando movimentações");
+            dataTable = $("#tblMovimentacaoPatrimonio").DataTable({
                 columnDefs: [
-                    { targets: 0, className: 'text-center', width: '10%' },
-                    { targets: 1, className: 'text-left', width: '20%' },
-                    { targets: 2, className: 'text-left', width: '20%' },
-                    { targets: 3, className: 'text-center', width: '10%' },
-                    { targets: 4, className: 'text-center', width: '10%' },
-                    { targets: 5, className: 'text-center', width: '10%' },
-                    { targets: 6, className: 'text-center', width: '10%' },
-                    { targets: 7, className: 'text-right', width: '10%' },
-                    { targets: 8, className: 'text-center', width: '10%' },
+                    { targets: 0, className: "text-center", width: "10%" },
+                    { targets: 1, className: "text-left", width: "20%" },
+                    { targets: 2, className: "text-left", width: "20%" },
+                    { targets: 3, className: "text-center", width: "10%" },
+                    { targets: 4, className: "text-center", width: "10%" },
+                    { targets: 5, className: "text-center", width: "10%" },
+                    { targets: 6, className: "text-center", width: "10%" },
+                    { targets: 7, className: "text-right", width: "10%" },
+                    { targets: 8, className: "text-center", width: "10%" }
                 ],
                 responsive: true,
                 ajax: {
-                    url: '/api/Patrimonio/MovimentacaoPatrimonioGrid',
-                    type: 'GET',
-                    datatype: 'json',
+                    url: "/api/Patrimonio/MovimentacaoPatrimonioGrid",
+                    type: "GET",
+                    datatype: "json",
                     data: { patrimonioId: patrimonioId },
-                    error: function (xhr, status, error) {
-                        try {
-                            console.error(
-                                'Erro ao carregar movimentações:',
-                                error,
-                            );
-                            AppToast.show(
-                                'Vermelho',
-                                'Erro ao carregar movimentações',
-                                3000,
-                            );
-                        } catch (err) {
-                            Alerta.TratamentoErroComLinha(
-                                'patrimonio.js',
-                                'loadGridMovimentacoes.error',
-                                err,
-                            );
-                        }
-                    },
+                    error: function (xhr, status, error)
+                    {
+                        try
+                        {
+                            console.error("Erro ao carregar movimentações:", error);
+                            AppToast.show('Vermelho', 'Erro ao carregar movimentações', 3000);
+                        } catch (err)
+                        {
+                            Alerta.TratamentoErroComLinha("patrimonio.js", "loadGridMovimentacoes.error", err);
+                        }
+                    }
                 },
                 columns: [
                     {
-                        data: 'dataMovimentacao',
-                        type: 'date',
-                        render: function (data, type, row) {
-                            try {
-                                if (type === 'display' && data) {
+                        data: "dataMovimentacao",
+                        type: "date",
+                        render: function (data, type, row)
+                        {
+                            try
+                            {
+                                if (type === "display" && data)
+                                {
                                     const date = new Date(data);
-                                    const day = String(date.getDate()).padStart(
-                                        2,
-                                        '0',
-                                    );
-                                    const month = String(
-                                        date.getMonth() + 1,
-                                    ).padStart(2, '0');
+                                    const day = String(date.getDate()).padStart(2, "0");
+                                    const month = String(date.getMonth() + 1).padStart(2, "0");
                                     const year = date.getFullYear();
                                     return `${day}/${month}/${year}`;
                                 }
                                 return data;
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'patrimonio.js',
-                                    'loadGridMovimentacoes.render',
-                                    error,
-                                );
+                            } catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha("patrimonio.js", "loadGridMovimentacoes.render", error);
                                 return data;
                             }
-                        },
+                        }
                     },
-                    { data: 'npr' },
-                    { data: 'descricao' },
-                    { data: 'setorOrigemNome' },
-                    { data: 'secaoOrigemNome' },
-                    { data: 'setorDestinoNome' },
-                    { data: 'secaoDestinoNome' },
-                    { data: 'responsavelMovimentacao' },
-                    {
-                        data: 'movimentacaoPatrimonioId',
-                        render: function (data) {
+                    { data: "npr" },
+                    { data: "descricao" },
+                    { data: "setorOrigemNome" },
+                    { data: "secaoOrigemNome" },
+                    { data: "setorDestinoNome" },
+                    { data: "secaoDestinoNome" },
+                    { data: "responsavelMovimentacao" },
+                    {
+                        data: "movimentacaoPatrimonioId",
+                        render: function (data)
+                        {
                             return `<div class="text-center">
                                 <a class="btn-delete btn btn-sm btn-vinho text-white"
                                    data-ejtip="Excluir movimentação"
@@ -1977,120 +1774,101 @@
                                     <i class="far fa-trash-alt"></i>
                                 </a>
                             </div>`;
-                        },
-                    },
+                        }
+                    }
                 ],
-                order: [[0, 'desc']],
+                order: [[0, "desc"]],
                 language: {
-                    url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                    emptyTable: 'Sem movimentações encontradas',
+                    url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                    emptyTable: "Sem movimentações encontradas"
                 },
-                width: '100%',
+                width: "100%"
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'loadGridMovimentacoes',
-                error,
-            );
-        }
-    }
-
-    function setupDeleteMovimentacaoHandlers() {
-        try {
-            $(document).on('click', '.btn-delete', function () {
-                try {
-                    var id = $(this).data('id');
-                    console.log('Deletar movimentação:', id);
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "loadGridMovimentacoes", error);
+        }
+    }
+
+    function setupDeleteMovimentacaoHandlers()
+    {
+        try
+        {
+            $(document).on("click", ".btn-delete", function ()
+            {
+                try
+                {
+                    var id = $(this).data("id");
+                    console.log("Deletar movimentação:", id);
 
                     Alerta.Confirmar(
-                        'Confirmar Exclusão',
-                        'Você tem certeza que deseja apagar esta movimentação? Não será possível recuperar os dados eliminados!',
-                        'Sim, excluir',
-                        'Cancelar',
-                    ).then(function (willDelete) {
-                        try {
-                            if (willDelete) {
-                                var dataToPost = JSON.stringify({
-                                    MovimentacaoPatrimonioId: id,
-                                });
+                        "Confirmar Exclusão",
+                        "Você tem certeza que deseja apagar esta movimentação? Não será possível recuperar os dados eliminados!",
+                        "Sim, excluir",
+                        "Cancelar"
+                    ).then(function (willDelete)
+                    {
+                        try
+                        {
+                            if (willDelete)
+                            {
+                                var dataToPost = JSON.stringify({ MovimentacaoPatrimonioId: id });
                                 $.ajax({
-                                    url: '/api/Patrimonio/DeleteMovimentacaoPatrimonio',
-                                    type: 'POST',
+                                    url: "/api/Patrimonio/DeleteMovimentacaoPatrimonio",
+                                    type: "POST",
                                     data: dataToPost,
-                                    contentType:
-                                        'application/json; charset=utf-8',
-                                    dataType: 'json',
-                                    success: function (data) {
-                                        try {
-                                            if (data.success) {
-                                                AppToast.show(
-                                                    'Verde',
-                                                    data.message,
-                                                    2000,
-                                                );
+                                    contentType: "application/json; charset=utf-8",
+                                    dataType: "json",
+                                    success: function (data)
+                                    {
+                                        try
+                                        {
+                                            if (data.success)
+                                            {
+                                                AppToast.show('Verde', data.message, 2000);
                                                 dataTable.ajax.reload();
-                                            } else {
-                                                AppToast.show(
-                                                    'Vermelho',
-                                                    data.message,
-                                                    2000,
-                                                );
+                                            } else
+                                            {
+                                                AppToast.show('Vermelho', data.message, 2000);
                                             }
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha(
-                                                'patrimonio.js',
-                                                'setupDeleteMovimentacaoHandlers.success',
-                                                error,
-                                            );
+                                        } catch (error)
+                                        {
+                                            Alerta.TratamentoErroComLinha("patrimonio.js", "setupDeleteMovimentacaoHandlers.success", error);
                                         }
                                     },
-                                    error: function (err) {
-                                        try {
+                                    error: function (err)
+                                    {
+                                        try
+                                        {
                                             console.error(err);
-                                            AppToast.show(
-                                                'Erro ao deletar movimentação',
-                                                'Vermelho',
-                                                2000,
-                                            );
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha(
-                                                'patrimonio.js',
-                                                'setupDeleteMovimentacaoHandlers.error',
-                                                error,
-                                            );
+                                            AppToast.show('Erro ao deletar movimentação', 'Vermelho', 2000);
+                                        } catch (error)
+                                        {
+                                            Alerta.TratamentoErroComLinha("patrimonio.js", "setupDeleteMovimentacaoHandlers.error", error);
                                         }
-                                    },
+                                    }
                                 });
                             }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'patrimonio.js',
-                                'setupDeleteMovimentacaoHandlers.then',
-                                error,
-                            );
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("patrimonio.js", "setupDeleteMovimentacaoHandlers.then", error);
                         }
                     });
-                } catch (innerError) {
-                    Alerta.TratamentoErroComLinha(
-                        'patrimonio.js',
-                        'setupDeleteMovimentacaoHandlers.click',
-                        innerError,
-                    );
+                } catch (innerError)
+                {
+                    Alerta.TratamentoErroComLinha("patrimonio.js", "setupDeleteMovimentacaoHandlers.click", innerError);
                 }
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'patrimonio.js',
-                'setupDeleteMovimentacaoHandlers',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("patrimonio.js", "setupDeleteMovimentacaoHandlers", error);
         }
     }
 }
 
-if (typeof onMarcaChange !== 'undefined') {
+if (typeof onMarcaChange !== 'undefined')
+{
     window.onMarcaChange = onMarcaChange;
 }
 
-console.log('patrimonio.js carregado com sucesso');
+console.log("patrimonio.js carregado com sucesso");
```
