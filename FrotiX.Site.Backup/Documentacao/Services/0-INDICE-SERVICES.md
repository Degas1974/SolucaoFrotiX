# Índice - Services

## Visão Geral
Este diretório contém **serviços de negócio** e **utilitários** do sistema FrotiX. Os serviços são organizados por funcionalidade e incluem cálculos financeiros, logging, comunicação, processamento de dados e integrações externas.

## Status da Documentação
**22 arquivos documentados** (100% completo)

---

## 📁 Estrutura de Diretórios

### Raiz (`Services/`)
- [Servicos.md](./Servicos.md) - Cálculos de custos de viagens e utilitários
- [ServicosAsync.md](./ServicosAsync.md) - Versões assíncronas dos cálculos
- [Validations.md](./Validations.md) - Atributos de validação customizados
- [LogService.md](./LogService.md) - Serviço de logging centralizado
- [MailService.md](./MailService.md) - Envio de emails via SMTP
- [GlosaService.md](./GlosaService.md) - Cálculo e listagem de glosas
- [ToastService.md](./ToastService.md) - Notificações toast para frontend
- [ReCaptchaService.md](./ReCaptchaService.md) - Validação reCAPTCHA (parcial)
- [MotoristaFotoService.md](./MotoristaFotoService.md) - Processamento de fotos
- [RazorRenderService.md](./RazorRenderService.md) - Renderização Razor em strings
- [CacheWarmupService.md](./CacheWarmupService.md) - Warm-up de cache em background
- [AlertasBackgroundService.md](./AlertasBackgroundService.md) - Serviço de alertas em background
- [CustomReportSourceResolver.md](./CustomReportSourceResolver.md) - Resolver de relatórios Telerik
- [TelerikReportWarmupService.md](./TelerikReportWarmupService.md) - Warm-up do Telerik Report Server
- [VeiculoEstatisticaService.md](./VeiculoEstatisticaService.md) - Estatísticas de veículos
- [ViagemEstatisticaService.md](./ViagemEstatisticaService.md) - Estatísticas de viagens

### Subdiretório `Pdf/`
- [RelatorioEconomildoPdfService.md](./Pdf/RelatorioEconomildoPdfService.md) - Geração de PDFs do Dashboard Economildo
- [RelatorioEconomildoDto.md](./Pdf/RelatorioEconomildoDto.md) - DTOs para relatórios PDF
- [SvgIcones.md](./Pdf/SvgIcones.md) - Ícones SVG para PDFs

### Subdiretório `WhatsApp/`
- [EvolutionApiWhatsAppService.md](./WhatsApp/EvolutionApiWhatsAppService.md) - Integração com Evolution API (WhatsApp)
- [Dtos.md](./WhatsApp/Dtos.md) - DTOs para WhatsApp
- [EvolutionApiOptions.md](./WhatsApp/EvolutionApiOptions.md) - Configurações da Evolution API

---

## 📊 Categorização por Funcionalidade

### 💰 Cálculos Financeiros
- **Servicos.cs**: Cálculos de custos (combustível, veículo, motorista, operador, lavador)
- **ServicosAsync.cs**: Versões assíncronas otimizadas
- **GlosaService.cs**: Cálculo de glosas em contratos

### 📊 Estatísticas e Relatórios
- **VeiculoEstatisticaService.cs**: Estatísticas de viagens por veículo
- **ViagemEstatisticaService.cs**: Estatísticas consolidadas de viagens
- **RelatorioEconomildoPdfService.cs**: Geração de PDFs do Dashboard Economildo
- **CustomReportSourceResolver.cs**: Resolver de relatórios Telerik

### 🔔 Notificações e Comunicação
- **ToastService.cs**: Notificações toast no frontend
- **MailService.cs**: Envio de emails
- **AlertasBackgroundService.cs**: Alertas agendados via SignalR
- **EvolutionApiWhatsAppService.cs**: Integração WhatsApp

### 🛠️ Utilitários e Helpers
- **Validations.cs**: Atributos de validação
- **LogService.cs**: Logging centralizado
- **MotoristaFotoService.cs**: Processamento de imagens
- **RazorRenderService.cs**: Renderização Razor em strings
- **ReCaptchaService.cs**: Validação reCAPTCHA

### ⚡ Performance e Cache
- **CacheWarmupService.cs**: Warm-up de cache em background
- **TelerikReportWarmupService.cs**: Warm-up do Telerik Report Server

---

## 🔑 Serviços Críticos

### Alta Prioridade
1. **Servicos.cs**: Cálculos financeiros fundamentais para o sistema
2. **LogService.cs**: Rastreabilidade e debugging
3. **CacheWarmupService.cs**: Performance de dropdowns e autocompletes
4. **AlertasBackgroundService.cs**: Notificações em tempo real

### Média Prioridade
5. **GlosaService.cs**: Gestão financeira de contratos
6. **MailService.cs**: Comunicação com usuários
7. **VeiculoEstatisticaService.cs**: Análises e relatórios
8. **ViagemEstatisticaService.cs**: Estatísticas consolidadas

### Baixa Prioridade
9. **ToastService.cs**: Feedback visual (melhora UX)
10. **ReCaptchaService.cs**: Segurança (implementação parcial)
11. **MotoristaFotoService.cs**: Processamento de imagens
12. **RazorRenderService.cs**: Templates de email

---

## 📝 Observações Gerais

### Padrões Comuns
- **Injeção de Dependência**: Todos os serviços são injetáveis via DI container
- **Error Handling**: Maioria usa `Alerta.TratamentoErroComLinha()` para logging
- **Async/Await**: Serviços que fazem I/O usam métodos assíncronos
- **Background Services**: Serviços de background implementam `IHostedService`

### Dependências Externas
- **MailKit**: Envio de emails
- **Telerik Reporting**: Geração de relatórios
- **QuestPDF**: Geração de PDFs (Dashboard Economildo)
- **SignalR**: Notificações em tempo real
- **Evolution API**: Integração WhatsApp

### Configurações
- Maioria dos serviços usa `IOptions<T>` para configurações
- Configurações em `appsettings.json`
- Classes de configuração em `Settings/`

---

## 🔗 Relacionamentos

### Servicos.cs
- Usado por: `CustosViagemController`, `ServicosAsync`
- Usa: `IUnitOfWork`, `HtmlAgilityPack`

### LogService.cs
- Usado por: `ErrorLoggingMiddleware`, `Alerta`, `AlertaBackend`
- Usa: `IWebHostEnvironment`, `IHttpContextAccessor`

### CacheWarmupService.cs
- Usado por: Sistema automaticamente (IHostedService)
- Usa: `IUnitOfWork`, `IMemoryCache`

### AlertasBackgroundService.cs
- Usado por: Sistema automaticamente (BackgroundService)
- Usa: `IAlertasFrotiXRepository`, `AlertasHub` (SignalR)

---

## 📚 Documentação Relacionada
- [Controllers](../Controllers/0-INDICE-CONTROLLERS.md): Usam serviços para lógica de negócio
- [Repository](../Repository/0-INDICE-REPOSITORY.md): Acessados via `IUnitOfWork`
- [Helpers](../Helpers/0-INDICE-HELPERS.md): Utilitários complementares
- [Middlewares](../Middlewares/0-INDICE-MIDDLEWARES.md): Pipeline HTTP

---

**Última atualização**: Documentação completa de todos os 22 arquivos em `Services/` e subdiretórios.


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
