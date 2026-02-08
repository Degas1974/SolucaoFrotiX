# wwwroot/js/viagens/kendo-editor-upsert.js

**Mudanca:** GRANDE | **+232** linhas | **-197** linhas

---

```diff
--- JANEIRO: wwwroot/js/viagens/kendo-editor-upsert.js
+++ ATUAL: wwwroot/js/viagens/kendo-editor-upsert.js
@@ -1,139 +1,149 @@
 let _kendoEditorUpsert = null;
 let _kendoEditorUpsertInitialized = false;
 
-function initKendoEditorUpsert() {
-    try {
+function initKendoEditorUpsert()
+{
+    try
+    {
         const textarea = document.getElementById('rte');
         if (!textarea) return null;
 
-        if (_kendoEditorUpsertInitialized && _kendoEditorUpsert) {
+        if (_kendoEditorUpsertInitialized && _kendoEditorUpsert)
+        {
             return _kendoEditorUpsert;
         }
 
         const existingEditor = $(textarea).data('kendoEditor');
-        if (existingEditor) {
+        if (existingEditor)
+        {
             existingEditor.destroy();
             $(textarea).unwrap();
         }
 
-        _kendoEditorUpsert = $(textarea)
-            .kendoEditor({
-                tools: [
-                    'bold',
-                    'italic',
-                    'underline',
-                    'strikethrough',
-                    'separator',
-                    'justifyLeft',
-                    'justifyCenter',
-                    'justifyRight',
-                    'justifyFull',
-                    'separator',
-                    'insertUnorderedList',
-                    'insertOrderedList',
-                    'separator',
-                    'indent',
-                    'outdent',
-                    'separator',
-                    'createLink',
-                    'unlink',
-                    'separator',
-                    'insertImage',
-                    'separator',
-                    'fontName',
-                    'fontSize',
-                    'separator',
-                    'foreColor',
-                    'backColor',
-                    'separator',
-                    'cleanFormatting',
-                    'separator',
-                    'viewHtml',
-                ],
-                stylesheets: [],
-                messages: {
-                    bold: 'Negrito',
-                    italic: 'Itálico',
-                    underline: 'Sublinhado',
-                    strikethrough: 'Tachado',
-                    justifyLeft: 'Alinhar à Esquerda',
-                    justifyCenter: 'Centralizar',
-                    justifyRight: 'Alinhar à Direita',
-                    justifyFull: 'Justificar',
-                    insertUnorderedList: 'Lista com Marcadores',
-                    insertOrderedList: 'Lista Numerada',
-                    indent: 'Aumentar Recuo',
-                    outdent: 'Diminuir Recuo',
-                    createLink: 'Inserir Link',
-                    unlink: 'Remover Link',
-                    insertImage: 'Inserir Imagem',
-                    fontName: 'Fonte',
-                    fontSize: 'Tamanho da Fonte',
-                    foreColor: 'Cor do Texto',
-                    backColor: 'Cor de Fundo',
-                    cleanFormatting: 'Limpar Formatação',
-                    viewHtml: 'Ver HTML',
-                },
-                resizable: {
-                    content: true,
-                    toolbar: false,
-                },
-                imageBrowser: {
-                    transport: {
-                        read: '/api/Viagem/ListarImagens',
-                        uploadUrl: '/api/Viagem/SaveImage',
-                        thumbnailUrl: function (path) {
-                            return path;
-                        },
-                    },
-                },
-            })
-            .data('kendoEditor');
+        _kendoEditorUpsert = $(textarea).kendoEditor({
+            tools: [
+                "bold",
+                "italic",
+                "underline",
+                "strikethrough",
+                "separator",
+                "justifyLeft",
+                "justifyCenter",
+                "justifyRight",
+                "justifyFull",
+                "separator",
+                "insertUnorderedList",
+                "insertOrderedList",
+                "separator",
+                "indent",
+                "outdent",
+                "separator",
+                "createLink",
+                "unlink",
+                "separator",
+                "insertImage",
+                "separator",
+                "fontName",
+                "fontSize",
+                "separator",
+                "foreColor",
+                "backColor",
+                "separator",
+                "cleanFormatting",
+                "separator",
+                "viewHtml"
+            ],
+            stylesheets: [],
+            messages: {
+                bold: "Negrito",
+                italic: "Itálico",
+                underline: "Sublinhado",
+                strikethrough: "Tachado",
+                justifyLeft: "Alinhar à Esquerda",
+                justifyCenter: "Centralizar",
+                justifyRight: "Alinhar à Direita",
+                justifyFull: "Justificar",
+                insertUnorderedList: "Lista com Marcadores",
+                insertOrderedList: "Lista Numerada",
+                indent: "Aumentar Recuo",
+                outdent: "Diminuir Recuo",
+                createLink: "Inserir Link",
+                unlink: "Remover Link",
+                insertImage: "Inserir Imagem",
+                fontName: "Fonte",
+                fontSize: "Tamanho da Fonte",
+                foreColor: "Cor do Texto",
+                backColor: "Cor de Fundo",
+                cleanFormatting: "Limpar Formatação",
+                viewHtml: "Ver HTML"
+            },
+            resizable: {
+                content: true,
+                toolbar: false
+            },
+            imageBrowser: {
+                transport: {
+                    read: "/api/Viagem/ListarImagens",
+                    uploadUrl: "/api/Viagem/SaveImage",
+                    thumbnailUrl: function(path) {
+                        return path;
+                    }
+                }
+            }
+        }).data('kendoEditor');
 
         _kendoEditorUpsertInitialized = true;
 
         criarCompatibilidadeSyncfusionUpsert(textarea);
 
         return _kendoEditorUpsert;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'kendo-editor-upsert.js',
-            'initKendoEditorUpsert',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("kendo-editor-upsert.js", "initKendoEditorUpsert", error);
         return null;
     }
 }
 
-function criarCompatibilidadeSyncfusionUpsert(textarea) {
-    try {
+function criarCompatibilidadeSyncfusionUpsert(textarea)
+{
+    try
+    {
 
         const compatObj = {
             _value: '',
             _readonly: false,
             _enabled: true,
 
-            getValue: function () {
-                if (_kendoEditorUpsert) {
+            getValue: function()
+            {
+                if (_kendoEditorUpsert)
+                {
                     return _kendoEditorUpsert.value() || '';
                 }
                 return '';
             },
 
-            setValue: function (val) {
-                if (_kendoEditorUpsert) {
+            setValue: function(val)
+            {
+                if (_kendoEditorUpsert)
+                {
                     _kendoEditorUpsert.value(val || '');
                 }
             },
 
-            refresh: function () {
-                if (_kendoEditorUpsert) {
+            refresh: function()
+            {
+                if (_kendoEditorUpsert)
+                {
                     _kendoEditorUpsert.refresh();
                 }
             },
 
-            enable: function () {
-                if (_kendoEditorUpsert) {
+            enable: function()
+            {
+                if (_kendoEditorUpsert)
+                {
                     _kendoEditorUpsert.body.contentEditable = true;
                     $(textarea).closest('.k-editor').removeClass('k-disabled');
                     this._enabled = true;
@@ -141,8 +151,10 @@
                 }
             },
 
-            disable: function () {
-                if (_kendoEditorUpsert) {
+            disable: function()
+            {
+                if (_kendoEditorUpsert)
+                {
                     _kendoEditorUpsert.body.contentEditable = false;
                     $(textarea).closest('.k-editor').addClass('k-disabled');
                     this._enabled = false;
@@ -150,179 +162,202 @@
                 }
             },
 
-            focus: function () {
-                if (_kendoEditorUpsert) {
+            focus: function()
+            {
+                if (_kendoEditorUpsert)
+                {
                     _kendoEditorUpsert.focus();
                 }
-            },
+            }
         };
 
         Object.defineProperty(compatObj, 'value', {
-            get: function () {
+            get: function()
+            {
                 return this.getValue();
             },
-            set: function (val) {
+            set: function(val)
+            {
                 this.setValue(val);
-            },
+            }
         });
 
         Object.defineProperty(compatObj, 'readonly', {
-            get: function () {
+            get: function()
+            {
                 return this._readonly;
             },
-            set: function (val) {
+            set: function(val)
+            {
                 this._readonly = val;
-                if (_kendoEditorUpsert) {
-                    if (val) {
+                if (_kendoEditorUpsert)
+                {
+                    if (val)
+                    {
                         this.disable();
-                    } else {
+                    }
+                    else
+                    {
                         this.enable();
                     }
                 }
-            },
+            }
         });
 
         Object.defineProperty(compatObj, 'enabled', {
-            get: function () {
+            get: function()
+            {
                 return this._enabled;
             },
-            set: function (val) {
+            set: function(val)
+            {
                 this._enabled = val;
-                if (_kendoEditorUpsert) {
-                    if (val) {
+                if (_kendoEditorUpsert)
+                {
+                    if (val)
+                    {
                         this.enable();
-                    } else {
+                    }
+                    else
+                    {
                         this.disable();
                     }
                 }
-            },
+            }
         });
 
-        if (!textarea.ej2_instances) {
+        if (!textarea.ej2_instances)
+        {
             textarea.ej2_instances = [];
         }
         textarea.ej2_instances[0] = compatObj;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'kendo-editor-upsert.js',
-            'criarCompatibilidadeSyncfusionUpsert',
-            error,
-        );
-    }
-}
-
-function destroyKendoEditorUpsert() {
-    try {
-        if (_kendoEditorUpsert) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("kendo-editor-upsert.js", "criarCompatibilidadeSyncfusionUpsert", error);
+    }
+}
+
+function destroyKendoEditorUpsert()
+{
+    try
+    {
+        if (_kendoEditorUpsert)
+        {
             _kendoEditorUpsert.destroy();
             _kendoEditorUpsert = null;
             _kendoEditorUpsertInitialized = false;
 
             const textarea = document.getElementById('rte');
-            if (textarea && textarea.ej2_instances) {
+            if (textarea && textarea.ej2_instances)
+            {
                 textarea.ej2_instances = [];
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'kendo-editor-upsert.js',
-            'destroyKendoEditorUpsert',
-            error,
-        );
-    }
-}
-
-function getEditorUpsertValue() {
-    try {
-        if (_kendoEditorUpsert) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("kendo-editor-upsert.js", "destroyKendoEditorUpsert", error);
+    }
+}
+
+function getEditorUpsertValue()
+{
+    try
+    {
+        if (_kendoEditorUpsert)
+        {
             return _kendoEditorUpsert.value() || '';
         }
         return '';
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'kendo-editor-upsert.js',
-            'getEditorUpsertValue',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("kendo-editor-upsert.js", "getEditorUpsertValue", error);
         return '';
     }
 }
 
-function setEditorUpsertValue(html) {
-    try {
-        if (_kendoEditorUpsert) {
+function setEditorUpsertValue(html)
+{
+    try
+    {
+        if (_kendoEditorUpsert)
+        {
             _kendoEditorUpsert.value(html || '');
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'kendo-editor-upsert.js',
-            'setEditorUpsertValue',
-            error,
-        );
-    }
-}
-
-function clearEditorUpsert() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("kendo-editor-upsert.js", "setEditorUpsertValue", error);
+    }
+}
+
+function clearEditorUpsert()
+{
+    try
+    {
         setEditorUpsertValue('');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'kendo-editor-upsert.js',
-            'clearEditorUpsert',
-            error,
-        );
-    }
-}
-
-function enableEditorUpsert() {
-    try {
-        if (_kendoEditorUpsert) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("kendo-editor-upsert.js", "clearEditorUpsert", error);
+    }
+}
+
+function enableEditorUpsert()
+{
+    try
+    {
+        if (_kendoEditorUpsert)
+        {
             _kendoEditorUpsert.body.contentEditable = true;
             $('#rte').closest('.k-editor').removeClass('k-disabled');
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'kendo-editor-upsert.js',
-            'enableEditorUpsert',
-            error,
-        );
-    }
-}
-
-function disableEditorUpsert() {
-    try {
-        if (_kendoEditorUpsert) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("kendo-editor-upsert.js", "enableEditorUpsert", error);
+    }
+}
+
+function disableEditorUpsert()
+{
+    try
+    {
+        if (_kendoEditorUpsert)
+        {
             _kendoEditorUpsert.body.contentEditable = false;
             $('#rte').closest('.k-editor').addClass('k-disabled');
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'kendo-editor-upsert.js',
-            'disableEditorUpsert',
-            error,
-        );
-    }
-}
-
-$(document).ready(function () {
-    try {
-
-        setTimeout(function () {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("kendo-editor-upsert.js", "disableEditorUpsert", error);
+    }
+}
+
+$(document).ready(function()
+{
+    try
+    {
+
+        setTimeout(function()
+        {
             initKendoEditorUpsert();
 
             if (window.viagemFinalizada === true) {
                 disableEditorUpsert();
             }
         }, 300);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'kendo-editor-upsert.js',
-            'document.ready',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("kendo-editor-upsert.js", "document.ready", error);
     }
 });
 
-function toolbarClick(e) {
-
-}
+function toolbarClick(e)
+{
+
+}
```
