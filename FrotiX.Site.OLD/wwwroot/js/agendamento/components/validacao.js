/* ****************************************************************************************
 * ⚡ ARQUIVO: validacao.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Validação completa de formulários de agendamento e viagens. Classe
 *                   ValidadorAgendamento orquestra todas as validações de campos
 *                   obrigatórios, regras de negócio (km, datas, recorrência), e
 *                   confirmações de usuário. Suporta validação condicional (agendamento
 *                   vs viagem realizada), múltiplos componentes UI (Kendo UI,
 *                   Syncfusion bridge, jQuery), e flags de confirmação para evitar
 *                   prompts repetidos.
 * 📥 ENTRADAS     : viagemId (string, opcional), valores de campos DOM (Kendo
 *                   $(el).data("kendoXxx"), Syncfusion bridge getSyncfusionInstance,
 *                   jQuery val()), flags globais
 *                   (window.transformandoEmViagem, window.CarregandoAgendamento),
 *                   texto de botão (#btnConfirma)
 * 📤 SAÍDAS       : Promises<boolean> (true=válido, false=inválido), arrays this.erros,
 *                   Alerta.Erro/Confirmar dialogs, foco em campos inválidos, limpeza
 *                   de campos (val(""))
 * 🔗 CHAMADA POR  : Formulários de agendamento (main.js, dialogs.js), botões de submit,
 *                   event handlers de campos
 * 🔄 CHAMA        : jQuery ($), Kendo $(el).data("kendoDropDownList/ComboBox/MultiSelect"),
 *                   window.getSyncfusionInstance (bridge para controles ainda Syncfusion),
 *                   moment.js, Alerta.Erro/Confirmar, window.parseDate,
 *                   Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : Kendo UI (DropDownList, ComboBox, MultiSelect, DatePicker),
 *                   Syncfusion EJ2 bridge (lstPeriodos, calDatasSelecionadas,
 *                   lstSetorRequisitanteAgendamento), jQuery, moment.js, Alerta.js,
 *                   window.parseDate, syncfusion.utils.js
 * 📝 OBSERVAÇÕES  : Exporta window.ValidadorAgendamento (instância global) e 3 funções
 *                   legacy (ValidaCampos, validarDatas, validarDatasInicialFinal).
 *                   Todos os métodos async retornam Promises<boolean>. Flags de
 *                   confirmação (_kmConfirmado, _finalizacaoConfirmada) resetadas a
 *                   cada validação. Validação condicional baseada em botão e flags.
 *                   Typos: "padrío" (line 379), console.log em produção (lines 375-445).
 *
 * 📋 ÍNDICE DE MÉTODOS DA CLASSE (21 métodos + constructor):
 *
 * ┌─ CONSTRUCTOR ─────────────────────────────────────────────────────────┐
 * │ 1. constructor()                                                      │
 * │    → Inicializa this.erros = []                                      │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ MÉTODO PRINCIPAL DE ORQUESTRAÇÃO ───────────────────────────────────┐
 * │ 2. async validar(viagemId = null)                                    │
 * │    → param {string} viagemId - ID da viagem (opcional)               │
 * │    → returns {Promise<boolean>} true se válido                       │
 * │    → Reseta this.erros = [], _kmConfirmado=false, _finalizacaoConfirmada=false│
 * │    → Executa validações em ordem (retorna false no primeiro erro):   │
 * │      1. validarDataInicial()                                         │
 * │      2. validarFinalidade()                                          │
 * │      3. validarOrigem()                                              │
 * │      4. validarDestino()                                             │
 * │      5. Se algumFinalPreenchido: validarFinalizacao()                │
 * │      6. Se !ehAgendamento OU algumFinalPreenchido: validarCamposViagem()│
 * │      7. validarRequisitante()                                        │
 * │      8. validarRamal()                                               │
 * │      9. validarSetor()                                               │
 * │      10. validarEvento()                                             │
 * │      11. Se !transformandoEmViagem: validarRecorrencia()             │
 * │      12. validarPeriodoRecorrencia()                                 │
 * │      13. validarDiasVariados()                                       │
 * │      14. validarKmFinal()                                            │
 * │      15. Se algumFinalPreenchido: confirmarFinalizacao()             │
 * │    → ehAgendamento detectado por texto do botão:                     │
 * │      "Edita Agendamento" OU "Confirma Agendamento" OU "Confirmar"    │
 * │    → try-catch: Alerta.TratamentoErroComLinha, retorna false         │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ VALIDAÇÕES DE CAMPOS OBRIGATÓRIOS ──────────────────────────────────┐
 * │ 3. async validarDataInicial()                                        │
 * │    → Valida txtDataInicial (Syncfusion DatePicker)                  │
 * │    → Se inválido/null: seta moment().toDate() + dataBind()          │
 * │    → returns true sempre (corrige automaticamente)                  │
 * │                                                                       │
 * │ 4. async validarFinalidade()                                         │
 * │    → Valida lstFinalidade (Kendo DropDownList)                      │
 * │    → Se vazio/null: Alerta.Erro + retorna false                     │
 * │                                                                       │
 * │ 5. async validarOrigem()                                             │
 * │    → Valida cmbOrigem (Syncfusion)                                   │
 * │    → Se vazio/null: Alerta.Erro + retorna false                     │
 * │                                                                       │
 * │ 6. async validarDestino()                                            │
 * │    → Valida cmbDestino (Syncfusion)                                  │
 * │    → Se vazio/null: Alerta.Erro + retorna false                     │
 * │                                                                       │
 * │ 7. async validarRequisitante()                                       │
 * │    → Valida lstRequisitante (Kendo ComboBox)                         │
 * │    → Usa $(element).data("kendoComboBox").value()                    │
 * │    → Se vazio/null: Alerta.Erro + retorna false                     │
 * │                                                                       │
 * │ 8. async validarRamal()                                              │
 * │    → Valida txtRamalRequisitanteSF via jQuery val()                  │
 * │    → Fallback: $("#txtRamalRequisitante").val()                     │
 * │    → Se vazio/null: Alerta.Erro + retorna false                     │
 * │                                                                       │
 * │ 9. async validarSetor()                                              │
 * │    → Valida lstSetorRequisitanteAgendamento (Syncfusion bridge)      │
 * │    → Verifica visibilidade: offsetWidth>0 && offsetHeight>0          │
 * │    → Se oculto: retorna true (pula validação)                        │
 * │    → Usa window.getSyncfusionInstance() para obter widget            │
 * │    → Valida valor (pode ser array ou string): !== "" && length>0    │
 * │    → Se vazio/null: Alerta.Erro + retorna false                     │
 * │                                                                       │
 * │ 10. async validarEvento()                                            │
 * │     → Se finalidade[0]==="Evento": valida lstEventos (Kendo ComboBox)│
 * │     → Se vazio/null: Alerta.Erro + retorna false                    │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ VALIDAÇÕES DE FINALIZAÇÃO ──────────────────────────────────────────┐
 * │ 11. verificarCamposFinalizacao()                                     │
 * │     → Não-async, returns boolean                                    │
 * │     → Verifica se ALGUM campo final preenchido:                      │
 * │       txtDataFinal || txtHoraFinal || ddtCombustivelFinal || txtKmFinal│
 * │     → returns true se algum preenchido                              │
 * │                                                                       │
 * │ 12. async validarFinalizacao()                                       │
 * │     → Valida TODOS campos de finalização preenchidos:                │
 * │       dataFinal && horaFinal && combustivelFinal && kmFinal          │
 * │     → Se incompleto: Alerta.Erro com lista de campos + retorna false│
 * │     → Valida dataFinal <= dataAtual (comparação sem hora)            │
 * │     → Se dataFinal > hoje: Alerta.Erro + limpa campo + focus        │
 * │     → Valida destino obrigatório quando finalizado                   │
 * │                                                                       │
 * │ 13. async confirmarFinalizacao()                                     │
 * │     → Se todos campos finais preenchidos && !_finalizacaoConfirmada: │
 * │       Alerta.Confirmar "Você está criando a viagem como Realizada"  │
 * │     → Se !confirmacao: retorna false                                │
 * │     → Marca _finalizacaoConfirmada=true (evita re-prompt)            │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ VALIDAÇÕES DE VIAGEM (não-agendamento) ─────────────────────────────┐
 * │ 14. async validarCamposViagem()                                      │
 * │     → Valida campos obrigatórios para viagem aberta/realizada:       │
 * │       - Motorista (lstMotorista): obrigatório                        │
 * │       - Veículo (lstVeiculo): obrigatório                            │
 * │       - KM: validarKmInicialFinal()                                  │
 * │       - Combustível Inicial (ddtCombustivelInicial): obrigatório     │
 * │     → Nota: Ficha de Vistoria NÃO é mais obrigatória (comentário)   │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ VALIDAÇÕES DE RECORRÊNCIA ──────────────────────────────────────────┐
 * │ 15. async validarRecorrencia()                                       │
 * │     → Valida lstRecorrente e lstPeriodos                             │
 * │     → Se recorrente="S" && !periodo: Alerta.Erro                    │
 * │     → Se periodo="S" OU "Q" (Semanal/Quinzenal):                    │
 * │       lstDias.value.length > 0 (dias da semana obrigatório)          │
 * │     → Se periodo="M" (Mensal):                                       │
 * │       lstDiasMes.value !== "" (dia do mês obrigatório)              │
 * │                                                                       │
 * │ 16. async validarPeriodoRecorrencia()                                │
 * │     → Se periodo ∈ ["D","S","Q","M"]:                               │
 * │       txtFinalRecorrencia.value obrigatório (data final)             │
 * │                                                                       │
 * │ 17. async validarDiasVariados()                                      │
 * │     → Se periodo="V" (Dias Variados):                                │
 * │       Verifica calDatasSelecionadas (Syncfusion Calendar)            │
 * │       Se não disponível: retorna true (editando existente)           │
 * │       Se disponível: calendarObj.values.length > 0 (ao menos 1 dia)  │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ VALIDAÇÕES DE KM ────────────────────────────────────────────────────┐
 * │ 18. async validarKmInicialFinal()                                    │
 * │     → Compara txtKmInicial vs txtKmFinal                             │
 * │     → Se !kmInicial || !kmFinal: retorna true (não aplica)          │
 * │     → Converte para float: replace(",",".") + parseFloat             │
 * │     → Valida kmFinal >= kmInicial (erro bloqueante)                  │
 * │     → Valida diff <= 2000km (erro bloqueante, limpa campo)           │
 * │     → Se diff > 100km && !_kmConfirmado:                            │
 * │       Alerta.Confirmar "excede em 100km... Tem certeza?"            │
 * │       Se !confirmacao: limpa campo + focus + retorna false           │
 * │       Se confirmacao: marca _kmConfirmado=true (evita re-prompt)     │
 * │                                                                       │
 * │ 19. async validarKmFinal()                                           │
 * │     → Se kmFinal preenchido: parseFloat(kmFinal) > 0                │
 * │     → Se <=0: Alerta.Erro                                           │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES GLOBAIS LEGACY ─────────────────────────────────────────────┐
 * │ 20. window.ValidaCampos(viagemId)                                    │
 * │     → Wrapper legacy para window.ValidadorAgendamento.validar()      │
 * │     → returns await ValidadorAgendamento.validar(viagemId)           │
 * │                                                                       │
 * │ 21. window.validarDatas()                                            │
 * │     → Valida txtDataInicial vs txtDataFinal (jQuery val())           │
 * │     → Se diferença >= 5 dias: Alerta.Confirmar                      │
 * │     → Se !confirmacao: limpa txtDataFinal + focus                   │
 * │     → Usa window.parseDate + setHours(0,0,0,0)                      │
 * │                                                                       │
 * │ 22. window.validarDatasInicialFinal(DataInicial, DataFinal)         │
 * │     → Valida diferença entre datas (params)                          │
 * │     → Idêntico a validarDatas mas recebe strings como params         │
 * │     → Se diferença >= 5 dias: Alerta.Confirmar                      │
 * │     → Se !confirmacao: limpa txtDataFinal.value=null + focus        │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE VALIDAÇÃO COMPLETA (validar):
 * 1. Reseta erros e flags de confirmação
 * 2. Valida campos obrigatórios base (data, finalidade, origem, destino)
 * 3. Verifica campos de finalização (verificarCamposFinalizacao)
 * 4. Se algum campo final preenchido: valida finalização completa
 * 5. Detecta se é agendamento (texto do botão #btnConfirma)
 * 6. Se NÃO agendamento OU tem finalização: valida campos de viagem
 * 7. Valida requisitante, ramal, setor
 * 8. Se finalidade="Evento": valida evento
 * 9. Se !transformandoEmViagem: valida recorrência
 * 10. Valida período de recorrência e dias variados
 * 11. Valida km final e confirma finalização se aplicável
 * 12. Retorna true se todas validações passaram
 *
 * 🔄 FLUXO DE VALIDAÇÃO CONDICIONAL (agendamento vs viagem):
 * 1. Lê texto do botão #btnConfirma
 * 2. ehAgendamento = texto contém "Edita/Confirma Agendamento" OU "Confirmar"
 * 3. algumFinalPreenchido = verificarCamposFinalizacao()
 * 4. Se !ehAgendamento OU algumFinalPreenchido:
 *    - Valida motorista, veículo, km, combustível (campos de viagem)
 * 5. Senão: pula validação de campos de viagem (agendamento não precisa)
 *
 * 🔄 FLUXO DE VALIDAÇÃO KM COM CONFIRMAÇÃO:
 * 1. validarKmInicialFinal() chamado
 * 2. Calcula diff = kmFinal - kmInicial
 * 3. Se diff < 0: erro bloqueante (km final < inicial)
 * 4. Se diff > 2000: erro bloqueante (excede 2000km)
 * 5. Se diff > 100 && !_kmConfirmado:
 *    a. Alerta.Confirmar "excede em 100km"
 *    b. Se !confirmacao: limpa campo + retorna false
 *    c. Se confirmacao: marca _kmConfirmado=true
 * 6. _kmConfirmado=true evita re-prompt na mesma sessão de validação
 * 7. Flag resetada no início de validar()
 *
 * 🔄 FLUXO DE VALIDAÇÃO RECORRÊNCIA:
 * 1. Se recorrente="S" (Sim): valida periodo obrigatório
 * 2. Se periodo="S" ou "Q" (Semanal/Quinzenal):
 *    - lstDias deve ter ao menos 1 dia selecionado
 * 3. Se periodo="M" (Mensal):
 *    - lstDiasMes deve ter dia do mês selecionado
 * 4. Se periodo="V" (Dias Variados):
 *    - calDatasSelecionadas.values deve ter ao menos 1 data
 *    - Se calendário não disponível: retorna true (editando existente)
 * 5. Se periodo ∈ ["D","S","Q","M"]:
 *    - txtFinalRecorrencia obrigatório (data final da recorrência)
 *
 * 📌 COMPONENTES UI SUPORTADOS:
 * - Kendo UI: $("#el").data("kendoDropDownList/ComboBox/MultiSelect").value()
 * - Syncfusion bridge: window.getSyncfusionInstance("id").value (lstPeriodos,
 *   calDatasSelecionadas, lstSetorRequisitanteAgendamento)
 * - jQuery: $("#element").val()
 * - Kendo helpers: window.getKendoDateValue(), window.setKendoDateValue()
 *
 * 📌 FLAGS DE CONTROLE:
 * - this._kmConfirmado: evita re-prompt de confirmação de km > 100
 * - this._finalizacaoConfirmada: evita re-prompt de confirmação de finalização
 * - window.transformandoEmViagem: pula validação de recorrência
 * - window.CarregandoAgendamento: flag de carregamento (não usada aqui)
 *
 * 📌 VALIDAÇÕES BLOQUEANTES vs NÃO-BLOQUEANTES:
 * - Bloqueante (Alerta.Erro, retorna false): todos os campos obrigatórios, km > 2000
 * - Não-bloqueante (Alerta.Confirmar, pode continuar): km > 100, datas diff > 5 dias
 *
 * 📌 REGRAS DE NEGÓCIO:
 * - Data Final <= Data Atual (finalização)
 * - KM Final > KM Inicial
 * - Diferença KM <= 2000km (bloqueante)
 * - Diferença KM > 100km (confirmação)
 * - Diferença datas >= 5 dias (confirmação)
 * - Finalidade="Evento" → Evento obrigatório
 * - Recorrente="S" → Periodo obrigatório
 * - Periodo="S"/"Q" → Dias da Semana obrigatório
 * - Periodo="M" → Dia do Mês obrigatório
 * - Periodo="V" → Datas Variadas obrigatório
 * - Periodo ∈ ["D","S","Q","M"] → Data Final Recorrência obrigatório
 * - Campos de finalização: todos ou nenhum (não aceita parcial)
 * - Agendamento: motorista/veículo/km/combustível opcional
 * - Viagem Realizada: motorista/veículo/km/combustível obrigatório
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Typo "padrío" (line 379) deveria ser "padrão"
 * - console.log/error presentes em produção (lines 375, 387, 411, 420, 427, 445, 450, 579)
 * - validarSetor verifica visibilidade (pode estar oculto dinamicamente)
 * - parseFloat usa replace(",",".") para aceitar formato pt-BR
 * - Comparação de datas sempre com setHours(0,0,0,0) para ignorar hora
 * - this.erros array inicializado mas nunca populado (possível uso futuro)
 * - Ficha de Vistoria removida como obrigatória (comentário line 286)
 * - Kendo widgets acessados via $("#id").data("kendoXxx").value()
 * - Syncfusion widgets restantes acessados via window.getSyncfusionInstance()
 * - validarRamal usa jQuery val() com fallback para campo HTML padrão
 * - toLocaleString('pt-BR') para formatação de números (linha 630)
 *
 * 🔌 VERSÃO: 2.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 13/02/2026
 **************************************************************************************** */

