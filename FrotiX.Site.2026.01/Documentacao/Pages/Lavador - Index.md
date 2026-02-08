# Gestão de Lavadores e Controle de Higienização

A gestão de **Lavadores** no FrotiX é um componente essencial para a longevidade da frota e a conformidade com as normas de higiene do Estado. O LavadorController gerencia esses profissionais, vinculando-os a fornecedores e contratos específicos, o que permite o rastreio rigoroso de quem realizou cada serviço de limpeza.

## 🧼 Responsabilidades e Fluxo Operacional

Diferente de outros colaboradores, o lavador tem um vínculo direto com a **Garantia de Qualidade** (Glosas). A listagem principal do FrotiX consolida o histórico de quem está ativo em cada pátio:

### Pontos de Atenção na Implementação:

1.  **Bloqueio de Exclusão (Integridade de Contrato):** 
    O sistema proíbe a remoção de um lavador que esteja nominalmente citado em qualquer contrato de prestação de serviço. No método Delete, a tabela LavadorContrato é consultada para garantir que nenhum histórico de auditoria seja perdido.
    
2.  **Identificação Visual (Foto de Perfil):**
    O controlador fornece métodos dedicados (PegaFoto e PegaFotoModal) que convertem os dados binários do banco em Base64 para exibição instantânea na interface, facilitando a fiscalização presencial.

3.  **Gestão de Status:**
    A desativação (Inativo) é preferível à exclusão. O método UpdateStatusLavador gerencia essa transição, garantindo que o lavador pare de aparecer em novas escalas, mas permaneça nos registros de serviços já concluídos.

## 🛠 Snippets de Lógica Principal

### Consulta com Identificação de Fornecedor
Este código demonstra como o FrotiX mapeia o lavador através do contrato até chegar à empresa fornecedora:

`csharp
var result = (
    from l in _unitOfWork.Lavador.GetAll()
    join ct in _unitOfWork.Contrato.GetAll() on l.ContratoId equals ct.ContratoId into ctr
    from ctrResult in ctr.DefaultIfEmpty() 
    join f in _unitOfWork.Fornecedor.GetAll() on (ctrResult == null ? Guid.Empty : ctrResult.FornecedorId) equals f.FornecedorId into frd
    from frdResult in frd.DefaultIfEmpty()
    select new {
        l.Nome,
        ContratoLavador = ctrResult != null 
            ? $"{ctrResult.AnoContrato}/{ctrResult.NumeroContrato} - {frdResult.DescricaoFornecedor}"
            : "<b>(Sem Contrato)</b>"
    }
).ToList();
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Exceções:** Todas as Actions são protegidas por blocos 	ry-catch, utilizando a ferramenta global de logging Alerta.TratamentoErroComLinha para facilitar o debug em ambiente de produção.
- **Retornos Normalizados:** Em caso de erro em consultas, o controlador retorna uma View() padronizada ou um objeto JSON vazio, evitando que a interface do usuário (Syncfusion/DataTables) trave ou exiba erros técnicos brutos.
- **Performance de Imagens:** O processamento de fotos é feito de forma sob demanda, evitando que o carregamento da lista principal fique lento devido ao peso das imagens binárias.


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
