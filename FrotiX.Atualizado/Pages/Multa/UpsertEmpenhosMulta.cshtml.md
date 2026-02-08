# Pages/Multa/UpsertEmpenhosMulta.cshtml

**Mudanca:** GRANDE | **+72** linhas | **-121** linhas

---

```diff
--- JANEIRO: Pages/Multa/UpsertEmpenhosMulta.cshtml
+++ ATUAL: Pages/Multa/UpsertEmpenhosMulta.cshtml
@@ -22,25 +22,11 @@
 
         /* ====== ANIMAÇÃO GLOW FROTIX ====== */
         @@keyframes buttonWiggle {
-            0% {
-                transform: translateY(0) rotate(0deg);
-            }
-
-            25% {
-                transform: translateY(-2px) rotate(-1deg);
-            }
-
-            50% {
-                transform: translateY(-3px) rotate(0deg);
-            }
-
-            75% {
-                transform: translateY(-2px) rotate(1deg);
-            }
-
-            100% {
-                transform: translateY(0) rotate(0deg);
-            }
+            0% { transform: translateY(0) rotate(0deg); }
+            25% { transform: translateY(-2px) rotate(-1deg); }
+            50% { transform: translateY(-3px) rotate(0deg); }
+            75% { transform: translateY(-2px) rotate(1deg); }
+            100% { transform: translateY(0) rotate(0deg); }
         }
 
         /* ====== VARIÁVEIS LOCAIS ====== */
@@ -106,15 +92,11 @@
         }
 
         /* ====== TABELAS ====== */
-        #tblAporte thead,
-        #tblanulacao thead,
-        #tblMultas thead {
+        #tblAporte thead, #tblanulacao thead, #tblMultas thead {
             background: linear-gradient(180deg, #3D5771 0%, #2d4559 100%);
         }
 
-        #tblAporte thead th,
-        #tblanulacao thead th,
-        #tblMultas thead th {
+        #tblAporte thead th, #tblanulacao thead th, #tblMultas thead th {
             color: #fff !important;
             font-weight: 600;
             font-size: 0.75rem;
@@ -126,9 +108,7 @@
             vertical-align: middle !important;
         }
 
-        #tblAporte tbody tr:hover,
-        #tblanulacao tbody tr:hover,
-        #tblMultas tbody tr:hover {
+        #tblAporte tbody tr:hover, #tblanulacao tbody tr:hover, #tblMultas tbody tr:hover {
             background-color: rgba(50, 93, 136, 0.06) !important;
         }
 
@@ -185,30 +165,21 @@
             background-color: var(--ftx-empenho-azul) !important;
             box-shadow: 0 0 8px rgba(61, 87, 113, 0.5), 0 2px 4px rgba(61, 87, 113, 0.3) !important;
         }
-
-        .ftx-btn-editar:hover {
-            box-shadow: 0 0 20px rgba(61, 87, 113, 0.8), 0 6px 12px rgba(61, 87, 113, 0.5) !important;
-        }
+        .ftx-btn-editar:hover { box-shadow: 0 0 20px rgba(61, 87, 113, 0.8), 0 6px 12px rgba(61, 87, 113, 0.5) !important; }
 
         /* Apagar - Vinho com GLOW */
         .ftx-btn-apagar {
             background-color: var(--ftx-empenho-vinho) !important;
             box-shadow: 0 0 8px rgba(114, 47, 55, 0.5), 0 2px 4px rgba(114, 47, 55, 0.3) !important;
         }
-
-        .ftx-btn-apagar:hover {
-            box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important;
-        }
+        .ftx-btn-apagar:hover { box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important; }
 
         /* Ver Multas - Roxo com GLOW */
         .ftx-btn-multas {
             background-color: var(--ftx-empenho-roxo) !important;
             box-shadow: 0 0 8px rgba(124, 58, 237, 0.5), 0 2px 4px rgba(124, 58, 237, 0.3) !important;
         }
-
-        .ftx-btn-multas:hover {
-            box-shadow: 0 0 20px rgba(124, 58, 237, 0.8), 0 6px 12px rgba(124, 58, 237, 0.5) !important;
-        }
+        .ftx-btn-multas:hover { box-shadow: 0 0 20px rgba(124, 58, 237, 0.8), 0 6px 12px rgba(124, 58, 237, 0.5) !important; }
 
         /* ====== BOTÕES PRINCIPAIS COM GLOW ====== */
         .ftx-btn-submit {
@@ -227,7 +198,6 @@
             box-shadow: 0 0 8px rgba(61, 87, 113, 0.5), 0 2px 4px rgba(61, 87, 113, 0.3) !important;
             width: 100%;
         }
-
         .ftx-btn-submit:hover {
             background-color: var(--ftx-empenho-azul-hover) !important;
             animation: buttonWiggle 0.5s ease-in-out !important;
@@ -252,7 +222,6 @@
             width: 100%;
             text-decoration: none;
         }
-
         .ftx-btn-voltar:hover {
             background-color: var(--ftx-empenho-vinho-hover) !important;
             animation: buttonWiggle 0.5s ease-in-out !important;
@@ -305,7 +274,6 @@
             transition: all .3s ease, transform .2s ease !important;
             min-width: 120px;
         }
-
         .ftx-btn-modal:hover {
             animation: buttonWiggle 0.5s ease-in-out !important;
             color: #fff !important;
@@ -315,19 +283,13 @@
             background-color: var(--ftx-empenho-azul) !important;
             box-shadow: 0 0 8px rgba(61, 87, 113, 0.5), 0 2px 4px rgba(61, 87, 113, 0.3) !important;
         }
-
-        .ftx-btn-confirmar:hover {
-            box-shadow: 0 0 20px rgba(61, 87, 113, 0.8), 0 6px 12px rgba(61, 87, 113, 0.5) !important;
-        }
+        .ftx-btn-confirmar:hover { box-shadow: 0 0 20px rgba(61, 87, 113, 0.8), 0 6px 12px rgba(61, 87, 113, 0.5) !important; }
 
         .ftx-btn-fechar {
             background-color: var(--ftx-empenho-vinho) !important;
             box-shadow: 0 0 8px rgba(114, 47, 55, 0.5), 0 2px 4px rgba(114, 47, 55, 0.3) !important;
         }
-
-        .ftx-btn-fechar:hover {
-            box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important;
-        }
+        .ftx-btn-fechar:hover { box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important; }
 
         /* ====== ÁREAS DE TABELAS ====== */
         .ftx-tables-section {
@@ -353,7 +315,7 @@
             margin-top: 2rem;
         }
 
-        .ftx-form-actions>div {
+        .ftx-form-actions > div {
             flex: 1;
             max-width: 200px;
         }
@@ -418,35 +380,28 @@
                             <div class="row">
                                 <div class="col-12 col-md-5">
                                     <div class="form-group">
-                                        <label class="ftx-form-label"
-                                            asp-for="EmpenhoMultaObj.EmpenhoMulta.OrgaoAutuanteId">Órgão
-                                            Autuante</label>
+                                        <label class="ftx-form-label" asp-for="EmpenhoMultaObj.EmpenhoMulta.OrgaoAutuanteId">Órgão Autuante</label>
                                         @Html.DropDownListFor(
-                                        m => m.EmpenhoMultaObj.EmpenhoMulta.OrgaoAutuanteId,
-                                                                                Model.EmpenhoMultaObj.OrgaoList,
-                                                                                "-- Selecione um Órgão --",
-                                                                                new { @class = "form-control form-control-xs", @id = "listaorgao" })
+                                            m => m.EmpenhoMultaObj.EmpenhoMulta.OrgaoAutuanteId,
+                                            Model.EmpenhoMultaObj.OrgaoList,
+                                            "-- Selecione um Órgão --",
+                                            new { @class = "form-control form-control-xs", @id = "listaorgao" })
                                     </div>
                                 </div>
                                 <div class="col-12 col-md-3">
                                     <div class="form-group">
-                                        <label class="ftx-form-label"
-                                            asp-for="EmpenhoMultaObj.EmpenhoMulta.NotaEmpenho">Nota de Empenho</label>
-                                        <span class="text-danger font-weight-light"
-                                            asp-validation-for="EmpenhoMultaObj.EmpenhoMulta.NotaEmpenho"></span>
+                                        <label class="ftx-form-label" asp-for="EmpenhoMultaObj.EmpenhoMulta.NotaEmpenho">Nota de Empenho</label>
+                                        <span class="text-danger font-weight-light" asp-validation-for="EmpenhoMultaObj.EmpenhoMulta.NotaEmpenho"></span>
                                         <input class="form-control form-control-xs"
-                                            asp-for="EmpenhoMultaObj.EmpenhoMulta.NotaEmpenho"
-                                            placeholder="000000000000"
-                                            data-ejtip="Número da Nota de Empenho (12 dígitos)" />
+                                               asp-for="EmpenhoMultaObj.EmpenhoMulta.NotaEmpenho"
+                                               placeholder="000000000000"
+                                               data-ejtip="Número da Nota de Empenho (12 dígitos)" />
                                     </div>
                                 </div>
                                 <div class="col-6 col-md-2">
                                     <div class="form-group">
-                                        <label class="ftx-form-label"
-                                            asp-for="EmpenhoMultaObj.EmpenhoMulta.AnoVigencia">Vigência</label>
-                                        <select class="form-control form-control-xs"
-                                            asp-for="EmpenhoMultaObj.EmpenhoMulta.AnoVigencia"
-                                            data-ejtip="Ano de Vigência">
+                                        <label class="ftx-form-label" asp-for="EmpenhoMultaObj.EmpenhoMulta.AnoVigencia">Vigência</label>
+                                        <select class="form-control form-control-xs" asp-for="EmpenhoMultaObj.EmpenhoMulta.AnoVigencia" data-ejtip="Ano de Vigência">
                                             <option value="">-- Ano --</option>
                                             <option value="2025">2025</option>
                                             <option value="2024">2024</option>
@@ -463,23 +418,22 @@
                             <div class="row mt-3">
                                 <div class="col-6 col-md-2">
                                     <div class="form-group">
-                                        <label class="ftx-form-label"
-                                            asp-for="EmpenhoMultaObj.EmpenhoMulta.SaldoInicial">Saldo Inicial</label>
+                                        <label class="ftx-form-label" asp-for="EmpenhoMultaObj.EmpenhoMulta.SaldoInicial">Saldo Inicial</label>
                                         <input id="txtSaldoInicial" class="form-control form-control-xs"
-                                            style="text-align: right;"
-                                            asp-for="EmpenhoMultaObj.EmpenhoMulta.SaldoInicial"
-                                            onKeyPress="return(moeda(this,'.',',',event))" placeholder="0,00"
-                                            data-ejtip="Saldo Inicial do Empenho" />
+                                               style="text-align: right;"
+                                               asp-for="EmpenhoMultaObj.EmpenhoMulta.SaldoInicial"
+                                               onKeyPress="return(moeda(this,'.',',',event))"
+                                               placeholder="0,00"
+                                               data-ejtip="Saldo Inicial do Empenho" />
                                     </div>
                                 </div>
                                 <div class="col-6 col-md-2">
                                     <div class="form-group">
-                                        <label class="ftx-form-label"
-                                            asp-for="EmpenhoMultaObj.EmpenhoMulta.SaldoAtual">Saldo Atual</label>
+                                        <label class="ftx-form-label" asp-for="EmpenhoMultaObj.EmpenhoMulta.SaldoAtual">Saldo Atual</label>
                                         <input id="txtSaldoAtual" disabled class="form-control form-control-xs"
-                                            style="text-align: right; background-color: #e9ecef;"
-                                            asp-for="EmpenhoMultaObj.EmpenhoMulta.SaldoAtual"
-                                            data-ejtip="Saldo Atual do Empenho" />
+                                               style="text-align: right; background-color: #e9ecef;"
+                                               asp-for="EmpenhoMultaObj.EmpenhoMulta.SaldoAtual"
+                                               data-ejtip="Saldo Atual do Empenho" />
                                     </div>
                                 </div>
                                 @if (isEdit)
@@ -488,14 +442,15 @@
                                         <div class="form-group">
                                             <label class="ftx-form-label">Multas Pagas</label>
                                             <input id="txtSaldoMultas" disabled class="form-control form-control-xs"
-                                                style="text-align: right; background-color: #e9ecef;"
-                                                data-ejtip="Total de Multas Pagas com este Empenho" />
+                                                   style="text-align: right; background-color: #e9ecef;"
+                                                   data-ejtip="Total de Multas Pagas com este Empenho" />
                                         </div>
                                     </div>
                                     <div class="col-6 col-md-2 d-flex align-items-end">
                                         <a class="ftx-btn-icon ftx-btn-multas btn-ver-multas-empenho"
-                                            style="margin-bottom: 0.5rem;" data-id="@empenhoId"
-                                            data-ejtip="Ver Multas do Empenho">
+                                           style="margin-bottom: 0.5rem;"
+                                           data-id="@empenhoId"
+                                           data-ejtip="Ver Multas do Empenho">
                                             <i class="fa-duotone fa-file-invoice-dollar"></i>
                                         </a>
                                     </div>
@@ -510,8 +465,7 @@
 
                         <div class="ftx-form-actions">
                             <div>
-                                <button type="submit" asp-page-handler="Submit" class="ftx-btn-submit btn-submit-spin"
-                                    data-ejtip="@(isEdit ? "Atualizar empenho do órgão" : "Criar novo empenho do órgão")">
+                                <button type="submit" asp-page-handler="Submit" class="ftx-btn-submit btn-submit-spin" data-ejtip="@(isEdit ? "Atualizar empenho do órgão" : "Criar novo empenho do órgão")">
                                     <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
                                     @(isEdit ? "Atualizar Empenho" : "Criar Empenho")
                                 </button>
@@ -539,7 +493,7 @@
                                                 <th>Data</th>
                                                 <th>Descrição</th>
                                                 <th>Valor</th>
-                                                <th>Ações</th>
+                                                <th>Ação</th>
                                                 <th>Id</th>
                                                 <th>Original</th>
                                             </tr>
@@ -559,7 +513,7 @@
                                                 <th>Data</th>
                                                 <th>Descrição</th>
                                                 <th>Valor</th>
-                                                <th>Ações</th>
+                                                <th>Ação</th>
                                                 <th>Id</th>
                                                 <th>Original</th>
                                             </tr>
@@ -595,7 +549,7 @@
                             <th>Local</th>
                             <th>Data Pagamento</th>
                             <th>Valor Pago</th>
-                            <th>Ações</th>
+                            <th>Ação</th>
                             <th>MultaId</th>
                         </tr>
                     </thead>
@@ -634,8 +588,7 @@
                     <div class="col-12 col-md-7">
                         <div class="form-group">
                             <label class="ftx-form-label">Descrição</label>
-                            <input id="txtDescricaoAporte" class="form-control form-control-xs"
-                                placeholder="Descrição do aporte" />
+                            <input id="txtDescricaoAporte" class="form-control form-control-xs" placeholder="Descrição do aporte" />
                         </div>
                     </div>
                 </div>
@@ -643,8 +596,7 @@
                     <div class="col-12 col-md-4">
                         <div class="form-group">
                             <label class="ftx-form-label">Valor do Aporte</label>
-                            <input id="txtValorAporte" class="form-control form-control-xs" style="text-align: right;"
-                                placeholder="0,00" onKeyPress="return(moeda(this,'.',',',event))" />
+                            <input id="txtValorAporte" class="form-control form-control-xs" style="text-align: right;" placeholder="0,00" onKeyPress="return(moeda(this,'.',',',event))" />
                         </div>
                     </div>
                 </div>
@@ -685,8 +637,7 @@
                     <div class="col-12 col-md-7">
                         <div class="form-group">
                             <label class="ftx-form-label">Descrição</label>
-                            <input id="txtDescricaoanulacao" class="form-control form-control-xs"
-                                placeholder="Descrição da anulação" />
+                            <input id="txtDescricaoanulacao" class="form-control form-control-xs" placeholder="Descrição da anulação" />
                         </div>
                     </div>
                 </div>
@@ -694,8 +645,7 @@
                     <div class="col-12 col-md-4">
                         <div class="form-group">
                             <label class="ftx-form-label">Valor da Anulação</label>
-                            <input id="txtValoranulacao" class="form-control form-control-xs" style="text-align: right;"
-                                placeholder="0,00" onKeyPress="return(moeda(this,'.',',',event))" />
+                            <input id="txtValoranulacao" class="form-control form-control-xs" style="text-align: right;" placeholder="0,00" onKeyPress="return(moeda(this,'.',',',event))" />
                         </div>
                     </div>
                 </div>
@@ -715,10 +665,8 @@
 </div>
 
 @section ScriptsBlock {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script src="~/js/formplugins/select2/select2.bundle.js" asp-append-version="true"></script>
     <script src="~/js/cadastros/aporte_001.js" asp-append-version="true"></script>
@@ -792,13 +740,13 @@
                                 render: function (data) {
                                     try {
                                         return `<div class="text-center" style="white-space: nowrap;">
