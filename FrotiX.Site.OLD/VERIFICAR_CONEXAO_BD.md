# ✅ Guia de Verificação - Conexão com Banco de Dados FrotiX

## 📋 Status da Configuração

### ✅ Configurações Corretas Identificadas

| Item | Status | Detalhes |
|------|--------|----------|
| SQL Server Local | ✅ **RODANDO** | Serviço `MSSQLSERVER` está ativo |
| `appsettings.Development.json` | ✅ **CRIADO** | Arquivo criado na raiz do projeto |
| `launchSettings.json` | ✅ **CONFIGURADO** | `ASPNETCORE_ENVIRONMENT = Development` |
| Connection String (Dev) | ✅ **LOCALHOST** | Aponta para `localhost` com Windows Auth |
| Connection String (Prod) | ✅ **CTRAN01** | Aponta para servidor de produção |

---

## 🔍 Possíveis Causas do Erro

O erro **"provider: Provedor de Pipes Nomeados, error: 40"** geralmente ocorre por:

### 1. Banco de dados "Frotix" não existe no SQL Server local

**Verificar:**
```sql
-- Abra SQL Server Management Studio (SSMS) e execute:
SELECT name FROM sys.databases WHERE name = 'Frotix';
```

**Se não existir:**
- ✅ Restaurar backup do banco `Frotix` no SQL Server local
- ✅ Ou executar o script `FrotiX.sql` para criar o banco

### 2. Aplicação não está usando `appsettings.Development.json`

**Verificar no código:**
- Certifique-se de rodar com perfil **"Development"**
- No Visual Studio: Verifique o perfil de execução (dropdown na toolbar)
- Via `dotnet run`: Use `dotnet run --environment Development`

### 3. Named Pipes não habilitado no SQL Server

**Verificar:**
1. Abra **SQL Server Configuration Manager**
2. Vá em: `SQL Server Network Configuration` → `Protocols for MSSQLSERVER`
3. Certifique-se que **"Named Pipes"** e **"TCP/IP"** estão **Enabled**
4. Se alterar, reinicie o serviço SQL Server

### 4. Permissões de Windows Authentication

**Verificar:**
- Seu usuário Windows precisa ter acesso ao SQL Server local
- No SSMS, vá em: `Security` → `Logins`
- Adicione seu usuário Windows com permissões `db_owner` no banco `Frotix`

---

## 🔧 Solução Rápida (Testar Conexão)

### Opção 1: Via SSMS (SQL Server Management Studio)
1. Abra SSMS
2. Server name: `localhost` ou `(local)` ou `.`
3. Authentication: `Windows Authentication`
4. Clique em **Connect**
5. Verifique se o banco `Frotix` aparece na lista de databases

### Opção 2: Via Command Line
```bash
# Testar conexão básica
sqlcmd -S localhost -E -Q "SELECT @@SERVERNAME, @@VERSION"

# Verificar se banco Frotix existe
sqlcmd -S localhost -E -Q "SELECT name FROM sys.databases WHERE name = 'Frotix'"
```

### Opção 3: Alterar Connection String Temporariamente

Se o problema persistir, tente diferentes formatos de Data Source:

**No `appsettings.Development.json`, teste uma dessas opções:**

```json
// Opção 1: Localhost com ponto
"Data Source='.';Initial Catalog=Frotix;Trusted_Connection=True;..."

// Opção 2: (local)
"Data Source='(local)';Initial Catalog=Frotix;Trusted_Connection=True;..."

// Opção 3: Nome da máquina (substitua SEU_NOME_PC)
"Data Source='SEU_NOME_PC';Initial Catalog=Frotix;Trusted_Connection=True;..."

// Opção 4: TCP/IP explícito (forçar TCP em vez de Named Pipes)
"Data Source='localhost,1433';Initial Catalog=Frotix;Trusted_Connection=True;..."
```

---

## 🚀 Checklist de Resolução

Execute na ordem:

- [ ] **1.** Confirmar que SQL Server está rodando
  ```bash
  net start | grep -i "SQL Server (MSSQLSERVER)"
  ```

- [ ] **2.** Verificar se banco `Frotix` existe localmente
  ```sql
  SELECT name FROM sys.databases WHERE name = 'Frotix';
  ```

- [ ] **3.** Se não existir, restaurar backup ou executar `FrotiX.sql`

- [ ] **4.** Confirmar que `appsettings.Development.json` está na raiz
  ```
  c:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\appsettings.Development.json
  ```

- [ ] **5.** Rodar aplicação em modo Development
  ```bash
  cd "c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site.OLD"
  dotnet run --environment Development
  ```

- [ ] **6.** Verificar logs da aplicação para confirmar qual connection string está sendo usada

- [ ] **7.** Se problema persistir, habilitar Named Pipes no SQL Server Configuration Manager

---

## 📝 Logs Úteis

Adicione logging temporário no `Startup.cs` ou `Program.cs`:

```csharp
// Ver qual connection string está sendo usada
var connString = Configuration.GetConnectionString("FrotiX");
Console.WriteLine($"[DEBUG] Connection String: {connString}");
```

---

## 🆘 Se Nada Funcionar

**Connection String de fallback (SQL Server Authentication):**

Se Windows Auth não funcionar, crie um login SQL:

```sql
-- No SSMS, execute:
USE [master]
GO
CREATE LOGIN [FrotixDev] WITH PASSWORD=N'Dev@123!', DEFAULT_DATABASE=[Frotix]
GO
USE [Frotix]
GO
CREATE USER [FrotixDev] FOR LOGIN [FrotixDev]
GO
ALTER ROLE [db_owner] ADD MEMBER [FrotixDev]
GO
```

**Depois altere `appsettings.Development.json`:**
```json
"FrotiX": "Data Source=localhost;Initial Catalog=Frotix;User ID=FrotixDev;Password=Dev@123!;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

---

## ✅ Teste Final

Após resolver, teste com:

```bash
cd "c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site.OLD"
dotnet run --environment Development
```

Acesse: `http://localhost:5000` e tente fazer login.

---

**Criado em:** 10/02/2026
**Projeto:** FrotiX.Site.OLD
**Ambiente:** Development (Local)
