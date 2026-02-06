# Portal de Entrada e Experimentos Syncfusion

A página principal (HomeController) do FrotiX atua como o ponto de recepção do sistema. Além de servir a View de boas-vindas, ela abriga estruturas de teste e demonstração para os componentes **Syncfusion EJ2**, servindo de referência técnica para a implementação de novas grids e funcionalidades complexas no sistema.

## 🏠 O Papel da Home no FrotiX

Diferente de módulos operacionais como "Frotas" ou "Abastecimento", o HomeController é frequentemente usado para validar a comunicação entre o backend C# e os componentes comerciais do frontend. Ele contém exemplos de:
- **DataSource Local:** Para carregamento ultra-rápido de dados estáticos.
- **UrlDatasource:** Exemplo de implementação para consumo assíncrono com suporte a paginação nativa da Grid.
- **CRUD Operations:** Demonstração de Actions para Inserção, Atualização e Deleção seguindo o protocolo esperado pelo Syncfusion.

### Inteligência de Dados de Teste

O objeto OrdersDetails dentro do controlador simula uma base de dados real, gerando centenas de registros em memória para testes de performance e comportamento de interface, garantindo que o visual do FrotiX seja validado antes da integração com o SQL Server.

## 🛠 Snippets de Lógica Principal

### Implementação de DataSource Remoto (Padrão Syncfusion)
Este trecho exemplifica como o controlador deve responder a uma requisição de dados paginada, respeitando os parâmetros skip e 	ake enviados pela grid:

`csharp
public IActionResult UrlDatasource([FromBody] Data dm)
{
    var order = OrdersDetails.GetAllRecords();
    var Data = order.ToList();
    int count = order.Count();
    
    // Resposta estruturada com contagem total para a paginação funcionar
    return dm.requiresCounts
        ? Json(new { result = Data.Skip(dm.skip).Take(dm.take), count = count })
        : Json(Data);
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Erros Generativo:** Mesmo em dados de teste, o controlador utiliza Alerta.TratamentoErroComLinha, mantendo o padrão de "Zero Tolerance" para erros silenciosos em todo o ecossistema.
- **IgnoreAntiforgeryToken:** Utilizado estrategicamente em endpoints de API para facilitar testes rápidos e integrações AJAX, desde que as sessões de usuário (Identity) estejam devidamente configuradas.
- **Mocking Interno:** A classe OrdersDetails dentro do arquivo HomeController.cs é um exemplo de como o FrotiX permite prototipagem rápida de novos módulos sem depender de Migrations imediatas.


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
