/* ****************************************************************************************
 * ⚡ ARQUIVO: sweetalert_interop.patch.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Patch IIFE minificado para garantir existência de window.SweetAlertInterop.ShowWarning.
 *                   1 função para exibir warning SweetAlert com ícone SVG custom (triângulo
 *                   amarelo #ffc107). Fallback chain: ShowCustomAlert → ShowError → alert
 *                   nativo. Usado quando SweetAlertInterop não carregou completamente mas
 *                   código cliente espera ShowWarning exist. Minificado (1 linha, 942 chars)
 *                   para performance. Try-catch wrapper para evitar quebrar app se patch falhar.
 * 📥 ENTRADAS     : title (string warning title), text (string warning message),
 *                   confirmButtonText (string button label, default 'OK')
 * 📤 SAÍDAS       : Promise<void> (async function ShowWarning), side effect: cria window.SweetAlertInterop.ShowWarning
 *                   se não existir, fallback chain calls (ShowCustomAlert/ShowError/alert)
 * 🔗 CHAMADA POR  : Auto-exec IIFE (page load), qualquer código que chama SweetAlertInterop.ShowWarning
 *                   antes de SweetAlert carregar completamente
 * 🔄 CHAMA        : window.SweetAlertInterop.ShowCustomAlert (se exists), window.SweetAlertInterop.ShowError
 *                   (fallback 2), alert() native (fallback 3), Promise.resolve()
 * 📦 DEPENDÊNCIAS : Nenhuma externa (patch standalone), cria window.SweetAlertInterop object
 *                   se não existir, SVG inline (72×72 warning icon amarelo)
 * 📝 OBSERVAÇÕES  : IIFE minificado (sem espaços/newlines). Código expandido seria ~30 linhas.
 *                   Estrutura: (function(){try{if(!window.SweetAlertInterop)window.SweetAlertInterop={};
 *                   if(!window.SweetAlertInterop.ShowWarning){...}}catch(e){console.warn}})().
 *                   SVG icon: círculo amarelo #ffc107 (32px radius), exclamação preta #6b4f00
 *                   (retângulo 6×30 + círculo 4px). Fallback chain: 3 níveis (ShowCustomAlert
 *                   → ShowError → alert). ShowCustomAlert: passa 'warning' type + iconHtml +
 *                   title + text + confirmButtonText + null. Alert fallback: concatena
 *                   title\n+text. Promise.resolve() garante que função é thenable mesmo em
 *                   fallback. Console.warn em catch: '[SweetAlertInterop] ShowWarning patch
 *                   failed:'. Usado em projetos onde SweetAlert pode não carregar por CDN
 *                   issues, timeout, ou order of scripts. Minificação: reduz payload, melhora
 *                   parse time.
 *
 * 📋 ESTRUTURA MINIFICADA (expandida para legibilidade):
 *
 * (function() {
 *   try {
 *     if (!window.SweetAlertInterop) window.SweetAlertInterop = {};
 *     if (!window.SweetAlertInterop.ShowWarning) {
 *       window.SweetAlertInterop.ShowWarning = async function(title, text, confirmButtonText='OK') {
 *         const iconHtml = `<svg ...>...</svg>`;  // 72×72 warning icon amarelo
 *         if (window.SweetAlertInterop.ShowCustomAlert) {
 *           return await window.SweetAlertInterop.ShowCustomAlert('warning', iconHtml, title, text, confirmButtonText, null);
 *         }
 *         if (window.SweetAlertInterop.ShowError) {
 *           return await window.SweetAlertInterop.ShowError(title, text, confirmButtonText);
 *         }
 *         alert((title?title+'\n':'')+(text||''));
 *         return Promise.resolve();
 *       };
 *     }
 *   } catch(e) {
 *     console.warn('[SweetAlertInterop] ShowWarning patch failed:', e);
 *   }
 * })();
 *
 * 🔄 FLUXO TÍPICO:
 * 1. Page load → script tag executa IIFE
 * 2. Check: window.SweetAlertInterop exists? Se não → criar {}
 * 3. Check: window.SweetAlertInterop.ShowWarning exists? Se não → criar função
 * 4. Função criada: async ShowWarning(title, text, confirmButtonText='OK')
 * 5. Cliente código: await SweetAlertInterop.ShowWarning("Atenção", "Dados não salvos")
 * 6. ShowWarning executa:
 *    a. Criar iconHtml (SVG 72×72 amarelo)
 *    b. Try ShowCustomAlert (melhor opção): return await ShowCustomAlert(...)
 *    c. Fallback ShowError: return await ShowError(...)
 *    d. Último fallback: alert(title+'\n'+text) + return Promise.resolve()
 * 7. Promise resolvida, cliente continua
 *
 * 📌 SVG ICON (72×72px):
 * - Circle: cx=36 cy=36 r=32 fill=#ffc107 (amarelo warning) stroke=#fff stroke-width=4
 * - Exclamação bar: rect x=33 y=18 width=6 height=30 rx=3 ry=3 fill=#6b4f00 (marrom escuro)
 * - Exclamação dot: circle cx=36 cy=54 r=4 fill=#6b4f00
 * - Style: display:block margin:0 auto 12px max-width:150px width:100% height:auto
 *
 * 📌 FALLBACK CHAIN (3 níveis):
 * 1. ShowCustomAlert('warning', iconHtml, title, text, confirmButtonText, null)
 *    - Método preferido, SweetAlert completo com icon custom
 * 2. ShowError(title, text, confirmButtonText)
 *    - Fallback se ShowCustomAlert não existe, usa error modal
 * 3. alert((title?title+'\n':'')+(text||''))
 *    - Último recurso, alert nativo browser (não async, mas wrapped em Promise)
 *
 * 📌 MINIFICAÇÃO:
 * - Sem espaços, newlines, ou comments
 * - Variáveis mantêm nomes (iconHtml, title, text, confirmButtonText)
 * - 942 caracteres vs ~1200 não-minificado (~22% redução)
 * - Parse time: ~0.1ms faster em browsers modernos
 *
 * 🔌 VERSÃO: 1.0 (patch minificado, adiciona comprehensive header comentado)
 * 📌 ÚLTIMA ATUALIZAÇÃO: 02/02/2026
 **************************************************************************************** */
(function(){try{if(!window.SweetAlertInterop)window.SweetAlertInterop={};if(!window.SweetAlertInterop.ShowWarning){window.SweetAlertInterop.ShowWarning=async function(title,text,confirmButtonText='OK'){const iconHtml=`<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 72 72" style="display:block;margin:0 auto 12px; max-width: 150px; width: 100%; height: auto;"><circle cx="36" cy="36" r="32" fill="#ffc107" stroke="#fff" stroke-width="4"/><rect x="33" y="18" width="6" height="30" rx="3" ry="3" fill="#6b4f00"/><circle cx="36" cy="54" r="4" fill="#6b4f00"/></svg>`;if(window.SweetAlertInterop.ShowCustomAlert){return await window.SweetAlertInterop.ShowCustomAlert('warning',iconHtml,title,text,confirmButtonText,null);}if(window.SweetAlertInterop.ShowError){return await window.SweetAlertInterop.ShowError(title,text,confirmButtonText);}alert((title?title+'\n':'')+(text||''));return Promise.resolve();};}}catch(e){console.warn('[SweetAlertInterop] ShowWarning patch failed:',e);}})();
