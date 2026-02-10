# 📘 Documentação: eventoupsert.js

> **Arquivo:** `/wwwroot/js/cadastros/eventoupsert.js`
> **Data de Documentação:** 02/02/2026
> **Módulo:** Agendamento/Eventos
> **Status:** ✅ Documentado

---

## 🎯 Objetivo Geral

Gerenciar a lógica de formulário de criação e edição de eventos (Upsert), incluindo:
- Inicialização de dropdowns DropDownTree (requisitante e setor)
- Validação de campos numéricos (quantidade de participantes)
- Carregamento dinâmico de tabela DataTable com viagens associadas ao evento
- Estatísticas consolidadas de custos (total, médio, sem custo)
- Modal de detalhamento de custos por viagem
- Modal de desassociação de viagens com transferência de finalidade
- Sincronização bidirecional entre DropDownTree de setor e campo de texto visível

---

## 📥 Entradas

### Variáveis Globais (Passadas pela View/Controller)

| Variável | Tipo | Descrição |
|----------|------|-----------|
| `eventoId` | UUID | ID do evento (GUID completo ou "00000000-0000-0000-0000-000000000000" para novo) |
| `requisitanteId` | UUID | ID do requisitante pré-selecionado (se edição) |
| `setorsolicitanteId` | UUID | ID do setor requisitante pré-selecionado (se edição) |

### Eventos DOM

| Elemento | Evento | Descrição |
|----------|--------|-----------|
| `.btn-custos-viagem` | `click` | Botão para abrir modal de custos da viagem |
| `.btn-desassociar-viagem` | `click` | Botão para abrir modal de desassociação |
| `#btnConfirmarDesassociar` | `click` | Botão de confirmação do modal de desassociação |
| `#tblViagens` | `draw.dt` | Evento de redraw do DataTable (recalcular estatísticas) |
| `#ddtSetorRequisitanteEvento` | `change` | Evento de mudança do DropDownTree de setor |

---

## 📤 Saídas

### Manipulação DOM

| Elemento | Ação | Descrição |
|----------|------|-----------|
| `#tblViagens` | Inicializado como DataTable | Tabela com viagens associadas ao evento |
| `#totalViagens` | `.text()` | Quantidade total de viagens |
| `#custoTotalViagens` | `.text()` | Custo total consolidado em BRL |
| `#custoMedioViagem` | `.text()` | Custo médio por viagem em BRL |
| `#viagensSemCusto` | `.text()` | Quantidade de viagens sem custo registrado |
| `#TituloViagens` | `.html()` | Título dinâmico com resumo de estatísticas |
| `#modalCustosViagem` | `Bootstrap.Modal.show()` | Modal de detalhamento de custos |
| `#modalDesassociar` | `Bootstrap.Modal.show()` | Modal de desassociação de viagem |
| `#txtSetorRequisitante` | `.value` | Campo de texto sincronizado com DropDownTree |

### Chamadas AJAX

| Endpoint | Método | Descrição |
|----------|--------|-----------|
| `/api/viagem/listaviagensevento` | GET | Carrega lista de viagens associadas ao evento |
| `/api/viagem/ObterTotalCustoViagensEvento` | GET | Obtém estatísticas consolidadas (total, média, sem custo) |
| `/api/viagem/DesassociarViagemEvento` | POST | Remove viagem do evento e atribui nova finalidade |
| `/api/viagem/ObterCustosViagem` | GET | Detalhamento completo de custos por viagem |

---

## 🔗 Chamada Por

- **Página Razor:** `/Pages/Operacao/Evento/Upsert.cshtml` ou equivalente
- **Evento:** Carregamento automático via `<script src="...eventoupsert.js"></script>`
- **Disparo:** `$(document).ready(function() {...})` ao carregar a página

---

## 🔄 Chama

### Funções Internas

