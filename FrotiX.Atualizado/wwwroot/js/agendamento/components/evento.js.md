# wwwroot/js/agendamento/components/evento.js

**Mudanca:** GRANDE | **+542** linhas | **-582** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/components/evento.js
+++ ATUAL: wwwroot/js/agendamento/components/evento.js
@@ -1,5 +1,6 @@
-function inicializarSistemaEvento() {
-    console.log('🎯 Inicializando Sistema de Evento...');
+function inicializarSistemaEvento()
+{
+    console.log("🎯 Inicializando Sistema de Evento...");
 
     configurarMonitoramentoFinalidade();
 
@@ -9,170 +10,178 @@
 
     configurarRequisitanteEvento();
 
-    console.log('✅ Sistema de Evento inicializado!');
-}
-
-function obterModalBootstrap(modalId) {
+    console.log("✅ Sistema de Evento inicializado!");
+}
+
+function obterModalBootstrap(modalId)
+{
     const modalEl = document.getElementById(modalId);
-    if (!modalEl || !window.bootstrap || !window.bootstrap.Modal) {
+    if (!modalEl || !window.bootstrap || !window.bootstrap.Modal)
+    {
         return null;
     }
 
     return window.bootstrap.Modal.getOrCreateInstance(modalEl);
 }
 
-function mostrarModalFallback(modalId) {
+function mostrarModalFallback(modalId)
+{
     const modal = obterModalBootstrap(modalId);
-    if (modal) {
+    if (modal)
+    {
         modal.show();
         return true;
     }
 
-    if (window.jQuery && typeof window.jQuery.fn.modal === 'function') {
-        window.jQuery(`#${modalId}`).modal('show');
+    if (window.jQuery && typeof window.jQuery.fn.modal === "function")
+    {
+        window.jQuery(`#${modalId}`).modal("show");
         return true;
     }
 
     return false;
 }
 
-function fecharModalFallback(modalId) {
+function fecharModalFallback(modalId)
+{
     const modal = obterModalBootstrap(modalId);
-    if (modal) {
+    if (modal)
+    {
         modal.hide();
         return true;
     }
 
-    if (window.jQuery && typeof window.jQuery.fn.modal === 'function') {
-        window.jQuery(`#${modalId}`).modal('hide');
+    if (window.jQuery && typeof window.jQuery.fn.modal === "function")
+    {
+        window.jQuery(`#${modalId}`).modal("hide");
         return true;
     }
 
     return false;
 }
 
-function obterValorDataEvento(input) {
-    try {
-
-        const picker = $(input).data('kendoDatePicker');
-        if (picker && picker.value()) {
+function obterValorDataEvento(input)
+{
+    try
+    {
+
+        const picker = $(input).data("kendoDatePicker");
+        if (picker && picker.value())
+        {
             return picker.value();
         }
 
-        if (!input || !input.value) {
+        if (!input || !input.value)
+        {
             return null;
         }
 
         const parsed = new Date(input.value);
         return Number.isNaN(parsed.getTime()) ? null : parsed;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'evento.js',
-            'obterValorDataEvento',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("evento.js", "obterValorDataEvento", error);
         return null;
     }
 }
 
-function limparValorDataEvento(input) {
-    try {
-
-        const picker = $(input).data('kendoDatePicker');
-        if (picker) {
+function limparValorDataEvento(input)
+{
+    try
+    {
+
+        const picker = $(input).data("kendoDatePicker");
+        if (picker)
+        {
             picker.value(null);
             return;
         }
 
-        if (input) {
-            input.value = '';
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'evento.js',
-            'limparValorDataEvento',
-            error,
-        );
-    }
-}
-
-function configurarMonitoramentoFinalidade() {
-    const lstFinalidade = document.getElementById('lstFinalidade');
-
-    if (!lstFinalidade) {
-        console.warn('⚠️ lstFinalidade não encontrado');
+        if (input)
+        {
+            input.value = "";
+        }
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("evento.js", "limparValorDataEvento", error);
+    }
+}
+
+function configurarMonitoramentoFinalidade()
+{
+    const lstFinalidade = document.getElementById("lstFinalidade");
+
+    if (!lstFinalidade)
+    {
+        console.warn("⚠️ lstFinalidade não encontrado");
         return;
     }
 
-    if (lstFinalidade.ej2_instances && lstFinalidade.ej2_instances[0]) {
+    if (lstFinalidade.ej2_instances && lstFinalidade.ej2_instances[0])
+    {
         const dropdown = lstFinalidade.ej2_instances[0];
 
-        dropdown.select = function (args) {
-            console.log(
-                '🎯 Finalidade SELECIONADA (select event):',
-                args.itemData,
-            );
-
-            const finalidade =
-                args.itemData?.text ||
-                args.itemData?.Descricao ||
-                args.itemData?.FinalidadeId ||
-                '';
-
-            console.log('🔍 Processando:', finalidade);
+        dropdown.select = function (args)
+        {
+            console.log("🎯 Finalidade SELECIONADA (select event):", args.itemData);
+
+            const finalidade = args.itemData?.text || args.itemData?.Descricao || args.itemData?.FinalidadeId || "";
+
+            console.log("🔍 Processando:", finalidade);
             controlarVisibilidadeSecaoEvento(finalidade);
         };
 
-        dropdown.change = function (args) {
-            console.log('🔄 Finalidade mudou (change event):', args.value);
+        dropdown.change = function (args)
+        {
+            console.log("🔄 Finalidade mudou (change event):", args.value);
             controlarVisibilidadeSecaoEvento(args.value);
         };
 
-        console.log('✅ Listener de Finalidade configurado (SELECT + CHANGE)');
+        console.log("✅ Listener de Finalidade configurado (SELECT + CHANGE)");
 
         const valorAtual = dropdown.value;
-        if (valorAtual) {
+        if (valorAtual)
+        {
             controlarVisibilidadeSecaoEvento(valorAtual);
         }
-    } else {
-        console.warn('⚠️ lstFinalidade não é componente EJ2');
-    }
-}
-
-function configurarRequisitanteEvento() {
-    console.log('🔧 === INÍCIO configurarRequisitanteEvento ===');
-
-    const tentarConfigurar = (tentativa = 1) => {
-        console.log(
-            `🔄 Tentativa ${tentativa} de configurar requisitante de evento...`,
-        );
-
-        const lstRequisitanteEvento = document.getElementById(
-            'lstRequisitanteEvento',
-        );
-
-        if (!lstRequisitanteEvento) {
-            console.warn(
-                `⚠️ lstRequisitanteEvento não encontrado no DOM (tentativa ${tentativa})`,
-            );
-
-            if (tentativa < 5) {
+    } else
+    {
+        console.warn("⚠️ lstFinalidade não é componente EJ2");
+    }
+}
+
+function configurarRequisitanteEvento()
+{
+    console.log("🔧 === INÍCIO configurarRequisitanteEvento ===");
+
+    const tentarConfigurar = (tentativa = 1) =>
+    {
+        console.log(`🔄 Tentativa ${tentativa} de configurar requisitante de evento...`);
+
+        const lstRequisitanteEvento = document.getElementById("lstRequisitanteEvento");
+
+        if (!lstRequisitanteEvento)
+        {
+            console.warn(`⚠️ lstRequisitanteEvento não encontrado no DOM (tentativa ${tentativa})`);
+
+            if (tentativa < 5)
+            {
                 console.log(` ⏰ Tentando novamente em 300ms...`);
                 setTimeout(() => tentarConfigurar(tentativa + 1), 300);
-            } else {
-                console.error(
-                    '❌ lstRequisitanteEvento não encontrado após 5 tentativas',
-                );
             }
+            else
+            {
+                console.error('❌ lstRequisitanteEvento não encontrado após 5 tentativas');
+            }
             return;
         }
 
         console.log('✅ Elemento lstRequisitanteEvento encontrado');
 
-        if (
-            lstRequisitanteEvento.ej2_instances &&
-            lstRequisitanteEvento.ej2_instances[0]
-        ) {
+        if (lstRequisitanteEvento.ej2_instances && lstRequisitanteEvento.ej2_instances[0])
+        {
             const dropdown = lstRequisitanteEvento.ej2_instances[0];
 
             console.log('✅ Componente Syncfusion encontrado:');
@@ -181,57 +190,59 @@
             console.log(' - Text atual:', dropdown.text);
             console.log(' - DataSource:', dropdown.dataSource);
 
-            if (dropdown.select) {
+            if (dropdown.select)
+            {
                 console.log('⚠️ Listener select já existe, será substituído');
             }
 
-            dropdown.select = function (args) {
-                console.log(
-                    '🔔 [LISTENER] Select disparado no lstRequisitanteEvento:',
-                );
+            dropdown.select = function (args)
+            {
+                console.log('🔔 [LISTENER] Select disparado no lstRequisitanteEvento:');
                 console.log(' - isInteraction:', args.isInteraction);
                 console.log(' - itemData:', args.itemData);
                 console.log(' - value:', args.e?.target?.value);
 
-                if (typeof window.onSelectRequisitanteEvento === 'function') {
+                if (typeof window.onSelectRequisitanteEvento === 'function')
+                {
                     window.onSelectRequisitanteEvento(args);
                 }
             };
 
             console.log('✅ Listener de select configurado com sucesso!');
             console.log('🔧 === FIM configurarRequisitanteEvento ===');
-        } else {
-            console.warn(
-                `⚠️ lstRequisitanteEvento não é componente Syncfusion (tentativa ${tentativa})`,
-            );
-
-            if (tentativa < 5) {
+        }
+        else
+        {
+            console.warn(`⚠️ lstRequisitanteEvento não é componente Syncfusion (tentativa ${tentativa})`);
+
+            if (tentativa < 5)
+            {
                 console.log(` ⏰ Tentando novamente em 300ms...`);
                 setTimeout(() => tentarConfigurar(tentativa + 1), 300);
-            } else {
-                console.error(
-                    '❌ lstRequisitanteEvento não inicializado após 5 tentativas',
-                );
-                console.log(
-                    '🔧 === FIM configurarRequisitanteEvento (FALHOU) ===',
-                );
             }
+            else
+            {
+                console.error('❌ lstRequisitanteEvento não inicializado após 5 tentativas');
+                console.log('🔧 === FIM configurarRequisitanteEvento (FALHOU) ===');
+            }
         }
     };
 
     tentarConfigurar();
 }
 
-window.onSelectRequisitanteEvento = function (args) {
+window.onSelectRequisitanteEvento = function (args)
+{
     console.log('🎯 Requisitante de Evento selecionado!');
     console.log(' itemData:', args.itemData);
 
-    try {
-
-        const requisitanteId =
-            args.itemData?.id || args.itemData?.RequisitanteId;
-
-        if (!args || !args.itemData || !requisitanteId) {
+    try
+    {
+
+        const requisitanteId = args.itemData?.id || args.itemData?.RequisitanteId;
+
+        if (!args || !args.itemData || !requisitanteId)
+        {
             console.warn('⚠️ Dados inválidos do requisitante');
             console.log(' id:', args.itemData?.id);
             console.log(' RequisitanteId:', args.itemData?.RequisitanteId);
@@ -241,439 +252,392 @@
         console.log('✅ Requisitante ID:', requisitanteId);
 
         $.ajax({
-            url: '/Viagens/Upsert?handler=PegaSetor',
-            method: 'GET',
-            dataType: 'json',
+            url: "/Viagens/Upsert?handler=PegaSetor",
+            method: "GET",
+            dataType: "json",
             data: { id: requisitanteId },
-            success: function (res) {
+            success: function (res)
+            {
                 console.log('📦 Resposta do servidor (Setor):', res);
 
-                try {
+                try
+                {
 
                     const setorId = res.data || (res.success && res.data);
 
-                    if (setorId) {
-
-                        const txtSetorEvento = document.getElementById(
-                            'txtSetorRequisitanteEvento',
-                        );
-                        const lstSetorEvento = document.getElementById(
-                            'lstSetorRequisitanteEvento',
-                        );
-
-                        if (!txtSetorEvento || !lstSetorEvento) {
-                            console.error(
-                                '❌ Campos de setor não encontrados no DOM',
-                            );
+                    if (setorId)
+                    {
+
+                        const txtSetorEvento = document.getElementById("txtSetorRequisitanteEvento");
+                        const lstSetorEvento = document.getElementById("lstSetorRequisitanteEvento");
+
+                        if (!txtSetorEvento || !lstSetorEvento)
+                        {
+                            console.error('❌ Campos de setor não encontrados no DOM');
                             return;
                         }
 
                         $.ajax({
-                            url: '/Viagens/Upsert?handler=AJAXPreencheListaSetores',
-                            method: 'GET',
-                            dataType: 'json',
-                            success: function (resSetores) {
-                                console.log(
-                                    '📋 Lista de setores recebida:',
-                                    resSetores,
-                                );
-                                console.log(
-                                    '🔍 Procurando SetorId:',
-                                    setorId,
-                                    '(tipo:',
-                                    typeof setorId,
-                                    ')',
-                                );
+                            url: "/Viagens/Upsert?handler=AJAXPreencheListaSetores",
+                            method: "GET",
+                            dataType: "json",
+                            success: function (resSetores)
+                            {
+                                console.log('📋 Lista de setores recebida:', resSetores);
+                                console.log('🔍 Procurando SetorId:', setorId, '(tipo:', typeof setorId, ')');
 
                                 const setores = resSetores.data || [];
-                                console.log(
-                                    '📊 Total de setores na lista:',
-                                    setores.length,
-                                );
+                                console.log('📊 Total de setores na lista:', setores.length);
 
                                 if (setores.length > 0) {
-                                    console.log(
-                                        '📄 Exemplo de setor na lista:',
-                                        setores[0],
-                                    );
-                                    console.log(
-                                        '📄 Campos disponíveis:',
-                                        Object.keys(setores[0]),
-                                    );
+                                    console.log('📄 Exemplo de setor na lista:', setores[0]);
+                                    console.log('📄 Campos disponíveis:', Object.keys(setores[0]));
                                 }
 
-                                const setorIdNormalizado = setorId
-                                    .toString()
-                                    .toLowerCase();
-                                console.log(
-                                    '🔧 SetorId normalizado:',
-                                    setorIdNormalizado,
-                                );
-
-                                const setorEncontrado = setores.find((s) => {
+                                const setorIdNormalizado = setorId.toString().toLowerCase();
+                                console.log('🔧 SetorId normalizado:', setorIdNormalizado);
+
+                                const setorEncontrado = setores.find(s => {
                                     if (!s.setorSolicitanteId) return false;
-                                    const idNormalizado = s.setorSolicitanteId
-                                        .toString()
-                                        .toLowerCase();
-                                    console.log(
-                                        ' 🔎 Comparando:',
-                                        idNormalizado,
-                                        '===',
-                                        setorIdNormalizado,
-                                        '?',
-                                        idNormalizado === setorIdNormalizado,
-                                    );
+                                    const idNormalizado = s.setorSolicitanteId.toString().toLowerCase();
+                                    console.log(' 🔎 Comparando:', idNormalizado, '===', setorIdNormalizado, '?', idNormalizado === setorIdNormalizado);
                                     return idNormalizado === setorIdNormalizado;
                                 });
 
-                                console.log(
-                                    '🔍 Setor encontrado?',
-                                    setorEncontrado,
-                                );
-
-                                if (setorEncontrado) {
+                                console.log('🔍 Setor encontrado?', setorEncontrado);
+
+                                if (setorEncontrado)
+                                {
 
                                     txtSetorEvento.value = setorEncontrado.nome;
 
                                     lstSetorEvento.value = setorId;
 
-                                    console.log(
-                                        '✅ Setor atualizado:',
-                                        setorEncontrado.nome,
-                                        '(',
-                                        setorId,
-                                        ')',
-                                    );
-                                } else {
-                                    console.warn(
-                                        '⚠️ Setor não encontrado na lista:',
-                                        setorId,
-                                    );
-                                    txtSetorEvento.value =
-                                        'Setor não identificado';
+                                    console.log('✅ Setor atualizado:', setorEncontrado.nome, '(', setorId, ')');
+                                }
+                                else
+                                {
+                                    console.warn('⚠️ Setor não encontrado na lista:', setorId);
+                                    txtSetorEvento.value = 'Setor não identificado';
                                     lstSetorEvento.value = setorId;
                                 }
                             },
-                            error: function (xhr, status, error) {
-                                console.error(
-                                    '❌ Erro ao buscar lista de setores:',
-                                    error,
-                                );
+                            error: function (xhr, status, error)
+                            {
+                                console.error('❌ Erro ao buscar lista de setores:', error);
                                 txtSetorEvento.value = 'Erro ao buscar setor';
                                 lstSetorEvento.value = setorId;
-                            },
+                            }
                         });
-                    } else {
+                    }
+                    else
+                    {
                         console.warn('⚠️ Setor não encontrado na resposta');
 
-                        const txtSetorEvento = document.getElementById(
-                            'txtSetorRequisitanteEvento',
-                        );
-                        const lstSetorEvento = document.getElementById(
-                            'lstSetorRequisitanteEvento',
-                        );
+                        const txtSetorEvento = document.getElementById("txtSetorRequisitanteEvento");
+                        const lstSetorEvento = document.getElementById("lstSetorRequisitanteEvento");
 
                         if (txtSetorEvento) txtSetorEvento.value = '';
                         if (lstSetorEvento) lstSetorEvento.value = '';
                     }
-                } catch (error) {
+                }
+                catch (error)
+                {
                     console.error('❌ Erro ao setar setor:', error);
-                    Alerta.TratamentoErroComLinha(
-                        'evento.js',
-                        'onSelectRequisitanteEvento.setor',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha('evento.js', 'onSelectRequisitanteEvento.setor', error);
                 }
             },
-            error: function (xhr, status, error) {
-                console.error('❌ Erro ao buscar setor:', {
-                    xhr,
-                    status,
-                    error,
-                });
-                Alerta.TratamentoErroComLinha(
-                    'evento.js',
-                    'onSelectRequisitanteEvento.ajax.setor',
-                    error,
-                );
-
-                const txtSetorEvento = document.getElementById(
-                    'txtSetorRequisitanteEvento',
-                );
-                const lstSetorEvento = document.getElementById(
-                    'lstSetorRequisitanteEvento',
-                );
+            error: function (xhr, status, error)
+            {
+                console.error('❌ Erro ao buscar setor:', { xhr, status, error });
+                Alerta.TratamentoErroComLinha('evento.js', 'onSelectRequisitanteEvento.ajax.setor', error);
+
+                const txtSetorEvento = document.getElementById("txtSetorRequisitanteEvento");
+                const lstSetorEvento = document.getElementById("lstSetorRequisitanteEvento");
 
                 if (txtSetorEvento) txtSetorEvento.value = '';
                 if (lstSetorEvento) lstSetorEvento.value = '';
-            },
+            }
         });
-    } catch (error) {
+    }
+    catch (error)
+    {
         console.error('❌ Erro geral em onSelectRequisitanteEvento:', error);
-        Alerta.TratamentoErroComLinha(
-            'evento.js',
-            'onSelectRequisitanteEvento',
-            error,
+        Alerta.TratamentoErroComLinha('evento.js', 'onSelectRequisitanteEvento', error);
+    }
+};
+
+function controlarVisibilidadeSecaoEvento(finalidade)
+{
+    const sectionEvento = document.getElementById("sectionEvento");
+    const btnEvento = document.getElementById("btnEvento");
+
+    if (!sectionEvento)
+    {
+        console.warn("sectionEvento nao encontrado");
+        return;
+    }
+
+    let isEvento = false;
+
+    if (Array.isArray(finalidade))
+    {
+        isEvento = finalidade.some(f =>
+            f === "Evento" || f === "E" ||
+            (f && f.toLowerCase && f.toLowerCase() === "evento")
         );
-    }
-};
-
-function controlarVisibilidadeSecaoEvento(finalidade) {
-    const sectionEvento = document.getElementById('sectionEvento');
-    const btnEvento = document.getElementById('btnEvento');
-
-    if (!sectionEvento) {
-        console.warn('sectionEvento nao encontrado');
-        return;
-    }
-
-    let isEvento = false;
-
-    if (Array.isArray(finalidade)) {
-        isEvento = finalidade.some(
-            (f) =>
-                f === 'Evento' ||
-                f === 'E' ||
-                (f && f.toLowerCase && f.toLowerCase() === 'evento'),
-        );
-    } else {
-        isEvento =
-            finalidade === 'Evento' ||
-            finalidade === 'E' ||
-            (finalidade &&
-                finalidade.toLowerCase &&
-                finalidade.toLowerCase() === 'evento');
-    }
-
-    if (isEvento) {
-        sectionEvento.style.display = 'block';
-
-        if (btnEvento) {
-            btnEvento.style.display = 'block';
-            console.log('✅ Botão Novo Evento exibido (evento.js)');
-        }
-    } else {
-        sectionEvento.style.display = 'none';
-
-        if (btnEvento) {
-            btnEvento.style.display = 'none';
-            console.log('➖ Botão Novo Evento escondido (evento.js)');
-        }
-
-        if (typeof fecharFormularioCadastroEvento === 'function') {
+    } else
+    {
+        isEvento = finalidade === "Evento" ||
+            finalidade === "E" ||
+            (finalidade && finalidade.toLowerCase && finalidade.toLowerCase() === "evento");
+    }
+
+    if (isEvento)
+    {
+        sectionEvento.style.display = "block";
+
+        if (btnEvento)
+        {
+            btnEvento.style.display = "block";
+            console.log("✅ Botão Novo Evento exibido (evento.js)");
+        }
+    } else
+    {
+        sectionEvento.style.display = "none";
+
+        if (btnEvento)
+        {
+            btnEvento.style.display = "none";
+            console.log("➖ Botão Novo Evento escondido (evento.js)");
+        }
+
+        if (typeof fecharFormularioCadastroEvento === "function")
+        {
             fecharFormularioCadastroEvento();
         }
     }
 }
 
-function configurarBotaoNovoEvento() {
-    const btnEvento = document.getElementById('btnEvento');
-
-    if (!btnEvento) {
-        console.warn('btnEvento nao encontrado');
+function configurarBotaoNovoEvento()
+{
+    const btnEvento = document.getElementById("btnEvento");
+
+    if (!btnEvento)
+    {
+        console.warn("btnEvento nao encontrado");
         return;
     }
 
     const novoBotao = btnEvento.cloneNode(true);
     btnEvento.parentNode.replaceChild(novoBotao, btnEvento);
 
-    novoBotao.addEventListener('click', function (e) {
+    novoBotao.addEventListener("click", function (e)
+    {
         e.preventDefault();
         e.stopPropagation();
 
         abrirFormularioCadastroEvento();
     });
 
-    console.log('Botao Novo Evento configurado (modal)');
-}
-
-function abrirFormularioCadastroEvento() {
+    console.log("Botao Novo Evento configurado (modal)");
+}
+
+function abrirFormularioCadastroEvento()
+{
     limparCamposCadastroEvento();
-    const dataInicialEl = document.getElementById('txtDataInicialEvento');
-
-    if (!mostrarModalFallback('modalEvento')) {
-        console.warn('modalEvento nao encontrado ou Bootstrap indisponivel');
-    }
-
-    setTimeout(() => {
-        const txtNome = document.getElementById('txtNomeEvento');
-        if (txtNome) {
+    const dataInicialEl = document.getElementById("txtDataInicialEvento");
+
+    if (!mostrarModalFallback("modalEvento"))
+    {
+        console.warn("modalEvento nao encontrado ou Bootstrap indisponivel");
+    }
+
+    setTimeout(() =>
+    {
+        const txtNome = document.getElementById("txtNomeEvento");
+        if (txtNome)
+        {
             txtNome.focus();
         }
     }, 300);
 }
 
-function fecharFormularioCadastroEvento() {
-    fecharModalFallback('modalEvento');
+function fecharFormularioCadastroEvento()
+{
+    fecharModalFallback("modalEvento");
 
     limparCamposCadastroEvento();
-    console.log('Formulario de cadastro fechado');
-}
-
-function configurarBotoesCadastroEvento() {
-
-    const btnInserir = document.getElementById('btnInserirEvento');
-    if (btnInserir) {
-
-        btnInserir.className = 'btn btn-azul';
-        btnInserir.innerHTML =
-            '<i class="fa-regular fa-thumbs-up"></i> Salvar Evento';
+    console.log("Formulario de cadastro fechado");
+}
+
+function configurarBotoesCadastroEvento()
+{
+
+    const btnInserir = document.getElementById("btnInserirEvento");
+    if (btnInserir)
+    {
+
+        btnInserir.className = "btn btn-azul";
+        btnInserir.innerHTML = '<i class="fa-regular fa-thumbs-up"></i> Salvar Evento';
 
         const novoBtnInserir = btnInserir.cloneNode(true);
         btnInserir.parentNode.replaceChild(novoBtnInserir, btnInserir);
 
-        novoBtnInserir.addEventListener('click', function () {
-            console.log('💾 Inserindo evento...');
+        novoBtnInserir.addEventListener("click", function ()
+        {
+            console.log("💾 Inserindo evento...");
             inserirNovoEvento();
         });
     }
 
-    const btnCancelar = document.getElementById('btnCancelarEvento');
-    if (btnCancelar) {
-
-        btnCancelar.className = 'btn btn-vinho';
-        btnCancelar.innerHTML =
-            '<i class="fa-regular fa-circle-xmark"></i> Cancelar';
+    const btnCancelar = document.getElementById("btnCancelarEvento");
+    if (btnCancelar)
+    {
+
+        btnCancelar.className = "btn btn-vinho";
+        btnCancelar.innerHTML = '<i class="fa-regular fa-circle-xmark"></i> Cancelar';
 
         const novoBtnCancelar = btnCancelar.cloneNode(true);
         btnCancelar.parentNode.replaceChild(novoBtnCancelar, btnCancelar);
 
-        novoBtnCancelar.addEventListener('click', function () {
-            console.log('❌ Cancelando cadastro');
+        novoBtnCancelar.addEventListener("click", function ()
+        {
+            console.log("❌ Cancelando cadastro");
             fecharFormularioCadastroEvento();
         });
     }
 
-    console.log('✅ Botões do formulário configurados com estilos corretos');
-}
-
-function limparCamposCadastroEvento() {
-    try {
-        console.log('🧹 Limpando campos do formulário...');
-
-        const txtNome = document.getElementById('txtNomeEvento');
-        if (txtNome) txtNome.value = '';
-
-        const txtDescricao = document.getElementById('txtDescricaoEvento');
-        if (txtDescricao) txtDescricao.value = '';
-
-        const txtDataInicial = document.getElementById('txtDataInicialEvento');
+    console.log("✅ Botões do formulário configurados com estilos corretos");
+}
+
+function limparCamposCadastroEvento()
+{
+    try
+    {
+        console.log("🧹 Limpando campos do formulário...");
+
+        const txtNome = document.getElementById("txtNomeEvento");
+        if (txtNome) txtNome.value = "";
+
+        const txtDescricao = document.getElementById("txtDescricaoEvento");
+        if (txtDescricao) txtDescricao.value = "";
+
+        const txtDataInicial = document.getElementById("txtDataInicialEvento");
         limparValorDataEvento(txtDataInicial);
 
-        const txtDataFinal = document.getElementById('txtDataFinalEvento');
+        const txtDataFinal = document.getElementById("txtDataFinalEvento");
         limparValorDataEvento(txtDataFinal);
 
-        const kendoNumericQtd = $('#txtQtdParticipantesEventoCadastro').data(
-            'kendoNumericTextBox',
-        );
-        if (kendoNumericQtd) {
-            kendoNumericQtd.value(null);
+        const txtQuantidade = document.getElementById("txtQtdParticipantesEventoCadastro");
+        if (txtQuantidade?.ej2_instances?.[0])
+        {
+            txtQuantidade.ej2_instances[0].value = 0;
         }
 
         const comboRequisitante = getRequisitanteEventoCombo();
-        if (comboRequisitante) {
+        if (comboRequisitante)
+        {
             comboRequisitante.value(null);
         }
 
-        const txtSetor = document.getElementById('txtSetorRequisitanteEvento');
+        const txtSetor = document.getElementById("txtSetorRequisitanteEvento");
         if (txtSetor) txtSetor.value = '';
 
-        const lstSetor = document.getElementById('lstSetorRequisitanteEvento');
+        const lstSetor = document.getElementById("lstSetorRequisitanteEvento");
         if (lstSetor) lstSetor.value = '';
 
-        console.log('✅ Campos limpos com sucesso');
-    } catch (error) {
-        console.error('❌ Erro ao limpar campos:', error);
-        Alerta.TratamentoErroComLinha(
-            'evento.js',
-            'limparCamposCadastroEvento',
-            error,
-        );
-    }
-}
-
-function inserirNovoEvento() {
-    try {
-        console.log('💾 Iniciando inserção de evento...');
-
-        const txtNome = document.getElementById('txtNomeEvento');
-        const txtDescricao = document.getElementById('txtDescricaoEvento');
-        const txtDataInicial = document.getElementById('txtDataInicialEvento');
-        const txtDataFinal = document.getElementById('txtDataFinalEvento');
-
-        const kendoNumericQtd = $('#txtQtdParticipantesEventoCadastro').data(
-            'kendoNumericTextBox',
-        );
-
-        if (!txtNome || !txtNome.value.trim()) {
-            Alerta.Alerta('Atenção', 'O Nome do Evento é obrigatório!');
-            return;
-        }
-
-        if (!txtDescricao || !txtDescricao.value.trim()) {
-            Alerta.Alerta('Atenção', 'A Descrição do Evento é obrigatória!');
+        console.log("✅ Campos limpos com sucesso");
+
+    } catch (error)
+    {
+        console.error("❌ Erro ao limpar campos:", error);
+        Alerta.TratamentoErroComLinha("evento.js", "limparCamposCadastroEvento", error);
+    }
+}
+
+function inserirNovoEvento()
+{
+    try
+    {
+        console.log("💾 Iniciando inserção de evento...");
+
+        const txtNome = document.getElementById("txtNomeEvento");
+        const txtDescricao = document.getElementById("txtDescricaoEvento");
+        const txtDataInicial = document.getElementById("txtDataInicialEvento");
+        const txtDataFinal = document.getElementById("txtDataFinalEvento");
+        const txtQuantidade = document.getElementById("txtQtdParticipantesEventoCadastro");
+
+        if (!txtNome || !txtNome.value.trim())
+        {
+            Alerta.Alerta("Atenção", "O Nome do Evento é obrigatório!");
+            return;
+        }
+
+        if (!txtDescricao || !txtDescricao.value.trim())
+        {
+            Alerta.Alerta("Atenção", "A Descrição do Evento é obrigatória!");
             return;
         }
 
         const dataInicial = obterValorDataEvento(txtDataInicial);
         const dataFinal = obterValorDataEvento(txtDataFinal);
 
-        if (!dataInicial) {
-            Alerta.Alerta('Atencao', 'A Data Inicial eh obrigatoria!');
-            return;
-        }
-
-        if (!dataFinal) {
-            Alerta.Alerta('Atencao', 'A Data Final eh obrigatoria!');
-            return;
-        }
-
-        if (dataInicial > dataFinal) {
-            Alerta.Alerta(
-                'Atencao',
-                'A Data Inicial nao pode ser maior que a Data Final!',
-            );
-            if (txtDataFinal?.ej2_instances?.[0]) {
+        if (!dataInicial)
+        {
+            Alerta.Alerta("Atencao", "A Data Inicial eh obrigatoria!");
+            return;
+        }
+
+        if (!dataFinal)
+        {
+            Alerta.Alerta("Atencao", "A Data Final eh obrigatoria!");
+            return;
+        }
+
+        if (dataInicial > dataFinal)
+        {
+            Alerta.Alerta("Atencao", "A Data Inicial nao pode ser maior que a Data Final!");
+            if (txtDataFinal?.ej2_instances?.[0])
+            {
                 txtDataFinal.ej2_instances[0].value = null;
-            } else if (txtDataFinal) {
-                txtDataFinal.value = '';
             }
-            return;
-        }
-
-        const quantidade = kendoNumericQtd ? kendoNumericQtd.value() : 0;
-
-        if (!quantidade || quantidade <= 0) {
-            Alerta.Alerta(
-                'Atenção',
-                'A Quantidade de Participantes é obrigatória!',
-            );
-            return;
-        }
-
-        if (!Number.isInteger(quantidade) || quantidade > 2147483647) {
-            Alerta.Alerta(
-                'Atenção',
-                'A Quantidade de Participantes deve ser um número inteiro válido (máximo: 2.147.483.647)!',
-            );
-
-            if (kendoNumericQtd) kendoNumericQtd.value(null);
-            return;
-        }
-
-        const lstSetor = document.getElementById('lstSetorRequisitanteEvento');
+            else if (txtDataFinal)
+            {
+                txtDataFinal.value = "";
+            }
+            return;
+        }
+
+        const quantidadePicker = txtQuantidade?.ej2_instances?.[0];
+        const quantidade = quantidadePicker?.value || 0;
+
+        if (!quantidade || quantidade <= 0)
+        {
+            Alerta.Alerta("Atenção", "A Quantidade de Participantes é obrigatória!");
+            return;
+        }
+
+        if (!Number.isInteger(quantidade) || quantidade > 2147483647)
+        {
+            Alerta.Alerta("Atenção", "A Quantidade de Participantes deve ser um número inteiro válido (máximo: 2.147.483.647)!");
+
+            quantidadePicker.value = null;
+            return;
+        }
+
+        const lstSetor = document.getElementById("lstSetorRequisitanteEvento");
         const comboRequisitante = getRequisitanteEventoCombo();
 
-        if (!lstSetor || !lstSetor.value || lstSetor.value.trim() === '') {
-            Alerta.Alerta(
-                'Atenção',
-                'O Setor é obrigatório! Selecione um requisitante primeiro.',
-            );
-            return;
-        }
-
-        if (!comboRequisitante || !comboRequisitante.value()) {
-            Alerta.Alerta('Atenção', 'O Requisitante é obrigatório!');
+        if (!lstSetor || !lstSetor.value || lstSetor.value.trim() === '')
+        {
+            Alerta.Alerta("Atenção", "O Setor é obrigatório! Selecione um requisitante primeiro.");
+            return;
+        }
+
+        if (!comboRequisitante || !comboRequisitante.value())
+        {
+            Alerta.Alerta("Atenção", "O Requisitante é obrigatório!");
             return;
         }
 
@@ -686,24 +650,27 @@
             SetorSolicitanteId: setorId,
             RequisitanteId: requisitanteId,
             QtdParticipantes: quantidade,
-            DataInicial: moment(dataInicial).format('MM-DD-YYYY'),
-            DataFinal: moment(dataFinal).format('MM-DD-YYYY'),
-            Status: '1',
+            DataInicial: moment(dataInicial).format("MM-DD-YYYY"),
+            DataFinal: moment(dataFinal).format("MM-DD-YYYY"),
+            Status: "1"
         };
 
-        console.log('📦 Objeto a ser enviado:', objEvento);
+        console.log("📦 Objeto a ser enviado:", objEvento);
 
         $.ajax({
-            type: 'POST',
-            url: '/api/Viagem/AdicionarEvento',
-            contentType: 'application/json; charset=utf-8',
-            dataType: 'json',
+            type: "POST",
+            url: "/api/Viagem/AdicionarEvento",
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
             data: JSON.stringify(objEvento),
-            success: function (data) {
-                try {
-                    console.log('✅ Resposta do servidor:', data);
-
-                    if (data.success) {
+            success: function (data)
+            {
+                try
+                {
+                    console.log("✅ Resposta do servidor:", data);
+
+                    if (data.success)
+                    {
 
                         AppToast.show('Verde', data.message);
 
@@ -711,64 +678,58 @@
 
                         fecharFormularioCadastroEvento();
 
-                        console.log('✅ Evento inserido com sucesso!');
-                    } else {
-                        Alerta.Alerta(
-                            'Erro',
-                            data.message || 'Erro ao adicionar evento',
-                        );
+                        console.log("✅ Evento inserido com sucesso!");
                     }
-                } catch (error) {
-                    console.error('❌ Erro no success do AJAX:', error);
-                    Alerta.TratamentoErroComLinha(
-                        'evento.js',
-                        'ajax.AdicionarEvento.success',
-                        error,
-                    );
+                    else
+                    {
+                        Alerta.Alerta("Erro", data.message || "Erro ao adicionar evento");
+                    }
+                }
+                catch (error)
+                {
+                    console.error("❌ Erro no success do AJAX:", error);
+                    Alerta.TratamentoErroComLinha("evento.js", "ajax.AdicionarEvento.success", error);
                 }
             },
-            error: function (jqXHR, textStatus, errorThrown) {
-                try {
-                    console.error('❌ Erro na requisição AJAX:', errorThrown);
-                    console.error(' Status:', textStatus);
-                    console.error(' Response:', jqXHR.responseText);
-
-                    Alerta.Alerta(
-                        'Erro',
-                        'Erro ao adicionar evento no servidor',
-                    );
-                } catch (error) {
-                    console.error('❌ Erro no error handler:', error);
-                    Alerta.TratamentoErroComLinha(
-                        'evento.js',
-                        'ajax.AdicionarEvento.error',
-                        error,
-                    );
+            error: function (jqXHR, textStatus, errorThrown)
+            {
+                try
+                {
+                    console.error("❌ Erro na requisição AJAX:", errorThrown);
+                    console.error(" Status:", textStatus);
+                    console.error(" Response:", jqXHR.responseText);
+
+                    Alerta.Alerta("Erro", "Erro ao adicionar evento no servidor");
                 }
-            },
+                catch (error)
+                {
+                    console.error("❌ Erro no error handler:", error);
+                    Alerta.TratamentoErroComLinha("evento.js", "ajax.AdicionarEvento.error", error);
+                }
+            }
         });
-    } catch (error) {
-        console.error('❌ Erro ao inserir evento:', error);
-        Alerta.TratamentoErroComLinha('evento.js', 'inserirNovoEvento', error);
-    }
-}
-
-function atualizarListaEventos(eventoId, eventoText) {
-    try {
-        console.log('🔄 Atualizando lista de eventos...');
-        console.log(' EventoId:', eventoId);
-        console.log(' EventoText:', eventoText);
-
-        const lstEventos = document.getElementById('lstEventos');
-
-        if (
-            !lstEventos ||
-            !lstEventos.ej2_instances ||
-            !lstEventos.ej2_instances[0]
-        ) {
-            console.error(
-                '❌ lstEventos não encontrado ou não é componente EJ2',
-            );
+
+    }
+    catch (error)
+    {
+        console.error("❌ Erro ao inserir evento:", error);
+        Alerta.TratamentoErroComLinha("evento.js", "inserirNovoEvento", error);
+    }
+}
+
+function atualizarListaEventos(eventoId, eventoText)
+{
+    try
+    {
+        console.log("🔄 Atualizando lista de eventos...");
+        console.log(" EventoId:", eventoId);
+        console.log(" EventoText:", eventoText);
+
+        const lstEventos = document.getElementById("lstEventos");
+
+        if (!lstEventos || !lstEventos.ej2_instances || !lstEventos.ej2_instances[0])
+        {
+            console.error("❌ lstEventos não encontrado ou não é componente EJ2");
             return;
         }
 
@@ -776,30 +737,32 @@
 
         const novoItem = {
             EventoId: eventoId,
-            Evento: eventoText,
+            Evento: eventoText
         };
 
-        console.log('📦 Novo item a ser adicionado:', novoItem);
+        console.log("📦 Novo item a ser adicionado:", novoItem);
 
         let dataSource = comboBox.dataSource || [];
 
-        if (!Array.isArray(dataSource)) {
+        if (!Array.isArray(dataSource))
+        {
             dataSource = [];
         }
 
-        const jaExiste = dataSource.some((item) => item.EventoId === eventoId);
-
-        if (!jaExiste) {
+        const jaExiste = dataSource.some(item => item.EventoId === eventoId);
+
+        if (!jaExiste)
+        {
 
             dataSource.push(novoItem);
-            console.log('📦 Novo item adicionado ao array');
+            console.log("📦 Novo item adicionado ao array");
 
             dataSource.sort((a, b) => {
                 const nomeA = (a.Evento || '').toString().toLowerCase();
                 const nomeB = (b.Evento || '').toString().toLowerCase();
                 return nomeA.localeCompare(nomeB);
             });
-            console.log('🔄 Lista ordenada alfabeticamente');
+            console.log("🔄 Lista ordenada alfabeticamente");
 
             comboBox.dataSource = [];
             comboBox.dataBind();
@@ -807,173 +770,173 @@
             comboBox.dataSource = dataSource;
             comboBox.dataBind();
 
-            console.log('✅ Lista atualizada e ordenada com sucesso');
-        } else {
-            console.log('⚠️ Item já existe na lista');
-        }
-
-        setTimeout(() => {
-            console.log('🔄 Selecionando novo evento...');
+            console.log("✅ Lista atualizada e ordenada com sucesso");
+        }
+        else
+        {
+            console.log("⚠️ Item já existe na lista");
+        }
+
+        setTimeout(() =>
+        {
+            console.log("🔄 Selecionando novo evento...");
 
             comboBox.value = eventoId;
 
             comboBox.dataBind();
 
-            console.log('✅ Evento selecionado');
-            console.log(' Value:', comboBox.value);
-            console.log(' Text:', comboBox.text);
-
-            setTimeout(() => {
-
-                if (typeof window.exibirDadosEvento === 'function') {
-                    console.log('🔍 Chamando window.exibirDadosEvento...');
+            console.log("✅ Evento selecionado");
+            console.log(" Value:", comboBox.value);
+            console.log(" Text:", comboBox.text);
+
+            setTimeout(() =>
+            {
+
+                if (typeof window.exibirDadosEvento === 'function')
+                {
+                    console.log("🔍 Chamando window.exibirDadosEvento...");
                     window.exibirDadosEvento(novoItem);
-                } else if (typeof exibirDadosEvento === 'function') {
-                    console.log('🔍 Chamando exibirDadosEvento...');
+                }
+                else if (typeof exibirDadosEvento === 'function')
+                {
+                    console.log("🔍 Chamando exibirDadosEvento...");
                     exibirDadosEvento(novoItem);
-                } else {
-                    console.warn('⚠️ Função exibirDadosEvento não encontrada');
+                }
+                else
+                {
+                    console.warn("⚠️ Função exibirDadosEvento não encontrada");
                 }
             }, 100);
+
         }, 250);
 
-        console.log('✅ Processo de atualização iniciado');
-    } catch (error) {
-        console.error('❌ Erro ao atualizar lista de eventos:', error);
-        Alerta.TratamentoErroComLinha(
-            'evento.js',
-            'atualizarListaEventos',
-            error,
-        );
-    }
-}
-
-function diagnosticarSistemaEvento() {
-    console.log('=== DIAGNÓSTICO DO SISTEMA DE EVENTO ===');
-
-    const sectionEvento = document.getElementById('sectionEvento');
-    console.log('1. sectionEvento existe?', !!sectionEvento);
-    if (sectionEvento) {
-        console.log(' - Display:', sectionEvento.style.display);
-        console.log(
-            ' - Visível?',
-            sectionEvento.offsetWidth > 0 && sectionEvento.offsetHeight > 0,
-        );
-    }
-
-    const sectionCadastro = document.getElementById('modalEvento');
-    console.log('2. modalEvento existe?', !!sectionCadastro);
-    if (sectionCadastro) {
-        console.log(' - Display:', sectionCadastro.style.display);
-        console.log(
-            ' - Visível?',
-            sectionCadastro.offsetWidth > 0 && sectionCadastro.offsetHeight > 0,
-        );
-    }
-
-    const lstFinalidade = document.getElementById('lstFinalidade');
-    console.log('3. lstFinalidade existe?', !!lstFinalidade);
-    if (lstFinalidade?.ej2_instances) {
-        console.log(' - É componente EJ2?', true);
-        console.log(' - Valor atual:', lstFinalidade.ej2_instances[0].value);
-    }
-
-    const lstEventos = document.getElementById('lstEventos');
-    console.log('4. lstEventos existe?', !!lstEventos);
-    if (lstEventos?.ej2_instances) {
-        console.log(' - É componente EJ2?', true);
-        console.log(' - DataSource:', lstEventos.ej2_instances[0].dataSource);
-        console.log(
-            ' - Quantidade de itens:',
-            lstEventos.ej2_instances[0].dataSource?.length || 0,
-        );
-    }
-
-    const btnEvento = document.getElementById('btnEvento');
-    console.log('5. btnEvento existe?', !!btnEvento);
-    if (btnEvento) {
-        console.log(
-            ' - Display:',
-            window.getComputedStyle(btnEvento).display,
-        );
-        console.log(
-            ' - Visível?',
-            btnEvento.offsetWidth > 0 && btnEvento.offsetHeight > 0,
-        );
-        console.log(
-            ' - Dimensões:',
-            btnEvento.offsetWidth + 'x' + btnEvento.offsetHeight,
-        );
-    }
-
-    const btnInserir = document.getElementById('btnInserirEvento');
-    console.log('6. btnInserirEvento existe?', !!btnInserir);
-
-    const btnCancelar = document.getElementById('btnCancelarEvento');
-    console.log('7. btnCancelarEvento existe?', !!btnCancelar);
-
-    console.log('=== FIM DO DIAGNÓSTICO ===');
-}
-
-function testarMostrarSecaoEvento() {
-    console.log('🧪 Teste: Mostrando seção de evento');
-    controlarVisibilidadeSecaoEvento('Evento');
-}
-
-function testarOcultarSecaoEvento() {
-    console.log('🧪 Teste: Ocultando seção de evento');
-    controlarVisibilidadeSecaoEvento('Transporte');
-}
-
-function testarAbrirFormulario() {
-    console.log('🧪 Teste: Abrindo formulário de cadastro');
+        console.log("✅ Processo de atualização iniciado");
+
+    }
+    catch (error)
+    {
+        console.error("❌ Erro ao atualizar lista de eventos:", error);
+        Alerta.TratamentoErroComLinha("evento.js", "atualizarListaEventos", error);
+    }
+}
+
+function diagnosticarSistemaEvento()
+{
+    console.log("=== DIAGNÓSTICO DO SISTEMA DE EVENTO ===");
+
+    const sectionEvento = document.getElementById("sectionEvento");
+    console.log("1. sectionEvento existe?", !!sectionEvento);
+    if (sectionEvento)
+    {
+        console.log(" - Display:", sectionEvento.style.display);
+        console.log(" - Visível?", sectionEvento.offsetWidth > 0 && sectionEvento.offsetHeight > 0);
+    }
+
+    const sectionCadastro = document.getElementById("modalEvento");
+    console.log("2. modalEvento existe?", !!sectionCadastro);
+    if (sectionCadastro)
+    {
+        console.log(" - Display:", sectionCadastro.style.display);
+        console.log(" - Visível?", sectionCadastro.offsetWidth > 0 && sectionCadastro.offsetHeight > 0);
+    }
+
+    const lstFinalidade = document.getElementById("lstFinalidade");
+    console.log("3. lstFinalidade existe?", !!lstFinalidade);
+    if (lstFinalidade?.ej2_instances)
+    {
+        console.log(" - É componente EJ2?", true);
+        console.log(" - Valor atual:", lstFinalidade.ej2_instances[0].value);
+    }
+
+    const lstEventos = document.getElementById("lstEventos");
+    console.log("4. lstEventos existe?", !!lstEventos);
+    if (lstEventos?.ej2_instances)
+    {
+        console.log(" - É componente EJ2?", true);
+        console.log(" - DataSource:", lstEventos.ej2_instances[0].dataSource);
+        console.log(" - Quantidade de itens:", lstEventos.ej2_instances[0].dataSource?.length || 0);
+    }
+
+    const btnEvento = document.getElementById("btnEvento");
+    console.log("5. btnEvento existe?", !!btnEvento);
+    if (btnEvento)
+    {
+        console.log(" - Display:", window.getComputedStyle(btnEvento).display);
+        console.log(" - Visível?", btnEvento.offsetWidth > 0 && btnEvento.offsetHeight > 0);
+        console.log(" - Dimensões:", btnEvento.offsetWidth + "x" + btnEvento.offsetHeight);
+    }
+
+    const btnInserir = document.getElementById("btnInserirEvento");
+    console.log("6. btnInserirEvento existe?", !!btnInserir);
+
+    const btnCancelar = document.getElementById("btnCancelarEvento");
+    console.log("7. btnCancelarEvento existe?", !!btnCancelar);
+
+    console.log("=== FIM DO DIAGNÓSTICO ===");
+}
+
+function testarMostrarSecaoEvento()
+{
+    console.log("🧪 Teste: Mostrando seção de evento");
+    controlarVisibilidadeSecaoEvento("Evento");
+}
+
+function testarOcultarSecaoEvento()
+{
+    console.log("🧪 Teste: Ocultando seção de evento");
+    controlarVisibilidadeSecaoEvento("Transporte");
+}
+
+function testarAbrirFormulario()
+{
+    console.log("🧪 Teste: Abrindo formulário de cadastro");
     abrirFormularioCadastroEvento();
 }
 
-function testarFecharFormulario() {
-    console.log('🧪 Teste: Fechando formulário de cadastro');
+function testarFecharFormulario()
+{
+    console.log("🧪 Teste: Fechando formulário de cadastro");
     fecharFormularioCadastroEvento();
 }
 
-function testarLimparCampos() {
-    console.log('🧪 Teste: Limpando campos');
+function testarLimparCampos()
+{
+    console.log("🧪 Teste: Limpando campos");
     limparCamposCadastroEvento();
 }
 
-function verificarElementosEvento() {
-    console.log('=== VERIFICAÇÃO DE ELEMENTOS ===');
+function verificarElementosEvento()
+{
+    console.log("=== VERIFICAÇÃO DE ELEMENTOS ===");
 
     const elementos = [
-        'sectionEvento',
-        'modalEvento',
-        'lstEventos',
-        'btnEvento',
-        'txtNomeEvento',
-        'txtDescricaoEvento',
-        'txtDataInicialEvento',
-        'txtDataFinalEvento',
-        'txtQtdParticipantesEventoCadastro',
-        'lstRequisitanteEvento',
-        'lstSetorRequisitanteEvento',
-        'btnInserirEvento',
-        'btnCancelarEvento',
+        "sectionEvento",
+        "modalEvento",
+        "lstEventos",
+        "btnEvento",
+        "txtNomeEvento",
+        "txtDescricaoEvento",
+        "txtDataInicialEvento",
+        "txtDataFinalEvento",
+        "txtQtdParticipantesEventoCadastro",
+        "lstRequisitanteEvento",
+        "lstSetorRequisitanteEvento",
+        "btnInserirEvento",
+        "btnCancelarEvento"
     ];
 
     let todosExistem = true;
 
-    elementos.forEach((id) => {
+    elementos.forEach(id =>
+    {
         const elemento = document.getElementById(id);
         const existe = !!elemento;
-        console.log(existe ? '✅' : '❌', id, 'existe?', existe);
+        console.log(existe ? "✅" : "❌", id, "existe?", existe);
         if (!existe) todosExistem = false;
     });
 
-    console.log('=== FIM DA VERIFICAÇÃO ===');
-    console.log(
-        todosExistem
-            ? '✅ Todos os elementos existem!'
-            : '⚠️ Alguns elementos estão faltando!',
-    );
+    console.log("=== FIM DA VERIFICAÇÃO ===");
+    console.log(todosExistem ? "✅ Todos os elementos existem!" : "⚠️ Alguns elementos estão faltando!");
 
     return todosExistem;
 }
```
