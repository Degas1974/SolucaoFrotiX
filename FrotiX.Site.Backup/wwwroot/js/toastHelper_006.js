/* ****************************************************************************************
 * ⚡ ARQUIVO: toastHelper_006.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Helper para exibir toasts usando Syncfusion EJ2 Toast com estilos
 *                   customizados FrotiX. Cria instância única de toast e aplica classes
 *                   CSS personalizadas via evento beforeOpen.
 * 📥 ENTRADAS     : message (string), type (info|success|danger|warning), icon (HTML)
 * 📤 SAÍDAS       : Toast visual Syncfusion no canto superior direito da tela
 * 🔗 CHAMADA POR  : Código JavaScript geral, handlers de sucesso/erro em AJAX/fetch
 * 🔄 CHAMA        : ej.notifications.Toast (Syncfusion EJ2), DOM API (createElement)
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 Toast (ej.notifications.Toast), CSS custom-toast-*
 * 📝 OBSERVAÇÕES  : Singleton pattern (um único #syncfusion-toast no body), auto-close
 *                   em 4 segundos, animação ZoomIn/FadeOut, classes CSS dinâmicas
 *
 * 📋 FUNÇÃO PRINCIPAL:
 *
 * ┌─ showSyncfusionToast(message, type, icon) ────────────────────────────┐
 * │ → Exibe toast Syncfusion com estilo customizado                       │
 * │ → Cria div #syncfusion-toast se não existir (singleton)              │
 * │ → Instancia Toast com config: position TopRight, timeout 4s          │
 * │ → beforeOpen: remove classes antigas, adiciona custom-toast-{type}   │
 * │ → Suporta types: info, success, danger, warning                      │
 * │ → content: HTML com .toast-content > .toast-icon + .toast-text       │
 * └─────────────────────────────────────────────────────────────────────────┘
 *
 * 🎨 CLASSES CSS APLICADAS:
 * - custom-toast-info (azul)
 * - custom-toast-success (verde)
 * - custom-toast-danger (vermelho)
 * - custom-toast-warning (amarelo)
 *
 * 📝 USO:
 * showSyncfusionToast("Operação concluída!", "success", "✓");
 * showSyncfusionToast("Erro ao salvar", "danger", "✗");
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 02/02/2026
 **************************************************************************************** */

function showSyncfusionToast(message, type = "info", icon = "") {
    const toastId = "syncfusion-toast";

    if (!document.getElementById(toastId)) {
        const div = document.createElement("div");
        div.id = toastId;
        document.body.appendChild(div);
    }

    const toast = new ej.notifications.Toast({
        position: { X: 'Right', Y: 'Top' },
        timeOut: 4000,
        showCloseButton: true,
        animation: {
            show: { effect: 'ZoomIn', duration: 500 },
            hide: { effect: 'FadeOut', duration: 500 }
        },
        // Evento mágico
        beforeOpen: (args) => {
            const toastEl = args.element;

            toastEl.classList.remove(
                "custom-toast-success",
                "custom-toast-danger",
                "custom-toast-info",
                "custom-toast-warning"
            );

            toastEl.classList.add(`custom-toast-${type}`);
        }
    });

    toast.appendTo(`#${toastId}`);

    const content = `
        <div class="toast-content">
            <span class="toast-icon">${icon}</span>
            <span class="toast-text">${message}</span>
        </div>
    `;

    toast.show({
        content: content
    });
}
