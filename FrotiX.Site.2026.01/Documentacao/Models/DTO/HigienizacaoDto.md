# Documentação: HigienizacaoDto.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Estrutura do Model](#estrutura-do-model)
5. [Interconexões](#interconexões)
6. [Exemplos de Uso](#exemplos-de-uso)

---

## Visão Geral

O arquivo `HigienizacaoDto.cs` contém DTOs usados para operações de higienização (normalização) de dados de origem e destino de viagens. Permite consolidar valores duplicados ou similares em um único valor padronizado.

**Principais características:**

✅ **Higienização de Dados**: Normaliza valores de origem/destino  
✅ **Consolidação**: Agrupa valores antigos em um novo valor  
✅ **Bulk Operations**: Suporta operações em lote  
✅ **DTOs Específicos**: DTOs separados para origem e destino

### Objetivo

Os DTOs de `HigienizacaoDto` resolvem o problema de:
- Normalizar valores de origem/destino duplicados
- Consolidar variações do mesmo local
- Facilitar limpeza de dados históricos
- Melhorar qualidade dos dados para relatórios

---

## Arquitetura

### Tecnologias Utilizadas

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| ASP.NET Core | 5.0+ | APIs de higienização |

---

## Estrutura de Arquivos

### Arquivo Principal
```
Models/DTO/HigienizacaoDto.cs
```

### Arquivos Relacionados
- Controllers de higienização de dados
- Services de normalização de texto

---

## Estrutura do Model

```csharp
public class HigienizacaoDto
{
    public string Tipo { get; set; }               // "origem" ou "destino"
    public List<string> AntigosValores { get; set; }
    public string NovosValores { get; set; }
}

public class CorrecaoOrigemDto
{
    public List<string> Origens { get; set; }
    public string NovaOrigem { get; set; }
}

public class CorrecaoDestinoDto
{
    public List<string> Destinos { get; set; }
    public string NovoDestino { get; set; }
}
```

**Classes:**

- `HigienizacaoDto`: DTO genérico para higienização
- `CorrecaoOrigemDto`: DTO específico para correção de origens
- `CorrecaoDestinoDto`: DTO específico para correção de destinos

**Propriedades:**

- `Tipo` (string): "origem" ou "destino"
- `AntigosValores` (List<string>): Lista de valores a serem substituídos
- `NovosValores` (string): Valor único que substituirá os antigos
- `Origens`/`Destinos` (List<string>): Valores a serem consolidados
- `NovaOrigem`/`NovoDestino` (string): Valor consolidado

---

## Interconexões

### Quem Chama Este Arquivo

#### 1. **Controllers de Higienização** → Processa Correções

**Quando**: Administrador executa higienização de dados  
**Por quê**: Normalizar valores duplicados

```csharp
[HttpPost]
[Route("HigienizarOrigens")]
public IActionResult HigienizarOrigens([FromBody] CorrecaoOrigemDto dto)
{
    foreach (var origemAntiga in dto.Origens)
    {
        var viagens = _unitOfWork.Viagem
            .GetAll(v => v.Origem == origemAntiga);
        
        foreach (var viagem in viagens)
        {
            viagem.Origem = dto.NovaOrigem;
            _unitOfWork.Viagem.Update(viagem);
        }
    }
    
    _unitOfWork.Save();
    return Ok(new { success = true });
}
```

---

## Exemplos de Uso

### Cenário 1: Consolidar Origens Similares

**Situação**: Existem variações do mesmo local ("Câmara", "Camara", "CAMARA")

**Código**:
```csharp
var dto = new CorrecaoOrigemDto
{
    Origens = new List<string> { "Camara", "CAMARA", "Câmara dos Deputados" },
    NovaOrigem = "Câmara dos Deputados"
};

// API consolida todas as variações
```

**Resultado**: Todas as viagens com origens na lista são atualizadas para o valor consolidado

---

## Notas Importantes

1. **Bulk Operations**: Operações em lote para melhor performance
2. **Validação**: Validar se valores existem antes de consolidar
3. **Auditoria**: Considerar log de alterações

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação de documentação do arquivo `HigienizacaoDto`

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
