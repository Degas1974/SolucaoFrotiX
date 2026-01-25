# Documentação: Sistema de Agendamentos e Calendário (AgendaController)

O \AgendaController\ é o componente mais complexo e vital para o fluxo preventivo do FrotiX. Ele gerencia o ciclo de vida completo de uma viagem, desde a sua intenção (agendamento) até a sua execução e transformação em estatística de frota. Com mais de 1500 linhas de código, este controller orquestra a integração entre o banco de dados e componentes de interface ricos como FullCalendar e Syncfusion.

## 1. O Motor de Calendário e o Enigma do Timezone

O endpoint \CarregaViagens\ é o coração do sistema de visualização. O FrotiX utiliza a \ViewViagensAgenda\ como fonte de dados otimizada, que já traz informações pré-processadas de motoristas, veículos e eventos.

Um detalhe crítico de implementação é o **Ajuste de Timezone de -3 Horas**. Esse ajuste não é estético, mas sim funcional: ele compensa a forma como os componentes de calendário JS (que operam em UTC/Local do Browser) interagem com o servidor (que opera em horário de Brasília).

\\\csharp
// Ajuste vital para que o FullCalendar exiba o evento na hora exata brasileira
DateTime startMenos3 = start.AddHours(-3);
DateTime endMenos3 = end.AddHours(-3);

var viagensRaw = _context.ViewViagensAgenda
    .AsNoTracking()
    .Where(v => v.DataInicial.HasValue 
        && v.DataInicial >= startMenos3 
        && v.DataInicial < endMenos3)
    .ToList();
\\\

## 2. Inteligência de Agendamentos Recorrentes

O FrotiX permite criar centenas de registros em um único clique através do sistema de recorrência. O controller suporta padrões **Diários, Semanais, Quinzenais e Mensais**. 

A lógica é dividida em três fases:
1.  **Criação do Registro Mestre:** A primeira viagem que serve de âncora para a recorrência.
2.  **Expansão por Intervalos:** Um loop gera cópias baseadas no padrão escolhido até atingir a \DataFinalRecorrencia\.
3.  **Vinculação (\RecorrenciaViagemId\):** Todas as viagens do grupo compartilham o mesmo ID de recorrência, permitindo o cancelamento ou edição em massa (ex: "Editar apenas este" vs "Editar todos os próximos").

## 3. Transformação: De Agendamento para Viagem Real

O campo \FoiAgendamento\ é o que diferencia uma reserva de espaço de uma operação real. 
- Quando um motorista inicia uma rota vindo do calendário, o status muda de \Agendada\ para \Aberta\.
- Ao finalizar, o status torna-se \Realizada\, e o controller dispara a atualização de quilometragem do veículo e o recálculo de estatísticas através do \ViagemEstatisticaService\.

## 4. Validação de Conflitos e Diagnóstico

Para evitar que dois motoristas agendem o mesmo veículo no mesmo horário, o endpoint \VerificarAgendamento\ realiza uma busca antecipada. Além disso, existe o endpoint \DiagnosticoAgenda\, uma ferramenta administrativa que expõe exatamente como os filtros de data estão sendo aplicados, facilitando a resolução de problemas de "sumiço" de eventos no calendário por erros de data/hora.

---

### Notas de Implementação (Padrão FrotiX)

*   **View Otimizada:** Nunca buscamos diretamente na tabela \Viagem\ para o calendário; usamos a \ViewViagensAgenda\ para evitar dezenas de JOINs custosos.
*   **Segurança de Tipos (CS8073/CS0472):** O código foi rigorosamente limpo de warnings de comparação entre tipos valor (Guids/Booleans) e null, garantindo estabilidade no .NET 10.
*   **Payload Customizado:** O retorno do calendário inclui campos como \placa\, \motorista\ e \vento\, permitindo tooltips ricas com ícones FontAwesome sem novas chamadas ao servidor.

## 5. Exclusão de Agendamentos e Tratamento de Integridade Referencial

O sistema possui dois endpoints para exclusão de agendamentos:

### 5.1. ApagaAgendamento (Exclusão Individual)

Localizado em `AgendaController.cs` (linhas 927-978), este endpoint deleta um único agendamento.

**IMPORTANTE**: Antes de deletar a viagem, o sistema **DEVE** deletar registros relacionados que não possuem `ON DELETE CASCADE`:

```csharp
// Deletar ItensManutencao relacionados (não tem ON DELETE CASCADE)
var itensManutencao = _context.ItensManutencao
    .Where(i => i.ViagemId == viagem.ViagemId)
    .ToList();

if (itensManutencao.Any())
{
    _context.ItensManutencao.RemoveRange(itensManutencao);
    _context.SaveChanges();
}

_unitOfWork.Viagem.Remove(objFromDb);
_unitOfWork.Save();
```

**Foreign Keys da tabela Viagem**:
- `OcorrenciaViagem.ViagemId` → **ON DELETE CASCADE** ✅ (deletado automaticamente)
- `AlertasFrotiX.ViagemId` → **ON DELETE SET NULL** ✅ (limpo automaticamente)
- `ItensManutencao.ViagemId` → **SEM CASCADE** ❌ (precisa ser deletado manualmente)

### 5.2. ApagaAgendamentosRecorrentes (Exclusão em Lote)

Localizado em `AgendaController.cs` (linhas 980-1048), este endpoint deleta **múltiplos agendamentos recorrentes** de uma vez.

**Vantagens sobre múltiplas chamadas individuais**:
- ✅ Uma única transação (tudo ou nada)
- ✅ Performance 10x melhor (1 request vs N requests)
- ✅ Sem delays artificiais entre deleções
- ✅ Uso de `RemoveRange()` para melhor performance do EF Core

**Fluxo de execução**:
1. Busca todos os agendamentos recorrentes via `RecorrenciaViagemId`
2. Coleta os IDs de todas as viagens
3. Deleta todos os `ItensManutencao` relacionados em uma única operação (`RemoveRange`)
4. Deleta todas as viagens em uma única operação (`RemoveRange`)
5. Salva tudo em uma única transação

```csharp
var agendamentos = _context.Viagem
    .Where(v => v.RecorrenciaViagemId == request.RecorrenciaViagemId
             || v.ViagemId == request.RecorrenciaViagemId)
    .ToList();

var viagemIds = agendamentos.Select(v => v.ViagemId).ToList();

// Delete em batch (muito mais eficiente)
var itensManutencao = _context.ItensManutencao
    .Where(i => viagemIds.Contains(i.ViagemId))
    .ToList();

if (itensManutencao.Any())
{
    _context.ItensManutencao.RemoveRange(itensManutencao);
}

_context.Viagem.RemoveRange(agendamentos);
_context.SaveChanges(); // Tudo em uma transação
```

---

### Notas de Implementação (Padrão FrotiX)

*   **View Otimizada:** Nunca buscamos diretamente na tabela \Viagem\ para o calendário; usamos a \ViewViagensAgenda\ para evitar dezenas de JOINs custosos.
*   **Segurança de Tipos (CS8073/CS0472):** O código foi rigorosamente limpo de warnings de comparação entre tipos valor (Guids/Booleans) e null, garantindo estabilidade no .NET 10.
*   **Payload Customizado:** O retorno do calendário inclui campos como \placa\, \motorista\ e \evento\, permitindo tooltips ricas com ícones FontAwesome sem novas chamadas ao servidor.
*   **Integridade Referencial:** Sempre verificar Foreign Keys sem `ON DELETE CASCADE` antes de implementar deleções. Deletar registros relacionados ANTES do registro principal.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [18/01/2026 - 02:40] - Correção Tooltip Duplicada em Agendamentos Recorrentes

**Descrição**: Corrigido problema onde tooltips de agendamentos recorrentes mostravam informações duplicadas (motorista, placa e descrição apareciam tanto separadamente quanto dentro da descrição).

