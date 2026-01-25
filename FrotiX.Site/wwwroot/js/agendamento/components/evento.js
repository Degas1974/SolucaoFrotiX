// ===============================================================
// SISTEMA DE EVENTO - COMPLETO E CORRIGIDO
// Arquivo: wwwroot/js/agendamento/components/evento.js
// ===============================================================

/**
 * Inicializa o sistema de evento
 * Chame esta função no final da ExibeViagem
 */
function inicializarSistemaEvento()
{
    console.log("🎯 Inicializando Sistema de Evento...");

    // 1. Monitora mudanças na finalidade
    configurarMonitoramentoFinalidade();

    // 2. Configura o botão "Novo Evento"
    configurarBotaoNovoEvento();

    // 3. Configura botões do formulário de cadastro
    configurarBotoesCadastroEvento();

    // 4. Configura evento select do requisitante de evento
    configurarRequisitanteEvento();

    console.log("✅ Sistema de Evento inicializado!");
}

/**
 * Monitora a lista de Finalidades
 */
function obterModalBootstrap(modalId)
{
    const modalEl = document.getElementById(modalId);
    if (!modalEl || !window.bootstrap || !window.bootstrap.Modal)
    {
        return null;
    }

    return window.bootstrap.Modal.getOrCreateInstance(modalEl);
}

function mostrarModalFallback(modalId)
{
    const modal = obterModalBootstrap(modalId);
    if (modal)
    {
        modal.show();
        return true;
    }

    if (window.jQuery && typeof window.jQuery.fn.modal === "function")
    {
        window.jQuery(`#${modalId}`).modal("show");
        return true;
    }

    return false;
}

function fecharModalFallback(modalId)
{
    const modal = obterModalBootstrap(modalId);
    if (modal)
    {
        modal.hide();
        return true;
    }

    if (window.jQuery && typeof window.jQuery.fn.modal === "function")
    {
        window.jQuery(`#${modalId}`).modal("hide");
        return true;
    }

    return false;
}

/**
 * Telerik DatePicker - Não precisa de rebuild como Syncfusion
 * Componentes Telerik são mais estáveis dentro de modais
 */

function obterValorDataEvento(input)
{
    try
    {
        // Telerik usa data("kendoDatePicker")
        const picker = $(input).data("kendoDatePicker");
        if (picker && picker.value())
        {
            return picker.value();
        }

        // Fallback: input nativo
        if (!input || !input.value)
        {
            return null;
        }

        const parsed = new Date(input.value);
        return Number.isNaN(parsed.getTime()) ? null : parsed;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("evento.js", "obterValorDataEvento", error);
        return null;
    }
}

function limparValorDataEvento(input)
{
    try
    {
        // Telerik usa data("kendoDatePicker")
        const picker = $(input).data("kendoDatePicker");
        if (picker)
        {
            picker.value(null);
            return;
        }

        // Fallback: input nativo
        if (input)
        {
            input.value = "";
        }
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("evento.js", "limparValorDataEvento", error);
    }
}

function configurarMonitoramentoFinalidade()
{
    const lstFinalidade = document.getElementById("lstFinalidade");

    if (!lstFinalidade)
    {
        console.warn("⚠️ lstFinalidade não encontrado");
        return;
    }

    // Verifica se é componente Syncfusion
    if (lstFinalidade.ej2_instances && lstFinalidade.ej2_instances[0])
    {
        const dropdown = lstFinalidade.ej2_instances[0];

        // Adiciona listener para SELECT (dispara imediatamente ao clicar)
        dropdown.select = function (args)
        {
            console.log("🎯 Finalidade SELECIONADA (select event):", args.itemData);

            // Pega o texto da finalidade
            const finalidade = args.itemData?.text || args.itemData?.Descricao || args.itemData?.FinalidadeId || "";

            console.log("🔍 Processando:", finalidade);
            controlarVisibilidadeSecaoEvento(finalidade);
        };

        // TAMBÉM adiciona listener para CHANGE (backup para casos de programático)
        dropdown.change = function (args)
        {
            console.log("🔄 Finalidade mudou (change event):", args.value);
            controlarVisibilidadeSecaoEvento(args.value);
        };

        console.log("✅ Listener de Finalidade configurado (SELECT + CHANGE)");

        // Verifica estado inicial
        const valorAtual = dropdown.value;
        if (valorAtual)
        {
            controlarVisibilidadeSecaoEvento(valorAtual);
        }
    } else
    {
        console.warn("⚠️ lstFinalidade não é componente EJ2");
    }
}

