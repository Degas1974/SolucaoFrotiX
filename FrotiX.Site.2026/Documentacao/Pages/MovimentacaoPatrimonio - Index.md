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
