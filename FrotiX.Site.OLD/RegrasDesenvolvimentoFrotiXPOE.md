# 📘 Regras de Desenvolvimento FrotiX – POE

> **Projeto:** FrotiX 2026 – FrotiX.Site  
> **Tipo:** Aplicação Web ASP.NET Core MVC – Gestão de Frotas  
> **Stack:** .NET 10, C#, Entity Framework Core, SQL Server, Bootstrap 5.3, jQuery, Syncfusion EJ2, Telerik UI  
> **Status:** ✅ Arquivo ÚNICO e OFICIAL de regras do projeto  

---

## 🔰 0. COMO ESTE ARQUIVO DEVE SER USADO (LEIA PRIMEIRO)

Este arquivo é a **ÚNICA FONTE DE VERDADE** para regras técnicas, padrões, fluxo de trabalho e comportamento esperado de **desenvolvedores e agentes de IA** no projeto FrotiX.

### ✅ Regras fundamentais
- Este arquivo **substitui integralmente** qualquer outro arquivo de regras.
- Arquivos `README.md`, `GEMINI.md` e `CLAUDE.md` **não contêm regras**, apenas redirecionam para este.
- Em caso de conflito de interpretação:  
  👉 **este arquivo sempre vence**.
- Nenhum código deve ser escrito sem respeitar este documento.

---

## 🧠 1. VISÃO GERAL DO PROJETO FROTIX

### 1.1 Objetivo
O FrotiX é uma solução corporativa de **Gestão de Frotas**, cobrindo:

✅ Veículos  
✅ Motoristas  
✅ Viagens  
✅ Abastecimentos  
✅ Manutenções  
✅ Multas  
✅ Estatísticas operacionais e financeiras  

### 1.2 Filosofia do Projeto
- Código defensivo
- Regras explícitas
- Banco de dados como fonte da verdade
- UX consistente
- Documentação obrigatória
- Rastreabilidade total

---

## 🚨 2. REGRAS INVIOLÁVEIS (ZERO TOLERANCE)

### 2.1 TRY-CATCH (OBRIGATÓRIO)

#### ✅ C#
```csharp
public IActionResult MinhaAction()
{
    try
    {
        // código
    }
    catch (Exception error)
    {
        Alerta.TratamentoErroComLinha("MeuController.cs", "MinhaAction", error);
        return Json(new { success = false, message = error.Message });
    }
}

✅ JavaScript
javascript
function minhaFuncao() {
    try {
        // código
    } catch (erro) {
        Alerta.TratamentoErroComLinha("arquivo.js", "minhaFuncao", erro);
    }
}

📌 NUNCA criar função sem try-catch.

2.2 ALERTAS E UX (SweetAlert FrotiX)
❌ PROIBIDO:

alert()
confirm()
prompt()
✅ OBRIGATÓRIO:

javascript
Alerta.Sucesso(titulo, msg)
Alerta.Erro(titulo, msg)
Alerta.Warning(titulo, msg)
Alerta.Info(titulo, msg)
Alerta.Confirmar(titulo, msg, btnSim, btnNao).then(ok => { ... })

2.3 ÍCONES (FontAwesome DUOTONE)
✅ SEMPRE:

html
<i class="fa-duotone fa-car"
   style="--fa-primary-color:#ff6b35; --fa-secondary-color:#6c757d;"></i>

❌ NUNCA:

fa-solid
fa-regular
fa-light
fa-thin
fa-brands
📌 Ícones fora do padrão devem ser convertidos automaticamente.

2.4 LOADING OVERLAY (OBRIGATÓRIO)
✅ Sempre usar overlay fullscreen com logo pulsante:

html
<div class="ftx-spin-overlay">
    <div class="ftx-spin-box">
        <img src="/images/logo_gota_frotix_transparente.png" class="ftx-loading-logo" />
        <div class="ftx-loading-text">Processando...</div>
    </div>
</div>

❌ Proibido:

Spinner Bootstrap
fa-spinner
loading inline
🧱 3. BANCO DE DADOS – FONTE DA VERDADE
3.1 FrotiX.txt
FrotiX.txt representa a estrutura REAL do banco SQL Server
Foi gerado a partir do banco
É a baseline oficial
📌 O banco manda. O código se adapta.

3.2 Regra Model ↔ Banco
Antes de escrever código que manipule dados:

Conferir tabela
Conferir colunas
Conferir tipos
Conferir constraints
Conferir relacionamentos
❌ Nunca assumir estrutura “de cabeça”.

3.3 Alterações de Banco / Modelos (FLUXO OBRIGATÓRIO)
Sempre que um Model:

for criado
for alterado
tiver campo adicionado/removido
✅ O agente DEVE entregar:

1️⃣ Script SQL
2️⃣ Explicação de impacto
3️⃣ Diff mental (antes/depois)

Exemplo:
sql
ALTER TABLE dbo.Veiculo
ADD ConsumoNormalizado DECIMAL(10,2) NULL;

Impacto:

Novo campo para métricas normalizadas
Nenhuma quebra de compatibilidade
Antes: campo inexistente
Depois: campo disponível

📌 Após aprovação:

Atualizar FrotiX.txt
Só então ajustar código
🧩 4. PADRÕES DE CÓDIGO
4.1 Controllers / APIs
❌ Nunca usar [Authorize] em [ApiController]
4.2 CSS
Global: wwwroot/css/frotix.css
Local: <style> no .cshtml
@keyframes em Razor → @@keyframes
4.3 Tooltips
✅ Sempre usar:

html
data-bs-custom-class="tooltip-ftx-azul"

🔄 5. FLUXO DE TRABALHO
5.1 Git
Branch preferencial: main
Commit automático após criação/alteração
Commit apenas dos arquivos da sessão atual
Correção de erro próprio → explicar erro + correção no commit
5.2 Documentação (CRÍTICO)
📁 Pasta: Documentacao/

Para cada funcionalidade:

.md (técnico)
.html (portfólio A4)
Sempre manter:

versão
data
log de modificações
5.3 Logs de Conversa
📁 Pasta: Conversas/

Um .md por sessão
Criado no início
Atualizado durante
Encerrado com resumo executivo
🤖 6. COMPORTAMENTO DOS AGENTS
Antes de escrever código:

Ler este arquivo
Conferir FrotiX.txt se houver banco
Ao detectar divergência:

Avisar no chat
Não corrigir silenciosamente
Ao alterar banco:

Script SQL
Impacto
Diff mental
Atualizar FrotiX.txt
🗂️ 7. VERSIONAMENTO DESTE ARQUIVO
Formato:

Versão X.Y

X = mudança estrutural
Y = ajustes incrementais
Exemplos:

1.0 → consolidação inicial
1.1 → ajustes
2.0 → mudança de fluxo
📌 Recomenda-se registrar mudanças no topo do arquivo quando evoluir.

✅ FIM DO DOCUMENTO
