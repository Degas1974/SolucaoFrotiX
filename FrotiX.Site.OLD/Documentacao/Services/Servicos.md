# Central de Cálculos e Utilitários de Negócio

A classe Servicos atua como o motor de cálculos matemáticos e utilitários compartilhados do FrotiX. É aqui que reside a lógica de precificação de cada deslocamento, convertendo métricas operacionais (KM, Tempo) em valores financeiros precisos para faturamento e auditoria.

## 💰 Inteligência de Precificação

O FrotiX utiliza três pilares principais para determinar o custo de uma viagem:

### 1. Custo de Combustível
A lógica prioriza o dado mais recente e fiel possível. Se o veículo abasteceu recentemente, o sistema utiliza o valor unitário do último cupom fiscal. Caso contrário, recorre à média mensal de preços da região/combustível cadastrada no módulo de MediaCombustivel.

### 2. Custo do Veículo (Disponibilidade)
Baseado em um modelo de "Horas Úteis". O sistema considera que um veículo tem 21.120 minutos úteis por mês (22 dias úteis × 16 horas operacionais). O custo por minuto é derivado do valor mensal do contrato, garantindo que a cobrança seja proporcional ao tempo de uso real dentro da janela operacional.

### 3. Custo do Motorista (Terceirização)
Para motoristas sob contrato de locação, o sistema busca o valor na última repactuação registrada. O cálculo segue a mesma lógica de janelas úteis, mas com foco na carga horária de trabalho (geralmente 12h/dia).

## 🛠 Snippets de Lógica Principal

### Cálculo de Custo de Combustível
Abaixo, a decisão lógica para escolha do preço do litro:

`csharp
// Busca o último abastecimento real do veículo
var combustivelObj = _unitOfWork.Abastecimento
    .GetAll(a => a.VeiculoId == viagemObj.VeiculoId)
    .OrderByDescending(o => o.DataHora);

// Se não houver cupom, usa a média do mês
if (combustivelObj.FirstOrDefault() == null) {
    var media = _unitOfWork.MediaCombustivel
        .GetAll(a => a.CombustivelId == veiculoObj.CombustivelId)
        .OrderByDescending(o => o.Ano).ThenByDescending(o => o.Mes);
    ValorCombustivel = (double)media.FirstOrDefault().PrecoMedio;
} else {
    ValorCombustivel = (double)combustivelObj.FirstOrDefault().ValorUnitario;
}
`

## 📝 Notas de Implementação

- **Consumo Padrão:** Caso o veículo não tenha um consumo médio cadastrado, o sistema assume o baseline de 10.0km/l para evitar divisão por zero e custos zerados.
- **Segurança de Cálculo:** O método CalculaCustoVeiculo possui uma trava de segurança (Math.Min) que impede que o custo de uma única viagem ultrapasse o valor mensal total do contrato do veículo.
- **Tratamento de Strings:** A classe inclui o método ConvertHtml, essencial para limpar tags e formatar textos vindos de editores ricos (Rich Text) antes de exibi-los em relatórios ou grids.

---
*Documentação de núcleo de serviços - FrotiX 2026. O motor financeiro da frota.*
