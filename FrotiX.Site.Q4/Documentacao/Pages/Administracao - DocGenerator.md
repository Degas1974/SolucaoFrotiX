# Documentação: Administração - DocGenerator

> **Última Atualização**: 13/01/2026
> **Versão Atual**: 0.1

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

O **DocGenerator** é uma ferramenta administrativa do FrotiX que automatiza a geração de documentação técnica do sistema. Ele permite selecionar arquivos/pastas do projeto e gerar documentação em formato Markdown.

### Características Principais
- ✅ **Seleção Visual**: Árvore hierárquica (TreeView) do projeto para seleção de arquivos
- ✅ **Geração Automatizada**: Cria documentação MD baseada em estrutura de código
- ✅ **Log em Tempo Real**: Acompanhamento do progresso via SignalR
- ✅ **Customização**: Opções para incluir/excluir tipos de arquivos e pastas
- ✅ **Interface Moderna**: UI com cards, loading overlays e feedback visual

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/Administracao/
│   ├── DocGenerator.cshtml           ← UI da ferramenta
│   └── DocGenerator.cshtml.cs        ← PageModel
├── Controllers/Api/
│   └── DocGeneratorController.cs     ← API de geração
├── Hubs/
│   └── DocGenerationHub.cs           ← SignalR Hub
└── Services/DocGenerator/
    ├── DocGeneratorService.cs        ← Lógica principal
    └── [outros serviços]
```

### Tecnologias Utilizadas

| Tecnologia | Uso |
|------------|-----|
| ASP.NET Core Razor Pages | Frontend |
| SignalR | Comunicação em tempo real |
| Bootstrap 5 | UI/Layout |
| FontAwesome Duotone | Ícones |
| JavaScript/jQuery | Interatividade |

---

## Frontend

### Componentes Principais

1. **TreeView de Arquivos**: Árvore hierárquica para seleção de arquivos/pastas
2. **Painel de Opções**: Configurações de geração (incluir/excluir tipos)
3. **Log de Execução**: Console em tempo real com mensagens do processo
4. **Botões de Ação**:
   - "Selecionar Tudo" (`btn-outline-primary`)
   - "Limpar" (`btn-vinho`) - deseleciona todos
   - "Atualizar" (`btn-outline-info`) - recarrega árvore
   - "Gerar Documentação" (`btn-verde`) - inicia processo
   - "Limpar Log" (`btn-vinho`) - limpa console

### Loading Overlay

Modal de espera com logo FrotiX pulsante durante processamento:
```html
<div id="loadingOverlay" class="ftx-spin-overlay">
    <div class="ftx-spin-box">
        <img src="/images/logo_gota_frotix_transparente.png" class="ftx-loading-logo" />
        <div class="ftx-loading-text">Gerando documentação...</div>
    </div>
</div>
```

---

## Endpoints API

### POST `/api/DocGenerator/GenerateDocs`

**Descrição**: Inicia processo de geração de documentação

**Request Body**:
```json
{
  "selectedFiles": ["path/to/file1.cs", "path/to/file2.cshtml"],
  "options": {
    "includeComments": true,
    "generateIndex": true
  }
}
```

**Response**:
```json
{
  "success": true,
  "message": "Documentação gerada com sucesso",
  "filesProcessed": 42
}
```

---

## Validações

### Frontend
- **Validação de Seleção**: Ao menos 1 arquivo deve ser selecionado antes de gerar
- **Validação de Caminho**: Paths devem ser válidos e dentro do projeto

### Backend
- **Autorização**: Apenas usuários admin podem acessar
- **Validação de Arquivos**: Verifica se arquivos existem e são legíveis
- **Sanitização**: Remove paths absolutos perigosos

---

## Troubleshooting

### Problema: Geração não inicia

**Sintoma**: Botão "Gerar Documentação" não responde

**Causas Possíveis**:
- Nenhum arquivo selecionado
- SignalR desconectado
- Erro de permissão no servidor

**Solução**:
1. Verificar se há arquivos selecionados na árvore
2. Verificar console do navegador (F12) para erros JavaScript
3. Verificar logs do servidor para erros de permissão

---

### Problema: Log não atualiza

**Sintoma**: Log de execução não mostra mensagens em tempo real

**Causas Possíveis**:
- Conexão SignalR perdida
- Hub não configurado corretamente

**Solução**:
1. Recarregar a página
2. Verificar console para erros de conexão SignalR
3. Verificar configuração de Hub em `Startup.cs`

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [13/01/2026 18:30] - Fase 4: Padronização btn-outline-secondary → btn-vinho

**Descrição**: Substituída classe Bootstrap outline genérica `btn-outline-secondary` por `btn-vinho` (padrão FrotiX sólido) em 2 botões de ação limpar.

**Problema Identificado**:
- Uso de classe Bootstrap outline genérica `btn-outline-secondary` em botões de limpar
- Inconsistência com padrão FrotiX que define `btn-vinho` para ações de cancelar/limpar
- Botões outline não seguiam visual consistente do sistema

**Solução Implementada**:
- Botão "Limpar" seleções (linha 494): mudado de `btn-outline-secondary` para `btn-vinho`
- Botão "Limpar" log (linha 616): mudado de `btn-outline-secondary` para `btn-vinho`
- Ambos são semanticamente ações de "cancelar/resetar" estado
- Alinhamento com diretrizes FrotiX: ações de cancelar/limpar usam `btn-vinho`
- Consistência com Fases 1, 2, 3 e restante da Fase 4 do projeto de padronização

**Arquivos Afetados**:
- Pages/Administracao/DocGenerator.cshtml (linhas 494, 616)

**Impacto**:
- ✅ Botões mantêm cor vinho consistente ao pressionar
- ✅ Semântica visual correta (limpar = cancelar/resetar)
- ✅ Alinhamento com padrão visual FrotiX
- ✅ Consistência em todo o sistema
- ✅ 2 botões padronizados

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 0.1

---

## [13/01/2026 18:30] - Criação da documentação

**Descrição**: Criada documentação inicial do DocGenerator.

**Status**: ✅ **Concluído**

**Versão**: 0.1
