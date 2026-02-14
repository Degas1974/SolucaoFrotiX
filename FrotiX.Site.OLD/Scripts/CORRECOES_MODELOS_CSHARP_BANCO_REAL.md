# CORREÇÕES MODELOS C# - SINCRONIZAÇÃO COM BANCO REAL
**Data:** 13/02/2026
**Objetivo:** Alinhar modelos C# com schema do banco SQL Server Frotix

---

## 🔴 PRIORIDADE CRÍTICA - TIPOS INCOMPATÍVEIS

### 1. AlertasFrotiX.cs - 3 correções URGENTES

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\AlertasFrotiX.cs`

```csharp
// ❌ ANTES (ERRADO - CAUSA ERRO)
public TipoAlerta TipoAlerta { get; set; }
public PrioridadeAlerta Prioridade { get; set; }
public TipoExibicaoAlerta TipoExibicao { get; set; }

// ✅ DEPOIS (CORRETO)
public int TipoAlerta { get; set; }
public int Prioridade { get; set; }
public int TipoExibicao { get; set; }

// NOTA: Se quiser manter enums, use conversão explícita:
// [NotMapped]
// public TipoAlerta TipoAlertaEnum => (TipoAlerta)TipoAlerta;
```

---

### 2. CorridasTaxiLeg.cs - 1 correção URGENTE

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\Cadastros\CorridasTaxiLeg.cs`

```csharp
// ❌ ANTES (ERRADO)
public string? QRU { get; set; }

// ✅ DEPOIS (CORRETO)
public int? QRU { get; set; }
```

---

### 3. Viagem.cs - 2 correções URGENTES

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\Cadastros\Viagem.cs`

```csharp
// ❌ ANTES (ERRADO)
public string? DataFinalizacao { get; set; }
public List<DateTime>? DatasSelecionadas { get; set; }

// ✅ DEPOIS (CORRETO)
public DateTime? DataFinalizacao { get; set; }
public string DatasSelecionadas { get; set; }  // JSON serializado

// NOTA: Para deserializar DatasSelecionadas:
// [NotMapped]
// public List<DateTime>? DatasSelecionadasList
// {
//     get => string.IsNullOrEmpty(DatasSelecionadas)
//            ? null
//            : JsonSerializer.Deserialize<List<DateTime>>(DatasSelecionadas);
//     set => DatasSelecionadas = value == null
//            ? null
//            : JsonSerializer.Serialize(value);
// }
```

---

## 🟡 PRIORIDADE ALTA - COLUNAS NÃO MAPEADAS

### 4. Abastecimento.cs - Adicionar 5 colunas

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\Cadastros\Abastecimento.cs`

```csharp
// ADICIONAR ao modelo:
public int? KmRodadoNormalizado { get; set; }
public double? LitrosNormalizado { get; set; }
public decimal? ConsumoCalculado { get; set; }
public decimal? ConsumoNormalizado { get; set; }
public bool? EhOutlier { get; set; }
```

**Explicação:**
- Colunas usadas para normalização e detecção de outliers em abastecimentos
- Populadas por stored procedures ou triggers

---

### 5. VeiculoPadraoViagem.cs - RECONSTRUIR MODELO COMPLETO

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\VeiculoPadraoViagem.cs`

**⚠️ ATENÇÃO:** Este modelo está **COMPLETAMENTE DESALINHADO** com o banco!

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Site.OLD.Models
{
    [Table("VeiculoPadraoViagem")]
    public class VeiculoPadraoViagem
    {
        [Key]
        public int VeiculoPadraoViagemId { get; set; }

        [Required]
        public Guid VeiculoId { get; set; }

        // Estatísticas de Duração
        public decimal? AvgDuracaoMinutos { get; set; }
        public decimal? DesvioPadraoDuracaoMinutos { get; set; }
        public int? MinDuracaoMinutos { get; set; }
        public int? MaxDuracaoNormalMinutos { get; set; }
        public int? MedianaDuracaoMinutos { get; set; }
        public decimal? MedianaMinutos { get; set; }
        public int? Percentil95Duracao { get; set; }
        public int? Percentil99Duracao { get; set; }

        // Estatísticas de Km
        public decimal? AvgKmPorViagem { get; set; }
        public decimal? DesvioPadraoKm { get; set; }
        public decimal? MaxKmNormalPorViagem { get; set; }
        public decimal? MedianaKm { get; set; }
        public decimal? Q1Km { get; set; }
        public decimal? Q3Km { get; set; }
        public decimal? IQRKm { get; set; }
        public decimal? LimiteInferiorKm { get; set; }
        public decimal? LimiteSuperiorKm { get; set; }

        // Estatísticas Gerais
        public decimal? AvgKmPorDia { get; set; }
        public decimal? MediaKmEntreAbastecimentos { get; set; }
        public decimal? MediaDiasEntreAbastecimentos { get; set; }

        // Contadores
        public int? TotalAbastecimentosAnalisados { get; set; }
        public int? TotalViagensAnalisadas { get; set; }
        public int? TotalViagensRealizadas { get; set; }

        // Metadados
        [StringLength(50)]
        public string? TipoUso { get; set; }

        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        // Navegação
        [ForeignKey("VeiculoId")]
        public virtual Veiculo? Veiculo { get; set; }
    }
}
```

