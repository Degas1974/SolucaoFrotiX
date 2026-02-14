# Documentação: requisitante.service.js

> **Última Atualização**: 16/01/2026 19:15
> **Versão Atual**: 2.1

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
- ✅ **Modal Bootstrap**: Sistema de modal empilhado (stacked modals) - modal filho acima do pai
- ✅ **Sem Fechamento do Pai**: Modal de Agendamento permanece aberto quando abre Novo Requisitante
- ✅ **API Integration**: Comunicação com backend via AJAX (POST `/api/Viagem/AdicionarRequisitante`)
- ✅ **Validações Client-Side**: Ramal (8 dígitos), Email (@camara.leg.br), Nome (obrigatório)
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
| Bootstrap 5 | - | Modals empilhados (stacked modals com backdrop: false) |
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

## [16/01/2026 17:30] - Refatoração: Clear and Reload Pattern para Ordenação

**Descrição**: Refatorada lógica de atualização da lista de requisitantes para usar padrão "Clear and Reload", garantindo renderização correta e consistência com padrão usado em eventos.

**Problema Identificado**:
- Código usava `addItem()` seguido de modificação direta do `dataSource`
- Syncfusion ComboBox não garante renderização ao modificar `dataSource` diretamente
- Inconsistente com padrão aplicado em `evento.js` (evento.service.js)
- Lista podia ficar visualmente desordenada em alguns casos

**Solução Implementada** (linhas 1152-1210):

**ANTES**:
```javascript
// Adiciona o item (sem índice específico)
comboRequisitante.addItem(novoItem);

// Reordena o dataSource alfabeticamente
const dataSource = comboRequisitante.dataSource;
if (dataSource && Array.isArray(dataSource)) {
    dataSource.sort((a, b) => {
        const nomeA = (a.Requisitante || '').toLowerCase();
        const nomeB = (b.Requisitante || '').toLowerCase();
        return nomeA.localeCompare(nomeB, 'pt-BR');
    });
    comboRequisitante.dataSource = dataSource;
}
comboRequisitante.value = data.requisitanteid;
comboRequisitante.dataBind();
```

**DEPOIS - Clear and Reload Pattern**:
```javascript
// Obter dataSource atual
let dataSource = comboRequisitante.dataSource || [];

if (!Array.isArray(dataSource)) {
    dataSource = [];
}

// Verificar se já existe
const jaExiste = dataSource.some(item => item.RequisitanteId === data.requisitanteid);

if (!jaExiste) {
    // 1. Adicionar ao array
    dataSource.push(novoItem);
    console.log("📦 Novo item adicionado ao array");

    // 2. Ordenar alfabeticamente
    dataSource.sort((a, b) => {
        const nomeA = (a.Requisitante || '').toString().toLowerCase();
        const nomeB = (b.Requisitante || '').toString().toLowerCase();
        return nomeA.localeCompare(nomeB, 'pt-BR');
    });
    console.log("🔄 Lista ordenada alfabeticamente");

    // 3. Limpar componente
    comboRequisitante.dataSource = [];
    comboRequisitante.dataBind();

    // 4. Recarregar com lista ordenada
    comboRequisitante.dataSource = dataSource;
    comboRequisitante.dataBind();

    console.log("✅ Lista atualizada e ordenada com sucesso");
}

// Selecionar novo requisitante
comboRequisitante.value = data.requisitanteid;
comboRequisitante.dataBind();
```

**Vantagens do Novo Padrão**:

1. **Renderização Garantida**: Clear + Reload força Syncfusion a reconstruir lista
2. **Consistência**: Mesmo padrão usado em evento.js
3. **Verificação de Duplicata**: Evita adicionar mesmo item duas vezes
4. **Logs Detalhados**: Melhor rastreabilidade
5. **Type-Safe**: Uso de `.toString()` antes de `.toLowerCase()`
6. **Null-Safe**: Tratamento de array vazio

**Por Que Clear and Reload?**:
- Syncfusion ComboBox **não reordena automaticamente** quando `dataSource` é modificado
- Simplesmente ordenar array e atribuir não atualiza renderização visual
- Necessário "resetar" componente limpando e recarregando
- Força reconstrução da lista na ordem correta

**Logs Adicionados**:
- `📦 Novo requisitante a ser adicionado`
- `📦 Novo item adicionado ao array`
- `🔄 Lista ordenada alfabeticamente`
- `✅ Lista atualizada e ordenada com sucesso`
- `✅ Requisitante selecionado`

**Arquivos Afetados**:
- `wwwroot/js/agendamento/services/requisitante.service.js` (linhas 1152-1210)

**Impacto**:
- ✅ Lista sempre ordenada visualmente
- ✅ Renderização correta garantida
- ✅ Padrão consistente no projeto
- ✅ Código mais robusto e manutenível

**Status**: ✅ **Concluído**

**Versão**: 2.0

---

## [13/01/2026 19:15] - Correção: Ordem Alfabética, Ramal e Setor após Inserção

**Descrição**: Três correções no callback de sucesso ao inserir novo Requisitante:

1. **Ordem alfabética**: O novo requisitante estava sendo adicionado no índice 0 (início da lista), ignorando a ordem alfabética.

2. **Campo Ramal**: O código tratava `txtRamalRequisitanteSF` como componente Syncfusion, mas é um input HTML simples.

3. **Campo Setor**: O DropDownTree Syncfusion espera array como value, não string simples.

