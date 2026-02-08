# wwwroot/js/cadastros/listaeventos.js

**Mudanca:** GRANDE | **+70** linhas | **-94** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/listaeventos.js
+++ ATUAL: wwwroot/js/cadastros/listaeventos.js
@@ -1,25 +1,26 @@
-function carregarDetalhamentoCustos(viagemId) {
-    try {
+function carregarDetalhamentoCustos(viagemId)
+{
+    try
+    {
         $.ajax({
             url: '/api/ViagemEvento/ObterDetalhamentoCustosViagem',
             type: 'GET',
             data: { viagemId: viagemId },
-            success: function (response) {
-                try {
-                    if (response.success) {
+            success: function (response)
+            {
+                try
+                {
+                    if (response.success)
+                    {
                         const dados = response.data;
 
-                        $('#nomeEventoDetalhes').text(
-                            'Requisitante: ' + dados.nomeRequisitante ||
-                                'Viagem',
-                        );
+                        $('#nomeEventoDetalhes').text("Requisitante: " + dados.nomeRequisitante || 'Viagem');
 
                         let dataHoraInicial = '--';
-                        if (dados.dataInicial) {
+                        if (dados.dataInicial)
+                        {
                             const dataInicial = formatarData(dados.dataInicial);
-                            const horaInicial = dados.horaInicial
-                                ? dados.horaInicial.substring(0, 5)
-                                : '--';
+                            const horaInicial = dados.horaInicial ? dados.horaInicial.substring(0, 5) : '--';
                             dataHoraInicial = `${dataInicial} às ${horaInicial}`;
                         }
                         $('#dataHoraInicialDetalhes').text(dataHoraInicial);
@@ -28,110 +29,85 @@
                         if (dados.dataFinal)
                         {
                             const dataFinal = formatarData(dados.dataFinal);
-                            const horaFinal = dados.horaFinal
-                                ? dados.horaFinal.substring(0, 5)
-                                : '--';
+                            const horaFinal = dados.horaFinal ? dados.horaFinal.substring(0, 5) : '--';
                             dataHoraFinal = `${dataFinal} às ${horaFinal}`;
                         }
                         $('#dataHoraFinalDetalhes').text(dataHoraFinal);
 
-                        const tempoTotal =
-                            dados.tempoTotalHoras !== null &&
-                            dados.tempoTotalHoras !== undefined
-                                ? dados.tempoTotalHoras.toFixed(2)
-                                : '0.00';
+                        const tempoTotal = (dados.tempoTotalHoras !== null && dados.tempoTotalHoras !== undefined)
+                            ? dados.tempoTotalHoras.toFixed(2)
+                            : '0.00';
                         $('#tempoTotalDetalhes').text(`${tempoTotal} horas`);
 
-                        $('#custoMotoristaDetalhes').text(
-                            formatarMoeda(dados.custoMotorista || 0),
-                        );
-                        $('#custoVeiculoDetalhes').text(
-                            formatarMoeda(dados.custoVeiculo || 0),
-                        );
-                        $('#custoCombustivelDetalhes').text(
-                            formatarMoeda(dados.custoCombustivel || 0),
-                        );
-                        $('#custoTotalDetalhes').text(
-                            formatarMoeda(dados.custoTotal || 0),
-                        );
-                    } else {
-                        AppToast.show(
-                            'Vermelho',
-                            response.message ||
-                                'Erro ao carregar detalhamento de custos',
-                            3000,
-                        );
+                        $('#custoMotoristaDetalhes').text(formatarMoeda(dados.custoMotorista || 0));
+                        $('#custoVeiculoDetalhes').text(formatarMoeda(dados.custoVeiculo || 0));
+                        $('#custoCombustivelDetalhes').text(formatarMoeda(dados.custoCombustivel || 0));
+                        $('#custoTotalDetalhes').text(formatarMoeda(dados.custoTotal || 0));
+                    } else
+                    {
+                        AppToast.show('Vermelho', response.message || 'Erro ao carregar detalhamento de custos', 3000);
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ListaEventos.cshtml',
-                        'carregarDetalhamentoCustos.success',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha('ListaEventos.cshtml', 'carregarDetalhamentoCustos.success', error);
                 }
             },
-            error: function (xhr, status, error) {
-                try {
-                    console.error(
-                        'Erro ao carregar detalhamento de custos:',
-                        error,
-                    );
-                    AppToast.show(
-                        'Vermelho',
-                        'Erro ao carregar detalhamento de custos',
-                        3000,
-                    );
-                } catch (err) {
+            error: function (xhr, status, error)
+            {
+                try
+                {
+                    console.error('Erro ao carregar detalhamento de custos:', error);
+                    AppToast.show('Vermelho', 'Erro ao carregar detalhamento de custos', 3000);
+                } catch (err)
+                {
                     console.error('Erro no tratamento de erro:', err);
                 }
-            },
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ListaEventos.cshtml',
-            'carregarDetalhamentoCustos',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('ListaEventos.cshtml', 'carregarDetalhamentoCustos', error);
     }
 }
 
-function carregarValorTotalEventoModal() {
-    try {
+function carregarValorTotalEventoModal()
+{
+    try
+    {
         $.ajax({
-            url: '/api/evento/ObterValorTotal',
-            type: 'GET',
+            url: "/api/evento/ObterValorTotal",
+            type: "GET",
             data: { Id: eventoIdAtual },
-            success: function (response) {
-                try {
-                    if (response.success) {
-                        $('#valorTotalEventoModal').val(
-                            formatarMoeda(response.valorTotal),
-                        );
-                    } else {
-                        $('#valorTotalEventoModal').val('R$ 0,00');
+            success: function (response)
+            {
+                try
+                {
+                    if (response.success)
+                    {
+                        $("#valorTotalEventoModal").val(formatarMoeda(response.valorTotal));
+                    } else
+                    {
+                        $("#valorTotalEventoModal").val("R$ 0,00");
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ListaEventos.cshtml',
-                        'carregarValorTotalEventoModal.success',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ListaEventos.cshtml", "carregarValorTotalEventoModal.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                try {
+            error: function (xhr, status, error)
+            {
+                try
+                {
                     console.error('Erro ao carregar valor total:', error);
-                    $('#valorTotalEventoModal').val('R$ 0,00');
-                } catch (err) {
+                    $("#valorTotalEventoModal").val("R$ 0,00");
+                } catch (err)
+                {
                     console.error('Erro no tratamento de erro:', err);
                 }
-            },
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ListaEventos.cshtml',
-            'carregarValorTotalEventoModal',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ListaEventos.cshtml", "carregarValorTotalEventoModal", error);
     }
 }
```
