# Motor de Viagens e Operação Logística

A **Viagem** é a unidade de valor do FrotiX. É aqui que todos os dados (Veículos, Motoristas, Custos, Combustível) se encontram para gerar a operação logística. O ViagemController é disparado como o controlador mais complexo do sistema, lidando com milhares de registros e cálculos financeiros em tempo real.

## 🚀 Inteligência de Operação

O sistema de viagens não é apenas um registro de logs, mas um motor de inteligência que calcula o TCO (_Total Cost of Ownership_) de cada deslocamento.

### Funcionalidades Críticas:

1.  **Cálculo de Custo em Batch:** O FrotiX possui um algoritmo otimizado para recalcular os custos de milhares de viagens em segundos. Ele utiliza um cache em memória para evitar consultas repetitivas ao banco de dados sobre preços de combustíveis e salários.
2.  **Ficha de Vistoria Digital:** Acoplado à viagem, o sistema gerencia a imagem digitalizada da vistoria (yte[]), garantindo que qualquer avaria ou conformidade seja documentada visualmente e vinculada ao ID da viagem.
3.  **Filtros de Alta Performance:** Utiliza expressões Lambda/Linq dinâmicas (iagemsFilters) para permitir consultas simultâneas por data, placa, motorista e status sem perda de performance.

## 🛠 Snippets de Lógica Principal

### Otimização de Cálculo de Massa (Cache Singleton)

Para evitar que o cálculo de custo de 10.000 viagens faça 50.000 conexões ao banco, utilizamos o padrão de Cache de Dados Compartilhados:

`csharp
private class DadosCalculoCache {
public Dictionary<Guid, double> ValoresCombustivel { get; set; } = new Dictionary<Guid, double>();
public Dictionary<Guid, MotoristaInfo> InfoMotoristas { get; set; } = new Dictionary<Guid, MotoristaInfo>();
// ... outros dados carregados UMA VEZ
}

// No Batch, carregamos tudo antes do Loop
var cache = await CarregarDadosCalculoCache();
foreach (var viagem in batch) {
CalcularCustosViagem(viagem, cache); // Cálculo puramente em memória!
}
`

### Gestão Visual (Ficha de Vistoria)

O controlador lida com o upload e conversão de Base64 para garantir que a interface (Index.cshtml) possa mostrar a imagem sem precisar salvar em disco físico, mantendo tudo no banco para segurança e portabilidade.

## 📝 Notas de Implementação

- **Status "Realizada":** Apenas viagens marcadas como Realizadas entram no motor de cálculo de custos. Isso evita distorções financeiras em agendamentos futuros ou cancelados.
- **Integração com Eventos:** O ViagemEventoController permite anexar ocorrências (quebras, acidentes) diretamente à viagem, afetando os indicadores de disponibilidade do DashboardEventos.
- **Precisão de KM:** O sistema valida o KmInicial e KmFinal. Se a diferença for negativa ou excessiva (fora do padrão do veículo), um alerta é gerado no módulo de Auditoria.

---

_Documentação gerada para a Solução FrotiX 2026. Este controlador é central para a operação do sistema._

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [22/01/2026 20:30] - Ajuste de tratamento de erros e cabeçalhos

**Descrição**: Atualização dos cabeçalhos de documentação e inclusão de try-catch nas rotinas da página de Viagens.

**Mudanças**:

- Atualizado o cabeçalho de documentação em `Pages/Viagens/Index.cshtml` e `wwwroot/js/cadastros/ViagemIndex.js`.
- Inclusão de `try-catch` em `OnGet` e `OnPost` com `Alerta.TratamentoErroComLinha`.

**Arquivos Afetados**:

- `Pages/Viagens/Index.cshtml`
- `wwwroot/js/cadastros/ViagemIndex.js`

**Impacto**:

- Robustez na inicialização da página e alinhamento das informações de documentação.

**Status**: ✅ **Concluído**

**Versão**: 3.2

---

## [22/01/2026 20:15] - Refatoração completa do Modal de Finalização

**Descrição**: Modal de Finalizar Viagem completamente refatorado com layout moderno em cards, inclusão de todos os itens devolvidos e correção do botão de Ficha de Vistoria.

**Mudanças**:

- Modal agora usa layout em cards coloridos (Dados Iniciais, Dados Finais, Itens Devolvidos, Observações, Ocorrências).
- Seção "Controle de Itens Devolvidos" com 6 itens: Documento, Cartão, Cabo, Arla, Cinta, Tablet.
- Switches visuais maiores (3rem x 1.5rem) para melhor usabilidade.
- Cards com bordas coloridas por seção (cinza, verde, azul, amarelo, vermelho).
- Botão de Ficha de Vistoria agora trata corretamente `null`, `false` e `0` como "sem ficha".
- Tooltips Syncfusion funcionam em todos os botões, inclusive desabilitados.

**Arquivos Afetados**:

- `wwwroot/js/cadastros/ViagemIndex.js`
- `Pages/Viagens/Index.cshtml`

**Impacto**:

- Modal mais organizado e visualmente moderno.
- Controle completo de itens devolvidos pelo motorista.
- Feedback claro ao usuário sobre estado de cada botão.

**Status**: ✅ **Concluído**

**Versão**: 3.1

---

## [22/01/2026 19:45] - Correção de regressões visuais

**Descrição**: Restauradas funcionalidades visuais que haviam sido perdidas: badges de Status com ícones duotone, tooltips nos botões de ação, cor do botão de Ficha de Vistoria.

**Mudanças**:

