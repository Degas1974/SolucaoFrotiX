# 📘 Regras de Desenvolvimento FrotiX – Arquivo Consolidado

> **Projeto:** FrotiX 2026 – FrotiX.Site
> **Tipo:** Aplicação Web ASP.NET Core MVC – Gestão de Frotas
> **Stack:** .NET 10, C#, Entity Framework Core, SQL Server, Bootstrap 5.3, jQuery, Syncfusion EJ2, Telerik UI
> **Status:** ✅ Arquivo ÚNICO e OFICIAL de regras do projeto
> **Versão:** 1.3
> **Última Atualização:** 21/01/2026

---

## 🔰 0. COMO ESTE ARQUIVO DEVE SER USADO (LEIA PRIMEIRO)

Este arquivo é a **ÚNICA FONTE DE VERDADE** para regras técnicas, padrões, fluxo de trabalho e comportamento esperado de **desenvolvedores e agentes de IA** no projeto FrotiX.

### ✅ Regras fundamentais

- Este arquivo **substitui integralmente** qualquer outro arquivo de regras
- Arquivos `README.md`, `GEMINI.md` e `CLAUDE.md` **redirecionam para este arquivo**
- Em caso de conflito de interpretação: **este arquivo sempre vence**
- Nenhum código deve ser escrito sem respeitar este documento

### 📂 Estrutura de Arquivos de Regras

```
FrotiX.Site/
├── RegrasDesenvolvimentoFrotiX.md  ← ESTE ARQUIVO (fonte única)
├── CLAUDE.md                        ← Redireciona para este
├── GEMINI.md                        ← Redireciona para este
├── FrotiX.sql                       ← Estrutura do banco (CONSULTAR SEMPRE)
└── .claude/CLAUDE.md                ← Diretrizes de documentação
```

---

## 🗄️ 1. BANCO DE DADOS – FONTE DA VERDADE

### ⚠️ REGRA CRÍTICA: SEMPRE CONSULTAR O BANCO ANTES DE CODIFICAR

O arquivo **`FrotiX.sql`** contém a estrutura REAL do banco de dados SQL Server e **DEVE SER CONSULTADO** antes de qualquer operação que envolva:

- Criação/alteração de Models
- Queries no banco de dados
- Mapeamento de campos em ViewModels
- Operações CRUD

### 📋 O que contém o FrotiX.sql

- Todas as tabelas do sistema
- Todas as views (prefixo `View_` ou `vw_`)
- Índices e constraints
- Stored Procedures
- Triggers
- Tipos de dados de cada coluna

### ✅ Fluxo OBRIGATÓRIO antes de codificar com banco

```
1. ANTES de escrever código que manipule dados:
   └─→ Ler FrotiX.sql para conferir estrutura

2. Verificar:
   ├─→ Nome exato da tabela/view
   ├─→ Nome exato das colunas
   ├─→ Tipos de dados
   ├─→ Nullable ou NOT NULL
   └─→ Relacionamentos (FKs)

3. Se precisar alterar banco:
   ├─→ Entregar script SQL
   ├─→ Explicar impacto
   └─→ Atualizar FrotiX.sql após aprovação
```

### ❌ ERROS COMUNS A EVITAR

- Assumir nome de coluna "de cabeça"
- Usar tipo errado (ex: `int` quando é `uniqueidentifier`)
- Não verificar se campo é nullable
- Confundir tabela com view
- Usar nome de coluna de outra tabela

### 📝 Quando alterar o banco

Sempre que um Model for criado/alterado ou tiver campo adicionado/removido, entregar:

```
1️⃣ Script SQL completo
2️⃣ Explicação de impacto
3️⃣ Diff mental (antes/depois)
```

**Exemplo:**

```sql
ALTER TABLE dbo.Veiculo
ADD ConsumoNormalizado DECIMAL(10,2) NULL;
```

- **Impacto:** Novo campo para métricas normalizadas
- **Antes:** campo inexistente
- **Depois:** campo disponível, nullable

📌 **Após aprovação:** Atualizar FrotiX.sql e só então ajustar código

---

## 🚨 2. REGRAS INVIOLÁVEIS (ZERO TOLERANCE)

### 2.1 TRY-CATCH (OBRIGATÓRIO)

#### ✅ C

```csharp
public IActionResult MinhaAction()
{
    try
    {
        // código
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("MeuController.cs", "MinhaAction", error);
        return Json(new { success = false, message = error.Message });
    }
}
```

#### ✅ JavaScript

```javascript
function minhaFuncao() {
  try {
    // código
  } catch (erro) {
    Alerta.TratamentoErroComLinha("arquivo.js", "minhaFuncao", erro);
  }
}
```

📌 **NUNCA** criar função sem try-catch

### 2.2 REGISTRO DE ERROS NO SISTEMA DE LOG (OBRIGATÓRIO)

#### 🎯 Contexto

O FrotiX possui um **sistema centralizado de logging** que registra todos os erros em arquivos de texto diários (`Logs/frotix_log_YYYY-MM-DD.txt`). Este sistema permite rastreamento completo de erros, auditoria e análise de problemas.

#### ⚠️ REGRA CRÍTICA

**TODOS os erros capturados em blocos `try-catch` DEVEM ser registrados no sistema de log.**

Não basta apenas usar `Alerta.TratamentoErroComLinha()` para feedback visual ao usuário. É **OBRIGATÓRIO** também registrar o erro no sistema de log para rastreabilidade e análise posterior.

#### 📋 Serviço de Log Disponível

O sistema utiliza o serviço `ILogService` que deve ser injetado via Dependency Injection:

```csharp
private readonly ILogService _logService;

public MeuController(ILogService logService)
{
    _logService = logService;
}
```

#### ✅ PADRÃO OBRIGATÓRIO - Backend (C#)

**Para Controllers e APIs:**

