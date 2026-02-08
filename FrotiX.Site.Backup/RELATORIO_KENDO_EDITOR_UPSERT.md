# 📘 Relatório Técnico Completo: Kendo Editor na Página Viagens/Upsert.cshtml

> **Projeto:** FrotiX 2026 - FrotiX.Site.Backup (Janeiro)
> **Data:** 08/02/2026
> **Versão que FUNCIONA:** FrotiX.Site.Backup (Janeiro 2026)
> **Objetivo:** Documentar completamente o mecanismo de funcionamento do Editor Kendo/Telerik para identificar problemas em versões posteriores

---

## 📑 Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura e Componentes](#arquitetura-e-componentes)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Declaração HTML do Editor](#declaração-html-do-editor)
5. [Carregamento de Dependências](#carregamento-de-dependências)
6. [Inicialização do Editor](#inicialização-do-editor)
7. [Camada de Compatibilidade Syncfusion → Kendo](#camada-de-compatibilidade)
8. [Funções Globais Disponíveis](#funções-globais)
9. [Fluxo Completo de Execução](#fluxo-completo)
10. [Configuração da Toolbar](#configuração-da-toolbar)
11. [API de Integração](#api-de-integração)
12. [Possíveis Causas de Erro em Outras Versões](#possíveis-causas-de-erro)
13. [Checklist de Verificação](#checklist-de-verificação)

---

## 1. Visão Geral

### O Que É

O componente Editor utilizado na página `Pages/Viagens/Upsert.cshtml` é o **Kendo UI Editor** (da Telerik), um editor WYSIWYG (What You See Is What You Get) para edição de texto rico em HTML.

### Por Que Existe

Este editor foi implementado como **substituto do Syncfusion RichTextEditor**, mantendo compatibilidade retroativa através de uma **camada de abstração em JavaScript**.

### Funcionalidade

- Permite ao usuário editar o campo **"Descrição da Viagem"** (campo: `ViagemObj.Viagem.Descricao`)
- Oferece formatação rica: negrito, itálico, sublinhado, listas, links, imagens, tabelas, cores, etc.
- Salva conteúdo em formato HTML limpo
- Pode ser habilitado/desabilitado programaticamente (ex: viagens finalizadas ficam somente leitura)

---

## 2. Arquitetura e Componentes

### Diagrama de Arquitetura

```
┌──────────────────────────────────────────────────────────────────┐
│                  Página: Viagens/Upsert.cshtml                   │
│                                                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ SEÇÃO HTML (linha ~1903)                                   │ │
│  │ <textarea id="rte" name="ViagemObj.Viagem.Descricao">     │ │
│  │     @Html.Raw(Model.ViagemObj?.Viagem?.Descricao ?? "")   │ │
│  │ </textarea>                                                │ │
│  └────────────────────────────────────────────────────────────┘ │
│                           ↓                                      │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ SEÇÃO SCRIPTS (linha ~3050)                                │ │
│  │ <!-- Kendo UI Core -->                                     │ │
│  │ <link href="~/lib/kendo-ui/styles/kendo.default.min.css"> │ │
│  │ <script src="~/lib/kendo-ui/js/kendo.all.min.js">        │ │
│  │                                                            │ │
│  │ <!-- Kendo UI - Tradução pt-BR (CDN) -->                  │ │
│  │ <script src="...kendo.culture.pt-BR.min.js">              │ │
│  │ <script src="...kendo.messages.pt-BR.min.js">             │ │
│  │                                                            │ │
│  │ <!-- ⭐ ARQUIVO CRÍTICO DE INICIALIZAÇÃO -->               │ │
│  │ <script src="~/js/viagens/kendo-editor-upsert.js">        │ │
│  │                                                            │ │
│  │ <!-- Lógica de negócio da página -->                      │ │
│  │ <script src="~/js/cadastros/ViagemUpsert.js">             │ │
│  └────────────────────────────────────────────────────────────┘ │
└──────────────────────────────────────────────────────────────────┘
                              ↓
┌──────────────────────────────────────────────────────────────────┐
│    Arquivo: ~/js/viagens/kendo-editor-upsert.js (525 linhas)    │
│                                                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ $(document).ready() → setTimeout(300ms)                    │ │
│  │     ↓                                                      │ │
│  │ initKendoEditorUpsert()                                    │ │
│  │     ↓                                                      │ │
│  │ 1. Busca elemento: document.getElementById('rte')          │ │
│  │ 2. Verifica se já foi inicializado                        │ │
│  │ 3. Destrói instância anterior se existir                  │ │
│  │ 4. Cria Kendo Editor via jQuery:                          │ │
│  │    $(textarea).kendoEditor({ tools: [...], messages: {} })│ │
│  │ 5. Armazena em variável global: _kendoEditorUpsert         │ │
│  │ 6. Cria camada de compatibilidade Syncfusion              │ │
│  │     ↓                                                      │ │
│  │ criarCompatibilidadeSyncfusionUpsert(textarea)             │ │
│  │     ↓                                                      │ │
│  │ textarea.ej2_instances[0] = { compatibilidade API }        │ │
│  └────────────────────────────────────────────────────────────┘ │
└──────────────────────────────────────────────────────────────────┘
                              ↓
┌──────────────────────────────────────────────────────────────────┐
│         Arquivo: ~/js/cadastros/ViagemUpsert.js                 │
│                                                                  │
│  • Acessa editor via: _kendoEditorUpsert (variável global)      │
│  • Acessa editor via: textarea.ej2_instances[0] (compatibilidade)│
│  • Funções usadas:                                               │
│    - getEditorUpsertValue()  → obtém HTML                       │
│    - setEditorUpsertValue(html) → define HTML                   │
│    - enableEditorUpsert()    → habilita edição                  │
│    - disableEditorUpsert()   → desabilita edição                │
└──────────────────────────────────────────────────────────────────┘
```

---

## 3. Estrutura de Arquivos

### Arquivos Envolvidos

| Arquivo | Localização | Função | Linhas Críticas |
|---------|-------------|--------|-----------------|
| **Upsert.cshtml** | `Pages/Viagens/Upsert.cshtml` | Página Razor principal | 1903 (HTML), 3050-3063 (Scripts) |
| **kendo-editor-upsert.js** | `wwwroot/js/viagens/kendo-editor-upsert.js` | ⭐ Inicialização e compatibilidade | 150-511 (todo arquivo) |
| **ViagemUpsert.js** | `wwwroot/js/cadastros/ViagemUpsert.js` | Lógica de negócio da página | 1433-1457 (desabilita editor) |
| **kendo.all.min.js** | `wwwroot/lib/kendo-ui/js/kendo.all.min.js` | Biblioteca Kendo UI | - |
| **kendo.default.min.css** | `wwwroot/lib/kendo-ui/styles/kendo.default.min.css` | Estilos Kendo UI | - |
| **kendo.culture.pt-BR.min.js** | CDN Telerik | Localização pt-BR | - |
| **kendo.messages.pt-BR.min.js** | CDN Telerik | Mensagens pt-BR | - |

### Hierarquia de Dependências

```
Upsert.cshtml
  ├─→ kendo.all.min.js (DEVE estar ANTES)
  ├─→ kendo.culture.pt-BR.min.js (DEVE estar ANTES)
  ├─→ kendo.messages.pt-BR.min.js (DEVE estar ANTES)
  ├─→ kendo-editor-upsert.js ⭐ (INICIALIZA O EDITOR)
  └─→ ViagemUpsert.js (USA o editor)
```

⚠️ **ORDEM CRÍTICA:** Se `kendo-editor-upsert.js` for carregado ANTES de `kendo.all.min.js`, o editor NÃO será criado!

---

## 4. Declaração HTML do Editor

### Localização: `Pages/Viagens/Upsert.cshtml` (linha ~1903)

```html
<!-- ═══════════════════════════════════════════════════════════
     SEÇÃO 6: DESCRIÇÃO DA VIAGEM
     ═══════════════════════════════════════════════════════════ -->
<div class="ftx-section ftx-section-descricao">
    <div class="ftx-section-title">
        <i class="fa-duotone fa-file-lines"></i>
        Descrição da Viagem
    </div>
    <div class="row">
        <div class="col-12">
            <label class="ftx-label">Passageiros / Carga</label>

            <!-- Campo hidden para armazenar versão Word (SFDT) -->
            <input type="hidden" id="DescricaoViagemWordBase64"
                   name="DescricaoViagemWordBase64" />

            <!-- ⭐ TEXTAREA QUE SERÁ TRANSFORMADO EM KENDO EDITOR -->
            <!-- Comentário original: "Kendo Editor - Substitui Syncfusion RTE" -->
            <textarea id="rte"
                      name="ViagemObj.Viagem.Descricao"
                      style="height:320px; width:100%;">
                @Html.Raw(Model.ViagemObj?.Viagem?.Descricao ?? "")
            </textarea>

            <!-- Validação -->
            <div id="errorMessage">
                <span asp-validation-for="@Model.ViagemObj.Viagem.Descricao"></span>
            </div>
        </div>
    </div>
</div>
```

### Características Importantes

1. **ID obrigatório:** `id="rte"` - O arquivo `kendo-editor-upsert.js` busca especificamente por esse ID
2. **Name binding:** `name="ViagemObj.Viagem.Descricao"` - Vincula ao Model no backend
3. **Conteúdo inicial:** `@Html.Raw(...)` - Renderiza HTML existente (ao editar viagem)
4. **Estilo inline:** `height:320px; width:100%` - Dimensões base (Kendo sobrescreve)

---

## 5. Carregamento de Dependências

### Localização: `Pages/Viagens/Upsert.cshtml` (linha ~3050-3063)

```html
@section ScriptsBlock {
    <!-- Bootstrap Bundle -->
    <script src="~/node_modules/bootstrap/dist/js/bootstrap.bundle.min.js"
            asp-append-version="true"></script>

    <!-- ═══════════════════════════════════════════════════════════
         KENDO UI - NÚCLEO (LOCAL)
         ═══════════════════════════════════════════════════════════ -->
    <link href="~/lib/kendo-ui/styles/kendo.default.min.css"
          rel="stylesheet"
          asp-append-version="true" />

    <script src="~/lib/kendo-ui/js/kendo.all.min.js"
            asp-append-version="true"></script>

    <!-- ═══════════════════════════════════════════════════════════
         KENDO UI - LOCALIZAÇÃO PT-BR (CDN)
         ═══════════════════════════════════════════════════════════ -->
    <script src="https://kendo.cdn.telerik.com/2023.1.117/js/cultures/kendo.culture.pt-BR.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2023.1.117/js/messages/kendo.messages.pt-BR.min.js"></script>

    <!-- ═══════════════════════════════════════════════════════════
         ⭐ ARQUIVO CRÍTICO: INICIALIZAÇÃO DO KENDO EDITOR
         ═══════════════════════════════════════════════════════════ -->
    <script src="~/js/viagens/kendo-editor-upsert.js"
            asp-append-version="true"></script>

    <!-- Validação IA - Validador Evolutivo de Finalização -->
    <script src="~/js/validacao/ValidadorFinalizacaoIA.js"
            asp-append-version="true"></script>

    <!-- Lógica de Negócio da Página -->
    <script src="~/js/cadastros/ViagemUpsert.js"
            asp-append-version="true"></script>
}
```

### Ordem de Carregamento (CRÍTICO)

```
1. kendo.all.min.js              ← Biblioteca base (PRIMEIRO)
2. kendo.culture.pt-BR.min.js    ← Cultura/formatação
3. kendo.messages.pt-BR.min.js   ← Mensagens traduzidas
4. kendo-editor-upsert.js        ← INICIALIZA o editor (depende de 1-3)
5. ViagemUpsert.js               ← USA o editor (depende de 4)
```

⚠️ **Se essa ordem for alterada, o editor NÃO funcionará!**

---

## 6. Inicialização do Editor

### Arquivo: `~/js/viagens/kendo-editor-upsert.js`

#### 6.1. Variáveis Globais

```javascript
// Linha 144-145
let _kendoEditorUpsert = null;              // ⭐ Instância do Kendo Editor
let _kendoEditorUpsertInitialized = false;  // Flag de controle
```

Essas variáveis são **globais** (acessíveis de qualquer arquivo JS).

#### 6.2. Função Principal: `initKendoEditorUpsert()`

**Localização:** Linha 150-256

```javascript
function initKendoEditorUpsert()
{
    try
    {
        // ═══════════════════════════════════════════════════════════
        // PASSO 1: Buscar elemento HTML
        // ═══════════════════════════════════════════════════════════
        const textarea = document.getElementById('rte');
        if (!textarea) return null;

        // ═══════════════════════════════════════════════════════════
        // PASSO 2: Verificar se já foi inicializado
        // ═══════════════════════════════════════════════════════════
        if (_kendoEditorUpsertInitialized && _kendoEditorUpsert)
        {
            return _kendoEditorUpsert;
        }

        // ═══════════════════════════════════════════════════════════
        // PASSO 3: Destruir instância anterior (se existir)
        // ═══════════════════════════════════════════════════════════
        const existingEditor = $(textarea).data('kendoEditor');
        if (existingEditor)
        {
            existingEditor.destroy();
            $(textarea).unwrap();
        }

        // ═══════════════════════════════════════════════════════════
        // PASSO 4: CRIAR KENDO EDITOR (NÚCLEO)
        // ═══════════════════════════════════════════════════════════
        _kendoEditorUpsert = $(textarea).kendoEditor({
            tools: [
                "bold", "italic", "underline", "strikethrough",
                "separator",
                "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
                "separator",
                "insertUnorderedList", "insertOrderedList",
                "separator",
                "indent", "outdent",
                "separator",
                "createLink", "unlink",
                "separator",
                "insertImage",
                "separator",
                "fontName", "fontSize",
                "separator",
                "foreColor", "backColor",
                "separator",
                "cleanFormatting",
                "separator",
                "viewHtml"
            ],
            stylesheets: [],
            messages: {
                bold: "Negrito",
                italic: "Itálico",
                underline: "Sublinhado",
                // ... (todas as traduções pt-BR)
            },
            resizable: {
                content: true,
                toolbar: false
            },
            imageBrowser: {
                transport: {
                    read: "/api/Viagem/ListarImagens",
                    uploadUrl: "/api/Viagem/SaveImage",
                    thumbnailUrl: function(path) {
                        return path;
                    }
                }
            }
        }).data('kendoEditor');  // ⭐ RETORNA INSTÂNCIA DO EDITOR

        _kendoEditorUpsertInitialized = true;

        // ═══════════════════════════════════════════════════════════
        // PASSO 5: Criar camada de compatibilidade Syncfusion
        // ═══════════════════════════════════════════════════════════
        criarCompatibilidadeSyncfusionUpsert(textarea);

        return _kendoEditorUpsert;
    }
    catch (error)
    {
        Alerta.TratamentoErroComLinha("kendo-editor-upsert.js", "initKendoEditorUpsert", error);
        return null;
    }
}
```

#### 6.3. Inicialização Automática (DOM Ready)

**Localização:** Linha 493-511

```javascript
$(document).ready(function () {
    try {
        // ═══════════════════════════════════════════════════════════
        // DELAY DE 300ms: Garante que DOM está completamente pronto
        // ═══════════════════════════════════════════════════════════
        setTimeout(function () {
            initKendoEditorUpsert();

            // Se viagem finalizada, desabilitar editor
            if (window.viagemFinalizada === true) {
                disableEditorUpsert();
            }
        }, 300);
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'kendo-editor-upsert.js',
            'document.ready',
            error,
        );
    }
});
```

⚠️ **IMPORTANTE:** O delay de 300ms é **crítico**. Se removido, pode causar erros de "elemento não encontrado".

---

## 7. Camada de Compatibilidade Syncfusion → Kendo

### Por Que Existe?

O código antigo (ViagemUpsert.js) foi escrito para funcionar com **Syncfusion RichTextEditor**, que usa a API `ej2_instances[0]`. Para evitar refatoração massiva, foi criada uma **camada de compatibilidade** que simula a API do Syncfusion usando Kendo por baixo.

### Função: `criarCompatibilidadeSyncfusionUpsert(textarea)`

**Localização:** Linha 261-375

```javascript
function criarCompatibilidadeSyncfusionUpsert(textarea) {
    try {
        // ═══════════════════════════════════════════════════════════
        // OBJETO DE COMPATIBILIDADE (simula API Syncfusion)
        // ═══════════════════════════════════════════════════════════
        const compatObj = {
            _value: '',
            _readonly: false,
            _enabled: true,

            // ─────────────────────────────────────────────────────
            // GETTER: Retorna valor atual do editor
            // ─────────────────────────────────────────────────────
            getValue: function () {
                if (_kendoEditorUpsert) {
                    return _kendoEditorUpsert.value() || '';
                }
                return '';
            },

            // ─────────────────────────────────────────────────────
            // SETTER: Define novo valor no editor
            // ─────────────────────────────────────────────────────
            setValue: function (val) {
                if (_kendoEditorUpsert) {
                    _kendoEditorUpsert.value(val || '');
                }
            },

            // ─────────────────────────────────────────────────────
            // REFRESH: Atualiza editor (não necessário no Kendo)
            // ─────────────────────────────────────────────────────
            refresh: function () {
                if (_kendoEditorUpsert) {
                    _kendoEditorUpsert.refresh();
                }
            },

            // ─────────────────────────────────────────────────────
            // ENABLE: Habilita edição
            // ─────────────────────────────────────────────────────
            enable: function () {
                if (_kendoEditorUpsert) {
                    _kendoEditorUpsert.body.contentEditable = true;
                    $(textarea).closest('.k-editor').removeClass('k-disabled');
                    this._enabled = true;
                    this._readonly = false;
                }
            },

            // ─────────────────────────────────────────────────────
            // DISABLE: Desabilita edição
            // ─────────────────────────────────────────────────────
            disable: function () {
                if (_kendoEditorUpsert) {
                    _kendoEditorUpsert.body.contentEditable = false;
                    $(textarea).closest('.k-editor').addClass('k-disabled');
                    this._enabled = false;
                    this._readonly = true;
                }
            },

            // ─────────────────────────────────────────────────────
            // FOCUS: Foca no editor
            // ─────────────────────────────────────────────────────
            focus: function () {
                if (_kendoEditorUpsert) {
                    _kendoEditorUpsert.focus();
                }
            },
        };

        // ═══════════════════════════════════════════════════════════
        // DEFINIR GETTER/SETTER PARA PROPRIEDADE 'value'
        // ═══════════════════════════════════════════════════════════
        Object.defineProperty(compatObj, 'value', {
            get: function () {
                return this.getValue();
            },
            set: function (val) {
                this.setValue(val);
            },
        });

        // ═══════════════════════════════════════════════════════════
        // DEFINIR GETTER/SETTER PARA PROPRIEDADE 'readonly'
        // ═══════════════════════════════════════════════════════════
        Object.defineProperty(compatObj, 'readonly', {
            get: function () {
                return this._readonly;
            },
            set: function (val) {
                this._readonly = val;
                if (_kendoEditorUpsert) {
                    if (val) {
                        this.disable();
                    } else {
                        this.enable();
                    }
                }
            },
        });

        // ═══════════════════════════════════════════════════════════
        // DEFINIR GETTER/SETTER PARA PROPRIEDADE 'enabled'
        // ═══════════════════════════════════════════════════════════
        Object.defineProperty(compatObj, 'enabled', {
            get: function () {
                return this._enabled;
            },
            set: function (val) {
                this._enabled = val;
                if (_kendoEditorUpsert) {
                    if (val) {
                        this.enable();
                    } else {
                        this.disable();
                    }
                }
            },
        });

        // ═══════════════════════════════════════════════════════════
        // ⭐ SIMULAR ej2_instances PARA COMPATIBILIDADE
        // ═══════════════════════════════════════════════════════════
        if (!textarea.ej2_instances) {
            textarea.ej2_instances = [];
        }
        textarea.ej2_instances[0] = compatObj;

    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'kendo-editor-upsert.js',
            'criarCompatibilidadeSyncfusionUpsert',
            error,
        );
    }
}
```

### Como Funciona na Prática

**Código antigo (ViagemUpsert.js) usa:**

```javascript
// Obter elemento
const rteElement = document.getElementById('rte');

// Acessar instância Syncfusion (SIMULADA)
const rteInstance = rteElement.ej2_instances[0];

// Usar métodos Syncfusion (TRADUZIDOS para Kendo)
rteInstance.enabled = false;  // → Chama disable() → Kendo contentEditable = false
```

**Por baixo dos panos:**

```javascript
rteInstance.enabled = false
    ↓
Object.defineProperty 'enabled' setter
    ↓
this.disable()
    ↓
_kendoEditorUpsert.body.contentEditable = false
$('#rte').closest('.k-editor').addClass('k-disabled')
```

---

## 8. Funções Globais Disponíveis

### Tabela de Funções

| Função | Localização | Descrição | Retorno |
|--------|-------------|-----------|---------|
| `initKendoEditorUpsert()` | Linha 150 | Inicializa o editor | Instância Kendo ou null |
| `destroyKendoEditorUpsert()` | Linha 380 | Destrói o editor e limpa memória | void |
| `getEditorUpsertValue()` | Linha 405 | Obtém HTML do editor | string (HTML) |
| `setEditorUpsertValue(html)` | Linha 424 | Define HTML no editor | void |
| `clearEditorUpsert()` | Linha 441 | Limpa conteúdo do editor | void |
| `enableEditorUpsert()` | Linha 456 | Habilita edição | void |
| `disableEditorUpsert()` | Linha 474 | Desabilita edição | void |
| `toolbarClick(e)` | Linha 521 | Callback vazio (compatibilidade) | void |

### Exemplos de Uso

```javascript
// ═══════════════════════════════════════════════════════════
// OBTER CONTEÚDO (ao salvar viagem)
// ═══════════════════════════════════════════════════════════
const descricao = getEditorUpsertValue();
// Retorna: "<p><strong>Texto</strong> formatado com <em>HTML</em></p>"

// ═══════════════════════════════════════════════════════════
// DEFINIR CONTEÚDO (ao carregar viagem existente)
// ═══════════════════════════════════════════════════════════
setEditorUpsertValue("<p>Nova descrição</p>");

// ═══════════════════════════════════════════════════════════
// LIMPAR EDITOR (nova viagem)
// ═══════════════════════════════════════════════════════════
clearEditorUpsert();

// ═══════════════════════════════════════════════════════════
// DESABILITAR EDITOR (viagem finalizada)
// ═══════════════════════════════════════════════════════════
disableEditorUpsert();

// ═══════════════════════════════════════════════════════════
// HABILITAR EDITOR (reabrir viagem)
// ═══════════════════════════════════════════════════════════
enableEditorUpsert();
```

---

## 9. Fluxo Completo de Execução

### Timeline Detalhada

```
┌─────────────────────────────────────────────────────────────────┐
│ MOMENTO 1: Carregamento da Página                              │
└─────────────────────────────────────────────────────────────────┘
[00:00.000] Browser requisita /Viagens/Upsert
[00:00.050] Servidor renderiza Upsert.cshtml
[00:00.100] HTML enviado ao browser
              ├─→ <textarea id="rte"> no DOM
              └─→ @section ScriptsBlock renderizado no rodapé

┌─────────────────────────────────────────────────────────────────┐
│ MOMENTO 2: Carregamento de Scripts (ORDEM CRÍTICA)             │
└─────────────────────────────────────────────────────────────────┘
[00:00.150] ⬇️ Baixa kendo.all.min.js (biblioteca base)
[00:00.200] ✅ Kendo UI carregado → window.kendo disponível
[00:00.250] ⬇️ Baixa kendo.culture.pt-BR.min.js
[00:00.300] ⬇️ Baixa kendo.messages.pt-BR.min.js
[00:00.350] ⬇️ Baixa kendo-editor-upsert.js
[00:00.400] ✅ Funções globais disponíveis:
              - initKendoEditorUpsert
              - getEditorUpsertValue
              - setEditorUpsertValue
              - enableEditorUpsert
              - disableEditorUpsert
[00:00.450] ⬇️ Baixa ViagemUpsert.js

┌─────────────────────────────────────────────────────────────────┐
│ MOMENTO 3: DOM Ready + Inicialização (CRÍTICO)                 │
└─────────────────────────────────────────────────────────────────┘
[00:00.500] 🟢 $(document).ready disparado
[00:00.500] kendo-editor-upsert.js → $(document).ready() executado
              ├─→ setTimeout(..., 300) agendado
              └─→ Aguarda 300ms para garantir DOM completo

[00:00.800] ⏰ setTimeout dispara → initKendoEditorUpsert()
              ├─→ Busca: document.getElementById('rte')
              ├─→ ✅ Elemento encontrado: <textarea id="rte">
              ├─→ Verifica: _kendoEditorUpsertInitialized === false
              ├─→ Destrói editor anterior (se existir)
              ├─→ Cria Kendo Editor:
              │     $(textarea).kendoEditor({ tools: [...] })
              ├─→ Armazena instância: _kendoEditorUpsert = editor
              ├─→ Define flag: _kendoEditorUpsertInitialized = true
              └─→ Cria compatibilidade: textarea.ej2_instances[0] = compatObj

[00:00.850] ✅ Editor PRONTO para uso
              ├─→ Variável global: _kendoEditorUpsert !== null
              ├─→ Compatibilidade: textarea.ej2_instances[0] !== undefined
              └─→ Toolbar renderizada com 25 ferramentas

┌─────────────────────────────────────────────────────────────────┐
│ MOMENTO 4: Uso pelo Código de Negócio (ViagemUpsert.js)        │
└─────────────────────────────────────────────────────────────────┘
[00:01.000] Usuário carrega viagem ID 12345
              ├─→ AJAX: GET /api/Viagens/ObterDetalhes/12345
              ├─→ Response: { Descricao: "<p><b>Relatório</b></p>" }
              └─→ Chama: setEditorUpsertValue(response.Descricao)
                    ↓
                _kendoEditorUpsert.value("<p><b>Relatório</b></p>")
                    ↓
                Editor exibe texto formatado

[00:05.000] Usuário edita texto no editor (WYSIWYG)
              ├─→ Clica em "Bold" → texto fica <strong>
              ├─→ Insere lista → <ul><li>Item</li></ul>
              └─→ Editor mantém HTML em memória

[00:10.000] Usuário clica em "Salvar Viagem"
              ├─→ validarFormulario() executado
              ├─→ Chama: getEditorUpsertValue()
              │     ↓
              │   _kendoEditorUpsert.value()
              │     ↓
              │   Retorna: "<p><strong>Texto</strong> editado...</p>"
              ├─→ Monta FormData com descricao
              └─→ POST /api/Viagens/Salvar
                    ↓
                Backend salva HTML no banco
                    ↓
                ✅ Viagem salva com descrição rica

┌─────────────────────────────────────────────────────────────────┐
│ MOMENTO 5: Desabilitar Editor (Viagem Finalizada)              │
└─────────────────────────────────────────────────────────────────┘
[00:15.000] Usuário carrega viagem FINALIZADA
              ├─→ Backend retorna: { ...viagem, StatusFinalizado: true }
              ├─→ ViagemUpsert.js detecta viagem finalizada
              └─→ setTimeout(500ms) → disableEditorUpsert()
                    ↓
                _kendoEditorUpsert.body.contentEditable = false
                $('#rte').closest('.k-editor').addClass('k-disabled')
                    ↓
                ✅ Editor fica SOMENTE LEITURA (cinza, cursor proibido)
```

---

## 10. Configuração da Toolbar

### Ferramentas Disponíveis (25 itens)

**Localização:** `kendo-editor-upsert.js` linha 173-204

```javascript
tools: [
    // ═══════════════════════════════════════════════════════════
    // GRUPO 1: FORMATAÇÃO BÁSICA
    // ═══════════════════════════════════════════════════════════
    "bold",              // Negrito (Ctrl+B)
    "italic",            // Itálico (Ctrl+I)
    "underline",         // Sublinhado (Ctrl+U)
    "strikethrough",     // Tachado
    "separator",

    // ═══════════════════════════════════════════════════════════
    // GRUPO 2: ALINHAMENTO
    // ═══════════════════════════════════════════════════════════
    "justifyLeft",       // Alinhar à esquerda
    "justifyCenter",     // Centralizar
    "justifyRight",      // Alinhar à direita
    "justifyFull",       // Justificar
    "separator",

    // ═══════════════════════════════════════════════════════════
    // GRUPO 3: LISTAS
    // ═══════════════════════════════════════════════════════════
    "insertUnorderedList", // Lista com marcadores
    "insertOrderedList",   // Lista numerada
    "separator",

    // ═══════════════════════════════════════════════════════════
    // GRUPO 4: INDENTAÇÃO
    // ═══════════════════════════════════════════════════════════
    "indent",            // Aumentar recuo
    "outdent",           // Diminuir recuo
    "separator",

    // ═══════════════════════════════════════════════════════════
    // GRUPO 5: LINKS
    // ═══════════════════════════════════════════════════════════
    "createLink",        // Inserir link
    "unlink",            // Remover link
    "separator",

    // ═══════════════════════════════════════════════════════════
    // GRUPO 6: IMAGENS
    // ═══════════════════════════════════════════════════════════
    "insertImage",       // Inserir imagem (upload ou URL)
    "separator",

    // ═══════════════════════════════════════════════════════════
    // GRUPO 7: FONTES
    // ═══════════════════════════════════════════════════════════
    "fontName",          // Tipo de fonte (Arial, Times, etc)
    "fontSize",          // Tamanho da fonte (8px - 72px)
    "separator",

    // ═══════════════════════════════════════════════════════════
    // GRUPO 8: CORES
    // ═══════════════════════════════════════════════════════════
    "foreColor",         // Cor do texto
    "backColor",         // Cor de fundo
    "separator",

    // ═══════════════════════════════════════════════════════════
    // GRUPO 9: UTILITÁRIOS
    // ═══════════════════════════════════════════════════════════
    "cleanFormatting",   // Limpar formatação
    "separator",

    // ═══════════════════════════════════════════════════════════
    // GRUPO 10: MODO HTML
    // ═══════════════════════════════════════════════════════════
    "viewHtml"           // Ver/Editar código HTML
]
```

### Upload de Imagens (ImageBrowser)

**Localização:** `kendo-editor-upsert.js` linha 233-241

```javascript
imageBrowser: {
    transport: {
        // ═══════════════════════════════════════════════════════════
        // Endpoint para listar imagens disponíveis
        // ═══════════════════════════════════════════════════════════
        read: "/api/Viagem/ListarImagens",

        // ═══════════════════════════════════════════════════════════
        // Endpoint para fazer upload de novas imagens
        // ═══════════════════════════════════════════════════════════
        uploadUrl: "/api/Viagem/SaveImage",

        // ═══════════════════════════════════════════════════════════
        // Função para gerar URL da thumbnail
        // ═══════════════════════════════════════════════════════════
        thumbnailUrl: function(path) {
            return path;
        }
    }
}
```

⚠️ **IMPORTANTE:** Os endpoints `/api/Viagem/ListarImagens` e `/api/Viagem/SaveImage` DEVEM existir no backend!

---

## 11. API de Integração

### Variável Global: `_kendoEditorUpsert`

```javascript
// ═══════════════════════════════════════════════════════════
// MÉTODOS KENDO NATIVOS (uso direto)
// ═══════════════════════════════════════════════════════════

// Obter valor HTML
const html = _kendoEditorUpsert.value();

// Definir valor HTML
_kendoEditorUpsert.value("<p>Novo conteúdo</p>");

// Focar no editor
_kendoEditorUpsert.focus();

// Atualizar/refresh
_kendoEditorUpsert.refresh();

// Acessar corpo editável (iframe)
const body = _kendoEditorUpsert.body; // Element contentEditable

// Destruir editor
_kendoEditorUpsert.destroy();
```

### API de Compatibilidade: `textarea.ej2_instances[0]`

```javascript
const rteElement = document.getElementById('rte');
const rteInstance = rteElement.ej2_instances[0];

// ═══════════════════════════════════════════════════════════
// MÉTODOS (compatibilidade Syncfusion)
// ═══════════════════════════════════════════════════════════

// Obter valor
const html = rteInstance.getValue();
// Ou: rteInstance.value

// Definir valor
rteInstance.setValue("<p>Conteúdo</p>");
// Ou: rteInstance.value = "<p>Conteúdo</p>";

// Habilitar
rteInstance.enable();
// Ou: rteInstance.enabled = true;

// Desabilitar
rteInstance.disable();
// Ou: rteInstance.enabled = false;

// Modo somente leitura
rteInstance.readonly = true;

// Focar
rteInstance.focus();

// Atualizar
rteInstance.refresh();
```

### Funções Globais Helper

```javascript
// ═══════════════════════════════════════════════════════════
// FUNÇÕES WRAPPER (mais simples e seguras)
// ═══════════════════════════════════════════════════════════

// Obter valor
const descricao = getEditorUpsertValue();

// Definir valor
setEditorUpsertValue("<p>Nova descrição</p>");

// Limpar
clearEditorUpsert();

// Habilitar
enableEditorUpsert();

// Desabilitar
disableEditorUpsert();
```

---

## 12. Possíveis Causas de Erro em Outras Versões

### Checklist de Diagnóstico

#### ❌ ERRO 1: "Editor não aparece / Textarea simples visível"

**Possíveis causas:**

1. **Arquivo `kendo-editor-upsert.js` não está sendo carregado**
   - Verificar: DevTools → Network → Buscar `kendo-editor-upsert.js`
   - Solução: Adicionar `<script src="~/js/viagens/kendo-editor-upsert.js">`

2. **`kendo.all.min.js` não foi carregado ANTES de `kendo-editor-upsert.js`**
   - Verificar: Console → `Uncaught ReferenceError: kendo is not defined`
   - Solução: Carregar `kendo.all.min.js` PRIMEIRO

3. **ID do textarea foi alterado (não é mais `id="rte"`)**
   - Verificar: Inspecionar HTML → `<textarea id="???">`
   - Solução: Mudar para `id="rte"` OU alterar `kendo-editor-upsert.js` linha 154

4. **Delay de 300ms foi removido**
   - Verificar: `kendo-editor-upsert.js` linha 496 → `setTimeout(..., 300)`
   - Solução: Restaurar delay ou aumentar para 500ms

#### ❌ ERRO 2: "Editor aparece mas não funciona (botões não respondem)"

**Possíveis causas:**

1. **Tradução pt-BR não foi carregada**
   - Verificar: Console → Warnings sobre mensagens não encontradas
   - Solução: Adicionar `kendo.culture.pt-BR.min.js` e `kendo.messages.pt-BR.min.js`

2. **Conflito de versões jQuery**
   - Verificar: Console → `jQuery.fn.kendoEditor is not a function`
   - Solução: Garantir jQuery 3.x carregado ANTES do Kendo

3. **CSP (Content Security Policy) bloqueando execução**
   - Verificar: Console → CSP violation
   - Solução: Adicionar `script-src 'unsafe-inline'` no CSP header

#### ❌ ERRO 3: "Editor funciona mas não salva conteúdo"

**Possíveis causas:**

1. **Função `getEditorUpsertValue()` não está sendo chamada no submit**
   - Verificar: ViagemUpsert.js → Buscar por `getEditorUpsertValue()`
   - Solução: Adicionar chamada antes do POST

2. **Name attribute do textarea foi removido**
   - Verificar: `<textarea id="rte" name="???">`
   - Solução: Adicionar `name="ViagemObj.Viagem.Descricao"`

3. **Backend não está mapeando campo Descricao**
   - Verificar: Controller → `[FromBody] ViagemModel model`
   - Solução: Garantir propriedade `Descricao` no Model

#### ❌ ERRO 4: "Cannot read properties of null (_kendoEditorUpsert)"

**Possíveis causas:**

1. **Inicialização falhou silenciosamente**
   - Verificar: Console → Buscar erros no `initKendoEditorUpsert()`
   - Solução: Verificar try-catch, pode ter exception escondida

2. **ViagemUpsert.js está chamando editor ANTES da inicialização**
   - Verificar: Ordem de execução no $(document).ready
   - Solução: Aguardar evento `kendoEditorUpsertReady` (criar callback)

3. **Múltiplas inicializações destruindo editor**
   - Verificar: `_kendoEditorUpsertInitialized` flag
   - Solução: Garantir inicialização única

#### ❌ ERRO 5: "Upload de imagens não funciona (botão Insert Image)"

**Possíveis causas:**

1. **Endpoints `/api/Viagem/ListarImagens` e `/api/Viagem/SaveImage` não existem**
   - Verificar: Network → 404 Not Found ao clicar em "Insert Image"
   - Solução: Criar endpoints no backend

2. **Token AntiForgery não está sendo enviado**
   - Verificar: Network → Request Headers → Falta `XSRF-TOKEN`
   - Solução: Adicionar `toolbarClick` handler (linha 147-165 do Upsert.cshtml)

3. **CORS bloqueando upload**
   - Verificar: Console → CORS policy error
   - Solução: Configurar CORS no backend

#### ❌ ERRO 6: "Editor aparece duplicado ou quebrado visualmente"

**Possíveis causas:**

1. **CSS do Kendo não foi carregado**
   - Verificar: Network → `kendo.default.min.css` carregou?
   - Solução: Adicionar `<link href="~/lib/kendo-ui/styles/kendo.default.min.css">`

2. **Conflito de CSS com Bootstrap ou Syncfusion**
   - Verificar: Inspecionar elemento → Classes `.k-editor` com estilos conflitantes
   - Solução: Aumentar especificidade do CSS ou usar `!important`

3. **Editor sendo inicializado múltiplas vezes**
   - Verificar: Inspecionar DOM → Múltiplos `.k-editor` wrappers
   - Solução: Destruir instância anterior (linha 164-169)

---

## 13. Checklist de Verificação

### ✅ Para Garantir que o Editor Funciona

Use este checklist ao comparar com versões que NÃO funcionam:

#### 1. Estrutura HTML
- [ ] Existe `<textarea id="rte">`?
- [ ] Tem atributo `name="ViagemObj.Viagem.Descricao"`?
- [ ] Está dentro de um `<form>` válido?

#### 2. Bibliotecas Kendo
- [ ] `kendo.all.min.js` está em `wwwroot/lib/kendo-ui/js/`?
- [ ] `kendo.default.min.css` está em `wwwroot/lib/kendo-ui/styles/`?
- [ ] Versão do Kendo é 2023.1.117 ou superior?

#### 3. Scripts na Ordem Correta
- [ ] `kendo.all.min.js` carrega PRIMEIRO?
- [ ] `kendo.culture.pt-BR.min.js` carrega DEPOIS?
- [ ] `kendo.messages.pt-BR.min.js` carrega DEPOIS?
- [ ] `kendo-editor-upsert.js` carrega DEPOIS?
- [ ] `ViagemUpsert.js` carrega POR ÚLTIMO?

#### 4. Arquivo `kendo-editor-upsert.js`
- [ ] Existe em `wwwroot/js/viagens/kendo-editor-upsert.js`?
- [ ] Contém função `initKendoEditorUpsert()`?
- [ ] Contém `$(document).ready(...)` com setTimeout(300)?
- [ ] Contém `criarCompatibilidadeSyncfusionUpsert()`?
- [ ] Exporta funções globais (`getEditorUpsertValue`, etc)?

#### 5. Inicialização
- [ ] Console mostra erro de "kendo is not defined"? (se sim, ordem errada)
- [ ] Console mostra erro de "Cannot read property 'kendoEditor'"? (se sim, jQuery faltando)
- [ ] Inspecionar `_kendoEditorUpsert` no console → deve ser object, não null

#### 6. Integração com ViagemUpsert.js
- [ ] ViagemUpsert.js chama `getEditorUpsertValue()` ao salvar?
- [ ] ViagemUpsert.js chama `disableEditorUpsert()` se viagem finalizada?
- [ ] ViagemUpsert.js referencia `textarea.ej2_instances[0]`?

#### 7. Backend
- [ ] Controller tem ação para salvar Descricao?
- [ ] Model tem propriedade `Descricao` do tipo `string`?
- [ ] Endpoints `/api/Viagem/ListarImagens` e `/api/Viagem/SaveImage` existem?

#### 8. DevTools Verification
- [ ] Network → `kendo-editor-upsert.js` retorna 200?
- [ ] Network → `kendo.all.min.js` retorna 200?
- [ ] Console → Sem erros de JavaScript?
- [ ] Elements → `.k-editor` wrapper existe ao redor do textarea?
- [ ] Elements → `.k-editor` contém iframe com contenteditable?

---

## 14. Comparação Entre Versões

### Diferenças Prováveis em Versões Quebradas

| Aspecto | FrotiX.Site.Backup (Janeiro - FUNCIONA) | Versões Posteriores (QUEBRADO) |
|---------|----------------------------------------|-------------------------------|
| **Arquivo kendo-editor-upsert.js** | ✅ Existe em `wwwroot/js/viagens/` | ❓ Pode ter sido deletado ou movido |
| **Referência no Upsert.cshtml** | ✅ Linha 3058: `<script src="~/js/viagens/kendo-editor-upsert.js">` | ❓ Pode ter sido removida |
| **Ordem de scripts** | ✅ Kendo → kendo-editor-upsert → ViagemUpsert | ❓ Pode ter sido alterada |
| **ID do textarea** | ✅ `id="rte"` | ❓ Pode ter mudado para outro ID |
| **Biblioteca Kendo** | ✅ Local em `lib/kendo-ui/` | ❓ Pode ter sido removida ou atualizada quebra compatibilidade |
| **Delay de inicialização** | ✅ 300ms (setTimeout) | ❓ Pode ter sido removido |

### Como Identificar o Problema

1. **Abrir versão quebrada no DevTools**
2. **Console → Digitar:**
   ```javascript
   window._kendoEditorUpsert
   ```
   - Se retornar `undefined` → Editor NÃO foi inicializado
   - Se retornar `null` → Inicialização falhou
   - Se retornar `Object` → Editor OK, problema está em outro lugar

3. **Network → Verificar:**
   ```
   kendo-editor-upsert.js → Status 200?
   kendo.all.min.js → Status 200?
   ```

4. **Elements → Inspecionar:**
   ```html
   <textarea id="rte">
   ```
   - Se NÃO existe `.k-editor` wrapper ao redor → Kendo não transformou o textarea
   - Se existe mas textarea está visível → Erro de CSS

5. **Comparar arquivos:**
   ```bash
   # Comparar Upsert.cshtml
   diff FrotiX.Site.Backup/Pages/Viagens/Upsert.cshtml FrotiX.Site.Novo/Pages/Viagens/Upsert.cshtml

   # Comparar kendo-editor-upsert.js
   diff FrotiX.Site.Backup/wwwroot/js/viagens/kendo-editor-upsert.js FrotiX.Site.Novo/wwwroot/js/viagens/kendo-editor-upsert.js
   ```

---

## 15. Solução Rápida: Copiar Arquivos da Versão que Funciona

### Passo a Passo

1. **Copiar arquivo de inicialização:**
   ```bash
   cp FrotiX.Site.Backup/wwwroot/js/viagens/kendo-editor-upsert.js \
      FrotiX.Site.Novo/wwwroot/js/viagens/kendo-editor-upsert.js
   ```

2. **Verificar referência no Upsert.cshtml (linha ~3058):**
   ```html
   <script src="~/js/viagens/kendo-editor-upsert.js" asp-append-version="true"></script>
   ```

3. **Verificar ordem de scripts:**
   ```html
   <!-- 1. Kendo Core -->
   <script src="~/lib/kendo-ui/js/kendo.all.min.js"></script>

   <!-- 2. Tradução -->
   <script src="https://kendo.cdn.telerik.com/2023.1.117/js/cultures/kendo.culture.pt-BR.min.js"></script>
   <script src="https://kendo.cdn.telerik.com/2023.1.117/js/messages/kendo.messages.pt-BR.min.js"></script>

   <!-- 3. Inicialização (CRÍTICO) -->
   <script src="~/js/viagens/kendo-editor-upsert.js"></script>

   <!-- 4. Lógica da página -->
   <script src="~/js/cadastros/ViagemUpsert.js"></script>
   ```

4. **Limpar cache do browser:**
   - Chrome: Ctrl+Shift+Delete → Clear cache
   - Ou: DevTools → Network → Disable cache

5. **Testar:**
   - Abrir `/Viagens/Upsert`
   - Console → `_kendoEditorUpsert` → deve retornar Object
   - Editor deve aparecer com toolbar

---

## 16. Conclusão

### Resumo Executivo

O componente **Kendo Editor** na página `Viagens/Upsert.cshtml` funciona através de uma **arquitetura em 3 camadas**:

1. **HTML:** Textarea simples `<textarea id="rte">`
2. **Transformação:** Arquivo `kendo-editor-upsert.js` transforma textarea em editor WYSIWYG
3. **Compatibilidade:** Camada que simula API Syncfusion para código legado

### Pontos Críticos para Funcionamento

| # | Ponto Crítico | Por Que é Importante |
|---|---------------|----------------------|
| 1 | **Ordem de scripts** | Kendo ANTES de kendo-editor-upsert.js, senão `kendo` não existe |
| 2 | **ID do textarea = "rte"** | Código busca especificamente por esse ID |
| 3 | **Delay de 300ms** | Garante que DOM está pronto antes de inicializar |
| 4 | **Arquivo kendo-editor-upsert.js** | SEM esse arquivo, editor NÃO inicializa |
| 5 | **Variável global _kendoEditorUpsert** | Usada por ViagemUpsert.js para acessar editor |

### Onde Procurar Problemas em Versões Quebradas

1. ✅ Arquivo `kendo-editor-upsert.js` existe?
2. ✅ Referência ao arquivo no `@section ScriptsBlock`?
3. ✅ Ordem correta de scripts (Kendo → kendo-editor-upsert → ViagemUpsert)?
4. ✅ ID do textarea ainda é "rte"?
5. ✅ Biblioteca Kendo (`kendo.all.min.js`) ainda existe em `wwwroot/lib/`?

---

**Documento gerado em:** 08/02/2026
**Autor:** Claude Sonnet 4.5
**Versão FrotiX analisada:** FrotiX.Site.Backup (Janeiro 2026)
**Status:** ✅ Editor funcionando perfeitamente nesta versão
