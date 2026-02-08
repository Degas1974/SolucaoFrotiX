# Documentação: Fornecedor - Upsert (Criação e Edição)

> **Última Atualização**: 13/01/2026
> **Versão Atual**: 1.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura da Interface](#estrutura-da-interface)
4. [Lógica de Frontend (JavaScript)](#lógica-de-frontend-javascript)
5. [Endpoints API](#endpoints-api)
6. [Troubleshooting](#troubleshooting)

---

## Visão Geral

A página de **Upsert de Fornecedor** (`Pages/Fornecedor/Upsert.cshtml`) permite cadastrar novos fornecedores ou editar os existentes. O formulário é simples e focado em dados cadastrais essenciais.

### Características Principais

- ✅ **Formulário Simples**: Campos diretos para preenchimento rápido.
- ✅ **Máscara de CNPJ**: Formatação automática.
- ✅ **Contatos Múltiplos**: Suporte a dois contatos.

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/
│   └── Fornecedor/
│       └── Upsert.cshtml            # View do Formulário
│
├── Controllers/
│   └── FornecedorController.cs      # Controller (Submit)
```

### Tecnologias Utilizadas

| Tecnologia                   | Uso             |
| ---------------------------- | --------------- |
| **ASP.NET Core Razor Pages** | Renderização    |
| **JavaScript Puro**          | Máscara de CNPJ |
| **Bootstrap 5**              | Layout          |

---

## Estrutura da Interface

O formulário é renderizado com Razor Tag Helpers.

```html
<form method="post" asp-action="Upsert">
  <div class="row">
    <div class="col-4">
      <label>CNPJ</label>
      <input id="cnpj" asp-for="FornecedorObj.CNPJ" />
    </div>
    <div class="col-8">
      <label>Descrição</label>
      <input asp-for="FornecedorObj.DescricaoFornecedor" />
    </div>
  </div>
  <!-- ... Contatos ... -->
  <button type="submit">Salvar</button>
</form>
```

---

## Lógica de Frontend (JavaScript)

O script de máscara para o CNPJ é embutido na página para simplicidade.

```javascript
document.getElementById("cnpj").addEventListener("input", function (e) {
  var x = e.target.value
    .replace(/\D/g, "")
    .match(/(\d{0,2})(\d{0,3})(\d{0,3})(\d{0,4})(\d{0,2})/);
  e.target.value = !x[2]
    ? x[1]
    : x[1] + "." + x[2] + "." + x[3] + "/" + x[4] + (x[5] ? "-" + x[5] : "");
});
```

---

## Endpoints API

### POST `/Fornecedor/Upsert` (Handler)

Processa o formulário. Se o ID for vazio, cria um novo registro; caso contrário, atualiza o existente.

---

## Troubleshooting

### Máscara de CNPJ falha

**Causa**: O evento `input` não está sendo disparado ou regex incorreto.
**Solução**: Verifique se o ID do campo é `cnpj`.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [16/01/2026 17:15] - Auditoria Global: Campos Obrigatórios (.label-required)

**Descrição**: Adicionado asterisco vermelho em labels de campos mandatórios identificados via lógica de validação (Back/Front).

---

## [13/01/2026 15:30] - Padronização: Substituição de btn-ftx-fechar por btn-vinho

**Descrição**: Substituída classe `btn-ftx-fechar` por `btn-vinho` em botões de cancelar/fechar operação.

**Problema Identificado**:

- Classe `btn-ftx-fechar` não tinha `background-color` definido no estado `:active`
- Botões ficavam BRANCOS ao serem pressionados (em vez de manter cor rosada/vinho)
- Comportamento visual inconsistente com padrão FrotiX

**Solução Implementada**:

- Todos os botões cancelar/fechar padronizados para usar classe `.btn-vinho`
- Classe `.btn-vinho` já possui `background-color: #4a1f24` no estado `:active`
- Garantia de cor rosada/vinho ao pressionar botão

**Arquivos Afetados**:

- Pages/Fornecedor/Upsert.cshtml - Substituição de `btn-ftx-fechar` por `btn-vinho` em botão de modal

**Impacto**:

- ✅ Botão mantém cor rosada/vinho ao ser pressionado
- ✅ Alinhamento com padrão visual FrotiX
- ✅ Consistência em todo o sistema

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.1

---

## [06/01/2026] - Criação da Documentação

**Descrição**:
Documentação inicial do formulário de Upsert de Fornecedores.

**Status**: ✅ **Documentado**

**Responsável**: Claude (AI Assistant)
**Versão**: 1.0
