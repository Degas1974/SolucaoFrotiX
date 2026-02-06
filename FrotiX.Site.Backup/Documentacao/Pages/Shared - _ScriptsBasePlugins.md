# Documentação: Shared - \_ScriptsBasePlugins

> **Última Atualização**: 23/01/2026
> **Versão Atual**: 1.3

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Grupos de Scripts](#grupos-de-scripts)
4. [CLDR e Localização](#cldr-e-localização)

---

## Visão Geral

Arquivo parcial que carrega todos os scripts e plugins base do sistema FrotiX. Incluído no `_Layout.cshtml` e define a ordem de carregamento dos scripts.

### Características Principais

- ✅ **Ordem Crítica**: Scripts carregados em ordem específica para evitar conflitos
- ✅ **CDNs**: Uso de CDNs para bibliotecas externas
- ✅ **Syncfusion EJ2**: Configuração completa com CLDR pt-BR
- ✅ **SignalR**: Gerenciamento de conexão em tempo real
- ✅ **Licenciamento**: Registro de licenças (Syncfusion, Telerik)

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/Shared/_ScriptsBasePlugins.cshtml
├── Pages/Shared/_Layout.cshtml (inclui este arquivo)
```

### Grupos de Scripts (Ordem de Carregamento)

| Grupo | Descrição                                     |
| ----- | --------------------------------------------- |
| 1     | Dependências Base (jQuery, SignalR)           |
| 2     | Funções Globais FrotiX                        |
| 3     | Frameworks UI (Kendo, Syncfusion, Bootstrap)  |
| 4     | Bundles do Projeto                            |
| 5     | Bibliotecas de Dados (Moment, DataTables)     |
| 6     | UI e Alertas (jQuery UI, Toastr, SweetAlert2) |
| 7     | Helpers e Wrappers                            |
| 8     | Funções Utilitárias                           |
| 9     | Configurações Específicas                     |
| 10    | Utilitários de Limpeza                        |
| 11    | Localização Syncfusion (CLDR)                 |

---

## CLDR e Localização

### Carregamento Síncrono de CLDR

Os dados CLDR são carregados de forma **síncrona** imediatamente após o `ej2.min.js` para garantir que todos os componentes Syncfusion tenham acesso aos dados de localização antes de serem inicializados.

**Problema Resolvido**: Erro "Cannot read properties of undefined (reading 'percentSign')" que ocorria quando componentes tentavam formatar números antes do CLDR estar pronto.

**Complemento aplicado (21/01/2026)**: o bloco `cldrCalendar` passou a incluir `dayPeriods`, `eras`, formatos `narrow/short` para meses e dias e o conjunto completo de `dateTimeFormats.availableFormats`, prevenindo falhas no Syncfusion Calendar (`Format options or type given must be invalid`).

**Solução** (linhas 170-300):

```javascript
(function () {
  var cldrNumberingSystems = {
    /* ... */
  };
  var cldrNumbers = {
    /* pt-BR numbers config */
  };
  var cldrCalendar = {
    /* pt-BR calendar config */
  };

  if (typeof ej !== "undefined" && ej.base && ej.base.loadCldr) {
    ej.base.loadCldr(cldrNumberingSystems, cldrNumbers, cldrCalendar);
    ej.base.setCulture("pt-BR");
    window.__cldrLoaded = true;
  }
})();
```

### Traduções L10n Adicionais

Traduções para componentes específicos (PDFViewer, Uploader, DatePicker, etc.) são carregadas após o CLDR base (linhas 541-603).

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [23/01/2026 11:35] - Fallback de símbolos de formatação Syncfusion

**Descrição**:
Adicionado script de fallback que carrega símbolos básicos de formatação (percentSign, decimal, group) **imediatamente** após o `ej2.min.js` carregar, antes do CLDR completo.

**Problema**:
Erro `TypeError: Cannot read properties of undefined (reading 'percentSign')` continuava ocorrendo ao fechar modais, mesmo com o CLDR carregado síncronamente.

**Causa Raiz**:
Durante a destruição de componentes Syncfusion (ao fechar modais), o código tenta formatar números mas em alguns casos os dados CLDR não estão disponíveis no momento exato.

**Solução**:
Script de fallback (linhas 235-269) que garante símbolos mínimos disponíveis:
```javascript
var fallbackSymbols = {
    'main': {
        'pt-BR': {
            'numbers': {
                'symbols-numberSystem-latn': {
                    'decimal': ',',
                    'group': '.',
                    'percentSign': '%',
                    // ... outros símbolos
                }
            }
        }
    }
};
ej.base.loadCldr(fallbackSymbols);
```

**Arquivos Afetados**:
- `Pages/Shared/_ScriptsBasePlugins.cshtml` (linhas 235-269)

**Impacto**: Elimina erros de formatação ao fechar modais

**Status**: ✅ Concluído

**Versão**: 1.3

---

## [23/01/2026 09:10] - Tratamento de erros em scripts globais

**Descrição**:
Inclusão de `try-catch` nas funções JavaScript do partial para conformidade com as regras FrotiX.

**Mudanças**:

1. Adicionados `try-catch` em inicialização do SignalR, cultura do Kendo, PDF.js, CLDR e licença Syncfusion.
2. Uso de `Alerta.TratamentoErroComLinha` para rastreabilidade.

**Arquivos Afetados**:

- `Pages/Shared/_ScriptsBasePlugins.cshtml`

**Impacto**: Redução de falhas silenciosas em inicializações críticas.

**Status**: ✅ Concluído

**Versão**: 1.2

---

## [21/01/2026 12:16] - Complemento CLDR Calendar (dayPeriods/eras/availableFormats)

**Descrição**:
Expansão do objeto `cldrCalendar` com dados completos de períodos do dia (AM/PM), eras e formatos de data/hora adicionais, conforme CLDR oficial.

**Problema**:
O Syncfusion Calendar tentava formatar datas com padrões não presentes no CLDR reduzido, resultando no erro:

```
Format options or type given must be invalid
```

**Solução**:

1. Adicionados `dayPeriods` e `eras` no calendário gregoriano
2. Inclusão de `narrow/short` para meses e dias
3. `dateTimeFormats.availableFormats` incluído para suportar padrões internos do Syncfusion

**Arquivos Afetados**:

- `Pages/Shared/_ScriptsBasePlugins.cshtml` (bloco `cldrCalendar`)

**Impacto**: Calendar Syncfusion passa a renderizar datas sem erro de formato

**Status**: ✅ Concluído

**Versão**: 1.1

---

## [12/01/2026 16:00] - CLDR Síncrono para Syncfusion

**Descrição**:
Alterado carregamento de dados CLDR de assíncrono para síncrono para evitar erro "percentSign" em componentes Syncfusion.

**Problema**:
Componentes Syncfusion (especialmente PDFViewer com NumericTextBox interno) tentavam formatar números antes dos dados CLDR estarem carregados, causando erro:

```
Cannot read properties of undefined (reading 'percentSign')
```

**Solução**:

1. Dados CLDR agora são carregados inline, síncronamente, logo após `ej2.min.js`
2. Inclui: numberingSystems, numbers (pt-BR), calendar (pt-BR)
3. Código de carregamento assíncrono antigo foi simplificado para apenas carregar traduções L10n adicionais

**Arquivos Afetados**:

- `Pages/Shared/_ScriptsBasePlugins.cshtml` (linhas 170-300, 541-603)

**Impacto**: Corrige erro de formatação em todas as páginas que usam componentes Syncfusion

**Status**: ✅ **Concluído**

**Versão**: 1.0

---

**Última atualização**: 12/01/2026
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

- âŒ **ANTES**: \_unitOfWork.Entity.AsTracking().Get(id) ou \_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: \_unitOfWork.Entity.GetWithTracking(id) ou \_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

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
