# SUPER PROMPT - Correção do Sistema de Agendamento Recorrente FrotiX

## 📋 CONTEXTO GERAL

Estou trabalhando em um sistema ASP.NET Core (FrotiX) que tem uma página de Agenda (`Pages/Agenda/Index.cshtml`) com funcionalidade de agendamentos recorrentes. O sistema usa:
- **Backend**: ASP.NET Core com Razor Pages
- **Frontend**: jQuery, Syncfusion EJ2, Telerik/Kendo UI
- **Calendário**: FullCalendar

## 🎯 PROBLEMA PRINCIPAL

A funcionalidade de "Agendamento Recorrente" está parcialmente quebrada. Houve uma migração dos DatePickers de Syncfusion para Telerik/Kendo, e durante esse processo vários problemas surgiram.

---

## ✅ O QUE ESTÁ FUNCIONANDO

1. **DatePickers Telerik**: Substituídos com sucesso, calendários estão com tamanho adequado e em PT-BR
2. **Card de Recorrência**: Aparece tanto em Novo Agendamento quanto em Edição
3. **Detecção de recorrência**: O sistema detecta corretamente quando um agendamento é recorrente (via `recorrente === "S"`, `intervalo`, ou `recorrenciaViagemId`)

---

## ❌ O QUE ESTÁ QUEBRADO

### PROBLEMA 1: lstRecorrente mostra "Não" quando deveria mostrar "Sim"
**Localização**: `wwwroot/js/agendamento/components/exibe-viagem.js`
**Sintoma**: Na edição de agendamento recorrente, o dropdown `lstRecorrente` aparece como "Não" ao invés de "Sim", mesmo quando `objViagem.recorrente === "S"`
**Consequência**: Os controles de recorrência (período, dias, etc.) não aparecem porque a lógica depende de `lstRecorrente.value === "S"`

**Logs do console mostram**:
```
✅ RECORRENTE: Agendamento é RECORRENTE
   - Recorrente: S
   - Intervalo: D
   - RecorrenciaViagemId: 98c83775-3e16-44c4-2e0c-08de562679b0
✅ Card de Configurações de Recorrência visível
✅ lstRecorrente definido como 'Sim'  // <-- DIZ que definiu, mas não reflete na UI
```

**Código suspeito** (linhas ~1440-1450 de exibe-viagem.js):
```javascript
const lstRecorrente = document.getElementById("lstRecorrente");
if (lstRecorrente && lstRecorrente.ej2_instances && lstRecorrente.ej2_instances[0])
{
    lstRecorrente.ej2_instances[0].value = "S";
    lstRecorrente.ej2_instances[0].enabled = false;
    lstRecorrente.ej2_instances[0].dataBind();
    console.log("✅ lstRecorrente definido como 'Sim'");
}
```

**Possível causa**: O `dataBind()` pode não estar funcionando, ou algo está sobrescrevendo o valor depois.

---

### PROBLEMA 2: Lista de Dias da Semana (lstDias) vem vazia
**Localização**: `wwwroot/js/agendamento/components/recorrencia-logic.js` e `recorrencia-init.js`
**Sintoma**: Quando seleciona "Semanal" ou "Quinzenal", o controle de múltipla escolha de dias da semana (`lstDias`) aparece vazio - sem os chips de Segunda a Domingo
**Elemento**: `<ejs-multiselect id="lstDias">` ou similar

**Como deveria funcionar**:
- Deveria mostrar chips selecionáveis: Segunda, Terça, Quarta, Quinta, Sexta, Sábado, Domingo
- Alinhados verticalmente e horizontalmente no controller
- Permitir múltipla seleção

---

### PROBLEMA 3: Calendário Syncfusion de Dias Variados com erro de formato
**Localização**: `wwwroot/js/agendamento/components/recorrencia-logic.js` - função `inicializarCalendarioSyncfusion`
**Sintoma**: Ao selecionar "Dias Variados", aparece erro ao invés do calendário

**Erro exato**:
```
Erro Gerado em: ej2.min.js
Método: inicializarCalendarioSyncfusion
Linha: 10
Erro: Format options or type given must be invalid
```

**Stack trace**:
```
at si.dateFormat (ej2.min.js:10:69770)
at xi.getDateFormat (ej2.min.js:10:84716)
at xi.formatDate (ej2.min.js:10:85347)
at J_.titleUpdate (ej2.min.js:10:3437592)
```

**Possível causa**: Conflito de cultura/locale entre Kendo UI (pt-BR) e Syncfusion, ou formato de data inválido sendo passado para o Calendar Syncfusion.

---

## 📁 ARQUIVOS RELEVANTES

### JavaScript Principal
- `wwwroot/js/agendamento/main.js` - Ponto de entrada do sistema de agendamento
- `wwwroot/js/agendamento/components/exibe-viagem.js` - Exibe dados da viagem no modal (CRÍTICO)
- `wwwroot/js/agendamento/components/recorrencia-logic.js` - Lógica dos controles de recorrência
- `wwwroot/js/agendamento/components/recorrencia-init.js` - Inicialização dos dropdowns de recorrência
- `wwwroot/js/agendamento/components/modal-viagem-novo.js` - Controle do modal

### Razor Pages
- `Pages/Agenda/Index.cshtml` - Página principal com o modal e controles

### CSS
- `wwwroot/css/frotix.css` - Estilos globais (incluindo correções para Kendo DatePicker)

