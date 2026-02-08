# Pages/Escalas/UpsertCEscala.cshtml

**Mudanca:** GRANDE | **+91** linhas | **-96** linhas

---

```diff
--- JANEIRO: Pages/Escalas/UpsertCEscala.cshtml
+++ ATUAL: Pages/Escalas/UpsertCEscala.cshtml
@@ -16,35 +16,12 @@
         border-radius: 5px;
         transition: all 0.3s;
     }
-
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
-    .indisponibilidade-section,
-    .servico-section,
-    .economildo-section {
+    .status-checkbox.active-disponivel { background-color: #d4edda; border: 2px solid #28a745; }
+    .status-checkbox.active-indisponivel { background-color: #f8d7da; border: 2px solid #dc3545; }
+    .status-checkbox.active-economildo { background-color: #d1ecf1; border: 2px solid #17a2b8; }
+    .status-checkbox.active-servico { background-color: #fff3cd; border: 2px solid #ffc107; }
+    .status-checkbox.active-reservado { background-color: #e2e3e5; border: 2px solid #6c757d; }
+    .indisponibilidade-section, .servico-section, .economildo-section {
         display: none;
         margin-top: 20px;
         padding: 15px;
@@ -59,20 +36,17 @@
         border-radius: 5px;
         border: 1px solid #dee2e6;
     }
-
     .dia-checkbox {
         display: inline-block;
         margin-right: 15px;
         margin-bottom: 10px;
     }
-
     .dia-checkbox input[type="checkbox"] {
         width: 18px;
         height: 18px;
         margin-right: 5px;
         vertical-align: middle;
     }
-
     .dia-checkbox label {
         font-weight: 500;
         vertical-align: middle;
@@ -107,8 +81,11 @@
                         <label asp-for="MotoristaId" class="control-label required">
                             <i class="fas fa-user"></i> Motorista
                         </label>
-                        <ejs-dropdownlist id="motoristaId" name="MotoristaId" dataSource="@Model.MotoristaList"
-                            placeholder="Selecione o motorista" allowFiltering="true" floatLabelType="Auto">
+                        <ejs-dropdownlist id="motoristaId" name="MotoristaId"
+                                         dataSource="@Model.MotoristaList"
+                                         placeholder="Selecione o motorista"
+                                         allowFiltering="true"
+                                         floatLabelType="Auto">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                         <span asp-validation-for="MotoristaId" class="text-danger"></span>
@@ -119,13 +96,15 @@
                         <label asp-for="VeiculoId" class="control-label">
                             <i class="fas fa-car"></i> Veículo
                         </label>
-                        <ejs-dropdownlist id="veiculoId" name="VeiculoId" dataSource="@Model.VeiculoList"
-                            placeholder="Selecione o veículo" allowFiltering="true" floatLabelType="Auto">
+                        <ejs-dropdownlist id="veiculoId" name="VeiculoId"
+                                         dataSource="@Model.VeiculoList"
+                                         placeholder="Selecione o veículo"
+                                         allowFiltering="true"
+                                         floatLabelType="Auto">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                         <div class="form-check mt-2">
-                            <input type="checkbox" class="form-check-input" id="veiculoNaoDefinido"
-                                name="VeiculoNaoDefinido" />
+                            <input type="checkbox" class="form-check-input" id="veiculoNaoDefinido" name="VeiculoNaoDefinido" />
                             <label class="form-check-label" for="veiculoNaoDefinido">
                                 <i class="fas fa-ban text-muted"></i> Veículo não definido
                             </label>
@@ -141,8 +120,11 @@
                         <label asp-for="DataInicio" class="control-label required">
                             <i class="fas fa-calendar-day"></i> Data Início
                         </label>
-                        <ejs-datepicker id="dataInicio" name="DataInicio" value="@Model.DataInicio" format="dd/MM/yyyy"
-                            placeholder="Selecione a data de início" floatLabelType="Auto"></ejs-datepicker>
+                        <ejs-datepicker id="dataInicio" name="DataInicio"
+                                       value="@Model.DataInicio"
+                                       format="dd/MM/yyyy"
+                                       placeholder="Selecione a data de início"
+                                       floatLabelType="Auto"></ejs-datepicker>
                         <span asp-validation-for="DataInicio" class="text-danger"></span>
                         <small class="form-text text-muted">
                             Data inicial do período de vigência da escala
@@ -154,8 +136,11 @@
                         <label asp-for="DataFim" class="control-label required">
                             <i class="fas fa-calendar-day"></i> Data Fim
                         </label>
-                        <ejs-datepicker id="dataFim" name="DataFim" value="@Model.DataFim" format="dd/MM/yyyy"
-                            placeholder="Selecione a data fim" floatLabelType="Auto"></ejs-datepicker>
+                        <ejs-datepicker id="dataFim" name="DataFim"
+                                       value="@Model.DataFim"
+                                       format="dd/MM/yyyy"
+                                       placeholder="Selecione a data fim"
+                                       floatLabelType="Auto"></ejs-datepicker>
                         <span asp-validation-for="DataFim" class="text-danger"></span>
                         <small class="form-text text-muted">
                             Data final do período de vigência da escala
@@ -211,8 +196,10 @@
                         <label asp-for="TurnoId" class="control-label required">
                             <i class="fas fa-clock"></i> Turno
                         </label>
-                        <ejs-dropdownlist id="turnoId" name="TurnoId" dataSource="@Model.TurnoList"
-                            placeholder="Selecione o turno" floatLabelType="Auto">
+                        <ejs-dropdownlist id="turnoId" name="TurnoId"
+                                         dataSource="@Model.TurnoList"
+                                         placeholder="Selecione o turno"
+                                         floatLabelType="Auto">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                         <span asp-validation-for="TurnoId" class="text-danger"></span>
@@ -223,8 +210,10 @@
                         <label asp-for="TipoServicoId" class="control-label required">
                             <i class="fas fa-briefcase"></i> Tipo de Serviço
                         </label>
-                        <ejs-dropdownlist id="tipoServicoId" name="TipoServicoId" dataSource="@Model.TipoServicoList"
-                            placeholder="Selecione o tipo de serviço" floatLabelType="Auto">
+                        <ejs-dropdownlist id="tipoServicoId" name="TipoServicoId"
+                                         dataSource="@Model.TipoServicoList"
+                                         placeholder="Selecione o tipo de serviço"
+                                         floatLabelType="Auto">
                             <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                         </ejs-dropdownlist>
                         <span asp-validation-for="TipoServicoId" class="text-danger"></span>
@@ -238,8 +227,12 @@
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
@@ -248,8 +241,12 @@
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
@@ -266,8 +263,7 @@
                 <div class="col-md-2">
                     <div class="status-checkbox" id="statusDisponivel">
                         <div class="form-check">
-                            <input type="radio" name="StatusMotorista" value="Disponível" checked
-                                class="form-check-input" />
+                            <input type="radio" name="StatusMotorista" value="Disponível" checked class="form-check-input" />
                             <label class="form-check-label">
                                 <i class="fas fa-check-circle text-success"></i> Disponível
                             </label>
@@ -323,11 +319,12 @@
                         <div class="form-group">
                             <label asp-for="CategoriaIndisponibilidade"></label>
                             <ejs-dropdownlist id="categoriaIndisponibilidade" name="CategoriaIndisponibilidade"
-                                dataSource='@(new List<object> {
-                                                                                                                 new { text = "Folga", value = "Folga" },
-                                                                                                                 new { text = "Férias", value = "Férias" },
-                                                                                                                 new { text = "Recesso", value = "Recesso" }
-                                                                                                             })' placeholder="Selecione a categoria">
+                                             dataSource='@(new List<object> {
+                                                 new { text = "Folga", value = "Folga" },
+                                                 new { text = "Férias", value = "Férias" },
+                                                 new { text = "Recesso", value = "Recesso" }
+                                             })'
+                                             placeholder="Selecione a categoria">
                                 <e-dropdownlist-fields text="text" value="value"></e-dropdownlist-fields>
                             </ejs-dropdownlist>
                         </div>
@@ -336,22 +333,25 @@
                         <div class="form-group">
                             <label asp-for="DataInicioIndisponibilidade"></label>
                             <ejs-datepicker id="dataInicioIndisponibilidade" name="DataInicioIndisponibilidade"
-                                format="dd/MM/yyyy" placeholder="Data início"></ejs-datepicker>
+                                           format="dd/MM/yyyy"
+                                           placeholder="Data início"></ejs-datepicker>
                         </div>
                     </div>
                     <div class="col-md-3">
                         <div class="form-group">
                             <label asp-for="DataFimIndisponibilidade"></label>
                             <ejs-datepicker id="dataFimIndisponibilidade" name="DataFimIndisponibilidade"
-                                format="dd/MM/yyyy" placeholder="Data fim"></ejs-datepicker>
+                                           format="dd/MM/yyyy"
+                                           placeholder="Data fim"></ejs-datepicker>
                         </div>
                     </div>
                     <div class="col-md-3">
                         <div class="form-group">
                             <label asp-for="MotoristaCobertorId"></label>
                             <ejs-dropdownlist id="motoristaCobertorId" name="MotoristaCobertorId"
-                                dataSource="@Model.MotoristaList" placeholder="Selecione o cobertor"
-                                allowFiltering="true">
+                                             dataSource="@Model.MotoristaList"
+                                             placeholder="Selecione o cobertor"
+                                             allowFiltering="true">
                                 <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                             </ejs-dropdownlist>
                         </div>
@@ -359,8 +359,7 @@
                 </div>
             </div>
 
-            <div class="economildo-section" id="economildoSection"
-                style="@(Model.MotoristaEconomildo ? "display: block;" : "")">
+            <div class="economildo-section" id="economildoSection" style="@(Model.MotoristaEconomildo ? "display: block;" : "")">
                 <h6><i class="fas fa-user-tie"></i> Configurar Economildo</h6>
                 <div class="row">
                     <div class="col-md-12">
@@ -368,8 +367,11 @@
                             <label asp-for="Lotacao" class="control-label">
                                 <i class="fas fa-map-marker-alt"></i> Lotação
                             </label>
-                            <ejs-dropdownlist id="lotacao" name="Lotacao" dataSource="@Model.LotacaoList"
-                                value="@Model.Lotacao" placeholder="Selecione a lotação" floatLabelType="Auto">
+                            <ejs-dropdownlist id="lotacao" name="Lotacao"
+                                              dataSource="@Model.LotacaoList"
+                                              value="@Model.Lotacao"
+                                              placeholder="Selecione a lotação"
+                                              floatLabelType="Auto">
                                 <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                             </ejs-dropdownlist>
                         </div>
@@ -384,8 +386,9 @@
                         <div class="form-group">
                             <label asp-for="RequisitanteId"></label>
                             <ejs-dropdownlist id="requisitanteId" name="RequisitanteId"
-                                dataSource="@Model.RequisitanteList" placeholder="Selecione o requisitante"
-                                allowFiltering="true">
+                                             dataSource="@Model.RequisitanteList"
+                                             placeholder="Selecione o requisitante"
+                                             allowFiltering="true">
                                 <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                             </ejs-dropdownlist>
                         </div>
@@ -402,8 +405,10 @@
         <div class="card-body">
             <div class="form-group">
                 <label asp-for="Observacoes" class="control-label"></label>
-                <ejs-textbox id="observacoes" name="Observacoes" multiline="true" placeholder="Digite as observações..."
-                    floatLabelType="Auto"></ejs-textbox>
+                <ejs-textbox id="observacoes" name="Observacoes"
+                            multiline="true"
+                            placeholder="Digite as observações..."
+                            floatLabelType="Auto"></ejs-textbox>
             </div>
         </div>
     </div>
@@ -421,47 +426,30 @@
 </form>
 
 @section ScriptsBlock {
-    @{
-        await Html.RenderPartialAsync("_ValidationScriptsPartial");
-    }
+    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
 
     <script src="~/js/cadastros/CriarEscala.js" asp-append-version="true"></script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CONTROLE DE EXIBIÇÃO DE SEÇÕES CONDICIONAIS
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Gerencia visibilidade das seções de Indisponibilidade e Serviço
-            * baseado no estado dos checkboxes correspondentes
-                */
-        document.addEventListener('DOMContentLoaded', function () {
-                /**
-                 * Referências aos elementos de controle
-                 * @@type { HTMLInputElement | null } checkboxIndisponivel - Checkbox de motorista indisponível
-                * @@type { HTMLInputElement| null
-        } checkboxServico - Checkbox de motorista em serviço
-        * @@type { HTMLElement| null} sectionIndisponibilidade - Seção de detalhes de indisponibilidade
-        * @@type { HTMLElement| null} sectionServico - Seção de detalhes de serviço
-        */
-                const checkboxIndisponivel = document.getElementById('MotoristaIndisponivel');
-        const checkboxServico = document.getElementById('MotoristaEmServico');
-        const sectionIndisponibilidade = document.getElementById('indisponibilidadeSection');
-        const sectionServico = document.getElementById('servicoSection');
-
-        /** Handler: Alterna visibilidade da seção de indisponibilidade */
-        if (checkboxIndisponivel) {
-            checkboxIndisponivel.addEventListener('change', function () {
-                sectionIndisponibilidade.style.display = this.checked ? 'block' : 'none';
-            });
-        }
-
-        /** Handler: Alterna visibilidade da seção de serviço */
-        if (checkboxServico) {
-            checkboxServico.addEventListener('change', function () {
-                sectionServico.style.display = this.checked ? 'block' : 'none';
-            });
-        }
-            });
+
+        document.addEventListener('DOMContentLoaded', function() {
+
+            const checkboxIndisponivel = document.getElementById('MotoristaIndisponivel');
+            const checkboxServico = document.getElementById('MotoristaEmServico');
+            const sectionIndisponibilidade = document.getElementById('indisponibilidadeSection');
+            const sectionServico = document.getElementById('servicoSection');
+
+            if (checkboxIndisponivel) {
+                checkboxIndisponivel.addEventListener('change', function() {
+                    sectionIndisponibilidade.style.display = this.checked ? 'block' : 'none';
+                });
+            }
+
+            if (checkboxServico) {
+                checkboxServico.addEventListener('change', function() {
+                    sectionServico.style.display = this.checked ? 'block' : 'none';
+                });
+            }
+        });
     </script>
 }
```
