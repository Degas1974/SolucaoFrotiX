# Documentação: ocorrencias.js

> **Arquivo:** `wwwroot/js/cadastros/ocorrencias.js`
> **Versão:** 1.0
> **Data:** 01/02/2026
> **Módulo:** Gestão de Ocorrências - Front-End
> **Status:** Documentado completo

---

## 🎯 Objetivo Geral

O arquivo `ocorrencias.js` é responsável pela **gerenciamento completo da interface de Ocorrências de Viagem** no sistema FrotiX. Ele implementa:

1. **Grid de Listagem** com DataTables (10/25/50 registros ou todos)
2. **Filtros Avançados** (Data, Data Inicial/Final, Veículo, Motorista, Status)
3. **Modal de Edição** de ocorrências (resumo, descrição, solução)
4. **Upload de Imagens/Vídeos** com preview visual
5. **Gestão de Status** (Aberta, Baixada, Pendente, Manutenção)
6. **Ações em Lote** (Editar, Baixar, Visualizar Imagem)

Este arquivo segue **rigorosamente** o padrão FrotiX com try-catch em todas as funções, uso de Syncfusion para componentes ricos, e integração com o sistema de alertas SweetAlert.

---

## 📥 📤 Entradas e Saídas

### Entradas
- **Cliques em botões:** Filtrar, Editar, Baixar, Ver Imagem
- **Seleção de filtros:** Combos Syncfusion (Veículo, Motorista, Status)
- **Datas:** Data única ou intervalo (Data Inicial/Final)
- **Upload de arquivo:** Imagem ou vídeo até 50MB
- **RTE Syncfusion:** Descrição e Solução da ocorrência

### Saídas
- **Grid DataTable:** Lista de ocorrências com ações
- **Modal de Edição:** Formulário completo da ocorrência
- **Modal de Baixa Rápida:** Solução rápida para ocorrências sem solução
- **Modal de Visualização:** Imagem/vídeo em full-size
- **Toast de Feedback:** Mensagens de sucesso/erro (AppToast)

---

## 🔗 Chamada Por / Chama

### Chamada Por
- Navegação do usuário para `/Ocorrencias` ou similar
- Eventos DOM (cliques em botões da grid)
- `document.ready` (inicialização)
- Eventos de modal (shown/hidden)

### Chama
- **APIs:**
  - `GET /api/OcorrenciaViagem/ListarGestao` - Listar ocorrências com filtros
  - `GET /api/OcorrenciaViagem/ObterOcorrencia?id={id}` - Obter detalhes
  - `POST /api/OcorrenciaViagem/UploadImagem` - Upload de imagem/vídeo
  - `POST /api/OcorrenciaViagem/EditarOcorrencia` - Salvar alterações
  - `POST /api/OcorrenciaViagem/BaixarOcorrencia` - Dar baixa na ocorrência

- **Funções Locais:**
  - `BuildGridOcorrencias(params)` - Construir DataTable
  - `carregarOcorrencia(id)` - Carregar dados para edição
  - `uploadImagemOcorrencia(file)` - Upload de arquivo
  - `executarBaixaOcorrencia(id, solucao, callback)` - Dar baixa

- **Bibliotecas/Plugins:**
  - `jQuery` - Manipulação DOM
  - `DataTables` - Grid interativa
  - `Syncfusion (ej2)` - Combos e RTE
  - `Bootstrap Modal` - Modais
  - `Alerta.js` - Alertas SweetAlert
  - `AppToast` - Notificações toast

---

## 📦 Dependências

| Dependência | Tipo | Localização | Descrição |
|---|---|---|---|
| jQuery | Lib | CDN | Manipulação DOM e eventos |
| DataTables | Plugin | CDN | Grid interativa com exportação |
| Syncfusion EJ2 | UI Framework | CDN | Dropdowns, RTE, Tooltips |
| Bootstrap 5.3 | CSS Framework | CDN | Modals, Grid, Componentes |
| FontAwesome 6 | Icons | CDN | Ícones duotone |
| Alerta.js | Custom | `wwwroot/js/` | Sistema de alertas SweetAlert |
| AppToast | Custom | `wwwroot/js/` | Notificações toast |
| FtxSpin | Custom | `wwwroot/js/` | Loading overlay |

---

## 🗂️ Estrutura do Arquivo

### Seções Principais

```
1. Header Identificação
2. Variáveis Globais (dataTable, imagemOcorrenciaAlterada)
3. Funções de Loading (mostrarLoadingOcorrencias, esconderLoadingOcorrencias)
4. Helpers (abreviarNomeMotorista, _keyIsoFromBR, getComboValue)
5. Construção da Grid (BuildGridOcorrencias)
6. Coleta de Parâmetros (collectParamsFromUI)
7. Validação de Datas (validateDatesBeforeSearch)
8. Upload de Imagem (uploadImagemOcorrencia, exibirPreviewImagem)
9. Modal: Carregar Ocorrência (carregarOcorrencia)
10. Baixa de Ocorrências (executarBaixaOcorrencia, processarBaixaComValidacao)
11. Visualização de Imagem (abrirVisualizacaoImagem)
12. Inicialização (document.ready e event delegation)
13. Localização RTE Syncfusion (pt-BR)
```

---

## 🔑 Variáveis Globais

