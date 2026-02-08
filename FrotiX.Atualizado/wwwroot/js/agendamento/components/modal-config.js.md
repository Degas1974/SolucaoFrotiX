# wwwroot/js/agendamento/components/modal-config.js

**Mudanca:** GRANDE | **+81** linhas | **-79** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/components/modal-config.js
+++ ATUAL: wwwroot/js/agendamento/components/modal-config.js
@@ -1,5 +1,4 @@
-const TITLE_STYLE =
-    "font-family: 'Outfit', sans-serif; font-weight: 700; color: #fff; text-shadow: 0 2px 4px rgba(0,0,0,0.2);";
+const TITLE_STYLE = "font-family: 'Outfit', sans-serif; font-weight: 700; color: #fff; text-shadow: 0 2px 4px rgba(0,0,0,0.2);";
 
 window.ModalConfig = {
     NOVO_AGENDAMENTO: {
@@ -9,7 +8,7 @@
                    style="--fa-primary-color: #006400; --fa-secondary-color: #A9BA9D;"></i>
                 Criar Agendamento
             </h3>`,
-        tipo: 'novo',
+        tipo: 'novo'
     },
 
     EDITAR_AGENDAMENTO: {
@@ -19,7 +18,7 @@
                    style="--fa-primary-color: #002F6C; --fa-secondary-color: #7DA2CE;"></i>
                 Editar Agendamento
             </h3>`,
-        tipo: 'editar',
+        tipo: 'editar'
     },
 
     AGENDAMENTO_CANCELADO: {
@@ -29,7 +28,7 @@
                    style="--fa-primary-color: #8B0000; --fa-secondary-color: #FF4C4C;"></i>
                 Agendamento Cancelado
             </h3>`,
-        tipo: 'cancelado',
+        tipo: 'cancelado'
     },
 
     VIAGEM_ABERTA: {
@@ -38,7 +37,7 @@
                 <i class='fa-duotone fa-solid fa-suitcase-rolling' aria-hidden='true'></i>
                 Exibindo Viagem (Aberta)
             </h3>`,
-        tipo: 'aberta',
+        tipo: 'aberta'
     },
 
     VIAGEM_REALIZADA: {
@@ -49,7 +48,7 @@
                 <span class='btn-vinho fw-bold fst-italic'>Edição Não Permitida</span>
                 )
             </h3>`,
-        tipo: 'realizada',
+        tipo: 'realizada'
     },
 
     VIAGEM_CANCELADA: {
@@ -60,7 +59,7 @@
                 <span class='btn-vinho fw-bold fst-italic'>Edição Não Permitida</span>
                 )
             </h3>`,
-        tipo: 'cancelada',
+        tipo: 'cancelada'
     },
 
     TRANSFORMAR_VIAGEM: {
@@ -70,128 +69,132 @@
                    style="--fa-primary-color: #002F6C; --fa-secondary-color: #7DA2CE;"></i>
                 Transformar Agendamento em Viagem
             </h3>`,
-        tipo: 'transformar',
-    },
-};
-
-window.setModalTitle = function (tipo, statusTexto = null) {
-    try {
+        tipo: 'transformar'
+    }
+};
+
+window.setModalTitle = function (tipo, statusTexto = null)
+{
+    try
+    {
         const config = window.ModalConfig[tipo];
 
-        if (!config) {
-            console.warn('⚠️ Tipo de modal não encontrado:', tipo);
+        if (!config)
+        {
+            console.warn("⚠️ Tipo de modal não encontrado:", tipo);
             return;
         }
 
         let tituloHtml = '';
-        if (config.htmlFunc) {
+        if (config.htmlFunc)
+        {
 
             tituloHtml = config.htmlFunc(statusTexto);
-        } else {
+        } else
+        {
 
             tituloHtml = config.html;
         }
 
-        const tituloElement = document.getElementById('Titulo');
-        if (tituloElement) {
+        const tituloElement = document.getElementById("Titulo");
+        if (tituloElement)
+        {
             tituloElement.innerHTML = tituloHtml;
         }
 
         const seletores = [
-            '#modalViagens .modal-title',
-            '#modalViagemTitulo',
-            '#modalViagens .modal-header h3',
-            '#modalViagens .modal-header',
+            "#modalViagens .modal-title",
+            "#modalViagemTitulo",
+            "#modalViagens .modal-header h3",
+            "#modalViagens .modal-header"
         ];
 
-        seletores.forEach((seletor) => {
-            try {
+        seletores.forEach(seletor =>
+        {
+            try
+            {
                 const elemento = document.querySelector(seletor);
-                if (elemento && elemento.id !== 'Titulo') {
-
-                    if (elemento.classList.contains('modal-header')) {
-
-                        let titleEl = elemento.querySelector(
-                            '.modal-title, h3, h5',
-                        );
-                        if (titleEl) {
-                            titleEl.innerHTML = tituloHtml.replace(
-                                /<h3[^>]*>|<\/h3>/g,
-                                '',
-                            );
+                if (elemento && elemento.id !== "Titulo")
+                {
+
+                    if (elemento.classList.contains('modal-header'))
+                    {
+
+                        let titleEl = elemento.querySelector('.modal-title, h3, h5');
+                        if (titleEl)
+                        {
+                            titleEl.innerHTML = tituloHtml.replace(/<h3[^>]*>|<\/h3>/g, '');
                         }
-                    } else {
-                        elemento.innerHTML = tituloHtml.replace(
-                            /<h3[^>]*>|<\/h3>/g,
-                            '',
-                        );
+                    } else
+                    {
+                        elemento.innerHTML = tituloHtml.replace(/<h3[^>]*>|<\/h3>/g, '');
                     }
                 }
-            } catch (e) {
+            } catch (e)
+            {
                 console.warn(`Erro ao definir título em ${seletor}:`, e);
             }
         });
 
-        console.log('📋 Título do modal definido:', tipo);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-config.js',
-            'setModalTitle',
-            error,
-        );
-    }
-};
-
-window.resetModal = function () {
-    try {
+        console.log("📋 Título do modal definido:", tipo);
+
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-config.js", "setModalTitle", error);
+    }
+};
+
+window.resetModal = function ()
+{
+    try
+    {
         window.setModalTitle('NOVO_AGENDAMENTO');
         window.limparCamposModalViagens();
         window.inicializarCamposModal();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('modal-config.js', 'resetModal', error);
-    }
-};
-
-window.garantirBotoesFechaHabilitados = function () {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-config.js", "resetModal", error);
+    }
+};
+
+window.garantirBotoesFechaHabilitados = function ()
+{
+    try
+    {
         const seletores = [
             '#btnFecha',
             '#btnFechar',
             '#btnCancelar',
             '#modalViagens .btn-close',
             '#modalViagens [data-bs-dismiss="modal"]',
-            '.modal-footer .btn-secondary',
+            '.modal-footer .btn-secondary'
         ];
 
-        seletores.forEach((seletor) => {
-            try {
+        seletores.forEach(seletor =>
+        {
+            try
+            {
                 const elementos = document.querySelectorAll(seletor);
-                elementos.forEach((el) => {
-                    if (el) {
+                elementos.forEach(el =>
+                {
+                    if (el)
+                    {
                         el.disabled = false;
                         el.classList.remove('disabled');
                         el.style.pointerEvents = 'auto';
                         el.style.opacity = '1';
                     }
                 });
-            } catch (e) {
+            } catch (e)
+            {
                 console.warn(`Erro ao habilitar ${seletor}:`, e);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modal-config.js',
-            'garantirBotoesFechaHabilitados',
-            error,
-        );
-    }
-};
-
-window.garantirBotoesFechaHabilitados();
-
-if (!window._garantirBotoesListenerRegistrado) {
-    $('#modalViagens').on('shown.bs.modal', function () {
-        window.garantirBotoesFechaHabilitados();
-    });
-    window._garantirBotoesListenerRegistrado = true;
-}
+
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modal-config.js", "garantirBotoesFechaHabilitados", error);
+    }
+};
+
+setInterval(window.garantirBotoesFechaHabilitados, 1000);
```
