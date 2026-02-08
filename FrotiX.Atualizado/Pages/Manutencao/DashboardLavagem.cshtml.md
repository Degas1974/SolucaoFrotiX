# Pages/Manutencao/DashboardLavagem.cshtml

**Mudanca:** GRANDE | **+0** linhas | **-48** linhas

---

```diff
--- JANEIRO: Pages/Manutencao/DashboardLavagem.cshtml
+++ ATUAL: Pages/Manutencao/DashboardLavagem.cshtml
@@ -649,57 +649,6 @@
 </div>
 
 <div class="row g-4 mb-4">
-    <div class="col-12">
-        <div class="chart-container-lav">
-            <h5 class="chart-title-lav">
-                <i class="fa-duotone fa-stopwatch"></i> Duracao das Lavagens
-                <span id="msgMockup" class="badge bg-warning text-dark ms-2" style="display: none; font-size: 0.7rem;">
-                    <i class="fa-solid fa-info-circle"></i> Dados de exemplo (preencha Hora Fim para ver dados reais)
-                </span>
-            </h5>
-
-            <div class="row mb-3" id="statsDuracao">
-                <div class="col-md-3">
-                    <div class="stat-mini-lav text-center p-2 rounded" style="background: linear-gradient(135deg, #ecfeff 0%, #cffafe 100%);">
-                        <div class="text-muted small">Duracao Media</div>
-                        <div class="fw-bold fs-5" style="color: #0891b2;" id="duracaoMedia">-</div>
-                    </div>
-                </div>
-                <div class="col-md-3">
-                    <div class="stat-mini-lav text-center p-2 rounded" style="background: linear-gradient(135deg, #ecfeff 0%, #cffafe 100%);">
-                        <div class="text-muted small">Duracao Minima</div>
-                        <div class="fw-bold fs-5" style="color: #0891b2;" id="duracaoMinima">-</div>
-                    </div>
-                </div>
-                <div class="col-md-3">
-                    <div class="stat-mini-lav text-center p-2 rounded" style="background: linear-gradient(135deg, #ecfeff 0%, #cffafe 100%);">
-                        <div class="text-muted small">Duracao Maxima</div>
-                        <div class="fw-bold fs-5" style="color: #0891b2;" id="duracaoMaxima">-</div>
-                    </div>
-                </div>
-                <div class="col-md-3">
-                    <div class="stat-mini-lav text-center p-2 rounded" style="background: linear-gradient(135deg, #ecfeff 0%, #cffafe 100%);">
-                        <div class="text-muted small">Lavagens c/ Duracao</div>
-                        <div class="fw-bold fs-5" style="color: #0891b2;" id="totalComDuracao">-</div>
-                    </div>
-                </div>
-            </div>
-
-            <div class="row">
-                <div class="col-md-7">
-                    <h6 class="text-muted mb-2"><i class="fa-duotone fa-chart-column me-1"></i> Distribuicao por Faixa de Tempo</h6>
-                    <div id="chartDuracao"></div>
-                </div>
-                <div class="col-md-5">
-                    <h6 class="text-muted mb-2"><i class="fa-duotone fa-layer-group me-1"></i> Tempo Medio por Categoria</h6>
-                    <div id="duracaoPorCategoria"></div>
-                </div>
-            </div>
-        </div>
-    </div>
-</div>
-
-<div class="row g-4 mb-4">
     <div class="col-md-6">
         <div class="chart-container-lav">
             <h5 class="chart-title-lav">
```

### REMOVER do Janeiro

```html
    <div class="col-12">
        <div class="chart-container-lav">
            <h5 class="chart-title-lav">
                <i class="fa-duotone fa-stopwatch"></i> Duracao das Lavagens
                <span id="msgMockup" class="badge bg-warning text-dark ms-2" style="display: none; font-size: 0.7rem;">
                    <i class="fa-solid fa-info-circle"></i> Dados de exemplo (preencha Hora Fim para ver dados reais)
                </span>
            </h5>
            <div class="row mb-3" id="statsDuracao">
                <div class="col-md-3">
                    <div class="stat-mini-lav text-center p-2 rounded" style="background: linear-gradient(135deg, #ecfeff 0%, #cffafe 100%);">
                        <div class="text-muted small">Duracao Media</div>
                        <div class="fw-bold fs-5" style="color: #0891b2;" id="duracaoMedia">-</div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-mini-lav text-center p-2 rounded" style="background: linear-gradient(135deg, #ecfeff 0%, #cffafe 100%);">
                        <div class="text-muted small">Duracao Minima</div>
                        <div class="fw-bold fs-5" style="color: #0891b2;" id="duracaoMinima">-</div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-mini-lav text-center p-2 rounded" style="background: linear-gradient(135deg, #ecfeff 0%, #cffafe 100%);">
                        <div class="text-muted small">Duracao Maxima</div>
                        <div class="fw-bold fs-5" style="color: #0891b2;" id="duracaoMaxima">-</div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-mini-lav text-center p-2 rounded" style="background: linear-gradient(135deg, #ecfeff 0%, #cffafe 100%);">
                        <div class="text-muted small">Lavagens c/ Duracao</div>
                        <div class="fw-bold fs-5" style="color: #0891b2;" id="totalComDuracao">-</div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7">
                    <h6 class="text-muted mb-2"><i class="fa-duotone fa-chart-column me-1"></i> Distribuicao por Faixa de Tempo</h6>
                    <div id="chartDuracao"></div>
                </div>
                <div class="col-md-5">
                    <h6 class="text-muted mb-2"><i class="fa-duotone fa-layer-group me-1"></i> Tempo Medio por Categoria</h6>
                    <div id="duracaoPorCategoria"></div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row g-4 mb-4">
```

