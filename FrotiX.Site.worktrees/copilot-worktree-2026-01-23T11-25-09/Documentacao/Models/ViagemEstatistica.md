# Documentação: ViagemEstatistica.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 🎯 Objetivos

O Model `ViagemEstatistica` armazena estatísticas agregadas de viagens por data de referência, incluindo contagens, custos, quilometragem e dados agregados em formato JSON para dashboards e relatórios.

**Principais objetivos:**

✅ Armazenar estatísticas diárias agregadas de viagens  
✅ Incluir custos totais e médios por tipo  
✅ Armazenar dados agregados em JSON para performance  
✅ Suportar dashboards e relatórios com dados pré-calculados

---

## 🏗️ Estrutura do Model

```csharp
[Table("ViagemEstatistica")]
public class ViagemEstatistica
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public DateTime DataReferencia { get; set; }
    
    // Estatísticas gerais
    public int TotalViagens { get; set; }
    public int ViagensFinalizadas { get; set; }
    public int ViagensEmAndamento { get; set; }
    public int ViagensAgendadas { get; set; }
    public int ViagensCanceladas { get; set; }
    
    // Custos
    public decimal CustoTotal { get; set; }
    public decimal CustoMedioPorViagem { get; set; }
    public decimal CustoVeiculo { get; set; }
    public decimal CustoMotorista { get; set; }
    public decimal CustoOperador { get; set; }
    public decimal CustoLavador { get; set; }
    public decimal CustoCombustivel { get; set; }
    
    // Quilometragem
    public decimal QuilometragemTotal { get; set; }
    public decimal QuilometragemMedia { get; set; }
    
    // Dados agregados em JSON
    [Column(TypeName = "nvarchar(max)")]
    public string ViagensPorStatusJson { get; set; }
    public string ViagensPorMotoristaJson { get; set; }
    public string ViagensPorVeiculoJson { get; set; }
    // ... outros campos JSON
    
    // Timestamps
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
```

---

## 🔗 Quem Chama e Por Quê

### ViagemEstatisticaService.cs → Gera Estatísticas

```csharp
public async Task GerarEstatisticasDia(DateTime dataReferencia)
{
    var estatistica = new ViagemEstatistica
    {
        DataReferencia = dataReferencia
    };
    
    var viagens = await _context.Viagem
        .Where(v => v.DataInicial.Value.Date == dataReferencia)
        .ToListAsync();
    
    estatistica.TotalViagens = viagens.Count;
    estatistica.ViagensFinalizadas = viagens.Count(v => v.Status == "Realizada");
    estatistica.CustoTotal = (decimal)viagens.Sum(v => v.CustoTotal);
    
    // Agrega dados em JSON
    estatistica.ViagensPorStatusJson = JsonSerializer.Serialize(
        viagens.GroupBy(v => v.Status)
            .Select(g => new { Status = g.Key, Quantidade = g.Count() })
    );
    
    _unitOfWork.ViagemEstatistica.Add(estatistica);
    _unitOfWork.Save();
}
```

---

## 📝 Notas Importantes

1. **Dados pré-calculados** - Estatísticas são calculadas e armazenadas para performance.

2. **JSON para agregações** - Campos JSON armazenam dados agregados complexos.

3. **Uma entrada por dia** - `DataReferencia` identifica o dia das estatísticas.

---

**📅 Documentação criada em:** 08/01/2026


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
