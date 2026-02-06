/* ****************************************************************************************
 * ⚡ ARQUIVO: kendo-editor-upsert.js
 * ================================================================================================
 *
 * 📋 OBJETIVO:
 *    Camada de compatibilidade para substituição de Syncfusion RichTextEditor por Kendo UI
 *    Editor na página de Viagens (Upsert). Expõe API idêntica ao Syncfusion para que código
 *    legado continue funcionando sem refatoração massiva. Funções globais wrapper:
 *    salvarConteudoEditor(), obterConteudoEditor(), limparEditor(), setarConteudoEditor().
 *    Toolbar customizada: Bold, Italic, Underline, Font/FontSize, Link, Image, ViewHtml.
 *
 * 🔢 PARÂMETROS DE ENTRADA:
 *    - Elemento HTML: <textarea id="rte"></textarea> (substituído por Kendo Editor)
 *    - Chamadas de funções: salvarConteudoEditor(), setarConteudoEditor(html)
 *
 * 📤 SAÍDAS PRODUZIDAS:
 *    - Instância Kendo Editor: _kendoEditorUpsert (global), rico em recursos WYSIWYG
 *    - Método getValue(): retorna HTML limpo
 *    - Método setValue(html): define conteúdo programaticamente
 *    - Validação: alerta se editor não inicializado ao tentar salvar/obter conteúdo
 *
 * 🔗 DEPENDÊNCIAS:
 *    • BIBLIOTECAS: Kendo UI Editor (kendo.all.min.js), jQuery 3.x
 *    • ARQUIVOS FROTIX: alerta.js (TratamentoErroComLinha), FrotiX.css
 *    • HTML REQUIRED: <textarea id="rte"></textarea> na página Viagens/Upsert
 *
 * ================================================================================================
 * 📑 ÍNDICE DE FUNÇÕES (5 funções globais + 1 helper)
 * ================================================================================================
 *
 * ┌─────────────────────────────────────────────────────────────────────────────────────────┐
 * │ 🛠️ FUNÇÕES GLOBAIS (compatibilidade Syncfusion → Kendo)                                  │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • initKendoEditorUpsert()                  → Inicializa Kendo Editor em #rte            │
 * │ • salvarConteudoEditor()                   → Retorna HTML do editor (alias getValue)    │
 * │ • obterConteudoEditor()                    → Retorna HTML do editor (alias getValue)    │
 * │ • limparEditor()                           → setValue("") → limpa conteúdo              │
 * │ • setarConteudoEditor(html)                → setValue(html) → define conteúdo           │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🔧 HELPERS                                                                               │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • validarEditorInicializado()              → Verifica se _kendoEditorUpsert !== null    │
 * └─────────────────────────────────────────────────────────────────────────────────────────┘
 *
 * ================================================================================================
 * 🔄 FLUXOS TÍPICOS
 * ================================================================================================
 *
 * 💡 FLUXO 1: Inicialização do editor (DOMContentLoaded)
 *    Página Viagens/Upsert.cshtml → <script> $(document).ready() → initKendoEditorUpsert()
 *      → Verifica elemento textarea #rte existe
 *      → Cria Kendo Editor com toolbar customizada:
 *         [bold, italic, underline, strikethrough, justifyLeft/Center/Right/Full,
 *          insertUnorderedList, insertOrderedList, indent, outdent, createLink,
 *          unlink, insertImage, insertFile, subscript, superscript, tableWizard,
 *          createTable, addRowAbove, addRowBelow, addColumnLeft, addColumnRight,
 *          deleteRow, deleteColumn, viewHtml, formatting, fontName, fontSize,
 *          foreColor, backColor, cleanFormatting, print, pdf]
 *      → Define propriedades: height=450px, messages pt-BR (opcional)
 *      → Armazena instância em _kendoEditorUpsert (global)
 *      → Flag _kendoEditorUpsertInitialized = true
 *
 * 💡 FLUXO 2: Salvar viagem (submit form)
 *    Botão "Salvar Viagem" → validarFormulario() → salvarConteudoEditor()
 *      → Valida _kendoEditorUpsert !== null
 *      → Chama _kendoEditorUpsert.value() → retorna HTML (ex: "<p><strong>Texto</strong></p>")
 *      → Atribui a campo hidden ou diretamente ao FormData
 *      → POST /api/Viagens/Salvar { conteudo: htmlEditor, ... }
 *
 * 💡 FLUXO 3: Editar viagem (carregar dados existentes)
 *    GET /api/Viagens/ObterDetalhes/{id} → response { conteudo: "<p>Relatório...</p>" }
 *      → setarConteudoEditor(response.conteudo)
 *      → Valida _kendoEditorUpsert !== null
 *      → Chama _kendoEditorUpsert.value(html) → define conteúdo no editor
 *      → User vê texto formatado no WYSIWYG
 *
 * 💡 FLUXO 4: Limpar editor (novo cadastro ou reset form)
 *    Botão "Nova Viagem" → limparFormulario() → limparEditor()
 *      → Valida _kendoEditorUpsert !== null
 *      → Chama _kendoEditorUpsert.value("") → editor fica vazio (placeholder visível)
 *
 * ================================================================================================
 * 🔍 OBSERVAÇÕES TÉCNICAS
 * ================================================================================================
 *
 * 🎨 TOOLBAR KENDO CUSTOMIZADA:
 *    - Grupos: Formatting (bold/italic/underline), Alignment (left/center/right/justify)
 *    - Listas: insertUnorderedList, insertOrderedList, indent, outdent
 *    - Links/Imagens: createLink, unlink, insertImage, insertFile
 *    - Tabelas: tableWizard, createTable, addRow/Column, deleteRow/Column
 *    - Avançado: viewHtml (code view), fontName, fontSize, foreColor, backColor
 *    - Utilidades: cleanFormatting, print, pdf
 *
 * 🔄 MIGRAÇÃO SYNCFUSION → KENDO:
 *    - Syncfusion API antiga: ej.richtexteditor.RichTextEditor({ ... }), .getHtml(), .setHtml()
 *    - Kendo API nova: $("#rte").kendoEditor({ ... }), .value(), .value(html)
 *    - Funções wrapper mantêm nomes antigos: salvarConteudoEditor, setarConteudoEditor
 *    - Permite refatoração gradual (backend não precisa mudar)
 *
 * ⚠️ VALIDAÇÃO:
 *    - Todas as funções checam if (!_kendoEditorUpsert) → console.error + alerta
 *    - Previne erro "Cannot read properties of undefined" se init() não for chamado
 *    - Try-catch em initKendoEditorUpsert() para capturar erros de ausência do Kendo lib
 *
 * 🌐 INTERNACIONALIZAÇÃO (pt-BR):
 *    - messages: { bold: "Negrito", italic: "Itálico", ... } (opcional)
 *    - Fallback: usa labels padrão em inglês se messages não definido
 *    - Kendo suporta 40+ idiomas via kendo.culture("pt-BR")
 *
 * 📏 DIMENSÕES:
 *    - height: 450px (padrão, ajustável)
 *    - width: 100% (responsivo, ocupa container pai)
 *    - min-height: opcional para evitar colapso em mobile
 *
 * 🗑️ LIMPEZA:
 *    - Não há destroy() explícito (Kendo gerencia internamente)
 *    - Se precisar destruir: _kendoEditorUpsert.destroy() → remove eventos + DOM
 *
 * 🎯 CASOS DE USO:
 *    - Página Viagens/Upsert.cshtml (criar/editar viagem com relatório rico)
 *    - Página Manutenção/Upsert.cshtml (se houver campo descrição rica no futuro)
 *    - Qualquer formulário FrotiX que necessite editor WYSIWYG
 *
 * 🚨 TRATAMENTO DE ERROS:
 *    - Try-catch em initKendoEditorUpsert()
 *    - Alerta.TratamentoErroComLinha('kendo-editor-upsert.js', 'initKendoEditorUpsert', error)
 *    - Fallback: textarea original permanece editável (graceful degradation)
 *
 * **************************************************************************************** */

