# FrotiX_AtualizarEstatisticasMotoristas

- **Categoria:** [Uncategorized (Local)]
- **Descrição:** Atualiza tabelas estatísticas do dashboard de motoristas a cada hora.

## Passos
- **Step 1 — Recalcular Estatísticas do Mês Atual**
  - Script T-SQL:
    - Define `@Ano = YEAR(GETDATE())` e `@Mes = MONTH(GETDATE())`.
    - Executa `sp_RecalcularEstatisticasMotoristas @Ano, @Mes`.
    - Recalcula também o mês anterior (ajusta ano/mês quando o mês atual é janeiro).
    - Executa novamente `sp_RecalcularEstatisticasMotoristas @Ano, @Mes` para o mês anterior.
  - Banco: `FrotiX`
  - Tentativas: 0 (sem retry)
  - Ação em falha: termina o job

## Agendamento
- Tipo: diário (`freq_type=4`)
- Frequência intradiária: a cada 1 hora (`freq_subday_type=8`, `freq_subday_interval=1`)
- Janela diária: 00:00:00 até 23:59:59
- Vigência: início 2026-01-01, sem data de fim
- Servidor: (local)

## Observações
- O passo cobre mês atual e anterior, mitigando lançamentos retroativos.
- Sem retries configurados; considerar adicionar para maior resiliência.
- Executa no mesmo ciclo horário de outros jobs; avaliar sobreposição conforme carga.


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
