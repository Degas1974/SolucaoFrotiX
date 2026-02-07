# SUPER PROMPT — Corrigir PRIMARY KEY na tabela Fornecedor (SQL Server 2022)

## Contexto
Tenho um banco SQL Server 2022 (v16.00.1165) chamado "Frotix". Preciso converter a coluna `FornecedorId` da tabela `Fornecedor` de **NULL + UNIQUE INDEX** para **NOT NULL + PRIMARY KEY**.

## Definição atual da tabela (extraída do schema dump)

```sql
CREATE TABLE dbo.Fornecedor (
  FornecedorId uniqueidentifier NULL DEFAULT (newid()),
  DescricaoFornecedor varchar(100) NULL,
  CNPJ varchar(50) NULL,
  Endereco varchar(150) NULL,
  Contato01 varchar(100) NULL,
  Telefone01 varchar(50) NULL,
  Contato02 varchar(100) NULL,
  Telefone02 varchar(50) NULL,
  Status bit NULL
)
ON [PRIMARY]
```

### Índice único existente (NÃO é PK, é um UNIQUE INDEX):
```sql
CREATE UNIQUE INDEX KEY_Fornecedor_FornecedorId
  ON dbo.Fornecedor (FornecedorId)
  ON [PRIMARY]
```

### FKs de OUTRAS tabelas que REFERENCIAM Fornecedor.FornecedorId:
```sql
-- 1) Contrato → Fornecedor
ALTER TABLE dbo.Contrato
  ADD CONSTRAINT FK_Contrato_Fornecedor
  FOREIGN KEY (FornecedorId) REFERENCES dbo.Fornecedor (FornecedorId)

-- 2) AtaRegistroPrecos → Fornecedor
ALTER TABLE dbo.AtaRegistroPrecos
  ADD CONSTRAINT FK_AtaRegistroPrecos_Fornecedor
  FOREIGN KEY (FornecedorId) REFERENCES dbo.Fornecedor (FornecedorId)
```

### Dados existentes:
- A tabela NÃO possui nenhum registro com FornecedorId NULL (já verificado).
- A coluna é definida como NULL mas todos os registros têm valor preenchido.

## O que preciso fazer

1. Converter `FornecedorId` de `NULL` para `NOT NULL`
2. Remover o `UNIQUE INDEX` e criar uma `PRIMARY KEY CLUSTERED` no lugar
3. Manter o `DEFAULT (newid())` funcionando
4. Manter as FKs de Contrato e AtaRegistroPrecos intactas (ou recriar)

## O que já tentei (3 iterações, todas falharam com o MESMO erro)

### Erro em TODAS as tentativas:
```
Error (linha_do_ADD_CONSTRAINT): 8111: Cannot define PRIMARY KEY constraint on nullable column in table 'Fornecedor'.
Error (mesma_linha): 1750: Could not create constraint or index. See previous errors.
```

O erro SEMPRE aparece na linha do `ALTER TABLE ... ADD CONSTRAINT PK_... PRIMARY KEY`, indicando que o `ALTER COLUMN ... NOT NULL` da linha anterior NÃO está funcionando.

### Tentativa 1 — Abordagem simples:
```sql
-- Drop unique index
DROP INDEX KEY_Fornecedor_FornecedorId ON dbo.Fornecedor;
-- Alter column
ALTER TABLE dbo.Fornecedor ALTER COLUMN FornecedorId uniqueidentifier NOT NULL;
-- Add PK
ALTER TABLE dbo.Fornecedor ADD CONSTRAINT PK_Fornecedor_FornecedorId PRIMARY KEY CLUSTERED (FornecedorId);
```
**Resultado:** Erro 8111 no ADD CONSTRAINT. O ALTER COLUMN não teve efeito.

### Tentativa 2 — Removendo FKs externas primeiro:
Adicionei SQL dinâmico para dropar FK_Contrato_Fornecedor e FK_AtaRegistroPrecos_Fornecedor antes de alterar a coluna. Também tratei o UNIQUE INDEX como possível UNIQUE CONSTRAINT (checando sys.key_constraints).
**Resultado:** Mesmo erro 8111.

### Tentativa 3 — Removendo FKs + DEFAULT constraint:
Descobri que a coluna tem `DEFAULT (newid())`. Adicionei passo para dropar o DEFAULT anônimo via `sys.default_constraints` antes do ALTER COLUMN.
**Resultado:** Mesmo erro 8111. O ALTER COLUMN continua não transformando a coluna em NOT NULL.