```javascript
var dataTable = null;                      // Instância do DataTable
var imagemOcorrenciaAlterada = false;      // Flag se imagem foi alterada
var novaImagemOcorrencia = "";             // Path/URL da nova imagem
```

**Justificativa:** Variáveis globais necessárias para persistir o estado da grid e imagem durante a sessão do usuário.

---

## 🛠️ Funções Principais

### 1. Loading Overlay

#### `mostrarLoadingOcorrencias(mensagem)`

```javascript
/***
 * 🎯 OBJETIVO: Exibir overlay de loading com mensagem customizada
 * 📥 ENTRADA: mensagem [string] - Texto a exibir no overlay
 * 📤 SAÍDA: void
 * 🔄 CHAMA: -
 * 🔗 CHAMADA POR: BuildGridOcorrencias
 */
function mostrarLoadingOcorrencias(mensagem) {
    try {
        var overlay = document.getElementById('loadingOverlayOcorrencias');
        if (overlay) {
            var msgEl = overlay.querySelector('.ftx-loading-text');
            if (msgEl && mensagem) msgEl.textContent = mensagem;
            overlay.style.display = 'flex';
        }
    } catch (error) {
        console.warn("Erro ao mostrar loading:", error);
    }
}
```

**Linha-a-linha:**
- Linha 1: Procura elemento com ID `loadingOverlayOcorrencias` no DOM
- Linha 2: Se existe, busca o elemento com classe `ftx-loading-text`
- Linha 3: Atualiza o texto se mensagem foi fornecida
- Linha 4: Exibe o overlay com display flex

---

#### `esconderLoadingOcorrencias()`

```javascript
function esconderLoadingOcorrencias() {
    try {
        var overlay = document.getElementById('loadingOverlayOcorrencias');
        if (overlay) {
            overlay.style.display = 'none';
        }
    } catch (error) {
        console.warn("Erro ao esconder loading:", error);
    }
}
```

**Linha-a-linha:**
- Procura o overlay e o oculta mudando display para `none`
- Try-catch local evita quebras se elemento não existe

---

### 2. Helpers de Formatação

#### `abreviarNomeMotorista(nome)`

```javascript
/***
 * 🎯 OBJETIVO: Abreviar nome do motorista (max 2 palavras, sem conectores)
 * 📥 ENTRADA: nome [string] - Nome completo do motorista
 * 📤 SAÍDA: string - Nome abreviado (ex: "João Silva")
 * 🔄 CHAMA: -
 * 🔗 CHAMADA POR: BuildGridOcorrencias (columnDef targets:2)
 */
function abreviarNomeMotorista(nome) {
    try {
        if (!nome) return "";
        const palavras = String(nome).trim().split(/\s+/);
        const out = [];
        const CONECTORES = new Set([
            "de", "da", "do", "dos", "das", "e", "d", "d'", "del", "della", "di", "du", "van", "von",
        ]);

        for (const w of palavras) {
            const limp = w.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "").replace(/[.:()]/g, "");
            if (CONECTORES.has(limp)) continue;  // Skip conectores
            out.push(w);
            if (out.length === 2) break;          // Max 2 palavras
        }

        return out.join(" ");
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "abreviarNomeMotorista", error);
        return nome || "";
    }
}
```

**Lógica:**
- Remove preposições (de, da, do, etc)
- Normaliza acentos para comparação
- Retorna apenas as 2 primeiras palavras significativas
- **Exemplo:** "João da Silva Santos" → "João Silva"

---

#### `_keyIsoFromBR(value)`

```javascript
/***
 * 🎯 OBJETIVO: Converter data BR (DD/MM/YYYY) para ISO (YYYY-MM-DD) para sorting
 * 📥 ENTRADA: value [string] - Data em formato "DD/MM/YYYY"
 * 📤 SAÍDA: string - Data em ISO format para sort correto
 * 🔄 CHAMA: -
 * 🔗 CHAMADA POR: BuildGridOcorrencias (columnDef targets:1 sort)
 */
function _keyIsoFromBR(value) {
    try {
        if (!value) return "";
        const [dd, mm, yyyy] = value.split("/");
        return `${yyyy}-${mm}-${dd}`;  // ISO format para sort correto
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "_keyIsoFromBR", error);
        return "";
    }
}
```

**Por que existe:** DataTables ordena alfabeticamente por padrão. Sem conversão para ISO, "01/01/2026" ficaria após "31/12/2025".

---

#### `getComboValue(comboId)`

```javascript
/***
 * 🎯 OBJETIVO: Obter valor selecionado de um Syncfusion Combo (ej2)
 * 📥 ENTRADA: comboId [string] - ID do elemento combo
 * 📤 SAÍDA: string - Valor selecionado ou string vazia
 * 🔄 CHAMA: -
 * 🔗 CHAMADA POR: collectParamsFromUI, múltiplas funções
 */
function getComboValue(comboId) {
    try {
        const el = document.getElementById(comboId);
        if (el && el.ej2_instances && el.ej2_instances.length > 0) {
            const inst = el.ej2_instances[0];
            if (inst && inst.value != null && inst.value !== "") return inst.value;
        }
        return "";
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "getComboValue", error);
        return "";
    }
}
```

**Linha-a-linha:**
- Linha 1: Obtém elemento Syncfusion pelo ID
- Linha 2-3: Acessa a instância EJ2 (Syncfusion armazena em `el.ej2_instances[0]`)
- Linha 4: Retorna valor se não for null ou vazio

