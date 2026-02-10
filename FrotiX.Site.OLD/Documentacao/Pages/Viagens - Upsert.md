# Documentação: Alertas FrotiX (Upsert)

> **Última Atualização**: 16/01/2026 18:15
> **Versão Atual**: 1.4

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Estrutura da Interface](#estrutura-da-interface)
4. [Lógica de Frontend (JavaScript)](#lógica-de-frontend-javascript)
5. [Endpoints API](#endpoints-api)
6. [Troubleshooting](#troubleshooting)

---

## Visão Geral

A página **Upsert de Viagens** (`Pages/Viagens/Upsert.cshtml`) é responsável pela criação e edição de viagens. Diferente de um CRUD simples, esta página lida com uma lógica de negócios complexa, incluindo validação cruzada de datas/horas, cálculo automático de duração, integração com fichas de vistoria (upload de imagem) e registro de múltiplas ocorrências.

### Características Principais
- ✅ **Formulário Segmentado**: Dividido em seções (Período, Trajeto, Motorista/Veículo, Combustível, Documentos, Solicitante).
- ✅ **Validação Dinâmica**: Verifica disponibilidade de veículo e motorista em tempo real (via componentes Syncfusion).
- ✅ **Ficha de Vistoria**: Upload com preview e zoom de imagem.
- ✅ **Integração Mobile**: Se a viagem foi criada via App, exibe rubricas e dados específicos.
- ✅ **Gestão de Ocorrências**: Permite adicionar múltiplas ocorrências durante a criação/edição.
- ✅ **Bloqueio de Edição**: Viagens com status "Realizada" ou "Cancelada" são abertas em modo somente leitura.

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/
│   └── Viagens/
│       └── Upsert.cshtml            # View do formulário
│
├── Controllers/
│   └── ViagemController.cs          # Endpoints API (Submit, UploadFicha)
│
├── wwwroot/
│   ├── js/
│   │   └── cadastros/
│   │       └── ViagemUpsert.js      # Lógica de validação e submissão
│   │   └── viagens/
│   │       └── kendo-editor-upsert.js # Editor de texto rico para descrição
```

### Tecnologias Utilizadas

| Tecnologia | Uso |
|------------|-----|
| **ASP.NET Core Razor Pages** | Renderização do formulário |
| **Syncfusion EJ2** | Dropdowns, ComboBox, TimePicker |
| **Kendo UI Editor** | Editor de texto rico para descrição |
| **Bootstrap 5** | Layout e Modais |
| **jQuery** | Manipulação do DOM e AJAX |

---

## Estrutura da Interface

A view `Upsert.cshtml` é organizada em cards temáticos para facilitar o preenchimento.

### 1. Período da Viagem
Campos de Data/Hora Inicial e Final. O sistema calcula automaticamente a duração.

```html
<div class="ftx-section ftx-section-periodo">
    <div class="row g-3">
        <div class="col-6 col-md-2">
            <label>Data Inicial</label>
            <input id="txtDataInicial" type="date" class="form-control" />
        </div>
        <!-- ... -->
        <div class="col-6 col-md-2">
            <label>Duração</label>
            <input id="txtDuracao" class="form-control ftx-calculated" disabled />
        </div>
    </div>
</div>
```

### 2. Motorista e Veículo
Usa `ejs-combobox` com templates customizados para mostrar a foto do motorista.

```html
<ejs-combobox id="cmbMotorista"
              dataSource="@ViewData["dataMotorista"]"
              created="onCmbMotoristaCreated"
              change="MotoristaValueChange">
    <e-combobox-fields text="Nome" value="MotoristaId"></e-combobox-fields>
</ejs-combobox>
```

### 3. Ficha de Vistoria
Seção dedicada ao upload de imagem da ficha de papel digitalizada.

```html
<input type="file" id="txtFile" accept="image/*" onchange="VisualizaImagem(this)" />
<img id="imgViewerItem" src="..." class="img-thumbnail" />
```

---

## Lógica de Frontend (JavaScript)

O arquivo `ViagemUpsert.js` (e scripts inline) gerencia a complexidade da página.

### Validação de Duplicidade (Fuzzy Logic)
O script inclui algoritmos de Levenshtein para sugerir se um destino digitado ("Aeroporto") já existe na lista ("Aeroporto Intl").

```javascript
function similarity(a, b) {
    // Calcula similaridade entre strings para evitar duplicação de cadastros
    // ex: "São Paulo" vs "Sao Paulo"
}
```

### Cálculo de Duração
Ao alterar datas ou horas, a duração é recalculada instantaneamente.

```javascript
document.getElementById("txtHoraFinal")?.addEventListener("focusout", calcularDuracaoViagem);

function calcularDuracaoViagem() {
    // Usa moment.js ou Date nativo para diff
    // Atualiza o campo #txtDuracao
}
```

### Proteção de Dados
O sistema detecta alterações no formulário e alerta o usuário se ele tentar sair sem salvar.

```javascript
$('form').on('change input', 'input, select, textarea', function () {
    formularioAlterado = true;
});

$('#btnVoltarLista').on('click', function (e) {
    if (formularioAlterado) {
        Alerta.Confirmar("Descartar Alterações?", ...);
    }
});
```

---

## Endpoints API

### POST `/Viagens/Upsert` (Handler Razor)
Processa o formulário principal.
- **Model**: `ViagemViewModel`.
- **Lógica**: Se `ViagemId` for vazio, cria; senão, atualiza.

### POST `/api/Viagem/UploadFichaVistoria`
Recebe o arquivo de imagem via AJAX `FormData`.
- **Parâmetros**: `arquivo` (IFormFile), `viagemId` (Guid).
- **Retorno**: JSON `{ success: true, message: "..." }`.

### GET `/api/Viagem/FotoMotorista?id={id}`
Retorna a foto do motorista em Base64 para o template do ComboBox.

---

## Troubleshooting

### Erro "Request Verification Token"
**Causa**: O upload de imagem no editor Kendo/Syncfusion não está enviando o token CSRF.
**Solução**: O script `toolbarClick` injeta o header `XSRF-TOKEN` manualmente antes do upload.

### Combo de Motorista sem foto
**Causa**: O endpoint `/api/Viagem/FotoMotorista` retornou 404 ou null.
**Solução**: O template JS tem um fallback para `/images/barbudo.jpg` (placeholder padrão).

### Campos desabilitados
**Causa**: A viagem está com status "Realizada" ou "Cancelada".
**Lógica**: O Razor renderiza a view verificando `Model.ViagemObj.Viagem.Status`. Se finalizada, botões de salvar são ocultados e inputs podem ser renderizados como `disabled`.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [16/01/2026 18:15] - REFACTOR: Migração para Comparador Compartilhado

**Descrição**: Migração da classe `NaturalStringComparer` local para `Helpers/ListasCompartilhadas.cs`, tornando-a reutilizável em todo o sistema.

**Mudanças**:
1. **Removida classe local** (linhas 2039-2097): Deletada classe `NaturalStringComparer` que estava duplicada
2. **Atualizado uso** (linhas 455, 1649): Alterado para usar `FrotiX.Helpers.NaturalStringComparer()`
3. **Benefício**: Código DRY (Don't Repeat Yourself), facilita manutenção futura

**Arquivos Afetados**:
- `Pages/Viagens/Upsert.cshtml.cs` (linhas 455, 1649, deletado 2039-2097)
- `Helpers/ListasCompartilhadas.cs` (linhas 29-92) [novo comparador]

**Impacto**: Ordenação continua funcionando corretamente, mas agora usa código compartilhado.

**Status**: ✅ **Concluído**

**Versão**: 1.4

---

## [16/01/2026] - FIX: Ordenação Alfabética de Requisitantes (pt-BR)

**Descrição**: Corrigida ordenação alfabética da lista de requisitantes para respeitar cultura pt-BR (case-insensitive, ignora acentos).

**Problema**:
- Método `OnGetAJAXPreencheListaRequisitantes()` usava ordenação SQL padrão
- Ordenação SQL não respeitava cultura pt-BR
- Resultado: Lista com ordenação inconsistente (ex: "001" em posições aleatórias)

**Solução** (linhas 443-456):
- Removida ordenação do banco de dados
- Implementada ordenação em memória com `StringComparer.Create(new CultureInfo("pt-BR"), ignoreCase: true)`
- Garante consistência com ordenação JavaScript (`localeCompare('pt-BR')`)

**Benefícios**:
1. Ordenação consistente (pt-BR)
2. Case-insensitive ("André" e "andre" juntos)
3. Ignora acentos na comparação
4. Compatível com ordenação do frontend

**Arquivos Afetados**:
- `Pages/Viagens/Upsert.cshtml.cs` (linhas 443-456)

**Status**: ✅ **Concluído**

**Versão**: 1.3

---

## [13/01/2026 18:30] - Fase 4: Padronização btn-outline-secondary → btn-vinho

**Descrição**: Substituída classe Bootstrap outline genérica `btn-outline-secondary` por `btn-vinho` (padrão FrotiX sólido) em botão de limpar imagem de ocorrência.

**Problema Identificado**:
- Uso de classe Bootstrap outline genérica `btn-outline-secondary` em botão de limpar imagem
- Inconsistência com padrão FrotiX que define `btn-vinho` para ações de cancelar/limpar
- Botão outline não seguia visual consistente do sistema

**Solução Implementada**:
- Botão "Limpar Imagem Ocorrência" (linha 2505): mudado de `btn-outline-secondary` para `btn-vinho`
- Limpar imagem é semanticamente uma ação de "cancelar/remover" conteúdo
- Alinhamento com diretrizes FrotiX: ações de cancelar/limpar usam `btn-vinho`
- Consistência com Fases 1, 2, 3 e restante da Fase 4 do projeto de padronização

**Arquivos Afetados**:
- Pages/Viagens/Upsert.cshtml (linha 2505)

**Impacto**:
- ✅ Botão mantém cor vinho consistente ao pressionar
- ✅ Semântica visual correta (limpar = cancelar/remover)
- ✅ Alinhamento com padrão visual FrotiX
- ✅ Consistência em todo o sistema

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.2

---

## [13/01/2026 15:30] - Padronização: Substituição de btn-ftx-fechar por btn-vinho

**Descrição**: Substituída classe `btn-ftx-fechar` por `btn-vinho` em botões de cancelar/fechar operação.

**Problema Identificado**:
- Classe `btn-ftx-fechar` não tinha `background-color` definido no estado `:active`
- Botões ficavam BRANCOS ao serem pressionados (em vez de manter cor rosada/vinho)
- Comportamento visual inconsistente com padrão FrotiX

**Solução Implementada**:
- Todos os botões cancelar/fechar padronizados para usar classe `.btn-vinho`
- Classe `.btn-vinho` já possui `background-color: #4a1f24` no estado `:active`
- Garantia de cor rosada/vinho ao pressionar botão

**Arquivos Afetados**:
- Pages/Viagens/Upsert.cshtml - Substituição de `btn-ftx-fechar` por `btn-vinho` em botão de modal

**Impacto**:
- ✅ Botão mantém cor rosada/vinho ao ser pressionado
- ✅ Alinhamento com padrão visual FrotiX
- ✅ Consistência em todo o sistema

**Status**: ✅ **Concluído**

**Responsável**: Claude Code

**Versão**: 1.1

---
## [06/01/2026] - Criação da Documentação

**Descrição**:
Documentação inicial do módulo de Upsert de Viagens, cobrindo formulário, validações e upload de ficha.

**Status**: ✅ **Documentado**

**Responsável**: Claude (AI Assistant)
**Versão**: 1.0


## [16/01/2026 19:20] - FIX: Trim em nomes para corrigir ordenação

Adicionado `.Trim()` nos métodos `OnGetAJAXPreencheListaRequisitantes()` e `PreencheListaRequisitantes()` para eliminar espaços em branco.

**Linhas**: 448, 456, 1644, 1651

**Versão**: 1.5

