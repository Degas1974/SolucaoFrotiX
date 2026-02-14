# 🔍 RELATÓRIO DE ANÁLISE: Limpeza_Origem_Destino.sql

**Data:** 13/02/2026
**Analista:** Claude Sonnet 4.5
**Banco de Dados:** FrotiX (SQL Server 2022)
**Tabela Afetada:** `dbo.Viagem`

---

## 📊 RESUMO EXECUTIVO

O script `Limpeza_Origem_Destino.sql` foi criado para padronizar e unificar valores duplicados nos campos `Origem` e `Destino` da tabela `Viagem`. Após análise contra a estrutura do banco em `Frotix.sql`, foram identificados **4 problemas críticos/médios** que precisam ser corrigidos antes da execução.

### ✅ **Recomendação Geral**

**NÃO EXECUTE O SCRIPT SEM CORREÇÕES**. Execute primeiro o script de verificação `VERIFICAR_TAMANHO_ORIGEM_DESTINO.sql` e corrija os problemas listados abaixo.

---

## 🔧 ESTRUTURA ATUAL DA TABELA VIAGEM

### Definição dos Campos (Frotix.sql:7373-7374)

```sql
Origem  varchar(max) NULL CONSTRAINT DF_Viagem_Origem DEFAULT (''),
Destino varchar(max) NULL CONSTRAINT DF_Viagem_Destino DEFAULT (''),
```

**Especificações:**
- **Tipo:** `varchar(max)` (ASCII, tamanho ilimitado)
- **Nullable:** `NULL`
- **Default:** `''` (string vazia)
- **Collation:** Padrão do banco (provavelmente `Latin1_General_CI_AS`)

---

## ⚠️ PROBLEMAS IDENTIFICADOS

### 🔴 **PROBLEMA 1: Incompatibilidade de Tipo de Dados** (CRÍTICO)

**Localização:** Linha 172-175

**Problema:**
```sql
CREATE TABLE #MapeamentoOrigemDestino (
    ValorAntigo NVARCHAR(500) NOT NULL,   -- ❌ ERRADO
    ValorCanonico NVARCHAR(500) NOT NULL, -- ❌ ERRADO
    Razao NVARCHAR(200) NOT NULL
);
```

**Banco Real:** `varchar(max)`
**Script:** `NVARCHAR(500)`

**Impactos:**
1. ❌ **Truncamento:** Valores com mais de 500 caracteres serão cortados
2. ❌ **Collation:** Comparação `nvarchar` x `varchar` pode ter resultados inesperados
3. ❌ **Unicode desnecessário:** Uso de `N'string'` em todo o script aumenta memória
4. ❌ **Performance:** Conversão implícita em cada comparação

**Correção:**
```sql
CREATE TABLE #MapeamentoOrigemDestino (
    ValorAntigo VARCHAR(MAX) NOT NULL,   -- ✅ CORRETO
    ValorCanonico VARCHAR(MAX) NOT NULL, -- ✅ CORRETO
    Razao VARCHAR(200) NOT NULL
);
```

**Ação Adicional:**
- Remover prefixo `N` de todas as strings literais (ex: `N'Aeroporto'` → `'Aeroporto'`)
- Há **196 ocorrências** de `N'` no script (linhas 198-470)

---

### 🟡 **PROBLEMA 2: Função Permanente no Schema DBO** (MÉDIO)

**Localização:** Linha 719-778

**Problema:**
```sql
CREATE FUNCTION dbo.LevenshteinDistance(@string1 NVARCHAR(MAX), @string2 NVARCHAR(MAX))
RETURNS INT
AS
BEGIN
    ...
END;
```

**Impactos:**
1. ⚠️ Função criada **permanentemente** no schema `dbo`
2. ⚠️ Não verifica se já existe antes de criar (erro se existir)
3. ⚠️ Linha 986 tenta `DROP FUNCTION dbo.LevenshteinDistance` mas falha se houver erro antes

