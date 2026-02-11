# 🧠 Kendo Fuzzy Validator v2.0

## Sistema Inteligente de Validação para Origem e Destino

**Última Atualização:** 11/02/2026
**Versão:** 2.0
**Autor:** Claude Sonnet 4.5 (FrotiX Team)

---

## 📋 Índice

1. [Visão Geral](#visão-geral)
2. [Funcionalidades](#funcionalidades)
3. [Arquitetura](#arquitetura)
4. [Instalação](#instalação)
5. [Configuração](#configuração)
6. [API Pública](#api-pública)
7. [Algoritmos](#algoritmos)
8. [Métricas e Histórico](#métricas-e-histórico)
9. [Troubleshooting](#troubleshooting)
10. [Changelog](#changelog)

---

## 🎯 Visão Geral

O **Kendo Fuzzy Validator** é um sistema avançado de validação inteligente que detecta automaticamente:

- ✅ **Duplicatas** - Valores idênticos digitados de formas diferentes
- ✅ **Similaridades** - Valores muito parecidos (>85% de similaridade)
- ✅ **Auto-correção** - Correção automática para formato canônico
- ✅ **Validação cruzada** - Origem e Destino não podem ser muito similares
- ✅ **Histórico** - Rastreamento de correções realizadas

### Exemplo de Uso

**Cenário 1: Duplicata detectada**
```
Usuário digita: "Brasilia"
Sistema detecta: Já existe "Brasília" na lista
Ação: Auto-corrige para "Brasília" (98% similaridade)
```

**Cenário 2: Valor similar**
```
Usuário digita: "São Paulo - SP"
Sistema detecta: Já existe "São Paulo" na lista
Ação: Alerta warning (92% similaridade) + sugestão
```

**Cenário 3: Origem = Destino**
```
Origem: "Rio de Janeiro"
Destino: "Rio de Janero" (erro de digitação)
Ação: Warning - campos muito parecidos (95% similaridade)
```

---

## ✨ Funcionalidades

### 🆕 Versão 2.0 (Novidades)

| Funcionalidade | Descrição | Ativado por padrão |
|----------------|-----------|-------------------|
| **Adaptado para Kendo UI** | Migração completa de Syncfusion EJ2 para Kendo ComboBox | ✅ |
| **Debouncing inteligente** | Evita validações excessivas (300ms de delay) | ✅ |
| **Cache de validações** | Performance otimizada (5 min de expiração) | ✅ |
| **Highlight visual** | Efeitos visuais de feedback (animações CSS) | ✅ |
| **Histórico persistente** | LocalStorage com até 100 correções | ✅ |
| **Auto-correção** | Correção automática acima de 98% de similaridade | ✅ |
| **Métricas de qualidade** | Tracking de validações, duplicatas, correções | ✅ |
| **Sugestões inteligentes** | Mostra melhor match com % de similaridade | ✅ |

### ⚙️ Versão 1.0 (Base Original)

- Algoritmo de Levenshtein para cálculo de distância
- Normalização de texto (remove acentos, espaços, case)
- Thresholds configuráveis (info/warning)
- Alertas via sistema Alerta do FrotiX

---

## 🏗️ Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site.OLD/
├── wwwroot/
│   ├── css/
│   │   └── kendo-fuzzy-validator.css          # Estilos de highlight
│   └── js/
│       └── viagens/
│           ├── kendo-fuzzy-validator.js        # Módulo principal
│           └── KENDO_FUZZY_VALIDATOR_README.md # Esta documentação
└── Pages/
    └── Viagens/
        └── Upsert.cshtml                       # Página que usa o sistema
```

### Fluxo de Validação

```
┌──────────────────────────────┐
│  Usuário digita no ComboBox  │
└──────────┬───────────────────┘
           │
           ▼
┌──────────────────────────────┐
│  Evento 'change' disparado   │
└──────────┬───────────────────┘
           │
           ▼
┌──────────────────────────────┐
│  Debounce (300ms)            │
└──────────┬───────────────────┘
           │
           ▼
┌──────────────────────────────┐
│  Verificar cache             │
└──────────┬───────────────────┘
           │
    ┌──────┴──────┐
    │             │
 Cache Hit    Cache Miss
    │             │
    ▼             ▼
┌───────┐   ┌──────────────────┐
│Retorna│   │ Normalizar texto │
└───────┘   └─────────┬────────┘
                      │
                      ▼
            ┌──────────────────────┐
            │ Calcular similaridade│
            └─────────┬────────────┘
                      │
         ┌────────────┼────────────┐
         │            │            │
    ≥98% similar  ≥92% similar  ≥85% similar
         │            │            │
         ▼            ▼            ▼
   Auto-corrige   Warning       Info
         │            │            │
         ▼            ▼            ▼
   ┌──────────────────────────────┐
   │  Salvar no cache e histórico │
   └──────────────────────────────┘
```

---

## 🚀 Instalação

### Passo 1: Arquivos necessários

Certifique-se de que os arquivos estão presentes:

```html
<!-- No <head> ou @section HeadBlock -->
<link href="~/css/kendo-fuzzy-validator.css" rel="stylesheet" asp-append-version="true" />

<!-- Antes do fechamento de </body> ou @section ScriptsBlock -->
<script src="~/js/viagens/kendo-fuzzy-validator.js" asp-append-version="true"></script>
```

### Passo 2: Garantir que Kendo ComboBox está inicializado

```javascript
// Os ComboBox precisam existir ANTES de inicializar o Fuzzy Validator
$("#cmbOrigem").kendoComboBox({
    dataSource: listaOrigem,
    filter: "contains",
    // ... outras opções
});

$("#cmbDestino").kendoComboBox({
    dataSource: listaDestino,
    filter: "contains",
    // ... outras opções
});
```

### Passo 3: Inicializar o sistema

```javascript
// Aguardar 200ms para garantir que os ComboBox estão prontos
setTimeout(function() {
    KendoFuzzyValidator.init({
        // Configurações (todas opcionais)
    });
}, 200);
```

---

## ⚙️ Configuração

### Configuração Padrão

```javascript
KendoFuzzyValidator.init(); // Usa valores padrão
```

### Configuração Personalizada

```javascript
KendoFuzzyValidator.init({
    // Thresholds de similaridade (0.0 a 1.0)
    thresholds: {
        info: 0.85,      // 85% = Alerta informativo (azul)
        warning: 0.92,   // 92% = Alerta warning (laranja)
        critical: 0.98   // 98% = Auto-correção automática
    },

    // Debouncing (milissegundos)
    debounceDelay: 300,  // Aguardar 300ms após parar de digitar

    // Cache
    enableCache: true,
    cacheExpiration: 5 * 60 * 1000,  // 5 minutos

    // Histórico
    enableHistory: true,
    historyStorageKey: 'frotix_fuzzy_history',
    maxHistoryEntries: 100,

    // Auto-correção
    autoCorrect: true,        // Corrigir automaticamente acima do threshold critical
    showSuggestions: true,    // Mostrar sugestão nos alertas

    // Highlight visual
    enableHighlight: true,
    highlightDuration: 2000   // 2 segundos
});
```

### Desabilitar Auto-correção

```javascript
KendoFuzzyValidator.init({
    autoCorrect: false  // Apenas alertar, nunca corrigir automaticamente
});
```

### Ajustar Sensibilidade

```javascript
// Mais rigoroso (detecta mais duplicatas)
KendoFuzzyValidator.init({
    thresholds: {
        info: 0.80,      // 80%
        warning: 0.88,   // 88%
        critical: 0.95   // 95%
    }
});

// Mais permissivo (detecta menos duplicatas)
KendoFuzzyValidator.init({
    thresholds: {
        info: 0.90,      // 90%
        warning: 0.95,   // 95%
        critical: 0.99   // 99%
    }
});
```

---

## 📖 API Pública

### `KendoFuzzyValidator.init(config)`

Inicializa o sistema de validação.

**Parâmetros:**
- `config` (Object, opcional) - Objeto de configuração

**Retorno:**
- `true` - Sistema inicializado com sucesso
- `false` - Erro na inicialização

**Exemplo:**
```javascript
const success = KendoFuzzyValidator.init();
if (success) {
    console.log('Sistema inicializado!');
}
```

---

### `KendoFuzzyValidator.getMetrics()`

Retorna métricas de uso do sistema.

**Retorno:**
```javascript
{
    totalValidations: 150,       // Total de validações realizadas
    duplicatesDetected: 12,      // Duplicatas detectadas
    autoCorrections: 8,          // Correções automáticas
    userCorrections: 4,          // Correções manuais do usuário
    avgSimilarityScore: 0.93,    // Score médio de similaridade
    lastValidation: Date         // Data/hora da última validação
}
```

**Exemplo:**
```javascript
const metrics = KendoFuzzyValidator.getMetrics();
console.log(`Validações: ${metrics.totalValidations}`);
console.log(`Duplicatas detectadas: ${metrics.duplicatesDetected}`);
console.log(`Taxa de auto-correção: ${(metrics.autoCorrections / metrics.duplicatesDetected * 100).toFixed(1)}%`);
```

---

### `KendoFuzzyValidator.getHistory()`

Retorna histórico de correções do localStorage.

**Retorno:**
```javascript
[
    {
        timestamp: "2026-02-11T14:30:00.000Z",
        field: "Origem",
        comboId: "cmbOrigem",
        original: "Brasilia",
        corrected: "Brasília",
        similarity: 98
    },
    {
        timestamp: "2026-02-11T14:28:00.000Z",
        field: "Destino",
        comboId: "cmbDestino",
        original: "Sao Paulo",
        corrected: "São Paulo",
        similarity: 95
    }
    // ... até 100 entradas
]
```

**Exemplo:**
```javascript
const history = KendoFuzzyValidator.getHistory();
console.table(history.slice(0, 10));  // Últimas 10 correções
```

---

### `KendoFuzzyValidator.clearHistory()`

Limpa histórico de correções do localStorage.

**Exemplo:**
```javascript
KendoFuzzyValidator.clearHistory();
console.log('Histórico limpo!');
```

---

### `KendoFuzzyValidator.version`

Propriedade somente leitura com a versão do sistema.

**Exemplo:**
```javascript
console.log(`Versão: ${KendoFuzzyValidator.version}`);  // "2.0"
```

---

## 🧮 Algoritmos

### Normalização de Texto

Remove acentos, espaços extras e normaliza caixa para comparação:

```javascript
normalizeText("São Paulo - SP") → "sao paulo  sp"
normalizeText("  BrAsÍlIa  ")   → "brasilia"
```

**Passos:**
1. Normalização Unicode NFKC
2. Remoção de caracteres invisíveis (zero-width, soft hyphen)
3. Conversão para minúsculas
4. Normalização NFD + remoção de diacríticos (acentos)
5. Normalização de espaços (múltiplos → único)
6. Trim

---

### Distância de Levenshtein

Algoritmo de programação dinâmica para calcular o número mínimo de edições (inserção, deleção, substituição) necessárias para transformar uma string em outra.

**Complexidade:** O(n × m) - onde n e m são os tamanhos das strings

**Otimização:** Usa apenas 2 linhas da matriz DP em vez de n+1 (economia de memória)

**Exemplo:**
```
levenshteinDistance("Brasília", "Brasilia") → 1  (1 substituição: í → i)
levenshteinDistance("São Paulo", "Sao Paulo") → 1  (1 substituição: ã → a)
levenshteinDistance("Rio", "Belo Horizonte") → 13  (muitas diferenças)
```

---

### Score de Similaridade

Normaliza a distância de Levenshtein para um score entre 0.0 e 1.0:

```
similarity = 1 - (levenshteinDistance / maxLength)
```

Onde `maxLength` é o comprimento da string mais longa.

**Exemplo:**
```javascript
calculateSimilarity("Brasília", "Brasilia")   → 0.988  (98.8%)
calculateSimilarity("São Paulo", "Sao Paulo") → 0.900  (90.0%)
calculateSimilarity("Rio", "Belo Horizonte")  → 0.071  (7.1%)
```

---

## 📊 Métricas e Histórico

### Visualizar Métricas (Console)

```javascript
const m = KendoFuzzyValidator.getMetrics();
console.log(`
╔════════════════════════════════════════╗
║   MÉTRICAS DO FUZZY VALIDATOR v2.0     ║
╠════════════════════════════════════════╣
║ Validações totais:       ${m.totalValidations}
║ Duplicatas detectadas:   ${m.duplicatesDetected}
║ Auto-correções:          ${m.autoCorrections}
║ Correções manuais:       ${m.userCorrections}
║ Score médio:             ${(m.avgSimilarityScore * 100).toFixed(1)}%
║ Última validação:        ${m.lastValidation ? m.lastValidation.toLocaleString('pt-BR') : 'N/A'}
╚════════════════════════════════════════╝
`);
```

### Exportar Histórico para Excel/CSV

```javascript
// Copiar para clipboard (colar no Excel)
const history = KendoFuzzyValidator.getHistory();
const csv = history.map(h =>
    `${h.timestamp},${h.field},${h.original},${h.corrected},${h.similarity}%`
).join('\n');
navigator.clipboard.writeText('Timestamp,Campo,Original,Corrigido,Similaridade\n' + csv);
console.log('Histórico copiado para clipboard!');
```

### Limpar Histórico Antigo

```javascript
// Manter apenas últimos 7 dias
const history = KendoFuzzyValidator.getHistory();
const sevenDaysAgo = Date.now() - (7 * 24 * 60 * 60 * 1000);
const recentHistory = history.filter(h => new Date(h.timestamp) > sevenDaysAgo);
localStorage.setItem('frotix_fuzzy_history', JSON.stringify(recentHistory));
console.log(`Histórico filtrado: ${history.length} → ${recentHistory.length} entradas`);
```

---

## 🐛 Troubleshooting

### Problema: "KendoFuzzyValidator is not defined"

**Causa:** Script não foi carregado ou carregou após tentativa de inicialização

**Solução:**
```javascript
// ❌ ERRADO - Inicializar imediatamente
KendoFuzzyValidator.init();

// ✅ CORRETO - Aguardar carregamento
setTimeout(() => {
    if (typeof KendoFuzzyValidator !== 'undefined') {
        KendoFuzzyValidator.init();
    }
}, 200);
```

---

### Problema: Validação não está funcionando

**Diagnóstico:**
```javascript
// 1. Verificar se ComboBox existe
console.log($("#cmbOrigem").data("kendoComboBox"));  // Deve retornar objeto

// 2. Verificar dataSource
const combo = $("#cmbOrigem").data("kendoComboBox");
console.log(combo.dataSource.data());  // Deve ter dados

// 3. Verificar se fuzzy foi inicializado
console.log(KendoFuzzyValidator.getMetrics());  // Deve ter métricas
```

**Solução:**
- Garantir que ComboBox foi inicializado ANTES do Fuzzy Validator
- Verificar se dataSource tem dados
- Ver console para erros

---

### Problema: Auto-correção muito agressiva

**Solução:**
```javascript
// Aumentar threshold critical para 99%
KendoFuzzyValidator.init({
    thresholds: {
        critical: 0.99  // Só auto-corrige com 99% de certeza
    }
});

// OU desabilitar completamente
KendoFuzzyValidator.init({
    autoCorrect: false
});
```

---

### Problema: Muitos alertas

**Solução:**
```javascript
// Aumentar thresholds (mais permissivo)
KendoFuzzyValidator.init({
    thresholds: {
        info: 0.90,      // Só alerta acima de 90%
        warning: 0.95    // Só warning acima de 95%
    }
});
```

---

## 📝 Changelog

### Versão 2.0 (11/02/2026)

**🆕 Novidades:**
- ✅ Migração completa de Syncfusion EJ2 para Kendo UI ComboBox
- ✅ Debouncing inteligente (evita validações excessivas)
- ✅ Sistema de cache com expiração (5min)
- ✅ Highlight visual com animações CSS
- ✅ Histórico persistente (localStorage, até 100 entradas)
- ✅ Métricas de qualidade de dados
- ✅ API pública para consulta de métricas e histórico
- ✅ Documentação completa

**🔧 Melhorias:**
- Performance: Cache reduz validações repetidas
- UX: Animações visuais de feedback
- Rastreabilidade: Histórico de todas as correções
- Configurabilidade: Thresholds e comportamentos configuráveis

**🐛 Correções:**
- Corrigido problema de inicialização com Syncfusion
- Corrigido validação cruzada Origem/Destino
- Corrigido normalização de textos com caracteres especiais

---

### Versão 1.0 (Data desconhecida)

**Base original (Syncfusion EJ2):**
- Algoritmo de Levenshtein
- Normalização de texto
- Validação de duplicatas
- Validação cruzada
- Thresholds configuráveis
- Alertas via sistema Alerta

---

## 🤝 Contribuições

Para reportar bugs ou sugerir melhorias, contate o time de desenvolvimento FrotiX.

---

## 📄 Licença

© 2026 FrotiX - Todos os direitos reservados.

---

**Desenvolvido com ❤️ por Claude Sonnet 4.5 (FrotiX Team)**
