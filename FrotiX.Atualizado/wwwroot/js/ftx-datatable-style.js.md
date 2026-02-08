# wwwroot/js/ftx-datatable-style.js

**Mudanca:** GRANDE | **+175** linhas | **-106** linhas

---

```diff
--- JANEIRO: wwwroot/js/ftx-datatable-style.js
+++ ATUAL: wwwroot/js/ftx-datatable-style.js
@@ -22,170 +22,239 @@
     };
 
     function rgbParaHsl(r, g, b) {
-        r /= 255; g /= 255; b /= 255;
-        var max = Math.max(r, g, b), min = Math.min(r, g, b);
-        var h, s, l = (max + min) / 2;
-
-        if (max === min) {
-            h = s = 0;
-        } else {
-            var d = max - min;
-            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
-            switch (max) {
-                case r: h = ((g - b) / d + (g < b ? 6 : 0)) / 6; break;
-                case g: h = ((b - r) / d + 2) / 6; break;
-                case b: h = ((r - g) / d + 4) / 6; break;
-            }
-        }
-        return { h: h * 360, s: s * 100, l: l * 100 };
+        try {
+            r /= 255; g /= 255; b /= 255;
+            var max = Math.max(r, g, b), min = Math.min(r, g, b);
+            var h, s, l = (max + min) / 2;
+
+            if (max === min) {
+                h = s = 0;
+            } else {
+                var d = max - min;
+                s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
+                switch (max) {
+                    case r: h = ((g - b) / d + (g < b ? 6 : 0)) / 6; break;
+                    case g: h = ((b - r) / d + 2) / 6; break;
+                    case b: h = ((r - g) / d + 4) / 6; break;
+                }
+            }
+            return { h: h * 360, s: s * 100, l: l * 100 };
+        } catch (erro) {
+            console.error('Erro em rgbParaHsl:', erro);
+            return { h: 0, s: 0, l: 50 };
+        }
     }
 
     function hslParaHex(h, s, l) {
-        h /= 360; s /= 100; l /= 100;
-        var r, g, b;
-
-        if (s === 0) {
-            r = g = b = l;
-        } else {
-            var hue2rgb = function(p, q, t) {
-                if (t < 0) t += 1;
-                if (t > 1) t -= 1;
-                if (t < 1/6) return p + (q - p) * 6 * t;
-                if (t < 1/2) return q;
-                if (t < 2/3) return p + (q - p) * (2/3 - t) * 6;
-                return p;
+        try {
+            h /= 360; s /= 100; l /= 100;
+            var r, g, b;
+
+            if (s === 0) {
+                r = g = b = l;
+            } else {
+                var hue2rgb = function(p, q, t) {
+                    if (t < 0) t += 1;
+                    if (t > 1) t -= 1;
+                    if (t < 1/6) return p + (q - p) * 6 * t;
+                    if (t < 1/2) return q;
+                    if (t < 2/3) return p + (q - p) * (2/3 - t) * 6;
+                    return p;
+                };
+                var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
+                var p = 2 * l - q;
+                r = hue2rgb(p, q, h + 1/3);
+                g = hue2rgb(p, q, h);
+                b = hue2rgb(p, q, h - 1/3);
+            }
+
+            var toHex = function(x) {
+                var hex = Math.round(x * 255).toString(16);
+                return hex.length === 1 ? '0' + hex : hex;
             };
-            var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
-            var p = 2 * l - q;
-            r = hue2rgb(p, q, h + 1/3);
-            g = hue2rgb(p, q, h);
-            b = hue2rgb(p, q, h - 1/3);
-        }
-
-        var toHex = function(x) {
-            var hex = Math.round(x * 255).toString(16);
-            return hex.length === 1 ? '0' + hex : hex;
-        };
-
-        return '#' + toHex(r) + toHex(g) + toHex(b);
+
+            return '#' + toHex(r) + toHex(g) + toHex(b);
+        } catch (erro) {
+            console.error('Erro em hslParaHex:', erro);
+            return '#000000';
+        }
     }
 
     function clarearCor(cor, percentual) {
-
-        cor = cor.replace('#', '');
-
-        var r = parseInt(cor.substring(0, 2), 16);
-        var g = parseInt(cor.substring(2, 4), 16);
-        var b = parseInt(cor.substring(4, 6), 16);
-
-        var hsl = rgbParaHsl(r, g, b);
-
-        hsl.l = Math.min(100, hsl.l + percentual);
-
-        return hslParaHex(hsl.h, hsl.s, hsl.l);
+        try {
+
+            cor = cor.replace('#', '');
+
+            var r = parseInt(cor.substring(0, 2), 16);
+            var g = parseInt(cor.substring(2, 4), 16);
+            var b = parseInt(cor.substring(4, 6), 16);
+
+            var hsl = rgbParaHsl(r, g, b);
+
+            hsl.l = Math.min(100, hsl.l + percentual);
+
+            return hslParaHex(hsl.h, hsl.s, hsl.l);
+        } catch (erro) {
+            console.error('Erro em clarearCor:', erro);
+            return cor;
+        }
     }
 
     function encontrarModalPai(elemento) {
-        if (!elemento || !elemento.closest) return null;
-        return elemento.closest('.modal');
+        try {
+            if (!elemento || !elemento.closest) return null;
+            return elemento.closest('.modal');
+        } catch (erro) {
+            console.error('Erro em encontrarModalPai:', erro);
+            return null;
+        }
     }
 
     function obterCorHeaderModal(modal) {
-        if (!modal) return null;
-
-        var header = modal.querySelector('.modal-header');
-        if (!header) return null;
-
-        var classes = header.className.split(' ');
-        for (var i = 0; i < classes.length; i++) {
-            var classe = classes[i].trim();
-            if (coresHeadersModal[classe]) {
-                return coresHeadersModal[classe];
-            }
-        }
-
-        try {
-            var style = window.getComputedStyle(header);
-            var bg = style.backgroundColor;
-            if (bg && bg !== 'transparent' && bg !== 'rgba(0, 0, 0, 0)') {
-                var match = bg.match(/rgba?\((\d+),\s*(\d+),\s*(\d+)/);
-                if (match) {
-                    var r = parseInt(match[1]).toString(16).padStart(2, '0');
-                    var g = parseInt(match[2]).toString(16).padStart(2, '0');
-                    var b = parseInt(match[3]).toString(16).padStart(2, '0');
-                    var hex = '#' + r + g + b;
-                    if (hex !== '#000000' && hex !== '#ffffff') {
-                        return hex;
+        try {
+            if (!modal) return null;
+
+            var header = modal.querySelector('.modal-header');
+            if (!header) return null;
+
+            var classes = header.className.split(' ');
+            for (var i = 0; i < classes.length; i++) {
+                var classe = classes[i].trim();
+                if (coresHeadersModal[classe]) {
+                    return coresHeadersModal[classe];
+                }
+            }
+
+            try {
+                var style = window.getComputedStyle(header);
+                var bg = style.backgroundColor;
+                if (bg && bg !== 'transparent' && bg !== 'rgba(0, 0, 0, 0)') {
+                    var match = bg.match(/rgba?\((\d+),\s*(\d+),\s*(\d+)/);
+                    if (match) {
+                        var r = parseInt(match[1]).toString(16).padStart(2, '0');
+                        var g = parseInt(match[2]).toString(16).padStart(2, '0');
+                        var b = parseInt(match[3]).toString(16).padStart(2, '0');
+                        var hex = '#' + r + g + b;
+                        if (hex !== '#000000' && hex !== '#ffffff') {
+                            return hex;
+                        }
                     }
                 }
-            }
-        } catch (e) {
-
-        }
-
-        return null;
+            } catch (e) {
+
+            }
+
+            return null;
+        } catch (erro) {
+            console.error('Erro em obterCorHeaderModal:', erro);
+            return null;
+        }
     }
 
     function aplicarEstilo(el, cor) {
-        el.style.setProperty('background', cor, 'important');
-        el.style.setProperty('background-color', cor, 'important');
-        el.style.setProperty('background-image', 'none', 'important');
-        el.style.setProperty('color', '#ffffff', 'important');
-        el.style.setProperty('font-family', "'Outfit', sans-serif", 'important');
-        el.style.setProperty('font-weight', '600', 'important');
-        el.style.setProperty('text-transform', 'uppercase', 'important');
-        el.style.setProperty('font-size', '0.82rem', 'important');
-        el.style.setProperty('letter-spacing', '0.3px', 'important');
+        try {
+            el.style.setProperty('background', cor, 'important');
+            el.style.setProperty('background-color', cor, 'important');
+            el.style.setProperty('background-image', 'none', 'important');
+            el.style.setProperty('color', '#ffffff', 'important');
+            el.style.setProperty('font-family', "'Outfit', sans-serif", 'important');
+            el.style.setProperty('font-weight', '600', 'important');
+            el.style.setProperty('text-transform', 'uppercase', 'important');
+            el.style.setProperty('font-size', '0.82rem', 'important');
+            el.style.setProperty('letter-spacing', '0.3px', 'important');
+        } catch (erro) {
+            console.error('Erro em aplicarEstilo:', erro);
+        }
     }
 
     function aplicarEstiloHeader() {
-        document.querySelectorAll('thead, thead th').forEach(function(el) {
-
-            var modal = encontrarModalPai(el);
-
-            if (modal) {
-
-                var corModal = obterCorHeaderModal(modal);
-
-                if (corModal) {
-                    var corClara = clarearCor(corModal, 20);
-                    aplicarEstilo(el, corClara);
-                } else {
-
-                    aplicarEstilo(el, corPadrao);
-                }
-            } else {
-
-                aplicarEstilo(el, corPadrao);
+        try {
+            document.querySelectorAll('thead, thead th').forEach(function(el) {
+                try {
+
+                    var modal = encontrarModalPai(el);
+
+                    if (modal) {
+
+                        var corModal = obterCorHeaderModal(modal);
+
+                        if (corModal) {
+                            var corClara = clarearCor(corModal, 20);
+                            aplicarEstilo(el, corClara);
+                        } else {
+
+                            aplicarEstilo(el, corPadrao);
+                        }
+                    } else {
+
+                        aplicarEstilo(el, corPadrao);
+                    }
+                } catch (erroEl) {
+                    console.error('Erro ao processar elemento em aplicarEstiloHeader:', erroEl);
+                }
+            });
+        } catch (erro) {
+            console.error('Erro em aplicarEstiloHeader:', erro);
+        }
+    }
+
+    if (document.readyState === 'loading') {
+        document.addEventListener('DOMContentLoaded', function() {
+            try {
+                aplicarEstiloHeader();
+            } catch (erro) {
+                console.error('Erro no DOMContentLoaded listener:', erro);
             }
         });
-    }
-
-    if (document.readyState === 'loading') {
-        document.addEventListener('DOMContentLoaded', aplicarEstiloHeader);
     } else {
         aplicarEstiloHeader();
     }
 
     window.addEventListener('load', function() {
-        aplicarEstiloHeader();
-
-        setTimeout(aplicarEstiloHeader, 500);
+        try {
+            aplicarEstiloHeader();
+
+            setTimeout(function() {
+                try {
+                    aplicarEstiloHeader();
+                } catch (erro) {
+                    console.error('Erro no setTimeout de window load:', erro);
+                }
+            }, 500);
+        } catch (erro) {
+            console.error('Erro no window load listener:', erro);
+        }
     });
 
     var observer = new MutationObserver(function(mutations) {
-        mutations.forEach(function(mutation) {
-            if (mutation.addedNodes.length > 0) {
-                aplicarEstiloHeader();
-            }
-        });
+        try {
+            mutations.forEach(function(mutation) {
+                try {
+                    if (mutation.addedNodes.length > 0) {
+                        aplicarEstiloHeader();
+                    }
+                } catch (erroMutation) {
+                    console.error('Erro ao processar mutation:', erroMutation);
+                }
+            });
+        } catch (erro) {
+            console.error('Erro no MutationObserver callback:', erro);
+        }
     });
 
     if (document.body) {
-        observer.observe(document.body, { childList: true, subtree: true });
+        try {
+            observer.observe(document.body, { childList: true, subtree: true });
+        } catch (erro) {
+            console.error('Erro ao iniciar MutationObserver:', erro);
+        }
     } else {
         document.addEventListener('DOMContentLoaded', function() {
-            observer.observe(document.body, { childList: true, subtree: true });
+            try {
+                observer.observe(document.body, { childList: true, subtree: true });
+            } catch (erro) {
+                console.error('Erro ao iniciar MutationObserver no DOMContentLoaded:', erro);
+            }
         });
     }
 
```
