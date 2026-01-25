# Documentação: Abastecimento - DashboardAbastecimento

> **Última Atualização**: 17/01/2026
> **Versão Atual**: 0.4

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
├── Pages/Abastecimento/DashboardAbastecimento.cshtml
├── Pages/Abastecimento/DashboardAbastecimento.cshtml.cs
```

### Informações de Roteamento

- **Módulo**: `Abastecimento`
- **Página**: `DashboardAbastecimento`
- **Rota (Razor Pages)**: `/<convenção Razor Pages>`
- **@model**: `FrotiX.Pages.Abastecimento.DashboardAbastecimentoModel`

---

## Frontend

### Assets referenciados na página

- **CSS** (0):
- **JS** (9):
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-base/dist/global/ej2-base.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-charts/dist/global/ej2-charts.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-compression/dist/global/ej2-compression.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-data/dist/global/ej2-data.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-file-utils/dist/global/ej2-file-utils.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-heatmap/dist/global/ej2-heatmap.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-pdf-export/dist/global/ej2-pdf-export.min.js`
  - `https://cdn.syncfusion.com/ej2/23.1.36/ej2-svg-base/dist/global/ej2-svg-base.min.js`
  - `~/js/dashboards/dashboard-abastecimento.js`

### Observações detectadas

- Contém `@section ScriptsBlock`.
- Contém `@section HeadBlock`.

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

## [17/01/2026] - Toast Amarelo para Veículo Sem Abastecimento no Período

**Descrição**:
Implementado toast amarelo que é exibido quando o usuário muda o **ano** ou **mês** na aba "Consumo por Veículo" e a placa que estava selecionada não possui abastecimentos no novo período.

**Alterações no JavaScript** (`dashboard-abastecimento.js`):

- Evento `filtroAnoVeiculo.change` agora guarda a placa selecionada (via Select2 API) antes de limpar e carrega dados do novo ano
- Evento `filtroMesVeiculo.change` também guarda a placa selecionada antes de limpar
- Função `carregarDadosVeiculo` recebe novos parâmetros opcionais `placaAnterior` e `textoPlacaAnterior`
- Função `executarCarregamentoVeiculo` recebe os mesmos parâmetros para verificar se a placa anterior ainda está disponível
- Verificação feita comparando a placa anterior com a lista `data.placasDisponiveis` retornada pela API
- Mensagem do toast padronizada: "Nenhum Abastecimento no Período para o Veículo [PLACA]"
- Duração do toast alterada de 6s para 4s
- Corrigido escopo da variável `mensagensToast` (movida para fora do `try` interno)

**Fluxo**:

1. Usuário seleciona um veículo na lista de Placas
2. Usuário muda o ano ou mês no filtro
3. Sistema guarda a placa selecionada (ID e texto) via Select2 API
4. Sistema limpa lista de placas e carrega dados do novo período
5. API retorna novas placas disponíveis para o período
6. Se a placa anterior não estiver na nova lista, exibe toast amarelo (4 segundos)

**Arquivos Afetados**:

- `wwwroot/js/dashboards/dashboard-abastecimento.js`

**Status**: ✅ **Concluído**

---

## [17/01/2026] - Ajustes nos Filtros de Consumo por Veículo

**Descrição**:
Aprimorada a experiência de filtros da aba "Consumo por Veículo" com manutenção automática da placa selecionada quando ela permanece válida.

**Alterações no Frontend**:

- Reorganização dos controles: Período Fixo + Período Personalizado na primeira linha; Placa na segunda linha.
- A lista de placas é recarregada sempre que o período (Ano/Mês ou Período Personalizado) muda.
- Alteração das datas do período personalizado atualiza a lista automaticamente quando o intervalo está completo.
- Quando a placa selecionada permanece disponível após o filtro, ela é mantida e os dados são recarregados.
- Quando a placa sai da lista, exibe toast amarelo informando ausência de abastecimentos no período.
- Remoção dos Períodos Rápidos para simplificar a seleção de datas.
- Lista de placas exibe Placa + Marca/Modelo conforme cadastro do veículo.
- Toast amarelo agora é exibido após o overlay e fica mais tempo em tela.
- Label de período não mantém a placa quando não há seleção ativa.
- Filtros são executados automaticamente ao selecionar mês ou completar datas válidas no período personalizado.
- Data inicial maior que data final limpa o campo final.
- Ao preencher apenas uma das datas do período personalizado, a lista de placas é esvaziada.
- Toast amarelo padrão agora permanece 6 segundos.
- Toast de mês sem dados exibido após o loading (correção de escopo).

**Arquivos Afetados**:

- `Pages/Abastecimento/DashboardAbastecimento.cshtml`
- `wwwroot/js/dashboards/dashboard-abastecimento.js`
- `Controllers/AbastecimentoController.DashboardAPI.cs`

**Status**: ✅ **Concluído**

---

## [15/01/2026] - Melhorias na Aba Consumo por Veículo

**Descrição**:
Implementadas diversas melhorias na aba "Consumo por Veículo" do Dashboard de Abastecimentos:

**Alterações no Frontend (DashboardAbastecimento.cshtml e dashboard-abastecimento.js)**:

- Adicionados gráficos vazios com mensagem "Escolha um Veículo para visualizar os Dados" quando nenhum veículo está selecionado
- Removido filtro de "Modelo Veículo" para simplificação da interface
- Seleção de Placa não dispara mais busca automática - apenas o botão "Filtrar" executa a pesquisa
- Corrigido bug que mostrava GUID ao invés do texto da placa no card de Período/Placa
- Cor da barra do veículo selecionado alterada de azul (#2563eb) para laranja FrotiX (#ff6b35)
- Estrela indicadora (★) movida para o final da barra no gráfico de ranking
- Gráfico de Valor Total Mensal agora exibe todos os 12 meses do ano, destacando o mês selecionado em laranja
- Adicionado tooltip "Selecione os Parâmetros para a Pesquisa" no botão Filtrar
- Corrigido posicionamento do ícone/label da Placa (agora fica em cima do campo Select2)
- Adicionado CSS `.filtro-placa-container` para garantir layout correto com Select2

**Alterações no Backend (AbastecimentoController.DashboardAPI.cs)**:

- Endpoint DashboardVeiculo agora retorna TODOS os meses do ano para os gráficos
- Totais são calculados apenas do período filtrado, mas gráficos mostram ano completo
- Adicionado campo `mesSelecionado` na resposta para frontend destacar mês correto

**Arquivos Afetados**:

- `Pages/Abastecimento/DashboardAbastecimento.cshtml` (CSS e HTML)
- `wwwroot/js/dashboards/dashboard-abastecimento.js` (lógica de renderização)
- `Controllers/AbastecimentoController.DashboardAPI.cs` (dados do endpoint)
- `wwwroot/js/pdf-export-profissional.js` (suporte a orientação forçada)

**Status**: ✅ **Concluído**

---

## [08/01/2026 18:24] - Criação automática da documentação (stub)

**Descrição**:

- Criado esqueleto de documentação automaticamente a partir da estrutura de arquivos e referências encontradas na página.
- **TODO**: Completar PARTE 1 com detalhes e trechos de código reais.

**Status**: ✅ **Gerado (pendente detalhamento)**


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
