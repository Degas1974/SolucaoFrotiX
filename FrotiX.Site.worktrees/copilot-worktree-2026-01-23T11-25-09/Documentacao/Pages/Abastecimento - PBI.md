# Documentação: Abastecimento - Power BI (PBI)

> **Última Atualização**: 16/01/2026
> **Versão Atual**: 1.1

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura](#arquitetura)
3. [Funcionalidades Específicas](#funcionalidades-específicas)
4. [Frontend](#frontend)
5. [Troubleshooting](#troubleshooting)

---

## Visão Geral

A página **Abastecimento - PBI** exibe um relatório incorporado do Microsoft Power BI, focado na análise de abastecimentos por veículo. Ela permite a visualização interativa de métricas e gráficos gerados externamente na plataforma Power BI.

### Características Principais

- ✅ **Power BI Embedded**: Integração via `iframe` com relatório hospedado.
- ✅ **Modais de Cadastro Rápido**: Permite inserir Requisitantes e Setores Solicitantes sem sair da tela (embora o foco principal seja o relatório, o código inclui esses modais, possivelmente reutilizados ou legados).
- ✅ **Layout Fullscreen**: O iframe ocupa largura total e altura considerável (900px) para melhor visualização.

---

## Arquitetura

### Estrutura de Arquivos

```
FrotiX.Site/
├── Pages/
│   └── Abastecimento/
│       ├── PBI.cshtml               # View Principal (HTML + JS inline)
│       └── PBI.cshtml.cs            # PageModel (Backend)
│
├── wwwroot/
│   └── css/
│       └── frotix.css               # Estilos globais
```

### Tecnologias Utilizadas

| Tecnologia | Uso |
|------------|-----|
| **ASP.NET Core Razor Pages** | Estrutura da página |
| **Power BI** | Relatórios analíticos incorporados |
| **Syncfusion DropDownTree** | Seleção hierárquica de setores nos modais |
| **Bootstrap 5** | Modais e layout |

---

## Funcionalidades Específicas

### 1. Relatório Power BI
O núcleo da página é um `iframe` que carrega um relatório público ou autenticado do Power BI.

**Implementação**:
```html
<iframe title="Frotix Atualizado - Relatório por Veículo"
        width="100%"
        height="900"
        src="https://app.powerbi.com/view?r=eyJrIjoiNmVhZjNhYTktZGYwNy00NDdkLTk5NGEtNTc2NzU1OTUzMjEwIiwidCI6IjU2MjFkNjRmLTRjZjgtNDdmNS1iMzc5LTJiMmFiNzljMWM1ZiJ9"
        frameborder="0"
        allowFullScreen="true">
</iframe>
```

### 2. Modais de Cadastro (Requisitante/Setor)
A página inclui código para dois modais: "Inserir Novo Requisitante" e "Inserir Novo Setor Solicitante". Eles utilizam AJAX para submeter dados à API de Viagem (`/api/Viagem/AdicionarRequisitante`, `/api/Viagem/AdicionarSetor`).

> ⚠️ **Nota**: A presença destes modais nesta página específica de Power BI parece atípica e pode ser um resquício de copiar/colar de outra funcionalidade (como cadastro de viagens), já que a página exibe apenas o relatório. No entanto, eles estão no código fonte.

**Exemplo de Chamada AJAX (Inserir Setor)**:
```javascript
$.ajax({
    type: "post",
    url: "/api/Viagem/AdicionarSetor",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    data: objSetor,
    success: function (data) {
        AppToast.show('Verde', data.message);
        PreencheListaSetores();
        $("#modalSetor").hide();
    },
    error: function (data) {
        console.log(data);
    }
});
```

---

## Frontend

### Scripts Inline

A página contém scripts para manipulação dos modais e validação de formulários, além de integração com componentes Syncfusion (DropDownTree).

**Prevenção de Submit com Enter**:
```javascript
function stopEnterSubmitting(e) {
    if (e.keyCode == 13) {
        var src = e.srcElement || e.target;
        if (src.tagName.toLowerCase() != "div") {
            if (e.preventDefault) { e.preventDefault(); }
            else { e.returnValue = false; }
        }
    }
}
```

---

## Troubleshooting

### Problema: Relatório não carrega (Tela cinza/branca no iframe)
**Causa**: Link do Power BI expirado, sem permissão ou bloqueado por política de segurança (CORS/X-Frame-Options).
**Solução**: Verificar se a URL no `src` do iframe ainda é válida e pública.

### Problema: Modais não abrem
**Causa**: IDs duplicados ou falta de trigger button visível.
**Observação**: Não há botões visíveis no código HTML principal (`PBI.cshtml`) para abrir esses modais (`#modalRequisitante`, `#modalSetor`). Eles existem no DOM mas podem estar inacessíveis ao usuário final nesta tela.

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [16/01/2026] - Aplicação de Trim + NaturalStringComparer em métodos de Requisitantes

**Descrição**:
Aplicado padrão de Trim + NaturalStringComparer em 2 métodos que carregam lista de requisitantes:
- `OnGetAJAXPreencheListaRequisitantes()` - Handler AJAX para preenchimento de lista
- `PreencheListaRequisitantes()` - Método de preenchimento no OnGet

**Padrão Aplicado**:
1. Query banco sem orderBy (melhor performance)
2. `.ToList()` para materializar em memória
3. `.Select()` com `.Trim()` para remover espaços iniciais/finais
4. `.OrderBy()` com `NaturalStringComparer` (números antes de letras, case-insensitive, pt-BR)

**Motivo**:
- Remover espaços em branco que causam desordenação alfabética
- Garantir ordenação natural (001, 002, 003, ..., A, B, C)
- Consistência com padrão aplicado em todo o sistema

**Arquivos Afetados**:
- `Pages/Abastecimento/PBI.cshtml.cs` (2 métodos)

**Status**: ✅ **Concluído**

**Versão**: 1.1

---

## [06/01/2026] - Criação da Documentação

**Descrição**:
Documentação inicial da página de Power BI de Abastecimento.

**Status**: ✅ **Documentado**

**Responsável**: Claude (AI Assistant)
**Versão**: 1.0


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
