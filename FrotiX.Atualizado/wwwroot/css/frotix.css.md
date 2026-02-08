# wwwroot/css/frotix.css

**Mudanca:** GRANDE | **+448** linhas | **-386** linhas

---

```diff
--- JANEIRO: wwwroot/css/frotix.css
+++ ATUAL: wwwroot/css/frotix.css
@@ -514,8 +514,7 @@
   gap: 0.5rem;
   transition: all 0.3s ease;
   text-decoration: none;
-  box-shadow:
-    0 0 0 2px rgba(255, 255, 255, 0.8),
+  box-shadow: 0 0 0 2px rgba(255, 255, 255, 0.8),
      0 0 12px rgba(160, 82, 45, 0.4) !important;
   position: relative;
   overflow: visible !important;
@@ -527,8 +526,7 @@
   background-color: #8b4513 !important;
   color: #fff !important;
   transform: translateY(-2px);
-  box-shadow:
-    0 0 0 2px rgba(255, 255, 255, 1),
+  box-shadow: 0 0 0 2px rgba(255, 255, 255, 1),
      0 0 20px rgba(160, 82, 45, 0.6) !important;
 }
 
@@ -917,11 +915,8 @@
   margin: 12px !important;
   border: 0 !important;
   border-radius: 12px !important;
-  box-shadow:
-    0 8px 24px rgba(0, 0, 0, 0.15),
-    0 2px 8px rgba(0, 0, 0, 0.1) !important;
-  font-family:
-    -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif !important;
+  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15), 0 2px 8px rgba(0, 0, 0, 0.1) !important;
+  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif !important;
   color: #fff !important;
   overflow: hidden !important;
   padding: 16px !important;
@@ -996,9 +991,7 @@
 }
 
 .e-toast.app-toast:hover {
-  box-shadow:
-    0 12px 32px rgba(0, 0, 0, 0.2),
-    0 4px 12px rgba(0, 0, 0, 0.15) !important;
+  box-shadow: 0 12px 32px rgba(0, 0, 0, 0.2), 0 4px 12px rgba(0, 0, 0, 0.15) !important;
 }
 
 @media (prefers-reduced-motion: reduce) {
@@ -1078,31 +1071,6 @@
   text-shadow: 0 2px 4px rgba(47, 79, 79, 0.25);
 }
 
-.ftx-header-user {
-  gap: 0.5rem;
-  font-family: "Outfit", sans-serif;
-  font-weight: 600;
-  color: #f8fafc;
-}
-
-.ftx-header-user-label {
-  color: #f8fafc;
-  font-size: 0.92rem;
-  letter-spacing: 0.2px;
-  max-width: 240px;
-  overflow: hidden;
-  text-overflow: ellipsis;
-  white-space: nowrap;
-}
-
-.ftx-header-user i.fa-duotone {
-  --fa-primary-color: #ff6b35;
-  --fa-secondary-color: #6c757d;
-  --fa-secondary-opacity: 0.9;
-  font-size: 1.05rem;
-  line-height: 1;
-}
-
 .ftx-avatar {
   --avatar-scale: 1.2;
   position: relative;
@@ -1125,9 +1093,7 @@
   font-size: 18px;
   line-height: 1;
   transform: scale(1);
-  transition:
-    transform 140ms ease-out,
-    opacity 0.15s ease-in-out;
+  transition: transform 140ms ease-out, opacity 0.15s ease-in-out;
 }
 
 .ftx-avatar-img {
@@ -1138,9 +1104,7 @@
   object-fit: cover;
   opacity: 0;
   visibility: hidden;
-  transition:
-    transform 140ms ease-out,
-    opacity 0.15s ease-in-out;
+  transition: transform 140ms ease-out, opacity 0.15s ease-in-out;
   transform: scale(1);
 }
 
@@ -1194,9 +1158,7 @@
 .btn-marrom,
 .btn-amarelo {
   border-radius: 0.375rem !important;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
   border: none;
   position: relative;
 }
@@ -1211,9 +1173,7 @@
 .btn.btn-ftx-fechar:not(.btn-icon-28):not(.btn-xs):not(.ftx-actions *),
 .btn.btn-editar:not(.btn-icon-28):not(.btn-xs):not(.ftx-actions *),
 .btn.btn-marrom:not(.btn-icon-28):not(.btn-xs):not(.ftx-actions *) {
-  box-shadow:
-    0 0 0 1px #000,
-    0 0 0 3px #bbb !important;
+  box-shadow: 0 0 0 1px #000, 0 0 0 3px #bbb !important;
 }
 
 .btn.btn-azul:not(.btn-icon-28):not(.btn-xs):not(.ftx-actions *):hover,
@@ -1226,10 +1186,7 @@
 .btn.btn-ftx-fechar:not(.btn-icon-28):not(.btn-xs):not(.ftx-actions *):hover,
 .btn.btn-editar:not(.btn-icon-28):not(.btn-xs):not(.ftx-actions *):hover,
 .btn.btn-marrom:not(.btn-icon-28):not(.btn-xs):not(.ftx-actions *):hover {
-  box-shadow:
-    0 0 0 1px #000,
-    0 0 0 3px #999,
-    0 4px 12px rgba(0, 0, 0, 0.25) !important;
+  box-shadow: 0 0 0 1px #000, 0 0 0 3px #999, 0 4px 12px rgba(0, 0, 0, 0.25) !important;
 }
 
 .btn-xs {
@@ -1255,13 +1212,8 @@
   white-space: nowrap;
   font-weight: 700;
   letter-spacing: 0.2px;
-  transition:
-    box-shadow 0.18s ease,
-    transform 0.18s ease,
-    filter 0.18s ease;
-  box-shadow:
-    0 0 8px rgba(0, 0, 0, 0.08),
-    0 2px 4px rgba(0, 0, 0, 0.06);
+  transition: box-shadow 0.18s ease, transform 0.18s ease, filter 0.18s ease;
+  box-shadow: 0 0 8px rgba(0, 0, 0, 0.08), 0 2px 4px rgba(0, 0, 0, 0.06);
   border: 0;
   color: #fff !important;
   border-radius: 0.5rem;
@@ -1271,9 +1223,7 @@
 .btn-verde {
 
   background-color: var(--status-active-bg) !important;
-  box-shadow:
-    0 0 8px var(--status-active-shadow),
-    0 2px 4px rgba(0, 0, 0, 0.12) !important;
+  box-shadow: 0 0 8px var(--status-active-shadow), 0 2px 4px rgba(0, 0, 0, 0.12) !important;
   color: #fff !important;
 }
 
@@ -1281,8 +1231,7 @@
 .btn-verde:hover {
   background-color: var(--status-active-bg-hover) !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px var(--status-active-shadow),
+  box-shadow: 0 0 20px var(--status-active-shadow),
     0 6px 12px rgba(0, 0, 0, 0.18) !important;
 }
 
@@ -1290,8 +1239,7 @@
 .fundo-cinza {
   background-color: var(--status-inactive-bg) !important;
   color: aliceblue !important;
-  box-shadow:
-    0 0 8px var(--status-inactive-shadow),
+  box-shadow: 0 0 8px var(--status-inactive-shadow),
     0 2px 4px rgba(0, 0, 0, 0.12) !important;
 }
 
@@ -1299,8 +1247,7 @@
 .fundo-cinza:hover {
   background-color: var(--status-inactive-bg-hover) !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px var(--status-inactive-shadow),
+  box-shadow: 0 0 20px var(--status-inactive-shadow),
     0 6px 12px rgba(0, 0, 0, 0.18) !important;
 }
 
@@ -1316,9 +1263,7 @@
   background-color: #3d5771 !important;
   border-color: #2d4559 !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(61, 87, 113, 0.5),
-    0 2px 4px rgba(61, 87, 113, 0.3) !important;
+  box-shadow: 0 0 8px rgba(61, 87, 113, 0.5), 0 2px 4px rgba(61, 87, 113, 0.3) !important;
 }
 
 .btn.btn-azul:hover,
@@ -1327,9 +1272,7 @@
   background-color: #2d4559 !important;
   border-color: #1f3241 !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(61, 87, 113, 0.8),
-    0 6px 12px rgba(61, 87, 113, 0.5) !important;
+  box-shadow: 0 0 20px rgba(61, 87, 113, 0.8), 0 6px 12px rgba(61, 87, 113, 0.5) !important;
   color: white !important;
 }
 
@@ -1352,9 +1295,7 @@
   background-color: #a0522d !important;
   border-color: #8b4513 !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(160, 82, 45, 0.5),
-    0 2px 4px rgba(160, 82, 45, 0.3);
+  box-shadow: 0 0 8px rgba(160, 82, 45, 0.5), 0 2px 4px rgba(160, 82, 45, 0.3);
 }
 
 .btn.btn-fundo-laranja:hover,
@@ -1363,9 +1304,7 @@
   background-color: #8b4513 !important;
   border-color: #6b3410 !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(160, 82, 45, 0.8),
-    0 6px 12px rgba(160, 82, 45, 0.5);
+  box-shadow: 0 0 20px rgba(160, 82, 45, 0.8), 0 6px 12px rgba(160, 82, 45, 0.5);
   color: white !important;
 }
 
@@ -1388,9 +1327,7 @@
   background-color: #722f37 !important;
   border-color: #5a252c !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(114, 47, 55, 0.5),
-    0 2px 4px rgba(114, 47, 55, 0.3) !important;
+  box-shadow: 0 0 8px rgba(114, 47, 55, 0.5), 0 2px 4px rgba(114, 47, 55, 0.3) !important;
 }
 
 .btn.btn-vinho:hover,
@@ -1399,9 +1336,7 @@
   background-color: #5a252c !important;
   border-color: #4a1f24 !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(114, 47, 55, 0.8),
-    0 6px 12px rgba(114, 47, 55, 0.5) !important;
+  box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important;
   color: white !important;
 }
 
@@ -1451,13 +1386,9 @@
   background-color: #7e583d !important;
   border-color: #6a4a33 !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(126, 88, 61, 0.5),
-    0 2px 4px rgba(126, 88, 61, 0.3) !important;
+  box-shadow: 0 0 8px rgba(126, 88, 61, 0.5), 0 2px 4px rgba(126, 88, 61, 0.3) !important;
   border-radius: 0.375rem !important;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
   border: none;
   position: relative;
   text-decoration: none !important;
@@ -1470,9 +1401,7 @@
   background-color: #6a4a33 !important;
   border-color: #5a3d2a !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(126, 88, 61, 0.8),
-    0 6px 12px rgba(126, 88, 61, 0.5) !important;
+  box-shadow: 0 0 20px rgba(126, 88, 61, 0.8), 0 6px 12px rgba(126, 88, 61, 0.5) !important;
   color: white !important;
 }
 
@@ -1530,9 +1459,7 @@
   background-color: #dc3545 !important;
   border-color: #c82333 !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(220, 53, 69, 0.5),
-    0 2px 4px rgba(220, 53, 69, 0.3) !important;
+  box-shadow: 0 0 8px rgba(220, 53, 69, 0.5), 0 2px 4px rgba(220, 53, 69, 0.3) !important;
 }
 
 .btn.fundo-vermelho:hover,
@@ -1541,9 +1468,7 @@
   background-color: #c82333 !important;
   border-color: #bd2130 !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(220, 53, 69, 0.8),
-    0 6px 12px rgba(220, 53, 69, 0.5) !important;
+  box-shadow: 0 0 20px rgba(220, 53, 69, 0.8), 0 6px 12px rgba(220, 53, 69, 0.5) !important;
   color: white !important;
 }
 
@@ -1566,9 +1491,7 @@
   background-color: #2f4f4f !important;
   border-color: #253a3a !important;
   color: aliceblue !important;
-  box-shadow:
-    0 0 8px rgba(47, 79, 79, 0.5),
-    0 2px 4px rgba(47, 79, 79, 0.3) !important;
+  box-shadow: 0 0 8px rgba(47, 79, 79, 0.5), 0 2px 4px rgba(47, 79, 79, 0.3) !important;
 }
 
 .btn.fundo-cinza:hover,
@@ -1577,9 +1500,7 @@
   background-color: #253a3a !important;
   border-color: #1e2e2e !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(47, 79, 79, 0.8),
-    0 6px 12px rgba(47, 79, 79, 0.5) !important;
+  box-shadow: 0 0 20px rgba(47, 79, 79, 0.8), 0 6px 12px rgba(47, 79, 79, 0.5) !important;
   color: aliceblue !important;
 }
 
@@ -1601,9 +1522,7 @@
 button.fundo-roxo {
   background-color: #4b0082 !important;
   color: #fff !important;
-  box-shadow:
-    0 0 8px rgba(75, 0, 130, 0.5),
-    0 2px 4px rgba(75, 0, 130, 0.3) !important;
+  box-shadow: 0 0 8px rgba(75, 0, 130, 0.5), 0 2px 4px rgba(75, 0, 130, 0.3) !important;
 }
 
 .btn.fundo-roxo:hover,
@@ -1611,9 +1530,7 @@
 button.fundo-roxo:hover {
   background-color: #3a006b !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 25px rgba(75, 0, 130, 0.8),
-    0 6px 15px rgba(75, 0, 130, 0.5) !important;
+  box-shadow: 0 0 25px rgba(75, 0, 130, 0.8), 0 6px 15px rgba(75, 0, 130, 0.5) !important;
   color: white !important;
 }
 
@@ -1634,13 +1551,8 @@
   background-color: #7b3f00 !important;
   border-color: #7b3f00 !important;
   color: #fff !important;
-  box-shadow:
-    0 0 8px rgba(123, 63, 0, 0.5),
-    0 2px 4px rgba(123, 63, 0, 0.3) !important;
-  transition:
-    box-shadow 0.2s ease,
-    filter 0.2s ease,
-    transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(123, 63, 0, 0.5), 0 2px 4px rgba(123, 63, 0, 0.3) !important;
+  transition: box-shadow 0.2s ease, filter 0.2s ease, transform 0.2s ease !important;
 }
 
 .btn.fundo-chocolate:hover,
@@ -1648,9 +1560,7 @@
 button.fundo-chocolate:hover {
   filter: brightness(1.06);
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 25px rgba(123, 63, 0, 0.8),
-    0 8px 20px rgba(123, 63, 0, 0.5) !important;
+  box-shadow: 0 0 25px rgba(123, 63, 0, 0.8), 0 8px 20px rgba(123, 63, 0, 0.5) !important;
   color: white !important;
 }
 
@@ -1701,9 +1611,7 @@
   background-color: var(--bs-btn-bg) !important;
   border-color: var(--bs-btn-border-color) !important;
   color: var(--bs-btn-color) !important;
-  box-shadow:
-    0 0 8px rgba(58, 110, 165, 0.5),
-    0 2px 4px rgba(58, 110, 165, 0.3) !important;
+  box-shadow: 0 0 8px rgba(58, 110, 165, 0.5), 0 2px 4px rgba(58, 110, 165, 0.3) !important;
 }
 
 .btn.btn-editar:hover,
@@ -1711,8 +1619,7 @@
   background-color: var(--bs-btn-hover-bg) !important;
   border-color: var(--bs-btn-hover-border-color) !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(58, 110, 165, 0.8),
+  box-shadow: 0 0 20px rgba(58, 110, 165, 0.8),
     0 6px 12px rgba(58, 110, 165, 0.5) !important;
   color: white !important;
 }
@@ -1732,17 +1639,13 @@
   background: #2f4f4f !important;
   color: #fff !important;
   --btn-color: #2f4f4f;
-  box-shadow:
-    0 0 8px rgba(47, 79, 79, 0.5),
-    0 2px 4px rgba(47, 79, 79, 0.3) !important;
+  box-shadow: 0 0 8px rgba(47, 79, 79, 0.5), 0 2px 4px rgba(47, 79, 79, 0.3) !important;
 }
 
 .btn-foto:hover {
   background-color: #253a3a !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(47, 79, 79, 0.8),
-    0 6px 12px rgba(47, 79, 79, 0.5) !important;
+  box-shadow: 0 0 20px rgba(47, 79, 79, 0.8), 0 6px 12px rgba(47, 79, 79, 0.5) !important;
   color: white !important;
 }
 
@@ -1769,9 +1672,7 @@
   background-color: #38a169 !important;
   border-color: #2d7a50 !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(56, 161, 105, 0.5),
-    0 2px 4px rgba(56, 161, 105, 0.3) !important;
+  box-shadow: 0 0 8px rgba(56, 161, 105, 0.5), 0 2px 4px rgba(56, 161, 105, 0.3) !important;
 }
 
 .btn.btn-verde:hover,
@@ -1780,8 +1681,7 @@
   background-color: #2d7a50 !important;
   border-color: #246640 !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(56, 161, 105, 0.8),
+  box-shadow: 0 0 20px rgba(56, 161, 105, 0.8),
     0 6px 12px rgba(56, 161, 105, 0.5) !important;
   color: white !important;
 }
@@ -1805,9 +1705,7 @@
   background-color: #f59e0b !important;
   border-color: #d97706 !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(245, 158, 11, 0.5),
-    0 2px 4px rgba(245, 158, 11, 0.3) !important;
+  box-shadow: 0 0 8px rgba(245, 158, 11, 0.5), 0 2px 4px rgba(245, 158, 11, 0.3) !important;
 }
 
 .btn.btn-amarelo:hover,
@@ -1816,8 +1714,7 @@
   background-color: #d97706 !important;
   border-color: #b45309 !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(245, 158, 11, 0.8),
+  box-shadow: 0 0 20px rgba(245, 158, 11, 0.8),
     0 6px 12px rgba(245, 158, 11, 0.5) !important;
   color: white !important;
 }
@@ -1841,9 +1738,7 @@
   background-color: #2c2c2c !important;
   border-color: #1c1c1c !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(44, 44, 44, 0.5),
-    0 2px 4px rgba(44, 44, 44, 0.3) !important;
+  box-shadow: 0 0 8px rgba(44, 44, 44, 0.5), 0 2px 4px rgba(44, 44, 44, 0.3) !important;
 }
 
 .btn.btn-preto:hover,
@@ -1852,9 +1747,7 @@
   background-color: #1c1c1c !important;
   border-color: #0f0f0f !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(44, 44, 44, 0.8),
-    0 6px 12px rgba(44, 44, 44, 0.5) !important;
+  box-shadow: 0 0 20px rgba(44, 44, 44, 0.8), 0 6px 12px rgba(44, 44, 44, 0.5) !important;
   color: white !important;
 }
 
@@ -1877,9 +1770,7 @@
   background-color: #7b4230 !important;
   border-color: #6b3220 !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(123, 66, 48, 0.5),
-    0 2px 4px rgba(123, 66, 48, 0.3) !important;
+  box-shadow: 0 0 8px rgba(123, 66, 48, 0.5), 0 2px 4px rgba(123, 66, 48, 0.3) !important;
 }
 
 .btn.btn-marrom:hover,
@@ -1888,9 +1779,7 @@
   background-color: #6b3220 !important;
   border-color: #5b2210 !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(123, 66, 48, 0.8),
-    0 6px 12px rgba(123, 66, 48, 0.5) !important;
+  box-shadow: 0 0 20px rgba(123, 66, 48, 0.8), 0 6px 12px rgba(123, 66, 48, 0.5) !important;
   color: white !important;
 }
 
@@ -1915,13 +1804,8 @@
   background: linear-gradient(135deg, var(--bg1), var(--bg2));
   color: #fff !important;
   border: 0;
-  box-shadow:
-    0 0 8px rgba(13, 110, 253, 0.5),
-    0 2px 4px rgba(13, 110, 253, 0.3) !important;
-  transition:
-    transform 0.12s ease,
-    box-shadow 0.12s ease,
-    filter 0.12s ease;
+  box-shadow: 0 0 8px rgba(13, 110, 253, 0.5), 0 2px 4px rgba(13, 110, 253, 0.3) !important;
+  transition: transform 0.12s ease, box-shadow 0.12s ease, filter 0.12s ease;
 }
 
 .modal-footer .btn.custom-primary-btn:hover,
@@ -1930,9 +1814,7 @@
 .btn.custom-primary-btn:focus {
   filter: brightness(1.05);
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(13, 110, 253, 0.8),
-    0 8px 20px var(--shadow) !important;
+  box-shadow: 0 0 20px rgba(13, 110, 253, 0.8), 0 8px 20px var(--shadow) !important;
   color: #fff !important;
 }
 
@@ -1974,9 +1856,7 @@
   --azul-escuro-hover: #1565c0;
   background-color: var(--azul-escuro) !important;
   color: #fff !important;
-  transition:
-    background-color 0.2s ease,
-    transform 0.2s ease,
+  transition: background-color 0.2s ease, transform 0.2s ease,
     box-shadow 0.2s ease;
   border: 0;
   border-radius: 8px;
@@ -1984,18 +1864,14 @@
   text-decoration: none;
   display: inline-block;
   padding: 10px 16px;
-  box-shadow:
-    0 0 8px rgba(13, 71, 161, 0.5),
-    0 2px 4px rgba(13, 71, 161, 0.3) !important;
+  box-shadow: 0 0 8px rgba(13, 71, 161, 0.5), 0 2px 4px rgba(13, 71, 161, 0.3) !important;
 }
 
 .azul-escuro:hover,
 .azul-escuro:focus-visible {
   background-color: var(--azul-escuro-hover) !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(13, 71, 161, 0.8),
-    0 6px 12px rgba(13, 71, 161, 0.5) !important;
+  box-shadow: 0 0 20px rgba(13, 71, 161, 0.8), 0 6px 12px rgba(13, 71, 161, 0.5) !important;
 }
 
 .azul-escuro.auto:hover,
@@ -2031,12 +1907,8 @@
   background-color: #2c2c2c !important;
   border-color: #1c1c1c !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(44, 44, 44, 0.5),
-    0 2px 4px rgba(44, 44, 44, 0.3) !important;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(44, 44, 44, 0.5), 0 2px 4px rgba(44, 44, 44, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
   border-radius: 0.375rem !important;
 }
 
@@ -2046,9 +1918,7 @@
   background-color: #1c1c1c !important;
   border-color: #0f0f0f !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(44, 44, 44, 0.8),
-    0 6px 12px rgba(44, 44, 44, 0.5) !important;
+  box-shadow: 0 0 20px rgba(44, 44, 44, 0.8), 0 6px 12px rgba(44, 44, 44, 0.5) !important;
   color: white !important;
   filter: brightness(1.1) !important;
 }
@@ -2070,12 +1940,8 @@
   background-color: #7b3f00 !important;
   border-color: #7b3f00 !important;
   color: #fff !important;
-  box-shadow:
-    0 0 8px rgba(123, 63, 0, 0.5),
-    0 2px 4px rgba(123, 63, 0, 0.3) !important;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(123, 63, 0, 0.5), 0 2px 4px rgba(123, 63, 0, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
   border-radius: 0.375rem !important;
 }
 
@@ -2083,9 +1949,7 @@
   background-color: #5c2f00 !important;
   border-color: #5c2f00 !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 25px rgba(123, 63, 0, 0.8),
-    0 8px 20px rgba(123, 63, 0, 0.5) !important;
+  box-shadow: 0 0 25px rgba(123, 63, 0, 0.8), 0 8px 20px rgba(123, 63, 0, 0.5) !important;
   color: white !important;
   filter: brightness(1.06) !important;
 }
@@ -2105,12 +1969,8 @@
   background-color: #2c2c2c !important;
   border-color: #1c1c1c !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(44, 44, 44, 0.5),
-    0 2px 4px rgba(44, 44, 44, 0.3) !important;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(44, 44, 44, 0.5), 0 2px 4px rgba(44, 44, 44, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
   border-radius: 0.375rem !important;
 }
 
@@ -2120,9 +1980,7 @@
   background-color: #1c1c1c !important;
   border-color: #0f0f0f !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(44, 44, 44, 0.8),
-    0 6px 12px rgba(44, 44, 44, 0.5) !important;
+  box-shadow: 0 0 20px rgba(44, 44, 44, 0.8), 0 6px 12px rgba(44, 44, 44, 0.5) !important;
   color: white !important;
   filter: brightness(1.1) !important;
 }
@@ -2146,12 +2004,8 @@
   background-color: #4a6b8a !important;
   border-color: #3a5a7a !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(74, 107, 138, 0.5),
-    0 2px 4px rgba(74, 107, 138, 0.3) !important;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(74, 107, 138, 0.5), 0 2px 4px rgba(74, 107, 138, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
   border-radius: 0.375rem !important;
   border: none;
   position: relative;
@@ -2163,8 +2017,7 @@
   background-color: #3a5a7a !important;
   border-color: #2a4a6a !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(74, 107, 138, 0.8),
+  box-shadow: 0 0 20px rgba(74, 107, 138, 0.8),
     0 6px 12px rgba(74, 107, 138, 0.5) !important;
   color: white !important;
   filter: brightness(1.1) !important;
@@ -2204,12 +2057,8 @@
   background-color: #4a6b8a !important;
   border-color: #3a5a7a !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(74, 107, 138, 0.5),
-    0 2px 4px rgba(74, 107, 138, 0.3) !important;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(74, 107, 138, 0.5), 0 2px 4px rgba(74, 107, 138, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
   border-radius: 0.375rem !important;
 }
 
@@ -2219,8 +2068,7 @@
   background-color: #3a5a7a !important;
   border-color: #2a4a6a !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(74, 107, 138, 0.8),
+  box-shadow: 0 0 20px rgba(74, 107, 138, 0.8),
     0 6px 12px rgba(74, 107, 138, 0.5) !important;
   color: white !important;
   filter: brightness(1.1) !important;
@@ -2773,9 +2621,7 @@
   background-color: #4b8b3b !important;
   border-color: #3d7230 !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(75, 139, 59, 0.5),
-    0 2px 4px rgba(75, 139, 59, 0.3) !important;
+  box-shadow: 0 0 8px rgba(75, 139, 59, 0.5), 0 2px 4px rgba(75, 139, 59, 0.3) !important;
 }
 
 .btn.btn-verde-dinheiro:hover,
@@ -2784,9 +2630,7 @@
   background-color: #3d7230 !important;
   border-color: #2f5925 !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(75, 139, 59, 0.8),
-    0 6px 12px rgba(75, 139, 59, 0.5) !important;
+  box-shadow: 0 0 20px rgba(75, 139, 59, 0.8), 0 6px 12px rgba(75, 139, 59, 0.5) !important;
   color: white !important;
 }
 
@@ -2809,9 +2653,7 @@
   background-color: #557570 !important;
   border-color: #466560 !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(85, 117, 112, 0.5),
-    0 2px 4px rgba(85, 117, 112, 0.3) !important;
+  box-shadow: 0 0 8px rgba(85, 117, 112, 0.5), 0 2px 4px rgba(85, 117, 112, 0.3) !important;
 }
 
 .btn.btn-custos-viagem:hover,
@@ -2820,8 +2662,7 @@
   background-color: #466560 !important;
   border-color: #375550 !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(85, 117, 112, 0.8),
+  box-shadow: 0 0 20px rgba(85, 117, 112, 0.8),
     0 6px 12px rgba(85, 117, 112, 0.5) !important;
   color: white !important;
 }
@@ -2855,8 +2696,7 @@
   background-color: #a97b6e !important;
   border-color: #8f6a5e !important;
   color: white !important;
-  box-shadow:
-    0 0 8px rgba(169, 123, 110, 0.5),
+  box-shadow: 0 0 8px rgba(169, 123, 110, 0.5),
     0 2px 4px rgba(169, 123, 110, 0.3) !important;
 }
 
@@ -2866,8 +2706,7 @@
   background-color: #8f6a5e !important;
   border-color: #7a5a4f !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(169, 123, 110, 0.8),
+  box-shadow: 0 0 20px rgba(169, 123, 110, 0.8),
     0 6px 12px rgba(169, 123, 110, 0.5) !important;
   color: white !important;
 }
@@ -2901,9 +2740,7 @@
   padding: 1rem;
   text-align: center;
   box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
-  transition:
-    transform 0.2s ease,
-    box-shadow 0.2s ease;
+  transition: transform 0.2s ease, box-shadow 0.2s ease;
   border: 1px solid rgba(0, 0, 0, 0.05);
   height: 100%;
 }
@@ -3232,9 +3069,7 @@
 }
 
 .modal.fade .modal-dialog {
-  transition:
-    transform 0.25s ease-out,
-    opacity 0.25s ease-out;
+  transition: transform 0.25s ease-out, opacity 0.25s ease-out;
 }
 
 .modal.show .modal-dialog {
@@ -3313,6 +3148,18 @@
     var(--modal-dinheiro) 100%
   );
   color: #fff;
+}
+
+.modal-header-roxo {
+  background: linear-gradient(
+    135deg,
+    #5e35b1 0%,
+    #7e57c2 50%,
+    #5e35b1 100%
+  );
+  color: #fff;
+  background-size: 200% 200%;
+  animation: gradientShift 8s ease infinite;
 }
 
 .modal-title,
@@ -3993,12 +3840,8 @@
   display: inline-flex;
   align-items: center;
   gap: 0.5rem;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
-  box-shadow:
-    0 0 8px rgba(114, 47, 55, 0.5),
-    0 2px 4px rgba(114, 47, 55, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(114, 47, 55, 0.5), 0 2px 4px rgba(114, 47, 55, 0.3) !important;
   cursor: pointer;
 }
 
@@ -4006,9 +3849,7 @@
   background-color: #5a252c !important;
   color: #fff !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(114, 47, 55, 0.8),
-    0 6px 12px rgba(114, 47, 55, 0.5) !important;
+  box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important;
 }
 
 .btn-ftx-fechar:active,
@@ -4036,12 +3877,8 @@
   display: inline-flex;
   align-items: center;
   gap: 0.5rem;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
-  box-shadow:
-    0 0 8px rgba(114, 47, 55, 0.5),
-    0 2px 4px rgba(114, 47, 55, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(114, 47, 55, 0.5), 0 2px 4px rgba(114, 47, 55, 0.3) !important;
   cursor: pointer;
 }
 
@@ -4049,9 +3886,7 @@
   background-color: #5a252c !important;
   color: #fff !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(114, 47, 55, 0.8),
-    0 6px 12px rgba(114, 47, 55, 0.5) !important;
+  box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important;
 }
 
 .btn-ftx-voltar:active,
@@ -4079,12 +3914,8 @@
   display: inline-flex;
   align-items: center;
   gap: 0.5rem;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
-  box-shadow:
-    0 0 8px rgba(61, 87, 113, 0.5),
-    0 2px 4px rgba(61, 87, 113, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(61, 87, 113, 0.5), 0 2px 4px rgba(61, 87, 113, 0.3) !important;
   cursor: pointer;
 }
 
@@ -4092,9 +3923,7 @@
   background-color: #2d4156 !important;
   color: #fff !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(61, 87, 113, 0.8),
-    0 6px 12px rgba(61, 87, 113, 0.5) !important;
+  box-shadow: 0 0 20px rgba(61, 87, 113, 0.8), 0 6px 12px rgba(61, 87, 113, 0.5) !important;
 }
 
 .btn-ftx-confirmar:active,
@@ -4115,12 +3944,8 @@
   display: inline-flex;
   align-items: center;
   gap: 0.5rem;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
-  box-shadow:
-    0 0 8px rgba(46, 125, 50, 0.5),
-    0 2px 4px rgba(46, 125, 50, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(46, 125, 50, 0.5), 0 2px 4px rgba(46, 125, 50, 0.3) !important;
   cursor: pointer;
 }
 
@@ -4128,9 +3953,7 @@
   background-color: #1b5e20 !important;
   color: #fff !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(46, 125, 50, 0.8),
-    0 6px 12px rgba(46, 125, 50, 0.5) !important;
+  box-shadow: 0 0 20px rgba(46, 125, 50, 0.8), 0 6px 12px rgba(46, 125, 50, 0.5) !important;
 }
 
 .btn-ftx-salvar:active,
@@ -4151,12 +3974,8 @@
   display: inline-flex;
   align-items: center;
   gap: 0.5rem;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
-  box-shadow:
-    0 0 8px rgba(198, 40, 40, 0.5),
-    0 2px 4px rgba(198, 40, 40, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(198, 40, 40, 0.5), 0 2px 4px rgba(198, 40, 40, 0.3) !important;
   cursor: pointer;
 }
 
@@ -4164,9 +3983,7 @@
   background-color: #b71c1c !important;
   color: #fff !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(198, 40, 40, 0.8),
-    0 6px 12px rgba(198, 40, 40, 0.5) !important;
+  box-shadow: 0 0 20px rgba(198, 40, 40, 0.8), 0 6px 12px rgba(198, 40, 40, 0.5) !important;
 }
 
 .btn-ftx-excluir:active,
@@ -4187,12 +4004,8 @@
   display: inline-flex;
   align-items: center;
   gap: 0.5rem;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
-  box-shadow:
-    0 0 8px rgba(84, 110, 122, 0.5),
-    0 2px 4px rgba(84, 110, 122, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(84, 110, 122, 0.5), 0 2px 4px rgba(84, 110, 122, 0.3) !important;
   cursor: pointer;
 }
 
@@ -4200,8 +4013,7 @@
   background-color: #37474f !important;
   color: #fff !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(84, 110, 122, 0.8),
+  box-shadow: 0 0 20px rgba(84, 110, 122, 0.8),
     0 6px 12px rgba(84, 110, 122, 0.5) !important;
 }
 
@@ -4254,12 +4066,8 @@
   display: inline-flex;
   align-items: center;
   gap: 0.5rem;
-  transition:
-    all 0.3s ease,
-    transform 0.2s ease !important;
-  box-shadow:
-    0 0 8px rgba(114, 47, 55, 0.5),
-    0 2px 4px rgba(114, 47, 55, 0.3) !important;
+  transition: all 0.3s ease, transform 0.2s ease !important;
+  box-shadow: 0 0 8px rgba(114, 47, 55, 0.5), 0 2px 4px rgba(114, 47, 55, 0.3) !important;
   cursor: pointer;
 }
 
@@ -4267,9 +4075,7 @@
   background-color: #5a252c !important;
   color: #fff !important;
   animation: buttonWiggle 0.5s ease-in-out !important;
-  box-shadow:
-    0 0 20px rgba(114, 47, 55, 0.8),
-    0 6px 12px rgba(114, 47, 55, 0.5) !important;
+  box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important;
 }
 
 .btn-modal-fechar:active,
@@ -4590,9 +4396,8 @@
 }
 
 .tooltip-ftx-azul {
-  font-family:
-    -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue",
-    Arial, sans-serif !important;
+  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto,
+    "Helvetica Neue", Arial, sans-serif !important;
 }
 
 .tooltip-ftx-azul .tooltip-inner {
@@ -4617,41 +4422,6 @@
 .tooltip-ftx-azul.bs-tooltip-bottom .tooltip-arrow::before,
 .tooltip-ftx-azul.bs-tooltip-start .tooltip-arrow::before,
 .tooltip-ftx-azul.bs-tooltip-end .tooltip-arrow::before {
-  display: none !important;
-}
-
-.tooltip-ftx-agenda-dinamica {
-  font-family:
-    -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue",
-    Arial, sans-serif !important;
-}
-
-.tooltip-ftx-agenda-dinamica .tooltip-inner {
-  font-size: 0.8rem !important;
-  font-weight: 500 !important;
-  padding: 0.5rem 0.75rem !important;
-  border-radius: 0.5rem !important;
-  border: 1px solid rgba(0, 0, 0, 0.15) !important;
-  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2) !important;
-  max-width: 350px !important;
-  text-align: left !important;
-  line-height: 1.6 !important;
-  white-space: normal !important;
-}
-
-.tooltip-ftx-agenda-dinamica .tooltip-inner i {
-  margin-right: 0.35rem !important;
-  opacity: 0.9 !important;
-}
-
-.tooltip-ftx-agenda-dinamica .tooltip-arrow {
-  display: none !important;
-}
-
-.tooltip-ftx-agenda-dinamica.bs-tooltip-top .tooltip-arrow::before,
-.tooltip-ftx-agenda-dinamica.bs-tooltip-bottom .tooltip-arrow::before,
-.tooltip-ftx-agenda-dinamica.bs-tooltip-start .tooltip-arrow::before,
-.tooltip-ftx-agenda-dinamica.bs-tooltip-end .tooltip-arrow::before {
   display: none !important;
 }
 
@@ -4704,67 +4474,376 @@
   background-color: #a0d0f0 !important;
 }
 
-.k-calendar,
-.k-calendar-container,
-.k-animation-container > .k-calendar {
-  min-width: 280px !important;
-  width: auto !important;
-}
-
-.k-calendar .k-calendar-view,
-.k-calendar .k-calendar-monthview {
-  min-width: 260px !important;
-}
-
-.k-calendar td,
-.k-calendar th,
-.k-calendar .k-calendar-td,
-.k-calendar .k-calendar-th {
-  min-width: 32px !important;
-  width: 32px !important;
-  height: 32px !important;
-  padding: 2px !important;
-  text-align: center !important;
-}
-
-.k-calendar .k-link,
-.k-calendar .k-calendar-td .k-link {
-  min-width: 28px !important;
-  min-height: 28px !important;
-  line-height: 28px !important;
-  display: flex !important;
-  align-items: center !important;
-  justify-content: center !important;
-}
-
-.k-calendar thead th,
-.k-calendar .k-calendar-thead th,
-.k-calendar .k-calendar-th {
-  font-size: 0.75rem !important;
-  font-weight: 600 !important;
-  min-width: 32px !important;
-}
-
-.k-animation-container {
-  min-width: 280px !important;
-}
-
-.k-datepicker,
-.k-datepicker .k-picker-wrap,
-.k-datepicker .k-input-inner,
-.k-input.k-datepicker,
-span.k-datepicker {
-  height: var(--ftx-input-height) !important;
-  min-height: var(--ftx-input-height) !important;
-}
-
-.k-datepicker .k-input-inner {
+.dataTables_wrapper .dt-buttons .dt-button,
+.dataTables_wrapper .dt-buttons button.dt-button,
+div.dt-buttons .dt-button,
+div.dt-buttons button.dt-button {
+  background: linear-gradient(135deg, #cc5200 0%, #b34700 100%) !important;
+  border: none !important;
+  color: #fff !important;
   padding: 0.375rem 0.75rem !important;
   font-size: 0.875rem !important;
-}
-
-.k-datepicker .k-input-button,
-.k-datepicker .k-select {
-  height: var(--ftx-input-height) !important;
-  min-height: var(--ftx-input-height) !important;
-}
+  font-weight: 500 !important;
+  border-radius: 0.25rem !important;
+  transition: all 0.3s ease !important;
+  box-shadow: 0 2px 4px rgba(204, 82, 0, 0.3) !important;
+  margin-right: 0.25rem !important;
+  text-transform: none !important;
+  font-family: 'Outfit', sans-serif !important;
+}
+
+.dataTables_wrapper .dt-buttons .dt-button:hover,
+.dataTables_wrapper .dt-buttons button.dt-button:hover,
+div.dt-buttons .dt-button:hover,
+div.dt-buttons button.dt-button:hover {
+  background: linear-gradient(135deg, #b34700 0%, #a03d00 100%) !important;
+  box-shadow: 0 4px 8px rgba(204, 82, 0, 0.5) !important;
+  transform: translateY(-1px) !important;
+}
+
+.dataTables_wrapper .dt-buttons .dt-button:active,
+.dataTables_wrapper .dt-buttons button.dt-button:active,
+div.dt-buttons .dt-button:active,
+div.dt-buttons button.dt-button:active {
+  transform: translateY(0) !important;
+  box-shadow: 0 1px 2px rgba(204, 82, 0, 0.3) !important;
+}
+
+.dataTables_wrapper .dt-button-collection,
+div.dt-button-collection {
+  background: #fff !important;
+  border: 1px solid #cc5200 !important;
+  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15) !important;
+}
+
+.dataTables_wrapper .dt-button-collection .dt-button,
+div.dt-button-collection .dt-button {
+  background: transparent !important;
+  color: #333 !important;
+  box-shadow: none !important;
+  text-align: left !important;
+}
+
+.dataTables_wrapper .dt-button-collection .dt-button:hover,
+div.dt-button-collection .dt-button:hover {
+  background: #fff3e6 !important;
+  color: #cc5200 !important;
+  transform: none !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button,
+div.dataTables_paginate .paginate_button {
+  background: linear-gradient(135deg, #cc5200 0%, #b34700 100%) !important;
+  border: none !important;
+  color: #fff !important;
+  padding: 0.375rem 0.75rem !important;
+  margin: 0 0.125rem !important;
+  border-radius: 0.25rem !important;
+  transition: all 0.3s ease !important;
+  box-shadow: 0 2px 4px rgba(204, 82, 0, 0.3) !important;
+  font-weight: 500 !important;
+  font-family: 'Outfit', sans-serif !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button:hover,
+div.dataTables_paginate .paginate_button:hover {
+  background: linear-gradient(135deg, #b34700 0%, #a03d00 100%) !important;
+  color: #fff !important;
+  box-shadow: 0 4px 8px rgba(204, 82, 0, 0.5) !important;
+  transform: translateY(-1px) !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button.current,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current:hover,
+div.dataTables_paginate .paginate_button.current,
+div.dataTables_paginate .paginate_button.current:hover {
+  background: linear-gradient(135deg, #a03d00 0%, #8a3300 100%) !important;
+  color: #fff !important;
+  box-shadow: 0 0 0 2px rgba(204, 82, 0, 0.4) !important;
+  transform: none !important;
+  font-weight: 700 !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button.current a,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current span,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current *,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current:hover a,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current:hover span,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current:hover *,
+div.dataTables_paginate .paginate_button.current a,
+div.dataTables_paginate .paginate_button.current span,
+div.dataTables_paginate .paginate_button.current *,
+div.dataTables_paginate .paginate_button.current:hover a,
+div.dataTables_paginate .paginate_button.current:hover span,
+div.dataTables_paginate .paginate_button.current:hover * {
+  color: #fff !important;
+  font-weight: 700 !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button.disabled,
+.dataTables_wrapper .dataTables_paginate .paginate_button.disabled:hover,
+div.dataTables_paginate .paginate_button.disabled,
+div.dataTables_paginate .paginate_button.disabled:hover {
+  background: #e0e0e0 !important;
+  color: #ffffff !important;
+  cursor: not-allowed !important;
+  box-shadow: none !important;
+  transform: none !important;
+  opacity: 0.6 !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button.disabled a,
+.dataTables_wrapper .dataTables_paginate .paginate_button.disabled span,
+.dataTables_wrapper .dataTables_paginate .paginate_button.disabled *,
+.dataTables_wrapper .dataTables_paginate .paginate_button.disabled:hover a,
+.dataTables_wrapper .dataTables_paginate .paginate_button.disabled:hover span,
+.dataTables_wrapper .dataTables_paginate .paginate_button.disabled:hover *,
+div.dataTables_paginate .paginate_button.disabled a,
+div.dataTables_paginate .paginate_button.disabled span,
+div.dataTables_paginate .paginate_button.disabled *,
+div.dataTables_paginate .paginate_button.disabled:hover a,
+div.dataTables_paginate .paginate_button.disabled:hover span,
+div.dataTables_paginate .paginate_button.disabled:hover * {
+  color: #ffffff !important;
+}
+
+.dataTables_wrapper .dataTables_paginate,
+div.dataTables_paginate {
+  margin-top: 1rem !important;
+}
+
+.dataTables_wrapper .dataTables_paginate span .paginate_button,
+div.dataTables_paginate span .paginate_button {
+  margin: 0 0.125rem !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button *,
+.dataTables_wrapper .dataTables_paginate .paginate_button a,
+.dataTables_wrapper .dataTables_paginate .paginate_button a:link,
+.dataTables_wrapper .dataTables_paginate .paginate_button a:visited,
+.dataTables_wrapper .dataTables_paginate .paginate_button span,
+.dataTables_wrapper .dataTables_paginate .paginate_button .page-link,
+.dataTables_wrapper .dataTables_paginate .page-link,
+.dataTables_wrapper .dataTables_paginate .pagination .page-item:not(.disabled) .page-link,
+.dataTables_wrapper .dataTables_paginate .pagination .page-item.active .page-link,
+div.dataTables_paginate .paginate_button *,
+div.dataTables_paginate .paginate_button a,
+div.dataTables_paginate .paginate_button a:link,
+div.dataTables_paginate .paginate_button a:visited,
+div.dataTables_paginate .paginate_button span,
+div.dataTables_paginate .paginate_button .page-link,
+div.dataTables_paginate .page-link,
+div.dataTables_paginate .pagination .page-item:not(.disabled) .page-link,
+div.dataTables_paginate .pagination .page-item.active .page-link {
+  color: #fff !important;
+  text-decoration: none !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button:hover *,
+.dataTables_wrapper .dataTables_paginate .paginate_button:hover a,
+.dataTables_wrapper .dataTables_paginate .paginate_button:hover a:link,
+.dataTables_wrapper .dataTables_paginate .paginate_button:hover a:visited,
+.dataTables_wrapper .dataTables_paginate .paginate_button:hover span,
+.dataTables_wrapper .dataTables_paginate .paginate_button:hover .page-link,
+.dataTables_wrapper .dataTables_paginate .page-link:hover,
+.dataTables_wrapper .dataTables_paginate .pagination .page-item:not(.disabled) .page-link:hover,
+div.dataTables_paginate .paginate_button:hover *,
+div.dataTables_paginate .paginate_button:hover a,
+div.dataTables_paginate .paginate_button:hover a:link,
+div.dataTables_paginate .paginate_button:hover a:visited,
+div.dataTables_paginate .paginate_button:hover span,
+div.dataTables_paginate .paginate_button:hover .page-link,
+div.dataTables_paginate .page-link:hover,
+div.dataTables_paginate .pagination .page-item:not(.disabled) .page-link:hover {
+  color: #fff !important;
+  text-decoration: none !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button.current *,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current a,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current a:link,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current a:visited,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current span,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current .page-link,
+.dataTables_wrapper .dataTables_paginate .page-link.current,
+.dataTables_wrapper .dataTables_paginate .pagination .page-item.active .page-link,
+div.dataTables_paginate .paginate_button.current *,
+div.dataTables_paginate .paginate_button.current a,
+div.dataTables_paginate .paginate_button.current a:link,
+div.dataTables_paginate .paginate_button.current a:visited,
+div.dataTables_paginate .paginate_button.current span,
+div.dataTables_paginate .paginate_button.current .page-link,
+div.dataTables_paginate .page-link.current,
+div.dataTables_paginate .pagination .page-item.active .page-link {
+  color: #fff !important;
+  text-decoration: none !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .pagination .page-item .page-link,
+.dataTables_wrapper .dataTables_paginate .pagination .page-item .page-link *,
+div.dataTables_paginate .pagination .page-item .page-link,
+div.dataTables_paginate .pagination .page-item .page-link * {
+  color: #fff !important;
+}
+
+.page-item .page-link,
+.dataTables_paginate .paginate_button {
+    background-color: #343a40 !important;
+    border-color: #454d55 !important;
+    color: #ffffff !important;
+}
+
+.page-item .page-link:hover,
+.dataTables_paginate .paginate_button:hover {
+    background-color: #23272b !important;
+    color: #ffffff !important;
+    border-color: #1d2124 !important;
+}
+
+.page-item.active .page-link,
+.paginate_button.current,
+.paginate_button.active {
+    background-color: #0d6efd !important;
+    border-color: #0d6efd !important;
+    color: #ffffff !important;
+    font-weight: bold !important;
+}
+
+.page-item.disabled .page-link,
+.paginate_button.disabled {
+    background-color: #6c757d !important;
+    border-color: #6c757d !important;
+    color: #ced4da !important;
+    opacity: 0.6 !important;
+}
+
+.page-item .page-link,
+.dataTables_paginate .paginate_button,
+.dataTables_wrapper .dataTables_paginate .paginate_button {
+    background: linear-gradient(135deg, #cc5200 0%, #b34700 100%) !important;
+    border: none !important;
+    color: #ffffff !important;
+    box-shadow: 0 2px 4px rgba(204, 82, 0, 0.3) !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button.current,
+.dataTables_wrapper .dataTables_paginate .paginate_button.active,
+.dataTables_wrapper .page-item.active .page-link,
+.page-item.active .page-link {
+    background: linear-gradient(135deg, #a03d00 0%, #8a3300 100%) !important;
+    color: #ffffff !important;
+    font-weight: bold !important;
+    border: 1px solid rgba(255,255,255,0.2) !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button:hover,
+.dataTables_wrapper .page-item .page-link:hover,
+.page-item .page-link:hover {
+    background: linear-gradient(135deg, #b34700 0%, #a03d00 100%) !important;
+    color: #ffffff !important;
+    transform: translateY(-1px) !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button *,
+.dataTables_wrapper .page-item .page-link * {
+    color: #ffffff !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button:not(.disabled) a,
+.dataTables_wrapper .dataTables_paginate .paginate_button:not(.disabled) span,
+.dataTables_wrapper .dataTables_paginate .paginate_button:not(.disabled),
+div.dataTables_paginate .paginate_button:not(.disabled) a,
+div.dataTables_paginate .paginate_button:not(.disabled) span,
+div.dataTables_paginate .paginate_button:not(.disabled),
+.page-item:not(.disabled) .page-link,
+.page-item:not(.disabled) .page-link a,
+.page-item:not(.disabled) .page-link span {
+    color: #ffffff !important;
+    text-decoration: none !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button:not(.disabled):hover a,
+.dataTables_wrapper .dataTables_paginate .paginate_button:not(.disabled):hover span,
+.dataTables_wrapper .dataTables_paginate .paginate_button:not(.disabled):hover,
+div.dataTables_paginate .paginate_button:not(.disabled):hover a,
+div.dataTables_paginate .paginate_button:not(.disabled):hover span,
+div.dataTables_paginate .paginate_button:not(.disabled):hover,
+.page-item:not(.disabled) .page-link:hover,
+.page-item:not(.disabled) .page-link:hover a,
+.page-item:not(.disabled) .page-link:hover span {
+    color: #ffffff !important;
+    text-decoration: none !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button.current a,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current span,
+.dataTables_wrapper .dataTables_paginate .paginate_button.current,
+.dataTables_wrapper .dataTables_paginate .paginate_button.active a,
+.dataTables_wrapper .dataTables_paginate .paginate_button.active span,
+.dataTables_wrapper .dataTables_paginate .paginate_button.active,
+div.dataTables_paginate .paginate_button.current a,
+div.dataTables_paginate .paginate_button.current span,
+div.dataTables_paginate .paginate_button.current,
+div.dataTables_paginate .paginate_button.active a,
+div.dataTables_paginate .paginate_button.active span,
+div.dataTables_paginate .paginate_button.active,
+.page-item.active .page-link,
+.page-item.active .page-link a,
+.page-item.active .page-link span {
+    color: #ffffff !important;
+    text-decoration: none !important;
+}
+
+.dataTables_wrapper .dataTables_paginate a:link:not(.disabled),
+.dataTables_wrapper .dataTables_paginate a:visited:not(.disabled),
+.dataTables_wrapper .dataTables_paginate a:hover:not(.disabled),
+.dataTables_wrapper .dataTables_paginate a:active:not(.disabled),
+div.dataTables_paginate a:link:not(.disabled),
+div.dataTables_paginate a:visited:not(.disabled),
+div.dataTables_paginate a:hover:not(.disabled),
+div.dataTables_paginate a:active:not(.disabled) {
+    color: #ffffff !important;
+    text-decoration: none !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button.previous:not(.disabled),
+.dataTables_wrapper .dataTables_paginate .paginate_button.next:not(.disabled),
+.dataTables_wrapper .dataTables_paginate .paginate_button.first:not(.disabled),
+.dataTables_wrapper .dataTables_paginate .paginate_button.last:not(.disabled),
+div.dataTables_paginate .paginate_button.previous:not(.disabled),
+div.dataTables_paginate .paginate_button.next:not(.disabled),
+div.dataTables_paginate .paginate_button.first:not(.disabled),
+div.dataTables_paginate .paginate_button.last:not(.disabled) {
+    background: linear-gradient(135deg, #cc5200 0%, #b34700 100%) !important;
+    color: #ffffff !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button.previous:not(.disabled) a,
+.dataTables_wrapper .dataTables_paginate .paginate_button.next:not(.disabled) a,
+.dataTables_wrapper .dataTables_paginate .paginate_button.first:not(.disabled) a,
+.dataTables_wrapper .dataTables_paginate .paginate_button.last:not(.disabled) a,
+.dataTables_wrapper .dataTables_paginate .paginate_button.previous:not(.disabled) span,
+.dataTables_wrapper .dataTables_paginate .paginate_button.next:not(.disabled) span,
+.dataTables_wrapper .dataTables_paginate .paginate_button.first:not(.disabled) span,
+.dataTables_wrapper .dataTables_paginate .paginate_button.last:not(.disabled) span,
+div.dataTables_paginate .paginate_button.previous:not(.disabled) a,
+div.dataTables_paginate .paginate_button.next:not(.disabled) a,
+div.dataTables_paginate .paginate_button.first:not(.disabled) a,
+div.dataTables_paginate .paginate_button.last:not(.disabled) a,
+div.dataTables_paginate .paginate_button.previous:not(.disabled) span,
+div.dataTables_paginate .paginate_button.next:not(.disabled) span,
+div.dataTables_paginate .paginate_button.first:not(.disabled) span,
+div.dataTables_paginate .paginate_button.last:not(.disabled) span {
+    color: #ffffff !important;
+    text-decoration: none !important;
+}
+
+.dataTables_wrapper .dataTables_paginate .paginate_button:not(.disabled) > a,
+.dataTables_wrapper .dataTables_paginate .paginate_button:not(.disabled) > span,
+div.dataTables_paginate .paginate_button:not(.disabled) > a,
+div.dataTables_paginate .paginate_button:not(.disabled) > span,
+.dataTables_wrapper .dataTables_paginate span .paginate_button:not(.disabled) a,
+div.dataTables_paginate span .paginate_button:not(.disabled) a {
+    color: #ffffff !important;
+    text-decoration: none !important;
+}
```
