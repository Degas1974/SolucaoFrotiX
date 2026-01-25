# Rede de Segurança e Monitoramento Global (Middleware)

O ErrorLoggingMiddleware é a sentinela do pipeline de requisições do FrotiX. Ele atua como um invólucro (wrapper) sobre toda a aplicação, garantindo que nenhum erro, seja ele uma exceção de código ou uma falha de protocolo HTTP, passe despercebido. Sua existência é o que permite ao sistema cumprir a regra de "Tolerância Zero" para erros não registrados.

## 🛡 Escudo de Execução

Diferente do 	ry-catch manual nas Actions, este middleware captura falhas que ocorrem em níveis mais baixos ou inesperados da infraestrutura.

### Mecanismos de Detecção:
1.  **Interceptador de Exceções:** Se qualquer parte do código lançar uma exceção que não foi capturada por um 	ry-catch local, o middleware a isola, extrai o arquivo, o método e a linha exata da falha através de Regex no StackTrace, e registra o incidente antes de repassar para a página de erro amigável.
2.  **Monitor de Status HTTP:** Ele observa o código de resposta de cada requisição. Sempre que o servidor responde com um código maior que 400 (como 404 - Não Encontrado ou 401 - Não Autorizado), o middleware registra o evento no log, mesmo que não tenha havido uma exceção de código.
3.  **Enriquecimento de Contexto:** Cada erro registrado pelo middleware contém a URL exata, o método HTTP (GET/POST) e a mensagem de status amigável, facilitando a depuração por parte da equipe de DevOps.

## 🛠 Snippets de Lógica Principal

### Captura Automática de Linha do Erro
Abaixo, a lógica inteligente que analisa a "pilha de chamadas" para encontrar a origem do problema:

`csharp
// Extração via Regex para precisão de diagnóstico
if (!string.IsNullOrEmpty(ex.StackTrace)) {
    var match = Regex.Match(ex.StackTrace, @":line (\d+)");
    if (match.Success && int.TryParse(match.Groups[1].Value, out var l)) {
        linha = l; // Identificou a linha exata no código .cs
    }
}

// Registro centralizado integrado ao LogService
logService.Error($"Exceção não tratada: {ex.Message}", ex, arquivo, metodo, linha);
`

## 📝 Notas de Implementação

- **Seamless Integration:** Através do método de extensão UseErrorLogging, o middleware é injetado no Program.cs de forma transparente, protegendo todo o processamento da aplicação com uma única linha de código.
- **Dicionário de Status:** O middleware possui um mapeador interno que converte códigos técnicos (como 429) em descrições humanas ("Muitas requisições"), enriquecendo os relatórios de erros.
- **Não-Bloqueante:** O middleware utiliza o padrão wait _next(context), garantindo que a monitoração não adicione latência perceptível ao tempo de resposta do usuário.

---
*Documentação de resiliência de sistema - FrotiX 2026. Segurança em cada camada do pipeline.*


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
