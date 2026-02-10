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
