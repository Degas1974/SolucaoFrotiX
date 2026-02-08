# Gestão de Multas, Infrações e Recursos

O módulo de **Multas** do FrotiX é um sistema completo de workflow jurídico-financeiro. Ele rastreia cada infração desde a notificação inicial (Autuação) até o pagamento final (Penalidade) ou deferimento de recurso. O MultaController é o centro integrador que vincula o veículo, o motorista e o órgão autuante.

## ⚖️ Workflow da Infração

Diferente de uma lista simples, o sistema separa as infrações por "Fases", refletindo o rito processual do Código de Trânsito Brasileiro.

### Fases e Documental:
1.  **Autuação vs. Penalidade:** O sistema diferencia notificações iniciais de penalidades pecuniárias, permitindo o acompanhamento de prazos de defesa prévia.
2.  **Repositório de Provas (PDF):** Integração nativa com o MultaPdfViewerController, permitindo anexar e visualizar o Auto de Infração, a Guia de Recolhimento e o Comprovante de Pagamento sem sair do painel.
3.  **Gestão de Prazos:** Notificação inteligente baseada no ValorAteVencimento. O controlador expõe flags dinâmicas para a interface mostrar alertas de "Próximo ao Vencimento" ou "Pagamento em Atraso".

## 🛠 Snippets de Lógica Principal

### Projeção Inteligente para DataTables
Para evitar sobrecarga no frontend, o controlador já entrega os dados "prontos para consumo", incluindo o estado visual dos botões de pagamento:

`csharp
select new {
    multaId = vm.MultaId,
    placa = vm.Placa,
    valorFormatado = vm.ValorAteVencimento?.ToString("C"),
    paga = vm.Paga,
    // Lógica de interface injetada no backend:
    habilitado = vm.Paga == true ? "" : "data-toggle='modal' data-target='#modalRegistraPagamento'",
    tooltip = vm.Paga == true ? "Pagamento já Registrado" : "Registrar Pagamento"
}
`

## 📝 Notas de Implementação

- **Vinculação de Empenho:** No caso de frotas públicas ou locadas, multas podem ser vinculadas a Empenhos Financeiros para pagamento automático via MovimentacaoEmpenhoMulta.
- **Identificação do Infrator:** O sistema cruza as datas e horas da multa com a ViewViagens para sugerir automaticamente qual motorista estava em posse do veículo no momento da infração.
- **Conversão HTML:** Notas e observações são tratadas por Servicos.ConvertHtml para garantir que quebras de linha e formatações ricas sejam preservadas na visualização da grid.

---
*Documentação gerada para a Solução FrotiX 2026. Rigor e transparência no controle de infrações.*
