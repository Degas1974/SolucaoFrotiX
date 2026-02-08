# Pages/Escalas/ListaEscala.cshtml

**Mudanca:** GRANDE | **+89** linhas | **-79** linhas

---

```diff
--- JANEIRO: Pages/Escalas/ListaEscala.cshtml
+++ ATUAL: Pages/Escalas/ListaEscala.cshtml
@@ -43,22 +43,15 @@
         background-color: yellow;
         color: black !important;
     }
-
-    .status-reservado {
-        background-color: #6c757d;
-        color: white;
-    }
-
+    .status-reservado { background-color: #6c757d; color: white; }
     .motoristas-vez-card {
         border-radius: 10px;
         box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
         transition: transform 0.3s;
     }
-
     .motoristas-vez-card:hover {
         transform: translateY(-5px);
     }
-
     .contador-viagens {
         font-size: 2rem;
         font-weight: bold;
@@ -106,8 +99,7 @@
 
 <div class="card mb-3">
     <div class="card-header bg-primary text-white">
-        <h5 class="mb-0"><i class="fas fa-users"></i> Motoristas da Vez (Total: @(Model.EscalasObj?.Count(e =>
-                        e.StatusMotorista == "Disponível") ?? 0))</h5>
+        <h5 class="mb-0"><i class="fas fa-users"></i> Motoristas da Vez (Total: @(Model.EscalasObj?.Count(e => e.StatusMotorista == "Disponível") ?? 0))</h5>
     </div>
     <div class="card-body">
         <div id="motoristasVezContainer" class="row">
@@ -115,10 +107,10 @@
             @if (Model.EscalasObj != null && Model.EscalasObj.Any())
             {
                 var motoristasDisponiveis = Model.EscalasObj
-                .Where(e => e.StatusMotorista == "Disponível" && e.MotoristaId.HasValue)
-                .GroupBy(e => e.MotoristaId)
-                .Select(g => g.First())
-                .Take(6);
+                    .Where(e => e.StatusMotorista == "Disponível" && e.MotoristaId.HasValue)
+                    .GroupBy(e => e.MotoristaId)
+                    .Select(g => g.First())
+                    .Take(6);
 
                 foreach (var motorista in motoristasDisponiveis)
                 {
@@ -127,13 +119,13 @@
                             <div class="card-body p-2">
                                 @if (motorista.Foto != null && motorista.Foto.Length > 0)
                                 {
-                                    <img src="data:image/png;base64,@Convert.ToBase64String(motorista.Foto)" class="rounded-circle"
-                                        width="50" height="50" alt="@motorista.NomeMotorista">
+                                    <img src="data:image/png;base64,@Convert.ToBase64String(motorista.Foto)"
+                                         class="rounded-circle" width="50" height="50" alt="@motorista.NomeMotorista">
                                 }
                                 else
                                 {
-                                    <img src="/images/default-driver.png" class="rounded-circle" width="50" height="50"
-                                        alt="@motorista.NomeMotorista">
+                                    <img src="/images/default-driver.png"
+                                         class="rounded-circle" width="50" height="50" alt="@motorista.NomeMotorista">
                                 }
                                 <h6 class="mt-2 mb-1">@motorista.NomeMotorista</h6>
                                 <p class="mb-1 small">@(motorista.Placa ?? "Sem veículo")</p>
@@ -167,15 +159,20 @@
                 <div class="col-md-3">
                     <div class="form-group">
                         <label asp-for="Filtro.DataFiltro"></label>
-                        <ejs-datepicker id="DataEscala" asp-for="Filtro.DataFiltro" format="dd/MM/yyyy"
-                            placeholder="Selecione a data"></ejs-datepicker>
+                        <ejs-datepicker id="DataEscala"
+                                        asp-for="Filtro.DataFiltro"
+                                        format="dd/MM/yyyy"
+                                        placeholder="Selecione a data"></ejs-datepicker>
                     </div>
                 </div>
                 <div class="col-md-3">
                     <div class="form-group">
                         <label asp-for="Filtro.TipoServicoId"></label>
-                        <ejs-dropdownlist id="tipoServicoId" asp-for="Filtro.TipoServicoId"
-                            dataSource="@Model.Filtro.TipoServicoList" placeholder="Todos" allowFiltering="true">
+                        <ejs-dropdownlist id="tipoServicoId"
+                                          asp-for="Filtro.TipoServicoId"
+                                          dataSource="@Model.Filtro.TipoServicoList"
+                                          placeholder="Todos"
+                                          allowFiltering="true">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                     </div>
@@ -183,8 +180,11 @@
                 <div class="col-md-3">
                     <div class="form-group">
                         <label asp-for="Filtro.TurnoId"></label>
-                        <ejs-dropdownlist id="turnoId" asp-for="Filtro.TurnoId" dataSource="@Model.Filtro.TurnoList"
-                            placeholder="Todos" allowFiltering="true">
+                        <ejs-dropdownlist id="turnoId"
+                                          asp-for="Filtro.TurnoId"
+                                          dataSource="@Model.Filtro.TurnoList"
+                                          placeholder="Todos"
+                                          allowFiltering="true">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                     </div>
@@ -192,8 +192,11 @@
                 <div class="col-md-3">
                     <div class="form-group">
                         <label asp-for="Filtro.StatusMotorista"></label>
-                        <ejs-dropdownlist id="statusMotorista" asp-for="Filtro.StatusMotorista"
-                            dataSource="@Model.Filtro.StatusList" placeholder="Todos" allowFiltering="true">
+                        <ejs-dropdownlist id="statusMotorista"
+                                          asp-for="Filtro.StatusMotorista"
+                                          dataSource="@Model.Filtro.StatusList"
+                                          placeholder="Todos"
+                                          allowFiltering="true">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                     </div>
@@ -203,8 +206,11 @@
                 <div class="col-md-3">
                     <div class="form-group">
                         <label asp-for="Filtro.MotoristaId"></label>
-                        <ejs-dropdownlist id="motoristaId" asp-for="Filtro.MotoristaId"
-                            dataSource="@Model.Filtro.MotoristaList" placeholder="Todos" allowFiltering="true">
+                        <ejs-dropdownlist id="motoristaId"
+                                          asp-for="Filtro.MotoristaId"
+                                          dataSource="@Model.Filtro.MotoristaList"
+                                          placeholder="Todos"
+                                          allowFiltering="true">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                     </div>
@@ -212,8 +218,11 @@
                 <div class="col-md-3">
                     <div class="form-group">
                         <label asp-for="Filtro.VeiculoId"></label>
-                        <ejs-dropdownlist id="veiculoId" asp-for="Filtro.VeiculoId"
-                            dataSource="@Model.Filtro.VeiculoList" placeholder="Todos" allowFiltering="true">
+                        <ejs-dropdownlist id="veiculoId"
+                                          asp-for="Filtro.VeiculoId"
+                                          dataSource="@Model.Filtro.VeiculoList"
+                                          placeholder="Todos"
+                                          allowFiltering="true">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                     </div>
@@ -221,8 +230,11 @@
                 <div class="col-md-3">
                     <div class="form-group">
                         <label asp-for="Filtro.Lotacao"></label>
-                        <ejs-dropdownlist id="lotacao" asp-for="Filtro.Lotacao" dataSource="@Model.Filtro.LotacaoList"
-                            placeholder="Todas" allowFiltering="true">
+                        <ejs-dropdownlist id="lotacao"
+                                          asp-for="Filtro.Lotacao"
+                                          dataSource="@Model.Filtro.LotacaoList"
+                                          placeholder="Todas"
+                                          allowFiltering="true">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                     </div>
@@ -230,8 +242,7 @@
                 <div class="col-md-3">
                     <div class="form-group">
                         <label asp-for="Filtro.TextoPesquisa"></label>
-                        <ejs-textbox id="textoPesquisa" asp-for="Filtro.TextoPesquisa"
-                            placeholder="Pesquisar..."></ejs-textbox>
+                        <ejs-textbox id="textoPesquisa" asp-for="Filtro.TextoPesquisa" placeholder="Pesquisar..."></ejs-textbox>
                     </div>
                 </div>
             </div>
@@ -255,14 +266,17 @@
     </div>
     <div class="card-body">
 
-        <ejs-grid id="gridEscalas" dataSource="@Model.EscalasObj" allowPaging="true" allowSorting="true"
-            allowExcelExport="true" allowPdfExport="true"
-            toolbar="@(new List<string>() { "ExcelExport", "PdfExport", "Search" })">
+        <ejs-grid id="gridEscalas"
+                  dataSource="@Model.EscalasObj"
+                  allowPaging="true"
+                  allowSorting="true"
+                  allowExcelExport="true"
+                  allowPdfExport="true"
+                  toolbar="@(new List<string>() { "ExcelExport", "PdfExport", "Search" })">
             <e-grid-pagesettings pageSize="20"></e-grid-pagesettings>
             <e-grid-columns>
 
-                <e-grid-column field="DataEscala" headerText="Data" width="100" format="dd/MM/yyyy"
-                    type="date"></e-grid-column>
+                <e-grid-column field="DataEscala" headerText="Data" width="100" format="dd/MM/yyyy" type="date"></e-grid-column>
                 <e-grid-column field="NomeMotorista" headerText="Motorista" width="150"></e-grid-column>
                 <e-grid-column field="Placa" headerText="Veículo" width="100"></e-grid-column>
                 <e-grid-column field="NomeTurno" headerText="Turno" width="100"></e-grid-column>
@@ -272,10 +286,8 @@
                 <e-grid-column field="Lotacao" headerText="Lotação" width="100"></e-grid-column>
                 <e-grid-column field="NumeroSaidas" headerText="Saídas" width="80" textAlign="Center"></e-grid-column>
 
-                <e-grid-column field="StatusMotorista" headerText="Status" width="120"
-                    template="#templateStatusMotorista"></e-grid-column>
-                <e-grid-column field="EscalaDiaId" headerText="Ações" width="150" template="#actionTemplate"
-                    textAlign="Center"></e-grid-column>
+                <e-grid-column field="StatusMotorista" headerText="Status" width="120" template="#templateStatusMotorista"></e-grid-column>
+                <e-grid-column field="EscalaDiaId" headerText="Ações" width="150" template="#actionTemplate" textAlign="Center"></e-grid-column>
             </e-grid-columns>
         </ejs-grid>
     </div>
@@ -302,8 +314,8 @@
                     <div class="row mb-4">
                         <div class="col-md-3 text-center">
                             <img id="viewFotoMotorista" src="/images/default-driver.png"
-                                class="rounded-circle border border-3 border-info" width="120" height="120"
-                                alt="Foto Motorista">
+                                 class="rounded-circle border border-3 border-info"
+                                 width="120" height="120" alt="Foto Motorista">
                             <h5 id="viewNomeMotorista" class="mt-2 mb-0">-</h5>
                             <span id="viewStatusBadge" class="badge mt-2">-</span>
                         </div>
@@ -336,8 +348,7 @@
                                             </p>
                                             <p class="mb-0">
                                                 <i class="fas fa-route text-info me-2"></i>
-                                                <strong>Saídas:</strong> <span id="viewNumeroSaidas"
-                                                    class="badge bg-primary">0</span>
+                                                <strong>Saídas:</strong> <span id="viewNumeroSaidas" class="badge bg-primary">0</span>
                                             </p>
                                         </div>
                                     </div>
@@ -354,8 +365,7 @@
                             <div class="col-md-11">
                                 <h6 class="mb-1"><i class="fas fa-exchange-alt me-2"></i>Motorista em Cobertura</h6>
                                 <p class="mb-1">
-                                    <strong>Cobertor:</strong> <span id="viewNomeCobertor"
-                                        class="text-primary fw-bold">-</span>
+                                    <strong>Cobertor:</strong> <span id="viewNomeCobertor" class="text-primary fw-bold">-</span>
                                 </p>
                                 <p class="mb-0">
                                     <strong>Motivo:</strong> <span id="viewMotivoCobertura">-</span>
@@ -372,8 +382,7 @@
                             <div class="col-md-11">
                                 <h6 class="mb-1"><i class="fas fa-user-check me-2"></i>Este motorista está cobrindo</h6>
                                 <p class="mb-1">
-                                    <strong>Motorista Coberto:</strong> <span id="viewNomeCoberto"
-                                        class="text-primary fw-bold">-</span>
+                                    <strong>Motorista Coberto:</strong> <span id="viewNomeCoberto" class="text-primary fw-bold">-</span>
                                 </p>
                                 <p class="mb-0">
                                     <strong>Motivo:</strong> <span id="viewMotivoCoberto">-</span>
@@ -431,7 +440,7 @@
     </div>
     <div class="card-body">
         <div id="observacoesContainer">
-            <p class="text-muted">Nenhuma observação para hoje</p>
+           <p class="text-muted">Nenhuma observação para hoje</p>
         </div>
         <button type="button" class="btn btn-warning btn-sm" data-bs-toggle="modal" data-bs-target="#modalObservacao">
             <i class="fas fa-plus"></i> Adicionar Observação
@@ -453,14 +462,11 @@
                 </div>
                 <div class="form-group mb-3">
                     <label>Descrição</label>
-                    <ejs-textbox id="obsDescription" multiline="true"
-                        placeholder="Descrição da observação"></ejs-textbox>
+                    <ejs-textbox id="obsDescription" multiline="true" placeholder="Descrição da observação"></ejs-textbox>
                 </div>
                 <div class="form-group mb-3">
                     <label>Prioridade</label>
-                    <ejs-dropdownlist id="obsPriority"
-                        dataSource='@(new List<object> { new { text = "Baixa", value = "Baixa" }, new { text = "Normal", value = "Normal" }, new { text = "Alta", value = "Alta" } })'
-                        value='@("Normal")'>
+                    <ejs-dropdownlist id="obsPriority" dataSource='@(new List<object> { new { text = "Baixa", value = "Baixa" }, new { text = "Normal", value = "Normal" }, new { text = "Alta", value = "Alta" } })' value='@("Normal")'>
                         <e-dropdownlist-fields text="text" value="value"></e-dropdownlist-fields>
                     </ejs-dropdownlist>
                 </div>
@@ -492,33 +498,33 @@
     <script src="~/js/cadastros/ListaEscala.js" asp-append-version="true"></script>
 
     <script type="text/x-template" id="templateStatusMotorista">
-            ${if(StatusMotorista)}
-                <span class="badge status-${getStatusClass(StatusMotorista)}">${StatusMotorista}</span>
-            ${else}
-                <span class="badge bg-secondary">Indefinido</span>
-            ${/if}
-        </script>
+        ${if(StatusMotorista)}
+            <span class="badge status-${getStatusClass(StatusMotorista)}">${StatusMotorista}</span>
+        ${else}
+            <span class="badge bg-secondary">Indefinido</span>
+        ${/if}
+    </script>
 
     <script id="actionTemplate" type="text/x-template">
-            ${if(EscalaDiaId)}
-                <div class="btn-group btn-group-sm">
-                    <button class="btn btn-info btn-sm text-white btn-visualizar"
-                            data-id="${EscalaDiaId}"
-                            title="Visualizar">
-                        <i class="fas fa-eye"></i>
-                    </button>
-                    <a href="/Escalas/UpsertEEscala?id=${EscalaDiaId}" class="btn btn-primary btn-sm" title="Editar">
-                        <i class="fas fa-edit"></i>
-                    </a>
-                    <button class="btn btn-danger btn-sm btn-excluir"
-                            data-id="${EscalaDiaId}"
-                            title="Excluir">
-                        <i class="fas fa-trash"></i>
-                    </button>
-                </div>
-            ${else}
-                <span class="text-muted">Sem ações</span>
-            ${/if}
-        </script>
+        ${if(EscalaDiaId)}
+            <div class="btn-group btn-group-sm">
+                <button class="btn btn-info btn-sm text-white btn-visualizar"
+                        data-id="${EscalaDiaId}"
+                        title="Visualizar">
+                    <i class="fas fa-eye"></i>
+                </button>
+                <a href="/Escalas/UpsertEEscala?id=${EscalaDiaId}" class="btn btn-primary btn-sm" title="Editar">
+                    <i class="fas fa-edit"></i>
+                </a>
+                <button class="btn btn-danger btn-sm btn-excluir"
+                        data-id="${EscalaDiaId}"
+                        title="Excluir">
+                    <i class="fas fa-trash"></i>
+                </button>
+            </div>
+        ${else}
+            <span class="text-muted">Sem ações</span>
+        ${/if}
+    </script>
 
 }
```
