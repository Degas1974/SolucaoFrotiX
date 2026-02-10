# Gestão de Abastecimento e Eficiência Energética

O controle de **Abastecimento** é um dos maiores centros de custos de qualquer frota. No FrotiX, o AbastecimentoController gerencia desde o lançamento manual de cupons até a importação massiva de dados via Excel (NPOI), garantindo que cada litro de combustível seja rastreado e auditado.

## ⛽ Inteligência de Combustível

Este módulo não apenas registra valores, mas valida o consumo real dos veículos cruzando quilometragens e médias históricas.

### Destaques Tecnológicos:

1.  **Importação Massiva (NPOI):** Suporta arquivos XLS e XLSX com milhares de linhas. O sistema processa esses arquivos em background para evitar timeouts e garantir a integridade dos dados.
2.  **Feedback em Tempo Real (SignalR):** Durante importações pesadas, o controlador utiliza o ImportacaoHub para enviar mensagens de progresso diretamente para a tela do usuário, criando uma experiência fluida.
3.  **Filtros Multidimensionais:** O sistema permite extrair relatórios instantâneos filtrados por Motorista, Unidade, Veículo ou Tipo de Combustível, utilizando a ViewAbastecimentos para máxima performance.

## 🛠 Snippets de Lógica Principal

### Filtros Especializados para Relatórios

O controlador expõe endpoints específicos para cada tipo de visão, facilitando a construção de telas de detalhes no frontend:

`csharp
[Route("AbastecimentoVeiculos")]
public IActionResult AbastecimentoVeiculos(Guid Id) {
    // Filtra utilizando a View para trazer nomes de motoristas e placas resolvidos
    var dados = _unitOfWork.ViewAbastecimentos.GetAll()
        .Where(va => va.VeiculoId == Id)
        .OrderByDescending(va => va.DataHora).ToList();
    return Ok(new { data = dados });
}
`

## 📝 Notas de Implementação

- **Atomicidade na Importação:** O uso de blocos de TransactionScope nas parciais de importação garante que, caso uma linha do Excel esteja corrompida, o sistema possa reverter o lote inteiro ou marcar apenas os erros, mantendo o banco consistente.
- **Normalização de Dados:** O sistema trata variações de nomes de combustíveis e nomes de motoristas (Uppercase/Lowercase) durante a importação para evitar duplicidades no cadastro.
- **Integração com Dashboard:** Os dados gerados aqui alimentam diretamente o AbastecimentoController.DashboardAPI, gerando indicadores de R$/KM e Litros/100KM.

---

_Documentação gerada para a Solução FrotiX 2026. A precisão destes dados é a base para a economia da frota._

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [16/01/2026 17:15] - Auditoria Global: Campos Obrigatórios (.label-required)

**Descrição**: Adicionado asterisco vermelho em labels de campos mandatórios identificados via lógica de validação (Back/Front).
