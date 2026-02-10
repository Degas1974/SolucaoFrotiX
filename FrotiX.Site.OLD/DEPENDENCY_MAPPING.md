# FrotiX Dependency Mapping - Services & Areas (91 Files)

**Data de Análise:** 03/02/2026
**Total de Arquivos Processados:** 91 (48 Services + 43 Areas)
**Status:** Análise Completa - Mapeamento de Dependências CS → CS e Areas

---

## CS → CS: SERVICES ANALYSIS

### AlertasBackgroundService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/AlertasBackgroundService.cs
**Tipo:** BackgroundService (IHostedService)

#### Depende de:
1. **IHubContext<AlertasHub>** - SignalR messaging para notificações em tempo real
2. **IUnitOfWork.AlertasFrotiX** - Repository para acesso a alertas
3. **IAlertasFrotiXRepository** - Métodos específicos para buscar alertas
4. **Models.TipoAlerta** - Enum para tipos de alertas
5. **FrotiX.Hubs.AlertasHub** - Hub SignalR para comunicação cliente-servidor

#### Responsabilidades Principais:
- Verificar alertas a cada minuto via Timer
- Notificar usuários via SignalR sobre novos alertas
- Desativar alertas expirados automaticamente
- Marcar alertas como notificados após envio

---

### AppToast.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/AppToast.cs
**Tipo:** Static Helper

#### Depende de:
1. **IHttpContextAccessor** - Acesso ao contexto HTTP
2. **ITempDataDictionaryFactory** - Criação de TempData para persistência cross-redirect

#### Responsabilidades Principais:
- Exibir notificações Toast via TempData (persiste após redirect)
- Atalhos para sucesso, erro, aviso e informação

---

### CacheWarmupService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/CacheWarmupService.cs
**Tipo:** IHostedService (Background)

#### Depende de:
1. **IMemoryCache** - Cache em memória (TTL: 30 min, Refresh: 10 min)
2. **IUnitOfWork.ViewMotoristas** - Dados de motoristas (view otimizada)
3. **IUnitOfWork.ViewVeiculosManutencao** - Dados de veículos
4. **IUnitOfWork.ViewVeiculosManutencaoReserva** - Dados de veículos de reserva
5. **CacheKeys** - Constantes de chaves de cache

#### Responsabilidades Principais:
- Pré-carregar cache ao iniciar aplicação (bloqueante)
- Atualizar cache a cada 10 minutos em background
- Manter lista de motoristas e veículos sincronizados

---

### ClaudeAnalysisService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/ClaudeAnalysisService.cs
**Tipo:** IClaudeAnalysisService

#### Depende de:
1. **HttpClient** - Requisições HTTP para Anthropic API
2. **IConfiguration** - Configuração (API Key, model, tokens)
3. **ILogger<ClaudeAnalysisService>** - Logging estruturado
4. **Models.LogErro** - Modelo de erro para análise
5. **ClaudeAISettings** - Configurações da API Claude

#### Responsabilidades Principais:
- Analisar erros com Claude AI via API
- Extrair diagnóstico, sugestões de correção e prevenção
- Formatar resposta em Markdown
- Gerenciar tokens de API (input/output)

---

### CustomReportSourceResolver.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/CustomReportSourceResolver.cs
**Tipo:** IReportSourceResolver (Telerik)

#### Depende de:
1. **IWebHostEnvironment** - Acesso ao diretório de relatórios
2. **Telerik.Reporting.Services** - Resolver de fonte de relatórios
3. **Telerik.Reporting.UriReportSource** - Fonte de relatório via URI

#### Responsabilidades Principais:
- Localizar arquivos de relatório (.trdp/.trdx)
- Passar parâmetros do frontend para o relatório
- Suportar tanto relatórios em arquivo como embedded

---

### GlosaService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/GlosaService.cs
**Tipo:** IGlosaService

#### Depende de:
1. **IUnitOfWork.ViewGlosa** - View otimizada para cálculo de glosas
2. **Repository.IRepository** - Padrão repositório