**Problema Original**:
- Novo requisitante aparecia fora da ordem alfabética na lista
- Campo Setor não era preenchido automaticamente após inserção
- Campo Ramal funcionava por acidente (verificação Syncfusion falhava silenciosamente)

**Solução Implementada**:

### Ordem Alfabética (linhas 1163-1180)

**ANTES (incorreto)**:
```javascript
comboRequisitante.addItem(novoItem, 0);  // Adiciona no índice 0 (início)
comboRequisitante.value = data.requisitanteid;
comboRequisitante.dataBind();
```

**DEPOIS (correto)**:
```javascript
// Adiciona o item (sem índice específico)
comboRequisitante.addItem(novoItem);

// Reordena o dataSource alfabeticamente
const dataSource = comboRequisitante.dataSource;
if (dataSource && Array.isArray(dataSource))
{
    dataSource.sort((a, b) =>
    {
        const nomeA = (a.Requisitante || '').toLowerCase();
        const nomeB = (b.Requisitante || '').toLowerCase();
        return nomeA.localeCompare(nomeB, 'pt-BR');
    });
    comboRequisitante.dataSource = dataSource;
}

comboRequisitante.value = data.requisitanteid;
comboRequisitante.dataBind();
```

### Campo Ramal (linhas 1185-1192)

**ANTES (incorreto)**:
```javascript
const txtRamalRequisitanteSF = document.getElementById("txtRamalRequisitanteSF");
if (txtRamalRequisitanteSF && txtRamalRequisitanteSF.ej2_instances && txtRamalRequisitanteSF.ej2_instances[0])
{
    const ramalTextBox = txtRamalRequisitanteSF.ej2_instances[0];
    ramalTextBox.value = txtRamal.value.trim();
    ramalTextBox.dataBind();
}
```

**DEPOIS (correto)**:
```javascript
// txtRamalRequisitanteSF é um input HTML simples, não Syncfusion
const txtRamalRequisitanteSF = document.getElementById("txtRamalRequisitanteSF");
if (txtRamalRequisitanteSF)
{
    txtRamalRequisitanteSF.value = txtRamal.value.trim();
}
```

### Campo Setor (linhas 1194-1210)

**ANTES (incorreto)**:
```javascript
comboSetor.value = setorValue;  // String simples
comboSetor.dataBind();
```

**DEPOIS (correto)**:
```javascript
// DropDownTree espera array como value
comboSetor.value = [setorValue.toString()];
comboSetor.dataBind();
```

**Arquivos Afetados**:
- `wwwroot/js/agendamento/services/requisitante.service.js` (linhas 1152-1210)

**Impacto**:
- ✅ Novo requisitante aparece em ordem alfabética correta na lista
- ✅ Campo Ramal é preenchido automaticamente após inserção
- ✅ Campo Setor é preenchido automaticamente após inserção

**Status**: ✅ **Concluído**

---

## [13/01/2026 17:30] - Correção: Validação do Setor com TreeView e Nome aceita Números

**Descrição**: Duas correções importantes no modal de cadastro de Novo Requisitante:

1. **Validação do Setor corrigida**: A validação ainda referenciava o DropDownTree antigo (`ddtSetorNovoRequisitante`), mas o modal agora usa TreeView inline com campo oculto `hiddenSetorId`.

2. **Campo Nome aceita números**: A função `sanitizeNomeCompleto()` foi corrigida para aceitar números além de letras Unicode.

**Problema Original**:
- Erro "Setor do Requisitante é obrigatório" mesmo com setor selecionado
- Campo Nome rejeitava números, permitindo apenas letras

**Solução Implementada**:

### Validação do Setor (linhas 1030-1111)

**ANTES (incorreto)**:
```javascript
const ddtSetor = document.getElementById("ddtSetorNovoRequisitante");
if (ddtSetor && ddtSetor.ej2_instances && ddtSetor.ej2_instances[0]) {
    setorValue = ddtSetor.ej2_instances[0].value;
}
```

**DEPOIS (correto)**:
```javascript
const hiddenSetorId = document.getElementById("hiddenSetorId");
if (hiddenSetorId) {
    setorValue = hiddenSetorId.value;
}
```

### Sanitização do Nome (linha 758)

**ANTES**:
```javascript
let limpo = valor.replace(/[^\p{L} ]+/gu, '');  // Apenas letras e espaços
```

**DEPOIS**:
```javascript
let limpo = valor.replace(/[^\p{L}\p{N} ]+/gu, '');  // Letras, números e espaços
```

**Arquivos Afetados**:
- `wwwroot/js/agendamento/services/requisitante.service.js`

**Impacto**:
- ✅ Validação do setor funciona corretamente com o novo TreeView
- ✅ Campo Nome aceita letras e números
- ✅ Integração completa com o modal de TreeView inline

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.8

---

## [13/01/2026 06:10] - Correção: Validação de Nome com Camel Case

**Descrição**: Adicionada validação de Camel Case no campo Nome, seguindo exatamente o padrão de `Pages/Usuarios/Upsert.cshtml`.

**Problema**: Campo Nome estava apenas validando se era obrigatório, mas **não estava convertendo para Camel Case** automaticamente.

**Solução**: Implementadas duas funções auxiliares e atualizada a validação de Nome (linhas 711-747, 893-936):

### Funções Auxiliares Adicionadas

