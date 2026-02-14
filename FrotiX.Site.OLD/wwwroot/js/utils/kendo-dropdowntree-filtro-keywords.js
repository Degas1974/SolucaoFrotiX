// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: kendo-dropdowntree-filtro-keywords.js                              ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Helper para filtro customizado em Kendo DropDownTree que busca em keywords. ║
// ║                                                                              ║
// ║ USO PRINCIPAL:                                                               ║
// ║ • GestaoRecursosNavegacao - filtro de ícones FontAwesome por keywords       ║
// ║                                                                              ║
// ║ CARACTERÍSTICAS:                                                             ║
// ║ • Busca hierárquica (categoria + itens filhos)                              ║
// ║ • Busca em múltiplos campos (text, label, keywords array)                   ║
// ║ • Normalização de texto (sem acentos, case insensitive)                     ║
// ║ • Mantém estrutura pai-filho visível quando filho corresponde ao filtro     ║
// ║                                                                              ║
// ║ CRIADO EM: 2026-02-14                                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

/**
 * Normaliza texto removendo acentos e convertendo para minúsculas
 * @param {string} texto - Texto a ser normalizado
 * @returns {string} Texto normalizado
 */
function normalizarTexto(texto) {
    try {
        if (!texto) return '';
        return texto
            .toString()
            .toLowerCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, ''); // Remove acentos
    } catch (erro) {
        console.error('Erro ao normalizar texto:', erro);
        return texto ? texto.toString().toLowerCase() : '';
    }
}

/**
 * Verifica se um item (ícone) corresponde ao filtro
 * @param {object} item - Item do dataSource (ícone)
 * @param {string} filtroNormalizado - Texto do filtro normalizado
 * @returns {boolean} true se o item corresponde ao filtro
 */
function itemCorrespondeAoFiltro(item, filtroNormalizado) {
    try {
        // Se o item é uma categoria, não filtra diretamente (filtra pelos filhos)
        if (item.isCategory) {
            return false;
        }

        // Busca no texto/label do item
        if (normalizarTexto(item.text).includes(filtroNormalizado)) {
            return true;
        }

        // Busca no campo name se existir
        if (item.name && normalizarTexto(item.name).includes(filtroNormalizado)) {
            return true;
        }

        // Busca nas keywords (array de strings)
        if (item.keywords && Array.isArray(item.keywords)) {
            for (let keyword of item.keywords) {
                if (normalizarTexto(keyword).includes(filtroNormalizado)) {
                    return true;
                }
            }
        }

        return false;
    } catch (erro) {
        console.error('Erro ao verificar correspondência do item:', erro);
        return false;
    }
}

/**
 * Filtra dados hierárquicos (categorias + ícones) baseado em keywords
 * @param {Array} dados - Array de categorias com filhos (estrutura hierárquica)
 * @param {string} filtro - Texto do filtro digitado pelo usuário
 * @returns {Array} Dados filtrados mantendo estrutura hierárquica
 */
function filtrarDadosHierarquicos(dados, filtro) {
    try {
        // Se não há filtro, retorna todos os dados
        if (!filtro || filtro.trim() === '') {
            return dados;
        }

        const filtroNormalizado = normalizarTexto(filtro);
        const resultado = [];

        // Percorre cada categoria
        for (let categoria of dados) {
            // Se não é uma categoria válida, pula
            if (!categoria.child || !Array.isArray(categoria.child)) {
                continue;
            }

            // Filtra os filhos (ícones) da categoria
            const filhosFiltrados = categoria.child.filter(filho =>
                itemCorrespondeAoFiltro(filho, filtroNormalizado)
            );

            // Se há filhos que correspondem ao filtro, inclui a categoria
            if (filhosFiltrados.length > 0) {
                resultado.push({
                    ...categoria,
                    child: filhosFiltrados,
                    expanded: true // Expande categoria quando há filtro
                });
            }
        }

        return resultado;
    } catch (erro) {
        Alerta.TratamentoErroComLinha('kendo-dropdowntree-filtro-keywords.js', 'filtrarDadosHierarquicos', erro);
        return dados; // Em caso de erro, retorna dados originais
    }
}

/**
 * Cria um Kendo DropDownTree com filtro customizado por keywords
 * @param {object} config - Configuração do DropDownTree
 * @param {string} config.elementId - ID do elemento HTML (ex: 'ddlIcone')
 * @param {Array} config.dados - Array de dados hierárquicos
 * @param {object} config.template - Template do item (opcional)
 * @param {Function} config.onChange - Callback quando valor muda
 * @param {Function} config.onSelect - Callback quando item é selecionado
 * @param {string} config.placeholder - Texto placeholder
 * @param {number} config.height - Altura do popup (padrão: 400)
 * @param {number} config.filterDelay - Delay do filtro em ms (padrão: 300)
 * @returns {object} Instância do widget Kendo DropDownTree
 */