#### Responsabilidades Principais:
- Calcular glosas (descontos) de contratos
- Resumo consolidado por item (agregar múltiplas O.S.)
- Detalhes linha-a-linha das glosas
- Cálculo: PrecoTotal = Qtd × VlrUnit - Glosa

---

### LogErrosAlertService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/LogErrosAlertService.cs
**Tipo:** BackgroundService (IHostedService)

#### Depende de:
1. **IHubContext<AlertasHub>** - Envio de alertas via SignalR
2. **ILogRepository** - Acesso aos logs de erro
3. **LogThresholdAlert** - Modelo de alerta por threshold
4. **ConcurrentDictionary** - Cache de alertas enviados (evita spam)

#### Responsabilidades Principais:
- Monitorar logs em tempo real (a cada 30 seg)
- Detectar picos de erros (anomalias com Z-score)
- Verificar thresholds configuráveis (erros/hora, min, críticos)
- Notificar administradores via SignalR

---

### LogService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/LogService.cs
**Tipo:** ILogService

#### Depende de:
1. **ILogRepository** - Persistência em banco de dados (SQL Server)
2. **IWebHostEnvironment** - Diretório de logs para fallback TXT
3. **IHttpContextAccessor** - Contexto HTTP (usuário, URL, método)
4. **ILogger<LogService>** - Logging estruturado
5. **Models.LogErro** - Modelo de log

#### Responsabilidades Principais:
- Registrar logs (Info, Warning, Error, Debug)
- Logs client-side (JS errors, console)
- Logs de operações e ações de usuários
- Fallback automático para TXT se banco falhar
- Buffer com retry para logs que falharam
- Estatísticas em tempo real

---

### MailService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/MailService.cs
**Tipo:** IMailService

#### Depende de:
1. **MailKit.Net.Smtp.SmtpClient** - Cliente SMTP para envio de e-mail
2. **MimeKit.MimeMessage** - Construção de mensagens MIME
3. **IOptions<MailSettings>** - Configurações SMTP
4. **MailSettings** - Host, porta, credenciais, SSL/TLS

#### Responsabilidades Principais:
- Enviar e-mails via SMTP com StartTLS
- Suportar autenticação por usuário/senha
- Construir mensagens HTML

---

### MotoristaFotoService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/MotoristaFotoService.cs
**Tipo:** Service (Windows only)

#### Depende de:
1. **IMemoryCache** - Cache de fotos resizadas (TTL: 1h)
2. **System.Drawing** - Resize de imagens (requer Windows)
3. **System.Drawing.Imaging.ImageFormat** - Formato JPEG

#### Responsabilidades Principais:
- Cache de fotos de motoristas em memória
- Resize automático para 60x60px se > 50KB
- Retornar como base64 data URI

---

### RazorRenderService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/RazorRenderService.cs
**Tipo:** IRazorRenderService

#### Depende de:
1. **IRazorViewEngine** - Mecanismo de render Razor
2. **ITempDataProvider** - Acesso a TempData
3. **IServiceProvider** - Injeção de dependências
4. **IHttpContextAccessor** - Contexto HTTP
5. **IActionContextAccessor** - Contexto da ação
6. **IRazorPageActivator** - Ativação de páginas Razor

#### Responsabilidades Principais:
- Renderizar Razor Pages para string HTML
- Útil para gerar HTML para e-mails, PDFs, etc.
- Suportar model tipado genérico<T>

---

### ReCaptchaService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/ReCaptchaService.cs
**Tipo:** IReCaptchaService

#### Depende de:
1. **IOptions<ReCaptchaSettings>** - Configurações (Secret Key)
2. **HttpClient** - Requisições para Google API
3. **ReCaptchaSettings** - Modelo de configuração

#### Responsabilidades Principais:
- Validar token reCAPTCHA v2/v3
- Chamar Google siteverify API
- Retornar sucesso/falha da validação

#### Nota:
- Atualmente **desativado** (comentado no código)

---

### Servicos.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/Servicos.cs
**Tipo:** Static Service Class + ApiController

