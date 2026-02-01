/* ****************************************************************************************
 * ⚡ ARQUIVO: localization-init.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Inicializa e configura localização pt-BR para todos os componentes
 *                   Syncfusion EJ2. Carrega arquivos CLDR (Unicode Common Locale Data)
 *                   e define strings de interface em português brasileiro.
 * 📥 ENTRADAS     : Arquivos CLDR JSON (/cldr/*.json), cultura pt-BR hardcoded
 * 📤 SAÍDAS       : ej.base configurado com cultura pt-BR, strings L10n carregadas
 * 🔗 CHAMADA POR  : Layouts principais (_Layout.cshtml), páginas que usam Syncfusion
 * 🔄 CHAMA        : fetch API, ej.base.loadCldr, ej.base.setCulture, ej.base.L10n.load
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 (ej.base), arquivos CLDR em /cldr/, fetch API
 * 📝 OBSERVAÇÕES  : Função assíncrona exposta em window.loadSyncfusionLocalization,
 *                   deve ser chamada antes da inicialização de componentes Syncfusion
 *
 * 📋 ÍNDICE DE COMPONENTES LOCALIZADOS (13 componentes):
 *
 * ┌─ CLDR DATA (6 arquivos JSON) ──────────────────────────────────────────┐
 * │ 1. /cldr/ca-gregorian.json    - Calendário gregoriano pt-BR           │
 * │ 2. /cldr/numbers.json         - Formatação de números                 │
 * │ 3. /cldr/timeZoneNames.json   - Nomes de fusos horários               │
 * │ 4. /cldr/currencies.json      - Símbolos e nomes de moedas            │
 * │ 5. /cldr/numberingSystems.json - Sistemas de numeração               │
 * │ 6. /cldr/weekData.json        - Dados de semana (primeiro dia, etc.) │
 * └─────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ COMPONENTES SYNCFUSION LOCALIZADOS ───────────────────────────────────┐
 * │ 1. calendar                                                            │
 * │    → Botão "Hoje", nomes de dias/meses, firstDayOfWeek=0 (domingo)   │
 * │                                                                         │
 * │ 2. datepicker                                                          │
 * │    → Placeholder "Selecione uma data", botões Hoje/Fechar            │
 * │                                                                         │
 * │ 3. timepicker                                                          │
 * │    → Placeholder "Selecione um horário", botões Agora/OK/Cancelar    │
 * │                                                                         │
 * │ 4. datetimepicker                                                      │
 * │    → Placeholder "Selecione data e horário", botões Hoje/Agora/OK    │
 * │                                                                         │
 * │ 5. grid                                                                │
 * │    → Mensagens de ações (Adicionar, Editar, Excluir, etc.)          │
 * │    → EmptyRecord, GroupDropArea, confirmações                        │
 * │                                                                         │
 * │ 6. pager                                                               │
 * │    → Navegação de páginas, tooltips, dropdown de itens por página   │
 * │    → Formato "{0} de {1} páginas", "({0} itens)"                    │
 * │                                                                         │
 * │ 7. dropdowns                                                           │
 * │    → noRecordsTemplate, actionFailureTemplate, Selecionar tudo       │
 * │                                                                         │
 * │ 8. numerictextbox                                                      │
 * │    → Títulos dos botões de incrementar/decrementar                   │
 * │                                                                         │
 * │ 9. textbox                                                             │
 * │    → Placeholder padrão "Insira texto"                               │
 * │                                                                         │
 * │ 10. buttons                                                            │
 * │     → Textos Sim/Não para botões de confirmação                      │
 * │                                                                         │
 * │ 11. dialog                                                             │
 * │     → Botão "Fechar" de diálogos                                     │
 * │                                                                         │
 * │ 12. richtexteditor                                                     │
 * │     → Toolbar completa em pt-BR (Negrito, Itálico, Alinhar, etc.)   │
 * │     → Comandos de formatação, links, imagens, tabelas                │
 * │                                                                         │
 * │ 13. calendar (dias da semana)                                          │
 * │     → dayNames completos: Domingo, Segunda-feira, ..., Sábado        │
 * │     → dayNamesShort: Dom, Seg, Ter, Qua, Qui, Sex, Sáb              │
 * │     → monthNames: Janeiro, Fevereiro, ..., Dezembro                  │
 * │     → monthNamesShort: Jan, Fev, Mar, ..., Dez                       │
 * └─────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE INICIALIZAÇÃO:
 * 1. Layout chama window.loadSyncfusionLocalization() via await
 * 2. Função faz fetch de 6 arquivos CLDR JSON em paralelo (Promise.all)
 * 3. Verifica se ej.base está disponível (typeof ej !== 'undefined')
 * 4. ej.base.loadCldr(...cldrData) carrega dados CLDR
 * 5. ej.base.setCulture('pt-BR') define cultura padrão
 * 6. ej.base.L10n.load({ 'pt-BR': {...} }) carrega strings de interface
 * 7. Componentes Syncfusion subsequentes usam automaticamente pt-BR
 *
 * 📌 ESTRUTURA DE DADOS L10N:
 * {
 *   'pt-BR': {
 *     'component-name': {
 *       'key': 'Valor em português'
 *     }
 *   }
 * }
 *
 * Exemplo grid:
 * {
 *   'pt-BR': {
 *     grid: {
 *       EmptyRecord: 'Nenhum registro encontrado',
 *       Add: 'Adicionar',
 *       Edit: 'Editar'
 *     }
 *   }
 * }
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - CLDR files devem estar em /cldr/ (raiz do site)
 * - Promise.all garante carregamento paralelo (performance)
 * - firstDayOfWeek: 0 = Domingo (padrão brasileiro)
 * - Função async permite uso de await no caller
 * - Não há fallback se ej.base não existir (verificação silenciosa)
 * - Rich Text Editor tem 47 strings localizadas (toolbar completa)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

window.loadSyncfusionLocalization = async function ()
{
    try {
        const culture = 'pt-BR';

        const cldrFiles = [
            '/cldr/ca-gregorian.json',
            '/cldr/numbers.json',
            '/cldr/timeZoneNames.json',
            '/cldr/currencies.json',
            '/cldr/numberingSystems.json',
            '/cldr/weekData.json'
        ];

        const cldrData = await Promise.all(cldrFiles.map(url => fetch(url).then(res => res.json())));

        if (typeof ej !== 'undefined' && ej.base)
        {
            ej.base.loadCldr(...cldrData);
            ej.base.setCulture(culture);
            ej.base.L10n.load({
                'pt-BR': {
                    calendar: {
                        today: 'Hoje',
                        weekHeader: 'Sm',
                        firstDayOfWeek: 0,
                        dayNames: ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'],
                        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
                        monthNames: [
                            'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
                            'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'
                        ],
                        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun',
                            'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez']
                    },
                    datepicker: {
                        placeholder: 'Selecione uma data',
                        today: 'Hoje',
                        close: 'Fechar'
                    },
                    timepicker: {
                        placeholder: 'Selecione um horário',
                        now: 'Agora',
                        ok: 'OK',
                        cancel: 'Cancelar'
                    },
                    datetimepicker: {
                        placeholder: 'Selecione data e horário',
                        today: 'Hoje',
                        now: 'Agora',
                        ok: 'OK',
                        cancel: 'Cancelar'
                    },
                    grid: {
                        EmptyRecord: 'Nenhum registro encontrado',
                        GroupDropArea: 'Arraste um cabeçalho de coluna aqui para agrupar',
                        UnGroup: 'Clique aqui para desagrupar',
                        EmptyDataSourceError: 'DataSource não deve estar vazio no carregamento inicial',
                        Add: 'Adicionar',
                        Edit: 'Editar',
                        Cancel: 'Cancelar',
                        Update: 'Atualizar',
                        Delete: 'Excluir',
                        Print: 'Imprimir',
                        FilterButton: 'Filtrar',
                        ClearButton: 'Limpar',
                        Search: 'Buscar',
                        Save: 'Salvar',
                        ConfirmDelete: 'Tem certeza de que deseja excluir este registro?',
                        True: 'Verdadeiro',
                        False: 'Falso',
                        ChooseDate: 'Escolha uma data',
                    },
                    pager: {
                        currentPageInfo: '{0} de {1} páginas',
                        totalItemsInfo: '({0} itens)',
                        firstPageTooltip: 'Primeira página',
                        lastPageTooltip: 'Última página',
                        nextPageTooltip: 'Próxima página',
                        previousPageTooltip: 'Página anterior',
                        nextPagerTooltip: 'Próximo pager',
                        previousPagerTooltip: 'Pager anterior',
                        pagerDropDown: 'Itens por página',
                        pagerAllDropDown: 'Itens',
                        All: 'Todos'
                    },
                    dropdowns: {
                        noRecordsTemplate: 'Nenhum registro encontrado',
                        actionFailureTemplate: 'Erro ao carregar os dados',
                        select: 'Selecionar',
                        selectAllText: 'Selecionar tudo',
                        unSelectAllText: 'Desmarcar tudo',
                        placeholder: 'Selecione'
                    },
                    numerictextbox: {
                        incrementTitle: 'Incrementar valor',
                        decrementTitle: 'Decrementar valor'
                    },
                    textbox: {
                        placeholder: 'Insira texto'
                    },
                    buttons: {
                        yes: 'Sim',
                        no: 'Não'
                    },
                    dialog: {
                        close: 'Fechar'
                    },
                    richtexteditor: {
                        bold: 'Negrito',
                        italic: 'Itálico',
                        underline: 'Sublinhado',
                        strikethrough: 'Tachado',
                        superscript: 'Sobrescrito',
                        subscript: 'Subscrito',
                        justifyLeft: 'Alinhar à esquerda',
                        justifyCenter: 'Centralizar',
                        justifyRight: 'Alinhar à direita',
                        justifyFull: 'Justificar',
                        undo: 'Desfazer',
                        redo: 'Refazer',
                        clearAll: 'Limpar tudo',
                        cut: 'Cortar',
                        copy: 'Copiar',
                        paste: 'Colar',
                        fontName: 'Fonte',
                        fontSize: 'Tamanho da fonte',
                        format: 'Formato',
                        alignments: 'Alinhamentos',
                        lists: 'Listas',
                        orderedList: 'Lista ordenada',
                        unorderedList: 'Lista não ordenada',
                        insertLink: 'Inserir link',
                        openLink: 'Abrir link',
                        editLink: 'Editar link',
                        removeLink: 'Remover link',
                        image: 'Imagem',
                        fileManager: 'Gerenciador de arquivos',
                        table: 'Tabela',
                        insertTable: 'Inserir tabela',
                        insertRowBefore: 'Inserir linha acima',
                        insertRowAfter: 'Inserir linha abaixo',
                        deleteRow: 'Excluir linha',
                        insertColumnLeft: 'Inserir coluna à esquerda',
                        insertColumnRight: 'Inserir coluna à direita',
                        deleteColumn: 'Excluir coluna',
                        deleteTable: 'Excluir tabela',
                    }
                }
            });
        }
    } catch (erro) {
        console.error('[FrotiX] Erro ao carregar localização Syncfusion:', erro);
    }
}