```csharp
[HttpGet]
public async Task<IActionResult> MinhaAction(int id)
{
    try
    {
        // Código da função
        var dados = await _repository.ObterDados(id);
        return Ok(dados);
    }
    catch (DbUpdateException dbEx)
    {
        // 1️⃣ REGISTRAR NO LOG (OBRIGATÓRIO)
        _logService.Error(
            "Erro ao atualizar banco de dados",
            dbEx,
            "MeuController.cs",
            "MinhaAction"
        );

        // 2️⃣ FEEDBACK VISUAL PARA O USUÁRIO
        Alerta.TratamentoErroComLinha("MeuController.cs", "MinhaAction", dbEx);

        return StatusCode(500, new { success = false, message = "Erro ao processar sua solicitação" });
    }
    catch (Exception error)
    {
        // 1️⃣ REGISTRAR NO LOG (OBRIGATÓRIO)
        _logService.Error(
            error.Message,
            error,
            "MeuController.cs",
            "MinhaAction"
        );

        // 2️⃣ FEEDBACK VISUAL PARA O USUÁRIO
        Alerta.TratamentoErroComLinha("MeuController.cs", "MinhaAction", error);

        return StatusCode(500, new { success = false, message = error.Message });
    }
}
```

**Para Pages (Razor Pages):**

```csharp
public class MinhaPageModel : PageModel
{
    private readonly ILogService _logService;

    public MinhaPageModel(ILogService logService)
    {
        _logService = logService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            // Código da página
            return Page();
        }
        catch (Exception error)
        {
            // 1️⃣ REGISTRAR NO LOG
            _logService.Error(
                error.Message,
                error,
                "MinhaPage.cshtml.cs",
                "OnGetAsync"
            );

            // 2️⃣ FEEDBACK VISUAL
            Alerta.TratamentoErroComLinha("MinhaPage.cshtml.cs", "OnGetAsync", error);

            return RedirectToPage("/Error");
        }
    }
}
```

**Para Services e Repositories:**

```csharp
public class MeuService
{
    private readonly ILogService _logService;

    public MeuService(ILogService logService)
    {
        _logService = logService;
    }

    public async Task<Resultado> ProcessarDados(int id)
    {
        try
        {
            // Processamento
            return resultado;
        }
        catch (Exception ex)
        {
            // REGISTRAR NO LOG
            _logService.Error(
                "Erro ao processar dados",
                ex,
                "MeuService.cs",
                "ProcessarDados"
            );

            throw; // Re-lançar para que o controller trate
        }
    }
}
```

#### ✅ PADRÃO OBRIGATÓRIO - Frontend (JavaScript)

**Para erros JavaScript, enviar para o endpoint de log:**

```javascript
function minhaFuncao() {
    try {
        // Código da função
    } catch (error) {
        // 1️⃣ REGISTRAR NO LOG DO SERVIDOR (OBRIGATÓRIO)
        fetch('/api/LogErros/LogJavaScript', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                mensagem: error.message,
                arquivo: 'meu-arquivo.js',
                metodo: 'minhaFuncao',
                linha: error.lineNumber,
                coluna: error.columnNumber,
                stack: error.stack,
                userAgent: navigator.userAgent,
                url: window.location.href
            })
        });

        // 2️⃣ FEEDBACK VISUAL
        Alerta.TratamentoErroComLinha("meu-arquivo.js", "minhaFuncao", error);
    }
}
```

#### 📊 Métodos do ILogService

| Método | Uso | Quando Usar |
|--------|-----|-------------|
| `Error(message, exception, arquivo, metodo)` | Registrar erro crítico | Em TODOS os blocos catch |
| `Warning(message, arquivo, metodo)` | Registrar aviso | Situações anormais, mas não críticas |
| `Info(message, arquivo, metodo)` | Registrar informação | Eventos importantes do sistema |
| `ErrorJS(message, arquivo, metodo, linha, coluna, stack, userAgent, url)` | Registrar erro JavaScript | Via endpoint API |
| `UserAction(action, details, usuario)` | Registrar ação de usuário | Auditoria (deleções, alterações sensíveis) |
| `HttpError(statusCode, path, method, message, usuario)` | Registrar erro HTTP | Erros 404, 500, etc |
| `OperationStart(operationName, arquivo)` | Iniciar rastreamento de operação | Processos longos (importações) |
| `OperationSuccess(operationName, details)` | Registrar sucesso de operação | Após conclusão bem-sucedida |
| `OperationFailed(operationName, exception, arquivo)` | Registrar falha de operação | Erro em processo longo |

#### 📝 Campos que DEVEM ser Preenchidos

Ao chamar `_logService.Error()`:

| Campo | Obrigatório | Descrição | Exemplo |
|-------|-------------|-----------|---------|
| `message` | ✅ Sim | Descrição do erro | `"Erro ao buscar veículo"` |
| `exception` | ✅ Sim | Objeto Exception capturado | `ex` do catch |
| `arquivo` | ✅ Sim | Nome do arquivo fonte | `"VeiculoController.cs"` |
| `metodo` | ✅ Sim | Nome do método/função | `"ObterVeiculo"` |
| `linha` | ❌ Não | Número da linha (preenchido automaticamente) | `123` |

#### 🔍 O que é Registrado no Log

Cada erro gera uma entrada completa no arquivo de log:

```
[14:30:25.456] [ERROR] ❌ NullReferenceException na busca de veículos
  📄 Arquivo: VeiculoController.cs
  🔧 Método: ObterVeiculo
  📍 Linha: 42
  🌐 URL: /api/Veiculo/Get/123
  👤 Usuário: admin@frotix.com.br
  ⚡ Exception: NullReferenceException
  💬 Message: Object reference not set to an instance of an object.
  📚 StackTrace:
      at FrotiX.Controllers.VeiculoController.ObterVeiculo(Int32 id)
      at System.Threading.Tasks.Task.Execute()
```

#### 📍 Onde Visualizar os Logs

- **Interface Web:** `/Administracao/LogErros`
  - Filtros por data, tipo de erro, busca de texto
  - Estatísticas em tempo real
  - Download de arquivo de log
  - Limpeza de logs antigos

- **Arquivo Físico:** `Logs/frotix_log_YYYY-MM-DD.txt`

