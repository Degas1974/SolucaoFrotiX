# Validations.cs

## Visão Geral
Namespace contendo **atributos de validação customizados** para uso em modelos ASP.NET Core. Estes atributos são aplicados via Data Annotations (`[ValidationAttribute]`) e executam validação server-side antes de processar requisições.

## Localização
`Services/Validations.cs` (namespace `FrotiX.Validations`)

## Dependências
- `System.ComponentModel.DataAnnotations` (`ValidationAttribute`)
- `System.Text.RegularExpressions` (validações com regex)

## Atributos de Validação

### 1. `ValidateStrings` (Case-Insensitive)
**Propósito**: Valida se o valor está em uma lista de strings permitidas (separadas por vírgula), sem diferenciar maiúsculas/minúsculas.

**Uso**:
```csharp
[ValidateStrings("Ativo,Inativo,Cancelado")]
public string Status { get; set; }
```

**Lógica**:
- Compara valor em lowercase com cada item da lista
- Permite string vazia se a lista contiver `""` como item válido
- Retorna `false` se valor não estiver na lista

**Complexidade**: Baixa

---

### 2. `ValidateStringsWithSensitivity` (Case-Sensitive)
**Propósito**: Mesma funcionalidade de `ValidateStrings`, mas **diferenciando** maiúsculas/minúsculas.

**Uso**:
```csharp
[ValidateStringsWithSensitivity("ATIVO,INATIVO")]
public string Status { get; set; }
```

**Complexidade**: Baixa

---

### 3. `OnlyUrls`
**Propósito**: Valida se o valor é uma URL válida (HTTP ou HTTPS).

**Uso**:
```csharp
[OnlyUrls]
public string Website { get; set; }
```

**Lógica**: Usa `Uri.TryCreate()` para validar formato absoluto e esquema HTTP/HTTPS.

**Complexidade**: Baixa

---

### 4. `OnlyDigits`
**Propósito**: Valida se o valor contém apenas dígitos (0-9).

**Uso**:
```csharp
[OnlyDigits]
public string Cpf { get; set; }
```

**Lógica**: Regex `^[0-9]+$`

**Complexidade**: Baixa

---

### 5. `OnlyCharacters`
**Propósito**: Valida se o valor contém apenas letras (a-z, A-Z).

**Uso**:
```csharp
[OnlyCharacters]
public string Nome { get; set; }
```

**Lógica**: Regex `^[a-zA-Z]+$`

**Complexidade**: Baixa

---

### 6. `UpperCase`
**Propósito**: Valida se o valor contém apenas letras maiúsculas.

**Uso**:
```csharp
[UpperCase]
public string Sigla { get; set; }
```

**Lógica**: Regex `^[A-Z]+$`

**Complexidade**: Baixa

---

### 7. `LowerCase`
**Propósito**: Valida se o valor contém apenas letras minúsculas.

**Uso**:
```csharp
[LowerCase]
public string Codigo { get; set; }
```

**Lógica**: Regex `^[a-z]+$`

**Complexidade**: Baixa

---

### 8. `ValidateDate`
**Propósito**: Valida se o valor está em um formato de data específico.

**Uso**:
```csharp
[ValidateDate("dd/MM/yyyy")]
public string DataNascimento { get; set; }
```

**Lógica**: Usa `DateTime.TryParseExact()` com formato especificado e `CultureInfo.InvariantCulture`.

**Mensagem Customizada**: `"{0} must be in {1} format."`

**Complexidade**: Baixa

---

### 9. `DateRange`
**Propósito**: Valida se o valor está dentro de um intervalo de datas (com formato específico).

**Uso**:
```csharp
[DateRange("dd/MM/yyyy", "01/01/2020", "31/12/2024")]
public string Data { get; set; }
```

**Lógica**: 
1. Valida formato da data de entrada
2. Valida formato das datas `from` e `to`
3. Verifica se `from <= data <= to`

**Mensagem Customizada**: `"{0} must be between than {1} and {2} with {3} format."`

**Complexidade**: Média

---

### 10. `ValidateDomainAtEnd`
**Propósito**: Valida se o valor termina com um domínio específico (útil para emails).

**Uso**:
```csharp
[ValidateDomainAtEnd("@camara.leg.br")]
public string Email { get; set; }
```

**Lógica**: 
- Verifica se o domínio está contido no valor
- Garante que o domínio está **no final** da string

**Mensagem Customizada**: `"{0} precisa conter {1} ao final."`

**Complexidade**: Baixa

---

### 11. `ValidDomainAnyWhere`
**Propósito**: Valida se o valor contém um domínio específico em qualquer posição.

**Uso**:
```csharp
[ValidDomainAnyWhere("camara.leg.br")]
public string Url { get; set; }
```

**Lógica**: Verifica se o domínio está contido (case-insensitive).

**Complexidade**: Baixa

---

### 12. `NumOrChars`
**Propósito**: Valida se o valor contém apenas números, letras ou ambos.

**Uso**:
```csharp
[NumOrChars]
public string Codigo { get; set; }
```

**Lógica**: Regex `^[a-zA-Z0-9]+$`

**Complexidade**: Baixa

---

### 13. `ValidateDecimals`
**Propósito**: Valida se o valor é um decimal com até 2 casas decimais.

**Uso**:
```csharp
[ValidateDecimals]
public string Valor { get; set; }
```

**Lógica**: Regex `^[0-9]*?[.][0-9][0-9]?$`

