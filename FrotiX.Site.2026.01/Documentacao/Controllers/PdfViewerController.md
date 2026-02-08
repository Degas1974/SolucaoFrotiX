# Documentação: PdfViewerController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `PdfViewerController` fornece endpoints para visualização de PDFs usando Syncfusion PDF Viewer, incluindo carregamento, bookmarks e renderização de páginas.

**Principais características:**

✅ **Visualização PDF**: Carregamento e renderização de PDFs  
✅ **Bookmarks**: Navegação por bookmarks  
✅ **Syncfusion**: Integração com Syncfusion PDF Viewer  
✅ **Suporte**: Arquivos do sistema de arquivos ou Base64

---

## Endpoints API

### POST `/api/PdfViewer/Load`

**Descrição**: Carrega PDF para visualização

**Request Body**: `Dictionary<string, string>` com:
- `document` - Caminho do arquivo ou Base64
- `isFileName` - Boolean indicando se é caminho de arquivo

**Response**: JSON com dados do PDF para Syncfusion PDF Viewer

---

### POST `/api/PdfViewer/Bookmarks`

**Descrição**: Obtém bookmarks do PDF

---

### POST `/api/PdfViewer/RenderPdfPages`

**Descrição**: Renderiza páginas específicas do PDF

---

## Interconexões

### Quem Chama Este Controller

- **Syncfusion PDF Viewer**: Componente de visualização
- **Pages**: Páginas que exibem PDFs

### O Que Este Controller Chama

- **Syncfusion PDF Renderer**: Biblioteca de renderização

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do PdfViewerController

**Arquivos Afetados**:
- `Controllers/PdfViewerController.cs`
- `Controllers/PdfViewerCNHController.cs`

**Impacto**: Documentação de referência para visualização de PDFs

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