#### 🎯 Cenários Especiais

**1. Operações Longas (Importações, Processamentos):**

```csharp
var operationName = "Importação de Abastecimentos";

try
{
    _logService.OperationStart(operationName, "ImportController.cs");

    // Processamento
    var totalRegistros = await ProcessarArquivo(arquivo);

    _logService.OperationSuccess(operationName, $"{totalRegistros} registros processados");
}
catch (Exception ex)
{
    _logService.OperationFailed(operationName, ex, "ImportController.cs");
    throw;
}
```

**2. Ações Sensíveis (Auditoria):**

```csharp
// Após deletar registro
_logService.UserAction(
    action: "Deletou viagem ID: 12345",
    details: "Motivo: Erro de digitação",
    usuario: User.Identity?.Name
);
```

**3. Validações que Falham (Warnings):**

```csharp
if (dataFinal < dataInicial)
{
    _logService.Warning(
        "Data final menor que data inicial",
        "ViagemController.cs",
        "ValidarDatas"
    );

    return BadRequest(new { message = "Data final deve ser maior que inicial" });
}
```

#### ❌ ERROS COMUNS A EVITAR

- ❌ Usar apenas `Alerta.TratamentoErroComLinha()` sem registrar no log
- ❌ Não injetar `ILogService` no construtor
- ❌ Registrar dados sensíveis (senhas, tokens) no log
- ❌ Não preencher `arquivo` e `metodo`
- ❌ Usar `Console.WriteLine()` ou `Debug.WriteLine()` ao invés do log

#### ✅ CHECKLIST DE IMPLEMENTAÇÃO

Ao escrever código com try-catch:

- [ ] Injetou `ILogService` no construtor?
- [ ] Chamou `_logService.Error()` no bloco catch?
- [ ] Passou `Exception` completo para o log?
- [ ] Preencheu `arquivo` e `metodo` corretamente?
- [ ] Também chamou `Alerta.TratamentoErroComLinha()` para feedback visual?
- [ ] Para JavaScript: enviou erro para `/api/LogErros/LogJavaScript`?

#### 📌 Exemplo Completo - Integração Total

```csharp
public class VeiculoController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogService _logService; // ✅ Injetado

    public VeiculoController(IUnitOfWork unitOfWork, ILogService logService)
    {
        _unitOfWork = unitOfWork;
        _logService = logService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterVeiculo(int id)
    {
        try
        {
            var veiculo = await _unitOfWork.Veiculo.Get(id);

            if (veiculo == null)
            {
                // ⚠️ Warning - não é erro crítico
                _logService.Warning(
                    $"Veículo ID {id} não encontrado",
                    "VeiculoController.cs",
                    "ObterVeiculo"
                );

                return NotFound(new { message = "Veículo não encontrado" });
            }

            return Ok(veiculo);
        }
        catch (Exception error)
        {
            // 1️⃣ REGISTRAR NO LOG (rastreabilidade)
            _logService.Error(
                $"Erro ao buscar veículo ID {id}",
                error,
                "VeiculoController.cs",
                "ObterVeiculo"
            );

            // 2️⃣ FEEDBACK VISUAL (UX)
            Alerta.TratamentoErroComLinha("VeiculoController.cs", "ObterVeiculo", error);

            // 3️⃣ RETORNO CONSISTENTE
            return StatusCode(500, new { success = false, message = "Erro ao processar sua solicitação" });
        }
    }
}
```

**Data de Adição:** 21/01/2026

### 2.3 ALERTAS E UX (SweetAlert FrotiX)

#### ❌ PROIBIDO

- `alert()`
- `confirm()`
- `prompt()`

#### ✅ OBRIGATÓRIO

```javascript
Alerta.Sucesso(titulo, msg)
Alerta.Erro(titulo, msg)
Alerta.Warning(titulo, msg)
Alerta.Info(titulo, msg)
Alerta.Confirmar(titulo, msg, btnSim, btnNao).then(ok => { ... })
Alerta.TratamentoErroComLinha(arquivo, metodo, erro)
```

**Importante:**

- Todas retornam **Promises**
- Sempre usar `.then()` ou `await`
- `Alerta.Confirmar()` retorna `true` se confirmou, `false` se cancelou

### 2.4 ÍCONES (FontAwesome DUOTONE)

#### ✅ SEMPRE

```html
<i
  class="fa-duotone fa-car"
  style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d;"
></i>
```

#### ❌ NUNCA

- `fa-solid`
- `fa-regular`
- `fa-light`
- `fa-thin`
- `fa-brands`

**Cores Padrão FrotiX:**

- **Primária:** Laranja `#ff6b35`
- **Secundária:** Cinza `#6c757d`

📌 Ícones fora do padrão devem ser convertidos: `iconClass.replace(/fa-(regular|solid|light)/g, 'fa-duotone')`

### 2.5 LOADING OVERLAY (OBRIGATÓRIO)

#### ✅ Sempre usar overlay fullscreen com logo pulsante

```html
<div class="ftx-spin-overlay">
  <div class="ftx-spin-box">
    <img
      src="/images/logo_gota_frotix_transparente.png"
      class="ftx-loading-logo"
    />
    <div class="ftx-loading-bar"></div>
    <div class="ftx-loading-text">Processando...</div>
    <div class="ftx-loading-subtext">Por favor, aguarde...</div>
  </div>
</div>
```

#### ✅ Via JavaScript (FtxSpin)

```javascript
FtxSpin.show("Carregando dados"); // Mostrar
FtxSpin.hide(); // Esconder
```

#### ❌ PROIBIDO

- Spinner Bootstrap (`spinner-border`)
- `fa-spinner fa-spin`
- Loading inline na página
- Fundo branco em modais de loading

---

## 🎨 3. PADRÕES VISUAIS

### 3.1 Botões - Paleta Oficial