---

### 6. Viagem.cs - Adicionar 11 colunas

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\Cadastros\Viagem.cs`

```csharp
// ADICIONAR ao modelo:

// Ocorrências
public string ResumoOcorrencia { get; set; }
public string DescricaoOcorrencia { get; set; }
public string StatusOcorrencia { get; set; }
public string DescricaoSolucaoOcorrencia { get; set; }
public Guid? ItemManutencaoId { get; set; }

// Agendamento
public string AgendamentoTMP { get; set; }

// Vistoria
public string DanoAvaria { get; set; }
public string DanoAvariaFinal { get; set; }

// Mídias (Base64)
public byte[] FotosBase64 { get; set; }
public byte[] VideosBase64 { get; set; }
public byte[] FotosFinaisBase64 { get; set; }
public byte[] VideosFinaisBase64 { get; set; }

// ID Sequencial Adicional (além do ViagemId GUID)
public int Id { get; set; }
```

---

### 7. AlertasFrotiX.cs - Adicionar 3 colunas de recorrência

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\AlertasFrotiX.cs`

```csharp
// ADICIONAR ao modelo:
[StringLength(1)]
public string Recorrente { get; set; }  // 'S' ou 'N'

[StringLength(1)]
public string Intervalo { get; set; }   // 'D'=Diário, 'S'=Semanal, 'M'=Mensal

public DateTime? DataFinalRecorrencia { get; set; }
```

---

### 8. ViagemEstatistica.cs - Adicionar 4 colunas

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\ViagemEstatistica.cs`

```csharp
// ADICIONAR ao modelo:
public int? KmTotal { get; set; }
public decimal? KmMedio { get; set; }
public int? MinutosTotal { get; set; }
public int? MinutosMedio { get; set; }
```

---

### 9. CorridasTaxiLeg.cs - Adicionar 1 coluna

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\Cadastros\CorridasTaxiLeg.cs`

```csharp
// ADICIONAR ao modelo:
public double? Valor { get; set; }
```

---

### 10. Lavagem.cs - Adicionar 1 coluna

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\Cadastros\Lavagem.cs`

```csharp
// ADICIONAR ao modelo:
public DateTime? Horario { get; set; }

// Se já existe propriedade "HorarioLavagem", considere renomear:
// [Column("Horario")]
// public DateTime? HorarioLavagem { get; set; }
```

---

### 11. Motorista.cs - Adicionar 1 coluna

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\Cadastros\Motorista.cs`

```csharp
// ADICIONAR ao modelo:
public Guid? CondutorId { get; set; }

// Navegação (opcional):
[ForeignKey("CondutorId")]
public virtual CondutorApoio? Condutor { get; set; }
```

---

### 12. AtaRegistroPrecos.cs - Adicionar 2 colunas de auditoria

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\Cadastros\AtaRegistroPrecos.cs`

```csharp
// ADICIONAR ao modelo:
[StringLength(100)]
public string? UsuarioIdAlteracao { get; set; }

public DateTime? DataAlteracao { get; set; }
```

---

### 13. Contrato.cs - Adicionar 2 colunas de auditoria

**Localização:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\Cadastros\Contrato.cs`

