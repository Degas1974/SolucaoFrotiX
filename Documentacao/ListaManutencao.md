# 📚 ANÁLISE COMPLETA - ListaManutencao.js

## Arquivo Analisado: wwwroot/js/cadastros/ListaManutencao.js

**Localização:** `/FrotiX.Site/wwwroot/js/cadastros/ListaManutencao.js`
**Tipo:** JavaScript (Client-Side Script)
**Padrão:** FrotiX UI Interaction Module
**Status:** ✅ Processado em 02/02/2026

---

## 1️⃣ VISÃO GERAL

### Propósito Principal
Gerenciar a interface da lista de Ordens de Serviço (OS) de Manutenção, incluindo:
- Carregamento e filtros de dados
- Exibição em DataTable com status e ações
- Modal para baixa de OS
- Gerenciamento de itens de manutenção
- Visualização de fotos de ocorrências
- Remoção de itens com marca como pendente

### Endpoints Consumidos
| Método | Endpoint | Propósito |
|--------|----------|-----------|
| GET | `/api/Manutencao/` | Listar OSs com filtros |
| GET | `/api/Manutencao/ItensOS` | Listar itens de uma OS |
| POST | `/api/Manutencao/BaixaOS` | Baixar/Fechar OS |
| POST | `/api/Manutencao/CancelaManutencao` | Cancelar OS |

---

## 2️⃣ ESTRUTURA GERAL

```javascript
// Blocos principais:
// 1. Variáveis globais (URLapi, IDapi)
// 2. Funções de loading overlay
// 3. Inicialização no DOMContentLoaded
// 4. Carregamento inicial da tabela
// 5. Filtros (Mês/Ano + Veículo + Status)
// 6. Recriação/recarregamento da tabela
// 7. Normalização de booleanos
// 8. Listagem com filtros avançados
// 9. Modal de itens e DataTable
// 10. Visualização de fotos
// 11. Toggle de reserva
// 12. Remoção de itens para pendente
// 13. Baixa de OS
// 14. Cancelamento de OS
// 15. Abertura de modal para baixa
```

---

## 3️⃣ VARIÁVEIS GLOBAIS

```javascript
var URLapi = "";           // Endpoint para carregamento da tabela
var IDapi = "";            // ID da entidade (veículo ou similar)

// Flags de filtro (não visíveis no código atual, mas referenciados)
var escolhendoVeiculo = false;
var escolhendoData = false;
var escolhendoStatus = false;

// Gerenciamento de itens removidos para pendente
var itensRemovidosParaPendente = [];

// DataTable de itens
var dataTableItens;

// Linha selecionada na tabela de itens
var LinhaManutencaoSelecionada = 0;
```

---

## 4️⃣ FUNÇÕES PRINCIPAIS

### 4.1 Controle de Loading Overlay

#### `mostrarLoadingManutencao()`
```javascript
function mostrarLoadingManutencao() {
    try {
        var overlay = document.getElementById('loadingOverlayManutencao');
        if (overlay) {
            overlay.style.display = 'flex';
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "mostrarLoadingManutencao", error);
    }
}
```

**Análise:**
- ✅ Try-catch obrigatório
- ✅ Uso de Alerta.TratamentoErroComLinha
- Exibe overlay de loading ao iniciar requisição
- Padrão FrotiX para UX responsiva

#### `esconderLoadingManutencao()`
```javascript
function esconderLoadingManutencao() {
    try {
        var overlay = document.getElementById('loadingOverlayManutencao');
        if (overlay) {
            overlay.style.display = 'none';
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "esconderLoadingManutencao", error);
    }
}
```

**Análise:**
- ✅ Espelho de mostrarLoadingManutencao
- ✅ Tratamento de erro consistente
- Usado no callback initComplete do DataTable

---

### 4.2 Inicialização no DOMContentLoaded

```javascript
document.addEventListener('DOMContentLoaded', function () {
    try {
        // Remove bootstrap tooltips para evitar conflito visual
        var els = document.querySelectorAll('[data-bs-toggle="tooltip"]');
        els.forEach(function (el) {
            try {
                var inst = window.bootstrap?.Tooltip?.getInstance?.(el);
                inst?.dispose?.();
            } catch (_) { }
            el.removeAttribute('data-bs-toggle');
            if (!el.hasAttribute('data-ejtip') && el.getAttribute('title')) {
                el.setAttribute('data-ejtip', el.getAttribute('title'));
                el.removeAttribute('title');
            }
        });

        // Carrega automaticamente as OSs Abertas ao iniciar
        carregaManutencaoInicial();

    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "DOMContentLoaded", error);
    }
});
```

**Análise Detalhada:**

| Linha | Código | Propósito |
|-------|--------|-----------|
| 1-11 | Remover Bootstrap tooltips | Migra de Bootstrap para Syncfusion (data-ejtip) |
| 12 | `carregaManutencaoInicial()` | Carrega tabela de OSs ao iniciar página |

**Padrão Identificado:**
- ✅ Migração de Bootstrap para Syncfusion
- ✅ Optional chaining (?.) para segurança
- ✅ Try-catch com captura vazia (_) para fallback
- Garante compatibilidade visual (tooltips com padrão FrotiX)

---

### 4.3 Carregamento Inicial: `carregaManutencaoInicial()`

**Localização:** Linhas 77-252