| Classe              | Cor     | Quando Usar                               |
| ------------------- | ------- | ----------------------------------------- |
| `btn-azul`          | #325d88 | Salvar, Editar, Inserir, Atualizar, Criar |
| `btn-verde`         | #38A169 | Importar, Processar, Confirmar, Aprovar   |
| `btn-vinho`         | #722f37 | Cancelar, Fechar, Excluir, Apagar         |
| `btn-voltar`        | #7E583D | Voltar à lista                            |
| `btn-header-orange` | #A0522D | Ação principal em header                  |
| `btn-amarelo`       | #f59e0b | Correções automáticas                     |

### 3.2 Tooltips – SEMPRE Syncfusion

**REGRA INVIOLÁVEL:** Usar **APENAS** tooltips Syncfusion com `data-ejtip`

**NUNCA** usar tooltips Bootstrap (`data-bs-toggle="tooltip"`)

**Sintaxe correta:**

```html
<button data-ejtip="Texto do tooltip"></button>
```

**Para elementos dinâmicos (DataTables):** Usar `drawCallback` para reinicializar:

```javascript
drawCallback: function() {
    if (window.ejTooltip) {
        window.ejTooltip.refresh();
    }
}
```

### 3.3 CSS

- **Global:** `wwwroot/css/frotix.css`
- **Local:** `<style>` no `.cshtml`
- **Keyframes em Razor:** usar `@@keyframes` (escapar @)

### 3.4 Labels de Rodapé do Modal (Agendamento/Viagem)

**Contexto:** O modal de viagens/agendamentos exibe labels no rodapé indicando quem criou/agendou o registro.

**REGRA CRÍTICA:** Os campos são DIFERENTES para Agendamento vs Viagem:

| Tipo de Registro                                                | Campo de Usuário       | Campo de Data     | Label Exibida                           |
| --------------------------------------------------------------- | ---------------------- | ----------------- | --------------------------------------- |
| **Agendamento** (StatusAgendamento=true OU FoiAgendamento=true) | `usuarioIdAgendamento` | `dataAgendamento` | "Agendado por X em DD/MM/AAAA às HH:mm" |
| **Viagem** (StatusAgendamento=false E FoiAgendamento=false)     | `usuarioIdCriacao`     | `dataCriacao`     | "Criado por X em DD/MM/AAAA às HH:mm"   |

**NUNCA:**

- ❌ Usar `usuarioIdCriacao`/`dataCriacao` para registros que são/foram agendamentos
- ❌ Usar `usuarioIdAgendamento`/`dataAgendamento` para registros que nunca foram agendamentos
- ❌ Confundir `StatusAgendamento` (AINDA é agendamento) com `FoiAgendamento` (JÁ FOI agendamento)

**Implementação atual:** `exibe-viagem.js` → função `configurarRodapeLabelsExistente(objViagem)`

**Data de Adição:** 20/01/2026

---

## 🧩 4. PADRÕES DE CÓDIGO

### 4.1 Controllers / APIs

- ❌ NUNCA usar `[Authorize]` em `[ApiController]`
- Sempre retornar `{ success, message, data }` em APIs

### 4.2 Repositories e Entity Framework Tracking

**Contexto:** Para otimização de memória e performance, o Entity Framework Core foi configurado com tracking seletivo.

**Regra:** SEMPRE usar os métodos corretos do repositório dependendo da operação:

#### ✅ PARA CONSULTAS (READ-ONLY):

```csharp
// Usa AsNoTracking() internamente (mais rápido, menos memória)
var veiculo = _unitOfWork.Veiculo.Get(id);
var veiculos = _unitOfWork.Veiculo.GetAll();
var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(v => v.Placa == "ABC1234");
```

#### ✅ PARA OPERAÇÕES DE ESCRITA (UPDATE/DELETE):

```csharp
// Usa AsTracking() internamente (permite Update/Delete)
var veiculo = _unitOfWork.Veiculo.GetWithTracking(id);
veiculo.Placa = "XYZ5678";
_unitOfWork.Veiculo.Update(veiculo);

var motorista = _unitOfWork.Motorista.GetFirstOrDefaultWithTracking(m => m.CPF == cpf);
motorista.Nome = "Novo Nome";
_unitOfWork.Motorista.Update(motorista);
```

#### ❌ NUNCA FAZER:

```csharp
// ❌ ERRADO - AsTracking() não existe nas interfaces
var obj = _unitOfWork.Entity.AsTracking().Get(id);

// ❌ ERRADO - Usar Get() normal para operações de Update
var veiculo = _unitOfWork.Veiculo.Get(id); // NoTracking por padrão
veiculo.Placa = "ABC1234";
_unitOfWork.Veiculo.Update(veiculo); // ❌ Não vai funcionar corretamente
```

#### 📋 Métodos Disponíveis:

| Método                                  | Tracking       | Quando Usar             |
| --------------------------------------- | -------------- | ----------------------- |
| `Get(id)`                               | ❌ No          | Apenas visualizar dados |
| `GetWithTracking(id)`                   | ✅ Sim         | Vai fazer Update/Delete |
| `GetFirstOrDefault(filter)`             | ❌ No          | Apenas visualizar dados |
| `GetFirstOrDefaultWithTracking(filter)` | ✅ Sim         | Vai fazer Update/Delete |
| `GetAll(filter)`                        | ❌ No (padrão) | Listagens               |

**Data de Adição:** 19/01/2026

---

### 4.3 Páginas Upsert (Criar/Editar)

**Header:**

```html
<div class="ftx-card-header d-flex justify-content-between align-items-center">
  <h2 class="titulo-paginas mb-0">
    <i class="fa-duotone fa-[icone]"></i> Título
  </h2>
  <a href="/Modulo" class="btn btn-header-orange">
    <i class="fa-duotone fa-rotate-left icon-rotate-left"></i> Voltar
  </a>
</div>
```

**Botões de Ação:**

- Criar: `btn btn-azul btn-submit-spin` + ícone `fa-floppy-disk icon-pulse`
- Atualizar: `btn btn-azul btn-submit-spin` + ícone `fa-floppy-disk icon-pulse`
- Cancelar: `btn btn-vinho` + ícone `fa-circle-xmark icon-pulse`

---

## 🔄 5. FLUXO DE TRABALHO

