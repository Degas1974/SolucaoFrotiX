# Documentação: validacao.js

> **Última Atualização**: 18/01/2026 01:35
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

## [18/01/2026 - 01:35] - Correção de detecção de texto do botão para edição de agendamento

**Descrição**: Corrigida detecção do texto do botão "Confirmar" para que agendamentos em edição não sejam erroneamente validados como viagens.

**Problema**:
- Ao editar um agendamento, o sistema pedia Combustível Inicial, Motorista e Veículo (campos de viagem)
- Isso ocorria porque o botão de salvar tinha texto "Confirmar", mas o código só verificava "Edita Agendamento" ou "Confirma Agendamento"
- Como "Confirmar" não estava na lista, `ehAgendamento` retornava `false`, acionando validação de campos de viagem

**Solução**:
```javascript
// ANTES (não reconhecia "Confirmar")
const ehAgendamento = btnTexto === "Edita Agendamento" || btnTexto === "Confirma Agendamento";

// DEPOIS (reconhece "Confirmar")
const ehAgendamento = btnTexto === "Edita Agendamento" || btnTexto === "Confirma Agendamento" || btnTexto === "Confirmar";
```

**Impacto**:
- ✅ Agendamentos em edição podem ser salvos sem preencher Motorista, Veículo e Combustível Inicial
- ✅ Validação correta de acordo com o tipo de operação (agendamento vs viagem)
- ✅ Removidos logs de debug que foram adicionados temporariamente para diagnóstico

**Arquivos Afetados**:
- `wwwroot/js/agendamento/components/validacao.js` (linhas 54-61)

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.3

---

## [18/01/2026 - 01:30] - Correção de validação de recorrência Mensal

**Descrição**: Corrigida validação de recorrência para exigir corretamente Dia do Mês quando período é Mensal, e separar validação de Dias da Semana para Semanal/Quinzenal.

**Problema**:
- Validação estava verificando Dias da Semana para período Mensal (incorreto)
- Não validava Dia do Mês para período Mensal (campo obrigatório não era validado)

**Solução**:
```javascript
// ANTES (validava Dias da Semana para Mensal - incorreto)
if ((periodo === "S" || periodo === "Q" || periodo === "M"))
{
    const diasSelecionados = document.getElementById("lstDias").ej2_instances[0].value;
    if (diasSelecionados === "" || diasSelecionados === null)
    {
        await Alerta.Erro("Informação Ausente", "Se o período foi escolhido como semanal, quinzenal ou mensal, você precisa escolher os Dias da Semana");
        return false;
    }
}

// DEPOIS (validações separadas e corretas)
// Validação 2: Semanal/Quinzenal → Dias da Semana obrigatório
if (periodo === "S" || periodo === "Q")
{
    const diasSelecionados = document.getElementById("lstDias").ej2_instances[0].value;
    if (!diasSelecionados || diasSelecionados.length === 0)
    {
        await Alerta.Erro("Informação Ausente", "Para período Semanal ou Quinzenal, você precisa escolher ao menos um Dia da Semana");
        return false;
    }
}

// Validação 3: Mensal → Dia do Mês obrigatório
if (periodo === "M")
{
    const diaMes = document.getElementById("lstDiasMes").ej2_instances[0].value;
    if (!diaMes || diaMes === "" || diaMes === null)
    {
        await Alerta.Erro("Informação Ausente", "Para período Mensal, você precisa escolher o Dia do Mês");
        return false;
    }
}
```

**Regras de Validação de Recorrência** (Atualizadas):

1. **Recorrência SIM** → Período **obrigatório**
2. **Período Diário** → Data Final Recorrência **obrigatória**
3. **Período Semanal** → Data Final Recorrência **obrigatória** + Dias da Semana **obrigatório** (ao menos um)
4. **Período Quinzenal** → Data Final Recorrência **obrigatória** + Dias da Semana **obrigatório** (ao menos um)
5. **Período Mensal** → Data Final Recorrência **obrigatória** + Dia do Mês **obrigatório**
6. **Período Dias Variados** → Ao menos **uma data selecionada** no calendário

**Arquivos Afetados**:
- `wwwroot/js/agendamento/components/validacao.js` (linhas 487-531)

**Impacto**:
- ✅ Validação correta para cada tipo de período
- ✅ Mensagens de erro específicas para cada caso
- ✅ Impossível criar recorrência Mensal sem Dia do Mês

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
*Versão*: 1.1