/**
 * Classe para validação de campos
 */
class ValidadorAgendamento
{
    constructor()
    {
        this.erros = [];
    }

    /**
     * Valida todos os campos do formulário
     * param {string} viagemId - ID da viagem (opcional)
     * returns {Promise<boolean>} true se válido
     */
    async validar(viagemId = null)
    {
        try
        {
            this.erros = [];
            
            // Resetar flags de confirmação para nova validação
            this._kmConfirmado = false;
            this._finalizacaoConfirmada = false;

            // Validar data inicial
            if (!await this.validarDataInicial()) return false;

            // Validar finalidade
            if (!await this.validarFinalidade()) return false;

            // Validar origem
            if (!await this.validarOrigem()) return false;

            // Validar destino
            if (!await this.validarDestino()) return false;

            // Validar campos de finalização (se preenchidos)
            const algumFinalPreenchido = this.verificarCamposFinalizacao();
            if (algumFinalPreenchido)
            {
                if (!await this.validarFinalizacao()) return false;
            }

            // Validações específicas de viagem ABERTA (não agendamento)
            // Motorista, Veículo, KM e Combustível NÃO são obrigatórios em agendamentos
            // SÓ validar esses campos se:
            // 1. Está criando/editando uma viagem JÁ ABERTA/REALIZADA (não agendamento)
            // 2. OU se algum campo de finalização foi preenchido (transformando agendamento em viagem)
            const btnTexto = $("#btnConfirma").text().trim();
            const ehAgendamento = btnTexto === "Edita Agendamento" || btnTexto === "Confirma Agendamento" || btnTexto === "Confirmar";

            // Se NÃO for agendamento OU se tem campos de finalização preenchidos
            if (!ehAgendamento || algumFinalPreenchido)
            {
                if (!await this.validarCamposViagem()) return false;
            }

            // Validar requisitante
            if (!await this.validarRequisitante()) return false;

            // Validar ramal
            if (!await this.validarRamal()) return false;

            // Validar setor
            if (!await this.validarSetor()) return false;

            // Validar evento (se finalidade for "Evento")
            if (!await this.validarEvento()) return false;

            // Validar recorrência
            if (window.transformandoEmViagem === false)
            {
                if (!await this.validarRecorrencia()) return false;
            }

            // Validar período de recorrência
            if (!await this.validarPeriodoRecorrencia()) return false;

            // Validar dias variados
            if (!await this.validarDiasVariados()) return false;

            // Validar quilometragem final
            if (!await this.validarKmFinal()) return false;

            // Validar campos de finalização completos
            if (algumFinalPreenchido)
            {
                if (!await this.confirmarFinalizacao()) return false;
            }

            return true;

        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validar", error);
            return false;
        }
    }