---

### 3. Construção da Grid (BuildGridOcorrencias)

```javascript
/***
 * 🎯 OBJETIVO: Construir/reconstruir DataTable com ocorrências, com filtros aplicados
 * 📥 ENTRADA: params [object] - { veiculoId, motoristaId, statusId, data, dataInicial, dataFinal }
 * 📤 SAÍDA: void - Popula #tblOcorrencia
 * 🔄 CHAMA: GET /api/OcorrenciaViagem/ListarGestao
 * 🔗 CHAMADA POR: document.ready, btnFiltrar.click
 * 📝 NOTA: Usa columnDef customizado para render de data, motorista e status
 */
function BuildGridOcorrencias(params) {
    try {
        // [UI] Mostrar loading
        mostrarLoadingOcorrencias('Carregando Ocorrências...');

        // [LOGICA] Destroir DataTable anterior se existe
        if ($.fn.DataTable.isDataTable("#tblOcorrencia")) {
            $("#tblOcorrencia").DataTable().destroy();
            $("#tblOcorrencia tbody").empty();
        }

        // [AJAX] Criar nova instância DataTable
        dataTable = $("#tblOcorrencia").DataTable({
            autoWidth: false,
            dom: "Bfrtip",  // Buttons, filter, paginate, info, table
            lengthMenu: [[10, 25, 50, -1], ["10 linhas", "25 linhas", "50 linhas", "Todas"]],
            buttons: [
                "pageLength",                                    // Seletor de linhas por página
                "excel",                                        // Exportar Excel
                { extend: "pdfHtml5", orientation: "landscape", pageSize: "LEGAL" }  // PDF
            ],
            order: [[1, "desc"]],  // Ordenar por data descrescente

            // [UI] Definição de colunas com estilo e renderização
            columnDefs: [
                { targets: 0, className: "text-center", width: "5%" },  // Ficha/ID
                {
                    targets: 1,  // Data
                    className: "text-center",
                    width: "8%",
                    render: function (value, type) {
                        try {
                            if (!value) return "";
                            // [LOGICA] Para sort, converter para ISO; para display, manter DD/MM/YYYY
                            if (type === "sort" || type === "type") {
                                if (/^\d{2}\/\d{2}\/\d{4}$/.test(value)) {
                                    return _keyIsoFromBR(value);  // Retornar ISO para sort correto
                                }
                            }
                            return value;  // Manter formato original para display
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.data", error);
                            return "";
                        }
                    }
                },
                {
                    targets: 2,  // Motorista
                    className: "text-left",
                    width: "12%",
                    render: function (data, type) {
                        try {
                            // [UI] Display: abreviar; Sort: manter original
                            return type === "display" ? abreviarNomeMotorista(data) : data;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.motorista", error);
                            return data;
                        }
                    }
                },
                { targets: 3, className: "text-left", width: "15%" },  // Descrição Veículo
                { targets: 4, className: "text-left", width: "15%" },  // Resumo Ocorrência
                { targets: 5, className: "text-left", width: "15%" },  // Descrição Solução
                { targets: 6, className: "text-center", width: "8%" },  // Status
                { targets: 7, className: "text-center", width: "8%" },  // Ações
                { targets: 8, visible: false }  // Descrição completa (oculta, para modal)
            ],

            responsive: true,

            // [AJAX] Configuração de carregamento de dados
            ajax: {
                url: "/api/OcorrenciaViagem/ListarGestao",
                type: "GET",
                dataType: "json",
                data: params,  // Passar filtros como parâmetros
                error: function (xhr, error, thrown) {
                    try {
                        esconderLoadingOcorrencias();
                        console.error("Erro ao carregar ocorrências:", error, thrown);
                        AppToast.show("Vermelho", "Erro ao carregar ocorrências", 3000);
                    } catch (err) {
                        Alerta.TratamentoErroComLinha("ocorrencias.js", "ajax.error", err);
                    }
                }
            },

            // [DADOS] Mapeamento de colunas
            columns: [
                { data: "noFichaVistoria", defaultContent: "-" },
                { data: "data", defaultContent: "-" },
                { data: "nomeMotorista", defaultContent: "-" },
                { data: "descricaoVeiculo", defaultContent: "-" },
                { data: "resumoOcorrencia", defaultContent: "-" },
                { data: "descricaoSolucaoOcorrencia", defaultContent: "-" },
                {
                    data: "statusOcorrencia",
                    render: function (data, type, row) {
                        try {
                            var s = row.statusOcorrencia || "Aberta";
                            var icon = "";
                            var badgeClass = "ftx-badge-aberta";

                            // [UI] Renderizar badge com ícone e cor
                            switch (s) {
                                case "Aberta":
                                    icon = '<i class="fa-duotone fa-circle-exclamation me-1"></i>';
                                    badgeClass = "ftx-badge-aberta";
                                    break;
                                case "Baixada":
                                    icon = '<i class="fa-duotone fa-circle-check me-1"></i>';
                                    badgeClass = "ftx-badge-baixada";
                                    break;
                                case "Pendente":
                                    icon = '<i class="fa-duotone fa-clock me-1"></i>';
                                    badgeClass = "ftx-badge-pendente";
                                    break;
                                case "Manutenção":
                                    icon = '<i class="fa-duotone fa-wrench me-1"></i>';
                                    badgeClass = "ftx-badge-manutencao";
                                    break;
                            }

                            return `<span class="ftx-badge-status ${badgeClass}">${icon}${s}</span>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.status", error);
                            return "";
                        }
                    }
                },
                {
                    data: "ocorrenciaViagemId",
                    render: function (data, type, row) {
                        try {
                            var baixada = row.statusOcorrencia === "Baixada";
                            var temImagem = row.imagemOcorrencia && row.imagemOcorrencia.trim() !== "";

                            // [UI] Botão Editar (Azul, sempre ativo)
                            var btnEditar = `
                                <a class="btn-azul btn-icon-28 btn-editar-ocorrencia"
                                    data-id="${data}"
                                    data-ejtip="Editar Ocorrência"
                                    style="cursor:pointer;">
                                    <i class="fa-duotone fa-pen-to-square"></i>
                                </a>`;

                            // [UI] Botão Baixar (Vinho, desabilita se já baixada)
                            var btnBaixa = `
                                <a class="btn-vinho btn-icon-28 btn-baixar ${baixada ? 'disabled' : ''}"
                                    data-id="${data}"
                                    data-ejtip="${baixada ? 'Já baixada' : 'Dar Baixa'}"
                                    style="cursor:pointer;"
                                    ${baixada ? 'disabled' : ''}>
                                    <i class="fa-duotone fa-flag-checkered"></i>
                                </a>`;

                            // [UI] Botão Ver Imagem (Terracota, desabilita se sem imagem)
                            var btnImagem = `
                                <a class="btn-terracota btn-icon-28 btn-ver-imagem ${temImagem ? '' : 'disabled'}"
                                    data-imagem="${row.imagemOcorrencia || ''}"
                                    data-ejtip="${temImagem ? 'Ver Imagem/Vídeo' : 'Sem imagem'}"
                                    style="cursor:pointer;"
                                    ${temImagem ? '' : 'disabled'}>
                                    <i class="fa-duotone fa-image"></i>
                                </a>`;

                            // [UI] Retornar botões lado-a-lado
                            return `<div class="text-center" style="display:flex; justify-content:center; gap:4px;">
                                ${btnEditar}
                                ${btnBaixa}
                                ${btnImagem}
                            </div>`;
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("ocorrencias.js", "grid.render.acoes", error);
                            return "";
                        }
                    }
                },
                { data: "descricaoOcorrencia", defaultContent: "" }  // Oculta (targets:8)
            ],

            language: {
                url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/pt-BR.json"
            },

            // [UI] Callback após grid desenhada
            drawCallback: function () {
                try {
                    console.log("[ocorrencias.js] Grid carregada com", this.api().rows().count(), "registros");
                    esconderLoadingOcorrencias();
                } catch (error) {
                    Alerta.TratamentoErroComLinha("ocorrencias.js", "drawCallback", error);
                }
            }
        });
    } catch (error) {
        esconderLoadingOcorrencias();
        Alerta.TratamentoErroComLinha("ocorrencias.js", "BuildGridOcorrencias", error);
    }
}
```

**Resumo da lógica:**
1. Mostra loading overlay
2. Destrói DataTable anterior (se existe)
3. Cria nova instância com configuração completa
4. Define ordem padrão (Data desc)
5. Mapeia colunas com render customizado
6. Configura AJAX para `ListarGestao` passando filtros
7. Renderiza status com badge e ícones
8. Renderiza ações (Editar, Baixar, Ver Imagem) com estado (disabled/enabled)
9. Callback `drawCallback` esconde loading quando grid termina

---

### 4. Coleta de Parâmetros (collectParamsFromUI)

```javascript
/***
 * 🎯 OBJETIVO: Coletar valores de filtros da UI e montar objeto de parâmetros
 * 📥 ENTRADA: -
 * 📤 SAÍDA: object { veiculoId, motoristaId, statusId, data, dataInicial, dataFinal }
 * 🔄 CHAMA: getComboValue() x3
 * 🔗 CHAMADA POR: btnFiltrar.click
 */