function criarDropDownTreeComFiltroKeywords(config) {
    try {
        const {
            elementId,
            dados,
            template,
            onChange,
            onSelect,
            placeholder = 'Selecione...',
            height = 400,
            filterDelay = 300
        } = config;

        // Valida parâmetros obrigatórios
        if (!elementId || !dados) {
            throw new Error('elementId e dados são obrigatórios');
        }

        const $element = $('#' + elementId);
        if ($element.length === 0) {
            throw new Error(`Elemento #${elementId} não encontrado`);
        }

        // Destrói widget existente se houver
        const existingWidget = $element.data('kendoDropDownTree');
        if (existingWidget) {
            existingWidget.destroy();
        }

        // Armazena dados originais para filtro
        let dadosOriginais = JSON.parse(JSON.stringify(dados));
        let dadosFiltrados = dadosOriginais;

        // Cria dataSource hierárquico
        const dataSourceConfig = {
            data: dadosFiltrados,
            schema: {
                model: {
                    id: 'id',
                    hasChildren: 'hasChild',
                    children: 'child'
                }
            }
        };

        // Inicializa DropDownTree
        $element.kendoDropDownTree({
            dataSource: dataSourceConfig,
            dataTextField: 'text',
            dataValueField: 'id',
            placeholder: placeholder,
            height: height,
            autoClose: true, // Fecha automaticamente ao selecionar
            filter: 'contains', // Habilita filtro (será customizado)

            // Template customizado se fornecido
            template: template || undefined,

            // Eventos
            change: function(e) {
                if (onChange && typeof onChange === 'function') {
                    onChange(e);
                }
            },

            select: function(e) {
                if (onSelect && typeof onSelect === 'function') {
                    onSelect(e);
                }
            },

            // Evento de filtro customizado
            filtering: function(e) {
                try {
                    // Previne comportamento padrão
                    e.preventDefault();

                    const filtro = e.filter ? e.filter.value : '';

                    // Se timeout anterior existe, cancela
                    if (this._filterTimeout) {
                        clearTimeout(this._filterTimeout);
                    }

                    // Aplica debounce no filtro
                    this._filterTimeout = setTimeout(() => {
                        // Filtra dados usando nossa função customizada
                        dadosFiltrados = filtrarDadosHierarquicos(dadosOriginais, filtro);

                        // Atualiza dataSource
                        const dataSource = this.dataSource;
                        dataSource.data(dadosFiltrados);

                        // Se há filtro, expande todos os itens
                        if (filtro && filtro.trim() !== '') {
                            this.treeview.expand('.k-item');
                        }

                        console.log(`Filtro aplicado: "${filtro}" - ${dadosFiltrados.length} categoria(s) encontrada(s)`);
                    }, filterDelay);
                } catch (erro) {
                    Alerta.TratamentoErroComLinha('kendo-dropdowntree-filtro-keywords.js', 'filtering event', erro);
                }
            }
        });

        const widget = $element.data('kendoDropDownTree');

        // Adiciona método helper para obter dados originais
        widget.getDadosOriginais = function() {
            return dadosOriginais;
        };

        // Adiciona método helper para resetar filtro
        widget.resetarFiltro = function() {
            dadosFiltrados = JSON.parse(JSON.stringify(dadosOriginais));
            this.dataSource.data(dadosFiltrados);
            this.value('');
        };

        console.log(`✅ DropDownTree #${elementId} criado com filtro por keywords`);
        return widget;

    } catch (erro) {
        Alerta.TratamentoErroComLinha('kendo-dropdowntree-filtro-keywords.js', 'criarDropDownTreeComFiltroKeywords', erro);
        return null;
    }
}

/**
 * Busca um item por ID em dados hierárquicos
 * @param {Array} dados - Array de categorias com filhos
 * @param {string} id - ID do item a buscar
 * @returns {object|null} Item encontrado ou null
 */
function buscarItemPorId(dados, id) {
    try {
        for (let categoria of dados) {
            // Verifica se a categoria tem o ID
            if (categoria.id === id) {
                return categoria;
            }

            // Procura nos filhos
            if (categoria.child && Array.isArray(categoria.child)) {
                for (let filho of categoria.child) {
                    if (filho.id === id) {
                        return filho;
                    }
                }
            }
        }
        return null;
    } catch (erro) {
        console.error('Erro ao buscar item por ID:', erro);
        return null;
    }
}
