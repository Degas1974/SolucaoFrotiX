# wwwroot/js/agendamento/components/dialogs.js

**Mudanca:** GRANDE | **+88** linhas | **-76** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/components/dialogs.js
+++ ATUAL: wwwroot/js/agendamento/components/dialogs.js
@@ -1,149 +1,159 @@
-window.showDialogDiasSemana = function () {
-    try {
+window.showDialogDiasSemana = function ()
+{
+    try
+    {
 
         const dialog = new ej.popups.Dialog({
-            header:
-                '<div style="display: flex; align-items: center; justify-content: space-between;">' +
+            header: '<div style="display: flex; align-items: center; justify-content: space-between;">' +
                 '<i class="fa fa-exclamation-triangle" aria-hidden="true" style="color: #e67e22;"></i>' +
                 '<span style="flex-grow: 1; text-align: center;">Dia da semana inconsistente</span>' +
                 '<i class="fa fa-exclamation-triangle" aria-hidden="true" style="color: #e67e22;"></i>' +
                 '</div>',
-            content:
-                '<div style="font-size: 1.1em; color: #555; line-height: 1.5;">' +
+            content: '<div style="font-size: 1.1em; color: #555; line-height: 1.5;">' +
                 '<p><i class="fa fa-calendar" aria-hidden="true" style="color: #3498db;"></i> O dia da semana da data inicial não corresponde a nenhum dos dias selecionados.</p>' +
                 '<p><strong>O que deseja fazer?</strong></p>' +
                 '</div>',
             showCloseIcon: true,
             closeOnEscape: false,
             isModal: true,
-            position: { X: 'center', Y: 'center' },
+            position: { X: "center", Y: "center" },
             buttons: [
                 {
-                    click: function () {
+                    click: function ()
+                    {
                         dialog.hide();
                     },
                     buttonModel: {
-                        content:
-                            '<i class="fa-light fa-rocket-launch" aria-hidden="true"></i> Ignorar',
+                        content: '<i class="fa-light fa-rocket-launch" aria-hidden="true"></i> Ignorar',
                         isPrimary: true,
-                        cssClass: 'e-success custom-button',
-                    },
+                        cssClass: 'e-success custom-button'
+                    }
                 },
                 {
-                    click: function () {
-
-                        const txtDataInicialKendo =
-                            $('#txtDataInicial').data('kendoDatePicker');
-                        if (txtDataInicialKendo) {
-                            txtDataInicialKendo.value(null);
-                        } else {
-                            $('#txtDataInicial').val('');
-                        }
-                        $('#txtDataInicial').focus();
+                    click: function ()
+                    {
+                        window.setKendoDateValue("txtDataInicial", null);
+                        document.getElementById('txtDataInicial')?.focus();
                         dialog.hide();
                     },
                     buttonModel: {
-                        content:
-                            '<i class="fa fa-calendar" aria-hidden="true"></i> Mudar Data Inicial',
-                        cssClass: 'e-warning custom-button',
-                    },
+                        content: '<i class="fa fa-calendar" aria-hidden="true"></i> Mudar Data Inicial',
+                        cssClass: 'e-warning custom-button'
+                    }
                 },
                 {
-                    click: function () {
-
-                        const lstDiasKendo =
-                            $('#lstDias').data('kendoMultiSelect');
-                        if (lstDiasKendo) {
-                            lstDiasKendo.value([]);
+                    click: function ()
+                    {
+                        const diasSelect = document.getElementById('lstDias').ej2_instances[0];
+                        if (diasSelect instanceof ej.dropdowns.MultiSelect)
+                        {
+                            diasSelect.value = [];
                         }
                         dialog.hide();
                     },
                     buttonModel: {
-                        content:
-                            '<i class="fa-regular fa-broom-ball" aria-hidden="true"></i> Limpar Dias da Semana',
-                        cssClass: 'e-danger custom-button',
-                    },
-                },
+                        content: '<i class="fa-regular fa-broom-ball" aria-hidden="true"></i> Limpar Dias da Semana',
+                        cssClass: 'e-danger custom-button'
+                    }
+                }
             ],
             animationSettings: { effect: 'Zoom' },
             cssClass: 'custom-dialog',
             width: '450px',
             height: 'auto',
             visible: true,
-            close: () => {
+            close: () =>
+            {
                 dialog.destroy();
-            },
+            }
         });
 
         dialog.appendTo('#dialog-container-diassemana');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dialogs.js',
-            'showDialogDiasSemana',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("dialogs.js", "showDialogDiasSemana", error);
     }
 };
 
-window.onChange = function (args) {
-    try {
-        if (window.dialogObj) {
-            if (args.checked) {
-                window.dialogObj.overlayClick = function () {
+window.onChange = function (args)
+{
+    try
+    {
+        if (window.dialogObj)
+        {
+            if (args.checked)
+            {
+                window.dialogObj.overlayClick = function ()
+                {
                     window.dialogObj.hide();
                 };
-            } else {
-                window.dialogObj.overlayClick = function () {
+            } else
+            {
+                window.dialogObj.overlayClick = function ()
+                {
                     window.dialogObj.show();
                 };
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('dialogs.js', 'onChange', error);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("dialogs.js", "onChange", error);
     }
 };
 
-window.dialogClose = function () {
-    try {
-        if (window.dialogBtn) {
+window.dialogClose = function ()
+{
+    try
+    {
+        if (window.dialogBtn)
+        {
             window.dialogBtn.style.display = 'block';
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('dialogs.js', 'dialogClose', error);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("dialogs.js", "dialogClose", error);
     }
 };
 
-window.dialogOpen = function () {
-    try {
-        if (window.dialogBtn) {
+window.dialogOpen = function ()
+{
+    try
+    {
+        if (window.dialogBtn)
+        {
             window.dialogBtn.style.display = 'none';
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('dialogs.js', 'dialogOpen', error);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("dialogs.js", "dialogOpen", error);
     }
 };
 
-window.dlgButtonClick = function () {
-    try {
-        if (window.dialogObj) {
+window.dlgButtonClick = function ()
+{
+    try
+    {
+        if (window.dialogObj)
+        {
             window.dialogObj.hide();
         }
 
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('dialogs.js', 'dlgButtonClick', error);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("dialogs.js", "dlgButtonClick", error);
     }
 };
 
-window.dlgButtonCloseClick = function () {
-    try {
-        if (window.dialogObj) {
+window.dlgButtonCloseClick = function ()
+{
+    try
+    {
+        if (window.dialogObj)
+        {
             window.dialogObj.hide();
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dialogs.js',
-            'dlgButtonCloseClick',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("dialogs.js", "dlgButtonCloseClick", error);
     }
 };
```