**Correção:**
```sql
-- Adicionar verificação ANTES de criar
IF OBJECT_ID('dbo.LevenshteinDistance', 'FN') IS NOT NULL
    DROP FUNCTION dbo.LevenshteinDistance;
GO

CREATE FUNCTION dbo.LevenshteinDistance(@string1 VARCHAR(MAX), @string2 VARCHAR(MAX))
RETURNS INT
AS
BEGIN
    ...
END;
GO
```

**Correção no Final (linha 986):**
```sql
-- Adicionar verificação
IF OBJECT_ID('dbo.LevenshteinDistance', 'FN') IS NOT NULL
    DROP FUNCTION dbo.LevenshteinDistance;
```

---

### 🔴 **PROBLEMA 3: Coluna Inexistente** (ERRO FATAL)

**Localização:** Linha 940

**Problema:**
```sql
INSERT INTO #MapeamentoOrigemDestino (ValorAntigo, ValorCanonico, Observacao)  -- ❌ Coluna "Observacao" não existe!
SELECT
    ValorOriginal,
    ValorCanonico,
    N'Fuzzy Match (' + CAST(SimilarityPercent AS NVARCHAR(10)) + N'% similaridade)'
FROM #BestMatches;
```

**Impacto:**
❌ Script irá **FALHAR** nesta linha com erro:
```
Msg 207, Level 16, State 1, Line 940
Invalid column name 'Observacao'.
```

**Causa:**
A tabela `#MapeamentoOrigemDestino` foi criada com coluna `Razao` (linha 175), mas o INSERT usa `Observacao`.

**Correção:**
```sql
INSERT INTO #MapeamentoOrigemDestino (ValorAntigo, ValorCanonico, Razao)  -- ✅ CORRETO
SELECT
    ValorOriginal,
    ValorCanonico,
    'Fuzzy Match (' + CAST(SimilarityPercent AS VARCHAR(10)) + '% similaridade)'  -- Remover N
FROM #BestMatches;
```

---

### 🟡 **PROBLEMA 4: Performance com Cursores** (MÉDIO)

**Localização:** Linhas 799-914 (Fuzzy Matching)

**Problema:**
```sql
-- Cursor 1: Todos os valores de Origem não mapeados
DECLARE origem_cursor CURSOR FOR ...
OPEN origem_cursor;
FETCH NEXT ...

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Cursor 2: Todos os valores canônicos (PARA CADA VALOR DE ORIGEM!)
    DECLARE canonico_cursor CURSOR FOR ...
    OPEN canonico_cursor;
    FETCH NEXT ...

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Calcular Levenshtein para CADA COMBINAÇÃO
        SET @LevenshteinDist = dbo.LevenshteinDistance(...);
        ...
    END
    ...
END
```

**Impactos:**
1. 🐌 **Complexidade O(n²):** Para cada valor não mapeado, compara com TODOS os canônicos
2. 🐌 **Tabelas grandes:** Se houver 1.000 valores não mapeados e 200 canônicos = 200.000 comparações
3. 🐌 **Levenshtein é custoso:** Algoritmo de distância de edição é O(m×n) por comparação
4. 🐌 **Estimativa:** Pode levar **horas** para executar em produção

**Recomendação:**
- ✅ Executar em **horário de baixo uso** (madrugada/final de semana)
- ✅ Monitorar progresso via `PRINT` statements
- ✅ Considerar desabilitar temporariamente o fuzzy matching se houver muitos registros

**Alternativa (se houver problemas de performance):**
- Comentar as linhas 703-991 (Fase 3 - Fuzzy Matching) e executar apenas a limpeza manual

---

## ✅ PONTOS POSITIVOS DO SCRIPT

### 1. **Backup Automático** ✅
```sql
SELECT ViagemId, Origem AS OrigemOriginal, Destino AS DestinoOriginal
INTO dbo.Viagem_Backup_OrigemDestino
FROM dbo.Viagem;
```
- Cria tabela de backup completa antes de qualquer alteração
- Permite rollback manual se necessário

