# agendamento-core.js � Controle Unificado de Recorr�ncia

## Vis�o Geral
O arquivo **agendamento-core.js** centraliza a inicializa��o dos controles Kendo da recorr�ncia e a orquestra��o de visibilidade dos campos no modal de agendamento da Agenda. Ele garante que os componentes corretos sejam exibidos conforme o per�odo selecionado e harmoniza a camada de UI com regras de CSS existentes (inclusive regras com `!important`).

Este m�dulo conversa diretamente com:
- **Kendo UI** (DropDownList, MultiSelect, DatePicker)
- **Syncfusion** (Calendar para datas variadas)
- **Layout da Agenda** (`Pages/Agenda/Index.cshtml`)

Ele tambTm passou a **inicializar o Calendar Syncfusion sob demanda**, sincronizando a listbox `lstDatasVariadas` e os badges de contagem (`badgeContadorDatas` e `badgeContadorDatasVariadas`) sempre que o per?odo **Dias Variados** T selecionado.

## Por que este m�dulo � cr�tico
A recorr�ncia � um fluxo sens�vel do sistema: erros de visibilidade impedem o usu�rio de selecionar **Data Final** ou **Datas Variadas**, quebrando o processo de cria��o/edi��o. O m�dulo garante que:
- Campos obrigat�rios apare�am no momento certo
- IDs corretos sejam usados (ex.: `divDiaMes`, `calendarContainer`, `listboxDatasVariadasContainer`)
- CSS com `display: none !important` seja superado quando necess�rio

---

## Snippet 3 - Calend�rio Syncfusion + badges de Dias Variados

```javascript
_garantirCalendario: async function () {
    try {
        const calElement = document.getElementById("calDatasSelecionadas");
        if (!calElement || calElement.ej2_instances?.[0]) {
            this._sincronizarDatasVariadas(calElement?.ej2_instances?.[0]?.values || []);
            return;
        }

        if (typeof window.loadSyncfusionLocalization === "function" && !window.syncfusionLocalizationFailed) {
            if (!window.syncfusionLocalizationPromise) {
                window.syncfusionLocalizationPromise = window.loadSyncfusionLocalization()
                    .then(() => { window.syncfusionLocalizationReady = true; })
                    .catch((error) => {
                        window.syncfusionLocalizationFailed = true;
                        Utils.erro("Recorrencia._garantirCalendario.localizacao", error);
                    });
            }

            await window.syncfusionLocalizationPromise;
        }

        const localeCalendario = window.syncfusionLocalizationReady ? "pt-BR" : "en-US";
        const calendario = new ej.calendars.Calendar({
            isMultiSelection: true,
            showTodayButton: false,
            locale: localeCalendario,
            change: (args) => this._sincronizarDatasVariadas(args?.values || [])
        });

        calendario.appendTo("#calDatasSelecionadas");
        this._sincronizarDatasVariadas(calendario.values || []);
    } catch (error) {
        Utils.erro("Recorrencia._garantirCalendario", error);
    }
},

_sincronizarDatasVariadas: function (datas) {
    try {
        const datasNormalizadas = Array.isArray(datas) ? datas.map(d => new Date(d)) : [];
        window.selectedDates = datasNormalizadas;

        const listbox = document.getElementById("lstDatasVariadas");
        if (listbox) {
            listbox.innerHTML = "";
            listbox.size = Math.max(Math.min(datasNormalizadas.length, 5), 1);
        }

        this._atualizarBadge("#badgeContadorDatas", datasNormalizadas.length, "flex");
        this._atualizarBadge("#badgeContadorDatasVariadas", datasNormalizadas.length, "inline-flex");
    } catch (error) {
        Utils.erro("Recorrencia._sincronizarDatasVariadas", error);
    }
}
```

### Detalhamento t�cnico
- **`_garantirCalendario`** garante que a localiza��o CLDR esteja carregada antes de criar o Syncfusion Calendar, evitando o erro de formato inv��lido.
- **`_sincronizarDatasVariadas`** mant�m `window.selectedDates`, popula `lstDatasVariadas` e atualiza badges do calend�rio e da listbox.

---

## Snippet 1 � `Utils.mostrar` (visibilidade com `!important`)

```javascript
mostrar: function (seletor, mostrar = true, displayPadrao = "block") {
    try {
        const $el = $(seletor);
        if (!$el.length) {
            return;
        }

        const displayFinal = mostrar ? displayPadrao : "none";

        $el.each(function () {
            this.style.setProperty("display", displayFinal, "important");
        });
    } catch (error) {
        Utils.erro("Utils.mostrar", error);
    }
}
```

### Detalhamento t�cnico
- **`displayPadrao`** permite usar `block` (colunas) ou `flex` (linhas `.row`).
- **`setProperty(..., "important")`** � obrigat�rio para vencer CSS fixo como `#divFinalRecorrencia { display: none !important; }`.
- **`try/catch`** garante rastreabilidade via `Alerta.TratamentoErroComLinha`.

---

## Snippet 2 � `Recorrencia.atualizarVisibilidade`