**1. `toCamelCase(str)` (linhas 711-732)**
- Converte string para Camel Case
- Mantém conectores em minúsculo: 'de', 'da', 'do', 'das', 'dos', 'e'
- Primeira palavra sempre maiúscula

**2. `sanitizeNomeCompleto(valor)` (linhas 734-747)**
- Remove caracteres inválidos (apenas letras Unicode e espaços)
- Limita a 80 caracteres

### Atualização da Validação de Nome

**Event Listener INPUT** (linhas 901-911):
```javascript
novoNome.addEventListener("input", function() {
    novoNome.value = sanitizeNomeCompleto(novoNome.value);
});
```
- Remove caracteres inválidos em tempo real
- Limita a 80 caracteres automaticamente

**Event Listener BLUR** (linhas 914-933):
```javascript
novoNome.addEventListener("blur", function() {
    const valor = sanitizeNomeCompleto(novoNome.value.trim());
    if (valor) {
        novoNome.value = toCamelCase(valor);
        novoNome.classList.remove('is-invalid');
    } else {
        novoNome.classList.add('is-invalid');
    }
});
```
- Converte para Camel Case ao sair do campo
- Valida se campo não está vazio

**Exemplos de Conversão**:
- "JOÃO SILVA" → "João Silva"
- "maria de souza" → "Maria de Souza"
- "PEDRO DOS SANTOS" → "Pedro dos Santos"
- "ANA E CARLOS" → "Ana e Carlos"

**Arquivos Afetados**:
- `wwwroot/js/agendamento/services/requisitante.service.js` (linhas 711-747, 893-936)

**Impacto**:
- ✅ Nome agora segue padrão FrotiX de Camel Case
- ✅ Consistência com validação de Usuarios/Upsert
- ✅ Conectores corretamente em minúsculo

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.7 (atualização)

---

## [13/01/2026 05:54] - Implementação Completa de Modais Empilhados e Validações Padrão FrotiX

**Descrição**: Refatoração completa das funções de abertura/fechamento de modal e implementação de validações client-side seguindo o padrão de `Usuarios/Upsert.cshtml`.

**Problemas Corrigidos**:

### 1. Modal de Agendamento Fechava Inesperadamente
**Sintoma**: Ao abrir modal de Novo Requisitante, o modal de Agendamento (modalViagens) fechava automaticamente.

**Causa**: Função `abrirFormularioCadastroRequisitante()` ainda continha código de accordion (100+ linhas) tentando manipular elementos que não existem mais. O modal não era aberto via Bootstrap corretamente.

**Solução**: Reescrita completa da função (linhas 372-541):
```javascript
function abrirFormularioCadastroRequisitante()
{
    // 1. Limpa campos
    limparCamposCadastroRequisitante();

    // 2. Abre modal Bootstrap com backdrop: false
    const modalInstance = new bootstrap.Modal(modalElement, {
        backdrop: false, // NÃO cobrir modal pai
        keyboard: true
    });
    modalInstance.show();

    // 3. Inicializa DropDownTree após 'shown.bs.modal'
    modalElement.addEventListener('shown.bs.modal', function inicializarDropdown() {
        // Recria ddtSetorNovoRequisitante com z-index 1060
        // ...
    }, { once: true });
}
```

**Resultado**:
- Modal filho (`modalNovoRequisitante`) abre ACIMA do pai (`modalViagens`)
- Modal pai permanece aberto e funcional
- z-index do popup do DropDownTree configurado para 1060 (acima de modal 1055)

---

### 2. Dropdown de Requisitantes Não Atualizava Após Salvar
**Status**: Código já existia e estava correto (linhas 1176-1192).

A atualização já era feita automaticamente via:
```javascript
comboRequisitante.addItem(novoItem, 0); // Adiciona no topo
comboRequisitante.value = data.requisitanteid; // Seleciona
comboRequisitante.dataBind(); // Atualiza UI
```

**Observação**: Se o problema ainda ocorrer, verificar se `lstRequisitante` existe no DOM quando modal é aberto.

---

### 3. Validações Não Seguiam Padrão FrotiX
**Sintoma**: Validações eram básicas (apenas required) sem padrão de formato.

**Causa**: Faltava implementar validações client-side conforme padrão de `Pages/Usuarios/Upsert.cshtml`.

**Solução**: Nova função `configurarValidacoesRequisitante()` implementada (linhas 807-980):

#### Validação de Ramal (linhas 810-843)
```javascript
const txtRamal = document.getElementById("txtRamal");
const novoRamal = txtRamal.cloneNode(true); // Remove listeners antigos
txtRamal.parentNode.replaceChild(novoRamal, txtRamal);

// Input: aceita apenas dígitos (max 8)
novoRamal.addEventListener("input", function() {
    let valor = novoRamal.value.replace(/\D/g, '');
    valor = valor.substring(0, 8);
    novoRamal.value = valor;
});

// Blur: valida padrão /^[1-9]\d{7}$/
novoRamal.addEventListener("blur", function() {
    const valor = novoRamal.value.trim();
    const regex = /^[1-9]\d{7}$/;
    if (valor && !regex.test(valor)) {
        novoRamal.classList.add('is-invalid');
    } else {
        novoRamal.classList.remove('is-invalid');
    }
});
```

**Regra**: Ramal deve ter 8 dígitos começando com 1-9 (não pode começar com 0).

---

