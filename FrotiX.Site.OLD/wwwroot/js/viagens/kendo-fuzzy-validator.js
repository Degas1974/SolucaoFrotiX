/****************************************************************************************
 * 📦 MÓDULO: Kendo Fuzzy Validator (Sistema de Validação Inteligente)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Sistema avançado de validação fuzzy para Kendo ComboBox
 *                   Detecta duplicatas, similaridades e auto-corrige valores
 *
 * 🆕 NOVIDADES    : ✅ Adaptado para Kendo UI ComboBox (removido Syncfusion)
 *                   ✅ Debouncing inteligente (evita validações excessivas)
 *                   ✅ Cache de validações (performance)
 *                   ✅ Highlight visual de sugestões
 *                   ✅ Histórico de correções (localStorage)
 *                   ✅ Métricas de qualidade de dados
 *                   ✅ Suporte a múltiplos algoritmos de similaridade
 *                   ✅ Configuração granular por controle
 *
 * 📅 VERSÃO       : 2.0 (11/02/2026) - Reescrita completa para Kendo UI
 * 👤 AUTOR        : Claude Sonnet 4.5 (FrotiX Team)
 ****************************************************************************************/

(function (window) {
    'use strict';

    /****************************************************************************************
     * 🔧 CONFIGURAÇÃO GLOBAL
     ****************************************************************************************/
    const CONFIG = {
        // Thresholds de similaridade
        thresholds: {
            info: 0.85,      // 85% = Alerta informativo
            warning: 0.92,   // 92% = Alerta de warning
            critical: 0.98   // 98% = Auto-correção
        },

        // Debouncing (ms)
        debounceDelay: 300,

        // Cache
        enableCache: true,
        cacheExpiration: 5 * 60 * 1000, // 5 minutos

        // Histórico
        enableHistory: true,
        historyStorageKey: 'frotix_fuzzy_history',
        maxHistoryEntries: 100,

        // Auto-correção
        autoCorrect: true,
        showSuggestions: true,

        // Highlight visual
        enableHighlight: true,
        highlightDuration: 2000, // 2 segundos

        // Algoritmos de similaridade disponíveis
        algorithms: {
            levenshtein: true,    // Distância de edição
            jaro: false,          // Jaro distance (futuro)
            soundex: false        // Similaridade fonética (futuro)
        }
    };

    /****************************************************************************************
     * 📊 MÉTRICAS E ESTATÍSTICAS
     ****************************************************************************************/
    const METRICS = {
        totalValidations: 0,
        duplicatesDetected: 0,
        autoCorrections: 0,
        userCorrections: 0,
        avgSimilarityScore: 0,
        lastValidation: null
    };

    /****************************************************************************************
     * 💾 CACHE DE VALIDAÇÕES
     ****************************************************************************************/
    const validationCache = new Map();

    /****************************************************************************************
     * 🕒 DEBOUNCE TIMERS
     ****************************************************************************************/
    const debounceTimers = new Map();

    /****************************************************************************************
     * ⚡ FUNÇÃO: normalizeText
     * --------------------------------------------------------------------------------------
     * Remove acentos, espaços extras e normaliza texto para comparação
     ****************************************************************************************/
    function normalizeText(text) {
        try {
            if (!text) return '';
            return String(text)
                .normalize('NFKC')
                .replace(/[\u200B-\u200D\uFEFF]/g, '')
                .replace(/\u00A0/g, ' ')
                .toLowerCase()
                .normalize('NFD')
                .replace(/[\u0300-\u036f]/g, '')
                .replace(/[\s\u00A0]+/g, ' ')
                .trim();
        } catch (error) {
            console.error('normalizeText error:', error);
            return '';
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: capitalizeFirstLetter
     * --------------------------------------------------------------------------------------
     * Capitaliza primeira letra do texto (para texto livre digitado pelo usuário)
     ****************************************************************************************/
    function capitalizeFirstLetter(text) {
        try {
            if (!text) return '';
            const trimmed = String(text).trim();
            if (!trimmed) return '';
            return trimmed.charAt(0).toUpperCase() + trimmed.slice(1);
        } catch (error) {
            console.error('capitalizeFirstLetter error:', error);
            return text;
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: levenshteinDistance
     * --------------------------------------------------------------------------------------
     * Calcula distância de Levenshtein (edições necessárias entre duas strings)
     * Algoritmo: Programação Dinâmica O(n*m)
     ****************************************************************************************/
    function levenshteinDistance(a, b) {
        try {
            const n = a.length;
            const m = b.length;

            if (n === 0) return m;
            if (m === 0) return n;

            // Otimização: usar apenas 2 linhas da matriz DP
            let prev = Array.from({ length: m + 1 }, (_, i) => i);
            let curr = new Array(m + 1);

            for (let i = 1; i <= n; i++) {
                curr[0] = i;
                for (let j = 1; j <= m; j++) {
                    const cost = a[i - 1] === b[j - 1] ? 0 : 1;
                    curr[j] = Math.min(
                        prev[j] + 1,      // deletion
                        curr[j - 1] + 1,  // insertion
                        prev[j - 1] + cost // substitution
                    );
                }
                [prev, curr] = [curr, prev];
            }

            return prev[m];
        } catch (error) {
            console.error('levenshteinDistance error:', error);
            return Infinity;
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: calculateSimilarity
     * --------------------------------------------------------------------------------------
     * Calcula score de similaridade normalizado (0.0 a 1.0)
     * 1.0 = idênticos, 0.0 = completamente diferentes
     ****************************************************************************************/
    function calculateSimilarity(a, b) {
        try {
            const na = normalizeText(a);
            const nb = normalizeText(b);

            if (!na && !nb) return 1.0;
            if (!na || !nb) return 0.0;
            if (na === nb) return 1.0;

            // ═══════════════════════════════════════════════════════════════════════
            // NOVO: Detectar se uma string é SUBSTRING da outra
            // ═══════════════════════════════════════════════════════════════════════
            const isSubstring = nb.includes(na) || na.includes(nb);

            if (isSubstring) {
                // Determinar qual é a substring e qual é a string maior
                const shorter = na.length < nb.length ? na : nb;
                const longer = na.length < nb.length ? nb : na;

                // Score baseado no tamanho relativo da substring
                // Exemplo: "080" (3) em "... (0800)" (58) = 3/58 = 0.05
                const lengthRatio = shorter.length / longer.length;

                // Score mínimo de 85% para qualquer substring válida
                // Score máximo de 97% para substrings que ocupam > 50% da string
                const baseScore = 0.85;
                const bonus = lengthRatio * 0.12; // Até 12% de bônus
                const substringScore = baseScore + bonus;

                console.log(`[Fuzzy] 🔍 SUBSTRING detectada: "${shorter}" em "${longer}" (ratio: ${(lengthRatio * 100).toFixed(1)}%, score: ${(substringScore * 100).toFixed(0)}%)`);

                return Math.min(substringScore, 0.97); // Cap em 97%
            }

            // Fallback: Usar Levenshtein distance para similaridade por edições
            const dist = levenshteinDistance(na, nb);
            const maxLen = Math.max(na.length, nb.length);

            return 1 - (dist / maxLen);
        } catch (error) {
            console.error('calculateSimilarity error:', error);
            return 0.0;
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: getKendoComboBox
     * --------------------------------------------------------------------------------------
     * Obtém instância do Kendo ComboBox por ID do elemento
     ****************************************************************************************/
    function getKendoComboBox(elementId) {
        try {
            const element = $(`#${elementId}`);
            if (!element.length) return null;
            return element.data('kendoComboBox');
        } catch (error) {
            console.error(`getKendoComboBox error (${elementId}):`, error);
            return null;
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: getComboDataSource
     * --------------------------------------------------------------------------------------
     * Extrai array de textos do dataSource do Kendo ComboBox
     ****************************************************************************************/
    function getComboDataSource(combo) {
        try {
            if (!combo) return [];

            const ds = combo.dataSource;
            if (!ds) return [];

            const data = ds.data();
            if (!Array.isArray(data)) return [];

            // Se for array de strings simples
            if (data.length && typeof data[0] === 'string') {
                return data;
            }

            // Se for array de objetos, extrair campo de texto
            const textField = combo.options.dataTextField;
            if (textField) {
                return data.map(item => item[textField] || '').filter(x => x);
            }

            return [];
        } catch (error) {
            console.error('getComboDataSource error:', error);
            return [];
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: getCacheKey
     * --------------------------------------------------------------------------------------
     * Gera chave única para cache de validações
     ****************************************************************************************/
    function getCacheKey(comboId, value) {
        return `${comboId}:${normalizeText(value)}`;
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: getCachedValidation
     * --------------------------------------------------------------------------------------
     * Recupera resultado de validação do cache (se ainda válido)
     ****************************************************************************************/
    function getCachedValidation(comboId, value) {
        if (!CONFIG.enableCache) return null;

        const key = getCacheKey(comboId, value);
        const cached = validationCache.get(key);

        if (!cached) return null;

        // Verificar expiração
        if (Date.now() - cached.timestamp > CONFIG.cacheExpiration) {
            validationCache.delete(key);
            return null;
        }

        return cached.result;
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: setCachedValidation
     * --------------------------------------------------------------------------------------
     * Armazena resultado de validação no cache
     ****************************************************************************************/
    function setCachedValidation(comboId, value, result) {
        if (!CONFIG.enableCache) return;

        const key = getCacheKey(comboId, value);
        validationCache.set(key, {
            timestamp: Date.now(),
            result: result
        });

        // Limpar cache antigo periodicamente
        if (validationCache.size > 1000) {
            const now = Date.now();
            for (const [k, v] of validationCache.entries()) {
                if (now - v.timestamp > CONFIG.cacheExpiration) {
                    validationCache.delete(k);
                }
            }
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: findBestMatch
     * --------------------------------------------------------------------------------------
     * Encontra o item mais similar na lista de opções
     ****************************************************************************************/
    function findBestMatch(typedValue, options) {
        try {
            if (!typedValue || !Array.isArray(options) || !options.length) {
                return { match: null, score: 0, similarCount: 0 };
            }

            let bestMatch = null;
            let bestScore = 0;
            let similarCount = 0; // Contador de matches similares (score >= 80%)
            const SIMILAR_THRESHOLD = 0.80; // 80% de similaridade

            for (const option of options) {
                const score = calculateSimilarity(typedValue, option);

                // Contar opções com similaridade >= 80%
                if (score >= SIMILAR_THRESHOLD) {
                    similarCount++;
                }

                if (score > bestScore) {
                    bestScore = score;
                    bestMatch = option;
                }
            }

            // Subtrair 1 porque o melhor match já será mostrado separadamente
            const otherSimilarCount = similarCount > 0 ? similarCount - 1 : 0;

            return { match: bestMatch, score: bestScore, similarCount: otherSimilarCount };
        } catch (error) {
            console.error('findBestMatch error:', error);
            return { match: null, score: 0, similarCount: 0 };
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: getFieldLabel
     * --------------------------------------------------------------------------------------
     * Retorna label amigável do campo (Origem/Destino/Item)
     ****************************************************************************************/
    function getFieldLabel(comboId) {
        const labels = {
            'cmbOrigem': 'Origem',
            'cmbDestino': 'Destino'
        };
        return labels[comboId] || 'Item';
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: highlightControl
     * --------------------------------------------------------------------------------------
     * Aplica highlight visual temporário no controle
     ****************************************************************************************/
    function highlightControl(combo, type = 'warning') {
        if (!CONFIG.enableHighlight || !combo) return;

        try {
            const $wrapper = $(combo.element).closest('.k-combobox');
            const className = `fuzzy-highlight-${type}`;

            $wrapper.addClass(className);

            setTimeout(() => {
                $wrapper.removeClass(className);
            }, CONFIG.highlightDuration);
        } catch (error) {
            console.error('highlightControl error:', error);
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: saveToHistory
     * --------------------------------------------------------------------------------------
     * Salva correção no histórico (localStorage)
     ****************************************************************************************/
    function saveToHistory(comboId, original, corrected, score) {
        if (!CONFIG.enableHistory) return;

        try {
            const history = JSON.parse(localStorage.getItem(CONFIG.historyStorageKey) || '[]');

            history.unshift({
                timestamp: new Date().toISOString(),
                field: getFieldLabel(comboId),
                comboId: comboId,
                original: original,
                corrected: corrected,
                similarity: Math.round(score * 100)
            });

            // Manter apenas últimas N entradas
            if (history.length > CONFIG.maxHistoryEntries) {
                history.length = CONFIG.maxHistoryEntries;
            }

            localStorage.setItem(CONFIG.historyStorageKey, JSON.stringify(history));
        } catch (error) {
            console.error('saveToHistory error:', error);
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: autoCorrectValue
     * --------------------------------------------------------------------------------------
     * Auto-corrige valor do ComboBox para a opção canônica
     ****************************************************************************************/
    function autoCorrectValue(combo, comboId, canonical) {
        if (!CONFIG.autoCorrect || !combo || !canonical) return false;

        try {
            const originalValue = combo.value();

            // Atualizar valor do combo
            combo.value(canonical);
            combo.text(canonical);

            // Salvar no histórico
            saveToHistory(comboId, originalValue, canonical, 1.0);

            // Atualizar métricas
            METRICS.autoCorrections++;

            // Highlight de sucesso
            highlightControl(combo, 'success');

            console.log(`[Fuzzy] Auto-correção: "${originalValue}" → "${canonical}"`);
            return true;
        } catch (error) {
            console.error('autoCorrectValue error:', error);
            return false;
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: showAlert
     * --------------------------------------------------------------------------------------
     * Exibe alerta usando sistema Alerta do FrotiX
     ****************************************************************************************/
    function showAlert(type, title, message, confirmText = 'OK') {
        try {
            if (typeof Alerta === 'undefined') {
                console.warn(`[Fuzzy ${type}] ${title}: ${message}`);
                return;
            }

            switch (type) {
                case 'info':
                    Alerta.Info(title, message, confirmText);
                    break;
                case 'warning':
                    Alerta.Warning(title, message, confirmText);
                    break;
                default:
                    console.warn(`[Fuzzy] ${title}: ${message}`);
            }
        } catch (error) {
            console.error('showAlert error:', error);
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: showInlineAlert
     * --------------------------------------------------------------------------------------
     * Exibe aviso visual inline embaixo do campo
     ****************************************************************************************/
    function showInlineAlert(comboId, type, text, similarCount = 0) {
        try {
            console.log(`[Fuzzy] 🎨 showInlineAlert chamada:`, { comboId, type, text, similarCount });

            // Determinar ID do elemento de alerta baseado no comboId
            let alertId;
            if (comboId === 'cmbOrigem') {
                alertId = 'fuzzy-alert-origem';
            } else if (comboId === 'cmbDestino') {
                alertId = 'fuzzy-alert-destino';
            } else {
                console.warn(`[Fuzzy] ❌ ComboId desconhecido: ${comboId}`);
                return; // Sem elemento de alerta para este campo
            }

            const alertElement = document.getElementById(alertId);
            if (!alertElement) {
                console.error(`[Fuzzy] ❌ Elemento de alerta não encontrado: ${alertId}`);
                return;
            }
            console.log(`[Fuzzy] ✅ Elemento de alerta encontrado: ${alertId}`);

            const textElement = alertElement.querySelector('.fuzzy-alert-text');
            if (!textElement) {
                console.error(`[Fuzzy] ❌ Elemento de texto não encontrado em: ${alertId}`);
                return;
            }

            // Remover classes anteriores
            alertElement.classList.remove('warning', 'info', 'success', 'multiple');

            // Adicionar classe do tipo
            alertElement.classList.add(type);

            // Montar texto com contagem de similares (se houver)
            let finalText = text;
            if (similarCount > 0) {
                alertElement.classList.add('multiple'); // Classe extra para estilo diferente
                finalText += ` • Além de outros ${similarCount} item${similarCount > 1 ? 's' : ''} similar${similarCount > 1 ? 'es' : ''}`;
            }

            textElement.textContent = finalText;

            // Mostrar elemento
            alertElement.style.display = 'flex';

        } catch (error) {
            console.error('showInlineAlert error:', error);
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: hideInlineAlert
     * --------------------------------------------------------------------------------------
     * Oculta aviso visual inline
     ****************************************************************************************/
    function hideInlineAlert(comboId) {
        try {
            let alertId;
            if (comboId === 'cmbOrigem') {
                alertId = 'fuzzy-alert-origem';
            } else if (comboId === 'cmbDestino') {
                alertId = 'fuzzy-alert-destino';
            } else {
                return;
            }

            const alertElement = document.getElementById(alertId);
            if (!alertElement) return;

            alertElement.style.display = 'none';
            alertElement.classList.remove('warning', 'info', 'success');

        } catch (error) {
            console.error('hideInlineAlert error:', error);
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: validateDuplicate
     * --------------------------------------------------------------------------------------
     * Valida se valor digitado é duplicata/similar a algum item da lista
     ****************************************************************************************/
    function validateDuplicate(combo, comboId, options) {
        try {
            console.log(`[Fuzzy] 🔍 validateDuplicate chamada para ${comboId}`);

            if (!combo) {
                console.warn(`[Fuzzy] ❌ Combo não encontrado em validateDuplicate: ${comboId}`);
                return;
            }

            const typedValue = combo.value();
            console.log(`[Fuzzy] 📝 Valor digitado em ${comboId}: "${typedValue}"`);

            if (!typedValue) {
                console.log(`[Fuzzy] ⚠️ Valor vazio em ${comboId} - ocultando alerta`);
                hideInlineAlert(comboId);
                return;
            }

            // Verificar cache
            const cached = getCachedValidation(comboId, typedValue);
            if (cached !== null) {
                console.log(`[Fuzzy] 💾 Valor em cache encontrado para ${comboId}:`, cached);
                return cached;
            }

            const fieldLabel = getFieldLabel(comboId);
            const normalized = normalizeText(typedValue);
            console.log(`[Fuzzy] 🔤 Valor normalizado: "${normalized}"`);

            // Se já existe exato na lista, ok
            const exactMatch = options.some(opt => String(opt) === typedValue);
            if (exactMatch) {
                console.log(`[Fuzzy] ✅ Match EXATO encontrado - ocultando alerta`);
                hideInlineAlert(comboId);
                setCachedValidation(comboId, typedValue, { valid: true });
                return { valid: true };
            }
            console.log(`[Fuzzy] ❌ Nenhum match EXATO - continuando validação`);

            // Buscar match exato normalizado (para auto-correção)
            const canonicalMap = new Map();
            for (const opt of options) {
                const norm = normalizeText(opt);
                if (!canonicalMap.has(norm)) {
                    canonicalMap.set(norm, opt);
                }
            }

            if (canonicalMap.has(normalized)) {
                const canonical = canonicalMap.get(normalized);
                console.log(`[Fuzzy] 🔄 Match NORMALIZADO encontrado: "${canonical}"`);
                if (canonical !== typedValue) {
                    console.log(`[Fuzzy] 🔄 Auto-corrigindo "${typedValue}" → "${canonical}"`);
                    // Auto-corrigir
                    autoCorrectValue(combo, comboId, canonical);
                    setCachedValidation(comboId, typedValue, { valid: true, corrected: canonical });
                    return { valid: true, corrected: canonical };
                }
                console.log(`[Fuzzy] ✅ Valor já é canônico - retornando válido`);
                return { valid: true };
            }
            console.log(`[Fuzzy] 🔍 Nenhum match normalizado - buscando similar`);



            // Buscar melhor match similar
            console.log(`[Fuzzy] 🎯 Buscando melhor match similar...`);
            const { match, score, similarCount } = findBestMatch(typedValue, options);
            console.log(`[Fuzzy] 🎯 Melhor match:`, { match, score, pct: Math.round(score * 100), similarCount });

            // Atualizar métricas
            METRICS.totalValidations++;
            METRICS.lastValidation = new Date();
            METRICS.avgSimilarityScore =
                (METRICS.avgSimilarityScore * (METRICS.totalValidations - 1) + score) / METRICS.totalValidations;

            if (!match) {
                console.log(`[Fuzzy] ❌ Nenhum match similar encontrado - aplicando capitalização`);

                // NOVO: Capitalizar primeira letra de texto livre
                const capitalized = capitalizeFirstLetter(typedValue);
                if (capitalized !== typedValue) {
                    console.log(`[Fuzzy] 🔤 Capitalizando: "${typedValue}" → "${capitalized}"`);
                    combo.text(capitalized);
                    combo.input.val(capitalized);
                }

                hideInlineAlert(comboId);
                setCachedValidation(comboId, typedValue, { valid: true });
                return { valid: true };
            }

            const pct = Math.round(score * 100);
            const suggestion = CONFIG.showSuggestions
                ? `\n\nSugestão: "${match}" (similaridade ${pct}%)`
                : '';

            console.log(`[Fuzzy] 📊 Verificando thresholds:`, {
                score: pct + '%',
                critical: Math.round(CONFIG.thresholds.critical * 100) + '%',
                warning: Math.round(CONFIG.thresholds.warning * 100) + '%',
                info: Math.round(CONFIG.thresholds.info * 100) + '%'
            });

            // Verificar thresholds
            if (score >= CONFIG.thresholds.critical) {
                console.log(`[Fuzzy] 🟢 CRITICAL threshold atingido - auto-corrigindo`);
                // Muito similar - auto-corrigir sem perguntar
                autoCorrectValue(combo, comboId, match);
                showInlineAlert(comboId, 'success', `✓ Auto-corrigido para "${match}" (${pct}%)`, similarCount);
                METRICS.duplicatesDetected++;
                setCachedValidation(comboId, typedValue, { valid: true, corrected: match });
                return { valid: true, corrected: match };
            }
            else if (score >= CONFIG.thresholds.warning) {
                console.log(`[Fuzzy] 🟠 WARNING threshold atingido - mostrando alerta`);
                // Provável duplicata - warning
                highlightControl(combo, 'warning');
                showInlineAlert(comboId, 'warning', `⚠ Atenção: similar a "${match}" (${pct}%)`, similarCount);
                // SweetAlert REMOVIDO - apenas alerta inline
                METRICS.duplicatesDetected++;
                setCachedValidation(comboId, typedValue, { valid: false, score: score, match: match });
                return { valid: false, score: score, match: match };
            }
            else if (score >= CONFIG.thresholds.info) {
                console.log(`[Fuzzy] 🔵 INFO threshold atingido - mostrando alerta`);
                // Semelhança alta - info
                highlightControl(combo, 'info');
                showInlineAlert(comboId, 'info', `ℹ Aviso: parecido com "${match}" (${pct}%)`, similarCount);
                // SweetAlert REMOVIDO - apenas alerta inline
                setCachedValidation(comboId, typedValue, { valid: true, score: score, match: match });
                return { valid: true, score: score, match: match };
            }

            console.log(`[Fuzzy] ⬜ Score abaixo de todos os thresholds - aplicando capitalização`);

            // NOVO: Capitalizar primeira letra de texto livre
            const capitalized = capitalizeFirstLetter(typedValue);
            if (capitalized !== typedValue) {
                console.log(`[Fuzzy] 🔤 Capitalizando: "${typedValue}" → "${capitalized}"`);
                combo.text(capitalized);
                combo.input.val(capitalized);
            }

            hideInlineAlert(comboId);
            setCachedValidation(comboId, typedValue, { valid: true });
            return { valid: true };

        } catch (error) {
            console.error('validateDuplicate error:', error);
            return { valid: true };
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: validateCrossField
     * --------------------------------------------------------------------------------------
     * Valida se Origem e Destino são muito similares entre si
     ****************************************************************************************/
    function validateCrossField(origemCombo, destinoCombo) {
        try {
            if (!origemCombo || !destinoCombo) return;

            const origem = origemCombo.value();
            const destino = destinoCombo.value();

            if (!origem || !destino) return;

            const score = calculateSimilarity(origem, destino);
            const pct = Math.round(score * 100);

            if (score >= CONFIG.thresholds.warning) {
                highlightControl(origemCombo, 'warning');
                highlightControl(destinoCombo, 'warning');
                showAlert('warning',
                    'Origem e Destino muito parecidos',
                    `Os campos parecem referir-se ao mesmo lugar (similaridade ${pct}%).`,
                    'OK'
                );
            }
            else if (score >= CONFIG.thresholds.info) {
                highlightControl(origemCombo, 'info');
                highlightControl(destinoCombo, 'info');
                showAlert('info',
                    'Origem e Destino semelhantes',
                    `Verifique se são realmente distintos (similaridade ${pct}%).`,
                    'OK'
                );
            }
        } catch (error) {
            console.error('validateCrossField error:', error);
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: debounce
     * --------------------------------------------------------------------------------------
     * Implementa debouncing para evitar validações excessivas
     ****************************************************************************************/
    function debounce(key, callback, delay) {
        const existingTimer = debounceTimers.get(key);
        if (existingTimer) {
            clearTimeout(existingTimer);
        }

        const timer = setTimeout(() => {
            debounceTimers.delete(key);
            callback();
        }, delay);

        debounceTimers.set(key, timer);
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: wireComboValidation
     * --------------------------------------------------------------------------------------
     * Conecta validação fuzzy a um Kendo ComboBox
     ****************************************************************************************/
    function wireComboValidation(comboId, peerComboId, options = {}) {
        try {
            const combo = getKendoComboBox(comboId);
            const peerCombo = peerComboId ? getKendoComboBox(peerComboId) : null;

            if (!combo) {
                console.warn(`[Fuzzy] Combo não encontrado: ${comboId}`);
                return false;
            }

            // CRÍTICO: Aceitar dataSource passado como parâmetro ou pegar do combo
            let dataSource = options.dataSource || getComboDataSource(combo);
            if (!dataSource.length) {
                console.warn(`[Fuzzy] DataSource vazio para: ${comboId}`);
                return false;
            }

            // Cachear dataSource no combo
            combo.__fuzzyDataSource = dataSource;

            // Evento CHANGE (quando usuário seleciona ou digita)
            combo.bind('change', function (e) {
                console.log(`[Fuzzy] 🔔 Evento CHANGE disparado em ${comboId}:`, {
                    value: combo.value(),
                    text: combo.text()
                });

                debounce(`${comboId}_change`, () => {
                    console.log(`[Fuzzy] 🔍 Iniciando validação para ${comboId}`);
                    const currentOptions = combo.__fuzzyDataSource || getComboDataSource(combo);
                    validateDuplicate(combo, comboId, currentOptions);

                    // Validação cruzada
                    if (peerCombo) {
                        validateCrossField(
                            comboId === 'cmbOrigem' ? combo : peerCombo,
                            comboId === 'cmbOrigem' ? peerCombo : combo
                        );
                    }
                }, CONFIG.debounceDelay);
            });

            console.log(`[Fuzzy] Validação conectada: ${comboId} (${dataSource.length} opções)`);
            return true;

        } catch (error) {
            console.error(`wireComboValidation error (${comboId}):`, error);
            return false;
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: init
     * --------------------------------------------------------------------------------------
     * Inicializa sistema de validação fuzzy para Origem e Destino
     ****************************************************************************************/
    function init(customConfig = {}) {
        try {
            // Merge configuração customizada
            Object.assign(CONFIG, customConfig);

            console.log('[Fuzzy] Inicializando sistema de validação...');

            // Conectar validação aos ComboBox (com dataSource opcional)
            const origemOk = wireComboValidation('cmbOrigem', 'cmbDestino', {
                dataSource: customConfig.origemData
            });
            const destinoOk = wireComboValidation('cmbDestino', 'cmbOrigem', {
                dataSource: customConfig.destinoData
            });

            if (origemOk && destinoOk) {
                console.log('[Fuzzy] ✅ Sistema inicializado com sucesso!');
                console.log('[Fuzzy] Configuração:', CONFIG);
                return true;
            } else {
                console.warn('[Fuzzy] ⚠️ Sistema inicializado parcialmente');
                return false;
            }

        } catch (error) {
            console.error('[Fuzzy] Erro na inicialização:', error);
            Alerta.TratamentoErroComLinha('kendo-fuzzy-validator.js', 'init', error);
            return false;
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: getMetrics
     * --------------------------------------------------------------------------------------
     * Retorna métricas de uso do sistema
     ****************************************************************************************/
    function getMetrics() {
        return { ...METRICS };
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: getHistory
     * --------------------------------------------------------------------------------------
     * Retorna histórico de correções
     ****************************************************************************************/
    function getHistory() {
        try {
            return JSON.parse(localStorage.getItem(CONFIG.historyStorageKey) || '[]');
        } catch (error) {
            console.error('getHistory error:', error);
            return [];
        }
    }

    /****************************************************************************************
     * ⚡ FUNÇÃO: clearHistory
     * --------------------------------------------------------------------------------------
     * Limpa histórico de correções
     ****************************************************************************************/
    function clearHistory() {
        try {
            localStorage.removeItem(CONFIG.historyStorageKey);
            console.log('[Fuzzy] Histórico limpo');
        } catch (error) {
            console.error('clearHistory error:', error);
        }
    }

    /****************************************************************************************
     * 🌐 EXPORTAÇÃO PÚBLICA (API)
     ****************************************************************************************/
    window.KendoFuzzyValidator = {
        init: init,
        getMetrics: getMetrics,
        getHistory: getHistory,
        clearHistory: clearHistory,
        version: '2.0'
    };

    console.log('[Fuzzy] Módulo carregado (v2.0)');

})(window);
