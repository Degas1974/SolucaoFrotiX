# Documentação: MailRequest.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 🎯 Objetivos

O Model `MailRequest` é usado para encapsular dados de requisição de envio de email no sistema.

**Principais objetivos:**

✅ Capturar destinatário, assunto e corpo do email  
✅ Padronizar estrutura de dados para serviços de email  
✅ Facilitar integração com provedores de email (SendGrid, SMTP, etc.)

---

## 🏗️ Estrutura do Model

```csharp
public class MailRequest
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
```

**Características:**
- ✅ Propriedades simples - Estrutura básica de email
- ✅ Sem validações - Validações feitas no serviço de email

---

## 🔗 Quem Chama e Por Quê

### Services/EmailService.cs → Envio de Email

```csharp
public async Task SendEmailAsync(MailRequest request)
{
    var message = new MimeMessage();
    message.To.Add(new MailboxAddress("", request.ToEmail));
    message.Subject = request.Subject;
    message.Body = new TextPart("html") { Text = request.Body };
    
    using (var client = new SmtpClient())
    {
        await client.ConnectAsync("smtp.example.com", 587);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
```

---

## 📝 Notas Importantes

1. **Estrutura simples** - Apenas campos essenciais para envio básico.

2. **Body como HTML** - Geralmente contém HTML para formatação.

---

**📅 Documentação criada em:** 08/01/2026


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
