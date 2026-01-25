# Gestão de Movimentação de Ativos (Ativos Móveis)

Enquanto o cadastro de Patrimônio define o "que" temos, a **Movimentação de Patrimônio** define "onde" e "com quem" os bens estão. Este módulo é crítico para a responsabilidade fiscal e o controle de carga de cada unidade administrativa do FrotiX.

## 📦 Logística de Bens

O processo de movimentação é rastreado por um workflow de transferência que garante que nenhum item fique em um "limbo" administrativo.

### Fluxo de Operação:
1.  **Requisição de Mudança:** Um bem é selecionado para sair de um Setor/Seção A para um Setor/Seção B.
2.  **Responsabilidade por Item:** Cada movimentação registra o ID do usuário responsável, criando uma linha do tempo imutável de posse.
3.  **Locks de Concorrência:** O sistema utiliza um mecanismo de bloqueio (lock) no backend para garantir que, se dois gestores tentarem transferir o mesmo item ao mesmo tempo, apenas a primeira solicitação seja processada.

## 🛠 Snippets de Lógica Principal

### Registro de Nova Movimentação (Safety First)
A criação de uma movimentação não é apenas um INSERT; ela atualiza o estado atual do bem no cadastro principal de forma atômica:

`csharp
public IActionResult CreateMovimentacao(MovimentacaoPatrimonio mov) {
    // 1. Gera o registro de histórico
    _unitOfWork.MovimentacaoPatrimonio.Add(mov);
    
    // 2. Localiza o bem e atualiza sua localização ATUAL (Sincronização)
    var patrimonio = _unitOfWork.Patrimonio.GetFirstOrDefault(p => p.PatrimonioId == mov.PatrimonioId);
    if (patrimonio != null) {
        patrimonio.SetorId = mov.SetorIdDestino;
        patrimonio.SecaoId = mov.SecaoIdDestino;
        _unitOfWork.Patrimonio.Update(patrimonio);
    }
    
    _unitOfWork.Save();
}
`

## 📝 Notas de Implementação

- **Integração com Dashboards:** As movimentações alimentam o Patrimonio - Dashboard, permitindo ver em tempo real quais setores estão recebendo mais equipamentos.
- **Conferência Física:** O histórico de movimentações é a base para o relatório de Conferência de Carga, onde cada detentor de setor deve assinar o inventário recebido.
- **Nomenclatura (NPR):** Todas as movimentações utilizam o Número de Patrimônio (NPR) como chave visual para facilitar a busca rápida via scanner de código de barras.

---
*Documentacao gerada para a Solução FrotiX 2026. Controle total sobre o inventário público.*


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
