# Documentação: Encarregado.cs

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

O Model `Encarregado` representa pessoas responsáveis por supervisionar contratos e operações de veículos no sistema FrotiX. Inclui dados pessoais, foto, vínculo com contrato e controle de status.

**Principais objetivos:**

✅ Cadastrar encarregados com dados pessoais completos (nome, CPF, celular, data nascimento)  
✅ Armazenar foto do encarregado em formato binário  
✅ Vincular encarregado a um contrato específico  
✅ Controlar status ativo/inativo  
✅ Rastrear alterações (quem alterou e quando)  
✅ Suportar upload de foto através de `IFormFile`

---

## 📁 Arquivos Envolvidos

### Arquivo Principal
- **`Models/Encarregado.cs`** - Model principal + EncarregadoViewModel

### Arquivos que Utilizam
- **`Controllers/EncarregadoController.cs`** - Endpoints CRUD
- **`Pages/Encarregado/Index.cshtml`** - Listagem de encarregados
- **`Pages/Encarregado/Upsert.cshtml`** - Formulário de criação/edição
- **`Repository/EncarregadoRepository.cs`** - Acesso a dados
- **`wwwroot/js/cadastros/encarregado.js`** - Lógica JavaScript do DataTable

---

## 🏗️ Estrutura do Model

### Classe Principal: Encarregado

```csharp
public class Encarregado
{
    // ✅ Chave primária
    [Key]
    public Guid EncarregadoId { get; set; }

    // ✅ Dados pessoais obrigatórios
    [StringLength(100)]
    [Required(ErrorMessage = "(O Nome é obrigatório)")]
    [Display(Name = "Nome do Encarregado")]
    public string? Nome { get; set; }

    [StringLength(20)]
    [Required(ErrorMessage = "(O Ponto é obrigatório)")]
    [Display(Name = "Ponto")]
    public string? Ponto { get; set; }

    [DataType(DataType.DateTime)]
    [Required(ErrorMessage = "(A data de nascimento é obrigatória)")]
    [Display(Name = "Data de Nascimento")]
    public DateTime? DataNascimento { get; set; }

    [StringLength(20)]
    [Required(ErrorMessage = "(O CPF é obrigatório)")]
    [Display(Name = "CPF")]
    public string? CPF { get; set; }

    // ✅ Contatos
    [StringLength(50)]
    [Required(ErrorMessage = "(O celular é obrigatório)")]
    [Display(Name = "Primeiro Celular")]
    public string? Celular01 { get; set; }

    [StringLength(50)]
    [Display(Name = "Segundo Celular")]
    public string? Celular02 { get; set; }

    // ✅ Dados profissionais
    [DataType(DataType.DateTime)]
    [Display(Name = "Data de Ingresso")]
    public DateTime? DataIngresso { get; set; }

    // ✅ Foto (armazenada como byte[])
    public byte[]? Foto { get; set; }

    // ✅ Status e controle
    [Display(Name = "Ativo/Inativo")]
    public bool Status { get; set; }

    public DateTime? DataAlteracao { get; set; }
    public string? UsuarioIdAlteracao { get; set; }

    // ✅ Vínculo com contrato
    [ValidaLista(ErrorMessage = "(O contrato é obrigatório)")]
    [Display(Name = "Contrato")]
    public Guid ContratoId { get; set; }

    [ForeignKey("ContratoId")]
    public virtual Contrato? Contrato { get; set; }

    // ✅ Campo não mapeado para upload de foto
    [NotMapped]
    public IFormFile? ArquivoFoto { get; set; }
}
```

### Classe ViewModel: EncarregadoViewModel

```csharp
public class EncarregadoViewModel
{
    public Guid EncarregadoId { get; set; }
    public Guid ContratoId { get; set; }
    public Encarregado? Encarregado { get; set; }
    public string? NomeUsuarioAlteracao { get; set; }
    public IEnumerable<SelectListItem>? ContratoList { get; set; }
}
```

**Uso do ViewModel:**
- ✅ Usado em `Pages/Encarregado/Upsert.cshtml.cs` para carregar dados do formulário
- ✅ Inclui lista de contratos para dropdown
- ✅ Inclui nome do usuário que fez última alteração (para exibição)