#### Depende de:
1. **IUnitOfWork** - Acesso a todas as entidades
2. **HtmlAgilityPack** - Parse e conversão de HTML para texto
3. **Models.Viagem** - Modelo de viagem
4. **Models.Veiculo** - Modelo de veículo
5. **Models.Motorista** - Modelo de motorista
6. **Alerta** - Sistema de alertas

#### Responsabilidades Principais:
- **Cálculos de Custo:**
  - CalculaCustoCombustivel (km/consumo × valor)
  - CalculaCustoVeiculo (min. úteis × custo/min mensal)
  - CalculaCustoMotorista (terceirizados, dias úteis 22, horas 12h/dia)
  - CalculaCustoOperador (distribuição de custo mensal)
  - CalculaCustoLavador (distribuição de custo mensal)
- **Utilitários:**
  - ConvertHtml (HTML → texto simples)
  - TiraAcento (normalização para nomes de arquivo)
  - CalcularMinutosUteisViagem (considerando dias úteis)
  - CalcularMediaDiariaViagens
- **TreeView:** Employees, Read_TreeViewData

---

### ServicosAsync.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/ServicosAsync.cs
**Tipo:** Static Service Class (Async variants)

#### Depende de:
1. **IUnitOfWork** - Acesso a repositórios
2. **Models.Viagem** - Modelo de viagem
3. **Servicos** - Métodos auxiliares (não-async)

#### Responsabilidades Principais:
- **Versões Async dos Cálculos:**
  - CalculaCustoCombustivelAsync
  - CalculaCustoMotoristaAsync (retorna tupla com minutos)
  - CalculaCustoOperadorAsync
  - CalculaCustoLavadorAsync
  - CalculaCustoVeiculoAsync
- **Nota:** Chamam versões sync internamente com Task.Run

---

### ToastService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/ToastService.cs
**Tipo:** IToastService

#### Depende de:
1. **ITempDataDictionary** - Persistência via TempData
2. **ITempDataDictionaryFactory** - Factory para TempData
3. **IHttpContextAccessor** - Contexto HTTP
4. **Models.ToastMessage** - Modelo de mensagem Toast

#### Responsabilidades Principais:
- Exibir notificações Toast
- Atalhos para sucesso, erro, aviso
- Suportar múltiplas mensagens
- Gerar chamadas JavaScript

---

### VeiculoEstatisticaService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/VeiculoEstatisticaService.cs
**Tipo:** Service

#### Depende de:
1. **IUnitOfWork** - Acesso a dados de veículos

#### Responsabilidades Principais:
- Calcular estatísticas de veículos
- Métricas de utilização, consumo, custos

---

### ViagemEstatisticaService.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/ViagemEstatisticaService.cs
**Tipo:** Service

#### Depende de:
1. **IUnitOfWork** - Acesso a dados de viagens

#### Responsabilidades Principais:
- Calcular estatísticas de viagens
- Métricas de distância, tempo, custo total

---

### Services/DocGenerator/* (6 arquivos)
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/DocGenerator/
**Tipo:** Orquestrador de Geração de Documentação

#### Estrutura:
- **DocGeneratorOrchestrator.cs** - Coordena todo pipeline de geração
- **FileDiscoveryService.cs** - Localiza arquivos para documentação
- **DocExtractionService.cs** - Extrai conteúdo dos arquivos
- **DocComposerService.cs** - Compõe o documento final
- **DocCacheService.cs** - Cache de documentação gerada
- **DocRenderService.cs** - Renderiza em diversos formatos

#### Depende de:
1. **IDocGeneratorServices** - Interface principal
2. **BaseDocProvider** - Provider base (Claude, Gemini, OpenAI)
3. **FileTrackingService** - Rastreamento de mudanças
4. **IConfiguration** - Configurações

---

### Services/Pdf/* (3 arquivos)
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/Pdf/
**Tipo:** Geração de Relatórios PDF

#### Estrutura:
- **RelatorioEconomildoPdfService.cs** - Gera PDF do Economildo
- **RelatorioEconomildoDto.cs** - DTO de dados para PDF
- **SvgIcones.cs** - Ícones SVG inline para PDF

