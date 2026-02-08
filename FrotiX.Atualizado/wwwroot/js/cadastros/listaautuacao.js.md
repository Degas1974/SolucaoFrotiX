# wwwroot/js/cadastros/listaautuacao.js

**Mudanca:** GRANDE | **+361** linhas | **-441** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/listaautuacao.js
+++ ATUAL: wwwroot/js/cadastros/listaautuacao.js
@@ -1,81 +1,68 @@
-function mostrarLoadingAutuacao() {
-    try {
-
+function mostrarLoadingAutuacao()
+{
+    try
+    {
         const overlay = document.getElementById('loadingOverlayAutuacao');
-        if (overlay) {
-
+        if (overlay)
+        {
             overlay.style.display = 'flex';
         }
-    } catch (error) {
-        try {
-            Alerta.TratamentoErroComLinha(
-                'listaautuacao.js',
-                'mostrarLoadingAutuacao',
-                error,
-            );
-        } catch (_) {}
+    }
+    catch (error)
+    {
+        try { Alerta.TratamentoErroComLinha("listaautuacao.js", "mostrarLoadingAutuacao", error); } catch (_) { }
     }
 }
 
-function esconderLoadingAutuacao() {
-    try {
-
+function esconderLoadingAutuacao()
+{
+    try
+    {
         const overlay = document.getElementById('loadingOverlayAutuacao');
-        if (overlay) {
-
+        if (overlay)
+        {
             overlay.style.display = 'none';
         }
-    } catch (error) {
-        try {
-            Alerta.TratamentoErroComLinha(
-                'listaautuacao.js',
-                'esconderLoadingAutuacao',
-                error,
-            );
-        } catch (_) {}
+    }
+    catch (error)
+    {
+        try { Alerta.TratamentoErroComLinha("listaautuacao.js", "esconderLoadingAutuacao", error); } catch (_) { }
     }
 }
 
-$(document).ready(function () {
-    try {
-
+$(document).ready(function ()
+{
+    try
+    {
         ListaTodasNotificacoes();
-    } catch (error) {
-        try {
-            Alerta.TratamentoErroComLinha(
-                'listaautuacao.js',
-                'document.ready',
-                error,
-            );
-        } catch (_) {}
+    } catch (error)
+    {
+        try { Alerta.TratamentoErroComLinha("listaautuacao.js", "document.ready", error); } catch (_) { }
     }
 });
 
-function ListaTodasNotificacoes() {
-    try {
+function ListaTodasNotificacoes()
+{
+    try
+    {
 
         mostrarLoadingAutuacao();
 
-        const veiculos =
-            document.getElementById('lstVeiculos')?.ej2_instances?.[0];
-        const tipos =
-            document.getElementById('lstTiposMulta')?.ej2_instances?.[0];
-        const motoristas =
-            document.getElementById('lstMotorista')?.ej2_instances?.[0];
+        const veiculos = document.getElementById('lstVeiculos')?.ej2_instances?.[0];
+        const tipos = document.getElementById('lstTiposMulta')?.ej2_instances?.[0];
+        const motoristas = document.getElementById('lstMotorista')?.ej2_instances?.[0];
         const orgaos = document.getElementById('lstOrgao')?.ej2_instances?.[0];
-        const statusCb =
-            document.getElementById('lstStatus')?.ej2_instances?.[0];
-
-        const veiculoId = veiculos?.value ?? '';
-        const tipoMultaId =
-            tipos?.value !== null && tipos?.value !== '' ? tipos.value : '';
-        const motoristaId = motoristas?.value ?? '';
-        const orgaoAutuanteId = orgaos?.value ?? '';
-        const statusId = statusCb?.value ?? '';
+        const statusCb = document.getElementById('lstStatus')?.ej2_instances?.[0];
+
+        const veiculoId = veiculos?.value ?? "";
+        const tipoMultaId = (tipos?.value !== null && tipos?.value !== "") ? tipos.value : "";
+        const motoristaId = motoristas?.value ?? "";
+        const orgaoAutuanteId = orgaos?.value ?? "";
+        const statusId = statusCb?.value ?? "";
 
         DTBetterErrors.setGlobalOptions({
             logToConsole: true,
-            dedupeWindowMs: 3000,
+            dedupeWindowMs: 3000
         });
 
         DTBetterErrors.enable('#tblMulta', {
@@ -84,32 +71,26 @@
             encaminharParaAlerta: true,
             preferEnriquecido: true,
             showModal: true,
-            previewLimit: 1200,
+            previewLimit: 1200
         });
 
-        if ($.fn.DataTable.isDataTable('#tblMulta')) {
-            try {
+        if ($.fn.DataTable.isDataTable('#tblMulta'))
+        {
+            try
+            {
                 $('#tblMulta').DataTable().clear().destroy();
-            } catch (_) {}
+            } catch (_) { }
             $('#tblMulta tbody').empty();
         }
 
         let dataTableMultas = $('#tblMulta').DataTable({
             autoWidth: false,
             dom: 'Bfrtip',
-            lengthMenu: [
-                [10, 25, 50, -1],
-                ['10 linhas', '25 linhas', '50 linhas', 'Todas as Linhas'],
-            ],
-
+            lengthMenu: [[10, 25, 50, -1], ['10 linhas', '25 linhas', '50 linhas', 'Todas as Linhas']],
             buttons: [
                 'pageLength',
                 'excel',
-                {
-                    extend: 'pdfHtml5',
-                    orientation: 'landscape',
-                    pageSize: 'LEGAL',
-                },
+                { extend: 'pdfHtml5', orientation: 'landscape', pageSize: 'LEGAL' }
             ],
             order: [[1, 'desc']],
             responsive: true,
@@ -118,9 +99,9 @@
 
             columnDefs: [
                 {
-                    targets: 0,
-                    className: 'text-center',
-                    render: function (data, type, full) {
+                    targets: 0, className: "text-center",
+                    render: function (data, type, full)
+                    {
                         return `
               <div class="text-center">
                 <a aria-label="&#9762; (${full.observacao})"
@@ -130,15 +111,14 @@
                    ${full.numInfracao}
                 </a>
               </div>`;
-                    },
+                    }
                 },
-                { targets: 1, className: 'text-center' },
-                { targets: 2, className: 'text-center' },
-                {
-                    targets: 3,
-                    className: 'text-left',
-                    width: '12%',
-                    render: function (data, type, full) {
+                { targets: 1, className: "text-center" },
+                { targets: 2, className: "text-center" },
+                {
+                    targets: 3, className: "text-left", width: "12%",
+                    render: function (data, type, full)
+                    {
                         return `
               <div class="text-center">
                 <a aria-label="&#128241; (${full.telefone})"
@@ -147,14 +127,14 @@
                    ${full.nome}
                 </a>
               </div>`;
-                    },
+                    }
                 },
-                { targets: 4, className: 'text-center' },
-                { targets: 5, className: 'text-left' },
-                {
-                    targets: 6,
-                    className: 'text-left',
-                    render: function (data, type, full) {
+                { targets: 4, className: "text-center" },
+                { targets: 5, className: "text-left" },
+                {
+                    targets: 6, className: "text-left",
+                    render: function (data, type, full)
+                    {
                         return `
               <div class="text-center">
                 <a aria-label="&#9940; (${full.descricao})"
@@ -164,31 +144,37 @@
                    ${full.artigo}
                 </a>
               </div>`;
-                    },
+                    }
                 },
-                { targets: 7, className: 'text-left', width: '16%' },
-                { targets: 8, className: 'text-center' },
-                { targets: 9, className: 'text-right' },
-                { targets: 10, className: 'text-right' },
-                {
-
-                    targets: 11,
-                    className: 'text-center',
-                    width: '10%',
-                    render: function (data, type, full) {
+                { targets: 7, className: "text-left", width: "16%" },
+                { targets: 8, className: "text-center" },
+                { targets: 9, className: "text-right" },
+                { targets: 10, className: "text-right" },
+                {
+
+                    targets: 11, className: "text-center", width: "10%",
+                    render: function (data, type, full)
+                    {
                         let badgeClass = 'ftx-badge-default';
                         let iconClass = 'fa-circle-question';
 
-                        if (data === 'Notificado') {
+                        if (data === 'Notificado')
+                        {
                             badgeClass = 'ftx-badge-notificado';
                             iconClass = 'fa-bell';
-                        } else if (data === 'Reconhecido') {
+                        }
+                        else if (data === 'Reconhecido')
+                        {
                             badgeClass = 'ftx-badge-reconhecido';
                             iconClass = 'fa-circle-check';
-                        } else if (data === 'Pendente') {
+                        }
+                        else if (data === 'Pendente')
+                        {
                             badgeClass = 'ftx-badge-pendente';
                             iconClass = 'fa-clock';
-                        } else if (data === 'Pago') {
+                        }
+                        else if (data === 'Pago')
+                        {
                             badgeClass = 'ftx-badge-pago';
                             iconClass = 'fa-money-bill-wave';
                         }
@@ -197,17 +183,15 @@
                                     <i class="fa-solid ${iconClass}"></i>
                                     ${data || '-'}
                                 </span>`;
-                    },
+                    }
                 },
                 {
 
-                    targets: 12,
-                    className: 'text-center',
-                    width: '15%',
-                    render: function (data, type, full) {
-
-                        const temPDF =
-                            full.autuacaoPDF && full.autuacaoPDF !== '';
+                    targets: 12, className: "text-center", width: "15%",
+                    render: function (data, type, full)
+                    {
+
+                        const temPDF = full.autuacaoPDF && full.autuacaoPDF !== '';
 
                         const btnExibirPDF = temPDF
                             ? `<a class="ftx-btn-icon ftx-btn-pdf btn-exibe-autuacao"
@@ -246,465 +230,385 @@
                 </a>
                 ${btnExibirPDF}
               </div>`;
-                    },
-                },
+                    }
+                }
             ],
 
             ajax: {
-                url: '/api/multa/ListaMultas',
-                type: 'GET',
+                url: "/api/multa/ListaMultas",
+                type: "GET",
                 data: {
-                    fase: 'Autuação',
+                    fase: "Autuação",
                     veiculo: veiculoId,
                     orgao: orgaoAutuanteId,
                     motorista: motoristaId,
                     infracao: tipoMultaId,
-                    status: statusId,
+                    status: statusId
                 },
-                datatype: 'json',
-                error: function (xhr, error, thrown) {
-                    try {
+                datatype: "json",
+                error: function (xhr, error, thrown)
+                {
+                    try
+                    {
                         esconderLoadingAutuacao();
-                        console.error(
-                            'Erro ao carregar autuações:',
-                            error,
-                            thrown,
-                        );
-                    } catch (e) {
-                        try {
-                            Alerta.TratamentoErroComLinha(
-                                'listaautuacao.js',
-                                'ajax.error',
-                                e,
-                            );
-                        } catch (_) {}
-                    }
-                },
+                        console.error('Erro ao carregar autuações:', error, thrown);
+                    }
+                    catch (e)
+                    {
+                        try { Alerta.TratamentoErroComLinha("listaautuacao.js", "ajax.error", e); } catch (_) { }
+                    }
+                }
             },
             columns: [
-                { data: 'numInfracao' },
-                { data: 'data' },
-                { data: 'hora' },
-                { data: 'nome' },
-                { data: 'placa' },
-                { data: 'sigla' },
-                { data: 'artigo' },
-                { data: 'localizacao' },
-                { data: 'vencimento' },
-                { data: 'valorAteVencimento' },
-                { data: 'valorPosVencimento' },
-                { data: 'status' },
-                { data: 'multaId' },
+                { data: "numInfracao" }, { data: "data" }, { data: "hora" }, { data: "nome" },
+                { data: "placa" }, { data: "sigla" }, { data: "artigo" }, { data: "localizacao" },
+                { data: "vencimento" }, { data: "valorAteVencimento" }, { data: "valorPosVencimento" },
+                { data: "status" }, { data: "multaId" }
             ],
 
             language: {
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
+                emptyTable: "Nenhum registro encontrado",
+                info: "Mostrando de _START_ até _END_ de _TOTAL_ registros",
+                infoEmpty: "Mostrando 0 até 0 de 0 registros",
+                infoFiltered: "(Filtrados de _MAX_ registros)",
+                infoThousands: ".",
+                loadingRecords: "Carregando...",
+                processing: "Processando...",
+                zeroRecords: "Nenhum registro encontrado",
+                search: "Pesquisar",
+                paginate: { next: "Próximo", previous: "Anterior", first: "Primeiro", last: "Último" },
+                aria: { sortAscending: ": Ordenar colunas de forma ascendente", sortDescending: ": Ordenar colunas de forma descendente" },
+                select: { rows: { "_": "Selecionado %d linhas", "1": "Selecionado 1 linha" } },
+                buttons: {
+                    copySuccess: { "1": "Uma linha copiada com sucesso", "_": "%d linhas copiadas com sucesso" },
+                    collection: "Coleção <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"></span>",
+                    colvis: "Visibilidade da Coluna", colvisRestore: "Restaurar Visibilidade", copy: "Copiar",
+                    copyKeys: "Pressione ctrl ou ⌘ + C para copiar os dados da tabela. Para cancelar, clique nesta mensagem ou pressione Esc.",
+                    copyTitle: "Copiar para a Área de Transferência", csv: "CSV", excel: "Excel",
+                    pageLength: { "-1": "Mostrar todos os registros", "_": "Mostrar %d registros" },
+                    pdf: "PDF", print: "Imprimir"
                 },
-                aria: {
-                    sortAscending: ': Ordenar colunas de forma ascendente',
-                    sortDescending: ': Ordenar colunas de forma descendente',
-                },
-                select: {
-                    rows: {
-                        _: 'Selecionado %d linhas',
-                        1: 'Selecionado 1 linha',
-                    },
-                },
-                buttons: {
-                    copySuccess: {
-                        1: 'Uma linha copiada com sucesso',
-                        _: '%d linhas copiadas com sucesso',
-                    },
-                    collection:
-                        'Coleção <span class="ui-button-icon-primary ui-icon ui-icon-triangle-1-s"></span>',
-                    colvis: 'Visibilidade da Coluna',
-                    colvisRestore: 'Restaurar Visibilidade',
-                    copy: 'Copiar',
-                    copyKeys:
-                        'Pressione ctrl ou ⌘ + C para copiar os dados da tabela. Para cancelar, clique nesta mensagem ou pressione Esc.',
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
-                lengthMenu: 'Exibir _MENU_ resultados por página',
-                thousands: '.',
-                decimal: ',',
+                lengthMenu: "Exibir _MENU_ resultados por página",
+                thousands: ".",
+                decimal: ","
             },
-            drawCallback: function (settings) {
-                try {
+            drawCallback: function (settings)
+            {
+                try
+                {
 
                     esconderLoadingAutuacao();
 
-                    if (window.FTXTooltip) {
+                    if (window.FTXTooltip)
+                    {
                         window.FTXTooltip.refresh();
                     }
-                } catch (error) {
-                    try {
-                        Alerta.TratamentoErroComLinha(
-                            'listaautuacao.js',
-                            'drawCallback',
-                            error,
-                        );
-                    } catch (_) {}
+                } catch (error)
+                {
+                    try { Alerta.TratamentoErroComLinha("listaautuacao.js", "drawCallback", error); } catch (_) { }
                 }
             },
-            initComplete: function (settings, json) {
-                try {
+            initComplete: function (settings, json)
+            {
+                try
+                {
 
                     esconderLoadingAutuacao();
-                } catch (error) {
-                    try {
-                        Alerta.TratamentoErroComLinha(
-                            'listaautuacao.js',
-                            'initComplete',
-                            error,
-                        );
-                    } catch (_) {}
+                } catch (error)
+                {
+                    try { Alerta.TratamentoErroComLinha("listaautuacao.js", "initComplete", error); } catch (_) { }
+                }
+            }
+        });
+    } catch (error)
+    {
+
+        esconderLoadingAutuacao();
+        try { Alerta.TratamentoErroComLinha("listaautuacao.js", "ListaTodasNotificacoes", error); } catch (_) { }
+    }
+}
+
+$(document).on('click', '.btn-status', function (e)
+{
+    try
+    {
+        e.preventDefault();
+        const multaId = $(this).data('id');
+        $('#txtIdStatus').val(multaId);
+
+        $.ajax({
+            type: "get",
+            url: "/api/Multa/PegaStatus",
+            data: { Id: multaId },
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
+            success: function (data)
+            {
+                try
+                {
+                    const { numInfracao, data: dataAutuacao, hora, nome, status } = data;
+
+                    $("#txtNumInfracaoStatus").html(numInfracao || '-');
+                    $("#txtDataStatus").html(dataAutuacao || '-');
+                    $("#txtHoraStatus").html(hora || '-');
+                    $("#txtMotoristaStatus").html(nome || '-');
+                    $("#txtStatusAtual").html(status || '-');
+
+                    const lstStatus = document.getElementById('lstStatusAlterado')?.ej2_instances?.[0];
+                    if (lstStatus)
+                    {
+                        lstStatus.value = status;
+                    }
+
+                    $('#modalAlteraStatus').modal('show');
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("listaautuacao.js", "btn-status.success", error);
                 }
             },
-        });
-    } catch (error) {
-
-        esconderLoadingAutuacao();
-        try {
-            Alerta.TratamentoErroComLinha(
-                'listaautuacao.js',
-                'ListaTodasNotificacoes',
-                error,
-            );
-        } catch (_) {}
-    }
-}
-
-$(document).on('click', '.btn-status', function (e) {
-    try {
-        e.preventDefault();
-
-        const multaId = $(this).data('id');
-
-        $('#txtIdStatus').val(multaId);
-
-        $.ajax({
-            type: 'get',
-            url: '/api/Multa/PegaStatus',
-            data: { Id: multaId },
-            contentType: 'application/json; charset=utf-8',
-            dataType: 'json',
-            success: function (data) {
-                try {
-
-                    const {
-                        numInfracao,
-                        data: dataAutuacao,
-                        hora,
-                        nome,
-                        status,
-                    } = data;
-
-                    $('#txtNumInfracaoStatus').html(numInfracao || '-');
-                    $('#txtDataStatus').html(dataAutuacao || '-');
-                    $('#txtHoraStatus').html(hora || '-');
-                    $('#txtMotoristaStatus').html(nome || '-');
-                    $('#txtStatusAtual').html(status || '-');
-
-                    const lstStatus =
-                        document.getElementById('lstStatusAlterado')
-                            ?.ej2_instances?.[0];
-                    if (lstStatus) {
-                        lstStatus.value = status;
-                    }
-
-                    $('#modalAlteraStatus').modal('show');
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'listaautuacao.js',
-                        'btn-status.success',
-                        error,
-                    );
-                }
-            },
-            error: function (data) {
+            error: function (data)
+            {
                 Alerta.Erro('Erro', 'Erro ao recuperar status', 'OK');
                 console.log(data);
-            },
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'listaautuacao.js',
-            'btn-status.click',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("listaautuacao.js", "btn-status.click", error);
     }
 });
 
-$(document).on('click', '.btn-pagamento', function (e) {
-    try {
+$(document).on('click', '.btn-pagamento', function (e)
+{
+    try
+    {
         e.preventDefault();
-
         const multaId = $(this).data('id');
 
         $('#txtId').val(multaId);
         console.log('✅ txtId definido como:', multaId);
 
         $.ajax({
-            type: 'get',
-            url: '/api/Multa/PegaObservacao',
+            type: "get",
+            url: "/api/Multa/PegaObservacao",
             data: { Id: multaId },
-            contentType: 'application/json; charset=utf-8',
-            dataType: 'json',
-            success: function (data) {
-                try {
-
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
+            success: function (data)
+            {
+                try
+                {
                     const { nomeMotorista, numInfracao, observacao } = data;
 
-                    const rte =
-                        document.getElementById('rte')?.ej2_instances?.[0];
-                    if (rte) {
-                        rte.value = observacao || '';
-                    }
-
-                    $('#h3Titulo').html(
-                        `<i class="fa-duotone fa-money-bill-transfer me-2"></i>Transformar em Penalidade a Autuação nº ${numInfracao} de ${nomeMotorista}`,
-                    );
+                    const rte = document.getElementById('rte')?.ej2_instances?.[0];
+                    if (rte)
+                    {
+                        rte.value = observacao || "";
+                    }
+
+                    $("#h3Titulo").html(`<i class="fa-duotone fa-money-bill-transfer me-2"></i>Transformar em Penalidade a Autuação nº ${numInfracao} de ${nomeMotorista}`);
 
                     $('#modalTransformaPenalidade').modal('show');
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'listaautuacao.js',
-                        'btn-pagamento.success',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("listaautuacao.js", "btn-pagamento.success", error);
                 }
             },
-            error: function (data) {
+            error: function (data)
+            {
                 Alerta.Erro('Erro', 'Erro ao carregar dados da multa', 'OK');
                 console.log(data);
-            },
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'listaautuacao.js',
-            'btn-pagamento.click',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("listaautuacao.js", "btn-pagamento.click", error);
     }
 });
 
-$(document).on('click', '.btn-apagar', function (e) {
-    try {
+$(document).on('click', '.btn-apagar', function (e)
+{
+    try
+    {
         e.preventDefault();
-
         const id = $(this).data('id');
 
         Alerta.Confirmar(
-            'Você tem certeza que deseja apagar esta Autuação?',
-            'Não será possível recuperar os dados eliminados!',
-            'Excluir',
-            'Cancelar',
-        ).then(function (confirmed) {
-            try {
-
+            "Você tem certeza que deseja apagar esta Autuação?",
+            "Não será possível recuperar os dados eliminados!",
+            "Excluir",
+            "Cancelar"
+        ).then(function (confirmed)
+        {
+            try
+            {
                 if (!confirmed) return;
 
                 $.ajax({
                     url: '/api/Multa/Delete',
-                    type: 'POST',
+                    type: "POST",
                     data: JSON.stringify({ MultaId: id }),
-                    contentType: 'application/json; charset=utf-8',
-                    dataType: 'json',
-                    success: function (data) {
-                        try {
-
-                            if (data.success) {
+                    contentType: "application/json; charset=utf-8",
+                    dataType: "json",
+                    success: function (data)
+                    {
+                        try
+                        {
+                            if (data.success)
+                            {
                                 AppToast.show('Verde', data.message, 2000);
-
                                 ListaTodasNotificacoes();
-                            } else {
+                            }
+                            else
+                            {
                                 AppToast.show('Vermelho', data.message, 3000);
                             }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'listaautuacao.js',
-                                'btn-apagar.success',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("listaautuacao.js", "btn-apagar.success", error);
                         }
                     },
-                    error: function (err) {
-                        try {
+                    error: function (err)
+                    {
+                        try
+                        {
                             console.log(err);
-                            Alerta.Erro(
-                                'Erro',
-                                'Ocorreu um problema ao apagar.',
-                                'OK',
-                            );
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'listaautuacao.js',
-                                'btn-apagar.error',
-                                error,
-                            );
-                        }
-                    },
+                            Alerta.Erro('Erro', 'Ocorreu um problema ao apagar.', 'OK');
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("listaautuacao.js", "btn-apagar.error", error);
+                        }
+                    }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'listaautuacao.js',
-                    'btn-apagar.confirmar',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("listaautuacao.js", "btn-apagar.confirmar", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'listaautuacao.js',
-            'btn-apagar.click',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("listaautuacao.js", "btn-apagar.click", error);
     }
 });
 
-$(document).on('click', '.btn-exibe-autuacao', function (e) {
-    try {
+$(document).on('click', '.btn-exibe-autuacao', function (e)
+{
+    try
+    {
         e.preventDefault();
 
         const autuacaoPDF = $(this).data('autuacaopdf');
         const multaId = $(this).data('id');
 
-        console.log(
-            '🔍 Clique em btn-exibe-autuacao, autuacaoPDF:',
-            autuacaoPDF,
-        );
-
-        if (!autuacaoPDF || autuacaoPDF === '') {
-            AppToast.show(
-                'Amarelo',
-                'PDF não informado na gravação desta autuação',
-                3000,
-            );
+        console.log('🔍 Clique em btn-exibe-autuacao, autuacaoPDF:', autuacaoPDF);
+
+        if (!autuacaoPDF || autuacaoPDF === '')
+        {
+            AppToast.show('Amarelo', 'PDF não informado na gravação desta autuação', 3000);
             return;
         }
 
         $.ajax({
-            type: 'GET',
-            url: '/api/Multa/VerificaPDFExiste',
+            type: "GET",
+            url: "/api/Multa/VerificaPDFExiste",
             data: { nomeArquivo: autuacaoPDF },
-            dataType: 'json',
-            success: function (data) {
-                try {
-                    if (data.success && data.existe) {
-
+            dataType: "json",
+            success: function (data)
+            {
+                try
+                {
+                    if (data.success && data.existe)
+                    {
                         exibirPDFAutuacao(autuacaoPDF);
-                    } else {
-
-                        AppToast.show(
-                            'Vermelho',
-                            'Arquivo PDF não encontrado no servidor. O arquivo pode ter sido removido ou movido.',
-                            4000,
-                        );
+                    }
+                    else
+                    {
+                        AppToast.show('Vermelho', 'Arquivo PDF não encontrado no servidor. O arquivo pode ter sido removido ou movido.', 4000);
                         console.warn('⚠️ Arquivo não encontrado:', autuacaoPDF);
                     }
-                } catch (innerError) {
-                    Alerta.TratamentoErroComLinha(
-                        'listaautuacao.js',
-                        'btn-exibe-autuacao.verificaPDF.success',
-                        innerError,
-                    );
+                }
+                catch (innerError)
+                {
+                    Alerta.TratamentoErroComLinha("listaautuacao.js", "btn-exibe-autuacao.verificaPDF.success", innerError);
                 }
             },
-            error: function (xhr, status, error) {
-
-                console.warn(
-                    '⚠️ Erro ao verificar existência do PDF, tentando abrir:',
-                    error,
-                );
+            error: function (xhr, status, error)
+            {
+                console.warn('⚠️ Erro ao verificar existência do PDF, tentando abrir:', error);
                 exibirPDFAutuacao(autuacaoPDF);
-            },
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'listaautuacao.js',
-            'btn-exibe-autuacao.click',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("listaautuacao.js", "btn-exibe-autuacao.click", error);
     }
 });
 
-function exibirPDFAutuacao(nomeArquivo) {
-    try {
-
-        if (!nomeArquivo || nomeArquivo === '') {
+function exibirPDFAutuacao(nomeArquivo)
+{
+    try
+    {
+        if (!nomeArquivo || nomeArquivo === '')
+        {
             console.error('Nome do arquivo PDF inválido');
             return;
         }
 
         $('#modalExibePDF').modal('show');
 
-        setTimeout(function () {
-            try {
-
-                const viewerElement = document.getElementById(
-                    'pdfViewerAutuacaoControl',
-                );
-                if (!viewerElement) {
+        setTimeout(function ()
+        {
+            try
+            {
+                const viewerElement = document.getElementById('pdfViewerAutuacaoControl');
+                if (!viewerElement)
+                {
                     console.error('PDF Viewer de Autuação não encontrado');
                     return;
                 }
 
                 const pdfViewer = viewerElement.ej2_instances?.[0];
-                if (!pdfViewer) {
+                if (!pdfViewer)
+                {
                     console.error('Instância do PDF Viewer não encontrada');
                     return;
                 }
 
-                pdfViewer.documentLoadFailed = function (args) {
-                    try {
+                pdfViewer.documentLoadFailed = function (args)
+                {
+                    try
+                    {
                         console.error('❌ Falha ao carregar PDF:', args);
                         $('#modalExibePDF').modal('hide');
-                        AppToast.show(
-                            'Vermelho',
-                            'Arquivo PDF não encontrado no servidor ou corrompido',
-                            4000,
-                        );
-                    } catch (err) {
-                        console.error(
-                            'Erro no handler documentLoadFailed:',
-                            err,
-                        );
+                        AppToast.show('Vermelho', 'Arquivo PDF não encontrado no servidor ou corrompido', 4000);
+                    }
+                    catch (err)
+                    {
+                        console.error('Erro no handler documentLoadFailed:', err);
                     }
                 };
 
-                pdfViewer.documentLoad = function () {
-                    try {
-
-                        setTimeout(function () {
+                pdfViewer.documentLoad = function ()
+                {
+                    try
+                    {
+                        setTimeout(function ()
+                        {
                             pdfViewer.magnificationModule.fitToWidth();
                             console.log('✅ Zoom ajustado para FitToWidth');
                         }, 100);
-                    } catch (err) {
-                        console.warn(
-                            '⚠️ Não foi possível ajustar zoom automaticamente:',
-                            err,
-                        );
+                    } catch (err)
+                    {
+                        console.warn('⚠️ Não foi possível ajustar zoom automaticamente:', err);
                     }
                 };
 
@@ -713,39 +617,32 @@
                 pdfViewer.load(nomeArquivo, null);
 
                 console.log('✅ PDF de Autuação carregado:', nomeArquivo);
-            } catch (err) {
+            } catch (err)
+            {
                 console.error('❌ Erro ao carregar PDF:', err);
-                Alerta.TratamentoErroComLinha(
-                    'listaautuacao.js',
-                    'exibirPDFAutuacao.timeout',
-                    err,
-                );
+                Alerta.TratamentoErroComLinha("listaautuacao.js", "exibirPDFAutuacao.timeout", err);
             }
         }, 500);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'listaautuacao.js',
-            'exibirPDFAutuacao',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("listaautuacao.js", "exibirPDFAutuacao", error);
     }
 }
 
-$('#modalExibePDF').on('hidden.bs.modal', function () {
-    try {
-
-        const pdfViewer = document.getElementById('pdfViewerAutuacaoControl')
-            ?.ej2_instances?.[0];
-        if (pdfViewer) {
-
+$('#modalExibePDF').on('hidden.bs.modal', function ()
+{
+    try
+    {
+        const pdfViewer = document.getElementById('pdfViewerAutuacaoControl')?.ej2_instances?.[0];
+        if (pdfViewer)
+        {
             pdfViewer.unload();
         }
         console.log('✅ Modal Exibir PDF fechado e viewer limpo');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'listaautuacao.js',
-            'modalExibePDF.hidden',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("listaautuacao.js", "modalExibePDF.hidden", error);
     }
 });
```
