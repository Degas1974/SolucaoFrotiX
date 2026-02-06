# sp_TratarNulosTabela

## Código completo

```sql
CREATE PROCEDURE dbo.sp_TratarNulosTabela
    @NomeTabela NVARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SQL NVARCHAR(MAX) = 'UPDATE ' + QUOTENAME(@NomeTabela) + ' SET ';
    DECLARE @Colunas NVARCHAR(MAX) = '';
    DECLARE @Where NVARCHAR(MAX) = ' WHERE ';
    
    SELECT @Colunas = @Colunas + QUOTENAME(c.COLUMN_NAME) + ' = ISNULL(' + QUOTENAME(c.COLUMN_NAME) + ', ' +
        CASE 
            WHEN c.DATA_TYPE IN ('varchar','nvarchar','char','nchar','text','ntext') THEN ''''''
            WHEN c.DATA_TYPE IN ('int','bigint','smallint','tinyint','decimal','numeric','float','real','money') THEN '0'
            WHEN c.DATA_TYPE = 'bit' THEN '1'
            ELSE 'NULL'
        END + '), '
    FROM INFORMATION_SCHEMA.COLUMNS c
    WHERE c.TABLE_NAME = @NomeTabela
      AND c.TABLE_SCHEMA = 'dbo'
      AND c.IS_NULLABLE = 'YES'
      AND c.DATA_TYPE IN ('varchar','nvarchar','char','nchar','text','ntext',
                          'int','bigint','smallint','tinyint','decimal','numeric','float','real','money',
                          'bit')
      -- Exclui colunas que são FK
      AND NOT EXISTS (
          SELECT 1 FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
          INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc 
              ON kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
          WHERE tc.CONSTRAINT_TYPE = 'FOREIGN KEY'
            AND kcu.TABLE_NAME = c.TABLE_NAME
            AND kcu.COLUMN_NAME = c.COLUMN_NAME
      )
      -- Exclui colunas com CHECK constraints
      AND NOT EXISTS (
          SELECT 1 FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
          INNER JOIN INFORMATION_SCHEMA.CHECK_CONSTRAINTS cc 
              ON ccu.CONSTRAINT_NAME = cc.CONSTRAINT_NAME
          WHERE ccu.TABLE_NAME = c.TABLE_NAME
            AND ccu.COLUMN_NAME = c.COLUMN_NAME
      );
    
    SELECT @Where = @Where + QUOTENAME(c.COLUMN_NAME) + ' IS NULL OR '
    FROM INFORMATION_SCHEMA.COLUMNS c
    WHERE c.TABLE_NAME = @NomeTabela
      AND c.TABLE_SCHEMA = 'dbo'
      AND c.IS_NULLABLE = 'YES'
      AND c.DATA_TYPE IN ('varchar','nvarchar','char','nchar','text','ntext',
                          'int','bigint','smallint','tinyint','decimal','numeric','float','real','money',
                          'bit')
      AND NOT EXISTS (
          SELECT 1 FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
          INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc 
              ON kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
          WHERE tc.CONSTRAINT_TYPE = 'FOREIGN KEY'
            AND kcu.TABLE_NAME = c.TABLE_NAME
            AND kcu.COLUMN_NAME = c.COLUMN_NAME
      )
      AND NOT EXISTS (
          SELECT 1 FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
          INNER JOIN INFORMATION_SCHEMA.CHECK_CONSTRAINTS cc 
              ON ccu.CONSTRAINT_NAME = cc.CONSTRAINT_NAME
          WHERE ccu.TABLE_NAME = c.TABLE_NAME
            AND ccu.COLUMN_NAME = c.COLUMN_NAME
      );
    
    IF LEN(@Colunas) > 1
    BEGIN
        SET @Colunas = LEFT(@Colunas, LEN(@Colunas) - 1);
        SET @Where = LEFT(@Where, LEN(@Where) - 3);
        SET @SQL = @SQL + @Colunas + @Where;
        EXEC sp_executesql @SQL;
    END
END
```

## Explicação por blocos

- **SQL dinâmico seguro**: usa `QUOTENAME` em tabela/colunas para evitar injeção.
- **Seleção de colunas elegíveis**: só nulas, tipos texto/numérico/bit, exclui FKs e colunas com CHECK.
- **Defaults aplicados**: string vazia, 0 numérico, 1 para bit, NULL para demais.
- **WHERE dinâmico**: limita a linhas com nulos nas colunas elegíveis.
- **Uso**: saneamento manual controlado; recomendável rodar com cautela (pode mascarar falta de dados).


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:
- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**: 
- OtimizaÃ§Ã£o de memÃ³ria e performance
- Tracking seletivo (apenas quando necessÃ¡rio para Update/Delete)
- PadrÃ£o mais limpo e explÃ­cito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seÃ§Ã£o 4.2)

**Impacto**: 
- Melhoria de performance em operaÃ§Ãµes de leitura (usa AsNoTracking por padrÃ£o)
- Tracking correto em operaÃ§Ãµes de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

**VersÃ£o**: Incremento de patch