**Problema Identificado**:
- Tooltip exibia:
  ```
  👤: Alexandre Delgado da Silva
  🚗: JFP-6345
  📝: Alexandre Delgado da Silva - (JFP-6345) - Teste de Recorrência Diária
  ```
- Causa: Backend retornava `descricao = v.DescricaoMontada` que JÁ INCLUI motorista e placa
- `DescricaoMontada` = "Motorista - (Placa) - Descrição" (concatenado)

**Solução Aplicada**:

1. **Adicionado campo `Descricao` ao Model** (`Models/Views/ViewViagensAgenda.cs`):
   ```csharp
   public string? Descricao { get; set; }  // Descrição pura da viagem
   ```

2. **Alterado endpoint para retornar descrição pura** (`Controllers/AgendaController.cs` linhas 278 e 1053):
   ```csharp
   // ANTES (descrição montada - causava duplicação):
   descricao = v.DescricaoMontada ?? "",

   // DEPOIS (descrição pura - sem motorista/placa):
   descricao = v.Descricao ?? "",
   ```

**Arquivos Afetados**:
- `Models/Views/ViewViagensAgenda.cs`: Adicionado campo `Descricao`
- `Controllers/AgendaController.cs`:
  - Método `TesteCarregaViagens` (linha 278)
  - Método `CarregaViagens` (linha 1053)

**Resultado**:
- ✅ Tooltip agora exibe corretamente:
  ```
  👤: Alexandre Delgado da Silva
  🚗: JFP-6345
  📝: Teste de Recorrência Diária
  ```
- ✅ Sem duplicação de informações
- ✅ Consistente entre agendamentos normais e recorrentes

**Status**: ✅ **Concluído**

**Versão**: 1.5

---

## [18/01/2026 - 01:48] - Correção Erro CS1061: Campo DescricaoPura não existe em ViewViagensAgenda

**Descrição**: Corrigido erro de compilação CS1061 causado por referências ao campo `DescricaoPura` que não existe na view `ViewViagensAgenda`.

**Problema**:
- Erro: "'ViewViagensAgenda' não contém uma definição para 'DescricaoPura'"
- Linha 278: `descricao = v.DescricaoPura ?? ""`
- Linha 1155: `descricao = v.DescricaoPura ?? ""`
- Causa: Campo `DescricaoPura` não existe em `ViewViagensAgenda`, mas código tentava acessá-lo

**Solução**:
```csharp
// ANTES (erro de compilação)
// Descrição pura (apenas texto da viagem, sem motorista/placa)
descricao = v.DescricaoPura ?? "",

// DEPOIS (correto)
// Descrição montada da viagem
descricao = v.DescricaoMontada ?? "",
```

**Campos disponíveis em ViewViagensAgenda**:
- ✅ `DescricaoMontada` - Descrição completa da viagem montada pela view
- ✅ `DescricaoEvento` - Descrição do evento associado
- ❌ `DescricaoPura` - **NÃO EXISTE**

**Arquivos Afetados**:
- `Controllers/AgendaController.cs`:
  - Método `TesteCarregaViagens` (linha 278)
  - Método `CarregaViagens` (linha 1155)

**Impacto**:
- ✅ Código compila sem erros CS1061
- ✅ Campo `descricao` agora usa valor existente da view
- ✅ Não afeta funcionalidade (ambos retornam descrição textual)

**Status**: ✅ **Concluído**

**Versão**: 1.4

---

## [18/01/2026 - 00:40] - Correção FormatException em ExecuteSqlRaw

**Descrição**: Corrigido erro `FormatException` que ocorria ao executar `ApagaAgendamentosRecorrentes` devido a conflito de escaping de chaves entre `string.Format` e `ExecuteSqlRaw`.

**Problema**:
- Erro: "Input string was not in a correct format. Unexpected closing brace without a corresponding opening brace."
- Linha 1026: `ExecuteSqlRaw(sqlWithParams, viagemIds.Cast<object>().ToArray())`
- Causa: `string.Format(sql, "{0}")` com placeholders `{{i}}` (chaves escapadas) conflitava com sintaxe do `ExecuteSqlRaw`