/**
 * Configura o evento select do requisitante de evento
 * para preencher automaticamente o setor
 */
function configurarRequisitanteEvento()
{
    console.log("🔧 === INÍCIO configurarRequisitanteEvento ===");

    // Função para tentar configurar
    const tentarConfigurar = (tentativa = 1) =>
    {
        console.log(`🔄 Tentativa ${tentativa} de configurar requisitante de evento...`);

        const lstRequisitanteEvento = document.getElementById("lstRequisitanteEvento");

        if (!lstRequisitanteEvento)
        {
            console.warn(`⚠️ lstRequisitanteEvento não encontrado no DOM (tentativa ${tentativa})`);

            if (tentativa < 5)
            {
                console.log(`   ⏰ Tentando novamente em 300ms...`);
                setTimeout(() => tentarConfigurar(tentativa + 1), 300);
            }
            else
            {
                console.error('❌ lstRequisitanteEvento não encontrado após 5 tentativas');
            }
            return;
        }

        console.log('✅ Elemento lstRequisitanteEvento encontrado');

        // Verifica se é componente Syncfusion
        if (lstRequisitanteEvento.ej2_instances && lstRequisitanteEvento.ej2_instances[0])
        {
            const dropdown = lstRequisitanteEvento.ej2_instances[0];

            console.log('✅ Componente Syncfusion encontrado:');
            console.log('   - Tipo:', dropdown.constructor.name);
            console.log('   - Value atual:', dropdown.value);
            console.log('   - Text atual:', dropdown.text);
            console.log('   - DataSource:', dropdown.dataSource);

            // Verifica se já tem um listener
            if (dropdown.select)
            {
                console.log('⚠️ Listener select já existe, será substituído');
            }

            // Configura o listener select
            dropdown.select = function (args)
            {
                console.log('🔔 [LISTENER] Select disparado no lstRequisitanteEvento:');
                console.log('   - isInteraction:', args.isInteraction);
                console.log('   - itemData:', args.itemData);
                console.log('   - value:', args.e?.target?.value);

                // Chama a função global
                if (typeof window.onSelectRequisitanteEvento === 'function')
                {
                    window.onSelectRequisitanteEvento(args);
                }
            };

            console.log('✅ Listener de select configurado com sucesso!');
            console.log('🔧 === FIM configurarRequisitanteEvento ===');
        }
        else
        {
            console.warn(`⚠️ lstRequisitanteEvento não é componente Syncfusion (tentativa ${tentativa})`);

            if (tentativa < 5)
            {
                console.log(`   ⏰ Tentando novamente em 300ms...`);
                setTimeout(() => tentarConfigurar(tentativa + 1), 300);
            }
            else
            {
                console.error('❌ lstRequisitanteEvento não inicializado após 5 tentativas');
                console.log('🔧 === FIM configurarRequisitanteEvento (FALHOU) ===');
            }
        }
    };

    // Inicia as tentativas
    tentarConfigurar();
}

/**
 * ================================================================
 * NOVA FUNÇÃO: Atualiza campos quando Requisitante Evento é selecionado
 * Esta função é chamada pelo listener em configurarRequisitanteEvento()
 * ================================================================
 */
