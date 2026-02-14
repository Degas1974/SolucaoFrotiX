# Documentação: UpsertCupons.cshtml.cs

> **Última Atualização**: 13/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Lógica de Negócio](#lógica-de-negócio)
5. [Interconexões](#interconexões)
6. [Validações](#validações)
7. [Upload de Arquivos](#upload-de-arquivos)
8. [Exemplos de Uso](#exemplos-de-uso)
9. [Troubleshooting](#troubleshooting)

---

## Visão Geral

O arquivo `UpsertCupons.cshtml.cs` é o **PageModel** (C# backend) da página de criação e edição de **Cupons de Abastecimento** do sistema FrotiX.

### Características Principais
- ✅ **CRUD Completo**: Create (INSERT) e Update (EDIT) de cupons de abastecimento
- ✅ **Upload de PDFs**: Permite anexar cupons fiscais em PDF
- ✅ **Normalização de Nomes**: Remove acentos e espaços dos nomes de arquivo
- ✅ **Repository Pattern**: Usa Unit of Work para acesso ao banco de dados
- ✅ **Toast Notifications**: Feedback visual via Notyf
- ✅ **Try-Catch Global**: Tratamento de erros em todos os métodos
- ✅ **Redirecionamento**: Após salvar, retorna para `./RegistraCupons`

### Objetivo
Permitir que operadores registrem cupons de abastecimento, anexando documentos fiscais (PDFs) e associando-os a viagens/veículos. Funciona como formulário inline de criação/edição dentro do fluxo de registro de cupons.

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| ASP.NET Core | 3.1+ | Razor Pages (PageModel) |
| Entity Framework Core | 3.1+ | ORM via Repository Pattern |
| AspNetCoreHero.ToastNotification | - | Notificações toast (Notyf) |
| IFormFile | - | Upload de arquivos |
| System.IO | - | Manipulação de arquivos/diretórios |

### Padrões de Design
- **Repository Pattern**: Acesso ao banco via `IUnitOfWork`
- **Dependency Injection**: Injeção de `IUnitOfWork`, `INotyfService`, `IWebHostEnvironment`
- **ViewModel**: Usa `RegistroCupomAbastecimentoViewModel` para binding
- **Try-Catch Global**: Tratamento de erros consistente em todos os métodos

---

## Estrutura de Arquivos

### Arquivo Principal
```
Pages/Abastecimento/UpsertCupons.cshtml.cs
```

### Arquivos Relacionados
- `Pages/Abastecimento/UpsertCupons.cshtml` - View Razor (frontend)
- `Pages/Abastecimento/RegistraCupons.cshtml` - Página de listagem (destino após salvar)
- `Models/RegistroCupomAbastecimento.cs` - Model de entidade
- `Models/RegistroCupomAbastecimentoViewModel.cs` - ViewModel
- `Repository/IRepository/IUnitOfWork.cs` - Interface do Repository Pattern
- `Helpers/Alerta.cs` - Helper de tratamento de erros

### Diretório de Upload
```
wwwroot/DadosEditaveis/Cupons/
```
Cupons PDF são salvos neste diretório.

---

## Lógica de Negócio

### Propriedades

#### `RegistroCupomId` (Estática)
**Localização**: Linha 21

**Tipo**: `Guid` (estático)

**Propósito**: Armazena o ID do registro de cupom atual durante a operação. Usado para manter estado entre requisições.

**Observação**: Propriedade estática pode causar problemas em ambientes multi-thread. Considerar usar `TempData` ou session state em futuras refatorações.

---

#### `RegistroCupomAbastecimentoObj` (Bound Property)
**Localização**: Linha 42-45

**Tipo**: `RegistroCupomAbastecimentoViewModel`

**Atributo**: `[BindProperty]` - Permite binding bidirecional entre view e PageModel

**Propósito**: Objeto que encapsula os dados do cupom de abastecimento para criação/edição.

---

### Métodos Principais

#### Construtor: `UpsertCuponsModel()` (Linhas 23-39)

**Propósito**: Injeção de dependências e inicialização do PageModel.

**Parâmetros**:
- `IUnitOfWork unitOfWork` - Acesso ao repositório de dados
- `INotyfService notyf` - Serviço de notificações toast
- `IWebHostEnvironment hostingEnvironment` - Ambiente de hospedagem (para caminhos de arquivo)

**Exemplo de Código**:
```csharp
public UpsertCuponsModel(
    IUnitOfWork unitOfWork,
    INotyfService notyf,
    IWebHostEnvironment hostingEnvironment
)
{
    try
    {
        _unitOfWork = unitOfWork;
        _notyf = notyf;
        _hostingEnvironment = hostingEnvironment;
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs", "UpsertCuponsModel", error);
    }
}
```

**Fluxo de Execução**:
1. Recebe dependências injetadas pelo ASP.NET Core
2. Armazena referências em campos privados
3. Em caso de erro (improvável no construtor), registra com `Alerta.TratamentoErroComLinha()`

---

#### Método: `SetViewModel()` (Linhas 47-61)

**Propósito**: Inicializa o ViewModel com uma nova instância de `RegistroCupomAbastecimento`.

**Fluxo de Execução**:
1. Cria nova instância de `RegistroCupomAbastecimentoViewModel`
2. Inicializa propriedade `RegistroCupomAbastecimento` com novo objeto vazio
3. Em caso de erro, registra e retorna sem valor (void)

**Exemplo de Código**:
```csharp
private void SetViewModel()
{
    try
    {
        RegistroCupomAbastecimentoObj = new RegistroCupomAbastecimentoViewModel
        {
            RegistroCupomAbastecimento = new Models.RegistroCupomAbastecimento(),
        };
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs", "SetViewModel", error);
        return;
    }
}
```

**Quando é Chamado**:
- Início do método `OnGet()` para preparar o formulário

---

#### Método: `OnGet(Guid id)` (Linhas 63-93)

**Propósito**: Handler do GET request - carrega dados para criação (id vazio) ou edição (id existente).

**Parâmetros**:
- `Guid id` - ID do cupom a editar (ou `Guid.Empty` para novo cupom)

**Retorno**: `IActionResult` - `Page()` para renderizar a view ou `NotFound()` se registro não existir

**Fluxo de Execução**:
1. Chama `SetViewModel()` para inicializar ViewModel
2. Se `id != Guid.Empty`:
   - Armazena ID na propriedade estática `RegistroCupomId`
   - Busca cupom no banco via `_unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault()`
   - Se não encontrar, retorna `NotFound()`
3. Retorna `Page()` para renderizar o formulário

**Exemplo de Código**:
```csharp
public IActionResult OnGet(Guid id)
{
    try
    {
        SetViewModel();

        if (id != Guid.Empty)
        {
            RegistroCupomId = id;
        }

        if (id != Guid.Empty)
        {
            RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento =
                _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(m =>
                    m.RegistroCupomId == id
                );
            if (RegistroCupomAbastecimentoObj == null)
            {
                return NotFound();
            }
        }

        return Page();
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs", "OnGet", error);
        return Page();
    }
}
```

**Casos Especiais**:
- **Novo cupom** (`id == Guid.Empty`): Formulário vazio para criação
- **Editar cupom** (`id != Guid.Empty`): Formulário preenchido com dados existentes
- **Cupom não encontrado**: Retorna `NotFound()` (HTTP 404)

---

#### Método: `OnPostSubmit()` (Linhas 95-112)

**Propósito**: Handler do POST para **criação** de novo cupom.

**Retorno**: `IActionResult` - Redireciona para `./RegistraCupons`

**Fluxo de Execução**:
1. Adiciona cupom no repositório via `_unitOfWork.RegistroCupomAbastecimento.Add()`
2. Persiste mudanças com `_unitOfWork.Save()`
3. Mostra notificação de sucesso via `_notyf.Success()`
4. Redireciona para página de listagem `./RegistraCupons`

**Exemplo de Código**:
```csharp
public IActionResult OnPostSubmit()
{
    try
    {
        _notyf.Success("Registro adicionado com sucesso!", 3);
        _unitOfWork.RegistroCupomAbastecimento.Add(
            RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento
        );
        _unitOfWork.Save();

        return RedirectToPage("./RegistraCupons");
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs", "OnPostSubmit", error);
        return RedirectToPage("./RegistraCupons");
    }
}
```

**Observação**: Mesmo em caso de erro, redireciona para listagem. A notificação de erro é exibida via `Alerta.TratamentoErroComLinha()`.

---

#### Método: `OnPostEdit(Guid Id)` (Linhas 114-146)

**Propósito**: Handler do POST para **atualização** de cupom existente.

**Parâmetros**:
- `Guid Id` - ID do cupom a atualizar

**Retorno**: `IActionResult` - Redireciona para `./RegistraCupons`

**Fluxo de Execução**:
1. Define o `RegistroCupomId` do objeto com o ID recebido
2. Atualiza o cupom no repositório via `_unitOfWork.RegistroCupomAbastecimento.Update()`
3. Persiste mudanças com `_unitOfWork.Save()`
4. Mostra notificação de sucesso via `_notyf.Success()`
5. Redireciona para página de listagem `./RegistraCupons`

**Exemplo de Código**:
```csharp
public IActionResult OnPostEdit(Guid Id)
{
    try
    {
        RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento.RegistroCupomId = Id;

        Guid RegistroCupomId = Guid.Empty;

        if (
            RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento.RegistroCupomId
            != Guid.Empty
        )
        {
            RegistroCupomId = RegistroCupomAbastecimentoObj
                .RegistroCupomAbastecimento
                .RegistroCupomId;
        }

        _notyf.Success("Registro atualizado com sucesso!", 3);
        _unitOfWork.RegistroCupomAbastecimento.Update(
            RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento
        );

        _unitOfWork.Save();

        return RedirectToPage("./RegistraCupons");
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs", "OnPostEdit", error);
        return RedirectToPage("./RegistraCupons");
    }
}
```

**Observação**: A variável local `RegistroCupomId` é declarada mas parece não ser usada após atribuição. Possível código legado.

---

#### Método: `OnPostSavePDF(IEnumerable<IFormFile> files)` (Linhas 148-183)

**Propósito**: Handler do POST para **upload de arquivos PDF** (cupons fiscais).

**Parâmetros**:
- `IEnumerable<IFormFile> files` - Coleção de arquivos enviados via form

**Retorno**: `ActionResult` - `Content("")` vazio (requisição AJAX)

**Fluxo de Execução**:
1. Verifica se `files` não é nulo
2. Para cada arquivo:
   - Define caminho de destino: `wwwroot/DadosEditaveis/Cupons/`
   - Cria diretório se não existir
   - Normaliza nome do arquivo (remove acentos, substitui espaços por `_`, converte para UPPERCASE)
   - Copia arquivo para o destino via `FileStream`
3. Retorna `Content("")` vazio (requisição AJAX não precisa de resposta)

**Exemplo de Código**:
```csharp
public ActionResult OnPostSavePDF(IEnumerable<IFormFile> files)
{
    try
    {
        if (files != null)
        {
            foreach (var file in files)
            {
                string folderName = "DadosEditaveis/Cupons";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    string fullPath = Path.Combine(newPath, TiraAcento(file.FileName));
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
            }
        }

        return Content("");
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs", "OnPostSavePDF", error);
        return Content("");
    }
}
```

**Casos Especiais**:
- **Múltiplos arquivos**: Loop processa todos os arquivos enviados
- **Arquivo vazio** (`file.Length <= 0`): Ignorado silenciosamente
- **Diretório inexistente**: Criado automaticamente

---

#### Método: `TiraAcento(string frase)` (Linhas 185-204)

**Propósito**: Normaliza string removendo acentos, substituindo espaços por `_` e convertendo para UPPERCASE.

**Parâmetros**:
- `string frase` - String original a normalizar

**Retorno**: `string` - String normalizada ou vazia em caso de erro

**Fluxo de Execução**:
1. Cria `StringBuilder` para resultado
2. Para cada caractere da string:
   - Chama `RemoveAcento(c)` para remover acentuação
   - Se caractere é espaço, substitui por `_`
   - Adiciona ao resultado
3. Converte resultado para UPPERCASE via `ToUpper()`
4. Retorna string normalizada

**Exemplo de Código**:
```csharp
static string TiraAcento(string frase)
{
    try
    {
        StringBuilder resultado = new StringBuilder();

        foreach (char c in frase)
        {
            char caractereSemAcento = RemoveAcento(c);
            resultado.Append(caractereSemAcento == ' ' ? '_' : caractereSemAcento);
        }

        return resultado.ToString().ToUpper();
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs", "TiraAcento", error);
        return string.Empty;
    }
}
```

**Exemplos de Transformação**:
- "Nota Fiscal.pdf" → "NOTA_FISCAL.PDF"
- "Cupão de Abastecimento.pdf" → "CUPAO_DE_ABASTECIMENTO.PDF"
- "Récibo Número 123.pdf" → "RECIBO_NUMERO_123.PDF"

---

#### Método: `RemoveAcento(char c)` (Linhas 206-275)

**Propósito**: Remove acentuação de um único caractere, convertendo para versão sem acento.

**Parâmetros**:
- `char c` - Caractere a normalizar

**Retorno**: `char` - Caractere sem acento ou caractere original se não tiver acento

**Fluxo de Execução**:
1. Usa `switch` para mapear caracteres acentuados para versões sem acento
2. Suporta acentos: agudo (´), grave (`), circunflexo (^), til (~), cedilha (ç)
3. Versões maiúsculas e minúsculas
4. Se caractere não tiver mapeamento, retorna original

**Exemplo de Código**:
```csharp
static char RemoveAcento(char c)
{
    try
    {
        switch (c)
        {
            case 'Á':
            case 'á':
                return 'a';
            case 'É':
            case 'é':
                return 'e';
            case 'Í':
            case 'í':
                return 'i';
            case 'Ó':
            case 'ó':
                return 'o';
            case 'Ú':
            case 'ú':
                return 'u';
            // ... outros casos ...
            case 'Ç':
            case 'ç':
                return 'c';
            default:
                return c;
        }
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs", "RemoveAcento", error);
        return default(char);
    }
}
```

**Mapeamentos Suportados**:
- Agudo: á→a, é→e, í→i, ó→o, ú→u
- Grave: à→a, è→e, ì→i, ò→o, ù→u
- Circunflexo: â→a, ê→e, î→i, ô→o, û→u
- Til: ã→a, õ→o
- Cedilha: ç→c
- Maiúsculas e minúsculas

---

## Interconexões

### Quem Chama Este PageModel

**Frontend (View)**:
- `Pages/Abastecimento/UpsertCupons.cshtml` - View Razor que renderiza o formulário
  - Chama `OnGet()` quando página é carregada
  - Chama `OnPostSubmit()` quando formulário é submetido (novo cupom)
  - Chama `OnPostEdit()` quando formulário é submetido (editar cupom)
  - Chama `OnPostSavePDF()` via AJAX para upload de PDFs

**Outras Páginas**:
- `Pages/Abastecimento/RegistraCupons.cshtml` - Página de listagem
  - Redireciona para `UpsertCupons?id=Guid` para edição
  - Redireciona para `UpsertCupons` (sem id) para novo cupom

---

### O Que Este PageModel Chama

**Repository (Banco de Dados)**:
- `IUnitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault()` - Busca cupom por ID
- `IUnitOfWork.RegistroCupomAbastecimento.Add()` - Adiciona novo cupom
- `IUnitOfWork.RegistroCupomAbastecimento.Update()` - Atualiza cupom existente
- `IUnitOfWork.Save()` - Persiste mudanças no banco

**Notificações**:
- `INotyfService.Success()` - Exibe toast de sucesso

**Sistema de Arquivos**:
- `IWebHostEnvironment.WebRootPath` - Obtém caminho raiz da aplicação
- `Directory.Exists()` / `Directory.CreateDirectory()` - Gerencia diretórios
- `FileStream.CopyTo()` - Copia arquivo para disco

**Tratamento de Erros**:
- `Alerta.TratamentoErroComLinha()` - Registra e exibe erros

---

### Fluxo de Dados

**Criação de Novo Cupom**:
```
Usuário acessa /Abastecimento/UpsertCupons
    ↓
OnGet(Guid.Empty) → SetViewModel() → Page()
    ↓
Usuário preenche formulário e anexa PDF
    ↓
OnPostSavePDF() → Salva PDF em wwwroot/DadosEditaveis/Cupons/
    ↓
OnPostSubmit() → Add() → Save() → Redirect RegistraCupons
```

**Edição de Cupom Existente**:
```
Usuário clica "Editar" em RegistraCupons
    ↓
OnGet(cupomId) → SetViewModel() → GetFirstOrDefault() → Page()
    ↓
Usuário modifica dados e/ou anexa novo PDF
    ↓
OnPostSavePDF() (opcional) → Salva novo PDF
    ↓
OnPostEdit(cupomId) → Update() → Save() → Redirect RegistraCupons
```

---

## Validações

### Frontend (View)
Validações são implementadas na view `UpsertCupons.cshtml` (não neste arquivo).

### Backend (PageModel)

#### Validação de ID
**Método**: `OnGet()`
```csharp
if (id != Guid.Empty)
{
    RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento =
        _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(m =>
            m.RegistroCupomId == id
        );
    if (RegistroCupomAbastecimentoObj == null)
    {
        return NotFound(); // HTTP 404 se não encontrar
    }
}
```
**Regra**: Se ID fornecido, deve existir no banco. Caso contrário, retorna 404.

---

#### Validação de Arquivos
**Método**: `OnPostSavePDF()`
```csharp
if (files != null)
{
    foreach (var file in files)
    {
        if (file.Length > 0) // Ignora arquivos vazios
        {
            // ... salva arquivo
        }
    }
}
```
**Regra**: Apenas arquivos com tamanho > 0 são processados.

**Observação**: Não há validação de tipo de arquivo (extensão .pdf). Qualquer arquivo pode ser enviado.

---

## Upload de Arquivos

### Configuração

**Diretório de Destino**:
```
wwwroot/DadosEditaveis/Cupons/
```

**Caminho Absoluto** (exemplo):
```
C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site\wwwroot\DadosEditaveis\Cupons\
```

---

### Processo de Upload

1. **Recebe arquivos** via `IEnumerable<IFormFile> files`
2. **Cria diretório** se não existir
3. **Normaliza nome** via `TiraAcento()`:
   - Remove acentos
   - Substitui espaços por `_`
   - Converte para UPPERCASE
4. **Salva arquivo** via `FileStream`

**Exemplo de Nome Normalizado**:
- Original: `Nota Fiscal São Paulo.pdf`
- Normalizado: `NOTA_FISCAL_SAO_PAULO.PDF`

---

### Segurança

**Riscos Identificados**:
- ❌ **Sem validação de extensão**: Qualquer arquivo pode ser enviado
- ❌ **Sem validação de tamanho máximo**: Arquivos grandes podem causar problemas
- ❌ **Sem validação de conteúdo**: Não verifica se é realmente um PDF
- ❌ **Sobrescrita de arquivos**: Arquivo com mesmo nome é sobrescrito sem aviso
- ❌ **Path Traversal**: Nome de arquivo malicioso pode navegar diretórios

**Recomendações de Melhoria**:
1. Validar extensão permitida (apenas .pdf)
2. Definir tamanho máximo de arquivo (ex: 5 MB)
3. Gerar nomes únicos (GUID + extensão) para evitar sobrescrita
4. Sanitizar nome de arquivo para evitar path traversal
5. Validar conteúdo do arquivo (magic numbers do PDF)

---

## Exemplos de Uso

### Cenário 1: Criar Novo Cupom

**Passos**:
1. Usuário navega para `/Abastecimento/UpsertCupons`
2. `OnGet(Guid.Empty)` é chamado
3. Formulário vazio é exibido
4. Usuário preenche dados e anexa PDF
5. JavaScript chama `OnPostSavePDF()` via AJAX (PDF é salvo)
6. Usuário clica em "Salvar"
7. `OnPostSubmit()` é chamado
8. Cupom é adicionado ao banco
9. Toast "Registro adicionado com sucesso!" é exibido
10. Usuário é redirecionado para `/Abastecimento/RegistraCupons`

**Resultado Esperado**: Novo cupom criado e PDF salvo em `wwwroot/DadosEditaveis/Cupons/`

---

### Cenário 2: Editar Cupom Existente

**Passos**:
1. Usuário clica "Editar" em `/Abastecimento/RegistraCupons`
2. Navegação para `/Abastecimento/UpsertCupons?id=abc-123-def`
3. `OnGet(abc-123-def)` é chamado
4. Cupom é buscado no banco via `GetFirstOrDefault()`
5. Formulário é preenchido com dados existentes
6. Usuário modifica dados e/ou anexa novo PDF
7. JavaScript chama `OnPostSavePDF()` via AJAX (se novo PDF)
8. Usuário clica em "Atualizar"
9. `OnPostEdit(abc-123-def)` é chamado
10. Cupom é atualizado no banco
11. Toast "Registro atualizado com sucesso!" é exibido
12. Usuário é redirecionado para `/Abastecimento/RegistraCupons`

**Resultado Esperado**: Cupom atualizado no banco e novo PDF salvo (se anexado)

---

## Troubleshooting

### Problema: Cupom não é salvo

**Sintomas**:
- Formulário é submetido
- Toast de sucesso é exibido
- Mas cupom não aparece na listagem

**Causas Possíveis**:
1. Erro silencioso no `_unitOfWork.Save()`
2. Transação não foi commitada
3. Entity Framework tracking issues

**Diagnóstico**:
1. Verificar logs de erro em `Alerta.TratamentoErroComLinha()`
2. Adicionar breakpoint em `OnPostSubmit()` linha 103
3. Verificar se `_unitOfWork.Save()` lança exceção
4. Verificar banco de dados diretamente

**Solução**:
```csharp
// Adicionar logging explícito
try
{
    _notyf.Success("Registro adicionado com sucesso!", 3);
    _unitOfWork.RegistroCupomAbastecimento.Add(
        RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento
    );

    var rowsAffected = _unitOfWork.Save();

    // Log para diagnóstico
    if (rowsAffected == 0)
    {
        throw new Exception("Nenhuma linha foi afetada no Save()");
    }

    return RedirectToPage("./RegistraCupons");
}
catch (Exception error)
{
    Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs", "OnPostSubmit", error);
    _notyf.Error("Erro ao salvar cupom: " + error.Message, 5);
    return Page(); // Mantém usuário na página para corrigir
}
```

---

### Problema: PDF não é enviado

**Sintomas**:
- Upload parece funcionar
- Mas arquivo não aparece em `wwwroot/DadosEditaveis/Cupons/`

**Causas Possíveis**:
1. `file.Length == 0` (arquivo vazio)
2. Erro ao criar diretório (permissões)
3. Erro ao copiar arquivo (disco cheio, permissões)
4. Nome de arquivo inválido após normalização

**Diagnóstico**:
1. Verificar logs de erro
2. Verificar permissões do diretório `wwwroot/DadosEditaveis/Cupons/`
3. Adicionar logging em `OnPostSavePDF()`:

```csharp
if (file.Length > 0)
{
    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
    string normalizedName = TiraAcento(file.FileName);
    string fullPath = Path.Combine(newPath, normalizedName);

    Console.WriteLine($"Salvando arquivo: {file.FileName}");
    Console.WriteLine($"Nome normalizado: {normalizedName}");
    Console.WriteLine($"Caminho completo: {fullPath}");
    Console.WriteLine($"Tamanho: {file.Length} bytes");

    using (var stream = new FileStream(fullPath, FileMode.Create))
    {
        file.CopyTo(stream);
    }

    Console.WriteLine("Arquivo salvo com sucesso!");
}
```

**Solução**:
- Verificar permissões do diretório
- Garantir espaço em disco
- Validar nome de arquivo antes de salvar

---

### Problema: NotFound() ao editar cupom

**Sintomas**:
- Usuário clica "Editar"
- Recebe HTTP 404

**Causas Possíveis**:
1. ID inválido (cupom foi deletado)
2. ID não é um Guid válido
3. Erro no `GetFirstOrDefault()`

**Diagnóstico**:
1. Verificar URL: `/Abastecimento/UpsertCupons?id=xxx`
2. Validar se `id` é um Guid válido
3. Verificar se cupom existe no banco

**Solução**:
```csharp
public IActionResult OnGet(Guid id)
{
    try
    {
        SetViewModel();

        if (id != Guid.Empty)
        {
            RegistroCupomId = id;

            Console.WriteLine($"Buscando cupom com ID: {id}");

            RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento =
                _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(m =>
                    m.RegistroCupomId == id
                );

            if (RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento == null)
            {
                Console.WriteLine("Cupom não encontrado!");
                _notyf.Error($"Cupom com ID {id} não encontrado", 5);
                return RedirectToPage("./RegistraCupons"); // Redirect ao invés de NotFound
            }

            Console.WriteLine("Cupom encontrado com sucesso!");
        }

        return Page();
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("UpsertCupons.cshtml.cs", "OnGet", error);
        return RedirectToPage("./RegistraCupons");
    }
}
```

---

### Problema: Propriedade estática causa concorrência

**Sintomas**:
- Múltiplos usuários editando cupons diferentes
- Dados misturados entre requisições

**Causa**:
```csharp
public static Guid RegistroCupomId; // PROPRIEDADE ESTÁTICA!
```
Propriedades estáticas são compartilhadas entre todas as requisições. Em ambiente multi-thread, isso causa race conditions.

**Solução**: Usar `TempData` ou session state:

```csharp
// Remover propriedade estática
// public static Guid RegistroCupomId; // REMOVER

// Usar TempData ao invés
public IActionResult OnGet(Guid id)
{
    try
    {
        SetViewModel();

        if (id != Guid.Empty)
        {
            TempData["RegistroCupomId"] = id; // Armazena em TempData

            RegistroCupomAbastecimentoObj.RegistroCupomAbastecimento =
                _unitOfWork.RegistroCupomAbastecimento.GetFirstOrDefault(m =>
                    m.RegistroCupomId == id
                );
            // ...
        }

        return Page();
    }
    // ...
}

// Recuperar em outros métodos
public IActionResult OnPostEdit(Guid Id)
{
    try
    {
        var cupomId = TempData["RegistroCupomId"] != null
            ? (Guid)TempData["RegistroCupomId"]
            : Id;
        // ...
    }
    // ...
}
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [13/01/2026 06:40] - Criação da Documentação Inicial

**Descrição**: Criação da documentação completa do PageModel `UpsertCupons.cshtml.cs`.

**Conteúdo Documentado**:
- Visão geral da funcionalidade
- Arquitetura e tecnologias utilizadas
- Todos os métodos principais:
  - `UpsertCuponsModel()` (construtor)
  - `SetViewModel()`
  - `OnGet(Guid id)`
  - `OnPostSubmit()`
  - `OnPostEdit(Guid Id)`
  - `OnPostSavePDF(IEnumerable<IFormFile> files)`
  - `TiraAcento(string frase)`
  - `RemoveAcento(char c)`
- Interconexões com Repository, Notificações e Sistema de Arquivos
- Validações frontend e backend
- Upload de arquivos (processo, segurança, riscos)
- Exemplos de uso (criar e editar cupom)
- Troubleshooting de problemas comuns

**Problemas Identificados**:
1. **Propriedade estática** `RegistroCupomId` pode causar race conditions em ambiente multi-thread
2. **Sem validação de tipo de arquivo** em `OnPostSavePDF()` - qualquer arquivo pode ser enviado
3. **Sem validação de tamanho** - arquivos grandes podem causar problemas
4. **Path Traversal** potencial - nome de arquivo não é sanitizado corretamente
5. **Sobrescrita de arquivos** sem aviso - arquivo com mesmo nome é substituído

**Recomendações de Melhoria**:
1. Substituir propriedade estática por `TempData` ou session state
2. Validar extensão de arquivo (apenas .pdf)
3. Definir tamanho máximo de upload (ex: 5 MB)
4. Gerar nomes únicos (GUID + extensão)
5. Validar conteúdo do arquivo (magic numbers)

**Arquivos Documentados**:
- `Pages/Abastecimento/UpsertCupons.cshtml.cs`

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.0

---

**Última atualização**: 13/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.0