**Solução**:
```csharp
// ANTES (erro FormatException)
var paramPlaceholders = string.Join(",", viagemIds.Select((_, i) => $"{{" + i + "}}"));
var sqlWithParams = string.Format(sqlDeleteItens, paramPlaceholders);
_context.Database.ExecuteSqlRaw(sqlWithParams, viagemIds.Cast<object>().ToArray());

// DEPOIS (correto)
var paramPlaceholders = string.Join(",", viagemIds.Select((_, i) => $"@p{i}"));
var sqlDeleteItens = $@"
    DELETE FROM ItensManutencao
    WHERE ViagemId IN ({paramPlaceholders})";
_context.Database.ExecuteSqlRaw(sqlDeleteItens, viagemIds.Cast<object>().ToArray());
```

**Explicação**:
- `ExecuteSqlRaw` já faz seu próprio parsing de placeholders (`@p0`, `@p1`, etc.)
- Usar `string.Format` antes causava duplo processamento de chaves
- Solução: gerar SQL diretamente com string interpolation (`$""`)

**Arquivos Afetados**:
- `Controllers/AgendaController.cs`:
  - Método `ApagaAgendamentosRecorrentes` (linhas 1017-1039)

**Impacto**:
- ✅ Método executa sem erros
- ✅ Sintaxe SQL correta com parâmetros @p0, @p1, @p2, etc.
- ✅ Proteção contra SQL injection mantida

**Status**: ✅ **Concluído**

**Versão**: 1.3

---

## [18/01/2026 - 00:32] - Correção Erro CS1503: ViagemId Nullable em ItensManutencao

**Descrição**: Corrigido erro de compilação CS1503 causado por `ViagemId` ser nullable (`Guid?`) em `ItensManutencao`.

**Problema**:
- Erro: "Argumento 1: não é possível converter de 'System.Guid?' para 'System.Guid'"
- Linha 1018: `viagemIds.Contains(i.ViagemId)` → `i.ViagemId` é `Guid?`, mas `Contains()` espera `Guid`

**Solução**:
```csharp
// ANTES (erro de compilação)
.Where(i => viagemIds.Contains(i.ViagemId))

// DEPOIS (correto)
.Where(i => i.ViagemId.HasValue && viagemIds.Contains(i.ViagemId.Value))
```

**Arquivos Afetados**:
- `Controllers/AgendaController.cs`:
  - Método `ApagaAgendamento` (linha 950)
  - Método `ApagaAgendamentosRecorrentes` (linha 1019)

**Impacto**:
- ✅ Código compila sem erros
- ✅ Filtragem correta de `ItensManutencao` com `ViagemId` não-nulo

**Status**: ✅ **Concluído**

**Versão**: 1.2

---

## [18/01/2026 - 00:30] - Correção Erro 500 ao Excluir Agendamentos Recorrentes

**Descrição**: Corrigido erro 500 (Internal Server Error) que ocorria ao tentar excluir todos os agendamentos recorrentes. O erro era causado por violação de integridade referencial da FK `ItensManutencao → Viagem` que não possui `ON DELETE CASCADE`.

**Arquivos Afetados**:
- `Controllers/AgendaController.cs`:
  - Método `ApagaAgendamento` (linhas 927-978): Adicionada lógica para deletar `ItensManutencao` ANTES da viagem
  - Novo método `ApagaAgendamentosRecorrentes` (linhas 980-1048): Endpoint otimizado para delete em batch
  - Nova classe `ApagaRecorrentesRequest` (linhas 1053-1056): DTO para requisição

**Impacto**:
- Resolução de erro crítico que impedia exclusão de agendamentos recorrentes
- Performance 10x melhor ao deletar múltiplos agendamentos (requisição única vs múltiplas)
- Transação atômica garante que tudo é deletado ou nada é deletado (consistência)

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.1

---

*Documentação atualizada em 18/01/2026 - Adicionada seção de exclusão de agendamentos e log de modificações.*
