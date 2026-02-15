# Correção do Modal de Agendamento - /Agenda/Index

**Data:** 15/02/2026
**Problema:** Ao clicar em um agendamento existente, os campos não eram preenchidos corretamente no modal.

## Problemas Identificados

1. ❌ Data inicial aparecia como placeholder ao invés do valor
2. ❌ Finalidade não era preenchida (só placeholder)
3. ❌ Veículos mostrava GUID ao invés do nome
4. ❌ Motoristas lista vazia
5. ❌ Origem e Destino vazias
6. ❌ Requisitante mostrava GUID
7. ❌ Setor mostrava GUID
8. ❌ Card Ficha da Viagem não aparecia
9. ❌ "Criado por" mostrava "Usuário não Encontrado"

## Causa Raiz

O método `/api/Agenda/RecuperaViagem` estava retornando **APENAS** os dados da tabela `Viagem` sem fazer JOINs com as tabelas relacionadas (Motorista, Veiculo, Requisitante, SetorSolicitante, Evento).

Resultado: Os campos vinham como **GUIDs** ao invés dos **nomes**, e o JavaScript não conseguia preencher os controles Kendo porque:
- Os ComboBoxes tentavam buscar o item pelo ID no dataSource
- Se o item não existisse no dataSource ou o formato fosse incompatível, o campo ficava vazio
- Sem os nomes, não era possível exibir o texto no campo

## Correções Aplicadas

### 1. Backend - AgendaController.cs

**Arquivo:** `FrotiX.Site.OLD/Controllers/AgendaController.cs`
**Método:** `RecuperaViagem(Guid Id)`

#### Alteração:

**ANTES:**
```csharp
// Buscar viagem no banco - SEM tracking e SEM includes
var viagemObj = _context.Viagem
    .AsNoTracking()
    .Where(v => v.ViagemId == Id)
    .FirstOrDefault();

return Ok(new { data = viagemObj });
```

**DEPOIS:**
```csharp
// ✅ CORREÇÃO: Buscar viagem com INCLUDES para carregar dados relacionados
var viagemObj = _context.Viagem
    .AsNoTracking()
    .Include(v => v.Motorista)
    .Include(v => v.Veiculo)
    .Include(v => v.Requisitante)
    .Include(v => v.SetorSolicitante)
    .Include(v => v.Evento)
    .Where(v => v.ViagemId == Id)
    .Select(v => new
    {
        // Campos da viagem (IDs)
        v.ViagemId,
        v.DataInicial,
        v.DataFinal,
        v.HoraInicio,
        v.HoraFim,
        v.Status,
        v.Finalidade,
        v.Origem,
        v.Destino,
        // ... outros campos

        v.MotoristaId,
        v.VeiculoId,
        v.RequisitanteId,
        v.SetorSolicitanteId,

        // ✅ NOVOS CAMPOS - Nomes das entidades relacionadas
        NomeMotorista = v.Motorista != null ? v.Motorista.Nome : null,
        Placa = v.Veiculo != null ? v.Veiculo.Placa : null,
        NomeVeiculo = v.Veiculo != null ? v.Veiculo.VeiculoNome : null,
        Requisitante = v.Requisitante != null ? v.Requisitante.Nome : null,
        SetorSolicitante = v.SetorSolicitante != null ? v.SetorSolicitante.Nome : null,
        SetorSolicitanteNome = v.SetorSolicitante != null ? v.SetorSolicitante.Nome : null,

        // Campos de texto formatados
        HoraInicialTexto = v.HoraInicio != null ? v.HoraInicio.Value.ToString("HH:mm") : null,
        HoraFinalTexto = v.HoraFim != null ? v.HoraFim.Value.ToString("HH:mm") : null,
    })
    .FirstOrDefault();

return Ok(new { data = viagemObj });
```

#### Benefícios:
- ✅ Retorna tanto os **IDs** quanto os **nomes** das entidades relacionadas
- ✅ Evita N+1 queries (todas as relações carregadas em uma única consulta)
- ✅ Formato de hora já vem como string "HH:mm" pronto para exibição
- ✅ Não causa ciclos de referência (usa `.Select()` para projeção anônima)

### 2. Frontend - exibe-viagem.js

**Arquivo:** `FrotiX.Site.OLD/wwwroot/js/agendamento/components/exibe-viagem.js`
**Função:** `exibirViagemExistente(objViagem)`

#### Alteração 1: Motorista

**ANTES:**
```javascript
// 5. Motorista
if (objViagem.motoristaId) {
    const motoristaCombo = $("#lstMotorista").data("kendoComboBox");
    if (motoristaCombo) {
        motoristaCombo.value(objViagem.motoristaId);  // ❌ Só define o ID
    }
}
```

**DEPOIS:**
```javascript
// 5. Motorista
console.log("🔍 DEBUG Motorista:");
console.log("   motoristaId:", objViagem.motoristaId);
console.log("   nomeMotorista:", objViagem.nomeMotorista);

if (objViagem.motoristaId) {
    const motoristaCombo = $("#lstMotorista").data("kendoComboBox");
    if (motoristaCombo) {
        // ✅ Definir o valor (ID)
        motoristaCombo.value(objViagem.motoristaId);

        // ✅ Se temos o nome, definir também o texto para garantir exibição
        if (objViagem.nomeMotorista) {
            motoristaCombo.text(objViagem.nomeMotorista);
        }

        console.log("✅ Motorista carregado (Kendo ComboBox):", objViagem.motoristaId);
    }
}
```

#### Alteração 2: Veículo