| Função | Linha | Descrição |
|--------|-------|-----------|
| `carregarEstatisticasViagens()` | 34 | Busca dados consolidados de viagens |
| `formatarMoeda(valor)` | 94 | Formata número para BRL |
| `initEventoTable()` | 116 | Inicializa DataTable de viagens |
| `atualizarTotalViagens()` | 319 | Atualiza título dinâmico com totais |
| `carregarDetalhamentoCustos(viagemId)` | 593 | Carrega custos detalhados de uma viagem |
| `atualizarCampoSetor()` | 732 | Sincroniza DropDownTree de setor com campo de texto |
| `findTextByValue(data, value)` | 776 | Busca recursiva de texto em árvore DropDownTree |

### APIs Externas

| Endpoint | Método | Descrição |
|----------|--------|-----------|
| `/api/viagem/ObterTotalCustoViagensEvento` | GET | Chamado em `carregarEstatisticasViagens()` |
| `/api/viagem/listaviagensevento` | GET | Chamado pelo DataTable via AJAX |
| `/api/viagem/DesassociarViagemEvento` | POST | Chamado ao confirmar desassociação |
| `/api/viagem/ObterCustosViagem` | GET | Chamado em `carregarDetalhamentoCustos()` |

### Bibliotecas/Plugins

| Lib/Plugin | Função | Versão |
|-----------|--------|--------|
| jQuery | Seleção DOM, AJAX, eventos | 3.x |
| DataTables | Tabela de viagens | 1.10.25+ |
| Bootstrap Modal | Modals de custos e desassociação | 5.3 |
| Syncfusion EJ2 | DropDownTree (requisitante, setor) | 20.x+ |
| Alerta.js | Exibição de alertas SweetAlert | Custom |
| AppToast.js | Exibição de toasts (notificações rápidas) | Custom |
| FtxSpin.js | Loading overlay | Custom |

---

## 📦 Dependências

### JavaScript

```javascript
// Variáveis globais esperadas
eventoId          // String/UUID
requisitanteId    // String/UUID
setorsolicitanteId // String/UUID

// Funções globais esperadas
mostrarLoading(msg)  // FtxSpin.show() - Exibe overlay de loading
esconderLoading()    // FtxSpin.hide() - Esconde overlay
Alerta.TratamentoErroComLinha(arquivo, metodo, erro) // Tratamento de erros
Alerta.Sucesso/Erro/Warning(titulo, msg) // Alertas
AppToast.show(cor, msg, duracao) // Toasts notificações
```

### HTML/DOM

Elementos esperados na página:

```html
<!-- Campos de formulário -->
<input id="txtDataInicialEvento" type="date" />
<input id="txtQtdParticipantes" type="number" />

<!-- DropDownTrees Syncfusion -->
<div id="lstRequisitanteEvento" class="e-dropdowntree"></div>
<div id="ddtSetorRequisitanteEvento" class="e-dropdowntree"></div>
<input id="ddlSetorRequisitanteEvento" type="hidden" />
<input id="txtSetorRequisitante" type="text" readonly />

<!-- Tabela de Viagens -->
<table id="tblViagens" class="table"></table>

<!-- Elementos de Estatísticas -->
<div id="totalViagens"></div>
<div id="custoTotalViagens"></div>
<div id="custoMedioViagem"></div>
<div id="viagensSemCusto"></div>
<div id="TituloViagens"></div>

<!-- Modals -->
<div id="modalCustosViagem" class="modal">
  <div id="requisitanteCustos"></div>
  <div id="infoViagemCustos"></div>
  <div id="tempoTotalCustos"></div>
  <div id="kmPercorridoCustos"></div>
  <div id="litrosGastosCustos"></div>
  <div id="custoMotoristaCustos"></div>
  <div id="custoVeiculoCustos"></div>
  <div id="custoCombustivelCustos"></div>
  <div id="custoTotalCustos"></div>
</div>

<div id="modalDesassociar" class="modal">
  <input id="viagemIdDesassociar" type="hidden" />
  <div id="infoViagemDesassociar"></div>
  <select id="lstNovaFinalidade"></select>
  <button id="btnConfirmarDesassociar">Confirmar</button>
</div>
```

---

## 🚨 Avisos e Observações

### 1. Inicialização Dupla (Lines 6-18 + 393-407)

O arquivo tenta preencher DDTs duas vezes:
- **Linhas 6-18:** No document.ready
- **Linhas 393-407:** Novamente depois

**Recomendação:** Consolidar em um único bloco para evitar redundância.

