# CustomReportSourceResolver.cs

> **Última Atualização**: 23/01/2026 12:00  
> **Versão**: 1.1  
> **Documentação Intra-Código**: ✅ Completa

---

# PARTE 2: LOG DE MODIFICAÇÕES

## [23/01/2026 12:00] - Documentação Intra-Código Completa

**Descrição**: Adicionados Cards e tags semânticas  
**Status**: ✅ Concluído

---

# PARTE 1: VISÃO GERAL

## Visão Geral

Resolver customizado para **Telerik Reporting** que localiza e carrega relatórios (`.trdp` ou `.trdx`) da pasta `Reports` e passa parâmetros do frontend para o relatório.

## Localização

`Services/CustomReportSourceResolver.cs`

## Dependências

- `Telerik.Reporting` (`IReportSourceResolver`, `ReportSource`, `UriReportSource`)
- `Telerik.Reporting.Services` (`OperationOrigin`)
- `Microsoft.AspNetCore.Hosting` (`IWebHostEnvironment`)

## Interface (`IReportSourceResolver`)

### `Resolve(string reportId, OperationOrigin operationOrigin, IDictionary<string, object> currentParameterValues)`

Resolve um relatório pelo ID e retorna `ReportSource` com parâmetros aplicados.

**Parâmetros**:

- `reportId`: ID/nome do relatório (ex.: `"RelatorioViagens"`)
- `operationOrigin`: Origem da operação (ex.: `Print`, `Export`)
- `currentParameterValues`: Parâmetros do frontend (ex.: `{ "DataInicio": "2024-01-01", "DataFim": "2024-12-31" }`)

**Retorna**: `ReportSource` (configurado com URI e parâmetros)

---

## Implementação (`CustomReportSourceResolver`)

### Construtor

```csharp
public CustomReportSourceResolver(IWebHostEnvironment environment)
```

Armazena `IWebHostEnvironment` para acessar `ContentRootPath`.

---

### Método Principal

#### `Resolve(string reportId, OperationOrigin operationOrigin, IDictionary<string, object> currentParameterValues)`

**Propósito**: Localiza arquivo de relatório e aplica parâmetros.

**Fluxo**:

1. Monta caminho do relatório: `{ContentRootPath}/Reports/{reportId}`
2. Adiciona extensão se não tiver:
   - Se não terminar com `.trdp` ou `.trdx` → adiciona `.trdp`
3. Verifica se arquivo existe:
   - Se não existir → lança `FileNotFoundException`
4. Cria `UriReportSource` com caminho do arquivo
5. **CRÍTICO**: Aplica parâmetros do frontend:
   - Itera sobre `currentParameterValues`
   - Adiciona cada parâmetro ao `reportPackageSource.Parameters`
6. Retorna `ReportSource`

**Chamado de**: Telerik Report Server automaticamente quando relatório é solicitado

**Complexidade**: Baixa (operações de arquivo simples)

---

## Estrutura de Diretórios

```
{ContentRootPath}/
  Reports/
    RelatorioViagens.trdp
    RelatorioMotoristas.trdp
    RelatorioVeiculos.trdx
    ...
```

---

## Contribuição para o Sistema FrotiX

### 📊 Integração com Telerik Reporting

- Resolve relatórios automaticamente da pasta `Reports`
- Suporta formatos `.trdp` (packaged) e `.trdx` (XML)
- Integra com Telerik Report Server

### 🔧 Passagem de Parâmetros

- **Crítico**: Passa parâmetros do frontend para o relatório
- Permite filtros dinâmicos (datas, IDs, etc.)
- Evita necessidade de hardcode de parâmetros

### 🎯 Flexibilidade

- Suporta múltiplos formatos de relatório
- Adiciona extensão automaticamente se não especificada
- Valida existência do arquivo antes de carregar

## Observações Importantes

1. **⚠️ CRÍTICO - Passagem de Parâmetros**: O código aplica parâmetros do frontend ao relatório. Sem isso, relatórios não receberiam filtros dinâmicos. Este é um ponto crítico da implementação.

2. **Extensão Padrão**: Se não especificar extensão, assume `.trdp`. Se usar `.trdx`, especifique explicitamente.

3. **Caminho Absoluto**: Usa `ContentRootPath` para caminho absoluto. Relatórios devem estar na pasta `Reports` na raiz da aplicação.

4. **Error Handling**: Lança `FileNotFoundException` se relatório não existir. Telerik Report Server captura e retorna erro ao frontend.

5. **Parâmetros Nulos**: Se `currentParameterValues` for `null`, não aplica parâmetros. Relatório será carregado sem filtros.

6. **Operation Origin**: O parâmetro `operationOrigin` não é usado atualmente, mas está disponível para lógica futura (ex.: diferentes relatórios para Print vs Export).

## Exemplo de Uso

### Frontend (JavaScript)

```javascript
var reportViewer = $("#reportViewer").data("telerik_ReportViewer");
reportViewer.reportSource({
  report: "RelatorioViagens",
  parameters: {
    DataInicio: "2024-01-01",
    DataFim: "2024-12-31",
    VeiculoId: "123e4567-e89b-12d3-a456-426614174000",
  },
});
```

### Backend (Automático)

O `CustomReportSourceResolver` é chamado automaticamente pelo Telerik Report Server quando o relatório é solicitado. Não é necessário chamar manualmente.

## Registro no DI Container

```csharp
// Startup.cs ou Program.cs
services.AddScoped<IReportSourceResolver, CustomReportSourceResolver>();
```

## Arquivos Relacionados

- `Reports/*.trdp`: Arquivos de relatório Telerik
- `Reports/*.trdx`: Arquivos de relatório Telerik (XML)
- `Controllers/Relatorio*Controller.cs`: Controllers que expõem relatórios
- `Telerik.Reporting`: Biblioteca Telerik Reporting

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

- âŒ **ANTES**: \_unitOfWork.Entity.AsTracking().Get(id) ou \_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: \_unitOfWork.Entity.GetWithTracking(id) ou \_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

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
