# Documentação: Shared - \_DropdownMenu.cshtml

> **Última Atualização**: 21/01/2026  
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

Partial responsável por exibir o dropdown do usuário com avatar, nome e ação de logout. Nesta atualização, o nome recebe `data-default` para permitir atualização pelo header.

## Estrutura de Arquivo

```
Pages/Shared/_DropdownMenu.cshtml
```

## Trecho Relevante

```html
<div
  id="divUser"
  class="fs-lg text-truncate text-truncate-lg"
  data-default="@(Settings.Theme.User)"
>
  @(Settings.Theme.User)
</div>
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [21/01/2026 14:20] - Fallback do usuário logado no dropdown

**Descrição**: Adicionado `data-default` no elemento do nome do usuário para uso pelo script do header.

**Arquivos Afetados**:

- Pages/Shared/\_DropdownMenu.cshtml

**Status**: ✅ **Concluído**