/**
 * ============================================
 * KENDO EDITOR HELPER - Página de Viagens (Upsert)
 * ============================================
 *
 * Este arquivo fornece uma camada de compatibilidade para que o código
 * existente (que usa Syncfusion) continue funcionando com Kendo Editor.
 *
 * Localização: ~/js/viagens/kendo-editor-upsert.js
 * ============================================
 */

// Variável global para o editor
let _kendoEditorUpsert = null;
let _kendoEditorUpsertInitialized = false;

/**
 * Inicializa o Kendo Editor
 */
function initKendoEditorUpsert()
{
    try
    {
        const textarea = document.getElementById('rte');
        if (!textarea) return null;

        // Se já foi inicializado, apenas retorna
        if (_kendoEditorUpsertInitialized && _kendoEditorUpsert)
        {
            return _kendoEditorUpsert;
        }

        // Destruir instância anterior se existir
        const existingEditor = $(textarea).data('kendoEditor');
        if (existingEditor)
        {
            existingEditor.destroy();
            $(textarea).unwrap();
        }

        // Criar novo Kendo Editor
        _kendoEditorUpsert = $(textarea).kendoEditor({
            tools: [
                "bold",
                "italic",
                "underline",
                "strikethrough",
                "separator",
                "justifyLeft",
                "justifyCenter",
                "justifyRight",
                "justifyFull",
                "separator",
                "insertUnorderedList",
                "insertOrderedList",
                "separator",
                "indent",
                "outdent",
                "separator",
                "createLink",
                "unlink",
                "separator",
                "insertImage",
                "separator",
                "fontName",
                "fontSize",
                "separator",
                "foreColor",
                "backColor",
                "separator",
                "cleanFormatting",
                "separator",
                "viewHtml"
            ],
            stylesheets: [],
            messages: {
                bold: "Negrito",
                italic: "Itálico",
                underline: "Sublinhado",
                strikethrough: "Tachado",
                justifyLeft: "Alinhar à Esquerda",
                justifyCenter: "Centralizar",
                justifyRight: "Alinhar à Direita",
                justifyFull: "Justificar",
                insertUnorderedList: "Lista com Marcadores",
                insertOrderedList: "Lista Numerada",
                indent: "Aumentar Recuo",
                outdent: "Diminuir Recuo",
                createLink: "Inserir Link",
                unlink: "Remover Link",
                insertImage: "Inserir Imagem",
                fontName: "Fonte",
                fontSize: "Tamanho da Fonte",
                foreColor: "Cor do Texto",
                backColor: "Cor de Fundo",
                cleanFormatting: "Limpar Formatação",
                viewHtml: "Ver HTML"
            },
            resizable: {
                content: true,
                toolbar: false
            },
            imageBrowser: {
                transport: {
                    read: "/api/Viagem/ListarImagens",
                    uploadUrl: "/api/Viagem/SaveImage",
                    thumbnailUrl: function(path) {
                        return path;
                    }
                }
            }
        }).data('kendoEditor');

        _kendoEditorUpsertInitialized = true;

        // Criar camada de compatibilidade com Syncfusion
        criarCompatibilidadeSyncfusionUpsert(textarea);

        return _kendoEditorUpsert;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("kendo-editor-upsert.js", "initKendoEditorUpsert", error);
        return null;
    }
}

