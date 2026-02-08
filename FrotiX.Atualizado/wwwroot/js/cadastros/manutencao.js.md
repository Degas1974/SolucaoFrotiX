# wwwroot/js/cadastros/manutencao.js

**Mudanca:** GRANDE | **+2095** linhas | **-2276** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/manutencao.js
+++ ATUAL: wwwroot/js/cadastros/manutencao.js
@@ -1,145 +1,153 @@
-var ManutencaoId = '';
+var ManutencaoId = "";
 var dataTableOcorrencias;
 var dataTablePendencias;
 var defaultRTE;
-var ImagemSelecionada = 'semimagem.jpg';
+var ImagemSelecionada = "semimagem.jpg";
 var dataTableItens;
 
 var linhaSelecionadaFoto = -1;
 
 window.modoVisualizacaoFoto = false;
 
-document.getElementById('txtFileItem').addEventListener('change', async (e) => {
-    const file = e.target.files?.[0];
-    if (!file) return;
-
-    imgViewerItem.src = URL.createObjectURL(file);
-
-    const token =
-        document.querySelector('meta[name="request-verification-token"]')
-            ?.content ||
-        document.querySelector(
-            '#uploadForm input[name="__RequestVerificationToken"]',
-        )?.value;
-
-    console.log('Anti-forgery token:', token?.slice(0, 12) + '...');
-    if (!token) {
-        alert('Token antiforgery não encontrado na página.');
-        return;
-    }
-
-    const fd = new FormData();
-    fd.append('files', file, file.name);
-    fd.append('__RequestVerificationToken', token);
-
-    const resp = await fetch('/Uploads/UploadPDF?handler=SaveIMGManutencao', {
-        method: 'POST',
-        body: fd,
-        headers: { 'X-CSRF-TOKEN': token },
-        credentials: 'same-origin',
+document.getElementById("txtFileItem").addEventListener("change", async (e) =>
+{
+    try
+    {
+        const file = e.target.files?.[0];
+        if (!file) return;
+
+        imgViewerItem.src = URL.createObjectURL(file);
+
+        const token =
+            document.querySelector('meta[name="request-verification-token"]')?.content ||
+            document.querySelector('#uploadForm input[name="__RequestVerificationToken"]')?.value;
+
+        console.log("Anti-forgery token:", token?.slice(0, 12) + "...");
+        if (!token) { alert("Token antiforgery não encontrado na página."); return; }
+
+        const fd = new FormData();
+        fd.append("files", file, file.name);
+        fd.append("__RequestVerificationToken", token);
+
+        const resp = await fetch("/Uploads/UploadPDF?handler=SaveIMGManutencao", {
+            method: "POST",
+            body: fd,
+            headers: { "X-CSRF-TOKEN": token },
+            credentials: "same-origin"
     });
 
-    if (!resp.ok) {
+    if (!resp.ok)
+    {
         const txt = await resp.text();
-        throw new Error('Falha no upload: ' + resp.status + ' - ' + txt);
+        throw new Error("Falha no upload: " + resp.status + " - " + txt);
     }
 
     const data = await resp.json();
     window.ImagemSelecionada = data.fileName;
 });
 
-document.addEventListener(
-    'click',
-    function (e) {
-        try {
-            const toggler = e.target.closest('[data-bs-toggle="modal"]');
-            if (!toggler || !window.bootstrap || !bootstrap.Modal) return;
-
-            const opened = document.querySelector('.modal.show');
-            if (!opened) return;
-
-            const targetSelector = toggler.getAttribute('data-bs-target');
-            if (targetSelector && opened.matches(targetSelector)) return;
-
-            const inst =
-                bootstrap.Modal.getInstance(opened) ||
-                bootstrap.Modal.getOrCreateInstance(opened);
-            if (inst && typeof inst.hide === 'function') {
-                inst.hide();
-            }
-        } catch (error) {
-            console.error('Erro ao fechar modal:', error);
-        }
-    },
-    true,
-);
-
-document.querySelectorAll('.modal').forEach(function (el) {
-    if (window.bootstrap?.Modal) {
-        bootstrap.Modal.getInstance(el) ||
-            bootstrap.Modal.getOrCreateInstance(el, {
-                backdrop: true,
-                keyboard: true,
-            });
+document.addEventListener('click', function (e)
+{
+    try
+    {
+        const toggler = e.target.closest('[data-bs-toggle="modal"]');
+        if (!toggler || !window.bootstrap || !bootstrap.Modal) return;
+
+        const opened = document.querySelector('.modal.show');
+        if (!opened) return;
+
+        const targetSelector = toggler.getAttribute('data-bs-target');
+        if (targetSelector && opened.matches(targetSelector)) return;
+
+        const inst = bootstrap.Modal.getInstance(opened) || bootstrap.Modal.getOrCreateInstance(opened);
+        if (inst && typeof inst.hide === 'function')
+        {
+            inst.hide();
+        }
+    }
+    catch (error)
+    {
+        console.error("Erro ao fechar modal:", error);
+    }
+}, true);
+
+document.querySelectorAll('.modal').forEach(function (el)
+{
+    if (window.bootstrap?.Modal)
+    {
+        bootstrap.Modal.getInstance(el) || bootstrap.Modal.getOrCreateInstance(el, { backdrop: true, keyboard: true });
     }
 });
 
-document.addEventListener('DOMContentLoaded', function () {
-    try {
+document.addEventListener('DOMContentLoaded', function()
+{
+    try
+    {
         const btnAdicionaItem = document.getElementById('btnAdicionaItem');
-        if (btnAdicionaItem) {
-            btnAdicionaItem.addEventListener('click', function (e) {
-                try {
+        if (btnAdicionaItem)
+        {
+            btnAdicionaItem.addEventListener('click', function(e)
+            {
+                try
+                {
                     const modalEl = document.getElementById('modalManutencao');
-                    if (modalEl && window.bootstrap?.Modal) {
-                        const modal =
-                            bootstrap.Modal.getOrCreateInstance(modalEl);
-                        if (modal) {
+                    if (modalEl && window.bootstrap?.Modal)
+                    {
+                        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
+                        if (modal)
+                        {
                             modal.show();
                         }
                     }
-                } catch (error) {
-                    console.error('Erro ao abrir modalManutencao:', error);
-                    Alerta.TratamentoErroComLinha(
-                        'manutencao.js',
-                        'click.btnAdicionaItem',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    console.error("Erro ao abrir modalManutencao:", error);
+                    Alerta.TratamentoErroComLinha("manutencao.js", "click.btnAdicionaItem", error);
                 }
             });
         }
-    } catch (error) {
-        console.error('Erro ao configurar btnAdicionaItem:', error);
+    }
+    catch (error)
+    {
+        console.error("Erro ao configurar btnAdicionaItem:", error);
     }
 });
 
-function hojeLocalYYYYMMDD() {
-    try {
+function hojeLocalYYYYMMDD()
+{
+    try
+    {
         const d = new Date();
-        const pad = (n) => {
-            try {
-                return String(n).padStart(2, '0');
-            } catch (error) {
-                Alerta.TratamentoErroComLinha('manutencao.js', 'pad', error);
+        const pad = (n) =>
+        {
+            try
+            {
+                return String(n).padStart(2, "0");
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("manutencao.js", "pad", error);
             }
         };
         return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'manutencao.js',
-            'hojeLocalYYYYMMDD',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("manutencao.js", "hojeLocalYYYYMMDD", error);
     }
 }
 
-function normalizaInputDate($input) {
-    try {
-        const raw = ($input && $input.val ? $input.val() : '').trim();
+function normalizaInputDate($input)
+{
+    try
+    {
+        const raw = ($input && $input.val ? $input.val() : "").trim();
         if (!raw) return;
 
         const ddmmyyyy = raw.match(/^(\d{2})\/(\d{2})\/(\d{4})$/);
-        if (ddmmyyyy) {
+        if (ddmmyyyy)
+        {
             const [, dd, mm, yyyy] = ddmmyyyy;
             $input.val(`${yyyy}-${mm}-${dd}`);
             return;
@@ -148,555 +156,536 @@
         if (/^\d{4}-\d{2}-\d{2}$/.test(raw)) return;
 
         const d = new Date(raw);
-        if (!isNaN(d)) {
-            const pad = (n) => {
-                try {
-                    return String(n).padStart(2, '0');
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'manutencao.js',
-                        'pad',
-                        error,
-                    );
+        if (!isNaN(d))
+        {
+            const pad = (n) =>
+            {
+                try
+                {
+                    return String(n).padStart(2, "0");
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("manutencao.js", "pad", error);
                 }
             };
-            $input.val(
-                `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`,
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'manutencao.js',
-            'normalizaInputDate',
-            error,
-        );
+            $input.val(`${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`);
+        }
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("manutencao.js", "normalizaInputDate", error);
     }
 }
 
-document.addEventListener('DOMContentLoaded', function () {
-    try {
+document.addEventListener("DOMContentLoaded", function ()
+{
+    try
+    {
         const popoverTriggerList = [].slice.call(
             document.querySelectorAll('[data-bs-toggle="popover"]'),
         );
-        popoverTriggerList.forEach(function (el) {
-            try {
+        popoverTriggerList.forEach(function (el)
+        {
+            try
+            {
                 new bootstrap.Popover(el);
-            } catch (error) {
+            }
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha(
-                    'manutencao.js',
-                    'callback@popoverTriggerList.forEach#0',
+                    "manutencao.js",
+                    "callback@popoverTriggerList.forEach#0",
                     error,
                 );
             }
         });
 
         const $ = window.jQuery;
-        const $ds = $('#txtDataSolicitacao');
-
-        function toYMD(value) {
-            try {
-                if (!value) return '';
+        const $ds = $("#txtDataSolicitacao");
+
+        function toYMD(value)
+        {
+            try
+            {
+                if (!value) return "";
                 const v = String(value).trim();
-                if (/^\d{2}\/\d{2}\/\d{4}$/.test(v)) {
-                    const [d, m, y] = v.split('/');
+                if (/^\d{2}\/\d{2}\/\d{4}$/.test(v))
+                {
+                    const [d, m, y] = v.split("/");
                     return `${y}-${m}-${d}`;
                 }
 
                 if (/^\d{4}-\d{2}-\d{2}$/.test(v)) return v;
                 if (/^\d{4}-\d{2}-\d{2}T/.test(v)) return v.slice(0, 10);
 
-                if (window.moment) {
+                if (window.moment)
+                {
                     const m = moment.parseZone(
                         v,
-                        [moment.ISO_8601, 'DD/MM/YYYY', 'YYYY-MM-DD'],
+                        [moment.ISO_8601, "DD/MM/YYYY", "YYYY-MM-DD"],
                         true,
                     );
-                    if (m.isValid()) return m.format('YYYY-MM-DD');
+                    if (m.isValid()) return m.format("YYYY-MM-DD");
                 }
                 return v;
-            } catch (error) {
-                Alerta.TratamentoErroComLinha('manutencao.js', 'toYMD', error);
-            }
-        }
-
-        function diffDaysYMD(a, b) {
-            try {
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("manutencao.js", "toYMD", error);
+            }
+        }
+
+        function diffDaysYMD(a, b)
+        {
+            try
+            {
                 if (!a || !b) return 0;
-                const [ay, am, ad] = a.split('-').map(Number);
-                const [by, bm, bd] = b.split('-').map(Number);
+                const [ay, am, ad] = a.split("-").map(Number);
+                const [by, bm, bd] = b.split("-").map(Number);
                 const A = Date.UTC(ay, am - 1, ad);
                 const B = Date.UTC(by, bm - 1, bd);
                 return Math.round((A - B) / 86400000);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'manutencao.js',
-                    'diffDaysYMD',
-                    error,
-                );
-            }
-        }
-
-        if (
-            typeof manutencaoId !== 'undefined' &&
-            manutencaoId !== '' &&
-            manutencaoId !== 'null'
-        ) {
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("manutencao.js", "diffDaysYMD", error);
+            }
+        }
+
+        if (typeof manutencaoId !== "undefined" && manutencaoId !== "" && manutencaoId !== "null")
+        {
 
             const DataCriacao = formatarDataBR(manutencaoDataCriacao);
 
             const HoraCriacao = formatarHora(manutencaoDataCriacao);
 
             $.ajax({
-                url: '/api/Manutencao/RecuperaUsuario',
-                type: 'GET',
+                url: "/api/Manutencao/RecuperaUsuario",
+                type: "GET",
                 data: { id: manutencaoIdUsuarioCriacao },
-                contentType: 'application/json; charset=utf-8',
-                dataType: 'json',
-                success: function (data) {
-                    try {
-                        let usuarioCriacao = '';
-                        $.each(data, function (key, val) {
-                            try {
+                contentType: "application/json; charset=utf-8",
+                dataType: "json",
+                success: function (data)
+                {
+                    try
+                    {
+                        let usuarioCriacao = "";
+                        $.each(data, function (key, val)
+                        {
+                            try
+                            {
                                 usuarioCriacao = val;
-                            } catch (error) {
+                            }
+                            catch (error)
+                            {
                                 Alerta.TratamentoErroComLinha(
-                                    'manutencao.js',
-                                    'callback@$.each#1',
+                                    "manutencao.js",
+                                    "callback@$.each#1",
                                     error,
                                 );
                             }
                         });
-                        if (!usuarioCriacao) {
-                            document.getElementById(
-                                'divUsuarioCriacao',
-                            ).style.display = 'none';
-                            document.getElementById(
-                                'lblUsuarioCriacao',
-                            ).innerHTML = '';
-                        } else {
-                            document.getElementById(
-                                'divUsuarioCriacao',
-                            ).style.display = 'block';
-                            document.getElementById(
-                                'lblUsuarioCriacao',
-                            ).innerHTML =
+                        if (!usuarioCriacao)
+                        {
+                            document.getElementById("divUsuarioCriacao").style.display = "none";
+                            document.getElementById("lblUsuarioCriacao").innerHTML = "";
+                        } else
+                        {
+                            document.getElementById("divUsuarioCriacao").style.display = "block";
+                            document.getElementById("lblUsuarioCriacao").innerHTML =
                                 '<i class="fa-sharp-duotone fa-solid fa-user-plus"></i> <span>Criado por:</span> ' +
                                 usuarioCriacao +
-                                ' em ' +
+                                " em " +
                                 DataCriacao +
-                                ' às ' +
+                                " às " +
                                 HoraCriacao;
                         }
-                    } catch (error) {
-                        TratamentoErroComLinha(
-                            'manutencao.js',
-                            '(success: function)',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        TratamentoErroComLinha("manutencao.js", "(success: function)", error);
                     }
                 },
-                error: function (err) {
-                    try {
+                error: function (err)
+                {
+                    try
+                    {
                         console.log(err);
-                        alert('something went wrong');
-                    } catch (error) {
-                        TratamentoErroComLinha(
-                            'manutencao.js',
-                            '(error: function)',
-                            error,
-                        );
+                        alert("something went wrong");
+                    }
+                    catch (error)
+                    {
+                        TratamentoErroComLinha("manutencao.js", "(error: function)", error);
                     }
                 },
             });
 
-            if (
-                typeof manutencaoIdUsuarioAlteracao !== 'undefined' &&
-                manutencaoIdUsuarioAlteracao != null &&
-                manutencaoIdUsuarioAlteracao != ''
-            ) {
+            if (typeof manutencaoIdUsuarioAlteracao !== "undefined" && manutencaoIdUsuarioAlteracao != null && manutencaoIdUsuarioAlteracao != "")
+            {
                 const DataAlteracao = formatarDataBR(manutencaoDataAlteracao);
                 const HoraAlteracao = formatarHora(manutencaoDataAlteracao);
 
                 $.ajax({
-                    url: '/api/Manutencao/RecuperaUsuario',
-                    type: 'GET',
+                    url: "/api/Manutencao/RecuperaUsuario",
+                    type: "GET",
                     data: { id: manutencaoIdUsuarioAlteracao },
-                    contentType: 'application/json; charset=utf-8',
-                    dataType: 'json',
-                    success: function (data) {
-                        try {
-                            let usuarioAlteracao = '';
-                            $.each(data, function (key, val) {
-                                try {
+                    contentType: "application/json; charset=utf-8",
+                    dataType: "json",
+                    success: function (data)
+                    {
+                        try
+                        {
+                            let usuarioAlteracao = "";
+                            $.each(data, function (key, val)
+                            {
+                                try
+                                {
                                     usuarioAlteracao = val;
-                                } catch (error) {
+                                }
+                                catch (error)
+                                {
                                     Alerta.TratamentoErroComLinha(
-                                        'manutencao.js',
-                                        'callback@$.each#alteracao',
+                                        "manutencao.js",
+                                        "callback@$.each#alteracao",
                                         error,
                                     );
                                 }
                             });
-                            if (!usuarioAlteracao) {
-                                document.getElementById(
-                                    'divUsuarioAlteracao',
-                                ).style.display = 'none';
-                                document.getElementById(
-                                    'lblUsuarioAlteracao',
-                                ).innerHTML = '';
-                            } else {
-                                document.getElementById(
-                                    'divUsuarioAlteracao',
-                                ).style.display = 'block';
-                                document.getElementById(
-                                    'lblUsuarioAlteracao',
-                                ).innerHTML =
+                            if (!usuarioAlteracao)
+                            {
+                                document.getElementById("divUsuarioAlteracao").style.display = "none";
+                                document.getElementById("lblUsuarioAlteracao").innerHTML = "";
+                            } else
+                            {
+                                document.getElementById("divUsuarioAlteracao").style.display = "block";
+                                document.getElementById("lblUsuarioAlteracao").innerHTML =
                                     '<i class="fa-sharp-duotone fa-solid fa-user-pen"></i> <span>Alterado por:</span> ' +
                                     usuarioAlteracao +
-                                    ' em ' +
+                                    " em " +
                                     DataAlteracao +
-                                    ' às ' +
+                                    " às " +
                                     HoraAlteracao;
                             }
-                        } catch (error) {
-                            TratamentoErroComLinha(
-                                'manutencao.js',
-                                '(success: function alteracao)',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            TratamentoErroComLinha("manutencao.js", "(success: function alteracao)", error);
                         }
                     },
-                    error: function (err) {
-                        try {
+                    error: function (err)
+                    {
+                        try
+                        {
                             console.log(err);
-                        } catch (error) {
-                            TratamentoErroComLinha(
-                                'manutencao.js',
-                                '(error: function alteracao)',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            TratamentoErroComLinha("manutencao.js", "(error: function alteracao)", error);
                         }
                     },
                 });
-            } else {
-                var divAlteracao = document.getElementById(
-                    'divUsuarioAlteracao',
-                );
-                if (divAlteracao) {
-                    divAlteracao.style.display = 'none';
-                    document.getElementById('lblUsuarioAlteracao').innerHTML =
-                        '';
+            } else
+            {
+                var divAlteracao = document.getElementById("divUsuarioAlteracao");
+                if (divAlteracao)
+                {
+                    divAlteracao.style.display = "none";
+                    document.getElementById("lblUsuarioAlteracao").innerHTML = "";
                 }
             }
 
-            if (
-                manutencaoIdUsuarioCancelamento != null &&
-                manutencaoIdUsuarioCancelamento != ''
-            ) {
-                const DataCancelamento = formatarDataBR(
-                    manutencaoDataCancelamento,
-                );
-
-                const HoraCancelamento = formatarHora(
-                    manutencaoDataCancelamento,
-                );
+            if (manutencaoIdUsuarioCancelamento != null && manutencaoIdUsuarioCancelamento != "")
+            {
+
+                const DataCancelamento = formatarDataBR(manutencaoDataCancelamento);
+
+                const HoraCancelamento = formatarHora(manutencaoDataCancelamento);
 
                 $.ajax({
-                    url: '/api/Manutencao/RecuperaUsuario',
-                    type: 'GET',
+                    url: "/api/Manutencao/RecuperaUsuario",
+                    type: "GET",
                     data: { id: manutencaoIdUsuarioCancelamento },
-                    contentType: 'application/json; charset=utf-8',
-                    dataType: 'json',
-                    success: function (data) {
-                        try {
-                            let usuarioCancelamento = '';
-                            $.each(data, function (key, val) {
-                                try {
+                    contentType: "application/json; charset=utf-8",
+                    dataType: "json",
+                    success: function (data)
+                    {
+                        try
+                        {
+                            let usuarioCancelamento = "";
+                            $.each(data, function (key, val)
+                            {
+                                try
+                                {
                                     usuarioCancelamento = val;
-                                } catch (error) {
+                                }
+                                catch (error)
+                                {
                                     Alerta.TratamentoErroComLinha(
-                                        'manutencao.js',
-                                        'callback@$.each#1',
+                                        "manutencao.js",
+                                        "callback@$.each#1",
                                         error,
                                     );
                                 }
                             });
-                            if (!usuarioCancelamento) {
-                                document.getElementById(
-                                    'divUsuarioCancelamento',
-                                ).style.display = 'none';
-                                document.getElementById(
-                                    'lblUsuarioCancelamento',
-                                ).innerHTML = '';
-                            } else {
-                                document.getElementById(
-                                    'divUsuarioCancelamento',
-                                ).style.display = 'block';
-                                document.getElementById(
-                                    'lblUsuarioCancelamento',
-                                ).innerHTML =
+                            if (!usuarioCancelamento)
+                            {
+                                document.getElementById("divUsuarioCancelamento").style.display =
+                                    "none";
+                                document.getElementById("lblUsuarioCancelamento").innerHTML = "";
+                            } else
+                            {
+                                document.getElementById("divUsuarioCancelamento").style.display =
+                                    "block";
+                                document.getElementById("lblUsuarioCancelamento").innerHTML =
                                     '<i class="fa-sharp-duotone fa-solid fa-user-xmark"></i> <span>Cancelado por:</span> ' +
                                     usuarioCancelamento +
-                                    ' em ' +
+                                    " em " +
                                     DataCancelamento +
-                                    ' às ' +
+                                    " às " +
                                     HoraCancelamento;
                             }
-                        } catch (error) {
-                            TratamentoErroComLinha(
-                                'manutencao.js',
-                                '(success: function)',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            TratamentoErroComLinha("manutencao.js", "(success: function)", error);
                         }
                     },
-                    error: function (err) {
-                        try {
+                    error: function (err)
+                    {
+                        try
+                        {
                             console.log(err);
-                            alert('something went wrong');
-                        } catch (error) {
-                            TratamentoErroComLinha(
-                                'manutencao.js',
-                                '(error: function)',
-                                error,
-                            );
+                            alert("something went wrong");
+                        }
+                        catch (error)
+                        {
+                            TratamentoErroComLinha("manutencao.js", "(error: function)", error);
                         }
                     },
                 });
-            } else {
-                document.getElementById(
-                    'divUsuarioCancelamento',
-                ).style.display = 'none';
-                document.getElementById('lblUsuarioCancelamento').innerHTML =
-                    '';
-            }
-
-            if (
-                manutencaoIdUsuarioFinalizacao != null &&
-                manutencaoIdUsuarioFinalizacao != ''
-            ) {
-                const DataFinalizacao = formatarDataBR(
-                    manutencaoDataFinalizacao,
-                );
+            } else
+            {
+                document.getElementById("divUsuarioCancelamento").style.display = "none";
+                document.getElementById("lblUsuarioCancelamento").innerHTML = "";
+            }
+
+            if (manutencaoIdUsuarioFinalizacao != null && manutencaoIdUsuarioFinalizacao != "")
+            {
+
+                const DataFinalizacao = formatarDataBR(manutencaoDataFinalizacao);
 
                 const HoraFinalizacao = formatarHora(manutencaoDataFinalizacao);
 
                 $.ajax({
-                    url: '/api/Manutencao/RecuperaUsuario',
-                    type: 'GET',
+                    url: "/api/Manutencao/RecuperaUsuario",
+                    type: "GET",
                     data: { id: manutencaoIdUsuarioFinalizacao },
-                    contentType: 'application/json; charset=utf-8',
-                    dataType: 'json',
-                    success: function (data) {
-                        try {
-                            let usuarioFinalizacao = '';
-                            $.each(data, function (key, val) {
-                                try {
+                    contentType: "application/json; charset=utf-8",
+                    dataType: "json",
+                    success: function (data)
+                    {
+                        try
+                        {
+                            let usuarioFinalizacao = "";
+                            $.each(data, function (key, val)
+                            {
+                                try
+                                {
                                     usuarioFinalizacao = val;
-                                } catch (error) {
+                                }
+                                catch (error)
+                                {
                                     Alerta.TratamentoErroComLinha(
-                                        'manutencao.js',
-                                        'callback@$.each#1',
+                                        "manutencao.js",
+                                        "callback@$.each#1",
                                         error,
                                     );
                                 }
                             });
-                            if (!usuarioFinalizacao) {
-                                document.getElementById(
-                                    'divUsuarioFinalizacao',
-                                ).style.display = 'none';
-                                document.getElementById(
-                                    'lblUsuarioFinalizacao',
-                                ).innerHTML = '';
-                            } else {
-                                document.getElementById(
-                                    'divUsuarioFinalizacao',
-                                ).style.display = 'block';
-                                document.getElementById(
-                                    'lblUsuarioFinalizacao',
-                                ).innerHTML =
-                                    '<i class="fa-duotone fa-solid fa-user-check"></i> <span>Finalizado/Baixado por:</span> ' +
+                            if (!usuarioFinalizacao)
+                            {
+                                document.getElementById("divUsuarioFinalizacao").style.display =
+                                    "none";
+                                document.getElementById("lblUsuarioFinalizacao").innerHTML = "";
+                            } else
+                            {
+                                document.getElementById("divUsuarioFinalizacao").style.display =
+                                    "block";
+                                document.getElementById("lblUsuarioFinalizacao").innerHTML =
+                                    '<i class="fa-duotone fa-solid fa-user-check"></i> <span>Finalizado por:</span> ' +
                                     usuarioFinalizacao +
-                                    ' em ' +
+                                    " em " +
                                     DataFinalizacao +
-                                    ' às ' +
+                                    " às " +
                                     HoraFinalizacao;
                             }
-                        } catch (error) {
-                            TratamentoErroComLinha(
-                                'manutencao.js',
-                                '(success: function)',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            TratamentoErroComLinha("manutencao.js", "(success: function)", error);
                         }
                     },
-                    error: function (err) {
-                        try {
+                    error: function (err)
+                    {
+                        try
+                        {
                             console.log(err);
-                            alert('something went wrong');
-                        } catch (error) {
-                            TratamentoErroComLinha(
-                                'manutencao.js',
-                                '(error: function)',
-                                error,
-                            );
+                            alert("something went wrong");
+                        }
+                        catch (error)
+                        {
+                            TratamentoErroComLinha("manutencao.js", "(error: function)", error);
                         }
                     },
                 });
-            } else {
-                document.getElementById('divUsuarioFinalizacao').style.display =
-                    'none';
-                document.getElementById('lblUsuarioFinalizacao').innerHTML = '';
-            }
-        } else {
-            document.getElementById('divUsuarioCriacao').style.display = 'none';
-            document.getElementById('lblUsuarioCriacao').innerHTML = '';
-            var divAlteracao = document.getElementById('divUsuarioAlteracao');
-            if (divAlteracao) {
-                divAlteracao.style.display = 'none';
-                document.getElementById('lblUsuarioAlteracao').innerHTML = '';
-            }
-            document.getElementById('divUsuarioCancelamento').style.display =
-                'none';
-            document.getElementById('lblUsuarioCancelamento').innerHTML = '';
-            document.getElementById('divUsuarioFinalizacao').style.display =
-                'none';
-            document.getElementById('lblUsuarioFinalizacao').innerHTML = '';
+            } else
+            {
+                document.getElementById("divUsuarioFinalizacao").style.display = "none";
+                document.getElementById("lblUsuarioFinalizacao").innerHTML = "";
+            }
+        } else
+        {
+            document.getElementById("divUsuarioCriacao").style.display = "none";
+            document.getElementById("lblUsuarioCriacao").innerHTML = "";
+            var divAlteracao = document.getElementById("divUsuarioAlteracao");
+            if (divAlteracao)
+            {
+                divAlteracao.style.display = "none";
+                document.getElementById("lblUsuarioAlteracao").innerHTML = "";
+            }
+            document.getElementById("divUsuarioCancelamento").style.display = "none";
+            document.getElementById("lblUsuarioCancelamento").innerHTML = "";
+            document.getElementById("divUsuarioFinalizacao").style.display = "none";
+            document.getElementById("lblUsuarioFinalizacao").innerHTML = "";
         }
 
         var StatusOS = manutencaoStatusOS;
-        if (StatusOS === 'Fechada' || StatusOS === 'Cancelada') {
-            $('#btnEdita').hide();
-            $('#btnAdiciona').hide();
-
-            var $keepBtn = $('#btnVoltarLista')
-                .add('#btnVoltar')
+        if (StatusOS === "Fechada" || StatusOS === "Cancelada")
+        {
+            $("#btnEdita").hide();
+            $("#btnAdiciona").hide();
+
+            var $keepBtn = $("#btnVoltarLista")
+                .add("#btnVoltar")
                 .add('a[href*="ListaManutencao"]')
-                .add('a.btn.btn-vinho.form-control')
+                .add("a.btn.btn-vinho.form-control")
                 .add('[data-bs-dismiss="modal"]')
-                .add('#btnFecharModal')
-                .add('.modal .btn-vinho');
+                .add("#btnFecharModal")
+                .add(".modal .btn-vinho");
 
             var $keep = $keepBtn.add($keepBtn.parents());
 
-            $('input, select, textarea, button')
+            $("input, select, textarea, button")
                 .not($keep)
-                .prop('disabled', true)
-                .css('opacity', 0.5);
-
-            $('a, .btn')
+                .prop("disabled", true)
+                .css("opacity", 0.5);
+
+            $("a, .btn")
                 .not($keep)
-                .attr('aria-disabled', 'true')
-                .attr('tabindex', '-1')
-                .on('click.block', function (e) {
-                    try {
+                .attr("aria-disabled", "true")
+                .attr("tabindex", "-1")
+                .on("click.block", function (e)
+                {
+                    try
+                    {
                         e.preventDefault();
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'manutencao.js',
-                            'callback@$.not.attr.attr.on#1',
+                            "manutencao.js",
+                            "callback@$.not.attr.attr.on#1",
                             error,
                         );
                     }
                 })
-                .css('opacity', 0.5);
-
-            $('div').not($keep).css('pointer-events', 'none');
+                .css("opacity", 0.5);
+
+            $("div").not($keep).css("pointer-events", "none");
 
             $(
                 'input:disabled, select:disabled, textarea:disabled, button:disabled, a[aria-disabled="true"], .btn[aria-disabled="true"]',
-            ).css('cursor', 'not-allowed');
+            ).css("cursor", "not-allowed");
 
             var $btnVoltar = $("#btnVoltar, a[href*='ListaManutencao']");
             $btnVoltar
-                .removeAttr('aria-disabled')
-                .removeAttr('tabindex')
-                .off('click.block')
-                .prop('disabled', false)
+                .removeAttr("aria-disabled")
+                .removeAttr("tabindex")
+                .off("click.block")
+                .prop("disabled", false)
                 .css({
-                    opacity: '1',
-                    'pointer-events': 'auto',
-                    cursor: 'pointer',
+                    "opacity": "1",
+                    "pointer-events": "auto",
+                    "cursor": "pointer"
                 });
 
-            $btnVoltar.parents().css('pointer-events', 'auto');
-
-            var $btnFecharModal = $(
-                '[data-bs-dismiss="modal"], #btnFecharModal, .modal .btn-vinho, .modal button.btn-vinho',
-            );
+            $btnVoltar.parents().css("pointer-events", "auto");
+
+            var $btnFecharModal = $('[data-bs-dismiss="modal"], #btnFecharModal, .modal .btn-vinho, .modal button.btn-vinho');
             $btnFecharModal
-                .removeAttr('aria-disabled')
-                .removeAttr('tabindex')
-                .off('click.block')
-                .prop('disabled', false)
+                .removeAttr("aria-disabled")
+                .removeAttr("tabindex")
+                .off("click.block")
+                .prop("disabled", false)
                 .css({
-                    opacity: '1',
-                    'pointer-events': 'auto',
-                    cursor: 'pointer',
+                    "opacity": "1",
+                    "pointer-events": "auto",
+                    "cursor": "pointer"
                 });
 
-            $(
-                '.modal, .modal-dialog, .modal-content, .modal-header, .modal-body, .modal-footer',
-            ).css('pointer-events', 'auto');
-            $btnFecharModal.parents().css('pointer-events', 'auto');
+            $(".modal, .modal-dialog, .modal-content, .modal-header, .modal-body, .modal-footer").css("pointer-events", "auto");
+            $btnFecharModal.parents().css("pointer-events", "auto");
 
             window.modoVisualizacaoFoto = true;
 
-            $('#btnAdicionaItem').hide();
-
-            $('#btnAdicionarFoto').hide();
-
-            var lblItensSelecionados = document
-                .querySelector('#tblItens')
-                ?.closest('.card')
-                ?.querySelector(
-                    '.card-header span, .card-header h5, .card-header h6, .card-header .card-title',
-                );
-            if (lblItensSelecionados) {
-                lblItensSelecionados.innerHTML =
-                    '<i class="fa-duotone fa-solid fa-wrench me-2"></i>Manutenções efetuadas no veículo';
-            }
-
-            var lblItensById = document.getElementById('lblItensSelecionados');
-            if (lblItensById) {
-                lblItensById.innerHTML =
-                    '<i class="fa-duotone fa-solid fa-wrench me-2"></i>Manutenções efetuadas no veículo';
-            }
-        } else {
+            $("#btnAdicionaItem").hide();
+
+            $("#btnAdicionarFoto").hide();
+
+            var lblItensSelecionados = document.querySelector("#tblItens")?.closest(".card")?.querySelector(".card-header span, .card-header h5, .card-header h6, .card-header .card-title");
+            if (lblItensSelecionados)
+            {
+                lblItensSelecionados.innerHTML = '<i class="fa-duotone fa-solid fa-wrench me-2"></i>Manutenções efetuadas no veículo';
+            }
+
+            var lblItensById = document.getElementById("lblItensSelecionados");
+            if (lblItensById)
+            {
+                lblItensById.innerHTML = '<i class="fa-duotone fa-solid fa-wrench me-2"></i>Manutenções efetuadas no veículo';
+            }
+        } else
+        {
 
             window.modoVisualizacaoFoto = false;
         }
 
-        if (
-            StatusOS != 'Aberta' &&
-            StatusOS !== 'Fechada' &&
-            StatusOS !== 'Cancelada'
-        ) {
-
-            if (!$ds.val()) {
-                $ds.val(moment().format('YYYY-MM-DD'));
-            }
-        }
-
-        if (
-            typeof manutencaoId !== 'undefined' &&
-            manutencaoId !== '' &&
-            manutencaoId !== 'null' &&
-            manutencaoId !== '00000000-0000-0000-0000-000000000000'
-        ) {
-            var lstStatusEl = document.getElementById('lstStatus');
-            if (lstStatusEl) {
+        if (StatusOS != "Aberta" && StatusOS !== "Fechada" && StatusOS !== "Cancelada")
+        {
+
+            if (!$ds.val())
+            {
+                $ds.val(moment().format("YYYY-MM-DD"));
+            }
+        }
+
+        if (typeof manutencaoId !== "undefined" && manutencaoId !== "" && manutencaoId !== "null" && manutencaoId !== "00000000-0000-0000-0000-000000000000")
+        {
+            var lstStatusEl = document.getElementById("lstStatus");
+            if (lstStatusEl)
+            {
                 lstStatusEl.disabled = true;
-                lstStatusEl.style.opacity = '0.6';
-                lstStatusEl.style.cursor = 'not-allowed';
-                lstStatusEl.title =
-                    "O status só pode ser alterado através do botão 'Baixar OS' na tela de Controle de Manutenções";
-            }
-        }
-
-        (function protectSolicDate() {
-            try {
-                const originalRaw = ($ds.val() || '').trim();
+                lstStatusEl.style.opacity = "0.6";
+                lstStatusEl.style.cursor = "not-allowed";
+                lstStatusEl.title = "O status só pode ser alterado através do botão 'Baixar OS' na tela de Controle de Manutenções";
+            }
+        }
+
+        (function protectSolicDate()
+        {
+            try
+            {
+                const originalRaw = ($ds.val() || "").trim();
                 if (!originalRaw) return;
 
                 const originalYMD = toYMD(originalRaw);
@@ -705,34 +694,39 @@
                 $ds.val(originalYMD);
 
                 let userTouched = false;
-                $ds.on('focus input change', function () {
-                    try {
+                $ds.on("focus input change", function ()
+                {
+                    try
+                    {
                         userTouched = true;
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'manutencao.js',
-                            'callback@$ds.on#1',
+                            "manutencao.js",
+                            "callback@$ds.on#1",
                             error,
                         );
                     }
                 });
 
-                function guard() {
-                    try {
+                function guard()
+                {
+                    try
+                    {
                         if (userTouched) return;
                         const cur = toYMD($ds.val());
                         if (!cur) return;
                         const d = diffDaysYMD(cur, originalYMD);
 
-                        if (d === 1 || d === -1) {
+                        if (d === 1 || d === -1)
+                        {
                             $ds.val(originalYMD);
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'manutencao.js',
-                            'guard',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("manutencao.js", "guard", error);
                     }
                 }
 
@@ -740,40 +734,49 @@
                 setTimeout(guard, 150);
                 setTimeout(guard, 350);
                 setTimeout(guard, 800);
-                window.addEventListener('load', guard);
-            } catch (error) {
+                window.addEventListener("load", guard);
+            }
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha(
-                    'manutencao.js',
-                    'callback@function protectSolicDate() { const orig#0',
+                    "manutencao.js",
+                    "callback@function protectSolicDate() { const orig#0",
                     error,
                 );
             }
         })();
 
-        if (window.FTXTooltip) {
+        if (window.FTXTooltip)
+        {
             FTXTooltip.setAutoClose(1500);
         }
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'DOMContentLoaded', error);
+
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "DOMContentLoaded", error);
     }
 });
 
-function removeHTML(str) {
-    try {
-        var tmp = document.createElement('DIV');
+function removeHTML(str)
+{
+    try
+    {
+        var tmp = document.createElement("DIV");
         tmp.innerHTML = str;
-        return tmp.textContent || tmp.innerText || '';
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('manutencao.js', 'removeHTML', error);
+        return tmp.textContent || tmp.innerText || "";
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("manutencao.js", "removeHTML", error);
     }
 }
 
-function escAttr(s) {
+function escAttr(s)
+{
     return String(s)
-        .replace(/&/g, '&amp;')
-        .replace(/"/g, '&quot;')
-        .replace(/</g, '&lt;')
-        .replace(/>/g, '&gt;');
+        .replace(/&/g, '&amp;').replace(/"/g, '&quot;')
+        .replace(/</g, '&lt;').replace(/>/g, '&gt;');
 }
 
 function SelecionaLinha(
@@ -786,16 +789,14 @@
     MotoristaId,
     ImagemOcorrencia,
     ItemManutencaoId,
-    origemBtn,
-) {
-    try {
+    origemBtn
+)
+{
+    try
+    {
 
         const img = ImagemOcorrencia || '';
-        const temFoto =
-            img &&
-            img !== '' &&
-            img !== 'null' &&
-            img.toLowerCase() !== 'semimagem.jpg';
+        const temFoto = img && img !== '' && img !== 'null' && img.toLowerCase() !== 'semimagem.jpg';
 
         const botaoFoto = temFoto
             ? `<button type="button"
@@ -841,15 +842,13 @@
                             --fa-secondary-opacity: 0.8;"></i>
                 </button>`;
 
-        $('#tblItens')
-            .DataTable()
-            .row.add({
-                tipoItem: 'Ocorrência',
-                numFicha: Ficha,
-                dataItem: DataOcorrencia,
-                nomeMotorista: Motorista,
-                resumo: Resumo,
-                acoes: `
+        $('#tblItens').DataTable().row.add({
+            tipoItem: "Ocorrência",
+            numFicha: Ficha,
+            dataItem: DataOcorrencia,
+            nomeMotorista: Motorista,
+            resumo: Resumo,
+            acoes: `
                         <div class="col-acao">
                             <div class="d-flex gap-2 justify-content-center">
                                 <button type="button"
@@ -876,19 +875,20 @@
                             </div>
                         </div>
                         `,
-                itemManutencaoId: ItemManutencaoId,
-                descricao: Descricao,
-                resumoTexto: Resumo,
-                motoristaId: MotoristaId,
-                imagemOcorrencia: ImagemOcorrencia,
-                viagemId: ViagemId,
-            })
-            .draw(false);
+            itemManutencaoId: ItemManutencaoId,
+            descricao: Descricao,
+            resumoTexto: Resumo,
+            motoristaId: MotoristaId,
+            imagemOcorrencia: ImagemOcorrencia,
+            viagemId: ViagemId
+        }).draw(false);
 
         const dtOrigem = $(origemBtn).closest('table').DataTable();
         dtOrigem.row($(origemBtn).closest('tr')).remove().draw(false);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('manutencao.js', 'SelecionaLinha', error);
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("manutencao.js", "SelecionaLinha", error);
     }
 }
 
@@ -902,17 +902,15 @@
     MotoristaId,
     ImagemOcorrencia,
     ItemManutencaoId,
-    origemBtn,
-) {
-    try {
+    origemBtn
+)
+{
+    try
+    {
         Descricao = removeHTML(Descricao);
 
         const img = ImagemOcorrencia || '';
-        const temFoto =
-            img &&
-            img !== '' &&
-            img !== 'null' &&
-            img.toLowerCase() !== 'semimagem.jpg';
+        const temFoto = img && img !== '' && img !== 'null' && img.toLowerCase() !== 'semimagem.jpg';
 
         const botaoFoto = temFoto
             ? `<button type="button"
@@ -958,10 +956,10 @@
                             --fa-secondary-opacity: 0.8;"></i>
                 </button>`;
 
-        $('#tblItens')
+        $("#tblItens")
             .DataTable()
             .row.add({
-                tipoItem: 'Pendência',
+                tipoItem: "Pendência",
                 numFicha: Ficha,
                 dataItem: DataOcorrencia,
                 nomeMotorista: Motorista,
@@ -1004,16 +1002,16 @@
 
         const dtOrigem = $(origemBtn).closest('table').DataTable();
         dtOrigem.row($(origemBtn).closest('tr')).remove().draw(false);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'manutencao.js',
-            'SelecionaLinhaPendencia',
-            error,
-        );
+
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("manutencao.js", "SelecionaLinhaPendencia", error);
     }
 }
 
-class Ocorrencia {
+class Ocorrencia
+{
     constructor(
         noFichaVistoria,
         data,
@@ -1024,8 +1022,10 @@
         motoristaId,
         imagemOcorrencia,
         itemManutencaoId,
-    ) {
-        try {
+    )
+    {
+        try
+        {
             this.noFichaVistoria = noFichaVistoria;
             this.data = data;
             this.motorista = motorista;
@@ -1035,170 +1035,167 @@
             this.motoristaId = motoristaId;
             this.imagemOcorrencia = imagemOcorrencia;
             this.itemManutencaoId = itemManutencaoId;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'manutencao.js',
-                'constructor',
-                error,
-            );
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("manutencao.js", "constructor", error);
         }
     }
 }
 
-$('#tblOcorrencia').on('click', 'a.btnSeleciona', function () {
-    try {
-        dataTableOcorrencias.row($(this).parents('tr')).remove().draw(false);
-    } catch (error) {
-        TratamentoErroComLinha(
-            'manutencao.js',
-            'click.btnSeleciona.tblOcorrencia',
-            error,
-        );
+$("#tblOcorrencia").on("click", "a.btnSeleciona", function ()
+{
+    try
+    {
+        dataTableOcorrencias.row($(this).parents("tr")).remove().draw(false);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "click.btnSeleciona.tblOcorrencia", error);
     }
 });
 
-$('#tblPendencia').on('click', 'a.btnSeleciona', function () {
-    try {
-        dataTablePendencias.row($(this).parents('tr')).remove().draw(false);
-    } catch (error) {
-        TratamentoErroComLinha(
-            'manutencao.js',
-            'click.btnSeleciona.tblPendencia',
-            error,
-        );
+$("#tblPendencia").on("click", "a.btnSeleciona", function ()
+{
+    try
+    {
+        dataTablePendencias.row($(this).parents("tr")).remove().draw(false);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "click.btnSeleciona.tblPendencia", error);
     }
 });
 
-$('#btnFechar').click(function (e) {
-    try {
-
-        $('.modal-backdrop').remove();
-        $('body').removeClass('modal-open').css({
-            overflow: '',
-            'padding-right': '',
+$("#btnFechar").click(function (e)
+{
+    try
+    {
+
+        $(".modal-backdrop").remove();
+        $("body").removeClass("modal-open").css({
+            'overflow': '',
+            'padding-right': ''
         });
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'click.btnFechar', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "click.btnFechar", error);
     }
 });
 
-function onCreate() {
-    try {
+function onCreate()
+{
+    try
+    {
         defaultRTE = this;
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'onCreate', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "onCreate", error);
     }
 }
 
-$('#modalManutencao')
-    .on('shown.bs.modal', function () {
-        try {
+$("#modalManutencao")
+    .on("shown.bs.modal", function ()
+    {
+        try
+        {
             defaultRTE.refreshUI();
-            document.getElementById('txtData').value =
-                moment(Date()).format('YYYY-MM-DD');
-
-            $('#imgViewerItem').attr(
-                'src',
-                '/DadosEditaveis/ImagensOcorrencias/semimagem.jpg',
-            );
-        } catch (error) {
-            TratamentoErroComLinha(
-                'manutencao.js',
-                'modalManutencao.shown.bs.modal',
-                error,
-            );
+            document.getElementById("txtData").value = moment(Date()).format("YYYY-MM-DD");
+
+            $("#imgViewerItem").attr("src", "/DadosEditaveis/ImagensOcorrencias/semimagem.jpg");
+        } catch (error)
+        {
+            TratamentoErroComLinha("manutencao.js", "modalManutencao.shown.bs.modal", error);
         }
     })
 
-    .on('hidden.bs.modal', function () {
-        try {
-            document.getElementById('txtData').value = '';
-            document.getElementById('txtResumo').value = '';
-
-            var descricao =
-                document.getElementById('rteManutencao').ej2_instances[0];
-            descricao.value = '';
-
-            var motorista =
-                document.getElementById('lstMotorista').ej2_instances[0];
-            motorista.value = '';
-
-            var $file = $('#txtFileItem');
-            if ($file.length) {
+    .on("hidden.bs.modal", function ()
+    {
+        try
+        {
+            document.getElementById("txtData").value = "";
+            document.getElementById("txtResumo").value = "";
+
+            var descricao = document.getElementById("rteManutencao").ej2_instances[0];
+            descricao.value = "";
+
+            var motorista = document.getElementById("lstMotorista").ej2_instances[0];
+            motorista.value = "";
+
+            var $file = $("#txtFileItem");
+            if ($file.length)
+            {
                 var upload =
-                    (typeof $file.getKendoUpload === 'function' &&
-                        $file.getKendoUpload()) ||
-                    $file.data('kendoUpload');
-
-                if (upload && typeof upload.clearAllFiles === 'function') {
+                    (typeof $file.getKendoUpload === "function" && $file.getKendoUpload()) ||
+                    $file.data("kendoUpload");
+
+                if (upload && typeof upload.clearAllFiles === "function")
+                {
                     upload.clearAllFiles();
-                } else if (
-                    upload &&
-                    typeof upload.removeAllFiles === 'function'
-                ) {
+                } else if (upload && typeof upload.removeAllFiles === "function")
+                {
                     upload.removeAllFiles();
-                } else {
-                    $file.val('');
-                    var $wrap = $file.closest('.k-upload');
-                    if ($wrap.length) {
-                        $wrap.find('.k-file').remove();
-                        $wrap.find('.k-upload-status').text('');
+                } else
+                {
+                    $file.val("");
+                    var $wrap = $file.closest(".k-upload");
+                    if ($wrap.length)
+                    {
+                        $wrap.find(".k-file").remove();
+                        $wrap.find(".k-upload-status").text("");
                     }
                 }
             }
 
             setTimeout(() => {
-                $('.modal-backdrop').remove();
-                $('body').removeClass('modal-open').css({
-                    overflow: '',
-                    'padding-right': '',
+                $(".modal-backdrop").remove();
+                $("body").removeClass("modal-open").css({
+                    'overflow': '',
+                    'padding-right': ''
                 });
             }, 100);
-        } catch (error) {
-            TratamentoErroComLinha(
-                'manutencao.js',
-                'modalManutencao.hidden.bs.modal',
-                error,
-            );
+        } catch (error)
+        {
+            TratamentoErroComLinha("manutencao.js", "modalManutencao.hidden.bs.modal", error);
         }
     });
 
-$('#btnInsereItem').click(function (e) {
-    try {
+$("#btnInsereItem").click(function (e)
+{
+    try
+    {
         e.preventDefault();
 
         ImagemOcorrencia = ImagemSelecionada;
 
-        DataItem = $('#txtData').val();
-        Resumo = $('#txtResumo').val();
+        DataItem = $("#txtData").val();
+        Resumo = $("#txtResumo").val();
 
         DataItem =
             DataItem.substring(8, 10) +
-            '/' +
+            "/" +
             DataItem.substring(5, 7) +
-            '/' +
+            "/" +
             DataItem.substring(0, 4);
 
-        if (Resumo === '') {
+        if (Resumo === "")
+        {
             Alerta.Erro(
-                'Informação Ausente',
-                'O Resumo do Item é obrigatório',
-                'Ok',
+                "Informação Ausente",
+                "O Resumo do Item é obrigatório",
+                "Ok"
+
             );
             return;
         }
 
-        var Descricao =
-            document.getElementById('rteManutencao').ej2_instances[0];
-        var Motorista =
-            document.getElementById('lstMotorista').ej2_instances[0];
+        var Descricao = document.getElementById("rteManutencao").ej2_instances[0];
+        var Motorista = document.getElementById("lstMotorista").ej2_instances[0];
 
         const img = ImagemOcorrencia || '';
-        const temFoto =
-            img &&
-            img !== '' &&
-            img !== 'null' &&
-            img.toLowerCase() !== 'semimagem.jpg';
+        const temFoto = img && img !== '' && img !== 'null' && img.toLowerCase() !== 'semimagem.jpg';
 
         const botaoFoto = temFoto
             ? `<button type="button"
@@ -1244,11 +1241,11 @@
                             --fa-secondary-opacity: 0.8;"></i>
                 </button>`;
 
-        $('#tblItens')
+        $("#tblItens")
             .DataTable()
             .row.add({
-                tipoItem: 'Inserção Manual',
-                numFicha: 'N/A',
+                tipoItem: "Inserção Manual",
+                numFicha: "N/A",
                 dataItem: DataItem,
                 nomeMotorista: Motorista.text,
                 resumo: `<div class='text-center'><a aria-label='&#9881; (${removeHTML(Descricao.value)})' data-microtip-position='top' role='tooltip' data-microtip-size='medium' style='cursor:pointer;'>${Resumo}</a></div>`,
@@ -1273,294 +1270,280 @@
                                     </button>
 
                                     ${botaoFoto}`,
-                itemManutencaoId: '',
+                itemManutencaoId: "",
                 descricao: removeHTML(Descricao.value),
                 resumo: Resumo,
                 motoristaId: Motorista.value,
                 imagemOcorrencia: ImagemOcorrencia,
-                viagemId: '',
+                viagemId: "",
             })
             .draw(false);
 
         const modalEl = document.getElementById('modalManutencao');
-        if (modalEl && window.bootstrap?.Modal) {
+        if (modalEl && window.bootstrap?.Modal)
+        {
             const modal = bootstrap.Modal.getInstance(modalEl);
             if (modal) modal.hide();
         }
 
         setTimeout(() => {
-            $('.modal-backdrop').remove();
-            $('body').removeClass('modal-open').css({
-                overflow: '',
-                'padding-right': '',
+            $(".modal-backdrop").remove();
+            $("body").removeClass("modal-open").css({
+                'overflow': '',
+                'padding-right': ''
             });
         }, 300);
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'click.btnInsereItem', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "click.btnInsereItem", error);
     }
 });
 
-$('#tblItens tbody').on('click', 'tr', function () {
-    try {
+$("#tblItens tbody").on("click", "tr", function ()
+{
+    try
+    {
         LinhaSelecionada = dataTableItens.row(this).index();
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'click.tr.tblItens', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "click.tr.tblItens", error);
     }
 });
 
-$(document).on(
-    'click',
-    "#tblOcorrencia [data-bs-target='#modalFotoOcorrencia']",
-    function (e) {
-        try {
-            e.preventDefault();
-            e.stopPropagation();
-
-            const modalEl = document.getElementById('modalFotoOcorrencia');
-            if (modalEl && window.bootstrap?.Modal) {
-
-                $(modalEl).data('triggerBtn', this);
-                const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
-                modal.show();
-            }
-        } catch (error) {
-            console.error('Erro ao abrir modalFotoOcorrencia:', error);
-            Alerta.TratamentoErroComLinha(
-                'manutencao.js',
-                'click.modalFotoOcorrencia',
-                error,
-            );
-        }
-    },
-);
-
-$(document).on(
-    'click',
-    "#tblPendencia [data-bs-target='#modalFotoPendencia']",
-    function (e) {
-        try {
-            e.preventDefault();
-            e.stopPropagation();
-
-            const modalEl = document.getElementById('modalFotoPendencia');
-            if (modalEl && window.bootstrap?.Modal) {
-                $(modalEl).data('triggerBtn', this);
-                const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
-                modal.show();
-            }
-        } catch (error) {
-            console.error('Erro ao abrir modalFotoPendencia:', error);
-            Alerta.TratamentoErroComLinha(
-                'manutencao.js',
-                'click.modalFotoPendencia',
-                error,
-            );
-        }
-    },
-);
-
-$(document).on(
-    'click',
-    "#tblItens [data-bs-target='#modalFotoManutencao']",
-    function (e) {
-        try {
-            e.preventDefault();
-            e.stopPropagation();
-
-            const modalEl = document.getElementById('modalFotoManutencao');
-            if (modalEl && window.bootstrap?.Modal) {
-                $(modalEl).data('triggerBtn', this);
-                const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
-                modal.show();
-            }
-        } catch (error) {
-            console.error('Erro ao abrir modalFotoManutencao:', error);
-            Alerta.TratamentoErroComLinha(
-                'manutencao.js',
-                'click.modalFotoManutencao',
-                error,
-            );
-        }
-    },
-);
-
-(function () {
-    try {
+$(document).on("click", "#tblOcorrencia [data-bs-target='#modalFotoOcorrencia']", function(e)
+{
+    try
+    {
+        e.preventDefault();
+        e.stopPropagation();
+
+        const modalEl = document.getElementById('modalFotoOcorrencia');
+        if (modalEl && window.bootstrap?.Modal)
+        {
+
+            $(modalEl).data('triggerBtn', this);
+            const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
+            modal.show();
+        }
+    }
+    catch (error)
+    {
+        console.error("Erro ao abrir modalFotoOcorrencia:", error);
+        Alerta.TratamentoErroComLinha("manutencao.js", "click.modalFotoOcorrencia", error);
+    }
+});
+
+$(document).on("click", "#tblPendencia [data-bs-target='#modalFotoPendencia']", function(e)
+{
+    try
+    {
+        e.preventDefault();
+        e.stopPropagation();
+
+        const modalEl = document.getElementById('modalFotoPendencia');
+        if (modalEl && window.bootstrap?.Modal)
+        {
+            $(modalEl).data('triggerBtn', this);
+            const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
+            modal.show();
+        }
+    }
+    catch (error)
+    {
+        console.error("Erro ao abrir modalFotoPendencia:", error);
+        Alerta.TratamentoErroComLinha("manutencao.js", "click.modalFotoPendencia", error);
+    }
+});
+
+$(document).on("click", "#tblItens [data-bs-target='#modalFotoManutencao']", function(e)
+{
+    try
+    {
+        e.preventDefault();
+        e.stopPropagation();
+
+        const modalEl = document.getElementById('modalFotoManutencao');
+        if (modalEl && window.bootstrap?.Modal)
+        {
+            $(modalEl).data('triggerBtn', this);
+            const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
+            modal.show();
+        }
+    }
+    catch (error)
+    {
+        console.error("Erro ao abrir modalFotoManutencao:", error);
+        Alerta.TratamentoErroComLinha("manutencao.js", "click.modalFotoManutencao", error);
+    }
+});
+
+(function ()
+{
+    try
+    {
         const modalElement = document.getElementById('modalFotoOcorrencia');
-        if (!modalElement) {
-            console.warn(
-                '[manutencao.js] Elemento #modalFotoOcorrencia não encontrado',
-            );
+        if (!modalElement)
+        {
+            console.warn('[manutencao.js] Elemento #modalFotoOcorrencia não encontrado');
             return;
         }
 
-        modalElement.addEventListener('show.bs.modal', function (event) {
-            try {
-
-                var btnClicado =
-                    event.relatedTarget || $(modalElement).data('triggerBtn');
+        modalElement.addEventListener('show.bs.modal', function (event)
+        {
+            try
+            {
+
+                var btnClicado = event.relatedTarget || $(modalElement).data('triggerBtn');
                 var imagemOcorrencia = $(btnClicado).data('imagem') || '';
 
                 $(modalElement).removeData('triggerBtn');
 
-                $('#imgViewerOcorrencia').removeAttr('src');
-
-                if (
-                    imagemOcorrencia &&
-                    imagemOcorrencia !== '' &&
-                    imagemOcorrencia !== 'null'
-                ) {
-                    $('#imgViewerOcorrencia').attr(
-                        'src',
-                        '/DadosEditaveis/ImagensOcorrencias/' +
-                            imagemOcorrencia,
+                $("#imgViewerOcorrencia").removeAttr("src");
+
+                if (imagemOcorrencia && imagemOcorrencia !== '' && imagemOcorrencia !== 'null')
+                {
+                    $("#imgViewerOcorrencia").attr(
+                        "src",
+                        "/DadosEditaveis/ImagensOcorrencias/" + imagemOcorrencia
                     );
-                } else {
-                    $('#imgViewerOcorrencia').attr(
-                        'src',
-                        '/DadosEditaveis/ImagensOcorrencias/semimagem.jpg',
+                } else
+                {
+                    $("#imgViewerOcorrencia").attr(
+                        "src",
+                        "/DadosEditaveis/ImagensOcorrencias/semimagem.jpg"
                     );
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'manutencao.js',
-                    'modalFotoOcorrencia.show.bs.modal',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("manutencao.js", "modalFotoOcorrencia.show.bs.modal", error);
             }
         });
 
-        modalElement.addEventListener('hide.bs.modal', function (event) {
-            try {
-                $('#imgViewerOcorrencia').removeAttr('src');
+        modalElement.addEventListener('hide.bs.modal', function (event)
+        {
+            try
+            {
+                $("#imgViewerOcorrencia").removeAttr("src");
 
                 setTimeout(() => {
-                    document
-                        .querySelectorAll('.modal-backdrop')
-                        .forEach((backdrop) => backdrop.remove());
+                    document.querySelectorAll('.modal-backdrop').forEach(backdrop => backdrop.remove());
                     document.body.classList.remove('modal-open');
                     document.body.style.overflow = '';
                     document.body.style.paddingRight = '';
                 }, 150);
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'manutencao.js',
-                    'modalFotoOcorrencia.hide.bs.modal',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("manutencao.js", "modalFotoOcorrencia.hide.bs.modal", error);
             }
         });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'manutencao.js',
-            'init@modalFotoOcorrencia',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "init@modalFotoOcorrencia", error);
     }
 })();
 
-(function () {
-    try {
+(function ()
+{
+    try
+    {
         const modalElement = document.getElementById('modalFotoPendencia');
-        if (!modalElement) {
-            console.warn(
-                '[manutencao.js] Elemento #modalFotoPendencia não encontrado',
-            );
+        if (!modalElement)
+        {
+            console.warn('[manutencao.js] Elemento #modalFotoPendencia não encontrado');
             return;
         }
 
-        modalElement.addEventListener('show.bs.modal', function (event) {
-            try {
-
-                var btnClicado =
-                    event.relatedTarget || $(modalElement).data('triggerBtn');
-                var imagemPendencia =
-                    $(btnClicado).data('foto') ||
-                    $(btnClicado).data('imagem') ||
-                    '';
+        modalElement.addEventListener('show.bs.modal', function (event)
+        {
+            try
+            {
+
+                var btnClicado = event.relatedTarget || $(modalElement).data('triggerBtn');
+                var imagemPendencia = $(btnClicado).data('foto') || $(btnClicado).data('imagem') || '';
 
                 $(modalElement).removeData('triggerBtn');
 
-                $('#imgViewerPendencia').removeAttr('src');
-
-                if (
-                    imagemPendencia &&
-                    imagemPendencia !== '' &&
-                    imagemPendencia !== 'null'
-                ) {
-                    $('#imgViewerPendencia').attr(
-                        'src',
-                        '/DadosEditaveis/ImagensOcorrencias/' +
-                            decodeURIComponent(imagemPendencia),
+                $("#imgViewerPendencia").removeAttr("src");
+
+                if (imagemPendencia && imagemPendencia !== '' && imagemPendencia !== 'null')
+                {
+                    $("#imgViewerPendencia").attr(
+                        "src",
+                        "/DadosEditaveis/ImagensOcorrencias/" + decodeURIComponent(imagemPendencia)
                     );
-                } else {
-                    $('#imgViewerPendencia').attr(
-                        'src',
-                        '/DadosEditaveis/ImagensOcorrencias/semimagem.jpg',
+                } else
+                {
+                    $("#imgViewerPendencia").attr(
+                        "src",
+                        "/DadosEditaveis/ImagensOcorrencias/semimagem.jpg"
                     );
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'manutencao.js',
-                    'modalFotoPendencia.show.bs.modal',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("manutencao.js", "modalFotoPendencia.show.bs.modal", error);
             }
         });
 
-        modalElement.addEventListener('hide.bs.modal', function (event) {
-            try {
-                $('#imgViewerPendencia').removeAttr('src');
+        modalElement.addEventListener('hide.bs.modal', function (event)
+        {
+            try
+            {
+                $("#imgViewerPendencia").removeAttr("src");
 
                 setTimeout(() => {
-                    document
-                        .querySelectorAll('.modal-backdrop')
-                        .forEach((backdrop) => backdrop.remove());
+                    document.querySelectorAll('.modal-backdrop').forEach(backdrop => backdrop.remove());
                     document.body.classList.remove('modal-open');
                     document.body.style.overflow = '';
                     document.body.style.paddingRight = '';
                 }, 150);
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'manutencao.js',
-                    'modalFotoPendencia.hide.bs.modal',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("manutencao.js", "modalFotoPendencia.hide.bs.modal", error);
             }
         });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'manutencao.js',
-            'init@modalFotoPendencia',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "init@modalFotoPendencia", error);
     }
 })();
 
-(function initModalFoto() {
+(function initModalFoto()
+{
     if (window.__bindModalFoto) return;
     window.__bindModalFoto = true;
 
-    window.BASE_IMAGENS = '/DadosEditaveis/ImagensOcorrencias/';
+    window.BASE_IMAGENS = "/DadosEditaveis/ImagensOcorrencias/";
     const BASE = window.BASE_IMAGENS;
     const modalEl = document.getElementById('modalFotoManutencao');
     const imgEl = document.getElementById('imgViewerNovo');
 
-    if (!modalEl || !imgEl) {
-        console.warn('Modal ou imagem não encontrados');
+    if (!modalEl || !imgEl)
+    {
+        console.warn("Modal ou imagem não encontrados");
         return;
     }
 
-    modalEl.addEventListener('show.bs.modal', function (event) {
-        try {
+    modalEl.addEventListener('show.bs.modal', function (event)
+    {
+        try
+        {
 
             const btn = event.relatedTarget || $(modalEl).data('triggerBtn');
 
             $(modalEl).removeData('triggerBtn');
 
-            if (!btn) {
-                console.warn('Botão não encontrado para modalFotoManutencao');
-                imgEl.src = BASE + 'semimagem.jpg';
+            if (!btn)
+            {
+                console.warn("Botão não encontrado para modalFotoManutencao");
+                imgEl.src = BASE + "semimagem.jpg";
                 return;
             }
 
@@ -1571,101 +1554,93 @@
 
             linhaSelecionadaFoto = table.row($actualTr).index();
 
-            console.log('Linha selecionada:', linhaSelecionadaFoto);
+            console.log("Linha selecionada:", linhaSelecionadaFoto);
 
             const rowData = table.row(linhaSelecionadaFoto).data();
-            const fotoAtual = rowData?.imagemOcorrencia || '';
-
-            const temFoto =
-                fotoAtual &&
-                fotoAtual.toLowerCase() !== 'null' &&
-                fotoAtual !== '';
-
-            imgEl.src = temFoto
-                ? BASE + encodeURIComponent(fotoAtual)
-                : BASE + 'semimagem.jpg';
-
-            console.log('Foto carregada:', fotoAtual);
+            const fotoAtual = rowData?.imagemOcorrencia || "";
+
+            const temFoto = fotoAtual && fotoAtual.toLowerCase() !== "null" && fotoAtual !== "";
+
+            imgEl.src = temFoto ? (BASE + encodeURIComponent(fotoAtual)) : (BASE + "semimagem.jpg");
+
+            console.log("Foto carregada:", fotoAtual);
 
             const modoVisualizacao = window.modoVisualizacaoFoto === true;
 
             const elementosUpload = [
-                '#txtFileItem',
-                '#txtFileItemNovo',
-                '#divUploadFoto',
-                '#divControlesUpload',
-                '.upload-container',
-                '.k-upload',
-                '#btnGravarFoto',
-                '#btnSalvarFoto',
-                '#lblSelecionarFoto',
-                '#lblInstrucaoUpload',
+                "#txtFileItem", "#txtFileItemNovo",
+                "#divUploadFoto", "#divControlesUpload",
+                ".upload-container", ".k-upload",
+                "#btnGravarFoto", "#btnSalvarFoto",
+                "#lblSelecionarFoto", "#lblInstrucaoUpload"
             ];
 
-            elementosUpload.forEach((selector) => {
-                const el =
-                    modalEl.querySelector(selector) ||
-                    document.querySelector(selector);
-                if (el) {
-                    el.style.display = modoVisualizacao ? 'none' : '';
+            elementosUpload.forEach(selector =>
+            {
+                const el = modalEl.querySelector(selector) || document.querySelector(selector);
+                if (el)
+                {
+                    el.style.display = modoVisualizacao ? "none" : "";
                 }
             });
 
-            const inputFile = document.getElementById('txtFileItemNovo');
-            if (inputFile) {
-                const wrapper = inputFile.closest(
-                    '.mb-3, .form-group, .upload-wrapper',
-                );
-                if (wrapper) {
-                    wrapper.style.display = modoVisualizacao ? 'none' : '';
-                } else {
-                    inputFile.style.display = modoVisualizacao ? 'none' : '';
+            const inputFile = document.getElementById("txtFileItemNovo");
+            if (inputFile)
+            {
+                const wrapper = inputFile.closest(".mb-3, .form-group, .upload-wrapper");
+                if (wrapper)
+                {
+                    wrapper.style.display = modoVisualizacao ? "none" : "";
+                } else
+                {
+                    inputFile.style.display = modoVisualizacao ? "none" : "";
                 }
             }
 
-            const modalTitle = modalEl.querySelector('.modal-title');
-            if (modalTitle) {
-                modalTitle.textContent = modoVisualizacao
-                    ? 'Visualizar Foto'
-                    : 'Foto do Item';
-            }
-
-            const footerButtons = modalEl.querySelectorAll(
-                '.modal-footer button:not([data-bs-dismiss]), .modal-footer .btn-primary, .modal-footer .btn-success',
-            );
-            footerButtons.forEach((btn) => {
-                if (
-                    btn.textContent.toLowerCase().includes('gravar') ||
-                    btn.textContent.toLowerCase().includes('salvar') ||
-                    btn.classList.contains('btn-primary') ||
-                    btn.classList.contains('btn-success')
-                ) {
-                    btn.style.display = modoVisualizacao ? 'none' : '';
+            const modalTitle = modalEl.querySelector(".modal-title");
+            if (modalTitle)
+            {
+                modalTitle.textContent = modoVisualizacao ? "Visualizar Foto" : "Foto do Item";
+            }
+
+            const footerButtons = modalEl.querySelectorAll(".modal-footer button:not([data-bs-dismiss]), .modal-footer .btn-primary, .modal-footer .btn-success");
+            footerButtons.forEach(btn =>
+            {
+                if (btn.textContent.toLowerCase().includes("gravar") ||
+                    btn.textContent.toLowerCase().includes("salvar") ||
+                    btn.classList.contains("btn-primary") ||
+                    btn.classList.contains("btn-success"))
+                {
+                    btn.style.display = modoVisualizacao ? "none" : "";
                 }
             });
 
-            const botoesFechar = modalEl.querySelectorAll(
-                '[data-bs-dismiss="modal"], .btn-vinho, #btnFecharModal',
-            );
-            botoesFechar.forEach((btnFechar) => {
-                btnFechar.removeAttribute('aria-disabled');
-                btnFechar.removeAttribute('tabindex');
+            const botoesFechar = modalEl.querySelectorAll('[data-bs-dismiss="modal"], .btn-vinho, #btnFecharModal');
+            botoesFechar.forEach(btnFechar =>
+            {
+                btnFechar.removeAttribute("aria-disabled");
+                btnFechar.removeAttribute("tabindex");
                 btnFechar.disabled = false;
-                btnFechar.style.opacity = '1';
-                btnFechar.style.pointerEvents = 'auto';
-                btnFechar.style.cursor = 'pointer';
+                btnFechar.style.opacity = "1";
+                btnFechar.style.pointerEvents = "auto";
+                btnFechar.style.cursor = "pointer";
             });
-        } catch (error) {
-            console.error('Erro ao abrir modal:', error);
-            imgEl.src = BASE + 'semimagem.jpg';
+
+        } catch (error)
+        {
+            console.error("Erro ao abrir modal:", error);
+            imgEl.src = BASE + "semimagem.jpg";
             linhaSelecionadaFoto = -1;
         }
     });
 
-    modalEl.addEventListener('hide.bs.modal', function () {
-
-        setTimeout(() => {
-            document.querySelectorAll('.modal-backdrop').forEach((backdrop) => {
+    modalEl.addEventListener('hide.bs.modal', function ()
+    {
+
+        setTimeout(() =>
+        {
+            document.querySelectorAll('.modal-backdrop').forEach(backdrop =>
+            {
                 backdrop.remove();
             });
             document.body.classList.remove('modal-open');
@@ -1675,154 +1650,180 @@
     });
 })();
 
-function fecharModaisAbertos() {
-    document.querySelectorAll('.modal.show').forEach((el) => {
+function fecharModaisAbertos()
+{
+    document.querySelectorAll('.modal.show').forEach(el =>
+    {
 
         const inst = bootstrap.Modal.getOrCreateInstance(el);
         inst.hide();
     });
 }
 
-$('#txtFile').change(function (event) {
-    try {
+$("#txtFile").change(function (event)
+{
+    try
+    {
         var files = event.target.files;
-        $('#imgViewer').attr('src', window.URL.createObjectURL(files[0]));
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'change.txtFile', error);
+        $("#imgViewer").attr("src", window.URL.createObjectURL(files[0]));
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "change.txtFile", error);
     }
 });
 
-$('#txtFileItem').change(function (event) {
-    try {
+$("#txtFileItem").change(function (event)
+{
+    try
+    {
         var files = event.target.files;
         ImagemSelecionada = files[0].name;
-        var image = document.getElementById('imgViewerItem');
+        var image = document.getElementById("imgViewerItem");
         image.src = URL.createObjectURL(event.target.files[0]);
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'change.txtFileItem', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "change.txtFileItem", error);
     }
 });
 
-function fnExibeReserva() {
-    try {
-        if (document.getElementById('lstReserva').value === '1') {
-            document.getElementById('divReserva').style.display = 'block';
-        } else {
-            document.getElementById('divReserva').style.display = 'none';
-        }
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'fnExibeReserva', error);
+function fnExibeReserva()
+{
+    try
+    {
+        if (document.getElementById("lstReserva").value === "1")
+        {
+            document.getElementById("divReserva").style.display = "block";
+        } else
+        {
+            document.getElementById("divReserva").style.display = "none";
+        }
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "fnExibeReserva", error);
     }
 }
 
-$(document).ready(function () {
-    try {
-
-        document.getElementById('divOcorrencias').style.display = 'none';
-        document.getElementById('divPendencias').style.display = 'none';
-        document.getElementById('divItens').style.display = 'none';
-
-        if (
-            manutencaoId != null &&
-            manutencaoId != '00000000-0000-0000-0000-000000000000'
-        ) {
+$(document).ready(function ()
+{
+    try
+    {
+
+        document.getElementById("divOcorrencias").style.display = "none";
+        document.getElementById("divPendencias").style.display = "none";
+        document.getElementById("divItens").style.display = "none";
+
+        if (manutencaoId != null && manutencaoId != "00000000-0000-0000-0000-000000000000")
+        {
 
             ManutencaoId = manutencaoId;
 
-            var lstVeiculo =
-                document.getElementById('lstVeiculo').ej2_instances[0];
+            var lstVeiculo = document.getElementById("lstVeiculo").ej2_instances[0];
             lstVeiculo.enabled = false;
 
             VeiculoChange();
 
             var ReservaEnviado = manutencaoReservaEnviado;
-            if (ReservaEnviado === true) {
-                document.getElementById('lstReserva').value = '1';
+            if (ReservaEnviado === true)
+            {
+                document.getElementById("lstReserva").value = "1";
                 fnExibeReserva();
-            } else if (ReservaEnviado === false) {
-                document.getElementById('lstReserva').value = '0';
+            } else if (ReservaEnviado === false)
+            {
+                document.getElementById("lstReserva").value = "0";
             }
 
             var StatusOS = manutencaoStatusOS;
 
-            if (StatusOS === 'Fechada') {
-
-                $('#btnEdita').hide();
-                $('#btnAdiciona').hide();
-
-                var $keepBtn = $('#btnVoltarLista')
-                    .add('#btnVoltar')
+            if (StatusOS === "Fechada")
+            {
+
+                $("#btnEdita").hide();
+                $("#btnAdiciona").hide();
+
+                var $keepBtn = $("#btnVoltarLista")
+                    .add("#btnVoltar")
                     .add('a[href*="ListaManutencao"]')
-                    .add('a.btn.btn-vinho.form-control');
+                    .add("a.btn.btn-vinho.form-control");
 
                 var $keep = $keepBtn.add($keepBtn.parents());
 
-                $('input, select, textarea, button')
+                $("input, select, textarea, button")
                     .not($keep)
-                    .prop('disabled', true)
-                    .css('opacity', 0.5);
-
-                $('a, .btn')
+                    .prop("disabled", true)
+                    .css("opacity", 0.5);
+
+                $("a, .btn")
                     .not($keep)
-                    .attr('aria-disabled', 'true')
-                    .attr('tabindex', '-1')
-                    .on('click.block', function (e) {
-                        try {
+                    .attr("aria-disabled", "true")
+                    .attr("tabindex", "-1")
+                    .on("click.block", function (e)
+                    {
+                        try
+                        {
                             e.preventDefault();
-                        } catch (error) {
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'manutencao.js',
-                                'callback@$.not.attr.attr.on#1',
+                                "manutencao.js",
+                                "callback@$.not.attr.attr.on#1",
                                 error,
                             );
                         }
                     })
-                    .css('opacity', 0.5);
-
-                $('div').not($keep).css('pointer-events', 'none');
+                    .css("opacity", 0.5);
+
+                $("div").not($keep).css("pointer-events", "none");
 
                 $(
                     'input:disabled, select:disabled, textarea:disabled, button:disabled, a[aria-disabled="true"], .btn[aria-disabled="true"]',
-                ).css('cursor', 'not-allowed');
+                ).css("cursor", "not-allowed");
 
                 var $btnVoltar = $("#btnVoltar, a[href*='ListaManutencao']");
                 $btnVoltar
-                    .removeAttr('aria-disabled')
-                    .removeAttr('tabindex')
-                    .off('click.block')
+                    .removeAttr("aria-disabled")
+                    .removeAttr("tabindex")
+                    .off("click.block")
                     .css({
-                        opacity: '1',
-                        'pointer-events': 'auto',
-                        cursor: 'pointer',
+                        "opacity": "1",
+                        "pointer-events": "auto",
+                        "cursor": "pointer"
                     });
-                $btnVoltar.parents().css('pointer-events', 'auto');
-            }
-        } else {
-
-            document.getElementById('txtDataSolicitacao').value =
-                moment(Date()).format('YYYY-MM-DD');
-        }
-
-        try {
-            DataTable.datetime('DD/MM/YYYY');
-
-            dataTableItens = $('#tblItens').DataTable({
+                $btnVoltar.parents().css("pointer-events", "auto");
+            }
+        } else
+        {
+
+            document.getElementById("txtDataSolicitacao").value =
+                moment(Date()).format("YYYY-MM-DD");
+        }
+
+        try
+        {
+            DataTable.datetime("DD/MM/YYYY");
+
+            dataTableItens = $("#tblItens").DataTable({
                 autoWidth: false,
-                dom: 'Bfrtip',
+                dom: "Bfrtip",
                 bFilter: false,
                 buttons: [],
                 aaSorting: [],
                 columnDefs: [
-                    { targets: 0, className: 'text-center', width: '5%' },
-                    { targets: 1, className: 'text-center', width: '4%' },
-                    { targets: 2, className: 'text-center', width: '4%' },
-                    { targets: 3, className: 'text-left', width: '20%' },
+                    { targets: 0, className: "text-center", width: "5%" },
+                    { targets: 1, className: "text-center", width: "4%" },
+                    { targets: 2, className: "text-center", width: "4%" },
+                    { targets: 3, className: "text-left", width: "20%" },
                     {
                         targets: 4,
-                        className: 'text-center',
-                        width: '30%',
-                        render: function (data, type, full, meta) {
-                            try {
+                        className: "text-center",
+                        width: "30%",
+                        render: function (data, type, full, meta)
+                        {
+                            try
+                            {
                                 return `<div class="text-center">
                             <a aria-label="&#9881; (${full.descricao})"
                                data-microtip-position="top"
@@ -1830,10 +1831,12 @@
                                data-microtip-size="medium"
                                style="cursor:pointer;"
                                data-id='${data}'>${full.resumo}</a></div>`;
-                            } catch (error) {
+                            }
+                            catch (error)
+                            {
                                 Alerta.TratamentoErroComLinha(
-                                    'manutencao.js',
-                                    'render',
+                                    "manutencao.js",
+                                    "render",
                                     error,
                                 );
                             }
@@ -1841,21 +1844,21 @@
                     },
                     {
                         targets: 5,
-                        className: 'text-center',
-                        width: '5%',
-                        render: function (data, type, full, meta) {
-                            const foto = full.imagemOcorrencia || '';
+                        className: "text-center",
+                        width: "5%",
+                        render: function (data, type, full, meta)
+                        {
+                            const foto = full.imagemOcorrencia || "";
                             const fotoAttr = encodeURIComponent(foto);
                             const rowIndex = meta.row;
 
-                            const isManual =
-                                full.tipoItem === 'Inserção Manual';
+                            const isManual = full.tipoItem === "Inserção Manual";
                             const iconeRemover = isManual
-                                ? 'fa-trash-can'
-                                : 'fa-up-from-bracket';
+                                ? "fa-trash-can"
+                                : "fa-up-from-bracket";
                             const tooltipRemover = isManual
-                                ? 'Excluir Item'
-                                : 'Devolver Item';
+                                ? "Excluir Item"
+                                : "Devolver Item";
 
                             return `
                                         <div class="col-acao">
@@ -1882,12 +1885,7 @@
                                                 </button>
 
                                                 ${(() => {
-                                                    const temFoto =
-                                                        foto &&
-                                                        foto !== '' &&
-                                                        foto !== 'null' &&
-                                                        foto.toLowerCase() !==
-                                                            'semimagem.jpg';
+                                                    const temFoto = foto && foto !== '' && foto !== 'null' && foto.toLowerCase() !== 'semimagem.jpg';
                                                     if (temFoto) {
                                                         return `<button type="button"
                                                         class="btn btn-sm btnFoto js-ver-foto"
@@ -1941,625 +1939,525 @@
                                             </div>
                                         </div>
                                     `;
-                        },
+                        }
                     },
-                    {
-                        targets: 6,
-                        className: 'text-center',
-                        width: '10%',
-                        visible: false,
-                    },
-                    {
-                        targets: 7,
-                        className: 'text-center',
-                        width: '10%',
-                        visible: false,
-                    },
-                    {
-                        targets: 8,
-                        className: 'text-center',
-                        width: '10%',
-                        visible: false,
-                    },
-                    {
-                        targets: 9,
-                        className: 'text-center',
-                        width: '10%',
-                        visible: false,
-                    },
-                    {
-                        targets: 10,
-                        className: 'text-center',
-                        width: '10%',
-                        visible: false,
-                    },
-                    {
-                        targets: 11,
-                        className: 'text-center',
-                        width: '10%',
-                        visible: false,
-                    },
+                    { targets: 6, className: "text-center", width: "10%", visible: false },
+                    { targets: 7, className: "text-center", width: "10%", visible: false },
+                    { targets: 8, className: "text-center", width: "10%", visible: false },
+                    { targets: 9, className: "text-center", width: "10%", visible: false },
+                    { targets: 10, className: "text-center", width: "10%", visible: false },
+                    { targets: 11, className: "text-center", width: "10%", visible: false },
                 ],
 
                 responsive: true,
                 ajax: {
-                    url: '/api/manutencao/ItensOS',
+                    url: "/api/manutencao/ItensOS",
                     data: { Id: ManutencaoId },
-                    type: 'GET',
-                    datatype: 'json',
+                    type: "GET",
+                    datatype: "json",
                 },
                 columns: [
-                    { data: 'tipoItem' },
-                    { data: 'numFicha' },
-                    { data: 'dataItem' },
-                    { data: 'nomeMotorista' },
-                    { data: 'resumo' },
-                    { data: 'itemManutencaoId' },
-                    { data: 'itemManutencaoId' },
-                    { data: 'descricao' },
-                    { data: 'resumo' },
-                    { data: 'motoristaId' },
-                    { data: 'imagemOcorrencia' },
-                    { data: 'viagemId' },
+                    { data: "tipoItem" },
+                    { data: "numFicha" },
+                    { data: "dataItem" },
+                    { data: "nomeMotorista" },
+                    { data: "resumo" },
+                    { data: "itemManutencaoId" },
+                    { data: "itemManutencaoId" },
+                    { data: "descricao" },
+                    { data: "resumo" },
+                    { data: "motoristaId" },
+                    { data: "imagemOcorrencia" },
+                    { data: "viagemId" },
                 ],
 
                 language: {
-                    emptyTable: 'Nenhum registro encontrado',
-                    info: 'Mostrando de _START_ até _END_ de _TOTAL_ registros',
-                    infoEmpty: 'Mostrando 0 até 0 de 0 registros',
-                    infoFiltered: '(Filtrados de _MAX_ registros)',
-                    infoThousands: '.',
-                    loadingRecords: 'Carregando...',
-                    processing: 'Processando...',
-                    zeroRecords: 'Nenhum registro encontrado',
-                    search: 'Pesquisar',
+                    emptyTable: "Nenhum registro encontrado",
+                    info: "Mostrando de _START_ até _END_ de _TOTAL_ registros",
+                    infoEmpty: "Mostrando 0 até 0 de 0 registros",
+                    infoFiltered: "(Filtrados de _MAX_ registros)",
+                    infoThousands: ".",
+                    loadingRecords: "Carregando...",
+                    processing: "Processando...",
+                    zeroRecords: "Nenhum registro encontrado",
+                    search: "Pesquisar",
                     paginate: {
-                        next: 'Próximo',
-                        previous: 'Anterior',
-                        first: 'Primeiro',
-                        last: 'Último',
+                        next: "Próximo",
+                        previous: "Anterior",
+                        first: "Primeiro",
+                        last: "Último",
                     },
                     aria: {
-                        sortAscending: ': Ordenar colunas de forma ascendente',
-                        sortDescending:
-                            ': Ordenar colunas de forma descendente',
+                        sortAscending: ": Ordenar colunas de forma ascendente",
+                        sortDescending: ": Ordenar colunas de forma descendente",
                     },
                     select: {
-                        rows: {
-                            _: 'Selecionado %d linhas',
-                            1: 'Selecionado 1 linha',
-                        },
-                        cells: {
-                            1: '1 célula selecionada',
-                            _: '%d células selecionadas',
-                        },
-                        columns: {
-                            1: '1 coluna selecionada',
-                            _: '%d colunas selecionadas',
-                        },
+                        rows: { _: "Selecionado %d linhas", 1: "Selecionado 1 linha" },
+                        cells: { 1: "1 célula selecionada", _: "%d células selecionadas" },
+                        columns: { 1: "1 coluna selecionada", _: "%d colunas selecionadas" },
                     },
                     buttons: {
                         copySuccess: {
-                            1: 'Uma linha copiada com sucesso',
-                            _: '%d linhas copiadas com sucesso',
+                            1: "Uma linha copiada com sucesso",
+                            _: "%d linhas copiadas com sucesso",
                         },
                         collection:
                             'Coleção <span class="ui-button-icon-primary ui-icon ui-icon-triangle-1-s"></span>',
-                        colvis: 'Visibilidade da Coluna',
-                        colvisRestore: 'Restaurar Visibilidade',
-                        copy: 'Copiar',
+                        colvis: "Visibilidade da Coluna",
+                        colvisRestore: "Restaurar Visibilidade",
+                        copy: "Copiar",
                         copyKeys:
-                            'Pressione ctrl ou u2318 + C para copiar os dados da tabela para a área de transferência do sistema. Para cancelar, clique nesta mensagem ou pressione Esc..',
-                        copyTitle: 'Copiar para a Área de Transferência',
-                        csv: 'CSV',
-                        excel: 'Excel',
+                            "Pressione ctrl ou u2318 + C para copiar os dados da tabela para a área de transferência do sistema. Para cancelar, clique nesta mensagem ou pressione Esc..",
+                        copyTitle: "Copiar para a Área de Transferência",
+                        csv: "CSV",
+                        excel: "Excel",
                         pageLength: {
-                            '-1': 'Mostrar todos os registros',
-                            _: 'Mostrar %d registros',
+                            "-1": "Mostrar todos os registros",
+                            _: "Mostrar %d registros",
                         },
-                        pdf: 'PDF',
-                        print: 'Imprimir',
+                        pdf: "PDF",
+                        print: "Imprimir",
                     },
                     autoFill: {
-                        cancel: 'Cancelar',
-                        fill: 'Preencher todas as células com',
-                        fillHorizontal: 'Preencher células horizontalmente',
-                        fillVertical: 'Preencher células verticalmente',
+                        cancel: "Cancelar",
+                        fill: "Preencher todas as células com",
+                        fillHorizontal: "Preencher células horizontalmente",
+                        fillVertical: "Preencher células verticalmente",
                     },
-                    lengthMenu: 'Exibir _MENU_ resultados por página',
+                    lengthMenu: "Exibir _MENU_ resultados por página",
                     searchBuilder: {
-                        add: 'Adicionar Condição',
-                        button: {
-                            0: 'Construtor de Pesquisa',
-                            _: 'Construtor de Pesquisa (%d)',
-                        },
-                        clearAll: 'Limpar Tudo',
-                        condition: 'Condição',
+                        add: "Adicionar Condição",
+                        button: { 0: "Construtor de Pesquisa", _: "Construtor de Pesquisa (%d)" },
+                        clearAll: "Limpar Tudo",
+                        condition: "Condição",
                         conditions: {
                             date: {
-                                after: 'Depois',
-                                before: 'Antes',
-                                between: 'Entre',
-                                empty: 'Vazio',
-                                equals: 'Igual',
-                                not: 'Não',
-                                notBetween: 'Não Entre',
-                                notEmpty: 'Não Vazio',
+                                after: "Depois",
+                                before: "Antes",
+                                between: "Entre",
+                                empty: "Vazio",
+                                equals: "Igual",
+                                not: "Não",
+                                notBetween: "Não Entre",
+                                notEmpty: "Não Vazio",
                             },
                             number: {
-                                between: 'Entre',
-                                empty: 'Vazio',
-                                equals: 'Igual',
-                                gt: 'Maior Que',
-                                gte: 'Maior ou Igual a',
-                                lt: 'Menor Que',
-                                lte: 'Menor ou Igual a',
-                                not: 'Não',
-                                notBetween: 'Não Entre',
-                                notEmpty: 'Não Vazio',
+                                between: "Entre",
+                                empty: "Vazio",
+                                equals: "Igual",
+                                gt: "Maior Que",
+                                gte: "Maior ou Igual a",
+                                lt: "Menor Que",
+                                lte: "Menor ou Igual a",
+                                not: "Não",
+                                notBetween: "Não Entre",
+                                notEmpty: "Não Vazio",
                             },
                             string: {
-                                contains: 'Contém',
-                                empty: 'Vazio',
-                                endsWith: 'Termina Com',
-                                equals: 'Igual',
-                                not: 'Não',
-                                notEmpty: 'Não Vazio',
-                                startsWith: 'Começa Com',
+                                contains: "Contém",
+                                empty: "Vazio",
+                                endsWith: "Termina Com",
+                                equals: "Igual",
+                                not: "Não",
+                                notEmpty: "Não Vazio",
+                                startsWith: "Começa Com",
                             },
                             array: {
-                                contains: 'Contém',
-                                empty: 'Vazio',
-                                equals: 'Igual à',
-                                not: 'Não',
-                                notEmpty: 'Não vazio',
-                                without: 'Não possui',
+                                contains: "Contém",
+                                empty: "Vazio",
+                                equals: "Igual à",
+                                not: "Não",
+                                notEmpty: "Não vazio",
+                                without: "Não possui",
                             },
                         },
-                        data: 'Data',
-                        deleteTitle: 'Excluir regra de filtragem',
-                        logicAnd: 'E',
-                        logicOr: 'Ou',
-                        title: {
-                            0: 'Construtor de Pesquisa',
-                            _: 'Construtor de Pesquisa (%d)',
-                        },
-                        value: 'Valor',
+                        data: "Data",
+                        deleteTitle: "Excluir regra de filtragem",
+                        logicAnd: "E",
+                        logicOr: "Ou",
+                        title: { 0: "Construtor de Pesquisa", _: "Construtor de Pesquisa (%d)" },
+                        value: "Valor",
                     },
                     searchPanes: {
-                        clearMessage: 'Limpar Tudo',
-                        collapse: {
-                            0: 'Painéis de Pesquisa',
-                            _: 'Painéis de Pesquisa (%d)',
-                        },
-                        count: '{total}',
-                        countFiltered: '{shown} ({total})',
-                        emptyPanes: 'Nenhum Painel de Pesquisa',
-                        loadMessage: 'Carregando Painéis de Pesquisa...',
-                        title: 'Filtros Ativos',
+                        clearMessage: "Limpar Tudo",
+                        collapse: { 0: "Painéis de Pesquisa", _: "Painéis de Pesquisa (%d)" },
+                        count: "{total}",
+                        countFiltered: "{shown} ({total})",
+                        emptyPanes: "Nenhum Painel de Pesquisa",
+                        loadMessage: "Carregando Painéis de Pesquisa...",
+                        title: "Filtros Ativos",
                     },
-                    thousands: '.',
+                    thousands: ".",
                     datetime: {
-                        previous: 'Anterior',
-                        next: 'Próximo',
-                        hours: 'Hora',
-                        minutes: 'Minuto',
-                        seconds: 'Segundo',
-                        amPm: ['am', 'pm'],
-                        unknown: '-',
+                        previous: "Anterior",
+                        next: "Próximo",
+                        hours: "Hora",
+                        minutes: "Minuto",
+                        seconds: "Segundo",
+                        amPm: ["am", "pm"],
+                        unknown: "-",
                         months: {
-                            0: 'Janeiro',
-                            1: 'Fevereiro',
-                            2: 'Março',
-                            3: 'Abril',
-                            4: 'Maio',
-                            5: 'Junho',
-                            6: 'Julho',
-                            7: 'Agosto',
-                            8: 'Setembro',
-                            9: 'Outubro',
-                            10: 'Novembro',
-                            11: 'Dezembro',
+                            0: "Janeiro",
+                            1: "Fevereiro",
+                            2: "Março",
+                            3: "Abril",
+                            4: "Maio",
+                            5: "Junho",
+                            6: "Julho",
+                            7: "Agosto",
+                            8: "Setembro",
+                            9: "Outubro",
+                            10: "Novembro",
+                            11: "Dezembro",
                         },
                         weekdays: [
-                            'Domingo',
-                            'Segunda-feira',
-                            'Terça-feira',
-                            'Quarta-feira',
-                            'Quinte-feira',
-                            'Sexta-feira',
-                            'Sábado',
+                            "Domingo",
+                            "Segunda-feira",
+                            "Terça-feira",
+                            "Quarta-feira",
+                            "Quinte-feira",
+                            "Sexta-feira",
+                            "Sábado",
                         ],
                     },
                     editor: {
-                        close: 'Fechar',
-                        create: {
-                            button: 'Novo',
-                            submit: 'Criar',
-                            title: 'Criar novo registro',
-                        },
-                        edit: {
-                            button: 'Editar',
-                            submit: 'Atualizar',
-                            title: 'Editar registro',
-                        },
+                        close: "Fechar",
+                        create: { button: "Novo", submit: "Criar", title: "Criar novo registro" },
+                        edit: { button: "Editar", submit: "Atualizar", title: "Editar registro" },
                         error: {
                             system: 'Ocorreu um erro no sistema (<a target="\\" rel="nofollow" href="\\">Mais informações</a>).',
                         },
                         multi: {
                             noMulti:
-                                'Essa entrada pode ser editada individualmente, mas não como parte do grupo',
-                            restore: 'Desfazer alterações',
-                            title: 'Multiplos valores',
-                            info: 'Os itens selecionados contêm valores diferentes...',
+                                "Essa entrada pode ser editada individualmente, mas não como parte do grupo",
+                            restore: "Desfazer alterações",
+                            title: "Multiplos valores",
+                            info: "Os itens selecionados contêm valores diferentes...",
                         },
                         remove: {
-                            button: 'Remover',
+                            button: "Remover",
                             confirm: {
-                                _: 'Tem certeza que quer deletar %d linhas?',
-                                1: 'Tem certeza que quer deletar 1 linha?',
+                                _: "Tem certeza que quer deletar %d linhas?",
+                                1: "Tem certeza que quer deletar 1 linha?",
                             },
-                            submit: 'Remover',
-                            title: 'Remover registro',
+                            submit: "Remover",
+                            title: "Remover registro",
                         },
                     },
-                    decimal: ',',
+                    decimal: ",",
                 },
-                width: '100%',
-
-                drawCallback: function (settings) {
-                    try {
-
-                        var statusOS =
-                            window.manutencaoStatusOS ||
-                            manutencaoStatusOS ||
-                            '';
-                        var osFechadaOuCancelada =
-                            statusOS === 'Fechada' || statusOS === 'Cancelada';
-
-                        console.log(
-                            'drawCallback - Status OS:',
-                            statusOS,
-                            '| Fechada/Cancelada:',
-                            osFechadaOuCancelada,
-                        );
-
-                        if (osFechadaOuCancelada) {
-
-                            var $botoesF = $(
-                                "#tblItens .js-ver-foto, #tblItens .btnFoto, #tblItens [data-bs-target='#modalFotoManutencao']",
-                            );
-                            console.log(
-                                'Botões de foto encontrados:',
-                                $botoesF.length,
-                            );
-
-                            $botoesF.each(function () {
+                width: "100%",
+
+                drawCallback: function(settings)
+                {
+                    try
+                    {
+
+                        var statusOS = window.manutencaoStatusOS || manutencaoStatusOS || "";
+                        var osFechadaOuCancelada = (statusOS === "Fechada" || statusOS === "Cancelada");
+
+                        console.log("drawCallback - Status OS:", statusOS, "| Fechada/Cancelada:", osFechadaOuCancelada);
+
+                        if (osFechadaOuCancelada)
+                        {
+
+                            var $botoesF = $("#tblItens .js-ver-foto, #tblItens .btnFoto, #tblItens [data-bs-target='#modalFotoManutencao']");
+                            console.log("Botões de foto encontrados:", $botoesF.length);
+
+                            $botoesF.each(function() {
                                 $(this)
-                                    .removeAttr('aria-disabled')
-                                    .removeAttr('tabindex')
-                                    .off('click.block')
-                                    .prop('disabled', false)
+                                    .removeAttr("aria-disabled")
+                                    .removeAttr("tabindex")
+                                    .off("click.block")
+                                    .prop("disabled", false)
                                     .css({
-                                        opacity: '1',
-                                        'pointer-events': 'auto',
-                                        cursor: 'pointer',
+                                        "opacity": "1",
+                                        "pointer-events": "auto",
+                                        "cursor": "pointer"
                                     });
                             });
 
-                            $('#tblItens').css('pointer-events', 'auto');
-                            $('#tblItens tbody').css('pointer-events', 'auto');
-                            $('#tblItens td').css('pointer-events', 'auto');
-                            $botoesF
-                                .parents('td, tr, div, .col-acao')
-                                .css('pointer-events', 'auto');
-
-                            var $botoesRemover = $(
-                                '#tblItens .js-remover-item, #tblItens .btn-delete',
-                            );
-                            $botoesRemover.prop('disabled', true).css({
-                                opacity: '0.35',
-                                'pointer-events': 'none',
-                                cursor: 'not-allowed',
-                            });
+                            $("#tblItens").css("pointer-events", "auto");
+                            $("#tblItens tbody").css("pointer-events", "auto");
+                            $("#tblItens td").css("pointer-events", "auto");
+                            $botoesF.parents("td, tr, div, .col-acao").css("pointer-events", "auto");
+
+                            var $botoesRemover = $("#tblItens .js-remover-item, #tblItens .btn-delete");
+                            $botoesRemover
+                                .prop("disabled", true)
+                                .css({
+                                    "opacity": "0.35",
+                                    "pointer-events": "none",
+                                    "cursor": "not-allowed"
+                                });
                         }
-                    } catch (error) {
-                        console.error('Erro no drawCallback:', error);
                     }
-                },
+                    catch (error)
+                    {
+                        console.error("Erro no drawCallback:", error);
+                    }
+                }
             });
-        } catch (error) {
-            TratamentoErroComLinha(
-                'manutencao.js',
-                'document.ready.dataTableItensInit',
-                error,
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'manutencao.js',
-            'callback@$.ready#0',
-            error,
-        );
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("manutencao.js", "document.ready.dataTableItensInit", error);
+        }
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("manutencao.js", "callback@$.ready#0", error);
     }
 });
 
-$('#btnAdiciona').click(function (event) {
-    try {
+$("#btnAdiciona").click(function (event)
+{
+    try
+    {
         event.preventDefault();
         InsereRegistro();
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'click.btnAdiciona', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "click.btnAdiciona", error);
     }
 });
 
-$('#btnEdita').click(function (event) {
-    try {
+$("#btnEdita").click(function (event)
+{
+    try
+    {
         event.preventDefault();
         InsereRegistro();
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'click.btnEdita', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "click.btnEdita", error);
     }
 });
 
-function PreenchePagina() {
-    try {
-
-        VeiculoId =
-            document.getElementById('lstVeiculo').ej2_instances[0].value;
-
-        if (VeiculoId != '' && VeiculoId != null) {
-            document.getElementById('divOcorrencias').style.display = 'block';
-            document.getElementById('divPendencias').style.display = 'block';
-            document.getElementById('divItens').style.display = 'block';
-
-            try {
-                if ($('#txtOS').val() === '' || $('#txtOS').val() === null) {
-
-                    document.getElementById('txtDataSolicitacao').value =
-                        moment(Date()).format('YYYY-MM-DD');
-                    let DataOS =
-                        document.getElementById('txtDataSolicitacao').value;
-                    console.log('DataOS 1: ' + DataOS);
+function PreenchePagina()
+{
+    try
+    {
+
+        VeiculoId = document.getElementById("lstVeiculo").ej2_instances[0].value;
+
+        if (VeiculoId != "" && VeiculoId != null)
+        {
+            document.getElementById("divOcorrencias").style.display = "block";
+            document.getElementById("divPendencias").style.display = "block";
+            document.getElementById("divItens").style.display = "block";
+
+            try
+            {
+                if ($("#txtOS").val() === "" || $("#txtOS").val() === null)
+                {
+
+                    document.getElementById("txtDataSolicitacao").value =
+                        moment(Date()).format("YYYY-MM-DD");
+                    let DataOS = document.getElementById("txtDataSolicitacao").value;
+                    console.log("DataOS 1: " + DataOS);
                     DataOS =
                         DataOS.substring(0, 4) +
-                        '.' +
+                        "." +
                         DataOS.substring(5, 7) +
-                        '.' +
+                        "." +
                         DataOS.substring(8, 10) +
-                        '-';
-                    console.log('DataOS 2: ' + DataOS);
-                    let VeiculoPlaca =
-                        document.getElementById('lstVeiculo').ej2_instances[0]
-                            .text;
-                    let indiceEspaco = VeiculoPlaca.indexOf(' ');
+                        "-";
+                    console.log("DataOS 2: " + DataOS);
+                    let VeiculoPlaca = document.getElementById("lstVeiculo").ej2_instances[0].text;
+                    let indiceEspaco = VeiculoPlaca.indexOf(" ");
                     VeiculoPlaca = VeiculoPlaca.substring(0, indiceEspaco);
-                    VeiculoPlaca = VeiculoPlaca.replace('-', '').replace(
-                        ' ',
-                        '',
-                    );
-                    console.log('VeiculoPlaca: ' + VeiculoPlaca);
-                    $('#txtOS').attr('value', DataOS + VeiculoPlaca);
+                    VeiculoPlaca = VeiculoPlaca.replace("-", "").replace(" ", "");
+                    console.log("VeiculoPlaca: " + VeiculoPlaca);
+                    $("#txtOS").attr("value", DataOS + VeiculoPlaca);
                 }
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'manutencao.js',
-                    'VeiculoChange.MontaNumeroOS',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("manutencao.js", "VeiculoChange.MontaNumeroOS", error);
             }
 
             var languageDataTable = {
-                emptyTable: 'Nenhum registro encontrado',
-                info: 'Mostrando de _START_ até _END_ de _TOTAL_ registros',
-                infoEmpty: 'Mostrando 0 até 0 de 0 registros',
-                infoFiltered: '(Filtrados de _MAX_ registros)',
-                loadingRecords: 'Carregando...',
-                processing: 'Processando...',
-                zeroRecords: 'Nenhum registro encontrado',
-                search: 'Pesquisar',
+                emptyTable: "Nenhum registro encontrado",
+                info: "Mostrando de _START_ até _END_ de _TOTAL_ registros",
+                infoEmpty: "Mostrando 0 até 0 de 0 registros",
+                infoFiltered: "(Filtrados de _MAX_ registros)",
+                loadingRecords: "Carregando...",
+                processing: "Processando...",
+                zeroRecords: "Nenhum registro encontrado",
+                search: "Pesquisar",
                 paginate: {
-                    next: 'Próximo',
-                    previous: 'Anterior',
-                    first: 'Primeiro',
-                    last: 'Último',
+                    next: "Próximo",
+                    previous: "Anterior",
+                    first: "Primeiro",
+                    last: "Último",
                 },
-                value: 'Valor',
-                leftTitle: 'Critérios Externos',
-                rightTitle: 'Critérios Internos',
+                value: "Valor",
+                leftTitle: "Critérios Externos",
+                rightTitle: "Critérios Internos",
                 searchPanes: {
-                    clearMessage: 'Limpar Tudo',
+                    clearMessage: "Limpar Tudo",
                     collapse: {
-                        0: 'Painéis de Pesquisa',
-                        _: 'Painéis de Pesquisa (%d)',
+                        0: "Painéis de Pesquisa",
+                        _: "Painéis de Pesquisa (%d)",
                     },
-                    count: '{total}',
-                    countFiltered: '{shown} ({total})',
-                    emptyPanes: 'Nenhum Painel de Pesquisa',
-                    loadMessage: 'Carregando Painéis de Pesquisa...',
-                    title: 'Filtros Ativos',
+                    count: "{total}",
+                    countFiltered: "{shown} ({total})",
+                    emptyPanes: "Nenhum Painel de Pesquisa",
+                    loadMessage: "Carregando Painéis de Pesquisa...",
+                    title: "Filtros Ativos",
                 },
-                thousands: '.',
+                thousands: ".",
                 datetime: {
-                    previous: 'Anterior',
-                    next: 'Próximo',
-                    hours: 'Hora',
-                    minutes: 'Minuto',
-                    seconds: 'Segundo',
-                    amPm: ['am', 'pm'],
-
-                    unknown: '-',
+                    previous: "Anterior",
+                    next: "Próximo",
+                    hours: "Hora",
+                    minutes: "Minuto",
+                    seconds: "Segundo",
+                    amPm: ["am", "pm"],
+
+                    unknown: "-",
                     months: {
-                        0: 'Janeiro',
-                        1: 'Fevereiro',
-                        2: 'Março',
-                        3: 'Abril',
-                        4: 'Maio',
-                        5: 'Junho',
-                        6: 'Julho',
-                        7: 'Agosto',
-                        8: 'Setembro',
-                        9: 'Outubro',
-                        10: 'Novembro',
-                        11: 'Dezembro',
+                        0: "Janeiro",
+                        1: "Fevereiro",
+                        2: "Março",
+                        3: "Abril",
+                        4: "Maio",
+                        5: "Junho",
+                        6: "Julho",
+                        7: "Agosto",
+                        8: "Setembro",
+                        9: "Outubro",
+                        10: "Novembro",
+                        11: "Dezembro",
                     },
                     weekdays: [
-                        'Domingo',
-                        'Segunda-feira',
-                        'Terça-feira',
-                        'Quarta-feira',
-                        'Quinte-feira',
-                        'Sexta-feira',
-                        'Sábado',
+                        "Domingo",
+                        "Segunda-feira",
+                        "Terça-feira",
+                        "Quarta-feira",
+                        "Quinte-feira",
+                        "Sexta-feira",
+                        "Sábado",
                     ],
                 },
                 editor: {
-                    close: 'Fechar',
+                    close: "Fechar",
                     create: {
-                        button: 'Novo',
-                        submit: 'Criar',
-                        title: 'Criar novo registro',
+                        button: "Novo",
+                        submit: "Criar",
+                        title: "Criar novo registro",
                     },
                     edit: {
-                        button: 'Editar',
-                        submit: 'Atualizar',
-                        title: 'Editar registro',
+                        button: "Editar",
+                        submit: "Atualizar",
+                        title: "Editar registro",
                     },
                     error: {
                         system: 'Ocorreu um erro no sistema (<a target="\\" rel="nofollow" href="\\">Mais informações<\/a>).',
                     },
                     multi: {
                         noMulti:
-                            'Essa entrada pode ser editada individualmente, mas não como parte do grupo',
-                        restore: 'Desfazer alterações',
-                        title: 'Multiplos valores',
-                        info: 'Os itens selecionados contêm valores diferentes para esta entrada. Para editar e definir todos os itens para esta entrada com o mesmo valor, clique ou toque aqui, caso contrário, eles manterío seus valores individuais.',
+                            "Essa entrada pode ser editada individualmente, mas não como parte do grupo",
+                        restore: "Desfazer alterações",
+                        title: "Multiplos valores",
+                        info: "Os itens selecionados contêm valores diferentes para esta entrada. Para editar e definir todos os itens para esta entrada com o mesmo valor, clique ou toque aqui, caso contrário, eles manterío seus valores individuais.",
                     },
                     remove: {
-                        button: 'Remover',
+                        button: "Remover",
                         confirm: {
-                            _: 'Tem certeza que quer deletar %d linhas?',
-                            1: 'Tem certeza que quer deletar 1 linha?',
+                            _: "Tem certeza que quer deletar %d linhas?",
+                            1: "Tem certeza que quer deletar 1 linha?",
                         },
-                        submit: 'Remover',
-                        title: 'Remover registro',
+                        submit: "Remover",
+                        title: "Remover registro",
                     },
                 },
-                decimal: ',',
+                decimal: ",",
             };
 
-            var osFechada =
-                typeof manutencaoStatusOS !== 'undefined' &&
-                (manutencaoStatusOS === 'Fechada' ||
-                    manutencaoStatusOS === 'Cancelada');
-
-            if (osFechada) {
-                document.getElementById('divOcorrencias').style.display =
-                    'none';
-                document.getElementById('divPendencias').style.display = 'none';
-            }
-
-            try {
-                DataTable.datetime('DD/MM/YYYY');
-
-                if (!osFechada) {
-                    if (
-                        typeof dataTableOcorrencias !== 'undefined' &&
-                        dataTableOcorrencias
-                    ) {
+            var osFechada = typeof manutencaoStatusOS !== "undefined" &&
+                           (manutencaoStatusOS === "Fechada" || manutencaoStatusOS === "Cancelada");
+
+            if (osFechada)
+            {
+                document.getElementById("divOcorrencias").style.display = "none";
+                document.getElementById("divPendencias").style.display = "none";
+            }
+
+            try
+            {
+                DataTable.datetime("DD/MM/YYYY");
+
+                if (!osFechada)
+                {
+                    if (typeof dataTableOcorrencias !== "undefined" && dataTableOcorrencias)
+                    {
                         dataTableOcorrencias.destroy();
                     }
-                    $('#tblOcorrencia tbody').empty();
-
-                    dataTableOcorrencias = $('#tblOcorrencia').DataTable({
-                        autoWidth: false,
-                        dom: 'Bfrtip',
-                        bFilter: false,
-                        buttons: [],
-                        aaSorting: [],
-                        columnDefs: [
+                    $("#tblOcorrencia tbody").empty();
+
+                    dataTableOcorrencias = $("#tblOcorrencia").DataTable({
+                    autoWidth: false,
+                    dom: "Bfrtip",
+                    bFilter: false,
+                    buttons: [],
+                    aaSorting: [],
+                    columnDefs: [
+                        { targets: 0, className: "text-center", width: "4%" },
+                        { targets: 1, className: "text-center", width: "4%" },
+                        { targets: 2, className: "text-left", width: "20%" },
+                        {
+                            targets: 3,
+                            className: "text-center",
+                            width: "30%",
+                            render: function (data, type, full)
                             {
-                                targets: 0,
-                                className: 'text-center',
-                                width: '4%',
-                            },
-                            {
-                                targets: 1,
-                                className: 'text-center',
-                                width: '4%',
-                            },
-                            {
-                                targets: 2,
-                                className: 'text-left',
-                                width: '20%',
-                            },
-                            {
-                                targets: 3,
-                                className: 'text-center',
-                                width: '30%',
-                                render: function (data, type, full) {
-                                    try {
-                                        return `<div class="text-center">
+                                try
+                                {
+                                    return `<div class="text-center">
                                                 <a aria-label="&#9881; (${removeHTML(full.descricaoOcorrencia)})" data-microtip-position="top" role="tooltip" data-microtip-size="medium" style="cursor:pointer;"
                                                 data-id='${data}'>${full.resumoOcorrencia}</a></div>`;
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'manutencao.js',
-                                            'render',
-                                            error,
-                                        );
-                                    }
-                                },
+                                }
+                                catch (error)
+                                {
+                                    Alerta.TratamentoErroComLinha(
+                                        "manutencao.js",
+                                        "render",
+                                        error,
+                                    );
+                                }
                             },
+                        },
+                        { targets: 4, className: "text-center", width: "5%" },
+                        { targets: 5, className: "text-center", width: "5%", visible: false },
+                        { targets: 6, className: "text-center", width: "5%", visible: false },
+                        { targets: 7, className: "text-center", width: "10%", visible: false },
+                        { targets: 8, className: "text-center", width: "10%", visible: false },
+
+                    ],
+
+                    responsive: true,
+                    ajax: {
+                        url: "/api/manutencao/OcorrenciasVeiculosManutencao",
+                        data: { id: VeiculoId },
+                        type: "GET",
+                        datatype: "json",
+                    },
+                    columns: [
+                        { data: "noFichaVistoria" },
+                        { data: "dataInicial" },
+                        { data: "nomeMotorista" },
+                        { data: "resumoOcorrencia" },
+                        {
+                            data: "viagemId",
+                            render: function (data, type, full)
                             {
-                                targets: 4,
-                                className: 'text-center',
-                                width: '5%',
-                            },
-                            {
-                                targets: 5,
-                                className: 'text-center',
-                                width: '5%',
-                                visible: false,
-                            },
-                            {
-                                targets: 6,
-                                className: 'text-center',
-                                width: '5%',
-                                visible: false,
-                            },
-                            {
-                                targets: 7,
-                                className: 'text-center',
-                                width: '10%',
-                                visible: false,
-                            },
-                            {
-                                targets: 8,
-                                className: 'text-center',
-                                width: '10%',
-                                visible: false,
-                            },
-
-                        ],
-
-                        responsive: true,
-                        ajax: {
-                            url: '/api/manutencao/OcorrenciasVeiculosManutencao',
-                            data: { id: VeiculoId },
-                            type: 'GET',
-                            datatype: 'json',
-                        },
-                        columns: [
-                            { data: 'noFichaVistoria' },
-                            { data: 'dataInicial' },
-                            { data: 'nomeMotorista' },
-                            { data: 'resumoOcorrencia' },
-                            {
-                                data: 'viagemId',
-                                render: function (data, type, full) {
-                                    try {
-                                        return `
+                                try
+                                {
+                                    return `
                                                 <div class="col-acao">
                                                     <div class="d-flex gap-2 justify-content-center">
                                                         <button type="button"
@@ -2576,8 +2474,8 @@
                                                                 data-ficha=${JSON.stringify(full.noFichaVistoria)}
                                                                 data-data=${JSON.stringify(full.dataInicial)}
                                                                 data-motorista=${JSON.stringify(full.nomeMotorista)}
-                                                                data-resumo=${JSON.stringify(full.resumoOcorrencia || '')}
-                                                                data-descricao=${JSON.stringify(removeHTML(full.descricaoOcorrencia || ''))}
+                                                                data-resumo=${JSON.stringify(full.resumoOcorrencia || "")}
+                                                                data-descricao=${JSON.stringify(removeHTML(full.descricaoOcorrencia || ""))}
                                                                 data-motorista-id=${JSON.stringify(full.motoristaId)}
                                                                 data-imagem=${JSON.stringify(full.imagemOcorrencia)}
                                                                 data-item-id=${JSON.stringify(full.itemManutencaoId)}
@@ -2592,16 +2490,8 @@
                                                         </button>
 
                                                         ${(() => {
-                                                            const img =
-                                                                full.imagemOcorrencia ||
-                                                                '';
-                                                            const temFoto =
-                                                                img &&
-                                                                img !== '' &&
-                                                                img !==
-                                                                    'null' &&
-                                                                img.toLowerCase() !==
-                                                                    'semimagem.jpg';
+                                                            const img = full.imagemOcorrencia || '';
+                                                            const temFoto = img && img !== '' && img !== 'null' && img.toLowerCase() !== 'semimagem.jpg';
                                                             if (temFoto) {
                                                                 return `<button type="button"
                                                                 class="btn btn-sm js-ver-foto"
@@ -2650,145 +2540,111 @@
                                                     </div>
                                                 </div>
                                             `;
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'ocorrencias.js',
-                                            'render',
-                                            error,
-                                        );
-                                    }
-                                },
+                                }
+                                catch (error)
+                                {
+                                    Alerta.TratamentoErroComLinha(
+                                        "ocorrencias.js",
+                                        "render",
+                                        error,
+                                    );
+                                }
                             },
-                            { data: 'descricaoOcorrencia' },
-                            { data: 'motoristaId' },
-                            { data: 'imagemOcorrencia' },
-                            { data: 'itemManutencaoId' },
-
-                        ],
-
-                        language: languageDataTable,
-                        width: '100%',
-                    });
+                        }, { data: "descricaoOcorrencia" },
+                        { data: "motoristaId" },
+                        { data: "imagemOcorrencia" },
+                        { data: "itemManutencaoId" },
+
+                    ],
+
+                    language: languageDataTable,
+                    width: "100%",
+                });
                 }
-            } catch (error) {
+            }
+            catch (error)
+            {
                 TratamentoErroComLinha(
-                    'manutencao.js',
-                    'VeiculoChange.DataTableOcorrencias',
+                    "manutencao.js",
+                    "VeiculoChange.DataTableOcorrencias",
                     error,
                 );
             }
-            try {
-
-                if (!osFechada) {
-                    if (
-                        typeof dataTablePendencias !== 'undefined' &&
-                        dataTablePendencias
-                    ) {
+            try
+            {
+
+                if (!osFechada)
+                {
+                    if (typeof dataTablePendencias !== "undefined" && dataTablePendencias)
+                    {
                         dataTablePendencias.destroy();
                     }
-                    $('#tblPendencia tbody').empty();
-
-                    dataTablePendencias = $('#tblPendencia').DataTable({
-                        autoWidth: false,
-                        dom: 'Bfrtip',
-                        bFilter: false,
-                        buttons: [],
-                        aaSorting: [],
-                        columnDefs: [
+                    $("#tblPendencia tbody").empty();
+
+                    dataTablePendencias = $("#tblPendencia").DataTable({
+                    autoWidth: false,
+                    dom: "Bfrtip",
+                    bFilter: false,
+                    buttons: [],
+                    aaSorting: [],
+                    columnDefs: [
+                        { targets: 0, className: "text-center", width: "4%" },
+                        { targets: 1, className: "text-center", width: "4%" },
+                        { targets: 2, className: "text-left", width: "20%" },
+                        {
+                            targets: 3,
+                            className: "text-center",
+                            width: "30%",
+                            render: function (data, type, full)
                             {
-                                targets: 0,
-                                className: 'text-center',
-                                width: '4%',
-                            },
-                            {
-                                targets: 1,
-                                className: 'text-center',
-                                width: '4%',
-                            },
-                            {
-                                targets: 2,
-                                className: 'text-left',
-                                width: '20%',
-                            },
-                            {
-                                targets: 3,
-                                className: 'text-center',
-                                width: '30%',
-                                render: function (data, type, full) {
-                                    try {
-                                        return `<div class="text-center">
+                                try
+                                {
+                                    return `<div class="text-center">
                                             <a aria-label="&#9881; (${removeHTML(full.descricao)})" data-microtip-position="top" role="tooltip" data-microtip-size="medium" style="cursor:pointer;"
                                             data-id='${data}'>${full.resumo}</a></div>`;
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'manutencao.js',
-                                            'render',
-                                            error,
-                                        );
-                                    }
-                                },
+                                }
+                                catch (error)
+                                {
+                                    Alerta.TratamentoErroComLinha(
+                                        "manutencao.js",
+                                        "render",
+                                        error,
+                                    );
+                                }
                             },
+                        },
+                        { targets: 4, className: "text-center", width: "5%" },
+                        { targets: 5, className: "text-center", width: "5%", visible: false },
+                        { targets: 6, className: "text-center", width: "5%", visible: false },
+                        { targets: 7, className: "text-center", width: "10%", visible: false },
+                        { targets: 8, className: "text-center", width: "10%", visible: false },
+                        { targets: 9, className: "text-center", width: "10%", visible: false },
+
+                    ],
+
+                    responsive: true,
+                    ajax: {
+                        url: "/api/manutencao/OcorrenciasVeiculosPendencias",
+                        data: { id: VeiculoId },
+                        type: "GET",
+                        datatype: "json",
+                    },
+                    columns: [
+                        { data: "numFicha" },
+                        { data: "dataItem" },
+                        { data: "nome" },
+                        { data: "resumo" },
+                        {
+                            data: "itemManutencaoId",
+                            render: function (data, type, full, meta)
                             {
-                                targets: 4,
-                                className: 'text-center',
-                                width: '5%',
-                            },
-                            {
-                                targets: 5,
-                                className: 'text-center',
-                                width: '5%',
-                                visible: false,
-                            },
-                            {
-                                targets: 6,
-                                className: 'text-center',
-                                width: '5%',
-                                visible: false,
-                            },
-                            {
-                                targets: 7,
-                                className: 'text-center',
-                                width: '10%',
-                                visible: false,
-                            },
-                            {
-                                targets: 8,
-                                className: 'text-center',
-                                width: '10%',
-                                visible: false,
-                            },
-                            {
-                                targets: 9,
-                                className: 'text-center',
-                                width: '10%',
-                                visible: false,
-                            },
-
-                        ],
-
-                        responsive: true,
-                        ajax: {
-                            url: '/api/manutencao/OcorrenciasVeiculosPendencias',
-                            data: { id: VeiculoId },
-                            type: 'GET',
-                            datatype: 'json',
-                        },
-                        columns: [
-                            { data: 'numFicha' },
-                            { data: 'dataItem' },
-                            { data: 'nome' },
-                            { data: 'resumo' },
-                            {
-                                data: 'itemManutencaoId',
-                                render: function (data, type, full, meta) {
-                                    try {
-                                        const foto =
-                                            full.imagemOcorrencia || '';
-                                        const fotoAttr =
-                                            encodeURIComponent(foto);
-                                        const rowIndex = meta.row;
-
-                                        return `
+                                try
+                                {
+                                    const foto = full.imagemOcorrencia || "";
+                                    const fotoAttr = encodeURIComponent(foto);
+                                    const rowIndex = meta.row;
+
+                                    return `
                                             <div class="col-acao">
                                                 <div class="d-flex gap-2 justify-content-center">
                                                     <button type="button"
@@ -2821,12 +2677,7 @@
                                                     </button>
 
                                                     ${(() => {
-                                                        const temFoto =
-                                                            foto &&
-                                                            foto !== '' &&
-                                                            foto !== 'null' &&
-                                                            foto.toLowerCase() !==
-                                                                'semimagem.jpg';
+                                                        const temFoto = foto && foto !== '' && foto !== 'null' && foto.toLowerCase() !== 'semimagem.jpg';
                                                         if (temFoto) {
                                                             return `<button type="button"
                                                             class="btn btn-sm js-ver-foto-pendencia"
@@ -2874,41 +2725,43 @@
                                                 </div>
                                             </div>
                                         `;
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'manutencao.js',
-                                            'render',
-                                            error,
-                                        );
-                                    }
-                                },
+                                }
+                                catch (error)
+                                {
+                                    Alerta.TratamentoErroComLinha(
+                                        "manutencao.js",
+                                        "render",
+                                        error,
+                                    );
+                                }
                             },
-                            { data: 'descricao' },
-                            { data: 'motoristaId' },
-                            { data: 'imagemOcorrencia' },
-                            { data: 'itemManutencaoId' },
-                            { data: 'viagemId' },
-
-                        ],
-
-                        language: languageDataTable,
-                        width: '100%',
-                    });
+                        }, { data: "descricao" },
+                        { data: "motoristaId" },
+                        { data: "imagemOcorrencia" },
+                        { data: "itemManutencaoId" },
+                        { data: "viagemId" },
+
+                    ],
+
+                    language: languageDataTable,
+                    width: "100%",
+                });
                 }
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'manutencao.js',
-                    'VeiculoChange.DataTablePendencias',
-                    error,
-                );
-            }
-        }
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'VeiculoChange', error);
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("manutencao.js", "VeiculoChange.DataTablePendencias", error);
+            }
+        }
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "VeiculoChange", error);
     }
 }
 
-$(document).on('click', '.js-selecionar-ocorrencia', function () {
+$(document).on('click', '.js-selecionar-ocorrencia', function ()
+{
     const d = this.dataset;
     SelecionaLinha(
         d.viagemId,
@@ -2920,473 +2773,475 @@
         d.motoristaId,
         d.imagem,
         d.itemId,
-        this,
+        this
     );
 });
 
-function InsereRegistro() {
-    try {
-        if (document.getElementById('txtOS').value === '') {
+function InsereRegistro()
+{
+    try
+    {
+        if (document.getElementById("txtOS").value === "")
+        {
             Alerta.Erro(
-                'Informação Ausente',
-                'O número da OS é obrigatório',
-                'Ok',
+                "Informação Ausente",
+                "O número da OS é obrigatório",
+                "Ok"
             );
             return;
         }
 
-        if (document.getElementById('txtDataSolicitacao').value === '') {
+        if (document.getElementById("txtDataSolicitacao").value === "")
+        {
             Alerta.Erro(
-                'Informação Ausente',
-                'A data de solicitação é obrigatória',
-                'Ok',
+                "Informação Ausente",
+                "A data de solicitação é obrigatória",
+                "Ok"
             );
             return;
         }
 
-        if (document.getElementById('lstStatus').value === '') {
+        if (document.getElementById("lstStatus").value === "")
+        {
             Alerta.Erro(
-                'Informação Ausente',
-                'O Status deve ser informado!',
-                'OK',
+                "Informação Ausente",
+                "O Status deve ser informado!",
+                "OK"
             );
             return;
         }
 
         if (
-            document.getElementById('txtDataDisponibilidade').value != '' &&
-            document.getElementById('txtDataDisponibilidade').value != null
-        ) {
+            document.getElementById("txtDataDisponibilidade").value != "" &&
+            document.getElementById("txtDataDisponibilidade").value != null
+        )
+        {
             if (
-                document.getElementById('txtDataSolicitacao').value >
-                document.getElementById('txtDataDisponibilidade').value
-            ) {
+                document.getElementById("txtDataSolicitacao").value >
+                document.getElementById("txtDataDisponibilidade").value
+            )
+            {
                 Alerta.Erro(
-                    'Informação Errada',
-                    'A Data de Disponibilização não pode ser superior à Data de Solicitação',
+                    "Informação Errada",
+                    "A Data de Disponibilização não pode ser superior à Data de Solicitação",
                 );
                 return;
             }
         }
 
         if (
-            document.getElementById('txtDataEntrega').value != '' &&
-            document.getElementById('txtDataEntrega').value != null
-        ) {
+            document.getElementById("txtDataEntrega").value != "" &&
+            document.getElementById("txtDataEntrega").value != null
+        )
+        {
             if (
-                document.getElementById('txtDataSolicitacao').value >
-                document.getElementById('txtDataEntrega').value
-            ) {
+                document.getElementById("txtDataSolicitacao").value >
+                document.getElementById("txtDataEntrega").value
+            )
+            {
                 Alerta.Erro(
-                    'Informação Errada',
-                    'A Data de Entrega\\Recolhimento não pode ser superior à Data de Solicitação',
+                    "Informação Errada",
+                    "A Data de Entrega\\Recolhimento não pode ser superior à Data de Solicitação",
                 );
                 return;
             }
         }
 
         if (
-            document.getElementById('txtDataDevolucao').value != '' &&
-            document.getElementById('txtDataDevolucao').value != null
-        ) {
+            document.getElementById("txtDataDevolucao").value != "" &&
+            document.getElementById("txtDataDevolucao").value != null
+        )
+        {
             if (
-                document.getElementById('txtDataEntrega').value >
-                document.getElementById('txtDataDevolucao').value
-            ) {
+                document.getElementById("txtDataEntrega").value >
+                document.getElementById("txtDataDevolucao").value
+            )
+            {
                 Alerta.Erro(
-                    'Informação Errada',
-                    'A Data de Devolução não pode ser superior à Data de Entrega\\Recolhimento',
+                    "Informação Errada",
+                    "A Data de Devolução não pode ser superior à Data de Entrega\\Recolhimento",
                 );
                 return;
             }
         }
 
         if (
-            document.getElementById('txtDataDisponibilidade').value != '' &&
-            document.getElementById('txtDataDisponibilidade').value != null
-        ) {
+            document.getElementById("txtDataDisponibilidade").value != "" &&
+            document.getElementById("txtDataDisponibilidade").value != null
+        )
+        {
             if (
-                document.getElementById('txtDataSolicitacao').value >
-                document.getElementById('txtDataDisponibilidade').value
-            ) {
+                document.getElementById("txtDataSolicitacao").value >
+                document.getElementById("txtDataDisponibilidade").value
+            )
+            {
                 Alerta.Erro(
-                    'Informação Errada',
-                    'A Data de Disponibilização não pode ser inferior à Data de Solicitação',
+                    "Informação Errada",
+                    "A Data de Disponibilização não pode ser inferior à Data de Solicitação",
                 );
                 return;
             }
         }
 
         if (
-            document.getElementById('txtDataRecebimentoReserva').value != '' &&
-            document.getElementById('txtDataRecebimentoReserva').value != null
-        ) {
+            document.getElementById("txtDataRecebimentoReserva").value != "" &&
+            document.getElementById("txtDataRecebimentoReserva").value != null
+        )
+        {
             if (
-                document.getElementById('txtDataSolicitacao').value >
-                document.getElementById('txtDataRecebimentoReserva').value
-            ) {
+                document.getElementById("txtDataSolicitacao").value >
+                document.getElementById("txtDataRecebimentoReserva").value
+            )
+            {
                 Alerta.Erro(
-                    'Informação Errada',
-                    'A Data de Chegada do Reserva não pode ser inferior à Data de Solicitação',
+                    "Informação Errada",
+                    "A Data de Chegada do Reserva não pode ser inferior à Data de Solicitação",
                 );
                 return;
             }
         }
 
         if (
-            document.getElementById('txtDataDevolucaoReserva').value != '' &&
-            document.getElementById('txtDataDevolucaoReserva').value != null
-        ) {
+            document.getElementById("txtDataDevolucaoReserva").value != "" &&
+            document.getElementById("txtDataDevolucaoReserva").value != null
+        )
+        {
             if (
-                document.getElementById('txtDataDevolucaoReserva').value <
-                document.getElementById('txtDataRecebimentoReserva').value
-            ) {
+                document.getElementById("txtDataDevolucaoReserva").value <
+                document.getElementById("txtDataRecebimentoReserva").value
+            )
+            {
                 Alerta.Erro(
-                    'Informação Errada',
-                    'A Data de Saída do Reserva não pode ser superior à Data de Chegada dele',
+                    "Informação Errada",
+                    "A Data de Saída do Reserva não pode ser superior à Data de Chegada dele",
                 );
                 return;
             }
         }
 
-        var dtItens = $('#tblItens').DataTable();
-        if (dtItens.rows().count() === 0) {
+        var dtItens = $("#tblItens").DataTable();
+        if (dtItens.rows().count() === 0)
+        {
             Alerta.Erro(
-                'Informação Ausente',
-                'É preciso informar ao menos um item para manutenção',
+                "Informação Ausente",
+                "É preciso informar ao menos um item para manutenção",
             );
 
             return;
         }
 
-        $('#btnEdita').prop('disabled', true);
-        $('#btnAdiciona').prop('disabled', true);
-
-        var veiculo = document.getElementById('lstVeiculo').ej2_instances[0];
-        var veiculoreserva =
-            document.getElementById('lstVeiculoReserva').ej2_instances[0];
-
-        if (
-            manutencaoId === null ||
-            manutencaoId === '00000000-0000-0000-0000-000000000000'
-        ) {
-            ManutencaoId = '00000000-0000-0000-0000-000000000000';
+        $("#btnEdita").prop("disabled", true);
+        $("#btnAdiciona").prop("disabled", true);
+
+        var veiculo = document.getElementById("lstVeiculo").ej2_instances[0];
+        var veiculoreserva = document.getElementById("lstVeiculoReserva").ej2_instances[0];
+
+        if (manutencaoId === null || manutencaoId === "00000000-0000-0000-0000-000000000000")
+        {
+            ManutencaoId = "00000000-0000-0000-0000-000000000000";
         }
 
         var objManutencao = JSON.stringify({
             ManutencaoId: ManutencaoId,
-            NumOS: $('#txtOS').val(),
-            DataSolicitacao: $('#txtDataSolicitacao').val(),
-            DataDisponibilidade: $('#txtDataDisponibilidade').val(),
-            DataEntrega: $('#txtDataEntrega').val(),
+            NumOS: $("#txtOS").val(),
+            DataSolicitacao: $("#txtDataSolicitacao").val(),
+            DataDisponibilidade: $("#txtDataDisponibilidade").val(),
+            DataEntrega: $("#txtDataEntrega").val(),
             VeiculoId: veiculo.value,
-            ResumoOS: $('#txtResumoOS').val(),
-            DataDevolucao: $('#txtDataDevolucao').val(),
-            StatusOS: $('#lstStatus').val(),
-            ReservaEnviado: parseInt($('#lstReserva').val()),
+            ResumoOS: $("#txtResumoOS").val(),
+            DataDevolucao: $("#txtDataDevolucao").val(),
+            StatusOS: $("#lstStatus").val(),
+            ReservaEnviado: parseInt($("#lstReserva").val()),
             VeiculoReservaId: veiculoreserva.value,
-            DataRecebimentoReserva: $('#txtDataRecebimentoReserva').val(),
-            DataDevolucaoReserva: $('#txtDataDevolucaoReserva').val(),
+            DataRecebimentoReserva: $("#txtDataRecebimentoReserva").val(),
+            DataDevolucaoReserva: $("#txtDataDevolucaoReserva").val(),
 
         });
 
-        var StatusManutencao = $('#lstStatus').val();
-        var NumOS = $('#txtOS').val();
-        var DataOS = $('#txtDataSolicitacao').val();
-
-        if (
-            manutencaoId === null ||
-            manutencaoId === '00000000-0000-0000-0000-000000000000'
-        ) {
+        var StatusManutencao = $("#lstStatus").val();
+        var NumOS = $("#txtOS").val();
+        var DataOS = $("#txtDataSolicitacao").val();
+
+        if (manutencaoId === null || manutencaoId === "00000000-0000-0000-0000-000000000000")
+        {
             $.ajax({
-                type: 'post',
-                url: 'api/Manutencao/InsereOS',
-                contentType: 'application/json; charset=utf-8',
-                dataType: 'json',
+                type: "post",
+                url: "api/Manutencao/InsereOS",
+                contentType: "application/json; charset=utf-8",
+                dataType: "json",
                 data: objManutencao,
-                success: function (data) {
-                    try {
+                success: function (data)
+                {
+                    try
+                    {
                         var ManutencaoId = data.data;
                         var Linhas = dtItens.rows().count();
 
-                        for (var i = 0; i < Linhas; i++) {
-                            try {
-                                var dataOS = $('#tblItens')
-                                    .DataTable()
-                                    .row(i)
-                                    .data();
+                        for (var i = 0; i < Linhas; i++)
+                        {
+                            try
+                            {
+                                var dataOS = $("#tblItens").DataTable().row(i).data();
                                 var StatusItem =
-                                    StatusManutencao === 'Fechada'
-                                        ? 'Baixada'
-                                        : 'Manutenção';
+                                    StatusManutencao === "Fechada" ? "Baixada" : "Manutenção";
 
                                 var objItemOS = JSON.stringify({
                                     ManutencaoId: ManutencaoId,
-                                    TipoItem: dataOS['tipoItem'],
-                                    NumFicha: dataOS['numFicha'],
+                                    TipoItem: dataOS["tipoItem"],
+                                    NumFicha: dataOS["numFicha"],
                                     DataItem:
-                                        dataOS['dataItem'].substring(6, 10) +
-                                        '-' +
-                                        dataOS['dataItem'].substring(3, 5) +
-                                        '-' +
-                                        dataOS['dataItem'].substring(0, 2),
-                                    Resumo: dataOS['resumo'],
-                                    Descricao: dataOS['descricao'],
+                                        dataOS["dataItem"].substring(6, 10) +
+                                        "-" +
+                                        dataOS["dataItem"].substring(3, 5) +
+                                        "-" +
+                                        dataOS["dataItem"].substring(0, 2),
+                                    Resumo: dataOS["resumo"],
+                                    Descricao: dataOS["descricao"],
                                     Status: StatusItem,
-                                    MotoristaId: dataOS['motoristaId'],
-                                    ViagemId: dataOS['viagemId'],
-                                    ImagemOcorrencia:
-                                        dataOS['imagemOcorrencia'],
+                                    MotoristaId: dataOS["motoristaId"],
+                                    ViagemId: dataOS["viagemId"],
+                                    ImagemOcorrencia: dataOS["imagemOcorrencia"],
                                     NumOS: NumOS,
                                     DataOS: DataOS,
                                 });
 
                                 $.ajax({
-                                    type: 'post',
-                                    url: 'api/Manutencao/InsereItemOS',
-                                    contentType:
-                                        'application/json; charset=utf-8',
-                                    dataType: 'json',
+                                    type: "post",
+                                    url: "api/Manutencao/InsereItemOS",
+                                    contentType: "application/json; charset=utf-8",
+                                    dataType: "json",
                                     data: objItemOS,
-                                    success: function (data) {
-                                        try {
-                                        } catch (error) {
+                                    success: function (data)
+                                    {
+                                        try
+                                        {
+                                        }
+                                        catch (error)
+                                        {
                                             Alerta.TratamentoErroComLinha(
-                                                'manutencao.js',
-                                                'success',
+                                                "manutencao.js",
+                                                "success",
                                                 error,
                                             );
                                         }
                                     },
-                                    error: function (data) {
-                                        try {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                                3000,
-                                            );
+                                    error: function (data)
+                                    {
+                                        try
+                                        {
+                                            AppToast.show("Vermelho", data.message, 3000);
                                             console.log(data);
-                                        } catch (error) {
+                                        }
+                                        catch (error)
+                                        {
                                             TratamentoErroComLinha(
-                                                'manutencao.js',
-                                                'ajax.InsereItemOS.error',
+                                                "manutencao.js",
+                                                "ajax.InsereItemOS.error",
                                                 error,
                                             );
                                         }
                                     },
                                 });
-                            } catch (error) {
-                                TratamentoErroComLinha(
-                                    'manutencao.js',
-                                    'loop.InsereItemOS',
-                                    error,
-                                );
+                            }
+                            catch (error)
+                            {
+                                TratamentoErroComLinha("manutencao.js", "loop.InsereItemOS", error);
                             }
                         }
-                        AppToast.show(
-                            'Verde',
-                            'OS de Manutenção Adicionada com Sucesso!',
-                            1000,
-                        );
-                        setTimeout(() => {
-                            location.replace('/manutencao/listamanutencao');
+                        AppToast.show("Verde", "OS de Manutenção Adicionada com Sucesso!", 1000);
+                        setTimeout(() =>
+                        {
+                            location.replace("/manutencao/listamanutencao");
                         }, 1000);
-                    } catch (error) {
-                        TratamentoErroComLinha(
-                            'manutencao.js',
-                            'ajax.InsereOS.success',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        TratamentoErroComLinha("manutencao.js", "ajax.InsereOS.success", error);
                     }
                 },
-                error: function (data) {
-                    try {
-                        AppToast.show('Vermelho', data.message, 3000);
+                error: function (data)
+                {
+                    try
+                    {
+                        AppToast.show("Vermelho", data.message, 3000);
                         console.log(data);
-                    } catch (error) {
-                        TratamentoErroComLinha(
-                            'manutencao.js',
-                            'ajax.InsereOS.error',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        TratamentoErroComLinha("manutencao.js", "ajax.InsereOS.error", error);
                     }
                 },
             });
-        } else {
+        } else
+        {
             $.ajax({
-                type: 'post',
-                url: 'api/Manutencao/InsereOS',
-                contentType: 'application/json; charset=utf-8',
-                dataType: 'json',
+                type: "post",
+                url: "api/Manutencao/InsereOS",
+                contentType: "application/json; charset=utf-8",
+                dataType: "json",
                 data: objManutencao,
-                success: function (data) {
-                    try {
-                        var LinhasOcorrencias = dataTableOcorrencias
-                            .rows()
-                            .count();
-                        for (var i = 0; i < LinhasOcorrencias; i++) {
-                            try {
-                                var dataOcorrencia = $('#tblOcorrencia')
-                                    .DataTable()
-                                    .row(i)
-                                    .data();
+                success: function (data)
+                {
+                    try
+                    {
+                        var LinhasOcorrencias = dataTableOcorrencias.rows().count();
+                        for (var i = 0; i < LinhasOcorrencias; i++)
+                        {
+                            try
+                            {
+                                var dataOcorrencia = $("#tblOcorrencia").DataTable().row(i).data();
                                 if (
-                                    dataOcorrencia['itemManutencaoId'] !=
-                                        null &&
-                                    dataOcorrencia['itemManutencaoId'] != ''
-                                ) {
+                                    dataOcorrencia["itemManutencaoId"] != null &&
+                                    dataOcorrencia["itemManutencaoId"] != ""
+                                )
+                                {
                                     var objItemOcorrencia = JSON.stringify({
-                                        ItemManutencaoId:
-                                            dataOcorrencia['itemManutencaoId'],
+                                        ItemManutencaoId: dataOcorrencia["itemManutencaoId"],
                                     });
 
                                     $.ajax({
-                                        type: 'post',
-                                        url: 'api/Manutencao/ApagaConexaoOcorrencia',
-                                        contentType:
-                                            'application/json; charset=utf-8',
-                                        dataType: 'json',
+                                        type: "post",
+                                        url: "api/Manutencao/ApagaConexaoOcorrencia",
+                                        contentType: "application/json; charset=utf-8",
+                                        dataType: "json",
                                         data: objItemOcorrencia,
-                                        success: function (data) {
-                                            try {
-                                            } catch (error) {
+                                        success: function (data)
+                                        {
+                                            try
+                                            {
+                                            }
+                                            catch (error)
+                                            {
                                                 Alerta.TratamentoErroComLinha(
-                                                    'manutencao.js',
-                                                    'success',
+                                                    "manutencao.js",
+                                                    "success",
                                                     error,
                                                 );
                                             }
                                         },
-                                        error: function (data) {
-                                            try {
-                                                AppToast.show(
-                                                    'Vermelho',
-                                                    data.message,
-                                                    3000,
-                                                );
+                                        error: function (data)
+                                        {
+                                            try
+                                            {
+                                                AppToast.show("Vermelho", data.message, 3000);
                                                 console.log(data);
-                                            } catch (error) {
+                                            }
+                                            catch (error)
+                                            {
                                                 TratamentoErroComLinha(
-                                                    'manutencao.js',
-                                                    'ajax.ApagaConexaoOcorrencia.error',
+                                                    "manutencao.js",
+                                                    "ajax.ApagaConexaoOcorrencia.error",
                                                     error,
                                                 );
                                             }
                                         },
                                     });
                                 }
-                            } catch (error) {
+                            }
+                            catch (error)
+                            {
                                 TratamentoErroComLinha(
-                                    'manutencao.js',
-                                    'loop.ApagaConexaoOcorrencia',
+                                    "manutencao.js",
+                                    "loop.ApagaConexaoOcorrencia",
                                     error,
                                 );
                             }
                         }
 
-                        var LinhasPendencias = dataTablePendencias
-                            .rows()
-                            .count();
-                        for (var i = 0; i < LinhasPendencias; i++) {
-                            try {
-                                var dataPendencia = $('#tblPendencia')
-                                    .DataTable()
-                                    .row(i)
-                                    .data();
+                        var LinhasPendencias = dataTablePendencias.rows().count();
+                        for (var i = 0; i < LinhasPendencias; i++)
+                        {
+                            try
+                            {
+                                var dataPendencia = $("#tblPendencia").DataTable().row(i).data();
                                 if (
-                                    dataPendencia['itemManutencaoId'] != null &&
-                                    dataPendencia['itemManutencaoId'] != ''
-                                ) {
+                                    dataPendencia["itemManutencaoId"] != null &&
+                                    dataPendencia["itemManutencaoId"] != ""
+                                )
+                                {
                                     var objItemPendencia = JSON.stringify({
-                                        ItemManutencaoId:
-                                            dataPendencia['itemManutencaoId'],
+                                        ItemManutencaoId: dataPendencia["itemManutencaoId"],
                                     });
 
                                     $.ajax({
-                                        type: 'post',
-                                        url: 'api/Manutencao/ApagaConexaoPendencia',
-                                        contentType:
-                                            'application/json; charset=utf-8',
-                                        dataType: 'json',
+                                        type: "post",
+                                        url: "api/Manutencao/ApagaConexaoPendencia",
+                                        contentType: "application/json; charset=utf-8",
+                                        dataType: "json",
                                         data: objItemPendencia,
-                                        success: function (data) {
-                                            try {
-                                            } catch (error) {
+                                        success: function (data)
+                                        {
+                                            try
+                                            {
+                                            }
+                                            catch (error)
+                                            {
                                                 Alerta.TratamentoErroComLinha(
-                                                    'manutencao.js',
-                                                    'success',
+                                                    "manutencao.js",
+                                                    "success",
                                                     error,
                                                 );
                                             }
                                         },
-                                        error: function (data) {
-                                            try {
-                                                AppToast.show(
-                                                    'Vermelho',
-                                                    data.message,
-                                                    3000,
-                                                );
+                                        error: function (data)
+                                        {
+                                            try
+                                            {
+                                                AppToast.show("Vermelho", data.message, 3000);
                                                 console.log(data);
-                                            } catch (error) {
+                                            }
+                                            catch (error)
+                                            {
                                                 TratamentoErroComLinha(
-                                                    'manutencao.js',
-                                                    'ajax.ApagaConexaoPendencia.error',
+                                                    "manutencao.js",
+                                                    "ajax.ApagaConexaoPendencia.error",
                                                     error,
                                                 );
                                             }
                                         },
                                     });
                                 }
-                            } catch (error) {
+                            }
+                            catch (error)
+                            {
                                 TratamentoErroComLinha(
-                                    'manutencao.js',
-                                    'loop.ApagaConexaoPendencia',
+                                    "manutencao.js",
+                                    "loop.ApagaConexaoPendencia",
                                     error,
                                 );
                             }
                         }
 
-                        var objManutencaoIdOnly = JSON.stringify({
-                            ManutencaoId: ManutencaoId,
-                        });
-                        var StatusManutencaoEdit = $('#lstStatus').val();
+                        var objManutencaoIdOnly = JSON.stringify({ ManutencaoId: ManutencaoId });
+                        var StatusManutencaoEdit = $("#lstStatus").val();
 
                         $.ajax({
-                            type: 'post',
-                            url: 'api/Manutencao/ApagaItens',
-                            contentType: 'application/json; charset=utf-8',
-                            dataType: 'json',
+                            type: "post",
+                            url: "api/Manutencao/ApagaItens",
+                            contentType: "application/json; charset=utf-8",
+                            dataType: "json",
                             data: objManutencaoIdOnly,
-                            success: function (data) {
-                                try {
+                            success: function (data)
+                            {
+                                try
+                                {
 
                                     var Linhas = dtItens.rows().count();
                                     var promessas = [];
 
-                                    for (var i = 0; i < Linhas; i++) {
-                                        try {
-                                            const EMPTY_GUID =
-                                                '00000000-0000-0000-0000-000000000000';
-                                            const nonEmpty = (v) =>
-                                                v !== null &&
-                                                v !== undefined &&
-                                                String(v).trim() !== '';
-
-                                            const dataOS = $('#tblItens')
-                                                .DataTable()
-                                                .row(i)
-                                                .data();
-
-                                            const [dd, mm, yyyy] = String(
-                                                dataOS.dataItem || '',
-                                            ).split('/');
+                                    for (var i = 0; i < Linhas; i++)
+                                    {
+                                        try
+                                        {
+                                            const EMPTY_GUID = '00000000-0000-0000-0000-000000000000';
+                                            const nonEmpty = v => v !== null && v !== undefined && String(v).trim() !== '';
+
+                                            const dataOS = $("#tblItens").DataTable().row(i).data();
+
+                                            const [dd, mm, yyyy] = String(dataOS.dataItem || '').split('/');
                                             const dataISO = `${yyyy}-${mm}-${dd}`;
 
-                                            var StatusItem =
-                                                StatusManutencaoEdit ===
-                                                'Fechada'
-                                                    ? 'Baixada'
-                                                    : 'Manutenção';
+                                            var StatusItem = StatusManutencaoEdit === "Fechada" ? "Baixada" : "Manutenção";
 
                                             const payload = {
                                                 ManutencaoId: ManutencaoId,
@@ -3396,236 +3251,199 @@
                                                 Resumo: dataOS.resumo,
                                                 Descricao: dataOS.descricao,
                                                 Status: StatusItem,
-                                                ImagemOcorrencia:
-                                                    dataOS.imagemOcorrencia,
-                                                NumOS: $('#txtOS').val(),
-                                                DataOS: $(
-                                                    '#txtDataSolicitacao',
-                                                ).val(),
-
-                                                ...(nonEmpty(
-                                                    dataOS.motoristaId,
-                                                ) &&
-                                                dataOS.motoristaId !==
-                                                    EMPTY_GUID
-                                                    ? {
-                                                          MotoristaId:
-                                                              dataOS.motoristaId,
-                                                      }
-                                                    : {}),
-                                                ...(nonEmpty(dataOS.viagemId) &&
-                                                dataOS.viagemId !== EMPTY_GUID
-                                                    ? {
-                                                          ViagemId:
-                                                              dataOS.viagemId,
-                                                      }
-                                                    : {}),
+                                                ImagemOcorrencia: dataOS.imagemOcorrencia,
+                                                NumOS: $("#txtOS").val(),
+                                                DataOS: $("#txtDataSolicitacao").val(),
+
+                                                ...(nonEmpty(dataOS.motoristaId) && dataOS.motoristaId !== EMPTY_GUID
+                                                    ? { MotoristaId: dataOS.motoristaId } : {}),
+                                                ...(nonEmpty(dataOS.viagemId) && dataOS.viagemId !== EMPTY_GUID
+                                                    ? { ViagemId: dataOS.viagemId } : {}),
                                             };
 
-                                            const objItemOS =
-                                                JSON.stringify(payload);
+                                            const objItemOS = JSON.stringify(payload);
 
                                             var promessa = $.ajax({
-                                                type: 'post',
-                                                url: 'api/Manutencao/InsereItemOS',
-                                                contentType:
-                                                    'application/json; charset=utf-8',
-                                                dataType: 'json',
-                                                data: objItemOS,
+                                                type: "post",
+                                                url: "api/Manutencao/InsereItemOS",
+                                                contentType: "application/json; charset=utf-8",
+                                                dataType: "json",
+                                                data: objItemOS
                                             });
                                             promessas.push(promessa);
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha(
-                                                'manutencao.js',
-                                                'loop.InsereItemOS',
-                                                error,
-                                            );
+                                        }
+                                        catch (error)
+                                        {
+                                            Alerta.TratamentoErroComLinha("manutencao.js", "loop.InsereItemOS", error);
                                         }
                                     }
 
-                                    $.when
-                                        .apply($, promessas)
-                                        .done(function () {
-                                            AppToast.show(
-                                                'Verde',
-                                                'OS de Manutenção Registrada com Sucesso!',
-                                                1000,
-                                            );
-                                            setTimeout(() => {
-                                                location.replace(
-                                                    '/manutencao/listamanutencao',
-                                                );
-                                            }, 1000);
-                                        })
-                                        .fail(function () {
-                                            AppToast.show(
-                                                'Amarelo',
-                                                'OS atualizada, mas houve erros em alguns itens.',
-                                                3000,
-                                            );
-                                        });
-                                } catch (error) {
+                                    $.when.apply($, promessas).done(function() {
+                                        AppToast.show("Verde", "OS de Manutenção Registrada com Sucesso!", 1000);
+                                        setTimeout(() => {
+                                            location.replace("/manutencao/listamanutencao");
+                                        }, 1000);
+                                    }).fail(function() {
+                                        AppToast.show("Amarelo", "OS atualizada, mas houve erros em alguns itens.", 3000);
+                                    });
+                                }
+                                catch (error)
+                                {
                                     Alerta.TratamentoErroComLinha(
-                                        'manutencao.js',
-                                        'ApagaItens.success',
+                                        "manutencao.js",
+                                        "ApagaItens.success",
                                         error,
                                     );
                                 }
                             },
-                            error: function (data) {
-                                try {
-                                    AppToast.show(
-                                        'Vermelho',
-                                        'Erro ao remover itens antigos: ' +
-                                            data.message,
-                                        3000,
-                                    );
+                            error: function (data)
+                            {
+                                try
+                                {
+                                    AppToast.show("Vermelho", "Erro ao remover itens antigos: " + data.message, 3000);
                                     console.log(data);
-                                } catch (error) {
+                                }
+                                catch (error)
+                                {
                                     Alerta.TratamentoErroComLinha(
-                                        'manutencao.js',
-                                        'ajax.ApagaItens.error',
+                                        "manutencao.js",
+                                        "ajax.ApagaItens.error",
                                         error,
                                     );
                                 }
                             },
                         });
-                    } catch (error) {
-                        TratamentoErroComLinha(
-                            'manutencao.js',
-                            'ajax.InsereOS.success',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        TratamentoErroComLinha("manutencao.js", "ajax.InsereOS.success", error);
                     }
                 },
-                error: function (data) {
-                    try {
-                        AppToast.show('Vermelho', data.message, 3000);
+                error: function (data)
+                {
+                    try
+                    {
+                        AppToast.show("Vermelho", data.message, 3000);
                         console.log(data);
-                    } catch (error) {
-                        TratamentoErroComLinha(
-                            'manutencao.js',
-                            'ajax.InsereOS.error',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        TratamentoErroComLinha("manutencao.js", "ajax.InsereOS.error", error);
                     }
                 },
             });
         }
-    } catch (error) {
-        TratamentoErroComLinha('manutencao.js', 'InsereRegistro', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("manutencao.js", "InsereRegistro", error);
     }
 }
 
-document
-    .getElementById('txtFileItemNovo')
-    .addEventListener('change', async function (e) {
-        try {
-            const file = e.target.files?.[0];
-            if (!file) return;
-
-            const imgEl = document.getElementById('imgViewerNovo');
-            imgEl.src = URL.createObjectURL(file);
-
-            const token =
-                document.querySelector(
-                    'meta[name="request-verification-token"]',
-                )?.content ||
-                document.querySelector(
-                    '#uploadForm input[name="__RequestVerificationToken"]',
-                )?.value;
-
-            console.log('Anti-forgery token:', token?.slice(0, 12) + '...');
-
-            if (!token) {
-                AppToast.show(
-                    'Vermelho',
-                    'Token antiforgery não encontrado na página',
-                    3000,
-                );
-                return;
-            }
-
-            const fd = new FormData();
-            fd.append('files', file, file.name);
-            fd.append('__RequestVerificationToken', token);
-
-            const resp = await fetch(
-                '/Uploads/UploadPDF?handler=SaveIMGManutencao',
-                {
-                    method: 'POST',
-                    body: fd,
-                    headers: { 'X-CSRF-TOKEN': token },
-                    credentials: 'same-origin',
-                },
-            );
-
-            if (!resp.ok) {
-                const txt = await resp.text();
-                throw new Error(
-                    'Falha no upload: ' + resp.status + ' - ' + txt,
-                );
-            }
-
-            const data = await resp.json();
-            window.ImagemSelecionada = data.fileName;
-
-            AppToast.show('Verde', 'Foto carregada com sucesso!', 2000);
-        } catch (error) {
-            console.error('Erro no upload:', error);
-            AppToast.show(
-                'Vermelho',
-                'Erro ao fazer upload da foto: ' + error.message,
-                5000,
-            );
-        }
-    });
-
-$('#btnAdicionarFoto')
-    .off('click')
-    .on('click', function (e) {
-        try {
-            e.preventDefault();
-
-            if (linhaSelecionadaFoto === -1) {
-                AppToast.show('Amarelo', 'Nenhuma linha selecionada', 2000);
-                return;
-            }
-
-            if (!window.ImagemSelecionada) {
-                AppToast.show('Amarelo', 'Selecione uma foto primeiro', 2000);
-                return;
-            }
-
-            const table = $('#tblItens').DataTable();
-            const rowData = table.row(linhaSelecionadaFoto).data();
-
-            if (rowData) {
-                rowData.imagemOcorrencia = window.ImagemSelecionada;
-                table.row(linhaSelecionadaFoto).data(rowData).draw(false);
-
-                AppToast.show('Verde', 'Foto atualizada com sucesso!', 2000);
-
-                forceCloseModal();
-            } else {
-                AppToast.show('Vermelho', 'Erro: linha não encontrada', 2000);
-            }
-        } catch (error) {
-            console.error('Erro ao adicionar foto:', error);
-            AppToast.show('Vermelho', 'Erro ao atualizar a foto', 2000);
-        }
-    });
-
-$('#btnFecharModal')
-    .off('click')
-    .on('click', function (e) {
+document.getElementById("txtFileItemNovo").addEventListener("change", async function (e)
+{
+    try
+    {
+        const file = e.target.files?.[0];
+        if (!file) return;
+
+        const imgEl = document.getElementById('imgViewerNovo');
+        imgEl.src = URL.createObjectURL(file);
+
+        const token =
+            document.querySelector('meta[name="request-verification-token"]')?.content ||
+            document.querySelector('#uploadForm input[name="__RequestVerificationToken"]')?.value;
+
+        console.log("Anti-forgery token:", token?.slice(0, 12) + "...");
+
+        if (!token)
+        {
+            AppToast.show("Vermelho", "Token antiforgery não encontrado na página", 3000);
+            return;
+        }
+
+        const fd = new FormData();
+        fd.append("files", file, file.name);
+        fd.append("__RequestVerificationToken", token);
+
+        const resp = await fetch("/Uploads/UploadPDF?handler=SaveIMGManutencao", {
+            method: "POST",
+            body: fd,
+            headers: { "X-CSRF-TOKEN": token },
+            credentials: "same-origin"
+        });
+
+        if (!resp.ok)
+        {
+            const txt = await resp.text();
+            throw new Error("Falha no upload: " + resp.status + " - " + txt);
+        }
+
+        const data = await resp.json();
+        window.ImagemSelecionada = data.fileName;
+
+        AppToast.show("Verde", "Foto carregada com sucesso!", 2000);
+
+    } catch (error)
+    {
+        console.error("Erro no upload:", error);
+        AppToast.show("Vermelho", "Erro ao fazer upload da foto: " + error.message, 5000);
+    }
+});
+
+$("#btnAdicionarFoto").off('click').on('click', function (e)
+{
+    try
+    {
         e.preventDefault();
-        forceCloseModal();
-    });
-
-function forceCloseModal() {
-    try {
-        const BASE = '/DadosEditaveis/ImagensOcorrencias/';
+
+        if (linhaSelecionadaFoto === -1)
+        {
+            AppToast.show("Amarelo", "Nenhuma linha selecionada", 2000);
+            return;
+        }
+
+        if (!window.ImagemSelecionada)
+        {
+            AppToast.show("Amarelo", "Selecione uma foto primeiro", 2000);
+            return;
+        }
+
+        const table = $('#tblItens').DataTable();
+        const rowData = table.row(linhaSelecionadaFoto).data();
+
+        if (rowData)
+        {
+            rowData.imagemOcorrencia = window.ImagemSelecionada;
+            table.row(linhaSelecionadaFoto).data(rowData).draw(false);
+
+            AppToast.show("Verde", "Foto atualizada com sucesso!", 2000);
+
+            forceCloseModal();
+
+        } else
+        {
+            AppToast.show("Vermelho", "Erro: linha não encontrada", 2000);
+        }
+
+    } catch (error)
+    {
+        console.error("Erro ao adicionar foto:", error);
+        AppToast.show("Vermelho", "Erro ao atualizar a foto", 2000);
+    }
+});
+
+$("#btnFecharModal").off('click').on('click', function (e)
+{
+    e.preventDefault();
+    forceCloseModal();
+});
+
+function forceCloseModal()
+{
+    try
+    {
+        const BASE = "/DadosEditaveis/ImagensOcorrencias/";
         const modalEl = document.getElementById('modalFotoManutencao');
 
         linhaSelecionadaFoto = -1;
@@ -3635,25 +3453,31 @@
         if (fileInput) fileInput.value = '';
 
         const imgEl = document.getElementById('imgViewerNovo');
-        if (imgEl) imgEl.src = BASE + 'semimagem.jpg';
-
-        if (modalEl) {
+        if (imgEl) imgEl.src = BASE + "semimagem.jpg";
+
+        if (modalEl)
+        {
             const modal = bootstrap.Modal.getInstance(modalEl);
-            if (modal) {
+            if (modal)
+            {
                 modal.hide();
-            } else {
+            } else
+            {
 
                 const newModal = new bootstrap.Modal(modalEl);
                 newModal.hide();
             }
         }
 
-        setTimeout(() => {
-            document.querySelectorAll('.modal-backdrop').forEach((backdrop) => {
+        setTimeout(() =>
+        {
+            document.querySelectorAll('.modal-backdrop').forEach(backdrop =>
+            {
                 backdrop.remove();
             });
 
-            if (modalEl) {
+            if (modalEl)
+            {
                 modalEl.classList.remove('show');
                 modalEl.style.display = 'none';
                 modalEl.setAttribute('aria-hidden', 'true');
@@ -3662,89 +3486,91 @@
             document.body.classList.remove('modal-open');
             document.body.style.overflow = '';
             document.body.style.paddingRight = '';
+
         }, 200);
-    } catch (error) {
-        console.error('Erro ao fechar modal:', error);
+
+    } catch (error)
+    {
+        console.error("Erro ao fechar modal:", error);
     }
 }
 
-(function initModalFotoPendencia() {
-    const BASE = '/DadosEditaveis/ImagensOcorrencias/';
+(function initModalFotoPendencia()
+{
+    const BASE = "/DadosEditaveis/ImagensOcorrencias/";
     const modalEl = document.getElementById('modalFotoPendencia');
     const imgEl = document.getElementById('imgViewerPendencia');
 
     if (!modalEl || !imgEl) return;
 
-    document.addEventListener('click', function (e) {
+    document.addEventListener('click', function (e)
+    {
         const btn = e.target.closest('[data-bs-target="#modalFotoPendencia"]');
         if (!btn) return;
 
-        const foto = btn.dataset.foto
-            ? decodeURIComponent(btn.dataset.foto)
-            : '';
-        const temFoto = foto && foto.toLowerCase() !== 'null' && foto !== '';
-
-        imgEl.src = temFoto
-            ? BASE + encodeURIComponent(foto)
-            : BASE + 'semimagem.jpg';
+        const foto = btn.dataset.foto ? decodeURIComponent(btn.dataset.foto) : "";
+        const temFoto = foto && foto.toLowerCase() !== "null" && foto !== "";
+
+        imgEl.src = temFoto ? (BASE + encodeURIComponent(foto)) : (BASE + "semimagem.jpg");
     });
 
-    modalEl.addEventListener('hide.bs.modal', () => {
+    modalEl.addEventListener('hide.bs.modal', () =>
+    {
         imgEl.removeAttribute('src');
     });
 })();
 
-$(document).on('click', '.js-selecionar-pendencia', function () {
+$(document).on('click', '.js-selecionar-pendencia', function ()
+{
     const d = this.dataset;
     SelecionaLinhaPendencia(
-        d.viagemId,
-        d.ficha,
-        d.data,
-        d.nome,
-        d.resumo,
-        d.descricao,
-        d.motoristaId,
-        d.imagem,
-        d.itemId,
-        this,
+        d.viagemId, d.ficha, d.data, d.nome,
+        d.resumo, d.descricao, d.motoristaId,
+        d.imagem, d.itemId,
+        this
     );
 });
 
-$(document).on('click', '.js-remover-item', function () {
+$(document).on('click', '.js-remover-item', function ()
+{
     const id = this.dataset.itemId;
     RemoveItem(id, this);
 });
 
 var removeItemDebounce = false;
 
-function RemoveItem(itemId, buttonElement) {
-
-    if (removeItemDebounce) {
-        console.log('RemoveItem bloqueada por debounce');
+function RemoveItem(itemId, buttonElement)
+{
+
+    if (removeItemDebounce)
+    {
+        console.log("RemoveItem bloqueada por debounce");
         return;
     }
 
     removeItemDebounce = true;
-    console.log('RemoveItem executando...');
-
-    try {
+    console.log("RemoveItem executando...");
+
+    try
+    {
         const table = $('#tblItens').DataTable();
         const $tr = $(buttonElement).closest('tr');
         const data = table.row($tr).data();
 
-        if (!data) {
-            AppToast.show('Amarelo', 'Item não encontrado na tabela', 3000);
+        if (!data)
+        {
+            AppToast.show("Amarelo", "Item não encontrado na tabela", 3000);
             return;
         }
 
-        if (data['tipoItem'] === 'Ocorrência') {
+        if (data["tipoItem"] === "Ocorrência")
+        {
             let Ocorrencias = [];
 
-            if (
-                dataTableOcorrencias &&
-                dataTableOcorrencias.data().count() > 0
-            ) {
-                for (var i = 0; i < dataTableOcorrencias.data().count(); i++) {
+            if (dataTableOcorrencias && dataTableOcorrencias.data().count() > 0)
+            {
+                for (var i = 0; i < dataTableOcorrencias.data().count(); i++)
+                {
                     let ocorrencia = new Ocorrencia(
                         dataTableOcorrencias.cell(i, 0).data(),
                         dataTableOcorrencias.cell(i, 1).data(),
@@ -3762,24 +3588,26 @@
                 }
             }
 
-            $('#tblOcorrencia')
+            $("#tblOcorrencia")
                 .DataTable()
                 .row.add({
-                    noFichaVistoria: data['numFicha'],
-                    dataInicial: data['dataItem'],
-                    nomeMotorista: data['nomeMotorista'],
-                    resumoOcorrencia: data['resumo'],
-                    viagemId: data['viagemId'],
-                    descricaoOcorrencia: data['descricao'],
-                    motoristaId: data['motoristaId'],
-                    imagemOcorrencia: data['imagemOcorrencia'],
-                    itemManutencaoId: data['itemManutencaoId'],
+                    noFichaVistoria: data["numFicha"],
+                    dataInicial: data["dataItem"],
+                    nomeMotorista: data["nomeMotorista"],
+                    resumoOcorrencia: data["resumo"],
+                    viagemId: data["viagemId"],
+                    descricaoOcorrencia: data["descricao"],
+                    motoristaId: data["motoristaId"],
+                    imagemOcorrencia: data["imagemOcorrencia"],
+                    itemManutencaoId: data["itemManutencaoId"],
                 })
                 .draw(false);
 
-            Ocorrencias.forEach((o) => {
-                try {
-                    $('#tblOcorrencia')
+            Ocorrencias.forEach((o) =>
+            {
+                try
+                {
+                    $("#tblOcorrencia")
                         .DataTable()
                         .row.add({
                             noFichaVistoria: o.noFichaVistoria,
@@ -3793,19 +3621,26 @@
                             itemManutencaoId: o.itemManutencaoId,
                         })
                         .draw(false);
-                } catch (error) {
+                }
+                catch (error)
+                {
                     Alerta.TratamentoErroComLinha(
-                        'manutencao.js',
-                        'callback@Ocorrencias.forEach#0',
+                        "manutencao.js",
+                        "callback@Ocorrencias.forEach#0",
                         error,
                     );
                 }
             });
-        } else if (data['tipoItem'] === 'Pendência') {
+
+        }
+        else if (data["tipoItem"] === "Pendência")
+        {
             let Pendencias = [];
 
-            if (dataTablePendencias && dataTablePendencias.data().count() > 0) {
-                for (var i = 0; i < dataTablePendencias.data().count(); i++) {
+            if (dataTablePendencias && dataTablePendencias.data().count() > 0)
+            {
+                for (var i = 0; i < dataTablePendencias.data().count(); i++)
+                {
                     let pendencia = new Ocorrencia(
                         dataTablePendencias.cell(i, 0).data(),
                         dataTablePendencias.cell(i, 1).data(),
@@ -3824,24 +3659,26 @@
                 }
             }
 
-            $('#tblPendencia')
+            $("#tblPendencia")
                 .DataTable()
                 .row.add({
-                    numFicha: data['numFicha'],
-                    dataItem: data['dataItem'],
-                    nome: data['nomeMotorista'],
-                    resumo: data['resumo'],
-                    itemManutencaoId: data['itemManutencaoId'],
-                    descricao: data['descricao'],
-                    motoristaId: data['motoristaId'],
-                    imagemOcorrencia: data['imagemOcorrencia'],
-                    viagemId: data['viagemId'],
+                    numFicha: data["numFicha"],
+                    dataItem: data["dataItem"],
+                    nome: data["nomeMotorista"],
+                    resumo: data["resumo"],
+                    itemManutencaoId: data["itemManutencaoId"],
+                    descricao: data["descricao"],
+                    motoristaId: data["motoristaId"],
+                    imagemOcorrencia: data["imagemOcorrencia"],
+                    viagemId: data["viagemId"],
                 })
                 .draw(false);
 
-            Pendencias.forEach((p) => {
-                try {
-                    $('#tblPendencia')
+            Pendencias.forEach((p) =>
+            {
+                try
+                {
+                    $("#tblPendencia")
                         .DataTable()
                         .row.add({
                             numFicha: p.noFichaVistoria,
@@ -3855,10 +3692,12 @@
                             viagemId: p.viagemId,
                         })
                         .draw(false);
-                } catch (error) {
+                }
+                catch (error)
+                {
                     Alerta.TratamentoErroComLinha(
-                        'manutencao.js',
-                        'callback@Pendencias.forEach#0',
+                        "manutencao.js",
+                        "callback@Pendencias.forEach#0",
                         error,
                     );
                 }
@@ -3866,32 +3705,39 @@
         }
 
         table.row($tr).remove().draw(false);
-        AppToast.show(
-            'Verde',
-            'Item removido e devolvido à lista original!',
-            2000,
-        );
-    } catch (error) {
-        console.error('Erro ao remover item:', error);
-        AppToast.show('Vermelho', 'Erro ao remover o item', 3000);
-        Alerta.TratamentoErroComLinha('manutencao.js', 'RemoveItem', error);
-    } finally {
-
-        setTimeout(() => {
+        AppToast.show("Verde", "Item removido e devolvido à lista original!", 2000);
+
+    }
+    catch (error)
+    {
+        console.error("Erro ao remover item:", error);
+        AppToast.show("Vermelho", "Erro ao remover o item", 3000);
+        Alerta.TratamentoErroComLinha("manutencao.js", "RemoveItem", error);
+    }
+    finally
+    {
+
+        setTimeout(() =>
+        {
             removeItemDebounce = false;
-            console.log('Debounce liberado');
+            console.log("Debounce liberado");
         }, 1000);
     }
 }
 
-function processarRemocaoItem(data, $tr) {
-    try {
-        if (data['tipoItem'] === 'Ocorrência') {
+function processarRemocaoItem(data, $tr)
+{
+    try
+    {
+        if (data["tipoItem"] === "Ocorrência")
+        {
 
             let Ocorrencias = [];
 
-            if (dataTableOcorrencias.data().count() > 0) {
-                for (var i = 0; i < dataTableOcorrencias.data().count(); i++) {
+            if (dataTableOcorrencias.data().count() > 0)
+            {
+                for (var i = 0; i < dataTableOcorrencias.data().count(); i++)
+                {
                     let ocorrencia = new Ocorrencia(
                         dataTableOcorrencias.cell(i, 0).data(),
                         dataTableOcorrencias.cell(i, 1).data(),
@@ -3908,43 +3754,42 @@
                 }
             }
 
-            $('#tblOcorrencia')
-                .DataTable()
-                .row.add({
-                    noFichaVistoria: data['numFicha'],
-                    dataInicial: data['dataItem'],
-                    nomeMotorista: data['nomeMotorista'],
-                    resumoOcorrencia: data['resumo'],
-                    viagemId: data['viagemId'],
-                    descricaoOcorrencia: data['descricao'],
-                    motoristaId: data['motoristaId'],
-                    imagemOcorrencia: data['imagemOcorrencia'],
-                    itemManutencaoId: data['itemManutencaoId'],
-                })
-                .draw(false);
-
-            Ocorrencias.forEach((o) => {
-                $('#tblOcorrencia')
-                    .DataTable()
-                    .row.add({
-                        noFichaVistoria: o.noFichaVistoria,
-                        dataInicial: o.data,
-                        nomeMotorista: o.motorista,
-                        resumoOcorrencia: o.resumo,
-                        viagemId: o.viagemId,
-                        descricaoOcorrencia: o.descricao,
-                        motoristaId: o.motoristaId,
-                        imagemOcorrencia: o.imagemOcorrencia,
-                        itemManutencaoId: o.itemManutencaoId,
-                    })
-                    .draw(false);
+            $("#tblOcorrencia").DataTable().row.add({
+                noFichaVistoria: data["numFicha"],
+                dataInicial: data["dataItem"],
+                nomeMotorista: data["nomeMotorista"],
+                resumoOcorrencia: data["resumo"],
+                viagemId: data["viagemId"],
+                descricaoOcorrencia: data["descricao"],
+                motoristaId: data["motoristaId"],
+                imagemOcorrencia: data["imagemOcorrencia"],
+                itemManutencaoId: data["itemManutencaoId"],
+            }).draw(false);
+
+            Ocorrencias.forEach((o) =>
+            {
+                $("#tblOcorrencia").DataTable().row.add({
+                    noFichaVistoria: o.noFichaVistoria,
+                    dataInicial: o.data,
+                    nomeMotorista: o.motorista,
+                    resumoOcorrencia: o.resumo,
+                    viagemId: o.viagemId,
+                    descricaoOcorrencia: o.descricao,
+                    motoristaId: o.motoristaId,
+                    imagemOcorrencia: o.imagemOcorrencia,
+                    itemManutencaoId: o.itemManutencaoId,
+                }).draw(false);
             });
-        } else if (data['tipoItem'] === 'Pendência') {
+
+        } else if (data["tipoItem"] === "Pendência")
+        {
 
             let Pendencias = [];
 
-            if (dataTablePendencias.data().count() > 0) {
-                for (var i = 0; i < dataTablePendencias.data().count(); i++) {
+            if (dataTablePendencias.data().count() > 0)
+            {
+                for (var i = 0; i < dataTablePendencias.data().count(); i++)
+                {
                     let pendencia = new Ocorrencia(
                         dataTablePendencias.cell(i, 0).data(),
                         dataTablePendencias.cell(i, 1).data(),
@@ -3962,50 +3807,39 @@
                 }
             }
 
-            $('#tblPendencia')
-                .DataTable()
-                .row.add({
-                    numFicha: data['numFicha'],
-                    dataItem: data['dataItem'],
-                    nome: data['nomeMotorista'],
-                    resumo: data['resumo'],
-                    itemManutencaoId: data['itemManutencaoId'],
-                    descricao: data['descricao'],
-                    motoristaId: data['motoristaId'],
-                    imagemOcorrencia: data['imagemOcorrencia'],
-                    viagemId: data['viagemId'],
-                })
-                .draw(false);
-
-            Pendencias.forEach((p) => {
-                $('#tblPendencia')
-                    .DataTable()
-                    .row.add({
-                        numFicha: p.noFichaVistoria,
-                        dataItem: p.data,
-                        nome: p.motorista,
-                        resumo: p.resumo,
-                        itemManutencaoId: p.itemManutencaoId,
-                        descricao: p.descricao,
-                        motoristaId: p.motoristaId,
-                        imagemOcorrencia: p.imagemOcorrencia,
-                        viagemId: p.viagemId,
-                    })
-                    .draw(false);
+            $("#tblPendencia").DataTable().row.add({
+                numFicha: data["numFicha"],
+                dataItem: data["dataItem"],
+                nome: data["nomeMotorista"],
+                resumo: data["resumo"],
+                itemManutencaoId: data["itemManutencaoId"],
+                descricao: data["descricao"],
+                motoristaId: data["motoristaId"],
+                imagemOcorrencia: data["imagemOcorrencia"],
+                viagemId: data["viagemId"],
+            }).draw(false);
+
+            Pendencias.forEach((p) =>
+            {
+                $("#tblPendencia").DataTable().row.add({
+                    numFicha: p.noFichaVistoria,
+                    dataItem: p.data,
+                    nome: p.motorista,
+                    resumo: p.resumo,
+                    itemManutencaoId: p.itemManutencaoId,
+                    descricao: p.descricao,
+                    motoristaId: p.motoristaId,
+                    imagemOcorrencia: p.imagemOcorrencia,
+                    viagemId: p.viagemId,
+                }).draw(false);
             });
         }
 
         dataTableItens.row($tr).remove().draw(false);
-        AppToast.show(
-            'Verde',
-            'Item removido e devolvido à lista original!',
-            2000,
-        );
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'manutencao.js',
-            'processarRemocaoItem',
-            error,
-        );
+        AppToast.show("Verde", "Item removido e devolvido à lista original!", 2000);
+
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("manutencao.js", "processarRemocaoItem", error);
     }
 }
```
