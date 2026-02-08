# Pages/Empenho/Upsert.cshtml

**Mudanca:** GRANDE | **+169** linhas | **-303** linhas

---

```diff
--- JANEIRO: Pages/Empenho/Upsert.cshtml
+++ ATUAL: Pages/Empenho/Upsert.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
 
 @model FrotiX.Pages.Empenho.UpsertModel
@@ -333,29 +332,14 @@
         }
 
         @@keyframes ftxHeaderGradientShift {
-            0% {
-                background-position: 0% 50%;
-            }
-
-            50% {
-                background-position: 100% 50%;
-            }
-
-            100% {
-                background-position: 0% 50%;
-            }
+            0% { background-position: 0% 50%; }
+            50% { background-position: 100% 50%; }
+            100% { background-position: 0% 50%; }
         }
 
         @@keyframes ftxHeaderShine {
-
-            0%,
-            100% {
-                left: -100%;
-            }
-
-            50% {
-                left: 100%;
-            }
+            0%, 100% { left: -100%; }
+            50% { left: 100%; }
         }
 
         .modal-header-dinheiro .modal-title {
@@ -452,14 +436,11 @@
                                 <i class="fa-duotone fa-money-check-dollar-pen"></i>
                                 @(Model.EmpenhoObj.Empenho.EmpenhoId != Guid.Empty ? "Atualizar Empenho" : "Criar Novo Empenho")
                             </h2>
-                            @if (Model.EmpenhoObj.Empenho.EmpenhoId != Guid.Empty &&
-                                                        !string.IsNullOrEmpty(Model.EmpenhoObj.Empenho.NotaEmpenho))
+                            @if (Model.EmpenhoObj.Empenho.EmpenhoId != Guid.Empty && !string.IsNullOrEmpty(Model.EmpenhoObj.Empenho.NotaEmpenho))
                             {
                                 <div class="ftx-card-actions">
-                                    <span class="badge"
-                                        style="background: rgba(255, 255, 255, 0.2); padding: 0.5rem 1rem; border-radius: 20px; font-weight: 600; font-size: 0.95rem; color: #ffd700;">
-                                        <i
-                                            class="fa-duotone fa-hashtag me-1 icon-pulse me-1"></i>@Model.EmpenhoObj.Empenho.NotaEmpenho
+                                    <span class="badge" style="background: rgba(255, 255, 255, 0.2); padding: 0.5rem 1rem; border-radius: 20px; font-weight: 600; font-size: 0.95rem; color: #ffd700;">
+                                        <i class="fa-duotone fa-hashtag me-1 icon-pulse me-1"></i>@Model.EmpenhoObj.Empenho.NotaEmpenho
                                     </span>
                                 </div>
                             }
@@ -472,10 +453,11 @@
                                         <label class="ftx-label" asp-for="EmpenhoObj.Empenho.NotaEmpenho">
                                             <i class="fa-duotone fa-file-invoice"></i> Nota de Empenho
                                         </label>
-                                        <span class="text-danger"
-                                            asp-validation-for="EmpenhoObj.Empenho.NotaEmpenho"></span>
-                                        <input id="txtNotaEmpenho" class="form-control ftx-form-control"
-                                            asp-for="EmpenhoObj.Empenho.NotaEmpenho" placeholder="Ex: 2024NE00123" />
+                                        <span class="text-danger" asp-validation-for="EmpenhoObj.Empenho.NotaEmpenho"></span>
+                                        <input id="txtNotaEmpenho"
+                                               class="form-control ftx-form-control"
+                                               asp-for="EmpenhoObj.Empenho.NotaEmpenho"
+                                               placeholder="Ex: 2024NE00123" />
                                     </div>
                                 </div>
                                 <div class="col-md-2 col-12">
@@ -483,10 +465,11 @@
                                         <label class="ftx-label" asp-for="EmpenhoObj.Empenho.DataEmissao">
                                             <i class="fa-duotone fa-calendar-day"></i> Data de Emissão
                                         </label>
-                                        <span class="text-danger"
-                                            asp-validation-for="EmpenhoObj.Empenho.DataEmissao"></span>
-                                        <input id="txtDataEmissao" class="form-control ftx-form-control"
-                                            asp-for="EmpenhoObj.Empenho.DataEmissao" type="date" />
+                                        <span class="text-danger" asp-validation-for="EmpenhoObj.Empenho.DataEmissao"></span>
+                                        <input id="txtDataEmissao"
+                                               class="form-control ftx-form-control"
+                                               asp-for="EmpenhoObj.Empenho.DataEmissao"
+                                               type="date" />
                                     </div>
                                 </div>
                                 <div class="col-md-2 col-12">
@@ -494,10 +477,11 @@
                                         <label class="ftx-label" asp-for="EmpenhoObj.Empenho.VigenciaInicial">
                                             <i class="fa-duotone fa-calendar-arrow-down"></i> Vigência Inicial
                                         </label>
-                                        <span class="text-danger"
-                                            asp-validation-for="EmpenhoObj.Empenho.VigenciaInicial"></span>
-                                        <input id="txtVigenciaInicial" class="form-control ftx-form-control"
-                                            asp-for="EmpenhoObj.Empenho.VigenciaInicial" type="date" />
+                                        <span class="text-danger" asp-validation-for="EmpenhoObj.Empenho.VigenciaInicial"></span>
+                                        <input id="txtVigenciaInicial"
+                                               class="form-control ftx-form-control"
+                                               asp-for="EmpenhoObj.Empenho.VigenciaInicial"
+                                               type="date" />
                                     </div>
                                 </div>
                                 <div class="col-md-2 col-12">
@@ -505,10 +489,11 @@
                                         <label class="ftx-label" asp-for="EmpenhoObj.Empenho.VigenciaFinal">
                                             <i class="fa-duotone fa-calendar-arrow-up"></i> Vigência Final
                                         </label>
-                                        <span class="text-danger"
-                                            asp-validation-for="EmpenhoObj.Empenho.VigenciaFinal"></span>
-                                        <input id="txtVigenciaFinal" class="form-control ftx-form-control"
-                                            asp-for="EmpenhoObj.Empenho.VigenciaFinal" type="date" />
+                                        <span class="text-danger" asp-validation-for="EmpenhoObj.Empenho.VigenciaFinal"></span>
+                                        <input id="txtVigenciaFinal"
+                                               class="form-control ftx-form-control"
+                                               asp-for="EmpenhoObj.Empenho.VigenciaFinal"
+                                               type="date" />
                                     </div>
                                 </div>
                                 <div class="col-md-2 col-12">
@@ -516,10 +501,10 @@
                                         <label class="ftx-label" asp-for="EmpenhoObj.Empenho.AnoVigencia">
                                             <i class="fa-duotone fa-calendar-check"></i> Ano de Vigência
                                         </label>
-                                        <span class="text-danger font-weight-light"
-                                            asp-validation-for="EmpenhoObj.Empenho.AnoVigencia"></span>
-                                        <select id="lstVigencia" class="form-control ftx-form-control"
-                                            asp-for="EmpenhoObj.Empenho.AnoVigencia">
+                                        <span class="text-danger font-weight-light" asp-validation-for="EmpenhoObj.Empenho.AnoVigencia"></span>
+                                        <select id="lstVigencia"
+                                                class="form-control ftx-form-control"
+                                                asp-for="EmpenhoObj.Empenho.AnoVigencia">
                                             <option value="">-- Ano --</option>
                                             <option value="2026">2026</option>
                                             <option value="2025">2025</option>
@@ -554,12 +539,11 @@
                                         <label class="ftx-label" asp-for="EmpenhoObj.Empenho.ContratoId">
                                             <i class="fa-duotone fa-file-signature"></i> Contrato
                                         </label>
-                                        <span class="text-danger"
-                                            asp-validation-for="EmpenhoObj.Empenho.ContratoId"></span>
+                                        <span class="text-danger" asp-validation-for="EmpenhoObj.Empenho.ContratoId"></span>
                                         @Html.DropDownListFor(m => m.EmpenhoObj.Empenho.ContratoId,
-                                        Model.EmpenhoObj.ContratoList,
-                                                                                "- Selecione um Contrato -",
-                                                                                new { @class = "form-control ftx-form-control" })
+                                            Model.EmpenhoObj.ContratoList,
+                                            "- Selecione um Contrato -",
+                                            new { @class = "form-control ftx-form-control" })
                                     </div>
                                 </div>
 
@@ -570,9 +554,9 @@
                                         </label>
                                         <span class="text-danger" asp-validation-for="EmpenhoObj.Empenho.AtaId"></span>
                                         @Html.DropDownListFor(m => m.EmpenhoObj.Empenho.AtaId,
-                                                                                Model.EmpenhoObj.AtaList,
-                                                                                "- Selecione uma Ata -",
-                                                                                new { @class = "form-control ftx-form-control" })
+                                            Model.EmpenhoObj.AtaList,
+                                            "- Selecione uma Ata -",
+                                            new { @class = "form-control ftx-form-control" })
                                     </div>
                                 </div>
                             </div>
@@ -583,12 +567,14 @@
                                         <label class="ftx-label" asp-for="EmpenhoObj.Empenho.SaldoInicial">
                                             <i class="fa-duotone fa-coins"></i> Saldo Inicial (R$)
                                         </label>
-                                        <span class="text-danger"
-                                            asp-validation-for="EmpenhoObj.Empenho.SaldoInicial"></span>
-                                        <input id="SaldoInicial" class="form-control ftx-form-control"
-                                            data-inputmask="'alias': 'currency'" style="text-align: right;"
-                                            asp-for="EmpenhoObj.Empenho.SaldoInicial"
-                                            onKeyPress="return(moeda(this,'.',',',event))" placeholder="0,00" />
+                                        <span class="text-danger" asp-validation-for="EmpenhoObj.Empenho.SaldoInicial"></span>
+                                        <input id="SaldoInicial"
+                                               class="form-control ftx-form-control"
+                                               data-inputmask="'alias': 'currency'"
+                                               style="text-align: right;"
+                                               asp-for="EmpenhoObj.Empenho.SaldoInicial"
+                                               onKeyPress="return(moeda(this,'.',',',event))"
+                                               placeholder="0,00" />
                                     </div>
                                 </div>
                                 <div class="col-md-3 col-12">
@@ -596,13 +582,15 @@
                                         <label class="ftx-label" asp-for="EmpenhoObj.Empenho.SaldoFinal">
                                             <i class="fa-duotone fa-sack-dollar"></i> Saldo Final (R$)
                                         </label>
-                                        <span class="text-danger"
-                                            asp-validation-for="EmpenhoObj.Empenho.SaldoFinal"></span>
-                                        <input id="SaldoFinal" class="form-control ftx-form-control" disabled
-                                            data-inputmask="'alias': 'currency'"
-                                            style="text-align: right; background-color: #e8f5e9;"
-                                            asp-for="EmpenhoObj.Empenho.SaldoFinal"
-                                            onKeyPress="return(moeda(this,'.',',',event))" placeholder="0,00" />
+                                        <span class="text-danger" asp-validation-for="EmpenhoObj.Empenho.SaldoFinal"></span>
+                                        <input id="SaldoFinal"
+                                               class="form-control ftx-form-control"
+                                               disabled
+                                               data-inputmask="'alias': 'currency'"
+                                               style="text-align: right; background-color: #e8f5e9;"
+                                               asp-for="EmpenhoObj.Empenho.SaldoFinal"
+                                               onKeyPress="return(moeda(this,'.',',',event))"
+                                               placeholder="0,00" />
                                     </div>
                                 </div>
                                 <div class="col-md-3 col-12">
@@ -610,24 +598,30 @@
                                         <label class="ftx-label">
                                             <i class="fa-duotone fa-file-invoice-dollar"></i> Total Notas Fiscais
                                         </label>
-                                        <input id="SaldoNotas" class="form-control ftx-form-control"
-                                            data-inputmask="'alias': 'currency'"
-                                            style="text-align: right; background-color: #fff3e0;" disabled
-                                            placeholder="0,00" />
+                                        <input id="SaldoNotas"
+                                               class="form-control ftx-form-control"
+                                               data-inputmask="'alias': 'currency'"
+                                               style="text-align: right; background-color: #fff3e0;"
+                                               disabled
+                                               placeholder="0,00" />
                                     </div>
                                 </div>
                                 <div class="col-md-3 col-12 d-flex align-items-end">
                                     @if (Model.EmpenhoObj.Empenho.EmpenhoId != Guid.Empty)
                                     {
-                                        <button type="button" id="btnVerNotas"
-                                            class="ftx-btn ftx-btn-azul ftx-btn-notas w-100">
+                                        <button type="button"
+                                                id="btnVerNotas"
+                                                class="ftx-btn ftx-btn-azul ftx-btn-notas w-100">
                                             <i class="fa-duotone fa-file-invoice-dollar"></i> Ver Notas Fiscais
                                         </button>
                                     }
                                     else
                                     {
-                                        <button type="button" id="btnVerNotas" class="ftx-btn ftx-btn-notas w-100"
-                                            style="background: #9ca3af; cursor: not-allowed;" disabled>
+                                        <button type="button"
+                                                id="btnVerNotas"
+                                                class="ftx-btn ftx-btn-notas w-100"
+                                                style="background: #9ca3af; cursor: not-allowed;"
+                                                disabled>
                                             <i class="fa-duotone fa-file-invoice-dollar"></i> Ver Notas Fiscais
                                         </button>
                                     }
@@ -640,19 +634,22 @@
                                         <div id="divSubmit" class="col-12 col-md-4 pb-2">
                                             @if (Model.EmpenhoObj.Empenho.EmpenhoId != Guid.Empty)
                                             {
-                                                <button id="btnSubmit" method="post" asp-page-handler="Edit"
-                                                    asp-route-id=@Model.EmpenhoObj.Empenho.EmpenhoId
-                                                    class="btn btn-azul btn-submit-spin w-100">
-                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
-                                                    Atualizar Empenho
+                                                <button id="btnSubmit"
+                                                        method="post"
+                                                        asp-page-handler="Edit"
+                                                        asp-route-id=@Model.EmpenhoObj.Empenho.EmpenhoId
+                                                        class="btn btn-azul btn-submit-spin w-100">
+                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i> Atualizar Empenho
                                                 </button>
                                             }
                                             else
                                             {
-                                                <button id="btnSubmit" type="submit" value="Submit"
-                                                    asp-page-handler="Submit" class="btn btn-azul btn-submit-spin w-100">
-                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i> Criar
-                                                    Empenho
+                                                <button id="btnSubmit"
+                                                        type="submit"
+                                                        value="Submit"
+                                                        asp-page-handler="Submit"
+                                                        class="btn btn-azul btn-submit-spin w-100">
+                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i> Criar Empenho
                                                 </button>
                                             }
                                         </div>
@@ -660,16 +657,21 @@
                                         <div class="col-12" hidden>
                                             @if (Model.EmpenhoObj.Empenho.EmpenhoId != Guid.Empty)
                                             {
-                                                <button id="btnEscondido" method="post" asp-page-handler="Edit"
-                                                    asp-route-id=@Model.EmpenhoObj.Empenho.EmpenhoId
-                                                    class="btn btn-azul form-control">
+                                                <button id="btnEscondido"
+                                                        method="post"
+                                                        asp-page-handler="Edit"
+                                                        asp-route-id=@Model.EmpenhoObj.Empenho.EmpenhoId
+                                                        class="btn btn-azul form-control">
                                                     Atualizar
                                                 </button>
                                             }
                                             else
                                             {
-                                                <button id="btnEscondido" type="submit" value="Submit"
-                                                    asp-page-handler="Submit" class="btn btn-azul form-control">
+                                                <button id="btnEscondido"
+                                                        type="submit"
+                                                        value="Submit"
+                                                        asp-page-handler="Submit"
+                                                        class="btn btn-azul form-control">
                                                     Criar
                                                 </button>
                                             }
@@ -677,8 +679,7 @@
 
                                         <div class="col-12 col-md-4">
                                             <a asp-page="./Index" class="btn btn-ftx-fechar w-100" data-ftx-loading>
-                                                <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i>
-                                                Cancelar Operação
+                                                <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Cancelar Operação
                                             </a>
                                         </div>
                                     </div>
@@ -714,11 +715,9 @@
                                             </table>
                                         </div>
 
-                                        <div class="p-3 text-center"
-                                            style="background: rgba(45, 74, 45, 0.05); border-top: 1px solid rgba(45, 74, 45, 0.1);">
+                                        <div class="p-3 text-center" style="background: rgba(45, 74, 45, 0.05); border-top: 1px solid rgba(45, 74, 45, 0.1);">
                                             <button type="button" id="btnAdicionarAporte" class="btn btn-verde">
-                                                <i class="fa-duotone fa-circle-plus icon-space icon-pulse"></i> Adicionar
-                                                Aporte
+                                                <i class="fa-duotone fa-circle-plus icon-space icon-pulse"></i> Adicionar Aporte
                                             </button>
                                         </div>
                                     </div>
@@ -748,11 +747,9 @@
                                             </table>
                                         </div>
 
-                                        <div class="p-3 text-center"
-                                            style="background: rgba(114, 47, 55, 0.05); border-top: 1px solid rgba(114, 47, 55, 0.1);">
+                                        <div class="p-3 text-center" style="background: rgba(114, 47, 55, 0.05); border-top: 1px solid rgba(114, 47, 55, 0.1);">
                                             <button type="button" id="btnAdicionarAnulacao" class="btn btn-vinho">
-                                                <i class="fa-duotone fa-circle-minus icon-space icon-pulse"></i> Adicionar
-                                                Anulação
+                                                <i class="fa-duotone fa-circle-minus icon-space icon-pulse"></i> Adicionar Anulação
                                             </button>
                                         </div>
                                     </div>
@@ -769,13 +766,11 @@
 <div class="modal fade" id="modalAporte" tabindex="-1" aria-labelledby="ModalLabelAporte" aria-hidden="true">
     <div class="modal-dialog modal-lg">
         <div class="modal-content">
-            <div class="modal-header"
-                style="background: linear-gradient(135deg, #2d4a2d 0%, #3d5c3d 100%); color: #fff;">
+            <div class="modal-header" style="background: linear-gradient(135deg, #2d4a2d 0%, #3d5c3d 100%); color: #fff;">
                 <h5 class="modal-title" id="ModalLabelAporte">
                     <i class="fa-duotone fa-circle-plus me-2"></i>Editar Aporte do Empenho
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmAporte">
@@ -795,8 +790,7 @@
                                 <label class="ftx-label">
                                     <i class="fa-duotone fa-text"></i> Descrição
                                 </label>
-                                <input id="txtDescricaoAporte" class="form-control ftx-form-control"
-                                    placeholder="Descrição do aporte" />
+                                <input id="txtDescricaoAporte" class="form-control ftx-form-control" placeholder="Descrição do aporte" />
                             </div>
                         </div>
                     </div>
@@ -806,9 +800,12 @@
                                 <label class="ftx-label">
                                     <i class="fa-duotone fa-coins"></i> Valor do Aporte
                                 </label>
-                                <input id="txtValorAporte" class="form-control ftx-form-control"
-                                    data-inputmask="'alias': 'currency'" style="text-align: right;"
-                                    onKeyPress="return(moeda(this,'.',',',event))" placeholder="0,00" />
+                                <input id="txtValorAporte"
+                                       class="form-control ftx-form-control"
+                                       data-inputmask="'alias': 'currency'"
+                                       style="text-align: right;"
+                                       onKeyPress="return(moeda(this,'.',',',event))"
+                                       placeholder="0,00" />
                             </div>
                         </div>
                     </div>
@@ -829,13 +826,11 @@
 <div class="modal fade" id="modalanulacao" tabindex="-1" aria-labelledby="ModalLabelAnulacao" aria-hidden="true">
     <div class="modal-dialog modal-lg">
         <div class="modal-content">
-            <div class="modal-header"
-                style="background: linear-gradient(135deg, #722f37 0%, #8b3a44 100%); color: #fff;">
+            <div class="modal-header" style="background: linear-gradient(135deg, #722f37 0%, #8b3a44 100%); color: #fff;">
                 <h5 class="modal-title" id="ModalLabelAnulacao">
                     <i class="fa-duotone fa-circle-minus me-2"></i>Editar Anulação do Empenho
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmanulacao">
@@ -855,8 +850,7 @@
                                 <label class="ftx-label">
                                     <i class="fa-duotone fa-text"></i> Descrição
                                 </label>
-                                <input id="txtDescricaoanulacao" class="form-control ftx-form-control"
-                                    placeholder="Descrição da anulação" />
+                                <input id="txtDescricaoanulacao" class="form-control ftx-form-control" placeholder="Descrição da anulação" />
                             </div>
                         </div>
                     </div>
@@ -866,9 +860,12 @@
                                 <label class="ftx-label">
                                     <i class="fa-duotone fa-coins"></i> Valor da Anulação
                                 </label>
-                                <input id="txtValoranulacao" class="form-control ftx-form-control"
-                                    data-inputmask="'alias': 'currency'" style="text-align: right;"
-                                    onKeyPress="return(moeda(this,'.',',',event))" placeholder="0,00" />
+                                <input id="txtValoranulacao"
+                                       class="form-control ftx-form-control"
+                                       data-inputmask="'alias': 'currency'"
+                                       style="text-align: right;"
+                                       onKeyPress="return(moeda(this,'.',',',event))"
+                                       placeholder="0,00" />
                             </div>
                         </div>
                     </div>
@@ -904,7 +901,7 @@
                             <th>Valor</th>
                             <th>Valor de Glosa</th>
                             <th>Motivo da Glosa</th>
-                            <th>Ações</th>
+                            <th>Ação</th>
                             <th>ContratoId</th>
                             <th>EmpenhoId</th>
                         </tr>
@@ -921,8 +918,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalAdicionarAporte" data-bs-backdrop="static" data-bs-keyboard="true" tabindex="-1"
-    aria-labelledby="ModalLabelAdicionarAporte" aria-hidden="true">
+<div class="modal fade" id="modalAdicionarAporte" data-bs-backdrop="static" data-bs-keyboard="true" tabindex="-1" aria-labelledby="ModalLabelAdicionarAporte" aria-hidden="true">
     <div class="modal-dialog modal-lg">
         <div class="modal-content">
             <div class="modal-header modal-header-dinheiro">
@@ -930,8 +926,7 @@
                     <i class="fa-duotone fa-hand-holding-dollar me-2"></i>
                     <span>Aportar Valores ao Empenho</span>
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmAdicionarAporte">
@@ -947,8 +942,7 @@
                             <label class="ftx-label">
                                 <i class="fa-duotone fa-text"></i> Descrição
                             </label>
-                            <input id="txtDescricaoAdicionarAporte" class="form-control ftx-form-control"
-                                placeholder="Descrição do aporte..." />
+                            <input id="txtDescricaoAdicionarAporte" class="form-control ftx-form-control" placeholder="Descrição do aporte..." />
                         </div>
                     </div>
                     <div class="row g-3 mt-2">
@@ -956,9 +950,11 @@
                             <label class="ftx-label">
                                 <i class="fa-duotone fa-coins"></i> Valor do Aporte
                             </label>
-                            <input id="txtValorAdicionarAporte" class="form-control ftx-form-control"
-                                style="text-align: right;" onkeypress="return moeda(this,'.',',',event)"
-                                placeholder="0,00" />
+                            <input id="txtValorAdicionarAporte"
+                                   class="form-control ftx-form-control"
+                                   style="text-align: right;"
+                                   onkeypress="return moeda(this,'.',',',event)"
+                                   placeholder="0,00" />
                         </div>
                     </div>
                 </form>
@@ -975,8 +971,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalAdicionarAnulacao" data-bs-backdrop="static" data-bs-keyboard="true" tabindex="-1"
-    aria-labelledby="ModalLabelAdicionarAnulacao" aria-hidden="true">
+<div class="modal fade" id="modalAdicionarAnulacao" data-bs-backdrop="static" data-bs-keyboard="true" tabindex="-1" aria-labelledby="ModalLabelAdicionarAnulacao" aria-hidden="true">
     <div class="modal-dialog modal-lg">
         <div class="modal-content">
             <div class="modal-header modal-header-vinho">
@@ -984,8 +979,7 @@
                     <i class="fa-duotone fa-ban me-2"></i>
                     <span>Anular Valores do Empenho</span>
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmAdicionarAnulacao">
@@ -1001,8 +995,7 @@
                             <label class="ftx-label">
                                 <i class="fa-duotone fa-text"></i> Descrição
                             </label>
-                            <input id="txtDescricaoAdicionarAnulacao" class="form-control ftx-form-control"
-                                placeholder="Motivo da anulação..." />
+                            <input id="txtDescricaoAdicionarAnulacao" class="form-control ftx-form-control" placeholder="Motivo da anulação..." />
                         </div>
                     </div>
                     <div class="row g-3 mt-2">
@@ -1010,9 +1003,11 @@
                             <label class="ftx-label">
                                 <i class="fa-duotone fa-coins"></i> Valor da Anulação
                             </label>
-                            <input id="txtValorAdicionarAnulacao" class="form-control ftx-form-control"
-                                style="text-align: right;" onkeypress="return moeda(this,'.',',',event)"
-                                placeholder="0,00" />
+                            <input id="txtValorAdicionarAnulacao"
+                                   class="form-control ftx-form-control"
+                                   style="text-align: right;"
+                                   onkeypress="return moeda(this,'.',',',event)"
+                                   placeholder="0,00" />
                         </div>
                     </div>
                 </form>
@@ -1035,20 +1030,6 @@
     <script src="~/js/cadastros/anulacao_001.js" asp-append-version="true"></script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * FUNÇÕES UTILITÁRIAS DE CONVERSÃO E FORMATAÇÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Conjunto de funções para conversão entre formatos brasileiro
-         * e numérico, máscara de moeda e formatação de datas
-         */
-
-        /**
-         * Converte string monetária brasileira para número
-         * @@description Remove símbolos R$, pontos de milhar e converte vírgula decimal
-         * @@param {string} str - Valor formatado como moeda BR (ex: "R$ 10.000,50")
-         * @@returns {number} Valor numérico (ex: 10000.50), retorna 0 se inválido
-         */
         function parseCurrencyBR(str) {
             try {
                 if (!str) return 0;
@@ -1065,12 +1046,6 @@
             }
         }
 
-        /**
-         * Formata número para padrão monetário brasileiro
-         * @@description Converte número para string com 2 casas decimais, separador de milhar
-         * @@param {number|string} value - Valor numérico a ser formatado
-         * @@returns {string} Valor formatado (ex: "1.234,56"), retorna "0,00" se inválido
-         */
         function formatNumberBR(value) {
             try {
                 const num = typeof value === 'number' ? value : parseFloat(value);
@@ -1086,15 +1061,6 @@
             }
         }
 
-        /**
-         * Aplica máscara de moeda brasileira durante digitação
-         * @@description Formata valor em tempo real (últimos 2 dígitos = centavos)
-         * @@param {HTMLInputElement} input - Elemento input que recebe a digitação
-         * @@param {string} sep - Separador de milhar (geralmente ".")
-         * @@param {string} dec - Separador decimal (geralmente ",")
-         * @@param {Event} event - Evento de teclado (keypress)
-         * @@returns {boolean} false para prevenir inserção padrão do caractere
-         */
         function moeda(input, sep, dec, event) {
             try {
                 let digitado = "",
@@ -1110,8 +1076,8 @@
                 if ("0123456789".indexOf(digitado) === -1) return false;
 
                 for (tamanho = input.value.length, i = 0;
-                    i < tamanho && (input.value.charAt(i) === "0" || input.value.charAt(i) === dec);
-                    i++);
+                     i < tamanho && (input.value.charAt(i) === "0" || input.value.charAt(i) === dec);
+                     i++);
 
                 for (limpo = ""; i < tamanho; i++) {
                     if ("0123456789".indexOf(input.value.charAt(i)) !== -1) {
@@ -1155,12 +1121,6 @@
             }
         }
 
-        /**
-         * Converte data do formato brasileiro para ISO
-         * @@description Transforma "dd/mm/yyyy" em "yyyy-mm-dd" para APIs
-         * @@param {string} dateStr - Data em formato BR (ex: "31/12/2026")
-         * @@returns {string} Data em formato ISO (ex: "2026-12-31")
-         */
         function convertDateToISO(dateStr) {
             try {
                 if (!dateStr) return '';
@@ -1176,13 +1136,6 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * INICIALIZAÇÃO DO DOCUMENTO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Configura estado inicial dos campos, carrega saldos via AJAX,
-         * inicializa modais Bootstrap 5 e DataTables para movimentações
-         */
         $(document).ready(function () {
             try {
                 $("#divContrato, #divAta").hide();
@@ -1264,12 +1217,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * MODAL APORTE - EDIÇÃO DE APORTE EXISTENTE
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Modal Bootstrap 5 para editar aportes já registrados
-                 */
                 const modalAporteEl = document.getElementById('modalAporte');
                 const modalAporte = new bootstrap.Modal(modalAporteEl);
 
@@ -1309,12 +1256,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * MODAL ANULAÇÃO - EDIÇÃO DE ANULAÇÃO EXISTENTE
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Modal Bootstrap 5 para editar anulações já registradas
-                 */
                 const modalAnulacaoEl = document.getElementById('modalanulacao');
                 const modalAnulacao = new bootstrap.Modal(modalAnulacaoEl);
 
@@ -1354,12 +1295,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * MODAL NOTAS FISCAIS - VISUALIZAÇÃO
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Modal com DataTable para listar notas fiscais vinculadas
-                 */
                 const modalNFEl = document.getElementById('modalNF');
                 let dataTableNF = null;
 
@@ -1408,17 +1343,17 @@
                                     data: "notaFiscalId",
                                     render: function (data) {
                                         return `
