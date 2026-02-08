# Documentação - atualizacustosviagem.js

**Arquivo:** `/FrotiX.Site/wwwroot/js/cadastros/atualizacustosviagem.js`

**Responsabilidade:** Gerenciar a tela de ajuste/atualização de custos e dados de viagens com interface modal, carregamento de dados via API e gravação de alterações.

**Padrão:** FrotiX - Bootstrap 5 + Syncfusion + jQuery

**Data de Criação:** Desconhecida (documentação: 02/02/2026)

---

## 📋 Índice

1. [Visão Geral](#visão-geral)
2. [Variáveis Globais](#variáveis-globais)
3. [Funções Principais](#funções-principais)
4. [Fluxo de Dados](#fluxo-de-dados)
5. [Integração com API](#integração-com-api)
6. [Componentes UI](#componentes-ui)
7. [Tratamento de Erros](#tratamento-de-erros)
8. [Notas Técnicas](#notas-técnicas)

---

## 🎯 Visão Geral

Este arquivo JavaScript implementa a funcionalidade de **ajuste e atualização de custos de viagens** no sistema FrotiX. Ele gerencia:

- **Dois modais Bootstrap 5:** Um para ajuste de custos e outro para visualização de ficha de vistoria
- **Carregamento de dados** de uma viagem específica via API REST
- **Validação e gravação** de alterações de dados
- **DataTable** para listagem de viagens com paginação, busca e ordenação
- **Integração com Syncfusion** para componentes especializados (dropdowns, calendários, etc.)

**Casos de Uso:**
- Ajustar dados de uma viagem após sua realização
- Visualizar fichas de vistoria digitalizadas
- Atualizar motorista, veículo, horários, quilometragem, etc.

---

## 🔧 Variáveis Globais

```javascript
// Instâncias dos modais Bootstrap 5
let modalAjustaCustos = null;  // Modal para ajuste de dados
let modalFicha = null;         // Modal para ficha de vistoria
```

**Propósito:** Armazenar referências aos modais para controle programático (abrir/fechar).

---

## 🚀 Funções Principais

### 1. **$(document).ready(function())**

**Responsabilidade:** Inicializar componentes quando o DOM está pronto.

**Fluxo:**
```
document.ready
  ├─ inicializarModais() → Configura modais Bootstrap e listeners
  └─ initDataTable() → Carrega e exibe tabela de viagens
```

**Tratamento de Erro:** Toda exceção é capturada e enviada para `Alerta.TratamentoErroComLinha()`.

---

### 2. **inicializarModais()**

**Responsabilidade:** Configurar os dois modais e seus event listeners.

**Detalhamento:**

#### Modal 1: Ajusta Custos (`#modalAjustaCustos`)
```javascript
// Instancia modal com opções
modalAjustaCustos = new bootstrap.Modal(modalAjustaCustosEl, {
    keyboard: true,        // Permite fechar com ESC
    backdrop: "static"     // Não fecha ao clicar fora
});

// Listener para evento 'show'
// Quando modal abre, carrega dados da viagem pelo ID
```

**Fluxo ao abrir:**
1. Obtém `data-id` (ID da viagem) do botão que disparou o modal
2. Chama `carregarDadosViagem(viagemId)` para populartodos os campos
3. Aguarda 300ms para Syncfusion carregar dados antes de setar valores

#### Modal 2: Ficha de Vistoria (`#modalFicha`)
```javascript
// Listener para evento 'show'
// Salva o ID da viagem em campo oculto
// Carrega imagem da ficha via carregarFichaVistoria()
```

**Fluxo ao abrir:**
1. Obtém viagem ID do botão
2. Salva em `#txtViagemId`
3. Chama `carregarFichaVistoria(viagemId, button)` para carregar imagem

#### Botão de Ação (`#btnAjustarViagem`)
```javascript
// Listener para clique
// Dispara gravarViagem() quando clicado
```

---

### 3. **mostrarLoading(mensagem)**

**Responsabilidade:** Exibir overlay de carregamento com mensagem personalizada.

**Parâmetros:**
- `mensagem` (string, opcional): Texto a exibir no overlay

**Implementação:**
```javascript
// Busca elemento #loadingOverlayCustos
// Define display: "flex"
// Atualiza mensagem em #txtLoadingMessage se fornecida
```

**Uso Típico:**
```javascript
mostrarLoading("Carregando Dados de Viagens...");
```

---

### 4. **esconderLoading()**

**Responsabilidade:** Ocultar o overlay de carregamento.

**Implementação:**
```javascript
// Busca elemento #loadingOverlayCustos
// Define display: "none"
```

---

### 5. **carregarDadosViagem(viagemId)**

**Responsabilidade:** Buscar dados de uma viagem específica via API e popular o modal de ajuste.

**Chamada API:**
```
GET /api/Viagem/GetViagem/{viagemId}
```

**Resposta Esperada:**
```json
{
    "success": true,
    "data": {
        "viagemId": "uuid",
        "noFichaVistoria": 123,
        "finalidade": "Evento",
        "eventoId": "uuid",
        "dataInicial": "2026-02-02",
        "horaInicio": "08:00",
        "dataFinal": "2026-02-02",
        "horaFim": "17:00",
        "kmInicial": 1000,
        "kmFinal": 1050,
        "motoristaId": "uuid",
        "veiculoId": "uuid",
        "requisitanteId": "uuid",
        "setorSolicitanteId": "uuid",
        "ramalRequisitante": "123"
    }
}
```

**Campos Populados:**

| Campo DOM | Dados | Observação |
|-----------|-------|-----------|
| `#txtId` | `viagemId` | ID da viagem |
| `#txtNoFichaVistoria` | `noFichaVistoria` | Número da ficha |
| `#lstFinalidadeAlterada` | `finalidade` | Syncfusion DropDown |
| `#lstEvento` | `eventoId` | Habilitado se finalidade="Evento" |
| `#txtDataInicial` | `dataInicial` | Data inicio (YYYY-MM-DD) |
| `#txtHoraInicial` | `horaInicio` | Hora início |
| `#txtDataFinal` | `dataFinal` | Data fim |
| `#txtHoraFinal` | `horaFim` | Hora fim |
| `#txtKmInicial` | `kmInicial` | Km iniciais |
| `#txtKmFinal` | `kmFinal` | Km finais |
| `#txtRamalRequisitante` | `ramalRequisitante` | Ramal |
| `#lstMotoristaAlterado` | `motoristaId` | Após 300ms delay |
| `#lstVeiculoAlterado` | `veiculoId` | Após 300ms delay |
| `#lstRequisitanteAlterado` | `requisitanteId` | Após 300ms delay |
| `#lstSetorSolicitanteAlterado` | `[setorSolicitanteId]` | Array (DropDownTree) |

**Lógica Especial - Finalidade "Evento":**
```javascript
if (viagem.finalidade === "Evento" && viagem.eventoId) {
    // Habilita dropdown de eventos
    lstEvento.ej2_instances[0].enabled = true;
    lstEvento.ej2_instances[0].value = [viagem.eventoId.toString()];
    // Mostra div de eventos
    $(".esconde-diveventos").show();
} else {
    // Desabilita e esconde
    lstEvento.ej2_instances[0].enabled = false;
    lstEvento.ej2_instances[0].value = null;
    $(".esconde-diveventos").hide();
}
```

**Delay de 300ms:** Necessário porque Syncfusion leva tempo para renderizar os dropdowns antes de receber valores.

---

### 6. **carregarFichaVistoria(viagemId, button)**

**Responsabilidade:** Buscar imagem da ficha de vistoria digitalizada.

**Chamada API:**
```
GET /api/Viagem/PegaFichaModal
Params: id = viagemId
```

**Resposta Esperada:**
```
// String base64 da imagem JPG OU null/false/""
```

**Lógica:**

1. **Se API retorna valor válido:**
   - Obtém número da ficha da tabela (primeira coluna da linha)
   - Monta label: "Ficha de Vistoria Nº: **123**"
   - Seta imagem: `data:image/jpg;base64,{resposta}`

2. **Se API retorna null/false/"":**
   - Monta label: "Viagem sem Ficha de Vistoria Digitalizada"
   - Seta imagem para placeholder: `/Images/FichaAmarelaNova.jpg`

**Elementos DOM:**
- `#DynamicModalLabel`: Título do modal (ícone + texto)
- `#imgViewer`: Elemento `<img>` para exibir ficha

---

### 7. **gravarViagem()**

**Responsabilidade:** Coletar dados do modal, validar e enviar para API de atualização.

**Fluxo Detalhado:**

#### 1. Coleta de Dados
```javascript
// Lê todos os campos do modal
const dados = {
    ViagemId: string,
    NoFichaVistoria: int?,
    Finalidade: string?,
    EventoId: string?, // Guid ou null
    DataInicial: date?,
    HoraInicio: time?,
    DataFinal: date?,
    HoraFim: time?,
    KmInicial: int?,
    KmFinal: int?,
    MotoristaId: string?,
    VeiculoId: string?,
    SetorSolicitanteId: string?,
    RequisitanteId: string?,
    RamalRequisitante: string?
};
```

#### 2. Validações Implícitas
- Converte km para `parseInt()` (null se vazio)
- Converte número da ficha para `parseInt()`
- Trata array do DropDownTree (pega primeiro elemento)
- Trata array do Evento (pega primeiro elemento)

#### 3. Feedback Visual
```javascript
// Mostra spinner no botão
btnAjustar.disabled = true;
spinner.classList.remove("d-none");
btnText.textContent = "Gravando...";
```

#### 4. Chamada API
```
POST /api/Viagem/AtualizarDadosViagemDashboard
Content-Type: application/json
Body: dados (JSON.stringify)
```

#### 5. Resposta Sucesso
```javascript
if (res.success) {
    // 1. Fecha modal
    modalAjustaCustos.hide();

    // 2. Mostra loading
    mostrarLoading("Atualizando dados...");

    // 3. Recarrega DataTable com callback
    $("#tblViagem").DataTable().ajax.reload(function () {
        esconderLoading();
        AppToast.show("Verde", "Viagem atualizada com sucesso!", 3000);
    }, false);
}
```

#### 6. Resposta Erro
```javascript
// Mostra toast vermelho com mensagem de erro
AppToast.show("Vermelho", res.message || "Erro ao atualizar viagem", 4000);
```

#### 7. Recuperação do Botão
```javascript
// Sempre remove spinner e restaura texto (sucesso ou erro)
spinner.classList.add("d-none");
btnText.textContent = "Ajustar Viagem";
btnAjustar.disabled = false;
```

---

### 8. **setNumericValue(elementId, value)**

**Responsabilidade:** Setar valor em campo NumericTextBox Syncfusion.

**Parâmetros:**
- `elementId`: ID do elemento
- `value`: Valor numérico

**Implementação:**
```javascript
if (element && element.ej2_instances) {
    element.ej2_instances[0].value = value || 0;
}
```

---

### 9. **getNumericValue(elementId)**

**Responsabilidade:** Ler valor de campo NumericTextBox Syncfusion.

**Retorno:** Número ou 0 se inválido.

**Implementação:**
```javascript
if (element && element.ej2_instances) {
    return element.ej2_instances[0].value || 0;
}
return 0;
```

---

### 10. **formatarDataParaInput(dataStr)**

**Responsabilidade:** Converter data entre formatos DD/MM/YYYY ↔ YYYY-MM-DD.

**Parâmetros:**
- `dataStr`: String de data em qualquer formato

**Lógica:**

1. **Já está em YYYY-MM-DD:** Retorna direto
2. **Está em DD/MM/YYYY:** Converte para YYYY-MM-DD
3. **Outro formato:** Retorna como está

**Exemplo:**
```javascript
formatarDataParaInput("15/01/2026")  // → "2026-01-15"
formatarDataParaInput("2026-01-15")  // → "2026-01-15"
```

---

### 11. **initDataTable()**

**Responsabilidade:** Inicializar tabela DataTable com listagem de viagens.

**Configuração:**

```javascript
$("#tblViagem").DataTable({
    processing: false,
    serverSide: false,
    paging: true,
    searching: true,
    ordering: true,
    order: [[1, "desc"]],  // Ordena por coluna 1 (data) descendente
    ajax: {
        url: "/api/custosviagem",
        type: "GET"
    },
    // ... (ver seção de colunas abaixo)
});
```

**Fonte de Dados:**
```
GET /api/custosviagem
```

**Resposta Esperada:**
```json
{
    "data": [
        {
            "noFichaVistoria": 123,
            "dataInicial": "2026-02-02",
            "dataFinal": "2026-02-02",
            "horaInicio": "08:00",
            "horaFim": "17:00",
            "finalidade": "Evento",
            "nomeMotorista": "João Silva",
            "descricaoVeiculo": "Ônibus 001",
            "kmInicial": 1000,
            "kmFinal": 1050,
            "viagemId": "uuid-aqui"
        }
    ]
}
```

#### Colunas da Tabela

| # | Campo | Fonte | Renderização | Ações |
|---|-------|-------|--------------|-------|
| 0 | Nº Ficha | `noFichaVistoria` | Texto | - |
| 1 | Data Inicial | `dataInicial` | Texto | Ordenável |
| 2 | Data Final | `dataFinal` | Texto | Ordenável |
| 3 | Hora Início | `horaInicio` | Texto | - |
| 4 | Hora Fim | `horaFim` | Texto | - |
| 5 | Finalidade | `finalidade` | Texto | - |
| 6 | Motorista | `nomeMotorista` | Texto | - |
| 7 | Veículo | `descricaoVeiculo` | Texto | - |
| 8 | Km Inicial | `kmInicial` | `toLocaleString("pt-BR")` | Ordenável |
| 9 | Km Final | `kmFinal` | `toLocaleString("pt-BR")` | Ordenável |
| 10 | Ações | `viagemId` | HTML (2 botões) | Não ordenável |
| 11 | Row # | `viagemId` | Meta.row (oculta) | Oculta |

#### Botões de Ação

**Botão 1: Editar (Azul)**
```html
<button class="btn btn-icon-28 btn-azul"
    data-bs-toggle="modal"
    data-bs-target="#modalAjustaCustos"
    data-id="{viagemId}"
    aria-label="Editar Dados">
    <i class="fa-duotone fa-pen-to-square"></i>
</button>
```
- Abre modal de ajuste de custos
- Passa ID da viagem via `data-id`

**Botão 2: Ver Ficha (Laranja)**
```html
<button class="btn btn-icon-28 btn-fundo-laranja"
    data-bs-toggle="modal"
    data-bs-target="#modalFicha"
    data-id="{viagemId}"
    aria-label="Ver Ficha de Vistoria">
    <i class="fa-duotone fa-file-image"></i>
</button>
```
- Abre modal de visualização de ficha
- Passa ID da viagem via `data-id`

#### Configuração de Linguagem
```javascript
language: {
    emptyTable: "Nenhum registro encontrado",
    info: "Mostrando de _START_ até _END_ de _TOTAL_ registros",
    paginate: { first, last, next, previous }
    // ... etc
}
```

#### DOM
```javascript
dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip'
```
- `l` = Seletor de linhas por página
- `f` = Campo de busca
- `r` = Processamento
- `t` = Tabela
- `i` = Info
- `p` = Paginação

---

## 📊 Fluxo de Dados

```
┌─────────────────────────────────────────────────────────┐
│ Página Carrega (document.ready)                          │
└──────────────┬──────────────────────────────────────────┘
               │
        ┌──────┴───────┐
        │              │
        ▼              ▼
   inicializar    initDataTable()
   Modais()        │
    │              ├─ GET /api/custosviagem
    │              ├─ Renderiza tabela
    │              └─ Mostra loading → esconde
    │
    └─ Listeners prontos
       para modal.show

┌──────────────────────────────────────────────────────────┐
│ Usuário clica em "Editar" na tabela                      │
└──────────────┬──────────────────────────────────────────┘
               │
               ▼
        Modal Ajusta Custos abre
        (evento show.bs.modal)
               │
               ├─ Pega data-id do botão
               │
               └─ carregarDadosViagem(id)
                  │
                  ├─ GET /api/Viagem/GetViagem/{id}
                  │
                  ├─ Popula campos direto
                  │
                  └─ setTimeout 300ms
                     └─ Popula combos Syncfusion

┌──────────────────────────────────────────────────────────┐
│ Usuário edita dados e clica "Ajustar Viagem"            │
└──────────────┬──────────────────────────────────────────┘
               │
               ▼
        gravarViagem()
        │
        ├─ Coleta dados do modal
        ├─ Mostra spinner no botão
        │
        └─ POST /api/Viagem/AtualizarDadosViagemDashboard
           │
           ├─ Sucesso
           │  ├─ Fecha modal
           │  ├─ Mostra loading
           │  └─ Recarrega tabela via DataTable.ajax.reload()
           │
           └─ Erro
              └─ Mostra toast vermelho

┌──────────────────────────────────────────────────────────┐
│ Usuário clica em "Ver Ficha" na tabela                   │
└──────────────┬──────────────────────────────────────────┘
               │
               ▼
        Modal Ficha abre
        (evento show.bs.modal)
               │
               ├─ Salva ID em #txtViagemId
               │
               └─ carregarFichaVistoria(id, button)
                  │
                  ├─ GET /api/Viagem/PegaFichaModal?id={id}
                  │
                  └─ Se válido: renderiza imagem base64
                     Se inválido: mostra placeholder
```

---

## 🔗 Integração com API

### Endpoints Utilizados

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| **GET** | `/api/Viagem/GetViagem/{id}` | Busca dados completos de uma viagem |
| **GET** | `/api/Viagem/PegaFichaModal` | Busca imagem da ficha (base64) |
| **POST** | `/api/Viagem/AtualizarDadosViagemDashboard` | Grava alterações |
| **GET** | `/api/custosviagem` | Lista viagens para tabela |

### Formato de Erro Padrão

Todas as chamadas AJAX esperam resposta:
```json
{
    "success": boolean,
    "message": "string descritivo",
    "data": object
}
```

---

## 🎨 Componentes UI

### Modais (Bootstrap 5)
- **`#modalAjustaCustos`**: Modal com formulário de ajuste
- **`#modalFicha`**: Modal com visualizador de imagem

### Campos do Modal Ajusta Custos
```html
<!-- ID e Ficha -->
<input type="hidden" id="txtId">
<input type="number" id="txtNoFichaVistoria">

<!-- Sincfusion Dropdowns -->
<select id="lstFinalidadeAlterada">
<select id="lstEvento">
<select id="lstMotoristaAlterado">
<select id="lstVeiculoAlterado">
<select id="lstRequisitanteAlterado">
<select id="lstSetorSolicitanteAlterado"> <!-- DropDownTree -->

<!-- Datas e Horas -->
<input type="date" id="txtDataInicial">
<input type="time" id="txtHoraInicial">
<input type="date" id="txtDataFinal">
<input type="time" id="txtHoraFinal">

<!-- Quilometragem -->
<input type="number" id="txtKmInicial">
<input type="number" id="txtKmFinal">

<!-- Ramal -->
<input type="text" id="txtRamalRequisitante">

<!-- Botão de ação -->
<button id="btnAjustarViagem">
    <span class="spinner-border d-none"></span>
    <span class="btn-text">Ajustar Viagem</span>
</button>
```

### Tabela
```html
<table id="tblViagem" class="table">
    <!-- Renderizado por DataTable -->
</table>
```

### Loading
```html
<div id="loadingOverlayCustos" style="display: none;">
    <span id="txtLoadingMessage">Carregando...</span>
</div>
```

---

## 🛡️ Tratamento de Erros

### Sistema de Alerta
Todo erro é capturado e tratado com:
```javascript
Alerta.TratamentoErroComLinha(
    "atualizacustosviagem.js",  // Nome do arquivo
    "nomeDaFuncao",              // Nome da função/seção
    error                        // Objeto de erro
);
```

### Erros por Seção

| Seção | Erros Capturados |
|-------|-----------------|
| `document.ready` | Falha na inicialização |
| `inicializarModais` | Erro ao configurar modais |
| `carregarDadosViagem` | Falha na API ou população de campos |
| `gravarViagem` | Falha na validação ou envio |
| `initDataTable` | Falha no carregamento da tabela |

### Toast de Feedback
- **Verde:** `AppToast.show("Verde", mensagem, 3000)` - Sucesso
- **Amarelo:** `AppToast.show("Amarelo", mensagem, 3000)` - Aviso
- **Vermelho:** `AppToast.show("Vermelho", mensagem, 4000)` - Erro

---

## 📝 Notas Técnicas

### 1. Delay de 300ms para Syncfusion
```javascript
setTimeout(function() {
    // Popula combos aqui
}, 300);
```
**Motivo:** Syncfusion precisa renderizar os componentes antes de receber dados. Sem delay, os valores podem não ser aplicados.

### 2. DropDownTree com Array
```javascript
lstSetor.ej2_instances[0].value = [setorSolicitanteId];  // Array!
```
**Motivo:** DropDownTree do Syncfusion espera um array, mesmo que com um único valor.

### 3. Evento com Array
```javascript
lstEvento.ej2_instances[0].value = [viagem.eventoId.toString()];
```
**Motivo:** MultiSelect do Syncfusion trabalha com array internamente.

### 4. Lógica de Finalidade
```javascript
if (viagem.finalidade === "Evento" && viagem.eventoId) {
    // Habilita e mostra dropdown de eventos
} else {
    // Desabilita e esconde
}
```
**Motivo:** O campo de evento é condicional. Só deve ser habilitado se finalidade for "Evento".

### 5. Reload da DataTable
```javascript
$("#tblViagem").DataTable().ajax.reload(callback, false);
```
- **Segundo parâmetro `false`:** Volta para primeira página (pode usar `true` para manter paginação)
- **Callback:** Executado após receber novos dados da API

### 6. Formatação de Números
```javascript
data.toLocaleString("pt-BR")  // 1000 → "1.000"
```
**Motivo:** Formatação brasileira com separador de milhar.

### 7. Tratamento de Null
```javascript
const valor = document.getElementById("campo").value || null;
```
**Motivo:** Converte string vazia em `null` para API.

### 8. Desabilitação de Botão
```javascript
btnAjustar.disabled = true;
```
Previne duplo clique durante envio de dados.

---

## 🔄 Ciclo de Vida

```
┌─────────────────────────────────────┐
│ 1. Page Load                         │
│    ├─ jQuery ready                  │
│    ├─ inicializarModais()           │
│    └─ initDataTable()               │
└──────────────┬──────────────────────┘

┌──────────────▼──────────────────────┐
│ 2. Tabela Exibida                   │
│    └─ Aguardando interação          │
└──────────────┬──────────────────────┘

┌──────────────▼──────────────────────┐
│ 3a. Clique em "Editar"              │
│     ├─ Modal abre                   │
│     ├─ carregarDadosViagem()        │
│     └─ Formulário populado          │
└──────────────┬──────────────────────┘

┌──────────────▼──────────────────────┐
│ 4. Edição de Dados                  │
│    └─ Usuário altera campos         │
└──────────────┬──────────────────────┘

┌──────────────▼──────────────────────┐
│ 5. Clique em "Ajustar Viagem"       │
│    ├─ gravarViagem()                │
│    ├─ Validação local               │
│    └─ POST /api/.../Atualizar       │
└──────────────┬──────────────────────┘

┌──────────────▼──────────────────────┐
│ 6. Resposta API                     │
│    ├─ Sucesso: Recarrega tabela     │
│    └─ Erro: Toast vermelho          │
└──────────────┬──────────────────────┘

┌──────────────▼──────────────────────┐
│ 7. Volta para Estado Inicial        │
│    └─ Pronto para nova edição       │
└─────────────────────────────────────┘
```

---

## ⚙️ Dependências Externas

| Biblioteca | Versão | Uso |
|-----------|--------|-----|
| **jQuery** | ? | AJAX, seletores DOM |
| **Bootstrap 5** | 5.x | Modal, classes utility |
| **Syncfusion** | ? | Dropdowns, DatePickers, NumericTextBox |
| **DataTable** | ? | Tabela interativa |
| **Font Awesome** | Duotone | Ícones (fa-duotone obrigatório) |
| **AppToast** | FrotiX | Toast de feedback |
| **Alerta** | FrotiX | Sistema de alertas |

---

## 📚 Arquivos Relacionados

| Arquivo | Responsabilidade |
|---------|-----------------|
| `alerta.js` | Sistema de alertas SweetAlert |
| `frotix.js` | Globais FrotiX (AppToast, FtxSpin) |
| `frotix.css` | Estilos globais |
| Controller API `ViagemController` | Endpoints `/api/Viagem/*` |
| Controller API `CustosViagemController` | Endpoint `/api/custosviagem` |

---

## 🧪 Exemplo de Uso

### Carregar e Editar Viagem

```javascript
// 1. Tabela carrega automaticamente ao abrir página
// 2. Usuário vê lista de viagens

// 3. Usuário clica no botão "Editar" (ícone lápis)
// Modal abre automaticamente:
// - Evento show.bs.modal dispara
// - carregarDadosViagem(viagemId) é chamada
// - Dados populam o formulário

// 4. Usuário altera dados (ex: motorista, km final, etc.)

// 5. Usuário clica "Ajustar Viagem"
// - gravarViagem() coleta dados
// - POST enviado para /api/Viagem/AtualizarDadosViagemDashboard
// - Modal fecha
// - Tabela recarrega com novos dados
// - Toast verde exibido
```

---

## 📌 Resumo de Responsabilidades

| Função | Responsabilidade |
|--------|-----------------|
| `inicializarModais()` | Setup de modais e listeners |
| `carregarDadosViagem()` | GET dados, popula formulário |
| `carregarFichaVistoria()` | GET imagem, exibe no modal |
| `gravarViagem()` | POST alterações para API |
| `initDataTable()` | Cria tabela com listagem |
| `mostrarLoading()` | Exibe overlay de carregamento |
| `esconderLoading()` | Oculta overlay |
| Helpers numéricos | Get/Set valores Syncfusion |
| `formatarDataParaInput()` | Converte formatos de data |

---

**Documentação gerada em:** 02/02/2026

**Status:** Completo com todas as funções documentadas
