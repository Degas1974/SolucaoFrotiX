# sp_RecalcularTodasEstatisticasAbastecimentos

## Código completo

```sql
CREATE PROCEDURE dbo.sp_RecalcularTodasEstatisticasAbastecimentos
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AnoMes TABLE (Ano INT, Mes INT);

    -- Busca todos os anos/meses com abastecimentos
    INSERT INTO @AnoMes (Ano, Mes)
    SELECT DISTINCT YEAR(DataHora), MONTH(DataHora)
    FROM Abastecimento
    WHERE DataHora IS NOT NULL
    ORDER BY YEAR(DataHora), MONTH(DataHora);

    DECLARE @Ano INT, @Mes INT;
    DECLARE cur CURSOR FOR SELECT Ano, Mes FROM @AnoMes ORDER BY Ano, Mes;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Ano, @Mes;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Processando ' + CAST(@Mes AS VARCHAR) + '/' + CAST(@Ano AS VARCHAR) + '...';
        EXEC sp_RecalcularEstatisticasAbastecimentos @Ano, @Mes;
        FETCH NEXT FROM cur INTO @Ano, @Mes;
    END

    CLOSE cur;
    DEALLOCATE cur;

    -- Recalcula estatísticas anuais
    DECLARE @Anos TABLE (Ano INT);
    INSERT INTO @Anos SELECT DISTINCT Ano FROM @AnoMes;

    DECLARE cur2 CURSOR FOR SELECT Ano FROM @Anos ORDER BY Ano;
    OPEN cur2;
    FETCH NEXT FROM cur2 INTO @Ano;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Processando estatísticas anuais de ' + CAST(@Ano AS VARCHAR) + '...';
        EXEC sp_RecalcularEstatisticasAbastecimentosAnuais @Ano;
        FETCH NEXT FROM cur2 INTO @Ano;
    END

    CLOSE cur2;
    DEALLOCATE cur2;

    PRINT 'Todas as estatísticas de abastecimentos foram recalculadas!';
END
```

## Explicação por blocos

- **Levantamento de períodos**: monta tabela temporária com todos os (ano, mês) existentes em `Abastecimento`.
- **Loop mensal**: percorre cada mês encontrado e chama `sp_RecalcularEstatisticasAbastecimentos`.
- **Loop anual**: em seguida percorre cada ano distinto e chama `sp_RecalcularEstatisticasAbastecimentosAnuais`.
- **Logs**: imprime progresso por mês/ano e mensagem final.
- **Uso**: rotina pesada de reprocessamento completo, indicada após importações ou grandes correções históricas.


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
