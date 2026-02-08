# wwwroot/js/cadastros/ListaEscala.js

**Mudanca:** GRANDE | **+149** linhas | **-385** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/ListaEscala.js
+++ ATUAL: wwwroot/js/cadastros/ListaEscala.js
@@ -1,18 +1,15 @@
 let gridEscalas = null;
 
-window.visualizarEscala = function (escalaId) {
-    try {
-        console.log('👁️ visualizarEscala chamada com ID:', escalaId);
+window.visualizarEscala = function(escalaId) {
+    try {
 
         if (!escalaId) {
-            console.error('❌ ID da escala não informado');
             AppToast.show('Vermelho', 'ID da escala não informado', 2000);
             return;
         }
 
         var modalElement = document.getElementById('modalVisualizarEscala');
         if (!modalElement) {
-            console.error('❌ Modal não encontrado no DOM');
             AppToast.show('Vermelho', 'Erro: Modal não encontrado', 2000);
             return;
         }
@@ -32,53 +29,32 @@
             data: { id: escalaId },
             success: function (response) {
                 try {
-                    console.log('✅ Resposta AJAX:', response);
-
                     if (response.success && response.data) {
                         preencherModalVisualizacao(response.data);
                     } else {
-                        console.error('❌ Erro na resposta:', response.message);
-                        AppToast.show(
-                            'Vermelho',
-                            response.message || 'Erro ao carregar dados',
-                            3000,
-                        );
+                        AppToast.show('Vermelho', response.message || 'Erro ao carregar dados', 3000);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ListaEscala.js',
-                        'visualizarEscala.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("ListaEscala.js", "visualizarEscala.success", error);
                 }
             },
             error: function (xhr, status, error) {
-                console.error('❌ Erro AJAX:', error);
-                console.error('❌ Status:', xhr.status);
-                console.error('❌ Response:', xhr.responseText);
-                AppToast.show(
-                    'Vermelho',
-                    'Erro ao carregar dados da escala',
-                    3000,
-                );
+                AppToast.show('Vermelho', 'Erro ao carregar dados da escala', 3000);
             },
             complete: function () {
+
                 if (loadingElement) loadingElement.style.display = 'none';
                 if (conteudoElement) conteudoElement.style.display = 'block';
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ListaEscala.js',
-            'visualizarEscala',
-            error,
-        );
+            }
+        });
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('ListaEscala.js', 'visualizarEscala', error);
     }
 };
 
