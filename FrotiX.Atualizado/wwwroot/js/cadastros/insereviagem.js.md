# wwwroot/js/cadastros/insereviagem.js

**Mudanca:** GRANDE | **+945** linhas | **-1069** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/insereviagem.js
+++ ATUAL: wwwroot/js/cadastros/insereviagem.js
@@ -1,584 +1,594 @@
 $(document).ready(function () {
-    var viagemId = document.getElementById('txtViagemId').value;
-
-    console.log('viagemId: ' + viagemId);
-
-    if (
-        viagemId != null &&
-        viagemId != '00000000-0000-0000-0000-000000000000'
-    ) {
-        $.ajax({
-            type: 'GET',
+
+    var viagemId = document.getElementById("txtViagemId").value;
+
+    console.log("viagemId: " + viagemId);
+
+    if (viagemId != null && viagemId != '00000000-0000-0000-0000-000000000000')
+    {
+        $.ajax({
+            type: "GET",
             url: '/api/Agenda/RecuperaViagem',
             data: { id: viagemId },
-            contentType: 'application/json',
-            dataType: 'json',
+            contentType: "application/json",
+            dataType: "json",
             success: function (response) {
+
                 ExibeViagem(response.data);
-            },
+
+            }
         });
+
     }
 });
 
 function ExibeViagem(viagem) {
 
-    document.getElementById('ddtFinalidade').ej2_instances[0].value =
-        viagem.finalidade;
-    document.getElementById('ddtFinalidade').ej2_instances[0].text =
-        viagem.finalidade;
+    document.getElementById("ddtFinalidade").ej2_instances[0].value = viagem.finalidade;
+    document.getElementById("ddtFinalidade").ej2_instances[0].text = viagem.finalidade;
 
     if (viagem.eventoId != null) {
-        var ddtEventos = document.getElementById('ddtEventos').ej2_instances[0];
+        var ddtEventos = document.getElementById("ddtEventos").ej2_instances[0];
         ddtEventos.enabled = true;
         ddtEventos.value = [viagem.eventoId];
-        document.getElementById('btnEvento').style.display = 'block';
-        $(
-            '.esconde-diveventos',
-        ).show();
-    } else {
-        var ddtEventos = document.getElementById('ddtEventos').ej2_instances[0];
+        document.getElementById("btnEvento").style.display = "block";
+        $(".esconde-diveventos").show();
+    }
+    else {
+        var ddtEventos = document.getElementById("ddtEventos").ej2_instances[0];
         ddtEventos.enabled = false;
-        document.getElementById('btnEvento').style.display = 'none';
-        $(
-            '.esconde-diveventos',
-        ).hide();
-    }
-
-    document.getElementById('ddtSetor').ej2_instances[0].value = [
-        viagem.setorSolicitanteId,
-    ];
-
-    document.getElementById('ddtCombustivelInicial').ej2_instances[0].value = [
-        viagem.combustivelInicial,
-    ];
-    document.getElementById('ddtCombustivelFinal').ej2_instances[0].value = [
-        viagem.combustivelFinal,
-    ];
+        document.getElementById("btnEvento").style.display = "none";
+        $(".esconde-diveventos").hide();
+    }
+
+    document.getElementById("ddtSetor").ej2_instances[0].value = [viagem.setorSolicitanteId];
+
+    document.getElementById("ddtCombustivelInicial").ej2_instances[0].value = [viagem.combustivelInicial];
+    document.getElementById("ddtCombustivelFinal").ej2_instances[0].value = [viagem.combustivelFinal];
 
     if (viagem.status === 'Realizada' || viagem.status === 'Cancelada') {
 
-        $('#divPainel :input').each(function () {
-            $(this).prop('disabled', true);
+        $("#divPainel :input").each(function () {
+            $(this).prop("disabled", true);
         });
 
-        var rte = document.getElementById('rte').ej2_instances[0];
+        var rte = document.getElementById("rte").ej2_instances[0];
         rte.enabled = false;
-        var cmbMotorista =
-            document.getElementById('cmbMotorista').ej2_instances[0];
+        var cmbMotorista = document.getElementById("cmbMotorista").ej2_instances[0];
         cmbMotorista.enabled = false;
-        var cmbVeiculo = document.getElementById('cmbVeiculo').ej2_instances[0];
+        var cmbVeiculo = document.getElementById("cmbVeiculo").ej2_instances[0];
         cmbVeiculo.enabled = false;
-        var cmbRequisitante =
-            document.getElementById('cmbRequisitante').ej2_instances[0];
+        var cmbRequisitante = document.getElementById("cmbRequisitante").ej2_instances[0];
         cmbRequisitante.enabled = false;
-        var ddtSetor = document.getElementById('ddtSetor').ej2_instances[0];
+        var ddtSetor = document.getElementById("ddtSetor").ej2_instances[0];
         ddtSetor.enabled = false;
-        var ddtCombustivelInicial = document.getElementById(
-            'ddtCombustivelInicial',
-        ).ej2_instances[0];
+        var ddtCombustivelInicial = document.getElementById("ddtCombustivelInicial").ej2_instances[0];
         ddtCombustivelInicial.enabled = false;
-        var ddtCombustivelFinal = document.getElementById('ddtCombustivelFinal')
-            .ej2_instances[0];
+        var ddtCombustivelFinal = document.getElementById("ddtCombustivelFinal").ej2_instances[0];
         ddtCombustivelFinal.enabled = false;
-        var ddtSetor = document.getElementById('ddtSetor').ej2_instances[0];
+        var ddtSetor = document.getElementById("ddtSetor").ej2_instances[0];
         ddtSetor.enabled = false;
-        document.getElementById('ddtFinalidade').ej2_instances[0].enabled =
-            false;
-        document.getElementById('ddtEventos').ej2_instances[0].enabled = false;
-        var cmdRequisitante = document.getElementById('btnRequisitante');
+        document.getElementById('ddtFinalidade').ej2_instances[0].enabled = false;
+        document.getElementById("ddtEventos").ej2_instances[0].enabled = false;
+        var cmdRequisitante = document.getElementById("btnRequisitante");
         cmdRequisitante.disabled = true;
-        var cmdSetor = document.getElementById('btnSetor');
+        var cmdSetor = document.getElementById("btnSetor");
         cmdSetor.disabled = true;
-        var cmdEvento = document.getElementById('btnEvento');
+        var cmdEvento = document.getElementById("btnEvento");
         cmdEvento.disabled = true;
 
-        document.getElementById('divSubmit').style.display = 'none';
-        $('#btnSubmit').hide();
-
-    }
+        document.getElementById("divSubmit").style.display = 'none';
+        $("#btnSubmit").hide();
+
+    }
+
 }
 
-$('#modalEvento')
-    .modal({
+    $("#modalEvento").modal({
         keyboard: true,
         backdrop: false,
         show: false,
-    })
-    .on('show.bs.modal', function (event) {})
-    .on('hide.bs.modal', function (event) {
-        var setores = document.getElementById('ddtSetorRequisitanteEvento')
-            .ej2_instances[0];
-        setores.value = '';
-        var requisitantes = document.getElementById('lstRequisitanteEvento')
-            .ej2_instances[0];
-        requisitantes.value = '';
-        $('#txtNome').val('');
-        $('#txtDescricao').val('');
-        $('#txtDataInicial').val('');
-        $('#txtDataFinal').val('');
+    }).on("show.bs.modal", function (event) {
+    }).on("hide.bs.modal", function (event) {
+        var setores = document.getElementById('ddtSetorRequisitanteEvento').ej2_instances[0];
+        setores.value = "";
+        var requisitantes = document.getElementById('lstRequisitanteEvento').ej2_instances[0];
+        requisitantes.value = "";
+        $("#txtNome").val('');
+        $("#txtDescricao").val('');
+        $("#txtDataInicial").val('');
+        $("#txtDataFinal").val('');
         $('.modal-backdrop').remove();
-        $(document.body).removeClass('modal-open');
-    });
-
-$('#modalRequisitante')
-    .modal({
+        $(document.body).removeClass("modal-open");
+    });
+
+    $("#modalRequisitante").modal({
         keyboard: true,
-        backdrop: 'static',
+        backdrop: "static",
         show: false,
-    })
-    .on('show.bs.modal', function (event) {})
-    .on('hide.bs.modal', function (event) {
-        var setores = document.getElementById('ddtSetorRequisitante')
-            .ej2_instances[0];
-        setores.value = '';
-        $('#txtPonto').val('');
-        $('#txtNome').val('');
-        $('#txtRamal').val('');
-        $('#txtEmail').val('');
+    }).on("show.bs.modal", function (event) {
+    }).on("hide.bs.modal", function (event) {
+        var setores = document.getElementById('ddtSetorRequisitante').ej2_instances[0];
+    setores.value = "";
+        $("#txtPonto").val('');
+        $("#txtNome").val('');
+        $("#txtRamal").val('');
+        $("#txtEmail").val('');
         $('.modal-backdrop').remove();
     });
 
-$('#modalSetor')
-    .modal({
+    $("#modalSetor").modal({
         keyboard: true,
-        backdrop: 'static',
+        backdrop: "static",
         show: false,
-    })
-    .on('show.bs.modal', function (event) {})
-    .on('hide.bs.modal', function (event) {
+    }).on("show.bs.modal", function (event) {
+    }).on("hide.bs.modal", function (event) {
         var setores = document.getElementById('ddtSetorPai').ej2_instances[0];
-        setores.value = '';
-        $('#txtSigla').val('');
-        $('#txtNomeSetor').val('');
-        $('#txtRamalSetor').val('');
-    });
-
-$('#txtNoFichaVistoria').focusout(function () {
-    if (document.getElementById('txtNoFichaVistoria').value === '') {
-        return;
-    }
-
-    var noFichaVistoria = document.getElementById('txtNoFichaVistoria').value;
-
-    $.ajax({
-        url: '/Viagens/Upsert?handler=VerificaFicha',
-        method: 'GET',
-        datatype: 'json',
-        data: { id: document.getElementById('txtNoFichaVistoria').value },
-        success: function (res) {
-            var maxFichaVistoria = [res.data];
-
-            console.log('NoFichaVistoria: ' + noFichaVistoria);
-            console.log('MAxFichaVistoria: ' + maxFichaVistoria);
-
-            if (noFichaVistoria > parseInt(maxFichaVistoria) + 100) {
-                swal({
-                    title: 'Alerta na Ficha de Vistoria',
-                    text: 'O número inserido difere em +100 da última Ficha inserida!',
-                    icon: 'warning',
-                    buttons: true,
-                    dangerMode: true,
-                    buttons: {
-                        ok: 'Ok',
-                    },
-                });
-
-                return;
-            }
-
-            if (noFichaVistoria < parseInt(maxFichaVistoria) - 100) {
-                swal({
-                    title: 'Alerta na Ficha de Vistoria',
-                    text: 'O número inserido difere em -100 da última Ficha inserida!',
-                    icon: 'warning',
-                    buttons: true,
-                    dangerMode: true,
-                    buttons: {
-                        ok: 'Ok',
-                    },
-                });
-
-                return;
-            }
-
-        },
-    });
-
-    $.ajax({
-        url: '/Viagens/Upsert?handler=FichaExistente',
-        method: 'GET',
-        datatype: 'json',
-        data: { id: document.getElementById('txtNoFichaVistoria').value },
-        success: function (res) {
-            var ExisteFicha = [res.data];
-
-            console.log('Data: ' + [res.data]);
-            console.log('ExisteFicha: ' + ExisteFicha);
-
-            if (ExisteFicha[0] === true) {
-                swal({
-                    title: 'Alerta na Ficha de Vistoria',
-                    text: 'Já existe uma Ficha inserida com esta numeração!',
-                    icon: 'warning',
-                    buttons: true,
-                    dangerMode: true,
-                    buttons: {
-                        ok: 'Ok',
-                    },
-                });
-
-                return;
-            }
-
-        },
-    });
-});
-
-$('#txtDataFinal').focusout(function () {
-    DataInicial = $('#txtDataInicial').val();
-    DataFinal = $('#txtDataFinal').val();
-
-    if (DataFinal === '') {
-        return;
-    }
-
-    if (DataFinal < DataInicial) {
-        $('#txtDataFinal').val('');
-        swal({
-            title: 'Erro na Data',
-            text: 'A data final deve ser maior que a inicial!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-    }
-});
-
-$('#txtDataInicial').focusout(function () {
-    DataInicial = $('#txtDataInicial').val();
-    DataFinal = $('#txtDataFinal').val();
-
-    if (DataInicial === '' || DataFinal === '') {
-        return;
-    }
-
-    if (DataInicial > DataFinal) {
-        $('#txtDataInicial').val('');
-        swal({
-            title: 'Erro na Data',
-            text: 'A data inicial deve ser menor que a final!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-    }
-});
-
-$('#txtHoraFinal').focusout(function () {
-    HoraInicial = $('#txtHoraInicial').val();
-    HoraFinal = $('#txtHoraFinal').val();
-    DataInicial = $('#txtDataInicial').val();
-    DataFinal = $('#txtDataFinal').val();
-
-    console.log(HoraInicial);
-    console.log(HoraFinal);
-    console.log(DataFinal);
-
-    if (DataFinal === '') {
-        $('#txtHoraFinal').val('');
-        swal({
-            title: 'Erro na Hora Final',
-            text: 'Preencha a Data Final para poder preencher a Hora Final!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-    }
-
-    if (HoraFinal < HoraInicial && DataInicial === DataFinal) {
-        $('#txtHoraFinal').val('');
-        swal({
-            title: 'Erro na Hora',
-            text: 'A hora final deve ser maior que a inicial!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-    }
-});
-
-$('#txtHoraInicial').focusout(function () {
-    HoraInicial = $('#txtHoraInicial').val();
-    HoraFinal = $('#txtHoraFinal').val();
-
-    console.log(HoraInicial);
-    console.log(HoraFinal);
-
-    if (HoraFinal === '') {
-        return;
-    }
-
-    if (HoraInicial > HoraFinal) {
-        $('#txtHoraInicial').val('');
-        swal({
-            title: 'Erro na Hora',
-            text: 'A hora inicial deve ser menor que a final!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-    }
-
-});
-
-$('#txtKmFinal').focusout(function () {
-    kmInicial = parseInt($('#txtKmInicial').val());
-    kmFinal = parseInt($('#txtKmFinal').val());
-
-    if (kmFinal < kmInicial) {
-        $('#txtKmFinal').val('');
-        swal({
-            title: 'Erro na Quilometragem',
-            text: 'A quilometragem final deve ser maior que a inicial!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-    }
-
-    if (kmFinal - kmInicial > 100) {
-        swal({
-            title: 'Alerta na Quilometragem',
-            text: 'A quilometragem final excede em 100km a inicial!',
-            icon: 'warning',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-    }
-});
-
-$('#txtKmInicial').focusout(function () {
-    kmInicial = parseInt($('#txtKmInicial').val());
-    kmFinal = parseInt($('#txtKmFinal').val());
-
-    if (kmInicial > kmFinal) {
-        $('#txtKmInicial').val('');
-        swal({
-            title: 'Erro na Quilometragem',
-            text: 'A quilometragem inicial deve ser menor que a final!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-    }
-});
-
-$('#txtKmInicial').focusout(function () {
-    if ($('#txtKmInicial').val() === '' || $('#txtKmInicial').val() === null) {
-        return;
-    }
-
-    kmInicial = parseInt($('#txtKmInicial').val());
-    kmAtual = parseInt($('#txtKmAtual').val());
-
-    console.log(kmInicial);
-
-    if (kmInicial < kmAtual) {
-        $('#txtKmInicial').val('');
-        swal({
-            title: 'Erro na Quilometragem',
-            text: 'A quilometragem inicial deve ser maior que a atual!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    if (kmInicial !== kmAtual) {
-
-        swal({
-            title: 'Erro na Quilometragem',
-            text: 'A quilometragem inicial não confere com a atual!',
-            icon: 'warning',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-});
-
-function toolbarClick(e) {
-    if (e.item.id == 'rte_toolbar_Image') {
-        var element = document.getElementById('rte_upload');
-        element.ej2_instances[0].uploading = function upload(args) {
-            args.currentRequest.setRequestHeader(
-                'XSRF-TOKEN',
-                document.getElementsByName('__RequestVerificationToken')[0]
-                    .value,
-            );
-        };
-    }
-}
-
-function PreencheListaEventos(novoeventoId) {
-    $.ajax({
-        url: '/Viagens/Upsert?handler=AJAXPreencheListaEventos',
-        method: 'GET',
-        datatype: 'json',
-        success: function (res) {
-            var eventoid = res.data[0].eventoId;
-            var nome = res.data[0].nome;
-
-            let EventoList = [{ EventoId: eventoid, Nome: nome }];
-
-            for (var i = 1; i < res.data.length; ++i) {
-                console.log(res.data[i].eventoId + res.data[i].nome);
-
-                eventoid = res.data[i].eventoId;
-                nome = res.data[i].nome;
-
-                let evento = { EventoId: eventoid, Nome: nome };
-                EventoList.push(evento);
-            }
-
-            console.log(EventoList);
-
-            document.getElementById(
-                'ddtEventos',
-            ).ej2_instances[0].fields.dataSource = EventoList;
-        },
-    });
-
-    document.getElementById('ddtEventos').ej2_instances[0].refresh();
-}
-
-function PreencheListaRequisitantes() {
-    $.ajax({
-        url: '/Viagens/Upsert?handler=AJAXPreencheListaRequisitantes',
-        method: 'GET',
-        datatype: 'json',
-        success: function (res) {
-            var requisitanteid = res.data[0].requisitanteId;
-            var nomerequisitante = res.data[0].requisitante;
-
-            let RequisitanteList = [
-                {
-                    RequisitanteId: requisitanteid,
-                    Requisitante: nomerequisitante,
-                },
-            ];
-
-            for (var i = 1; i < res.data.length; ++i) {
-                console.log(
-                    res.data[i].requisitanteId + res.data[i].requisitante,
-                );
-
-                requisitanteid = res.data[i].requisitanteId;
-                nomerequisitante = res.data[i].requisitante;
-
-                let requisitante = {
-                    RequisitanteId: requisitanteid,
-                    Requisitante: nomerequisitante,
-                };
-                RequisitanteList.push(requisitante);
-            }
-
-            console.log(RequisitanteList);
-
-            document.getElementById(
-                'cmbRequisitante',
-            ).ej2_instances[0].fields.dataSource = RequisitanteList;
-        },
-    });
-
-    document.getElementById('cmbRequisitante').ej2_instances[0].refresh();
-}
-
-function PreencheListaSetores(SetorSolicitanteId) {
-    $.ajax({
-        url: '/Viagens/Upsert?handler=AJAXPreencheListaSetores',
-        method: 'GET',
-        datatype: 'json',
-        success: function (res) {
-            var setorSolicitanteId = res.data[0].setorSolicitanteId;
-            var setorPaiId = res.data[0].setorPaiId;
-            var nome = res.data[0].nome;
-            var hasChild = res.data[0].hasChild;
-
-            let SetorList = [
-                {
-                    SetorSolicitanteId: setorSolicitanteId,
-                    SetorPaiId: setorPaiId,
-                    Nome: nome,
-                    HasChild: hasChild,
-                },
-            ];
-
-            for (var i = 1; i < res.data.length; ++i) {
-                console.log(
-                    res.data[i].requisitanteId + res.data[i].requisitante,
-                );
-
-                setorSolicitanteId = res.data[i].setorSolicitanteId;
-                setorPaiId = res.data[i].setorPaiId;
-                nome = res.data[i].nome;
-                hasChild = res.data[i].hasChild;
-
-                let setor = {
-                    SetorSolicitanteId: setorSolicitanteId,
-                    SetorPaiId: setorPaiId,
-                    Nome: nome,
-                    HasChild: hasChild,
-                };
-                SetorList.push(setor);
-            }
-
-            console.log(SetorList);
-
-            document.getElementById(
-                'ddtSetor',
-            ).ej2_instances[0].fields.dataSource = SetorList;
-        },
-    });
-
-    document.getElementById('ddtSetor').ej2_instances[0].refresh();
-    var strSetor = String(SetorSolicitanteId);
-    document.getElementById('ddtSetor').ej2_instances[0].value = [strSetor];
-}
-
-function RequisitanteValueChange() {
-    var ddTreeObj = document.getElementById('cmbRequisitante').ej2_instances[0];
+        setores.value = "";
+        $("#txtSigla").val('');
+        $("#txtNomeSetor").val('');
+        $("#txtRamalSetor").val('');
+    });
+
+    $("#txtNoFichaVistoria").focusout(function () {
+
+        if (document.getElementById("txtNoFichaVistoria").value === '') {
+            return;
+        }
+
+        var noFichaVistoria = document.getElementById("txtNoFichaVistoria").value;
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=VerificaFicha",
+            method: "GET",
+            datatype: "json",
+            data: { id: document.getElementById("txtNoFichaVistoria").value },
+            success: function (res) {
+
+                var maxFichaVistoria = [res.data];
+
+                console.log("NoFichaVistoria: " + noFichaVistoria);
+                console.log("MAxFichaVistoria: " + maxFichaVistoria);
+
+                if ((noFichaVistoria > (parseInt(maxFichaVistoria) + 100))) {
+                    swal({
+                        title: "Alerta na Ficha de Vistoria",
+                        text: "O número inserido difere em +100 da última Ficha inserida!",
+                        icon: "warning",
+                        buttons: true,
+                        dangerMode: true,
+                        buttons: {
+                            ok: "Ok"
+                        }
+                    })
+
+                    return;
+                }
+
+                if ((noFichaVistoria < (parseInt(maxFichaVistoria) - 100))) {
+                    swal({
+                        title: "Alerta na Ficha de Vistoria",
+                        text: "O número inserido difere em -100 da última Ficha inserida!",
+                        icon: "warning",
+                        buttons: true,
+                        dangerMode: true,
+                        buttons: {
+                            ok: "Ok"
+                        }
+                    })
+
+                    return;
+                }
+
+            }
+        })
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=FichaExistente",
+            method: "GET",
+            datatype: "json",
+            data: { id: document.getElementById("txtNoFichaVistoria").value },
+            success: function (res) {
+
+                var ExisteFicha = [res.data];
+
+                console.log("Data: " + [res.data]);
+                console.log("ExisteFicha: " + ExisteFicha);
+
+                if (ExisteFicha[0] === true) {
+                    swal({
+                        title: "Alerta na Ficha de Vistoria",
+                        text: "Já existe uma Ficha inserida com esta numeração!",
+                        icon: "warning",
+                        buttons: true,
+                        dangerMode: true,
+                        buttons: {
+                            ok: "Ok"
+                        }
+                    })
+
+                    return;
+                }
+
+            }
+        })
+
+    });
+
+    $("#txtDataFinal").focusout(function () {
+
+        DataInicial = $("#txtDataInicial").val();
+        DataFinal = $("#txtDataFinal").val();
+
+        if (DataFinal === '') {
+            return;
+        }
+
+        if ((DataFinal < DataInicial)) {
+            $("#txtDataFinal").val('');
+            swal({
+                title: "Erro na Data",
+                text: "A data final deve ser maior que a inicial!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+
+        }
+    });
+
+    $("#txtDataInicial").focusout(function () {
+
+        DataInicial = $("#txtDataInicial").val();
+        DataFinal = $("#txtDataFinal").val();
+
+        if (DataInicial === '' || DataFinal === '') {
+            return;
+        }
+
+        if ((DataInicial > DataFinal)) {
+            $("#txtDataInicial").val('');
+            swal({
+                title: "Erro na Data",
+                text: "A data inicial deve ser menor que a final!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+
+        }
+    });
+
+    $("#txtHoraFinal").focusout(function () {
+
+        HoraInicial = $("#txtHoraInicial").val();
+        HoraFinal = $("#txtHoraFinal").val();
+        DataInicial = $("#txtDataInicial").val();
+        DataFinal = $("#txtDataFinal").val();
+
+        console.log(HoraInicial);
+        console.log(HoraFinal);
+        console.log(DataFinal);
+
+        if (DataFinal === '') {
+            $("#txtHoraFinal").val('');
+            swal({
+                title: "Erro na Hora Final",
+                text: "Preencha a Data Final para poder preencher a Hora Final!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+        }
+
+        if ((HoraFinal < HoraInicial) && (DataInicial === DataFinal)) {
+            $("#txtHoraFinal").val('');
+            swal({
+                title: "Erro na Hora",
+                text: "A hora final deve ser maior que a inicial!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+
+        }
+    });
+
+    $("#txtHoraInicial").focusout(function () {
+
+        HoraInicial = $("#txtHoraInicial").val();
+        HoraFinal = $("#txtHoraFinal").val();
+
+        console.log(HoraInicial);
+        console.log(HoraFinal);
+
+        if (HoraFinal === '') {
+            return;
+        }
+
+        if (HoraInicial > HoraFinal) {
+            $("#txtHoraInicial").val('');
+            swal({
+                title: "Erro na Hora",
+                text: "A hora inicial deve ser menor que a final!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+
+        }
+
+    });
+
+    $("#txtKmFinal").focusout(function () {
+
+        kmInicial = parseInt($("#txtKmInicial").val());
+        kmFinal = parseInt($("#txtKmFinal").val());
+
+        if (kmFinal < kmInicial) {
+            $("#txtKmFinal").val('');
+            swal({
+                title: "Erro na Quilometragem",
+                text: "A quilometragem final deve ser maior que a inicial!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+
+        }
+
+        if ((kmFinal - kmInicial) > 100) {
+            swal({
+                title: "Alerta na Quilometragem",
+                text: "A quilometragem final excede em 100km a inicial!",
+                icon: "warning",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+
+        }
+
+    });
+
+    $("#txtKmInicial").focusout(function () {
+
+        kmInicial = parseInt($("#txtKmInicial").val());
+        kmFinal = parseInt($("#txtKmFinal").val());
+
+        if (kmInicial > kmFinal) {
+            $("#txtKmInicial").val('');
+            swal({
+                title: "Erro na Quilometragem",
+                text: "A quilometragem inicial deve ser menor que a final!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+
+        }
+    });
+
+    $("#txtKmInicial").focusout(function () {
+
+        if ($("#txtKmInicial").val() === '' || $("#txtKmInicial").val() === null) {
+            return;
+        }
+
+        kmInicial = parseInt($("#txtKmInicial").val());
+        kmAtual = parseInt($("#txtKmAtual").val());
+
+        console.log(kmInicial);
+
+        if (kmInicial < kmAtual) {
+            $("#txtKmInicial").val('');
+            swal({
+                title: "Erro na Quilometragem",
+                text: "A quilometragem inicial deve ser maior que a atual!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        if (kmInicial !== kmAtual) {
+
+            swal({
+                title: "Erro na Quilometragem",
+                text: "A quilometragem inicial não confere com a atual!",
+                icon: "warning",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+    });
+
+    function toolbarClick(e) {
+        if (e.item.id == "rte_toolbar_Image") {
+            var element = document.getElementById('rte_upload')
+            element.ej2_instances[0].uploading = function upload(args) {
+                args.currentRequest.setRequestHeader('XSRF-TOKEN', document.getElementsByName('__RequestVerificationToken')[0].value);
+            }
+        }
+    }
+
+    function PreencheListaEventos(novoeventoId) {
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=AJAXPreencheListaEventos",
+            method: "GET",
+            datatype: "json",
+            success: function (res) {
+
+                var eventoid = res.data[0].eventoId;
+                var nome = res.data[0].nome;
+
+                let EventoList = [{ "EventoId": eventoid, "Nome": nome }];
+
+                for (var i = 1; i < res.data.length; ++i) {
+                    console.log(res.data[i].eventoId + res.data[i].nome);
+
+                    eventoid = res.data[i].eventoId;
+                    nome = res.data[i].nome;
+
+                    let evento = { EventoId: eventoid, Nome: nome }
+                    EventoList.push(evento);
+
+                }
+
+                console.log(EventoList);
+
+                document.getElementById("ddtEventos").ej2_instances[0].fields.dataSource = EventoList;
+
+            }
+        })
+
+        document.getElementById("ddtEventos").ej2_instances[0].refresh();
+
+    }
+
+    function PreencheListaRequisitantes() {
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=AJAXPreencheListaRequisitantes",
+            method: "GET",
+            datatype: "json",
+            success: function (res) {
+
+                var requisitanteid = res.data[0].requisitanteId;
+                var nomerequisitante = res.data[0].requisitante;
+
+                let RequisitanteList = [{ "RequisitanteId": requisitanteid, "Requisitante": nomerequisitante }];
+
+                for (var i = 1; i < res.data.length; ++i) {
+                    console.log(res.data[i].requisitanteId + res.data[i].requisitante);
+
+                    requisitanteid = res.data[i].requisitanteId;
+                    nomerequisitante = res.data[i].requisitante;
+
+                    let requisitante = { RequisitanteId: requisitanteid, Requisitante: nomerequisitante }
+                    RequisitanteList.push(requisitante);
+
+                }
+
+                console.log(RequisitanteList);
+
+                document.getElementById("cmbRequisitante").ej2_instances[0].fields.dataSource = RequisitanteList;
+
+            }
+        })
+
+        document.getElementById("cmbRequisitante").ej2_instances[0].refresh();
+    }
+
+    function PreencheListaSetores(SetorSolicitanteId) {
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=AJAXPreencheListaSetores",
+            method: "GET",
+            datatype: "json",
+            success: function (res) {
+
+                var setorSolicitanteId = res.data[0].setorSolicitanteId;
+                var setorPaiId = res.data[0].setorPaiId;
+                var nome = res.data[0].nome;
+                var hasChild = res.data[0].hasChild;
+
+                let SetorList = [{ "SetorSolicitanteId": setorSolicitanteId, "SetorPaiId": setorPaiId, "Nome": nome, "HasChild": hasChild }];
+
+                for (var i = 1; i < res.data.length; ++i) {
+                    console.log(res.data[i].requisitanteId + res.data[i].requisitante);
+
+                    setorSolicitanteId = res.data[i].setorSolicitanteId;
+                    setorPaiId = res.data[i].setorPaiId;
+                    nome = res.data[i].nome;
+                    hasChild = res.data[i].hasChild;
+
+                    let setor = { "SetorSolicitanteId": setorSolicitanteId, "SetorPaiId": setorPaiId, "Nome": nome, "HasChild": hasChild }
+                    SetorList.push(setor);
+                }
+
+                console.log(SetorList);
+
+                document.getElementById("ddtSetor").ej2_instances[0].fields.dataSource = SetorList;
+
+            }
+        })
+
+        document.getElementById("ddtSetor").ej2_instances[0].refresh();
+        var strSetor = String(SetorSolicitanteId);
+        document.getElementById("ddtSetor").ej2_instances[0].value = [strSetor];
+    }
+
+    function RequisitanteValueChange() {
+
+        var ddTreeObj = document.getElementById("cmbRequisitante").ej2_instances[0];
+
+        if (ddTreeObj.value === null) {
+            return;
+        }
+
+        var requisitanteid = String(ddTreeObj.value);
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=PegaSetor",
+            method: "GET",
+            datatype: "json",
+            data: { id: requisitanteid },
+            success: function (res) {
+                document.getElementById("ddtSetor").ej2_instances[0].value = [res.data];
+            }
+        })
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=PegaRamal",
+            method: "GET",
+            datatype: "json",
+            data: { id: requisitanteid },
+            success: function (res) {
+                var ramal = res.data;
+                var s = document.getElementById("txtRamalRequisitante");
+                s.value = ramal;
+            }
+        })
+
+    }
+
+function RequisitanteEventoValueChange() {
+
+    var ddTreeObj = document.getElementById("lstRequisitanteEvento").ej2_instances[0];
 
     if (ddTreeObj.value === null) {
         return;
@@ -587,697 +597,616 @@
     var requisitanteid = String(ddTreeObj.value);
 
     $.ajax({
-        url: '/Viagens/Upsert?handler=PegaSetor',
-        method: 'GET',
-        datatype: 'json',
+        url: "/Viagens/Upsert?handler=PegaSetor",
+        method: "GET",
+        datatype: "json",
         data: { id: requisitanteid },
         success: function (res) {
-            document.getElementById('ddtSetor').ej2_instances[0].value = [
-                res.data,
-            ];
-        },
-    });
-
-    $.ajax({
-        url: '/Viagens/Upsert?handler=PegaRamal',
-        method: 'GET',
-        datatype: 'json',
-        data: { id: requisitanteid },
-        success: function (res) {
-            var ramal = res.data;
-            var s = document.getElementById('txtRamalRequisitante');
-            s.value = ramal;
-        },
-    });
+            document.getElementById("ddtSetorRequisitanteEvento").ej2_instances[0].value = [res.data];
+        }
+    })
 
 }
 
-function RequisitanteEventoValueChange() {
-    var ddTreeObj = document.getElementById('lstRequisitanteEvento')
-        .ej2_instances[0];
-
-    if (ddTreeObj.value === null) {
-        return;
-    }
-
-    var requisitanteid = String(ddTreeObj.value);
-
-    $.ajax({
-        url: '/Viagens/Upsert?handler=PegaSetor',
-        method: 'GET',
-        datatype: 'json',
-        data: { id: requisitanteid },
-        success: function (res) {
-            document.getElementById(
-                'ddtSetorRequisitanteEvento',
-            ).ej2_instances[0].value = [res.data];
-        },
-    });
-}
-
-function MotoristaValueChange() {
-    var ddTreeObj = document.getElementById('cmbMotorista').ej2_instances[0];
-
-    console.log('Objeto Requisitante: ' + ddTreeObj);
-
-    if (ddTreeObj.value === null) {
-        return;
-    }
-
-    var motoristaid = String(ddTreeObj.value);
-
-    $.ajax({
-        url: '/Viagens/Upsert?handler=VerificaMotoristaViagem',
-        method: 'GET',
-        datatype: 'json',
-        data: { id: motoristaid },
-        success: function (res) {
-            var viajando = res.data;
-
-            console.log(viajando);
-
-            if (viajando) {
-                swal({
-                    title: 'Motorista em Viagem',
-                    text: 'Este motorista encontra-se em uma viagem não terminada!',
-                    icon: 'warning',
-                    buttons: true,
-                    dangerMode: true,
-                    buttons: {
-                        ok: 'Ok',
-                    },
-                });
-            }
-        },
-    });
-}
-
-function VeiculoValueChange() {
-    var ddTreeObj = document.getElementById('cmbVeiculo').ej2_instances[0];
-
-    console.log('Objeto Requisitante: ' + ddTreeObj);
-
-    if (ddTreeObj.value === null) {
-        return;
-    }
-
-    var veiculoid = String(ddTreeObj.value);
-
-    $.ajax({
-        url: '/Viagens/Upsert?handler=VerificaVeiculoViagem',
-        method: 'GET',
-        datatype: 'json',
-        data: { id: veiculoid },
-        success: function (res) {
-            var viajando = res.data;
-            console.log(viajando);
-            if (viajando) {
-                swal({
-                    title: 'Veículo em Viagem',
-                    text: 'Este veículo encontra-se em uma viagem não terminada!',
-                    icon: 'warning',
-                    buttons: true,
-                    dangerMode: true,
-                    buttons: {
-                        ok: 'Ok',
-                    },
-                });
-            }
-        },
-    });
-
-    $.ajax({
-        url: '/Viagens/Upsert?handler=PegaKmAtualVeiculo',
-        method: 'GET',
-        datatype: 'json',
-        data: { id: veiculoid },
-        success: function (res) {
-            var km = res.data;
-            var kmAtual = document.getElementById('txtKmAtual');
-            kmAtual.value = km;
-        },
-    });
-}
-
-function valueChange() {
-
-}
-
-function select(args) {
-
-}
-
-function ddtCombustivelChange() {
-
-}
-
-$('#btnInserirEvento').click(function (e) {
-    e.preventDefault();
-
-    if ($('#txtNomeDoEvento').val() === '') {
-        swal({
-            title: 'Atenção',
-            text: 'O Nome do Evento é obrigatório!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
+    function MotoristaValueChange() {
+
+        var ddTreeObj = document.getElementById("cmbMotorista").ej2_instances[0];
+
+        console.log("Objeto Requisitante: " + ddTreeObj);
+
+        if (ddTreeObj.value === null) {
+            return;
+        }
+
+        var motoristaid = String(ddTreeObj.value);
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=VerificaMotoristaViagem",
+            method: "GET",
+            datatype: "json",
+            data: { id: motoristaid },
+            success: function (res) {
+
+                var viajando = res.data;
+
+                console.log(viajando);
+
+                if (viajando) {
+                    swal({
+                        title: "Motorista em Viagem",
+                        text: "Este motorista encontra-se em uma viagem não terminada!",
+                        icon: "warning",
+                        buttons: true,
+                        dangerMode: true,
+                        buttons: {
+                            ok: "Ok"
+                        }
+                    })
+                }
+            }
+        })
+    }
+
+    function VeiculoValueChange() {
+
+        var ddTreeObj = document.getElementById("cmbVeiculo").ej2_instances[0];
+
+        console.log("Objeto Requisitante: " + ddTreeObj);
+
+        if (ddTreeObj.value === null) {
+            return;
+        }
+
+        var veiculoid = String(ddTreeObj.value);
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=VerificaVeiculoViagem",
+            method: "GET",
+            datatype: "json",
+            data: { id: veiculoid },
+            success: function (res) {
+                var viajando = res.data;
+                console.log(viajando);
+                if (viajando) {
+                    swal({
+                        title: "Veículo em Viagem",
+                        text: "Este veículo encontra-se em uma viagem não terminada!",
+                        icon: "warning",
+                        buttons: true,
+                        dangerMode: true,
+                        buttons: {
+                            ok: "Ok"
+                        }
+                    })
+                }
+            }
+        })
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=PegaKmAtualVeiculo",
+            method: "GET",
+            datatype: "json",
+            data: { id: veiculoid },
+            success: function (res) {
+                var km = res.data;
+                var kmAtual = document.getElementById("txtKmAtual");
+                kmAtual.value = km;
+            }
+        })
+
+    }
+
+    function valueChange() {
+
+    }
+
+    function select(args) {
+
+    }
+
+    function ddtCombustivelChange() {
+
+    }
+
+    $("#btnInserirEvento").click(function (e) {
+
+        e.preventDefault();
+
+        if ($("#txtNomeDoEvento").val() === "") {
+            swal({
+                title: 'Atenção',
+                text: "O Nome do Evento é obrigatório!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+
+        if ($("#txtDescricao").val() === "") {
+            swal({
+                title: 'Atenção',
+                text: "A Descrição do Evento é obrigatória!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+
+        if ($("#txtDataInicialEvento").val() === "") {
+            swal({
+                title: 'Atenção',
+                text: "A Data Inicial é obrigatória!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+
+        if ($("#txtDataFinalEvento").val() === "") {
+            swal({
+                title: 'Atenção',
+                text: "A Data Final é obrigatória!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+
+        if ($("#txtQtdPessoas").val() === "") {
+            swal({
+                title: 'Atenção',
+                text: "A Quantidade de Pessoas é obrigatória!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+
+        var setores = document.getElementById('ddtSetorRequisitanteEvento').ej2_instances[0];
+        if ((setores.value === null)) {
+            swal({
+                title: 'Atenção',
+                text: "O Setor do Requisitante é obrigatório!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+        var setorSolicitanteId = setores.value.toString();
+
+        var requisitantes = document.getElementById('lstRequisitanteEvento').ej2_instances[0];
+        if ((requisitantes.value === null)) {
+            swal({
+                title: 'Atenção',
+                text: "O Requisitante é obrigatório!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+        var requisitanteId = requisitantes.value.toString();
+
+        var objEvento = JSON.stringify({ "Nome": $('#txtNomeDoEvento').val(), "Descricao": $('#txtDescricaoEvento').val(), "SetorSolicitanteId": setorSolicitanteId, "RequisitanteId": requisitanteId, "QtdParticipantes": $('#txtQtdPessoas').val(), "DataInicial": moment(document.getElementById('txtDataInicialEvento').value).format("MM-DD-YYYY"), "DataFinal": moment(document.getElementById('txtDataFinalEvento').value).format("MM-DD-YYYY"), "Status": "1" });
+
+        console.log(objEvento);
+
+        $.ajax({
+            type: "post",
+            url: "/api/Viagem/AdicionarEvento",
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
+            data: objEvento,
+
+            success: function (data) {
+                toastr.success(data.message);
+                PreencheListaEventos(data.eventoId);
+                $("#modalEvento").hide();
+
             },
+            error: function (data) {
+                alert('error');
+                console.log(data);
+            }
         });
-        return;
-    }
-
-    if ($('#txtDescricao').val() === '') {
-        swal({
-            title: 'Atenção',
-            text: 'A Descrição do Evento é obrigatória!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
+    });
+
+    $("#btnInserirRequisitante").click(function (e) {
+
+        e.preventDefault();
+
+        if ($("#txtPonto").val() === "") {
+            swal({
+                title: 'Atenção',
+                text: "O Ponto do Requisitante é obrigatório!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+
+        if ($("#txtNome").val() === "") {
+            swal({
+                title: 'Atenção',
+                text: "O Nome do Requisitante é obrigatório!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+
+        if ($("#txtRamal").val() === "") {
+            swal({
+                title: 'Atenção',
+                text: "O Ramal do Requisitante é obrigatório!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+
+        var setores = document.getElementById('ddtSetorRequisitante').ej2_instances[0];
+        if ((setores.value === null)) {
+            swal({
+                title: 'Atenção',
+                text: "O Setor do Requisitante é obrigatório!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+
+        var setorSolicitanteId = setores.value.toString();
+
+        var objRequisitante = JSON.stringify({ "Nome": $('#txtNome').val(), "Ponto": $('#txtPonto').val(), "Ramal": $('#txtRamal').val(), "Email": $('#txtEmail').val(), "SetorSolicitanteId": setorSolicitanteId })
+
+        $.ajax({
+            type: "post",
+            url: "/api/Viagem/AdicionarRequisitante",
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
+            data: objRequisitante,
+
+            success: function (data) {
+
+                if (data.success) {
+                    toastr.success(data.message);
+                    let d = document.getElementById('modalRequisitante')
+                    d.style.display = "none"
+
+                    document.getElementById("cmbRequisitante").ej2_instances[0].addItem({ RequisitanteId: data.requisitanteid, Requisitante: $('#txtNome').val() + " - " + $('#txtPonto').val() }, 0)
+                    $("#modalRequisitante").hide();
+                    $('.modal-backdrop').remove();
+
+                    $('body').removeClass('modal-open');
+                    $("body").css("overflow", "auto");
+
+                    $("#btnFecharRequisitante").click();
+                    console.log("Passei por todas as etapas do Sucess do Adiciona Requisitante no AJAX");
+                }
+                else {
+                    toastr.error(data.message);
+                }
+
             },
+            error: function (data) {
+                AppToast.show('Vermelho', 'Já existe requisitante com este ponto/nome', 3000);
+                console.log(data);
+            }
         });
-        return;
-    }
-
-    if ($('#txtDataInicialEvento').val() === '') {
-        swal({
-            title: 'Atenção',
-            text: 'A Data Inicial é obrigatória!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
+    });
+
+    $("#btnInserirSetor").click(function (e) {
+
+        e.preventDefault();
+
+        if ($("#txtNomeSetor").val() === "") {
+            swal({
+                title: 'Atenção',
+                text: "O Nome do Setor é obrigatório!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+
+        if ($("#txtRamalSetor").val() === "") {
+            swal({
+                title: 'Atenção',
+                text: "O Ramal do Seor é obrigatório!",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    close: "Fechar",
+                }
+            });
+            return
+        };
+
+        setorPaiId = null;
+
+        if (document.getElementById('ddtSetorPai').ej2_instances[0].value !== '' && document.getElementById('ddtSetorPai').ej2_instances[0].value !== null) {
+            setorPaiId = document.getElementById('ddtSetorPai').ej2_instances[0].value.toString();
+        }
+
+        if ((setorPaiId === null)) {
+            var objSetor = JSON.stringify({ "Nome": $('#txtNomeSetor').val(), "Ramal": $('#txtRamalSetor').val(), "Sigla": $('#txtSigla').val() })
+        }
+        else {
+            var objSetor = JSON.stringify({ "Nome": $('#txtNomeSetor').val(), "Ramal": $('#txtRamalSetor').val(), "Sigla": $('#txtSigla').val(), "SetorPaiId": setorPaiId })
+        };
+
+        console.log(objSetor);
+
+        $.ajax({
+            type: "post",
+            url: "/api/Viagem/AdicionarSetor",
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
+            data: objSetor,
+
+            success: function (data) {
+                toastr.success(data.message);
+                PreencheListaSetores(data.setorId);
+                $("#modalSetor").hide();
+                let d = document.getElementById('modalSetor')
+                d.style.display = "none"
+                $("#modalSetor").hide();
+                $('.modal-backdrop').remove();
+                $('body').removeClass('modal-open');
+                $("body").css("overflow", "auto");
+
             },
+            error: function (data) {
+                AppToast.show('Vermelho', 'Erro ao adicionar setor', 3000);
+                console.log(data);
+            }
         });
-        return;
-    }
-
-    if ($('#txtDataFinalEvento').val() === '') {
-        swal({
-            title: 'Atenção',
-            text: 'A Data Final é obrigatória!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
-            },
+    });
+
+    $("#txtFile").change(function (event) {
+        var files = event.target.files; f
+        $("#imgViewer").attr("src", window.URL.createObjectURL(files[0]));
+        $("#painelfundo").css({
+            "padding-bottom:": "200px"
         });
-        return;
-    }
-
-    if ($('#txtQtdPessoas').val() === '') {
-        swal({
-            title: 'Atenção',
-            text: 'A Quantidade de Pessoas é obrigatória!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
-            },
-        });
-        return;
-    }
-
-    var setores = document.getElementById('ddtSetorRequisitanteEvento')
-        .ej2_instances[0];
-    if (setores.value === null) {
-        swal({
-            title: 'Atenção',
-            text: 'O Setor do Requisitante é obrigatório!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
-            },
-        });
-        return;
-    }
-    var setorSolicitanteId = setores.value.toString();
-
-    var requisitantes = document.getElementById('lstRequisitanteEvento')
-        .ej2_instances[0];
-    if (requisitantes.value === null) {
-        swal({
-            title: 'Atenção',
-            text: 'O Requisitante é obrigatório!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
-            },
-        });
-        return;
-    }
-    var requisitanteId = requisitantes.value.toString();
-
-    var objEvento = JSON.stringify({
-        Nome: $('#txtNomeDoEvento').val(),
-        Descricao: $('#txtDescricaoEvento').val(),
-        SetorSolicitanteId: setorSolicitanteId,
-        RequisitanteId: requisitanteId,
-        QtdParticipantes: $('#txtQtdPessoas').val(),
-        DataInicial: moment(
-            document.getElementById('txtDataInicialEvento').value,
-        ).format('MM-DD-YYYY'),
-        DataFinal: moment(
-            document.getElementById('txtDataFinalEvento').value,
-        ).format('MM-DD-YYYY'),
-        Status: '1',
-    });
-
-    console.log(objEvento);
-
-    $.ajax({
-        type: 'post',
-        url: '/api/Viagem/AdicionarEvento',
-        contentType: 'application/json; charset=utf-8',
-        dataType: 'json',
-        data: objEvento,
-
-        success: function (data) {
-            toastr.success(data.message);
-            PreencheListaEventos(data.eventoId);
-            $('#modalEvento').hide();
-        },
-        error: function (data) {
-            alert('error');
-            console.log(data);
-        },
-    });
-});
-
-$('#btnInserirRequisitante').click(function (e) {
-    e.preventDefault();
-
-    if ($('#txtPonto').val() === '') {
-        swal({
-            title: 'Atenção',
-            text: 'O Ponto do Requisitante é obrigatório!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
-            },
-        });
-        return;
-    }
-
-    if ($('#txtNome').val() === '') {
-        swal({
-            title: 'Atenção',
-            text: 'O Nome do Requisitante é obrigatório!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
-            },
-        });
-        return;
-    }
-
-    if ($('#txtRamal').val() === '') {
-        swal({
-            title: 'Atenção',
-            text: 'O Ramal do Requisitante é obrigatório!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
-            },
-        });
-        return;
-    }
-
-    var setores = document.getElementById('ddtSetorRequisitante')
-        .ej2_instances[0];
-    if (setores.value === null) {
-        swal({
-            title: 'Atenção',
-            text: 'O Setor do Requisitante é obrigatório!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
-            },
-        });
-        return;
-    }
-
-    var setorSolicitanteId = setores.value.toString();
-
-    var objRequisitante = JSON.stringify({
-        Nome: $('#txtNome').val(),
-        Ponto: $('#txtPonto').val(),
-        Ramal: $('#txtRamal').val(),
-        Email: $('#txtEmail').val(),
-        SetorSolicitanteId: setorSolicitanteId,
-    });
-
-    $.ajax({
-        type: 'post',
-        url: '/api/Viagem/AdicionarRequisitante',
-        contentType: 'application/json; charset=utf-8',
-        dataType: 'json',
-        data: objRequisitante,
-
-        success: function (data) {
-            if (data.success) {
-                toastr.success(data.message);
-                let d = document.getElementById('modalRequisitante');
-                d.style.display = 'none';
-
-                document
-                    .getElementById('cmbRequisitante')
-                    .ej2_instances[0].addItem(
-                        {
-                            RequisitanteId: data.requisitanteid,
-                            Requisitante:
-                                $('#txtNome').val() +
-                                ' - ' +
-                                $('#txtPonto').val(),
-                        },
-                        0,
-                    );
-                $('#modalRequisitante').hide();
-                $('.modal-backdrop').remove();
-
-                $('body').removeClass('modal-open');
-                $('body').css('overflow', 'auto');
-
-                $('#btnFecharRequisitante').click();
-                console.log(
-                    'Passei por todas as etapas do Sucess do Adiciona Requisitante no AJAX',
-                );
-            } else {
-                toastr.error(data.message);
-            }
-        },
-        error: function (data) {
-            toastr.error('Já existe um requisitante com este ponto/nome');
-            console.log(data);
-        },
-    });
-});
-
-$('#btnInserirSetor').click(function (e) {
-    e.preventDefault();
-
-    if ($('#txtNomeSetor').val() === '') {
-        swal({
-            title: 'Atenção',
-            text: 'O Nome do Setor é obrigatório!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
-            },
-        });
-        return;
-    }
-
-    if ($('#txtRamalSetor').val() === '') {
-        swal({
-            title: 'Atenção',
-            text: 'O Ramal do Seor é obrigatório!',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                close: 'Fechar',
-            },
-        });
-        return;
-    }
-
-    setorPaiId = null;
-
-    if (
-        document.getElementById('ddtSetorPai').ej2_instances[0].value !== '' &&
-        document.getElementById('ddtSetorPai').ej2_instances[0].value !== null
-    ) {
-        setorPaiId = document
-            .getElementById('ddtSetorPai')
-            .ej2_instances[0].value.toString();
-    }
-
-    if (setorPaiId === null) {
-        var objSetor = JSON.stringify({
-            Nome: $('#txtNomeSetor').val(),
-            Ramal: $('#txtRamalSetor').val(),
-            Sigla: $('#txtSigla').val(),
-        });
-    } else {
-        var objSetor = JSON.stringify({
-            Nome: $('#txtNomeSetor').val(),
-            Ramal: $('#txtRamalSetor').val(),
-            Sigla: $('#txtSigla').val(),
-            SetorPaiId: setorPaiId,
-        });
-    }
-
-    console.log(objSetor);
-
-    $.ajax({
-        type: 'post',
-        url: '/api/Viagem/AdicionarSetor',
-        contentType: 'application/json; charset=utf-8',
-        dataType: 'json',
-        data: objSetor,
-
-        success: function (data) {
-            toastr.success(data.message);
-            PreencheListaSetores(data.setorId);
-            $('#modalSetor').hide();
-            let d = document.getElementById('modalSetor');
-            d.style.display = 'none';
-            $('#modalSetor').hide();
-            $('.modal-backdrop').remove();
-            $('body').removeClass('modal-open');
-            $('body').css('overflow', 'auto');
-        },
-        error: function (data) {
-            alert('error');
-            console.log(data);
-        },
-    });
-});
-
-$('#txtFile').change(function (event) {
-    var files = event.target.files;
-    f;
-    $('#imgViewer').attr('src', window.URL.createObjectURL(files[0]));
-    $('#painelfundo').css({
-        'padding-bottom:': '200px',
-    });
-});
-
-$('#btnSubmit').click(function (event) {
-    event.preventDefault();
-
-    if (document.getElementById('txtNoFichaVistoria').value === '') {
-        swal({
-            title: 'Informação Ausente',
-            text: 'O número da Ficha de Vistoria é obrigatório',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    if (document.getElementById('txtDataInicial').value === '') {
-        swal({
-            title: 'Informação Ausente',
-            text: 'A Data Inicial é obrigatória',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    if (document.getElementById('txtHoraInicial').value === '') {
-        swal({
-            title: 'Informação Ausente',
-            text: 'A Hora Inicial é obrigatória',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    var lstFinalidade =
-        document.getElementById('ddtFinalidade').ej2_instances[0];
-    if (lstFinalidade.value[0] === null) {
-        swal({
-            title: 'Informação Ausente',
-            text: 'A Finalidade é obrigatória',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    if (document.getElementById('txtOrigem').value === '') {
-        swal({
-            title: 'Informação Ausente',
-            text: 'A Origem é obrigatória',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    var ddtMotorista = document.getElementById('cmbMotorista').ej2_instances[0];
-    if (ddtMotorista.value === null) {
-        swal({
-            title: 'Informação Ausente',
-            text: 'O Motorista é obrigatório',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    var cmbVeiculo = document.getElementById('cmbVeiculo').ej2_instances[0];
-    if (cmbVeiculo.value === null) {
-        swal({
-            title: 'Informação Ausente',
-            text: 'O Veículo é obrigatório',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    if (document.getElementById('txtKmInicial').value === '') {
-        swal({
-            title: 'Informação Ausente',
-            text: 'A Quilometragem Inicial é obrigatória',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    var ddtCombustivelInicial = document.getElementById('ddtCombustivelInicial')
-        .ej2_instances[0];
-
-    console.log(ddtCombustivelInicial.value);
-
-    if (
-        ddtCombustivelInicial.value !== 'tanquevazio' &&
-        ddtCombustivelInicial.value !== 'tanqueumquarto' &&
-        ddtCombustivelInicial.value !== 'tanquemeiotanque' &&
-        ddtCombustivelInicial.value !== 'tanquetresquartos' &&
-        ddtCombustivelInicial.value !== 'tanquecheio'
-    ) {
-        console.log('Compustível Inicial: ' + ddtCombustivelInicial.value);
-    }
-
-    if (ddtCombustivelInicial.value === null) {
-        swal({
-            title: 'Informação Ausente',
-            text: 'O Nível de Combustível Inicial é obrigatório',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    var cmbRequisitante =
-        document.getElementById('cmbRequisitante').ej2_instances[0];
-    if (cmbRequisitante.value[0] === null) {
-        swal({
-            title: 'Informação Ausente',
-            text: 'O Requisitante é obrigatório',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    if (document.getElementById('txtRamalRequisitante').value === '') {
-        swal({
-            title: 'Informação Ausente',
-            text: 'O Ramal do Requisitante é obrigatório',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    var ddtSetor = document.getElementById('ddtSetor').ej2_instances[0];
-    if (ddtSetor.value === null) {
-        swal({
-            title: 'Informação Ausente',
-            text: 'O Setor Solicitante é obrigatório',
-            icon: 'error',
-            buttons: true,
-            dangerMode: true,
-            buttons: {
-                ok: 'Ok',
-            },
-        });
-        return;
-    }
-
-    $('#btnSubmit').prop('disabled', true);
-
-    $('#btnEscondido').click();
-});
+    });
+
+    $("#btnSubmit").click(function (event) {
+
+        event.preventDefault()
+
+        if (document.getElementById("txtNoFichaVistoria").value === "") {
+            swal({
+                title: "Informação Ausente",
+                text: "O número da Ficha de Vistoria é obrigatório",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        if (document.getElementById("txtDataInicial").value === "") {
+            swal({
+                title: "Informação Ausente",
+                text: "A Data Inicial é obrigatória",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        if (document.getElementById("txtHoraInicial").value === "") {
+            swal({
+                title: "Informação Ausente",
+                text: "A Hora Inicial é obrigatória",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        var lstFinalidade = document.getElementById("ddtFinalidade").ej2_instances[0];
+        if (lstFinalidade.value[0] === null) {
+            swal({
+                title: "Informação Ausente",
+                text: "A Finalidade é obrigatória",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        if (document.getElementById("txtOrigem").value === "") {
+            swal({
+                title: "Informação Ausente",
+                text: "A Origem é obrigatória",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        var ddtMotorista = document.getElementById("cmbMotorista").ej2_instances[0];
+        if (ddtMotorista.value === null) {
+            swal({
+                title: "Informação Ausente",
+                text: "O Motorista é obrigatório",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        var cmbVeiculo = document.getElementById("cmbVeiculo").ej2_instances[0];
+        if (cmbVeiculo.value === null) {
+            swal({
+                title: "Informação Ausente",
+                text: "O Veículo é obrigatório",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        if (document.getElementById("txtKmInicial").value === "") {
+            swal({
+                title: "Informação Ausente",
+                text: "A Quilometragem Inicial é obrigatória",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        var ddtCombustivelInicial = document.getElementById("ddtCombustivelInicial").ej2_instances[0];
+
+        console.log(ddtCombustivelInicial.value);
+
+        if (ddtCombustivelInicial.value !== "tanquevazio" && ddtCombustivelInicial.value !== "tanqueumquarto" && ddtCombustivelInicial.value !== "tanquemeiotanque" && ddtCombustivelInicial.value !== "tanquetresquartos" && ddtCombustivelInicial.value !== "tanquecheio") {
+            console.log("Compustível Inicial: " + ddtCombustivelInicial.value);
+        }
+
+        if (ddtCombustivelInicial.value === null) {
+            swal({
+                title: "Informação Ausente",
+                text: "O Nível de Combustível Inicial é obrigatório",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        var cmbRequisitante = document.getElementById("cmbRequisitante").ej2_instances[0];
+        if (cmbRequisitante.value[0] === null) {
+            swal({
+                title: "Informação Ausente",
+                text: "O Requisitante é obrigatório",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        if (document.getElementById("txtRamalRequisitante").value === "") {
+            swal({
+                title: "Informação Ausente",
+                text: "O Ramal do Requisitante é obrigatório",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        var ddtSetor = document.getElementById("ddtSetor").ej2_instances[0];
+        if (ddtSetor.value === null) {
+            swal({
+                title: "Informação Ausente",
+                text: "O Setor Solicitante é obrigatório",
+                icon: "error",
+                buttons: true,
+                dangerMode: true,
+                buttons: {
+                    ok: "Ok"
+                }
+            })
+            return;
+        }
+
+        $("#btnSubmit").prop("disabled", true);
+
+        $("#btnEscondido").click();
+
+    });
```
