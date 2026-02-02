/* ****************************************************************************************
 * ⚡ ARQUIVO: viagem.service.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Serviço de negócio para operações de viagens. Encapsula validações
 *                   de ficha de vistoria (verifica intervalo válido ±100, existência),
 *                   verificação de status (aberta/fechada), recuperação de usuário por
 *                   ID, e listagem de setores solicitantes (com hierarquia pai/filho).
 *                   Usa window.ApiClient para requests GET, retorna objetos padronizados
 *                   {success, data/error}.
 * 📥 ENTRADAS     : viagemId (string), usuário ID (string), noFicha (string/number),
 *                   nenhum param para listarSetores
 * 📤 SAÍDAS       : Promises<Object> com {success: boolean, data/error: any, extras?}
 *                   - verificarStatus: {success, data: boolean isAberta}
 *                   - recuperarUsuario: {success, data: string nomeUsuario}
 *                   - verificarFicha: {success, maxFicha: number, diferencaGrande: boolean}
 *                   - fichaExiste: {success, existe: boolean}
 *                   - listarSetores: {success, data: Array<Setor>}
 *                   Em caso de erro: {success:false, error: error.message, data?: fallback}
 * 🔗 CHAMADA POR  : Event handlers (ficha change), validadores (validacao.js), componentes
 *                   de formulário, exibe-viagem.js, outros services
 * 🔄 CHAMA        : window.ApiClient.get, $.ajax (jQuery), $.each, parseInt, criarErroAjax,
 *                   Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : window.ApiClient (api-client.js), jQuery ($.ajax, $.each), Alerta.js,
 *                   criarErroAjax function
 * 📝 OBSERVAÇÕES  : Exporta window.ViagemService (instância global). Todos os métodos
 *                   async retornam Promises. verificarFicha e fichaExiste usam $.ajax
 *                   direto (não ApiClient) com Promise wrapper. recuperarUsuario usa
 *                   $.each para iterar object keys (poderia usar Object.values).
 *                   verificarFicha calcula diferencaGrande = |noFicha - maxFicha| > 100
 *                   (regra de negócio). listarSetores mapeia response para format
 *                   {SetorSolicitanteId, SetorPaiId, Nome, HasChild} (tree structure).
 *
 * 📋 ÍNDICE DE MÉTODOS DA CLASSE (5 métodos + constructor):
 *
 * ┌─ CONSTRUCTOR ─────────────────────────────────────────────────────────┐
 * │ 1. constructor()                                                      │
 * │    → Inicializa this.api = window.ApiClient                          │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ STATUS E USUÁRIOS ───────────────────────────────────────────────────┐
 * │ 2. async verificarStatus(viagemId)                                    │
 * │    → param {string} viagemId - ID da viagem                           │
 * │    → returns {Promise<Object>} {success, data: boolean}               │
 * │    → GET /api/Viagem/PegarStatusViagem?viagemId={id}                  │
 * │    → success: {success:true, data: isAberta}                          │
 * │    → error: {success:false, error: error.message, data: false}        │
 * │    → try-catch: Alerta.TratamentoErroComLinha("verificarStatus")      │
 * │                                                                        │
 * │ 3. async recuperarUsuario(id)                                         │
 * │    → param {string} id - ID do usuário                                │
 * │    → returns {Promise<Object>} {success, data: string nomeUsuario}    │
 * │    → GET /api/Agenda/RecuperaUsuario?id={id}                          │
 * │    → Itera response com $.each(data, (key, val) => nomeUsuario=val)  │
 * │    → success: {success:true, data: nomeUsuario}                       │
 * │    → error: {success:false, error: error.message, data: ""}           │
 * │    → try-catch: Alerta.TratamentoErroComLinha("recuperarUsuario")     │
 * │    → NOTA: $.each desnecessário, poderia usar Object.values(data)[0]  │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ VALIDAÇÃO DE FICHA DE VISTORIA ─────────────────────────────────────┐
 * │ 4. async verificarFicha(noFicha)                                      │
 * │    → param {string} noFicha - Número da ficha de vistoria            │
 * │    → returns {Promise<Object>} {success, maxFicha, diferencaGrande}  │
 * │    → Usa $.ajax direto (não ApiClient) + Promise wrapper             │
 * │    → GET /Viagens/Upsert?handler=VerificaFicha&id={noFicha}          │
 * │    → maxFicha = parseInt(res.data)                                   │
 * │    → diferencaGrande = noFicha > maxFicha+100 || noFicha < maxFicha-100│
 * │    → success: resolve({success:true, maxFicha, diferencaGrande})     │
 * │    → error: criarErroAjax + Alerta.TratamentoErroComLinha + reject   │
 * │    → try-catch: Alerta.TratamentoErroComLinha + {success:false, error}│
 * │    → REGRA DE NEGÓCIO: ficha deve estar dentro de ±100 de maxFicha   │
 * │                                                                        │
 * │ 5. async fichaExiste(noFicha)                                         │
 * │    → param {string} noFicha - Número da ficha                        │
 * │    → returns {Promise<Object>} {success, existe: boolean}            │
 * │    → Usa $.ajax direto (não ApiClient) + Promise wrapper             │
 * │    → GET /Viagens/Upsert?handler=FichaExistente&id={noFicha}         │
 * │    → success: resolve({success:true, existe: res.data===true})       │
 * │    → error: criarErroAjax + Alerta.TratamentoErroComLinha + reject   │
 * │    → try-catch: {success:false, error: error.message, existe: false} │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ LISTAGEM DE SETORES ────────────────────────────────────────────────┐
 * │ 6. async listarSetores()                                              │
 * │    → returns {Promise<Object>} {success, data: Array<Setor>}         │
 * │    → Usa $.ajax direto (não ApiClient) + Promise wrapper             │
 * │    → GET /Viagens/Upsert?handler=AJAXPreencheListaSetores            │
 * │    → Mapeia res.data para formato consistente:                       │
 * │      {                                                                │
 * │        SetorSolicitanteId: item.setorSolicitanteId,                  │
 * │        SetorPaiId: item.setorPaiId,                                  │
 * │        Nome: item.nome,                                              │
 * │        HasChild: item.hasChild                                       │
 * │      }                                                                │
 * │    → success: resolve({success:true, data: setores})                 │
 * │    → error: criarErroAjax + Alerta.TratamentoErroComLinha + reject   │
 * │    → try-catch: {success:false, error: error.message, data: []}      │
 * │    → Usado para popular DropDownTree com hierarquia de setores       │
 * └───────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE VALIDAÇÃO DE FICHA:
 * 1. Usuário preenche campo noFicha (ex: "12345")
 * 2. Event handler chama ViagemService.verificarFicha("12345")
 * 3. AJAX GET VerificaFicha retorna maxFicha (ex: 12300)
 * 4. Calcula diferencaGrande:
 *    - 12345 > 12300+100 (12400)? false
 *    - 12345 < 12300-100 (12200)? false
 *    - diferencaGrande = false (OK, dentro do intervalo)
 * 5. Retorna {success:true, maxFicha: 12300, diferencaGrande: false}
 * 6. Código chamador decide se mostra warning ou permite continuar
 *
 * 🔄 FLUXO DE VERIFICAÇÃO DE EXISTÊNCIA:
 * 1. Usuário preenche noFicha (ex: "12000")
 * 2. Event handler chama ViagemService.fichaExiste("12000")
 * 3. AJAX GET FichaExistente retorna res.data=true (ficha já existe)
 * 4. Retorna {success:true, existe: true}
 * 5. Código chamador mostra erro "Ficha já cadastrada"
 *
 * 🔄 FLUXO DE LISTAGEM DE SETORES:
 * 1. Componente DropDownTree precisa popular setores
 * 2. Chama ViagemService.listarSetores()
 * 3. AJAX GET AJAXPreencheListaSetores retorna array de setores
 * 4. Mapeia para formato consistente (PascalCase)
 * 5. Retorna {success:true, data: [...]}
 * 6. DropDownTree usa data para criar árvore (pai→filho via SetorPaiId)
 *
 * 📌 ESTRUTURA DE RETORNO PADRONIZADA:
 * - Sucesso: {success: true, data?: any, extras?}
 * - Falha: {success: false, error: string, data?: fallback}
 * - verificarFicha: {success, maxFicha, diferencaGrande} (sem data)
 * - fichaExiste: {success, existe} (sem data)
 * - Outros: {success, data: value, error?: string}
 *
 * 📌 MÉTODOS QUE USAM ApiClient vs $.ajax:
 * - ApiClient: verificarStatus, recuperarUsuario
 * - $.ajax direto: verificarFicha, fichaExiste, listarSetores
 * - Motivo: métodos com Promise wrapper manual usam $.ajax
 * - Possível refactor: migrar todos para ApiClient
 *
 * 📌 REGRA DE NEGÓCIO - FICHA DE VISTORIA:
 * - Intervalo válido: maxFicha ± 100
 * - Se noFicha > maxFicha+100 OU noFicha < maxFicha-100: diferencaGrande=true
 * - Exemplo: maxFicha=12000
 *   - 11900 a 12100: OK (diferencaGrande=false)
 *   - 11899 ou 12101: WARNING (diferencaGrande=true)
 *
 * 📌 ESTRUTURA DE SETOR:
 * - SetorSolicitanteId: ID único do setor
 * - SetorPaiId: ID do setor pai (null se raiz)
 * - Nome: Nome do setor
 * - HasChild: boolean, tem filhos (usada por DropDownTree)
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - window.ViagemService: instância global criada (line 206)
 * - Todos os métodos são async: retornam Promises
 * - recuperarUsuario usa $.each para iterar (poderia usar Object.values()[0])
 * - verificarFicha retorna objeto diferente (sem propriedade data)
 * - listarSetores mapeia camelCase→PascalCase para consistência
 * - Todos têm try-catch completo com Alerta.TratamentoErroComLinha
 * - Promise wrappers manuais (new Promise + resolve/reject) em 3 métodos
 * - criarErroAjax cria erro estruturado de jqXHR (função externa)
 * - fichaExiste retorna existe=false em fallback (safe default)
 * - listarSetores retorna data=[] em fallback (safe default)
 * - verificarStatus retorna data=false em fallback (safe default: fechada)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

class ViagemService
{
    constructor()
    {
        this.api = window.ApiClient;
    }

    /**
     * Verifica status da viagem (aberta/fechada)
     * param {string} viagemId - ID da viagem
     * returns {Promise<boolean>} true se aberta, false se fechada
     */
    async verificarStatus(viagemId)
    {
        try
        {
            const isAberta = await this.api.get('/api/Viagem/PegarStatusViagem', { viagemId });

            return {
                success: true,
                data: isAberta
            };
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("viagem.service.js", "verificarStatus", error);
            return {
                success: false,
                error: error.message,
                data: false
            };
        }
    }

    /**
     * Recupera usuário por ID
     * param {string} id - ID do usuário
     * returns {Promise<string>} Nome do usuário
     */
    async recuperarUsuario(id)
    {
        try
        {
            const data = await this.api.get('/api/Agenda/RecuperaUsuario', { id });

            let nomeUsuario = "";
            $.each(data, function (key, val)
            {
                nomeUsuario = val;
            });

            return {
                success: true,
                data: nomeUsuario
            };
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("viagem.service.js", "recuperarUsuario", error);
            return {
                success: false,
                error: error.message,
                data: ""
            };
        }
    }

    /**
     * Verifica número de ficha de vistoria
     * param {string} noFicha - Número da ficha
     * returns {Promise<Object>} Informações da validação
     */
    async verificarFicha(noFicha)
    {
        try
        {
            return new Promise((resolve, reject) =>
            {
                $.ajax({
                    url: "/Viagens/Upsert?handler=VerificaFicha",
                    method: "GET",
                    datatype: "json",
                    data: { id: noFicha },
                    success: function (res)
                    {
                        const maxFicha = parseInt(res.data);
                        const diferencaGrande = noFicha > maxFicha + 100 || noFicha < maxFicha - 100;

                        resolve({
                            success: true,
                            maxFicha: maxFicha,
                            diferencaGrande: diferencaGrande
                        });
                    },
                    error: function (jqXHR, textStatus, errorThrown)
                    {
                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                        Alerta.TratamentoErroComLinha("viagem.service.js", "verificarFicha", erro);
                        reject(erro);
                    }
                });
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("viagem.service.js", "verificarFicha", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Verifica se ficha já existe
     * param {string} noFicha - Número da ficha
     * returns {Promise<boolean>} true se existe
     */
    async fichaExiste(noFicha)
    {
        try
        {
            return new Promise((resolve, reject) =>
            {
                $.ajax({
                    url: "/Viagens/Upsert?handler=FichaExistente",
                    method: "GET",
                    datatype: "json",
                    data: { id: noFicha },
                    success: function (res)
                    {
                        resolve({
                            success: true,
                            existe: res.data === true
                        });
                    },
                    error: function (jqXHR, textStatus, errorThrown)
                    {
                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                        Alerta.TratamentoErroComLinha("viagem.service.js", "fichaExiste", erro);
                        reject(erro);
                    }
                });
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("viagem.service.js", "fichaExiste", error);
            return {
                success: false,
                error: error.message,
                existe: false
            };
        }
    }

    /**
     * Lista setores
     * returns {Promise<Array>} Lista de setores
     */
    async listarSetores()
    {
        try
        {
            return new Promise((resolve, reject) =>
            {
                $.ajax({
                    url: "/Viagens/Upsert?handler=AJAXPreencheListaSetores",
                    method: "GET",
                    datatype: "json",
                    success: function (res)
                    {
                        const setores = res.data.map(item => ({
                            SetorSolicitanteId: item.setorSolicitanteId,
                            SetorPaiId: item.setorPaiId,
                            Nome: item.nome,
                            HasChild: item.hasChild
                        }));

                        resolve({
                            success: true,
                            data: setores
                        });
                    },
                    error: function (jqXHR, textStatus, errorThrown)
                    {
                        const erro = criarErroAjax(jqXHR, textStatus, errorThrown, this);
                        Alerta.TratamentoErroComLinha("viagem.service.js", "listarSetores", erro);
                        reject(erro);
                    }
                });
            });
        } catch (error)
        {
            Alerta.TratamentoErroComLinha("viagem.service.js", "listarSetores", error);
            return {
                success: false,
                error: error.message,
                data: []
            };
        }
    }
}

// Instância global
window.ViagemService = new ViagemService();
