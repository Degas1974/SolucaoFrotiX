# sp_RecalcularEstatisticasAbastecimentosAnuais

## Código completo

```sql
CREATE PROCEDURE dbo.sp_RecalcularEstatisticasAbastecimentosAnuais
    @Ano INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Recalcula estatísticas anuais por veículo (para ranking)
        DELETE FROM EstatisticaAbastecimentoVeiculo WHERE Ano = @Ano;

        INSERT INTO EstatisticaAbastecimentoVeiculo (Ano, VeiculoId, Placa, TipoVeiculo, Categoria, TotalAbastecimentos, ValorTotal, LitrosTotal, DataAtualizacao)
        SELECT
            @Ano,
            a.VeiculoId,
            v.Placa,
            m.DescricaoModelo,
            v.Categoria,
            COUNT(*),
            SUM(ISNULL(a.Litros, 0) * ISNULL(a.ValorUnitario, 0)),
            SUM(ISNULL(a.Litros, 0)),
            GETDATE()
        FROM Abastecimento a
        INNER JOIN Veiculo v ON a.VeiculoId = v.VeiculoId
        LEFT JOIN ModeloVeiculo m ON v.ModeloId = m.ModeloId
        WHERE YEAR(a.DataHora) = @Ano
          AND a.VeiculoId IS NOT NULL AND a.VeiculoId <> '00000000-0000-0000-0000-000000000000'
        GROUP BY a.VeiculoId, v.Placa, m.DescricaoModelo, v.Categoria;

        -- Atualiza lista de anos disponíveis
        DELETE FROM AnosDisponiveisAbastecimento WHERE Ano = @Ano;
        INSERT INTO AnosDisponiveisAbastecimento (Ano, TotalAbastecimentos, DataAtualizacao)
        SELECT @Ano, COUNT(*), GETDATE()
        FROM Abastecimento
        WHERE YEAR(DataHora) = @Ano;

        COMMIT TRANSACTION;
        PRINT 'Estatísticas anuais de abastecimentos do ano ' + CAST(@Ano AS VARCHAR) + ' recalculadas com sucesso.';

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
```

## Explicação por blocos

- **Limpeza e recálculo**: remove registros do ano e insere consolidados por veículo (placa, modelo, categoria) com totais de abastecimentos, litros e valor.
- **Registro de anos**: atualiza `AnosDisponiveisAbastecimento` com total de abastecimentos do ano.
- **Transação**: garante consistência; rollback em erro.
- **Uso**: chamado em rotinas de mês atual e recálculo total; executável manualmente por ano.


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
