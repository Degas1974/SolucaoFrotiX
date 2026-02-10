# Documentação: Gestão de Contratos e Terceirização (ContratoController)

O \ContratoController\ é o pilar jurídico e financeiro para toda a frota terceirizada do FrotiX. Ele gerencia as regras de negócio que regem o relacionamento entre a instituição e as empresas fornecedoras, controlando desde a vigência temporal até o detalhamento de custos por item contratado.

## 1. O "Mapa de Dependências" (Data Intelligence)

Diferente de cadastros simples, o endpoint \Get\ deste controller realiza uma operação massiva de inteligência de dados. Para cada contrato listado, o sistema calcula em tempo real o número de vínculos ativos em nove categorias diferentes:
-   **Operacionais:** Veículos, Motoristas, Lavadores, Operadores e Encarregados.
-   **Financeiros:** Empenhos e Notas Fiscais.
-   **Contratuais:** Repactuações e Itens de Preço.

Essa densidade de informação, encapsulada nos campos \DepVeiculos\, \DepEmpenhos\, etc., é o que permite ao frontend do FrotiX oferecer uma experiência de "zero erro", onde o usuário sabe exatamente por que não pode excluir um contrato antes mesmo de tentar.

\\\csharp
// Exemplo de como o sistema mapeia a "árvore" de dependências para garantir integridade
veiculosContrato = _unitOfWork.VeiculoContrato
    .GetAll(x => contratoIds.Contains(x.ContratoId))
    .GroupBy(x => x.ContratoId)
    .ToDictionary(g => g.Key , g => g.Count());
\\\

## 2. Ciclo de Vida e Repactuações

O contrato no FrotiX é dinâmico. Através do sistema de **Repactuações**, o administrador pode atualizar os valores dos itens sem perder o histórico do contrato original. O controller gerencia esse versionamento, garantindo que novos cálculos de custos usem sempre a repactuação ativa (a mais recente), enquanto auditorias podem consultar os valores do passado.

## 3. Segurança na Exclusão (Cascade Guard)

O endpoint \Delete\ atua como um guardião da integridade do banco. A exclusão de um contrato é uma operação de "terra arrasada" que só é permitida se não houver veículos ou empenhos vinculados. Caso o contrato seja elegível para exclusão, o controller realiza a limpeza recursiva de todas as repactuações e itens de contrato associados, evitando o surgimento de registros órfãos que corromperiam os cálculos de BI.

---

### Notas de Implementação (Padrão FrotiX)

*   **Partial Class Optimization:** O controller é dividido em arquivos parciais para separar a lógica de CRUD da lógica de verificação de dependências (\ContratoController.VerificarDependencias.cs\).
*   **IgnoreAntiforgeryToken:** Aplicado para facilitar a integração com componentes de terceiros e chamadas AJAX de alto volume vindas do Grid.
*   **Performance via Dictionaries:** O carregamento da lista utiliza dicionários pré-calculados na memória para evitar o problema de performance "N+1 queries", garantindo que a listagem de centenas de contratos seja instantânea.

---
*Documentação atualizada em 2026.01.14 conforme novo padrão de Prosa Leve.*
