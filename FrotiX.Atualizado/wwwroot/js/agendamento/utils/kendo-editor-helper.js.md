# wwwroot/js/agendamento/utils/kendo-editor-helper.js

**Mudanca:** GRANDE | **+66** linhas | **-87** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/utils/kendo-editor-helper.js
+++ ATUAL: wwwroot/js/agendamento/utils/kendo-editor-helper.js
@@ -4,34 +4,25 @@
     let pendingValue = null;
 
     const TOOLS = [
-        'bold',
-        'italic',
-        'underline',
-        'strikethrough',
-        'justifyLeft',
-        'justifyCenter',
-        'justifyRight',
-        'justifyFull',
-        'insertUnorderedList',
-        'insertOrderedList',
-        'indent',
-        'outdent',
-        'createLink',
-        'unlink',
-        'insertImage',
-        'viewHtml',
-        'formatting',
-        'fontName',
-        'fontSize',
-        'foreColor',
-        'backColor',
-        'cleanFormatting',
+        "bold", "italic", "underline", "strikethrough",
+        "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
+        "insertUnorderedList", "insertOrderedList",
+        "indent", "outdent",
+        "createLink", "unlink",
+        "insertImage",
+        "viewHtml",
+        "formatting",
+        "fontName",
+        "fontSize",
+        "foreColor",
+        "backColor",
+        "cleanFormatting"
     ];
 
     function getEditorInstance() {
-        const textarea = document.getElementById('rteDescricao');
+        const textarea = document.getElementById("rteDescricao");
         if (textarea) {
-            return $(textarea).data('kendoEditor');
+            return $(textarea).data("kendoEditor");
         }
         return null;
     }
@@ -40,12 +31,12 @@
         try {
             const editor = getEditorInstance();
             if (editor) {
-                return editor.value() || '';
-            }
-            return pendingValue || '';
-        } catch (error) {
-            console.error('[KendoEditorHelper] Erro ao obter HTML:', error);
-            return '';
+                return editor.value() || "";
+            }
+            return pendingValue || "";
+        } catch (error) {
+            console.error("[KendoEditorHelper] Erro ao obter HTML:", error);
+            return "";
         }
     }
 
@@ -53,119 +44,113 @@
         try {
             const editor = getEditorInstance();
             if (editor) {
-                editor.value(html || '');
-                console.log('[KendoEditorHelper] HTML definido no editor');
+                editor.value(html || "");
+                console.log("[KendoEditorHelper] HTML definido no editor");
             } else {
                 pendingValue = html;
-                console.log(
-                    '[KendoEditorHelper] HTML guardado para aplicar depois',
-                );
-            }
-        } catch (error) {
-            console.error('[KendoEditorHelper] Erro ao definir HTML:', error);
+                console.log("[KendoEditorHelper] HTML guardado para aplicar depois");
+            }
+        } catch (error) {
+            console.error("[KendoEditorHelper] Erro ao definir HTML:", error);
         }
     }
 
     function destroyKendoEditor() {
         try {
-            const textarea = document.getElementById('rteDescricao');
+            const textarea = document.getElementById("rteDescricao");
             if (!textarea) return;
 
-            const editor = $(textarea).data('kendoEditor');
+            const editor = $(textarea).data("kendoEditor");
 
             if (editor) {
 
                 editor.destroy();
-                console.log('[KendoEditorHelper] Instância destruída');
-            }
-
-            const wrapper = $(textarea).closest('.k-editor');
+                console.log("[KendoEditorHelper] Instância destruída");
+            }
+
+            const wrapper = $(textarea).closest(".k-editor");
             if (wrapper.length) {
 
                 wrapper.before(textarea);
                 wrapper.remove();
-                console.log('[KendoEditorHelper] Wrapper removido');
-            }
-
-            $(textarea).removeData('kendoEditor');
+                console.log("[KendoEditorHelper] Wrapper removido");
+            }
+
+            $(textarea).removeData("kendoEditor");
 
             $(textarea).show().css({
-                height: '250px',
-                width: '100%',
+                "height": "250px",
+                "width": "100%"
             });
 
             delete textarea.ej2_instances;
 
             pendingValue = null;
 
