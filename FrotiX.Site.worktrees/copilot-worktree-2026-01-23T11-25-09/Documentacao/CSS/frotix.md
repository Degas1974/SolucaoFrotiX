# Documentação: frotix.css - Estilos Globais do Sistema FrotiX

> **Última Atualização**: 21/01/2026
> **Versão Atual**: 3.0
> **Padrão**: FrotiX Simplificado

---

## Objetivos

O arquivo **frotix.css** é a folha de estilos global do sistema FrotiX, contendo todas as classes utilitárias, componentes reutilizáveis, animações e padrões visuais seguidos em toda a aplicação.

- ✅ **Botões** padronizados (laranja header, ação, cores temáticas)
- ✅ **Badges de status** (ativo/inativo) com cores e animações
- ✅ **Formulários** (altura padrão 38px, centralização vertical)
- ✅ **Tooltips** Syncfusion customizados (sem setas, tema azul)

---

## Arquivos Envolvidos

1. **wwwroot/css/frotix.css** - Arquivo principal (4036 linhas)

2. **Pages/Shared/\_Head.cshtml** - Carregamento global do CSS

## 1. Variáveis CSS Globais

### Problema

Centralizar valores de cores, tamanhos e espaçamentos para facilitar manutenção e garantir consistência visual.

### Solução

Usar CSS Custom Properties (`:root`) para definir variáveis reutilizáveis em todo o sistema.

### Código

```css
:root {
  /* Motor do Glow (botões) */
  --glow-ring: 1px;
  --glow-spread: 4px;
  --glow-depth: 8px;

  /* Paleta para Status (centralizado no Global) */
  --status-active-bg: #22c55e; /* Verde "ok" */
  --status-active-bg-hover: #16a34a;
  --status-active-shadow: rgba(34, 197, 94, 0.45);
  --status-inactive-bg: #2f4f4f; /* Cinza tema */
  --status-inactive-bg-hover: #253a3a;
  --status-inactive-shadow: rgba(47, 79, 79, 0.45);

  /* Botões em tabela */
  --ftx-icon-btn: 28px;
  --ftx-badge-font: 0.72rem;
  --ftx-badge-pad-v: 0.18rem; /* vertical */
  --ftx-badge-pad-h: 0.5rem; /* horizontal */
  --ftx-badge-radius: 0.65rem;
  --ftx-avatar: 32px;
  --ftx-action-icon: 0.9rem;

  /* Altura padrão dos controles de formulário */
  --ftx-input-height: 38px;
}
```

**✅ Comentários:**

- Variáveis começam com `--` (sintaxe CSS Custom Properties)
- Valores podem ser sobrescritos em componentes específicos
- Facilita temas dark/light futuros

---

## 2. Header de Card Padrão (ftx-card-header)

### Problema

Criar header consistente para todas as páginas com título, ícone e botões de ação, seguindo identidade visual FrotiX.

### Solução

Classe `.ftx-card-header` com gradiente animado, título estilizado e área de ações flexível.

### Código

```css
/* Container do Header do Card */
.ftx-card-header {
  background: linear-gradient(
    135deg,
    #325d88 0%,
    #2a4d73 25%,
    #3d6f9e 50%,
    #2a4d73 75%,
    #325d88 100%
  );
  background-size: 400% 400%;
  animation: ftxHeaderGradientShift 8s ease infinite;
  padding: 1.25rem 1.5rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 1rem;
  border-radius: 10px 10px 0 0;
}

/* Título do Header */
.ftx-card-header .titulo-paginas {
  font-family: "Outfit", sans-serif !important;
  font-weight: 900 !important;
  font-size: 1.925rem !important;
  color: #ffffff !important;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.3);
}

/* Ícone Duotone - Branco/Laranja */
.ftx-card-header .titulo-paginas i.fa-duotone::before {
  color: #ffffff !important;
}
.ftx-card-header .titulo-paginas i.fa-duotone::after {
  color: #c67750 !important;
}
```