function collectParamsFromUI() {
    try {
        const data = ($("#txtData").val() || "").trim();
        const dataInicial = ($("#txtDataInicial").val() || "").trim();
        const dataFinal = ($("#txtDataFinal").val() || "").trim();
        const temPeriodo = dataInicial && dataFinal;

        const veiculoId = getComboValue("lstVeiculos");
        const motoristaId = getComboValue("lstMotorista");

        let statusId = getComboValue("lstStatus");
        if (!statusId) {
            // [LOGICA] Se nenhum status selecionado:
            // - Se houver outros filtros, buscar "Todas"
            // - Senão, padrão é "Aberta"
            statusId = (veiculoId || motoristaId || data || temPeriodo) ? "Todas" : "Aberta";
        }

        return {
            veiculoId: veiculoId,
            motoristaId: motoristaId,
            statusId: statusId,
            data: temPeriodo ? "" : data,              // Exclusivo se não usar período
            dataInicial: temPeriodo ? dataInicial : "",  // Exclusivo se usar período
            dataFinal: temPeriodo ? dataFinal : ""       // Exclusivo se usar período
        };
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "collectParamsFromUI", error);
        return { statusId: "Aberta" };  // Fallback seguro
    }
}
```

**Lógica:**
- Se Data Inicial/Final preenchidas, usar período (data fica vazia)
- Se apenas Data preenchida, usar data única
- Status automático: "Todas" se houver outros filtros, "Aberta" se busca vazia

---

### 5. Validação de Datas (validateDatesBeforeSearch)

```javascript
/***
 * 🎯 OBJETIVO: Validar que Data Inicial e Final estão ambas preenchidas ou ambas vazias
 * 📥 ENTRADA: -
 * 📤 SAÍDA: boolean - true se válido, false se erro
 * 🔄 CHAMA: Alerta.Erro()
 * 🔗 CHAMADA POR: btnFiltrar.click
 */
