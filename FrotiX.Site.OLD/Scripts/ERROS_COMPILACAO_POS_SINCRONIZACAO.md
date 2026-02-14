# 🚨 ERROS DE COMPILAÇÃO PÓS-SINCRONIZAÇÃO

**Data:** 13/02/2026 21:45
**Total de Erros:** 47
**Causa:** Mudanças nos modelos para refletir banco de dados real

---

## 📋 CATEGORIAS DE ERROS

### 1. ENUM → INT em AlertasFrotiX (37 erros) 🔴

**Problema:** Mudamos `TipoAlerta`, `Prioridade` e `TipoExibicao` de enum para `int`, mas o código ainda usa os enums.

**Arquivos Afetados:**
- `Controllers/AlertasFrotiXController.cs` (26 erros)
- `Services/AlertasBackgroundService.cs` (3 erros)
- `Repository/AlertasFrotiXRepository.cs` (3 erros)
- `Pages/AlertasFrotiX/Upsert.cshtml.cs` (5 erros)

**Solução:** Adicionar conversões explícitas (cast) entre int e enum onde necessário.

**Exemplo de Correção:**

```csharp
// ❌ ANTES (erro):
alerta.TipoAlerta = TipoAlerta.Agendamento;
// Tentando atribuir enum para int

// ✅ DEPOIS (correto):
alerta.TipoAlerta = (int)TipoAlerta.Agendamento;
// Converte enum para int

// ❌ ANTES (erro):
var tipoAlerta = TipoAlerta.Agendamento;
if (alerta.TipoAlerta == tipoAlerta) { }
// Comparando int com enum

// ✅ DEPOIS (correto):
var tipoAlerta = TipoAlerta.Agendamento;
if (alerta.TipoAlerta == (int)tipoAlerta) { }
// Compara int com int

// OU

if ((TipoAlerta)alerta.TipoAlerta == tipoAlerta) { }
// Converte int para enum antes de comparar
```

---

### 2. Propriedades Renomeadas em VeiculoPadraoViagem (8 erros) 🟡

**Problema:** Renomeamos propriedades para refletir banco real, mas `VeiculoPadraoViagemRepository.cs` usa nomes antigos.

**Arquivo:** `Repository/VeiculoPadraoViagemRepository.cs` (linhas 92-95)

**Renomeações Necessárias:**

| Nome Antigo (código) | Nome Novo (modelo) | Tipo |
|----------------------|-------------------|------|
| `TotalViagens` | `TotalViagensAnalisadas` | int? |
| `MediaDuracaoMinutos` | `AvgDuracaoMinutos` | decimal? |
| `MediaKmPorViagem` | `AvgKmPorViagem` | decimal? |
| `MediaKmPorDia` | `AvgKmPorDia` | decimal? |

**Correção:**

```csharp
// ❌ ANTES:
padrao.TotalViagens = totalViagens ?? 0;
padrao.MediaDuracaoMinutos = mediaDuracao ?? 0;
padrao.MediaKmPorViagem = mediaKm ?? 0;
padrao.MediaKmPorDia = mediaKmDia ?? 0;

// ✅ DEPOIS:
padrao.TotalViagensAnalisadas = totalViagens ?? 0;
padrao.AvgDuracaoMinutos = mediaDuracao ?? 0;
padrao.AvgKmPorViagem = mediaKm ?? 0;
padrao.AvgKmPorDia = mediaKmDia ?? 0;
```

---

### 3. CorridasTaxiLeg.QRU - String → Int (1 erro) 🟢

**Problema:** Mudamos `QRU` de `string?` para `int?`, mas código tenta atribuir string.

**Arquivo:** `Controllers/TaxiLegController.cs` (linha 296)

**Correção:**

```csharp
// ❌ ANTES:
corrida.QRU = dadosApi.QRU;  // QRU é string no JSON

// ✅ DEPOIS (opção 1 - converter string para int):
corrida.QRU = int.TryParse(dadosApi.QRU, out int qru) ? qru : (int?)null;

// ✅ DEPOIS (opção 2 - criar propriedade auxiliar):
[NotMapped]
public string? QRUString
{
    get => QRU?.ToString();
    set => QRU = int.TryParse(value, out int qru) ? qru : (int?)null;
}
```

---

### 4. PK Duplicada em VeiculoPadraoViagem (potencial)

**Observação:** O modelo antigo usava `VeiculoId` como PK, mas o banco usa `VeiculoPadraoViagemId` (int identity) como PK.

**Impacto:** Pode haver código tentando usar `VeiculoId` como chave que precisa ser ajustado.

---

## 🔧 ESTRATÉGIAS DE CORREÇÃO

### Opção A: Correção Manual Seletiva ⚡ (RECOMENDADO)
- Corrigir apenas erros críticos que quebram funcionalidades ativas
- Tempo estimado: 30-60 minutos
- Menor risco de introduzir bugs

