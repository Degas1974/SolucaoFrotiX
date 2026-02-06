# Documentação: RepactuacaoVeiculo.cs

**📅 Última Atualização:** 13/01/2026
**📋 Versão:** 2.1 (Padrão FrotiX Simplificado)

---

## 🎯 Objetivos

O Model `RepactuacaoVeiculo` armazena valores individuais de cada veículo quando há repactuação de contrato de locação, permitindo rastrear valores específicos por veículo em cada repactuação.

**Principais objetivos:**

✅ Armazenar valor individual de cada veículo em uma repactuação  
✅ Vincular veículo à repactuação de contrato  
✅ Permitir observações específicas por veículo  
✅ Rastrear histórico de valores em repactuações

---

## 📁 Arquivos Envolvidos

- **`Models/RepactuacaoVeiculo.cs`** - Model Entity Framework Core
- **`Controllers/ContratoController.cs`** - Endpoints de repactuação
- **`Pages/Contrato/RepactuacaoContrato.cshtml`** - Interface de repactuação
- **`Repository/RepactuacaoVeiculoRepository.cs`** - Acesso a dados

---

## 🏗️ Estrutura do Model

```csharp
public class RepactuacaoVeiculo
{
    [Key]
    public Guid RepactuacaoVeiculoId { get; set; }

    [Display(Name = "Repactuação")]
    public Guid RepactuacaoContratoId { get; set; }
    [ForeignKey("RepactuacaoContratoId")]
    public virtual RepactuacaoContrato RepactuacaoContrato { get; set; }

    [Display(Name = "Veículo")]
    public Guid VeiculoId { get; set; }
    [ForeignKey("VeiculoId")]
    public virtual Veiculo Veiculo { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Valor (R$)")]
    public double? Valor { get; set; }

    [Display(Name = "Observação")]
    public string? Observacao { get; set; }
}
```

---

## 🗄️ Mapeamento Model ↔ Banco de Dados

```sql
CREATE TABLE [dbo].[RepactuacaoVeiculo] (
    [RepactuacaoVeiculoId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [RepactuacaoContratoId] UNIQUEIDENTIFIER NOT NULL,
    [VeiculoId] UNIQUEIDENTIFIER NOT NULL,
    [Valor] FLOAT NULL,
    [Observacao] NVARCHAR(MAX) NULL,
    
    CONSTRAINT [FK_RepactuacaoVeiculo_RepactuacaoContrato] 
        FOREIGN KEY ([RepactuacaoContratoId]) REFERENCES [RepactuacaoContrato]([RepactuacaoContratoId]),
    CONSTRAINT [FK_RepactuacaoVeiculo_Veiculo] 
        FOREIGN KEY ([VeiculoId]) REFERENCES [Veiculo]([VeiculoId])
);
```

---

## 🔗 Quem Chama e Por Quê

### ContratoController.cs → Criar Repactuação

**Quando:** Usuário cria nova repactuação de contrato  
**Por quê:** Armazenar valores individuais de cada veículo

```csharp
[HttpPost("CriarRepactuacao")]
public IActionResult CriarRepactuacao([FromBody] RepactuacaoContrato repactuacao, List<RepactuacaoVeiculo> veiculos)
{
    _unitOfWork.RepactuacaoContrato.Add(repactuacao);
    
    foreach (var veiculo in veiculos)
    {
        veiculo.RepactuacaoVeiculoId = Guid.NewGuid();
        veiculo.RepactuacaoContratoId = repactuacao.RepactuacaoContratoId;
        _unitOfWork.RepactuacaoVeiculo.Add(veiculo);
    }
    
    _unitOfWork.Save();
    return Json(new { success = true });
}
```

---

## 📝 Notas Importantes

1. **Valor opcional** - `Valor` pode ser NULL se não houver valor específico.

2. **Relacionamento obrigatório** - Ambos `RepactuacaoContratoId` e `VeiculoId` são obrigatórios.

---

## 📋 Modificações Recentes

### [13/01/2026] - Correção de Warning CS8618

Adicionado `= null!` às propriedades de navegação virtual para eliminar warnings de compilação:
- `RepactuacaoContrato { get; set; } = null!;` (linha 21)
- `Veiculo { get; set; } = null!;` (linha 27)

Motivo: Propriedades de navegação EF Core são inicializadas pelo framework, mas o compilador não sabe disso.

---

**📅 Documentação criada em:** 08/01/2026
**📅 Última modificação:** 13/01/2026


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
