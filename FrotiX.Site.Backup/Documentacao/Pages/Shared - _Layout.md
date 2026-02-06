# Documentação: Shared - _Layout

> **Última Atualização**: 13/01/2026
> **Versão Atual**: 0.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Frontend](#frontend)
4. [Endpoints API](#endpoints-api)
5. [Validações](#validações)
6. [Troubleshooting](#troubleshooting)

---

## Visão Geral

O arquivo `_Layout.cshtml` é o **template de layout compartilhado principal** do sistema FrotiX Web. Ele define a estrutura HTML base que envolve todas as páginas da aplicação, incluindo header, sidebar, footer e modais globais.

### Características Principais
- ✅ **Layout Responsivo**: Estrutura adaptável para desktop, tablet e mobile
- ✅ **Sidebar de Navegação**: Menu lateral com ícones FontAwesome Duotone
- ✅ **Header Global**: Barra superior com logo, breadcrumbs e notificações
- ✅ **Modais Globais**: Modal de alertas e notificações acessível em todo o sistema
- ✅ **Scripts Compartilhados**: Carregamento de bibliotecas JS comuns (jQuery, Bootstrap, Syncfusion, etc.)
- ✅ **Estilos Globais**: Referências a CSS compartilhado (frotix.css, Bootstrap, FontAwesome)

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/
│   └── Shared/
│       └── _Layout.cshtml          ← Este arquivo
├── wwwroot/
│   ├── css/
│   │   └── frotix.css              ← Estilos globais
│   └── js/
│       ├── frotix.js               ← Scripts globais
│       ├── alerta.js               ← Sistema de alertas
│       └── global-toast.js         ← Notificações toast
```

### Informações de Roteamento

- **Módulo**: `Shared` (compartilhado)
- **Tipo**: Layout Template (Razor Pages)
- **Escopo**: Global - usado por todas as páginas do sistema
- **Rota**: N/A (template, não página acessível)

---

## Frontend

### Assets referenciados

**CSS Globais:**
- Bootstrap 5.3.x
- FontAwesome 6.x Duotone
- Syncfusion EJ2
- `~/css/frotix.css` (estilos customizados FrotiX)

**JavaScript Globais:**
- jQuery 3.x
- Bootstrap 5.x Bundle
- Syncfusion EJ2
- `~/js/frotix.js`
- `~/js/alerta.js`
- `~/js/global-toast.js`

### Componentes Principais

1. **Header**: Logo FrotiX, breadcrumbs, notificações
2. **Sidebar**: Menu de navegação hierárquico com ícones
3. **Content Area**: `@RenderBody()` - área onde páginas individuais são renderizadas
4. **Footer**: Informações de copyright e versão
5. **Modal de Alertas**: Modal global para exibição de alertas do sistema

---

## Endpoints API

> Este é um arquivo de layout, não consome endpoints diretamente. Endpoints são consumidos pelas páginas individuais que utilizam este layout.

---

## Validações

> Este é um arquivo de layout, validações são implementadas nas páginas individuais.

---

## Troubleshooting

### Problema: Estilos não carregam corretamente

**Sintoma**: Página exibida sem estilos ou com layout quebrado

**Causas Possíveis**:
- Arquivo `frotix.css` não encontrado
- Ordem de carregamento incorreta dos arquivos CSS
- Cache do navegador desatualizado

**Solução**:
1. Verificar se `wwwroot/css/frotix.css` existe
2. Limpar cache do navegador (Ctrl+Shift+Delete)
3. Verificar console do navegador para erros 404

---

### Problema: Scripts JavaScript não funcionam

**Sintoma**: Funcionalidades que dependem de JS não respondem (alertas, modais, etc.)

**Causas Possíveis**:
- Ordem de carregamento de scripts incorreta
- Erro em algum script global bloqueando execução

**Solução**:
1. Abrir console do navegador (F12)
2. Verificar erros JavaScript
3. Garantir que jQuery carrega antes de outros scripts

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [13/01/2026 18:00] - Fase 3: Padronização btn-secondary → btn-vinho

**Descrição**: Substituída classe Bootstrap genérica `btn-secondary` por `btn-vinho` (padrão FrotiX oficial) em modal global de alertas.

**Problema Identificado**:
- Modal global de alertas usando `btn-secondary` para botão de fechar
- Inconsistência com padrão FrotiX que define `btn-vinho` para ações de fechar/cancelar
- Falta de padronização visual em modais globais

**Solução Implementada**:
- Modal de alertas (linha 345): botão "Fechar" mudado de `btn-secondary` para `btn-vinho`
- Alinhamento com diretrizes FrotiX: botões de fechar/cancelar SEMPRE usam `btn-vinho`
- Consistência com outras 8 correções aplicadas em todo o sistema (Fase 3)

**Arquivos Afetados**:
- Pages/Shared/_Layout.cshtml (linha 345)

**Impacto**:
- ✅ Botão mantém cor vinho consistente ao pressionar
- ✅ Alinhamento com padrão visual FrotiX
- ✅ Consistência em modais globais e locais
- ✅ Usado por todas as páginas do sistema

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 0.1

---

## [13/01/2026 18:00] - Criação da documentação

**Descrição**: Criada documentação inicial do arquivo _Layout.cshtml.

**Status**: ✅ **Concluído**

**Versão**: 0.1


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
