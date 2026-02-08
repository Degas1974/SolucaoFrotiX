# wwwroot/js/cadastros/ocorrencias.js

**Mudanca:** GRANDE | **+650** linhas | **-783** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/ocorrencias.js
+++ ATUAL: wwwroot/js/cadastros/ocorrencias.js
@@ -9,7 +9,7 @@
             overlay.style.display = 'flex';
         }
     } catch (error) {
-        console.warn('Erro ao mostrar loading:', error);
+        console.warn("Erro ao mostrar loading:", error);
     }
 }
 
@@ -20,245 +20,216 @@
             overlay.style.display = 'none';
         }
     } catch (error) {
-        console.warn('Erro ao esconder loading:', error);
+        console.warn("Erro ao esconder loading:", error);
     }
 }
 var imagemOcorrenciaAlterada = false;
-var novaImagemOcorrencia = '';
+var novaImagemOcorrencia = "";
 
 const CONECTORES = new Set([
-    'de',
-    'da',
-    'do',
-    'dos',
-    'das',
-    'e',
-    'd',
-    "d'",
-    'del',
-    'della',
-    'di',
-    'du',
-    'van',
-    'von',
+    "de", "da", "do", "dos", "das", "e", "d", "d'", "del", "della", "di", "du", "van", "von",
 ]);
 