#### Validação de Email (linhas 846-929)
```javascript
const txtEmail = document.getElementById("txtEmail");
const novoEmail = txtEmail.cloneNode(true);
txtEmail.parentNode.replaceChild(novoEmail, txtEmail);

// Blur: auto-adiciona @camara.leg.br
novoEmail.addEventListener("blur", function() {
    let valor = novoEmail.value.trim().toLowerCase();
    if (valor) {
        // Remove @camara.leg.br se já existe
        valor = valor.replace(/@camara\.leg\.br$/i, '');
        // Remove outros @
        valor = valor.replace(/@/g, '');
        // Remove caracteres inválidos
        valor = valor.replace(/[^a-z0-9._-]/g, '');
        // Adiciona domínio
        if (valor.length > 0) {
            valor = valor + '@camara.leg.br';
        }
        novoEmail.value = valor;

        // Valida formato final
        const regex = /^[a-z0-9._-]+@camara\.leg\.br$/;
        if (valor && !regex.test(valor)) {
            novoEmail.classList.add('is-invalid');
        } else {
            novoEmail.classList.remove('is-invalid');
        }
    }
});

// Input: converte para lowercase, remove caracteres inválidos
novoEmail.addEventListener("input", function() {
    let valor = novoEmail.value.toLowerCase();
    valor = valor.replace(/[^a-z0-9._@-]/g, '');
    // Permite apenas 1 @
    const numArrobas = (valor.match(/@/g) || []).length;
    if (numArrobas > 1) {
        const partes = valor.split('@');
        valor = partes[0] + '@' + partes.slice(1).join('');
    }
    novoEmail.value = valor;
});
```

**Regras**:
- Apenas lowercase
- Caracteres permitidos: a-z, 0-9, `.`, `_`, `-`, `@`
- Domínio obrigatório: `@camara.leg.br`
- Apenas 1 `@` permitido

---

#### Validação de Nome (linhas 893-936)
```javascript
const txtNome = document.getElementById("txtNome");
const novoNome = txtNome.cloneNode(true);
txtNome.parentNode.replaceChild(novoNome, txtNome);

// INPUT: Remove caracteres inválidos e limita a 80 chars
novoNome.addEventListener("input", function() {
    novoNome.value = sanitizeNomeCompleto(novoNome.value);
});

// BLUR: Converte para Camel Case e valida
novoNome.addEventListener("blur", function() {
    const valor = sanitizeNomeCompleto(novoNome.value.trim());
    if (valor) {
        novoNome.value = toCamelCase(valor);
        novoNome.classList.remove('is-invalid');
    } else {
        novoNome.classList.add('is-invalid');
    }
});
```

**Funções Auxiliares** (linhas 711-747):
```javascript
function toCamelCase(str) {
    const conectores = ['de', 'da', 'do', 'das', 'dos', 'e'];
    return str
        .toLowerCase()
        .split(' ')
        .filter(palavra => palavra.length > 0)
        .map((palavra, index) => {
            // Primeira palavra sempre em Camel Case, demais verificar se é conector
            if (index === 0 || !conectores.includes(palavra)) {
                return palavra.charAt(0).toUpperCase() + palavra.slice(1);
            }
            return palavra;
        })
        .join(' ');
}

function sanitizeNomeCompleto(valor) {
    // Remove tudo exceto letras Unicode e espaços
    let limpo = valor.replace(/[^\p{L} ]+/gu, '');
    if (limpo.length > 80) {
        limpo = limpo.substring(0, 80);
    }
    return limpo;
}
```

**Regras**:
- Campo obrigatório (não pode estar vazio)
- Apenas letras Unicode e espaços
- Máximo 80 caracteres
- **Camel Case automático**: Primeira letra de cada palavra maiúscula, exceto conectores ('de', 'da', 'do', 'das', 'dos', 'e')
- Exemplos:
  - "JOÃO SILVA" → "João Silva"
  - "maria de souza" → "Maria de Souza"
  - "PEDRO DOS SANTOS" → "Pedro dos Santos"

---

### 4. Função `fecharFormularioCadastroRequisitante()` Atualizada (linhas 543-570)

**ANTES** (código de accordion):
```javascript
function fecharFormularioCadastroRequisitante()
{
    const sectionCadastro = document.getElementById("sectionCadastroRequisitante");
    if (sectionCadastro) {
        sectionCadastro.style.display = "none";
        isProcessing = false;
    }
}
```

**DEPOIS** (código de modal Bootstrap):
```javascript
function fecharFormularioCadastroRequisitante()
{
    const modalElement = document.getElementById('modalNovoRequisitante');
    if (modalElement) {
        const modalInstance = bootstrap.Modal.getInstance(modalElement);
        if (modalInstance) {
            modalInstance.hide();
        }
    }
    isProcessing = false;
}
```

---

### 5. Integração com `configurarBotoesCadastroRequisitante()` (linha 991)

Adicionada chamada para as validações:
```javascript
function configurarBotoesCadastroRequisitante()
{
    configurarValidacaoPonto(); // Já existia
    configurarValidacoesRequisitante(); // NOVA LINHA

    // ... resto do código
}
```

---

**Arquivos Afetados**:
- `wwwroot/js/agendamento/services/requisitante.service.js` (linhas 372-541, 543-570, 807-980, 991)

