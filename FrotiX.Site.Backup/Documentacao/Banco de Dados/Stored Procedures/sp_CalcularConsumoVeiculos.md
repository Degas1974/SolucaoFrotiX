# sp_CalcularConsumoVeiculos

## Código completo

```sql
CREATE PROCEDURE dbo.sp_CalcularConsumoVeiculos
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @VeiculosAtualizados INT = 0;
    DECLARE @TemTriggers BIT = 0;

    BEGIN TRY
        -- Verifica se as colunas necessárias existem
        IF NOT EXISTS (
            SELECT 1 FROM sys.columns 
            WHERE object_id = OBJECT_ID('Veiculo') AND name = 'Consumo'
        )
        BEGIN
            RAISERROR('Coluna Veiculo.Consumo não existe!', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (
            SELECT 1 FROM sys.columns 
            WHERE object_id = OBJECT_ID('Abastecimento') AND name IN ('Litros', 'KmRodado')
        )
        BEGIN
            RAISERROR('Colunas Abastecimento.Litros ou KmRodado não existem!', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        -- Verifica se existem triggers na tabela Veiculo
        IF EXISTS (
            SELECT 1 FROM sys.triggers 
            WHERE parent_id = OBJECT_ID('Veiculo') AND is_disabled = 0
        )
        BEGIN
            SET @TemTriggers = 1;
            PRINT '  [INFO] Desabilitando triggers em Veiculo para evitar recursão...';
            
            DECLARE @TriggerName NVARCHAR(128);
            DECLARE trigger_cursor CURSOR FOR
                SELECT name FROM sys.triggers 
                WHERE parent_id = OBJECT_ID('Veiculo') AND is_disabled = 0;
            
            OPEN trigger_cursor;
            FETCH NEXT FROM trigger_cursor INTO @TriggerName;
            
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC('ALTER TABLE Veiculo DISABLE TRIGGER ' + @TriggerName);
                PRINT '  [INFO] Trigger desabilitado: ' + @TriggerName;
                FETCH NEXT FROM trigger_cursor INTO @TriggerName;
            END
            
            CLOSE trigger_cursor;
            DEALLOCATE trigger_cursor;
        END

        -- Atualiza o consumo de cada veículo baseado na média de seus abastecimentos
        PRINT '  [EXEC] Calculando consumo médio por veículo...';
        
        UPDATE V
        SET V.Consumo = ROUND(Calc.ConsumoMedio, 2)
        FROM Veiculo V
        INNER JOIN (
            SELECT 
                VeiculoId,
                AVG(CAST(KmRodado AS FLOAT) / NULLIF(Litros, 0)) AS ConsumoMedio
            FROM Abastecimento
            WHERE Litros > 0 
              AND KmRodado > 0
            GROUP BY VeiculoId
            HAVING AVG(CAST(KmRodado AS FLOAT) / NULLIF(Litros, 0)) > 0
        ) Calc ON V.VeiculoId = Calc.VeiculoId;

        SET @VeiculosAtualizados = @@ROWCOUNT;

        -- Reabilita triggers se foram desabilitados
        IF @TemTriggers = 1
        BEGIN
            PRINT '  [INFO] Reabilitando triggers em Veiculo...';
            
            DECLARE trigger_cursor2 CURSOR FOR
                SELECT name FROM sys.triggers 
                WHERE parent_id = OBJECT_ID('Veiculo');
            
            OPEN trigger_cursor2;
            FETCH NEXT FROM trigger_cursor2 INTO @TriggerName;
            
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC('ALTER TABLE Veiculo ENABLE TRIGGER ' + @TriggerName);
                PRINT '  [INFO] Trigger reabilitado: ' + @TriggerName;
                FETCH NEXT FROM trigger_cursor2 INTO @TriggerName;
            END
            
            CLOSE trigger_cursor2;
            DEALLOCATE trigger_cursor2;
        END

        COMMIT TRANSACTION;

        PRINT '  [OK]   Cálculo de consumo concluído com sucesso!';
        PRINT '  [INFO] Total de veículos atualizados: ' + CAST(@VeiculosAtualizados AS VARCHAR(10));

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Tenta reabilitar triggers mesmo em caso de erro
        IF @TemTriggers = 1
        BEGIN
            BEGIN TRY
                PRINT '  [WARN] Tentando reabilitar triggers após erro...';
                
                DECLARE trigger_cursor3 CURSOR FOR
                    SELECT name FROM sys.triggers 
                    WHERE parent_id = OBJECT_ID('Veiculo');
                
                OPEN trigger_cursor3;
                FETCH NEXT FROM trigger_cursor3 INTO @TriggerName;
                
                WHILE @@FETCH_STATUS = 0
                BEGIN
                    EXEC('ALTER TABLE Veiculo ENABLE TRIGGER ' + @TriggerName);
                    FETCH NEXT FROM trigger_cursor3 INTO @TriggerName;
                END
                
                CLOSE trigger_cursor3;
                DEALLOCATE trigger_cursor3;
            END TRY
            BEGIN CATCH
                PRINT '  [ERROR] Falha ao reabilitar triggers!';
            END CATCH
        END

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
```

## Explicação por blocos

- **Pré-validação**: garante presença das colunas necessárias; aborta cedo com erro claro.
- **Gestão de triggers**: desabilita todas as triggers de `Veiculo` durante o update; reabilita no sucesso ou tenta reabilitar mesmo após erro.
- **Cálculo de consumo**: média de `KmRodado/Litros` por veículo em abastecimentos válidos; grava em `Veiculo.Consumo` com 2 casas decimais.
- **Transação e logs**: operações dentro de transação; prints informam progresso e total de veículos atualizados; ROLLBACK/RAISERROR em falhas.
- **Uso**: etapa 2 do job; roda após normalização de abastecimentos para usar dados limpos.


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