-                                                <div class="text-center">
-                                                    <a href="/NotaFiscal/Upsert?id=${data}"
-                                                       class="btn btn-sm btn-azul text-white">
-                                                        <i class="fa-duotone fa-pen-to-square"></i>
-                                                    </a>
-                                                    <a class="btn btn-sm btn-delete btn-vinho text-white"
-                                                       data-id='${data}'>
-                                                        <i class="fa-duotone fa-trash-can"></i>
-                                                    </a>
-                                                </div>
-                                            `;
+                                            <div class="text-center">
+                                                <a href="/NotaFiscal/Upsert?id=${data}"
+                                                   class="btn btn-sm btn-azul text-white">
+                                                    <i class="fa-duotone fa-pen-to-square"></i>
+                                                </a>
+                                                <a class="btn btn-sm btn-delete btn-vinho text-white"
+                                                   data-id='${data}'>
+                                                    <i class="fa-duotone fa-trash-can"></i>
+                                                </a>
+                                            </div>
+                                        `;
                                     }
                                 },
                                 { data: "contratoId" },
@@ -1435,12 +1370,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * MODAL ADICIONAR APORTE - NOVO REGISTRO
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Modal Bootstrap 5 para adicionar novo aporte ao empenho
-                 */
                 const modalAdicionarAporteEl = document.getElementById('modalAdicionarAporte');
                 let modalAdicionarAporteInstance = null;
 