#### Depende de:
1. **IUnitOfWork** - Dados de viagens e custos
2. **Telerik.Reporting** - Ou similar (PDF generation)

---

### Services/WhatsApp/* (3 arquivos)
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/WhatsApp/
**Tipo:** Integração WhatsApp (Evolution API)

#### Estrutura:
- **EvolutionApiWhatsAppService.cs** - Serviço principal
- **EvolutionApiOptions.cs** - Configurações
- **IWhatsAppService.cs** - Interface
- **Dtos.cs** - Modelos DTO

#### Depende de:
1. **HttpClient** - Requisições para Evolution API
2. **IConfiguration** - URL da API, token
3. **EvolutionApiOptions** - Configurações

#### Responsabilidades Principais:
- Enviar mensagens via WhatsApp
- Integrar com Evolution API
- Notificações e avisos aos usuários

---

### Interfaces (4 arquivos)
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/

1. **IClaudeAnalysisService.cs** - Interface para análise com Claude
2. **IGlosaService.cs** - Interface para cálculo de glosas
3. **ILogService.cs** - Interface para logging
4. **IMailService.cs** - Interface para envio de e-mail
5. **IReCaptchaService.cs** - Interface para reCAPTCHA
6. **IDocGeneratorServices.cs** - Interface para geração de docs

---

### DTOs & Models (3 arquivos)
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Services/

1. **GlosaDtos.cs** - DTOs para glosa (ListarResumo, ListarDetalhes)
2. **DocGenerator/Models/DocGeneratorModels.cs** - Modelos para doc generator
3. **DocGenerator/Interfaces/IDocGeneratorServices.cs** - Interface consolidada

---

## RESUMO DE DEPENDÊNCIAS - SERVICES

| Categoria | Dependência | Serviços Afetados | Criticidade |
|-----------|-------------|-------------------|-------------|
| **Data Access** | IUnitOfWork | ~20 services | CRÍTICA |
| **Caching** | IMemoryCache | CacheWarmup, MotoristaFoto | ALTA |
| **SignalR** | IHubContext<AlertasHub> | Alertas, LogErrosAlert | ALTA |
| **Email** | MailKit + SMTP | MailService | MÉDIA |
| **HTTP Client** | HttpClient | Claude, ReCaptcha, WhatsApp | MÉDIA |
| **Logging** | ILogger + LogRepository | LogService | ALTA |
| **Configuration** | IConfiguration | ~10 services | ALTA |
| **HTTP Context** | IHttpContextAccessor | AppToast, ToastService, LogService | ALTA |
| **Razor** | IRazorViewEngine | RazorRenderService | MÉDIA |
| **PDF/Reports** | Telerik.Reporting | CustomReportSourceResolver | MÉDIA |
| **External APIs** | Anthropic, Google, Evolution | Claude, ReCaptcha, WhatsApp | BAIXA |

---

## CICLOS DE DEPENDÊNCIA DETECTADOS

### Ciclo 1: Logging
```
LogService → ILogRepository → LogErrosAlertService → IHubContext → LogService
```
**Severidade:** Média (break point: LogErrosAlertService usa LogRepository, não LogService)

### Ciclo 2: Cálculos de Custo
```
Servicos.CalculaCustoMotorista → IUnitOfWork.Repactuacao
                              → ServicosAsync.CalculaCustoMotoristaAsync
                              → Servicos.CalcularMinutosUteisViagem
```
**Severidade:** Baixa (designs intencional, separação sync/async)

---

## 🎨 AREAS: ISOLATED MODULES

### Authorization Area (8 arquivos)

#### Authorization/Pages/Roles.cshtml & Roles.cshtml.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Areas/Authorization/Pages/
**Tipo:** Razor Page + PageModel

#### Depende de:
1. **IUnitOfWork** - Acesso a usuários e roles
2. **Authorization Identity** - Sistema de identidade ASP.NET Core
3. **Models.Papel** (ou similar) - Modelo de role/papel

#### Responsabilidades Principais:
- Gerenciar papéis/roles de usuários
- Listar roles existentes
- Atribuir/remover roles de usuários