function validateDatesBeforeSearch() {
    try {
        const dataInicial = ($("#txtDataInicial").val() || "").trim();
        const dataFinal = ($("#txtDataFinal").val() || "").trim();

        // [VALIDACAO] Uma preenchida e outra não = erro
        if ((dataInicial && !dataFinal) || (!dataInicial && dataFinal)) {
            Alerta.Erro("Informação Ausente", "Para filtrar por período, preencha Data Inicial e Data Final.", "OK");
            return false;
        }

        return true;
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "validateDatesBeforeSearch", error);
        return false;
    }
}
```

---

### 6. Upload de Imagem (uploadImagemOcorrencia)

```javascript
/***
 * 🎯 OBJETIVO: Fazer upload de imagem/vídeo para servidor e atualizar preview
 * 📥 ENTRADA: file [File] - Objeto File do input
 * 📤 SAÍDA: void - Atualiza novaImagemOcorrencia e exibe preview
 * 🔄 CHAMA: POST /api/OcorrenciaViagem/UploadImagem, exibirPreviewImagem()
 * 🔗 CHAMADA POR: inputImagemOcorrencia.change
 * 📝 NOTA: Usa FormData para envio de arquivo
 */
async function uploadImagemOcorrencia(file) {
    try {
        // [AJAX] Preparar FormData e fazer upload
        const formData = new FormData();
        formData.append("file", file);

        const response = await fetch("/api/OcorrenciaViagem/UploadImagem", {
            method: "POST",
            body: formData
        });

        const data = await response.json();

        if (data.success) {
            // [DADOS] Atualizar estado global
            imagemOcorrenciaAlterada = true;
            novaImagemOcorrencia = data.path || data.url || "";

            // [UI] Exibir preview
            exibirPreviewImagem(novaImagemOcorrencia);
            AppToast.show("Verde", "Imagem enviada com sucesso!", 2000);
        } else {
            AppToast.show("Vermelho", data.message || "Erro ao enviar imagem.", 3000);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "uploadImagemOcorrencia", error);
        AppToast.show("Vermelho", "Erro ao enviar imagem.", 3000);
    }
}
```

---

### 7. Preview de Imagem (exibirPreviewImagem)

```javascript
/***
 * 🎯 OBJETIVO: Exibir preview de imagem ou vídeo no modal, com opção de remover
 * 📥 ENTRADA: src [string] - Path/URL da imagem/vídeo ou vazia
 * 📤 SAÍDA: void - Popula #divImagemOcorrencia
 * 🔄 CHAMA: -
 * 🔗 CHAMADA POR: uploadImagemOcorrencia(), carregarOcorrencia(), limparModal()
 * 📝 NOTA: Detecta vídeo vs imagem pela extensão
 */
function exibirPreviewImagem(src) {
    try {
        const container = $("#divImagemOcorrencia");
        container.empty();

        if (!src) {
            // [UI] Modo "adicionar": ícone + texto
            container.html(`
                <div class="p-3 text-center border rounded bg-light" style="cursor:pointer;" onclick="$('#inputImagemOcorrencia').click();">
                    <i class="fa-duotone fa-image fa-3x text-muted mb-2"></i>
                    <p class="text-muted mb-0">Clique para adicionar imagem ou vídeo</p>
                </div>
            `);
            return;
        }

        // [LOGICA] Detectar se é vídeo
        const isVideo = /\.(mp4|webm)$/i.test(src);

        if (isVideo) {
            // [UI] Renderizar vídeo com controles e botão de remover
            container.html(`
                <div class="position-relative">
                    <video src="${src}" controls style="max-width:100%; max-height:200px; border-radius:8px;"></video>
                    <button type="button" class="btn btn-sm btn-danger position-absolute top-0 end-0 m-1" onclick="removerImagemOcorrencia()">
                        <i class="fa-duotone fa-trash"></i>
                    </button>
                </div>
            `);
        } else {
            // [UI] Renderizar imagem com botão de remover
            container.html(`
                <div class="position-relative">
                    <img src="${src}" alt="Preview" style="max-width:100%; max-height:200px; border-radius:8px; cursor:pointer;" onclick="$('#inputImagemOcorrencia').click();" />
                    <button type="button" class="btn btn-sm btn-danger position-absolute top-0 end-0 m-1" onclick="removerImagemOcorrencia()">
                        <i class="fa-duotone fa-trash"></i>
                    </button>
                </div>
            `);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "exibirPreviewImagem", error);
    }
}
```

---

### 8. Remover Imagem (removerImagemOcorrencia)

```javascript
/***
 * 🎯 OBJETIVO: Remover imagem/vídeo selecionado e voltar para estado vazio
 * 📥 ENTRADA: -
 * 📤 SAÍDA: void - Reseta estado e preview
 * 🔄 CHAMA: exibirPreviewImagem()
 * 🔗 CHAMADA POR: btn-danger no preview
 */