```csharp
// ADICIONAR ao modelo:
[StringLength(100)]
public string? UsuarioIdAlteracao { get; set; }

public DateTime? DataAlteracao { get; set; }
```

---

## 🟢 PRIORIDADE MÉDIA - PROPRIEDADES EXTRAS (NotMapped)

### Padrão para Propriedades Auxiliares

**Todas as propriedades abaixo devem receber `[NotMapped]`:**

#### Tipo 1: Listas de SelectListItem (56 ocorrências)

```csharp
[NotMapped]
public IEnumerable<SelectListItem>? VeiculoList { get; set; }

[NotMapped]
public IEnumerable<SelectListItem>? CombustivelList { get; set; }

[NotMapped]
public IEnumerable<SelectListItem>? MotoristaList { get; set; }

// ... etc (aplicar em TODAS as propriedades "*List")
```

---

#### Tipo 2: Propriedades de Upload de Arquivos (7 ocorrências)

```csharp
[NotMapped]
public IFormFile? ArquivoFoto { get; set; }
```

Aplica-se a:
- Encarregado.cs
- Lavador.cs
- Motorista.cs
- Operador.cs
- Viagem.cs

---

#### Tipo 3: Nomes de Usuários (10 ocorrências)

```csharp
[NotMapped]
public string? NomeUsuarioAlteracao { get; set; }

[NotMapped]
public string? NomeUsuarioCriacao { get; set; }

[NotMapped]
public string? NomeUsuarioFinalizacao { get; set; }

[NotMapped]
public string? NomeUsuarioCancelamento { get; set; }

[NotMapped]
public string? NomeUsuarioAgendamento { get; set; }
```

Aplica-se a:
- Encarregado.cs
- Lavador.cs
- Motorista.cs
- MovimentacaoPatrimonio.cs
- Operador.cs
- SetorSolicitante.cs
- Veiculo.cs
- Viagem.cs

---

#### Tipo 4: Propriedades Circulares (Auto-referência) (20 ocorrências)

```csharp
// REMOVER ou marcar como [NotMapped]:
[NotMapped]
public Abastecimento? Abastecimento { get; set; }

[NotMapped]
public AtaRegistroPrecos? AtaRegistroPrecos { get; set; }

// ... etc
```

**RECOMENDAÇÃO:** **REMOVER** essas propriedades - elas causam loops infinitos em serialização JSON.

Aplica-se a:
- Abastecimento.cs
- AtaRegistroPrecos.cs
- Contrato.cs
- Empenho.cs
- EmpenhoMulta.cs
- Encarregado.cs
- EncarregadoContrato.cs
- Evento.cs
- Lavador.cs
- LavadorContrato.cs
- Manutencao.cs
- ModeloVeiculo.cs
- Motorista.cs
- MotoristaContrato.cs
- MovimentacaoEmpenho.cs
- MovimentacaoEmpenhoMulta.cs
- MovimentacaoPatrimonio.cs
- Multa.cs
- NotaFiscal.cs
- Operador.cs
- OperadorContrato.cs
- Patrimonio.cs
- PlacaBronze.cs
- Recurso.cs
- RegistroCupomAbastecimento.cs
- Requisitante.cs
- SetorSolicitante.cs
- Veiculo.cs
- VeiculoAta.cs
- VeiculoContrato.cs
- Viagem.cs

---

#### Tipo 5: Propriedades de UI/Auxiliares (66 ocorrências)

Adicionar `[NotMapped]` em:

**Viagem.cs:**
```csharp
[NotMapped]
public bool? OperacaoBemSucedida { get; set; }

[NotMapped]
public bool CriarViagemFechada { get; set; }

[NotMapped]
public string? Hora { get; set; }

[NotMapped]
public DateTime? HoraInicial { get; set; }

[NotMapped]
public bool? CartaoAbastecimentoEntregue { get; set; }

[NotMapped]
public List<OcorrenciaFinalizacaoDTO>? Ocorrencias { get; set; }

[NotMapped]
public bool? CartaoAbastecimentoDevolvido { get; set; }

[NotMapped]
public bool? editarTodosRecorrentes { get; set; }

[NotMapped]
public string? HoraFinalizacao { get; set; }

[NotMapped]
public string? Data { get; set; }

[NotMapped]
public string? Resumo { get; set; }

[NotMapped]
public DateTime? EditarAPartirData { get; set; }

[NotMapped]
public bool? DocumentoEntregue { get; set; }

[NotMapped]
public bool? DocumentoDevolvido { get; set; }

[NotMapped]
public List<DateTime>? DataEspecifica { get; set; }
```