### 2. Compatibilidade de Nomenclatura em Campos

**Linha 620:** O código usa coalescing para compatibilidade com ambos os formatos de resposta:

```javascript
var infoViagem = d.infoViagem ?? d.InfoViagem;  // camelCase ou PascalCase
```

Isso indica que há ambiguidade na API sobre nomenclatura (provavelmente migrations de serialização JSON).

**Recomendação:** Padronizar a API para usar sempre `camelCase` em JSON.

### 3. MutationObserver para DropDownTree (Lines 812-838)

Um `MutationObserver` monitora mudanças no DOM do DropDownTree para sincronizar o campo de texto visível. Isso é necessário porque:

- O DropDownTree Syncfusion não dispara eventos de `change` confiáveis em todos os casos
- O campo de texto é renderizado em outro elemento do formulário

**Performance:** O observer roda continuamente, mesmo que desnecessário. Considerar usar apenas `change` event.

### 4. DataTable Configuração Minimalista

**Linha 127-296:** O DataTable não utiliza features como:
- Busca (searching: false)
- Ordenação (ordering: false)
- Paginação customizada (dom: 'rtip')

Isso reduz funcionalidade, mas melhora performance para listas grandes. Apropriado para this use case.

### 5. Tratamento de NULL em Custos

**Linhas 249-259:** Custos NULL são exibidos como `-` em vez de `R$ 0,00`. Isso diferencia entre "sem informação" e "custo zero", que é semanticamente correto.

### 6. Coluna de Ações não Ordernable (Line 265)

```javascript
orderable: false,
searchable: false
```

Correto - botões de ação não devem ser ordenáveis ou pesquisáveis.

---

## 📝 Fluxo de Execução

### 1. Inicialização (document.ready)

```
1. Try-catch wrapper
   ├─ Preencher DDTs se evento em edição (linhas 6-18)
   ├─ Validar entrada de quantidade de participantes (linhas 20-31)
   ├─ Definir data inicial como "hoje" se novo evento (linhas 14-18)
   └─ Try-catch global para capturar erros
```

### 2. Carregamento de Dados

```
1. DOMContentLoaded (linhas 727-844)
   ├─ Monitorar mudanças do DropDownTree de setor
   └─ Sincronizar campo de texto visível com DropDownTree oculto

2. document.ready (linhas 1-694)
   ├─ Se #tblViagens existe → initEventoTable()
   │   ├─ Mostrar loading FtxSpin
   │   ├─ Carregar dados via AJAX: /api/viagem/listaviagensevento
   │   │   └─ Preencher DataTable com colunas customizadas
   │   ├─ Chamar carregarEstatisticasViagens()
   │   └─ Esconder loading
   │
   └─ Chamar carregarEstatisticasViagens() imediatamente (linha 304)
       ├─ AJAX GET: /api/viagem/ObterTotalCustoViagensEvento
       └─ Preencher #totalViagens, #custoTotalViagens, #custoMedioViagem, #viagensSemCusto
```

### 3. Redraw do DataTable (draw.dt event - linhas 307-316, 381-390)

```
1. Quando tabela é redesenhada
   ├─ Chamar carregarEstatisticasViagens()
   └─ Chamar atualizarTotalViagens()
```

### 4. Click em Botão "Custos da Viagem" (linhas 412-450)

```
1. Prevenir default (e.preventDefault())
2. Extrair ID da viagem (data-id)
3. Chamar carregarDetalhamentoCustos(viagemId)
   ├─ Limpar valores anteriores do modal
   ├─ AJAX GET: /api/viagem/ObterCustosViagem
   ├─ Preencher campos do modal (motorista, veículo, combustível, totais)
   └─ Compatibilidade: camelCase vs PascalCase (linhas 620-660)
4. Abrir modal via Bootstrap.Modal.show()
```

### 5. Click em Botão "Desassociar Viagem" (linhas 455-493)

```
1. Extrair dados: ID, ficha, requisitante
2. Preencher campos do modal:
   - #viagemIdDesassociar ← ID
   - #infoViagemDesassociar ← "Ficha XXX - Requisitante YYY"
   - #lstNovaFinalidade ← limpar seleção
3. Abrir modal
```

