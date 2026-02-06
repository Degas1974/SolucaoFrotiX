# Documentação: Shared - \_Compose

> **Última Atualização**: 23/01/2026
> **Versão Atual**: 1.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

Partial responsável por renderizar o painel fixo de composição de mensagens, com header, ações e conteúdo interno delegados ao `_ComposeLayout`.

## Estrutura

- **Container principal** posicionado no canto inferior direito com tamanho fixo.
- **Header desktop** com ações de fullscreen e fechamento.
- **Header mobile** com título e botão de cancelamento.
- **Conteúdo** injetado via `partial` `_ComposeLayout`.

## Dependências

- HTML/CSS do tema (classes `panel`, `bg-fusion-600`, etc.).
- `_ComposeLayout.cshtml` para o conteúdo interno.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [23/01/2026 10:20] - Padronização de ícones duotone

**Descrição**:
Atualização dos ícones do cabeçalho para FontAwesome Duotone.

**Arquivos Afetados**:

- `Pages/Shared/_Compose.cshtml`

**Impacto**: Conformidade com o padrão visual FrotiX.

**Status**: ✅ Concluído

**Versão**: 1.1

---

## [23/01/2026 09:50] - Card de identidade do arquivo

**Descrição**:
Inclusão do card ASCII no topo do partial.

**Arquivos Afetados**:

- `Pages/Shared/_Compose.cshtml`

**Impacto**: Documentação intra-código padronizada.

**Status**: ✅ Concluído

**Versão**: 1.0