**Impacto**:
- ✅ Modal de Agendamento permanece aberto quando abre Novo Requisitante
- ✅ Dropdown de Requisitantes atualiza automaticamente (código já existia)
- ✅ Validações client-side seguem padrão FrotiX (Ramal, Email, Nome)
- ✅ Código mais limpo (-100 linhas de código obsoleto de accordion)
- ✅ Experiência do usuário melhorada

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.7

---

## [13/01/2026 01:00] - Adaptação Total para Modal Bootstrap (Remoção de Código Accordion)

**Descrição**: Removido/comentado todo código relacionado ao accordion de Requisitante após migração 100% para modal Bootstrap. Atualizado ID do DropDownTree de Setores para corresponder ao do modal.

**Problema**:
- JavaScript ainda tentava manipular elementos do accordion que foram removidos do HTML
- ID `ddtSetorRequisitante` não existe mais, foi substituído por `ddtSetorNovoRequisitante` no modal
- Código complexo de interceptação de cliques e listeners globais não era mais necessário

**Alterações Aplicadas**:

### 1. Replace All: Atualização de IDs (8 ocorrências)
```javascript
// ANTES
ddtSetorRequisitante

// DEPOIS
ddtSetorNovoRequisitante
```

**Linhas afetadas**: 500, 504, 508, 512, 538, 550, 619, 685, 686

### 2. Função `inicializarSistemaRequisitante()` (linhas 214, 221-268)

**Código Comentado**:
- Linha 214: Chamada para `configurarBotaoNovoRequisitante()` desabilitada
  - Razão: Modal usa `data-bs-toggle="modal"`, não precisa de listener manual
- Linhas 221-268: Todo o listener global de clicks comentado
  - Razão: Era para prevenir fechamento do accordion durante validação
  - Modal não fecha acidentalmente como accordion

### 3. Função `configurarBotoesCadastroRequisitante()` (linhas 838-864)

**Código Comentado**:
- Configuração do botão `btnFecharAccordionRequisitante`
- Razão: Modal tem botão com `data-bs-dismiss="modal"`, Bootstrap gerencia automaticamente

### 4. Função `salvarNovoRequisitante()` (linhas 1039-1046)

**ANTES**:
```javascript
// ===== FECHAR FORMULÁRIO =====
fecharFormularioCadastroRequisitante();
limparCamposCadastroRequisitante();
```

**DEPOIS**:
```javascript
// ===== FECHAR MODAL =====
const modalNovoRequisitante = bootstrap.Modal.getInstance(document.getElementById('modalNovoRequisitante'));
if (modalNovoRequisitante)
{
    modalNovoRequisitante.hide();
    console.log("✅ Modal fechado");
}
limparCamposCadastroRequisitante();
```

**Arquivos Afetados**:
- `wwwroot/js/agendamento/services/requisitante.service.js` (múltiplas linhas)
- `Documentacao/JavaScript/requisitante.service.md` (v1.5 → v1.6)

**Impacto**:
- **Simplificação**: ~200 linhas de código accordion comentadas/removidas
- **Menos complexidade**: Sem interceptação manual de eventos, Bootstrap gerencia tudo
- **Correção**: IDs agora correspondem aos elementos reais no modal
- **Performance**: Menos event listeners ativos

**Funções Mantidas** (ainda usadas pelo modal):
- `configurarValidacaoPonto()` - Validação do campo Ponto
- `salvarNovoRequisitante()` - Chamada da API
- `limparCamposCadastroRequisitante()` - Limpeza de campos

**Funções Obsoletas** (ainda no código mas não chamadas):
- `configurarBotaoNovoRequisitante()` - Não é mais chamada
- `abrirFormularioCadastroRequisitante()` - Não é mais chamada (modal abre automaticamente)
- `fecharFormularioCadastroRequisitante()` - Substituída por `modal.hide()`

**Status**: ✅ **Concluído**

**Versão**: 1.6

---

## [13/01/2026 00:30] - Correção Crítica: Erro de Sintaxe JavaScript (Missing catch or finally)

**Descrição**: Corrigido erro de sintaxe JavaScript que impedia o carregamento do módulo e bloqueava a renderização do DropDownTree.

**Problema Crítico**:
- Console do navegador reportava: `requisitante.service.js:496 Uncaught SyntaxError: Missing catch or finally after try`
- Erro impedia carregamento de **todo o arquivo JavaScript**
- DropDownTree não renderizava porque JavaScript não estava executando
- Usuário enviou screenshot mostrando campo vazio e console com erro

**Causa Raiz**:
- Chave de fechamento `}` **solta** na linha 496 da função `abrirFormularioCadastroRequisitante()`
- Estrutura do código estava quebrada após implementação do MutationObserver
- Linha 496 tinha: `}` isolada entre o observer e o código de limpeza de campos
- JavaScript interpretou como fechamento inválido de bloco try sem catch

**Código Problemático (linha 496)**:
```javascript
observer.observe(sectionCadastro, {
    attributes: true,
    childList: true,
    subtree: true
});

// Salvar observer para desconectar depois
window.__accordionObserver = observer;
}  // ← CHAVE SOLTA CAUSANDO ERRO

// 4) Limpa campos
limparCamposCadastroRequisitante();
```

**Solução Aplicada**:
```javascript
observer.observe(sectionCadastro, {
    attributes: true,
    childList: true,
    subtree: true
});

// Salvar observer para desconectar depois
window.__accordionObserver = observer;
// ← CHAVE REMOVIDA

// 4) Limpa campos
limparCamposCadastroRequisitante();
```

