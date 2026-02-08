# wwwroot/js/cadastros/insereviagem_001.js

**Mudanca:** GRANDE | **+211** linhas | **-241** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/insereviagem_001.js
+++ ATUAL: wwwroot/js/cadastros/insereviagem_001.js
@@ -1,331 +1,311 @@
-$(document).ready(function () {
-    try {
-
-        if (
-            window.SyncfusionTooltips &&
-            typeof window.SyncfusionTooltips.init === 'function'
-        ) {
+$(document).ready(function ()
+{
+    try
+    {
+
+        if (window.SyncfusionTooltips && typeof window.SyncfusionTooltips.init === 'function')
+        {
             window.SyncfusionTooltips.init();
         }
 
-        $('#txtNoFichaVistoria').focusout(function () {
-            try {
+        $("#txtNoFichaVistoria").focusout(function ()
+        {
+            try
+            {
                 const noFichaVistoria = parseInt($(this).val());
 
-                if (!noFichaVistoria || noFichaVistoria === 0) {
+                if (!noFichaVistoria || noFichaVistoria === 0)
+                {
                     return;
                 }
 
                 $.ajax({
-                    url: '/Viagens/Upsert?handler=MaxFicha',
-                    method: 'GET',
-                    datatype: 'json',
-                    success: function (res) {
-                        try {
+                    url: "/Viagens/Upsert?handler=MaxFicha",
+                    method: "GET",
+                    datatype: "json",
+                    success: function (res)
+                    {
+                        try
+                        {
                             const maxFichaVistoria = parseInt(res.data);
 
-                            if (noFichaVistoria > maxFichaVistoria + 100) {
+                            if (noFichaVistoria > (maxFichaVistoria + 100))
+                            {
                                 Alerta.Warning(
                                     'Alerta na Ficha de Vistoria',
-                                    'O número inserido difere em +100 da última Ficha inserida!',
+                                    'O número inserido difere em +100 da última Ficha inserida!'
                                 );
                                 return;
                             }
 
-                            if (noFichaVistoria < maxFichaVistoria - 100) {
+                            if (noFichaVistoria < (maxFichaVistoria - 100))
+                            {
                                 Alerta.Warning(
                                     'Alerta na Ficha de Vistoria',
-                                    'O número inserido difere em -100 da última Ficha inserida!',
+                                    'O número inserido difere em -100 da última Ficha inserida!'
                                 );
                                 return;
                             }
-                        } catch (innerError) {
-                            Alerta.TratamentoErroComLinha(
-                                'insereviagem.js',
-                                'txtNoFichaVistoria.MaxFicha.success',
-                                innerError,
-                            );
+
+                        } catch (innerError)
+                        {
+                            Alerta.TratamentoErroComLinha("insereviagem.js", "txtNoFichaVistoria.MaxFicha.success", innerError);
                         }
                     },
-                    error: function (xhr, status, error) {
-                        Alerta.TratamentoErroComLinha(
-                            'insereviagem.js',
-                            'txtNoFichaVistoria.MaxFicha.error',
-                            new Error(error),
-                        );
-                    },
+                    error: function (xhr, status, error)
+                    {
+                        Alerta.TratamentoErroComLinha("insereviagem.js", "txtNoFichaVistoria.MaxFicha.error", new Error(error));
+                    }
                 });
 
                 $.ajax({
-                    url: '/Viagens/Upsert?handler=FichaExistente',
-                    method: 'GET',
-                    datatype: 'json',
+                    url: "/Viagens/Upsert?handler=FichaExistente",
+                    method: "GET",
+                    datatype: "json",
                     data: { id: noFichaVistoria },
-                    success: function (res) {
-                        try {
+                    success: function (res)
+                    {
+                        try
+                        {
                             const existeFicha = res.data;
 
-                            if (existeFicha === true) {
+                            if (existeFicha === true)
+                            {
                                 Alerta.Warning(
                                     'Alerta na Ficha de Vistoria',
-                                    'Já existe uma Ficha inserida com esta numeração!',
+                                    'Já existe uma Ficha inserida com esta numeração!'
                                 );
-                                $('#txtNoFichaVistoria').val('');
-                                $('#txtNoFichaVistoria').focus();
+                                $("#txtNoFichaVistoria").val('');
+                                $("#txtNoFichaVistoria").focus();
                             }
-                        } catch (innerError) {
-                            Alerta.TratamentoErroComLinha(
-                                'insereviagem.js',
-                                'txtNoFichaVistoria.FichaExistente.success',
-                                innerError,
-                            );
+
+                        } catch (innerError)
+                        {
+                            Alerta.TratamentoErroComLinha("insereviagem.js", "txtNoFichaVistoria.FichaExistente.success", innerError);
                         }
                     },
-                    error: function (xhr, status, error) {
-                        Alerta.TratamentoErroComLinha(
-                            'insereviagem.js',
-                            'txtNoFichaVistoria.FichaExistente.error',
-                            new Error(error),
-                        );
-                    },
+                    error: function (xhr, status, error)
+                    {
+                        Alerta.TratamentoErroComLinha("insereviagem.js", "txtNoFichaVistoria.FichaExistente.error", new Error(error));
+                    }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'insereviagem.js',
-                    'txtNoFichaVistoria.focusout',
-                    error,
-                );
-            }
-        });
-
-        $('#txtDataFinal').focusout(function () {
-            try {
-                const dataInicial = $('#txtDataInicial').val();
-                const dataFinal = $('#txtDataFinal').val();
-
-                if (dataFinal === '') {
-                    return;
-                }
-
-                if (dataFinal < dataInicial) {
-                    $('#txtDataFinal').val('');
+
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("insereviagem.js", "txtNoFichaVistoria.focusout", error);
+            }
+        });
+
+        $("#txtDataFinal").focusout(function ()
+        {
+            try
+            {
+                const dataInicial = $("#txtDataInicial").val();
+                const dataFinal = $("#txtDataFinal").val();
+
+                if (dataFinal === '')
+                {
+                    return;
+                }
+
+                if (dataFinal < dataInicial)
+                {
+                    $("#txtDataFinal").val('');
                     Alerta.Warning(
                         'Erro na Data',
-                        'A data final deve ser maior que a inicial!',
+                        'A data final deve ser maior que a inicial!'
                     );
-                    $('#txtDataFinal').focus();
-                }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'insereviagem.js',
-                    'txtDataFinal.focusout',
-                    error,
-                );
-            }
-        });
-
-        $('#txtHoraFinal').focusout(function () {
-            try {
-                const horaInicial = $('#txtHoraInicial').val();
-                const horaFinal = $('#txtHoraFinal').val();
-
-                if (horaFinal === '') {
-                    return;
-                }
-
-                if (horaFinal <= horaInicial) {
-                    $('#txtHoraFinal').val('');
+                    $("#txtDataFinal").focus();
+                }
+
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("insereviagem.js", "txtDataFinal.focusout", error);
+            }
+        });
+
+        $("#txtHoraFinal").focusout(function ()
+        {
+            try
+            {
+                const horaInicial = $("#txtHoraInicial").val();
+                const horaFinal = $("#txtHoraFinal").val();
+
+                if (horaFinal === '')
+                {
+                    return;
+                }
+
+                if (horaFinal <= horaInicial)
+                {
+                    $("#txtHoraFinal").val('');
                     Alerta.Warning(
                         'Erro no Horário',
-                        'O horário final deve ser maior que o inicial!',
+                        'O horário final deve ser maior que o inicial!'
                     );
-                    $('#txtHoraFinal').focus();
-                }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'insereviagem.js',
-                    'txtHoraFinal.focusout',
-                    error,
-                );
-            }
-        });
-
-        $('#txtKmFinal').focusout(function () {
-            try {
-                const kmInicial = parseFloat($('#txtKmInicial').val());
-                const kmFinal = parseFloat($('#txtKmFinal').val());
-
-                if (!kmFinal || kmFinal === 0) {
-                    return;
-                }
-
-                if (kmFinal <= kmInicial) {
-                    $('#txtKmFinal').val('');
+                    $("#txtHoraFinal").focus();
+                }
+
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("insereviagem.js", "txtHoraFinal.focusout", error);
+            }
+        });
+
+        $("#txtKmFinal").focusout(function ()
+        {
+            try
+            {
+                const kmInicial = parseFloat($("#txtKmInicial").val());
+                const kmFinal = parseFloat($("#txtKmFinal").val());
+
+                if (!kmFinal || kmFinal === 0)
+                {
+                    return;
+                }
+
+                if (kmFinal <= kmInicial)
+                {
+                    $("#txtKmFinal").val('');
                     Alerta.Warning(
                         'Erro na Quilometragem',
-                        'A quilometragem final deve ser maior que a inicial!',
+                        'A quilometragem final deve ser maior que a inicial!'
                     );
-                    $('#txtKmFinal').focus();
-                }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'insereviagem.js',
-                    'txtKmFinal.focusout',
-                    error,
-                );
-            }
-        });
-
-        $('#lstVeiculo').change(function () {
-            try {
+                    $("#txtKmFinal").focus();
+                }
+
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("insereviagem.js", "txtKmFinal.focusout", error);
+            }
+        });
+
+        $("#lstVeiculo").change(function ()
+        {
+            try
+            {
                 const veiculoId = $(this).val();
 
-                if (!veiculoId) {
-                    $('#txtKmInicial').val('');
+                if (!veiculoId)
+                {
+                    $("#txtKmInicial").val('');
                     return;
                 }
 
                 $.ajax({
-                    url: '/api/Viagem/UltimaQuilometragem',
-                    method: 'GET',
+                    url: "/api/Viagem/UltimaQuilometragem",
+                    method: "GET",
                     data: { veiculoId: veiculoId },
-                    success: function (res) {
-                        try {
-                            if (res.success && res.data) {
-                                $('#txtKmInicial').val(res.data.quilometragem);
-                                AppToast.show(
-                                    'Verde',
-                                    `Última quilometragem: ${res.data.quilometragem} km`,
-                                    3000,
-                                );
+                    success: function (res)
+                    {
+                        try
+                        {
+                            if (res.success && res.data)
+                            {
+                                $("#txtKmInicial").val(res.data.quilometragem);
+                                AppToast.show('Verde', `Última quilometragem: ${res.data.quilometragem} km`, 3000);
                             }
-                        } catch (innerError) {
-                            Alerta.TratamentoErroComLinha(
-                                'insereviagem.js',
-                                'lstVeiculo.UltimaQuilometragem.success',
-                                innerError,
-                            );
+                        } catch (innerError)
+                        {
+                            Alerta.TratamentoErroComLinha("insereviagem.js", "lstVeiculo.UltimaQuilometragem.success", innerError);
                         }
                     },
-                    error: function (xhr, status, error) {
-                        Alerta.TratamentoErroComLinha(
-                            'insereviagem.js',
-                            'lstVeiculo.UltimaQuilometragem.error',
-                            new Error(error),
-                        );
-                    },
+                    error: function (xhr, status, error)
+                    {
+                        Alerta.TratamentoErroComLinha("insereviagem.js", "lstVeiculo.UltimaQuilometragem.error", new Error(error));
+                    }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'insereviagem.js',
-                    'lstVeiculo.change',
-                    error,
-                );
-            }
-        });
-
-        $('#txtLitrosAbastecidos').focusout(function () {
-            try {
+
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("insereviagem.js", "lstVeiculo.change", error);
+            }
+        });
+
+        $("#txtLitrosAbastecidos").focusout(function ()
+        {
+            try
+            {
                 const litros = parseFloat($(this).val());
 
-                if (!litros || litros === 0) {
-                    return;
-                }
-
-                if (litros > 200) {
+                if (!litros || litros === 0)
+                {
+                    return;
+                }
+
+                if (litros > 200)
+                {
                     Alerta.Warning(
                         'Atenção no Abastecimento',
-                        'Quantidade de litros abastecidos parece estar muito alta. Verifique!',
+                        'Quantidade de litros abastecidos parece estar muito alta. Verifique!'
                     );
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'insereviagem.js',
-                    'txtLitrosAbastecidos.focusout',
-                    error,
-                );
-            }
-        });
-
-        $('#btnSalvar').click(function (e) {
-            try {
+
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("insereviagem.js", "txtLitrosAbastecidos.focusout", error);
+            }
+        });
+
+        $("#btnSalvar").click(function (e)
+        {
+            try
+            {
                 e.preventDefault();
 
-                if (!$('#txtDataInicial').val()) {
-                    Alerta.Warning(
-                        'Informação Ausente',
-                        'A data inicial da viagem é obrigatória',
-                    );
-                    $('#txtDataInicial').focus();
-                    return false;
-                }
-
-                if (!$('#txtDataFinal').val()) {
-                    Alerta.Warning(
-                        'Informação Ausente',
-                        'A data final da viagem é obrigatória',
-                    );
-                    $('#txtDataFinal').focus();
-                    return false;
-                }
-
-                if (!$('#lstVeiculo').val()) {
-                    Alerta.Warning(
-                        'Informação Ausente',
-                        'O veículo da viagem é obrigatório',
-                    );
-                    $('#lstVeiculo').focus();
-                    return false;
-                }
-
-                if (!$('#lstMotorista').val()) {
-                    Alerta.Warning(
-                        'Informação Ausente',
-                        'O motorista da viagem é obrigatório',
-                    );
-                    $('#lstMotorista').focus();
-                    return false;
-                }
-
-                if (
-                    !$('#txtKmInicial').val() ||
-                    parseFloat($('#txtKmInicial').val()) === 0
-                ) {
-                    Alerta.Warning(
-                        'Informação Ausente',
-                        'A quilometragem inicial é obrigatória',
-                    );
-                    $('#txtKmInicial').focus();
-                    return false;
-                }
-
-                if (
-                    !$('#txtKmFinal').val() ||
-                    parseFloat($('#txtKmFinal').val()) === 0
-                ) {
-                    Alerta.Warning(
-                        'Informação Ausente',
-                        'A quilometragem final é obrigatória',
-                    );
-                    $('#txtKmFinal').focus();
+                if (!$("#txtDataInicial").val())
+                {
+                    Alerta.Warning('Informação Ausente', 'A data inicial da viagem é obrigatória');
+                    $("#txtDataInicial").focus();
+                    return false;
+                }
+
+                if (!$("#txtDataFinal").val())
+                {
+                    Alerta.Warning('Informação Ausente', 'A data final da viagem é obrigatória');
+                    $("#txtDataFinal").focus();
+                    return false;
+                }
+
+                if (!$("#lstVeiculo").val())
+                {
+                    Alerta.Warning('Informação Ausente', 'O veículo da viagem é obrigatório');
+                    $("#lstVeiculo").focus();
+                    return false;
+                }
+
+                if (!$("#lstMotorista").val())
+                {
+                    Alerta.Warning('Informação Ausente', 'O motorista da viagem é obrigatório');
+                    $("#lstMotorista").focus();
+                    return false;
+                }
+
+                if (!$("#txtKmInicial").val() || parseFloat($("#txtKmInicial").val()) === 0)
+                {
+                    Alerta.Warning('Informação Ausente', 'A quilometragem inicial é obrigatória');
+                    $("#txtKmInicial").focus();
+                    return false;
+                }
+
+                if (!$("#txtKmFinal").val() || parseFloat($("#txtKmFinal").val()) === 0)
+                {
+                    Alerta.Warning('Informação Ausente', 'A quilometragem final é obrigatória');
+                    $("#txtKmFinal").focus();
                     return false;
                 }
 
                 AppToast.show('Amarelo', 'Salvando viagem...', 2000);
-                $('#formViagem').submit();
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'insereviagem.js',
-                    'btnSalvar.click',
-                    error,
-                );
-            }
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'insereviagem.js',
-            'document.ready',
-            error,
-        );
+                $("#formViagem").submit();
+
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("insereviagem.js", "btnSalvar.click", error);
+            }
+        });
+
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("insereviagem.js", "document.ready", error);
     }
 });
```