-                                                <a class="ftx-btn-icon ftx-btn-editar btn-editar-aporte" data-id="${data}" data-ejtip="Editar Aporte">
-                                                    <i class="fa-duotone fa-pen-to-square"></i>
-                                                </a>
-                                                <a class="ftx-btn-icon ftx-btn-apagar btn-deleteaporte" data-context="empenhoMulta" data-id="${data}" data-ejtip="Excluir Aporte">
-                                                    <i class="fa-duotone fa-trash-can"></i>
-                                                </a>
-                                            </div>`;
+                                            <a class="ftx-btn-icon ftx-btn-editar btn-editar-aporte" data-id="${data}" data-ejtip="Editar Aporte">
+                                                <i class="fa-duotone fa-pen-to-square"></i>
+                                            </a>
+                                            <a class="ftx-btn-icon ftx-btn-apagar btn-deleteaporte" data-context="empenhoMulta" data-id="${data}" data-ejtip="Excluir Aporte">
+                                                <i class="fa-duotone fa-trash-can"></i>
+                                            </a>
+                                        </div>`;
                                     }
                                     catch (error) {
                                         Alerta.TratamentoErroComLinha("UpsertEmpenhosMulta.cshtml", "render.aporte", error);
@@ -855,13 +803,13 @@
                                 render: function (data) {
                                     try {
                                         return `<div class="text-center" style="white-space: nowrap;">
-                                                <a class="ftx-btn-icon ftx-btn-editar btn-editar-anulacao" data-id="${data}" data-ejtip="Editar Anulação">
-                                                    <i class="fa-duotone fa-pen-to-square"></i>
-                                                </a>
-                                                <a class="ftx-btn-icon ftx-btn-apagar btn-deleteanulacao" data-context="empenhoMulta" data-id="${data}" data-ejtip="Excluir Anulação">
-                                                    <i class="fa-duotone fa-trash-can"></i>
-                                                </a>
-                                            </div>`;
+                                            <a class="ftx-btn-icon ftx-btn-editar btn-editar-anulacao" data-id="${data}" data-ejtip="Editar Anulação">
+                                                <i class="fa-duotone fa-pen-to-square"></i>
+                                            </a>
+                                            <a class="ftx-btn-icon ftx-btn-apagar btn-deleteanulacao" data-context="empenhoMulta" data-id="${data}" data-ejtip="Excluir Anulação">
+                                                <i class="fa-duotone fa-trash-can"></i>
+                                            </a>
+                                        </div>`;
                                     }
                                     catch (error) {
                                         Alerta.TratamentoErroComLinha("UpsertEmpenhosMulta.cshtml", "render.anulacao", error);
@@ -929,13 +877,13 @@
                                     render: function (data) {
                                         try {
                                             return `<div class="text-center" style="white-space: nowrap;">
-                                                    <a href="/Multa/UpsertPenalidade?id=${data}" class="ftx-btn-icon ftx-btn-editar" data-ejtip="Editar Multa">
-                                                        <i class="fa-duotone fa-pen-to-square"></i>
-                                                    </a>
-                                                    <a class="ftx-btn-icon ftx-btn-apagar btn-delete-multa" data-id="${data}" data-ejtip="Excluir Multa">
-                                                        <i class="fa-duotone fa-trash-can"></i>
-                                                    </a>
-                                                </div>`;
+                                                <a href="/Multa/UpsertPenalidade?id=${data}" class="ftx-btn-icon ftx-btn-editar" data-ejtip="Editar Multa">
+                                                    <i class="fa-duotone fa-pen-to-square"></i>
+                                                </a>
+                                                <a class="ftx-btn-icon ftx-btn-apagar btn-delete-multa" data-id="${data}" data-ejtip="Excluir Multa">
+                                                    <i class="fa-duotone fa-trash-can"></i>
+                                                </a>
+                                            </div>`;
                                         }
                                         catch (error) {
                                             Alerta.TratamentoErroComLinha("UpsertEmpenhosMulta.cshtml", "render.multas", error);
@@ -1131,15 +1079,6 @@
             }
         });
 
-            /**
-             * Aplica máscara de moeda brasileira (R$) ao input
-             * @@param { HTMLInputElement } a - Campo de input
-            * @@param { string } e - Separador de milhar(.)
-                * @@param { string } r - Separador decimal(,)
-                    * @@param { Event } t - Evento de teclado
-                        * @@returns { boolean } True para permitir input, false para bloquear
-                            * @@description Formata valores como 1.234, 56 em tempo real
-                                */
         function moeda(a, e, r, t) {
             try {
                 let n = "", h = 0, j = 0, u = 0, tamanho2 = 0, ajd2 = "", l = "";
```