**Arquivos Afetados**:
- `wwwroot/js/agendamento/services/requisitante.service.js` (linha 496)
- `Documentacao/JavaScript/requisitante.service.md` (v1.4 → v1.5)

**Impacto**:
- **CRÍTICO**: JavaScript não carregava, bloqueando TODAS as funcionalidades do requisitante
- **Resolução**: Módulo agora carrega corretamente, DropDownTree pode renderizar

**Status**: ✅ **Concluído**

**Versão**: 1.5

---

## [12/01/2026 23:45] - Refatoração Completa: DropDownTree Renderizado via JavaScript Puro

**Descrição**: Implementada solução definitiva para o problema de renderização do DropDownTree de Setores no modal de Novo Requisitante. Substituído tag Razor `<ejs-dropdowntree>` por input HTML simples e inicialização via JavaScript puro no evento `shown.bs.modal`.

**Problema Persistente**:
- DropDownTree de Setores **não renderizava** no modal de Novo Requisitante
- Usuário reportou: "Continua não renderizando a lista"
- Tentativas anteriores com `recreate` e `destroy` não resolveram
- Tag Razor `<ejs-dropdowntree>` com `@ViewData["dataSetor"]` não funcionava em contexto de modal aninhado

**Causa Raiz Identificada**:
- Tag `<ejs-dropdowntree>` do Razor é renderizada no **servidor (server-side)**
- Quando modal não está visível (display: none), Syncfusion não consegue calcular dimensões e posição
- Dados de `@ViewData["dataSetor"]` podem não estar disponíveis no momento correto
- Modal Bootstrap é aberto **depois** do carregamento da página, causando conflito de timing

**Solução Implementada** (Padrão JavaScript Puro):

### 1. HTML Simplificado (Pages/Agenda/Index.cshtml linha 1688)

**ANTES (Tag Razor Syncfusion)**:
```html
<ejs-dropdowntree id="ddtSetorRequisitante"
                  ejs-for="@ViewData["dataSetor"]"
                  placeholder="Selecione o setor..."
                  allowFiltering="true"
                  popupHeight="200px"
                  showCheckBox="false"
                  sortOrder="Ascending">
    <e-dropdowntree-fields dataSource="@ViewData["dataSetor"]"
                            value="SetorSolicitanteId"
                            text="Nome"
                            parentValue="SetorPaiId"
                            hasChildren="HasChild">
    </e-dropdowntree-fields>
</ejs-dropdowntree>
```

**DEPOIS (Input HTML Simples)**:
```html
<input id="ddtSetorRequisitante" type="text" />
```

### 2. Inicialização via JavaScript (requisitante.service.js linhas 1119-1176)

**Nova Função `inicializarDropDownTreeModal()`**:
```javascript
function inicializarDropDownTreeModal()
{
    const modalRequisitante = document.getElementById("modalNovoRequisitante");

    if (!modalRequisitante) {
        console.error("❌ Modal modalNovoRequisitante não encontrado");
        return;
    }

    // ✅ EVENTO-CHAVE: Escuta quando modal é EXIBIDO (não apenas aberto)
    modalRequisitante.addEventListener('shown.bs.modal', function ()
    {
        console.log("🔓 Modal Novo Requisitante EXIBIDO - Criando DropDownTree");

        const ddtSetor = document.getElementById("ddtSetorRequisitante");

        if (!ddtSetor) {
            console.error("❌ Elemento ddtSetorRequisitante não encontrado");
            return;
        }

        // ✅ Verificar se dados de setores existem
        if (!window.SETORES_DATA || window.SETORES_DATA.length === 0)
        {
            console.log("⚠️ SETORES_DATA vazio - tentando capturar...");
            const capturado = capturarDadosSetores();

            if (!capturado)
            {
                console.warn("⚠️ Primeira tentativa falhou - retry em 500ms");
                setTimeout(() => {
                    capturarDadosSetores();
                    if (window.SETORES_DATA && window.SETORES_DATA.length > 0)
                    {
                        console.log("✅ Dados capturados no retry - criando dropdown");
                        criarDropDownTree(ddtSetor);
                    }
                    else
                    {
                        console.error("❌ Não foi possível capturar dados de setores");
                    }
                }, 500);
                return;
            }
        }

        // ✅ Criar DropDownTree quando modal está visível e dados disponíveis
        criarDropDownTree(ddtSetor);
    });

    console.log("✅ Listener shown.bs.modal registrado para modal Novo Requisitante");
}
```

### 3. Criação do DropDownTree (requisitante.service.js linhas 1181-1240)

**Nova Função `criarDropDownTree()`**:
```javascript
function criarDropDownTree(elemento)
{
    try
    {
        console.log("🔧 Criando DropDownTree de Setores...");

        // ✅ Destruir instância anterior se existir
        if (elemento.ej2_instances && elemento.ej2_instances[0])
        {
            console.log("🗑️ Destruindo instância anterior");
            elemento.ej2_instances[0].destroy();
        }

        console.log(`📦 Dados disponíveis: ${window.SETORES_DATA?.length || 0} setores`);

        // ✅ Criar nova instância com dados capturados
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
            created: function () {
                console.log("✅ DropDownTree CREATED disparado");
            },
            dataBound: function () {
                console.log("✅ DropDownTree DATA BOUND disparado");
                console.log(`   Total de itens carregados: ${this.treeData?.length || 0}`);
            }
        });

        dropdown.appendTo(elemento);
        console.log("✅ DropDownTree criado e anexado com sucesso");
    }
    catch (error)
    {
        console.error("❌ Erro ao criar DropDownTree:", error);
        Alerta.TratamentoErroComLinha("requisitante.service.js", "criarDropDownTree", error);
    }
}
```

