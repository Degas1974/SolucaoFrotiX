# Documentação: syncfusion_tooltips.js - Sistema Global de Tooltips

> **Última Atualização**: 21/01/2026
> **Versão Atual**: 2.1
> **Padrão**: FrotiX Simplificado

---

## Objetivos

O arquivo **syncfusion_tooltips.js** inicializa e gerencia tooltips globais usando Syncfusion EJ2, substituindo tooltips do Bootstrap 5 e fornecendo interface consistente em toda a aplicação.

**Principais funcionalidades:**
- ✅ **Tooltips globais** para elementos com `data-ejtip`
- ✅ **Remoção automática** de tooltips Bootstrap conflitantes
- ✅ **Suporte a elementos dinâmicos** via MutationObserver
- ✅ **Fechamento automático** após 2 segundos
- ✅ **Design customizado** sem setas, estilo FrotiX

---

## Arquivos Envolvidos

1. **wwwroot/js/syncfusion_tooltips.js** - Arquivo principal (211 linhas)
2. **Pages/Shared/_ScriptsBasePlugins.cshtml** - Carregamento global
3. **Syncfusion EJ2** - Biblioteca externa (ej.popups.Tooltip)

---

## Problema

Bootstrap 5 e Syncfusion podem conflitar ao gerenciar tooltips. Precisamos de sistema único que funcione com elementos estáticos e dinâmicos (DataTables, modais).

## Solução

Criar instância global do Syncfusion Tooltip que detecta elementos com `data-ejtip`, remove atributos Bootstrap, e atualiza automaticamente quando novos elementos são adicionados ao DOM.

---

## Código Principal

```javascript
(function ()
{
    function initializeTooltip()
    {
        // Verifica se o Syncfusion está carregado
        if (typeof ej === 'undefined' || !ej.popups || !ej.popups.Tooltip)
        {
            console.warn('Syncfusion não carregado. Tentando novamente em 500ms...');
            setTimeout(initializeTooltip, 500);
            return;
        }

        // Desabilita tooltips do Bootstrap 5
        document.querySelectorAll('[data-ejtip]').forEach(function (el)
        {
            el.removeAttribute('data-bs-toggle');
            el.removeAttribute('data-bs-original-title');
            el.removeAttribute('title');
            
            if (window.bootstrap?.Tooltip?.getInstance)
            {
                const bsTooltip = window.bootstrap.Tooltip.getInstance(el);
                bsTooltip?.dispose();
            }
        });

        // Cria instância GLOBAL
        window.ejTooltip = new ej.popups.Tooltip({
            target: '[data-ejtip]',
            opensOn: 'Hover',
            position: 'TopCenter',
            showTipPointer: false, // Sem setas
            content: function (args) {
                return args.getAttribute('data-ejtip') || 'Sem descrição';
            },
            afterOpen: function (args) {
                // Fecha automaticamente após 2 segundos
                setTimeout(() => this.close(), 2000);
            }
        });

        window.ejTooltip.appendTo('body');
    }

    // Observer para elementos dinâmicos
    const observer = new MutationObserver(() => {
        document.querySelectorAll('[data-ejtip]').forEach(function (el) {
            el.removeAttribute('data-bs-toggle');
            el.removeAttribute('data-bs-original-title');
            el.removeAttribute('title');
        });
        if (window.ejTooltip) window.ejTooltip.refresh();
    });

    observer.observe(document.body, { childList: true, subtree: true });
})();
```

**✅ Comentários:**
- Aguarda Syncfusion carregar (retry a cada 500ms)
- Remove atributos Bootstrap para evitar conflitos
- Usa MutationObserver para detectar elementos dinâmicos
- Fecha automaticamente após 2s para não poluir tela

---

## Exemplo de Uso

```html
<!-- Tooltip simples -->
<button data-ejtip="Clique para salvar">Salvar</button>

<!-- Tooltip em elemento dinâmico (DataTable) -->
<script>
    // Após criar elemento dinamicamente
    const btn = document.createElement('button');
    btn.setAttribute('data-ejtip', 'Editar registro');
    // Tooltip será aplicado automaticamente
</script>

<!-- Atualizar tooltips manualmente -->
<script>
    refreshTooltips(); // Função global disponível
</script>
```

---

## Troubleshooting

**Tooltips não aparecem:** Verificar se Syncfusion está carregado antes deste arquivo  
**Conflito com Bootstrap:** Sistema remove atributos automaticamente, mas verificar ordem de carregamento  
**Elementos dinâmicos:** MutationObserver deve detectar, mas chamar `refreshTooltips()` se necessário

---

## Referências

- **Syncfusion EJ2:** Biblioteca externa
- **Carregamento:** `Pages/Shared/_ScriptsBasePlugins.cshtml`


---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [21/01/2026] - Correção Crítica: Loop Infinito no MutationObserver

**Descrição**: O MutationObserver estava causando travamento total de modais em múltiplas páginas (Agenda, AlertasFrotiX). O problema era um loop infinito: quando componentes Syncfusion/Kendo modificavam o DOM, o observer disparava, chamava `ejTooltip.refresh()` que modificava o DOM novamente, disparando o observer em loop.

**Alterações**:
1. **Debounce de 500ms**: Observer agora aguarda 500ms antes de processar mutações
2. **Flag anti-reentrância**: Variável `isRefreshing` evita processamento simultâneo
3. **Filtro de relevância**: Só processa mutações com novos elementos `[data-ejtip]`
4. **Proteção null no beforeClose**: Verificação de `args.element` antes de acessar

**Código Adicionado**:
```javascript
let tooltipRefreshTimer = null;
let isRefreshing = false;

const observer = new MutationObserver((mutations) => {
    if (isRefreshing) return;
    // Verificar se há novos elementos com data-ejtip...
    // Debounce de 500ms antes de processar
});
```

**Arquivos Afetados**:
- `wwwroot/js/syncfusion_tooltips.js` (linhas 214-285)

**Impacto**: Resolve travamento completo de modais em todo o sistema

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 2.1

---

## [19/01/2026] - Atualização: Implementação de Métodos com Tracking Seletivo

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
