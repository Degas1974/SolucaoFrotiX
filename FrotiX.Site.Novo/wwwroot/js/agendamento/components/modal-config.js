/* ****************************************************************************************
 * ⚡ ARQUIVO: modal-config.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Configurações centralizadas de títulos, ícones e estilos para modal
 *                   de viagens (modalViagens). Define 7 tipos de modal com HTML templates
 *                   para títulos (Font Awesome duotone icons com cores customizadas, fonte
 *                   Outfit 700, text-shadow). Exporta ModalConfig object, setModalTitle
 *                   function (atualiza título por tipo), resetModal (reseta modal), e
 *                   garantirBotoesFechaHabilitados (setInterval 1000ms para forçar botões
 *                   de fechar sempre enabled - workaround para bug de desabilitação).
 * 📥 ENTRADAS     : setModalTitle(tipo: string chave ModalConfig, statusTexto?: string
 *                   opcional para títulos dinâmicos), resetModal() sem params,
 *                   garantirBotoesFechaHabilitados() sem params
 * 📤 SAÍDAS       : setModalTitle atualiza DOM (#Titulo.innerHTML e fallback seletores),
 *                   resetModal chama setModalTitle + limparCamposModalViagens +
 *                   inicializarCamposModal, garantirBotoesFechaHabilitados atualiza DOM
 *                   (disabled=false, remove class disabled, style pointerEvents/opacity)
 * 🔗 CHAMADA POR  : exibe-viagem.js (ExibeViagem, preencherCamposParaEdicao, modos:
 *                   novo agendamento, editar agendamento, viagem aberta, viagem realizada,
 *                   viagem cancelada, transformar em viagem), modal close events (resetModal),
 *                   setInterval automático 1000ms (garantirBotoesFechaHabilitados), qualquer
 *                   código que precise setar título do modal modalViagens
 * 🔄 CHAMA        : document.getElementById("Titulo"), document.querySelector (múltiplos
 *                   seletores fallback), element.innerHTML setter, element.classList.remove,
 *                   element.style setters (pointerEvents, opacity), document.querySelectorAll,
 *                   Array.forEach, String.replace (regex /<h3[^>]*>|<\/h3>/g), console.log,
 *                   console.warn, Alerta.TratamentoErroComLinha, window.limparCamposModalViagens,
 *                   window.inicializarCamposModal, setInterval (1000ms polling)
 * 📦 DEPENDÊNCIAS : Alerta.TratamentoErroComLinha (frotix-core.js), window.limparCamposModalViagens
 *                   (exibe-viagem.js), window.inicializarCamposModal (exibe-viagem.js), Font
 *                   Awesome duotone icons (fa-duotone CSS classes: fa-calendar-lines-pen,
 *                   fa-calendar-xmark, fa-solid fa-suitcase-rolling), Outfit font family
 *                   (Google Fonts ou local), DOM elements (#Titulo, #modalViagens .modal-title,
 *                   #modalViagemTitulo, #modalViagens .modal-header, #btnFecha, #btnFechar,
 *                   #btnCancelar, .btn-close, [data-bs-dismiss="modal"], .modal-footer
 *                   .btn-secondary), CSS custom properties (--fa-primary-color, --fa-secondary-color)
 * 📝 OBSERVAÇÕES  : Exporta window.ModalConfig (object com 7 keys), window.setModalTitle,
 *                   window.resetModal, window.garantirBotoesFechaHabilitados (4 exports).
 *                   TITLE_STYLE constant com inline CSS (não exportado, interno). setInterval
 *                   executa garantirBotoesFechaHabilitados a cada 1 segundo (polling
 *                   agressivo para workaround de botões desabilitados). ModalConfig keys:
 *                   NOVO_AGENDAMENTO, EDITAR_AGENDAMENTO, AGENDAMENTO_CANCELADO, VIAGEM_ABERTA,
 *                   VIAGEM_REALIZADA (htmlFunc dinâmico), VIAGEM_CANCELADA (htmlFunc),
 *                   TRANSFORMAR_VIAGEM. Cores Font Awesome: Verde (#006400/#A9BA9D) para novo,
 *                   Azul (#002F6C/#7DA2CE) para editar/transformar, Vermelho (#8B0000/#FF4C4C)
 *                   para cancelado. Fallback seletores: 4 diferentes querySelector attempts
 *                   para compatibilidade. HTML templates com <h3> wrapper (removido em
 *                   fallbacks via regex). btn-vinho class para "Edição Não Permitida" span
 *                   (fw-bold fst-italic Bootstrap classes). Try-catch em todas as 3 funções
 *                   com TratamentoErroComLinha. Console.warn para erros silenciosos em
 *                   fallback loops. Títulos com text-shadow rgba(0,0,0,0.2) para profundidade.
 *                   Font Outfit weight 700 (bold) para destaque. statusTexto parameter usado
 *                   em VIAGEM_REALIZADA e VIAGEM_CANCELADA para exibir status específico
 *                   (ex: "Realizada", "Em Andamento", "Cancelada pelo Usuario"). Polling
 *                   setInterval mantém UX consistente mesmo se código externo desabilitar
 *                   botões (proteção contra bugs).
 *
 * 📋 ÍNDICE DE OBJETOS E FUNÇÕES (1 constant, 1 object, 3 functions, 1 setInterval):
 *
 * ┌─ CONSTANT TITLE_STYLE ──────────────────────────────────────────────┐
 * │ const TITLE_STYLE = "font-family: 'Outfit', sans-serif; font-weight:│
 * │   700; color: #fff; text-shadow: 0 2px 4px rgba(0,0,0,0.2);"        │
 * │ → Inline CSS para títulos do modal                                  │
 * │ → Usado em todos os templates HTML de ModalConfig                   │
 * │ → Não exportado (escopo de arquivo)                                 │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ OBJECT window.ModalConfig ─────────────────────────────────────────┐
 * │ → Configurações de 7 tipos de modal                                 │
 * │ → Estrutura de cada key: {html ou htmlFunc, tipo}                   │
 * │ → Keys (7 tipos):                                                    │
 * │                                                                      │
 * │   1. NOVO_AGENDAMENTO:                                               │
 * │      • html: <h3> + fa-calendar-lines-pen (verde #006400/#A9BA9D)   │
 * │      • tipo: 'novo'                                                  │
 * │      • Título: "Criar Agendamento"                                   │
 * │                                                                      │
 * │   2. EDITAR_AGENDAMENTO:                                             │
 * │      • html: <h3> + fa-calendar-lines-pen (azul #002F6C/#7DA2CE)    │
 * │      • tipo: 'editar'                                                │
 * │      • Título: "Editar Agendamento"                                  │
 * │                                                                      │
 * │   3. AGENDAMENTO_CANCELADO:                                          │
 * │      • html: <h3> + fa-calendar-xmark (vermelho #8B0000/#FF4C4C)    │
 * │      • tipo: 'cancelado'                                             │
 * │      • Título: "Agendamento Cancelado"                               │
 * │                                                                      │
 * │   4. VIAGEM_ABERTA:                                                  │
 * │      • html: <h3> + fa-solid fa-suitcase-rolling (sem cores custom) │
 * │      • tipo: 'aberta'                                                │
 * │      • Título: "Exibindo Viagem (Aberta)"                            │
 * │                                                                      │
 * │   5. VIAGEM_REALIZADA:                                               │
 * │      • htmlFunc: (statusTexto = 'Realizada') => template string     │
 * │      • tipo: 'realizada'                                             │
 * │      • Título: "Exibindo Viagem (${statusTexto} - Edição Não        │
 * │        Permitida)" com span.btn-vinho                                │
 * │      • statusTexto padrão: 'Realizada'                               │
 * │                                                                      │
 * │   6. VIAGEM_CANCELADA:                                               │
 * │      • htmlFunc: (statusTexto = 'Cancelada') => template string     │
 * │      • tipo: 'cancelada'                                             │
 * │      • Título: "Exibindo Viagem (${statusTexto} - Edição Não        │
 * │        Permitida)" com span.btn-vinho                                │
 * │      • statusTexto padrão: 'Cancelada'                               │
 * │                                                                      │
 * │   7. TRANSFORMAR_VIAGEM:                                             │
 * │      • html: <h3> + fa-calendar-lines-pen (azul #002F6C/#7DA2CE)    │
 * │      • tipo: 'transformar'                                           │
 * │      • Título: "Transformar Agendamento em Viagem"                   │
 * │                                                                      │
 * │ → Export: window.ModalConfig                                         │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.setModalTitle(tipo, statusTexto) ───────────────────────────┐
 * │ → Define título do modal baseado em tipo (key de ModalConfig)       │
 * │ → param tipo: string, key do ModalConfig (ex: 'NOVO_AGENDAMENTO')  │
 * │ → param statusTexto: string opcional, usado em htmlFunc (VIAGEM_    │
 * │                      REALIZADA, VIAGEM_CANCELADA)                   │
 * │ → returns void (atualiza DOM diretamente)                           │
 * │ → Fluxo:                                                             │
 * │   1. try-catch wrapper                                               │
 * │   2. const config = window.ModalConfig[tipo]                         │
 * │   3. if !config: console.warn + return                               │
 * │   4. Obter tituloHtml:                                               │
 * │      a. if config.htmlFunc: tituloHtml = config.htmlFunc(statusTexto)│
 * │      b. else: tituloHtml = config.html                               │
 * │   5. const tituloElement = getElementById("Titulo")                  │
 * │   6. if tituloElement: tituloElement.innerHTML = tituloHtml          │
 * │   7. Fallback loop: 4 seletores alternativos                         │
 * │      a. "#modalViagens .modal-title"                                 │
 * │      b. "#modalViagemTitulo"                                         │
 * │      c. "#modalViagens .modal-header h3"                             │
 * │      d. "#modalViagens .modal-header"                                │
 * │      - Para cada: querySelector, if exists e não é #Titulo:          │
 * │        * Se .modal-header: busca .modal-title/h3/h5 dentro, innerHTML│
 * │          sem tags <h3>                                               │
 * │        * Senão: innerHTML direto sem tags <h3>                       │
 * │      - Try-catch interno: console.warn se erro                       │
 * │   8. console.log "📋 Título do modal definido:", tipo                │
 * │   9. catch: Alerta.TratamentoErroComLinha                            │
 * │ → Regex replace: /<h3[^>]*>|<\/h3>/g para remover tags h3 em        │
 * │   fallbacks (evita nested h3)                                        │
 * │ → Export: window.setModalTitle                                       │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.resetModal() ───────────────────────────────────────────────┐
 * │ → Reseta modal para estado inicial (novo agendamento)               │
 * │ → returns void                                                       │
 * │ → Fluxo:                                                             │
 * │   1. try-catch wrapper                                               │
 * │   2. window.setModalTitle('NOVO_AGENDAMENTO')                        │
 * │   3. window.limparCamposModalViagens()                               │
 * │   4. window.inicializarCamposModal()                                 │
 * │   5. catch: Alerta.TratamentoErroComLinha                            │
 * │ → Usado ao fechar modal ou resetar para novo agendamento            │
 * │ → Dependências externas: limparCamposModalViagens,                   │
 * │   inicializarCamposModal (definidas em exibe-viagem.js)             │
 * │ → Export: window.resetModal                                          │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ window.garantirBotoesFechaHabilitados() ───────────────────────────┐
 * │ → Garante que botões de fechar nunca sejam desabilitados            │
 * │ → returns void                                                       │
 * │ → Fluxo:                                                             │
 * │   1. try-catch wrapper                                               │
 * │   2. const seletores = [6 seletores]:                                │
 * │      a. '#btnFecha'                                                  │
 * │      b. '#btnFechar'                                                 │
 * │      c. '#btnCancelar'                                               │
 * │      d. '#modalViagens .btn-close'                                   │
 * │      e. '#modalViagens [data-bs-dismiss="modal"]'                    │
 * │      f. '.modal-footer .btn-secondary'                               │
 * │   3. seletores.forEach(seletor => {                                  │
 * │      a. try-catch interno                                            │
 * │      b. const elementos = querySelectorAll(seletor)                  │
 * │      c. elementos.forEach(el => {                                    │
 * │           if (el):                                                   │
 * │             el.disabled = false                                      │
 * │             el.classList.remove('disabled')                          │
 * │             el.style.pointerEvents = 'auto'                          │
 * │             el.style.opacity = '1'                                   │
 * │         })                                                           │
 * │      d. catch interno: console.warn                                  │
 * │      })                                                              │
 * │   4. catch outer: Alerta.TratamentoErroComLinha                      │
 * │ → Chamado por setInterval a cada 1000ms (polling)                   │
 * │ → Workaround para bug onde botões são desabilitados indevidamente   │
 * │ → Export: window.garantirBotoesFechaHabilitados                      │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ AUTO-EXECUÇÃO setInterval ─────────────────────────────────────────┐
 * │ setInterval(window.garantirBotoesFechaHabilitados, 1000);            │
 * │ → Executa garantirBotoesFechaHabilitados a cada 1000ms (1 segundo)  │
 * │ → Polling contínuo para manter botões de fechar sempre habilitados  │
 * │ → Inicia automaticamente ao carregar script                         │
 * │ → Não pode ser cancelado (sem clearInterval)                        │
 * └─────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO TÍPICO DE CONFIGURAÇÃO DE TÍTULO:
 * 1. exibe-viagem.js detecta modo de abertura do modal (novo/editar/viagem aberta/etc)
 * 2. Chama setModalTitle(tipo) com key apropriada (ex: 'NOVO_AGENDAMENTO')
 * 3. setModalTitle busca config em ModalConfig[tipo]
 * 4. Se htmlFunc: chama function com statusTexto, senão usa html direto
 * 5. Atualiza #Titulo.innerHTML com HTML template (inclui icon + texto)
 * 6. Fallback: tenta 4 seletores alternativos para compatibilidade
 * 7. Console.log confirma título definido
 * 8. Modal exibe título com ícone Font Awesome duotone e estilo Outfit
 *
 * 🔄 FLUXO DE RESET DE MODAL:
 * 1. Modal é fechado (close button click, backdrop click, ESC key)
 * 2. Close event handler chama resetModal()
 * 3. resetModal() executa 3 operações:
 *    a. setModalTitle('NOVO_AGENDAMENTO') - restaura título padrão
 *    b. limparCamposModalViagens() - limpa todos os campos do formulário
 *    c. inicializarCamposModal() - reinicializa Syncfusion controls
 * 4. Modal pronto para próxima abertura em estado limpo
 *
 * 🔄 FLUXO DE GARANTIA DE BOTÕES HABILITADOS:
 * 1. setInterval dispara garantirBotoesFechaHabilitados() a cada 1 segundo
 * 2. Function busca 6 seletores diferentes de botões de fechar
 * 3. Para cada elemento encontrado:
 *    a. disabled = false
 *    b. remove class 'disabled'
 *    c. pointerEvents = 'auto'
 *    d. opacity = '1'
 * 4. Garante UX consistente mesmo se código externo desabilitar botões
 * 5. Polling contínuo sem parar (workaround permanente)
 *
 * 📌 CORES DOS ÍCONES FONT AWESOME (7 configs):
 * - NOVO_AGENDAMENTO: Verde escuro (#006400) + Verde claro (#A9BA9D)
 * - EDITAR_AGENDAMENTO: Azul escuro (#002F6C) + Azul claro (#7DA2CE)
 * - AGENDAMENTO_CANCELADO: Vermelho escuro (#8B0000) + Vermelho claro (#FF4C4C)
 * - VIAGEM_ABERTA: Cores padrão Font Awesome (não especificado)
 * - VIAGEM_REALIZADA: Cores padrão Font Awesome (ícone suitcase-rolling)
 * - VIAGEM_CANCELADA: Cores padrão Font Awesome (ícone suitcase-rolling)
 * - TRANSFORMAR_VIAGEM: Azul escuro (#002F6C) + Azul claro (#7DA2CE)
 *
 * → Cores definidas via CSS custom properties: --fa-primary-color, --fa-secondary-color
 *
 * 📌 ÍCONES FONT AWESOME (3 icons):
 * - fa-calendar-lines-pen: Usado em NOVO_AGENDAMENTO, EDITAR_AGENDAMENTO,
 *   TRANSFORMAR_VIAGEM (calendário com caneta)
 * - fa-calendar-xmark: Usado em AGENDAMENTO_CANCELADO (calendário com X)
 * - fa-solid fa-suitcase-rolling: Usado em VIAGEM_ABERTA, VIAGEM_REALIZADA,
 *   VIAGEM_CANCELADA (mala de viagem)
 *
 * 📌 TÍTULOS DINÂMICOS (htmlFunc):
 * - VIAGEM_REALIZADA e VIAGEM_CANCELADA usam htmlFunc com statusTexto parameter
 * - Exemplo: setModalTitle('VIAGEM_REALIZADA', 'Em Andamento')
 *   → Título: "Exibindo Viagem (Em Andamento - Edição Não Permitida)"
 * - statusTexto padrão: 'Realizada' ou 'Cancelada' se não fornecido
 * - Span com class btn-vinho, fw-bold, fst-italic para "Edição Não Permitida"
 *
 * 📌 FALLBACK SELETORES (4 seletores):
 * 1. #modalViagens .modal-title - Selector CSS descendente (Bootstrap padrão)
 * 2. #modalViagemTitulo - ID específico alternativo
 * 3. #modalViagens .modal-header h3 - H3 dentro do header
 * 4. #modalViagens .modal-header - Header completo (busca title dentro)
 *
 * → Usado se #Titulo não existir ou para compatibilidade com múltiplas estruturas DOM
 * → Regex /<h3[^>]*>|<\/h3>/g remove tags <h3> para evitar nested headers
 *
 * 📌 DOM ELEMENTS DEPENDENCY (13+ elements):
 * - #Titulo: elemento principal para título do modal (primeira tentativa)
 * - #modalViagens: container do modal de viagens
 * - #modalViagens .modal-title: título Bootstrap padrão
 * - #modalViagemTitulo: ID alternativo para título
 * - #modalViagens .modal-header: header do modal
 * - #modalViagens .modal-header h3: h3 dentro do header
 * - #btnFecha: botão fechar (variação 1)
 * - #btnFechar: botão fechar (variação 2)
 * - #btnCancelar: botão cancelar
 * - #modalViagens .btn-close: botão close Bootstrap
 * - #modalViagens [data-bs-dismiss="modal"]: todos os botões com data-attribute dismiss
 * - .modal-footer .btn-secondary: botões secundários no footer (geralmente cancelar)
 * - .modal-title, h3, h5: elementos de título (fallback querySelectorAll)
 *
 * 📌 EXTERNAL DEPENDENCIES:
 * - window.limparCamposModalViagens: função definida em exibe-viagem.js
 * - window.inicializarCamposModal: função definida em exibe-viagem.js
 * - Alerta.TratamentoErroComLinha: função de logging de erros (frotix-core.js)
 * - Font Awesome CSS: fa-duotone, fa-solid, fa-calendar-lines-pen, fa-calendar-xmark,
 *   fa-suitcase-rolling
 * - Outfit font: Google Fonts ou arquivo local
 * - Bootstrap 5: .modal-title, .modal-header, .modal-footer, .btn-close, .btn-secondary,
 *   [data-bs-dismiss="modal"]
 * - CSS custom properties: --fa-primary-color, --fa-secondary-color
 *
 * 📌 POLLING INTERVAL:
 * - setInterval com 1000ms (1 segundo)
 * - Agressivo mas necessário para workaround de bug
 * - Sem clearInterval (polling permanente enquanto página carregada)
 * - Impacto de performance: mínimo (querySelectorAll + forEach + property setters)
 * - Alternativa: MutationObserver mais eficiente, mas polling é mais simples
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - TITLE_STYLE não exportado (escopo de arquivo, usado via template string)
 * - Font Outfit weight 700 para bold (pode precisar Google Fonts import)
 * - text-shadow 0 2px 4px rgba(0,0,0,0.2) para profundidade visual
 * - color: #fff para títulos brancos (assume modal header com fundo escuro)
 * - btn-vinho class custom do projeto (não Bootstrap)
 * - fw-bold = font-weight: bold (Bootstrap utility)
 * - fst-italic = font-style: italic (Bootstrap utility)
 * - Try-catch em todas as 3 funções exportadas
 * - Console.warn para erros não-críticos em fallback loops
 * - Console.log para confirmação de operações bem-sucedidas
 * - Alerta.TratamentoErroComLinha para erros críticos com stack trace
 * - querySelectorAll retorna NodeList (não Array), mas forEach nativo funciona
 * - classList.remove não lança erro se class não existe
 * - style setters diretos para máxima compatibilidade
 * - pointerEvents = 'auto' reverte 'none' que poderia bloquear cliques
 * - opacity = '1' reverte opacidade reduzida que poderia indicar disabled
 * - disabled = false é a propriedade mais importante (HTML attribute)
 * - Fallback seletores tentam múltiplas estruturas DOM para robustez
 * - Regex em replace usa [^>]* para capturar atributos dentro de <h3>
 * - Template strings (backticks) para htmlFunc com interpolação ${statusTexto}
 * - Arrow functions em htmlFunc para sintaxe concisa
 * - Default parameter values em htmlFunc: statusTexto = 'Realizada'/'Cancelada'
 * - aria-hidden='true' em alguns ícones para acessibilidade (screen readers)
 * - Config object pattern facilita extensão (adicionar novos tipos)
 * - Tipo field em config não usado atualmente (pode ser para lógica futura)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/**
 * Estilo inline para títulos com fonte Outfit
 */
