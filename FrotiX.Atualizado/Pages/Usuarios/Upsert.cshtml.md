# Pages/Usuarios/Upsert.cshtml

**Mudanca:** MEDIA | **+2** linhas | **-25** linhas

---

```diff
--- JANEIRO: Pages/Usuarios/Upsert.cshtml
+++ ATUAL: Pages/Usuarios/Upsert.cshtml
@@ -414,7 +414,7 @@
 
                                             <button type="button" class="ftx-foto-clear" id="btnLimparFoto"
                                                 style="display: none !important;">
-                                                <i class="fa-duotone fa-trash-can"></i>
+                                                <i class="fa-solid fa-trash-can"></i>
                                             </button>
                                         </div>
 
@@ -449,7 +449,7 @@
                                 </button>
                             }
 
-                            <a asp-page="./Index" class="btn btn-vinho" data-ftx-loading>
+                            <a asp-page="./Index" class="btn btn-ftx-fechar" data-ftx-loading>
                                 <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Cancelar Operação
                             </a>
                         </div>
@@ -572,19 +572,6 @@
                     });
                 }
 
-                    /**
-                     * ═══════════════════════════════════════════════════════════════════════════
-                     * FORMATAÇÃO E VALIDAÇÃO DE CAMPOS
-                     * ═══════════════════════════════════════════════════════════════════════════
-                     */
-
-                    /**
-                     * Converte string para Camel Case, exceto conectores.
-                     * @@param { string } str - Texto a converter.
-                     * @@returns { string } Texto em Camel Case.
-                     * @@description Conectores(da, de, do, e, etc.) permanecem minúsculos,
-                     * exceto quando são a primeira palavra.
-                     */
                 function toCamelCase(str) {
 
                     const conectores = ['da', 'de', 'do', 'das', 'dos', 'e', 'ou', 'a', 'o', 'as', 'os', 'em', 'na', 'no', 'nas', 'nos', 'para', 'por', 'com'];
@@ -603,11 +590,6 @@
                         .join(' ');
                 }
 
-                    /**
-                     * Limpa e valida nome completo.
-                     * @@param { string } valor - Valor do campo.
-                     * @@returns { string } Valor limpo(somente letras e espaços, max 80 chars).
-                     */
                 function sanitizeNomeCompleto(valor) {
                     let limpo = valor.replace(/[^\p{L} ]+/gu, '');
                     if (limpo.length > 80) {
@@ -641,12 +623,6 @@
                     });
                 }
 
-                    /**
-                     * Formata campo de ponto com prefixo p_ e até 10 dígitos numéricos.
-                     * @@param { string } valor - Valor do campo.
-                     * @@returns { Object } { formatted, digitsLength, overflow }.
-                     * @@description Formato: p_0000000000(prefixo + máx 10 dígitos).
-                     */
                 const txtPonto = document.getElementById('txtPonto');
                 const txtUserName = document.getElementById('txtUserName');
                 const maxPontoDigits = 10;
```

### REMOVER do Janeiro

```html
                                                <i class="fa-duotone fa-trash-can"></i>
                            <a asp-page="./Index" class="btn btn-vinho" data-ftx-loading>
                    /**
                     * ═══════════════════════════════════════════════════════════════════════════
                     * FORMATAÇÃO E VALIDAÇÃO DE CAMPOS
                     * ═══════════════════════════════════════════════════════════════════════════
                     */
                    /**
                     * Converte string para Camel Case, exceto conectores.
                     * @@param { string } str - Texto a converter.
                     * @@returns { string } Texto em Camel Case.
                     * @@description Conectores(da, de, do, e, etc.) permanecem minúsculos,
                     * exceto quando são a primeira palavra.
                     */
                    /**
                     * Limpa e valida nome completo.
                     * @@param { string } valor - Valor do campo.
                     * @@returns { string } Valor limpo(somente letras e espaços, max 80 chars).
                     */
                    /**
                     * Formata campo de ponto com prefixo p_ e até 10 dígitos numéricos.
                     * @@param { string } valor - Valor do campo.
                     * @@returns { Object } { formatted, digitsLength, overflow }.
                     * @@description Formato: p_0000000000(prefixo + máx 10 dígitos).
                     */
```


### ADICIONAR ao Janeiro

```html
                                                <i class="fa-solid fa-trash-can"></i>
                            <a asp-page="./Index" class="btn btn-ftx-fechar" data-ftx-loading>
```
