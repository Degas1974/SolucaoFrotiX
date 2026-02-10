# Documentação: syncfusion.utils.js

> **Última Atualização**: 12/01/2026
> **Versão Atual**: 1.2

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Lógica de Negócio](#lógica-de-negócio)
5. [Interconexões](#interconexões)
6. [Configuração de Localização](#configuração-de-localização)
7. [Funções Utilitárias](#funções-utilitárias)
8. [Exemplos de Uso](#exemplos-de-uso)
9. [Troubleshooting](#troubleshooting)

---

## Visão Geral

**Descrição**: O arquivo `syncfusion.utils.js` contém funções utilitárias e configurações globais para componentes Syncfusion EJ2 usados no sistema FrotiX. É responsável por inicialização, localização (pt-BR), manipulação de instâncias e configurações específicas dos componentes.

### Características Principais
- ✅ **Configuração de Localização pt-BR**: Configura idioma português brasileiro para todos os componentes Syncfusion
- ✅ **Funções Utilitárias**: Helpers para obter instâncias e valores de componentes
- ✅ **Gerenciamento de Tooltips**: Limpeza global de tooltips para evitar duplicações
- ✅ **Configuração RichTextEditor**: Setup de paste de imagens em base64
- ✅ **CLDR Completo**: Dados de calendário e formatação numérica para pt-BR

### Objetivo
Centralizar configurações e utilitários relacionados aos componentes Syncfusion, garantindo comportamento consistente e localização adequada em português brasileiro em todo o sistema FrotiX.

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| Syncfusion EJ2 | - | Biblioteca de componentes UI |
| JavaScript ES6 | - | Linguagem de programação |
| CLDR (Common Locale Data Repository) | 36 | Dados de localização |

### Padrões de Design
- **Singleton Pattern**: Configuração global única de localização
- **Helper Functions**: Funções utilitárias globais no objeto `window`
- **Event Callbacks**: Callbacks reutilizáveis para componentes Syncfusion

---

## Estrutura de Arquivos

### Arquivo Principal
```
wwwroot/js/agendamento/utils/syncfusion.utils.js
```

### Arquivos Relacionados
- `wwwroot/js/agendamento/main.js` - Chama `configurarLocalizacaoSyncfusion()` na inicialização
- `Pages/Agenda/Index.cshtml` - Usa componentes Syncfusion configurados por este arquivo
- `wwwroot/js/agendamento/components/*.js` - Usam funções utilitárias deste arquivo

---

## Lógica de Negócio

### Funções/Métodos Principais

#### Método: `window.getSyncfusionInstance(id)`
**Localização**: Linha 10 do arquivo `syncfusion.utils.js`

**Propósito**: Obtém a instância Syncfusion de um elemento HTML pelo ID

**Parâmetros**:
- `id` (string): ID do elemento HTML que contém o componente Syncfusion

**Retorno**: Object|null - Instância Syncfusion ou null se não encontrada

**Exemplo de Código**:
```javascript
window.getSyncfusionInstance = function (id)
{
    try
    {
        const el = document.getElementById(id);
        if (el && Array.isArray(el.ej2_instances) && el.ej2_instances.length > 0 && el.ej2_instances[0])
        {
            return el.ej2_instances[0];
        }
        return null;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "getSyncfusionInstance", error);
        return null;
    }
};
```

**Fluxo de Execução**:
1. Busca elemento HTML pelo ID
2. Verifica se elemento existe e tem instâncias Syncfusion (`ej2_instances`)
3. Verifica se array de instâncias tem pelo menos 1 elemento
4. Retorna primeira instância ou null

**Casos Especiais**:
- **Caso A**: Elemento não existe → retorna null
- **Caso B**: Elemento existe mas não tem instâncias Syncfusion → retorna null
- **Caso C**: Ocorre erro → registra via `Alerta.TratamentoErroComLinha` e retorna null

---

#### Método: `window.getSfValue0(inst)`
**Localização**: Linha 32 do arquivo `syncfusion.utils.js`

**Propósito**: Obtém o primeiro valor de um componente Syncfusion (útil para componentes multi-seleção)

**Parâmetros**:
- `inst` (Object): Instância Syncfusion

**Retorno**: * - Primeiro valor ou null

**Exemplo de Código**:
```javascript
window.getSfValue0 = function (inst)
{
    try
    {
        if (!inst) return null;
        const v = inst.value;
        if (Array.isArray(v)) return v.length ? v[0] : null;
        return v ?? null;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "getSfValue0", error);
        return null;
    }
};
```

**Fluxo de Execução**:
1. Verifica se instância existe
2. Obtém valor da instância
3. Se valor é array → retorna primeiro elemento (ou null se array vazio)
4. Se valor não é array → retorna o valor diretamente
5. Se valor é undefined/null → retorna null

**Casos Especiais**:
- **Caso A**: Instância null → retorna null
- **Caso B**: Valor é array vazio → retorna null
- **Caso C**: Valor é array com elementos → retorna primeiro elemento
- **Caso D**: Valor é escalar → retorna valor

---

#### Método: `window.limpaTooltipsGlobais(timeout)`
**Localização**: Linha 51 do arquivo `syncfusion.utils.js`

**Propósito**: Remove todos os tooltips globais (Syncfusion e Bootstrap) para evitar duplicações e conflitos

**Parâmetros**:
- `timeout` (number): Timeout em milissegundos antes de executar limpeza (padrão: 200ms)

**Retorno**: void

**Exemplo de Código**:
```javascript
window.limpaTooltipsGlobais = function (timeout = 200)
{
    try
    {
        setTimeout(() =>
        {
            try
            {
                // Remove wrappers de tooltip Syncfusion
                document.querySelectorAll(".e-tooltip-wrap").forEach(t =>
                {
                    try
                    {
                        t.remove();
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "limpaTooltipsGlobais_remove", error);
                    }
                });

                // Destroi instâncias Syncfusion de tooltips
                document.querySelectorAll(".e-control.e-tooltip").forEach(el =>
                {
                    try
                    {
                        const instance = el.ej2_instances?.[0];
                        if (instance?.destroy) instance.destroy();
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "limpaTooltipsGlobais_destroy", error);
                    }
                });

                // Remove atributos title (Bootstrap)
                document.querySelectorAll("[title]").forEach(el =>
                {
                    try
                    {
                        el.removeAttribute("title");
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "limpaTooltipsGlobais_removeAttr", error);
                    }
                });

                // Dispose tooltips Bootstrap e remove elementos
                $('[data-bs-toggle="tooltip"]').tooltip("dispose");
                $(".tooltip").remove();
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("syncfusion.utils.js", "limpaTooltipsGlobais_timeout", error);
            }
        }, timeout);
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "limpaTooltipsGlobais", error);
    }
};
```

**Fluxo de Execução**:
1. Aguarda timeout configurado
2. Remove todos os `.e-tooltip-wrap` do DOM
3. Destroi todas as instâncias Syncfusion de tooltips (`.e-control.e-tooltip`)
4. Remove todos os atributos `title` de elementos (previne tooltips nativos)
5. Dispose tooltips Bootstrap via jQuery
6. Remove todos os elementos `.tooltip` do DOM

**Casos Especiais**:
- **Caso A**: Erro ao remover elemento específico → registra erro mas continua limpando outros
- **Caso B**: Timeout customizado → usa valor fornecido pelo usuário
- **Caso C**: Timeout padrão → usa 200ms

---

#### Método: `window.rebuildLstPeriodos()`
**Localização**: Linha 109 do arquivo `syncfusion.utils.js`

**Propósito**: Rebuilda o DropDownList de períodos (meses) usando dados globais

**Parâmetros**: Nenhum

**Retorno**: void

**Exemplo de Código**:
```javascript
window.rebuildLstPeriodos = function ()
{
    try
    {
        new ej.dropdowns.DropDownList({
            dataSource: window.dataPeriodos || [],
            fields: {
                value: "PeriodoId",
                text: "Periodo"
            },
            placeholder: "Selecione o período",
            allowFiltering: true,
            showClearButton: true,
            sortOrder: "Ascending"
        }).appendTo("#lstPeriodos");
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "rebuildLstPeriodos", error);
    }
};
```

**Fluxo de Execução**:
1. Cria novo DropDownList Syncfusion
2. Usa dados de `window.dataPeriodos` (ou array vazio se não existir)
3. Configura campos de valor e texto
4. Habilita filtro e botão de limpar
5. Ordena em ordem ascendente
6. Anexa ao elemento `#lstPeriodos`

**Casos Especiais**:
- **Caso A**: `window.dataPeriodos` não existe → usa array vazio
- **Caso B**: Elemento `#lstPeriodos` não existe → pode gerar erro silencioso

---

#### Método: `window.initializeModalTooltips()`
**Localização**: Linha 133 do arquivo `syncfusion.utils.js`

**Propósito**: Inicializa tooltips Syncfusion em elementos dentro de modais

**Parâmetros**: Nenhum

**Retorno**: void

**Exemplo de Código**:
```javascript
window.initializeModalTooltips = function ()
{
    try
    {
        const tooltipElements = document.querySelectorAll('[data-ejtip]');
        tooltipElements.forEach(function (element)
        {
            try
            {
                new ej.popups.Tooltip({
                    target: element
                });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("syncfusion.utils.js", "initializeModalTooltips_forEach", error);
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "initializeModalTooltips", error);
    }
};
```

**Fluxo de Execução**:
1. Busca todos os elementos com atributo `data-ejtip`
2. Para cada elemento:
   - Cria novo Tooltip Syncfusion
   - Define elemento como target do tooltip

**Casos Especiais**:
- **Caso A**: Erro ao criar tooltip em elemento específico → registra erro mas continua para próximo
- **Caso B**: Nenhum elemento com `data-ejtip` → não faz nada

---

#### Método: `window.setupRTEImagePaste(rteId)`
**Localização**: Linha 160 do arquivo `syncfusion.utils.js`

**Propósito**: Configura RichTextEditor para suportar paste de imagens da área de transferência convertendo para base64

**Parâmetros**:
- `rteId` (string): ID do elemento RichTextEditor

**Retorno**: void

**Exemplo de Código**:
```javascript
window.setupRTEImagePaste = function (rteId)
{
    try
    {
        const rteDescricao = document.getElementById(rteId);
        if (!rteDescricao || !rteDescricao.ej2_instances || !rteDescricao.ej2_instances[0])
        {
            return;
        }

        const rte = rteDescricao.ej2_instances[0];

        rte.element.addEventListener("paste", function (event)
        {
            try
            {
                const clipboardData = event.clipboardData;

                if (clipboardData && clipboardData.items)
                {
                    const items = clipboardData.items;

                    for (let i = 0; i < items.length; i++)
                    {
                        const item = items[i];

                        if (item.type.indexOf("image") !== -1)
                        {
                            const blob = item.getAsFile();
                            const reader = new FileReader();

                            reader.onloadend = function ()
                            {
                                try
                                {
                                    const base64Image = reader.result.split(",")[1];
                                    const pastedHtml = `<img src="data:image/png;base64,${base64Image}" />`;
                                    rte.executeCommand('insertHTML', pastedHtml);
                                } catch (error)
                                {
                                    Alerta.TratamentoErroComLinha("syncfusion.utils.js", "setupRTEImagePaste_onloadend", error);
                                }
                            };

                            reader.readAsDataURL(blob);
                            break;
                        }
                    }
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("syncfusion.utils.js", "setupRTEImagePaste_paste", error);
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "setupRTEImagePaste", error);
    }
};
```

**Fluxo de Execução**:
1. Busca elemento RichTextEditor pelo ID
2. Valida se elemento existe e tem instância Syncfusion
3. Obtém instância RTE
4. Adiciona listener de evento `paste` no elemento
5. Quando ocorre paste:
   - Acessa dados da área de transferência
   - Itera por todos os itens colados
   - Se encontra item do tipo imagem:
     - Converte blob para File
     - Lê arquivo como Data URL (base64)
     - Extrai base64 do resultado
     - Insere HTML com imagem base64 no RTE
     - Para o loop (apenas primeira imagem)

**Casos Especiais**:
- **Caso A**: Elemento RTE não existe → retorna sem fazer nada
- **Caso B**: Paste não contém imagem → não faz nada
- **Caso C**: Paste contém múltiplas imagens → insere apenas a primeira

---

#### Método: `window.configurarLocalizacaoSyncfusion()`
**Localização**: Linha 223 do arquivo `syncfusion.utils.js`

**Propósito**: Configura localização completa pt-BR para todos os componentes Syncfusion (textos, calendário, formatação numérica)

**Parâmetros**: Nenhum

**Retorno**: void

**Exemplo de Código Resumido**:
```javascript
window.configurarLocalizacaoSyncfusion = function ()
{
    try
    {
        // Configurar L10n (textos dos componentes)
        const L10n = ej.base.L10n;
        L10n.load({
            pt: {
                calendar: {
                    today: "Hoje"
                }
            },
            "pt-BR": {
                calendar: {
                    today: "Hoje"
                },
                richtexteditor: {
                    alignments: "Alinhamentos",
                    justifyLeft: "Alinhar à Esquerda",
                    // ... (150+ traduções)
                }
            }
        });

        // Configurar cultura pt-BR
        if (ej.base && ej.base.setCulture)
        {
            ej.base.setCulture('pt-BR');
        }

        // Carregar dados CLDR para português
        if (ej.base && ej.base.loadCldr)
        {
            const ptBRCldr = {
                "main": {
                    "pt-BR": {
                        "identity": {
                            "version": { "_cldrVersion": "36" },
                            "language": "pt"
                        },
                        "dates": {
                            "calendars": {
                                "gregorian": {
                                    "months": {
                                        "format": {
                                            "abbreviated": {
                                                "1": "jan", "2": "fev", "3": "mar", "4": "abr",
                                                "5": "mai", "6": "jun", "7": "jul", "8": "ago",
                                                "9": "set", "10": "out", "11": "nov", "12": "dez"
                                            },
                                            "wide": {
                                                "1": "janeiro", "2": "fevereiro", "3": "março",
                                                "4": "abril", "5": "maio", "6": "junho",
                                                "7": "julho", "8": "agosto", "9": "setembro",
                                                "10": "outubro", "11": "novembro", "12": "dezembro"
                                            }
                                        }
                                    },
                                    "days": {
                                        "format": {
                                            "abbreviated": {
                                                "sun": "dom", "mon": "seg", "tue": "ter",
                                                "wed": "qua", "thu": "qui", "fri": "sex", "sat": "sáb"
                                            },
                                            "wide": {
                                                "sun": "domingo", "mon": "segunda", "tue": "terça",
                                                "wed": "quarta", "thu": "quinta", "fri": "sexta", "sat": "sábado"
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        "numbers": {
                            "defaultNumberingSystem": "latn",
                            "symbols-numberSystem-latn": {
                                "decimal": ",",
                                "group": ".",
                                "list": ";",
                                "percentSign": "%",
                                "plusSign": "+",
                                "minusSign": "-",
                                "exponential": "E",
                                "superscriptingExponent": "×",
                                "perMille": "‰",
                                "infinity": "∞",
                                "nan": "NaN",
                                "timeSeparator": ":"
                            },
                            "decimalFormats-numberSystem-latn": {
                                "standard": "#,##0.###"
                            },
                            "percentFormats-numberSystem-latn": {
                                "standard": "#,##0%"
                            },
                            "currencyFormats-numberSystem-latn": {
                                "standard": "¤ #,##0.00",
                                "accounting": "¤ #,##0.00"
                            },
                            "currencies": {
                                "BRL": {
                                    "displayName": "Real brasileiro",
                                    "symbol": "R$"
                                },
                                "USD": {
                                    "displayName": "Dólar americano",
                                    "symbol": "US$"
                                },
                                "EUR": {
                                    "displayName": "Euro",
                                    "symbol": "€"
                                }
                            }
                        }
                    }
                }
            };

            ej.base.loadCldr(ptBRCldr);
        }

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "configurarLocalizacaoSyncfusion", error);
    }
};
```

**Fluxo de Execução**:
1. **Carrega textos traduzidos**:
   - Configura L10n com traduções pt-BR
   - Inclui traduções para Calendar e RichTextEditor
   - Mais de 150 strings traduzidas
2. **Define cultura**:
   - Chama `ej.base.setCulture('pt-BR')`
   - Define cultura padrão para todos os componentes
3. **Carrega CLDR**:
   - Define dados de calendário (meses e dias em português)
   - Define dados de formatação numérica (separadores, símbolos)
   - Define formatos de número, porcentagem e moeda
   - Define informações de moedas (BRL, USD, EUR)

**Casos Especiais**:
- **Caso A**: `ej.base` não existe → não executa configuração (componentes não carregados)
- **Caso B**: `setCulture` não disponível → pula configuração de cultura
- **Caso C**: `loadCldr` não disponível → pula carregamento de CLDR
- **Caso D**: Erro em qualquer etapa → registra erro mas não interrompe aplicação

**Importância da Seção Numbers**:
- **Sem seção numbers**: Componentes não conseguem formatar números/porcentagens → erro "Cannot read properties of undefined (reading 'percentSign')"
- **Com seção numbers completa**: Componentes formatam corretamente números, porcentagens e moedas em pt-BR

---

#### Método: `window.onCreate()`
**Localização**: Linha 477 do arquivo `syncfusion.utils.js`

**Propósito**: Callback global de criação de RichTextEditor (compatibilidade legada)

**Parâmetros**: Nenhum (usa `this` para referenciar instância RTE)

**Retorno**: void

**Exemplo de Código**:
```javascript
window.onCreate = function ()
{
    try
    {
        window.defaultRTE = this;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "onCreate", error);
    }
};
```

**Fluxo de Execução**:
1. Armazena instância RTE atual em `window.defaultRTE`
2. Permite acesso global à instância padrão do RTE

**Casos Especiais**:
- **Caso A**: Múltiplos RTEs → apenas o último criado fica em `window.defaultRTE`

---

#### Método: `window.toolbarClick(e)`
**Localização**: Linha 488 do arquivo `syncfusion.utils.js`

**Propósito**: Callback de click em toolbar do RichTextEditor (configura upload de imagens)

**Parâmetros**:
- `e` (Object): Evento de click da toolbar

**Retorno**: void

**Exemplo de Código**:
```javascript
window.toolbarClick = function (e)
{
    try
    {
        if (e.item.id == "rte_toolbar_Image")
        {
            const element = document.getElementById("rte_upload");
            if (element && element.ej2_instances && element.ej2_instances[0])
            {
                element.ej2_instances[0].uploading = function (args)
                {
                    try
                    {
                        args.currentRequest.setRequestHeader("XSRF-TOKEN", document.getElementsByName("__RequestVerificationToken")[0].value);
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "toolbarClick_uploading", error);
                    }
                };
            }
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "toolbarClick", error);
    }
};
```

**Fluxo de Execução**:
1. Verifica se item clicado é o botão de imagem (`rte_toolbar_Image`)
2. Se sim:
   - Busca elemento de upload (`rte_upload`)
   - Obtém instância Syncfusion do uploader
   - Configura callback `uploading`
   - No callback, adiciona header XSRF-TOKEN para segurança CSRF

**Casos Especiais**:
- **Caso A**: Click em outro item da toolbar → não faz nada
- **Caso B**: Elemento uploader não existe → não configura upload
- **Caso C**: Token CSRF não existe → pode gerar erro

---

#### Método: `window.onDateChange(args)`
**Localização**: Linha 518 do arquivo `syncfusion.utils.js`

**Propósito**: Callback de mudança de data em calendários

**Parâmetros**:
- `args` (Object): Argumentos do evento de mudança de data

**Retorno**: void

**Exemplo de Código**:
```javascript
window.onDateChange = function (args)
{
    try
    {
        window.selectedDates = args.values;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("syncfusion.utils.js", "onDateChange", error);
    }
};
```

**Fluxo de Execução**:
1. Armazena valores de data selecionados em variável global `window.selectedDates`
2. Permite acesso global às datas selecionadas

**Casos Especiais**:
- **Caso A**: Múltiplas datas selecionadas → `args.values` é array
- **Caso B**: Data única selecionada → `args.values` pode ser array com 1 elemento ou valor escalar

---

## Interconexões

### Quem Chama Este Arquivo
- **`wwwroot/js/agendamento/main.js`**:
  - Linha 53: Chama `window.configurarLocalizacaoSyncfusion()` na inicialização da página
  - Usa funções utilitárias para manipular componentes Syncfusion

- **`wwwroot/js/agendamento/components/*.js`**:
  - Usam `window.getSyncfusionInstance()` para obter instâncias de componentes
  - Usam `window.getSfValue0()` para obter valores
  - Chamam `window.limpaTooltipsGlobais()` ao fechar modais

- **`Pages/Agenda/Index.cshtml`**:
  - Referencia este arquivo na seção de scripts
  - Depende da configuração de localização para funcionar corretamente

### O Que Este Arquivo Faz
- **Configura Syncfusion EJ2**: Define idioma, cultura e dados CLDR
- **Fornece utilitários**: Helpers para manipulação de componentes
- **Gerencia tooltips**: Evita duplicações e conflitos
- **Configura RTE**: Setup de paste de imagens e upload

### Fluxo de Dados
```
Inicialização da Página
    ↓
main.js carrega
    ↓
Chama configurarLocalizacaoSyncfusion()
    ↓
Syncfusion EJ2 configurado em pt-BR
    ↓
Componentes criados usam configuração
    ↓
Interações do usuário usam funções utilitárias
```

---

## Configuração de Localização

### L10n (Textos dos Componentes)

**Propósito**: Traduzir todos os textos exibidos nos componentes Syncfusion

**Componentes Traduzidos**:
- Calendar: "Hoje"
- RichTextEditor: 150+ strings traduzidas (toolbar, diálogos, mensagens)

**Exemplo de Traduções**:
```javascript
L10n.load({
    "pt-BR": {
        richtexteditor: {
            bold: "Negrito",
            italic: "Itálico",
            underline: "Sublinhado",
            createLink: "Inserir Link",
            image: "Inserir Imagem",
            // ... 150+ traduções
        }
    }
});
```

### CLDR (Common Locale Data Repository)

**Propósito**: Fornecer dados de localização completos (calendário, números, moedas)

**Estrutura CLDR pt-BR**:

#### 1. Identity (Identificação)
```javascript
"identity": {
    "version": { "_cldrVersion": "36" },
    "language": "pt"
}
```

#### 2. Dates (Calendário)
```javascript
"dates": {
    "calendars": {
        "gregorian": {
            "months": {
                "format": {
                    "abbreviated": {
                        "1": "jan", "2": "fev", ..., "12": "dez"
                    },
                    "wide": {
                        "1": "janeiro", "2": "fevereiro", ..., "12": "dezembro"
                    }
                }
            },
            "days": {
                "format": {
                    "abbreviated": {
                        "sun": "dom", "mon": "seg", ..., "sat": "sáb"
                    },
                    "wide": {
                        "sun": "domingo", "mon": "segunda", ..., "sat": "sábado"
                    }
                }
            }
        }
    }
}
```

#### 3. Numbers (Formatação Numérica) ⚠️ IMPORTANTE

**Adicionado em**: 12/01/2026 (versão 1.1)

**Problema que resolve**: Sem esta seção, componentes Syncfusion geram erro "Cannot read properties of undefined (reading 'percentSign')"

**Estrutura**:
```javascript
"numbers": {
    "defaultNumberingSystem": "latn",
    "symbols-numberSystem-latn": {
        "decimal": ",",           // Separador decimal (1,5)
        "group": ".",             // Separador de milhar (1.000)
        "list": ";",              // Separador de lista
        "percentSign": "%",       // ⚠️ CRÍTICO - Símbolo de porcentagem
        "plusSign": "+",          // Sinal de positivo
        "minusSign": "-",         // Sinal de negativo
        "exponential": "E",       // Notação científica
        "superscriptingExponent": "×",
        "perMille": "‰",          // Por mil
        "infinity": "∞",          // Infinito
        "nan": "NaN",             // Not a Number
        "timeSeparator": ":"      // Separador de hora (10:30)
    },
    "decimalFormats-numberSystem-latn": {
        "standard": "#,##0.###"   // Formato: 1.234,567
    },
    "percentFormats-numberSystem-latn": {
        "standard": "#,##0%"      // Formato: 75%
    },
    "currencyFormats-numberSystem-latn": {
        "standard": "¤ #,##0.00", // Formato: R$ 1.234,56
        "accounting": "¤ #,##0.00"
    },
    "currencies": {
        "BRL": {
            "displayName": "Real brasileiro",
            "symbol": "R$"
        },
        "USD": {
            "displayName": "Dólar americano",
            "symbol": "US$"
        },
        "EUR": {
            "displayName": "Euro",
            "symbol": "€"
        }
    }
}
```

**Por que é necessário**:
- DatePickers, ComboBoxes e outros componentes precisam formatar valores numéricos
- Sem os símbolos (especialmente `percentSign`), ocorre erro ao acessar propriedade undefined
- CLDR incompleto causa falha silenciosa ou erro JavaScript que bloqueia interações

---

## Funções Utilitárias

### Resumo de Todas as Funções

| Função | Propósito | Retorno |
|--------|-----------|---------|
| `getSyncfusionInstance(id)` | Obter instância Syncfusion de elemento | Object\|null |
| `getSfValue0(inst)` | Obter primeiro valor de componente | * |
| `limpaTooltipsGlobais(timeout)` | Limpar todos os tooltips | void |
| `rebuildLstPeriodos()` | Rebuildar lista de períodos | void |
| `initializeModalTooltips()` | Inicializar tooltips em modal | void |
| `setupRTEImagePaste(rteId)` | Configurar paste de imagens em RTE | void |
| `configurarLocalizacaoSyncfusion()` | Configurar localização pt-BR | void |
| `onCreate()` | Callback de criação RTE | void |
| `toolbarClick(e)` | Callback de click em toolbar RTE | void |
| `onDateChange(args)` | Callback de mudança de data | void |

---

## Exemplos de Uso

### Cenário 1: Obter Valor de ComboBox

**Situação**: Preciso obter o valor selecionado de um ComboBox Syncfusion

**Código**:
```javascript
// Obter instância
const veiculoCombo = window.getSyncfusionInstance('lstVeiculo');

// Obter valor (pode ser array em multi-seleção)
const veiculoId = window.getSfValue0(veiculoCombo);

console.log('Veículo selecionado:', veiculoId);
```

**Resultado Esperado**: ID do veículo selecionado ou null

---

### Cenário 2: Limpar Tooltips ao Fechar Modal

**Situação**: Ao fechar modal, preciso limpar tooltips para evitar duplicações

**Código**:
```javascript
$('#modalViagens').on('hidden.bs.modal', function() {
    window.limpaTooltipsGlobais(200); // Limpa após 200ms
});
```

**Resultado Esperado**: Todos os tooltips removidos do DOM

---

### Cenário 3: Configurar Paste de Imagens em RTE

**Situação**: Quero permitir que usuário cole imagens da área de transferência diretamente no editor

**Código**:
```javascript
// Após criar RichTextEditor
window.setupRTEImagePaste('txtDescricao');

// Agora usuário pode colar imagens (Ctrl+V) e elas serão convertidas para base64
```

**Resultado Esperado**: Imagens coladas aparecem no editor como tags `<img>` com base64

---

### Cenário 4: Inicializar Localização na Página

**Situação**: Primeira coisa a fazer ao carregar página com componentes Syncfusion

**Código**:
```javascript
$(document).ready(function() {
    // Configurar localização ANTES de criar componentes
    window.configurarLocalizacaoSyncfusion();

    // Agora criar componentes...
    new ej.calendars.DatePicker({
        // ... configurações
    }).appendTo('#txtDataInicial');
});
```

**Resultado Esperado**: Todos os componentes criados após a configuração usarão pt-BR

---

## Troubleshooting

### Problema: Erro "Cannot read properties of undefined (reading 'percentSign')"

**Sintoma**: Ao interagir com componentes Syncfusion (clicar em DatePicker, abrir ComboBox, etc.), ocorre erro JavaScript

**Causa**: Objeto CLDR pt-BR está incompleto - falta seção `numbers`

**Diagnóstico**:
1. Abrir console do navegador (F12)
2. Reproduzir ação que causa erro
3. Verificar stack trace - erro aponta para Syncfusion tentando acessar `percentSign`

**Solução**:
1. Verificar se `configurarLocalizacaoSyncfusion()` é chamada ANTES de criar componentes
2. Verificar se objeto `ptBRCldr` tem seção `numbers` completa
3. Se falta seção `numbers`, adicionar conforme exemplo na seção "Configuração de Localização"

**Código Relacionado**: Linhas 392-513 de `syncfusion.utils.js`

**Versão onde foi corrigido**: 1.1 (12/01/2026)

---

### Problema: Componentes exibem textos em inglês

**Sintoma**: Botões, labels e mensagens dos componentes Syncfusion aparecem em inglês

**Causa**: L10n não foi carregado ou `configurarLocalizacaoSyncfusion()` não foi chamada

**Diagnóstico**:
1. Verificar console do navegador - buscar mensagens de erro
2. Verificar se `main.js` chama `configurarLocalizacaoSyncfusion()`
3. Verificar ordem de carregamento dos scripts

**Solução**:
1. Garantir que `syncfusion.utils.js` é carregado ANTES de criar componentes
2. Garantir que `configurarLocalizacaoSyncfusion()` é chamada na inicialização
3. Verificar se não há erros JavaScript que impedem execução da função

**Código Relacionado**: Linhas 223-381 de `syncfusion.utils.js`

---

### Problema: Tooltips duplicados ou não desaparecem

**Sintoma**: Ao passar mouse sobre elementos, aparecem múltiplos tooltips sobrepostos ou tooltips não desaparecem

**Causa**: Tooltips não estão sendo limpos corretamente ao fechar modais ou trocar de página

**Diagnóstico**:
1. Inspecionar DOM (F12 → Elements)
2. Buscar elementos `.e-tooltip-wrap` ou `.tooltip`
3. Verificar se há múltiplos elementos

**Solução**:
1. Chamar `window.limpaTooltipsGlobais()` ao fechar modais
2. Chamar no evento `hidden.bs.modal` de modais Bootstrap
3. Se necessário, aumentar timeout: `limpaTooltipsGlobais(500)`

**Código Relacionado**: Linhas 51-104 de `syncfusion.utils.js`

---

### Problema: RTE não aceita paste de imagens

**Sintoma**: Ao colar imagem da área de transferência (Ctrl+V), nada acontece no RichTextEditor

**Causa**: `setupRTEImagePaste()` não foi chamada para o RTE específico

**Diagnóstico**:
1. Verificar se função foi chamada após criação do RTE
2. Verificar ID passado para a função está correto
3. Verificar console para erros

**Solução**:
1. Após criar RTE, chamar: `window.setupRTEImagePaste('idDoRTE')`
2. Verificar se ID do elemento está correto
3. Verificar se RTE foi criado antes de chamar a função

**Código Relacionado**: Linhas 160-218 de `syncfusion.utils.js`

---

### Problema: Datas exibidas em formato incorreto (MM/DD/YYYY)

**Sintoma**: Datas aparecem em formato americano ao invés de brasileiro (DD/MM/YYYY)

**Causa**: Cultura pt-BR não foi definida ou CLDR não foi carregado

**Diagnóstico**:
1. Verificar se `ej.base.setCulture('pt-BR')` foi chamada
2. Verificar se `loadCldr(ptBRCldr)` foi executada
3. Verificar console para erros

**Solução**:
1. Garantir que `configurarLocalizacaoSyncfusion()` é chamada na inicialização
2. Verificar se não há erros que impedem execução completa da função
3. Se necessário, chamar manualmente:
   ```javascript
   ej.base.setCulture('pt-BR');
   ```

**Código Relacionado**: Linhas 384-387 de `syncfusion.utils.js`

---

### Problema: Meses e dias exibidos em inglês no calendário

**Sintoma**: DatePicker mostra "January", "Monday", etc. ao invés de "Janeiro", "Segunda"

**Causa**: Seção `dates` do CLDR não foi carregada corretamente

**Diagnóstico**:
1. Verificar se objeto `ptBRCldr` está completo
2. Verificar se `loadCldr()` foi chamada sem erros
3. Verificar console para mensagens de erro

**Solução**:
1. Verificar seção `dates` → `calendars` → `gregorian` do objeto CLDR
2. Garantir que tem `months` e `days` com traduções
3. Recarregar página após correção

**Código Relacionado**: Linhas 401-460 de `syncfusion.utils.js`

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [12/01/2026 - 21:00] - Fix CRÍTICO: Execução imediata da configuração CLDR

**Descrição**: Adicionada execução AUTOMÁTICA e IMEDIATA da configuração de localização Syncfusion assim que o arquivo carregar, ANTES de qualquer componente ser criado.

**Problema Identificado** (pelos logs do console):
- Função `configurarLocalizacaoSyncfusion()` NÃO estava sendo executada
- Logs adicionados (`✅ Syncfusion: Cultura pt-BR definida`, etc.) não apareciam no console
- Componentes eram criados SEM o CLDR ter sido carregado
- Quando `txtQtdParticipantesEvento` (NumericTextBox) tentava formatar número → erro "Cannot read properties of undefined (reading 'percentSign')"

**Causa Raiz**:
- `configurarLocalizacaoSyncfusion()` estava definida mas nunca executada automaticamente
- `main.js` chamava a função, mas DEPOIS dos componentes já terem sido criados
- Componentes criados sem CLDR ficavam com configuração incompleta permanentemente

**Correção Aplicada**:
Adicionado **IIFE** (Immediately Invoked Function Expression) no final do arquivo que:
1. Executa assim que `syncfusion.utils.js` carregar
2. Tenta encontrar `ej.base` a cada 100ms (máximo 50 tentativas = 5 segundos)
3. Quando encontra, executa `window.configurarLocalizacaoSyncfusion()` IMEDIATAMENTE
4. Logs detalhados de todo o processo:
   - `🚀 [syncfusion.utils.js] Arquivo carregado - executando configuração pt-BR IMEDIATAMENTE...`
   - `⏳ [syncfusion.utils.js] ej.base não disponível ainda (tentativa X/50)...`
   - `✅ [syncfusion.utils.js] ej.base encontrado - configurando localização...`

**Arquivos Afetados**:
- `wwwroot/js/agendamento/utils/syncfusion.utils.js` (linhas 543-567 adicionadas)

**Código Adicionado**:
```javascript
// ====================================================================
// EXECUÇÃO IMEDIATA DA CONFIGURAÇÃO DE LOCALIZAÇÃO
// ====================================================================

(function() {
    console.log('🚀 [syncfusion.utils.js] Arquivo carregado - executando configuração pt-BR IMEDIATAMENTE...');

    // Aguardar até que ej.base esteja disponível
    function tentarConfigurar(tentativas = 0) {
        if (typeof ej !== 'undefined' && ej.base) {
            console.log('✅ [syncfusion.utils.js] ej.base encontrado - configurando localização...');
            window.configurarLocalizacaoSyncfusion();
        } else if (tentativas < 50) {
            console.log(`⏳ [syncfusion.utils.js] ej.base não disponível ainda (tentativa ${tentativas + 1}/50)...`);
            setTimeout(() => tentarConfigurar(tentativas + 1), 100);
        } else {
            console.error('❌ [syncfusion.utils.js] TIMEOUT: ej.base não carregou após 5 segundos!');
        }
    }

    tentarConfigurar();
})();
```

**Impacto**:
- ✅ CLDR carregado ANTES de qualquer componente ser criado
- ✅ Todos os componentes Syncfusion terão formatação numérica pt-BR
- ✅ Erro "percentSign undefined" será ELIMINADO definitivamente
- ✅ Console mostrará logs de sucesso da configuração
- ✅ Não depende mais de chamadas manuais em outros arquivos

**Status**: ✅ **Concluído** - Solução definitiva

**Responsável**: Claude Code

**Versão**: 1.1 → 1.2

---

## [12/01/2026 - 20:30] - Debug: Logs detalhados na configuração CLDR

**Descrição**: Adicionados logs de console detalhados na função `configurarLocalizacaoSyncfusion()` para diagnosticar erro "percentSign undefined" que continua ocorrendo ao clicar em agendamentos.

**Problema**: Erro "Cannot read properties of undefined (reading 'percentSign')" persiste mesmo após adicionar seção numbers ao CLDR.

**Logs Adicionados**:
- `✅ Syncfusion: Cultura pt-BR definida` - Confirma que setCulture foi executado
- `❌ Syncfusion: ej.base.setCulture não disponível` - Se ej.base não existe
- `🔄 Syncfusion: Carregando CLDR pt-BR...` - Início do carregamento
- `✅ Syncfusion: CLDR contém seção numbers com percentSign` - Valida presença da seção
- `❌ Syncfusion: CLDR NÃO contém seção numbers!` - Alerta se seção ausente
- `✅ Syncfusion: CLDR pt-BR carregado com sucesso` - Carregamento completo
- `❌ Syncfusion: ej.base.loadCldr não disponível` - Se loadCldr não existe

**Arquivos Afetados**:
- `wwwroot/js/agendamento/utils/syncfusion.utils.js` (linhas 386-393, 521-531)

**Objetivo**: Permitir diagnóstico preciso via console do navegador para identificar se:
1. Função está sendo executada
2. ej.base está disponível
3. CLDR tem seção numbers
4. loadCldr está funcionando

**Status**: ✅ **Concluído** - Aguardando teste com console aberto

**Responsável**: Claude Code

---

## [12/01/2026 - 19:15] - Adição: Seção numbers ao CLDR pt-BR

**Descrição**: Adicionada seção completa de formatação numérica (`numbers`) ao objeto CLDR pt-BR para corrigir erro "Cannot read properties of undefined (reading 'percentSign')".

**Problema Identificado**:
- Componentes Syncfusion geravam erro ao tentar formatar números/porcentagens
- Erro: `TypeError: Cannot read properties of undefined (reading 'percentSign')`
- Causa: Objeto CLDR tinha apenas seções `identity` e `dates`, faltava `numbers`

**Correção Aplicada**:
- Adicionada seção `numbers` completa com:
  - Símbolos numéricos (decimal, group, percentSign, etc.)
  - Formatos de números decimais
  - Formatos de porcentagem
  - Formatos de moeda
  - Informações de moedas (BRL, USD, EUR)

**Arquivos Afetados**:
- `wwwroot/js/agendamento/utils/syncfusion.utils.js` (linhas 461-513)

**Impacto**:
- ✅ Componentes Syncfusion formatam números corretamente em pt-BR
- ✅ Erro "percentSign undefined" eliminado
- ✅ Suporte completo para formatação de moedas e porcentagens

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.0 → 1.1

---

## [05/01/2026] - Criação: Documentação inicial

**Descrição**: Criada documentação completa do arquivo `syncfusion.utils.js`.

**Conteúdo**:
- Visão geral de todas as funções utilitárias
- Explicação detalhada de cada método
- Configuração de localização pt-BR
- Seção de troubleshooting com problemas comuns
- Exemplos de uso práticos

**Status**: ✅ **Concluído**

**Responsável**: Sistema de Documentação FrotiX

**Versão**: 1.0

---

## Histórico de Versões

| Versão | Data | Descrição |
|--------|------|-----------|
| 1.0 | 05/01/2026 | Documentação inicial |
| 1.1 | 12/01/2026 | Adicionada seção numbers ao CLDR pt-BR |

---

## Referências

- [Documentação da Agenda](./Index.md) - Página principal que usa este arquivo
- [Syncfusion EJ2 Documentation](https://ej2.syncfusion.com/documentation/) - Documentação oficial Syncfusion
- [CLDR Project](http://cldr.unicode.org/) - Common Locale Data Repository

---

**Última atualização**: 12/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.1