const TITLE_STYLE = "font-family: 'Outfit', sans-serif; font-weight: 700; color: #fff; text-shadow: 0 2px 4px rgba(0,0,0,0.2);";

/**
 * Configuração EXATA dos ícones e títulos do modal (versão original)
 */
window.ModalConfig = {
    NOVO_AGENDAMENTO: {
        html: `
            <h3 class='modal-title' style="${TITLE_STYLE}">
                <i class="fa-duotone fa-calendar-lines-pen" 
                   style="--fa-primary-color: #006400; --fa-secondary-color: #A9BA9D;"></i>
                Criar Agendamento
            </h3>`,
        tipo: 'novo'
    },

    EDITAR_AGENDAMENTO: {
        html: `
            <h3 class='modal-title' style="${TITLE_STYLE}">
                <i class="fa-duotone fa-calendar-lines-pen" 
                   style="--fa-primary-color: #002F6C; --fa-secondary-color: #7DA2CE;"></i>
                Editar Agendamento
            </h3>`,
        tipo: 'editar'
    },

    AGENDAMENTO_CANCELADO: {
        html: `
            <h3 class='modal-title' style="${TITLE_STYLE}">
                <i class="fa-duotone fa-calendar-xmark" 
                   style="--fa-primary-color: #8B0000; --fa-secondary-color: #FF4C4C;"></i>
                Agendamento Cancelado
            </h3>`,
        tipo: 'cancelado'
    },

    VIAGEM_ABERTA: {
        html: `
            <h3 class='modal-title' style="${TITLE_STYLE}">
                <i class='fa-duotone fa-solid fa-suitcase-rolling' aria-hidden='true'></i> 
                Exibindo Viagem (Aberta)
            </h3>`,
        tipo: 'aberta'
    },

    VIAGEM_REALIZADA: {
        htmlFunc: (statusTexto = 'Realizada') => `
            <h3 class='modal-title' style="${TITLE_STYLE}">
                <i class='fa-duotone fa-solid fa-suitcase-rolling' aria-hidden='true'></i> 
                Exibindo Viagem (${statusTexto} - 
                <span class='btn-vinho fw-bold fst-italic'>Edição Não Permitida</span>
                )
            </h3>`,
        tipo: 'realizada'
    },

    VIAGEM_CANCELADA: {
        htmlFunc: (statusTexto = 'Cancelada') => `
            <h3 class='modal-title' style="${TITLE_STYLE}">
                <i class='fa-duotone fa-solid fa-suitcase-rolling' aria-hidden='true'></i> 
                Exibindo Viagem (${statusTexto} - 
                <span class='btn-vinho fw-bold fst-italic'>Edição Não Permitida</span>
                )
            </h3>`,
        tipo: 'cancelada'
    },

    TRANSFORMAR_VIAGEM: {
        html: `
            <h3 class='modal-title' style="${TITLE_STYLE}">
                <i class="fa-duotone fa-calendar-lines-pen" 
                   style="--fa-primary-color: #002F6C; --fa-secondary-color: #7DA2CE;"></i>
                Transformar Agendamento em Viagem
            </h3>`,
        tipo: 'transformar'
    }
};