### 6. Confirmação de Desassociação (linhas 498-588)

```
1. Validação:
   - Verificar se nova finalidade foi selecionada
   - Se não → toast amarelo + focus no select

2. Desabilitar botão + mostrar spinner

3. AJAX POST: /api/viagem/DesassociarViagemEvento
   ├─ Body: { ViagemId, NovaFinalidade }
   │
   ├─ Success:
   │   ├─ Toast verde com mensagem
   │   ├─ Fechar modal
   │   ├─ Recarregar DataTable (.ajax.reload())
   │   ├─ Chamar carregarEstatisticasViagens()
   │   └─ Chamar atualizarTotalViagens()
   │
   ├─ Error:
   │   └─ Toast vermelho com mensagem de erro
   │
   └─ Complete:
       ├─ Restaurar estado do botão
       └─ Remover spinner
```

---

## 🔍 Análise por Seção

### A. Inicialização de DropDownTrees (Linhas 5-18, 393-407)

#### Código
```javascript
// [LOGICA] Preenchimento inicial dos DDTs ao editar
if (eventoId !== '00000000-0000-0000-0000-000000000000' && eventoId !== null)
{
    const ddtReq = document.getElementById("lstRequisitanteEvento")?.ej2_instances?.[0];
    const ddtSet = document.getElementById("ddtSetorRequisitanteEvento")?.ej2_instances?.[0];
    if (ddtReq) ddtReq.value = requisitanteId;
    if (ddtSet) ddtSet.value = setorsolicitanteId;
}
```

#### Análise
- **Operador `?.`:** Safe navigation para evitar errors se elemento não existe
- **`ej2_instances[0]`:** Acesso à instância Syncfusion EJ2 do componente
- **Condição:** Apenas em modo edição (evento não é novo)
- **Safe assignment:** Validar que objeto existe antes de atribuir `.value`

**Problema:** Faz isso em dois lugares (redundante). Considerar consolidar.

---

### B. Validação de Quantidade de Participantes (Linhas 20-31)

#### Código
```javascript
// [VALIDACAO] Evita números negativos nos participantes
$("#txtQtdParticipantes").on("input", function ()
{
    try
    {
        const v = parseInt(this.value || "0", 10);
        if (v < 0) this.value = 0;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("eventoupsert.js", "txtQtdParticipantes.input", error);
    }
});
```

#### Análise
- **Evento `input`:** Dispara a cada caractere digitado
- **Try-catch:** Necessário pois `parseInt()` pode falhar
- **Regra:** Quantidade não pode ser negativa
- **Fallback:** Se vazio, assume "0"

**Melhoria possível:** Validar max também (ex: max = 9999), ou usar input type="number" com min/max HTML5.

---

### C. Carregamento de Estatísticas (Linhas 34-91)

#### Código Completo
```javascript
// [AJAX] Endpoint: GET /api/viagem/ObterTotalCustoViagensEvento
function carregarEstatisticasViagens()
{
    try
    {
        $.ajax({
            url: "/api/viagem/ObterTotalCustoViagensEvento",
            type: "GET",
            data: { Id: eventoId },
            success: function (response)
            {
                try
                {
                    if (response.success)
                    {
                        // [UI] Preencher campos de estatísticas
                        $("#totalViagens").text(response.totalViagens);
                        $("#custoTotalViagens").text(response.totalCustoFormatado);
                        $("#viagensSemCusto").text(response.viagensSemCusto || "0");

                        // [LOGICA] Calcular e preencher média
                        const media = response.custoMedioFormatado ||
                            formatarMoeda(response.totalViagens > 0 ? response.totalCusto / response.totalViagens : 0);
                        $("#custoMedioViagem").text(media);

                        // [UI] Adicionar classes de destaque se necessário
                        if (response.viagensSemCusto > 0)
                        {
                            $("#viagensSemCusto").addClass('text-danger');
                        }

                        console.log('Estatísticas carregadas:', response);
                    }
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarEstatisticasViagens.success", error);
                }
            },
            error: function (xhr, status, error)
            {
                try
                {
                    console.error('Erro ao carregar estatísticas:', error);
                    // [UI] Valores padrão em caso de erro
                    $("#totalViagens").text("0");
                    $("#custoTotalViagens").text("R$ 0,00");
                    $("#custoMedioViagem").text("R$ 0,00");
                    $("#viagensSemCusto").text("0");
                } catch (err)
                {
                    Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarEstatisticasViagens.error", err);
                }
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarEstatisticasViagens", error);
    }
}
```