```javascript
function carregaManutencaoInicial() {
    try {
        mostrarLoadingManutencao();

        if ($.fn.dataTable && $.fn.dataTable.moment) {
            $.fn.dataTable.moment("DD/MM/YYYY");
        }

        $("#tblManutencao").DataTable({
            autoWidth: false,
            dom: "Bfrtip",
            lengthMenu: [[10, 25, 50, -1], ["10 linhas", "25 linhas", "50 linhas", "Todas as Linhas"]],
            buttons: ["pageLength", "excel", { extend: "pdfHtml5", orientation: "landscape", pageSize: "LEGAL" }],
            order: [[2, "desc"]],
            columnDefs: [/* ... 11 colunas ... */],
            responsive: true,
            ajax: {
                url: "/api/Manutencao/",
                type: "GET",
                dataType: "json",
                data: {
                    veiculoId: "",
                    statusId: "Aberta",  // Default: OSs abertas
                    mes: "",
                    ano: "",
                    dataInicial: "",
                    dataFinal: ""
                },
                error: function (xhr, error, thrown) {
                    esconderLoadingManutencao();
                    Alerta.TratamentoErroComLinha("ListaManutencao.js", "ajax.error@carregaManutencaoInicial", thrown);
                }
            },
            initComplete: function () {
                esconderLoadingManutencao();
            },
            columns: [
                { data: "numOS" },
                { data: "descricaoVeiculo" },
                { data: "dataSolicitacao" },
                { data: "dataEntrega" },
                { data: "dataRecolhimento" },
                { data: "dataDevolucao" },
                { data: "dias" },
                { data: "reserva" },
                { data: "resumoOS" },
                {
                    data: "statusOS",
                    render: function (d) {
                        try {
                            const v = (d || "").trim();
                            if (v === "Aberta")
                                return '<span class="ftx-manut-badge ftx-manut-badge-aberta"><i class="fa-solid fa-circle-check"></i> Aberta</span>';
                            if (v === "Cancelada")
                                return '<span class="ftx-manut-badge ftx-manut-badge-cancelada"><i class="fa-solid fa-xmark"></i> Cancelada</span>';
                            return '<span class="ftx-manut-badge ftx-manut-badge-fechada"><i class="fa-solid fa-lock"></i> Fechada/Baixada</span>';
                        } catch (error) {
                            Alerta.TratamentoErroComLinha("ListaManutencao.js", "render@statusOS@init", error);
                            return "";
                        }
                    }
                },
                {
                    data: "manutencaoId",
                    render: function (data, type, full) {
                        // Renderizar botões de ação
                    }
                }
            ],
            language: { /* Português BR */ },
            width: "100%"
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "carregaManutencaoInicial", error);
    }
}
```

**Análise Completa:**

| Aspecto | Detalhes |
|---------|----------|
| **Padrão DOM** | `Bfrtip` = Buttons, filtering, rows, table info, pagination |
| **Padrão de Paginação** | 10, 25, 50, Todas |
| **Botões Exportação** | pageLength, Excel, PDF (landscape, LEGAL) |
| **Ordenação Inicial** | Coluna 2 (dataSolicitacao) descendente |
| **Responsive** | ✅ SIM |
| **Default Status** | "Aberta" (apenas OSs abertas) |
| **Localização** | Português BR |
| **Loading Indicator** | ✅ mostrarLoadingManutencao / esconderLoadingManutencao |

**Coluna 10 - Renderização de Status:**
- "Aberta" → Badge verde com ícone check
- "Cancelada" → Badge vermelha com ícone X
- Outras → Badge cinza com ícone lock

**Coluna 11 - Renderização de Ações:**
- ✏️ Editar/Visualizar OS
- ⬇️ Baixar OS
- 🚫 Cancelar OS

---

### 4.4 Filtros Avançados: `$("#btnDatas").click()`

**Localização:** Linhas 257-497

**Lógica de Filtros:**

```javascript
$("#btnDatas").click(function () {
    try {
        // 1. Capturar valores de filtro
        let Mes = $("#lstMes").val();
        let Ano = $("#lstAno").val();
        let dataInicial = ($("#txtDataInicial").val() || "").trim();
        let dataFinal = ($("#txtDataFinal").val() || "").trim();

        // 2. Validar período (ambos ou nenhum)
        const temIni = dataInicial.length > 0;
        const temFim = dataFinal.length > 0;

        if ((temIni && !temFim) || (!temIni && temFim)) {
            Alerta.Erro("Informação Ausente",
                "Para filtrar por período, preencha Data Inicial e Data Final.", "Ok");
            return;
        }

        // 3. Se período preenchido, ignorar Mês/Ano
        if (temIni && temFim) {
            Mes = "";
            Ano = "";
        } else {
            // 4. Se Mês/Ano, validar ambos
            if ((Mes && !Ano) || (!Mes && Ano)) {
                Alerta.Erro("Informação Ausente",
                    "Informe Mês e Ano (ou use Período com as duas datas).", "Ok");
                return;
            }
        }

        // 5. Capturar veículo (Syncfusion ComboBox)
        let veiculoId = "";
        const veiculosCombo = document.getElementById("lstVeiculos");
        if (veiculosCombo?.ej2_instances?.length > 0) {
            const combo = veiculosCombo.ej2_instances[0];
            if (combo.value) veiculoId = combo.value;
        }

        // 6. Capturar status (lógica especial)
        let statusId = "Aberta";
        const statusCombo = document.getElementById("lstStatus");
        if (statusCombo?.ej2_instances?.length > 0) {
            const st = statusCombo.ej2_instances[0];
            if (st.value === "" || st.value === null) {
                // Se vazio e há filtros, mostrar "Todas"
                if (veiculoId || (Mes && Ano) || (temIni && temFim))
                    statusId = "Todas";
            } else
                statusId = st.value;
        }

        // 7. Destruir DataTable anterior e recriar com novos parâmetros
        mostrarLoadingManutencao();
        var dt = $("#tblManutencao").DataTable();
        dt.destroy();
        $("#tblManutencao tbody").empty();

        // 8. Recriar DataTable com parâmetros de filtro
        $("#tblManutencao").DataTable({
            // ... mesma configuração que carregaManutencaoInicial
            ajax: {
                url: "/api/Manutencao/",
                type: "GET",
                dataType: "json",
                data: {
                    veiculoId: veiculoId,
                    statusId: statusId,
                    mes: Mes || "",
                    ano: Ano || "",
                    dataInicial: temIni && temFim ? dataInicial : "",
                    dataFinal: temIni && temFim ? dataFinal : ""
                },
                // ...
            },
            // ... columns, language, etc (repetidas) ...
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "btnDatas.click", error);
    }
});
```

**Fluxo de Validação:**

```
┌─────────────────────────────────────────┐
│ Usuário clica btnDatas                  │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ Capturar: Mês, Ano, DataInicial, DataFinal
│ Capturar: Veículo (Syncfusion)         │
│ Capturar: Status (Syncfusion)          │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ Validar período (ambos ou nenhum)       │
│ ❌ Erro se: temIni XOR temFim           │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ Se período preenchido:                  │
│   Limpar Mês/Ano (período tem prioridade)
│ Senão:                                  │
│   Validar Mês/Ano (ambos ou nenhum)    │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ Capturar Status:                        │
│ Se vazio E há filtros → "Todas"        │
│ Senão → Valor selecionado               │
│ Default → "Aberta"                      │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ Destruir DataTable anterior             │
│ Recriar com parâmetros de filtro        │
│ Fazer GET /api/Manutencao/ com params   │
└─────────────────────────────────────────┘
```

**Características:**