### Script atual completo (que falha):
```sql
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;

    -- Verificar se há NULLs
    IF EXISTS (SELECT 1 FROM dbo.Fornecedor WHERE FornecedorId IS NULL)
        RAISERROR('Existem NULLs', 16, 1);

    -- PASSO 1: Remover FKs externas
    DECLARE @fkDrop NVARCHAR(MAX) = N'';
    DECLARE @fkRecreate NVARCHAR(MAX) = N'';
    SELECT
        @fkDrop = @fkDrop + 'ALTER TABLE ' + QUOTENAME(SCHEMA_NAME(fk.schema_id)) + '.' + QUOTENAME(OBJECT_NAME(fk.parent_object_id))
            + ' DROP CONSTRAINT ' + QUOTENAME(fk.name) + '; ',
        @fkRecreate = @fkRecreate + 'ALTER TABLE ' + QUOTENAME(SCHEMA_NAME(fk.schema_id)) + '.' + QUOTENAME(OBJECT_NAME(fk.parent_object_id))
            + ' WITH NOCHECK ADD CONSTRAINT ' + QUOTENAME(fk.name) + ' FOREIGN KEY ('
            + COL_NAME(fkc.parent_object_id, fkc.parent_column_id) + ') REFERENCES '
            + QUOTENAME(SCHEMA_NAME(fk.schema_id)) + '.' + QUOTENAME(OBJECT_NAME(fk.referenced_object_id))
            + '(' + COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) + '); '
    FROM sys.foreign_keys fk
    JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
    WHERE fk.referenced_object_id = OBJECT_ID('dbo.Fornecedor');
    IF LEN(@fkDrop) > 0 EXEC sp_executesql @fkDrop;

    -- PASSO 2: Remover DEFAULT constraint
    DECLARE @defName NVARCHAR(256);
    SELECT @defName = dc.name
    FROM sys.default_constraints dc
    WHERE dc.parent_object_id = OBJECT_ID('dbo.Fornecedor')
      AND dc.parent_column_id = COLUMNPROPERTY(OBJECT_ID('dbo.Fornecedor'), 'FornecedorId', 'ColumnId');
    IF @defName IS NOT NULL
    BEGIN
        DECLARE @dropDef NVARCHAR(500) = 'ALTER TABLE dbo.Fornecedor DROP CONSTRAINT [' + @defName + ']';
        EXEC sp_executesql @dropDef;
    END

    -- PASSO 3: Remover unique index/constraint
    IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'KEY_Fornecedor_FornecedorId' AND parent_object_id = OBJECT_ID('dbo.Fornecedor'))
        ALTER TABLE dbo.Fornecedor DROP CONSTRAINT KEY_Fornecedor_FornecedorId;
    ELSE IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'KEY_Fornecedor_FornecedorId' AND object_id = OBJECT_ID('dbo.Fornecedor'))
        DROP INDEX KEY_Fornecedor_FornecedorId ON dbo.Fornecedor;

    -- PASSO 4: ALTER COLUMN para NOT NULL  <<<--- ESTE NÃO FUNCIONA
    ALTER TABLE dbo.Fornecedor ALTER COLUMN FornecedorId uniqueidentifier NOT NULL;

    -- PASSO 5: Adicionar PK + DEFAULT  <<<--- ERRO 8111 AQUI (coluna ainda é NULL)
    ALTER TABLE dbo.Fornecedor ADD
        CONSTRAINT PK_Fornecedor_FornecedorId PRIMARY KEY CLUSTERED (FornecedorId),
        CONSTRAINT DF_Fornecedor_FornecedorId DEFAULT (newid()) FOR FornecedorId;

    -- PASSO 6: Recriar FKs
    IF LEN(@fkRecreate) > 0 EXEC sp_executesql @fkRecreate;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH
```

## Hipóteses não testadas

1. **ALTER COLUMN pode estar falhando silenciosamente** — talvez haja outra dependência na coluna (CHECK constraint, computed column, statistics, etc.) que impede a alteração mas não gera erro com XACT_ABORT + TRY/CATCH
2. **COLUMNPROPERTY pode estar retornando NULL** — fazendo o lookup do DEFAULT falhar, e o DEFAULT não ser removido. Sem remover o DEFAULT, ALTER COLUMN falha
3. **O UNIQUE INDEX pode ter dependências que não estamos vendo** — talvez statistics auto-geradas
4. **Problema com transação** — talvez o ALTER COLUMN DDL dentro de uma transação explícita + XACT_ABORT tenha algum comportamento não-padrão

## O que preciso de você

1. **Diagnóstico:** Monte um script de diagnóstico que eu possa rodar ANTES da correção para ver exatamente o que existe na coluna FornecedorId (constraints, indexes, defaults, statistics, dependências).

2. **Correção robusta:** Monte o script corrigido que funcione, levando em conta todos os cenários possíveis. Se possível, adicione PRINTs de diagnóstico em cada passo para identificar exatamente onde está falhando.

3. **O script precisa estar dentro de uma transação com ROLLBACK total** — se qualquer passo falhar, nada deve ser alterado.

## Queries úteis para diagnóstico

```sql
-- Ver todas as dependências na coluna FornecedorId
SELECT * FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('dbo.Fornecedor');
SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Fornecedor');
SELECT * FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID('dbo.Fornecedor');
SELECT * FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('dbo.Fornecedor');
SELECT * FROM sys.stats WHERE object_id = OBJECT_ID('dbo.Fornecedor');
SELECT * FROM sys.foreign_keys WHERE referenced_object_id = OBJECT_ID('dbo.Fornecedor');
SELECT c.name, c.is_nullable, c.column_id
FROM sys.columns c WHERE c.object_id = OBJECT_ID('dbo.Fornecedor') AND c.name = 'FornecedorId';

-- Verificar se COLUMNPROPERTY funciona
SELECT COLUMNPROPERTY(OBJECT_ID('dbo.Fornecedor'), 'FornecedorId', 'ColumnId') AS ColumnId;
```
