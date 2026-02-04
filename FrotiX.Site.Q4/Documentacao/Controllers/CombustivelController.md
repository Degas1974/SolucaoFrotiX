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