**Exemplos Válidos**: `"10.5"`, `"100.99"`, `".50"`

**Complexidade**: Baixa

---

### 14. `ValidateAmount`
**Propósito**: Valida se o valor é um valor monetário com até 3 casas decimais.

**Uso**:
```csharp
[ValidateAmount]
public string Valor { get; set; }
```

**Lógica**: Regex `^[0-9]*?([.][0-9][0-9]?[0-9]?)?$`

**Exemplos Válidos**: `"100"`, `"100.5"`, `"100.99"`, `"100.999"`

**Complexidade**: Baixa

---

### 15. `ValidateMinAge` (Compara com Data Atual)
**Propósito**: Valida se a data representa uma idade mínima em relação à data atual.

**Uso**:
```csharp
[ValidateMinAge("dd/MM/yyyy", "18")]
public string DataNascimento { get; set; }
```

**Lógica**:
1. Valida formato da data
2. Calcula diferença em anos, meses e dias entre data atual e data informada
3. Verifica se idade >= idade mínima especificada

**Mensagem Customizada**: `"{0} must have {1} format and Date should have minimum age of {2} years."`

**Complexidade**: Média

---

### 16. `ValidateMinAgeWithGivenDate` (Compara com Data Específica)
**Propósito**: Mesma funcionalidade de `ValidateMinAge`, mas compara com uma data específica em vez da data atual.

**Uso**:
```csharp
[ValidateMinAgeWithGivenDate("dd/MM/yyyy", "18", "31/12/2024")]
public string DataNascimento { get; set; }
```

**Complexidade**: Média

---

### 17. `ValidaLista`
**Propósito**: Valida se uma lista não está vazia e não contém valores padrão de dropdown.

**Uso**:
```csharp
[ValidaLista]
public string VeiculoId { get; set; }
```

**Lógica**:
- Retorna `false` se valor for `null` ou `""`
- Retorna `false` se valor contiver `"--Selecione um Modelo --"` (texto padrão de dropdown)

**Complexidade**: Baixa

---

### 18. `ValidaZero`
**Propósito**: Valida se o valor não é zero.

**Uso**:
```csharp
[ValidaZero]
public int Quantidade { get; set; }
```

**Lógica**: Retorna `false` se valor for `"0"` ou `null`.

**Complexidade**: Baixa

---

### 19. `FormatCnpjCpf` (Classe Estática)
**Propósito**: Utilitários para formatação de CNPJ/CPF.

#### `FormatCNPJ(string CNPJ)`
Formata CNPJ: `"99999999999999"` → `"99.999.999/9999-99"`

#### `FormatCPF(string CPF)`
Formata CPF: `"99999999999"` → `"999.999.999-99"`

#### `SemFormatacao(string Codigo)`
Remove formatação: `"99.999.999/9999-99"` → `"99999999999999"`

**Complexidade**: Baixa

---

## Contribuição para o Sistema FrotiX

### 🛡️ Validação Server-Side
Estes atributos garantem que dados inválidos **nunca** cheguem ao banco de dados ou sejam processados pela lógica de negócio. A validação ocorre automaticamente no Model Binding do ASP.NET Core.

### 📋 Consistência
- Validações centralizadas evitam duplicação de código
- Mensagens de erro padronizadas melhoram UX
- Validações específicas para domínio brasileiro (CNPJ/CPF, formato de data DD/MM/YYYY)

### 🔧 Facilidade de Uso
Aplicação simples via atributos:
```csharp
public class VeiculoModel
{
    [ValidaZero]
    public int Quantidade { get; set; }
    
    [ValidateDate("dd/MM/yyyy")]
    public string DataInicio { get; set; }
    
    [OnlyDigits]
    public string Placa { get; set; }
}
```

## Observações Importantes

1. **Valores Nulos**: A maioria dos atributos retorna `true` se o valor for `null`, permitindo que `[Required]` faça a validação de obrigatoriedade separadamente.

2. **Case Sensitivity**: `ValidateStrings` é case-insensitive, enquanto `ValidateStringsWithSensitivity` diferencia maiúsculas/minúsculas. Escolha conforme necessário.

3. **Formato de Data**: Sempre use `CultureInfo.InvariantCulture` para evitar problemas com localização.

4. **Regex Performance**: Os regex são simples e eficientes, mas em grandes volumes podem ser otimizados com `RegexOptions.Compiled`.

## Arquivos Relacionados
- Modelos que usam estes atributos (ex.: `Models/Veiculo.cs`, `Models/Motorista.cs`)
- `Controllers/`: Validação automática via Model Binding
- `Pages/`: Validação em Razor Pages

---

## 📋 Modificações Recentes

### [13/01/2026 04:34] - Correção de Warning CS0252

**Descrição**: Corrigida comparação de referência não intencional

**Mudança**:
- **Linha 811**: Correção de comparação entre `object` e `string`
  - Antes: `if (value == "")`
  - Depois: `if (value?.ToString() == "")`
  - Motivo: Comparar `object` diretamente com `string` literal faz comparação de referência, não de valor. O correto é converter para string primeiro.

**Arquivos Afetados**:
- `Services/Validations.cs` (linha 811)

**Impacto**: Corrige lógica de validação e elimina warning CS0252 sem alterar comportamento esperado.

**Status**: ✅ **Concluído**

---

**📅 Última atualização:** 13/01/2026


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
