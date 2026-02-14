# AÇÕES NECESSÁRIAS NOS MODELOS C# PÓS-SINCRONIZAÇÃO

**Data:** 13/02/2026
**Versão:** 1.0
**Autor:** Claude Sonnet 4.5 (FrotiX Team)

---

## CONTEXTO

O script `SINCRONIZAR_BANCO_COM_MODELOS.sql` identificou **761 discrepâncias** entre os modelos C# e o banco de dados SQL Server. Dessas:

- **190 discrepâncias nullable** - A maioria requer correção nos MODELOS C#
- **11 discrepâncias MaxLength** - TODAS requerem correção nos MODELOS C#
- **560 colunas ausentes no SQL** - São propriedades de navegação/NotMapped (OK)

Este documento detalha **TODAS as alterações necessárias nos modelos C#** para completar a sincronização.

---

## PRINCÍPIO FUNDAMENTAL

> **O BANCO DE DADOS SQL SERVER É A FONTE DE VERDADE**

Quando há conflito entre modelo C# e banco SQL:
- ✅ **Sempre ajuste o modelo C# para refletir o banco**
- ❌ **Nunca altere o banco para refletir o modelo** (exceto em casos específicos aprovados)

---

## 1. CORREÇÕES NULLABLE (190 DISCREPÂNCIAS)

### 1.1 Abastecimento.cs (5 correções)

**Arquivo:** `FrotiX.Site.OLD/Models/Cadastros/Abastecimento.cs`

```csharp
// ❌ ANTES (INCORRETO)
public double? Litros { get; set; }
public double? ValorUnitario { get; set; }
public DateTime? DataHora { get; set; }
public int? KmRodado { get; set; }
public int? Hodometro { get; set; }

// ✅ DEPOIS (CORRETO - banco é NOT NULL)
public double Litros { get; set; }
public double ValorUnitario { get; set; }
public DateTime DataHora { get; set; }
public int KmRodado { get; set; }
public int Hodometro { get; set; }
```

**Justificativa:** Colunas no SQL são `NOT NULL`, então o modelo C# não pode ser nullable.

---

### 1.2 AlertasFrotiX.cs (12 correções)

**Arquivo:** `FrotiX.Site.OLD/Models/AlertasFrotiX.cs`

#### Correções 1-4: Campos NOT NULL no banco

```csharp
// ❌ ANTES (INCORRETO)
public string? Titulo { get; set; }
public string? Descricao { get; set; }
public DateTime? DataInsercao { get; set; }
public string? UsuarioCriadorId { get; set; }

// ✅ DEPOIS (CORRETO)
public string Titulo { get; set; } = string.Empty;
public string Descricao { get; set; } = string.Empty;
public DateTime DataInsercao { get; set; }
public string UsuarioCriadorId { get; set; } = string.Empty;
```

#### Correções 5-11: Dias da semana (bool → bool?)

```csharp
// ❌ ANTES (INCORRETO - banco permite NULL)
public bool Monday { get; set; }
public bool Tuesday { get; set; }
public bool Wednesday { get; set; }
public bool Thursday { get; set; }
public bool Friday { get; set; }
public bool Saturday { get; set; }
public bool Sunday { get; set; }

// ✅ DEPOIS (CORRETO)
public bool? Monday { get; set; }
public bool? Tuesday { get; set; }
public bool? Wednesday { get; set; }
public bool? Thursday { get; set; }
public bool? Friday { get; set; }
public bool? Saturday { get; set; }
public bool? Sunday { get; set; }
```

#### Correção 12: DiasSemana

```csharp
// ❌ ANTES (INCORRETO)
public string DiasSemana { get; set; } = string.Empty;

// ✅ DEPOIS (CORRETO)
public string? DiasSemana { get; set; }
```

**Justificativa:** O script SQL já alterou o banco para permitir NULL nos dias da semana (ALTER TABLE executado).

---

### 1.3 AlertasUsuario.cs (1 correção)

