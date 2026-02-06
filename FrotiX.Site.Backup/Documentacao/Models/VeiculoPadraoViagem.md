# Documentação: VeiculoPadraoViagem.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 📋 Índice

1. [Objetivos](#objetivos)
2. [Arquivos Envolvidos](#arquivos-envolvidos)
3. [Estrutura do Model](#estrutura-do-model)
4. [Mapeamento Model ↔ Banco de Dados](#mapeamento-model--banco-de-dados)
5. [Quem Chama e Por Quê](#quem-chama-e-por-quê)
6. [Problema → Solução → Código](#problema--solução--código)
7. [Fluxo de Funcionamento](#fluxo-de-funcionamento)
8. [Troubleshooting](#troubleshooting)

---

## 🎯 Objetivos

O Model `VeiculoPadraoViagem` armazena padrões de comportamento calculados para cada veículo baseados no histórico de viagens e abastecimentos. Esses padrões são usados para detecção de outliers (valores anormais) e normalização de dados.

**Principais objetivos:**

✅ Armazenar médias calculadas de cada veículo (KM, duração, consumo)  
✅ Suportar detecção de outliers em viagens e abastecimentos  
✅ Permitir normalização de dados baseada em histórico  
✅ Classificar tipo de uso do veículo (baseado em padrões)  
✅ Rastrear quando padrões foram atualizados pela última vez

---

## 📁 Arquivos Envolvidos

### Arquivo Principal
- **`Models/VeiculoPadraoViagem.cs`** - Model Entity Framework Core

### Arquivos que Utilizam
- **`Repository/VeiculoPadraoViagemRepository.cs`** - Acesso e atualização de padrões
- **`Services/ViagemEstatisticaService.cs`** - Calcula padrões a partir de histórico
- **`Controllers/AdministracaoController.cs`** - Usa padrões para estatísticas
- **`Controllers/DashboardViagensController.cs`** - Usa padrões para análise

---

## 🏗️ Estrutura do Model

```csharp
[Table("VeiculoPadraoViagem")]
public class VeiculoPadraoViagem
{
    // ✅ Chave primária (VeiculoId é a chave)
    [Key]
    [Display(Name = "Veículo")]
    public Guid VeiculoId { get; set; }

    // ✅ Classificação do veículo
    [StringLength(50)]
    [Display(Name = "Tipo de Uso")]
    public string? TipoUso { get; set; } // Ex: "Urbano", "Rodoviário", "Misto"

    // ✅ Estatísticas de viagens
    [Display(Name = "Total de Viagens")]
    public int TotalViagens { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Média de Duração (Minutos)")]
    public decimal? MediaDuracaoMinutos { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Média de KM por Viagem")]
    public decimal? MediaKmPorViagem { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Média de KM por Dia")]
    public decimal? MediaKmPorDia { get; set; }

    // ✅ Estatísticas de abastecimentos
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Média KM entre Abastecimentos")]
    public decimal? MediaKmEntreAbastecimentos { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Média Dias entre Abastecimentos")]
    public decimal? MediaDiasEntreAbastecimentos { get; set; }

    [Display(Name = "Total Abastecimentos Analisados")]
    public int? TotalAbastecimentosAnalisados { get; set; }

    // ✅ Controle
    [Display(Name = "Data de Atualização")]
    public DateTime? DataAtualizacao { get; set; }

    // ✅ Navegação
    [ForeignKey("VeiculoId")]
    public virtual Veiculo? Veiculo { get; set; }
}
```

---

## 🗄️ Mapeamento Model ↔ Banco de Dados

### Estrutura SQL da Tabela

```sql
CREATE TABLE [dbo].[VeiculoPadraoViagem] (
    [VeiculoId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    
    -- Classificação
    [TipoUso] NVARCHAR(50) NULL,
    
    -- Estatísticas de viagens
    [TotalViagens] INT NOT NULL DEFAULT 0,
    [MediaDuracaoMinutos] DECIMAL(18,2) NULL,
    [MediaKmPorViagem] DECIMAL(18,2) NULL,
    [MediaKmPorDia] DECIMAL(18,2) NULL,
    
    -- Estatísticas de abastecimentos
    [MediaKmEntreAbastecimentos] DECIMAL(18,2) NULL,
    [MediaDiasEntreAbastecimentos] DECIMAL(18,2) NULL,
    [TotalAbastecimentosAnalisados] INT NULL,
    
    -- Controle
    [DataAtualizacao] DATETIME2 NULL,
    
    -- Foreign Key
    CONSTRAINT [FK_VeiculoPadraoViagem_Veiculo] 
        FOREIGN KEY ([VeiculoId]) REFERENCES [Veiculo]([VeiculoId]) ON DELETE CASCADE
);

-- Índice único (chave primária já é índice)
```

**Configuração no DbContext:**
```csharp
modelBuilder.Entity<VeiculoPadraoViagem>(entity =>
{
    entity.HasKey(e => e.VeiculoId);
    entity.HasOne(e => e.Veiculo)
        .WithOne()
        .HasForeignKey<VeiculoPadraoViagem>(e => e.VeiculoId)
        .OnDelete(DeleteBehavior.Cascade);
});
```

---

## 🔗 Quem Chama e Por Quê

### 1. **VeiculoPadraoViagemRepository.cs** → Atualizar Padrões

**Quando:** Serviço calcula novos padrões baseado em histórico  
**Por quê:** Atualizar médias quando há novos dados

```csharp
public void Update(VeiculoPadraoViagem veiculoPadraoViagem)
{
    var objFromDb = _db.VeiculoPadraoViagem
        .FirstOrDefault(s => s.VeiculoId == veiculoPadraoViagem.VeiculoId);
    
    if (objFromDb != null)
    {
        // ✅ Atualiza todos os campos
        objFromDb.TipoUso = veiculoPadraoViagem.TipoUso;
        objFromDb.TotalViagens = veiculoPadraoViagem.TotalViagens;
        objFromDb.MediaDuracaoMinutos = veiculoPadraoViagem.MediaDuracaoMinutos;
        objFromDb.MediaKmPorViagem = veiculoPadraoViagem.MediaKmPorViagem;
        objFromDb.MediaKmPorDia = veiculoPadraoViagem.MediaKmPorDia;
        objFromDb.MediaKmEntreAbastecimentos = veiculoPadraoViagem.MediaKmEntreAbastecimentos;
        objFromDb.MediaDiasEntreAbastecimentos = veiculoPadraoViagem.MediaDiasEntreAbastecimentos;
        objFromDb.TotalAbastecimentosAnalisados = veiculoPadraoViagem.TotalAbastecimentosAnalisados;
        objFromDb.DataAtualizacao = DateTime.Now; // ✅ Atualiza timestamp
        
        _db.SaveChanges();
    }
}
```

### 2. **AdministracaoController.cs** → Distribuição por Tipo de Uso

**Quando:** Dashboard precisa de distribuição de veículos por tipo  
**Por quê:** Exibir gráfico de distribuição

```csharp
[HttpGet("DistribuicaoPorTipoUso")]
public async Task<IActionResult> DistribuicaoPorTipoUso()
{
    var distribuicao = await _context.VeiculoPadraoViagem
        .AsNoTracking()
        .Where(v => !string.IsNullOrEmpty(v.TipoUso))
        .GroupBy(v => v.TipoUso)
        .Select(g => new
        {
            tipoUso = g.Key,
            quantidade = g.Count()
        })
        .OrderByDescending(x => x.quantidade)
        .ToListAsync();
    
    return Ok(new { sucesso = true, dados = distribuicao });
}
```

---

## 🛠️ Problema → Solução → Código

### Problema: Detecção de Outliers em Viagens

**Problema:** Identificar viagens com KM anormalmente alto ou baixo comparado ao padrão do veículo.

**Solução:** Comparar KM da viagem com `MediaKmPorViagem` e desvio padrão calculado.

**Código:**

```csharp
// ✅ Em serviço de validação
public bool IsOutlier(Viagem viagem, VeiculoPadraoViagem padrao)
{
    if (!padrao.MediaKmPorViagem.HasValue || padrao.TotalViagens < 10)
        return false; // Não tem dados suficientes
    
    var kmViagem = (viagem.KmFinal - viagem.KmInicial) ?? 0;
    var media = (double)padrao.MediaKmPorViagem.Value;
    
    // ✅ Considera outlier se estiver além de 2 desvios padrão
    // Simplificado: se estiver além de 50% da média
    var limiteSuperior = media * 1.5;
    var limiteInferior = media * 0.3;
    
    return kmViagem > limiteSuperior || kmViagem < limiteInferior;
}
```

### Problema: Cálculo de Padrões a Partir de Histórico

**Problema:** Calcular médias de KM, duração e consumo para cada veículo baseado em histórico.

**Solução:** Agregar dados de `Viagem` e `Abastecimento` e calcular médias.

**Código:**

```csharp
// ✅ Em ViagemEstatisticaService.cs
public async Task CalcularPadroesVeiculo(Guid veiculoId)
{
    var viagens = await _context.Viagem
        .Where(v => v.VeiculoId == veiculoId && v.Status == "Realizada")
        .ToListAsync();
    
    var abastecimentos = await _context.Abastecimento
        .Where(a => a.VeiculoId == veiculoId && a.KmRodado.HasValue)
        .OrderBy(a => a.DataAbastecimento)
        .ToListAsync();
    
    var padrao = new VeiculoPadraoViagem
    {
        VeiculoId = veiculoId,
        TotalViagens = viagens.Count,
        MediaDuracaoMinutos = viagens
            .Where(v => v.DuracaoMinutos.HasValue)
            .Average(v => (decimal?)v.DuracaoMinutos.Value),
        MediaKmPorViagem = viagens
            .Where(v => v.KmRodado.HasValue)
            .Average(v => (decimal?)v.KmRodado.Value),
        MediaKmPorDia = viagens
            .GroupBy(v => v.DataInicial.Value.Date)
            .Select(g => g.Sum(v => v.KmRodado ?? 0))
            .Average(),
        MediaKmEntreAbastecimentos = abastecimentos
            .Where(a => a.KmRodado.HasValue)
            .Average(a => (decimal?)a.KmRodado.Value),
        MediaDiasEntreAbastecimentos = CalcularMediaDiasEntreAbastecimentos(abastecimentos),
        TotalAbastecimentosAnalisados = abastecimentos.Count,
        DataAtualizacao = DateTime.Now
    };
    
    // ✅ Classifica tipo de uso baseado em padrões
    padrao.TipoUso = ClassificarTipoUso(padrao);
    
    _unitOfWork.VeiculoPadraoViagem.AddOrUpdate(padrao);
    _unitOfWork.Save();
}

private string ClassificarTipoUso(VeiculoPadraoViagem padrao)
{
    if (padrao.MediaKmPorViagem < 50)
        return "Urbano";
    else if (padrao.MediaKmPorViagem > 200)
        return "Rodoviário";
    else
        return "Misto";
}
```

---

## 🔄 Fluxo de Funcionamento

### Fluxo: Cálculo de Padrões

```
1. Serviço de estatísticas é executado (agendado ou manual)
   ↓
2. Para cada veículo ativo:
   ├─ Busca todas as viagens realizadas
   ├─ Busca todos os abastecimentos
   ├─ Calcula médias:
   │   ├─ Média KM por viagem
   │   ├─ Média duração
   │   ├─ Média KM por dia
   │   ├─ Média KM entre abastecimentos
   │   └─ Média dias entre abastecimentos
   ├─ Classifica tipo de uso
   └─ Atualiza VeiculoPadraoViagem
   ↓
3. Padrões são salvos no banco
   ↓
4. Padrões são usados para validação e análise
```

---

## 🔍 Troubleshooting

### Erro: Padrões não são calculados

**Causa:** Serviço não está sendo executado ou não há dados suficientes.

**Solução:**
```csharp
// ✅ Verificar se há dados suficientes
if (viagens.Count < 10)
{
    // Não calcula padrões com poucos dados
    return;
}
```

### Erro: Tipo de uso não é classificado

**Causa:** `ClassificarTipoUso()` não está sendo chamado ou retorna null.

**Solução:**
```csharp
// ✅ Sempre classificar após calcular médias
padrao.TipoUso = ClassificarTipoUso(padrao) ?? "Não Classificado";
```

---

## 📝 Notas Importantes

1. **Chave primária é VeiculoId** - Um registro por veículo, relação 1:1.

2. **CASCADE DELETE** - Se veículo for deletado, padrões são removidos automaticamente.

3. **Cálculo periódico** - Padrões devem ser recalculados periodicamente conforme novos dados.

4. **Detecção de outliers** - Usado para validar viagens e abastecimentos anormais.

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
