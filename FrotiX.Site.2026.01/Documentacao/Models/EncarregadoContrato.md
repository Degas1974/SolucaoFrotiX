# Documentação: EncarregadoContrato.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 🎯 Objetivos

O Model `EncarregadoContrato` representa uma **tabela de relacionamento N-N** entre `Encarregado` e `Contrato` usando **chave primária composta** (ambos os IDs como chave).

**Principais objetivos:**

✅ Permitir que um encarregado esteja vinculado a múltiplos contratos  
✅ Permitir que um contrato tenha múltiplos encarregados  
✅ Usar chave composta para garantir unicidade da combinação  
✅ Simplificar estrutura sem necessidade de ID adicional

---

## 📁 Arquivos Envolvidos

- **`Models/EncarregadoContrato.cs`** - Model com chave composta
- **`Pages/Contrato/ItensContrato.cshtml`** - Interface de gestão de vínculos
- **`Controllers/ItensContratoController.cs`** - Endpoints para vincular/desvincular
- **`Data/FrotiXDbContext.cs`** - Configuração da chave composta

---

## 🏗️ Estrutura do Model

```csharp
public class EncarregadoContrato
{
    // ✅ Chave primária composta (2 Foreign Keys)
    [Key, Column(Order = 0)]
    public Guid EncarregadoId { get; set; }

    [Key, Column(Order = 1)]
    public Guid ContratoId { get; set; }
}
```

**Características:**
- ✅ Chave composta usando `[Key, Column(Order = ...)]`
- ✅ Sem propriedades adicionais (apenas relacionamento)
- ✅ Sem ID próprio (usa combinação dos dois IDs)

---

## 🗄️ Mapeamento Model ↔ Banco de Dados

```sql
CREATE TABLE [dbo].[EncarregadoContrato] (
    [EncarregadoId] UNIQUEIDENTIFIER NOT NULL,
    [ContratoId] UNIQUEIDENTIFIER NOT NULL,
    
    -- Chave primária composta
    CONSTRAINT [PK_EncarregadoContrato] 
        PRIMARY KEY ([EncarregadoId], [ContratoId]),
    
    -- Foreign Keys
    CONSTRAINT [FK_EncarregadoContrato_Encarregado] 
        FOREIGN KEY ([EncarregadoId]) REFERENCES [Encarregado]([EncarregadoId]) ON DELETE CASCADE,
    CONSTRAINT [FK_EncarregadoContrato_Contrato] 
        FOREIGN KEY ([ContratoId]) REFERENCES [Contrato]([ContratoId]) ON DELETE CASCADE
);
```

**Configuração no DbContext:**
```csharp
modelBuilder.Entity<EncarregadoContrato>()
    .HasKey(ec => new { ec.EncarregadoId, ec.ContratoId });
```

---

## 🔗 Quem Chama e Por Quê

### ItensContratoController.cs → Vincular Encarregado a Contrato

```csharp
[HttpPost("IncluirEncarregado")]
public IActionResult IncluirEncarregado([FromBody] ICIncluirEncarregadoContratoVM vm)
{
    // ✅ Verifica se já existe vínculo
    var existe = _unitOfWork.EncarregadoContrato
        .GetFirstOrDefault(ec => 
            ec.EncarregadoId == vm.EncarregadoId && 
            ec.ContratoId == vm.ContratoId);
    
    if (existe != null)
        return Json(new { success = false, message = "Encarregado já vinculado" });
    
    // ✅ Cria novo vínculo
    var encarregadoContrato = new EncarregadoContrato
    {
        EncarregadoId = vm.EncarregadoId,
        ContratoId = vm.ContratoId
    };
    
    _unitOfWork.EncarregadoContrato.Add(encarregadoContrato);
    _unitOfWork.Save();
    
    return Json(new { success = true });
}
```

---

## 🛠️ Problema → Solução → Código

### Problema: Evitar Duplicatas sem ID Próprio

**Solução:** Chave primária composta garante unicidade automaticamente.

```csharp
// ✅ Tentar adicionar duplicata resulta em erro de chave primária
try
{
    var novo = new EncarregadoContrato
    {
        EncarregadoId = encarregadoId,
        ContratoId = contratoId
    };
    _unitOfWork.EncarregadoContrato.Add(novo);
    _unitOfWork.Save();
}
catch (DbUpdateException ex)
{
    // ✅ SQL Server retorna erro de violação de chave primária
    if (ex.InnerException?.Message.Contains("PRIMARY KEY") == true)
    {
        // Vínculo já existe
    }
}
```

---

## 📝 Notas Importantes

1. **Chave composta** - Não precisa de `EncarregadoContratoId`, usa combinação dos dois IDs.

2. **CASCADE DELETE** - Se encarregado ou contrato for deletado, vínculos são removidos automaticamente.

3. **Sem propriedades extras** - Apenas relacionamento, sem campos adicionais como data de vinculação.

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
