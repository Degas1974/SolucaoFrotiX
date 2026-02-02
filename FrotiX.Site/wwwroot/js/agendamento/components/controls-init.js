/* ****************************************************************************************
 * ⚡ ARQUIVO: controls-init.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Inicialização programática de event handlers para controles Syncfusion
 *                   do formulário de agendamento. Configura eventos (change, select, blur,
 *                   created, toolbarClick) e templates customizados (motorista com foto)
 *                   para 10 componentes diferentes. Remove eventos anteriores (=null)
 *                   antes de atribuir novos para evitar duplicação. Deve ser chamado
 *                   APÓS DOM pronto E controles renderizados.
 * 📥 ENTRADAS     : Nenhum parâmetro (função void), acessa DOM diretamente
 *                   (getElementById), referências globais (window.* callbacks), ej2_instances
 * 📤 SAÍDAS       : Event handlers configurados (change, select, blur, created, toolbarClick),
 *                   templates aplicados (itemTemplate, valueTemplate para motorista),
 *                   console.log com status de configuração (produção!), callbacks invocados
 *                   (onLstMotoristaCreated)
 * 🔗 CHAMADA POR  : DOMContentLoaded handlers, main.js inicialização, após render de
 *                   controles Syncfusion
 * 🔄 CHAMA        : document.getElementById, console.log, window.* callbacks
 *                   (lstFinalidade_Change, MotoristaValueChange, VeiculoValueChange,
 *                   onSelectRequisitante, RequisitanteValueChange, etc.),
 *                   Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 (ej2_instances), window.* callback functions
 *                   (event-handlers.js), Alerta.js, imagens (/images/barbudo.jpg)
 * 📝 OBSERVAÇÕES  : Exporta window.inicializarEventHandlersControles (função global).
 *                   Todos os eventos resetados (=null) antes de atribuir para evitar
 *                   múltiplos handlers. console.log em produção (lines 11-258). Templates
 *                   de motorista com foto circular 40x40px (item) e 30x30px (value).
 *                   lstRequisitante tem 2 eventos (select + change). lstPeriodos condicional
 *                   (só configura se window.PeriodosValueChange existir).
 *
 * 📋 ÍNDICE DE FUNÇÕES (1 função global window.*):
 *
 * ┌─ FUNÇÃO PRINCIPAL ────────────────────────────────────────────────────┐
 * │ 1. window.inicializarEventHandlersControles()                        │
 * │    → Inicializa event handlers para 10 componentes Syncfusion        │
 * │    → Para cada componente:                                           │
 * │      1. getElementById + verifica ej2_instances[0] existe            │
 * │      2. Remove evento anterior: obj.event = null                     │
 * │      3. Atribui novo evento: obj.event = function(args) { ... }      │
 * │      4. console.log status                                           │
 * │    → try-catch: Alerta.TratamentoErroComLinha                        │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * 📦 COMPONENTES CONFIGURADOS (10 componentes):
 *
 * ┌─ 1. FINALIDADE ───────────────────────────────────────────────────────┐
 * │ • ID: lstFinalidade (Syncfusion DropDownList)                        │
 * │ • Eventos: change                                                     │
 * │ • Callback: window.lstFinalidade_Change(args)                        │
 * │ • Reset: finalidadeObj.change = null antes                           │
 * │ • Log: "✅ lstFinalidade: change event configurado"                  │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 2. MOTORISTA (com templates customizados) ──────────────────────────┐
 * │ • ID: lstMotorista (Syncfusion DropDownList)                         │
 * │ • Eventos: created, change                                           │
 * │ • Callbacks:                                                          │
 * │   - created: window.onLstMotoristaCreated()                          │
 * │   - change: window.MotoristaValueChange(args)                        │
 * │ • Templates:                                                          │
 * │   - itemTemplate: div.d-flex com img (40x40px circular) + span       │
 * │   - valueTemplate: div.d-flex com img (30x30px circular) + span      │
 * │   - Imagem: data.FotoBase64 (se startsWith 'data:image')             │
 * │              senão '/images/barbudo.jpg'                             │
 * │   - onerror: fallback para '/images/barbudo.jpg'                     │
 * │   - Text: data.Nome || data.MotoristaCondutor || ''                  │
 * │ • Executa onLstMotoristaCreated() imediatamente após templates       │
 * │ • Log: "🔧 Inicializando lstMotorista...", "✅ configurado"          │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 3. VEÍCULO ──────────────────────────────────────────────────────────┐
 * │ • ID: lstVeiculo (Syncfusion DropDownList)                           │
 * │ • Eventos: change                                                     │
 * │ • Callback: window.VeiculoValueChange(args)                          │
 * │ • Reset: veiculoObj.change = null antes                              │
 * │ • Log: "✅ lstVeiculo: change event configurado"                     │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 4. REQUISITANTE (2 eventos) ────────────────────────────────────────┐
 * │ • ID: lstRequisitante (Syncfusion DropDownList)                      │
 * │ • Eventos: select, change                                            │
 * │ • Callbacks:                                                          │
 * │   - select: window.onSelectRequisitante(args) (preenche ramal/setor) │
 * │   - change: window.RequisitanteValueChange(args)                     │
 * │ • console.log: Antes/Depois de cada evento (debug)                   │
 * │ • Reset: requisitanteObj.select = null, .change = null               │
 * │ • Log: "🔧 Configurando eventos...", "✅ select e change configurados"│
 * │ • Diferença:                                                          │
 * │   - select: dispara ao selecionar item da lista                      │
 * │   - change: dispara ao mudar valor (inclusive digitação)             │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 5. REQUISITANTE EVENTO ─────────────────────────────────────────────┐
 * │ • ID: lstRequisitanteEvento (Syncfusion DropDownList)                │
 * │ • Eventos: change                                                     │
 * │ • Callback: window.RequisitanteEventoValueChange(args)               │
 * │ • Reset: requisitanteEventoObj.change = null antes                   │
 * │ • Log: "✅ lstRequisitanteEvento: change event configurado"          │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 6. DIAS DA SEMANA ───────────────────────────────────────────────────┐
 * │ • ID: lstDias (Syncfusion MultiSelect)                               │
 * │ • Eventos: blur                                                       │
 * │ • Callback: window.onBlurLstDias(args)                               │
 * │ • Reset: diasObj.blur = null antes                                   │
 * │ • Log: "✅ lstDias: blur event configurado"                          │
 * │ • Nota: usa blur (não change) para validar após seleção completa     │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 7. RICH TEXT EDITOR (Descrição) ────────────────────────────────────┐
 * │ • ID: rteDescricao (Syncfusion RichTextEditor)                       │
 * │ • Eventos: created, toolbarClick                                     │
 * │ • Callbacks:                                                          │
 * │   - created: window.onCreate() (se função existir)                   │
 * │   - toolbarClick: window.toolbarClick(args) (se função existir)      │
 * │ • Verificação condicional: if (window.onCreate) antes de atribuir    │
 * │ • Log: "✅ rteDescricao: created e toolbarClick events configurados" │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 8. RECORRENTE ───────────────────────────────────────────────────────┐
 * │ • ID: lstRecorrente (Syncfusion DropDownList)                        │
 * │ • Eventos: change                                                     │
 * │ • Callback: window.RecorrenteValueChange(args)                       │
 * │ • Reset: recorrenteObj.change = null antes                           │
 * │ • Log: "✅ lstRecorrente: change event configurado"                  │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 9. PERÍODOS (condicional) ──────────────────────────────────────────┐
 * │ • ID: lstPeriodos (Syncfusion DropDownList)                          │
 * │ • Eventos: change                                                     │
 * │ • Callback: window.PeriodosValueChange(args)                         │
 * │ • Condicional: if (window.PeriodosValueChange) antes de atribuir     │
 * │ • Nota: só configura se callback existir globalmente                 │
 * │ • Log: "✅ lstPeriodos: change event configurado" (se configurado)   │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ 10. SETOR REQUISITANTE (comentado) ─────────────────────────────────┐
 * │ • ID: ddtSetorRequisitante (comentado, linhas 171-181)               │
 * │ • Nota: estava com change="MotoristaValueChange" (provavelmente erro)│
 * │ • Deixado sem evento específico (comentado)                          │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE INICIALIZAÇÃO:
 * 1. inicializarEventHandlersControles() chamado
 * 2. console.log "🎯 Inicializando event handlers..."
 * 3. Para cada componente (10 total):
 *    a. getElementById('id')
 *    b. Verifica ej2_instances && ej2_instances[0] existe
 *    c. Se existe:
 *       - Obtém referência: obj = element.ej2_instances[0]
 *       - Remove evento anterior: obj.event = null
 *       - Atribui novo evento: obj.event = function(args) { callback(args) }
 *       - console.log status
 *    d. Se não existe: pula (nenhum erro lançado)
 * 4. console.log "✅ Todos os event handlers foram configurados!"
 * 5. try-catch global: Alerta.TratamentoErroComLinha se qualquer erro
 *
 * 🔄 FLUXO DE TEMPLATES MOTORISTA:
 * 1. Obtém referência motoristaObj = lstMotorista.ej2_instances[0]
 * 2. Define itemTemplate = function(data):
 *    a. Verifica data.FotoBase64 && startsWith('data:image')
 *    b. Se sim: imgSrc = data.FotoBase64
 *    c. Senão: imgSrc = '/images/barbudo.jpg'
 *    d. Retorna HTML: div.d-flex com img (40x40px) + span
 *    e. img onerror: this.src='/images/barbudo.jpg'
 * 3. Define valueTemplate = function(data):
 *    a. Idêntico a itemTemplate mas img 30x30px
 * 4. Executa onLstMotoristaCreated() imediatamente
 * 5. console.log "✅ lstMotorista configurado"
 *
 * 🔄 FLUXO DE EVENTO REQUISITANTE (dual events):
 * 1. Obtém referência requisitanteObj
 * 2. console.log "Antes - select/change" (valores anteriores)
 * 3. Reset: requisitanteObj.select = null, .change = null
 * 4. Atribui select: function(args) { onSelectRequisitante(args) }
 * 5. Atribui change: function(args) { RequisitanteValueChange(args) }
 * 6. console.log "Depois - select/change" (novos valores)
 * 7. console.log "✅ select e change events configurados"
 *
 * 📌 PATTERN DE RESET E ATRIBUIÇÃO:
 * - Sempre: obj.event = null ANTES de obj.event = function() { ... }
 * - Motivo: evita múltiplos handlers se função chamada repetidamente
 * - Exemplo: finalidadeObj.change = null; finalidadeObj.change = function() {}
 *
 * 📌 TEMPLATES MOTORISTA (itemTemplate vs valueTemplate):
 * - itemTemplate: renderizado em cada item da lista dropdown (40x40px)
 * - valueTemplate: renderizado no campo selecionado (30x30px)
 * - Ambos: div.d-flex.align-items-center com img circular + span
 * - Foto: data.FotoBase64 (base64 data URI) ou fallback /images/barbudo.jpg
 * - Text: data.Nome || data.MotoristaCondutor || ''
 * - onerror: this.src='/images/barbudo.jpg' (duplo fallback)
 *
 * 📌 EVENTOS CONDICIONAIS:
 * - rteDescricao: if (window.onCreate) antes de atribuir created
 * - lstPeriodos: if (window.PeriodosValueChange) antes de atribuir change
 * - Motivo: funções callback podem não existir em todos os contextos
 *
 * 📌 CALLBACKS WINDOW.* ESPERADOS:
 * 1. lstFinalidade_Change(args)
 * 2. onLstMotoristaCreated() - sem args
 * 3. MotoristaValueChange(args)
 * 4. VeiculoValueChange(args)
 * 5. onSelectRequisitante(args) - preenche ramal/setor
 * 6. RequisitanteValueChange(args)
 * 7. RequisitanteEventoValueChange(args)
 * 8. onBlurLstDias(args)
 * 9. onCreate() - RTE created
 * 10. toolbarClick(args) - RTE toolbar
 * 11. RecorrenteValueChange(args)
 * 12. PeriodosValueChange(args) - condicional
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - console.log em produção (11 + console.log por componente)
 * - Função deve ser chamada APÓS controles renderizados (não em DOMContentLoaded cedo demais)
 * - lstRequisitante único com 2 eventos (select + change)
 * - lstDias usa blur (não change) para validar após seleção completa (MultiSelect)
 * - ddtSetorRequisitante comentado (linhas 169-181) - estava com evento errado
 * - onLstMotoristaCreated() chamado imediatamente após definir templates (linha 92)
 * - Nenhum erro lançado se componente não existir (if verifica antes de acessar)
 * - try-catch global captura qualquer erro de qualquer componente
 * - Imagens motorista: object-fit: cover; border-radius: 50%; (circular)
 * - lstPeriodos condicional sugere que nem todos contextos precisam desse evento
 * - console.log emoji: 🎯 (início), 🔧 (configurando), ✅ (sucesso)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/**
 * Inicializa todos os event handlers dos controles Syncfusion
 * Deve ser chamado APÓS o DOM estar pronto E após os controles serem renderizados
 */
