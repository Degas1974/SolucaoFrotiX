# Configuração da API Key do Claude - Guia Completo

## Resumo do que aconteceu

A API Key do Claude foi detectada no GitHub e **foi permanentemente desativada** pelo Anthropic por segurança.

Você precisa:
1. Gerar uma nova chave
2. Armazená-la de forma segura (SEM commitar no GitHub)
3. Commitar as mudanças de proteção que já fiz

---

## Etapa 1: Gerar Nova API Key

1. Acesse: https://platform.claude.com/settings/keys
2. Clique em **"Create Key"**
3. Dê um nome: `FrotiX-Dev` (ou outro nome que preferir)
4. Copie a chave gerada (ela começa com `sk-ant-api03-...`)
5. **IMPORTANTE**: Guarde essa chave, você só verá ela uma vez!

---

## Etapa 2: Armazenar a Chave com User Secrets (Desenvolvimento)

Abra o terminal na pasta do projeto `FrotiX.Site` e execute:

```bash
cd "C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site"
```

Agora armazene a chave (substitua `SUA-NOVA-CHAVE-AQUI` pela chave que você copiou):

```bash
dotnet user-secrets set "ClaudeAI:ApiKey" "sk-ant-api03-SUA-NOVA-CHAVE-AQUI"
```

**Exemplo real:**
```bash
dotnet user-secrets set "ClaudeAI:ApiKey" "sk-ant-api03-RVkSNcdGoh1PPg-ABC123XYZ..."
```

### Onde a chave fica armazenada?

A chave fica em um arquivo **FORA do repositório**, no seu perfil do Windows:

```
%APPDATA%\Microsoft\UserSecrets\aspnet-SmartAdmin.WebUI-E383D1D3-A583-4134-97C9-6DFEB2F1657F\secrets.json
```

Este arquivo **NUNCA** vai para o GitHub!

---

## Etapa 3: Verificar se funcionou

Execute este comando para ver os segredos armazenados:

```bash
dotnet user-secrets list
```

Você deve ver algo como:
```
ClaudeAI:ApiKey = sk-ant-api03-...
```

---

## Etapa 4: Commitar as Proteções de Segurança

Agora você pode commitar as mudanças que fiz para proteger o repositório:

```bash
git add .gitignore
git add appsettings.json
git commit -m "security: Remove chave do Claude e adiciona proteção ao .gitignore"
git push
```

**O que mudou:**
- `.gitignore`: Agora ignora arquivos `appsettings.Development.json` e outros arquivos sensíveis
- `appsettings.json`: A chave foi removida (agora tem apenas `""`)

---

## Como o código acessa a chave?

O código continua funcionando normalmente! O ASP.NET Core automaticamente:

1. Lê o `appsettings.json` (sem a chave)
2. Sobrescreve com os valores do User Secrets (com a chave)

No código C#:
```csharp
var apiKey = _configuration["ClaudeAI:ApiKey"]; // Pega do User Secrets automaticamente
```

---

## Para Produção (quando subir para o servidor)

No servidor de produção, você **NÃO** usa User Secrets. Use **variável de ambiente**:

### No Windows Server / IIS:

1. Painel de Controle → Sistema → Configurações Avançadas → Variáveis de Ambiente
2. Adicione uma nova variável:
   - Nome: `ClaudeAI__ApiKey` (dois underscores)
   - Valor: `sk-ant-api03-SUA-CHAVE-DE-PRODUCAO`

### No Linux:

Edite `/etc/environment` ou configure no serviço systemd:
```bash
export ClaudeAI__ApiKey="sk-ant-api03-SUA-CHAVE-DE-PRODUCAO"
```

---

## Checklist Final

- [ ] Gerei nova API Key no Anthropic
- [ ] Armazenei com `dotnet user-secrets set "ClaudeAI:ApiKey" "..."`
- [ ] Verifiquei com `dotnet user-secrets list`
- [ ] Commitei as proteções (`.gitignore` e `appsettings.json`)
- [ ] **NÃO** commitei nenhum arquivo com a chave dentro

---

## Observações Importantes

1. **A chave antiga (sk-ant-api03-RVk...) está MORTA** - não funciona mais
2. **User Secrets só funciona em Development** - em produção use variáveis de ambiente
3. **Nunca commite appsettings.Development.json** - agora está protegido no `.gitignore`
4. Se precisar de outras API Keys no futuro, use o mesmo processo

---

## Comandos de Referência Rápida

```bash
# Ver todos os segredos
dotnet user-secrets list

# Remover um segredo
dotnet user-secrets remove "ClaudeAI:ApiKey"

# Limpar todos os segredos
dotnet user-secrets clear

# Ver onde os segredos estão armazenados
echo %APPDATA%\Microsoft\UserSecrets\aspnet-SmartAdmin.WebUI-E383D1D3-A583-4134-97C9-6DFEB2F1657F
```

---

Pronto! Agora você pode usar a API do Claude com segurança, sem expor a chave no GitHub.
