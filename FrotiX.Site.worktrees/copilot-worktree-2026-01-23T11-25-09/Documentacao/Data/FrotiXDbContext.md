# O Coração do Schema de Dados (Contexto de Banco)

O FrotiXDbContext é a fundação de dados de toda a solução. Como contexto principal do Entity Framework Core, ele mapeia a complexa teia de relacionamentos entre veículos, motoristas, contratos e custos. Sua arquitetura é projetada para suportar alta carga de dados e operações de longa duração, típicas de sistemas de logística pública.

## 🏛 Arquitetura do Contexto

O contexto não é apenas uma lista de tabelas; ele é um orquestrador de schema dinâmico.

### Características Mandatórias

1. **Alta Disponibilidade (Timeout):** Devido às operações de massa (como o cálculo de custos de 500.000 viagens), o contexto é configurado com um CommandTimeout estendido (9000 segundos). Isso evita que jobs de estatísticas sejam interrompidos por limitações padrão de rede.
2. **Organização por Classes Parciais:** O contexto utiliza o padrão partial class (ex: FrotiXDbContext.OcorrenciaViagem.cs). Isso permite que o mapeamento de centenas de tabelas e views seja modularizado, facilitando a manutenção e prevenindo conflitos de merge em grandes arquivos.
3. **Mapeamento de Views SQL:** Além de tabelas físicas, o contexto mapeia dezenas de **Views SQL** otimizadas (DbQuery ou DbSet sem chave, dependendo da versão do EF). Isso permite que o FrotiX realize consultas complexas com a performance do SQL nativo e a facilidade do LINQ.

## 🛠 Snippets de Mapeamento Principal

### Configuração de Resiliência

Abaixo, a configuração de inicialização que garante suporte a backups e jobs demorados:

`csharp
public partial class FrotiXDbContext : DbContext {
    public FrotiXDbContext(DbContextOptions<FrotiXDbContext> options) : base(options) {
        // Timeout de 150 minutos para processamento de massa
        Database.SetCommandTimeout(9000);
    }
}
`

## 📝 Notas de Implementação

- **Estatísticas Persistentes:** O contexto mapeia tabelas específicas de KPIs (ViagemEstatistica, AbastecimentoEstatistica), que são alimentadas pelos serviços de inteligência de negócio.
- **Relacionamentos Complexos:** Tabelas como AspNetUsers (Identity) são integradas diretamente ao schema de negócios, permitindo que a auditoria (UsuarioIdAlteracao) seja feita via integridade referencial forte (Foreign Keys).
- **Views de Performance:** Views como ViewViagens e ViewAbastecimentos são tratadas como entidades de leitura, permitindo que o sistema projete dados complexos sem a necessidade de múltiplos Include() manuais.

---

## 📜 Log de Modificações

| Data       | Autor  | Descrição da Alteração                                                                         |
| :--------- | :----- | :--------------------------------------------------------------------------------------------- |
| 16/01/2026 | Gemini | Inclusão dos novos DbSets e mapeamento de views sem chave (HasNoKey) para o módulo de Escalas. |

---

_Documentacao de estrutura de dados - FrotiX 2026. A base sólida para a mobilidade._

---

## PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [21/01/2026] - AtualizaÃ§Ã£o: PrecisÃ£o Decimal PadrÃ£o no Modelo EF Core

**DescriÃ§Ã£o**: ConfiguraÃ§Ã£o global de precisÃ£o/escala para propriedades `decimal` sem tipo definido, evitando truncamento silencioso e warnings do EF Core.

**Arquivos Afetados**:

- `Data/FrotiXDbContext.ModelConfiguration.cs`

**MudanÃ§as**:

- DefiniÃ§Ã£o de `HasPrecision(18, 4)` para `decimal` e `decimal?` sem precisÃ£o configurada.

**Motivo**:

- Eliminar warnings de validaÃ§Ã£o do modelo e garantir consistÃªncia de dados.

**Impacto**:

- ReduÃ§Ã£o de warnings na inicializaÃ§Ã£o do EF Core.
- ProteÃ§Ã£o contra truncamento de valores de estatÃ­sticas e KPIs.

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:

- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:

- âŒ **ANTES**: `_unitOfWork.Entity.AsTracking().Get(id)` ou `_unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)`
- âœ… **AGORA**: `_unitOfWork.Entity.GetWithTracking(id)` ou `_unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)`

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
