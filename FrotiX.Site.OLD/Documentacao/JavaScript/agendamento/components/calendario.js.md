# Documentação: calendario.js

> **Última Atualização**: 17/01/2026 10:00
> **Versão Atual**: 1.2

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O arquivo `calendario.js` gerencia a inicialização e configuração do calendário FullCalendar v6 na página de Agenda. É responsável por renderizar eventos, configurar interações do usuário e aplicar tooltips customizadas.

**Principais características:**

✅ **Integração FullCalendar**: Inicializa calendário com configurações localizadas em PT-BR
✅ **Tooltips Dinâmicas**: Gera tooltips HTML com ícones e cores adaptativas
✅ **Manipulação de Cores**: Funções para clarear cores e detectar luminância
✅ **Carregamento de Eventos**: Busca eventos via API `/api/Agenda/CarregaViagens`
✅ **Timezone**: Compatível com timezone brasileiro (-3h UTC)

---

## Funções Principais

### lightenColor(color, percent)

**Descrição**: Clareia uma cor hexadecimal em uma porcentagem específica.

**Parâmetros**:
- `color` (string): Cor em formato hex (#RRGGBB)
- `percent` (number): Porcentagem para clarear (0-100)

**Retorno**: (string) Cor clareada em formato hex

**Algoritmo**:
1. Converte hex para RGB
2. Para cada componente (R, G, B):
   - Calcula: `novo = atual + (255 - atual) * (percent / 100)`
   - Garante: `novo <= 255`
3. Converte RGB de volta para hex

**Exemplo**:
```javascript
lightenColor('#3D5771', 20); // Retorna '#6480A0' (20% mais claro)
```

---

### isColorDark(color)

**Descrição**: Detecta se uma cor é escura usando a fórmula de luminância relativa W3C.

**Parâmetros**:
- `color` (string): Cor em formato hex (#RRGGBB)

**Retorno**: (boolean) `true` se a cor for escura

**Algoritmo**:
1. Converte hex para RGB
2. Calcula luminância: `L = (0.299 * R + 0.587 * G + 0.114 * B) / 255`
3. Retorna: `L < 0.5`

**Uso**: Define se o texto da tooltip deve ser branco (cor escura) ou preto (cor clara).

**Exemplo**:
```javascript
isColorDark('#3D5771'); // Retorna true (cor escura, texto branco)
isColorDark('#A0D0FF'); // Retorna false (cor clara, texto preto)
```

---

### gerarTooltipHTML(event)

**Descrição**: Gera HTML da tooltip com ícones FontAwesome e quebras de linha.

**Parâmetros**:
- `event` (object): Objeto do evento FullCalendar com `extendedProps`

**Estrutura esperada de `event.extendedProps`**:
```javascript
{
  placa: "ABC-1234",
  motorista: "João da Silva",
  evento: "Reunião Anual",
  finalidade: "Evento",
  descricao: "Descrição adicional"
}
```

**Retorno**: (string) HTML formatado

**Formato de Saída**:

```html
<i class="fa-duotone fa-tent"></i>: Reunião Anual<br>
<i class="fa-duotone fa-user-tie"></i>: João da Silva<br>
<i class="fa-duotone fa-car"></i>: ABC-1234<br>
<i class="fa-duotone fa-memo-pad"></i>: Descrição adicional
```

**Nota**: A ordem é Evento → Motorista → Veículo → Descrição. O formato inclui dois pontos (`:`) após cada ícone para melhor legibilidade.

**Regras**:
1. **Veículo** (fa-car): Sempre exibido
   - Se `placa` vazia: "Veículo não Informado"
2. **Motorista** (fa-user-tie): Exibido se preenchido
3. **Evento** (fa-tent): Exibido APENAS se `finalidade === "Evento"` e `evento` preenchido
4. **Descrição** (fa-memo-pad): Exibido se preenchido
   - Remove " - " do final se existir
5. Remove `<br>` final se existir

---

### window.InitializeCalendar(URL)

**Descrição**: **FUNÇÃO PRINCIPAL** - Inicializa o calendário FullCalendar na página de Agenda.

**Parâmetros**:
- `URL` (string): Endpoint para carregar eventos (ex: `/api/Agenda/CarregaViagens`)

**Configurações do Calendário**:

#### Toolbar
```javascript
headerToolbar: {
  left: "prev,next today",
  center: "title",
  right: "dayGridMonth,timeGridWeek,timeGridDay"
}
```

#### Botões Traduzidos
```javascript
buttonText: {
  today: "Hoje",
  dayGridMonth: "mensal",
  timeGridWeek: "semanal",
  timeGridDay: "diário"
}
```

#### Eventos

**eventClick**: Abre modal com detalhes do agendamento

**eventDidMount**: **CRÍTICO** - Aplicação de tooltips customizadas
1. Gera HTML com `gerarTooltipHTML(info.event)`
2. Obtém cor de fundo: `info.event.backgroundColor`
3. Clareia em 20%: `lightenColor(bgColor, 20)`
4. Define cor do texto: `isColorDark(lightColor) ? '#FFFFFF' : '#000000'`
5. Cria tooltip Bootstrap com classe `tooltip-ftx-agenda-dinamica`
6. Listener `shown.bs.tooltip`: Aplica cores dinamicamente ao tooltip-inner

**Código**:
```javascript
eventDidMount: function (info) {
    const tooltipHTML = gerarTooltipHTML(info.event);
    const bgColor = info.event.backgroundColor || '#808080';
    const lightColor = lightenColor(bgColor, 20);
    const textColor = isColorDark(lightColor) ? '#FFFFFF' : '#000000';

    new bootstrap.Tooltip(info.el, {
        html: true,
        title: tooltipHTML,
        customClass: 'tooltip-ftx-agenda-dinamica',
        trigger: 'hover'
    });

    info.el.addEventListener('shown.bs.tooltip', function() {
        const tooltipElement = document.querySelector('.tooltip-ftx-agenda-dinamica .tooltip-inner');
        if (tooltipElement) {
            tooltipElement.style.backgroundColor = lightColor;
            tooltipElement.style.color = textColor;
        }
    });
}
```

---

## Interconexões

### Quem Chama Este Arquivo
- `Pages/Agenda/Index.cshtml` → Carrega via `<script src="~/js/agendamento/components/calendario.js">`
- `main.js` → Chama `InitializeCalendar('/api/Agenda/CarregaViagens')`

### O Que Este Arquivo Chama
- **FullCalendar API**: `new FullCalendar.Calendar()`
- **Bootstrap Tooltip**: `new bootstrap.Tooltip()`
- **AgendaController**: Requisições AJAX para `/api/Agenda/CarregaViagens`
- **Alerta.js**: `Alerta.TratamentoErroComLinha()` para erros

### Dependências
- FullCalendar v6.1.15
- Bootstrap v5.3.8 (tooltips)
- FontAwesome Duotone (ícones)
- jQuery 3.x

---

## Fluxo de Dados

```
InitializeCalendar('/api/Agenda/CarregaViagens')
    ↓
FullCalendar renderiza
    ↓
Para cada evento → eventDidMount()
    ↓
gerarTooltipHTML(event)
    ↓
lightenColor(bgColor, 20)
    ↓
isColorDark(lightColor)
    ↓
Bootstrap.Tooltip criada
    ↓
shown.bs.tooltip → aplica cores dinamicamente
```

---

## Troubleshooting

### Problema: Tooltip não aparece

**Sintoma**: Tooltip não é exibida ao passar mouse sobre evento

**Causas Possíveis**:
1. Bootstrap não carregado
2. Classe CSS `.tooltip-ftx-agenda-dinamica` não definida
3. Erro em `gerarTooltipHTML()`

**Diagnóstico**:
```javascript
// Console do navegador
console.log(bootstrap.Tooltip); // Deve retornar a classe
document.querySelector('.tooltip-ftx-agenda-dinamica'); // Deve retornar elemento
```

**Solução**:
1. Verificar carregamento do Bootstrap
2. Verificar CSS em `Pages/Agenda/Index.cshtml`
3. Verificar console por erros JS

---

### Problema: Cor da tooltip incorreta

**Sintoma**: Tooltip com cor de fundo errada ou texto ilegível

**Causas Possíveis**:
1. `lightenColor()` retornando cor inválida
2. `isColorDark()` com threshold incorreto
3. Evento sem `backgroundColor`

**Diagnóstico**:
```javascript
// No eventDidMount, adicionar logs
console.log('Cor original:', bgColor);
console.log('Cor clareada:', lightColor);
console.log('Cor do texto:', textColor);
```

**Solução**:
1. Verificar formato hex da cor original
2. Ajustar threshold em `isColorDark()` se necessário (linha 58: `< 0.5`)
3. Garantir que API retorna `backgroundColor` válido

---

### Problema: Ícones não aparecem

**Sintoma**: Tooltip exibe texto sem ícones FontAwesome

**Causas Possíveis**:
1. FontAwesome não carregado
2. Classes duotone não disponíveis
3. CSS dos ícones não aplicado

**Diagnóstico**:
```javascript
// Console do navegador
document.querySelector('.fa-duotone'); // Deve retornar elementos
```

**Solução**:
1. Verificar carregamento do FontAwesome Kit
2. Verificar se kit inclui ícones duotone
3. Verificar CSS em `.tooltip-ftx-agenda-dinamica .tooltip-inner i`

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [17/01/2026 10:00] - Adicionado dois pontos entre ícone e texto no tooltip

**Descrição**: Melhoria visual no tooltip da Agenda. Adicionado o caractere `:` (dois pontos) entre o ícone e o texto de cada linha para melhor legibilidade.

**Formato Anterior**: `ícone texto`
**Formato Novo**: `ícone: texto`

**Mudanças**:

- Linha 95: Evento - `fa-tent`: → `fa-tent: `
- Linha 101-103: Motorista - `fa-user-tie`: → `fa-user-tie: `
- Linha 109-111: Veículo - `fa-car`: → `fa-car: `
- Linha 117: Descrição - `fa-memo-pad`: → `fa-memo-pad: `

**Arquivos Afetados**:

- `wwwroot/js/agendamento/components/calendario.js` (função `gerarTooltipHTML`, linhas 91-118)

**Impacto**: Tooltips exibem com formato mais claro e legível

**Status**: ✅ **Concluído**

**Versão**: 1.2

---

## [16/01/2026 14:30] - Correção Mapeamento extendedProps para Tooltips

**Descrição**: Corrigido bug crítico onde tooltips exibiam apenas "Veículo não Informado" porque os campos individuais não estavam sendo mapeados para `extendedProps` do FullCalendar.

**Problema Identificado**:
- API retornava corretamente: `{placa, motorista, evento, finalidade, descricao, ...}`
- Mas no mapeamento dos eventos (linhas 276-291), apenas `description` era passado diretamente
- FullCalendar automaticamente colocava `description` em `extendedProps`, mas ignorava os outros campos
- Resultado: `extendedProps` continha apenas `{description: "..."}`, sem `placa`, `motorista`, `evento`, `finalidade`

**Mudanças**:
1. **Mapeamento de Eventos** (linhas 276-291):
   - Adicionado objeto `extendedProps` explícito no evento
   - Agora mapeia corretamente todos os campos:
     ```javascript
     extendedProps: {
         descricao: item.descricao,
         placa: item.placa,
         motorista: item.motorista,
         evento: item.evento,
         finalidade: item.finalidade
     }
     ```

2. **Remoção de Debug**:
   - Removidos `console.log` temporários da função `gerarTooltipHTML` (linhas 89-94)

**Arquivos Afetados**:
- `wwwroot/js/agendamento/components/calendario.js` (linhas 276-291)

**Impacto**: Tooltips agora exibem corretamente:
- ✅ Placa do veículo com ícone de carro
- ✅ Nome do motorista com ícone de pessoa
- ✅ Nome do evento (quando finalidade = 'Evento') com ícone de tenda
- ✅ Descrição com ícone de memo
- ✅ Cores dinâmicas (20% mais clara que o evento)

**Status**: ✅ **Concluído**

**Versão**: 1.1

---

## [16/01/2026 12:40] - Implementação Inicial de Tooltips Dinâmicas

**Descrição**: Criação do sistema de tooltips customizadas com ícones FontAwesome e cores adaptativas para eventos do calendário.

**Mudanças**:
1. **Função `lightenColor(color, percent)`** (linhas 11-38):
   - Clareia cor hexadecimal em porcentagem específica
   - Usado para gerar cor de fundo da tooltip 20% mais clara que o evento

2. **Função `isColorDark(color)`** (linhas 45-64):
   - Detecta se cor é escura usando fórmula W3C de luminância
   - Retorna `true` se luminância < 0.5
   - Usado para definir cor do texto (branco/preto)

3. **Função `gerarTooltipHTML(event)`** (linhas 71-120):
   - Constrói HTML com ícones e quebras de linha
   - Trata campos vazios adequadamente
   - Remove " - " do final da descrição
   - Estrutura:
     - Veículo (fa-car)
     - Motorista (fa-user-tie)
     - Evento (fa-tent) - apenas se finalidade = "Evento"
     - Descrição (fa-memo-pad)

4. **Modificação `eventDidMount`** (linhas 350-396):
   - Substitui tooltip simples por tooltip customizada
   - Integra funções de cor e HTML
   - Aplica cores dinamicamente após criação
   - Listener `shown.bs.tooltip` para aplicar estilos

**Arquivos Afetados**:
- `wwwroot/js/agendamento/components/calendario.js`

**Integração**:
- CSS: `.tooltip-ftx-agenda-dinamica` em `Pages/Agenda/Index.cshtml`
- Backend: Campos `placa`, `motorista`, `evento`, `finalidade` de `AgendaController`

**Impacto**: Tooltips agora exibem informações detalhadas e formatadas com cores dinâmicas adaptadas ao evento.

**Status**: ✅ **Concluído**

**Versão**: 1.0

---

**Última atualização**: 16/01/2026 14:30
**Autor**: Sistema FrotiX
**Versão**: 1.1