**✅ Comentários:**

- Gradiente animado cria efeito de movimento sutil
- Ícones Font Awesome Duotone com cores específicas
- Responsivo: empilha verticalmente em mobile

---

## 2.1 Header do App - Usuário Logado (ponto + nome)

### Problema

O header principal precisava exibir o usuário logado com padrão FrotiX, incluindo ícone duotone padrão e truncamento seguro.

### Solução

Criar estilos dedicados para o rótulo do usuário no header, com paleta laranja/cinza e texto formatado.

### Código

```css
.ftx-header-user {
  gap: 0.5rem;
  font-family: "Outfit", sans-serif;
  font-weight: 600;
  color: #f8fafc;
}

.ftx-header-user-label {
  color: #f8fafc;
  font-size: 0.92rem;
  letter-spacing: 0.2px;
  max-width: 240px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.ftx-header-user i.fa-duotone {
  --fa-primary-color: #ff6b35;
  --fa-secondary-color: #6c757d;
  --fa-secondary-opacity: 0.9;
  font-size: 1.05rem;
  line-height: 1;
}
```

**✅ Comentários:**

- Exibe o usuário no formato `(ponto.) Nome`
- Ícone duotone usa paleta padrão FrotiX
- Truncamento evita quebra em telas menores

---

## 3. Botão Laranja do Header

### Problema

Botão de ação principal no header deve ser destacado e seguir padrão visual específico.

### Solução

