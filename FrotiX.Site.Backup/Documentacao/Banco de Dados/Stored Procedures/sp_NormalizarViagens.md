# sp_NormalizarViagens

## Código completo

```sql
CREATE PROCEDURE dbo.sp_NormalizarViagens
    @DiasParaProcessar INT = 365,
    @ForcarReprocessamento BIT = 0
AS
BEGIN
    SET NOCOUNT ON
    
    DECLARE @KM_MAXIMO_VIAGEM INT = 2000
    DECLARE @TotalNormalizadas INT = 0
    DECLARE @DataLimite DATE = DATEADD(DAY, -@DiasParaProcessar, GETDATE())
    
    PRINT '  Normalizando viagens dos últimos ' + CAST(@DiasParaProcessar AS VARCHAR) + ' dias...'
    
    -- PASSO 1: Copiar datas válidas
    PRINT '  [PASSO 1] Copiando datas válidas...'
    
    UPDATE Viagem
    SET 
        DataInicialNormalizada = DataInicial,
        DataFinalNormalizada = DataFinal,
        HoraInicioNormalizada = HoraInicio,
        HoraFimNormalizada = HoraFim,
        FoiNormalizada = 0,
        TipoNormalizacao = NULL,
        DataNormalizacao = GETDATE()
    WHERE DataInicial IS NOT NULL
      AND DataInicial >= @DataLimite
      AND (@ForcarReprocessamento = 1 OR DataInicialNormalizada IS NULL)
    
    PRINT '           ' + CAST(@@ROWCOUNT AS VARCHAR) + ' datas copiadas'
    
    -- PASSO 2: Corrigir datas invertidas
    PRINT '  [PASSO 2] Corrigindo datas invertidas...'
    
    UPDATE Viagem
    SET 
        DataInicialNormalizada = DataFinalNormalizada,
        DataFinalNormalizada = DataInicialNormalizada,
        FoiNormalizada = 1,
        TipoNormalizacao = 'DATAS_INVERTIDAS'
    WHERE DataInicialNormalizada > DataFinalNormalizada
      AND DataFinalNormalizada IS NOT NULL
      AND DataInicial >= @DataLimite
    
    SET @TotalNormalizadas = @TotalNormalizadas + @@ROWCOUNT
    PRINT '           ' + CAST(@@ROWCOUNT AS VARCHAR) + ' datas invertidas corrigidas'
    
    -- PASSO 3: Copiar KM válidos
    PRINT '  [PASSO 3] Copiando KM válidos...'
    
    UPDATE Viagem
    SET 
        KmInicialNormalizado = KmInicial,
        KmFinalNormalizado = KmFinal,
        KmRodadoNormalizado = KmFinal - KmInicial
    WHERE KmInicial IS NOT NULL
      AND KmFinal IS NOT NULL
      AND KmFinal >= KmInicial
      AND (KmFinal - KmInicial) <= @KM_MAXIMO_VIAGEM
      AND DataInicial >= @DataLimite
    
    PRINT '           ' + CAST(@@ROWCOUNT AS VARCHAR) + ' KM válidos copiados'
    
    -- PASSO 4: Tratar KM outliers usando padrão do veículo
    PRINT '  [PASSO 4] Corrigindo KM outliers...'
    
    UPDATE v
    SET 
        KmFinalNormalizado = v.KmInicial + CAST(COALESCE(p.MedianaKm, p.AvgKmPorViagem, 50) AS INT),
        KmRodadoNormalizado = CAST(COALESCE(p.MedianaKm, p.AvgKmPorViagem, 50) AS INT),
        FoiNormalizada = 1,
        TipoNormalizacao = COALESCE(v.TipoNormalizacao + ',', '') + 'KM_OUTLIER'
    FROM Viagem v
    INNER JOIN VeiculoPadraoViagem p ON v.VeiculoId = p.VeiculoId
    WHERE v.KmInicial IS NOT NULL
      AND v.KmFinal IS NOT NULL
      AND (v.KmFinal - v.KmInicial) > @KM_MAXIMO_VIAGEM
      AND v.DataInicial >= @DataLimite
    
    SET @TotalNormalizadas = @TotalNormalizadas + @@ROWCOUNT
    PRINT '           ' + CAST(@@ROWCOUNT AS VARCHAR) + ' KM outliers corrigidos'
    
    -- PASSO 5: Anular KM outliers sem padrão
    UPDATE v
    SET 
        KmFinalNormalizado = NULL,
        KmRodadoNormalizado = NULL,
        FoiNormalizada = 1,
        TipoNormalizacao = COALESCE(v.TipoNormalizacao + ',', '') + 'KM_ANULADO'
    FROM Viagem v
    LEFT JOIN VeiculoPadraoViagem p ON v.VeiculoId = p.VeiculoId
    WHERE v.KmInicial IS NOT NULL
      AND v.KmFinal IS NOT NULL
      AND (v.KmFinal - v.KmInicial) > @KM_MAXIMO_VIAGEM
      AND p.VeiculoId IS NULL
      AND v.DataInicial >= @DataLimite
    
    SET @TotalNormalizadas = @TotalNormalizadas + @@ROWCOUNT
    
    -- PASSO 6: Calcular MinutosNormalizado
    PRINT '  [PASSO 6] Calculando minutos normalizados...'
    
    UPDATE Viagem
    SET MinutosNormalizado = dbo.fn_CalculaMinutosUteis(
        DataInicialNormalizada,
        DataFinalNormalizada,
        HoraInicioNormalizada,
        HoraFimNormalizada
    )
    WHERE DataInicialNormalizada IS NOT NULL
      AND HoraInicioNormalizada IS NOT NULL
      AND DataInicial >= @DataLimite
    
    PRINT '           ' + CAST(@@ROWCOUNT AS VARCHAR) + ' minutos calculados'
    
    -- PASSO 7: Limitar minutos excessivos
    UPDATE Viagem
    SET MinutosNormalizado = 13200,
        FoiNormalizada = 1,
        TipoNormalizacao = COALESCE(TipoNormalizacao + ',', '') + 'MINUTOS_LIMITADOS'
    WHERE MinutosNormalizado > 13200
      AND DataInicial >= @DataLimite
    
    SET @TotalNormalizadas = @TotalNormalizadas + @@ROWCOUNT
    
    PRINT ''
    PRINT '  ✓ Normalização concluída! Total corrigidas: ' + CAST(@TotalNormalizadas AS VARCHAR)
    
    RETURN @TotalNormalizadas
END
```

## Explicação por blocos

- **Janela de processamento**: limita às viagens dos últimos `@DiasParaProcessar` (default 365); pode forçar reprocessamento com `@ForcarReprocessamento = 1`.
- **Passo 1 – copia campos**: clona datas/horas válidas para campos normalizados, zera flags.
- **Passo 2 – datas invertidas**: troca início/fim quando invertidos, marca normalização.
- **Passo 3 – km válidos**: copia km quando consistentes e <= 2000 km.
- **Passo 4 – km outliers com padrão**: corrige km usando mediana/avg do `VeiculoPadraoViagem`; marca `KM_OUTLIER`.
- **Passo 5 – km outliers sem padrão**: anula km se não há padrão para o veículo; marca `KM_ANULADO`.
- **Passo 6 – minutos**: calcula minutos úteis via `fn_CalculaMinutosUteis` com datas/horas normalizadas.
- **Passo 7 – limite minutos**: trunca minutos > 13.200 (220h) e registra motivo.
- **Saída**: total normalizado; logs passo a passo. Base para recálculo de custos e estatísticas.


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
