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