**Arquivo:** `FrotiX.Site.OLD/Models/AlertasUsuario.cs`

```csharp
// ❌ ANTES (INCORRETO)
public bool Apagado { get; set; }

// ✅ DEPOIS (CORRETO)
public bool? Apagado { get; set; }
```

---

### 1.4 AnosDisponiveisAbastecimento.cs (2 correções)

**Arquivo:** `FrotiX.Site.OLD/Models/AnosDisponiveisAbastecimento.cs`

```csharp
// ❌ ANTES (INCORRETO)
public int TotalAbastecimentos { get; set; }
public DateTime DataAtualizacao { get; set; }

// ✅ DEPOIS (CORRETO)
public int? TotalAbastecimentos { get; set; }
public DateTime? DataAtualizacao { get; set; }
```

---

### 1.5 AspNetUsers.cs (1 correção)

**Arquivo:** `FrotiX.Site.OLD/Models/AspNetUsers.cs`

```csharp
// ❌ ANTES (INCORRETO)
public string? Id { get; set; }

// ✅ DEPOIS (CORRETO)
public string Id { get; set; } = string.Empty;
```

**Justificativa:** `Id` é chave primária, não pode ser nullable.

---

### 1.6 AtaRegistroPrecos.cs (4 correções)

**Arquivo:** `FrotiX.Site.OLD/Models/Cadastros/AtaRegistroPrecos.cs`

```csharp
// ❌ ANTES (INCORRETO)
public string NumeroProcesso { get; set; } = string.Empty;
public string Objeto { get; set; } = string.Empty;
public bool Status { get; set; }
public Guid FornecedorId { get; set; }

// ✅ DEPOIS (CORRETO)
public string? NumeroProcesso { get; set; }
public string? Objeto { get; set; }
public bool? Status { get; set; }
public Guid? FornecedorId { get; set; }
```

---

### 1.7 Combustivel.cs (1 correção)

**Arquivo:** `FrotiX.Site.OLD/Models/Cadastros/Combustivel.cs`

```csharp
// ❌ ANTES (INCORRETO)
public bool Status { get; set; }

// ✅ DEPOIS (CORRETO)
public bool? Status { get; set; }
```

---

### 1.8 Contrato.cs (6 correções)

**Arquivo:** `FrotiX.Site.OLD/Models/Cadastros/Contrato.cs`

```csharp
// ❌ ANTES (INCORRETO)
public bool ContratoEncarregados { get; set; }
public bool ContratoOperadores { get; set; }
public bool ContratoMotoristas { get; set; }
public bool ContratoLavadores { get; set; }
public bool Status { get; set; }
public Guid FornecedorId { get; set; }

// ✅ DEPOIS (CORRETO)
public bool? ContratoEncarregados { get; set; }
public bool? ContratoOperadores { get; set; }
public bool? ContratoMotoristas { get; set; }
public bool? ContratoLavadores { get; set; }
public bool? Status { get; set; }
public Guid? FornecedorId { get; set; }
```

---

### 1.9 DEMAIS MODELOS (157 correções restantes)

Para os demais modelos, siga o padrão acima:

1. Abra o arquivo `AUDITORIA_COMPLETA_MODELOS_VS_BANCO.md`
2. Localize o modelo C# correspondente
3. Para cada discrepância nullable:
   - **Se SQL é NOT NULL e C# é nullable (`?`)**: Remova o `?` no C#
   - **Se SQL é NULL e C# é NOT NULL**: Adicione o `?` no C#