window.onSelectRequisitanteEvento = function (args)
{
    console.log('🎯 Requisitante de Evento selecionado!');
    console.log('   itemData:', args.itemData);

    try
    {
        // Validação - aceita tanto id quanto RequisitanteId
        const requisitanteId = args.itemData?.id || args.itemData?.RequisitanteId;

        if (!args || !args.itemData || !requisitanteId)
        {
            console.warn('⚠️ Dados inválidos do requisitante');
            console.log('   id:', args.itemData?.id);
            console.log('   RequisitanteId:', args.itemData?.RequisitanteId);
            return;
        }

        console.log('✅ Requisitante ID:', requisitanteId);

        // BUSCAR SETOR DO REQUISITANTE
        $.ajax({
            url: "/Viagens/Upsert?handler=PegaSetor",
            method: "GET",
            dataType: "json",
            data: { id: requisitanteId },
            success: function (res)
            {
                console.log('📦 Resposta do servidor (Setor):', res);

                try
                {
                    // A resposta pode vir como {data: 'id'} ou {success: true, data: 'id'}
                    const setorId = res.data || (res.success && res.data);

                    if (setorId)
                    {
                        // Campos: texto readonly (display) + hidden (valor)
                        const txtSetorEvento = document.getElementById("txtSetorRequisitanteEvento");
                        const lstSetorEvento = document.getElementById("lstSetorRequisitanteEvento");

                        if (!txtSetorEvento || !lstSetorEvento)
                        {
                            console.error('❌ Campos de setor não encontrados no DOM');
                            return;
                        }

                        // Buscar nome do setor via AJAX
                        $.ajax({
                            url: "/Viagens/Upsert?handler=AJAXPreencheListaSetores",
                            method: "GET",
                            dataType: "json",
                            success: function (resSetores)
                            {
                                console.log('📋 Lista de setores recebida:', resSetores);
                                console.log('🔍 Procurando SetorId:', setorId, '(tipo:', typeof setorId, ')');

                                const setores = resSetores.data || [];
                                console.log('📊 Total de setores na lista:', setores.length);

                                // Debug: Mostrar alguns setores da lista
                                if (setores.length > 0) {
                                    console.log('📄 Exemplo de setor na lista:', setores[0]);
                                    console.log('📄 Campos disponíveis:', Object.keys(setores[0]));
                                }

                                // Normalizar ambos para string lowercase para comparação
                                const setorIdNormalizado = setorId.toString().toLowerCase();
                                console.log('🔧 SetorId normalizado:', setorIdNormalizado);

                                const setorEncontrado = setores.find(s => {
                                    if (!s.setorSolicitanteId) return false; // ✅ CORRIGIDO: lowercase
                                    const idNormalizado = s.setorSolicitanteId.toString().toLowerCase();
                                    console.log('  🔎 Comparando:', idNormalizado, '===', setorIdNormalizado, '?', idNormalizado === setorIdNormalizado);
                                    return idNormalizado === setorIdNormalizado;
                                });

                                console.log('🔍 Setor encontrado?', setorEncontrado);

                                if (setorEncontrado)
                                {
                                    // Preenche campo texto com nome do setor
                                    txtSetorEvento.value = setorEncontrado.nome; // ✅ CORRIGIDO: lowercase
                                    // Preenche campo hidden com ID do setor
                                    lstSetorEvento.value = setorId;

                                    console.log('✅ Setor atualizado:', setorEncontrado.nome, '(', setorId, ')');
                                }
                                else
                                {
                                    console.warn('⚠️ Setor não encontrado na lista:', setorId);
                                    txtSetorEvento.value = 'Setor não identificado';
                                    lstSetorEvento.value = setorId;
                                }
                            },
                            error: function (xhr, status, error)
                            {
                                console.error('❌ Erro ao buscar lista de setores:', error);
                                txtSetorEvento.value = 'Erro ao buscar setor';
                                lstSetorEvento.value = setorId;
                            }
                        });
                    }
                    else
                    {
                        console.warn('⚠️ Setor não encontrado na resposta');

                        // Limpa os campos se não houver setor
                        const txtSetorEvento = document.getElementById("txtSetorRequisitanteEvento");
                        const lstSetorEvento = document.getElementById("lstSetorRequisitanteEvento");

                        if (txtSetorEvento) txtSetorEvento.value = '';
                        if (lstSetorEvento) lstSetorEvento.value = '';
                    }
                }
                catch (error)
                {
                    console.error('❌ Erro ao setar setor:', error);
                    Alerta.TratamentoErroComLinha('evento.js', 'onSelectRequisitanteEvento.setor', error);
                }
            },
            error: function (xhr, status, error)
            {
                console.error('❌ Erro ao buscar setor:', { xhr, status, error });
                Alerta.TratamentoErroComLinha('evento.js', 'onSelectRequisitanteEvento.ajax.setor', error);

                // Limpa os campos em caso de erro
                const txtSetorEvento = document.getElementById("txtSetorRequisitanteEvento");
                const lstSetorEvento = document.getElementById("lstSetorRequisitanteEvento");

                if (txtSetorEvento) txtSetorEvento.value = '';
                if (lstSetorEvento) lstSetorEvento.value = '';
            }
        });
    }
    catch (error)
    {
        console.error('❌ Erro geral em onSelectRequisitanteEvento:', error);
        Alerta.TratamentoErroComLinha('evento.js', 'onSelectRequisitanteEvento', error);
    }
};