**Outros modelos com propriedades auxiliares:**
- Combustivel.cs: `Ano`, `Mes`, `PrecoMedio`, `NotaFiscalId`
- Contrato.cs: `QtdOperadores`, `QtdMotoristas`, `QtdLavadores`, `QtdEncarregados`, etc
- MovimentacaoPatrimonio.cs: `PatrimonioNome`, `SecaoDestinoNome`, `SetorOrigemNome`, etc
- NotaFiscal.cs: `CustoMensalMotorista`, `MediaGasolina`, `MediaDiesel`, etc
- PlacaBronze.cs: `VeiculoId`
- Recurso.cs: `HasChild`
- VeiculoPadraoViagem.cs: `MediaKmPorViagem`, `MediaKmPorDia`, `TotalViagens`, `MediaDuracaoMinutos`

---

## ⚪ PRIORIDADE BAIXA - CRIAR MODELOS PARA TABELAS ÓRFÃS

### 14. AlertasUsuario (IMPORTANTE)

**Criar:** `C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models\AlertasUsuario.cs`

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Site.OLD.Models
{
    [Table("AlertasUsuario")]
    public class AlertasUsuario
    {
        [Key]
        public Guid AlertasUsuarioId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid AlertasFrotiXId { get; set; }

        [Required]
        [StringLength(450)]
        public string UsuarioId { get; set; }

        public bool Lido { get; set; } = false;
        public DateTime? DataLeitura { get; set; }

        public bool Notificado { get; set; } = false;
        public DateTime? DataNotificacao { get; set; }

        public bool? Apagado { get; set; } = false;
        public DateTime? DataApagado { get; set; }

        // Navegação
        [ForeignKey("AlertasFrotiXId")]
        public virtual AlertasFrotiX? Alerta { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual AspNetUsers? Usuario { get; set; }
    }
}
```

---

### 15-18. Repactuacao* (4 tabelas)

**NOTA:** Já existem modelos na pasta `Models/` mas não estão sendo detectados.
Verificar se há problema de namespace ou duplicação.

---

### 19-23. WhatsApp* (5 tabelas)

**Criar modelos para:**
- WhatsAppContatos
- WhatsAppFilaMensagens
- WhatsAppInstancias
- WhatsAppMensagens
- WhatsAppWebhookLogs

**Sugestão:** Criar pasta `Models/WhatsApp/` para organizar.

---

## SCRIPT DE VALIDAÇÃO

Após aplicar todas as correções, executar:

```bash
# 1. Recompilar
dotnet build

# 2. Re-executar análise
powershell -ExecutionPolicy Bypass -File "Scripts\Analisa-Schema.ps1"

# 3. Verificar resultados:
# - CRITICO deve ser 0
# - ALTO deve ser < 5
# - MEDIO deve ser < 20

# 4. Gerar migration para validar
dotnet ef migrations add ValidacaoSincronizacaoBanco

# 5. Revisar migration gerada (NÃO aplicar ainda!)
```

---

## ORDEM RECOMENDADA DE APLICAÇÃO

1. **DIA 1:** Correções CRÍTICAS (itens 1-3) ⚡
2. **DIA 2:** VeiculoPadraoViagem (item 5) ⚡
3. **DIA 3:** Viagem (item 6) ⚡
4. **DIA 4:** Abastecimento, AlertasFrotiX, ViagemEstatistica (itens 4, 7, 8) 🟡
5. **DIA 5:** Correções menores (itens 9-13) 🟡
6. **SEMANA 2:** Adicionar [NotMapped] (163 propriedades) 🟢
7. **SEMANA 3:** Criar modelos órfãos (itens 14-23) ⚪

---

**Total de Alterações:** ~400 linhas de código
**Tempo Estimado:** 2-3 semanas (com testes)
**Risco:** MÉDIO (requer testes extensivos após cada etapa)

---

**Gerado automaticamente em:** 13/02/2026
**Baseado em:** Banco SQL Server Frotix (localhost)
