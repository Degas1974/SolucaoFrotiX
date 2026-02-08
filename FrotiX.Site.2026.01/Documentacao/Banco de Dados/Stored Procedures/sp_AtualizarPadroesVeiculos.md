# sp_AtualizarPadroesVeiculos

## Código completo

```sql
CREATE PROCEDURE dbo.sp_AtualizarPadroesVeiculos
AS
BEGIN
    SET NOCOUNT ON
    
    DECLARE @KM_MAXIMO_VIAGEM INT = 2000
    DECLARE @VeiculosProcessados INT
    
    PRINT '  Calculando padrões estatísticos por veículo...'
    
    -- Limpar tabela
    DELETE FROM VeiculoPadraoViagem
    
    -- PASSO 1: Calcular agregações básicas
    ;WITH AgregacoesVeiculo AS (
        SELECT 
            VeiculoId,
            AVG(CAST(COALESCE(MinutosNormalizado, Minutos) AS DECIMAL(18,2))) AS AvgDuracaoMinutos,
            AVG(CAST(COALESCE(KmRodadoNormalizado, KmFinal - KmInicial) AS DECIMAL(18,2))) AS AvgKmPorViagem,
            CASE 
                WHEN COUNT(DISTINCT CAST(COALESCE(DataInicialNormalizada, DataInicial) AS DATE)) > 0 
                THEN SUM(CAST(COALESCE(KmRodadoNormalizado, KmFinal - KmInicial) AS DECIMAL(18,2))) / 
                     COUNT(DISTINCT CAST(COALESCE(DataInicialNormalizada, DataInicial) AS DATE))
                ELSE NULL 
            END AS AvgKmPorDia,
            ISNULL(STDEV(CAST(COALESCE(KmRodadoNormalizado, KmFinal - KmInicial) AS DECIMAL(18,2))), 0) AS StdDevKm,
            COUNT(*) AS TotalViagens,
            CASE 
                WHEN AVG(CAST(COALESCE(MinutosNormalizado, Minutos) AS FLOAT)) < 120 THEN 'DIARIO'
                WHEN AVG(CAST(COALESCE(MinutosNormalizado, Minutos) AS FLOAT)) > 480 THEN 'LONGA_DURACAO'
                ELSE 'MISTO'
            END AS TipoUsoCalculado
        FROM Viagem
        WHERE Status = 'Realizada'
          AND VeiculoId IS NOT NULL
          AND COALESCE(KmRodadoNormalizado, KmFinal - KmInicial) BETWEEN 1 AND @KM_MAXIMO_VIAGEM
        GROUP BY VeiculoId
        HAVING COUNT(*) >= 5
    ),
    -- PASSO 2: Calcular percentis
    PercentisPorVeiculo AS (
        SELECT DISTINCT
            VeiculoId,
            PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY COALESCE(KmRodadoNormalizado, KmFinal - KmInicial)) 
                OVER (PARTITION BY VeiculoId) AS MedianaKm,
            PERCENTILE_CONT(0.25) WITHIN GROUP (ORDER BY COALESCE(KmRodadoNormalizado, KmFinal - KmInicial)) 
                OVER (PARTITION BY VeiculoId) AS Q1,
            PERCENTILE_CONT(0.75) WITHIN GROUP (ORDER BY COALESCE(KmRodadoNormalizado, KmFinal - KmInicial)) 
                OVER (PARTITION BY VeiculoId) AS Q3
        FROM Viagem
        WHERE Status = 'Realizada'
          AND VeiculoId IS NOT NULL
          AND COALESCE(KmRodadoNormalizado, KmFinal - KmInicial) BETWEEN 1 AND @KM_MAXIMO_VIAGEM
    )
    -- PASSO 3: Inserir dados combinados
    INSERT INTO VeiculoPadraoViagem (
        VeiculoId, 
        AvgDuracaoMinutos, 
        AvgKmPorViagem, 
        AvgKmPorDia,
        MaxKmNormalPorViagem,
        MedianaKm,
        Q1Km,
        Q3Km,
        IQRKm,
        LimiteInferiorKm,
        LimiteSuperiorKm,
        TotalViagensAnalisadas,
        TipoUso,
        DataAtualizacao
    )
    SELECT 
        A.VeiculoId,
        A.AvgDuracaoMinutos,
        A.AvgKmPorViagem,
        A.AvgKmPorDia,
        -- Max normal = média + 2 desvios padrão (mínimo 500km)
        CASE 
            WHEN A.AvgKmPorViagem + 2 * A.StdDevKm > 500
            THEN A.AvgKmPorViagem + 2 * A.StdDevKm
            ELSE 500
        END AS MaxKmNormal,
        P.MedianaKm,
        P.Q1,
        P.Q3,
        P.Q3 - P.Q1 AS IQR,
        P.Q1 - 1.5 * (P.Q3 - P.Q1) AS LimiteInferior,
        P.Q3 + 1.5 * (P.Q3 - P.Q1) AS LimiteSuperior,
        A.TotalViagens,
        A.TipoUsoCalculado,
        GETDATE()
    FROM AgregacoesVeiculo A
    INNER JOIN PercentisPorVeiculo P ON A.VeiculoId = P.VeiculoId
    
    SELECT @VeiculosProcessados = @@ROWCOUNT
    
    PRINT '  ✓ Padrões calculados para ' + CAST(@VeiculosProcessados AS VARCHAR) + ' veículos'
    
    RETURN @VeiculosProcessados
END
```

## Explicação por blocos

- **Limpeza inicial**: zera `VeiculoPadraoViagem` para recarga completa.
- **Passo 1 – agregações**: para veículos com ≥5 viagens realizadas e km entre 1–2000, calcula médias (duração, km/viagem, km/dia), desvio padrão e classifica o tipo de uso (DIARIO/LONGA_DURACAO/MISTO).
- **Passo 2 – percentis**: computa mediana, Q1, Q3 de km rodado por veículo.
- **Passo 3 – inserção**: insere linha por veículo com limites IQR, max km normal (média + 2*desvio, mínimo 500 km), IQR e tipo de uso.
- **Resultado**: imprime quantidade processada e retorna o total; base é usada depois em `sp_NormalizarViagens` para corrigir outliers de km.


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
