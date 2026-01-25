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
