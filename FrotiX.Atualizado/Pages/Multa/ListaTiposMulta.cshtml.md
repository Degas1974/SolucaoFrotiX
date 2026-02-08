# Pages/Multa/ListaTiposMulta.cshtml

**Mudanca:** GRANDE | **+9** linhas | **-23** linhas

---

```diff
--- JANEIRO: Pages/Multa/ListaTiposMulta.cshtml
+++ ATUAL: Pages/Multa/ListaTiposMulta.cshtml
@@ -69,16 +69,6 @@
 
 @section ScriptsBlock {
     <script asp-append-version="true">
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * LISTA TIPOS DE MULTA - GERENCIAMENTO DE INFRAÇÕES
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Listagem de tipos de multa / infração com DataTable.
-             * Permite visualizar, editar e excluir registros.
-             * @@requires jQuery DataTables, Bootstrap, Alerta
-            * @@file Multa / ListaTiposMulta.cshtml
-            */
-
         let dataTable;
 
         $(document).ready(function () {
@@ -148,11 +138,6 @@
             }
         });
 
-            /**
-             * Carrega/recarrega DataTable de tipos de multa
-             * @@description Inicializa tabela com AJAX para / api / Multa / PegaTipoMulta
-            * Suporta exportação Excel / PDF, paginação e ordenação
-                */
         function loadList() {
             try {
                 if ($.fn.DataTable.isDataTable('#tblTipoMulta')) {
@@ -198,16 +183,16 @@
                             "render": function (data) {
                                 try {
                                     const edit = `<a href="/Multa/UpsertTipoMulta?id=${data}"
-                                                         class="btn btn-sm btn-azul text-white btn-icon-28"
-                                                         data-ejtip="Editar Infração">
-                                                         <i class="fa-duotone fa-edit"></i>
-                                                      </a>`;
+                                                     class="btn btn-sm btn-azul text-white btn-icon-28"
+                                                     data-ejtip="Editar Infração">
+                                                     <i class="fa-duotone fa-edit"></i>
+                                                  </a>`;
                                     const del = `<button type="button"
-                                                             class="btn btn-sm btn-vinho text-white btn-icon-28 btn-delete"
-                                                             data-id="${data}"
-                                                             data-ejtip="Excluir Infração">
-                                                             <i class="fa-duotone fa-trash-alt"></i>
-                                                     </button>`;
+                                                         class="btn btn-sm btn-vinho text-white btn-icon-28 btn-delete"
+                                                         data-id="${data}"
+                                                         data-ejtip="Excluir Infração">
+                                                         <i class="fa-duotone fa-trash-alt"></i>
+                                                 </button>`;
                                     return `<div class="d-flex justify-content-center gap-1">${edit}${del}</div>`;
                                 } catch (error) {
                                     Alerta.TratamentoErroComLinha("ListaTiposMulta.cshtml", "columns.render", error);
```

### REMOVER do Janeiro

```html
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * LISTA TIPOS DE MULTA - GERENCIAMENTO DE INFRAÇÕES
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Listagem de tipos de multa / infração com DataTable.
             * Permite visualizar, editar e excluir registros.
             * @@requires jQuery DataTables, Bootstrap, Alerta
            * @@file Multa / ListaTiposMulta.cshtml
            */
            /**
             * Carrega/recarrega DataTable de tipos de multa
             * @@description Inicializa tabela com AJAX para / api / Multa / PegaTipoMulta
            * Suporta exportação Excel / PDF, paginação e ordenação
                */
                                                         class="btn btn-sm btn-azul text-white btn-icon-28"
                                                         data-ejtip="Editar Infração">
                                                         <i class="fa-duotone fa-edit"></i>
                                                      </a>`;
                                                             class="btn btn-sm btn-vinho text-white btn-icon-28 btn-delete"
                                                             data-id="${data}"
                                                             data-ejtip="Excluir Infração">
                                                             <i class="fa-duotone fa-trash-alt"></i>
                                                     </button>`;
```


### ADICIONAR ao Janeiro

```html
                                                     class="btn btn-sm btn-azul text-white btn-icon-28"
                                                     data-ejtip="Editar Infração">
                                                     <i class="fa-duotone fa-edit"></i>
                                                  </a>`;
                                                         class="btn btn-sm btn-vinho text-white btn-icon-28 btn-delete"
                                                         data-id="${data}"
                                                         data-ejtip="Excluir Infração">
                                                         <i class="fa-duotone fa-trash-alt"></i>
                                                 </button>`;
```