#### Análise
- **Estrutura:** Try-catch em 3 níveis (função, success, error)
- **Resposta esperada:**
  ```json
  {
    "success": true,
    "totalViagens": 10,
    "totalCusto": 5000.00,
    "totalCustoFormatado": "R$ 5.000,00",
    "custoMedioFormatado": "R$ 500,00",
    "viagensSemCusto": 2
  }
  ```
- **Fallback:** Se `custoMedioFormatado` não vem, calcula manualmente
- **UI Pattern:** Valores padrão ("0", "R$ 0,00") em caso de erro
- **Destaque:** Adiciona classe `text-danger` se há viagens sem custo

---

### D. Formatação de Moeda (Linhas 94-108)

#### Código
```javascript
// [HELPER] Função auxiliar para formatar moeda
function formatarMoeda(valor)
{
    try
    {
        if (!valor && valor !== 0) return "R$ 0,00";
        return parseFloat(valor).toLocaleString('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("eventoupsert.js", "formatarMoeda", error);
        return "R$ 0,00";
    }
}
```

#### Análise
- **Validação `!valor && valor !== 0`:** Diferencia entre falsy (null, undefined, "") e zero
  - `null` → "R$ 0,00" ✅
  - `0` → "R$ 0,00" (após parseFloat) ✅
  - `undefined` → "R$ 0,00" ✅
  - `""` → "R$ 0,00" ✅
- **`toLocaleString('pt-BR', {...})`:** Formata com vírgulas decimais e ponto como separador de milhares
  - `1000.50` → `"R$ 1.000,50"`
- **Fallback:** Sempre retorna formato BRL válido

---

### E. Inicialização do DataTable (Linhas 111-301)

#### Configuração AJAX
```javascript
ajax: {
    url: "/api/viagem/listaviagensevento",
    type: "GET",
    data: { Id: eventoId },
    dataSrc: 'data',  // Dados estão em response.data
    beforeSend: function () { console.time('Requisição API'); },
    complete: function (data) {
        // [UI] Esconde loading
        if (typeof esconderLoading === 'function') esconderLoading();

        // [LOGICA] Após carregar os dados, buscar o total
        console.timeEnd('Requisição API');
        console.log('Quantidade de registros:', data.responseJSON?.data?.length);
        carregarEstatisticasViagens();
    },
    error: function (xhr, status, error) {
        // Esconde loading mesmo em caso de erro
        if (typeof esconderLoading === 'function') esconderLoading();
        console.error('Erro ao carregar viagens:', error);
    }
}
```

**Resposta esperada:**
```json
{
  "data": [
    {
      "noFichaVistoria": "123456",
      "dataInicial": "2026-02-01T10:00:00",
      "horaInicio": "2026-02-01T10:30:00",
      "nomeRequisitante": "João Silva",
      "nomeSetor": "Logística",
      "nomeMotorista": "Carlos",
      "descricaoVeiculo": "Iveco Stralis",
      "custoViagem": 500.50,
      "viagemId": "uuid-aqui"
    }
  ]
}
```

#### Colunas do DataTable

| Coluna | Propriedade | Renderização | Notas |
|--------|-------------|--------------|-------|
| 1 | `noFichaVistoria` | Direto (ou '-') | Ficha de vistoria |
| 2 | `dataInicial` | Formata DD/MM/AAAA | Date parsing e formatação |
| 3 | `horaInicio` | Formata HH:MM | Time extraction |
| 4 | `nomeRequisitante` | Direto | Left-aligned |
| 5 | `nomeSetor` | Direto | Left-aligned |
| 6 | `nomeMotorista` | Direto (ou '<span class="text-muted">-</span>') | Gray text se null |
| 7 | `descricaoVeiculo` | Direto | Left-aligned |
| 8 | `custoViagem` | `toLocaleString('pt-BR', {currency: 'BRL'})` | Right-aligned, moeda formatada |
| 9 | `viagemId` | Botões de ação | Detalhes e Desassociar |

