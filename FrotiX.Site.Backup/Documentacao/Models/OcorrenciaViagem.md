# Documentação: OcorrenciaViagem.cs

**📅 Última Atualização:** 08/01/2026  
**📋 Versão:** 2.0 (Padrão FrotiX Simplificado)

---

## 📋 Índice

1. [Objetivos](#objetivos)
2. [Arquivos Envolvidos](#arquivos-envolvidos)
3. [Estrutura do Model](#estrutura-do-model)
4. [Mapeamento Model ↔ Banco de Dados](#mapeamento-model--banco-de-dados)
5. [Quem Chama e Por Quê](#quem-chama-e-por-quê)
6. [Problema → Solução → Código](#problema--solução--código)
7. [Fluxo de Funcionamento](#fluxo-de-funcionamento)
8. [Troubleshooting](#troubleshooting)

---

## 🎯 Objetivos

O Model `OcorrenciaViagem` representa problemas, incidentes ou observações registradas durante uma viagem, permitindo rastreamento de ocorrências, upload de imagens, controle de status (Aberta/Baixada) e vinculação com manutenções.

**Principais objetivos:**

✅ Registrar ocorrências durante viagens (problemas, incidentes, observações)  
✅ Armazenar imagens das ocorrências (caminho do arquivo)  
✅ Controlar status da ocorrência (Aberta/Baixada)  
✅ Rastrear quem criou e quem baixou a ocorrência  
✅ Vincular ocorrência com item de manutenção (quando resolvida)  
✅ Armazenar observações e solução da ocorrência

---

## 📁 Arquivos Envolvidos

### Arquivo Principal
- **`Models/OcorrenciaViagem.cs`** - Model Entity Framework Core

### Arquivos que Utilizam
- **`Controllers/OcorrenciaViagemController.cs`** - Endpoints CRUD e gestão
- **`Pages/Ocorrencia/Index.cshtml`** - Página de gestão de ocorrências
- **`Pages/Viagens/Index.cshtml`** - Modal de ocorrências na listagem de viagens
- **`Repository/OcorrenciaViagemRepository.cs`** - Acesso a dados
- **`Models/ViewOcorrenciasViagem.cs`** - View com JOINs para listagem
- **`Models/ViewOcorrenciasAbertasVeiculo.cs`** - View para ocorrências em aberto

---

## 🏗️ Estrutura do Model

```csharp
[Table("OcorrenciaViagem")]
public class OcorrenciaViagem
{
    // ✅ Chave primária
    [Key]
    public Guid OcorrenciaViagemId { get; set; }

    // ✅ Relacionamentos obrigatórios
    [Required]
    public Guid ViagemId { get; set; }

    [Required]
    public Guid VeiculoId { get; set; }

    public Guid? MotoristaId { get; set; } // Opcional

    // ✅ Dados da ocorrência
    [StringLength(200)]
    public string Resumo { get; set; } = "";

    public string Descricao { get; set; } = "";
    public string ImagemOcorrencia { get; set; } = ""; // Caminho do arquivo

    // ✅ Controle de status
    [StringLength(20)]
    public string Status { get; set; } = "Aberta"; // "Aberta" ou "Baixada"

    /// <summary>
    /// Status da ocorrência: NULL ou true = Aberta, false = Baixada
    /// </summary>
    public bool? StatusOcorrencia { get; set; }

    // ✅ Controle de datas
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime? DataBaixa { get; set; }

    // ✅ Rastreamento de usuários
    [StringLength(100)]
    public string UsuarioCriacao { get; set; } = "";

    [StringLength(100)]
    public string UsuarioBaixa { get; set; } = "";

    // ✅ Vinculação com manutenção
    public Guid? ItemManutencaoId { get; set; }

    // ✅ Observações e solução
    [StringLength(500)]
    public string Observacoes { get; set; } = "";

    [StringLength(500)]
    public string Solucao { get; set; } = "";

    // ✅ Relacionamentos virtuais (comentados - não usados)
    //[ForeignKey("ViagemId")]
    //public virtual Viagem? Viagem { get; set; }
    //[ForeignKey("VeiculoId")]
    //public virtual Veiculo? Veiculo { get; set; }
    //[ForeignKey("MotoristaId")]
    //public virtual Motorista? Motorista { get; set; }
}
```

---

## 🗄️ Mapeamento Model ↔ Banco de Dados

### Estrutura SQL da Tabela

```sql
CREATE TABLE [dbo].[OcorrenciaViagem] (
    [OcorrenciaViagemId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    
    -- Relacionamentos
    [ViagemId] UNIQUEIDENTIFIER NOT NULL,
    [VeiculoId] UNIQUEIDENTIFIER NOT NULL,
    [MotoristaId] UNIQUEIDENTIFIER NULL,
    
    -- Dados da ocorrência
    [Resumo] NVARCHAR(200) NOT NULL DEFAULT '',
    [Descricao] NVARCHAR(MAX) NOT NULL DEFAULT '',
    [ImagemOcorrencia] NVARCHAR(MAX) NOT NULL DEFAULT '',
    
    -- Controle de status
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'Aberta',
    [StatusOcorrencia] BIT NULL, -- NULL ou true = Aberta, false = Baixada
    
    -- Controle de datas
    [DataCriacao] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [DataBaixa] DATETIME2 NULL,
    
    -- Rastreamento
    [UsuarioCriacao] NVARCHAR(100) NOT NULL DEFAULT '',
    [UsuarioBaixa] NVARCHAR(100) NOT NULL DEFAULT '',
    
    -- Vinculação
    [ItemManutencaoId] UNIQUEIDENTIFIER NULL,
    
    -- Observações
    [Observacoes] NVARCHAR(500) NOT NULL DEFAULT '',
    [Solucao] NVARCHAR(500) NOT NULL DEFAULT '',
    
    -- Foreign Keys (se necessário)
    CONSTRAINT [FK_OcorrenciaViagem_Viagem] 
        FOREIGN KEY ([ViagemId]) REFERENCES [Viagem]([ViagemId]),
    CONSTRAINT [FK_OcorrenciaViagem_Veiculo] 
        FOREIGN KEY ([VeiculoId]) REFERENCES [Veiculo]([VeiculoId]),
    CONSTRAINT [FK_OcorrenciaViagem_Motorista] 
        FOREIGN KEY ([MotoristaId]) REFERENCES [Motorista]([MotoristaId]),
    CONSTRAINT [FK_OcorrenciaViagem_ItemManutencao] 
        FOREIGN KEY ([ItemManutencaoId]) REFERENCES [ItensManutencao]([ItensManutencaoId])
);

-- Índices
CREATE INDEX [IX_OcorrenciaViagem_ViagemId] ON [OcorrenciaViagem]([ViagemId]);
CREATE INDEX [IX_OcorrenciaViagem_VeiculoId] ON [OcorrenciaViagem]([VeiculoId]);
CREATE INDEX [IX_OcorrenciaViagem_StatusOcorrencia] ON [OcorrenciaViagem]([StatusOcorrencia]);
CREATE INDEX [IX_OcorrenciaViagem_DataCriacao] ON [OcorrenciaViagem]([DataCriacao]);
```

### Tabela Comparativa

| Campo Model | Tipo Model | Campo SQL | Tipo SQL | Nullable | Observações |
|-------------|------------|-----------|----------|----------|-------------|
| `OcorrenciaViagemId` | `Guid` | `OcorrenciaViagemId` | `UNIQUEIDENTIFIER` | ❌ | Chave primária |
| `ViagemId` | `Guid` | `ViagemId` | `UNIQUEIDENTIFIER` | ❌ | FK para Viagem |
| `VeiculoId` | `Guid` | `VeiculoId` | `UNIQUEIDENTIFIER` | ❌ | FK para Veiculo |
| `MotoristaId` | `Guid?` | `MotoristaId` | `UNIQUEIDENTIFIER` | ✅ | FK para Motorista |
| `Resumo` | `string` | `Resumo` | `NVARCHAR(200)` | ❌ | Resumo da ocorrência |
| `Descricao` | `string` | `Descricao` | `NVARCHAR(MAX)` | ❌ | Descrição completa |
| `ImagemOcorrencia` | `string` | `ImagemOcorrencia` | `NVARCHAR(MAX)` | ❌ | Caminho da imagem |
| `Status` | `string` | `Status` | `NVARCHAR(20)` | ❌ | "Aberta" ou "Baixada" |
| `StatusOcorrencia` | `bool?` | `StatusOcorrencia` | `BIT` | ✅ | NULL/true=Aberta, false=Baixada |
| `DataCriacao` | `DateTime` | `DataCriacao` | `DATETIME2` | ❌ | Data de criação |
| `DataBaixa` | `DateTime?` | `DataBaixa` | `DATETIME2` | ✅ | Data de baixa |
| `UsuarioCriacao` | `string` | `UsuarioCriacao` | `NVARCHAR(100)` | ❌ | Usuário que criou |
| `UsuarioBaixa` | `string` | `UsuarioBaixa` | `NVARCHAR(100)` | ❌ | Usuário que baixou |
| `ItemManutencaoId` | `Guid?` | `ItemManutencaoId` | `UNIQUEIDENTIFIER` | ✅ | FK para ItensManutencao |
| `Observacoes` | `string` | `Observacoes` | `NVARCHAR(500)` | ❌ | Observações adicionais |
| `Solucao` | `string` | `Solucao` | `NVARCHAR(500)` | ❌ | Solução aplicada |

**Triggers:** Nenhum trigger associado a esta tabela.

---

## 🔗 Quem Chama e Por Quê

### 1. **OcorrenciaViagemController.cs** → Criar Ocorrência

**Quando:** Usuário registra ocorrência durante viagem  
**Por quê:** Salvar problema/incidente para rastreamento

```csharp
[HttpPost("Criar")]
public async Task<IActionResult> Criar([FromBody] OcorrenciaViagem ocorrencia)
{
    ocorrencia.OcorrenciaViagemId = Guid.NewGuid();
    ocorrencia.DataCriacao = DateTime.Now;
    ocorrencia.Status = "Aberta";
    ocorrencia.StatusOcorrencia = true;
    ocorrencia.UsuarioCriacao = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
    _unitOfWork.OcorrenciaViagem.Add(ocorrencia);
    _unitOfWork.Save();
    
    return Json(new { success = true });
}
```

### 2. **OcorrenciaViagemController.cs** → Baixar Ocorrência

**Quando:** Usuário resolve ocorrência e marca como baixada  
**Por quê:** Fechar ocorrência e registrar solução

```csharp
[HttpPost("Baixar/{id}")]
public IActionResult Baixar(Guid id, [FromBody] OcorrenciaViagem ocorrencia)
{
    var objFromDb = _unitOfWork.OcorrenciaViagem
        .GetFirstOrDefault(o => o.OcorrenciaViagemId == id);
    
    if (objFromDb == null)
        return Json(new { success = false, message = "Ocorrência não encontrada" });
    
    // ✅ Marca como baixada
    objFromDb.Status = "Baixada";
    objFromDb.StatusOcorrencia = false;
    objFromDb.DataBaixa = DateTime.Now;
    objFromDb.UsuarioBaixa = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    objFromDb.Solucao = ocorrencia.Solucao;
    objFromDb.Observacoes = ocorrencia.Observacoes;
    objFromDb.ItemManutencaoId = ocorrencia.ItemManutencaoId;
    
    _unitOfWork.OcorrenciaViagem.Update(objFromDb);
    _unitOfWork.Save();
    
    return Json(new { success = true });
}
```

---

## 🛠️ Problema → Solução → Código

### Problema: Upload de Imagem da Ocorrência

**Problema:** Ocorrências precisam ter imagens anexadas, mas o Model armazena apenas caminho (string), não o arquivo em si.

**Solução:** Controller recebe `IFormFile`, salva arquivo no servidor, e armazena caminho relativo no campo `ImagemOcorrencia`.

**Código:**

```csharp
[HttpPost("CriarComImagem")]
public async Task<IActionResult> CriarComImagem(
    [FromForm] OcorrenciaViagem ocorrencia,
    [FromForm] IFormFile imagem)
{
    if (imagem != null && imagem.Length > 0)
    {
        // ✅ Gera nome único para arquivo
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagem.FileName)}";
        var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", "ocorrencias");
        
        // ✅ Cria diretório se não existir
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);
        
        var filePath = Path.Combine(uploadPath, fileName);
        
        // ✅ Salva arquivo
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imagem.CopyToAsync(stream);
        }
        
        // ✅ Armazena caminho relativo
        ocorrencia.ImagemOcorrencia = $"/Uploads/ocorrencias/{fileName}";
    }
    
    ocorrencia.OcorrenciaViagemId = Guid.NewGuid();
    ocorrencia.DataCriacao = DateTime.Now;
    
    _unitOfWork.OcorrenciaViagem.Add(ocorrencia);
    _unitOfWork.Save();
    
    return Json(new { success = true });
}
```

### Problema: Duplo Controle de Status

**Problema:** Model tem dois campos para status: `Status` (string) e `StatusOcorrencia` (bool?), o que pode causar inconsistências.

**Solução:** Manter ambos sincronizados: `StatusOcorrencia = null ou true` → `Status = "Aberta"`, `StatusOcorrencia = false` → `Status = "Baixada"`.

**Código:**

```csharp
// ✅ Método helper para sincronizar status
private void SincronizarStatus(OcorrenciaViagem ocorrencia)
{
    if (ocorrencia.StatusOcorrencia == null || ocorrencia.StatusOcorrencia == true)
    {
        ocorrencia.Status = "Aberta";
        ocorrencia.StatusOcorrencia = true;
    }
    else
    {
        ocorrencia.Status = "Baixada";
        ocorrencia.StatusOcorrencia = false;
        if (!ocorrencia.DataBaixa.HasValue)
            ocorrencia.DataBaixa = DateTime.Now;
    }
}
```

---

## 🔄 Fluxo de Funcionamento

### Fluxo: Criar Ocorrência Durante Viagem

```
1. Usuário está visualizando viagem e clica em "Registrar Ocorrência"
   ↓
2. Modal abre com formulário (Resumo, Descrição, Upload de imagem)
   ↓
3. Usuário preenche dados e faz upload de imagem
   ↓
4. JavaScript envia FormData via AJAX POST
   ↓
5. Controller recebe OcorrenciaViagem + IFormFile
   ↓
6. Se há imagem:
   ├─ Gera nome único (Guid + extensão)
   ├─ Salva em /wwwroot/Uploads/ocorrencias/
   └─ Armazena caminho em ImagemOcorrencia
   ↓
7. Preenche dados automáticos:
   ├─ OcorrenciaViagemId = Guid.NewGuid()
   ├─ DataCriacao = DateTime.Now
   ├─ Status = "Aberta"
   ├─ StatusOcorrencia = true
   └─ UsuarioCriacao = usuário atual
   ↓
8. Salva no banco
   ↓
9. Retorna sucesso
   ↓
10. Modal fecha e lista de ocorrências é atualizada
```

### Fluxo: Baixar Ocorrência

```
1. Usuário visualiza ocorrência aberta e clica em "Baixar"
   ↓
2. Modal abre com campos: Solucao, Observacoes, ItemManutencaoId
   ↓
3. Usuário preenche solução e opcionalmente vincula manutenção
   ↓
4. JavaScript envia dados via AJAX POST
   ↓
5. Controller busca ocorrência pelo ID
   ↓
6. Atualiza campos:
   ├─ Status = "Baixada"
   ├─ StatusOcorrencia = false
   ├─ DataBaixa = DateTime.Now
   ├─ UsuarioBaixa = usuário atual
   ├─ Solucao = dados do formulário
   ├─ Observacoes = dados do formulário
   └─ ItemManutencaoId = dados do formulário (opcional)
   ↓
7. Salva alterações
   ↓
8. Retorna sucesso
   ↓
9. Ocorrência desaparece da lista de abertas
```

---

## 🔍 Troubleshooting

### Erro: Imagem não aparece após upload

**Causa:** Caminho salvo está incorreto ou arquivo não foi salvo corretamente.

**Solução:**
```csharp
// ✅ Verificar se caminho está correto
var caminhoCompleto = Path.Combine(_hostingEnvironment.WebRootPath, 
    ocorrencia.ImagemOcorrencia.TrimStart('/'));
    
if (!System.IO.File.Exists(caminhoCompleto))
{
    // Arquivo não existe - verificar upload
}
```

### Erro: Status inconsistente entre campos

**Causa:** `Status` e `StatusOcorrencia` não estão sincronizados.

**Solução:**
```csharp
// ✅ Sempre sincronizar ao atualizar
private void SincronizarStatus(OcorrenciaViagem ocorrencia)
{
    if (ocorrencia.StatusOcorrencia == false)
    {
        ocorrencia.Status = "Baixada";
    }
    else
    {
        ocorrencia.Status = "Aberta";
        ocorrencia.StatusOcorrencia = true;
    }
}
```

---

## 📊 Endpoints API Resumidos

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/ocorrenciaviagem/listarporviagem/{viagemId}` | Lista ocorrências de uma viagem |
| `GET` | `/api/ocorrenciaviagem/listarocorrenciasmodal/{viagemId}` | Lista para modal |
| `GET` | `/api/ocorrenciaviagem/listargestao` | Lista todas com filtros |
| `POST` | `/api/ocorrenciaviagem/criar` | Cria nova ocorrência |
| `POST` | `/api/ocorrenciaviagem/criarcomimagem` | Cria com upload de imagem |
| `POST` | `/api/ocorrenciaviagem/baixar/{id}` | Baixa ocorrência |
| `PUT` | `/api/ocorrenciaviagem/atualizar/{id}` | Atualiza ocorrência |

---

## 📝 Notas Importantes

1. **Duplo controle de status** - `Status` (string) e `StatusOcorrencia` (bool?) devem estar sincronizados.

2. **Imagem como caminho** - `ImagemOcorrencia` armazena caminho relativo, não dados binários.

3. **Relacionamentos comentados** - Foreign keys virtuais estão comentadas, mas podem ser descomentadas se necessário.

4. **Rastreamento completo** - Campos `UsuarioCriacao` e `UsuarioBaixa` permitem auditoria completa.

---

**📅 Documentação criada em:** 08/01/2026  
**🔄 Última atualização:** 08/01/2026


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
- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

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