/**
 * Controla a visibilidade da seção de evento
 * param {string|Array} finalidade - Valor da finalidade
 */
function controlarVisibilidadeSecaoEvento(finalidade)
{
    const sectionEvento = document.getElementById("sectionEvento");
    const btnEvento = document.getElementById("btnEvento");

    if (!sectionEvento)
    {
        console.warn("sectionEvento nao encontrado");
        return;
    }

    let isEvento = false;

    if (Array.isArray(finalidade))
    {
        isEvento = finalidade.some(f =>
            f === "Evento" || f === "E" ||
            (f && f.toLowerCase && f.toLowerCase() === "evento")
        );
    } else
    {
        isEvento = finalidade === "Evento" ||
            finalidade === "E" ||
            (finalidade && finalidade.toLowerCase && finalidade.toLowerCase() === "evento");
    }

    if (isEvento)
    {
        sectionEvento.style.display = "block";

        // ✅ MOSTRAR o botão Novo Evento
        if (btnEvento)
        {
            btnEvento.style.display = "block";
            console.log("✅ Botão Novo Evento exibido (evento.js)");
        }
    } else
    {
        sectionEvento.style.display = "none";

        // ❌ ESCONDER o botão Novo Evento
        if (btnEvento)
        {
            btnEvento.style.display = "none";
            console.log("➖ Botão Novo Evento escondido (evento.js)");
        }

        if (typeof fecharFormularioCadastroEvento === "function")
        {
            fecharFormularioCadastroEvento();
        }
    }
}


/**
 * Configura o botão "Novo Evento"
 */
function configurarBotaoNovoEvento()
{
    const btnEvento = document.getElementById("btnEvento");

    if (!btnEvento)
    {
        console.warn("btnEvento nao encontrado");
        return;
    }

    const novoBotao = btnEvento.cloneNode(true);
    btnEvento.parentNode.replaceChild(novoBotao, btnEvento);

    novoBotao.addEventListener("click", function (e)
    {
        e.preventDefault();
        e.stopPropagation();

        abrirFormularioCadastroEvento();
    });

    console.log("Botao Novo Evento configurado (modal)");
}


/**
 * Abre o formulário de cadastro de evento
 */
function abrirFormularioCadastroEvento()
{
    limparCamposCadastroEvento();
    const dataInicialEl = document.getElementById("txtDataInicialEvento");
    // Telerik DatePickers não precisam de rebuild
    // Os componentes são estáveis dentro de modais Bootstrap

    if (!mostrarModalFallback("modalEvento"))
    {
        console.warn("modalEvento nao encontrado ou Bootstrap indisponivel");
    }

    setTimeout(() =>
    {
        const txtNome = document.getElementById("txtNomeEvento");
        if (txtNome)
        {
            txtNome.focus();
        }
    }, 300);
}


/**
 * Fecha o formulário de cadastro
 */
function fecharFormularioCadastroEvento()
{
    fecharModalFallback("modalEvento");

    limparCamposCadastroEvento();
    console.log("Formulario de cadastro fechado");
}


/**
 * Configura os botões do formulário de cadastro
 */
