# Documentação: ErroHelper.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

A classe `ErroHelper` é um utilitário estático para montar scripts JavaScript que exibem alertas SweetAlert2 no cliente, especialmente para tratamento de erros.

**Principais características:**

✅ **Geração de Scripts**: Cria scripts JavaScript dinamicamente  
✅ **Integração SweetAlert**: Scripts para SweetAlertInterop  
✅ **Sanitização**: Escapa caracteres especiais para JavaScript  
✅ **Múltiplos Tipos**: Erro, aviso, informação, confirmação

---

## Estrutura da Classe

### Classe Estática

```csharp
public static class ErroHelper
```

**Padrão**: Classe estática com métodos utilitários

---

## Métodos Principais

### `MontarScriptErro(string classe, string metodo, Exception ex)`

**Descrição**: Monta script JavaScript para exibir erro técnico com SweetAlert

**Parâmetros**:
- `classe`: Nome da classe onde ocorreu o erro
- `metodo`: Nome do método onde ocorreu o erro
- `ex`: Exceção com detalhes do erro

**Retorno**: String com script JavaScript

**Script Gerado**:
```javascript
SweetAlertInterop.ShowTratamentoErroComLinha(
    'MeuController', 
    'SalvarDados', 
    { 
        message: 'Mensagem do erro', 
        stack: 'Stack trace completo' 
    }
);
```

**Sanitização**: Escapa aspas simples (`'` → `\'`) e remove quebras de linha

**Uso**:
```csharp
try
{
    // código
}
catch (Exception ex)
{
    string script = ErroHelper.MontarScriptErro("MeuController", "Salvar", ex);
    // Executar script no cliente
}
```

---

### `MontarScriptAviso(string titulo, string mensagem)`

**Descrição**: Monta script para alerta de aviso

**Script Gerado**:
```javascript
SweetAlertInterop.ShowWarning('Título', 'Mensagem');
```

**Uso**:
```csharp
string script = ErroHelper.MontarScriptAviso("Atenção", "Esta ação não pode ser desfeita.");
```

---

### `MontarScriptInfo(string titulo, string mensagem)`

**Descrição**: Monta script para alerta de informação

**Script Gerado**:
```javascript
SweetAlertInterop.ShowInfo('Título', 'Mensagem');
```

**Uso**:
```csharp
string script = ErroHelper.MontarScriptInfo("Informação", "Operação concluída.");
```

---

### `MontarScriptConfirmacao(string titulo, string mensagem, string textoConfirmar, string textoCancelar)`

**Descrição**: Monta script para alerta de confirmação

**Script Gerado**:
```javascript
SweetAlertInterop.ShowConfirm(
    'Título', 
    'Mensagem', 
    'Confirmar', 
    'Cancelar'
);
```

**Uso**:
```csharp
string script = ErroHelper.MontarScriptConfirmacao(
    "Confirmar Exclusão", 
    "Deseja realmente excluir?", 
    "Sim", 
    "Não"
);
```

---

## Método Auxiliar Privado

### `Sanitize(string input)`

**Descrição**: Sanitiza string para uso em JavaScript

**Transformações**:
1. Substitui `'` por `\'` (escape de aspas)
2. Remove `\r` (carriage return)
3. Remove `\n` (newline)
4. Substitui `\n` por espaço

**Uso**: Chamado internamente por todos os métodos públicos

**Exemplo**:
```csharp
string input = "Erro: 'Não foi possível salvar'\nLinha 2";
string sanitized = Sanitize(input);
// Resultado: "Erro: \'Não foi possível salvar\' Linha 2"
```

---

## Interconexões

### Quem Usa Esta Classe

- **Controllers**: Para gerar scripts de erro em views Razor
- **Pages**: Para exibir alertas em páginas Razor
- **JavaScript**: Executa scripts gerados via `SweetAlertInterop`

### O Que Esta Classe Usa

- **System**: `Exception`, `String`

---

## Comparação com `Alerta.cs`

| Característica | Alerta.cs | ErroHelper.cs |
|----------------|-----------|---------------|
| **Método** | TempData | Script JavaScript |
| **Execução** | Automática (via JS) | Manual (executar script) |
| **Uso** | Controllers/API | Views Razor |
| **Flexibilidade** | Menor | Maior (controle total) |

**Uso Recomendado**:
- **Alerta.cs**: Para uso em Controllers/API (mais simples)
- **ErroHelper.cs**: Para uso em Views Razor quando precisa de controle manual

---

## Exemplos de Uso

### Exemplo 1: Erro em View Razor

```csharp
@page
@{
    try
    {
        // código
    }
    catch (Exception ex)
    {
        string script = ErroHelper.MontarScriptErro("MinhaPage", "OnGet", ex);
        <script>@Html.Raw(script)</script>
    }
}
```

### Exemplo 2: Confirmação em View

```csharp
@{
    string scriptConfirmacao = ErroHelper.MontarScriptConfirmacao(
        "Confirmar Exclusão",
        "Deseja realmente excluir este item?",
        "Sim, excluir",
        "Cancelar"
    );
}

<button onclick="@Html.Raw(scriptConfirmacao)">Excluir</button>
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do ErroHelper

**Arquivos Afetados**:
- `Helpers/ErroHelper.cs`

**Impacto**: Documentação de referência para geração de scripts de alerta

**Status**: ✅ **Concluído**

**Versão**: 2.0

---

**Última atualização**: 08/01/2026  
**Autor**: Sistema FrotiX  
**Versão**: 2.0


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
