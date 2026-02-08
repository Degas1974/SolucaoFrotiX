# sp_AtualizarEstatisticasVeiculosMesAtual

## Código completo

```sql
CREATE PROCEDURE dbo.sp_AtualizarEstatisticasVeiculosMesAtual
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Ano INT = YEAR(GETDATE());
    DECLARE @Mes INT = MONTH(GETDATE());

    -- Atualiza snapshot da frota
    EXEC sp_RecalcularEstatisticasVeiculoGeral;
    EXEC sp_RecalcularEstatisticasVeiculoCategoria;
    EXEC sp_RecalcularEstatisticasVeiculoStatus;
    EXEC sp_RecalcularEstatisticasVeiculoModelo;
    EXEC sp_RecalcularEstatisticasVeiculoCombustivel;
    EXEC sp_RecalcularEstatisticasVeiculoUnidade;
    EXEC sp_RecalcularEstatisticasVeiculoAnoFabricacao;

    -- Atualiza mês atual
    EXEC sp_RecalcularEstatisticasVeiculoUsoMensal @Ano, @Mes;
    EXEC sp_RecalcularRankingsVeiculoAnual @Ano;

    -- Atualiza mês anterior
    IF @Mes = 1
    BEGIN
        SET @Ano = @Ano - 1;
        SET @Mes = 12;
    END
    ELSE
        SET @Mes = @Mes - 1;

    EXEC sp_RecalcularEstatisticasVeiculoUsoMensal @Ano, @Mes;

    PRINT 'Estatísticas de veículos do mês atual e anterior atualizadas!';
END
```

## Explicação por blocos

- **Snapshot da frota**: recalcula estatísticas gerais e por categoria/status/modelo/combustível/unidade/ano de fabricação.
- **Mês atual**: recalcula uso mensal e rankings anuais do ano corrente.
- **Mês anterior**: recua um mês (ajustando ano em janeiro) e recalcula uso mensal.
- **Saída**: mensagem de conclusão.
- **Uso**: job diário/semana para manter KPIs de frota atualizados sem percorrer histórico completo.


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
