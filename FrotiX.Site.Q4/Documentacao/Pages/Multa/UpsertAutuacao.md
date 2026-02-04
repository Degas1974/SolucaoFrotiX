# Documentação: Multa - Upsert Autuação

> **Última Atualização**: 16/01/2026
> **Versão Atual**: 1.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice

1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Lógica de Negócio](#lógica-de-negócio)
5. [Interconexões](#interconexões)
6. [Endpoints API](#endpoints-api)
7. [Frontend](#frontend)
8. [Validações](#validações)
9. [Troubleshooting](#troubleshooting)

---

## Visão Geral

Página de criação e edição (Upsert) de notificações de autuação de multas no sistema FrotiX.

### Características Principais

- ✅ **Criação de Autuações**: Permite criar novas notificações de autuação
- ✅ **Edição de Autuações**: Permite atualizar autuações existentes
- ✅ **Upload de PDF**: Upload do documento de notificação em PDF
- ✅ **Busca de Viagem**: Vincula a multa a uma viagem específica baseada em data/hora/veículo
- ✅ **Ficha de Vistoria**: Exibe e baixa a ficha de vistoria relacionada
- ✅ **Gestão de Empenhos**: Associa multa a empenho do órgão autuante
- ✅ **Validações**: Valida contratos/atas de veículos e motoristas

### Objetivo

Centralizar o cadastro de notificações de autuação, facilitando o controle de multas, vinculação com viagens, motoristas e veículos, além de gestão financeira através de empenhos.

---

## Arquitetura

### Tecnologias Utilizadas

| Tecnologia     | Versão  | Uso                                                                       |
| -------------- | ------- | ------------------------------------------------------------------------- |
| ASP.NET Core   | 9.0     | Backend Razor Pages                                                       |
| Syncfusion EJ2 | 31.1.22 | Componentes UI (DropDowns, ComboBox, Uploader, PDFViewer, RichTextEditor) |
| jQuery         | 3.x     | Manipulação DOM e AJAX                                                    |
| Bootstrap      | 5.3.8   | Layout responsivo e modais                                                |
| jsPDF          | 2.5.1   | Geração de PDF da ficha de vistoria                                       |

### Padrões de Design

- **Razor Pages**: Padrão MVVM com PageModel
- **Repository Pattern**: Acesso a dados através de repositories
- **Dependency Injection**: Injeção de dependências no PageModel
- **AJAX**: Comunicação assíncrona com API

---

## Estrutura de Arquivos

### Arquivo Principal

```
Pages/Multa/UpsertAutuacao.cshtml
```

### Arquivos Relacionados

- `Pages/Multa/UpsertAutuacao.cshtml.cs` - PageModel com lógica de backend
- `wwwroot/js/cadastros/upsert_autuacao.js` - JavaScript específico da página
- `Controllers/MultaController.cs` - API endpoints relacionados

---

## Lógica de Negócio

### Campos Principais

#### Dados da Autuação

- **Número da Infração**: Identificador único da autuação
- **Data da Infração**: Data em que ocorreu a infração
- **Hora da Infração**: Hora em que ocorreu a infração
- **Data de Notificação**: Data em que foi notificado
- **Data Limite**: Data limite para pagamento
- **Localização**: Local onde ocorreu a infração
- **Valor Até Vencimento**: Valor se pago até vencimento
- **Valor Pós Vencimento**: Valor se pago após vencimento

#### Vínculos

- **Tipo de Infração**: Tipo de multa cadastrado
- **Status**: Status atual da autuação
- **Órgão Autuante**: Órgão que aplicou a multa
- **Empenho**: Empenho para pagamento da multa
- **Veículo**: Veículo autuado
- **Motorista**: Motorista responsável
- **Contrato/Ata**: Instrumento contratual do veículo/motorista
- **Ficha de Vistoria**: Número da ficha de vistoria relacionada

#### Documentos

- **PDF da Notificação**: Upload do documento oficial de autuação
- **Observações**: Campo rich text para observações gerais

---

## Interconexões

### Quem Chama Esta Página

- `Pages/Multa/ListaAutuacao.cshtml` → Botão "Nova Autuação" ou "Editar"
- Navegação direta via menu

### O Que Esta Página Chama

- **API MultaController**:

  - `/api/Multa/MultaExistente` - Verifica se número de infração já existe
  - `/api/Multa/PegaInstrumentoVeiculo` - Busca contrato/ata do veículo
  - `/api/Multa/ValidaContratoVeiculo` - Valida contrato do veículo
  - `/api/Multa/ValidaAtaVeiculo` - Valida ata do veículo
  - `/api/Multa/PegaContratoMotorista` - Busca contrato do motorista
  - `/api/Multa/ValidaContratoMotorista` - Valida contrato do motorista
  - `/api/Multa/ProcuraViagem` - Busca viagem pela data/hora/veículo
  - `/api/Multa/ProcuraFicha` - Busca ficha de vistoria
  - `/api/Multa/PegaImagemFichaVistoria` - Obtém imagem da ficha
  - `/api/MultaUpload/save` - Upload de PDF
  - `/api/MultaUpload/remove` - Remove PDF
  - `/api/MultaPdfViewer` - Serviço do PDF Viewer

- **PageModel Handlers**:

  - `/Multa/UpsertAutuacao?handler=AJAXPreencheListaEmpenhos` - Lista empenhos por órgão
  - `/Multa/UpsertAutuacao?handler=PegaSaldoEmpenho` - Obtém saldo do empenho
  - `/Multa/UpsertAutuacao?handler=Submit` - Cria nova autuação
  - `/Multa/UpsertAutuacao?handler=Edit` - Atualiza autuação existente

- **API ViagemController**:
  - `/api/Viagem/SaveImage` - Upload de imagens no RichTextEditor
  - `/api/Viagem/PegaFichaModal` - Busca dados da ficha para modal

---

## Endpoints API

### GET `/api/Multa/MultaExistente`

**Descrição**: Verifica se já existe autuação com o número de infração informado

**Parâmetros de Query**:

- `numeroInfracao` (string): Número da infração

**Response**:

```json
{
  "success": true,
  "data": [true] // true se existe, false se não
}
```

---

### GET `/api/Multa/PegaInstrumentoVeiculo`

**Descrição**: Busca o contrato ou ata associado ao veículo

**Parâmetros de Query**:

- `Id` (Guid): ID do veículo

**Response**:

```json
{
  "success": true,
  "instrumentoid": "guid-do-contrato-ou-ata",
  "instrumento": "contrato" // ou "ata"
}
```

---

### POST `/api/Multa/ProcuraViagem`

**Descrição**: Busca viagem baseada em data, hora e veículo

**Request Body**:

```json
{
  "Data": "2026-01-12",
  "Hora": "14:30",
  "VeiculoId": "guid-do-veiculo"
}
```

**Response**:

```json
{
  "success": true,
  "viagemid": "guid-da-viagem",
  "nofichavistoria": 12345,
  "motoristaid": "guid-do-motorista"
}
```

---

### POST `/api/Multa/ProcuraFicha`

**Descrição**: Busca ficha de vistoria pelo número

**Request Body**:

```json
{
  "NoFichaVistoria": 12345
}
```

**Response**:

```json
{
  "success": true,
  "viagemid": "guid-da-viagem"
}
```

---

### GET `/api/Multa/PegaImagemFichaVistoria`

**Descrição**: Obtém imagem da ficha de vistoria em base64

**Parâmetros de Query**:

- `noFicha` (int): Número da ficha

**Response**:

```json
{
  "success": true,
  "imagemBase64": "data:image/png;base64,...",
  "noFichaVistoria": 12345
}
```

---

## Frontend

### Estrutura HTML

#### Card Principal

```html
<div class="panel ftx-card-styled">
  <div class="ftx-card-header">
    <h2 class="titulo-paginas">
      <i class="fa-duotone fa-user-police-tie"></i>
      Criar/Atualizar Autuação
    </h2>
  </div>
  <div class="panel-content">
    <!-- Formulário -->
  </div>
</div>
```

#### Componentes Syncfusion Principais

- **DropDownList**: Infração, Empenhos
- **ComboBox**: Status, Órgão, Veículo, Motorista, Contratos, Atas
- **Uploader**: Upload de PDF
- **PDFViewer**: Visualização do PDF da autuação
- **RichTextEditor**: Campo de observações

#### Modal Ficha de Vistoria

```html
<div class="modal fade" id="modalFicha">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
      <div class="modal-header">
        <h4 id="DynamicModalLabelFicha">Ficha de Vistoria</h4>
      </div>
      <div class="modal-body">
        <img id="imgFichaVistoria" />
      </div>
      <div class="modal-footer">
        <button id="btnBaixarPDF">Baixar PDF</button>
        <button data-bs-dismiss="modal">Fechar</button>
      </div>
    </div>
  </div>
</div>
```

### JavaScript Principal

#### Configuração de Cultura Syncfusion

```javascript
// REMOVIDO: Configuração duplicada que causava erro
// ej.base.setCulture('pt-BR');
// ej.base.setCurrencyCode('BRL');
// Configuração já feita globalmente em _ScriptsBasePlugins.cshtml
```

**IMPORTANTE**: A configuração de cultura do Syncfusion é feita de forma assíncrona em `_ScriptsBasePlugins.cshtml` após carregar os dados CLDR. Não deve ser duplicada nas páginas individuais.

#### Formatação de Moeda

```javascript
// Formata número para moeda BR
function formatCurrencyBR(valor) {
  return (
    "R$ " +
    parseFloat(valor).toLocaleString("pt-BR", {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    })
  );
}

// Converte moeda BR para número
function parseCurrencyBR(str) {
  return (
    parseFloat(
      str
        .replace(/\s/g, "")
        .replace("R$", "")
        .replace(/\./g, "")
        .replace(",", ".")
    ) || 0
  );
}
```

#### Upload de PDF

```javascript
function onAutuacaoUploadSuccess(args) {
  const serverResponse = JSON.parse(args.e.target.response);
  const fileName = serverResponse.files[0].name;

  // Atualiza campo hidden com nome do arquivo
  $("#txtAutuacaoPDF").val(fileName);

  // Carrega no viewer
  loadPdfInViewer(fileName);
}
```

#### Busca de Viagem

```javascript
$(".btnViagem").click(function () {
  $.ajax({
    type: "POST",
    url: "/api/Multa/ProcuraViagem",
    data: {
      Data: $("#txtDataInfracao").val(),
      Hora: $("#txtHoraInfracao").val(),
      VeiculoId: lstVeiculo.value,
    },
    success: function (data) {
      if (data.success) {
        $("#txtNoFichaVistoria").val(data.nofichavistoria);
        lstMotorista.value = data.motoristaid;
      }
    },
  });
});
```

### CSS Customizado

#### Variáveis de Cores

```css
:root {
  --ftx-autuacao-azul: #3d5771;
  --ftx-autuacao-azul-dark: #2d4559;
  --ftx-autuacao-vinho: #722f37;
  --ftx-autuacao-verde: #2e8b57;
  --ftx-autuacao-terracota: #a0522d;
}
```

#### Botões com Animação

```css
.btn-azul:hover {
  animation: buttonWiggle 0.5s ease-in-out;
}

@keyframes buttonWiggle {
  0% {
    transform: translateY(0) rotate(0deg);
  }
  25% {
    transform: translateY(-2px) rotate(-1deg);
  }
  50% {
    transform: translateY(-3px) rotate(0deg);
  }
  75% {
    transform: translateY(-2px) rotate(1deg);
  }
  100% {
    transform: translateY(0) rotate(0deg);
  }
}
```

---

## Validações

### Frontend

- **Número de Infração**: Obrigatório, verifica duplicidade via AJAX
- **Data da Infração**: Obrigatória para busca de viagem
- **Hora da Infração**: Obrigatória para busca de viagem
- **Veículo**: Obrigatório para busca de viagem
- **Valores**: Formatação de moeda brasileira

### Backend (Model)

- **Validações via DataAnnotations** no Model `Multa`
- **Validação de Contratos**: Veículo/Motorista devem pertencer ao contrato selecionado
- **Validação de Atas**: Veículo deve pertencer à ata selecionada

---

## Troubleshooting

### Problema: Erro "Cannot read properties of undefined (reading 'percentSign')"

**Sintoma**: Página trava ao carregar, erro no console do navegador relacionado ao Syncfusion

**Causa**: Configuração de cultura (`ej.base.setCulture`) sendo chamada antes dos dados CLDR serem carregados

**Solução**:

1. Remover configuração duplicada de cultura da página individual
2. Configuração é feita globalmente em `_ScriptsBasePlugins.cshtml` após carregar dados CLDR

**Código Relacionado**: Linhas 691-693 (anteriormente) do arquivo UpsertAutuacao.cshtml

**Status**: ✅ Corrigido em 12/01/2026

---

### Problema: PDF não carrega no viewer após upload

**Sintoma**: Upload bem-sucedido mas PDF não aparece no viewer

**Causa**: Nome do arquivo não está sendo atualizado no campo hidden `txtAutuacaoPDF`

**Diagnóstico**: Verificar no console se o callback `onAutuacaoUploadSuccess` está sendo executado

**Solução**:

1. Garantir que eventos do uploader estão vinculados:

```javascript
uploaderAutuacao.success = onAutuacaoUploadSuccess;
```

2. Verificar se campo hidden está sendo atualizado:

```javascript
$("#txtAutuacaoPDF").val(fileName);
```

---

### Problema: Viagem não encontrada

**Sintoma**: Ao clicar em "Procurar Viagem", retorna que viagem não foi encontrada

**Causa**: Não existe viagem cadastrada para o veículo na data/hora informada

**Solução**:

1. Verificar se existe viagem cadastrada para o veículo
2. Confirmar data e hora corretas da infração
3. Sistema busca viagem que esteja ativa no momento da infração

---

### Problema: Modal de ficha não abre

**Sintoma**: Ao clicar no botão da ficha, nada acontece

**Causa**: Bootstrap 5 usa nova API de modais

**Solução**: Verificar se modal está sendo aberto corretamente:

```javascript
const modal = new bootstrap.Modal(document.getElementById("modalFicha"));
modal.show();
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [16/01/2026 - 15:00] - Refatoração UI/UX e Editor Kendo

**Descrição**:
Refatoração da página para melhoria de usabilidade, validação de dados e padronização do editor de texto rico.

**Mudanças**:

1. **Campos Obrigatórios**: Adição de asteriscos vermelhos (\*) via CSS `label-required` e sincronização com `[Required]` no Model.
2. **Kendo Editor**: Substituição do Syncfusion RichTextEditor pelo Telerik Kendo Editor, padronizando com o restante do sistema.
3. **Camel Case Dinâmico**: Conversão automática de Número de Infração e Localização para Capitalização de Palavras (Title Case) durante a digitação.
4. **Validação de Datas**: Bloqueio de datas fora de ordem cronológica (Infração -> Notificação -> Limite) com foco no aviso do controle incorreto.
5. **Correção de Ícones**: Atualização de `fa-thin` para `fa-duotone` seguindo o design system do projeto.

**Impacto**:

- Melhor integridade de dados via validação de backend.
- UI mais clara indicando campos obrigatórios.
- Consistência de ferramentas de edição de texto.

---

## [12/01/2026 - 16:35] - Correção de RichTextEditor não renderizando

**Descrição**:
Campo de Observações renderizava como textarea simples em vez do RichTextEditor completo com barra de ferramentas.

**Causa**:
RichTextEditor tinha atributo `locale="pt-BR"` que tentava usar cultura antes dos dados CLDR estarem carregados, causando falha na renderização do componente.

**Solução Implementada**:
Removido atributo `locale="pt-BR"` do RichTextEditor na linha 571:

```html
<!-- ANTES (não renderizava): -->
<ejs-richtexteditor
  ejs-for="@Model.MultaObj.Multa.Observacao"
  id="rte"
  toolbarClick="toolbarClick"
  locale="pt-BR"
  height="150px"
>
  <!-- DEPOIS (renderiza corretamente): -->
  <ejs-richtexteditor
    ejs-for="@Model.MultaObj.Multa.Observacao"
    id="rte"
    toolbarClick="toolbarClick"
    height="150px"
  ></ejs-richtexteditor
></ejs-richtexteditor>
```

O componente agora usa a cultura configurada globalmente após CLDR estar pronto.

**Arquivos Afetados**:

- `Pages/Multa/UpsertAutuacao.cshtml` (linha 571)

**Impacto**:

- ✅ RichTextEditor renderiza corretamente com barra de ferramentas
- ✅ Funcionalidades de formatação disponíveis (negrito, itálico, listas, imagens, etc.)
- ✅ Editor funciona em português brasileiro

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.0

---

## [12/01/2026 - 16:30] - Correção de erro ao carregar página (percentSign undefined)

**Descrição**:
Página travava ao carregar com erro no console: `TypeError: Cannot read properties of undefined (reading 'percentSign')`.

**Causa**:
Código estava configurando cultura brasileira do Syncfusion (`ej.base.setCulture('pt-BR')`) antes que os dados CLDR (Common Locale Data Repository) fossem carregados. O Syncfusion tentava acessar propriedades de formatação de número que ainda não existiam.

**Solução Implementada**:
Removidas as linhas 691-693 que configuravam cultura duplicadamente:

```javascript
// ANTES (causava erro):
ej.base.setCulture('pt-BR');
ej.base.setCurrencyCode('BRL');

// DEPOIS (comentário explicativo):
<!-- Configuração de cultura já feita globalmente em _ScriptsBasePlugins.cshtml -->
```

A configuração de cultura já é feita corretamente de forma assíncrona em `Pages/Shared/_ScriptsBasePlugins.cshtml`, que:

1. Carrega dados CLDR de múltiplas fontes com retry
2. Aplica configuração de cultura após dados estarem prontos
3. Tem fallback com dados mínimos caso CDN falhe

**Arquivos Afetados**:

- `Pages/Multa/UpsertAutuacao.cshtml` (linhas 690-693)

**Impacto**:

- ✅ Página agora carrega sem erros
- ✅ Componentes Syncfusion funcionam corretamente com cultura pt-BR
- ✅ Formatação de datas, números e moeda em português brasileiro

**Status**: ✅ **Concluído**

**Responsável**: Claude Sonnet 4.5

**Versão**: 1.0

---

## Histórico de Versões

| Versão | Data       | Descrição                                                                                  |
| ------ | ---------- | ------------------------------------------------------------------------------------------ |
| 1.0    | 12/01/2026 | Versão inicial da documentação - Correção erro percentSign                                 |
| 1.2    | 16/01/2026 | Refinamento da validação de datas para focar no controle editado (incluindo Data Infração) |
| 1.3    | 16/01/2026 | Campo "Valor Até Vencimento" marcado como obrigatório (asterisco + model validation)       |

---

## Referências

- [Syncfusion EJ2 Documentation](https://ej2.syncfusion.com/documentation/)
- [CLDR - Common Locale Data Repository](http://cldr.unicode.org/)
- [Bootstrap 5 Modal Documentation](https://getbootstrap.com/docs/5.3/components/modal/)
- [jsPDF Documentation](https://github.com/parallax/jsPDF)

---

**Última atualização**: 16/01/2026
**Autor**: GitHub Copilot (Gemini 3 Flash)
**Versão**: 1.3