### 2. **Transação com Try/Catch** ✅
```sql
BEGIN TRANSACTION;
BEGIN TRY
    UPDATE ...
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT ERROR_MESSAGE();
END CATCH;
```
- Garante atomicidade: tudo ou nada
- Rollback automático em caso de erro

### 3. **Correção de Encoding UTF-8/Latin1** ✅ (FASE 1.5)
- 30 substituições de caracteres mal interpretados
- Exemplos: `Ã£` → `ã`, `Ã§` → `ç`, `Ã©` → `é`
- **Muito importante** para dados com acentuação

### 4. **Mapeamento Abrangente** ✅
- 196 mapeamentos configurados
- Cobertura de variações de case, typos, espaços extras, acentuação

### 5. **Auto-Fix de Duplicatas** ✅
- Sistema inteligente de priorização baseado em ortografia correta
- Remove duplicatas case-insensitive automaticamente

### 6. **Estatísticas Detalhadas** ✅
- Mostra redução de valores únicos com percentual
- Tabela de resultados em aba separada (SSMS)

### 7. **Fuzzy Matching com Levenshtein** ✅ (se houver performance)
- Threshold de 85% de similaridade
- Adiciona automaticamente novos mapeamentos

---

## 📋 SCRIPT DE VERIFICAÇÃO PRÉ-EXECUÇÃO

Foi criado o script **`VERIFICAR_TAMANHO_ORIGEM_DESTINO.sql`** que verifica:

1. ✅ Valores com mais de 500 caracteres (truncamento)
2. ✅ Estatísticas gerais (total de viagens, valores únicos, comprimento máximo)
3. ✅ Tipo de dados dos campos (confirmação de `varchar(max)`)
4. ✅ Emite conclusão APROVAR/REPROVAR execução

**Executar ANTES do script de limpeza:**
```sql
-- No SSMS:
:r "C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Scripts\VERIFICAR_TAMANHO_ORIGEM_DESTINO.sql"
GO
```

---

## 🔧 CORREÇÕES NECESSÁRIAS

### Checklist de Correções (Arquivo: Limpeza_Origem_Destino.sql)

- [ ] **Linha 173-175:** Alterar `NVARCHAR(500)` → `VARCHAR(MAX)`
- [ ] **Linhas 198-470:** Remover prefixo `N` de todas as strings literais (196 ocorrências)
- [ ] **Linha 719:** Adicionar `IF OBJECT_ID ... DROP FUNCTION` antes de `CREATE FUNCTION`
- [ ] **Linha 719:** Alterar parâmetros de `NVARCHAR(MAX)` → `VARCHAR(MAX)`
- [ ] **Linha 940:** Alterar `Observacao` → `Razao`
- [ ] **Linha 940:** Remover prefixo `N` da string de fuzzy match
- [ ] **Linha 986:** Adicionar `IF OBJECT_ID ... DROP FUNCTION` antes do DROP
- [ ] **Linha 782-787:** Alterar `NVARCHAR(255)` → `VARCHAR(MAX)` em `#FuzzyCandidates`
- [ ] **Linha 917-922:** Alterar `NVARCHAR(255)` → `VARCHAR(MAX)` em `#BestMatches`

### Script Corrigido (Principais Trechos)

#### Trecho 1: Tabela de Mapeamento
```sql
-- ANTES (linha 172-175)
CREATE TABLE #MapeamentoOrigemDestino (
    ValorAntigo NVARCHAR(500) NOT NULL,
    ValorCanonico NVARCHAR(500) NOT NULL,
    Razao NVARCHAR(200) NOT NULL
);

-- DEPOIS
CREATE TABLE #MapeamentoOrigemDestino (
    ValorAntigo VARCHAR(MAX) NOT NULL,
    ValorCanonico VARCHAR(MAX) NOT NULL,
    Razao VARCHAR(200) NOT NULL
);
```