---

## 🗄️ Mapeamento Model ↔ Banco de Dados

### Estrutura SQL da Tabela

```sql
CREATE TABLE [dbo].[Encarregado] (
    [EncarregadoId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    
    -- Dados pessoais
    [Nome] NVARCHAR(100) NOT NULL,
    [Ponto] NVARCHAR(20) NOT NULL,
    [DataNascimento] DATETIME2 NOT NULL,
    [CPF] NVARCHAR(20) NOT NULL,
    
    -- Contatos
    [Celular01] NVARCHAR(50) NOT NULL,
    [Celular02] NVARCHAR(50) NULL,
    
    -- Dados profissionais
    [DataIngresso] DATETIME2 NULL,
    [Foto] VARBINARY(MAX) NULL,
    
    -- Status e controle
    [Status] BIT NOT NULL DEFAULT 1,
    [DataAlteracao] DATETIME2 NULL,
    [UsuarioIdAlteracao] NVARCHAR(450) NULL,
    
    -- Vínculo com contrato
    [ContratoId] UNIQUEIDENTIFIER NOT NULL,
    
    -- Foreign Keys
    CONSTRAINT [FK_Encarregado_Contrato] 
        FOREIGN KEY ([ContratoId]) REFERENCES [Contrato]([ContratoId]),
    CONSTRAINT [FK_Encarregado_UsuarioAlteracao] 
        FOREIGN KEY ([UsuarioIdAlteracao]) REFERENCES [AspNetUsers]([Id])
);

-- Índices
CREATE INDEX [IX_Encarregado_ContratoId] ON [Encarregado]([ContratoId]);
CREATE INDEX [IX_Encarregado_Status] ON [Encarregado]([Status]);
CREATE INDEX [IX_Encarregado_CPF] ON [Encarregado]([CPF]);
```

### Tabela Comparativa

| Campo Model | Tipo Model | Campo SQL | Tipo SQL | Nullable | Observações |
|-------------|------------|-----------|----------|----------|-------------|
| `EncarregadoId` | `Guid` | `EncarregadoId` | `UNIQUEIDENTIFIER` | ❌ | Chave primária |
| `Nome` | `string?` | `Nome` | `NVARCHAR(100)` | ❌ | Nome completo |
| `Ponto` | `string?` | `Ponto` | `NVARCHAR(20)` | ❌ | Identificação do ponto |
| `DataNascimento` | `DateTime?` | `DataNascimento` | `DATETIME2` | ❌ | Data de nascimento |
| `CPF` | `string?` | `CPF` | `NVARCHAR(20)` | ❌ | CPF do encarregado |
| `Celular01` | `string?` | `Celular01` | `NVARCHAR(50)` | ❌ | Telefone principal |
| `Celular02` | `string?` | `Celular02` | `NVARCHAR(50)` | ✅ | Telefone secundário |
| `DataIngresso` | `DateTime?` | `DataIngresso` | `DATETIME2` | ✅ | Data de entrada |
| `Foto` | `byte[]?` | `Foto` | `VARBINARY(MAX)` | ✅ | Foto em binário |
| `Status` | `bool` | `Status` | `BIT` | ❌ | Ativo/Inativo |
| `DataAlteracao` | `DateTime?` | `DataAlteracao` | `DATETIME2` | ✅ | Última alteração |
| `UsuarioIdAlteracao` | `string?` | `UsuarioIdAlteracao` | `NVARCHAR(450)` | ✅ | FK para AspNetUsers |
| `ContratoId` | `Guid` | `ContratoId` | `UNIQUEIDENTIFIER` | ❌ | FK para Contrato |

**Triggers:** Nenhum trigger associado a esta tabela.

---

## 🔗 Quem Chama e Por Quê

### 1. **EncarregadoController.cs** → Listagem com JOINs

**Quando:** Página Index carrega lista de encarregados  
**Por quê:** Precisa exibir informações do contrato e fornecedor relacionados

