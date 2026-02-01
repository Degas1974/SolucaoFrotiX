/* ****************************************************************************************
 * ⚡ ARQUIVO: ftx-datatable-style.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Aplica estilos padrão FrotiX aos headers de DataTables (thead e th).
 *                   DataTables normais usam cor azul padrão (#4a6fa5). DataTables dentro
 *                   de modais Bootstrap usam cor 20% mais clara que o header do modal.
 *                   Sistema automático com MutationObserver para detectar novos DataTables.
 * 📥 ENTRADAS     : Elementos DOM (thead, th), classes de header modal, eventos DOM
 * 📤 SAÍDAS       : Estilos CSS inline aplicados aos headers (background, color, font, etc.)
 * 🔗 CHAMADA POR  : DOMContentLoaded (automático), window load (automático),
 *                   MutationObserver (automático), código manual (window.ftxAplicarEstiloDataTable)
 * 🔄 CHAMA        : querySelectorAll, closest, querySelector, window.getComputedStyle,
 *                   style.setProperty, MutationObserver
 * 📦 DEPENDÊNCIAS : Vanilla JavaScript (sem jQuery), DOM API, MutationObserver API
 * 📝 OBSERVAÇÕES  : IIFE auto-executável, MutationObserver para detecção automática de novos
 *                   DataTables, mapeamento de 13 cores de headers modais (modal-header-azul,
 *                   ftx-modal-header-terracota, etc.), conversão RGB↔HSL↔Hex, lightness +20%,
 *                   !important em todos os estilos para sobrescrever DataTables defaults
 *
 * 📋 ÍNDICE DE FUNÇÕES (7 funções + 4 event listeners + 1 MutationObserver):
 *
 * ┌─ FUNÇÕES DE CONVERSÃO DE COR (RGB↔HSL↔Hex) ────────────────────────────────────┐
 * │ 1. rgbParaHsl(r, g, b)                                                           │
 * │    → Converte RGB (0-255) para HSL (h: 0-360, s: 0-100, l: 0-100)              │
 * │    → Algoritmo: calcula max/min, delta, hue (switch case), saturation, lightness│
 * │    → Retorna { h, s, l }                                                        │
 * │                                                                                  │
 * │ 2. hslParaHex(h, s, l)                                                          │
 * │    → Converte HSL (h: 0-360, s: 0-100, l: 0-100) para Hex (#RRGGBB)            │
 * │    → Função interna hue2rgb(p, q, t) para cálculo RGB                           │
 * │    → Função interna toHex(x) para conversão decimal → hex (padded)             │
 * │    → Retorna string '#rrggbb'                                                   │
 * │                                                                                  │
 * │ 3. clarearCor(cor, percentual)                                                  │
 * │    → Clareia cor hex em percentual (0-100) via aumento de lightness HSL        │
 * │    → Fluxo: Hex → RGB → HSL → +percentual lightness → HSL → Hex               │
 * │    → Usado para gerar cor de DataTable (header modal + 20%)                    │
 * │    → Retorna cor hex clareada                                                   │
 * └──────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES DE DETECÇÃO DE MODAL E COR ────────────────────────────────────────────┐
 * │ 4. encontrarModalPai(elemento)                                                   │
 * │    → Busca modal Bootstrap pai do elemento via .closest('.modal')              │
 * │    → Retorna elemento modal ou null                                             │
 * │                                                                                  │
 * │ 5. obterCorHeaderModal(modal)                                                   │
 * │    → Extrai cor base do .modal-header dentro do modal                           │
 * │    → Estratégia 1: Verifica classes conhecidas no mapeamento coresHeadersModal │
 * │      (modal-header-azul, ftx-modal-header-terracota, etc.)                     │
 * │    → Estratégia 2: Fallback para window.getComputedStyle + regex RGB parsing   │
 * │    → Retorna cor hex (#RRGGBB) ou null                                          │
 * └──────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES DE APLICAÇÃO DE ESTILO ────────────────────────────────────────────────┐
 * │ 6. aplicarEstilo(el, cor)                                                       │
 * │    → Aplica estilos inline ao elemento (thead ou th)                            │
 * │    → Propriedades: background, background-color, background-image: none,       │
 * │      color: #fff, font-family: Outfit, font-weight: 600, text-transform: uppercase,│
 * │      font-size: 0.82rem, letter-spacing: 0.3px                                  │
 * │    → Usa setProperty(..., 'important') para sobrescrever DataTables defaults   │
 * │                                                                                  │
 * │ 7. aplicarEstiloHeader()                                                        │
 * │    → Função PRINCIPAL que aplica estilo a todos thead e thead th               │
 * │    → Lógica: querySelectorAll('thead, thead th'), para cada elemento:          │
 * │      - Verifica se está dentro de modal (encontrarModalPai)                    │
 * │      - Se modal: obterCorHeaderModal → clarearCor(+20%) → aplicarEstilo        │
 * │      - Se fora de modal: corPadrao (#4a6fa5) → aplicarEstilo                   │
 * │    → Chamada por: DOMContentLoaded, window load, setTimeout(500ms), MutationObserver│
 * └──────────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ EVENT LISTENERS E OBSERVERS ────────────────────────────────────────────────────┐
 * │ 8. DOMContentLoaded listener (condicional)                                       │
 * │    → Se document.readyState === 'loading': adiciona listener                    │
 * │    → Else: chama aplicarEstiloHeader() imediatamente                            │
 * │                                                                                  │
 * │ 9. window load listener                                                         │
 * │    → Chama aplicarEstiloHeader() após window load                               │
 * │    → setTimeout(aplicarEstiloHeader, 500ms) para garantir DataTables renderizado│
 * │                                                                                  │
 * │ 10. MutationObserver callback                                                   │
 * │    → Observa document.body (childList: true, subtree: true)                     │
 * │    → Quando addedNodes.length > 0: chama aplicarEstiloHeader()                  │
 * │    → Detecta novos DataTables adicionados dinamicamente (AJAX, modais, etc.)   │
 * │                                                                                  │
 * │ 11. DOMContentLoaded listener para observer (condicional)                       │
 * │    → Se document.body não existe ainda: espera DOMContentLoaded                 │
 * │    → Então inicia observer.observe(document.body, ...)                          │
 * └──────────────────────────────────────────────────────────────────────────────────┘
 *
 * 🎨 CORES DE HEADERS MODAIS (coresHeadersModal - 13 cores mapeadas):
 * - modal-header-dinheiro: #3d4a3d (verde militar escuro)
 * - modal-header-azul: #325d88 (azul padrão FrotiX)
 * - modal-header-verde: #4A803B (verde)
 * - modal-header-vinho: #6b1f1f (vinho)
 * - modal-header-terracota: #a0522d (terracota)
 * - modal-header-laranja: #cc5500 (laranja)
 * - modal-header-roxo: #6B2FA2 (roxo)
 * - ftx-modal-header: #2d5a87 (azul padrão modal FrotiX)
 * - ftx-modal-header-azul: #2d5a87
 * - ftx-modal-header-terracota: #b45a3c
 * - ftx-modal-header-verde: #2e7d32
 * - ftx-modal-header-vinho: #722f37
 * - ftx-modal-header-laranja: #e65100
 * - ftx-modal-header-roxo: #5e35b1
 * - ftx-modal-header-cinza: #455a64
 *
 * 📌 COR PADRÃO (DataTables fora de modal): #4a6fa5 (azul padrão FrotiX)
 *
 * 📌 API PÚBLICA (window):
 * - window.ftxAplicarEstiloDataTable() - aplica estilo manualmente
 * - window.ftxClarearCor(cor, percentual) - clareia cor hex
 * - window.ftxCoresHeadersModal - objeto com mapeamento de cores (pode ser modificado)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - IIFE pattern: (function() { ... })() - auto-executável
 * - MutationObserver: detecção automática de novos DataTables (AJAX, modais dinâmicos)
 * - !important: todos os estilos usam !important para sobrescrever DataTables defaults
 * - 3 momentos de aplicação: DOMContentLoaded, window load, window load + 500ms
 * - Conversão de cor: Hex → RGB → HSL → +20% lightness → HSL → Hex
 * - Fallback: se não encontrar cor de modal, usa corPadrao (#4a6fa5)
 * - Font: 'Outfit', sans-serif (Google Font)
 * - Font-size: 0.82rem, font-weight: 600, text-transform: uppercase, letter-spacing: 0.3px
 *
 * 🔌 VERSÃO: 1.0.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 08/01/2026
 * 📌 DOCUMENTAÇÃO EXTERNA: Documentacao/JavaScript/ftx-datatable-style.js.md
 **************************************************************************************** */
