/* ****************************************************************************************
 * ⚡ ARQUIVO: dialogs.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciamento de diálogos e modais do sistema de agendamento. Cria
 *                   e controla Syncfusion Dialogs para alertas customizados (ex:
 *                   inconsistência de dia da semana), gerencia callbacks legacy de
 *                   overlay click, open/close, e button click. Exporta funções globais
 *                   window.* para uso em event handlers e validações.
 * 📥 ENTRADAS     : args (object com .checked para checkbox), referências globais
 *                   (window.dialogObj, window.dialogBtn), IDs de elementos DOM
 *                   (#txtDataInicial, #lstDias, #dialog-container-diassemana)
 * 📤 SAÍDAS       : Syncfusion Dialog criado e renderizado no DOM (appendTo), campos
 *                   limpos (value=null, val("")), foco em campos (#txtDataInicial),
 *                   exibição/ocultação de elementos (display block/none), dialog
 *                   destroy ao fechar
 * 🔗 CHAMADA POR  : Validações (validacao.js), event handlers de recorrência, botões
 *                   de formulário, callbacks de checkbox
 * 🔄 CHAMA        : ej.popups.Dialog (Syncfusion Dialog API), document.getElementById,
 *                   jQuery ($), ej2_instances[0] (Syncfusion components), Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 (Dialog, MultiSelect), jQuery, Font Awesome icons,
 *                   Alerta.js
 * 📝 OBSERVAÇÕES  : Exporta 6 funções window.* (showDialogDiasSemana, onChange,
 *                   dialogClose, dialogOpen, dlgButtonClick, dlgButtonCloseClick).
 *                   Todas têm try-catch completo. showDialogDiasSemana cria dialog
 *                   dinâmico com animação Zoom, 3 botões customizados, close callback
 *                   com destroy. Legacy callbacks mantidos para compatibilidade com
 *                   código antigo (window.dialogObj, window.dialogBtn).
 *
 * 📋 ÍNDICE DE FUNÇÕES (6 funções globais window.*):
 *
 * ┌─ DIALOG PRINCIPAL (Inconsistência de Dia da Semana) ───────────────────┐
 * │ 1. window.showDialogDiasSemana()                                       │
 * │    → Cria Syncfusion Dialog para aviso de dia da semana inconsistente  │
 * │    → Config:                                                            │
 * │      • header: HTML com ícones Font Awesome (fa-exclamation-triangle)  │
 * │      • content: HTML com mensagem e pergunta "O que deseja fazer?"     │
 * │      • showCloseIcon: true, closeOnEscape: false, isModal: true        │
 * │      • position: center X/Y                                            │
 * │      • animationSettings: { effect: 'Zoom' }                           │
 * │      • cssClass: 'custom-dialog', width: '450px', height: 'auto'       │
 * │      • visible: true (exibe imediatamente)                             │
 * │    → Botões (3):                                                        │
 * │      1. Ignorar (e-success, isPrimary): dialog.hide()                  │
 * │      2. Mudar Data Inicial (e-warning):                                │
 * │         - Limpa txtDataInicial (ej2_instances ou jQuery)               │
 * │         - Focus em txtDataInicial                                      │
 * │         - dialog.hide()                                                │
 * │      3. Limpar Dias da Semana (e-danger):                              │
 * │         - Limpa lstDias.value = [] (se MultiSelect)                    │
 * │         - dialog.hide()                                                │
 * │    → close callback: dialog.destroy() (limpa memória)                  │
 * │    → appendTo('#dialog-container-diassemana')                          │
 * │    → try-catch: Alerta.TratamentoErroComLinha                          │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ CALLBACKS LEGACY (mantidos para compatibilidade) ──────────────────────┐
 * │ 2. window.onChange(args)                                                │
 * │    → param {object} args - Objeto com .checked (boolean)               │
 * │    → Se window.dialogObj existe:                                       │
 * │      • Se args.checked: overlayClick = function() { hide() }           │
 * │      • Senão: overlayClick = function() { show() }                     │
 * │    → Alterna comportamento de click no overlay (hide vs show)          │
 * │    → try-catch: Alerta.TratamentoErroComLinha                          │
 * │                                                                          │
 * │ 3. window.dialogClose()                                                 │
 * │    → Se window.dialogBtn existe: dialogBtn.style.display = 'block'     │
 * │    → Mostra botão de dialog ao fechar                                  │
 * │    → try-catch: Alerta.TratamentoErroComLinha                          │
 * │                                                                          │
 * │ 4. window.dialogOpen()                                                  │
 * │    → Se window.dialogObj existe: dialogBtn.style.display = 'none'      │
 * │    → Esconde botão de dialog ao abrir                                  │
 * │    → try-catch: Alerta.TratamentoErroComLinha                          │
 * │                                                                          │
 * │ 5. window.dlgButtonClick()                                              │
 * │    → Se window.dialogObj existe: dialogObj.hide()                      │
 * │    → Comentário: "Lógica para inserir o requisitante" (não implementado)│
 * │    → try-catch: Alerta.TratamentoErroComLinha                          │
 * │                                                                          │
 * │ 6. window.dlgButtonCloseClick()                                         │
 * │    → Se window.dialogObj existe: dialogObj.hide()                      │
 * │    → try-catch: Alerta.TratamentoErroComLinha                          │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE CRIAÇÃO DE DIALOG (showDialogDiasSemana):
 * 1. Chamada: showDialogDiasSemana()
 * 2. new ej.popups.Dialog({ config completa })
 * 3. Cria header HTML com ícones Font Awesome (fa-exclamation-triangle)
 * 4. Cria content HTML com mensagem de inconsistência
 * 5. Define 3 botões com click handlers:
 *    a. Ignorar: fecha dialog
 *    b. Mudar Data Inicial: limpa txtDataInicial + focus + fecha
 *    c. Limpar Dias da Semana: limpa lstDias + fecha
 * 6. Define close callback: destroy dialog (libera memória)
 * 7. appendTo('#dialog-container-diassemana')
 * 8. Dialog exibido automaticamente (visible: true)
 *
 * 🔄 FLUXO DE BOTÃO "Mudar Data Inicial":
 * 1. Usuário clica em "Mudar Data Inicial"
 * 2. Tenta limpar via Syncfusion: txtDataInicial.ej2_instances[0]
 *    - value = null
 *    - dataBind()
 * 3. Se falhar, usa jQuery fallback: $('#txtDataInicial').val('')
 * 4. Focus em txtDataInicial: $('#txtDataInicial').focus()
 * 5. Fecha dialog: dialog.hide()
 * 6. Close callback dispara: dialog.destroy()
 *
 * 🔄 FLUXO DE BOTÃO "Limpar Dias da Semana":
 * 1. Usuário clica em "Limpar Dias da Semana"
 * 2. Obtém lstDias.ej2_instances[0]
 * 3. Verifica se é MultiSelect: instanceof ej.dropdowns.MultiSelect
 * 4. Se sim: diasSelect.value = [] (limpa array de seleção)
 * 5. Fecha dialog: dialog.hide()
 * 6. Close callback dispara: dialog.destroy()
 *
 * 🔄 FLUXO LEGACY onChange (overlayClick):
 * 1. Checkbox muda estado → onChange(args)
 * 2. Se args.checked = true:
 *    - overlayClick = hide() (clicar no overlay fecha dialog)
 * 3. Se args.checked = false:
 *    - overlayClick = show() (clicar no overlay reabre dialog)
 * 4. Comportamento alternado conforme checkbox
 *
 * 📌 ESTRUTURA DE DIALOG (Syncfusion):
 * - header: HTML string com ícones e texto centralizado
 * - content: HTML string com parágrafos e ícones
 * - buttons: Array de { click, buttonModel: { content, isPrimary?, cssClass } }
 * - animationSettings: { effect: 'Zoom'|'Fade'|'SlideDown'|... }
 * - position: { X: 'center'|'left'|'right', Y: 'center'|'top'|'bottom' }
 * - closeOnEscape: true/false (permite fechar com ESC)
 * - isModal: true (overlay escurece fundo)
 * - visible: true (exibe ao criar)
 * - close: callback executado ao fechar
 * - appendTo: seletor CSS do container
 *
 * 📌 BOTÕES CUSTOMIZADOS:
 * - Ignorar: e-success (verde), isPrimary: true (destacado)
 * - Mudar Data Inicial: e-warning (amarelo/laranja)
 * - Limpar Dias da Semana: e-danger (vermelho)
 * - Todos têm ícones Font Awesome no content (fa-rocket-launch, fa-calendar, fa-broom-ball)
 * - cssClass: 'custom-button' para estilo adicional
 *
 * 📌 ÍCONES FONT AWESOME USADOS:
 * - fa-exclamation-triangle (header): alerta de inconsistência
 * - fa-calendar (content): referência a data
 * - fa-light fa-rocket-launch (botão Ignorar): continuar
 * - fa-calendar (botão Mudar Data): alterar data
 * - fa-regular fa-broom-ball (botão Limpar): limpar seleção
 *
 * 📌 VARIÁVEIS GLOBAIS LEGACY:
 * - window.dialogObj: instância de Syncfusion Dialog (usado em callbacks)
 * - window.dialogBtn: elemento de botão (mostrado/escondido em open/close)
 * - Mantidas para compatibilidade com código antigo
 * - onChange, dialogClose, dialogOpen, dlgButtonClick, dlgButtonCloseClick dependem delas
 *
 * 📌 LIMPEZA DE CAMPOS:
 * - txtDataInicial: tenta Syncfusion (ej2_instances[0].value=null + dataBind()), fallback jQuery
 * - lstDias: apenas Syncfusion (value=[]), sem fallback
 * - Após limpar: focus em campo relevante
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Dialog destruído ao fechar (close callback) para liberar memória
 * - showDialogDiasSemana cria novo Dialog a cada chamada (não reutiliza)
 * - closeOnEscape: false (força usuário a escolher botão, não permite ESC)
 * - isModal: true (bloqueia interação com resto da página)
 * - width: '450px', height: 'auto' (ajusta altura ao conteúdo)
 * - animationSettings Zoom: efeito de zoom in/out
 * - Legacy callbacks usam window.dialogObj (não a instância local 'dialog')
 * - dlgButtonClick tem comentário de lógica não implementada
 * - Todos os callbacks legacy verificam existência de dialogObj/dialogBtn antes de usar
 * - Font Awesome classes variam: fa (regular), fa-light, fa-regular (diferentes pesos)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/**
 * Mostra diálogo de inconsistência de dia da semana
 */
