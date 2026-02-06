# sp_RecalcularEstatisticasVeiculoUnidade

## Código completo

```sql
CREATE PROCEDURE dbo.sp_RecalcularEstatisticasVeiculoUnidade
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM EstatisticaVeiculoUnidade;

        INSERT INTO EstatisticaVeiculoUnidade (
            UnidadeId, Unidade, TotalVeiculos, VeiculosAtivos, DataAtualizacao
        )
        SELECT
            MIN(v.UnidadeId), -- primeiro UnidadeId encontrado por sigla
            ISNULL(u.Sigla, 'Sem Unidade'),
            COUNT(*),
            SUM(CASE WHEN v.Status = 1 THEN 1 ELSE 0 END),
            GETDATE()
        FROM Veiculo v
        LEFT JOIN Unidade u ON v.UnidadeId = u.UnidadeId
        GROUP BY u.Sigla;

        COMMIT TRANSACTION;
        PRINT 'Estatísticas por unidade recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
```

## Explicação por blocos

- **Transação**: limpa e repovoa `EstatisticaVeiculoUnidade`.
- **Cálculo**: total e ativos por sigla de unidade; mantém um `UnidadeId` representativo.
- **Uso**: snapshot regional; chamado nas rotinas “todas” e “mês atual”.


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
