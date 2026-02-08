# Pages/Frota/DashboardEconomildo.cshtml

**Mudanca:** GRANDE | **+63** linhas | **-289** linhas

---

```diff
--- JANEIRO: Pages/Frota/DashboardEconomildo.cshtml
+++ ATUAL: Pages/Frota/DashboardEconomildo.cshtml
@@ -208,17 +208,9 @@
         line-height: 1;
     }
 
-    .eco-mob-card.rodoviaria .eco-mob-value {
-        color: #f97316;
-    }
-
-    .eco-mob-card.pgr .eco-mob-value {
-        color: #3b82f6;
-    }
-
-    .eco-mob-card.cefor .eco-mob-value {
-        color: #8b5cf6;
-    }
+    .eco-mob-card.rodoviaria .eco-mob-value { color: #f97316; }
+    .eco-mob-card.pgr .eco-mob-value { color: #3b82f6; }
+    .eco-mob-card.cefor .eco-mob-value { color: #8b5cf6; }
 
     .eco-mob-sublabel {
         font-size: 0.8rem;
@@ -296,13 +288,8 @@
         letter-spacing: 0.3px;
     }
 
-    .eco-trajeto-item.ida .eco-trajeto-tipo {
-        color: #3b82f6;
-    }
-
-    .eco-trajeto-item.volta .eco-trajeto-tipo {
-        color: #22c55e;
-    }
+    .eco-trajeto-item.ida .eco-trajeto-tipo { color: #3b82f6; }
+    .eco-trajeto-item.volta .eco-trajeto-tipo { color: #22c55e; }
 
     .eco-trajeto-destino {
         font-size: 0.75rem;
@@ -521,17 +508,9 @@
         border-radius: 50%;
     }
 
-    .eco-turno-dot.manha {
-        background: #f97316;
-    }
-
-    .eco-turno-dot.tarde {
-        background: #3b82f6;
-    }
-
-    .eco-turno-dot.noite {
-        background: #8b5cf6;
-    }
+    .eco-turno-dot.manha { background: #f97316; }
+    .eco-turno-dot.tarde { background: #3b82f6; }
+    .eco-turno-dot.noite { background: #8b5cf6; }
 
     /* ========== HEATMAP STYLES ========== */
     .eco-heatmap-card {
@@ -594,76 +573,22 @@
     }
 
     /* Cores do heatmap - escala terracota (viagens) */
-    .eco-heatmap-0 {
-        background: #faf6f4;
-        color: #94a3b8;
-    }
-
-    .eco-heatmap-1 {
-        background: #fee4e2;
-        color: #b45a3c;
-    }
-
-    .eco-heatmap-2 {
-        background: #fecaca;
-        color: #b45a3c;
-    }
-
-    .eco-heatmap-3 {
-        background: #f5a898;
-        color: #8b4531;
-    }
-
-    .eco-heatmap-4 {
-        background: #e8a87c;
-        color: #ffffff;
-    }
-
-    .eco-heatmap-5 {
-        background: #c96d4e;
-        color: #ffffff;
-    }
-
-    .eco-heatmap-6 {
-        background: #b45a3c;
-        color: #ffffff;
-    }
+    .eco-heatmap-0 { background: #faf6f4; color: #94a3b8; }
+    .eco-heatmap-1 { background: #fee4e2; color: #b45a3c; }
+    .eco-heatmap-2 { background: #fecaca; color: #b45a3c; }
+    .eco-heatmap-3 { background: #f5a898; color: #8b4531; }
+    .eco-heatmap-4 { background: #e8a87c; color: #ffffff; }
+    .eco-heatmap-5 { background: #c96d4e; color: #ffffff; }
+    .eco-heatmap-6 { background: #b45a3c; color: #ffffff; }
 
     /* Cores do heatmap passageiros - escala laranja */
-    .eco-heatmap-passageiros-0 {
-        background: #fffbeb;
-        color: #78716c;
-    }
-
-    .eco-heatmap-passageiros-1 {
-        background: #fef3c7;
-        color: #92400e;
-    }
-
-    .eco-heatmap-passageiros-2 {
-        background: #fde68a;
-        color: #92400e;
-    }
-
-    .eco-heatmap-passageiros-3 {
-        background: #fcd34d;
-        color: #78350f;
-    }
-
-    .eco-heatmap-passageiros-4 {
-        background: #fbbf24;
-        color: #ffffff;
-    }
-
-    .eco-heatmap-passageiros-5 {
-        background: #f59e0b;
-        color: #ffffff;
-    }
-
-    .eco-heatmap-passageiros-6 {
-        background: #d97706;
-        color: #ffffff;
-    }
+    .eco-heatmap-passageiros-0 { background: #fffbeb; color: #78716c; }
+    .eco-heatmap-passageiros-1 { background: #fef3c7; color: #92400e; }
+    .eco-heatmap-passageiros-2 { background: #fde68a; color: #92400e; }
+    .eco-heatmap-passageiros-3 { background: #fcd34d; color: #78350f; }
+    .eco-heatmap-passageiros-4 { background: #fbbf24; color: #ffffff; }
+    .eco-heatmap-passageiros-5 { background: #f59e0b; color: #ffffff; }
+    .eco-heatmap-passageiros-6 { background: #d97706; color: #ffffff; }
 
     /* Legenda do heatmap */
     .eco-heatmap-legend {
@@ -746,7 +671,6 @@
 
     /* ========== RESPONSIVIDADE ========== */
     @@media (max-width: 1400px) {
-
         .eco-kpi-card.span-2,
         .eco-kpi-card.span-3 {
             grid-column: span 4;
@@ -825,14 +749,11 @@
     <div class="eco-header">
         <div class="eco-header-title">
             <div class="eco-header-icon">
-                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                    stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                     <path d="M8 6v6"></path>
                     <path d="M15 6v6"></path>
                     <path d="M2 12h19.6"></path>
-                    <path
-                        d="M18 18h3s.5-1.7.8-2.8c.1-.4.2-.8.2-1.2 0-.4-.1-.8-.2-1.2l-1.4-5C20.1 6.8 19.1 6 18 6H4a2 2 0 0 0-2 2v10h3">
-                    </path>
+                    <path d="M18 18h3s.5-1.7.8-2.8c.1-.4.2-.8.2-1.2 0-.4-.1-.8-.2-1.2l-1.4-5C20.1 6.8 19.1 6 18 6H4a2 2 0 0 0-2 2v10h3"></path>
                     <circle cx="7" cy="18" r="2"></circle>
                     <circle cx="17" cy="18" r="2"></circle>
                 </svg>
@@ -872,8 +793,7 @@
             <div class="eco-trajetos">
                 <div class="eco-trajeto-item ida">
                     <div class="eco-trajeto-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                             <path d="M5 12h14"></path>
                             <path d="m12 5 7 7-7 7"></path>
                         </svg>
@@ -891,8 +811,7 @@
                 </div>
                 <div class="eco-trajeto-item volta">
                     <div class="eco-trajeto-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                             <path d="M19 12H5"></path>
                             <path d="m12 19-7-7 7-7"></path>
                         </svg>
@@ -925,8 +844,7 @@
             <div class="eco-trajetos">
                 <div class="eco-trajeto-item ida">
                     <div class="eco-trajeto-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                             <path d="M5 12h14"></path>
                             <path d="m12 5 7 7-7 7"></path>
                         </svg>
@@ -944,8 +862,7 @@
                 </div>
                 <div class="eco-trajeto-item volta">
                     <div class="eco-trajeto-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                             <path d="M19 12H5"></path>
                             <path d="m12 19-7-7 7-7"></path>
                         </svg>
@@ -978,8 +895,7 @@
             <div class="eco-trajetos">
                 <div class="eco-trajeto-item ida">
                     <div class="eco-trajeto-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                             <path d="M5 12h14"></path>
                             <path d="m12 5 7 7-7 7"></path>
                         </svg>
@@ -997,8 +913,7 @@
                 </div>
                 <div class="eco-trajeto-item volta">
                     <div class="eco-trajeto-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                             <path d="M19 12H5"></path>
                             <path d="m12 19-7-7 7-7"></path>
                         </svg>
@@ -1022,8 +937,7 @@
 
         <div class="eco-kpi-card span-3">
             <div class="eco-kpi-icon">
-                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                    stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                     <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"></path>
                     <circle cx="9" cy="7" r="4"></circle>
                     <path d="M22 21v-2a4 4 0 0 0-3-3.87"></path>
@@ -1036,8 +950,7 @@
 
         <div class="eco-kpi-card span-3">
             <div class="eco-kpi-icon">
-                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                    stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                     <path d="M14 18V6a2 2 0 0 0-2-2H4a2 2 0 0 0-2 2v11a1 1 0 0 0 1 1h2"></path>
                     <path d="M15 18H9"></path>
                     <path d="M19 18h2a1 1 0 0 0 1-1v-3.65a1 1 0 0 0-.22-.624l-3.48-4.35A1 1 0 0 0 17.52 8H14"></path>
@@ -1051,8 +964,7 @@
 
         <div class="eco-kpi-card span-3">
             <div class="eco-kpi-icon">
-                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                    stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                     <rect x="3" y="4" width="18" height="18" rx="2" ry="2"></rect>
                     <line x1="16" y1="2" x2="16" y2="6"></line>
                     <line x1="8" y1="2" x2="8" y2="6"></line>
@@ -1065,8 +977,7 @@
 
         <div class="eco-kpi-card span-3">
             <div class="eco-kpi-icon">
-                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                    stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                     <path d="M12 20V10"></path>
                     <path d="M18 20V4"></path>
                     <path d="M6 20v-4"></path>
@@ -1083,8 +994,7 @@
             <div class="eco-chart-header">
                 <div class="eco-chart-title">
                     <div class="eco-chart-title-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                             <line x1="18" y1="20" x2="18" y2="10"></line>
                             <line x1="12" y1="20" x2="12" y2="4"></line>
                             <line x1="6" y1="20" x2="6" y2="14"></line>
@@ -1093,10 +1003,7 @@
                     Usuarios por Mes
                 </div>
                 <button type="button" class="eco-btn-pdf" onclick="exportarPdf('UsuariosMes')" title="Exportar PDF">
-                    <svg viewBox="0 0 512 512">
-                        <path fill="currentColor"
-                            d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z" />
-                    </svg>
+                    <svg viewBox="0 0 512 512"><path fill="currentColor" d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z"/></svg>
                     PDF
                 </button>
             </div>
@@ -1109,8 +1016,7 @@
             <div class="eco-chart-header">
                 <div class="eco-chart-title">
                     <div class="eco-chart-title-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                             <circle cx="12" cy="12" r="10"></circle>
                             <path d="M12 2a7 7 0 0 0 0 14"></path>
                         </svg>
@@ -1118,10 +1024,7 @@
                     Usuarios por Turno
                 </div>
                 <button type="button" class="eco-btn-pdf" onclick="exportarPdf('UsuariosTurno')" title="Exportar PDF">
-                    <svg viewBox="0 0 512 512">
-                        <path fill="currentColor"
-                            d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z" />
-                    </svg>
+                    <svg viewBox="0 0 512 512"><path fill="currentColor" d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z"/></svg>
                     PDF
                 </button>
             </div>
@@ -1148,8 +1051,7 @@
             <div class="eco-chart-header">
                 <div class="eco-chart-title">
                     <div class="eco-chart-title-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                             <path d="M3 3v18h18"></path>
                             <path d="m19 9-5 5-4-4-3 3"></path>
                         </svg>
@@ -1157,10 +1059,7 @@
                     Comparativo Mensal por MOB
                 </div>
                 <button type="button" class="eco-btn-pdf" onclick="exportarPdf('ComparativoMob')" title="Exportar PDF">
-                    <svg viewBox="0 0 512 512">
-                        <path fill="currentColor"
-                            d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z" />
-                    </svg>
+                    <svg viewBox="0 0 512 512"><path fill="currentColor" d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z"/></svg>
                     PDF
                 </button>
             </div>
@@ -1173,8 +1072,7 @@
             <div class="eco-chart-header">
                 <div class="eco-chart-title">
                     <div class="eco-chart-title-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                             <rect x="3" y="4" width="18" height="18" rx="2" ry="2"></rect>
                             <line x1="16" y1="2" x2="16" y2="6"></line>
                             <line x1="8" y1="2" x2="8" y2="6"></line>
@@ -1183,12 +1081,8 @@
                     </div>
                     Usuarios por Dia da Semana
                 </div>
-                <button type="button" class="eco-btn-pdf" onclick="exportarPdf('UsuariosDiaSemana')"
-                    title="Exportar PDF">
-                    <svg viewBox="0 0 512 512">
-                        <path fill="currentColor"
-                            d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z" />
-                    </svg>
+                <button type="button" class="eco-btn-pdf" onclick="exportarPdf('UsuariosDiaSemana')" title="Exportar PDF">
+                    <svg viewBox="0 0 512 512"><path fill="currentColor" d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z"/></svg>
                     PDF
                 </button>
             </div>
@@ -1201,20 +1095,15 @@
             <div class="eco-chart-header">
                 <div class="eco-chart-title">
                     <div class="eco-chart-title-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                             <circle cx="12" cy="12" r="10"></circle>
                             <polyline points="12 6 12 12 16 14"></polyline>
                         </svg>
                     </div>
                     Distribuicao por Horario
                 </div>
-                <button type="button" class="eco-btn-pdf" onclick="exportarPdf('DistribuicaoHorario')"
-                    title="Exportar PDF">
-                    <svg viewBox="0 0 512 512">
-                        <path fill="currentColor"
-                            d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z" />
-                    </svg>
+                <button type="button" class="eco-btn-pdf" onclick="exportarPdf('DistribuicaoHorario')" title="Exportar PDF">
+                    <svg viewBox="0 0 512 512"><path fill="currentColor" d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z"/></svg>
                     PDF
                 </button>
             </div>
@@ -1227,14 +1116,11 @@
             <div class="eco-chart-header">
                 <div class="eco-chart-title">
                     <div class="eco-chart-title-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                             <path d="M8 6v6"></path>
                             <path d="M15 6v6"></path>
                             <path d="M2 12h19.6"></path>
-                            <path
-                                d="M18 18h3s.5-1.7.8-2.8c.1-.4.2-.8.2-1.2 0-.4-.1-.8-.2-1.2l-1.4-5C20.1 6.8 19.1 6 18 6H4a2 2 0 0 0-2 2v10h3">
-                            </path>
+                            <path d="M18 18h3s.5-1.7.8-2.8c.1-.4.2-.8.2-1.2 0-.4-.1-.8-.2-1.2l-1.4-5C20.1 6.8 19.1 6 18 6H4a2 2 0 0 0-2 2v10h3"></path>
                             <circle cx="7" cy="18" r="2"></circle>
                             <circle cx="17" cy="18" r="2"></circle>
                         </svg>
@@ -1242,10 +1128,7 @@
                     Top 10 Veiculos
                 </div>
                 <button type="button" class="eco-btn-pdf" onclick="exportarPdf('TopVeiculos')" title="Exportar PDF">
-                    <svg viewBox="0 0 512 512">
-                        <path fill="currentColor"
-                            d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z" />
-                    </svg>
+                    <svg viewBox="0 0 512 512"><path fill="currentColor" d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z"/></svg>
                     PDF
                 </button>
             </div>
@@ -1258,8 +1141,7 @@
             <div class="eco-chart-header">
                 <div class="eco-chart-title">
                     <div class="eco-chart-title-icon">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                             <rect x="3" y="3" width="7" height="7"></rect>
                             <rect x="14" y="3" width="7" height="7"></rect>
                             <rect x="14" y="14" width="7" height="7"></rect>
@@ -1269,10 +1151,7 @@
                     Mapa de Calor - Distribuicao de Viagens (Dia x Hora)
                 </div>
                 <button type="button" class="eco-btn-pdf" onclick="exportarPdf('HeatmapViagens')" title="Exportar PDF">
-                    <svg viewBox="0 0 512 512">
-                        <path fill="currentColor"
-                            d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z" />
-                    </svg>
+                    <svg viewBox="0 0 512 512"><path fill="currentColor" d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z"/></svg>
                     PDF
                 </button>
             </div>
@@ -1281,30 +1160,10 @@
                     <thead>
                         <tr>
                             <th>Dia/Hora</th>
-                            <th>00</th>
-                            <th>01</th>
-                            <th>02</th>
-                            <th>03</th>
-                            <th>04</th>
-                            <th>05</th>
-                            <th>06</th>
-                            <th>07</th>
-                            <th>08</th>
-                            <th>09</th>
-                            <th>10</th>
-                            <th>11</th>
-                            <th>12</th>
-                            <th>13</th>
-                            <th>14</th>
-                            <th>15</th>
-                            <th>16</th>
-                            <th>17</th>
-                            <th>18</th>
-                            <th>19</th>
-                            <th>20</th>
-                            <th>21</th>
-                            <th>22</th>
-                            <th>23</th>
+                            <th>00</th><th>01</th><th>02</th><th>03</th><th>04</th><th>05</th>
+                            <th>06</th><th>07</th><th>08</th><th>09</th><th>10</th><th>11</th>
+                            <th>12</th><th>13</th><th>14</th><th>15</th><th>16</th><th>17</th>
+                            <th>18</th><th>19</th><th>20</th><th>21</th><th>22</th><th>23</th>
                         </tr>
                     </thead>
                     <tbody id="heatmapBody"></tbody>
@@ -1328,10 +1187,8 @@
         <div class="eco-chart-card eco-heatmap-card">
             <div class="eco-chart-header">
                 <div class="eco-chart-title">
-                    <div class="eco-chart-title-icon"
-                        style="background: linear-gradient(135deg, #fef3c7, #fde68a); color: #d97706;">
-                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor"
-                            stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
+                    <div class="eco-chart-title-icon" style="background: linear-gradient(135deg, #fef3c7, #fde68a); color: #d97706;">
+                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                             <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"></path>
                             <circle cx="9" cy="7" r="4"></circle>
                             <path d="M22 21v-2a4 4 0 0 0-3-3.87"></path>
@@ -1340,12 +1197,8 @@
                     </div>
                     Mapa de Calor - Distribuicao de Passageiros (Dia x Hora)
                 </div>
-                <button type="button" class="eco-btn-pdf" onclick="exportarPdf('HeatmapPassageiros')"
-                    title="Exportar PDF">
-                    <svg viewBox="0 0 512 512">
-                        <path fill="currentColor"
-                            d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z" />
-                    </svg>
+                <button type="button" class="eco-btn-pdf" onclick="exportarPdf('HeatmapPassageiros')" title="Exportar PDF">
+                    <svg viewBox="0 0 512 512"><path fill="currentColor" d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128z"/></svg>
                     PDF
                 </button>
             </div>
@@ -1354,30 +1207,10 @@
                     <thead>
                         <tr>
                             <th>Dia/Hora</th>
-                            <th>00</th>
-                            <th>01</th>
-                            <th>02</th>
-                            <th>03</th>
-                            <th>04</th>
-                            <th>05</th>
-                            <th>06</th>
-                            <th>07</th>
-                            <th>08</th>
-                            <th>09</th>
-                            <th>10</th>
-                            <th>11</th>
-                            <th>12</th>
-                            <th>13</th>
-                            <th>14</th>
-                            <th>15</th>
-                            <th>16</th>
-                            <th>17</th>
-                            <th>18</th>
-                            <th>19</th>
-                            <th>20</th>
-                            <th>21</th>
-                            <th>22</th>
-                            <th>23</th>
+                            <th>00</th><th>01</th><th>02</th><th>03</th><th>04</th><th>05</th>
+                            <th>06</th><th>07</th><th>08</th><th>09</th><th>10</th><th>11</th>
+                            <th>12</th><th>13</th><th>14</th><th>15</th><th>16</th><th>17</th>
+                            <th>18</th><th>19</th><th>20</th><th>21</th><th>22</th><th>23</th>
                         </tr>
                     </thead>
                     <tbody id="heatmapPassageirosBody"></tbody>
@@ -1403,22 +1236,7 @@
 @section ScriptsBlock {
     <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.1/dist/chart.umd.min.js" asp-append-version="true"></script>
     <script asp-append-version="true">
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * DASHBOARD ECONOMILDO - VISUALIZAÇÃO DE DADOS DE FROTA
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Dashboard interativo com gráficos Chart.js para análise de
-            * consumo, usuários e viagens da frota.Inclui filtros por MOB,
-             * mês e ano com atualização dinâmica via AJAX.
-             * @@requires Chart.js 4.4.1 +
-             */
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * VARIÁVEIS GLOBAIS DOS GRÁFICOS
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@type { Chart | null } Instâncias dos gráficos Chart.js para destruição / recriação
-            */
+
         let chartUsuariosMes = null;
         let chartTurno = null;
         let chartComparativoMob = null;
@@ -1426,10 +1244,6 @@
         let chartHora = null;
         let chartTopVeiculos = null;
 
-            /**
-             * Paleta de cores do tema terracota
-             * @@type { Object } cores - Objeto com cores temáticas para gráficos
-            */
         const cores = {
             primary: '#b45a3c',
             secondary: '#c96d4e',
@@ -1442,19 +1256,9 @@
             gradient: ['#b45a3c', '#d4856a']
         };
 
-            /**
-             * Configuração global de fonte para Chart.js
-             * @@description Define família de fontes e cor padrão para todos os gráficos
-            */
         Chart.defaults.font.family = "'Inter', 'Segoe UI', sans-serif";
         Chart.defaults.color = '#64748b';
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * INICIALIZAÇÃO DO DOCUMENTO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Inicializa gráficos e configura eventos de filtro
-            */
         document.addEventListener('DOMContentLoaded', function () {
             try {
                 inicializarGraficos();
@@ -1468,13 +1272,6 @@
             }
         });
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * INICIALIZAÇÃO DOS GRÁFICOS CHART.JS
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Cria instâncias dos gráficos com configurações padrão.
-             * Os dados são carregados posteriormente via carregarDashboard()
-            */
         function inicializarGraficos() {
             try {
 
@@ -1717,17 +1514,6 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * CARREGAMENTO DE DADOS DO DASHBOARD
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Carrega dados do dashboard via API
-             * @@description Faz fetch para / api / Viagem / DashboardEconomildo com filtros
-            * e atualiza todos os gráficos e KPIs
-                */
         function carregarDashboard() {
             try {
                 const mob = document.getElementById('filtroMob').value;
@@ -1759,12 +1545,6 @@
             }
         }
 
-            /**
-             * Atualiza todos os elementos do dashboard com dados recebidos
-             * @@description Atualiza KPIs, cards MOB e dados de todos os gráficos Chart.js
-            * @@param { Object } data - Dados retornados pela API
-                * @@param { string } mobFiltro - Filtro MOB selecionado
-                    */
         function atualizarDashboard(data, mobFiltro) {
             try {
 
@@ -1843,11 +1623,6 @@
             }
         }
 
-            /**
-             * Extrai valor numérico de minutos de string formatada
-             * @@param { string } valor - Valor no formato "12 min"
-            * @@returns { string } Número sem sufixo ou "-" se inválido
-                */
         function extrairMinutos(valor) {
             try {
                 if (!valor) return '-';
@@ -1857,17 +1632,6 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES DE HEATMAP
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Renderização de mapas de calor para visualização de viagens
-            */
-
-            /**
-             * Carrega dados do heatmap de viagens via API
-             * @@description Faz fetch para / api / Viagem / HeatmapEconomildo
-            */
         function carregarHeatmap() {
             try {
                 const mob = document.getElementById('filtroMob').value;
@@ -2017,17 +1781,6 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES AUXILIARES DE FORMATAÇÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Formata número para padrão brasileiro
-             * @@param { number } num - Número a formatar
-            * @@returns { string } Número formatado(ex: "1.234") ou "-" se inválido
-                */
         function formatarNumero(num) {
             try {
                 if (num === null || num === undefined) return '-';
```