| Aspecto | Detalhe |
|---------|---------|
| **Validação Período** | Ambos preenchidos OU ambos vazios |
| **Validação Mês/Ano** | Ambos preenchidos OU ambos vazios |
| **Prioridade** | Período > Mês/Ano |
| **Status Default** | "Aberta" ou "Todas" (se houver outro filtro) |
| **Syncfusion Access** | `element.ej2_instances[0].value` |
| **Erro Handling** | Alerta.Erro para validações |

---

### 4.5 Recriação de Tabela: `ListaTblManutencao(URLapi, IDapi)`

**Localização:** Linhas 503-679

**Propósito:**
Reconstruir completamente a DataTable com novos parâmetros de URL e ID dinamicamente.

```javascript
function ListaTblManutencao(URLapi, IDapi) {
    try {
        // Configurar formatos de data
        if ($.fn.dataTable && $.fn.dataTable.moment) {
            $.fn.dataTable.moment("DD/MM/YYYY");
            $.fn.dataTable.moment("DD/MM/YYYY HH:mm:ss");
            $.fn.dataTable.moment("DD/MM/YYYY HH:mm");
        }

        // Destruir DataTable anterior
        var dataTableManutencao = $("#tblManutencao").DataTable();
        dataTableManutencao.destroy();
        $("#tblManutencao tbody").empty();

        // Recriar com novos parâmetros
        dataTableManutencao = $("#tblManutencao").DataTable({
            autoWidth: false,
            dom: "Bfrtip",
            ajax: {
                url: URLapi,      // Parâmetro dinâmico
                data: { id: IDapi },  // Parâmetro dinâmico
                type: "GET",
                dataType: "json"
            },
            columns: [/* ... */],
            language: { /* ... */ }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "ListaTblManutencao", error);
    }
}
```

**Diferenças vs carregaManutencaoInicial():**

| Aspecto | carregaManutencaoInicial | ListaTblManutencao |
|---------|--------------------------|-------------------|
| URL | Fixa: `/api/Manutencao/` | Dinâmica: URLapi |
| ID | Não usa | Dinâmica: IDapi |
| Parâmetros | Múltiplos (veiculoId, statusId, mes, ano, etc) | Simples (id) |
| Uso | Carregamento inicial | Recarga em contextos específicos |

**Dados da Coluna (mapeamento JSON):**

| Coluna | Campo JSON | Descrição |
|--------|-----------|-----------|
| 0 | `numOS` | Número da OS |
| 1 | `placaDescricao` | Placa + Descrição do veículo |
| 2 | `dataSolicitacao` | Data de solicitação |
| 3 | `dataDisponibilidade` | Data de disponibilidade |
| 4 | `dataEntrega` | Data de entrega |
| 5 | `dataDevolucao` | Data de devolução |
| 6 | `dias` | Dias decorridos |
| 7 | `reserva` | Flag de reserva |
| 8 | `resumoOS` | Resumo/descrição |
| 9 | `statusOS` | Aberta/Cancelada/Fechada (renderizado) |
| 10 | `manutencaoId` | ID para ações (renderizado) |

---

### 4.6 Normalização de Booleanos: `normalizaBool(v)`

**Localização:** Linhas 684-698

```javascript
function normalizaBool(v) {
    try {
        if (v === true || v === false) return v;
        if (typeof v === "number") return v === 1;
        if (v == null) return false;
        var s = String(v).trim().toLowerCase();
        return s === "true" || s === "1" || s === "sim" || s === "enviado";
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "normalizaBool", error);
        return false;
    }
}
```

**Lógica:**

| Entrada | Saída | Contexto |
|---------|-------|---------|
| `true` \| `false` | Retorna como está | Boolean puro |
| `1` | `true` | Número 1 |
| `0` | `false` | Número 0 |
| `null` \| `undefined` | `false` | Valores nulos |
| `"true"` \| `"1"` \| `"sim"` \| `"enviado"` | `true` | String normalizada |
| Qualquer outro | `false` | Fallback |

**Propósito:** Converter booleanos vindos do servidor (que podem ter formatos variados) em valores JS padrão.

---

### 4.7 Listagem Completa: `ListaTodasManutencao()`

**Localização:** Linhas 707-723

```javascript
var escolhendoVeiculo = false;
var escolhendoData = false;
var escolhendoStatus = false;

function ListaTodasManutencao() {
    try {
        escolhendoVeiculo = false;
        escolhendoData = false;
        escolhendoStatus = false;

        URLapi = "api/manutencao/ListaManutencao";
        IDapi = "";

        ListaTblManutencao(URLapi, IDapi);
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "ListaTodasManutencao", error);
    }
}
```

**Propósito:**
- Reset dos flags de filtro
- Define URL padrão para listar todas as manutenções
- Chama ListaTblManutencao com parâmetros estáticos

---

### 4.8 Modal de Itens: Iniciação no `shown.bs.modal`

**Localização:** Linhas 743-858

```javascript
$("#modalManutencao")
    .on("shown.bs.modal", function (event) {
        try {
            // Limpa array de itens removidos
            itensRemovidosParaPendente = [];

            // Obtém ManutencaoId
            var ManutencaoId = $("#txtId").val() || $("#btnFecharManutencao").data("id");

            if (!ManutencaoId) {
                console.warn("ManutencaoId não encontrado");
                return;
            }

            // Destruir DataTable anterior se existir
            if ($.fn.DataTable.isDataTable("#tblItens")) {
                $("#tblItens").DataTable().destroy();
                $("#tblItens tbody").empty();
            }

            // Recriar DataTable de itens
            dataTableItens = $("#tblItens").DataTable({
                autoWidth: false,
                order: [[2, "desc"]],
                columnDefs: [
                    { targets: 0, visible: true, className: "text-center", width: "8%" },
                    // ... 12 colunas ...
                ],
                ajax: {
                    url: "/api/Manutencao/ItensOS",
                    data: { id: ManutencaoId },
                    type: "GET",
                    dataType: "json"
                },
                columns: [
                    { data: "tipoItem" },
                    { data: "numFicha" },
                    { data: "dataItem" },
                    { data: "nomeMotorista" },
                    { data: "resumo" },
                    {
                        data: null,
                        render: function (data, type, full) {
                            // Renderizar botões de foto e remover
                        }
                    },
                    { data: "itemManutencaoId" },
                    { data: "descricao" },
                    { data: "status" },
                    { data: "motoristaId" },
                    { data: "imagemOcorrencia" },
                    { data: "viagemId" }
                ],
                language: { /* ... */ }
            });
        } catch (error) {
            Alerta.TratamentoErroComLinha("ListaManutencao.js", "modalManutencao.shown", error);
        }
    });
```

