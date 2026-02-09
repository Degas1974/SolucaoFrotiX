# 🔧 Guia de Troubleshooting: Controles Kendo UI e Telerik

> **Versão**: 1.0  
> **Última Atualização**: 09/02/2026  
> **Projeto**: FrotiX.Site.2026.01

---

## 📋 Índice

1. [Visão Geral](#visão-geral)
2. [Problemas Comuns](#problemas-comuns)
3. [Soluções Implementadas](#soluções-implementadas)
4. [Estrutura de Arquivos](#estrutura-de-arquivos)
5. [Checklist de Diagnóstico](#checklist-de-diagnóstico)
6. [Boas Práticas](#boas-práticas)

---

## 🎯 Visão Geral

O projeto FrotiX utiliza:
- **Kendo UI** (versão local) para controles de UI avançados
- **Telerik Report Viewer** (versão 18.1.24.514) para visualização de relatórios
- **Syncfusion EJ2** (versão 32.1.19) para componentes adicionais

### ⚠️ Configuração Atual

- **Kendo UI**: Instalação LOCAL (não CDN) em `wwwroot/lib/kendo/`
- **Telerik Reports**: Servido via endpoint `/api/reports/resources/`
- **Licenciamento**: Arquivos protegidos, não devem ser modificados automaticamente

---

## 🐛 Problemas Comuns

### 1. ❌ CSS do Kendo UI Não Carrega (404)

**Sintomas:**
- Controles Kendo aparecem sem estilização
- Console mostra erro 404 para arquivo CSS
- Layout quebrado em componentes Kendo (grids, dropdowns, etc.)

**Causa:**
```html
<!-- ERRADO - Caminho incorreto -->
<link rel="stylesheet" href="~/lib/kendo/styles/themes/bootstrap/bootstrap-main.css" />
```

O diretório `themes/bootstrap/` não existe. Os arquivos CSS estão diretamente em `styles/`.

**Solução:**
```html
<!-- CORRETO -->
<link rel="stylesheet" href="~/lib/kendo/styles/bootstrap-main.css" asp-append-version="true" />
```

**Arquivo Afetado:** `Pages/Shared/_Head.cshtml` (linha 101)

---

### 2. ⚠️ Erros de "collapsible" ou "toggle" no Console

**Sintomas:**
```
Cannot read properties of undefined (reading 'toggle')
Cannot read properties of undefined (reading 'collapsible')
```

**Causa:**
- Inicialização prematura de controles Kendo
- DOM não totalmente carregado
- Conflito com outros frameworks

**Solução:**
O arquivo `wwwroot/js/kendo-error-suppressor.js` já está implementado e suprime esses erros conhecidos.

**Verificar:**
1. Se o suppressor está carregado PRIMEIRO em `_ScriptsBasePlugins.cshtml`
2. Se a ordem de carregamento está correta (ver seção Ordem de Scripts)

---

### 3. 🔴 Telerik Report Viewer Não Carrega

**Sintomas:**
- Relatórios não são exibidos
- Erro 404 para `telerikReportViewer-*.js`
- Console mostra erro de módulo não encontrado

**Causas Possíveis:**
1. Endpoint `/api/reports/resources/` não configurado corretamente
2. Versão do Report Viewer incompatível
3. Falta de licença válida

**Solução:**
```html
<!-- Versão atual em uso -->
<script src="/api/reports/resources/js/telerikReportViewer-18.1.24.514.min.js"></script>
```

**Verificar:**
1. Se o serviço de Reports está rodando
2. Se a rota `/api/reports/resources/` retorna arquivos corretamente
3. Logs do servidor para erros relacionados a Telerik

---

### 4. 💡 Erros de Formatação Syncfusion (percentSign, currencySign)

**Sintomas:**
```
Cannot read properties of undefined (reading 'percentSign')
Cannot read properties of undefined (reading 'currencySign')
```

**Causa:**
- Scripts de formatação Syncfusion carregam antes do CLDR
- Problema conhecido do Syncfusion EJ2

**Solução:**
O `kendo-error-suppressor.js` já trata esses erros. Verificar se está ativo:

```javascript
console.log('[SUPRESSOR] ✅ Ativo - erros do Kendo e Syncfusion serão suprimidos');
```

---

## ✅ Soluções Implementadas

### 1. Correção do Caminho CSS

**Antes:**
```html
<link rel="stylesheet" href="~/lib/kendo/styles/themes/bootstrap/bootstrap-main.css" />
```

**Depois:**
```html
<link rel="stylesheet" href="~/lib/kendo/styles/bootstrap-main.css" asp-append-version="true" />
```

### 2. Supressor de Erros Ativo

**Arquivo:** `wwwroot/js/kendo-error-suppressor.js`

Suprime automaticamente:
- ✅ Erros `collapsible` e `toggle` do Kendo
- ✅ Erros `percentSign` e `currencySign` do Syncfusion
- ✅ Erros de formatação antes do carregamento do CLDR
- ✅ Erros de Promise rejection relacionados

### 3. Ordem Correta de Scripts

**Arquivo:** `Pages/Shared/_ScriptsBasePlugins.cshtml`

```html
<!-- 1. PRIMEIRO: Supressor de erros -->
<script src="~/js/kendo-error-suppressor.js"></script>

<!-- 2. jQuery -->
<script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>

<!-- 3. Kendo UI -->
<script src="~/lib/kendo/js/jszip.min.js"></script>
<script src="~/lib/kendo/js/kendo.all.min.js"></script>
<script src="~/lib/kendo/js/kendo.aspnetmvc.min.js"></script>

<!-- 4. Cultura pt-BR -->
<script src="~/lib/kendo/js/cultures/kendo.culture.pt-BR.min.js"></script>
<script src="~/lib/kendo/js/messages/kendo.messages.pt-BR.min.js"></script>

<!-- 5. Telerik Report Viewer -->
<script src="/api/reports/resources/js/telerikReportViewer-18.1.24.514.min.js"></script>

<!-- 6. Syncfusion -->
<script src="https://cdn.syncfusion.com/ej2/32.1.19/dist/ej2.min.js"></script>
```

**⚠️ IMPORTANTE:** Nunca altere esta ordem sem necessidade!

---

## 📁 Estrutura de Arquivos

### Kendo UI (Local)

```
FrotiX.Site.2026.01/
└── wwwroot/
    └── lib/
        └── kendo/
            ├── js/
            │   ├── kendo.all.min.js          ✅ Script principal
            │   ├── kendo.aspnetmvc.min.js    ✅ Integração ASP.NET
            │   ├── jszip.min.js              ✅ Dependência para Excel
            │   ├── cultures/
            │   │   └── kendo.culture.pt-BR.min.js  ✅ Localização
            │   └── messages/
            │       └── kendo.messages.pt-BR.min.js ✅ Mensagens traduzidas
            └── styles/
                ├── bootstrap-main.css         ✅ Tema principal
                ├── bootstrap-3.css
                ├── bootstrap-4.css
                ├── bootstrap-main-dark.css
                └── ...outras variantes
```

### Telerik Reports

```
Endpoint: /api/reports/resources/js/
├── telerikReportViewer-18.1.24.514.min.js  ✅ Em uso
└── telerikReportViewer-19.1.25.521.min.js  ⚠️ Comentado (versão mais nova)
```

### Scripts Críticos

```
FrotiX.Site.2026.01/
└── wwwroot/
    └── js/
        ├── kendo-error-suppressor.js     ✅ Supressor de erros
        ├── localization-init.js          ✅ Inicialização de localização
        └── agendamento/
            └── utils/
                ├── kendo-datetime.js     ✅ Helpers para DateTimePicker
                └── kendo-editor-helper.js ✅ Helpers para Editor
```

---

## ✔️ Checklist de Diagnóstico

Use este checklist quando encontrar problemas com Kendo/Telerik:

### 🔍 Fase 1: Verificação Inicial

- [ ] Abrir DevTools (F12) e verificar aba Console
- [ ] Verificar aba Network para erros 404
- [ ] Verificar se há erros relacionados a "kendo", "telerik" ou "syncfusion"

### 🎯 Fase 2: CSS e Estilos

- [ ] Verificar se `bootstrap-main.css` carrega sem erro 404
- [ ] Inspecionar elemento Kendo para ver se tem classes CSS aplicadas
- [ ] Verificar caminho correto: `~/lib/kendo/styles/bootstrap-main.css`
- [ ] Confirmar que `asp-append-version="true"` está presente

### 📜 Fase 3: Scripts JavaScript

- [ ] Verificar se `kendo-error-suppressor.js` carrega PRIMEIRO
- [ ] Verificar se jQuery carrega antes do Kendo
- [ ] Verificar ordem: jQuery → Kendo → Cultura → Telerik → Syncfusion
- [ ] Verificar no console se aparece: `[SUPRESSOR] ✅ Ativo`
- [ ] Verificar se aparece: `✅ Kendo UI cultura pt-BR configurada`

### 🌐 Fase 4: Telerik Reports

- [ ] Verificar se endpoint `/api/reports/resources/` responde
- [ ] Verificar versão do viewer: `18.1.24.514`
- [ ] Verificar se `ReportsController` está funcionando
- [ ] Verificar logs do servidor para erros de licença

### 🔧 Fase 5: Controles Específicos

- [ ] Grid: Verificar se inicializa com `.kendoGrid()`
- [ ] DatePicker: Verificar se aceita cultura pt-BR
- [ ] Upload: Verificar endpoint de upload configurado
- [ ] Editor: Verificar se toolbar aparece corretamente

---

## 📝 Boas Práticas

### ✅ DO (Faça)

1. **Sempre use arquivos locais** para Kendo UI (não CDN)
2. **Mantenha a ordem de scripts** em `_ScriptsBasePlugins.cshtml`
3. **Use `asp-append-version="true"`** para cache busting
4. **Configure cultura pt-BR** para todos os controles
5. **Documente alterações** em configuração de UI
6. **Teste em múltiplos browsers** (Chrome, Edge, Firefox)

### ❌ DON'T (Não Faça)

1. **Não use CDN** para Kendo UI (licenciamento)
2. **Não altere ordem de scripts** sem necessidade
3. **Não remova `kendo-error-suppressor.js`**
4. **Não use múltiplas versões** do mesmo controle
5. **Não modifique arquivos Kendo** diretamente
6. **Não ignore erros 404** de recursos

---

## 🔗 Referências Rápidas

### Documentação Oficial

- [Kendo UI for jQuery](https://docs.telerik.com/kendo-ui/introduction)
- [Telerik Reporting](https://docs.telerik.com/reporting)
- [Syncfusion EJ2](https://ej2.syncfusion.com/documentation/)

### Arquivos Críticos do Projeto

| Arquivo | Propósito |
|---------|-----------|
| `Pages/Shared/_Head.cshtml` | Carrega CSS do Kendo e outras dependências |
| `Pages/Shared/_ScriptsBasePlugins.cshtml` | Ordem de carregamento de todos os scripts |
| `wwwroot/js/kendo-error-suppressor.js` | Suprime erros conhecidos |
| `TELERIK_ARQUIVOS_MANUAIS.md` | Lista de 85 arquivos com referências a Kendo/Telerik |
| `RegrasDesenvolvimentoFrotiX.md` | Regras gerais do projeto |

### Scripts de Utilidade

```javascript
// Verificar se Kendo está carregado
console.log(typeof kendo !== 'undefined' ? '✅ Kendo OK' : '❌ Kendo não carregado');

// Verificar cultura atual
console.log('Cultura:', kendo.culture().name);

// Verificar versão do Kendo
console.log('Versão:', kendo.version);

// Listar widgets Kendo na página
console.log('Widgets:', $('[data-role]').length);
```

---

## 🆘 Suporte

### Problemas Persistentes?

Se após seguir este guia o problema persistir:

1. ✅ Verificar se todos os itens do checklist foram cumpridos
2. 📸 Fazer screenshot do erro no console
3. 📋 Copiar stack trace completo
4. 🔍 Verificar `TELERIK_ARQUIVOS_MANUAIS.md` se o arquivo problemático está na lista
5. 📝 Documentar o problema e contexto

### Erros de Licenciamento

Se aparecer erro relacionado a licença:
- Verificar se os arquivos Kendo estão corrompidos
- Não redistribuir arquivos Kendo sem autorização
- Contatar administrador do sistema para renovação de licença

---

## 📅 Histórico de Alterações

| Data | Versão | Mudança |
|------|--------|---------|
| 09/02/2026 | 1.0 | ✅ Correção do caminho CSS do Kendo UI |
| 09/02/2026 | 1.0 | 📄 Criação deste documento de troubleshooting |

---

**🎯 Lembre-se:** Kendo UI e Telerik são ferramentas poderosas, mas requerem configuração correta e ordem de carregamento adequada. Este guia deve ser sua primeira referência ao encontrar problemas.
