# Documentação: Viagens - ListaEventos

> **Última Atualização**: 25/01/2026
> **Versão Atual**: 0.5

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Frontend](#frontend)
4. [Endpoints API](#endpoints-api)
5. [Validações](#validações)
6. [Troubleshooting](#troubleshooting)

---

## Visão Geral

> **TODO**: Descrever o objetivo da página e as principais ações do usuário.

### Características Principais
- ✅ **TODO**

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/Viagens/ListaEventos.cshtml
├── Pages/Viagens/ListaEventos.cshtml.cs
```

### Informações de Roteamento

- **Módulo**: `Viagens`
- **Página**: `ListaEventos`
- **Rota (Razor Pages)**: `/<convenção Razor Pages>`

---

## Frontend

### Assets referenciados na página

- **CSS** (3):
  - `https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css`
  - `https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css`
  - `~/css/ftx-card-styled.css`
- **JS** (7):
  - `https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js`
  - `https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js`
  - `https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js`
  - `https://cdn.kendostatic.com/2022.1.412/js/kendo.aspnetmvc.min.js`
  - `https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js`
  - `js/cadastros/listaeventos.js`
  - `js/loading_script.js`

### Observações detectadas
- Contém `@section ScriptsBlock`.
- Contém `@section HeadBlock`.
- Possível uso de DataTables (detectado por string).

---

## Endpoints API

> **TODO**: Listar endpoints consumidos pela página e incluir trechos reais de código do Controller/Handler quando aplicável.

---

## Validações

> **TODO**: Listar validações do frontend e backend (com trechos reais do código).

---

## Troubleshooting

> **TODO**: Problemas comuns, sintomas, causa e solução.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [25/01/2026] - Implementação de ordenação dinâmica e estilização global DataTables

**Descrição**:
- Adicionada função `data()` no AJAX do DataTables para enviar parâmetros de ordenação (`orderColumn`, `orderDir`)
- Removido CSS duplicado de botões DataTables (movido para `frotix.css` global)
- Habilitada ordenação na coluna Status (removido `orderable: false`)
- Configuração de ordenação padrão mantida: coluna "Início" em ordem decrescente

**Arquivos Afetados**:
- `Pages/Viagens/ListaEventos.cshtml` (linhas 726-745, 792-793)
- `wwwroot/css/frotix.css` (novo: estilos globais DataTables)
- `Controllers/ViagemController.ListaEventos.cs` (suporte backend)

**Código AJAX (envio de parâmetros)**:
```javascript
data: function(d) {
    if (d.order && d.order.length > 0) {
        return {
            draw: d.draw,
            start: d.start,
            length: d.length,
            orderColumn: d.order[0].column,
            orderDir: d.order[0].dir
        };
    }
    return {
        draw: d.draw,
        start: d.start,
        length: d.length,
        orderColumn: 1,  // Padrão: coluna "Início"
        orderDir: 'desc'
    };
}
```

**Impacto**:
- Ordenação funcional em todas as colunas clicáveis
- Estilos de botões DataTables padronizados em todo o sistema (laranja #cc5200)
- Coluna Status agora ordenável

**Status**: ✅ **Concluído**

---

## [13/01/2026] - Desabilitar botão Excluir para eventos com viagens associadas

**Descrição**:
- Implementada lógica para desabilitar o botão "Apagar Evento" quando o evento possui viagens associadas
- O botão agora exibe uma tooltip informando quantas viagens estão associadas e que o evento não pode ser excluído
- O endpoint `GET /api/viagem/listaeventos` foi modificado para retornar a contagem de viagens por evento (`viagensCount`)

**Arquivos Afetados**:
- `Pages/Viagens/ListaEventos.cshtml` (render da coluna de ações, linhas 806-840)
- `Controllers/ViagemController.ListaEventos.cs` (retorno de `viagensCount`)

**Código Frontend (render)**:
```javascript
// Verifica se há viagens associadas ao evento
var temViagens = row.viagensCount > 0;
var btnApagarClasse = temViagens
    ? "btn btn-secondary text-white btn-icon-28 disabled"
    : "btn btn-vinho text-white btn-icon-28 btn-apagar";
var btnApagarTooltip = temViagens
    ? `Este evento possui ${row.viagensCount} viagem(ns) associada(s) e não pode ser excluído`
    : "Apagar Evento";
```

**Código Backend (ViagemController.ListaEventos.cs)**:
```csharp
var viagensDict = _context.Viagem
    .Where(v => v.EventoId != null && eventoIds.Contains(v.EventoId.Value))
    .AsNoTracking()
    .GroupBy(v => v.EventoId)
    .Select(g => new
    {
        EventoId = g.Key,
        CustoTotal = g.Sum(v => ...),
        ViagensCount = g.Count()
    })
    .ToDictionary(...);

// No retorno:
return new {
    ...
    viagensCount = viagensCount
};
```

**Impacto**: Proteção de integridade - eventos com viagens não podem ser excluídos acidentalmente

**Status**: ✅ **Concluído**

---

## [14/01/2026 00:00] - Padronização do ícone do header no Modal de Detalhamento

**Descrição**:
- Alterado o ícone do header do **Modal de Detalhamento de Custos** (`#modalDetalhes`)
- O ícone `fa-file-invoice-dollar` foi substituído por `fa-money-check-dollar` para manter consistência visual com o Modal de Custos do Evento

**Arquivos Afetados**:
- `Pages/Viagens/ListaEventos.cshtml` (linha 358)

**Impacto**: Visual - consistência de ícones entre modais relacionados

**Status**: ✅ **Concluído**

---

## [13/01/2026 23:15] - Correção de ícones nos botões Fechar dos modais

**Descrição**:
- Corrigido o ícone do botão "Fechar" no **Modal de Custos do Evento** (`#modalCusto`)
- Corrigido o ícone do botão "Fechar" no **Modal de Detalhamento de Custos** (`#modalDetalhes`)
- O ícone `fa-rotate-left` (voltar) foi substituído por `fa-circle-xmark` (X no círculo), conforme padrão FrotiX para botões de fechar modal

**Arquivos Afetados**:
- `Pages/Viagens/ListaEventos.cshtml` (linhas 341 e 456)

**Impacto**: Visual - padronização de ícones em modais

**Status**: ✅ **Concluído**

---

## [08/01/2026 18:24] - Criação automática da documentação (stub)

**Descrição**:
- Criado esqueleto de documentação automaticamente a partir da estrutura de arquivos e referências encontradas na página.
- **TODO**: Completar PARTE 1 com detalhes e trechos de código reais.

**Status**: ✅ **Gerado (pendente detalhamento)**
