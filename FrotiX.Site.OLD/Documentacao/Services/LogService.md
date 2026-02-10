# Sistema Centralizado de Logs e Telemetria

O LogService é a "caixa preta" do FrotiX. Ele fornece uma infraestrutura robusta para capturar eventos operacionais, erros de sistema e alertas de segurança, tanto do lado do servidor (C#) quanto do lado do cliente (JavaScript). Através deste serviço, os desenvolvedores e administradores conseguem reconstruir incidentes e monitorar a saúde da aplicação em tempo real.

## 📝 Arquitetura de Registro

O serviço foi projetado para ser resiliente e detalhado, gravando informações que facilitam o diagnóstico imediato.

### Características Principais:
1.  **Persistência Diária:** Os logs são segmentados em arquivos de texto por data (rotix_log_yyyy-MM-dd.txt), facilitando o arquivamento e a busca por eventos específicos de um dia.
2.  **Captura de Contexto HTTP:** Cada entrada de log captura automaticamente o **Usuário Logado** e a **URL/Rota** que disparou o evento. Isso é fundamental para identificar se um erro é generalizado ou restrito a um perfil de usuário.
3.  **Cross-Platform (C# & JS):** O serviço expõe endpoints para que erros de JavaScript no navegador sejam transmitidos e gravados no servidor, permitindo visualizar falhas de frontend que de outra forma ficariam ocultas no console do usuário.

## 🛠 Snippets de Lógica Principal

### Captura de Erros de Backend
Abaixo, a implementação que enriquece a mensagem de erro com metadados de execução:

`csharp
public void Error(string message, Exception? ex, string? arquivo, string? metodo, int? linha) {
    var sb = new StringBuilder();
    sb.AppendLine($"[ERROR] ❌ {message}");
    sb.AppendLine($"  📄 Local: {arquivo} | Função: {metodo} | Linha: {linha}");
    sb.AppendLine($"  🌐 URL: {GetCurrentUrl()}");
    sb.AppendLine($"  👤 Usuário: {GetCurrentUser()}");
    if (ex != null) sb.AppendLine($"  💥 Exception: {ex.Message}");
    
    WriteLog(sb.ToString());
    OnErrorOccurred?.Invoke(message); // Notifica observadores em tempo real
}
`

## 📝 Notas de Implementação

- **Thread-Safety:** O serviço utiliza um lock (_lockObject) para garantir que múltiplos processos tentando gravar logs simultaneamente não corrompam o arquivo ou causem exceções de acesso em disco.
- **Notificações em Tempo Real:** O evento OnErrorOccurred permite que outros módulos (como um dashboard de administração) mostrem alertas visuais assim que um erro crítico acontece.
- **Automatic Folder Creation:** Na inicialização, o serviço verifica e cria automaticamente o diretório de logs (/Logs), simplificando o processo de deploy em novos ambientes.

---
*Documentação de telemetria e logs - FrotiX 2026. Transparência total sobre a saúde do sistema.*
