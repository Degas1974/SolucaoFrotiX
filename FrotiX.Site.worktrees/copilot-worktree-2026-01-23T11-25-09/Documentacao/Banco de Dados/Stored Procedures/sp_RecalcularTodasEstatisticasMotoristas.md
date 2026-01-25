# sp_RecalcularTodasEstatisticasMotoristas

## Código completo

```sql
CREATE PROCEDURE dbo.sp_RecalcularTodasEstatisticasMotoristas
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AnoMes TABLE (Ano INT, Mes INT);

    -- Busca todos os anos/meses com viagens
    INSERT INTO @AnoMes (Ano, Mes)
    SELECT DISTINCT YEAR(DataInicial), MONTH(DataInicial)
    FROM Viagem
    WHERE DataInicial IS NOT NULL AND MotoristaId IS NOT NULL
    ORDER BY YEAR(DataInicial), MONTH(DataInicial);

    -- Adiciona meses de multas que não estão em viagens
    INSERT INTO @AnoMes (Ano, Mes)
    SELECT DISTINCT YEAR(Data), MONTH(Data)
    FROM Multa
    WHERE Data IS NOT NULL
      AND MotoristaId IS NOT NULL
      AND NOT EXISTS (
          SELECT 1 FROM @AnoMes am
          WHERE am.Ano = YEAR(Data) AND am.Mes = MONTH(Data)
      );

    -- Adiciona meses de abastecimentos que não estão nas anteriores
    INSERT INTO @AnoMes (Ano, Mes)
    SELECT DISTINCT YEAR(DataHora), MONTH(DataHora)
    FROM Abastecimento
    WHERE DataHora IS NOT NULL
      AND MotoristaId IS NOT NULL
      AND MotoristaId <> '00000000-0000-0000-0000-000000000000'
      AND NOT EXISTS (
          SELECT 1 FROM @AnoMes am
          WHERE am.Ano = YEAR(DataHora) AND am.Mes = MONTH(DataHora)
      );

    DECLARE @Ano INT, @Mes INT;
    DECLARE cur CURSOR FOR SELECT Ano, Mes FROM @AnoMes ORDER BY Ano, Mes;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Ano, @Mes;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Processando ' + CAST(@Mes AS VARCHAR) + '/' + CAST(@Ano AS VARCHAR) + '...';
        EXEC sp_RecalcularEstatisticasMotoristas @Ano, @Mes;
        FETCH NEXT FROM cur INTO @Ano, @Mes;
    END

    CLOSE cur;
    DEALLOCATE cur;

    PRINT 'Todas as estatísticas foram recalculadas!';
END
```

## Explicação por blocos

- **Levantamento de períodos**: combina meses que têm viagens, multas ou abastecimentos com motorista, evitando duplicação.
- **Loop mensal**: para cada (ano, mês) executa `sp_RecalcularEstatisticasMotoristas`.
- **Logs**: imprime mês/ano processado e mensagem final.
- **Uso**: reprocessamento completo dos KPIs de motoristas após correções amplas em viagens/multas/abastecimentos.


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
