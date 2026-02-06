# Gestão de Operadores de Máquinas e Equipamentos

Enquanto motoristas conduzem veículos de passeio, os **Operadores** são responsáveis pelos ativos pesados (retroescavadeiras, geradores, etc.) no ecossistema FrotiX. O OperadorController gerencia esses profissionais, garantindo que seu vínculo com fornecedores e contratos de locação seja mantido com precisão.

## 🏗 Especialização Operacional

O operador, assim como o encarregado, é um elo crítico no contrato de prestação de serviços. O FrotiX mantém um controle rigoroso sobre sua alocação pátio-contrato:

### Pontos de Atenção na Implementação:

1.  **Proteção de Chave Estrangeira Social:** 
    O sistema bloqueia a exclusão de um operador caso ele esteja vinculado a qualquer serviço ativo (OperadorContrato). Isso garante que os diários de bordo e registros de hora-máquina nunca percam a referência de quem estava no comando do equipamento.
    
2.  **Rastreabilidade de Alteração:**
    Cada registro de operador exibe quem foi o último gestor a alterar seus dados (UsuarioIdAlteracao -> NomeCompleto), criando uma camada de responsabilidade sobre os dados cadastrais.

3.  **Identificação Visual Obrigatória:**
    Através do PegaFotoModal, o sistema permite que supervisores de campo identifiquem o operador pela foto armazenada em banco, garantindo que a pessoa operando a máquina é de fato o profissional credenciado.

## 🛠 Snippets de Lógica Principal

### Captura de Foto com Conversão em Tempo Real
Este helper do controlador demonstra como os dados binários da foto são entregues à interface de forma limpa:

`csharp
[HttpGet("PegaFotoModal")]
public JsonResult PegaFotoModal(Guid id)
{
    var objFromDb = _unitOfWork.Operador.GetFirstOrDefault(u => u.OperadorId == id);
    if (objFromDb.Foto != null) {
        // Converte o byte[] para uma string Base64 consumível por tags <img>
        var base64 = this.GetImage(Convert.ToBase64String(objFromDb.Foto));
        return Json(base64);
    }
    return Json(false);
}
`

## 📝 Notas de Implementação (Padrão FrotiX)

- **Tratamento de Erros:** Todas as chamadas de banco e lógica de negócio são protegidas por 	ry-catch, registrando falhas via Alerta.TratamentoErroComLinha com metadados do arquivo OperadorController.cs.
- **Status Ativo/Inativo:** A troca de status gera uma mensagem amigável para o log do sistema, registrando o nome do operador e o novo estado (Ex: "Atualizado Status do Operador [Nome: João] (Ativo)").
- **Join de Fornecedor:** A listagem principal realiza um *Outer Join* com a tabela de Fornecedores, expondo claramente a empresa parceira responsável pelo profissional, facilitando a gestão de RH terceirizado.


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
