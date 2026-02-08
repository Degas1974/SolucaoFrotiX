# Documentação: \_DropdownMenu.cshtml — Menu do Usuário

> **Última Atualização**: 21/01/2026  
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

Partial responsável por renderizar o dropdown do usuário no header principal. Exibe avatar, nome e ação de logout. Nesta atualização, passou a expor o valor padrão de nome para ser atualizado pela chamada de usuário logado.

## Estrutura de Arquivo

```
Pages/Shared/_DropdownMenu.cshtml
```

## Elementos Principais

- **Avatar**: imagem de perfil padrão.
- **Nome do usuário**: `#divUser` com fallback (`data-default`).
- **Ação**: botão de logout.

### Trecho relevante

```html
<div
  id="divUser"
  class="fs-lg text-truncate text-truncate-lg"
  data-default="@(Settings.Theme.User)"
>
  @(Settings.Theme.User)
</div>
```

## Integração

- O valor de `data-default` é consumido pelo script do header para atualizar a label com `(ponto.) Nome`.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [21/01/2026 14:20] - Fallback do usuário logado no dropdown

**Descrição**: Adicionado atributo `data-default` no elemento de nome do usuário para permitir atualização via script do header.

**Arquivos Afetados**:

- Pages/Shared/\_DropdownMenu.cshtml

**Status**: ✅ **Concluído**
