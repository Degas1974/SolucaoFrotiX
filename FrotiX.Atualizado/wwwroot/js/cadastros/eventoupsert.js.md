# wwwroot/js/cadastros/eventoupsert.js

**Mudanca:** GRANDE | **+481** linhas | **-609** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/eventoupsert.js
+++ ATUAL: wwwroot/js/cadastros/eventoupsert.js
@@ -1,137 +1,120 @@
-$(document).ready(function () {
-    try {
-
-        if (
-            eventoId !== '00000000-0000-0000-0000-000000000000' &&
-            eventoId !== null
-        ) {
-            const ddtReq = document.getElementById('lstRequisitanteEvento')
-                ?.ej2_instances?.[0];
-            const ddtSet = document.getElementById('ddtSetorRequisitanteEvento')
-                ?.ej2_instances?.[0];
+$(document).ready(function ()
+{
+    try
+    {
+
+        if (eventoId !== '00000000-0000-0000-0000-000000000000' && eventoId !== null)
+        {
+            const ddtReq = document.getElementById("lstRequisitanteEvento")?.ej2_instances?.[0];
+            const ddtSet = document.getElementById("ddtSetorRequisitanteEvento")?.ej2_instances?.[0];
             if (ddtReq) ddtReq.value = requisitanteId;
             if (ddtSet) ddtSet.value = setorsolicitanteId;
-        } else {
+        } else
+        {
 
             const hoje = new Date().toISOString().slice(0, 10);
-            const di = document.getElementById('txtDataInicialEvento');
+            const di = document.getElementById("txtDataInicialEvento");
             if (di && !di.value) di.value = hoje;
         }
 
-        $('#txtQtdParticipantes').on('input', function () {
-            try {
-                const v = parseInt(this.value || '0', 10);
+        $("#txtQtdParticipantes").on("input", function ()
+        {
+            try
+            {
+                const v = parseInt(this.value || "0", 10);
                 if (v < 0) this.value = 0;
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'txtQtdParticipantes.input',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "txtQtdParticipantes.input", error);
             }
         });
 
-        function carregarEstatisticasViagens() {
-            try {
+        function carregarEstatisticasViagens()
+        {
+            try
+            {
+
                 $.ajax({
-                    url: '/api/viagem/ObterTotalCustoViagensEvento',
-                    type: 'GET',
+                    url: "/api/viagem/ObterTotalCustoViagensEvento",
+                    type: "GET",
                     data: { Id: eventoId },
-                    success: function (response) {
-                        try {
-                            if (response.success) {
-
-                                $('#totalViagens').text(response.totalViagens);
-                                $('#custoTotalViagens').text(
-                                    response.totalCustoFormatado,
-                                );
-                                $('#viagensSemCusto').text(
-                                    response.viagensSemCusto || '0',
-                                );
-
-                                const media =
-                                    response.custoMedioFormatado ||
-                                    formatarMoeda(
-                                        response.totalViagens > 0
-                                            ? response.totalCusto /
-                                                  response.totalViagens
-                                            : 0,
-                                    );
-                                $('#custoMedioViagem').text(media);
-
-                                if (response.viagensSemCusto > 0) {
-                                    $('#viagensSemCusto').addClass(
-                                        'text-danger',
-                                    );
-                                }
-
-                                console.log(
-                                    'Estatísticas carregadas:',
-                                    response,
-                                );
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'eventoupsert.js',
-                                'carregarEstatisticasViagens.success',
-                                error,
-                            );
+                    success: function (response)
+                    {
+                        try
+                        {
+                            if (response.success)
+                            {
+
+                                $("#totalViagens").text(response.totalViagens);
+                                $("#custoTotalViagens").text(response.totalCustoFormatado);
+                                $("#viagensSemCusto").text(response.viagensSemCusto || "0");
+
+                                const media = response.custoMedioFormatado ||
+                                    formatarMoeda(response.totalViagens > 0 ? response.totalCusto / response.totalViagens : 0);
+                                $("#custoMedioViagem").text(media);
+
+                                if (response.viagensSemCusto > 0)
+                                {
+                                    $("#viagensSemCusto").addClass('text-danger');
+                                }
+
+                                console.log('Estatísticas carregadas:', response);
+                            }
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarEstatisticasViagens.success", error);
                         }
                     },
-                    error: function (xhr, status, error) {
-                        try {
-                            console.error(
-                                'Erro ao carregar estatísticas:',
-                                error,
-                            );
-
-                            $('#totalViagens').text('0');
-                            $('#custoTotalViagens').text('R$ 0,00');
-                            $('#custoMedioViagem').text('R$ 0,00');
-                            $('#viagensSemCusto').text('0');
-                        } catch (err) {
-                            Alerta.TratamentoErroComLinha(
-                                'eventoupsert.js',
-                                'carregarEstatisticasViagens.error',
-                                err,
-                            );
-                        }
-                    },
+                    error: function (xhr, status, error)
+                    {
+                        try
+                        {
+                            console.error('Erro ao carregar estatísticas:', error);
+
+                            $("#totalViagens").text("0");
+                            $("#custoTotalViagens").text("R$ 0,00");
+                            $("#custoMedioViagem").text("R$ 0,00");
+                            $("#viagensSemCusto").text("0");
+                        } catch (err)
+                        {
+                            Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarEstatisticasViagens.error", err);
+                        }
+                    }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'carregarEstatisticasViagens',
-                    error,
-                );
-            }
-        }
-
-        function formatarMoeda(valor) {
-            try {
-                if (!valor && valor !== 0) return 'R$ 0,00';
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarEstatisticasViagens", error);
+            }
+        }
+
+        function formatarMoeda(valor)
+        {
+            try
+            {
+                if (!valor && valor !== 0) return "R$ 0,00";
                 return parseFloat(valor).toLocaleString('pt-BR', {
                     style: 'currency',
-                    currency: 'BRL',
+                    currency: 'BRL'
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'formatarMoeda',
-                    error,
-                );
-                return 'R$ 0,00';
-            }
-        }
-
-        if ($('#tblViagens').length) {
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "formatarMoeda", error);
+                return "R$ 0,00";
+            }
+        }
+
+        if ($("#tblViagens").length)
+        {
             initEventoTable();
         }
 
-        function initEventoTable() {
-            try {
-
-                if (typeof mostrarLoading === 'function') {
+        function initEventoTable()
+        {
+            try
+            {
+
+                if (typeof mostrarLoading === 'function')
+                {
                     mostrarLoading('Carregando Viagens do Evento...');
                 }
 
@@ -143,142 +126,139 @@
                     responsive: true,
                     processing: false,
                     ajax: {
-                        url: '/api/viagem/listaviagensevento',
-                        type: 'GET',
+                        url: "/api/viagem/listaviagensevento",
+                        type: "GET",
                         data: { Id: eventoId },
                         dataSrc: 'data',
-                        beforeSend: function () {
+                        beforeSend: function ()
+                        {
                             console.time('Requisição API');
                         },
-                        complete: function (data) {
-                            try {
-
-                                if (typeof esconderLoading === 'function') {
+                        complete: function (data)
+                        {
+                            try
+                            {
+
+                                if (typeof esconderLoading === 'function')
+                                {
                                     esconderLoading();
                                 }
 
                                 console.timeEnd('Requisição API');
-                                console.log(
-                                    'Quantidade de registros:',
-                                    data.responseJSON?.data?.length,
-                                );
+                                console.log('Quantidade de registros:', data.responseJSON?.data?.length);
                                 carregarEstatisticasViagens();
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'eventoupsert.js',
-                                    'DataTable.ajax.complete',
-                                    error,
-                                );
-                            }
-                        },
-                        error: function (xhr, status, error) {
-
-                            if (typeof esconderLoading === 'function') {
+                            } catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha("eventoupsert.js", "DataTable.ajax.complete", error);
+                            }
+                        },
+                        error: function (xhr, status, error)
+                        {
+
+                            if (typeof esconderLoading === 'function')
+                            {
                                 esconderLoading();
                             }
                             console.error('Erro ao carregar viagens:', error);
-                        },
+                        }
                     },
                     columns: [
                         {
-                            data: 'noFichaVistoria',
-                            className: 'text-center',
-                            width: '5%',
-                            render: function (data) {
+                            data: "noFichaVistoria",
+                            className: "text-center",
+                            width: "5%",
+                            render: function (data)
+                            {
                                 return data || '-';
-                            },
-                        },
-                        {
-                            data: 'dataInicial',
-                            className: 'text-center',
-                            width: '7%',
-                            render: function (data, type, row) {
+                            }
+                        },
+                        {
+                            data: "dataInicial",
+                            className: "text-center",
+                            width: "7%",
+                            render: function (data, type, row)
+                            {
                                 if (!data) return '-';
-                                if (type === 'display') {
+                                if (type === 'display')
+                                {
                                     const date = new Date(data);
-                                    const dia = date
-                                        .getDate()
-                                        .toString()
-                                        .padStart(2, '0');
-                                    const mes = (date.getMonth() + 1)
-                                        .toString()
-                                        .padStart(2, '0');
+                                    const dia = date.getDate().toString().padStart(2, '0');
+                                    const mes = (date.getMonth() + 1).toString().padStart(2, '0');
                                     const ano = date.getFullYear();
                                     return `${dia}/${mes}/${ano}`;
                                 }
                                 return data;
-                            },
-                        },
-                        {
-                            data: 'horaInicio',
-                            className: 'text-center',
-                            width: '5%',
-                            render: function (data, type, row) {
+                            }
+                        },
+                        {
+                            data: "horaInicio",
+                            className: "text-center",
+                            width: "5%",
+                            render: function (data, type, row)
+                            {
                                 if (!data) return '-';
-                                if (type === 'display') {
+                                if (type === 'display')
+                                {
                                     const date = new Date(data);
-                                    const horas = date
-                                        .getHours()
-                                        .toString()
-                                        .padStart(2, '0');
-                                    const minutos = date
-                                        .getMinutes()
-                                        .toString()
-                                        .padStart(2, '0');
+                                    const horas = date.getHours().toString().padStart(2, '0');
+                                    const minutos = date.getMinutes().toString().padStart(2, '0');
                                     return `${horas}:${minutos}`;
                                 }
                                 return data;
-                            },
-                        },
-                        {
-                            data: 'nomeRequisitante',
-                            className: 'text-left',
-                            width: '18%',
-                        },
-                        {
-                            data: 'nomeSetor',
-                            className: 'text-left',
-                            width: '18%',
-                        },
-                        {
-                            data: 'nomeMotorista',
-                            className: 'text-left',
-                            width: '12%',
-                            render: function (data) {
-                                return (
-                                    data || '<span class="text-muted">-</span>'
-                                );
-                            },
-                        },
-                        {
-                            data: 'descricaoVeiculo',
-                            className: 'text-left',
-                            width: '15%',
-                        },
-                        {
-                            data: 'custoViagem',
-                            className: 'text-end',
-                            width: '10%',
-                            render: function (data, type, row) {
-                                if (type === 'display') {
-                                    if (data === null || data === undefined) {
+                            }
+                        },
+                        {
+                            data: "nomeRequisitante",
+                            className: "text-left",
+                            width: "18%"
+                        },
+                        {
+                            data: "nomeSetor",
+                            className: "text-left",
+                            width: "18%"
+                        },
+                        {
+                            data: "nomeMotorista",
+                            className: "text-left",
+                            width: "12%",
+                            render: function (data)
+                            {
+                                return data || '<span class="text-muted">-</span>';
+                            }
+                        },
+                        {
+                            data: "descricaoVeiculo",
+                            className: "text-left",
+                            width: "15%"
+                        },
+                        {
+                            data: "custoViagem",
+                            className: "text-end",
+                            width: "10%",
+                            render: function (data, type, row)
+                            {
+                                if (type === 'display')
+                                {
+                                    if (data === null || data === undefined)
+                                    {
                                         return '<span class="text-muted">-</span>';
                                     }
                                     return data.toLocaleString('pt-BR', {
                                         style: 'currency',
-                                        currency: 'BRL',
+                                        currency: 'BRL'
                                     });
                                 }
                                 return parseFloat(data) || 0;
-                            },
-                        },
-                        {
-                            data: 'viagemId',
-                            className: 'text-center',
-                            width: '10%',
+                            }
+                        },
+                        {
+                            data: "viagemId",
+                            className: "text-center",
+                            width: "10%",
                             orderable: false,
                             searchable: false,
-                            render: function (data, type, row) {
+                            render: function (data, type, row)
+                            {
                                 return `
                                     <div class="d-flex justify-content-center gap-1">
                                         <a class="btn fundo-roxo text-white btn-icon-28 btn-custos-viagem"
@@ -296,239 +276,207 @@
                                             <i class="fa-duotone fa-link-slash"></i>
                                         </a>
                                     </div>`;
-                            },
-                        },
+                            }
+                        }
                     ],
                     language: {
-                        url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                        processing: 'Carregando...',
-                        emptyTable:
-                            'Nenhuma viagem realizada encontrada para este evento',
+                        url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                        processing: "Carregando...",
+                        emptyTable: "Nenhuma viagem realizada encontrada para este evento"
                     },
                     pageLength: 10,
-                    dom: 'rtip',
+                    dom: 'rtip'
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'initEventoTable',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "initEventoTable", error);
             }
         }
 
         carregarEstatisticasViagens();
 
-        $('#tblViagens').on('draw.dt', function () {
-            try {
+        $('#tblViagens').on('draw.dt', function ()
+        {
+            try
+            {
                 carregarEstatisticasViagens();
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'tblViagens.draw',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "tblViagens.draw", error);
             }
         });
 
-        function atualizarTotalViagens() {
-            try {
+        function atualizarTotalViagens()
+        {
+            try
+            {
                 $.ajax({
-                    url: '/api/viagem/ObterTotalCustoViagensEvento',
-                    type: 'GET',
+                    url: "/api/viagem/ObterTotalCustoViagensEvento",
+                    type: "GET",
                     data: { Id: eventoId },
-                    success: function (response) {
-                        try {
-                            if (response.success) {
-
-                                $('#TituloViagens').html(
+                    success: function (response)
+                    {
+                        try
+                        {
+                            if (response.success)
+                            {
+
+                                $("#TituloViagens").html(
                                     `Viagens associadas ao Evento - ` +
-                                        `Total de ${response.totalViagens} viagem(ns) - ` +
-                                        `Custo Total: ${response.totalCustoFormatado}`,
+                                    `Total de ${response.totalViagens} viagem(ns) - ` +
+                                    `Custo Total: ${response.totalCustoFormatado}`
                                 );
 
-                                $('#totalViagens').text(response.totalViagens);
-                                $('#custoTotal').text(
-                                    response.totalCustoFormatado,
+                                $("#totalViagens").text(response.totalViagens);
+                                $("#custoTotal").text(response.totalCustoFormatado);
+
+                                console.log('Total calculado com sucesso:', response);
+                            } else
+                            {
+                                console.error('Erro ao calcular total:', response.error);
+                                $("#TituloViagens").html(
+                                    `Viagens associadas ao Evento - Erro ao calcular total`
                                 );
-
-                                console.log(
-                                    'Total calculado com sucesso:',
-                                    response,
-                                );
-                            } else {
-                                console.error(
-                                    'Erro ao calcular total:',
-                                    response.error,
-                                );
-                                $('#TituloViagens').html(
-                                    `Viagens associadas ao Evento - Erro ao calcular total`,
-                                );
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'eventoupsert.js',
-                                'atualizarTotalViagens.success',
-                                error,
+                            }
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarTotalViagens.success", error);
+                        }
+                    },
+                    error: function (xhr, status, error)
+                    {
+                        try
+                        {
+                            console.error('Erro na requisição:', error);
+                            $("#TituloViagens").html(
+                                `Viagens associadas ao Evento - Erro ao carregar total`
                             );
-                        }
-                    },
-                    error: function (xhr, status, error) {
-                        try {
-                            console.error('Erro na requisição:', error);
-                            $('#TituloViagens').html(
-                                `Viagens associadas ao Evento - Erro ao carregar total`,
-                            );
-                        } catch (err) {
-                            Alerta.TratamentoErroComLinha(
-                                'eventoupsert.js',
-                                'atualizarTotalViagens.error',
-                                err,
-                            );
-                        }
-                    },
+                        } catch (err)
+                        {
+                            Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarTotalViagens.error", err);
+                        }
+                    }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'atualizarTotalViagens',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarTotalViagens", error);
             }
         }
 
         atualizarTotalViagens();
 
-        $('#tblViagens').on('draw.dt', function () {
-            try {
+        $('#tblViagens').on('draw.dt', function ()
+        {
+            try
+            {
                 atualizarTotalViagens();
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'tblViagens.draw.atualizarTotal',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "tblViagens.draw.atualizarTotal", error);
             }
         });
 
-        if (
-            eventoId !== '00000000-0000-0000-0000-000000000000' &&
-            eventoId !== null
-        ) {
-            const ddtReq = document.getElementById('lstRequisitanteEvento')
-                ?.ej2_instances?.[0];
-            const ddlSetor = document.getElementById(
-                'ddlSetorRequisitanteEvento',
-            );
-
-            if (ddtReq && requisitanteId) {
+        if (eventoId !== '00000000-0000-0000-0000-000000000000' && eventoId !== null)
+        {
+            const ddtReq = document.getElementById("lstRequisitanteEvento")?.ej2_instances?.[0];
+            const ddlSetor = document.getElementById('ddlSetorRequisitanteEvento');
+
+            if (ddtReq && requisitanteId)
+            {
                 ddtReq.value = [requisitanteId];
             }
 
-            if (ddlSetor && setorsolicitanteId) {
+            if (ddlSetor && setorsolicitanteId)
+            {
                 ddlSetor.value = setorsolicitanteId;
             }
         }
 
-        $(document).on('click', '.btn-custos-viagem', function (e) {
-            try {
+        $(document).on('click', '.btn-custos-viagem', function (e)
+        {
+            try
+            {
                 e.preventDefault();
                 const viagemId = $(this).data('id');
                 const requisitante = $(this).data('requisitante');
 
                 console.log('Clique no botão de custos, Viagem ID:', viagemId);
 
-                if (!viagemId) {
+                if (!viagemId)
+                {
                     console.error('ID da viagem não encontrado');
-                    AppToast.show(
-                        'Vermelho',
-                        'Erro ao abrir modal: ID não encontrado',
-                        3000,
-                    );
+                    AppToast.show('Vermelho', 'Erro ao abrir modal: ID não encontrado', 3000);
                     return;
                 }
 
-                $('#requisitanteCustos').text(
-                    'Requisitante: ' + (requisitante || 'Não informado'),
-                );
+                $('#requisitanteCustos').text('Requisitante: ' + (requisitante || 'Não informado'));
 
                 carregarDetalhamentoCustos(viagemId);
 
-                const modalElement =
-                    document.getElementById('modalCustosViagem');
-                if (modalElement) {
+                const modalElement = document.getElementById('modalCustosViagem');
+                if (modalElement)
+                {
                     const modal = new bootstrap.Modal(modalElement);
                     modal.show();
-                } else {
+                } else
+                {
                     console.error('Modal de custos não encontrado no DOM');
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'btn-custos-viagem.click',
-                    error,
-                );
+
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "btn-custos-viagem.click", error);
             }
         });
 
-        $(document).on('click', '.btn-desassociar-viagem', function (e) {
-            try {
+        $(document).on('click', '.btn-desassociar-viagem', function (e)
+        {
+            try
+            {
                 e.preventDefault();
                 const viagemId = $(this).data('id');
                 const ficha = $(this).data('ficha');
                 const requisitante = $(this).data('requisitante');
 
-                console.log(
-                    'Clique no botão de desassociar, Viagem ID:',
-                    viagemId,
-                );
-
-                if (!viagemId) {
+                console.log('Clique no botão de desassociar, Viagem ID:', viagemId);
+
+                if (!viagemId)
+                {
                     console.error('ID da viagem não encontrado');
-                    AppToast.show(
-                        'Vermelho',
-                        'Erro ao abrir modal: ID não encontrado',
-                        3000,
-                    );
+                    AppToast.show('Vermelho', 'Erro ao abrir modal: ID não encontrado', 3000);
                     return;
                 }
 
                 $('#viagemIdDesassociar').val(viagemId);
-                $('#infoViagemDesassociar').text(
-                    `Ficha ${ficha || '-'} - ${requisitante || 'Não informado'}`,
-                );
+                $('#infoViagemDesassociar').text(`Ficha ${ficha || '-'} - ${requisitante || 'Não informado'}`);
                 $('#lstNovaFinalidade').val('');
 
-                const modalElement =
-                    document.getElementById('modalDesassociar');
-                if (modalElement) {
+                const modalElement = document.getElementById('modalDesassociar');
+                if (modalElement)
+                {
                     const modal = new bootstrap.Modal(modalElement);
                     modal.show();
-                } else {
-                    console.error(
-                        'Modal de desassociação não encontrado no DOM',
-                    );
+                } else
+                {
+                    console.error('Modal de desassociação não encontrado no DOM');
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'btn-desassociar-viagem.click',
-                    error,
-                );
+
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "btn-desassociar-viagem.click", error);
             }
         });
 
-        $('#btnConfirmarDesassociar').on('click', function () {
-            try {
+        $('#btnConfirmarDesassociar').on('click', function ()
+        {
+            try
+            {
                 const viagemId = $('#viagemIdDesassociar').val();
                 const novaFinalidade = $('#lstNovaFinalidade').val();
 
-                if (!novaFinalidade) {
-                    AppToast.show(
-                        'Amarelo',
-                        'Selecione uma nova finalidade para a viagem!',
-                        3000,
-                    );
+                if (!novaFinalidade)
+                {
+                    AppToast.show('Amarelo', 'Selecione uma nova finalidade para a viagem!', 3000);
                     $('#lstNovaFinalidade').focus();
                     return;
                 }
@@ -536,99 +484,79 @@
                 const btn = $(this);
                 const textoOriginal = btn.html();
                 btn.prop('disabled', true);
-                btn.html(
-                    '<i class="fa-solid fa-spinner fa-spin icon-space"></i> Processando...',
-                );
+                btn.html('<i class="fa-solid fa-spinner fa-spin icon-space"></i> Processando...');
 
                 $.ajax({
-                    url: '/api/viagem/DesassociarViagemEvento',
-                    type: 'POST',
-                    contentType: 'application/json; charset=utf-8',
+                    url: "/api/viagem/DesassociarViagemEvento",
+                    type: "POST",
+                    contentType: "application/json; charset=utf-8",
                     data: JSON.stringify({
                         ViagemId: viagemId,
-                        NovaFinalidade: novaFinalidade,
+                        NovaFinalidade: novaFinalidade
                     }),
-                    success: function (response) {
-                        try {
-                            if (response.success) {
-                                AppToast.show(
-                                    'Verde',
-                                    response.message ||
-                                        'Viagem desassociada com sucesso!',
-                                    3000,
-                                );
-
-                                const modalElement =
-                                    document.getElementById('modalDesassociar');
-                                const modal =
-                                    bootstrap.Modal.getInstance(modalElement);
+                    success: function (response)
+                    {
+                        try
+                        {
+                            if (response.success)
+                            {
+                                AppToast.show('Verde', response.message || 'Viagem desassociada com sucesso!', 3000);
+
+                                const modalElement = document.getElementById('modalDesassociar');
+                                const modal = bootstrap.Modal.getInstance(modalElement);
                                 if (modal) modal.hide();
 
-                                if ($.fn.DataTable.isDataTable('#tblViagens')) {
-                                    $('#tblViagens')
-                                        .DataTable()
-                                        .ajax.reload(null, false);
+                                if ($.fn.DataTable.isDataTable('#tblViagens'))
+                                {
+                                    $('#tblViagens').DataTable().ajax.reload(null, false);
                                 }
 
                                 carregarEstatisticasViagens();
                                 atualizarTotalViagens();
-                            } else {
-                                AppToast.show(
-                                    'Vermelho',
-                                    response.message ||
-                                        'Erro ao desassociar viagem',
-                                    3000,
-                                );
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'eventoupsert.js',
-                                'btnConfirmarDesassociar.success',
-                                error,
-                            );
+                            } else
+                            {
+                                AppToast.show('Vermelho', response.message || 'Erro ao desassociar viagem', 3000);
+                            }
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("eventoupsert.js", "btnConfirmarDesassociar.success", error);
                         }
                     },
-                    error: function (xhr, status, error) {
-                        try {
+                    error: function (xhr, status, error)
+                    {
+                        try
+                        {
                             console.error('Erro ao desassociar viagem:', error);
-                            AppToast.show(
-                                'Vermelho',
-                                'Erro ao desassociar viagem do evento',
-                                3000,
-                            );
-                        } catch (err) {
-                            Alerta.TratamentoErroComLinha(
-                                'eventoupsert.js',
-                                'btnConfirmarDesassociar.error',
-                                err,
-                            );
+                            AppToast.show('Vermelho', 'Erro ao desassociar viagem do evento', 3000);
+                        } catch (err)
+                        {
+                            Alerta.TratamentoErroComLinha("eventoupsert.js", "btnConfirmarDesassociar.error", err);
                         }
                     },
-                    complete: function () {
-                        try {
+                    complete: function ()
+                    {
+                        try
+                        {
 
                             btn.prop('disabled', false);
                             btn.html(textoOriginal);
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'eventoupsert.js',
-                                'btnConfirmarDesassociar.complete',
-                                error,
-                            );
-                        }
-                    },
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("eventoupsert.js", "btnConfirmarDesassociar.complete", error);
+                        }
+                    }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'btnConfirmarDesassociar.click',
-                    error,
-                );
+
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "btnConfirmarDesassociar.click", error);
             }
         });
 
-        function carregarDetalhamentoCustos(viagemId) {
-            try {
+        function carregarDetalhamentoCustos(viagemId)
+        {
+            try
+            {
 
                 $('#infoViagemCustos').text('--');
                 $('#tempoTotalCustos').text('-');
@@ -640,272 +568,223 @@
                 $('#custoTotalCustos').text('R$ 0,00');
 
                 $.ajax({
-                    url: '/api/viagem/ObterCustosViagem',
-                    type: 'GET',
+                    url: "/api/viagem/ObterCustosViagem",
+                    type: "GET",
                     data: { viagemId: viagemId },
-                    success: function (response) {
-                        try {
-                            if (response.success && response.data) {
+                    success: function (response)
+                    {
+                        try
+                        {
+                            if (response.success && response.data)
+                            {
                                 const d = response.data;
 
                                 var infoViagem = d.infoViagem ?? d.InfoViagem;
-                                if (infoViagem) {
+                                if (infoViagem)
+                                {
                                     $('#infoViagemCustos').text(infoViagem);
                                 }
 
-                                var duracaoFormatada =
-                                    d.duracaoFormatada ?? d.DuracaoFormatada;
-                                if (duracaoFormatada) {
-                                    $('#tempoTotalCustos').text(
-                                        duracaoFormatada,
-                                    );
-                                }
-
-                                var kmPercorrido =
-                                    d.kmPercorrido ?? d.KmPercorrido;
-                                if (kmPercorrido !== undefined) {
-                                    $('#kmPercorridoCustos').text(
-                                        kmPercorrido + ' km',
-                                    );
-                                }
-
-                                var litrosGastos =
-                                    d.litrosGastos ?? d.LitrosGastos;
-                                if (
-                                    litrosGastos !== undefined &&
-                                    litrosGastos > 0
-                                ) {
-                                    $('#litrosGastosCustos').text(
-                                        litrosGastos
-                                            .toFixed(2)
-                                            .replace('.', ',') + ' L',
-                                    );
-                                }
-
-                                var custoMotorista =
-                                    d.custoMotorista ?? d.CustoMotorista ?? 0;
-                                var custoVeiculo =
-                                    d.custoVeiculo ?? d.CustoVeiculo ?? 0;
-                                var custoCombustivel =
-                                    d.custoCombustivel ??
-                                    d.CustoCombustivel ??
-                                    0;
-                                var custoTotal =
-                                    d.custoTotal ?? d.CustoTotal ?? 0;
-
-                                $('#custoMotoristaCustos').text(
-                                    formatarMoeda(custoMotorista),
-                                );
-                                $('#custoVeiculoCustos').text(
-                                    formatarMoeda(custoVeiculo),
-                                );
-                                $('#custoCombustivelCustos').text(
-                                    formatarMoeda(custoCombustivel),
-                                );
-
-                                $('#custoTotalCustos').text(
-                                    formatarMoeda(custoTotal),
-                                );
+                                var duracaoFormatada = d.duracaoFormatada ?? d.DuracaoFormatada;
+                                if (duracaoFormatada)
+                                {
+                                    $('#tempoTotalCustos').text(duracaoFormatada);
+                                }
+
+                                var kmPercorrido = d.kmPercorrido ?? d.KmPercorrido;
+                                if (kmPercorrido !== undefined)
+                                {
+                                    $('#kmPercorridoCustos').text(kmPercorrido + ' km');
+                                }
+
+                                var litrosGastos = d.litrosGastos ?? d.LitrosGastos;
+                                if (litrosGastos !== undefined && litrosGastos > 0)
+                                {
+                                    $('#litrosGastosCustos').text(litrosGastos.toFixed(2).replace('.', ',') + ' L');
+                                }
+
+                                var custoMotorista = d.custoMotorista ?? d.CustoMotorista ?? 0;
+                                var custoVeiculo = d.custoVeiculo ?? d.CustoVeiculo ?? 0;
+                                var custoCombustivel = d.custoCombustivel ?? d.CustoCombustivel ?? 0;
+                                var custoTotal = d.custoTotal ?? d.CustoTotal ?? 0;
+
+                                $('#custoMotoristaCustos').text(formatarMoeda(custoMotorista));
+                                $('#custoVeiculoCustos').text(formatarMoeda(custoVeiculo));
+                                $('#custoCombustivelCustos').text(formatarMoeda(custoCombustivel));
+
+                                $('#custoTotalCustos').text(formatarMoeda(custoTotal));
 
                                 console.log('Custos carregados:', d);
-                            } else {
-                                console.warn(
-                                    'Dados de custos não encontrados:',
-                                    response.message,
-                                );
-                                AppToast.show(
-                                    'Amarelo',
-                                    response.message ||
-                                        'Dados de custos não disponíveis',
-                                    2000,
-                                );
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'eventoupsert.js',
-                                'carregarDetalhamentoCustos.success',
-                                error,
-                            );
+                            } else
+                            {
+                                console.warn('Dados de custos não encontrados:', response.message);
+                                AppToast.show('Amarelo', response.message || 'Dados de custos não disponíveis', 2000);
+                            }
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarDetalhamentoCustos.success", error);
                         }
                     },
-                    error: function (xhr, status, error) {
-                        try {
+                    error: function (xhr, status, error)
+                    {
+                        try
+                        {
                             console.error('Erro ao carregar custos:', error);
-                            AppToast.show(
-                                'Vermelho',
-                                'Erro ao carregar detalhamento de custos',
-                                3000,
-                            );
-                        } catch (err) {
-                            Alerta.TratamentoErroComLinha(
-                                'eventoupsert.js',
-                                'carregarDetalhamentoCustos.error',
-                                err,
-                            );
-                        }
-                    },
+                            AppToast.show('Vermelho', 'Erro ao carregar detalhamento de custos', 3000);
+                        } catch (err)
+                        {
+                            Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarDetalhamentoCustos.error", err);
+                        }
+                    }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'carregarDetalhamentoCustos',
-                    error,
-                );
-            }
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'eventoupsert.js',
-            'document.ready',
-            error,
-        );
+
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarDetalhamentoCustos", error);
+            }
+        }
+
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("eventoupsert.js", "document.ready", error);
     }
 });
 
-function calcularTotalViagens() {
-    try {
+function calcularTotalViagens()
+{
+    try
+    {
         const table = $('#tblViagens').DataTable();
         let total = 0;
 
-        table.rows({ page: 'current' }).every(function () {
+        table.rows({ page: 'current' }).every(function ()
+        {
             const data = this.data();
-            if (data && data.custoViagem) {
+            if (data && data.custoViagem)
+            {
                 total += parseFloat(data.custoViagem) || 0;
             }
         });
 
-        $('#TituloViagens').html(
-            `Viagens associadas ao Evento - Custo Total: R$ ${total.toFixed(2).replace('.', ',')}`,
+        $("#TituloViagens").html(
+            `Viagens associadas ao Evento - Custo Total: R$ ${total.toFixed(2).replace('.', ',')}`
         );
 
         return total;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'eventoupsert.js',
-            'calcularTotalViagens',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("eventoupsert.js", "calcularTotalViagens", error);
         return 0;
     }
 }
 
-document.addEventListener('DOMContentLoaded', function () {
-    try {
-
-        function atualizarCampoSetor() {
-            try {
-
-                setTimeout(function () {
-                    try {
-
-                        var setorDropDown = document.getElementById(
-                            'ddtSetorRequisitanteEvento',
-                        );
-                        if (
-                            setorDropDown &&
-                            setorDropDown.ej2_instances &&
-                            setorDropDown.ej2_instances[0]
-                        ) {
+document.addEventListener('DOMContentLoaded', function ()
+{
+    try
+    {
+
+        function atualizarCampoSetor()
+        {
+            try
+            {
+
+                setTimeout(function ()
+                {
+                    try
+                    {
+
+                        var setorDropDown = document.getElementById('ddtSetorRequisitanteEvento');
+                        if (setorDropDown && setorDropDown.ej2_instances && setorDropDown.ej2_instances[0])
+                        {
                             var setorInstance = setorDropDown.ej2_instances[0];
 
                             var textoSetor = setorInstance.text || '';
 
-                            if (!textoSetor && setorInstance.value) {
+                            if (!textoSetor && setorInstance.value)
+                            {
 
                                 var selectedData = setorInstance.treeData;
-                                if (selectedData && selectedData.length > 0) {
-                                    textoSetor = findTextByValue(
-                                        selectedData,
-                                        setorInstance.value[0],
-                                    );
-                                }
-                            }
-
-                            document.getElementById(
-                                'txtSetorRequisitante',
-                            ).value = textoSetor;
-                        }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'eventoupsert.js',
-                            'atualizarCampoSetor.setTimeout',
-                            error,
-                        );
+                                if (selectedData && selectedData.length > 0)
+                                {
+                                    textoSetor = findTextByValue(selectedData, setorInstance.value[0]);
+                                }
+                            }
+
+                            document.getElementById('txtSetorRequisitante').value = textoSetor;
+                        }
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarCampoSetor.setTimeout", error);
                     }
                 }, 100);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'atualizarCampoSetor',
-                    error,
-                );
-            }
-        }
-
-        function findTextByValue(data, value) {
-            try {
-                for (var i = 0; i < data.length; i++) {
-                    if (data[i].SetorSolicitanteId === value) {
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarCampoSetor", error);
+            }
+        }
+
+        function findTextByValue(data, value)
+        {
+            try
+            {
+                for (var i = 0; i < data.length; i++)
+                {
+                    if (data[i].SetorSolicitanteId === value)
+                    {
                         return data[i].Nome;
                     }
 
-                    if (data[i].child) {
+                    if (data[i].child)
+                    {
                         var found = findTextByValue(data[i].child, value);
                         if (found) return found;
                     }
                 }
                 return '';
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'findTextByValue',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "findTextByValue", error);
                 return '';
             }
         }
 
-        var setorDropDown = document.getElementById(
-            'ddtSetorRequisitanteEvento',
-        );
-        if (setorDropDown) {
+        var setorDropDown = document.getElementById('ddtSetorRequisitanteEvento');
+        if (setorDropDown)
+        {
             setorDropDown.addEventListener('change', atualizarCampoSetor);
 
             setTimeout(atualizarCampoSetor, 500);
         }
 
-        var observer = new MutationObserver(function (mutations) {
-            try {
-                mutations.forEach(function (mutation) {
-                    if (
-                        mutation.type === 'attributes' ||
-                        mutation.type === 'childList'
-                    ) {
+        var observer = new MutationObserver(function (mutations)
+        {
+            try
+            {
+                mutations.forEach(function (mutation)
+                {
+                    if (mutation.type === 'attributes' || mutation.type === 'childList')
+                    {
                         atualizarCampoSetor();
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'eventoupsert.js',
-                    'MutationObserver.callback',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("eventoupsert.js", "MutationObserver.callback", error);
             }
         });
 
-        if (setorDropDown) {
+        if (setorDropDown)
+        {
             observer.observe(setorDropDown, {
                 attributes: true,
                 childList: true,
                 subtree: true,
-                attributeFilter: ['value'],
+                attributeFilter: ['value']
             });
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'eventoupsert.js',
-            'DOMContentLoaded',
-            error,
-        );
+
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("eventoupsert.js", "DOMContentLoaded", error);
     }
 });
```
