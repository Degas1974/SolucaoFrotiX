# wwwroot/js/cadastros/autuacao.js

**Mudanca:** GRANDE | **+176** linhas | **-232** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/autuacao.js
+++ ATUAL: wwwroot/js/cadastros/autuacao.js
@@ -1,11 +1,12 @@
 function lstOrgaoChange() {
-    try {
-        document.getElementById('lstEmpenhos').ej2_instances[0].dataSource = [];
-        document.getElementById('lstEmpenhos').ej2_instances[0].dataBind();
-        document.getElementById('lstEmpenhos').ej2_instances[0].text = '';
-        $('#txtEmpenhoMultaId').attr('value', '');
-
-        var lstOrgao = document.getElementById('lstOrgao').ej2_instances[0];
+    try
+    {
+        document.getElementById("lstEmpenhos").ej2_instances[0].dataSource = [];
+        document.getElementById("lstEmpenhos").ej2_instances[0].dataBind();
+        document.getElementById("lstEmpenhos").ej2_instances[0].text = "";
+        $("#txtEmpenhoMultaId").attr("value", "");
+
+        var lstOrgao = document.getElementById("lstOrgao").ej2_instances[0];
         console.log(lstOrgao.value);
 
         if (lstOrgao.value === null) {
@@ -15,30 +16,26 @@
         var orgaoid = String(lstOrgao.value);
 
         $.ajax({
-            url: '/Multa/UpsertPenalidade?handler=AJAXPreencheListaEmpenhos',
-            method: 'GET',
-            datatype: 'json',
+            url: "/Multa/UpsertPenalidade?handler=AJAXPreencheListaEmpenhos",
+            method: "GET",
+            datatype: "json",
 
             data: { id: orgaoid },
 
             success: function (res) {
-                try {
+                try
+                {
                     if (res.data.length != 0) {
                         var empenhomultaid = res.data[0].empenhoMultaId;
                         var notaempenho = res.data[0].notaEmpenho;
 
                         let EmpenhoList = [
-                            {
-                                EmpenhoMultaId: empenhomultaid,
-                                NotaEmpenho: notaempenho,
-                            },
+                            { EmpenhoMultaId: empenhomultaid, NotaEmpenho: notaempenho },
                         ];
 
                         for (var i = 1; i < res.data.length; ++i) {
                             console.log(
-                                res.data[i].empenhoMultaId +
-                                    ' - ' +
-                                    res.data[i].notaEmpenho,
+                                res.data[i].empenhoMultaId + " - " + res.data[i].notaEmpenho,
                             );
 
                             empenhomultaid = res.data[i].empenhoMultaId;
@@ -51,290 +48,237 @@
                             EmpenhoList.push(empenho);
                         }
 
-                        document.getElementById(
-                            'lstEmpenhos',
-                        ).ej2_instances[0].dataSource = EmpenhoList;
-                        document
-                            .getElementById('lstEmpenhos')
-                            .ej2_instances[0].dataBind();
+                        document.getElementById("lstEmpenhos").ej2_instances[0].dataSource =
+                            EmpenhoList;
+                        document.getElementById("lstEmpenhos").ej2_instances[0].dataBind();
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'autuacao_<num>.js',
-                        'success',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("autuacao_<num>.js", "lstOrgaoChange.success", error);
                 }
             },
         });
 
-        document.getElementById('lstEmpenhos').ej2_instances[0].refresh();
-
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'autuacao_<num>.js',
-            'lstOrgaoChange',
-            error,
-        );
+        document.getElementById("lstEmpenhos").ej2_instances[0].refresh();
+
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("autuacao_<num>.js", "lstOrgaoChange", error);
     }
 }
 
 function lstEmpenhosChange() {
-    try {
-        var lstEmpenhos =
-            document.getElementById('lstEmpenhos').ej2_instances[0];
-        $('#txtEmpenhoMultaId').attr('value', lstEmpenhos.value);
+    try
+    {
+        var lstEmpenhos = document.getElementById("lstEmpenhos").ej2_instances[0];
+        $("#txtEmpenhoMultaId").attr("value", lstEmpenhos.value);
 
         var empenhoid = String(lstEmpenhos.value);
 
         $.ajax({
-            url: '/Multa/UpsertAutuacao?handler=PegaSaldoEmpenho',
-            method: 'GET',
-            datatype: 'json',
+            url: "/Multa/UpsertAutuacao?handler=PegaSaldoEmpenho",
+            method: "GET",
+            datatype: "json",
 
             data: { id: empenhoid },
 
             success: function (res) {
-                try {
+                try
+                {
 
                     var saldoempenho = res.data;
 
-                    $('#txtSaldoEmpenho').val(
-                        Intl.NumberFormat('pt-BR', {
-                            style: 'currency',
-                            currency: 'BRL',
-                        }).format(saldoempenho),
+                    $("#txtSaldoEmpenho").val(
+                        Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" }).format(
+                            saldoempenho,
+                        ),
                     );
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'autuacao_<num>.js',
-                        'success',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("autuacao_<num>.js", "success", error);
                 }
             },
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'autuacao_<num>.js',
-            'lstEmpenhosChange',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("autuacao_<num>.js", "lstEmpenhosChange", error);
     }
 }
 
-$(document).ready(function () {
-    try {
-        if (multaId != '00000000-0000-0000-0000-000000000000') {
-            document.getElementById('lstInfracao').ej2_instances[0].value = [
-                '@Model.MultaObj.Multa.TipoMultaId',
-            ];
-            $('#txtNoFichaVistoria').val(
-                '@Model.MultaObj.Multa.NoFichaVistoria',
-            );
-
-            if ('@Model.MultaObj.Multa.AutuacaoPDF') {
-                createPdfViewer(
-                    '/DadosEditaveis/Multas/' +
-                        encodeURIComponent('@Model.MultaObj.Multa.AutuacaoPDF'),
-                );
-            }
-            if (
-                !'@Model.MultaObj.Multa.ValorAteVencimento' ||
-                '@Model.MultaObj.Multa.ValorAteVencimento' == 0
-            ) {
-                $('#txtValorAteVencimento').val('0,00');
-            }
-            if (
-                !'@Model.MultaObj.Multa.ValorPosVencimento' ||
-                '@Model.MultaObj.Multa.ValorPosVencimento' == 0
-            ) {
-                $('#txtValorPosVencimento').val('0,00');
-            }
-
-            if (typeof lstEmpenhosChange === 'function') lstEmpenhosChange();
-        } else {
-            document.getElementById(
-                'lstContratoVeiculo',
-            ).ej2_instances[0].text = '';
-            document.getElementById(
-                'lstContratoMotorista',
-            ).ej2_instances[0].text = '';
-            document.getElementById('lstOrgao').ej2_instances[0].text = '';
-            document.getElementById('lstEmpenhos').ej2_instances[0].text = '';
-            document.getElementById('lstVeiculo').ej2_instances[0].text = '';
-            document.getElementById('lstAtaVeiculo').ej2_instances[0].text = '';
-            document.getElementById('lstMotorista').ej2_instances[0].text = '';
-            $('#txtValorAteVencimento').val('0,00');
-            $('#txtValorPosVencimento').val('0,00');
+$(document).ready(function ()
+{
+    try
+    {
+        if (multaId != '00000000-0000-0000-0000-000000000000')
+        {
+            document.getElementById("lstInfracao").ej2_instances[0].value = ['@Model.MultaObj.Multa.TipoMultaId'];
+            $('#txtNoFichaVistoria').val('@Model.MultaObj.Multa.NoFichaVistoria');
+
+            if ('@Model.MultaObj.Multa.AutuacaoPDF')
+            {
+                createPdfViewer("/DadosEditaveis/Multas/" + encodeURIComponent('@Model.MultaObj.Multa.AutuacaoPDF'));
+            }
+            if (!('@Model.MultaObj.Multa.ValorAteVencimento') || '@Model.MultaObj.Multa.ValorAteVencimento' == 0) { $('#txtValorAteVencimento').val("0,00"); }
+            if (!('@Model.MultaObj.Multa.ValorPosVencimento') || '@Model.MultaObj.Multa.ValorPosVencimento' == 0) { $('#txtValorPosVencimento').val("0,00"); }
+
+            if (typeof lstEmpenhosChange === "function") lstEmpenhosChange();
+        } else
+        {
+            document.getElementById("lstContratoVeiculo").ej2_instances[0].text = "";
+            document.getElementById("lstContratoMotorista").ej2_instances[0].text = "";
+            document.getElementById("lstOrgao").ej2_instances[0].text = "";
+            document.getElementById("lstEmpenhos").ej2_instances[0].text = "";
+            document.getElementById("lstVeiculo").ej2_instances[0].text = "";
+            document.getElementById("lstAtaVeiculo").ej2_instances[0].text = "";
+            document.getElementById("lstMotorista").ej2_instances[0].text = "";
+            $('#txtValorAteVencimento').val("0,00");
+            $('#txtValorPosVencimento').val("0,00");
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'UpsertAutuacao.cshtml',
-            'document.ready',
-            error,
-        );
-    }
-});
-
-$(document).on('click', '.btnViagem', function () {
-    if (!$('#txtDataInfracao').val()) {
-        Alerta.Warning(
-            'Informação Ausente',
-            'A Data da Infração deve ser informada',
-        );
-        return;
-    }
-
-    if (!$('#txtHoraInfracao').val()) {
-        Alerta.Warning(
-            'Informação Ausente',
-            'A Hora da Infração é obrigatória',
-        );
-        return;
-    }
-
-    const lstVeiculo = document.getElementById('lstVeiculo').ej2_instances[0];
-    if (lstVeiculo.value == null) {
-        Alerta.Warning('Informação Ausente', 'O Veículo deve ser informado');
-        return;
-    }
-
-    var dataToPost = JSON.stringify({
-        VeiculoId: lstVeiculo.value,
-        Data: $('#txtDataInfracao').val(),
-        Hora: $('#txtHoraInfracao').val(),
-    });
+    } catch (error) { Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml", "document.ready", error); }
+});
+
+$(document).on('click', '.btnViagem', function ()
+{
+    if (!$('#txtDataInfracao').val())
+    {
+        Alerta.Warning("Informação Ausente", "A Data da Infração deve ser informada")
+        return;
+    }
+
+    if (!$('#txtHoraInfracao').val())
+    {
+        Alerta.Warning("Informação Ausente", "A Hora da Infração é obrigatória")
+        return;
+    }
+
+    const lstVeiculo = document.getElementById("lstVeiculo").ej2_instances[0];
+    if (lstVeiculo.value == null)
+    {
+        Alerta.Warning("Informação Ausente", "O Veículo deve ser informado")
+        return;
+    }
+
+    var dataToPost = JSON.stringify({ VeiculoId: lstVeiculo.value, Data: $('#txtDataInfracao').val(), Hora: $('#txtHoraInfracao').val() });
     $.ajax({
         url: '/api/Multa/ProcuraViagem',
-        type: 'POST',
+        type: "POST",
         data: dataToPost,
-        contentType: 'application/json; charset=utf-8',
-        dataType: 'json',
-        success: function (data) {
-            try {
-                if (data.success) {
+        contentType: "application/json; charset=utf-8",
+        dataType: "json",
+        success: function (data)
+        {
+            try
+            {
+                if (data.success)
+                {
                     AppToast.show('Verde', data.message);
                     $('#txtNoFichaVistoria').val(data.nofichavistoria);
                     $('#txtNoFichaVistoriaEscondido').val(data.nofichavistoria);
                     EscolhendoMotorista = true;
-                    document.getElementById(
-                        'lstMotorista',
-                    ).ej2_instances[0].value = data.motoristaid;
-                } else {
+                    document.getElementById("lstMotorista").ej2_instances[0].value = data.motoristaid;
+                } else
+                {
                     $('#txtNoFichaVistoria').val('');
                     $('#txtNoFichaVistoriaEscondido').val('');
                     AppToast.show('Vermelho', data.message);
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'UpsertAutuacao.cshtml',
-                    'ProcuraViagem.success',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml", "ProcuraViagem.success", error);
             }
         },
     });
 });
 
 var ViagemId = '';
-$(document).on('click', '.btnFicha', function () {
-    if (!$('#txtNoFichaVistoria').val()) {
-        Alerta.Warning(
-            'Informação Ausente',
-            'Nenhuma Ficha de Vistoria Localizada',
-        );
-        return;
-    }
-
-    var dataToPost = JSON.stringify({
-        NoFichaVistoria: $('#txtNoFichaVistoria').val(),
-    });
+$(document).on('click', '.btnFicha', function ()
+{
+    if (!$('#txtNoFichaVistoria').val())
+    {
+        Alerta.Warning("Informação Ausente", "Nenhuma Ficha de Vistoria Localizada")
+        return;
+    }
+
+    var dataToPost = JSON.stringify({ NoFichaVistoria: $('#txtNoFichaVistoria').val() });
     $.ajax({
         url: '/api/Multa/ProcuraFicha',
-        type: 'POST',
+        type: "POST",
         data: dataToPost,
-        contentType: 'application/json; charset=utf-8',
-        dataType: 'json',
-        success: function (data) {
-            try {
-                if (data.success) {
+        contentType: "application/json; charset=utf-8",
+        dataType: "json",
+        success: function (data)
+        {
+            try
+            {
+                if (data.success)
+                {
                     ViagemId = data.viagemid;
                     AppToast.show('Verde', data.message);
                     modalFicha.show();
-                } else {
+                }
+                else
+                {
                     AppToast.show('Vermelho', data.message);
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'UpsertAutuacao.cshtml',
-                    'ProcuraFicha.success',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml", "ProcuraFicha.success", error);
             }
         },
-        error: function (err) {
+        error: function (err)
+        {
             console.log(err);
             alert('something went wrong');
-        },
+        }
     });
 });
 
 const modalFicha = new bootstrap.Modal(document.getElementById('modalFicha'), {
     keyboard: true,
-    backdrop: 'static',
-});
-
-document
-    .getElementById('modalFicha')
-    .addEventListener('show.bs.modal', function () {
-        try {
-            $.ajaxSetup({ async: false });
-            $.ajax({
-                type: 'get',
-                url: '/api/Viagem/PegaFichaModal',
-                data: { id: ViagemId },
-                success: function (res) {
-                    const fv = $('#txtNoFichaVistoria').val();
-                    $('#imgViewer').removeAttr('src');
-                    if (res === false) {
-                        $('#DynamicModalLabel').html(
-                            'Infração sem Autuação digitalizada',
-                        );
-                        $('#imgViewer').attr(
-                            'src',
-                            '/Images/FichaAmarelaNova.jpg',
-                        );
-                    } else {
-                        $('#DynamicModalLabel').html(
-                            'Ficha de Vistoria Nº: <b>' + fv + '</b>',
-                        );
-                        $('#imgViewer').attr(
-                            'src',
-                            'data:image/jpg;base64,' + res,
-                        );
-                    }
-                },
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'UpsertAutuacao.cshtml',
-                'modalFicha.show',
-                error,
-            );
-        }
-    });
-
-document
-    .getElementById('modalFicha')
-    .addEventListener('hide.bs.modal', function () {
-        try {
-            $('#imgViewer').removeAttr('src');
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'UpsertAutuacao.cshtml',
-                'modalFicha.hide',
-                error,
-            );
-        }
-    });
+    backdrop: 'static'
+});
+
+document.getElementById('modalFicha').addEventListener('show.bs.modal', function ()
+{
+    try
+    {
+        $.ajaxSetup({ async: false });
+        $.ajax({
+            type: "get",
+            url: "/api/Viagem/PegaFichaModal",
+            data: { id: ViagemId },
+            success: function (res)
+            {
+                const fv = $('#txtNoFichaVistoria').val();
+                $('#imgViewer').removeAttr("src");
+                if (res === false)
+                {
+                    $("#DynamicModalLabel").html("Infração sem Autuação digitalizada");
+                    $('#imgViewer').attr('src', "/Images/FichaAmarelaNova.jpg");
+                } else
+                {
+                    $("#DynamicModalLabel").html("Ficha de Vistoria Nº: <b>" + fv + "</b>");
+                    $('#imgViewer').attr('src', "data:image/jpg;base64," + res);
+                }
+            }
+        });
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml", "modalFicha.show", error);
+    }
+});
+
+document.getElementById('modalFicha').addEventListener('hide.bs.modal', function ()
+{
+    try
+    {
+        $('#imgViewer').removeAttr("src");
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("UpsertAutuacao.cshtml", "modalFicha.hide", error);
+    }
+});
```
