# 📘 Regras de Desenvolvimento FrotiX – Arquivo Consolidado

> **Projeto:** FrotiX 2026 – FrotiX.Site
> **Tipo:** Aplicação Web ASP.NET Core MVC – Gestão de Frotas
> **Stack:** .NET 10, C#, Entity Framework Core, SQL Server, Bootstrap 5.3, jQuery, Syncfusion EJ2, Telerik UI
> **Status:** ✅ Arquivo ÚNICO e OFICIAL de regras do projeto
> **Versão:** 1.5
> **Última Atualização:** 03/02/2026

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

#### ✅ C #

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

### 2.2 ALERTAS E UX (SweetAlert FrotiX)

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

### 2.3 ÍCONES (FontAwesome DUOTONE)

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

### 2.4 LOADING OVERLAY (OBRIGATÓRIO)

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

### 3.3 Telerik/Kendo – Localização pt-BR (OBRIGATÓRIO)

**REGRA INVIOLÁVEL:** Todo e qualquer controle **Telerik/Kendo** DEVE ser inicializado em **pt-BR**.

**Requisitos mínimos:**
- Carregar os scripts de cultura e mensagens **da mesma versão** do Kendo usada na página.
- Executar `kendo.culture("pt-BR")` **antes** de inicializar qualquer widget.

**Exemplo correto:**

```html
<script src="https://kendo.cdn.telerik.com/2025.2.520/js/kendo.all.min.js"></script>
<script src="https://kendo.cdn.telerik.com/2025.2.520/js/cultures/kendo.culture.pt-BR.min.js"></script>
<script src="https://kendo.cdn.telerik.com/2025.2.520/js/messages/kendo.messages.pt-BR.min.js"></script>
<script>
  if (window.kendo && kendo.culture) {
    kendo.culture("pt-BR");
  }
</script>
```

**Observações:**
- Se houver **mais de um carregamento** do Kendo na página, a cultura deve ser aplicada **após o último carregamento**.
- Não confiar no idioma padrão do navegador.

### 3.3.1 Telerik/Kendo – Timing de Inicialização (OBRIGATÓRIO)

**REGRA INVIOLÁVEL:** Toda inicialização de widgets Kendo via JavaScript DEVE ocorrer dentro de `$(document).ready()` ou evento equivalente, **após** o carregamento completo da biblioteca.

**Motivo:** O erro `kendo is not defined` ocorre quando código JavaScript tenta usar objetos Kendo **antes** da biblioteca ter sido carregada. Isso acontece frequentemente quando:
- Scripts são colocados no `<head>` ou início da página
- Código Kendo está fora de `$(document).ready()`
- Múltiplos carregamentos de Kendo criam race conditions

**✅ CORRETO – Inicialização dentro de $(document).ready() com verificação:**

```javascript
$(document).ready(function() {
    // Verificar se Kendo está disponível antes de usar
    if (typeof kendo !== 'undefined' && kendo.ui) {
        $("#meuUpload").kendoUpload({
            async: { saveUrl: "/api/upload", removeUrl: "/api/remove" }
        });
    }
});
```

**✅ CORRETO – Override de método com verificação:**

```javascript
$(document).ready(function() {
    if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.Upload) {
        // Desabilitar drag-drop em mobile
        kendo.ui.Upload.fn._supportsDrop = function() { 
            return false; 
        };
    }
});
```

**❌ ERRADO – Fora de $(document).ready():**

```javascript
// NUNCA fazer isso! Causa "kendo is not defined"
kendo.ui.Upload.fn._supportsDrop = function() { return false; };

$("#elemento").kendoDatePicker({ format: "dd/MM/yyyy" });
```

**Checklist de Auditoria Kendo:**
1. Todo código `.kendo*()` está dentro de `$(document).ready()`?
2. Todo acesso a `kendo.ui.*` tem verificação `if (typeof kendo !== 'undefined')`?
3. Scripts de cultura (`kendo.culture.pt-BR.js`) carregam **após** `kendo.all.min.js`?
4. Não há duplicação de carregamento do Kendo na mesma página?

### 3.4 CSS

- **Global:** `wwwroot/css/frotix.css`
- **Local:** `<style>` no `.cshtml`
- **Keyframes em Razor:** usar `@@keyframes` (escapar @)

---

## 🧩 4. PADRÕES DE CÓDIGO

### 4.1 Controllers / APIs

- ❌ NUNCA usar `[Authorize]` em `[ApiController]`
- Sempre retornar `{ success, message, data }` em APIs

### 4.2 Páginas Upsert (Criar/Editar)

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

## 🌐 4.5 TRATAMENTO DE ERROS E APIS (PADRÃO OBRIGATÓRIO)

> **Adicionado em:** 01/02/2026 | **Motivo:** Resolver problema de "Script error" e padronizar respostas da API

### 4.5.1 Resposta Padronizada da API (ApiResponse)

**REGRA:** Todos os endpoints API devem retornar o formato `ApiResponse<T>`.

```csharp
// Sucesso
return Ok(new ApiResponse<object>
{
    Success = true,
    Data = result,
    Message = "Operação realizada com sucesso",
    RequestId = requestId  // Guid.NewGuid().ToString("N")[..8]
});

// Erro
return StatusCode(500, ApiResponse<object>.FromException(ex, includeDetails: isDevelopment));
```

**Formato JSON:**
```json
{
  "success": true,
  "data": [...],
  "message": "10 veículo(s) encontrado(s)",
  "requestId": "a1b2c3d4"
}
```

📁 **Arquivo:** `Models/Api/ApiResponse.cs`

### 4.5.2 Headers CORS Obrigatórios

**REGRA:** O CORS deve expor headers para rastreamento de erros.

```csharp
// Startup.cs - ConfigureServices
services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders(
            "Content-Disposition",    // Downloads
            "X-Request-Id",           // Rastreamento de erros
            "X-Error-Details"         // Detalhes de erro (debug)
        ));
});
```

### 4.5.3 Scripts CDN - crossorigin Obrigatório

**REGRA:** Todos os scripts de CDN externos DEVEM ter `crossorigin="anonymous"`.

**Por quê?** Sem esse atributo, erros de scripts externos aparecem como "Script error." sem detalhes.

```html
<!-- ✅ CORRETO -->
<script src="https://cdn.example.com/lib.js" crossorigin="anonymous"></script>

<!-- ❌ ERRADO -->
<script src="https://cdn.example.com/lib.js"></script>
```

📁 **Arquivo:** `Pages/Shared/_ScriptsBasePlugins.cshtml`

### 4.5.4 Cliente API JavaScript (FrotiXApi)

**REGRA:** Preferir `FrotiXApi` para chamadas AJAX com tratamento robusto.

```javascript
// ✅ USAR FrotiXApi (recomendado)
FrotiXApi.get('/api/Veiculo/GetAll')
    .then(function(response) {
        if (response.success) {
            console.log('Dados:', response.data);
        }
    })
    .catch(function(error) {
        // error.requestId permite rastrear no servidor
        AppToast.show('Vermelho', error.message + ' (ID: ' + error.requestId + ')');
        Alerta.TratamentoErroComLinha('meuArquivo.js', 'minhaFuncao', error);
    });
```

**Recursos do FrotiXApi:**
- Retry automático (2x) para erros de rede
- RequestId em todas as requisições
- Envio automático de erros para `/api/LogErros/Client`
- Timeout configurável (padrão: 30s)

📁 **Arquivo:** `wwwroot/js/frotix-api-client.js`

### 4.5.5 Handlers Globais de Erro JavaScript

**REGRA:** Os handlers globais já estão configurados no `_Layout.cshtml`. Não remover!

```html
<!-- Ordem de carregamento (NÃO ALTERAR) -->
<script src="~/js/global-error-handler.js"></script>  <!-- window.onerror -->
<script src="~/js/frotix-api-client.js"></script>     <!-- FrotiXApi -->
<script src="~/js/frotix-error-logger.js"></script>   <!-- Logger -->
<script src="~/js/console-interceptor.js"></script>   <!-- console.* -->
```

**Erros capturados automaticamente:**
- `window.onerror` (erros de sintaxe/runtime)
- `unhandledrejection` (Promises sem catch)
- `console.error` (erros logados no console)
- Falhas de AJAX/fetch

📁 **Arquivos:** `wwwroot/js/global-error-handler.js`, `wwwroot/js/console-interceptor.js`

### 4.5.6 Endpoint de Logs do Cliente

**REGRA:** Erros do frontend são enviados para `/api/LogErros/Client`.

```javascript
// Enviado automaticamente pelo FrotiXApi e global-error-handler.js
POST /api/LogErros/Client
{
    "Tipo": "HTTP-ERROR",      // ou GLOBAL-ERROR, UNHANDLED-PROMISE
    "Mensagem": "Erro ao carregar dados",
    "StatusCode": 500,
    "RequestId": "a1b2c3d4",
    "Url": "https://frotix.com/veiculos",
    "UserAgent": "Mozilla/5.0...",
    "Timestamp": "2026-02-01T10:30:00Z"
}
```

📁 **Arquivo:** `Controllers/LogErrosController.cs`

### 4.5.7 Referência Rápida - Arquivos de Tratamento de Erros

| Arquivo | Descrição |
|---------|-----------|
| `Models/Api/ApiResponse.cs` | Classe padronizada para respostas da API |
| `wwwroot/js/global-error-handler.js` | Captura `window.onerror` e `unhandledrejection` |
| `wwwroot/js/frotix-api-client.js` | Cliente HTTP robusto com retry e logging |
| `wwwroot/js/console-interceptor.js` | Intercepta `console.*` e envia para servidor |
| `Controllers/LogErrosController.cs` | Endpoints para receber logs do cliente |

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

### 5.1.2 Comando explícito do usuário

Quando o usuário disser **"Faça comit e push para o Main"**, executar **imediatamente**:

```bash
git add -A
git commit -m "chore: commit solicitado pelo usuário"
git push origin main
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

| Arquivo                          | Descrição                              |
| -------------------------------- | -------------------------------------- |
| `RegrasDesenvolvimentoFrotiX.md` | Este arquivo - regras consolidadas     |
| `FrotiX.sql`                     | Estrutura do banco de dados            |
| `CLAUDE.md`                      | Redirecionador para agentes Claude     |
| `GEMINI.md`                      | Redirecionador para agentes Gemini     |
| `.claude/CLAUDE.md`              | Diretrizes de documentação             |
| `wwwroot/css/frotix.css`         | CSS global do sistema                  |
| `wwwroot/js/frotix.js`           | JS global (inclui FtxSpin)             |
| `wwwroot/js/alerta.js`           | Sistema de alertas SweetAlert          |
| `wwwroot/js/frotix-api-client.js`| Cliente API robusto (FrotiXApi)        |
| `wwwroot/js/global-error-handler.js` | Handler global de erros JS         |
| `Models/Api/ApiResponse.cs`      | Classe padronizada para respostas API  |

---

## 🗂️ 8. VERSIONAMENTO DESTE ARQUIVO

**Formato:** `X.Y`

- **X** = mudança estrutural
- **Y** = ajustes incrementais

### Histórico de Versões

| Versão | Data       | Descrição                                                                        |
| ------ | ---------- | -------------------------------------------------------------------------------- |
| 1.5    | 03/02/2026 | Adiciona seção 5.13 (Guia de Enriquecimento - Segunda Passada) com checklist detalhado por tipo de arquivo, exemplos de antes/depois, e workflow para agentes Haiku de enriquecimento de documentação |
| 1.4    | 03/02/2026 | Adiciona seções 5.11 (Mapeamento de Dependências) e 5.12 (Análise de Arquivos Críticos). Atualiza 5.6 (🎯 MOTIVO em AJAX) e 5.9 (símbolos ⬅️ ➡️). Estabelece regra de limpeza do ArquivosCriticos.md |
| 1.3    | 01/02/2026 | Adiciona seção 4.5 - Tratamento de Erros e APIs (ApiResponse, CORS, FrotiXApi)   |
| 1.2    | 29/01/2026 | Atualização completa dos padrões visuais de Cards (Arquivo e Função) com ícones  |
| 1.1    | 18/01/2026 | Adiciona regras de commit/push automáticos e push obrigatório para main         |
| 1.0    | 14/01/2026 | Consolidação inicial (CLAUDE.md + GEMINI.md + RegrasDesenvolvimentoFrotiXPOE.md) |

---

## 📝 5. DOCUMENTAÇÃO INTRA-CÓDIGO (PADRÃO OBRIGATÓRIO)

> 📁 **Arquivo de Acompanhamento:** `DocumentacaoIntracodigo.md` - Usado para mapear o andamento do processo de documentação

### 5.1 Visão Geral

Cada arquivo de código (C#, JS ou CSHTML) deve ser um artefato auto-explicativo. Adotamos um padrão de documentação com **headers descritivos** e **comentários robustos** para garantir leitura rápida, manutenção segura e rastreabilidade completa.

---

### 5.2 Card do Arquivo (Header Principal)

**REGRA:** Todo arquivo (.cs, .js ou .cshtml) DEVE iniciar com um **Card de Identificação** descrevendo:
- Objetivo do arquivo
- Entradas e saídas esperadas
- Quem chama e o que chama
- Dependências principais

#### ✅ Modelo para C# (Controllers, Services, Repositories)

```csharp
/* ****************************************************************************************
 * ⚡ ARQUIVO: NomeDoArquivo.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Descrição clara e objetiva da responsabilidade do arquivo.
 *
 * 📥 ENTRADAS     : Tipos de requisições ou parâmetros que este arquivo recebe.
 *
 * 📤 SAÍDAS       : Tipo de resposta (JSON, View, ActionResult, etc).
 *
 * 🔗 CHAMADA POR  : Quem invoca este arquivo (UI, outros controllers, middlewares).
 *
 * 🔄 CHAMA        : O que este arquivo invoca (repositories, services, APIs externas).
 *
 * 📦 DEPENDÊNCIAS : IUnitOfWork, ILogger, DbContext, etc.
 *
 * 📝 OBSERVAÇÕES  : Informações adicionais importantes (se aplicável).
 **************************************************************************************** */
```

#### ✅ Modelo para JavaScript

```javascript
/* ****************************************************************************************
 * ⚡ ARQUIVO: nomeDoArquivo.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Descrição clara e objetiva da responsabilidade do arquivo.
 *
 * 📥 ENTRADAS     : Eventos DOM, parâmetros de funções, dados de formulário.
 *
 * 📤 SAÍDAS       : Manipulação DOM, chamadas AJAX, retornos de funções.
 *
 * 🔗 CHAMADA POR  : Eventos onclick, document.ready, outras funções JS.
 *
 * 🔄 CHAMA        : Endpoints da API, funções auxiliares, plugins externos.
 *
 * 📦 DEPENDÊNCIAS : jQuery, Syncfusion, Alerta.js, FtxSpin, etc.
 *
 * 📝 OBSERVAÇÕES  : Informações adicionais importantes (se aplicável).
 **************************************************************************************** */
```

#### ✅ Modelo para CSHTML (Razor Pages)

```html
@*
****************************************************************************************
⚡ ARQUIVO: NomeDaPagina.cshtml
--------------------------------------------------------------------------------------
🎯 OBJETIVO     : Descrição clara do propósito da página.

📥 ENTRADAS     : Model, ViewData, TempData, parâmetros de rota.

📤 SAÍDAS       : Renderização HTML, formulários, modals.

🔗 CHAMADA POR  : Navegação do usuário, redirecionamentos de controllers.

🔄 CHAMA        : Controllers (via formulários/AJAX), scripts JS, partials.

📦 DEPENDÊNCIAS : Bootstrap, Syncfusion, jQuery, scripts customizados.

📝 OBSERVAÇÕES  : Informações adicionais importantes (se aplicável).
****************************************************************************************
*@
```

---

### 5.3 Card de Função (Documentação Detalhada)

**REGRA:** Cada função DEVE ter um header descritivo imediatamente antes da declaração.

#### ✅ Modelo para C# (Funções/Métodos)

```csharp
/****************************************************************************************
 * ⚡ FUNÇÃO: NomeDaFuncao
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Descrição detalhada do que a função faz.
 *
 * 📥 ENTRADAS     : param1 [tipo] - Descrição
 *                   param2 [tipo] - Descrição
 *
 * 📤 SAÍDAS       : Tipo de retorno e o que representa.
 *
 * 🔗 CHAMADA POR  : Quem invoca esta função.
 *
 * 🔄 CHAMA        : O que esta função invoca internamente.
 *
 * 📝 OBSERVAÇÕES  : Regras especiais, validações, side effects.
 ****************************************************************************************/
public IActionResult NomeDaFuncao(int param1, string param2)
{
    try
    {
        // [LOGICA] Descrição do bloco de código
        // código aqui
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("NomeDoArquivo.cs", "NomeDaFuncao", error);
        return Json(new { success = false, message = error.Message });
    }
}
```

#### ✅ Modelo para JavaScript (Funções)

```javascript
/****************************************************************************************
 * ⚡ FUNÇÃO: nomeDaFuncao
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Descrição detalhada do que a função faz.
 *
 * 📥 ENTRADAS     : param1 [tipo] - Descrição
 *
 * 📤 SAÍDAS       : Tipo de retorno (void, Promise, Object, etc).
 *
 * 🔗 CHAMADA POR  : Evento onclick, outra função, DOMContentLoaded.
 *
 * 🔄 CHAMA        : Endpoints da API, funções auxiliares.
 *
 * 📝 OBSERVAÇÕES  : Regras especiais, validações, dependências.
 ****************************************************************************************/
function nomeDaFuncao(param1) {
    try {
        // [AJAX] Chamada para endpoint /api/recurso
        fetch('/api/recurso', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(param1)
        })
        .then(r => r.json())
        .then(data => {
            // [UI] Atualizar interface
        });
    } catch (erro) {
        Alerta.TratamentoErroComLinha("arquivo.js", "nomeDaFuncao", erro);
    }
}
```

---

### 5.4 Comentários Internos (Tags Semânticas)

**REGRA:** Use tags descritivas para categorizar blocos de código e facilitar a leitura.

| Tag | Significado | Exemplo de Uso |
| :--- | :--- | :--- |
| `// [UI]` | Manipulação de DOM, CSS, Visibilidade | `Elemento.style.display = 'none'` |
| `// [LOGICA]` | Regras de fluxo, algoritmos, loops | `Cálculo de média ponderada` |
| `// [REGRA]` | Regras de Negócio obrigatórias | `Validar se data fim > data inicio` |
| `// [DADOS]` | Manipulação de Objetos/JSON/Models | `Mapear ViewModel para DTO` |
| `// [AJAX]` | Chamadas HTTP, Fetch, APIs | `fetch('/api/endpoint')` |
| `// [DB]` | Operações com Banco de Dados | `_unitOfWork.Repository.Add(obj)` |
| `// [PERFORMANCE]` | Otimizações, Cache, Lazy Load | `Usar cache para evitar query` |
| `// [DEBUG]` | Logs, verificação de erros | `console.log("Valores:", val)` |
| `// [HELPER]` | Funções utilitárias locais | `FormatarData(...)` |
| `// [SEGURANCA]` | Validações de segurança | `Verificar permissão do usuário` |
| `// [VALIDACAO]` | Validações de entrada | `if (string.IsNullOrEmpty(nome))` |

---

### 5.5 JavaScript em Páginas CSHTML

**REGRA CRÍTICA:** Código JavaScript dentro de páginas `.cshtml` DEVE ser documentado seguindo os mesmos padrões.

#### ✅ Exemplo de JavaScript Documentado em CSHTML

