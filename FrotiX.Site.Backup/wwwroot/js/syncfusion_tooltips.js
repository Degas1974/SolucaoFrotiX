/* ****************************************************************************************
 * ⚡ ARQUIVO: syncfusion_tooltips.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciador GLOBAL de tooltips usando Syncfusion EJ2. Substitui
 *                   tooltips Bootstrap por Syncfusion em todo o sistema, com suporte
 *                   a elementos dinâmicos e auto-refresh via MutationObserver.
 * 📥 ENTRADAS     : Elementos HTML com atributo [data-ejtip], eventos hover/mouse
 * 📤 SAÍDAS       : Tooltips visuais Syncfusion estilo dark (#4a6b8a), sem setas
 * 🔗 CHAMADA POR  : Auto-execução IIFE no carregamento da página (_Layout.cshtml)
 * 🔄 CHAMA        : Syncfusion EJ2 Tooltip (ej.popups.Tooltip), MutationObserver API
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 (ej.popups.Tooltip), Bootstrap 5 (para limpeza)
 * 📝 OBSERVAÇÕES  : Tooltips fecham automaticamente após 2 segundos, suportam HTML
 *                   (quebras de linha com \n → <br>), sem setas visuais (showTipPointer: false)
 *
 * 📋 ÍNDICE DE FUNÇÕES (2 funções principais + 4 callbacks Syncfusion):
 *
 * ┌─ FUNÇÕES PRINCIPAIS ───────────────────────────────────────────────────────────┐
 * │ 1. initializeTooltip()                                                          │
 * │    → Inicializa tooltip global Syncfusion, remove tooltips Bootstrap           │
 * │    → Aguarda carregamento do Syncfusion (retry 500ms)                          │
 * │    → Cria instância global window.ejTooltip                                    │
 * │    → Adiciona CSS customizado dark (#4a6b8a)                                   │
 * │                                                                                 │
 * │ 2. refreshTooltips() (window.refreshTooltips)                                  │
 * │    → Atualiza tooltips para elementos dinâmicos                                │
 * │    → Remove atributos Bootstrap de novos elementos                             │
 * │    → Chama ejTooltip.refresh()                                                 │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ CALLBACKS SYNCFUSION ─────────────────────────────────────────────────────────┐
 * │ 3. content(args)                                                                │
 * │    → Retorna texto do tooltip via data-ejtip                                   │
 * │    → Converte \n para <br> (suporte HTML)                                      │
 * │                                                                                 │
 * │ 4. beforeOpen(args)                                                             │
 * │    → Define conteúdo antes de abrir tooltip                                    │
 * │    → Converte \n para <br>, fallback "Sem descrição"                           │
 * │                                                                                 │
 * │ 5. afterOpen(args)                                                              │
 * │    → Configura auto-close após 2 segundos                                      │
 * │    → Armazena timeout ID em data-close-timeout                                 │
 * │                                                                                 │
 * │ 6. beforeClose(args)                                                            │
 * │    → Limpa timeout de auto-close                                               │
 * └─────────────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 AUTO-EXECUTÁVEL:
 * - IIFE auto-executa no carregamento
 * - MutationObserver detecta elementos dinâmicos (DataTables, modals, AJAX)
 * - DOMContentLoaded + fallback para execução imediata
 *
 * 🎨 ESTILO VISUAL:
 * - Background: #4a6b8a (azul acinzentado escuro)
 * - Texto: #ffffff (branco)
 * - Border: #7a8a9a, border-radius: 8px
 * - Shadow: 0 2px 8px rgba(0,0,0,0.15)
 * - SEM setas (showTipPointer: false)
 *
 * 📌 REFERÊNCIA EXTERNA: Documentacao/JavaScript/syncfusion_tooltips.js.md
 **************************************************************************************** */