**Evento Disparado:** `shown.bs.modal` (após modal estar completamente visível)

**Fluxo:**

```
┌────────────────────────────────────┐
│ Modal de Manutenção abre           │
└────────────────────────────────────┘
              ↓
┌────────────────────────────────────┐
│ Evento: shown.bs.modal dispara      │
└────────────────────────────────────┘
              ↓
┌────────────────────────────────────┐
│ Limpar itensRemovidosParaPendente   │
│ Obter ManutencaoId                 │
│ Validar se ID existe               │
└────────────────────────────────────┘
              ↓
┌────────────────────────────────────┐
│ Destruir tblItens anterior          │
│ Esvaziar tbody                      │
└────────────────────────────────────┘
              ↓
┌────────────────────────────────────┐
│ Recriar tblItens com novo ID       │
│ GET /api/Manutencao/ItensOS?id=... │
│ Renderizar com botões (foto, remover)
└────────────────────────────────────┘
```

**Colunas da tabela de itens:**

| # | Campo | Visible | Propósito |
|---|-------|---------|-----------|
| 0 | tipoItem | Sim | Tipo do item (ex: Peça, Serviço) |
| 1 | numFicha | Sim | Número de ficha/referência |
| 2 | dataItem | Sim | Data do item |
| 3 | nomeMotorista | Sim | Nome do motorista |
| 4 | resumo | Sim | Resumo/descrição |
| 5 | Ações | Sim | Botões: Ver Foto, Remover |
| 6-11 | Dados | Não | Campos ocultos para lógica |

---

### 4.9 Visualização de Fotos: `btn-ver-foto.click`

**Localização:** Linhas 863-888

```javascript
$(document).on("click", ".btn-ver-foto", function () {
    try {
        var imagem = $(this).data("imagem");
        var imgEl = document.getElementById("imgViewerOcorrencia");
        var placeholder = document.getElementById("noImagePlaceholder");

        if (imagem && imagem.trim().length > 0) {
            imgEl.src = imagem;
            imgEl.style.display = "block";
            if (placeholder) placeholder.style.display = "none";
        } else {
            imgEl.style.display = "none";
            if (placeholder) placeholder.style.display = "block";
        }

        var modal = new bootstrap.Modal(document.getElementById("modalFoto"));
        modal.show();
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "btn-ver-foto.click", error);
    }
});
```

**Lógica:**
1. Captura URL da imagem do atributo `data-imagem`
2. Se imagem válida: exibe em modalFoto
3. Se vazia: exibe placeholder
4. Abre modal Bootstrap com `new bootstrap.Modal()`

**Estados do Botão (renderizado em 4.8):**

```javascript
// Com imagem
const btnFoto = `<button class="btn-ver-foto"
    data-imagem="${full.imagemOcorrencia}"
    style="background: linear-gradient(135deg, #17a2b8, #138496);">
    <i class="fa-duotone fa-camera-polaroid"></i>
</button>`;

// Sem imagem
const btnFoto = `<button class="btn-ver-foto"
    data-imagem=""
    disabled style="opacity: 0.6;">
    <i class="fa-duotone fa-camera-slash"></i>
</button>`;
```

---

### 4.10 Toggle de Reserva: `lstReserva.change`

**Localização:** Linhas 893-909

```javascript
$("#lstReserva").change(function () {
    try {
        var val = $(this).val();
        if (val === "1") {
            $("#divReserva").slideDown(200);
        } else {
            $("#divReserva").slideUp(200);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "lstReserva.change", error);
    }
});
```

**Comportamento:**
- Valor "1" → Exibe campos de reserva (slideDown)
- Outro valor → Oculta campos de reserva (slideUp)

**Campos de Reserva (presumidos):**
- `#txtDataRecebimentoReserva`
- `#txtDataDevolucaoReserva`
- `#lstVeiculoReserva` (Syncfusion ComboBox)

---

### 4.11 Remoção de Itens: `btn-remover-item-baixa.click`

**Localização:** Linhas 914-966

```javascript
$(document).on("click", ".btn-remover-item-baixa", function () {
    try {
        var $btn = $(this);
        var $tr = $btn.closest("tr");
        var rowData = dataTableItens.row($tr).data();

        if (!rowData) {
            AppToast.show("Vermelho", "Erro ao obter dados do item.", 2000);
            return;
        }

        Alerta.Confirmar(
            "Remover da Baixa?",
            "Este item NÃO será baixado junto com a OS e ficará como PENDENTE. Deseja continuar?",
            "Sim, Remover",
            "Cancelar"
        ).then(function (confirmado) {
            if (confirmado) {
                try {
                    // Armazena o item removido
                    itensRemovidosParaPendente.push({
                        itemManutencaoId: rowData.itemManutencaoId,
                        viagemId: rowData.viagemId,
                        tipoItem: rowData.tipoItem,
                        numFicha: rowData.numFicha
                    });

                    // Remove a linha do grid
                    dataTableItens.row($tr).remove().draw(false);

                    AppToast.show("Amarelo", "Item removido. Ficará como PENDENTE após a baixa.", 3000);

                    console.log("[ListaManutencao.js] Itens removidos para Pendente:", itensRemovidosParaPendente);
                } catch (error) {
                    Alerta.TratamentoErroComLinha("ListaManutencao.js", "btn-remover-item-baixa.confirm", error);
                }
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "btn-remover-item-baixa.click", error);
    }
});
```

**Fluxo:**

```
┌──────────────────────────────────┐
│ Usuário clica "Remover da Baixa" │
└──────────────────────────────────┘
              ↓
┌──────────────────────────────────┐
│ Obter dados da linha             │
│ Validar se linha existe          │
└──────────────────────────────────┘
              ↓
┌──────────────────────────────────┐
│ Alerta.Confirmar (SweetAlert)    │
│ Mensagem: item ficará PENDENTE    │
└──────────────────────────────────┘
              ↓
         ┌─────────┴─────────┐
         ↓                   ↓
    [Sim, Remover]    [Cancelar]
         ↓                   ↓
┌─────────────────┐  [Sem ação]
│ Armazenar item  │
│ Remover linha   │
│ Toast amarelo   │
└─────────────────┘
```

**Informações Armazenadas:**
```javascript
{
    itemManutencaoId: string,
    viagemId: string,
    tipoItem: string,
    numFicha: string
}
```

**Notificações:**
- Erro: Toast vermelho
- Sucesso: Toast amarelo (3s)
- Debug: console.log

---

### 4.12 Baixa de OS: `btnFecharManutencao.click`

**Localização:** Linhas 971-1060

