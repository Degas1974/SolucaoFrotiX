# Inteligência de Glosas e Auditoria de Contratos

O GlosaService é o componente encarregado de calcular a liquidação financeira de contratos de prestação de serviços por disponibilidade. Sua missão principal é transformar os dados operacionais de indisponibilidade de veículos (saída para manutenção, atrasos na entrega) em valores financeiros reais de desconto, garantindo que o sistema pague apenas pelo que foi efetivamente entregue.

## 🧠 Lógica de Consolidação (Ateste Mensal)

Diferente de uma simples consulta, este serviço realiza uma agregação multidimensional. Ele cruza o que foi **contratado** (Itens de Contrato) com o que foi **executado** (Ordens de Serviço na oficina).

### Como o cálculo funciona:
1.  **Visão por Item:** O serviço agrupa todas as Ordens de Serviço por item de contrato. Isso é crucial porque um mesmo item (ex: "Sedan Premium") pode ter múltiplos veículos sofrendo glosas no mesmo mês.
2.  **Cálculo do Ateste:** O serviço define o valor de "Ateste" (o valor liberado para pagamento) subtraindo o somatório das glosas do valor total mensal contratado.
3.  **Independência de O.S.:** Uma característica vital da implementação é que o preço total mensal do item é derivado da Quantidade * ValorUnitario do contrato, e não da soma das O.S., garantindo que o teto contratual seja respeitado.

## 🛠 Snippets de Lógica Principal

### Agregação de Valores via LINQ
Abaixo, o trecho que realiza a mágica da consolidação financeira por item de contrato:

`csharp
var query = baseQuery
    .GroupBy(g => new { g.NumItem, g.Descricao })
    .Select(s => new GlosaResumoItemDto {
        NumItem = s.Key.NumItem,
        Descricao = s.Key.Descricao,
        // O preço total é a potência contratada máxima do item
        PrecoTotalMensal = (s.Max(i => i.Quantidade) * s.Max(i => i.ValorUnitario)),
        // A glosa é a soma real de todas as indisponibilidades do período
        Glosa = s.Sum(i => i.ValorGlosa),
        // Valor Final = Contrato - Penalidades
        ValorParaAteste = (s.Max(i => i.Quantidade) * s.Max(i => i.ValorUnitario)) - s.Sum(i => i.ValorGlosa),
    });
`

## 📝 Notas de Implementação

- **DTOs Desacoplados:** Utilizamos o GlosaResumoItemDto e GlosaDetalheItemDto para garantir que a interface receba apenas os campos formatados (como datas amigáveis e valores em decimal), sem expor entidades ricas do EF Core.
- **Performance:** O uso de GetAllReducedIQueryable com sNoTracking: true garante que o processamento seja feito predominantemente em memória ou otimizado pelo SQL Server, ideal para relatórios complexos.
- **Data de Retorno:** No detalhamento de glosas, a DataDevolucao é apresentada como o "Retorno" do veículo à frota ativa, fechando o ciclo de indisponibilidade.

---
*Documentação de inteligência de negócios - FrotiX 2026. Precisão absoluta no controle de custos.*
