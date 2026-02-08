# wwwroot/js/cadastros/Glosa_001.js

**Mudanca:** GRANDE | **+386** linhas | **-386** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/Glosa_001.js
+++ ATUAL: wwwroot/js/cadastros/Glosa_001.js
@@ -1,160 +1,146 @@
 $(document).ready(function () {
-    try {
-
-        $('#status').on('change', function () {
-            try {
-
+    try
+    {
+        $("#status").on("change", function () {
+            try
+            {
                 var status = $(this).val();
-
-                $('#ListaContratos').empty();
-
+                $("#ListaContratos").empty();
                 loadListaContratos(status);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'Glosa_001.js',
-                    'callback@$.on#1',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("Glosa_<num>.js", "callback@$.on#1", error);
             }
         });
 
-        $('#tabVeiculos').attr('hidden', 'hidden');
-        $('#tab_borders_icons-4').attr('hidden', 'hidden');
-        $('#tblVeiculos').attr('hidden', 'hidden');
+        $("#tabVeiculos").attr("hidden", "hidden");
+        $("#tab_borders_icons-4").attr("hidden", "hidden");
+        $("#tblVeiculos").attr("hidden", "hidden");
 
         loadListaContratos(1);
 
         function loadListaContratos(tipoContrato) {
-            try {
-
+            try
+            {
                 $.ajax({
-                    type: 'get',
-                    url: '/api/Contrato/ListaContratosVeiculosGlosa',
-                    data: { tipoContrato: tipoContrato || '' },
+                    type: "get",
+                    url: "/api/Contrato/ListaContratosVeiculosGlosa",
+                    data: { tipoContrato: tipoContrato || "" },
                     success: function (res) {
-                        try {
-
-                            console.log('Função Nova:', res.data);
-
-                            var option =
-                                '<option>-- Selecione um Contrato --</option>';
+                        try
+                        {
+                            console.log("Função Nova:", res.data);
+
+                            var option = "<option>-- Selecione um Contrato --</option>";
 
                             if (res && res.data && res.data.length) {
-
                                 res.data.forEach(function (obj) {
-                                    try {
-
+                                    try
+                                    {
                                         option +=
                                             '<option value="' +
                                             obj.value +
                                             '">' +
                                             obj.text +
-                                            '</option>';
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'Glosa_001.js',
-                                            'callback@res.data.forEach#0',
+                                            "</option>";
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "Glosa_<num>.js",
+                                            "callback@res.data.forEach#0",
                                             error,
                                         );
                                     }
                                 });
                             }
 
-                            $('#ListaContratos').empty().append(option);
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'Glosa_001.js',
-                                'success',
-                                error,
-                            );
+                            $("#ListaContratos").empty().append(option);
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("Glosa_<num>.js", "success", error);
                         }
                     },
                     error: function (error) {
-                        try {
-
+                        try
+                        {
                             console.log(error);
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'Glosa_001.js',
-                                'error',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("Glosa_<num>.js", "error", error);
                         }
                     },
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'Glosa_001.js',
-                    'loadListaContratos',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("Glosa_<num>.js", "loadListaContratos", error);
             }
         }
 
         function loadTblVeiculos() {
-            try {
-
-                var id = $('#ListaContratos').val();
-
-                var dataTableVeiculo = $('#tblVeiculo').DataTable();
+            try
+            {
+                var id = $("#ListaContratos").val();
+
+                var dataTableVeiculo = $("#tblVeiculo").DataTable();
                 dataTableVeiculo.destroy();
 
-                dataTableVeiculo = $('#tblVeiculo').DataTable({
-
+                dataTableVeiculo = $("#tblVeiculo").DataTable({
                     columnDefs: [
                         {
                             targets: 0,
-                            className: 'text-center',
-                            width: '9%',
+                            className: "text-center",
+                            width: "9%",
                         },
                         {
                             targets: 1,
-                            className: 'text-left',
-                            width: '17%',
+                            className: "text-left",
+                            width: "17%",
                         },
                         {
                             targets: 2,
-                            className: 'text-center',
-                            width: '5%',
-                            defaultContent: '',
+                            className: "text-center",
+                            width: "5%",
+                            defaultContent: "",
                         },
                         {
                             targets: 3,
-                            className: 'text-center',
-                            width: '5%',
+                            className: "text-center",
+                            width: "5%",
                         },
                         {
                             targets: 4,
-                            className: 'text-center',
-                            width: '7%',
+                            className: "text-center",
+                            width: "7%",
                         },
                         {
                             targets: 5,
-                            className: 'text-center',
-                            width: '8%',
+                            className: "text-center",
+                            width: "8%",
                         },
                     ],
 
                     responsive: true,
-
                     ajax: {
-                        url: '/api/veiculo/glosaveiculocontratos',
+                        url: "/api/veiculo/glosaveiculocontratos",
                         data: { id: id },
-                        type: 'GET',
-                        datatype: 'json',
+                        type: "GET",
+                        datatype: "json",
                     },
-
                     columns: [
-                        { data: 'placa' },
-                        { data: 'marcaModelo' },
-                        { data: 'sigla' },
-                        { data: 'combustivelDescricao' },
-                        {
-
-                            data: 'status',
+                        { data: "placa" },
+                        { data: "marcaModelo" },
+                        { data: "sigla" },
+                        { data: "combustivelDescricao" },
+                        {
+                            data: "status",
                             render: function (data, type, row, meta) {
-                                try {
-
+                                try
+                                {
                                     if (data)
                                         return (
                                             '<a href="javascript:void" class="updateStatusVeiculo btn btn-verde btn-xs text-white" data-url="/api/Veiculo/updateStatusVeiculo?Id=' +
@@ -167,30 +153,33 @@
                                             row.veiculoId +
                                             '">Inativo</a>'
                                         );
-                                } catch (error) {
+                                }
+                                catch (error)
+                                {
                                     Alerta.TratamentoErroComLinha(
-                                        'Glosa_001.js',
-                                        'render',
+                                        "Glosa_<num>.js",
+                                        "render",
                                         error,
                                     );
                                 }
                             },
                         },
                         {
-
-                            data: 'veiculoId',
+                            data: "veiculoId",
                             render: function (data) {
-                                try {
-
+                                try
+                                {
                                     return `<div class="text-center">
                                 <a class="btn-deleteveiculocontrato btn btn-vinho btn-xs text-white" aria-label="Remover o veículo do contrato !" data-microtip-position="top" role="tooltip" style="cursor:pointer;" data-id='${data}'>
                                     <i class="far fa-trash-alt"></i>
                                 </a>
                     </div>`;
-                                } catch (error) {
+                                }
+                                catch (error)
+                                {
                                     Alerta.TratamentoErroComLinha(
-                                        'Glosa_001.js',
-                                        'render',
+                                        "Glosa_<num>.js",
+                                        "render",
                                         error,
                                     );
                                 }
@@ -199,594 +188,556 @@
                     ],
 
                     language: {
-                        url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                        emptyTable: 'Sem Dados para Exibição',
+                        url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                        emptyTable: "Sem Dados para Exibição",
                     },
-                    width: '100%',
+                    width: "100%",
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'Glosa_001.js',
-                    'loadTblVeiculos',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("Glosa_<num>.js", "loadTblVeiculos", error);
             }
         }
 
         function loadTblNotas() {
-            try {
-
-                var id = $('#ListaContratos').val();
-
-                var dataTableNotas = $('#tblNotas').DataTable();
+            try
+            {
+                var id = $("#ListaContratos").val();
+
+                var dataTableNotas = $("#tblNotas").DataTable();
                 dataTableNotas.destroy();
 
-                dataTableNotas = $('#tblNotas').DataTable({
-
+                dataTableNotas = $("#tblNotas").DataTable({
                     columnDefs: [
                         {
                             targets: 0,
-                            className: 'text-center',
-                            width: '8%',
+                            className: "text-center",
+                            width: "8%",
                         },
                         {
                             targets: 1,
-                            className: 'text-center',
-                            width: '8%',
+                            className: "text-center",
+                            width: "8%",
                         },
                         {
                             targets: 2,
-                            className: 'text-right',
-                            width: '10%',
+                            className: "text-right",
+                            width: "10%",
                         },
                         {
                             targets: 3,
-                            className: 'text-right',
-                            width: '8%',
+                            className: "text-right",
+                            width: "8%",
                         },
                         {
                             targets: 4,
-                            className: 'text-left',
-                            width: '15%',
+                            className: "text-left",
+                            width: "15%",
                         },
                         {
                             targets: 5,
-                            className: 'text-center',
-                            width: '8%',
+                            className: "text-center",
+                            width: "8%",
                         },
                         {
                             targets: 6,
-                            className: 'text-center',
-                            width: '10%',
+                            className: "text-center",
+                            width: "10%",
                             visible: false,
                         },
                         {
                             targets: 7,
-                            className: 'text-center',
-                            width: '10%',
+                            className: "text-center",
+                            width: "10%",
                             visible: false,
                         },
                     ],
 
                     responsive: true,
-
                     ajax: {
-                        url: '/api/notafiscal/nfcontratos',
+                        url: "/api/notafiscal/nfcontratos",
                         data: { id: id },
-                        type: 'GET',
-                        datatype: 'json',
+                        type: "GET",
+                        datatype: "json",
                     },
                     columns: [
-                        { data: 'numeroNF' },
-                        { data: 'dataFormatada' },
-                        { data: 'valorNFFormatado' },
-                        { data: 'valorGlosaFormatado' },
-                        { data: 'motivoGlosa' },
-                        {
-                            data: 'notaFiscalId',
+                        { data: "numeroNF" },
+                        { data: "dataFormatada" },
+                        { data: "valorNFFormatado" },
+                        { data: "valorGlosaFormatado" },
+                        { data: "motivoGlosa" },
+                        {
+                            data: "notaFiscalId",
                             render: function (data) {
-                                try {
+                                try
+                                {
                                     return `<div class="text-center">
                                 <a class="btn btn-delete btn-vinho btn-xs text-white" aria-label="Excluir a Nota Fiscal!" data-microtip-position="top" role="tooltip" style="cursor:pointer;" data-id='${data}'>
                                     <i class="far fa-trash-alt"></i>
                                 </a>
                         </div>`;
-                                } catch (error) {
+                                }
+                                catch (error)
+                                {
                                     Alerta.TratamentoErroComLinha(
-                                        'Glosa_001.js',
-                                        'render',
+                                        "Glosa_<num>.js",
+                                        "render",
                                         error,
                                     );
                                 }
                             },
                         },
-                        { data: 'contratoId' },
-                        { data: 'empenhoId' },
+                        { data: "contratoId" },
+                        { data: "empenhoId" },
                     ],
 
                     language: {
-                        url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                        emptyTable: 'Sem Dados para Exibição',
+                        url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                        emptyTable: "Sem Dados para Exibição",
                     },
-                    width: '100%',
+                    width: "100%",
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'Glosa_001.js',
-                    'loadTblNotas',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("Glosa_<num>.js", "loadTblNotas", error);
             }
         }
 
-        $('#ListaContratos').on('change', function () {
-            try {
-
-                var id = $('#ListaContratos').val();
+        $("#ListaContratos").on("change", function () {
+            try
+            {
+                var id = $("#ListaContratos").val();
 
                 $.ajax({
-                    type: 'get',
-                    url: '/api/Contrato/PegaContrato',
+                    type: "get",
+                    url: "/api/Contrato/PegaContrato",
                     data: { id: id },
                     success: function (res) {
-                        try {
-
+                        try
+                        {
                             console.info(res.data);
                             console.info(res.data[0].contratoLavadores);
                             console.info(res.data[0].contratoMotoristas);
                             console.info(res.data[0].contratoOperadores);
                             console.info(res.data[0].tipoContrato);
 
-                            $('#tabMotoristas').attr('hidden', 'hidden');
-                            $('#tab_borders_icons-1').attr('hidden', 'hidden');
-                            $('#tblMotoristas').attr('hidden', 'hidden');
-
-                            $('#tabOperadores').attr('hidden', 'hidden');
-                            $('#tab_borders_icons-2').attr('hidden', 'hidden');
-                            $('#tblOperadores').attr('hidden', 'hidden');
-
-                            $('#tabLavadores').attr('hidden', 'hidden');
-                            $('#tab_borders_icons-3').attr('hidden', 'hidden');
-                            $('#tblLavadores').attr('hidden', 'hidden');
-
-                            $('#tabVeiculos').attr('hidden', 'hidden');
-                            $('#tab_borders_icons-4').attr('hidden', 'hidden');
-                            $('#tblVeiculos').attr('hidden', 'hidden');
-
-                            $('#tabNotas').attr('hidden', 'hidden');
-                            $('#tab_borders_icons-5').attr('hidden', 'hidden');
-                            $('#tblNotFiscal').attr('hidden', 'hidden');
-
-                            if (res.data[0].tipoContrato === 'Terceirização') {
-
+                            $("#tabMotoristas").attr("hidden", "hidden");
+                            $("#tab_borders_icons-1").attr("hidden", "hidden");
+                            $("#tblMotoristas").attr("hidden", "hidden");
+
+                            $("#tabOperadores").attr("hidden", "hidden");
+                            $("#tab_borders_icons-2").attr("hidden", "hidden");
+                            $("#tblOperadores").attr("hidden", "hidden");
+
+                            $("#tabLavadores").attr("hidden", "hidden");
+                            $("#tab_borders_icons-3").attr("hidden", "hidden");
+                            $("#tblLavadores").attr("hidden", "hidden");
+
+                            $("#tabVeiculos").attr("hidden", "hidden");
+                            $("#tab_borders_icons-4").attr("hidden", "hidden");
+                            $("#tblVeiculos").attr("hidden", "hidden");
+
+                            $("#tabNotas").attr("hidden", "hidden");
+                            $("#tab_borders_icons-5").attr("hidden", "hidden");
+                            $("#tblNotFiscal").attr("hidden", "hidden");
+
+                            if (res.data[0].tipoContrato === "Terceirização") {
                                 if (res.data[0].contratoMotoristas === true) {
-                                    $('#tabMotoristas').removeAttr('hidden');
-                                    $('#tab_borders_icons-1').removeAttr(
-                                        'hidden',
-                                    );
-                                    $('#tblMotoristas').removeAttr('hidden');
-                                    $('.nav-tabs li:eq(0) a').tab('show');
+                                    $("#tabMotoristas").removeAttr("hidden");
+                                    $("#tab_borders_icons-1").removeAttr("hidden");
+                                    $("#tblMotoristas").removeAttr("hidden");
+                                    $(".nav-tabs li:eq(0) a").tab("show");
                                     loadTblMotoristas();
                                 }
 
                                 if (res.data[0].contratoOperadores === true) {
-                                    $('#tabOperadores').removeAttr('hidden');
-                                    $('#tab_borders_icons-2').removeAttr(
-                                        'hidden',
-                                    );
-                                    $('#tblOperadores').removeAttr('hidden');
-
+                                    $("#tabOperadores").removeAttr("hidden");
+                                    $("#tab_borders_icons-2").removeAttr("hidden");
+                                    $("#tblOperadores").removeAttr("hidden");
+                                    if (res.data[0].contratoMotoristas === false) {
+                                        $(".nav-tabs li:eq(1) a").tab("show");
+                                    }
+                                    loadTblOperadores();
+                                }
+
+                                if (res.data[0].contratoLavadores === true) {
+                                    $("#tabLavadores").removeAttr("hidden");
+                                    $("#tab_borders_icons-3").removeAttr("hidden");
+                                    $("#tblLavadores").removeAttr("hidden");
                                     if (
-                                        res.data[0].contratoMotoristas === false
-                                    ) {
-                                        $('.nav-tabs li:eq(1) a').tab('show');
-                                    }
-                                    loadTblOperadores();
-                                }
-
-                                if (res.data[0].contratoLavadores === true) {
-                                    $('#tabLavadores').removeAttr('hidden');
-                                    $('#tab_borders_icons-3').removeAttr(
-                                        'hidden',
-                                    );
-                                    $('#tblLavadores').removeAttr('hidden');
-
-                                    if (
-                                        res.data[0].contratoMotoristas ===
-                                            false ||
+                                        res.data[0].contratoMotoristas === false ||
                                         res.data[0].contratoOperadores === false
                                     ) {
-                                        $('.nav-tabs li:eq(2) a').tab('show');
+                                        $(".nav-tabs li:eq(2) a").tab("show");
                                     }
                                     loadTblLavadores();
                                 }
                             }
 
-                            if (res.data[0].tipoContrato === 'Locação') {
-                                $('#tabVeiculos').removeAttr('hidden');
-                                $('#tab_borders_icons-4').removeAttr('hidden');
-                                $('#tblVeiculo').removeAttr('hidden');
-                                $('.nav-tabs li:eq(3) a').tab('show');
+                            if (res.data[0].tipoContrato === "Locação") {
+                                $("#tabVeiculos").removeAttr("hidden");
+                                $("#tab_borders_icons-4").removeAttr("hidden");
+                                $("#tblVeiculo").removeAttr("hidden");
+                                $(".nav-tabs li:eq(3) a").tab("show");
                                 loadTblVeiculos();
                             }
 
-                            if (res.data[0].tipoContrato === 'Serviços') {
-                                $('#tabNotas').removeAttr('hidden');
-                                $('#tab_borders_icons-5').removeAttr('hidden');
-                                $('.nav-tabs li:eq(4) a').tab('show');
-                                $('#tblNotaFiscal').removeAttr('hidden');
+                            if (res.data[0].tipoContrato === "Serviços") {
+                                $("#tabNotas").removeAttr("hidden");
+                                $("#tab_borders_icons-5").removeAttr("hidden");
+                                $(".nav-tabs li:eq(4) a").tab("show");
+                                $("#tblNotaFiscal").removeAttr("hidden");
                                 loadTblNotas();
-                                console.log('Entrei nas Notas');
+                                console.log("Entrei nas Notas");
                             }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'Glosa_001.js',
-                                'success',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("Glosa_<num>.js", "success", error);
                         }
                     },
                     error: function (err) {
-                        try {
+                        try
+                        {
                             console.log(err);
-                            alert('something went wrong');
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'Glosa_001.js',
-                                'error',
-                                error,
-                            );
+                            alert("something went wrong");
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("Glosa_<num>.js", "error", error);
                         }
                     },
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'Glosa_001.js',
-                    'callback@$.on#1',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("Glosa_<num>.js", "callback@$.on#1", error);
             }
         });
 
-        $(document).on('click', '.btn-deletemotoristacontrato', function () {
-            try {
-
-                var id = $(this).data('id');
-
-                var contratoid = $('#ListaContratos').val();
+        $(document).on("click", ".btn-deletemotoristacontrato", function () {
+            try
+            {
+                var id = $(this).data("id");
+                var contratoid = $("#ListaContratos").val();
 
                 Alerta.Confirmar(
-                    'Você tem certeza que deseja remover este motorista do contrato?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Remover',
-                    'Cancelar',
+                    "Você tem certeza que deseja remover este motorista do contrato?",
+                    "Não será possível recuperar os dados eliminados!",
+                    "Remover",
+                    "Cancelar"
+
                 ).then((willDelete) => {
-                    try {
-
+                    try
+                    {
                         if (willDelete) {
-
                             var dataToPost = JSON.stringify({
                                 MotoristaId: id,
                                 ContratoId: contratoid,
                             });
-                            var url = '/api/Motorista/DeleteContrato';
-
+                            var url = "/api/Motorista/DeleteContrato";
                             $.ajax({
                                 url: url,
-                                type: 'POST',
+                                type: "POST",
                                 data: dataToPost,
-                                contentType: 'application/json; charset=utf-8',
-                                dataType: 'json',
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
                                 success: function (data) {
-                                    try {
-
+                                    try
+                                    {
                                         if (data.success) {
-                                            AppToast.show(
-                                                'Verde',
-                                                data.message,
-                                            );
-
+                                            AppToast.show('Verde', data.message);
                                             loadTblMotoristas();
                                         } else {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                            );
+                                            AppToast.show('Vermelho', data.message);
                                         }
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'Glosa_001.js',
-                                            'success',
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "Glosa_<num>.js",
+                                            "success",
                                             error,
                                         );
                                     }
                                 },
                                 error: function (err) {
-                                    try {
+                                    try
+                                    {
                                         console.log(err);
-                                        alert('something went wrong');
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'Glosa_001.js',
-                                            'error',
+                                        alert("something went wrong");
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "Glosa_<num>.js",
+                                            "error",
                                             error,
                                         );
                                     }
                                 },
                             });
                         }
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'Glosa_001.js',
-                            'callback@swal.then#0',
+                            "Glosa_<num>.js",
+                            "callback@swal.then#0",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'Glosa_001.js',
-                    'callback@$.on#2',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("Glosa_<num>.js", "callback@$.on#2", error);
             }
         });
 
-        $(document).on('click', '.btn-deleteoperadorcontrato', function () {
-            try {
-
-                var id = $(this).data('id');
-
-                var contratoid = $('#ListaContratos').val();
+        $(document).on("click", ".btn-deleteoperadorcontrato", function () {
+            try
+            {
+                var id = $(this).data("id");
+                var contratoid = $("#ListaContratos").val();
 
                 Alerta.Confirmar(
-                    'Você tem certeza que deseja remover este operador do contrato?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Remover',
-                    'Cancelar',
+                    "Você tem certeza que deseja remover este operador do contrato?",
+                    "Não será possível recuperar os dados eliminados!",
+                    "Remover",
+                    "Cancelar"
+
                 ).then((willDelete) => {
-                    try {
-
+                    try
+                    {
                         if (willDelete) {
-
                             var dataToPost = JSON.stringify({
                                 OperadorId: id,
                                 ContratoId: contratoid,
                             });
-                            var url = '/api/Operador/DeleteContrato';
-
+                            var url = "/api/Operador/DeleteContrato";
                             $.ajax({
                                 url: url,
-                                type: 'POST',
+                                type: "POST",
                                 data: dataToPost,
-                                contentType: 'application/json; charset=utf-8',
-                                dataType: 'json',
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
                                 success: function (data) {
-                                    try {
-
+                                    try
+                                    {
                                         if (data.success) {
-                                            AppToast.show(
-                                                'Verde',
-                                                data.message,
-                                            );
-
+                                            AppToast.show('Verde', data.message);
                                             loadTblOperadores();
                                         } else {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                            );
+                                            AppToast.show('Vermelho', data.message);
                                         }
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'Glosa_001.js',
-                                            'success',
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "Glosa_<num>.js",
+                                            "success",
                                             error,
                                         );
                                     }
                                 },
                                 error: function (err) {
-                                    try {
+                                    try
+                                    {
                                         console.log(err);
-                                        alert('something went wrong');
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'Glosa_001.js',
-                                            'error',
+                                        alert("something went wrong");
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "Glosa_<num>.js",
+                                            "error",
                                             error,
                                         );
                                     }
                                 },
                             });
                         }
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'Glosa_001.js',
-                            'callback@swal.then#0',
+                            "Glosa_<num>.js",
+                            "callback@swal.then#0",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'Glosa_001.js',
-                    'callback@$.on#2',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("Glosa_<num>.js", "callback@$.on#2", error);
             }
         });
 
-        $(document).on('click', '.btn-deletelavadorcontrato', function () {
-            try {
-
-                var id = $(this).data('id');
-
-                var contratoid = $('#ListaContratos').val();
+        $(document).on("click", ".btn-deletelavadorcontrato", function () {
+            try
+            {
+                var id = $(this).data("id");
+                var contratoid = $("#ListaContratos").val();
 
                 Alerta.Confirmar(
-                    'Você tem certeza que deseja remover este lavador do contrato?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Remover',
-                    'Cancelar',
+                    "Você tem certeza que deseja remover este lavador do contrato?",
+                    "Não será possível recuperar os dados eliminados!",
+                    "Remover",
+                    "Cancelar"
+
                 ).then((willDelete) => {
-                    try {
-
+                    try
+                    {
                         if (willDelete) {
-
                             var dataToPost = JSON.stringify({
                                 LavadorId: id,
                                 ContratoId: contratoid,
                             });
-                            var url = '/api/Lavador/DeleteContrato';
-
+                            var url = "/api/Lavador/DeleteContrato";
                             $.ajax({
                                 url: url,
-                                type: 'POST',
+                                type: "POST",
                                 data: dataToPost,
-                                contentType: 'application/json; charset=utf-8',
-                                dataType: 'json',
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
                                 success: function (data) {
-                                    try {
+                                    try
+                                    {
                                         if (data.success) {
-                                            AppToast.show(
-                                                'Verde',
-                                                data.message,
-                                            );
+                                            AppToast.show('Verde', data.message);
                                             loadTblLavadores();
                                         } else {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                            );
+                                            AppToast.show('Vermelho', data.message);
                                         }
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'Glosa_001.js',
-                                            'success',
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "Glosa_<num>.js",
+                                            "success",
                                             error,
                                         );
                                     }
                                 },
                                 error: function (err) {
-                                    try {
+                                    try
+                                    {
                                         console.log(err);
-                                        alert('something went wrong');
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'Glosa_001.js',
-                                            'error',
+                                        alert("something went wrong");
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "Glosa_<num>.js",
+                                            "error",
                                             error,
                                         );
                                     }
                                 },
                             });
                         }
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'Glosa_001.js',
-                            'callback@swal.then#0',
+                            "Glosa_<num>.js",
+                            "callback@swal.then#0",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'Glosa_001.js',
-                    'callback@$.on#2',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("Glosa_<num>.js", "callback@$.on#2", error);
             }
         });
 
-        $(document).on('click', '.btn-deleteveiculocontrato', function () {
-            try {
-
-                var id = $(this).data('id');
-
-                var contratoid = $('#ListaContratos').val();
+        $(document).on("click", ".btn-deleteveiculocontrato", function () {
+            try
+            {
+                var id = $(this).data("id");
+                var contratoid = $("#ListaContratos").val();
 
                 Alerta.Confirmar(
-                    'Você tem certeza que deseja remover este veículo do contrato?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Remover',
-                    'Cancelar',
+                    "Você tem certeza que deseja remover este veículo do contrato?",
+                    "Não será possível recuperar os dados eliminados!",
+                    "Remover",
+                    "Cancelar"
                 ).then((willDelete) => {
-                    try {
-
+                    try
+                    {
                         if (willDelete) {
-
                             var dataToPost = JSON.stringify({
                                 VeiculoId: id,
                                 ContratoId: contratoid,
                             });
-                            var url = '/api/Veiculo/DeleteContrato';
-
+                            var url = "/api/Veiculo/DeleteContrato";
                             $.ajax({
                                 url: url,
-                                type: 'POST',
+                                type: "POST",
                                 data: dataToPost,
-                                contentType: 'application/json; charset=utf-8',
-                                dataType: 'json',
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
                                 success: function (data) {
-                                    try {
-
+                                    try
+                                    {
                                         if (data.success) {
-                                            AppToast.show(
-                                                'Verde',
-                                                data.message,
-                                            );
-
+                                            AppToast.show('Verde', data.message);
                                             loadTblVeiculos();
                                         } else {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                            );
+                                            AppToast.show('Vermelho', data.message);
                                         }
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'Glosa_001.js',
-                                            'success',
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "Glosa_<num>.js",
+                                            "success",
                                             error,
                                         );
                                     }
                                 },
                                 error: function (err) {
-                                    try {
+                                    try
+                                    {
                                         console.log(err);
-                                        alert('something went wrong');
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'Glosa_001.js',
-                                            'error',
+                                        alert("something went wrong");
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "Glosa_<num>.js",
+                                            "error",
                                             error,
                                         );
                                     }
                                 },
                             });
                         }
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'Glosa_001.js',
-                            'callback@swal.then#0',
+                            "Glosa_<num>.js",
+                            "callback@swal.then#0",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'Glosa_001.js',
-                    'callback@$.on#2',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("Glosa_<num>.js", "callback@$.on#2", error);
             }
         });
-    } catch (error) {
-
-        Alerta.TratamentoErroComLinha(
-            'Glosa_001.js',
-            'callback@$.ready#0',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("Glosa_<num>.js", "callback@$.ready#0", error);
     }
 });
```
