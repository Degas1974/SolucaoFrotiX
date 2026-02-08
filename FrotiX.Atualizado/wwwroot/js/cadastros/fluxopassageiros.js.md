# wwwroot/js/cadastros/fluxopassageiros.js

**Mudanca:** GRANDE | **+349** linhas | **-391** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/fluxopassageiros.js
+++ ATUAL: wwwroot/js/cadastros/fluxopassageiros.js
@@ -1,359 +1,319 @@
-$(document).ready(function () {
-    try {
-        $('#txtData').on('change', function () {
-            try {
-
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'fluxopassageiros.js',
-                    'txtData.change',
-                    error,
-                );
+$(document).ready(function ()
+{
+    try
+    {
+        $("#txtData").on("change", function ()
+        {
+            try
+            {
+
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("fluxopassageiros.js", "txtData.change", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'fluxopassageiros.js',
-            'document.ready',
-            error,
-        );
-    }
-});
-
-function executarInsercaoLinha() {
-    try {
-        const economildo =
-            document.getElementById('lstVeiculos').ej2_instances[0].value;
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("fluxopassageiros.js", "document.ready", error);
+    }
+});
+
+function executarInsercaoLinha()
+{
+    try
+    {
+        const economildo = document.getElementById('lstVeiculos').ej2_instances[0].value;
         const idaOuVolta = document.getElementById('lstIdaVolta').value;
         const horainicio = document.getElementById('txtHoraInicio').value;
         const horafim = document.getElementById('txtHoraFim').value;
         const qtdpassageirosStr = document.getElementById('txtQtd').value;
         const qtdpassageiros = parseInt(qtdpassageirosStr, 10);
 
-        const horafimanterior =
-            idaOuVolta === 'IDA'
-                ? document.getElementById('txtHoraFimAnteriorIda').value
-                : document.getElementById('txtHoraFimAnteriorVolta').value;
-
-        if (horainicio === '') {
-            Alerta.Erro('Erro!', 'A Hora de Início da viagem é obrigatória.');
-            return;
-        }
-        if (horafim === '') {
-            Alerta.Erro('Erro!', 'A Hora de Fim da viagem é obrigatória.');
-            return;
-        }
-        if (qtdpassageirosStr === '') {
-            Alerta.Erro('Erro!', 'A Quantidade de passageiros é obrigatória.');
-            return;
-        }
-        if (isNaN(qtdpassageiros) || qtdpassageiros < 0) {
-            Alerta.Erro(
-                'Erro!',
-                'A Quantidade de passageiros deve ser zero ou um número positivo.',
-            );
-            return;
-        }
-        if (horainicio > horafim) {
-            Alerta.Erro(
-                'Erro!',
-                'A Hora Inicial está menor do que a Hora Final.',
-            );
-            return;
-        }
-        if (horafimanterior !== '' && horainicio < horafimanterior) {
-            Alerta.Erro(
-                'Erro!',
-                'A Hora Inicial está menor do que a Hora Final anterior.',
-            );
+        const horafimanterior = (idaOuVolta === 'IDA')
+            ? document.getElementById('txtHoraFimAnteriorIda').value
+            : document.getElementById('txtHoraFimAnteriorVolta').value;
+
+        if (horainicio === '')
+        {
+            Alerta.Erro("Erro!", "A Hora de Início da viagem é obrigatória.");
+            return;
+        }
+        if (horafim === '')
+        {
+            Alerta.Erro("Erro!", "A Hora de Fim da viagem é obrigatória.");
+            return;
+        }
+        if (qtdpassageirosStr === '')
+        {
+            Alerta.Erro("Erro!", "A Quantidade de passageiros é obrigatória.");
+            return;
+        }
+        if (isNaN(qtdpassageiros) || qtdpassageiros < 0)
+        {
+            Alerta.Erro("Erro!", "A Quantidade de passageiros deve ser zero ou um número positivo.");
+            return;
+        }
+        if (horainicio > horafim)
+        {
+            Alerta.Erro("Erro!", "A Hora Inicial está menor do que a Hora Final.");
+            return;
+        }
+        if (horafimanterior !== '' && horainicio < horafimanterior)
+        {
+            Alerta.Erro("Erro!", "A Hora Inicial está menor do que a Hora Final anterior.");
             return;
         }
 
         $.ajax({
-            type: 'GET',
-            url: '/api/Viagem/PegaCategoria',
+            type: "GET",
+            url: "/api/Viagem/PegaCategoria",
             data: { id: economildo },
-            success: function (res) {
-                try {
+            success: function (res)
+            {
+                try
+                {
                     const categoria = res || '';
-                    if (categoria === 'Ônibus' && qtdpassageiros > 150) {
-                        Alerta.Erro(
-                            'Erro!',
-                            'A Quantidade de passageiros do Ônibus não pode exceder 150 pessoas.',
+                    if (categoria === 'Ônibus' && qtdpassageiros > 150)
+                    {
+                        Alerta.Erro("Erro!", "A Quantidade de passageiros do Ônibus não pode exceder 150 pessoas.");
+                        return;
+                    }
+                    if (categoria === 'Coletivos Pequenos' && qtdpassageiros > 20)
+                    {
+                        Alerta.Erro("Erro!", "A Quantidade de passageiros dos Coletivos Pequenos não pode exceder 20 pessoas.");
+                        return;
+                    }
+
+                    if (idaOuVolta === 'IDA')
+                    {
+                        const gridIda = document.getElementById('grdIda').ej2_instances[0];
+                        gridIda.addRecord(
+                            { horainicioida: horainicio, horafimida: horafim, qtdpassageirosida: qtdpassageiros },
+                            gridIda.getRows().length + 1
                         );
-                        return;
-                    }
-                    if (
-                        categoria === 'Coletivos Pequenos' &&
-                        qtdpassageiros > 20
-                    ) {
-                        Alerta.Erro(
-                            'Erro!',
-                            'A Quantidade de passageiros dos Coletivos Pequenos não pode exceder 20 pessoas.',
+                        document.getElementById('txtHoraInicioAnteriorIda').value = horainicio;
+                        document.getElementById('txtHoraFimAnteriorIda').value = horafim;
+                        document.getElementById('txtQtdAnteriorIda').value = String(qtdpassageiros);
+                    }
+                    else
+                    {
+                        const gridVolta = document.getElementById('grdVolta').ej2_instances[0];
+                        gridVolta.addRecord(
+                            { horainiciovolta: horainicio, horafimvolta: horafim, qtdpassageirosvolta: qtdpassageiros },
+                            gridVolta.getRows().length + 1
                         );
-                        return;
-                    }
-
-                    if (idaOuVolta === 'IDA') {
-                        const gridIda =
-                            document.getElementById('grdIda').ej2_instances[0];
-                        gridIda.addRecord(
-                            {
-                                horainicioida: horainicio,
-                                horafimida: horafim,
-                                qtdpassageirosida: qtdpassageiros,
-                            },
-                            gridIda.getRows().length + 1,
-                        );
-                        document.getElementById(
-                            'txtHoraInicioAnteriorIda',
-                        ).value = horainicio;
-                        document.getElementById('txtHoraFimAnteriorIda').value =
-                            horafim;
-                        document.getElementById('txtQtdAnteriorIda').value =
-                            String(qtdpassageiros);
-                    } else {
-                        const gridVolta =
-                            document.getElementById('grdVolta')
-                                .ej2_instances[0];
-                        gridVolta.addRecord(
-                            {
-                                horainiciovolta: horainicio,
-                                horafimvolta: horafim,
-                                qtdpassageirosvolta: qtdpassageiros,
-                            },
-                            gridVolta.getRows().length + 1,
-                        );
-                        document.getElementById(
-                            'txtHoraInicioAnteriorVolta',
-                        ).value = horainicio;
-                        document.getElementById(
-                            'txtHoraFimAnteriorVolta',
-                        ).value = horafim;
-                        document.getElementById('txtQtdAnteriorVolta').value =
-                            String(qtdpassageiros);
+                        document.getElementById('txtHoraInicioAnteriorVolta').value = horainicio;
+                        document.getElementById('txtHoraFimAnteriorVolta').value = horafim;
+                        document.getElementById('txtQtdAnteriorVolta').value = String(qtdpassageiros);
                     }
 
                     document.getElementById('txtHoraInicio').value = '';
                     document.getElementById('txtHoraFim').value = '';
                     document.getElementById('txtQtd').value = '';
-                    setTimeout(function () {
-                        try {
+                    setTimeout(function ()
+                    {
+                        try
+                        {
                             $('#txtHoraInicio').focus();
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'fluxopassageiros.js',
-                                'executarInsercaoLinha.focus.setTimeout',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("fluxopassageiros.js", "executarInsercaoLinha.focus.setTimeout", error);
                         }
                     }, 250);
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'fluxopassageiros.js',
-                        'executarInsercaoLinha.ajax.success',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("fluxopassageiros.js", "executarInsercaoLinha.ajax.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                try {
+            error: function (xhr, status, error)
+            {
+                try
+                {
                     console.error('Erro AJAX:', error);
-                    AppToast.show(
-                        'Vermelho',
-                        'Não foi possível obter a categoria do veículo.',
-                        3000,
-                    );
-                    Alerta.Erro(
-                        'Erro',
-                        'Não foi possível obter a categoria do veículo.',
-                    );
-                } catch (err) {
-                    Alerta.TratamentoErroComLinha(
-                        'fluxopassageiros.js',
-                        'executarInsercaoLinha.ajax.error',
-                        err,
-                    );
-                }
-            },
+                    AppToast.show('Vermelho', 'Não foi possível obter a categoria do veículo.', 3000);
+                    Alerta.Erro('Erro', 'Não foi possível obter a categoria do veículo.');
+                }
+                catch (err)
+                {
+                    Alerta.TratamentoErroComLinha("fluxopassageiros.js", "executarInsercaoLinha.ajax.error", err);
+                }
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'fluxopassageiros.js',
-            'executarInsercaoLinha',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("fluxopassageiros.js", "executarInsercaoLinha", error);
     }
 }
 
-document
-    .getElementById('btnInsereFicha')
-    .addEventListener('click', function (e) {
-        e.preventDefault();
-        try {
-            const dataviagem = document.getElementById('txtData').value;
-            const economildo =
-                document.getElementById('lstVeiculos').ej2_instances[0].value;
-            const motorista =
-                document.getElementById('lstMotoristas').ej2_instances[0].value;
-            const mob = document.getElementById('lstMOB').value;
-            const responsavel = document.getElementById('lstResponsavel').value;
-
-            if (dataviagem === '') {
-                AppToast.show(
-                    'Vermelho',
-                    'A Data da viagem é obrigatória.',
-                    2000,
-                );
-                setTimeout(function () {
-                    try {
-                        $('#txtData').focus();
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'fluxopassageiros.js',
-                            'btnInsereFicha.txtData.focus.setTimeout',
-                            error,
-                        );
-                    }
-                }, 500);
-                return;
-            }
-            if (!economildo) {
-                AppToast.show('Vermelho', 'O Veículo é obrigatório.', 2000);
-                setTimeout(function () {
-                    try {
-                        $('#lstVeiculos').focus();
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'fluxopassageiros.js',
-                            'btnInsereFicha.lstVeiculos.focus.setTimeout',
-                            error,
-                        );
-                    }
-                }, 500);
-                return;
-            }
-            if (!motorista) {
-                AppToast.show('Vermelho', 'O Motorista é obrigatório.', 2000);
-                setTimeout(function () {
-                    try {
-                        $('#lstMotoristas').focus();
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'fluxopassageiros.js',
-                            'btnInsereFicha.lstMotoristas.focus.setTimeout',
-                            error,
-                        );
-                    }
-                }, 500);
-                return;
-            }
-            if (mob === '') {
-                AppToast.show('Vermelho', 'O MOB é obrigatório.', 2000);
-                setTimeout(function () {
-                    try {
-                        $('#lstMOB').focus();
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'fluxopassageiros.js',
-                            'btnInsereFicha.lstMOB.focus.setTimeout',
-                            error,
-                        );
-                    }
-                }, 500);
-                return;
-            }
-            if (responsavel === '') {
-                AppToast.show('Vermelho', 'O Responsável é obrigatório.', 2000);
-                setTimeout(function () {
-                    try {
-                        $('#lstResponsavel').focus();
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'fluxopassageiros.js',
-                            'btnInsereFicha.lstResponsavel.focus.setTimeout',
-                            error,
-                        );
-                    }
-                }, 500);
-                return;
-            }
-
-            const btnInsere = document.getElementById('btnInsereFicha');
-            const iconOriginal = btnInsere.querySelector('i');
-            const iconClasses = iconOriginal.className;
-            iconOriginal.className = 'fa-solid fa-spinner fa-spin icon-space';
-            btnInsere.disabled = true;
-
-            document.getElementById('divTitulo').hidden = false;
-            document.getElementById('divInsercao').hidden = false;
-            document.getElementById('divRegistro').hidden = false;
-            document.getElementById('divBotao').hidden = false;
-
-            setTimeout(function () {
-                try {
-                    $('#lstIdaVolta').focus();
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'fluxopassageiros.js',
-                        'btnInsereFicha.lstIdaVolta.focus.setTimeout',
-                        error,
-                    );
+document.getElementById('btnInsereFicha').addEventListener('click', function (e)
+{
+    e.preventDefault();
+    try
+    {
+        const dataviagem = document.getElementById('txtData').value;
+        const economildo = document.getElementById('lstVeiculos').ej2_instances[0].value;
+        const motorista = document.getElementById('lstMotoristas').ej2_instances[0].value;
+        const mob = document.getElementById('lstMOB').value;
+        const responsavel = document.getElementById('lstResponsavel').value;
+
+        if (dataviagem === '')
+        {
+            AppToast.show('Vermelho', 'A Data da viagem é obrigatória.', 2000);
+            setTimeout(function ()
+            {
+                try
+                {
+                    $('#txtData').focus();
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnInsereFicha.txtData.focus.setTimeout", error);
                 }
             }, 500);
-
-            document.getElementById('txtData').disabled = true;
-            document.getElementById('lstVeiculos').ej2_instances[0].enabled =
-                false;
-            document.getElementById('lstMotoristas').ej2_instances[0].enabled =
-                false;
-            document.getElementById('lstMOB').disabled = true;
-            document.getElementById('lstResponsavel').disabled = true;
-            document.getElementById('btnInsereFicha').hidden = true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'fluxopassageiros.js',
-                'btnInsereFicha.click',
-                error,
-            );
-        }
-    });
-
-document.getElementById('btnInsere').addEventListener('click', function (e) {
+            return;
+        }
+        if (!economildo)
+        {
+            AppToast.show('Vermelho', 'O Veículo é obrigatório.', 2000);
+            setTimeout(function ()
+            {
+                try
+                {
+                    $('#lstVeiculos').focus();
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnInsereFicha.lstVeiculos.focus.setTimeout", error);
+                }
+            }, 500);
+            return;
+        }
+        if (!motorista)
+        {
+            AppToast.show('Vermelho', 'O Motorista é obrigatório.', 2000);
+            setTimeout(function ()
+            {
+                try
+                {
+                    $('#lstMotoristas').focus();
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnInsereFicha.lstMotoristas.focus.setTimeout", error);
+                }
+            }, 500);
+            return;
+        }
+        if (mob === '')
+        {
+            AppToast.show('Vermelho', 'O MOB é obrigatório.', 2000);
+            setTimeout(function ()
+            {
+                try
+                {
+                    $('#lstMOB').focus();
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnInsereFicha.lstMOB.focus.setTimeout", error);
+                }
+            }, 500);
+            return;
+        }
+        if (responsavel === '')
+        {
+            AppToast.show('Vermelho', 'O Responsável é obrigatório.', 2000);
+            setTimeout(function ()
+            {
+                try
+                {
+                    $('#lstResponsavel').focus();
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnInsereFicha.lstResponsavel.focus.setTimeout", error);
+                }
+            }, 500);
+            return;
+        }
+
+        const btnInsere = document.getElementById('btnInsereFicha');
+        const iconOriginal = btnInsere.querySelector('i');
+        const iconClasses = iconOriginal.className;
+        iconOriginal.className = 'fa-solid fa-spinner fa-spin icon-space';
+        btnInsere.disabled = true;
+
+        document.getElementById('divTitulo').hidden = false;
+        document.getElementById('divInsercao').hidden = false;
+        document.getElementById('divRegistro').hidden = false;
+        document.getElementById('divBotao').hidden = false;
+
+        setTimeout(function ()
+        {
+            try
+            {
+                $('#lstIdaVolta').focus();
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnInsereFicha.lstIdaVolta.focus.setTimeout", error);
+            }
+        }, 500);
+
+        document.getElementById('txtData').disabled = true;
+        document.getElementById('lstVeiculos').ej2_instances[0].enabled = false;
+        document.getElementById('lstMotoristas').ej2_instances[0].enabled = false;
+        document.getElementById('lstMOB').disabled = true;
+        document.getElementById('lstResponsavel').disabled = true;
+        document.getElementById('btnInsereFicha').hidden = true;
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnInsereFicha.click", error);
+    }
+});
+
+document.getElementById('btnInsere').addEventListener('click', function (e)
+{
     e.preventDefault();
-    try {
+    try
+    {
         executarInsercaoLinha();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'fluxopassageiros.js',
-            'btnInsere.click',
-            error,
-        );
-    }
-});
-
-document.getElementById('txtQtd').addEventListener('keypress', function (e) {
-    try {
-        if (e.key === 'Enter' || e.keyCode === 13) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnInsere.click", error);
+    }
+});
+
+document.getElementById('txtQtd').addEventListener('keypress', function (e)
+{
+    try
+    {
+        if (e.key === 'Enter' || e.keyCode === 13)
+        {
             e.preventDefault();
             executarInsercaoLinha();
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'fluxopassageiros.js',
-            'txtQtd.keypress',
-            error,
-        );
-    }
-});
-
-document.getElementById('btnSubmite').addEventListener('click', function (e) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("fluxopassageiros.js", "txtQtd.keypress", error);
+    }
+});
+
+document.getElementById('btnSubmite').addEventListener('click', function (e)
+{
     e.preventDefault();
-    try {
-        const veiculoId =
-            document.getElementById('lstVeiculos').ej2_instances[0].value;
-        const motoristaId =
-            document.getElementById('lstMotoristas').ej2_instances[0].value;
+    try
+    {
+        const veiculoId = document.getElementById('lstVeiculos').ej2_instances[0].value;
+        const motoristaId = document.getElementById('lstMotoristas').ej2_instances[0].value;
         const data = $('#txtData').val();
         const mob = $('#lstMOB').val();
         const responsavel = $('#lstResponsavel').val();
@@ -361,13 +321,17 @@
         const viagens = [];
 
         const gridIda = document.getElementById('grdIda').ej2_instances[0];
-        if (gridIda && gridIda.getRows().length > 0) {
-            for (let i = 0; i < gridIda.getRows().length; i++) {
-                try {
+        if (gridIda && gridIda.getRows().length > 0)
+        {
+            for (let i = 0; i < gridIda.getRows().length; i++)
+            {
+                try
+                {
                     const c0 = gridIda.getRows()[i].cells[0]?.innerHTML || '';
                     const c1 = gridIda.getRows()[i].cells[1]?.innerHTML || '';
                     const c2 = gridIda.getRows()[i].cells[2]?.innerHTML || '';
-                    if (c0 !== '') {
+                    if (c0 !== '')
+                    {
                         viagens.push({
                             Data: data,
                             IdaVolta: 'IDA',
@@ -377,27 +341,29 @@
                             VeiculoId: veiculoId,
                             MotoristaId: motoristaId,
                             MOB: mob,
-                            Responsavel: responsavel,
+                            Responsavel: responsavel
                         });
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'fluxopassageiros.js',
-                        'btnSubmite.gridIda.for',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnSubmite.gridIda.for", error);
                 }
             }
         }
 
         const gridVolta = document.getElementById('grdVolta').ej2_instances[0];
-        if (gridVolta && gridVolta.getRows().length > 0) {
-            for (let i = 0; i < gridVolta.getRows().length; i++) {
-                try {
+        if (gridVolta && gridVolta.getRows().length > 0)
+        {
+            for (let i = 0; i < gridVolta.getRows().length; i++)
+            {
+                try
+                {
                     const c0 = gridVolta.getRows()[i].cells[0]?.innerHTML || '';
                     const c1 = gridVolta.getRows()[i].cells[1]?.innerHTML || '';
                     const c2 = gridVolta.getRows()[i].cells[2]?.innerHTML || '';
-                    if (c0 !== '') {
+                    if (c0 !== '')
+                    {
                         viagens.push({
                             Data: data,
                             IdaVolta: 'VOLTA',
@@ -407,25 +373,20 @@
                             VeiculoId: veiculoId,
                             MotoristaId: motoristaId,
                             MOB: mob,
-                            Responsavel: responsavel,
+                            Responsavel: responsavel
                         });
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'fluxopassageiros.js',
-                        'btnSubmite.gridVolta.for',
-                        error,
-                    );
-                }
-            }
-        }
-
-        if (viagens.length === 0) {
-            AppToast.show(
-                'Vermelho',
-                'Adicione pelo menos uma viagem antes de registrar.',
-                2000,
-            );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnSubmite.gridVolta.for", error);
+                }
+            }
+        }
+
+        if (viagens.length === 0)
+        {
+            AppToast.show('Vermelho', 'Adicione pelo menos uma viagem antes de registrar.', 2000);
             return;
         }
 
@@ -436,83 +397,80 @@
         btnSubmite.disabled = true;
 
         $.ajax({
-            type: 'POST',
-            url: '/api/Viagem/AdicionarViagensEconomildoLote',
-            contentType: 'application/json; charset=utf-8',
-            dataType: 'json',
+            type: "POST",
+            url: "/api/Viagem/AdicionarViagensEconomildoLote",
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
             data: JSON.stringify(viagens),
-            success: function (data) {
-                try {
-                    if (data.success) {
+            success: function (data)
+            {
+                try
+                {
+                    if (data.success)
+                    {
                         AppToast.show('Verde', data.message, 2000);
-                        setTimeout(function () {
-                            try {
+                        setTimeout(function ()
+                        {
+                            try
+                            {
 
                                 window.location.href = '/Viagens/GestaoFluxo';
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'fluxopassageiros.js',
-                                    'btnSubmite.redirect.setTimeout',
-                                    error,
-                                );
+                            }
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnSubmite.redirect.setTimeout", error);
                             }
                         }, 500);
-                    } else {
+                    }
+                    else
+                    {
 
                         iconOriginal.className = iconClasses;
                         btnSubmite.disabled = false;
                         AppToast.show('Vermelho', data.message, 3000);
                     }
-                } catch (error) {
+                }
+                catch (error)
+                {
 
                     iconOriginal.className = iconClasses;
                     btnSubmite.disabled = false;
-                    Alerta.TratamentoErroComLinha(
-                        'fluxopassageiros.js',
-                        'btnSubmite.ajax.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnSubmite.ajax.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                try {
+            error: function (xhr, status, error)
+            {
+                try
+                {
 
                     iconOriginal.className = iconClasses;
                     btnSubmite.disabled = false;
 
                     console.error('Erro AJAX:', error);
-                    AppToast.show(
-                        'Vermelho',
-                        'Um erro aconteceu ao adicionar as viagens.',
-                        3000,
-                    );
-                } catch (err) {
-                    Alerta.TratamentoErroComLinha(
-                        'fluxopassageiros.js',
-                        'btnSubmite.ajax.error',
-                        err,
-                    );
-                }
-            },
+                    AppToast.show('Vermelho', 'Um erro aconteceu ao adicionar as viagens.', 3000);
+                }
+                catch (err)
+                {
+                    Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnSubmite.ajax.error", err);
+                }
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'fluxopassageiros.js',
-            'btnSubmite.click',
-            error,
-        );
-    }
-});
-
-document.getElementById('btnCancela').addEventListener('click', function (e) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnSubmite.click", error);
+    }
+});
+
+document.getElementById('btnCancela').addEventListener('click', function (e)
+{
     e.preventDefault();
-    try {
+    try
+    {
         window.location.href = '/Viagens/GestaoFluxo';
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'fluxopassageiros.js',
-            'btnCancela.click',
-            error,
-        );
-    }
-});
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("fluxopassageiros.js", "btnCancela.click", error);
+    }
+});
```
