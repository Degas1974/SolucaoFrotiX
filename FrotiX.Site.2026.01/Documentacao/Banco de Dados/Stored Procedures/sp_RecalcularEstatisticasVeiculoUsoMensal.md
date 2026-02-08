# sp_RecalcularEstatisticasVeiculoUsoMensal

## Código completo

```sql
CREATE PROCEDURE dbo.sp_RecalcularEstatisticasVeiculoUsoMensal
    @Ano INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @DataInicio DATE = DATEFROMPARTS(@Ano, @Mes, 1);
        DECLARE @DataFim DATE = EOMONTH(@DataInicio);

        DELETE FROM EstatisticaVeiculoUsoMensal WHERE Ano = @Ano AND Mes = @Mes;

        DECLARE @TotalViagens INT = 0;
        DECLARE @KmTotal DECIMAL(18,2) = 0;

        SELECT @TotalViagens = COUNT(*),
               @KmTotal = ISNULL(SUM(ISNULL(KmFinal, 0) - ISNULL(KmInicial, 0)), 0)
        FROM Viagem
        WHERE DataInicial >= @DataInicio AND DataInicial < DATEADD(DAY, 1, @DataFim);

        DECLARE @TotalAbastecimentos INT = 0;
        DECLARE @LitrosTotal DECIMAL(18,2) = 0;
        DECLARE @ValorAbastecimento DECIMAL(18,2) = 0;

        SELECT @TotalAbastecimentos = COUNT(*),
               @LitrosTotal = ISNULL(SUM(ISNULL(Litros, 0)), 0),
               @ValorAbastecimento = ISNULL(SUM(ISNULL(Litros, 0) * ISNULL(ValorUnitario, 0)), 0)
        FROM Abastecimento
        WHERE DataHora >= @DataInicio AND DataHora < DATEADD(DAY, 1, @DataFim);

        DECLARE @ConsumoMedio DECIMAL(10,2) = 0;
        IF @LitrosTotal > 0
            SET @ConsumoMedio = @KmTotal / @LitrosTotal;

        INSERT INTO EstatisticaVeiculoUsoMensal (
            Ano, Mes, TotalViagens, KmTotalRodado,
            TotalAbastecimentos, LitrosTotal, ValorAbastecimento,
            ConsumoMedio, DataAtualizacao
        )
        VALUES (
            @Ano, @Mes, @TotalViagens, @KmTotal,
            @TotalAbastecimentos, @LitrosTotal, @ValorAbastecimento,
            @ConsumoMedio, GETDATE()
        );

        COMMIT TRANSACTION;
        PRINT 'Estatísticas de uso do mês ' + CAST(@Mes AS VARCHAR) + '/' + CAST(@Ano AS VARCHAR) + ' recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
```

## Explicação por blocos

- **Intervalo**: mês/ano informados.
- **Viagens**: total e km rodado (simples diferença KmFinal-KmInicial).
- **Abastecimentos**: total, litros e valor do mês.
- **Consumo médio**: km total / litros totais se houver litros.
- **Persistência**: grava em `EstatisticaVeiculoUsoMensal`; transação protege o recálculo.
- **Uso**: chamado nas rotinas “todas” e “mês atual”.


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