function configurarBotoesCadastroEvento()
{
    // Botão Salvar Evento (Inserir)
    const btnInserir = document.getElementById("btnInserirEvento");
    if (btnInserir)
    {
        // Aplicar classe e ícone corretos
        btnInserir.className = "btn btn-azul";
        btnInserir.innerHTML = '<i class="fa-regular fa-thumbs-up"></i> Salvar Evento';

        const novoBtnInserir = btnInserir.cloneNode(true);
        btnInserir.parentNode.replaceChild(novoBtnInserir, btnInserir);

        novoBtnInserir.addEventListener("click", function ()
        {
            console.log("💾 Inserindo evento...");
            inserirNovoEvento();
        });
    }

    // Botão Cancelar
    const btnCancelar = document.getElementById("btnCancelarEvento");
    if (btnCancelar)
    {
        // Aplicar classe e ícone corretos
        btnCancelar.className = "btn btn-vinho";
        btnCancelar.innerHTML = '<i class="fa-regular fa-circle-xmark"></i> Cancelar';

        const novoBtnCancelar = btnCancelar.cloneNode(true);
        btnCancelar.parentNode.replaceChild(novoBtnCancelar, btnCancelar);

        novoBtnCancelar.addEventListener("click", function ()
        {
            console.log("❌ Cancelando cadastro");
            fecharFormularioCadastroEvento();
        });
    }

    console.log("✅ Botões do formulário configurados com estilos corretos");
}

/**
 * Limpa todos os campos do formulário de cadastro
 */
function limparCamposCadastroEvento()
{
    try
    {
        console.log("🧹 Limpando campos do formulário...");

        // Campos de texto simples
        const txtNome = document.getElementById("txtNomeEvento");
        if (txtNome) txtNome.value = "";

        const txtDescricao = document.getElementById("txtDescricaoEvento");
        if (txtDescricao) txtDescricao.value = "";
        // Datas
        const txtDataInicial = document.getElementById("txtDataInicialEvento");
        limparValorDataEvento(txtDataInicial);

        const txtDataFinal = document.getElementById("txtDataFinalEvento");
        limparValorDataEvento(txtDataFinal);

        // NumericTextBox (quantidade)
        const txtQuantidade = document.getElementById("txtQtdParticipantesEventoCadastro");
        if (txtQuantidade?.ej2_instances?.[0])
        {
            txtQuantidade.ej2_instances[0].value = 0;
        }

        // ComboBox Telerik (requisitante)
        const comboRequisitante = getRequisitanteEventoCombo();
        if (comboRequisitante)
        {
            comboRequisitante.value(null);
        }

        // Campo texto readonly (setor - nome)
        const txtSetor = document.getElementById("txtSetorRequisitanteEvento");
        if (txtSetor) txtSetor.value = '';

        // Campo hidden (setor - ID)
        const lstSetor = document.getElementById("lstSetorRequisitanteEvento");
        if (lstSetor) lstSetor.value = '';

        console.log("✅ Campos limpos com sucesso");

    } catch (error)
    {
        console.error("❌ Erro ao limpar campos:", error);
        Alerta.TratamentoErroComLinha("evento.js", "limparCamposCadastroEvento", error);
    }
}

/**
 * Insere um novo evento no banco de dados
 * Adaptado do código de ViagemUpsert.js
 */