### Opção B: Correção Automática em Massa 🤖
- Script PowerShell/Regex para substituir padrões
- Tempo estimado: 15 minutos
- Maior risco (pode corrigir código que não deveria)

### Opção C: Conversão Reversa (não recomendado) ⏪
- Desfazer mudanças nos modelos
- Manter enums/nomes antigos
- **NÃO RECOMENDADO** - problema persiste (banco vs modelo desalinhado)

---

## 📝 CHECKLIST DE CORREÇÃO

### Fase 1: Enum Casts (AlertasFrotiX) - 37 erros

**AlertasFrotiXController.cs:**
- [ ] Linha 433: `Prioridade.Baixa` → `(int)Prioridade.Baixa`
- [ ] Linha 434: `Prioridade.Media` → `(int)Prioridade.Media`
- [ ] Linha 719: `TipoAlerta = model.TipoAlerta` → cast
- [ ] Linha 720: `Prioridade = model.Prioridade` → cast
- [ ] Linha 721: `TipoExibicao = model.TipoExibicao` → cast
- [ ] Linha 772: Similar ao 719-721
- [ ] Linha 854: Similar ao 719-721
- [ ] Linha 1087-1089: `TipoAlerta.*` → cast
- [ ] Linha 1139: cast
- [ ] Linha 1143: cast
- [ ] Linha 1217: cast
- [ ] Linha 1377-1378: cast
- [ ] Linha 1451: cast
- [ ] Linha 1455: cast
- [ ] Linha 1524-1525: cast

**AlertasBackgroundService.cs:**
- [ ] Linha 141: `TipoAlerta.Motorista` → cast
- [ ] Linha 142: `TipoAlerta.Veiculo` → cast
- [ ] Linha 143: `TipoAlerta.Manutencao` → cast

**AlertasFrotiXRepository.cs:**
- [ ] Linha 269: `== TipoExibicaoAlerta.RecorrenteDiario` → cast
- [ ] Linha 272: `== TipoExibicaoAlerta.RecorrenteSemanal` → cast
- [ ] Linha 278: `== TipoExibicaoAlerta.RecorrenteQuinzenal` → cast

**Upsert.cshtml.cs:**
- [ ] Linha 246: `TipoAlerta = model.TipoAlerta` → cast
- [ ] Linha 247: `Prioridade = model.Prioridade` → cast
- [ ] Linha 248: `TipoExibicao = model.TipoExibicao` → cast
- [ ] Linha 383: Inverso (int → enum) → cast
- [ ] Linha 384: Inverso → cast
- [ ] Linha 385: Inverso → cast
- [ ] Linha 488: Similar ao 383-385

---

### Fase 2: Propriedades VeiculoPadraoViagem - 8 erros

**VeiculoPadraoViagemRepository.cs:**
- [ ] Linha 92: `TotalViagens` → `TotalViagensAnalisadas` (2x)
- [ ] Linha 93: `MediaDuracaoMinutos` → `AvgDuracaoMinutos` (2x)
- [ ] Linha 94: `MediaKmPorViagem` → `AvgKmPorViagem` (2x)
- [ ] Linha 95: `MediaKmPorDia` → `AvgKmPorDia` (2x)

---

### Fase 3: CorridasTaxiLeg.QRU - 1 erro

**TaxiLegController.cs:**
- [ ] Linha 296: Adicionar conversão `string → int?`

---

## ⏱️ ESTIMATIVA DE TEMPO

| Fase | Erros | Tempo Estimado | Dificuldade |
|------|-------|----------------|-------------|
| Fase 1 (Enums) | 37 | 40 minutos | 🟡 Média |
| Fase 2 (Rename) | 8 | 10 minutos | 🟢 Fácil |
| Fase 3 (QRU) | 1 | 5 minutos | 🟢 Fácil |
| **TOTAL** | **47** | **55 minutos** | - |

---

## 🎯 RECOMENDAÇÃO FINAL

**Escolha Opção A** (correção manual seletiva):
1. Começar pela Fase 2 (mais fácil, 8 erros)
2. Depois Fase 3 (1 erro)
3. Por último Fase 1 (37 erros, mas padrão repetitivo)

**Alternativa:** Se você quer economizar tempo, posso gerar um script PowerShell que faça as substituições automaticamente (Opção B), mas recomendo revisar manualmente depois.

---

## 📊 PROGRESSO GERAL

| Categoria | Modelos Sincronizados | Código Atualizado | Status |
|-----------|----------------------|-------------------|--------|
| **MODELOS** | 60/60 ✅ | - | 100% |
| **SQL** | 1/1 ✅ | - | 100% |
| **CÓDIGO C#** | - | 0/47 ❌ | 0% |
| **TOTAL** | - | - | **47% pendente** |

---

**Próximo passo:** Escolha uma das opções de correção (A, B ou C) para eu prosseguir.