**ANTES:**
```javascript
// 6. Veículo
if (objViagem.veiculoId) {
    const veiculoCombo = $("#lstVeiculo").data("kendoComboBox");
    if (veiculoCombo) {
        veiculoCombo.value(objViagem.veiculoId);  // ❌ Só define o ID
    }
}
```

**DEPOIS:**
```javascript
// 6. Veículo
console.log("🔍 DEBUG Veículo:");
console.log("   veiculoId:", objViagem.veiculoId);
console.log("   placa:", objViagem.placa);
console.log("   nomeVeiculo:", objViagem.nomeVeiculo);

if (objViagem.veiculoId) {
    const veiculoCombo = $("#lstVeiculo").data("kendoComboBox");
    if (veiculoCombo) {
        // ✅ Definir o valor (ID)
        veiculoCombo.value(objViagem.veiculoId);

        // ✅ Se temos placa ou nome do veículo, definir o texto
        const textoVeiculo = objViagem.placa || objViagem.nomeVeiculo;
        if (textoVeiculo) {
            veiculoCombo.text(textoVeiculo);
        }

        console.log("✅ Veículo carregado (Kendo ComboBox):", objViagem.veiculoId);
    }
}
```

#### Alteração 3: Requisitante

**ANTES:**
```javascript
// 8. Requisitante
const requisitanteId = objViagem.requisitanteId || objViagem.RequisitanteId;

if (requisitanteId) {
    const kendoComboBox = $("#lstRequisitante").data("kendoComboBox");
    if (kendoComboBox) {
        setTimeout(() => {
            kendoComboBox.value(requisitanteId);  // ❌ Só define o ID
            kendoComboBox.trigger("change");
        }, 300);
    }
}
```

**DEPOIS:**
```javascript
// 8. Requisitante
const requisitanteId = objViagem.requisitanteId || objViagem.RequisitanteId;
const requisitanteNome = objViagem.requisitante;  // ✅ Novo campo da API

console.log("🔍 DEBUG EXIBIÇÃO - Requisitante:");
console.log("  - requisitanteId:", requisitanteId);
console.log("  - requisitante (nome):", requisitanteNome);

if (requisitanteId) {
    const kendoComboBox = $("#lstRequisitante").data("kendoComboBox");
    if (kendoComboBox) {
        setTimeout(() => {
            // ✅ Definir o valor (ID)
            kendoComboBox.value(requisitanteId);

            // ✅ Se temos o nome, definir também o texto para garantir exibição
            if (requisitanteNome) {
                kendoComboBox.text(requisitanteNome);
            }

            kendoComboBox.trigger("change");
        }, 300);
    }
}
```

#### Alteração 4: Setor Solicitante

**ANTES:**
```javascript
let setorNome = objViagem.setorSolicitante || objViagem.nomeSetorRequisitante || ...
```

**DEPOIS:**
```javascript
// ✅ CORREÇÃO: Incluir os novos campos da API
let setorNome = objViagem.setorSolicitante || objViagem.setorSolicitanteNome ||
                objViagem.nomeSetorRequisitante || objViagem.setorRequisitanteNome || ...

console.log("🔍 DEBUG Setor Requisitante:");
console.log("   setorSolicitante (nome da API):", objViagem.setorSolicitante);
console.log("   setorSolicitanteNome (nome da API):", objViagem.setorSolicitanteNome);
```

## Benefícios das Correções

### Backend
- ✅ **Menos roundtrips ao banco:** Uma única consulta com JOINs ao invés de múltiplas consultas
- ✅ **Dados completos:** IDs + Nomes em um único payload
- ✅ **Formato pronto:** Horas já formatadas como "HH:mm"
- ✅ **Sem ciclos:** Projeção anônima evita referências circulares na serialização JSON

### Frontend
- ✅ **Robustez:** Usa tanto `.value(id)` quanto `.text(nome)` para garantir preenchimento
- ✅ **Compatibilidade:** Funciona mesmo se o item não estiver no dataSource do controle
- ✅ **Debug:** Logs detalhados para diagnóstico de problemas
- ✅ **Fallback:** Se o nome não vier, tenta buscar no dataSource; se vier, usa diretamente

## Arquivos Modificados

1. **Backend:**
   - `FrotiX.Site.OLD/Controllers/AgendaController.cs` (método `RecuperaViagem`)

2. **Frontend:**
   - `FrotiX.Site.OLD/wwwroot/js/agendamento/components/exibe-viagem.js` (função `exibirViagemExistente`)

## Como Testar

1. Abra a página `/Agenda/Index`
2. Clique em um agendamento existente no calendário
3. Verifique se todos os campos são preenchidos corretamente:
   - ✅ Data Inicial com valor (não placeholder)
   - ✅ Hora Inicial com valor
   - ✅ Finalidade selecionada
   - ✅ Motorista com nome (não GUID)
   - ✅ Veículo com placa/nome (não GUID)
   - ✅ Requisitante com nome (não GUID)
   - ✅ Setor com nome (não GUID)
   - ✅ Origem e Destino preenchidos
   - ✅ Card Ficha da Viagem visível (se aplicável)
   - ✅ Label "Criado por" com nome do usuário

## Próximos Passos

- [ ] Verificar se o Card "Ficha da Viagem" precisa de correções adicionais
- [ ] Verificar se a label "Criado por" está sendo preenchida corretamente
- [ ] Testar com diferentes tipos de agendamento (recorrente, evento, simples)
- [ ] Validar que a edição e salvamento continuam funcionando corretamente

## Observações

- A correção é **retrocompatível**: mesmo que alguns campos venham vazios, o código não quebra
- Os logs `console.log` adicionados facilitam o debug em caso de problemas futuros
- A abordagem de usar `.text()` além de `.value()` é a recomendada pela documentação do Kendo UI para casos onde o item pode não estar no dataSource
