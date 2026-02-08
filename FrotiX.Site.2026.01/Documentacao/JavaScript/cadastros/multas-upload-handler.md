# Documentação: multas-upload-handler.js

> **Última Atualização**: 22/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Padrão Module Pattern](#padrão-module-pattern)
4. [API Pública](#api-pública)
5. [Funções Internas](#funções-internas)
6. [Dependências](#dependências)

---

## Visão Geral

Este arquivo JavaScript implementa um **módulo centralizado de upload de PDFs** para o sistema de multas do FrotiX. Utiliza o padrão Module Pattern (IIFE) para encapsular a lógica e expor apenas a API necessária.

### Características Principais

- ✅ Módulo auto-contido (IIFE) exposto globalmente como `MultasUploadHandler`
- ✅ Suporte a múltiplos tipos de upload: Autuação, Penalidade, Comprovante, EDoc, Outros
- ✅ Integração com Syncfusion Uploader para seleção de arquivos
- ✅ Visualização imediata de PDFs no Syncfusion PDF Viewer
- ✅ Factory pattern para criação de handlers de sucesso

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── wwwroot/js/cadastros/multas-upload-handler.js   ← Este arquivo (módulo)
├── Pages/Multa/UploadPDF.cshtml                    ← Página de upload
├── Pages/Multa/UpsertAutuacao.cshtml               ← Usa o módulo
├── Pages/Multa/UpsertPenalidade.cshtml             ← Usa o módulo
```

### Padrão de Exposição Global

```javascript
window.MultasUploadHandler = (function () {
  // Código privado
  return {
    // API pública
  };
})();
```

---

## Padrão Module Pattern

O arquivo utiliza **IIFE (Immediately Invoked Function Expression)** para:

1. **Encapsulamento**: Variáveis e funções internas não poluem o escopo global
2. **API Controlada**: Apenas métodos públicos são expostos via `window.MultasUploadHandler`
3. **Estado Privado**: Referências ao PDF Viewer são mantidas internamente
4. **Factory Pattern**: `createSuccessHandler()` cria handlers específicos por tipo

---

## API Pública

### onUploadSelected(args)

Callback para quando um arquivo é selecionado no Uploader.

- Valida extensão do arquivo (.pdf)
- Prepara preview local se possível

### onUploadFailure(args)

Callback para falha no upload.

- Exibe Alerta.Erro com detalhes
- Limpa estado do uploader

### onUploadSuccess_Autuacao(args)

Handler de sucesso para upload de PDF de Autuação.

### onUploadSuccess_Penalidade(args)

Handler de sucesso para upload de PDF de Penalidade.

### onUploadSuccess_Comprovante(args)

Handler de sucesso para upload de Comprovante de Pagamento.

### onUploadSuccess_EDoc(args)

Handler de sucesso para upload de E-Doc.

### onUploadSuccess_OutrosDocumentos(args)

Handler de sucesso para upload de Outros Documentos.

---

## Funções Internas

### getViewer()

Obtém referência ao componente Syncfusion PDF Viewer.

**Retorno:**

- Instância do PDF Viewer ou null

### loadPdfInViewer(base64Content)

Carrega PDF codificado em base64 no viewer.

**Parâmetros:**

- `base64Content` (string): Conteúdo do PDF em base64

### extractPayload(args)

Extrai dados do evento de upload bem-sucedido.

**Parâmetros:**

- `args` (object): Argumentos do evento de upload

**Retorno:**

- Objeto com `fileId`, `fileName`, `base64Content`

### createSuccessHandler(tipoDocumento)

Factory que cria handler de sucesso para tipo específico.

**Parâmetros:**

- `tipoDocumento` (string): Tipo do documento ("Autuacao", "Penalidade", etc.)

**Retorno:**

- Função handler configurada

---

## Dependências

| Biblioteca                | Versão | Uso                     |
| ------------------------- | ------ | ----------------------- |
| jQuery                    | 3.x    | Manipulação DOM         |
| Syncfusion EJ2 Uploader   | 29.x   | Componente de upload    |
| Syncfusion EJ2 PDF Viewer | 29.x   | Visualização de PDFs    |
| SweetAlert2               | -      | Via Alerta.\* (interop) |

---

## Padrões FrotiX Aplicados

- ✅ Try-catch em todas as funções com `Alerta.TratamentoErroComLinha`
- ✅ Alertas via biblioteca interna (não `alert()` nativo)
- ✅ Module Pattern para código organizado
- ✅ Factory Pattern para reutilização de lógica

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [22/01/2026] - Documentação JSDoc Completa

**Descrição**: Adicionada documentação JSDoc padrão FrotiX completa ao módulo.

**Arquivos Afetados**:

- wwwroot/js/cadastros/multas-upload-handler.js

**Mudanças**:

- ✅ Cabeçalho JSDoc com box visual FrotiX e metadados
- ✅ Documentação do padrão Module Pattern (IIFE)
- ✅ Comentários inline em funções internas e públicas
- ✅ Documentação da API pública exposta

**Motivo**: Conformidade com padrão de documentação FrotiX

**Impacto**: Apenas documentação, sem alteração funcional

**Status**: ✅ **Concluído**

**Responsável**: Claude Opus 4.5

**Versão**: 1.0
