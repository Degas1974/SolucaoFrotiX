# wwwroot/css/modal-viagens-consolidado.css

**Mudanca:** GRANDE | **+30** linhas | **-193** linhas

---

```diff
--- JANEIRO: wwwroot/css/modal-viagens-consolidado.css
+++ ATUAL: wwwroot/css/modal-viagens-consolidado.css
@@ -1,14 +1,3 @@
-#divNoFichaVistoria {
-    display: none !important;
-    visibility: hidden !important;
-    opacity: 0 !important;
-    height: 0 !important;
-    width: 0 !important;
-    overflow: hidden !important;
-    position: absolute !important;
-    left: -9999px !important;
-}
-
 @import url('https://fonts.googleapis.com/css2?family=Outfit:wght@400;600;700;900&display=swap');
 
 body {
@@ -173,6 +162,30 @@
 
 .e-datepicker .e-input-group-icon,
 .e-timepicker .e-input-group-icon {
+    height: 38px !important;
+    line-height: 38px !important;
+    width: 38px !important;
+    font-size: 16px !important;
+}
+
+.k-datepicker.k-input,
+.k-timepicker.k-input {
+    height: 38px !important;
+    min-height: 38px !important;
+    border: 1px solid #ced4da !important;
+    border-radius: 0.375rem !important;
+    box-sizing: border-box !important;
+}
+
+.k-datepicker .k-input-inner,
+.k-timepicker .k-input-inner {
+    height: 36px !important;
+    padding: 0 8px !important;
+    line-height: 36px !important;
+}
+
+.k-datepicker .k-input-button,
+.k-timepicker .k-input-button {
     height: 38px !important;
     line-height: 38px !important;
     width: 38px !important;
@@ -315,7 +328,6 @@
     border-radius: 0.35rem;
     margin-bottom: 1rem;
     box-shadow: 0 0.15rem 1.75rem 0 rgba(58, 59, 69, 0.15);
-    overflow: visible !important;
 }
 
 .section-card-header {
@@ -329,65 +341,6 @@
 .section-card-body {
     padding: 1.25rem;
     overflow: visible !important;
-}
-
-.section-card-body .d-flex,
-.section-card-body .d-flex.flex-column,
-.section-card-body .d-flex.align-items-center,
-.section-card-body .d-flex.justify-content-between {
-    overflow: visible !important;
-}
-
-.section-card-body .e-dropdowntree,
-.section-card-body .e-dropdownlist,
-.section-card-body .e-combobox,
-.section-card-body .e-multiselect {
-    position: relative !important;
-    z-index: 1050 !important;
-    overflow: visible !important;
-}
-
-.section-card-body .e-popup,
-.section-card-body .e-ddt-popup {
-    z-index: 1055 !important;
-}
-
-.section-card-body .e-ddt-wrapper,
-.section-card-body .e-input-group.e-control-wrapper {
-    overflow: visible !important;
-    clip-path: none !important;
-}
-
-.section-card-body .form-group {
-    overflow: visible !important;
-    position: relative;
-}
-
-.section-card-body .row {
-    overflow: visible !important;
-}
-
-.section-card-body [class*="col-"] {
-    overflow: visible !important;
-}
-
-.section-card-body .dropdown-wrapper {
-    position: relative !important;
-    overflow: visible !important;
-    z-index: 1;
-}
-
-.btn.disabled,
-.btn:disabled,
-.btn.disabled:hover,
-.btn:disabled:hover,
-.btn.disabled:focus,
-.btn:disabled:focus {
-    pointer-events: auto !important;
-    opacity: 0.5;
-    cursor: not-allowed !important;
-    transform: none !important;
-    box-shadow: none !important;
 }
 
 .field-icon {
@@ -779,35 +732,26 @@
     box-shadow: 0 2px 6px rgba(0, 0, 0, 0.3);
 }
 
-.ftx-calendario-wrapper {
-    position: relative;
-    display: inline-block;
-}
-
 #badgeContadorDatas.badge-contador-datas {
+    width: 35px !important;
+    height: 35px !important;
+    min-width: 35px !important;
+    min-height: 35px !important;
     position: absolute !important;
-    top: -10px;
-    right: -10px;
-    width: 32px !important;
-    height: 32px !important;
-    min-width: 32px !important;
-    min-height: 32px !important;
+    top: 10px;
+    right: 10px;
     border-radius: 50% !important;
     background-color: #FF8C00 !important;
     color: white !important;
     border: 2px solid white !important;
-    display: inline-flex !important;
+    display: flex !important;
     align-items: center !important;
     justify-content: center !important;
-    font-size: 13px !important;
+    font-size: 14px !important;
     font-weight: bold !important;
-    font-family: 'Poppins', Arial, sans-serif;
-    line-height: 20px !important;
     box-shadow: 0 2px 8px rgba(0, 0, 0, 0.3) !important;
-    z-index: 999999 !important;
+    z-index: 1000 !important;
     transition: all 0.3s ease !important;
-    cursor: default;
-    text-align: center;
 }
 
 #badgeContadorDatas:hover {
@@ -1444,138 +1388,3 @@
     text-shadow: 0 2px 4px rgba(0, 0, 0, 0.25) !important;
     letter-spacing: 0.5px !important;
 }
-
-.btn-ficha-vistoria {
-    display: inline-flex;
-    align-items: center;
-    justify-content: center;
-    width: 26px;
-    height: 26px;
-    padding: 0;
-    border: none;
-    border-radius: 50%;
-    background: linear-gradient(135deg, #FF8C00, #FF6B00);
-    color: white;
-    cursor: pointer;
-    transition: all 0.3s ease;
-    box-shadow: 0 2px 6px rgba(255, 140, 0, 0.4);
-}
-
-.btn-ficha-vistoria:hover:not(:disabled) {
-    transform: scale(1.15);
-    box-shadow: 0 4px 12px rgba(255, 140, 0, 0.5);
-}
-
-.btn-ficha-vistoria:disabled {
-    background: linear-gradient(135deg, #ccc, #aaa);
-    cursor: not-allowed;
-    opacity: 0.6;
-    box-shadow: none;
-}
-
-.btn-ficha-vistoria i {
-    font-size: 13px;
-}
-
-#modalFichaVistoria .modal-content {
-    border-radius: 12px;
-    overflow: hidden;
-    box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
-}
-
-.modal-header-ficha {
-    background: linear-gradient(135deg, #FF8C00, #FF6B00);
-    color: white;
-    border-bottom: none;
-    padding: 1rem 1.5rem;
-}
-
-.modal-header-ficha .modal-title {
-    font-family: 'Outfit', sans-serif;
-    font-weight: 700;
-    font-size: 1.25rem;
-    display: flex;
-    align-items: center;
-    gap: 0.5rem;
-}
-
-.modal-header-ficha .btn-close-white {
-    filter: brightness(0) invert(1);
-    opacity: 0.9;
-}
-
-.modal-header-ficha .btn-close-white:hover {
-    opacity: 1;
-}
-
-.ficha-vistoria-container {
-    display: flex;
-    align-items: center;
-    justify-content: center;
-    min-height: 400px;
-    background-color: #f8f9fa;
-    padding: 1rem;
-}
-
-.ficha-vistoria-img {
-    max-width: 100%;
-    max-height: 70vh;
-    object-fit: contain;
-    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
-    border-radius: 4px;
-}
-
-.btn-ficha-imprimir {
-    background: linear-gradient(135deg, #FF8C00, #FF6B00);
-    border: none;
-    color: white;
-    padding: 0.5rem 1.5rem;
-    border-radius: 0.375rem;
-    font-weight: 600;
-    transition: all 0.3s ease;
-}
-
-.btn-ficha-imprimir:hover {
-    background: linear-gradient(135deg, #FF6B00, #E55C00);
-    color: white;
-    transform: translateY(-1px);
-    box-shadow: 0 4px 12px rgba(255, 140, 0, 0.4);
-}
-
-.btn-ficha-imprimir:active {
-    transform: translateY(0);
-}
-
-#modalFichaVistoria + .modal-backdrop {
-    z-index: 1055;
-}
-
-.btn-fechar-ficha {
-    background: linear-gradient(135deg, #FF8C00, #FF6B00);
-    border: none;
-    color: white;
-    padding: 0.5rem 2rem;
-    border-radius: 0.375rem;
-    font-weight: 600;
-    transition: all 0.3s ease;
-    font-family: 'Outfit', sans-serif;
-}
-
-.btn-fechar-ficha:hover {
-    background: linear-gradient(135deg, #FF6B00, #E55C00);
-    color: white;
-    transform: translateY(-1px);
-    box-shadow: 0 4px 12px rgba(255, 140, 0, 0.4);
-}
-
-.btn-fechar-ficha:active {
-    transform: translateY(0);
-}
-
-#divNoFichaVistoria {
-    display: none !important;
-}
-
-#modalFinalizaViagem .card-body {
-    padding: 1.5rem !important;
-}
```