window.showDialogDiasSemana = function ()
{
    try
    {
        // Create a new instance of Syncfusion Dialog
        const dialog = new ej.popups.Dialog({
            header: '<div style="display: flex; align-items: center; justify-content: space-between;">' +
                '<i class="fa fa-exclamation-triangle" aria-hidden="true" style="color: #e67e22;"></i>' +
                '<span style="flex-grow: 1; text-align: center;">Dia da semana inconsistente</span>' +
                '<i class="fa fa-exclamation-triangle" aria-hidden="true" style="color: #e67e22;"></i>' +
                '</div>',
            content: '<div style="font-size: 1.1em; color: #555; line-height: 1.5;">' +
                '<p><i class="fa fa-calendar" aria-hidden="true" style="color: #3498db;"></i> O dia da semana da data inicial não corresponde a nenhum dos dias selecionados.</p>' +
                '<p><strong>O que deseja fazer?</strong></p>' +
                '</div>',
            showCloseIcon: true,
            closeOnEscape: false,
            isModal: true,
            position: { X: "center", Y: "center" },
            buttons: [
                {
                    click: function ()
                    {
                        dialog.hide();
                    },
                    buttonModel: {
                        content: '<i class="fa-light fa-rocket-launch" aria-hidden="true"></i> Ignorar',
                        isPrimary: true,
                        cssClass: 'e-success custom-button'
                    }
                },
                {
                    click: function ()
                    {
                        window.setKendoDateValue("txtDataInicial", null);
                        document.getElementById('txtDataInicial')?.focus();
                        dialog.hide();
                    },
                    buttonModel: {
                        content: '<i class="fa fa-calendar" aria-hidden="true"></i> Mudar Data Inicial',
                        cssClass: 'e-warning custom-button'
                    }
                },
                {
                    click: function ()
                    {
                        const diasSelect = window.getSyncfusionInstance('lstDias');
                        if (diasSelect)
                        {
                            diasSelect.value = [];
                        }
                        dialog.hide();
                    },
                    buttonModel: {
                        content: '<i class="fa-regular fa-broom-ball" aria-hidden="true"></i> Limpar Dias da Semana',
                        cssClass: 'e-danger custom-button'
                    }
                }
            ],
            animationSettings: { effect: 'Zoom' },
            cssClass: 'custom-dialog',
            width: '450px',
            height: 'auto',
            visible: true,
            close: () =>
            {
                dialog.destroy();
            }
        });

        // Append dialog to the specified container
        dialog.appendTo('#dialog-container-diassemana');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("dialogs.js", "showDialogDiasSemana", error);
    }
};

