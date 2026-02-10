# Guia Passo a Passo: Gerar e Configurar a Chave do Claude

## Passo 1: Gerar a Nova Chave

1. Abra o navegador e acesse: **https://platform.claude.com/settings/keys**
2. Faça login na sua conta Anthropic (se ainda não estiver logado)
3. Clique no botão **"Create Key"** ou **"+ Create API Key"**
4. Na tela de criação:
   - **Name**: Digite `FrotiX-Dev` (ou qualquer nome que quiser)
   - Clique em **"Create Key"**
5. **COPIE A CHAVE AGORA** - ela aparece assim: `sk-ant-api03-xxxxxxxxxx...`
   - Você só verá essa chave uma vez!
   - Guarde em um lugar seguro (Notepad temporariamente)

---

## Passo 2: Criar/Editar o Arquivo secrets.json

### Opção A: Usando o Comando (RECOMENDADO - mais fácil)

Abra o **PowerShell** ou **CMD** e execute:

```powershell
cd "C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site"
dotnet user-secrets set "ClaudeAI:ApiKey" "COLE-SUA-CHAVE-AQUI"
```

**Exemplo real:**
```powershell
dotnet user-secrets set "ClaudeAI:ApiKey" "sk-ant-api03-RVkSNcdGoh1PPg-ABC123..."
```

Pronto! O comando cria tudo automaticamente.

---

### Opção B: Editando Manualmente o Arquivo

Se preferir fazer manualmente:

**1. Abra o Explorador de Arquivos**

**2. Cole este caminho na barra de endereços e aperte ENTER:**
```
%APPDATA%\Microsoft\UserSecrets\aspnet-SmartAdmin.WebUI-E383D1D3-A583-4134-97C9-6DFEB2F1657F
```

**3. Se a pasta NÃO existir:**
   - Volte para: `%APPDATA%\Microsoft\UserSecrets`
   - Crie uma nova pasta com o nome exato: `aspnet-SmartAdmin.WebUI-E383D1D3-A583-4134-97C9-6DFEB2F1657F`
   - Entre nessa pasta

**4. Dentro da pasta, crie/edite o arquivo `secrets.json`:**
   - Se o arquivo já existe, abra com Notepad
   - Se não existe, crie um novo arquivo chamado `secrets.json`

**5. Cole este conteúdo no arquivo:**

```json
{
  "ClaudeAI:ApiKey": "sk-ant-api03-COLE-SUA-CHAVE-AQUI"
}
```

**Exemplo com chave real:**
```json
{
  "ClaudeAI:ApiKey": "sk-ant-api03-RVkSNcdGoh1PPg-ABC123XYZ789..."
}
```

**6. Salve o arquivo** (Ctrl+S)

---

## Passo 3: Verificar se Funcionou

Abra o PowerShell na pasta do projeto:

```powershell
cd "C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site"
dotnet user-secrets list
```

Deve mostrar:
```
ClaudeAI:ApiKey = sk-ant-api03-...
```

---

## Passo 4: Testar no Projeto

Rode o projeto:

```powershell
dotnet run
```

ou abra no Visual Studio e execute (F5).

O código vai ler a chave automaticamente do User Secrets!

---

## Formato Completo do secrets.json (caso tenha mais segredos)

Se você tiver mais configurações sensíveis no futuro:

```json
{
  "ClaudeAI:ApiKey": "sk-ant-api03-...",
  "ConnectionStrings:DefaultConnection": "...",
  "MailSettings:Password": "..."
}
```

Note que usa `:` (dois pontos) para separar seções, não `__` (underscores).

---

## Resumo Visual

```
1. Site Anthropic          2. Copiar Chave                3. secrets.json
   ┌─────────────┐            ┌──────────────┐              ┌─────────────────┐
   │ Create Key  │  ──────>   │ sk-ant-api03 │  ──────>     │ {               │
   │   FrotiX    │            │ RVk...BAAA   │              │   "ClaudeAI:... │
   └─────────────┘            └──────────────┘              │ }               │
                                                             └─────────────────┘
                                                             %APPDATA%\...
```

---

## Dica Extra

Se quiser ver o caminho completo do arquivo, cole no PowerShell:

```powershell
echo $env:APPDATA\Microsoft\UserSecrets\aspnet-SmartAdmin.WebUI-E383D1D3-A583-4134-97C9-6DFEB2F1657F\secrets.json
```

Isso mostra: `C:\Users\SEU-USUARIO\AppData\Roaming\Microsoft\UserSecrets\aspnet-SmartAdmin...\secrets.json`

---

## Comandos Úteis de Referência

```powershell
# Ver todos os segredos armazenados
dotnet user-secrets list

# Adicionar/atualizar um segredo
dotnet user-secrets set "Chave:Subchave" "valor"

# Remover um segredo específico
dotnet user-secrets remove "ClaudeAI:ApiKey"

# Limpar TODOS os segredos
dotnet user-secrets clear
```

---

## Para Produção (Servidor)

No servidor de produção, **NÃO use User Secrets**. Use **variáveis de ambiente**:

### Windows Server / IIS:
1. Painel de Controle → Sistema → Configurações Avançadas → Variáveis de Ambiente
2. Adicione: `ClaudeAI__ApiKey` (dois underscores) com o valor da chave

### Linux:
```bash
export ClaudeAI__ApiKey="sk-ant-api03-SUA-CHAVE-DE-PRODUCAO"
```

---

**IMPORTANTE**: Use a Opção A (comando dotnet) - é mais rápido e não tem risco de erro no JSON!

---

## Troubleshooting

**Problema**: "O termo 'dotnet' não é reconhecido"
- **Solução**: Instale o .NET SDK: https://dotnet.microsoft.com/download

**Problema**: A chave não está sendo lida no código
- **Solução 1**: Verifique se está rodando em modo Development (não Release)
- **Solução 2**: Execute `dotnet user-secrets list` para confirmar que a chave está lá
- **Solução 3**: Reinicie o Visual Studio/aplicação

**Problema**: O arquivo secrets.json não existe
- **Solução**: Use o comando `dotnet user-secrets set` que ele cria automaticamente