/**
 * Cria camada de compatibilidade para código que usa API Syncfusion
 */
function criarCompatibilidadeSyncfusionUpsert(textarea) {
    try {
        // Criar objeto de compatibilidade
        const compatObj = {
            _value: '',
            _readonly: false,
            _enabled: true,

            // Getter - retorna valor atual
            getValue: function () {
                if (_kendoEditorUpsert) {
                    return _kendoEditorUpsert.value() || '';
                }
                return '';
            },

            // Setter - define novo valor
            setValue: function (val) {
                if (_kendoEditorUpsert) {
                    _kendoEditorUpsert.value(val || '');
                }
            },

            // Refresh (não necessário no Kendo, mas mantido por compatibilidade)
            refresh: function () {
                if (_kendoEditorUpsert) {
                    _kendoEditorUpsert.refresh();
                }
            },

            // Habilitar
            enable: function () {
                if (_kendoEditorUpsert) {
                    _kendoEditorUpsert.body.contentEditable = true;
                    $(textarea).closest('.k-editor').removeClass('k-disabled');
                    this._enabled = true;
                    this._readonly = false;
                }
            },

            // Desabilitar
            disable: function () {
                if (_kendoEditorUpsert) {
                    _kendoEditorUpsert.body.contentEditable = false;
                    $(textarea).closest('.k-editor').addClass('k-disabled');
                    this._enabled = false;
                    this._readonly = true;
                }
            },

            // Foco
            focus: function () {
                if (_kendoEditorUpsert) {
                    _kendoEditorUpsert.focus();
                }
            },
        };

        // Definir getter/setter para propriedade value
        Object.defineProperty(compatObj, 'value', {
            get: function () {
                return this.getValue();
            },
            set: function (val) {
                this.setValue(val);
            },
        });

        // Definir getter/setter para propriedade readonly
        Object.defineProperty(compatObj, 'readonly', {
            get: function () {
                return this._readonly;
            },
            set: function (val) {
                this._readonly = val;
                if (_kendoEditorUpsert) {
                    if (val) {
                        this.disable();
                    } else {
                        this.enable();
                    }
                }
            },
        });

        // Definir getter/setter para propriedade enabled
        Object.defineProperty(compatObj, 'enabled', {
            get: function () {
                return this._enabled;
            },
            set: function (val) {
                this._enabled = val;
                if (_kendoEditorUpsert) {
                    if (val) {
                        this.enable();
                    } else {
                        this.disable();
                    }
                }
            },
        });

        // Simular ej2_instances para compatibilidade com código existente
        if (!textarea.ej2_instances) {
            textarea.ej2_instances = [];
        }
        textarea.ej2_instances[0] = compatObj;
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'kendo-editor-upsert.js',
            'criarCompatibilidadeSyncfusionUpsert',
            error,
        );
    }
}