/**
 * Callbacks de diálogo (legacy - manter para compatibilidade)
 */
window.onChange = function (args)
{
    try
    {
        if (window.dialogObj)
        {
            if (args.checked)
            {
                window.dialogObj.overlayClick = function ()
                {
                    window.dialogObj.hide();
                };
            } else
            {
                window.dialogObj.overlayClick = function ()
                {
                    window.dialogObj.show();
                };
            }
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("dialogs.js", "onChange", error);
    }
};

window.dialogClose = function ()
{
    try
    {
        if (window.dialogBtn)
        {
            window.dialogBtn.style.display = 'block';
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("dialogs.js", "dialogClose", error);
    }
};

window.dialogOpen = function ()
{
    try
    {
        if (window.dialogBtn)
        {
            window.dialogBtn.style.display = 'none';
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("dialogs.js", "dialogOpen", error);
    }
};

window.dlgButtonClick = function ()
{
    try
    {
        if (window.dialogObj)
        {
            window.dialogObj.hide();
        }
        // Lógica para inserir o requisitante
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("dialogs.js", "dlgButtonClick", error);
    }
};

window.dlgButtonCloseClick = function ()
{
    try
    {
        if (window.dialogObj)
        {
            window.dialogObj.hide();
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("dialogs.js", "dlgButtonCloseClick", error);
    }
};