### Configuração
- `Pages/Shared/_ScriptsBasePlugins.cshtml` - Carregamento de scripts (Kendo, Syncfusion)

---

## 🔧 REGRA DE NEGÓCIO DO CARD DE RECORRÊNCIA

**REGRA SIMPLES**:
- Se `Recorrente = "S"` no banco → Card visível, controles **DESABILITADOS**, dados **PREENCHIDOS**
- Se `Recorrente = "N"` ou `null` → Card **INVISÍVEL**

**O usuário NÃO PODE transformar um agendamento não-recorrente em recorrente durante edição.**

---

## 🔍 DETALHES TÉCNICOS IMPORTANTES

### Formato dos dados do servidor
O objeto `objViagem` vem do servidor via AJAX e pode ter propriedades em **camelCase** (padrão JSON do .NET):
```javascript
{
    "recorrente": "S",           // NÃO "Recorrente"
    "intervalo": "D",            // NÃO "Intervalo"
    "recorrenciaViagemId": "...", // NÃO "RecorrenciaViagemId"
    "dataFinalRecorrencia": "2026-01-31T00:00:00"
}
```

**Correção já aplicada**: Em `exibe-viagem.js`, a detecção de recorrência agora usa:
```javascript
const recorrenteVal = objViagem.Recorrente || objViagem.recorrente;
const intervaloVal = objViagem.Intervalo || objViagem.intervalo;
```

### Componentes Syncfusion usados
- `ejs-dropdownlist` - lstRecorrente, lstPeriodos, lstDiasMes
- `ejs-multiselect` - lstDias (dias da semana)
- `ejs-calendar` - calDatasSelecionadas (para Dias Variados)

### Componentes Telerik/Kendo usados
- `kendo-datepicker` - txtDataInicial, txtDataFinal, txtDataInicioEvento, txtDataFimEvento, txtFinalRecorrencia

---

## 📝 HISTÓRICO DE TENTATIVAS

1. **Substituição dos DatePickers Syncfusion por Telerik** ✅ SUCESSO
2. **Configuração de cultura pt-BR para Kendo** ✅ SUCESSO
3. **CSS para largura do calendário Kendo** ✅ SUCESSO
4. **Correção de detecção PascalCase/camelCase** ⚠️ PARCIAL - detecta, mas não aplica corretamente
5. **Alteração global de objViagem.Intervalo** ❌ FALHOU - quebrou outros componentes

---

## 🎯 O QUE PRECISA SER FEITO

1. **Investigar por que `lstRecorrente.ej2_instances[0].value = "S"` não está funcionando**
   - Verificar se o componente está sendo reinicializado depois
   - Verificar se há conflito com `recorrencia-logic.js` que pode estar resetando

2. **Corrigir a lista lstDias (dias da semana)**
   - Verificar se o dataSource está sendo carregado
   - Verificar se o componente Syncfusion MultiSelect está inicializado corretamente

3. **Corrigir o erro de formato do Calendar Syncfusion**
   - Possível conflito de cultura entre Kendo (pt-BR) e Syncfusion
   - Verificar parâmetros passados para `inicializarCalendarioSyncfusion`

---

## 🔎 COMO DEBUGAR

Ao abrir o console do navegador e editar um agendamento recorrente, você verá:
```
🔍 DEBUG RECORRÊNCIA:
   - recorrenteValue: S
   - intervaloValue: D
   - recorrenciaViagemIdValue: 98c83775-...
✅ RECORRENTE: Agendamento é RECORRENTE
✅ Card de Configurações de Recorrência visível
✅ lstRecorrente definido como 'Sim'
✅ divPeriodo visível
✅ Período definido: D
```

Mas na tela, `lstRecorrente` mostra "Não" e os campos de recorrência não aparecem.

---

## 📚 COMANDOS ÚTEIS

```bash
# Ver histórico de commits relacionados
git log --oneline -20

# Reverter para versão estável anterior
git checkout a9122920 -- wwwroot/js/agendamento/components/exibe-viagem.js

# Buscar todas as referências a lstRecorrente
grep -rn "lstRecorrente" wwwroot/js/agendamento/

# Buscar todas as referências a lstDias
grep -rn "lstDias" wwwroot/js/agendamento/
```

---

## ⚠️ CUIDADOS

1. **NÃO fazer substituições globais** - já tentamos e quebrou mais coisas
2. **Testar AMBOS os cenários**: Novo Agendamento E Edição de Agendamento Recorrente
3. **O Card de Recorrência deve aparecer em Novo Agendamento** para o usuário poder criar agendamentos recorrentes
4. **Manter compatibilidade** com os outros componentes Syncfusion que ainda são usados

---

## 🏁 CRITÉRIO DE SUCESSO

1. ✅ Novo Agendamento: Card de Recorrência visível, controles habilitados
2. ✅ Edição de Não-Recorrente: Card de Recorrência invisível
3. ⬜ Edição de Recorrente: Card visível, lstRecorrente = "Sim", controles desabilitados e preenchidos
4. ⬜ Seleção Semanal/Quinzenal: lstDias aparece com os 7 dias da semana
5. ⬜ Seleção Dias Variados: Calendário Syncfusion aparece sem erro

---

**Por favor, analise os arquivos mencionados e proponha uma solução para cada um dos 3 problemas identificados.**
