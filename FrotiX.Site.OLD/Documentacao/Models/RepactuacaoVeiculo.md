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