- Badges de Status voltam a usar `fa-duotone` em vez de `fa-solid`.
- Tooltip do botão Finalizar restaurado com `data-ejtip`.
- Botão de Ficha de Vistoria recebe cor verde (#228B22) via CSS `.btn-foto`.
- Tooltips explicativas em botões desabilitados mantidas via Syncfusion.
- Modal de Finalização preservado (layout simples com rows e separadores).

**Arquivos Afetados**:

- `wwwroot/js/cadastros/ViagemIndex.js`
- `Pages/Viagens/Index.cshtml`

**Impacto**:

- Visual consistente com o padrão FrotiX.
- Usuário recebe feedback visual adequado em todos os estados de botões.

**Status**: ✅ **Concluído**

**Versão**: 3.0

---

## [22/01/2026 17:30] - Alteração de Label no Modal de Finalização

**Descrição**: Alterado label "Status Documento" para apenas "Documento" no modal de finalização de viagens para melhorar organização visual e permitir que todos os itens caibam em uma linha.

**Problema Identificado**:

- Label "Status Documento" era muito longa, causando quebra de linha indesejada
- Documento e Cartão de Abastecimento também são "Itens Devolvidos", assim como Cabo, Arla, Cinta e Tablet
- Todos os itens devem ter nomes concisos para manter consistência visual em uma única linha

**Solução Implementada**:

Alterado label de `Status Documento` para apenas `Documento` (linha 999):

```html
<label class="ftx-label">
  <i class="fa-duotone fa-file-check"></i>
  Documento
</label>
```

**Arquivos Afetados**:

- `Pages/Viagens/Index.cshtml` (linha 999)

**Impacto**:

- Melhor organização visual no modal de finalização
- Todos os itens de devolução (Documento, Cartão Abastecimento, Cabo, Arla, Cinta, Tablet) agora cabem em uma linha
- Consistência com o padrão visual dos outros itens

**Observação**: Esta alteração foi feita em conjunto com o ajuste de padding dos cards internos do modal (ver documentação de `modal-viagens-consolidado.css` v1.6).

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 2.4

---

## [22/01/2026 14:41] - FIX: Correção de Tooltips em Botões Desabilitados e Listas Completas

**Descrição**: Correção para garantir que tooltips Syncfusion funcionem corretamente em botões desabilitados e remoção de filtros de Status das listas de Veículo, Motorista e Evento.

**Motivação**:

- Usuários não conseguiam ver explicações quando botões estavam desabilitados
- Listas de dropdowns precisam incluir registros inativos para consultas históricas

**Arquivos Afetados**:

- Pages/Viagens/Index.cshtml (estilos CSS)
- wwwroot/js/cadastros/ViagemIndex.js (drawCallback)

**Mudanças**:

1. **CSS para Tooltips em Botões Desabilitados** (Index.cshtml):

```css
/* Garante que tooltips funcionem em botões desabilitados */
.btn.disabled {
  pointer-events: auto !important; /* Permite hover */
  cursor: not-allowed !important; /* Indica não clicável */
}

/* Previne cliques mas mantém hover */
.btn.disabled:active,
.btn.disabled:focus {
  pointer-events: none !important;
}

/* Z-index para tooltips */
.ftx-tooltip-viagens.e-tooltip-wrap {
  z-index: 10000 !important;
}
```

2. **DrawCallback Melhorado** (ViagemIndex.js, linhas 2002-2022):

```javascript
drawCallback: function (settings) {
    // [UI] Reinicializa tooltips Syncfusion
    var tooltipElements = document.querySelectorAll('[data-ejtip]');
    tooltipElements.forEach(function (element) {
        if (!element.ej2_instances || element.ej2_instances.length === 0) {
            new ej.popups.Tooltip({
                content: element.getAttribute('data-ejtip'),
                position: 'TopCenter',
                opensOn: 'Hover',
                cssClass: 'ftx-tooltip-viagens'
            }).appendTo(element);
        }
    });
}
```

**Solução Técnica**:

- `pointer-events: auto` permite hover mesmo em elementos desabilitados
- `cursor: not-allowed` mantém feedback visual de desabilitado
- `pointer-events: none` em `:active` e `:focus` previne cliques
- Tooltips Syncfusion com `opensOn: 'Hover'` para responsividade

**Problema Resolvido**:

- Tooltips agora aparecem em todos os botões, mesmo desabilitados
- Mensagens explicativas ("Disponível após finalizar a viagem", "Somente viagens Abertas podem ser finalizadas") agora visíveis

**Impacto**: Positivo - Melhor UX sem quebrar comportamento de botões desabilitados

**Status**: ✅ **Concluído**

**Responsável**: Claude Code + Usuário

**Versão**: 2.3

---

## [21/01/2026] - Correção: Modal de Ficha de Vistoria e Tooltips em Botões Desabilitados

**Descrição**: Criação do modal `#modalFicha` que estava faltando e adição de tooltips explicativos em botões desabilitados do DataTable

**Arquivos Afetados**:

- Pages/Viagens/Index.cshtml (adicionado modal completo)
- wwwroot/js/cadastros/ViagemIndex.js (adicionados tooltips dinâmicos)

**Mudanças**:

1. **Modal de Ficha de Vistoria**:
   - Criado modal `#modalFicha` completo com estrutura para upload e visualização de fichas
   - Incluídos elementos: `#hiddenViagemId`, `#txtFile`, `#imgFichaViewer`, `#noImageContainer`, `#uploadContainer`, `#loadingSpinner`, `#imageContainer`, `#btnSalvarFicha`, `#btnAlterarFicha`
   - Modal com backdrop estático e suporte a arquivos de imagem e PDF

2. **Tooltips Explicativos**:
   - Botão "Finalizar": Tooltip dinâmico que explica quando está desabilitado ("Somente viagens Abertas podem ser finalizadas")
   - Botão "Cancelar": Tooltip dinâmico ("Somente viagens Abertas podem ser canceladas")
   - Botão "Ficha de Vistoria": Tooltip dinâmico ("Disponível após finalizar a viagem" quando aberta)
   - Botão "Custos da Viagem": Tooltip dinâmico ("Disponível após finalizar a viagem" quando aberta)
   - Botão "Ocorrências da Viagem": Tooltip dinâmico ("Disponível após finalizar a viagem" quando aberta)

**Problema Resolvido**:

- Botão "Exibir Ficha da Viagem" não estava abrindo o modal (modal não existia no HTML)
- Botões desabilitados não informavam ao usuário o motivo da desabilitação

**Impacto**:

- Funcionalidade de ficha de vistoria agora está operacional
- Melhor UX com tooltips informativos
- Usuários entendem por que alguns botões estão desabilitados

**Status**: ✅ **Concluído**

**Responsável**: Sistema

**Versão**: Correção de bug

---

## [21/01/2026] - Padronização de Nomenclatura

**Descrição**: Renomeada coluna "Ação" para "Ações" no cabeçalho do DataTable para padronização do sistema

**Arquivos Afetados**:

- Pages/Viagens/Index.cshtml
- wwwroot/js/cadastros/ViagemIndex.js (comentário da coluna)

**Impacto**: Alteração cosmética, sem impacto funcional

**Status**: ✅ **Concluído**

**Responsável**: Sistema

**Versão**: Atual

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:

- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:

- âŒ **ANTES**: \_unitOfWork.Entity.AsTracking().Get(id) ou \_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: \_unitOfWork.Entity.GetWithTracking(id) ou \_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

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
