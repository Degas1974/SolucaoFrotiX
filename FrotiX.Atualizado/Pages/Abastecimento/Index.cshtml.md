# Pages/Abastecimento/Index.cshtml

**Mudanca:** GRANDE | **+186** linhas | **-330** linhas

---

```diff
--- JANEIRO: Pages/Abastecimento/Index.cshtml
+++ ATUAL: Pages/Abastecimento/Index.cshtml
@@ -263,7 +263,7 @@
                                         <th>Kms</th>
                                         <th>Consumo</th>
                                         <th>Média</th>
-                                        <th>Ações</th>
+                                        <th>Ação</th>
                                     </tr>
                                 </thead>
                                 <tbody>
@@ -325,38 +325,12 @@
 
 @section ScriptsBlock {
     <script asp-append-version="true">
-                            /**
-                             * ═══════════════════════════════════════════════════════════════════════════
-                             * VARIÁVEIS DE ESTADO - FILTROS DE ABASTECIMENTO
-                             * ═══════════════════════════════════════════════════════════════════════════
-                             * @@description Flags que indicam qual filtro está sendo utilizado
-            * Apenas um filtro pode estar ativo por vez
-                */
-
-                            /** @@type { boolean } Indica se o usuário está filtrando por veículo */
         var escolhendoVeiculo = false;
-                            /** @@type { boolean } Indica se o usuário está filtrando por unidade */
         var escolhendoUnidade = false;
-                            /** @@type { boolean } Indica se o usuário está filtrando por motorista */
         var escolhendoMotorista = false;
-                            /** @@type { boolean } Indica se o usuário está filtrando por combustível */
         var escolhendoCombustivel = false;
-                            /** @@type { boolean } Indica se o usuário está filtrando por data */
         var escolhendoData = false;
 
-                            /**
-                             * ═══════════════════════════════════════════════════════════════════════════
-                             * HANDLERS DE SELEÇÃO DE FILTROS
-                             * ═══════════════════════════════════════════════════════════════════════════
-                             * @@description Funções chamadas ao fechar / selecionar cada dropdown de filtro
-            * Resetam os outros filtros e recarregam a tabela se valor for nulo
-                */
-
-                            /**
-                             * Define filtro ativo como Veículo
-                             * @@description Chamado ao fechar o dropdown de veículos
-            * @@returns { void}
-            */
         function DefineEscolhaVeiculo() {
             try {
                 console.log("Fechou Veículo");
@@ -375,11 +349,6 @@
             }
         }
 
-                            /**
-                             * Define filtro ativo como Unidade
-                             * @@description Chamado ao fechar o dropdown de unidades
-            * @@returns { void}
-            */
         function DefineEscolhaUnidade() {
             try {
                 console.log("Fechou Unidade");
@@ -398,11 +367,6 @@
             }
         }
 
-                            /**
-                             * Define filtro ativo como Motorista
-                             * @@description Chamado ao fechar o dropdown de motoristas
-            * @@returns { void}
-            */
         function DefineEscolhaMotorista() {
             try {
                 console.log("Fechou Motorista");
@@ -421,11 +385,6 @@
             }
         }
 
-                            /**
-                             * Define filtro ativo como Combustível
-                             * @@description Chamado ao fechar o dropdown de combustíveis
-            * @@returns { void}
-            */
         function DefineEscolhaCombustivel() {
             try {
                 console.log("Fechou Combustível");
@@ -444,11 +403,6 @@
             }
         }
 
-                            /**
-                             * Define filtro ativo como Data
-                             * @@description Chamado ao selecionar uma data no campo de filtro
-            * @@returns { void}
-            */
         function DefineEscolhaData() {
             try {
                 console.log("Escolheu Data");
@@ -462,19 +416,6 @@
             }
         }
 
-                            /**
-                             * ═══════════════════════════════════════════════════════════════════════════
-                             * DATATABLE - LISTAGEM PRINCIPAL DE ABASTECIMENTOS
-                             * ═══════════════════════════════════════════════════════════════════════════
-                             * @@description Carrega e renderiza a tabela com todos os abastecimentos
-            * Inclui configurações de exportação, paginação e tradução PT - BR
-                */
-
-                            /**
-                             * Lista todos os abastecimentos sem filtro
-                             * @@description Reseta flags de filtro e recarrega DataTable via API
-            * @@returns { void}
-            */
         function ListaTodosAbastecimentos() {
             try {
                 console.log("Lista Todos");
@@ -546,13 +487,13 @@
                             "data": "abastecimentoId",
                             "render": function (data) {
                                 return `<div class="text-center">
