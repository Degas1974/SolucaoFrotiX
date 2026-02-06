# Importação e Integração TaxiLeg

O **TaxiLeg** é um subsistema de mobilidade urbana (transporte por aplicativo corporativo) integrado ao FrotiX. O TaxiLegController é o motor de ingestão de dados que lê planilhas de faturamento externas (Excel) e as transforma em métricas analíticas dentro da plataforma, permitindo auditoria cruzada de quilometragem, tempo de espera e custos de terceiros.

## 📊 Ingestão de Dados e Auditoria

A importação do TaxiLeg não é apenas um upload de arquivos; é um processo de normalização de dados complexos:

### Pontos de Atenção na Implementação:

1.  **Motor de Parsing Excel (NPOI):** 
    O controlador utiliza a biblioteca NPOI para ler arquivos .xls e .xlsx. Ele percorre cada linha validando tipos de dados (Numéricos, Datas, Horas) e tratando variações de formatação que ocorrem em exportações de diferentes fornecedores.
    
2.  **Validação de Período Único:**
    Para evitar duplicidade de faturamento, o sistema verifica se o mês/ano contido na planilha já foi importado. Através do ICorridasTaxiLegRepository.ExisteCorridaNoMesAno, o FrotiX bloqueia re-importações acidentais que distorceriam os KPIs de custo.

3.  **Cálculos de Duração e Espera:**
    Durante a importação, o sistema calcula dinamicamente o tempo de duração da viagem e o tempo de espera do passageiro, combinando colunas de Data e Hora com precisão de minutos.

## 🛠 Snippets de Lógica Principal

### Extração Flexível de Horário
Implementação para tratar campos de hora que podem chegar como Numeric (Excel Date) ou String (Formatada):

`csharp
private string ExtrairHora(IRow row, int cellIndex)
{
    var cell = row.GetCell(cellIndex);
    if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell)) {
        return cell.DateCellValue.ToString("HH:mm");
    }
    string raw = cell.ToString().Trim();
    if (TimeSpan.TryParse(raw, out var ts)) return ts.ToString(@"hh\:mm");
    return "";
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Erros de Memória:** O uso do NPOI com stream.Position = 0 garante que arquivos grandes sejam lidos sem estourar o buffer, e cada erro em células individuais é capturado via Alerta.TratamentoErroComLinha mencionando o arquivo TaxiLegController.cs.
- **Cultura Localizada:** Utiliza explicitamente a cultura pt-BR para parsing de moedas e datas, assegurando que virgulas e pontos decimais sejam interpretados corretamente conforme os padrões brasileiros do setor público.
- **Feedback ao Usuário:** O sistema retorna objetos JSON detalhados indicando se a importação foi bem-sucedida ou qual regra de negócio (ex: "Mes já importado") impediu a operação.


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

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
