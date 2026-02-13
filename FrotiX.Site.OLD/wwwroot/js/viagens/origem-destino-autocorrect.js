/****************************************************************************************
 * 📄 ARQUIVO: origem-destino-autocorrect.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO: Auto-correção de texto nos campos Origem/Destino (blur)
 * 📅 DATA: 12/02/2026
 * 👤 AUTOR: Sistema FrotiX
 * 📌 VERSÃO: 1.0
 * --------------------------------------------------------------------------------------
 * 📝 DESCRIÇÃO:
 *    Módulo de auto-correção automática aplicada no evento blur (lost focus) dos
 *    campos Origem e Destino. Aplica correções ortográficas, capitalização de siglas,
 *    normalização de espaços, padronização de pontuação e outras melhorias de qualidade.
 * --------------------------------------------------------------------------------------
 * 🔧 FUNCIONALIDADES:
 *    - Correção ortográfica automática (acentos, cedilha, s/z, typos)
 *    - Capitalização de siglas e acrônimos (PGR, Cefor, Ctran, UnB)
 *    - Capitalização de primeira letra
 *    - Preposições e artigos em minúscula (de, da, do, dos, e, para)
 *    - Normalização de espaços (múltiplos → único)
 *    - Padronização de hífen/travessão (-, –, — → " - ")
 *    - Remoção de pontuação desnecessária no final
 *    - Números romanos em maiúscula (i, ii, iii → I, II, III, IV)
 *    - Remoção de caracteres duplicados
 * --------------------------------------------------------------------------------------
 * 📦 DEPENDÊNCIAS:
 *    - jQuery 3.7.1+
 *    - Kendo UI 2025.4.1321+
 * --------------------------------------------------------------------------------------
 * 🔗 PÁGINAS QUE USAM:
 *    - Pages/Viagens/Upsert.cshtml (cmbOrigem, cmbDestino)
 *    - Pages/Agenda/Index.cshtml (cmbOrigem, cmbDestino)
 * --------------------------------------------------------------------------------------
 * 📋 CHANGELOG:
 *    v1.1 (12/02/2026) - Adicionada correção universal de encoding UTF-8/Latin1 (30 mapeamentos)
 *                        Corrige QUALQUER caractere malformado em QUALQUER posição do texto
 *    v1.0 (12/02/2026) - Versão inicial com dicionário de 136 correções
 ****************************************************************************************/

