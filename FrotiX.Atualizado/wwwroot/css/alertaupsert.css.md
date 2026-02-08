# wwwroot/css/alertaupsert.css

**Mudanca:** GRANDE | **+544** linhas | **-645** linhas

---

```diff
--- JANEIRO: wwwroot/css/alertaupsert.css
+++ ATUAL: wwwroot/css/alertaupsert.css
@@ -1,877 +1,766 @@
 :root {
-  --ctrl-h: 40px;
-  --ctrl-pad-y: 0.375rem;
-  --ctrl-pad-x: 0.75rem;
-  --ctrl-radius: 0.25rem;
+    --ctrl-h: 40px;
+    --ctrl-pad-y: .375rem;
+    --ctrl-pad-x: .75rem;
+    --ctrl-radius: .25rem;
 }
 
 .section-legend {
-  display: flex;
-  align-items: center;
-  gap: 0.5rem;
-  margin: 1.5rem 0 1rem 0;
-  font-weight: 700;
-  font-size: 1.05rem;
-  color: #325d88;
-}
-
-.section-legend i {
-  color: #0ea5e9;
-}
-
-.section-legend .legend-note {
-  font-weight: 500;
-  font-size: 0.9rem;
-  color: #6c757d;
-}
+    display: flex;
+    align-items: center;
+    gap: .5rem;
+    margin: 1.5rem 0 1rem 0;
+    font-weight: 700;
+    font-size: 1.05rem;
+    color: #325d88;
+}
+
+    .section-legend i {
+        color: #0ea5e9;
+    }
+
+    .section-legend .legend-note {
+        font-weight: 500;
+        font-size: .9rem;
+        color: #6c757d;
+    }
 
 .label {
-  margin-bottom: 0.25rem;
-  margin-top: 0.25rem;
-  font-weight: 600;
-  color: #374151;
+    margin-bottom: .25rem;
+    margin-top: .25rem;
+    font-weight: 600;
+    color: #374151;
 }
 
 .required-field::after {
-  content: " *";
-  color: #ef4444;
+    content: ' *';
+    color: #ef4444;
 }
 
 .btn-azul {
-  background-color: #0ea5e9;
-  color: white;
-  border: none;
-  transition: all 0.3s ease;
-}
-
-.btn-azul:hover {
-  background-color: #0284c7;
-  transform: translateY(-2px);
-  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
-}
+    background-color: #0ea5e9;
+    color: white;
+    border: none;
+    transition: all 0.3s ease;
+}
+
+    .btn-azul:hover {
+        background-color: #0284c7;
+        transform: translateY(-2px);
+        box-shadow: 0 4px 8px rgba(0,0,0,0.2);
+    }
 
 .btn-cancelar {
-  background-color: #6c757d;
-  color: white;
-  border: none;
-  transition: all 0.3s ease;
-}
-
-.btn-cancelar:hover {
-  background-color: #5a6268;
-}
+    background-color: #6c757d;
+    color: white;
+    border: none;
+    transition: all 0.3s ease;
+}
+
+    .btn-cancelar:hover {
+        background-color: #5a6268;
+    }
 
 .tipo-alerta-card {
-  border: 2px solid #e5e7eb;
-  border-radius: 0.5rem;
-  padding: 1.25rem 1rem;
-  cursor: pointer;
-  transition: all 0.3s ease;
-  text-align: center;
-  background-color: #ffffff;
-  position: relative;
-  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
-  display: flex;
-  flex-direction: column;
-  align-items: center;
-  justify-content: center;
-  gap: 0.5rem;
-}
-
-.tipo-alerta-card[data-tipo="1"]:hover {
-  border-color: #0ea5e9;
-  transform: translateY(-2px);
-}
-
-.tipo-alerta-card[data-tipo="2"]:hover {
-  border-color: #f59e0b;
-  transform: translateY(-2px);
-}
-
-.tipo-alerta-card[data-tipo="3"]:hover {
-  border-color: #14b8a6;
-  transform: translateY(-2px);
-}
-
-.tipo-alerta-card[data-tipo="4"]:hover {
-  border-color: #7c3aed;
-  transform: translateY(-2px);
-}
-
-.tipo-alerta-card[data-tipo="5"]:hover {
-  border-color: #dc2626;
-  transform: translateY(-2px);
-}
-
-.tipo-alerta-card[data-tipo="6"]:hover {
-  border-color: #6c757d;
-  transform: translateY(-2px);
-}
-
-.tipo-alerta-card.selected {
-  border-width: 4px;
-  transform: translateY(-3px);
-}
-
-.tipo-alerta-card[data-tipo="1"].selected {
-  border-color: #0ea5e9;
-  background: linear-gradient(135deg, #f0f9ff 0%, #e0f2fe 100%);
-  box-shadow:
-    0 0 0 4px rgba(14, 165, 233, 0.15),
-    0 6px 16px rgba(14, 165, 233, 0.3);
-}
-
-.tipo-alerta-card[data-tipo="2"].selected {
-  border-color: #f59e0b;
-  background: linear-gradient(135deg, #fffbeb 0%, #fef3c7 100%);
-  box-shadow:
-    0 0 0 4px rgba(245, 158, 11, 0.15),
-    0 6px 16px rgba(245, 158, 11, 0.3);
-}
-
-.tipo-alerta-card[data-tipo="3"].selected {
-  border-color: #14b8a6;
-  background: linear-gradient(135deg, #f0fdfa 0%, #ccfbf1 100%);
-  box-shadow:
-    0 0 0 4px rgba(20, 184, 166, 0.15),
-    0 6px 16px rgba(20, 184, 166, 0.3);
-}
-
-.tipo-alerta-card[data-tipo="4"].selected {
-  border-color: #7c3aed;
-  background: linear-gradient(135deg, #faf5ff 0%, #f3e8ff 100%);
-  box-shadow:
-    0 0 0 4px rgba(124, 58, 237, 0.15),
-    0 6px 16px rgba(124, 58, 237, 0.3);
-}
-
-.tipo-alerta-card[data-tipo="5"].selected {
-  border-color: #dc2626;
-  background: linear-gradient(135deg, #fef2f2 0%, #fee2e2 100%);
-  box-shadow:
-    0 0 0 4px rgba(220, 38, 38, 0.15),
-    0 6px 16px rgba(220, 38, 38, 0.3);
-}
-
-.tipo-alerta-card[data-tipo="6"].selected {
-  border-color: #6c757d;
-  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
-  box-shadow:
-    0 0 0 4px rgba(108, 117, 125, 0.15),
-    0 6px 16px rgba(108, 117, 125, 0.3);
-}
-
-.tipo-alerta-card.selected::before {
-  content: "✓";
-  font-family: Arial, sans-serif;
-  font-weight: bold;
-  position: absolute;
-  top: 8px;
-  right: 8px;
-  font-size: 1.4rem;
-  animation: checkAnimation 0.3s ease;
-  z-index: 10;
-  line-height: 1;
-}
-
-.tipo-alerta-card[data-tipo="1"].selected::before {
-  color: #0ea5e9;
-}
-
-.tipo-alerta-card[data-tipo="2"].selected::before {
-  color: #f59e0b;
-}
-
-.tipo-alerta-card[data-tipo="3"].selected::before {
-  color: #14b8a6;
-}
-
-.tipo-alerta-card[data-tipo="4"].selected::before {
-  color: #7c3aed;
-}
-
-.tipo-alerta-card[data-tipo="5"].selected::before {
-  color: #dc2626;
-}
-
-.tipo-alerta-card[data-tipo="6"].selected::before {
-  color: #6c757d;
-}
+    border: 2px solid #e5e7eb;
+    border-radius: 0.5rem;
+    padding: 1.25rem 1rem;
+    cursor: pointer;
+    transition: all 0.3s ease;
+    text-align: center;
+    background-color: #ffffff;
+    position: relative;
+    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
+    display: flex;
+    flex-direction: column;
+    align-items: center;
+    justify-content: center;
+    gap: 0.5rem;
+}
+
+    .tipo-alerta-card[data-tipo="1"]:hover {
+        border-color: #0ea5e9;
+        transform: translateY(-2px);
+    }
+
+    .tipo-alerta-card[data-tipo="2"]:hover {
+        border-color: #f59e0b;
+        transform: translateY(-2px);
+    }
+
+    .tipo-alerta-card[data-tipo="3"]:hover {
+        border-color: #14b8a6;
+        transform: translateY(-2px);
+    }
+
+    .tipo-alerta-card[data-tipo="4"]:hover {
+        border-color: #7c3aed;
+        transform: translateY(-2px);
+    }
+
+    .tipo-alerta-card[data-tipo="5"]:hover {
+        border-color: #dc2626;
+        transform: translateY(-2px);
+    }
+
+    .tipo-alerta-card[data-tipo="6"]:hover {
+        border-color: #6c757d;
+        transform: translateY(-2px);
+    }
+
+    .tipo-alerta-card.selected {
+        border-width: 4px;
+        transform: translateY(-3px);
+    }
+
+    .tipo-alerta-card[data-tipo="1"].selected {
+        border-color: #0ea5e9;
+        background: linear-gradient(135deg, #f0f9ff 0%, #e0f2fe 100%);
+        box-shadow: 0 0 0 4px rgba(14, 165, 233, 0.15), 0 6px 16px rgba(14, 165, 233, 0.3);
+    }
+
+    .tipo-alerta-card[data-tipo="2"].selected {
+        border-color: #f59e0b;
+        background: linear-gradient(135deg, #fffbeb 0%, #fef3c7 100%);
+        box-shadow: 0 0 0 4px rgba(245, 158, 11, 0.15), 0 6px 16px rgba(245, 158, 11, 0.3);
+    }
+
+    .tipo-alerta-card[data-tipo="3"].selected {
+        border-color: #14b8a6;
+        background: linear-gradient(135deg, #f0fdfa 0%, #ccfbf1 100%);
+        box-shadow: 0 0 0 4px rgba(20, 184, 166, 0.15), 0 6px 16px rgba(20, 184, 166, 0.3);
+    }
+
+    .tipo-alerta-card[data-tipo="4"].selected {
+        border-color: #7c3aed;
+        background: linear-gradient(135deg, #faf5ff 0%, #f3e8ff 100%);
+        box-shadow: 0 0 0 4px rgba(124, 58, 237, 0.15), 0 6px 16px rgba(124, 58, 237, 0.3);
+    }
+
+    .tipo-alerta-card[data-tipo="5"].selected {
+        border-color: #dc2626;
+        background: linear-gradient(135deg, #fef2f2 0%, #fee2e2 100%);
+        box-shadow: 0 0 0 4px rgba(220, 38, 38, 0.15), 0 6px 16px rgba(220, 38, 38, 0.3);
+    }
+
+    .tipo-alerta-card[data-tipo="6"].selected {
+        border-color: #6c757d;
+        background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
+        box-shadow: 0 0 0 4px rgba(108, 117, 125, 0.15), 0 6px 16px rgba(108, 117, 125, 0.3);
+    }
+
+    .tipo-alerta-card.selected::before {
+        content: '✓';
+        font-family: Arial, sans-serif;
+        font-weight: bold;
+        position: absolute;
+        top: 8px;
+        right: 8px;
+        font-size: 1.4rem;
+        animation: checkAnimation 0.3s ease;
+        z-index: 10;
+        line-height: 1;
+    }
+
+    .tipo-alerta-card[data-tipo="1"].selected::before {
+        color: #0ea5e9;
+    }
+
+    .tipo-alerta-card[data-tipo="2"].selected::before {
+        color: #f59e0b;
+    }
+
+    .tipo-alerta-card[data-tipo="3"].selected::before {
+        color: #14b8a6;
+    }
+
+    .tipo-alerta-card[data-tipo="4"].selected::before {
+        color: #7c3aed;
+    }
+
+    .tipo-alerta-card[data-tipo="5"].selected::before {
+        color: #dc2626;
+    }
+
+    .tipo-alerta-card[data-tipo="6"].selected::before {
+        color: #6c757d;
+    }
 
 @keyframes checkAnimation {
-  0% {
-    transform: scale(0) rotate(-180deg);
-    opacity: 0;
-  }
-
-  50% {
-    transform: scale(1.2) rotate(0deg);
-  }
-
-  100% {
-    transform: scale(1) rotate(0deg);
-    opacity: 1;
-  }
+    0% {
+        transform: scale(0) rotate(-180deg);
+        opacity: 0;
+    }
+
+    50% {
+        transform: scale(1.2) rotate(0deg);
+    }
+
+    100% {
+        transform: scale(1) rotate(0deg);
+        opacity: 1;
+    }
 }
 
 .tipo-alerta-card i {
-  font-size: 2.5rem;
-  margin-bottom: 0.75rem;
-  display: block;
-  transition: all 0.3s ease;
+    font-size: 2.5rem;
+    margin-bottom: 0.75rem;
+    display: block;
+    transition: all 0.3s ease;
 }
 
 .tipo-alerta-card.selected i {
-  font-size: 2.7rem;
-  transform: scale(1.1);
+    font-size: 2.7rem;
+    transform: scale(1.1);
 }
 
 .tipo-alerta-card > div:not(.preview-badge) {
-  display: none;
+    display: none;
 }
 
 .preview-badge {
-  display: block;
-  padding: 0.35rem 0.85rem;
-  border-radius: 0.3rem;
-  font-size: 0.875rem;
-  font-weight: 600;
-  color: white;
-  transition: all 0.3s ease;
-  margin: 0 auto;
-  width: fit-content;
+    display: block;
+    padding: 0.35rem 0.85rem;
+    border-radius: 0.3rem;
+    font-size: 0.875rem;
+    font-weight: 600;
+    color: white;
+    transition: all 0.3s ease;
+    margin: 0 auto;
+    width: fit-content;
 }
 
 .tipo-alerta-card.selected .preview-badge {
-  transform: scale(1.05);
-  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
+    transform: scale(1.05);
+    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
 }
 
 @media (max-width: 768px) {
-  .tipo-alerta-card {
-    padding: 0.75rem;
-  }
-
-  .tipo-alerta-card i {
-    font-size: 1.5rem;
-  }
-
-  .tipo-alerta-card.selected i {
-    font-size: 1.7rem;
-  }
-
-  .tipo-alerta-card.selected::before {
-    font-size: 1rem;
-    top: 6px;
-    right: 6px;
-  }
-}
-
-.user-item-multiselect {
-  display: flex;
-  align-items: center;
-  padding: 6px 4px;
-  border-radius: 6px;
-}
-
-.user-icon-circle {
-  width: 36px;
-  height: 36px;
-  background: linear-gradient(135deg, #325d88 0%, #264666 100%);
-  color: white;
-  border-radius: 50%;
-  display: flex;
-  align-items: center;
-  justify-content: center;
-  font-weight: 700;
-  margin-right: 12px;
-  font-size: 15px;
-  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
-  flex-shrink: 0;
-}
-
-.user-info-text {
-  display: flex;
-  flex-direction: column;
-  overflow: hidden;
-}
-
-.user-name-full {
-  font-weight: 600;
-  color: #2c3e50;
-  line-height: 1.2;
-  display: block;
-  font-size: 14px;
-  white-space: nowrap;
-  overflow: hidden;
-  text-overflow: ellipsis;
-}
-
-.user-ponto-text {
-  font-size: 11px;
-  color: #7f8c8d;
-  font-weight: 500;
-}
-
-.user-value-multiselect {
-  display: inline-flex;
-  align-items: center;
-}
-
-.user-name-selected {
-  font-weight: 600;
+    .tipo-alerta-card {
+        padding: 0.75rem;
+    }
+
+        .tipo-alerta-card i {
+            font-size: 1.5rem;
+        }
+
+        .tipo-alerta-card.selected i {
+            font-size: 1.7rem;
+        }
+
+        .tipo-alerta-card.selected::before {
+            font-size: 1rem;
+            top: 6px;
+            right: 6px;
+        }
 }
 
 .motorista-item-alerta {
-  display: flex;
-  align-items: center;
-  gap: 10px;
-  padding: 6px 8px;
-  transition: background-color 0.2s ease;
-}
-
-.motorista-item-alerta:hover {
-  background-color: #f0f8ff;
-}
+    display: flex;
+    align-items: center;
+    gap: 10px;
+    padding: 6px 8px;
+    transition: background-color 0.2s ease;
+}
+
+    .motorista-item-alerta:hover {
+        background-color: #f0f8ff;
+    }
 
 .motorista-foto-alerta-item {
-  width: 40px;
-  height: 40px;
-  border-radius: 50%;
-  object-fit: cover;
-  border: 2px solid #e0e0e0;
-  transition:
-    transform 0.3s ease,
-    border-color 0.3s ease;
-  flex-shrink: 0;
+    width: 40px;
+    height: 40px;
+    border-radius: 50%;
+    object-fit: cover;
+    border: 2px solid #e0e0e0;
+    transition: transform 0.3s ease, border-color 0.3s ease;
+    flex-shrink: 0;
 }
 
 .motorista-item-alerta:hover .motorista-foto-alerta-item {
-  transform: scale(1.1);
-  border-color: #14b8a6;
+    transform: scale(1.1);
+    border-color: #14b8a6;
 }
 
 .motorista-nome-alerta {
-  flex: 1;
-  overflow: hidden;
-  text-overflow: ellipsis;
-  white-space: nowrap;
+    flex: 1;
+    overflow: hidden;
+    text-overflow: ellipsis;
+    white-space: nowrap;
 }
 
 .motorista-selected-alerta {
-  display: flex;
-  align-items: center;
-  gap: 8px;
-  padding: 2px 0;
+    display: flex;
+    align-items: center;
+    gap: 8px;
+    padding: 2px 0;
 }
 
 .motorista-foto-alerta-selected {
-  width: 30px;
-  height: 30px;
-  border-radius: 50%;
-  object-fit: cover;
-  border: 2px solid #e0e0e0;
-  flex-shrink: 0;
+    width: 30px;
+    height: 30px;
+    border-radius: 50%;
+    object-fit: cover;
+    border: 2px solid #e0e0e0;
+    flex-shrink: 0;
 }
 
 #MotoristaId_popup .e-list-item {
-  padding: 0 !important;
+    padding: 0 !important;
 }
 
 #MotoristaId_popup .e-dropdownbase .e-list-item {
-  min-height: 52px;
+    min-height: 52px;
 }
 
 @media (max-width: 768px) {
-  .motorista-foto-alerta-item {
-    width: 35px;
-    height: 35px;
-  }
-
-  .motorista-foto-alerta-selected {
-    width: 28px;
-    height: 28px;
-  }
+    .motorista-foto-alerta-item {
+        width: 35px;
+        height: 35px;
+    }
+
+    .motorista-foto-alerta-selected {
+        width: 28px;
+        height: 28px;
+    }
 }
 
 .agendamento-card-item {
-  padding: 12px;
-  border-bottom: 1px solid #e5e7eb;
-  transition: all 0.2s ease;
-  background: white;
-}
-
-.agendamento-card-item:hover {
-  background: #f8fafc;
-  border-left: 3px solid #0ea5e9;
-  padding-left: 9px;
-}
+    padding: 12px;
+    border-bottom: 1px solid #e5e7eb;
+    transition: all 0.2s ease;
+    background: white;
+}
+
+    .agendamento-card-item:hover {
+        background: #f8fafc;
+        border-left: 3px solid #0ea5e9;
+        padding-left: 9px;
+    }
 
 .agendamento-card-header {
-  display: flex;
-  justify-content: space-between;
-  align-items: center;
-  margin-bottom: 8px;
+    display: flex;
+    justify-content: space-between;
+    align-items: center;
+    margin-bottom: 8px;
 }
 
 .agendamento-card-title {
-  display: flex;
-  align-items: center;
-  gap: 6px;
-  font-size: 14px;
-  color: #1e293b;
-}
-
-.agendamento-card-title i {
-  color: #0ea5e9;
-  font-size: 16px;
-}
+    display: flex;
+    align-items: center;
+    gap: 6px;
+    font-size: 14px;
+    color: #1e293b;
+}
+
+    .agendamento-card-title i {
+        color: #0ea5e9;
+        font-size: 16px;
+    }
 
 .agendamento-hora {
-  color: #64748b;
-  font-size: 13px;
+    color: #64748b;
+    font-size: 13px;
 }
 
 .agendamento-badge {
-  padding: 3px 10px;
-  background: #e0f2fe;
-  color: #0369a1;
-  border-radius: 12px;
-  font-size: 11px;
-  font-weight: 600;
-  text-transform: uppercase;
+    padding: 3px 10px;
+    background: #e0f2fe;
+    color: #0369a1;
+    border-radius: 12px;
+    font-size: 11px;
+    font-weight: 600;
+    text-transform: uppercase;
 }
 
 .agendamento-card-body {
-  display: flex;
-  flex-direction: column;
-  gap: 8px;
+    display: flex;
+    flex-direction: column;
+    gap: 8px;
 }
 
 .agendamento-rota {
-  display: flex;
-  align-items: center;
-  gap: 8px;
-  font-size: 13px;
-  padding: 6px 0;
+    display: flex;
+    align-items: center;
+    gap: 8px;
+    font-size: 13px;
+    padding: 6px 0;
 }
 
 .agendamento-origem,
 .agendamento-destino {
-  display: flex;
-  align-items: center;
-  gap: 4px;
-  color: #475569;
-}
-
-.agendamento-origem i {
-  color: #10b981;
-}
-
-.agendamento-destino i {
-  color: #ef4444;
-}
+    display: flex;
+    align-items: center;
+    gap: 4px;
+    color: #475569;
+}
+
+    .agendamento-origem i {
+        color: #10b981;
+    }
+
+    .agendamento-destino i {
+        color: #ef4444;
+    }
 
 .agendamento-seta {
-  color: #94a3b8;
-  font-size: 12px;
+    color: #94a3b8;
+    font-size: 12px;
 }
 
 .agendamento-requisitante {
-  display: flex;
-  align-items: center;
-  gap: 6px;
-  font-size: 12px;
-  color: #64748b;
-  padding-top: 4px;
-  border-top: 1px solid #f1f5f9;
-}
-
-.agendamento-requisitante i {
-  color: #8b5cf6;
-}
+    display: flex;
+    align-items: center;
+    gap: 6px;
+    font-size: 12px;
+    color: #64748b;
+    padding-top: 4px;
+    border-top: 1px solid #f1f5f9;
+}
+
+    .agendamento-requisitante i {
+        color: #8b5cf6;
+    }
 
 .agendamento-selected {
-  display: flex;
-  align-items: center;
-  gap: 8px;
-  padding: 2px 0;
-}
-
-.agendamento-selected i {
-  color: #0ea5e9;
-  font-size: 16px;
-}
+    display: flex;
+    align-items: center;
+    gap: 8px;
+    padding: 2px 0;
+}
+
+    .agendamento-selected i {
+        color: #0ea5e9;
+        font-size: 16px;
+    }
 
 .agendamento-selected-text {
-  font-size: 14px;
-  color: #1e293b;
+    font-size: 14px;
+    color: #1e293b;
 }
 
 #ViagemId_popup {
-  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
-  border-radius: 8px;
-  overflow: hidden;
-}
-
-#ViagemId_popup .e-list-item {
-  padding: 0 !important;
-}
-
-#ViagemId_popup .e-dropdownbase .e-list-item {
-  min-height: auto;
-}
-
-#ViagemId_popup .e-content {
-  scrollbar-width: thin;
-  scrollbar-color: #cbd5e1 #f1f5f9;
-}
-
-#ViagemId_popup .e-content::-webkit-scrollbar {
-  width: 8px;
-}
-
-#ViagemId_popup .e-content::-webkit-scrollbar-track {
-  background: #f1f5f9;
-}
-
-#ViagemId_popup .e-content::-webkit-scrollbar-thumb {
-  background: #cbd5e1;
-  border-radius: 4px;
-}
-
-#ViagemId_popup .e-content::-webkit-scrollbar-thumb:hover {
-  background: #94a3b8;
-}
+    box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
+    border-radius: 8px;
+    overflow: hidden;
+}
+
+    #ViagemId_popup .e-list-item {
+        padding: 0 !important;
+    }
+
+    #ViagemId_popup .e-dropdownbase .e-list-item {
+        min-height: auto;
+    }
+
+    #ViagemId_popup .e-content {
+        scrollbar-width: thin;
+        scrollbar-color: #cbd5e1 #f1f5f9;
+    }
+
+        #ViagemId_popup .e-content::-webkit-scrollbar {
+            width: 8px;
+        }
+
+        #ViagemId_popup .e-content::-webkit-scrollbar-track {
+            background: #f1f5f9;
+        }
+
+        #ViagemId_popup .e-content::-webkit-scrollbar-thumb {
+            background: #cbd5e1;
+            border-radius: 4px;
+        }
+
+            #ViagemId_popup .e-content::-webkit-scrollbar-thumb:hover {
+                background: #94a3b8;
+            }
 
 @media (max-width: 768px) {
-  .agendamento-rota {
-    flex-direction: column;
-    align-items: flex-start;
-    gap: 4px;
-  }
-
-  .agendamento-seta {
-    display: none;
-  }
+    .agendamento-rota {
+        flex-direction: column;
+        align-items: flex-start;
+        gap: 4px;
+    }
+
+    .agendamento-seta {
+        display: none;
+    }
 }
 
 #ViagemId_popup .agendamento-card-item i {
-  display: inline-flex;
-  align-items: center;
-  flex: 0 0 auto;
-  line-height: 1;
-  vertical-align: middle;
+    display: inline-flex;
+    align-items: center;
+    flex: 0 0 auto;
+    line-height: 1;
+    vertical-align: middle;
 }
 
 .agendamento-rota {
-  flex-wrap: wrap;
-  row-gap: 4px;
+    flex-wrap: wrap;
+    row-gap: 4px;
 }
 
 .agendamento-origem,
 .agendamento-destino {
-  display: inline-flex;
-  align-items: center;
-  gap: 4px;
-  min-width: 0;
-  max-width: calc(50% - 12px);
-  white-space: nowrap;
-  overflow: hidden;
-  text-overflow: ellipsis;
+    display: inline-flex;
+    align-items: center;
+    gap: 4px;
+    min-width: 0;
+    max-width: calc(50% - 12px);
+    white-space: nowrap;
+    overflow: hidden;
+    text-overflow: ellipsis;
 }
 
 .agendamento-seta {
-  flex: 0 0 auto;
+    flex: 0 0 auto;
 }
 
 #ViagemId_popup .e-dropdownbase .e-list-item {
-  min-height: auto;
+    min-height: auto;
 }
 
 #ViagemId_popup .e-list-item:nth-child(odd) .agendamento-card-item {
-  background: #ffffff;
+    background: #ffffff;
 }
 
 #ViagemId_popup .e-list-item:nth-child(even) .agendamento-card-item {
-  background: #fffaf2;
+    background: #fffaf2;
 }
 
 .agendamento-hora {
-  display: inline-flex;
-  align-items: center;
-  gap: 6px;
-  font-weight: 700;
-}
-
-.agendamento-hora i {
-  color: #0ea5e9;
-  font-size: 16px;
-  line-height: 1;
-}
+    display: inline-flex;
+    align-items: center;
+    gap: 6px;
+    font-weight: 700;
+}
+
+    .agendamento-hora i {
+        color: #0ea5e9;
+        font-size: 16px;
+        line-height: 1;
+    }
 
 #ManutencaoId_popup {
-  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
-  border-radius: 8px;
-  overflow: hidden;
+    box-shadow: 0 10px 25px rgba(0,0,0,.15);
+    border-radius: 8px;
+    overflow: hidden;
+}
+
+    #ManutencaoId_popup .e-list-item {
+        padding: 0 !important;
+    }
+
+    #ManutencaoId_popup .e-dropdownbase .e-list-item {
+        min-height: auto;
+    }
+
+    #ManutencaoId_popup .e-list-item:nth-child(odd) .manutencao-card-item {
+        background: #ffffff;
+    }
+
+    #ManutencaoId_popup .e-list-item:nth-child(even) .manutencao-card-item {
+        background: #fffaf2;
+    }
+
+.manutencao-card-item {
+    padding: 12px;
+    border-bottom: 1px solid #e5e7eb;
+    transition: .2s;
+    background: white;
+}
+
+    .manutencao-card-item:hover {
+        background: #fffbeb;
+        border-left: 3px solid #f59e0b;
+        padding-left: 9px;
+    }
+
+.manutencao-card-header {
+    display: flex;
+    justify-content: space-between;
+    align-items: center;
+    margin-bottom: 6px;
+}
+
+.manutencao-card-title {
+    display: flex;
+    align-items: center;
+    gap: 6px;
+    font-size: 14px;
+    color: #1e293b;
+}
+
+    .manutencao-card-title i {
+        color: #f59e0b;
+        font-size: 16px;
+    }
+
+.manutencao-card-body {
+    display: flex;
+    flex-direction: column;
+    gap: 6px;
+}
+
+.manutencao-linha {
+    display: flex;
+    align-items: center;
+    gap: 12px;
+    font-size: 13px;
+    padding: 4px 0;
+    flex-wrap: wrap;
+}
+
+.manutencao-dado {
+    display: inline-flex;
+    align-items: center;
+    gap: 6px;
+    color: #475569;
+    min-width: 0;
+    white-space: nowrap;
+    overflow: hidden;
+    text-overflow: ellipsis;
+}
+
+    .manutencao-dado i {
+        display: inline-flex;
+        align-items: center;
+        line-height: 1;
+        flex: 0 0 auto;
+    }
+
+.manutencao-selected {
+    display: flex;
+    align-items: center;
+    gap: 8px;
+    padding: 2px 0;
+}
+
+    .manutencao-selected i {
+        color: #f59e0b;
+        font-size: 16px;
+    }
+
+.manutencao-selected-text {
+    font-size: 14px;
+    color: #1e293b;
+}
+
+@media (max-width:768px) {
+    .manutencao-linha {
+        gap: 8px;
+    }
 }
 
 #ManutencaoId_popup .e-list-item {
-  padding: 0 !important;
+    padding: 0 !important;
 }
 
 #ManutencaoId_popup .e-dropdownbase .e-list-item {
-  min-height: auto;
-}
-
-#ManutencaoId_popup .e-list-item:nth-child(odd) .manutencao-card-item {
-  background: #ffffff;
+    min-height: auto;
+}
+
+#ManutencaoId_popup .manutencao-card-item i {
+    display: inline-flex;
+    align-items: center;
+    flex: 0 0 auto;
+    line-height: 1;
+}
+
+.manutencao-card-item {
+    padding: 12px;
+    border-bottom: 1px solid #e5e7eb;
+    background: #fff;
+}
+
+.manutencao-card-header {
+    display: flex;
+    align-items: center;
+    justify-content: space-between;
+    margin-bottom: 6px;
+}
+
+.manutencao-card-title {
+    display: flex;
+    align-items: center;
+    gap: 8px;
+    color: #1e293b;
+}
+
+    .manutencao-card-title i {
+        color: #f59e0b;
+        font-size: 16px;
+    }
+
+.manutencao-card-body {
+    display: flex;
+    flex-direction: column;
+    gap: 6px;
+}
+
+.manutencao-linha {
+    display: flex;
+    flex-wrap: wrap;
+    gap: 12px;
+}
+
+.manutencao-dado {
+    display: inline-flex;
+    align-items: center;
+    gap: 6px;
+    color: #475569;
+    min-width: 0;
+    white-space: nowrap;
+    overflow: hidden;
+    text-overflow: ellipsis;
 }
 
 #ManutencaoId_popup .e-list-item:nth-child(even) .manutencao-card-item {
-  background: #fffaf2;
-}
-
-.manutencao-card-item {
-  padding: 12px;
-  border-bottom: 1px solid #e5e7eb;
-  transition: 0.2s;
-  background: white;
-}
-
-.manutencao-card-item:hover {
-  background: #fffbeb;
-  border-left: 3px solid #f59e0b;
-  padding-left: 9px;
-}
-
-.manutencao-card-header {
-  display: flex;
-  justify-content: space-between;
-  align-items: center;
-  margin-bottom: 6px;
-}
-
-.manutencao-card-title {
-  display: flex;
-  align-items: center;
-  gap: 6px;
-  font-size: 14px;
-  color: #1e293b;
-}
-
-.manutencao-card-title i {
-  color: #f59e0b;
-  font-size: 16px;
-}
-
-.manutencao-card-body {
-  display: flex;
-  flex-direction: column;
-  gap: 6px;
-}
-
-.manutencao-linha {
-  display: flex;
-  align-items: center;
-  gap: 12px;
-  font-size: 13px;
-  padding: 4px 0;
-  flex-wrap: wrap;
+    background: #fffaf2;
+}
+
+.manutencao-selected {
+    display: flex;
+    align-items: center;
+    gap: 8px;
+}
+
+    .manutencao-selected i {
+        color: #f59e0b;
+    }
+
+.manutencao-selected-text {
+    color: #1e293b;
 }
 
 .manutencao-dado {
-  display: inline-flex;
-  align-items: center;
-  gap: 6px;
-  color: #475569;
-  min-width: 0;
-  white-space: nowrap;
-  overflow: hidden;
-  text-overflow: ellipsis;
-}
-
-.manutencao-dado i {
-  display: inline-flex;
-  align-items: center;
-  line-height: 1;
-  flex: 0 0 auto;
-}
-
-.manutencao-selected {
-  display: flex;
-  align-items: center;
-  gap: 8px;
-  padding: 2px 0;
-}
-
-.manutencao-selected i {
-  color: #f59e0b;
-  font-size: 16px;
-}
-
-.manutencao-selected-text {
-  font-size: 14px;
-  color: #1e293b;
+    display: inline-flex;
+    align-items: center;
+    gap: 6px;
+}
+
+    .manutencao-dado i {
+        line-height: 1;
+        flex: 0 0 auto;
+    }
+
+.manutencao-legenda {
+    font-weight: 600;
+    color: #334155;
+}
+
+.manutencao-valor {
+    color: #475569;
 }
 
 @media (max-width: 768px) {
-  .manutencao-linha {
-    gap: 8px;
-  }
-}
-
-#ManutencaoId_popup .e-list-item {
-  padding: 0 !important;
-}
-
-#ManutencaoId_popup .e-dropdownbase .e-list-item {
-  min-height: auto;
-}
-
-#ManutencaoId_popup .manutencao-card-item i {
-  display: inline-flex;
-  align-items: center;
-  flex: 0 0 auto;
-  line-height: 1;
-}
-
-.manutencao-card-item {
-  padding: 12px;
-  border-bottom: 1px solid #e5e7eb;
-  background: #fff;
-}
-
-.manutencao-card-header {
-  display: flex;
-  align-items: center;
-  justify-content: space-between;
-  margin-bottom: 6px;
-}
-
-.manutencao-card-title {
-  display: flex;
-  align-items: center;
-  gap: 8px;
-  color: #1e293b;
-}
-
-.manutencao-card-title i {
-  color: #f59e0b;
-  font-size: 16px;
-}
-
-.manutencao-card-body {
-  display: flex;
-  flex-direction: column;
-  gap: 6px;
-}
-
-.manutencao-linha {
-  display: flex;
-  flex-wrap: wrap;
-  gap: 12px;
-}
-
-.manutencao-dado {
-  display: inline-flex;
-  align-items: center;
-  gap: 6px;
-  color: #475569;
-  min-width: 0;
-  white-space: nowrap;
-  overflow: hidden;
-  text-overflow: ellipsis;
-}
-
-#ManutencaoId_popup .e-list-item:nth-child(even) .manutencao-card-item {
-  background: #fffaf2;
-}
-
-.manutencao-selected {
-  display: flex;
-  align-items: center;
-  gap: 8px;
-}
-
-.manutencao-selected i {
-  color: #f59e0b;
-}
-
-.manutencao-selected-text {
-  color: #1e293b;
-}
-
-.manutencao-dado {
-  display: inline-flex;
-  align-items: center;
-  gap: 6px;
-}
-
-.manutencao-dado i {
-  line-height: 1;
-  flex: 0 0 auto;
-}
-
-.manutencao-legenda {
-  font-weight: 600;
-  color: #334155;
-}
-
-.manutencao-valor {
-  color: #475569;
-}
-
-@media (max-width: 768px) {
-  .manutencao-dado {
-    gap: 4px;
-  }
+    .manutencao-dado {
+        gap: 4px;
+    }
 }
 
 .section-legend .legend-note::before,
 .section-legend .legend-note::after {
-  content: none !important;
-  display: none !important;
-}
-
-#HorarioExibicao .e-input-group,
-#HorarioExibicao.e-timepicker .e-input-group,
-#HorarioExibicao.e-timepicker .e-input {
-  border-top: 1px solid #d1d5db;
-  border-bottom: 1px solid #d1d5db;
-  border-radius: 0;
-  background-color: transparent;
-}
-
-#calendarContainerAlerta {
-  overflow: visible;
-}
-#calendarContainerAlerta .e-calendar {
-  width: 120%;
-  max-width: none;
-  min-width: 320px;
-}
-#calendarContainerAlerta .e-calendar .e-header {
-  display: flex;
-  flex-wrap: wrap;
-  gap: 0.75rem;
-  align-items: center;
-  justify-content: space-between;
-}
-#calendarContainerAlerta .e-calendar .e-header .e-title {
-  flex: 1;
-}
-
-.usuarios-item-template,
-.usuarios-value-template {
-  display: flex;
-  align-items: center;
-  gap: 0.4rem;
-  font-weight: 600;
-  font-size: 0.95rem;
-}
-.usuarios-dot {
-  color: #ff6b35;
-  font-size: 1.1rem;
-  line-height: 0;
-}
+    content: none !important;
+    display: none !important;
+}
```
