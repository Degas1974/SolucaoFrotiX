/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║     ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗    ║
 * ║     ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗   ║
 * ║     █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║   ║
 * ║     ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║   ║
 * ║     ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝   ║
 * ║     ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝    ║
 * ║                                                                          ║
 * ║                      SOLUÇÃO FROTIX - GESTÃO DE FROTAS                   ║
 * ║                                                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: botao-loading.js                                             ║
 * ║ 📍 LOCAL: wwwroot/js/                                                    ║
 * ║ 📋 VERSÃO: 1.0                                                           ║
 * ║ 📅 ATUALIZAÇÃO: 23/01/2026                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                       ║
 * ║    Utilitário para feedback visual em botões durante operações async.    ║
 * ║    • Exibe spinner e texto "Aguarde..." no botão                         ║
 * ║    • Desabilita botão para evitar duplo clique                           ║
 * ║    • Muda cursor para wait durante processamento                         ║
 * ║    • Restaura estado original após conclusão                             ║
 * ║                                                                          ║
 * ║ 🔗 RELEVÂNCIA: Média (UX - Feedback Visual)                              ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

(function () {
    function ativarBotaoLoading($btn) {
        const $spinner = $btn.find('.spinner-border');
        const $btnText = $btn.find('.btn-text');
        const novoTexto = $btn.data('loading-text') || 'Aguarde...';

        $btn.prop('disabled', true);
        $spinner.removeClass('d-none');
        $btnText.text(novoTexto);
        document.body.style.cursor = 'wait';
    }

    function restaurarBotaoLoading($btn, textoOriginal) {
        const $spinner = $btn.find('.spinner-border');
        const $btnText = $btn.find('.btn-text');

        $btn.prop('disabled', false);
        $spinner.addClass('d-none');
        $btnText.text(textoOriginal);
        document.body.style.cursor = 'default';
    }

    // Ativação automática para qualquer botão com .btn-loading
    $(document).on('click', '.btn-loading', function (e) {
        const $btn = $(this);
        const textoOriginal = $btn.find('.btn-text').text();

        ativarBotaoLoading($btn);

        setTimeout(() => {
            $btn.trigger('btn:loading:start', [
                $btn,
                textoOriginal,
                function done() {
                    restaurarBotaoLoading($btn, textoOriginal);
                },
            ]);
        }, 100);
    });
})();
