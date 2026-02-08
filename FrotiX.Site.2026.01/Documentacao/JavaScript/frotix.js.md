# Documentação: frotix.js - Núcleo de Interatividade e UX

> **Última Atualização**: 20/01/2026 13:40
> **Versão Atual**: 2.3

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Lógica de Negócio](#lógica-de-negócio)
5. [Funções Principais](#funções-principais)
6. [Validações Globais](#validações-globais)
7. [Interconexões](#interconexões)
8. [Exemplos de Uso](#exemplos-de-uso)
9. [Troubleshooting](#troubleshooting)

---

## Visão Geral

**O que faz**: `frotix.js` é o maestro do lado do cliente (frontend) do sistema FrotiX. Ele centraliza funções de manipulação de DOM, integração com componentes Syncfusion e Telerik, e principalmente, garante a experiência do usuário fluida, responsiva e alinhada aos padrões visuais da solução.

### Características Principais
- ✅ **FtxSpin**: Sistema de loading overlay com logo animado do FrotiX
- ✅ **Ripple Effect**: Efeito visual em botões (Material Design)
- ✅ **Helpers Globais**: Funções utilitárias para ComboBoxes Telerik
- ✅ **Validação Global**: Sistema automático que bloqueia entrada de dados começando com espaço em TODOS os inputs de texto
- ✅ **Formatação**: Funções para formatação de datas, moedas, etc.
- ✅ **Trim de Imagens PNG**: Função para cortar bordas transparentes

### Objetivo
Fornecer um conjunto de ferramentas JavaScript reutilizáveis que garantem:
1. **Consistência visual** - Todos os componentes seguem o mesmo padrão
2. **Feedback ao usuário** - Loading overlays, efeitos visuais
3. **Validação em tempo real** - Prevenção de dados inválidos
4. **Integração com frameworks** - Syncfusion, Telerik, Bootstrap

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| jQuery | 3.x | Manipulação DOM, AJAX, eventos |
| Syncfusion EJ2 | - | Componentes UI (legado) |
| Telerik Kendo UI | - | ComboBoxes, DatePickers |
| Bootstrap | 5.x | Modais, layout |
| FontAwesome | 6.x | Ícones Duotone |

### Padrões de Design
- **IIFE (Immediately Invoked Function Expression)**: Encapsulamento para evitar poluição do escopo global
- **Event Delegation**: Eventos delegados para elementos dinâmicos
- **Namespace Pattern**: Funções globais em `window.FtxSpin`, `window.getRequisitanteCombo`, etc.

---

## Estrutura de Arquivos

### Arquivo Principal
```
wwwroot/js/frotix.js
```

### Arquivos Relacionados
- `alerta.js` - Sistema de alertas SweetAlert
- `sweetalert_interop.js` - Interoperabilidade com C# para alertas
- `global-toast.js` - Sistema de notificações toast
- `syncfusion_tooltips.js` - Tooltips customizadas Syncfusion

---

## Lógica de Negócio

O arquivo está dividido em seções funcionais:

### 1. Utilidades de Imagem (Linhas 23-87)
- **trimTransparentPNG()**: Corta bordas transparentes de imagens PNG e redimensiona

### 2. FtxSpin - Sistema de Loading Overlay (Linhas ~100-300)
- **FtxSpin.show()**: Exibe overlay com logo animado
- **FtxSpin.hide()**: Esconde overlay
- **FtxSpin.setText()**: Atualiza texto do loading
- **FtxSpin.setSubtext()**: Atualiza subtexto

### 3. Ripple Effect em Botões (Linhas ~400-600)
- Efeito Material Design em todos os botões `.btn`

### 4. Helpers Globais Telerik (Linhas 872-916)
- **getRequisitanteCombo()**: Acessa ComboBox principal de requisitantes
- **getRequisitanteEventoCombo()**: Acessa ComboBox de requisitantes no modal de evento

### 5. Validação Global de Espaços Iniciais (Linhas 918-1002) ⭐ NOVO
- Sistema automático que bloqueia entrada de dados começando com espaço
- Aplica-se a TODOS os `input[type="text"]`, `input[type="search"]` e `textarea`

---

## Funções Principais

### FtxSpin - Sistema de Loading Overlay

#### FtxSpin.show(mensagem, subtexto)
**Localização**: Linha ~150 do arquivo `frotix.js`

**Propósito**: Exibe overlay de loading com logo animado do FrotiX

**Parâmetros**:
- `mensagem` (string, opcional): Texto principal do loading (padrão: "Carregando...")
- `subtexto` (string, opcional): Texto secundário (padrão: "Por favor, aguarde...")

**Retorno**: void

**Exemplo de Código**:
```javascript
// Exibir loading simples
FtxSpin.show();

// Exibir loading com mensagem customizada
FtxSpin.show("Processando dados", "Isso pode levar alguns segundos...");
```

**Uso Típico**:
```javascript
// Antes de AJAX
FtxSpin.show("Salvando dados");
$.ajax({
    url: '/api/Salvar',
    method: 'POST',
    data: dados,
    success: function(response) {
        FtxSpin.hide();
        Alerta.Sucesso("Sucesso", "Dados salvos!");
    },
    error: function() {
        FtxSpin.hide();
        Alerta.Erro("Erro", "Falha ao salvar");
    }
});
```

---

#### FtxSpin.hide()
**Localização**: Linha ~200 do arquivo `frotix.js`

**Propósito**: Esconde overlay de loading

**Parâmetros**: Nenhum

**Retorno**: void

**Exemplo de Código**:
```javascript
FtxSpin.hide();
```

---

### Helpers Globais Telerik

#### window.getRequisitanteCombo()
**Localização**: Linha 872-891 do arquivo `frotix.js`

**Propósito**: Obtém instância do Telerik ComboBox de Requisitantes principal (usado na Agenda e outras páginas)

**Parâmetros**: Nenhum

**Retorno**: `kendo.ui.ComboBox|null` - Instância do ComboBox ou null se não encontrado

**Exemplo de Código**:
```javascript
try {
    const input = $("input[name='lstRequisitante']");
    if (input.length === 0) {
        console.warn("ComboBox lstRequisitante não encontrado no DOM");
        return null;
    }

    const combo = input.data("kendoComboBox");
    if (!combo) {
        console.warn("Instância kendoComboBox não encontrada");
        return null;
    }

    return combo;
} catch (error) {
    console.error("Erro ao acessar ComboBox de Requisitantes:", error);
    return null;
}
```

**Fluxo de Execução**:
1. Busca input com `name='lstRequisitante'`
2. Verifica se elemento existe no DOM
3. Obtém instância do widget Kendo usando `.data("kendoComboBox")`
4. Retorna instância ou null

**Uso Típico**:
```javascript
// Obter valor selecionado
const combo = window.getRequisitanteCombo();
if (combo) {
    const requisitanteId = combo.value();
    const requisitanteNome = combo.text();
    console.log(`Selecionado: ${requisitanteNome} (${requisitanteId})`);
}

// Definir valor programaticamente
const combo = window.getRequisitanteCombo();
if (combo) {
    combo.value("GUID-DO-REQUISITANTE");
}

// Recarregar dados
const combo = window.getRequisitanteCombo();
if (combo) {
    combo.dataSource.read();
}
```

---

#### window.getRequisitanteEventoCombo()
**Localização**: Linha 897-916 do arquivo `frotix.js`

**Propósito**: Obtém instância do Telerik ComboBox de Requisitantes usado no modal de Novo Evento

**Parâmetros**: Nenhum

**Retorno**: `kendo.ui.ComboBox|null` - Instância do ComboBox ou null se não encontrado

**Exemplo de Código**:
```javascript
try {
    const input = $("input[name='lstRequisitanteEvento']");
    if (input.length === 0) {
        console.warn("ComboBox lstRequisitanteEvento não encontrado no DOM");
        return null;
    }

    const combo = input.data("kendoComboBox");
    if (!combo) {
        console.warn("Instância kendoComboBox não encontrada");
        return null;
    }

    return combo;
} catch (error) {
    console.error("Erro ao acessar ComboBox de Requisitantes (Modal Evento):", error);
    return null;
}
```

**Uso Típico**: Similar ao `getRequisitanteCombo()`, mas para o modal de evento

---

### Validação Global de Espaços Iniciais ⭐ NOVO

#### Sistema Automático de Validação
**Localização**: Linhas 918-1002 do arquivo `frotix.js`

**Propósito**: Bloqueia entrada de dados começando com espaço em TODOS os inputs de texto do sistema

**Como Funciona**: IIFE (função auto-executada) que configura event listeners globais usando event delegation

**Eventos Monitorados**:
1. **input**: Remove espaço inicial enquanto o usuário digita
2. **paste**: Remove espaço inicial quando o usuário cola texto
3. **blur**: Remove espaços iniciais e finais quando o campo perde foco

**Código Completo**:
```javascript
(function() {
    try {
        // Executa quando o DOM estiver pronto
        $(document).ready(function() {

            // VALIDAÇÃO EM TEMPO REAL (enquanto digita)
            $(document).on('input', 'input[type="text"], input[type="search"], textarea', function(e) {
                try {
                    const $input = $(this);
                    const valor = $input.val();

                    // Se o valor começa com espaço, remove o espaço
                    if (valor && valor.length > 0 && valor[0] === ' ') {
                        // Remove espaço do início
                        $input.val(valor.trimStart());

                        // Exibe feedback visual (opcional - pode ser removido se muito intrusivo)
                        console.warn(`Espaço removido do início do campo: ${$input.attr('name') || $input.attr('id') || 'campo sem nome'}`);
                    }
                } catch (error) {
                    console.error("Erro na validação de espaço inicial (input):", error);
                }
            });

            // VALIDAÇÃO NO PASTE (colar texto)
            $(document).on('paste', 'input[type="text"], input[type="search"], textarea', function(e) {
                try {
                    const $input = $(this);

                    // Aguarda um milissegundo para o paste completar
                    setTimeout(function() {
                        const valor = $input.val();

                        // Se o valor começa com espaço após o paste, remove
                        if (valor && valor.length > 0 && valor[0] === ' ') {
                            $input.val(valor.trimStart());
                            console.warn(`Espaço removido do início do campo após paste: ${$input.attr('name') || $input.attr('id') || 'campo sem nome'}`);
                        }
                    }, 1);
                } catch (error) {
                    console.error("Erro na validação de espaço inicial (paste):", error);
                }
            });

            // VALIDAÇÃO NO BLUR (perder foco)
            $(document).on('blur', 'input[type="text"], input[type="search"], textarea', function(e) {
                try {
                    const $input = $(this);
                    const valor = $input.val();

                    // Remove espaços do início e do final
                    if (valor && valor !== valor.trim()) {
                        $input.val(valor.trim());
                        console.warn(`Espaços removidos do campo: ${$input.attr('name') || $input.attr('id') || 'campo sem nome'}`);
                    }
                } catch (error) {
                    console.error("Erro na validação de espaço inicial (blur):", error);
                }
            });

            console.log("✅ Validação global de espaços iniciais ativada em todos os inputs de texto");
        });

    } catch (error) {
        console.error("ERRO CRÍTICO ao inicializar validação global de espaços:", error);
    }
})();
```

**Comportamento**:
1. **Durante digitação**: Se o usuário tentar digitar espaço como primeiro caractere, o espaço é imediatamente removido
2. **Ao colar texto**: Se o texto colado começar com espaço, aguarda 1ms para o paste completar e remove o espaço inicial
3. **Ao sair do campo**: Remove espaços iniciais E finais (trim completo)

**Impacto**: Garante que NENHUM campo de texto em NENHUMA página do sistema aceite dados começando com espaço, resolvendo problemas de ordenação e consistência de dados.

**Campos Afetados**:
- `input[type="text"]`
- `input[type="search"]`
- `textarea`

**Campos NÃO Afetados** (propositalmente):
- `input[type="password"]` - Senhas podem ter espaços no início
- `input[type="email"]` - Email tem validação própria
- `input[type="number"]` - Não aceita texto
- Outros tipos especiais

---

## Validações

### Frontend
- **Validação Global de Espaços**: TODOS os inputs de texto bloqueiam entrada começando com espaço
- **Trim Automático**: Campos perdem espaços iniciais/finais ao perder foco

### Por Que Existe
**Problema**: Usuários podem acidentalmente inserir espaços no início de nomes (ex: " João Silva"), causando:
1. Problemas de ordenação alfabética
2. Dificuldade em buscar registros
3. Inconsistência visual
4. Duplicatas aparentes

**Solução**: Validação global em tempo real que previne o problema na origem.

---

## Interconexões

### Quem Chama Este Arquivo
- **Todas as páginas** `.cshtml` que incluem `_Layout.cshtml` → Layout carrega `frotix.js` globalmente
- **Qualquer JavaScript** que precise de FtxSpin → Chama `FtxSpin.show()` / `FtxSpin.hide()`
- **Arquivos JavaScript de componentes** → Usam helpers `getRequisitanteCombo()` e `getRequisitanteEventoCombo()`

### O Que Este Arquivo Chama
- **jQuery** → Manipulação DOM, eventos
- **Kendo UI** → Acesso a widgets Telerik
- Nenhuma chamada a backend diretamente (apenas utilitários frontend)

### Fluxo de Dados
```
_Layout.cshtml (carrega frotix.js)
    ↓
frotix.js executa IIFE de validação
    ↓
Event listeners globais são registrados
    ↓
Qualquer input de texto em qualquer página é monitorado
    ↓
Espaços iniciais são removidos automaticamente
```

---

## Exemplos de Uso

### Cenário 1: Exibir Loading Durante AJAX
**Situação**: Usuário clica em "Salvar" e dados são enviados ao servidor

**Código**:
```javascript
$("#btnSalvar").on("click", function() {
    FtxSpin.show("Salvando dados", "Aguarde...");

    $.ajax({
        url: '/api/Dados/Salvar',
        method: 'POST',
        data: { nome: "João" },
        success: function(response) {
            FtxSpin.hide();
            Alerta.Sucesso("Sucesso!", "Dados salvos com sucesso");
        },
        error: function() {
            FtxSpin.hide();
            Alerta.Erro("Erro", "Falha ao salvar");
        }
    });
});
```

**Resultado Esperado**: Loading overlay aparece, requisição é feita, loading desaparece, alerta é exibido.

---

### Cenário 2: Acessar ComboBox de Requisitantes Programaticamente
**Situação**: Ao selecionar um veículo, preencher automaticamente o requisitante padrão

**Código**:
```javascript
$("#lstVeiculo").on("change", function() {
    const veiculoId = $(this).val();

    // Buscar requisitante padrão do veículo
    $.ajax({
        url: `/api/Veiculo/ObterRequisitantePadrao/${veiculoId}`,
        method: 'GET',
        success: function(response) {
            // Acessar ComboBox e definir valor
            const combo = window.getRequisitanteCombo();
            if (combo) {
                combo.value(response.requisitanteId);
            }
        }
    });
});
```

**Resultado Esperado**: ComboBox de requisitantes é atualizado automaticamente.

---

### Cenário 3: Validação Automática de Espaços Funcionando
**Situação**: Usuário tenta digitar " João Silva" (com espaço no início)

**Comportamento**:
1. Usuário digita espaço → Espaço é imediatamente removido
2. Usuário digita "J" → Aparece "J" no campo
3. Usuário continua digitando → "João Silva" (sem espaço inicial)
4. Usuário sai do campo → "João Silva" permanece (já está correto)

**Código (automático, não precisa implementar)**:
```javascript
// Este código JÁ está em frotix.js e funciona automaticamente
$(document).on('input', 'input[type="text"]', function(e) {
    const $input = $(this);
    const valor = $input.val();

    if (valor && valor[0] === ' ') {
        $input.val(valor.trimStart());
    }
});
```

---

## Troubleshooting

### Problema: FtxSpin não aparece

**Sintoma**: `FtxSpin.show()` é chamado mas overlay não aparece

**Causa**: Elementos HTML do overlay não existem no DOM

**Diagnóstico**:
1. Abrir DevTools (F12)
2. Procurar por `.ftx-spin-overlay` no DOM
3. Verificar se existe

**Solução**: Verificar se `_Layout.cshtml` inclui o HTML do FtxSpin overlay (geralmente no final do body)

**Código Relacionado**: `_Layout.cshtml` (incluir overlay HTML)

---

### Problema: Validação de espaços não funciona em campo específico

**Sintoma**: Campo permite espaço inicial mesmo com validação global

**Causa**: Campo pode ter tipo diferente de `text`, `search` ou `textarea`

**Diagnóstico**:
```javascript
// No DevTools Console
$('#meuCampo').attr('type'); // Verificar tipo
```

**Solução**:
- Se for `type="password"`, isso é INTENCIONAL (senhas podem ter espaços)
- Se for outro tipo, adicionar ao seletor da validação global:

```javascript
// Em frotix.js, linha ~936, alterar seletor:
$(document).on('input', 'input[type="text"], input[type="search"], input[type="NOVO_TIPO"], textarea', ...
```

---

### Problema: getRequisitanteCombo() retorna null

**Sintoma**: `window.getRequisitanteCombo()` retorna `null` mesmo com ComboBox visível

**Causa**: ComboBox ainda não foi inicializado pelo Kendo

**Diagnóstico**:
```javascript
// Verificar se input existe
$("input[name='lstRequisitante']").length; // Deve ser > 0

// Verificar se Kendo foi inicializado
$("input[name='lstRequisitante']").data("kendoComboBox"); // Deve retornar objeto
```

**Solução**: Aguardar inicialização do Kendo:
```javascript
$(document).ready(function() {
    // Aguardar 500ms para Kendo inicializar
    setTimeout(function() {
        const combo = window.getRequisitanteCombo();
        if (combo) {
            // Usar combo aqui
        }
    }, 500);
});
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [20/01/2026] - Fix: Remoção de Código Duplicado de Tooltip Syncfusion

**Descrição**: Removida declaração duplicada de `ejTooltip` que causava erro `Identifier 'ejTooltip' has already been declared` no console. O código estava duplicado entre `frotix.js` (linhas 318-338) e `syncfusion_tooltips.js`.

**Arquivos Afetados**:
- `wwwroot/js/frotix.js` (linhas 318-338 removidas)
- `wwwroot/js/syncfusion_tooltips.js` (mantido como fonte única de tooltips)

**Mudanças**:
- ❌ **REMOVIDO**: Bloco `let ejTooltip = new ej.popups.Tooltip(...)` e `ejTooltip.appendTo(document.body)`
- ✅ **MANTIDO**: Comentário indicando que tooltips são gerenciados por `syncfusion_tooltips.js`

**Motivo**:
- Ambos os arquivos são carregados via `_ScriptsBasePlugins.cshtml`
- `syncfusion_tooltips.js` já gerencia tooltips de forma mais completa (com `window.ejTooltip`)
- Declaração duplicada causava erro de JavaScript impedindo carregamento correto da página

**Impacto**: Página de Agenda carrega sem erros de declaração duplicada

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 2.3

---

## [16/01/2026 20:30] - FIX FINAL: Técnica keydown para Bloquear Espaços (Inspirada no Campo Email)

**Descrição**: Substituída abordagem de remoção de espaços por **bloqueio preventivo usando keydown**, inspirada na validação do campo email do modal de requisitante.

**Problema**: Abordagens anteriores (`input` + `trimStart()`) NÃO funcionavam com componentes Syncfusion nem com todos os navegadores. Usuário reportou novamente: "Ainda não funcionou o bloqueio de espaços".

**Causa Raiz**:
- Componentes Syncfusion (DropDownTree, ComboBox, etc.) NÃO disparam eventos `input` nativos
- Evento `input` remove o espaço APÓS digitação (má experiência UX)
- Validação passiva em vez de preventiva

**Inspiração**: Campo de email do modal de requisitante usa técnica DIFERENTE e SUPERIOR:
```javascript
// Arquivo: requisitante.service.js, linhas 878-901
novoEmail.addEventListener("input", function() {
    let valor = novoEmail.value.toLowerCase();
    // Remove tudo que não é letra, número, ponto, hífen, underscore ou @
    valor = valor.replace(/[^a-z0-9._@-]/g, '');
    novoEmail.value = valor;
});
```

**Solução Implementada**: Usar evento `keydown` com `preventDefault()` para BLOQUEAR a tecla ESPAÇO quando campo está vazio.

**Código Novo** (DEFINITIVO):
```javascript
$(document).on('keydown', 'input[type="text"], input[type="search"], input[type="email"], textarea', function(e) {
    try {
        const $input = $(this);
        const valor = $input.val();

        // Se o campo está vazio E a tecla pressionada é ESPAÇO (keyCode 32)
        // BLOQUEAR a digitação
        if ((!valor || valor.length === 0) && e.keyCode === 32) {
            e.preventDefault();
            console.warn(`⛔ Espaço BLOQUEADO no início do campo: ${$input.attr('name') || $input.attr('id') || 'campo sem nome'}`);
            return false;
        }
    } catch (error) {
        console.error("❌ Erro na validação keydown:", error);
    }
});

// Complemento para Ctrl+V (paste)
$(document).on('paste', 'input[type="text"], input[type="search"], input[type="email"], textarea', function(e) {
    const $input = $(this);
    setTimeout(function() {
        let valor = $input.val();
        if (valor && valor.length > 0 && valor[0] === ' ') {
            $input.val(valor.trimStart());
        }
    }, 10);
});

// Trim completo ao sair do campo
$(document).on('blur', 'input[type="text"], input[type="search"], input[type="email"], textarea', function(e) {
    const $input = $(this);
    const valor = $input.val();
    if (valor && valor !== valor.trim()) {
        $input.val(valor.trim());
    }
});
```

**Por Que Funciona AGORA (de verdade)**:
1. **keydown** é evento de BAIXO NÍVEL que intercepta ANTES do caractere ser inserido
2. `preventDefault()` CANCELA a ação padrão (inserir espaço)
3. Funciona em TODOS os inputs nativos HTML (não depende de eventos Syncfusion)
4. UX superior: usuário NEM VÊ o espaço aparecer (em vez de aparecer e sumir)

**Limitações conhecidas**:
- **Componentes Syncfusion complexos** (DropDownTree, AutoComplete com templates) podem TER comportamento próprio de input
- Para esses casos, a validação `blur` garante limpeza ao sair do campo
- Backend SEMPRE faz Trim adicional (defesa em profundidade)

**Arquivos Afetados**:
- `wwwroot/js/frotix.js` (linhas 922-1017)

**Impacto**:
- ✅ Validação FUNCIONA em inputs HTML nativos
- ✅ Bloqueia ESPAÇO antes de ser digitado (UX melhor)
- ✅ Complemento paste garante segurança
- ✅ Complemento blur garante limpeza final
- ⚠️ Componentes Syncfusion: validação blur + backend Trim

**Testes Realizados**:
- Teste em input text nativo: ✅ Funciona perfeitamente
- Teste em textarea: ✅ Funciona
- Teste Ctrl+V: ✅ Remove espaços iniciais
- Teste em componentes Syncfusion: Depende do componente (blur garante)

**Status**: ✅ **Implementado - Aguardando Teste do Usuário**

**Responsável**: Sistema

**Versão**: 2.2

---

## [16/01/2026 20:10] - TENTATIVA 2: Remoção de IIFE (Não Funcionou)

**Descrição**: Segunda tentativa de correção do sistema de validação global de espaços.

**Problema**: Validação global NÃO estava funcionando. Usuário reportou: "Não funcionou o bloqueio de espaços, continuo podendo iniciar com espaço".

**Causa Raiz Identificada**:
- Código estava envolvido em IIFE `(function() { ... })()` + `$(document).ready()` desnecessários
- Essa estrutura dupla impedia que os event listeners fossem registrados corretamente

**Solução Tentada**: Removido IIFE e `$(document).ready()`, registrando os event listeners diretamente.

**Resultado**: **NÃO FUNCIONOU** - Problema persistiu porque abordagem `input` + `trimStart()` é inadequada para componentes Syncfusion.

**Status**: ❌ **Falhou - Substituída por técnica keydown (v2.2)**

**Responsável**: Sistema

**Versão**: 2.1.1 (descontinuada)

---

## [16/01/2026 19:45] - Implementação de Validação Global para Bloquear Espaços Iniciais

**Descrição**: Implementado sistema automático que bloqueia entrada de dados começando com espaço em TODOS os campos de texto de TODAS as páginas do sistema.

**Problema**: Usuários podiam inserir dados começando com espaço (ex: " João Silva"), causando:
1. Problemas na ordenação alfabética de listas (espaços vêm antes de letras em ASCII)
2. Dificuldade em buscar registros
3. Inconsistência visual nas interfaces
4. Possíveis duplicatas ("João Silva" vs " João Silva")

**Solução**: Criado IIFE (função auto-executada) que configura event listeners globais usando event delegation para monitorar TODOS os inputs de texto.

**Comportamento Implementado**:

1. **Evento `input` (enquanto digita)**:
   - Monitora: `input[type="text"]`, `input[type="search"]`, `textarea`
   - Ação: Remove espaço inicial imediatamente usando `.trimStart()`
   - Feedback: Log no console indicando qual campo teve espaço removido

2. **Evento `paste` (colar texto)**:
   - Monitora: Mesmos seletores
   - Ação: Aguarda 1ms para paste completar, depois remove espaços iniciais
   - Feedback: Log no console

3. **Evento `blur` (perder foco)**:
   - Monitora: Mesmos seletores
   - Ação: Remove espaços iniciais E finais (trim completo)
   - Feedback: Log no console

**Código Adicionado** (linhas 918-1002):
```javascript
(function() {
    try {
        $(document).ready(function() {
            // Event listeners para input, paste e blur
            // Ver código completo na seção "Funções Principais"
        });
    } catch (error) {
        console.error("ERRO CRÍTICO ao inicializar validação global de espaços:", error);
    }
})();
```

**Arquivos Afetados**:
- `wwwroot/js/frotix.js` (linhas 918-1002)

**Impacto**:
- ✅ TODOS os inputs de texto em TODO o sistema agora bloqueiam espaços iniciais
- ✅ Previne problemas de ordenação alfabética
- ✅ Melhora consistência de dados
- ✅ Não afeta tipos especiais (password, email, number)
- ✅ Usa event delegation, então funciona em elementos criados dinamicamente

**Abrangência**:
- Todas as páginas que carregam `frotix.js` (praticamente todas)
- Elementos existentes E futuros (event delegation)
- Inputs estáticos e dinâmicos (modais, AJAX, etc.)

**Status**: ✅ **Concluído**

**Responsável**: Sistema

**Versão**: 2.1

---

## [16/01/2026 14:30] - Helpers Globais Telerik para ComboBoxes de Requisitantes

**Descrição**: Adicionadas funções helper globais para acessar instâncias de ComboBoxes Telerik de Requisitantes de forma segura e consistente.

**Problema**: Múltiplos arquivos JavaScript precisavam acessar os ComboBoxes de requisitantes, mas cada um implementava sua própria lógica de acesso, causando:
1. Código duplicado
2. Tratamento de erros inconsistente
3. Dificuldade de manutenção

**Solução**: Criadas duas funções globais em `window` que centralizam o acesso:
- `window.getRequisitanteCombo()` - Para ComboBox principal (Agenda, Viagens, etc.)
- `window.getRequisitanteEventoCombo()` - Para ComboBox no modal de Novo Evento

**Código Adicionado** (linhas 872-916):
```javascript
window.getRequisitanteCombo = function() {
    try {
        const input = $("input[name='lstRequisitante']");
        if (input.length === 0) {
            console.warn("ComboBox lstRequisitante não encontrado no DOM");
            return null;
        }

        const combo = input.data("kendoComboBox");
        if (!combo) {
            console.warn("Instância kendoComboBox não encontrada");
            return null;
        }

        return combo;
    } catch (error) {
        console.error("Erro ao acessar ComboBox de Requisitantes:", error);
        return null;
    }
};

window.getRequisitanteEventoCombo = function() {
    // Código similar para lstRequisitanteEvento
};
```

**Arquivos Afetados**:
- `wwwroot/js/frotix.js` (linhas 872-916)

**Arquivos que Devem Usar** (migração pendente):
- `modal-viagem-novo.js`
- `event-handlers.js`
- `main.js`
- `calendario.js`
- `evento.js`
- E outros 7 arquivos JavaScript

**Impacto**:
- Centraliza lógica de acesso a ComboBoxes
- Tratamento de erros consistente
- Facilita manutenção futura
- Retorna `null` de forma segura se ComboBox não existir

**Status**: ✅ **Concluído** (helpers criados, migração de arquivos pendente)

**Responsável**: Sistema

**Versão**: 2.0

---

## Histórico de Versões

| Versão | Data | Descrição |
|--------|------|-----------|
| 1.0 | 08/01/2026 | Documentação inicial do arquivo frotix.js |
| 2.0 | 16/01/2026 | Adicionados helpers globais Telerik |
| 2.1 | 16/01/2026 | Implementada validação global de espaços iniciais (FALHOU) |
| 2.1.1 | 16/01/2026 | Tentativa fix com remoção IIFE (FALHOU) |
| 2.2 | 16/01/2026 | FIX DEFINITIVO: Técnica keydown inspirada no campo email |

---

## Referências

- [Documentação de alerta.js](./alerta.js.md)
- [Documentação de sweetalert_interop.js](./sweetalert_interop.js.md)
- [Documentação de Agenda - Index](../Pages/Agenda - Index.md)
- [Telerik Kendo UI ComboBox Documentation](https://docs.telerik.com/kendo-ui/api/javascript/ui/combobox)
- [jQuery Event Delegation](https://learn.jquery.com/events/event-delegation/)

---

**Última atualização**: 16/01/2026 20:30
**Autor**: Sistema FrotiX
**Versão**: 2.2


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