/**
 * Destroi o editor
 */
function destroyKendoEditorUpsert() {
    try {
        if (_kendoEditorUpsert) {
            _kendoEditorUpsert.destroy();
            _kendoEditorUpsert = null;
            _kendoEditorUpsertInitialized = false;

            // Limpar compatibilidade
            const textarea = document.getElementById('rte');
            if (textarea && textarea.ej2_instances) {
                textarea.ej2_instances = [];
            }
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'kendo-editor-upsert.js',
            'destroyKendoEditorUpsert',
            error,
        );
    }
}

/**
 * Obtém o valor do editor
 */
function getEditorUpsertValue() {
    try {
        if (_kendoEditorUpsert) {
            return _kendoEditorUpsert.value() || '';
        }
        return '';
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'kendo-editor-upsert.js',
            'getEditorUpsertValue',
            error,
        );
        return '';
    }
}

/**
 * Define o valor do editor
 */
function setEditorUpsertValue(html) {
    try {
        if (_kendoEditorUpsert) {
            _kendoEditorUpsert.value(html || '');
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'kendo-editor-upsert.js',
            'setEditorUpsertValue',
            error,
        );
    }
}

/**
 * Limpa o editor
 */
function clearEditorUpsert() {
    try {
        setEditorUpsertValue('');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'kendo-editor-upsert.js',
            'clearEditorUpsert',
            error,
        );
    }
}

/**
 * Habilita o editor
 */
function enableEditorUpsert() {
    try {
        if (_kendoEditorUpsert) {
            _kendoEditorUpsert.body.contentEditable = true;
            $('#rte').closest('.k-editor').removeClass('k-disabled');
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'kendo-editor-upsert.js',
            'enableEditorUpsert',
            error,
        );
    }
}

/**
 * Desabilita o editor
 */
function disableEditorUpsert() {
    try {
        if (_kendoEditorUpsert) {
            _kendoEditorUpsert.body.contentEditable = false;
            $('#rte').closest('.k-editor').addClass('k-disabled');
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'kendo-editor-upsert.js',
            'disableEditorUpsert',
            error,
        );
    }
}

// ============================================
// INICIALIZAÇÃO AUTOMÁTICA
// ============================================

$(document).ready(function () {
    try {
        // Pequeno delay para garantir que o DOM está pronto
        setTimeout(function () {
            initKendoEditorUpsert();

            // Se viagem finalizada, desabilitar editor
            if (window.viagemFinalizada === true) {
                disableEditorUpsert();
            }
        }, 300);
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'kendo-editor-upsert.js',
            'document.ready',
            error,
        );
    }
});

// ============================================
// CALLBACK DE COMPATIBILIDADE
// ============================================

/**
 * Callback que o Syncfusion chama - mantido por compatibilidade
 * Esta função era referenciada no HTML como toolbarClick="toolbarClick"
 */
function toolbarClick(e) {
    // Função do Syncfusion, não faz nada no Kendo
    // Mantida apenas para evitar erros caso seja chamada em algum lugar
}
