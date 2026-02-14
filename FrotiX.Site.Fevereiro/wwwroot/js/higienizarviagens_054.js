/* ****************************************************************************************
 * ⚡ ARQUIVO: higienizarviagens_054.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerencia funcionalidade de higienização/unificação de origens e
 *                   destinos de viagens. Permite mover itens entre listas, normalizar
 *                   textos, e enviar unificações para API via POST.
 * 📥 ENTRADAS     : Listas Syncfusion EJ2 ListBox com origens/destinos duplicados,
 *                   valores de inputs txtNovaOrigem/txtNovoDestino
 * 📤 SAÍDAS       : POST /api/viagem/unificar, SweetAlert modals, atualização de listas
 * 🔗 CHAMADA POR  : Páginas de administração de viagens (higienização)
 * 🔄 CHAMA        : Swal.fire, fetch API, Syncfusion ListBox API, console.*, loading overlay
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 ListBox, SweetAlert2, loadingOverlayHigienizar element
 * 📝 OBSERVAÇÕES  : Normalização NFD, dedupe de listas, double-click para mover itens,
 *                   animação CSS entering/highlighted, contadores badge em tempo real
 *
 * 📋 ÍNDICE DE FUNÇÕES (14 funções + 1 event listener):
 *
 * ┌─ FUNÇÕES DE LOADING ───────────────────────────────────────────────────┐
 * │ 1. mostrarLoading(texto)                                                │
 * │    → Exibe overlay de loading e desabilita todos os botões da tela     │
 * │    → Atualiza texto do loading se fornecido                            │
 * │    → Adiciona classe .btn-disabled-loading aos botões                  │
 * │                                                                         │
 * │ 2. esconderLoading()                                                   │
 * │    → Esconde overlay de loading e reabilita todos os botões           │
 * │    → Remove classe .btn-disabled-loading dos botões                   │
 * │                                                                         │
 * │ 3. showLoading()                                                       │
 * │    → Alias para mostrarLoading() (compatibilidade)                    │
 * │                                                                         │
 * │ 4. hideLoading()                                                       │
 * │    → Alias para esconderLoading() (compatibilidade)                   │
 * └─────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES DE NORMALIZAÇÃO E MANIPULAÇÃO DE LISTAS ──────────────────────┐
 * │ 5. normalizarTexto(texto)                                              │
 * │    → Normaliza texto para comparação (NFD, remove acentos, lowercase) │
 * │    → Remove espaços múltiplos, trim, converte / para /                │
 * │    → Retorna string normalizada                                       │
 * │                                                                         │
 * │ 6. moverSelecionados(origemId, destinoId)                             │
 * │    → Move itens selecionados de uma ListBox para outra                │
 * │    → Usa normalizarTexto para comparação case-insensitive             │
 * │    → Adiciona animação entering/highlighted (1s)                      │
 * │    → Atualiza contadores de ambas as listas                           │
 * │                                                                         │
 * │ 7. atualizarListbox(id, itens)                                        │
 * │    → Atualiza dataSource de uma ListBox Syncfusion                    │
 * │    → Chama dataBind() para refresh                                    │
 * │                                                                         │
 * │ 8. atualizarContador(listBoxId)                                       │
 * │    → Atualiza badge de contagem de itens de uma ListBox              │
 * │    → Mapeamento: listOrigens → badgeOrigens, etc.                    │
 * │    → Suporta badge normal e badge com .badge-num                     │
 * │                                                                         │
 * │ 9. obterTextosDaLista(listId)                                         │
 * │    → Extrai array de strings (texto) de uma ListBox                  │
 * │    → Suporta formato string[] e objeto[] com .text                   │
 * │    → Filtra vazios e aplica trim                                     │
 * └─────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES DE GRAVAÇÃO E API ─────────────────────────────────────────────┐
 * │ 10. gravarUnificacaoViagens()                                          │
 * │     → Função legada de unificação conjunta origem+destino             │
 * │     → Valida txtUnificar + pelo menos uma seleção                     │
 * │     → POST /api/viagem/unificar com reload após sucesso              │
 * │                                                                         │
 * │ 11. gravarOrigem()                                                     │
 * │     → Grava unificação apenas de origens                              │
 * │     → Valida txtNovaOrigem + listOrigensSelecionadas não vazia       │
 * │     → Chama enviarRequisicaoUnificacao com destinosSelecionados=[]   │
 * │                                                                         │
 * │ 12. gravarDestino()                                                    │
 * │     → Grava unificação apenas de destinos                             │
 * │     → Valida txtNovoDestino + listDestinosSelecionados não vazia     │
 * │     → Chama enviarRequisicaoUnificacao com origensSelecionadas=[]    │
 * │                                                                         │
 * │ 13. enviarRequisicaoUnificacao(dados)                                 │
 * │     → Envia POST /api/viagem/unificar com loading overlay            │
 * │     → Formato JSON: { novoValor, origensSelecionadas, destinosSelecionados } │
 * │     → Exibe SweetAlert sucesso/erro, reload após sucesso             │
 * └─────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ EVENT LISTENERS ───────────────────────────────────────────────────────┐
 * │ 14. DOMContentLoaded handler                                           │
 * │     → Configura double-click em 4 ListBoxes para mover itens          │
 * │     → Pares: listOrigens ↔ listOrigensSelecionadas                   │
 * │     │        listDestinos ↔ listDestinosSelecionados                 │
 * │     → Adiciona listeners em btnGravarOrigem e btnGravarDestino       │
 * └─────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE UNIFICAÇÃO:
 * 1. Usuário move itens entre listas (double-click ou arrastar)
 * 2. Preenche novo valor (txtNovaOrigem ou txtNovoDestino)
 * 3. Clica em btnGravarOrigem ou btnGravarDestino
 * 4. Validação de campos (novo valor não vazio + pelo menos 1 item selecionado)
 * 5. enviarRequisicaoUnificacao mostra loading e faz POST /api/viagem/unificar
 * 6. API processa unificação no banco de dados
 * 7. SweetAlert de sucesso → location.reload() para atualizar listas
 *
 * 📌 FORMATO JSON API:
 * POST /api/viagem/unificar
 * {
 *   "novoValor": "São Paulo - SP",
 *   "origensSelecionadas": ["sao paulo", "SP", "São Paulo"],
 *   "destinosSelecionados": []
 * }
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - normalizarTexto usa NFD (Canonical Decomposition) para remover acentos
 * - moverSelecionados suporta tanto string[] quanto objeto[] com .text
 * - Animação CSS: .entering + .highlighted (1000ms)
 * - Loading overlay desabilita TODOS os botões da página (não apenas os de ação)
 * - Badge mapping hardcoded para 4 ListBoxes específicas
 *
 * 🔌 VERSÃO: 2.0 (Padrão FrotiX Simplificado)
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

// ===== FUNÇÕES DE LOADING - PADRÃO FROTIX =====
function mostrarLoading(texto) {
    try {
        const overlay = document.getElementById('loadingOverlayHigienizar');
        if (overlay) {
            if (texto) {
                const loadingText = overlay.querySelector('.ftx-loading-text');
                if (loadingText) loadingText.textContent = texto;
            }
            overlay.style.display = 'flex';
        }
        // Desabilita todos os botões da tela
        document.querySelectorAll('button, input[type="button"], input[type="submit"]').forEach(btn => {
            btn.disabled = true;
            btn.classList.add('btn-disabled-loading');
        });
    } catch (erro) {
        console.error('[FrotiX] Erro em mostrarLoading:', erro);
    }
}

function esconderLoading() {
    try {
        const overlay = document.getElementById('loadingOverlayHigienizar');
        if (overlay) {
            overlay.style.display = 'none';
        }
        // Reabilita todos os botões
        document.querySelectorAll('button, input[type="button"], input[type="submit"]').forEach(btn => {
            btn.disabled = false;
            btn.classList.remove('btn-disabled-loading');
        });
    } catch (erro) {
        console.error('[FrotiX] Erro em esconderLoading:', erro);
    }
}

// Alias para compatibilidade com código existente
function showLoading() {
    try {
        mostrarLoading();
    } catch (erro) {
        console.error('[FrotiX] Erro em showLoading:', erro);
    }
}

function hideLoading() {
    try {
        esconderLoading();
    } catch (erro) {
        console.error('[FrotiX] Erro em hideLoading:', erro);
    }
}

function normalizarTexto(texto)
{
    try {
        return texto
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .replace(/\s+/g, ' ')
            .replace(/\//g, '/')
            .trim()
            .toLowerCase();
    } catch (erro) {
        console.error('[FrotiX] Erro em normalizarTexto:', erro);
        return String(texto || '').toLowerCase();
    }
}

function moverSelecionados(origemId, destinoId)
{
    try
    {
        const origem = document.getElementById(origemId).ej2_instances[0];
        const destino = document.getElementById(destinoId).ej2_instances[0];

        const selectedElements = origem.getSelectedItems();
        const textosSelecionados = selectedElements.map(el => el.innerText.trim()).filter(Boolean);
        const dataOrigem = origem.getDataList();
        const dataDestino = destino.getDataList();

        if (!textosSelecionados.length) return;

        // Detecta se a lista é de objetos ou strings
        const itensParaMover = textosSelecionados.map(texto =>
        {
            const textoNormalizado = normalizarTexto(texto);
            const item = dataOrigem.find(i =>
            {
                const valor = typeof i === 'string' ? i : i.text;
                if (!valor) return false;
                // Adicione o log aqui 👇
                console.log(
                    '[Comparação]',
                    'valor:', valor,
                    '| normalizado:', normalizarTexto(valor),
                    '| texto:', texto,
                    '| textoNormalizado:', textoNormalizado
                );
                return normalizarTexto(valor) === textoNormalizado;
            });
            return item
                ? (typeof item === 'string' ? { text: item, value: item } : item)
                : null;
        }).filter(Boolean);

        // Adiciona na lista de destino
        destino.addItems(itensParaMover);

        // Remove da lista de origem (agora cobre ambos os formatos)
        origem.dataSource = dataOrigem.filter(item => !textosSelecionados.includes(typeof item === 'string' ? item : item.text));
        origem.dataBind();

        // Animação opcional nos itens adicionados
        setTimeout(() =>
        {
            const listboxElement = destino.element;
            const items = listboxElement.querySelectorAll("li.e-list-item");
            items.forEach(item =>
            {
                if (textosSelecionados.includes(item.textContent.trim()))
                {
                    item.classList.add("entering", "highlighted");
                    setTimeout(() => item.classList.remove("entering", "highlighted"), 1000);
                }
            });
        }, 200);

        atualizarContador(origemId);
        atualizarContador(destinoId);
    } catch (error)
    {
        console.error(error);
        Swal.fire("Erro", "Falha ao mover itens: " + error.message, "error");
    }
}

document.addEventListener("DOMContentLoaded", function () {
    try {
        const pares = [
            ["listOrigens", "listOrigensSelecionadas"],
            ["listOrigensSelecionadas", "listOrigens"],
            ["listDestinos", "listDestinosSelecionados"],
            ["listDestinosSelecionados", "listDestinos"]
        ];

        pares.forEach(([origemId, destinoId]) => {
            const list = document.getElementById(origemId).ej2_instances[0];
            list.element.addEventListener("dblclick", () => moverSelecionados(origemId, destinoId));
        });

        document.getElementById("btnGravarOrigem").addEventListener("click", gravarOrigem);
        document.getElementById("btnGravarDestino").addEventListener("click", gravarDestino);
    } catch (error) {
        console.error(error);
        Swal.fire("Erro", "Erro na inicialização: " + error.message, "error");
    }
});

function gravarUnificacaoViagens() {
    try {
        const novoValor = document.getElementById("txtUnificar").value.trim();
        const origemList = document.getElementById("listOrigensSelecionadas").ej2_instances[0];
        const destinoList = document.getElementById("listDestinosSelecionados").ej2_instances[0];

        const origens = obterTextosDaLista("listOrigensSelecionadas");
        const destinos = obterTextosDaLista("listDestinosSelecionados");

        if (!novoValor || (origens.length === 0 && destinos.length === 0)) {
            Swal.fire("Atenção", "Informe o novo valor e selecione pelo menos uma origem ou destino.", "warning");
            return;
        }

        const dados = {
            novoValor: novoValor,
            origensSelecionadas: origens,
            destinosSelecionados: destinos
        };

        fetch('/api/viagem/unificar', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dados)
        })
            .then(response => {
                if (!response.ok) throw new Error('Erro ao gravar a unificação.');
                return response.json();
            })
            .then(() => {
                Swal.fire("Sucesso", "Unificação concluída com sucesso!", "success").then(() => location.reload());
            })
            .catch(error => {
                console.error(error);
                Swal.fire("Erro", error.message, "error");
            });

    } catch (error) {
        console.error(error);
        Swal.fire("Erro", "Erro ao preparar a unificação: " + error.message, "error");
    }
}

function gravarOrigem() {
    try {
        const novaOrigem = document.getElementById("txtNovaOrigem").value.trim();
        const origensSelecionadas = obterTextosDaLista("listOrigensSelecionadas");

        if (!novaOrigem) {
            Swal.fire("Atenção", "Informe o novo valor de origem.", "warning");
            return;
        }

        if (origensSelecionadas.length === 0) {
            Swal.fire("Atenção", "Selecione ao menos uma origem para unificar.", "warning");
            return;
        }

        const dados = {
            novoValor: novaOrigem,
            origensSelecionadas: origensSelecionadas,
            destinosSelecionados: []
        };

        enviarRequisicaoUnificacao(dados);

    } catch (error) {
        console.error(error);
        Swal.fire("Erro", "Falha ao gravar origem: " + error.message, "error");
    }
}

function gravarDestino() {
    try {
        const novoDestino = document.getElementById("txtNovoDestino").value.trim();
        const destinosSelecionados = obterTextosDaLista("listDestinosSelecionados");

        if (!novoDestino) {
            Swal.fire("Atenção", "Informe o novo valor de destino.", "warning");
            return;
        }

        if (destinosSelecionados.length === 0) {
            Swal.fire("Atenção", "Selecione ao menos um destino para unificar.", "warning");
            return;
        }

        const dados = {
            novoValor: novoDestino,
            destinosSelecionados: destinosSelecionados,
            origensSelecionadas: []
        };

        enviarRequisicaoUnificacao(dados);

    } catch (error) {
        console.error(error);
        Swal.fire("Erro", "Falha ao gravar destino: " + error.message, "error");
    }
}

function enviarRequisicaoUnificacao(dados)
{
    try
    {
        showLoading();
        fetch('/api/viagem/unificar', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dados)
        })
            .then(response =>
            {
                if (!response.ok) throw new Error("Erro ao gravar a unificação.");
                return response.json();
            })
            .then(() =>
            {
                hideLoading();
                Swal.fire("Sucesso", "Unificação realizada com sucesso!", "success").then(() => location.reload());
            })
            .catch(error =>
            {
                hideLoading();
                console.error(error);
                Swal.fire("Erro", error.message, "error");
            });
    } catch (error)
    {
        hideLoading();
        console.error(error);
        Swal.fire("Erro", "Falha ao enviar os dados: " + error.message, "error");
    }
}

function atualizarListbox(id, itens) {
    try {
        const listbox = document.getElementById(id).ej2_instances[0];
        listbox.dataSource = itens;
        listbox.dataBind();
    } catch (error) {
        console.error(error);
        Swal.fire("Erro", "Falha ao atualizar lista: " + error.message, "error");
    }
}

function atualizarContador(listBoxId)
{
    try
    {
        const list = document.getElementById(listBoxId).ej2_instances[0];
        const count = list.getDataList().length;

        const badgeMap = {
            listOrigens: "badgeOrigens",
            listOrigensSelecionadas: "badgeOrigensSelecionadas",
            listDestinos: "badgeDestinos",
            listDestinosSelecionados: "badgeDestinosSelecionados"
        };

        const badgeId = badgeMap[listBoxId];
        if (badgeId)
        {
            const badge = document.getElementById(badgeId);
            if (badge)
            {
                // badge normal
                const badgeNum = badge.querySelector('.badge-num');
                if (badgeNum)
                {
                    badgeNum.innerText = count;
                } else
                {
                    badge.innerText = count;
                }
            }
        }
    } catch (error)
    {
        console.error(error);
        Swal.fire("Erro", "Falha ao atualizar contador: " + error.message, "error");
    }
}
function obterTextosDaLista(listId) {
    try {
        const list = document.getElementById(listId).ej2_instances[0];
        const lista = list.getDataList();

        return lista
            .map(item => typeof item === 'string' ? item : item?.text)
            .filter(texto => !!texto && texto.trim())
            .map(texto => texto.trim());

    } catch (error) {
        console.error(error);
        Swal.fire("Erro", "Erro ao ler itens da lista: " + error.message, "error");
        return [];
    }
}
