# DeepSeek IDE

IDE desktop similar ao VS Code com integração da API DeepSeek para assistência de IA em programação.

## Funcionalidades

- **Editor de Código** - Monaco Editor com syntax highlighting para múltiplas linguagens
- **Explorador de Arquivos** - Navegação em pastas e abertura de arquivos locais
- **Integração Git** - Status, commit, push, pull integrados via LibGit2Sharp
- **Chat com IA** - Converse com o DeepSeek para tirar dúvidas e obter ajuda
- **Análise de Código** - Envie código para revisão automática pela IA
- **Interface Moderna** - Design inspirado no VS Code com tema escuro

## Requisitos

- Windows 10/11
- .NET 8.0 Runtime
- WebView2 Runtime (vem com Windows 11, ou instale separadamente)
- Conexão com internet (para Monaco Editor e API DeepSeek)

## Como Executar

```bash
cd DeepSeekIDE
dotnet run --project DeepSeekIDE.App
```

Ou compile e execute o executável:

```bash
dotnet build -c Release
.\DeepSeekIDE.App\bin\Release\net8.0-windows\DeepSeekIDE.exe
```

## Estrutura do Projeto

```
DeepSeekIDE/
├── DeepSeekIDE.sln              # Solution
├── DeepSeekIDE.App/             # Aplicação WPF
│   ├── MainWindow.xaml          # Interface principal
│   ├── MainWindow.xaml.cs       # Lógica da interface
│   └── appsettings.json         # Configurações
├── DeepSeekIDE.Core/            # Biblioteca de serviços
│   ├── Models/                  # Modelos de dados
│   │   ├── ChatMessage.cs       # Mensagens do chat
│   │   ├── FileSystemItem.cs    # Itens do explorador
│   │   └── GitModels.cs         # Modelos Git
│   └── Services/                # Serviços
│       ├── DeepSeekService.cs   # Integração com API DeepSeek
│       ├── FileSystemService.cs # Manipulação de arquivos
│       └── GitService.cs        # Operações Git
└── DeepSeekIDE.Api/             # API Backend (futuro)
```

## Configuração

A chave da API DeepSeek está configurada em:
- `MainWindow.xaml.cs` (constante DEEPSEEK_API_KEY)
- `appsettings.json` (para uso futuro com DI)

Para alterar a chave, edite um dos arquivos acima.

## Análise de Arquivos por Nome

Digite apenas o nome do arquivo (ou parte dele) no campo "Analisar Arquivo" e pressione Enter. O IDE irá:
1. Buscar o arquivo no workspace atual
2. Carregar seu conteúdo
3. Enviar para análise pela IA com as regras configuradas

Exemplos de busca:
- `MainWindow.cs` - busca exata
- `Controller` - encontra arquivos que contenham "Controller" no nome
- `Pages/Home/Index.cshtml` - caminho relativo

## Arquivo de Regras (DEEPSEEK_RULES.md)

Crie um arquivo `DEEPSEEK_RULES.md` na raiz do seu projeto para definir regras personalizadas que serão aplicadas em todas as conversas.

Exemplo de conteúdo:
```markdown
# Regras do Projeto

## Padrões de Código
- Use async/await para operações de I/O
- Sempre adicione try-catch em métodos públicos
- Prefira injeção de dependência

## Convenções
- Nomes de classes em PascalCase
- Variáveis privadas com prefixo underscore (_)
```

O arquivo é carregado automaticamente quando você abre um workspace.

## Atalhos de Teclado

| Atalho | Ação |
|--------|------|
| Ctrl+S | Salvar arquivo |
| Ctrl+Shift+E | Mostrar Explorador |
| Ctrl+Shift+F | Mostrar Busca |
| Ctrl+Shift+G | Mostrar Git |
| Ctrl+Enter | Enviar mensagem no chat |
| Enter (no campo arquivo) | Analisar arquivo por nome |

## Tecnologias

- **WPF** - Interface desktop
- **WebView2** - Hospeda o Monaco Editor
- **Monaco Editor** - Editor de código (via CDN)
- **LibGit2Sharp** - Operações Git nativas
- **DeepSeek API** - IA para chat e análise de código

## API DeepSeek

O projeto usa a API DeepSeek compatível com OpenAI:
- Base URL: `https://api.deepseek.com/v1`
- Modelos: `deepseek-chat`, `deepseek-reasoner`
- Streaming: Suportado para respostas em tempo real

## Licença

Projeto interno FrotiX