/**
 * Define título do modal com HTML exato da versão original
 * param {string} tipo - Tipo de modal (chave do ModalConfig)
 * param {string} statusTexto - Texto de status adicional (opcional)
 */
window.setModalTitle = function (tipo, statusTexto = null)
{
    try
    {
        const config = window.ModalConfig[tipo];

        if (!config)
        {
            console.warn("⚠️ Tipo de modal não encontrado:", tipo);
            return;
        }

        // Obter HTML do título
        let tituloHtml = '';
        if (config.htmlFunc)
        {
            // Se for função (para títulos dinâmicos com statusTexto)
            tituloHtml = config.htmlFunc(statusTexto);
        } else
        {
            // Se for HTML estático
            tituloHtml = config.html;
        }

        // Atualizar usando ID "Titulo" (como no original)
        const tituloElement = document.getElementById("Titulo");
        if (tituloElement)
        {
            tituloElement.innerHTML = tituloHtml;
        }

        // Fallback: tentar outros seletores comuns
        const seletores = [
            "#modalViagens .modal-title",
            "#modalViagemTitulo",
            "#modalViagens .modal-header h3",
            "#modalViagens .modal-header"
        ];

        seletores.forEach(seletor =>
        {
            try
            {
                const elemento = document.querySelector(seletor);
                if (elemento && elemento.id !== "Titulo")
                {
                    // Se o elemento não for o "Titulo" principal, inserir dentro dele
                    if (elemento.classList.contains('modal-header'))
                    {
                        // Se for o header, buscar ou criar o elemento de título dentro dele
                        let titleEl = elemento.querySelector('.modal-title, h3, h5');
                        if (titleEl)
                        {
                            titleEl.innerHTML = tituloHtml.replace(/<h3[^>]*>|<\/h3>/g, '');
                        }
                    } else
                    {
                        elemento.innerHTML = tituloHtml.replace(/<h3[^>]*>|<\/h3>/g, '');
                    }
                }
            } catch (e)
            {
                console.warn(`Erro ao definir título em ${seletor}:`, e);
            }
        });

        console.log("📋 Título do modal definido:", tipo);

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-config.js", "setModalTitle", error);
    }
};