#### Análise de Renderizações Personalizadas

**Coluna 2 (Data Inicial):**
```javascript
render: function (data, type, row)
{
    if (!data) return '-';
    if (type === 'display')
    {
        const date = new Date(data);
        const dia = date.getDate().toString().padStart(2, '0');
        const mes = (date.getMonth() + 1).toString().padStart(2, '0');
        const ano = date.getFullYear();
        return `${dia}/${mes}/${ano}`;
    }
    return data;  // Retorna ISO para sorting
}
```

**Problema potencial:** Se `type !== 'display'` (ex: sorting), retorna ISO (string). DataTables não consegue ordenar corretamente strings ISO. **Solução:** Retornar timestamp numérico para sorting.

---

### F. Detalhamento de Custos (Linhas 593-688)

#### Código Estrutura
```javascript
function carregarDetalhamentoCustos(viagemId) {
    try {
        // [UI] Limpa valores anteriores
        $('#infoViagemCustos').text('--');
        $('#tempoTotalCustos').text('-');
        // ... etc

        // [AJAX] GET /api/viagem/ObterCustosViagem
        $.ajax({
            url: "/api/viagem/ObterCustosViagem",
            type: "GET",
            data: { viagemId: viagemId },  // Parâmetro correto: viagemId
            success: function (response) {
                // ... preencher campos

                // [LOGICA] Compatibilidade com ambos os formatos
                var infoViagem = d.infoViagem ?? d.InfoViagem;
                var duracaoFormatada = d.duracaoFormatada ?? d.DuracaoFormatada;
                var kmPercorrido = d.kmPercorrido ?? d.KmPercorrido;
                var litrosGastos = d.litrosGastos ?? d.LitrosGastos;
                var custoMotorista = d.custoMotorista ?? d.CustoMotorista ?? 0;
                var custoVeiculo = d.custoVeiculo ?? d.CustoVeiculo ?? 0;
                var custoCombustivel = d.custoCombustivel ?? d.CustoCombustivel ?? 0;
                var custoTotal = d.custoTotal ?? d.CustoTotal ?? 0;
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("eventoupsert.js", "carregarDetalhamentoCustos", error);
    }
}
```

#### Resposta Esperada
```json
{
  "success": true,
  "data": {
    "infoViagem": "Ficha 123456 - 01/02/2026",  // ou InfoViagem
    "duracaoFormatada": "2h 30m",                // ou DuracaoFormatada
    "kmPercorrido": 150.5,                       // ou KmPercorrido
    "litrosGastos": 25.8,                        // ou LitrosGastos
    "custoMotorista": 100.00,                    // ou CustoMotorista
    "custoVeiculo": 200.00,                      // ou CustoVeiculo
    "custoCombustivel": 150.00,                  // ou CustoCombustivel
    "custoTotal": 450.00                         // ou CustoTotal
  }
}
```

**Compatibilidade:** O código assume que API pode retornar tanto camelCase quanto PascalCase. Usa nullish coalescing (`??`) para fallback.

---

### G. Modal de Desassociação (Linhas 455-588)

#### Fluxo Completo

1. **Abrir Modal (linhas 455-493):**
   ```javascript
   $(document).on('click', '.btn-desassociar-viagem', function (e) {
       e.preventDefault();
       const viagemId = $(this).data('id');
       const ficha = $(this).data('ficha');
       const requisitante = $(this).data('requisitante');

       // [UI] Preenche dados no modal
       $('#viagemIdDesassociar').val(viagemId);
       $('#infoViagemDesassociar').text(`Ficha ${ficha || '-'} - ${requisitante || 'Não informado'}`);
       $('#lstNovaFinalidade').val('');  // Limpa seleção

       // [UI] Abre modal Bootstrap
       const modalElement = document.getElementById('modalDesassociar');
       const modal = new bootstrap.Modal(modalElement);
       modal.show();
   });
   ```

