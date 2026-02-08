# wwwroot/js/cadastros/viagens_014.js

**Mudanca:** GRANDE | **+528** linhas | **-734** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/viagens_014.js
+++ ATUAL: wwwroot/js/cadastros/viagens_014.js
@@ -1,56 +1,41 @@
 $(document).ready(function () {
     try {
-
-        document
-            .getElementById('ddtCombustivelInicial')
-            .ej2_instances[0].showPopup();
-        document
-            .getElementById('ddtCombustivelInicial')
-            .ej2_instances[0].hidePopup();
-        console.log('Mostrei/Escondi Popup');
+        document.getElementById('ddtCombustivelInicial').ej2_instances[0].showPopup();
+        document.getElementById('ddtCombustivelInicial').ej2_instances[0].hidePopup();
+        console.log("Mostrei/Escondi Popup");
 
         ListaTodasViagens();
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'viagens_014.js',
-            'document.ready',
-            error,
-        );
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: AppInit | Método: document.ready | Erro: " + error.message);
     }
 });
 
 $(document).on('click', '.btn-cancelar', function () {
     try {
-
         var id = $(this).data('id');
 
         swal({
-            title: 'Você tem certeza que deseja cancelar esta viagem?',
-            text: 'Não será possível desfazer a operação!',
-            icon: 'warning',
+            title: "Você tem certeza que deseja cancelar esta viagem?",
+            text: "Não será possível desfazer a operação!",
+            icon: "warning",
             buttons: {
-                cancel: 'Desistir',
-                confirm: 'Cancelar a Viagem',
+                cancel: "Desistir",
+                confirm: "Cancelar a Viagem"
             },
-            dangerMode: true,
+            dangerMode: true
         }).then((willDelete) => {
-
             if (willDelete) {
                 try {
-
-                    var dataToPost = JSON.stringify({ ViagemId: id });
-
+                    var dataToPost = JSON.stringify({ 'ViagemId': id });
                     $.ajax({
                         url: '/api/Viagem/Cancelar',
-                        type: 'POST',
+                        type: "POST",
                         data: dataToPost,
-                        contentType: 'application/json; charset=utf-8',
-                        dataType: 'json',
-
+                        contentType: "application/json; charset=utf-8",
+                        dataType: "json",
                         success: function (data) {
                             try {
                                 if (data.success) {
-
                                     toastr.success(data.message);
                                     dataTable = $('#tblViagem').DataTable();
                                     dataTable.ajax.reload();
@@ -58,37 +43,20 @@
                                     toastr.error(data.message);
                                 }
                             } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'viagens_014.js',
-                                    'cancelar.ajax.success',
-                                    error,
-                                );
+                                Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: CancelarViagem | Método: success | Erro: " + error.message);
                             }
                         },
-
                         error: function (err) {
-                            Alerta.TratamentoErroComLinha(
-                                'viagens_014.js',
-                                'cancelar.ajax.error',
-                                err,
-                            );
-                        },
+                            Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: CancelarViagem | Método: error | Erro: " + err.message);
+                        }
                     });
                 } catch (error) {
-                    Alerta.Erro(
-                        '⚠️ Erro Sem Tratamento',
-                        'Classe: CancelarViagem | Método: then | Erro: ' +
-                            error.message,
-                    );
+                    Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: CancelarViagem | Método: then | Erro: " + error.message);
                 }
             }
         });
     } catch (error) {
-        Alerta.Erro(
-            '⚠️ Erro Sem Tratamento',
-            'Classe: CancelarViagem | Método: click.btn-cancelar | Erro: ' +
-                error.message,
-        );
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: CancelarViagem | Método: click.btn-cancelar | Erro: " + error.message);
     }
 });
 
@@ -97,19 +65,18 @@
         var dataTableViagens = $('#tblViagem').DataTable();
         var row = $(this).closest('tr');
         var data = dataTableViagens.row(row).data();
-        console.log('Linha: ' + row + ' - Dado: ' + data);
-
-        var RowNumber =
-            $(event.relatedTarget).closest('tr').find('td:eq(9)').text() - 1;
+        console.log("Linha: " + row + " - Dado: " + data);
+
+        var RowNumber = $(event.relatedTarget).closest("tr").find("td:eq(9)").text() - 1;
 
         defaultRTE.refreshUI();
         defaultRTEDescricao.refreshUI();
 
         var button = $(event.relatedTarget);
-        var viagemId = button.data('id');
+        var viagemId = button.data("id");
         console.log(viagemId);
         $('#txtId').attr('value', viagemId);
-        console.log('Row Number: ' + RowNumber);
+        console.log("Row Number: " + RowNumber);
 
         var data = $('#tblViagem').DataTable().row(RowNumber).data();
 
@@ -130,12 +97,7 @@
         var Descricao = data['descricao'];
         var ImagemOcorrencia = data['imagemOcorrencia'];
 
-        $('#h3Titulo').html(
-            'Finalizar a Viagem - Ficha nº ' +
-                noFichaVistoria +
-                ' de ' +
-                NomeMotorista,
-        );
+        $("#h3Titulo").html("Finalizar a Viagem - Ficha nº " + noFichaVistoria + " de " + NomeMotorista);
         console.log(DescricaoOcorrencia);
 
         $('#txtDataInicial').val(DataInicial);
@@ -145,284 +107,188 @@
         $('#txtDataFinal').val('');
         $('#txtHoraFinal').val('');
         $('#txtKmFinal').val('');
-        document.getElementById('txtKmFinal').value = '';
-
-        var nivelinicial = document.getElementById('ddtCombustivelInicial')
-            .ej2_instances[0];
+        document.getElementById("txtKmFinal").value = "";
+
+        var nivelinicial = document.getElementById('ddtCombustivelInicial').ej2_instances[0];
         nivelinicial.value = [CombustivelInicial];
         nivelinicial.enabled = false;
 
