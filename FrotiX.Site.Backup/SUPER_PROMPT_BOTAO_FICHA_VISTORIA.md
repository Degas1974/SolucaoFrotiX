# 🔧 SUPER PROMPT: Botão de Ficha de Vistoria na Página de Agenda

> **Data de Criação**: 22/01/2026
> **Prioridade**: ALTA
> **Status**: PENDENTE IMPLEMENTAÇÃO

---

## 📋 ÍNDICE

1. [Resumo Executivo](#resumo-executivo)
2. [Problema Detalhado](#problema-detalhado)
3. [Arquivos Envolvidos](#arquivos-envolvidos)
4. [Estrutura do Banco de Dados](#estrutura-do-banco-de-dados)
5. [Estado Atual vs Estado Esperado](#estado-atual-vs-estado-esperado)
6. [Solução Técnica](#solução-técnica)
7. [Script SQL Necessário](#script-sql-necessário)
8. [Alterações no Código JavaScript](#alterações-no-código-javascript)
9. [Testes e Validação](#testes-e-validação)
10. [Documentação a Atualizar](#documentação-a-atualizar)
11. [Regras de Negócio](#regras-de-negócio)

---

## 🎯 RESUMO EXECUTIVO

### Problema
O **botão laranja** que deveria aparecer ao lado do campo **"Destino"** na página de Agenda não está sendo exibido. Este botão é responsável por abrir um modal que mostra a Ficha de Vistoria do agendamento/viagem corrente.

### Causa Raiz
- O campo `TemFichaVistoriaReal` foi adicionado ao Model `Viagem` (linha 246 de [Models/Cadastros/Viagem.cs](Models/Cadastros/Viagem.cs#L246))
- O campo **NÃO está sendo usado** no JavaScript para controlar a visibilidade do botão
- Os registros existentes na tabela `Viagem` não foram atualizados com o valor correto de `TemFichaVistoriaReal`

### Impacto
- Usuários **NÃO CONSEGUEM** visualizar Fichas de Vistoria através da interface de Agenda
- Perda de funcionalidade crítica para operação

### Solução Resumida
1. Executar script SQL para popular `TemFichaVistoriaReal` com valores corretos
2. Alterar JavaScript para usar `TemFichaVistoriaReal` ao invés de verificar apenas `FichaVistoria`
3. Implementar lógica de exibição/bloqueio do botão baseado no campo
4. Testar em cenários reais

---

## 🔍 PROBLEMA DETALHADO

### Contexto Histórico

**21/01/2026**: Campo `TemFichaVistoriaReal` foi adicionado ao Model `Viagem`:

```csharp
/// <summary>
/// Indica se a viagem possui uma Ficha de Vistoria real (não a padrão/amarelinha).
/// True = Ficha real cadastrada, False/NULL = Sem ficha ou ficha padrão.
/// </summary>
[Display(Name = "Tem Ficha Real")]
public bool? TemFichaVistoriaReal { get; set; }
```

**Objetivo do Campo**: Diferenciar entre:
- Fichas de Vistoria **REAIS** (digitalizadas, PDFs, imagens de fichas físicas)
- Fichas de Vistoria **PADRÃO** (amarelinha gerada automaticamente pelo sistema)

### Comportamento Atual

1. **Botão HTML existe** em [Pages/Agenda/Index.cshtml](Pages/Agenda/Index.cshtml#L1084-L1091):

```html
<button type="button" id="btnVisualizarFichaVistoria"
    class="btn-ficha-vistoria ms-2"
    title="Visualizar Ficha de Vistoria"
    data-ejtip="Clique para visualizar a Ficha de Vistoria desta viagem"
    style="display: none;"
    disabled>
    <i class="fa-duotone fa-clipboard-list"></i>
</button>
```

2. **Botão é SEMPRE escondido** ao carregar nova viagem ([exibe-viagem.js:416-422](wwwroot/js/agendamento/components/exibe-viagem.js#L416-L422)):

```javascript
// Esconder botão de Ficha de Vistoria (não há ficha em nova viagem)
const btnFichaVistoria = document.getElementById("btnVisualizarFichaVistoria");
if (btnFichaVistoria)
{
    btnFichaVistoria.style.display = "none";
    btnFichaVistoria.disabled = true;
    btnFichaVistoria.dataset.viagemId = "";
    btnFichaVistoria.dataset.noFicha = "";
}
```

3. **NÃO HÁ LÓGICA** para mostrar o botão quando uma viagem existente com ficha real é exibida

4. **Event listener está implementado** ([exibe-viagem.js:4848-4867](wwwroot/js/agendamento/components/exibe-viagem.js#L4848-L4867)), mas botão nunca fica visível

---

## 📁 ARQUIVOS ENVOLVIDOS

| Arquivo | Caminho Completo | Função |
|---------|------------------|--------|
| **Model Viagem** | `Models/Cadastros/Viagem.cs` | Define campo `TemFichaVistoriaReal` (linha 246) |
| **Agenda HTML** | `Pages/Agenda/Index.cshtml` | Contém o botão `btnVisualizarFichaVistoria` (linha 1084) |
| **Exibe Viagem JS** | `wwwroot/js/agendamento/components/exibe-viagem.js` | Controla exibição do modal (função `exibirViagemExistente` linha 684) |
| **Script SQL** | `Scripts/AddTemFichaVistoriaReal.sql` | Script para adicionar coluna (JÁ EXECUTADO) |
| **Tabela Banco** | `dbo.Viagem` | Tabela com campo `TemFichaVistoriaReal BIT NULL` |

### Dependências

```
┌─────────────────────────────┐
│  dbo.Viagem (SQL Server)    │
│  Campo: TemFichaVistoriaReal│
└──────────┬──────────────────┘
           │
           ▼
┌─────────────────────────────┐
│  Models/Cadastros/Viagem.cs │
│  public bool? TemFicha...   │
└──────────┬──────────────────┘
           │
           ▼
┌─────────────────────────────┐
│  AgendaController.cs        │
│  Retorna objeto Viagem      │
└──────────┬──────────────────┘
           │
           ▼
┌─────────────────────────────┐
│  exibe-viagem.js            │
│  exibirViagemExistente()    │
│  → Controlar visibilidade   │
│     do botão                │
└──────────┬──────────────────┘
           │
           ▼
┌─────────────────────────────┐
│  Index.cshtml               │
│  btnVisualizarFichaVistoria │
│  (HTML do botão)            │
└─────────────────────────────┘
```

---

## 🗄️ ESTRUTURA DO BANCO DE DADOS

### Tabela: `dbo.Viagem`

#### Campo Relevante

```sql
CREATE TABLE dbo.Viagem (
    -- ... outros campos ...

    FichaVistoria VARBINARY(MAX) NULL,
    -- ^ Contém os bytes da imagem/PDF da ficha (pode ser NULL)

    TemFichaVistoriaReal BIT NULL,
    -- ^ Indica se FichaVistoria contém uma ficha REAL (não a amarelinha padrão)
    -- TRUE = Ficha real digitalizada
    -- FALSE ou NULL = Sem ficha ou ficha padrão do sistema

    NoFichaVistoria INT NULL,
    -- ^ Número da ficha física (0 = mobile/sem ficha)

    -- ... outros campos ...
)
```

#### Relação entre campos

| Campo | Tipo | Nullable | Descrição |
|-------|------|----------|-----------|
| `FichaVistoria` | `VARBINARY(MAX)` | ✅ | Bytes da imagem/PDF da ficha |
| `TemFichaVistoriaReal` | `BIT` | ✅ | Flag: TRUE = ficha real, FALSE/NULL = sem ficha real |
| `NoFichaVistoria` | `INT` | ✅ | Número da ficha física (0 ou NULL = mobile) |

#### Casos de Uso

| Cenário | FichaVistoria | TemFichaVistoriaReal | NoFichaVistoria | Interpretação |
|---------|---------------|---------------------|-----------------|---------------|
| Viagem com ficha real digitalizada | `[bytes]` | `TRUE` | `123` | ✅ **MOSTRAR BOTÃO** |
| Viagem com ficha amarelinha (padrão) | `[bytes]` | `FALSE` | `0` ou `NULL` | ❌ Não mostrar botão |
| Viagem sem ficha (mobile) | `NULL` | `FALSE` ou `NULL` | `0` ou `NULL` | ❌ Não mostrar botão |
| Viagem antiga (antes do campo) | `[bytes]` | `NULL` | `456` | ⚠️ Precisa atualização! |

---

## ⚖️ ESTADO ATUAL VS ESTADO ESPERADO

### 🔴 ESTADO ATUAL (ERRADO)

#### Interface Usuário
```
┌─────────────────────────────────────┐
│ Destino: [Brasília-DF       ▼]     │  ← BOTÃO LARANJA AUSENTE!
└─────────────────────────────────────┘
```

#### Código JavaScript
```javascript
// ❌ SEMPRE esconde o botão
const btnFichaVistoria = document.getElementById("btnVisualizarFichaVistoria");
if (btnFichaVistoria) {
    btnFichaVistoria.style.display = "none";  // SEMPRE ESCONDIDO
    btnFichaVistoria.disabled = true;
}
```

#### Banco de Dados
```sql
-- ⚠️ Registros existentes com FichaVistoria preenchida
-- mas TemFichaVistoriaReal = NULL (não atualizado)

SELECT TOP 5
    ViagemId,
    NoFichaVistoria,
    CASE WHEN FichaVistoria IS NULL THEN 'NULL' ELSE 'COM BYTES' END AS FichaVistoria,
    ISNULL(CAST(TemFichaVistoriaReal AS VARCHAR), 'NULL') AS TemFichaVistoriaReal
FROM Viagem
WHERE FichaVistoria IS NOT NULL;

-- Resultado esperado:
-- ViagemId | NoFichaVistoria | FichaVistoria | TemFichaVistoriaReal
-- ---------|-----------------|---------------|---------------------
-- abc...   | 123             | COM BYTES     | NULL  ← PROBLEMA!
-- def...   | 456             | COM BYTES     | NULL  ← PROBLEMA!
```

---

### ✅ ESTADO ESPERADO (CORRETO)

#### Interface Usuário - Caso 1: Viagem COM Ficha Real

```
┌─────────────────────────────────────────────┐
│ Destino: [Brasília-DF       ▼] [📋]        │  ← BOTÃO LARANJA VISÍVEL
└─────────────────────────────────────────────┘
                                    ▲
                                    └─ Botão laranja ATIVO
                                       Tooltip: "Clique para visualizar..."
```

#### Interface Usuário - Caso 2: Viagem SEM Ficha Real

```
┌─────────────────────────────────────────────┐
│ Destino: [Brasília-DF       ▼] [📋]        │  ← BOTÃO CINZA DESABILITADO
└─────────────────────────────────────────────┘
                                    ▲
                                    └─ Botão cinza BLOQUEADO
                                       Tooltip: "Sem ficha de vistoria"
```

#### Interface Usuário - Caso 3: Nova Viagem

```
┌─────────────────────────────────────────────┐
│ Destino: [                  ▼]             │  ← BOTÃO OCULTO
└─────────────────────────────────────────────┘
    (Não há viagem carregada, botão invisível)
```

#### Código JavaScript (ESPERADO)

```javascript
// ✅ LÓGICA CORRETA - Controlar baseado em TemFichaVistoriaReal
const btnFichaVistoria = document.getElementById("btnVisualizarFichaVistoria");
if (btnFichaVistoria && objViagem) {
    // Verificar se é nova viagem
    if (!objViagem.viagemId || objViagem.viagemId === '00000000-0000-0000-0000-000000000000') {
        // Nova viagem: ESCONDER botão
        btnFichaVistoria.style.display = "none";
        btnFichaVistoria.disabled = true;
    } else {
        // Viagem existente: SEMPRE MOSTRAR botão
        btnFichaVistoria.style.display = "inline-block";

        // Verificar se tem ficha REAL
        if (objViagem.temFichaVistoriaReal === true) {
            // TEM FICHA REAL: Botão ATIVO (laranja)
            btnFichaVistoria.disabled = false;
            btnFichaVistoria.classList.remove("btn-ficha-vistoria-sem");
            btnFichaVistoria.classList.add("btn-ficha-vistoria");
            btnFichaVistoria.title = "Visualizar Ficha de Vistoria";
            btnFichaVistoria.dataset.viagemId = objViagem.viagemId;
            btnFichaVistoria.dataset.noFicha = objViagem.noFichaVistoria || "";
        } else {
            // SEM FICHA REAL: Botão BLOQUEADO (cinza)
            btnFichaVistoria.disabled = true;
            btnFichaVistoria.classList.remove("btn-ficha-vistoria");
            btnFichaVistoria.classList.add("btn-ficha-vistoria-sem");
            btnFichaVistoria.title = "Sem Ficha de Vistoria";
            btnFichaVistoria.dataset.viagemId = "";
            btnFichaVistoria.dataset.noFicha = "";
        }
    }
}
```

#### Banco de Dados (ESPERADO)

```sql
-- ✅ Registros atualizados corretamente

SELECT TOP 5
    ViagemId,
    NoFichaVistoria,
    CASE WHEN FichaVistoria IS NULL THEN 'NULL' ELSE 'COM BYTES' END AS FichaVistoria,
    CAST(TemFichaVistoriaReal AS VARCHAR) AS TemFichaVistoriaReal
FROM Viagem
ORDER BY DataCriacao DESC;

-- Resultado esperado:
-- ViagemId | NoFichaVistoria | FichaVistoria | TemFichaVistoriaReal
-- ---------|-----------------|---------------|---------------------
-- abc...   | 123             | COM BYTES     | 1       ← CORRETO!
-- def...   | 0               | NULL          | 0       ← CORRETO!
-- ghi...   | 456             | COM BYTES     | 1       ← CORRETO!
```

---

## 🛠️ SOLUÇÃO TÉCNICA

### Etapa 1: Atualizar Banco de Dados

**CRÍTICO**: Executar script SQL para popular `TemFichaVistoriaReal` em registros existentes.

### Etapa 2: Modificar JavaScript

**Arquivo**: `wwwroot/js/agendamento/components/exibe-viagem.js`

**Função Alvo**: `exibirViagemExistente(objViagem)` (linha 684)

**Local de Inserção**: Após popular campo "Destino" (aproximadamente linha 1260)

### Etapa 3: Adicionar CSS para Estado Bloqueado

**Arquivo**: `wwwroot/css/frotix.css` ou `wwwroot/css/modal-viagens-consolidado.css`

**Estilos Necessários**:
- `.btn-ficha-vistoria` → Botão ativo (laranja)
- `.btn-ficha-vistoria-sem` → Botão bloqueado (cinza)

### Etapa 4: Testar Cenários

1. Nova viagem → Botão invisível
2. Viagem com ficha real → Botão laranja ativo
3. Viagem sem ficha → Botão cinza bloqueado
4. Clique no botão → Modal abre com ficha correta

---

## 💾 SCRIPT SQL NECESSÁRIO

### Script 1: Verificar Estado Atual

```sql
-- ============================================
-- DIAGNÓSTICO: Verificar estado atual dos dados
-- ============================================

-- Verificar se coluna existe
SELECT
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Viagem'
  AND COLUMN_NAME = 'TemFichaVistoriaReal';

-- Se retornar resultado: coluna existe ✅
-- Se não retornar: coluna NÃO existe ❌ (executar AddTemFichaVistoriaReal.sql primeiro)

-- Verificar distribuição de valores
SELECT
    CASE WHEN TemFichaVistoriaReal IS NULL THEN 'NULL'
         WHEN TemFichaVistoriaReal = 1 THEN 'TRUE'
         ELSE 'FALSE'
    END AS TemFichaVistoriaReal,
    COUNT(*) AS Quantidade
FROM dbo.Viagem
GROUP BY TemFichaVistoriaReal
ORDER BY TemFichaVistoriaReal;

-- Resultado esperado ANTES do update:
-- TemFichaVistoriaReal | Quantidade
-- ---------------------|------------
-- NULL                 | 15000  ← PROBLEMA! Maioria NULL
-- FALSE                | 100
-- TRUE                 | 50

-- Verificar casos problemáticos (FichaVistoria preenchida mas TemFichaVistoriaReal NULL)
SELECT
    COUNT(*) AS RegistrosProblematicos
FROM dbo.Viagem
WHERE FichaVistoria IS NOT NULL
  AND TemFichaVistoriaReal IS NULL;

-- Se > 0: Precisa executar Script 2
```

### Script 2: Atualizar Registros Existentes (EXECUTAR!)

```sql
-- ============================================
-- ATUALIZAÇÃO: Popular TemFichaVistoriaReal
-- Data: 22/01/2026
-- Descrição: Atualiza campo baseado em FichaVistoria
-- ============================================

-- 🛡️ SEGURANÇA: Fazer backup antes
-- BACKUP DATABASE FrotiX TO DISK = 'C:\Backup\FrotiX_PreUpdateTemFicha_22012026.bak';

BEGIN TRANSACTION;

DECLARE @TotalAtualizados INT = 0;
DECLARE @ComFichaReal INT = 0;
DECLARE @SemFichaReal INT = 0;

-- ═══════════════════════════════════════════════════════════════
-- 🔹 REGRA DE NEGÓCIO:
-- TemFichaVistoriaReal = TRUE  → Quando FichaVistoria NÃO é NULL
-- TemFichaVistoriaReal = FALSE → Quando FichaVistoria é NULL
-- ═══════════════════════════════════════════════════════════════

-- Atualizar registros COM ficha (FichaVistoria IS NOT NULL)
UPDATE dbo.Viagem
SET TemFichaVistoriaReal = 1
WHERE FichaVistoria IS NOT NULL
  AND (TemFichaVistoriaReal IS NULL OR TemFichaVistoriaReal = 0);

SET @ComFichaReal = @@ROWCOUNT;
PRINT '✅ Registros COM ficha real atualizados: ' + CAST(@ComFichaReal AS VARCHAR);

-- Atualizar registros SEM ficha (FichaVistoria IS NULL)
UPDATE dbo.Viagem
SET TemFichaVistoriaReal = 0
WHERE FichaVistoria IS NULL
  AND (TemFichaVistoriaReal IS NULL OR TemFichaVistoriaReal = 1);

SET @SemFichaReal = @@ROWCOUNT;
PRINT '✅ Registros SEM ficha real atualizados: ' + CAST(@SemFichaReal AS VARCHAR);

SET @TotalAtualizados = @ComFichaReal + @SemFichaReal;
PRINT '📊 Total de registros atualizados: ' + CAST(@TotalAtualizados AS VARCHAR);

-- Verificar resultado
SELECT
    CASE WHEN TemFichaVistoriaReal = 1 THEN 'COM Ficha Real'
         WHEN TemFichaVistoriaReal = 0 THEN 'SEM Ficha Real'
         ELSE 'NULL (ERRO!)'
    END AS Status,
    COUNT(*) AS Quantidade
FROM dbo.Viagem
GROUP BY TemFichaVistoriaReal
ORDER BY TemFichaVistoriaReal DESC;

-- Verificar se ainda há NULLs (não deveria!)
IF EXISTS (SELECT 1 FROM dbo.Viagem WHERE TemFichaVistoriaReal IS NULL)
BEGIN
    PRINT '⚠️ ATENÇÃO: Ainda existem registros com TemFichaVistoriaReal = NULL!';
    ROLLBACK TRANSACTION;
    RAISERROR('Atualização incompleta. Verificar dados.', 16, 1);
END
ELSE
BEGIN
    PRINT '✅ Todos os registros foram atualizados corretamente!';
    COMMIT TRANSACTION;
    PRINT '🎉 TRANSAÇÃO CONFIRMADA COM SUCESSO!';
END

GO
```

### Script 3: Validação Pós-Atualização

```sql
-- ============================================
-- VALIDAÇÃO: Verificar integridade dos dados
-- ============================================

-- 1. Verificar se há incoerências (FichaVistoria preenchida mas flag = FALSE)
SELECT
    ViagemId,
    NoFichaVistoria,
    CASE WHEN FichaVistoria IS NULL THEN 'NULL' ELSE 'PREENCHIDO' END AS FichaVistoria,
    TemFichaVistoriaReal
FROM dbo.Viagem
WHERE (FichaVistoria IS NOT NULL AND TemFichaVistoriaReal = 0)
   OR (FichaVistoria IS NULL AND TemFichaVistoriaReal = 1);

-- Resultado esperado: 0 registros (nenhuma incoerência)

-- 2. Estatísticas finais
SELECT
    'Total de Viagens' AS Metrica,
    COUNT(*) AS Valor
FROM dbo.Viagem

UNION ALL

SELECT
    'Com Ficha Real' AS Metrica,
    COUNT(*) AS Valor
FROM dbo.Viagem
WHERE TemFichaVistoriaReal = 1

UNION ALL

SELECT
    'Sem Ficha Real' AS Metrica,
    COUNT(*) AS Valor
FROM dbo.Viagem
WHERE TemFichaVistoriaReal = 0

UNION ALL

SELECT
    'NULL (ERRO)' AS Metrica,
    COUNT(*) AS Valor
FROM dbo.Viagem
WHERE TemFichaVistoriaReal IS NULL;

-- 3. Top 10 viagens COM ficha real (para teste visual)
SELECT TOP 10
    v.ViagemId,
    v.NoFichaVistoria,
    v.DataInicial,
    ve.Placa,
    m.Nome AS Motorista,
    v.Destino,
    v.TemFichaVistoriaReal
FROM dbo.Viagem v
LEFT JOIN dbo.Veiculo ve ON v.VeiculoId = ve.VeiculoId
LEFT JOIN dbo.Motorista m ON v.MotoristaId = m.MotoristaId
WHERE v.TemFichaVistoriaReal = 1
ORDER BY v.DataCriacao DESC;

-- Use esses registros para testar a interface!
```

---

## 📝 ALTERAÇÕES NO CÓDIGO JAVASCRIPT

### Arquivo: `wwwroot/js/agendamento/components/exibe-viagem.js`

### Localização da Alteração

**Função**: `exibirViagemExistente(objViagem)`
**Linha Aproximada**: 1260 (após popular campo Destino)
**Seção**: Após bloco de código que preenche `cmbDestino`

### Código ATUAL (Linhas 1252-1260)

```javascript
if (objViagem.destino)
{
    const cmbDestino = document.getElementById("cmbDestino");
    if (cmbDestino && cmbDestino.ej2_instances && cmbDestino.ej2_instances[0])
    {
        cmbDestino.ej2_instances[0].value = objViagem.destino;
        cmbDestino.ej2_instances[0].dataBind();
    }
}

// 11. Descrição
if (objViagem.descricao)
{
    // ...
}
```

### Código NOVO (Inserir APÓS linha 1260)

```javascript
if (objViagem.destino)
{
    const cmbDestino = document.getElementById("cmbDestino");
    if (cmbDestino && cmbDestino.ej2_instances && cmbDestino.ej2_instances[0])
    {
        cmbDestino.ej2_instances[0].value = objViagem.destino;
        cmbDestino.ej2_instances[0].dataBind();
    }
}

// ═══════════════════════════════════════════════════════════════
// 🔹 BLOCO: Controlar Botão de Ficha de Vistoria
// Exibe botão ao lado do campo Destino quando viagem tem ficha real.
// Botão fica BLOQUEADO (cinza) se não houver ficha.
// Data: 22/01/2026
// ═══════════════════════════════════════════════════════════════
try
{
    const btnFichaVistoria = document.getElementById("btnVisualizarFichaVistoria");
    if (btnFichaVistoria)
    {
        // Verificar se é viagem existente (não nova)
        const isViagemExistente = objViagem.viagemId &&
                                  objViagem.viagemId !== '00000000-0000-0000-0000-000000000000';

        if (!isViagemExistente)
        {
            // Nova viagem: ESCONDER botão
            btnFichaVistoria.style.display = "none";
            btnFichaVistoria.disabled = true;
            btnFichaVistoria.dataset.viagemId = "";
            btnFichaVistoria.dataset.noFicha = "";
            console.log("🔘 [FichaVistoria] Botão OCULTO (nova viagem)");
        }
        else
        {
            // Viagem existente: MOSTRAR botão
            btnFichaVistoria.style.display = "inline-block";

            // Verificar se tem ficha REAL (campo do banco TemFichaVistoriaReal)
            const temFichaReal = objViagem.temFichaVistoriaReal === true ||
                                 objViagem.temFichaVistoriaReal === 1 ||
                                 objViagem.temFichaVistoriaReal === "true";

            if (temFichaReal)
            {
                // TEM FICHA REAL: Botão ATIVO (laranja)
                btnFichaVistoria.disabled = false;
                btnFichaVistoria.classList.remove("btn-ficha-vistoria-sem");
                btnFichaVistoria.classList.add("btn-ficha-vistoria");
                btnFichaVistoria.setAttribute("data-ejtip", "Clique para visualizar a Ficha de Vistoria desta viagem");
                btnFichaVistoria.title = "Visualizar Ficha de Vistoria";
                btnFichaVistoria.dataset.viagemId = objViagem.viagemId;
                btnFichaVistoria.dataset.noFicha = objViagem.noFichaVistoria || "";

                console.log(`✅ [FichaVistoria] Botão ATIVO - ViagemId: ${objViagem.viagemId}, NoFicha: ${objViagem.noFichaVistoria}`);
            }
            else
            {
                // SEM FICHA REAL: Botão BLOQUEADO (cinza)
                btnFichaVistoria.disabled = true;
                btnFichaVistoria.classList.remove("btn-ficha-vistoria");
                btnFichaVistoria.classList.add("btn-ficha-vistoria-sem");
                btnFichaVistoria.setAttribute("data-ejtip", "Esta viagem não possui Ficha de Vistoria digitalizada");
                btnFichaVistoria.title = "Sem Ficha de Vistoria";
                btnFichaVistoria.dataset.viagemId = "";
                btnFichaVistoria.dataset.noFicha = "";

                console.log(`⚠️ [FichaVistoria] Botão BLOQUEADO - Viagem sem ficha real`);
            }
        }
    }
}
catch (error)
{
    console.error("[FichaVistoria] Erro ao configurar botão de Ficha de Vistoria:", error);
    Alerta.TratamentoErroComLinha("exibe-viagem.js", "exibirViagemExistente (btnFichaVistoria)", error);
}

// 11. Descrição
if (objViagem.descricao)
{
    // ...
}
```

### Remover Código Antigo

**Localização**: Função `configurarModalParaNovo()` (linha aproximada 415-423)

**Código a REMOVER** (ou comentar):

```javascript
// ❌ REMOVER ESTE BLOCO - Lógica agora está em exibirViagemExistente
// Esconder botão de Ficha de Vistoria (não há ficha em nova viagem)
const btnFichaVistoria = document.getElementById("btnVisualizarFichaVistoria");
if (btnFichaVistoria)
{
    btnFichaVistoria.style.display = "none";
    btnFichaVistoria.disabled = true;
    btnFichaVistoria.dataset.viagemId = "";
    btnFichaVistoria.dataset.noFicha = "";
}
```

**Substituir por**:

```javascript
// ✅ NOVO CÓDIGO - Controle agora está centralizado em exibirViagemExistente
// Botão será controlado dinamicamente baseado em TemFichaVistoriaReal
```

---

## 🎨 ALTERAÇÕES NO CSS

### Arquivo: `wwwroot/css/frotix.css` ou `wwwroot/css/modal-viagens-consolidado.css`

### Estilos Necessários

```css
/* ═══════════════════════════════════════════════════════════════
   BOTÃO DE FICHA DE VISTORIA - ao lado do campo Destino
   ══════════════════════════════════════════════════════════════ */

/* Botão ATIVO (laranja) - Quando tem ficha real */
.btn-ficha-vistoria {
    background: linear-gradient(135deg, #ff6b35 0%, #ff8c42 100%);
    color: white;
    border: none;
    border-radius: 8px;
    padding: 10px 14px;
    font-size: 18px;
    cursor: pointer;
    transition: all 0.3s ease;
    box-shadow: 0 2px 8px rgba(255, 107, 53, 0.3);
    display: inline-flex;
    align-items: center;
    justify-content: center;
    min-width: 44px;
    min-height: 44px;
}

.btn-ficha-vistoria:hover:not(:disabled) {
    background: linear-gradient(135deg, #ff8c42 0%, #ff6b35 100%);
    box-shadow: 0 4px 12px rgba(255, 107, 53, 0.5);
    transform: translateY(-2px);
}

.btn-ficha-vistoria:active:not(:disabled) {
    transform: translateY(0);
    box-shadow: 0 2px 6px rgba(255, 107, 53, 0.4);
}

.btn-ficha-vistoria i {
    font-size: 20px;
}

/* Botão BLOQUEADO (cinza) - Quando NÃO tem ficha real */
.btn-ficha-vistoria-sem {
    background: linear-gradient(135deg, #6c757d 0%, #5a6268 100%);
    color: #dee2e6;
    border: none;
    border-radius: 8px;
    padding: 10px 14px;
    font-size: 18px;
    cursor: not-allowed;
    transition: all 0.3s ease;
    box-shadow: 0 2px 8px rgba(108, 117, 125, 0.2);
    display: inline-flex;
    align-items: center;
    justify-content: center;
    min-width: 44px;
    min-height: 44px;
    opacity: 0.6;
}

.btn-ficha-vistoria-sem i {
    font-size: 20px;
    opacity: 0.7;
}

/* Container do Destino + Botão (d-flex no HTML) */
.destino-container {
    display: flex;
    align-items: center;
    gap: 8px;
}

/* Responsividade */
@media (max-width: 576px) {
    .btn-ficha-vistoria,
    .btn-ficha-vistoria-sem {
        padding: 8px 12px;
        font-size: 16px;
        min-width: 40px;
        min-height: 40px;
    }

    .btn-ficha-vistoria i,
    .btn-ficha-vistoria-sem i {
        font-size: 18px;
    }
}
```

---

## ✅ TESTES E VALIDAÇÃO

### Checklist de Testes

#### 1. Preparação
- [ ] Backup do banco de dados realizado
- [ ] Script SQL executado com sucesso
- [ ] Campo `TemFichaVistoriaReal` atualizado em todos os registros
- [ ] Código JavaScript atualizado em `exibe-viagem.js`
- [ ] CSS adicionado em `frotix.css`
- [ ] Build da aplicação bem-sucedido
- [ ] Cache do navegador limpo (Ctrl + F5)

#### 2. Cenário 1: Nova Viagem/Agendamento
**Passos**:
1. Abrir página de Agenda
2. Clicar em "Novo Agendamento" ou em uma data vazia no calendário
3. Preencher campos obrigatórios (Finalidade, Veículo, Motorista, Destino)

**Resultado Esperado**:
- [ ] Botão de Ficha de Vistoria **NÃO APARECE** ao lado do Destino
- [ ] Console não mostra erros JavaScript
- [ ] Log no console: `"🔘 [FichaVistoria] Botão OCULTO (nova viagem)"`

#### 3. Cenário 2: Editar Viagem COM Ficha Real
**Passos**:
1. Executar query para identificar viagem com ficha:
   ```sql
   SELECT TOP 1 ViagemId, NoFichaVistoria, Destino
   FROM Viagem
   WHERE TemFichaVistoriaReal = 1
   ORDER BY DataCriacao DESC;
   ```
2. No calendário, clicar na viagem identificada
3. Modal abre com dados da viagem

**Resultado Esperado**:
- [ ] Botão de Ficha de Vistoria **APARECE** ao lado do campo Destino
- [ ] Botão está **ATIVO** (cor laranja)
- [ ] Botão **NÃO está desabilitado** (cursor: pointer)
- [ ] Tooltip mostra: "Clique para visualizar a Ficha de Vistoria desta viagem"
- [ ] `btn.dataset.viagemId` está preenchido corretamente
- [ ] `btn.dataset.noFicha` está preenchido corretamente
- [ ] Console mostra: `"✅ [FichaVistoria] Botão ATIVO - ViagemId: ..., NoFicha: ..."`

#### 4. Cenário 3: Editar Viagem SEM Ficha Real
**Passos**:
1. Executar query para identificar viagem sem ficha:
   ```sql
   SELECT TOP 1 ViagemId, NoFichaVistoria, Destino
   FROM Viagem
   WHERE TemFichaVistoriaReal = 0
   ORDER BY DataCriacao DESC;
   ```
2. No calendário, clicar na viagem identificada
3. Modal abre com dados da viagem

**Resultado Esperado**:
- [ ] Botão de Ficha de Vistoria **APARECE** ao lado do campo Destino
- [ ] Botão está **BLOQUEADO** (cor cinza)
- [ ] Botão está **desabilitado** (cursor: not-allowed)
- [ ] Tooltip mostra: "Esta viagem não possui Ficha de Vistoria digitalizada"
- [ ] `btn.dataset.viagemId` está **VAZIO**
- [ ] `btn.dataset.noFicha` está **VAZIO**
- [ ] Console mostra: `"⚠️ [FichaVistoria] Botão BLOQUEADO - Viagem sem ficha real"`

#### 5. Cenário 4: Clicar no Botão (Viagem COM Ficha)
**Passos**:
1. Carregar viagem com `TemFichaVistoriaReal = 1`
2. Clicar no botão laranja ao lado do Destino

**Resultado Esperado**:
- [ ] Modal de Ficha de Vistoria **ABRE**
- [ ] Spinner de carregamento **APARECE** inicialmente
- [ ] Imagem da ficha **CARREGA** após alguns segundos
- [ ] Imagem é **VISÍVEL** e **LEGÍVEL**
- [ ] Console mostra detalhes da requisição à API
- [ ] Não há erros 404 ou 500 no Network

#### 6. Cenário 5: Clicar no Botão Bloqueado (Viagem SEM Ficha)
**Passos**:
1. Carregar viagem com `TemFichaVistoriaReal = 0`
2. Tentar clicar no botão cinza

**Resultado Esperado**:
- [ ] Clique **NÃO faz nada** (botão desabilitado)
- [ ] Modal **NÃO abre**
- [ ] Cursor mostra "not-allowed"

#### 7. Cenário 6: Responsividade Mobile
**Passos**:
1. Abrir DevTools (F12)
2. Ativar modo responsivo (Ctrl + Shift + M)
3. Selecionar iPhone 12 Pro ou similar
4. Carregar viagem com ficha

**Resultado Esperado**:
- [ ] Botão **REDUZ de tamanho** conforme CSS mobile
- [ ] Botão continua **VISÍVEL** e **CLICÁVEL**
- [ ] Layout não quebra
- [ ] Campo Destino + Botão ficam alinhados

#### 8. Cenário 7: Performance
**Passos**:
1. Abrir DevTools → Performance
2. Iniciar gravação
3. Clicar em viagem com ficha
4. Parar gravação

**Resultado Esperado**:
- [ ] Tempo de resposta da função `exibirViagemExistente` < 100ms
- [ ] Não há recalculações de layout desnecessárias
- [ ] Botão aparece **imediatamente** ao carregar viagem

#### 9. Cenário 8: Múltiplas Aberturas
**Passos**:
1. Abrir viagem COM ficha → Verificar botão ATIVO
2. Fechar modal
3. Abrir viagem SEM ficha → Verificar botão BLOQUEADO
4. Fechar modal
5. Abrir nova viagem → Verificar botão OCULTO
6. Repetir 3x

**Resultado Esperado**:
- [ ] Botão sempre reflete o estado correto
- [ ] Não há "vazamento" de estado entre modais
- [ ] Event listeners não duplicam

#### 10. Cenário 9: Validação de Dados
**Passos**:
1. Inspecionar `objViagem` no console:
   ```javascript
   console.log("objViagem:", objViagem);
   console.log("temFichaVistoriaReal:", objViagem.temFichaVistoriaReal);
   ```
2. Verificar tipo de dado retornado

**Resultado Esperado**:
- [ ] `objViagem.temFichaVistoriaReal` existe
- [ ] Valor é `true`, `false`, `1`, `0`, `"true"` ou `"false"` (verificar conversão)
- [ ] Não é `null` ou `undefined` para viagens existentes

---

## 📚 DOCUMENTAÇÃO A ATUALIZAR

### Arquivos de Documentação

| Arquivo | O Que Atualizar |
|---------|-----------------|
| `Documentacao/JavaScript/exibe-viagem.md` | Adicionar seção sobre controle do botão de Ficha de Vistoria |
| `Documentacao/Pages/Agenda - Index.md` | Documentar botão `btnVisualizarFichaVistoria` |
| `Documentacao/Models/Cadastros/Viagem.md` | Explicar uso do campo `TemFichaVistoriaReal` |
| `Documentacao/Comentarios/AndamentoComentarios.md` | Marcar exibe-viagem.js como atualizado |
| `SUPER_PROMPT_BOTAO_FICHA_VISTORIA.md` | Atualizar status para CONCLUÍDO após implementação |

### Conteúdo a Adicionar na Documentação

#### Para `Documentacao/JavaScript/exibe-viagem.md`

```markdown
## Botão de Ficha de Vistoria

### Localização
Aparece ao lado do campo **Destino** quando uma viagem existente é carregada.

### Comportamento

| Situação | Visibilidade | Estado | Cor | Ação |
|----------|--------------|--------|-----|------|
| Nova viagem | Oculto | N/A | N/A | Não aparece |
| Viagem COM ficha real | Visível | Ativo | Laranja | Abre modal com ficha |
| Viagem SEM ficha real | Visível | Bloqueado | Cinza | Não faz nada |

### Campo do Banco
O botão é controlado pelo campo `TemFichaVistoriaReal` da tabela `Viagem`:
- `TRUE` (1): Viagem possui ficha de vistoria real digitalizada
- `FALSE` (0): Viagem não possui ficha ou possui apenas ficha padrão

### Código
Ver função `exibirViagemExistente()` em `exibe-viagem.js`, linha ~1265.

### Atualização
**Data**: 22/01/2026
**Motivo**: Implementar controle de visibilidade do botão baseado em `TemFichaVistoriaReal`
```

---

## 📏 REGRAS DE NEGÓCIO

### Regra 1: Visibilidade do Botão

**REGRA**: O botão de Ficha de Vistoria **SEMPRE deve aparecer** ao lado do campo Destino quando uma viagem **EXISTENTE** é carregada no modal de Agenda.

**Exceção**: Botão **NÃO aparece** ao criar nova viagem/agendamento.

### Regra 2: Estado do Botão

**REGRA**: O estado (ativo/bloqueado) do botão depende **EXCLUSIVAMENTE** do campo `TemFichaVistoriaReal` do banco de dados.

**Lógica**:
```
SE (TemFichaVistoriaReal == TRUE ou 1 ou "true")
    ENTÃO Botão ATIVO (laranja, clicável)
SENÃO
    Botão BLOQUEADO (cinza, não clicável)
FIM SE
```

### Regra 3: Sincronização com Banco de Dados

**REGRA**: O campo `TemFichaVistoriaReal` **DEVE ser atualizado automaticamente** sempre que:
- Uma ficha de vistoria for **ADICIONADA** a uma viagem → `TemFichaVistoriaReal = TRUE`
- Uma ficha de vistoria for **REMOVIDA** de uma viagem → `TemFichaVistoriaReal = FALSE`

**Implementação Sugerida**: Trigger SQL ou lógica na camada de serviço.

### Regra 4: Ficha Padrão (Amarelinha) vs Ficha Real

**REGRA**: Fichas de vistoria **PADRÃO** geradas automaticamente pelo sistema ("amarelinha") **NÃO devem** ser consideradas fichas reais.

**Distinção**:
- **Ficha Real**: Digitalização de ficha física preenchida manualmente (PDF, imagem)
- **Ficha Padrão**: Template amarelo gerado pelo sistema

**Critério**: Se `NoFichaVistoria == 0` ou `NULL`, considerar como **ficha padrão** (não real).

### Regra 5: Modal de Visualização

**REGRA**: Ao clicar no botão ATIVO, o modal **DEVE exibir APENAS** a Ficha de Vistoria da viagem corrente (não de outras viagens).

**Validação**: Verificar `ViagemId` no `dataset` do botão.

### Regra 6: Card de Ficha de Vistoria (Obsoleto)

**REGRA**: O **card antigo** de Ficha de Vistoria (que aparecia como seção separada) **NÃO deve ser excluído** do código, mas deve permanecer **OCULTO**.

**Motivo**: Pode ser reutilizado no futuro ou em outra funcionalidade.

### Regra 7: Retrocompatibilidade

**REGRA**: Viagens criadas **ANTES** da implementação do campo `TemFichaVistoriaReal` devem funcionar corretamente após execução do script de atualização.

**Validação**: Nenhuma viagem deve ficar com `TemFichaVistoriaReal = NULL` após o update.

### Regra 8: Tooltip Informativo

**REGRA**: O tooltip do botão **DEVE mudar** conforme o estado:
- Botão ATIVO: "Clique para visualizar a Ficha de Vistoria desta viagem"
- Botão BLOQUEADO: "Esta viagem não possui Ficha de Vistoria digitalizada"

---

## 🚦 CHECKLIST DE IMPLEMENTAÇÃO

### Fase 1: Banco de Dados
- [ ] **1.1** Fazer backup do banco de dados FrotiX
- [ ] **1.2** Executar Script 1 (Verificar Estado Atual)
- [ ] **1.3** Revisar resultado do diagnóstico
- [ ] **1.4** Executar Script 2 (Atualizar Registros)
- [ ] **1.5** Executar Script 3 (Validação)
- [ ] **1.6** Confirmar que não há registros com `TemFichaVistoriaReal = NULL`

### Fase 2: Código JavaScript
- [ ] **2.1** Abrir arquivo `exibe-viagem.js`
- [ ] **2.2** Localizar função `exibirViagemExistente()` (linha ~684)
- [ ] **2.3** Inserir código de controle do botão após linha 1260
- [ ] **2.4** Remover/comentar código antigo na função `configurarModalParaNovo()`
- [ ] **2.5** Verificar sintaxe JavaScript (sem erros)
- [ ] **2.6** Salvar arquivo

### Fase 3: CSS
- [ ] **3.1** Abrir arquivo `frotix.css`
- [ ] **3.2** Adicionar estilos `.btn-ficha-vistoria` e `.btn-ficha-vistoria-sem`
- [ ] **3.3** Testar responsividade (media query)
- [ ] **3.4** Salvar arquivo

### Fase 4: Build e Deploy
- [ ] **4.1** Executar build da aplicação (`dotnet build`)
- [ ] **4.2** Corrigir erros de compilação (se houver)
- [ ] **4.3** Executar aplicação localmente (`dotnet run`)
- [ ] **4.4** Verificar no navegador (limpar cache: Ctrl + F5)

### Fase 5: Testes
- [ ] **5.1** Executar Cenário 1 (Nova Viagem)
- [ ] **5.2** Executar Cenário 2 (Viagem COM Ficha)
- [ ] **5.3** Executar Cenário 3 (Viagem SEM Ficha)
- [ ] **5.4** Executar Cenário 4 (Clicar Botão Ativo)
- [ ] **5.5** Executar Cenário 5 (Clicar Botão Bloqueado)
- [ ] **5.6** Executar Cenário 6 (Responsividade)
- [ ] **5.7** Executar Cenário 7 (Performance)
- [ ] **5.8** Executar Cenário 8 (Múltiplas Aberturas)
- [ ] **5.9** Executar Cenário 9 (Validação de Dados)

### Fase 6: Documentação
- [ ] **6.1** Atualizar `exibe-viagem.md`
- [ ] **6.2** Atualizar `Agenda - Index.md`
- [ ] **6.3** Atualizar `Viagem.md`
- [ ] **6.4** Atualizar `AndamentoComentarios.md`
- [ ] **6.5** Marcar este super prompt como CONCLUÍDO

### Fase 7: Commit e Push
- [ ] **7.1** `git add` nos arquivos alterados
- [ ] **7.2** `git commit` com mensagem descritiva
- [ ] **7.3** `git push origin main`
- [ ] **7.4** Verificar no repositório remoto

---

## 🎬 MENSAGEM DE COMMIT SUGERIDA

```
feat: Implementa botão de Ficha de Vistoria na página de Agenda

- Adiciona controle de visibilidade do botão baseado em TemFichaVistoriaReal
- Botão aparece ao lado do campo Destino em viagens existentes
- Botão ativo (laranja) quando tem ficha real, bloqueado (cinza) quando não tem
- Executa script SQL para popular TemFichaVistoriaReal em registros existentes
- Adiciona estilos CSS para estados do botão (ativo/bloqueado)
- Remove lógica antiga que sempre escondia o botão
- Atualiza documentação (exibe-viagem.md, Agenda - Index.md, Viagem.md)

Arquivos alterados:
- wwwroot/js/agendamento/components/exibe-viagem.js
- wwwroot/css/frotix.css
- Scripts/UpdateTemFichaVistoriaReal.sql (NOVO)
- Documentacao/JavaScript/exibe-viagem.md
- Documentacao/Pages/Agenda - Index.md
- Documentacao/Models/Cadastros/Viagem.md

Resolves: #[número da issue, se houver]

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>
```

---

## 🔗 REFERÊNCIAS

### Arquivos Importantes
- [Models/Cadastros/Viagem.cs:246](Models/Cadastros/Viagem.cs#L246) → Campo `TemFichaVistoriaReal`
- [Pages/Agenda/Index.cshtml:1084](Pages/Agenda/Index.cshtml#L1084) → HTML do botão
- [exibe-viagem.js:416](wwwroot/js/agendamento/components/exibe-viagem.js#L416) → Lógica antiga (esconder botão)
- [exibe-viagem.js:684](wwwroot/js/agendamento/components/exibe-viagem.js#L684) → Função `exibirViagemExistente()`
- [exibe-viagem.js:4848](wwwroot/js/agendamento/components/exibe-viagem.js#L4848) → Event listener do botão

### Documentação Relacionada
- [Documentacao/JavaScript/exibe-viagem.md](Documentacao/JavaScript/exibe-viagem.md)
- [Documentacao/Models/Cadastros/Viagem.md](Documentacao/Models/Cadastros/Viagem.md)
- [RegrasDesenvolvimentoFrotiX.md](RegrasDesenvolvimentoFrotiX.md)

### Scripts SQL
- [Scripts/AddTemFichaVistoriaReal.sql](Scripts/AddTemFichaVistoriaReal.sql) → Adicionar coluna (JÁ EXECUTADO)
- `Scripts/UpdateTemFichaVistoriaReal.sql` → Atualizar registros (CRIAR E EXECUTAR)

---

## 📝 NOTAS ADICIONAIS

### Considerações de Performance
- O campo `TemFichaVistoriaReal` é do tipo `BIT` (1 byte), impacto mínimo no banco
- A verificação no JavaScript é simples (comparação booleana), sem overhead
- Modal de Ficha carrega imagem sob demanda (lazy loading), não afeta página principal

### Segurança
- Botão chama função `window.abrirModalFichaVistoria()` que valida `ViagemId` antes de buscar ficha
- API de Ficha deve validar permissões do usuário (não permitir acesso a fichas de outras unidades)

### Acessibilidade
- Botão tem `title` e `data-ejtip` para screen readers
- Tamanho mínimo de 44x44px para touch targets (WCAG 2.1)
- Contraste de cores atende WCAG AA

### Manutenibilidade
- Código bem documentado com comentários explicativos
- Uso de try-catch para tratamento de erros
- Logs no console para debugging

### Próximos Passos (Futuro)
1. Criar trigger SQL para atualizar `TemFichaVistoriaReal` automaticamente ao inserir/atualizar `FichaVistoria`
2. Adicionar indicador visual no calendário (ícone) para viagens com ficha real
3. Implementar pré-visualização (thumbnail) da ficha ao passar mouse sobre o botão

---

**FIM DO SUPER PROMPT**

📌 **Status**: AGUARDANDO IMPLEMENTAÇÃO
📅 **Criado em**: 22/01/2026
👤 **Criado por**: Claude Sonnet 4.5
🔄 **Última atualização**: 22/01/2026