-            console.log('[KendoEditorHelper] Editor destruído completamente');
-        } catch (error) {
-            console.error('[KendoEditorHelper] Erro ao destruir:', error);
+            console.log("[KendoEditorHelper] Editor destruído completamente");
+
+        } catch (error) {
+            console.error("[KendoEditorHelper] Erro ao destruir:", error);
         }
     }
 
     function initKendoEditor() {
         try {
-            const textarea = document.getElementById('rteDescricao');
+            const textarea = document.getElementById("rteDescricao");
             if (!textarea) {
-                console.warn(
-                    '[KendoEditorHelper] Textarea #rteDescricao não encontrado',
-                );
+                console.warn("[KendoEditorHelper] Textarea #rteDescricao não encontrado");
                 return null;
             }
 
-            let editor = $(textarea).data('kendoEditor');
-            if (editor) {
-                console.log(
-                    '[KendoEditorHelper] Instância já existe, reutilizando',
-                );
+            let editor = $(textarea).data("kendoEditor");
+            if (editor) {
+                console.log("[KendoEditorHelper] Instância já existe, reutilizando");
                 if (pendingValue !== null) {
                     editor.value(pendingValue);
                     pendingValue = null;
-                    console.log('[KendoEditorHelper] Valor pendente aplicado');
+                    console.log("[KendoEditorHelper] Valor pendente aplicado");
                 }
                 return editor;
             }
 
-            const orphanWrapper = $(textarea).closest('.k-editor');
+            const orphanWrapper = $(textarea).closest(".k-editor");
             if (orphanWrapper.length) {
-                console.warn(
-                    '[KendoEditorHelper] Wrapper órfão encontrado, limpando...',
-                );
+                console.warn("[KendoEditorHelper] Wrapper órfão encontrado, limpando...");
                 orphanWrapper.before(textarea);
                 orphanWrapper.remove();
-                $(textarea).removeData('kendoEditor');
+                $(textarea).removeData("kendoEditor");
             }
 
             $(textarea).kendoEditor({
                 tools: TOOLS,
                 resizable: {
                     content: true,
-                    toolbar: false,
-                },
+                    toolbar: false
+                }
             });
 
-            editor = $(textarea).data('kendoEditor');
+            editor = $(textarea).data("kendoEditor");
 
             if (pendingValue !== null && editor) {
                 editor.value(pendingValue);
                 pendingValue = null;
-                console.log('[KendoEditorHelper] Valor pendente aplicado');
+                console.log("[KendoEditorHelper] Valor pendente aplicado");
             }
 
             createCompatibilityLayer(textarea);
 
-            console.log('[KendoEditorHelper] Editor inicializado com sucesso');
+            console.log("[KendoEditorHelper] Editor inicializado com sucesso");
             return editor;
-        } catch (error) {
-            console.error('[KendoEditorHelper] Erro ao inicializar:', error);
+
+        } catch (error) {
+            console.error("[KendoEditorHelper] Erro ao inicializar:", error);
             return null;
         }
     }
 
     function clearContent() {
         pendingValue = null;
-        setHtml('');
+        setHtml("");
     }
 
     function refreshEditor() {
@@ -202,28 +187,24 @@
             refresh: function () {
                 refreshEditor();
             },
-            enabled: true,
+            enabled: true
         };
 
         textarea.ej2_instances = [compatObj];
     }
 
     function createInitialCompatibilityLayer() {
-        const textarea = document.getElementById('rteDescricao');
+        const textarea = document.getElementById("rteDescricao");
         if (textarea && !textarea.ej2_instances) {
             createCompatibilityLayer(textarea);
-            console.log(
-                '[KendoEditorHelper] Camada de compatibilidade inicial criada',
-            );
+            console.log("[KendoEditorHelper] Camada de compatibilidade inicial criada");
         }
     }
 
     function setupModalIntegration() {
-        const modal = document.getElementById('modalViagens');
+        const modal = document.getElementById("modalViagens");
         if (!modal) {
-            console.warn(
-                '[KendoEditorHelper] Modal #modalViagens não encontrado',
-            );
+            console.warn("[KendoEditorHelper] Modal #modalViagens não encontrado");
             return;
         }
 
@@ -241,13 +222,13 @@
             setTimeout(createInitialCompatibilityLayer, 50);
         });
 
-        console.log('[KendoEditorHelper] Integração com modal configurada');
+        console.log("[KendoEditorHelper] Integração com modal configurada");
     }
 
     const originalRefresh = window.refreshComponenteSafe;
 
     window.refreshComponenteSafe = function (componentId) {
-        if (componentId === 'rteDescricao') {
+        if (componentId === "rteDescricao") {
             refreshEditor();
             return;
         }
@@ -271,7 +252,7 @@
 
         get isInitialized() {
             return getEditorInstance() !== null;
-        },
+        }
     };
 
     $(document).ready(function () {
@@ -279,5 +260,6 @@
         setupModalIntegration();
     });
 
-    console.log('[KendoEditorHelper] Módulo carregado v4');
+    console.log("[KendoEditorHelper] Módulo carregado v4");
+
 })(window);
```
