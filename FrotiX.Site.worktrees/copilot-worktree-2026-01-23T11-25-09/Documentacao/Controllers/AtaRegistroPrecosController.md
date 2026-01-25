# Documentação: Gestão de Atas de Registro de Preços (AtaRegistroPrecosController)

O \AtaRegistroPrecosController\ é o responsável por toda a base contratual de locação e serviços do FrotiX. Ele gerencia o ciclo de vida das Atas de Registro de Preços (ARP), desde a inserção do valor inicial até as repactuações de preços ao longo do tempo. Dada a natureza financeira e jurídica desses dados, este controller possui validações de integridade rigorosas.

## 1. Hierarquia de Dados e Integridade

Uma Ata no FrotiX não é um registro isolado. Ela funciona como o topo de uma pirâmide de dependências:
- **Ata:** O documento mestre com número, ano e fornecedor.
- **Repactuação:** Eventos cronológicos que atualizam os valores da ata.
- **Itens da Ata:** Os valores específicos negociados (ex: valor por KM, valor da diária).

O endpoint \Delete\ exemplifica essa complexidade. O sistema impede a exclusão de uma Ata se houver veículos vinculados a ela, protegendo o histórico de custos da frota. Se a exclusão for permitida, o controller realiza um **Cascade Delete manual**, limpando recursivamente todos os itens e repactuações antes de remover o registro mestre.

\\\csharp
// Navegação profunda para garantir que nenhum 'item órfão' permaneça no banco
foreach (var repactuacao in objRepactuacao)
{
    var objItemRepactuacao = _unitOfWork.ItemVeiculoAta.GetAll(iva => 
        iva.RepactuacaoAtaId == repactuacao.RepactuacaoAtaId);
    foreach (var itemveiculo in objItemRepactuacao)
    {
        _unitOfWork.ItemVeiculoAta.Remove(itemveiculo);
    }
    _unitOfWork.RepactuacaoAta.Remove(repactuacao);
}
\\\

## 2. Formatação para Consumo Frontend (DataTables)

O endpoint \Get\ foi projetado para entregar dados "prontos para exibir". Ele realiza o join com a tabela de fornecedores e formata campos complexos, como o número do processo (ex: \123/24\) e o valor monetário formatado em Real (R$). 

Além disso, ele retorna contadores de dependência (\depItens\, \depVeiculos\), o que permite que a interface do usuário desabilite botões de exclusão ou exiba avisos preventivos sem precisar fazer novas chamadas à API.

## 3. Automatização da Repactuação Inicial

Ao inserir uma nova Ata (\InsereAta\), o sistema cria automaticamente uma primeira "Repactuação" com a descrição "Valor Inicial". Isso garante que o motor de cálculo de custos do FrotiX sempre tenha um ponto de partida histórico, mesmo que a Ata nunca sofra reajustes reais.

---

### Notas de Implementação (Padrão FrotiX)

*   **LINQ Join Otimizado:** O controller utiliza sintaxe de query LINQ para garantir performance em consultas que envolvem múltiplas tabelas de base.
*   **Tratamento de Erros:** Todas as operações críticas são protegidas por blocos \	ry-catch\ que utilizam o helper \Alerta.TratamentoErroComLinha\ para facilitar o debug em produção.
*   **Partial Class:** Por ser um controller que tende a crescer com novas regras de negócio financeiras, ele é definido como \partial\, permitindo a separação lógica em diferentes arquivos físicos se necessário.

---
*Documentação atualizada em 2026.01.14 conforme novo padrão de Prosa Leve.*


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