```javascript
$("#btnFecharManutencao").click(function () {
    try {
        var ManutencaoId = $(this).data("id");
        var dataDevolucao = $("#txtDataDevolucao").val();
        var resumoOS = $("#txtResumoOS").val();
        var reservaEnviado = $("#lstReserva").val();
        var veiculoReservaId = "";
        var dataRecebimentoReserva = $("#txtDataRecebimentoReserva").val();
        var dataDevolucaoReserva = $("#txtDataDevolucaoReserva").val();

        // Capturar veículo de reserva (Syncfusion)
        var veiculoReservaCombo = document.getElementById("lstVeiculoReserva");
        if (veiculoReservaCombo?.ej2_instances?.length > 0) {
            var combo = veiculoReservaCombo.ej2_instances[0];
            if (combo.value) veiculoReservaId = combo.value;
        }

        // Validar data de devolução
        if (!dataDevolucao) {
            Alerta.Warning("Campo Obrigatório", "Informe a Data de Devolução.", "Ok");
            return;
        }

        // Montar mensagem com itens removidos
        var msgConfirm = "Deseja baixar esta Ordem de Serviço?";
        if (itensRemovidosParaPendente.length > 0) {
            msgConfirm += "\n\n⚠️ " + itensRemovidosParaPendente.length +
                         " item(ns) removido(s) ficará(ão) como PENDENTE.";
        }

        Alerta.Confirmar("Confirma Baixa?", msgConfirm, "Sim", "Não")
            .then(function (confirmado) {
                if (confirmado) {
                    $.ajax({
                        url: "/api/Manutencao/BaixaOS",
                        type: "POST",
                        dataType: "json",
                        data: {
                            manutencaoId: ManutencaoId,
                            dataDevolucao: dataDevolucao,
                            resumoOS: resumoOS,
                            reservaEnviado: reservaEnviado,
                            veiculoReservaId: veiculoReservaId,
                            dataRecebimentoReserva: dataRecebimentoReserva,
                            dataDevolucaoReserva: dataDevolucaoReserva,
                            itensRemovidosJson: JSON.stringify(itensRemovidosParaPendente)
                        },
                        success: function (response) {
                            try {
                                if (response.sucesso !== false) {
                                    var msg = "Ordem de Serviço baixada com sucesso!";
                                    if (itensRemovidosParaPendente.length > 0) {
                                        msg += " (" + itensRemovidosParaPendente.length +
                                               " item(ns) marcado(s) como Pendente)";
                                    }
                                    AppToast.show("Verde", msg, 4000);

                                    itensRemovidosParaPendente = []; // Limpar

                                    $("#modalManutencao").modal("hide");
                                    $("#btnDatas").click();
                                } else {
                                    AppToast.show("Vermelho", response.message || "Erro ao baixar a OS.", 3000);
                                }
                            } catch (error) {
                                Alerta.TratamentoErroComLinha("ListaManutencao.js", "BaixaOS.success", error);
                            }
                        },
                        error: function () {
                            AppToast.show("Vermelho", "Erro de comunicação com o servidor.", 3000);
                        }
                    });
                }
            });
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "btnFecharManutencao.click", error);
    }
});
```

**Fluxo Completo:**

```
┌──────────────────────────────────────┐
│ Usuário clica "Baixar OS"            │
└──────────────────────────────────────┘
              ↓
┌──────────────────────────────────────┐
│ Capturar valores do formulário       │
│ - dataDevolucao (obrigatório)        │
│ - resumoOS                           │
│ - Reserva (1=sim, 0=não)             │
│ - Veículo de Reserva (se reserva)    │
│ - Datas de Reserva                   │
│ - Itens Removidos (JSON)             │
└──────────────────────────────────────┘
              ↓
┌──────────────────────────────────────┐
│ Validar Data de Devolução            │
│ ❌ Se vazia: Alerta.Warning          │
└──────────────────────────────────────┘
              ↓
┌──────────────────────────────────────┐
│ Alerta.Confirmar                     │
│ Informar se há itens PENDENTES       │
└──────────────────────────────────────┘
              ↓
         ┌─────────┴─────────┐
         ↓                   ↓
      [Sim]             [Não]
         ↓                   ↓
┌─────────────────┐  [Cancelar]
│ POST /api/...   │
│ BaixaOS         │
└─────────────────┘
         ↓
    ┌────┴────┐
    ↓         ↓
[Sucesso] [Erro]
    ↓         ↓
  ✅ Toast  ❌ Toast
  Fechar    Msg erro
  Modal
```

**Payload POST:**

```javascript
{
    manutencaoId: string,           // ID da OS
    dataDevolucao: string,          // Data (obrigatória)
    resumoOS: string,               // Descrição
    reservaEnviado: string,         // "1" ou "0"
    veiculoReservaId: string,       // ID do veículo de reserva
    dataRecebimentoReserva: string, // Data de recebimento
    dataDevolucaoReserva: string,   // Data de devolução
    itensRemovidosJson: string      // JSON stringified
}
```

**Resposta Esperada:**

```javascript
{
    sucesso: boolean,
    message: string (opcional)
}
```

---

### 4.13 Cancelamento de OS: `btn-deletemanutencao.click`

**Localização:** Linhas 1065-1109

```javascript
$(document).on("click", ".btn-deletemanutencao", function () {
    try {
        var ManutencaoId = $(this).data("id");

        Alerta.Confirmar("Confirma Cancelamento?",
                        "Deseja cancelar esta Ordem de Serviço?",
                        "Sim, Cancelar",
                        "Não")
            .then(function (confirmado) {
                if (confirmado) {
                    $.ajax({
                        url: "/api/Manutencao/CancelaManutencao",
                        type: "POST",
                        dataType: "json",
                        data: { id: ManutencaoId },
                        success: function (response) {
                            try {
                                if (response.sucesso !== false) {
                                    AppToast.show("Verde", "Ordem de Serviço cancelada!", 3000);
                                    $("#btnDatas").click();
                                } else {
                                    AppToast.show("Vermelho", "Erro ao cancelar a OS.", 3000);
                                }
                            } catch (error) {
                                Alerta.TratamentoErroComLinha("ListaManutencao.js",
                                                              "CancelaManutencao.success", error);
                            }
                        },
                        error: function () {
                            AppToast.show("Vermelho", "Erro de comunicação com o servidor.", 3000);
                        }
                    });
                }
            });
    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "btn-deletemanutencao.click", error);
    }
});
```

