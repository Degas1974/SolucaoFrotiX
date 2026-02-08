# Pages/Encarregado/Index.cshtml

**Mudanca:** GRANDE | **+11** linhas | **-33** linhas

---

```diff
--- JANEIRO: Pages/Encarregado/Index.cshtml
+++ ATUAL: Pages/Encarregado/Index.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Encarregado.IndexModel
 
 @{
@@ -26,8 +25,7 @@
             gap: 1rem;
             border-radius: 8px 8px 0 0;
             position: relative;
-            overflow: visible;
-            /* Permite wiggle dos botões sem cortar */
+            overflow: visible; /* Permite wiggle dos botões sem cortar */
         }
 
         .ftx-card-header::before {
@@ -37,20 +35,14 @@
             left: -100%;
             width: 100%;
             height: 100%;
-            background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.1), transparent);
+            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.1), transparent);
             animation: shimmer 3s infinite;
-            clip-path: inset(0 round 8px 8px 0 0);
-            /* Mantém shimmer dentro do border-radius */
+            clip-path: inset(0 round 8px 8px 0 0); /* Mantém shimmer dentro do border-radius */
         }
 
         @@keyframes shimmer {
-            0% {
-                left: -100%;
-            }
-
-            100% {
-                left: 100%;
-            }
+            0% { left: -100%; }
+            100% { left: 100%; }
         }
 
         .ftx-card-header .titulo-paginas {
@@ -103,7 +95,7 @@
             background: #fff;
             border-radius: 8px;
             padding: 1rem;
-            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06);
+            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
         }
 
         #tblEncarregado thead {
@@ -211,7 +203,7 @@
             left: -100%;
             width: 100%;
             height: 100%;
-            background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.1), transparent);
+            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.1), transparent);
             animation: shimmer 3s infinite;
         }
 
@@ -255,7 +247,7 @@
         }
 
         .ftx-modal .modal-footer {
-            border-top: 1px solid rgba(0, 0, 0, 0.08);
+            border-top: 1px solid rgba(0,0,0,0.08);
             padding: 1rem 1.25rem;
             background: #f8f9fa;
         }
@@ -329,7 +321,7 @@
                                     <th>Celular</th>
                                     <th>Contrato</th>
                                     <th>Status</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                 </tr>
                             </thead>
                             <tbody></tbody>
@@ -350,8 +342,7 @@
                     <i class="fa-duotone fa-camera-retro"></i>
                     <span id="txtTituloFoto">Foto do Encarregado</span>
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <div class="ftx-foto-container">
@@ -372,20 +363,10 @@
     <script src="~/js/cadastros/encarregado.js" asp-append-version="true"></script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * INICIALIZAÇÃO E HANDLERS DE MODAL DE FOTO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Configura visualização de foto do encarregado em modal Bootstrap 5.
-            * Handler delegado para botões dinâmicos criados pelo DataTable.
-             */
         $(document).ready(function () {
             try {
-                    /**
-                     * Handler delegado para botão de visualização de foto
-                     * @@description Abre modal e carrega foto do encarregado via AJAX
-                    */
-                $(document).on('click', '.btn-foto', function (e) {
+
+                $(document).on('click', '.btn-foto', function(e) {
                     try {
                         e.preventDefault();
                         e.stopPropagation();
@@ -430,10 +411,6 @@
                     }
                 });
 
-                    /**
-                     * Handler de fechamento do modal de foto
-                     * @@description Limpa imagem e título ao fechar para evitar flash visual
-                    */
                 $('#modalFoto').on('hide.bs.modal', function () {
                     try {
                         $('#imgViewer').attr('src', '/Images/barbudo.jpg');
```

### REMOVER do Janeiro

```html
            overflow: visible;
            /* Permite wiggle dos botões sem cortar */
            background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.1), transparent);
            clip-path: inset(0 round 8px 8px 0 0);
            /* Mantém shimmer dentro do border-radius */
            0% {
                left: -100%;
            }
            100% {
                left: 100%;
            }
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06);
            background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.1), transparent);
            border-top: 1px solid rgba(0, 0, 0, 0.08);
                                    <th>Ações</th>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
                    aria-label="Fechar"></button>
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * INICIALIZAÇÃO E HANDLERS DE MODAL DE FOTO
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Configura visualização de foto do encarregado em modal Bootstrap 5.
            * Handler delegado para botões dinâmicos criados pelo DataTable.
             */
                    /**
                     * Handler delegado para botão de visualização de foto
                     * @@description Abre modal e carrega foto do encarregado via AJAX
                    */
                $(document).on('click', '.btn-foto', function (e) {
                    /**
                     * Handler de fechamento do modal de foto
                     * @@description Limpa imagem e título ao fechar para evitar flash visual
                    */
```


### ADICIONAR ao Janeiro

```html
            overflow: visible; /* Permite wiggle dos botões sem cortar */
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.1), transparent);
            clip-path: inset(0 round 8px 8px 0 0); /* Mantém shimmer dentro do border-radius */
            0% { left: -100%; }
            100% { left: 100%; }
            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.1), transparent);
            border-top: 1px solid rgba(0,0,0,0.08);
                                    <th>Ação</th>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
                $(document).on('click', '.btn-foto', function(e) {
```
