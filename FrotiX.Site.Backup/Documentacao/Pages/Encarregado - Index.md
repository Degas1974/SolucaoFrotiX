# Gestão de Encarregados - API e Listagem

A gestão de **Encarregados** no FrotiX é o braço operacional que interconecta Contratos, Fornecedores e a supervisão direta no pátio. O EncarregadoController não apenas lista nomes; ele mapeia a hierarquia de responsabilidade, permitindo que o sistema identifique quem é o ponto de contato para cada veículo ou equipe em um contrato específico.

## 🚌 O Papel do Encarregado no Ecossistema

Diferente de um motorista, o encarregado é frequentemente o elo com o **Fornecedor**. Na listagem principal, o FrotiX realiza um *Join* triplo (Encarregado + Contrato + Fornecedor) para exibir de forma clara a que empresa ele pertence, facilitando a comunicação em caso de irregularidades ou necessidades de manutenção.

### Pontos de Atenção na Implementação:

1.  **Integridade de Dados (Soft Delete de Negócio):** 
    Antes de permitir a exclusão de um encarregado no método Delete, o sistema verifica a tabela EncarregadoContrato. Se houver qualquer vínculo ativo com um contrato, a exclusão é bloqueada. Isso evita "buracos" históricos na gestão de frotas.
    
2.  **Tratamento de Imagens (Base64):**
    O controlador gerencia fotos de perfil para identificação rápida. O método GetImage converte string Base64 em yte[], garantindo compatibilidade com o armazenamento em banco de dados e exibição em modais de detalhamento.

3.  **Auditória de Alterações:**
    O campo UsuarioIdAlteracao é mapeado para AspNetUsers.NomeCompleto, garantindo transparência sobre quem fez a última atualização cadastral do colaborador.

## 🛠 Snippets de Lógica Principal

### Consulta de Listagem com Identificação de Contrato
Este trecho exemplifica o JOIN complexo usado para montar a grid, tratando casos onde o encarregado pode estar temporariamente sem contrato vinculado:

`csharp
var result = (
    from e in _unitOfWork.Encarregado.GetAll()
    join ct in _unitOfWork.Contrato.GetAll() on e.ContratoId equals ct.ContratoId into ctr
    from ctrResult in ctr.DefaultIfEmpty()
    join f in _unitOfWork.Fornecedor.GetAll() on (ctrResult == null ? Guid.Empty : ctrResult.FornecedorId) equals f.FornecedorId into frd
    from frdResult in frd.DefaultIfEmpty()
    select new {
        e.Nome,
        ContratoEncarregado = ctrResult != null 
            ? $"{ctrResult.AnoContrato}/{ctrResult.NumeroContrato} - {frdResult.DescricaoFornecedor}"
            : "<b>(Sem Contrato)</b>"
    }
).ToList();
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Erros:** Todos os métodos (Get, Delete, UpdateStatus) seguem o padrão Alerta.TratamentoErroComLinha, injetando Logs detalhados em caso de falhas de banco ou rede.
- **UI Responsiva:** Os dados são devolvidos no formato JSON { data: [...] }, otimizados para consumo via **jQuery DataTables** ou **Syncfusion Grid**.
- **Segurança de Status:** A troca de status (Ativo/Inativo) gera uma descrição amigável para o log do sistema, registrando exatamente o que mudou e o novo estado.


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
