# GEMINI.md - Contexto e Regras do Projeto FrotiX

Este arquivo define o contexto, regras mandatórias e padrões de desenvolvimento para o projeto FrotiX. Deve ser consultado no início de cada sessão para garantir alinhamento com as convenções do projeto.

## 1. Visão Geral do Projeto
- **Nome:** Solução FrotiX 2026 (FrotiX.Site)
- **Tipo:** Aplicação Web ASP.NET Core MVC (Gestão de Frotas)
- **Stack Backend:** .NET 10 (Preview/Custom), C#, Entity Framework Core, SQL Server.
- **Stack Frontend:** Bootstrap 5.3, jQuery, Syncfusion EJ2, Telerik UI.
- **Relatórios:** Stimulsoft Reports, QuestPDF.

## 2. Regras Mandatórias (Zero Tolerance)

### 2.1. Tratamento de Erros (Try-Catch)
**TODAS** as funções, sejam C# (Actions/Services) ou JavaScript, DEVEM ter tratamento de erro.
- **C#:** Envolver todo o corpo da Action em `try-catch`. No `catch`, usar `Alerta.TratamentoErroComLinha` e retornar JSON ou View de erro.
- **JavaScript:** Usar `try-catch` e chamar `Alerta.TratamentoErroComLinha("Arquivo.js", "Funcao", erro)`.

### 2.2. Interface e UX (Alertas)
**PROIBIDO:** Usar `alert()`, `confirm()` ou `prompt()` nativos do navegador.
**OBRIGATÓRIO:** Usar a biblioteca interna baseada em SweetAlert:
- `Alerta.Sucesso(titulo, msg)`
- `Alerta.Erro(titulo, msg)`
- `Alerta.Confirmar(titulo, msg, btnSim, btnNao).then(ok => ...)`
- `Alerta.Warning(titulo, msg)`

### 2.3. Ícones (FontAwesome Duotone)
**SEMPRE** usar estilo **Duotone**.
- **Classe:** `fa-duotone fa-[icone]`
- **Cores Padrão:**
  - Primária (Laranja): `#ff6b35` (`--fa-primary-color`)
  - Secundária (Cinza): `#6c757d` (`--fa-secondary-color`)
- **Proibido:** `fa-solid`, `fa-regular`, `fa-light`. Se encontrar, converter para `fa-duotone`.
- **Botão Voltar:** `fa-duotone fa-rotate-left icon-space icon-rotate-left`.

### 2.4. Loading Overlays
Sempre que houver uma operação assíncrona demorada (AJAX, Form Submit):
- Usar o padrão **FrotiX Spin Overlay** (fundo escuro, blur, logo pulsante).
- **HTML Padrão:**
  ```html
  <div id="loading-overlay" class="ftx-spin-overlay">
      <div class="ftx-spin-box">
          <img src="/images/logo_gota_frotix_transparente.png" class="ftx-loading-logo" />
          <div class="ftx-loading-text">Processando...</div>
      </div>
  </div>
  ```
- **Logo deve pulsar:** Classe `ftx-loading-logo` tem animação obrigatória.

## 3. Padrões de Código e Arquivos

### 3.1. Controllers e Actions
- **Authorize:** Não usar `[Authorize]` diretamente nos Endpoints de API (`[ApiController]`).
- **Nomenclatura:** PascalCase para C#, camelCase para JS.

### 3.2. CSS e Estilização
- **Global:** `wwwroot/css/frotix.css`.
- **Local:** No próprio `.cshtml` dentro de `<style>`.
- **Keyframes:** Em arquivos `.cshtml`, escapar o `@` (ex: `@@keyframes`).
- **Tooltips:** Classe obrigatória `tooltip-ftx-azul` (Fundo azul petróleo, sem setas, bordas arredondadas).

### 3.3. Botões
- **Header:** `btn btn-header-orange` (Laranja, borda preta).
- **Salvar/Criar:** `btn btn-azul btn-submit-spin` (Azul, ícone pulsa e vira spin ao clicar).
- **Cancelar:** `btn btn-ftx-fechar` (Vinho).
- **Voltar:** `btn btn-voltar` (Marrom).

## 4. Fluxo de Trabalho e Git

### 4.1. Controle de Versão
- **Branch:** Preferência por `main`.
- **Commits:**
  - Automáticos após alterações significativas.
  - Mensagens descritivas.
  - Se corrigir um erro próprio, explicar "Erro: X, Correção: Y" no commit.

### 4.2. Documentação (Processo Crítico)
- **Local:** Pasta `Documentacao/`.
- **Formato Duplo:** Para cada funcionalidade, manter **DOIS** arquivos sincronizados:
  1. `.md` (Técnico/Dev).
  2. `.html` (Portfólio/Visual - Formato A4).
- **Updates:** Ao alterar código, atualizar a documentação correspondente e adicionar entrada no log de modificações do arquivo.

### 4.3. Logs de Conversa
- **Pasta:** `Conversas/`.
- **Arquivo:** Criar/Atualizar arquivo `.md` para cada sessão (ex: `2026.01.13 - Refatoração Login.md`).
- **Conteúdo:** Resumo executivo, arquivos alterados, decisões técnicas.

## 5. Comandos Úteis
- **Build:** `dotnet build`
- **Start:** `dotnet run` ou `npm start` (verificar scripts).
- **Correção Nulos:** `.\fix_cs8618.ps1` (Script PowerShell para corrigir warnings CS8618).
- **Inject Try-Catch:** `inject-trycatch.bat` (Automatiza inserção de tratamento de erros).

---
*Este arquivo deve ser lido pelo Agente Gemini no início de interações para garantir conformidade com o ecossistema FrotiX.*