-        var nivelfinal = document.getElementById('ddtCombustivelFinal')
-            .ej2_instances[0];
-
-        $('#imgViewerItem').removeAttr('src');
+        var nivelfinal = document.getElementById('ddtCombustivelFinal').ej2_instances[0];
+
+        $('#imgViewerItem').removeAttr("src");
 
         if (!ImagemOcorrencia) {
-            $('#imgViewerItem').attr(
-                'src',
-                '/DadosEditaveis/ImagensOcorrencias/semimagem.jpg',
-            );
+            $('#imgViewerItem').attr('src', "/DadosEditaveis/ImagensOcorrencias/semimagem.jpg");
         } else {
-            $('#imgViewerItem').attr(
-                'src',
-                '/DadosEditaveis/ImagensOcorrencias/' + ImagemOcorrencia,
-            );
+            $('#imgViewerItem').attr('src', "/DadosEditaveis/ImagensOcorrencias/" + ImagemOcorrencia);
         }
 
         if (DataFinal != null) {
-            $('#txtDataFinal')
-                .removeAttr('type')
-                .val(DataFinal)
-                .attr('readonly', true);
+            $('#txtDataFinal').removeAttr("type").val(DataFinal).attr('readonly', true);
             $('#txtHoraFinal').val(HoraFinal).attr('readonly', true);
             document.getElementById('txtKmFinal').value = KmFinal;
             nivelfinal.value = [CombustivelFinal];
             nivelfinal.enabled = false;
 
-            var descricaoLista =
-                document.getElementById('rteDescricao').ej2_instances[0];
+            var descricaoLista = document.getElementById('rteDescricao').ej2_instances[0];
             descricaoLista.value = Descricao;
 
             $('#txtResumo').val(ResumoOcorrencia).attr('readonly', true);
-            $('#chkStatusDocumento')
-                .prop('checked', StatusDocumento)
-                .attr('readonly', true);
-            $('#chkStatusCartaoAbastecimento')
-                .prop('checked', StatusCartaoAbastecimento)
-                .attr('readonly', true);
-
-            var descricaoOcorrenciaLista =
-                document.getElementById('rteOcorrencias').ej2_instances[0];
+            $('#chkStatusDocumento').prop("checked", StatusDocumento).attr('readonly', true);
+            $('#chkStatusCartaoAbastecimento').prop("checked", StatusCartaoAbastecimento).attr('readonly', true);
+
+            var descricaoOcorrenciaLista = document.getElementById('rteOcorrencias').ej2_instances[0];
             descricaoOcorrenciaLista.value = DescricaoOcorrencia;
 
             $('#btnFinalizarViagem').hide();
         } else {
             const d = new Date();
-            $('#txtDataFinal')
-                .removeAttr('type')
-                .attr('type', 'date')
-                .val(
-                    `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`,
-                );
-            $('#txtHoraFinal').val(
-                `${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}`,
-            );
+            $('#txtDataFinal').removeAttr("type").attr('type', 'date').val(`${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`);
+            $('#txtHoraFinal').val(`${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}`);
             $('#btnFinalizarViagem').show();
 
-            $('#chkStatusDocumento')
-                .prop('checked', true)
-                .attr('readonly', false);
-            $('#chkStatusCartaoAbastecimento')
-                .prop('checked', true)
-                .attr('readonly', false);
-
-            var descricaoLista =
-                document.getElementById('rteDescricao').ej2_instances[0];
+            $('#chkStatusDocumento').prop("checked", true).attr('readonly', false);
+            $('#chkStatusCartaoAbastecimento').prop("checked", true).attr('readonly', false);
+
+            var descricaoLista = document.getElementById('rteDescricao').ej2_instances[0];
             descricaoLista.value = Descricao;
 
-            console.log('Viagem Não Realizada');
-        }
-    } catch (error) {
-        Alerta.Erro(
-            '⚠️ Erro Sem Tratamento',
-            'Classe: ModalFinalizaViagem | Método: shown.bs.modal | Erro: ' +
-                error.message,
-        );
+            console.log("Viagem Não Realizada");
+        }
+    } catch (error) {
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: ModalFinalizaViagem | Método: shown.bs.modal | Erro: " + error.message);
     }
 });
 
