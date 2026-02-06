# Comunicação Externa e Notificações por E-mail

O MailService é o portal de saída de mensagens transacionais do FrotiX. Integrado ao **MimeKit** e **MailKit**, ele é responsável por garantir que informações sensíveis (como tokens de recuperação de senha e notificações de sistema) cheguem à caixa de entrada do usuário de forma segura e formatada.

## 📧 Arquitetura de Mensageria

O serviço utiliza o protocolo SMTP com camadas de segurança modernas para garantir a entregabilidade.

### Fluxo de Envio:
1.  **Injeção de Configurações:** O serviço consome dados do ppsettings.json via padrão IOptions<MailSettings>, mantendo credenciais (Host, Password, Porta) isoladas do código-fonte.
2.  **Segurança de Conexão:** Utiliza SecureSocketOptions.StartTlsWhenAvailable, adaptando-se automaticamente à segurança oferecida pelo servidor de e-mail (Office 365, Gmail, etc).
3.  **MimeMessage Corporativo:** Formata e-mails com suporte total a HTML, permitindo o uso de templates ricos com a identidade visual do FrotiX.

## 🛠 Snippets de Lógica Principal

### Despacho Assíncrono de E-mail
Abaixo, a implementação central de envio que protege a fluidez da aplicação:

`csharp
public async Task SendEmailAsync(MailRequest mailRequest) {
    var email = new MimeMessage();
    email.Sender = MailboxAddress.Parse(_settings.Mail);
    email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
    email.Subject = mailRequest.Subject;
    email.Body = new TextPart("html") { Text = mailRequest.Body };

    using var smtp = new SmtpClient();
    // Conexão segura e autenticação automática
    smtp.Connect(_settings.Host, _settings.Port, SecureSocketOptions.StartTlsWhenAvailable);
    smtp.Authenticate(_settings.Mail, _settings.Password);
    await smtp.SendAsync(email);
    smtp.Disconnect(true);
}
`

## 📝 Notas de Implementação

- **Performance:** O envio é sempre assíncrono (sync Task), evitando que a interface do usuário trave enquanto aguarda a resposta do servidor SMTP.
- **Identidade Visual:** O remetente é fixado como "FrotiX - Autenticação", criando confiança no usuário final ao receber e-mails de segurança.
- **Isolamento de Erros:** Exceções no servidor de e-mail devem ser tratadas pelo chamador, permitindo que o sistema ofereça alternativas (como reenvio ou alerta de suporte) caso o provider de e-mail esteja offline.

---
*Documentação de integração externa - FrotiX 2026. Conectividade e segurança na comunicação.*


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
