# 🔑 Configuração da API Key do OpenAI

## 📋 Métodos de Configuração

Existem **3 formas** de configurar a API Key do OpenAI no FrotiX para uso no `DocGenerator`:

---

## ✅ Método 1: appsettings.json (Desenvolvimento Local)

### 1. Obter sua API Key

1. Acesse: [https://platform.openai.com/api-keys](https://platform.openai.com/api-keys)
2. Faça login com sua conta OpenAI.
3. Clique em **"Create new secret key"**.
4. Copie a chave gerada (formato: `sk-proj-...`).

### 2. Editar appsettings.json

Localize a seção `DocGenerator -> OpenAI` e cole sua chave:

```json
"DocGenerator": {
    "DefaultProvider": "OpenAI", // Opcional: define OpenAI como padrão
    "OpenAI": {
        "ApiKey": "SUA_API_KEY_AQUI", // ⬅️ COLE AQUI!
        "Model": "gpt-4o",
        "BaseUrl": "https://api.openai.com/v1",
        "MaxTokens": 4000,
        "Temperature": 0.7
    }
}
```

---

## ✅ Método 2: Variável de Ambiente (Recomendado para Produção)

O `OpenAiDocProvider` procura pela variável de ambiente `DOCGENERATOR_OPENAI_APIKEY`.

### Windows PowerShell

```powershell
[System.Environment]::SetEnvironmentVariable("DOCGENERATOR_OPENAI_APIKEY", "sua_chave_aqui", "User")
```

---

## ✅ Método 3: appsettings.Development.json

Recomendado para evitar commit de chaves no repositório.

```json
{
  "DocGenerator": {
    "OpenAI": {
      "ApiKey": "sk-proj-..."
    }
  }
}
```

---

## 🚀 Como Testar

Após configurar, você pode alterar o `DefaultProvider` para `"OpenAI"` no `appsettings.json` e reiniciar a aplicação.