function inserirNovoEvento()
{
    try
    {
        console.log("💾 Iniciando inserção de evento...");

        // Validação de campos obrigatórios
        const txtNome = document.getElementById("txtNomeEvento");
        const txtDescricao = document.getElementById("txtDescricaoEvento");
        const txtDataInicial = document.getElementById("txtDataInicialEvento");
        const txtDataFinal = document.getElementById("txtDataFinalEvento");
        const txtQuantidade = document.getElementById("txtQtdParticipantesEventoCadastro");

        if (!txtNome || !txtNome.value.trim())
        {
            Alerta.Alerta("Atenção", "O Nome do Evento é obrigatório!");
            return;
        }

        if (!txtDescricao || !txtDescricao.value.trim())
        {
            Alerta.Alerta("Atenção", "A Descrição do Evento é obrigatória!");
            return;
        }
        // Pega as datas (Syncfusion ou input nativo)
        const dataInicial = obterValorDataEvento(txtDataInicial);
        const dataFinal = obterValorDataEvento(txtDataFinal);

        if (!dataInicial)
        {
            Alerta.Alerta("Atencao", "A Data Inicial eh obrigatoria!");
            return;
        }

        if (!dataFinal)
        {
            Alerta.Alerta("Atencao", "A Data Final eh obrigatoria!");
            return;
        }

        if (dataInicial > dataFinal)
        {
            Alerta.Alerta("Atencao", "A Data Inicial nao pode ser maior que a Data Final!");
            if (txtDataFinal?.ej2_instances?.[0])
            {
                txtDataFinal.ej2_instances[0].value = null;
            }
            else if (txtDataFinal)
            {
                txtDataFinal.value = "";
            }
            return;
        }

        // Pega quantidade
        const quantidadePicker = txtQuantidade?.ej2_instances?.[0];
        const quantidade = quantidadePicker?.value || 0;

        if (!quantidade || quantidade <= 0)
        {
            Alerta.Alerta("Atenção", "A Quantidade de Participantes é obrigatória!");
            return;
        }

        // Validação: Quantidade deve ser número inteiro
        if (!Number.isInteger(quantidade) || quantidade > 2147483647)
        {
            Alerta.Alerta("Atenção", "A Quantidade de Participantes deve ser um número inteiro válido (máximo: 2.147.483.647)!");
            // Limpa o campo de quantidade
            quantidadePicker.value = null;
            return;
        }

        // Pega setor (campo hidden) e requisitante (ComboBox Telerik)
        const lstSetor = document.getElementById("lstSetorRequisitanteEvento"); // Hidden input
        const comboRequisitante = getRequisitanteEventoCombo(); // ComboBox Telerik

        // Validação do setor (agora é um campo hidden)
        if (!lstSetor || !lstSetor.value || lstSetor.value.trim() === '')
        {
            Alerta.Alerta("Atenção", "O Setor é obrigatório! Selecione um requisitante primeiro.");
            return;
        }

        // Validação do requisitante (ComboBox Telerik)
        if (!comboRequisitante || !comboRequisitante.value())
        {
            Alerta.Alerta("Atenção", "O Requisitante é obrigatório!");
            return;
        }

        const setorId = lstSetor.value.toString(); // Lê do hidden input
        const requisitanteId = comboRequisitante.value().toString();

        // Prepara objeto para envio
        const objEvento = {
            Nome: txtNome.value.trim(),
            Descricao: txtDescricao.value.trim(),
            SetorSolicitanteId: setorId,
            RequisitanteId: requisitanteId,
            QtdParticipantes: quantidade,
            DataInicial: moment(dataInicial).format("MM-DD-YYYY"),
            DataFinal: moment(dataFinal).format("MM-DD-YYYY"),
            Status: "1"
        };

        console.log("📦 Objeto a ser enviado:", objEvento);

        // Envia via AJAX
        $.ajax({
            type: "POST",
            url: "/api/Viagem/AdicionarEvento",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(objEvento),
            success: function (data)
            {
                try
                {
                    console.log("✅ Resposta do servidor:", data);

                    if (data.success)
                    {
                        // Mostra mensagem de sucesso
                        AppToast.show('Verde', data.message);

                        // Atualiza a lista de eventos com o novo evento
                        atualizarListaEventos(data.eventoId, data.eventoText);

                        // Fecha o formulário
                        fecharFormularioCadastroEvento();

                        console.log("✅ Evento inserido com sucesso!");
                    }
                    else
                    {
                        Alerta.Alerta("Erro", data.message || "Erro ao adicionar evento");
                    }
                }
                catch (error)
                {
                    console.error("❌ Erro no success do AJAX:", error);
                    Alerta.TratamentoErroComLinha("evento.js", "ajax.AdicionarEvento.success", error);
                }
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                try
                {
                    console.error("❌ Erro na requisição AJAX:", errorThrown);
                    console.error("   Status:", textStatus);
                    console.error("   Response:", jqXHR.responseText);

                    Alerta.Alerta("Erro", "Erro ao adicionar evento no servidor");
                }
                catch (error)
                {
                    console.error("❌ Erro no error handler:", error);
                    Alerta.TratamentoErroComLinha("evento.js", "ajax.AdicionarEvento.error", error);
                }
            }
        });

    }
    catch (error)
    {
        console.error("❌ Erro ao inserir evento:", error);
        Alerta.TratamentoErroComLinha("evento.js", "inserirNovoEvento", error);
    }
}

/**
 * Atualiza a lista de eventos após adicionar um novo
 * param {string} eventoId - ID do evento recém-criado
 * param {string} eventoText - Nome do evento recém-criado
 */
