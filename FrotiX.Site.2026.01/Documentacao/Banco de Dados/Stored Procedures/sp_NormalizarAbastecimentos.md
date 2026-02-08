# sp_NormalizarAbastecimentos

## Código completo

```sql
CREATE PROCEDURE dbo.sp_NormalizarAbastecimentos
AS
BEGIN
    SET NOCOUNT ON
    
    DECLARE @TotalRegistros INT
    DECLARE @OutliersDetectados INT

    BEGIN TRY
        BEGIN TRANSACTION

        -- STEP 1: Calcular consumo original
        PRINT '  [STEP 1] Calculando consumo original...'
        
        UPDATE Abastecimento
        SET ConsumoCalculado = CAST(KmRodado AS FLOAT) / Litros
        WHERE Litros > 0 AND KmRodado > 0
        
        SELECT @TotalRegistros = @@ROWCOUNT
        PRINT '           ' + CAST(@TotalRegistros AS VARCHAR) + ' registros calculados'

        -- STEP 2: Calcular estatísticas e normalizar por veículo
        PRINT '  [STEP 2] Detectando outliers (método IQR)...'
        
        ;WITH EstatisticasVeiculo AS (
            SELECT DISTINCT
                VeiculoId,
                PERCENTILE_CONT(0.25) WITHIN GROUP (ORDER BY ConsumoCalculado) OVER (PARTITION BY VeiculoId) AS Q1,
                PERCENTILE_CONT(0.75) WITHIN GROUP (ORDER BY ConsumoCalculado) OVER (PARTITION BY VeiculoId) AS Q3,
                PERCENTILE_CONT(0.50) WITHIN GROUP (ORDER BY ConsumoCalculado) OVER (PARTITION BY VeiculoId) AS Mediana,
                PERCENTILE_CONT(0.50) WITHIN GROUP (ORDER BY KmRodado) OVER (PARTITION BY VeiculoId) AS MedianaKm,
                PERCENTILE_CONT(0.50) WITHIN GROUP (ORDER BY Litros) OVER (PARTITION BY VeiculoId) AS MedianaLitros
            FROM Abastecimento
            WHERE Litros > 0 AND KmRodado > 0 AND ConsumoCalculado IS NOT NULL
        ),
        LimitesVeiculo AS (
            SELECT 
                VeiculoId, Q1, Q3, Mediana, MedianaKm, MedianaLitros,
                -- IQR com limites de sanidade (3 a 30 km/l)
                CASE WHEN Q1 - 1.5 * (Q3 - Q1) < 3 THEN 3 ELSE Q1 - 1.5 * (Q3 - Q1) END AS LimiteInf,
                CASE WHEN Q3 + 1.5 * (Q3 - Q1) > 30 THEN 30 ELSE Q3 + 1.5 * (Q3 - Q1) END AS LimiteSup
            FROM EstatisticasVeiculo
        )
        UPDATE A
        SET 
            A.EhOutlier = CASE WHEN A.ConsumoCalculado < L.LimiteInf OR A.ConsumoCalculado > L.LimiteSup THEN 1 ELSE 0 END,
            A.KmRodadoNormalizado = CASE WHEN A.ConsumoCalculado < L.LimiteInf OR A.ConsumoCalculado > L.LimiteSup THEN CAST(L.MedianaKm AS INT) ELSE A.KmRodado END,
            A.LitrosNormalizado = CASE WHEN A.ConsumoCalculado < L.LimiteInf OR A.ConsumoCalculado > L.LimiteSup THEN L.MedianaLitros ELSE A.Litros END,
            A.ConsumoNormalizado = CASE WHEN A.ConsumoCalculado < L.LimiteInf OR A.ConsumoCalculado > L.LimiteSup THEN L.Mediana ELSE A.ConsumoCalculado END
        FROM Abastecimento A
        INNER JOIN LimitesVeiculo L ON A.VeiculoId = L.VeiculoId
        WHERE A.Litros > 0 AND A.KmRodado > 0 AND A.ConsumoCalculado IS NOT NULL

        SELECT @OutliersDetectados = COUNT(*) FROM Abastecimento WHERE EhOutlier = 1
        PRINT '           ' + CAST(@OutliersDetectados AS VARCHAR) + ' outliers detectados e normalizados'

        COMMIT TRANSACTION
        
        PRINT ''
        PRINT '  ✓ Normalização de abastecimentos concluída!'

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        RAISERROR(@ErrorMessage, 16, 1)
    END CATCH
END
```

## Explicação por blocos

- **Passo 1 – consumo calculado**: `ConsumoCalculado = KmRodado / Litros` para registros positivos; contabiliza quantos foram processados.
- **Estatísticas por veículo (CTE EstatisticasVeiculo)**: calcula Q1, Q3, mediana, mediana de km e litros por `VeiculoId`.
- **Limites IQR sanitizados (CTE LimitesVeiculo)**: aplica IQR com piso 3 km/l e teto 30 km/l para evitar falsos positivos.
- **Normalização**: marca outliers (`EhOutlier = 1`) e substitui km/litros/consumo por medianas do veículo quando fora dos limites.
- **Log/controle**: conta outliers tratados; toda operação é transacional com ROLLBACK em erro.
- **Impacto**: melhora a qualidade de dados de abastecimento antes de recalcular consumo de veículos, custos e estatísticas.


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