```html
@section Scripts {
<script>
    /* ****************************************************************************************
     * ⚡ SCRIPT: Gerenciamento de Veículos
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Carregar grid de veículos e gerenciar eventos de edição/exclusão.
     *
     * 📥 ENTRADAS     : Dados do endpoint /api/Veiculo/GetAll
     *
     * 📤 SAÍDAS       : Grid populado, modals de edição/exclusão
     *
     * 🔗 CHAMADA POR  : document.ready
     *
     * 🔄 CHAMA        : carregarGrid(), excluirVeiculo(id), editarVeiculo(id)
     **************************************************************************************** */

    $(document).ready(function() {
        try {
            // [AJAX] Carregar dados do grid
            carregarGrid();
        } catch (erro) {
            Alerta.TratamentoErroComLinha("Veiculos.cshtml", "document.ready", erro);
        }
    });

    /****************************************************************************************
     * ⚡ FUNÇÃO: carregarGrid
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Buscar veículos da API e popular DataTable
     *
     * 📥 ENTRADAS     : Nenhuma
     *
     * 📤 SAÍDAS       : Grid populado com dados
     *
     * 🔗 CHAMADA POR  : document.ready
     *
     * 🔄 CHAMA        : GET /api/Veiculo/GetAll
     ****************************************************************************************/
    function carregarGrid() {
        try {
            // [AJAX] Buscar dados
            fetch('/api/Veiculo/GetAll')
                .then(r => r.json())
                .then(response => {
                    // [UI] Popular grid
                    $('#gridVeiculos').DataTable({
                        data: response.data
                    });
                });
        } catch (erro) {
            Alerta.TratamentoErroComLinha("Veiculos.cshtml", "carregarGrid", erro);
        }
    }
</script>
}
```

---

### 5.6 Chamadas AJAX - Documentação Obrigatória

**REGRA:** Toda chamada AJAX/Fetch DEVE documentar:
- Endpoint chamado (método HTTP + rota)
- Parâmetros enviados
- Resposta esperada

#### ✅ Exemplo de Chamada AJAX Documentada

```javascript
/****************************************************************************************
 * [AJAX] Endpoint: POST /api/Veiculo/Create
 * --------------------------------------------------------------------------------------
 * 📥 ENVIA        : { Placa, ModeloId, Status }
 * 📤 RECEBE       : { success: bool, message: string, data: { VeiculoId } }
 * 🎯 MOTIVO       : Criar novo veículo no sistema após validação do formulário
 ****************************************************************************************/
fetch('/api/Veiculo/Create', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
        Placa: placa,
        ModeloId: modeloId,
        Status: true
    })
})
.then(r => r.json())
.then(response => {
    if (response.success) {
        Alerta.Sucesso("Sucesso", response.message);
    }
});
```

---

### 5.7 Try-Catch Obrigatório