// syncfusion_tooltips.js - Tooltip GLOBAL para todos os elementos com data-ejtip
(function ()
{
    function initializeTooltip()
    {
        try
        {
            // Verifica se o Syncfusion está carregado
            if (typeof ej === 'undefined' || !ej.popups || !ej.popups.Tooltip)
            {
                console.warn('Syncfusion não carregado. Tentando novamente em 500ms...');
                setTimeout(initializeTooltip, 500);
                return;
            }

        // Desabilita tooltips do Bootstrap 5 usando try-catch
        document.querySelectorAll('[data-ejtip]').forEach(function (el)
        {
            try
            {
                el.removeAttribute('data-bs-toggle');
                el.removeAttribute('data-bs-original-title');
                el.removeAttribute('title');

                if (window.bootstrap?.Tooltip?.getInstance)
                {
                    const bsTooltip = window.bootstrap.Tooltip.getInstance(el);
                    bsTooltip?.dispose();
                }
            } catch (e)
            {
                console.warn('Erro ao limpar tooltip Bootstrap:', e);
            }
        });

        // Destrói instância anterior se existir
        if (window.ejTooltip)
        {
            try
            {
                window.ejTooltip.destroy();
            } catch (e)
            {
                console.warn('Erro ao destruir tooltip anterior:', e);
            }
        }

        // Adiciona CSS customizado para o tooltip (COM REMOÇÃO DE SETAS)
        if (!document.getElementById('custom-tooltip-style'))
        {
            const style = document.createElement('style');
            style.id = 'custom-tooltip-style';
            style.textContent = `
                .e-tooltip-wrap {
                    background-color: #4a6b8a !important;
                    color: #ffffff !important;
                    border: 1px solid #7a8a9a !important;
                    border-radius: 8px !important;
                    padding: 8px 12px !important;
                    font-size: 13px !important;
                    box-shadow: 0 2px 8px rgba(0,0,0,0.15) !important;
                    z-index: 99999 !important;
                }
                .e-tooltip-wrap .e-tip-content {
                    color: #ffffff !important;
                    line-height: 1.4 !important;
                    display: block !important;
                    visibility: visible !important;
                    text-align: left !important;
                    white-space: normal !important;
                }
                .e-tooltip-wrap.e-popup {
                    background-color: #4a6b8a !important;
                }

                /* ===== REMOÇÃO DAS SETAS ===== */
                .e-tooltip-wrap .e-arrow-tip,
                .e-tooltip-wrap .e-arrow-tip-outer,
                .e-tooltip-wrap .e-arrow-tip-inner {
                    display: none !important;
                }
                .e-tooltip-wrap.e-tip-top {
                    margin-bottom: 0 !important;
                }
                .e-tooltip-wrap.e-tip-bottom {
                    margin-top: 0 !important;
                }
                /* (Opcional) Bootstrap tooltip */
                .tooltip .tooltip-arrow {
                    display: none !important;
                }
            `;
            document.head.appendChild(style);
        }

        // Cria nova instância GLOBAL com content como FUNÇÃO
        window.ejTooltip = new ej.popups.Tooltip({
            target: '[data-ejtip]',
            opensOn: 'Hover',
            position: 'TopCenter',
            showTipPointer: false, // ← DESATIVA A SETA PROGRAMATICAMENTE
            cssClass: 'custom-dark-tooltip',
            enableHtmlSanitizer: false, // ← PERMITE HTML (ex: <br> para quebra de linha)
            // CRÍTICO: content como função que retorna o texto
            content: function (args)
            {
                try
                {
                    let tooltipText = args.getAttribute('data-ejtip');
                    console.log('Tooltip text:', tooltipText);
                    // Converte \n para <br> para suportar quebras de linha
                    if (tooltipText) {
                        tooltipText = tooltipText.replace(/\n/g, '<br>');
                    }
                    return tooltipText || 'Sem descrição';
                }
                catch (erro)
                {
                    console.error('Erro em content callback:', erro);
                    return 'Erro ao carregar tooltip';
                }
            },
            beforeOpen: function (args)
            {
                try
                {
                    // Garante que o conteúdo seja definido antes de abrir
                    const target = args.target;
                    let tooltipText = target.getAttribute('data-ejtip');

                    if (tooltipText)
                    {
                        // Converte \n para <br> para suportar quebras de linha
                        tooltipText = tooltipText.replace(/\n/g, '<br>');
                        this.content = tooltipText;
                        console.log('Tooltip configurado com:', tooltipText);
                    } else
                    {
                        console.warn('Elemento sem data-ejtip:', target);
                        this.content = 'Sem descrição';
                    }
                }
                catch (erro)
                {
                    console.error('Erro em beforeOpen callback:', erro);
                    this.content = 'Erro ao carregar tooltip';
                }
            },
            afterOpen: function (args)
            {
                try
                {
                    // Força o fechamento após 2 segundos
                    const tooltipElement = args.element;
                    const closeTimeout = setTimeout(() =>
                    {
                        this.close();
                    }, 2000);

                    tooltipElement.setAttribute('data-close-timeout', closeTimeout);
                }
                catch (erro)
                {
                    console.error('Erro em afterOpen callback:', erro);
                }
            },
            beforeClose: function (args)
            {
                try
                {
                    const closeTimeout = args.element.getAttribute('data-close-timeout');
                    if (closeTimeout)
                    {
                        clearTimeout(parseInt(closeTimeout));
                        args.element.removeAttribute('data-close-timeout');
                    }
                }
                catch (erro)
                {
                    console.error('Erro em beforeClose callback:', erro);
                }
            }
        });

            window.ejTooltip.appendTo('body');
            console.log('✓ Tooltip GLOBAL Syncfusion inicializado (sem setas)');
        }
        catch (erro)
        {
            console.error('Erro em initializeTooltip:', erro);
            Alerta.TratamentoErroComLinha('syncfusion_tooltips.js', 'initializeTooltip', erro);
        }
    }

    // Refresher para elementos dinâmicos
    window.refreshTooltips = function ()
    {
        try
        {
            document.querySelectorAll('[data-ejtip]').forEach(function (el)
            {
                el.removeAttribute('data-bs-toggle');
                el.removeAttribute('data-bs-original-title');
                el.removeAttribute('title');
            });

            if (window.ejTooltip)
            {
                window.ejTooltip.refresh();
                console.log('✓ Tooltips atualizados');
            } else
            {
                console.warn('⚠ ejTooltip não está inicializado. Inicializando...');
                initializeTooltip();
            }
        }
        catch (erro)
        {
            console.error('Erro em refreshTooltips:', erro);
            Alerta.TratamentoErroComLinha('syncfusion_tooltips.js', 'refreshTooltips', erro);
        }
    };

    // Inicializa quando DOM estiver pronto
    if (document.readyState === 'loading')
    {
        document.addEventListener('DOMContentLoaded', initializeTooltip);
    } else
    {
        initializeTooltip();
    }

    // Observer para detectar elementos adicionados dinamicamente
    const observer = new MutationObserver(() =>
    {
        document.querySelectorAll('[data-ejtip]').forEach(function (el)
        {
            el.removeAttribute('data-bs-toggle');
            el.removeAttribute('data-bs-original-title');
            el.removeAttribute('title');
        });

        if (window.ejTooltip)
        {
            window.ejTooltip.refresh();
        }
    });

    if (document.readyState === 'loading')
    {
        document.addEventListener('DOMContentLoaded', () =>
        {
            observer.observe(document.body, {
                childList: true,
                subtree: true
            });
        });
    } else
    {
        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    }
})();