function removerImagemOcorrencia() {
    try {
        imagemOcorrenciaAlterada = true;
        novaImagemOcorrencia = "";
        exibirPreviewImagem("");
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "removerImagemOcorrencia", error);
    }
}
```

---

### 9. Carregar Ocorrência (carregarOcorrencia)

```javascript
/***
 * 🎯 OBJETIVO: Buscar dados da ocorrência do servidor e popular modal de edição
 * 📥 ENTRADA: id [string/number] - ID da ocorrência (OcorrenciaViagemId)
 * 📤 SAÍDA: void - Popula campos modal e exibe ele
 * 🔄 CHAMA: GET /api/OcorrenciaViagem/ObterOcorrencia, exibirPreviewImagem()
 * 🔗 CHAMADA POR: btn-editar-ocorrencia.click
 */
async function carregarOcorrencia(id) {
    try {
        if (!id) {
            console.warn("ID inválido para carregar ocorrência");
            return;
        }

        // [AJAX] Buscar dados do servidor
        const response = await fetch(`/api/OcorrenciaViagem/ObterOcorrencia?id=${id}`);

        if (!response.ok) {
            throw new Error(`Erro HTTP: ${response.status}`);
        }

        const text = await response.text();
        let data;
        try {
            data = JSON.parse(text);
        } catch (e) {
            console.error("Erro ao parsear resposta servida:", text);
            throw new Error("Resposta inválida do servidor (não é JSON).");
        }

        if (data.success && data.ocorrencia) {
            const oc = data.ocorrencia;

            // [DADOS] Popular campos de texto
            $("#txtId").val(oc.ocorrenciaViagemId || "");
            $("#txtResumo").val(oc.resumoOcorrencia || "");
            $("#txtImagemOcorrenciaAtual").val(oc.imagemOcorrencia || "");
            $("#chkStatusOcorrencia").val(oc.statusOcorrencia || "Aberta");

            // [DADOS] Popular RTEs (Rich Text Editors)
            const rteDesc = document.getElementById("rteOcorrencias")?.ej2_instances?.[0];
            const rteSol = document.getElementById("rteSolucao")?.ej2_instances?.[0];

            if (rteDesc) rteDesc.value = oc.descricaoOcorrencia || "";
            if (rteSol) rteSol.value = oc.solucaoOcorrencia || "";

            // [UI] Exibir preview de imagem
            exibirPreviewImagem(oc.imagemOcorrencia || "");

            // [UI] Atualizar título do modal
            const titulo = oc.statusOcorrencia === "Baixada" ? "Visualizar Ocorrência" : "Editar Ocorrência";
            $("#modalOcorrenciaLabel span").text(titulo);

            // [UI] Habilitar/desabilitar botões conforme status
            const baixada = oc.statusOcorrencia === "Baixada";
            $("#btnBaixarOcorrenciaModal").prop("disabled", baixada);
            $("#btnEditarOcorrencia").prop("disabled", baixada);

            // [UI] Exibir modal
            new bootstrap.Modal(document.getElementById("modalOcorrencia")).show();
        } else {
            AppToast.show("Vermelho", data.message || "Erro ao carregar ocorrência.", 3000);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "carregarOcorrencia", error);
        AppToast.show("Vermelho", "Erro ao carregar ocorrência.", 3000);
    }
}
```

---

### 10. Executar Baixa (executarBaixaOcorrencia)

```javascript
/***
 * 🎯 OBJETIVO: Enviar requisição de baixa da ocorrência para o servidor
 * 📥 ENTRADA: id [string] - ID ocorrência
 *             solucao [string] - Texto da solução (pode vir do RTE)
 *             callbackSucesso [function] - Callback após sucesso
 * 📤 SAÍDA: void - Recarrega grid se sucesso
 * 🔄 CHAMA: POST /api/OcorrenciaViagem/BaixarOcorrencia
 * 🔗 CHAMADA POR: processarBaixaComValidacao(), btnBaixarModal.click
 */
