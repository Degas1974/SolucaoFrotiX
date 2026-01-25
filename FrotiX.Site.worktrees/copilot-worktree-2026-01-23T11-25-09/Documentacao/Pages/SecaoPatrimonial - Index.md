# Gestão de Seções Patrimoniais

As **Seções Patrimoniais** são sub-divisões dos Setores. Enquanto um Setor pode ser um "Departamento de Logística", uma Seção pode ser o "Arquivo Geral" ou o "Pátio de Manutenção". O SecaoController permite este nível granular de localização, essencial para vistorias de inventário detalhadas.

## 📍 Localização Granular

A seção é sempre dependente de um Setor. Esta relação Pai-Filho é rigorosamente aplicada em todos os endpoints de busca e cadastro.

### Pontos de Atenção na Implementação:

1.  **Combos Dependentes (Load on Demand):** 
    O método ListaSecoesCombo aceita um setorSelecionado via parâmetro. Isso permite que a interface do FrotiX filtre as seções em tempo real conforme o usuário escolhe o setor de origem ou destino em uma movimentação.
    
2.  **Join Automático na Listagem:**
    Diferente de consultas simples, o ListaSecoes sempre traz o nome do **Setor Pai** vinculado, garantindo o contexto geográfico do bem sem a necessidade de o usuário memorizar IDs.

3.  **Integridade de Status:**
    Embora uma seção possa ser desativada individualmente, sua visibilidade nas buscas de movimentação depende também do status do Setor Pai, garantindo consistência na árvore de localização.

## 🛠 Snippets de Lógica Principal

### Filtragem Reativa de Seções por Setor
Implementação padrão para alimentar dropdowns dinâmicos no frontend:

`csharp
[HttpGet("ListaSecoesCombo")]
public IActionResult ListaSecoesCombo(Guid? setorSelecionado)
{
    if (!setorSelecionado.HasValue) return Json(new { success = true, data = new List<object>() });

    var secoes = _unitOfWork.SecaoPatrimonial.GetAll()
        .Where(s => s.SetorId == setorSelecionado && s.Status == true)
        .OrderBy(s => s.NomeSecao)
        .Select(s => new { text = s.NomeSecao, value = s.SecaoId })
        .ToList();

    return Json(new { success = true, data = secoes });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Exceções:** Todas as chamadas ao repositório são protegidas por blocos 	ry-catch com injeção de logs detalhados, registrando o arquivo SecaoController.cs na base de erros.
- **Normalização de Retorno:** Retornos formatados para o padrão success/data, facilitando o consumo por componentes modernos de Grid e Select no frontend.
- **Preservação de Histórico:** Desativar uma seção não remove os bens vinculados a ela em registros históricos, preservando a integridade do inventário pretérito.


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
