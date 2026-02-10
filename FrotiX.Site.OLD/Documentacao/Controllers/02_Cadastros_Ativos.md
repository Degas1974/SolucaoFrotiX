# Guia de Engenharia: Controllers de Cadastro e Ativos

Responsáveis pela manutenção da integridade dos dados mestres da frota.

## 🎛 Controladores Principais
- **VeiculoController**: Gerencia o ciclo de vida do ativo. Inclui parciais para uploads de CRLV e troca rápida de unidade/lotação.
- **MotoristaController**: Focado no condutor. Gerencia desde a foto do perfil até a validação de vencimento de CNH.
- **UnidadeController**: Define a árvore hierárquica do sistema, permitindo o isolamento de dados por setor.

## ⚡ Lógica de Soft-Delete
Para manter a integridade histórica, estes controladores não permitem a exclusão real de registros que possuem dependências em viagens ou abastecimentos, retornando mensagens de erro amigáveis ao invés de exceções de banco.