```csharp
[HttpGet]
public IActionResult Get()
{
    var result = (
        from e in _unitOfWork.Encarregado.GetAll()
        
        join ct in _unitOfWork.Contrato.GetAll()
            on e.ContratoId equals ct.ContratoId
            into ctr
        from ctrResult in ctr.DefaultIfEmpty() // ✅ LEFT JOIN
        
        join f in _unitOfWork.Fornecedor.GetAll()
            on ctrResult?.FornecedorId equals f.FornecedorId
            into frd
        from frdResult in frd.DefaultIfEmpty() // ✅ LEFT JOIN
        
        select new
        {
            e.EncarregadoId,
            e.Nome,
            e.Ponto,
            e.Celular01,
            e.Status,
            ContratoEncarregado = ctrResult != null 
                ? $"{ctrResult.AnoContrato}/{ctrResult.NumeroContrato} - {frdResult?.DescricaoFornecedor}"
                : "Sem contrato"
        }
    ).ToList();
    
    return Json(new { data = result });
}
```

### 2. **Pages/Encarregado/Upsert.cshtml.cs** → Upload de Foto

**Quando:** Usuário faz upload de foto no formulário  
**Por quê:** Converter `IFormFile` para `byte[]` e salvar no banco

```csharp
if (FotoUpload != null && FotoUpload.Length > 0)
{
    using (var memoryStream = new MemoryStream())
    {
        await FotoUpload.CopyToAsync(memoryStream);
        EncarregadoObj.Encarregado.Foto = memoryStream.ToArray(); // ✅ Converte para byte[]
    }
}
```

---

## 🛠️ Problema → Solução → Código

### Problema: Upload e Armazenamento de Foto

**Problema:** Fotos precisam ser enviadas via formulário HTML (`IFormFile`), mas o banco armazena como `byte[]`. É necessário converter e validar tamanho/formato.

**Solução:** Usar propriedade `[NotMapped]` para `ArquivoFoto` (não vai para banco) e converter para `byte[]` antes de salvar.

**Código:**

```csharp
// ✅ Em Upsert.cshtml.cs
[BindProperty]
public IFormFile FotoUpload { get; set; }

public async Task<IActionResult> OnPostAsync()
{
    if (FotoUpload != null && FotoUpload.Length > 0)
    {
        // ✅ Valida tamanho (máximo 5MB)
        if (FotoUpload.Length > 5 * 1024 * 1024)
        {
            _notyf.Error("Foto muito grande. Tamanho máximo: 5MB");
            return Page();
        }
        
        // ✅ Valida formato (apenas imagens)
        var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extensao = Path.GetExtension(FotoUpload.FileName).ToLower();
        if (!extensoesPermitidas.Contains(extensao))
        {
            _notyf.Error("Formato inválido. Use apenas JPG, PNG ou GIF");
            return Page();
        }
        
        // ✅ Converte IFormFile para byte[]
        using (var memoryStream = new MemoryStream())
        {
            await FotoUpload.CopyToAsync(memoryStream);
            EncarregadoObj.Encarregado.Foto = memoryStream.ToArray();
        }
    }
    
    // ✅ Salva no banco
    if (EncarregadoObj.Encarregado.EncarregadoId == Guid.Empty)
    {
        _unitOfWork.Encarregado.Add(EncarregadoObj.Encarregado);
    }
    else
    {
        _unitOfWork.Encarregado.Update(EncarregadoObj.Encarregado);
    }
    
    _unitOfWork.Save();
    return RedirectToPage("./Index");
}
```

### Problema: Exibição de Foto em Modal

**Problema:** Foto está em `byte[]` no banco, precisa ser convertida para exibir em `<img>` HTML.

**Solução:** Criar endpoint que retorna foto como `FileResult` convertendo `byte[]` para imagem.

**Código:**

```csharp
// ✅ Em EncarregadoController.cs
[HttpGet("Foto/{id}")]
public IActionResult GetFoto(Guid id)
{
    var encarregado = _unitOfWork.Encarregado.GetFirstOrDefault(e => e.EncarregadoId == id);
    
    if (encarregado?.Foto == null || encarregado.Foto.Length == 0)
    {
        // ✅ Retorna imagem padrão se não houver foto
        return File("/Images/barbudo.jpg", "image/jpeg");
    }
    
    // ✅ Retorna foto do banco
    return File(encarregado.Foto, "image/jpeg");
}
```