-function abreviarNomeMotorista(nome) {
-    try {
-        if (!nome) return '';
+function abreviarNomeMotorista(nome)
+{
+    try
+    {
+        if (!nome) return "";
         const palavras = String(nome).trim().split(/\s+/);
         const out = [];
 
-        for (const w of palavras) {
-            const limp = w
-                .toLowerCase()
-                .normalize('NFD')
-                .replace(/[\u0300-\u036f]/g, '')
-                .replace(/[.:()]/g, '');
+        for (const w of palavras)
+        {
+            const limp = w.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "").replace(/[.:()]/g, "");
             if (CONECTORES.has(limp)) continue;
             out.push(w);
             if (out.length === 2) break;
         }
 
-        return out.join(' ');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'abreviarNomeMotorista',
-            error,
-        );
-        return nome || '';
-    }
-}
-
-function _keyIsoFromBR(value) {
-    try {
-        if (!value) return '';
-        const [dd, mm, yyyy] = value.split('/');
+        return out.join(" ");
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "abreviarNomeMotorista", error);
+        return nome || "";
+    }
+}
+
+function _keyIsoFromBR(value)
+{
+    try
+    {
+        if (!value) return "";
+        const [dd, mm, yyyy] = value.split("/");
         return `${yyyy}-${mm}-${dd}`;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('ocorrencias.js', '_keyIsoFromBR', error);
-        return '';
-    }
-}
-
-function getComboValue(comboId) {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "_keyIsoFromBR", error);
+        return "";
+    }
+}
+
+function getComboValue(comboId)
+{
+    try
+    {
         const el = document.getElementById(comboId);
-        if (el && el.ej2_instances && el.ej2_instances.length > 0) {
+        if (el && el.ej2_instances && el.ej2_instances.length > 0)
+        {
             const inst = el.ej2_instances[0];
-            if (inst && inst.value != null && inst.value !== '')
-                return inst.value;
-        }
-        return '';
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('ocorrencias.js', 'getComboValue', error);
-        return '';
-    }
-}
-
-function BuildGridOcorrencias(params) {
-    try {
+            if (inst && inst.value != null && inst.value !== "") return inst.value;
+        }
+        return "";
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "getComboValue", error);
+        return "";
+    }
+}
+
+function BuildGridOcorrencias(params)
+{
+    try
+    {
 
         mostrarLoadingOcorrencias('Carregando Ocorrências...');
 
-        if ($.fn.DataTable.isDataTable('#tblOcorrencia')) {
-            $('#tblOcorrencia').DataTable().destroy();
-            $('#tblOcorrencia tbody').empty();
-        }
-
-        dataTable = $('#tblOcorrencia').DataTable({
+        if ($.fn.DataTable.isDataTable("#tblOcorrencia"))
+        {
+            $("#tblOcorrencia").DataTable().destroy();
+            $("#tblOcorrencia tbody").empty();
+        }
+
+        dataTable = $("#tblOcorrencia").DataTable({
             autoWidth: false,
-            dom: 'Bfrtip',
-            lengthMenu: [
-                [10, 25, 50, -1],
-                ['10 linhas', '25 linhas', '50 linhas', 'Todas'],
-            ],
-            buttons: [
-                'pageLength',
-                'excel',
-                {
-                    extend: 'pdfHtml5',
-                    orientation: 'landscape',
-                    pageSize: 'LEGAL',
-                },
-            ],
-            order: [[1, 'desc']],
+            dom: "Bfrtip",
+            lengthMenu: [[10, 25, 50, -1], ["10 linhas", "25 linhas", "50 linhas", "Todas"]],
+            buttons: ["pageLength", "excel", { extend: "pdfHtml5", orientation: "landscape", pageSize: "LEGAL" }],
+            order: [[1, "desc"]],
             columnDefs: [
-                { targets: 0, className: 'text-center', width: '5%' },
+                { targets: 0, className: "text-center", width: "5%" },
                 {
                     targets: 1,
-                    className: 'text-center',
-                    width: '8%',
-                    render: function (value, type) {
-                        try {
-                            if (!value) return '';
-                            if (type === 'sort' || type === 'type') {
-                                if (/^\d{2}\/\d{2}\/\d{4}$/.test(value))
-                                    return _keyIsoFromBR(value);
+                    className: "text-center",
+                    width: "8%",
+                    render: function (value, type)
+                    {
+                        try
+                        {
+                            if (!value) return "";
+                            if (type === "sort" || type === "type")
+                            {
+                                if (/^\d{2}\/\d{2}\/\d{4}$/.test(value)) return _keyIsoFromBR(value);
                             }
                             return value;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ocorrencias.js',
-                                'grid.render.data',
-                                error,
-                            );
-                            return '';
                         }
-                    },
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.data", error);
+                            return "";
+                        }
+                    }
                 },
                 {
                     targets: 2,
-                    className: 'text-left',
-                    width: '12%',
-                    render: function (data, type) {
-                        try {
-                            return type === 'display'
-                                ? abreviarNomeMotorista(data)
-                                : data;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ocorrencias.js',
-                                'grid.render.motorista',
-                                error,
-                            );
+                    className: "text-left",
+                    width: "12%",
+                    render: function (data, type)
+                    {
+                        try
+                        {
+                            return type === "display" ? abreviarNomeMotorista(data) : data;
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.motorista", error);
                             return data;
                         }
-                    },
+                    }
                 },
-                { targets: 3, className: 'text-left', width: '15%' },
-                { targets: 4, className: 'text-left', width: '15%' },
-                { targets: 5, className: 'text-left', width: '15%' },
-                { targets: 6, className: 'text-center', width: '8%' },
-                { targets: 7, className: 'text-center', width: '8%' },
-                { targets: 8, visible: false },
+                { targets: 3, className: "text-left", width: "15%" },
+                { targets: 4, className: "text-left", width: "15%" },
+                { targets: 5, className: "text-left", width: "15%" },
+                { targets: 6, className: "text-center", width: "8%" },
+                { targets: 7, className: "text-center", width: "8%" },
+                { targets: 8, visible: false }
             ],
             responsive: true,
             ajax: {
-                url: '/api/OcorrenciaViagem/ListarGestao',
-                type: 'GET',
-                dataType: 'json',
+                url: "/api/OcorrenciaViagem/ListarGestao",
+                type: "GET",
+                dataType: "json",
                 data: params,
-                error: function (xhr, error, thrown) {
-                    try {
+                error: function (xhr, error, thrown)
+                {
+                    try
+                    {
                         esconderLoadingOcorrencias();
-                        console.error(
-                            'Erro ao carregar ocorrências:',
-                            error,
-                            thrown,
-                        );
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao carregar ocorrências',
-                            3000,
-                        );
-                    } catch (err) {
-                        Alerta.TratamentoErroComLinha(
-                            'ocorrencias.js',
-                            'ajax.error',
-                            err,
-                        );
+                        console.error("Erro ao carregar ocorrências:", error, thrown);
+                        AppToast.show("Vermelho", "Erro ao carregar ocorrências", 3000);
+                    }
+                    catch (err)
+                    {
+                        Alerta.TratamentoErroComLinha("ocorrencias.js", "ajax.error", err);
+                    }
+                }
+            },
+            columns: [
+                { data: "noFichaVistoria", defaultContent: "-" },
+                { data: "data", defaultContent: "-" },
+                { data: "nomeMotorista", defaultContent: "-" },
+                { data: "descricaoVeiculo", defaultContent: "-" },
+                { data: "resumoOcorrencia", defaultContent: "-" },
+                { data: "descricaoSolucaoOcorrencia", defaultContent: "-" },
+                {
+                    data: "statusOcorrencia",
+                    render: function (data, type, row)
+                    {
+                        try
+                        {
+                            var s = row.statusOcorrencia || "Aberta";
+                            var icon = "";
+                            var badgeClass = "ftx-badge-aberta";
+
+                            switch (s)
+                            {
+                                case "Aberta":
+                                    icon = '<i class="fa-duotone fa-circle-exclamation me-1"></i>';
+                                    badgeClass = "ftx-badge-aberta";
+                                    break;
+                                case "Baixada":
+                                    icon = '<i class="fa-duotone fa-circle-check me-1"></i>';
+                                    badgeClass = "ftx-badge-baixada";
+                                    break;
+                                case "Pendente":
+                                    icon = '<i class="fa-duotone fa-clock me-1"></i>';
+                                    badgeClass = "ftx-badge-pendente";
+                                    break;
+                                case "Manutenção":
+                                    icon = '<i class="fa-duotone fa-wrench me-1"></i>';
+                                    badgeClass = "ftx-badge-manutencao";
+                                    break;
+                            }
+
+                            return `<span class="ftx-badge-status ${badgeClass}">${icon}${s}</span>`;
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.status", error);
+                            return "";
+                        }
                     }
                 },
-            },
-            columns: [
-                { data: 'noFichaVistoria', defaultContent: '-' },
-                { data: 'data', defaultContent: '-' },
-                { data: 'nomeMotorista', defaultContent: '-' },
-                { data: 'descricaoVeiculo', defaultContent: '-' },
-                { data: 'resumoOcorrencia', defaultContent: '-' },
-                { data: 'descricaoSolucaoOcorrencia', defaultContent: '-' },
-                {
-                    data: 'statusOcorrencia',
-                    render: function (data, type, row) {
-                        try {
-                            var s = row.statusOcorrencia || 'Aberta';
-                            var icon = '';
-                            var badgeClass = 'ftx-badge-aberta';
-
-                            switch (s) {
-                                case 'Aberta':
-                                    icon =
-                                        '<i class="fa-duotone fa-circle-exclamation me-1"></i>';
-                                    badgeClass = 'ftx-badge-aberta';
-                                    break;
-                                case 'Baixada':
-                                    icon =
-                                        '<i class="fa-duotone fa-circle-check me-1"></i>';
-                                    badgeClass = 'ftx-badge-baixada';
-                                    break;
-                                case 'Pendente':
-                                    icon =
-                                        '<i class="fa-duotone fa-clock me-1"></i>';
-                                    badgeClass = 'ftx-badge-pendente';
-                                    break;
-                                case 'Manutenção':
-                                    icon =
-                                        '<i class="fa-duotone fa-wrench me-1"></i>';
-                                    badgeClass = 'ftx-badge-manutencao';
-                                    break;
-                            }
-
-                            return `<span class="ftx-badge-status ${badgeClass}">${icon}${s}</span>`;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ocorrencias.js',
-                                'grid.render.status',
-                                error,
-                            );
-                            return '';
-                        }
-                    },
-                },
-                {
-                    data: 'ocorrenciaViagemId',
-                    render: function (data, type, row) {
-                        try {
-                            var baixada = row.statusOcorrencia === 'Baixada';
-                            var temImagem =
-                                row.imagemOcorrencia &&
-                                row.imagemOcorrencia.trim() !== '';
+                {
+                    data: "ocorrenciaViagemId",
+                    render: function (data, type, row)
+                    {
+                        try
+                        {
+                            var baixada = row.statusOcorrencia === "Baixada";
+                            var temImagem = row.imagemOcorrencia && row.imagemOcorrencia.trim() !== "";
 
                             var btnEditar = `
                                 <a class="btn-azul btn-icon-28 btn-editar-ocorrencia"
@@ -291,150 +262,139 @@
                                 ${btnBaixa}
                                 ${btnImagem}
                             </div>`;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ocorrencias.js',
-                                'grid.render.acoes',
-                                error,
-                            );
-                            return '';
                         }
-                    },
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.acoes", error);
+                            return "";
+                        }
+                    }
                 },
-                { data: 'descricaoOcorrencia', defaultContent: '' },
+                { data: "descricaoOcorrencia", defaultContent: "" }
             ],
             language: {
-                url: '
+                url: "
             },
-            drawCallback: function () {
-                try {
-                    console.log(
-                        '[ocorrencias.js] Grid carregada com',
-                        this.api().rows().count(),
-                        'registros',
-                    );
+            drawCallback: function ()
+            {
+                try
+                {
+                    console.log("[ocorrencias.js] Grid carregada com", this.api().rows().count(), "registros");
 
                     esconderLoadingOcorrencias();
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ocorrencias.js',
-                        'drawCallback',
-                        error,
-                    );
-                }
-            },
-        });
-    } catch (error) {
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ocorrencias.js", "drawCallback", error);
+                }
+            }
+        });
+    }
+    catch (error)
+    {
         esconderLoadingOcorrencias();
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'BuildGridOcorrencias',
-            error,
-        );
-    }
-}
-
-function collectParamsFromUI() {
-    try {
-        const data = ($('#txtData').val() || '').trim();
-        const dataInicial = ($('#txtDataInicial').val() || '').trim();
-        const dataFinal = ($('#txtDataFinal').val() || '').trim();
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "BuildGridOcorrencias", error);
+    }
+}
+
+function collectParamsFromUI()
+{
+    try
+    {
+        const data = ($("#txtData").val() || "").trim();
+        const dataInicial = ($("#txtDataInicial").val() || "").trim();
+        const dataFinal = ($("#txtDataFinal").val() || "").trim();
         const temPeriodo = dataInicial && dataFinal;
 
-        const veiculoId = getComboValue('lstVeiculos');
-        const motoristaId = getComboValue('lstMotorista');
-
-        let statusId = getComboValue('lstStatus');
-        if (!statusId) {
-            statusId =
-                veiculoId || motoristaId || data || temPeriodo
-                    ? 'Todas'
-                    : 'Aberta';
+        const veiculoId = getComboValue("lstVeiculos");
+        const motoristaId = getComboValue("lstMotorista");
+
+        let statusId = getComboValue("lstStatus");
+        if (!statusId)
+        {
+            statusId = (veiculoId || motoristaId || data || temPeriodo) ? "Todas" : "Aberta";
         }
 
         return {
             veiculoId: veiculoId,
             motoristaId: motoristaId,
             statusId: statusId,
-            data: temPeriodo ? '' : data,
-            dataInicial: temPeriodo ? dataInicial : '',
-            dataFinal: temPeriodo ? dataFinal : '',
+            data: temPeriodo ? "" : data,
+            dataInicial: temPeriodo ? dataInicial : "",
+            dataFinal: temPeriodo ? dataFinal : ""
         };
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'collectParamsFromUI',
-            error,
-        );
-        return { statusId: 'Aberta' };
-    }
-}
-
-function validateDatesBeforeSearch() {
-    try {
-        const dataInicial = ($('#txtDataInicial').val() || '').trim();
-        const dataFinal = ($('#txtDataFinal').val() || '').trim();
-
-        if ((dataInicial && !dataFinal) || (!dataInicial && dataFinal)) {
-            Alerta.Erro(
-                'Informação Ausente',
-                'Para filtrar por período, preencha Data Inicial e Data Final.',
-                'OK',
-            );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "collectParamsFromUI", error);
+        return { statusId: "Aberta" };
+    }
+}
+
+function validateDatesBeforeSearch()
+{
+    try
+    {
+        const dataInicial = ($("#txtDataInicial").val() || "").trim();
+        const dataFinal = ($("#txtDataFinal").val() || "").trim();
+
+        if ((dataInicial && !dataFinal) || (!dataInicial && dataFinal))
+        {
+            Alerta.Erro("Informação Ausente", "Para filtrar por período, preencha Data Inicial e Data Final.", "OK");
             return false;
         }
 
         return true;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'validateDatesBeforeSearch',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "validateDatesBeforeSearch", error);
         return false;
     }
 }
 
