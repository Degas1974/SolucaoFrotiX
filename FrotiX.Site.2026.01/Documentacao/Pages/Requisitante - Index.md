# Gestão de Requisitantes de Viagem

Os **Requisitantes** são as pessoas autorizadas dentro de cada setor solicitante para demandar serviços de transporte no FrotiX. O RequisitanteController gerencia estes cadastros, integrando-os aos setores correspondentes e mantendo o histórico de controle de ramais e pontos de identificação.

## 👥 Autorização e vínculo

O requisitante atua como o "cliente interno" do sistema. Cada registro é obrigatoriamente vinculado a um setor, garantindo que o custo das viagens possa ser rateado corretamente no final do mês.

### Pontos de Atenção na Implementação:

1.  **Upsert Inteligente:** 
    O sistema utiliza um único endpoint (Upsert) para criação e edição, diferenciando a ação pela presença do RequisitanteId. Isso simplifica a lógica de frontend e garante que as auditorias de DataAlteracao e UsuarioIdAlteracao sejam salvas consistentemente.
    
2.  **Mapeamento de Usuário Logado:**
    Ao salvar um requisitante, o sistema extrai o ID do gestor responsável através de User.FindFirst(ClaimTypes.NameIdentifier), assegurando que saibamos quem autorizou o credenciamento de cada servidor/colaborador.

3.  **Hierarquia de Setores:**
    Para facilitar a navegação em órgãos complexos, o controlador suporta métodos informativos como GetSetoresHierarquia, permitindo que o frontend organize a árvore de setores antes da seleção do requisitante.

## 🛠 Snippets de Lógica Principal

### Consulta com Join de Setores Solicitantes
Este trecho exemplifica como a lista principal é montada para exibir o nome do setor em vez de apenas o ID:

`csharp
[HttpGet]
public IActionResult Get()
{
    var result = (
        from r in _unitOfWork.Requisitante.GetAll()
        join s in _unitOfWork.SetorSolicitante.GetAll() on r.SetorSolicitanteId equals s.SetorSolicitanteId
        orderby r.Nome
        select new {
            r.Ponto,
            r.Nome,
            NomeSetor = s.Nome,
            r.Status,
            r.RequisitanteId
        }
    ).ToList();
    return Json(new { data = result });
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Exceções:** Implementação rigorosa de 	ry-catch com uso de Alerta.TratamentoErroComLinha, incluindo detalhamento da InnerException para falhas complexas de banco.
- **Normalização de Dados:** O controlador utiliza heurísticas simples para garantir que campos nulos (Ponto, Nome) cheguem ao frontend como strings vazias (""), evitando erros de renderização em componentes JavaScript.
- **Roteamento API REST:** Segue o padrão pi/[controller], expondo endpoints específicos como GetAll, GetById e GetSetores para consumo modular.


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