-window.excluirEscala = function (id) {
-    try {
-        console.log('🗑️ excluirEscala chamada com ID:', id);
+window.excluirEscala = function(id) {
+    try {
 
         if (!id) {
             AppToast.show('Vermelho', 'ID da escala não informado', 2000);
@@ -91,18 +67,16 @@
 
         window.location.href = '/Escalas/ListaEscala?handler=Delete&id=' + id;
     } catch (error) {
-        Alerta.TratamentoErroComLinha('ListaEscala.js', 'excluirEscala', error);
+        Alerta.TratamentoErroComLinha("ListaEscala.js", "excluirEscala", error);
     }
 };
 
-window.getStatusClass = function (status) {
-    try {
+window.getStatusClass = function(status) {
+    try {
+
         if (!status) return 'secondary';
 
-        const statusLower = String(status)
-            .toLowerCase()
-            .normalize('NFD')
-            .replace(/[\u0300-\u036f]/g, '');
+        const statusLower = String(status).toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');
 
         if (statusLower.includes('indisponivel')) return 'indisponivel';
         if (statusLower.includes('disponivel')) return 'disponivel';
@@ -120,203 +94,134 @@
 
 $(document).ready(function () {
     try {
-        console.log('✅ ListaEscala.js inicializando...');
-
-        $(document).on('click', '.btn-visualizar', function (e) {
+
+        $(document).on('click', '.btn-visualizar', function(e) {
             try {
                 e.preventDefault();
                 e.stopPropagation();
                 var escalaId = $(this).data('id');
-                console.log('👁️ Clique em visualizar, ID:', escalaId);
                 if (escalaId) {
                     visualizarEscala(escalaId);
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ListaEscala.js',
-                    'btn-visualizar.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.btn-excluir', function (e) {
+                Alerta.TratamentoErroComLinha("ListaEscala.js", "btn-visualizar.click", error);
+            }
+        });
+
+        $(document).on('click', '.btn-excluir', function(e) {
             try {
                 e.preventDefault();
                 e.stopPropagation();
                 var escalaId = $(this).data('id');
-                console.log('🗑️ Clique em excluir, ID:', escalaId);
                 if (escalaId) {
                     excluirEscala(escalaId);
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ListaEscala.js',
-                    'btn-excluir.click',
-                    error,
-                );
-            }
-        });
-
-        console.log('✅ Event delegation configurado');
+                Alerta.TratamentoErroComLinha("ListaEscala.js", "btn-excluir.click", error);
+            }
+        });
 
         setTimeout(function () {
             try {
                 const gridElement = document.getElementById('gridEscalas');
 
-                if (
-                    gridElement &&
-                    gridElement.ej2_instances &&
-                    gridElement.ej2_instances.length > 0
-                ) {
+                if (gridElement && gridElement.ej2_instances && gridElement.ej2_instances.length > 0) {
                     gridEscalas = gridElement.ej2_instances[0];
-                    console.log('✅ Grid Syncfusion inicializado');
-
-                    const totalRegistros = gridEscalas.dataSource
-                        ? gridEscalas.dataSource.length
-                        : 0;
-                    console.log('📊 Registros no grid:', totalRegistros);
+
+                    const totalRegistros = gridEscalas.dataSource ? gridEscalas.dataSource.length : 0;
 
                     if (totalRegistros > 0) {
-                        AppToast.show(
-                            'Verde',
-                            `${totalRegistros} escala(s) carregada(s)`,
-                            3000,
-                        );
+                        AppToast.show('Verde', `${totalRegistros} escala(s) carregada(s)`, 3000);
                     } else {
-                        AppToast.show(
-                            'Amarelo',
-                            'Nenhuma escala encontrada para hoje',
-                            3000,
-                        );
+                        AppToast.show('Amarelo', 'Nenhuma escala encontrada para hoje', 3000);
                     }
 
                     configurarEventosGrid();
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ListaEscala.js',
-                    'setTimeout',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("ListaEscala.js", "setTimeout", error);
             }
         }, 300);
 
         configurarEventos();
 
-        setTimeout(function () {
+        setTimeout(function() {
             carregarObservacoesDia();
         }, 500);
 
-        $('#btnFichaEscala')
-            .off('click')
-            .on('click', function (e) {
+        $('#btnFichaEscala').off('click').on('click', function(e) {
+            try {
+                e.preventDefault();
+                var dataFiltro = obterValorComponente('DataEscala');
+                var url = '/Escalas/FichaEscalas';
+
+                if (dataFiltro) {
+                    var dataFormatada = new Date(dataFiltro).toISOString().split('T')[0];
+                    url += '?DataEscala=' + dataFormatada;
+                }
+                window.location.href = url;
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("ListaEscala.js", "btnFichaEscala.click", error);
+
+                window.location.href = '/Escalas/FichaEscalas';
+            }
+        });
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("ListaEscala.js", "document.ready", error);
+    }
+});
+
+function configurarEventos() {
+    try {
+
+        $('#btnFiltrar').off('click').on('click', function (e) {
+            try {
+                e.preventDefault();
+
+                filtrarEscalas();
+                carregarObservacoesDia();
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("ListaEscala.js", "btnFiltrar.click", error);
+            }
+        });
+
+        $('#btnLimpar, a[href*="ListaEscala"]').off('click').on('click', function (e) {
+            try {
+                if ($(this).hasClass('btn-secondary') || $(this).text().includes('Limpar')) {
+                    e.preventDefault();
+                    window.location.href = '/Escalas/ListaEscala';
+                }
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("ListaEscala.js", "btnLimpar.click", error);
+            }
+        });
+
+        var dataEscalaElement = document.getElementById('DataEscala');
+        if (dataEscalaElement && dataEscalaElement.ej2_instances && dataEscalaElement.ej2_instances[0]) {
+            dataEscalaElement.ej2_instances[0].change = function(args) {
                 try {
-                    e.preventDefault();
-                    var dataFiltro = obterValorComponente('DataEscala');
-                    var url = '/Escalas/FichaEscalas';
-                    if (dataFiltro) {
-                        var dataFormatada = new Date(dataFiltro)
-                            .toISOString()
-                            .split('T')[0];
-                        url += '?DataEscala=' + dataFormatada;
-                    }
-                    window.location.href = url;
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ListaEscala.js',
-                        'btnFichaEscala.click',
-                        error,
-                    );
-
-                    window.location.href = '/Escalas/FichaEscalas';
-                }
-            });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ListaEscala.js',
-            'document.ready',
-            error,
-        );
-    }
-});
-
-function configurarEventos() {
-    try {
-        $('#btnFiltrar')
-            .off('click')
-            .on('click', function (e) {
-                try {
-                    e.preventDefault();
-                    console.log('🔍 Aplicando filtros...');
-                    filtrarEscalas();
+
                     carregarObservacoesDia();
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ListaEscala.js',
-                        'btnFiltrar.click',
-                        error,
-                    );
-                }
-            });
-
-        $('#btnLimpar, a[href*="ListaEscala"]')
-            .off('click')
-            .on('click', function (e) {
-                try {
-                    if (
-                        $(this).hasClass('btn-secondary') ||
-                        $(this).text().includes('Limpar')
-                    ) {
-                        console.log('🧹 Limpando filtros...');
-                        e.preventDefault();
-                        window.location.href = '/Escalas/ListaEscala';
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ListaEscala.js',
-                        'btnLimpar.click',
-                        error,
-                    );
-                }
-            });
-
-        var dataEscalaElement = document.getElementById('DataEscala');
-        if (
-            dataEscalaElement &&
-            dataEscalaElement.ej2_instances &&
-            dataEscalaElement.ej2_instances[0]
-        ) {
-            dataEscalaElement.ej2_instances[0].change = function (args) {
-                try {
-                    console.log('📅 Data alterada:', args.value);
-                    carregarObservacoesDia();
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ListaEscala.js',
-                        'DataEscala.change',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("ListaEscala.js", "DataEscala.change", error);
                 }
             };
         }
 
-        console.log('✅ Eventos configurados');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ListaEscala.js',
-            'configurarEventos',
-            error,
-        );
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("ListaEscala.js", "configurarEventos", error);
     }
 }
 
 function configurarEventosGrid() {
     try {
+
         if (!gridEscalas) return;
 
         gridEscalas.toolbarClick = function (args) {
             try {
+
                 if (args.item.id === 'gridEscalas_excelexport') {
                     gridEscalas.excelExport();
                 }
@@ -324,28 +229,19 @@
                     gridEscalas.pdfExport();
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ListaEscala.js',
-                    'toolbarClick',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("ListaEscala.js", "toolbarClick", error);
             }
         };
 
-        console.log('✅ Eventos do grid configurados');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ListaEscala.js',
-            'configurarEventosGrid',
-            error,
-        );
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("ListaEscala.js", "configurarEventosGrid", error);
     }
 }
 
 function filtrarEscalas() {
     try {
+
         if (!gridEscalas) {
-            console.error('❌ Grid não inicializado');
             AppToast.show('Vermelho', 'Erro: Grid não inicializado', 2000);
             return;
         }
@@ -364,10 +260,8 @@
             veiculoId: obterValorComponente('veiculoId') || '',
             statusMotorista: obterValorComponente('statusMotorista') || '',
             lotacao: obterValorComponente('lotacao') || '',
-            textoPesquisa: obterValorComponente('textoPesquisa') || '',
+            textoPesquisa: obterValorComponente('textoPesquisa') || ''
         };
-
-        console.log('🔍 Filtros:', dados);
 
         $.ajax({
             url: '/api/Escala/GetEscalasFiltradas',
@@ -375,10 +269,9 @@
             data: dados,
             success: function (response) {
                 try {
-                    console.log('✅ Resposta recebida:', response);
-
                     if (response.success && response.data) {
-                        const dados = response.data.map((item) => {
+
+                        const resultados = response.data.map(item => {
                             return {
                                 EscalaDiaId: item.escalaDiaId,
                                 MotoristaId: item.motoristaId,
@@ -386,12 +279,8 @@
                                 TipoServicoId: item.tipoServicoId,
                                 TurnoId: item.turnoId,
                                 DataEscala: item.dataEscala,
-                                HoraInicio: item.horaInicio
-                                    ? item.horaInicio.substring(0, 5)
-                                    : '',
-                                HoraFim: item.horaFim
-                                    ? item.horaFim.substring(0, 5)
-                                    : '',
+                                HoraInicio: item.horaInicio ? item.horaInicio.substring(0, 5) : '',
+                                HoraFim: item.horaFim ? item.horaFim.substring(0, 5) : '',
                                 NomeMotorista: item.nomeMotorista,
                                 NomeServico: item.nomeServico,
                                 NomeTurno: item.nomeTurno,
@@ -400,48 +289,33 @@
                                 NumeroSaidas: item.numeroSaidas,
                                 StatusMotorista: item.statusMotorista,
                                 NomeRequisitante: item.nomeRequisitante,
-                                Observacoes: item.observacoes,
+                                Observacoes: item.observacoes
                             };
                         });
 
-                        gridEscalas.dataSource = dados;
-                        AppToast.show(
-                            'Verde',
-                            dados.length + ' escala(s) encontrada(s)',
-                            2000,
-                        );
+                        gridEscalas.dataSource = resultados;
+                        AppToast.show('Verde', resultados.length + ' escala(s) encontrada(s)', 2000);
                     } else {
-                        AppToast.show(
-                            'Amarelo',
-                            response.message || 'Nenhuma escala encontrada',
-                            3000,
-                        );
+
+                        AppToast.show('Amarelo', response.message || 'Nenhuma escala encontrada', 3000);
                         gridEscalas.dataSource = [];
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ListaEscala.js',
-                        'filtrarEscalas.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("ListaEscala.js", "filtrarEscalas.success", error);
                 }
             },
             error: function (xhr, status, error) {
-                console.error('❌ Erro AJAX:', error);
                 AppToast.show('Vermelho', 'Erro ao aplicar filtros', 3000);
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ListaEscala.js',
-            'filtrarEscalas',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("ListaEscala.js", "filtrarEscalas", error);
     }
 }
 
 function salvarObservacao() {
     try {
+
         const titulo = obterValorComponente('obsTitle');
         const descricao = obterValorComponente('obsDescription');
         const prioridade = obterValorComponente('obsPriority');
@@ -454,11 +328,7 @@
         }
 
         if (!exibirDe || !exibirAte) {
-            AppToast.show(
-                'Amarelo',
-                'Por favor, preencha as datas de exibição',
-                3000,
-            );
+            AppToast.show('Amarelo', 'Por favor, preencha as datas de exibição', 3000);
             return;
         }
 
@@ -471,20 +341,14 @@
                 descricao: descricao,
                 prioridade: prioridade || 'Normal',
                 exibirDe: new Date(exibirDe).toISOString(),
-                exibirAte: new Date(exibirAte).toISOString(),
+                exibirAte: new Date(exibirAte).toISOString()
             }),
             success: function (response) {
                 try {
                     if (response.success) {
-                        AppToast.show(
-                            'Verde',
-                            'Observação salva com sucesso!',
-                            3000,
-                        );
-
-                        const modal = bootstrap.Modal.getInstance(
-                            document.getElementById('modalObservacao'),
-                        );
+                        AppToast.show('Verde', 'Observação salva com sucesso!', 3000);
+
+                        const modal = bootstrap.Modal.getInstance(document.getElementById('modalObservacao'));
                         if (modal) modal.hide();
 
                         limparCampoComponente('obsTitle');
@@ -494,40 +358,26 @@
 
                         carregarObservacoesDia();
                     } else {
-                        AppToast.show(
-                            'Vermelho',
-                            response.message || 'Erro ao salvar',
-                            3000,
-                        );
+                        AppToast.show('Vermelho', response.message || 'Erro ao salvar', 3000);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ListaEscala.js',
-                        'salvarObservacao.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("ListaEscala.js", "salvarObservacao.success", error);
                 }
             },
             error: function (xhr, status, error) {
-                console.error('Erro AJAX:', error);
                 AppToast.show('Vermelho', 'Erro ao salvar observação', 3000);
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ListaEscala.js',
-            'salvarObservacao',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("ListaEscala.js", "salvarObservacao", error);
     }
 }
 
 function carregarObservacoesDia() {
     try {
+
         var dataFiltro = obterValorComponente('DataEscala');
-        var dataParam = dataFiltro
-            ? new Date(dataFiltro).toISOString()
-            : new Date().toISOString();
+        var dataParam = dataFiltro ? new Date(dataFiltro).toISOString() : new Date().toISOString();
 
         $.ajax({
             url: '/api/Escala/GetObservacoesDia',
@@ -535,53 +385,31 @@
             data: { data: dataParam },
             success: function (response) {
                 try {
-                    var container = document.getElementById(
-                        'observacoesContainer',
-                    );
+
+                    var container = document.getElementById('observacoesContainer');
                     if (!container) return;
 
-                    if (
-                        response.success &&
-                        response.data &&
-                        response.data.length > 0
-                    ) {
+                    if (response.success && response.data && response.data.length > 0) {
+
                         var html = '';
-                        response.data.forEach(function (obs) {
-                            var prioridadeClass =
-                                obs.prioridade === 'Alta'
-                                    ? 'danger'
-                                    : obs.prioridade === 'Normal'
-                                      ? 'warning'
-                                      : 'secondary';
+                        response.data.forEach(function(obs) {
+
+                            var prioridadeClass = obs.prioridade === 'Alta' ? 'danger' :
+                                                  obs.prioridade === 'Normal' ? 'warning' : 'secondary';
 
                             var dataDeFormatada = obs.exibirDe;
                             var dataAteFormatada = obs.exibirAte;
 
-                            html +=
-                                '<div class="alert alert-' +
-                                prioridadeClass +
-                                ' mb-2">';
-                            html +=
-                                '<div class="d-flex justify-content-between align-items-start">';
+                            html += '<div class="alert alert-' + prioridadeClass + ' mb-2">';
+                            html += '<div class="d-flex justify-content-between align-items-start">';
                             html += '<div>';
                             if (obs.titulo) {
-                                html +=
-                                    '<strong>' + obs.titulo + '</strong><br>';
+                                html += '<strong>' + obs.titulo + '</strong><br>';
                             }
                             html += obs.descricao;
-                            html +=
-                                '<br><small class="text-muted">Período: ' +
-                                dataDeFormatada +
-                                ' a ' +
-                                dataAteFormatada +
-                                ' | Prioridade: ' +
-                                obs.prioridade +
-                                '</small>';
+                            html += '<br><small class="text-muted">Período: ' + dataDeFormatada + ' a ' + dataAteFormatada + ' | Prioridade: ' + obs.prioridade + '</small>';
                             html += '</div>';
-                            html +=
-                                '<button type="button" class="btn btn-sm btn-outline-danger ms-2" onclick="excluirObservacaoDia(\'' +
-                                obs.observacaoId +
-                                '\')">';
+                            html += '<button type="button" class="btn btn-sm btn-outline-danger ms-2" onclick="excluirObservacaoDia(\'' + obs.observacaoId + '\')">';
                             html += '<i class="fas fa-trash"></i>';
                             html += '</button>';
                             html += '</div>';
@@ -589,32 +417,25 @@
                         });
                         container.innerHTML = html;
                     } else {
-                        container.innerHTML =
-                            '<p class="text-muted">Nenhuma observação para este dia</p>';
+
+                        container.innerHTML = '<p class="text-muted">Nenhuma observação para este dia</p>';
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ListaEscala.js',
-                        'carregarObservacoesDia.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("ListaEscala.js", "carregarObservacoesDia.success", error);
                 }
             },
             error: function (xhr, status, error) {
-                console.error('Erro ao carregar observações:', error);
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ListaEscala.js',
-            'carregarObservacoesDia',
-            error,
-        );
-    }
-}
-
-window.excluirObservacaoDia = function (observacaoId) {
-    try {
+
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("ListaEscala.js", "carregarObservacoesDia", error);
+    }
+}
+
+window.excluirObservacaoDia = function(observacaoId) {
+    try {
+
         if (!confirm('Deseja excluir esta observação?')) return;
 
         $.ajax({
@@ -626,39 +447,30 @@
                 try {
                     if (response.success) {
                         AppToast.show('Verde', 'Observação excluída!', 2000);
+
                         carregarObservacoesDia();
                     } else {
-                        AppToast.show(
-                            'Vermelho',
-                            response.message || 'Erro ao excluir',
-                            3000,
-                        );
+                        AppToast.show('Vermelho', response.message || 'Erro ao excluir', 3000);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ListaEscala.js',
-                        'excluirObservacaoDia.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("ListaEscala.js", "excluirObservacaoDia.success", error);
                 }
             },
             error: function () {
                 AppToast.show('Vermelho', 'Erro ao excluir observação', 3000);
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ListaEscala.js',
-            'excluirObservacaoDia',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("ListaEscala.js", "excluirObservacaoDia", error);
     }
 };
 
 function limparCampoComponente(elementId) {
     try {
+
         const element = document.getElementById(elementId);
         if (element && element.ej2_instances && element.ej2_instances[0]) {
+
             element.ej2_instances[0].value = null;
         }
     } catch (error) {
@@ -668,26 +480,23 @@
 
 function obterValorComponente(elementId) {
     try {
+
         const element = document.getElementById(elementId);
         if (element && element.ej2_instances && element.ej2_instances[0]) {
+
             return element.ej2_instances[0].value;
         }
         return null;
     } catch (error) {
-        console.warn(
-            `⚠️ Erro ao obter valor do componente ${elementId}:`,
-            error,
-        );
+        console.warn(`Erro ao obter valor do componente ${elementId}:`, error);
         return null;
     }
 }
 
 function preencherModalVisualizacao(dados) {
     try {
-        console.log('📝 Preenchendo modal com:', dados);
-
-        document.getElementById('viewNomeMotorista').textContent =
-            dados.nomeMotorista || '-';
+
+        document.getElementById('viewNomeMotorista').textContent = dados.nomeMotorista || '-';
 
         var fotoElement = document.getElementById('viewFotoMotorista');
         if (dados.fotoBase64 && dados.fotoBase64 !== '') {
@@ -699,8 +508,7 @@
         var statusBadge = document.getElementById('viewStatusBadge');
         var statusTexto = dados.statusMotorista || 'Indefinido';
         statusBadge.textContent = statusTexto;
-        statusBadge.className =
-            'badge mt-2 status-' + getStatusClass(statusTexto);
+        statusBadge.className = 'badge mt-2 status-' + getStatusClass(statusTexto);
 
         var dataFormatada = '-';
         if (dados.dataEscala) {
@@ -720,61 +528,40 @@
         }
         document.getElementById('viewHorario').textContent = horarioTexto;
 
-        document.getElementById('viewTurno').textContent =
-            dados.nomeTurno || '-';
-        document.getElementById('viewPlaca').textContent =
-            dados.placa || 'Sem veículo';
-        document.getElementById('viewLotacao').textContent =
-            dados.lotacao || '-';
-        document.getElementById('viewNumeroSaidas').textContent =
-            dados.numeroSaidas || '0';
-        document.getElementById('viewTipoServico').textContent =
-            dados.nomeServico || '-';
-        document.getElementById('viewRequisitante').textContent =
-            dados.nomeRequisitante || '-';
-        document.getElementById('viewObservacoes').textContent =
-            dados.observacoes || 'Nenhuma observação registrada';
+        document.getElementById('viewTurno').textContent = dados.nomeTurno || '-';
+        document.getElementById('viewPlaca').textContent = dados.placa || 'Sem veículo';
+        document.getElementById('viewLotacao').textContent = dados.lotacao || '-';
+        document.getElementById('viewNumeroSaidas').textContent = dados.numeroSaidas || '0';
+        document.getElementById('viewTipoServico').textContent = dados.nomeServico || '-';
+        document.getElementById('viewRequisitante').textContent = dados.nomeRequisitante || '-';
+        document.getElementById('viewObservacoes').textContent = dados.observacoes || 'Nenhuma observação registrada';
 
         var divCobertura = document.getElementById('divCoberturaInfo');
         var divCobrindo = document.getElementById('divCobrindoInfo');
-        var statusLower = (statusTexto || '')
-            .toLowerCase()
-            .normalize('NFD')
-            .replace(/[\u0300-\u036f]/g, '');
+
+        var statusLower = (statusTexto || '').toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');
 
         if (statusLower.includes('indisponivel')) {
             divCobertura.style.display = 'block';
-            document.getElementById('viewNomeCobertor').textContent =
-                dados.nomeMotoristaCobertor ||
-                dados.nomeCobertor ||
-                'Não definido';
-            document.getElementById('viewMotivoCobertura').textContent =
-                dados.motivoCobertura || 'Não informado';
+            document.getElementById('viewNomeCobertor').textContent = dados.nomeMotoristaCobertor || dados.nomeCobertor || 'Não definido';
+            document.getElementById('viewMotivoCobertura').textContent = dados.motivoCobertura || 'Não informado';
         } else {
             divCobertura.style.display = 'none';
         }
 
         if (dados.nomeMotoristaCobrindo && dados.nomeMotoristaCobrindo !== '') {
             divCobrindo.style.display = 'block';
-            document.getElementById('viewNomeCoberto').textContent =
-                dados.nomeMotoristaCobrindo;
-            document.getElementById('viewMotivoCoberto').textContent =
-                dados.motivoCoberturaCobrindo || 'Não informado';
+            document.getElementById('viewNomeCoberto').textContent = dados.nomeMotoristaCobrindo;
+            document.getElementById('viewMotivoCoberto').textContent = dados.motivoCoberturaCobrindo || 'Não informado';
         } else {
             divCobrindo.style.display = 'none';
         }
 
-        document.getElementById('btnEditarEscala').href =
-            '/Escalas/UpsertEEscala?id=' + dados.escalaDiaId;
-
-        console.log('✅ Modal preenchido com sucesso');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ListaEscala.js',
-            'preencherModalVisualizacao',
-            error,
-        );
-    }
-}
-
-console.log('📄 ListaEscala.js carregado');
+        document.getElementById('btnEditarEscala').href = '/Escalas/UpsertEEscala?id=' + dados.escalaDiaId;
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('ListaEscala.js', 'preencherModalVisualizacao', error);
+    }
+}
+
+console.log('✅ ListaEscala.js carregado');
```
