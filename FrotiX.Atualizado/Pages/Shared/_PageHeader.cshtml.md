# Pages/Shared/_PageHeader.cshtml

**Mudanca:** GRANDE | **+12** linhas | **-119** linhas

---

```diff
--- JANEIRO: Pages/Shared/_PageHeader.cshtml
+++ ATUAL: Pages/Shared/_PageHeader.cshtml
@@ -12,22 +12,19 @@
         <div class="ml-auto d-flex">
 
             <div>
-                <a href="/intel/analyticsdashboard"
-                    class="header-icon d-flex align-items-center justify-content-center ml-2" title="P&aacute;gina Inicial">
+                <a href="/intel/analyticsdashboard" class="header-icon d-flex align-items-center justify-content-center ml-2" title="P&aacute;gina Inicial">
                     <i class="fa-duotone fa-home"></i>
                 </a>
             </div>
 
             <div>
-                <a href="/agenda/index" class="header-icon d-flex align-items-center justify-content-center ml-2"
-                    title="Agenda">
+                <a href="/agenda/index" class="header-icon d-flex align-items-center justify-content-center ml-2" title="Agenda">
                     <i class="fa-duotone fa-calendar-alt"></i>
                 </a>
             </div>
 
             <div class="dropdown">
-                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton"
-                    data-bs-toggle="dropdown" title="Gest&atilde;o de Requisi&ccedil;&otilde;es">
+                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton" data-bs-toggle="dropdown" title="Gest&atilde;o de Requisi&ccedil;&otilde;es">
                     <i class="fa-duotone fa-clipboard"></i>
                 </a>
                 <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
@@ -37,8 +34,7 @@
             </div>
 
             <div class="dropdown">
-                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton"
-                    data-bs-toggle="dropdown" title="Gest&atilde;o de Viagens">
+                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton" data-bs-toggle="dropdown" title="Gest&atilde;o de Viagens">
                     <i class="fa-duotone fa-briefcase-clock"></i>
                 </a>
                 <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
@@ -51,8 +47,7 @@
             </div>
 
             <div class="dropdown">
-                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton"
-                    data-bs-toggle="dropdown" title="Gest&atilde;o de Manuten&ccedil;&atilde;o">
+                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton" data-bs-toggle="dropdown" title="Gest&atilde;o de Manuten&ccedil;&atilde;o">
                     <i class="fa-duotone fa-car-wrench"></i>
                 </a>
                 <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
@@ -65,8 +60,7 @@
             </div>
 
             <div class="dropdown">
-                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton"
-                    data-bs-toggle="dropdown" title="Gest&atilde;o de Abastecimento">
+                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton" data-bs-toggle="dropdown" title="Gest&atilde;o de Abastecimento">
                     <i class="fa-duotone fa-gas-pump"></i>
                 </a>
                 <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
@@ -78,8 +72,7 @@
             </div>
 
             <div class="dropdown">
-                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton"
-                    data-bs-toggle="dropdown" title="Gest&atilde;o de Contratos">
+                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton" data-bs-toggle="dropdown" title="Gest&atilde;o de Contratos">
                     <i class="fa-duotone fa-folders"></i>
                 </a>
                 <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
@@ -94,8 +87,7 @@
             </div>
 
             <div class="dropdown">
-                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton"
-                    data-bs-toggle="dropdown" title="Gest&atilde;o de Multas">
+                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton" data-bs-toggle="dropdown" title="Gest&atilde;o de Multas">
                     <i class="fa-duotone fa-pen-to-square"></i>
                 </a>
                 <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
@@ -109,8 +101,7 @@
             </div>
 
             <div class="dropdown">
-                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton"
-                    data-bs-toggle="dropdown" title="Gest&atilde;o de Cadastros">
+                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton" data-bs-toggle="dropdown" title="Gest&atilde;o de Cadastros">
                     <i class="fa-duotone fa-map-marker-alt"></i>
                 </a>
                 <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
@@ -122,8 +113,7 @@
             </div>
 
             <div class="dropdown">
-                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton"
-                    data-bs-toggle="dropdown" title="Taxileg" style="margin-right: 20px">
+                <a class="header-icon d-flex align-items-center justify-content-center ml-2" id="dropdownMenuButton" data-bs-toggle="dropdown" title="Taxileg" style="margin-right: 20px">
                     <i class="fa-duotone fa-taxi"></i>
                 </a>
                 <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
@@ -135,15 +125,8 @@
 
             <partial name="_AlertasSino" />
 
-            <div class="ftx-header-user d-flex align-items-center ms-3 me-2">
-                <i class="fa-duotone fa-user"></i>
-                <span id="ftxUserLabel" class="ftx-header-user-label"
-                    data-default="@(Settings.Theme.User)">@(Settings.Theme.User)</span>
-            </div>
-
             <div>
-                <a href="#" data-bs-toggle="dropdown" title="@(Settings.Theme.Email)"
-                    class="header-icon d-flex align-items-center justify-content-center ml-5">
+                <a href="#" data-bs-toggle="dropdown" title="@(Settings.Theme.Email)" class="header-icon d-flex align-items-center justify-content-center ml-5">
                     <img src="~/Images/barbudo.jpg" class="profile-image rounded-circle" alt="@(Settings.Theme.User)">
                 </a>
                 <div class="dropdown-menu dropdown-menu-animated dropdown-lg">
@@ -155,104 +138,5 @@
 }
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * PAGE HEADER - EXIBIÇÃO DE USUÁRIO LOGADO
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Funções para formatação e atualização dinâmica do nome do usuário
-        * logado no header da aplicação.
-     * @@requires jQuery
-            * @@file Shared / _PageHeader.cshtml
-            */
-                (function () {
-                    "use strict";
-
-        /**
-         * Formata o label do usuário combinando ponto e nome
-         * @@param { string } ponto - Número do ponto do usuário
-                        * @@param { string } nome - Nome completo do usuário
-                            * @@returns { string } Label formatado "(ponto.) nome" ou apenas nome
-                                */
-                    function formatarLabelUsuario(ponto, nome) {
-                        try {
-                            const pontoLimpo = (ponto || '').trim();
-                            const nomeLimpo = (nome || '').trim();
-
-                            if (!pontoLimpo && !nomeLimpo) {
-                                return '';
-                            }
-
-                            if (!pontoLimpo) {
-                                return nomeLimpo;
-                            }
-
-                            return `(${pontoLimpo}.) ${nomeLimpo}`.trim();
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("_PageHeader.cshtml", "formatarLabelUsuario", error);
-                            return '';
-                        }
-                    }
-
-        /**
-         * Atualiza o label do usuário logado via AJAX
-         * @@description Busca dados do usuário atual e atualiza elementos do header
-                        */
-        function atualizarUsuarioLogado() {
-                            try {
-                                const headerLabel = document.getElementById('ftxUserLabel');
-                                const dropdownLabel = document.getElementById('divUser');
-                                const fallbackLabel = headerLabel?.getAttribute('data-default') || dropdownLabel?.getAttribute('data-default') || '';
-
-                                if (!window.jQuery) {
-                                    if (headerLabel && fallbackLabel) {
-                                        headerLabel.textContent = fallbackLabel;
-                                    }
-                                    if (dropdownLabel && fallbackLabel) {
-                                        dropdownLabel.textContent = fallbackLabel;
-                                    }
-                                    return;
-                                }
-
-                                $.ajax({
-                                    url: '/api/Login/RecuperaUsuarioAtual',
-                                    type: 'GET',
-                                    dataType: 'json',
-                                    success: function (response) {
-                                        try {
-                                            const label = formatarLabelUsuario(response?.ponto, response?.nome) || fallbackLabel;
-                                            if (headerLabel) {
-                                                headerLabel.textContent = label;
-                                            }
-                                            if (dropdownLabel) {
-                                                dropdownLabel.textContent = label;
-                                            }
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha("_PageHeader.cshtml", "success.atualizarUsuarioLogado", error);
-                                        }
-                                    },
-                                    error: function (xhr, status, error) {
-                                        try {
-                                            if (headerLabel && fallbackLabel) {
-                                                headerLabel.textContent = fallbackLabel;
-                                            }
-                                            if (dropdownLabel && fallbackLabel) {
-                                                dropdownLabel.textContent = fallbackLabel;
-                                            }
-                                        } catch (innerError) {
-                                            Alerta.TratamentoErroComLinha("_PageHeader.cshtml", "error.atualizarUsuarioLogado", innerError);
-                                        }
-                                    }
-                                });
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("_PageHeader.cshtml", "atualizarUsuarioLogado", error);
-                            }
-                        }
-
-        try {
-                        atualizarUsuarioLogado();
-                        $("#dropdownMenuButton").click();
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha("_PageHeader.cshtml", "init", error);
-                    }
-                })();
+    $("#dropdownMenuButton").click();
 </script>
```
