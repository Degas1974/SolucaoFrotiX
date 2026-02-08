# Padronização de Marcas de Veículos

A tabela de **Marcas** é o primeiro nível de classificação da frota no FrotiX. O MarcaVeiculoController garante que todos os veículos cadastrados sigam uma nomenclatura padronizada, fundamental para gerar estatísticas de manutenção por fabricante e relatórios de desempenho de combustível.

## 🏷 Estrutura de Dependência

No FrotiX, uma Marca é a "âncora" para múltiplos Modelos. Esta hierarquia é protegida por regras rígidas de banco de dados para evitar inconsistências nos ativos do Estado.

### Pontos de Atenção na Implementação:

1.  **Cascata de Deleção Bloqueada:** 
    O sistema nunca permite excluir uma Marca que possua Modelos vinculados. O método Delete realiza esta conferência de forma proativa, retornando uma mensagem de aviso em vez de estourar um erro de chave estrangeira do SQL.
    
2.  **Status e Ativação:**
    A gestão de marcas permite desativar fabricantes antigos ou não mais utilizados em novas licitações, mantendo o histórico de veículos antigos intacto através da coluna Status.

3.  **Endpoint Simplificado:**
    O Get desta API é otimizado para preencher rapidamente Dropdowns e Select2 em toda a plataforma, fornecendo apenas os dados essencias (MarcaId e DescricaoMarca).

## 🛠 Snippets de Lógica Principal

### Validação de Dependência antes da Exclusão
Este trecho mostra como o FrotiX protege a árvore de dados antes de executar um comando de deleção:

`csharp
[HttpPost]
public IActionResult Delete(MarcaVeiculoViewModel model)
{
    // Verifica se existem modelos dependentes desta marca
    var modeloDependente = _unitOfWork.ModeloVeiculo.GetFirstOrDefault(u => u.MarcaId == model.MarcaId);
    
    if (modeloDependente != null) {
        return Json(new { success = false, message = "Existem modelos associados a essa marca" });
    }

    _unitOfWork.MarcaVeiculo.Remove(objFromDb);
    _unitOfWork.Save();
    return Json(new { success = true, message = "Marca removida com sucesso" });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Auditória de Log:** Todas as mudanças de status geram descrições estruturadas (ex: "Atualizado Status da Marca [Nome: X] (Inativo)"), facilitando a leitura de logs administrativos.
- **Tratamento de Erros:** Utiliza a injeção de logs global Alerta.TratamentoErroComLinha mencionando explicitamente o arquivo MarcaVeiculoController.cs.
- **Roteamento API:** Segue o padrão RESTful pi/[controller], permitindo integração fluida com o frontend moderno do FrotiX.


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
