# Documentação: Usuarios - MudarSenha

> **Última Atualização**: 21/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Frontend](#frontend)
4. [Backend (PageModel)](#backend-pagemodel)
5. [Histórico de Mudanças](#histórico-de-mudanças)

---

## Visão Geral

Página dedicada à alteração de senha do usuário autenticado no sistema FrotiX. A interface foi redesenhada para seguir o padrão visual "FrotiX 2026", oferecendo uma experiência focada em segurança e clareza.

### Características Principais

- ✅ **Identidade Visual**: Uso de ícones duotone grandes, cores sóbrias e tipografia 'Outfit'.
- ✅ **Informações do Perfil**: Exibição detalhada do usuário logado (Foto/Avatar, Ponto e Nome Completo).
- ✅ **Segurança**: Olhos de alternância de visibilidade de senha personalizados em tom laranja FrotiX.
- ✅ **Navegação**: Integração com o header padrão incluindo botão de retorno para a listagem de usuários.

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/Usuarios/MudarSenha.cshtml
├── Pages/Usuarios/MudarSenha.cshtml.cs
```

### Informações de Roteamento

- **Módulo**: `Usuarios`
- **Página**: `MudarSenha`
- **Rota (Razor Pages)**: `/Usuarios/MudarSenha` (Padrão)
- **@model**: `FrotiX.Pages.Usuarios.MudarSenhaModel`

---

## Frontend

### Estilização (CSS Customizado)

A página utiliza estilos locais para garantir a unificação visual:

- `.btn-ftx-eye-orange`: Botões de visualização da senha na cor laranja `#ff6b35`.
- `.ftx-password-input`: Inputs com borda reforçada de `2px` para alinhamento estético.
- `.ftx-user-info-card`: Card com sombra e bordas arredondadas para destaque do perfil.

### Componentes

- **Header**: Título "Mudança de Senha" acompanhado pelo ícone `fa-key-skeleton`.
- **Botões**:
  - `btn-header-orange`: Botão de retorno "Voltar para a Lista".
  - `btn-azul btn-submit-spin`: Botão principal de confirmação com animação de pulse no ícone.
  - `btn-vinho`: Botão "Cancelar Operação".

---

## Backend (PageModel)

### Carregamento de Dados

O método `LoadUserDetailsAsync` é responsável por buscar as informações estendidas do usuário no banco de dados (`AspNetUsers`):

- Recupera o `Ponto`.
- Formata o `NomeCompleto` para _Title Case_.
- Converte a `Foto` (byte array) para Base64 para exibição.

### Lógica de Alteração

Utiliza o `UserManager<IdentityUser>` para processar a mudança:

1. `ChangePasswordAsync`: Valida a senha atual e define a nova.
2. `RefreshSignInAsync`: Mantém a sessão ativa após a troca de credenciais.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [21/01/2026] - Refatoração Completa: Padrão FrotiX 2026

**Descrição**: Atualização total da interface e lógica de exibição para conformidade com a nova identidade visual do projeto.

**Arquivos Afetados**:

- `Pages/Usuarios/MudarSenha.cshtml`
- `Pages/Usuarios/MudarSenha.cshtml.cs`

**Mudanças**:

- ✅ **Design**: Novo layout com card de informações do usuário e header estilizado.
- ✅ **Ícones**: Conversão de ícones sólidos para Duotone (`fa-key-skeleton`, `fa-user-gear`).
- ✅ **Bugfix**: Resolvido erro 404 causado por definição de rota duplicada no `@page`.
- ✅ **Backend**: Implementado carregamento de foto e formatação de nome em Title Case.
- ✅ **UX**: Botões de ação padronizados (`btn-azul`, `btn-vinho`) e inputs com bordas de `2px`.

**Status**: ✅ **Concluído**
**Responsável**: GitHub Copilot (Agente IA)
**Versão**: 1.0 (Lançamento Padrão 2026)
