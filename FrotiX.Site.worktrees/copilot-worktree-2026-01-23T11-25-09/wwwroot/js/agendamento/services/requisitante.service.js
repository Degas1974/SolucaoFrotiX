/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está completamente documentado em:                         ║
 * ║  📄 Documentacao/JavaScript/requisitante.service.md                      ║
 * ║                                                                          ║
 * ║  A documentação inclui:                                                   ║
 * ║  • Visão geral da funcionalidade                                        ║
 * ║  • Explicação detalhada de cada método                                   ║
 * ║  • Interconexões com outros arquivos                                     ║
 * ║  • Exemplos de uso                                                       ║
 * ║  • Troubleshooting completo                                              ║
 * ║  • Correção crítica do fechamento automático do accordion                ║
 * ║                                                                          ║
 * ║  Última atualização: 12/01/2026                                          ║
 * ║  Versão: 1.1                                                             ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

// ====================================================================
// REQUISITANTE SERVICE - Serviço para gerenciamento de requisitantes
// ====================================================================

/* eslint-disable no-undef */
(function ()
{
    "use strict";

    // Debug: Rastrear cargas do arquivo
    window.requisitanteServiceLoadCount = (window.requisitanteServiceLoadCount || 0) + 1;
    console.log("🔄 requisitante_service.js CARREGADO - Carga #" + window.requisitanteServiceLoadCount);
    console.log("   Timestamp:", new Date().toISOString());

    // ------------------------------
    // Serviço (chamadas à API)
    // ------------------------------
    class RequisitanteService
    {
        constructor()
        {
            this.api = window.ApiClient;
        }

        /**
         * Adiciona novo requisitante
         * @param {Object} dados - Dados do requisitante
         * @returns {Promise<Object>} Resultado da operação
         */
        async adicionar(dados)
        {
            try
            {
                const response = await this.api.post('/api/Viagem/AdicionarRequisitante', dados);

                if (response.success)
                {
                    return {
                        success: true,
                        message: response.message,
                        requisitanteId: response.requisitanteid
                    };
                } else
                {
                    return {
                        success: false,
                        message: response.message || "Erro ao adicionar requisitante"
                    };
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("requisitante_service.js", "adicionar", error);
                return {
                    success: false,
                    error: error.message
                };
            }
        }

        /**
         * Lista requisitantes
         * @returns {Promise<{success:boolean,data:any[],error?:string}>}
         */
        async listar()
        {
            try
            {
                return new Promise((resolve, reject) =>
                {
                    $.ajax({
                        url: "/Viagens/Upsert?handler=AJAXPreencheListaRequisitantes",
                        method: "GET",
                        datatype: "json",
                        success: function (res)
                        {
                            const requisitantes = res.data.map(item => ({
                                RequisitanteId: item.requisitanteId,
                                Requisitante: item.requisitante
                            }));

                            resolve({
                                success: true,
                                data: requisitantes
                            });
                        },
                        error: function (jqXHR, textStatus, errorThrown)
                        {
                            const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                            Alerta.TratamentoErroComLinha("requisitante.service.js", "listar", erro);
                            reject(erro);
                        }
                    });
                });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("requisitante.service.js", "listar", error);
                return {
                    success: false,
                    error: error.message,
                    data: []
                };
            }
        }
    }

    // Instância global do serviço
    window.RequisitanteService = new RequisitanteService();

    // Flag para prevenir fechamento durante validação
    let estaValidando = false;

    // Flag para evitar duplo clique no botão Novo Requisitante
    let isProcessing = false;

    // Contador de inicializações (debug)
    let inicializacaoCount = 0;


    // ===============================================================
    // CAPTURA DE DADOS DE SETORES DO VIEWDATA
    // ===============================================================

    /**
     * Captura dados de setores já carregados nos outros controles
     */
    function capturarDadosSetores()
    {
        try
        {
            // Tentar pegar dos controles já existentes
            const lstSetorAgendamento = document.getElementById("lstSetorRequisitanteAgendamento");

            if (lstSetorAgendamento && lstSetorAgendamento.ej2_instances && lstSetorAgendamento.ej2_instances[0])
            {
                const dados = lstSetorAgendamento.ej2_instances[0].fields?.dataSource;
                if (dados && dados.length > 0)
                {
                    window.SETORES_DATA = dados;
                    console.log(`✅ Dados de setores capturados: ${dados.length} itens`);
                    return true;
                }
            }

            // Tentar do lstSetorRequisitanteEvento
            const lstSetorEvento = document.getElementById("lstSetorRequisitanteEvento");
            if (lstSetorEvento && lstSetorEvento.ej2_instances && lstSetorEvento.ej2_instances[0])
            {
                const dados = lstSetorEvento.ej2_instances[0].fields?.dataSource;
                if (dados && dados.length > 0)
                {
                    window.SETORES_DATA = dados;
                    console.log(`✅ Dados de setores capturados do evento: ${dados.length} itens`);
                    return true;
                }
            }

            console.warn("⚠️ Não foi possível capturar dados de setores");
            return false;

        } catch (error)
        {
            console.error("❌ Erro ao capturar dados de setores:", error);
            return false;
        }
    }

    // ===============================================================
    // SISTEMA DE REQUISITANTE - ACCORDION (UI)
    // ===============================================================

    /**
     * Inicializa o sistema de requisitante (chamar ao abrir o modal)
     */
    function inicializarSistemaRequisitante()
    {
        inicializacaoCount++;
        console.log(`🔄 inicializarSistemaRequisitante chamada (${inicializacaoCount}x)`);

        // PROTEÇÃO: Evitar múltiplas inicializações
        if (window.requisitanteServiceInicializado)
        {
            console.log("⚠️ Sistema já inicializado, ignorando chamada duplicada");
            return;
        }

        // Marca como inicializado IMEDIATAMENTE para evitar race conditions
        window.requisitanteServiceInicializado = true;
        console.log("📍 Marcado como inicializado. Próximas chamadas serão ignoradas.");

        // ⚠️ MODAL: Botão "Novo Requisitante" agora usa Bootstrap Modal (data-bs-toggle="modal")
        // Não precisamos mais interceptar o clique manualmente
        // configurarBotaoNovoRequisitante(); // <-- DESABILITADO: lógica de accordion removida

        // Configura botões do formulário de cadastro no modal
        configurarBotoesCadastroRequisitante();

        // ⚠️ ACCORDION REMOVIDO: Código global click listener era para accordion
        // Agora usamos modal, então não precisamos mais desse listener complexo
        /*
        // Remove listener global antigo (se existir)
        if (window.globalClickListener)
        {
            document.removeEventListener("click", window.globalClickListener, true);
            console.log("🗑️ Listener global antigo removido");
        }

        // Cria função nomeada para o listener global
        // BLOQUEIO SELETIVO: Apenas botão btnRequisitante e elementos do accordion
        window.globalClickListener = function (e)
        {
            if (!estaValidando) return;

            // Permitir cliques no SweetAlert
            if (e.target.closest('.swal2-container') ||
                e.target.classList.contains('swal2-container'))
            {
                return; // ✅ SweetAlert pode funcionar normalmente
            }

            // Bloquear apenas: btnRequisitante e elementos do accordion
            const btnRequisitante = document.getElementById('btnRequisitante');
            const accordionRequisitante = document.getElementById('accordionRequisitante');

            const clickedBtn = e.target === btnRequisitante ||
                (btnRequisitante && btnRequisitante.contains(e.target));

            const clickedAccordion = accordionRequisitante &&
                (e.target === accordionRequisitante ||
                    accordionRequisitante.contains(e.target));

            if (clickedBtn || clickedAccordion)
            {
                console.log("🛑 Click bloqueado durante validação no:",
                    clickedBtn ? "botão" : "accordion");
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
            }
        };

        // Adiciona listener global para prevenir fechamento durante validação
        document.addEventListener("click", window.globalClickListener, true);
        console.log("✅ Listener global adicionado");
        console.log("🔍 window.globalClickListener referência:", window.globalClickListener ? "EXISTE" : "NULL");
        console.log("🔍 Tipo:", typeof window.globalClickListener);
        */

        console.log("✅ Sistema de Requisitante inicializado!");
    }

    /**
     * Configura o botão "Novo Requisitante" (toggle)
     */
    function configurarBotaoNovoRequisitante()
    {
        console.log("🔧 Configurando botão Novo Requisitante...");
        const btnRequisitante = document.getElementById("btnRequisitante");

        if (!btnRequisitante)
        {
            console.error("❌ btnRequisitante NÃO ENCONTRADO no DOM!");
            return;
        }

        console.log("✅ btnRequisitante encontrado:", btnRequisitante);

        // Remove listeners anteriores clonando o botão
        const novoBotao = btnRequisitante.cloneNode(true);
        btnRequisitante.parentNode.replaceChild(novoBotao, btnRequisitante);

        // Adiciona listener (TOGGLE) - fase de captura
        novoBotao.addEventListener("click", function (e)
        {
            console.log("🖱️ ========================================");
            console.log("🖱️ CLIQUE NO btnRequisitante DETECTADO!");
            console.log("🖱️ ========================================");
            console.log("   - estaValidando:", estaValidando);
            console.log("   - isProcessing:", isProcessing);
            console.log("   - Event:", e);
            console.log("   - Target:", e.target);

            // Ignorar se está validando
            if (estaValidando)
            {
                console.log("⏸️ Validação em andamento, ignorando clique");
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                return false;
            }

            if (isProcessing)
            {
                console.log("⏸️ Já processando, ignorando clique");
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                return false;
            }

            isProcessing = true;

            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();

            const sectionCadastro = document.getElementById("sectionCadastroRequisitante");

            if (!sectionCadastro)
            {
                console.error("❌ sectionCadastroRequisitante NÃO ENCONTRADO!");
                isProcessing = false;
                return false;
            }

            console.log("✅ sectionCadastroRequisitante encontrado:", sectionCadastro);
            console.log("   - style.display atual:", sectionCadastro.style.display);

            // TOGGLE
            const estaOculto = (sectionCadastro.style.display === "none" || !sectionCadastro.style.display);
            console.log("   - estaOculto:", estaOculto);

            if (estaOculto)
            {
                console.log("🆕 ========================================");
                console.log("🆕 ABRINDO FORMULÁRIO DE REQUISITANTE");
                console.log("🆕 ========================================");
                abrirFormularioCadastroRequisitante();

                setTimeout(() =>
                {
                    isProcessing = false;
                }, 300);
            } else
            {
                console.log("➖ Fechando formulário de cadastro de requisitante");
                fecharFormularioCadastroRequisitante();
                setTimeout(() => { isProcessing = false; }, 300);
            }

            return false;
        }, true); // capture

        console.log("✅ Botão Novo Requisitante configurado (modo TOGGLE)");
    }

    /**
     * Abre o modal de cadastro de requisitante
     */
    function abrirFormularioCadastroRequisitante()
    {
        try
        {
            console.log("🆕 ABRINDO modal de requisitante...");

            // 1) Limpa campos antes de abrir
            limparCamposCadastroRequisitante();

            // 2) Abre o modal Bootstrap
            const modalElement = document.getElementById('modalNovoRequisitante');
            if (!modalElement) {
                console.error("❌ Modal modalNovoRequisitante não encontrado no DOM");
                return;
            }

            // Garantir que o modal pai (modalViagens) NÃO será fechado
            // Definir z-index do novo modal para ficar acima
            const modalViagens = document.getElementById('modalViagens');
            if (modalViagens) {
                console.log("🔓 Garantindo que modalViagens permanece aberto...");
                // Não fazer nada com modalViagens - deixar aberto
            }

            // Criar ou obter instância do modal
            let modalInstance = bootstrap.Modal.getInstance(modalElement);
            if (!modalInstance) {
                modalInstance = new bootstrap.Modal(modalElement, {
                    backdrop: 'static', // Backdrop estático para evitar fechar ao clicar fora acidentalmente
                    keyboard: false     // Evitar fechar com ESC para não fechar o pai junto
                });
            }

            // Abrir o modal
            modalInstance.show();
            
            // 🔥 CORREÇÃO DE Z-INDEX PARA MODAIS EMPILHADOS
            // O modal pai (modalViagens) tem z-index padrão (1055).
            // O novo modal precisa ser maior. E o backdrop dele também.
            setTimeout(() => {
                // Ajustar z-index do modal filho
                modalElement.style.zIndex = '1060';
                
                // Ajustar z-index do backdrop do modal filho (o último backdrop criado)
                const backdrops = document.querySelectorAll('.modal-backdrop');
                if (backdrops.length > 1) {
                    const ultimoBackdrop = backdrops[backdrops.length - 1];
                    ultimoBackdrop.style.zIndex = '1059'; // Acima do modal pai (1055), abaixo do filho (1060)
                }
            }, 150); // Pequeno delay para garantir que o Bootstrap criou o backdrop

            console.log("✅ Modal de Novo Requisitante aberto (Stacking corrigido)");

            // 3) CRÍTICO: Destruir e recriar ddtSetorNovoRequisitante após modal abrir
            // Syncfusion não renderiza popup corretamente quando controle é criado com display:none
            modalElement.addEventListener('shown.bs.modal', function inicializarDropdown() {
                setTimeout(() =>
                {
                    const ddtSetor = document.getElementById("ddtSetorNovoRequisitante");

                if (!ddtSetor)
                {
                    console.error("❌ ddtSetorNovoRequisitante não encontrado no DOM");
                    return;
                }

                console.log("🔍 ddtSetorNovoRequisitante encontrado, iniciando recriação...");

                // Capturar dados de setores se ainda não existirem
                if (!window.SETORES_DATA || window.SETORES_DATA.length === 0)
                {
                    console.log("📦 Capturando dados de setores...");
                    const capturado = capturarDadosSetores();

                    if (!capturado || !window.SETORES_DATA || window.SETORES_DATA.length === 0)
                    {
                        console.error("❌ Não foi possível capturar dados de setores!");
                        console.error("   window.SETORES_DATA:", window.SETORES_DATA);
                        Alerta.Warning(
                            "Atenção",
                            "Não foi possível carregar a lista de setores. Por favor, recarregue a página.",
                            "OK"
                        );
                        return;
                    }
                }

                console.log(`📦 Dados de setores disponíveis: ${window.SETORES_DATA?.length || 0} itens`);

                // Destruir instância antiga se existir
                if (ddtSetor.ej2_instances && ddtSetor.ej2_instances[0])
                {
                    console.log("🗑️ Destruindo instância antiga de ddtSetorNovoRequisitante...");
                    try
                    {
                        ddtSetor.ej2_instances[0].destroy();
                    }
                    catch (error)
                    {
                        console.warn("⚠️ Erro ao destruir instância antiga:", error);
                    }
                }

                // Recriar o controle
                console.log("🔧 Recriando ddtSetorNovoRequisitante...");

                try
                {
                    const novoDropdown = new ej.dropdowns.DropDownTree({
                        fields: {
                            dataSource: window.SETORES_DATA || [],
                            value: 'SetorSolicitanteId',
                            text: 'Nome',
                            parentValue: 'SetorPaiId',
                            hasChildren: 'HasChild'
                        },
                        allowFiltering: true,
                        placeholder: 'Selecione o setor...',
                        sortOrder: 'Ascending',
                        showCheckBox: false,
                        filterType: 'Contains',
                        filterBarPlaceholder: 'Procurar...',
                        popupHeight: '200px',
                        popupWidth: '100%',

                        // 🔥 EVENTOS CRÍTICOS PARA GARANTIR BOA EXPERIÊNCIA NO MODAL
                        open: function(args) {
                            console.log("🔓 DropDownTree ABERTO (popup)");
                            // Garantir z-index correto do popup
                            if (args && args.popup && args.popup.element) {
                                args.popup.element.style.zIndex = '1060'; // Acima do modal (1055)
                            }
                        },

                        select: function(args) {
                            console.log("✅ Item SELECIONADO no DropDownTree:", args.nodeData?.text);
                            // Prevenir propagação que pode disparar fechamento
                            if (args.event) {
                                args.event.stopPropagation();
                            }
                        },

                        blur: function(args) {
                            console.log("👁️ DropDownTree BLUR (perdeu foco)");
                            // Não fechar accordion ao perder foco
                        },

                        close: function(args) {
                            console.log("🔒 DropDownTree FECHADO (popup)");
                            // Modal permanece aberto naturalmente - não precisa forçar reabertura
                        },

                        created: function() {
                            console.log("✅ DropDownTree CREATED disparado");
                        },

                        dataBound: function() {
                            console.log("✅ DropDownTree DATA BOUND disparado");
                            console.log(`   Total de itens: ${this.treeData?.length || 0}`);
                        }
                    });

                    novoDropdown.appendTo(ddtSetor);

                    console.log(`✅ ddtSetorNovoRequisitante recriado - ${window.SETORES_DATA?.length || 0} itens carregados`);
                    console.log("🔍 Instância criada:", novoDropdown);
                }
                catch (error)
                {
                    console.error("❌ Erro ao criar DropDownTree:", error);
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "abrirFormularioCadastroRequisitante - criar dropdown", error);
                }

                }, 100);

                // Remover listener após executar uma vez
                modalElement.removeEventListener('shown.bs.modal', inicializarDropdown);
            }, { once: true });

            console.log("✅ Modal de cadastro de requisitante sendo aberto");
        } catch (error)
        {
            console.error("❌ Erro ao abrir modal:", error);
            Alerta.TratamentoErroComLinha("requisitante.service.js", "abrirFormularioCadastroRequisitante", error);
        }
    }

    /**
     * Fecha o modal de cadastro de requisitante
     */
    function fecharFormularioCadastroRequisitante()
    {
        try
        {
            console.log("➖ Fechando modal de cadastro de requisitante");

            const modalElement = document.getElementById('modalNovoRequisitante');
            if (modalElement) {
                const modalInstance = bootstrap.Modal.getInstance(modalElement);
                if (modalInstance) {
                    modalInstance.hide();
                    console.log("✅ Modal fechado via Bootstrap");
                }
            }

            // Reset da flag de processamento
            isProcessing = false;

            console.log("✅ Modal fechado");
        } catch (error)
        {
            console.error("❌ Erro ao fechar modal:", error);
            Alerta.TratamentoErroComLinha("requisitante.service.js", "fecharFormularioCadastroRequisitante", error);
        }
    }

    /**
     * Limpa os campos do formulário de cadastro de requisitante
     */
    function limparCamposCadastroRequisitante()
    {
        try
        {
            console.log("🧹 Limpando campos do formulário de requisitante");
            console.log("   Stack trace:", new Error().stack);

            // Campos de texto simples
            const txtPonto = document.getElementById("txtPonto");
            const txtNome = document.getElementById("txtNome");
            const txtRamal = document.getElementById("txtRamal");
            const txtEmail = document.getElementById("txtEmail");

            if (txtPonto) txtPonto.value = "";
            if (txtNome) txtNome.value = "";
            if (txtRamal) txtRamal.value = "";
            if (txtEmail) txtEmail.value = "";

            // Dropdown de Setor
            const ddtSetor = document.getElementById("ddtSetorNovoRequisitante");
            console.log("🔍 ddtSetorNovoRequisitante:", ddtSetor ? "encontrado" : "NÃO ENCONTRADO");

            if (ddtSetor)
            {
                console.log("🔍 ej2_instances:", ddtSetor.ej2_instances ? "existe" : "NÃO EXISTE");

                if (ddtSetor.ej2_instances && ddtSetor.ej2_instances[0])
                {
                    const dropdown = ddtSetor.ej2_instances[0];
                    console.log(`🔍 DataSource: ${dropdown.fields?.dataSource?.length || 0} itens`);
                    console.log("🔍 Campos configurados:", {
                        value: dropdown.fields.value,
                        text: dropdown.fields.text,
                        parentValue: dropdown.fields.parentValue,
                        hasChildren: dropdown.fields.hasChildren
                    });
                    console.log("🔍 Primeiros 3 itens:", dropdown.fields?.dataSource?.slice(0, 3));

                    dropdown.value = null;
                    dropdown.dataBind();
                    console.log("✅ ddtSetorNovoRequisitante limpo");
                } else
                {
                    console.warn("⚠️ ddtSetorNovoRequisitante não está inicializado");
                }
            }

            console.log("✅ Campos limpos");
        } catch (error)
        {
            console.error("❌ Erro ao limpar campos:", error);
        }
    }

    /**
     * Configura validação do campo Ponto
     */
    function configurarValidacaoPonto()
    {
        const txtPonto = document.getElementById("txtPonto");
        if (!txtPonto)
        {
            console.warn("⚠️ txtPonto não encontrado");
            return;
        }

        // Remove listeners anteriores
        const novoCampo = txtPonto.cloneNode(true);
        txtPonto.parentNode.replaceChild(novoCampo, txtPonto);

        // Adiciona validação no blur (lostfocus)
        novoCampo.addEventListener("blur", function(e)
        {
            try
            {
                let valor = novoCampo.value.trim();

                if (!valor)
                {
                    return; // Campo vazio, não valida
                }

                // Verificar tamanho máximo (50 caracteres conforme banco)
                if (valor.length > 50)
                {
                    Alerta.Warning(
                        "Atenção",
                        "O Ponto não pode ter mais de 50 caracteres. Será truncado.",
                        "OK"
                    );
                    valor = valor.substring(0, 50);
                }

                // Verificar se começa com "p_" (minúsculo)
                if (valor.toLowerCase().startsWith("p_"))
                {
                    // Se começa com P_ (maiúsculo), converter para p_
                    if (valor.startsWith("P_"))
                    {
                        valor = "p_" + valor.substring(2);
                        console.log("✅ P_ convertido para p_");
                    }
                    // Se já está correto (p_), não faz nada
                }
                else
                {
                    // Não começa com p_ nem P_ - adicionar p_
                    valor = "p_" + valor;
                    console.log("✅ p_ adicionado ao início");
                }

                // Verificar novamente tamanho após adicionar p_
                if (valor.length > 50)
                {
                    Alerta.Warning(
                        "Atenção",
                        "O Ponto não pode ter mais de 50 caracteres (incluindo 'p_'). Será truncado.",
                        "OK"
                    );
                    valor = valor.substring(0, 50);
                }

                // Atualizar campo
                novoCampo.value = valor;

            }
            catch (error)
            {
                console.error("❌ Erro na validação do Ponto:", error);
                Alerta.TratamentoErroComLinha("requisitante.service.js", "configurarValidacaoPonto", error);
            }
        });

        console.log("✅ Validação de Ponto configurada");
    }

    /**
     * Converte string para Camel Case
     * @param {string} str - String para converter
     * @returns {string} String em Camel Case
     */
    function toCamelCase(str)
    {
        const conectores = ['de', 'da', 'do', 'das', 'dos', 'e'];
        return str
            .toLowerCase()
            .split(' ')
            .filter(palavra => palavra.length > 0)
            .map((palavra, index) =>
            {
                // Primeira palavra sempre em Camel Case, demais verificar se é conector
                if (index === 0 || !conectores.includes(palavra)) {
                    return palavra.charAt(0).toUpperCase() + palavra.slice(1);
                }
                return palavra;
            })
            .join(' ');
    }

    /**
     * Remove caracteres inválidos do nome e limita a 80 caracteres
     * @param {string} valor - Valor para sanitizar
     * @returns {string} Valor sanitizado
     */
    function sanitizeNomeCompleto(valor)
    {
        // Remove tudo exceto letras Unicode, números e espaços
        let limpo = valor.replace(/[^\p{L}\p{N} ]+/gu, '');
        if (limpo.length > 80) {
            limpo = limpo.substring(0, 80);
        }
        return limpo;
    }

    /**
     * Configura validações de Email, Ramal e Nome (padrão Usuarios/Upsert)
     */
    function configurarValidacoesRequisitante()
    {
        // =====================================================
        // VALIDAÇÃO: Ramal - apenas números (máx 8 dígitos, começa com 1-9)
        // =====================================================
        const txtRamal = document.getElementById("txtRamal");
        if (txtRamal)
        {
            // Remove listeners anteriores
            const novoRamal = txtRamal.cloneNode(true);
            txtRamal.parentNode.replaceChild(novoRamal, txtRamal);

            novoRamal.addEventListener("input", function()
            {
                try
                {
                    let valor = novoRamal.value.replace(/\D/g, '');
                    valor = valor.substring(0, 8);
                    novoRamal.value = valor;
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtRamal.input", error);
                }
            });

            novoRamal.addEventListener("blur", function()
            {
                try
                {
                    const valor = novoRamal.value.trim();
                    const regex = /^[1-9]\d{7}$/; // 8 dígitos começando com 1-9

                    if (valor && !regex.test(valor))
                    {
                        novoRamal.classList.add('is-invalid');
                    }
                    else
                    {
                        novoRamal.classList.remove('is-invalid');
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtRamal.blur", error);
                }
            });

            console.log("✅ Validação de Ramal configurada");
        }

        // =====================================================
        // VALIDAÇÃO: Email obrigatoriamente terminando em @camara.leg.br
        // =====================================================
        const txtEmail = document.getElementById("txtEmail");
        if (txtEmail)
        {
            // Remove listeners anteriores
            const novoEmail = txtEmail.cloneNode(true);
            txtEmail.parentNode.replaceChild(novoEmail, txtEmail);

            novoEmail.addEventListener("blur", function()
            {
                try
                {
                    let valor = novoEmail.value.trim().toLowerCase();

                    if (valor)
                    {
                        // Remove @camara.leg.br se já existir
                        valor = valor.replace(/@camara\.leg\.br$/i, '');
                        // Remove qualquer @ que possa existir
                        valor = valor.replace(/@/g, '');
                        // Remove caracteres inválidos (permite: letras, números, ponto, hífen, underscore)
                        valor = valor.replace(/[^a-z0-9._-]/g, '');

                        if (valor.length > 0)
                        {
                            // Adiciona domínio obrigatório
                            valor = valor + '@camara.leg.br';
                        }
                        else
                        {
                            valor = '';
                        }

                        novoEmail.value = valor;

                        // Valida formato final
                        const regex = /^[a-z0-9._-]+@camara\.leg\.br$/;
                        if (valor && !regex.test(valor))
                        {
                            novoEmail.classList.add('is-invalid');
                        }
                        else
                        {
                            novoEmail.classList.remove('is-invalid');
                        }
                    }
                    else
                    {
                        novoEmail.classList.add('is-invalid');
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtEmail.blur", error);
                }
            });

            novoEmail.addEventListener("input", function()
            {
                try
                {
                    // Converte para minúsculo
                    let valor = novoEmail.value.toLowerCase();
                    // Remove tudo que não é letra, número, ponto, hífen, underscore ou @
                    valor = valor.replace(/[^a-z0-9._@-]/g, '');

                    // Limita a 1 @
                    const numArrobas = (valor.match(/@/g) || []).length;
                    if (numArrobas > 1)
                    {
                        const partes = valor.split('@');
                        valor = partes[0] + '@' + partes.slice(1).join('');
                    }

                    novoEmail.value = valor;
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtEmail.input", error);
                }
            });

            console.log("✅ Validação de Email configurada");
        }

        // =====================================================
        // VALIDAÇÃO: Nome obrigatório em Camel Case
        // =====================================================
        const txtNome = document.getElementById("txtNome");
        if (txtNome)
        {
            // Remove listeners anteriores
            const novoNome = txtNome.cloneNode(true);
            txtNome.parentNode.replaceChild(novoNome, txtNome);

            // INPUT: Remove caracteres inválidos e limita a 80 chars
            novoNome.addEventListener("input", function()
            {
                try
                {
                    novoNome.value = sanitizeNomeCompleto(novoNome.value);
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtNome.input", error);
                }
            });

            // BLUR: Converte para Camel Case e valida se não está vazio
            novoNome.addEventListener("blur", function()
            {
                try
                {
                    const valor = sanitizeNomeCompleto(novoNome.value.trim());
                    if (valor)
                    {
                        novoNome.value = toCamelCase(valor);
                        novoNome.classList.remove('is-invalid');
                    }
                    else
                    {
                        novoNome.classList.add('is-invalid');
                    }
                }
                catch (error)
                {
                    Alerta.TratamentoErroComLinha("requisitante.service.js", "txtNome.blur", error);
                }
            });

            console.log("✅ Validação de Nome configurada");
        }
    }

    /**
     * Configura os botões do formulário de cadastro de requisitante
     */
    function configurarBotoesCadastroRequisitante()
    {
        // ===== CONFIGURAR VALIDAÇÃO DO CAMPO PONTO =====
        configurarValidacaoPonto();

        // ===== CONFIGURAR VALIDAÇÕES DE RAMAL, EMAIL E NOME =====
        configurarValidacoesRequisitante();

        // ===== BOTÃO SALVAR =====
        const btnSalvarRequisitante = document.getElementById("btnInserirRequisitante");
        if (btnSalvarRequisitante)
        {
            // Remove listeners anteriores
            const novoBotaoSalvar = btnSalvarRequisitante.cloneNode(true);
            btnSalvarRequisitante.parentNode.replaceChild(novoBotaoSalvar, btnSalvarRequisitante);

            // Adiciona novo listener
            novoBotaoSalvar.addEventListener("click", function (e)
            {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                salvarNovoRequisitante();
            }, true);

            console.log("✅ Botão Salvar configurado");
        } else
        {
            console.warn("⚠️ btnInserirRequisitante não encontrado");
        }

        // ⚠️ MODAL: Botão "Cancelar Operação" no modal usa data-bs-dismiss="modal"
        // Não precisamos configurar listener manualmente - Bootstrap gerencia isso
        /*
        // ===== BOTÃO FECHAR =====
        const btnCancelarRequisitante = document.getElementById("btnFecharAccordionRequisitante");
        if (btnCancelarRequisitante)
        {
            // Remove listeners anteriores
            const novoBotaoFechar = btnCancelarRequisitante.cloneNode(true);
            btnCancelarRequisitante.parentNode.replaceChild(novoBotaoFechar, btnCancelarRequisitante);

            // Adiciona novo listener
            novoBotaoFechar.addEventListener("click", function (e)
            {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                fecharFormularioCadastroRequisitante();
                limparCamposCadastroRequisitante();
            }, true);

            console.log("✅ Botão Fechar configurado");
        } else
        {
            console.warn("⚠️ btnFecharAccordionRequisitante não encontrado");
        }
        */

        console.log("✅ Botões configurados com estilos padrão");
    }

    /**
     * Salva o novo requisitante chamando a API via AJAX
     */
    function salvarNovoRequisitante()
    {
        try
        {
            console.log("💾 Iniciando salvamento de requisitante.");

            // ===== OBTER CAMPOS =====
            const txtPonto = document.getElementById("txtPonto");
            const txtNome = document.getElementById("txtNome");
            const txtRamal = document.getElementById("txtRamal");
            const txtEmail = document.getElementById("txtEmail");
            // ATUALIZADO: Usar campo oculto do TreeView em vez do DropDownTree antigo
            const hiddenSetorId = document.getElementById("hiddenSetorId");

            // ===== VALIDAÇÕES =====
            console.log("🔍 Iniciando validações - ativando flag estaValidando");
            estaValidando = true;

            if (!txtPonto || !txtPonto.value.trim())
            {
                console.log("❌ Validação falhou: Ponto obrigatório");

                // Agendar desativação da flag ANTES de mostrar alerta
                const resetTimer = setTimeout(() =>
                {
                    estaValidando = false;
                    console.log("✅ Flag estaValidando desativada (timeout Ponto)");
                }, 2000);

                Alerta.Alerta("Atenção", "O Ponto é obrigatório!");
                if (txtPonto) txtPonto.focus();
                return;
            }

            if (!txtNome || !txtNome.value.trim())
            {
                console.log("❌ Validação falhou: Nome obrigatório");

                const resetTimer = setTimeout(() =>
                {
                    estaValidando = false;
                    console.log("✅ Flag estaValidando desativada (timeout Nome)");
                }, 2000);

                Alerta.Alerta("Atenção", "O Nome é obrigatório!");
                if (txtNome) txtNome.focus();
                return;
            }

            if (!txtRamal || !txtRamal.value.trim())
            {
                console.log("❌ Validação falhou: Ramal obrigatório");

                const resetTimer = setTimeout(() =>
                {
                    estaValidando = false;
                    console.log("✅ Flag estaValidando desativada (timeout Ramal)");
                }, 2000);

                Alerta.Alerta("Atenção", "O Ramal é obrigatório!");
                if (txtRamal) txtRamal.focus();
                return;
            }

            // ATUALIZADO: Obter valor do campo oculto preenchido pelo TreeView
            let setorValue = null;
            if (hiddenSetorId)
            {
                setorValue = hiddenSetorId.value;
                console.log("🔍 Validando hiddenSetorId (TreeView):");
                console.log("  - Valor:", setorValue);
            } else
            {
                console.error("❌ hiddenSetorId não encontrado no DOM!");
            }

            if (!setorValue || setorValue.trim() === "")
            {
                console.log("❌ Validação falhou: Setor obrigatório");

                const resetTimer = setTimeout(() =>
                {
                    estaValidando = false;
                    console.log("✅ Flag estaValidando desativada (timeout Setor)");
                }, 2000);

                Alerta.Alerta("Atenção", "O Setor do Requisitante é obrigatório!");
                return;
            }

            // Validações passaram
            console.log("✅ Todas as validações passaram");
            estaValidando = false;

            // ===== MONTAR OBJETO =====
            const objRequisitante = {
                Nome: txtNome.value.trim(),
                Ponto: txtPonto.value.trim(),
                Ramal: parseInt(txtRamal.value.trim()),
                Email: txtEmail ? txtEmail.value.trim() : "",
                SetorSolicitanteId: setorValue.toString()
            };

            console.log("📦 Dados coletados:", objRequisitante);

            // ===== CHAMAR API VIA AJAX =====
            $.ajax({
                type: "POST",
                url: "/api/Viagem/AdicionarRequisitante",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(objRequisitante),
                success: function (data)
                {
                    try
                    {
                        if (data.success)
                        {
                            console.log("✅ Requisitante adicionado com sucesso!");
                            console.log("📦 Resposta da API:", data);

                            if (typeof AppToast !== 'undefined')
                            {
                                AppToast.show('Verde', data.message);
                            } else if (typeof toastr !== 'undefined')
                            {
                                toastr.success(data.message);
                            }

                            // ===== ATUALIZAR DROPDOWN lstRequisitante =====
                            const comboRequisitante = getRequisitanteCombo();
                            if (comboRequisitante)
                            {
                                const novoItem = {
                                    RequisitanteId: data.requisitanteid,
                                    Requisitante: txtNome.value.trim() + " - " + txtPonto.value.trim()
                                };

                                console.log("📦 Novo requisitante a ser adicionado:", novoItem);

                                // Obter dataSource atual (Telerik)
                                let dataSource = comboRequisitante.dataSource.data() || [];

                                if (!Array.isArray(dataSource))
                                {
                                    dataSource = [];
                                }

                                // Verificar se já existe
                                const jaExiste = dataSource.some(item => item.RequisitanteId === data.requisitanteid);

                                if (!jaExiste)
                                {
                                    // Adiciona o novo item
                                    dataSource.push(novoItem);
                                    console.log("📦 Novo item adicionado ao array");

                                    // Ordena alfabeticamente por nome do requisitante (case-insensitive)
                                    dataSource.sort((a, b) => {
                                        const nomeA = (a.Requisitante || '').toString().toLowerCase();
                                        const nomeB = (b.Requisitante || '').toString().toLowerCase();
                                        return nomeA.localeCompare(nomeB, 'pt-BR');
                                    });
                                    console.log("🔄 Lista ordenada alfabeticamente");

                                    // Atualiza dataSource (Telerik usa setDataSource)
                                    comboRequisitante.setDataSource(dataSource);

                                    console.log("✅ Lista atualizada e ordenada com sucesso");
                                }
                                else
                                {
                                    console.log("⚠️ Requisitante já existe na lista");
                                }

                                // Seleciona o novo requisitante (Telerik)
                                comboRequisitante.value(data.requisitanteid);

                                console.log("✅ Requisitante selecionado:", data.requisitanteid);
                            }

                            // ===== ATUALIZAR RAMAL =====
                            // txtRamalRequisitanteSF é um input HTML simples, não Syncfusion
                            const txtRamalRequisitanteSF = document.getElementById("txtRamalRequisitanteSF");
                            if (txtRamalRequisitanteSF)
                            {
                                txtRamalRequisitanteSF.value = txtRamal.value.trim();
                                console.log("✅ Campo Ramal atualizado:", txtRamal.value.trim());
                            }

                            // ===== ATUALIZAR SETOR =====
                            const lstSetorRequisitanteAgendamento = document.getElementById("lstSetorRequisitanteAgendamento");
                            if (lstSetorRequisitanteAgendamento && lstSetorRequisitanteAgendamento.ej2_instances && lstSetorRequisitanteAgendamento.ej2_instances[0])
                            {
                                const comboSetor = lstSetorRequisitanteAgendamento.ej2_instances[0];
                                console.log("🔍 Atualizando Setor:");
                                console.log("  - setorValue (closure):", setorValue);
                                console.log("  - Tipo:", typeof setorValue);

                                // DropDownTree espera array como value
                                comboSetor.value = [setorValue.toString()];
                                comboSetor.dataBind();
                                console.log("✅ Campo Setor atualizado para:", setorValue);
                            } else
                            {
                                console.error("❌ lstSetorRequisitanteAgendamento não encontrado ou não é Syncfusion");
                            }

                            // ===== FECHAR MODAL =====
                            const modalNovoRequisitante = bootstrap.Modal.getInstance(document.getElementById('modalNovoRequisitante'));
                            if (modalNovoRequisitante)
                            {
                                modalNovoRequisitante.hide();
                                console.log("✅ Modal fechado");
                            }
                            limparCamposCadastroRequisitante();

                        } else
                        {
                            console.error("❌ Erro ao adicionar requisitante:", data.message);

                            if (typeof AppToast !== 'undefined')
                            {
                                AppToast.show('Vermelho', data.message);
                            } else if (typeof toastr !== 'undefined')
                            {
                                toastr.error(data.message);
                            } else
                            {
                                Alerta.Erro("Atenção", data.message);
                            }
                        }
                    } catch (error)
                    {
                        console.error("❌ Erro no callback de sucesso:", error);
                        Alerta.TratamentoErroComLinha(
                            "requisitante_service.js",
                            "salvarNovoRequisitante.ajax.success",
                            error
                        );
                    }
                },
                error: function (jqXHR, textStatus, errorThrown)
                {
                    try
                    {
                        console.error("❌ Erro na requisição AJAX:", textStatus, errorThrown);
                        console.error("Resposta:", jqXHR.responseText);

                        Alerta.Erro("Atenção", "Erro ao adicionar requisitante. Verifique se já existe um requisitante com este ponto/nome!");

                        Alerta.TratamentoErroComLinha(
                            "requisitante_service.js",
                            "salvarNovoRequisitante.ajax.error",
                            new Error(textStatus + ": " + errorThrown)
                        );
                    } catch (error)
                    {
                        console.error("❌ Erro no callback de erro:", error);
                    }
                }
            });

        } catch (error)
        {
            estaValidando = false;
            console.error("❌ Erro ao salvar requisitante:", error);
            Alerta.TratamentoErroComLinha("requisitante_service.js", "salvarNovoRequisitante", error);
        }
    }

    /**
     * Reseta o sistema de requisitante ao fechar o modal
     * Permite que seja reinicializado na próxima abertura
     */
    function resetarSistemaRequisitante()
    {
        console.log("🔄 Resetando sistema de requisitante...");

        // Resetar flag de inicialização
        window.requisitanteServiceInicializado = false;

        // Fechar accordion se estiver aberto
        fecharFormularioCadastroRequisitante();

        // Limpar campos
        limparCamposCadastroRequisitante();

        // Desconectar MutationObserver se existir
        if (window.__accordionObserver)
        {
            window.__accordionObserver.disconnect();
            window.__accordionObserver = null;
        }

        console.log("✅ Sistema de requisitante resetado");
    }

    /**
     * Inicializa o DropDownTree quando o modal é exibido
     */
    function inicializarDropDownTreeModal()
    {
        console.log("🔧 Inicializando DropDownTree no modal...");

        const modalRequisitante = document.getElementById("modalNovoRequisitante");
        if (!modalRequisitante)
        {
            console.warn("⚠️ modalNovoRequisitante não encontrado");
            return;
        }

        // Listener para quando o modal for completamente exibido
        modalRequisitante.addEventListener('shown.bs.modal', function ()
        {
            console.log("📢 Modal mostrado - inicializando DropDownTree...");

            const ddtSetor = document.getElementById("ddtSetorNovoRequisitante");
            if (!ddtSetor)
            {
                console.error("❌ ddtSetorNovoRequisitante não encontrado no DOM");
                return;
            }

            // Capturar dados de setores se ainda não existirem
            if (!window.SETORES_DATA || window.SETORES_DATA.length === 0)
            {
                console.log("📦 Capturando dados de setores do modal...");
                const capturado = capturarDadosSetores();

                if (!capturado || !window.SETORES_DATA || window.SETORES_DATA.length === 0)
                {
                    console.error("❌ Dados de setores não disponíveis!");
                    console.error("   Tentando aguardar carregamento da página...");

                    // Tentar novamente após 500ms
                    setTimeout(() =>
                    {
                        capturarDadosSetores();
                        if (window.SETORES_DATA && window.SETORES_DATA.length > 0)
                        {
                            console.log(`✅ Dados capturados após delay: ${window.SETORES_DATA.length} itens`);
                            criarDropDownTree(ddtSetor);
                        }
                        else
                        {
                            console.error("❌ Ainda não foi possível capturar dados de setores!");
                        }
                    }, 500);
                    return;
                }
            }

            console.log(`📦 Dados disponíveis: ${window.SETORES_DATA?.length || 0} itens`);
            criarDropDownTree(ddtSetor);
        });

        console.log("✅ Listener do modal configurado");
    }

    /**
     * Cria o DropDownTree no elemento fornecido
     */
    function criarDropDownTree(elemento)
    {
        try
        {
            console.log("🔧 Criando DropDownTree...");

            // Destruir instância antiga se existir
            if (elemento.ej2_instances && elemento.ej2_instances[0])
            {
                console.log("🗑️ Destruindo instância antiga...");
                try
                {
                    elemento.ej2_instances[0].destroy();
                }
                catch (error)
                {
                    console.warn("⚠️ Erro ao destruir:", error);
                }
            }

            // Criar nova instância
            const dropdown = new ej.dropdowns.DropDownTree({
                fields: {
                    dataSource: window.SETORES_DATA || [],
                    value: 'SetorSolicitanteId',
                    text: 'Nome',
                    parentValue: 'SetorPaiId',
                    hasChildren: 'HasChild'
                },
                allowFiltering: true,
                placeholder: 'Selecione o setor...',
                sortOrder: 'Ascending',
                showCheckBox: false,
                filterType: 'Contains',
                filterBarPlaceholder: 'Procurar...',
                popupHeight: '200px',
                popupWidth: '100%',
                width: '100%',

                created: function ()
                {
                    console.log("✅ DropDownTree CREATED");
                },

                dataBound: function ()
                {
                    console.log("✅ DropDownTree DATA BOUND");
                    console.log(`   Itens carregados: ${this.treeData?.length || 0}`);
                }
            });

            dropdown.appendTo(elemento);
            console.log(`✅ DropDownTree criado com sucesso - ${window.SETORES_DATA?.length || 0} itens`);
        }
        catch (error)
        {
            console.error("❌ Erro ao criar DropDownTree:", error);
            Alerta.TratamentoErroComLinha("requisitante.service.js", "criarDropDownTree", error);
        }
    }

    // ===============================================================
    // EXPORTAR FUNÇÕES GLOBALMENTE
    // ===============================================================
    window.inicializarSistemaRequisitante = inicializarSistemaRequisitante;
    window.resetarSistemaRequisitante = resetarSistemaRequisitante;
    window.configurarBotaoNovoRequisitante = configurarBotaoNovoRequisitante;
    window.abrirFormularioCadastroRequisitante = abrirFormularioCadastroRequisitante;
    window.fecharFormularioCadastroRequisitante = fecharFormularioCadastroRequisitante;
    window.limparCamposCadastroRequisitante = limparCamposCadastroRequisitante;
    window.salvarNovoRequisitante = salvarNovoRequisitante;
    window.capturarDadosSetores = capturarDadosSetores;
    window.inicializarDropDownTreeModal = inicializarDropDownTreeModal;

    // ===============================================================
    // AUTO-INICIALIZAÇÃO
    // ===============================================================
    // Inicializar o listener do modal quando o DOM estiver pronto
    if (document.readyState === 'loading')
    {
        document.addEventListener('DOMContentLoaded', inicializarDropDownTreeModal);
    }
    else
    {
        inicializarDropDownTreeModal();
    }
})();
