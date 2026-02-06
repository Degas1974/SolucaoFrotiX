# Documentação: Administração - Ajuste de Custos de Viagem

> **Última Atualização**: 06/01/2026
> **Versão Atual**: 1.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Funcionalidades Específicas](#funcionalidades-específicas)
4. [Endpoints API](#endpoints-api)
5. [Frontend](#frontend)
6. [Troubleshooting](#troubleshooting)

---

## Visão Geral

A página **Ajuste nos Dados das Viagens** permite ao administrador visualizar e editar detalhes críticos de viagens já cadastradas, como quilometragem, motorista, veículo, horários e finalidade. É utilizada principalmente para correção de lançamentos errados que afetam o cálculo de custos.

### Características Principais

- ✅ **Listagem Detalhada**: Exibe viagens com dados de auditoria (Vistoria, KM, Datas).
- ✅ **Edição Completa**: Modal para alterar praticamente todos os campos da viagem.
- ✅ **Visualização de Ficha**: Modal para visualizar a imagem da Ficha de Vistoria digitalizada.
- ✅ **Integração Syncfusion**: Comboboxes e DropDownTrees para seleção de dados.

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/
│   └── Administracao/
│       ├── AjustaCustosViagem.cshtml        # View (HTML + Razor)
│       └── AjustaCustosViagem.cshtml.cs     # PageModel (Carregamento de Listas)
│
├── Controllers/
│   └── CustosViagemController.cs            # API Principal
│
├── wwwroot/
│   ├── js/
│   │   └── cadastros/
│   │       └── atualizacustosviagem.js      # Lógica Frontend
```

### Tecnologias Utilizadas

| Tecnologia | Uso |
|------------|-----|
| **jQuery DataTables** | Grid de listagem |
| **Syncfusion EJ2** | Comboboxes, DropDownTree |
| **Bootstrap 5** | Modais |
| **ASP.NET Core API** | Backend de dados |

---

## Funcionalidades Específicas

### 1. Listagem de Viagens
Carrega dados da view `ViewCustosViagem` via API.

**Inicialização do DataTable (`atualizacustosviagem.js`)**:
```javascript
function initDataTable() {
    $("#tblViagem").DataTable({
        ajax: {
            url: "/api/custosviagem",
            type: "GET",
            dataSrc: function (json) {
                return json.data || [];
            }
        },
        columns: [
            { data: "noFichaVistoria" },
            { data: "dataInicial" },
            // ... outras colunas
            {
                data: "viagemId",
                render: function (data) {
                    return `<button ... data-id="${data}" ...><i class="fa-duotone fa-pen-to-square"></i></button>
                            <button ... data-id="${data}" ...><i class="fa-duotone fa-file-image"></i></button>`;
                }
            }
        ]
    });
}
```

### 2. Edição de Viagem (Modal Ajusta Custos)
Permite alterar dados da viagem. Ao abrir o modal, os dados são carregados via AJAX.

**Carregamento de Dados (`carregarDadosViagem`)**:
```javascript
function carregarDadosViagem(viagemId) {
    $.ajax({
        type: "GET",
        url: "/api/Viagem/GetViagem/" + viagemId,
        success: function (res) {
            const viagem = res.data;
            document.getElementById("txtId").value = viagem.viagemId;
            document.getElementById("txtNoFichaVistoria").value = viagem.noFichaVistoria;

            // Setar valores Syncfusion
            const lstMotorista = document.getElementById("lstMotoristaAlterado");
            if (lstMotorista && lstMotorista.ej2_instances) {
                lstMotorista.ej2_instances[0].value = viagem.motoristaId;
            }
            // ... outros campos
        }
    });
}
```

**Gravação (`gravarViagem`)**:
```javascript
function gravarViagem() {
    const dados = {
        ViagemId: document.getElementById("txtId").value,
        NoFichaVistoria: document.getElementById("txtNoFichaVistoria").value,
        // ... coleta todos os campos
    };

    $.ajax({
        type: "POST",
        url: "/api/Viagem/AtualizarDadosViagemDashboard",
        contentType: "application/json",
        data: JSON.stringify(dados),
        success: function (res) {
            if (res.success) {
                AppToast.show("Verde", "Viagem atualizada com sucesso!", 3000);
                $("#tblViagem").DataTable().ajax.reload();
            }
        }
    });
}
```

### 3. Visualização de Ficha de Vistoria
Modal que exibe a imagem (BLOB) da ficha de vistoria armazenada no banco.

**Endpoint Backend (`CustosViagemController.cs`)**:
```csharp
[HttpGet]
[Route("PegaFichaModal")]
public JsonResult PegaFichaModal(Guid id)
{
    var objFromDb = _unitOfWork.Viagem.GetFirstOrDefault(u => u.ViagemId == id);
    if (objFromDb.FichaVistoria != null)
    {
        // Converte bytes para Base64 para exibição
        objFromDb.FichaVistoria = this.GetImage(Convert.ToBase64String(objFromDb.FichaVistoria));
        return Json(objFromDb.FichaVistoria);
    }
    return Json(false);
}
```

---

## Endpoints API

### 1. GET `/api/custosviagem`
Retorna a lista de viagens para o grid.

### 2. GET `/api/Viagem/GetViagem/{id}`
Retorna os detalhes de uma viagem específica para edição.

### 3. POST `/api/Viagem/AtualizarDadosViagemDashboard`
Recebe o objeto JSON com os dados editados e atualiza o registro.

### 4. GET `/api/CustosViagem/PegaFichaModal`
Retorna a imagem da ficha de vistoria em Base64 (ou array de bytes tratado).

---

## Frontend

### Controles Syncfusion
A página utiliza diversos controles EJ2:
- `ejs-combobox`: Para Motorista, Veículo, Requisitante, Finalidade.
- `ejs-dropdowntree`: Para Setor Solicitante e Evento.

### Lógica de Evento vs Finalidade
Um script controla a visibilidade do campo "Evento" baseado na "Finalidade".

```javascript
function FinalidadeChange() {
    var finalidadeCb = document.getElementById('lstFinalidadeAlterada').ej2_instances[0];
    var eventoDdt = document.getElementById('lstEvento').ej2_instances[0];

    if (finalidadeCb.value === 'Evento') {
        eventoDdt.enabled = true;
        $(".esconde-diveventos").show();
    } else {
        eventoDdt.enabled = false;
        eventoDdt.value = null;
        $(".esconde-diveventos").hide();
    }
}
```

---

## Troubleshooting

### Problema: Campos Syncfusion vazios ao abrir modal
**Causa**: O modal abre antes dos controles Syncfusion serem renderizados ou o AJAX retorna antes deles estarem prontos.
**Solução**: O código usa um `setTimeout` de 300ms em `carregarDadosViagem` para garantir que as instâncias EJ2 estejam disponíveis antes de setar os valores.

### Problema: Imagem da ficha não carrega
**Causa**: Viagem sem ficha ou erro na conversão Base64.
**Comportamento**: O sistema exibe uma imagem padrão `/Images/FichaAmarelaNova.jpg` caso não encontre a ficha no banco.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [06/01/2026] - Criação da Documentação

**Descrição**:
Documentação inicial da página de Ajuste de Custos de Viagem.

**Status**: ✅ **Documentado**

**Responsável**: Claude (AI Assistant)
**Versão**: 1.0


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

## [21/01/2026] - PadronizaÃ§Ã£o de Nomenclatura

**DescriÃ§Ã£o**: Renomeada coluna "AÃ§Ã£o" para "AÃ§Ãµes" no cabeÃ§alho do DataTable para padronizaÃ§Ã£o do sistema

**Arquivos Afetados**:
- Arquivo .cshtml correspondente

**Impacto**: AlteraÃ§Ã£o cosmÃ©tica, sem impacto funcional

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema

**VersÃ£o**: Atual

---

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
