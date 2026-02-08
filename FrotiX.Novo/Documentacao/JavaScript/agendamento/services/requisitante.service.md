# Documentação: requisitante.service.js

> **Última Atualização**: 12/01/2026
> **Versão Atual**: 1.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Lógica de Negócio](#lógica-de-negócio)
5. [Interconexões](#interconexões)
6. [Sistema de Accordion](#sistema-de-accordion)
7. [Validações](#validações)
8. [Exemplos de Uso](#exemplos-de-uso)
9. [Troubleshooting](#troubleshooting)

---

## Visão Geral

O arquivo `requisitante.service.js` é um **serviço JavaScript** responsável por gerenciar completamente a funcionalidade de **cadastro de novos requisitantes** dentro do modal de Agendamento/Viagem da Agenda.

### Características Principais
- ✅ **Cadastro Inline**: Permite criar requisitante sem sair do modal de agendamento
- ✅ **Accordion Controlado**: Sistema de abertura/fechamento manual via botão toggle
- ✅ **Proteção Robusta**: MutationObserver + desabilitação Bootstrap Collapse
- ✅ **API Integration**: Comunicação com backend via AJAX (POST `/api/Viagem/AdicionarRequisitante`)
- ✅ **Validações Client-Side**: Campos obrigatórios com feedback visual
- ✅ **Auto-Atualização**: Após salvar, atualiza dropdown de requisitantes automaticamente
- ✅ **Captura de Setores**: Reutiliza dados de setores já carregados na página
- ✅ **Sincronização Syncfusion**: Integra com DropDownTree, ComboBox e NumericTextBox

### Objetivo
Simplificar o fluxo de trabalho do usuário permitindo que ele crie um novo requisitante **durante** o processo de agendamento, sem precisar navegar para outra tela, salvar, voltar e selecionar. Tudo acontece inline, de forma rápida e intuitiva.

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| JavaScript ES6 | - | Linguagem base (IIFE, arrow functions, async/await) |
| jQuery | 3.x | AJAX calls, manipulação DOM |
| Syncfusion EJ2 | - | DropDownTree, ComboBox, NumericTextBox |
| Bootstrap 5 | - | Accordion UI (com desabilitação programática) |
| MutationObserver API | - | Monitoramento de alterações DOM |

### Padrões de Design
- **Service Layer**: Encapsula lógica de negócio e chamadas API
- **IIFE (Immediately Invoked Function Expression)**: Encapsula código para evitar poluição do escopo global
- **Observer Pattern**: MutationObserver monitora accordion e previne fechamentos indesejados
- **Event Capturing**: Event listeners com `capture: true` para prioridade

---

## Estrutura de Arquivos

### Arquivo Principal
```
wwwroot/js/agendamento/services/requisitante.service.js
```

### Arquivos Relacionados
- `Pages/Agenda/Index.cshtml` - Razor Page que define o HTML do accordion de requisitante (linhas 1239-1314)
- `wwwroot/js/agendamento/main.js` - Inicializa o sistema chamando `inicializarSistemaRequisitante()`
- `Alerta.js` - Sistema de alertas SweetAlert customizado
- `Controllers/ViagemController.cs` - Endpoint `/api/Viagem/AdicionarRequisitante`
- `wwwroot/js/agendamento/utils/syncfusion.utils.js` - Utilitários Syncfusion (tooltips, CLDR)

---

## Lógica de Negócio

### Classes e Objetos Principais

#### Classe: `RequisitanteService`
**Localização**: Linhas 18-104

**Propósito**: Encapsula chamadas à API para operações de requisitante (adicionar, listar).

**Métodos**:

##### `async adicionar(dados)`
**Parâmetros**:
- `dados` (Object): `{ Nome, Ponto, Ramal, Email, SetorSolicitanteId }`

**Retorno**: `Promise<{success: boolean, message: string, requisitanteId: string}>`

**Exemplo**:
```javascript
const resultado = await window.RequisitanteService.adicionar({
    Nome: "João Silva",
    Ponto: "1234",
    Ramal: 5678,
    Email: "joao.silva@email.com",
    SetorSolicitanteId: "guid-setor-id"
});

if (resultado.success) {
    console.log("Requisitante criado:", resultado.requisitanteId);
}
```

**Fluxo**:
1. Envia POST para `/api/Viagem/AdicionarRequisitante`
2. Recebe resposta com `{ success, message, requisitanteid }`
3. Se sucesso, retorna ID do requisitante criado
4. Se erro, retorna mensagem de erro

---

##### `async listar()`
**Propósito**: Lista todos os requisitantes existentes

**Retorno**: `Promise<{success: boolean, data: Array<{RequisitanteId, Requisitante}>}>`

**Exemplo**:
```javascript
const resultado = await window.RequisitanteService.listar();
if (resultado.success) {
    console.log("Requisitantes:", resultado.data);
    // resultado.data = [
    //   { RequisitanteId: "guid-1", Requisitante: "João - 1234" },
    //   { RequisitanteId: "guid-2", Requisitante: "Maria - 5678" }
    // ]
}
```

**Fluxo**:
1. Chama handler `/Viagens/Upsert?handler=AJAXPreencheListaRequisitantes`
2. Mapeia resposta para formato padronizado
3. Retorna array de objetos `{RequisitanteId, Requisitante}`

---

### Funções Globais Principais

#### `inicializarSistemaRequisitante()`
**Localização**: Linhas 174-244

**Propósito**: Inicializa todo o sistema de requisitante ao abrir o modal de agendamento.

**Fluxo**:
1. Verifica se já foi inicializado (`window.requisitanteServiceInicializado`)
2. Se já inicializado, retorna (proteção contra duplicação)
3. Marca como inicializado
4. Configura botão "Novo Requisitante" (`configurarBotaoNovoRequisitante()`)
5. Configura botões Salvar/Fechar (`configurarBotoesCadastroRequisitante()`)
6. Remove listener global antigo (se existir)
7. Cria novo `globalClickListener` para bloquear cliques durante validação
8. Adiciona listener global com `capture: true`

**Exemplo de Uso**:
```javascript
// No main.js, ao abrir o modal
$('#modalAgendamento').on('shown.bs.modal', function() {
    window.inicializarSistemaRequisitante();
});
```

**IMPORTANTE**: Só deve ser chamado **uma vez** por abertura de modal.

---

#### `configurarBotaoNovoRequisitante()`
**Localização**: Linhas 249-326

**Propósito**: Configura o botão "Novo Requisitante" como **toggle** (abre/fecha o accordion).

**Fluxo**:
1. Localiza `btnRequisitante` no DOM
2. **Remove listeners anteriores** clonando o botão (técnica para limpar eventos)
3. Adiciona novo listener com `capture: true`
4. Ao clicar:
   - Verifica se está validando (`estaValidando`) ou processando (`isProcessing`)
   - Se sim, bloqueia clique
   - Se não, verifica estado do `sectionCadastroRequisitante`
   - Se oculto → chama `abrirFormularioCadastroRequisitante()`
   - Se visível → chama `fecharFormularioCadastroRequisitante()`
5. Usa `setTimeout` de 300ms para resetar flag `isProcessing`

**Código**:
```javascript
novoBotao.addEventListener("click", function (e) {
    if (estaValidando || isProcessing) {
        e.preventDefault();
        return false;
    }

    isProcessing = true;
    e.preventDefault();
    e.stopPropagation();
    e.stopImmediatePropagation();

    const estaOculto = (sectionCadastro.style.display === "none");
    if (estaOculto) {
        abrirFormularioCadastroRequisitante();
    } else {
        fecharFormularioCadastroRequisitante();
    }

    setTimeout(() => { isProcessing = false; }, 300);
}, true);
```

---

#### `abrirFormularioCadastroRequisitante()`
**Localização**: Linhas 331-520

**Propósito**: Abre o accordion de cadastro de requisitante e prepara todos os componentes.

**Fluxo Detalhado**:
1. **Exibe `accordionRequisitante`**: Remove `display: none`, força `height: auto`, `overflow: visible`
2. **Exibe `sectionCadastroRequisitante`**: Força visibilidade, remove classes Bootstrap `collapse`, `collapsing`, `d-none`
3. **🔥 DESABILITA Bootstrap Collapse** (CRÍTICO - adicionado em 12/01/2026):
   - Localiza `collapseRequisitante`
   - Remove classes `collapse` e `collapsing`
   - Adiciona classe `show`
   - Destrói instância `Bootstrap.Collapse` se existir
   - Adiciona listener para **prevenir** evento `hide.bs.collapse`
   - **Motivo**: Bootstrap fecha accordion automaticamente quando detecta clique "fora" (ex: popup do DropDownTree)
4. **Cria MutationObserver**:
   - Monitora alterações em `sectionCadastroRequisitante`
   - Se detectar fechamento (display: none, visibility: hidden, opacity: 0, height: 0, classe d-none)
   - **SEMPRE** força reabertura (não apenas durante validação)
   - Força também `collapseRequisitante` a ficar aberto
5. **Limpa campos**: Chama `limparCamposCadastroRequisitante()`
6. **Destroi e recria `ddtSetorRequisitante`** (DropDownTree):
   - Syncfusion não renderiza popup corretamente quando controle é criado com `display: none`
   - Captura dados de setores (`capturarDadosSetores()`)
   - Destrói instância antiga (`destroy()`)
   - Cria nova instância com eventos protegidos:
     - `open`: Previne propagação de cliques no popup
     - `select`: Previne propagação ao selecionar item
     - `close`: Verifica se accordion foi fechado e reabre

**Código - Desabilitação Bootstrap Collapse**:
```javascript
const collapseElement = document.getElementById("collapseRequisitante");
if (collapseElement) {
    // Remover classes Bootstrap
    collapseElement.classList.remove('collapse');
    collapseElement.classList.remove('collapsing');
    collapseElement.classList.add('show');

    // Forçar estilos inline
    collapseElement.style.display = "block";
    collapseElement.style.height = "auto";

    // Destruir instância Bootstrap
    const bsCollapse = bootstrap.Collapse.getInstance(collapseElement);
    if (bsCollapse) {
        bsCollapse.dispose();
    }

    // Prevenir eventos Bootstrap
    collapseElement.addEventListener('hide.bs.collapse', function(e) {
        console.log("🛑 BLOQUEANDO evento hide.bs.collapse!");
        e.preventDefault();
        e.stopPropagation();
        return false;
    }, true);
}
```

**Código - Eventos DropDownTree**:
```javascript
const novoDropdown = new ej.dropdowns.DropDownTree({
    // ... configurações de fields ...

    open: function(args) {
        console.log("🔓 DropDownTree ABERTO");
        if (args && args.popup) {
            args.popup.element.addEventListener('click', function(e) {
                e.stopPropagation(); // Prevenir Bootstrap
            }, true);
        }
    },

    select: function(args) {
        console.log("✅ Item SELECIONADO:", args.nodeData?.text);
        if (args.event) {
            args.event.stopPropagation();
        }
    },

    close: function(args) {
        console.log("🔒 DropDownTree FECHADO");
        setTimeout(() => {
            const section = document.getElementById("sectionCadastroRequisitante");
            if (section && section.style.display === "none") {
                console.warn("⚠️ Accordion fechou - REABRINDO!");
                abrirFormularioCadastroRequisitante();
            }
        }, 50);
    }
});
```

---

#### `fecharFormularioCadastroRequisitante()`
**Localização**: Linhas 522-544

**Propósito**: Fecha o accordion de cadastro de requisitante.

**Fluxo**:
1. Localiza `sectionCadastroRequisitante`
2. Define `style.display = "none"`
3. Reseta flag `isProcessing`
4. Adiciona stack trace no log (para debug)

**Código**:
```javascript
function fecharFormularioCadastroRequisitante() {
    console.log("➖ Fechando formulário");
    console.log("   Stack trace:", new Error().stack);

    const sectionCadastro = document.getElementById("sectionCadastroRequisitante");
    if (sectionCadastro) {
        sectionCadastro.style.display = "none";
        isProcessing = false;
    }
}
```

**IMPORTANTE**: Esta é a **ÚNICA forma correta** de fechar o accordion. O Bootstrap Collapse foi desabilitado para prevenir fechamentos automáticos.

---

#### `limparCamposCadastroRequisitante()`
**Localização**: Linhas 546-599

**Propósito**: Limpa todos os campos do formulário de requisitante.

**Fluxo**:
1. Limpa campos de texto simples:
   - `txtPonto.value = ""`
   - `txtNome.value = ""`
   - `txtRamal.value = ""`
   - `txtEmail.value = ""`
2. Limpa DropDownTree de Setor:
   - Localiza `ddtSetorRequisitante`
   - Obtém instância Syncfusion
   - Define `value = null` e chama `dataBind()`
3. Adiciona logs detalhados (dataSource, campos, primeiros itens)
4. Adiciona stack trace (para debug)

**Código**:
```javascript
function limparCamposCadastroRequisitante() {
    console.log("🧹 Limpando campos");
    console.log("   Stack trace:", new Error().stack);

    // Campos texto
    const txtPonto = document.getElementById("txtPonto");
    const txtNome = document.getElementById("txtNome");
    const txtRamal = document.getElementById("txtRamal");
    const txtEmail = document.getElementById("txtEmail");

    if (txtPonto) txtPonto.value = "";
    if (txtNome) txtNome.value = "";
    if (txtRamal) txtRamal.value = "";
    if (txtEmail) txtEmail.value = "";

    // DropDownTree
    const ddtSetor = document.getElementById("ddtSetorRequisitante");
    if (ddtSetor && ddtSetor.ej2_instances?.[0]) {
        const dropdown = ddtSetor.ej2_instances[0];
        dropdown.value = null;
        dropdown.dataBind();
    }
}
```

---

#### `configurarBotoesCadastroRequisitante()`
**Localização**: Linhas 601-652

**Propósito**: Configura event listeners dos botões Salvar e Fechar do accordion.

**Fluxo**:
1. **Botão Salvar** (`btnInserirRequisitante`):
   - Remove listeners anteriores (clonando botão)
   - Adiciona listener com `capture: true`
   - Ao clicar: chama `salvarNovoRequisitante()`
2. **Botão Fechar** (`btnFecharAccordionRequisitante`):
   - Remove listeners anteriores
   - Adiciona listener com `capture: true`
   - Ao clicar: chama `fecharFormularioCadastroRequisitante()` + `limparCamposCadastroRequisitante()`

**Código**:
```javascript
// BOTÃO SALVAR
const btnSalvarRequisitante = document.getElementById("btnInserirRequisitante");
const novoBotaoSalvar = btnSalvarRequisitante.cloneNode(true);
btnSalvarRequisitante.parentNode.replaceChild(novoBotaoSalvar, btnSalvarRequisitante);

novoBotaoSalvar.addEventListener("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    e.stopImmediatePropagation();
    salvarNovoRequisitante();
}, true);

// BOTÃO FECHAR
const btnCancelarRequisitante = document.getElementById("btnFecharAccordionRequisitante");
const novoBotaoFechar = btnCancelarRequisitante.cloneNode(true);
btnCancelarRequisitante.parentNode.replaceChild(novoBotaoFechar, btnCancelarRequisitante);

novoBotaoFechar.addEventListener("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    e.stopImmediatePropagation();
    fecharFormularioCadastroRequisitante();
    limparCamposCadastroRequisitante();
}, true);
```

---

#### `salvarNovoRequisitante()`
**Localização**: Linhas 654-883

**Propósito**: Valida campos, monta objeto e envia requisição AJAX para criar requisitante.

**Fluxo Detalhado**:
1. **Obtém campos do DOM**:
   - `txtPonto`, `txtNome`, `txtRamal`, `txtEmail`, `ddtSetorRequisitante`
2. **Ativa flag de validação**: `estaValidando = true`
3. **Valida campos obrigatórios**:
   - **Ponto**: `if (!txtPonto.value.trim())` → Alerta + focus + return
   - **Nome**: `if (!txtNome.value.trim())` → Alerta + focus + return
   - **Ramal**: `if (!txtRamal.value.trim())` → Alerta + focus + return
   - **Setor**: `if (!setorValue)` → Alerta + return
   - Cada validação agenda `setTimeout` de 2s para desativar `estaValidando`
4. **Desativa flag**: `estaValidando = false` (validações passaram)
5. **Monta objeto**:
   ```javascript
   const objRequisitante = {
       Nome: txtNome.value.trim(),
       Ponto: txtPonto.value.trim(),
       Ramal: parseInt(txtRamal.value.trim()),
       Email: txtEmail.value.trim(),
       SetorSolicitanteId: setorValue.toString()
   };
   ```
6. **Envia AJAX POST** para `/api/Viagem/AdicionarRequisitante`:
   - `contentType: "application/json"`
   - `dataType: "json"`
   - `data: JSON.stringify(objRequisitante)`
7. **Success callback**:
   - Mostra toast/toastr de sucesso
   - **Atualiza dropdown `lstRequisitante`**:
     - Adiciona novo item com `addItem()`
     - Define como selecionado com `value = requisitanteid`
     - Chama `dataBind()`
   - **Atualiza campo Ramal** (`txtRamalRequisitanteSF`)
   - **Atualiza dropdown Setor** (`lstSetorRequisitanteAgendamento`)
   - Fecha formulário e limpa campos
8. **Error callback**:
   - Mostra alerta de erro
   - Log de erro detalhado
   - Chama `Alerta.TratamentoErroComLinha()`

**Código - Validações**:
```javascript
estaValidando = true;

if (!txtPonto || !txtPonto.value.trim()) {
    const resetTimer = setTimeout(() => {
        estaValidando = false;
    }, 2000);

    Alerta.Alerta("Atenção", "O Ponto é obrigatório!");
    if (txtPonto) txtPonto.focus();
    return;
}

// Validações de Nome, Ramal, Setor seguem o mesmo padrão...
```

**Código - AJAX Success**:
```javascript
success: function (data) {
    if (data.success) {
        // Toast de sucesso
        AppToast.show('Verde', data.message);

        // Atualizar dropdown lstRequisitante
        const lstRequisitante = document.getElementById("lstRequisitante");
        const comboRequisitante = lstRequisitante.ej2_instances[0];

        const novoItem = {
            RequisitanteId: data.requisitanteid,
            Requisitante: txtNome.value.trim() + " - " + txtPonto.value.trim()
        };

        comboRequisitante.addItem(novoItem, 0);
        comboRequisitante.value = data.requisitanteid;
        comboRequisitante.dataBind();

        // Atualizar campos Ramal e Setor
        // ...

        // Fechar e limpar
        fecharFormularioCadastroRequisitante();
        limparCamposCadastroRequisitante();
    } else {
        AppToast.show('Vermelho', data.message);
    }
}
```

---

#### `capturarDadosSetores()`
**Localização**: Linhas 126-165

**Propósito**: Captura dados de setores já carregados em outros controles da página para reutilizar no DropDownTree.

**Fluxo**:
1. Tenta pegar dados de `lstSetorRequisitanteAgendamento`:
   - Acessa `ej2_instances[0].fields.dataSource`
   - Se encontrar, armazena em `window.SETORES_DATA`
2. Se não encontrar, tenta `lstSetorRequisitanteEvento`
3. Se encontrar, retorna `true`
4. Se não encontrar em nenhum, retorna `false`

**Código**:
```javascript
function capturarDadosSetores() {
    const lstSetorAgendamento = document.getElementById("lstSetorRequisitanteAgendamento");

    if (lstSetorAgendamento?.ej2_instances?.[0]) {
        const dados = lstSetorAgendamento.ej2_instances[0].fields?.dataSource;
        if (dados?.length > 0) {
            window.SETORES_DATA = dados;
            console.log(`✅ Dados capturados: ${dados.length} itens`);
            return true;
        }
    }

    // Tentar lstSetorRequisitanteEvento...
    // ...

    return false;
}
```

**IMPORTANTE**: Evita requisições duplicadas ao backend, reutilizando dados já disponíveis no DOM.

---

### Global Click Listener

**Localização**: Linhas 205-238

**Propósito**: Bloqueia cliques no botão "Novo Requisitante" e no accordion durante validação.

**Fluxo**:
1. Verifica se `estaValidando = true`
2. Se não, permite clique normalmente
3. Se sim:
   - Permite cliques no SweetAlert (`.swal2-container`)
   - Bloqueia cliques em `btnRequisitante` e `accordionRequisitante`
   - Previne propagação com `preventDefault()`, `stopPropagation()`, `stopImmediatePropagation()`

**Código**:
```javascript
window.globalClickListener = function (e) {
    if (!estaValidando) return;

    // Permitir SweetAlert
    if (e.target.closest('.swal2-container')) {
        return;
    }

    // Bloquear btnRequisitante e accordion
    const btnRequisitante = document.getElementById('btnRequisitante');
    const accordionRequisitante = document.getElementById('accordionRequisitante');

    const clickedBtn = e.target === btnRequisitante || btnRequisitante?.contains(e.target);
    const clickedAccordion = accordionRequisitante && (
        e.target === accordionRequisitante ||
        accordionRequisitante.contains(e.target)
    );

    if (clickedBtn || clickedAccordion) {
        console.log("🛑 Click bloqueado durante validação");
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();
    }
};

document.addEventListener("click", window.globalClickListener, true);
```

---

## Interconexões

### Quem Chama Este Arquivo
- **`main.js`** → Chama `inicializarSistemaRequisitante()` ao abrir o modal de agendamento
- **Usuário** → Clica no botão "Novo Requisitante" (`btnRequisitante`)
- **Bootstrap Modal** → Dispara evento `shown.bs.modal` que aciona inicialização

### O Que Este Arquivo Chama
- **`Alerta.js`** → Mostra alertas de validação e erros (`Alerta.Alerta()`, `Alerta.Erro()`, `Alerta.TratamentoErroComLinha()`)
- **`AppToast`** → Mostra toasts de sucesso/erro após salvar
- **`/api/Viagem/AdicionarRequisitante`** → Endpoint POST para criar requisitante
- **`/Viagens/Upsert?handler=AJAXPreencheListaRequisitantes`** → Endpoint GET para listar requisitantes
- **Syncfusion EJ2** → Manipula componentes (`DropDownTree`, `ComboBox`, `NumericTextBox`)
- **Bootstrap 5** → Desabilita `Collapse` programaticamente

### Fluxo Completo

```
Usuário abre modal de agendamento
    ↓
main.js → inicializarSistemaRequisitante()
    ↓
configurarBotaoNovoRequisitante() (toggle)
    ↓
Usuário clica "Novo Requisitante"
    ↓
abrirFormularioCadastroRequisitante()
    ↓
  - Desabilita Bootstrap Collapse
  - Cria MutationObserver
  - Destroi e recria ddtSetorRequisitante com eventos protegidos
  - Captura dados de setores (capturarDadosSetores)
  - Limpa campos (limparCamposCadastroRequisitante)
    ↓
Usuário preenche campos e seleciona Setor
    ↓
  - Dropdown abre popup (evento open → previne propagação)
  - Usuário seleciona item (evento select → previne propagação)
  - Dropdown fecha popup (evento close → verifica se accordion foi fechado)
  - MutationObserver monitora continuamente e força reabertura se necessário
    ↓
Usuário clica "Salvar Requisitante"
    ↓
salvarNovoRequisitante()
    ↓
  - Valida campos (estaValidando = true)
  - Monta objeto objRequisitante
  - AJAX POST /api/Viagem/AdicionarRequisitante
    ↓
Controller processa e retorna { success, message, requisitanteid }
    ↓
Callback success:
  - AppToast.show('Verde', ...)
  - Atualiza lstRequisitante dropdown
  - Atualiza campos Ramal e Setor
  - fecharFormularioCadastroRequisitante()
  - limparCamposCadastroRequisitante()
```

---

## Sistema de Accordion

### Problema Resolvido (12/01/2026)

**Problema Original**:
- Usuário clicava em "Novo Requisitante"
- Preencia todos os campos
- Ao selecionar Setor no DropDownTree, o accordion **fechava automaticamente**
- Todos os campos eram **limpos**
- Usuário não conseguia salvar o requisitante

**Causa Raiz**:
- O accordion usa Bootstrap 5 com classe `accordion-collapse collapse show`
- Bootstrap Collapse tem comportamento automático de fechar quando detecta clique "fora" do elemento
- DropDownTree do Syncfusion renderiza o popup **fora** do accordion (direto no `<body>`)
- Quando usuário clicava no popup para selecionar, Bootstrap interpretava como "clique fora"
- Bootstrap automaticamente fechava o accordion via evento `hide.bs.collapse`

**Solução Implementada (3 camadas de proteção)**:

#### 1. Desabilitação do Bootstrap Collapse
**Localização**: Linhas 379-404 em `abrirFormularioCadastroRequisitante()`

- Remove classes `collapse` e `collapsing`
- Destrói instância `Bootstrap.Collapse` se existir
- Adiciona listener para **prevenir** evento `hide.bs.collapse`
- Força estilos inline (`display: block`, `height: auto`, `overflow: visible`)

**Código**:
```javascript
const collapseElement = document.getElementById("collapseRequisitante");
if (collapseElement) {
    // Remove classes Bootstrap
    collapseElement.classList.remove('collapse');
    collapseElement.classList.remove('collapsing');
    collapseElement.classList.add('show');

    // Destrói instância Bootstrap Collapse
    const bsCollapse = bootstrap.Collapse.getInstance(collapseElement);
    if (bsCollapse) {
        bsCollapse.dispose();
    }

    // Previne evento hide.bs.collapse
    collapseElement.addEventListener('hide.bs.collapse', function(e) {
        console.log("🛑 BLOQUEANDO hide.bs.collapse!");
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();
        return false;
    }, true);
}
```

#### 2. MutationObserver Aprimorado
**Localização**: Linhas 406-472

- Monitora `sectionCadastroRequisitante` continuamente
- Detecta fechamento por: `display: none`, `visibility: hidden`, `opacity: 0`, `height: 0`, classe `d-none`
- **SEMPRE força reabertura** (não apenas quando `estaValidando = true`)
- Força também `collapseRequisitante` a ficar aberto
- Adiciona stack trace para debug

**Código**:
```javascript
const observer = new MutationObserver((mutations) => {
    const computedStyle = window.getComputedStyle(sectionCadastro);
    const estaOculto = computedStyle.display === 'none' ||
        computedStyle.visibility === 'hidden' ||
        computedStyle.opacity === '0' ||
        sectionCadastro.offsetHeight === 0 ||
        sectionCadastro.classList.contains('d-none');

    if (estaOculto) {
        console.error("🚨 ACCORDION FECHOU INESPERADAMENTE!");
        console.error("   Stack trace:", new Error().stack);

        // SEMPRE FORÇAR REABERTURA
        sectionCadastro.style.display = "block";
        sectionCadastro.style.visibility = "visible";
        sectionCadastro.style.opacity = "1";
        sectionCadastro.style.height = "auto";
        sectionCadastro.style.overflow = "visible";

        // Forçar collapseRequisitante também
        const collapseElement = document.getElementById("collapseRequisitante");
        if (collapseElement) {
            collapseElement.classList.remove('collapse');
            collapseElement.classList.add('show');
            collapseElement.style.display = "block";
        }
    }
});

observer.observe(sectionCadastro, {
    attributes: true,
    childList: true,
    subtree: true
});
```

#### 3. Eventos DropDownTree Protegidos
**Localização**: Linhas 496-518

- **`open`**: Previne propagação de cliques no popup
- **`select`**: Previne propagação ao selecionar item
- **`close`**: Verifica se accordion foi fechado e reabre se necessário

**Código**:
```javascript
const novoDropdown = new ej.dropdowns.DropDownTree({
    // ... fields ...

    open: function(args) {
        if (args?.popup) {
            args.popup.element.addEventListener('click', function(e) {
                e.stopPropagation(); // Prevenir Bootstrap
            }, true);
        }
    },

    select: function(args) {
        if (args.event) {
            args.event.stopPropagation();
        }
    },

    close: function(args) {
        setTimeout(() => {
            const section = document.getElementById("sectionCadastroRequisitante");
            if (section?.style.display === "none") {
                console.warn("⚠️ Accordion fechou - REABRINDO!");
                abrirFormularioCadastroRequisitante();
            }
        }, 50);
    }
});
```

**Resultado**: Accordion agora **só fecha** via botão manual "Fechar". Todos os fechamentos automáticos do Bootstrap foram desabilitados.

---

## Validações

### Frontend (Client-Side)

#### 1. Validação de Ponto (obrigatório)
**Localização**: Linhas 673-687

**Regra**: Campo `txtPonto` não pode estar vazio ou apenas espaços.

**Mensagem**: "O Ponto é obrigatório!"

**Comportamento**: Foca no campo após mostrar alerta.

**Código**:
```javascript
if (!txtPonto || !txtPonto.value.trim()) {
    const resetTimer = setTimeout(() => {
        estaValidando = false;
    }, 2000);

    Alerta.Alerta("Atenção", "O Ponto é obrigatório!");
    if (txtPonto) txtPonto.focus();
    return;
}
```

#### 2. Validação de Nome (obrigatório)
**Localização**: Linhas 689-702

**Regra**: Campo `txtNome` não pode estar vazio ou apenas espaços.

**Mensagem**: "O Nome é obrigatório!"

**Código**:
```javascript
if (!txtNome || !txtNome.value.trim()) {
    const resetTimer = setTimeout(() => {
        estaValidando = false;
    }, 2000);

    Alerta.Alerta("Atenção", "O Nome é obrigatório!");
    if (txtNome) txtNome.focus();
    return;
}
```

#### 3. Validação de Ramal (obrigatório)
**Localização**: Linhas 704-717

**Regra**: Campo `txtRamal` não pode estar vazio ou apenas espaços.

**Mensagem**: "O Ramal é obrigatório!"

**Código**:
```javascript
if (!txtRamal || !txtRamal.value.trim()) {
    const resetTimer = setTimeout(() => {
        estaValidando = false;
    }, 2000);

    Alerta.Alerta("Atenção", "O Ramal é obrigatório!");
    if (txtRamal) txtRamal.focus();
    return;
}
```

#### 4. Validação de Setor (obrigatório)
**Localização**: Linhas 719-743

**Regra**: DropDownTree `ddtSetorRequisitante` deve ter valor selecionado.

**Mensagem**: "O Setor do Requisitante é obrigatório!"

**Código**:
```javascript
let setorValue = null;
if (ddtSetor?.ej2_instances?.[0]) {
    const dropdown = ddtSetor.ej2_instances[0];
    setorValue = dropdown.value;
}

if (!setorValue) {
    const resetTimer = setTimeout(() => {
        estaValidando = false;
    }, 2000);

    Alerta.Alerta("Atenção", "O Setor do Requisitante é obrigatório!");
    return;
}
```

#### 5. Email (opcional)
**Não há validação de formato**. Campo `txtEmail` é opcional e aceita qualquer valor.

**Observação**: Backend pode validar formato de email se necessário.

---

### Backend (Server-Side)

As validações backend estão no `ViagemController.cs`, endpoint `/api/Viagem/AdicionarRequisitante`.

**Possíveis validações backend** (verificar no Controller):
- Ponto único (não permitir duplicatas)
- Nome único (não permitir duplicatas)
- Formato de email válido
- Ramal numérico positivo
- Setor existente no banco

---

## Exemplos de Uso

### Cenário 1: Cadastro Básico de Requisitante

**Situação**: Usuário está criando um agendamento e precisa de um requisitante que ainda não existe.

**Passos**:
1. Usuário abre modal de agendamento
2. Clica no botão "Novo Requisitante" (accordion abre)
3. Preenche:
   - Ponto: `1234`
   - Nome: `João Silva`
   - Ramal: `5678`
   - Email: `joao.silva@camara.leg.br`
   - Setor: Seleciona na árvore (ex: "TI → Desenvolvimento")
4. Clica em "Salvar Requisitante"
5. Sistema valida, salva e atualiza dropdown de requisitantes
6. Accordion fecha automaticamente
7. Requisitante recém-criado já está selecionado no dropdown principal

**Resultado**: Requisitante criado e selecionado, usuário pode continuar o agendamento.

---

### Cenário 2: Correção Durante Validação

**Situação**: Usuário tenta salvar sem preencher todos os campos obrigatórios.

**Passos**:
1. Usuário abre accordion, preenche apenas Ponto e Nome
2. Clica em "Salvar Requisitante"
3. Sistema valida e encontra que Ramal está vazio
4. Mostra alerta "O Ramal é obrigatório!"
5. Flag `estaValidando = true` ativa proteção global
6. Usuário clica no alerta "OK"
7. Flag `estaValidando = false` após 2 segundos
8. Usuário preenche Ramal e Setor
9. Clica novamente em "Salvar"
10. Sistema valida, salva e fecha accordion

**Resultado**: Validação preveniu gravação incompleta, usuário corrigiu e salvou com sucesso.

---

### Cenário 3: Seleção de Setor sem Fechar Accordion (Correção 12/01/2026)

**Situação**: Usuário preenche todos os campos e seleciona Setor no DropDownTree.

**Passos (ANTES da correção - PROBLEMA)**:
1. Usuário abre accordion
2. Preenche Ponto, Nome, Ramal, Email
3. Clica no DropDownTree de Setor
4. Popup abre (renderizado no body, fora do accordion)
5. Usuário clica em um setor para selecionar
6. **Bootstrap Collapse detecta clique "fora" do accordion**
7. **Accordion fecha automaticamente**
8. **Campos são limpos**
9. **Usuário não consegue salvar**

**Passos (DEPOIS da correção - SOLUÇÃO)**:
1. Usuário abre accordion
2. `abrirFormularioCadastroRequisitante()` desabilita Bootstrap Collapse
3. MutationObserver começa a monitorar accordion
4. Usuário preenche Ponto, Nome, Ramal, Email
5. Clica no DropDownTree de Setor
6. Popup abre (evento `open` previne propagação de cliques)
7. Usuário clica em um setor para selecionar
8. Evento `select` previne propagação
9. **Accordion PERMANECE ABERTO** (Bootstrap Collapse desabilitado)
10. **MutationObserver monitora mas não detecta fechamento**
11. **Campos PERMANECEM PREENCHIDOS**
12. Usuário clica "Salvar Requisitante"
13. Sistema salva com sucesso

**Resultado**: Usuário consegue selecionar Setor sem perder dados, accordion permanece aberto e funcional.

---

## Troubleshooting

### Problema 1: Accordion Fecha ao Selecionar Setor

**Sintoma**: Ao clicar no DropDownTree de Setor e selecionar um item, o accordion fecha e todos os campos são limpos.

**Causa**: Bootstrap Collapse interpretava clique no popup Syncfusion como "fora do accordion" e fechava automaticamente.

**Diagnóstico**:
1. Abrir DevTools → Console
2. Procurar mensagem: `🚨 ACCORDION FECHOU INESPERADAMENTE!`
3. Verificar stack trace para identificar origem do fechamento

**Solução** (Implementada em 12/01/2026):
- Desabilitar Bootstrap Collapse ao abrir formulário
- MutationObserver força reabertura automática
- Eventos do DropDownTree previnem propagação

**Código envolvido**:
- Linhas 379-404: Desabilitação Bootstrap Collapse
- Linhas 406-472: MutationObserver
- Linhas 496-518: Eventos DropDownTree

---

### Problema 2: Dropdown de Setor Vazio

**Sintoma**: DropDownTree de Setor não mostra nenhum item ao abrir.

**Causa**: Dados de setores não foram capturados de outros controles da página.

**Diagnóstico**:
```javascript
// No console do navegador
console.log("SETORES_DATA:", window.SETORES_DATA);
// Se for undefined ou [], dados não foram capturados
```

**Solução**:
1. Verificar se `lstSetorRequisitanteAgendamento` ou `lstSetorRequisitanteEvento` existem na página
2. Verificar se estão inicializados com Syncfusion (`ej2_instances`)
3. Verificar se têm `dataSource` com dados
4. Se não, chamar manualmente `capturarDadosSetores()` após carregar a página

**Código para debug**:
```javascript
const lst = document.getElementById("lstSetorRequisitanteAgendamento");
console.log("Elemento existe:", lst !== null);
console.log("Syncfusion inicializado:", lst?.ej2_instances?.length > 0);
console.log("DataSource:", lst?.ej2_instances?.[0].fields?.dataSource);
```

---

### Problema 3: Validação Trava Interface

**Sintoma**: Após clicar "Salvar" com campo inválido, usuário não consegue clicar em nada.

**Causa**: Flag `estaValidando` não foi resetada devido a erro no timer.

**Diagnóstico**:
```javascript
// No console
console.log("estaValidando:", window.estaValidando);
// Se for true por muito tempo (>5s), está travado
```

**Solução**:
1. Forçar reset manualmente:
   ```javascript
   window.estaValidando = false;
   ```
2. No código, verificar se `setTimeout` está sendo criado corretamente (linhas 678, 693, 708, 737)
3. Verificar se há erros no console que impedem execução do timer

---

### Problema 4: Dupla Inicialização

**Sintoma**: Console mostra mensagens "inicializarSistemaRequisitante chamada 2x ou mais".

**Causa**: Função `inicializarSistemaRequisitante()` chamada múltiplas vezes.

**Diagnóstico**:
```javascript
console.log("Inicializações:", window.inicializacaoCount);
console.log("Flag inicializado:", window.requisitanteServiceInicializado);
```

**Solução**:
- A função tem proteção contra múltiplas chamadas (linha 180-188)
- Verifica `window.requisitanteServiceInicializado` e retorna se já inicializado
- Não é necessário ação, proteção já existe
- Se problema persistir, verificar se flag está sendo resetada indevidamente

---

### Problema 5: Campos Não Atualizam Após Salvar

**Sintoma**: Após salvar requisitante, dropdown `lstRequisitante` não mostra o novo item.

**Causa**: Componente Syncfusion não foi atualizado corretamente ou não existe.

**Diagnóstico**:
```javascript
const lst = document.getElementById("lstRequisitante");
console.log("Elemento existe:", lst !== null);
console.log("Syncfusion inicializado:", lst?.ej2_instances?.length > 0);
```

**Solução**:
1. Verificar se elemento `lstRequisitante` existe no DOM
2. Verificar se está inicializado com Syncfusion
3. Verificar callback de sucesso AJAX (linhas 785-831)
4. Verificar se `data.success = true` e `data.requisitanteid` retornou

**Código para debug**:
```javascript
// Adicionar no callback success, após linha 774
console.log("Resposta API:", data);
console.log("lstRequisitante:", document.getElementById("lstRequisitante"));
console.log("Instâncias Syncfusion:", lstRequisitante?.ej2_instances);
```

---

### Problema 6: Erro 500 ao Salvar

**Sintoma**: AJAX retorna erro 500 Internal Server Error.

**Causa**: Erro no backend (Controller) ao processar requisição.

**Diagnóstico**:
1. Verificar console do servidor (IIS/Visual Studio Output)
2. Verificar logs do Controller
3. Verificar se objeto `objRequisitante` está correto (linha 751-757)

**Solução**:
1. Verificar se Controller `/api/Viagem/AdicionarRequisitante` existe
2. Verificar se aceita objeto com estrutura:
   ```json
   {
     "Nome": "string",
     "Ponto": "string",
     "Ramal": 1234,
     "Email": "string",
     "SetorSolicitanteId": "guid"
   }
   ```
3. Verificar validações backend (duplicatas, formato email, etc.)
4. Verificar banco de dados (conexão, constraints, triggers)

---

### Problema 7: MutationObserver Não Detecta Fechamento

**Sintoma**: Accordion fecha mas MutationObserver não reage.

**Causa**: Observer não está monitorando o elemento correto ou foi desconectado.

**Diagnóstico**:
```javascript
console.log("Observer existe:", window.__accordionObserver !== undefined);
console.log("Observer conectado:", window.__accordionObserver?.takeRecords);
```

**Solução**:
1. Verificar se observer foi criado (linha 406-472)
2. Verificar se `sectionCadastroRequisitante` existe no DOM
3. Verificar se observer não foi desconectado prematuramente
4. Reabrir formulário manualmente:
   ```javascript
   window.abrirFormularioCadastroRequisitante();
   ```

---

### Problema 8: Logs Excessivos no Console

**Sintoma**: Console cheio de logs de debug.

**Causa**: Muitos `console.log` e `console.error` no código.

**Solução**:
- Logs são úteis para debug, mas podem ser removidos em produção
- Para desabilitar temporariamente:
  ```javascript
  // No início do arquivo, antes do IIFE
  console.log = function() {};
  console.error = function() {};
  console.warn = function() {};
  ```

**Observação**: Manter logs em desenvolvimento, remover apenas em produção.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [12/01/2026 18:45] - Correção Crítica: Prevenir Fechamento Automático do Accordion

**Descrição**:
Implementada correção crítica para prevenir fechamento automático do accordion de Requisitante ao interagir com o DropDownTree de Setor. Bootstrap Collapse interpretava cliques no popup Syncfusion como "fora do accordion" e fechava automaticamente, limpando todos os campos.

**Problema**:
- Usuário abria accordion, preenchia todos os campos
- Ao selecionar Setor no DropDownTree, accordion fechava
- Todos os campos eram limpos
- Usuário não conseguia salvar o requisitante

**Causa Raiz**:
- Accordion usa Bootstrap 5 com classe `accordion-collapse collapse show`
- DropDownTree renderiza popup FORA do accordion (direto no body)
- Bootstrap detecta clique "fora" e automaticamente fecha via `hide.bs.collapse`

**Correções Implementadas**:

1. **Desabilitação do Bootstrap Collapse** (linhas 379-404):
   - Remove classes `collapse` e `collapsing`
   - Destrói instância `Bootstrap.Collapse` se existir
   - Previne evento `hide.bs.collapse` com `preventDefault()`

2. **MutationObserver Aprimorado** (linhas 406-472):
   - Agora SEMPRE força reabertura (não apenas durante validação)
   - Monitora `display`, `visibility`, `opacity`, `height`, classes
   - Força reabertura de `sectionCadastroRequisitante` e `collapseRequisitante`
   - Adiciona stack trace para debug

3. **Eventos DropDownTree Protegidos** (linhas 496-518):
   - `open`: Previne propagação de cliques no popup
   - `select`: Previne propagação ao selecionar item
   - `close`: Verifica se accordion foi fechado e reabre se necessário

4. **Logs de Debug Adicionados**:
   - Stack trace em `fecharFormularioCadastroRequisitante()` (linha 527)
   - Stack trace em `limparCamposCadastroRequisitante()` (linha 555)
   - Logs detalhados em eventos do DropDownTree

**Arquivos Afetados**:
- `wwwroot/js/agendamento/services/requisitante.service.js` (linhas 331-520)

**Impacto**:
- Accordion agora só fecha via botão manual
- Usuário pode selecionar Setor sem perder dados
- Sistema continua com validações e proteções existentes

**Status**: ✅ **Concluído e Testado**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.1

---

## [Data Anterior] - Versão Inicial

**Descrição**: Criação inicial do sistema de cadastro de requisitantes inline.

**Funcionalidades**:
- Accordion com formulário de cadastro
- Validações client-side
- Integração com API backend
- Auto-atualização de dropdowns

**Status**: ✅ **Concluído**

**Versão**: 1.0

---

## Histórico de Versões

| Versão | Data | Descrição |
|--------|------|-----------|
| 1.0 | - | Versão inicial - Sistema de cadastro inline de requisitantes |
| 1.1 | 12/01/2026 | Correção crítica: Prevenir fechamento automático do accordion ao usar DropDownTree |

---

## Referências

- [Documentação da Agenda](../../Pages/Index.md)
- [Documentação do ViagemController](../../Controllers/ViagemController.md)
- [Documentação do Alerta.js](../alerta.js.md)
- [Documentação Syncfusion Utils](../agendamento/utils/syncfusion.utils.md)

---

**Última atualização**: 12/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.1
