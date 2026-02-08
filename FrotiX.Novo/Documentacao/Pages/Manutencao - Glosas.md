# Gestão de Glosas e Conformidade de Lavagem

A documentação das **Glosas** no FrotiX representa o controle de qualidade final do sistema sobre os serviços de lavagem prestados por terceiros. O GlosaController atua como um motor de cálculo e validação, convertendo inconformidades operacionais em deduções financeiras automáticas, garantindo que o Estado pague apenas pelo serviço efetivamente realizado conforme contrato.

## 🧼 O Ciclo da Glosa

O controlador gerencia dois níveis de informação essenciais para a fiscalização de contratos:
- **Resumo de Glosa:** Visão consolidada por veículo e período, ideal para o Ateste de faturamento.
- **Detalhes de Glosa:** Visão minuciosa que justifica cada desconto, informando datas de solicitação, disponibilidade e o motivo exato da penalidade (atraso na disponibilização, qualidade inferior, etc).

### Funcionalidades Avançadas:

1.  **Exportação Multi-Aba para Excel:** 
    Utilizando a biblioteca ClosedXML, o controlador gera relatórios dinâmicos onde os fiscais podem conferir o resumo e o detalhamento em abas separadas de um mesmo arquivo .xlsx.
    
2.  **Integração com Syncfusion EJ2:**
    Os endpoints esumo e detalhes são otimizados para o componente **DataManager** do Syncfusion, suportando ordenação, busca e paginação complexas diretamente no servidor (Server-side rendering), o que garante performance mesmo com milhares de registros de lavagem.

3.  **Formatação Dinâmica de Planilhas:**
    O sistema inclui helpers internos para garantir que colunas de moeda (R$) e data (DD/MM/AAAA) cheguem ao fiscal já formatadas no Excel, economizando tempo de conferência manual.

## 🛠 Snippets de Lógica Principal

### Processamento de Dados para Grid Syncfusion
O método abaixo demonstra como o FrotiX lida com requisições complexas da Grid comercial do Syncfusion, aplicando filtros e paginação sob demanda:

`csharp
[HttpGet("resumo")]
public IActionResult Resumo([FromQuery] DataManagerRequest dm, [FromQuery] Guid contratoId, [FromQuery] int ano, [FromQuery] int mes)
{
    var data = _service.ListarResumo(contratoId, mes, ano).AsQueryable();
    var ops = new DataOperations();
    IEnumerable result = data;

    // Aplicação dinâmica de filtros da Grid
    if (dm.Search?.Count > 0) result = ops.PerformSearching(result, dm.Search);
    result = ops.PerformFiltering(result, dm.Where, "and");
    result = ops.PerformSorting(result, dm.Sorted);

    var count = result.Cast<object>().Count();
    result = ops.PerformSkip(result, dm.Skip);
    result = ops.PerformTake(result, dm.Take);

    return new JsonResult(new DataResult { Result = result, Count = count });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Cálculos via Service:** Toda a lógica pesada de cálculo de glosa (Dias de Atraso x Valor Diário) reside no IGlosaService, mantendo o controlador limpo e focado na interface de dados.
- **Padrão de Retorno:** Utiliza JsonResult customizado para o formato DataResult, exigido pelos componentes modernizados do frontend.
- **Excel Profissional:** As tabelas geradas no Excel utilizam o tema TableStyleMedium2 e ajuste automático de colunas (AdjustToContents), elevando o padrão de entrega para o cliente final.
