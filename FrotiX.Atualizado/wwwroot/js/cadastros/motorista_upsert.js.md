# wwwroot/js/cadastros/motorista_upsert.js

**Mudanca:** GRANDE | **+230** linhas | **-250** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/motorista_upsert.js
+++ ATUAL: wwwroot/js/cadastros/motorista_upsert.js
@@ -1,295 +1,271 @@
-$(document).ready(function () {
-    try {
-
-        const motoristaId = $('#MotoristaObj_Motorista_MotoristaId').val();
-        if (
-            !motoristaId ||
-            motoristaId === '00000000-0000-0000-0000-000000000000'
-        ) {
-            $('#chkStatus').prop('checked', true);
-
-            $('#imgPreview').attr('src', '/Images/barbudo.jpg');
-            $('#imgPreview').show();
-            $('#txtSemFoto').hide();
-        }
-
-        setTimeout(function () {
-            try {
+$(document).ready(function ()
+{
+    try
+    {
+
+        const motoristaId = $("#MotoristaObj_Motorista_MotoristaId").val();
+        if (!motoristaId || motoristaId === "00000000-0000-0000-0000-000000000000")
+        {
+            $("#chkStatus").prop("checked", true);
+
+            $("#imgPreview").attr("src", "/Images/barbudo.jpg");
+            $("#imgPreview").show();
+            $("#txtSemFoto").hide();
+        }
+
+        setTimeout(function ()
+        {
+            try
+            {
                 aplicarMascaras();
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'motorista_upsert.js',
-                    'setTimeout.aplicarMascaras',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("motorista_upsert.js", "setTimeout.aplicarMascaras", error);
             }
         }, 100);
 
-        $('#txtCPF').on('blur', function () {
-            try {
+        $("#txtCPF").on("blur", function ()
+        {
+            try
+            {
                 var cpf = $(this).val()?.trim();
 
-                if (!cpf || cpf === '') {
+                if (!cpf || cpf === "")
+                {
                     return;
                 }
 
-                if (!validarCPF(cpf)) {
+                if (!validarCPF(cpf))
+                {
 
                     $(this).val('');
 
-                    Alerta.Erro(
-                        'CPF Inválido',
-                        'O CPF informado não é válido. Verifique os dígitos digitados.',
-                    );
-                }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'motorista_upsert.js',
-                    'txtCPF.blur',
-                    error,
-                );
+                    Alerta.Erro("CPF Inválido", "O CPF informado não é válido. Verifique os dígitos digitados.");
+                }
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("motorista_upsert.js", "txtCPF.blur", error);
             }
         });
 
-        $('#fotoUpload').on('change', function (e) {
-            try {
+        $("#fotoUpload").on("change", function (e)
+        {
+            try
+            {
                 var file = e.target.files[0];
 
-                if (file) {
-
-                    var tiposPermitidos = [
-                        'image/jpeg',
-                        'image/jpg',
-                        'image/png',
-                        'image/gif',
-                    ];
-                    if (!tiposPermitidos.includes(file.type)) {
-                        Alerta.Erro(
-                            'Formato Inválido',
-                            'Por favor, selecione uma imagem nos formatos: JPG, PNG ou GIF',
-                        );
+                if (file)
+                {
+
+                    var tiposPermitidos = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
+                    if (!tiposPermitidos.includes(file.type))
+                    {
+                        Alerta.Erro("Formato Inválido", "Por favor, selecione uma imagem nos formatos: JPG, PNG ou GIF");
                         $(this).val('');
                         return;
                     }
 
                     var tamanhoMaximo = 5 * 1024 * 1024;
-                    if (file.size > tamanhoMaximo) {
-                        Alerta.Erro(
-                            'Arquivo Muito Grande',
-                            'A foto deve ter no máximo 5MB',
-                        );
+                    if (file.size > tamanhoMaximo)
+                    {
+                        Alerta.Erro("Arquivo Muito Grande", "A foto deve ter no máximo 5MB");
                         $(this).val('');
                         return;
                     }
 
                     var reader = new FileReader();
 
-                    reader.onload = function (event) {
-                        try {
-                            $('#imgPreview').attr('src', event.target.result);
-                            $('#imgPreview').show();
-                            $('#txtSemFoto').hide();
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'motorista_upsert.js',
-                                'fotoUpload.reader.onload',
-                                error,
-                            );
+                    reader.onload = function (event)
+                    {
+                        try
+                        {
+                            $("#imgPreview").attr("src", event.target.result);
+                            $("#imgPreview").show();
+                            $("#txtSemFoto").hide();
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("motorista_upsert.js", "fotoUpload.reader.onload", error);
                         }
                     };
 
-                    reader.onerror = function () {
-                        try {
-                            Alerta.Erro(
-                                'Erro ao Carregar Imagem',
-                                'Não foi possível carregar a imagem selecionada',
-                            );
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'motorista_upsert.js',
-                                'fotoUpload.reader.onerror',
-                                error,
-                            );
+                    reader.onerror = function ()
+                    {
+                        try
+                        {
+                            Alerta.Erro("Erro ao Carregar Imagem", "Não foi possível carregar a imagem selecionada");
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("motorista_upsert.js", "fotoUpload.reader.onerror", error);
                         }
                     };
 
                     reader.readAsDataURL(file);
-                } else {
-
-                    $('#imgPreview').hide();
-                    $('#txtSemFoto').show();
-                }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'motorista_upsert.js',
-                    'fotoUpload.change',
-                    error,
-                );
+                }
+                else
+                {
+
+                    $("#imgPreview").hide();
+                    $("#txtSemFoto").show();
+                }
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("motorista_upsert.js", "fotoUpload.change", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'motorista_upsert.js',
-            'document.ready',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("motorista_upsert.js", "document.ready", error);
     }
 });
 
-function validarFormulario() {
-    try {
+function validarFormulario()
+{
+    try
+    {
         return validarCamposObrigatorios();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'motorista_upsert.js',
-            'validarFormulario',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("motorista_upsert.js", "validarFormulario", error);
         return false;
     }
 }
 
-function validarCamposObrigatorios() {
-    try {
-
-        const nome = $('#txtNome').val()?.trim();
-        if (!nome || nome === '') {
-            Alerta.Erro('Informação Ausente', 'O campo Nome é obrigatório.');
-            $('#txtNome').focus();
-            return false;
-        }
-
-        const cpf = $('#txtCPF').val()?.trim();
-        if (!cpf || cpf === '') {
-            Alerta.Erro('Informação Ausente', 'O campo CPF é obrigatório.');
-            $('#txtCPF').focus();
-            return false;
-        }
-
-        if (!validarCPF(cpf)) {
-            Alerta.Erro(
-                'CPF Inválido',
-                'O CPF informado não é válido. Verifique os dígitos digitados.',
-            );
-            $('#txtCPF').focus();
-            return false;
-        }
-
-        const dataNascimento = $('#txtDataNascimento').val()?.trim();
-        if (!dataNascimento || dataNascimento === '') {
-            Alerta.Erro(
-                'Informação Ausente',
-                'O campo Data de Nascimento é obrigatório.',
-            );
-            $('#txtDataNascimento').focus();
-            return false;
-        }
-
-        const celular01 = $('#txtCelular01').val()?.trim();
-        if (!celular01 || celular01 === '') {
-            Alerta.Erro(
-                'Informação Ausente',
-                'O campo Primeiro Celular é obrigatório.',
-            );
-            $('#txtCelular01').focus();
-            return false;
-        }
-
-        const cnh = $('#txtCNH').val()?.trim();
-        if (!cnh || cnh === '') {
-            Alerta.Erro('Informação Ausente', 'O campo CNH é obrigatório.');
-            $('#txtCNH').focus();
-            return false;
-        }
-
-        const categoriaCNH = $('#txtCategoriaCNH').val()?.trim();
-        if (!categoriaCNH || categoriaCNH === '') {
-            Alerta.Erro(
-                'Informação Ausente',
-                'O campo Categoria Habilitação é obrigatório.',
-            );
-            $('#txtCategoriaCNH').focus();
-            return false;
-        }
-
-        const dataVencimentoCNH = $('#txtDataVencimentoCNH').val()?.trim();
-        if (!dataVencimentoCNH || dataVencimentoCNH === '') {
-            Alerta.Erro(
-                'Informação Ausente',
-                'O campo Data Vencimento CNH é obrigatório.',
-            );
-            $('#txtDataVencimentoCNH').focus();
-            return false;
-        }
-
-        const dataIngresso = $('#txtDataIngresso').val()?.trim();
-        if (!dataIngresso || dataIngresso === '') {
-            Alerta.Erro(
-                'Informação Ausente',
-                'O campo Data Ingresso é obrigatório.',
-            );
-            $('#txtDataIngresso').focus();
-            return false;
-        }
-
-        const unidadeId = $('#ddlUnidade').val();
-        if (!unidadeId || unidadeId === '') {
-            Alerta.Erro(
-                'Informação Ausente',
-                'O campo Unidade Vinculada é obrigatório.',
-            );
-            $('#ddlUnidade').focus();
-            return false;
-        }
-
-        const tipoCondutor = $('#ddlTipoCondutor').val();
-        if (!tipoCondutor || tipoCondutor === '') {
-            Alerta.Erro(
-                'Informação Ausente',
-                'O campo Tipo Condutor é obrigatório.',
-            );
-            $('#ddlTipoCondutor').focus();
-            return false;
-        }
-
-        const efetivoFerista = $('#ddlEfetivoFerista').val();
-        if (!efetivoFerista || efetivoFerista === '') {
-            Alerta.Erro(
-                'Informação Ausente',
-                'O campo Efetivo / Ferista é obrigatório.',
-            );
-            $('#ddlEfetivoFerista').focus();
+function validarCamposObrigatorios()
+{
+    try
+    {
+
+        const nome = $("#txtNome").val()?.trim();
+        if (!nome || nome === "")
+        {
+            Alerta.Erro("Informação Ausente", "O campo Nome é obrigatório.");
+            $("#txtNome").focus();
+            return false;
+        }
+
+        const cpf = $("#txtCPF").val()?.trim();
+        if (!cpf || cpf === "")
+        {
+            Alerta.Erro("Informação Ausente", "O campo CPF é obrigatório.");
+            $("#txtCPF").focus();
+            return false;
+        }
+
+        if (!validarCPF(cpf))
+        {
+            Alerta.Erro("CPF Inválido", "O CPF informado não é válido. Verifique os dígitos digitados.");
+            $("#txtCPF").focus();
+            return false;
+        }
+
+        const dataNascimento = $("#txtDataNascimento").val()?.trim();
+        if (!dataNascimento || dataNascimento === "")
+        {
+            Alerta.Erro("Informação Ausente", "O campo Data de Nascimento é obrigatório.");
+            $("#txtDataNascimento").focus();
+            return false;
+        }
+
+        const celular01 = $("#txtCelular01").val()?.trim();
+        if (!celular01 || celular01 === "")
+        {
+            Alerta.Erro("Informação Ausente", "O campo Primeiro Celular é obrigatório.");
+            $("#txtCelular01").focus();
+            return false;
+        }
+
+        const cnh = $("#txtCNH").val()?.trim();
+        if (!cnh || cnh === "")
+        {
+            Alerta.Erro("Informação Ausente", "O campo CNH é obrigatório.");
+            $("#txtCNH").focus();
+            return false;
+        }
+
+        const categoriaCNH = $("#txtCategoriaCNH").val()?.trim();
+        if (!categoriaCNH || categoriaCNH === "")
+        {
+            Alerta.Erro("Informação Ausente", "O campo Categoria Habilitação é obrigatório.");
+            $("#txtCategoriaCNH").focus();
+            return false;
+        }
+
+        const dataVencimentoCNH = $("#txtDataVencimentoCNH").val()?.trim();
+        if (!dataVencimentoCNH || dataVencimentoCNH === "")
+        {
+            Alerta.Erro("Informação Ausente", "O campo Data Vencimento CNH é obrigatório.");
+            $("#txtDataVencimentoCNH").focus();
+            return false;
+        }
+
+        const dataIngresso = $("#txtDataIngresso").val()?.trim();
+        if (!dataIngresso || dataIngresso === "")
+        {
+            Alerta.Erro("Informação Ausente", "O campo Data Ingresso é obrigatório.");
+            $("#txtDataIngresso").focus();
+            return false;
+        }
+
+        const unidadeId = $("#ddlUnidade").val();
+        if (!unidadeId || unidadeId === "")
+        {
+            Alerta.Erro("Informação Ausente", "O campo Unidade Vinculada é obrigatório.");
+            $("#ddlUnidade").focus();
+            return false;
+        }
+
+        const tipoCondutor = $("#ddlTipoCondutor").val();
+        if (!tipoCondutor || tipoCondutor === "")
+        {
+            Alerta.Erro("Informação Ausente", "O campo Tipo Condutor é obrigatório.");
+            $("#ddlTipoCondutor").focus();
+            return false;
+        }
+
+        const efetivoFerista = $("#ddlEfetivoFerista").val();
+        if (!efetivoFerista || efetivoFerista === "")
+        {
+            Alerta.Erro("Informação Ausente", "O campo Efetivo / Ferista é obrigatório.");
+            $("#ddlEfetivoFerista").focus();
             return false;
         }
 
         return true;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'motorista_upsert.js',
-            'validarCamposObrigatorios',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("motorista_upsert.js", "validarCamposObrigatorios", error);
         return false;
     }
 }
 
-function aplicarMascaras() {
-    try {
-
-        if (typeof $.fn.mask === 'undefined') {
+function aplicarMascaras()
+{
+    try
+    {
+
+        if (typeof $.fn.mask === 'undefined')
+        {
             console.error('jQuery Mask Plugin NÃO ESTÁ DISPONÍVEL!');
             console.log('$.fn.mask:', $.fn.mask);
             console.log('jQuery versão:', $.fn.jquery);
 
-            $('#txtCategoriaCNH').on('input', function () {
-                try {
+            $("#txtCategoriaCNH").on("input", function ()
+            {
+                try
+                {
                     $(this).val($(this).val().toUpperCase());
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'motorista_upsert.js',
-                        'txtCategoriaCNH.input',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("motorista_upsert.js", "txtCategoriaCNH.input", error);
                 }
             });
             return;
@@ -297,78 +273,82 @@
 
         console.log('jQuery Mask Plugin disponível! Aplicando máscaras...');
 
-        $('#txtCPF').mask('000.000.000-00');
-
-        $('#txtCelular01').mask('(00) 00000-0000');
-        $('#txtCelular02').mask('(00) 00000-0000');
-
-        $('#txtCNH').mask('00000000000');
-
-        $('#txtCategoriaCNH').on('input', function () {
-            try {
+        $("#txtCPF").mask("000.000.000-00");
+
+        $("#txtCelular01").mask("(00) 00000-0000");
+        $("#txtCelular02").mask("(00) 00000-0000");
+
+        $("#txtCNH").mask("00000000000");
+
+        $("#txtCategoriaCNH").on("input", function ()
+        {
+            try
+            {
                 $(this).val($(this).val().toUpperCase());
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'motorista_upsert.js',
-                    'txtCategoriaCNH.input',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("motorista_upsert.js", "txtCategoriaCNH.input", error);
             }
         });
 
         console.log('Máscaras aplicadas com sucesso!');
-    } catch (error) {
+    }
+    catch (error)
+    {
         console.error('ERRO ao aplicar máscaras:', error);
-        Alerta.TratamentoErroComLinha(
-            'motorista_upsert.js',
-            'aplicarMascaras',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("motorista_upsert.js", "aplicarMascaras", error);
     }
 }
 
-function validarCPF(cpf) {
-    try {
+function validarCPF(cpf)
+{
+    try
+    {
 
         cpf = cpf.replace(/[^\d]/g, '');
 
-        if (cpf.length !== 11) {
-            return false;
-        }
-
-        if (/^(\d)\1{10}$/.test(cpf)) {
+        if (cpf.length !== 11)
+        {
+            return false;
+        }
+
+        if (/^(\d)\1{10}$/.test(cpf))
+        {
             return false;
         }
 
         let soma = 0;
-        for (let i = 0; i < 9; i++) {
+        for (let i = 0; i < 9; i++)
+        {
             soma += parseInt(cpf.charAt(i)) * (10 - i);
         }
         let resto = soma % 11;
         let digitoVerificador1 = resto < 2 ? 0 : 11 - resto;
 
-        if (digitoVerificador1 !== parseInt(cpf.charAt(9))) {
+        if (digitoVerificador1 !== parseInt(cpf.charAt(9)))
+        {
             return false;
         }
 
         soma = 0;
-        for (let i = 0; i < 10; i++) {
+        for (let i = 0; i < 10; i++)
+        {
             soma += parseInt(cpf.charAt(i)) * (11 - i);
         }
         resto = soma % 11;
         let digitoVerificador2 = resto < 2 ? 0 : 11 - resto;
 
-        if (digitoVerificador2 !== parseInt(cpf.charAt(10))) {
+        if (digitoVerificador2 !== parseInt(cpf.charAt(10)))
+        {
             return false;
         }
 
         return true;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'motorista_upsert.js',
-            'validarCPF',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("motorista_upsert.js", "validarCPF", error);
         return false;
     }
 }
```
