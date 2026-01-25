# Gestão de Frota e Monitoramento de Veículos

A gestão de **Veículos** é o núcleo operacional do FrotiX. Este módulo controla desde a quilometragem e consumo até a disponibilidade para viagens e vínculos contratuais. O VeiculoController utiliza views otimizadas para garantir que a gestão da frota seja rápida e precisa.

## 🚗 Ciclo de Vida do Veículo

Diferente de um cadastro simples, o veículo no FrotiX possui estados dinâmicos que influenciam todo o sistema de agendamento.

### Principais Pilares:
1.  **Views Reduzidas para Performance:** O sistema utiliza ViewVeiculos.GetAllReduced para carregar apenas os campos essenciais (Placa, KM, Status, Sigla, Origem), reduzindo o payload em mais de 70% em frotas grandes.
2.  **Origem e Propriedade:** Controlamos se o veículo é **Próprio** ou **Locado**, o que afeta diretamente o cálculo de custos de viagens (depreciação vs. custo de contrato).
3.  **Veículos Reserva:** Flag estratégica para identificar ativos que não devem ser priorizados em agendamentos rotineiros, servindo como suporte para manutenções programadas.

## 🛠 Snippets de Lógica Principal

### Proteção de Integridade (Soft vs. Hard Rules)
O sistema impede a exclusão de veículos que possuem "história" no FrotiX. Se um veículo já fez uma viagem ou está sob contrato, ele se torna vital para a auditoria financeira.

`csharp
// Bloqueio de Declusão por Vínculo Contratual
var veiculoContrato = _unitOfWork.VeiculoContrato.GetFirstOrDefault(u => u.VeiculoId == model.VeiculoId);
if (veiculoContrato != null) {
    return Json(new { success = false , message = "Não foi possível remover o veículo. Ele está associado a contratos!" });
}

// Bloqueio por Histórico de Operação
var objViagem = _unitOfWork.Viagem.GetFirstOrDefault(u => u.VeiculoId == model.VeiculoId);
if (objViagem != null) {
    return Json(new { success = false , message = "Não foi possível remover o veículo. Ele está associado a viagens!" });
}
`

## 📝 Notas de Implementação

- **Atualização de Status:** Ao alternar de Ativo para Inativo, o sistema gera uma mensagem descritiva de auditoria que é enviada à interface, informando exatamente qual placa foi afetada.
- **Integração com CRLV:** O módulo estende funcionalidades para o UploadCRLVController, permitindo a gestão do documento digitalizado do veículo.
- **Cálculo de Consumo:** A quilometragem informada no cadastro de veículo serve como baseline para as validações do módulo de Abastecimento, impedindo lançamentos de KM retroativo ou impossível.

---
*Documentação gerada para a Solução FrotiX 2026.*


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