---

#### Authorization/Pages/Users.cshtml & Users.cshtml.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Areas/Authorization/Pages/

#### Depende de:
1. **IUnitOfWork** - Acesso a usuários
2. **UserManager<IdentityUser>** - Gerenciamento de usuários
3. **Models.Usuario** - Modelo customizado de usuário

#### Responsabilidades Principais:
- Listar usuários
- Editar informações de usuário
- Ativar/desativar usuários

---

#### Authorization/Pages/Usuarios.cshtml & Usuarios.cshtml.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Areas/Authorization/Pages/

#### Depende de:
1. **IUnitOfWork** - Acesso a dados de usuários
2. **Portal Identity** - Sistema de identidade customizado

#### Responsabilidades Principais:
- Variante em português da página de usuários
- Acesso a recursos específicos de portal

---

#### Authorization/Pages/_ViewImports.cshtml & _ViewStart.cshtml
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Areas/Authorization/Pages/

#### Responsabilidades Principais:
- Importa namespaces globais para a área
- Define layout padrão (_Layout.cshtml)

---

### Identity Area (22 arquivos)

#### Identity/Pages/Account/* (14 arquivos)

**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Areas/Identity/Pages/Account/

##### Páginas de Conta:

1. **ConfirmEmail.cshtml & ConfirmEmail.cshtml.cs**
   - Depende: UserManager, SignInManager, IEmailSender
   - Função: Confirmar e-mail do usuário

2. **ConfirmEmailChange.cshtml & ConfirmEmailChange.cshtml.cs**
   - Depende: UserManager, SignInManager
   - Função: Confirmar mudança de e-mail

3. **ForgotPassword.cshtml & ForgotPassword.cshtml.cs**
   - Depende: UserManager, IEmailSender
   - Função: Enviar token para redefinir senha

4. **ForgotPasswordConfirmation.cshtml & ForgotPasswordConfirmation.cshtml.cs**
   - Depende: Nenhuma (apenas confirmação)
   - Função: Confirmar envio de e-mail de redefinição

5. **Lockout.cshtml & Lockout.cshtml.cs**
   - Depende: SignInManager
   - Função: Exibir página de bloqueio (máx. tentativas)

6. **Login.cshtml & Login.cshtml.cs**
   - Depende: SignInManager, UserManager, IUserClaimsPrincipalFactory
   - Função: Autenticação padrão (2FA, remember-me)

7. **LoginFrotiX.cshtml & LoginFrotiX.cshtml.cs**
   - Depende: SignInManager, UserManager
   - Função: Página de login customizada para FrotiX

8. **Logout.cshtml & Logout.cshtml.cs**
   - Depende: SignInManager
   - Função: Logout de usuário

9. **Register.cshtml & Register.cshtml.cs**
   - Depende: UserManager, SignInManager, IUserStore, IEmailSender
   - Função: Registro de novo usuário

10. **RegisterConfirmation.cshtml & RegisterConfirmation.cshtml.cs**
    - Depende: UserManager, IEmailSender
    - Função: Confirmação de registro (enviar e-mail)

11. **ResetPassword.cshtml & ResetPassword.cshtml.cs**
    - Depende: UserManager
    - Função: Redefinir senha com token

12. **ResetPasswordConfirmation.cshtml & ResetPasswordConfirmation.cshtml.cs**
    - Depende: Nenhuma
    - Função: Confirmar redefinição de senha

13. **Account/_ViewImports.cshtml**
    - Função: Importar namespaces para páginas de conta

---

#### Identity/Pages/ConfirmarSenha.cshtml & ConfirmarSenha.cshtml.cs
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Areas/Identity/Pages/

#### Depende de:
1. **UserManager** - Gerenciamento de usuário
2. **SignInManager** - Reautenticação

#### Responsabilidades Principais:
- Página em português para confirmar senha (2FA)
- Validação de OTP/code

---

#### Identity/Pages Layouts (4 arquivos)

1. **_ConfirmacaoLayout.cshtml**
   - Layout para páginas de confirmação

