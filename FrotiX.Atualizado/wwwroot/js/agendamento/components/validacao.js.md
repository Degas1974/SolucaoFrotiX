# wwwroot/js/agendamento/components/validacao.js

**Mudanca:** GRANDE | **+474** linhas | **-619** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/components/validacao.js
+++ ATUAL: wwwroot/js/agendamento/components/validacao.js
@@ -1,774 +1,613 @@
-class ValidadorAgendamento {
-    constructor() {
+class ValidadorAgendamento
+{
+    constructor()
+    {
         this.erros = [];
     }
 
-    async validar(viagemId = null) {
-        try {
+    async validar(viagemId = null)
+    {
+        try
+        {
             this.erros = [];
 
             this._kmConfirmado = false;
             this._finalizacaoConfirmada = false;
 
-            if (!(await this.validarDataInicial())) return false;
-
-            if (!(await this.validarFinalidade())) return false;
-
-            if (!(await this.validarOrigem())) return false;
-
-            if (!(await this.validarDestino())) return false;
+            if (!await this.validarDataInicial()) return false;
+
+            if (!await this.validarFinalidade()) return false;
+
+            if (!await this.validarOrigem()) return false;
+
+            if (!await this.validarDestino()) return false;
 
             const algumFinalPreenchido = this.verificarCamposFinalizacao();
-            if (algumFinalPreenchido) {
-                if (!(await this.validarFinalizacao())) return false;
-            }
-
-            const btnTexto = $('#btnConfirma').text().trim();
-            const ehAgendamento =
-                btnTexto === 'Edita Agendamento' ||
-                btnTexto === 'Confirma Agendamento' ||
-                btnTexto === 'Confirmar';
-
-            if (!ehAgendamento || algumFinalPreenchido) {
-                if (!(await this.validarCamposViagem())) return false;
-            }
-
-            if (!(await this.validarRequisitante())) return false;
-
-            if (!(await this.validarRamal())) return false;
-
-            if (!(await this.validarSetor())) return false;
-
-            if (!(await this.validarEvento())) return false;
-
-            if (window.transformandoEmViagem === false) {
-                if (!(await this.validarRecorrencia())) return false;
-            }
-
-            if (!(await this.validarPeriodoRecorrencia())) return false;
-
-            if (!(await this.validarDiasVariados())) return false;
-
-            if (!(await this.validarKmFinal())) return false;
-
-            if (algumFinalPreenchido) {
-                if (!(await this.confirmarFinalizacao())) return false;
-            }
-
-            return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha('validacao.js', 'validar', error);
-            return false;
-        }
-    }
-
-    async validarDataInicial() {
-        try {
-
-            const kendoDatePicker =
-                $('#txtDataInicial').data('kendoDatePicker');
-
-            if (kendoDatePicker) {
-                const valDataInicial = kendoDatePicker.value();
-
-                if (!valDataInicial || !moment(valDataInicial).isValid()) {
-                    kendoDatePicker.value(new Date());
-                    return true;
-                }
-
-                const dataInicial = new Date(valDataInicial);
-                dataInicial.setHours(0, 0, 0, 0);
-                const hoje = new Date();
-                hoje.setHours(0, 0, 0, 0);
-
-                if (dataInicial < hoje) {
-                    await Alerta.Erro(
-                        'Data Invalida',
-                        'A <strong>Data Inicial</strong> nao pode ser anterior a data de hoje.',
-                    );
-                    kendoDatePicker.focus();
-                    return false;
-                }
-            } else {
-
-                const txtDataInicial =
-                    document.getElementById('txtDataInicial');
-                if (
-                    txtDataInicial &&
-                    (!txtDataInicial.value || txtDataInicial.value === '')
-                ) {
-                    txtDataInicial.value = moment().format('YYYY-MM-DD');
-                }
-            }
-
-            return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarDataInicial',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarFinalidade() {
-        try {
-            const finalidade =
-                document.getElementById('lstFinalidade').ej2_instances[0].value;
-
-            if (finalidade === '' || finalidade === null) {
+            if (algumFinalPreenchido)
+            {
+                if (!await this.validarFinalizacao()) return false;
+            }
+
+            const btnTexto = $("#btnConfirma").text().trim();
+            const ehAgendamento = btnTexto === "Edita Agendamento" || btnTexto === "Confirma Agendamento" || btnTexto === "Confirmar";
+
+            if (!ehAgendamento || algumFinalPreenchido)
+            {
+                if (!await this.validarCamposViagem()) return false;
+            }
+
+            if (!await this.validarRequisitante()) return false;
+
+            if (!await this.validarRamal()) return false;
+
+            if (!await this.validarSetor()) return false;
+
+            if (!await this.validarEvento()) return false;
+
+            if (window.transformandoEmViagem === false)
+            {
+                if (!await this.validarRecorrencia()) return false;
+            }
+
+            if (!await this.validarPeriodoRecorrencia()) return false;
+
+            if (!await this.validarDiasVariados()) return false;
+
+            if (!await this.validarKmFinal()) return false;
+
+            if (algumFinalPreenchido)
+            {
+                if (!await this.confirmarFinalizacao()) return false;
+            }
+
+            return true;
+
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validar", error);
+            return false;
+        }
+    }
+
+    async validarDataInicial()
+    {
+        try
+        {
+            const valDataInicial = window.getKendoDateValue("txtDataInicial");
+
+            if (!valDataInicial || !moment(valDataInicial).isValid())
+            {
+                window.setKendoDateValue("txtDataInicial", moment().toDate(), true);
+            }
+
+            return true;
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarDataInicial", error);
+            return false;
+        }
+    }
+
+    async validarFinalidade()
+    {
+        try
+        {
+            const finalidade = document.getElementById("lstFinalidade").ej2_instances[0].value;
+
+            if (finalidade === "" || finalidade === null)
+            {
+                await Alerta.Erro("Informação Ausente", "A <strong>Finalidade</strong> é obrigatória");
+                return false;
+            }
+
+            return true;
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarFinalidade", error);
+            return false;
+        }
+    }
+
+    async validarOrigem()
+    {
+        try
+        {
+            const origem = document.getElementById("cmbOrigem").ej2_instances[0].value;
+
+            if (origem === "" || origem === null)
+            {
+                await Alerta.Erro("Informação Ausente", "A Origem é obrigatória");
+                return false;
+            }
+
+            return true;
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarOrigem", error);
+            return false;
+        }
+    }
+
+    async validarDestino()
+    {
+        try
+        {
+            const destino = document.getElementById("cmbDestino").ej2_instances[0].value;
+
+            if (destino === "" || destino === null)
+            {
+                await Alerta.Erro("Informação Ausente", "O Destino é obrigatório");
+                return false;
+            }
+
+            return true;
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarDestino", error);
+            return false;
+        }
+    }
+
+    verificarCamposFinalizacao()
+    {
+        try
+        {
+            const dataFinal = $("#txtDataFinal").val();
+            const horaFinal = $("#txtHoraFinal").val();
+            const combustivelFinal = document.getElementById("ddtCombustivelFinal").ej2_instances[0].value;
+            const kmFinal = $("#txtKmFinal").val();
+
+            return dataFinal || horaFinal || combustivelFinal || kmFinal;
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "verificarCamposFinalizacao", error);
+            return false;
+        }
+    }
+
+    async validarFinalizacao()
+    {
+        try
+        {
+            const dataFinal = $("#txtDataFinal").val();
+            const horaFinal = $("#txtHoraFinal").val();
+            const combustivelFinal = document.getElementById("ddtCombustivelFinal")?.ej2_instances?.[0]?.value;
+            const kmFinal = $("#txtKmFinal").val();
+
+            const todosFinalPreenchidos = dataFinal && horaFinal && combustivelFinal && kmFinal;
+
+            if (!todosFinalPreenchidos)
+            {
                 await Alerta.Erro(
-                    'Informação Ausente',
-                    'A <strong>Finalidade</strong> é obrigatória',
+                    "Campos de Finalização Incompletos",
+                    "Para gravar uma viagem como 'Realizada', é necessário preencher todos os campos de Finalização:\n\n• Data Final\n• Hora Final\n• Km Final\n• Combustível Final"
                 );
                 return false;
             }
 
-            return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarFinalidade',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarOrigem() {
-        try {
-            const origem =
-                document.getElementById('cmbOrigem').ej2_instances[0].value;
-
-            if (origem === '' || origem === null) {
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'A Origem é obrigatória',
-                );
-                return false;
-            }
-
-            return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarOrigem',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarDestino() {
-        try {
-            const destino =
-                document.getElementById('cmbDestino').ej2_instances[0].value;
-
-            if (destino === '' || destino === null) {
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'O Destino é obrigatório',
-                );
-                return false;
-            }
-
-            return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarDestino',
-                error,
-            );
-            return false;
-        }
-    }
-
-    verificarCamposFinalizacao() {
-        try {
-            const dataFinal = $('#txtDataFinal').val();
-            const horaFinal = $('#txtHoraFinal').val();
-            const combustivelFinal = document.getElementById(
-                'ddtCombustivelFinal',
-            ).ej2_instances[0].value;
-            const kmFinal = $('#txtKmFinal').val();
-
-            return dataFinal || horaFinal || combustivelFinal || kmFinal;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'verificarCamposFinalizacao',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarFinalizacao() {
-        try {
-            const dataFinal = $('#txtDataFinal').val();
-            const horaFinal = $('#txtHoraFinal').val();
-            const combustivelFinal = document.getElementById(
-                'ddtCombustivelFinal',
-            )?.ej2_instances?.[0]?.value;
-            const kmFinal = $('#txtKmFinal').val();
-
-            const todosFinalPreenchidos =
-                dataFinal && horaFinal && combustivelFinal && kmFinal;
-
-            if (!todosFinalPreenchidos) {
-                await Alerta.Erro(
-                    'Campos de Finalização Incompletos',
-                    "Para gravar uma viagem como 'Realizada', é necessário preencher todos os campos de Finalização:\n\n• Data Final\n• Hora Final\n• Km Final\n• Combustível Final",
-                );
-                return false;
-            }
-
-            if (dataFinal) {
-                const dtFinal = window.parseDate
-                    ? window.parseDate(dataFinal)
-                    : new Date(dataFinal);
+            if (dataFinal)
+            {
+                const dtFinal = window.parseDate ? window.parseDate(dataFinal) : new Date(dataFinal);
                 const dtAtual = new Date();
 
                 dtFinal.setHours(0, 0, 0, 0);
                 dtAtual.setHours(0, 0, 0, 0);
 
-                if (dtFinal > dtAtual) {
+                if (dtFinal > dtAtual)
+                {
                     await Alerta.Erro(
-                        'Data Inválida',
-                        'A Data Final não pode ser superior à data atual.',
+                        "Data Inválida",
+                        "A Data Final não pode ser superior à data atual."
                     );
-                    $('#txtDataFinal').val('');
-                    $('#txtDataFinal').focus();
+                    window.setKendoDateValue("txtDataFinal", null);
+                    document.getElementById("txtDataFinal")?.focus();
                     return false;
                 }
             }
 
-            const destino =
-                document.getElementById('cmbDestino')?.ej2_instances?.[0]
-                    ?.value;
-            if (destino === '' || destino === null) {
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'O Destino é obrigatório para finalizar a viagem',
-                );
-                return false;
-            }
-
-            return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarFinalizacao',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarCamposViagem() {
-        try {
-
-            const lstMotorista =
-                document.getElementById('lstMotorista').ej2_instances[0];
-            if (lstMotorista.value === null || lstMotorista.value === '') {
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'O Motorista é obrigatório',
-                );
-                return false;
-            }
-
-            const lstVeiculo =
-                document.getElementById('lstVeiculo').ej2_instances[0];
-            if (lstVeiculo.value === null || lstVeiculo.value === '') {
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'O Veículo é obrigatório',
-                );
+            const destino = document.getElementById("cmbDestino")?.ej2_instances?.[0]?.value;
+            if (destino === "" || destino === null)
+            {
+                await Alerta.Erro("Informação Ausente", "O Destino é obrigatório para finalizar a viagem");
+                return false;
+            }
+
+            return true;
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarFinalizacao", error);
+            return false;
+        }
+    }
+
+    async validarCamposViagem()
+    {
+        try
+        {
+
+            const lstMotorista = document.getElementById("lstMotorista").ej2_instances[0];
+            if (lstMotorista.value === null || lstMotorista.value === "")
+            {
+                await Alerta.Erro("Informação Ausente", "O Motorista é obrigatório");
+                return false;
+            }
+
+            const lstVeiculo = document.getElementById("lstVeiculo").ej2_instances[0];
+            if (lstVeiculo.value === null || lstVeiculo.value === "")
+            {
+                await Alerta.Erro("Informação Ausente", "O Veículo é obrigatório");
                 return false;
             }
 
             const kmOk = await this.validarKmInicialFinal();
             if (!kmOk) return false;
 
-            const ddtCombustivelInicial = document.getElementById(
-                'ddtCombustivelInicial',
-            ).ej2_instances[0];
-            if (
-                ddtCombustivelInicial.value === '' ||
-                ddtCombustivelInicial.value === null
-            ) {
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'O Combustível Inicial é obrigatório',
-                );
-                return false;
-            }
-
-            return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarCamposViagem',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarRequisitante() {
-        try {
-
-            const lstRequisitanteEl =
-                document.getElementById('lstRequisitante');
-            const kendoComboBox = lstRequisitanteEl
-                ? $(lstRequisitanteEl).data('kendoComboBox')
-                : null;
-
-            const valorRequisitante = kendoComboBox
-                ? kendoComboBox.value()
-                : null;
-
-            if (!valorRequisitante || valorRequisitante === '') {
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'O Requisitante é obrigatório',
-                );
-                return false;
-            }
-
-            return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarRequisitante',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarRamal() {
-        try {
-
-            const ramalSFElement = document.getElementById(
-                'txtRamalRequisitanteSF',
-            );
-
-            if (
-                ramalSFElement &&
-                ramalSFElement.ej2_instances &&
-                ramalSFElement.ej2_instances[0]
-            ) {
+            const ddtCombustivelInicial = document.getElementById("ddtCombustivelInicial").ej2_instances[0];
+            if (ddtCombustivelInicial.value === "" || ddtCombustivelInicial.value === null)
+            {
+                await Alerta.Erro("Informação Ausente", "O Combustível Inicial é obrigatório");
+                return false;
+            }
+
+            return true;
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarCamposViagem", error);
+            return false;
+        }
+    }
+
+    async validarRequisitante()
+    {
+        try
+        {
+
+            const lstRequisitanteEl = document.getElementById("lstRequisitante");
+            const kendoComboBox = lstRequisitanteEl ? $(lstRequisitanteEl).data("kendoComboBox") : null;
+
+            const valorRequisitante = kendoComboBox ? kendoComboBox.value() : null;
+
+            if (!valorRequisitante || valorRequisitante === "")
+            {
+                await Alerta.Erro("Informação Ausente", "O Requisitante é obrigatório");
+                return false;
+            }
+
+            return true;
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarRequisitante", error);
+            return false;
+        }
+    }
+
+    async validarRamal()
+    {
+        try
+        {
+
+            const ramalSFElement = document.getElementById("txtRamalRequisitanteSF");
+
+            if (ramalSFElement && ramalSFElement.ej2_instances && ramalSFElement.ej2_instances[0])
+            {
 
                 const ramalSF = ramalSFElement.ej2_instances[0];
-                const valorRamalSF = document.getElementById(
-                    'txtRamalRequisitanteSF',
-                ).value;
-
-                if (
-                    !valorRamalSF ||
-                    valorRamalSF === '' ||
-                    valorRamalSF === null
-                ) {
-                    await Alerta.Erro(
-                        'Informação Ausente',
-                        'O Ramal do Requisitante é obrigatório',
-                    );
+                const valorRamalSF = document.getElementById("txtRamalRequisitanteSF").value;
+
+                if (!valorRamalSF || valorRamalSF === "" || valorRamalSF === null)
+                {
+                    await Alerta.Erro("Informação Ausente", "O Ramal do Requisitante é obrigatório");
                     return false;
                 }
 
-                console.log('✅ Ramal validado (Syncfusion):', valorRamalSF);
+                console.log("✅ Ramal validado (Syncfusion):", valorRamalSF);
                 return true;
             }
 
-            const valorRamal = $('#txtRamalRequisitante').val();
-            if (!valorRamal || valorRamal === '' || valorRamal === null) {
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'O Ramal do Requisitante é obrigatório',
-                );
-                return false;
-            }
-
-            console.log('✅ Ramal validado (HTML):', valorRamal);
-            return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarRamal',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarSetor() {
-        try {
-
-            const lstSetorElement = document.getElementById(
-                'lstSetorRequisitanteAgendamento',
-            );
-
-            if (!lstSetorElement) {
-                console.error(
-                    '❌ Elemento lstSetorRequisitanteAgendamento não encontrado',
-                );
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'O Setor do Requisitante é obrigatório',
-                );
-                return false;
-            }
-
-            const isVisible =
-                lstSetorElement.offsetWidth > 0 &&
-                lstSetorElement.offsetHeight > 0;
-            if (!isVisible) {
-                console.log(
-                    'ℹ️ lstSetorRequisitanteAgendamento está oculto - pulando validação',
-                );
+            const valorRamal = $("#txtRamalRequisitante").val();
+            if (!valorRamal || valorRamal === "" || valorRamal === null)
+            {
+                await Alerta.Erro("Informação Ausente", "O Ramal do Requisitante é obrigatório");
+                return false;
+            }
+
+            console.log("✅ Ramal validado (HTML):", valorRamal);
+            return true;
+
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarRamal", error);
+            return false;
+        }
+    }
+
+    async validarSetor()
+    {
+        try
+        {
+
+            const lstSetorElement = document.getElementById("lstSetorRequisitanteAgendamento");
+
+            if (!lstSetorElement)
+            {
+                console.error("❌ Elemento lstSetorRequisitanteAgendamento não encontrado");
+                await Alerta.Erro("Informação Ausente", "O Setor do Requisitante é obrigatório");
+                return false;
+            }
+
+            const isVisible = lstSetorElement.offsetWidth > 0 && lstSetorElement.offsetHeight > 0;
+            if (!isVisible)
+            {
+                console.log("ℹ️ lstSetorRequisitanteAgendamento está oculto - pulando validação");
                 return true;
             }
 
-            if (
-                !lstSetorElement.ej2_instances ||
-                lstSetorElement.ej2_instances.length === 0
-            ) {
-                console.error(
-                    '❌ lstSetorRequisitanteAgendamento não está inicializado como componente EJ2',
-                );
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'O Setor do Requisitante é obrigatório',
-                );
+            if (!lstSetorElement.ej2_instances || lstSetorElement.ej2_instances.length === 0)
+            {
+                console.error("❌ lstSetorRequisitanteAgendamento não está inicializado como componente EJ2");
+                await Alerta.Erro("Informação Ausente", "O Setor do Requisitante é obrigatório");
                 return false;
             }
 
             const lstSetor = lstSetorElement.ej2_instances[0];
             const valorSetor = lstSetor.value;
 
-            if (
-                !valorSetor ||
-                valorSetor === '' ||
+            if (!valorSetor ||
+                valorSetor === "" ||
                 valorSetor === null ||
-                (Array.isArray(valorSetor) && valorSetor.length === 0)
-            ) {
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'O Setor do Requisitante é obrigatório',
-                );
-                return false;
-            }
-
-            console.log('✅ Setor validado:', valorSetor);
-            return true;
-        } catch (error) {
-            console.error('❌ Erro em validarSetor:', error);
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarSetor',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarEvento() {
-        try {
-            const finalidade =
-                document.getElementById('lstFinalidade').ej2_instances[0].value;
-
-            if (finalidade && finalidade[0] === 'Evento') {
-                const evento =
-                    document.getElementById('lstEventos').ej2_instances[0]
-                        .value;
-
-                if (evento === '' || evento === null) {
-                    await Alerta.Erro(
-                        'Informação Ausente',
-                        'O Nome do Evento é obrigatório',
-                    );
+                (Array.isArray(valorSetor) && valorSetor.length === 0))
+            {
+                await Alerta.Erro("Informação Ausente", "O Setor do Requisitante é obrigatório");
+                return false;
+            }
+
+            console.log("✅ Setor validado:", valorSetor);
+            return true;
+
+        } catch (error)
+        {
+            console.error("❌ Erro em validarSetor:", error);
+            Alerta.TratamentoErroComLinha("validacao.js", "validarSetor", error);
+            return false;
+        }
+    }
+
+    async validarEvento()
+    {
+        try
+        {
+            const finalidade = document.getElementById("lstFinalidade").ej2_instances[0].value;
+
+            if (finalidade && finalidade[0] === "Evento")
+            {
+                const evento = document.getElementById("lstEventos").ej2_instances[0].value;
+
+                if (evento === "" || evento === null)
+                {
+                    await Alerta.Erro("Informação Ausente", "O Nome do Evento é obrigatório");
                     return false;
                 }
             }
 
             return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarEvento',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarRecorrencia() {
-        try {
-
-            const lstRecorrenteKendo =
-                $('#lstRecorrente').data('kendoDropDownList');
-            const lstPeriodosKendo =
-                $('#lstPeriodos').data('kendoDropDownList');
-
-            const recorrente = lstRecorrenteKendo
-                ? lstRecorrenteKendo.value()
-                : null;
-            const periodo = lstPeriodosKendo ? lstPeriodosKendo.value() : null;
-
-            if (recorrente === 'S' && (!periodo || periodo === '')) {
-                await Alerta.Erro(
-                    'Informação Ausente',
-                    'Se o Agendamento é Recorrente, você precisa escolher o Período de Recorrência',
-                );
-                return false;
-            }
-
-            if (periodo === 'S' || periodo === 'Q') {
-
-                const lstDiasKendo = $('#lstDias').data('kendoMultiSelect');
-                const diasSelecionados = lstDiasKendo
-                    ? lstDiasKendo.value()
-                    : [];
-
-                if (!diasSelecionados || diasSelecionados.length === 0) {
-                    await Alerta.Erro(
-                        'Informação Ausente',
-                        'Para período Semanal ou Quinzenal, você precisa escolher ao menos um Dia da Semana',
-                    );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarEvento", error);
+            return false;
+        }
+    }
+
+    async validarRecorrencia()
+    {
+        try
+        {
+            const recorrente = document.getElementById("lstRecorrente").ej2_instances[0].value;
+            const periodo = document.getElementById("lstPeriodos").ej2_instances[0].value;
+
+            if (recorrente === "S" && (!periodo || periodo === ""))
+            {
+                await Alerta.Erro("Informação Ausente", "Se o Agendamento é Recorrente, você precisa escolher o Período de Recorrência");
+                return false;
+            }
+
+            if (periodo === "S" || periodo === "Q")
+            {
+                const diasSelecionados = document.getElementById("lstDias").ej2_instances[0].value;
+
+                if (!diasSelecionados || diasSelecionados.length === 0)
+                {
+                    await Alerta.Erro("Informação Ausente", "Para período Semanal ou Quinzenal, você precisa escolher ao menos um Dia da Semana");
                     return false;
                 }
             }
 
-            if (periodo === 'M') {
-
-                const lstDiasMesKendo =
-                    $('#lstDiasMes').data('kendoDropDownList');
-                const diaMes = lstDiasMesKendo ? lstDiasMesKendo.value() : null;
-
-                if (!diaMes || diaMes === '' || diaMes === null) {
-                    await Alerta.Erro(
-                        'Informação Ausente',
-                        'Para período Mensal, você precisa escolher o Dia do Mês',
-                    );
+            if (periodo === "M")
+            {
+                const diaMes = document.getElementById("lstDiasMes").ej2_instances[0].value;
+
+                if (!diaMes || diaMes === "" || diaMes === null)
+                {
+                    await Alerta.Erro("Informação Ausente", "Para período Mensal, você precisa escolher o Dia do Mês");
                     return false;
                 }
             }
 
             return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarRecorrencia',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarPeriodoRecorrencia() {
-        try {
-
-            const lstPeriodosKendo =
-                $('#lstPeriodos').data('kendoDropDownList');
-            const periodo = lstPeriodosKendo ? lstPeriodosKendo.value() : null;
-
-            if (
-                periodo === 'D' ||
-                periodo === 'S' ||
-                periodo === 'Q' ||
-                periodo === 'M'
-            ) {
-
-                const txtFinalRecorrencia = document.getElementById(
-                    'txtFinalRecorrencia',
-                );
-                const dataFinal = txtFinalRecorrencia
-                    ? txtFinalRecorrencia.value?.trim()
-                    : '';
-
-                if (dataFinal === '' || dataFinal === null) {
-                    await Alerta.Erro(
-                        'Informação Ausente',
-                        'Se o período foi escolhido como diário, semanal, quinzenal ou mensal, você precisa escolher a Data Final',
-                    );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarRecorrencia", error);
+            return false;
+        }
+    }
+
+    async validarPeriodoRecorrencia()
+    {
+        try
+        {
+            const periodo = document.getElementById("lstPeriodos").ej2_instances[0].value;
+
+            if ((periodo === "D" || periodo === "S" || periodo === "Q" || periodo === "M"))
+            {
+                const dataFinal = window.getKendoDateValue("txtFinalRecorrencia");
+
+                if (dataFinal === "" || dataFinal === null)
+                {
+                    await Alerta.Erro("Informação Ausente", "Se o período foi escolhido como diário, semanal, quinzenal ou mensal, você precisa escolher a Data Final");
                     return false;
                 }
             }
 
             return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarPeriodoRecorrencia',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarDiasVariados() {
-        try {
-
-            const lstPeriodosKendo =
-                $('#lstPeriodos').data('kendoDropDownList');
-            const periodo = lstPeriodosKendo ? lstPeriodosKendo.value() : null;
-
-            if (periodo === 'V') {
-
-                const calendarElement = document.getElementById(
-                    'calDatasSelecionadas',
-                );
-
-                if (
-                    !calendarElement ||
-                    !calendarElement.ej2_instances ||
-                    !calendarElement.ej2_instances[0]
-                ) {
-
-                    console.log(
-                        'ℹ️ Calendário não disponível - pulando validação de dias variados',
-                    );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarPeriodoRecorrencia", error);
+            return false;
+        }
+    }
+
+    async validarDiasVariados()
+    {
+        try
+        {
+            const periodo = document.getElementById("lstPeriodos").ej2_instances[0].value;
+
+            if (periodo === "V")
+            {
+
+                const calendarElement = document.getElementById("calDatasSelecionadas");
+
+                if (!calendarElement || !calendarElement.ej2_instances || !calendarElement.ej2_instances[0])
+                {
+
+                    console.log("ℹ️ Calendário não disponível - pulando validação de dias variados");
                     return true;
                 }
 
                 const calendarObj = calendarElement.ej2_instances[0];
                 const selectedDates = calendarObj.values;
 
-                if (!selectedDates || selectedDates.length === 0) {
-                    await Alerta.Erro(
-                        'Informação Ausente',
-                        'Se o período foi escolhido como Dias Variados, você precisa escolher ao menos um dia no Calendário',
-                    );
+                if (!selectedDates || selectedDates.length === 0)
+                {
+                    await Alerta.Erro("Informação Ausente", "Se o período foi escolhido como Dias Variados, você precisa escolher ao menos um dia no Calendário");
                     return false;
                 }
             }
 
             return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarDiasVariados',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarKmInicialFinal() {
-        try {
-            const kmInicial = $('#txtKmInicial').val();
-            const kmFinal = $('#txtKmFinal').val();
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarDiasVariados", error);
+            return false;
+        }
+    }
+
+    async validarKmInicialFinal()
+    {
+        try
+        {
+            const kmInicial = $("#txtKmInicial").val();
+            const kmFinal = $("#txtKmFinal").val();
 
             if (!kmInicial || !kmFinal) return true;
 
-            const ini = parseFloat(kmInicial.replace(',', '.'));
-            const fim = parseFloat(kmFinal.replace(',', '.'));
-
-            if (fim < ini) {
+            const ini = parseFloat(kmInicial.replace(",", "."));
+            const fim = parseFloat(kmFinal.replace(",", "."));
+
+            if (fim < ini)
+            {
+                await Alerta.Erro("Erro", "A quilometragem final deve ser maior que a inicial.");
+                return false;
+            }
+
+            const diff = fim - ini;
+            if (diff > 2000)
+            {
                 await Alerta.Erro(
-                    'Erro',
-                    'A quilometragem final deve ser maior que a inicial.',
+                    "Quilometragem Inválida",
+                    `A quilometragem final não pode exceder a inicial em mais de 2.000 km.\n\nDiferença informada: ${diff.toLocaleString('pt-BR')} km`
                 );
-                return false;
-            }
-
-            const diff = fim - ini;
-            if (diff > 2000) {
-                await Alerta.Erro(
-                    'Quilometragem Inválida',
-                    `A quilometragem final não pode exceder a inicial em mais de 2.000 km.\n\nDiferença informada: ${diff.toLocaleString('pt-BR')} km`,
+                $("#txtKmFinal").val("");
+                $("#txtKmFinal").focus();
+                return false;
+            }
+
+            if (diff > 100 && !this._kmConfirmado)
+            {
+                const confirmacao = await Alerta.Confirmar(
+                    "Atenção",
+                    "A quilometragem <strong>final</strong> excede em 100km a <strong>inicial</strong>. Tem certeza?",
+                    "Tenho certeza! 💪🏼",
+                    "Me enganei! 😟"
                 );
-                $('#txtKmFinal').val('');
-                $('#txtKmFinal').focus();
-                return false;
-            }
-
-            if (diff > 100 && !this._kmConfirmado) {
+
+                if (!confirmacao)
+                {
+                    $("#txtKmFinal").val("");
+                    $("#txtKmFinal").focus();
+                    return false;
+                }
+
+                this._kmConfirmado = true;
+            }
+
+            return true;
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarKmInicialFinal", error);
+            return false;
+        }
+    }
+
+    async validarKmFinal()
+    {
+        try
+        {
+            const kmFinal = $("#txtKmFinal").val();
+
+            if (kmFinal && parseFloat(kmFinal) <= 0)
+            {
+                await Alerta.Erro("Informação Incorreta", "A Quilometragem Final deve ser maior que zero");
+                return false;
+            }
+
+            return true;
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "validarKmFinal", error);
+            return false;
+        }
+    }
+
+    async confirmarFinalizacao()
+    {
+        try
+        {
+            const dataFinal = $("#txtDataFinal").val();
+            const horaFinal = $("#txtHoraFinal").val();
+            const combustivelFinal = document.getElementById("ddtCombustivelFinal").ej2_instances[0].value;
+            const kmFinal = $("#txtKmFinal").val();
+
+            const todosFinalPreenchidos = dataFinal && horaFinal && combustivelFinal && kmFinal;
+
+            if (todosFinalPreenchidos && !this._finalizacaoConfirmada)
+            {
                 const confirmacao = await Alerta.Confirmar(
-                    'Atenção',
-                    'A quilometragem <strong>final</strong> excede em 100km a <strong>inicial</strong>. Tem certeza?',
-                    'Tenho certeza! 💪🏼',
-                    'Me enganei! 😟',
+                    "Confirmar Fechamento",
+                    'Você está criando a viagem como "Realizada". Deseja continuar?',
+                    "Sim, criar!",
+                    "Cancelar"
                 );
 
-                if (!confirmacao) {
-                    $('#txtKmFinal').val('');
-                    $('#txtKmFinal').focus();
-                    return false;
-                }
-
-                this._kmConfirmado = true;
-            }
-
-            return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarKmInicialFinal',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async validarKmFinal() {
-        try {
-            const kmFinal = $('#txtKmFinal').val();
-
-            if (kmFinal && parseFloat(kmFinal) <= 0) {
-                await Alerta.Erro(
-                    'Informação Incorreta',
-                    'A Quilometragem Final deve ser maior que zero',
-                );
-                return false;
-            }
-
-            return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'validarKmFinal',
-                error,
-            );
-            return false;
-        }
-    }
-
-    async confirmarFinalizacao() {
-        try {
-            const dataFinal = $('#txtDataFinal').val();
-            const horaFinal = $('#txtHoraFinal').val();
-            const combustivelFinal = document.getElementById(
-                'ddtCombustivelFinal',
-            ).ej2_instances[0].value;
-            const kmFinal = $('#txtKmFinal').val();
-
-            const todosFinalPreenchidos =
-                dataFinal && horaFinal && combustivelFinal && kmFinal;
-
-            if (todosFinalPreenchidos && !this._finalizacaoConfirmada) {
-                const confirmacao = await Alerta.Confirmar(
-                    'Confirmar Fechamento',
-                    'Você está criando a viagem como "Realizada". Deseja continuar?',
-                    'Sim, criar!',
-                    'Cancelar',
-                );
-
                 if (!confirmacao) return false;
 
                 this._finalizacaoConfirmada = true;
             }
 
             return true;
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'validacao.js',
-                'confirmarFinalizacao',
-                error,
-            );
+        } catch (error)
+        {
+            Alerta.TratamentoErroComLinha("validacao.js", "confirmarFinalizacao", error);
             return false;
         }
     }
@@ -776,19 +615,24 @@
 
 window.ValidadorAgendamento = new ValidadorAgendamento();
 
-window.ValidaCampos = async function (viagemId) {
-    try {
+window.ValidaCampos = async function (viagemId)
+{
+    try
+    {
         return await window.ValidadorAgendamento.validar(viagemId);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('validacao.js', 'ValidaCampos', error);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("validacao.js", "ValidaCampos", error);
         return false;
     }
 };
 
-window.validarDatas = async function () {
-    try {
-        const txtDataInicial = $('#txtDataInicial').val();
-        const txtDataFinal = $('#txtDataFinal').val();
+window.validarDatas = async function ()
+{
+    try
+    {
+        const txtDataInicial = $("#txtDataInicial").val();
+        const txtDataFinal = $("#txtDataFinal").val();
 
         if (!txtDataFinal || !txtDataInicial) return true;
 
@@ -800,30 +644,35 @@
 
         const diferenca = (dtFinal - dtInicial) / (1000 * 60 * 60 * 24);
 
-        if (diferenca >= 5) {
+        if (diferenca >= 5)
+        {
             const confirmacao = await Alerta.Confirmar(
-                'Atenção',
-                'A Data Final está 5 dias ou mais após a Inicial. Tem certeza?',
-                'Tenho certeza! 💪🏼',
-                'Me enganei! 😟',
+                "Atenção",
+                "A Data Final está 5 dias ou mais após a Inicial. Tem certeza?",
+                "Tenho certeza! 💪🏼",
+                "Me enganei! 😟"
             );
 
-            if (!confirmacao) {
-                $('#txtDataFinal').val('');
-                $('#txtDataFinal').focus();
+            if (!confirmacao)
+            {
+                window.setKendoDateValue("txtDataFinal", null);
+                document.getElementById("txtDataFinal")?.focus();
                 return false;
             }
         }
 
         return true;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('validacao.js', 'validarDatas', error);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("validacao.js", "validarDatas", error);
         return false;
     }
 };
 
-window.validarDatasInicialFinal = async function (DataInicial, DataFinal) {
-    try {
+window.validarDatasInicialFinal = async function (DataInicial, DataFinal)
+{
+    try
+    {
         const dtIni = window.parseDate(DataInicial);
         const dtFim = window.parseDate(DataFinal);
 
@@ -831,30 +680,27 @@
 
         const diff = (dtFim - dtIni) / (1000 * 60 * 60 * 24);
 
-        if (diff >= 5) {
+        if (diff >= 5)
+        {
             const confirmacao = await Alerta.Confirmar(
-                'Atenção',
-                'A Data Final está 5 dias ou mais após a Inicial. Tem certeza?',
-                'Tenho certeza! 💪🏼',
-                'Me enganei! 😟',
+                "Atenção",
+                "A Data Final está 5 dias ou mais após a Inicial. Tem certeza?",
+                "Tenho certeza! 💪🏼",
+                "Me enganei! 😟"
             );
 
-            if (!confirmacao) {
-                const txtDataFinalElement =
-                    document.getElementById('txtDataFinal');
-                txtDataFinalElement.value = null;
-                txtDataFinalElement.focus();
+            if (!confirmacao)
+            {
+                window.setKendoDateValue("txtDataFinal", null);
+                document.getElementById("txtDataFinal")?.focus();
                 return false;
             }
         }
 
         return true;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'validacao.js',
-            'validarDatasInicialFinal',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("validacao.js", "validarDatasInicialFinal", error);
         return false;
     }
 };
```