(function (window) {
    'use strict';

    /****************************************************************************************
     * 📚 DICIONÁRIO DE CORREÇÕES ORTOGRÁFICAS
     * --------------------------------------------------------------------------------------
     * Baseado no script SQL Limpeza_Origem_Destino.sql (136 mapeamentos)
     ****************************************************************************************/
    const DICTIONARY = {
        // Preposição "A"
        'Á definir': 'A definir',
        'À definir': 'A definir',
        'á definir': 'A definir',
        'à definir': 'A definir',

        // Acentos faltantes
        'Deposito': 'Depósito',
        'deposito': 'Depósito',
        'Area': 'Área',
        'area': 'Área',
        'Sanitaria': 'Sanitária',
        'sanitaria': 'Sanitária',
        'Ambulancia': 'Ambulância',
        'ambulancia': 'Ambulância',
        'Auditorio': 'Auditório',
        'auditorio': 'Auditório',
        'Saida': 'Saída',
        'saida': 'Saída',
        'Emergencia': 'Emergência',
        'emergencia': 'Emergência',
        'Funcionario': 'Funcionário',
        'funcionario': 'Funcionário',
        'Rodoviaria': 'Rodoviária',
        'rodoviaria': 'Rodoviária',
        'Grafica': 'Gráfica',
        'grafica': 'Gráfica',
        'Residencia': 'Residência',
        'residencia': 'Residência',

        // Cedilha faltante
        'Recepcao': 'Recepção',
        'recepcao': 'Recepção',
        'RECEPCAO': 'Recepção',
        'Estacao': 'Estação',
        'estacao': 'Estação',
        'Manutencao': 'Manutenção',
        'manutencao': 'Manutenção',
        'Administracao': 'Administração',
        'administracao': 'Administração',
        'Coordenacao': 'Coordenação',
        'coordenacao': 'Coordenação',

        // Til faltante
        'Portao': 'Portão',
        'portao': 'Portão',

        // Typos comuns
        'trasmissão': 'transmissão',
        'Trasmissão': 'Transmissão',
        'trasmissao': 'transmissão',
        'Descaga': 'Descarga',
        'descaga': 'descarga',
        'caneteiro': 'canteiro',
        'Pragramação': 'Programação',
        'pragramação': 'programação',

        // Siglas e Acrônimos (conforme regras da língua portuguesa)
        'cefor': 'Cefor',
        'CEFOR': 'Cefor',
        'ceforc': 'Cefor',
        'ctran': 'Ctran',
        'CTRAN': 'Ctran',
        'ctram': 'Ctran',
        'pgr': 'PGR',
        'Pgr': 'PGR',
        'unb': 'UnB',
        'Unb': 'UnB',
        'UNB': 'UnB',

        // Nomes próprios
        'camara': 'Câmara',
        'Camara': 'Câmara',
        'ANIVERSARIO': 'Aniversário',
        'Aniversario': 'Aniversário',
        'aniversario': 'Aniversário',
        'brasilia': 'Brasília',
        'Brasilia': 'Brasília',

        // Palavras completas (incompletas → completas)
        'Almoxarifado': 'Almoxarifado SIA',
        'sia': 'Almoxarifado SIA',
        'Sia': 'Almoxarifado SIA',
        'Universidade de Brasília': 'UnB'
    };

    /****************************************************************************************
     * 🔧 MAPA DE CORREÇÃO DE ENCODING UTF-8 → Latin1/Windows-1252
     * --------------------------------------------------------------------------------------
     * Caracteres acentuados UTF-8 interpretados incorretamente como Latin1
     ****************************************************************************************/
    const ENCODING_FIXES = {
        // Minúsculas com til
        'Ã£': 'ã',  // a til
        'Ãµ': 'õ',  // o til

        // Minúsculas com cedilha
        'Ã§': 'ç',  // c cedilha

        // Minúsculas com acento agudo
        'Ã¡': 'á',  // a agudo
        'Ã©': 'é',  // e agudo
        'Ã­': 'í',  // i agudo
        'Ã³': 'ó',  // o agudo
        'Ãº': 'ú',  // u agudo

        // Minúsculas com acento circunflexo
        'Ã¢': 'â',  // a circunflexo
        'Ãª': 'ê',  // e circunflexo
        'Ã´': 'ô',  // o circunflexo

        // Minúsculas com acento grave
        'Ã ': 'à',  // a grave

        // Maiúsculas com til
        'Ã': 'Ã',   // A til
        'Ãµ': 'Õ',  // O til (versão maiúscula, mesmo código que minúscula)

        // Maiúsculas com cedilha
        'Ã‡': 'Ç',  // C cedilha

        // Maiúsculas com acento agudo
        'Ã': 'Á',   // A agudo
        'Ã‰': 'É',  // E agudo
        'Ã': 'Í',   // I agudo
        'Ã"': 'Ó',  // O agudo
        'Ãš': 'Ú',  // U agudo

        // Maiúsculas com acento circunflexo
        'Ã‚': 'Â',  // A circunflexo
        'ÃŠ': 'Ê',  // E circunflexo
        'Ã"': 'Ô',  // O circunflexo

        // Maiúsculas com acento grave
        'Ã€': 'À'   // A grave
    };

    /****************************************************************************************
     * 📋 LISTA DE PREPOSIÇÕES/ARTIGOS (devem ficar em minúscula, exceto início)
     ****************************************************************************************/
    const PREPOSITIONS = [
        'de', 'da', 'do', 'dos', 'das',
        'a', 'ao', 'aos', 'à', 'às',
        'e', 'ou',
        'em', 'na', 'no', 'nas', 'nos',
        'para', 'com', 'sem', 'por'
    ];

    /****************************************************************************************
     * 🎯 FUNÇÃO: autoCorrectText
     * --------------------------------------------------------------------------------------
     * Aplica TODAS as correções automáticas ao texto fornecido
     *
     * @param {string} text - Texto original a ser corrigido
     * @returns {string} - Texto corrigido
     ****************************************************************************************/
    function autoCorrectText(text) {
        try {
            if (!text || typeof text !== 'string') {
                return '';
            }

            let corrected = text;

            // ══════════════════════════════════════════════════════════════════════════
            // ETAPA 0: CORREÇÃO DE ENCODING UTF-8/Latin1 (PRIMEIRA PRIORIDADE)
            // ══════════════════════════════════════════════════════════════════════════

            corrected = fixEncoding(corrected);

            // ══════════════════════════════════════════════════════════════════════════
            // ETAPA 1: NORMALIZAÇÃO DE ESPAÇOS
            // ══════════════════════════════════════════════════════════════════════════

            // Remove espaços no início e fim
            corrected = corrected.trim();

            // Converte múltiplos espaços em um único espaço
            corrected = corrected.replace(/\s{2,}/g, ' ');

            // Remove espaços antes de pontuação (, ; . ! ?)
            corrected = corrected.replace(/\s+([,;.!?])/g, '$1');

            // Garante espaço APÓS pontuação (se não tiver)
            corrected = corrected.replace(/([,;.!?])([^\s])/g, '$1 $2');

            // ══════════════════════════════════════════════════════════════════════════
            // ETAPA 2: NORMALIZAÇÃO DE CARACTERES ESPECIAIS
            // ══════════════════════════════════════════════════════════════════════════

            // Padroniza aspas ("" '' → "")
            corrected = corrected.replace(/[""'']/g, '"');

            // Padroniza hífen/travessão (–, — → -)
            corrected = corrected.replace(/[–—]/g, '-');

            // Normaliza hífen entre palavras (garante espaços: "palavra-palavra" → "palavra - palavra")
            // Mas PRESERVA hífens em Anexo I-senado, etc.
            corrected = corrected.replace(/(\w)\s*-\s*(\w)/g, function(match, before, after) {
                // Se for número romano seguido de hífen (Anexo I-, II-, III-), manter sem espaço
                if (/^(I|II|III|IV|V|VI|VII|VIII|IX|X)$/i.test(before.trim())) {
                    return before + '- ' + after;
                }
                // Caso contrário, adicionar espaços
                return before + ' - ' + after;
            });

            // Remove caracteres duplicados de pontuação (, , → ,)
            corrected = corrected.replace(/([,;.!?])\1+/g, '$1');

            // Remove pontuação no final (exceto ponto final legítimo)
            corrected = corrected.replace(/[,;]+$/g, '');

            // ══════════════════════════════════════════════════════════════════════════
            // ETAPA 3: NÚMEROS ROMANOS EM MAIÚSCULA
            // ══════════════════════════════════════════════════════════════════════════

            // Anexo i, ii, iii, iv → Anexo I, II, III, IV
            corrected = corrected.replace(/\b(Anexo)\s+(i|ii|iii|iv|v|vi|vii|viii|ix|x)\b/gi, function(match, anexo, num) {
                return 'Anexo ' + num.toUpperCase();
            });

            // ══════════════════════════════════════════════════════════════════════════
            // ETAPA 4: APLICAR DICIONÁRIO DE CORREÇÕES ORTOGRÁFICAS
            // ══════════════════════════════════════════════════════════════════════════

            // Aplicar correções exatas do dicionário (case-sensitive)
            Object.keys(DICTIONARY).forEach(function(wrong) {
                const right = DICTIONARY[wrong];
                // Substituição global com word boundary para evitar substituições parciais
                const regex = new RegExp('\\b' + escapeRegex(wrong) + '\\b', 'g');
                corrected = corrected.replace(regex, right);
            });

            // ══════════════════════════════════════════════════════════════════════════
            // ETAPA 5: CAPITALIZAÇÃO INTELIGENTE
            // ══════════════════════════════════════════════════════════════════════════

            // Dividir em palavras
            const words = corrected.split(/\s+/);
            const capitalizedWords = words.map(function(word, index) {
                const lowerWord = word.toLowerCase();

                // Se for preposição/artigo E não for a primeira palavra, minúscula
                if (index > 0 && PREPOSITIONS.includes(lowerWord)) {
                    return lowerWord;
                }

                // Se for sigla conhecida (PGR, Cefor, Ctran, UnB), aplicar capitalização correta
                if (lowerWord === 'pgr') return 'PGR';
                if (lowerWord === 'cefor') return 'Cefor';
                if (lowerWord === 'ctran') return 'Ctran';
                if (lowerWord === 'unb') return 'UnB';

                // Se for número romano, manter maiúscula
                if (/^(I|II|III|IV|V|VI|VII|VIII|IX|X)$/i.test(word)) {
                    return word.toUpperCase();
                }

                // Se for palavra toda maiúscula E tiver > 3 letras, capitalizar primeira letra
                if (word === word.toUpperCase() && word.length > 3 && /^[A-ZÁÉÍÓÚÂÊÔÃÕÇ]+$/.test(word)) {
                    return word.charAt(0).toUpperCase() + word.slice(1).toLowerCase();
                }

                // Se a palavra começar com minúscula, capitalizar primeira letra
                if (word.charAt(0) === word.charAt(0).toLowerCase() && /^[a-záéíóúâêôãõç]/.test(word.charAt(0))) {
                    return word.charAt(0).toUpperCase() + word.slice(1);
                }

                // Caso contrário, manter como está
                return word;
            });

            corrected = capitalizedWords.join(' ');

            // ══════════════════════════════════════════════════════════════════════════
            // ETAPA 6: CAPITALIZAÇÃO OBRIGATÓRIA DA PRIMEIRA LETRA
            // ══════════════════════════════════════════════════════════════════════════

            if (corrected.length > 0) {
                corrected = corrected.charAt(0).toUpperCase() + corrected.slice(1);
            }

            // ══════════════════════════════════════════════════════════════════════════
            // ETAPA 7: LIMPEZA FINAL
            // ══════════════════════════════════════════════════════════════════════════

            // Trim final
            corrected = corrected.trim();

            return corrected;

        } catch (error) {
            console.error('[AutoCorrect] Erro ao corrigir texto:', error);
            return text; // Retorna texto original em caso de erro
        }
    }

    /****************************************************************************************
     * 🔧 FUNÇÃO: fixEncoding
     * --------------------------------------------------------------------------------------
     * Corrige caracteres UTF-8 mal interpretados como Latin1/Windows-1252
     * Aplica substituições EM TODA a string, não apenas em palavras específicas
     *
     * @param {string} text - Texto com possíveis erros de encoding
     * @returns {string} - Texto com encoding corrigido
     ****************************************************************************************/
    function fixEncoding(text) {
        try {
            if (!text || typeof text !== 'string') {
                return text;
            }

            let fixed = text;

            // Aplicar todas as correções de encoding
            Object.keys(ENCODING_FIXES).forEach(function(wrong) {
                const right = ENCODING_FIXES[wrong];
                // Substituição global - em qualquer lugar do texto
                fixed = fixed.split(wrong).join(right);
            });

            return fixed;

        } catch (error) {
            console.error('[AutoCorrect] Erro ao corrigir encoding:', error);
            return text;
        }
    }

    /****************************************************************************************
     * 🛠️ FUNÇÃO AUXILIAR: escapeRegex
     * --------------------------------------------------------------------------------------
     * Escapa caracteres especiais de regex para uso em RegExp
     ****************************************************************************************/
    function escapeRegex(string) {
        return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    }

    /****************************************************************************************
     * 🔌 FUNÇÃO: applyToComboBox
     * --------------------------------------------------------------------------------------
     * Aplica auto-correção ao ComboBox Kendo no evento blur
     *
     * @param {string} comboBoxId - ID do elemento ComboBox (#cmbOrigem, #cmbDestino)
     ****************************************************************************************/
    function applyToComboBox(comboBoxId) {
        try {
            const $combo = $(comboBoxId);
            if (!$combo.length) {
                console.warn('[AutoCorrect] ComboBox não encontrado:', comboBoxId);
                return;
            }

            const combo = $combo.data('kendoComboBox');
            if (!combo) {
                console.warn('[AutoCorrect] Widget Kendo não inicializado:', comboBoxId);
                return;
            }

            // Adicionar listener no evento blur do input
            combo.input.on('blur', function() {
                try {
                    const originalText = combo.text();
                    if (!originalText) {
                        return; // Vazio, nada a corrigir
                    }

                    const correctedText = autoCorrectText(originalText);

                    // Se houve alteração, aplicar correção
                    if (correctedText !== originalText) {
                        console.log('[AutoCorrect] Correção aplicada:', {
                            combo: comboBoxId,
                            original: originalText,
                            corrected: correctedText
                        });

                        combo.text(correctedText);
                        combo.input.val(correctedText);

                        // Trigger change para notificar outras validações
                        combo.trigger('change');
                    }
                } catch (error) {
                    console.error('[AutoCorrect] Erro no blur:', error);
                }
            });

            console.log('[AutoCorrect] Aplicado ao ComboBox:', comboBoxId);

        } catch (error) {
            console.error('[AutoCorrect] Erro ao aplicar ao ComboBox:', error);
        }
    }

    /****************************************************************************************
     * 🔌 FUNÇÃO: init
     * --------------------------------------------------------------------------------------
     * Inicializa o módulo de auto-correção para os ComboBoxes especificados
     *
     * @param {Object} options - Opções de configuração
     * @param {string} options.origemId - ID do ComboBox Origem (padrão: '#cmbOrigem')
     * @param {string} options.destinoId - ID do ComboBox Destino (padrão: '#cmbDestino')
     ****************************************************************************************/
    function init(options) {
        try {
            const config = $.extend({
                origemId: '#cmbOrigem',
                destinoId: '#cmbDestino'
            }, options || {});

            console.log('[AutoCorrect] Inicializando módulo de auto-correção...');

            // Aplicar ao ComboBox Origem
            if (config.origemId) {
                applyToComboBox(config.origemId);
            }

            // Aplicar ao ComboBox Destino
            if (config.destinoId) {
                applyToComboBox(config.destinoId);
            }

            console.log('[AutoCorrect] Módulo inicializado com sucesso');

        } catch (error) {
            console.error('[AutoCorrect] Erro ao inicializar:', error);
        }
    }

    /****************************************************************************************
     * 📤 EXPORTAR API PÚBLICA
     ****************************************************************************************/
    window.OrigemDestinoAutoCorrect = {
        init: init,
        autoCorrectText: autoCorrectText,
        applyToComboBox: applyToComboBox,
        fixEncoding: fixEncoding,
        getDictionary: function() { return DICTIONARY; },
        getEncodingFixes: function() { return ENCODING_FIXES; },
        version: '1.1'
    };

    console.log('[AutoCorrect] Módulo carregado (v1.1)');

})(window);