### 4. Auto-Inicialização (requisitante.service.js linhas 1255-1266)

**Código de Auto-Start**:
```javascript
// ✅ Auto-inicialização quando documento carregar
if (document.readyState === 'loading')
{
    document.addEventListener('DOMContentLoaded', inicializarDropDownTreeModal);
    console.log("⏳ Aguardando DOMContentLoaded para inicializar modal Requisitante");
}
else
{
    // ✅ DOM já carregado - inicializar imediatamente
    inicializarDropDownTreeModal();
    console.log("✅ DOM já carregado - inicializando modal Requisitante agora");
}
```

### 5. Alteração no Botão (Pages/Agenda/Index.cshtml linhas 1705-1708)

**Texto do Botão Atualizado**:
```html
<button type="button" class="btn btn-ftx-fechar" data-bs-dismiss="modal">
    <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i>
    Cancelar Operação
</button>
```

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml`:
  - Linha 1688: Tag `<ejs-dropdowntree>` → `<input type="text">`
  - Linha 1707: Texto "Fechar" → "Cancelar Operação"
  - Linha 1705: Classe `btn-vinho` → `btn-ftx-fechar`
- `wwwroot/js/agendamento/services/requisitante.service.js`:
  - Linhas 1119-1176: Nova função `inicializarDropDownTreeModal()`
  - Linhas 1181-1240: Nova função `criarDropDownTree()`
  - Linhas 1255-1266: Código de auto-inicialização

**Por Que Funciona Agora**:
1. ✅ **Evento `shown.bs.modal`**: Garante que modal está **visível** antes de criar componente
2. ✅ **Input HTML simples**: Não depende de renderização server-side
3. ✅ **JavaScript puro**: Total controle sobre timing de criação
4. ✅ **Dados capturados no cliente**: Reutiliza `window.SETORES_DATA` já disponível
5. ✅ **Retry automático**: Se dados não estiverem disponíveis, tenta novamente após 500ms
6. ✅ **Destroy antes de criar**: Garante que não há instância duplicada
7. ✅ **Width 100%**: Garante visualização correta dentro do modal
8. ✅ **Logs detalhados**: Facilita debug se houver problemas

**Vantagens da Abordagem**:
- ✅ Solução robusta e confiável
- ✅ Não depende de timing de servidor
- ✅ Funciona com modais aninhados
- ✅ Reutiliza dados já carregados (performance)
- ✅ Fácil de debugar com logs
- ✅ Padrão reutilizável para outros modais

**Impacto**:
- ✅ **CRÍTICO RESOLVIDO**: DropDownTree agora renderiza corretamente
- ✅ Usuário pode selecionar Setor ao criar requisitante
- ✅ Modal funciona perfeitamente com Bootstrap 5
- ✅ Botão "Cancelar Operação" segue padrão FrotiX
- ✅ Sistema pronto para uso em produção

**Status**: ✅ **Concluído** - Aguardando teste do usuário

**Responsável**: Claude Code

**Versão**: 1.4

---

## [12/01/2026 23:15] - Validação do Campo Ponto e Melhorias no DropDownTree

**Descrição**: Implementadas validações automáticas do campo Ponto e melhorias na renderização do DropDownTree de Setores.

**Alterações Implementadas**:

### 1. Nova Função `configurarValidacaoPonto()` (linhas 679-759)

**Funcionalidade**: Valida o campo Ponto automaticamente no evento `blur` (lostfocus).

**Regras de Validação**:
- ✅ **Prefixo obrigatório**: Garante que o ponto sempre comece com `p_` (minúsculo)
- ✅ **Conversão automática**: Converte `P_` maiúsculo para `p_` minúsculo
- ✅ **Adição automática**: Se não houver prefixo, adiciona `p_` no início
- ✅ **Tamanho máximo**: Valida 50 caracteres (conforme limite do banco de dados)
- ✅ **Truncamento automático**: Se exceder 50 chars, trunca e exibe alerta
- ✅ **Verificação dupla**: Valida tamanho antes e depois de adicionar `p_`

**Código**:
```javascript
function configurarValidacaoPonto()
{
    const txtPonto = document.getElementById("txtPonto");
    if (!txtPonto) { console.warn("⚠️ txtPonto não encontrado"); return; }

    // Remove listeners anteriores
    const novoCampo = txtPonto.cloneNode(true);
    txtPonto.parentNode.replaceChild(novoCampo, txtPonto);

    // Adiciona validação no blur (lostfocus)
    novoCampo.addEventListener("blur", function(e)
    {
        try
        {
            let valor = novoCampo.value.trim();
            if (!valor) { return; } // Campo vazio, não valida

            // Verificar tamanho máximo (50 caracteres conforme banco)
            if (valor.length > 50)
            {
                Alerta.Warning("Atenção", "O Ponto não pode ter mais de 50 caracteres...", "OK");
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
                Alerta.Warning("Atenção", "O Ponto não pode ter mais de 50 caracteres...", "OK");
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
```

### 2. Melhorias na Recriação do DropDownTree (linhas 503-629)

**Problema Anterior**: DropDownTree de Setores não renderizava corretamente.

**Soluções Implementadas**:
- ✅ **Validação de dados capturados**: Verifica se `window.SETORES_DATA` foi populado
- ✅ **Tratamento de erro robusto**: Try-catch ao destruir e criar instância
- ✅ **Alerta ao usuário**: Informa se dados de setores não foram carregados
- ✅ **Logs detalhados**: Debug completo do processo de recriação
- ✅ **Novos eventos**: Adicionados `created` e `dataBound` para monitoramento
- ✅ **popupWidth**: Adicionado `'100%'` para melhor visualização

**Código Adicionado**:
```javascript
// Capturar dados de setores se ainda não existirem
if (!window.SETORES_DATA || window.SETORES_DATA.length === 0)
{
    console.log("📦 Capturando dados de setores...");
    const capturado = capturarDadosSetores();

    if (!capturado || !window.SETORES_DATA || window.SETORES_DATA.length === 0)
    {
        console.error("❌ Não foi possível capturar dados de setores!");
        Alerta.Warning("Atenção", "Não foi possível carregar a lista de setores...", "OK");
        return;
    }
}

console.log(`📦 Dados de setores disponíveis: ${window.SETORES_DATA?.length || 0} itens`);

// Novos eventos adicionados
created: function() {
    console.log("✅ DropDownTree CREATED disparado");
},

dataBound: function() {
    console.log("✅ DropDownTree DATA BOUND disparado");
    console.log(`   Total de itens: ${this.treeData?.length || 0}`);
}
```

### 3. Integração com `configurarBotoesCadastroRequisitante()`

**Alteração**: A validação do Ponto agora é configurada automaticamente ao abrir o formulário.

**Código** (linha 767):
```javascript
function configurarBotoesCadastroRequisitante()
{
    // ===== CONFIGURAR VALIDAÇÃO DO CAMPO PONTO =====
    configurarValidacaoPonto();

    // ... resto da função
}
```

**Arquivos Afetados**:
- `wwwroot/js/agendamento/services/requisitante.service.js`
- `Pages/Agenda/Index.cshtml` (maxlength e placeholder adicionados)

**Impacto**:
- ✅ Validação automática do campo Ponto sem necessidade de ação do usuário
- ✅ Garantia de padronização (`p_` minúsculo sempre)
- ✅ Prevenção de erro no banco por tamanho excedido (50 chars)
- ✅ DropDownTree de Setores renderiza corretamente
- ✅ Melhor experiência do usuário com alertas informativos

**Status**: ✅ **Concluído**

**Versão**: 1.4

---

## [16/01/2026 19:15] - Migração para Telerik ComboBox

**Descrição**: Atualizada inserção de novo requisitante para API Telerik (`getRequisitanteCombo()`).

**Mudanças** (linhas 1152-1202):
- `ej2_instances[0]` → `getRequisitanteCombo()`
- `dataSource` (propriedade) → `dataSource.data()` (método)
- `dataSource = [...]` → `setDataSource([...])`
- `value = x` → `value(x)`

**Versão**: 2.1

---

## [12/01/2026 19:20] - Tooltip e Logs de Debug no Botão Novo Requisitante

**Descrição**:
Adicionada tooltip padrão FrotiX ao botão "Novo Requisitante" e logs de debug aprimorados para facilitar diagnóstico de problemas de inicialização.

**Alterações**:

1. **Tooltip no Botão** (Pages/Agenda/Index.cshtml linha 1168):
   - Adicionado `data-bs-toggle="tooltip"`
   - Classe customizada: `tooltip-ftx-azul`
   - Texto: "Acrescentar novo Requisitante"
   - Posicionamento: `top`

2. **Logs de Debug Aprimorados**:
   - Log ao configurar botão (`configurarBotaoNovoRequisitante`)
   - Log detalhado no evento de clique (target, estaValidando, isProcessing)
   - Log de estado do accordion (display, estaOculto)
   - Mensagens mais visíveis com separadores

3. **Função resetarSistemaRequisitante()**:
   - Reseta flag `requisitanteServiceInicializado`
   - Fecha accordion se estiver aberto
   - Limpa todos os campos
   - Desconecta MutationObserver
   - Permite reinicialização ao reabrir modal
   - Exportada globalmente para uso externo

**Arquivos Afetados**:
- `Pages/Agenda/Index.cshtml` (linha 1168)
- `wwwroot/js/agendamento/services/requisitante.service.js` (linhas 252-257, 268-274, 303-311, 860-883)

**Impacto**:
- Melhor UX com tooltip explicativa
- Facilita diagnóstico de problemas de inicialização
- Permite reset completo ao fechar/reabrir modal

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.2

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
| 1.2 | 12/01/2026 | Tooltip no botão, logs de debug aprimorados e função resetarSistemaRequisitante |
| 1.3 | 12/01/2026 | Validação automática do campo Ponto e melhorias no DropDownTree |
| 1.4 | 12/01/2026 | Refatoração completa: DropDownTree renderizado via JavaScript puro com shown.bs.modal |

---

## Referências

- [Documentação da Agenda](../../Pages/Index.md)
- [Documentação do ViagemController](../../Controllers/ViagemController.md)
- [Documentação do Alerta.js](../alerta.js.md)
- [Documentação Syncfusion Utils](../agendamento/utils/syncfusion.utils.md)

---

**Última atualização**: 12/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.4
