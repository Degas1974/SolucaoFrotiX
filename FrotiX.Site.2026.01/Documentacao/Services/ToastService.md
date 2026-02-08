# ToastService.cs e AppToast.cs

## Visão Geral
Sistema de **notificações toast** (mensagens temporárias) para o frontend. Dois componentes:
1. **`ToastService`**: Serviço injetável para uso em PageModels/Controllers
2. **`AppToast`**: Classe estática para uso direto em código C#

Ambos armazenam mensagens em `TempData` e as exibem via JavaScript no frontend.

## Localização
- `Services/ToastService.cs` (serviço injetável)
- `Services/AppToast.cs` (classe estática)

## Dependências
- `Microsoft.AspNetCore.Http` (`IHttpContextAccessor`)
- `Microsoft.AspNetCore.Mvc.ViewFeatures` (`ITempDataDictionary`, `ITempDataDictionaryFactory`)
- `FrotiX.Models` (`ToastMessage`)

## ToastService (Serviço Injetável)

### Interface (`IToastService`)

#### `Show(string texto, string cor = "Verde", int duracao = 2000)`
Exibe toast genérico.

#### `ShowSuccess(string texto, int duracao = 2000)`
Exibe toast de sucesso (verde).

#### `ShowError(string texto, int duracao = 2000)`
Exibe toast de erro (vermelho).

#### `ShowWarning(string texto, int duracao = 2000)`
Exibe toast de aviso (amarelo).

#### `GetJavaScriptCall(string texto, string cor = "Verde", int duracao = 2000)`
Retorna string JavaScript para chamada direta (útil para AJAX).

#### `ShowMultiple(params ToastMessage[] messages)`
Exibe múltiplas mensagens toast.

---

### Implementação (`ToastService`)

#### Construtor
```csharp
public ToastService(ITempDataDictionaryFactory tempDataFactory, IHttpContextAccessor httpContextAccessor)
```

**Inicialização**:
- Obtém `TempData` do contexto HTTP atual
- Armazena mensagens na chave `"ToastMessages"`

#### Armazenamento
- **Chave TempData**: `"ToastMessages"`
- **Formato**: JSON serializado (`List<ToastMessage>`)
- **Persistência**: Sobrevive a redirects (característica do `TempData`)

#### Métodos Principais

##### `Show(string texto, string cor, int duracao)`
1. Cria `ToastMessage`
2. Adiciona à fila via `AddMessageToQueue`
3. Serializa lista completa em JSON
4. Armazena em `TempData["ToastMessages"]`

##### `AddMessageToQueue(ToastMessage message)`
- Lê mensagens existentes de `TempData`
- Adiciona nova mensagem
- Serializa e atualiza `TempData`

##### `GetCurrentMessages()`
- Lê `TempData["ToastMessages"]`
- Deserializa JSON para `List<ToastMessage>`
- Retorna lista vazia se não houver mensagens

---

## AppToast (Classe Estática)

### Configuração
```csharp
public static void Configure(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataFactory)
```

**Deve ser chamado no `Startup.cs` ou `Program.cs`** para inicializar as dependências estáticas.

---

### Método Principal

#### `show(string color, string message, int duration = 2000)` ⚠️ **MINÚSCULO**
**Propósito**: Método principal para exibir toast (funciona com redirects).

**Características**:
- ✅ Funciona após redirects (usa `TempData`)
- ✅ Escapa caracteres especiais no JavaScript
- ✅ Armazena em `TempData["ToastScripts"]` (chave diferente de `ToastService`)

**Formato Armazenado**: String JavaScript direta:
```javascript
AppToast.show('Verde', 'Mensagem', 2000);
```

---

### Métodos de Atalho

#### `ShowSuccess(string message, int duration = 2000)`
Chama `show("Verde", message, duration)`.

#### `ShowError(string message, int duration = 3000)`
Chama `show("Vermelho", message, duration)`. Duração padrão: 3000ms.

#### `ShowWarning(string message, int duration = 2000)`
Chama `show("Amarelo", message, duration)`.

#### `ShowInfo(string message, int duration = 2000)`
Chama `show("Azul", message, duration)`.

---

### Escape de JavaScript

#### `EscapeJs(string input)`
Escapa caracteres especiais para uso seguro em JavaScript:
- `\` → `\\`
- `'` → `\'`
- `"` → `\"`
- `\n` → `\\n`
- `\r` → `\\r`

---

## Modelo `ToastMessage`

```csharp
public class ToastMessage
{
    public string Texto { get; set; }
    public string Cor { get; set; } // "Verde", "Vermelho", "Amarelo", "Azul"
    public int Duracao { get; set; } // milissegundos
}
```

---

## Frontend (JavaScript)

### Renderização
O frontend deve ler `TempData["ToastMessages"]` ou `TempData["ToastScripts"]` e executar:

```javascript
// Para ToastService (JSON)
var messages = @Html.Raw(TempData["ToastMessages"]);
messages.forEach(msg => {
    AppToast.show(msg.Cor, msg.Texto, msg.Duracao);
});

// Para AppToast (JavaScript direto)
@Html.Raw(TempData["ToastScripts"])
```

---

## Contribuição para o Sistema FrotiX

### 💬 Feedback ao Usuário
- Mensagens de sucesso após operações
- Erros exibidos de forma não intrusiva
- Avisos importantes destacados

### 🔄 Compatibilidade com Redirects
- `TempData` permite exibir mensagens após redirects (ex.: após POST → redirect GET)
- Funciona em fluxos de autenticação, criação/edição de entidades, etc.

### 🎨 Personalização
- Cores configuráveis (Verde, Vermelho, Amarelo, Azul)
- Duração configurável por mensagem
- Múltiplas mensagens suportadas

## Observações Importantes

1. **Duas Implementações**: Existem duas formas de usar toast:
   - `ToastService` (injetável): Usa JSON em `TempData["ToastMessages"]`
   - `AppToast` (estático): Usa JavaScript direto em `TempData["ToastScripts"]`
   
   **Recomendação**: Use `AppToast` para simplicidade, ou `ToastService` se precisar de injeção de dependência.

2. **Configuração Necessária**: `AppToast` requer chamada de `Configure()` no startup. Se não configurado, métodos retornam silenciosamente.

3. **Chaves Diferentes**: `ToastService` usa `"ToastMessages"`, enquanto `AppToast` usa `"ToastScripts"`. Não misture!

4. **Escape de JavaScript**: `AppToast` escapa caracteres automaticamente. `ToastService` depende do frontend fazer o escape ao renderizar JSON.

5. **Duração Padrão**: 
   - Sucesso/Aviso/Info: 2000ms (2 segundos)
   - Erro: 3000ms (3 segundos) - mais tempo para leitura

6. **Thread Safety**: Ambos são thread-safe pois cada requisição HTTP tem seu próprio `TempData`.

## Arquivos Relacionados
- `Models/ToastMessage.cs`: Modelo de mensagem toast
- `wwwroot/js/`: JavaScript do frontend que exibe os toasts (ex.: `app-toast.js`)
- `Pages/`: Razor Pages que usam `ToastService` ou `AppToast`
- `Controllers/`: Controllers que usam `ToastService` ou `AppToast`


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