-                                                                    <a class="btn text-white btn-acao-km"
-                                                                       data-bs-toggle="modal" data-bs-target="#modalEditaKm"
-                                                                       style="cursor:pointer; background-color:#3D5771; border-color:#2d4559; width:28px; height:28px; padding:0; display:inline-flex; align-items:center; justify-content:center; border-radius:.35rem;"
-                                                                       data-id='${data}'>
-                                                                        <i class="fad fa-pen-to-square"></i>
-                                                                    </a>
-                                                                </div>`;
+                                                <a class="btn text-white btn-acao-km"
+                                                   data-bs-toggle="modal" data-bs-target="#modalEditaKm"
+                                                   style="cursor:pointer; background-color:#3D5771; border-color:#2d4559; width:28px; height:28px; padding:0; display:inline-flex; align-items:center; justify-content:center; border-radius:.35rem;"
+                                                   data-id='${data}'>
+                                                    <i class="fad fa-pen-to-square"></i>
+                                                </a>
+                                            </div>`;
                             }
                         }
                     ],
@@ -714,12 +655,6 @@
             }
         }
 
-                        /**
-                         * Confirma exclusão de abastecimento via SweetAlert
-                         * @@description Exibe modal de confirmação e envia requisição DELETE
-            * @@async
-                         * @@returns { Promise<void>}
-            */
         async function confirmarExclusao() {
             try {
                 const confirmado = await SweetAlertInterop.ShowConfirm(
@@ -748,18 +683,6 @@
             }
         }
 
-                        /**
-                         * ═══════════════════════════════════════════════════════════════════════════
-                         * FUNÇÕES UTILITÁRIAS - DATATABLE
-                         * ═══════════════════════════════════════════════════════════════════════════
-                         * @@description Helpers reutilizáveis para configuração e manipulação do DataTable
-            */
-
-                        /**
-                         * Destrói o DataTable de forma segura
-                         * @@description Verifica existência antes de destruir, limpa tbody
-            * @@returns { void}
-            */
         function dtDestroySafe() {
             try {
                 if ($.fn.DataTable.isDataTable('#tblAbastecimentos')) {
@@ -771,215 +694,193 @@
             }
         }
 
-                        /**
-                         * Retorna objeto de configuração padrão do DataTable
-                         * @@description Configurações comuns: dom, paginação, colunas, tradução PT - BR
-            * @@returns { Object } Objeto de configuração do DataTable
-                */
-                        function dtCommonOptions() {
-                try {
-                    return {
-                        dom: 'Bfrtip',
-                        lengthMenu: [
-                            [10, 25, 50, -1],
-                            ['10 linhas', '25 linhas', '50 linhas', 'Todas as Linhas']
-                        ],
-                        buttons: ['pageLength', 'excel', {
-                            extend: 'pdfHtml5',
-                            orientation: 'landscape',
-                            pageSize: 'LEGAL'
-                        }],
-                        "aaSorting": [],
-                        'columnDefs': [
-                            { "targets": 0, "className": "text-center", "width": "2%" },
-                            { "targets": 1, "className": "text-center", "width": "2%" },
-                            { "targets": 2, "className": "text-center", "width": "2%" },
-                            { "targets": 3, "className": "text-left", "width": "4%" },
-                            { "targets": 4, "className": "text-left", "width": "8%" },
-                            { "targets": 5, "className": "text-center", "width": "3%" },
-                            { "targets": 6, "className": "text-left", "width": "5%" },
-                            { "targets": 7, "className": "text-right", "width": "2%" },
-                            { "targets": 8, "className": "text-right", "width": "2%" },
-                            { "targets": 9, "className": "text-right", "width": "2%" },
-                            { "targets": 10, "className": "text-right", "width": "2%" },
-                            { "targets": 11, "className": "text-center", "width": "2%" },
-                            { "targets": 12, "className": "text-center", "width": "2%" },
-                            { "targets": 13, "className": "text-center", "width": "2%" }
-                        ],
-                        responsive: true,
-                        "language": {
-                            "emptyTable": "Nenhum registro encontrado",
-                            "info": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
-                            "infoEmpty": "Mostrando 0 até 0 de 0 registros",
-                            "infoFiltered": "(Filtrados de _MAX_ registros)",
-                            "infoThousands": ".",
-                            "loadingRecords": "Carregando...",
-                            "processing": "Processando...",
-                            "zeroRecords": "Nenhum registro encontrado",
-                            "search": "Pesquisar",
-                            "paginate": {
-                                "next": "Próximo",
-                                "previous": "Anterior",
-                                "first": "Primeiro",
-                                "last": "Último"
-                            },
-                            "aria": {
-                                "sortAscending": ": Ordenar colunas de forma ascendente",
-                                "sortDescending": ": Ordenar colunas de forma descendente"
-                            },
-                            "select": {
-                                "rows": { "_": "Selecionado %d linhas", "1": "Selecionado 1 linha" },
-                                "cells": { "1": "1 célula selecionada", "_": "%d células selecionadas" },
-                                "columns": { "1": "1 coluna selecionada", "_": "%d colunas selecionadas" }
-                            },
-                            "buttons": {
-                                "copySuccess": { "1": "Uma linha copiada com sucesso", "_": "%d linhas copiadas com sucesso" },
-                                "collection": "Coleção <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"></span>",
-                                "colvis": "Visibilidade da Coluna",
-                                "colvisRestore": "Restaurar Visibilidade",
-                                "copy": "Copiar",
-                                "copyKeys": "Pressione ctrl ou ⌘ + C para copiar os dados da tabela.",
-                                "copyTitle": "Copiar para a Área de Transferência",
-                                "csv": "CSV",
-                                "excel": "Excel",
-                                "pageLength": { "-1": "Mostrar todos os registros", "_": "Mostrar %d registros" },
-                                "pdf": "PDF",
-                                "print": "Imprimir"
-                            },
-                            "autoFill": {
-                                "cancel": "Cancelar",
-                                "fill": "Preencher todas as células com",
-                                "fillHorizontal": "Preencher células horizontalmente",
-                                "fillVertical": "Preencher células verticalmente"
-                            },
-                            "lengthMenu": "Exibir _MENU_ resultados por página",
-                            "searchBuilder": {
-                                "add": "Adicionar Condição",
-                                "button": { "0": "Construtor de Pesquisa", "_": "Construtor de Pesquisa (%d)" },
-                                "clearAll": "Limpar Tudo",
-                                "condition": "Condição",
-                                "conditions": {
-                                    "date": {
-                                        "after": "Depois", "before": "Antes", "between": "Entre",
-                                        "empty": "Vazio", "equals": "Igual", "not": "Não",
-                                        "notBetween": "Não Entre", "notEmpty": "Não Vazio"
-                                    },
-                                    "number": {
-                                        "between": "Entre", "empty": "Vazio", "equals": "Igual",
-                                        "gt": "Maior Que", "gte": "Maior ou Igual a",
-                                        "lt": "Menor Que", "lte": "Menor ou Igual a",
-                                        "not": "Não", "notBetween": "Não Entre", "notEmpty": "Não Vazio"
-                                    },
-                                    "string": {
-                                        "contains": "Contém", "empty": "Vazio", "endsWith": "Termina Com",
-                                        "equals": "Igual", "not": "Não", "notEmpty": "Não Vazio",
-                                        "startsWith": "Começa Com"
-                                    },
-                                    "array": {
-                                        "contains": "Contém", "empty": "Vazio", "equals": "Igual à ",
-                                        "not": "Não", "notEmpty": "Não vazio", "without": "Não possui"
-                                    }
+        function dtCommonOptions() {
+            try {
+                return {
+                    dom: 'Bfrtip',
+                    lengthMenu: [
+                        [10, 25, 50, -1],
+                        ['10 linhas', '25 linhas', '50 linhas', 'Todas as Linhas']
+                    ],
+                    buttons: ['pageLength', 'excel', {
+                        extend: 'pdfHtml5',
+                        orientation: 'landscape',
+                        pageSize: 'LEGAL'
+                    }],
+                    "aaSorting": [],
+                    'columnDefs': [
+                        { "targets": 0, "className": "text-center", "width": "2%" },
+                        { "targets": 1, "className": "text-center", "width": "2%" },
+                        { "targets": 2, "className": "text-center", "width": "2%" },
+                        { "targets": 3, "className": "text-left", "width": "4%" },
+                        { "targets": 4, "className": "text-left", "width": "8%" },
+                        { "targets": 5, "className": "text-center", "width": "3%" },
+                        { "targets": 6, "className": "text-left", "width": "5%" },
+                        { "targets": 7, "className": "text-right", "width": "2%" },
+                        { "targets": 8, "className": "text-right", "width": "2%" },
+                        { "targets": 9, "className": "text-right", "width": "2%" },
+                        { "targets": 10, "className": "text-right", "width": "2%" },
+                        { "targets": 11, "className": "text-center", "width": "2%" },
+                        { "targets": 12, "className": "text-center", "width": "2%" },
+                        { "targets": 13, "className": "text-center", "width": "2%" }
+                    ],
+                    responsive: true,
+                    "language": {
+                        "emptyTable": "Nenhum registro encontrado",
+                        "info": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
+                        "infoEmpty": "Mostrando 0 até 0 de 0 registros",
+                        "infoFiltered": "(Filtrados de _MAX_ registros)",
+                        "infoThousands": ".",
+                        "loadingRecords": "Carregando...",
+                        "processing": "Processando...",
+                        "zeroRecords": "Nenhum registro encontrado",
+                        "search": "Pesquisar",
+                        "paginate": {
+                            "next": "Próximo",
+                            "previous": "Anterior",
+                            "first": "Primeiro",
+                            "last": "Último"
+                        },
+                        "aria": {
+                            "sortAscending": ": Ordenar colunas de forma ascendente",
+                            "sortDescending": ": Ordenar colunas de forma descendente"
+                        },
+                        "select": {
+                            "rows": { "_": "Selecionado %d linhas", "1": "Selecionado 1 linha" },
+                            "cells": { "1": "1 célula selecionada", "_": "%d células selecionadas" },
+                            "columns": { "1": "1 coluna selecionada", "_": "%d colunas selecionadas" }
+                        },
+                        "buttons": {
+                            "copySuccess": { "1": "Uma linha copiada com sucesso", "_": "%d linhas copiadas com sucesso" },
+                            "collection": "Coleção <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"></span>",
+                            "colvis": "Visibilidade da Coluna",
+                            "colvisRestore": "Restaurar Visibilidade",
+                            "copy": "Copiar",
+                            "copyKeys": "Pressione ctrl ou ⌘ + C para copiar os dados da tabela.",
+                            "copyTitle": "Copiar para a Área de Transferência",
+                            "csv": "CSV",
+                            "excel": "Excel",
+                            "pageLength": { "-1": "Mostrar todos os registros", "_": "Mostrar %d registros" },
+                            "pdf": "PDF",
+                            "print": "Imprimir"
+                        },
+                        "autoFill": {
+                            "cancel": "Cancelar",
+                            "fill": "Preencher todas as células com",
+                            "fillHorizontal": "Preencher células horizontalmente",
+                            "fillVertical": "Preencher células verticalmente"
+                        },
+                        "lengthMenu": "Exibir _MENU_ resultados por página",
+                        "searchBuilder": {
+                            "add": "Adicionar Condição",
+                            "button": { "0": "Construtor de Pesquisa", "_": "Construtor de Pesquisa (%d)" },
+                            "clearAll": "Limpar Tudo",
+                            "condition": "Condição",
+                            "conditions": {
+                                "date": {
+                                    "after": "Depois", "before": "Antes", "between": "Entre",
+                                    "empty": "Vazio", "equals": "Igual", "not": "Não",
+                                    "notBetween": "Não Entre", "notEmpty": "Não Vazio"
                                 },
-                                "data": "Data",
-                                "deleteTitle": "Excluir regra de filtragem",
-                                "logicAnd": "E",
-                                "logicOr": "Ou",
-                                "title": { "0": "Construtor de Pesquisa", "_": "Construtor de Pesquisa (%d)" },
-                                "value": "Valor",
-                                "leftTitle": "Critérios Externos",
-                                "rightTitle": "Critérios Internos"
-                            },
-                            "searchPanes": {
-                                "clearMessage": "Limpar Tudo",
-                                "collapse": { "0": "Painéis de Pesquisa", "_": "Painéis de Pesquisa (%d)" },
-                                "count": "{total}",
-                                "countFiltered": "{shown} ({total})",
-                                "emptyPanes": "Nenhum Painel de Pesquisa",
-                                "loadMessage": "Carregando Painéis de Pesquisa...",
-                                "title": "Filtros Ativos"
-                            },
-                            "thousands": ".",
-                            "datetime": {
-                                "previous": "Anterior", "next": "Próximo",
-                                "hours": "Hora", "minutes": "Minuto", "seconds": "Segundo",
-                                "amPm": ["am", "pm"], "unknown": "-",
-                                "months": {
-                                    "0": "Janeiro", "1": "Fevereiro", "2": "Março", "3": "Abril",
-                                    "4": "Maio", "5": "Junho", "6": "Julho", "7": "Agosto",
-                                    "8": "Setembro", "9": "Outubro", "10": "Novembro", "11": "Dezembro"
+                                "number": {
+                                    "between": "Entre", "empty": "Vazio", "equals": "Igual",
+                                    "gt": "Maior Que", "gte": "Maior ou Igual a",
+                                    "lt": "Menor Que", "lte": "Menor ou Igual a",
+                                    "not": "Não", "notBetween": "Não Entre", "notEmpty": "Não Vazio"
                                 },
-                                "weekdays": [
-                                    "Domingo", "Segunda-feira", "Terça-feira",
-                                    "Quarta-feira", "Quinte-feira", "Sexta-feira", "Sábado"
-                                ]
-                            },
-                            "editor": {
-                                "close": "Fechar",
-                                "create": { "button": "Novo", "submit": "Criar", "title": "Criar novo registro" },
-                                "edit": { "button": "Editar", "submit": "Atualizar", "title": "Editar registro" },
-                                "error": {
-                                    "system": "Ocorreu um erro no sistema (<a target=\"_blank\" rel=\"nofollow\" href=\"#\">Mais informações</a>)."
+                                "string": {
+                                    "contains": "Contém", "empty": "Vazio", "endsWith": "Termina Com",
+                                    "equals": "Igual", "not": "Não", "notEmpty": "Não Vazio",
+                                    "startsWith": "Começa Com"
                                 },
-                                "multi": {
-                                    "noMulti": "Essa entrada pode ser editada individualmente, mas não como parte do grupo",
-                                    "restore": "Desfazer alterações",
-                                    "title": "Multiplos valores",
-                                    "info": "Os itens selecionados contêm valores diferentes."
-                                },
-                                "remove": {
-                                    "button": "Remover",
-                                    "confirm": { "_": "Tem certeza que quer deletar %d linhas?", "1": "Tem certeza que quer deletar 1 linha?" },
-                                    "submit": "Remover",
-                                    "title": "Remover registro"
+                                "array": {
+                                    "contains": "Contém", "empty": "Vazio", "equals": "Igual à ",
+                                    "not": "Não", "notEmpty": "Não vazio", "without": "Não possui"
                                 }
                             },
-                            "decimal": ","
-                        },
-                        "width": "100%"
-                    };
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Index.cshtml", "dtCommonOptions", error);
-                    return {};
-                }
-            }
-
-                        /**
-                         * Renderiza HTML do botão de ação para edição de KM
-                         * @@description Retorna markup do botão que abre modal de edição
-            * @@param { string } abastecimentoId - GUID do abastecimento
-                * @@returns { string } HTML do botão de ação
-                    */
+                            "data": "Data",
+                            "deleteTitle": "Excluir regra de filtragem",
+                            "logicAnd": "E",
+                            "logicOr": "Ou",
+                            "title": { "0": "Construtor de Pesquisa", "_": "Construtor de Pesquisa (%d)" },
+                            "value": "Valor",
+                            "leftTitle": "Critérios Externos",
+                            "rightTitle": "Critérios Internos"
+                        },
+                        "searchPanes": {
+                            "clearMessage": "Limpar Tudo",
+                            "collapse": { "0": "Painéis de Pesquisa", "_": "Painéis de Pesquisa (%d)" },
+                            "count": "{total}",
+                            "countFiltered": "{shown} ({total})",
+                            "emptyPanes": "Nenhum Painel de Pesquisa",
+                            "loadMessage": "Carregando Painéis de Pesquisa...",
+                            "title": "Filtros Ativos"
+                        },
+                        "thousands": ".",
+                        "datetime": {
+                            "previous": "Anterior", "next": "Próximo",
+                            "hours": "Hora", "minutes": "Minuto", "seconds": "Segundo",
+                            "amPm": ["am", "pm"], "unknown": "-",
+                            "months": {
+                                "0": "Janeiro", "1": "Fevereiro", "2": "Março", "3": "Abril",
+                                "4": "Maio", "5": "Junho", "6": "Julho", "7": "Agosto",
+                                "8": "Setembro", "9": "Outubro", "10": "Novembro", "11": "Dezembro"
+                            },
+                            "weekdays": [
+                                "Domingo", "Segunda-feira", "Terça-feira",
+                                "Quarta-feira", "Quinte-feira", "Sexta-feira", "Sábado"
+                            ]
+                        },
+                        "editor": {
+                            "close": "Fechar",
+                            "create": { "button": "Novo", "submit": "Criar", "title": "Criar novo registro" },
+                            "edit": { "button": "Editar", "submit": "Atualizar", "title": "Editar registro" },
+                            "error": {
+                                "system": "Ocorreu um erro no sistema (<a target=\"_blank\" rel=\"nofollow\" href=\"#\">Mais informações</a>)."
+                            },
+                            "multi": {
+                                "noMulti": "Essa entrada pode ser editada individualmente, mas não como parte do grupo",
+                                "restore": "Desfazer alterações",
+                                "title": "Multiplos valores",
+                                "info": "Os itens selecionados contêm valores diferentes."
+                            },
+                            "remove": {
+                                "button": "Remover",
+                                "confirm": { "_": "Tem certeza que quer deletar %d linhas?", "1": "Tem certeza que quer deletar 1 linha?" },
+                                "submit": "Remover",
+                                "title": "Remover registro"
+                            }
+                        },
+                        "decimal": ","
+                    },
+                    "width": "100%"
+                };
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "dtCommonOptions", error);
+                return {};
+            }
+        }
+
         function renderBotaoAcao(abastecimentoId) {
             try {
                 return `<div class="text-center">
-                                                    <a class="btn text-white btn-acao-km"
-                                                       data-bs-toggle="modal" data-bs-target="#modalEditaKm"
-                                                       style="cursor:pointer; background-color:#3D5771; border-color:#2d4559; width:28px; height:28px; padding:0; display:inline-flex; align-items:center; justify-content:center; border-radius:.35rem;"
-                                                       data-id='${abastecimentoId}'>
-                                                        <i class="fad fa-pen-to-square"></i>
-                                                    </a>
-                                                </div>`;
+                                <a class="btn text-white btn-acao-km"
+                                   data-bs-toggle="modal" data-bs-target="#modalEditaKm"
+                                   style="cursor:pointer; background-color:#3D5771; border-color:#2d4559; width:28px; height:28px; padding:0; display:inline-flex; align-items:center; justify-content:center; border-radius:.35rem;"
+                                   data-id='${abastecimentoId}'>
+                                    <i class="fad fa-pen-to-square"></i>
+                                </a>
+                            </div>`;
             } catch (error) {
                 Alerta.TratamentoErroComLinha("Index.cshtml", "renderBotaoAcao", error);
                 return '';
             }
         }
 
-                        /**
-                         * ═══════════════════════════════════════════════════════════════════════════
-                         * INICIALIZAÇÃO E HANDLERS DE EVENTOS
-                         * ═══════════════════════════════════════════════════════════════════════════
-                         * @@description Configura eventos de filtro e carrega dados iniciais
-            */
         $(document).ready(function () {
             try {
-
                 ListaTodosAbastecimentos();
 
-                                /**
-                                 * Handler de mudança no campo de data
-                                 * @@description Filtra abastecimentos pela data selecionada
-                    */
                 $("#txtData").change(function () {
                     try {
                         DefineEscolhaData();
@@ -1043,18 +944,6 @@
             }
         });
 
-                    /**
-                     * ═══════════════════════════════════════════════════════════════════════════
-                     * MODAL DE EDIÇÃO DE KM
-                     * ═══════════════════════════════════════════════════════════════════════════
-                     * @@description Handlers para modal de edição de quilometragem do abastecimento
-            */
-
-                    /**
-                     * Handler de abertura do modal de edição de KM
-                     * @@description Busca dados do abastecimento no DataTable e preenche o modal
-            * @@param { Event } event - Evento Bootstrap Modal shown
-                */
         $('#modalEditaKm').on('shown.bs.modal', function (event) {
             try {
                 var button = $(event.relatedTarget);
@@ -1080,38 +969,20 @@
             } catch (error) {
                 Alerta.TratamentoErroComLinha("Index.cshtml", "shown.bs.modal", error);
             }
-                    /**
-                     * Handler de fechamento do modal
-                     * @@description Limpa campos e reseta estado do botão
-                */
-                    }).on("hide.bs.modal", function () {
-                    try {
-                        $('#txtKm').attr('value', '');
-
-                        $("#btnEditaKm").prop('disabled', false);
-                        $("#btnEditaKm .btn-text").removeClass('d-none');
-                        $("#btnEditaKm .btn-loading").addClass('d-none');
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha("Index.cshtml", "hide.bs.modal", error);
-                    }
-                });
-
-                    /**
-                     * ═══════════════════════════════════════════════════════════════════════════
-                     * HANDLERS DE FILTROS - COMBOBOX SYNCFUSION
-                     * ═══════════════════════════════════════════════════════════════════════════
-                     * @@description Funções chamadas quando o valor de cada combobox muda
-            * Reconstrói o DataTable com o filtro selecionado
-                */
-
-                    /**
-                     * Handler de mudança no ComboBox de Veículos
-                     * @@description Filtra abastecimentos pelo veículo selecionado
-            * @@returns { void}
-            */
+        }).on("hide.bs.modal", function () {
+            try {
+                $('#txtKm').attr('value', '');
+
+                $("#btnEditaKm").prop('disabled', false);
+                $("#btnEditaKm .btn-text").removeClass('d-none');
+                $("#btnEditaKm .btn-loading").addClass('d-none');
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Index.cshtml", "hide.bs.modal", error);
+            }
+        });
+
         function VeiculosValueChange() {
             try {
-
                 if (escolhendoVeiculo === false) {
                     return;
                 }
@@ -1169,14 +1040,8 @@
             }
         }
 
-                    /**
-                     * Handler de mudança no ComboBox de Combustível
-                     * @@description Filtra abastecimentos pelo tipo de combustível selecionado
-            * @@returns { void}
-            */
         function CombustivelValueChange() {
             try {
-
                 if (escolhendoCombustivel === false) {
                     return;
                 }
@@ -1234,11 +1099,6 @@
             }
         }
 
-                /**
-                 * Handler de mudança no ComboBox de Unidade
-                 * @@description Filtra abastecimentos pela unidade administrativa selecionada
-            * @@returns { void}
-            */
         function UnidadeValueChange() {
             try {
                 console.log("UnidadeValueChange");
@@ -1299,11 +1159,6 @@
             }
         }
 
-                /**
-                 * Handler de mudança no ComboBox de Motorista
-                 * @@description Filtra abastecimentos pelo motorista selecionado
-            * @@returns { void}
-            */
         function MotoristaValueChange() {
             try {
                 console.log("MotoristaValueChange");
@@ -1364,18 +1219,10 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * BOTÃO EDITAR KM - ATUALIZAÇÃO DE QUILOMETRAGEM
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Handler do botão de salvar no modal de edição de KM
-            * Inclui loading spinner durante requisição AJAX
-                */
         $("#btnEditaKm").click(function (e) {
             try {
                 e.preventDefault();
 
-                    /** @@type { string } Quilometragem informada pelo usuário */
                 const KmRodado = $("#txtKm").val();
 
                 if (KmRodado === '') {
@@ -1407,7 +1254,6 @@
                     data: objViagem,
                     success: function (data) {
                         try {
-
                             AppToast.show('Verde', data.message);
                             $('#tblAbastecimentos').DataTable().ajax.reload(null, false);
                             $('#modalEditaKm').modal('hide');
@@ -1417,7 +1263,6 @@
                     },
                     error: function (data) {
                         try {
-
                             AppToast.show('Vermelho', 'Erro ao salvar a quilometragem.');
                             console.log(data);
                         } catch (error) {
```