Botão com fundo marrom (#A0522D), borda preta fina + outline branco de 2px, e animações de hover.

### Código

```css
.btn-header-orange,
.btn-fundo-laranja {
  background-color: #a0522d !important;
  color: #fff !important;
  border: 1px solid #333 !important; /* Borda preta fina */
  border-radius: 8px;
  padding: 0.5rem 1.25rem;
  font-family: "Outfit", sans-serif !important;
  font-weight: 600;
  box-shadow:
    0 0 0 2px rgba(255, 255, 255, 0.8),
    /* Borda branca 2px */ 0 0 12px rgba(160, 82, 45, 0.4) !important;
}

.btn-header-orange:hover {
  background-color: #8b4513 !important;
  transform: translateY(-2px);
  box-shadow:
    0 0 0 2px rgba(255, 255, 255, 1),
    0 0 20px rgba(160, 82, 45, 0.6) !important;
}
```

**✅ Comentários:**

- `box-shadow` duplo cria efeito de borda branca externa
- Hover eleva botão (`translateY(-2px)`)
- Glow aumenta no hover para feedback visual

---

## 4. Badges de Status

### Problema

Badges de status (Ativo/Inativo) devem ser clicáveis, visíveis e seguir paleta de cores padrão.

### Solução

Classes `.badge-ativo` e `.badge-inativo` usando variáveis CSS, com hover e transições suaves.

### Código

```css
.badge-ativo {
  background-color: var(--status-active-bg) !important;
  color: #fff !important;
  padding: var(--ftx-badge-pad-v) var(--ftx-badge-pad-h);
  border-radius: var(--ftx-badge-radius);
  font-size: var(--ftx-badge-font);
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  box-shadow: 0 2px 4px var(--status-active-shadow);
}

.badge-ativo:hover {
  background-color: var(--status-active-bg-hover) !important;
  transform: scale(1.05);
}

.badge-inativo {
  background-color: var(--status-inactive-bg) !important;
  color: #fff !important;
  /* ... mesmo padrão ... */
}
```

**✅ Comentários:**

- Usa variáveis CSS para fácil manutenção
- Hover com `scale(1.05)` para feedback tátil
- Cursor pointer indica interatividade

---

## 5. Controles de Formulário (Altura Padrão 38px)

### Problema

Garantir altura uniforme em todos os controles de formulário (inputs, selects, Syncfusion) para alinhamento visual.

### Solução

Aplicar altura padrão via variável CSS e ajustar padding/line-height para centralização vertical.

### Código

```css
.form-control,
.form-select,
.e-ddl.e-input-group,
.e-dropdowntree .e-input-group {
  height: var(--ftx-input-height) !important;
  min-height: var(--ftx-input-height) !important;
  padding: 0.375rem 0.75rem !important;
  font-size: 0.875rem !important;
  border-radius: 0.375rem;
}

/* Selects nativos - centralização com line-height */
.form-select,
select.form-control {
  line-height: 38px !important;
  padding-top: 0 !important;
  padding-bottom: 0 !important;
}

/* Syncfusion - centralização com flexbox */
.e-input-group.e-control-wrapper {
  height: var(--ftx-input-height) !important;
  display: flex !important;
  align-items: center !important;
}
```

**✅ Comentários:**

- `!important` garante que sobrescreve estilos de bibliotecas externas
- Diferentes técnicas para diferentes tipos de controle (line-height vs flexbox)
- Textarea tem altura auto (pode crescer)

---

## 6. Tooltips Syncfusion Customizados

### Problema

Tooltips padrão do Syncfusion têm setas e cores que não seguem design FrotiX.

### Solução

Sobrescrever estilos do Syncfusion com gradiente azul, sem setas, e bordas arredondadas.

### Código

```css
.e-tooltip-wrap {
  background: linear-gradient(135deg, #3d5771 0%, #4a6b8a 100%) !important;
  color: #ffffff !important;
  border: none !important;
  border-radius: 6px !important;
  padding: 6px 12px !important;
  font-size: 12px !important;
  box-shadow: 0 3px 12px rgba(61, 87, 113, 0.35) !important;
  z-index: 99999 !important;
}

/* Ocultar setas */
.e-tooltip-wrap .e-arrow-tip,
.e-tooltip-wrap .e-arrow-tip-outer,
.e-tooltip-wrap .e-arrow-tip-inner {
  display: none !important;
}
```

**✅ Comentários:**

- Gradiente azul seguindo paleta FrotiX
- `z-index` alto garante que aparece sobre outros elementos
- Setas removidas via `display: none`

---

## 7. Botões de Ação em Tabelas (btn-icon-28)

### Problema

Botões de ação em tabelas devem ser pequenos, quadrados, com ícones centralizados e cores temáticas.

### Solução

Classe `.btn-icon-28` com tamanho fixo (28x28px), gradientes por função (editar=azul, excluir=vinho, etc).

### Código

```css
.btn-icon-28,
.ftx-btn-acao {
  width: var(--ftx-icon-btn);
  height: var(--ftx-icon-btn);
  min-width: var(--ftx-icon-btn);
  min-height: var(--ftx-icon-btn);
  padding: 0;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  border-radius: 6px;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-icon-28.btn-azul {
  background: linear-gradient(135deg, #3d5771 0%, #2d4559 100%) !important;
}

.btn-icon-28.btn-vinho {
  background: linear-gradient(135deg, #561d2d 0%, #3d1520 100%) !important;
}

.btn-icon-28.btn-verde {
  background: linear-gradient(135deg, #27ae60 0%, #219a52 100%) !important;
}
```

**✅ Comentários:**

- Tamanho fixo evita "pulos" ao trocar ícones
- Gradientes criam profundidade visual
- Hover com animação `buttonWiggle` para feedback

---

## 8. Animações

### Problema

Fornecer animações suaves e consistentes para feedback visual em interações.

### Solução

Keyframes reutilizáveis para diferentes tipos de animação (spin, pulse, fade, slide, ripple).

### Código

```css
/* Spinner */
@keyframes ftxspin {
  to {
    transform: rotate(360deg);
  }
}

/* Pulse (ícones) */
@keyframes pulseIcon {
  0%,
  100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.2);
  }
}

/* Fade In */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Ripple */
@keyframes ftxRippleAnim {
  from {
    transform: scale(0);
    opacity: 1;
  }
  to {
    transform: scale(2);
    opacity: 0;
  }
}

/* Button Wiggle */
@keyframes buttonWiggle {
  0% {
    transform: translateY(0) rotate(0deg);
  }
  25% {
    transform: translateY(-2px) rotate(-1deg);
  }
  50% {
    transform: translateY(-3px) rotate(0deg);
  }
  75% {
    transform: translateY(-2px) rotate(1deg);
  }
  100% {
    transform: translateY(0) rotate(0deg);
  }
}
```

**✅ Comentários:**

- Animações leves (duração curta) para não distrair
- `transform` é mais performático que mudanças de posição/tamanho
- Ripple usa `scale` + `opacity` para efeito de onda

---

## 9. Spinner Global

### Problema

Exibir indicador de carregamento consistente em toda a aplicação.

### Solução

Overlay fixo com logo FrotiX pulsante e barra de progresso animada.

### Código

```css
.ftx-spin-overlay {
  position: fixed;
  inset: 0;
  z-index: 50000;
  background: rgba(14, 14, 18, 0.82);
  display: flex;
  align-items: center;
  justify-content: center;
  backdrop-filter: saturate(120%) blur(2px);
}

.ftx-spin-box {
  text-align: center;
  padding: 28px 32px;
  border-radius: 16px;
  background: rgba(30, 30, 40, 0.35);
  border: 1px solid rgba(255, 255, 255, 0.08);
  color: #fff;
  min-width: 260px;
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.35);
}

.ftx-loading-logo {
  animation: pulseIcon 1.5s ease-in-out infinite;
}
```

**✅ Comentários:**

- `inset: 0` é equivalente a `top: 0; right: 0; bottom: 0; left: 0`
- `backdrop-filter` cria efeito de blur no fundo
- Logo pulsa para indicar atividade

---

## 10. Responsividade

### Problema

Garantir que componentes funcionem bem em dispositivos móveis.

### Solução

Media queries para ajustar layout, tamanhos de fonte e espaçamentos em telas pequenas.

### Código

```css
@media (max-width: 768px) {
  .ftx-card-header {
    flex-direction: column;
    text-align: center;
    padding: 1rem;
  }

  .ftx-card-header .titulo-paginas {
    font-size: 1.5rem !important;
    justify-content: center;
  }

  .btn-header-orange {
    width: 100%;
    justify-content: center;
  }
}
```

**✅ Comentários:**

- Breakpoint em 768px (tablets e abaixo)
- Header empilha verticalmente
- Botões ocupam largura total em mobile

---

## Estrutura do Arquivo

O arquivo está organizado em seções principais:

1. **Variáveis CSS** (`:root`)
2. **Reset e Base** (`*`, `body`, `html`)
3. **Controles de Formulário** (altura padrão, centralização)
4. **Animações** (keyframes)
5. **Spinner Global** (`.ftx-spin-overlay`)
6. **Header de Card** (`.ftx-card-header`)
7. **Botões** (header laranja, ação, cores)
8. **Badges** (status, contadores)
9. **Formulários** (labels, required, validação)
10. **SweetAlert2** (customizações)
11. **Tooltips** (Syncfusion customizados)
12. **Tabelas** (avatares, ações, espaçamento)
13. **Dropdowns** (motorista com foto)
14. **Responsividade** (media queries)

---

## Troubleshooting

**Estilos não aplicam:** Verificar ordem de carregamento (frotix.css deve vir após Bootstrap/Syncfusion)  
**Altura de inputs inconsistente:** Verificar se variável `--ftx-input-height` está definida  
**Tooltips com setas:** Verificar se regras de `display: none` para `.e-arrow-tip` estão aplicadas  
**Gradiente do header não anima:** Verificar se `animation: ftxHeaderGradientShift` está presente

---

## Referências

- **JavaScript Complementar:** `wwwroot/js/frotix.js`
- **Carregamento:** `Pages/Shared/_Head.cshtml`
- **Fonte:** Google Fonts - Outfit

---

## Changelog

**21/01/2026** - Versão 3.0

- Estilos para usuário logado no header principal
- Ícone duotone padrão aplicado ao rótulo do usuário

**13/01/2026 17:00** - Versão 2.7

- **OFICIALIZAÇÃO**: Classe `btn-verde` agora é OFICIAL no padrão FrotiX
- **Descoberta**: Classe já existia no frotix.css (linhas 1517-1538) mas não estava documentada
- **Implementação completa**: Estados normal, hover e active/focus corretos (não fica branca ao pressionar)
- **Cores**: Verde #38A169 (normal) → #2D7A50 (hover) → #246640 (active)
- **Características**: Mesmos padrões das outras classes (wiggle animation, box-shadow, transições)
- **Diretrizes de Uso**:
  - ✅ **Use `btn-verde` para**: Importar/Processar dados, Confirmar/Aprovar ações positivas, Aplicar sugestões/correções, Ações de sucesso/progresso
  - ✅ **Use `btn-azul` para**: Salvar/Editar registros comuns, Ações principais neutras, Inserir/Atualizar dados padrão
  - ✅ **Use `btn-vinho` para**: Cancelar/Fechar operações, Excluir/Apagar registros, Ações destrutivas
  - ✅ **Use `btn-voltar` para**: Voltar à lista, Retornar à tela anterior
  - ✅ **Use `btn-header-orange` para**: Ações principais em headers de cards
- **Paleta Completa FrotiX**:
  - 🔵 `btn-azul` (#325d88) - Ações principais neutras
  - 🟢 `btn-verde` (#38A169) - Ações de confirmação/sucesso/processamento
  - 🍷 `btn-vinho` (#722f37) - Cancelar/excluir/fechar
  - 🟠 `btn-header-orange` (#A0522D) - Destaque em headers
  - 🟤 `btn-voltar` (#7E583D) - Voltar à lista
  - 🟡 `btn-amarelo` (#f59e0b) - Ações especiais/correções
- **Próximos passos**: Revisar 31+ usos atuais de `btn-verde` para garantir uso correto conforme diretrizes

**13/01/2026 19:15** - Versão 2.9

- **FIX DEFINITIVO**: Corrigidas 4 classes CSS de botões cancelar/fechar que ficavam brancos ao pressionar
- **Problema**: Classes `.btn-ftx-fechar`, `.btn-ftx-voltar`, `.btn-ftx-cancelar` e `.btn-modal-fechar` não tinham `background-color` no estado `:active/:focus`
- **Causa Raiz**: Estados `:active/:focus` tinham apenas `transform` e `box-shadow`, sem definir cor de fundo
- **Solução**: Adicionadas 3 propriedades faltantes em cada classe:
  1. `.btn-ftx-fechar:active/:focus` (linhas 3563-3565) → `background-color: #4a1f24` (vinho escuro)
  2. `.btn-ftx-voltar:active/:focus` (linhas 3603-3605) → `background-color: #4a1f24` (vinho escuro)
  3. `.btn-ftx-cancelar:active/:focus` (linhas 3739-3741) → `background-color: #263238` (cinza escuro)
  4. `.btn-modal-fechar:active/:focus` (linhas 3804-3806) → `background-color: #4a1f24` (vinho escuro)
- **Propriedades adicionadas**: `background-color`, `color: #fff`, `border-color`
- **Padrão de cores**: Cor `:active` é 20% mais escura que `:hover` para feedback visual correto
- **Impacto**: 37 arquivos .cshtml com 54 ocorrências desses botões agora funcionam corretamente
- **Contexto**: Resolve problema sistêmico identificado no modal de agendamento que afetava TODO o sistema
- **Benefícios**:
  - ✅ Todos os botões cancelar/fechar mantêm cor rosada/escura ao pressionar
  - ✅ Comportamento visual consistente em TODO o sistema
  - ✅ Alinhamento com padrão FrotiX (mesmo padrão de `.btn-vinho`)
  - ✅ Resolve definitivamente issue de botões ficando brancos ao clicar
  - ✅ Classes legadas agora funcionam corretamente (não precisam mais ser substituídas)

---

## [16/01/2026 17:15] - Auditoria Global: Campos Obrigatórios (.label-required)

**Descrição**: Adicionado asterisco vermelho em labels de campos mandatórios identificados via lógica de validação (Back/Front).

- Pequenos ajustes de estilização nos botões e labels para suportar o layout ftx-card-styled.

---

**13/01/2026 18:45** - Versão 2.8

- **FIX CRÍTICO**: Adicionadas regras CSS específicas para btn-sm combinado com btn-azul e btn-vinho
- **Problema**: Botões pequenos (btn-sm) com btn-azul ficavam azul claro ao clicar, e btn-vinho ficavam brancos
- **Causa**: Especificidade CSS - Bootstrap sobrescrevia os estilos :active quando btn-sm era usado
- **Solução**: Adicionados seletores mais específicos (.btn-sm.btn-azul:active, .btn-sm.btn-vinho:active)
- **Linhas**: 1276-1293
- **Estilos garantidos**:
  - `.btn-sm.btn-azul:active` → background #1f3241 (azul escuro)
  - `.btn-sm.btn-vinho:active` → background #4a1f24 (vinho escuro)
- **Contexto**: Fix reportado no modal de agendamento (Agenda/Index.cshtml linhas 1418, 1424)
- **Benefícios**:
  - ✅ Botões btn-sm + btn-azul mantêm cor azul escura ao clicar
  - ✅ Botões btn-sm + btn-vinho mantêm cor vinho escura ao clicar
  - ✅ Consistência visual em TODOS os tamanhos de botões
  - ✅ Resolve problemas de especificidade CSS vs Bootstrap

**13/01/2026 17:00** - Versão 2.7

- **OFICIALIZAÇÃO**: Classe `btn-verde` agora é OFICIAL no padrão FrotiX
- **Diretrizes de Uso**:
  - ✅ **Use `btn-verde` para**: Importar/Processar dados, Confirmar/Aprovar ações positivas, Aplicar mudanças
  - ✅ **Use `btn-azul` para**: Salvar/Editar registros comuns, Criar novos registros, Ações principais neutras
  - ✅ **Use `btn-vinho` para**: Cancelar/Fechar operações, Excluir registros, Limpar/Resetar estado
- **Paleta Completa FrotiX**:
  - 🔵 `btn-azul` (#325d88) - Ações principais neutras (Salvar, Criar, Editar)
  - 🟢 `btn-verde` (#38A169) - Ações de confirmação/sucesso/processamento (Importar, Aprovar, Processar)
  - 🍷 `btn-vinho` (#722f37) - Ações destrutivas/saída (Cancelar, Excluir, Fechar, Limpar)
  - 🟠 `btn-header-orange` (#A0522D) - Ações de destaque em headers (Novo Cadastro)
  - 🟤 `btn-voltar` (#7E583D) - Voltar à lista
  - 🟡 `btn-amarelo` (#f59e0b) - Ações especiais/correções
- **Atualização CLAUDE.md**: Paleta oficial adicionada ao guia de desenvolvimento
- **Status do btn-verde**:
  - ✅ Classe já existia no CSS (linhas 1517-1538) com implementação completa
  - ✅ Agora oficialmente documentada com diretrizes claras de uso
  - ✅ ~32 usos no sistema validados e alinhados com as diretrizes

**13/01/2026 15:30** - Versão 2.6

- **PADRONIZAÇÃO MASSIVA**: Substituição de `btn-ftx-fechar` por `btn-vinho` em 37 arquivos
- Total de 46 ocorrências substituídas em todo o sistema
- **Motivo**: Classe `btn-ftx-fechar` não tinha `background-color` no estado `:active`, fazendo botões ficarem brancos ao serem pressionados
- **Solução**: Padronizar todos os botões cancelar/fechar para usar `.btn-vinho` que já funciona corretamente
- **Arquivos afetados**: 37 páginas .cshtml (Pages/Agenda/Index.cshtml, Pages/Veiculo/Upsert.cshtml, etc)
- **Classes legadas (não usadas em HTML)**: `btn-ftx-voltar`, `btn-ftx-cancelar`, `btn-modal-fechar` - permanecem no CSS para compatibilidade
- **Benefícios**:
  - ✅ Todos os botões cancelar/fechar agora têm cor rosada/vinho ao serem pressionados
  - ✅ Comportamento visual consistente em todo o sistema
  - ✅ Alinhamento com padrão FrotiX estabelecido

**13/01/2026 13:45** - Versão 2.5

- **Alteração de cor**: Modal header de Evento (modal-header-azul) alterado para tom bege rosado
- Variável `--modal-azul`: #325d88 → #AA9183
- Variável `--modal-azul-light`: #3d6f9e → #B9A092
- Afeta todos os modais que usam classe `modal-header-azul` (principalmente Modal Novo Evento)
- Mantém gradiente suave entre cor base e cor light
- Linha: 2729-2730

**13/01/2026 00:40** - Versão 2.4

- **CORREÇÃO CRÍTICA**: Adicionadas bordas para TODOS os DatePickers do sistema
- Mesmo padrão usado para DropDownTree (linhas 79-89) agora aplicado a DatePickers
- Novo bloco CSS (linhas 91-104) com seletores completos para e-datepicker
- Resolve problema de bordas ausentes em TODAS as páginas (não só Agenda)
- Bordas: 1px solid #ced4da em todos os lados com !important
- Inclui border-radius: 0.375rem e box-sizing: border-box

**13/01/2026 00:17** - Versão 2.3

- Corrigido seletor CSS que tornava botão "Cancelar Operação" oval
- Seletor `[data-bs-dismiss="modal"]` mudado para `.modal-header [data-bs-dismiss="modal"]`
- Agora apenas botões NO HEADER do modal ficam redondos
- Botões no rodapé mantêm `border-radius: .375rem` padrão
- Linha: 1072-1076

**12/01/2026 19:25** - Versão 2.2

- Corrigido `border-radius` do botão de fechar modal de `.375rem` para `50%`
- Botão agora é perfeitamente redondo em vez de oval
- Seletor: `[data-bs-dismiss="modal"], [data-dismiss="modal"], .modal-header .btn-close`
- Linha: 1073

**12/01/2026** - Versão 2.1

- Ajustada espessura da borda dupla dos botões de 4px para 3px (redução de 25%)
- Adicionada exceção `:not(.ftx-actions *)` para excluir botões da coluna Ações
- Botões de Status (`.ftx-badge-status`) já tinham exceção correta

**08/01/2026** - Versão 2.0 (Padrão FrotiX Simplificado)

- Documentação completa criada
- Todas as seções principais documentadas
- Exemplos de uso adicionados

---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [21/01/2026] - Header do App: usuário logado + ícone duotone

**Descrição**: Incluídos estilos do rótulo de usuário logado no header principal com ícone FontAwesome Duotone e truncamento seguro.

**Arquivos Afetados**:

- wwwroot/css/frotix.css

**Impacto**:

- ✅ Exibição do usuário no formato `(ponto.) Nome`
- ✅ Ícone duotone com paleta padrão laranja/cinza

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:

- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:

- âŒ **ANTES**: \_unitOfWork.Entity.AsTracking().Get(id) ou \_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: \_unitOfWork.Entity.GetWithTracking(id) ou \_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

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
