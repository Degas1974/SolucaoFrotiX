# Documentação: modal-viagens-consolidado.css

> **Última Atualização**: 22/01/2026 17:30
> **Versão Atual**: 1.6

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Estilos](#estrutura-de-estilos)
4. [Componentes Estilizados](#componentes-estilizados)
5. [Validações e Padrões](#validações-e-padrões)
6. [Troubleshooting](#troubleshooting)

---

## Visão Geral

O arquivo `modal-viagens-consolidado.css` contém estilos específicos para o **Modal de Viagens** da Agenda (`Pages/Agenda/Index.cshtml`). Ele consolida estilos de todos os componentes Syncfusion usados no modal, garantindo altura padrão de 38px e alinhamento vertical consistente.

### Características Principais
- ✅ **Altura Padrão 38px**: Todos os inputs, dropdowns, datepickers
- ✅ **Alinhamento Vertical**: Centralização perfeita de placeholder e texto
- ✅ **Bordas Consistentes**: Todos os componentes com borda `#ced4da`
- ✅ **Z-index Correto**: Popups aparecem acima do modal
- ✅ **Foco Destacado**: Borda azul com sombra ao focar

### Objetivo
Garantir que todos os componentes do Modal de Viagens tenham aparência consistente, independente do componente Syncfusion usado (ComboBox, DatePicker, DropDownTree, MultiSelect, NumericTextBox).

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| CSS3 | - | Estilos base |
| Syncfusion EJ2 | - | Componentes UI |
| Bootstrap 5 | - | Grid e utilitários |

### Escopo
Este CSS afeta apenas componentes dentro do **Modal de Viagens** (`#modalViagens`). Não afeta outros modais ou páginas do sistema.

---

## Estrutura de Estilos

### 1. DropDownTree (Seletores Hierárquicos)
**Localização**: Linhas 97-128

**Propósito**: Estilos para componentes de seleção hierárquica (Setor, Requisitante, etc.)

**Código**:
```css
.e-dropdowntree.e-control-wrapper {
    height: 38px !important;
    min-height: 38px !important;
}

.e-dropdowntree .e-input {
    height: 36px !important;
    padding: 0 8px !important;
    line-height: 36px !important;
    border: none !important;  /* Borda é aplicada no wrapper */
    box-shadow: none !important;
}

.e-dropdowntree .e-ddt-wrapper {
    display: flex !important;
    align-items: center !important;
    min-height: 38px !important;
    height: 38px !important;
    border: 1px solid #ced4da !important;
    border-radius: 0.25rem !important;
    box-sizing: border-box !important;
}
```

**Por que assim?**:
- Input interno sem borda (evita bordas duplas)
- Borda aplicada no wrapper externo
- Height 36px no input, 38px no wrapper (2px de borda)

---

### 2. DatePicker e TimePicker
**Localização**: Linhas 155-177

**Propósito**: Estilos para seletores de data e hora

**CORREÇÃO CRÍTICA** (12/01/2026 22:15):
- **Problema**: Campos DatePicker estavam sem bordas superior e inferior
- **Solução**: Adicionadas bordas explícitas em todas as direções

**Código**:
```css
.e-date-wrapper.e-control-wrapper,
.e-datepicker.e-control-wrapper,
.e-timepicker.e-control-wrapper {
    height: 38px !important;
    min-height: 38px !important;
    border: 1px solid #ced4da !important;
    border-top: 1px solid #ced4da !important;
    border-bottom: 1px solid #ced4da !important;
    border-left: 1px solid #ced4da !important;
    border-right: 1px solid #ced4da !important;
    border-radius: 0.375rem !important;
}

.e-date-wrapper .e-input,
.e-datepicker .e-input,
.e-timepicker .e-input {
    height: 36px !important;
    padding: 0 8px !important;
    line-height: 36px !important;
}

.e-datepicker .e-input-group-icon,
.e-timepicker .e-input-group-icon {
    height: 38px !important;
    line-height: 38px !important;
    width: 38px !important;
    font-size: 16px !important;
}
```

**Por que todas as bordas?**:
- Algumas versões do Syncfusion não aplicam `border` completo
- Declaração explícita de `border-top`, `border-bottom`, etc garante cobertura total
- Redundância intencional para evitar bugs visuais

---

### 3. MultiSelect (Seleção Múltipla)
**Localização**: Linhas 178-196

**Propósito**: Estilos para componentes de seleção múltipla (ex: múltiplos motoristas)

**Código**:
```css
.e-multiselect.e-input-group,
.e-multiselect.e-control-wrapper,
.e-multi-select-wrapper {
    min-height: 38px !important;
    height: 38px !important;
    max-height: 38px !important;
}

.e-multiselect .e-chips-collection,
.e-multiselect .e-delim-values {
    max-height: 36px !important;
    display: flex;
    align-items: center;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
}
```

**Por que `max-height`?**:
- MultiSelect pode expandir verticalmente com muitos itens
- `max-height: 38px` força altura única
- Overflow hidden com ellipsis para itens extras

---

### 4. NumericTextBox (Campos Numéricos)
**Localização**: Linhas 197-201

**Propósito**: Estilos para inputs numéricos (Quantidade, Km, etc.)

**Código**:
```css
.e-numerictextbox.e-control-wrapper {
    height: 38px !important;
    min-height: 38px !important;
}
```

---

### 5. Popups e Z-index
**Localização**: Linhas 203-223

**Propósito**: Garantir que popups de dropdown apareçam ACIMA do modal

**Problema Sem Isso**:
- Popup de dropdown aparece atrás do modal
- Usuário não consegue selecionar itens

**Solução**:
```css
.e-popup {
    z-index: 1055 !important;  /* Modal padrão Bootstrap é 1050 */
    position: absolute !important;
    overflow: visible !important;
}

.dropdown-wrapper {
    position: relative;
    overflow: visible !important;
    z-index: 1060;
}
```

**Por que 1055/1060?**:
- Modal Bootstrap: z-index 1050
- Popup deve estar acima: 1055+
- Wrapper extra segurança: 1060

---

### 6. Foco em Componentes Syncfusion
**Localização**: Linhas 225-240

**Propósito**: Destacar visualmente componente quando usuário interage

**Código**:
```css
.e-input-group:focus-within,
.e-input-group.e-input-focus,
.e-float-input:focus-within,
.e-float-input.e-input-focus,
.e-control-wrapper:focus-within,
.e-ddl:focus-within,
.e-dropdownbase:focus-within,
.e-datepicker:focus-within,
.e-timepicker:focus-within,
.e-dropdownlist:focus-within,
.e-combobox:focus-within,
.e-multiselect:focus-within {
    border-color: #80bdff !important;
    box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.15) !important;
}
```

**Por que `:focus-within`?**:
- Input interno pode estar focado, mas borda está no wrapper
- `:focus-within` detecta foco em qualquer filho
- Consistente com padrão Bootstrap

---

## Componentes Estilizados

### Tabela de Componentes
| Componente | Classe Principal | Altura | Borda | Z-index |
|------------|------------------|--------|-------|---------|
| DropDownTree | `.e-dropdowntree` | 38px | Sim (wrapper) | 1055+ (popup) |
| DatePicker | `.e-datepicker` | 38px | Sim (todas direções) | 1055+ (popup) |
| TimePicker | `.e-timepicker` | 38px | Sim | 1055+ (popup) |
| MultiSelect | `.e-multiselect` | 38px (max) | Sim | 1055+ (popup) |
| NumericTextBox | `.e-numerictextbox` | 38px | Sim | - |
| ComboBox | `.e-combobox` | 38px | Sim | 1055+ (popup) |

---

## Validações e Padrões

### Padrões de Altura
1. **Wrapper Externo**: Sempre 38px (min-height + height)
2. **Input Interno**: Sempre 36px (2px a menos que wrapper = bordas)
3. **Ícones**: Sempre 38px (alinhados com wrapper)

### Padrões de Borda
1. **Cor**: `#ced4da` (cinza padrão Bootstrap)
2. **Largura**: `1px` em todas as direções
3. **Radius**: `0.25rem` ou `0.375rem` dependendo do componente
4. **Foco**: `#80bdff` (azul Bootstrap)

### Padrões de Z-index
1. **Popups**: `1055` (acima de modal 1050)
2. **Wrappers**: `1060` (acima de popups se necessário)
3. **Nunca menos de 1050** (abaixo ficaria atrás do modal)

---

## Troubleshooting

### Problema: Popup de Dropdown Não Aparece
**Sintoma**: Ao clicar em dropdown, nada acontece ou popup aparece atrás do modal

**Causa**: Z-index do popup menor que z-index do modal

**Solução**: Verificar se `.e-popup` tem `z-index: 1055 !important`

**Código de Verificação**:
```javascript
// No console do navegador
const popup = document.querySelector('.e-popup');
console.log(window.getComputedStyle(popup).zIndex); // Deve ser >= 1055
```

---

### Problema: DatePicker Sem Bordas
**Sintoma**: Campo de data aparece sem borda superior ou inferior

**Causa**: Estilos de borda não aplicados ou sobrescritos

**Solução**:
1. Verificar se arquivo `modal-viagens-consolidado.css` está carregado
2. Verificar ordem de carregamento (deve vir DEPOIS de Syncfusion CSS)
3. Verificar se `!important` está presente

**Verificação**:
```javascript
// No console
const datepicker = document.querySelector('.e-datepicker.e-control-wrapper');
const styles = window.getComputedStyle(datepicker);
console.log('Border:', styles.border); // Deve ser "1px solid rgb(206, 212, 218)"
```

---

### Problema: Componentes com Altura Diferente
**Sintoma**: Alguns inputs têm 40px, outros 36px, outros 38px

**Causa**: Conflito de CSS ou ordem de carregamento

**Solução**:
1. Verificar se `modal-viagens-consolidado.css` está carregado DEPOIS de Syncfusion CSS
2. Verificar se há outros CSS customizados sobrescrevendo
3. Usar `!important` se necessário

**Ordem Correta de Carregamento**:
```html
<!-- 1. Bootstrap -->
<link rel="stylesheet" href="/css/bootstrap.min.css">

<!-- 2. Syncfusion -->
<link rel="stylesheet" href="/css/ej2/bootstrap.css">

<!-- 3. Frotix Global -->
<link rel="stylesheet" href="/css/frotix.css">

<!-- 4. Modal Consolidado (ÚLTIMO) -->
<link rel="stylesheet" href="/css/modal-viagens-consolidado.css">
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [22/01/2026 17:30] - Ajuste de Padding no Modal de Finalização

**Descrição**: Adicionado CSS para aumentar o padding dos cards internos no modal de finalização de viagens (`#modalFinalizaViagem`), evitando que os controles fiquem muito colados nas bordas.

**Problema Identificado**:
- Cards internos do modal de finalização (`#modalFinalizaViagem`) tinham padding padrão Bootstrap (1.25rem)
- Controles e labels ficavam muito próximos das bordas do card, criando sensação visual de aperto
- Usuário reportou que "cards internos estão muito colados na borda do card externo"

**Solução Implementada**:

Adicionado CSS específico para aumentar o padding de `.card-body` dentro do modal de finalização:

```css
/* ============================================
   MODAL DE FINALIZAÇÃO - PADDING DOS CARDS INTERNOS
   Adicionado em: 22/01/2026
   ============================================ */
#modalFinalizaViagem .card-body {
    padding: 1.5rem !important;
}
```

**Arquivos Afetados**:
- `wwwroot/css/modal-viagens-consolidado.css` (final do arquivo, após linha 1768)

**Impacto**:
- Todos os cards dentro do modal de finalização (Dados Iniciais, Dados Finais, Controle de Itens Devolvidos, Observações) agora têm espaçamento interno de 1.5rem
- Melhoria visual significativa na legibilidade e organização

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.6

---

## [22/01/2026 16:00] - Correção Definitiva: Overflow em Flexbox e Z-index de Dropdowns

**Descrição**: Implementação da correção definitiva para bordas cortadas em dropdowns, com overflow correto em divs flexbox e z-index adequado para componentes Syncfusion. Adicionado suporte global a tooltips em botões desabilitados.

**Problema Identificado**:

- Divs `.d-flex` aninhadas não tinham `overflow: visible` explícito
- Z-index dos dropdowns era muito baixo (apenas 1), permitindo sobreposição
- Botões desabilitados não permitiam tooltips por falta de `pointer-events: auto`
- Estrutura complexa de nesting criava contextos de stacking problemáticos

**Solução Implementada**:

1. **Overflow Visible em Flexbox** (linhas 376-380):

```css
.section-card-body .d-flex,
.section-card-body .d-flex.flex-column,
.section-card-body .d-flex.align-items-center,
.section-card-body .d-flex.justify-content-between {
    overflow: visible !important;
}
```

2. **Z-index Elevado para Dropdowns** (linhas 383-391):

```css
.section-card-body .e-dropdowntree,
.section-card-body .e-dropdownlist,
.section-card-body .e-combobox,
.section-card-body .e-multiselect {
    z-index: 1050 !important; /* Acima de modals padrão (1040) */
}

.section-card-body .e-popup,
.section-card-body .e-ddt-popup {
    z-index: 1055 !important;
}
```

3. **Tooltips em Botões Desabilitados** (linhas 417-425):

```css
.btn.disabled,
.btn:disabled,
.btn.disabled:hover,
.btn:disabled:hover {
    pointer-events: auto !important;
    opacity: 0.5;
    cursor: not-allowed !important;
    transform: none !important;
    box-shadow: none !important;
}
```

**Código Completo Modificado** (Seção 7):

```css
/* ============================================
   7. SECTION CARDS - CORREÇÃO OVERFLOW E Z-INDEX
   ============================================ */
.section-card {
    overflow: visible !important;
}

.section-card-body {
    padding: 1.25rem;
    overflow: visible !important;
}

/* CORREÇÃO CRÍTICA: overflow visible em flexbox */
.section-card-body .d-flex,
.section-card-body .d-flex.flex-column,
.section-card-body .d-flex.align-items-center {
    overflow: visible !important;
}

/* CORREÇÃO CRÍTICA: z-index adequado */
.section-card-body .e-dropdowntree,
.section-card-body .e-dropdownlist,
.section-card-body .e-combobox {
    z-index: 1050 !important;
}

/* Wrapper de dropdown */
.section-card-body .dropdown-wrapper {
    position: relative !important;
    overflow: visible !important;
    z-index: 1;
}

/* Botões desabilitados permitem tooltips */
.btn.disabled,
.btn:disabled {
    pointer-events: auto !important;
    opacity: 0.5;
    cursor: not-allowed !important;
}
```

**Impacto**:

- Bordas de dropdowns agora aparecem completamente visíveis
- Dropdowns aparecem acima de todos os outros elementos
- Tooltips funcionam em botões desabilitados (padrão global)
- Código consistente com padrão de Contrato/Index

**Arquivos Relacionados**:

- Pages/Agenda/Index.cshtml (uso do padrão)
- wwwroot/js/cadastros/contrato.js (referência do padrão)

**Status**: ✅ **Concluído**

**Responsável**: Claude Code (AI Assistant)

**Versão**: 1.5

---

## [22/01/2026 15:20] - Correção de Bordas Cortadas em Controles dentro de Section-Cards

**Descrição**: Corrigido problema de bordas dos controles (dropdowns, inputs) sendo cortadas ou visualmente incompletas dentro dos section-cards.

**Problema**:
- Controles Syncfusion (dropdowns, combobox, datepickers) dentro de `.section-card-body` tinham bordas visuais cortadas
- Bordas laterais coloridas dos cards (quando presentes) "invadiam" o espaço visual dos controles
- Dropdowns apareciam com bordas incompletas na parte superior/esquerda

**Solução Implementada**:
- Adicionado `overflow: visible !important` ao `.section-card` para prevenir corte de conteúdo
- Aplicado `position: relative` e `z-index: 1` a todos controles dentro de `.section-card-body`
- Garantido `overflow: visible` em wrappers Syncfusion (`.e-ddt-wrapper`, `.e-input-group.e-control-wrapper`)
- Adicionado `clip-path: none !important` para prevenir cortes de bordas
- Aplicado regras específicas para `.form-group`, `.row` e colunas `[class*="col-"]`

**Código Modificado** (Linhas 353-410):
```css
.section-card {
    border: 1px solid #e3e6f0;
    border-radius: 0.35rem;
    margin-bottom: 1rem;
    box-shadow: 0 0.15rem 1.75rem 0 rgba(58, 59, 69, 0.15);
    overflow: visible !important; /* ✅ Garantir que bordas não sejam cortadas */
}

/* Garantir que controles dentro dos cards tenham bordas completas */
.section-card-body .form-control,
.section-card-body .e-input-group,
.section-card-body .e-control-wrapper,
.section-card-body .e-dropdownlist,
.section-card-body .e-combobox,
.section-card-body .e-dropdowntree,
.section-card-body .e-multiselect,
.section-card-body .e-datepicker,
.section-card-body .e-timepicker {
    position: relative !important;
    z-index: 1 !important;
    overflow: visible !important;
}

/* Garantir que wrappers Syncfusion não cortem bordas */
.section-card-body .e-ddt-wrapper,
.section-card-body .e-input-group.e-control-wrapper {
    overflow: visible !important;
    clip-path: none !important;
}
```

**Impacto**:
- Controles agora têm bordas completas e visíveis em todos os lados
- Melhor experiência visual nos cards com bordas laterais coloridas
- Alinhamento perfeito de todos os elementos visuais

**Status**: ✅ **Concluído**

**Responsável**: Claude Code (AI Assistant)

**Versão**: 1.4

---

## [21/01/2026 14:16] - Correção de Posicionamento do Badge do Calendário de Dias Variados

**Descrição**: Corrigido posicionamento do badge contador de datas selecionadas (`#badgeContadorDatas`) no calendário de recorrência variada.

**Problema**:
- Badge aparecia completamente fora do lugar quando uma data era selecionada
- Posicionamento com `top: -40%` e `right: -60%` causava deslocamento excessivo
- Badge não aparecia corretamente no canto do calendário

**Solução Implementada**:
- Adicionado estilo para wrapper `.ftx-calendario-wrapper` com `position: relative` e `display: inline-block`
- Alterado posicionamento do badge para valores fixos: `top: -10px` e `right: -10px`
- Mantidas demais propriedades (tamanho, cor, sombra)

**Código Modificado** (Linhas 791-822):
```css
/* Wrapper do calendário com posição relativa para o badge */
.ftx-calendario-wrapper {
    position: relative;
    display: inline-block;
}

/* Badge contador de datas */
#badgeContadorDatas.badge-contador-datas {
    position: absolute !important;
    top: -10px;
    right: -10px;
    /* ... demais propriedades ... */
}
```

**Arquivos Afetados**:
- `wwwroot/css/modal-viagens-consolidado.css` (linhas 791-822)

**Status**: ✅ **Concluído**

**Versão**: 1.3

---

## [13/01/2026 00:15] - Reforço de Bordas com Seletores Específicos por ID

**Descrição**: Adicionados seletores CSS por ID específico para garantir que bordas sejam aplicadas em todos os DatePickers.

**Problema**:
- Bordas ainda não apareciam em alguns DatePickers
- Seletores genéricos não cobriam todos os casos

**Solução**:
- Adicionados seletores por ID: `#txtDataInicial`, `#txtDataFinal`, `#txtDataInicialEvento`, `#txtDataFinalEvento`
- Incluídos seletores para `.e-input-group.e-control-wrapper.e-date-wrapper` e `.e-input-group.e-control-wrapper.e-datepicker`
- Adicionado `box-sizing: border-box !important` para garantir cálculo correto de dimensões

**Código Modificado** (Linhas 155-173):
```css
/* DatePicker, TimePicker - Bordas em todos os lados */
.e-date-wrapper.e-control-wrapper,
.e-datepicker.e-control-wrapper,
.e-timepicker.e-control-wrapper,
#txtDataInicial.e-control-wrapper,
#txtDataFinal.e-control-wrapper,
#txtDataInicialEvento.e-control-wrapper,
#txtDataFinalEvento.e-control-wrapper,
.e-input-group.e-control-wrapper.e-date-wrapper,
.e-input-group.e-control-wrapper.e-datepicker {
    /* ... bordas ... */
    box-sizing: border-box !important;
}
```

**Status**: ✅ **Concluído**

---

## [12/01/2026 22:15] - Adicionadas Bordas aos DatePickers

**Descrição**: Corrigido problema visual onde campos DatePicker (Data Início, Data Fim, etc.) apareciam sem bordas superior e inferior.

**Problema**:
- DatePickers sem borda visível nos lados superior e inferior
- Dificuldade de identificar campo como input
- Inconsistência visual com outros componentes

**Causa**:
- Estilos de borda não estavam sendo aplicados corretamente
- Propriedade `border` genérica não funcionava em todas as versões do Syncfusion

**Solução Implementada**:
- Adicionadas declarações explícitas para TODAS as direções:
  - `border: 1px solid #ced4da !important;`
  - `border-top: 1px solid #ced4da !important;`
  - `border-bottom: 1px solid #ced4da !important;`
  - `border-left: 1px solid #ced4da !important;`
  - `border-right: 1px solid #ced4da !important;`
- Adicionado `border-radius: 0.375rem !important;`

**Código Modificado** (Linhas 155-165):
```css
.e-date-wrapper.e-control-wrapper,
.e-datepicker.e-control-wrapper,
.e-timepicker.e-control-wrapper {
    height: 38px !important;
    min-height: 38px !important;
    border: 1px solid #ced4da !important;
    border-top: 1px solid #ced4da !important;
    border-bottom: 1px solid #ced4da !important;
    border-left: 1px solid #ced4da !important;
    border-right: 1px solid #ced4da !important;
    border-radius: 0.375rem !important;
}
```

**Componentes Afetados**:
- `txtDataInicial` (Data Início da viagem)
- `txtDataFinal` (Data Fim da viagem)
- `txtDataInicialEvento` (Data Início do evento)
- `txtDataFinalEvento` (Data Fim do evento)
- Todos os outros DatePickers do modal

**Impacto**:
- ✅ Melhora visual significativa
- ✅ Consistência com outros inputs (DropDownTree, ComboBox, etc.)
- ✅ Facilita identificação dos campos para o usuário

**Status**: ✅ **Concluído**

---

## [21/01/2026] - Ajuste do Badge Contador de Datas

**Descrição**: Ajuste de posição do badge de contagem de datas (`#badgeContadorDatas`) para evitar sobreposição em resoluções menores.

**Mudança**:
- `top` passou para `-40%`
- `right` passou para `-60%`

**Impacto**:
- ✅ Badge fica ancorado ao canto do container
- ✅ Evita colisão com ícones do MultiSelect

**Status**: ✅ **Concluído**

---

## [DD/MM/AAAA] - Criação do Arquivo

**Descrição**: Criação inicial do arquivo `modal-viagens-consolidado.css` para consolidar estilos do Modal de Viagens.

**Objetivo**: Centralizar estilos Syncfusion do modal em um único arquivo para facilitar manutenção.

**Status**: ✅ **Concluído**

---

## Histórico de Versões

| Versão | Data | Descrição |
|--------|------|-----------|
| 1.0 | -/--/---- | Versão inicial |
| 1.1 | 12/01/2026 | Adicionadas bordas explícitas aos DatePickers |
| 1.2 | 21/01/2026 | Ajuste do badge contador de datas |

---

**Última atualização**: 21/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.2


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:
- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**: 
- OtimizaÃ§Ã£o de memÃ³ria e performance
- Tracking seletivo (apenas quando necessÃ¡rio para Update/Delete)
- PadrÃ£o mais limpo e explÃ­cito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seÃ§Ã£o 4.2)

**Impacto**: 
- Melhoria de performance em operaÃ§Ãµes de leitura (usa AsNoTracking por padrÃ£o)
- Tracking correto em operaÃ§Ãµes de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

**VersÃ£o**: Incremento de patch