/**
 * Reseta modal para estado inicial
 */
window.resetModal = function ()
{
    try
    {
        window.setModalTitle('NOVO_AGENDAMENTO');
        window.limparCamposModalViagens();
        window.inicializarCamposModal();
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-config.js", "resetModal", error);
    }
};

/**
 * Garante que botões de fechar nunca sejam desabilitados
 */
window.garantirBotoesFechaHabilitados = function ()
{
    try
    {
        const seletores = [
            '#btnFecha',
            '#btnFechar',
            '#btnCancelar',
            '#modalViagens .btn-close',
            '#modalViagens [data-bs-dismiss="modal"]',
            '.modal-footer .btn-secondary'
        ];

        seletores.forEach(seletor =>
        {
            try
            {
                const elementos = document.querySelectorAll(seletor);
                elementos.forEach(el =>
                {
                    if (el)
                    {
                        el.disabled = false;
                        el.classList.remove('disabled');
                        el.style.pointerEvents = 'auto';
                        el.style.opacity = '1';
                    }
                });
            } catch (e)
            {
                console.warn(`Erro ao habilitar ${seletor}:`, e);
            }
        });

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("modal-config.js", "garantirBotoesFechaHabilitados", error);
    }
};

// Garantir que botões de fechar sempre estejam habilitados
setInterval(window.garantirBotoesFechaHabilitados, 1000);
