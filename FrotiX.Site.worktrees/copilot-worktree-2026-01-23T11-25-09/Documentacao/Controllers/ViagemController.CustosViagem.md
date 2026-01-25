# Documentação: ViagemController.CustosViagem.cs

> **Última Atualização**: 14/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

Partial class do `ViagemController` responsável pelo endpoint de **detalhamento de custos** de uma viagem específica. Retorna informações completas sobre custos, duração, quilometragem e consumo estimado de combustível.

### Características Principais

- ✅ **Endpoint único**: `GET /api/Viagem/ObterCustosViagem?viagemId={guid}`
- ✅ **Cálculo de litros gastos**: Baseado em km percorrido / consumo médio do veículo
- ✅ **Custos detalhados**: Motorista, veículo, combustível, operador, lavador
- ✅ **Informações da viagem**: Requisitante, setor, motorista, veículo, datas/horários

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
└── Controllers/
    └── ViagemController.CustosViagem.cs  (partial class)
```

### Relacionamento

Este arquivo é uma **partial class** de `ViagemController.cs`, focado exclusivamente no endpoint de custos.

---

## Endpoint API

### GET `/api/Viagem/ObterCustosViagem`

**Descrição**: Retorna detalhamento completo dos custos de uma viagem

**Parâmetros**:

- `viagemId` (Guid): ID da viagem

**Response de Sucesso**:
```json
{
  "success": true,
  "data": {
    "viagemId": "guid",
    "requisitante": "Nome do Requisitante",
    "matricula": "12345",
    "setor": "Nome do Setor",
    "motorista": "Nome do Motorista",
    "veiculo": "Placa - Marca/Modelo",
    "dataInicial": "2024-05-29",
    "horaInicio": "11:30",
    "dataFinal": "2024-05-29",
    "horaFim": "12:28",
    "duracaoMinutos": 58,
    "duracaoFormatada": "58min",
    "kmPercorrido": 11,
    "litrosGastos": 1.1,
    "consumo": 10.0,
    "consumoFormatado": "10.00 km/l",
    "tipoCombustivel": "Diesel",
    "precoCombustivel": 5.89,
    "custoMotorista": 38.97,
    "custoVeiculo": 80.48,
    "custoCombustivel": 11.68,
    "custoOperador": 0,
    "custoLavador": 0,
    "custoTotal": 131.13
  }
}
```

---

## Lógica de Cálculo de Litros Gastos

### Fórmula Principal

```
litrosGastos = kmPercorrido / consumoVeiculo
```

Onde:
- `kmPercorrido` = KmFinal - KmInicial
- `consumoVeiculo` = Consumo médio do veículo em km/L (cadastro do veículo)

### Obtenção do Consumo do Veículo

1. **Prioridade 1**: Usa o campo `Consumo` do cadastro do veículo
2. **Prioridade 2**: Calcula média histórica baseada em abastecimentos (soma km / soma litros)

### Importante

**NÃO** se usa abastecimentos do período da viagem para calcular litros gastos, pois:
- Um abastecimento enche o tanque para múltiplas viagens
- Atribuir todo o combustível abastecido a uma única viagem gera valores absurdos

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [14/01/2026] - Correção do cálculo de Litros Gastos

**Descrição**:

- Removida lógica incorreta que buscava abastecimentos do período da viagem
- **Problema**: Sistema atribuía todos os litros abastecidos no dia à viagem, mesmo que fossem para múltiplas viagens futuras
- **Exemplo**: Viagem de 11km mostrava 67,53L gastos (valor do abastecimento do dia)
- **Solução**: Cálculo agora usa apenas `km percorrido / consumo médio do veículo`

**Arquivos Afetados**:

- `Controllers/ViagemController.CustosViagem.cs` (linhas 135-142)

**Impacto**: Modal de detalhamento de custos agora mostra valores corretos

**Status**: ✅ **Concluído**

**Versão**: 1.0

---

**Última atualização**: 14/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.0


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