**REGRA INVIOLÁVEL:** TODA função (C# ou JS) DEVE ter try-catch.

#### ✅ C#

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

---

### 5.8 Tooltips - Sempre Syncfusion

**REGRA:** Substituir tooltips Bootstrap por Syncfusion (exceto colunas de Ação em DataTables).

#### ❌ Bootstrap (NÃO usar)

```html
<button data-bs-toggle="tooltip" title="Editar">Editar</button>
```

#### ✅ Syncfusion (USAR)

```html
<button data-ejtip="Editar">Editar</button>
```

**Para elementos dinâmicos (DataTables):**

```javascript
drawCallback: function() {
    if (window.ejTooltip) {
        window.ejTooltip.refresh();
    }
}
```

---

### 5.9 Rastreabilidade de Funções Internas

**REGRA:** Documentar chamadas entre funções do MESMO arquivo usando símbolos direcionais.

#### ✅ Exemplo

```javascript
/****************************************************************************************
 * ⚡ FUNÇÃO: salvarDados
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Validar e enviar dados do formulário para a API
 *
 * ⬅️ CHAMADO POR  : Evento onclick do botão #btnSalvar
 *
 * ➡️ CHAMA        : validarFormulario() [linha 45]
 *                   enviarParaAPI() [linha 89]
 ****************************************************************************************/
function salvarDados() {
    if (!validarFormulario()) return;  // [HELPER] Função deste arquivo linha 45
    enviarParaAPI();                   // [AJAX] Função deste arquivo linha 89
}

/****************************************************************************************
 * ⚡ FUNÇÃO: validarFormulario
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Validar campos obrigatórios do formulário
 *
 * ⬅️ CHAMADO POR  : salvarDados() [linha 12]
 *
 * ➡️ CHAMA        : Nenhuma (função folha)
 ****************************************************************************************/
function validarFormulario() {
    // validação
}
```

**Nota sobre símbolos:**
- **⬅️ CHAMADO POR**: Indica origem da chamada (quem invoca esta função)
- **➡️ CHAMA**: Indica destino (o que esta função invoca)
- Alternativas aceitas: `🔗 CHAMADA POR` e `🔄 CHAMA` (mantidos por compatibilidade)

---

### 5.10 Processo de Documentação

**Workflow:**
1. Ler arquivo completo e entender seu propósito
2. Adicionar Card de Arquivo no topo
3. Documentar cada função com Card de Função
4. Adicionar comentários internos com tags semânticas
5. Garantir try-catch em todas as funções
6. Documentar chamadas AJAX com detalhes de entrada/saída
7. Mapear rastreabilidade (quem chama quem)
8. Atualizar `DocumentacaoIntracodigo.md`
9. Commit a cada 10 arquivos documentados

**Commits:**
- Fazer commit e push para `main` a cada 10 arquivos documentados
- Mensagem de commit: `docs: Lote [número] - [pasta] [descrição] ([quantidade] arquivos)`

**Feedback Visual:**
- Atualizar barra de progresso em `DocumentacaoIntracodigo.md`
- Mostrar percentual concluído após cada lote

---

## 📊 5.11 MAPEAMENTO DE DEPENDÊNCIAS (MapeamentoDependencias.md)

> 📁 **Arquivo de Referência:** `MapeamentoDependencias.md` - Mapa completo de todas as dependências entre arquivos do projeto

### 5.11.1 Visão Geral

O arquivo `MapeamentoDependencias.md` centraliza **TODAS** as relações de dependência entre arquivos do FrotiX, facilitando:
- Rastreamento de impacto de mudanças
- Identificação de acoplamento excessivo
- Planejamento de refatorações
- Onboarding de novos desenvolvedores

### 5.11.2 Estrutura do Arquivo

O mapeamento está dividido em **4 seções principais**:

#### 🔷 CS → CS: Backend calling Backend
Dependências entre arquivos C# (Controllers → Services → Repository → DbContext)

#### 🟦 JS → JS: Frontend calling Frontend
Dependências entre arquivos JavaScript (funções globais, helpers, plugins)

#### 🟨 JS → CS: AJAX calling Endpoints
Chamadas AJAX do frontend para endpoints da API

#### 🟩 CSHTML: Pages e Views
Arquivos Razor com suas dependências (scripts, partials, controllers)

### 5.11.3 Formato de Documentação

Cada dependência é documentada com **5 elementos obrigatórios**:

| Campo | Descrição | Exemplo |
|-------|-----------|---------|
| **Método/Função** | Nome exato do método/função chamado | `GetAllAsync()` |
| **Entrada** | Parâmetros esperados com tipos | `AbastecimentoFilter filters` |
| **Saída** | Tipo de retorno | `Task<List<AbastecimentoDTO>>` |
| **Motivo** | Razão de negócio/técnica da chamada | "Buscar abastecimentos com filtros aplicados" |
| **Linha** | Localização exata no arquivo origem | `AbastecimentoController.cs:156` |

#### ✅ Exemplo CS → CS

```markdown
### AbastecimentoController.cs
**Localização:** FrotiX.Site/Controllers/AbastecimentoController.cs
**Tipo:** API Controller (Partial Class)

#### Depende de:
1. **IUnitOfWork.Abastecimento** (Repository)
   - Método: `GetAllAsync()`
   - Entrada: `AbastecimentoFilter filters`
   - Saída: `Task<List<AbastecimentoDTO>>`
   - Motivo: Buscar abastecimentos com filtros aplicados no banco de dados
   - Linha: AbastecimentoController.cs:156

2. **ILogger<AbastecimentoController>** (Framework)
   - Método: `LogInformation()`, `LogError()`
   - Entrada: `string message, params object[] args`
   - Saída: `void`
   - Motivo: Registrar logs de operações e erros para auditoria
   - Linhas: AbastecimentoController.cs:178, 245, 389
```

#### ✅ Exemplo JS → JS

```markdown
### ViagemIndex.js
**Localização:** FrotiX.Site/wwwroot/js/cadastros/ViagemIndex.js

#### Depende de:
1. **alerta.js → Alerta.Confirmar()**
   - Entrada: `string message, function callback`
   - Saída: `Promise<boolean>`
   - Motivo: Exibir confirmação padronizada antes de cancelar viagem
   - Linha: ViagemIndex.js:425

2. **frotix.js → FtxSpin.show()**
   - Entrada: `Nenhum parâmetro`
   - Saída: `void`
   - Motivo: Exibir indicador de carregamento durante operações AJAX
   - Linha: ViagemIndex.js:512
```

#### ✅ Exemplo JS → CS

```markdown
### motorista_upsert.js
**Localização:** FrotiX.Site/wwwroot/js/cadastros/motorista_upsert.js

#### Chama endpoints:
1. **POST /Motorista/Upsert?handler=ValidaCPF**
   - Controller: MotoristaController.cs
   - Método: `OnPostValidaCPFAsync()`
   - Entrada: `{ cpf: string }`
   - Saída: `{ success: bool, message: string, motorista: { Id, Nome } }`
   - Motivo: Validar CPF em tempo real durante preenchimento do formulário
   - Linha: motorista_upsert.js:187

2. **POST /Motorista/Upsert?handler=UploadFoto**
   - Controller: MotoristaController.cs
   - Método: `OnPostUploadFotoAsync()`
   - Entrada: `FormData { file: File, motoristaId: int }`
   - Saída: `{ success: bool, message: string, fotoUrl: string }`
   - Motivo: Upload de foto do motorista para servidor e armazenamento no banco
   - Linha: motorista_upsert.js:543
```

### 5.11.4 Processo de Atualização

**REGRA:** O `MapeamentoDependencias.md` é atualizado **automaticamente** durante o processo de documentação intra-código.

**Workflow:**
1. Ao documentar um arquivo com Cards (Seção 5.2/5.3), extrair informações de dependências
2. Para cada entrada em `➡️ CHAMA` ou `📦 DEPENDÊNCIAS`, adicionar ao mapeamento
3. Para cada chamada AJAX com `📥📤🎯`, adicionar à seção JS→CS
4. Validar consistência bidirecional (se A chama B, então B é chamado por A)
5. Commit junto com a documentação intra-código

### 5.11.5 Validação de Consistência

**Regras de validação**:
- ✅ Toda dependência CS→CS deve ter entrada correspondente em ambos os arquivos
- ✅ Todo endpoint documentado em JS→CS deve existir em algum Controller
- ✅ Tipos de entrada/saída devem corresponder à assinatura real do método
- ✅ Números de linha devem ser atualizados ao modificar arquivos

---

## 🔍 5.12 ANÁLISE DE ARQUIVOS CRÍTICOS (ArquivosCriticos.md)

> 📁 **Arquivo de Referência:** `ArquivosCriticos.md` - Backlog de dívidas técnicas não resolvidas

### 5.12.1 Filosofia de Análise Completa

**REGRA FUNDAMENTAL:** Quando um arquivo é aberto durante uma sessão de chat/editor/agente, ele deve ser **analisado por completo**, não apenas em relação à questão específica levantada pelo usuário.

**Workflow Obrigatório:**

1. **Resolver a questão imediata** do usuário (bug, feature, dúvida)

2. **Analisar o arquivo completamente** buscando:
   - CSS inline excessivo (>200 linhas)
   - JavaScript inline excessivo (>200 linhas)
   - Código duplicado
   - Falta de validações
   - Performance issues (queries N+1, falta de cache, loops desnecessários)
   - Problemas de segurança (SQL injection, XSS, falta de sanitização)
   - Violações das regras FrotiX (falta de try-catch, tooltips Bootstrap, etc.)

3. **Apresentar resumo de otimizações** ao usuário com:
   - Lista completa de problemas encontrados
   - Estimativa de redução de linhas
   - Prioridade de cada problema (🔴 CRÍTICA, 🟡 ALTA, 🟠 MÉDIA, 🟢 BAIXA)

4. **PERGUNTAR ao usuário:**
   > "Encontrei [N] problemas neste arquivo. Quer que eu corrija agora ou prefere deixar documentado no ArquivosCriticos.md para análise posterior?"

5. **SE usuário aceitar corrigir AGORA:**
   - ✅ Implementar todas as correções aceitas
   - ✅ Fazer commit com mensagem descritiva
   - ❌ **NÃO adicionar** ao ArquivosCriticos.md
   - ✅ Problema resolvido, não há dívida técnica

6. **SE usuário optar por deixar PARA DEPOIS:**
   - ✅ **VERIFICAR** se arquivo já consta no ArquivosCriticos.md
   - ✅ SE NÃO existe: Adicionar entrada completa com todos os problemas
   - ✅ SE JÁ existe: Atualizar entrada existente (novos problemas ou mudança de prioridade)
   - ✅ Informar ao usuário: "Documentado em ArquivosCriticos.md para refatoração futura"

### 5.12.2 Regra de Limpeza (CRÍTICA)

**IMPORTANTE:** O ArquivosCriticos.md **NÃO É** um log histórico de todos os problemas encontrados.

**É um BACKLOG VIVO de dívidas técnicas pendentes:**
- ✅ Arquivo tem problema + usuário quer deixar para depois = **ADICIONAR**
- ✅ Arquivo tem problema + usuário aceita corrigir agora = **NÃO ADICIONAR**
- ✅ Arquivo estava no backlog + problema foi corrigido = **REMOVER do ArquivosCriticos.md**

**Exemplo de Limpeza:**
```markdown
<!-- SE o arquivo Multa/ListaAutuacao.cshtml for refatorado e os problemas corrigidos -->
<!-- REMOVER a entrada completa do ArquivosCriticos.md -->
<!-- Fazer commit: "refactor: ListaAutuacao.cshtml - corrige CSS/JS inline (closes #123)" -->
```

### 5.12.3 Verificação de Duplicação

**ANTES de adicionar um arquivo ao ArquivosCriticos.md:**

1. **Ler o arquivo completo** ArquivosCriticos.md
2. **Buscar** pelo nome do arquivo (ex: `grep -i "ListaAutuacao.cshtml"`)
3. **SE encontrado:**
   - Comparar problemas existentes vs. novos problemas encontrados
   - SE houver novos problemas: **Atualizar** a entrada (não duplicar)
   - SE problemas forem os mesmos: **Não fazer nada**
4. **SE NÃO encontrado:**
   - Adicionar entrada completa no final da seção correspondente

### 5.12.4 Critérios de Criticidade

| Nível | Descrição | Exemplos |
|-------|-----------|----------|
| 🔴 **CRÍTICA** | Impacto alto, refatoração urgente | CSS/JS inline >500 linhas, SQL injection, falta de validação em operações financeiras |
| 🟡 **ALTA** | Impacto médio, refatoração recomendada | CSS/JS inline 200-500 linhas, código duplicado em 3+ arquivos, queries N+1 |
| 🟠 **MÉDIA** | Impacto baixo, refatoração opcional | CSS/JS inline 100-200 linhas, falta de comentários, nomes de variáveis pouco descritivos |
| 🟢 **BAIXA** | Melhorias cosméticas | Formatação inconsistente, ordenação de imports, espaçamento |

### 5.12.5 Formato de Documentação

#### ✅ Template para ArquivosCriticos.md

```markdown
## 🔴 CRÍTICA: Multa/ListaAutuacao.cshtml (1307 linhas)

**Localização:** `FrotiX.Site/Pages/Multa/ListaAutuacao.cshtml`
**Data de Identificação:** 03/02/2026
**Status:** 🔴 PENDENTE (aguardando refatoração)

### Problemas Identificados

1. **CSS Inline Excessivo** (569 linhas - 44% do arquivo)
   - **Localização:** Linhas 45-614
   - **Impacto:** Dificulta manutenção, não reutilizável, aumenta tempo de carregamento
   - **Solução:** Extrair para `wwwroot/css/multa/lista-autuacao.css`

2. **JavaScript Inline Excessivo** (738+ linhas - 56% do arquivo)
   - **Localização:** Linhas 615-1307
   - **Impacto:** Duplicação com `listaautuacao.js`, não reutilizável
   - **Solução:** Consolidar com `wwwroot/js/cadastros/listaautuacao.js`

3. **Bootstrap CDN Redundante**
   - **Localização:** Linha 12
   - **Impacto:** Já carregado no _Layout.cshtml
   - **Solução:** Remover `<link>` duplicado

### Métricas

| Métrica | Atual | Após Refatoração | Redução |
|---------|-------|------------------|---------|
| Linhas totais | 1307 | ~500 | -62% |
| CSS inline | 569 | 0 | -100% |
| JS inline | 738 | ~50 (event handlers) | -93% |

### Prioridade

**Urgência:** 🔴 ALTA - Arquivo é mantido com frequência, mudanças geram conflitos de merge

### Plano de Refatoração

1. ✅ Extrair CSS para arquivo separado
2. ✅ Consolidar JavaScript com arquivo existente
3. ✅ Remover dependências duplicadas
4. ✅ Adicionar documentação intra-código completa
5. ✅ Atualizar MapeamentoDependencias.md
```

### 5.12.6 Quando Adicionar ao ArquivosCriticos.md

**Adicionar APENAS quando o usuário optar por deixar para depois:**

**Fluxo de Decisão:**
```
Arquivo aberto → Problema encontrado → Apresentar ao usuário
   ↓
   ├─ Usuário aceita corrigir AGORA
   │  └─ Corrigir → Commit → ❌ NÃO adicionar ao ArquivosCriticos.md
   │
   └─ Usuário quer deixar PARA DEPOIS
      └─ Verificar duplicação → Adicionar/Atualizar ArquivosCriticos.md
```

**Critérios técnicos para considerar um problema:**
- ✅ CSS inline > 200 linhas
- ✅ JavaScript inline > 200 linhas
- ✅ Código duplicado em 2+ arquivos
- ✅ Violações de segurança (SQL injection, XSS, CSRF)
- ✅ Falta de validação em operações críticas
- ✅ Performance issues evidentes (N+1, sem paginação, sem cache)

**NÃO adicionar quando:**
- ❌ Usuário corrigiu imediatamente
- ❌ Problemas cosméticos menores (formatação, nomes)
- ❌ Arquivos já em processo de refatoração
- ❌ Issues triviais que podem ser corrigidos em <5 minutos

### 5.12.7 Remoção do ArquivosCriticos.md

**REGRA:** Quando um arquivo for refatorado e os problemas corrigidos, **REMOVER** a entrada do ArquivosCriticos.md.

**Workflow de Remoção:**
1. ✅ Corrigir todos os problemas listados no ArquivosCriticos.md para aquele arquivo
2. ✅ Testar correções
3. ✅ Fazer commit: `refactor: [NomeArquivo] - resolve issues críticos`
4. ✅ **REMOVER** entrada completa do ArquivosCriticos.md
5. ✅ Atualizar estatísticas no topo do ArquivosCriticos.md
6. ✅ Fazer commit: `docs: ArquivosCriticos.md - remove [NomeArquivo] (resolvido)`

**Exemplo de Mensagem de Commit:**
```bash
git commit -m "refactor: ListaAutuacao.cshtml - extrai CSS/JS inline, remove CDN duplicado

- Extrai 569 linhas CSS para lista-autuacao.css
- Consolida 738 linhas JS em listaautuacao.js
- Remove Bootstrap CDN redundante
- Reduz arquivo de 1307 → 498 linhas (62%)

Closes #45 (ArquivosCriticos.md)"
```

### 5.12.8 Seção de Problemas no Final do Arquivo (OBRIGATÓRIA)

**REGRA NOVA:** Além da documentação centralizada em `ArquivosCriticos.md`, cada arquivo com problemas identificados DEVE ter uma **seção comentada no final** listando seus problemas, impactos e soluções propostas.

**Vantagens:**
- ✅ **Autocontido:** Desenvolvedor vê problemas ao abrir o arquivo
- ✅ **Versionado:** Histórico de problemas fica no Git junto com o código
- ✅ **Rastreável:** Fácil ver quando problema foi identificado e por quem
- ✅ **Visível:** Não depende de consultar arquivo externo
- ✅ **Dupla documentação:** ArquivosCriticos.md = índice, Seção no arquivo = detalhes

#### Formato para C# (.cs)

```csharp
/* ****************************************************************************************
 * 🚨 PROBLEMAS IDENTIFICADOS E MELHORIAS PROPOSTAS
 * --------------------------------------------------------------------------------------
 * Data de Identificação: 03/02/2026
 * Identificado por: Claude Sonnet 4.5 (Análise Automática)
 * Status: 🔴 PENDENTE (aguardando refatoração)
 *
 * ========================================================================================
 * PROBLEMA #1: [TÍTULO DO PROBLEMA]
 * ----------------------------------------------------------------------------------------
 * Severidade: 🔴 CRÍTICA / 🟡 ALTA / 🟠 MÉDIA / 🟢 BAIXA
 *
 * Descrição:
 * [Descrição detalhada do problema]
 *
 * Localização:
 * - Linhas: [números das linhas]
 * - Métodos afetados: [lista de métodos]
 *
 * Impacto:
 * - [Impacto 1]
 * - [Impacto 2]
 *
 * Solução Proposta:
 * [Descrição da solução recomendada]
 *
 * Código de Exemplo (Solução):
 * ```csharp
 * // Código sugerido aqui
 * ```
 *
 * Estimativa de Esforço: [tempo estimado]
 *
 * ========================================================================================
 * PROBLEMA #2: [OUTRO PROBLEMA]
 * [... mesma estrutura ...]
 *
 * ========================================================================================
 * OBSERVAÇÕES GERAIS:
 * [Observações adicionais sobre o arquivo, contexto histórico, etc.]
 *
 * REFERÊNCIAS:
 * - ArquivosCriticos.md (entrada completa)
 * - Issue #123 (se aplicável)
 * - Documentação relacionada
 **************************************************************************************** */
```

#### Formato para JavaScript (.js)

```javascript
/* ****************************************************************************************
 * 🚨 PROBLEMAS IDENTIFICADOS E MELHORIAS PROPOSTAS
 * --------------------------------------------------------------------------------------
 * Data de Identificação: 03/02/2026
 * Identificado por: Claude Sonnet 4.5 (Análise Automática)
 * Status: 🔴 PENDENTE (aguardando refatoração)
 *
 * ========================================================================================
 * PROBLEMA #1: Arquivo Muito Grande (1099 linhas)
 * ----------------------------------------------------------------------------------------
 * Severidade: 🔴 CRÍTICA
 *
 * Descrição:
 * Arquivo contém 1099 linhas com múltiplas responsabilidades, dificultando manutenção,
 * debugging e testabilidade. Combina lógica de UI, validações, chamadas AJAX e
 * configuração de 20+ componentes Syncfusion.
 *
 * Localização:
 * - Todo o arquivo (linhas 1-1099)
 * - Funções principais: inicializarModal(), validarFormulario(), salvarDados()
 *
 * Impacto:
 * - Manutenibilidade CRÍTICA (difícil entender fluxo completo)
 * - Testabilidade IMPOSSÍVEL (funções muito acopladas)
 * - Performance (arquivo grande carregado integralmente)
 * - Conflitos frequentes de merge no Git
 *
 * Solução Proposta:
 * Dividir em 4 módulos especializados:
 * 1. modal-agenda-main.js (150 linhas) - Setup e coordenação
 * 2. modal-agenda-controller.js (250 linhas) - Lógica de controle
 * 3. modal-agenda-validacoes.js (200 linhas) - Validações de negócio
 * 4. modal-agenda-syncfusion.js (250 linhas) - Componentes Syncfusion
 * 5. modal-agenda-api.js (180 linhas) - Chamadas AJAX consolidadas
 *
 * Código de Exemplo (Estrutura Nova):
 * ```javascript
 * // modal-agenda-main.js
 * import { ModalAgendaController } from './modal-agenda-controller.js';
 * import { ValidadorAgenda } from './modal-agenda-validacoes.js';
 *
 * const controller = new ModalAgendaController();
 * const validador = new ValidadorAgenda();
 *
 * export function abrirModal(agendaId) {
 *     controller.abrir(agendaId);
 * }
 * ```
 *
 * Estimativa de Esforço: 3-4 dias
 *
 * ========================================================================================
 * PROBLEMA #2: N+1 Queries AJAX
 * ----------------------------------------------------------------------------------------
 * Severidade: 🟡 ALTA
 *
 * Descrição:
 * Ao abrir o modal, são feitas 4 requisições AJAX sequenciais para carregar dados:
 * - GET /api/Motorista/GetAll
 * - GET /api/Veiculo/GetAll
 * - GET /api/Unidade/GetAll
 * - POST /api/ViagemAgenda/ValidarDistancia
 * Total: ~530ms + 4× overhead HTTP
 *
 * Localização:
 * - Linhas: 245, 267, 289, 312
 * - Função: abrirModal()
 *
 * Impacto:
 * - Performance (530ms+ para abrir modal)
 * - UX (usuário espera muito tempo)
 * - Servidor (4× carga desnecessária)
 *
 * Solução Proposta:
 * Criar endpoint consolidado único:
 * POST /api/ViagemAgenda/PrepareModal
 * Retorna: { motoristas, veiculos, unidades, validacoes }
 * Tempo estimado: ~120ms (1× overhead)
 *
 * Código de Exemplo (Solução):
 * ```javascript
 * // ANTES: 4 chamadas
 * const motoristas = await fetch('/api/Motorista/GetAll');
 * const veiculos = await fetch('/api/Veiculo/GetAll');
 * // ...
 *
 * // DEPOIS: 1 chamada
 * const dados = await fetch('/api/ViagemAgenda/PrepareModal', {
 *     method: 'POST',
 *     body: JSON.stringify({ agendaId: id })
 * });
 * const { motoristas, veiculos, unidades, validacoes } = dados;
 * ```
 *
 * Estimativa de Esforço: 1 dia (backend + frontend)
 *
 * ========================================================================================
 * OBSERVAÇÕES GERAIS:
 *
 * Este arquivo cresceu organicamente desde a versão 1.0 do FrotiX (2023) e acumulou
 * múltiplas features sem refatoração adequada. É um dos arquivos mais mantidos do
 * sistema (média de 2-3 alterações por semana), tornando a refatoração crítica para
 * evitar regressões e conflitos de merge.
 *
 * Histórico de problemas conhecidos:
 * - Bug de timezone no DatePicker Syncfusion (resolvido em v2.1)
 * - Conflito de validação recorrência (resolvido em v2.3)
 * - Memory leak com FullCalendar (pendente desde v2.4)
 *
 * REFERÊNCIAS:
 * - ArquivosCriticos.md (entrada completa com 6 problemas)
 * - Issue #234: Refatorar modal_agenda.js
 * - Documentação: Documentacao/JavaScript/modal_agenda.md
 **************************************************************************************** */
```

#### Formato para CSHTML (Razor Pages)

```cshtml
@*
****************************************************************************************
🚨 PROBLEMAS IDENTIFICADOS E MELHORIAS PROPOSTAS
--------------------------------------------------------------------------------------
Data de Identificação: 03/02/2026
Identificado por: Claude Sonnet 4.5 (Análise Automática)
Status: 🔴 PENDENTE (aguardando refatoração)

========================================================================================
PROBLEMA #1: CSS Inline Excessivo (569 linhas)
----------------------------------------------------------------------------------------
Severidade: 🔴 CRÍTICA

Descrição:
Arquivo contém 569 linhas de CSS inline na seção HeadBlock, representando 44% do
arquivo total. CSS não é reutilizável, não pode ser cacheado pelo browser, e
dificulta manutenção de estilos globais.

Localização:
- Linhas: 45-614 (seção @section HeadBlock)
- Estilos afetados: badges de status, animações, modals, tooltips, cards

Impacto:
- Performance: CSS não cacheado, carregado em cada request
- Manutenibilidade: Mudanças de estilo requerem editar CSHTML
- Consistência: Estilos duplicados em outros arquivos
- Bundle size: Aumenta tamanho da página desnecessariamente

Solução Proposta:
Extrair TODO o CSS para arquivo separado:
/wwwroot/css/multa/lista-autuacao.css

Manter apenas CSS crítico inline (se houver).

Código de Exemplo (Solução):
```cshtml
@* ANTES: 569 linhas CSS inline *@
@section HeadBlock {
    <style>
        /* 569 linhas de CSS... */
    </style>
}

@* DEPOIS: Referência externa *@
@section HeadBlock {
    <link rel="stylesheet" href="~/css/multa/lista-autuacao.css" asp-append-version="true" />
}
```

Estimativa de Esforço: 2 horas (extração + testes)

========================================================================================
PROBLEMA #2: JavaScript Inline Excessivo (738+ linhas)
----------------------------------------------------------------------------------------
Severidade: 🔴 CRÍTICA

Descrição:
Arquivo contém 738+ linhas de JavaScript inline, incluindo código DUPLICADO do
arquivo externo listaautuacao.js. Não pode ser minificado, testado ou reutilizado.

Localização:
- Linhas: 615-1307 (seção @section Scripts)
- Funções duplicadas: carregarTabela(), moeda(), toolbarClick()

Impacto:
- Duplicação: Mesmo código em 2 lugares (inline + listaautuacao.js)
- Manutenção: Mudanças devem ser sincronizadas manualmente
- Performance: JavaScript não minificado nem cacheado
- Debugging: Difícil debugar código inline sem source maps

Solução Proposta:
Consolidar TUDO no arquivo listaautuacao.js existente.
Manter apenas event wiring mínimo inline (< 10 linhas).

Código de Exemplo (Solução):
```cshtml
@* ANTES: 738+ linhas inline *@
@section Scripts {
<script>
    function carregarTabela() { /* 200 linhas */ }
    function moeda(valor) { /* 50 linhas */ }
    // ... 488 linhas mais
</script>
}

@* DEPOIS: Referência externa apenas *@
@section Scripts {
    <script src="~/js/cadastros/listaautuacao.js" asp-append-version="true"></script>
    <script>
        // Apenas event wiring específico desta página (se necessário)
        $(document).ready(function() {
            ListaAutuacao.inicializar(); // Função do arquivo externo
        });
    </script>
}
```

Estimativa de Esforço: 4 horas (consolidação + testes regressão)

========================================================================================
OBSERVAÇÕES GERAIS:

Página de Multas/Lista de Autuação é uma das mais complexas do sistema devido à
integração com múltiplos componentes (DataTables, Syncfusion RTE, Syncfusion
PDFViewer, Bootstrap Modals). Foi desenvolvida inicialmente como protótipo inline
e nunca passou por refatoração adequada.

PRIORIDADE: 🔴 URGENTE - Página é mantida semanalmente (média 1-2 alterações/semana),
causando conflitos frequentes de merge e dificuldade para novos desenvolvedores.

REFERÊNCIAS:
- ArquivosCriticos.md (entrada completa)
- Issue #156: Refatorar ListaAutuacao.cshtml
- Documentação: Documentacao/Pages/Multa/ListaAutuacao.md
****************************************************************************************
*@
```

---

#### 5.12.8.1 Workflow de Criação da Seção

**Quando adicionar a seção de problemas:**

1. **Durante análise completa** (Seção 5.12.1):
   - Ao apresentar problemas ao usuário
   - SE usuário optar por deixar para depois
   - Adicionar seção no final do arquivo

2. **Durante segunda passada** (Seção 5.13):
   - Se novos problemas forem identificados
   - Atualizar seção existente (não duplicar)

3. **Após identificação de agentes**:
   - Incorporar análises de agentes Haiku
   - Consolidar múltiplos problemas na mesma seção

**Processo:**
```
1. Identificar problemas no arquivo
2. Apresentar ao usuário
3. SE usuário optar por deixar para depois:
   ├─ Adicionar entrada ao ArquivosCriticos.md (índice central)
   └─ Adicionar seção comentada no final do arquivo (detalhes locais)
4. Fazer commit: "docs: Documenta problemas em [NomeArquivo]"
```

---

#### 5.12.8.2 Workflow de Atualização da Seção

**Quando atualizar:**
- Novos problemas identificados no mesmo arquivo
- Mudança de severidade (problema se agravou)
- Problema parcialmente resolvido (marcar como "EM PROGRESSO")
- Adicionar referências (issues, documentação)

**Não criar seções duplicadas!** Sempre atualizar a seção existente.

---

#### 5.12.8.3 Workflow de Remoção da Seção

**REGRA:** Quando TODOS os problemas listados forem resolvidos:

1. ✅ Corrigir todos os problemas
2. ✅ Testar correções
3. ✅ **REMOVER seção completa** do final do arquivo
4. ✅ **REMOVER entrada** do ArquivosCriticos.md
5. ✅ Fazer commit: `refactor: [NomeArquivo] - resolve todos os problemas críticos`

**Marcação de Progresso Parcial:**

Se apenas ALGUNS problemas foram resolvidos, **atualizar** a seção:

```javascript
/* ****************************************************************************************
 * ========================================================================================
 * PROBLEMA #1: Arquivo Muito Grande (1099 linhas)
 * ----------------------------------------------------------------------------------------
 * Severidade: 🔴 CRÍTICA
 * Status: ✅ RESOLVIDO (03/02/2026)
 *
 * Resolução aplicada:
 * Arquivo dividido em 4 módulos conforme proposto. Ver commit abc123.
 *
 * ========================================================================================
 * PROBLEMA #2: N+1 Queries AJAX
 * ----------------------------------------------------------------------------------------
 * Severidade: 🟡 ALTA
 * Status: 🔴 PENDENTE (aguardando desenvolvimento backend)
 *
 * [... descrição original mantida ...]
 **************************************************************************************** */
```

---

### 5.12.9 Nota Importante: Estratégias Intencionais

**Mix Kendo/Syncfusion:**

Não é inconsistência, mas **estratégia pontual** de substituição gradual de componentes Syncfusion problemáticos por equivalentes Kendo.

**Motivo:** Evitar regressões em sistemas estáveis, substituindo apenas onde há problemas técnicos comprovados.

**Exemplos válidos:**
- ✅ Substituir Syncfusion DatePicker por Kendo em página com bugs de timezone
- ✅ Manter Syncfusion Grid se está funcionando perfeitamente
- ❌ NÃO substituir "para padronizar" sem motivo técnico

**Regra:** Mix Kendo/Syncfusion **NÃO É problema crítico** quando for substituição pontual justificada. **NÃO adicionar** ao ArquivosCriticos.md apenas por mix de bibliotecas.

---

✅ **FIM DO DOCUMENTO**

📌 **Lembrete:** Este arquivo deve ser consultado no início de cada sessão de desenvolvimento ou interação com agentes de IA.

---

## 📝 5.13 GUIA DE ENRIQUECIMENTO - SEGUNDA PASSADA

> 📁 **Propósito:** Garantir que **TODOS** os arquivos tenham documentação completa com cards em todas as funções e comentários inline relevantes.

### 5.13.1 Objetivo da Segunda Passada

A segunda passada é um processo sistemático de **enriquecimento de documentação intra-código** que visa:

1. ✅ **Completude:** Garantir que TODAS as funções tenham cards ⚡ completos
2. ✅ **Rastreabilidade:** Documentar todas as dependências (⬅️ CHAMADO POR, ➡️ CHAMA)
3. ✅ **Clareza:** Adicionar comentários inline em lógica complexa
4. ✅ **Manutenibilidade:** Facilitar compreensão do código sem executá-lo
5. ✅ **Consolidação:** Incorporar informações levantadas pelos agentes de análise

**Quando realizar:** Após a primeira documentação de todos os arquivos, ou quando agentes de análise identificarem gaps na documentação existente.

---

### 5.13.2 Checklist por Tipo de Arquivo

#### ✅ C# (.cs, .cshtml.cs)

**Obrigatório:**
- [ ] **Card de Arquivo** no topo com ⚡ 🎯 📥 📤 🔗 🔄 📦 📝
- [ ] **Toda função pública** tem card ⚡ FUNÇÃO
- [ ] **Toda função privada complexa** (>20 linhas ou lógica não-trivial) tem card ⚡ FUNÇÃO
- [ ] Card tem **🎯 OBJETIVO, 📥 ENTRADAS, 📤 SAÍDAS**
- [ ] Card tem **⬅️ CHAMADO POR, ➡️ CHAMA** (rastreabilidade)
- [ ] **Dependências identificadas** pelos agentes estão documentadas em 📦 DEPENDÊNCIAS
- [ ] **Comentários inline** em:
  - LINQ complexo (3+ operações encadeadas)
  - Loops aninhados
  - Validações de negócio não-óbvias
  - Cálculos matemáticos
  - Operações de banco (queries customizadas, stored procedures)

**Exemplo de função documentada:**
```csharp
/****************************************************************************************
 * ⚡ FUNÇÃO: CalcularCustoTotal
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Calcular custo total de viagem incluindo combustível, motorista,
 *                   veículo e custos operacionais
 *
 * 📥 ENTRADAS     : viagemId [int] - ID da viagem
 *                   incluirExtras [bool] - Se deve incluir custos extras
 *
 * 📤 SAÍDAS       : decimal - Valor total calculado em R$
 *
 * ⬅️ CHAMADO POR  : ViagemController.OnPostFinalizarAsync() [linha 245]
 *                   RelatorioService.GerarRelatorioMensal() [linha 89]
 *
 * ➡️ CHAMA        : _unitOfWork.Viagem.GetFirstOrDefaultAsync() [linha 12]
 *                   ServicosAsync.CalculaCustoCombustivelAsync() [linha 18]
 *                   ServicosAsync.CalculaCustoMotoristaAsync() [linha 23]
 *
 * 📝 OBSERVAÇÕES  : Usa preços de repactuação se disponíveis, caso contrário usa
 *                   valores do contrato vigente. Custos extras incluem pedágios,
 *                   estacionamento e alimentação.
 ****************************************************************************************/
public async Task<decimal> CalcularCustoTotal(int viagemId, bool incluirExtras = false)
{
    try
    {
        // [DB] Buscar viagem com includes otimizados
        var viagem = await _unitOfWork.Viagem.GetFirstOrDefaultAsync(
            filter: v => v.Id == viagemId,
            includeProperties: "Veiculo,Motorista,Contrato,Repactuacao"
        );

        if (viagem == null)
            throw new ArgumentException($"Viagem {viagemId} não encontrada");

        // [LOGICA] Calcular custos base (combustível + motorista + veículo)
        decimal custoCombustivel = await ServicosAsync.CalculaCustoCombustivelAsync(viagem);
        decimal custoMotorista = await ServicosAsync.CalculaCustoMotoristaAsync(viagem);
        decimal custoVeiculo = await ServicosAsync.CalculaCustoVeiculoAsync(viagem);

        decimal custoTotal = custoCombustivel + custoMotorista + custoVeiculo;

        // [REGRA] Incluir custos extras apenas se solicitado e disponíveis
        if (incluirExtras && viagem.CustosExtras != null)
        {
            custoTotal += viagem.CustosExtras.Sum(e => e.Valor);
        }

        return Math.Round(custoTotal, 2);
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("ViagemService.cs", "CalcularCustoTotal", error);
        throw;
    }
}
```

---

#### ✅ JavaScript (.js)

**Obrigatório:**
- [ ] **Card de Arquivo** no topo
- [ ] **Toda função** tem card ⚡ FUNÇÃO
- [ ] **Todo AJAX** tem comentário `[AJAX]` com 📥 ENVIA, 📤 RECEBE, 🎯 MOTIVO
- [ ] **Funções globais** documentadas (ex: `window.abrirModal = function() {...}`)
- [ ] **Event handlers** documentados (onclick, change, submit, etc.)
- [ ] **Comentários inline** em:
  - Callbacks complexos
  - Promises/async-await chains
  - Manipulação DOM não-trivial
  - Validações customizadas
  - Transformações de dados (map, filter, reduce)

**Exemplo de função documentada com AJAX:**
```javascript
/****************************************************************************************
 * ⚡ FUNÇÃO: carregarDadosVeiculo
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Buscar dados completos do veículo e popular formulário de edição
 *
 * 📥 ENTRADAS     : veiculoId [number] - ID do veículo a ser carregado
 *
 * 📤 SAÍDAS       : Promise<void> - Resolve após popular formulário
 *
 * ⬅️ CHAMADO POR  : Evento onclick do botão .btn-editar [linha 45]
 *                   inicializarPagina() [linha 12]
 *
 * ➡️ CHAMA        : GET /api/Veiculo/GetById [AJAX]
 *                   popularFormulario() [linha 156]
 *                   FtxSpin.show(), FtxSpin.hide() [frotix.js]
 *
 * 📝 OBSERVAÇÕES  : Requer permissão "Veiculos.Visualizar" no backend
 ****************************************************************************************/
async function carregarDadosVeiculo(veiculoId) {
    try {
        // [UI] Exibir loading
        FtxSpin.show("Carregando dados do veículo...");

        /********************************************************************************
         * [AJAX] Endpoint: GET /api/Veiculo/GetById
         * ------------------------------------------------------------------------------
         * 📥 ENVIA        : veiculoId (query param)
         * 📤 RECEBE       : {
         *                     success: bool,
         *                     data: {
         *                       Id, Placa, Modelo, Marca, Ano, Km, Status,
         *                       ContratoId, Contrato: { ... }
         *                     },
         *                     message: string
         *                   }
         * 🎯 MOTIVO       : Buscar dados completos incluindo relacionamentos para
         *                   popular formulário de edição com todas as informações
         ********************************************************************************/
        const response = await fetch(`/api/Veiculo/GetById?id=${veiculoId}`);
        const result = await response.json();

        // [VALIDACAO] Verificar sucesso da requisição
        if (!result.success) {
            Alerta.Erro("Erro", result.message);
            return;
        }

        // [DADOS] Popular formulário com dados recebidos
        popularFormulario(result.data);

        // [UI] Exibir mensagem de sucesso
        Alerta.Sucesso("Sucesso", "Dados carregados com sucesso");

    } catch (erro) {
        // [DEBUG] Log detalhado do erro
        console.error("Erro ao carregar veículo:", erro);
        Alerta.TratamentoErroComLinha("veiculo-upsert.js", "carregarDadosVeiculo", erro);
    } finally {
        // [UI] Esconder loading (sempre executado)
        FtxSpin.hide();
    }
}
```

---

#### ✅ CSHTML (Razor Pages)

**Obrigatório:**
- [ ] **Card de Arquivo** no topo
- [ ] **JavaScript inline > 50 linhas** tem cards (preferencialmente extrair para arquivo .js separado)
- [ ] **Formulários** documentam POST/GET handler
- [ ] **Seção @section Scripts** documentada
- [ ] **Partials** documentados (`@await Html.PartialAsync(...)`)

**Exemplo de documentação em CSHTML:**
```cshtml
@*
****************************************************************************************
⚡ ARQUIVO: VeiculoUpsert.cshtml
--------------------------------------------------------------------------------------
🎯 OBJETIVO     : Página de criação e edição de veículos com validação em tempo real

📥 ENTRADAS     : Model: VeiculoViewModel (nullable para criar, populado para editar)
                  TempData["VeiculoId"] (quando vem de outra página)

📤 SAÍDAS       : POST /Veiculo/Upsert?handler=Salvar (formulário)
                  Redirect /Veiculo/Index (após sucesso)

🔗 CHAMADA POR  : Menu lateral → Cadastros → Veículos → Adicionar/Editar
                  VeiculoController.OnGetEdit(id)

🔄 CHAMA        : VeiculoController.OnPostSalvarAsync() (submit formulário)
                  /api/Veiculo/ValidarPlaca (AJAX validação)
                  veiculo-upsert.js (lógica de formulário)

📦 DEPENDÊNCIAS : Bootstrap 5.3, Syncfusion EJ2, jQuery 3.7
                  veiculo-upsert.js, alerta.js, frotix.js

📝 OBSERVAÇÕES  : Formulário usa validação dupla (cliente + servidor).
                  Placa é validada em tempo real via AJAX ao perder foco.
****************************************************************************************
*@

@page
@model VeiculoViewModel

<div class="container-fluid">
    <!-- [UI] Header da página com breadcrumb -->
    <div class="ftx-card-header">
        <h2 class="titulo-paginas mb-0">
            <i class="fa-duotone fa-car" style="--fa-primary-color:#ff6b35;"></i>
            @(Model.Id > 0 ? "Editar Veículo" : "Novo Veículo")
        </h2>
        <a href="/Veiculo" class="btn btn-header-orange">
            <i class="fa-duotone fa-rotate-left"></i> Voltar
        </a>
    </div>

    <!-- [UI] Formulário principal com validação Syncfusion -->
    <form method="post" asp-page-handler="Salvar" id="formVeiculo">
        @* [DADOS] Campo oculto para ID (apenas em edição) *@
        <input type="hidden" asp-for="Id" />

        <!-- ... campos do formulário ... -->
    </form>
</div>

@section Scripts {
<script>
    /************************************************************************************
     * [UI] Script: Validação e submissão do formulário de veículo
     * ----------------------------------------------------------------------------------
     * Inicializa componentes Syncfusion, configura validação de placa em tempo real,
     * e gerencia o fluxo de submissão com feedback visual ao usuário.
     ************************************************************************************/

    $(document).ready(function() {
        try {
            // [HELPER] Inicializar componentes Syncfusion
            inicializarComponentes();

            // [VALIDACAO] Configurar validação de placa
            configurarValidacaoPlaca();
        } catch (erro) {
            Alerta.TratamentoErroComLinha("VeiculoUpsert.cshtml", "document.ready", erro);
        }
    });
</script>
}
```

---

### 5.13.3 Quando Adicionar Comentários Inline

**✅ ADICIONAR comentários inline quando:**

1. **LINQ Complexo** (3+ operações encadeadas)
   ```csharp
   // [LOGICA] Filtrar veículos ativos, agrupar por contrato, ordenar por custo total
   var resultado = veiculos
       .Where(v => v.Status && v.ContratoId != null)
       .GroupBy(v => v.ContratoId)
       .Select(g => new { ContratoId = g.Key, Total = g.Sum(v => v.CustoMensal) })
       .OrderByDescending(x => x.Total)
       .ToList();
   ```

2. **Validações de Negócio Não-Óbvias**
   ```csharp
   // [REGRA] Data de fim deve ser no mínimo 5 dias úteis após data de início
   // (considerando feriados e fins de semana)
   if (CalcularDiasUteis(viagem.DataInicio, viagem.DataFim) < 5)
       throw new BusinessException("Viagem deve ter no mínimo 5 dias úteis");
   ```

3. **Cálculos Matemáticos ou Fórmulas**
   ```javascript
   // [LOGICA] Fórmula: Consumo = (Litros / KmPercorrido) * 100 (L/100km)
   const consumo = (abastecimento.Litros / viagem.KmPercorrido) * 100;
   ```

4. **Callbacks ou Promises Complexos**
   ```javascript
   // [AJAX] Chain de promises: Primeiro salva viagem, depois vincula motorista,
   // e finalmente atualiza status do veículo
   salvarViagem(dados)
       .then(viagemId => vincularMotorista(viagemId, motoristaId))
       .then(() => atualizarStatusVeiculo(veiculoId, "EM_VIAGEM"))
       .then(() => Alerta.Sucesso("Sucesso", "Viagem criada com sucesso"));
   ```

5. **Manipulação DOM Não-Trivial**
   ```javascript
   // [UI] Clonar linha da tabela, limpar campos, e inserir após última linha
   // mantendo event handlers com event delegation
   const novaLinha = $linhaTemplate.clone()
       .find('input').val('').end()
       .insertAfter($tabelaItens.find('tr:last'));
   ```

6. **Workarounds ou Soluções Temporárias**
   ```csharp
   // [PERFORMANCE] TODO: Otimizar com cache - query executada múltiplas vezes
   // Issue #234: Implementar cache de motoristas disponíveis
   var motoristas = await _unitOfWork.Motorista.GetAllAsync();
   ```

---

### 5.13.4 Quando NÃO Adicionar Comentários Inline

**❌ NÃO adicionar comentários óbvios ou redundantes:**

1. **Código auto-explicativo**
   ```csharp
   // ❌ MAU: Comentário óbvio
   // Incrementar contador
   contador++;

   // ✅ BOM: Sem comentário (código é claro)
   contador++;
   ```

2. **Nomes descritivos**
   ```javascript
   // ❌ MAU: Comentário desnecessário
   // Calcular total de itens
   const totalItens = calcularTotalItens();

   // ✅ BOM: Nome da função já explica
   const totalItens = calcularTotalItens();
   ```

3. **Operações simples**
   ```csharp
   // ❌ MAU: Comentário desnecessário
   // Verificar se veículo é nulo
   if (veiculo == null)
       return NotFound();

   // ✅ BOM: Guard clause clara sem comentário
   if (veiculo == null)
       return NotFound();
   ```

4. **Getters/Setters simples**
   ```csharp
   // ❌ MAU: Comentário desnecessário
   // Retornar placa
   public string Placa { get; set; }

   // ✅ BOM: Property auto-documentada
   public string Placa { get; set; }
   ```

---

### 5.13.5 Workflow para Agentes de Segunda Passada

**Processo sistemático para agentes Haiku:**

1. **Ler arquivo completo**
   - Entender propósito e contexto
   - Identificar funções sem documentação
   - Mapear dependências

2. **Identificar gaps de documentação**
   - Funções sem card ⚡
   - AJAX sem 📥📤🎯
   - Lógica complexa sem comentários
   - Falta de rastreabilidade (⬅️ ➡️)

3. **Consultar informações dos agentes de análise**
   - Dependências identificadas no `MapeamentoDependencias.md`
   - Problemas conhecidos no `ArquivosCriticos.md`
   - Padrões documentados em análises anteriores

4. **Adicionar documentação faltante**
   - Cards completos em funções
   - Comentários inline em lógica complexa
   - Rastreabilidade de chamadas
   - Informações de dependências

5. **Validar sintaxe e formatação**
   - Não quebrar código existente
   - Manter indentação consistente
   - Seguir padrões do projeto

6. **Usar Edit tool para atualizar**
   - Edições precisas (não reescrever arquivo inteiro)
   - Preservar código funcional
   - Manter formatação original

7. **Gerar relatório de enriquecimento**
   - Quantidade de funções documentadas
   - Quantidade de comentários adicionados
   - Problemas encontrados (se houver)

---

### 5.13.6 Exemplo de Antes/Depois

#### ANTES (Documentação Incompleta):
```javascript
// arquivo: veiculo-list.js

function carregarVeiculos() {
    $.ajax({
        url: '/api/Veiculo/GetAll',
        method: 'GET',
        success: function(response) {
            $('#gridVeiculos').DataTable({
                data: response.data
            });
        }
    });
}

function excluir(id) {
    if (confirm("Tem certeza?")) {
        $.post('/api/Veiculo/Delete', { id: id }, function() {
            location.reload();
        });
    }
}
```

#### DEPOIS (Documentação Completa):
```javascript
/* ****************************************************************************************
 * ⚡ ARQUIVO: veiculo-list.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar listagem e operações CRUD de veículos
 *
 * 📥 ENTRADAS     : Nenhuma (carregamento via AJAX)
 *
 * 📤 SAÍDAS       : Grid DataTable populado, confirmações de operações
 *
 * 🔗 CHAMADA POR  : VeiculoIndex.cshtml (document.ready)
 *
 * 🔄 CHAMA        : GET /api/Veiculo/GetAll
 *                   POST /api/Veiculo/Delete
 *                   Alerta.Confirmar() [alerta.js]
 *
 * 📦 DEPENDÊNCIAS : jQuery 3.7, DataTables 1.13, Alerta.js
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ FUNÇÃO: carregarVeiculos
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Buscar todos os veículos e popular grid DataTable
 *
 * 📥 ENTRADAS     : Nenhuma
 *
 * 📤 SAÍDAS       : Promise<void> - Grid populado ou mensagem de erro
 *
 * ⬅️ CHAMADO POR  : document.ready [linha 5]
 *                   excluir() - após exclusão bem-sucedida [linha 89]
 *
 * ➡️ CHAMA        : GET /api/Veiculo/GetAll [AJAX]
 *                   DataTable() [jQuery plugin]
 ****************************************************************************************/
function carregarVeiculos() {
    try {
        /********************************************************************************
         * [AJAX] Endpoint: GET /api/Veiculo/GetAll
         * ------------------------------------------------------------------------------
         * 📥 ENVIA        : Nenhum parâmetro
         * 📤 RECEBE       : {
         *                     success: bool,
         *                     data: Veiculo[],
         *                     message: string
         *                   }
         * 🎯 MOTIVO       : Carregar lista completa de veículos para exibir no grid
         *                   com paginação e filtros do lado cliente
         ********************************************************************************/
        $.ajax({
            url: '/api/Veiculo/GetAll',
            method: 'GET',
            success: function(response) {
                // [VALIDACAO] Verificar resposta da API
                if (!response.success) {
                    Alerta.Erro("Erro", response.message);
                    return;
                }

                // [UI] Inicializar DataTable com dados recebidos
                $('#gridVeiculos').DataTable({
                    data: response.data,
                    columns: [
                        { data: 'Placa' },
                        { data: 'Modelo' },
                        { data: 'Status' }
                    ]
                });
            },
            error: function(xhr, status, error) {
                // [DEBUG] Log detalhado do erro
                console.error("Erro ao carregar veículos:", error);
                Alerta.TratamentoErroComLinha("veiculo-list.js", "carregarVeiculos", error);
            }
        });
    } catch (erro) {
        Alerta.TratamentoErroComLinha("veiculo-list.js", "carregarVeiculos", erro);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: excluir
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Confirmar e executar exclusão de veículo do sistema
 *
 * 📥 ENTRADAS     : id [number] - ID do veículo a ser excluído
 *
 * 📤 SAÍDAS       : Promise<void> - Veículo excluído ou mensagem de erro
 *
 * ⬅️ CHAMADO POR  : Evento onclick do botão .btn-excluir [grid DataTable]
 *
 * ➡️ CHAMA        : Alerta.Confirmar() [alerta.js]
 *                   POST /api/Veiculo/Delete [AJAX]
 *                   carregarVeiculos() [linha 25] - após sucesso
 *
 * 📝 OBSERVAÇÕES  : Valida se veículo tem viagens ativas antes de excluir no backend
 ****************************************************************************************/
async function excluir(id) {
    try {
        // [UI] Exibir confirmação padronizada FrotiX
        const confirmado = await Alerta.Confirmar(
            "Confirmar Exclusão",
            "Tem certeza que deseja excluir este veículo?",
            "Sim, excluir",
            "Cancelar"
        );

        if (!confirmado) return;

        /********************************************************************************
         * [AJAX] Endpoint: POST /api/Veiculo/Delete
         * ------------------------------------------------------------------------------
         * 📥 ENVIA        : { id: number }
         * 📤 RECEBE       : { success: bool, message: string }
         * 🎯 MOTIVO       : Excluir veículo do banco de dados após validações de negócio
         *                   (verifica viagens ativas, contratos vinculados)
         ********************************************************************************/
        $.post('/api/Veiculo/Delete', { id: id }, function(response) {
            // [VALIDACAO] Verificar se exclusão foi bem-sucedida
            if (response.success) {
                Alerta.Sucesso("Sucesso", response.message);
                // [UI] Recarregar grid para refletir exclusão
                carregarVeiculos();
            } else {
                Alerta.Erro("Erro", response.message);
            }
        }).fail(function(xhr, status, error) {
            // [DEBUG] Log detalhado do erro
            console.error("Erro ao excluir veículo:", error);
            Alerta.TratamentoErroComLinha("veiculo-list.js", "excluir", error);
        });

    } catch (erro) {
        Alerta.TratamentoErroComLinha("veiculo-list.js", "excluir", erro);
    }
}
```

---

### 5.13.7 Incorporação de Informações dos Agentes

**REGRA:** Ao realizar a segunda passada, **incorporar** informações levantadas pelos agentes de análise anteriores:

1. **De MapeamentoDependencias.md:**
   - Adicionar dependências identificadas em `📦 DEPENDÊNCIAS`
   - Documentar chamadas em `➡️ CHAMA`
   - Documentar quem chama em `⬅️ CHAMADO POR`

2. **De ArquivosCriticos.md:**
   - Adicionar `📝 OBSERVAÇÕES` sobre problemas conhecidos
   - Documentar workarounds temporários
   - Referenciar issues pendentes

3. **De análises de padrões:**
   - Documentar padrões arquiteturais identificados
   - Explicar decisões de design
   - Justificar uso de bibliotecas específicas

**Exemplo:**
```csharp
/****************************************************************************************
 * 📝 OBSERVAÇÕES  : Este arquivo usa mix intencional Syncfusion + Kendo devido a
 *                   problemas conhecidos do DatePicker Syncfusion com timezones.
 *                   Ver ArquivosCriticos.md #45 para detalhes.
 *
 *                   Dependências identificadas por análise automática:
 *                   - IUnitOfWork.Viagem (10 chamadas)
 *                   - ILogger<ViagemController> (logging em todas as actions)
 *                   - ServicosAsync (cálculos de custo)
 ****************************************************************************************/
```

---

### 5.13.8 Validação de Completude

**Antes de considerar um arquivo concluído na segunda passada, validar:**

✅ **Checklist Final:**
- [ ] Card de arquivo presente e completo
- [ ] TODAS as funções têm card ⚡ (públicas obrigatório, privadas complexas recomendado)
- [ ] TODAS as chamadas AJAX têm 📥📤🎯
- [ ] Rastreabilidade completa (⬅️ ➡️) em todas as funções
- [ ] Comentários inline em lógica complexa (não-óbvia)
- [ ] SEM comentários óbvios ou redundantes
- [ ] Try-catch em TODAS as funções (regra inviolável)
- [ ] Informações dos agentes incorporadas
- [ ] Sintaxe validada (código não quebrado)
- [ ] Formatação consistente mantida

**Ao completar lote de arquivos:**
- Atualizar `DocumentacaoIntracodigo.md` com percentual de conclusão
- Commit: `docs: Segunda passada Lote [N] - [pasta] ([quantidade] arquivos enriquecidos)`

---

## 📝 IMPORTANTE: MEMÓRIA PERMANENTE

Este arquivo, `RegrasDesenvolvimentoFrotiX.md`, atua como a **MEMÓRIA PERMANENTE** do projeto.
Qualquer regra, padrão ou instrução que deva ser "memorizada" pelo agente deve ser adicionada aqui.

**AGENTES (Claude/Gemini/Copilot):**

1. **LEITURA OBRIGATÓRIA:** Você DEVE ler e seguir estritamente as regras deste arquivo.
2. **ESCRITA:** Se o usuário pedir para "memorizar" algo, adicione neste arquivo.