2. **Confirmar (linhas 498-588):**
   ```javascript
   $('#btnConfirmarDesassociar').on('click', function () {
       try {
           const viagemId = $('#viagemIdDesassociar').val();
           const novaFinalidade = $('#lstNovaFinalidade').val();

           // [VALIDACAO] Finalidade obrigatória
           if (!novaFinalidade) {
               AppToast.show('Amarelo', 'Selecione uma nova finalidade para a viagem!', 3000);
               $('#lstNovaFinalidade').focus();
               return;
           }

           // [UI] Desabilita botão + spinner
           const btn = $(this);
           btn.prop('disabled', true);
           btn.html('<i class="fa-solid fa-spinner fa-spin icon-space"></i> Processando...');

           // [AJAX] POST /api/viagem/DesassociarViagemEvento
           $.ajax({
               url: "/api/viagem/DesassociarViagemEvento",
               type: "POST",
               contentType: "application/json; charset=utf-8",
               data: JSON.stringify({
                   ViagemId: viagemId,
                   NovaFinalidade: novaFinalidade
               }),
               success: function (response) {
                   if (response.success) {
                       AppToast.show('Verde', response.message || 'Viagem desassociada com sucesso!', 3000);

                       // [UI] Fecha modal
                       const modalElement = document.getElementById('modalDesassociar');
                       const modal = bootstrap.Modal.getInstance(modalElement);
                       if (modal) modal.hide();

                       // [LOGICA] Recarrega DataTable e estatísticas
                       if ($.fn.DataTable.isDataTable('#tblViagens')) {
                           $('#tblViagens').DataTable().ajax.reload(null, false);
                       }
                       carregarEstatisticasViagens();
                       atualizarTotalViagens();
                   } else {
                       AppToast.show('Vermelho', response.message || 'Erro ao desassociar viagem', 3000);
                   }
               },
               error: function (xhr, status, error) {
                   AppToast.show('Vermelho', 'Erro ao desassociar viagem do evento', 3000);
               },
               complete: function () {
                   // [UI] Restaura botão
                   btn.prop('disabled', false);
                   btn.html(textoOriginal);
               }
           });
       } catch (error) {
           Alerta.TratamentoErroComLinha("eventoupsert.js", "btnConfirmarDesassociar.click", error);
       }
   });
   ```

**Body esperado:**
```json
{
  "ViagemId": "uuid-viagem",
  "NovaFinalidade": "uuid-finalidade-nova"
}
```

**Resposta esperada:**
```json
{
  "success": true,
  "message": "Viagem desassociada e atribuída a nova finalidade com sucesso"
}
```

---

### H. Sincronização DropDownTree ↔ Campo de Texto (Linhas 727-844)

#### Problema Que Resolve

Syncfusion DropDownTree é um control "oculto" que roda internamente em JavaScript. A página precisa exibir o valor selecionado em um campo de texto visível. Quando usuário seleciona algo no DropDownTree, esse valor precisa ser refletido no textbox.

#### Solução Implementada

**1. Listener de Change (Linhas 803-809):**
```javascript
var setorDropDown = document.getElementById('ddtSetorRequisitanteEvento');
if (setorDropDown) {
    setorDropDown.addEventListener('change', atualizarCampoSetor);

    // Se já houver um valor inicial, atualiza
    setTimeout(atualizarCampoSetor, 500);
}
```

**2. MutationObserver para Mudanças DOM (Linhas 812-838):**
```javascript
var observer = new MutationObserver(function (mutations) {
    try {
        mutations.forEach(function (mutation) {
            if (mutation.type === 'attributes' || mutation.type === 'childList') {
                atualizarCampoSetor();
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha("eventoupsert.js", "MutationObserver.callback", error);
    }
});

if (setorDropDown) {
    observer.observe(setorDropDown, {
        attributes: true,
        childList: true,
        subtree: true,
        attributeFilter: ['value']
    });
}
```

