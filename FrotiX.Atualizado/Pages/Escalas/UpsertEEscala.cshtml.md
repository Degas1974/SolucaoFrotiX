# Pages/Escalas/UpsertEEscala.cshtml

**Mudanca:** GRANDE | **+120** linhas | **-93** linhas

---

```diff
--- JANEIRO: Pages/Escalas/UpsertEEscala.cshtml
+++ ATUAL: Pages/Escalas/UpsertEEscala.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Escalas.UpsertEEscalaModel
 @{
     ViewData["Title"] = "Editar Escala";
@@ -14,39 +13,37 @@
         transition: all 0.3s;
     }
 
-    .status-checkbox.active-disponivel {
-        background-color: #d4edda;
-        border: 2px solid #28a745;
-    }
-
-    .status-checkbox.active-indisponivel {
-        background-color: #f8d7da;
-        border: 2px solid #dc3545;
-    }
-
-    .status-checkbox.active-economildo {
-        background-color: #d1ecf1;
-        border: 2px solid #17a2b8;
-    }
-
-    .status-checkbox.active-servico {
-        background-color: #fff3cd;
-        border: 2px solid #ffc107;
-    }
-
-    .status-checkbox.active-reservado {
-        background-color: #e2e3e5;
-        border: 2px solid #6c757d;
-    }
-
-    .status-checkbox.active-viagem {
-        background-color: #cfe2ff;
-        border: 2px solid #0d6efd;
-    }
-
-    .indisponibilidade-section,
-    .servico-section,
-    .economildo-section {
+        .status-checkbox.active-disponivel {
+            background-color: #d4edda;
+            border: 2px solid #28a745;
+        }
+
+        .status-checkbox.active-indisponivel {
+            background-color: #f8d7da;
+            border: 2px solid #dc3545;
+        }
+
+        .status-checkbox.active-economildo {
+            background-color: #d1ecf1;
+            border: 2px solid #17a2b8;
+        }
+
+        .status-checkbox.active-servico {
+            background-color: #fff3cd;
+            border: 2px solid #ffc107;
+        }
+
+        .status-checkbox.active-reservado {
+            background-color: #e2e3e5;
+            border: 2px solid #6c757d;
+        }
+
+        .status-checkbox.active-viagem {
+            background-color: #cfe2ff;
+            border: 2px solid #0d6efd;
+        }
+
+    .indisponibilidade-section, .servico-section, .economildo-section {
         display: none;
         margin-top: 20px;
         padding: 15px;
@@ -83,9 +80,9 @@
                 <h6>Motorista</h6>
                 <h4>@Model.NomeMotorista</h4>
                 <span class="badge bg-@(Model.StatusMotorista == "Disponível" ? "success" :
-                                                                               Model.StatusMotorista == "Em Viagem" ? "primary" :
-                                                                               Model.StatusMotorista == "Indisponível" ? "danger" :
-                                                                               Model.StatusMotorista == "Em Serviço" ? "warning" : "info")">
+                                                         Model.StatusMotorista == "Em Viagem" ? "primary" :
+                                                         Model.StatusMotorista == "Indisponível" ? "danger" :
+                                                         Model.StatusMotorista == "Em Serviço" ? "warning" : "info")">
                     @Model.StatusMotorista
                 </span>
             </div>
@@ -120,9 +117,13 @@
                         <label asp-for="MotoristaId" class="control-label required">
                             <i class="fas fa-user"></i> Motorista
                         </label>
-                        <ejs-dropdownlist id="motoristaId" name="MotoristaId" dataSource="@Model.MotoristaList"
-                            value="@Model.MotoristaId" placeholder="Selecione o motorista" allowFiltering="true"
-                            floatLabelType="Auto" enabled="false">
+                        <ejs-dropdownlist id="motoristaId" name="MotoristaId"
+                                          dataSource="@Model.MotoristaList"
+                                          value="@Model.MotoristaId"
+                                          placeholder="Selecione o motorista"
+                                          allowFiltering="true"
+                                          floatLabelType="Auto"
+                                          enabled="false">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                         <small class="text-muted">O motorista não pode ser alterado após a criação da escala</small>
@@ -134,15 +135,17 @@
                         <label asp-for="VeiculoId" class="control-label">
                             <i class="fas fa-car"></i> Veículo
                         </label>
-                        <ejs-dropdownlist id="veiculoId" name="VeiculoId" dataSource="@Model.VeiculoList"
-                            value="@Model.VeiculoId" placeholder="Selecione o veículo" allowFiltering="true"
-                            floatLabelType="Auto">
+                        <ejs-dropdownlist id="veiculoId" name="VeiculoId"
+                                          dataSource="@Model.VeiculoList"
+                                          value="@Model.VeiculoId"
+                                          placeholder="Selecione o veículo"
+                                          allowFiltering="true"
+                                          floatLabelType="Auto">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                         <div class="form-check mt-2">
-                            <input type="checkbox" class="form-check-input" id="veiculoNaoDefinido"
-                                name="VeiculoNaoDefinido" @(Model.VeiculoId == null || Model.VeiculoId == Guid.Empty ?
-                                                                                          "checked" : "") />
+                            <input type="checkbox" class="form-check-input" id="veiculoNaoDefinido" name="VeiculoNaoDefinido"
+                                   @(Model.VeiculoId == null || Model.VeiculoId == Guid.Empty ? "checked" : "") />
                             <label class="form-check-label" for="veiculoNaoDefinido">
                                 <i class="fas fa-ban text-muted"></i> Veículo não definido
                             </label>
@@ -158,8 +161,11 @@
                         <label asp-for="DataEscala" class="control-label required">
                             <i class="fas fa-calendar-day"></i> Data
                         </label>
-                        <ejs-datepicker id="dataEscala" name="DataEscala" value="@Model.DataEscala" format="dd/MM/yyyy"
-                            placeholder="Selecione a data" floatLabelType="Auto"></ejs-datepicker>
+                        <ejs-datepicker id="dataEscala" name="DataEscala"
+                                        value="@Model.DataEscala"
+                                        format="dd/MM/yyyy"
+                                        placeholder="Selecione a data"
+                                        floatLabelType="Auto"></ejs-datepicker>
                         <span asp-validation-for="DataEscala" class="text-danger"></span>
                     </div>
                 </div>
@@ -168,8 +174,11 @@
                         <label asp-for="TurnoId" class="control-label required">
                             <i class="fas fa-clock"></i> Turno
                         </label>
-                        <ejs-dropdownlist id="turnoId" name="TurnoId" dataSource="@Model.TurnoList"
-                            value="@Model.TurnoId" placeholder="Selecione o turno" floatLabelType="Auto">
+                        <ejs-dropdownlist id="turnoId" name="TurnoId"
+                                          dataSource="@Model.TurnoList"
+                                          value="@Model.TurnoId"
+                                          placeholder="Selecione o turno"
+                                          floatLabelType="Auto">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                         <span asp-validation-for="TurnoId" class="text-danger"></span>
@@ -183,8 +192,12 @@
                         <label asp-for="HoraInicio" class="control-label required">
                             <i class="fas fa-hourglass-start"></i> Hora Início
                         </label>
-                        <ejs-timepicker id="horaInicio" name="HoraInicio" value="@Model.HoraInicio"
-                            placeholder="Hora início" format="HH:mm" step="30" floatLabelType="Auto"></ejs-timepicker>
+                        <ejs-timepicker id="horaInicio" name="HoraInicio"
+                                       value="@Model.HoraInicio"
+                                       placeholder="Hora início"
+                                       format="HH:mm"
+                                       step="30"
+                                       floatLabelType="Auto"></ejs-timepicker>
                         <span asp-validation-for="HoraInicio" class="text-danger"></span>
                     </div>
                 </div>
@@ -193,8 +206,12 @@
                         <label asp-for="HoraFim" class="control-label required">
                             <i class="fas fa-hourglass-end"></i> Hora Fim
                         </label>
-                        <ejs-timepicker id="horaFim" name="HoraFim" value="@Model.HoraFim" placeholder="Hora fim"
-                            format="HH:mm" step="30" floatLabelType="Auto"></ejs-timepicker>
+                        <ejs-timepicker id="horaFim" name="HoraFim"
+                                       value="@Model.HoraFim"
+                                       placeholder="Hora fim"
+                                       format="HH:mm"
+                                       step="30"
+                                       floatLabelType="Auto"></ejs-timepicker>
                         <span asp-validation-for="HoraFim" class="text-danger"></span>
                     </div>
                 </div>
@@ -248,9 +265,11 @@
                         <label asp-for="TipoServicoId" class="control-label required">
                             <i class="fas fa-briefcase"></i> Tipo de Serviço
                         </label>
-                        <ejs-dropdownlist id="tipoServicoId" name="TipoServicoId" dataSource="@Model.TipoServicoList"
-                            value="@Model.TipoServicoId" placeholder="Selecione o tipo de serviço"
-                            floatLabelType="Auto">
+                        <ejs-dropdownlist id="tipoServicoId" name="TipoServicoId"
+                                          dataSource="@Model.TipoServicoList"
+                                          value="@Model.TipoServicoId"
+                                          placeholder="Selecione o tipo de serviço"
+                                          floatLabelType="Auto">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                         <span asp-validation-for="TipoServicoId" class="text-danger"></span>
@@ -267,10 +286,11 @@
         <div class="card-body">
             <div class="row">
                 <div class="col-md-2">
-                    <div class="status-checkbox @(Model.StatusMotorista == "Em Viagem" ? "active-viagem" : "")"
-                        id="statusViagem">
+                    <div class="status-checkbox @(Model.StatusMotorista == "Em Viagem" ? "active-viagem" : "")" id="statusViagem">
                         <div class="form-check">
-                            <input type="radio" name="StatusMotorista" value="Em Viagem" @(Model.StatusMotorista == "Em Viagem" ? "checked" : "") class="form-check-input" disabled />
+                            <input type="radio" name="StatusMotorista" value="Em Viagem"
+                                   @(Model.StatusMotorista == "Em Viagem" ? "checked" : "")
+                                   class="form-check-input" disabled />
                             <label class="form-check-label">
                                 <i class="fas fa-car text-primary"></i> Em Viagem
                             </label>
@@ -279,8 +299,7 @@
                     </div>
                 </div>
                 <div class="col-md-2">
-                    <div class="status-checkbox @(Model.MotoristaIndisponivel ? "active-indisponivel" : "")"
-                        id="statusIndisponivel">
+                    <div class="status-checkbox @(Model.MotoristaIndisponivel ? "active-indisponivel" : "")" id="statusIndisponivel">
                         <div class="form-check">
                             <input type="checkbox" asp-for="MotoristaIndisponivel" class="form-check-input" />
                             <label asp-for="MotoristaIndisponivel" class="form-check-label">
@@ -290,8 +309,7 @@
                     </div>
                 </div>
                 <div class="col-md-2">
-                    <div class="status-checkbox @(Model.MotoristaEconomildo ? "active-economildo" : "")"
-                        id="statusEconomildo">
+                    <div class="status-checkbox @(Model.MotoristaEconomildo ? "active-economildo" : "")" id="statusEconomildo">
                         <div class="form-check">
                             <input type="checkbox" asp-for="MotoristaEconomildo" class="form-check-input" />
                             <label asp-for="MotoristaEconomildo" class="form-check-label">
@@ -311,8 +329,7 @@
                     </div>
                 </div>
                 <div class="col-md-2">
-                    <div class="status-checkbox @(Model.MotoristaReservado ? "active-reservado" : "")"
-                        id="statusReservado">
+                    <div class="status-checkbox @(Model.MotoristaReservado ? "active-reservado" : "")" id="statusReservado">
                         <div class="form-check">
                             <input type="checkbox" asp-for="MotoristaReservado" class="form-check-input" />
                             <label asp-for="MotoristaReservado" class="form-check-label">
@@ -326,13 +343,11 @@
             @if (Model.StatusMotorista == "Em Viagem")
             {
                 <div class="alert alert-info mt-3">
-                    <i class="fas fa-info-circle"></i> O status "Em Viagem" é atualizado automaticamente quando o motorista
-                    inicia ou finaliza uma viagem.
+                    <i class="fas fa-info-circle"></i> O status "Em Viagem" é atualizado automaticamente quando o motorista inicia ou finaliza uma viagem.
                 </div>
             }
 
-            <div class="indisponibilidade-section" id="indisponibilidadeSection"
-                style="@(Model.MotoristaIndisponivel ? "display: block;" : "")">
+            <div class="indisponibilidade-section" id="indisponibilidadeSection" style="@(Model.MotoristaIndisponivel ? "display: block;" : "")">
                 <h6><i class="fas fa-calendar-times"></i> Configurar Indisponibilidade</h6>
                 <div class="row">
                     <div class="col-md-3">
@@ -341,14 +356,15 @@
                                 <i class="fas fa-tag"></i> Motivo
                             </label>
                             <ejs-dropdownlist id="categoriaIndisponibilidade" name="CategoriaIndisponibilidade"
-                                value="@Model.CategoriaIndisponibilidade" dataSource='@(new List<object> {
-                                                                                                                                                                     new { text = "Folga", value = "Folga" },
-                                                                                                                                                                     new { text = "Férias", value = "Férias" },
-                                                                                                                                                                     new { text = "Recesso", value = "Recesso" },
-                                                                                                                                                                     new { text = "Falta", value = "Falta"},
-                                                                                                                                                                     new { text = "Atestado", value = "Atestado"}
-                                                                                                                                                                 })'
-                                placeholder="Selecione a categoria">
+                                              value="@Model.CategoriaIndisponibilidade"
+                                              dataSource='@(new List<object> {
+                                                                                           new { text = "Folga", value = "Folga" },
+                                                                                           new { text = "Férias", value = "Férias" },
+                                                                                           new { text = "Recesso", value = "Recesso" },
+                                                                                           new { text = "Falta", value = "Falta"},
+                                                                                           new { text = "Atestado", value = "Atestado"}
+                                                                                       })'
+                                              placeholder="Selecione a categoria">
                                 <e-dropdownlist-fields text="text" value="value"></e-dropdownlist-fields>
                             </ejs-dropdownlist>
                         </div>
@@ -359,8 +375,9 @@
                                 <i class="fas fa-calendar-day"></i> Data Inicial
                             </label>
                             <ejs-datepicker id="dataInicioIndisponibilidade" name="DataInicioIndisponibilidade"
-                                value="@Model.DataInicioIndisponibilidade" format="dd/MM/yyyy"
-                                placeholder="Data início"></ejs-datepicker>
+                                            value="@Model.DataInicioIndisponibilidade"
+                                            format="dd/MM/yyyy"
+                                            placeholder="Data início"></ejs-datepicker>
                         </div>
                     </div>
                     <div class="col-md-3">
@@ -369,8 +386,9 @@
                                 <i class="fas fa-calendar-day"></i> Data Final
                             </label>
                             <ejs-datepicker id="dataFimIndisponibilidade" name="DataFimIndisponibilidade"
-                                value="@Model.DataFimIndisponibilidade" format="dd/MM/yyyy"
-                                placeholder="Data fim"></ejs-datepicker>
+                                            value="@Model.DataFimIndisponibilidade"
+                                            format="dd/MM/yyyy"
+                                            placeholder="Data fim"></ejs-datepicker>
                         </div>
                     </div>
                     <div class="col-md-3">
@@ -379,8 +397,10 @@
                                 <i class="fas fa-user-shield"></i> Motorista Cobertor
                             </label>
                             <ejs-dropdownlist id="motoristaCobertorId" name="MotoristaCobertorId"
-                                dataSource="@Model.MotoristaList" value="@Model.MotoristaCobertorId"
-                                placeholder="Selecione o cobertor" allowFiltering="true">
+                                              dataSource="@Model.MotoristaList"
+                                              value="@Model.MotoristaCobertorId"
+                                              placeholder="Selecione o cobertor"
+                                              allowFiltering="true">
                                 <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                             </ejs-dropdownlist>
                         </div>
@@ -388,8 +408,7 @@
                 </div>
             </div>
 
-            <div class="economildo-section" id="economildoSection"
-                style="@(Model.MotoristaEconomildo ? "display: block;" : "")">
+            <div class="economildo-section" id="economildoSection" style="@(Model.MotoristaEconomildo ? "display: block;" : "")">
                 <h6><i class="fas fa-user-tie"></i> Configurar Economildo</h6>
                 <div class="row">
                     <div class="col-md-12">
@@ -397,17 +416,19 @@
                             <label asp-for="Lotacao" class="control-label">
                                 <i class="fas fa-map-marker-alt"></i> Lotação
                             </label>
-                            <ejs-dropdownlist id="lotacao" name="Lotacao" dataSource="@Model.LotacaoList"
-                                value="@Model.Lotacao" placeholder="Selecione a lotação" floatLabelType="Auto">
-                                <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
-                            </ejs-dropdownlist>
-                        </div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="servico-section" id="servicoSection"
-                style="@(Model.MotoristaEmServico ? "display: block;" : "")">
+                            <ejs-dropdownlist id="lotacao" name="Lotacao"
+                                              dataSource="@Model.LotacaoList"
+                                              value="@Model.Lotacao"
+                                              placeholder="Selecione a lotação"
+                                              floatLabelType="Auto">
+                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
+                             </ejs-dropdownlist>
+                        </div>
+                    </div>
+                </div>
+            </div>
+
+            <div class="servico-section" id="servicoSection" style="@(Model.MotoristaEmServico ? "display: block;" : "")">
                 <h6><i class="fas fa-user-tie"></i> Configurar Serviço Fixo</h6>
                 <div class="row">
                     <div class="col-md-12">
@@ -416,8 +437,10 @@
                                 <i class="fas fa-user-tie"></i> Requisitante
                             </label>
                             <ejs-dropdownlist id="requisitanteId" name="RequisitanteId"
-                                dataSource="@Model.RequisitanteList" value="@Model.RequisitanteId"
-                                placeholder="Selecione o requisitante" allowFiltering="true">
+                                              dataSource="@Model.RequisitanteList"
+                                              value="@Model.RequisitanteId"
+                                              placeholder="Selecione o requisitante"
+                                              allowFiltering="true">
                                 <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                             </ejs-dropdownlist>
                         </div>
@@ -436,8 +459,11 @@
                 <label asp-for="Observacoes" class="control-label">
                     <i class="fas fa-comment"></i> Observações
                 </label>
-                <ejs-textbox id="observacoes" name="Observacoes" value="@Model.Observacoes" multiline="true"
-                    placeholder="Digite as observações..." floatLabelType="Auto"></ejs-textbox>
+                <ejs-textbox id="observacoes" name="Observacoes"
+                             value="@Model.Observacoes"
+                             multiline="true"
+                             placeholder="Digite as observações..."
+                             floatLabelType="Auto"></ejs-textbox>
             </div>
         </div>
     </div>
```
