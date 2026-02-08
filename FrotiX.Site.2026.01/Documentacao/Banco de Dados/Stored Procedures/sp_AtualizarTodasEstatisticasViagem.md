# sp_AtualizarTodasEstatisticasViagem

## Código completo

```sql
CREATE PROCEDURE dbo.sp_AtualizarTodasEstatisticasViagem
AS
BEGIN
    SET NOCOUNT ON
    
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ViagemEstatistica')
    BEGIN
        PRINT 'Tabela ViagemEstatistica nao existe - ignorando'
        RETURN
    END
    
    DECLARE @DataAtual DATE
    DECLARE @DataMin DATE
    DECLARE @DataMax DATE
    DECLARE @Hoje DATE = CAST(GETDATE() AS DATE)
    DECLARE @Total INT = 0
    
    SELECT @DataMin = MIN(CAST(DataInicial AS DATE)), @DataMax = MAX(CAST(DataInicial AS DATE))
    FROM Viagem
    WHERE DataInicial IS NOT NULL AND Status = 'Realizada'
    
    -- Limitar até a data atual (não calcular para datas futuras)
    IF @DataMax > @Hoje
        SET @DataMax = @Hoje
    
    SET @DataAtual = @DataMin
    
    PRINT 'Atualizando estatisticas de ' + CONVERT(VARCHAR, @DataMin, 103) + ' a ' + CONVERT(VARCHAR, @DataMax, 103) + '...'
    
    WHILE @DataAtual <= @DataMax
    BEGIN
        EXEC sp_AtualizarEstatisticasViagem @DataAtual
        SET @DataAtual = DATEADD(DAY, 1, @DataAtual)
        SET @Total = @Total + 1
        
        IF @Total % 100 = 0
            PRINT '  Processados ' + CAST(@Total AS VARCHAR) + ' dias...'
    END
    
    PRINT 'Total: ' + CAST(@Total AS VARCHAR) + ' dias processados com sucesso!'
    
    RETURN @Total
END
```

## Explicação por blocos

- **Pré-checagem**: só roda se a tabela `ViagemEstatistica` existir.
- **Intervalo**: pega a data mínima e máxima de viagens realizadas, limita ao dia atual para evitar futuro.
- **Loop diário**: executa `sp_AtualizarEstatisticasViagem @DataAtual` para cada dia do intervalo, avançando um dia por vez.
- **Progresso**: log a cada 100 dias processados e total final.
- **Uso**: etapa 6 do job de viagens; reprocessa histórico inteiro após saneamentos ou correções de base.


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