function atualizarListaEventos(eventoId, eventoText)
{
    try
    {
        console.log("🔄 Atualizando lista de eventos...");
        console.log("   EventoId:", eventoId);
        console.log("   EventoText:", eventoText);

        const lstEventos = document.getElementById("lstEventos");

        if (!lstEventos || !lstEventos.ej2_instances || !lstEventos.ej2_instances[0])
        {
            console.error("❌ lstEventos não encontrado ou não é componente EJ2");
            return;
        }

        const comboBox = lstEventos.ej2_instances[0];

        // Cria o novo item com a estrutura correta
        const novoItem = {
            EventoId: eventoId,
            Evento: eventoText
        };

        console.log("📦 Novo item a ser adicionado:", novoItem);

        // Obter dataSource atual
        let dataSource = comboBox.dataSource || [];

        if (!Array.isArray(dataSource))
        {
            dataSource = [];
        }

        // Verificar se já existe
        const jaExiste = dataSource.some(item => item.EventoId === eventoId);

        if (!jaExiste)
        {
            // Adiciona o novo item
            dataSource.push(novoItem);
            console.log("📦 Novo item adicionado ao array");

            // Ordena alfabeticamente por nome do evento
            dataSource.sort((a, b) => {
                const nomeA = (a.Evento || '').toString().toLowerCase();
                const nomeB = (b.Evento || '').toString().toLowerCase();
                return nomeA.localeCompare(nomeB);
            });
            console.log("🔄 Lista ordenada alfabeticamente");

            // Limpa o dataSource
            comboBox.dataSource = [];
            comboBox.dataBind();

            // Recarrega com a lista ordenada
            comboBox.dataSource = dataSource;
            comboBox.dataBind();

            console.log("✅ Lista atualizada e ordenada com sucesso");
        }
        else
        {
            console.log("⚠️ Item já existe na lista");
        }

        // Aguarda o componente processar
        setTimeout(() =>
        {
            console.log("🔄 Selecionando novo evento...");

            // Define o valor
            comboBox.value = eventoId;

            // Força a atualização visual
            comboBox.dataBind();

            console.log("✅ Evento selecionado");
            console.log("   Value:", comboBox.value);
            console.log("   Text:", comboBox.text);

            // Aguarda mais um pouco antes de buscar dados
            setTimeout(() =>
            {
                // Buscar e exibir os dados do evento
                if (typeof window.exibirDadosEvento === 'function')
                {
                    console.log("🔍 Chamando window.exibirDadosEvento...");
                    window.exibirDadosEvento(novoItem);
                }
                else if (typeof exibirDadosEvento === 'function')
                {
                    console.log("🔍 Chamando exibirDadosEvento...");
                    exibirDadosEvento(novoItem);
                }
                else
                {
                    console.warn("⚠️ Função exibirDadosEvento não encontrada");
                }
            }, 100);

        }, 250);

        console.log("✅ Processo de atualização iniciado");

    }
    catch (error)
    {
        console.error("❌ Erro ao atualizar lista de eventos:", error);
        Alerta.TratamentoErroComLinha("evento.js", "atualizarListaEventos", error);
    }
}

// ===============================================================
// DIAGNÓSTICO - Use no console para debugar
// ===============================================================

/**
 * Diagnóstico completo do sistema de evento
 */
