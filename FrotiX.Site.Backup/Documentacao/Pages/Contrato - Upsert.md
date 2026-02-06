# Documentação: Contrato - Upsert (Criação e Edição)

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

A página de **Upsert de Contrato** (`Pages/Contrato/Upsert.cshtml`) é uma das mais complexas do sistema, pois adapta sua interface dinamicamente conforme o **Tipo de Contrato** (Locação, Terceirização, Serviços). Ela gerencia dados cadastrais, vigência, valores financeiros e itens específicos de cada tipo.

### Características Principais

- ✅ **Interface Dinâmica**: Campos aparecem ou somem baseados no tipo de contrato.
- ✅ **Grid de Veículos**: Para contratos de Locação, permite adicionar veículos diretamente.
- ✅ **Configuração de Terceirização**: Checkboxes e inputs para definir cargos e custos de mão de obra.
- ✅ **Repactuação**: Em edição, permite navegar pelo histórico de aditivos.

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/
│   └── Contrato/
│       └── Upsert.cshtml            # View do Formulário
│
├── Controllers/
│   └── ContratoController.cs        # Endpoints API (Insert/Update)
```

### Tecnologias Utilizadas

| Tecnologia                   | Uso                            |
| ---------------------------- | ------------------------------ |
| **ASP.NET Core Razor Pages** | Renderização                   |
| **Syncfusion Grid**          | Tabela de Veículos             |
| **jQuery**                   | Lógica de exibição condicional |
| **Bootstrap 5**              | Layout                         |

---

## Estrutura da Interface

### Seção Identificação

Dados básicos como Número, Ano, Processo e Vigência.

### Seção Detalhes (Dinâmica)

Dependendo do `#lstTipoContrato`:

- **Locação**: Exibe `#divVeiculosAdd` (Grid Syncfusion).
- **Terceirização**: Exibe `#divTerceirizacao` (Cargos e Salários).
- **Serviços**: Exibe apenas campos de valor global.

```html
<select id="lstTipoContrato" class="form-control">
  <option value="Locação">Locação</option>
  <option value="Terceirização">Terceirização</option>
  <option value="Serviços">Serviços</option>
</select>
```

---

## Lógica de Frontend (JavaScript)

Scripts inline na página controlam a visibilidade das seções.

### Controle de Exibição

```javascript
$("#lstTipoContrato").on("change", function () {
  // Esconde tudo primeiro
  $("#divVeiculosAdd, #divTerceirizacao").hide();

  if (this.value === "Locação") {
    $("#divVeiculosAdd").show();
  } else if (this.value === "Terceirização") {
    $("#divTerceirizacao").show();
  }
});
```

### Submissão do Formulário

A função `InsereRegistro` coleta os dados, incluindo os do Grid Syncfusion (se aplicável), e envia para a API.

```javascript
function InsereRegistro() {
  // Validações...

  var objContrato = {
    // ... coleta dados dos inputs ...
    TipoContrato: $("#lstTipoContrato").val(),
    // ...
  };

  // Envio AJAX
  $.ajax({
    url: "api/Contrato/InsereContrato", // ou EditaContrato
    data: JSON.stringify(objContrato),
    // ...
  });
}
```

---

## Endpoints API

### POST `/api/Contrato/InsereContrato`

Cria um novo contrato.

### POST `/api/Contrato/EditaContrato`

Atualiza um contrato existente.

### POST `/api/Contrato/InsereItemContrato`

Utilizado em loop pelo frontend para salvar os itens do grid de veículos logo após criar o contrato.

---

## Troubleshooting

### Grid de Veículos não aparece

**Causa**: Tipo de contrato não selecionado ou erro no script de `change`.
**Solução**: Selecione "Locação". Verifique o console do navegador.

### Erro ao salvar itens

**Causa**: O ID do contrato (Repactuação) não foi retornado corretamente pela criação do contrato.
**Solução**: Verifique se a chamada `InsereContrato` retornou sucesso e o ID necessário para a chamada subsequente `InsereItemContrato`.

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

- Pages/Contrato/Upsert.cshtml - Substituição de `btn-ftx-fechar` por `btn-vinho` em botão de modal

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
Documentação inicial do formulário de Upsert de Contratos.

**Status**: ✅ **Documentado**

**Responsável**: Claude (AI Assistant)
**Versão**: 1.0


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
