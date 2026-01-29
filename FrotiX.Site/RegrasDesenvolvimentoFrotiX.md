# 📘 Regras de Desenvolvimento FrotiX – Arquivo Consolidado

> **Projeto:** FrotiX 2026 – FrotiX.Site
> **Tipo:** Aplicação Web ASP.NET Core MVC – Gestão de Frotas
> **Stack:** .NET 10, C#, Entity Framework Core, SQL Server, Bootstrap 5.3, jQuery, Syncfusion EJ2, Telerik UI
> **Status:** ✅ Arquivo ÚNICO e OFICIAL de regras do projeto
> **Versão:** 1.2
> **Última Atualização:** 29/01/2026

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

### 3.3 CSS

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
| 1.2    | 29/01/2026 | Atualização completa dos padrões visuais de Cards (Arquivo e Função) com ícones  |
| 1.1    | 18/01/2026 | Adiciona regras de commit/push automáticos e push obrigatório para main         |
| 1.0    | 14/01/2026 | Consolidação inicial (CLAUDE.md + GEMINI.md + RegrasDesenvolvimentoFrotiXPOE.md) |

---

## 📝 5. DOCUMENTAÇÃO INTRA-CÓDIGO (NOVO PADRÃO VISUAL MANDATÓRIO)

> 📁 **Arquivo de Acompanhamento:** `DocumentacaoIntracodigo.md` - Usado para mapear o andamento do processo de documentação

### 5.1 Visão Geral