-async function uploadImagemOcorrencia(file) {
-    try {
+async function uploadImagemOcorrencia(file)
+{
+    try
+    {
         const formData = new FormData();
-        formData.append('file', file);
-
-        const response = await fetch('/api/OcorrenciaViagem/UploadImagem', {
-            method: 'POST',
-            body: formData,
+        formData.append("file", file);
+
+        const response = await fetch("/api/OcorrenciaViagem/UploadImagem", {
+            method: "POST",
+            body: formData
         });
 
         const data = await response.json();
 
-        if (data.success) {
+        if (data.success)
+        {
             imagemOcorrenciaAlterada = true;
-            novaImagemOcorrencia = data.path || data.url || '';
+            novaImagemOcorrencia = data.path || data.url || "";
             exibirPreviewImagem(novaImagemOcorrencia);
-            AppToast.show('Verde', 'Imagem enviada com sucesso!', 2000);
-        } else {
-            AppToast.show(
-                'Vermelho',
-                data.message || 'Erro ao enviar imagem.',
-                3000,
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'uploadImagemOcorrencia',
-            error,
-        );
-        AppToast.show('Vermelho', 'Erro ao enviar imagem.', 3000);
-    }
-}
-
-function exibirPreviewImagem(src) {
-    try {
-        const container = $('#divImagemOcorrencia');
+            AppToast.show("Verde", "Imagem enviada com sucesso!", 2000);
+        }
+        else
+        {
+            AppToast.show("Vermelho", data.message || "Erro ao enviar imagem.", 3000);
+        }
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "uploadImagemOcorrencia", error);
+        AppToast.show("Vermelho", "Erro ao enviar imagem.", 3000);
+    }
+}
+
+function exibirPreviewImagem(src)
+{
+    try
+    {
+        const container = $("#divImagemOcorrencia");
         container.empty();
 
-        if (!src) {
+        if (!src)
+        {
             container.html(`
                 <div class="p-3 text-center border rounded bg-light" style="cursor:pointer;" onclick="$('#inputImagemOcorrencia').click();">
                     <i class="fa-duotone fa-image fa-3x text-muted mb-2"></i>
@@ -446,7 +406,8 @@
 
         const isVideo = /\.(mp4|webm)$/i.test(src);
 
-        if (isVideo) {
+        if (isVideo)
+        {
             container.html(`
                 <div class="position-relative">
                     <video src="${src}" controls style="max-width:100%; max-height:200px; border-radius:8px;"></video>
@@ -455,7 +416,9 @@
                     </button>
                 </div>
             `);
-        } else {
+        }
+        else
+        {
             container.html(`
                 <div class="position-relative">
                     <img src="${src}" alt="Preview" style="max-width:100%; max-height:200px; border-radius:8px; cursor:pointer;" onclick="$('#inputImagemOcorrencia').click();" />
@@ -465,617 +428,524 @@
                 </div>
             `);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'exibirPreviewImagem',
-            error,
-        );
-    }
-}
-
-function removerImagemOcorrencia() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "exibirPreviewImagem", error);
+    }
+}
+
+function removerImagemOcorrencia()
+{
+    try
+    {
         imagemOcorrenciaAlterada = true;
-        novaImagemOcorrencia = '';
-        exibirPreviewImagem('');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'removerImagemOcorrencia',
-            error,
-        );
-    }
-}
-
-function limparModal() {
-    try {
-        $('#txtId').val('');
-        $('#txtResumo').val('');
-        $('#txtImagemOcorrenciaAtual').val('');
-        $('#chkStatusOcorrencia').val('');
+        novaImagemOcorrencia = "";
+        exibirPreviewImagem("");
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "removerImagemOcorrencia", error);
+    }
+}
+
+function limparModal()
+{
+    try
+    {
+        $("#txtId").val("");
+        $("#txtResumo").val("");
+        $("#txtImagemOcorrenciaAtual").val("");
+        $("#chkStatusOcorrencia").val("");
         imagemOcorrenciaAlterada = false;
-        novaImagemOcorrencia = '';
-
-        const rteDesc =
-            document.getElementById('rteOcorrencias')?.ej2_instances?.[0];
-        const rteSol =
-            document.getElementById('rteSolucao')?.ej2_instances?.[0];
-        if (rteDesc) rteDesc.value = '';
-        if (rteSol) rteSol.value = '';
-
-        exibirPreviewImagem('');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('ocorrencias.js', 'limparModal', error);
-    }
-}
-
-function fecharModalOcorrencia() {
-    try {
-        const modal = bootstrap.Modal.getInstance(
-            document.getElementById('modalOcorrencia'),
-        );
+        novaImagemOcorrencia = "";
+
+        const rteDesc = document.getElementById("rteOcorrencias")?.ej2_instances?.[0];
+        const rteSol = document.getElementById("rteSolucao")?.ej2_instances?.[0];
+        if (rteDesc) rteDesc.value = "";
+        if (rteSol) rteSol.value = "";
+
+        exibirPreviewImagem("");
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "limparModal", error);
+    }
+}
+
+function fecharModalOcorrencia()
+{
+    try
+    {
+        const modal = bootstrap.Modal.getInstance(document.getElementById("modalOcorrencia"));
         if (modal) modal.hide();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'fecharModalOcorrencia',
-            error,
-        );
-    }
-}
-
-function fecharModalBaixaRapida() {
-    try {
-        const modal = bootstrap.Modal.getInstance(
-            document.getElementById('modalBaixaRapida'),
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "fecharModalOcorrencia", error);
+    }
+}
+
+function fecharModalBaixaRapida()
+{
+    try
+    {
+        const modal = bootstrap.Modal.getInstance(document.getElementById("modalBaixaRapida"));
         if (modal) modal.hide();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'fecharModalBaixaRapida',
-            error,
-        );
-    }
-}
-
-async function carregarOcorrencia(id) {
-    try {
-        const response = await fetch(
-            `/api/OcorrenciaViagem/ObterOcorrencia?id=${id}`,
-        );
-        const data = await response.json();
-
-        if (data.success && data.ocorrencia) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "fecharModalBaixaRapida", error);
+    }
+}
+
+async function carregarOcorrencia(id)
+{
+    try
+    {
+        if (!id) {
+             console.warn("ID inválido para carregar ocorrência");
+             return;
+        }
+
+        const response = await fetch(`/api/OcorrenciaViagem/ObterOcorrencia?id=${id}`);
+
+        if (!response.ok) {
+            throw new Error(`Erro HTTP: ${response.status}`);
+        }
+
+        const text = await response.text();
+        let data;
+        try {
+            data = JSON.parse(text);
+        } catch (e) {
+            console.error("Erro ao parsear resposta servida:", text);
+            throw new Error("Resposta inválida do servidor (não é JSON).");
+        }
+
+        if (data.success && data.ocorrencia)
+        {
             const oc = data.ocorrencia;
 
-            $('#txtId').val(oc.ocorrenciaViagemId || '');
-            $('#txtResumo').val(oc.resumoOcorrencia || '');
-            $('#txtImagemOcorrenciaAtual').val(oc.imagemOcorrencia || '');
-            $('#chkStatusOcorrencia').val(oc.statusOcorrencia || 'Aberta');
-
-            const rteDesc =
-                document.getElementById('rteOcorrencias')?.ej2_instances?.[0];
-            const rteSol =
-                document.getElementById('rteSolucao')?.ej2_instances?.[0];
-
-            if (rteDesc) rteDesc.value = oc.descricaoOcorrencia || '';
-            if (rteSol) rteSol.value = oc.solucaoOcorrencia || '';
-
-            exibirPreviewImagem(oc.imagemOcorrencia || '');
-
-            const titulo =
-                oc.statusOcorrencia === 'Baixada'
-                    ? 'Visualizar Ocorrência'
-                    : 'Editar Ocorrência';
-            $('#modalOcorrenciaLabel span').text(titulo);
-
-            const baixada = oc.statusOcorrencia === 'Baixada';
-            $('#btnBaixarOcorrenciaModal').prop('disabled', baixada);
-            $('#btnEditarOcorrencia').prop('disabled', baixada);
-
-            new bootstrap.Modal(
-                document.getElementById('modalOcorrencia'),
-            ).show();
-        } else {
-            AppToast.show(
-                'Vermelho',
-                data.message || 'Erro ao carregar ocorrência.',
-                3000,
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'carregarOcorrencia',
-            error,
-        );
-        AppToast.show('Vermelho', 'Erro ao carregar ocorrência.', 3000);
-    }
-}
-
-function verificarSolucaoPreenchida(solucao) {
-    try {
+            $("#txtId").val(oc.ocorrenciaViagemId || "");
+            $("#txtResumo").val(oc.resumoOcorrencia || "");
+            $("#txtImagemOcorrenciaAtual").val(oc.imagemOcorrencia || "");
+            $("#chkStatusOcorrencia").val(oc.statusOcorrencia || "Aberta");
+
+            const rteDesc = document.getElementById("rteOcorrencias")?.ej2_instances?.[0];
+            const rteSol = document.getElementById("rteSolucao")?.ej2_instances?.[0];
+
+            if (rteDesc) rteDesc.value = oc.descricaoOcorrencia || "";
+            if (rteSol) rteSol.value = oc.solucaoOcorrencia || "";
+
+            exibirPreviewImagem(oc.imagemOcorrencia || "");
+
+            const titulo = oc.statusOcorrencia === "Baixada" ? "Visualizar Ocorrência" : "Editar Ocorrência";
+            $("#modalOcorrenciaLabel span").text(titulo);
+
+            const baixada = oc.statusOcorrencia === "Baixada";
+            $("#btnBaixarOcorrenciaModal").prop("disabled", baixada);
+            $("#btnEditarOcorrencia").prop("disabled", baixada);
+
+            new bootstrap.Modal(document.getElementById("modalOcorrencia")).show();
+        }
+        else
+        {
+            AppToast.show("Vermelho", data.message || "Erro ao carregar ocorrência.", 3000);
+        }
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "carregarOcorrencia", error);
+        AppToast.show("Vermelho", "Erro ao carregar ocorrência.", 3000);
+    }
+}
+
+function verificarSolucaoPreenchida(solucao)
+{
+    try
+    {
         if (!solucao) return false;
-        const texto = solucao.replace(/<[^>]*>/g, '').trim();
+        const texto = solucao.replace(/<[^>]*>/g, "").trim();
         return texto.length > 0;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'verificarSolucaoPreenchida',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "verificarSolucaoPreenchida", error);
         return false;
     }
 }
 
-async function executarBaixaOcorrencia(id, solucao, callbackSucesso) {
-    try {
+async function executarBaixaOcorrencia(id, solucao, callbackSucesso)
+{
+    try
+    {
         const payload = {
             OcorrenciaViagemId: id,
             SolucaoOcorrencia: solucao,
-            StatusOcorrencia: 'Baixada',
+            StatusOcorrencia: "Baixada"
         };
 
-        const response = await fetch('/api/OcorrenciaViagem/BaixarOcorrencia', {
-            method: 'POST',
-            headers: { 'Content-Type': 'application/json' },
-            body: JSON.stringify(payload),
+        const response = await fetch("/api/OcorrenciaViagem/BaixarOcorrencia", {
+            method: "POST",
+            headers: { "Content-Type": "application/json" },
+            body: JSON.stringify(payload)
         });
 
         const data = await response.json();
 
-        if (data.success) {
-            AppToast.show(
-                'Verde',
-                data.message || 'Ocorrência baixada com sucesso!',
-                2000,
-            );
+        if (data.success)
+        {
+            AppToast.show("Verde", data.message || "Ocorrência baixada com sucesso!", 2000);
             if (callbackSucesso) callbackSucesso();
             if (dataTable) dataTable.ajax.reload(null, false);
-        } else {
-            AppToast.show(
-                'Vermelho',
-                data.message || 'Erro ao baixar ocorrência.',
-                3000,
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'executarBaixaOcorrencia',
-            error,
-        );
-        AppToast.show('Vermelho', 'Erro ao baixar ocorrência.', 3000);
-    }
-}
-
-async function processarBaixaComValidacao(id, solucaoAtual, callbackSucesso) {
-    try {
-        if (verificarSolucaoPreenchida(solucaoAtual)) {
+        }
+        else
+        {
+            AppToast.show("Vermelho", data.message || "Erro ao baixar ocorrência.", 3000);
+        }
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "executarBaixaOcorrencia", error);
+        AppToast.show("Vermelho", "Erro ao baixar ocorrência.", 3000);
+    }
+}
+
+async function processarBaixaComValidacao(id, solucaoAtual, callbackSucesso)
+{
+    try
+    {
+        if (verificarSolucaoPreenchida(solucaoAtual))
+        {
 
             await executarBaixaOcorrencia(id, solucaoAtual, callbackSucesso);
-        } else {
+        }
+        else
+        {
 
             if (callbackSucesso) callbackSucesso();
-            $('#txtBaixaRapidaId').val(id);
-            new bootstrap.Modal(
-                document.getElementById('modalBaixaRapida'),
-            ).show();
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'processarBaixaComValidacao',
-            error,
-        );
-    }
-}
-
-function abrirVisualizacaoImagem(src) {
-    try {
-        const container = $('#divImagemVisualizacao');
+            $("#txtBaixaRapidaId").val(id);
+            new bootstrap.Modal(document.getElementById("modalBaixaRapida")).show();
+        }
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "processarBaixaComValidacao", error);
+    }
+}
+
+function abrirVisualizacaoImagem(src)
+{
+    try
+    {
+        const container = $("#divImagemVisualizacao");
         container.empty();
 
-        if (!src) {
+        if (!src)
+        {
             container.html('<p class="text-muted">Sem imagem disponível</p>');
             return;
         }
 
         const isVideo = /\.(mp4|webm)$/i.test(src);
 
-        if (isVideo) {
-            container.html(
-                `<video src="${src}" controls style="max-width:100%; max-height:500px;"></video>`,
-            );
-            $('#modalVisualizarImagem .modal-title').html(
-                '<i class="fa-duotone fa-video me-2"></i>Vídeo da Ocorrência',
-            );
-        } else {
-            container.html(
-                `<img src="${src}" alt="Imagem" style="max-width:100%; max-height:500px;" />`,
-            );
-            $('#modalVisualizarImagem .modal-title').html(
-                '<i class="fa-duotone fa-image me-2"></i>Imagem da Ocorrência',
-            );
-        }
-
-        new bootstrap.Modal(
-            document.getElementById('modalVisualizarImagem'),
-        ).show();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'abrirVisualizacaoImagem',
-            error,
-        );
-    }
-}
-
-$(document).ready(function () {
-    try {
-
-        BuildGridOcorrencias({ statusId: 'Aberta' });
-
-        $('#btnFiltrarOcorrencias').on('click', function () {
-            try {
+        if (isVideo)
+        {
+            container.html(`<video src="${src}" controls style="max-width:100%; max-height:500px;"></video>`);
+            $("#modalVisualizarImagem .modal-title").html('<i class="fa-duotone fa-video me-2"></i>Vídeo da Ocorrência');
+        }
+        else
+        {
+            container.html(`<img src="${src}" alt="Imagem" style="max-width:100%; max-height:500px;" />`);
+            $("#modalVisualizarImagem .modal-title").html('<i class="fa-duotone fa-image me-2"></i>Imagem da Ocorrência');
+        }
+
+        new bootstrap.Modal(document.getElementById("modalVisualizarImagem")).show();
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "abrirVisualizacaoImagem", error);
+    }
+}
+
+$(document).ready(function ()
+{
+    try
+    {
+
+        BuildGridOcorrencias({ statusId: "Aberta" });
+
+        $("#btnFiltrarOcorrencias").on("click", function ()
+        {
+            try
+            {
                 if (!validateDatesBeforeSearch()) return;
                 const params = collectParamsFromUI();
                 BuildGridOcorrencias(params);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ocorrencias.js',
-                    'btnFiltrar.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.btn-editar-ocorrencia', function (e) {
-            try {
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnFiltrar.click", error);
+            }
+        });
+
+        $(document).on("click", ".btn-editar-ocorrencia", function (e)
+        {
+            try
+            {
                 e.preventDefault();
-                const id = $(this).data('id');
+                const id = $(this).data("id");
                 if (id) carregarOcorrencia(id);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ocorrencias.js',
-                    'btnEditar.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.btn-ver-imagem:not(.disabled)', function (e) {
-            try {
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnEditar.click", error);
+            }
+        });
+
+        $(document).on("click", ".btn-ver-imagem:not(.disabled)", function (e)
+        {
+            try
+            {
                 e.preventDefault();
-                const imagem = $(this).data('imagem');
+                const imagem = $(this).data("imagem");
                 if (imagem) abrirVisualizacaoImagem(imagem);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ocorrencias.js',
-                    'btnVerImagem.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on(
-            'click',
-            '.btn-baixar:not(.disabled)',
-            async function (e) {
-                try {
-                    e.preventDefault();
-                    const id = $(this).data('id');
-                    if (id) await processarBaixaComValidacao(id, '', null);
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ocorrencias.js',
-                        'btnBaixar.click',
-                        error,
-                    );
-                }
-            },
-        );
-
-        $('#btnConfirmarBaixaRapida').on('click', async function (e) {
-            try {
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnVerImagem.click", error);
+            }
+        });
+
+        $(document).on("click", ".btn-baixar:not(.disabled)", async function (e)
+        {
+            try
+            {
                 e.preventDefault();
+                const id = $(this).data("id");
+                if (id) await processarBaixaComValidacao(id, "", null);
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnBaixar.click", error);
+            }
+        });
+
+        $("#btnConfirmarBaixaRapida").on("click", async function (e)
+        {
+            try
+            {
+                e.preventDefault();
 
                 const $btn = $(this);
-                if ($btn.data('busy')) return;
-
-                const id = $('#txtBaixaRapidaId').val();
-                const solucao = (
-                    $('#txtBaixaRapidaSolucao').val() || ''
-                ).trim();
-
-                if (!solucao) {
-                    Alerta.Erro(
-                        'Informação Ausente',
-                        'Preencha a Solução da Ocorrência.',
-                        'OK',
-                    );
+                if ($btn.data("busy")) return;
+
+                const id = $("#txtBaixaRapidaId").val();
+                const solucao = ($("#txtBaixaRapidaSolucao").val() || "").trim();
+
+                if (!solucao)
+                {
+                    Alerta.Erro("Informação Ausente", "Preencha a Solução da Ocorrência.", "OK");
                     return;
                 }
 
-                $btn.data('busy', true)
-                    .prop('disabled', true)
-                    .html(
-                        '<i class="fa-duotone fa-spinner-third fa-spin me-1"></i> Baixando...',
-                    );
-
-                await executarBaixaOcorrencia(id, solucao, function () {
+                $btn.data("busy", true).prop("disabled", true).html('<i class="fa-duotone fa-spinner-third fa-spin me-1"></i> Baixando...');
+
+                await executarBaixaOcorrencia(id, solucao, function() {
                     fecharModalBaixaRapida();
                 });
 
-                $btn.data('busy', false)
-                    .prop('disabled', false)
-                    .html(
-                        '<i class="fa-duotone fa-flag-checkered me-1" style="color:#fff;"></i> Confirmar Baixa',
-                    );
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ocorrencias.js',
-                    'btnConfirmarBaixaRapida.click',
-                    error,
-                );
-                $('#btnConfirmarBaixaRapida')
-                    .data('busy', false)
-                    .prop('disabled', false)
-                    .html(
-                        '<i class="fa-duotone fa-flag-checkered me-1" style="color:#fff;"></i> Confirmar Baixa',
-                    );
-            }
-        });
-
-        $('#modalBaixaRapida').on('hidden.bs.modal', function () {
-            try {
-                $('#txtBaixaRapidaId').val('');
-                $('#txtBaixaRapidaSolucao').val('');
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ocorrencias.js',
-                    'modalBaixaRapida.hidden',
-                    error,
-                );
-            }
-        });
-
-        $('#btnBaixarOcorrenciaModal').on('click', async function (e) {
-            try {
+                $btn.data("busy", false).prop("disabled", false).html('<i class="fa-duotone fa-flag-checkered me-1" style="color:#fff;"></i> Confirmar Baixa');
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnConfirmarBaixaRapida.click", error);
+                $("#btnConfirmarBaixaRapida").data("busy", false).prop("disabled", false).html('<i class="fa-duotone fa-flag-checkered me-1" style="color:#fff;"></i> Confirmar Baixa');
+            }
+        });
+
+        $("#modalBaixaRapida").on("hidden.bs.modal", function ()
+        {
+            try
+            {
+                $("#txtBaixaRapidaId").val("");
+                $("#txtBaixaRapidaSolucao").val("");
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("ocorrencias.js", "modalBaixaRapida.hidden", error);
+            }
+        });
+
+        $("#btnBaixarOcorrenciaModal").on("click", async function (e)
+        {
+            try
+            {
                 e.preventDefault();
 
-                const id = $('#txtId').val();
-                if (!id) {
-                    Alerta.Erro(
-                        'Erro',
-                        'ID da ocorrência não encontrado.',
-                        'OK',
-                    );
+                const id = $("#txtId").val();
+                if (!id)
+                {
+                    Alerta.Erro("Erro", "ID da ocorrência não encontrado.", "OK");
                     return;
                 }
 
-                const rteSol =
-                    document.getElementById('rteSolucao')?.ej2_instances?.[0];
-                const solucaoAtual = rteSol?.value || '';
-
-                await processarBaixaComValidacao(
-                    id,
-                    solucaoAtual,
-                    fecharModalOcorrencia,
-                );
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ocorrencias.js',
-                    'btnBaixarModal.click',
-                    error,
-                );
-            }
-        });
-
-        $('#btnEditarOcorrencia').on('click', async function (e) {
-            try {
+                const rteSol = document.getElementById("rteSolucao")?.ej2_instances?.[0];
+                const solucaoAtual = rteSol?.value || "";
+
+                await processarBaixaComValidacao(id, solucaoAtual, fecharModalOcorrencia);
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnBaixarModal.click", error);
+            }
+        });
+
+        $("#btnEditarOcorrencia").on("click", async function (e)
+        {
+            try
+            {
                 e.preventDefault();
 
                 const $btn = $(this);
-                if ($btn.data('busy')) return;
-
-                const resumo = $('#txtResumo').val();
-                if (!resumo) {
-                    Alerta.Erro(
-                        'Informação Ausente',
-                        'O Resumo da Ocorrência é obrigatório.',
-                        'OK',
-                    );
+                if ($btn.data("busy")) return;
+
+                const resumo = $("#txtResumo").val();
+                if (!resumo)
+                {
+                    Alerta.Erro("Informação Ausente", "O Resumo da Ocorrência é obrigatório.", "OK");
                     return;
                 }
 
-                const rteDesc =
-                    document.getElementById('rteOcorrencias')
-                        ?.ej2_instances?.[0];
-                const rteSol =
-                    document.getElementById('rteSolucao')?.ej2_instances?.[0];
-
-                let imagemFinal = $('#txtImagemOcorrenciaAtual').val() || '';
-                if (imagemOcorrenciaAlterada) {
+                const rteDesc = document.getElementById("rteOcorrencias")?.ej2_instances?.[0];
+                const rteSol = document.getElementById("rteSolucao")?.ej2_instances?.[0];
+
+                let imagemFinal = $("#txtImagemOcorrenciaAtual").val() || "";
+                if (imagemOcorrenciaAlterada)
+                {
                     imagemFinal = novaImagemOcorrencia;
                 }
 
                 const payload = {
-                    OcorrenciaViagemId: $('#txtId').val(),
+                    OcorrenciaViagemId: $("#txtId").val(),
                     ResumoOcorrencia: resumo,
-                    DescricaoOcorrencia: rteDesc?.value || '',
-                    SolucaoOcorrencia: rteSol?.value || '',
-                    StatusOcorrencia:
-                        $('#chkStatusOcorrencia').val() || 'Aberta',
-                    ImagemOcorrencia: imagemFinal,
+                    DescricaoOcorrencia: rteDesc?.value || "",
+                    SolucaoOcorrencia: rteSol?.value || "",
+                    StatusOcorrencia: $("#chkStatusOcorrencia").val() || "Aberta",
+                    ImagemOcorrencia: imagemFinal
                 };
 
-                $btn.data('busy', true)
-                    .prop('disabled', true)
-                    .html(
-                        '<i class="fa-duotone fa-spinner-third fa-spin me-2"></i> Salvando...',
-                    );
-
-                const response = await fetch(
-                    '/api/OcorrenciaViagem/EditarOcorrencia',
-                    {
-                        method: 'POST',
-                        headers: { 'Content-Type': 'application/json' },
-                        body: JSON.stringify(payload),
-                    },
-                );
+                $btn.data("busy", true).prop("disabled", true).html('<i class="fa-duotone fa-spinner-third fa-spin me-2"></i> Salvando...');
+
+                const response = await fetch("/api/OcorrenciaViagem/EditarOcorrencia", {
+                    method: "POST",
+                    headers: { "Content-Type": "application/json" },
+                    body: JSON.stringify(payload)
+                });
 
                 const data = await response.json();
 
-                if (data.success) {
-                    AppToast.show(
-                        'Verde',
-                        data.message || 'Ocorrência atualizada!',
-                        2000,
-                    );
+                if (data.success)
+                {
+                    AppToast.show("Verde", data.message || "Ocorrência atualizada!", 2000);
                     fecharModalOcorrencia();
                     if (dataTable) dataTable.ajax.reload(null, false);
-                } else {
-                    AppToast.show(
-                        'Vermelho',
-                        data.message || 'Erro ao salvar.',
-                        2000,
-                    );
-                }
-
-                $btn.data('busy', false)
-                    .prop('disabled', false)
-                    .html(
-                        '<i class="fa-duotone fa-floppy-disk me-1" style="color:#fff;"></i> Salvar Alterações',
-                    );
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ocorrencias.js',
-                    'btnSalvar.click',
-                    error,
-                );
-                $('#btnEditarOcorrencia')
-                    .data('busy', false)
-                    .prop('disabled', false)
-                    .html(
-                        '<i class="fa-duotone fa-floppy-disk me-1" style="color:#fff;"></i> Salvar Alterações',
-                    );
-            }
-        });
-
-        $('#inputImagemOcorrencia').on('change', function (e) {
-            try {
+                }
+                else
+                {
+                    AppToast.show("Vermelho", data.message || "Erro ao salvar.", 2000);
+                }
+
+                $btn.data("busy", false).prop("disabled", false).html('<i class="fa-duotone fa-floppy-disk me-1" style="color:#fff;"></i> Salvar Alterações');
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("ocorrencias.js", "btnSalvar.click", error);
+                $("#btnEditarOcorrencia").data("busy", false).prop("disabled", false).html('<i class="fa-duotone fa-floppy-disk me-1" style="color:#fff;"></i> Salvar Alterações');
+            }
+        });
+
+        $("#inputImagemOcorrencia").on("change", function (e)
+        {
+            try
+            {
                 const file = e.target.files[0];
                 if (!file) return;
 
-                const tiposPermitidos = [
-                    'image/jpeg',
-                    'image/png',
-                    'image/gif',
-                    'image/webp',
-                    'video/mp4',
-                    'video/webm',
-                ];
-                if (!tiposPermitidos.includes(file.type)) {
-                    Alerta.Erro(
-                        'Tipo Inválido',
-                        'Selecione uma imagem (JPG, PNG, GIF, WebP) ou vídeo (MP4, WebM).',
-                        'OK',
-                    );
+                const tiposPermitidos = ["image/jpeg", "image/png", "image/gif", "image/webp", "video/mp4", "video/webm"];
+                if (!tiposPermitidos.includes(file.type))
+                {
+                    Alerta.Erro("Tipo Inválido", "Selecione uma imagem (JPG, PNG, GIF, WebP) ou vídeo (MP4, WebM).", "OK");
                     return;
                 }
 
-                if (file.size > 50 * 1024 * 1024) {
-                    Alerta.Erro(
-                        'Arquivo muito grande',
-                        'O arquivo deve ter no máximo 50MB.',
-                        'OK',
-                    );
+                if (file.size > 50 * 1024 * 1024)
+                {
+                    Alerta.Erro("Arquivo muito grande", "O arquivo deve ter no máximo 50MB.", "OK");
                     return;
                 }
 
                 uploadImagemOcorrencia(file);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ocorrencias.js',
-                    'inputImagem.change',
-                    error,
-                );
-            }
-        });
-
-        $('#modalOcorrencia').on('hidden.bs.modal', function () {
-            try {
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("ocorrencias.js", "inputImagem.change", error);
+            }
+        });
+
+        $("#modalOcorrencia").on("hidden.bs.modal", function ()
+        {
+            try
+            {
                 limparModal();
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ocorrencias.js',
-                    'modal.hidden',
-                    error,
-                );
-            }
-        });
-
-        $('#modalOcorrencia').on('shown.bs.modal', function () {
-            try {
-                document
-                    .getElementById('rteOcorrencias')
-                    ?.ej2_instances?.[0]?.refreshUI();
-                document
-                    .getElementById('rteSolucao')
-                    ?.ej2_instances?.[0]?.refreshUI();
-            } catch (_) {}
-        });
-
-        console.log('[ocorrencias.js] Inicialização concluída');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ocorrencias.js',
-            'document.ready',
-            error,
-        );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("ocorrencias.js", "modal.hidden", error);
+            }
+        });
+
+        $("#modalOcorrencia").on("shown.bs.modal", function ()
+        {
+            try
+            {
+                document.getElementById("rteOcorrencias")?.ej2_instances?.[0]?.refreshUI();
+                document.getElementById("rteSolucao")?.ej2_instances?.[0]?.refreshUI();
+            }
+            catch (_) { }
+        });
+
+        console.log("[ocorrencias.js] Inicialização concluída");
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ocorrencias.js", "document.ready", error);
     }
 });
 
-try {
-    if (typeof ej !== 'undefined' && ej.base && ej.base.L10n) {
+try
+{
+    if (typeof ej !== "undefined" && ej.base && ej.base.L10n)
+    {
         ej.base.L10n.load({
-            'pt-BR': {
+            "pt-BR": {
                 richtexteditor: {
-                    alignments: 'Alinhamentos',
-                    justifyLeft: 'Alinhar à Esquerda',
-                    justifyCenter: 'Centralizar',
-                    justifyRight: 'Alinhar à Direita',
-                    justifyFull: 'Justificar',
-                    fontName: 'Fonte',
-                    fontSize: 'Tamanho',
-                    fontColor: 'Cor da Fonte',
-                    backgroundColor: 'Cor de Fundo',
-                    bold: 'Negrito',
-                    italic: 'Itálico',
-                    underline: 'Sublinhado',
-                    strikethrough: 'Tachado',
-                    clearFormat: 'Limpar Formatação',
-                    cut: 'Cortar',
-                    copy: 'Copiar',
-                    paste: 'Colar',
-                    unorderedList: 'Lista',
-                    orderedList: 'Lista Numerada',
-                    indent: 'Aumentar Recuo',
-                    outdent: 'Diminuir Recuo',
-                    undo: 'Desfazer',
-                    redo: 'Refazer',
-                    createLink: 'Inserir Link',
-                    image: 'Inserir Imagem',
-                    fullscreen: 'Maximizar',
-                    formats: 'Formatos',
-                    sourcecode: 'Código Fonte',
-                },
-            },
-        });
-    }
-} catch (error) {
-    console.warn('Erro ao carregar localização RTE:', error);
-}
+                    alignments: "Alinhamentos", justifyLeft: "Alinhar à Esquerda", justifyCenter: "Centralizar",
+                    justifyRight: "Alinhar à Direita", justifyFull: "Justificar", fontName: "Fonte",
+                    fontSize: "Tamanho", fontColor: "Cor da Fonte", backgroundColor: "Cor de Fundo",
+                    bold: "Negrito", italic: "Itálico", underline: "Sublinhado", strikethrough: "Tachado",
+                    clearFormat: "Limpar Formatação", cut: "Cortar", copy: "Copiar", paste: "Colar",
+                    unorderedList: "Lista", orderedList: "Lista Numerada", indent: "Aumentar Recuo",
+                    outdent: "Diminuir Recuo", undo: "Desfazer", redo: "Refazer",
+                    createLink: "Inserir Link", image: "Inserir Imagem", fullscreen: "Maximizar",
+                    formats: "Formatos", sourcecode: "Código Fonte"
+                }
+            }
+        });
+    }
+}
+catch (error)
+{
+    console.warn("Erro ao carregar localização RTE:", error);
+}
```
