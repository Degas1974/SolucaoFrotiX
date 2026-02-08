# ReCaptchaService.cs e IReCaptchaService.cs

## Visão Geral
Serviço de **validação de reCAPTCHA** do Google. Atualmente implementado parcialmente (método `ValidateReCaptcha` não retorna resultado da validação).

## Localização
- `Services/ReCaptchaService.cs` (implementação)
- `Services/IReCaptchaService.cs` (interface)

## Dependências
- `Microsoft.Extensions.Options` (`IOptions<ReCaptchaSettings>`)
- `FrotiX.Settings` (`ReCaptchaSettings`)
- `System.Net.Http` (`HttpClient`)

## Interface (`IReCaptchaService`)

### `Configs` (propriedade)
Retorna configurações do reCAPTCHA (`ReCaptchaSettings`).

### `ValidateReCaptcha(string token)`
Valida token do reCAPTCHA com a API do Google.

**Parâmetros**:
- `token`: Token do reCAPTCHA retornado pelo frontend

**Retorna**: `bool` (atualmente sempre retorna `false` - ver Observações)

---

## Implementação (`ReCaptchaService`)

### Configuração
Configurações são injetadas via `IOptions<ReCaptchaSettings>`:
- `Secret`: Chave secreta do reCAPTCHA
- Outras configurações em `ReCaptchaSettings`

### Método Principal

#### `ValidateReCaptcha(string token)`
**Propósito**: Valida token do reCAPTCHA com a API do Google.

**Fluxo Atual**:
1. Monta URL da API do Google: `https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}`
2. Faz requisição GET assíncrona (usando `.Result` - bloqueante)
3. Lê resposta como string
4. ⚠️ **Código comentado**: Deserialização e validação estão comentadas
5. Retorna `false` sempre

**Código Comentado**:
```csharp
//CaptchaResponse response = JsonSerializer.Deserialize<CaptchaResponse>(content);
//if (response.success)
//    ret = true;
```

**Problema**: O método sempre retorna `false`, tornando a validação ineficaz.

**Chamado de**: Provavelmente `Controllers/LoginController` ou páginas de autenticação

**Complexidade**: Baixa (mas implementação incompleta)

---

## Configuração (`ReCaptchaSettings`)

Definido em `Settings/ReCaptchaSettings.cs` e configurado em `appsettings.json`:

```json
{
  "ReCaptchaSettings": {
    "Secret": "sua-chave-secreta-aqui"
  }
}
```

---

## Resposta da API do Google

A API do Google retorna JSON no formato:
```json
{
  "success": true,
  "challenge_ts": "2024-01-01T12:00:00Z",
  "hostname": "example.com"
}
```

---

## Contribuição para o Sistema FrotiX

### 🔒 Segurança
- **Objetivo**: Prevenir bots e ataques automatizados em formulários públicos
- **Status**: ⚠️ Implementação incompleta (sempre retorna `false`)

### 🛡️ Proteção de Formulários
- Login
- Recuperação de senha
- Formulários de contato
- Outros formulários públicos

## Observações Importantes

1. **⚠️ Implementação Incompleta**: O método `ValidateReCaptcha` não está funcionando corretamente. O código que deserializa e valida a resposta está comentado, fazendo com que sempre retorne `false`.

2. **Uso Bloqueante**: O código usa `.Result` em chamada assíncrona, bloqueando a thread. Deveria usar `await` e tornar o método `async Task<bool>`.

3. **Falta Modelo**: Não há modelo `CaptchaResponse` definido para deserializar a resposta da API do Google.

4. **Error Handling**: Não há tratamento de exceções. Se a requisição falhar, uma exceção será lançada.

5. **Timeout**: Não há timeout configurado no `HttpClient`. Requisições podem travar indefinidamente.

## Correção Sugerida

```csharp
public async Task<bool> ValidateReCaptchaAsync(string token)
{
    try
    {
        string url = $"https://www.google.com/recaptcha/api/siteverify?secret={_settings.Secret}&response={token}";
        
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(10);
        
        var response = await httpClient.GetAsync(url);
        if (response.StatusCode != HttpStatusCode.OK)
            return false;
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CaptchaResponse>(content);
        
        return result?.Success == true;
    }
    catch (Exception ex)
    {
        Alerta.TratamentoErroComLinha("ReCaptchaService.cs", "ValidateReCaptchaAsync", ex);
        return false;
    }
}

private class CaptchaResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("challenge_ts")]
    public string ChallengeTs { get; set; }
    
    [JsonPropertyName("hostname")]
    public string Hostname { get; set; }
}
```

## Arquivos Relacionados
- `Settings/ReCaptchaSettings.cs`: Configurações do reCAPTCHA
- `Controllers/LoginController.cs`: Provavelmente usa `IReCaptchaService`
- `appsettings.json`: Configurações do reCAPTCHA


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
