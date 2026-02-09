# 🚗 Solução FrotiX 2026

Sistema de Gestão de Frotas desenvolvido em ASP.NET Core 10.0

---

## 🎯 **DIRETÓRIO DE TRABALHO PRINCIPAL**

### ⚠️ ATENÇÃO: TODOS (Desenvolvedores & IAs)

**TRABALHE SEMPRE NO DIRETÓRIO:**

```bash
📁 FrotiX.Site.2026.01/
```

Este é o **projeto ativo principal**. Todos os outros são legados:

| Diretório | Status | Ação |
|-----------|--------|------|
| ✅ `FrotiX.Site.2026.01/` | **ATIVO** | **USE ESTE!** |
| ❌ `FrotiX.Site/` | Legado | Apenas referência |
| ❌ `FrotiX.Site.Novo/` | Legado | Apenas referência |
| ❌ `FrotiX.Site.Q4/` | Legado | Apenas referência |
| ❌ `FrotiX.Site.Backup/` | Legado | Apenas referência |

---

## 🚀 Início Rápido

### 1. Abrir no Visual Studio

```bash
# Abrir o workspace
code "Solucao FrotiX 2026.code-workspace"

# Ou abrir diretamente o projeto ativo
code FrotiX.Site.2026.01/
```

### 2. Executar Aplicação

**No Visual Studio:**
1. Selecione o perfil: **"FrotiX (Kestrel)"**
2. Pressione `F5` ou clique em ▶️
3. Acesse: **http://localhost:7100**

**Via Terminal:**
```powershell
cd FrotiX.Site.2026.01
dotnet run
```

### 3. Compilar

```powershell
cd FrotiX.Site.2026.01
dotnet build
```

---

## 🏗️ Estrutura do Projeto

```
FrotiX.Site.2026.01/
├── Controllers/          # Controllers MVC
├── Models/              # Modelos de dados
├── Views/               # Views Razor
├── wwwroot/             # Assets (JS, CSS, imagens)
├── Services/            # Lógica de negócio
├── Helpers/             # Funções auxiliares
├── Data/                # Contexto EF Core
└── Properties/          # Configurações de launch
```

---

## ⚙️ Configuração Técnica

| Item | Valor |
|------|-------|
| **Framework** | ASP.NET Core 10.0 |
| **Servidor** | Kestrel (porta 7100) |
| **Banco de Dados** | SQL Server |
| **Template** | SmartAdmin + Kendo UI |
| **Frontend** | Bootstrap 5.3.8 + jQuery |

---

## 📚 Documentação

### Para Humanos
- [Regras de Desenvolvimento](FrotiX.Site/RegrasDesenvolvimentoFrotiX.md)
- [Estrutura do Banco](FrotiX.sql)
- [Documentação Técnica](Documentacao/)

### Para IAs (GitHub Copilot, ChatGPT, Claude, etc.)
- [Instruções do Copilot](FrotiX.Site.2026.01/.copilot-instructions.md)
- [Contexto para IAs](FrotiX.Site.2026.01/.aicontext)
- [Instruções Globais](.github/copilot-instructions.md)

---

## 🤖 Configuração para Agentes de IA

Se você é uma IA (Copilot, ChatGPT, Claude, Genie, etc.):

### 📍 Lembre-se:
1. ✅ **Diretório de trabalho:** `FrotiX.Site.2026.01/`
2. ✅ **Antes de codificar:** Leia `RegrasDesenvolvimentoFrotiX.md`
3. ✅ **Ao buscar arquivos:** Priorize `FrotiX.Site.2026.01/`
4. ❌ **Não modifique:** Projetos legados (FrotiX.Site, etc.)

### 📖 Arquivos obrigatórios:
- `FrotiX.Site/RegrasDesenvolvimentoFrotiX.md` ← **LEIA SEMPRE**
- `FrotiX.Site.2026.01/.copilot-instructions.md`
- `FrotiX.sql` (quando trabalhar com banco)

---

## 🔧 Perfis de Execução

| Perfil | Servidor | URL | Uso |
|--------|----------|-----|-----|
| **FrotiX (Kestrel)** | Kestrel | http://localhost:7100 | ⭐ **Desenvolvimento diário** |
| **FrotiX (HTTPS)** | Kestrel | https://localhost:7101 | Testes com SSL |
| **SmartAdmin.WebUI** | Kestrel | https://localhost:5001 | Perfil original |

---

## 🛠️ Ferramentas Necessárias

- **Visual Studio 2025 Insiders** (ou superior)
- **.NET 10.0 SDK**
- **SQL Server** (Local ou Azure)
- **Node.js** (para dependências npm)

---

## 📝 Git Workflow

```bash
# Branch principal
git checkout main

# Sempre trabalhe em main
git pull origin main

# Commit após mudanças importantes
git add .
git commit -m "feat: descrição"
git push origin main
```

---

## 🐛 Troubleshooting

### Erro: "Failed to bind to address"
- **Solução:** Use o perfil **"FrotiX (Kestrel)"** ao invés de IIS Express
- IIS Express foi removido devido a problemas de bloqueio de arquivo

### Compilação lenta
- ✅ Target de pre-build desabilitado (economiza ~3 segundos)
- ✅ Analisadores desabilitados em builds incrementais

### Porta ocupada
- Verifique processos: `netstat -ano | findstr :7100`
- Mate o processo: `Stop-Process -Id [PID] -Force`

---

## 📞 Suporte

Para dúvidas ou problemas:
1. Consulte `RegrasDesenvolvimentoFrotiX.md`
2. Verifique `Documentacao/`
3. Contate o time de desenvolvimento

---

**✅ Workspace configurado. Bom desenvolvimento!** 🚀
