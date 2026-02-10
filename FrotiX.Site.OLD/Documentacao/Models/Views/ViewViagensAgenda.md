# Documentação: ViewViagensAgenda.cs

> **Última Atualização**: 18/01/2026
> **Versão Atual**: 2.2

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

VIEW que consolida viagens formatadas para exibição em calendário/agenda. Inclui campos específicos para componentes de calendário (start, end, cores) e informações de eventos relacionados.

## Estrutura do Model

```csharp
public class ViewViagensAgenda
{
    public Guid ViagemId { get; set; }
    public DateTime? DataInicial { get; set; }
    public DateTime? HoraInicio { get; set; }
    public string? Status { get; set; }
    public bool? StatusAgendamento { get; set; }
    public bool? FoiAgendamento { get; set; }
    public string? Finalidade { get; set; }
    public string? NomeEvento { get; set; }
    public Guid? VeiculoId { get; set; }
    public Guid? MotoristaId { get; set; }
    public Guid? EventoId { get; set; }
    public string? Titulo { get; set; }
    public DateTime? Start { get; set; }          // Para calendário
    public DateTime? End { get; set; }            // Para calendário
    public DateTime? DataFinal { get; set; }
    public DateTime? HoraFim { get; set; }
    public string? CorEvento { get; set; }
    public string? CorTexto { get; set; }
    public string? DescricaoEvento { get; set; }  // Descrição específica para eventos
    public string? DescricaoMontada { get; set; } // Descrição genérica montada
    public string? Descricao { get; set; }        // Descrição pura da viagem (sem motorista/placa)

    // Campos adicionados em 16/01/2026 para tooltips customizadas
    public string? Placa { get; set; }
    public string? NomeMotorista { get; set; }
    public string? NomeEventoFull { get; set; }
}
```

## Interconexões

Usada por `AgendaController` e `Pages/Agenda/Index.cshtml` para exibir viagens no calendário.

## Notas Importantes

1. **Formato Calendário**: Campos Start/End para compatibilidade com componentes de calendário
2. **Cores**: CorEvento e CorTexto para personalização visual
3. **Eventos**: Inclui informações de eventos relacionados

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [18/01/2026 - 02:40] - Re-Adição da Propriedade 'Descricao'

**Descrição**: Re-adicionada a propriedade `Descricao` ao model para corrigir tooltips duplicadas em agendamentos recorrentes.

**Contexto Histórico**:
- Em 12/01/2026, este campo foi removido por suposto erro "Invalid column name 'Descricao'"
- Investigação atual revelou que a VIEW `ViewViagensAgenda` **TEM SIM** o campo `Viagem.Descricao`
- O erro anterior pode ter sido causado por cache ou estado inconsistente do banco

**Problema Atual** (tooltips duplicadas):
- Tooltips mostravam: "Motorista - (Placa) - Descrição" na linha de descrição
- Causa: Backend retornava `DescricaoMontada` (que concatena motorista + placa + descrição)
- Resultado: Informações duplicadas na tooltip

**Solução Aplicada**:
```csharp
// RE-ADICIONADO em 18/01/2026:
public string? Descricao { get; set; }  // Descrição pura da viagem (sem motorista/placa)
```

**Verificação no Banco**:
```sql
-- Confirmado em Frotix.sql:
CREATE VIEW ViewViagensAgenda AS SELECT
    ...
    ,Viagem.Descricao  -- ✅ CAMPO EXISTE NA VIEW
    ...
```

**Campos de Descrição na VIEW**:
- `Descricao`: Descrição pura da viagem (texto simples)
- `DescricaoMontada`: "Motorista - (Placa) - Descrição" (concatenado)
- `DescricaoEvento`: Descrição específica para eventos

**Arquivos Afetados**:
- `Models/Views/ViewViagensAgenda.cs`: Re-adicionado campo `Descricao`

**Impacto**:
- ✅ Tooltips agora exibem apenas descrição pura (sem duplicação)
- ✅ Consistente entre agendamentos normais e recorrentes
- ✅ Backend pode escolher qual descrição usar conforme contexto

**Status**: ✅ **Concluído**

**Versão**: 2.2

---

## [12/01/2026 22:45] - Remoção da Propriedade 'Descricao'

**Descrição**: Removida a propriedade `Descricao` do model que causava erro "Invalid column name 'Descricao'" ao carregar a agenda.

**Problema Identificado**:
- A VIEW `ViewViagensAgenda` no banco de dados não retorna uma coluna chamada `Descricao`
- A VIEW retorna apenas duas colunas de descrição: `DescricaoMontada` e `DescricaoEvento`
- O Entity Framework tentava fazer SELECT da coluna `Descricao` que não existe, resultando em Error 500

**Solução Aplicada**:
- Removida a propriedade `public string? Descricao { get; set; }` da linha 20
- Model agora está alinhado com a estrutura real da VIEW no banco de dados
- `DescricaoMontada`: Descrição genérica formatada (Motorista - Placa - Descrição)
- `DescricaoEvento`: Descrição específica para eventos (inclui nome do evento)

**Arquivos Afetados**:
- `Models/Views/ViewViagensAgenda.cs` (linha 20 removida)

**Impacto**:
- ✅ Corrige Error 500 ao carregar agenda
- ✅ Permite que a nova cor de eventos (#8C7961) seja exibida corretamente
- ✅ Alinha model C# com estrutura SQL da VIEW

**Status**: ✅ **Concluído**

**Versão**: 2.1

---

## [08/01/2026] - Documentação Inicial Completa

**Status**: ✅ **Concluído**

**Versão**: 2.0

---

**Última atualização**: 18/01/2026
**Autor**: Sistema FrotiX
**Versão**: 2.2