**Fluxo:**
1. Confirmar cancelamento (SweetAlert)
2. POST `/api/Manutencao/CancelaManutencao` com ID
3. Se sucesso: Toast verde + Recarregar tabela
4. Se erro: Toast vermelho

---

### 4.14 Abertura do Modal: `btn-baixar.click`

**Localização:** Linhas 1114-1150

```javascript
$(document).on("click", ".btn-baixar", function (e) {
    try {
        // Verificar se desabilitado
        if ($(this).attr("aria-disabled") === "true") {
            e.preventDefault();
            e.stopPropagation();
            return;
        }

        var ManutencaoId = $(this).data("id");
        var $tr = $(this).closest("tr");

        // Obter dados da linha
        var dt = $("#tblManutencao").DataTable();
        var dataRow = dt.row($tr).data() || {};

        // Definir ID no modal
        $("#txtId").val(ManutencaoId);
        $("#btnFecharManutencao").data("id", ManutencaoId);

        // Preencher campos iniciais
        document.getElementById("txtDataDevolucao").value = moment().format("YYYY-MM-DD");
        $("#txtResumoOS").val(dataRow.resumoOS || "");

        // Abrir modal
        var modalEl = document.getElementById("modalManutencao");
        var modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();

    } catch (error) {
        Alerta.TratamentoErroComLinha("ListaManutencao.js", "btn-baixar.click", error);
    }
});
```

**Lógica:**
1. Validar se botão está desabilitado (aria-disabled)
2. Obter ID e dados da linha
3. Preencher formulário do modal:
   - txtDataDevolucao = Data atual (moment())
   - txtResumoOS = Resumo da linha
4. Abrir modal com Bootstrap API

**Preenchimento Automático:**
- Data de Devolução: hoje (YYYY-MM-DD)
- Resumo OS: da linha selecionada

---

## 5️⃣ FLUXO GERAL DE DADOS

```
┌─────────────────────────────────────────────────────────────────┐
│ 1. PÁGINA CARREGA                                               │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ 2. DOMContentLoaded EVENT                                        │
│    - Remover Bootstrap tooltips                                  │
│    - Converter para Syncfusion (data-ejtip)                      │
│    - Chamar carregaManutencaoInicial()                           │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ 3. carregaManutencaoInicial()                                    │
│    - Mostrar loading overlay                                     │
│    - Inicializar DataTable com GET /api/Manutencao              │
│    - Status Default: "Aberta"                                    │
│    - Renderizar colunas com status badges e ações                │
│    - Ocultar loading ao completar                                │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ 4. USUÁRIO INTERAGE - Filtros                                    │
│    - Seleciona Veículo (ComboBox Syncfusion)                     │
│    - Seleciona Status (ComboBox Syncfusion)                      │
│    - Seleciona Mês/Ano OU Data Inicial/Final                    │
│    - Clica "btnDatas"                                            │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ 5. btnDatas.click HANDLER                                        │
│    - Validar período (ambos ou nenhum)                           │
│    - Validar Mês/Ano (ambos ou nenhum)                           │
│    - Aplicar lógica de prioridade                                │
│    - Determinar status (Aberta / Todas)                          │
│    - Destruir DataTable anterior                                 │
│    - Recriar com novos parâmetros                                │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ 6. USUÁRIO CLICA AÇÃO                                            │
│                                                                  │
│    ┌─ Editar (lápis)                                             │
│    │  └─ Navegar para /Manutencao/Upsert?id=...                 │
│    │                                                              │
│    ├─ Baixar (bandeira)                                          │
│    │  ├─ btn-baixar.click                                        │
│    │  ├─ Preencher modal com dados                               │
│    │  ├─ Abrir modalManutencao                                   │
│    │  └─ Ocultar tblItens no modal (shown.bs.modal event)        │
│    │                                                              │
│    └─ Cancelar (proibido)                                        │
│       ├─ btn-deletemanutencao.click                              │
│       ├─ Confirmar com Alerta.Confirmar                          │
│       └─ POST /api/Manutencao/CancelaManutencao                  │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ 7. MODAL DE BAIXA ABRE (modalManutencao)                         │
│    - Evento: shown.bs.modal                                      │
│    - Limpar itensRemovidosParaPendente                           │
│    - Inicializar tblItens com GET /api/Manutencao/ItensOS       │
│    - Renderizar itens com botões (foto, remover)                 │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ 8. USUÁRIO INTERAGE NO MODAL                                     │
│                                                                  │
│    ├─ Ver Foto de Ocorrência                                     │
│    │  ├─ btn-ver-foto.click                                      │
│    │  └─ Abre modalFoto com imagem                               │
│    │                                                              │
│    ├─ Remover Item da Baixa                                      │
│    │  ├─ btn-remover-item-baixa.click                            │
│    │  ├─ Confirmar com Alerta.Confirmar                          │
│    │  ├─ Armazenar em itensRemovidosParaPendente[]               │
│    │  └─ Remover linha do tblItens                               │
│    │                                                              │
│    └─ Alternar Reserva                                           │
│       ├─ lstReserva.change                                       │
│       └─ Exibir/ocultar campos de veículo/datas de reserva       │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ 9. CONFIRMAR BAIXA                                               │
│    - btnFecharManutencao.click                                   │
│    - Validar Data de Devolução (obrigatória)                     │
│    - Confirmar com Alerta.Confirmar (mostrar itens pendentes)    │
│    - POST /api/Manutencao/BaixaOS                                │
│    - Enviar itensRemovidosJson                                   │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ 10. RESULTADO DA BAIXA                                           │
│                                                                  │
│    ✅ Sucesso:                                                   │
│       - Toast verde (4s)                                         │
│       - Limpar itensRemovidosParaPendente                        │
│       - Fechar modal                                             │
│       - Recarregar tabela (clicando btnDatas)                    │
│                                                                  │
│    ❌ Erro:                                                      │
│       - Toast vermelho com mensagem                              │
│       - Modal permanece aberto                                   │
└─────────────────────────────────────────────────────────────────┘
```

---

## 6️⃣ TABELAS DE DEPENDÊNCIA

### TABELA 1: Endpoints HTTP

| ID | Método | Endpoint | Consumidor | Propósito | Dados Enviados |
|----|--------|----------|-----------|-----------|-----------------|
| E1 | GET | `/api/Manutencao/` | carregaManutencaoInicial, btnDatas | Listar OSs com filtros | veiculoId, statusId, mes, ano, dataInicial, dataFinal |
| E2 | GET | `/api/Manutencao/ItensOS` | modalManutencao.shown | Listar itens de uma OS | id (manutencaoId) |
| E3 | POST | `/api/Manutencao/BaixaOS` | btnFecharManutencao | Baixar/Fechar OS | manutencaoId, dataDevolucao, resumoOS, reserva*, itensRemovidosJson |
| E4 | POST | `/api/Manutencao/CancelaManutencao` | btn-deletemanutencao | Cancelar OS | id (manutencaoId) |

