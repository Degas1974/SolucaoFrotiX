# Documentação: Shared - \_ColorProfileReference

> **Última Atualização**: 23/01/2026
> **Versão Atual**: 1.2

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

Partial que expõe classes de cor em um bloco oculto para consumo por scripts/clientes. Ele garante que os tokens de cor (primary, info, danger, warning, success, fusion) estejam presentes no DOM para leitura dinâmica.

## Estrutura

- `<p id="js-color-profile" class="d-none">` como contêiner oculto.
- Spans com classes de variações 50–900 para cada grupo de cor.

## Uso esperado

- Scripts podem ler cores computadas via `getComputedStyle` a partir das classes.
- Facilita integração de temas e gráficos sem hardcode de valores.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [23/01/2026 09:50] - Ajuste de card no partial

**Descrição**:
Ajuste do card ASCII no topo do partial para padronização do conteúdo.

**Arquivos Afetados**:

- `Pages/Shared/_ColorProfileReference.cshtml`

**Impacto**: Documentação intra-código alinhada ao padrão FrotiX.

**Status**: ✅ Concluído

**Versão**: 1.2

---

## [23/01/2026 09:40] - Card de identidade do arquivo

**Descrição**:
Inclusão de card ASCII no topo do partial para documentação intra-código.

**Arquivos Afetados**:

- `Pages/Shared/_ColorProfileReference.cshtml`

**Impacto**: Padrão de documentação interna aplicado ao partial.

**Status**: ✅ Concluído

**Versão**: 1.1

---

## [23/01/2026 09:30] - Registro de documentação inicial

**Descrição**:
Documentação inicial do partial de referência de cores.

**Arquivos Afetados**:

- `Pages/Shared/_ColorProfileReference.cshtml`

**Impacto**: Rastreabilidade e padronização da documentação interna.

**Status**: ✅ Concluído

**Versão**: 1.0