@@ -1471,12 +1400,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * MODAL ADICIONAR ANULAÇÃO - NOVO REGISTRO
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Modal Bootstrap 5 para adicionar nova anulação ao empenho
-                 */
                 const modalAdicionarAnulacaoEl = document.getElementById('modalAdicionarAnulacao');
                 let modalAdicionarAnulacaoInstance = null;
 
@@ -1507,13 +1430,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * DATATABLE DE APORTES
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Tabela de aportes do empenho com soma no rodapé.
-                 * Expõe window.AporteTable para reload externo.
-                 */
                 if ($('#tblAporte').length) {
                     try {
                         window.AporteTable = $('#tblAporte').DataTable({
@@ -1545,18 +1461,18 @@
                                     data: "movimentacaoId",
                                     render: function (data) {
                                         return `
-                                                <div class="text-center ftx-actions">
-                                                    <a class="btn btn-icon-sm btn-azul btn-edit-aporte text-white"
-                                                       data-id='${data}'>
-                                                        <i class="fa-duotone fa-pen-to-square"></i>
-                                                    </a>
-                                                    <a class="btn btn-icon-sm btn-vinho btn-deleteaporte text-white"
-                                                       data-context="empenho"
-                                                       data-id='${data}'>
-                                                        <i class="fa-duotone fa-trash-can"></i>
-                                                    </a>
-                                                </div>
-                                            `;
+                                            <div class="text-center ftx-actions">
+                                                <a class="btn btn-icon-sm btn-azul btn-edit-aporte text-white"
+                                                   data-id='${data}'>
+                                                    <i class="fa-duotone fa-pen-to-square"></i>
+                                                </a>
+                                                <a class="btn btn-icon-sm btn-vinho btn-deleteaporte text-white"
+                                                   data-context="empenho"
+                                                   data-id='${data}'>
+                                                    <i class="fa-duotone fa-trash-can"></i>
+                                                </a>
+                                            </div>
+                                        `;
                                     }
                                 },
                                 { data: "movimentacaoId" },
@@ -1589,13 +1505,6 @@
                     }
                 }
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * DATATABLE DE ANULAÇÕES
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Tabela de anulações/estornos do empenho com soma no rodapé.
-                 * Expõe window.anulacaoTable para reload externo.
-                 */
                 if ($('#tblanulacao').length) {
                     try {
                         window.anulacaoTable = $('#tblanulacao').DataTable({
@@ -1632,18 +1541,18 @@
                                     data: "movimentacaoId",
                                     render: function (data) {
                                         return `
-                                                <div class="text-center ftx-actions">
-                                                    <a class="btn btn-icon-sm btn-azul btn-edit-anulacao text-white"
-                                                       data-id='${data}'>
-                                                        <i class="fa-duotone fa-pen-to-square"></i>
-                                                    </a>
-                                                    <a class="btn btn-icon-sm btn-vinho btn-deleteanulacao text-white"
-                                                       data-context="empenho"
-                                                       data-id='${data}'>
-                                                        <i class="fa-duotone fa-trash-can"></i>
-                                                    </a>
-                                                </div>
-                                            `;
+                                            <div class="text-center ftx-actions">
+                                                <a class="btn btn-icon-sm btn-azul btn-edit-anulacao text-white"
+                                                   data-id='${data}'>
+                                                    <i class="fa-duotone fa-pen-to-square"></i>
+                                                </a>
+                                                <a class="btn btn-icon-sm btn-vinho btn-deleteanulacao text-white"
+                                                   data-context="empenho"
+                                                   data-id='${data}'>
+                                                    <i class="fa-duotone fa-trash-can"></i>
+                                                </a>
+                                            </div>
+                                        `;
                                     }
                                 },
                                 { data: "movimentacaoId" },
@@ -1681,17 +1590,6 @@
             }
         });
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * BOTÕES DE AÇÃO - EDITAR APORTE/ANULAÇÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Handlers para edição de movimentações existentes via modal
-         */
-
-        /**
-         * Handler do botão Salvar Edição de Aporte
-         * @@description Valida campos e envia PUT para /api/Empenho/EditarAporte
-         */
         $("#btnAportar").click(function (e) {
             try {
                 e.preventDefault();
@@ -1755,10 +1653,6 @@
             }
         });
 