*reservaEnviado, veiculoReservaId, dataRecebimentoReserva, dataDevolucaoReserva

### TABELA 2: Funções JavaScript Definidas

| ID | Função | Tipo | Localização | Propósito | Try-Catch | Dependências |
|----|--------|------|-------------|-----------|-----------|--------------|
| F1 | `mostrarLoadingManutencao()` | Utility | Inline | Exibir overlay de loading | ✅ SIM | DOM |
| F2 | `esconderLoadingManutencao()` | Utility | Inline | Ocultar overlay de loading | ✅ SIM | DOM |
| F3 | `carregaManutencaoInicial()` | Inicializador | Inline | Carregar tabela inicial | ✅ SIM | jQuery, DataTables, AJAX, Alerta |
| F4 | `btnDatas.click` | Handler | Inline | Aplicar filtros avançados | ✅ SIM | jQuery, Syncfusion, Alerta |
| F5 | `ListaTblManutencao(URLapi, IDapi)` | Recarregador | Inline | Recriar DataTable com novos params | ✅ SIM | jQuery, DataTables, AJAX |
| F6 | `normalizaBool(v)` | Converter | Inline | Normalizar valores booleanos | ✅ SIM | Nenhuma |
| F7 | `ListaTodasManutencao()` | Resetador | Inline | Reset filtros e carregar todas | ✅ SIM | ListaTblManutencao |
| F8 | `modalManutencao.shown` | Handler | jQuery.on | Inicializar tblItens ao abrir | ✅ SIM | jQuery, DataTables, AJAX |
| F9 | `btn-ver-foto.click` | Handler | jQuery.on | Visualizar foto de ocorrência | ✅ SIM | Bootstrap Modal |
| F10 | `lstReserva.change` | Handler | jQuery.on | Toggle de campos de reserva | ✅ SIM | jQuery |
| F11 | `btn-remover-item-baixa.click` | Handler | jQuery.on | Remover item da baixa | ✅ SIM | jQuery, Alerta, Toast |
| F12 | `btnFecharManutencao.click` | Handler | jQuery.on | Baixar OS | ✅ SIM | jQuery, AJAX, Alerta, Toast |
| F13 | `btn-deletemanutencao.click` | Handler | jQuery.on | Cancelar OS | ✅ SIM | jQuery, AJAX, Alerta, Toast |
| F14 | `btn-baixar.click` | Handler | jQuery.on | Abrir modal de baixa | ✅ SIM | jQuery, Bootstrap Modal, moment |

### TABELA 3: Componentes Externos

| ID | Biblioteca | Componente | Uso | Versão |
|----|-----------|-----------|-----|--------|
| C1 | jQuery | `$()`, `$.ajax()`, `$.fn.dataTable` | DOM manipulation, AJAX, DataTables | (não especificada) |
| C2 | DataTables | Inicialização, destroy, draw | Tabelas interativas | (não especificada) |
| C3 | Bootstrap | Modal API, Tooltip | Modais e tooltips | 5.x (getOrCreateInstance) |
| C4 | Syncfusion | ComboBox (EJ2) | Dropdowns de filtros | 2023+ (ej2_instances) |
| C5 | moment.js | `moment().format()` | Formatação de datas | (não especificada) |
| C6 | Font Awesome | Icons duotone | Ícones em botões e badges | 6.x (fa-duotone) |
| C7 | Custom (FrotiX) | `Alerta.*` | Sistema de alertas | (padrão FrotiX) |
| C8 | Custom (FrotiX) | `AppToast.show()` | Notificações toast | (padrão FrotiX) |

---

## 7️⃣ VALIDAÇÕES DE CONFORMIDADE FROTIX

### ✅ Regras de Desenvolvimento

| Regra | Implementado | Evidência | Linha |
|-------|-------------|-----------|-------|
| Try-Catch obrigatório em todas funções | ✅ SIM | 14/14 handlers com try-catch | Múltiplas |
| Usar Alerta.TratamentoErroComLinha | ✅ SIM | 17 ocorrências no arquivo | Múltiplas |
| Usar fa-duotone | ⚠️ PARCIAL | 25 refs a fa-solid (DEVERIA SER fa-duotone) | 152, 156, 158, 182, 191, 198, 204, 387, 391, 394, 418, 427, 435, 441, 569, 575, 576, 600, 609, 616, 622, 810, 828 |
| Sem alert() nativo | ✅ SIM | Nenhum alert() encontrado | - |
| Usar Alerta.* para confirmações | ✅ SIM | Alerta.Confirmar, Alerta.Warning, Alerta.Erro | 274, 287, 928, 1003, 1071, 1003 |
| Usar AppToast para notificações | ✅ SIM | 9 ocorrências de AppToast.show | 924, 950, 1033, 1042, 1051, 1087, 1091, 1100 |
| Validação de entrada | ✅ SIM | Múltiplas validações de campo | 269-276, 285-289, 307-310, 990-994 |
| Erro handling AJAX | ✅ SIM | error callbacks em $.ajax | 123-127, 359-363, 1049-1052, 1098-1101 |

### ⚠️ PONTOS DE ATENÇÃO

**CRÍTICO - Uso de fa-solid ao invés de fa-duotone:**
```javascript
// ❌ INCORRETO (25 ocorrências):
<i class="fa-solid fa-circle-check"></i>
<i class="fa-solid fa-xmark"></i>
<i class="fa-solid fa-lock"></i>

// ✅ DEVERIA SER:
<i class="fa-duotone fa-circle-check" style="--fa-primary-color: #fff; --fa-secondary-color: #...;"></i>
<i class="fa-duotone fa-xmark" style="--fa-primary-color: #fff; --fa-secondary-color: #...;"></i>
<i class="fa-duotone fa-lock" style="--fa-primary-color: #fff; --fa-secondary-color: #...;"></i>
```

**Recomendação:** Criar PR corrigindo todos os `fa-solid` para `fa-duotone` com estilos apropriados.

---

## 8️⃣ ESTRUTURA HTML ESPERADA

### Elementos Obrigatórios

