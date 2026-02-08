# Pages/Encarregado/Upsert.cshtml

**Mudanca:** GRANDE | **+40** linhas | **-126** linhas

---

```diff
--- JANEIRO: Pages/Encarregado/Upsert.cshtml
+++ ATUAL: Pages/Encarregado/Upsert.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @addTagHelper*, Microsoft.AspNetCore.Mvc.TagHelpers
 
 @model FrotiX.Pages.Encarregado.UpsertModel
@@ -38,7 +37,7 @@
             background: #fff;
             border-radius: 8px;
             margin-bottom: 1rem;
-            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06);
+            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
             overflow: hidden;
         }
 
@@ -207,8 +206,7 @@
             display: flex;
             gap: 0.75rem;
             flex-wrap: wrap;
-            padding-top: 0.5rem;
-            /* Espaço para animação wiggle dos botões */
+            padding-top: 0.5rem; /* Espaço para animação wiggle dos botões */
         }
 
         .ftx-btn-group .btn {
@@ -289,12 +287,8 @@
                                                         <i class="fa-duotone fa-user"></i>
                                                         Nome Completo
                                                     </label>
-                                                    <input class="form-control"
-                                                        asp-for="EncarregadoObj.Encarregado.Nome"
-                                                        placeholder="Digite o nome completo"
-                                                        onblur="toCamelCase(this)" />
-                                                    <span class="text-danger"
-                                                        asp-validation-for="EncarregadoObj.Encarregado.Nome"></span>
+                                                    <input class="form-control" asp-for="EncarregadoObj.Encarregado.Nome" placeholder="Digite o nome completo" onblur="toCamelCase(this)" />
+                                                    <span class="text-danger" asp-validation-for="EncarregadoObj.Encarregado.Nome"></span>
                                                 </div>
                                             </div>
                                             <div class="col-md-4">
@@ -303,11 +297,8 @@
                                                         <i class="fa-duotone fa-hashtag"></i>
                                                         Ponto
                                                     </label>
-                                                    <input class="form-control"
-                                                        asp-for="EncarregadoObj.Encarregado.Ponto"
-                                                        placeholder="Ex: 12345" onblur="normalizaPonto(this)" />
-                                                    <span class="text-danger"
-                                                        asp-validation-for="EncarregadoObj.Encarregado.Ponto"></span>
+                                                    <input class="form-control" asp-for="EncarregadoObj.Encarregado.Ponto" placeholder="Ex: 12345" onblur="normalizaPonto(this)" />
+                                                    <span class="text-danger" asp-validation-for="EncarregadoObj.Encarregado.Ponto"></span>
                                                 </div>
                                             </div>
                                         </div>
@@ -319,11 +310,8 @@
                                                         <i class="fa-duotone fa-id-card"></i>
                                                         CPF
                                                     </label>
-                                                    <input class="form-control" asp-for="EncarregadoObj.Encarregado.CPF"
-                                                        onkeyup="mascara(this)" onblur="validaCPF(this)" maxlength="14"
-                                                        placeholder="000.000.000-00" />
-                                                    <span class="text-danger"
-                                                        asp-validation-for="EncarregadoObj.Encarregado.CPF"></span>
+                                                    <input class="form-control" asp-for="EncarregadoObj.Encarregado.CPF" onkeyup="mascara(this)" onblur="validaCPF(this)" maxlength="14" placeholder="000.000.000-00" />
+                                                    <span class="text-danger" asp-validation-for="EncarregadoObj.Encarregado.CPF"></span>
                                                 </div>
                                             </div>
                                             <div class="col-md-6">
@@ -332,11 +320,8 @@
                                                         <i class="fa-duotone fa-cake-candles"></i>
                                                         Data de Nascimento
                                                     </label>
-                                                    <input class="form-control"
-                                                        asp-for="EncarregadoObj.Encarregado.DataNascimento" type="date"
-                                                        onblur="validaData(this, 'Data de Nascimento')" />
-                                                    <span class="text-danger"
-                                                        asp-validation-for="EncarregadoObj.Encarregado.DataNascimento"></span>
+                                                    <input class="form-control" asp-for="EncarregadoObj.Encarregado.DataNascimento" type="date" onblur="validaData(this, 'Data de Nascimento')" />
+                                                    <span class="text-danger" asp-validation-for="EncarregadoObj.Encarregado.DataNascimento"></span>
                                                 </div>
                                             </div>
                                         </div>
@@ -356,12 +341,8 @@
                                                         <i class="fa-duotone fa-mobile"></i>
                                                         Celular Principal
                                                     </label>
-                                                    <input class="form-control"
-                                                        asp-for="EncarregadoObj.Encarregado.Celular01"
-                                                        placeholder="(61) 99999-9999" onkeyup="mascaraTelefone(this)"
-                                                        onblur="mascaraTelefone(this)" maxlength="15" />
-                                                    <span class="text-danger"
-                                                        asp-validation-for="EncarregadoObj.Encarregado.Celular01"></span>
+                                                    <input class="form-control" asp-for="EncarregadoObj.Encarregado.Celular01" placeholder="(61) 99999-9999" onkeyup="mascaraTelefone(this)" onblur="mascaraTelefone(this)" maxlength="15" />
+                                                    <span class="text-danger" asp-validation-for="EncarregadoObj.Encarregado.Celular01"></span>
                                                 </div>
                                             </div>
                                             <div class="col-md-6">
@@ -370,12 +351,8 @@
                                                         <i class="fa-duotone fa-mobile-screen"></i>
                                                         Celular Secundário
                                                     </label>
-                                                    <input class="form-control"
-                                                        asp-for="EncarregadoObj.Encarregado.Celular02"
-                                                        placeholder="(61) 99999-9999" onkeyup="mascaraTelefone(this)"
-                                                        onblur="mascaraTelefone(this)" maxlength="15" />
-                                                    <span class="text-danger"
-                                                        asp-validation-for="EncarregadoObj.Encarregado.Celular02"></span>
+                                                    <input class="form-control" asp-for="EncarregadoObj.Encarregado.Celular02" placeholder="(61) 99999-9999" onkeyup="mascaraTelefone(this)" onblur="mascaraTelefone(this)" maxlength="15" />
+                                                    <span class="text-danger" asp-validation-for="EncarregadoObj.Encarregado.Celular02"></span>
                                                 </div>
                                             </div>
                                         </div>
@@ -395,11 +372,8 @@
                                                         <i class="fa-duotone fa-calendar-check"></i>
                                                         Data de Ingresso
                                                     </label>
-                                                    <input class="form-control"
-                                                        asp-for="EncarregadoObj.Encarregado.DataIngresso" type="date"
-                                                        onblur="validaData(this, 'Data de Ingresso')" />
-                                                    <span class="text-danger"
-                                                        asp-validation-for="EncarregadoObj.Encarregado.DataIngresso"></span>
+                                                    <input class="form-control" asp-for="EncarregadoObj.Encarregado.DataIngresso" type="date" onblur="validaData(this, 'Data de Ingresso')" />
+                                                    <span class="text-danger" asp-validation-for="EncarregadoObj.Encarregado.DataIngresso"></span>
                                                 </div>
                                             </div>
                                             <div class="col-md-6">
@@ -409,13 +383,12 @@
                                                         Contrato
                                                     </label>
                                                     @Html.DropDownListFor(
-                                                    m => m.EncarregadoObj.Encarregado.ContratoId,
-                                                                                                        Model.EncarregadoObj.ContratoList,
-                                                                                                        "-- Selecione um Contrato --",
-                                                                                                        new { @class = "form-select" }
-                                                                                                        )
-                                                    <span class="text-danger"
-                                                        asp-validation-for="EncarregadoObj.Encarregado.ContratoId"></span>
+                                                        m => m.EncarregadoObj.Encarregado.ContratoId,
+                                                        Model.EncarregadoObj.ContratoList,
+                                                        "-- Selecione um Contrato --",
+                                                        new { @class = "form-select" }
+                                                    )
+                                                    <span class="text-danger" asp-validation-for="EncarregadoObj.Encarregado.ContratoId"></span>
                                                 </div>
                                             </div>
                                         </div>
@@ -423,8 +396,10 @@
                                         <div class="row mt-2">
                                             <div class="col-md-4">
                                                 <div class="ftx-check-card">
-                                                    <input type="checkbox" class="form-check-input"
-                                                        asp-for="EncarregadoObj.Encarregado.Status" id="chkStatus" />
+                                                    <input type="checkbox"
+                                                           class="form-check-input"
+                                                           asp-for="EncarregadoObj.Encarregado.Status"
+                                                           id="chkStatus" />
                                                     <label for="chkStatus">
                                                         Encarregado Ativo
                                                     </label>
@@ -447,17 +422,19 @@
                                 <div class="ftx-btn-group">
                                     @if (isEdicao)
                                     {
-                                        <button type="submit" asp-page-handler="Edit"
-                                            asp-route-id="@Model.EncarregadoObj.Encarregado.EncarregadoId"
-                                            class="btn btn-azul btn-submit-spin">
+                                        <button type="submit"
+                                                asp-page-handler="Edit"
+                                                asp-route-id="@Model.EncarregadoObj.Encarregado.EncarregadoId"
+                                                class="btn btn-azul btn-submit-spin">
                                             <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
                                             Atualizar Encarregado
                                         </button>
                                     }
                                     else
                                     {
-                                        <button type="submit" asp-page-handler="Submit"
-                                            class="btn btn-azul btn-submit-spin">
+                                        <button type="submit"
+                                                asp-page-handler="Submit"
+                                                class="btn btn-azul btn-submit-spin">
                                             <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
                                             Criar Encarregado
                                         </button>
@@ -479,12 +456,14 @@
                                     <div class="ftx-card-body">
                                         <div class="ftx-foto-section">
                                             <div class="ftx-foto-container">
-                                                <img id="imgViewer" src="/Images/barbudo.jpg"
-                                                    alt="Foto do Encarregado" />
+                                                <img id="imgViewer" src="/Images/barbudo.jpg" alt="Foto do Encarregado" />
                                             </div>
                                             <div class="ftx-foto-upload">
-                                                <input class="form-control" id="txtFile" type="file" accept="image/*"
-                                                    asp-for="FotoUpload" />
+                                                <input class="form-control"
+                                                       id="txtFile"
+                                                       type="file"
+                                                       accept="image/*"
+                                                       asp-for="FotoUpload" />
                                             </div>
                                         </div>
                                     </div>
@@ -502,18 +481,7 @@
 
 @section ScriptsBlock {
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * FUNÇÕES DE MÁSCARA DE ENTRADA
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Máscaras para formatação de CPF e telefone durante digitação
-         */
-
-        /**
-         * Aplica máscara de CPF (999.999.999-99)
-         * @@description Formata o valor digitado no padrão brasileiro de CPF
-         * @@param {HTMLInputElement} i - Elemento input que recebe a máscara
-         */
+
         function mascara(i) {
             try {
                 var v = i.value || "";
@@ -535,11 +503,6 @@
             }
         }
 
-        /**
-         * Aplica máscara de telefone brasileiro
-         * @@description Formata para (99) 9999-9999 (fixo) ou (99) 99999-9999 (celular)
-         * @@param {HTMLInputElement} i - Elemento input que recebe a máscara
-         */
         function mascaraTelefone(i) {
             try {
                 var v = i.value || "";
@@ -565,25 +528,13 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * FUNÇÕES DE FORMATAÇÃO DE TEXTO
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Converte texto para Camel Case (Nome Próprio)
-         * @@description Capitaliza primeira letra de cada palavra, exceto preposições
-         * (de, da, do, e, em, etc.)
-         * @@param {HTMLInputElement} i - Elemento input com o nome a formatar
-         */
         function toCamelCase(i) {
             try {
                 var v = i.value || "";
 
                 var excecoes = ['de', 'da', 'do', 'das', 'dos', 'e', 'em', 'na', 'no', 'nas', 'nos', 'a', 'o', 'as', 'os'];
 
-                v = v.toLowerCase().split(' ').map(function (palavra, index) {
+                v = v.toLowerCase().split(' ').map(function(palavra, index) {
                     if (palavra.length === 0) return '';
 
                     if (index === 0 || excecoes.indexOf(palavra) === -1) {
@@ -598,11 +549,6 @@
             }
         }
 
-        /**
-         * Normaliza campo de ponto para formato p_XXXXX
-         * @@description Remove prefixos existentes (P_, p_, P) e adiciona p_ minúsculo
-         * @@param {HTMLInputElement} i - Elemento input com o código do ponto
-         */
         function normalizaPonto(i) {
             try {
                 var v = i.value || "";
@@ -622,19 +568,6 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * FUNÇÕES DE VALIDAÇÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Valida CPF com algoritmo de dígitos verificadores
-         * @@description Verifica se o CPF é válido conforme regras da Receita Federal.
-         * Rejeita CPFs com todos os dígitos iguais e valida dígitos verificadores.
-         * @@param {HTMLInputElement} i - Elemento input com o CPF a validar
-         * @@returns {boolean} true se válido, false se inválido (limpa e foca o campo)
-         */
         function validaCPF(i) {
             try {
                 var cpf = i.value || "";
@@ -690,15 +623,6 @@
             }
         }
 
-        /**
-         * Valida data conforme contexto do campo
-         * @@description Valida se a data é válida e aplica regras específicas:
-         * - Data de Nascimento: idade mínima 16 anos, máxima 100 anos
-         * - Data de Ingresso: não pode ser no futuro
-         * @@param {HTMLInputElement} i - Elemento input com a data
-         * @@param {string} nomeCampo - Nome do campo para mensagem de erro
-         * @@returns {boolean} true se válida, false se inválida
-         */
         function validaData(i, nomeCampo) {
             try {
                 var valor = i.value || "";
@@ -770,12 +694,6 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * PREVIEW DE FOTO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Exibe preview da foto selecionada antes do upload
-         */
         $("#txtFile").on("change", function (event) {
             try {
                 var files = event.target.files;
@@ -787,13 +705,6 @@
             }
         });
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * INICIALIZAÇÃO DO DOCUMENTO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Configura estado inicial: aplica máscaras aos campos de telefone,
-         * carrega foto do encarregado via AJAX ou define foto padrão
-         */
         $(document).ready(function () {
             try {
                 const encarregadoId = '@Model.EncarregadoObj.Encarregado.EncarregadoId';
```