-        /**
-         * Handler do botão Salvar Edição de Anulação
-         * @@description Valida campos e envia PUT para /api/Empenho/EditarAnulacao
-         */
         $("#btnAnular").click(function (e) {
             try {
                 e.preventDefault();
@@ -1822,18 +1716,6 @@
             }
         });
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * BOTÕES DE AÇÃO - ADICIONAR NOVO APORTE/ANULAÇÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Handlers para criação de novas movimentações
-         */
-
-        /**
-         * Handler do botão Confirmar Novo Aporte
-         * @@description Valida campos, envia POST para /api/Empenho/Aporte
-         * e recarrega página após sucesso
-         */
         $("#btnConfirmarAporte").on("click", function (e) {
             try {
                 e.preventDefault();
@@ -1903,11 +1785,6 @@
             }
         });
 
-        /**
-         * Handler do botão Confirmar Nova Anulação
-         * @@description Valida campos, envia POST para /api/Empenho/anulacao
-         * e recarrega página após sucesso
-         */
         $("#btnConfirmarAnulacao").on("click", function (e) {
             try {
                 e.preventDefault();
@@ -1977,17 +1854,6 @@
             }
         });
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * SUBMISSÃO DO FORMULÁRIO PRINCIPAL
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Validação e envio dos dados do empenho para criação/edição
-         */
-
-        /**
-         * Handler do botão Submit principal
-         * @@description Valida todos os campos obrigatórios antes de submeter
-         */
         $("#btnSubmit").click(function (event) {
             try {
                 event.preventDefault();
@@ -2043,10 +1909,6 @@
             }
         });
 
-        /**
-         * Handler do botão oculto para submissão via AJAX
-         * @@description Dispara a função InsereRegistro após validação
-         */
         $("#btnEscondido").click(function (event) {
             try {
                 event.preventDefault();
@@ -2056,11 +1918,6 @@
             }
         });
 
-        /**
-         * Envia dados do empenho para API
-         * @@description Monta payload e faz POST para InsereEmpenho (novo)
-         * ou EditaEmpenho (existente). Redireciona para listagem após sucesso.
-         */
         function InsereRegistro() {
             try {
                 const empenhoId = '@Model.EmpenhoObj.Empenho.EmpenhoId';
```
