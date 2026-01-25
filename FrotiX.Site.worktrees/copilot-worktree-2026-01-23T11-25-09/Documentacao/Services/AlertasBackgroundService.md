# Monitor de Notificações em Tempo Real (Background Worker)

> **Última Atualização**: 23/01/2026 11:40  
> **Versão**: 1.1  
> **Documentação Intra-Código**: ✅ Completa (Cards adicionados)

O AlertasBackgroundService é o agente silencioso do FrotiX. Operando como um **BackgroundService** do ASP.NET Core, ele funciona de forma independente do fluxo de requisições HTTP, garantindo que o sistema "pense" e "aja" mesmo quando nenhum usuário está interagindo com a interface. Sua principal responsabilidade é orquestrar a entrega de notificações críticas via SignalR.

---

# PARTE 2: LOG DE MODIFICAÇÕES

## [23/01/2026 11:40] - Documentação Intra-Código Completa

**Descrição**: Adicionados Cards de documentação padrão RegrasDesenvolvimentoFrotiX.md  
**Arquivos Afetados**: AlertasBackgroundService.cs  
**Status**: ✅ Concluído

---

## 🛰 Arquitetura de Notificação "Push"

Diferente de sistemas que dependem do usuário atualizar a página (Pull), o FrotiX utiliza este serviço para empurrar informações (Push).

### Dinâmica de Funcionamento:

1.  **Batimento Cardíaco (Timer):** O serviço executa um ciclo de verificação a cada 60 segundos. Esse intervalo balanceia a agilidade da notificação com a economia de recursos do servidor.
2.  **Escalabilidade com HubContext:** Através do IHubContext<AlertasHub>, o serviço consegue "falar" com o frontend. Ele identifica quais usuários devem receber cada alerta e envia a carga de dados (título, ícone, prioridade) diretamente para a conexão ativa do navegador.
3.  **Auditoria de Entrega:** O serviço não apenas envia a mensagem; ele marca o registro na tabela AlertasUsuarios como Notificado = true. Isso garante que, se um usuário tiver múltiplas abas abertas, ele receba a notificação sem duplicações desnecessárias.

## 🛠 Snippets de Lógica Principal

### Ciclo de Verificação e Envio

A lógica central que conecta o banco de dados ao canal em tempo real (SignalR):

`csharp
foreach (var usuarioId in usuariosNaoNotificados) {
// DISPARO EM TEMPO REAL
await \_hubContext.Clients.User(usuarioId).SendAsync("NovoAlerta", new {
titulo = alerta.Titulo,
descricao = alerta.Descricao,
iconeCss = ObterIconePorTipo(alerta.TipoAlerta),
corBadge = ObterCorPorTipo(alerta.TipoAlerta)
});

    // MARCAÇÃO DE SUCESSO
    alertaUsuario.Notificado = true;

}
`

## 📝 Notas de Implementação

- **Gestão de Escopo:** Como o serviço de background tem ciclo de vida Singleton, ele utiliza IServiceProvider.CreateScope() para acessar os repositórios (Scoped), garantindo que a conexão com o banco de dados seja aberta e fechada corretamente a cada ciclo.
- **Auto-Limpeza (Expirados):** Além de notificar, o serviço é responsável pela higienização da base, desativando automaticamente alertas que ultrapassaram a DataExpiracao.
- **Tratamento de Erros Isolado:** Falhas no envio de um alerta específico (como um erro de rede no SignalR) não interrompem o ciclo. O serviço captura a exceção, registra o log e prossegue para o próximo usuário ou alerta.

---

_Documentação de arquitetura de serviços - FrotiX 2026. Comunicação instantânea para uma frota conectada._

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

- âŒ **ANTES**: \_unitOfWork.Entity.AsTracking().Get(id) ou \_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: \_unitOfWork.Entity.GetWithTracking(id) ou \_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

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