async function executarBaixaOcorrencia(id, solucao, callbackSucesso) {
    try {
        // [DADOS] Preparar payload
        const payload = {
            OcorrenciaViagemId: id,
            SolucaoOcorrencia: solucao,
            StatusOcorrencia: "Baixada"
        };

        // [AJAX] Fazer requisição
        const response = await fetch("/api/OcorrenciaViagem/BaixarOcorrencia", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        const data = await response.json();

        if (data.success) {
            AppToast.show("Verde", data.message || "Ocorrência baixada com sucesso!", 2000);
            if (callbackSucesso) callbackSucesso();
            if (dataTable) dataTable.ajax.reload(null, false);  // Recarregar grid sem manter página
        } else {
            AppToast.show("Vermelho", data.message || "Erro ao baixar ocorrência.", 3000);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "executarBaixaOcorrencia", error);
        AppToast.show("Vermelho", "Erro ao baixar ocorrência.", 3000);
    }
}
```

---

### 11. Baixa com Validação (processarBaixaComValidacao)

```javascript
/***
 * 🎯 OBJETIVO: Validar se solução está preenchida; se sim, baixar direto; se não, abrir modal rápido
 * 📥 ENTRADA: id [string] - ID ocorrência
 *             solucaoAtual [string] - Solução já preenchida ou vazia
 *             callbackSucesso [function] - Callback após sucesso
 * 📤 SAÍDA: void
 * 🔄 CHAMA: verificarSolucaoPreenchida(), executarBaixaOcorrencia()
 * 🔗 CHAMADA POR: btn-baixar.click, btnBaixarModal.click
 */
async function processarBaixaComValidacao(id, solucaoAtual, callbackSucesso) {
    try {
        if (verificarSolucaoPreenchida(solucaoAtual)) {
            // [LOGICA] Solução já preenchida = baixar direto
            await executarBaixaOcorrencia(id, solucaoAtual, callbackSucesso);
        } else {
            // [LOGICA] Solução vazia = pedir ao usuário
            if (callbackSucesso) callbackSucesso();  // Fechar modal anterior se houver
            $("#txtBaixaRapidaId").val(id);
            new bootstrap.Modal(document.getElementById("modalBaixaRapida")).show();
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "processarBaixaComValidacao", error);
    }
}
```

---

### 12. Visualização de Imagem (abrirVisualizacaoImagem)

```javascript
/***
 * 🎯 OBJETIVO: Abrir modal full-size para visualizar imagem/vídeo
 * 📥 ENTRADA: src [string] - Path/URL da imagem/vídeo
 * 📤 SAÍDA: void - Exibe modal
 * 🔄 CHAMA: -
 * 🔗 CHAMADA POR: btn-ver-imagem.click
 */
function abrirVisualizacaoImagem(src) {
    try {
        const container = $("#divImagemVisualizacao");
        container.empty();

        if (!src) {
            container.html('<p class="text-muted">Sem imagem disponível</p>');
            return;
        }

        // [LOGICA] Detectar se é vídeo
        const isVideo = /\.(mp4|webm)$/i.test(src);

        if (isVideo) {
            // [UI] Renderizar vídeo full-size
            container.html(`<video src="${src}" controls style="max-width:100%; max-height:500px;"></video>`);
            $("#modalVisualizarImagem .modal-title").html('<i class="fa-duotone fa-video me-2"></i>Vídeo da Ocorrência');
        } else {
            // [UI] Renderizar imagem full-size
            container.html(`<img src="${src}" alt="Imagem" style="max-width:100%; max-height:500px;" />`);
            $("#modalVisualizarImagem .modal-title").html('<i class="fa-duotone fa-image me-2"></i>Imagem da Ocorrência');
        }

        // [UI] Exibir modal
        new bootstrap.Modal(document.getElementById("modalVisualizarImagem")).show();
    } catch (error) {
        Alerta.TratamentoErroComLinha("ocorrencias.js", "abrirVisualizacaoImagem", error);
    }
}
```

---

## 🚀 Inicialização (document.ready)

O arquivo inicia com `$(document).ready()` que:

1. **Carrega grid inicial** com status "Aberta"
2. **Registra event handlers:**
   - Botão Filtrar
   - Cliques em linhas da grid (delegação)
   - Input de imagem
   - Modais (show/hide)
   - Botão Salvar
   - Botão Confirmar Baixa Rápida

Todos os handlers usam **try-catch** e **Alerta.TratamentoErroComLinha()** para rastreamento de erros.

---

## 🌐 Localização Syncfusion (pt-BR)

Ao final do arquivo, há configuração de idioma português para o RTE (Rich Text Editor) Syncfusion, mapeando botões e menus para "Negrito", "Itálico", etc.

---

## 📋 Fluxos Principais

### Fluxo 1: Filtrar Ocorrências

```
btnFiltrar.click
    ↓
validateDatesBeforeSearch()
    ↓
collectParamsFromUI()
    ↓
BuildGridOcorrencias(params)
    ↓
mostrarLoadingOcorrencias()
    ↓
GET /api/OcorrenciaViagem/ListarGestao
    ↓
drawCallback → esconderLoadingOcorrencias()
```

---

### Fluxo 2: Editar Ocorrência

```
btn-editar-ocorrencia.click
    ↓
carregarOcorrencia(id)
    ↓
GET /api/OcorrenciaViagem/ObterOcorrencia
    ↓
Popula modal (campos + RTEs + preview imagem)
    ↓
showModal()
    ↓
[Usuário edita e clica "Salvar"]
    ↓
btnEditarOcorrencia.click
    ↓
Validar resumo obrigatório
    ↓
POST /api/OcorrenciaViagem/EditarOcorrencia
    ↓
Recarregar grid + Fechar modal
```

---

### Fluxo 3: Dar Baixa em Ocorrência

#### 3a) Com solução já preenchida:
```
btn-baixar.click (grid) OU btnBaixarModal.click
    ↓
processarBaixaComValidacao(id, solucao, callback)
    ↓
verificarSolucaoPreenchida() → true
    ↓
executarBaixaOcorrencia()
    ↓
POST /api/OcorrenciaViagem/BaixarOcorrencia
    ↓
Recarregar grid + Fechar modal (se veio de modal)
```

#### 3b) Sem solução (baixa rápida):
```
btn-baixar.click (grid) → solucao vazia
    ↓
processarBaixaComValidacao(id, "", callback)
    ↓
verificarSolucaoPreenchida() → false
    ↓
Abrir modalBaixaRapida
    ↓
[Usuário digita solução e clica "Confirmar"]
    ↓
btnConfirmarBaixaRapida.click
    ↓
executarBaixaOcorrencia(id, solucao, callback)
    ↓
POST /api/OcorrenciaViagem/BaixarOcorrencia
    ↓
Fechar modals + Recarregar grid
```

---

### Fluxo 4: Upload de Imagem

```
inputImagemOcorrencia.change
    ↓
Validar tipo (jpg, png, gif, webp, mp4, webm)
    ↓
Validar tamanho (max 50MB)
    ↓
uploadImagemOcorrencia(file)
    ↓
FormData + POST /api/OcorrenciaViagem/UploadImagem
    ↓
imagemOcorrenciaAlterada = true
    ↓
novaImagemOcorrencia = retorno do servidor
    ↓
exibirPreviewImagem(caminho)
    ↓
[Ao salvar modal, imagem é incluída no payload]
```

---

## 🧪 Validações

| Validação | Função | Comportamento |
|---|---|---|
| Datas período | `validateDatesBeforeSearch()` | Ambas ou nenhuma, senão erro |
| Resumo obrigatório | `btnEditarOcorrencia.click` | Toast erro se vazio |
| Tipo arquivo | `inputImagemOcorrencia.change` | Apenas imagens/vídeos, toast erro se inválido |
| Tamanho arquivo | `inputImagemOcorrencia.change` | Max 50MB, toast erro se maior |
| Solução para baixa | `processarBaixaComValidacao()` | Validação soft (abre modal se vazia) |

---

## 🔄 Integração com APIs

| Endpoint | Método | Parâmetros | Retorno | Quando |
|---|---|---|---|---|
| `/api/OcorrenciaViagem/ListarGestao` | GET | veiculoId, motoristaId, statusId, data, dataInicial, dataFinal | `{ success, data: [...], message }` | Filtrar |
| `/api/OcorrenciaViagem/ObterOcorrencia` | GET | id | `{ success, ocorrencia: {...}, message }` | Abrir modal |
| `/api/OcorrenciaViagem/UploadImagem` | POST | FormData(file) | `{ success, path/url, message }` | Upload |
| `/api/OcorrenciaViagem/EditarOcorrencia` | POST | JSON payload | `{ success, message }` | Salvar |
| `/api/OcorrenciaViagem/BaixarOcorrencia` | POST | JSON payload | `{ success, message }` | Dar baixa |

---

## 🎨 Padrões Visuais Utilizados

### Botões
- **Editar:** `btn-azul` (azul #325d88)
- **Baixar:** `btn-vinho` (vinho #722f37, com state `disabled`)
- **Ver Imagem:** `btn-terracota`

### Badges de Status
- **Aberta:** `ftx-badge-aberta` com ícone `fa-circle-exclamation`
- **Baixada:** `ftx-badge-baixada` com ícone `fa-circle-check`
- **Pendente:** `ftx-badge-pendente` com ícone `fa-clock`
- **Manutenção:** `ftx-badge-manutencao` com ícone `fa-wrench`

### Componentes
- **RTE:** Syncfusion `EJ2 RichTextEditor` (descrição e solução)
- **Combo:** Syncfusion `EJ2 DropDownList` (veículo, motorista, status)
- **Grid:** DataTables com Export Excel/PDF
- **Tooltips:** Syncfusion `data-ejtip` (nunca Bootstrap)
- **Toast:** `AppToast.show()` para notificações rápidas

---

## 📝 Histórico de Modificações

| Versão | Data | Alterações |
|---|---|---|
| 1.0 | 01/02/2026 | Documentação inicial completa, todas as funções documentadas |

---

## ⚠️ Observações Importantes

1. **Variáveis Globais:** `dataTable`, `imagemOcorrenciaAlterada`, `novaImagemOcorrencia` persistem para toda sessão do usuário.

2. **Try-Catch:** Todas as 30+ funções têm try-catch com `Alerta.TratamentoErroComLinha()` para rastreamento de erros no servidor.

3. **Sincfusion ej2_instances:** Combos e RTEs armazenam instância em `el.ej2_instances[0]`. Acessar sempre com `?.` para segurança.

4. **Upload de Arquivo:** O servidor retorna path ou URL da imagem. Ambas são aceitas e armazenadas em `novaImagemOcorrencia`.

5. **DataTable.ajax.reload():** Sem parâmetro de página, mantém-se na página atual. Use `dataTable.ajax.reload(null, false)`.

6. **Modal Cleanup:** `limparModal()` é chamada ao fechar qualquer modal para resetar estado e RTEs.

7. **RTE Refresh:** Ao abrir modal, é chamado `refreshUI()` nos RTEs para garantir renderização correta.

8. **Delegação de Eventos:** Botões da grid (editar, baixar, imagem) usam delegação (`.on()` no `document`) para funcionar com dinamicamente renderizado linhas.

---

## 🔍 Dependências Externas Críticas

```javascript
// jQuery
$, $.fn.DataTable

// Syncfusion EJ2
ej.base.L10n, document.getElementById().ej2_instances

// Bootstrap 5.3
bootstrap.Modal

// Custom
Alerta.*, AppToast.show()
```

Se alguma dependência faltar, o arquivo falhará silenciosamente (try-catch local protege, mas funcionalidade será quebrada).

---

**Fim da Documentação**