    /**
     * Valida data inicial
     */
    async validarDataInicial()
    {
        try
        {
            const valDataInicial = window.getKendoDateValue("txtDataInicial");

            if (!valDataInicial || !moment(valDataInicial).isValid())
            {
                window.setKendoDateValue("txtDataInicial", moment().toDate(), true);
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarDataInicial", error);
            return false;
        }
    }

    /**
     * Valida finalidade
     */
    async validarFinalidade()
    {
        try
        {
            // ✅ KENDO: lstFinalidade agora usa Kendo DropDownList
            const ddlFinalidade = $("#lstFinalidade").data("kendoDropDownList");
            const finalidade = ddlFinalidade ? ddlFinalidade.value() : null;

            if (finalidade === "" || finalidade === null)
            {
                await Alerta.Erro("Informação Ausente", "A <strong>Finalidade</strong> é obrigatória");
                return false;
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarFinalidade", error);
            return false;
        }
    }

    /**
     * Valida origem
     */
    async validarOrigem()
    {
        try
        {
            // ✅ KENDO: cmbOrigem agora usa Kendo ComboBox
            const origem = $("#cmbOrigem").data("kendoComboBox")?.value();

            if (origem === "" || origem === null)
            {
                await Alerta.Erro("Informação Ausente", "A Origem é obrigatória");
                return false;
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarOrigem", error);
            return false;
        }
    }

    /**
     * Valida destino
     */
    async validarDestino()
    {
        try
        {
            // ✅ KENDO: cmbDestino agora usa Kendo ComboBox
            const destino = $("#cmbDestino").data("kendoComboBox")?.value();

            if (destino === "" || destino === null)
            {
                await Alerta.Erro("Informação Ausente", "O Destino é obrigatório");
                return false;
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarDestino", error);
            return false;
        }
    }

    /**
     * Verifica se algum campo de finalização foi preenchido
     */
    verificarCamposFinalizacao()
    {
        try
        {
            const dataFinal = $("#txtDataFinal").val();
            const horaFinal = $("#txtHoraFinal").val();
            // ✅ KENDO: ddtCombustivelFinal agora usa Kendo DropDownList
            const ddlCombFinal = $("#ddtCombustivelFinal").data("kendoDropDownList");
            const combustivelFinal = ddlCombFinal ? ddlCombFinal.value() : null;
            const kmFinal = $("#txtKmFinal").val();

            return dataFinal || horaFinal || combustivelFinal || kmFinal;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "verificarCamposFinalizacao", error);
            return false;
        }
    }

    /**
     * Valida campos de finalização
     */
    async validarFinalizacao()
    {
        try
        {
            const dataFinal = $("#txtDataFinal").val();
            const horaFinal = $("#txtHoraFinal").val();
            // ✅ KENDO: ddtCombustivelFinal agora usa Kendo DropDownList
            const ddlCombFinalVal = $("#ddtCombustivelFinal").data("kendoDropDownList");
            const combustivelFinal = ddlCombFinalVal ? ddlCombFinalVal.value() : null;
            const kmFinal = $("#txtKmFinal").val();

            const todosFinalPreenchidos = dataFinal && horaFinal && combustivelFinal && kmFinal;

            if (!todosFinalPreenchidos)
            {
                await Alerta.Erro(
                    "Campos de Finalização Incompletos", 
                    "Para gravar uma viagem como 'Realizada', é necessário preencher todos os campos de Finalização:\n\n• Data Final\n• Hora Final\n• Km Final\n• Combustível Final"
                );
                return false;
            }

            // Validação: Data Final não pode ser superior à data atual
            if (dataFinal)
            {
                const dtFinal = window.parseDate ? window.parseDate(dataFinal) : new Date(dataFinal);
                const dtAtual = new Date();
                
                // Zerar horas para comparar apenas datas
                dtFinal.setHours(0, 0, 0, 0);
                dtAtual.setHours(0, 0, 0, 0);
                
                if (dtFinal > dtAtual)
                {
                    await Alerta.Erro(
                        "Data Inválida", 
                        "A Data Final não pode ser superior à data atual."
                    );
                    window.setKendoDateValue("txtDataFinal", null);
                    document.getElementById("txtDataFinal")?.focus();
                    return false;
                }
            }

            // Validar destino quando finalizado (✅ KENDO: cmbDestino agora usa Kendo ComboBox)
            const destino = $("#cmbDestino").data("kendoComboBox")?.value();
            if (destino === "" || destino === null)
            {
                await Alerta.Erro("Informação Ausente", "O Destino é obrigatório para finalizar a viagem");
                return false;
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarFinalizacao", error);
            return false;
        }
    }

    /**
     * Valida campos específicos de viagem
     */
    async validarCamposViagem()
    {
        try
        {
            // REMOVIDO: Ficha de Vistoria não é mais obrigatória
            // Se não informada, será gravada como 0

            // ✅ KENDO: lstMotorista agora usa Kendo ComboBox
            const cmbMotorista = $("#lstMotorista").data("kendoComboBox");
            const motorista = cmbMotorista ? cmbMotorista.value() : null;
            if (motorista === null || motorista === "")
            {
                await Alerta.Erro("Informação Ausente", "O Motorista é obrigatório");
                return false;
            }

            // ✅ KENDO: lstVeiculo agora usa Kendo ComboBox
            const cmbVeiculo = $("#lstVeiculo").data("kendoComboBox");
            const veiculo = cmbVeiculo ? cmbVeiculo.value() : null;
            if (veiculo === null || veiculo === "")
            {
                await Alerta.Erro("Informação Ausente", "O Veículo é obrigatório");
                return false;
            }

            // Validar km
            const kmOk = await this.validarKmInicialFinal();
            if (!kmOk) return false;

            // ✅ KENDO: ddtCombustivelInicial agora usa Kendo DropDownList
            const ddlCombInicial = $("#ddtCombustivelInicial").data("kendoDropDownList");
            const combInicial = ddlCombInicial ? ddlCombInicial.value() : null;
            if (combInicial === "" || combInicial === null)
            {
                await Alerta.Erro("Informação Ausente", "O Combustível Inicial é obrigatório");
                return false;
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarCamposViagem", error);
            return false;
        }
    }

    /**
     * Valida requisitante
     */
    async validarRequisitante()
    {
        try
        {
            // Telerik Kendo: usa $(element).data("kendoComboBox")
            const lstRequisitanteEl = document.getElementById("lstRequisitante");
            const kendoComboBox = lstRequisitanteEl ? $(lstRequisitanteEl).data("kendoComboBox") : null;

            const valorRequisitante = kendoComboBox ? kendoComboBox.value() : null;

            if (!valorRequisitante || valorRequisitante === "")
            {
                await Alerta.Erro("Informação Ausente", "O Requisitante é obrigatório");
                return false;
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarRequisitante", error);
            return false;
        }
    }

    /**
     * Valida ramal (VERSÃO CORRIGIDA)
     * Agora valida o campo correto: txtRamalRequisitanteSF
     */
    async validarRamal()
    {
        try
        {
            // ✅ KENDO: Ramal usa jQuery val() (input de texto simples)
            // Tenta campo Syncfusion primeiro, depois fallback para campo HTML padrão
            let valorRamal = $("#txtRamalRequisitanteSF").val();

            if (!valorRamal)
            {
                // Fallback: tentar validar o input HTML padrão
                valorRamal = $("#txtRamalRequisitante").val();
            }

            if (!valorRamal || valorRamal === "")
            {
                await Alerta.Erro("Informação Ausente", "O Ramal do Requisitante é obrigatório");
                return false;
            }

            return true;

        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarRamal", error);
            return false;
        }
    }

    /**
     * Valida setor (VERSÃO CORRIGIDA)
     * Agora valida o campo correto: lstSetorRequisitanteAgendamento
     */
    async validarSetor()
    {
        try
        {
            // Usar o nome correto do campo
            const lstSetorElement = document.getElementById("lstSetorRequisitanteAgendamento");

            // Verificar se o elemento existe
            if (!lstSetorElement)
            {
                console.error("❌ Elemento lstSetorRequisitanteAgendamento não encontrado");
                await Alerta.Erro("Informação Ausente", "O Setor do Requisitante é obrigatório");
                return false;
            }

            // Verificar se está visível (pode estar oculto em alguns casos)
            const isVisible = lstSetorElement.offsetWidth > 0 && lstSetorElement.offsetHeight > 0;
            if (!isVisible)
            {
                return true; // Se está oculto, não valida
            }

            // ✅ SYNCFUSION BRIDGE: lstSetorRequisitanteAgendamento usa DropDownTree (Syncfusion)
            const lstSetor = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstSetorRequisitanteAgendamento") : null;
            if (!lstSetor)
            {
                await Alerta.Erro("Informação Ausente", "O Setor do Requisitante é obrigatório");
                return false;
            }

            const valorSetor = lstSetor.value;

            // Validar o valor (pode ser array ou valor único)
            if (!valorSetor ||
                valorSetor === "" ||
                valorSetor === null ||
                (Array.isArray(valorSetor) && valorSetor.length === 0))
            {
                await Alerta.Erro("Informação Ausente", "O Setor do Requisitante é obrigatório");
                return false;
            }

            console.log("✅ Setor validado:", valorSetor);
            return true;

        } catch (error)
        {
            console.error("❌ Erro em validarSetor:", error);
            Alerta.TratamentoErroComLinha("validacao.js", "validarSetor", error);
            return false;
        }
    }

    /**
     * Valida evento
     */
    async validarEvento()
    {
        try
        {
            // ✅ KENDO: lstFinalidade agora usa Kendo DropDownList
            const ddlFin = $("#lstFinalidade").data("kendoDropDownList");
            const finalidade = ddlFin ? ddlFin.value() : null;

            if (finalidade && finalidade[0] === "Evento")
            {
                // ✅ KENDO: lstEventos agora usa Kendo ComboBox
                const cmbEventos = $("#lstEventos").data("kendoComboBox");
                const evento = cmbEventos ? cmbEventos.value() : null;

                if (evento === "" || evento === null)
                {
                    await Alerta.Erro("Informação Ausente", "O Nome do Evento é obrigatório");
                    return false;
                }
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarEvento", error);
            return false;
        }
    }

    /**
     * Valida recorrência
     */
    async validarRecorrencia()
    {
        try
        {
            // ✅ KENDO: lstRecorrente agora usa Kendo DropDownList
            const ddlRecorrente = $("#lstRecorrente").data("kendoDropDownList");
            const recorrente = ddlRecorrente ? ddlRecorrente.value() : null;

            // ✅ SYNCFUSION BRIDGE: lstPeriodos ainda usa Syncfusion DropDownList
            const sfPeriodos = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstPeriodos") : null;
            const periodo = sfPeriodos ? sfPeriodos.value : null;

            // Validação 1: Se recorrente = Sim, Período é obrigatório
            if (recorrente === "S" && (!periodo || periodo === ""))
            {
                await Alerta.Erro("Informação Ausente", "Se o Agendamento é Recorrente, você precisa escolher o Período de Recorrência");
                return false;
            }

            // Validação 2: Semanal/Quinzenal → Dias da Semana obrigatório
            if (periodo === "S" || periodo === "Q")
            {
                // ✅ KENDO: lstDias agora usa Kendo MultiSelect
                const mseDias = $("#lstDias").data("kendoMultiSelect");
                const diasSelecionados = mseDias ? mseDias.value() : [];

                if (!diasSelecionados || diasSelecionados.length === 0)
                {
                    await Alerta.Erro("Informação Ausente", "Para período Semanal ou Quinzenal, você precisa escolher ao menos um Dia da Semana");
                    return false;
                }
            }

            // Validação 3: Mensal → Dia do Mês obrigatório
            if (periodo === "M")
            {
                // ✅ KENDO: lstDiasMes agora usa Kendo DropDownList
                const ddlDiasMes = $("#lstDiasMes").data("kendoDropDownList");
                const diaMes = ddlDiasMes ? ddlDiasMes.value() : null;

                if (!diaMes || diaMes === "" || diaMes === null)
                {
                    await Alerta.Erro("Informação Ausente", "Para período Mensal, você precisa escolher o Dia do Mês");
                    return false;
                }
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarRecorrencia", error);
            return false;
        }
    }

    /**
     * Valida período de recorrência
     */
    async validarPeriodoRecorrencia()
    {
        try
        {
            // ✅ SYNCFUSION BRIDGE: lstPeriodos ainda usa Syncfusion DropDownList
            const sfPeriodos = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstPeriodos") : null;
            const periodo = sfPeriodos ? sfPeriodos.value : null;

            if ((periodo === "D" || periodo === "S" || periodo === "Q" || periodo === "M"))
            {
                const dataFinal = window.getKendoDateValue("txtFinalRecorrencia");

                if (dataFinal === "" || dataFinal === null)
                {
                    await Alerta.Erro("Informação Ausente", "Se o período foi escolhido como diário, semanal, quinzenal ou mensal, você precisa escolher a Data Final");
                    return false;
                }
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarPeriodoRecorrencia", error);
            return false;
        }
    }

    /**
     * Valida dias variados
     */
    async validarDiasVariados()
    {
        try
        {
            // ✅ SYNCFUSION BRIDGE: lstPeriodos ainda usa Syncfusion DropDownList
            const sfPeriodosV = window.getSyncfusionInstance ? window.getSyncfusionInstance("lstPeriodos") : null;
            const periodo = sfPeriodosV ? sfPeriodosV.value : null;

            if (periodo === "V")
            {
                // ✅ SYNCFUSION BRIDGE: calDatasSelecionadas usa Syncfusion Calendar
                const calendarObj = window.getSyncfusionInstance ? window.getSyncfusionInstance("calDatasSelecionadas") : null;

                if (!calendarObj)
                {
                    // Calendário não disponível (provavelmente está editando agendamento existente)
                    // Neste caso, a validação não se aplica pois os dias já estão definidos
                    return true;
                }

                const selectedDates = calendarObj.values;

                if (!selectedDates || selectedDates.length === 0)
                {
                    await Alerta.Erro("Informação Ausente", "Se o período foi escolhido como Dias Variados, você precisa escolher ao menos um dia no Calendário");
                    return false;
                }
            }

            return true;
        }
        catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarDiasVariados", error);
            return false;
        }
    }

    /**
     * Valida km inicial vs final
     */
    async validarKmInicialFinal()
    {
        try
        {
            const kmInicial = $("#txtKmInicial").val();
            const kmFinal = $("#txtKmFinal").val();

            if (!kmInicial || !kmFinal) return true;

            const ini = parseFloat(kmInicial.replace(",", "."));
            const fim = parseFloat(kmFinal.replace(",", "."));

            // Validação: Km Final deve ser maior que Km Inicial
            if (fim < ini)
            {
                await Alerta.Erro("Erro", "A quilometragem final deve ser maior que a inicial.");
                return false;
            }

            // Validação: Km Final não pode exceder Km Inicial em mais de 2.000km
            const diff = fim - ini;
            if (diff > 2000)
            {
                await Alerta.Erro(
                    "Quilometragem Inválida", 
                    `A quilometragem final não pode exceder a inicial em mais de 2.000 km.\n\nDiferença informada: ${diff.toLocaleString('pt-BR')} km`
                );
                $("#txtKmFinal").val("");
                $("#txtKmFinal").focus();
                return false;
            }

            // Alerta (não bloqueante) se diferença > 100km
            // Só perguntar se ainda não foi confirmado nesta sessão de validação
            if (diff > 100 && !this._kmConfirmado)
            {
                const confirmacao = await Alerta.Confirmar(
                    "Atenção",
                    "A quilometragem <strong>final</strong> excede em 100km a <strong>inicial</strong>. Tem certeza?",
                    "Tenho certeza! 💪🏼",
                    "Me enganei! 😟"
                );

                if (!confirmacao)
                {
                    $("#txtKmFinal").val("");
                    $("#txtKmFinal").focus();
                    return false;
                }
                
                // Marcar como confirmado para não perguntar novamente
                this._kmConfirmado = true;
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarKmInicialFinal", error);
            return false;
        }
    }

    /**
     * Valida km final
     */
    async validarKmFinal()
    {
        try
        {
            const kmFinal = $("#txtKmFinal").val();

            if (kmFinal && parseFloat(kmFinal) <= 0)
            {
                await Alerta.Erro("Informação Incorreta", "A Quilometragem Final deve ser maior que zero");
                return false;
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "validarKmFinal", error);
            return false;
        }
    }

    /**
     * Confirma finalização da viagem
     */
    async confirmarFinalizacao()
    {
        try
        {
            const dataFinal = $("#txtDataFinal").val();
            const horaFinal = $("#txtHoraFinal").val();
            // ✅ KENDO: ddtCombustivelFinal agora usa Kendo DropDownList
            const ddlCombFinalConf = $("#ddtCombustivelFinal").data("kendoDropDownList");
            const combustivelFinal = ddlCombFinalConf ? ddlCombFinalConf.value() : null;
            const kmFinal = $("#txtKmFinal").val();

            const todosFinalPreenchidos = dataFinal && horaFinal && combustivelFinal && kmFinal;

            // Só perguntar se ainda não foi confirmado nesta sessão de validação
            if (todosFinalPreenchidos && !this._finalizacaoConfirmada)
            {
                const confirmacao = await Alerta.Confirmar(
                    "Confirmar Fechamento",
                    'Você está criando a viagem como "Realizada". Deseja continuar?',
                    "Sim, criar!",
                    "Cancelar"
                );

                if (!confirmacao) return false;
                
                // Marcar como confirmado para não perguntar novamente
                this._finalizacaoConfirmada = true;
            }

            return true;
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("validacao.js", "confirmarFinalizacao", error);
            return false;
        }
    }
}

// Instância global
window.ValidadorAgendamento = new ValidadorAgendamento();

/**
 * Função legacy de validação (mantida para compatibilidade)
 */
window.ValidaCampos = async function (viagemId)
{
    try
    {
        return await window.ValidadorAgendamento.validar(viagemId);
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("validacao.js", "ValidaCampos", error);
        return false;
    }
};

/**
 * Validações assíncronas de datas
 */
window.validarDatas = async function ()
{
    try
    {
        const txtDataInicial = $("#txtDataInicial").val();
        const txtDataFinal = $("#txtDataFinal").val();

        if (!txtDataFinal || !txtDataInicial) return true;

        const dtInicial = window.parseDate(txtDataInicial);
        const dtFinal = window.parseDate(txtDataFinal);

        dtInicial.setHours(0, 0, 0, 0);
        dtFinal.setHours(0, 0, 0, 0);

        const diferenca = (dtFinal - dtInicial) / (1000 * 60 * 60 * 24);

        if (diferenca >= 5)
        {
            const confirmacao = await Alerta.Confirmar(
                "Atenção",
                "A Data Final está 5 dias ou mais após a Inicial. Tem certeza?",
                "Tenho certeza! 💪🏼",
                "Me enganei! 😟"
            );

            if (!confirmacao)
            {
                window.setKendoDateValue("txtDataFinal", null);
                document.getElementById("txtDataFinal")?.focus();
                return false;
            }
        }

        return true;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("validacao.js", "validarDatas", error);
        return false;
    }
};

window.validarDatasInicialFinal = async function (DataInicial, DataFinal)
{
    try
    {
        const dtIni = window.parseDate(DataInicial);
        const dtFim = window.parseDate(DataFinal);

        if (!dtIni || !dtFim || isNaN(dtIni) || isNaN(dtFim)) return true;

        const diff = (dtFim - dtIni) / (1000 * 60 * 60 * 24);

        if (diff >= 5)
        {
            const confirmacao = await Alerta.Confirmar(
                "Atenção",
                "A Data Final está 5 dias ou mais após a Inicial. Tem certeza?",
                "Tenho certeza! 💪🏼",
                "Me enganei! 😟"
            );

            if (!confirmacao)
            {
                window.setKendoDateValue("txtDataFinal", null);
                document.getElementById("txtDataFinal")?.focus();
                return false;
            }
        }

        return true;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("validacao.js", "validarDatasInicialFinal", error);
        return false;
    }
};
