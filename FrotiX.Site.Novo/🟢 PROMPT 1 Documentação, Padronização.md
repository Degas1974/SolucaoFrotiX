🟢 PROMPT 1: Documentação, Padronização Visual e Mapeamento
Este prompt foca exclusivamente na estrutura, beleza dos cards, regras no arquivo correto e mapeamento de dependências.

Você agora atua como o **Arquiteto Líder de Documentação do FrotiX**.
Sua missão é elevar o padrão de documentação do código para um nível de excelência visual e funcional, garantindo rastreabilidade total.

Siga rigorosamente as etapas abaixo.

### ETAPA 1: ATUALIZAÇÃO DA LEI (Arquivo `RegrasDesenvolvimentoFrotiX.md`)

O arquivo `DocumentacaoIntracodigo.md` deve ser usado para mapear o andamento do processo de documentação, caso precisemos parar e reiniciar novamente. Ele começa vazio e vai sendo preenchido com cada arquivo terminado.

Todas as regras devem residir exclusivamente no arquivo raiz **`RegrasDesenvolvimentoFrotiX.md`**.

Atualize este arquivo raiz adicionando/substituindo a seção de "Documentação Intra-Código" com os seguintes novos padrões visuais (mais ricos e detalhados):

#### 1.1. Card do Arquivo (Table of Contents)
**REGRA:** Todo arquivo (.cs ou .js) DEVE iniciar com um Card Mestre contendo o índice de suas funcionalidades.

**Modelo Visual:**
```csharp
/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: NomeDoArquivo.cs                                                                       ║
║ 📂 CAMINHO: /Pasta/Subpasta                                                                        ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Descrever brevemente a responsabilidade desta classe ou módulo.                                 ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
║ 1. [NomeFuncao1] : Breve descrição.............. (int id) -> bool                                  ║
║ 2. [NomeFuncao2] : Outra descrição.............. (string x) -> ActionResult                        ║
║ ...                                                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ ⚠️ MANUTENÇÃO:                                                                                     ║
║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

1.2. Card da Função (Rico em Ícones)
REGRA: O cabeçalho deve ser visualmente impactante, mantendo a compatibilidade com IntelliSense (XML Docs/JSDoc).

Modelo Visual C#:

/// <summary>
/// ╭───────────────────────────────────────────────────────────────────────────────────────╮
/// │ ⚡ FUNCIONALIDADE: NomeDaFuncao                                                       │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
/// │    Explicação clara da regra de negócio, comportamento e validações.                  │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 📥 INPUTS (Entradas):                                                                 │
/// │    • param1 [int]: Descrição do parâmetro.                                            │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 📤 OUTPUTS (Saídas):                                                                  │
/// │    • [bool]: O que retorna e em que condições.                                        │
/// │───────────────────────────────────────────────────────────────────────────────────────│
/// │ 🔗 RASTREABILIDADE (Quem chama e Quem é chamado):                                     │
/// │    ⬅️ CHAMADO POR : [Mapear quem invoca este método]                                  │
/// │    ➡️ CHAMA       : [Mapear serviços/métodos invocados internamente]                  │
/// ╰───────────────────────────────────────────────────────────────────────────────────────╯
/// </summary>

(Para JavaScript, use o mesmo visual de caixa ASCII dentro de blocos /** ... */)

ETAPA 2: MAPEAMENTO DE DEPENDÊNCIAS (Inteligência)
Antes de alterar os códigos, analise o projeto e crie/atualize o arquivo MapeamentoDependencias.md na raiz. Liste as relações cruzadas para que possamos preencher os campos ⬅️ CHAMADO POR e ➡️ CHAMA corretamente.

Formato esperado:

Tabela 1: Endpoints C# (Controller/Action) x Quem consome no JS.

Tabela 2: Funções JS Globais x Quem as invoca.

Tabela 3: Métodos de Serviço C# x Controllers que os utilizam.

ETAPA 3: EXECUÇÃO E VARREDURA
Vamos reiniciar a análise de TODAS as pastas abaixo. Se o arquivo já tiver documentação, atualize para o novo visual "Bonito" e verifique se o Card do Arquivo está atualizado com todas as funções. Se não tiver, crie. Verifique também se todas as funções possuem o nosso tratamento padrão Try-Catch. Se não possuírem, insira onde estiver faltando. Acrescente este dado quando for atualizar o arquivo 

Escopo de Varredura:

Areas

Controllers

Data

EndPoints

Extensions

Filters

Helpers

Hubs

Infrastructure

Logging

Middlewares

Models

Pages (Atenção: Documentar blocos <script> JS dentro dos .cshtml)

Properties

Repository

Services

Settings

Tools

AÇÃO AGORA:

Faça um levantamento do número total de arquivos a serem trabalhados, e use este dado para me dar um feedback visual do percentual de arquivos já trabalhados, vá atualizando a cada 1%

Confirme que atualizou o RegrasDesenvolvimentoFrotiX.md.

Gere o MapeamentoDependencias.md.

Comece a aplicar os novos Cards Visuais (Arquivo e Função) pasta por pasta, incluindo subdiretórios

A cada 10 arquivos modificados, dê comit e pull para o Main