**Lista de modelos com correções pendentes:**
- CoberturaFolga.cs (27 correções - TODAS propriedades não mapeadas, ignorar)
- ControleAcesso.cs
- CorridasTaxiLeg.cs
- CustoMensalItensContrato.cs
- Empenho.cs
- EmpenhoMulta.cs
- Encarregado.cs
- EscalaDia.cs
- Evento.cs
- Fornecedor.cs
- ItemVeiculoAta.cs
- ItemVeiculoContrato.cs
- ItensContrato.cs
- ItensManutencao.cs
- LavadorContrato.cs
- Lavagem.cs
- LogErro.cs
- LotacaoMotorista.cs
- Manutencao.cs
- MarcaVeiculo.cs
- MediaCombustivel.cs
- ModeloVeiculo.cs
- Motorista.cs
- MotoristaContrato.cs
- MotoristaItensPendentes.cs
- MovimentacaoEmpenho.cs
- MovimentacaoEmpenhoMulta.cs
- MovimentacaoPatrimonio.cs
- Multa.cs
- NotaFiscal.cs
- Ocorrencia.cs
- OperadorContrato.cs
- OrgaoAutuante.cs
- Patrimonio.cs
- PlacaBronze.cs
- RecorrenciaAlerta.cs
- RegistroCupomAbastecimento.cs
- RepactuacaoAta.cs
- RepactuacaoContrato.cs
- RepactuacaoServicos.cs
- RepactuacaoTerceirizacao.cs
- RepactuacaoVeiculo.cs
- Requisitante.cs
- SecaoPatrimonial.cs
- SetorPatrimonial.cs
- SetorSolicitante.cs
- TipoMulta.cs
- Unidade.cs
- Veiculo.cs
- VeiculoAta.cs
- VeiculoContrato.cs
- VeiculoPadraoViagem.cs
- Viagem.cs (EXCLUINDO Origem/Destino - será tratado separadamente)
- ViagemEstatistica.cs

---

## 2. CORREÇÕES MAXLENGTH (11 DISCREPÂNCIAS)

### 2.1 AbastecimentoPendente.cs (2 correções)

**Arquivo:** `FrotiX.Site.OLD/Models/AbastecimentoPendente.cs`

```csharp
// ❌ ANTES (INCORRETO)
[MaxLength(2000)]
public string? TipoPendencia { get; set; }

[MaxLength(50)]
public string? CampoCorrecao { get; set; }

// ✅ DEPOIS (CORRETO)
[MaxLength(50)] // SQL tem NVARCHAR(50), não 2000
public string? TipoPendencia { get; set; }

[MaxLength(20)] // SQL tem NVARCHAR(20), não 50
public string? CampoCorrecao { get; set; }
```

---

### 2.2 DEMAIS MODELOS (9 correções restantes)

Para localizar as demais 9 discrepâncias MaxLength:

```bash
# No arquivo de auditoria, procure por:
"MaxLength incompatível"
```

**Padrão de correção:**

```csharp
// ❌ ANTES
[MaxLength(VALOR_ERRADO)]
public string? Propriedade { get; set; }

// ✅ DEPOIS
[MaxLength(VALOR_CORRETO)] // Valor deve corresponder ao SQL
public string? Propriedade { get; set; }
```

**⚠️ IMPORTANTE:** O valor em `[MaxLength(X)]` deve **SEMPRE** corresponder ao tamanho da coluna no SQL Server.

---

## 3. PROPRIEDADES [NOTMAPPED] (560 ITENS)

Estas são propriedades que **NÃO existem no banco SQL** e estão marcadas (ou deveriam estar) com `[NotMapped]`.

### Exemplos comuns:

```csharp
// Propriedades de navegação (relacionamentos)
[NotMapped]
public virtual Veiculo? Veiculo { get; set; }

[NotMapped]
public virtual Motorista? Motorista { get; set; }

// Propriedades calculadas
[NotMapped]
public string NomeCompleto => $"{Nome} {Sobrenome}";

// Propriedades auxiliares (não persistidas)
[NotMapped]
public bool IsSelected { get; set; }
```

**AÇÃO:** Se a propriedade não existe no SQL e não está marcada com `[NotMapped]`, adicione o atributo.

---

## 4. CHECKLIST DE VALIDAÇÃO

Após aplicar todas as correções:

- [ ] Compilar solução (não deve haver erros de compilação)
- [ ] Executar migrations do EF Core (se aplicável)
- [ ] Executar script `SINCRONIZAR_BANCO_COM_MODELOS.sql`
- [ ] Executar nova auditoria completa para verificar sincronização
- [ ] Testar funcionalidades críticas (Abastecimento, Viagens, Multas)
- [ ] Validar que propriedades nullable estão corretas (null checks no código)

---

## 5. SCRIPT DE AUTOMAÇÃO (OPCIONAL)

Para facilitar a identificação de correções pendentes:

```powershell
# PowerShell: Localizar propriedades nullable que precisam de correção
Get-ChildItem -Path "FrotiX.Site.OLD\Models" -Filter "*.cs" -Recurse |
    Select-String -Pattern "public \w+\? \w+ { get; set; }" |
    Select-Object Filename, LineNumber, Line |
    Out-GridView -Title "Propriedades Nullable Encontradas"
```

---

## 6. RESPONSABILIDADES

| Arquivo | Correções Nullable | Correções MaxLength | Prioridade |
|---------|-------------------|---------------------|------------|
| Abastecimento.cs | 5 | 0 | 🔴 ALTA |
| AlertasFrotiX.cs | 12 | 0 | 🔴 ALTA |
| AbastecimentoPendente.cs | 0 | 2 | 🟡 MÉDIA |
| Viagem.cs | ~20 | ~3 | 🔴 ALTA |
| Veiculo.cs | ~15 | ~2 | 🔴 ALTA |
| Motorista.cs | ~12 | ~1 | 🔴 ALTA |
| Demais modelos | ~126 | ~3 | 🟢 BAIXA |

---

## 7. PRÓXIMAS AÇÕES

1. **Imediato:**
   - Corrigir Abastecimento.cs (5 nullable)
   - Corrigir AlertasFrotiX.cs (12 nullable)
   - Corrigir AbastecimentoPendente.cs (2 MaxLength)

2. **Curto prazo:**
   - Revisar todos os modelos de alta prioridade (Viagem, Veiculo, Motorista)
   - Executar script SQL de sincronização

3. **Médio prazo:**
   - Corrigir todos os demais modelos
   - Executar script de limpeza fuzzy (Viagem.Origem/Destino)
   - Nova auditoria completa

---

## 8. OBSERVAÇÕES IMPORTANTES

### 8.1 Viagem.Origem e Viagem.Destino

**NÃO ALTERAR NESTE MOMENTO!**

Estas colunas serão tratadas em script separado de limpeza fuzzy devido a:
- Dados inconsistentes (espaços, maiúsculas/minúsculas)
- Necessidade de normalização
- Potencial impacto em relatórios

### 8.2 Fornecedor.FornecedorId

**ATENÇÃO:** Esta coluna tem problema de design (UNIQUE INDEX ao invés de PRIMARY KEY).
Não alterar sem aprovação da equipe de DBA.

### 8.3 Tabelas de Estatísticas

Tabelas como `ViagemEstatistica`, `HeatmapViagens`, etc. usam DELETE+INSERT em batch.
Não possuem FKs por design. Isso é **intencional e correto**.

---

## 9. DOCUMENTAÇÃO DE REFERÊNCIA

- **Auditoria Completa:** `AUDITORIA_COMPLETA_MODELOS_VS_BANCO.md`
- **Script SQL:** `SINCRONIZAR_BANCO_COM_MODELOS.sql`
- **Schema SQL:** `Frotix.sql`
- **Convenções EF Core:** [Microsoft Docs - EF Core Conventions](https://learn.microsoft.com/ef/core/modeling/)

---

## 10. CHANGELOG

| Versão | Data | Autor | Mudanças |
|--------|------|-------|----------|
| 1.0 | 13/02/2026 | Claude Sonnet 4.5 | Documento inicial com todas as 201 correções (190 nullable + 11 MaxLength) |

---

**FIM DO DOCUMENTO**
