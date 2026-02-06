# Documentacao: relatorio.js

> **Ultima Atualizacao**: 21/01/2026
> **Versao Atual**: 1.1

---

# PARTE 1: DOCUMENTACAO DA FUNCIONALIDADE

## Indice
1. [Visao Geral](#visao-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Logica de Negocio](#logica-de-negocio)
5. [Interconexoes](#interconexoes)
6. [Funcoes Principais](#funcoes-principais)
7. [Troubleshooting](#troubleshooting)

---

## Visao Geral

**Descricao**: Modulo JavaScript responsavel por gerenciar a exibicao de relatorios Telerik (Ficha de Vistoria) dentro do modal de visualizacao de viagens na pagina de Agenda.

### Caracteristicas Principais
- Gerencia overlay de loading com logo FrotiX piscando
- Controla inicializacao e destruicao do viewer Telerik
- Previne conflitos de carregamento simultaneo
- Recupera dados da viagem via API
- Sincroniza estado com outras partes do modal

### Objetivo
Permitir que o usuario visualize a Ficha de Vistoria de uma viagem diretamente dentro do modal de agendamento, sem precisar abrir uma nova pagina.

---

## Arquitetura

### Tecnologias Utilizadas
| Tecnologia | Versao | Uso |
|------------|--------|-----|
| jQuery | 3.x | Manipulacao DOM |
| Telerik ReportViewer | - | Visualizacao de relatorios |
| Kendo UI | - | Dependencia do Telerik |

### Padroes de Design
- IIFE (Immediately Invoked Function Expression) para encapsulamento
- Flags globais para controle de estado
- Promises para sincronizacao assincrona

---

## Estrutura de Arquivos

### Arquivo Principal
```
wwwroot/js/agendamento/components/relatorio.js
```

### Arquivos Relacionados
- `wwwroot/js/agendamento/main.js` - Chama funcoes de relatorio
- `wwwroot/js/agendamento/components/exibe-viagem.js` - Integra com exibicao de viagem
- `Pages/Agenda/Index.cshtml` - Pagina que contem o container do relatorio

---

## Logica de Negocio

### Configuracoes (CONFIG)
```javascript
const CONFIG = {
    CARD_ID: 'cardRelatorio',           // ID do card que contem o relatorio
    VIEWER_ID: 'reportViewerAgenda',    // ID do viewer Telerik
    CONTAINER_ID: 'ReportContainerAgenda', // Container do viewer
    HIDDEN_ID: 'txtViagemIdRelatorio',  // Campo hidden com ID da viagem
    SERVICE_URL: '/api/reports/',       // URL do servico de relatorios
    RECOVERY_URL: '/api/Agenda/RecuperaViagem', // URL para recuperar dados
    TIMEOUT: 20000,                     // Timeout de carregamento (20s)
    SHOW_DELAY: 500,                    // Delay antes de mostrar
    VIEWER_HEIGHT: '800px',             // Altura fixa do viewer
    CONTAINER_MIN_HEIGHT: '850px'       // Altura minima do container
};
```

### Flags Globais
```javascript
window.isReportViewerLoading = false;      // Carregamento em andamento
window.isReportViewerDestroying = false;   // Destruicao em andamento
window.reportViewerInitPromise = null;     // Promise de inicializacao
window.reportViewerDestroyPromise = null;  // Promise de destruicao
```

---

## Interconexoes

### Quem Chama Este Arquivo
- `main.js` -> Chama `carregarRelatorioViagem()` quando viagem eh exibida
- `main.js` -> Chama `limparRelatorio()` quando modal fecha
- `exibe-viagem.js` -> Chama funcoes para mostrar/esconder relatorio

### O Que Este Arquivo Chama
- `Alerta.TratamentoErroComLinha()` -> Para tratamento de erros
- `/api/reports/` -> API Telerik para carregar relatorios
- `/api/Agenda/RecuperaViagem` -> Para obter dados da viagem

### Fluxo de Dados
```
Usuario clica em viagem
    |
main.js -> exibirViagem()
    |
relatorio.js -> mostrarLoadingRelatorio()
    |
relatorio.js -> carregarRelatorioViagem(viagemId)
    |
API /api/Agenda/RecuperaViagem
    |
Telerik ReportViewer inicializa
    |
relatorio.js -> esconderLoadingRelatorio()
```

---

## Funcoes Principais

### `window.mostrarLoadingRelatorio()`
**Proposito**: Exibe overlay de loading com logo FrotiX piscando

**Comportamento**:
- Remove overlay anterior se existir
- Cria HTML com logo piscando e barra de progresso
- Adiciona ao body com z-index 999999
- Bloqueia clicks e teclas ESC

**Codigo**:
```javascript
window.mostrarLoadingRelatorio = function () {
    $('#modal-relatorio-loading-overlay').remove();
    const html = `<div id="modal-relatorio-loading-overlay" ...>`;
    $('body').append(html);
    // Bloqueia interacoes
    $('#modal-relatorio-loading-overlay').on('click keydown', e => {
        e.preventDefault();
        return false;
    });
};
```

### `window.esconderLoadingRelatorio()`
**Proposito**: Remove overlay de loading com animacao

**Comportamento**:
- Aguarda 1 segundo (para evitar flash)
- Aplica fadeOut de 300ms
- Remove elemento do DOM apos animacao

**Codigo**:
```javascript
window.esconderLoadingRelatorio = function () {
    setTimeout(function () {
        $('#modal-relatorio-loading-overlay').fadeOut(300, function () {
            $(this).remove();
        });
    }, 1000);
};
```

### `window.carregarRelatorioViagem(viagemId)`
**Proposito**: Carrega e exibe relatorio Telerik para uma viagem

**Parametros**:
- `viagemId` (string/GUID): ID da viagem

**Comportamento**:
1. Mostra overlay de loading
2. Valida dependencias (jQuery, Telerik, Kendo)
3. Aguarda destruicao de instancia anterior
4. Recupera dados da viagem via API
5. Inicializa viewer Telerik com parametros
6. Exibe card do relatorio
7. Esconde overlay de loading

**Tratamento de Erro**:
```javascript
} catch (error) {
    window.isReportViewerLoading = false;
    window.esconderLoadingRelatorio(); // CRITICO: Remove overlay
    Alerta.TratamentoErroComLinha("relatorio.js", "carregarRelatorioViagem", error);
}
```

### `window.limparRelatorio()`
**Proposito**: Destroi instancia do viewer e limpa containers

**Comportamento**:
1. Verifica se ja esta destruindo (evita chamadas duplicadas)
2. Cancela carregamento pendente
3. Destroi instancia Telerik (dispose)
4. Limpa HTML do container
5. Esconde card do relatorio

---

## Troubleshooting

### Problema: Modal travado com overlay de loading
**Sintoma**: Modal nao responde a clicks, cursor mostra simbolo de proibido

**Causa**: Overlay de loading nao foi removido apos erro no carregamento

**Solucao**: Verificar se `window.esconderLoadingRelatorio()` eh chamado em TODOS os catch blocks

**Codigo Relacionado**: Linha 1020-1041 do arquivo

### Problema: Relatorio nao carrega
**Sintoma**: Card do relatorio aparece vazio ou com erro

**Causa Possivel**:
- Dependencias Telerik nao carregadas
- ViagemId invalido
- Timeout de carregamento

**Diagnostico**: Verificar console para mensagens `[Relatorio]`

### Problema: Conflito de carregamento
**Sintoma**: Erro ao carregar relatorio quando outro ja esta carregando

**Solucao**: O sistema usa flags globais para prevenir isso. Verificar se flags estao sendo respeitadas.

---

# PARTE 2: LOG DE MODIFICACOES/CORRECOES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [21/01/2026] - Melhorias no Loading e Tratamento de Erros

**Descricao**: Duas melhorias importantes no modulo de relatorio:

1. **Aumento do delay do overlay**: Aumentado de 1s para 2s o tempo de espera antes de esconder o overlay de loading, dando mais tempo para a Ficha da Viagem carregar completamente.

2. **Tratamento de erro Kendo collapsible**: Adicionada verificacao se o ReportViewer esta totalmente inicializado antes de tentar destruir. Quando o modal e fechado antes da ficha carregar, o erro "Cannot read properties of undefined (reading 'collapsible')" agora e silenciado.

**Arquivos Afetados**:
- `wwwroot/js/agendamento/components/relatorio.js` (linhas 48-61, 1090-1135)

**Impacto**:
- Overlay de loading permanece visivel por mais tempo, evitando flash de conteudo vazio
- Nao gera mais erro JavaScript ao fechar modal durante carregamento

**Status**: Concluido

**Responsavel**: Claude Code

**Versao**: 1.1

---

## [20/01/2026] - Correcao de Overlay Travando Modal

**Descricao**: Adicionada chamada a `window.esconderLoadingRelatorio()` no catch block da funcao `carregarRelatorioViagem()` para garantir que o overlay de loading seja removido mesmo em caso de erro.

**Arquivos Afetados**:
- `wwwroot/js/agendamento/components/relatorio.js` (linha 1026)

**Impacto**: Evita que o modal fique completamente bloqueado quando ocorre erro ao carregar relatorio

**Status**: Concluido

**Responsavel**: Claude Code

**Versao**: 1.0

---

## Historico de Versoes

| Versao | Data | Descricao |
|--------|------|-----------|
| 1.1 | 21/01/2026 | Aumento delay overlay + tratamento erro Kendo |
| 1.0 | 20/01/2026 | Documentacao inicial + correcao overlay |

---

## Referencias

- [Documentacao main.js](./main.md)
- [Documentacao exibe-viagem.js](./exibe-viagem.md)
- [Documentacao validacao.js](./validacao.md)

---

**Ultima atualizacao**: 20/01/2026
**Autor**: Sistema FrotiX / Claude Code
**Versao**: 1.0
