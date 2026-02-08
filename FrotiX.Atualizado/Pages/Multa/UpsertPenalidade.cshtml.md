# Pages/Multa/UpsertPenalidade.cshtml

**Mudanca:** GRANDE | **+150** linhas | **-259** linhas

---

```diff
--- JANEIRO: Pages/Multa/UpsertPenalidade.cshtml
+++ ATUAL: Pages/Multa/UpsertPenalidade.cshtml
@@ -19,25 +19,11 @@
     <style>
         /* ====== ANIMAÇÃO WIGGLE FROTIX ====== */
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
@@ -113,18 +99,17 @@
             color: #fff !important;
             border-radius: .375rem !important;
             transition: all .3s ease, transform .2s ease !important;
-            box-shadow: 0 3px 6px rgba(0, 0, 0, 0.35), 0 2px 4px rgba(0, 0, 0, 0.2) !important;
+            box-shadow: 0 3px 6px rgba(0,0,0,0.35), 0 2px 4px rgba(0,0,0,0.2) !important;
         }
 
         .btn-azul:hover {
             background-color: var(--ftx-penalidade-azul-dark) !important;
             color: #fff !important;
             animation: buttonWiggle 0.5s ease-in-out !important;
-            box-shadow: 0 5px 12px rgba(0, 0, 0, 0.4), 0 3px 6px rgba(0, 0, 0, 0.25) !important;
-        }
-
-        .btn-azul:active,
-        .btn-azul:focus {
+            box-shadow: 0 5px 12px rgba(0,0,0,0.4), 0 3px 6px rgba(0,0,0,0.25) !important;
+        }
+
+        .btn-azul:active, .btn-azul:focus {
             transform: translateY(0) scale(1) !important;
             color: #fff !important;
         }
@@ -135,18 +120,17 @@
             color: #fff !important;
             border-radius: .375rem !important;
             transition: all .3s ease, transform .2s ease !important;
-            box-shadow: 0 3px 6px rgba(0, 0, 0, 0.35), 0 2px 4px rgba(0, 0, 0, 0.2) !important;
+            box-shadow: 0 3px 6px rgba(0,0,0,0.35), 0 2px 4px rgba(0,0,0,0.2) !important;
         }
 
         .btn-vinho:hover {
             background-color: var(--ftx-penalidade-vinho-dark) !important;
             color: #fff !important;
             animation: buttonWiggle 0.5s ease-in-out !important;
-            box-shadow: 0 5px 12px rgba(0, 0, 0, 0.4), 0 3px 6px rgba(0, 0, 0, 0.25) !important;
-        }
-
-        .btn-vinho:active,
-        .btn-vinho:focus {
+            box-shadow: 0 5px 12px rgba(0,0,0,0.4), 0 3px 6px rgba(0,0,0,0.25) !important;
+        }
+
+        .btn-vinho:active, .btn-vinho:focus {
             transform: translateY(0) scale(1) !important;
             color: #fff !important;
         }
@@ -157,18 +141,17 @@
             color: #fff !important;
             border-radius: .375rem !important;
             transition: all .3s ease, transform .2s ease !important;
-            box-shadow: 0 3px 6px rgba(0, 0, 0, 0.35), 0 2px 4px rgba(0, 0, 0, 0.2) !important;
+            box-shadow: 0 3px 6px rgba(0,0,0,0.35), 0 2px 4px rgba(0,0,0,0.2) !important;
         }
 
         .btn-fundo-laranja:hover {
             background-color: var(--ftx-penalidade-laranja-dark) !important;
             color: #fff !important;
             animation: buttonWiggle 0.5s ease-in-out !important;
-            box-shadow: 0 5px 12px rgba(0, 0, 0, 0.4), 0 3px 6px rgba(0, 0, 0, 0.25) !important;
-        }
-
-        .btn-fundo-laranja:active,
-        .btn-fundo-laranja:focus {
+            box-shadow: 0 5px 12px rgba(0,0,0,0.4), 0 3px 6px rgba(0,0,0,0.25) !important;
+        }
+
+        .btn-fundo-laranja:active, .btn-fundo-laranja:focus {
             transform: translateY(0) scale(1) !important;
             color: #fff !important;
         }
@@ -192,7 +175,7 @@
             border: 1px solid #dee2e6;
             border-radius: 8px;
             overflow: hidden;
-            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
+            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
         }
 
         /* ===== RICH TEXT EDITOR ===== */
@@ -221,12 +204,12 @@
         }
 
         /* ===== CHECKBOX CUSTOMIZADO ===== */
-        .custom-control-input:checked~.custom-control-label::before {
+        .custom-control-input:checked ~ .custom-control-label::before {
             background-color: var(--ftx-penalidade-azul);
             border-color: var(--ftx-penalidade-azul);
         }
 
-        .custom-control-input:focus~.custom-control-label::before {
+        .custom-control-input:focus ~ .custom-control-label::before {
             box-shadow: 0 0 0 0.2rem rgba(61, 87, 113, 0.25);
         }
 
@@ -239,7 +222,7 @@
             border: none;
             border-radius: 12px;
             overflow: hidden;
-            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
+            box-shadow: 0 10px 40px rgba(0,0,0,0.2);
         }
 
         #modalFicha .modal-header {
@@ -259,9 +242,9 @@
 
         #modalFicha .modal-header .modal-title i {
             color: #fff !important;
-            filter: drop-shadow(0 1px 1px rgba(0, 0, 0, 0.2));
+            filter: drop-shadow(0 1px 1px rgba(0,0,0,0.2));
             --fa-primary-color: #fff;
-            --fa-secondary-color: rgba(255, 255, 255, 0.7);
+            --fa-secondary-color: rgba(255,255,255,0.7);
         }
 
         #modalFicha .modal-body {
@@ -272,7 +255,7 @@
         #modalFicha .modal-body img {
             border: 1px solid #dee2e6;
             border-radius: 8px;
-            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
+            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
         }
 
         /* ====== BOTÕES DOS MODAIS - COM SOMBREADO FORTE ====== */
@@ -288,18 +271,17 @@
             align-items: center;
             gap: 0.5rem;
             transition: all .3s ease, transform .2s ease !important;
-            box-shadow: 0 3px 6px rgba(0, 0, 0, 0.35), 0 2px 4px rgba(0, 0, 0, 0.2) !important;
+            box-shadow: 0 3px 6px rgba(0,0,0,0.35), 0 2px 4px rgba(0,0,0,0.2) !important;
         }
 
         .btn-modal-fechar:hover {
             background-color: var(--ftx-penalidade-vinho-dark) !important;
             color: #fff !important;
             animation: buttonWiggle 0.5s ease-in-out !important;
-            box-shadow: 0 5px 12px rgba(0, 0, 0, 0.4), 0 3px 6px rgba(0, 0, 0, 0.25) !important;
-        }
-
-        .btn-modal-fechar:active,
-        .btn-modal-fechar:focus {
+            box-shadow: 0 5px 12px rgba(0,0,0,0.4), 0 3px 6px rgba(0,0,0,0.25) !important;
+        }
+
+        .btn-modal-fechar:active, .btn-modal-fechar:focus {
             transform: translateY(0) scale(1) !important;
         }
 
@@ -310,9 +292,7 @@
                 margin: 1rem auto;
             }
 
-            .btn-azul,
-            .btn-vinho,
-            .btn-fundo-laranja {
+            .btn-azul, .btn-vinho, .btn-fundo-laranja {
                 font-size: 0.85rem;
                 padding: 0.5rem 1rem;
             }
@@ -320,8 +300,7 @@
     </style>
 }
 
-<form method="post" asp-action="Upsert" onkeypress='stopEnterSubmitting(window.event)' enctype="multipart/form-data"
-    autocomplete="off">
+<form method="post" asp-action="Upsert" onkeypress='stopEnterSubmitting(window.event)' enctype="multipart/form-data" autocomplete="off">
     <div class="row">
         <div class="col-xl-12">
             <div id="panel-1" class="panel ftx-card-styled">
@@ -355,90 +334,77 @@
                             <div class="row">
                                 <div class="col-12 col-md-4 col-xl-2">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold"
-                                            asp-for="MultaObj.Multa.NumInfracao"></label>
-                                        <span class="text-danger font-weight-light"
-                                            asp-validation-for="MultaObj.Multa.NumInfracao"></span>
-                                        <input id="txtNumInfracao" class="form-control form-control-xs"
-                                            asp-for="MultaObj.Multa.NumInfracao" />
+                                        <label class="label font-weight-bold" asp-for="MultaObj.Multa.NumInfracao"></label>
+                                        <span class="text-danger font-weight-light" asp-validation-for="MultaObj.Multa.NumInfracao"></span>
+                                        <input id="txtNumInfracao" class="form-control form-control-xs" asp-for="MultaObj.Multa.NumInfracao" />
                                     </div>
                                 </div>
                                 <div class="col-6 col-md-2 col-xl-2">
                                     <div class="form-group">
                                         <label class="label font-weight-bold" asp-for="MultaObj.Multa.Data"></label>
-                                        <span class="text-danger font-weight-light"
-                                            asp-validation-for="MultaObj.Multa.Data"></span>
-                                        <input id="txtDataInfracao" class="form-control form-control-xs"
-                                            asp-for="MultaObj.Multa.Data" type="date" />
+                                        <span class="text-danger font-weight-light" asp-validation-for="MultaObj.Multa.Data"></span>
+                                        <input id="txtDataInfracao" class="form-control form-control-xs" asp-for="MultaObj.Multa.Data" type="date" />
                                     </div>
                                 </div>
                                 <div class="col-6 col-md-2 col-xl-2">
                                     <div class="form-group">
                                         <label class="label font-weight-bold" asp-for="MultaObj.Multa.Hora"></label>
-                                        <span class="text-danger font-weight-light"
-                                            asp-validation-for="MultaObj.Multa.Hora"></span>
-                                        <input id="txtHoraInfracao" class="form-control form-control-xs"
-                                            asp-for="MultaObj.Multa.Hora" type="time" />
+                                        <span class="text-danger font-weight-light" asp-validation-for="MultaObj.Multa.Hora"></span>
+                                        <input id="txtHoraInfracao" class="form-control form-control-xs" asp-for="MultaObj.Multa.Hora" type="time" />
                                     </div>
                                 </div>
                                 <div class="col-6 col-md-2 col-xl-2">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold"
-                                            asp-for="MultaObj.Multa.DataNotificacao"></label>
-                                        <span class="text-danger font-weight-light"
-                                            asp-validation-for="MultaObj.Multa.DataNotificacao"></span>
-                                        <input id="txtDataNotificacao" class="form-control form-control-xs"
-                                            asp-for="MultaObj.Multa.DataNotificacao" type="date" />
+                                        <label class="label font-weight-bold" asp-for="MultaObj.Multa.DataNotificacao"></label>
+                                        <span class="text-danger font-weight-light" asp-validation-for="MultaObj.Multa.DataNotificacao"></span>
+                                        <input id="txtDataNotificacao" class="form-control form-control-xs" asp-for="MultaObj.Multa.DataNotificacao" type="date" />
                                     </div>
                                 </div>
                                 <div class="col-6 col-md-2 col-xl-2">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold"
-                                            asp-for="MultaObj.Multa.DataLimite"></label>
-                                        <span class="text-danger font-weight-light"
-                                            asp-validation-for="MultaObj.Multa.DataLimite"></span>
-                                        <input id="txtDataLimite" class="form-control form-control-xs"
-                                            asp-for="MultaObj.Multa.DataLimite" type="date" />
+                                        <label class="label font-weight-bold" asp-for="MultaObj.Multa.DataLimite"></label>
+                                        <span class="text-danger font-weight-light" asp-validation-for="MultaObj.Multa.DataLimite"></span>
+                                        <input id="txtDataLimite" class="form-control form-control-xs" asp-for="MultaObj.Multa.DataLimite" type="date" />
                                     </div>
                                 </div>
                                 <div class="col-6 col-md-2 col-xl-2">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold"
-                                            asp-for="MultaObj.Multa.Vencimento"></label>
-                                        <input id="txtVencimento" class="form-control form-control-xs"
-                                            asp-for="MultaObj.Multa.Vencimento" type="date" />
+                                        <label class="label font-weight-bold" asp-for="MultaObj.Multa.Vencimento"></label>
+                                        <input id="txtVencimento" class="form-control form-control-xs" asp-for="MultaObj.Multa.Vencimento" type="date" />
                                     </div>
                                 </div>
                             </div>
 
                             <div class="row">
                                 <div class="col-12 col-md-3 col-xl-3">
-                                    <label class="label font-weight-bold"
-                                        asp-for="MultaObj.Multa.ValorAteVencimento"></label>
-                                    <span class="text-danger"
-                                        asp-validation-for="MultaObj.Multa.ValorAteVencimento"></span>
-                                    <input id="txtValorAteVencimento" class="form-control form-control-xs"
-                                        style="text-align: right;" onkeypress="return moeda(this,'.',',',event)"
-                                        placeholder="R$ 0,00" />
-                                    <input type="hidden" asp-for="MultaObj.Multa.ValorAteVencimento"
-                                        id="hdnValorAteVencimento" />
+                                    <label class="label font-weight-bold" asp-for="MultaObj.Multa.ValorAteVencimento"></label>
+                                    <span class="text-danger" asp-validation-for="MultaObj.Multa.ValorAteVencimento"></span>
+                                    <input id="txtValorAteVencimento"
+                                           class="form-control form-control-xs"
+                                           style="text-align: right;"
+                                           onkeypress="return moeda(this,'.',',',event)"
+                                           placeholder="R$ 0,00" />
+                                    <input type="hidden" asp-for="MultaObj.Multa.ValorAteVencimento" id="hdnValorAteVencimento" />
                                 </div>
                                 <div class="col-12 col-md-3 col-xl-3">
-                                    <label class="label font-weight-bold"
-                                        asp-for="MultaObj.Multa.ValorPosVencimento"></label>
-                                    <span class="text-danger"
-                                        asp-validation-for="MultaObj.Multa.ValorPosVencimento"></span>
-                                    <input id="txtValorPosVencimento" class="form-control form-control-xs"
-                                        style="text-align: right;" onkeypress="return moeda(this,'.',',',event)"
-                                        placeholder="R$ 0,00" />
-                                    <input type="hidden" asp-for="MultaObj.Multa.ValorPosVencimento"
-                                        id="hdnValorPosVencimento" />
+                                    <label class="label font-weight-bold" asp-for="MultaObj.Multa.ValorPosVencimento"></label>
+                                    <span class="text-danger" asp-validation-for="MultaObj.Multa.ValorPosVencimento"></span>
+                                    <input id="txtValorPosVencimento"
+                                           class="form-control form-control-xs"
+                                           style="text-align: right;"
+                                           onkeypress="return moeda(this,'.',',',event)"
+                                           placeholder="R$ 0,00" />
+                                    <input type="hidden" asp-for="MultaObj.Multa.ValorPosVencimento" id="hdnValorPosVencimento" />
                                 </div>
                                 <div class="col-12 col-md-2 col-xl-2">
                                     <label class="label">&nbsp;</label>
                                     <div class="custom-control custom-checkbox" style="margin-top: 8px;">
-                                        <input id="chkPaga" name="MultaObj.Multa.Paga" class="custom-control-input"
-                                            type="checkbox" value="true" @(Model.MultaObj.Multa.Paga == true ? "checked" : "") />
+                                        <input id="chkPaga"
+                                               name="MultaObj.Multa.Paga"
+                                               class="custom-control-input"
+                                               type="checkbox"
+                                               value="true"
+                                               @(Model.MultaObj.Multa.Paga == true ? "checked" : "") />
                                         <label class="custom-control-label" for="chkPaga">Paga</label>
                                         <input type="hidden" name="MultaObj.Multa.Paga" value="false" />
                                     </div>
@@ -446,9 +412,12 @@
                                 <div class="col-12 col-md-2 col-xl-2">
                                     <label class="label">&nbsp;</label>
                                     <div class="custom-control custom-checkbox" style="margin-top: 8px;">
-                                        <input id="chkEnviadaSecle" name="MultaObj.Multa.EnviadaSecle"
-                                            class="custom-control-input" type="checkbox" value="true"
-                                            @(Model.MultaObj.Multa.EnviadaSecle == true ? "checked" : "") />
+                                        <input id="chkEnviadaSecle"
+                                               name="MultaObj.Multa.EnviadaSecle"
+                                               class="custom-control-input"
+                                               type="checkbox"
+                                               value="true"
+                                               @(Model.MultaObj.Multa.EnviadaSecle == true ? "checked" : "") />
                                         <label class="custom-control-label" for="chkEnviadaSecle">Enviada Secle</label>
                                         <input type="hidden" name="MultaObj.Multa.EnviadaSecle" value="false" />
                                     </div>
@@ -458,12 +427,9 @@
                             <div class="row pb-12">
                                 <div class="col-12">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold"
-                                            asp-for="MultaObj.Multa.Localizacao"></label>
-                                        <span class="text-danger font-weight-light"
-                                            asp-validation-for="MultaObj.Multa.Localizacao"></span>
-                                        <input id="txtLocalizacao" class="form-control form-control-xs"
-                                            asp-for="MultaObj.Multa.Localizacao" />
+                                        <label class="label font-weight-bold" asp-for="MultaObj.Multa.Localizacao"></label>
+                                        <span class="text-danger font-weight-light" asp-validation-for="MultaObj.Multa.Localizacao"></span>
+                                        <input id="txtLocalizacao" class="form-control form-control-xs" asp-for="MultaObj.Multa.Localizacao" />
                                     </div>
                                 </div>
                             </div>
@@ -472,24 +438,16 @@
                                 <div class="col-10">
                                     <div class="form-group">
                                         <label class="label font-weight-bold">Infração</label>
-                                        <ejs-dropdownlist id="lstInfracao" dataSource="@ViewData["dataInfracao"]"
-                                            placeholder="Selecione uma Infração..." allowFiltering="true"
-                                            filterType="Contains" popupHeight="220px"
-                                            ejs-for="@Model.MultaObj.Multa.TipoMultaId">
-                                            <e-dropdownlist-fields text="Descricao"
-                                                value="TipoMultaId"></e-dropdownlist-fields>
+                                        <ejs-dropdownlist id="lstInfracao" dataSource="@ViewData["dataInfracao"]" placeholder="Selecione uma Infração..." allowFiltering="true" filterType="Contains" popupHeight="220px" ejs-for="@Model.MultaObj.Multa.TipoMultaId">
+                                            <e-dropdownlist-fields text="Descricao" value="TipoMultaId"></e-dropdownlist-fields>
                                         </ejs-dropdownlist>
                                     </div>
                                 </div>
                                 <div class="col-2">
                                     <div class="form-group">
                                         <label class="label font-weight-bold" asp-for="MultaObj.Multa.Status"></label>
-                                        <span class="text-danger font-weight-light"
-                                            asp-validation-for="MultaObj.Multa.Status"></span>
-                                        <ejs-combobox id="lstStatus" placeholder="Status..." allowFiltering="true"
-                                            filterType="Contains" dataSource="@ViewData["dataStatus"]"
-                                            popupHeight="200px" width="100%" showClearButton="true"
-                                            ejs-for="@Model.MultaObj.Multa.Status">
+                                        <span class="text-danger font-weight-light" asp-validation-for="MultaObj.Multa.Status"></span>
+                                        <ejs-combobox id="lstStatus" placeholder="Status..." allowFiltering="true" filterType="Contains" dataSource="@ViewData["dataStatus"]" popupHeight="200px" width="100%" showClearButton="true" ejs-for="@Model.MultaObj.Multa.Status">
                                             <e-combobox-fields text="Descricao" value="StatusId"></e-combobox-fields>
                                         </ejs-combobox>
                                     </div>
@@ -499,25 +457,20 @@
                             <div class="row">
                                 <div class="col-3">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold"
-                                            asp-for="MultaObj.Multa.ProcessoEDoc"></label>
-                                        <input id="txtProcessoEDoc" class="form-control form-control-xs"
-                                            asp-for="MultaObj.Multa.ProcessoEDoc" />
+                                        <label class="label font-weight-bold" asp-for="MultaObj.Multa.ProcessoEDoc"></label>
+                                        <input id="txtProcessoEDoc" class="form-control form-control-xs" asp-for="MultaObj.Multa.ProcessoEDoc" />
                                     </div>
                                 </div>
                                 <div class="col-3">
                                     <div class="form-group">
-                                        <label class="label font-weight-bold"
-                                            asp-for="MultaObj.Multa.NoFichaVistoria"></label>
-                                        <input id="txtNoFichaVistoria" class="form-control form-control-xs"
-                                            asp-for="MultaObj.Multa.NoFichaVistoria" type="number" />
+                                        <label class="label font-weight-bold" asp-for="MultaObj.Multa.NoFichaVistoria"></label>
+                                        <input id="txtNoFichaVistoria" class="form-control form-control-xs" asp-for="MultaObj.Multa.NoFichaVistoria" type="number" />
                                     </div>
                                 </div>
                                 <div class="col-3">
                                     <div class="form-group">
                                         <label class="label font-weight-bold"></label>
-                                        <button id="btnFicha" type="button" class="btn btn-fundo-laranja btn-sm"
-                                            style="margin-top: 26px; height: 32px;" disabled>
+                                        <button id="btnFicha" type="button" class="btn btn-fundo-laranja btn-sm" style="margin-top: 26px; height: 32px;" disabled>
                                             <i class="fa-duotone fa-magnifying-glass"></i>
                                             Ver Ficha
                                         </button>
@@ -528,27 +481,19 @@
                             <div class="row">
                                 <div class="col-12 col-md-6 col-xl-4">
                                     <label class="label font-weight-bold">Órgão Autuante</label>
-                                    <ejs-combobox id="lstOrgao" placeholder="Selecione um Órgão Autuante"
-                                        allowFiltering="true" filterType="Contains" dataSource="@ViewData["dataOrgao"]"
-                                        popupHeight="200px" width="100%" showClearButton="true"
-                                        ejs-for="@Model.MultaObj.Multa.OrgaoAutuanteId">
+                                    <ejs-combobox id="lstOrgao" placeholder="Selecione um Órgão Autuante" allowFiltering="true" filterType="Contains" dataSource="@ViewData["dataOrgao"]" popupHeight="200px" width="100%" showClearButton="true" ejs-for="@Model.MultaObj.Multa.OrgaoAutuanteId">
                                         <e-combobox-fields text="Descricao" value="OrgaoId"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
                                 <div class="col-12 col-md-6 col-xl-4">
                                     <label class="label font-weight-bold">Empenho</label>
-                                    <ejs-combobox id="lstEmpenhos" placeholder="Selecione um Empenho"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["dataTodosEmpenhos"]" popupHeight="200px" width="100%"
-                                        showClearButton="true" ejs-for="@Model.MultaObj.Multa.EmpenhoMultaId">
-                                        <e-combobox-fields text="NotaEmpenho"
-                                            value="EmpenhoMultaId"></e-combobox-fields>
+                                    <ejs-combobox id="lstEmpenhos" placeholder="Selecione um Empenho" allowFiltering="true" filterType="Contains" dataSource="@ViewData["dataTodosEmpenhos"]" popupHeight="200px" width="100%" showClearButton="true" ejs-for="@Model.MultaObj.Multa.EmpenhoMultaId">
+                                        <e-combobox-fields text="NotaEmpenho" value="EmpenhoMultaId"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
                                 <div class="col-12 col-md-6 col-xl-4">
                                     <label class="label font-weight-bold">Saldo do Empenho</label>
-                                    <input id="txtEmpenhoSaldo" class="form-control form-control-xs" type="text"
-                                        readonly disabled style="background-color: #e9ecef;" />
+                                    <input id="txtEmpenhoSaldo" class="form-control form-control-xs" type="text" readonly disabled style="background-color: #e9ecef;" />
                                 </div>
                             </div>
 
@@ -557,30 +502,19 @@
                             <div class="row">
                                 <div class="col-12 col-md-6 col-xl-4">
                                     <label class="label font-weight-bold">Veículo</label>
-                                    <ejs-combobox id="lstVeiculo" placeholder="Selecione um Veículo"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["dataVeiculo"]" popupHeight="200px" width="100%"
-                                        showClearButton="true" ejs-for="@Model.MultaObj.Multa.VeiculoId">
+                                    <ejs-combobox id="lstVeiculo" placeholder="Selecione um Veículo" allowFiltering="true" filterType="Contains" dataSource="@ViewData["dataVeiculo"]" popupHeight="200px" width="100%" showClearButton="true" ejs-for="@Model.MultaObj.Multa.VeiculoId">
                                         <e-combobox-fields text="Descricao" value="VeiculoId"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
                                 <div class="col-12 col-md-6 col-xl-4">
                                     <label class="label font-weight-bold">Contrato do Veículo</label>
-                                    <ejs-combobox id="lstContratoVeiculo" placeholder="Selecione um Contrato"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["dataContratoVeiculos"]" popupHeight="200px" width="100%"
-                                        showClearButton="true" ejs-for="@Model.MultaObj.Multa.ContratoVeiculoId"
-                                        enabled="false">
+                                    <ejs-combobox id="lstContratoVeiculo" placeholder="Selecione um Contrato" allowFiltering="true" filterType="Contains" dataSource="@ViewData["dataContratoVeiculos"]" popupHeight="200px" width="100%" showClearButton="true" ejs-for="@Model.MultaObj.Multa.ContratoVeiculoId" enabled="false">
                                         <e-combobox-fields text="Descricao" value="ContratoId"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
                                 <div class="col-12 col-md-6 col-xl-4">
                                     <label class="label font-weight-bold">Ata do Veículo</label>
-                                    <ejs-combobox id="lstAtaVeiculo" placeholder="Selecione uma Ata"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["dataAtaVeiculos"]" popupHeight="200px" width="100%"
-                                        showClearButton="true" ejs-for="@Model.MultaObj.Multa.AtaVeiculoId"
-                                        enabled="false">
+                                    <ejs-combobox id="lstAtaVeiculo" placeholder="Selecione uma Ata" allowFiltering="true" filterType="Contains" dataSource="@ViewData["dataAtaVeiculos"]" popupHeight="200px" width="100%" showClearButton="true" ejs-for="@Model.MultaObj.Multa.AtaVeiculoId" enabled="false">
                                         <e-combobox-fields text="Descricao" value="AtaId"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
@@ -589,20 +523,13 @@
                             <div class="row">
                                 <div class="col-12 col-md-6 col-xl-4">
                                     <label class="label font-weight-bold">Motorista</label>
-                                    <ejs-combobox id="lstMotorista" placeholder="Selecione um Motorista"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["dataMotorista"]" popupHeight="200px" width="100%"
-                                        showClearButton="true" ejs-for="@Model.MultaObj.Multa.MotoristaId">
+                                    <ejs-combobox id="lstMotorista" placeholder="Selecione um Motorista" allowFiltering="true" filterType="Contains" dataSource="@ViewData["dataMotorista"]" popupHeight="200px" width="100%" showClearButton="true" ejs-for="@Model.MultaObj.Multa.MotoristaId">
                                         <e-combobox-fields text="Nome" value="MotoristaId"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
                                 <div class="col-12 col-md-6 col-xl-4">
                                     <label class="label font-weight-bold">Contrato do Motorista</label>
-                                    <ejs-combobox id="lstContratoMotorista" placeholder="Selecione um Contrato"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["dataContratoMotoristas"]" popupHeight="200px"
-                                        width="100%" showClearButton="true"
-                                        ejs-for="@Model.MultaObj.Multa.ContratoMotoristaId" enabled="false">
+                                    <ejs-combobox id="lstContratoMotorista" placeholder="Selecione um Contrato" allowFiltering="true" filterType="Contains" dataSource="@ViewData["dataContratoMotoristas"]" popupHeight="200px" width="100%" showClearButton="true" ejs-for="@Model.MultaObj.Multa.ContratoMotoristaId" enabled="false">
                                         <e-combobox-fields text="Descricao" value="ContratoId"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
@@ -614,19 +541,21 @@
                                 <div class="col-md-12 col-xl-10">
                                     <label class="label font-weight-bold">Notificação de Autuação (PDF)</label>
 
-                                    <ejs-uploader id="uploaderAutuacao" name="UploadFiles" multiple="false"
-                                        autoUpload="true" allowedExtensions=".pdf">
+                                    <ejs-uploader id="uploaderAutuacao"
+                                                  name="UploadFiles"
+                                                  multiple="false"
+                                                  autoUpload="true"
+                                                  allowedExtensions=".pdf">
                                         <e-uploader-asyncsettings saveUrl="/api/MultaUpload/save"
-                                            removeUrl="/api/MultaUpload/remove">
+                                                                  removeUrl="/api/MultaUpload/remove">
                                         </e-uploader-asyncsettings>
                                     </ejs-uploader>
 
-                                    <input id="txtAutuacaoPDF" class="form-control form-control-xs"
-                                        asp-for="MultaObj.Multa.AutuacaoPDF" hidden />
+                                    <input id="txtAutuacaoPDF" class="form-control form-control-xs" asp-for="MultaObj.Multa.AutuacaoPDF" hidden />
 
                                     <ejs-pdfviewer id="pdfviewerAutuacao"
-                                        style="height:500px;width:100%;margin-top:10px;"
-                                        serviceUrl="/api/MultaPdfViewer">
+                                                   style="height:500px;width:100%;margin-top:10px;"
+                                                   serviceUrl="/api/MultaPdfViewer">
                                     </ejs-pdfviewer>
                                 </div>
                             </div>
@@ -637,19 +566,21 @@
                                 <div class="col-md-12 col-xl-10">
                                     <label class="label font-weight-bold">Notificação de Penalidade (PDF)</label>
 
-                                    <ejs-uploader id="uploaderPenalidade" name="UploadFiles" multiple="false"
-                                        autoUpload="true" allowedExtensions=".pdf">
+                                    <ejs-uploader id="uploaderPenalidade"
+                                                  name="UploadFiles"
+                                                  multiple="false"
+                                                  autoUpload="true"
+                                                  allowedExtensions=".pdf">
                                         <e-uploader-asyncsettings saveUrl="/api/MultaUpload/save"
-                                            removeUrl="/api/MultaUpload/remove">
+                                                                  removeUrl="/api/MultaUpload/remove">
                                         </e-uploader-asyncsettings>
                                     </ejs-uploader>
 
-                                    <input id="txtPenalidadePDF" class="form-control form-control-xs"
-                                        asp-for="MultaObj.Multa.PenalidadePDF" hidden />
+                                    <input id="txtPenalidadePDF" class="form-control form-control-xs" asp-for="MultaObj.Multa.PenalidadePDF" hidden />
 
                                     <ejs-pdfviewer id="pdfviewerPenalidade"
-                                        style="height:500px;width:100%;margin-top:10px;"
-                                        serviceUrl="/api/MultaPdfViewer">
+                                                   style="height:500px;width:100%;margin-top:10px;"
+                                                   serviceUrl="/api/MultaPdfViewer">
                                     </ejs-pdfviewer>
                                 </div>
                             </div>
@@ -660,18 +591,21 @@
                                 <div class="col-md-12 col-xl-10">
                                     <label class="label font-weight-bold">Processo e-Doc (PDF)</label>
 
-                                    <ejs-uploader id="uploaderEDoc" name="UploadFiles" multiple="false"
-                                        autoUpload="true" allowedExtensions=".pdf">
+                                    <ejs-uploader id="uploaderEDoc"
+                                                  name="UploadFiles"
+                                                  multiple="false"
+                                                  autoUpload="true"
+                                                  allowedExtensions=".pdf">
                                         <e-uploader-asyncsettings saveUrl="/api/MultaUpload/save"
-                                            removeUrl="/api/MultaUpload/remove">
+                                                                  removeUrl="/api/MultaUpload/remove">
                                         </e-uploader-asyncsettings>
                                     </ejs-uploader>
 
-                                    <input id="txtProcessoEdocPDF" class="form-control form-control-xs"
-                                        asp-for="MultaObj.Multa.ProcessoEdocPDF" hidden />
-
-                                    <ejs-pdfviewer id="pdfviewerEDoc" style="height:500px;width:100%;margin-top:10px;"
-                                        serviceUrl="/api/MultaPdfViewer">
+                                    <input id="txtProcessoEdocPDF" class="form-control form-control-xs" asp-for="MultaObj.Multa.ProcessoEdocPDF" hidden />
+
+                                    <ejs-pdfviewer id="pdfviewerEDoc"
+                                                   style="height:500px;width:100%;margin-top:10px;"
+                                                   serviceUrl="/api/MultaPdfViewer">
                                     </ejs-pdfviewer>
                                 </div>
                             </div>
@@ -682,19 +616,21 @@
                                 <div class="col-md-12 col-xl-10">
                                     <label class="label font-weight-bold">Comprovante de Pagamento (PDF)</label>
 
-                                    <ejs-uploader id="uploaderComprovante" name="UploadFiles" multiple="false"
-                                        autoUpload="true" allowedExtensions=".pdf">
+                                    <ejs-uploader id="uploaderComprovante"
+                                                  name="UploadFiles"
+                                                  multiple="false"
+                                                  autoUpload="true"
+                                                  allowedExtensions=".pdf">
                                         <e-uploader-asyncsettings saveUrl="/api/MultaUpload/save"
-                                            removeUrl="/api/MultaUpload/remove">
+                                                                  removeUrl="/api/MultaUpload/remove">
                                         </e-uploader-asyncsettings>
                                     </ejs-uploader>
 
-                                    <input id="txtComprovantePDF" class="form-control form-control-xs"
-                                        asp-for="MultaObj.Multa.ComprovantePDF" hidden />
+                                    <input id="txtComprovantePDF" class="form-control form-control-xs" asp-for="MultaObj.Multa.ComprovantePDF" hidden />
 
                                     <ejs-pdfviewer id="pdfviewerComprovante"
-                                        style="height:500px;width:100%;margin-top:10px;"
-                                        serviceUrl="/api/MultaPdfViewer">
+                                                   style="height:500px;width:100%;margin-top:10px;"
+                                                   serviceUrl="/api/MultaPdfViewer">
                                     </ejs-pdfviewer>
                                 </div>
                             </div>
@@ -705,12 +641,9 @@
                                 <div class="col-md-12 col-xl-10 control-section">
                                     <div class="control-wrapper">
                                         <div>
-                                            <label class="label font-weight-bold">Observações a respeito da
-                                                Multa</label>
-                                            <ejs-richtexteditor ejs-for="@Model.MultaObj.Multa.Observacao" id="rte"
-                                                toolbarClick="toolbarClick" locale="pt-BR" height="150px">
-                                                <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage"
-                                                    path="./DadosEditaveis/ImagensViagens/"></e-richtexteditor-insertimagesettings>
+                                            <label class="label font-weight-bold">Observações a respeito da Multa</label>
+                                            <ejs-richtexteditor ejs-for="@Model.MultaObj.Multa.Observacao" id="rte" toolbarClick="toolbarClick" locale="pt-BR" height="150px">
+                                                <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage" path="./DadosEditaveis/ImagensViagens/"></e-richtexteditor-insertimagesettings>
                                             </ejs-richtexteditor>
                                             <div id="errorMessage">
                                                 <span asp-validation-for="@Model.MultaObj.Multa.Observacao"></span>
@@ -729,22 +662,16 @@
                                         <div id="divSubmit" class="col-12 col-md-4 pb-2">
                                             @if (Model.MultaObj.Multa.MultaId != Guid.Empty)
                                             {
-                                                <button id="btnSubmit" method="post" asp-page-handler="Edit"
-                                                    asp-route-id="@Model.MultaObj.Multa.MultaId"
-                                                    class="btn btn-azul btn-submit-spin form-control">
-                                                    <span class="spinner-border spinner-border-sm d-none" role="status"
-                                                        aria-hidden="true"></span>
+                                                <button id="btnSubmit" method="post" asp-page-handler="Edit" asp-route-id="@Model.MultaObj.Multa.MultaId" class="btn btn-azul btn-submit-spin form-control">
+                                                    <span class="spinner-border spinner-border-sm d-none" role="status" aria-hidden="true"></span>
                                                     <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
                                                     <span class="btn-text">Atualizar Penalidade</span>
                                                 </button>
                                             }
                                             else
                                             {
-                                                <button id="btnSubmit" type="submit" value="Submit"
-                                                    asp-page-handler="Submit"
-                                                    class="btn btn-azul btn-submit-spin form-control">
-                                                    <span class="spinner-border spinner-border-sm d-none" role="status"
-                                                        aria-hidden="true"></span>
+                                                <button id="btnSubmit" type="submit" value="Submit" asp-page-handler="Submit" class="btn btn-azul btn-submit-spin form-control">
+                                                    <span class="spinner-border spinner-border-sm d-none" role="status" aria-hidden="true"></span>
                                                     <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
                                                     <span class="btn-text">Criar Penalidade</span>
                                                 </button>
@@ -754,32 +681,24 @@
                                         <div class="col-12" hidden>
                                             @if (Model.MultaObj.Multa.MultaId != Guid.Empty)
                                             {
-                                                <button id="btnEscondido" method="post" asp-page-handler="Edit"
-                                                    asp-route-id="@Model.MultaObj.Multa.MultaId"
-                                                    class="btn btn-azul form-control">
+                                                <button id="btnEscondido" method="post" asp-page-handler="Edit" asp-route-id="@Model.MultaObj.Multa.MultaId" class="btn btn-azul form-control">
                                                     <span class="d-flex justify-content-center align-items-center">
-                                                        <i
-                                                            class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i><span>&nbsp;Atualizar
-                                                            Penalidade</span>
+                                                        <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i><span>&nbsp;Atualizar Penalidade</span>
                                                     </span>
                                                 </button>
                                             }
                                             else
                                             {
-                                                <button id="btnEscondido" type="submit" value="Submit"
-                                                    asp-page-handler="Submit" class="btn btn-azul form-control">
+                                                <button id="btnEscondido" type="submit" value="Submit" asp-page-handler="Submit" class="btn btn-azul form-control">
                                                     <span class="d-flex justify-content-center align-items-center">
-                                                        <i
-                                                            class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i><span>&nbsp;Criar
-                                                            Penalidade</span>
+                                                        <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i><span>&nbsp;Criar Penalidade</span>
                                                     </span>
                                                 </button>
                                             }
                                         </div>
 
                                         <div class="col-12 col-md-4">
-                                            <a asp-page="./ListaPenalidade" class="btn btn-ftx-fechar form-control"
-                                                data-ftx-loading>
+                                            <a asp-page="./ListaPenalidade" class="btn btn-ftx-fechar form-control" data-ftx-loading>
                                                 <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i>
                                                 <span>Cancelar Operação</span>
                                             </a>
@@ -796,8 +715,7 @@
     </div>
 </form>
 
-<div class="modal fade" id="modalFicha" tabindex="-1" aria-hidden="true" data-bs-backdrop="static"
-    data-bs-keyboard="false">
+<div class="modal fade" id="modalFicha" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
     <div class="modal-dialog modal-lg" role="document">
         <div class="modal-content">
             <div class="modal-header">
@@ -805,8 +723,7 @@
                     <i class="fa-duotone fa-file-lines"></i>
                     Ficha de Vistoria
                 </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <div class="row">
@@ -816,8 +733,7 @@
                 </div>
             </div>
             <div class="modal-footer">
-                <button id="btnFecharModalFichaVistoria" type="button" class="btn btn-modal-fechar"
-                    data-bs-dismiss="modal">
+                <button id="btnFecharModalFichaVistoria" type="button" class="btn btn-modal-fechar" data-bs-dismiss="modal">
                     <i class="fa-duotone fa-rotate-left"></i>
                     Fechar
                 </button>
@@ -828,10 +744,8 @@
 
 @section ScriptsBlock {
 
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script>
         const MultaId = "@(Model.MultaObj.Multa?.MultaId.ToString() ?? "")";
@@ -845,20 +759,7 @@
     </script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * UTILITÁRIOS DE FORMATAÇÃO DE MOEDA
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Funções para conversão entre formatos brasileiro e numérico
-         * para campos de valor monetário.
-         */
-
-        /**
-         * Converte valor em formato brasileiro para número.
-         * @@param {string} str - Valor no formato "R$ 10.000,50" ou "10.000,50".
-         * @@returns {number} Valor numérico (ex: 10000.50).
-         * @@example parseCurrencyBR("R$ 1.234,56")
-         */
+
         function parseCurrencyBR(str) {
             try {
                 if (!str) return 0;
@@ -874,15 +775,6 @@
             }
         }
 
-        /**
-         * Máscara de moeda em tempo real para campos input.
-         * @@param {HTMLInputElement} input - Campo de entrada.
-         * @@param {string} sep - Separador de milhar (padrão ".").
-         * @@param {string} dec - Separador decimal (padrão ",").
-         * @@param {KeyboardEvent} event - Evento de teclado.
-         * @@returns {boolean} False para prevenir input padrão.
-         * @@description Para R$ 1.000,00 digite 100000 (últimos 2 = centavos).
-         */
         function moeda(input, sep, dec, event) {
             try {
                 let digitado = "",
@@ -900,8 +792,8 @@
                 let valorAtual = input.value.replace('R$ ', '');
 
                 for (tamanho = valorAtual.length, i = 0;
-                    i < tamanho && (valorAtual.charAt(i) === "0" || valorAtual.charAt(i) === dec);
-                    i++);
+                     i < tamanho && (valorAtual.charAt(i) === "0" || valorAtual.charAt(i) === dec);
+                     i++);
 
                 for (limpo = ""; i < tamanho; i++) {
                     if ("0123456789".indexOf(valorAtual.charAt(i)) !== -1) {
@@ -945,11 +837,6 @@
             }
         }
 
-        /**
-         * Formata número para exibição em formato brasileiro.
-         * @@param {number} valor - Valor numérico.
-         * @@returns {string} Valor formatado "R$ 10.000,50".
-         */
         function formatCurrencyBR(valor) {
             try {
                 if (!valor && valor !== 0) return "";
```
