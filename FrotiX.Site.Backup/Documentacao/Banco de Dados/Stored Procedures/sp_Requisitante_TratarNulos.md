# sp_Requisitante_TratarNulos

## Código completo

```sql
CREATE PROCEDURE dbo.sp_Requisitante_TratarNulos
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Requisitante
    SET 
        Nome = ISNULL(Nome, ''),
        Ponto = ISNULL(Ponto, ''),
        Ramal = ISNULL(Ramal, 0),
        Email = ISNULL(Email, ''),
        Status = ISNULL(Status, 1),
        UsuarioIdAlteracao = ISNULL(UsuarioIdAlteracao, ''),
        DataAlteracao = ISNULL(DataAlteracao, GETDATE())
    WHERE 
        Nome IS NULL
        OR Ponto IS NULL
        OR Ramal IS NULL
        OR Email IS NULL
        OR Status IS NULL
        OR UsuarioIdAlteracao IS NULL
        OR DataAlteracao IS NULL;
END
```

## Explicação por blocos

- **Atualização simples**: um único `UPDATE` na tabela `Requisitante` preenchendo nulos com padrões seguros.
- **Defaults aplicados**: strings vazias para textos, 0 para `Ramal`, 1 para `Status`, `GETDATE()` para `DataAlteracao`, GUID/str vazia para `UsuarioIdAlteracao`.
- **Filtro**: só linhas que tenham algum campo nulo listado.
- **Uso**: rotina manual de saneamento; não há job conhecido. Executar apenas quando desejar higienizar dados legados.


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
