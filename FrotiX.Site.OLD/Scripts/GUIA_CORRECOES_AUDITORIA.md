# 🛠️ GUIA PRÁTICO DE CORREÇÕES - AUDITORIA DE MODELOS

**Como corrigir as 761 discrepâncias encontradas na auditoria**

---

## 📚 ÍNDICE

1. [Corrigir Nullable Incompatível (190 casos)](#1-corrigir-nullable-incompatível)
2. [Corrigir MaxLength Incompatível (11 casos)](#2-corrigir-maxlength-incompatível)
3. [Tratar Colunas Ausentes (560 casos)](#3-tratar-colunas-ausentes)
4. [Exemplos Práticos por Modelo](#4-exemplos-práticos)
5. [Checklist Pós-Correção](#5-checklist-pós-correção)

---

## 1. CORRIGIR NULLABLE INCOMPATÍVEL

### 🔴 SEVERIDADE: CRÍTICO (190 ocorrências)

### Problema

**C# permite `null`, SQL não permite `NULL` (ou vice-versa)**

### Exemplo

```csharp
// ❌ ERRADO - Discrepância encontrada pela auditoria
public double? Litros { get; set; }  // C# nullable
// SQL: Litros float NOT NULL
```

### Solução

#### Opção 1: Ajustar C# para corresponder ao SQL (RECOMENDADO)

```csharp
// ✅ CORRETO - C# NOT NULL corresponde a SQL NOT NULL
public double Litros { get; set; }  // Remove o '?'
```

#### Opção 2: Ajustar SQL para corresponder ao C# (SE NECESSÁRIO)

```sql
-- ✅ CORRETO - SQL NULL corresponde a C# nullable
ALTER TABLE dbo.Abastecimento
ALTER COLUMN Litros float NULL;  -- Adiciona NULL
```

**⚠️ ATENÇÃO:** Opção 2 requer migration e pode ter impacto em dados existentes!

### Como Identificar no Relatório

```markdown
#### 2. **Litros**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `double? (nullable=True)`
- **SQL:** `float (NOT NULL)`
- **Correção:** Alterar C# para:
```

### Passo a Passo

1. Abrir `AUDITORIA_COMPLETA_MODELOS_VS_BANCO.md`
2. Buscar "Nullable incompatível"
3. Para cada ocorrência:
   - Abrir o modelo C# correspondente
   - Verificar a coluna no `FrotiX.sql`
   - Decidir: ajustar C# ou SQL?
   - Aplicar correção
   - Testar CRUD da entidade

---

## 2. CORRIGIR MAXLENGTH INCOMPATÍVEL

### 🟡 SEVERIDADE: ATENÇÃO (11 ocorrências)

### Problema

**`[MaxLength]` em C# não corresponde ao tamanho da coluna SQL**

### Exemplo

```csharp
// ❌ ERRADO - Discrepância encontrada pela auditoria
[MaxLength(2000)]
public string? TipoPendencia { get; set; }
// SQL: TipoPendencia varchar(50)
```

### Solução

#### Opção 1: Ajustar C# MaxLength (MAIS COMUM)

```csharp
// ✅ CORRETO - MaxLength corresponde ao SQL
[MaxLength(50)]
public string? TipoPendencia { get; set; }
```

#### Opção 2: Aumentar coluna SQL (SE NECESSÁRIO)

```sql
-- ✅ CORRETO - SQL aumentado para corresponder ao C#
ALTER TABLE dbo.AbastecimentoPendente
ALTER COLUMN TipoPendencia varchar(2000);
```

### Como Identificar no Relatório

```markdown
#### 1. **TipoPendencia**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(2000)]`
- **SQL:** `(50)`
- **Correção:** Alterar [MaxLength] para 50
```

### Casos Específicos

| Modelo | Propriedade | C# MaxLength | SQL MaxLength | Correção |
|--------|-------------|--------------|---------------|----------|
| `AbastecimentoPendente` | `TipoPendencia` | 2000 | 50 | Reduzir C# → 50 |
| `AbastecimentoPendente` | `CampoCorrecao` | 50 | 20 | Reduzir C# → 20 |

---

## 3. TRATAR COLUNAS AUSENTES

### 🔵 SEVERIDADE: INFO (560 ocorrências)

### Problema

**Propriedade C# não tem coluna correspondente no SQL**

### Causas Comuns (ESPERADO)

1. **Propriedades `[NotMapped]`** (não devem mapear)
2. **Propriedades de navegação** (EF Core)
3. **Arquivos/uploads** (`IFormFile`)
4. **Flags temporárias de UI**

### Solução

#### Caso 1: Propriedade DEVE ser [NotMapped]

```csharp
// ❌ ANTES - Sem anotação (auditoria detecta como "coluna ausente")
public IFormFile? ArquivoFoto { get; set; }

// ✅ DEPOIS - Adicionar [NotMapped] explícito
[NotMapped]
public IFormFile? ArquivoFoto { get; set; }
```

#### Caso 2: Propriedade DEVE ter coluna no banco

Se a propriedade realmente deve persistir:

1. Criar migration para adicionar coluna
2. Atualizar `FrotiX.sql`
3. Executar migration

```csharp
// Exemplo: Nova propriedade que DEVE persistir
public string? NovaPropriedade { get; set; }
```

```sql
-- Migration correspondente
ALTER TABLE dbo.MeuModelo
ADD NovaPropriedade nvarchar(100) NULL;
```

### Exemplos de [NotMapped] Corretos

```csharp
// ✅ Navegação EF Core
[ForeignKey("VeiculoId")]
[NotMapped]  // SEMPRE marcar navegação
public virtual Veiculo? Veiculo { get; set; }

// ✅ Upload de arquivo
[NotMapped]
public IFormFile? ArquivoFoto { get; set; }

// ✅ Flag temporária de UI
[NotMapped]
public bool OperacaoBemSucedida { get; set; }

// ✅ Lista de itens relacionados (não mapeados diretamente)
[NotMapped]
public List<OcorrenciaFinalizacaoDTO>? Ocorrencias { get; set; }
```

---

## 4. EXEMPLOS PRÁTICOS

### Exemplo 1: Corrigir Abastecimento.cs

**Discrepâncias Encontradas:**
- 6 propriedades nullable incompatíveis

**Antes:**

```csharp
public class Abastecimento
{
    public Guid AbastecimentoId { get; set; }

    // ❌ ERRADO - Nullable incompatível
    public double? Litros { get; set; }  // SQL: float NOT NULL
    public double? ValorUnitario { get; set; }  // SQL: float NOT NULL
    public DateTime? DataHora { get; set; }  // SQL: datetime NOT NULL
    public int? KmRodado { get; set; }  // SQL: int NOT NULL
    public int? Hodometro { get; set; }  // SQL: int NOT NULL

    public Guid? VeiculoId { get; set; }
}
```

**Depois:**

```csharp
public class Abastecimento
{
    public Guid AbastecimentoId { get; set; }

    // ✅ CORRETO - Nullable corresponde ao SQL
    public double Litros { get; set; }  // SQL: float NOT NULL
    public double ValorUnitario { get; set; }  // SQL: float NOT NULL
    public DateTime DataHora { get; set; }  // SQL: datetime NOT NULL
    public int KmRodado { get; set; }  // SQL: int NOT NULL
    public int Hodometro { get; set; }  // SQL: int NOT NULL

    public Guid? VeiculoId { get; set; }  // FK pode ser NULL
}
```

### Exemplo 2: Corrigir AbastecimentoPendente.cs

**Discrepâncias Encontradas:**
- 2 propriedades MaxLength incompatível

**Antes:**

```csharp
public class AbastecimentoPendente
{
    // ❌ ERRADO - MaxLength incompatível
    [MaxLength(2000)]
    public string? TipoPendencia { get; set; }  // SQL: varchar(50)

    [MaxLength(50)]
    public string? CampoCorrecao { get; set; }  // SQL: varchar(20)
}
```

**Depois:**

```csharp
public class AbastecimentoPendente
{
    // ✅ CORRETO - MaxLength corresponde ao SQL
    [MaxLength(50)]
    public string? TipoPendencia { get; set; }  // SQL: varchar(50)

    [MaxLength(20)]
    public string? CampoCorrecao { get; set; }  // SQL: varchar(20)
}
```

### Exemplo 3: Adicionar [NotMapped] em Viagem.cs

**Discrepâncias Encontradas:**
- 25 colunas "ausentes" (na verdade são [NotMapped])

**Antes:**

```csharp
public class Viagem
{
    // ❌ SEM ANOTAÇÃO - Auditoria detecta como "coluna ausente"
    public IFormFile? ArquivoFoto { get; set; }
    public bool CriarViagemFechada { get; set; }
    public DateTime? EditarAPartirData { get; set; }
    public bool? OperacaoBemSucedida { get; set; }

    [ForeignKey("VeiculoId")]
    public virtual Veiculo? Veiculo { get; set; }
}
```

**Depois:**

```csharp
public class Viagem
{
    // ✅ CORRETO - [NotMapped] explícito
    [NotMapped]
    public IFormFile? ArquivoFoto { get; set; }

    [NotMapped]
    public bool CriarViagemFechada { get; set; }

    [NotMapped]
    public DateTime? EditarAPartirData { get; set; }

    [NotMapped]
    public bool? OperacaoBemSucedida { get; set; }

    [ForeignKey("VeiculoId")]
    [NotMapped]  // Navegação EF Core
    public virtual Veiculo? Veiculo { get; set; }
}
```

---

## 5. CHECKLIST PÓS-CORREÇÃO

Após corrigir discrepâncias de um modelo, validar:

### ✅ Checklist de Validação

- [ ] **Build sem erros**
  - Compilar projeto
  - Verificar warnings

- [ ] **Testes de CRUD**
  - Create: Criar novo registro
  - Read: Buscar registro existente
  - Update: Atualizar registro
  - Delete: Excluir registro

- [ ] **Validação de nullable**
  - Tentar salvar com campos obrigatórios vazios
  - Verificar mensagens de erro

- [ ] **Validação de MaxLength**
  - Tentar salvar string maior que MaxLength
  - Verificar truncamento/erro

- [ ] **Re-executar auditoria**
  ```bash
  python auditoria_modelos.py
  ```
  - Verificar se discrepâncias foram resolvidas

---

## 🎯 PRIORIZAÇÃO DE CORREÇÕES

### Fase 1: CRÍTICO (Começar aqui)

1. **Abastecimento** (6 nullable issues)
2. **AlertasFrotiX** (20 nullable issues)
3. **Viagem** (revisar linha por linha)

### Fase 2: ATENÇÃO

1. **AbastecimentoPendente** (2 MaxLength issues)
2. Demais modelos com MaxLength incompatível

### Fase 3: LIMPEZA

1. Adicionar `[NotMapped]` em TODAS propriedades não persistidas
2. Melhorar documentação de código

---

## 📖 REFERÊNCIAS

- **Relatório Completo:** `AUDITORIA_COMPLETA_MODELOS_VS_BANCO.md`
- **Sumário Executivo:** `SUMARIO_EXECUTIVO_AUDITORIA.md`
- **Estrutura do Banco:** `FrotiX.sql`
- **Regras do Projeto:** `RegrasDesenvolvimentoFrotiX.md` (raiz do workspace)

---

## 💡 DICAS FINAIS

1. **Sempre consultar FrotiX.sql ANTES de codificar**
   - Ver `RegrasDesenvolvimentoFrotiX.md` seção 1

2. **Usar [NotMapped] explicitamente**
   - Facilita futuras auditorias
   - Deixa intenção clara

3. **Validar MaxLength**
   - Adicionar em TODAS strings
   - Corresponder ao banco

4. **Testar após correções**
   - CRUD completo
   - Casos de erro

5. **Re-executar auditoria**
   - Validar que problema foi resolvido
   - Evitar regressões

---

✅ **Boa sorte com as correções!**

📞 **Dúvidas?** Consulte `README_AUDITORIA.md` ou `RegrasDesenvolvimentoFrotiX.md`
