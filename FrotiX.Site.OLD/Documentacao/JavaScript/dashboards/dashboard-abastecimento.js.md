# Documentação: dashboard-abastecimento.js

> **Última Atualização**: 17/01/2026
> **Versão Atual**: 1.0

---

## PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Funções Principais](#funções-principais)
5. [Filtros e Eventos](#filtros-e-eventos)
6. [Interconexões](#interconexões)

---

## Visão Geral

Script JavaScript responsável pelo Dashboard de Abastecimentos do sistema FrotiX. Gerencia três abas principais:
- **Consumo Geral**: Visão consolidada de todos os abastecimentos
- **Consumo Mensal**: Análise mês a mês
- **Consumo por Veículo**: Análise detalhada por veículo específico

### Características Principais

- ✅ Gráficos interativos com Syncfusion EJ2 Charts
- ✅ Mapa de calor (heatmap) para padrões de abastecimento
- ✅ Filtros por ano, mês e período personalizado
- ✅ Exportação para PDF com layout profissional
- ✅ Toast amarelo para feedback ao usuário
- ✅ Integração com Select2 para busca de placas

---

## Arquitetura

### Tecnologias Utilizadas

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| Syncfusion EJ2 Charts | 23.1.36 | Gráficos e Heatmaps |
| jQuery | 3.x | Manipulação DOM e AJAX |
| Select2 | - | Campo de busca de placas |
| jsPDF | 2.5.1 | Exportação PDF |
| html2canvas | 1.4.1 | Captura de elementos para PDF |

### Padrões de Design

- Variáveis globais para instâncias de gráficos (evitar memory leaks)
- Paleta de cores CARAMELO definida em constante `CORES`
- Overlay de loading padrão FrotiX

---

## Estrutura de Arquivos

### Arquivo Principal

```
wwwroot/js/dashboards/dashboard-abastecimento.js
```

### Arquivos Relacionados

- `Pages/Abastecimento/DashboardAbastecimento.cshtml` - Página Razor
- `Controllers/AbastecimentoController.DashboardAPI.cs` - API Backend

---

## Funções Principais

### Inicialização

| Função | Linha | Descrição |
|--------|-------|-----------|
| `inicializarTabs()` | ~280 | Configura navegação entre abas e eventos dos filtros |
| `inicializarFiltrosECarregar()` | ~175 | Busca anos disponíveis e inicializa filtros |

### Carregamento de Dados

| Função | Linha | Descrição |
|--------|-------|-----------|
| `carregarDadosVeiculo(autoSelecionarAno, placaAnterior, textoPlacaAnterior)` | ~1020 | Carrega dados da aba Veículo |
| `executarCarregamentoVeiculo(ano, mes, placaAnterior, textoPlacaAnterior)` | ~1063 | Executa chamada AJAX para dados de veículo |
| `carregarDadosVeiculoPeriodo(dataInicio, dataFim)` | ~785 | Carrega dados por período personalizado |

### Renderização

| Função | Linha | Descrição |
|--------|-------|-----------|
| `renderizarAbaGeral(data)` | ~1130 | Renderiza cards e gráficos da aba Geral |
| `renderizarAbaVeiculo(data, veiculoId, placa)` | - | Renderiza cards e gráficos da aba Veículo |
| `renderizarHeatmapVeiculo(ano, placa, mes)` | - | Renderiza mapa de calor |

### Utilitárias

| Função | Linha | Descrição |
|--------|-------|-----------|
| `exibirToastAmareloAposLoading(mensagens, duracaoMs)` | ~132 | Exibe toast amarelo após loading (padrão 4s) |
| `obterPlacaTextoCompleto(textoCompleto)` | ~149 | Extrai apenas a placa do texto completo |
| `preencherFiltrosVeiculo(data)` | ~2190 | Preenche lista de placas disponíveis |

---

## Filtros e Eventos

### Aba Consumo por Veículo

#### Evento: Mudança de Ano (`filtroAnoVeiculo.change`)

Quando o usuário muda o ano:
1. Guarda a placa selecionada via Select2 API
2. Limpa campos de período personalizado
3. Limpa lista de meses
4. Chama `prepararAtualizacaoVeiculoSemPlaca()`
5. Carrega dados passando placa anterior
6. Se placa não disponível no novo ano, exibe toast amarelo

```javascript
document.getElementById('filtroAnoVeiculo')?.addEventListener('change', function () {
    // Guardar placa selecionada antes de limpar
    const $selectPlaca = $('#filtroPlacaVeiculo');
    let placaSelecionadaAntes = '';
    let textoPlacaAntes = '';

    if ($selectPlaca.hasClass('select2-hidden-accessible')) {
        placaSelecionadaAntes = $selectPlaca.val() || '';
        const select2Data = $selectPlaca.select2('data');
        textoPlacaAntes = (select2Data && select2Data.length > 0) ? select2Data[0].text : '';
    }

    prepararAtualizacaoVeiculoSemPlaca();
    carregarDadosVeiculo(false, placaSelecionadaAntes, textoPlacaAntes);
});
```

#### Evento: Mudança de Mês (`filtroMesVeiculo.change`)

Mesmo comportamento do evento de ano - guarda placa antes de limpar e verifica disponibilidade após carregar dados.

#### Verificação de Placa Disponível

Na função `executarCarregamentoVeiculo`, após receber dados da API:

```javascript
if (placaAnterior && textoPlacaAnterior && textoPlacaAnterior !== 'Todas') {
    const placasDisponiveis = data?.placasDisponiveis || [];
    const placaAindaDisponivel = placasDisponiveis.some(p => p.veiculoId === placaAnterior);
    if (!placaAindaDisponivel) {
        const placaTexto = obterPlacaTextoCompleto(textoPlacaAnterior);
        mensagensToast.push('Nenhum Abastecimento no Período para o Veículo' + (placaTexto ? ' ' + placaTexto : ''));
    }
}
```

---

## Interconexões

### APIs Consumidas

| Endpoint | Método | Descrição |
|----------|--------|-----------|
| `/api/abastecimento/DashboardDados` | GET | Dados gerais e anos disponíveis |
| `/api/abastecimento/DashboardVeiculo` | GET | Dados por veículo/período |
| `/api/abastecimento/HeatmapDiaHora` | GET | Dados para mapa de calor |

### Dependências

- `AppToast` - Sistema de toast global
- `Alerta` - Sistema de alertas SweetAlert
- `FtxSpin` - Overlay de loading FrotiX

---

## PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [17/01/2026] - Toast Amarelo para Veículo Sem Abastecimento

**Descrição**:
Implementado toast amarelo que é exibido quando o usuário muda o ano ou mês na aba "Consumo por Veículo" e a placa selecionada não possui abastecimentos no novo período.

**Alterações**:

- Evento `filtroAnoVeiculo.change`: Agora guarda placa via Select2 API antes de limpar e carrega dados
- Evento `filtroMesVeiculo.change`: Também guarda placa antes de limpar
- Função `carregarDadosVeiculo`: Novos parâmetros `placaAnterior` e `textoPlacaAnterior`
- Função `executarCarregamentoVeiculo`: Verifica se placa anterior está disponível
- Função `exibirToastAmareloAposLoading`: Duração alterada de 6s para 4s
- Corrigido escopo da variável `mensagensToast` (movida para fora do `try`)

**Status**: ✅ **Concluído**

---

## [17/01/2026] - Criação da Documentação

**Descrição**: Documentação inicial criada.

**Status**: ✅ **Concluído**

---

**Última atualização**: 17/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.0
