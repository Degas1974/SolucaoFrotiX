# Documentação: UnitOfWork.OcorrenciaViagem.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O arquivo `UnitOfWork.OcorrenciaViagem.cs` é uma extensão parcial do `UnitOfWork` que adiciona repositories relacionados a ocorrências de viagens com lazy loading.

**Principais características:**

✅ **Partial Class**: Extensão do `UnitOfWork` principal  
✅ **Lazy Loading**: Repositories instanciados sob demanda  
✅ **Ocorrências de Viagem**: Repositories específicos para ocorrências

---

## Repositories Adicionados

### `OcorrenciaViagem`

**Descrição**: Repository para entidade `OcorrenciaViagem`

**Implementação**:
```csharp
public IOcorrenciaViagemRepository OcorrenciaViagem
{
    get
    {
        if (_ocorrenciaViagem == null)
            _ocorrenciaViagem = new OcorrenciaViagemRepository(_db);
        return _ocorrenciaViagem;
    }
}
```

**Lazy Loading**: Instanciado apenas quando acessado pela primeira vez

---

### `ViewOcorrenciasViagem`

**Descrição**: Repository para view `ViewOcorrenciasViagem`

**Uso**: Consultas otimizadas de ocorrências de viagens

---

### `ViewOcorrenciasAbertasVeiculo`

**Descrição**: Repository para view `ViewOcorrenciasAbertasVeiculo`

**Uso**: Consultas de ocorrências abertas agrupadas por veículo

---

## Interconexões

### Quem Usa Estes Repositories

- **OcorrenciaViagemController**: CRUD de ocorrências
- **DashboardVeiculosController**: Estatísticas de ocorrências
- **ViagemController**: Exibição de ocorrências em viagens

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do UnitOfWork.OcorrenciaViagem

**Arquivos Afetados**:
- `Repository/UnitOfWork.OcorrenciaViagem.cs`

**Impacto**: Documentação de referência para repositories de ocorrências

**Status**: ✅ **Concluído**

**Versão**: 2.0

---

**Última atualização**: 08/01/2026  
**Autor**: Sistema FrotiX  
**Versão**: 2.0


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
