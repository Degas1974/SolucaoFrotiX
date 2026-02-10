/* ****************************************************************************************
 * ⚡ ARQUIVO: botao-loading.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Sistema automático de loading state para botões Bootstrap com spinner.
 *                   Ativa/desativa estado de carregamento ao clicar em botões .btn-loading
 *                   com troca de texto, exibição de spinner e cursor "wait".
 * 📥 ENTRADAS     : Click events em botões com classe .btn-loading, atributo data-loading-text
 * 📤 SAÍDAS       : Botão desabilitado com spinner visível, cursor "wait", evento customizado
 * 🔗 CHAMADA POR  : Event delegation (document.on click), qualquer botão .btn-loading no DOM
 * 🔄 CHAMA        : jQuery .find(), .prop(), .removeClass(), .addClass(), .trigger()
 * 📦 DEPENDÊNCIAS : jQuery, Bootstrap 5 (spinner-border), Alerta.js
 * 📝 OBSERVAÇÕES  : IIFE auto-executável, event delegation para botões dinâmicos, callback
 *                   done() via evento customizado "btn:loading:start", timeout de 100ms
 *
 * 📋 ÍNDICE DE FUNÇÕES (3 funções principais):
 *
 * ┌─ FUNÇÕES DE LOADING STATE ─────────────────────────────────────────────────┐
 * │ 1. ativarBotaoLoading($btn)                                                 │
 * │    → Ativa estado de loading no botão                                      │
 * │    → Desabilita botão, mostra spinner, troca texto, cursor "wait"          │
 * │    → Lê data-loading-text ou usa "Aguarde..." como padrão                  │
 * │                                                                             │
 * │ 2. restaurarBotaoLoading($btn, textoOriginal)                               │
 * │    → Restaura botão ao estado original                                     │
 * │    → Habilita botão, esconde spinner, restaura texto, cursor "default"     │
 * │                                                                             │
 * │ 3. Click event handler (.btn-loading)                                       │
 * │    → Event delegation para todos os .btn-loading                           │
 * │    → Captura texto original, ativa loading                                 │
 * │    → Dispara evento customizado "btn:loading:start" com callback done()    │
 * │    → Timeout de 100ms para garantir atualização visual                     │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 ESTRUTURA HTML ESPERADA:
 * <button class="btn btn-loading" data-loading-text="Processando...">
 *   <span class="spinner-border spinner-border-sm d-none" role="status"></span>
 *   <span class="btn-text">Enviar</span>
 * </button>
 *
 * 🔄 EVENTO CUSTOMIZADO DISPARADO:
 * - "btn:loading:start" - Disparado após ativar loading (delay 100ms)
 *   Parâmetros: [$btn (jQuery), textoOriginal (string), done (function)]
 *   Exemplo de uso:
 *   $(document).on("btn:loading:start", function(e, $btn, textoOriginal, done) {
 *     // Fazer operação assíncrona
 *     $.ajax(...).always(() => done());
 *   });
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Usa event delegation $(document).on() para suportar botões dinâmicos
 * - Callback done() restaura botão automaticamente
 * - Cursor "wait" global em document.body durante loading
 * - Spinner Bootstrap 5 (.spinner-border) com .d-none toggle
 **************************************************************************************** */

(function () {
    function ativarBotaoLoading($btn) {
        try {
            const $spinner = $btn.find(".spinner-border");
            const $btnText = $btn.find(".btn-text");
            const novoTexto = $btn.data("loading-text") || "Aguarde...";

            $btn.prop("disabled", true);
            $spinner.removeClass("d-none");
            $btnText.text(novoTexto);
            document.body.style.cursor = "wait";
        } catch (erro) {
            console.error('Erro em ativarBotaoLoading:', erro);
            Alerta.TratamentoErroComLinha('botao-loading.js', 'ativarBotaoLoading', erro);
        }
    }

    function restaurarBotaoLoading($btn, textoOriginal) {
        try {
            const $spinner = $btn.find(".spinner-border");
            const $btnText = $btn.find(".btn-text");

            $btn.prop("disabled", false);
            $spinner.addClass("d-none");
            $btnText.text(textoOriginal);
            document.body.style.cursor = "default";
        } catch (erro) {
            console.error('Erro em restaurarBotaoLoading:', erro);
            Alerta.TratamentoErroComLinha('botao-loading.js', 'restaurarBotaoLoading', erro);
        }
    }

    // Ativação automática para qualquer botão com .btn-loading
    $(document).on("click", ".btn-loading", function (e) {
        try {
            const $btn = $(this);
            const textoOriginal = $btn.find(".btn-text").text();

            ativarBotaoLoading($btn);

            setTimeout(() => {
                try {
                    $btn.trigger("btn:loading:start", [$btn, textoOriginal, function done() {
                        restaurarBotaoLoading($btn, textoOriginal);
                    }]);
                } catch (erro) {
                    console.error('Erro em setTimeout callback:', erro);
                    Alerta.TratamentoErroComLinha('botao-loading.js', 'click.setTimeout', erro);
                    restaurarBotaoLoading($btn, textoOriginal);
                }
            }, 100);
        } catch (erro) {
            console.error('Erro em click handler .btn-loading:', erro);
            Alerta.TratamentoErroComLinha('botao-loading.js', 'click.btn-loading', erro);
        }
    });
})();
