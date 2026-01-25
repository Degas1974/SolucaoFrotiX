# EvolutionApiWhatsAppService.cs

## Visão Geral
Serviço de integração com **Evolution API** para envio de mensagens WhatsApp. Gerencia sessões, QR codes e envio de mensagens de texto e mídia.

## Localização
`Services/WhatsApp/EvolutionApiWhatsAppService.cs`

## Dependências
- `Microsoft.Extensions.Options` (`IOptions<EvolutionApiOptions>`)
- `System.Net.Http` (`HttpClient`, `JsonContent`)
- `System.Text.Json` (`JsonDocument`)
- `FrotiX.Services.WhatsApp` (DTOs, Options)

## Interface (`IWhatsAppService`)

### `StartSessionAsync(string session, CancellationToken)`
Inicia sessão WhatsApp e gera QR code.

### `GetStatusAsync(string session, CancellationToken)`
Obtém status da sessão (CONNECTED, QRCODE, DISCONNECTED).

### `GetQrBase64Async(string session, CancellationToken)`
Obtém QR code em Base64 para exibição.

### `SendTextAsync(string session, string phoneE164, string message, CancellationToken)`
Envia mensagem de texto.

### `SendMediaAsync(string session, string phoneE164, string fileName, string base64Data, string caption, CancellationToken)`
Envia mídia (imagem, vídeo, documento) com legenda opcional.

---

## Implementação (`EvolutionApiWhatsAppService`)

### Configuração
- `BaseUrl`: URL base da Evolution API
- `ApiKey`: Chave de API
- `DefaultSession`: Sessão padrão ("FrotiX")
- `Endpoints`: Dicionário de endpoints configuráveis

### Métodos Principais

#### `StartSessionAsync()`
- Faz POST para `/session/start`
- Payload: `{ session: "FrotiX" }`
- Retorna `ApiResult` com sucesso/erro

#### `GetStatusAsync()`
- Faz GET para `/session/status/{session}`
- Parseia JSON de resposta
- Extrai status e QR code (se disponível)
- Retorna `SessionStatusDto`

#### `GetQrBase64Async()`
- Faz GET para `/session/qr/{session}`
- Extrai QR code de resposta JSON
- Suporta múltiplos formatos (`qrcode`, `base64`)

#### `SendTextAsync()`
- Faz POST para `/message/sendText`
- Payload: `{ session, number, text }`
- Número deve estar em formato E.164 (ex.: `5591988887777`)

#### `SendMediaAsync()`
- Faz POST para `/message/sendMedia`
- Payload: `{ session, number, filename, caption, base64 }`
- Suporta Base64 com ou sem prefixo `data:*;base64,`

---

## DTOs (`Dtos.cs`)

### `ApiResult`
```csharp
public class ApiResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    
    public static ApiResult Ok(string msg = null);
    public static ApiResult Fail(string msg);
}
```

### `SessionStatusDto`
```csharp
public class SessionStatusDto
{
    public string Session { get; set; }
    public string Status { get; set; } // CONNECTED, QRCODE, DISCONNECTED
    public DateTime? UpdatedAt { get; set; }
    public string QrCodeBase64 { get; set; }
}
```

---

## Configuração (`EvolutionApiOptions`)

### `Resolve(string key, string session = null)`
Resolve endpoint substituindo `{session}` pelo valor fornecido.

**Endpoints Padrão**:
- `StartSession`: `/session/start`
- `GetQr`: `/session/qr/{session}`
- `GetStatus`: `/session/status/{session}`
- `SendText`: `/message/sendText`
- `SendMedia`: `/message/sendMedia`

---

## Contribuição para o Sistema FrotiX

### 📱 Comunicação WhatsApp
- Envio de notificações via WhatsApp
- Integração com sistema de alertas
- Comunicação direta com usuários

### 🔐 Gestão de Sessões
- Gerenciamento de sessões WhatsApp
- QR code para autenticação
- Status de conexão

## Observações Importantes

1. **⚠️ Resolve Ambigüidade**: Código usa `JsonContent.Create()` em vez de `PostAsJsonAsync()` para evitar ambigüidade de namespace.

2. **Formato E.164**: Números devem estar em formato E.164 (ex.: `5591988887777`). Sem espaços, parênteses ou hífens.

3. **Error Handling**: Todos os métodos capturam exceções e retornam `ApiResult.Fail()` ou DTO com status "ERROR".

4. **QR Code**: QR code é retornado em Base64. Para exibir, use `<img src="data:image/png;base64,{qrCode}">`.

5. **Timeout**: Não há timeout configurado no `HttpClient`. Considere adicionar timeout para evitar bloqueios.

## Arquivos Relacionados
- `Services/WhatsApp/IWhatsAppService.cs`: Interface do serviço
- `Services/WhatsApp/Dtos.cs`: DTOs
- `Services/WhatsApp/EvolutionApiOptions.cs`: Configurações
- `Controllers/Api/WhatsAppController.cs`: Endpoints da API


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
