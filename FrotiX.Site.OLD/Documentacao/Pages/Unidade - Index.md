# Gestão de Unidades e Lotação de Motoristas

As **Unidades** no FrotiX representam os pontos físicos ou administrativos de lotação da frota e do pessoal. O UnidadeController gerencia estas entidades e coordena um dos processos mais dinâmicos do sistema: o histórico de movimentação de motoristas entre diferentes unidades de trabalho.

## 🏢 Territorialidade e Lotação

Cada motorista deve estar vinculado a uma unidade para fins de escala e controle de jornada. O controlador gerencia este vínculo garantindo que o histórico nunca seja apagado, apenas atualizado.

### Pontos de Atenção na Implementação:

1.  **Lotação Instantânea (Sync):** 
    Ao realizar uma nova lotação através do método LotaMotorista, o sistema atualiza simultaneamente a tabela de histórico (LotacaoMotorista) e o registro atual na tabela principal (Motorista). Isso garante que consultas rápidas por "Unidade Atual" sejam sempre performáticas.
    
2.  **Proteção de Ativos Vinculados:**
    O sistema impede a exclusão de uma Unidade que possua **Veículos** associados. Esta regra protege a integridade dos dashboards geográficos e evita que ativos fiquem sem uma base operacional definida.

3.  **Gestão de Status e Notificações:**
    A troca de status de uma unidade aciona mensagens de sistema para logs administrativos. O controlador utiliza o INotyfService para fornecer feedback visual imediato ao gestor sobre o sucesso das operações de lotação.

## 🛠 Snippets de Lógica Principal

### Lotação de Motorista com Sincronização Dupla
Este código demonstra como o FrotiX mantém a coerência entre o histórico e o estado atual do colaborador:

`csharp
[HttpGet("LotaMotorista")]
public IActionResult LotaMotorista(string MotoristaId, string UnidadeId, string DataInicio, ...)
{
    // 1. Registra na tabela de histórico de lotações
    var objLotacao = new LotacaoMotorista { ... };
    _unitOfWork.LotacaoMotorista.Add(objLotacao);

    // 2. Sincroniza a UnidadeId diretamente na tabela do Motorista
    var obJMotorista = _unitOfWork.Motorista.GetFirstOrDefault(m => m.MotoristaId == Guid.Parse(MotoristaId));
    obJMotorista.UnidadeId = Guid.Parse(UnidadeId);
    _unitOfWork.Motorista.Update(obJMotorista);

    _unitOfWork.Save();
    return Json(new { message = "Lotação Adicionada com Sucesso" });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Erros:** Segue a norma "Zero Tolerance", com blocos 	ry-catch em todas as Actions e registro via Alerta.TratamentoErroComLinha no arquivo UnidadeController.cs.
- **Validação de Data:** As operações de edição de lotação validam as datas de início e fim para garantir que o histórico seja cronologicamente coerente.
- **Feedback Rico:** Utiliza AspNetCoreHero.ToastNotification em conjunto com a biblioteca interna de alertas para garantir que o usuário nunca fique sem confirmação de uma ação administrativa.
