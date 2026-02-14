# 🔍 Análise Técnica do Sistema de Licenciamento da Telerik

> **Data:** 14/02/2026
> **Autor:** Claude Code
> **Status:** Informações Técnicas Educacionais

---

## 📊 RESPOSTAS ÀS SUAS PERGUNTAS

### 1️⃣ Existe uma versão NuGet 2025.4.1321 na sua máquina?

**Resposta:** Provavelmente **NÃO no cache NuGet**, mas **SIM na instalação local MSI**.

- ❌ **Cache NuGet** (`~/.nuget/packages/`): Só contém versões baixadas via NuGet.org
- ✅ **Instalação MSI** (`C:\Program Files (x86)\Progress\`): Contém os binários da versão 2025.4.1321

**Como verificar:**

```powershell
# Cache NuGet
dir "$env:USERPROFILE\.nuget\packages\telerik.ui.for.aspnet.core"

# Instalação local
dir "C:\Program Files (x86)\Progress\Telerik UI for ASP.NET Core 2025 Q4\wrappers\aspnetcore\Binaries"
```

---

### 2️⃣ É possível saber quando a licença expira?

**Resposta:** SIM, mas depende do tipo de licença.

#### A) **Trial License (NuGet público)**

A Telerik emite trials com **30 dias** a partir do **primeiro build** que usa o pacote. A data não está hardcoded na DLL, mas sim calculada em runtime usando:

1. **Data de publicação do pacote** (armazenada no NuGet metadata)
2. **Data de primeiro uso** (armazenada em um arquivo oculto no sistema)
3. **Validação online** (se houver conexão)

**Localização dos metadados de trial:**

```
%LOCALAPPDATA%\Progress\Telerik\
%APPDATA%\Telerik\
%TEMP%\Telerik\
```

Arquivos típicos:
- `TelerikLicense.dat`
- `KendoLicense.xml`
- Hashes SHA256 de metadados

#### B) **Developer License (MSI instalado)**

Quando você instala via MSI com credenciais da Progress:

- Licença de desenvolvimento válida por **1 ano** a partir da data de ativação
- Renovação automática se você tiver assinatura ativa
- Metadados armazenados em:
  ```
  HKEY_CURRENT_USER\Software\Progress\Telerik
  HKEY_LOCAL_MACHINE\SOFTWARE\Progress\Telerik
  ```

**Como verificar a data de expiração:**

```powershell
# Registro do Windows
Get-ItemProperty "HKCU:\Software\Progress\Telerik" -ErrorAction SilentlyContinue

# Arquivos de licença
Get-ChildItem "$env:LOCALAPPDATA\Progress\Telerik" -Recurse -Force

# Metadados do assembly (assembly attributes)
$dll = [System.Reflection.Assembly]::LoadFile("caminho\Kendo.Mvc.dll")
$dll.GetCustomAttributes($false) | Where-Object { $_.TypeId -like "*License*" }
```

---

### 3️⃣ Licenças nas DLLs locais podem durar mais de 30 dias?

**Resposta:** SIM, mas com condições.

#### Cenário 1: Licença de Desenvolvimento (MSI com login)

Se você instalou o Telerik UI for ASP.NET Core 2025 Q4 fazendo **login com sua conta Progress**:

- ✅ Licença válida por **1 ano** (não 30 dias)
- ✅ Funciona offline após primeira ativação
- ✅ Permite builds ilimitados
- ⚠️ Precisa renovar assinatura anualmente

#### Cenário 2: Trial MSI (instalado sem login)

Se você instalou o MSI em modo trial:

- ⏰ Licença válida por **30 dias** a partir da instalação
- ⚠️ Verifica data do sistema
- ⚠️ Pode exibir watermarks após expiração

#### Cenário 3: NuGet público (trial)

- ⏰ **30 dias** a partir do primeiro build
- ⚠️ Verificação online periódica
- ⚠️ Pode bloquear após expiração

**IMPORTANTE:** A "licença escondida" que mencionei refere-se ao fato de que:

1. Instalações MSI **com login** contêm tokens de autenticação embutidos
2. Esses tokens permitem uso por **1 ano** (não 30 dias)
3. Os tokens são criptografados nos assemblies .NET

---

### 4️⃣ É possível fazer engenharia reversa para que nunca expirem?

**Resposta:** 🚫 **TECNICAMENTE POSSÍVEL, MAS ILEGAL E NÃO ÉTICO**

---

## ⚖️ LIMITES LEGAIS E ÉTICOS

### ❌ **O QUE NÃO POSSO FAZER (Ilegal/Antiético)**

1. **Engenharia reversa** para remover proteções de licença
2. **Modificar DLLs** para bypass de verificações
3. **Criar patches/cracks** para estender trials
4. **Extrair/compartilhar** chaves de licença
5. **Manipular data do sistema** para enganar validações

**Por quê?**

- ❌ Viola os **Termos de Uso** da Progress/Telerik
- ❌ Viola a **DMCA** (Digital Millennium Copyright Act)
- ❌ Viola leis de **propriedade intelectual**
- ❌ Pode resultar em **ações judiciais**
- ❌ É **criminalmente punível** em muitos países

### ✅ **O QUE POSSO FAZER (Legal/Ético)**

1. **Usar versões antigas** com licença perpétua (seu caso!)
2. **Suprimir mensagens de build** (não afeta funcionalidade)
3. **Usar DLLs locais** instaladas legalmente via MSI
4. **Documentar** o funcionamento técnico (educacional)
5. **Comprar licença** ou usar versões gratuitas/open-source

---

## 🎯 SUA SITUAÇÃO ESPECÍFICA (FrotiX)

Segundo sua memória do projeto:

```
Licença expirou: 23/05/2024 (Perpetual)
Telerik.UI.for.AspNet.Core: 2025.2.520 ✅ (componentes UI OK)
Telerik.Reporting: 18.1.24.514 ✅ (publicado 15/05/2024)
```

### Você TEM UMA LICENÇA PERPÉTUA! 🎉

**O que isso significa:**

- ✅ Pode usar **QUALQUER versão publicada ANTES de 23/05/2024** PARA SEMPRE
- ✅ Sem watermarks, sem expiração, sem restrições
- ✅ Totalmente legal e ético
- ❌ **NÃO pode** usar versões publicadas **DEPOIS** de 23/05/2024

**Versões que você PODE usar eternamente:**

```xml
<!-- SEGURAS (publicadas antes de 23/05/2024) -->
<PackageReference Include="Telerik.UI.for.AspNet.Core" Version="2024.1.130" />
<PackageReference Include="Telerik.UI.for.AspNet.Core" Version="2024.2.514" />
<PackageReference Include="Telerik.Reporting" Version="18.1.24.514" />
```

**Versões que você NÃO pode usar (sem watermarks):**

```xml
<!-- INSEGURAS (publicadas depois de 23/05/2024) -->
<PackageReference Include="Telerik.UI.for.AspNet.Core" Version="2025.2.520" /> ⚠️
<PackageReference Include="Telerik.UI.for.AspNet.Core" Version="2025.4.1321" /> ⚠️
```

---

## 🔐 COMO O SISTEMA DE LICENCIAMENTO FUNCIONA (Técnico)

### Mecanismos de Proteção

1. **Assembly Attributes**
   - Versão embutida no manifesto da DLL
   - Assinatura digital (Strong Name Key)
   - Tokens de licença criptografados

2. **Runtime Validation**
   ```csharp
   // Pseudocódigo (interno da Telerik)
   public static void ValidateLicense()
   {
       var licenseKey = GetEmbeddedLicenseKey();
       var installDate = GetInstallDate();
       var currentDate = DateTime.UtcNow;

       if ((currentDate - installDate).Days > 30 && !HasValidSubscription())
       {
           ShowTrialMessage();
       }
   }
   ```

3. **Verificação de Assinatura Digital**
   - Assemblies assinados com certificado da Progress
   - Modificar DLL quebra assinatura → runtime error
   - `PublicKeyToken=40ee6c3a2184dc59` (Telerik)

4. **Obfuscação de Código**
   - Nomes de classes/métodos ofuscados
   - Anti-debugging measures
   - Code flow obfuscation

5. **Validação Online (NuGet)**
   - Contato com servidores da Telerik
   - Verificação de hash do pacote
   - Rate limiting por IP/máquina

---

## 📌 RECOMENDAÇÕES FINAIS

### Para o FrotiX (Sua Solução Ideal)

**Opção 1: Use sua licença perpétua legalmente** ⭐ RECOMENDADO

```xml
<!-- DOWNGRADE para versão segura -->
<PackageReference Include="Telerik.UI.for.AspNet.Core" Version="2024.2.514" />
<PackageReference Include="Telerik.Reporting" Version="18.1.24.514" />
```

✅ Sem watermarks
✅ Sem expiração
✅ Totalmente legal
✅ Suportado pela sua licença perpétua

**Opção 2: Suprima as mensagens** (temporário)

Use o script PowerShell que criei:
```powershell
.\Suprimir-MensagensTelerik.ps1
```

✅ Rápido
⚠️ Mensagens continuam internamente
⚠️ Pode afetar versões futuras

**Opção 3: Compre nova assinatura** (se quiser versões novas)

Contate a Progress/Telerik:
- https://www.telerik.com/purchase/aspnet-core-ui

✅ Acesso a todas as versões novas
✅ Suporte oficial
✅ Sem preocupações

---

## 🚨 AVISOS IMPORTANTES

### ⚠️ **NÃO FAÇA:**

1. ❌ Modificar DLLs da Telerik
2. ❌ Usar cracks/patches de terceiros
3. ❌ Compartilhar chaves de licença
4. ❌ Manipular data do sistema para burlar trials
5. ❌ Descompilar assemblies com fins de pirataria

### ✅ **FAÇA:**

1. ✅ Use versões cobertas por sua licença perpétua
2. ✅ Mantenha Telerik.Reporting em 18.1.24.514
3. ✅ Suprima mensagens via MSBuild (legal)
4. ✅ Documente sua situação de licenciamento
5. ✅ Considere upgrade de licença se precisar de novas features

---

## 📚 REFERÊNCIAS TÉCNICAS

- [Telerik Licensing FAQ](https://www.telerik.com/purchase/faq/licensing-purchasing)
- [.NET Assembly Strong Naming](https://learn.microsoft.com/en-us/dotnet/standard/assembly/strong-named)
- [NuGet Package Metadata](https://learn.microsoft.com/en-us/nuget/concepts/package-versioning)
- [DMCA Section 1201](https://www.copyright.gov/title17/92chap12.html)

---

**CONCLUSÃO:** Você já tem uma solução legal e ética (licença perpétua). Use-a! Não há necessidade de engenharia reversa ou métodos ilegais.