### 5.1 Git

- **Branch preferencial:** `main`
- **Push SEMPRE para:** `main` (nunca para outras branches sem autorização explícita)
- **Commit automático** após criação/alteração de arquivos
- **Commit automático de código importante:** Sempre que código importante for fornecido durante a conversa, fazer commit e push automáticos imediatamente
- Commit apenas dos arquivos da sessão atual
- **Correção de erro próprio:** explicar erro + correção no commit

#### 5.1.1 Quando Fazer Commit e Push Automáticos

**Contexto:** Para garantir que código importante nunca seja perdido e esteja sempre versionado.

**Regra:** Fazer commit e push AUTOMÁTICOS e IMEDIATOS nas seguintes situações:

1. **Após criar/alterar arquivos de código:**
   - Arquivos `.cs`, `.cshtml`, `.js`, `.css`, `.sql`
   - Arquivos de configuração (`.json`, `.md`)

2. **Após fornecer código importante durante conversa:**
   - Implementações completas de funcionalidades
   - Correções de bugs críticos
   - Refatorações significativas
   - Novos componentes/services/controllers

3. **Após atualizar documentação:**
   - Arquivos em `Documentacao/`
   - Arquivos de regras (`RegrasDesenvolvimentoFrotiX.md`, `CLAUDE.md`, etc.)

**Processo:**

```bash
1. git add [arquivos da sessão]
2. git commit -m "[tipo]: [mensagem descritiva]"
3. git push origin main
4. Confirmar ao usuário: "✅ Código commitado e enviado para main"
```

**Tipos de commit:**

- `feat:` - Nova funcionalidade
- `fix:` - Correção de bug
- `refactor:` - Refatoração
- `docs:` - Documentação
- `style:` - Formatação/CSS
- `chore:` - Manutenção

**Exceção:** Só NÃO fazer commit automático se o usuário explicitamente pedir "não commite ainda" ou "aguarde para commitar".

**Data de Adição:** 18/01/2026

### 5.2 Documentação (Obrigatória e Detalhada)

📁 **Pastas Alvo:** `Documentacao/` e seus subdiretórios correspondentes a:

- `Controllers/`, `Services/`, `Repository/`
- `Data/` (Contextos e Configurações de Banco)
- `Helpers/` (Utilitários e Helpers customizados)
- `Hubs/` (Comunicação Real-time SignalR)
- `Middlewares/` (Pipeline de requisição e tratamento de erros)
- `Models/` (Entidades e DTOs críticos)
- `Pages/` (Páginas Razor e complementos)
- `wwwroot/js/` (Scripts globais e lógicas de front-end)

**REGRA DE OURO:** Toda alteração de código exige atualização imediata da documentação ANTES do push para `main`.

**Conteúdo Obrigatório por Arquivo `.md`:**

