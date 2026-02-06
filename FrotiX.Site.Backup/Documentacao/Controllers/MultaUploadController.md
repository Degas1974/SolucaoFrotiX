# Documentação: MultaUploadController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `MultaUploadController` gerencia upload e remoção de arquivos PDF de multas usando Syncfusion EJ2 Uploader.

**Principais características:**

✅ **Upload Múltiplo**: Suporte a upload de múltiplos arquivos PDF  
✅ **Validação**: Validação de extensão (apenas PDF)  
✅ **Normalização**: Normalização de nomes de arquivos (remove acentos)  
✅ **Timestamp**: Adiciona timestamp para evitar conflitos  
✅ **Remoção**: Remove arquivos por nome ou via UploadFiles

---

## Endpoints API

### POST `/api/MultaUpload/Save`

**Descrição**: Salva arquivos PDF de multas

**Request**: `multipart/form-data` com `UploadFiles` (IList<IFormFile>)

**Validações**:
- Apenas arquivos `.pdf` são aceitos
- Normaliza nome do arquivo (remove acentos)
- Adiciona timestamp para evitar conflitos

**Response**: Lista de arquivos com status de sucesso/falha

---

### POST `/api/MultaUpload/Remove`

**Descrição**: Remove arquivos PDF de multas

**Request**: `multipart/form-data` com `UploadFiles` ou `fileName` via form data

**Lógica**: Remove arquivo de `wwwroot/DadosEditaveis/Multas/`

---

### GET `/api/MultaUpload/GetFileList`

**Descrição**: Lista todos os arquivos PDF disponíveis na pasta de multas

**Response**: Lista com nome, tamanho, tipo e data de modificação

---

## Interconexões

### Quem Chama Este Controller

- **Syncfusion EJ2 Uploader**: Componente de upload
- **Pages**: `Pages/Multa/*.cshtml` - Páginas de gestão de multas

### O Que Este Controller Chama

- **`Servicos.TiraAcento()`**: Normalização de nomes
- **`IWebHostEnvironment`**: Caminho do diretório web

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do MultaUploadController

**Arquivos Afetados**:
- `Controllers/MultaUploadController.cs`

**Impacto**: Documentação de referência para upload de multas

**Status**: ✅ **Concluído**

**Versão**: 2.0

---

**Última atualização**: 08/01/2026  
**Autor**: Sistema FrotiX  
**Versão**: 2.0


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
