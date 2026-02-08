# O Coração do Schema de Dados (Contexto de Banco)

O FrotiXDbContext é a fundação de dados de toda a solução. Como contexto principal do Entity Framework Core, ele mapeia a complexa teia de relacionamentos entre veículos, motoristas, contratos e custos. Sua arquitetura é projetada para suportar alta carga de dados e operações de longa duração, típicas de sistemas de logística pública.

## 🏛 Arquitetura do Contexto

O contexto não é apenas uma lista de tabelas; ele é um orquestrador de schema dinâmico.

### Características Mandatórias:

1.  **Alta Disponibilidade (Timeout):** Devido às operações de massa (como o cálculo de custos de 500.000 viagens), o contexto é configurado com um CommandTimeout estendido (9000 segundos). Isso evita que jobs de estatísticas sejam interrompidos por limitações padrão de rede.
2.  **Organização por Classes Parciais:** O contexto utiliza o padrão partial class (ex: FrotiXDbContext.OcorrenciaViagem.cs). Isso permite que o mapeamento de centenas de tabelas e views seja modularizado, facilitando a manutenção e prevenindo conflitos de merge em grandes arquivos.
3.  **Mapeamento de Views SQL:** Além de tabelas físicas, o contexto mapeia dezenas de **Views SQL** otimizadas (DbQuery ou DbSet sem chave, dependendo da versão do EF). Isso permite que o FrotiX realize consultas complexas com a performance do SQL nativo e a facilidade do LINQ.

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