(function() {
    'use strict';

    // Cor padrão para DataTables fora de modais
    var corPadrao = '#4a6fa5';

    // Mapeamento de classes de header de modal conhecidas para suas cores base
    // DataTable interno = 20% mais claro que o header do modal
    var coresHeadersModal = {
        'modal-header-dinheiro': '#3d4a3d',      // Verde militar escuro → DataTable: #5A6B5A
        'modal-header-azul': '#325d88',           // Azul padrão FrotiX → DataTable: #4A7BA6
        'modal-header-verde': '#4A803B',          // Verde → DataTable: #6E9962
        'modal-header-vinho': '#6b1f1f',          // Vinho → DataTable: #8F4343
        'modal-header-terracota': '#a0522d',      // Terracota → DataTable: #C47651
        'modal-header-laranja': '#cc5500',        // Laranja → DataTable: #F07924
        'modal-header-roxo': '#6B2FA2',           // Roxo → DataTable: #8F53C6
        'ftx-modal-header': '#2d5a87',            // Azul padrão modal FrotiX
        'ftx-modal-header-azul': '#2d5a87',       // Azul (padrão FrotiX)
        'ftx-modal-header-terracota': '#b45a3c',  // Terracota (padrão FrotiX)
        'ftx-modal-header-verde': '#2e7d32',      // Verde (padrão FrotiX)
        'ftx-modal-header-vinho': '#722f37',      // Vinho (padrão FrotiX)
        'ftx-modal-header-laranja': '#e65100',    // Laranja (padrão FrotiX)
        'ftx-modal-header-roxo': '#5e35b1',       // Roxo (padrão FrotiX)
        'ftx-modal-header-cinza': '#455a64'       // Cinza (padrão FrotiX)
    };

    /**
     * Converte cor RGB para HSL
     */
    function rgbParaHsl(r, g, b) {
        try {
            r /= 255; g /= 255; b /= 255;
            var max = Math.max(r, g, b), min = Math.min(r, g, b);
            var h, s, l = (max + min) / 2;

            if (max === min) {
                h = s = 0;
            } else {
                var d = max - min;
                s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
                switch (max) {
                    case r: h = ((g - b) / d + (g < b ? 6 : 0)) / 6; break;
                    case g: h = ((b - r) / d + 2) / 6; break;
                    case b: h = ((r - g) / d + 4) / 6; break;
                }
            }
            return { h: h * 360, s: s * 100, l: l * 100 };
        } catch (erro) {
            console.error('Erro em rgbParaHsl:', erro);
            return { h: 0, s: 0, l: 50 };
        }
    }

    /**
     * Converte HSL para cor hexadecimal
     */
    function hslParaHex(h, s, l) {
        try {
            h /= 360; s /= 100; l /= 100;
            var r, g, b;

            if (s === 0) {
                r = g = b = l;
            } else {
                var hue2rgb = function(p, q, t) {
                    if (t < 0) t += 1;
                    if (t > 1) t -= 1;
                    if (t < 1/6) return p + (q - p) * 6 * t;
                    if (t < 1/2) return q;
                    if (t < 2/3) return p + (q - p) * (2/3 - t) * 6;
                    return p;
                };
                var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
                var p = 2 * l - q;
                r = hue2rgb(p, q, h + 1/3);
                g = hue2rgb(p, q, h);
                b = hue2rgb(p, q, h - 1/3);
            }

            var toHex = function(x) {
                var hex = Math.round(x * 255).toString(16);
                return hex.length === 1 ? '0' + hex : hex;
            };

            return '#' + toHex(r) + toHex(g) + toHex(b);
        } catch (erro) {
            console.error('Erro em hslParaHex:', erro);
            return '#000000';
        }
    }

    /**
     * Clareia uma cor em percentual (0-100)
     */
    function clarearCor(cor, percentual) {
        try {
            // Remove # se existir
            cor = cor.replace('#', '');

            // Converte hex para RGB
            var r = parseInt(cor.substring(0, 2), 16);
            var g = parseInt(cor.substring(2, 4), 16);
            var b = parseInt(cor.substring(4, 6), 16);

            // Converte para HSL
            var hsl = rgbParaHsl(r, g, b);

            // Aumenta a luminosidade em percentual (limitado a 100)
            hsl.l = Math.min(100, hsl.l + percentual);

            // Retorna em hex
            return hslParaHex(hsl.h, hsl.s, hsl.l);
        } catch (erro) {
            console.error('Erro em clarearCor:', erro);
            return cor;
        }
    }

    /**
     * Encontra o modal pai de um elemento
     */
    function encontrarModalPai(elemento) {
        try {
            if (!elemento || !elemento.closest) return null;
            return elemento.closest('.modal');
        } catch (erro) {
            console.error('Erro em encontrarModalPai:', erro);
            return null;
        }
    }

    /**
     * Encontra o header do modal e retorna sua cor base
     * Usa mapeamento de classes conhecidas
     */
    function obterCorHeaderModal(modal) {
        try {
            if (!modal) return null;

            // Procura pelo modal-header
            var header = modal.querySelector('.modal-header');
            if (!header) return null;

            // Verifica classes conhecidas no mapeamento
            var classes = header.className.split(' ');
            for (var i = 0; i < classes.length; i++) {
                var classe = classes[i].trim();
                if (coresHeadersModal[classe]) {
                    return coresHeadersModal[classe];
                }
            }

            // Fallback: tenta extrair cor computada
            try {
                var style = window.getComputedStyle(header);
                var bg = style.backgroundColor;
                if (bg && bg !== 'transparent' && bg !== 'rgba(0, 0, 0, 0)') {
                    var match = bg.match(/rgba?\((\d+),\s*(\d+),\s*(\d+)/);
                    if (match) {
                        var r = parseInt(match[1]).toString(16).padStart(2, '0');
                        var g = parseInt(match[2]).toString(16).padStart(2, '0');
                        var b = parseInt(match[3]).toString(16).padStart(2, '0');
                        var hex = '#' + r + g + b;
                        if (hex !== '#000000' && hex !== '#ffffff') {
                            return hex;
                        }
                    }
                }
            } catch (e) {
                // Ignora erros do getComputedStyle
            }

            return null;
        } catch (erro) {
            console.error('Erro em obterCorHeaderModal:', erro);
            return null;
        }
    }

    /**
     * Aplica estilo a um elemento thead ou th
     */
    function aplicarEstilo(el, cor) {
        try {
            el.style.setProperty('background', cor, 'important');
            el.style.setProperty('background-color', cor, 'important');
            el.style.setProperty('background-image', 'none', 'important');
            el.style.setProperty('color', '#ffffff', 'important');
            el.style.setProperty('font-family', "'Outfit', sans-serif", 'important');
            el.style.setProperty('font-weight', '600', 'important');
            el.style.setProperty('text-transform', 'uppercase', 'important');
            el.style.setProperty('font-size', '0.82rem', 'important');
            el.style.setProperty('letter-spacing', '0.3px', 'important');
        } catch (erro) {
            console.error('Erro em aplicarEstilo:', erro);
        }
    }

    /**
     * Função principal que aplica o estilo aos headers
     */
    function aplicarEstiloHeader() {
        try {
            document.querySelectorAll('thead, thead th').forEach(function(el) {
                try {
                    // Verifica se está dentro de um modal
                    var modal = encontrarModalPai(el);

                    if (modal) {
                        // Está dentro de modal - usa cor 20% mais clara que o header do modal
                        var corModal = obterCorHeaderModal(modal);

                        if (corModal) {
                            var corClara = clarearCor(corModal, 20);
                            aplicarEstilo(el, corClara);
                        } else {
                            // Se não encontrou cor do modal, usa padrão
                            aplicarEstilo(el, corPadrao);
                        }
                    } else {
                        // Fora de modal - usa cor padrão azul
                        aplicarEstilo(el, corPadrao);
                    }
                } catch (erroEl) {
                    console.error('Erro ao processar elemento em aplicarEstiloHeader:', erroEl);
                }
            });
        } catch (erro) {
            console.error('Erro em aplicarEstiloHeader:', erro);
        }
    }

    // Aplicar quando DOM estiver pronto
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', function() {
            try {
                aplicarEstiloHeader();
            } catch (erro) {
                console.error('Erro no DOMContentLoaded listener:', erro);
            }
        });
    } else {
        aplicarEstiloHeader();
    }

    // Aplicar após window load (quando DataTables terminar)
    window.addEventListener('load', function() {
        try {
            aplicarEstiloHeader();
            // Aplicar novamente após 500ms (garante que DataTables terminou)
            setTimeout(function() {
                try {
                    aplicarEstiloHeader();
                } catch (erro) {
                    console.error('Erro no setTimeout de window load:', erro);
                }
            }, 500);
        } catch (erro) {
            console.error('Erro no window load listener:', erro);
        }
    });

    // Observer para detectar quando DataTables adiciona elementos
    var observer = new MutationObserver(function(mutations) {
        try {
            mutations.forEach(function(mutation) {
                try {
                    if (mutation.addedNodes.length > 0) {
                        aplicarEstiloHeader();
                    }
                } catch (erroMutation) {
                    console.error('Erro ao processar mutation:', erroMutation);
                }
            });
        } catch (erro) {
            console.error('Erro no MutationObserver callback:', erro);
        }
    });

    // Observar mudanças no body
    if (document.body) {
        try {
            observer.observe(document.body, { childList: true, subtree: true });
        } catch (erro) {
            console.error('Erro ao iniciar MutationObserver:', erro);
        }
    } else {
        document.addEventListener('DOMContentLoaded', function() {
            try {
                observer.observe(document.body, { childList: true, subtree: true });
            } catch (erro) {
                console.error('Erro ao iniciar MutationObserver no DOMContentLoaded:', erro);
            }
        });
    }

    // Expor funções globalmente para uso manual se necessário
    window.ftxAplicarEstiloDataTable = aplicarEstiloHeader;
    window.ftxClarearCor = clarearCor;
    window.ftxCoresHeadersModal = coresHeadersModal; // Permite adicionar novas cores em runtime
})();