#### Trecho 2: Mapeamentos (exemplo)
```sql
-- ANTES (linha 199)
(N' Aeroporto ', N'Aeroporto', N'Espaços extras'),

-- DEPOIS
(' Aeroporto ', 'Aeroporto', 'Espaços extras'),
```

#### Trecho 3: Função Levenshtein
```sql
-- ANTES (linha 719)
CREATE FUNCTION dbo.LevenshteinDistance(@string1 NVARCHAR(MAX), @string2 NVARCHAR(MAX))

-- DEPOIS
IF OBJECT_ID('dbo.LevenshteinDistance', 'FN') IS NOT NULL
    DROP FUNCTION dbo.LevenshteinDistance;
GO

CREATE FUNCTION dbo.LevenshteinDistance(@string1 VARCHAR(MAX), @string2 VARCHAR(MAX))
```

#### Trecho 4: Fuzzy Insert
```sql
-- ANTES (linha 940)
INSERT INTO #MapeamentoOrigemDestino (ValorAntigo, ValorCanonico, Observacao)

-- DEPOIS
INSERT INTO #MapeamentoOrigemDestino (ValorAntigo, ValorCanonico, Razao)
```

---

## 📊 ESTIMATIVA DE IMPACTO

### Dados Estimados (baseado em MEMORY.md)

- **Tabela Viagem:** ~100 colunas, ~40 índices
- **Registros estimados:** Desconhecido (executar verificação)
- **Valores únicos (antes):** A ser determinado pelo script de verificação

### Tempo de Execução Estimado

| Fase | Operação | Tempo Estimado |
|------|----------|----------------|
| 1 | Backup + Estatísticas | 5-30 seg |
| 1.5 | Correção de Encoding | 10-60 seg |
| 2 | Criar Mapeamentos | < 1 seg |
| 3 | **Fuzzy Matching** | **5 min - 2 horas** ⚠️ |
| 4 | Executar UPDATEs | 10-120 seg |
| 5 | Estatísticas Finais | < 5 seg |
| **TOTAL** | **6 min - 2h 30min** | |

**Nota:** Fase 3 (Fuzzy Matching) é a mais lenta devido aos cursores.

---

## 🚀 PROTOCOLO DE EXECUÇÃO RECOMENDADO

### Fase 1: Preparação (OBRIGATÓRIA)

1. ✅ **Backup manual do banco completo**
   ```sql
   BACKUP DATABASE Frotix TO DISK = 'C:\Backups\Frotix_Antes_Limpeza_OrigemDestino.bak';
   ```

2. ✅ **Executar script de verificação**
   ```sql
   :r "VERIFICAR_TAMANHO_ORIGEM_DESTINO.sql"
   GO
   ```

3. ✅ **Aplicar correções no script** (checklist acima)

4. ✅ **Revisar mapeamentos** (linhas 198-470)
   - Verificar se fazem sentido para seu contexto
   - Adicionar/remover mapeamentos conforme necessário

### Fase 2: Teste em Ambiente de Desenvolvimento (RECOMENDADO)

1. ✅ Restaurar backup em banco de DEV/QA
2. ✅ Executar script corrigido
3. ✅ Validar resultados
4. ✅ Verificar performance

### Fase 3: Execução em Produção

1. ✅ **Escolher horário de baixo uso** (ex: domingo 03:00)
2. ✅ **Monitorar execução** (SSMS aberto com Messages)
3. ✅ **Validar resultado** após conclusão
4. ✅ **Manter backup por 7 dias** antes de remover

### Fase 4: Validação Pós-Execução

