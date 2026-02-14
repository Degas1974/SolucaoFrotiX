/* ****************************************************************************************
 * ⚡ ARQUIVO: kendo-dropdowntree-helper.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Utilitários para inicialização e manipulação do Kendo DropDownTree
 *                   com dados planos auto-referenciados (flat→tree). Centraliza a conversão
 *                   de dados hierárquicos que o Syncfusion fazia automaticamente via
 *                   parentValue/hasChildren, mas que o Kendo DropDownTree não suporta
 *                   nativamente (requires nested items[]).
 *
 * 📥 ENTRADAS     : Arrays planos de objetos com campos id, parentId, text, hasChild
 *
 * 📤 SAÍDAS       : Funções globais window.KendoDDTHelper para init, get, set, flatToTree
 *
 * 🔗 CHAMADA POR  : Páginas CSHTML que usam Kendo DropDownTree (SetorSolicitante/Upsert,
 *                   Requisitante/Upsert, Viagens/Upsert, Viagens/TaxiLeg, etc.)
 *
 * 🔄 CHAMA        : jQuery + Kendo UI kendoDropDownTree widget
 *
 * 📦 DEPENDÊNCIAS : jQuery 3.7, kendo.all.min.js (carregados em _ScriptsBasePlugins)
 *
 * 📝 OBSERVAÇÕES  : Este arquivo deve ser carregado APÓS kendo.all.min.js.
 *                   Registrado em _ScriptsBasePlugins.cshtml para disponibilidade global.
 *                   Substitui o comportamento automático do Syncfusion ejs-dropdowntree
 *                   que convertia dados planos para árvore via campo parentValue.
 **************************************************************************************** */