window.inicializarEventHandlersControles = function () {
    try {
        console.log('🎯 Inicializando event handlers dos controles...');

        // ============================================
        // FINALIDADE
        // ============================================
        const lstFinalidade = document.getElementById('lstFinalidade');
        if (lstFinalidade && lstFinalidade.ej2_instances && lstFinalidade.ej2_instances[0]) {
            const finalidadeObj = lstFinalidade.ej2_instances[0];

            // Remover eventos anteriores se existirem
            finalidadeObj.change = null;

            // Adicionar novo evento
            finalidadeObj.change = function (args) {
                if (window.lstFinalidade_Change) {
                    window.lstFinalidade_Change(args);
                }
            };

            console.log('✅ lstFinalidade: change event configurado');
        }

        // ============================================
        // MOTORISTA
        // ============================================
        const lstMotorista = document.getElementById('lstMotorista');
        if (lstMotorista && lstMotorista.ej2_instances && lstMotorista.ej2_instances[0]) {
            const motoristaObj = lstMotorista.ej2_instances[0];

            console.log('🔧 Inicializando lstMotorista...');

            // Atribuir evento created
            motoristaObj.created = function () {
                if (window.onLstMotoristaCreated) {
                    window.onLstMotoristaCreated();
                }
            };

            // Atribuir evento change
            motoristaObj.change = function (args) {
                if (window.MotoristaValueChange) {
                    window.MotoristaValueChange(args);
                }
            };

            // Aplicar templates IMEDIATAMENTE
            motoristaObj.itemTemplate = function (data) {
                if (!data) return '';

                let imgSrc = (data.FotoBase64 && data.FotoBase64.startsWith('data:image'))
                    ? data.FotoBase64
                    : '/images/barbudo.jpg';

                return `
            <div class="d-flex align-items-center">
                <img src="${imgSrc}" 
                     alt="Foto" 
                     style="height:40px; width:40px; border-radius:50%; margin-right:10px; object-fit: cover;" 
                     onerror="this.src='/images/barbudo.jpg';" />
                <span>${data.Nome || data.MotoristaCondutor || ''}</span>
            </div>`;
            };

            motoristaObj.valueTemplate = function (data) {
                if (!data) return '';

                let imgSrc = (data.FotoBase64 && data.FotoBase64.startsWith('data:image'))
                    ? data.FotoBase64
                    : '/images/barbudo.jpg';

                return `
            <div class="d-flex align-items-center">
                <img src="${imgSrc}" 
                     alt="Foto" 
                     style="height:30px; width:30px; border-radius:50%; margin-right:10px; object-fit: cover;" 
                     onerror="this.src='/images/barbudo.jpg';" />
                <span>${data.Nome || data.MotoristaCondutor || ''}</span>
            </div>`;
            };

            // Aplicar templates imediatamente
            if (window.onLstMotoristaCreated) {
                window.onLstMotoristaCreated();
            }

            console.log('✅ lstMotorista configurado');
        }

        // ============================================
        // VEÍCULO
        // ============================================
        const lstVeiculo = document.getElementById('lstVeiculo');
        if (lstVeiculo && lstVeiculo.ej2_instances && lstVeiculo.ej2_instances[0]) {
            const veiculoObj = lstVeiculo.ej2_instances[0];

            veiculoObj.change = null;
            veiculoObj.change = function (args) {
                if (window.VeiculoValueChange) {
                    window.VeiculoValueChange(args);
                }
            };

            console.log('✅ lstVeiculo: change event configurado');
        }

        // ============================================
        // REQUISITANTE
        // ============================================
        const lstRequisitante = document.getElementById('lstRequisitante');
        if (lstRequisitante && lstRequisitante.ej2_instances && lstRequisitante.ej2_instances[0]) {
            const requisitanteObj = lstRequisitante.ej2_instances[0];

            console.log('🔧 Configurando eventos do lstRequisitante...');
            console.log('   Antes - select:', requisitanteObj.select);
            console.log('   Antes - change:', requisitanteObj.change);

            // ===== EVENTO SELECT (NOVO!) =====
            // Dispara quando um item é selecionado da lista
            // Usado para preencher automaticamente ramal e setor
            requisitanteObj.select = null;
            requisitanteObj.select = function (args) {
                if (window.onSelectRequisitante) {
                    window.onSelectRequisitante(args);
                }
            };

            // ===== EVENTO CHANGE (ORIGINAL) =====
            // Dispara quando o valor do campo muda (inclusive digitação)
            requisitanteObj.change = null;
            requisitanteObj.change = function (args) {
                if (window.RequisitanteValueChange) {
                    window.RequisitanteValueChange(args);
                }
            };

            console.log('   Depois - select:', requisitanteObj.select);
            console.log('   Depois - change:', requisitanteObj.change);
            console.log('✅ lstRequisitante: select e change events configurados');
        }

        // ============================================
        // REQUISITANTE EVENTO
        // ============================================
        const lstRequisitanteEvento = document.getElementById('lstRequisitanteEvento');
        if (lstRequisitanteEvento && lstRequisitanteEvento.ej2_instances && lstRequisitanteEvento.ej2_instances[0]) {
            const requisitanteEventoObj = lstRequisitanteEvento.ej2_instances[0];

            requisitanteEventoObj.change = null;
            requisitanteEventoObj.change = function (args) {
                if (window.RequisitanteEventoValueChange) {
                    window.RequisitanteEventoValueChange(args);
                }
            };

            console.log('✅ lstRequisitanteEvento: change event configurado');
        }

        // ============================================
        // SETOR REQUISITANTE (no accordion)
        // ============================================
        //const ddtSetorRequisitante = document.getElementById('ddtSetorRequisitante');
        //if (ddtSetorRequisitante && ddtSetorRequisitante.ej2_instances && ddtSetorRequisitante.ej2_instances[0])
        //{
        //    const setorReqObj = ddtSetorRequisitante.ej2_instances[0];

        //    // NOTA: Este estava com change="MotoristaValueChange" mas provavelmente está errado
        //    // Deixando sem evento por enquanto ou você pode adicionar um específico
        //    setorReqObj.change = null;

        //    console.log('✅ ddtSetorRequisitante: inicializado (sem evento específico)');
        //}

        // ============================================
        // DIAS DA SEMANA (MultiSelect com blur)
        // ============================================
        const lstDias = document.getElementById('lstDias');
        if (lstDias && lstDias.ej2_instances && lstDias.ej2_instances[0]) {
            const diasObj = lstDias.ej2_instances[0];

            // Adicionar evento de blur
            diasObj.blur = null;
            diasObj.blur = function (args) {
                if (window.onBlurLstDias) {
                    window.onBlurLstDias(args);
                }
            };

            console.log('✅ lstDias: blur event configurado');
        }

        // ============================================
        // RICH TEXT EDITOR (Descrição)
        // ============================================
        const rteDescricao = document.getElementById('rteDescricao');
        if (rteDescricao && rteDescricao.ej2_instances && rteDescricao.ej2_instances[0]) {
            const rteObj = rteDescricao.ej2_instances[0];

            // Created event
            if (window.onCreate) {
                rteObj.created = function () {
                    window.onCreate();
                };
            }

            // ToolbarClick event
            if (window.toolbarClick) {
                rteObj.toolbarClick = function (args) {
                    window.toolbarClick(args);
                };
            }

            console.log('✅ rteDescricao: created e toolbarClick events configurados');
        }

        // ============================================
        // RECORRENTE
        // ============================================
        const lstRecorrente = document.getElementById('lstRecorrente');
        if (lstRecorrente && lstRecorrente.ej2_instances && lstRecorrente.ej2_instances[0]) {
            const recorrenteObj = lstRecorrente.ej2_instances[0];

            recorrenteObj.change = null;
            recorrenteObj.change = function (args) {
                if (window.RecorrenteValueChange) {
                    window.RecorrenteValueChange(args);
                }
            };

            console.log('✅ lstRecorrente: change event configurado');
        }

        // ============================================
        // PERÍODOS (se existir)
        // ============================================
        const lstPeriodos = document.getElementById('lstPeriodos');
        if (lstPeriodos && lstPeriodos.ej2_instances && lstPeriodos.ej2_instances[0]) {
            const periodosObj = lstPeriodos.ej2_instances[0];

            // Verificar se existe função de change para períodos
            if (window.PeriodosValueChange) {
                periodosObj.change = function (args) {
                    window.PeriodosValueChange(args);
                };
                console.log('✅ lstPeriodos: change event configurado');
            }
        }

        console.log('✅ Todos os event handlers foram configurados!');

    } catch (error) {
        Alerta.TratamentoErroComLinha("controls-init.js", "inicializarEventHandlersControles", error);
    }
};