Cada arquivo de código (C# ou JS) deve ser um artefato auto-explicativo. Adotamos um padrão visual de **"Cards"** com bordas ASCII elaboradas e **ícones semânticos** para garantir leitura rápida, visual impactante e manutenção segura.

---

### 5.2 Card do Arquivo (Table of Contents)

**REGRA:** Todo arquivo (.cs ou .js) DEVE iniciar com um **Card Mestre** contendo o índice de suas funcionalidades.

#### ✅ Modelo Visual C#

```csharp
/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: NomeDoArquivo.cs                                                                       ║
   ║ 📂 CAMINHO: /Pasta/Subpasta                                                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Descrever brevemente a responsabilidade desta classe ou módulo.                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [NomeFuncao1] : Breve descrição.............. (int id) -> bool                                  ║
   ║ 2. [NomeFuncao2] : Outra descrição.............. (string x) -> ActionResult                        ║
   ║ ...                                                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
```

#### ✅ Modelo Visual JavaScript

```javascript
/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: nomeDoArquivo.js                                                                       ║
   ║ 📂 CAMINHO: /wwwroot/js/                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Descrever brevemente a responsabilidade deste módulo JavaScript.                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [nomeFuncao1] : Breve descrição.............. (param1) -> void                                  ║
   ║ 2. [nomeFuncao2] : Outra descrição.............. (dados) -> Promise                                ║
   ║ ...                                                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
```

---

### 5.3 Card da Função (Rico em Ícones)

**REGRA:** O cabeçalho de cada função deve ser visualmente impactante, mantendo compatibilidade com IntelliSense (XML Docs/JSDoc).

#### ✅ Modelo Visual C# (XML Docs)

```csharp
/// <summary>
/// ╭───────────────────────────────────────────────────────────────────────────────────────╮
/// │ ⚡ FUNCIONALIDADE: NomeDaFuncao                                                       │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
/// │    Explicação clara da regra de negócio, comportamento e validações.                  │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 📥 INPUTS (Entradas):                                                                 │
/// │    • param1 [int]: Descrição do parâmetro.                                            │
/// │    • param2 [string]: Descrição do segundo parâmetro.                                 │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 📤 OUTPUTS (Saídas):                                                                  │
/// │    • [bool]: O que retorna e em que condições.                                        │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🔗 RASTREABILIDADE (Quem chama e Quem é chamado):                                     │
/// │    ⬅️ CHAMADO POR : [Mapear quem invoca este método]                                  │
/// │    ➡️ CHAMA       : [Mapear serviços/métodos invocados internamente]                  │
/// ╰───────────────────────────────────────────────────────────────────────────────────────╯
/// </summary>
public IActionResult NomeDaFuncao(int param1, string param2)
{
    // ...
}
```

#### ✅ Modelo Visual JavaScript (JSDoc)

```javascript
/**
 * ╭───────────────────────────────────────────────────────────────────────────────────────╮
 * │ ⚡ FUNCIONALIDADE: nomeDaFuncao                                                       │
 * │───────────────────────────────────────────────────────────────────────────────────────│
 * │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
 * │    Explicação clara da regra de negócio, comportamento e validações.                  │
 * │───────────────────────────────────────────────────────────────────────────────────────│
 * │ 📥 INPUTS (Entradas):                                                                 │
 * │    • param1 [string]: Descrição do parâmetro.                                         │
 * │───────────────────────────────────────────────────────────────────────────────────────│
 * │ 📤 OUTPUTS (Saídas):                                                                  │
 * │    • [Promise]: O que retorna e em que condições.                                     │
 * │───────────────────────────────────────────────────────────────────────────────────────│
 * │ 🔗 RASTREABILIDADE (Quem chama e Quem é chamado):                                     │
 * │    ⬅️ CHAMADO POR : [onclick #btnSalvar] [carregarDados()]                            │
 * │    ➡️ CHAMA       : [validarFormulario()] [fetch /api/salvar]                         │
 * ╰───────────────────────────────────────────────────────────────────────────────────────╯
 */
function nomeDaFuncao(param1) {
    // ...
}
```

---

### 5.4 Comentários Internos (Tags Semânticas)

**REGRA:** Não use comentários genéricos. Use Tags para categorizar o propósito do bloco de código.

| Tag | Significado | Exemplo de Uso |
| :--- | :--- | :--- |
| `// [UI]` | Manipulação de DOM, CSS, Visibilidade | `Elemento.style.display = 'none'` |
| `// [LOGICA]` | Regras de fluxo, algoritmos, loops | `Calculo de média ponderada` |
| `// [REGRA]` | Regras de Negócio obrigatórias | `Validar se data fim > data inicio` |
| `// [DADOS]` | Manipulação de Objetos/JSON/Models | `Mapear ViewModel para DTO` |
| `// [AJAX]` | Chamadas HTTP, Fetch, APIs | `$.ajax(...)` ou `HttpClient` |
| `// [PERFORMANCE]` | Otimizações, Cache, Lazy Load | `Usar cache para evitar query` |
| `// [DEBUG]` | Logs, verificação de erros | `console.log("Valores:", val)` |
| `// [HELPER]` | Funções utilitárias locais | `FormatarData(...)` |
| `// [DB]` | Operações com Banco de Dados | `context.SaveChanges()` |
| `// [SEGURANCA]` | Validações de segurança | `Verificar permissão do usuário` |

---

### 5.5 Regra do Try-Catch (Obrigatório em TODAS as funções)

**REGRA:** Toda função DEVE ter try-catch com tratamento padrão FrotiX.

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

### 5.6 Exemplo Completo Aplicado

#### Exemplo JavaScript Completo

```javascript
/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: agendamento.js                                                                         ║
   ║ 📂 CAMINHO: /wwwroot/js/modulos/                                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Gerencia todas as operações de CRUD do módulo de Agendamentos.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [salvarAgendamento] : Persiste dados do form........ () -> void                                 ║
   ║ 2. [obterDadosFormulario] : Coleta campos do form..... () -> Object                                ║
   ║ ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

/**
 * ╭───────────────────────────────────────────────────────────────────────────────────────╮
 * │ ⚡ FUNCIONALIDADE: salvarAgendamento                                                  │
 * │───────────────────────────────────────────────────────────────────────────────────────│
 * │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
 * │    Coleta dados do formulário de agendamento, valida o período e envia para a API.   │
 * │───────────────────────────────────────────────────────────────────────────────────────│
 * │ 📥 INPUTS (Entradas):                                                                 │
 * │    • Nenhum (coleta do DOM)                                                           │
 * │───────────────────────────────────────────────────────────────────────────────────────│
 * │ 📤 OUTPUTS (Saídas):                                                                  │
 * │    • void - Recarrega página em caso de sucesso                                       │
 * │───────────────────────────────────────────────────────────────────────────────────────│
 * │ 🔗 RASTREABILIDADE:                                                                   │
 * │    ⬅️ CHAMADO POR : [onclick #btnSalvar]                                              │
 * │    ➡️ CHAMA       : [obterDadosFormulario()] [POST /api/agendamentos]                 │
 * ╰───────────────────────────────────────────────────────────────────────────────────────╯
 */
function salvarAgendamento() {
    try {
        // [UI] Bloquear botão para evitar duplo clique
        const btn = document.getElementById('btnSalvar');
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
        fetch('/api/agendamentos', { 
            method: 'POST', 
            body: JSON.stringify(dados),
            headers: { 'Content-Type': 'application/json' }
        })
            .then(r => r.json())
            .then(response => {
                // [LOGICA] Verificar sucesso real da API
                if(response.success) {
                    Alerta.Sucesso("Salvo", "Agendamento criado!");
                    window.location.reload();
                } else {
                    throw new Error(response.message);
                }
            })
            .catch(err => {
                // [DEBUG] Log para rastreabilidade
                console.error("Erro no save:", err);
                Alerta.TratamentoErroComLinha("agendamento.js", "salvarAgendamento", err);
            });

    } catch (e) {
        Alerta.TratamentoErroComLinha("agendamento.js", "salvarAgendamento", e);
    }
}
```

---

✅ **FIM DO DOCUMENTO**

📌 **Lembrete:** Este arquivo deve ser consultado no início de cada sessão de desenvolvimento ou interação com agentes de IA.

---

## 📝 IMPORTANTE: MEMÓRIA PERMANENTE

Este arquivo, `RegrasDesenvolvimentoFrotiX.md`, atua como a **MEMÓRIA PERMANENTE** do projeto.
Qualquer regra, padrão ou instrução que deva ser "memorizada" pelo agente deve ser adicionada aqui.

**AGENTES (Claude/Gemini/Copilot):**

1. **LEITURA OBRIGATÓRIA:** Você DEVE ler e seguir estritamente as regras deste arquivo.
2. **ESCRITA:** Se o usuário pedir para "memorizar" algo, adicione neste arquivo.
