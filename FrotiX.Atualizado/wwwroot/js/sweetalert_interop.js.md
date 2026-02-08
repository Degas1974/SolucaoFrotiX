# wwwroot/js/sweetalert_interop.js

**Mudanca:** GRANDE | **+345** linhas | **-239** linhas

---

```diff
--- JANEIRO: wwwroot/js/sweetalert_interop.js
+++ ATUAL: wwwroot/js/sweetalert_interop.js
@@ -1,24 +1,201 @@
 window.SweetAlertInterop = {
     ShowCustomAlert: async function (icon, iconHtml, title, message, confirmButtonText, cancelButtonText = null)
     {
-        const msg = `
+        try
+        {
+            const msg = `
+            <div style="background:#1e1e2f; border-radius: 8px; overflow: hidden; font-family: 'Segoe UI', sans-serif; color: #e0e0e0;">
+              <div style="background:#2d2d4d; padding: 20px; text-align: center;">
+              <div style="margin-bottom: 10px;">
+                <div style="display: inline-block; max-width: 200px; width: 100%;">
+                ${iconHtml}
+                </div>
+             </div>
+             <div style="font-size: 20px; color: #c9a8ff; font-weight: bold;">${title}</div>
+            </div>
+
+              <div style="padding: 20px; font-size: 15px; line-height: 1.6; text-align: center; background:#1e1e2f">
+                <p>${message}</p>
+              </div>
+
+              <div style="background:#3b3b5c; padding: 15px; text-align: center;">
+                ${cancelButtonText ? `<button id="btnCancel" style="
+                  background: #555;
+                  border: none;
+                  color: #fff;
+                  padding: 10px 20px;
+                  margin-right: 10px;
+                  font-size: 14px;
+                  border-radius: 5px;
+                  cursor: pointer;
+                ">${cancelButtonText}</button>` : ''}
+
+                <button id="btnConfirm" style="
+                  background: #7b5ae0;
+                  border: none;
+                  color: #fff;
+                  padding: 10px 20px;
+                  font-size: 14px;
+                  border-radius: 5px;
+                  cursor: pointer;
+                ">${confirmButtonText}</button>
+              </div>
+            </div>`;
+
+            return new Promise((resolve) =>
+            {
+                Swal.fire({
+                    showConfirmButton: false,
+                    html: msg,
+                    backdrop: true,
+                    heightAuto: false,
+                    allowOutsideClick: false,
+                    allowEscapeKey: false,
+
+                    focusConfirm: false,
+                    customClass: {
+                        popup: 'swal2-popup swal2-no-border swal2-no-shadow'
+                    },
+                    didOpen: () =>
+                    {
+                        const popup = document.querySelector('.swal2-popup');
+                        if (popup)
+                        {
+                            popup.style.border = 'none';
+                            popup.style.boxShadow = 'none';
+                            popup.style.background = 'transparent';
+                        }
+                        const confirmBtn = document.getElementById('btnConfirm');
+                        if (confirmBtn) confirmBtn.onclick = () => { Swal.close(); resolve(true); };
+                        const cancelBtn = document.getElementById('btnCancel');
+                        if (cancelBtn) cancelBtn.onclick = () => { Swal.close(); resolve(false); };
+                    },
+                    didClose: () =>
+                    {
+
+                    }
+                });
+            });
+        }
+        catch (erro)
+        {
+            console.error('Erro em ShowCustomAlert:', erro);
+            Alerta.TratamentoErroComLinha('sweetalert_interop.js', 'ShowCustomAlert', erro);
+            return false;
+        }
+    },
+
+    ShowInfo: async function (title, text, confirmButtonText = "OK")
+    {
+        try
+        {
+            const iconHtml = '<img src="/images/info_sorridente_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
+            return await this.ShowCustomAlert('info', iconHtml, title, text, confirmButtonText);
+        }
+        catch (erro)
+        {
+            console.error('Erro em ShowInfo:', erro);
+            Alerta.TratamentoErroComLinha('sweetalert_interop.js', 'ShowInfo', erro);
+            return false;
+        }
+    },
+
+    ShowSuccess: async function (title, text, confirmButtonText = "OK")
+    {
+        try
+        {
+            const iconHtml = '<img src="/images/success_oculos_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
+            return await this.ShowCustomAlert('success', iconHtml, title, text, confirmButtonText);
+        }
+        catch (erro)
+        {
+            console.error('Erro em ShowSuccess:', erro);
+            Alerta.TratamentoErroComLinha('sweetalert_interop.js', 'ShowSuccess', erro);
+            return false;
+        }
+    },
+
+    ShowWarning: async function (title, text, confirmButtonText = "OK")
+    {
+        try
+        {
+            const iconSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="48" height="48" viewBox="0 0 72 72" style="display:block;margin:0 auto 12px;">
+                                <circle cx="36" cy="36" r="32" fill="#ffe066" stroke="#fff" stroke-width="4"/>
+                                <rect x="32" y="18" width="8" height="28" rx="4" fill="#222"/>
+                                <circle cx="36" cy="54" r="5" fill="#222"/>
+                            </svg>`;
+            const message = iconSvg + `<div>${text}</div>`;
+            const iconHtml = '<img src="/images/alerta_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
+            return await this.ShowCustomAlert('warning', iconHtml, title, message, confirmButtonText);
+        }
+        catch (erro)
+        {
+            console.error('Erro em ShowWarning:', erro);
+            Alerta.TratamentoErroComLinha('sweetalert_interop.js', 'ShowWarning', erro);
+            return false;
+        }
+    },
+
+    ShowError: async function (title, text, confirmButtonText = "OK")
+    {
+        try
+        {
+            const iconSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="48" height="48" viewBox="0 0 72 72" style="display:block;margin:0 auto 12px;">
+                                <circle cx="36" cy="36" r="32" fill="#ff4040" stroke="#fff" stroke-width="4"/>
+                                <line x1="20" y1="20" x2="52" y2="52" stroke="#ffe066" stroke-width="8" stroke-linecap="round"/>
+                                <line x1="52" y1="20" x2="20" y2="52" stroke="#ffe066" stroke-width="8" stroke-linecap="round"/>
+                            </svg>`;
+            const message = iconSvg + `<div>${text}</div>`;
+            const iconHtml = '<img src="/images/erro_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
+            return await this.ShowCustomAlert('error', iconHtml, title, message, confirmButtonText);
+        }
+        catch (erro)
+        {
+            console.error('Erro em ShowError:', erro);
+            Alerta.TratamentoErroComLinha('sweetalert_interop.js', 'ShowError', erro);
+            return false;
+        }
+    },
+
+    ShowConfirm: async function (title, text, confirmButtonText, cancelButtonText)
+    {
+        try
+        {
+            const iconHtml = '<img src="/images/confirmar_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
+            return await this.ShowCustomAlert('question', iconHtml, title, text, confirmButtonText, cancelButtonText);
+        }
+        catch (erro)
+        {
+            console.error('Erro em ShowConfirm:', erro);
+            Alerta.TratamentoErroComLinha('sweetalert_interop.js', 'ShowConfirm', erro);
+            return false;
+        }
+    },
+
+    ShowConfirm3: async function (title, text, buttonTodos = "Todos", buttonAtual = "Atual", buttonCancel = "Cancelar")
+    {
+        try
+        {
+            const iconHtml = '<img src="/images/confirmar_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
+
+            const msg = `
         <div style="background:#1e1e2f; border-radius: 8px; overflow: hidden; font-family: 'Segoe UI', sans-serif; color: #e0e0e0;">
           <div style="background:#2d2d4d; padding: 20px; text-align: center;">
-          <div style="margin-bottom: 10px;">
-            <div style="display: inline-block; max-width: 200px; width: 100%;">
-            ${iconHtml}
+            <div style="margin-bottom: 10px;">
+              <div style="display: inline-block; max-width: 200px; width: 100%;">
+                ${iconHtml}
+              </div>
             </div>
-         </div>
-         <div style="font-size: 20px; color: #c9a8ff; font-weight: bold;">${title}</div>
-        </div>
+            <div style="font-size: 20px; color: #c9a8ff; font-weight: bold;">${title}</div>
+          </div>
 
           <div style="padding: 20px; font-size: 15px; line-height: 1.6; text-align: center; background:#1e1e2f">
-            <p>${message}</p>
+            <p>${text}</p>
           </div>
 
           <div style="background:#3b3b5c; padding: 15px; text-align: center;">
-            ${cancelButtonText ? `<button id="btnCancel" style="
-              background: #555;
+            <button id="btnTodos" style="
+              background: #4CAF50;
               border: none;
               color: #fff;
               padding: 10px 20px;
@@ -26,217 +203,105 @@
               font-size: 14px;
               border-radius: 5px;
               cursor: pointer;
-            ">${cancelButtonText}</button>` : ''}
-
-            <button id="btnConfirm" style="
-              background: #7b5ae0;
+              transition: background 0.3s;
+            " onmouseover="this.style.background='#45a049'" onmouseout="this.style.background='#4CAF50'">
+              <i class="fa-solid fa-users" style="margin-right: 5px;"></i>${buttonTodos}
+            </button>
+
+            <button id="btnAtual" style="
+              background: #2196F3;
+              border: none;
+              color: #fff;
+              padding: 10px 20px;
+              margin-right: 10px;
+              font-size: 14px;
+              border-radius: 5px;
+              cursor: pointer;
+              transition: background 0.3s;
+            " onmouseover="this.style.background='#0b7dda'" onmouseout="this.style.background='#2196F3'">
+              <i class="fa-solid fa-user" style="margin-right: 5px;"></i>${buttonAtual}
+            </button>
+
+            <button id="btnCancel" style="
+              background: #555;
               border: none;
               color: #fff;
               padding: 10px 20px;
               font-size: 14px;
               border-radius: 5px;
               cursor: pointer;
-            ">${confirmButtonText}</button>
+              transition: background 0.3s;
+            " onmouseover="this.style.background='#333'" onmouseout="this.style.background='#555'">
+              <i class="fa-solid fa-xmark" style="margin-right: 5px;"></i>${buttonCancel}
+            </button>
           </div>
         </div>`;
 
-        return new Promise((resolve) =>
-        {
-            Swal.fire({
-                showConfirmButton: false,
-                html: msg,
-                backdrop: true,
-                heightAuto: false,
-                allowOutsideClick: false,
-                allowEscapeKey: false,
-
-                focusConfirm: false,
-                customClass: {
-                    popup: 'swal2-popup swal2-no-border swal2-no-shadow'
-                },
-                didOpen: () =>
-                {
-                    const popup = document.querySelector('.swal2-popup');
-                    if (popup)
-                    {
-                        popup.style.border = 'none';
-                        popup.style.boxShadow = 'none';
-                        popup.style.background = 'transparent';
-                    }
-                    const confirmBtn = document.getElementById('btnConfirm');
-                    if (confirmBtn) confirmBtn.onclick = () => { Swal.close(); resolve(true); };
-                    const cancelBtn = document.getElementById('btnCancel');
-                    if (cancelBtn) cancelBtn.onclick = () => { Swal.close(); resolve(false); };
-                },
-                didClose: () =>
-                {
-
-                }
+            return new Promise((resolve) =>
+            {
+                Swal.fire({
+                    showConfirmButton: false,
+                    html: msg,
+                    backdrop: true,
+                    heightAuto: false,
+                    allowOutsideClick: false,
+                    allowEscapeKey: false,
+
+                    focusConfirm: false,
+                    customClass: {
+                        popup: 'swal2-popup swal2-no-border swal2-no-shadow'
+                    },
+                    didOpen: () =>
+                    {
+                        const popup = document.querySelector('.swal2-popup');
+                        if (popup)
+                        {
+                            popup.style.border = 'none';
+                            popup.style.boxShadow = 'none';
+                            popup.style.background = 'transparent';
+                        }
+
+                        const btnTodos = document.getElementById('btnTodos');
+                        if (btnTodos) btnTodos.onclick = () =>
+                        {
+                            Swal.close();
+                            resolve("Todos");
+                        };
+
+                        const btnAtual = document.getElementById('btnAtual');
+                        if (btnAtual) btnAtual.onclick = () =>
+                        {
+                            Swal.close();
+                            resolve("Atual");
+                        };
+
+                        const btnCancel = document.getElementById('btnCancel');
+                        if (btnCancel) btnCancel.onclick = () =>
+                        {
+                            Swal.close();
+                            resolve(false);
+                        };
+                    },
+                    didClose: () =>
+                    {
+
+                    }
+                });
             });
-        });
-    },
-
-    ShowInfo: async function (title, text, confirmButtonText = "OK")
-    {
-        const iconHtml = '<img src="/images/info_sorridente_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
-        return await this.ShowCustomAlert('info', iconHtml, title, text, confirmButtonText);
-    },
-
-    ShowSuccess: async function (title, text, confirmButtonText = "OK")
-    {
-        const iconHtml = '<img src="/images/success_oculos_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
-        return await this.ShowCustomAlert('success', iconHtml, title, text, confirmButtonText);
-    },
-
-    ShowWarning: async function (title, text, confirmButtonText = "OK")
-    {
-        const iconSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="48" height="48" viewBox="0 0 72 72" style="display:block;margin:0 auto 12px;">
-                            <circle cx="36" cy="36" r="32" fill="#ffe066" stroke="#fff" stroke-width="4"/>
-                            <rect x="32" y="18" width="8" height="28" rx="4" fill="#222"/>
-                            <circle cx="36" cy="54" r="5" fill="#222"/>
-                        </svg>`;
-        const message = iconSvg + `<div>${text}</div>`;
-        const iconHtml = '<img src="/images/alerta_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
-        return await this.ShowCustomAlert('warning', iconHtml, title, message, confirmButtonText);
-    },
-
-    ShowError: async function (title, text, confirmButtonText = "OK")
-    {
-        const iconSvg = `<svg xmlns="http://www.w3.org/2000/svg" width="48" height="48" viewBox="0 0 72 72" style="display:block;margin:0 auto 12px;">
-                            <circle cx="36" cy="36" r="32" fill="#ff4040" stroke="#fff" stroke-width="4"/>
-                            <line x1="20" y1="20" x2="52" y2="52" stroke="#ffe066" stroke-width="8" stroke-linecap="round"/>
-                            <line x1="52" y1="20" x2="20" y2="52" stroke="#ffe066" stroke-width="8" stroke-linecap="round"/>
-                        </svg>`;
-        const message = iconSvg + `<div>${text}</div>`;
-        const iconHtml = '<img src="/images/erro_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
-        return await this.ShowCustomAlert('error', iconHtml, title, message, confirmButtonText);
-    },
-
-    ShowConfirm: async function (title, text, confirmButtonText, cancelButtonText)
-    {
-        const iconHtml = '<img src="/images/confirmar_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
-        return await this.ShowCustomAlert('question', iconHtml, title, text, confirmButtonText, cancelButtonText);
-    },
-
-    ShowConfirm3: async function (title, text, buttonTodos = "Todos", buttonAtual = "Atual", buttonCancel = "Cancelar")
-    {
-        const iconHtml = '<img src="/images/confirmar_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
-
-        const msg = `
-    <div style="background:#1e1e2f; border-radius: 8px; overflow: hidden; font-family: 'Segoe UI', sans-serif; color: #e0e0e0;">
-      <div style="background:#2d2d4d; padding: 20px; text-align: center;">
-        <div style="margin-bottom: 10px;">
-          <div style="display: inline-block; max-width: 200px; width: 100%;">
-            ${iconHtml}
-          </div>
-        </div>
-        <div style="font-size: 20px; color: #c9a8ff; font-weight: bold;">${title}</div>
-      </div>
-
-      <div style="padding: 20px; font-size: 15px; line-height: 1.6; text-align: center; background:#1e1e2f">
-        <p>${text}</p>
-      </div>
-
-      <div style="background:#3b3b5c; padding: 15px; text-align: center;">
-        <button id="btnTodos" style="
-          background: #4CAF50;
-          border: none;
-          color: #fff;
-          padding: 10px 20px;
-          margin-right: 10px;
-          font-size: 14px;
-          border-radius: 5px;
-          cursor: pointer;
-          transition: background 0.3s;
-        " onmouseover="this.style.background='#45a049'" onmouseout="this.style.background='#4CAF50'">
-          <i class="fa-solid fa-users" style="margin-right: 5px;"></i>${buttonTodos}
-        </button>
-
-        <button id="btnAtual" style="
-          background: #2196F3;
-          border: none;
-          color: #fff;
-          padding: 10px 20px;
-          margin-right: 10px;
-          font-size: 14px;
-          border-radius: 5px;
-          cursor: pointer;
-          transition: background 0.3s;
-        " onmouseover="this.style.background='#0b7dda'" onmouseout="this.style.background='#2196F3'">
-          <i class="fa-solid fa-user" style="margin-right: 5px;"></i>${buttonAtual}
-        </button>
-
-        <button id="btnCancel" style="
-          background: #555;
-          border: none;
-          color: #fff;
-          padding: 10px 20px;
-          font-size: 14px;
-          border-radius: 5px;
-          cursor: pointer;
-          transition: background 0.3s;
-        " onmouseover="this.style.background='#333'" onmouseout="this.style.background='#555'">
-          <i class="fa-solid fa-xmark" style="margin-right: 5px;"></i>${buttonCancel}
-        </button>
-      </div>
-    </div>`;
-
-        return new Promise((resolve) =>
-        {
-            Swal.fire({
-                showConfirmButton: false,
-                html: msg,
-                backdrop: true,
-                heightAuto: false,
-                allowOutsideClick: false,
-                allowEscapeKey: false,
-
-                focusConfirm: false,
-                customClass: {
-                    popup: 'swal2-popup swal2-no-border swal2-no-shadow'
-                },
-                didOpen: () =>
-                {
-                    const popup = document.querySelector('.swal2-popup');
-                    if (popup)
-                    {
-                        popup.style.border = 'none';
-                        popup.style.boxShadow = 'none';
-                        popup.style.background = 'transparent';
-                    }
-
-                    const btnTodos = document.getElementById('btnTodos');
-                    if (btnTodos) btnTodos.onclick = () =>
-                    {
-                        Swal.close();
-                        resolve("Todos");
-                    };
-
-                    const btnAtual = document.getElementById('btnAtual');
-                    if (btnAtual) btnAtual.onclick = () =>
-                    {
-                        Swal.close();
-                        resolve("Atual");
-                    };
-
-                    const btnCancel = document.getElementById('btnCancel');
-                    if (btnCancel) btnCancel.onclick = () =>
-                    {
-                        Swal.close();
-                        resolve(false);
-                    };
-                },
-                didClose: () =>
-                {
-
-                }
-            });
-        });
+        }
+        catch (erro)
+        {
+            console.error('Erro em ShowConfirm3:', erro);
+            Alerta.TratamentoErroComLinha('sweetalert_interop.js', 'ShowConfirm3', erro);
+            return false;
+        }
     },
 
     ShowErrorUnexpected: async function (classe, metodo, erro)
     {
-        console.log('=== ShowErrorUnexpected INICIADO ===');
+        try
+        {
+            console.log('=== ShowErrorUnexpected INICIADO ===');
         console.log('Classe:', classe);
         console.log('Método:', metodo);
         console.log('Erro:', erro);
@@ -551,54 +616,81 @@
         console.log('Inner erro presente?', !!innerErro);
         console.log('=== ShowErrorUnexpected EXIBINDO ALERTA ===');
 
-        return await this.ShowCustomAlert('error', iconHtml, title, message, "OK");
+            return await this.ShowCustomAlert('error', iconHtml, title, message, "OK");
+        }
+        catch (erroInterno)
+        {
+            console.error('Erro CRÍTICO em ShowErrorUnexpected:', erroInterno);
+
+            alert('Erro crítico ao exibir mensagem de erro: ' + (erroInterno.message || erroInterno));
+            return false;
+        }
     },
 
     ShowPreventionAlert: async function (message)
     {
-        const iconHtml = '<img src="/images/confirmar_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
-        const title = 'Atenção ao Preenchimento dos Dados';
-        const confirmText = 'Tenho certeza! 💪🏼';
-        const cancelText = 'Me enganei! 😟';
-        const confirmado = await this.ShowCustomAlert('warning', iconHtml, title, message, confirmText, cancelText);
-        return confirmado;
+        try
+        {
+            const iconHtml = '<img src="/images/confirmar_transparente.png" style="max-width: 150px; width: 100%; height: auto; margin-bottom: 10px;">';
+            const title = 'Atenção ao Preenchimento dos Dados';
+            const confirmText = 'Tenho certeza! 💪🏼';
+            const cancelText = 'Me enganei! 😟';
+            const confirmado = await this.ShowCustomAlert('warning', iconHtml, title, message, confirmText, cancelText);
+            return confirmado;
+        }
+        catch (erro)
+        {
+            console.error('Erro em ShowPreventionAlert:', erro);
+            Alerta.TratamentoErroComLinha('sweetalert_interop.js', 'ShowPreventionAlert', erro);
+            return false;
+        }
     },
 
     ShowNotification: function (message, color = "#28a745")
     {
-        let notify = document.getElementById("sweet-alert-notify");
-        if (!notify)
-        {
-            notify = document.createElement("div");
-            notify.id = "sweet-alert-notify";
-            notify.style.position = "fixed";
-            notify.style.top = "20px";
-            notify.style.right = "20px";
-            notify.style.zIndex = "10000";
-            notify.style.minWidth = "200px";
-            notify.style.padding = "12px 20px";
-            notify.style.borderRadius = "8px";
-            notify.style.fontSize = "16px";
-            notify.style.fontFamily = "'Segoe UI', sans-serif";
-            notify.style.color = "white";
-            notify.style.display = "none";
-            document.body.appendChild(notify);
-        }
-
-        notify.textContent = message;
-        notify.style.backgroundColor = color;
-        notify.style.display = "block";
-
-        setTimeout(() =>
-        {
-            notify.style.display = "none";
-        }, 3000);
+        try
+        {
+            let notify = document.getElementById("sweet-alert-notify");
+            if (!notify)
+            {
+                notify = document.createElement("div");
+                notify.id = "sweet-alert-notify";
+                notify.style.position = "fixed";
+                notify.style.top = "20px";
+                notify.style.right = "20px";
+                notify.style.zIndex = "10000";
+                notify.style.minWidth = "200px";
+                notify.style.padding = "12px 20px";
+                notify.style.borderRadius = "8px";
+                notify.style.fontSize = "16px";
+                notify.style.fontFamily = "'Segoe UI', sans-serif";
+                notify.style.color = "white";
+                notify.style.display = "none";
+                document.body.appendChild(notify);
+            }
+
+            notify.textContent = message;
+            notify.style.backgroundColor = color;
+            notify.style.display = "block";
+
+            setTimeout(() =>
+            {
+                notify.style.display = "none";
+            }, 3000);
+        }
+        catch (erro)
+        {
+            console.error('Erro em ShowNotification:', erro);
+            Alerta.TratamentoErroComLinha('sweetalert_interop.js', 'ShowNotification', erro);
+        }
     },
 
     ShowValidacaoIAConfirmar: async function (titulo, mensagem, confirmButtonText = "Confirmar", cancelButtonText = "Corrigir")
     {
-
-        const mensagemFormatada = mensagem.replace(/\n/g, '<br>');
+        try
+        {
+
+            const mensagemFormatada = mensagem.replace(/\n/g, '<br>');
 
         const iconHtml = '<img src="/images/alerta_transparente.png" style="max-width: 120px; width: 100%; height: auto; margin-bottom: 10px;">';
 
@@ -700,32 +792,47 @@
                 }
             });
         });