-$('#modalFinalizaViagem').on('hide.bs.modal', function () {
+$('#modalFinalizaViagem').on("hide.bs.modal", function () {
     try {
         $('#txtDataInicial, #txtHoraInicial, #txtKmInicial').val('');
-        document.getElementById(
-            'ddtCombustivelInicial',
-        ).ej2_instances[0].value = '';
-
-        $('#txtDataFinal, #txtHoraFinal, #txtKmFinal, #txtResumo')
-            .removeAttr('readonly')
-            .val('');
+        document.getElementById('ddtCombustivelInicial').ej2_instances[0].value = '';
+
+        $('#txtDataFinal, #txtHoraFinal, #txtKmFinal, #txtResumo').removeAttr("readonly").val('');
         $('#txtDataFinal').attr('type', 'date');
-        document.getElementById('ddtCombustivelFinal').ej2_instances[0].value =
-            '';
-        document.getElementById(
-            'ddtCombustivelFinal',
-        ).ej2_instances[0].enabled = true;
-
-        var descricaoOcorrenciaLista =
-            document.getElementById('rteOcorrencias').ej2_instances[0];
+        document.getElementById('ddtCombustivelFinal').ej2_instances[0].value = '';
+        document.getElementById('ddtCombustivelFinal').ej2_instances[0].enabled = true;
+
+        var descricaoOcorrenciaLista = document.getElementById('rteOcorrencias').ej2_instances[0];
         descricaoOcorrenciaLista.value = '';
         descricaoOcorrenciaLista.readonly = false;
 
-        var descricaoLista =
-            document.getElementById('rteDescricao').ej2_instances[0];
+        var descricaoLista = document.getElementById('rteDescricao').ej2_instances[0];
         descricaoLista.value = '';
         descricaoLista.readonly = false;
 
-        $('#chkStatusDocumento').attr('readonly', false).prop('checked', true);
-        $('#chkStatusCartaoAbastecimento')
-            .attr('readonly', false)
-            .prop('checked', true);
+        $('#chkStatusDocumento').attr('readonly', false).prop("checked", true);
+        $('#chkStatusCartaoAbastecimento').attr('readonly', false).prop("checked", true);
 
         $('#btnFinalizarViagem').attr('visible', 'true');
-        $('#imgViewerItem').removeAttr('src');
-
-    } catch (error) {
-        Alerta.Erro(
-            '⚠️ Erro Sem Tratamento',
-            'Classe: ModalFinalizaViagem | Método: hide.bs.modal | Erro: ' +
-                error.message,
-        );
+        $('#imgViewerItem').removeAttr("src");
+
+    } catch (error) {
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: ModalFinalizaViagem | Método: hide.bs.modal | Erro: " + error.message);
     }
 });
 
-$('#txtDataFinal').focusout(function () {
-    try {
-        var parts = $('#txtDataInicial').val().split('/');
+$("#txtDataFinal").focusout(function () {
+    try {
+        var parts = $("#txtDataInicial").val().split('/');
         var DataInicial = `${parts[2]}-${parts[1]}-${parts[0]}`;
-        var DataFinal = $('#txtDataFinal').val();
+        var DataFinal = $("#txtDataFinal").val();
 
         if (DataFinal < DataInicial) {
-            $('#txtDataFinal').val('');
-            Alerta.Erro(
-                'Erro na Data',
-                'A data final deve ser maior que a inicial!',
-            );
-        }
-    } catch (error) {
-        Alerta.Erro(
-            '⚠️ Erro Sem Tratamento',
-            'Classe: ValidadorData | Método: focusout.txtDataFinal | Erro: ' +
-                error.message,
-        );
+            $("#txtDataFinal").val('');
+            Alerta.Erro("Erro na Data", "A data final deve ser maior que a inicial!");
+        }
+    } catch (error) {
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: ValidadorData | Método: focusout.txtDataFinal | Erro: " + error.message);
     }
 });
 
-$('#txtHoraFinal').focusout(function () {
-    try {
-        var HoraInicial = $('#txtHoraInicial').val();
-        var HoraFinal = $('#txtHoraFinal').val();
-        var parts = $('#txtDataInicial').val().split('/');
+$("#txtHoraFinal").focusout(function () {
+    try {
+        var HoraInicial = $("#txtHoraInicial").val();
+        var HoraFinal = $("#txtHoraFinal").val();
+        var parts = $("#txtDataInicial").val().split('/');
         var DataInicial = `${parts[2]}-${parts[1]}-${parts[0]}`;
-        var DataFinal = $('#txtDataFinal').val();
+        var DataFinal = $("#txtDataFinal").val();
 
         if (DataFinal === '') {
-            $('#txtHoraFinal').val('');
-            Alerta.Erro(
-                'Erro na Hora Final',
-                'Preencha a Data Final para poder preencher a Hora Final!',
-            );
-        }
-
-        if (HoraFinal < HoraInicial && DataInicial === DataFinal) {
-            $('#txtHoraFinal').val('');
-            Alerta.Erro(
-                'Erro na Hora',
-                'A hora final deve ser maior que a inicial!',
-            );
-        }
-    } catch (error) {
-        Alerta.Erro(
-            '⚠️ Erro Sem Tratamento',
-            'Classe: ValidadorHora | Método: focusout.txtHoraFinal | Erro: ' +
-                error.message,
-        );
+            $("#txtHoraFinal").val('');
+            Alerta.Erro("Erro na Hora Final", "Preencha a Data Final para poder preencher a Hora Final!");
+        }
+
+        if ((HoraFinal < HoraInicial) && (DataInicial === DataFinal)) {
+            $("#txtHoraFinal").val('');
+            Alerta.Erro("Erro na Hora", "A hora final deve ser maior que a inicial!");
+        }
+    } catch (error) {
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: ValidadorHora | Método: focusout.txtHoraFinal | Erro: " + error.message);
     }
 });
 
-$('#txtKmFinal').focusout(function () {
-    try {
-        var kmInicial = parseInt($('#txtKmInicial').val());
-        var kmFinal = parseInt($('#txtKmFinal').val());
+$("#txtKmFinal").focusout(function () {
+    try {
+        var kmInicial = parseInt($("#txtKmInicial").val());
+        var kmFinal = parseInt($("#txtKmFinal").val());
 
         if (kmFinal < kmInicial) {
-            $('#txtKmFinal').val('');
-            Alerta.Erro(
-                'Erro na Quilometragem',
-                'A quilometragem final deve ser maior que a inicial!',
-            );
-        }
-
-        if (kmFinal - kmInicial > 100) {
-            Alerta.Erro(
-                'Alerta na Quilometragem',
-                'A quilometragem final excede em 100km a inicial!',
-            );
-        }
-    } catch (error) {
-        Alerta.Erro(
-            '⚠️ Erro Sem Tratamento',
-            'Classe: ValidadorKM | Método: focusout.txtKmFinal | Erro: ' +
-                error.message,
-        );
+            $("#txtKmFinal").val('');
+            Alerta.Erro("Erro na Quilometragem", "A quilometragem final deve ser maior que a inicial!");
+        }
+
+        if ((kmFinal - kmInicial) > 100) {
+            Alerta.Erro("Alerta na Quilometragem", "A quilometragem final excede em 100km a inicial!");
+        }
+    } catch (error) {
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: ValidadorKM | Método: focusout.txtKmFinal | Erro: " + error.message);
     }
 });
 
-$('#btnFinalizarViagem').click(async function (e) {
+$("#btnFinalizarViagem").click(async function (e) {
     try {
         e.preventDefault();
 
-        const DataFinal = $('#txtDataFinal').val();
+        const DataFinal = $("#txtDataFinal").val();
         if (DataFinal === '') {
-            Alerta.Erro('Erro na Data', 'A data final é obrigatória!');
+            Alerta.Erro("Erro na Data", "A data final é obrigatória!");
             return;
         }
 
         const datasOk = await validarDatasSimples();
         if (!datasOk) return;
 
-        const HoraFinal = $('#txtHoraFinal').val();
+        const HoraFinal = $("#txtHoraFinal").val();
         if (HoraFinal === '') {
-            Alerta.Erro('Erro na Hora', 'A hora final é obrigatória!');
+            Alerta.Erro("Erro na Hora", "A hora final é obrigatória!");
             return;
         }
 
-        const KmFinal = $('#txtKmFinal').val();
+        const KmFinal = $("#txtKmFinal").val();
         if (KmFinal === '') {
-            Alerta.Erro(
-                'Erro na Quilometragem',
-                'A quilometragem final é obrigatória!',
-            );
+            Alerta.Erro("Erro na Quilometragem", "A quilometragem final é obrigatória!");
             return;
         }
 
         const kmOk = await validarKmSimples();
         if (!kmOk) return;
 
-        var niveis = document.getElementById('ddtCombustivelFinal')
-            .ej2_instances[0];
-        if (niveis.value === null) {
-            Alerta.Erro(
-                'Atenção',
-                'O nível final de combustível é obrigatório!',
-            );
+        var niveis = document.getElementById('ddtCombustivelFinal').ej2_instances[0];
+        if ((niveis.value === null)) {
+            Alerta.Erro("Atenção", "O nível final de combustível é obrigatório!");
             return;
         }
 
         var nivelcombustivel = niveis.value.toString();
-        var descricaoOcorrencia =
-            document.getElementById('rteOcorrencias').ej2_instances[0];
-
-        if (
-            (descricaoOcorrencia.value || ImagemSelecionada) &&
-            !$('#txtResumo').val()
-        ) {
-            Alerta.Erro(
-                'Atenção',
-                'O Resumo da Ocorrência deve ser preenchido junto com a Descrição ou Imagem!',
-            );
+        var descricaoOcorrencia = document.getElementById('rteOcorrencias').ej2_instances[0];
+
+        if ((descricaoOcorrencia.value || ImagemSelecionada) && !$('#txtResumo').val()) {
+            Alerta.Erro("Atenção", "O Resumo da Ocorrência deve ser preenchido junto com a Descrição ou Imagem!");
             return;
         }
 
-        const statusDocumento = $('#chkStatusDocumento').prop('checked')
-            ? 'Entregue'
-            : 'Ausente';
-        const statusCartaoAbastecimento = $(
-            '#chkStatusCartaoAbastecimento',
-        ).prop('checked')
-            ? 'Entregue'
-            : 'Ausente';
-        const descricao =
-            document.getElementById('rteDescricao').ej2_instances[0];
+        const statusDocumento = $('#chkStatusDocumento').prop('checked') ? "Entregue" : "Ausente";
+        const statusCartaoAbastecimento = $('#chkStatusCartaoAbastecimento').prop('checked') ? "Entregue" : "Ausente";
+        const descricao = document.getElementById('rteDescricao').ej2_instances[0];
 
         const objViagem = JSON.stringify({
             ViagemId: $('#txtId').val(),
@@ -435,76 +301,61 @@
             StatusDocumento: statusDocumento,
             StatusCartaoAbastecimento: statusCartaoAbastecimento,
             Descricao: descricao.value,
-            ImagemOcorrencia: ImagemSelecionada,
+            ImagemOcorrencia: ImagemSelecionada
         });
 
         $.ajax({
-            type: 'POST',
-            url: '/api/Viagem/FinalizaViagem',
-            contentType: 'application/json; charset=utf-8',
-            dataType: 'json',
+            type: "POST",
+            url: "/api/Viagem/FinalizaViagem",
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
             data: objViagem,
             success: function (data) {
                 try {
                     toastr.success(data.message);
                     $('#tblViagem').DataTable().ajax.reload(null, false);
-                    $('#modalFinalizaViagem').hide();
-                    $('div').removeClass('modal-backdrop');
+                    $("#modalFinalizaViagem").hide();
+                    $("div").removeClass("modal-backdrop");
                     $('body').removeClass('modal-open');
                 } catch (error) {
-                    Alerta.Erro(
-                        '⚠️ Erro Sem Tratamento',
-                        'Classe: FinalizaViagem | Método: success | Erro: ' +
-                            error.message,
-                    );
+                    Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: FinalizaViagem | Método: success | Erro: " + error.message);
                 }
             },
             error: function (data) {
-                Alerta.Erro(
-                    '⚠️ Erro Sem Tratamento',
-                    'Classe: FinalizaViagem | Método: ajax.error | Erro: ' +
-                        data.message,
-                );
-            },
+                Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: FinalizaViagem | Método: ajax.error | Erro: " + data.message);
+            }
         });
     } catch (error) {
-        Alerta.Erro(
-            '⚠️ Erro Sem Tratamento',
-            'Classe: FinalizaViagem | Método: click.btnFinalizarViagem | Erro: ' +
-                error.message,
-        );
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: FinalizaViagem | Método: click.btnFinalizarViagem | Erro: " + error.message);
     }
 });
 
 function parseDate(d) {
     try {
         if (!d) return null;
-        if (d.includes('/')) {
-            const [dia, mes, ano] = d.split('/');
+        if (d.includes("/")) {
+            const [dia, mes, ano] = d.split("/");
             return new Date(ano, mes - 1, dia);
         }
-        if (d.includes('-')) {
-            const [ano, mes, dia] = d.split('-');
+        if (d.includes("-")) {
+            const [ano, mes, dia] = d.split("-");
             return new Date(ano, mes - 1, dia);
         }
         return null;
     } catch (error) {
-        Alerta.Erro(
-            '⚠️ Erro Sem Tratamento',
-            'Classe: Util | Método: parseDate | Erro: ' + error.message,
-        );
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: Util | Método: parseDate | Erro: " + error.message);
         return null;
     }
 }
 
 async function validarDatasSimples() {
     try {
-        const dataInicialStr = $('#txtDataInicial').val();
-        const dataFinalInput = $('#txtDataFinal');
+        const dataInicialStr = $("#txtDataInicial").val();
+        const dataFinalInput = $("#txtDataFinal");
         const dataFinalStr = dataFinalInput.val();
 
         if (dataInicialStr === '') {
-            Alerta.Erro('Erro na Data', 'A data inicial é obrigatória!');
+            Alerta.Erro("Erro na Data", "A data inicial é obrigatória!");
             return false;
         }
 
@@ -513,7 +364,7 @@
             const dtFinal = parseDate(dataFinalStr);
 
             if (!dtInicial || !dtFinal) {
-                Alerta.Erro('Erro na Data', 'Formato de data inválido!');
+                Alerta.Erro("Erro na Data", "Formato de data inválido!");
                 return false;
             }
 
@@ -524,21 +375,13 @@
 
             if (diferencaDias >= 5) {
                 const confirmado = await swal({
-                    title: 'Presta Atenção',
-                    text: 'A data final está 5 dias ou mais depois da data inicial. Tem certeza?',
-                    icon: 'warning',
+                    title: "Presta Atenção",
+                    text: "A data final está 5 dias ou mais depois da data inicial. Tem certeza?",
+                    icon: "warning",
                     buttons: {
-                        confirm: {
-                            text: 'Tem certeza?',
-                            value: true,
-                            visible: true,
-                        },
-                        cancel: {
-                            text: 'Me enganei!',
-                            value: false,
-                            visible: true,
-                        },
-                    },
+                        confirm: { text: "Tem certeza?", value: true, visible: true },
+                        cancel: { text: "Me enganei!", value: false, visible: true }
+                    }
                 });
 
                 if (!confirmado) {
@@ -550,59 +393,44 @@
 
         return true;
     } catch (error) {
-        Alerta.Erro(
-            '⚠️ Erro Sem Tratamento',
-            'Classe: ValidadorData | Método: validarDatasSimples | Erro: ' +
-                error.message,
-        );
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: ValidadorData | Método: validarDatasSimples | Erro: " + error.message);
         return false;
     }
 }
 
 async function validarKmSimples() {
     try {
-        const kmInicialInput = $('#txtKmInicial');
-        const kmFinalInput = $('#txtKmFinal');
+        const kmInicialInput = $("#txtKmInicial");
+        const kmFinalInput = $("#txtKmFinal");
 
         const kmInicial = kmInicialInput.val();
         const kmFinal = kmFinalInput.val();
 
-        if (kmInicial === '') {
-            Alerta.Erro(
-                'Informação Ausente',
-                'A Quilometragem Inicial é obrigatória',
-            );
+        if (kmInicial === "") {
+            Alerta.Erro("Informação Ausente", "A Quilometragem Inicial é obrigatória");
             return false;
         }
 
-        if (kmFinal !== '') {
-            const inicial = parseFloat(kmInicial.replace(',', '.'));
-            const final = parseFloat(kmFinal.replace(',', '.'));
+        if (kmFinal !== "") {
+            const inicial = parseFloat(kmInicial.replace(",", "."));
+            const final = parseFloat(kmFinal.replace(",", "."));
 
             if (!isNaN(inicial) && !isNaN(final)) {
                 const diferenca = final - inicial;
 
                 if (diferenca >= 100) {
                     const confirmado = await swal({
-                        title: 'Presta Atenção',
-                        text: 'A quilometragem final está 100 km ou mais acima da inicial. Tem certeza?',
-                        icon: 'warning',
+                        title: "Presta Atenção",
+                        text: "A quilometragem final está 100 km ou mais acima da inicial. Tem certeza?",
+                        icon: "warning",
                         buttons: {
-                            confirm: {
-                                text: 'Tem certeza?',
-                                value: true,
-                                visible: true,
-                            },
-                            cancel: {
-                                text: 'Me enganei!',
-                                value: false,
-                                visible: true,
-                            },
-                        },
+                            confirm: { text: "Tem certeza?", value: true, visible: true },
+                            cancel: { text: "Me enganei!", value: false, visible: true }
+                        }
                     });
 
                     if (!confirmado) {
-                        kmFinalInput.val('').focus();
+                        kmFinalInput.val("").focus();
                         return false;
                     }
                 }
@@ -611,11 +439,7 @@
 
         return true;
     } catch (error) {
-        Alerta.Erro(
-            '⚠️ Erro Sem Tratamento',
-            'Classe: ValidadorKM | Método: validarKmSimples | Erro: ' +
-                error.message,
-        );
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: ValidadorKM | Método: validarKmSimples | Erro: " + error.message);
         return false;
     }
 }
@@ -623,58 +447,47 @@
 function ListaTodasViagens() {
     try {
         $('#divViagens').LoadingScript('method_12', {
-            background_image: 'img/loading7.png',
-            main_width: 200,
-            animation_speed: 10,
-            additional_style: '',
-            after_element: false,
+            'background_image': 'img/loading7.png',
+            'main_width': 200,
+            'animation_speed': 10,
+            'additional_style': '',
+            'after_element': false
         });
 
-        var veiculoId = '';
+        var veiculoId = "";
         var veiculos = document.getElementById('lstVeiculos').ej2_instances[0];
         if (veiculos.value != null) veiculoId = veiculos.value;
 
-        var motoristaId = '';
-        var motoristas =
-            document.getElementById('lstMotorista').ej2_instances[0];
+        var motoristaId = "";
+        var motoristas = document.getElementById('lstMotorista').ej2_instances[0];
         if (motoristas.value != null) motoristaId = motoristas.value;
 
-        var eventoId = '';
+        var eventoId = "";
         var eventos = document.getElementById('lstEventos').ej2_instances[0];
         if (eventos.value != null) eventoId = eventos.value;
 
         var status = document.getElementById('lstStatus').ej2_instances[0];
-        var statusId = 'Aberta';
-
-        if (status.value === '' || status.value === null) {
-            if (
-                motoristas.value != null ||
-                veiculos.value != null ||
-                eventos.value != null ||
-                ($('#txtData').val() != null && $('#txtData').val() != '')
-            ) {
-                statusId = 'Todas';
+        var statusId = "Aberta";
+
+        if (status.value === "" || status.value === null) {
+            if ((motoristas.value != null || veiculos.value != null || eventos.value != null || ($('#txtData').val() != null && $('#txtData').val() != ''))) {
+                statusId = "Todas";
             }
         }
 
-        if (
-            motoristas.value == null &&
-            veiculos.value == null &&
-            eventos.value == null &&
-            ($('#txtData').val() === null || $('#txtData').val() === '')
-        ) {
+        if (motoristas.value == null && veiculos.value == null && eventos.value == null && ($('#txtData').val() === null || $('#txtData').val() === "")) {
             if (status.value != null) {
                 statusId = status.value;
             }
         }
 
-        var date = $('#txtData').val().split('-');
+        var date = $('#txtData').val().split("-");
         var day = date[2];
         var month = date[1];
         var year = date[0];
         var dataViagem = `${day}/${month}/${year}`;
 
-        var URLapi = '/api/viagem';
+        var URLapi = "/api/viagem";
 
         var dataTableViagens = $('#tblViagem').DataTable();
         dataTableViagens.destroy();
@@ -683,192 +496,170 @@
         DataTable.datetime('DD/MM/YYYY');
 
         dataTableViagens = $('#tblViagem').DataTable({
+
             autoWidth: false,
 
             dom: 'Bfrtip',
 
             lengthMenu: [
                 [10, 25, 50, -1],
-                ['10 linhas', '25 linhas', '50 linhas', 'Todas as Linhas'],
+                ['10 linhas', '25 linhas', '50 linhas', 'Todas as Linhas']
             ],
 
-            buttons: [
-                'pageLength',
-                'excel',
-                {
-                    extend: 'pdfHtml5',
-                    orientation: 'landscape',
-                    pageSize: 'LEGAL',
-                },
-            ],
-
-            order: [[0, 'desc']],
-
-            columnDefs: [
-                {
-                    targets: 0,
-                    className: 'text-center',
-                    width: '3%',
-                },
-                {
-                    targets: 1,
-                    className: 'text-center',
-                    width: '3%',
-                },
-                {
-                    targets: 2,
-                    className: 'text-center',
-                    width: '3%',
-                },
-                {
-                    targets: 3,
-                    className: 'text-left',
-                    width: '10%',
-                },
-                {
-                    targets: 4,
-                    className: 'text-left',
-                    width: '10%',
-                },
-                {
-                    targets: 5,
-                    className: 'text-left',
-                    width: '10%',
-                },
-                {
-                    targets: 6,
-                    className: 'text-left',
-                    width: '10%',
-                },
-                {
-                    targets: 7,
-                    className: 'text-center',
-                    width: '4%',
-                },
-                {
-                    targets: 8,
-                    className: 'text-center',
-                    width: '6%',
-                },
-                {
-                    targets: 9,
-                    className: 'text-center',
-                    width: '1%',
-                },
-                {
-                    targets: 10,
-                    className: 'text-center',
-                    visible: false,
-                },
-                {
-                    targets: 11,
-                    className: 'text-center',
-                    visible: false,
-                },
-                {
-                    targets: 12,
-                    className: 'text-center',
-                    visible: false,
-                },
-                {
-                    targets: 13,
-                    className: 'text-center',
-                    visible: false,
-                },
-                {
-                    targets: 14,
-                    className: 'text-center',
-                    visible: false,
-                },
-                {
-                    targets: 15,
-                    className: 'text-center',
-                    visible: false,
-                },
-                {
-                    targets: 16,
-                    className: 'text-center',
-                    visible: false,
-                },
-                {
-                    targets: 17,
-                    className: 'text-center',
-                    visible: false,
-                },
-                {
-                    targets: 18,
-                    className: 'text-center',
-                    visible: false,
-                },
-                {
-                    targets: 19,
-                    className: 'text-center',
-                    visible: false,
-                },
-                {
-                    targets: 20,
-                    className: 'text-center',
-                    visible: false,
-                },
-                {
-                    targets: 21,
-                    className: 'text-center',
-                    visible: false,
+            buttons: ['pageLength', 'excel', {
+                extend: 'pdfHtml5',
+                orientation: 'landscape',
+                pageSize: 'LEGAL'
+            }],
+
+            "order": [[0, 'desc']],
+
+            'columnDefs': [
+                {
+                    "targets": 0,
+                    "className": "text-center",
+                    "width": "3%",
+                },
+                {
+                    "targets": 1,
+                    "className": "text-center",
+                    "width": "3%",
+                },
+                {
+                    "targets": 2,
+                    "className": "text-center",
+                    "width": "3%",
+                },
+                {
+                    "targets": 3,
+                    "className": "text-left",
+                    "width": "10%",
+                },
+                {
+                    "targets": 4,
+                    "className": "text-left",
+                    "width": "10%",
+                },
+                {
+                    "targets": 5,
+                    "className": "text-left",
+                    "width": "10%",
+                },
+                {
+                    "targets": 6,
+                    "className": "text-left",
+                    "width": "10%",
+                },
+                {
+                    "targets": 7,
+                    "className": "text-center",
+                    "width": "4%",
+                },
+                {
+                    "targets": 8,
+                    "className": "text-center",
+                    "width": "6%",
+                },
+                {
+                    "targets": 9,
+                    "className": "text-center",
+                    "width": "1%",
+                },
+                {
+                    "targets": 10,
+                    "className": "text-center",
+                    "visible": false,
+                },
+                {
+                    "targets": 11,
+                    "className": "text-center",
+                    "visible": false,
+                },
+                {
+                    "targets": 12,
+                    "className": "text-center",
+                    "visible": false,
+                },
+                {
+                    "targets": 13,
+                    "className": "text-center",
+                    "visible": false,
+                },
+                {
+                    "targets": 14,
+                    "className": "text-center",
+                    "visible": false,
+                },
+                {
+                    "targets": 15,
+                    "className": "text-center",
+                    "visible": false,
+                },
+                {
+                    "targets": 16,
+                    "className": "text-center",
+                    "visible": false,
+                },
+                {
+                    "targets": 17,
+                    "className": "text-center",
+                    "visible": false,
+                },
+                {
+                    "targets": 18,
+                    "className": "text-center",
+                    "visible": false,
+                },
+                {
+                    "targets": 19,
+                    "className": "text-center",
+                    "visible": false,
+                },
+                {
+                    "targets": 20,
+                    "className": "text-center",
+                    "visible": false,
+                },
+                {
+                    "targets": 21,
+                    "className": "text-center",
+                    "visible": false,
                 },
             ],
             responsive: true,
-            ajax: {
-                url: URLapi,
-                type: 'GET',
-                data: {
-                    veiculoId: veiculoId,
-                    motoristaId: motoristaId,
-                    statusId: statusId,
-                    dataviagem: dataViagem,
-                    eventoId: eventoId,
-                },
-                datatype: 'json',
+            "ajax": {
+                "url": URLapi,
+                "type": "GET",
+                "data": { veiculoId: veiculoId, motoristaId: motoristaId, statusId: statusId, dataviagem: dataViagem, eventoId: eventoId },
+                "datatype": "json"
             },
 
-            columns: [
-                { data: 'noFichaVistoria' },
-                { data: 'dataInicial' },
-                { data: 'horaInicio' },
-                { data: 'nomeRequisitante' },
-                { data: 'nomeSetor' },
-                { data: 'nomeMotorista' },
-                { data: 'descricaoVeiculo' },
-                {
-                    data: 'status',
-                    render: function (data, type, row, meta) {
-                        if (row.status === 'Aberta')
-                            return (
-                                '<a href="javascript:void" class="updateStatusViagem btn btn-success btn-xs text-white" data-url="/api/Viagem/updateStatusViagem?Id=' +
-                                row.viagemId +
-                                '">Aberta</a>'
-                            );
-                        else if (row.status === 'Realizada')
-                            return (
-                                '<a href="javascript:void" class="updateStatusViagem btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Viagem/updateStatusViagem?Id=' +
-                                row.viagemId +
-                                '">Realizada</a>'
-                            );
+            "columns": [
+                { "data": "noFichaVistoria" },
+                { "data": "dataInicial" },
+                { "data": "horaInicio" },
+                { "data": "nomeRequisitante" },
+                { "data": "nomeSetor" },
+                { "data": "nomeMotorista" },
+                { "data": "descricaoVeiculo" },
+                {
+                    "data": "status",
+                    "render": function (data, type, row, meta) {
+                        if (row.status === "Aberta")
+                            return '<a href="javascript:void" class="updateStatusViagem btn btn-success btn-xs text-white" data-url="/api/Viagem/updateStatusViagem?Id=' + row.viagemId + '">Aberta</a>';
                         else
-                            return (
-                                '<a href="javascript:void" class="updateStatusViagem btn btn-danger btn-xs text-white text-bold" data-url="/api/Viagem/updateStatusViagem?Id=' +
-                                row.viagemId +
-                                '">Cancelada</a>'
-                            );
-                    },
-                },
-                {
-                    data: 'viagemId',
-                    render: function (data, type, row, meta) {
-                        const isAberta = row.status === 'Aberta';
-                        const disableClass = isAberta ? '' : 'btn-disabled';
-                        const disableTitle = isAberta
-                            ? ''
-                            : 'title="Ação não disponível"';
+                            if (row.status === "Realizada")
+                                return '<a href="javascript:void" class="updateStatusViagem btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Viagem/updateStatusViagem?Id=' + row.viagemId + '">Realizada</a>';
+                            else
+                                return '<a href="javascript:void" class="updateStatusViagem btn btn-danger btn-xs text-white text-bold" data-url="/api/Viagem/updateStatusViagem?Id=' + row.viagemId + '">Cancelada</a>';
+                    }
+                },
+                {
+                    "data": "viagemId",
+                    "render": function (data, type, row, meta) {
+                        const isAberta = row.status === "Aberta";
+                        const disableClass = isAberta ? "" : "btn-disabled";
+                        const disableTitle = isAberta ? "" : 'title="Ação não disponível"';
 
                         return `<div class="text-center">
                                 <a href="/Viagens/Upsert?id=${data}" class="btn btn-primary btn-xs text-white" aria-label="Editar a Viagem!" data-microtip-position="top" role="tooltip" style="cursor:pointer;">
@@ -887,233 +678,230 @@
                                     <i class="fa-light fa-print"></i>
                                 </a>
                             </div>`;
-                    },
-                },
-                { data: 'kmInicial' },
-                { data: 'combustivelInicial' },
-                { data: 'dataFinal' },
-                { data: 'horaFim' },
-                { data: 'kmFinal' },
-                { data: 'combustivelFinal' },
-                { data: 'resumoOcorrencia' },
-                { data: 'descricaoOcorrencia' },
-                { data: 'statusDocumento' },
-                { data: 'statusCartaoAbastecimento' },
-                { data: 'descricao' },
-                { data: 'imagemOcorrencia' },
+                    }
+                },
+                { "data": "kmInicial" },
+                { "data": "combustivelInicial" },
+                { "data": "dataFinal" },
+                { "data": "horaFim" },
+                { "data": "kmFinal" },
+                { "data": "combustivelFinal" },
+                { "data": "resumoOcorrencia" },
+                { "data": "descricaoOcorrencia" },
+                { "data": "statusDocumento" },
+                { "data": "statusCartaoAbastecimento" },
+                { "data": "descricao" },
+                { "data": "imagemOcorrencia" },
             ],
-            language: {
-                emptyTable: 'Nenhum registro encontrado',
-                info: 'Mostrando de _START_ até _END_ de _TOTAL_ registros',
-                infoEmpty: 'Mostrando 0 até 0 de 0 registros',
-                infoFiltered: '(Filtrados de _MAX_ registros)',
-                infoThousands: '.',
-                loadingRecords: 'Carregando...',
-                processing: 'Processando...',
-                zeroRecords: 'Nenhum registro encontrado',
-                search: 'Pesquisar',
-                paginate: {
-                    next: 'Próximo',
-                    previous: 'Anterior',
-                    first: 'Primeiro',
-                    last: 'Último',
-                },
-                aria: {
-                    sortAscending: ': Ordenar colunas de forma ascendente',
-                    sortDescending: ': Ordenar colunas de forma descendente',
-                },
-                select: {
-                    rows: {
-                        _: 'Selecionado %d linhas',
-                        1: 'Selecionado 1 linha',
-                    },
-                    cells: {
-                        1: '1 célula selecionada',
-                        _: '%d células selecionadas',
-                    },
-                    columns: {
-                        1: '1 coluna selecionada',
-                        _: '%d colunas selecionadas',
-                    },
-                },
-                buttons: {
-                    copySuccess: {
-                        1: 'Uma linha copiada com sucesso',
-                        _: '%d linhas copiadas com sucesso',
-                    },
-                    collection:
-                        'Coleção <span class="ui-button-icon-primary ui-icon ui-icon-triangle-1-s"><\/span>',
-                    colvis: 'Visibilidade da Coluna',
-                    colvisRestore: 'Restaurar Visibilidade',
-                    copy: 'Copiar',
-                    copyKeys:
-                        'Pressione ctrl ou u2318 + C para copiar os dados da tabela para a área de transferência do sistema. Para cancelar, clique nesta mensagem ou pressione Esc..',
-                    copyTitle: 'Copiar para a Área de Transferência',
-                    csv: 'CSV',
-                    excel: 'Excel',
-                    pageLength: {
-                        '-1': 'Mostrar todos os registros',
-                        _: 'Mostrar %d registros',
-                    },
-                    pdf: 'PDF',
-                    print: 'Imprimir',
-                },
-                autoFill: {
-                    cancel: 'Cancelar',
-                    fill: 'Preencher todas as células com',
-                    fillHorizontal: 'Preencher células horizontalmente',
-                    fillVertical: 'Preencher células verticalmente',
-                },
-                lengthMenu: 'Exibir _MENU_ resultados por página',
-                searchBuilder: {
-                    add: 'Adicionar Condição',
-                    button: {
-                        0: 'Construtor de Pesquisa',
-                        _: 'Construtor de Pesquisa (%d)',
-                    },
-                    clearAll: 'Limpar Tudo',
-                    condition: 'Condição',
-                    conditions: {
-                        date: {
-                            after: 'Depois',
-                            before: 'Antes',
-                            between: 'Entre',
-                            empty: 'Vazio',
-                            equals: 'Igual',
-                            not: 'Não',
-                            notBetween: 'Não Entre',
-                            notEmpty: 'Não Vazio',
+            "language": {
+                "emptyTable": "Nenhum registro encontrado",
+                "info": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
+                "infoEmpty": "Mostrando 0 até 0 de 0 registros",
+                "infoFiltered": "(Filtrados de _MAX_ registros)",
+                "infoThousands": ".",
+                "loadingRecords": "Carregando...",
+                "processing": "Processando...",
+                "zeroRecords": "Nenhum registro encontrado",
+                "search": "Pesquisar",
+                "paginate": {
+                    "next": "Próximo",
+                    "previous": "Anterior",
+                    "first": "Primeiro",
+                    "last": "Último"
+                },
+                "aria": {
+                    "sortAscending": ": Ordenar colunas de forma ascendente",
+                    "sortDescending": ": Ordenar colunas de forma descendente"
+                },
+                "select": {
+                    "rows": {
+                        "_": "Selecionado %d linhas",
+                        "1": "Selecionado 1 linha"
+                    },
+                    "cells": {
+                        "1": "1 célula selecionada",
+                        "_": "%d células selecionadas"
+                    },
+                    "columns": {
+                        "1": "1 coluna selecionada",
+                        "_": "%d colunas selecionadas"
+                    }
+                },
+                "buttons": {
+                    "copySuccess": {
+                        "1": "Uma linha copiada com sucesso",
+                        "_": "%d linhas copiadas com sucesso"
+                    },
+                    "collection": "Coleção <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"><\/span>",
+                    "colvis": "Visibilidade da Coluna",
+                    "colvisRestore": "Restaurar Visibilidade",
+                    "copy": "Copiar",
+                    "copyKeys": "Pressione ctrl ou u2318 + C para copiar os dados da tabela para a área de transferência do sistema. Para cancelar, clique nesta mensagem ou pressione Esc..",
+                    "copyTitle": "Copiar para a Área de Transferência",
+                    "csv": "CSV",
+                    "excel": "Excel",
+                    "pageLength": {
+                        "-1": "Mostrar todos os registros",
+                        "_": "Mostrar %d registros"
+                    },
+                    "pdf": "PDF",
+                    "print": "Imprimir"
+                },
+                "autoFill": {
+                    "cancel": "Cancelar",
+                    "fill": "Preencher todas as células com",
+                    "fillHorizontal": "Preencher células horizontalmente",
+                    "fillVertical": "Preencher células verticalmente"
+                },
+                "lengthMenu": "Exibir _MENU_ resultados por página",
+                "searchBuilder": {
+                    "add": "Adicionar Condição",
+                    "button": {
+                        "0": "Construtor de Pesquisa",
+                        "_": "Construtor de Pesquisa (%d)"
+                    },
+                    "clearAll": "Limpar Tudo",
+                    "condition": "Condição",
+                    "conditions": {
+                        "date": {
+                            "after": "Depois",
+                            "before": "Antes",
+                            "between": "Entre",
+                            "empty": "Vazio",
+                            "equals": "Igual",
+                            "not": "Não",
+                            "notBetween": "Não Entre",
+                            "notEmpty": "Não Vazio"
                         },
-                        number: {
-                            between: 'Entre',
-                            empty: 'Vazio',
-                            equals: 'Igual',
-                            gt: 'Maior Que',
-                            gte: 'Maior ou Igual a',
-                            lt: 'Menor Que',
-                            lte: 'Menor ou Igual a',
-                            not: 'Não',
-                            notBetween: 'Não Entre',
-                            notEmpty: 'Não Vazio',
+                        "number": {
+                            "between": "Entre",
+                            "empty": "Vazio",
+                            "equals": "Igual",
+                            "gt": "Maior Que",
+                            "gte": "Maior ou Igual a",
+                            "lt": "Menor Que",
+                            "lte": "Menor ou Igual a",
+                            "not": "Não",
+                            "notBetween": "Não Entre",
+                            "notEmpty": "Não Vazio"
                         },
-                        string: {
-                            contains: 'Contém',
-                            empty: 'Vazio',
-                            endsWith: 'Termina Com',
-                            equals: 'Igual',
-                            not: 'Não',
-                            notEmpty: 'Não Vazio',
-                            startsWith: 'Começa Com',
+                        "string": {
+                            "contains": "Contém",
+                            "empty": "Vazio",
+                            "endsWith": "Termina Com",
+                            "equals": "Igual",
+                            "not": "Não",
+                            "notEmpty": "Não Vazio",
+                            "startsWith": "Começa Com"
                         },
-                        array: {
-                            contains: 'Contém',
-                            empty: 'Vazio',
-                            equals: 'Igual à',
-                            not: 'Não',
-                            notEmpty: 'Não vazio',
-                            without: 'Não possui',
+                        "array": {
+                            "contains": "Contém",
+                            "empty": "Vazio",
+                            "equals": "Igual à",
+                            "not": "Não",
+                            "notEmpty": "Não vazio",
+                            "without": "Não possui"
+                        }
+                    },
+                    "data": "Data",
+                    "deleteTitle": "Excluir regra de filtragem",
+                    "logicAnd": "E",
+                    "logicOr": "Ou",
+                    "title": {
+                        "0": "Construtor de Pesquisa",
+                        "_": "Construtor de Pesquisa (%d)"
+                    },
+                    "value": "Valor",
+                    "leftTitle": "Critérios Externos",
+                    "rightTitle": "Critérios Internos"
+                },
+                "searchPanes": {
+                    "clearMessage": "Limpar Tudo",
+                    "collapse": {
+                        "0": "Painéis de Pesquisa",
+                        "_": "Painéis de Pesquisa (%d)"
+                    },
+                    "count": "{total}",
+                    "countFiltered": "{shown} ({total})",
+                    "emptyPanes": "Nenhum Painel de Pesquisa",
+                    "loadMessage": "Carregando Painéis de Pesquisa...",
+                    "title": "Filtros Ativos"
+                },
+                "thousands": ".",
+                "datetime": {
+                    "previous": "Anterior",
+                    "next": "Próximo",
+                    "hours": "Hora",
+                    "minutes": "Minuto",
+                    "seconds": "Segundo",
+                    "amPm": [
+                        "am",
+                        "pm"
+                    ],
+                    "unknown": "-",
+                    "months": {
+                        "0": "Janeiro",
+                        "1": "Fevereiro",
+                        "10": "Novembro",
+                        "11": "Dezembro",
+                        "2": "Março",
+                        "3": "Abril",
+                        "4": "Maio",
+                        "5": "Junho",
+                        "6": "Julho",
+                        "7": "Agosto",
+                        "8": "Setembro",
+                        "9": "Outubro"
+                    },
+                    "weekdays": [
+                        "Domingo",
+                        "Segunda-feira",
+                        "Terça-feira",
+                        "Quarta-feira",
+                        "Quinte-feira",
+                        "Sexta-feira",
+                        "Sábado"
+                    ]
+                },
+                "editor": {
+                    "close": "Fechar",
+                    "create": {
+                        "button": "Novo",
+                        "submit": "Criar",
+                        "title": "Criar novo registro"
+                    },
+                    "edit": {
+                        "button": "Editar",
+                        "submit": "Atualizar",
+                        "title": "Editar registro"
+                    },
+                    "error": {
+                        "system": "Ocorreu um erro no sistema (<a target=\"\\\" rel=\"nofollow\" href=\"\\\">Mais informações<\/a>)."
+                    },
+                    "multi": {
+                        "noMulti": "Essa entrada pode ser editada individualmente, mas não como parte do grupo",
+                        "restore": "Desfazer alterações",
+                        "title": "Multiplos valores",
+                        "info": "Os itens selecionados contêm valores diferentes para esta entrada. Para editar e definir todos os itens para esta entrada com o mesmo valor, clique ou toque aqui, caso contrário, eles manterão seus valores individuais."
+                    },
+                    "remove": {
+                        "button": "Remover",
+                        "confirm": {
+                            "_": "Tem certeza que quer deletar %d linhas?",
+                            "1": "Tem certeza que quer deletar 1 linha?"
                         },
-                    },
-                    data: 'Data',
-                    deleteTitle: 'Excluir regra de filtragem',
-                    logicAnd: 'E',
-                    logicOr: 'Ou',
-                    title: {
-                        0: 'Construtor de Pesquisa',
-                        _: 'Construtor de Pesquisa (%d)',
-                    },
-                    value: 'Valor',
-                    leftTitle: 'Critérios Externos',
-                    rightTitle: 'Critérios Internos',
-                },
-                searchPanes: {
-                    clearMessage: 'Limpar Tudo',
-                    collapse: {
-                        0: 'Painéis de Pesquisa',
-                        _: 'Painéis de Pesquisa (%d)',
-                    },
-                    count: '{total}',
-                    countFiltered: '{shown} ({total})',
-                    emptyPanes: 'Nenhum Painel de Pesquisa',
-                    loadMessage: 'Carregando Painéis de Pesquisa...',
-                    title: 'Filtros Ativos',
-                },
-                thousands: '.',
-                datetime: {
-                    previous: 'Anterior',
-                    next: 'Próximo',
-                    hours: 'Hora',
-                    minutes: 'Minuto',
-                    seconds: 'Segundo',
-                    amPm: ['am', 'pm'],
-                    unknown: '-',
-                    months: {
-                        0: 'Janeiro',
-                        1: 'Fevereiro',
-                        10: 'Novembro',
-                        11: 'Dezembro',
-                        2: 'Março',
-                        3: 'Abril',
-                        4: 'Maio',
-                        5: 'Junho',
-                        6: 'Julho',
-                        7: 'Agosto',
-                        8: 'Setembro',
-                        9: 'Outubro',
-                    },
-                    weekdays: [
-                        'Domingo',
-                        'Segunda-feira',
-                        'Terça-feira',
-                        'Quarta-feira',
-                        'Quinte-feira',
-                        'Sexta-feira',
-                        'Sábado',
-                    ],
-                },
-                editor: {
-                    close: 'Fechar',
-                    create: {
-                        button: 'Novo',
-                        submit: 'Criar',
-                        title: 'Criar novo registro',
-                    },
-                    edit: {
-                        button: 'Editar',
-                        submit: 'Atualizar',
-                        title: 'Editar registro',
-                    },
-                    error: {
-                        system: 'Ocorreu um erro no sistema (<a target="\\" rel="nofollow" href="\\">Mais informações<\/a>).',
-                    },
-                    multi: {
-                        noMulti:
-                            'Essa entrada pode ser editada individualmente, mas não como parte do grupo',
-                        restore: 'Desfazer alterações',
-                        title: 'Multiplos valores',
-                        info: 'Os itens selecionados contêm valores diferentes para esta entrada. Para editar e definir todos os itens para esta entrada com o mesmo valor, clique ou toque aqui, caso contrário, eles manterão seus valores individuais.',
-                    },
-                    remove: {
-                        button: 'Remover',
-                        confirm: {
-                            _: 'Tem certeza que quer deletar %d linhas?',
-                            1: 'Tem certeza que quer deletar 1 linha?',
-                        },
-                        submit: 'Remover',
-                        title: 'Remover registro',
-                    },
-                },
-                decimal: ',',
+                        "submit": "Remover",
+                        "title": "Remover registro"
+                    }
+                },
+                "decimal": ","
             },
-            width: '100%',
+            "width": "100%"
         });
 
         $('#divViagens').LoadingScript('destroy');
-    } catch (error) {
-        Alerta.Erro(
-            '⚠️ Erro Sem Tratamento',
-            'Classe: ListaViagens | Método: ListaTodasViagens | Erro: ' +
-                error.message,
-        );
+
+    } catch (error) {
+        Alerta.Erro("⚠️ Erro Sem Tratamento", "Classe: ListaViagens | Método: ListaTodasViagens | Erro: " + error.message);
     }
 }
```
