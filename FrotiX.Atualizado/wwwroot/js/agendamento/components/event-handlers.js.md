# wwwroot/js/agendamento/components/event-handlers.js

**Mudanca:** GRANDE | **+369** linhas | **-406** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/components/event-handlers.js
+++ ATUAL: wwwroot/js/agendamento/components/event-handlers.js
@@ -1,21 +1,22 @@
 window.requisitanteOriginal = {
     id: null,
     ramal: null,
-    setorId: null,
-};
-
-window.onSelectRequisitante = function (args) {
+    setorId: null
+};
+
+window.onSelectRequisitante = function (args)
+{
     console.log('🎯 Requisitante selecionado (SELECT event)!');
     console.log('📦 args:', args);
 
-    try {
-
-        const txtRamal = document.getElementById('txtRamalRequisitante');
-        const ddtSetorElement = document.getElementById(
-            'lstSetorRequisitanteAgendamento',
-        );
-
-        if (!args || !args.itemData || !args.itemData.RequisitanteId) {
+    try
+    {
+
+        const txtRamal = document.getElementById("txtRamalRequisitante");
+        const ddtSetorElement = document.getElementById("lstSetorRequisitanteAgendamento");
+
+        if (!args || !args.itemData || !args.itemData.RequisitanteId)
+        {
             console.warn('⚠️ Dados inválidos no evento select');
             return;
         }
@@ -25,70 +26,58 @@
 
         window.requisitanteOriginal.id = requisitanteId;
 
-        if (txtRamal) {
+        if (txtRamal)
+        {
 
         }
 
         $.ajax({
             url: '/Viagens/Upsert?handler=PegaRamal',
-            method: 'GET',
-            dataType: 'json',
+            method: "GET",
+            dataType: "json",
             data: { id: requisitanteId },
-            success: function (res) {
+            success: function (res)
+            {
                 console.log('📞 Resposta Ramal:', res);
 
                 const ramalValue = res.data || res;
 
-                if (
-                    ramalValue !== null &&
-                    ramalValue !== undefined &&
-                    ramalValue !== ''
-                ) {
-
-                    const ramalElement = document.getElementById(
-                        'txtRamalRequisitanteSF',
-                    );
-
-                    if (
-                        ramalElement &&
-                        ramalElement.ej2_instances &&
-                        ramalElement.ej2_instances[0]
-                    ) {
+                if (ramalValue !== null && ramalValue !== undefined && ramalValue !== '')
+                {
+
+                    const ramalElement = document.getElementById('txtRamalRequisitanteSF');
+
+                    if (ramalElement && ramalElement.ej2_instances && ramalElement.ej2_instances[0])
+                    {
                         const ramalTextBox = ramalElement.ej2_instances[0];
 
                         ramalTextBox.value = String(ramalValue);
 
                         ramalTextBox.dataBind();
 
-                        console.log(
-                            '✓ Ramal atualizado (Syncfusion):',
-                            ramalValue,
-                        );
-                    } else {
-                        console.error(
-                            '❌ TextBox Syncfusion não encontrado ou não inicializado',
-                        );
-
-                        if (ramalElement) {
+                        console.log('✓ Ramal atualizado (Syncfusion):', ramalValue);
+                    } else
+                    {
+                        console.error('❌ TextBox Syncfusion não encontrado ou não inicializado');
+
+                        if (ramalElement)
+                        {
                             ramalElement.value = ramalValue;
                         }
                     }
 
                     window.requisitanteOriginal.ramal = parseInt(ramalValue);
-                } else {
-
-                    const ramalElement = document.getElementById(
-                        'txtRamalRequisitanteSF',
-                    );
-
-                    if (
-                        ramalElement &&
-                        ramalElement.ej2_instances &&
-                        ramalElement.ej2_instances[0]
-                    ) {
+                } else
+                {
+
+                    const ramalElement = document.getElementById('txtRamalRequisitanteSF');
+
+                    if (ramalElement && ramalElement.ej2_instances && ramalElement.ej2_instances[0])
+                    {
                         ramalElement.ej2_instances[0].value = '';
                         ramalElement.ej2_instances[0].dataBind();
-                    } else if (ramalElement) {
+                    } else if (ramalElement)
+                    {
                         ramalElement.value = '';
                     }
 
@@ -96,45 +85,41 @@
                     console.warn('⚠️ Ramal não encontrado ou vazio');
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('❌ Erro ao buscar ramal:', error);
 
-                const ramalElement = document.getElementById(
-                    'txtRamalRequisitanteSF',
-                );
-                if (
-                    ramalElement &&
-                    ramalElement.ej2_instances &&
-                    ramalElement.ej2_instances[0]
-                ) {
+                const ramalElement = document.getElementById('txtRamalRequisitanteSF');
+                if (ramalElement && ramalElement.ej2_instances && ramalElement.ej2_instances[0])
+                {
                     ramalElement.ej2_instances[0].value = '';
                     ramalElement.ej2_instances[0].enabled = true;
-                } else if (ramalElement) {
+                } else if (ramalElement)
+                {
                     ramalElement.value = '';
                 }
 
                 window.requisitanteOriginal.ramal = null;
                 Alerta.Erro('Erro ao buscar ramal do requisitante');
-            },
+            }
         });
 
         $.ajax({
             url: '/Viagens/Upsert?handler=PegaSetor',
-            method: 'GET',
-            dataType: 'json',
+            method: "GET",
+            dataType: "json",
             data: { id: requisitanteId },
-            success: function (res) {
+            success: function (res)
+            {
                 console.log('🏢 Resposta Setor:', res);
 
                 const setorValue = res.data || res;
 
-                if (
-                    setorValue !== null &&
-                    setorValue !== undefined &&
-                    setorValue !== ''
-                ) {
-
-                    if (ddtSetorElement?.ej2_instances?.[0]) {
+                if (setorValue !== null && setorValue !== undefined && setorValue !== '')
+                {
+
+                    if (ddtSetorElement?.ej2_instances?.[0])
+                    {
                         const ddtSetorObj = ddtSetorElement.ej2_instances[0];
 
                         ddtSetorObj.value = [setorValue];
@@ -143,19 +128,17 @@
                         window.requisitanteOriginal.setorId = setorValue;
 
                         console.log('✓ Setor atualizado:', setorValue);
-                    } else {
-                        console.error(
-                            '❌ DropDownTree de setor não encontrado ou não inicializado',
-                        );
+                    } else
+                    {
+                        console.error('❌ DropDownTree de setor não encontrado ou não inicializado');
                         console.log('Elemento encontrado:', ddtSetorElement);
-                        console.log(
-                            'Instâncias:',
-                            ddtSetorElement?.ej2_instances,
-                        );
+                        console.log('Instâncias:', ddtSetorElement?.ej2_instances);
                     }
-                } else {
-
-                    if (ddtSetorElement?.ej2_instances?.[0]) {
+                } else
+                {
+
+                    if (ddtSetorElement?.ej2_instances?.[0])
+                    {
                         const ddtSetorObj = ddtSetorElement.ej2_instances[0];
                         ddtSetorObj.value = [];
                         ddtSetorObj.dataBind();
@@ -166,12 +149,14 @@
                     console.warn('⚠️ Setor não encontrado ou vazio');
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('❌ Erro ao buscar setor:', error);
                 console.error('Status:', status);
                 console.error('Response:', xhr.responseText);
 
-                if (ddtSetorElement?.ej2_instances?.[0]) {
+                if (ddtSetorElement?.ej2_instances?.[0])
+                {
                     const ddtSetorObj = ddtSetorElement.ej2_instances[0];
                     ddtSetorObj.value = [];
                     ddtSetorObj.dataBind();
@@ -180,25 +165,28 @@
                 window.requisitanteOriginal.setorId = null;
 
                 Alerta.Erro('Erro ao buscar setor do requisitante');
-            },
+            }
         });
-    } catch (error) {
+
+    } catch (error)
+    {
         console.error('❌ Erro na função onSelectRequisitante:', error);
         Alerta.Erro('Erro ao processar seleção do requisitante');
     }
 };
 
-window.onSelectRequisitanteEvento = function (args) {
+window.onSelectRequisitanteEvento = function (args)
+{
     console.log('🎯 Requisitante de EVENTO selecionado (SELECT event)!');
     console.log('📦 args:', args);
 
-    try {
-
-        const ddtSetorElement = document.getElementById(
-            'lstSetorRequisitanteEvento',
-        );
-
-        if (!args || !args.itemData || !args.itemData.RequisitanteId) {
+    try
+    {
+
+        const ddtSetorElement = document.getElementById("lstSetorRequisitanteEvento");
+
+        if (!args || !args.itemData || !args.itemData.RequisitanteId)
+        {
             console.warn('⚠️ Dados inválidos no evento select (Evento)');
             return;
         }
@@ -208,40 +196,37 @@
 
         $.ajax({
             url: '/Viagens/Upsert?handler=PegaSetor',
-            method: 'GET',
-            dataType: 'json',
+            method: "GET",
+            dataType: "json",
             data: { id: requisitanteId },
-            success: function (res) {
+            success: function (res)
+            {
                 console.log('🏢 Resposta Setor (Evento):', res);
 
                 const setorValue = res.data || res;
 
-                if (
-                    setorValue !== null &&
-                    setorValue !== undefined &&
-                    setorValue !== ''
-                ) {
-
-                    if (ddtSetorElement?.ej2_instances?.[0]) {
+                if (setorValue !== null && setorValue !== undefined && setorValue !== '')
+                {
+
+                    if (ddtSetorElement?.ej2_instances?.[0])
+                    {
                         const ddtSetorObj = ddtSetorElement.ej2_instances[0];
 
                         ddtSetorObj.value = [setorValue];
                         ddtSetorObj.dataBind();
 
                         console.log('✓ Setor atualizado (Evento):', setorValue);
-                    } else {
-                        console.error(
-                            '❌ DropDownTree de setor (Evento) não encontrado ou não inicializado',
-                        );
+                    } else
+                    {
+                        console.error('❌ DropDownTree de setor (Evento) não encontrado ou não inicializado');
                         console.log('Elemento encontrado:', ddtSetorElement);
-                        console.log(
-                            'Instâncias:',
-                            ddtSetorElement?.ej2_instances,
-                        );
+                        console.log('Instâncias:', ddtSetorElement?.ej2_instances);
                     }
-                } else {
-
-                    if (ddtSetorElement?.ej2_instances?.[0]) {
+                } else
+                {
+
+                    if (ddtSetorElement?.ej2_instances?.[0])
+                    {
                         const ddtSetorObj = ddtSetorElement.ej2_instances[0];
                         ddtSetorObj.value = [];
                         ddtSetorObj.dataBind();
@@ -250,455 +235,443 @@
                     console.warn('⚠️ Setor não encontrado ou vazio (Evento)');
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('❌ Erro ao buscar setor (Evento):', error);
                 console.error('Status:', status);
                 console.error('Response:', xhr.responseText);
 
-                if (ddtSetorElement?.ej2_instances?.[0]) {
+                if (ddtSetorElement?.ej2_instances?.[0])
+                {
                     const ddtSetorObj = ddtSetorElement.ej2_instances[0];
                     ddtSetorObj.value = [];
                     ddtSetorObj.dataBind();
                 }
 
                 Alerta.Erro('Erro ao buscar setor do requisitante');
-            },
+            }
         });
-    } catch (error) {
+
+    } catch (error)
+    {
         console.error('❌ Erro na função onSelectRequisitanteEvento:', error);
         Alerta.Erro('Erro ao processar seleção do requisitante do evento');
     }
 };
 
-window.lstFinalidade_Change = function (args) {
-    try {
-        console.log('📋 Finalidade mudou:', args.value, args.itemData);
-
-        const sectionEvento = document.getElementById('sectionEvento');
-        const modalEvento = document.getElementById('modalEvento');
-
-        if (!sectionEvento) {
-            console.error('❌ sectionEvento não encontrado no DOM');
-            return;
-        }
-
-        const finalidadeSelecionada =
-            args.itemData?.text || args.itemData?.Descricao || '';
-
-        console.log('🔍 Finalidade selecionada:', finalidadeSelecionada);
-
-        if (finalidadeSelecionada.toLowerCase().includes('evento')) {
-
-            sectionEvento.style.display = 'block';
-            console.log('✅ Seção de Evento exibida');
-        } else {
-
-            sectionEvento.style.display = 'none';
-
-            if (modalEvento && window.bootstrap && window.bootstrap.Modal) {
+window.lstFinalidade_Change = function (args)
+{
+    try
+    {
+        console.log("📋 Finalidade mudou:", args.value, args.itemData);
+
+        const sectionEvento = document.getElementById("sectionEvento");
+        const modalEvento = document.getElementById("modalEvento");
+
+        if (!sectionEvento)
+        {
+            console.error("❌ sectionEvento não encontrado no DOM");
+            return;
+        }
+
+        const finalidadeSelecionada = args.itemData?.text || args.itemData?.Descricao || "";
+
+        console.log("🔍 Finalidade selecionada:", finalidadeSelecionada);
+
+        if (finalidadeSelecionada.toLowerCase().includes("evento"))
+        {
+
+            sectionEvento.style.display = "block";
+            console.log("✅ Seção de Evento exibida");
+        } else
+        {
+
+            sectionEvento.style.display = "none";
+
+            if (modalEvento && window.bootstrap && window.bootstrap.Modal)
+            {
                 window.bootstrap.Modal.getOrCreateInstance(modalEvento).hide();
             }
 
-            const lstEventosElement = document.getElementById('lstEventos');
-            if (
-                lstEventosElement &&
-                lstEventosElement.ej2_instances &&
-                lstEventosElement.ej2_instances[0]
-            ) {
+            const lstEventosElement = document.getElementById("lstEventos");
+            if (lstEventosElement && lstEventosElement.ej2_instances && lstEventosElement.ej2_instances[0])
+            {
                 lstEventosElement.ej2_instances[0].value = null;
                 lstEventosElement.ej2_instances[0].dataBind();
-                console.log('✅ lstEventos limpo');
-            }
-
-            console.log('➖ Seção de Evento escondida');
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'event-handlers.js',
-            'lstFinalidade_Change',
-            error,
-        );
-    }
-};
-
-window.RequisitanteValueChange = function () {
-    try {
-
-        const comboBox = $('#lstRequisitante').data('kendoComboBox');
-
-        if (!comboBox) {
+                console.log("✅ lstEventos limpo");
+            }
+
+            console.log("➖ Seção de Evento escondida");
+        }
+
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("event-handlers.js", "lstFinalidade_Change", error);
+    }
+};
+
+window.RequisitanteValueChange = function ()
+{
+    try
+    {
+
+        const comboBox = $("#lstRequisitante").data("kendoComboBox");
+
+        if (!comboBox)
+        {
             console.warn('⚠️ lstRequisitante (Kendo ComboBox) não encontrado');
             return;
         }
 
-        if (comboBox.value() === null || comboBox.value() === '') {
+        if (comboBox.value() === null || comboBox.value() === '')
+        {
             return;
         }
 
         const requisitanteid = String(comboBox.value());
 
-        console.log(
-            'ℹ️ RequisitanteValueChange chamado (requisitante ID:',
-            requisitanteid,
-            ')',
-        );
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'event-handlers.js',
-            'RequisitanteValueChange',
-            error,
-        );
-    }
-};
-
-window.MotoristaValueChange = function () {
-    try {
-        const ddTreeObj =
-            document.getElementById('lstMotorista').ej2_instances[0];
-
-        console.log('Objeto Motorista:', ddTreeObj);
-
-        if (ddTreeObj.value === null || ddTreeObj.enabled === false) {
+        console.log('ℹ️ RequisitanteValueChange chamado (requisitante ID:', requisitanteid, ')');
+
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("event-handlers.js", "RequisitanteValueChange", error);
+    }
+};
+
+window.MotoristaValueChange = function ()
+{
+    try
+    {
+        const ddTreeObj = document.getElementById("lstMotorista").ej2_instances[0];
+
+        console.log("Objeto Motorista:", ddTreeObj);
+
+        if (ddTreeObj.value === null || ddTreeObj.enabled === false)
+        {
             return;
         }
 
         const motoristaid = String(ddTreeObj.value);
         return motoristaid;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'event-handlers.js',
-            'MotoristaValueChange',
-            error,
-        );
-    }
-};
-
-window.VeiculoValueChange = function () {
-    try {
-        const ddTreeObj =
-            document.getElementById('lstVeiculo').ej2_instances[0];
-
-        console.log('Objeto Veículo:', ddTreeObj);
-
-        if (ddTreeObj.value === null || ddTreeObj.enabled === false) {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("event-handlers.js", "MotoristaValueChange", error);
+    }
+};
+
+window.VeiculoValueChange = function ()
+{
+    try
+    {
+        const ddTreeObj = document.getElementById("lstVeiculo").ej2_instances[0];
+
+        console.log("Objeto Veículo:", ddTreeObj);
+
+        if (ddTreeObj.value === null || ddTreeObj.enabled === false)
+        {
             return;
         }
 
         const veiculoid = String(ddTreeObj.value);
 
         $.ajax({
-            url: '/Viagens/Upsert?handler=PegaKmAtualVeiculo',
-            method: 'GET',
-            datatype: 'json',
+            url: "/Viagens/Upsert?handler=PegaKmAtualVeiculo",
+            method: "GET",
+            datatype: "json",
             data: { id: veiculoid },
-            success: function (res) {
+            success: function (res)
+            {
                 const km = res.data;
-                const kmAtual = document.getElementById('txtKmAtual');
+                const kmAtual = document.getElementById("txtKmAtual");
                 kmAtual.value = km;
             },
-            error: function (jqXHR, textStatus, errorThrown) {
-                const erro = window.criarErroAjax(
-                    jqXHR,
-                    textStatus,
-                    errorThrown,
-                    this,
-                );
-                Alerta.TratamentoErroComLinha(
-                    'event-handlers.js',
-                    'VeiculoValueChange',
-                    erro,
-                );
+            error: function (jqXHR, textStatus, errorThrown)
+            {
+                const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
+                Alerta.TratamentoErroComLinha("event-handlers.js", "VeiculoValueChange", erro);
+            }
+        });
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("event-handlers.js", "VeiculoValueChange", error);
+    }
+};
+
+window.RequisitanteEventoValueChange = function ()
+{
+    try
+    {
+        const ddTreeObj = document.getElementById("lstRequisitanteEvento").ej2_instances[0];
+
+        if (ddTreeObj.value === null || ddTreeObj.value === '')
+        {
+            return;
+        }
+
+        const requisitanteid = String(ddTreeObj.value);
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=PegaSetor",
+            method: "GET",
+            datatype: "json",
+            data: { id: requisitanteid },
+            success: function (res)
+            {
+                document.getElementById("ddtSetorEvento").ej2_instances[0].value = [res.data];
             },
+            error: function (jqXHR, textStatus, errorThrown)
+            {
+                const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
+                Alerta.TratamentoErroComLinha("event-handlers.js", "RequisitanteEventoValueChange", erro);
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'event-handlers.js',
-            'VeiculoValueChange',
-            error,
-        );
-    }
-};
-
-window.RequisitanteEventoValueChange = function () {
-    try {
-        const ddTreeObj = document.getElementById('lstRequisitanteEvento')
-            .ej2_instances[0];
-
-        if (ddTreeObj.value === null || ddTreeObj.value === '') {
-            return;
-        }
-
-        const requisitanteid = String(ddTreeObj.value);
-
-        $.ajax({
-            url: '/Viagens/Upsert?handler=PegaSetor',
-            method: 'GET',
-            datatype: 'json',
-            data: { id: requisitanteid },
-            success: function (res) {
-                document.getElementById(
-                    'ddtSetorEvento',
-                ).ej2_instances[0].value = [res.data];
-            },
-            error: function (jqXHR, textStatus, errorThrown) {
-                const erro = window.criarErroAjax(
-                    jqXHR,
-                    textStatus,
-                    errorThrown,
-                    this,
-                );
-                Alerta.TratamentoErroComLinha(
-                    'event-handlers.js',
-                    'RequisitanteEventoValueChange',
-                    erro,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'event-handlers.js',
-            'RequisitanteEventoValueChange',
-            error,
-        );
-    }
-};
-
-window.onDateChange = function (args) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("event-handlers.js", "RequisitanteEventoValueChange", error);
+    }
+};
+
+window.onDateChange = function (args)
+{
+    try
+    {
         const selectedDates = args.model.values;
 
         const listbox = document.getElementById('selectedDates');
         listbox.innerHTML = '';
 
-        selectedDates.forEach(function (date) {
+        selectedDates.forEach(function (date)
+        {
             const li = document.createElement('li');
             li.textContent = new Date(date).toLocaleDateString();
             listbox.appendChild(li);
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'event-handlers.js',
-            'onDateChange',
-            error,
-        );
-    }
-};
-
-function inicializarEventoSelect() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("event-handlers.js", "onDateChange", error);
+    }
+};
+
+function inicializarEventoSelect()
+{
+    try
+    {
 
         const lstEventosElement = document.getElementById('lstEventos');
 
-        if (!lstEventosElement) {
-            console.warn('⚠️ ComboBox lstEventos não encontrado');
+        if (!lstEventosElement)
+        {
+            console.warn("⚠️ ComboBox lstEventos não encontrado");
             return;
         }
 
         const lstEventos = ej.base.getComponent(lstEventosElement, 'combobox');
 
-        if (!lstEventos) {
-            console.warn('⚠️ Instância do ComboBox lstEventos não encontrada');
-            return;
-        }
-
-        lstEventos.select = function (args) {
-            if (args.itemData) {
+        if (!lstEventos)
+        {
+            console.warn("⚠️ Instância do ComboBox lstEventos não encontrada");
+            return;
+        }
+
+        lstEventos.select = function (args)
+        {
+            if (args.itemData)
+            {
 
                 exibirDadosEvento(args.itemData);
             }
         };
 
-        lstEventos.clearing = function (args) {
+        lstEventos.clearing = function (args)
+        {
 
             ocultarDadosEvento();
         };
 
-        console.log('✅ Handler de seleção de evento inicializado');
-    } catch (error) {
-        console.error('❌ Erro ao inicializar handler de evento:', error);
+        console.log("✅ Handler de seleção de evento inicializado");
+
+    } catch (error)
+    {
+        console.error("❌ Erro ao inicializar handler de evento:", error);
     }
 }
 
-function exibirDadosEvento(eventoData) {
-    try {
-        console.log('📋 Exibindo dados do evento:', eventoData);
-        console.log(
-            '🔍 Estrutura completa do objeto:',
-            JSON.stringify(eventoData, null, 2),
-        );
+function exibirDadosEvento(eventoData)
+{
+    try
+    {
+        console.log("📋 Exibindo dados do evento:", eventoData);
+        console.log("🔍 Estrutura completa do objeto:", JSON.stringify(eventoData, null, 2));
 
         const divDados = document.getElementById('divDadosEventoSelecionado');
-        if (divDados) {
+        if (divDados)
+        {
             divDados.style.display = 'flex';
         }
 
         const eventoId = eventoData.EventoId || eventoData.eventoId;
-        console.log('🆔 EventoId:', eventoId);
-
-        if (eventoId) {
+        console.log("🆔 EventoId:", eventoId);
+
+        if (eventoId)
+        {
 
             $.ajax({
                 url: '/api/ViagemEvento/ObterPorId',
                 method: 'GET',
                 data: { id: eventoId },
-                success: function (response) {
-                    console.log(
-                        '✅ Dados do evento recebidos da API:',
-                        response,
-                    );
-
-                    if (response.success && response.data) {
+                success: function (response)
+                {
+                    console.log("✅ Dados do evento recebidos da API:", response);
+
+                    if (response.success && response.data)
+                    {
                         preencherCamposEvento(response.data);
-                    } else {
-                        console.warn(
-                            '⚠️ Resposta da API sem dados, usando itemData...',
-                        );
+                    } else
+                    {
+                        console.warn("⚠️ Resposta da API sem dados, usando itemData...");
                         preencherCamposEvento(eventoData);
                     }
                 },
-                error: function (xhr, status, error) {
-                    console.error('❌ Erro ao buscar dados do evento:', error);
-                    console.log('⚠️ Tentando usar dados do itemData...');
+                error: function (xhr, status, error)
+                {
+                    console.error("❌ Erro ao buscar dados do evento:", error);
+                    console.log("⚠️ Tentando usar dados do itemData...");
                     preencherCamposEvento(eventoData);
-                },
+                }
             });
-        } else {
-            console.log(
-                '⚠️ EventoId não encontrado, usando dados do itemData...',
-            );
+        } else
+        {
+            console.log("⚠️ EventoId não encontrado, usando dados do itemData...");
             preencherCamposEvento(eventoData);
         }
-    } catch (error) {
-        console.error('❌ Erro ao exibir dados do evento:', error);
+
+    } catch (error)
+    {
+        console.error("❌ Erro ao exibir dados do evento:", error);
     }
 }
 
-function preencherCamposEvento(dados) {
-    try {
-        console.log('📝 Preenchendo campos com:', dados);
-
-        const dataInicial =
-            dados.DataInicial || dados.dataInicial || dados.DataInicialEvento;
-        if (dataInicial) {
-            const dtInicio = ej.base.getComponent(
-                document.getElementById('txtDataInicioEvento'),
-                'datepicker',
-            );
-            if (dtInicio) {
+function preencherCamposEvento(dados)
+{
+    try
+    {
+        console.log("📝 Preenchendo campos com:", dados);
+
+        const dataInicial = dados.DataInicial || dados.dataInicial || dados.DataInicialEvento;
+        if (dataInicial)
+        {
+            const dtInicio = ej.base.getComponent(document.getElementById('txtDataInicioEvento'), 'datepicker');
+            if (dtInicio)
+            {
                 dtInicio.value = new Date(dataInicial);
-                console.log('✅ Data Início preenchida:', dataInicial);
-            }
-        } else {
-            console.warn('⚠️ Data Inicial não encontrada no objeto');
-        }
-
-        const dataFinal =
-            dados.DataFinal || dados.dataFinal || dados.DataFinalEvento;
-        if (dataFinal) {
-            const dtFim = ej.base.getComponent(
-                document.getElementById('txtDataFimEvento'),
-                'datepicker',
-            );
-            if (dtFim) {
+                console.log("✅ Data Início preenchida:", dataInicial);
+            }
+        } else
+        {
+            console.warn("⚠️ Data Inicial não encontrada no objeto");
+        }
+
+        const dataFinal = dados.DataFinal || dados.dataFinal || dados.DataFinalEvento;
+        if (dataFinal)
+        {
+            const dtFim = ej.base.getComponent(document.getElementById('txtDataFimEvento'), 'datepicker');
+            if (dtFim)
+            {
                 dtFim.value = new Date(dataFinal);
-                console.log('✅ Data Fim preenchida:', dataFinal);
-            }
-        } else {
-            console.warn('⚠️ Data Final não encontrada no objeto');
-        }
-
-        const qtdParticipantes =
-            dados.QtdParticipantes || dados.qtdParticipantes;
-        console.log(
-            '🔍 Tentando preencher QtdParticipantes com valor:',
-            qtdParticipantes,
-        );
-
-        if (qtdParticipantes !== undefined && qtdParticipantes !== null) {
-            const numParticipantes = $('#txtQtdParticipantesEvento').data(
-                'kendoNumericTextBox',
-            );
-            if (numParticipantes) {
-                numParticipantes.value(qtdParticipantes);
-                console.log(
-                    '✅ Qtd Participantes preenchida:',
-                    qtdParticipantes,
-                );
-            } else {
-                console.error(
-                    '❌ Componente NumericTextBox Kendo não encontrado!',
-                );
-            }
-        } else {
-            console.warn(
-                '⚠️ QtdParticipantes não encontrado no objeto. Valor recebido:',
-                qtdParticipantes,
-            );
-            console.log('📋 Objeto completo recebido:', dados);
-        }
-
-        console.log('✅ Dados do evento preenchidos com sucesso');
-    } catch (error) {
-        console.error('❌ Erro ao preencher campos do evento:', error);
+                console.log("✅ Data Fim preenchida:", dataFinal);
+            }
+        } else
+        {
+            console.warn("⚠️ Data Final não encontrada no objeto");
+        }
+
+        const qtdParticipantes = dados.QtdParticipantes || dados.qtdParticipantes;
+        console.log("🔍 Tentando preencher QtdParticipantes com valor:", qtdParticipantes);
+
+        if (qtdParticipantes !== undefined && qtdParticipantes !== null)
+        {
+            const numParticipantes = ej.base.getComponent(document.getElementById('txtQtdParticipantesEvento'), 'numerictextbox');
+            if (numParticipantes)
+            {
+                numParticipantes.value = qtdParticipantes;
+                console.log("✅ Qtd Participantes preenchida:", qtdParticipantes);
+            } else
+            {
+                console.error("❌ Componente NumericTextBox não encontrado!");
+            }
+        } else
+        {
+            console.warn("⚠️ QtdParticipantes não encontrado no objeto. Valor recebido:", qtdParticipantes);
+            console.log("📋 Objeto completo recebido:", dados);
+        }
+
+        console.log("✅ Dados do evento preenchidos com sucesso");
+
+    } catch (error)
+    {
+        console.error("❌ Erro ao preencher campos do evento:", error);
     }
 }
 
-function ocultarDadosEvento() {
-    try {
-        console.log('🙈 Ocultando dados do evento');
+function ocultarDadosEvento()
+{
+    try
+    {
+        console.log("🙈 Ocultando dados do evento");
 
         const divDados = document.getElementById('divDadosEventoSelecionado');
-        if (divDados) {
+        if (divDados)
+        {
             divDados.style.display = 'none';
         }
 
-        const dtInicio = ej.base.getComponent(
-            document.getElementById('txtDataInicioEvento'),
-            'datepicker',
-        );
-        if (dtInicio) {
+        const dtInicio = ej.base.getComponent(document.getElementById('txtDataInicioEvento'), 'datepicker');
+        if (dtInicio)
+        {
             dtInicio.value = null;
         }
 
-        const dtFim = ej.base.getComponent(
-            document.getElementById('txtDataFimEvento'),
-            'datepicker',
-        );
-        if (dtFim) {
+        const dtFim = ej.base.getComponent(document.getElementById('txtDataFimEvento'), 'datepicker');
+        if (dtFim)
+        {
             dtFim.value = null;
         }
 
-        const numParticipantes = $('#txtQtdParticipantesEvento').data(
-            'kendoNumericTextBox',
-        );
-        if (numParticipantes) {
-            numParticipantes.value(null);
-        }
-
-        console.log('✅ Dados do evento limpos');
-    } catch (error) {
-        console.error('❌ Erro ao ocultar dados do evento:', error);
+        const numParticipantes = ej.base.getComponent(document.getElementById('txtQtdParticipantesEvento'), 'numerictextbox');
+        if (numParticipantes)
+        {
+            numParticipantes.value = null;
+        }
+
+        console.log("✅ Dados do evento limpos");
+
+    } catch (error)
+    {
+        console.error("❌ Erro ao ocultar dados do evento:", error);
     }
 }
 
-window.onLstMotoristaCreated = function () {
-    try {
+window.onLstMotoristaCreated = function ()
+{
+    try
+    {
         console.log('🎯 onLstMotoristaCreated chamado');
 
         const combo = document.getElementById('lstMotorista');
 
-        if (!combo || !combo.ej2_instances || !combo.ej2_instances[0]) {
+        if (!combo || !combo.ej2_instances || !combo.ej2_instances[0])
+        {
             console.warn('❌ lstMotorista não encontrado');
             return;
         }
 
         const comboInstance = combo.ej2_instances[0];
 
-        comboInstance.itemTemplate = function (data) {
-            let imgSrc =
-                data.FotoBase64 && data.FotoBase64.startsWith('data:image')
-                    ? data.FotoBase64
-                    : '/images/barbudo.jpg';
+        comboInstance.itemTemplate = function (data)
+        {
+            let imgSrc = (data.FotoBase64 && data.FotoBase64.startsWith('data:image'))
+                ? data.FotoBase64
+                : '/images/barbudo.jpg';
 
             return `
                 <div class="d-flex align-items-center">
@@ -710,13 +683,13 @@
                 </div>`;
         };
 
-        comboInstance.valueTemplate = function (data) {
+        comboInstance.valueTemplate = function (data)
+        {
             if (!data) return '';
 
-            let imgSrc =
-                data.FotoBase64 && data.FotoBase64.startsWith('data:image')
-                    ? data.FotoBase64
-                    : '/images/barbudo.jpg';
+            let imgSrc = (data.FotoBase64 && data.FotoBase64.startsWith('data:image'))
+                ? data.FotoBase64
+                : '/images/barbudo.jpg';
 
             return `
                 <div class="d-flex align-items-center">
@@ -728,15 +701,14 @@
                 </div>`;
         };
 
-        console.log('✅ Templates de motorista configurados com sucesso');
-    } catch (error) {
+        console.log("✅ Templates de motorista configurados com sucesso");
+
+    } catch (error)
+    {
         console.error('❌ Erro:', error);
-        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'event-handlers.js',
-                'onLstMotoristaCreated',
-                error,
-            );
+        if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("event-handlers.js", "onLstMotoristaCreated", error);
         }
     }
 };
```
