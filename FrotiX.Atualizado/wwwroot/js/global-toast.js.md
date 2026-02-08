# wwwroot/js/global-toast.js

**Mudanca:** GRANDE | **+265** linhas | **-179** linhas

---

```diff
--- JANEIRO: wwwroot/js/global-toast.js
+++ ATUAL: wwwroot/js/global-toast.js
@@ -30,218 +30,304 @@
 
     function getContainer()
     {
-        if (!container)
-        {
-            container = document.createElement('div');
-            container.id = 'app-toast-container';
-            container.style.cssText = `
+        try
+        {
+            if (!container)
+            {
+                container = document.createElement('div');
+                container.id = 'app-toast-container';
+                container.style.cssText = `
+                    position: fixed;
+                    top: 20px;
+                    right: 20px;
+                    z-index: 100000;
+                    pointer-events: none;
+                `;
+                document.body.appendChild(container);
+            }
+            return container;
+        }
+        catch (erro)
+        {
+            console.error('Erro em getContainer:', erro);
+            return null;
+        }
+    }
+
+    function sanitizeText(text)
+    {
+        try
+        {
+            return String(text || '')
+                .replace(/&/g, '&amp;')
+                .replace(/</g, '&lt;')
+                .replace(/>/g, '&gt;')
+                .replace(/"/g, '&quot;')
+                .replace(/'/g, '&#39;');
+        }
+        catch (erro)
+        {
+            console.error('Erro em sanitizeText:', erro);
+            return '';
+        }
+    }
+
+    function clearTimers()
+    {
+        try
+        {
+            if (closeTimer)
+            {
+                clearTimeout(closeTimer);
+                closeTimer = null;
+            }
+
+            if (animationFrameId)
+            {
+                cancelAnimationFrame(animationFrameId);
+                animationFrameId = null;
+            }
+        }
+        catch (erro)
+        {
+            console.error('Erro em clearTimers:', erro);
+        }
+    }
+
+    function close()
+    {
+        try
+        {
+            clearTimers();
+
+            if (currentToast)
+            {
+                currentToast.style.animation = 'slideOutRight 0.4s ease forwards';
+
+                setTimeout(() =>
+                {
+                    try
+                    {
+                        if (currentToast && currentToast.parentNode)
+                        {
+                            currentToast.parentNode.removeChild(currentToast);
+                        }
+                        currentToast = null;
+                    }
+                    catch (erro)
+                    {
+                        console.error('Erro no setTimeout de close:', erro);
+                    }
+                }, 400);
+            }
+        }
+        catch (erro)
+        {
+            console.error('Erro em close:', erro);
+        }
+    }
+
+    function show(estilo, mensagem, duracaoMs)
+    {
+        try
+        {
+
+            close();
+
+            const timeout = Number.isFinite(duracaoMs) ? Math.max(0, duracaoMs) : 3000;
+            const style = STYLE_MAP[estilo] || STYLE_MAP["Amarelo"];
+            const text = sanitizeText(mensagem);
+            const progressId = 'app-toast-progress-' + Date.now() + '-' + Math.random().toString(36).substr(2, 9);
+
+            console.log(`%c[AppToast] Mostrando toast "${estilo}" por ${timeout}ms`, 'color: #4caf50; font-weight: bold;');
+
+            const toast = document.createElement('div');
+            toast.className = 'app-toast-item';
+            toast.style.cssText = `
+                background: ${style.gradient};
+                min-width: 380px;
+                max-width: 480px;
+                border-radius: 12px;
+                box-shadow: 0 8px 24px rgba(0,0,0,0.15);
+                overflow: hidden;
+                margin-bottom: 12px;
+                pointer-events: auto;
+                animation: slideInRight 0.4s ease forwards;
+                cursor: pointer;
+            `;
+
+            toast.innerHTML = `
+                <div style="display:flex;align-items:center;gap:16px;padding:16px 20px;">
+                    ${style.icon}
+                    <div style="flex:1;display:flex;flex-direction:column;gap:8px;">
+                        <div style="font-size:16px;font-weight:700;line-height:1.4;color:#fff;">${text}</div>
+                        <div style="position:relative;width:100%;height:4px;background:rgba(255,255,255,0.3);border-radius:4px;overflow:hidden;">
+                            <div id="${progressId}" style="height:100%;width:100%;background:#fff;transform-origin:left;transform:scaleX(1);transition:none;"></div>
+                        </div>
+                    </div>
+                </div>
+            `;
+
+            const cont = getContainer();
+            cont.appendChild(toast);
+            currentToast = toast;
+
+            if (timeout > 0)
+            {
+                const progressBar = document.getElementById(progressId);
+                const startTime = performance.now();
+
+                function animateProgress(currentTime)
+                {
+                    try
+                    {
+                        const elapsed = currentTime - startTime;
+                        const progress = Math.max(0, 1 - (elapsed / timeout));
+
+                        if (progressBar)
+                        {
+                            progressBar.style.transform = `scaleX(${progress})`;
+                        }
+
+                        if (progress > 0)
+                        {
+                            animationFrameId = requestAnimationFrame(animateProgress);
+                        }
+                        else
+                        {
+                            animationFrameId = null;
+                        }
+                    }
+                    catch (erro)
+                    {
+                        console.error('Erro em animateProgress:', erro);
+                    }
+                }
+
+                animationFrameId = requestAnimationFrame(animateProgress);
+
+                closeTimer = setTimeout(() =>
+                {
+                    try
+                    {
+                        console.log(`%c[AppToast] Fechando toast após ${timeout}ms`, 'color: #f44336; font-weight: bold;');
+                        close();
+                    }
+                    catch (erro)
+                    {
+                        console.error('Erro no setTimeout de show:', erro);
+                    }
+                }, timeout);
+            }
+
+            toast.addEventListener('click', () =>
+            {
+                try
+                {
+                    console.log('[AppToast] Toast fechado por clique');
+                    close();
+                }
+                catch (erro)
+                {
+                    console.error('Erro no click handler do toast:', erro);
+                }
+            });
+        }
+        catch (erro)
+        {
+            console.error('Erro em show:', erro);
+        }
+    }
+
+    function setPosition(x, y)
+    {
+        try
+        {
+            const cont = getContainer();
+
+            const horizontalPositions = {
+                'Right': 'right: 20px; left: auto;',
+                'Left': 'left: 20px; right: auto;',
+                'Center': 'left: 50%; transform: translateX(-50%);'
+            };
+
+            const verticalPositions = {
+                'Top': 'top: 20px; bottom: auto;',
+                'Bottom': 'bottom: 20px; top: auto;'
+            };
+
+            cont.style.cssText = `
                 position: fixed;
-                top: 20px;
-                right: 20px;
                 z-index: 100000;
                 pointer-events: none;
+                ${horizontalPositions[x] || horizontalPositions['Right']}
+                ${verticalPositions[y] || verticalPositions['Top']}
             `;
-            document.body.appendChild(container);
-        }
-        return container;
-    }
-
-    function sanitizeText(text)
-    {
-        return String(text || '')
-            .replace(/&/g, '&amp;')
-            .replace(/</g, '&lt;')
-            .replace(/>/g, '&gt;')
-            .replace(/"/g, '&quot;')
-            .replace(/'/g, '&#39;');
-    }
-
-    function clearTimers()
-    {
-        if (closeTimer)
-        {
-            clearTimeout(closeTimer);
-            closeTimer = null;
-        }
-
-        if (animationFrameId)
-        {
-            cancelAnimationFrame(animationFrameId);
-            animationFrameId = null;
-        }
-    }
-
-    function close()
-    {
-        clearTimers();
-
-        if (currentToast)
-        {
-            currentToast.style.animation = 'slideOutRight 0.4s ease forwards';
-
-            setTimeout(() =>
-            {
-                if (currentToast && currentToast.parentNode)
-                {
-                    currentToast.parentNode.removeChild(currentToast);
-                }
-                currentToast = null;
-            }, 400);
-        }
-    }
-
-    function show(estilo, mensagem, duracaoMs)
-    {
-
-        close();
-
-        const timeout = Number.isFinite(duracaoMs) ? Math.max(0, duracaoMs) : 3000;
-        const style = STYLE_MAP[estilo] || STYLE_MAP["Amarelo"];
-        const text = sanitizeText(mensagem);
-        const progressId = 'app-toast-progress-' + Date.now() + '-' + Math.random().toString(36).substr(2, 9);
-
-        console.log(`%c[AppToast] Mostrando toast "${estilo}" por ${timeout}ms`, 'color: #4caf50; font-weight: bold;');
-
-        const toast = document.createElement('div');
-        toast.className = 'app-toast-item';
-        toast.style.cssText = `
-            background: ${style.gradient};
-            min-width: 380px;
-            max-width: 480px;
-            border-radius: 12px;
-            box-shadow: 0 8px 24px rgba(0,0,0,0.15);
-            overflow: hidden;
-            margin-bottom: 12px;
-            pointer-events: auto;
-            animation: slideInRight 0.4s ease forwards;
-            cursor: pointer;
-        `;
-
-        toast.innerHTML = `
-            <div style="display:flex;align-items:center;gap:16px;padding:16px 20px;">
-                ${style.icon}
-                <div style="flex:1;display:flex;flex-direction:column;gap:8px;">
-                    <div style="font-size:16px;font-weight:700;line-height:1.4;color:#fff;">${text}</div>
-                    <div style="position:relative;width:100%;height:4px;background:rgba(255,255,255,0.3);border-radius:4px;overflow:hidden;">
-                        <div id="${progressId}" style="height:100%;width:100%;background:#fff;transform-origin:left;transform:scaleX(1);transition:none;"></div>
-                    </div>
-                </div>
-            </div>
-        `;
-
-        const cont = getContainer();
-        cont.appendChild(toast);
-        currentToast = toast;
-
-        if (timeout > 0)
-        {
-            const progressBar = document.getElementById(progressId);
-            const startTime = performance.now();
-
-            function animateProgress(currentTime)
-            {
-                const elapsed = currentTime - startTime;
-                const progress = Math.max(0, 1 - (elapsed / timeout));
-
-                if (progressBar)
-                {
-                    progressBar.style.transform = `scaleX(${progress})`;
-                }
-
-                if (progress > 0)
-                {
-                    animationFrameId = requestAnimationFrame(animateProgress);
-                }
-                else
-                {
-                    animationFrameId = null;
-                }
-            }
-
-            animationFrameId = requestAnimationFrame(animateProgress);
-
-            closeTimer = setTimeout(() =>
-            {
-                console.log(`%c[AppToast] Fechando toast após ${timeout}ms`, 'color: #f44336; font-weight: bold;');
+        }
+        catch (erro)
+        {
+            console.error('Erro em setPosition:', erro);
+        }
+    }
+
+    function addStyles()
+    {
+        try
+        {
+            if (!document.getElementById('app-toast-styles'))
+            {
+                const style = document.createElement('style');
+                style.id = 'app-toast-styles';
+                style.textContent = `
+                    @keyframes slideInRight {
+                        from {
+                            opacity: 0;
+                            transform: translateX(100%);
+                        }
+                        to {
+                            opacity: 1;
+                            transform: translateX(0);
+                        }
+                    }
+
+                    @keyframes slideOutRight {
+                        from {
+                            opacity: 1;
+                            transform: translateX(0);
+                        }
+                        to {
+                            opacity: 0;
+                            transform: translateX(100%);
+                        }
+                    }
+
+                    .app-toast-item:hover {
+                        box-shadow: 0 12px 32px rgba(0,0,0,0.2) !important;
+                        transform: translateY(-2px);
+                        transition: all 0.2s ease;
+                    }
+                `;
+                document.head.appendChild(style);
+            }
+        }
+        catch (erro)
+        {
+            console.error('Erro em addStyles:', erro);
+        }
+    }
+
+    document.addEventListener('keydown', (e) =>
+    {
+        try
+        {
+            if (e.key === 'Escape')
+            {
                 close();
-            }, timeout);
-        }
-
-        toast.addEventListener('click', () =>
-        {
-            console.log('[AppToast] Toast fechado por clique');
-            close();
-        });
-    }
-
-    function setPosition(x, y)
-    {
-        const cont = getContainer();
-
-        const horizontalPositions = {
-            'Right': 'right: 20px; left: auto;',
-            'Left': 'left: 20px; right: auto;',
-            'Center': 'left: 50%; transform: translateX(-50%);'
-        };
-
-        const verticalPositions = {
-            'Top': 'top: 20px; bottom: auto;',
-            'Bottom': 'bottom: 20px; top: auto;'
-        };
-
-        cont.style.cssText = `
-            position: fixed;
-            z-index: 100000;
-            pointer-events: none;
-            ${horizontalPositions[x] || horizontalPositions['Right']}
-            ${verticalPositions[y] || verticalPositions['Top']}
-        `;
-    }
-
-    function addStyles()
-    {
-        if (!document.getElementById('app-toast-styles'))
-        {
-            const style = document.createElement('style');
-            style.id = 'app-toast-styles';
-            style.textContent = `
-                @keyframes slideInRight {
-                    from {
-                        opacity: 0;
-                        transform: translateX(100%);
-                    }
-                    to {
-                        opacity: 1;
-                        transform: translateX(0);
-                    }
-                }
-
-                @keyframes slideOutRight {
-                    from {
-                        opacity: 1;
-                        transform: translateX(0);
-                    }
-                    to {
-                        opacity: 0;
-                        transform: translateX(100%);
-                    }
-                }
-
-                .app-toast-item:hover {
-                    box-shadow: 0 12px 32px rgba(0,0,0,0.2) !important;
-                    transform: translateY(-2px);
-                    transition: all 0.2s ease;
-                }
-            `;
-            document.head.appendChild(style);
-        }
-    }
-
-    document.addEventListener('keydown', (e) =>
-    {
-        if (e.key === 'Escape')
-        {
-            close();
+            }
+        }
+        catch (erro)
+        {
+            console.error('Erro no event listener keydown (ESC):', erro);
         }
     });
 
```
