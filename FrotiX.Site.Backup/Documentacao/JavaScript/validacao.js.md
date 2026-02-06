# Documentação: validacao.js

> **Última Atualização**: 18/01/2026
> **Versão Atual**: 1.3

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

**Descrição**: Sistema de validação de formulários de agendamentos e viagens. Centraliza todas as regras de negócio para garantir que os dados estão corretos antes de enviar ao servidor.

### Características Principais

- ✅ **Validação em camadas**: Campos obrigatórios, formatos, regras de negócio
- ✅ **Validação contextual**: Regras diferentes para agendamentos vs viagens abertas
- ✅ **Feedback visual**: Alertas SweetAlert customizados
- ✅ **Validação assíncrona**: Suporte a confirmações do usuário

---

## Arquitetura

### Tecnologias Utilizadas

| Tecnologia | Uso |
|------------|-----|
| JavaScript ES6 | Classes e async/await |
| jQuery | Manipulação DOM |
| Syncfusion EJ2 | Acesso aos componentes |
| SweetAlert | Alertas customizados |

---

## Classe Principal: ValidadorAgendamento

**Localização**: Linha 8 do arquivo `validacao.js`

**Propósito**: Encapsular todas as validações do formulário de agendamento/viagem

### Campos Obrigatórios em AGENDAMENTOS

- Data Inicial
- Hora Inicial
- Finalidade
- Origem
- Destino
- Requisitante
- Ramal
- Setor do Requisitante

### Campos Obrigatórios APENAS em VIAGENS ABERTAS/REALIZADAS

- Motorista (quando transforma em viagem)
- Veículo (quando transforma em viagem)
- Combustível Inicial (quando transforma em viagem)
- KM Inicial (quando transforma em viagem)

---

## Método Principal: validar()

**Localização**: Linha 20

**Propósito**: Método orquestrador que chama todas as validações na ordem correta

**Fluxo de Execução**:

1. Valida data inicial
2. Valida finalidade, origem e destino
3. Valida campos de finalização (se preenchidos)
4. **Valida campos de viagem** (APENAS se não for agendamento OU se está transformando em viagem)
5. Valida requisitante, ramal e setor
6. Valida evento (se finalidade = "Evento")
7. Valida recorrência

---

## Lógica de Validação Condicional

**Localização**: Linhas 49-61

**Regra**: Motorista, Veículo, KM e Combustível NÃO são obrigatórios em agendamentos

```javascript
const btnTexto = $("#btnConfirma").text().trim();
const ehAgendamento = btnTexto === "Edita Agendamento" || btnTexto === "Confirma Agendamento";

// Se NÃO for agendamento OU se tem campos de finalização preenchidos
if (!ehAgendamento || algumFinalPreenchido)
{
    if (!await this.validarCamposViagem()) return false;
}
```

**Quando `validarCamposViagem()` é chamada**:

- ✅ Se NÃO for agendamento (já é viagem aberta/realizada)
- ✅ Se algum campo de finalização foi preenchido (está transformando agendamento em viagem)

**Quando `validarCamposViagem()` NÃO é chamada**:

- ❌ Se for agendamento E não tem campos de finalização

---

## Métodos de Validação Individual

### validarFinalidade()

- **Linha**: 124
- **Obrigatório**: Sempre

### validarOrigem() / validarDestino()

- **Linhas**: 147, 170
- **Obrigatório**: Sempre

### validarCamposViagem()

- **Linha**: 274
- **Obrigatório**: Apenas em viagens (não em agendamentos)
- **Valida**: Motorista, Veículo, KM Inicial, Combustível Inicial

### validarRequisitante()

- **Linha**: 320
- **Obrigatório**: Sempre

### validarRamal()

- **Linha**: 348
- **Obrigatório**: Sempre

### validarSetor()

- **Linha**: 393
- **Obrigatório**: Sempre

---

## Interconexões

### Quem Chama Este Arquivo

- `main.js` → Chama `validar()` antes de salvar agendamento/viagem
- `modal-viagem-novo.js` → Usa validação ao clicar em confirmar

### O Que Este Arquivo Chama

- `Alerta.Erro()` → Exibe mensagens de erro
- `Alerta.Confirmar()` → Solicita confirmação do usuário
- Componentes Syncfusion/Kendo → Lê valores dos campos

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [18/01/2026 - 15:40] - Texto readonly apenas em edição

**Descrição**: Ajuste para que `validarPeriodoRecorrencia()` use `txtFinalRecorrenciaTexto` somente quando `carregandoViagemExistente` estiver ativo. Em novo agendamento, a validação segue no DatePicker.

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/validacao.js`

**Impacto**:

- ✅ Edição usa texto readonly quando o DatePicker está oculto
- ✅ Novo agendamento exige DatePicker preenchido

**Status**: ✅ **Concluído**

**Versão**: 1.3

---

## [18/01/2026 - 15:30] - Validação com texto readonly em edição

**Descrição**: `validarPeriodoRecorrencia()` passou a ler o valor de `txtFinalRecorrenciaTexto` quando o DatePicker está oculto durante edição de recorrências.

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/validacao.js`

**Impacto**:

- ✅ Evita alerta de “Data Final” em edições onde o DatePicker está oculto

**Status**: ✅ **Concluído**

**Versão**: 1.2

---

## [18/01/2026 - 01:04] - Ajuste de validação condicional para agendamentos

**Descrição**: Corrigida lógica de validação para que Motorista, Veículo, KM e Combustível NÃO sejam obrigatórios em agendamentos.

**Problema**:

- Validação exigia campos de viagem em agendamentos
- Usuário não conseguia salvar agendamento sem preencher Motorista/Veículo

**Solução**:

```javascript
// ANTES (sempre validava campos de viagem se viagemId existisse)
if (viagemId && viagemId !== "" && $("#btnConfirma").text() !== " Edita Agendamento")
{
    if (!await this.validarCamposViagem()) return false;
}

// DEPOIS (só valida se NÃO for agendamento OU se está transformando em viagem)
const btnTexto = $("#btnConfirma").text().trim();
const ehAgendamento = btnTexto === "Edita Agendamento" || btnTexto === "Confirma Agendamento";

if (!ehAgendamento || algumFinalPreenchido)
{
    if (!await this.validarCamposViagem()) return false;
}
```

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/validacao.js` (linhas 49-61)

**Impacto**:

- ✅ Agendamentos podem ser salvos sem Motorista/Veículo
- ✅ Validação de viagem só ocorre quando apropriado
- ✅ Regras de negócio alinhadas com requisitos

**Status**: ✅ **Concluído**

**Versão**: 1.1

---

*Documentação criada em 18/01/2026*
*Autor*: Sistema FrotiX
*Versão*: 1.3


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