**3. Atualização do Campo (Linhas 732-773):**
```javascript
function atualizarCampoSetor() {
    try {
        setTimeout(function () {
            try {
                var setorDropDown = document.getElementById('ddtSetorRequisitanteEvento');
                if (setorDropDown && setorDropDown.ej2_instances && setorDropDown.ej2_instances[0]) {
                    var setorInstance = setorDropDown.ej2_instances[0];

                    // [LOGICA] Obtém texto selecionado
                    var textoSetor = setorInstance.text || '';

                    // Se não houver texto, tenta encontrar de outras formas
                    if (!textoSetor && setorInstance.value) {
                        var selectedData = setorInstance.treeData;
                        if (selectedData && selectedData.length > 0) {
                            textoSetor = findTextByValue(selectedData, setorInstance.value[0]);
                        }
                    }

                    // [UI] Atualiza campo de texto
                    document.getElementById('txtSetorRequisitante').value = textoSetor;
                }
            } catch (error) {
                Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarCampoSetor.setTimeout", error);
            }
        }, 100);
    } catch (error) {
        Alerta.TratamentoErroComLinha("eventoupsert.js", "atualizarCampoSetor", error);
    }
}
```

**4. Busca Recursiva em Árvore (Linhas 776-799):**
```javascript
function findTextByValue(data, value) {
    try {
        for (var i = 0; i < data.length; i++) {
            if (data[i].SetorSolicitanteId === value) {
                return data[i].Nome;
            }
            // [LOGICA] Se houver subnós, procura recursivamente
            if (data[i].child) {
                var found = findTextByValue(data[i].child, value);
                if (found) return found;
            }
        }
        return '';
    } catch (error) {
        Alerta.TratamentoErroComLinha("eventoupsert.js", "findTextByValue", error);
        return '';
    }
}
```

**Estrutura esperada de dados (treeData):**
```javascript
[
  {
    "SetorSolicitanteId": "uuid-1",
    "Nome": "Logística",
    "child": [
      { "SetorSolicitanteId": "uuid-1-1", "Nome": "Transporte", "child": [] },
      { "SetorSolicitanteId": "uuid-1-2", "Nome": "Armazenagem", "child": [] }
    ]
  },
  {
    "SetorSolicitanteId": "uuid-2",
    "Nome": "Operações",
    "child": [...]
  }
]
```

---

## 🐛 Problemas Identificados

### P1: Documentação no Código (ícone fa-solid)

**Linha 517:**
```javascript
btn.html('<i class="fa-solid fa-spinner fa-spin icon-space"></i> Processando...');
```

**Problema:** Usa `fa-solid`, que viola a regra FrotiX de usar sempre `fa-duotone`.

**Solução:**
```javascript
btn.html('<i class="fa-duotone fa-spinner fa-spin icon-space"></i> Processando...');
```

---

### P2: Inicialização Redundante

**Linhas 6-18 e 393-407:** Preenchem DDTs duas vezes.

**Solução:** Consolidar em um único bloco.

---

### P3: Duplicação de Event Listeners

**Linhas 307-316 e 381-390:** Ambos os blocos executam ao draw do DataTable.

**Análise:** Intenção parece ser chamar ambas `carregarEstatisticasViagens()` e `atualizarTotalViagens()` em cada redraw. Confirmar com requisitos.

---

### P4: Performance do MutationObserver

**Linhas 812-838:** O observer roda continuamente.

**Recomendação:** Considerar usar apenas `change` event e remover observer.

---

## ✅ Pontos Fortes

1. ✅ Try-catch em 3 níveis (wrapper externo, success, error)
2. ✅ Validação de valores nulos com operadores seguros (`?.`, `??`)
3. ✅ Fallbacks para erros (valores padrão "R$ 0,00")
4. ✅ Destaque visual de anomalias (viagens sem custo em vermelho)
5. ✅ Comportamento gracioso em caso de erro na API
6. ✅ DataTable responsivo com colunas bem-dimensionadas
7. ✅ Modals com validações antes de submit
8. ✅ Rastreamento de tempo (console.time/timeEnd) para debugging
9. ✅ Logging detalhado em console

---

## 📋 Log de Modificações

| Data | Versão | Autor | Descrição |
|------|--------|-------|-----------|
| 02/02/2026 | 1.0 | Claude Code | Documentação inicial conforme padrão FrotiX |

---

**✅ Documentação completa segundo padrão FrotiX - Seção 5 (Documentação Intra-código)**