```javascript
// ✅ Em encarregado.js
function verFoto(encarregadoId) {
    $('#imgViewer').attr('src', `/api/encarregado/foto/${encarregadoId}`);
    $('#modalFoto').modal('show');
}
```

---

## 🔄 Fluxo de Funcionamento

### Fluxo: Cadastro de Encarregado com Foto

```
1. Usuário acessa /Encarregado/Upsert
   ↓
2. Preenche formulário (nome, CPF, celular, contrato)
   ↓
3. Seleciona arquivo de foto no input file
   ↓
4. JavaScript valida tamanho/formato client-side
   ↓
5. Submete formulário via POST
   ↓
6. Upsert.cshtml.cs recebe IFormFile FotoUpload
   ↓
7. Valida tamanho/formato server-side
   ↓
8. Converte IFormFile → byte[] usando MemoryStream
   ↓
9. Atribui byte[] à propriedade Encarregado.Foto
   ↓
10. Salva Encarregado no banco (Foto como VARBINARY)
   ↓
11. Redireciona para Index
```

### Fluxo: Visualização de Foto

```
1. Usuário clica em ícone de foto na listagem
   ↓
2. JavaScript chama função verFoto(encarregadoId)
   ↓
3. Atualiza src da <img> para /api/encarregado/foto/{id}
   ↓
4. Abre modal Bootstrap com foto
   ↓
5. Controller busca Encarregado do banco
   ↓
6. Se Foto != null → Retorna File(encarregado.Foto, "image/jpeg")
   ↓
7. Se Foto == null → Retorna imagem padrão
   ↓
8. Browser exibe foto no modal
```

---

## 🔍 Troubleshooting

### Erro: Foto não aparece após upload

**Causa:** `byte[]` não está sendo salvo corretamente ou conversão falhou.

**Solução:**
```csharp
// ✅ Verificar se conversão está funcionando
if (FotoUpload != null)
{
    using (var memoryStream = new MemoryStream())
    {
        await FotoUpload.CopyToAsync(memoryStream);
        var fotoBytes = memoryStream.ToArray();
        
        // ✅ Debug: verificar tamanho
        Console.WriteLine($"Tamanho da foto: {fotoBytes.Length} bytes");
        
        EncarregadoObj.Encarregado.Foto = fotoBytes;
    }
}
```

### Erro: Foto muito grande causa timeout

**Causa:** Foto maior que limite configurado no servidor.

**Solução:**
```csharp
// ✅ Configurar limite em Startup.cs ou Program.cs
services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
});
```

### Erro: CPF duplicado

**Causa:** Não há validação de unicidade no banco.

**Solução:**
```csharp
// ✅ Validar antes de salvar
var existe = _unitOfWork.Encarregado
    .GetFirstOrDefault(e => e.CPF == encarregado.CPF && 
                            e.EncarregadoId != encarregado.EncarregadoId);
if (existe != null)
{
    _notyf.Error("CPF já cadastrado para outro encarregado");
    return Page();
}
```

---

## 📊 Endpoints API Resumidos

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/encarregado` | Lista todos os encarregados com JOINs |
| `GET` | `/api/encarregado/{id}` | Detalhes de um encarregado |
| `GET` | `/api/encarregado/foto/{id}` | Retorna foto como imagem |
| `POST` | `/api/encarregado` | Cria novo encarregado |
| `PUT` | `/api/encarregado/{id}` | Atualiza encarregado |
| `DELETE` | `/api/encarregado/{id}` | Deleta encarregado |

---

## 📝 Notas Importantes

1. **Foto em `byte[]`** - Armazenada diretamente no banco como `VARBINARY(MAX)`, não em arquivo físico.

2. **`[NotMapped]`** - `ArquivoFoto` não é mapeado para banco, usado apenas para upload.

3. **Validação customizada** - `[ValidaLista]` valida se `ContratoId` foi selecionado no dropdown.

4. **Rastreamento de alterações** - `UsuarioIdAlteracao` e `DataAlteracao` são preenchidos automaticamente no Update.

5. **Relacionamento obrigatório** - `ContratoId` é obrigatório, não pode ser NULL.

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
