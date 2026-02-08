# Documentação: Cadastro de Tipos de Combustível (CombustivelController)

O \CombustivelController\ é um componente de suporte fundamental para a categorização da frota e a precisão nos relatórios de abastecimento. Embora seja um cadastro simples de "Tipos", ele serve como base para filtros em diversos dashboards e validações na ficha técnica dos veículos.

## 1. Integridade Referencial e Segurança

O sistema impede que um tipo de combustível seja removido se houver qualquer veículo vinculado a ele. Essa trava de segurança no endpoint \Delete\ evita a "orfandade" de registros em tabelas críticas, o que causaria erros de renderização em relatórios de custos.

\\\csharp
// Validação preventiva antes da exclusão física no banco
var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u => 
    u.CombustivelId == model.CombustivelId);

if (veiculo != null)
{
    return Json(new { success = false, message = "Existem veículos associados a essa combustível" });
}
\\\

## 2. Flexibilidade Operacional (Status Ativo/Inativo)

Diferente de uma exclusão física, o endpoint \UpdateStatusCombustivel\ permite desativar logicamente um combustível. Isso é útil quando a frota deixa de usar um combustível específico (ex: GNV), mas deseja manter o histórico de veículos antigos que o utilizaram. Um combustível inativo continua no banco para relatórios históricos, mas deixa de aparecer como opção em novos cadastros.

## 3. Uso em Cascata

Apesar de pequeno, este controller é um dos mais requisitados pelo frontend, sendo chamado não apenas pela sua tela de gestão, mas também para popular dropdowns e componentes de seleção nas telas de:
- **Cadastro de Veículos:** Define a matriz energética do ativo.
- **Lançamento de Abastecimento:** Filtra os tipos permitidos para o veículo selecionado.
- **Filtros de Dashboard:** Permite agrupar custos por tipo de energia (Etanol vs Gasolina vs Diesel).

---

### Notas de Implementação (Padrão FrotiX)

*   **Padrão Unit of Work:** O controller não acessa o \DbContext\ diretamente, utilizando a abstração \_unitOfWork.Combustivel\ para garantir o desacoplamento.
*   **Tratamento de Erros:** Implementado com \Alerta.TratamentoErroComLinha\, garantindo que qualquer falha de SQL seja reportada com o contexto exato do arquivo e método.
*   **Retorno JSON Consistente:** Todos os métodos retornam objetos anônimos no padrão \{ success, message, data }\, facilitando a integração com a biblioteca \lerta.js\ no frontend.

---
*Documentação atualizada em 2026.01.14 conforme novo padrão de Prosa Leve.*


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