1. **Explicação em Prosa:** Descrição completa da funcionalidade em estilo de "prosa leve", porém tecnicamente exaustiva. Não apenas listar campos, mas explicar o _porquê_ e o _como_ o módulo interage com o sistema.
2. **Code Snippets:** Incluir trechos das principais funções/métodos (C#, JS, SQL).
3. **Detalhamento Técnico:** Cada snippet deve ser acompanhado de uma explicação linha-a-linha ou por blocos lógicos do que está sendo executado.
4. **Log de Modificações:** Manter sempre o histórico (Versão/Data/Autor/O que mudou).

📌 **Formatos:**

- `.md` (Técnico e exaustivo) - **Prioridade Máxima**
- `.html` (Visual/Portfólio A4) - Gerado a partir do `.md` quando solicitado.

### 5.3 Logs de Conversa

📁 **Pasta:** `Conversas/`

- Um `.md` por sessão
- Formato: `AAAA.MM.DD-HH.mm - [Nome].md`
- Criar no início, atualizar durante, encerrar com resumo

---

## 🤖 6. COMPORTAMENTO DOS AGENTES DE IA

### Antes de escrever código

1. ✅ Ler este arquivo
2. ✅ Consultar `FrotiX.sql` se houver operação com banco
3. ✅ Verificar estrutura existente antes de criar

### Ao detectar divergência

- ⚠️ Avisar no chat
- ❌ Não corrigir silenciosamente

### Ao alterar banco

1. Entregar Script SQL
2. Explicar Impacto
3. Fornecer Diff mental
4. Aguardar aprovação
5. Atualizar `FrotiX.sql`

### Ao criar/modificar funcionalidade

1. Verificar documentação existente em `Documentacao/`
2. Atualizar documentação se existir
3. Criar documentação se não existir

---

## 📚 7. REFERÊNCIA RÁPIDA DE ARQUIVOS

| Arquivo                          | Descrição                          |
| -------------------------------- | ---------------------------------- |
| `RegrasDesenvolvimentoFrotiX.md` | Este arquivo - regras consolidadas |
| `FrotiX.sql`                     | Estrutura do banco de dados        |
| `CLAUDE.md`                      | Redirecionador para agentes Claude |
| `GEMINI.md`                      | Redirecionador para agentes Gemini |
| `.claude/CLAUDE.md`              | Diretrizes de documentação         |
| `wwwroot/css/frotix.css`         | CSS global do sistema              |
| `wwwroot/js/frotix.js`           | JS global (inclui FtxSpin)         |
| `wwwroot/js/alerta.js`           | Sistema de alertas SweetAlert      |

---

## 🗂️ 8. VERSIONAMENTO DESTE ARQUIVO

**Formato:** `X.Y`

- **X** = mudança estrutural
- **Y** = ajustes incrementais

### Histórico de Versões

| Versão | Data       | Descrição                                                                        |
| ------ | ---------- | -------------------------------------------------------------------------------- |
| 1.3    | 21/01/2026 | Adiciona regra obrigatória de registro de erros no Sistema de Log (seção 2.2)    |
| 1.2    | 19/01/2026 | Adiciona setup OpenAI Codex no VS Code e tasks oficiais                          |
| 1.1    | 18/01/2026 | Adiciona regras de commit/push automáticos e push obrigatório para main          |
| 1.0    | 14/01/2026 | Consolidação inicial (CLAUDE.md + GEMINI.md + RegrasDesenvolvimentoFrotiXPOE.md) |

---

## 📝 5. DOCUMENTAÇÃO DE CÓDIGO (NOVO PADRÃO MANDATÓRIO)

### 5.1 Visão Geral (Cards e Tags)

Cada arquivo de código (C# ou JS) deve ser um artefato auto-explicativo. Adotamos um padrão visual de "Cards" (cabeçalhos ASCII) e "Tags Semânticas" (comentários categorizados) para garantir leitura rápida e manutenção segura.

### 5.2 Estrutura do Card (Header de Função)

**REGRA:** Toda função ou método deve começar com um bloco de comentário visualmente delimitado.

#### ✅ JAVASCRIPT (JSDoc Visual)

```javascript
/**
 * ╭──────────────────────────────────────────────────────────────────────────────
 * │ FUNCIONALIDADE: [Descrição curta e direta do que a função faz]
 * │                 [Pode ter múltiplas linhas se necessário]
 * │──────────────────────────────────────────────────────────────────────────────
 * │ CHAMADO POR:
 * │    -> [Nome da função chamadora]
 * │    -> [Evento de DOM, ex: onclick btnSalvar]
 * │──────────────────────────────────────────────────────────────────────────────
 * │ PARÂMETROS: (Opcional se óbvio)
 * │    -> [param1]: [Descrição]
 * │──────────────────────────────────────────────────────────────────────────────
 */
function nomeDaFuncao(param1) {
  // ...
}
```

#### ✅ C# (XML Docs Visual)

```csharp
/// <summary>
/// ╭──────────────────────────────────────────────────────────────────────────────
/// │ FUNCIONALIDADE: [Descrição do método/endpoint]
/// │──────────────────────────────────────────────────────────────────────────────
/// │ PARÂMETROS:
/// │    -> [param1]: [Descrição]
/// │──────────────────────────────────────────────────────────────────────────────
/// │ RETORNO:
/// │    -> [Tipo e descrição do retorno]
/// │──────────────────────────────────────────────────────────────────────────────
/// </summary>
public IActionResult NomeDaAction(int param1)
{
    // ...
}
```

### 5.3 Comentários Internos (Tags Semânticas)

**REGRA:** Não use comentários genéricos. Use Tags para categorizar o propósito do bloco de código.

| Tag                | Significado                           | Exemplo de Uso                      |
| :----------------- | :------------------------------------ | :---------------------------------- |
| `// [UI]`          | Manipulação de DOM, CSS, Visibilidade | `Elemento.style.display = 'none'`   |
| `// [LOGICA]`      | Regras de fluxo, algoritmos, loops    | `Calculo de média ponderada`        |
| `// [REGRA]`       | Regras de Negócio obrigatórias        | `Validar se data fim > data inicio` |
| `// [DADOS]`       | Manipulação de Objetos/JSON/Models    | `Mapear ViewModel para DTO`         |
| `// [AJAX]`        | Chamadas HTTP, Fetch, APIs            | `$.ajax(...)` ou `HttpClient`       |
| `// [PERFORMANCE]` | Otimizações, Cache, Lazy Load         | `Usar cache para evitar query`      |
| `// [DEBUG]`       | Logs, verificação de erros            | `console.log("Valores:", val)`      |
| `// [HELPER]`      | Funções utilitárias locais            | `FormatarData(...)`                 |

### 5.4 Exemplo Completo Aplicado

#### Exemplo JavaScript

```javascript
/**
 * ╭──────────────────────────────────────────────────────────────────────────────
 * │ FUNCIONALIDADE: Salva os dados do formulário de agendamento.
 * │──────────────────────────────────────────────────────────────────────────────
 * │ CHAMADO POR:
 * │    -> onclick #btnSalvar
 * │──────────────────────────────────────────────────────────────────────────────
 */
function salvarAgendamento() {
  try {
    // [UI] Bloquear botão para evitar duplo clique
    const btn = document.getElementById("btnSalvar");
    btn.disabled = true;
    FtxSpin.show("Salvando...");

    // [DADOS] Coletar dados do form
    const dados = obterDadosFormulario();

    // [REGRA] Validar período
    if (dados.dtFim <= dados.dtInicio) {
      // [UI] Feedback
      FtxSpin.hide();
      Alerta.Erro("Erro na Data", "Data final deve ser maior que inicial");
      return;
    }

    // [AJAX] Enviar para API
    fetch("/api/agendamentos", { method: "POST", body: JSON.stringify(dados) })
      .then((r) => r.json())
      .then((response) => {
        // [LOGICA] Verificar sucesso real da API
        if (response.success) {
          Alerta.Sucesso("Salvo", "Agendamento criado!");
          window.location.reload();
        } else {
          throw new Error(response.message);
        }
      })
      .catch((err) => {
        // [DEBUG] Log para rastreabilidade
        console.error("Erro no save:", err);
        Alerta.TratamentoErroComLinha("arquivo.js", "salvarAgendamento", err);
      });
  } catch (e) {
    Alerta.TratamentoErroComLinha("arquivo.js", "salvarAgendamento", e);
  }
}
```

---

✅ **FIM DO DOCUMENTO**

📌 **Lembrete:** Este arquivo deve ser consultado no início de cada sessão de desenvolvimento ou interação com agentes de IA.

---

## 9. SETUP OPENAI CODEX NO VS CODE (OFICIAL)

### 9.1 Extensao e permissoes

- Usar a extensao oficial OpenAI no VS Code (ID: `openai.chatgpt`)
- Habilitar Workspace Trust para permitir Actions (tasks) e acesso ao repositorio
- O agente deve ter acesso de leitura/escrita ao workspace

### 9.2 API Key (SEGURANCA)

- PROIBIDO armazenar chave OpenAI em arquivos do repositorio (`.env`, `.vscode/settings.json`, `tasks.json`, `README.md`, logs)
- Usar Secret Storage da extensao ou variavel de ambiente do usuario: `OPENAI_API_KEY`
- Se a chave for exposta em chat ou arquivo, revogar/rotacionar imediatamente

### 9.3 Actions (Tasks)

- As tarefas oficiais ficam em `.vscode/tasks.json`
- Build padrao: `dotnet build`
- Build completo: `FrotiX: build (clean + restore + build)`
- Testes: `dotnet test`
- Execucao local: `dotnet run --project FrotiX.csproj` ou `dotnet watch run --project FrotiX.csproj`

### 9.4 Fluxo Editor + Chat + Actions

- Chat gera patch e sugere tarefas
- Actions executam tasks do projeto
- Validar resultados no terminal antes de commitar

## 📝 IMPORTANTE: MEMÓRIA PERMANENTE

Este arquivo, `RegrasDesenvolvimentoFrotiX.md`, atua como a **MEMÓRIA PERMANENTE** do projeto.
Qualquer regra, padrão ou instrução que deva ser "memorizada" pelo agente deve ser adicionada aqui.

**AGENTES (Claude/Gemini/Copilot):**

1. **LEITURA OBRIGATÓRIA:** Você DEVE ler e seguir estritamente as regras deste arquivo.
2. **ESCRITA:** Se o usuário pedir para "memorizar" algo, adicione neste arquivo.

# 📜 Regras de Documentação Interna de Código

> **Adicionar ao arquivo:** RegrasDesenvolvimentoFrotiX.md  
> **Seção sugerida:** Após regras de codificação, antes de regras de UI

---

## Regra: Documentação de Funções com Card Padrão

Toda função ou método **DEVE** ter um Card de documentação inserido imediatamente antes de sua declaração.

### Card para C# (.cs)

```csharp
/// ╔══════════════════════════════════════════════════════════════════════════════╗
/// ║ 📌 NOME: NomeDaFuncao                                                        ║
/// ╠══════════════════════════════════════════════════════════════════════════════╣
/// ║ 📝 DESCRIÇÃO:                                                                ║
/// ║    Descrição detalhada do que a função faz e seu propósito.                 ║
/// ║                                                                              ║
/// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
/// ║    Por que esta função existe e qual problema resolve no FrotiX.            ║
/// ╠══════════════════════════════════════════════════════════════════════════════╣
/// ║ 📥 PARÂMETROS:                                                               ║
/// ║    • parametro1 (Tipo): O que representa e como é usado                      ║
/// ║    • parametro2 (Tipo): O que representa e como é usado                      ║
/// ║                                                                              ║
/// ║ 📤 RETORNO:                                                                  ║
/// ║    • Tipo: O que retorna                                                     ║
/// ║    • Significado: O que esse retorno representa para o algoritmo            ║
/// ║    • Consumidor: Quem/onde esse retorno é utilizado                         ║
/// ╠══════════════════════════════════════════════════════════════════════════════╣
/// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
/// ║    • NomeFuncao1() → Motivo da chamada                                       ║
/// ║    • NomeFuncao2() → Motivo da chamada                                       ║
/// ║                                                                              ║
/// ║ 📲 CHAMADA POR:                                                              ║
/// ║    • NomeFuncao3() → Em qual situação/fluxo é chamada                       ║
/// ╠══════════════════════════════════════════════════════════════════════════════╣
/// ║ 🔗 ESCOPO: [INTERNA ao módulo] ou [EXTERNA - relaciona com outros arquivos] ║
/// ║    • Arquivos relacionados: Lista de arquivos que interagem                  ║
/// ╚══════════════════════════════════════════════════════════════════════════════╝
```

### Card para JavaScript (.js)

```javascript
/**
 * ╔══════════════════════════════════════════════════════════════════════════════╗
 * ║ 📌 NOME: nomeDaFuncao                                                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════╣
 * ║ 📝 DESCRIÇÃO:                                                                ║
 * ║    Descrição detalhada do que a função faz.                                  ║
 * ║                                                                              ║
 * ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
 * ║    Por que esta função existe no FrotiX.                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════╣
 * ║ 📥 PARÂMETROS:                                                               ║
 * ║    • parametro1 (tipo): Descrição                                            ║
 * ║                                                                              ║
 * ║ 📤 RETORNO:                                                                  ║
 * ║    • Tipo e significado do retorno                                           ║
 * ╠══════════════════════════════════════════════════════════════════════════════╣
 * ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
 * ║    • funcao1() → Motivo                                                      ║
 * ║                                                                              ║
 * ║ 📲 CHAMADA POR:                                                              ║
 * ║    • funcao2() → Contexto                                                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════╣
 * ║ 🔗 ESCOPO: [INTERNA] ou [EXTERNA]                                           ║
 * ║    • Arquivos relacionados: lista                                            ║
 * ╚══════════════════════════════════════════════════════════════════════════════╝
 */
```

---

## Regra: Comentários Intra-Código

Blocos críticos de código **DEVEM** ter comentários explicativos. Não comentar linha a linha, apenas trechos importantes.

### Situações que EXIGEM comentário:

- Decisões de negócio (if/switch com regras)
- Operações complexas (LINQ, cálculos, transformações)
- Integrações com outros módulos/APIs
- Validações importantes
- Loops com lógica não trivial

### Formato padrão:

```csharp
// ═══════════════════════════════════════════════════════════════
// 🔹 BLOCO: Nome descritivo do bloco
// Explicação do que este trecho faz e por que é necessário.
// ═══════════════════════════════════════════════════════════════
```

### Exemplo:

```csharp
// ═══════════════════════════════════════════════════════════════
// 🔹 BLOCO: Cálculo de Custos da Viagem
// Soma todos os custos associados: combustível, lavador e motorista.
// ATENÇÃO: NÃO existe CustoManutencao nem CustoCaucao neste contexto.
// ═══════════════════════════════════════════════════════════════
var custoTotal = viagem.CustoCombustivel + viagem.CustoLavador + viagem.CustoMotorista;
```

---

## Regra: Try-Catch Obrigatório com Padrão FrotiX

**TODAS** as funções **DEVEM** ter tratamento de exceção com `Alerta.TratamentoErroComLinha`.

### Padrão C# - Controllers/APIs:

```csharp
public async Task<IActionResult> NomeMetodo(int id)
{
    try
    {
        // Código da função
        return Ok(resultado);
    }
    catch (Exception ex)
    {
        return BadRequest(Alerta.TratamentoErroComLinha(ex));
    }
}
```

### Padrão C# - Services/Repositories:

```csharp
public async Task<Resultado> NomeMetodo(int id)
{
    try
    {
        // Código da função
        return resultado;
    }
    catch (Exception ex)
    {
        throw new Exception(Alerta.TratamentoErroComLinha(ex));
    }
}
```

### Padrão JavaScript - Função síncrona:

```javascript
function nomeFuncao(parametro) {
  try {
    // Código da função
  } catch (error) {
    Alerta.TratamentoErroComLinha(error, "nomeFuncao");
  }
}
```

### Padrão JavaScript - Função assíncrona:

```javascript
async function nomeFuncaoAsync(parametro) {
  try {
    // Código da função
  } catch (error) {
    Alerta.TratamentoErroComLinha(error, "nomeFuncaoAsync");
  }
}
```

### Padrão JavaScript - Arrow function:

```javascript
const nomeFuncao = (parametro) => {
  try {
    // Código da função
  } catch (error) {
    Alerta.TratamentoErroComLinha(error, "nomeFuncao");
  }
};

const nomeFuncaoAsync = async (parametro) => {
  try {
    // Código da função
  } catch (error) {
    Alerta.TratamentoErroComLinha(error, "nomeFuncaoAsync");
  }
};
```

---

## Regra: Alertas Padrão SweetAlert FrotiX

**PROIBIDO** usar `alert()`, `window.alert()`, `confirm()` ou qualquer sistema de alerta nativo.

**OBRIGATÓRIO** usar o padrão `Alerta.*` do FrotiX (SweetAlert).

### Tabela de Substituição:

| ❌ PROIBIDO (Legado)  | ✅ OBRIGATÓRIO (Padrão FrotiX)                             |
| --------------------- | ---------------------------------------------------------- |
| `alert("mensagem")`   | `Alerta.Mensagem("mensagem")`                              |
| `alert("Erro...")`    | `Alerta.Erro("mensagem")`                                  |
| `alert("Sucesso...")` | `Alerta.Sucesso("mensagem")`                               |
| `confirm("pergunta")` | `Alerta.Confirmacao("pergunta", callbackSim, callbackNao)` |
| `window.alert(...)`   | `Alerta.Mensagem(...)`                                     |

### Exemplo de Confirmação:

```javascript
// ❌ PROIBIDO
if (confirm("Deseja excluir este registro?")) {
  excluirRegistro(id);
}

// ✅ CORRETO
Alerta.Confirmacao(
  "Deseja excluir este registro?",
  () => excluirRegistro(id), // callback SIM
  null, // callback NÃO (opcional)
);
```

---

## Regra: Toast Padrão AppToast FrotiX

**PROIBIDO** usar `toastr`, `toast()` ou qualquer sistema de notificação toast que não seja `AppToast`.

**OBRIGATÓRIO** usar `AppToast.show(estilo, mensagem, duracao)`.

### Estilos Disponíveis:

| Estilo      | Cor      | Uso                            |
| ----------- | -------- | ------------------------------ |
| `'sucesso'` | Verde    | Operações concluídas com êxito |
| `'erro'`    | Vermelho | Falhas e erros                 |
| `'aviso'`   | Amarelo  | Alertas e avisos               |
| `'info'`    | Azul     | Informações gerais             |

### Tabela de Substituição:

| ❌ PROIBIDO (Legado)    | ✅ OBRIGATÓRIO (Padrão FrotiX)          |
| ----------------------- | --------------------------------------- |
| `toastr.success("msg")` | `AppToast.show('sucesso', 'msg', 3000)` |
| `toastr.error("msg")`   | `AppToast.show('erro', 'msg', 5000)`    |
| `toastr.warning("msg")` | `AppToast.show('aviso', 'msg', 4000)`   |
| `toastr.info("msg")`    | `AppToast.show('info', 'msg', 3000)`    |

### Durações Recomendadas:

- Sucesso: 3000ms (3 segundos)
- Info: 3000ms (3 segundos)
- Aviso: 4000ms (4 segundos)
- Erro: 5000ms (5 segundos)

---

## Regra: Ícones Padronizados para Documentação

| Ícone | Uso                                       |
| ----- | ----------------------------------------- |
| 📌    | Nome/Identificação da função              |
| 📝    | Descrição                                 |
| 🎯    | Importância/Objetivo                      |
| 📥    | Parâmetros de entrada                     |
| 📤    | Retorno/Saída                             |
| 📞    | Funções que chama                         |
| 📲    | Chamada por                               |
| 🔗    | Escopo/Relacionamentos                    |
| 🔹    | Bloco de código (comentário intra-código) |
| 🛡️    | Try-Catch                                 |
| 🔔    | Alerta/Toast                              |

---

## Regra: Arquivos de Controle de Documentação

Manter na pasta `Documentacao/Comentarios/`:

| Arquivo                    | Finalidade                                    |
| -------------------------- | --------------------------------------------- |
| `AndamentoComentarios.md`  | Controle de progresso da documentação         |
| `PendenciasComentarios.md` | Relacionamentos pendentes de identificação    |
| `MapaRelacionamentos.md`   | Visão geral dos relacionamentos entre módulos |

---

## Regra: Segurança nas Alterações

Ao inserir Try-Catch ou substituir Alertas/Toasts:

1. **NÃO ALTERAR** a lógica de execução do algoritmo
2. **NÃO MODIFICAR** returns, awaits ou fluxo de dados
3. **PRESERVAR** todos os parâmetros e tipos de retorno originais
4. **TESTAR** mentalmente a lógica antes de envolver com Try-Catch
5. **MANTER** o código compilável após cada alteração
