# Documentação: modal-viagens-consolidado.css

> **Última Atualização**: 13/01/2026
> **Versão Atual**: 1.2

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

---

**Última atualização**: 12/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.1
