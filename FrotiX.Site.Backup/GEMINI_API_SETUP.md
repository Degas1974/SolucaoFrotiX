# 🔑 Configuração da API Key do Gemini

## 📋 Métodos de Configuração

Existem **3 formas** de configurar a API Key do Gemini no FrotiX:

---

## ✅ Método 1: appsettings.json (Recomendado para Desenvolvimento)

### 1. Obter sua API Key

1. Acesse: <https://aistudio.google.com/app/apikey>
2. Clique em **"Get API key"** ou **"Create API key"**
3. Copie a chave gerada (formato: `AIza...`)

### 2. Editar appsettings.json

```json
"DocGenerator": {
    "DefaultProvider": "Gemini",
    "Gemini": {
        "ApiKey": "SUA_API_KEY_AQUI",  // ⬅️ COLE AQUI!
        "Model": "gemini-2.0-flash-exp",
        "BaseUrl": "https://generativelanguage.googleapis.com/v1beta",
        "MaxTokens": 8000,
        "Temperature": 0.7,
        "TopP": 0.9,
        "TopK": 40
    }
}
```

### 3. Salvar e reiniciar aplicação

```powershell
# Parar aplicação (Ctrl+C se rodando)
# Iniciar novamente
dotnet run
```

---

## ✅ Método 2: Variável de Ambiente (Recomendado para Produção)

### Windows PowerShell

```powershell
# Temporário (sessão atual)
$env:DOCGENERATOR_GEMINI_APIKEY = "AIzaSyC..."

# Permanente (usuário)
[System.Environment]::SetEnvironmentVariable("DOCGENERATOR_GEMINI_APIKEY", "AIzaSyC...", "User")

# Permanente (sistema - requer Admin)
[System.Environment]::SetEnvironmentVariable("DOCGENERATOR_GEMINI_APIKEY", "AIzaSyC...", "Machine")
```

### Windows CMD

```cmd
# Temporário (sessão atual)
set DOCGENERATOR_GEMINI_APIKEY=AIzaSyC...

# Permanente (sistema)
setx DOCGENERATOR_GEMINI_APIKEY "AIzaSyC..."
```

### Linux/Mac

```bash
# Temporário (sessão atual)
export DOCGENERATOR_GEMINI_APIKEY="AIzaSyC..."

# Permanente (adicionar ao ~/.bashrc ou ~/.zshrc)
echo 'export DOCGENERATOR_GEMINI_APIKEY="AIzaSyC..."' >> ~/.bashrc
source ~/.bashrc
```

---

## ✅ Método 3: appsettings.Development.json (Desenvolvimento Local)

### Criar/Editar appsettings.Development.json

```json
{
    "DocGenerator": {
        "Gemini": {
            "ApiKey": "AIzaSyC..."
        }
    }
}
```

**Vantagem:**

- Arquivo não commitado no Git (já está no `.gitignore`)
- Não expõe credenciais em repositório

---

## 🔍 Verificar Configuração

### Via Código (Program.cs ou Controller)

```csharp
var config = serviceProvider.GetRequiredService<IOptions<DocGeneratorSettings>>().Value;
var apiKey = config.Gemini.ApiKey;

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("❌ Gemini API Key NÃO configurada!");
}
else
{
    Console.WriteLine($"✅ Gemini API Key configurada: {apiKey.Substring(0, 10)}...");
}
```

### Via Terminal

```powershell
# Verificar variável de ambiente
$env:DOCGENERATOR_GEMINI_APIKEY
```

---

## 📝 Modelos Gemini Disponíveis (Janeiro 2026)

| Modelo | Descrição | Uso Recomendado |
|--------|-----------|-----------------|
| `gemini-2.0-flash-exp` | **Experimental** - Mais rápido e econômico | Desenvolvimento, testes |
| `gemini-2.0-flash-thinking-exp` | Com raciocínio explícito | Documentação complexa |
| `gemini-1.5-flash` | Estável, rápido | Produção (custo-benefício) |
| `gemini-1.5-pro` | Máxima qualidade | Documentação crítica |

**Recomendação FrotiX:** `gemini-2.0-flash-exp` (já configurado no appsettings.json)

---

## 🚨 Segurança

### ❌ NÃO FAZER

- ❌ Commitar API Keys no Git
- ❌ Compartilhar API Keys em chats/emails
- ❌ Usar API Keys em código front-end (JavaScript)

### ✅ FAZER

- ✅ Usar variáveis de ambiente em produção
- ✅ Adicionar `appsettings.Development.json` ao `.gitignore`
- ✅ Rotacionar API Keys periodicamente
- ✅ Usar secrets do Azure/AWS em cloud

---

## 🔗 Links Úteis

- **Gemini API Studio:** <https://aistudio.google.com/app/apikey>
- **Documentação Gemini:** <https://ai.google.dev/docs>
- **Preços Gemini:** <https://ai.google.dev/pricing>
- **Playground Gemini:** <https://aistudio.google.com/app/prompts/new_chat>

---

## 📞 Suporte

Se encontrar problemas:

1. Verificar se API Key está ativa em <https://aistudio.google.com/app/apikey>
2. Testar API Key manualmente:

   ```bash
   curl "https://generativelanguage.googleapis.com/v1beta/models?key=SUA_API_KEY"
   ```

3. Verificar logs do FrotiX em `Logs/` ou console
4. Consultar arquivo `GEMINI.md` para regras do projeto

---

**Última atualização:** 19/01/2026  
**Versão FrotiX:** 2026
