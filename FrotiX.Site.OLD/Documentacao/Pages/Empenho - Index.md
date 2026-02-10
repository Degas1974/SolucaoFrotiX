# Gestão Financeira: Empenhos e Movimentações

O empenho é o pilar financeiro do FrotiX. É através dele que o sistema controla a reserva orçamentária para contratos de locação, combustíveis e serviços. O EmpenhoController gerencia o ciclo de vida dessas reservas, desde a criação até as movimentações de **Aporte** e **Anulação**, garantindo que o saldo disponível em cada contrato ou ata seja sempre real e auditável.

## 💰 Inteligência Financeira e Saldos

O sistema utiliza a **ViewEmpenhos** para consolidar dados complexos de movimentações e notas fiscais. Isso permite que a listagem principal entregue métricas prontas como:
- **Saldo Inicial vs. Saldo Final:** A evolução real do recurso.
- **Média de NF por Movimentação:** Identificação de eficiência no faturamento.
- **Instrumentos Variáveis:** O controlador é polimórfico, tratando Contratos e Atas de Registro de Preço no mesmo endpoint Get, diferenciando as regras de exibição conforme o contexto.

### Fluxos Críticos de Movimentação:

1.  **Aporte de Recursos:** 
    Quando novos recursos são adicionados a um empenho, o sistema registra na tabela MovimentacaoEmpenho e atualiza atomisticamente o saldo final na tabela principal Empenho.
    
2.  **Anulação e Estorno:**
    Processo inverso ao aporte, essencial para correções contábeis. O sistema recalcula o saldo garantindo que não haja inconsistência entre o somatório das movimentações e o total exibido.

## 🛠 Snippets de Lógica Principal

### Cálculo Dinâmico de Saldo no Aporte
Ao realizar um aporte, o sistema sincroniza a movimentação com o cabeçalho do empenho para evitar a necessidade de reprocessar todo o histórico em cada consulta:

`csharp
[Route("Aporte")]
public IActionResult Aporte([FromBody] MovimentacaoEmpenho movimentacao)
{
    _unitOfWork.MovimentacaoEmpenho.Add(movimentacao);
    var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u => u.EmpenhoId == movimentacao.EmpenhoId);
    
    // Sincronização de Saldo (Vem corrigido do Frontend)
    empenho.SaldoFinal = empenho.SaldoFinal + movimentacao.Valor;
    
    _unitOfWork.Empenho.Update(empenho);
    _unitOfWork.Save();
    return Json(new { success = true, message = "Aporte realizado com sucesso" });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Proteção Anti-Exclusão:** Um empenho nunca pode ser removido se houver **Notas Fiscais** ou **Movimentações** vinculadas a ele. O sistema retorna mensagens de aviso claras ao usuário em vez de um erro genérico de banco de dados.
- **Formatação de Moeda:** Toda a saída de valores monetários no Get já vem formatada do backend (.ToString("C")), garantindo consistência visual na Grid, independentemente da cultura do navegador cliente.
- **Consumo de Memória:** O uso de ToList() nas consultas via IUnitOfWork é balanceado para evitar bloqueios em tabelas de grande volume como a ViewEmpenhos.