```javascript
atualizarVisibilidade: function (periodo) {
    try {
        const periodoNormalizado = (periodo || "").toString().trim().toUpperCase();
        Utils.log("Atualizando visibilidade para per�odo:", periodoNormalizado);

        const recorrenteValor = (Controles.getRecorrente() || "").toString().trim().toUpperCase();
        const mostrarPeriodo = recorrenteValor === "S" || recorrenteValor === "SIM" || recorrenteValor === "TRUE" || recorrenteValor === "1";
        const mostrarDias = periodoNormalizado === "S" || periodoNormalizado === "Q";
        const mostrarDiasMes = periodoNormalizado === "M";
        const mostrarFinal = periodoNormalizado === "D" || periodoNormalizado === "S" || periodoNormalizado === "Q" || periodoNormalizado === "M";
        const mostrarCalendario = periodoNormalizado === "V";

        Utils.mostrar(CONFIG.elementos.divPeriodo, mostrarPeriodo, "block");
        Utils.mostrar(CONFIG.elementos.divDias, mostrarDias, "block");
        Utils.mostrar(CONFIG.elementos.divDiasMes, mostrarDiasMes, "block");
        Utils.mostrar(CONFIG.elementos.divFinalRecorrencia, mostrarFinal, "block");
        Utils.mostrar(CONFIG.elementos.divCalendario, mostrarCalendario, "flex");
        Utils.mostrar(CONFIG.elementos.divListaDatas, mostrarCalendario, "flex");
        Utils.mostrar(CONFIG.elementos.calDatasSelecionadas, mostrarCalendario, "block");
        Utils.mostrar(CONFIG.elementos.lstDatasVariadas, mostrarCalendario, "block");
        Utils.mostrar(CONFIG.elementos.lblSelecioneAsDatas, mostrarCalendario, "block");

        if (mostrarCalendario) {
            this._garantirCalendario();
            this._sincronizarDatasVariadas(this._obterDatasCalendario());
            Utils.mostrar(CONFIG.elementos.badgeCalendario, true, "flex");
        } else {
            this._atualizarBadge(CONFIG.elementos.badgeCalendario, 0, "flex");
            this._atualizarBadge(CONFIG.elementos.badgeDatasVariadas, 0, "inline-flex");
        }
    } catch (error) {
        Utils.erro("Recorrencia.atualizarVisibilidade", error);
    }
}
```

### Detalhamento t�cnico (blocos principais)
1. **Normaliza��o do per�odo**
   - Converte o valor em `D/S/Q/M/V` (mai�sculo e sem espa�os).

2. **Reconhecimento de Recorrente**
   - Aceita `S`, `SIM`, `TRUE` ou `1` para evitar inconsist�ncias.

3. **Controle de visibilidade**
   - `divFinalRecorrencia` aparece em **Di�rio, Semanal, Quinzenal e Mensal**.
   - `calendarContainer` aparece apenas em **Dias Variados**.

4. **Renderiza��o confi�vel**
   - Usa `Utils.mostrar` com `display` correto (`block` ou `flex`).
5. **Calend�rio de Dias Variados**
   - Ao selecionar `V`, o m�dulo inicializa o **Syncfusion Calendar**, exibe `calDatasSelecionadas`/`lstDatasVariadas` e atualiza os badges de contagem.

---

## Pontos de Integra��o com o CSHTML
Os IDs abaixo precisam estar sincronizados com `Pages/Agenda/Index.cshtml`:
- `#divDiaMes`
- `#calendarContainer`
- `#calDatasSelecionadas`
- `#lblSelecioneAsDatas`
- `#badgeContadorDatas`
- `#listboxDatasVariadasContainer`
- `#lstDatasVariadas`
- `#badgeContadorDatasVariadas`

---

# Log de Modificações

> **Formato**: Mais recente no topo

## [21/01/2026 14:17] - Badge sempre visível no calendário de Dias Variados
- **Parâmetro `sempreMostrar`**: Adicionado quarto parâmetro opcional na função `_atualizarBadge(seletor, total, displayPadrao, sempreMostrar)`.
- **Comportamento padrão**: Badge permanece visível mesmo com contagem zero (`sempreMostrar = true` por padrão).
- **Ocultação explícita**: Chamadas em `atualizarVisibilidade` passam `false` para ocultar badges apenas quando calendário não está visível.
- **Correção de encoding**: Corrigido comentário "CalendÃ¡rio" para "Calendário".

## [21/01/2026] - Localizacao do calendario de Dias Variados
- **CLDR carregado sob demanda**: `_garantirCalendario` aguarda `loadSyncfusionLocalization` antes de instanciar o Syncfusion Calendar.
- **Fallback seguro**: aplica cultura `en-US` quando a localizacao falha, evitando travamentos no modal.

## [20/01/2026] - Calendario de Dias Variados e badges
- **Inicializacao sob demanda**: `_garantirCalendario` cria o Syncfusion Calendar quando o periodo `V` e selecionado.
- **Sincronizacao de UI**: `_sincronizarDatasVariadas` atualiza `lstDatasVariadas` e os badges `badgeContadorDatas`/`badgeContadorDatasVariadas`.
- **Compatibilidade**: `RecorrenciaController.prepararNovo` delega para `AgendamentoCore` e garante controles habilitados em novos agendamentos.

## [20/01/2026] � Ajuste de visibilidade da recorr�ncia
- **Corre��o de IDs**: `divDiaMes`, `calendarContainer`, `listboxDatasVariadasContainer`.
- **Visibilidade robusta**: `Utils.mostrar` agora aplica `display: ... !important`.
- **Normaliza��o de per�odo**: garante leitura consistente de `D/S/Q/M/V`.
- **Tratamento de erro**: adicionado `try/catch` em `Recorrencia.atualizarVisibilidade`.