```html
<!-- Loading Overlay -->
<div id="loadingOverlayManutencao" class="loading-overlay" style="display: none;">
    <!-- FtxSpin.show() ou similar -->
</div>

<!-- Filtros -->
<select id="lstMes"></select>
<select id="lstAno"></select>
<input id="txtDataInicial" type="date">
<input id="txtDataFinal" type="date">
<div id="lstVeiculos" class="ej2-component"></div> <!-- Syncfusion ComboBox -->
<div id="lstStatus" class="ej2-component"></div>   <!-- Syncfusion ComboBox -->
<button id="btnDatas">Aplicar Filtros</button>

<!-- Tabela Principal -->
<table id="tblManutencao" class="table">
    <thead>
        <tr>
            <th>OS</th>
            <th>Veículo</th>
            <th>Data Solicitação</th>
            <!-- ... 8 colunas mais ... -->
        </tr>
    </thead>
    <tbody></tbody>
</table>

<!-- Modal de Baixa -->
<div id="modalManutencao" class="modal fade">
    <input id="txtId" type="hidden">
    <input id="txtDataDevolucao" type="date">
    <textarea id="txtResumoOS"></textarea>
    <select id="lstReserva">
        <option value="0">Não</option>
        <option value="1">Sim</option>
    </select>
    <div id="divReserva" style="display: none;">
        <div id="lstVeiculoReserva" class="ej2-component"></div>
        <input id="txtDataRecebimentoReserva" type="date">
        <input id="txtDataDevolucaoReserva" type="date">
    </div>

    <!-- Tabela de Itens -->
    <table id="tblItens" class="table">
        <thead>
            <tr>
                <th>Tipo</th>
                <th>Ficha</th>
                <th>Data</th>
                <th>Motorista</th>
                <th>Resumo</th>
                <th>Ações</th>
                <!-- ... colunas ocultas ... -->
            </tr>
        </thead>
        <tbody></tbody>
    </table>

    <button id="btnFecharManutencao" data-id="">Baixar OS</button>
</div>

<!-- Modal de Foto -->
<div id="modalFoto" class="modal fade">
    <img id="imgViewerOcorrencia">
    <div id="noImagePlaceholder">Sem Imagem</div>
</div>
```

---

## 9️⃣ FLUXO DE ESTADO GLOBAL

```javascript
// Estado global do módulo:

var URLapi = "";                    // Endpoint atual
var IDapi = "";                     // ID do contexto

var escolhendoVeiculo = false;      // Flag de filtro
var escolhendoData = false;         // Flag de filtro
var escolhendoStatus = false;       // Flag de filtro

var dataTableItens;                 // Referência ao DataTable de itens
var LinhaManutencaoSelecionada = 0; // Índice de linha selecionada

var itensRemovidosParaPendente = [];  // Array de itens a marcar como pendente
                                      // Estrutura: { itemManutencaoId, viagemId, tipoItem, numFicha }
```

---

## 🔟 SEQUÊNCIA COMUM DE USO

### Caso 1: Carregar Página
```
1. DOMContentLoaded
2. carregaManutencaoInicial()
3. GET /api/Manutencao/ (status=Aberta)
4. Renderizar DataTable com OSs abertas
```

### Caso 2: Filtrar por Veículo + Status
```
1. Usuário seleciona veículo (ComboBox)
2. Usuário seleciona status (ComboBox)
3. Usuário clica btnDatas
4. Validar (OK)
5. Destruir DataTable
6. GET /api/Manutencao/?veiculoId=X&statusId=Y
7. Recriar DataTable com resultados
```

### Caso 3: Baixar OS
```
1. Usuário clica btn-baixar em linha
2. btn-baixar.click handler
3. Preencher modal com dados
4. Abrir modalManutencao
5. shown.bs.modal event → Carregar tblItens
6. GET /api/Manutencao/ItensOS?id=X
7. Usuário pode remover itens (itensRemovidosParaPendente[])
8. Usuário clica btnFecharManutencao
9. Validar Data de Devolução
10. Confirmar com Alerta.Confirmar
11. POST /api/Manutencao/BaixaOS (com itensRemovidosJson)
12. Toast verde + Recarregar tabela principal
```

### Caso 4: Cancelar OS
```
1. Usuário clica btn-deletemanutencao em linha
2. Confirmar com Alerta.Confirmar
3. POST /api/Manutencao/CancelaManutencao?id=X
4. Toast verde + Recarregar tabela
```

---

## 1️⃣1️⃣ CONCLUSÕES

### Pontos Fortes
1. ✅ Código bem organizado com múltiplos handlers
2. ✅ 100% conformidade com try-catch obrigatório
3. ✅ Uso consistente de Alerta.TratamentoErroComLinha
4. ✅ DataTables com configuração avançada (Excel, PDF, paginação)
5. ✅ Validações robustas de entrada (período, Mês/Ano)
6. ✅ Integração com Syncfusion ComboBoxes
7. ✅ Gerenciamento de estado (itensRemovidosParaPendente)
8. ✅ Padrão FrotiX de notificações (Alerta, AppToast)

### Áreas de Melhoria
1. ⚠️ **CRÍTICO:** Trocar fa-solid por fa-duotone (25 ocorrências)
2. 📌 Duplicação de código: carregaManutencaoInicial vs btnDatas DataTable (quase 250 linhas repetidas)
   - Sugestão: Criar função auxiliar `initDataTable(config)`
3. 📌 Tamanho do arquivo: 1150 linhas (considerar dividir em módulos)
4. 📌 Sem debounce nos eventos de filtro (multiple calls rápidas → multiple requisições)
5. 📌 Sem cache de dados para melhor performance

### Recomendações
1. **Refatoração Urgente:** Corrigir fa-solid → fa-duotone
2. **Refatoração Importante:** Extrair DataTable config para função reutilizável
3. **Performance:** Considerar lazy-loading de itens em OSs grandes
4. **UX:** Adicionar indicador de quantos itens foram removidos (antes de confirmar)
5. **Documentação:** Adicionar JSDoc para funções principais

---

## 1️⃣2️⃣ REFERÊNCIAS

- **Arquivo Fonte:** `/mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/wwwroot/js/cadastros/ListaManutencao.js`
- **Padrão Analisado:** Documentacao/EXEMPLO_ANALISE_COMPLETA.md (Lote 481)
- **Regras FrotiX:** RegrasDesenvolvimentoFrotiX.md
- **Dependências:** jQuery 3.x, DataTables 1.10+, Bootstrap 5.x, Syncfusion EJ2, moment.js, Font Awesome 6.x

---

**Análise Realizada em:** 02/02/2026
**Supervisor:** Claude Sonnet 4.5
**Versão da Documentação:** 1.0
**Status:** ✅ COMPLETO E VALIDADO
