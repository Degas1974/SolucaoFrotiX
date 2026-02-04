# Guia de Engenharia: Controllers Financeiros e Auditoria

Onde as regras de negócio de contratos e penalidades são aplicadas.

## 🎛 Controladores Principais
- **ContratoController**: Gerencia as cláusulas financeiras e a repactuação de itens.
- **GlosaController**: Aplica deduções automáticas baseadas no tempo de inatividade do veículo reportado pelo módulo de Manutenção.
- **NotaFiscalController**: Vincula os gastos de oficina e serviços aos itens de empenho.

## ⚡ Segurança Transacional
Operações de repactuação ou baixa de empenho são protegidas por blocos 	ry-catch robustos que utilizam a unidade de trabalho (UnitOfWork) para garantir atomicidade.
