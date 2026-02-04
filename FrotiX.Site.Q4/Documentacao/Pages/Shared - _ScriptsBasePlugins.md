# Documentação: Shared - _ScriptsBasePlugins

> **Última Atualização**: 12/01/2026
> **Versão Atual**: 1.0

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

| Grupo | Descrição |
|-------|-----------|
| 1 | Dependências Base (jQuery, SignalR) |
| 2 | Funções Globais FrotiX |
| 3 | Frameworks UI (Kendo, Syncfusion, Bootstrap) |
| 4 | Bundles do Projeto |
| 5 | Bibliotecas de Dados (Moment, DataTables) |
| 6 | UI e Alertas (jQuery UI, Toastr, SweetAlert2) |
| 7 | Helpers e Wrappers |
| 8 | Funções Utilitárias |
| 9 | Configurações Específicas |
| 10 | Utilitários de Limpeza |
| 11 | Localização Syncfusion (CLDR) |

---

## CLDR e Localização

### Carregamento Síncrono de CLDR

Os dados CLDR são carregados de forma **síncrona** imediatamente após o `ej2.min.js` para garantir que todos os componentes Syncfusion tenham acesso aos dados de localização antes de serem inicializados.

**Problema Resolvido**: Erro "Cannot read properties of undefined (reading 'percentSign')" que ocorria quando componentes tentavam formatar números antes do CLDR estar pronto.

**Solução** (linhas 170-300):
```javascript
(function() {
    var cldrNumberingSystems = { /* ... */ };
    var cldrNumbers = { /* pt-BR numbers config */ };
    var cldrCalendar = { /* pt-BR calendar config */ };

    if (typeof ej !== 'undefined' && ej.base && ej.base.loadCldr) {
        ej.base.loadCldr(cldrNumberingSystems, cldrNumbers, cldrCalendar);
        ej.base.setCulture('pt-BR');
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
