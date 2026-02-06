# Documentação: Administração - Higienização de Viagens

> **Última Atualização**: 13/01/2026
> **Versão Atual**: 1.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Funcionalidades Específicas](#funcionalidades-específicas)
4. [Endpoints API](#endpoints-api)
5. [Frontend](#frontend)
6. [Troubleshooting](#troubleshooting)

---

## Visão Geral

A página **Higienizar Origens/Destinos da Viagem** é uma ferramenta de manutenção de dados (Data Cleansing) projetada para padronizar os campos de texto livre "Origem" e "Destino" nas viagens.

Permite identificar variações de um mesmo local (ex: "Aeroporto", "Aero.", "Aeroporto Int.") e unificá-las em um único valor padronizado, atualizando todas as viagens afetadas de uma só vez.

### Características Principais

- ✅ **Listagem de Distintos**: Carrega todos os valores únicos de Origem e Destino presentes no banco.
- ✅ **Seleção Múltipla**: Interface de ListBox duplo ("De/Para") para selecionar múltiplos itens a serem corrigidos.
- ✅ **Padronização em Lote**: Aplica o novo valor a todos os registros que contêm os valores selecionados.
- ✅ **Feedback Visual**: Badges com contadores e animações ao mover itens.

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/
│   └── Administracao/
│       ├── HigienizarViagens.cshtml         # View (HTML + CSS)
│       └── HigienizarViagens.cshtml.cs      # PageModel
│
├── Controllers/
│   └── ViagemLimpezaController.cs           # API de Correção
│
├── wwwroot/
│   ├── js/
│   │   └── higienizarviagens_054.js         # Lógica Frontend
```

### Tecnologias Utilizadas

| Tecnologia | Uso |
|------------|-----|
| **Syncfusion ListBox** | Componente de seleção e transferência de itens |
| **ASP.NET Core API** | Execução dos updates no banco de dados |
| **SweetAlert2** | Mensagens de confirmação e erro |

---

## Funcionalidades Específicas

### 1. Seleção e Transferência
O usuário seleciona itens na lista da esquerda (valores existentes) e os move para a direita (valores a corrigir).

**Lógica Frontend (`higienizarviagens_054.js`)**:
```javascript
function moverSelecionados(origemId, destinoId) {
    const origem = document.getElementById(origemId).ej2_instances[0];
    const destino = document.getElementById(destinoId).ej2_instances[0];

    const selectedElements = origem.getSelectedItems();
    const textosSelecionados = selectedElements.map(el => el.innerText.trim()).filter(Boolean);

    // ... lógica para mover itens entre os datasources ...

    destino.addItems(itensParaMover);
    origem.dataSource = dataOrigem.filter(item => !textosSelecionados.includes(...));

    atualizarContador(origemId);
    atualizarContador(destinoId);
}
```

### 2. Normalização de Texto
Antes de processar, os textos são normalizados (remove acentos, espaços extras) para garantir comparação correta.

```javascript
function normalizarTexto(texto) {
    return texto
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '')
        .replace(/\s+/g, ' ')
        .replace(/\//g, '/')
        .trim()
        .toLowerCase();
}
```

### 3. Execução da Correção
O usuário digita o "Novo Valor" e clica em Gravar. O sistema envia a lista de valores antigos e o novo valor para a API.

**Chamada API**:
```javascript
function enviarRequisicaoUnificacao(dados) {
    showLoading();
    fetch('/api/viagem/unificar', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(dados)
    })
    .then(response => {
        if (!response.ok) throw new Error("Erro ao gravar.");
        return response.json();
    })
    .then(() => {
        hideLoading();
        Swal.fire("Sucesso", "Unificação realizada!", "success")
            .then(() => location.reload());
    });
}
```

---

## Endpoints API

### 1. GET `/api/ViagemLimpeza/origens`
Retorna lista de strings com todas as origens distintas.

### 2. GET `/api/ViagemLimpeza/destinos`
Retorna lista de strings com todos os destinos distintos.

### 3. POST `/api/ViagemLimpeza/corrigir-origem`
Atualiza o campo `Origem` nas viagens.

**Implementação (`ViagemLimpezaController.cs`)**:
```csharp
[HttpPost("corrigir-origem")]
public async Task<IActionResult> CorrigirOrigem([FromBody] CorrecaoRequest request)
{
    // Chama repositório que executa:
    // UPDATE Viagem SET Origem = @NovoValor WHERE Origem IN (@Anteriores)
    await _viagemRepo.CorrigirOrigemAsync(request.Anteriores, request.NovoValor);
    return NoContent();
}
```

### 4. POST `/api/ViagemLimpeza/corrigir-destino`
Atualiza o campo `Destino` nas viagens.

**Payload**:
```json
{
  "anteriores": ["Aeroporto", "Aero Int", "Galeão"],
  "novoValor": "Aeroporto Internacional Galeão"
}
```

> ⚠️ **Nota**: O código JavaScript faz referência a `/api/viagem/unificar`, mas o Controller analisado é `ViagemLimpezaController` com rotas `corrigir-origem` e `corrigir-destino`. Pode haver uma discrepância entre a versão do JS e do Controller, ou um redirecionamento de rota. A documentação reflete o código do Controller encontrado.

---

## Frontend

### Componentes ListBox
Utiliza `ejs-listbox` com `allowFiltering="true"` para facilitar a busca em listas grandes de endereços.

**HTML (`HigienizarViagens.cshtml`)**:
```html
<ejs-listbox id="listOrigens"
             dataSource="@Model.OrigensDistintas"
             allowFiltering="true"
             scope="origens"
             height="350px"
             cssClass="e-custom-listbox w-100"
             filterBarPlaceholder="Filtrar...">
</ejs-listbox>
```

---

## Troubleshooting

### Problema: Itens somem ao mover
**Sintoma**: Ao clicar na seta para mover, o item desaparece de ambas as listas.
**Causa**: Problema na manipulação do `dataSource` do Syncfusion ou incompatibilidade de tipos (string vs objeto {text, value}).
**Solução**: A função `moverSelecionados` possui lógica para detectar o tipo de dado e garantir que objetos sejam criados corretamente antes de adicionar ao destino.

### Problema: Erro ao gravar
**Sintoma**: Swal de erro "Falha ao enviar os dados".
**Causa**: Erro 500 na API (provavelmente timeout se houver muitas viagens para atualizar) ou rota incorreta.
**Verificação**: Confirmar se a rota no JS (`/api/viagem/unificar` ou `/api/ViagemLimpeza/...`) bate com o Controller.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [13/01/2026 17:15] - Padronização: Correção btn-verde → btn-azul

**Descrição**: Corrigidos botões que usavam `btn-verde` incorretamente para ações neutras de salvar/gravar.

**Problema Identificado**:
- Botões de "Salvar/Gravar" em modais CRUD estavam usando `btn-verde`
- `btn-verde` deve ser usado apenas para ações positivas (Importar, Processar, Aprovar, Aplicar)
- Botões de salvar genérico devem usar `btn-azul` (ações neutras)

**Solução Implementada**:
- Substituídas classes `btn-verde` por `btn-azul` em botões de salvar/gravar
- Alinhamento com diretrizes oficiais FrotiX (v2.7)

**Arquivos Afetados**:
- `Pages/Administracao/HigienizarViagens.cshtml` (linhas 271, 323)
  - btnGravarOrigem: `btn-verde` → `btn-azul`
  - btnGravarDestino: `btn-verde` → `btn-azul`

**Diretrizes FrotiX:**
- ✅ `btn-azul` para: Salvar/Editar/Inserir/Atualizar ações neutras
- ✅ `btn-verde` para: Importar/Processar/Aprovar/Aplicar/Confirmar ações positivas
- ✅ `btn-vinho` para: Cancelar/Fechar/Excluir

**Impacto**:
- ✅ Uso consistente de cores conforme tipo de ação
- ✅ Sistema 100% alinhado com padrão FrotiX

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.0 → 1.1

---

## [06/01/2026] - Criação da Documentação

**Descrição**:
Documentação inicial da página de Higienização de Viagens.

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
