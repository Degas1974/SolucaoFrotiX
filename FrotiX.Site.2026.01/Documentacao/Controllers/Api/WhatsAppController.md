# Documentação: WhatsAppController.cs

> **Última Atualização**: 15/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Métodos](#métodos)
4. [Dependências](#dependências)
5. [Propriedades](#propriedades)
6. [Interconexões](#interconexões)
7. [Troubleshooting](#troubleshooting)

---

## Visão Geral

**Arquivo**: `Controllers\Api\WhatsAppController.cs`

**Categoria**: Controller

**Total de Linhas**: 106

**Classe**: `WhatsAppController`

**Namespace**: `FrotiX.Controllers.Api`

### Características Principais

- ✅ **5** métodos públicos
- ✅ **0** propriedades
- ✅ **1** dependências injetadas
- ✅ Herda de: `ControllerBase`

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
│   ├── Controllers
    │   ├── Api
        │   └── WhatsAppController.cs
```

### Tecnologias Utilizadas

| Tecnologia | Uso |
|------------|-----|
| ASP.NET Core | Backend |
| C# 12 | Linguagem |

## Métodos

### `Start`

**Retorno**: `Task<IActionResult>`

**HTTP**: `POST` `start`

**Parâmetros**:

- `StartSessionRequest req` [FromBody]
- `CancellationToken ct` 

**Código**:

```csharp
{
            try
            {
                var session = string.IsNullOrWhiteSpace(req?.Session) ? null : req.Session.Trim();
                var r = await _wa.StartSessionAsync(session , ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Start" , ex);
                return BadRequest(new { success = false , message = ex.Message });
            }
        }
```

**Métodos Chamados**:

- `string.IsNullOrWhiteSpace`
- `req.Session.Trim`
- `_wa.StartSessionAsync`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `BadRequest`

---

### `Status`

**Retorno**: `Task<IActionResult>`

**HTTP**: `GET` `status`

**Parâmetros**:

- `string session` [FromQuery]
- `CancellationToken ct` 

**Código**:

```csharp
{
            try
            {
                var r = await _wa.GetStatusAsync(session , ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Status" , ex);
                return BadRequest(new { success = false , message = ex.Message });
            }
        }
```

**Métodos Chamados**:

- `_wa.GetStatusAsync`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `BadRequest`

---

### `Qr`

**Retorno**: `Task<IActionResult>`

**HTTP**: `GET` `qr`

**Parâmetros**:

- `string session` [FromQuery]
- `CancellationToken ct` 

**Código**:

```csharp
{
            try
            {
                var b64 = await _wa.GetQrBase64Async(session , ct);
                if (string.IsNullOrWhiteSpace(b64))
                    return NotFound(new { success = false , message = "QR não disponível." });

                // Se vier só o base64, garanta o prefixo data URI
                if (!b64.StartsWith("data:" , StringComparison.OrdinalIgnoreCase))
                    b64 = "data:image/png;base64," + b64;

                return Ok(new { success = true , qrcode = b64 });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Qr" , ex);
                return BadRequest(new { success = false , message = ex.Message });
            }
        }
```

**Métodos Chamados**:

- `_wa.GetQrBase64Async`
- `string.IsNullOrWhiteSpace`
- `NotFound`
- `b64.StartsWith`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `BadRequest`

---

### `SendText`

**Retorno**: `Task<IActionResult>`

**HTTP**: `POST` `send-text`

**Parâmetros**:

- `SendTextRequest req` [FromBody]
- `CancellationToken ct` 

**Código**:

```csharp
{
            try
            {
                var r = await _wa.SendTextAsync(req.Session , req.PhoneE164 , req.Message , ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "SendText" , ex);
                return BadRequest(new { success = false , message = ex.Message });
            }
        }
```

**Métodos Chamados**:

- `_wa.SendTextAsync`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `BadRequest`

---

### `SendMedia`

**Retorno**: `Task<IActionResult>`

**HTTP**: `POST` `send-media`

**Parâmetros**:

- `SendMediaRequest req` [FromBody]
- `CancellationToken ct` 

**Código**:

```csharp
{
            try
            {
                var r = await _wa.SendMediaAsync(req.Session , req.PhoneE164 , req.FileName , req.Base64Data , req.Caption , ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "SendMedia" , ex);
                return BadRequest(new { success = false , message = ex.Message });
            }
        }
```

**Métodos Chamados**:

- `_wa.SendMediaAsync`
- `Ok`
- `Alerta.TratamentoErroComLinha`
- `BadRequest`

---

## Dependências

| Interface | Campo | Descrição |
|-----------|-------|-----------|
| `IWhatsAppService` | `_wa` | Injetado via construtor |

## Interconexões

### Quem Chama Este Arquivo

*(Análise manual necessária)*

### O Que Este Arquivo Chama

- `IWhatsAppService` → Injetado via construtor


## Troubleshooting

### Problemas Comuns

#### Erro: Exceção não tratada

**Sintoma**: Erro 500 ao acessar a funcionalidade

**Solução**: Verificar logs em `/Documentacao/Logs/` e consultar `Alerta.TratamentoErroComLinha()`


---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [15/01/2026 06:47] - Documentação inicial gerada automaticamente

**Descrição**: Documentação gerada automaticamente pelo DocGenerator FrotiX.

**Arquivos Afetados**:
- `Controllers\Api\WhatsAppController.cs`

**Status**: ✅ **Concluído**



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