function diagnosticarSistemaEvento()
{
    console.log("=== DIAGNÓSTICO DO SISTEMA DE EVENTO ===");

    const sectionEvento = document.getElementById("sectionEvento");
    console.log("1. sectionEvento existe?", !!sectionEvento);
    if (sectionEvento)
    {
        console.log("   - Display:", sectionEvento.style.display);
        console.log("   - Visível?", sectionEvento.offsetWidth > 0 && sectionEvento.offsetHeight > 0);
    }

    const sectionCadastro = document.getElementById("modalEvento");
    console.log("2. modalEvento existe?", !!sectionCadastro);
    if (sectionCadastro)
    {
        console.log("   - Display:", sectionCadastro.style.display);
        console.log("   - Visível?", sectionCadastro.offsetWidth > 0 && sectionCadastro.offsetHeight > 0);
    }

    const lstFinalidade = document.getElementById("lstFinalidade");
    console.log("3. lstFinalidade existe?", !!lstFinalidade);
    if (lstFinalidade?.ej2_instances)
    {
        console.log("   - É componente EJ2?", true);
        console.log("   - Valor atual:", lstFinalidade.ej2_instances[0].value);
    }

    const lstEventos = document.getElementById("lstEventos");
    console.log("4. lstEventos existe?", !!lstEventos);
    if (lstEventos?.ej2_instances)
    {
        console.log("   - É componente EJ2?", true);
        console.log("   - DataSource:", lstEventos.ej2_instances[0].dataSource);
        console.log("   - Quantidade de itens:", lstEventos.ej2_instances[0].dataSource?.length || 0);
    }

    const btnEvento = document.getElementById("btnEvento");
    console.log("5. btnEvento existe?", !!btnEvento);
    if (btnEvento)
    {
        console.log("   - Display:", window.getComputedStyle(btnEvento).display);
        console.log("   - Visível?", btnEvento.offsetWidth > 0 && btnEvento.offsetHeight > 0);
        console.log("   - Dimensões:", btnEvento.offsetWidth + "x" + btnEvento.offsetHeight);
    }

    const btnInserir = document.getElementById("btnInserirEvento");
    console.log("6. btnInserirEvento existe?", !!btnInserir);

    const btnCancelar = document.getElementById("btnCancelarEvento");
    console.log("7. btnCancelarEvento existe?", !!btnCancelar);

    console.log("=== FIM DO DIAGNÓSTICO ===");
}

/**
 * Testa mostrar a seção de evento
 */
function testarMostrarSecaoEvento()
{
    console.log("🧪 Teste: Mostrando seção de evento");
    controlarVisibilidadeSecaoEvento("Evento");
}

/**
 * Testa ocultar a seção de evento
 */
function testarOcultarSecaoEvento()
{
    console.log("🧪 Teste: Ocultando seção de evento");
    controlarVisibilidadeSecaoEvento("Transporte");
}

/**
 * Testa abrir o formulário de cadastro
 */
function testarAbrirFormulario()
{
    console.log("🧪 Teste: Abrindo formulário de cadastro");
    abrirFormularioCadastroEvento();
}

/**
 * Testa fechar o formulário de cadastro
 */
function testarFecharFormulario()
{
    console.log("🧪 Teste: Fechando formulário de cadastro");
    fecharFormularioCadastroEvento();
}

/**
 * Testa limpar campos do formulário
 */
function testarLimparCampos()
{
    console.log("🧪 Teste: Limpando campos");
    limparCamposCadastroEvento();
}

/**
 * Verifica se todos os elementos necessários existem
 */
function verificarElementosEvento()
{
    console.log("=== VERIFICAÇÃO DE ELEMENTOS ===");

    const elementos = [
        "sectionEvento",
        "modalEvento",
        "lstEventos",
        "btnEvento",
        "txtNomeEvento",
        "txtDescricaoEvento",
        "txtDataInicialEvento",
        "txtDataFinalEvento",
        "txtQtdParticipantesEventoCadastro",
        "lstRequisitanteEvento",
        "lstSetorRequisitanteEvento",
        "btnInserirEvento",
        "btnCancelarEvento"
    ];

    let todosExistem = true;

    elementos.forEach(id =>
    {
        const elemento = document.getElementById(id);
        const existe = !!elemento;
        console.log(existe ? "✅" : "❌", id, "existe?", existe);
        if (!existe) todosExistem = false;
    });

    console.log("=== FIM DA VERIFICAÇÃO ===");
    console.log(todosExistem ? "✅ Todos os elementos existem!" : "⚠️ Alguns elementos estão faltando!");

    return todosExistem;
}

// ===============================================================
// EXPORTAÇÃO (se usar módulos)
// ===============================================================

// Se você usar módulos ES6, descomente as linhas abaixo:
// export {
//     inicializarSistemaEvento,
//     controlarVisibilidadeSecaoEvento,
//     abrirFormularioCadastroEvento,
//     fecharFormularioCadastroEvento,
//     diagnosticarSistemaEvento
// };