(function (window) {
    "use strict";

    /****************************************************************************************
     * ⚡ OBJETO: KendoDDTHelper
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Namespace para funções utilitárias de DropDownTree Kendo UI.
     *                   Evita poluição do escopo global e centraliza toda a lógica de
     *                   conversão flat→tree + inicialização padronizada.
     ****************************************************************************************/
    var KendoDDTHelper = {};

    /****************************************************************************************
     * ⚡ FUNÇÃO: flatToTree
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : [PORQUÊ] O Kendo DropDownTree exige dados hierárquicos com array
     *                   aninhado "items", mas o backend FrotiX envia dados planos com
     *                   campo parentId (mesmo padrão do Syncfusion parentValue).
     *                   [O QUE] Converte array plano [{id, parentId, text, ...}] em árvore
     *                   aninhada [{id, text, items: [{...}]}].
     *                   [COMO] 1) Indexa todos os nós por ID em um Map.
     *                          2) Itera e anexa cada nó ao items[] do seu pai.
     *                          3) Retorna apenas os nós raiz (sem parentId válido).
     *
     * 📥 ENTRADAS     : flatData [Array] - Array plano de objetos
     *                   idField [string] - Nome do campo ID (ex: "setorSolicitanteId")
     *                   parentField [string] - Nome do campo pai (ex: "setorPaiId")
     *
     * 📤 SAÍDAS       : Array - Árvore hierárquica com propriedade "items" aninhada
     *
     * ⬅️ CHAMADO POR  : initHierarchical(), código de páginas CSHTML
     *
     * ➡️ CHAMA        : Nenhuma (função pura)
     *
     * 📝 OBSERVAÇÕES  : parentId vazio/null/Guid.Empty ("00000000-...") = nó raiz.
     *                   Preserva todas as propriedades originais dos objetos.
     ****************************************************************************************/
    KendoDDTHelper.flatToTree = function (flatData, idField, parentField) {
        try {
            if (!flatData || !Array.isArray(flatData) || flatData.length === 0) {
                return [];
            }

            var EMPTY_GUID = "00000000-0000-0000-0000-000000000000";
            var map = {};
            var roots = [];

            // [LOGICA] Fase 1: Indexar todos os nós por ID
            for (var i = 0; i < flatData.length; i++) {
                var item = $.extend({}, flatData[i]); // clone para não mutar original
                item.items = [];
                var id = String(item[idField] || "");
                if (id) {
                    map[id] = item;
                }
            }

            // [LOGICA] Fase 2: Montar hierarquia - anexar filhos ao items[] do pai
            for (var key in map) {
                if (map.hasOwnProperty(key)) {
                    var node = map[key];
                    var parentId = String(node[parentField] || "");

                    // [REGRA] Nó é raiz se parentId vazio, null ou Guid.Empty
                    if (!parentId || parentId === EMPTY_GUID || parentId === "null" || parentId === "undefined") {
                        roots.push(node);
                    } else if (map[parentId]) {
                        map[parentId].items.push(node);
                    } else {
                        // [LOGICA] Pai referenciado não existe → tratar como raiz
                        roots.push(node);
                    }
                }
            }

            // [LOGICA] Fase 3: Remover items[] vazio para nós folha (evita ícone de expansão)
            _cleanEmptyItems(roots);

            return roots;
        } catch (error) {
            Alerta.TratamentoErroComLinha("kendo-dropdowntree-helper.js", "flatToTree", error);
            return [];
        }
    };

    /****************************************************************************************
     * ⚡ FUNÇÃO: _cleanEmptyItems (privada)
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Remove propriedade items[] vazia de nós folha para que o Kendo
     *                   não mostre ícone de expansão em itens sem filhos.
     ****************************************************************************************/
    function _cleanEmptyItems(nodes) {
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].items && nodes[i].items.length === 0) {
                delete nodes[i].items;
            } else if (nodes[i].items && nodes[i].items.length > 0) {
                _cleanEmptyItems(nodes[i].items);
            }
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: initHierarchical
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : [PORQUÊ] Padrão mais comum no FrotiX: DropDownTree de Setor
     *                   Solicitante com dados planos auto-referenciados vindos do backend.
     *                   [O QUE] Inicializa um Kendo DropDownTree com conversão automática
     *                   flat→tree e configuração padronizada (filtro, placeholder, etc).
     *                   [COMO] 1) Converte flatData para árvore via flatToTree().
     *                          2) Inicializa kendoDropDownTree com a árvore.
     *                          3) Pré-seleciona valor se fornecido.
     *
     * 📥 ENTRADAS     : config [Object] com propriedades:
     *                   - selector [string] - Seletor jQuery do input (ex: "#ddtree")
     *                   - flatData [Array] - Dados planos do backend
     *                   - idField [string] - Campo ID (default: "setorSolicitanteId")
     *                   - parentField [string] - Campo pai (default: "setorPaiId")
     *                   - textField [string] - Campo texto (default: "nome")
     *                   - valueField [string] - Campo valor (default: "setorSolicitanteId")
     *                   - placeholder [string] - Placeholder (default: "Selecione um Setor...")
     *                   - value [string|null] - Valor pré-selecionado
     *                   - enabled [bool] - Habilitado (default: true)
     *                   - filter [string] - Tipo de filtro (default: "contains")
     *                   - checkboxes [bool|Object] - Config checkboxes (default: false)
     *                   - change [Function] - Callback ao mudar valor
     *                   - select [Function] - Callback ao selecionar
     *                   - height [number|string] - Altura popup (default: "auto")
     *
     * 📤 SAÍDAS       : Object - Instância do widget kendoDropDownTree
     *
     * ⬅️ CHAMADO POR  : Páginas CSHTML nos @section ScriptsBlock
     *
     * ➡️ CHAMA        : flatToTree(), jQuery.kendoDropDownTree()
     ****************************************************************************************/
    KendoDDTHelper.initHierarchical = function (config) {
        try {
            var cfg = $.extend({
                selector: "",
                flatData: [],
                idField: "setorSolicitanteId",
                parentField: "setorPaiId",
                textField: "nome",
                valueField: "setorSolicitanteId",
                placeholder: "Selecione um Setor...",
                value: null,
                enabled: true,
                filter: "contains",
                checkboxes: false,
                change: null,
                select: null,
                height: "auto"
            }, config);

            // [LOGICA] Converter dados planos para hierárquicos
            var treeData = KendoDDTHelper.flatToTree(cfg.flatData, cfg.idField, cfg.parentField);

            var widgetConfig = {
                dataSource: treeData,
                dataTextField: cfg.textField,
                dataValueField: cfg.valueField,
                placeholder: cfg.placeholder,
                height: cfg.height,
                filter: cfg.filter,
                checkboxes: cfg.checkboxes,
                autoClose: cfg.checkboxes ? false : true
            };

            if (typeof cfg.change === "function") {
                widgetConfig.change = cfg.change;
            }
            if (typeof cfg.select === "function") {
                widgetConfig.select = cfg.select;
            }

            // [UI] Inicializar widget Kendo DropDownTree
            var $el = $(cfg.selector);
            var widget = $el.kendoDropDownTree(widgetConfig).data("kendoDropDownTree");

            // [LOGICA] Pré-selecionar valor se fornecido (DDT usa arrays)
            if (cfg.value && cfg.value !== "00000000-0000-0000-0000-000000000000") {
                widget.value([String(cfg.value)]);
            }

            // [UI] Habilitar/Desabilitar
            if (!cfg.enabled) {
                widget.enable(false);
            }

            return widget;
        } catch (error) {
            Alerta.TratamentoErroComLinha("kendo-dropdowntree-helper.js", "initHierarchical", error);
            return null;
        }
    };

    /****************************************************************************************
     * ⚡ FUNÇÃO: initFlat
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : [PORQUÊ] Alguns DDTs no FrotiX usam dados planos SEM hierarquia
     *                   (ex: Requisitante, Evento) — lista simples sem parentValue.
     *                   [O QUE] Inicializa DropDownTree com dados planos (sem conversão).
     *                   [COMO] Usa DataSource direto sem flatToTree().
     *
     * 📥 ENTRADAS     : config [Object] - mesma estrutura de initHierarchical,
     *                   mas sem idField/parentField (não precisa de conversão)
     *
     * 📤 SAÍDAS       : Object - Instância do widget kendoDropDownTree
     ****************************************************************************************/
    KendoDDTHelper.initFlat = function (config) {
        try {
            var cfg = $.extend({
                selector: "",
                data: [],
                textField: "text",
                valueField: "value",
                placeholder: "Selecione...",
                value: null,
                enabled: true,
                filter: "contains",
                change: null,
                height: "auto"
            }, config);

            var widgetConfig = {
                dataSource: cfg.data,
                dataTextField: cfg.textField,
                dataValueField: cfg.valueField,
                placeholder: cfg.placeholder,
                height: cfg.height,
                filter: cfg.filter,
                checkboxes: false,
                autoClose: true
            };

            if (typeof cfg.change === "function") {
                widgetConfig.change = cfg.change;
            }

            var $el = $(cfg.selector);
            var widget = $el.kendoDropDownTree(widgetConfig).data("kendoDropDownTree");

            if (cfg.value && cfg.value !== "00000000-0000-0000-0000-000000000000") {
                widget.value([String(cfg.value)]);
            }

            if (!cfg.enabled) {
                widget.enable(false);
            }

            return widget;
        } catch (error) {
            Alerta.TratamentoErroComLinha("kendo-dropdowntree-helper.js", "initFlat", error);
            return null;
        }
    };

    /****************************************************************************************
     * ⚡ FUNÇÃO: getInstance
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Obter instância do widget kendoDropDownTree de forma segura.
     *
     * 📥 ENTRADAS     : selector [string] - Seletor jQuery (ex: "#ddtree")
     *
     * 📤 SAÍDAS       : Object|null - Instância do widget ou null
     ****************************************************************************************/
    KendoDDTHelper.getInstance = function (selector) {
        try {
            var $el = $(selector);
            if ($el.length === 0) return null;
            return $el.data("kendoDropDownTree") || null;
        } catch (error) {
            return null;
        }
    };

    /****************************************************************************************
     * ⚡ FUNÇÃO: getValue
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Obter o valor selecionado do DropDownTree.
     *                   Retorna o primeiro valor do array (DDT sempre usa arrays).
     *
     * 📥 ENTRADAS     : selector [string] - Seletor jQuery
     *
     * 📤 SAÍDAS       : string|null - Primeiro valor selecionado ou null
     ****************************************************************************************/
    KendoDDTHelper.getValue = function (selector) {
        try {
            var widget = KendoDDTHelper.getInstance(selector);
            if (!widget) return null;
            var val = widget.value();
            return (val && val.length > 0) ? val[0] : null;
        } catch (error) {
            return null;
        }
    };

    /****************************************************************************************
     * ⚡ FUNÇÃO: getValues
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Obter todos os valores selecionados (para modo checkbox).
     *
     * 📥 ENTRADAS     : selector [string] - Seletor jQuery
     *
     * 📤 SAÍDAS       : Array - Array de valores selecionados
     ****************************************************************************************/
    KendoDDTHelper.getValues = function (selector) {
        try {
            var widget = KendoDDTHelper.getInstance(selector);
            if (!widget) return [];
            return widget.value() || [];
        } catch (error) {
            return [];
        }
    };

    /****************************************************************************************
     * ⚡ FUNÇÃO: setValue
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Definir valor selecionado no DropDownTree.
     *
     * 📥 ENTRADAS     : selector [string] - Seletor jQuery
     *                   value [string|Array] - Valor a selecionar
     *
     * 📤 SAÍDAS       : void
     ****************************************************************************************/
    KendoDDTHelper.setValue = function (selector, value) {
        try {
            var widget = KendoDDTHelper.getInstance(selector);
            if (!widget) return;

            if (value === null || value === undefined || value === "" ||
                value === "00000000-0000-0000-0000-000000000000") {
                widget.value([]);
            } else if (Array.isArray(value)) {
                widget.value(value);
            } else {
                widget.value([String(value)]);
            }
        } catch (error) {
            Alerta.TratamentoErroComLinha("kendo-dropdowntree-helper.js", "setValue", error);
        }
    };

    /****************************************************************************************
     * ⚡ FUNÇÃO: enable
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Habilitar ou desabilitar o DropDownTree.
     *
     * 📥 ENTRADAS     : selector [string], enabled [bool]
     *
     * 📤 SAÍDAS       : void
     ****************************************************************************************/
    KendoDDTHelper.enable = function (selector, enabled) {
        try {
            var widget = KendoDDTHelper.getInstance(selector);
            if (widget) widget.enable(enabled !== false);
        } catch (error) {
            Alerta.TratamentoErroComLinha("kendo-dropdowntree-helper.js", "enable", error);
        }
    };

    /****************************************************************************************
     * ⚡ FUNÇÃO: setDataSource
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Atualizar o dataSource do DropDownTree com novos dados planos
     *                   (converte flat→tree antes de aplicar). Útil para recarregar
     *                   dados dinamicamente (ex: GestaoRecursosNavegacao).
     *
     * 📥 ENTRADAS     : selector [string] - Seletor jQuery
     *                   flatData [Array] - Novos dados planos
     *                   idField [string] - Campo ID
     *                   parentField [string] - Campo pai
     *
     * 📤 SAÍDAS       : void
     ****************************************************************************************/
    KendoDDTHelper.setDataSource = function (selector, flatData, idField, parentField) {
        try {
            var widget = KendoDDTHelper.getInstance(selector);
            if (!widget) return;

            var treeData = KendoDDTHelper.flatToTree(flatData, idField, parentField);
            widget.setDataSource(new kendo.data.HierarchicalDataSource({
                data: treeData
            }));
        } catch (error) {
            Alerta.TratamentoErroComLinha("kendo-dropdowntree-helper.js", "setDataSource", error);
        }
    };

    /****************************************************************************************
     * ⚡ FUNÇÃO: setFlatDataSource
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Atualizar dataSource com dados planos SEM hierarquia.
     *
     * 📥 ENTRADAS     : selector [string], data [Array]
     *
     * 📤 SAÍDAS       : void
     ****************************************************************************************/
    KendoDDTHelper.setFlatDataSource = function (selector, data) {
        try {
            var widget = KendoDDTHelper.getInstance(selector);
            if (!widget) return;

            widget.setDataSource(new kendo.data.HierarchicalDataSource({
                data: data || []
            }));
        } catch (error) {
            Alerta.TratamentoErroComLinha("kendo-dropdowntree-helper.js", "setFlatDataSource", error);
        }
    };

    /****************************************************************************************
     * ⚡ FUNÇÃO: destroy
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Destruir instância do DropDownTree para re-criação.
     *
     * 📥 ENTRADAS     : selector [string]
     *
     * 📤 SAÍDAS       : void
     ****************************************************************************************/
    KendoDDTHelper.destroy = function (selector) {
        try {
            var widget = KendoDDTHelper.getInstance(selector);
            if (widget) {
                widget.destroy();
                $(selector).empty();
            }
        } catch (error) {
            Alerta.TratamentoErroComLinha("kendo-dropdowntree-helper.js", "destroy", error);
        }
    };

    // [LOGICA] Expor globalmente
    window.KendoDDTHelper = KendoDDTHelper;

})(window);