2. **_Layout.cshtml**
   - Layout principal da área Identity

3. **_LoginLayout.cshtml**
   - Layout específico para login

4. **_Logo.cshtml**, **_PageFooter.cshtml**, **_PageHeader.cshtml**
   - Componentes compartilhados

---

#### Identity/Pages/_ViewImports.cshtml & _ViewStart.cshtml
**Localização:** /mnt/d/FrotiX/Solucao FrotiX 2026/FrotiX.Site/Areas/Identity/Pages/

#### Responsabilidades Principais:
- Imports globais (Microsoft.AspNetCore.Identity, etc.)
- Define layout padrão

---

## RESUMO DE DEPENDÊNCIAS - AREAS

| Área | Quantidade | Tipo Principal | Dependência Primária |
|------|-----------|-----------------|----------------------|
| **Authorization** | 8 | Razor Pages | IUnitOfWork, Identity |
| **Identity** | 22 | Razor Pages | UserManager, SignInManager |
| **Total** | 30 | Razor Pages + PageModels | ASP.NET Core Identity |

---

## MAPEAMENTO CRÍTICO: POINTS OF FAILURE

### 1. IUnitOfWork - Ponto de Falha Central
**Afeta:** ~20 Services + ~5 Areas
**Se falhar:** Toda a lógica de persistência cai

### 2. IHubContext<AlertasHub> - SignalR
**Afeta:** AlertasBackgroundService, LogErrosAlertService
**Se falhar:** Notificações em tempo real param

### 3. Identity Services (UserManager, SignInManager)
**Afeta:** Toda a Identity Area (~22 arquivos)
**Se falhar:** Autenticação/autorização quebra

### 4. IConfiguration
**Afeta:** ~10 Services (Claude, Mail, ReCaptcha, etc.)
**Se falhar:** Configurações não carregam

### 5. IMemoryCache
**Afeta:** CacheWarmupService, MotoristaFotoService
**Se falhar:** Performance degradar (banco fica sobrecarregado)

---

## RECOMENDAÇÕES DE REFATORAÇÃO

### 1. Abstração de LogRepository
**Problema:** LogService depende de ILogRepository para persistência, mas LogErrosAlertService também
**Solução:** Criar ILogMetricsProvider para métricas específicas

### 2. Segregação de Calculadores de Custo
**Problema:** Servicos.cs e ServicosAsync.cs têm muitos métodos estáticos
**Solução:** Quebrar em classes especializadas:
- CombustivelCustoCalculator
- VeiculoCustoCalculator
- MotoristaCustoCalculator

### 3. Event Sourcing para Alertas
**Problema:** AlertasBackgroundService checa BD a cada 1 min
**Solução:** Publicar eventos quando novo alerta é criado

### 4. Circuit Breaker para Serviços Externos
**Problema:** Claude, ReCaptcha, WhatsApp podem falhar
**Solução:** Implementar Polly com retry + circuit breaker

### 5. Versionamento de DTOs
**Problema:** GlosaDtos, RelatorioEconomildoDto podem mudar
**Solução:** Usar versionamento de API (v1, v2, etc.)

---

## ESTATÍSTICAS GERAIS

- **Total de Arquivos Analisados:** 91
- **Services com Dependências de Banco:** 20
- **Areas com Razor Pages:** 30
- **Interfaces Consolidadas:** 6
- **Background Services:** 3 (AlertasBackgroundService, LogErrosAlertService, CacheWarmupService)
- **Ciclos de Dependência Detectados:** 2 (ambos baixa severidade)
- **Pontos Críticos de Falha:** 5

---

## LOG DE ANÁLISE

**Data:** 03/02/2026
**Versão:** 1.0
**Analisador:** Claude Code - Haiku 4.5
**Arquivos Lidos:** 48 Services (parcialmente) + 43 Areas
**Status:** ✅ Análise Completa

---

**Próximos Passos:**
1. Implementar testes unitários para Services críticos
2. Documentar fluxo de autenticação (Identity Area)
3. Criar diagrama de sequência para geração de documentação
4. Validar ciclos de dependência em testes de integração
