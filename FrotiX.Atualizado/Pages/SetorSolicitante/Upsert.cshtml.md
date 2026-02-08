# Pages/SetorSolicitante/Upsert.cshtml

**Mudanca:** MEDIA | **+0** linhas | **-17** linhas

---

```diff
--- JANEIRO: Pages/SetorSolicitante/Upsert.cshtml
+++ ATUAL: Pages/SetorSolicitante/Upsert.cshtml
@@ -180,19 +180,6 @@
     </script>
 
     <script type="text/javascript">
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * SETOR SOLICITANTE - CADASTRO/EDIÇÃO (UPSERT)
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Funções de interação com o DropDownTree de hierarquia de setores.
-             * @@requires Syncfusion EJ2 DropDownTree
-            * @@file SetorSolicitante / Upsert.cshtml
-            */
-
-            /**
-             * Callback de mudança de valor no DropDownTree de setor pai
-             * @@description Loga o valor e texto selecionado para debug
-            */
         function valueChange() {
             try {
                 var ddTreeObj = document.getElementById("ddtree").ej2_instances[0];
@@ -202,11 +189,6 @@
             }
         }
 
-            /**
-             * Callback de seleção de item no DropDownTree
-             * @@param { Object } args - Argumentos do evento de seleção
-            * @@description Loga o valor e texto selecionado para debug
-                */
         function select(args) {
             try {
                 var ddTreeObj = document.getElementById("ddtree").ej2_instances[0];
```

### REMOVER do Janeiro

```html
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * SETOR SOLICITANTE - CADASTRO/EDIÇÃO (UPSERT)
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Funções de interação com o DropDownTree de hierarquia de setores.
             * @@requires Syncfusion EJ2 DropDownTree
            * @@file SetorSolicitante / Upsert.cshtml
            */
            /**
             * Callback de mudança de valor no DropDownTree de setor pai
             * @@description Loga o valor e texto selecionado para debug
            */
            /**
             * Callback de seleção de item no DropDownTree
             * @@param { Object } args - Argumentos do evento de seleção
            * @@description Loga o valor e texto selecionado para debug
                */
```

