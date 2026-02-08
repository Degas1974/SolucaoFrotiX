# sp_RecalcularRankingsVeiculoAnual

## Código completo

```sql
CREATE PROCEDURE dbo.sp_RecalcularRankingsVeiculoAnual
    @Ano INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Ranking por KM Rodado
        DELETE FROM EstatisticaVeiculoRankingKm WHERE Ano = @Ano;
        INSERT INTO EstatisticaVeiculoRankingKm (
            Ano, VeiculoId, Placa, Modelo, KmRodado, TotalViagens, DataAtualizacao
        )
        SELECT @Ano, vi.VeiculoId, v.Placa, m.DescricaoModelo,
               SUM(ISNULL(vi.KmFinal, 0) - ISNULL(vi.KmInicial, 0)),
               COUNT(*),
               GETDATE()
        FROM Viagem vi
        INNER JOIN Veiculo v ON vi.VeiculoId = v.VeiculoId
        LEFT JOIN ModeloVeiculo m ON v.ModeloId = m.ModeloId
        WHERE YEAR(vi.DataInicial) = @Ano
          AND vi.VeiculoId IS NOT NULL AND vi.VeiculoId <> '00000000-0000-0000-0000-000000000000'
        GROUP BY vi.VeiculoId, v.Placa, m.DescricaoModelo;

        -- Ranking por Litros Abastecidos
        DELETE FROM EstatisticaVeiculoRankingLitros WHERE Ano = @Ano;
        INSERT INTO EstatisticaVeiculoRankingLitros (
            Ano, VeiculoId, Placa, Modelo, LitrosAbastecidos, ValorTotal, TotalAbastecimentos, DataAtualizacao
        )
        SELECT @Ano, a.VeiculoId, v.Placa, m.DescricaoModelo,
               SUM(ISNULL(a.Litros, 0)),
               SUM(ISNULL(a.Litros, 0) * ISNULL(a.ValorUnitario, 0)),
               COUNT(*),
               GETDATE()
        FROM Abastecimento a
        INNER JOIN Veiculo v ON a.VeiculoId = v.VeiculoId
        LEFT JOIN ModeloVeiculo m ON v.ModeloId = m.ModeloId
        WHERE YEAR(a.DataHora) = @Ano
          AND a.VeiculoId IS NOT NULL AND a.VeiculoId <> '00000000-0000-0000-0000-000000000000'
        GROUP BY a.VeiculoId, v.Placa, m.DescricaoModelo;

        -- Ranking por Consumo (km/l)
        DELETE FROM EstatisticaVeiculoRankingConsumo WHERE Ano = @Ano;
        WITH KmPorVeiculo AS (
            SELECT VeiculoId, SUM(ISNULL(KmFinal, 0) - ISNULL(KmInicial, 0)) AS KmRodado
            FROM Viagem
            WHERE YEAR(DataInicial) = @Ano
              AND VeiculoId IS NOT NULL AND VeiculoId <> '00000000-0000-0000-0000-000000000000'
            GROUP BY VeiculoId
        ),
        LitrosPorVeiculo AS (
            SELECT VeiculoId, SUM(ISNULL(Litros, 0)) AS LitrosAbastecidos, COUNT(*) AS TotalAbastecimentos
            FROM Abastecimento
            WHERE YEAR(DataHora) = @Ano
              AND VeiculoId IS NOT NULL AND VeiculoId <> '00000000-0000-0000-0000-000000000000'
            GROUP BY VeiculoId
        )
        INSERT INTO EstatisticaVeiculoRankingConsumo (
            Ano, VeiculoId, Placa, Modelo, KmRodado, LitrosAbastecidos,
            ConsumoKmPorLitro, TotalAbastecimentos, DataAtualizacao
        )
        SELECT @Ano,
               COALESCE(k.VeiculoId, l.VeiculoId),
               v.Placa,
               m.DescricaoModelo,
               ISNULL(k.KmRodado, 0),
               ISNULL(l.LitrosAbastecidos, 0),
               CASE WHEN ISNULL(l.LitrosAbastecidos, 0) > 0
                    THEN ROUND(ISNULL(k.KmRodado, 0) / l.LitrosAbastecidos, 2)
                    ELSE 0 END,
               ISNULL(l.TotalAbastecimentos, 0),
               GETDATE()
        FROM KmPorVeiculo k
        FULL OUTER JOIN LitrosPorVeiculo l ON k.VeiculoId = l.VeiculoId
        INNER JOIN Veiculo v ON COALESCE(k.VeiculoId, l.VeiculoId) = v.VeiculoId
        LEFT JOIN ModeloVeiculo m ON v.ModeloId = m.ModeloId
        WHERE ISNULL(k.KmRodado, 0) > 0 OR ISNULL(l.LitrosAbastecidos, 0) > 0;

        -- Anos disponíveis
        DELETE FROM AnosDisponiveisVeiculo WHERE Ano = @Ano;
        INSERT INTO AnosDisponiveisVeiculo (Ano, TotalViagens, TotalAbastecimentos, DataAtualizacao)
        SELECT @Ano,
               (SELECT COUNT(*) FROM Viagem WHERE YEAR(DataInicial) = @Ano),
               (SELECT COUNT(*) FROM Abastecimento WHERE YEAR(DataHora) = @Ano),
               GETDATE();

        COMMIT TRANSACTION;
        PRINT 'Rankings anuais de veículos do ano ' + CAST(@Ano AS VARCHAR) + ' recalculados com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
```

## Explicação por blocos

- **KM**: soma km rodado e viagens por veículo no ano.
-. **Litros/valor**: soma litros e valor abastecido.
- **Consumo**: km/l calculado do join km+litros (full join cobre casos só de km ou só de litros).
- **Anos disponíveis**: registra contagem de viagens/abastecimentos por ano.
- **Transação**: garante consistência; rollback em erro.
- **Uso**: rankings anuais; chamado nas rotinas “todas” e “mês atual”.


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