+        }
+        catch (erro)
+        {
+            console.error('Erro em ShowValidacaoIAConfirmar:', erro);
+            Alerta.TratamentoErroComLinha('sweetalert_interop.js', 'ShowValidacaoIAConfirmar', erro);
+            return false;
+        }
     }
 };
 
 function limparResiduosModalVanilla()
 {
-    document.querySelectorAll('.swal2-container, .swal2-backdrop-show').forEach(e => e.remove());
-
-    document.querySelectorAll('div').forEach(div =>
-    {
-        const style = getComputedStyle(div);
-        if (
-            (style.position === 'fixed' || style.position === 'absolute') &&
-            parseInt(style.zIndex || 0) >= 2000 &&
-            (parseInt(style.width) === window.innerWidth || style.width === '100vw' || style.left === '0px') &&
-            (parseInt(style.height) === window.innerHeight || style.height === '100vh' || style.top === '0px')
-        )
-        {
+    try
+    {
+        document.querySelectorAll('.swal2-container, .swal2-backdrop-show').forEach(e => e.remove());
+
+        document.querySelectorAll('div').forEach(div =>
+        {
+            const style = getComputedStyle(div);
             if (
-                !div.classList.contains('fc') &&
-                !div.classList.contains('fc-view-harness') &&
-                !div.classList.contains('modal-backdrop') &&
-                !div.closest('.modal')
+                (style.position === 'fixed' || style.position === 'absolute') &&
+                parseInt(style.zIndex || 0) >= 2000 &&
+                (parseInt(style.width) === window.innerWidth || style.width === '100vw' || style.left === '0px') &&
+                (parseInt(style.height) === window.innerHeight || style.height === '100vh' || style.top === '0px')
             )
             {
-                div.remove();
-            }
-        }
-    });
+                if (
+                    !div.classList.contains('fc') &&
+                    !div.classList.contains('fc-view-harness') &&
+                    !div.classList.contains('modal-backdrop') &&
+                    !div.closest('.modal')
+                )
+                {
+                    div.remove();
+                }
+            }
+        });
+    }
+    catch (erro)
+    {
+        console.error('Erro em limparResiduosModalVanilla:', erro);
+        Alerta.TratamentoErroComLinha('sweetalert_interop.js', 'limparResiduosModalVanilla', erro);
+    }
 }
```