```sql
-- 1. Verificar redução de valores únicos
SELECT COUNT(DISTINCT Origem) AS OrigemUnicos,
       COUNT(DISTINCT Destino) AS DestinoUnicos
FROM dbo.Viagem;

-- 2. Verificar se há valores ainda problemáticos
SELECT TOP 20 Origem, COUNT(*) AS Qtd
FROM dbo.Viagem
WHERE Origem IS NOT NULL AND Origem <> ''
GROUP BY Origem
ORDER BY COUNT(*) DESC;

SELECT TOP 20 Destino, COUNT(*) AS Qtd
FROM dbo.Viagem
WHERE Destino IS NOT NULL AND Destino <> ''
GROUP BY Destino
ORDER BY COUNT(*) DESC;

-- 3. Comparar com backup
SELECT 'Origem' AS Campo,
       COUNT(*) AS DiferencasEncontradas
FROM dbo.Viagem v
INNER JOIN dbo.Viagem_Backup_OrigemDestino b ON v.ViagemId = b.ViagemId
WHERE v.Origem <> b.OrigemOriginal

UNION ALL

SELECT 'Destino',
       COUNT(*)
FROM dbo.Viagem v
INNER JOIN dbo.Viagem_Backup_OrigemDestino b ON v.ViagemId = b.ViagemId
WHERE v.Destino <> b.DestinoOriginal;
```

---

## 🔄 PLANO DE ROLLBACK

### Rollback Automático (em caso de erro)

✅ Já implementado no script (linha 1049-1069) via `TRY/CATCH`

### Rollback Manual (se necessário após conclusão)

```sql
-- Verificar se backup existe
IF OBJECT_ID('dbo.Viagem_Backup_OrigemDestino', 'U') IS NULL
BEGIN
    PRINT '❌ ERRO: Backup não encontrado!';
    RETURN;
END

-- Restaurar valores originais
BEGIN TRANSACTION;

UPDATE v
SET v.Origem = b.OrigemOriginal,
    v.Destino = b.DestinoOriginal
FROM dbo.Viagem v
INNER JOIN dbo.Viagem_Backup_OrigemDestino b ON v.ViagemId = b.ViagemId
WHERE v.Origem <> b.OrigemOriginal
   OR v.Destino <> b.DestinoOriginal;

DECLARE @Restaurados INT = @@ROWCOUNT;

PRINT '✅ Rollback concluído: ' + CAST(@Restaurados AS VARCHAR) + ' registros restaurados.';

COMMIT TRANSACTION;
GO

-- Opcional: Remover backup após confirmação
-- DROP TABLE dbo.Viagem_Backup_OrigemDestino;
```

---

## 📝 CONCLUSÃO

### Status do Script

| Aspecto | Avaliação | Observação |
|---------|-----------|------------|
| **Lógica geral** | ✅ ÓTIMA | Abordagem bem estruturada e completa |
| **Segurança** | ✅ BOA | Backup + transação + try/catch |
| **Correções** | 🟢 EXCELENTES | Encoding + mapeamentos + fuzzy match |
| **Compatibilidade** | 🔴 **PROBLEMAS** | 4 erros críticos/médios a corrigir |
| **Performance** | 🟡 ACEITÁVEL | Cursors são lentos, mas funcionais |
| **Documentação** | ✅ ÓTIMA | Comentários detalhados e claros |

### Recomendação Final

✅ **APROVAR COM RESSALVAS**

O script é **bem elaborado e seguro**, mas precisa de **correções obrigatórias** antes da execução:

1. 🔴 **OBRIGATÓRIO:** Corrigir `NVARCHAR(500)` → `VARCHAR(MAX)` (Problema 1)
2. 🔴 **OBRIGATÓRIO:** Corrigir `Observacao` → `Razao` (Problema 3)
3. 🟡 **RECOMENDADO:** Adicionar verificação `IF OBJECT_ID` para função (Problema 2)
4. 🟡 **RECOMENDADO:** Executar em horário de baixo uso (Problema 4)

### Próximos Passos

1. ✅ Executar `VERIFICAR_TAMANHO_ORIGEM_DESTINO.sql`
2. ✅ Aplicar correções no script
3. ✅ Testar em ambiente de DEV/QA
4. ✅ Agendar execução em produção (horário de baixo uso)
5. ✅ Validar resultados pós-execução

---

**Documento gerado por:** Claude Sonnet 4.5 (FrotiX Team)
**Data:** 13/02/2026
**Versão:** 1.0
